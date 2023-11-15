// This file was genereated from Sdcb.FFmpeg.AutoGen, DO NOT CHANGE DIRECTLY.
#nullable enable
using Sdcb.FFmpeg.Common;
using Sdcb.FFmpeg.Formats;
using Sdcb.FFmpeg.Utils;
using Sdcb.FFmpeg.Filters;
using Sdcb.FFmpeg.Raw;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Sdcb.FFmpeg.Codecs;

/// <summary>
/// <para>This structure stores auxiliary information for decoding, presenting, or otherwise processing the coded stream. It is typically exported by demuxers and encoders and can be fed to decoders and muxers either in a per packet basis, or as global side data (applying to the entire coded stream).</para>
/// <see cref="AVPacketSideData" />
/// </summary>
public unsafe partial struct PacketSideData
{
    private AVPacketSideData* _ptr;
    
    public static implicit operator AVPacketSideData*(PacketSideData? data)
        => data.HasValue ? (AVPacketSideData*)data.Value._ptr : null;
    
    private PacketSideData(AVPacketSideData* ptr)
    {
        if (ptr == null)
        {
            throw new ArgumentNullException(nameof(ptr));
        }
        _ptr = ptr;
    }
    
    public static PacketSideData FromNative(AVPacketSideData* ptr) => new PacketSideData(ptr);
    public static PacketSideData FromNative(IntPtr ptr) => new PacketSideData((AVPacketSideData*)ptr);
    internal static PacketSideData? FromNativeOrNull(AVPacketSideData* ptr)
        => ptr != null ? new PacketSideData?(new PacketSideData(ptr)) : null;
    
    /// <summary>
    /// <para>original type: byte*</para>
    /// <see cref="AVPacketSideData.data" />
    /// </summary>
    public DataPointer Data
    {
        get => new DataPointer(_ptr->data, (int)_ptr->size)!;
        set => ((IntPtr)(_ptr->data = (byte*)value.Pointer) + (int)(_ptr->size = (ulong)value.Length)).ToPointer();
    }
    
    /// <summary>
    /// <see cref="AVPacketSideData.type" />
    /// </summary>
    public AVPacketSideDataType Type
    {
        get => _ptr->type;
        set => _ptr->type = value;
    }
}
