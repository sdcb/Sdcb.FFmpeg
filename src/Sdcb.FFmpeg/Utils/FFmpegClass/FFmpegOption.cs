using System;
using Sdcb.FFmpeg.Raw;
using System.Runtime.InteropServices;
using static Sdcb.FFmpeg.Raw.ffmpeg;

namespace Sdcb.FFmpeg.Utils;

public unsafe class FFmpegOption
{
    private readonly AVOption* _p;

    public FFmpegOption(AVOption* p)
    {
        if (p == null) throw new ArgumentNullException(nameof(p));
        _p = p;
    }

    public FFmpegOption(IntPtr p)
    {
        if (p == IntPtr.Zero) throw new ArgumentNullException(nameof(p));
        _p = (AVOption*)p;
    }

    public static implicit operator AVOption*(FFmpegOption data) => data._p;

    public string Name => Marshal.PtrToStringAnsi((IntPtr)_p->name)!;

    public string? Help => Marshal.PtrToStringAnsi((IntPtr)_p->help);

    public int Offset => _p->offset;

    public AVOptionType Type => _p->type;

    public object? DefaultValue => Type switch
    {
        AVOptionType.String => ((IntPtr)_p->default_val.str).PtrToStringUTF8(),
        AVOptionType.Rational => _p->default_val.q,
        AVOptionType.Float => _p->default_val.dbl, 
        _ => (IntPtr)_p->default_val.str
    };

    public double Min => _p->min;

    public double Max => _p->max;

    public AV_OPT_FLAG Flags => (AV_OPT_FLAG)_p->flags;

    public string? Unit => Marshal.PtrToStringAnsi((IntPtr)_p->unit);
}
