using System;
using Sdcb.FFmpeg.Raw;
using Sdcb.FFmpeg.Common;

namespace Sdcb.FFmpeg.Utils;

/// <summary>
/// <see cref="AVClass"/>
/// </summary>
public unsafe struct FFmpegClass
{
    private readonly AVClass* _p;

    private FFmpegClass(AVClass* p)
    {
        if (p == null) throw new ArgumentNullException(nameof(p));
        _p = p; 
    }

    public static FFmpegClass FromNative(AVClass* p) => new FFmpegClass(p);

    public static FFmpegClass? FromNativeOrNull(AVClass* p) => p != null ? new FFmpegClass(p) : null;

    public static implicit operator AVClass*(FFmpegClass data) => data._p;

    public string Name => ((IntPtr)_p->class_name).PtrToStringUTF8()!;

    public string Version => _p->version.ToFourCC();

    public AVClassCategory Category => _p->category;
}
