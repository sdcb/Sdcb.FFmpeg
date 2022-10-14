using Sdcb.FFmpeg.Codecs;
using Sdcb.FFmpeg.Formats;
using Sdcb.FFmpeg.Raw;
using Sdcb.FFmpeg.Swscales;
using Sdcb.FFmpeg.Toolboxs;
using Sdcb.FFmpeg.Toolboxs.Extensions;
using Sdcb.FFmpeg.Toolboxs.Generators;
using Sdcb.FFmpeg.Utils;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
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
        using Frame frame = Frame.CreateVideo(800, 600, AVPixelFormat.Yuv420p);
        VideoFrameGenerator.FillYuv420p(frame, 0);
        byte[] pngData = frame.EncodeToBytes(formatName: "apng");
        //File.WriteAllBytes("test.png", pngData);
        Assert.NotEmpty(pngData);
    }

    private byte[] MakeMp4(Codec codec, int width, int height, int frameCount = 30)
    {
        using FormatContext fc = FormatContext.AllocOutput(formatName: "mp4");
        fc.VideoCodec = codec;
        MediaStream vstream = fc.NewStream(fc.VideoCodec);
        using CodecContext vcodec = new CodecContext(fc.VideoCodec)
        {
            Width = width,
            Height = height,
            TimeBase = new AVRational(1, 15),
            PixelFormat = AVPixelFormat.Yuv420p,
            Flags = AV_CODEC_FLAG.GlobalHeader,
        };
        vcodec.Open(fc.VideoCodec, new MediaDictionary
        {
            ["preset"] = "ultrafast"
        });
        vstream.Codecpar!.CopyFrom(vcodec);

        using DynamicIOContext io = IOContext.OpenDynamic();
        fc.Pb = io;
        fc.WriteHeader();
        foreach (Packet packet in VideoFrameGenerator.Yuv420pSequence(vcodec.Width, vcodec.Height, frameCount)
            .EncodeFrames(vcodec))
        {
            try
            {
                packet.RescaleTimestamp(vcodec.TimeBase, vstream.TimeBase);
                packet.StreamIndex = vstream.Index;
                fc.InterleavedWritePacket(packet);
            }
            finally
            {
                packet.Unref();
            }
        }
        fc.WriteTrailer();
        return io.GetBuffer().ToArray();
    }

    [Fact]
    public void CreateMp4()
    {
        FFmpegLogger.LogWriter = (level, msg) => _console.WriteLine(msg?.Trim());
        byte[] mp4 = MakeMp4(Codec.CommonEncoders.Libx264, width: 320, height: 240);
        Assert.NotEmpty(mp4);
        _console.WriteLine($"mp4 size: {mp4.Length}");
    }

    [Fact]
    public void DecodeMp4()
    {
        FFmpegLogger.LogWriter = (level, msg) => { };
        byte[] mp4 = MakeMp4(Codec.CommonEncoders.Libx264, width: 320, height: 240);
        _console.WriteLine($"mp4 size: {mp4.Length}");

        FFmpegLogger.LogWriter = (level, msg) => _console.WriteLine(msg?.Trim());
        using IOContext io = IOContext.ReadStream(new MemoryStream(mp4));
        using FormatContext fc = FormatContext.OpenInputIO(io);
        MediaStream videoStream = fc.GetVideoStream();

        using CodecContext videoDecoder = new CodecContext(Codec.FindDecoderById(videoStream.Codecpar!.CodecId));
        videoDecoder.FillParameters(videoStream.Codecpar);
        videoDecoder.Open();

        using VideoFrameConverter sws = new();
        using Frame dest = Frame.CreateVideo(videoStream.Codecpar.Width, videoStream.Codecpar.Height, AVPixelFormat.Rgb0);
        foreach (Frame frame in fc.ReadPackets()
            .DecodePackets(videoDecoder))
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
        vstream.Codecpar!.CopyFrom(vcodec);

        using DynamicIOContext io = IOContext.OpenDynamic();
        fc.Pb = io;
        fc.WriteHeader();
        foreach (Packet packet in VideoFrameGenerator.Yuv420pSequence(vcodec.Width, vcodec.Height, 40)
            .ConvertFrames(vcodec)
            .EncodeFrames(vcodec))
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
                packet.Unref();
            }
        }
        fc.WriteTrailer();
        byte[] gif = io.GetBuffer().ToArray();
        Assert.NotEmpty(gif);
        //File.WriteAllBytes("test.gif", gif);
    }

    [Fact]
    public void MakeGifWithFilter()
    {
        using FormatContext fc = FormatContext.AllocOutput(formatName: "gif");
        fc.VideoCodec = Codec.CommonEncoders.Gif;
        MediaStream vstream = fc.NewStream(fc.VideoCodec);
        using CodecContext vcodec = new CodecContext(fc.VideoCodec)
        {
            Width = 150,
            Height = 100,
            TimeBase = new AVRational(1, 10),
            PixelFormat = AVPixelFormat.Pal8,
        };
        vcodec.Open(fc.VideoCodec);
        vstream.Codecpar!.CopyFrom(vcodec);

        using DynamicIOContext io = IOContext.OpenDynamic();
        fc.Pb = io;
        fc.WriteHeader();
        string filter = $"fps={vcodec.TimeBase.Inverse().ToDouble()},scale={vcodec.Width}:{vcodec.Height}:flags=lanczos,split[s0][s1];[s0]palettegen[p];[s1][p]paletteuse";
        VideoFrameGenerator.Yuv420pSequence(150, 100, 30)
            .ApplyVideoFilters(new AVRational(1, 30), vcodec.PixelFormat, filter)
            .EncodeAllFrames(fc, videoEncoder: vcodec)
            .WriteAll(fc);
        fc.WriteTrailer();
        byte[] gif = io.GetBuffer().ToArray();
        Assert.NotEmpty(gif);
        //File.WriteAllBytes("test.gif", gif);
    }

    [Fact]
    public void RemuxMp4WithFilter()
    {
        FFmpegLogger.LogWriter = (level, msg) => { };
        byte[] mp4 = MakeMp4(Codec.CommonEncoders.Libx264, width: 320, height: 240, frameCount: 30);
        _console.WriteLine($"mp4 size: {mp4.Length}");

        //FFmpegLogger.LogWriter = (level, msg) => _console.WriteLine(msg?.Trim());
        using IOContext input = IOContext.ReadStream(new MemoryStream(mp4));
        using FormatContext inFc = FormatContext.OpenInputIO(input);
        inFc.LoadStreamInfo();
        MediaStream inVideoStream = inFc.GetVideoStream();

        using CodecContext videoDecoder = new CodecContext(Codec.FindDecoderById(inVideoStream.Codecpar!.CodecId));
        videoDecoder.FillParameters(inVideoStream.Codecpar);
        videoDecoder.Open();

        using VideoFilterContext filter = VideoFilterContext.Create(inVideoStream, "scale=480:-1");

        using FormatContext outFc = FormatContext.AllocOutput(formatName: "mp4");
        outFc.VideoCodec = Codec.CommonEncoders.Libx264;
        MediaStream outVideoStream = outFc.NewStream(outFc.VideoCodec);
        using CodecContext videoEncoder = new CodecContext(outFc.VideoCodec)
        {
            Flags = AV_CODEC_FLAG.GlobalHeader,
        };
        filter.ConfigureEncoder(videoEncoder);
        videoEncoder.Open(inFc.VideoCodec, new MediaDictionary
        {
            ["preset"] = "ultrafast"
        });
        outVideoStream.Codecpar!.CopyFrom(videoEncoder);

        using DynamicIOContext io = IOContext.OpenDynamic();
        outFc.Pb = io;
        outFc.WriteHeader();
        inFc.ReadPackets()
            .DecodePackets(videoDecoder)
            .ApplyVideoFilters(filter)
            .EncodeAllFrames(outFc, null, videoEncoder)
            .WriteAll(outFc);
        outFc.WriteTrailer();
        byte[] remuxMp4 = io.GetBuffer().ToArray();
        Assert.NotEmpty(remuxMp4);
        //File.WriteAllBytes("test.mp4", remuxMp4);
    }
}
