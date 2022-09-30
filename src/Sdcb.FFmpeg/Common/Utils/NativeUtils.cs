using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Sdcb.FFmpeg.Common;

internal static class NativeUtils
{
    public static IEnumerable<T> ReadSequence<T>(IntPtr p, int unitSize, Func<IntPtr, bool> exitCondition, Func<IntPtr, T> valGetter)
    {
        if (p == IntPtr.Zero) yield break;

        while (!exitCondition(p))
        {
            yield return valGetter(p);
            p += unitSize;
        }
    }

    public static IEnumerable<IntPtr> EnumeratePtrIterator(Func<IntPtr, IntPtr> iterator)
    {
        IntPtr opaque = AllocOpaque();
        try
        {
            for (var p = IntPtr.Zero; (p = iterator(opaque)) != IntPtr.Zero;)
            {
                yield return p;
            }
        }
        finally
        {
            Marshal.FreeHGlobal(opaque);
        }

        unsafe static IntPtr AllocOpaque()
        {
            IntPtr address = Marshal.AllocHGlobal(IntPtr.Size);
            (*(IntPtr*)(address)) = new IntPtr(0);
            return address;
        }
    }

    public static IntPtr NotNull(IntPtr ptr, string? message = null)
    {
        if (ptr == IntPtr.Zero)
        {
            throw FFmpegException.NoMemory(message ?? "NotNull check failed, ptr == 0.");
        }
        return ptr;
    }
}
