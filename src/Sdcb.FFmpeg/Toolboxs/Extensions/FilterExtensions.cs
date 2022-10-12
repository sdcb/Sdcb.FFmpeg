using Sdcb.FFmpeg.Codecs;
using Sdcb.FFmpeg.Filters;
using Sdcb.FFmpeg.Formats;
using Sdcb.FFmpeg.Raw;
using Sdcb.FFmpeg.Utils;
using System;
using System.Collections.Generic;

namespace Sdcb.FFmpeg.Toolboxs;

public record VideoFilterContext(FilterGraph FilterGraph, FilterContext SourceContext, FilterContext SinkContext) : CommonFilterContext(FilterGraph, SourceContext, SinkContext)
{
    public static VideoFilterContext Create(MediaStream sourceStream, string filterText, AVPixelFormat sinkPixelFormat = AVPixelFormat.None)
    {
        FilterGraph graph = new();
        MediaDictionary dict = GetVideoSrcMetadata(sourceStream);
        (FilterContext srcCtx, FilterContext sinkCtx) = ConfigureVideo(graph, filterText, dict, sinkPixelFormat);
        return new VideoFilterContext(graph, srcCtx, sinkCtx);
    }

    public static VideoFilterContext Create(Frame videoFirstFrame, AVRational timebase, string filterText, AVPixelFormat sinkPixelFormat = AVPixelFormat.None)
    {
        FilterGraph graph = new();
        MediaDictionary dict = GetVideoSrcMetadata(videoFirstFrame, timebase);
        (FilterContext srcCtx, FilterContext sinkCtx) = ConfigureVideo(graph, filterText, dict, sinkPixelFormat);
        return new VideoFilterContext(graph, srcCtx, sinkCtx);
    }

    public VideoSinkParams SinkParams
    {
        get
        {
            FilterLink sinkInput = SinkContext.Inputs[0];
            return new VideoSinkParams(
                Width: sinkInput.W,
                Height: sinkInput.H,
                Timebase: sinkInput.TimeBase,
                Framerate: sinkInput.FrameRate,
                PixelFormat: (AVPixelFormat)sinkInput.Format);
        }
    }

    internal static (FilterContext srcCtx, FilterContext sinkCtx) ConfigureVideo(
        FilterGraph graph, string filterText, MediaDictionary bufferSrcDesc, AVPixelFormat sinkPixelFormat = AVPixelFormat.None)
    {
        FilterContext srcCtx = graph.CreateFilter("buffer", "in", bufferSrcDesc);
        FilterContext sinkCtx = graph.CreateFilter("buffersink", "out");
        if (sinkPixelFormat != AVPixelFormat.None)
        {
            sinkCtx.Options.Set("pix_fmts", new int[] { (int)sinkPixelFormat }, AV_OPT_SEARCH.Children);
        }
        graph.ParsePtr(filterText, new FilterInOut("out", sinkCtx), new FilterInOut("in", srcCtx));
        graph.Configure();
        return (srcCtx, sinkCtx);
    }

    internal static MediaDictionary GetVideoSrcMetadata(MediaStream sourceStream)
    {
        if (sourceStream.Codecpar == null)
        {
            throw new ArgumentNullException($"{nameof(sourceStream)}.{nameof(sourceStream.Codecpar)} should not be null.");
        }
        CodecParameters codecpar = sourceStream.Codecpar;

        if (codecpar.Width == 0)
        {
            throw new InvalidOperationException($"{nameof(sourceStream)} is not a video.");
        }

        return new MediaDictionary
        {
            ["width"] = codecpar.Width.ToString(),
            ["height"] = codecpar.Height.ToString(),
            ["pix_fmt"] = NameUtils.GetPixelFormatName((AVPixelFormat)codecpar.Format),
            ["time_base"] = sourceStream.TimeBase.ToString(),
            ["frame_rate"] = sourceStream.RFrameRate.ToString(),
            ["sar"] = codecpar.SampleAspectRatio.ToString(),
        };
    }

    internal static MediaDictionary GetVideoSrcMetadata(Frame videoFirstFrame, AVRational timebase)
    {
        if (videoFirstFrame.Width == 0)
        {
            throw new InvalidOperationException($"{nameof(videoFirstFrame)} is not a video frame.");
        }

        return new MediaDictionary
        {
            ["width"] = videoFirstFrame.Width.ToString(),
            ["height"] = videoFirstFrame.Height.ToString(),
            ["pix_fmt"] = NameUtils.GetPixelFormatName((AVPixelFormat)videoFirstFrame.Format),
            ["time_base"] = timebase.ToString(),
            ["sar"] = videoFirstFrame.SampleAspectRatio.ToString(),
        };
    }

    public override void ConfigureEncoder(CodecContext c)
    {
        VideoSinkParams p = SinkParams;
        c.Width = p.Width;
        c.Height = p.Height;
        c.TimeBase = p.Timebase;
        c.Framerate = p.Framerate;
        c.PixelFormat = p.PixelFormat;
    }
}

public record AudioFilterContext(FilterGraph FilterGraph, FilterContext SourceContext, FilterContext SinkContext) : CommonFilterContext(FilterGraph, SourceContext, SinkContext)
{
    public static AudioFilterContext Create(MediaStream sourceStream, string filterText, AudioSinkParams? sinkParams = null)
    {
        FilterGraph graph = new();
        MediaDictionary dict = GetAudioSrcMetadata(sourceStream);
        (FilterContext srcCtx, FilterContext sinkCtx) = ConfigureAudio(graph, filterText, dict, sinkParams);
        return new AudioFilterContext(graph, srcCtx, sinkCtx);
    }

