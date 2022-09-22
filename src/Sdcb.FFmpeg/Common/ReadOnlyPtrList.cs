using System;
using System.Collections;
using System.Collections.Generic;

namespace Sdcb.FFmpeg.Common;

internal unsafe class ReadOnlyPtrList<Ptr, Final> : IReadOnlyList<Final> where Ptr : unmanaged
{
    private readonly Ptr** _ptr;
    private int _size;
    private Func<IntPtr, Final> FinalAccessor { get; init; }

    public ReadOnlyPtrList(Ptr** ptr, int size, Func<IntPtr, Final> finalAccessor)
    {
        _ptr = ptr;
        _size = size;
        FinalAccessor = finalAccessor;
    }

    public Final this[int index]
    {
        get
        {
            if (index < 0 || index >= Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), $"Index {index} was outside the bound {Count}.");
            }
            return FinalAccessor((IntPtr)_ptr[index]);
        }
    }

    public int Count => _size;

    public IEnumerator<Final> GetEnumerator()
    {
        for (var i = 0; i < _size; ++i) yield return this[i];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
