// This file was genereated from Sdcb.FFmpeg.AutoGen, DO NOT CHANGE DIRECTLY.
#nullable enable
using Sdcb.FFmpeg.Common;
using Sdcb.FFmpeg.Codecs;
using Sdcb.FFmpeg.Raw;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Sdcb.FFmpeg.Formats;

/// <summary>
/// <para>Format I/O context. New fields can be added to the end with minor version bumps. Removal, reordering and changes to existing fields require a major version bump. sizeof(AVFormatContext) must not be used outside libav*, use avformat_alloc_context() to create an AVFormatContext.</para>
/// <see cref="AVFormatContext" />
/// </summary>
public unsafe partial class FormatContext : SafeHandle
{
    protected AVFormatContext* _ptr => (AVFormatContext*)handle;
    
    public static implicit operator AVFormatContext*(FormatContext data) => data != null ? (AVFormatContext*)data.handle : null;
    
    protected FormatContext(AVFormatContext* ptr, bool isOwner): base(NativeUtils.NotNull((IntPtr)ptr), isOwner)
    {
    }
    
    public static FormatContext FromNative(AVFormatContext* ptr, bool isOwner) => new FormatContext(ptr, isOwner);
    
    public static FormatContext? FromNativeOrNull(AVFormatContext* ptr, bool isOwner) => ptr == null ? null : new FormatContext(ptr, isOwner);
    
    public override bool IsInvalid => handle == IntPtr.Zero;
    
    /// <summary>
    /// <para>original type: AVClass*</para>
    /// <para>A class for logging and avoptions. Set by avformat_alloc_context(). Exports (de)muxer private options if they exist.</para>
    /// <see cref="AVFormatContext.av_class" />
    /// </summary>
    public FFmpegClass AvClass
    {
        get => FFmpegClass.FromNative(_ptr->av_class);
        set => _ptr->av_class = (AVClass*)value;
    }
    
    /// <summary>
    /// <para>original type: AVInputFormat*</para>
    /// <para>The input container format.</para>
    /// <see cref="AVFormatContext.iformat" />
    /// </summary>
    public InputFormat? InputFormat
    {
        get => Sdcb.FFmpeg.Formats.InputFormat.FromNativeOrNull(_ptr->iformat);
        set => _ptr->iformat = (AVInputFormat*)value;
    }
    
    /// <summary>
    /// <para>original type: AVOutputFormat*</para>
    /// <para>The output container format.</para>
    /// <see cref="AVFormatContext.oformat" />
    /// </summary>
    public OutputFormat? OutputFormat
    {
        get => Sdcb.FFmpeg.Formats.OutputFormat.FromNativeOrNull(_ptr->oformat);
        set => _ptr->oformat = (AVOutputFormat*)value;
    }
    
    /// <summary>
    /// <para>original type: void*</para>
    /// <para>Format private data. This is an AVOptions-enabled struct if and only if iformat/oformat.priv_class is not NULL.</para>
    /// <see cref="AVFormatContext.priv_data" />
    /// </summary>
    public IntPtr PrivateData
    {
        get => (IntPtr)_ptr->priv_data;
        set => _ptr->priv_data = (void*)value;
    }
    
    /// <summary>
    /// <para>original type: AVIOContext*</para>
    /// <para>I/O context.</para>
    /// <see cref="AVFormatContext.pb" />
    /// </summary>
    public IOContext? Pb
    {
        get => IOContext.FromNativeOrNull(_ptr->pb, false);
        set => _ptr->pb = value != null ? (AVIOContext*)value : null;
    }
    
    /// <summary>
    /// <para>Flags signalling stream properties. A combination of AVFMTCTX_*. Set by libavformat.</para>
    /// <see cref="AVFormatContext.ctx_flags" />
    /// </summary>
    public int ContextFlags
    {
        get => _ptr->ctx_flags;
        set => _ptr->ctx_flags = value;
    }
    
