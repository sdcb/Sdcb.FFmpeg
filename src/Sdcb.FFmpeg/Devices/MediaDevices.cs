using Sdcb.FFmpeg.Common;
using Sdcb.FFmpeg.Formats;
using Sdcb.FFmpeg.Raw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Sdcb.FFmpeg.Raw.ffmpeg;

namespace Sdcb.FFmpeg.Devices;

public unsafe static class MediaDevice
{
    /// <summary>
    /// <see cref="avdevice_version"/>
    /// </summary>
    public static string Version => avdevice_version().ToFourCC();

    /// <summary>
    /// <see cref="avdevice_configuration"/>
    /// </summary>
    public static HashSet<string> Configuration => avdevice_configuration()
        .Split(' ')
        .ToHashSet();

    /// <summary>
    /// <see cref="avdevice_license"/>
    /// </summary>
    public static string License => avdevice_license();

    /// <summary>
    /// <see cref="avdevice_register_all"/>
    /// </summary>
    public static void RegisterAll() => avdevice_register_all();

    /// <summary>
    /// <see cref="av_input_audio_device_next(AVInputFormat*)"/>
    /// </summary>
    public static IEnumerable<InputFormat> InputAudioDevices =>
        EnumeratePtr(ptr => (IntPtr)av_input_audio_device_next((AVInputFormat*)ptr))
        .Select(InputFormat.FromNative);

    /// <summary>
    /// <see cref="av_input_video_device_next(AVInputFormat*)"/>
    /// </summary>
    public static IEnumerable<InputFormat> InputVideoDevices =>
        EnumeratePtr(ptr => (IntPtr)av_input_video_device_next((AVInputFormat*)ptr))
        .Select(InputFormat.FromNative);

    /// <summary>
    /// <see cref="av_output_audio_device_next(AVOutputFormat*)"/>
    /// </summary>
    public static IEnumerable<OutputFormat> OutputAudioDevices =>
        EnumeratePtr(ptr => (IntPtr)av_output_audio_device_next((AVOutputFormat*)ptr))
        .Select(OutputFormat.FromNative);

    /// <summary>
    /// <see cref="av_output_video_device_next(AVOutputFormat*)"/>
    /// </summary>
    public static IEnumerable<OutputFormat> OutputVideoDevices =>
        EnumeratePtr(ptr => (IntPtr)av_output_video_device_next((AVOutputFormat*)ptr))
        .Select(OutputFormat.FromNative);

    private static IEnumerable<IntPtr> EnumeratePtr(Func<IntPtr, IntPtr> iterator)
    {
        IntPtr result = IntPtr.Zero;
        while (true)
        {
            result = iterator(result);
            if (result == IntPtr.Zero) break;
            yield return result;
        }
    }
}
