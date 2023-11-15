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
    public string? Extensions => _ptr->extensions != null ? PtrExtensions.PtrToStringUTF8((IntPtr)_ptr->extensions)! : null;
    
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
    /// <para>can use flags: AVFMT_NOFILE, AVFMT_NEEDNUMBER, AVFMT_GLOBALHEADER, AVFMT_NOTIMESTAMPS, AVFMT_VARIABLE_FPS, AVFMT_NODIMENSIONS, AVFMT_NOSTREAMS, AVFMT_TS_NONSTRICT, AVFMT_TS_NEGATIVE</para>
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
    public FFmpegClass? PrivateClass => FFmpegClass.FromNativeOrNull(_ptr->priv_class);
}
