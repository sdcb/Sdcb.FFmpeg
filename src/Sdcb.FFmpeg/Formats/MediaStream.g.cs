// This file was genereated from Sdcb.FFmpeg.AutoGen, DO NOT CHANGE DIRECTLY.
#nullable enable
using Sdcb.FFmpeg.Common;
using Sdcb.FFmpeg.Codecs;
using Sdcb.FFmpeg.Utils;
using Sdcb.FFmpeg.Filters;
using Sdcb.FFmpeg.Raw;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Sdcb.FFmpeg.Formats;

/// <summary>
/// <para>Stream structure. New fields can be added to the end with minor version bumps. Removal, reordering and changes to existing fields require a major version bump. sizeof(AVStream) must not be used outside libav*.</para>
/// <see cref="AVStream" />
/// </summary>
public unsafe partial struct MediaStream
{
    private AVStream* _ptr;
    
    public static implicit operator AVStream*(MediaStream? data)
        => data.HasValue ? (AVStream*)data.Value._ptr : null;
    
    private MediaStream(AVStream* ptr)
    {
        if (ptr == null)
        {
            throw new ArgumentNullException(nameof(ptr));
        }
        _ptr = ptr;
    }
    
    public static MediaStream FromNative(AVStream* ptr) => new MediaStream(ptr);
    public static MediaStream FromNative(IntPtr ptr) => new MediaStream((AVStream*)ptr);
    internal static MediaStream? FromNativeOrNull(AVStream* ptr)
        => ptr != null ? new MediaStream?(new MediaStream(ptr)) : null;
    
    /// <summary>
    /// <para>stream index in AVFormatContext</para>
    /// <see cref="AVStream.index" />
    /// </summary>
    public int Index
    {
        get => _ptr->index;
        set => _ptr->index = value;
    }
    
    /// <summary>
    /// <para>Format-specific stream ID. decoding: set by libavformat encoding: set by the user, replaced by libavformat if left unset</para>
    /// <see cref="AVStream.id" />
    /// </summary>
    public int Id
    {
        get => _ptr->id;
        set => _ptr->id = value;
    }
    
    /// <summary>
    /// <para>original type: AVCodecContext*</para>
    /// <see cref="AVStream.codec" />
    /// </summary>
    [Obsolete("use the codecpar struct instead")]
    public CodecContext Codec
    {
        get => CodecContext.FromNative(_ptr->codec, false);
        set => _ptr->codec = (AVCodecContext*)value;
    }
    
    /// <summary>
    /// <para>original type: void*</para>
    /// <see cref="AVStream.priv_data" />
    /// </summary>
    public IntPtr PrivateData
    {
        get => (IntPtr)_ptr->priv_data;
        set => _ptr->priv_data = (void*)value;
    }
    
    /// <summary>
    /// <para>This is the fundamental unit of time (in seconds) in terms of which frame timestamps are represented.</para>
    /// <see cref="AVStream.time_base" />
    /// </summary>
    public AVRational TimeBase
    {
        get => _ptr->time_base;
        set => _ptr->time_base = value;
    }
    
    /// <summary>
    /// <para>Decoding: pts of the first frame of the stream in presentation order, in stream time base. Only set this if you are absolutely 100% sure that the value you set it to really is the pts of the first frame. This may be undefined (AV_NOPTS_VALUE).</para>
    /// <see cref="AVStream.start_time" />
    /// </summary>
    public long StartTime
    {
        get => _ptr->start_time;
        set => _ptr->start_time = value;
    }
    
    /// <summary>
    /// <para>Decoding: duration of the stream, in stream time base. If a source file does not specify a duration, but does specify a bitrate, this value will be estimated from bitrate and file size.</para>
    /// <see cref="AVStream.duration" />
    /// </summary>
    public long Duration
    {
        get => _ptr->duration;
        set => _ptr->duration = value;
    }
    
    /// <summary>
    /// <para>number of frames in this stream if known or 0</para>
    /// <see cref="AVStream.nb_frames" />
    /// </summary>
    public long NbFrames
    {
        get => _ptr->nb_frames;
        set => _ptr->nb_frames = value;
    }
    
    /// <summary>
    /// <para>AV_DISPOSITION_* bit field</para>
    /// <see cref="AVStream.disposition" />
    /// </summary>
    public int Disposition
    {
        get => _ptr->disposition;
        set => _ptr->disposition = value;
    }
    
    /// <summary>
    /// <para>Selects which packets can be discarded at will and do not need to be demuxed.</para>
    /// <see cref="AVStream.discard" />
    /// </summary>
    public AVDiscard Discard
    {
        get => _ptr->discard;
        set => _ptr->discard = value;
    }
    
