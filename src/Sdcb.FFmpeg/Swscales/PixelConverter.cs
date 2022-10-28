using Sdcb.FFmpeg.Common;
using Sdcb.FFmpeg.Raw;
using Sdcb.FFmpeg.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using static Sdcb.FFmpeg.Raw.ffmpeg;

namespace Sdcb.FFmpeg.Swscales;

public unsafe class PixelConverter : SafeHandle
{
    public override bool IsInvalid => handle == IntPtr.Zero;

    public static implicit operator SwsContext*(PixelConverter data) => (SwsContext*)data.handle;
    public static PixelConverter FromNative(SwsContext* ptr, bool isOwner) => new PixelConverter((IntPtr)ptr, isOwner);

    protected PixelConverter(IntPtr nativePointer, bool isOwner) : base(nativePointer, isOwner)
    {
    }

    /// <summary>
    /// <see cref="sws_getContext(int, int, AVPixelFormat, int, int, AVPixelFormat, int, SwsFilter*, SwsFilter*, double*)"/>
    /// </summary>
    public PixelConverter(
        int sourceWidth, int sourceHeight, AVPixelFormat sourcePixelFormat,
        int destWidth, int destHeight, AVPixelFormat destPixelFormat,
        SWS flags = SWS.Bilinear) : base(NativeUtils.NotNull(
            ptr: (IntPtr)sws_getContext(
                sourceWidth, sourceHeight, sourcePixelFormat,
                destWidth, destHeight, destPixelFormat,
                flags: (int)flags,
                srcFilter: null,
                dstFilter: null,
                param: null),
            message: ErrorMessage(sourceWidth, sourceHeight, sourcePixelFormat, destWidth, destHeight, destPixelFormat, flags)), ownsHandle: true)
    {
    }

    internal static string ErrorMessage(
        int sourceWidth, int sourceHeight, AVPixelFormat sourcePixelFormat,
        int destWidth, int destHeight, AVPixelFormat destPixelFormat,
        SWS flags) =>
        string.Format("Impossible to create scale context for the conversion fmt:{0} s:{1}x{2} -> fmt:{3} s:{4}x{5} (sws_flags: {6})",
            NameUtils.GetPixelFormatName(sourcePixelFormat), sourceWidth, sourceHeight,
            NameUtils.GetPixelFormatName(destPixelFormat), destWidth, destHeight, flags);

    /// <summary>
    /// <see cref="sws_scale(SwsContext*, byte*[], int[], int, int, byte*[], int[])"/>
    /// </summary>
    public void Convert(byte_ptrArray4 sourceData, int_array4 sourceLinesize, int sourceSliceH, byte_ptrArray4 destData, int_array4 destLinesize, int sourceSliceY = 0) =>
        sws_scale(this,
            srcSlice: sourceData.ToRawArray(), srcStride: sourceLinesize.ToArray(), srcSliceY: sourceSliceY, srcSliceH: sourceSliceH,
            destData.ToRawArray(), destLinesize.ToArray()).ThrowIfError();

    /// <summary>
    /// <see cref="sws_scale(SwsContext*, byte*[], int[], int, int, byte*[], int[])"/>
    /// </summary>
    public void Convert(byte_ptrArray8 sourceData, int_array8 sourceLinesize, int sourceSliceH, byte_ptrArray8 destData, int_array8 destLinesize, int sourceSliceY = 0) =>
        sws_scale(this,
            srcSlice: sourceData.ToRawArray(), srcStride: sourceLinesize.ToArray(), srcSliceY: sourceSliceY, srcSliceH: sourceSliceH,
            destData.ToRawArray(), destLinesize.ToArray()).ThrowIfError();

    public FFmpegOptions Options => new FFmpegOptions(this);

    /// <summary>
    /// <see cref="sws_freeContext(SwsContext*)"/>
    /// </summary>
    public void Free()
    {
        sws_freeContext(this);
        handle = IntPtr.Zero;
        SetHandleAsInvalid();
    }

    protected override bool ReleaseHandle()
    {
        Free();
        return true;
    }

    /// <summary>
    /// <see cref="swscale_version"/>
    /// </summary>
    public static string Version => swscale_version().ToFourCC();

    /// <summary>
    /// <see cref="swscale_configuration"/>
    /// </summary>
    public static HashSet<string> Configuration => swscale_configuration()
        .Split(' ')
        .ToHashSet();

    /// <summary>
    /// <see cref="swscale_license"/>
    /// </summary>
    public static string License => swscale_license();
}
