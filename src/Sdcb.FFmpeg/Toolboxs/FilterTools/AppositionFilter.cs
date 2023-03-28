using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using Sdcb.FFmpeg.Codecs;
using Sdcb.FFmpeg.Filters;
using Sdcb.FFmpeg.Formats;
using Sdcb.FFmpeg.Toolboxs.Extensions;
using Sdcb.FFmpeg.Utils;

namespace Sdcb.FFmpeg.Toolboxs.FilterTools
{
    public record AppositionFilter
    {
        private VideoFilterContext videoFilterContext;
        public IReadOnlyList<VideoFilterContext> VideoTrack { get; } = new List<VideoFilterContext>();
        private AppositionFilter(Size outVideoSize,long duration)
        {
            FilterGraph graph = new();
            FilterContext nullfilter = graph.CreateFilter("nullsrc", "srcbase", $"duration={TimeSpan.FromTicks(duration*10).TotalSeconds}:rate=25:size={outVideoSize.Width}x{outVideoSize.Height}"); //graph.CreateFilter("nullsrc", "srcbase");
            videoFilterContext = new VideoFilterContext(graph, nullfilter, graph.CreateFilter("buffersink", "out"));
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
            var filter = new AppositionFilter(outVideoSize, appositions.Max(s=>s.Context.Duration));
            //long maxDuration = 0;
            foreach (AppositionParams par in appositions)
            {
                var f= filter.CreateVideoTrackByStream(par.Context.GetVideoStream(), par.Rect);
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

        public IEnumerable<Frame> GetConsumingEnumerable(params MediaThreadQueue<Frame>[] frames)
        {
            int index = 0;
            while (frames.Max(s=>s.Count)>0)
            {
                foreach (var dist in AppositionFrames(frames[index].Take(), index))
                    yield return dist;

                index++;
                if (index >= VideoTrack.Count)
                {
                    index=0;
                }
                //foreach (var dist in ReadFrames())
                //    yield return dist;
            }
        }
        public IEnumerable<Frame> AppositionFrames(Frame srcFrame, int srcIndex)
        {
            using Frame distFrame = new();
            foreach (var frame in VideoTrack[srcIndex].WriteFrame(distFrame, srcFrame))
                yield return frame;
            //if (srcFrame.Width == 0)
            //    yield return srcFrame;
            //AddFrame(srcFrame,srcIndex);
            //foreach (var frame in ReadFrames())
            //    yield return frame;
        }
        //public void AddFrame(Frame srcFrame, int srcIndex)
        //{
        //    using Frame distFrame = new();
        //    if (srcFrame.Width == 0)
        //        return;
        //    VideoTrack[srcIndex].WriteFrame(srcFrame);
        //    srcFrame.Unref();

        //}
        //public IEnumerable<Frame> ReadFrames()
        //{
        //    Frame distframe = new();
        //    foreach (var frame in videoFilterContext.ReadFrame(distframe))
        //        yield return frame;
        //}
        public void ConfigureEncoder(CodecContext c)
        {
            videoFilterContext.ConfigureEncoder(c);
        }
    }
    public record AppositionParams(FormatContext Context, Rectangle Rect);
}
