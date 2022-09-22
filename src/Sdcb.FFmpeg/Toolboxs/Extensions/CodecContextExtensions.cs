using Sdcb.FFmpeg.Codecs;
using Sdcb.FFmpeg.Raw;
using Sdcb.FFmpeg.Swresamples;
using Sdcb.FFmpeg.Swscales;
using System.Collections.Generic;
using static Sdcb.FFmpeg.Raw.ffmpeg;

namespace Sdcb.FFmpeg.Toolboxs.Extensions;

public static class CodecContextExtensions
{
    /// <summary>
    /// <see cref="sws_getCachedContext(SwsContext*, int, int, AVPixelFormat, int, int, AVPixelFormat, int, SwsFilter*, SwsFilter*, double*)"/>
    /// <see cref="sws_scale(SwsContext*, byte*[], int[], int, int, byte*[], int[])"/>
    /// </summary>
    public static IEnumerable<Frame> ConvertFrames(this CodecContext c, IEnumerable<Frame> sourceFrames, SWS flags = SWS.Bilinear)
    {
        using var destFrame = c.CreateFrame();
        int pts = 0;
        if (c.Codec.Type == AVMediaType.Video)
        {
            using var frameConverter = new VideoFrameConverter();
            foreach (var sourceFrame in sourceFrames)
            {
                frameConverter.ConvertFrame(sourceFrame, destFrame, flags);
                destFrame.Pts = pts++;
                yield return destFrame;
            }
        }
        else if (c.Codec.Type == AVMediaType.Audio)
        {
            using var frameConverter = new SampleConverter();
            foreach (var sourceFrame in sourceFrames)
            {
                frameConverter.ConvertFrame(destFrame, sourceFrame);
                destFrame.Pts = pts += c.FrameSize;
                yield return destFrame;
            }
        }
    }
}
