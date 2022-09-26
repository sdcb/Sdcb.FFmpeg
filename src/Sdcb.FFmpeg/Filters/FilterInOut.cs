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

public unsafe partial class FilterInOut
{
    public FilterInOut() : this(avfilter_inout_alloc(), isOwner: true)
    {
    }

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
