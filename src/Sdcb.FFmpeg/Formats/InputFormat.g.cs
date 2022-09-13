// This file was genereated from Sdcb.FFmpeg.AutoGen, DO NOT CHANGE DIRECTLY.
#nullable enable
using Sdcb.FFmpeg.Common;
using Sdcb.FFmpeg.Codecs;
using Sdcb.FFmpeg.Raw;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Sdcb.FFmpeg.Formats;

/// <summary>
/// <para>@{</para>
/// <see cref="AVInputFormat" />
/// </summary>
public unsafe partial struct InputFormat
{
    private AVInputFormat* _ptr;
    
    public static implicit operator AVInputFormat*(InputFormat? data)
        => data.HasValue ? (AVInputFormat*)data.Value._ptr : null;
    
    private InputFormat(AVInputFormat* ptr)
    {
        if (ptr == null)
        {
            throw new ArgumentNullException(nameof(ptr));
        }
        _ptr = ptr;
    }
    
    public static InputFormat FromNative(AVInputFormat* ptr) => new InputFormat(ptr);
    public static InputFormat FromNative(IntPtr ptr) => new InputFormat((AVInputFormat*)ptr);
    internal static InputFormat? FromNativeOrNull(AVInputFormat* ptr)
        => ptr != null ? new InputFormat?(new InputFormat(ptr)) : null;
    
    /// <summary>
    /// <para>original type: byte*</para>
    /// <para>A comma separated list of short names for the format. New names may be appended with a minor bump.</para>
    /// <see cref="AVInputFormat.name" />
    /// </summary>
    public IntPtr Name
    {
        get => (IntPtr)_ptr->name;
        set => _ptr->name = (byte*)value;
    }
    
    /// <summary>
    /// <para>original type: byte*</para>
    /// <para>Descriptive name for the format, meant to be more human-readable than name. You should use the NULL_IF_CONFIG_SMALL() macro to define it.</para>
    /// <see cref="AVInputFormat.long_name" />
    /// </summary>
    public IntPtr LongName
    {
        get => (IntPtr)_ptr->long_name;
        set => _ptr->long_name = (byte*)value;
    }
    
    /// <summary>
    /// <para>Can use flags: AVFMT_NOFILE, AVFMT_NEEDNUMBER, AVFMT_SHOW_IDS, AVFMT_NOTIMESTAMPS, AVFMT_GENERIC_INDEX, AVFMT_TS_DISCONT, AVFMT_NOBINSEARCH, AVFMT_NOGENSEARCH, AVFMT_NO_BYTE_SEEK, AVFMT_SEEK_TO_PTS.</para>
    /// <see cref="AVInputFormat.flags" />
    /// </summary>
    public int Flags
    {
        get => _ptr->flags;
        set => _ptr->flags = value;
    }
    
    /// <summary>
    /// <para>original type: byte*</para>
    /// <para>If extensions are defined, then no probe is done. You should usually not use extension format guessing because it is not reliable enough</para>
    /// <see cref="AVInputFormat.extensions" />
    /// </summary>
    public IntPtr Extensions
    {
        get => (IntPtr)_ptr->extensions;
        set => _ptr->extensions = (byte*)value;
    }
    
    /// <summary>
    /// <see cref="AVInputFormat.codec_tag" />
    /// </summary>
    public AVCodecTag** CodecTag
    {
        get => _ptr->codec_tag;
        set => _ptr->codec_tag = value;
    }
    
    /// <summary>
    /// <para>original type: AVClass*</para>
    /// <para>AVClass for the private context</para>
    /// <see cref="AVInputFormat.priv_class" />
    /// </summary>
    public FFmpegClass PrivClass
    {
        get => FFmpegClass.FromNative(_ptr->priv_class);
        set => _ptr->priv_class = (AVClass*)value;
    }
    
    /// <summary>
    /// <para>original type: byte*</para>
    /// <para>Comma-separated list of mime types. It is used check for matching mime types while probing.</para>
    /// <see cref="AVInputFormat.mime_type" />
    /// </summary>
    public IntPtr MimeType
    {
        get => (IntPtr)_ptr->mime_type;
        set => _ptr->mime_type = (byte*)value;
    }
    
    /// <summary>
    /// <para>*************************************************************** No fields below this line are part of the public API. They may not be used outside of libavformat and can be changed and removed at will. New public fields should be added right above. ****************************************************************</para>
    /// <see cref="AVInputFormat.raw_codec_id" />
    /// </summary>
    public int RawCodecId
    {
        get => _ptr->raw_codec_id;
        set => _ptr->raw_codec_id = value;
    }
    
