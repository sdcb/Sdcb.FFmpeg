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

        [Fact]
        public void CreateMp4()
        {
            FFmpegLogger.LogWriter = (level, msg) => _console.WriteLine(msg);

            using FormatContext fc = FormatContext.AllocOutput(formatName: "mp4");
            fc.VideoCodec = Codec.CommonEncoders.Libx264;
            MediaStream vstream = fc.NewStream(fc.VideoCodec);
            using CodecContext vcodec = new CodecContext(fc.VideoCodec)
            {
                Width = 800,
                Height = 600,
                TimeBase = new AVRational(1, 30),
                PixelFormat = AVPixelFormat.Yuv420p,
                Flags = AV_CODEC_FLAG.GlobalHeader,
            };
            vcodec.Open();
            vstream.Codecpar.CopyFrom(vcodec);

            using DynamicIOContext io = IOContext.OpenDynamic();
            fc.Pb = io;
            fc.WriteHeader();
            foreach (Packet packet in vcodec.EncodeFrames(vcodec.ConvertFrames(VideoFrameGenerator.Yuv420pSequence(vcodec.Width, vcodec.Height).Take(60))))
            {
                try
                {
                    packet.RescaleTimestamp(vcodec.TimeBase, vstream.TimeBase);
                    packet.StreamIndex = vstream.Index;
                    LogPacket(fc, packet);
                    fc.InterleavedWritePacket(packet);
                }
                finally
                {
                    packet.Unreference();
                }
            }
            fc.WriteTrailer();
            fc.Close();

            byte[] mp4 = io.GetBuffer().ToArray();
            Assert.NotEmpty(mp4);
            _console.WriteLine($"mp4 size: {mp4.Length}");

            unsafe void LogPacket(FormatContext fc, Packet packet)
            {
                AVRational timebase = MediaStream.FromNative(fc.Streams[packet.StreamIndex]).TimeBase;
                _console.WriteLine(string.Format("pts:{0} pts_time:{1} dts:{2} dts_time:{3} duration:{4} duration_time:{5} stream_index:{6}",
                       av_ts2str(packet.Pts), av_ts2timestr(packet.Pts, timebase),
                       av_ts2str(packet.Dts), av_ts2timestr(packet.Dts, timebase),
                       av_ts2str(packet.Duration), av_ts2timestr(packet.Duration, timebase),
                       packet.StreamIndex));

                static string av_ts2str(long pts) => pts == ffmpeg.AV_NOPTS_VALUE ? "NOPTS" : pts.ToString();
                static unsafe string av_ts2timestr(long pts, AVRational timebase) => pts == ffmpeg.AV_NOPTS_VALUE
                    ? "NOPTS"
                    : (1.0 * pts * timebase.Num / timebase.Den).ToString("N6");
            }
        }
    }
}
