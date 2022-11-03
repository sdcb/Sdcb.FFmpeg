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
/// <para>Structure to hold side data for an AVFrame.</para>
/// <see cref="AVFrameSideData" />
/// </summary>
public unsafe partial struct FrameSideData
{
    private AVFrameSideData* _ptr;
    
    public static implicit operator AVFrameSideData*(FrameSideData? data)
        => data.HasValue ? (AVFrameSideData*)data.Value._ptr : null;
    
    private FrameSideData(AVFrameSideData* ptr)
    {
        if (ptr == null)
        {
            throw new ArgumentNullException(nameof(ptr));
        }
        _ptr = ptr;
    }
    
    public static FrameSideData FromNative(AVFrameSideData* ptr) => new FrameSideData(ptr);
    public static FrameSideData FromNative(IntPtr ptr) => new FrameSideData((AVFrameSideData*)ptr);
    internal static FrameSideData? FromNativeOrNull(AVFrameSideData* ptr)
        => ptr != null ? new FrameSideData?(new FrameSideData(ptr)) : null;
    
    /// <summary>
    /// <see cref="AVFrameSideData.type" />
    /// </summary>
    public AVFrameSideDataType Type
    {
        get => _ptr->type;
        set => _ptr->type = value;
    }
    
    /// <summary>
    /// <para>original type: byte*</para>
    /// <see cref="AVFrameSideData.data" />
    /// </summary>
    public DataPointer Data
    {
        get => new DataPointer(_ptr->data, (int)_ptr->size)!;
        set => ((IntPtr)(_ptr->data = (byte*)value.Pointer) + (int)(_ptr->size = (ulong)value.Length)).ToPointer();
    }
    
    /// <summary>
    /// <para>original type: AVDictionary*</para>
    /// <see cref="AVFrameSideData.metadata" />
    /// </summary>
    public MediaDictionary Metadata
    {
        get => MediaDictionary.FromNative(_ptr->metadata, false);
        set => _ptr->metadata = (AVDictionary*)value;
    }
    
    /// <summary>
    /// <para>original type: AVBufferRef*</para>
    /// <see cref="AVFrameSideData.buf" />
    /// </summary>
    public BufferRef? Buf
    {
        get => BufferRef.FromNativeOrNull(_ptr->buf, false);
        set => _ptr->buf = value != null ? (AVBufferRef*)value : null;
    }
}
