using System.Collections.Generic;
using System.Linq;

using Sdcb.FFmpeg.Codecs;
using Sdcb.FFmpeg.Filters;
using Sdcb.FFmpeg.Raw;
using Sdcb.FFmpeg.Toolboxs.Extensions;
using Sdcb.FFmpeg.Utils;

namespace Sdcb.FFmpeg.Toolboxs.FilterTools
{
    public record AmixFilter
    {
        private FilterGraph graph;
        private FilterContext amixFilter, abuffersink;
        public IReadOnlyList<FilterContext> AudioTrack { get; }
        private AmixFilter(AudioSinkParams outFormat, int inputsCount)
        {
            graph = new FilterGraph();
            AudioTrack = new List<FilterContext>();
            string arg = $"inputs={inputsCount}";
            amixFilter = graph.CreateFilter("amix", "amix", arg);
            abuffersink = graph.CreateFilter("abuffersink", "sink");
            abuffersink.Options.Set("sample_fmts", new int[] { (int)outFormat.SampleFormat });
            abuffersink.Options.Set("ch_layouts", outFormat.ChLayout.Describe());
            abuffersink.Options.Set("sample_rates", new int[] { outFormat.SampleRate });
            amixFilter.Link(abuffersink);
        }
        public static AmixFilter AllocFilter(AudioSinkParams outFormat,params AMixParams[] inFormat)
        {
            AmixFilter amixFilter = new AmixFilter(outFormat, inFormat.Length);
            int index=0;
            foreach(AMixParams a in inFormat)
            {
               var src = amixFilter.graph.CreateFilter("abuffer", $"src{index}", 
                   $"sample_rate={a.Params.SampleRate}:" +
                   $"channels={a.Params.ChLayout.nb_channels}:" +
                   $"sample_fmt={(int)a.Params.SampleFormat}:" +
                   $"channel_layout={a.Params.ChLayout.u.mask}:" +
                   $"time_base={a.TimeBase}");
                src.Link(amixFilter.amixFilter,0,index);
                ((List<FilterContext>)amixFilter.AudioTrack).Add(src);
                index++;
            }
            return amixFilter;
        }
        public IEnumerable<Frame> GetConsumingEnumerable(params MediaThreadQueue<Frame>[] frames)
        {
            int index = 0;
            Frame redframe = new();
            while (frames.Max(s => s.Count) > 0)
            {
                WriteFrame(frames[index].Take(), index);
                foreach (var dist in ReadFrame(redframe))
                    yield return dist;

                index++;
                if (index >= AudioTrack.Count)
                {
                    index = 0;
                }
            }
        }
        private void WriteFrame(Frame? srcFrame,int index ,bool unref = true)
        {
            try
            {
                AudioTrack[index].WriteFrame(srcFrame);
            }
            finally
            {
                if (unref && srcFrame != null)
                {
                    srcFrame.Unref();
                }
            }
        }
        private IEnumerable<Frame> ReadFrame(Frame destFrame)
        {
            while (true)
            {
                CodecResult r = CodecContext.ToCodecResult(abuffersink.GetFrame(destFrame));
                if (r == CodecResult.Again || r == CodecResult.EOF) break;

                if (r == CodecResult.Success)
                {
                    yield return destFrame;
                }
            }
        }
    }
    public record AMixParams(AudioSinkParams Params,AVRational TimeBase);
}
