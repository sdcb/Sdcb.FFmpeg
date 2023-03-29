using System.Collections.Generic;

using Sdcb.FFmpeg.Filters;
using Sdcb.FFmpeg.Raw;
using Sdcb.FFmpeg.Utils;

namespace Sdcb.FFmpeg.Toolboxs.FilterTools
{
    public record AmixFilter(FilterGraph filterGraph, FilterContext sinkContext) : MultipleToOneFilter(filterGraph, sinkContext)
    {
        public static AmixFilter AllocFilter(AudioSinkParams outFormat,params AudioSinkParams[] inFormat)
        {
            FilterGraph graph = new FilterGraph();
            string arg = $"inputs={inFormat.Length}";
            FilterContext amixFilterCxt = graph.CreateFilter("amix", "amix", arg);
            FilterContext abuffersink = graph.CreateFilter("abuffersink", "sink");
            abuffersink.Options.Set("sample_fmts", new int[] { (int)outFormat.SampleFormat });
            abuffersink.Options.Set("ch_layouts", outFormat.ChLayout.Describe());
            abuffersink.Options.Set("sample_rates", new int[] { outFormat.SampleRate });
            amixFilterCxt.Link(abuffersink);

            AmixFilter amixFilter = new AmixFilter(graph, abuffersink);
            int index=0;
            foreach(AudioSinkParams a in inFormat)
            {
               var src = amixFilter.FilterGraph.CreateFilter("abuffer", $"src{index}", 
                   $"sample_rate={a.SampleRate}:" +
                   $"channels={a.ChLayout.nb_channels}:" +
                   $"sample_fmt={NameUtils.GetSampleFormatName(a.SampleFormat)}:" +
                   $"channel_layout={a.ChLayout.Describe()}:" +
                   $"time_base={new AVRational(1, a.SampleRate)}");
                src.Link(amixFilterCxt, 0,index);
                ((List<FilterContext>)amixFilter.Track).Add(src);
                index++;
            }
            amixFilter.FilterGraph.Configure();
            return amixFilter;
        }
    }
}
