using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Sdcb.FFmpeg.Raw;

public static partial class ffmpeg
{
    public static readonly int EAGAIN = 11;

    public static readonly int ENOMEM = 12;

    public static readonly int EINVAL = 22;

    public static readonly int EPIPE = 32;

    static ffmpeg()
    {
#if NET6_0_OR_GREATER
        SetupLibs();
        if (OperatingSystem.IsMacOS())
        {
            EAGAIN = 35;
        }
#endif
    }

#if NET6_0_OR_GREATER
    private static void SetupLibs()
    {
        NativeLibrary.SetDllImportResolver(typeof(ffmpeg).Assembly, DllImportResolver);
    }

    private static IntPtr DllImportResolver(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
    {
        if (OperatingSystem.IsWindows())
        {
            return NativeLibrary.Load(libraryName, assembly, searchPath);
        }
        else
        {
            (string name, string version) = libraryName.Split("-") switch { var x => (x[0], x[1]) };
            if (OperatingSystem.IsLinux() || OperatingSystem.IsFreeBSD())
            {
                return NativeLibrary.Load($"lib{name}.so.{version}", assembly, searchPath);
            }
            else if (OperatingSystem.IsMacOS())
            {
                return NativeLibrary.Load($"lib{name}.{version}.dylib", assembly, searchPath);
            }
            else
            {
                Console.WriteLine($"Warning: OS Platform {Environment.OSVersion.Platform} might not support");
                return NativeLibrary.Load($"lib{name}.so.{version}", assembly, searchPath);
            }
        }
    }
#endif

    public static ulong UINT64_C<T>(T a)
        => Convert.ToUInt64(a);

    public static int AVERROR<T1>(T1 a)
        => -Convert.ToInt32(a);

    public static int MKTAG<T1, T2, T3, T4>(T1 a, T2 b, T3 c, T4 d)
        => (int)(Convert.ToUInt32(a) | (Convert.ToUInt32(b) << 8) | (Convert.ToUInt32(c) << 16) |
                  (Convert.ToUInt32(d) << 24));

    public static int FFERRTAG<T1, T2, T3, T4>(T1 a, T2 b, T3 c, T4 d)
        => -MKTAG(a, b, c, d);

    public static uint AV_VERSION_INT(uint a, uint b, uint c) => (a << 16) | (b << 8) | c;

    public static string AV_VERSION_DOT<T1, T2, T3>(T1 a, T2 b, T3 c) => $"{a}.{b}.{c}";

    public static string AV_VERSION<T1, T2, T3>(T1 a, T2 b, T3 c) => AV_VERSION_DOT(a, b, c);

    public static AVChannelLayout AV_CHANNEL_LAYOUT_MASK(int nb, ulong channel) => new AVChannelLayout
    {
        order = AVChannelOrder.Native,
        nb_channels = nb,
        u = new AVChannelLayout_u
        {
            mask = channel
        }
    };
}