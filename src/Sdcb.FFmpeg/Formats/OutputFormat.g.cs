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
    public string Name => PtrExtensions.PtrToStringUTF8((IntPtr)_ptr->name)!;
    
    /// <summary>
    /// <para>original type: byte*</para>
    /// <para>Descriptive name for the format, meant to be more human-readable than name. You should use the NULL_IF_CONFIG_SMALL() macro to define it.</para>
    /// <see cref="AVOutputFormat.long_name" />
    /// </summary>
    public string LongName => PtrExtensions.PtrToStringUTF8((IntPtr)_ptr->long_name)!;
    
    /// <summary>
    /// <para>original type: byte*</para>
    /// <see cref="AVOutputFormat.mime_type" />
    /// </summary>
    public string? MimeType => _ptr->mime_type != null ? PtrExtensions.PtrToStringUTF8((IntPtr)_ptr->mime_type)! : null;
    
    /// <summary>
    /// <para>original type: byte*</para>
    /// <para>comma-separated filename extensions</para>
    /// <see cref="AVOutputFormat.extensions" />
    /// </summary>
    public IntPtr Extensions => (IntPtr)_ptr->extensions;
    
    /// <summary>
    /// <para>default audio codec</para>
    /// <see cref="AVOutputFormat.audio_codec" />
    /// </summary>
    public AVCodecID AudioCodec => _ptr->audio_codec;
    
    /// <summary>
    /// <para>default video codec</para>
    /// <see cref="AVOutputFormat.video_codec" />
    /// </summary>
    public AVCodecID VideoCodec => _ptr->video_codec;
    
    /// <summary>
    /// <para>default subtitle codec</para>
    /// <see cref="AVOutputFormat.subtitle_codec" />
    /// </summary>
    public AVCodecID SubtitleCodec => _ptr->subtitle_codec;
    
    /// <summary>
    /// <para>original type: int</para>
    /// <para>can use flags: AVFMT_NOFILE, AVFMT_NEEDNUMBER, AVFMT_GLOBALHEADER, AVFMT_NOTIMESTAMPS, AVFMT_VARIABLE_FPS, AVFMT_NODIMENSIONS, AVFMT_NOSTREAMS, AVFMT_ALLOW_FLUSH, AVFMT_TS_NONSTRICT, AVFMT_TS_NEGATIVE</para>
    /// <see cref="AVOutputFormat.flags" />
    /// </summary>
    public AVFMT Flags => (AVFMT)_ptr->flags;
    
    /// <summary>
    /// <para>List of supported codec_id-codec_tag pairs, ordered by "better choice first". The arrays are all terminated by AV_CODEC_ID_NONE.</para>
    /// <see cref="AVOutputFormat.codec_tag" />
    /// </summary>
    public AVCodecTag** CodecTag => _ptr->codec_tag;
    
    /// <summary>
    /// <para>original type: AVClass*</para>
    /// <para>AVClass for the private context</para>
    /// <see cref="AVOutputFormat.priv_class" />
    /// </summary>
    public FFmpegClass PrivateClass => FFmpegClass.FromNative(_ptr->priv_class);
    
    /// <summary>
    /// <para>*************************************************************** No fields below this line are part of the public API. They may not be used outside of libavformat and can be changed and removed at will. New public fields should be added right above. ****************************************************************</para>
    /// <see cref="AVOutputFormat.priv_data_size" />
    /// </summary>
    public int PrivateDataSize => _ptr->priv_data_size;
    
    /// <summary>
    /// <para>Internal flags. See FF_FMT_FLAG_* in internal.h.</para>
    /// <see cref="AVOutputFormat.flags_internal" />
    /// </summary>
    public int FlagsInternal => _ptr->flags_internal;
    
    /// <summary>
    /// <see cref="AVOutputFormat.write_header" />
    /// </summary>
    public AVOutputFormat_write_header_func WriteHeader => _ptr->write_header;
    
    /// <summary>
    /// <para>Write a packet. If AVFMT_ALLOW_FLUSH is set in flags, pkt can be NULL in order to flush data buffered in the muxer. When flushing, return 0 if there still is more data to flush, or 1 if everything was flushed and there is no more buffered data.</para>
    /// <see cref="AVOutputFormat.write_packet" />
    /// </summary>
    public AVOutputFormat_write_packet_func WritePacket => _ptr->write_packet;
    
    /// <summary>
    /// <see cref="AVOutputFormat.write_trailer" />
    /// </summary>
    public AVOutputFormat_write_trailer_func WriteTrailer => _ptr->write_trailer;
    
    /// <summary>
    /// <para>A format-specific function for interleavement. If unset, packets will be interleaved by dts.</para>
    /// <see cref="AVOutputFormat.interleave_packet" />
    /// </summary>
    public AVOutputFormat_interleave_packet_func InterleavePacket => _ptr->interleave_packet;
    
    /// <summary>
    /// <para>Test if the given codec can be stored in this container.</para>
    /// <see cref="AVOutputFormat.query_codec" />
    /// </summary>
    public AVOutputFormat_query_codec_func QueryCodec => _ptr->query_codec;
    
    /// <summary>
    /// <see cref="AVOutputFormat.get_output_timestamp" />
    /// </summary>
    public AVOutputFormat_get_output_timestamp_func GetOutputTimestamp => _ptr->get_output_timestamp;
    
    /// <summary>
    /// <para>Allows sending messages from application to device.</para>
    /// <see cref="AVOutputFormat.control_message" />
    /// </summary>
    public AVOutputFormat_control_message_func ControlMessage => _ptr->control_message;
    
    /// <summary>
    /// <para>Write an uncoded AVFrame.</para>
    /// <see cref="AVOutputFormat.write_uncoded_frame" />
    /// </summary>
    public AVOutputFormat_write_uncoded_frame_func WriteUncodedFrame => _ptr->write_uncoded_frame;
    
    /// <summary>
    /// <para>Returns device list with it properties.</para>
    /// <see cref="AVOutputFormat.get_device_list" />
    /// </summary>
    public AVOutputFormat_get_device_list_func GetDeviceList => _ptr->get_device_list;
    
    /// <summary>
    /// <para>default data codec</para>
    /// <see cref="AVOutputFormat.data_codec" />
    /// </summary>
    public AVCodecID DataCodec => _ptr->data_codec;
    
    /// <summary>
    /// <para>Initialize format. May allocate data here, and set any AVFormatContext or AVStream parameters that need to be set before packets are sent. This method must not write output.</para>
    /// <see cref="AVOutputFormat.init" />
    /// </summary>
    public AVOutputFormat_init_func Init => _ptr->init;
    
    /// <summary>
    /// <para>Deinitialize format. If present, this is called whenever the muxer is being destroyed, regardless of whether or not the header has been written.</para>
    /// <see cref="AVOutputFormat.deinit" />
    /// </summary>
    public AVOutputFormat_deinit_func Deinit => _ptr->deinit;
    
    /// <summary>
    /// <para>Set up any necessary bitstream filtering and extract any extra data needed for the global header.</para>
    /// <see cref="AVOutputFormat.check_bitstream" />
    /// </summary>
    public AVOutputFormat_check_bitstream_func CheckBitstream => _ptr->check_bitstream;
}