    /// <summary>
    /// <para>original type: AVStream**</para>
    /// <para>A list of all streams in the file. New streams are created with avformat_new_stream().</para>
    /// <see cref="AVFormatContext.streams" />
    /// </summary>
    public IReadOnlyList<MediaStream> Streams => new ReadOnlyPtrList<AVStream, MediaStream>(_ptr->streams, (int)_ptr->nb_streams, MediaStream.FromNative)!;
    
    /// <summary>
    /// <para>original type: byte*</para>
    /// <para>input or output URL. Unlike the old filename field, this field has no length restriction.</para>
    /// <see cref="AVFormatContext.url" />
    /// </summary>
    public IntPtr Url
    {
        get => (IntPtr)_ptr->url;
        set => _ptr->url = (byte*)value;
    }
    
    /// <summary>
    /// <para>Position of the first frame of the component, in AV_TIME_BASE fractional seconds. NEVER set this value directly: It is deduced from the AVStream values.</para>
    /// <see cref="AVFormatContext.start_time" />
    /// </summary>
    public long StartTime
    {
        get => _ptr->start_time;
        set => _ptr->start_time = value;
    }
    
    /// <summary>
    /// <para>Duration of the stream, in AV_TIME_BASE fractional seconds. Only set this value if you know none of the individual stream durations and also do not set any of them. This is deduced from the AVStream values if not set.</para>
    /// <see cref="AVFormatContext.duration" />
    /// </summary>
    public long Duration
    {
        get => _ptr->duration;
        set => _ptr->duration = value;
    }
    
    /// <summary>
    /// <para>Total stream bitrate in bit/s, 0 if not available. Never set it directly if the file_size and the duration are known as FFmpeg can compute it automatically.</para>
    /// <see cref="AVFormatContext.bit_rate" />
    /// </summary>
    public long BitRate
    {
        get => _ptr->bit_rate;
        set => _ptr->bit_rate = value;
    }
    
    /// <summary>
    /// <see cref="AVFormatContext.packet_size" />
    /// </summary>
    public uint PacketSize
    {
        get => _ptr->packet_size;
        set => _ptr->packet_size = value;
    }
    
    /// <summary>
    /// <see cref="AVFormatContext.max_delay" />
    /// </summary>
    public int MaxDelay
    {
        get => _ptr->max_delay;
        set => _ptr->max_delay = value;
    }
    
    /// <summary>
    /// <para>original type: int</para>
    /// <para>Flags modifying the (de)muxer behaviour. A combination of AVFMT_FLAG_*. Set by the user before avformat_open_input() / avformat_write_header().</para>
    /// <see cref="AVFormatContext.flags" />
    /// </summary>
    public AVFMT_FLAG Flags
    {
        get => (AVFMT_FLAG)_ptr->flags;
        set => _ptr->flags = (int)value;
    }
    
    /// <summary>
    /// <para>Maximum number of bytes read from input in order to determine stream properties. Used when reading the global header and in avformat_find_stream_info().</para>
    /// <see cref="AVFormatContext.probesize" />
    /// </summary>
    public long Probesize
    {
        get => _ptr->probesize;
        set => _ptr->probesize = value;
    }
    
    /// <summary>
    /// <para>Maximum duration (in AV_TIME_BASE units) of the data read from input in avformat_find_stream_info(). Demuxing only, set by the caller before avformat_find_stream_info(). Can be set to 0 to let avformat choose using a heuristic.</para>
    /// <see cref="AVFormatContext.max_analyze_duration" />
    /// </summary>
    public long MaxAnalyzeDuration
    {
        get => _ptr->max_analyze_duration;
        set => _ptr->max_analyze_duration = value;
    }
    
    /// <summary>
    /// <para>original type: byte*</para>
    /// <see cref="AVFormatContext.key" />
    /// </summary>
    public IntPtr Key
    {
        get => (IntPtr)_ptr->key;
        set => _ptr->key = (byte*)value;
    }
    
