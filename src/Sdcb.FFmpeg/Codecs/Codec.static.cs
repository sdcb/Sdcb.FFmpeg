using Sdcb.FFmpeg.Common;
using Sdcb.FFmpeg.Raw;
using System;
using System.Collections.Generic;
using System.Linq;
using static Sdcb.FFmpeg.Raw.ffmpeg;

namespace Sdcb.FFmpeg.Codecs;

public unsafe partial struct Codec
{
    /// <summary>
    /// <see cref="av_codec_iterate(void**)"/>
    /// </summary>
    public static IEnumerable<Codec> All => NativeUtils
        .EnumeratePtrIterator(ptr => (IntPtr)(av_codec_iterate((void**)ptr)))
        .Select(FromNative);

    public static IEnumerable<Codec> Encoders => All.Where(x => x.IsEncoder);

    public static IEnumerable<Codec> Decoders => All.Where(x => x.IsDecoder);

    /// <summary>
    /// <see cref="avcodec_find_decoder(AVCodecID)"/>
    /// </summary>
    public static Codec FindDecoderById(AVCodecID id) => FromNative(FindChecking(avcodec_find_decoder(id), id));

    public static IEnumerable<Codec> FindDecoders(AVCodecID id) => Decoders.Where(x => x.Id == id);

    /// <summary>
    /// <see cref="avcodec_find_decoder_by_name(string)"/>
    /// </summary>
    public static Codec? FindDecoderByName(string name) => FromNativeOrNull(avcodec_find_decoder_by_name(name));

    /// <summary>
    /// <see cref="avcodec_find_encoder(AVCodecID)"/>
    /// </summary>
    public static Codec FindEncoderById(AVCodecID id) => FromNative(FindChecking(avcodec_find_encoder(id), id));

    public static IEnumerable<Codec> FindEncoders(AVCodecID id) => Encoders.Where(x => x.Id == id);

    /// <summary>
    /// <see cref="avcodec_find_encoder_by_name(string)"/>
    /// </summary>
    public static Codec? FindEncoderByName(string name) => FromNativeOrNull(avcodec_find_encoder_by_name(name));

    private static AVCodec* FindChecking(AVCodec* codec, AVCodecID id) => codec != null ? codec : throw new FFmpegException($"codec id {id} not found.");
    private static AVCodec* FindChecking(AVCodec* codec, string name) => codec != null ? codec : throw new FFmpegException($"codec name '{name}' not found.");

    /// <summary>
    /// <see cref="avcodec_version"/>
    /// </summary>
    public static string Version => avcodec_version().ToFourCC();

    /// <summary>
    /// <see cref="avcodec_configuration"/>
    /// </summary>
    public static HashSet<string> Configuration => avcodec_configuration()
        .Split(' ')
        .ToHashSet();

    /// <summary>
    /// <see cref="avcodec_license"/>
    /// </summary>
    public static string License => avcodec_license();

    public static CommonEncoders CommonEncoders { get; } = new();
}
