using System;
using static Sdcb.FFmpeg.Raw.ffmpeg;

namespace Sdcb.FFmpeg.Raw
{
    public record struct AVRational
    {
        /// <summary>
        /// Numerator
        /// </summary>
        public int num;

        /// <summary>
        /// Denominator
        /// </summary>
        public int den;

        public AVRational(int number) : this(number, 1) { }

        public AVRational(int numerator, int denominator)
        {
            num = numerator;
            den = denominator;
        }

        public static AVRational FromDouble(double d, int max) => av_d2q(d, max);

        public double ToDouble() => num / (double)den;

        public uint ToIEEEFloat32() => av_q2intfloat(this);

        public unsafe AVRational Reduce()
        {
            AVRational result = this;
            av_reduce(&result.num, &result.den, num, den, int.MaxValue);
            return result;
        }

        public AVRational Inverse() => new AVRational(den, num);

        public static int Compare(in AVRational a, in AVRational b)
        {
            long tmp = a.num * (long)b.den - b.num * (long)a.den;

            if (tmp != 0) return (int)((tmp ^ a.den ^ b.den) >> 63) | 1;
            else if (b.den != 0 && a.den != 0) return 0;
            else if (a.num != 0 && b.num != 0) return (a.num >> 31) - (b.num >> 31);
            else return int.MinValue;
        }

        public static unsafe AVRational operator *(AVRational b, in AVRational c)
        {
            av_reduce(&b.num, &b.den,
                b.num * (long)c.num,
                b.den * (long)c.den, int.MaxValue);
            return b;
        }

        public static AVRational operator /(in AVRational b, in AVRational c) => b * new AVRational(c.den, c.num);

        public static unsafe AVRational operator +(AVRational b, in AVRational c)
        {
            av_reduce(&b.num, &b.den,
                b.num * (long)c.den + c.num * (long)b.den,
                b.den * (long)c.den, int.MaxValue);
            return b;
        }

        public static AVRational operator -(in AVRational b, in AVRational c) => b + new AVRational(-c.num, c.den);

        public static bool operator >(in AVRational a, in AVRational b) => Compare(a, b) > 0;

        public static bool operator <(in AVRational a, in AVRational b) => Compare(a, b) < 0;

        public static bool operator ==(in AVRational a, in AVRational b) => Compare(a, b) == 0;

        public static bool operator !=(in AVRational a, in AVRational b) => Compare(a, b) != 0;

        public static int Nearer(in AVRational q, in AVRational q1, in AVRational q2) => av_nearer_q(q, q1, q2);

        public static unsafe int FindNearestIndex(in AVRational q, AVRational* list) => av_find_nearest_q_idx(q, (AVRational*)list);

        public static AVRational Gcd(in AVRational a, in AVRational b, int maxDenominator, AVRational @default = default) => av_gcd_q(a, b, maxDenominator, @default);

        public override int GetHashCode() =>
#if NET6_0_OR_GREATER
            HashCode.Combine(num, den);
#else
            num ^ den;
#endif

        public override string ToString() => $"{num}/{den}({ToDouble():F6})";
    }
}
