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
    public string Name => PtrExtensions.PtrToStringUTF8((IntPtr)_ptr->name)!;
    
    /// <summary>
    /// <para>original type: byte*</para>
    /// <para>Descriptive name for the format, meant to be more human-readable than name. You should use the NULL_IF_CONFIG_SMALL() macro to define it.</para>
    /// <see cref="AVInputFormat.long_name" />
    /// </summary>
    public string LongName => PtrExtensions.PtrToStringUTF8((IntPtr)_ptr->long_name)!;
    
    /// <summary>
    /// <para>original type: int</para>
    /// <para>Can use flags: AVFMT_NOFILE, AVFMT_NEEDNUMBER, AVFMT_SHOW_IDS, AVFMT_NOTIMESTAMPS, AVFMT_GENERIC_INDEX, AVFMT_TS_DISCONT, AVFMT_NOBINSEARCH, AVFMT_NOGENSEARCH, AVFMT_NO_BYTE_SEEK, AVFMT_SEEK_TO_PTS.</para>
    /// <see cref="AVInputFormat.flags" />
    /// </summary>
    public AVFMT Flags => (AVFMT)_ptr->flags;
    
    /// <summary>
    /// <para>original type: byte*</para>
    /// <para>If extensions are defined, then no probe is done. You should usually not use extension format guessing because it is not reliable enough</para>
    /// <see cref="AVInputFormat.extensions" />
    /// </summary>
    public string? Extensions => _ptr->extensions != null ? PtrExtensions.PtrToStringUTF8((IntPtr)_ptr->extensions)! : null;
    
    /// <summary>
    /// <see cref="AVInputFormat.codec_tag" />
    /// </summary>
    public AVCodecTag** CodecTag => _ptr->codec_tag;
    
    /// <summary>
    /// <para>original type: AVClass*</para>
    /// <para>AVClass for the private context</para>
    /// <see cref="AVInputFormat.priv_class" />
    /// </summary>
    public FFmpegClass? PrivateClass => FFmpegClass.FromNativeOrNull(_ptr->priv_class);
    
    /// <summary>
    /// <para>original type: byte*</para>
    /// <para>Comma-separated list of mime types. It is used check for matching mime types while probing.</para>
    /// <see cref="AVInputFormat.mime_type" />
    /// </summary>
    public string? MimeType => _ptr->mime_type != null ? PtrExtensions.PtrToStringUTF8((IntPtr)_ptr->mime_type)! : null;
    
    /// <summary>
    /// <para>*************************************************************** No fields below this line are part of the public API. They may not be used outside of libavformat and can be changed and removed at will. New public fields should be added right above. ****************************************************************</para>
    /// <see cref="AVInputFormat.raw_codec_id" />
    /// </summary>
    public int RawCodecId => _ptr->raw_codec_id;
    
    /// <summary>
    /// <para>Size of private data so that it can be allocated in the wrapper.</para>
    /// <see cref="AVInputFormat.priv_data_size" />
    /// </summary>
    public int PrivateDataSize => _ptr->priv_data_size;
    
    /// <summary>
    /// <para>Internal flags. See FF_FMT_FLAG_* in internal.h.</para>
    /// <see cref="AVInputFormat.flags_internal" />
    /// </summary>
    public int FlagsInternal => _ptr->flags_internal;
    
}
