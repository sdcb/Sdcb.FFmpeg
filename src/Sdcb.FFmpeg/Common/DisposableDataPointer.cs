using System;
using static Sdcb.FFmpeg.Raw.ffmpeg;

namespace Sdcb.FFmpeg.Common;

public ref struct DisposableDataPointer
{
    public IntPtr Pointer { get; private set; }
    public int Length { get; private set; }

    public DisposableDataPointer(IntPtr pointer, int length)
    {
        Pointer = pointer;
        Length = length;
    }

    public unsafe DisposableDataPointer(byte* pointer, int length) : this((IntPtr)pointer, length)
    {
    }

    public unsafe DisposableDataPointer(int allocSize) : this(NativeUtils.NotNull((IntPtr)av_malloc((ulong)allocSize)), allocSize)
    {
    }

    public unsafe DisposableDataPointer(Span<byte> data)
    {
        fixed (byte* ptr = data)
        {
            Pointer = (IntPtr)ptr;
            Length = data.Length;
        }
    }

    public DataPointer Slice(int start, int end)
    {
        if (start > Length) throw new ArgumentOutOfRangeException(nameof(start));
        if (end < 0) throw new ArgumentOutOfRangeException(nameof(end));
        return new DataPointer(Pointer + start, end);
    } 

    public unsafe Span<byte> AsSpan() => new Span<byte>((byte*)Pointer, Length);

    public unsafe void Dispose()
    {
        av_free((void*)Pointer);
        Pointer = IntPtr.Zero;
        Length = 0;
    }
}
