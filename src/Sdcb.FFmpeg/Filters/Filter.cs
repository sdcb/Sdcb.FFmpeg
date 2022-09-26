using Sdcb.FFmpeg.Common;
using Sdcb.FFmpeg.Raw;
using System;
using System.Collections.Generic;
using System.Linq;
using static Sdcb.FFmpeg.Raw.ffmpeg;

namespace Sdcb.FFmpeg.Filters;

public unsafe partial struct Filter
{
    /// <summary>Return the LIBAVFILTER_VERSION_INT constant.</summary>
    public static uint Version => avfilter_version();

    /// <summary>Iterate over all registered filters.</summary>
    public static IEnumerable<Filter> All => NativeUtils
        .EnumeratePtrIterator(ptr => (IntPtr)av_filter_iterate((void**)ptr))
        .Select(FromNative);

    /// <summary>Get a filter definition matching the given name.</summary>
    /// <param name="name">the filter name to find</param>
    public static Filter? GetByName(string name) => FromNativeOrNull(avfilter_get_by_name(name));
}
