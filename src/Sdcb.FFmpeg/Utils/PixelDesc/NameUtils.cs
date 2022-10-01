using Sdcb.FFmpeg.Raw;
using System;
using System.Text;
using static Sdcb.FFmpeg.Raw.ffmpeg;

namespace Sdcb.FFmpeg.Utils;

public static class NameUtils
{
    /// <summary>
    /// <see cref="av_get_pix_fmt_name(AVPixelFormat)"/>
    /// </summary>
    public static string GetPixelFormatName(AVPixelFormat pixelFormat) => av_get_pix_fmt_name(pixelFormat);

    /// <summary>
    /// <see cref="av_get_channel_layout_string"/>
    /// </summary>
    public unsafe static string GetChannelLayoutString(ulong channelLayout, int channels = 0)
    {
        byte[] buffer = new byte[64];
        fixed(byte* ptr = buffer)
        {
            av_get_channel_layout_string(ptr, buffer.Length, channels, channelLayout);
            return PtrExtensions.PtrToStringUTF8((IntPtr)ptr)!;
        }
    }

    public static string GetSampleFormatName(AVSampleFormat sampleFormat) => av_get_sample_fmt_name(sampleFormat);

    /// <summary>
    /// <see cref="av_get_pix_fmt(string)"/>
    /// </summary>
    public static AVPixelFormat ToPixelFormat(string name) => av_get_pix_fmt(name);
}
