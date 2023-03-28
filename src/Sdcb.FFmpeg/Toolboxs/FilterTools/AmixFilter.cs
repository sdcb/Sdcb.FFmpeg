using System;
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
        public static AmixFilter AllocFilter(AudioSinkParams outFormat,params AudioSinkParams[] inFormat)
        {
            AmixFilter amixFilter = new AmixFilter(outFormat, inFormat.Length);
            int index=0;
            foreach(AudioSinkParams a in inFormat)
            {
               var src = amixFilter.graph.CreateFilter("abuffer", $"src{index}", 
                   $"sample_rate={a.SampleRate}:" +
                   $"channels={a.ChLayout.nb_channels}:" +
                   $"sample_fmt={NameUtils.GetSampleFormatName(a.SampleFormat)}:" +
                   $"channel_layout={a.ChLayout.Describe()}:" +
                   $"time_base={new AVRational(1, a.SampleRate)}");
                src.Link(amixFilter.amixFilter,0,index);
                ((List<FilterContext>)amixFilter.AudioTrack).Add(src);
                index++;
            }
            amixFilter.graph.Configure();
            return amixFilter;
        }
        public IEnumerable<FrameContext> WriteFrame(params MediaThreadQueue<Frame>[] queues)
        {
            do
            {
                using Frame distFrame = new();
                for (int i = 0; i < queues.Length; i++)
                {
                    if (!queues[i].IsCompleted)
                    {
                        var frame = queues[i].Take();
                        if(frame.SampleRate>0)
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
                    CodecResult r = CodecContext.ToCodecResult(abuffersink.GetFrame(distFrame));
                    if (r == CodecResult.Again) break;
                    else if (r == CodecResult.EOF) yield break;

                    if (r == CodecResult.Success)
                    {
                        yield return new(distFrame,-1);
                    }
                }
            } while (true);
        }
        private void WriteFrame(Frame? srcFrame, int index ,bool unref = true)
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
        public IEnumerable<Frame> WriteFrame(Frame distFrame, Frame? srcFrame,int index)
        {
            WriteFrame(srcFrame, index);
            while (true)
            {
                CodecResult r = CodecContext.ToCodecResult(abuffersink.GetFrame(distFrame));
                if (r == CodecResult.Again||r == CodecResult.EOF) break;

                if (r == CodecResult.Success)
                {
                    yield return distFrame;
                }
            }
        }
    }
}
