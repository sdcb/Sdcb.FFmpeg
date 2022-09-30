using Sdcb.FFmpeg.Raw;
using System;
using static Sdcb.FFmpeg.Raw.ffmpeg;

namespace Sdcb.FFmpeg.Filters;

public unsafe partial class FilterInOut
{
    public FilterInOut() : this(avfilter_inout_alloc(), isOwner: true)
    {
    }

    public FilterInOut(string name, FilterContext filterContext) : this()
    {
        Name = name;
        FilterContext = filterContext;
    }

    public void Reset(IntPtr ptr) => handle = ptr;

    public void Free()
    {
        AVFilterInOut* ptr = this;
        avfilter_inout_free(&ptr);
        handle = (IntPtr)ptr;
        SetHandleAsInvalid();
    }

    protected override bool ReleaseHandle()
    {
        Free();
        return true;
    }
}
