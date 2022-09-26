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

public unsafe partial class FilterLink : SafeHandle
{
    public void Free()
    {
        AVFilterLink* link = this;
        avfilter_link_free(&link);
        handle = (IntPtr)link;
        SetHandleAsInvalid();
    }

    protected override bool ReleaseHandle()
    {
        Free();
        return true;
    }
}
