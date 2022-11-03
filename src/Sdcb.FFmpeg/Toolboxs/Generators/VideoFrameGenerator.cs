using Sdcb.FFmpeg.Raw;
using Sdcb.FFmpeg.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sdcb.FFmpeg.Toolboxs.Generators;

public static class VideoFrameGenerator
{
    /// <returns>Caller must call <see cref="Frame.Unref"/> to the result when not used.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static IEnumerable<Frame> Yuv420pSequence(int width, int height, int frameCount)
    {
        using Frame dest = Frame.CreateVideo(width, height, AVPixelFormat.Yuv420p);
        using Frame destRef = new ();
        for (int i = 0; i < frameCount; ++i)
        {
            dest.MakeWritable();
            FillYuv420p(dest, i);

            destRef.Ref(dest);
            yield return destRef;
        }
    }

    /// <summary>
    /// This function will generate much faster samples because it benifits from parallism.
    /// </summary>
    /// <returns>Same as <see cref="Yuv420pSequence"/>, but caller must call <see cref="Frame.Free"/> to the result when not used.</returns>
    public static IEnumerable<Frame> WritableYuv420pSequence(int width, int height, int frameCount) => Enumerable
        .Range(0, frameCount)
        .AsParallel()
        .AsOrdered()
        .Select(i =>
        {
            Frame frame = Frame.CreateVideo(width, height, AVPixelFormat.Yuv420p);
            FillYuv420p(frame, i);
            return frame;
        });

    public static unsafe void FillYuv420p(Frame frame, int i)
    {
        int_array8 linesize = frame.Linesize;
        int linesize0 = linesize._[0];
        int linesize1 = linesize._[1];
        int linesize2 = linesize._[2];

        byte* data0 = (byte*)frame.Data._0;
        byte* data1 = (byte*)frame.Data._1;
        byte* data2 = (byte*)frame.Data._2;

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