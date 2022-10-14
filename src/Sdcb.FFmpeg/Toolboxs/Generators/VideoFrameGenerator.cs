using Sdcb.FFmpeg.Raw;
using Sdcb.FFmpeg.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Sdcb.FFmpeg.Toolboxs.Generators;

public static class VideoFrameGenerator
{
    /// <returns>Note: result frame must be disposed manuall.</returns>
    public static IEnumerable<Frame> Yuv420pSequence(int width, int height, int frameCount) => Enumerable
        .Range(0, frameCount)
        .AsParallel()
        .AsOrdered()
        .Select(i =>
        {
            Frame frame = Frame.CreateWritableVideo(width, height, AVPixelFormat.Yuv420p);
            FillYuv420p(frame, i);
            return frame;
        });

    public static unsafe void FillYuv420p(Frame frame, int i)
    {
        int_array8 linesize = frame.Linesize;
        int linesize0 = linesize._[0];
        int linesize1 = linesize._[1];
        int linesize2 = linesize._[2];

        byte* data0 = frame.Data._0;
        byte* data1 = frame.Data._1;
        byte* data2 = frame.Data._2;

        /* prepare a dummy image */
        /* Y */
        for (int y = 0; y < frame.Height; y++)
        {
            for (int x = 0; x < frame.Width; x++)
            {
                data0[y * linesize0 + x] = (byte)(x + y + i * 3);
            }
        }
        /* Cb and Cr */
        for (int y = 0; y < frame.Height / 2; y++)
        {
            for (int x = 0; x < frame.Width / 2; x++)
            {
                data1[y * linesize1 + x] = (byte)(128 + y + i * 2);
                data2[y * linesize2 + x] = (byte)(64 + x + i * 5);
            }
        }

        frame.Pts = i;
    }
}