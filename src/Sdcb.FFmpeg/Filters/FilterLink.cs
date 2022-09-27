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
    /// <summary>
    /// <para>Insert a filter in the middle of an existing link.</para>
    /// <see cref="avfilter_insert_filter"/>
    /// </summary>
    /// <param name="filter">the filter to be inserted</param>
    /// <param name="srcPadIndex">the input pad on the filter to connect</param>
    /// <param name="destPadIndex">the output pad on the filter to connect</param>
    public void InsertFilter(FilterContext filter, int srcPadIndex, int destPadIndex)
    {
        avfilter_insert_filter(this, filter, (uint)srcPadIndex, (uint)destPadIndex).ThrowIfError();
    }

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
