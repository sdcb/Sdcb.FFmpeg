using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;

using Sdcb.FFmpeg.Codecs;
using Sdcb.FFmpeg.Filters;
using Sdcb.FFmpeg.Formats;
using Sdcb.FFmpeg.Raw;
using Sdcb.FFmpeg.Toolboxs.Extensions;
using Sdcb.FFmpeg.Utils;

namespace Sdcb.FFmpeg.Toolboxs.FilterTools
{
    public record AppositionFilter
    {
        private VideoFilterContext videoFilterContext;
        //private FilterGraph filterGraph;
        //private FilterContext sinkCtx;
        public IReadOnlyList<VideoFilterContext> VideoTrack { get; } = new List<VideoFilterContext>();
        private AppositionFilter(Size outVideoSize)
        {
            FilterGraph graph = new();
            //filterGraph = graph;
            FilterContext nullfilter = graph.CreateFilter("nullsrc", "srcbase", $"duration=0.1:rate=25:size={outVideoSize.Width}x{outVideoSize.Height}"); //graph.CreateFilter("nullsrc", "srcbase");
            var sinkCtx = graph.CreateFilter("buffersink", "out");
            sinkCtx.Options.Set("pix_fmts", new int[] { (int)AVPixelFormat.Yuv420p }, AV_OPT_SEARCH.Children);
            videoFilterContext = new VideoFilterContext(graph, nullfilter, sinkCtx);
        }
        private VideoFilterContext CreateVideoTrackByStream(MediaStream mediaStream,Rectangle rect)
        {
            FilterContext scaleFilter,
            overlayFilter,
            buffersrc;//叠加流
            var dict = VideoFilterContext.GetVideoSrcMetadata(mediaStream);
            buffersrc = videoFilterContext.FilterGraph.CreateFilter("buffer", $"in{VideoTrack.Count}", dict);
            scaleFilter = videoFilterContext.FilterGraph.CreateFilter("scale", $"scale{VideoTrack.Count}", $"w={rect.Width}:h={rect.Height}");
            overlayFilter = videoFilterContext.FilterGraph.CreateFilter("overlay", $"overlay{VideoTrack.Count}", $"x={rect.X}:y={rect.Y}:format=yuv420");
            buffersrc.Link(scaleFilter, 0, 0);
            scaleFilter.Link(overlayFilter, 0, 1);
            return new VideoFilterContext(videoFilterContext.FilterGraph, buffersrc, overlayFilter);
        }
        /// <summary>
        /// Instantiate one AppositionFilter
        /// </summary>
        /// <param name="outVideoSize"></param>
        /// <param name="appositions"></param>
        /// <returns></returns>
        public static AppositionFilter AllocFilter(Size outVideoSize,params AppositionParams[] appositions)
        {
            var filter = new AppositionFilter(outVideoSize);
            foreach (AppositionParams par in appositions)
            {
                var f= filter.CreateVideoTrackByStream(par.Stream, par.Rect);
                if (filter.VideoTrack.Count > 0)
                {
                    filter.VideoTrack[filter.VideoTrack.Count - 1].SinkContext.Link(f.SinkContext,0,0);
                }
                ((List<VideoFilterContext>)filter.VideoTrack).Add(f);
            }

            filter.videoFilterContext.SourceContext.Link(filter.VideoTrack[0].SinkContext, 0, 0);
            filter.VideoTrack.Last().SinkContext.Link(filter.videoFilterContext.SinkContext, 0, 0);
            filter.videoFilterContext.FilterGraph.Configure();
            return filter;
        }

        public IEnumerable<FrameContext> WriteFrame(params MediaThreadQueue<Frame>[] queues)//index=-1表示已处理的帧
        {
            do
            {
                for (int i = 0; i < queues.Length; i++)
                {

                    if (!queues[i].IsCompleted)
                    {
                        var frame = queues[i].Take();
                        if (frame.Width > 0)
                            WriteFrame(frame, i);
                        else
                        {
                            yield return new FrameContext(frame, i);
                            continue;
                        }
                    }
                    else
                    {
                        WriteFrame(null, i);
                        yield return new FrameContext(null, i);
                    }
                }

                while (true)
                {
                    using Frame distFrame = new();
                    CodecResult r = CodecContext.ToCodecResult(videoFilterContext.SinkContext.GetFrame(distFrame));
                    if (r == CodecResult.Again) break;
                    else if (r == CodecResult.EOF) yield break;

                    if (r == CodecResult.Success)
                    {
                        yield return new FrameContext(distFrame, -1);
                    }
                }
            } while (true);
        }
        private void WriteFrame(Frame? srcFrame, int index)
        {
            try
            {
                VideoTrack[index].SourceContext.WriteFrame(srcFrame);
            }
            finally
            {
                if (srcFrame != null)
                {
                    srcFrame.Unref();
                }
            }
        }
        public IEnumerable<Frame> WriteFrame(Frame distFrame, Frame? srcFrame, int index)
        {
            WriteFrame(srcFrame, index);
            while (true)
            {
                CodecResult r = CodecContext.ToCodecResult(videoFilterContext.SinkContext.GetFrame(distFrame));
                if (r == CodecResult.Again||r == CodecResult.EOF) break;

                if (r == CodecResult.Success)
                {
                    yield return distFrame;
                }
            }
        }
    }
    public record FrameContext(Frame? Frame,int index);
    public record AppositionParams(MediaStream Stream, Rectangle Rect);

    public static class FrameContextExtensions
    {
        public static IEnumerable<Frame> ApplyAudioFilters(this IEnumerable<FrameContext> frames,AmixFilter amixFilter)
        {
            foreach(var inframe in frames)
            {
                if (inframe.Frame==null||(inframe.Frame != null&&inframe.index!=-1&&inframe.Frame.SampleRate>0))
                {
                    using Frame distframe = new();
                    foreach (var frame in amixFilter.WriteFrame(distframe, inframe.Frame, inframe.index))
                    {
                        yield return frame;
                    }
                }else if (inframe.index == -1&& inframe.Frame!=null)
                {
                    yield return inframe.Frame;
                }
            }
        }
    }
}