    /// <summary>
    /// <see cref="AVFormatContext.keylen" />
    /// </summary>
    public int Keylen
    {
        get => _ptr->keylen;
        set => _ptr->keylen = value;
    }
    
    /// <summary>
    /// <para>original type: AVProgram**</para>
    /// <see cref="AVFormatContext.programs" />
    /// </summary>
    public IReadOnlyList<MediaProgram> Programs => new ReadOnlyPtrList<AVProgram, MediaProgram>(_ptr->programs, (int)_ptr->nb_programs, MediaProgram.FromNative)!;
    
    /// <summary>
    /// <para>Forced video codec_id. Demuxing: Set by user.</para>
    /// <see cref="AVFormatContext.video_codec_id" />
    /// </summary>
    public AVCodecID VideoCodecId
    {
        get => _ptr->video_codec_id;
        set => _ptr->video_codec_id = value;
    }
    
    /// <summary>
    /// <para>Forced audio codec_id. Demuxing: Set by user.</para>
    /// <see cref="AVFormatContext.audio_codec_id" />
    /// </summary>
    public AVCodecID AudioCodecId
    {
        get => _ptr->audio_codec_id;
        set => _ptr->audio_codec_id = value;
    }
    
    /// <summary>
    /// <para>Forced subtitle codec_id. Demuxing: Set by user.</para>
    /// <see cref="AVFormatContext.subtitle_codec_id" />
    /// </summary>
    public AVCodecID SubtitleCodecId
    {
        get => _ptr->subtitle_codec_id;
        set => _ptr->subtitle_codec_id = value;
    }
    
    /// <summary>
    /// <para>Maximum amount of memory in bytes to use for the index of each stream. If the index exceeds this size, entries will be discarded as needed to maintain a smaller size. This can lead to slower or less accurate seeking (depends on demuxer). Demuxers for which a full in-memory index is mandatory will ignore this. - muxing: unused - demuxing: set by user</para>
    /// <see cref="AVFormatContext.max_index_size" />
    /// </summary>
    public uint MaxIndexSize
    {
        get => _ptr->max_index_size;
        set => _ptr->max_index_size = value;
    }
    
    /// <summary>
    /// <para>Maximum amount of memory in bytes to use for buffering frames obtained from realtime capture devices.</para>
    /// <see cref="AVFormatContext.max_picture_buffer" />
    /// </summary>
    public uint MaxPictureBuffer
    {
        get => _ptr->max_picture_buffer;
        set => _ptr->max_picture_buffer = value;
    }
    
    /// <summary>
    /// <para>original type: AVChapter**</para>
    /// <see cref="AVFormatContext.chapters" />
    /// </summary>
    public IReadOnlyList<MediaChapter> Chapters => new ReadOnlyPtrList<AVChapter, MediaChapter>(_ptr->chapters, (int)_ptr->nb_chapters, MediaChapter.FromNative)!;
    
    /// <summary>
    /// <para>original type: AVDictionary*</para>
    /// <para>Metadata that applies to the whole file.</para>
    /// <see cref="AVFormatContext.metadata" />
    /// </summary>
    public MediaDictionary Metadata
    {
        get => MediaDictionary.FromNative(_ptr->metadata, false);
        set => _ptr->metadata = (AVDictionary*)value;
    }
    
    /// <summary>
    /// <para>Start time of the stream in real world time, in microseconds since the Unix epoch (00:00 1st January 1970). That is, pts=0 in the stream was captured at this real world time. - muxing: Set by the caller before avformat_write_header(). If set to either 0 or AV_NOPTS_VALUE, then the current wall-time will be used. - demuxing: Set by libavformat. AV_NOPTS_VALUE if unknown. Note that the value may become known after some number of frames have been received.</para>
    /// <see cref="AVFormatContext.start_time_realtime" />
    /// </summary>
    public long StartTimeRealtime
    {
        get => _ptr->start_time_realtime;
        set => _ptr->start_time_realtime = value;
    }
    
