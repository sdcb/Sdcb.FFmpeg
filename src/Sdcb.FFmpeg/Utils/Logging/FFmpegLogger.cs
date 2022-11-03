using System;
using System.Runtime.InteropServices;
using System.Text;
using Sdcb.FFmpeg.Raw;
using static Sdcb.FFmpeg.Raw.ffmpeg;

namespace Sdcb.FFmpeg.Utils;

public static class FFmpegLogger
{
    /// <summary>
    /// <see cref="av_log_get_level"/>
    /// <see cref="av_log_set_level(int)"/>
    /// </summary>
    public static LogLevel LogLevel
    {
        get => (LogLevel)av_log_get_level();
        set => av_log_set_level((int)value);
    }

    /// <summary>
    /// <see cref="av_log_get_flags"/>
    /// <see cref="av_log_set_flags(int)"/>
    /// </summary>
    public static LogFlags LogFlags
    {
        get => (LogFlags)av_log_get_flags();
        set => av_log_set_flags((int)value);
    }

    /// <summary>
    /// <see cref="av_log_set_callback(av_log_set_callback_callback_func)"/>
    /// </summary>
    public static unsafe Action<LogLevel, string?>? LogWriter
    {
        set => av_log_set_callback(LogCallback = value switch
        {
            null => av_log_default_callback,
            _ => (p0, level, format, vl) =>
            {
                if (level > av_log_get_level()) return;

                const int lineSize = 1024;
                int printPrefix = 1;
                byte* lineBuffer = stackalloc byte[lineSize];
                int c = av_log_format_line2(p0, level, format, vl, lineBuffer, lineSize, &printPrefix);
                value.Invoke((LogLevel)level, Encoding.UTF8.GetString(lineBuffer, c));
            }
        });
    }

    private static av_log_set_callback_callback? LogCallback;

    /// <summary>
    /// <see cref="av_log(void*, int, string)"/>
    /// </summary>
    public unsafe static void Log(LogLevel level, string message) => av_log(null, (int)level, message);

    public static void LogTrace(string message) => Log(LogLevel.Trace, message);
    public static void LogDebug(string message) => Log(LogLevel.Debug, message);
    public static void LogVerbose(string message) => Log(LogLevel.Verbose, message);
    public static void LogInfo(string message) => Log(LogLevel.Info, message);
    public static void LogWarning(string message) => Log(LogLevel.Warning, message);
    public static void LogError(string message) => Log(LogLevel.Error, message);
    public static void LogFatal(string message) => Log(LogLevel.Fatal, message);
    public static void LogPanic(string message) => Log(LogLevel.Panic, message);
    public static void LogQuiet(string message) => Log(LogLevel.Quiet, message);
}
