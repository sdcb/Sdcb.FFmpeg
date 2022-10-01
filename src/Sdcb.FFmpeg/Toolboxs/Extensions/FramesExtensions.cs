using Sdcb.FFmpeg.Codecs;
using Sdcb.FFmpeg.Filters;
using Sdcb.FFmpeg.Raw;
using Sdcb.FFmpeg.Swresamples;
using Sdcb.FFmpeg.Swscales;
using Sdcb.FFmpeg.Utils;
using System.Collections.Generic;
using static Sdcb.FFmpeg.Raw.ffmpeg;

namespace Sdcb.FFmpeg.Toolboxs.Extensions;

public static class FramesExtensions
{
    /// <summary>
    /// <para>For video, convert frame pixel format, width, height into the same as CodecContext</para>
    /// <para>For audio, convert frame sames format into the same as CodecContext</para>
    /// <see cref="sws_getCachedContext(SwsContext*, int, int, AVPixelFormat, int, int, AVPixelFormat, int, SwsFilter*, SwsFilter*, double*)"/>
    /// <see cref="sws_scale(SwsContext*, byte*[], int[], int, int, byte*[], int[])"/>
    /// </summary>
    public static IEnumerable<Frame> ConvertFrames(this IEnumerable<Frame> sourceFrames, CodecContext c, SWS swsFlags = SWS.Bilinear)
    {
        using var destFrame = c.CreateFrame();
        int pts = 0;
        if (c.Codec.Type == AVMediaType.Video)
        {
            using var frameConverter = new VideoFrameConverter();
            foreach (var sourceFrame in sourceFrames)
            {
                frameConverter.ConvertFrame(sourceFrame, destFrame, swsFlags);
                destFrame.Pts = pts++;
                yield return destFrame;
            }
        }
        else if (c.Codec.Type == AVMediaType.Audio)
        {
            using var frameConverter = new SampleConverter();
            foreach (var sourceFrame in sourceFrames)
            {
                frameConverter.ConvertFrame(destFrame, sourceFrame);
                destFrame.Pts = pts += c.FrameSize;
                yield return destFrame;
            }
        }
    }

    public static IEnumerable<Frame> ApplyVideoFilters(this IEnumerable<Frame> srcFrames, AVRational srcTimebase, AVPixelFormat destPixelFormat, string filterText)
    {
        using FilterGraph graph = new();
        using FilterContext srcCtx = graph.AllocFilter("buffer", "in");
        using FilterContext sinkCtx = graph.CreateFilter("buffersink", "out");
        bool initialized = false;

        using Frame destFrame = new();
        foreach (Frame srcFrame in srcFrames)
        {
            if (!initialized)
            {
                if (srcFrame.Width > 0)
                {
                    // video
                    srcCtx.InitializeFromDictionary(new MediaDictionary
                    {
                        ["width"] = srcFrame.Width.ToString(),
                        ["height"] = srcFrame.Height.ToString(),
                        ["pix_fmt"] = NameUtils.GetPixelFormatName((AVPixelFormat)srcFrame.Format),
                        ["time_base"] = srcTimebase.ToString(),
                        ["sar"] = srcFrame.SampleAspectRatio.ToString(),
                    });                    
                }
                else
                {
                    // audio
                    srcCtx.InitializeFromDictionary(new MediaDictionary
                    {
                        ["time_base"] = srcTimebase.ToString(),
                        ["sample_rate"] = srcFrame.SampleRate.ToString(),
                        ["sample_fmt"] = NameUtils.GetSampleFormatName((AVSampleFormat)srcFrame.Format),
                        ["channel_layout"] = NameUtils.GetChannelLayoutString(srcFrame.ChannelLayout, srcFrame.Channels),
                        ["channels"] = srcFrame.Channels.ToString(), 
                    });
                }
                sinkCtx.Options.Set("pix_fmts", new int[] { (int)destPixelFormat }, AV_OPT_SEARCH.Children);
                graph.ParsePtr(filterText, new FilterInOut("out", sinkCtx), new FilterInOut("in", srcCtx));
                graph.Configure();
                initialized = true;
            }

            srcCtx.WriteFrame(srcFrame);

            while (true)
            {
                CodecResult r = CodecContext.ToCodecResult(sinkCtx.GetFrame(destFrame));
                if (r == CodecResult.Again || r == CodecResult.EOF) break;

                if (r == CodecResult.Success)
                {
                    yield return destFrame;
                    destFrame.Unreference();
                }
            }
        }

        srcCtx.WriteFrame(null);
        while (true)
        {
            CodecResult r = CodecContext.ToCodecResult(sinkCtx.GetFrame(destFrame));
            if (r == CodecResult.Again || r == CodecResult.EOF) break;
            if (r == CodecResult.Success)
            {
                yield return destFrame;
                destFrame.Unreference();
            }
        }
    }
}
