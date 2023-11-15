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
        [Obsolete("use av_packet_side_data_add() with the stream's  \"codecpar side data\"")]
        public void AddSideData(AVPacketSideDataType type, DataPointer data) =>
            av_stream_add_side_data(this, type, (byte*)data.Pointer, (ulong)data.Length).ThrowIfError();

        /// <summary>
        /// <see cref="av_stream_new_side_data(AVStream*, AVPacketSideDataType, ulong)"/>
        /// </summary>
        [Obsolete("use av_packet_side_data_new() with the stream's  \"codecpar side data\"")]
        public IntPtr NewSideData(AVPacketSideDataType type, long size) =>
            NativeUtils.NotNull((IntPtr)av_stream_new_side_data(this, type, (ulong)size));

        /// <summary>
        /// <see cref="av_stream_get_side_data(AVStream*, AVPacketSideDataType, ulong*)"/>
        /// </summary>
        [Obsolete("use av_packet_side_data_get() with the stream's  \"codecpar side data\"")]
        public DataPointer GetSideData(AVPacketSideDataType type)
        {
            ulong size;
            return new DataPointer(av_stream_get_side_data(this, type, &size), (int)size);
        }

        /// <summary>
        /// <see cref="av_index_search_timestamp(AVStream*, long, int)"/>
        /// </summary>
        public int SearchTimestamp(long timestamp, AVSEEK_FLAG flags = AVSEEK_FLAG.Backward)
            => av_index_search_timestamp(this, timestamp, (int)flags);
    }
}
