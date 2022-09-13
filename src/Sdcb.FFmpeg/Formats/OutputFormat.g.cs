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
/// <see cref="AVOutputFormat" />
/// </summary>
public unsafe partial struct OutputFormat
{
    private AVOutputFormat* _ptr;
    
    public static implicit operator AVOutputFormat*(OutputFormat? data)
        => data.HasValue ? (AVOutputFormat*)data.Value._ptr : null;
    
    private OutputFormat(AVOutputFormat* ptr)
    {
        if (ptr == null)
        {
            throw new ArgumentNullException(nameof(ptr));
        }
        _ptr = ptr;
    }
    
    public static OutputFormat FromNative(AVOutputFormat* ptr) => new OutputFormat(ptr);
    public static OutputFormat FromNative(IntPtr ptr) => new OutputFormat((AVOutputFormat*)ptr);
    internal static OutputFormat? FromNativeOrNull(AVOutputFormat* ptr)
        => ptr != null ? new OutputFormat?(new OutputFormat(ptr)) : null;
    
    /// <summary>
    /// <para>original type: byte*</para>
    /// <see cref="AVOutputFormat.name" />
    /// </summary>
    public IntPtr Name
    {
        get => (IntPtr)_ptr->name;
        set => _ptr->name = (byte*)value;
    }
    
    /// <summary>
    /// <para>original type: byte*</para>
    /// <para>Descriptive name for the format, meant to be more human-readable than name. You should use the NULL_IF_CONFIG_SMALL() macro to define it.</para>
    /// <see cref="AVOutputFormat.long_name" />
    /// </summary>
    public IntPtr LongName
    {
        get => (IntPtr)_ptr->long_name;
        set => _ptr->long_name = (byte*)value;
    }
    
    /// <summary>
    /// <para>original type: byte*</para>
    /// <see cref="AVOutputFormat.mime_type" />
    /// </summary>
    public IntPtr MimeType
    {
        get => (IntPtr)_ptr->mime_type;
        set => _ptr->mime_type = (byte*)value;
    }
    
    /// <summary>
    /// <para>original type: byte*</para>
    /// <para>comma-separated filename extensions</para>
    /// <see cref="AVOutputFormat.extensions" />
    /// </summary>
    public IntPtr Extensions
    {
        get => (IntPtr)_ptr->extensions;
        set => _ptr->extensions = (byte*)value;
    }
    
    /// <summary>
    /// <para>default audio codec</para>
    /// <see cref="AVOutputFormat.audio_codec" />
    /// </summary>
    public AVCodecID AudioCodec
    {
        get => _ptr->audio_codec;
        set => _ptr->audio_codec = value;
    }
    
    /// <summary>
    /// <para>default video codec</para>
    /// <see cref="AVOutputFormat.video_codec" />
    /// </summary>
    public AVCodecID VideoCodec
    {
        get => _ptr->video_codec;
        set => _ptr->video_codec = value;
    }
    
    /// <summary>
    /// <para>default subtitle codec</para>
    /// <see cref="AVOutputFormat.subtitle_codec" />
    /// </summary>
    public AVCodecID SubtitleCodec
    {
        get => _ptr->subtitle_codec;
        set => _ptr->subtitle_codec = value;
    }
    
    /// <summary>
    /// <para>can use flags: AVFMT_NOFILE, AVFMT_NEEDNUMBER, AVFMT_GLOBALHEADER, AVFMT_NOTIMESTAMPS, AVFMT_VARIABLE_FPS, AVFMT_NODIMENSIONS, AVFMT_NOSTREAMS, AVFMT_ALLOW_FLUSH, AVFMT_TS_NONSTRICT, AVFMT_TS_NEGATIVE</para>
    /// <see cref="AVOutputFormat.flags" />
    /// </summary>
    public int Flags
    {
        get => _ptr->flags;
        set => _ptr->flags = value;
    }
    
    /// <summary>
    /// <para>List of supported codec_id-codec_tag pairs, ordered by "better choice first". The arrays are all terminated by AV_CODEC_ID_NONE.</para>
    /// <see cref="AVOutputFormat.codec_tag" />
    /// </summary>
    public AVCodecTag** CodecTag
    {
        get => _ptr->codec_tag;
        set => _ptr->codec_tag = value;
    }
    
    /// <summary>
    /// <para>original type: AVClass*</para>
    /// <para>AVClass for the private context</para>
    /// <see cref="AVOutputFormat.priv_class" />
    /// </summary>
    public FFmpegClass PrivClass
    {
        get => FFmpegClass.FromNative(_ptr->priv_class);
        set => _ptr->priv_class = (AVClass*)value;
    }
    
    /// <summary>
    /// <para>*************************************************************** No fields below this line are part of the public API. They may not be used outside of libavformat and can be changed and removed at will. New public fields should be added right above. ****************************************************************</para>
    /// <see cref="AVOutputFormat.priv_data_size" />
    /// </summary>
    public int PrivDataSize
    {
        get => _ptr->priv_data_size;
        set => _ptr->priv_data_size = value;
    }
    
