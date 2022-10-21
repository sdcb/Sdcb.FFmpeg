using Sdcb.FFmpeg.Codecs;
using Sdcb.FFmpeg.Raw;
using System.Collections.Generic;

namespace Sdcb.FFmpeg.Toolboxs.Extensions
{
    public record struct PtsDts(long Pts, long Dts)
    {
        public bool HasValue => this != Default;
        public static PtsDts Default => new (ffmpeg.AV_NOPTS_VALUE, ffmpeg.AV_NOPTS_VALUE);
    }

    public static class PtsDtsExtensions
    {
        public static IEnumerable<Packet> RecordPtsDts(this IEnumerable<Packet> packets, Dictionary<int, PtsDts> packetTiming)
        {
            foreach (Packet packet in packets)
            {
                packetTiming[packet.StreamIndex] = new(packet.Pts, packet.Dts);
                yield return packet;
            }
        }
    }
}
