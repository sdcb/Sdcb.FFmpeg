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
    /// <para>original type: AVClass*</para>
    /// <para>A class for avoptions. Set on stream creation.</para>
    /// <see cref="AVStream.av_class" />
    /// </summary>
    public FFmpegClass AvClass
    {
        get => FFmpegClass.FromNative(_ptr->av_class);
        set => _ptr->av_class = (AVClass*)value;
    }
    
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
    /// <para>Stream disposition - a combination of AV_DISPOSITION_* flags. - demuxing: set by libavformat when creating the stream or in avformat_find_stream_info(). - muxing: may be set by the caller before avformat_write_header().</para>
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
    [Obsolete("use AVStream's \"codecpar side data\".")]
    public PacketSideData? SideData
    {
        get => PacketSideData.FromNativeOrNull(_ptr->side_data);
        set => _ptr->side_data = (AVPacketSideData*)value;
    }
    
    /// <summary>
    /// <para>The number of elements in the AVStream.side_data array.</para>
    /// <see cref="AVStream.nb_side_data" />
    /// </summary>
    [Obsolete("use AVStream's \"codecpar side data\".")]
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
    /// <para>Number of bits in timestamps. Used for wrapping control.</para>
    /// <see cref="AVStream.pts_wrap_bits" />
    /// </summary>
    public int PtsWrapBits
    {
        get => _ptr->pts_wrap_bits;
        set => _ptr->pts_wrap_bits = value;
    }
}
