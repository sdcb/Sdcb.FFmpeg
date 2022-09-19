using Sdcb.FFmpeg.Common;
using Sdcb.FFmpeg.Raw;
using System;
using System.Collections.Generic;
using static Sdcb.FFmpeg.Raw.ffmpeg;
using System.Linq;

namespace Sdcb.FFmpeg.Formats;

public unsafe partial struct InputFormat
{
    /// <summary>
    /// <see cref="av_find_input_format(string)"/>
    /// </summary>
    public static InputFormat? FindByShortName(string shortName)
    {
        AVInputFormat* ptr = av_find_input_format(shortName);
        return ptr != null ? new InputFormat(ptr) : null;
    }

    /// <summary>
    /// <see cref="av_probe_input_format(AVProbeData*, int)"/>
    /// </summary>
    public static InputFormat? ProbeInputFormat(ProbeData data, bool isOpened = false)
    {
        AVProbeData probeData;
        using (data.FillNativeScoped(&probeData))
        {
            AVInputFormat* ptr = av_probe_input_format(&probeData, isOpened ? 1 : 0);
            return ptr != null ? new InputFormat?(FromNative(ptr)) : null;
        }
    }

    /// <summary>
    /// <see cref="av_probe_input_format2(AVProbeData*, int, int*)"/>
    /// </summary>
    public static (int, InputFormat?) ProbeInputFormat2(ProbeData data, bool isOpened = false)
    {
        int maxScoreRet;
        AVProbeData probeData;
        using (data.FillNativeScoped(&probeData))
        {
            AVInputFormat* ptr = av_probe_input_format2(&probeData, isOpened ? 1 : 0, &maxScoreRet);
            return (maxScoreRet, ptr != null ? new InputFormat?(FromNative(ptr)) : null);
        }
    }

    /// <summary>
    /// <see cref="av_probe_input_format3(AVProbeData*, int, int*)"/>
    /// </summary>
    public static (int, InputFormat?) ProbeInputFormat3(ProbeData data, bool isOpened = false)
    {
        int scoreRet;
        AVProbeData probeData;
        using (data.FillNativeScoped(&probeData))
        {
            AVInputFormat* ptr = av_probe_input_format3(&probeData, isOpened ? 1 : 0, &scoreRet);
            return (scoreRet, ptr != null ? new InputFormat?(FromNative(ptr)) : null);
        }
    }

    /// <summary>
    /// <see cref="av_probe_input_buffer2(AVIOContext*, AVInputFormat**, string, void*, uint, uint)"/>
    /// </summary>
    public static (int, InputFormat) ProbeInputBuffer(IOContext io, string url, IntPtr logCtx, uint offset = 0, uint maxProbeSize = 0)
    {
        AVInputFormat* format;
        int score = av_probe_input_buffer2(io, &format, url, (void*)logCtx, offset, maxProbeSize).ThrowIfError();
        return (score, FromNative(format));
    }

    /// <summary>
    /// <see cref="av_demuxer_iterate(void**)"/>
    /// </summary>
    public static IEnumerable<InputFormat> All => NativeUtils
        .EnumeratePtrIterator(ptr => (IntPtr)av_demuxer_iterate((void**)ptr))
        .Select(x => FromNative((AVInputFormat*)x));

    private static InputFormat FindByShortNameNotNull(string shortName)
    {
        AVInputFormat* ptr = av_find_input_format(shortName);
        if (ptr == null)
        {
            throw new ArgumentOutOfRangeException(nameof(shortName), $"Cannot find InputFormat shortName: {shortName}");
        }
        return new InputFormat(ptr);
    }

    public static InputFormat DShow => FindByShortNameNotNull("dshow");
    public static InputFormat GdiGrab => FindByShortNameNotNull("gdigrab");
}