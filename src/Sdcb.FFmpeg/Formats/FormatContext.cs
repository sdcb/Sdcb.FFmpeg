using Sdcb.FFmpeg.Codecs;
using Sdcb.FFmpeg.Common;
using Sdcb.FFmpeg.Raw;
using Sdcb.FFmpeg.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using static Sdcb.FFmpeg.Raw.ffmpeg;

namespace Sdcb.FFmpeg.Formats;

public unsafe partial class FormatContext : SafeHandle
{
    /// <summary>
    /// <see cref="avformat_alloc_context"/>
    /// </summary>
    public FormatContext() : base(NativeUtils.NotNull((IntPtr)avformat_alloc_context()), ownsHandle: true)
    {
    }

    public FFmpegOptions Options => new FFmpegOptions(this);

    /// <summary>
    /// Allocate an AVFormatContext for an output format. avformat_free_context() can be used to free the context and everything allocated by the framework within it.
    /// <param name="format">format to use for allocating the context, if NULL format_name and filename are used instead</param>
    /// <param name="formatName">the name of output format to use for allocating the context, if NULL filename is used instead</param>
    /// <param name="fileName">the name of the filename to use for allocating the context, may be NULL</param>
    /// <see cref="avformat_alloc_output_context2(AVFormatContext**, AVOutputFormat*, string, string)"/>
    /// </summary>
    public static FormatContext AllocOutput(OutputFormat? format = null, string? formatName = null, string? fileName = null)
    {
        AVFormatContext* ptr;
        avformat_alloc_output_context2(
            &ptr,
            format ?? null,
            formatName,
            fileName).ThrowIfError();
        return FromNative(ptr, isOwner: true);
    }

    /// <summary>
    /// Open an input stream and read the header. The codecs are not opened. The stream must be closed with avformat_close_input().
    /// <param name="url">URL of the stream to open.</param>
    /// <param name="format">If non-NULL, this parameter forces a specific input format. Otherwise the format is autodetected.</param>
    /// <param name="options">A dictionary filled with AVFormatContext and demuxer-private options. On return this parameter will be destroyed and replaced with a dict containing options that were not found. May be NULL.</param>
    /// <see cref="avformat_open_input(AVFormatContext**, string, AVInputFormat*, AVDictionary**)"/>
    /// </summary>
    public static FormatContext OpenInputUrl(string? url, InputFormat? format = null, MediaDictionary? options = null)
    {
        AVFormatContext* resultPtr;
        AVDictionary* dictPtr = options;
        avformat_open_input(&resultPtr, url, format, &dictPtr).ThrowIfError();
        options?.Reset(dictPtr);
        return new InputFormatContext(resultPtr, isOwner: true);
    }

    /// <summary>
    /// Open an input stream and read the header. The codecs are not opened. The stream must be closed with avformat_close_input().
    /// <param name="inputIO">the IOContext to open.</param>
    /// <param name="format">If non-NULL, this parameter forces a specific input format. Otherwise the format is autodetected.</param>
    /// <param name="options">A dictionary filled with AVFormatContext and demuxer-private options. On return this parameter will be destroyed and replaced with a dict containing options that were not found. May be NULL.</param>
    /// <see cref="avformat_open_input(AVFormatContext**, string, AVInputFormat*, AVDictionary**)"/>
    /// </summary>
    public static FormatContext OpenInputIO(IOContext inputIO, InputFormat? format = null, MediaDictionary? options = null)
    {
        AVFormatContext* resultPtr = avformat_alloc_context();
        AVDictionary* dictPtr = options;
        resultPtr->pb = inputIO;
        avformat_open_input(&resultPtr, null, format, &dictPtr).ThrowIfError();
        options?.Reset(dictPtr);
        return new InputFormatContext(resultPtr, isOwner: true);
    }

    /// <summary>
    /// <see cref="av_format_inject_global_side_data(AVFormatContext*)"/>
    /// </summary>
    public void InjectGlobalSideData() => av_format_inject_global_side_data(this);

    /// <summary>
    /// <see cref="avformat_new_stream(AVFormatContext*, AVCodec*)"/>
    /// </summary>
    public MediaStream NewStream(Codec? codec = null) => new MediaStream(this, codec);

    /// <summary>
    /// <see cref="av_new_program(AVFormatContext*, int)"/>
    /// </summary>
    public MediaProgram NewProgram(int id) => MediaProgram.FromNative(av_new_program(this, id));

