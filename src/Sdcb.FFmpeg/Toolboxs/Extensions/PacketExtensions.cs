using Sdcb.FFmpeg.Codecs;
using Sdcb.FFmpeg.Common;
using Sdcb.FFmpeg.Filters;
using Sdcb.FFmpeg.Formats;
using Sdcb.FFmpeg.Raw;
using Sdcb.FFmpeg.Swresamples;
using Sdcb.FFmpeg.Swscales;
using Sdcb.FFmpeg.Utils;
using System;
using System.Collections.Generic;
using static Sdcb.FFmpeg.Raw.ffmpeg;

namespace Sdcb.FFmpeg.Toolboxs.Extensions;

public static class PacketsExtensions
{
    /// <summary>
    /// Calling every frame with following apis:
    /// <list type="bullet">
    /// <item><see cref="av_packet_clone"/></item>
    /// <item><see cref="av_packet_make_writable"/></item>
    /// </list>
    /// </summary>
    /// <returns>The result must free manually, and packets can be stored.</returns>
    public static IEnumerable<Packet> MakeWritable(this IEnumerable<Packet> packets)
    {
        foreach (Packet packet in packets)
        {
            Packet cloned = packet.Clone();
            cloned.MakeWritable();
            yield return cloned;
        }
    }

    /// <summary>
    /// packets -> frames
    /// </summary>
    public static IEnumerable<Frame> DecodePackets(this IEnumerable<Packet> packets, CodecContext c)
    {
        using Frame frame = new();

        foreach (Packet packet in packets)
            foreach (var _ in c.DecodePacket(packet, frame))
                yield return frame;

        foreach (var _ in c.DecodePacket(null, frame))
            yield return frame;
    }

    /// <summary>
    /// packets -> frames
    /// </summary>
    public static IEnumerable<Frame> DecodeAllPackets(this IEnumerable<Packet> packets, FormatContext fc,
        CodecContext? audioCodec = null,
        CodecContext? videoCodec = null)
    {
        using var frame = new Frame();

        foreach (Packet packet in packets)
        {
            CodecContext c = GetCodecContext(fc, audioCodec, videoCodec, packet);

            foreach (var _ in c.DecodePacket(packet, frame))
                yield return frame;
        }

        if (videoCodec != null)
        {
            foreach (var _ in videoCodec.DecodePacket(null, frame))
                yield return frame;
        }

        if (audioCodec != null)
        {
            foreach (var _ in audioCodec.DecodePacket(null, frame))
                yield return frame;
        }

        static CodecContext GetCodecContext(FormatContext fc, CodecContext? audioCodec, CodecContext? videoCodec, Packet packet)
        {
            MediaStream stream = fc.Streams[packet.StreamIndex];
            if (stream.Codecpar == null) throw new FFmpegException($"FormatContext.Streams[{packet.StreamIndex}].Codecpar should not be null.");

            CodecContext c = stream.Codecpar.CodecType switch
            {
                AVMediaType.Audio => audioCodec == null ? throw new ArgumentNullException(nameof(audioCodec)) : audioCodec,
                AVMediaType.Video => videoCodec == null ? throw new ArgumentNullException(nameof(videoCodec)) : videoCodec,
                var x => throw new FFmpegException($"FormatContext.Streams[{packet.StreamIndex}].Codecpar.CodecType {x} not supported in DecodePackets."),
            };

            return c;
        }
    }

    public static void WriteAll(this IEnumerable<Packet> packets, FormatContext fc)
    {
        foreach (Packet packet in packets)
        {
            try
            {
                fc.InterleavedWritePacket(packet);
            }
            finally
            {
                packet.Unref();
            }
        }
    }
}
