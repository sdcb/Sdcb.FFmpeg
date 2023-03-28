using Sdcb.FFmpeg.Codecs;
using Sdcb.FFmpeg.Formats;
using Sdcb.FFmpeg.Raw;
using Sdcb.FFmpeg.Swscales;
using Sdcb.FFmpeg.Tests.Filters;
using Sdcb.FFmpeg.Toolboxs.Extensions;
using Sdcb.FFmpeg.Toolboxs.FilterTools;
using Sdcb.FFmpeg.Toolboxs.Generators;
using Sdcb.FFmpeg.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Unicode;
using System.Threading.Tasks;

using Xunit;
using Xunit.Abstractions;

namespace Sdcb.FFmpeg.Tests;

public class Examples : IDisposable
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

    [Theory]
    //[InlineData(@"D:\华智信\项目\039\测试音视频\港片MV0.H264",
    //    @"D:\华智信\项目\039\测试音视频\港片MV0.H264",
    //    @"D:\华智信\项目\039\测试音视频\港片MV0.H264",
    //    @"D:\\项目\\GitHubSource\\CG.Fmcode\\MultiUnitTest\\Source\输出.mp4")]
    [InlineData(@"D:\华智信\项目\039\HZXDATA.Client\HZXDATA.Client\Sources\彩虹的微笑.mp4",
        @"D:\华智信\项目\039\HZXDATA.Client\HZXDATA.Client\Sources\彩虹的微笑.mp4",
        @"D:\华智信\项目\039\HZXDATA.Client\HZXDATA.Client\Sources\彩虹的微笑.mp4",
        @"D:\\项目\\GitHubSource\\CG.Fmcode\\MultiUnitTest\\Source\输出.mp4")]
    public async void AppositionMp4WithFilter(string mp4Path, string mp4Path2, string mp4Path3, string destFile)
    {
        FFmpegLogger.LogWriter = (a, b) =>
        _console.WriteLine(b);
        var context = CreateDecoderFrameQueue(mp4Path);
        var context1 = CreateDecoderFrameQueue(mp4Path2);
        var context2 = CreateDecoderFrameQueue(mp4Path3);


        AppositionFilter appositionFilter= AppositionFilter.AllocFilter(new System.Drawing.Size() { Width = 1024, Height = 768 }, new[]
        {
            new AppositionParams(context.inFc.GetVideoStream(),new System.Drawing.Rectangle(0, 0, 512, 384)),
            new AppositionParams(context1.inFc.GetVideoStream(),new System.Drawing.Rectangle(0, 385, 512, 384)),
            new AppositionParams(context2.inFc.GetVideoStream(),new System.Drawing.Rectangle(513, 256, 512, 384))
        });

        var outAudioFormat = new AudioSinkParams(GetChannelLayout(2), 48000, AVSampleFormat.Fltp);
        AmixFilter amixFilter = AmixFilter.AllocFilter(outAudioFormat, new[] 
        { 
            new AudioSinkParams(context.inFc.GetAudioStream().Codecpar!.ChLayout,
            context.inFc.GetAudioStream().Codecpar!.SampleRate,
            (AVSampleFormat)context.inFc.GetAudioStream().Codecpar!.Format),

            new AudioSinkParams(context1.inFc.GetAudioStream().Codecpar!.ChLayout,
            context1.inFc.GetAudioStream().Codecpar!.SampleRate,
            (AVSampleFormat)context1.inFc.GetAudioStream().Codecpar!.Format),

            new AudioSinkParams(context2.inFc.GetAudioStream().Codecpar!.ChLayout,
            context2.inFc.GetAudioStream().Codecpar!.SampleRate,
            (AVSampleFormat)context2.inFc.GetAudioStream().Codecpar!.Format),
        });

        using FormatContext outFc = FormatContext.AllocOutput(fileName: destFile);
        outFc.VideoCodec = Codec.CommonEncoders.Libx264;
        MediaStream outVideoStream = outFc.NewStream(outFc.VideoCodec);
        using CodecContext videoEncoder = new CodecContext(outFc.VideoCodec)
        {
            Width = 1024,
            Height = 768,
            TimeBase = new AVRational(1, 25),
            PixelFormat = AVPixelFormat.Yuv420p,
            Flags = AV_CODEC_FLAG.GlobalHeader,
            ThreadCount = Environment.ProcessorCount - 1,
        };
        videoEncoder.Open(outFc.VideoCodec, null);
        outVideoStream.Codecpar!.CopyFrom(videoEncoder);
        outVideoStream.TimeBase = videoEncoder.TimeBase;

        outFc.AudioCodec = Codec.CommonEncoders.AAC;
        MediaStream outAudioStream = outFc.NewStream(outFc.AudioCodec);
        using CodecContext audioEncoder = new(outFc.AudioCodec)
        {
            ChLayout = outAudioFormat.ChLayout,
            SampleFormat = outFc.AudioCodec.Value.NegociateSampleFormat(outAudioFormat.SampleFormat),
            SampleRate = outFc.AudioCodec.Value.NegociateSampleRates(outAudioFormat.SampleRate),
            BitRate = outAudioFormat.SampleRate
        };

        audioEncoder.TimeBase = new AVRational(1, audioEncoder.SampleRate);
        audioEncoder.Open(outFc.AudioCodec);
        outAudioStream.Codecpar!.CopyFrom(audioEncoder);

        // begin write
        IOContext io = IOContext.OpenWrite(destFile);
        outFc.Pb = io;
        outFc.WriteHeader();
        Task t = Task.Run(() =>
        {
            appositionFilter.WriteFrame(context.queue, context1.queue, context2.queue)
            .ApplyAudioFilters(amixFilter)
            .AudioFifo(audioEncoder)
            //.Select(s => s.Frame).Where(s => s != null && s.Width > 0)!
            .ConvertAllFrames(audioEncoder, videoEncoder)
            .ComputePtsDts(videoEncoder)
            .EncodeAllFrames(outFc, audioEncoder, videoEncoder)
            .WriteAll(outFc);
        });

        await Task.WhenAll(t);
        outFc.WriteTrailer();
        outFc.Flush();
        //outFc.Dispose();
    }
    private (MediaThreadQueue<Frame> queue,FormatContext inFc, CodecContext audioDecoder, CodecContext videoDecoder) CreateDecoderFrameQueue(string mp4Path)
    {
        FormatContext inFc = FormatContext.OpenInputUrl(mp4Path);
        inFc.LoadStreamInfo();

        // prepare input stream/codec
        MediaStream inAudioStream = inFc.GetAudioStream();
        CodecContext audioDecoder = new(Codec.FindDecoderById(inAudioStream.Codecpar!.CodecId));
        audioDecoder.FillParameters(inAudioStream.Codecpar);
        audioDecoder.Open();
        var a = audioDecoder.ChLayout;
        //audioDecoder.ChLayout = (ulong)ffmpeg.avge(audioDecoder.Channels);

        MediaStream inVideoStream = inFc.GetVideoStream();
        CodecContext videoDecoder = new(Codec.FindDecoderById(inVideoStream.Codecpar!.CodecId));
        videoDecoder.FillParameters(inVideoStream.Codecpar!);
        videoDecoder.Open();

        MediaThreadQueue<Frame> mediaThreadQueue = inFc
        .ReadPackets(inVideoStream.Index, inAudioStream.Index)
        .DecodeAllPackets(inFc,audioDecoder,videoDecoder)
        .ToThreadQueue();
        return (mediaThreadQueue,inFc, audioDecoder, videoDecoder);
    }
    private unsafe AVChannelLayout GetChannelLayout(int nb_channels)
    {
        AVChannelLayout layout = new AVChannelLayout();
        ffmpeg.av_channel_layout_default(&layout, nb_channels);
        return layout;
    }
    public void Dispose()
    {
        FFmpegLogger.LogWriter = null;
    }
}
