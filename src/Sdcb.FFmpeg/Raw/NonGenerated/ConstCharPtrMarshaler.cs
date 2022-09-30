using System;
using System.Runtime.InteropServices;

namespace Sdcb.FFmpeg.Raw;

internal class ConstCharPtrMarshaler : ICustomMarshaler
{
    public object MarshalNativeToManaged(IntPtr pNativeData) => pNativeData.PtrToStringUTF8()!;

    public IntPtr MarshalManagedToNative(object managedObj) => throw new NotImplementedException();

    public void CleanUpNativeData(IntPtr pNativeData)
    {
    }

    public void CleanUpManagedData(object managedObj)
    {
    }

    public int GetNativeDataSize() => IntPtr.Size;

    private static readonly ConstCharPtrMarshaler Instance = new();

    public static ICustomMarshaler GetInstance(string cookie) => Instance;
}