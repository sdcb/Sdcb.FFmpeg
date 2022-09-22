using Sdcb.FFmpeg.Raw;
using static Sdcb.FFmpeg.Raw.ffmpeg;

namespace Sdcb.FFmpeg.Utils;

public static class PixelUtils
{
    /// <summary>
    /// <see cref="av_get_pix_fmt_name(AVPixelFormat)"/>
    /// </summary>
    public static string GetPixelFormatName(AVPixelFormat pixelFormat) => av_get_pix_fmt_name(pixelFormat);

    /// <summary>
    /// <see cref="av_get_pix_fmt(string)"/>
    /// </summary>
    public static AVPixelFormat ToPixelFormat(string name) => av_get_pix_fmt(name);
}