    public static AudioFilterContext Create(Frame audioFirstFrame, string filterText, AudioSinkParams? sinkParams = null)
    {
        FilterGraph graph = new();
        MediaDictionary dict = GetAudioSrcMetadata(audioFirstFrame);
        (FilterContext srcCtx, FilterContext sinkCtx) = ConfigureAudio(graph, filterText, dict, sinkParams);
        return new AudioFilterContext(graph, srcCtx, sinkCtx);
    }

    public AudioSinkParams SinkParams
    {
        get
        {
            FilterLink sinkInput = SinkContext.Inputs[0];
            return new AudioSinkParams(
                SampleRate: sinkInput.SampleRate,
                SampleFormat: (AVSampleFormat)sinkInput.Format,
                ChannelLayout: sinkInput.ChannelLayout,
                Channels: sinkInput.Channels);
        }
    }

    internal static (FilterContext srcCtx, FilterContext sinkCtx) ConfigureAudio(FilterGraph graph, string filterText, MediaDictionary bufferSrcDesc, AudioSinkParams? paras = null)
    {
        FilterContext srcCtx = graph.CreateFilter("abuffer", "in", bufferSrcDesc);
        FilterContext sinkCtx = graph.CreateFilter("abuffersink", "out");
        if (paras != null)
        {
            if (paras.SampleFormat != AVSampleFormat.None)
            {
                sinkCtx.Options.Set("sample_fmts", new int[] { (int)paras.SampleFormat }, AV_OPT_SEARCH.Children);
            }
            if (paras.SampleRate != -1)
            {
                sinkCtx.Options.Set("sample_rates", new int[] { paras.SampleRate }, AV_OPT_SEARCH.Children);
            }
            if (paras.Channels != -1)
            {
                sinkCtx.Options.Set("channel_counts", new int[] { paras.Channels }, AV_OPT_SEARCH.Children);
            }
            if (paras.ChannelLayout != unchecked((ulong)-1))
            {
                sinkCtx.Options.Set("channel_layouts", new ulong[] { paras.ChannelLayout }, AV_OPT_SEARCH.Children);
            }
        }

        graph.ParsePtr(filterText, new FilterInOut("out", sinkCtx), new FilterInOut("in", srcCtx));
        graph.Configure();
        return (srcCtx, sinkCtx);
    }

    internal static MediaDictionary GetAudioSrcMetadata(MediaStream sourceStream)
    {
        if (sourceStream.Codecpar == null)
        {
            throw new ArgumentNullException($"{nameof(sourceStream)}.{nameof(sourceStream.Codecpar)} should not be null.");
        }
        CodecParameters codecpar = sourceStream.Codecpar;

        if (codecpar.SampleRate == 0)
        {
            throw new InvalidOperationException($"{nameof(sourceStream)} is not a audio.");
        }

        return new MediaDictionary
        {
            ["time_base"] = sourceStream.TimeBase.ToString(),
            ["sample_rate"] = codecpar.SampleRate.ToString(),
            ["sample_fmt"] = NameUtils.GetSampleFormatName((AVSampleFormat)codecpar.Format),
            ["channel_layout"] = NameUtils.GetChannelLayoutString(codecpar.ChannelLayout, codecpar.Channels),
            ["channels"] = codecpar.Channels.ToString(),
        };
    }

    internal static MediaDictionary GetAudioSrcMetadata(Frame audioFirstFrame)
    {
        if (audioFirstFrame.Width == 0)
        {
            throw new InvalidOperationException($"{nameof(audioFirstFrame)} is not a audio frame.");
        }

        return new MediaDictionary
        {
            ["time_base"] = new AVRational(1, audioFirstFrame.SampleRate).ToString(),
            ["sample_rate"] = audioFirstFrame.SampleRate.ToString(),
            ["sample_fmt"] = NameUtils.GetSampleFormatName((AVSampleFormat)audioFirstFrame.Format),
            ["channel_layout"] = NameUtils.GetChannelLayoutString(audioFirstFrame.ChannelLayout, audioFirstFrame.Channels),
            ["channels"] = audioFirstFrame.Channels.ToString(),
        };
    }

    public override void ConfigureEncoder(CodecContext c)
    {
        AudioSinkParams p = SinkParams;
        c.SampleRate = p.SampleRate;
        c.SampleFormat = p.SampleFormat;
        c.ChannelLayout = p.ChannelLayout;
        c.Channels = p.Channels;
        c.TimeBase = new AVRational(1, p.SampleRate);
    }
}

public abstract record CommonFilterContext(FilterGraph FilterGraph, FilterContext SourceContext, FilterContext SinkContext) : IDisposable
{
    public IEnumerable<Frame> WriteFrame(Frame destFrame, Frame? srcFrame)
    {
        SourceContext.WriteFrame(srcFrame);

        while (true)
        {
            CodecResult r = CodecContext.ToCodecResult(SinkContext.GetFrame(destFrame));
            if (r == CodecResult.Again || r == CodecResult.EOF) break;

            if (r == CodecResult.Success)
            {
                yield return destFrame;
                destFrame.Unreference();
            }
        }
    }

    public abstract void ConfigureEncoder(CodecContext c);

    public void Dispose()
    {
        SinkContext.Dispose();
        SourceContext.Dispose();
        FilterGraph.Dispose();
    }
}

public record AudioSinkParams(int SampleRate = -1, AVSampleFormat SampleFormat = AVSampleFormat.None, ulong ChannelLayout = unchecked((ulong)-1), int Channels = -1)
{
}

public record VideoSinkParams(int Width, int Height, AVRational Timebase, AVRational Framerate, AVPixelFormat PixelFormat)
{
}