using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Sdcb.FFmpeg;

internal static class PtrExtensions
{
#if NET6_0_OR_GREATER
    public static string? PtrToStringUTF8(this IntPtr ptr)
    {
        return Marshal.PtrToStringUTF8(ptr);
    }
#else
    public unsafe static string? PtrToStringUTF8(this IntPtr ptr)
    {
        if (ptr == IntPtr.Zero)
            return null;
        int length = 0;
        sbyte* psbyte = (sbyte*)ptr;
        while (psbyte[length] != 0)
            length++;
        return new string(psbyte, 0, length, Encoding.UTF8);
    }
#endif
}
