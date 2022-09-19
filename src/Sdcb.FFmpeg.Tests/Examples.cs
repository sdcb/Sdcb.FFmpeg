using Sdcb.FFmpeg.Codecs;
using Sdcb.FFmpeg.Formats;
using Sdcb.FFmpeg.Raw;
using Sdcb.FFmpeg.Toolboxs.Extensions;
using Sdcb.FFmpeg.Toolboxs.Generators;
using System.IO;
using Xunit;

namespace Sdcb.FFmpeg.Tests
{
    public class Examples
    {
        [Fact]
        public void CreatePng()
        {
            using Frame frame = Frame.CreateWritableVideo(800, 600, AVPixelFormat.Yuv420p);
            VideoFrameGenerator.FillYuv420p(frame, 0);
            byte[] pngData = frame.EncodeToBytes(formatName: "apng");
            //File.WriteAllBytes("test.png", pngData);
            Assert.NotEmpty(pngData);
        }

        //[Fact]
        //public void CreateGif()
        //{
        //    using Frame frame = Frame.CreateWritableVideo(800, 600, AVPixelFormat.Yuv420p);
        //    using FormatContext fc = FormatContext.AllocOutput(formatName: "gif");
        //    using DynamicIOContext io = IOContext.OpenDynamic();
        //    fc.Pb = io;
        //    fc.WriteHeader();
        //    foreach (Packet packet in  fc VideoFrameGenerator.Yuv420pSequence(800, 600))
        //    {
        //        fc.WritePacket(packet);
        //    }
        //    fc.WriteTrailer();

        //    byte[] gifData = frame.EncodeToBytes(formatName: "gif");
        //    File.WriteAllBytes("test.gif", gifData);
        //    Assert.NotEmpty(gifData);
        //}
    }
}
