using Sdcb.FFmpeg.Codecs;
using Sdcb.FFmpeg.Common;
using Sdcb.FFmpeg.Formats;
using Sdcb.FFmpeg.Raw;
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
    /// <returns>The result must call <see cref="Packet.Free"/> manually, and packets can be stored.</returns>
    public static IEnumerable<Packet> CloneMakeWritable(this IEnumerable<Packet> packets, bool unref = true)
    {
        foreach (Packet packet in packets)
        {
            Packet cloned = packet.Clone();
            if (unref) packet.Unref();
            cloned.MakeWritable();
            yield return cloned;
        }
    }

    /// <summary>
    /// packets -> frames
    /// </summary>
    /// <returns>Caller must call <see cref="Frame.Unref"/> to the result when not used.</returns>
    public static IEnumerable<Frame> DecodePackets(this IEnumerable<Packet> packets, CodecContext c)
    {
        using Frame destRef = new Frame();

        foreach (Packet packet in packets)
            foreach (Frame frame in c.DecodePacket(packet, destRef))
                yield return frame;

        foreach (Frame frame in c.DecodePacket(null, destRef))
            yield return frame;
    }

    /// <summary>
    /// packets -> frames
    /// </summary>
    /// <returns>Caller must call <see cref="Frame.Unref"/> to the result when not used.</returns>
    public static IEnumerable<Frame> DecodeAllPackets(this IEnumerable<Packet> packets, FormatContext fc,
        CodecContext? audioCodec = null,
        CodecContext? videoCodec = null)
    {
        using Frame destRef = new Frame();

        foreach (Packet packet in packets)
        {
            CodecContext c = GetCodecContext(fc, audioCodec, videoCodec, packet);

            foreach (Frame frame in c.DecodePacket(packet, destRef))
                yield return frame;
        }

        if (videoCodec != null)
        {
            foreach (Frame frame in videoCodec.DecodePacket(null, destRef))
                yield return frame;
        }

        if (audioCodec != null)
        {
            foreach (Frame frame in audioCodec.DecodePacket(null, destRef))
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

    public static void WriteAll(this IEnumerable<Packet> packets, FormatContext fc, bool unref = true)
    {
        foreach (Packet packet in packets)
        {
            try
            {
                fc.InterleavedWritePacket(packet);
            }
            finally
            {
                if (unref)
                {
                    packet.Unref();
                }
            }
        }
    }

    public unsafe static void SetData(this Packet packet, IntPtr data, int size)
    {
        AVPacket* ptr = (AVPacket*)packet;
        ptr->data = (byte*)data;
        ptr->size = size;
    }
}