    /// <summary>
    /// <para>The number of frames used for determining the framerate in avformat_find_stream_info(). Demuxing only, set by the caller before avformat_find_stream_info().</para>
    /// <see cref="AVFormatContext.fps_probe_size" />
    /// </summary>
    public int FpsProbeSize
    {
        get => _ptr->fps_probe_size;
        set => _ptr->fps_probe_size = value;
    }
    
    /// <summary>
    /// <para>Error recognition; higher values will detect more errors but may misdetect some more or less valid parts as errors. Demuxing only, set by the caller before avformat_open_input().</para>
    /// <see cref="AVFormatContext.error_recognition" />
    /// </summary>
    public int ErrorRecognition
    {
        get => _ptr->error_recognition;
        set => _ptr->error_recognition = value;
    }
    
    /// <summary>
    /// <para>Custom interrupt callbacks for the I/O layer.</para>
    /// <see cref="AVFormatContext.interrupt_callback" />
    /// </summary>
    public AVIOInterruptCB InterruptCallback
    {
        get => _ptr->interrupt_callback;
        set => _ptr->interrupt_callback = value;
    }
    
    /// <summary>
    /// <para>Flags to enable debugging.</para>
    /// <see cref="AVFormatContext.debug" />
    /// </summary>
    public int Debug
    {
        get => _ptr->debug;
        set => _ptr->debug = value;
    }
    
    /// <summary>
    /// <para>Maximum buffering duration for interleaving.</para>
    /// <see cref="AVFormatContext.max_interleave_delta" />
    /// </summary>
    public long MaxInterleaveDelta
    {
        get => _ptr->max_interleave_delta;
        set => _ptr->max_interleave_delta = value;
    }
    
    /// <summary>
    /// <para>Allow non-standard and experimental extension</para>
    /// <see cref="AVFormatContext.strict_std_compliance" />
    /// </summary>
    public int StrictStdCompliance
    {
        get => _ptr->strict_std_compliance;
        set => _ptr->strict_std_compliance = value;
    }
    
    /// <summary>
    /// <para>Flags indicating events happening on the file, a combination of AVFMT_EVENT_FLAG_*.</para>
    /// <see cref="AVFormatContext.event_flags" />
    /// </summary>
    public int EventFlags
    {
        get => _ptr->event_flags;
        set => _ptr->event_flags = value;
    }
    
    /// <summary>
    /// <para>Maximum number of packets to read while waiting for the first timestamp. Decoding only.</para>
    /// <see cref="AVFormatContext.max_ts_probe" />
    /// </summary>
    public int MaxTsProbe
    {
        get => _ptr->max_ts_probe;
        set => _ptr->max_ts_probe = value;
    }
    
    /// <summary>
    /// <para>Avoid negative timestamps during muxing. Any value of the AVFMT_AVOID_NEG_TS_* constants. Note, this works better when using av_interleaved_write_frame(). - muxing: Set by user - demuxing: unused</para>
    /// <see cref="AVFormatContext.avoid_negative_ts" />
    /// </summary>
    public int AvoidNegativeTs
    {
        get => _ptr->avoid_negative_ts;
        set => _ptr->avoid_negative_ts = value;
    }
    
    /// <summary>
    /// <para>Transport stream id. This will be moved into demuxer private options. Thus no API/ABI compatibility</para>
    /// <see cref="AVFormatContext.ts_id" />
    /// </summary>
    public int TsId
    {
        get => _ptr->ts_id;
        set => _ptr->ts_id = value;
    }
    
    /// <summary>
    /// <para>Audio preload in microseconds. Note, not all formats support this and unpredictable things may happen if it is used when not supported. - encoding: Set by user - decoding: unused</para>
    /// <see cref="AVFormatContext.audio_preload" />
    /// </summary>
    public int AudioPreload
    {
        get => _ptr->audio_preload;
        set => _ptr->audio_preload = value;
    }
    
