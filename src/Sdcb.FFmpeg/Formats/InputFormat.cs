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
    /// Find AVInputFormat based on the short name of the input format.
    /// <see cref="av_find_input_format(string)"/>
    /// </summary>
    public static InputFormat? FindByShortName(string shortName)
    {
        AVInputFormat* ptr = av_find_input_format(shortName);
        return ptr != null ? new InputFormat(ptr) : null;
    }

    /// <summary>
    /// Guess the file format.
    /// <param name="data">data to be probed</param>
    /// <param name="isOpened">Whether the file is already opened; determines whether demuxers with or without AVFMT_NOFILE are probed.</param>
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
    /// Guess the file format.
    /// <param name="data">data to be probed</param>
    /// <param name="isOpened">Whether the file is already opened; determines whether demuxers with or without AVFMT_NOFILE are probed.</param>
    /// <param name="score">A probe score larger that this is required to accept a detection, the variable is set to the actual detection score afterwards. If the score is &lt; = AVPROBE_SCORE_MAX / 4 it is recommended to retry with a larger probe buffer.</param>
    /// <see cref="av_probe_input_format2(AVProbeData*, int, int*)"/>
    /// </summary>
    public static ProbeResult ProbeInputFormat2(ProbeData data, bool isOpened = true, int score = AVPROBE_SCORE_MAX / 4)
    {
        AVProbeData probeData;
        using (data.FillNativeScoped(&probeData))
        {
            AVInputFormat* ptr = av_probe_input_format2(&probeData, isOpened ? 1 : 0, &score);
            return new ProbeResult(score, ptr != null ? new InputFormat?(FromNative(ptr)) : null);
        }
    }

    /// <summary>
    /// Guess the file format.
    /// <param name="isOpened">Whether the file is already opened; determines whether demuxers with or without AVFMT_NOFILE are probed.</param>
    /// <see cref="av_probe_input_format3(AVProbeData*, int, int*)"/>
    /// </summary>
    public static ProbeResult ProbeInputFormat3(ProbeData data, bool isOpened = true)
    {
        AVProbeData probeData;
        using (data.FillNativeScoped(&probeData))
        {
            int resultScore;
            AVInputFormat* ptr = av_probe_input_format3(&probeData, isOpened ? 1 : 0, &resultScore);
            return new ProbeResult(resultScore, ptr != null ? new InputFormat?(FromNative(ptr)) : null);
        }
    }

    /// <summary>
    /// Probe a bytestream to determine the input format. Each time a probe returns with a score that is too low, the probe buffer size is increased and another attempt is made. When the maximum probe size is reached, the input format with the highest score is returned.
    /// <param name="io">the bytestream to probe</param>
    /// <param name="url">the url of the stream</param>
    /// <param name="logCtx">the log context</param>
    /// <param name="offset">the offset within the bytestream to probe from</param>
    /// <param name="maxProbeSize">the maximum probe buffer size (zero for default)</param>
    /// <see cref="av_probe_input_buffer2(AVIOContext*, AVInputFormat**, string, void*, uint, uint)"/>
    /// </summary>
    public static ProbeResult ProbeInputBuffer(IOContext io, string url, IntPtr logCtx, uint offset = 0, uint maxProbeSize = 0)
    {
        AVInputFormat* format;
        int score = av_probe_input_buffer2(io, &format, url, (void*)logCtx, offset, maxProbeSize).ThrowIfError();
        return new ProbeResult(score, FromNativeOrNull(format));
    }

    /// <summary>
    /// Iterate over all registered demuxers.
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

    public record struct ProbeResult(int Score, InputFormat? Format)
    {
        public static implicit operator InputFormat(ProbeResult r) => r.Format switch
        {
            null => throw new FFmpegException($"No probed format found, score: {r.Score}"),
            _ => r.Format.Value,
        };
    }
}