    /// <summary>
    /// <para>Size of private data so that it can be allocated in the wrapper.</para>
    /// <see cref="AVInputFormat.priv_data_size" />
    /// </summary>
    public int PrivDataSize
    {
        get => _ptr->priv_data_size;
        set => _ptr->priv_data_size = value;
    }
    
    /// <summary>
    /// <para>Internal flags. See FF_FMT_FLAG_* in internal.h.</para>
    /// <see cref="AVInputFormat.flags_internal" />
    /// </summary>
    public int FlagsInternal
    {
        get => _ptr->flags_internal;
        set => _ptr->flags_internal = value;
    }
    
    /// <summary>
    /// <para>Tell if a given file has a chance of being parsed as this format. The buffer provided is guaranteed to be AVPROBE_PADDING_SIZE bytes big so you do not have to check for that unless you need more.</para>
    /// <see cref="AVInputFormat.read_probe" />
    /// </summary>
    public AVInputFormat_read_probe_func ReadProbe
    {
        get => _ptr->read_probe;
        set => _ptr->read_probe = value;
    }
    
    /// <summary>
    /// <para>Read the format header and initialize the AVFormatContext structure. Return 0 if OK. 'avformat_new_stream' should be called to create new streams.</para>
    /// <see cref="AVInputFormat.read_header" />
    /// </summary>
    public AVInputFormat_read_header_func ReadHeader
    {
        get => _ptr->read_header;
        set => _ptr->read_header = value;
    }
    
    /// <summary>
    /// <para>Read one packet and put it in 'pkt'. pts and flags are also set. 'avformat_new_stream' can be called only if the flag AVFMTCTX_NOHEADER is used and only in the calling thread (not in a background thread).</para>
    /// <see cref="AVInputFormat.read_packet" />
    /// </summary>
    public AVInputFormat_read_packet_func ReadPacket
    {
        get => _ptr->read_packet;
        set => _ptr->read_packet = value;
    }
    
    /// <summary>
    /// <para>Close the stream. The AVFormatContext and AVStreams are not freed by this function</para>
    /// <see cref="AVInputFormat.read_close" />
    /// </summary>
    public AVInputFormat_read_close_func ReadClose
    {
        get => _ptr->read_close;
        set => _ptr->read_close = value;
    }
    
    /// <summary>
    /// <para>Seek to a given timestamp relative to the frames in stream component stream_index.</para>
    /// <see cref="AVInputFormat.read_seek" />
    /// </summary>
    public AVInputFormat_read_seek_func ReadSeek
    {
        get => _ptr->read_seek;
        set => _ptr->read_seek = value;
    }
    
    /// <summary>
    /// <para>Get the next timestamp in stream[stream_index].time_base units.</para>
    /// <see cref="AVInputFormat.read_timestamp" />
    /// </summary>
    public AVInputFormat_read_timestamp_func ReadTimestamp
    {
        get => _ptr->read_timestamp;
        set => _ptr->read_timestamp = value;
    }
    
    /// <summary>
    /// <para>Start/resume playing - only meaningful if using a network-based format (RTSP).</para>
    /// <see cref="AVInputFormat.read_play" />
    /// </summary>
    public AVInputFormat_read_play_func ReadPlay
    {
        get => _ptr->read_play;
        set => _ptr->read_play = value;
    }
    
    /// <summary>
    /// <para>Pause playing - only meaningful if using a network-based format (RTSP).</para>
    /// <see cref="AVInputFormat.read_pause" />
    /// </summary>
    public AVInputFormat_read_pause_func ReadPause
    {
        get => _ptr->read_pause;
        set => _ptr->read_pause = value;
    }
    
    /// <summary>
    /// <para>Seek to timestamp ts. Seeking will be done so that the point from which all active streams can be presented successfully will be closest to ts and within min/max_ts. Active streams are all streams that have AVStream.discard &lt; AVDISCARD_ALL.</para>
    /// <see cref="AVInputFormat.read_seek2" />
    /// </summary>
    public AVInputFormat_read_seek2_func ReadSeek2
    {
        get => _ptr->read_seek2;
        set => _ptr->read_seek2 = value;
    }
    
    /// <summary>
    /// <para>Returns device list with it properties.</para>
    /// <see cref="AVInputFormat.get_device_list" />
    /// </summary>
    public AVInputFormat_get_device_list_func GetDeviceList
    {
        get => _ptr->get_device_list;
        set => _ptr->get_device_list = value;
    }
}