    /// <summary>
    /// <para>Max chunk time in microseconds. Note, not all formats support this and unpredictable things may happen if it is used when not supported. - encoding: Set by user - decoding: unused</para>
    /// <see cref="AVFormatContext.max_chunk_duration" />
    /// </summary>
    public int MaxChunkDuration
    {
        get => _ptr->max_chunk_duration;
        set => _ptr->max_chunk_duration = value;
    }
    
    /// <summary>
    /// <para>Max chunk size in bytes Note, not all formats support this and unpredictable things may happen if it is used when not supported. - encoding: Set by user - decoding: unused</para>
    /// <see cref="AVFormatContext.max_chunk_size" />
    /// </summary>
    public int MaxChunkSize
    {
        get => _ptr->max_chunk_size;
        set => _ptr->max_chunk_size = value;
    }
    
    /// <summary>
    /// <para>forces the use of wallclock timestamps as pts/dts of packets This has undefined results in the presence of B frames. - encoding: unused - decoding: Set by user</para>
    /// <see cref="AVFormatContext.use_wallclock_as_timestamps" />
    /// </summary>
    public int UseWallclockAsTimestamps
    {
        get => _ptr->use_wallclock_as_timestamps;
        set => _ptr->use_wallclock_as_timestamps = value;
    }
    
    /// <summary>
    /// <para>avio flags, used to force AVIO_FLAG_DIRECT. - encoding: unused - decoding: Set by user</para>
    /// <see cref="AVFormatContext.avio_flags" />
    /// </summary>
    public int AvioFlags
    {
        get => _ptr->avio_flags;
        set => _ptr->avio_flags = value;
    }
    
    /// <summary>
    /// <para>The duration field can be estimated through various ways, and this field can be used to know how the duration was estimated. - encoding: unused - decoding: Read by user</para>
    /// <see cref="AVFormatContext.duration_estimation_method" />
    /// </summary>
    public AVDurationEstimationMethod DurationEstimationMethod
    {
        get => _ptr->duration_estimation_method;
        set => _ptr->duration_estimation_method = value;
    }
    
    /// <summary>
    /// <para>Skip initial bytes when opening stream - encoding: unused - decoding: Set by user</para>
    /// <see cref="AVFormatContext.skip_initial_bytes" />
    /// </summary>
    public long SkipInitialBytes
    {
        get => _ptr->skip_initial_bytes;
        set => _ptr->skip_initial_bytes = value;
    }
    
    /// <summary>
    /// <para>Correct single timestamp overflows - encoding: unused - decoding: Set by user</para>
    /// <see cref="AVFormatContext.correct_ts_overflow" />
    /// </summary>
    public uint CorrectTsOverflow
    {
        get => _ptr->correct_ts_overflow;
        set => _ptr->correct_ts_overflow = value;
    }
    
    /// <summary>
    /// <para>Force seeking to any (also non key) frames. - encoding: unused - decoding: Set by user</para>
    /// <see cref="AVFormatContext.seek2any" />
    /// </summary>
    public int Seek2any
    {
        get => _ptr->seek2any;
        set => _ptr->seek2any = value;
    }
    
    /// <summary>
    /// <para>Flush the I/O context after each packet. - encoding: Set by user - decoding: unused</para>
    /// <see cref="AVFormatContext.flush_packets" />
    /// </summary>
    public int FlushPackets
    {
        get => _ptr->flush_packets;
        set => _ptr->flush_packets = value;
    }
    
    /// <summary>
    /// <para>format probing score. The maximal score is AVPROBE_SCORE_MAX, its set when the demuxer probes the format. - encoding: unused - decoding: set by avformat, read by user</para>
    /// <see cref="AVFormatContext.probe_score" />
    /// </summary>
    public int ProbeScore
    {
        get => _ptr->probe_score;
        set => _ptr->probe_score = value;
    }
    
