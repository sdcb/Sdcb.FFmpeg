using Sdcb.FFmpeg.Raw;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sdcb.FFmpeg.Formats;

internal unsafe class MediaStreamPtrArray : IReadOnlyList<MediaStream>
{
    private readonly AVStream** _ptrs;

    public MediaStreamPtrArray(AVStream** ptrs, int count)
    {
        _ptrs = ptrs;
        Count = count;
    }

    public MediaStream this[int index]
    {
        get
        {
            if (index < 0 || index >= Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), $"Index {index} was outside the bound {Count}.");
            }
            return MediaStream.FromNative(_ptrs[index]);
        }
    }

    public int Count { get; }

    public IEnumerator<MediaStream> GetEnumerator()
    {
        for (var i = 0; i < Count; ++i) yield return this[i];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
