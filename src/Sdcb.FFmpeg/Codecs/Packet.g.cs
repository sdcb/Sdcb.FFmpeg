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
/// <para>This structure stores compressed data. It is typically exported by demuxers and then passed as input to decoders, or received as output from encoders and then passed to muxers.</para>
/// <see cref="AVPacket" />
/// </summary>
public unsafe partial class Packet : SafeHandle
{
    protected AVPacket* _ptr => (AVPacket*)handle;
    
    public static implicit operator AVPacket*(Packet data) => data != null ? (AVPacket*)data.handle : null;
    
    protected Packet(AVPacket* ptr, bool isOwner): base(NativeUtils.NotNull((IntPtr)ptr), isOwner)
    {
    }
    
    public static Packet FromNative(AVPacket* ptr, bool isOwner) => new Packet(ptr, isOwner);
    
    internal static Packet FromNative(IntPtr ptr, bool isOwner) => new Packet((AVPacket*)ptr, isOwner);
    
    public static Packet? FromNativeOrNull(AVPacket* ptr, bool isOwner) => ptr == null ? null : new Packet(ptr, isOwner);
    
    public override bool IsInvalid => handle == IntPtr.Zero;
    
    /// <summary>
    /// <para>original type: AVBufferRef*</para>
    /// <para>A reference to the reference-counted buffer where the packet data is stored. May be NULL, then the packet data is not reference-counted.</para>
    /// <see cref="AVPacket.buf" />
    /// </summary>
    public BufferRef? Buf
    {
        get => BufferRef.FromNativeOrNull(_ptr->buf, false);
        set => _ptr->buf = value != null ? (AVBufferRef*)value : null;
    }
    
    /// <summary>
    /// <para>Presentation timestamp in AVStream-&gt;time_base units; the time at which the decompressed packet will be presented to the user. Can be AV_NOPTS_VALUE if it is not stored in the file. pts MUST be larger or equal to dts as presentation cannot happen before decompression, unless one wants to view hex dumps. Some formats misuse the terms dts and pts/cts to mean something different. Such timestamps must be converted to true pts/dts before they are stored in AVPacket.</para>
    /// <see cref="AVPacket.pts" />
    /// </summary>
    public long Pts
    {
        get => _ptr->pts;
        set => _ptr->pts = value;
    }
    
    /// <summary>
    /// <para>Decompression timestamp in AVStream-&gt;time_base units; the time at which the packet is decompressed. Can be AV_NOPTS_VALUE if it is not stored in the file.</para>
    /// <see cref="AVPacket.dts" />
    /// </summary>
    public long Dts
    {
        get => _ptr->dts;
        set => _ptr->dts = value;
    }
    
    /// <summary>
    /// <para>original type: byte*</para>
    /// <see cref="AVPacket.data" />
    /// </summary>
    public DataPointer Data
    {
        get => new DataPointer(_ptr->data, (int)_ptr->size)!;
        set => ((IntPtr)(_ptr->data = (byte*)value.Pointer) + (_ptr->size = value.Length)).ToPointer();
    }
    
    /// <summary>
    /// <see cref="AVPacket.stream_index" />
    /// </summary>
    public int StreamIndex
    {
        get => _ptr->stream_index;
        set => _ptr->stream_index = value;
    }
    
    /// <summary>
    /// <para>A combination of AV_PKT_FLAG values</para>
    /// <see cref="AVPacket.flags" />
    /// </summary>
    public int Flags
    {
        get => _ptr->flags;
        set => _ptr->flags = value;
    }
    
    /// <summary>
    /// <para>original type: AVPacketSideData*</para>
    /// <para>Additional packet data that can be provided by the container. Packet can contain several types of side information.</para>
    /// <see cref="AVPacket.side_data" />
    /// </summary>
    public PacketSideData? SideData
    {
        get => PacketSideData.FromNativeOrNull(_ptr->side_data);
        set => _ptr->side_data = (AVPacketSideData*)value;
    }
    
    /// <summary>
    /// <see cref="AVPacket.side_data_elems" />
    /// </summary>
    public int SideDataElems
    {
        get => _ptr->side_data_elems;
        set => _ptr->side_data_elems = value;
    }
    
    /// <summary>
    /// <para>Duration of this packet in AVStream-&gt;time_base units, 0 if unknown. Equals next_pts - this_pts in presentation order.</para>
    /// <see cref="AVPacket.duration" />
    /// </summary>
    public long Duration
    {
        get => _ptr->duration;
        set => _ptr->duration = value;
    }
    
    /// <summary>
    /// <para>byte position in stream, -1 if unknown</para>
    /// <see cref="AVPacket.pos" />
    /// </summary>
    public long Position
    {
        get => _ptr->pos;
        set => _ptr->pos = value;
    }
    
    /// <summary>
    /// <see cref="AVPacket.convergence_duration" />
    /// </summary>
    [Obsolete("Same as the duration field, but as int64_t. This was required for Matroska subtitles, whose duration values could overflow when the duration field was still an int.")]
    public long ConvergenceDuration
    {
        get => _ptr->convergence_duration;
        set => _ptr->convergence_duration = value;
    }
}