    /// <summary>
    /// <para>Maximum number of bytes read from input in order to identify the AVInputFormat "input format". Only used when the format is not set explicitly by the caller.</para>
    /// <see cref="AVFormatContext.format_probesize" />
    /// </summary>
    public int FormatProbesize
    {
        get => _ptr->format_probesize;
        set => _ptr->format_probesize = value;
    }
    
    /// <summary>
    /// <para>original type: byte*</para>
    /// <para>',' separated list of allowed decoders. If NULL then all are allowed - encoding: unused - decoding: set by user</para>
    /// <see cref="AVFormatContext.codec_whitelist" />
    /// </summary>
    public IntPtr CodecWhitelist
    {
        get => (IntPtr)_ptr->codec_whitelist;
        set => _ptr->codec_whitelist = (byte*)value;
    }
    
    /// <summary>
    /// <para>original type: byte*</para>
    /// <para>',' separated list of allowed demuxers. If NULL then all are allowed - encoding: unused - decoding: set by user</para>
    /// <see cref="AVFormatContext.format_whitelist" />
    /// </summary>
    public IntPtr FormatWhitelist
    {
        get => (IntPtr)_ptr->format_whitelist;
        set => _ptr->format_whitelist = (byte*)value;
    }
    
    /// <summary>
    /// <para>IO repositioned flag. This is set by avformat when the underlaying IO context read pointer is repositioned, for example when doing byte based seeking. Demuxers can use the flag to detect such changes.</para>
    /// <see cref="AVFormatContext.io_repositioned" />
    /// </summary>
    public int IoRepositioned
    {
        get => _ptr->io_repositioned;
        set => _ptr->io_repositioned = value;
    }
    
    /// <summary>
    /// <para>original type: AVCodec*</para>
    /// <para>Forced video codec. This allows forcing a specific decoder, even when there are multiple with the same codec_id. Demuxing: Set by user</para>
    /// <see cref="AVFormatContext.video_codec" />
    /// </summary>
    public Codec? VideoCodec
    {
        get => Codec.FromNativeOrNull(_ptr->video_codec);
        set => _ptr->video_codec = (AVCodec*)value;
    }
    
    /// <summary>
    /// <para>original type: AVCodec*</para>
    /// <para>Forced audio codec. This allows forcing a specific decoder, even when there are multiple with the same codec_id. Demuxing: Set by user</para>
    /// <see cref="AVFormatContext.audio_codec" />
    /// </summary>
    public Codec? AudioCodec
    {
        get => Codec.FromNativeOrNull(_ptr->audio_codec);
        set => _ptr->audio_codec = (AVCodec*)value;
    }
    
    /// <summary>
    /// <para>original type: AVCodec*</para>
    /// <para>Forced subtitle codec. This allows forcing a specific decoder, even when there are multiple with the same codec_id. Demuxing: Set by user</para>
    /// <see cref="AVFormatContext.subtitle_codec" />
    /// </summary>
    public Codec? SubtitleCodec
    {
        get => Codec.FromNativeOrNull(_ptr->subtitle_codec);
        set => _ptr->subtitle_codec = (AVCodec*)value;
    }
    
    /// <summary>
    /// <para>original type: AVCodec*</para>
    /// <para>Forced data codec. This allows forcing a specific decoder, even when there are multiple with the same codec_id. Demuxing: Set by user</para>
    /// <see cref="AVFormatContext.data_codec" />
    /// </summary>
    public Codec? DataCodec
    {
        get => Codec.FromNativeOrNull(_ptr->data_codec);
        set => _ptr->data_codec = (AVCodec*)value;
    }
    
