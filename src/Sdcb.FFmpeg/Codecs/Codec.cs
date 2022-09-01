using Sdcb.FFmpeg.Common;
using Sdcb.FFmpeg.Raw;
using System;
using System.Runtime.InteropServices;
using static Sdcb.FFmpeg.Raw.ffmpeg;

namespace Sdcb.FFmpeg.Codecs;

/// <summary>
/// <para>AVCodec.</para>
/// <see cref="AVCodec" />
/// </summary>
public unsafe partial struct Codec
{
    /// <summary>
    /// <see cref="av_codec_is_encoder(AVCodec*)"/>
    /// </summary>
    public bool IsEncoder => av_codec_is_encoder(this) != 0;

    /// <summary>
    /// <see cref="av_codec_is_decoder(AVCodec*)"/>
    /// </summary>
    public bool IsDecoder => av_codec_is_decoder(this) != 0;
}
