using System;
using System.Buffers.Binary;
using System.Linq;

namespace Sdcb.FFmpeg.Common;

internal static class FFmpegBitConverter
{
    /// <summary>
    /// fourcc (LSB first, so "ABCD" -> ('D'&lt;&lt;24) + ('C'&lt;&lt;16) + ('B'&lt;&lt;8) + 'A').
    /// </summary>
    public static string ToFourCC(this int ffmpegVersion) => string.Join(".", BitConverter.GetBytes(BinaryPrimitives.ReverseEndianness(ffmpegVersion)).SkipWhile(v => v == 0));

    /// <summary>
    /// fourcc (LSB first, so "ABCD" -> ('D'&lt;&lt;24) + ('C'&lt;&lt;16) + ('B'&lt;&lt;8) + 'A').
    /// </summary>
    public static string ToFourCC(this uint ffmpegVersion) => ToFourCC((int)ffmpegVersion);
}
