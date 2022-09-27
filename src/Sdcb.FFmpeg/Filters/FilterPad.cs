using Sdcb.FFmpeg.Raw;
using System.Collections;
using System.Collections.Generic;
using static Sdcb.FFmpeg.Raw.ffmpeg;

namespace Sdcb.FFmpeg.Filters
{
    public record struct FilterPad(string Name, AVMediaType MediaType)
    {
    }

    public unsafe class FilterPadList : IReadOnlyList<FilterPad>
    {
        private readonly AVFilterPad* _pads;

        public FilterPadList(AVFilterPad* filterPads)
        {
            _pads = filterPads;
        }

        public static explicit operator AVFilterPad*(FilterPadList list) => list._pads;

        public FilterPad this[int index] => new FilterPad(GetName(index), GetMediaType(index));

        public string GetName(int index) => avfilter_pad_get_name(_pads, index);

        public AVMediaType GetMediaType(int index) => avfilter_pad_get_type(_pads, index);

        public int Count => avfilter_pad_count(_pads);

        public IEnumerator<FilterPad> GetEnumerator()
        {
            int count = Count;
            for (int i = 0; i < count; ++i)
            {
                yield return this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
