using System.Collections.Generic;
using System.Drawing;

using Sdcb.FFmpeg.Filters;
using Sdcb.FFmpeg.Formats;
using Sdcb.FFmpeg.Raw;
using Sdcb.FFmpeg.Utils;

namespace Sdcb.FFmpeg.Toolboxs.FilterTools
{
    public record AppositionFilter:MultipleToOneFilter
    {
        public AppositionFilter(Size outVideoSize, FilterGraph FilterGraph, FilterContext SinkContext):base(FilterGraph,SinkContext)
        {
            SourceContext= FilterGraph.CreateFilter("nullsrc", "srcbase", $"duration=0.1:rate=25:size={outVideoSize.Width}x{outVideoSize.Height}");
        }
        private FilterContext SourceContext;
        private VideoFilterContext CreateVideoTrackByStream(MediaStream mediaStream,Rectangle rect)
        {
            FilterContext scaleFilter,
            overlayFilter,
            buffersrc;//叠加流
            var dict = VideoFilterContext.GetVideoSrcMetadata(mediaStream);
            buffersrc = FilterGraph.CreateFilter("buffer", $"in{Track.Count}", dict);
            scaleFilter = FilterGraph.CreateFilter("scale", $"scale{Track.Count}", $"w={rect.Width}:h={rect.Height}");
            overlayFilter = FilterGraph.CreateFilter("overlay", $"overlay{Track.Count}", $"x={rect.X}:y={rect.Y}:format=yuv420");
            buffersrc.Link(scaleFilter, 0, 0);
            scaleFilter.Link(overlayFilter, 0, 1);
            return new VideoFilterContext(FilterGraph, buffersrc, overlayFilter);
        }
        
        /// <summary>
        /// Instantiate one AppositionFilter
        /// </summary>
        /// <param name="outVideoSize"></param>
        /// <param name="appositions"></param>
        /// <returns></returns>
        public static AppositionFilter AllocFilter(Size outVideoSize,params AppositionParams[] appositions)
        {
            FilterGraph graph = new();
            var sink = graph.CreateFilter("buffersink", "out");
            sink.Options.Set("pix_fmts", new int[] { (int)AVPixelFormat.Yuv420p }, AV_OPT_SEARCH.Children);
            var filter = new AppositionFilter(outVideoSize,graph, sink);
            FilterContext? lastfilter=null,firstfilter=null;
            foreach (AppositionParams par in appositions)
            {
                var f= filter.CreateVideoTrackByStream(par.Stream, par.Rect);
                if (lastfilter!=null)
                {
                    lastfilter.Link(f.SinkContext,0,0);
                }
                else
                {
                    firstfilter = f.SinkContext;
                }
                ((List<FilterContext>)filter.Track).Add(f.SourceContext);
                lastfilter = f.SinkContext;
            }

            filter.SourceContext.Link(firstfilter!, 0, 0);
            lastfilter!.Link(filter.SinkContext, 0, 0);
            filter.FilterGraph.Configure();
            return filter;
        }
    }
    public record FrameContext(Frame? Frame,int index);
    public record AppositionParams(MediaStream Stream, Rectangle Rect);

    public static class FrameContextExtensions
    {
        public static IEnumerable<Frame> ToFrames(this IEnumerable<FrameContext> frames)
        {
            foreach(var inframe in frames)
            {
                yield return inframe.Frame!;
            }
        }
        public static IEnumerable<FrameContext> ApplyMultipleToOneFilter(this IEnumerable<FrameContext> frames, MultipleToOneFilter filter)
        {
            foreach (var inframe in frames)
            {
                if (inframe.Frame == null || (inframe.Frame != null && inframe.index != -1 && inframe.Frame.SampleRate > 0))
                {
                    using Frame distframe = new();
                    foreach (var frame in filter.WriteFrame(distframe, inframe.Frame, inframe.index))
                    {
                        yield return new(frame, -1);
                    }
                }
                else if (inframe.index == -1 && inframe.Frame != null)
                {
                    yield return inframe;
                }
            }
        }
    }
}
