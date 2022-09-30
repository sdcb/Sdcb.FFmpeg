using Sdcb.FFmpeg.Raw;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using static Sdcb.FFmpeg.Raw.ffmpeg;

namespace Sdcb.FFmpeg.Filters
{
    public record struct FilterPad(string Name, AVMediaType MediaType)
    {
        public unsafe static FilterPad? FromNativeOrNull(AVFilterPad* pad, int index = 0)
        {
            return pad != null ? FromNative(pad, index) : null;
        }

        public unsafe static FilterPad FromNative(AVFilterPad* pad, int index = 0)
        {
            return new FilterPad(avfilter_pad_get_name(pad, index), avfilter_pad_get_type(pad, index));
        }
    }

    public unsafe class FilterPadList : IReadOnlyList<FilterPad>
    {
        private readonly AVFilterPad* _pads;

        public FilterPadList(AVFilterPad* filterPads, int count)
        {
            _pads = filterPads;
            Count = count;
        }

        public static implicit operator AVFilterPad*(FilterPadList list) => list._pads;

        public FilterPad this[int index]
        {
            get
            {
                if (index < 0 || index >= Count) throw new ArgumentOutOfRangeException(nameof(index));
                return FilterPad.FromNative(_pads, index);
            }
        }

        public int Count { get; }

        public IEnumerator<FilterPad> GetEnumerator()
        {
            for (int i = 0; i < Count; ++i)
            {
                yield return this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