    /// <summary>
    /// <para>Internal flags. See FF_FMT_FLAG_* in internal.h.</para>
    /// <see cref="AVOutputFormat.flags_internal" />
    /// </summary>
    public int FlagsInternal
    {
        get => _ptr->flags_internal;
        set => _ptr->flags_internal = value;
    }
    
    /// <summary>
    /// <see cref="AVOutputFormat.write_header" />
    /// </summary>
    public AVOutputFormat_write_header_func WriteHeader
    {
        get => _ptr->write_header;
        set => _ptr->write_header = value;
    }
    
    /// <summary>
    /// <para>Write a packet. If AVFMT_ALLOW_FLUSH is set in flags, pkt can be NULL in order to flush data buffered in the muxer. When flushing, return 0 if there still is more data to flush, or 1 if everything was flushed and there is no more buffered data.</para>
    /// <see cref="AVOutputFormat.write_packet" />
    /// </summary>
    public AVOutputFormat_write_packet_func WritePacket
    {
        get => _ptr->write_packet;
        set => _ptr->write_packet = value;
    }
    
    /// <summary>
    /// <see cref="AVOutputFormat.write_trailer" />
    /// </summary>
    public AVOutputFormat_write_trailer_func WriteTrailer
    {
        get => _ptr->write_trailer;
        set => _ptr->write_trailer = value;
    }
    
    /// <summary>
    /// <para>A format-specific function for interleavement. If unset, packets will be interleaved by dts.</para>
    /// <see cref="AVOutputFormat.interleave_packet" />
    /// </summary>
    public AVOutputFormat_interleave_packet_func InterleavePacket
    {
        get => _ptr->interleave_packet;
        set => _ptr->interleave_packet = value;
    }
    
    /// <summary>
    /// <para>Test if the given codec can be stored in this container.</para>
    /// <see cref="AVOutputFormat.query_codec" />
    /// </summary>
    public AVOutputFormat_query_codec_func QueryCodec
    {
        get => _ptr->query_codec;
        set => _ptr->query_codec = value;
    }
    
    /// <summary>
    /// <see cref="AVOutputFormat.get_output_timestamp" />
    /// </summary>
    public AVOutputFormat_get_output_timestamp_func GetOutputTimestamp
    {
        get => _ptr->get_output_timestamp;
        set => _ptr->get_output_timestamp = value;
    }
    
    /// <summary>
    /// <para>Allows sending messages from application to device.</para>
    /// <see cref="AVOutputFormat.control_message" />
    /// </summary>
    public AVOutputFormat_control_message_func ControlMessage
    {
        get => _ptr->control_message;
        set => _ptr->control_message = value;
    }
    
    /// <summary>
    /// <para>Write an uncoded AVFrame.</para>
    /// <see cref="AVOutputFormat.write_uncoded_frame" />
    /// </summary>
    public AVOutputFormat_write_uncoded_frame_func WriteUncodedFrame
    {
        get => _ptr->write_uncoded_frame;
        set => _ptr->write_uncoded_frame = value;
    }
    
    /// <summary>
    /// <para>Returns device list with it properties.</para>
    /// <see cref="AVOutputFormat.get_device_list" />
    /// </summary>
    public AVOutputFormat_get_device_list_func GetDeviceList
    {
        get => _ptr->get_device_list;
        set => _ptr->get_device_list = value;
    }
    
    /// <summary>
    /// <para>default data codec</para>
    /// <see cref="AVOutputFormat.data_codec" />
    /// </summary>
    public AVCodecID DataCodec
    {
        get => _ptr->data_codec;
        set => _ptr->data_codec = value;
    }
    
    /// <summary>
    /// <para>Initialize format. May allocate data here, and set any AVFormatContext or AVStream parameters that need to be set before packets are sent. This method must not write output.</para>
    /// <see cref="AVOutputFormat.init" />
    /// </summary>
    public AVOutputFormat_init_func Init
    {
        get => _ptr->init;
        set => _ptr->init = value;
    }
    
    /// <summary>
    /// <para>Deinitialize format. If present, this is called whenever the muxer is being destroyed, regardless of whether or not the header has been written.</para>
    /// <see cref="AVOutputFormat.deinit" />
    /// </summary>
    public AVOutputFormat_deinit_func Deinit
    {
        get => _ptr->deinit;
        set => _ptr->deinit = value;
    }
    
    /// <summary>
    /// <para>Set up any necessary bitstream filtering and extract any extra data needed for the global header.</para>
    /// <see cref="AVOutputFormat.check_bitstream" />
    /// </summary>
    public AVOutputFormat_check_bitstream_func CheckBitstream
    {
        get => _ptr->check_bitstream;
        set => _ptr->check_bitstream = value;
    }
}
