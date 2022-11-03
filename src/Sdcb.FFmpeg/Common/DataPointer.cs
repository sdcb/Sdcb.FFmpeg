using System;

namespace Sdcb.FFmpeg.Common;

public record struct DataPointer(IntPtr Pointer, int Length)
{
    public unsafe DataPointer(byte* pointer, int length) : this((IntPtr)pointer, length)
    {
    }

    public DataPointer Slice(int start, int end)
    {
        if (start > Length) throw new ArgumentOutOfRangeException(nameof(start));
        if (end < 0) throw new ArgumentOutOfRangeException(nameof(end));
        return new DataPointer(Pointer + start, end);
    }

    public unsafe Span<byte> AsSpan() => new Span<byte>((byte*)Pointer, Length);

    public byte[] ToArray() => AsSpan().ToArray();
}
