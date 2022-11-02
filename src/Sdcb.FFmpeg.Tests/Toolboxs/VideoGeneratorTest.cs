using Sdcb.FFmpeg.Codecs;
using Sdcb.FFmpeg.Formats;
using Sdcb.FFmpeg.Raw;
using Sdcb.FFmpeg.Toolboxs.Extensions;
using Sdcb.FFmpeg.Toolboxs.Generators;
using Sdcb.FFmpeg.Utils;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Sdcb.FFmpeg.Tests.Toolboxs;

public class VideoGeneratorTest
{
    private readonly ITestOutputHelper _console;

    public VideoGeneratorTest(ITestOutputHelper console)
    {
        _console = console;
    }

    [Fact]
    public void CreateMp4()
    {
        FFmpegLogger.LogWriter = (level, msg) => _console.WriteLine(msg?.Trim());
        using FormatContext fc = FormatContext.AllocOutput(formatName: "mp4");
        fc.VideoCodec = Codec.CommonEncoders.Libx264;
        MediaStream vstream = fc.NewStream(fc.VideoCodec);
        using CodecContext vcodec = new CodecContext(fc.VideoCodec)
        {
            Width = 320,
            Height = 240,
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
        {
            Stopwatch sw = Stopwatch.StartNew();
            Frame[] frames = VideoFrameGenerator.WritableYuv420pSequence(vcodec.Width, vcodec.Height, 30)
                .ToArray();
            _console.WriteLine($"generate sequence elapsed: {sw.ElapsedMilliseconds}ms.");

            sw.Restart();
            Packet[] packets = frames.EncodeAllFrames(fc, null, vcodec).CloneMakeWritable().ToArray();
            frames.DisposeAll();
            _console.WriteLine($"encode frames elapsed: {sw.ElapsedMilliseconds}ms");

            sw.Restart();
            packets.WriteAll(fc);
            packets.DisposeAll();
            _console.WriteLine($"write all packets elapsed: {sw.ElapsedMilliseconds}ms");
        }
        fc.WriteTrailer();
        byte[] mp4 = io.GetBuffer().ToArray();

        Assert.NotEmpty(mp4);
        File.WriteAllBytes("test.mp4", mp4);
        _console.WriteLine($"mp4 size: {mp4.Length}");
    }
}