    /// <summary>
    /// <see cref="avformat_find_stream_info(AVFormatContext*, AVDictionary**)"/>
    /// </summary>
    public void LoadStreamInfo()
    {
        avformat_find_stream_info(this, null).ThrowIfError();
    }

    /// <summary>
    /// <see cref="av_find_program_from_stream(AVFormatContext*, AVProgram*, int)"/>
    /// </summary>
    public MediaProgram? FindProgramFromStream(MediaProgram? last, int index)
        => MediaProgram.FromNativeOrNull(av_find_program_from_stream(this, last, index));

    /// <summary>
    /// <see cref="av_find_best_stream(AVFormatContext*, AVMediaType, int, int, AVCodec**, int)"/>
    /// </summary>
    public (int streamId, Codec codec)? FindBestStreamId(AVMediaType type, int wantedStreamId = -1, int relatedStream = -1)
    {
        AVCodec* ptr;
        int id = av_find_best_stream(this, type, wantedStreamId, relatedStream, &ptr, flags: 0);
        if (id == AVERROR_DECODER_NOT_FOUND) return null;

        id.ThrowIfError();

        return (id, Codec.FromNative(ptr));
    }

    /// <summary>
    /// <see cref="av_find_best_stream(AVFormatContext*, AVMediaType, int, int, AVCodec**, int)"/>
    /// </summary>
    public MediaStream FindBestStream(AVMediaType type, int wantedStreamId = -1, int relatedStream = -1)
    {
        int streamId = av_find_best_stream(this, type, wantedStreamId, relatedStream, decoder_ret: null, flags: 0).ThrowIfError();
        return MediaStream.FromNative(_ptr->streams[streamId]);
    }

    /// <summary>
    /// <see cref="av_find_best_stream(AVFormatContext*, AVMediaType, int, int, AVCodec**, int)"/>
    /// </summary>
    public MediaStream? FindBestStreamOrNull(AVMediaType type, int wantedStreamId = -1, int relatedStream = -1)
    {
        int streamId = av_find_best_stream(this, type, wantedStreamId, relatedStream, decoder_ret: null, flags: 0);
        return streamId switch
        {
            < 0 => null,
            var x => MediaStream.FromNative(_ptr->streams[streamId])
        };
    }

    /// <summary>
    /// <see cref="av_read_frame(AVFormatContext*, AVPacket*)"/>
    /// </summary>
    public CodecResult ReadFrame(Packet packet) => CodecContext.ToCodecResult(av_read_frame(this, packet));

    /// <summary>
    /// Read <see cref="Packet"/> from <see cref="FormatContext"/>, whitelisted by <paramref name="allowedStreamIndexs"/>
    /// </summary>
    /// <param name="allowedStreamIndexs">
    /// <list type="bullet">
    /// <item>If 0 element, then return all <see cref="Packet"/></item>
    /// <item>If have any element, then return all <see cref="Packet"/> corresponding to specific <paramref name="allowedStreamIndexs"/></item>
    /// </list>
    /// </param>
    public IEnumerable<Packet> ReadPackets(params int[] allowedStreamIndexs)
    {
        using Packet packet = new();
        while (true)
        {
            CodecResult result = ReadFrame(packet);
            if (result == CodecResult.EOF) break;
            System.Diagnostics.Debug.Assert(result == CodecResult.Success);

            if (allowedStreamIndexs.Length == 0 || allowedStreamIndexs.Length > 0 && allowedStreamIndexs.Contains(packet.StreamIndex))
            {
                yield return packet;
            }
            else
            {
                packet.Unref();
            }
        }
    }

    /// <summary>
    /// <see cref="av_seek_frame(AVFormatContext*, int, long, int)"/>
    /// </summary>
    public void SeekFrame(long timestamp, int streamIndex = -1, AVSEEK_FLAG flags = AVSEEK_FLAG.Backward) => av_seek_frame(this, streamIndex, timestamp, (int)flags).ThrowIfError();

    /// <summary>
    /// <see cref="avformat_flush(AVFormatContext*)"/>
    /// </summary>
    public void Flush() => avformat_flush(this).ThrowIfError();

    /// <summary>
    /// <see cref="av_read_play(AVFormatContext*)"/>
    /// </summary>
    public void ReadPlay() => av_read_play(this).ThrowIfError();

    /// <summary>
    /// <see cref="av_read_pause(AVFormatContext*)"/>
    /// </summary>
    public void ReadPause() => av_read_pause(this).ThrowIfError();

