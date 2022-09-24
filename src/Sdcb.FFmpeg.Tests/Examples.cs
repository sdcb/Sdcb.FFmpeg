using Sdcb.FFmpeg.Codecs;
using Sdcb.FFmpeg.Formats;
using Sdcb.FFmpeg.Raw;
using Sdcb.FFmpeg.Swscales;
using Sdcb.FFmpeg.Toolboxs.Extensions;
using Sdcb.FFmpeg.Toolboxs.Generators;
using Sdcb.FFmpeg.Utils;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Sdcb.FFmpeg.Tests;

public class Examples
{
    private readonly ITestOutputHelper _console;

    public Examples(ITestOutputHelper console)
    {
        _console = console;
    }

    [Fact]
    public void CreatePng()
    {
        using Frame frame = Frame.CreateWritableVideo(800, 600, AVPixelFormat.Yuv420p);
        VideoFrameGenerator.FillYuv420p(frame, 0);
        byte[] pngData = frame.EncodeToBytes(formatName: "apng");
        //File.WriteAllBytes("test.png", pngData);
        Assert.NotEmpty(pngData);
    }

    private byte[] MakeMp4(Codec codec, int width, int height)
    {
        using FormatContext fc = FormatContext.AllocOutput(formatName: "mp4");
        fc.VideoCodec = codec;
        MediaStream vstream = fc.NewStream(fc.VideoCodec);
        using CodecContext vcodec = new CodecContext(fc.VideoCodec)
        {
            Width = width,
            Height = height,
            TimeBase = new AVRational(1, 30),
            PixelFormat = AVPixelFormat.Yuv420p,
            Flags = AV_CODEC_FLAG.GlobalHeader,
        };
        vcodec.Open(fc.VideoCodec, new MediaDictionary
        {
            ["preset"] = "ultrafast"
        });
        vstream.Codecpar.CopyFrom(vcodec);

        using DynamicIOContext io = IOContext.OpenDynamic();
        fc.Pb = io;
        fc.WriteHeader();
        foreach (Packet packet in vcodec.EncodeFrames(VideoFrameGenerator.Yuv420pSequence(vcodec.Width, vcodec.Height).Take(60)))
        {
            try
            {
                packet.RescaleTimestamp(vcodec.TimeBase, vstream.TimeBase);
                packet.StreamIndex = vstream.Index;
                fc.InterleavedWritePacket(packet);
            }
            finally
            {
                packet.Unreference();
            }
        }
        fc.WriteTrailer();
        return io.GetBuffer().ToArray();
    }

    [Fact]
    public void CreateMp4()
    {
        FFmpegLogger.LogWriter = (level, msg) => _console.WriteLine(msg.Trim());
        byte[] mp4 = MakeMp4(Codec.CommonEncoders.Libx264, width: 640, height: 480);
        Assert.NotEmpty(mp4);
        _console.WriteLine($"mp4 size: {mp4.Length}");
    }

    [Fact]
    public void DecodeMp4()
    {
        FFmpegLogger.LogWriter = (level, msg) => { };
        byte[] mp4 = MakeMp4(Codec.CommonEncoders.Libx264, width: 640, height: 480);
        _console.WriteLine($"mp4 size: {mp4.Length}");

        FFmpegLogger.LogWriter = (level, msg) => _console.WriteLine(msg.Trim());
        using IOContext io = IOContext.ReadStream(new MemoryStream(mp4));
        using FormatContext fc = FormatContext.OpenInputIO(io);
        MediaStream videoStream = fc.GetVideoStream();

        using CodecContext videoDecoder = new CodecContext(Codec.FindDecoderById(videoStream.Codecpar.CodecId));
        videoDecoder.FillParameters(videoStream.Codecpar);
        videoDecoder.Open();

        using VideoFrameConverter sws = new();
        using Frame dest = Frame.CreateWritableVideo(videoStream.Codecpar.Width, videoStream.Codecpar.Height, AVPixelFormat.Rgb0);
        foreach (Frame frame in videoDecoder.DecodePackets(fc.ReadPackets()))
        {
            Stopwatch sw = Stopwatch.StartNew();
            sws.ConvertFrame(frame, dest);
            sw.Stop();
            _console.WriteLine($"dts: {frame.PktDts}, pts: {frame.Pts}, cpn: {frame.CodedPictureNumber}, sws-elapse: {sw.Elapsed.TotalMilliseconds:F2}ms");
        }
    }

    [Fact]
    public void MakeGif()
    {
        using FormatContext fc = FormatContext.AllocOutput(formatName: "gif");
        fc.VideoCodec = Codec.CommonEncoders.Gif;
        MediaStream vstream = fc.NewStream(fc.VideoCodec);
        using CodecContext vcodec = new CodecContext(fc.VideoCodec)
        {
            Width = 400,
            Height = 50,
            TimeBase = new AVRational(1, 20),
            PixelFormat = AVPixelFormat.Rgb8,
        };
        vcodec.Open(fc.VideoCodec);
        vstream.Codecpar.CopyFrom(vcodec);

        using DynamicIOContext io = IOContext.OpenDynamic();
        fc.Pb = io;
        fc.WriteHeader();
        foreach (Packet packet in vcodec.EncodeFrames(vcodec.ConvertFrames(
            VideoFrameGenerator.Yuv420pSequence(vcodec.Width, vcodec.Height).Take(40)
            )))
        {
            try
            {
                packet.RescaleTimestamp(vcodec.TimeBase, vstream.TimeBase);
                long after = packet.Dts;
                packet.StreamIndex = vstream.Index;
                fc.InterleavedWritePacket(packet);
            }
            finally
            {
                packet.Unreference();
            }
        }
        fc.WriteTrailer();
        byte[] gif = io.GetBuffer().ToArray();
        Assert.NotEmpty(gif);
        //File.WriteAllBytes("test.gif", gif);
    }
}
