using Sdcb.FFmpeg.Codecs;
using Sdcb.FFmpeg.Filters;
using Sdcb.FFmpeg.Utils;
using System;
using System.Collections.Generic;

namespace Sdcb.FFmpeg.Toolboxs.FilterTools;

public abstract record CommonFilterContext(FilterGraph FilterGraph, FilterContext SourceContext, FilterContext SinkContext) : IDisposable
{
    /// <returns>Note: returned Frame must be disposed manually.</returns>
    public IEnumerable<Frame> WriteFrame(Frame destFrame, Frame? srcFrame, bool unref = true)
    {
        try
        {
            SourceContext.WriteFrame(srcFrame);
        }
        finally
        {
            if (unref && srcFrame != null)
            {
                srcFrame.Unref();
            }
        }

        while (true)
        {
            CodecResult r = CodecContext.ToCodecResult(SinkContext.GetFrame(destFrame));
            if (r == CodecResult.Again || r == CodecResult.EOF) break;

            if (r == CodecResult.Success)
            {
                yield return destFrame;
            }
        }
    }

    public abstract void ConfigureEncoder(CodecContext c);

    public void Dispose()
    {
        SinkContext.Dispose();
        SourceContext.Dispose();
        FilterGraph.Dispose();
    }
}
