using Sdcb.FFmpeg.Codecs;
using Sdcb.FFmpeg.Common;
using Sdcb.FFmpeg.Raw;
using Sdcb.FFmpeg.Utils;
using System;
using System.Runtime.InteropServices;
using static Sdcb.FFmpeg.Raw.ffmpeg;

namespace Sdcb.FFmpeg.Swscales;

/// <summary>
/// <see cref="SwsContext"/>
/// </summary>
public unsafe class VideoFrameConverter : SafeHandle
{
    public override bool IsInvalid => handle == IntPtr.Zero;

    public static implicit operator SwsContext*(VideoFrameConverter data) => (SwsContext*)data.handle;

    public VideoFrameConverter() : base(IntPtr.Zero, ownsHandle: true)
    {
    }

    /// <summary>
    /// <see cref="sws_getCachedContext(SwsContext*, int, int, AVPixelFormat, int, int, AVPixelFormat, int, SwsFilter*, SwsFilter*, double*)"/>
    /// <see cref="sws_scale(SwsContext*, byte*[], int[], int, int, byte*[], int[])"/>
    /// </summary>
    public void ConvertFrame(Frame sourceFrame, Frame destFrame, SWS flags = SWS.Bilinear)
    {
        handle = NativeUtils.NotNull(
            ptr: (IntPtr)sws_getCachedContext(this,
                sourceFrame.Width, sourceFrame.Height, (AVPixelFormat)sourceFrame.Format,
                destFrame.Width, destFrame.Height, (AVPixelFormat)destFrame.Format,
                flags: (int)flags,
                srcFilter: null, dstFilter: null, param: null),
            message: PixelConverter.ErrorMessage(
                sourceFrame.Width, sourceFrame.Height, (AVPixelFormat)sourceFrame.Format,
                destFrame.Width, destFrame.Height, (AVPixelFormat)destFrame.Format, flags));

        sws_scale(this,
            srcSlice: sourceFrame.Data.ToRawArray(),
            srcStride: sourceFrame.Linesize.ToArray(),
            srcSliceY: 0, srcSliceH: sourceFrame.Height,
            dst: destFrame.Data.ToRawArray(), destFrame.Linesize.ToArray()).ThrowIfError();
    }

    /// <summary>
    /// <see cref="sws_freeContext(SwsContext*)"/>
    /// </summary>
    public void Free()
    {
        sws_freeContext(this);
        handle = IntPtr.Zero;
    }

    protected override bool ReleaseHandle()
    {
        Free();
        return true;
    }
}