    /// <summary>
    /// <para>sample aspect ratio (0 if unknown) - encoding: Set by user. - decoding: Set by libavformat.</para>
    /// <see cref="AVStream.sample_aspect_ratio" />
    /// </summary>
    public AVRational SampleAspectRatio
    {
        get => _ptr->sample_aspect_ratio;
        set => _ptr->sample_aspect_ratio = value;
    }
    
    /// <summary>
    /// <para>original type: AVDictionary*</para>
    /// <see cref="AVStream.metadata" />
    /// </summary>
    public MediaDictionary Metadata
    {
        get => MediaDictionary.FromNative(_ptr->metadata, false);
        set => _ptr->metadata = (AVDictionary*)value;
    }
    
    /// <summary>
    /// <para>Average framerate</para>
    /// <see cref="AVStream.avg_frame_rate" />
    /// </summary>
    public AVRational AvgFrameRate
    {
        get => _ptr->avg_frame_rate;
        set => _ptr->avg_frame_rate = value;
    }
    
    /// <summary>
    /// <para>For streams with AV_DISPOSITION_ATTACHED_PIC disposition, this packet will contain the attached picture.</para>
    /// <see cref="AVStream.attached_pic" />
    /// </summary>
    public AVPacket AttachedPic
    {
        get => _ptr->attached_pic;
        set => _ptr->attached_pic = value;
    }
    
    /// <summary>
    /// <para>original type: AVPacketSideData*</para>
    /// <para>An array of side data that applies to the whole stream (i.e. the container does not allow it to change between packets).</para>
    /// <see cref="AVStream.side_data" />
    /// </summary>
    public PacketSideData? SideData
    {
        get => PacketSideData.FromNativeOrNull(_ptr->side_data);
        set => _ptr->side_data = (AVPacketSideData*)value;
    }
    
    /// <summary>
    /// <para>The number of elements in the AVStream.side_data array.</para>
    /// <see cref="AVStream.nb_side_data" />
    /// </summary>
    public int NbSideData
    {
        get => _ptr->nb_side_data;
        set => _ptr->nb_side_data = value;
    }
    
    /// <summary>
    /// <para>Flags indicating events happening on the stream, a combination of AVSTREAM_EVENT_FLAG_*.</para>
    /// <see cref="AVStream.event_flags" />
    /// </summary>
    public int EventFlags
    {
        get => _ptr->event_flags;
        set => _ptr->event_flags = value;
    }
    
    /// <summary>
    /// <para>Real base framerate of the stream. This is the lowest framerate with which all timestamps can be represented accurately (it is the least common multiple of all framerates in the stream). Note, this value is just a guess! For example, if the time base is 1/90000 and all frames have either approximately 3600 or 1800 timer ticks, then r_frame_rate will be 50/1.</para>
    /// <see cref="AVStream.r_frame_rate" />
    /// </summary>
    public AVRational RFrameRate
    {
        get => _ptr->r_frame_rate;
        set => _ptr->r_frame_rate = value;
    }
    
    /// <summary>
    /// <para>original type: byte*</para>
    /// <para>String containing pairs of key and values describing recommended encoder configuration. Pairs are separated by ','. Keys are separated from values by '='.</para>
    /// <see cref="AVStream.recommended_encoder_configuration" />
    /// </summary>
    [Obsolete("unused")]
    public IntPtr RecommendedEncoderConfiguration
    {
        get => (IntPtr)_ptr->recommended_encoder_configuration;
        set => _ptr->recommended_encoder_configuration = (byte*)value;
    }
    
    /// <summary>
    /// <para>original type: AVCodecParameters*</para>
    /// <para>Codec parameters associated with this stream. Allocated and freed by libavformat in avformat_new_stream() and avformat_free_context() respectively.</para>
    /// <see cref="AVStream.codecpar" />
    /// </summary>
    public CodecParameters? Codecpar
    {
        get => CodecParameters.FromNativeOrNull(_ptr->codecpar, false);
        set => _ptr->codecpar = value != null ? (AVCodecParameters*)value : null;
    }
    
    /// <summary>
    /// <para>original type: void*</para>
    /// <see cref="AVStream.unused" />
    /// </summary>
    public IntPtr Unused
    {
        get => (IntPtr)_ptr->unused;
        set => _ptr->unused = (void*)value;
    }
    
    /// <summary>
    /// <para>number of bits in pts (used for wrapping control)</para>
    /// <see cref="AVStream.pts_wrap_bits" />
    /// </summary>
    public int PtsWrapBits
    {
        get => _ptr->pts_wrap_bits;
        set => _ptr->pts_wrap_bits = value;
    }
    
    /// <summary>
    /// <para>Timestamp corresponding to the last dts sync point.</para>
    /// <see cref="AVStream.first_dts" />
    /// </summary>
    public long FirstDts
    {
        get => _ptr->first_dts;
        set => _ptr->first_dts = value;
    }
    
