using System;
using System.Runtime.InteropServices;
using static Sdcb.FFmpeg.Raw.ffmpeg;

namespace Sdcb.FFmpeg.Filters;

public unsafe partial class BufferSrcParameters : SafeHandle
{
    /// <summary>Allocate a new AVBufferSrcParameters instance. It should be freed by the caller with av_free().</summary>
    public BufferSrcParameters() : this(av_buffersrc_parameters_alloc(), isOwner: true)
    {
    }

    public void Free()
    {
        av_free(this);
        handle = IntPtr.Zero;
        SetHandleAsInvalid();
    }

    protected override bool ReleaseHandle()
    {
        Free();
        return true;
    }
}
