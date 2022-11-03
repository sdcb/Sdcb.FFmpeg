using Sdcb.FFmpeg.Codecs;
using Sdcb.FFmpeg.Utils;
using System.Collections.Generic;

namespace Sdcb.FFmpeg.Toolboxs.Extensions
{
    public static class CodecContextExtensions
    {
        /// <summary>
        /// 1 packet -> 0..N frame
        /// </summary>
        public static IEnumerable<Frame> DecodePacket(this CodecContext c, Packet? packet, Frame frame, bool unref = true)
        {
            try
            {
                c.SendPacket(packet);
            }
            finally
            {
                if (unref && packet != null)
                {
                    packet.Unref();
                }
            }

            while (true)
            {
                CodecResult s = c.ReceiveFrame(frame);
                if (s == CodecResult.Again || s == CodecResult.EOF) yield break;
                yield return frame;
            }
        }

        /// <summary>
        /// 1 frame -> 0..N packet
        /// </summary>
        public static IEnumerable<Packet> EncodeFrame(this CodecContext c, Frame? frame, Packet packet, bool unref = true)
        {
            try
            {
                c.SendFrame(frame);
            }
            finally
            {
                if (unref && frame != null)
                {
                    frame.Unref();
                }
            }

            while (true)
            {
                CodecResult s = c.ReceivePacket(packet);
                if (s == CodecResult.Again || s == CodecResult.EOF) yield break;
                yield return packet;
            }
        }
    }
}
