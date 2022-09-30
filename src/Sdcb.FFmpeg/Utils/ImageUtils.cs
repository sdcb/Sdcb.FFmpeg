using Sdcb.FFmpeg.Common;
using Sdcb.FFmpeg.Raw;
using System;
using static Sdcb.FFmpeg.Raw.ffmpeg;

namespace Sdcb.FFmpeg.Utils;

public static class ImageUtils
{
    /// <summary>
    /// <see cref="av_image_get_buffer_size(AVPixelFormat, int, int, int)"/>
    /// </summary>
    public static int GetBufferSize(AVPixelFormat pixelFormat, int width, int height, int align = 1)
        => av_image_get_buffer_size(pixelFormat, width, height, align).ThrowIfError();

    /// <summary>
    /// <see cref="av_image_copy_to_buffer(byte*, int, byte_ptrArray4, int_array4, AVPixelFormat, int, int, int)"/>
    /// </summary>
    public static unsafe int CopyToBuffer(
        AVPixelFormat pixelFormat, 
        int width, 
        int height, 
        DataPointer dest, 
        byte_ptrArray4 sourceData, int_array4 sourceLinesize, 
        int align = 1)
    {
        return av_image_copy_to_buffer(
            (byte*)dest.Pointer, dest.Length, 
            sourceData, sourceLinesize,
            pixelFormat, 
            width, height, align).ThrowIfError();
    }

    /// <summary>
    /// <see cref="av_image_fill_linesizes(ref int_array4, AVPixelFormat, int)"/>
    /// </summary>
    public static unsafe int_array4 GetLinesize(AVPixelFormat pixelFormat, int width)
    {
        int_array4 linesizes;
        av_image_fill_linesizes(ref linesizes, pixelFormat, width).ThrowIfError();
        return linesizes;
    }

    /// <summary>
    /// <see cref="av_image_fill_pointers(byte_ptrArray4, AVPixelFormat, int, byte*, int_array4)"/>
    /// </summary>
    public static unsafe byte_ptrArray4 ToPlaneData(IntPtr sourceData, int_array4 linesizes, AVPixelFormat pixelFormat, int height)
    {
        byte_ptrArray4 ret = default;
        av_image_fill_pointers(ret, pixelFormat, height, (byte*)sourceData, linesizes).ThrowIfError();
        return ret;
    }
}
