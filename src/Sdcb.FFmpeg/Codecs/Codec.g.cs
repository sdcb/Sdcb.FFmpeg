// This file was genereated from Sdcb.FFmpeg.AutoGen, DO NOT CHANGE DIRECTLY.
#nullable enable
using Sdcb.FFmpeg.Common;
using Sdcb.FFmpeg.Raw;
using System;
using System.Runtime.InteropServices;

namespace Sdcb.FFmpeg.Codecs;

/// <summary>
/// <para>AVCodec.</para>
/// <see cref="AVCodec" />
/// </summary>
public unsafe partial struct Codec
{
    private AVCodec* _ptr;
    
    public static implicit operator AVCodec*(Codec? data)
        => data.HasValue ? (AVCodec*)data.Value._ptr : null;
    
    private Codec(AVCodec* ptr)
    {
        if (ptr == null)
        {
            throw new ArgumentNullException(nameof(ptr));
        }
        _ptr = ptr;
    }
    
    public static Codec FromNative(AVCodec* ptr) => new Codec(ptr);
    public static Codec FromNative(IntPtr ptr) => new Codec((AVCodec*)ptr);
    internal static Codec? FromNativeOrNull(AVCodec* ptr)
        => ptr != null ? new Codec?(new Codec(ptr)) : null;
    
    /// <summary>
    /// <para>original type: byte*</para>
    /// <para>Name of the codec implementation. The name is globally unique among encoders and among decoders (but an encoder and a decoder can share the same name). This is the primary way to find a codec from the user perspective.</para>
    /// <see cref="AVCodec.name" />
    /// </summary>
    public IntPtr Name
    {
        get => (IntPtr)_ptr->name;
        set => _ptr->name = (byte*)value;
    }
    
    /// <summary>
    /// <para>original type: byte*</para>
    /// <para>Descriptive name for the codec, meant to be more human readable than name. You should use the NULL_IF_CONFIG_SMALL() macro to define it.</para>
    /// <see cref="AVCodec.long_name" />
    /// </summary>
    public IntPtr LongName
    {
        get => (IntPtr)_ptr->long_name;
        set => _ptr->long_name = (byte*)value;
    }
    
    /// <summary>
    /// <see cref="AVCodec.type" />
    /// </summary>
    public AVMediaType Type
    {
        get => _ptr->type;
        set => _ptr->type = value;
    }
    
    /// <summary>
    /// <see cref="AVCodec.id" />
    /// </summary>
    public AVCodecID Id
    {
        get => _ptr->id;
        set => _ptr->id = value;
    }
    
    /// <summary>
    /// <para>original type: int</para>
    /// <para>Codec capabilities. see AV_CODEC_CAP_*</para>
    /// <see cref="AVCodec.capabilities" />
    /// </summary>
    public AV_CODEC_CAP Capabilities
    {
        get => (AV_CODEC_CAP)_ptr->capabilities;
        set => _ptr->capabilities = (int)value;
    }
    
    /// <summary>
    /// <para>maximum value for lowres supported by the decoder</para>
    /// <see cref="AVCodec.max_lowres" />
    /// </summary>
    public byte MaxLowres
    {
        get => _ptr->max_lowres;
        set => _ptr->max_lowres = value;
    }
    
    /// <summary>
    /// <para>array of supported framerates, or NULL if any, array is terminated by {0,0}</para>
    /// <see cref="AVCodec.supported_framerates" />
    /// </summary>
    public AVRational* SupportedFramerates
    {
        get => _ptr->supported_framerates;
        set => _ptr->supported_framerates = value;
    }
    
    /// <summary>
    /// <para>array of supported pixel formats, or NULL if unknown, array is terminated by -1</para>
    /// <see cref="AVCodec.pix_fmts" />
    /// </summary>
    public AVPixelFormat* PixelFormats
    {
        get => _ptr->pix_fmts;
        set => _ptr->pix_fmts = value;
    }
    
    /// <summary>
    /// <para>array of supported audio samplerates, or NULL if unknown, array is terminated by 0</para>
    /// <see cref="AVCodec.supported_samplerates" />
    /// </summary>
    public int* SupportedSamplerates
    {
        get => _ptr->supported_samplerates;
        set => _ptr->supported_samplerates = value;
    }
    
    /// <summary>
    /// <para>array of supported sample formats, or NULL if unknown, array is terminated by -1</para>
    /// <see cref="AVCodec.sample_fmts" />
    /// </summary>
    public AVSampleFormat* SampleFormats
    {
        get => _ptr->sample_fmts;
        set => _ptr->sample_fmts = value;
    }
    
    /// <summary>
    /// <para>array of support channel layouts, or NULL if unknown. array is terminated by 0</para>
    /// <see cref="AVCodec.channel_layouts" />
    /// </summary>
    public ulong* ChannelLayouts
    {
        get => _ptr->channel_layouts;
        set => _ptr->channel_layouts = value;
    }
    
    /// <summary>
    /// <para>original type: AVClass*</para>
    /// <para>AVClass for the private context</para>
    /// <see cref="AVCodec.priv_class" />
    /// </summary>
    public FFmpegClass PrivClass
    {
        get => FFmpegClass.FromNative(_ptr->priv_class);
        set => _ptr->priv_class = (AVClass*)value;
    }
    
    /// <summary>
    /// <para>array of recognized profiles, or NULL if unknown, array is terminated by {FF_PROFILE_UNKNOWN}</para>
    /// <see cref="AVCodec.profiles" />
    /// </summary>
    public AVProfile* Profiles
    {
        get => _ptr->profiles;
        set => _ptr->profiles = value;
    }
    
    /// <summary>
    /// <para>original type: byte*</para>
    /// <para>Group name of the codec implementation. This is a short symbolic name of the wrapper backing this codec. A wrapper uses some kind of external implementation for the codec, such as an external library, or a codec implementation provided by the OS or the hardware. If this field is NULL, this is a builtin, libavcodec native codec. If non-NULL, this will be the suffix in AVCodec.name in most cases (usually AVCodec.name will be of the form "&lt;codec_name&gt;_&lt;wrapper_name&gt;").</para>
    /// <see cref="AVCodec.wrapper_name" />
    /// </summary>
    public IntPtr WrapperName
    {
        get => (IntPtr)_ptr->wrapper_name;
        set => _ptr->wrapper_name = (byte*)value;
    }
    
    /// <summary>
    /// <para>Array of supported channel layouts, terminated with a zeroed layout.</para>
    /// <see cref="AVCodec.ch_layouts" />
    /// </summary>
    public AVChannelLayout* ChLayouts
    {
        get => _ptr->ch_layouts;
        set => _ptr->ch_layouts = value;
    }
}
