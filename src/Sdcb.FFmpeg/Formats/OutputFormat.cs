using Sdcb.FFmpeg.Common;
using Sdcb.FFmpeg.Raw;
using System;
using System.Collections.Generic;
using static Sdcb.FFmpeg.Raw.ffmpeg;
using System.Linq;

namespace Sdcb.FFmpeg.Formats;

public unsafe partial struct OutputFormat
{
    /// <summary>
    /// Return the output format in the list of registered output formats which best matches the provided parameters, or return NULL if there is no match.
    /// <param name="shortName">if non-NULL checks if short_name matches with the names of the registered formats</param>
    /// <param name="fileName">if non-NULL checks if filename terminates with the extensions of the registered formats</param>
    /// <param name="mimeType">if non-NULL checks if mime_type matches with the MIME type of the registered formats</param>
    /// <see cref="av_guess_format(string, string, string)"/>
    /// </summary>
    public static OutputFormat? Guess(string? shortName = null, string? fileName = null, string? mimeType = null)
        => FromNativeOrNull(av_guess_format(shortName, fileName, mimeType));

    /// <summary>
    /// Iterate over all registered muxers.
    /// <see cref="av_muxer_iterate(void**)"/>
    /// </summary>
    public static IEnumerable<OutputFormat> All => NativeUtils
        .EnumeratePtrIterator(ptr => (IntPtr)av_muxer_iterate((void**)ptr))
        .Select(x => FromNative((AVOutputFormat*)x));
}
