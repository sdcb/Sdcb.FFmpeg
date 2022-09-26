using Sdcb.FFmpeg.Common;
using Sdcb.FFmpeg.Codecs;
using Sdcb.FFmpeg.Formats;
using Sdcb.FFmpeg.Utils;
using Sdcb.FFmpeg.Raw;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static Sdcb.FFmpeg.Raw.ffmpeg;

namespace Sdcb.FFmpeg.Filters;


public unsafe partial class FilterGraph : SafeHandle
{
    /// <summary>Allocate a filter graph.</summary>
    /// <see cref="avfilter_graph_alloc"/>
    public FilterGraph() : this(avfilter_graph_alloc(), isOwner: true)
    {
    }

    /// <summary>Free a graph, destroy its links, and set *graph to NULL. If *graph is NULL, do nothing.</summary>
    /// <see cref="avfilter_graph_free(AVFilterGraph**)"/>
    public void Free()
    {
        AVFilterGraph* graph = this;
        avfilter_graph_free(&graph);
        handle = (IntPtr)graph;
        SetHandleAsInvalid();
    }

    protected override bool ReleaseHandle()
    {
        Free();
        return true;
    }
}
