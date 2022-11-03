using Sdcb.FFmpeg.Common;
using Sdcb.FFmpeg.Raw;
using System;
using System.Runtime.InteropServices;
using static Sdcb.FFmpeg.Raw.ffmpeg;

namespace Sdcb.FFmpeg.Utils;

public unsafe partial class BufferRef : SafeHandle
{
    /// <summary>Allocate an AVBuffer of the given size using av_malloc().</summary>
    public static BufferRef Alloc(int size, bool isOwner = true) => new BufferRef(av_buffer_alloc((ulong)size), isOwner);

    /// <summary>Same as av_buffer_alloc(), except the returned buffer will be initialized to zero.</summary>
    public static BufferRef AllocZ(int size, bool isOwner = true) => new BufferRef(av_buffer_allocz((ulong)size), isOwner);

    /// <summary>Create a new reference to an AVBuffer.</summary>
    public static BufferRef Ref(BufferRef other, bool isOwner = true) => new BufferRef(av_buffer_ref(other), isOwner);

    /// <summary>Returns 1 if the caller may write to the data referred to by buf (which is true if and only if buf is the only reference to the underlying AVBuffer). Return 0 otherwise. A positive answer is valid until av_buffer_ref() is called on buf.</summary>
    public bool IsWritable => av_buffer_is_writable(this) != 0;

    /// <summary>Returns the opaque parameter set by av_buffer_create.</summary>
    public IntPtr Opaque => (IntPtr)av_buffer_get_opaque(this);

    public int RefCount => av_buffer_get_ref_count(this);

    /// <summary>Free a given reference and automatically free the buffer if there are no more references to it.</summary>
    public void Unref()
    {
        AVBufferRef* ptr = this;
        av_buffer_unref(&ptr);
        handle = (IntPtr)ptr;
    }

    /// <summary>Create a writable reference from a given buffer reference, avoiding data copy if possible.</summary>
    public void MakeWritable()
    {
        AVBufferRef* ptr = this;
        av_buffer_make_writable(&ptr).ThrowIfError();
        handle = (IntPtr)ptr;
    }

    /// <summary>Reallocate a given buffer.</summary>
    /// <param name="size">required new buffer size.</param>
    public void Realloc(int size)
    {
        AVBufferRef* ptr = this;
        av_buffer_realloc(&ptr, (ulong)size).ThrowIfError();
        handle = (IntPtr)ptr;
    }

    /// <summary>Ensure dst refers to the same data as src.</summary>
    /// <param name="src">Pointer to either a valid buffer reference or NULL. On success, this will point to a buffer reference equivalent to src. On failure, dst will be left untouched.</param>
    public void Replace(BufferRef src)
    {
        AVBufferRef* ptr = this;
        av_buffer_replace(&ptr, src).ThrowIfError();
        handle = (IntPtr)ptr;
    }

    protected override bool ReleaseHandle()
    {
        Unref();
        return true;
    }
}