    /// <summary>
    /// <see cref="avformat_write_header(AVFormatContext*, AVDictionary**)"/>
    /// </summary>
    public AVSTREAM_INIT_IN WriteHeader(MediaDictionary? options = null)
    {
        AVDictionary* dictPtr = options;
        int ret = avformat_write_header(this, &dictPtr).ThrowIfError();
        options?.Reset(dictPtr);
        return (AVSTREAM_INIT_IN)ret;
    }

    /// <summary>
    /// <see cref="avformat_init_output(AVFormatContext*, AVDictionary**)"/>
    /// </summary>
    public AVSTREAM_INIT_IN InitOutput(MediaDictionary? options = null)
    {
        AVDictionary* dictPtr = options;
        int ret = avformat_init_output(this, &dictPtr).ThrowIfError();
        options?.Reset(dictPtr);
        return (AVSTREAM_INIT_IN)ret;
    }

    /// <summary>
    /// <see cref="av_write_frame(AVFormatContext*, AVPacket*)"/>
    /// </summary>
    public void WritePacket(Packet packet) => av_write_frame(this, packet).ThrowIfError();

    /// <summary>
    /// <see cref="av_interleaved_write_frame(AVFormatContext*, AVPacket*)"/>
    /// </summary>
    public void InterleavedWritePacket(Packet packet) => av_interleaved_write_frame(this, packet).ThrowIfError();

    /// <summary>
    /// <see cref="av_write_uncoded_frame(AVFormatContext*, int, AVFrame*)"/>
    /// </summary>
    public void WriteUncodedFrame(int streamIndex, Frame frame) => av_write_uncoded_frame(this, streamIndex, frame).ThrowIfError();

    /// <summary>
    /// <see cref="av_interleaved_write_uncoded_frame(AVFormatContext*, int, AVFrame*)"/>
    /// </summary>
    public void InterleavedWriteUncodedFrame(int streamIndex, Frame frame)
        => av_interleaved_write_uncoded_frame(this, streamIndex, frame);

    /// <summary>
    /// <see cref="av_write_uncoded_frame_query(AVFormatContext*, int)"/>
    /// </summary>
    /// <param name="streamIndex"></param>
    /// <returns></returns>
    public bool SupportWriteUncodedFrame(int streamIndex) => av_write_uncoded_frame_query(this, streamIndex) switch
    {
        >= 0 => true,
        < 0 => false
    };

    /// <summary>
    /// <see cref="av_write_trailer(AVFormatContext*)"/>
    /// </summary>
    public void WriteTrailer() => av_write_trailer(this).ThrowIfError();

    public (long dts, long wall) GetOutputTimestamp(int streamIndex)
    {
        long dts, wall;
        av_get_output_timestamp(this, streamIndex, &dts, &wall).ThrowIfError();
        return (dts, wall);
    }

    /// <summary>
    /// <see cref="DefaultStreamIndex"/>
    /// </summary>
    public int DefaultStreamIndex => av_find_default_stream_index(this);

    /// <summary>
    /// <see cref="av_dump_format(AVFormatContext*, int, string, int)"/>
    /// </summary>
    public void DumpFormat(int streamIndex, string? url, bool isOutput) => av_dump_format(this, streamIndex, url, isOutput ? 1 : 0);

    public MediaStream GetVideoStream() => FindBestStream(AVMediaType.Video);
    public MediaStream GetAudioStream() => FindBestStream(AVMediaType.Audio);
    public MediaStream GetSubtitleStream() => FindBestStream(AVMediaType.Subtitle);

    /// <summary>
    /// <see cref="avformat_free_context(AVFormatContext*)"/>
    /// </summary>
    public void Free()
    {
        avformat_free_context(this);
        SetHandleAsInvalid();
    }

    protected override bool ReleaseHandle()
    {
        Free();
        return true;
    }

    /// <summary>
    /// <see cref="avformat_version"/>
    /// </summary>
    public static string Version => avformat_version().ToFourCC();

    /// <summary>
    /// <see cref="avformat_configuration"/>
    /// </summary>
    public static HashSet<string> Configuration => avformat_configuration()
        .Split(' ')
        .ToHashSet();

    /// <summary>
    /// <see cref="avformat_license"/>
    /// </summary>
    /// <returns></returns>
    public static string License() => avformat_license();

    /// <summary>
    /// <see cref="avformat_get_class"/>
    /// </summary>
    public static FFmpegClass TypeClass => FFmpegClass.FromNative(avformat_get_class());
}