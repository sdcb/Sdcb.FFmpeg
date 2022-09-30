using System;
using System.Collections;
using System.Collections.Generic;

namespace Sdcb.FFmpeg.Common;

internal unsafe class ReadOnlyNativeList<N> : IReadOnlyList<N> where N : unmanaged
{
    private readonly N* _ptr;
    private int _size;

    public ReadOnlyNativeList(N* ptr, int size)
    {
        _ptr = ptr;
        _size = size;
    }

    public N this[int index]
    {
        get
        {
            if (index < 0 || index >= Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), $"Index {index} was outside the bound {Count}.");
            }
            return _ptr[index];
        }
    }

    public int Count => _size;

    public IEnumerator<N> GetEnumerator()
    {
        for (var i = 0; i < _size; ++i) yield return this[i];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

internal unsafe class ReadOnlyNativeListWithCast<N, Final> : IReadOnlyList<Final> where N : unmanaged
{
    private readonly N* _ptr;
    private int _size;
    private Func<N, Final> FinalAccessor { get; init; }

    public ReadOnlyNativeListWithCast(N* ptr, int size, Func<N, Final> toFinal)
    {
        _ptr = ptr;
        _size = size;
        FinalAccessor = toFinal;
    }

    public Final this[int index]
    {
        get
        {
            if (index < 0 || index >= Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), $"Index {index} was outside the bound {Count}.");
            }
            return FinalAccessor(_ptr[index]);
        }
    }

    public int Count => _size;

    public IEnumerator<Final> GetEnumerator()
    {
        for (var i = 0; i < _size; ++i) yield return this[i];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}