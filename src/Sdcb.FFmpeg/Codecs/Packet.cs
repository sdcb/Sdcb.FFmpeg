using Sdcb.FFmpeg.Common;
using Sdcb.FFmpeg.Raw;
using Sdcb.FFmpeg.Utils;
using System;
using System.Runtime.InteropServices;
using static Sdcb.FFmpeg.Raw.ffmpeg;

namespace Sdcb.FFmpeg.Codecs;

public unsafe partial class Packet : SafeHandle, IBufferRefed
{
    /// <summary>
    /// <see cref="av_packet_alloc"/>
    /// </summary>
    public Packet() : base(NativeUtils.NotNull((IntPtr)av_packet_alloc()), ownsHandle: true)
    {
    }

    protected override bool ReleaseHandle()
    {
        Free();
        return true;
    }

    /// <summary>
    /// <see cref="av_packet_clone(AVPacket*)"/>
    /// </summary>
    /// <returns></returns>
    public Packet Clone() => FromNative(av_packet_clone(this), isOwner: true);

    /// <summary>
    /// Initialize optional fields of a packet with default values.
    /// This function is deprecated. Once it's removed, sizeof(AVPacket) will not be a part of the ABI anymore.
    /// <see cref="av_init_packet(AVPacket*)"/>
    /// </summary>
    [Obsolete]
    private void Initialize() => av_init_packet(this);

    /// <summary>
    /// <see cref="av_new_packet(AVPacket*, int)"/>
    /// </summary>
    private void AllocBuffer(int size) => av_new_packet(this, size);

    /// <summary>
    /// <see cref="av_shrink_packet(AVPacket*, int)"/>
    /// </summary>
    public void Shink(int size) => av_shrink_packet(this, size);

    /// <summary>
    /// <see cref="av_grow_packet(AVPacket*, int)"/>
    /// </summary>
    public void Grow(int growBy) => av_grow_packet(this, growBy);


    /// <summary>
    /// <see cref="av_packet_from_data(AVPacket*, byte*, int)"/>
    /// </summary>
    public void FromData(Span<byte> data)
    {
        fixed (byte* ptr = data)
        {
            av_packet_from_data(this, ptr, data.Length).ThrowIfError();
        }
    }

    /// <summary>
    /// <see cref="av_packet_new_side_data(AVPacket*, AVPacketSideDataType, ulong)"/>
    /// </summary>
    public IntPtr NewSideData(AVPacketSideDataType type, long size) => (IntPtr)av_packet_new_side_data(this, (AVPacketSideDataType)type, (ulong)size);

    /// <summary>
    /// <see cref="av_packet_add_side_data(AVPacket*, AVPacketSideDataType, byte*, ulong)"/>
    /// </summary>\
    public void AddSideData(AVPacketSideDataType type, Span<byte> data)
    {
        fixed (byte* ptr = data)
        {
            av_packet_add_side_data(this, (AVPacketSideDataType)type, ptr, (ulong)data.Length).ThrowIfError();
        }
    }

    /// <summary>
    /// <see cref="av_packet_shrink_side_data(AVPacket*, AVPacketSideDataType, ulong)"/>
    /// </summary>
    public void ShinkSideData(AVPacketSideDataType type, long size) => av_packet_shrink_side_data(this, (AVPacketSideDataType)type, (ulong)size).ThrowIfError();

    /// <summary>
    /// <see cref="av_packet_get_side_data(AVPacket*, AVPacketSideDataType, ulong*)"/>
    /// </summary>
    public DataPointer GetSideData(AVPacketSideDataType type)
    {
        ulong size;
        return new DataPointer(av_packet_get_side_data(this, (AVPacketSideDataType)type, &size), (int)size);
    }

    /// <summary>
    /// <see cref="av_packet_free_side_data(AVPacket*)"/>
    /// </summary>
    public void FreeSideData() => av_packet_free_side_data(this);

    /// <summary>
    /// <see cref="av_packet_ref(AVPacket*, AVPacket*)"/>
    /// </summary>
    public void Ref(Packet dest) => av_packet_ref(dest, this);

    /// <summary>
    /// <see cref="av_packet_unref(AVPacket*)"/>
    /// </summary>
    public void Unref() => av_packet_unref(this);

    /// <summary>
    /// <see cref="av_packet_move_ref(AVPacket*, AVPacket*)"/>
    /// </summary>
    public void MoveRef(Packet dest) => av_packet_move_ref(dest, this);

    /// <summary>
    /// <see cref="av_packet_copy_props(AVPacket*, AVPacket*)"/>
    /// </summary>
    public void CopyPropsFrom(Packet source) => av_packet_copy_props(this, source);

    /// <summary>
    /// <see cref="av_packet_make_refcounted(AVPacket*)"/>
    /// </summary>
    public void MakeRefCounted() => av_packet_make_refcounted(this).ThrowIfError();

    /// <summary>
    /// <see cref="av_packet_make_writable(AVPacket*)"/>
    /// </summary>
    public void MakeWritable() => av_packet_make_writable(this).ThrowIfError();

    /// <summary>
    /// <see cref="av_packet_rescale_ts(AVPacket*, AVRational, AVRational)"/>
    /// </summary>
    public void RescaleTimestamp(AVRational source, AVRational dest) => av_packet_rescale_ts(this, source, dest);

    /// <summary>
    /// <see cref="av_packet_free(AVPacket**)"/>
    /// </summary>
    public void Free()
    {
        AVPacket* ptr = this;
        av_packet_free(&ptr);
        handle = (IntPtr)ptr;
    }

    /// <summary>
    /// <see cref="av_packet_side_data_name(AVPacketSideDataType)"/>
    /// </summary>
    public static string GetSideDataName(AVPacketSideDataType type) => av_packet_side_data_name(type);

    /// <summary>
    /// <see cref="av_packet_pack_dictionary(AVDictionary*, ulong*)"/>
    /// </summary>
    public static DataPointer PackDirectory(MediaDictionary dict)
    {
        ulong size;
        return new DataPointer(av_packet_pack_dictionary(dict, &size), (int)size);
    }

    /// <summary>
    /// <see cref="av_packet_unpack_dictionary(byte*, ulong, AVDictionary**)"/>
    /// </summary>
    public static MediaDictionary UnpackDictionary(Span<byte> data)
    {
        AVDictionary* dict = null;
        fixed (byte* ptr = data)
        {
            av_packet_unpack_dictionary(ptr, (ulong)data.Length, &dict).ThrowIfError();
        }
        return MediaDictionary.FromNative(dict, isOwner: true);
    }

    void IBufferRefed.Ref(IBufferRefed other)
    {
        if (other is Packet packet)
        {
            Ref(packet);
        }
        throw new ArgumentException($"{other} is not a packet.");
    }

    void IBufferRefed.Unref() => Unref();

    void IBufferRefed.MakeWritable() => MakeWritable();

    IBufferRefed IBufferRefed.Clone() => Clone();
}
