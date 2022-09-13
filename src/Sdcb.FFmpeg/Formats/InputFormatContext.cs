using Sdcb.FFmpeg.Raw;
using static Sdcb.FFmpeg.Raw.ffmpeg;
using System;

namespace Sdcb.FFmpeg.Formats;

internal unsafe partial class InputFormatContext : FormatContext
{
    internal protected InputFormatContext(AVFormatContext* ptr, bool isOwner) : base(ptr, isOwner)
    {
    }

    /// <summary>
    /// <see cref="avformat_close_input(AVFormatContext**)"/>
    /// </summary>
    public void CloseInput()
    {
        AVFormatContext* ptr = this;
        avformat_close_input(&ptr);
        handle = (IntPtr)ptr;
    }

    protected override bool ReleaseHandle()
    {
        CloseInput();
        return true;
    }
}
