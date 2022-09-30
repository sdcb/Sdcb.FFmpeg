using System;
using static Sdcb.FFmpeg.Raw.ffmpeg;

namespace Sdcb.FFmpeg.Common;

public ref struct DisposableNativeString
{
    public IntPtr Pointer;

    public DisposableNativeString(IntPtr pointer)
    {
        if (pointer == IntPtr.Zero)
        {
            throw FFmpegException.NoMemory($"pointer = 0 in {nameof(DisposableNativeString)}.");
        }
        Pointer = pointer;
    }

    public unsafe DisposableNativeString(byte* pointer) : this((IntPtr)pointer)
    {
    }

    public unsafe void Dispose()
    {
        av_free((void*)Pointer);
        Pointer = IntPtr.Zero;
    }

    public override string? ToString()
    {
        return Pointer.PtrToStringUTF8();
    }
}
