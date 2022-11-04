using Sdcb.FFmpeg.Codecs;
using Sdcb.FFmpeg.Filters;
using Sdcb.FFmpeg.Formats;
using Sdcb.FFmpeg.Raw;
using Sdcb.FFmpeg.Utils;
using System;

namespace Sdcb.FFmpeg.Toolboxs.FilterTools;

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
                ChLayout: sinkInput.ChLayout);
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
            if (paras.ChLayout.order != AVChannelOrder.Unspec && paras.ChLayout.nb_channels != 0)
            {
                sinkCtx.Options.Set("ch_layouts", paras.ChLayout.Describe(), AV_OPT_SEARCH.Children);
            }
        }

        graph.ParsePtr(filterText, new FilterInOut("out", sinkCtx), new FilterInOut("in", srcCtx));
        graph.Configure();
        return (srcCtx, sinkCtx);
    }

    internal unsafe static MediaDictionary GetAudioSrcMetadata(MediaStream sourceStream)
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
        if (codecpar.ChLayout.order == AVChannelOrder.Unspec && codecpar.ChLayout.nb_channels != 0)
        {
            AVChannelLayout chLayout;
            ffmpeg.av_channel_layout_default(&chLayout, codecpar.ChLayout.nb_channels);
            codecpar.ChLayout = chLayout;
        }

        return new MediaDictionary
        {
            ["time_base"] = sourceStream.TimeBase.ToString(),
            ["sample_rate"] = codecpar.SampleRate.ToString(),
            ["sample_fmt"] = NameUtils.GetSampleFormatName((AVSampleFormat)codecpar.Format),
            ["channel_layout"] = codecpar.ChLayout.Describe(),
            ["channels"] = codecpar.ChLayout.nb_channels.ToString(),
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
            ["channel_layout"] = audioFirstFrame.ChLayout.Describe(),
            ["channels"] = audioFirstFrame.ChLayout.nb_channels.ToString(),
        };
    }

    public override void ConfigureEncoder(CodecContext c)
    {
        AudioSinkParams p = SinkParams;
        c.SampleRate = p.SampleRate;
        c.SampleFormat = p.SampleFormat;
        c.ChLayout = p.ChLayout;
        c.TimeBase = new AVRational(1, p.SampleRate);
    }
}
