using System;
using static Sdcb.FFmpeg.Raw.ffmpeg;

namespace Sdcb.FFmpeg.Raw;

public struct AVRational
{
    /// <summary>
    /// Numerator
    /// </summary>
    public int Num;

    /// <summary>
    /// Denominator
    /// </summary>
    public int Den;

    public AVRational(int number) : this(number, 1) { }

    public AVRational(int numerator, int denominator)
    {
        Num = numerator;
        Den = denominator;
    }

    public static AVRational FromDouble(double d, int max) => av_d2q(d, max);

    public double ToDouble() => Num / (double)Den;

    public uint ToIEEEFloat32() => av_q2intfloat(this);

    public unsafe AVRational Reduce()
    {
        AVRational result = this;
        av_reduce(&result.Num, &result.Den, Num, Den, int.MaxValue);
        return result;
    }

    public AVRational Inverse() => new AVRational(Den, Num);

    public static int Compare(in AVRational a, in AVRational b)
    {
        long tmp = a.Num * (long)b.Den - b.Num * (long)a.Den;

        if (tmp != 0) return (int)((tmp ^ a.Den ^ b.Den) >> 63) | 1;
        else if (b.Den != 0 && a.Den != 0) return 0;
        else if (a.Num != 0 && b.Num != 0) return (a.Num >> 31) - (b.Num >> 31);
        else return int.MinValue;
    }

    public static unsafe AVRational operator *(AVRational b, in AVRational c)
    {
        av_reduce(&b.Num, &b.Den,
            b.Num * (long)c.Num,
            b.Den * (long)c.Den, int.MaxValue);
        return b;
    }

    public static unsafe AVRational operator *(AVRational b, long c)
    {
        av_reduce(&b.Num, &b.Den,
            b.Num * (long)c,
            b.Den * (long)1, int.MaxValue);
        return b;
    }

    public static AVRational operator /(in AVRational b, in AVRational c) => b * new AVRational(c.Den, c.Num);

    public static unsafe AVRational operator +(AVRational b, in AVRational c)
    {
        av_reduce(&b.Num, &b.Den,
            b.Num * (long)c.Den + c.Num * (long)b.Den,
            b.Den * (long)c.Den, int.MaxValue);
        return b;
    }

    public static AVRational operator -(in AVRational b, in AVRational c) => b + new AVRational(-c.Num, c.Den);

    public static bool operator >(in AVRational a, in AVRational b) => Compare(a, b) > 0;

    public static bool operator <(in AVRational a, in AVRational b) => Compare(a, b) < 0;

    public static bool operator ==(in AVRational a, in AVRational b) => Compare(a, b) == 0;

    public static bool operator !=(in AVRational a, in AVRational b) => Compare(a, b) != 0;

    public static int Nearer(in AVRational q, in AVRational q1, in AVRational q2) => av_nearer_q(q, q1, q2);

    public static unsafe int FindNearestIndex(in AVRational q, AVRational* list) => av_find_nearest_q_idx(q, list);

    public static AVRational Gcd(in AVRational a, in AVRational b, int maxDenominator, AVRational def = default) => av_gcd_q(a, b, maxDenominator, def);
    public override string ToString() => $"{Num}/{Den}";
    public override readonly bool Equals(object? obj) => obj is AVRational r && Equals(r);

    public readonly bool Equals(AVRational other) => this == other;

    public override int GetHashCode() => HashCode.Combine(Num, Den);
}