    /// <summary>
    /// <para>Number of bytes to be written as padding in a metadata header. Demuxing: Unused. Muxing: Set by user via av_format_set_metadata_header_padding.</para>
    /// <see cref="AVFormatContext.metadata_header_padding" />
    /// </summary>
    public int MetadataHeaderPadding
    {
        get => _ptr->metadata_header_padding;
        set => _ptr->metadata_header_padding = value;
    }
    
    /// <summary>
    /// <para>original type: void*</para>
    /// <para>User data. This is a place for some private data of the user.</para>
    /// <see cref="AVFormatContext.opaque" />
    /// </summary>
    public IntPtr Opaque
    {
        get => (IntPtr)_ptr->opaque;
        set => _ptr->opaque = (void*)value;
    }
    
    /// <summary>
    /// <para>Output timestamp offset, in microseconds. Muxing: set by user</para>
    /// <see cref="AVFormatContext.output_ts_offset" />
    /// </summary>
    public long OutputTsOffset
    {
        get => _ptr->output_ts_offset;
        set => _ptr->output_ts_offset = value;
    }
    
    /// <summary>
    /// <para>original type: byte*</para>
    /// <para>dump format separator. can be ", " or " " or anything else - muxing: Set by user. - demuxing: Set by user.</para>
    /// <see cref="AVFormatContext.dump_separator" />
    /// </summary>
    public string? DumpSeparator
    {
        get => _ptr->dump_separator != null ? PtrExtensions.PtrToStringUTF8((IntPtr)_ptr->dump_separator)! : null;
        set => Options.Set("dump_separator", value);
    }
    
    /// <summary>
    /// <para>Forced Data codec_id. Demuxing: Set by user.</para>
    /// <see cref="AVFormatContext.data_codec_id" />
    /// </summary>
    public AVCodecID DataCodecId
    {
        get => _ptr->data_codec_id;
        set => _ptr->data_codec_id = value;
    }
    
    /// <summary>
    /// <para>original type: byte*</para>
    /// <para>',' separated list of allowed protocols. - encoding: unused - decoding: set by user</para>
    /// <see cref="AVFormatContext.protocol_whitelist" />
    /// </summary>
    public string? ProtocolWhitelist
    {
        get => _ptr->protocol_whitelist != null ? PtrExtensions.PtrToStringUTF8((IntPtr)_ptr->protocol_whitelist)! : null;
        set => Options.Set("protocol_whitelist", value);
    }
    
    /// <summary>
    /// <para>original type: byte*</para>
    /// <para>',' separated list of disallowed protocols. - encoding: unused - decoding: set by user</para>
    /// <see cref="AVFormatContext.protocol_blacklist" />
    /// </summary>
    public string? ProtocolBlacklist
    {
        get => _ptr->protocol_blacklist != null ? PtrExtensions.PtrToStringUTF8((IntPtr)_ptr->protocol_blacklist)! : null;
        set => Options.Set("protocol_blacklist", value);
    }
    
    /// <summary>
    /// <para>The maximum number of streams. - encoding: unused - decoding: set by user</para>
    /// <see cref="AVFormatContext.max_streams" />
    /// </summary>
    public int MaxStreams
    {
        get => _ptr->max_streams;
        set => _ptr->max_streams = value;
    }
    
    /// <summary>
    /// <para>Skip duration calcuation in estimate_timings_from_pts. - encoding: unused - decoding: set by user</para>
    /// <see cref="AVFormatContext.skip_estimate_duration_from_pts" />
    /// </summary>
    public int SkipEstimateDurationFromPts
    {
        get => _ptr->skip_estimate_duration_from_pts;
        set => _ptr->skip_estimate_duration_from_pts = value;
    }
    
    /// <summary>
    /// <para>Maximum number of packets that can be probed - encoding: unused - decoding: set by user</para>
    /// <see cref="AVFormatContext.max_probe_packets" />
    /// </summary>
    public int MaxProbePackets
    {
        get => _ptr->max_probe_packets;
        set => _ptr->max_probe_packets = value;
    }
    
}
