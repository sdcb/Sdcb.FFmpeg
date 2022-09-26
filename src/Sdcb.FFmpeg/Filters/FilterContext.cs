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

public unsafe partial class FilterContext : SafeHandle
{
    /// <summary>Free a filter context. This will also remove the filter from its filtergraph&apos;s list of filters.</summary>
    public void Free() => avfilter_free(this);

    protected override bool ReleaseHandle()
    {
        Free();
        SetHandleAsInvalid();
        return true;
    }
}
