// This file was genereated from Sdcb.FFmpeg.AutoGen, DO NOT CHANGE DIRECTLY.
#nullable enable
using Sdcb.FFmpeg.Common;
using Sdcb.FFmpeg.Codecs;
using Sdcb.FFmpeg.Formats;
using Sdcb.FFmpeg.Filters;
using Sdcb.FFmpeg.Raw;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Sdcb.FFmpeg.Utils;

/// <summary>
/// <para>A reference to a data buffer.</para>
/// <see cref="AVBufferRef" />
/// </summary>
public unsafe partial class BufferRef : SafeHandle
{
    protected AVBufferRef* _ptr => (AVBufferRef*)handle;
    
    public static implicit operator AVBufferRef*(BufferRef data) => data != null ? (AVBufferRef*)data.handle : null;
    
    protected BufferRef(AVBufferRef* ptr, bool isOwner): base(NativeUtils.NotNull((IntPtr)ptr), isOwner)
    {
    }
    
    public static BufferRef FromNative(AVBufferRef* ptr, bool isOwner) => new BufferRef(ptr, isOwner);
    
    internal static BufferRef FromNative(IntPtr ptr, bool isOwner) => new BufferRef((AVBufferRef*)ptr, isOwner);
    
    public static BufferRef? FromNativeOrNull(AVBufferRef* ptr, bool isOwner) => ptr == null ? null : new BufferRef(ptr, isOwner);
    
    public override bool IsInvalid => handle == IntPtr.Zero;
    
    /// <summary>
    /// <para>original type: AVBuffer*</para>
    /// <see cref="AVBufferRef.buffer" />
    /// </summary>
    public IntPtr Buffer
    {
        get => (IntPtr)_ptr->buffer;
        set => _ptr->buffer = (AVBuffer*)value;
    }
    
    /// <summary>
    /// <para>original type: byte*</para>
    /// <para>The data buffer. It is considered writable if and only if this is the only reference to the buffer, in which case av_buffer_is_writable() returns 1.</para>
    /// <see cref="AVBufferRef.data" />
    /// </summary>
    public DataPointer Data
    {
        get => new DataPointer(_ptr->data, (int)_ptr->size)!;
        set => ((IntPtr)(_ptr->data = (byte*)value.Pointer) + (int)(_ptr->size = (ulong)value.Length)).ToPointer();
    }
    
}
