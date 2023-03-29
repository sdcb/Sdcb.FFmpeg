using System.Collections.Generic;
using System.ComponentModel;

using Sdcb.FFmpeg.Codecs;
using Sdcb.FFmpeg.Filters;
using Sdcb.FFmpeg.Toolboxs.Extensions;
using Sdcb.FFmpeg.Utils;

namespace Sdcb.FFmpeg.Toolboxs.FilterTools
{
    public abstract record MultipleToOneFilter(FilterGraph FilterGraph,FilterContext SinkContext)
    {
        public IReadOnlyList<FilterContext> Track { get; } = new List<FilterContext>();
        public IEnumerable<Frame> WriteFrame(Frame distFrame, Frame? srcFrame, int index)
        {
            WriteFrame(srcFrame, index);
            while (true)
            {
                CodecResult r = CodecContext.ToCodecResult(SinkContext.GetFrame(distFrame));
                if (r == CodecResult.Again || r == CodecResult.EOF) break;

                if (r == CodecResult.Success)
                {
                    yield return distFrame;
                }
            }
        }

        public IEnumerable<FrameContext> WriteFrame(params MediaThreadQueue<Frame>[] queues)//index=-1表示已处理的帧
        {
            do
            {
                for (int i = 0; i < queues.Length; i++)
                {

                    if (!queues[i].IsCompleted)
                    {
                        var frame = queues[i].Take();
                        if (frame.Width > 0)
                            WriteFrame(frame, i);
                        else
                        {
                            yield return new FrameContext(frame, i);
                            continue;
                        }
                    }
                    else
                    {
                        WriteFrame(null, i);
                        yield return new FrameContext(null, i);
                    }
                }

                while (true)
                {
                    using Frame distFrame = new();
                    CodecResult r = CodecContext.ToCodecResult(SinkContext.GetFrame(distFrame));
                    if (r == CodecResult.Again) break;
                    else if (r == CodecResult.EOF) yield break;

                    if (r == CodecResult.Success)
                    {
                        yield return new FrameContext(distFrame, -1);
                    }
                }
            } while (true);
        }
        private void WriteFrame(Frame? srcFrame, int index)
        {
            try
            {
                Track[index].WriteFrame(srcFrame);
            }
            finally
            {
                if (srcFrame != null)
                {
                    srcFrame.Unref();
                }
            }
        }
    }
}