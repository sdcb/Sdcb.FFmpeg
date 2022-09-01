using System;
using Sdcb.FFmpeg.Common;
using Sdcb.FFmpeg.Raw;
using static Sdcb.FFmpeg.Raw.ffmpeg;

namespace Sdcb.FFmpeg.Codecs;

public unsafe partial class CodecParameters
{
    /// <summary>
    /// <see cref="avcodec_parameters_alloc"/>
    /// </summary>
    public static CodecParameters Create() => new CodecParameters(avcodec_parameters_alloc(), isOwner: true);

    /// <summary>
    /// <see cref="avcodec_parameters_copy(AVCodecParameters*, AVCodecParameters*)"/>
    /// </summary>
    public void CopyFrom(CodecParameters source)
    {
        avcodec_parameters_copy(this, source).ThrowIfError();
    }

    /// <summary>
    /// <see cref="avcodec_parameters_from_context(AVCodecParameters*, AVCodecContext*)"/>
    /// </summary>
    public void CopyFrom(CodecContext ctx)
    {
        avcodec_parameters_from_context(this, ctx).ThrowIfError();
    }

    /// <summary>
    /// <see cref="avcodec_parameters_free(AVCodecParameters**)"/>
    /// </summary>
    public void Free()
    {
        AVCodecParameters* ptr = this;
        avcodec_parameters_free(&ptr);
        handle = (IntPtr)ptr;
    }

    protected override bool ReleaseHandle()
    {
        Free();
        return true;
    }
}
