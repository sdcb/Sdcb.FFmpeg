using Sdcb.FFmpeg.Codecs;
using Sdcb.FFmpeg.Common;
using Sdcb.FFmpeg.Formats;
using Sdcb.FFmpeg.Raw;
using Sdcb.FFmpeg.Toolboxs.Extensions;
using Sdcb.FFmpeg.Toolboxs.Generators;
using System;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Sdcb.FFmpeg.Tests
{
    public class Examples
    {
        private readonly ITestOutputHelper _console;

        public Examples(ITestOutputHelper console)
        {
            _console = console;
        }

        [Fact]
        public void CreatePng()
        {
            using Frame frame = Frame.CreateWritableVideo(800, 600, AVPixelFormat.Yuv420p);
            VideoFrameGenerator.FillYuv420p(frame, 0);
            byte[] pngData = frame.EncodeToBytes(formatName: "apng");
            //File.WriteAllBytes("test.png", pngData);
            Assert.NotEmpty(pngData);
        }

        private byte[] MakeMp4()
        {
            using FormatContext fc = FormatContext.AllocOutput(formatName: "mp4");
            fc.VideoCodec = Codec.CommonEncoders.Libx264;
            MediaStream vstream = fc.NewStream(fc.VideoCodec);
            using CodecContext vcodec = new CodecContext(fc.VideoCodec)
            {
                Width = 640,
                Height = 480,
                TimeBase = new AVRational(1, 30),
                PixelFormat = AVPixelFormat.Yuv420p,
                Flags = AV_CODEC_FLAG.GlobalHeader,
            };
            vcodec.Open();
            vstream.Codecpar.CopyFrom(vcodec);

            using DynamicIOContext io = IOContext.OpenDynamic();
            fc.Pb = io;
            fc.WriteHeader();
            foreach (Packet packet in vcodec.EncodeFrames(VideoFrameGenerator.Yuv420pSequence(vcodec.Width, vcodec.Height).Take(60)))
            {
                try
                {
                    packet.RescaleTimestamp(vcodec.TimeBase, vstream.TimeBase);
                    packet.StreamIndex = vstream.Index;
                    fc.InterleavedWritePacket(packet);
                }
                finally
                {
                    packet.Unreference();
                }
            }
            fc.WriteTrailer();
            return io.GetBuffer().ToArray();
        }

        [Fact]
        public void CreateMp4()
        {
            FFmpegLogger.LogWriter = (level, msg) => _console.WriteLine(msg.Trim());
            byte[] mp4 = MakeMp4();
            Assert.NotEmpty(mp4);
            Console.WriteLine($"mp4 size: {mp4.Length}");
        }
    }
}
