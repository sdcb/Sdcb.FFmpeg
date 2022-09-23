using Sdcb.FFmpeg.Codecs;
using Sdcb.FFmpeg.Common;
using Sdcb.FFmpeg.Raw;
using System;
using static Sdcb.FFmpeg.Raw.ffmpeg;


namespace Sdcb.FFmpeg.Formats
{
    public unsafe partial struct MediaStream
    {
        /// <summary>
        /// <see cref="avformat_new_stream(AVFormatContext*, AVCodec*)"/>
        /// </summary>
        public MediaStream(FormatContext formatContext, Codec? codec = null) : this(avformat_new_stream(formatContext, codec))
        {
        }

        /// <summary>
        /// <see cref="av_stream_add_side_data(AVStream*, AVPacketSideDataType, byte*, ulong)"/>
        /// </summary>
        public void AddSideData(AVPacketSideDataType type, DataPointer data) =>
            av_stream_add_side_data(this, type, (byte*)data.Pointer, (ulong)data.Length).ThrowIfError();

        /// <summary>
        /// <see cref="av_stream_new_side_data(AVStream*, AVPacketSideDataType, int)"/>
        /// </summary>
        public IntPtr NewSideData(AVPacketSideDataType type, int size) =>
            NativeUtils.NotNull((IntPtr)av_stream_new_side_data(this, type, size));

        /// <summary>
        /// <see cref="av_stream_get_side_data(AVStream*, AVPacketSideDataType, int*)"/>
        /// </summary>
        public DataPointer GetSideData(AVPacketSideDataType type)
        {
            int size;
            return new DataPointer(av_stream_get_side_data(this, type, &size), size);
        }

        /// <summary>
        /// <see cref="av_index_search_timestamp(AVStream*, long, int)"/>
        /// </summary>
        public int SearchTimestamp(long timestamp, AVSEEK_FLAG flags = AVSEEK_FLAG.Backward)
            => av_index_search_timestamp(this, timestamp, (int)flags);
    }
}