    /// <summary>
    /// <see cref="AVStream.cur_dts" />
    /// </summary>
    public long CurDts
    {
        get => _ptr->cur_dts;
        set => _ptr->cur_dts = value;
    }
    
    /// <summary>
    /// <see cref="AVStream.last_IP_pts" />
    /// </summary>
    public long LastIpPts
    {
        get => _ptr->last_IP_pts;
        set => _ptr->last_IP_pts = value;
    }
    
    /// <summary>
    /// <see cref="AVStream.last_IP_duration" />
    /// </summary>
    public int LastIpDuration
    {
        get => _ptr->last_IP_duration;
        set => _ptr->last_IP_duration = value;
    }
    
    /// <summary>
    /// <para>Number of packets to buffer for codec probing</para>
    /// <see cref="AVStream.probe_packets" />
    /// </summary>
    public int ProbePackets
    {
        get => _ptr->probe_packets;
        set => _ptr->probe_packets = value;
    }
    
    /// <summary>
    /// <para>Number of frames that have been demuxed during avformat_find_stream_info()</para>
    /// <see cref="AVStream.codec_info_nb_frames" />
    /// </summary>
    public int CodecInfoNbFrames
    {
        get => _ptr->codec_info_nb_frames;
        set => _ptr->codec_info_nb_frames = value;
    }
    
    /// <summary>
    /// <see cref="AVStream.need_parsing" />
    /// </summary>
    public AVStreamParseType NeedParsing
    {
        get => _ptr->need_parsing;
        set => _ptr->need_parsing = value;
    }
    
    /// <summary>
    /// <para>original type: AVCodecParserContext*</para>
    /// <see cref="AVStream.parser" />
    /// </summary>
    public CodecParserContext Parser
    {
        get => CodecParserContext.FromNative(_ptr->parser, false);
        set => _ptr->parser = (AVCodecParserContext*)value;
    }
    
    /// <summary>
    /// <para>original type: void*</para>
    /// <see cref="AVStream.unused7" />
    /// </summary>
    public IntPtr Unused7
    {
        get => (IntPtr)_ptr->unused7;
        set => _ptr->unused7 = (void*)value;
    }
    
    /// <summary>
    /// <see cref="AVStream.unused6" />
    /// </summary>
    public AVProbeData Unused6
    {
        get => _ptr->unused6;
        set => _ptr->unused6 = value;
    }
    
    /// <summary>
    /// <see cref="AVStream.unused5" />
    /// </summary>
    public long_array17 Unused5
    {
        get => _ptr->unused5;
        set => _ptr->unused5 = value;
    }
    
    /// <summary>
    /// <para>Only used if the format does not support seeking natively.</para>
    /// <see cref="AVStream.index_entries" />
    /// </summary>
    public AVIndexEntry* IndexEntries
    {
        get => _ptr->index_entries;
        set => _ptr->index_entries = value;
    }
    
    /// <summary>
    /// <see cref="AVStream.nb_index_entries" />
    /// </summary>
    public int NbIndexEntries
    {
        get => _ptr->nb_index_entries;
        set => _ptr->nb_index_entries = value;
    }
    
    /// <summary>
    /// <see cref="AVStream.index_entries_allocated_size" />
    /// </summary>
    public uint IndexEntriesAllocatedSize
    {
        get => _ptr->index_entries_allocated_size;
        set => _ptr->index_entries_allocated_size = value;
    }
    
    /// <summary>
    /// <para>Stream Identifier This is the MPEG-TS stream identifier +1 0 means unknown</para>
    /// <see cref="AVStream.stream_identifier" />
    /// </summary>
    public int StreamIdentifier
    {
        get => _ptr->stream_identifier;
        set => _ptr->stream_identifier = value;
    }
    
    /// <summary>
    /// <see cref="AVStream.unused8" />
    /// </summary>
    public int Unused8
    {
        get => _ptr->unused8;
        set => _ptr->unused8 = value;
    }
    
    /// <summary>
    /// <see cref="AVStream.unused9" />
    /// </summary>
    public int Unused9
    {
        get => _ptr->unused9;
        set => _ptr->unused9 = value;
    }
    
    /// <summary>
    /// <see cref="AVStream.unused10" />
    /// </summary>
    public int Unused10
    {
        get => _ptr->unused10;
        set => _ptr->unused10 = value;
    }
    
    /// <summary>
    /// <para>An opaque field for libavformat internal usage. Must not be accessed in any way by callers.</para>
    /// <see cref="AVStream.@internal" />
    /// </summary>
    public AVStreamInternal* Internal
    {
        get => _ptr->@internal;
        set => _ptr->@internal = value;
    }
}
