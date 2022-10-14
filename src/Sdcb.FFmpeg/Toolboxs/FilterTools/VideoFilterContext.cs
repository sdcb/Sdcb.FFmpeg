using Sdcb.FFmpeg.Codecs;
using Sdcb.FFmpeg.Filters;
using Sdcb.FFmpeg.Formats;
using Sdcb.FFmpeg.Raw;
using Sdcb.FFmpeg.Utils;
using System;

namespace Sdcb.FFmpeg.Toolboxs.FilterTools;

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
            ["time_base"] = sourceStream.RFrameRate.Inverse().ToString(),
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
            ["frame_rate"] = timebase.Inverse().ToString(),
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
