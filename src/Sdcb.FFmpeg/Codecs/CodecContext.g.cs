// This file was genereated from Sdcb.FFmpeg.AutoGen, DO NOT CHANGE DIRECTLY.
#nullable enable
using Sdcb.FFmpeg.Common;
using Sdcb.FFmpeg.Formats;
using Sdcb.FFmpeg.Utils;
using Sdcb.FFmpeg.Filters;
using Sdcb.FFmpeg.Raw;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Sdcb.FFmpeg.Codecs;

/// <summary>
/// <para>main external API structure. New fields can be added to the end with minor version bumps. Removal, reordering and changes to existing fields require a major version bump. You can use AVOptions (av_opt* / av_set/get*()) to access these fields from user applications. The name string for AVOptions options matches the associated command line parameter name and can be found in libavcodec/options_table.h The AVOption/command line parameter names differ in some cases from the C structure field names for historic reasons or brevity. sizeof(AVCodecContext) must not be used outside libav*.</para>
/// <see cref="AVCodecContext" />
/// </summary>
public unsafe partial class CodecContext : SafeHandle
{
    protected AVCodecContext* _ptr => (AVCodecContext*)handle;
    
    public static implicit operator AVCodecContext*(CodecContext data) => data != null ? (AVCodecContext*)data.handle : null;
    
    protected CodecContext(AVCodecContext* ptr, bool isOwner): base(NativeUtils.NotNull((IntPtr)ptr), isOwner)
    {
    }
    
    public static CodecContext FromNative(AVCodecContext* ptr, bool isOwner) => new CodecContext(ptr, isOwner);
    
    internal static CodecContext FromNative(IntPtr ptr, bool isOwner) => new CodecContext((AVCodecContext*)ptr, isOwner);
    
    public static CodecContext? FromNativeOrNull(AVCodecContext* ptr, bool isOwner) => ptr == null ? null : new CodecContext(ptr, isOwner);
    
    public override bool IsInvalid => handle == IntPtr.Zero;
    
    /// <summary>
    /// <para>original type: AVClass*</para>
    /// <para>information on struct for av_log - set by avcodec_alloc_context3</para>
    /// <see cref="AVCodecContext.av_class" />
    /// </summary>
    public FFmpegClass AvClass
    {
        get => FFmpegClass.FromNative(_ptr->av_class);
        set => _ptr->av_class = (AVClass*)value;
    }
    
    /// <summary>
    /// <see cref="AVCodecContext.log_level_offset" />
    /// </summary>
    public int LogLevelOffset
    {
        get => _ptr->log_level_offset;
        set => _ptr->log_level_offset = value;
    }
    
    /// <summary>
    /// <see cref="AVCodecContext.codec_type" />
    /// </summary>
    public AVMediaType CodecType
    {
        get => _ptr->codec_type;
        set => _ptr->codec_type = value;
    }
    
    /// <summary>
    /// <para>original type: AVCodec*</para>
    /// <see cref="AVCodecContext.codec" />
    /// </summary>
    public Codec Codec
    {
        get => Codec.FromNative(_ptr->codec);
        set => _ptr->codec = (AVCodec*)value;
    }
    
    /// <summary>
    /// <see cref="AVCodecContext.codec_id" />
    /// </summary>
    public AVCodecID CodecId
    {
        get => _ptr->codec_id;
        set => _ptr->codec_id = value;
    }
    
    /// <summary>
    /// <para>fourcc (LSB first, so "ABCD" -&gt; ('D'&lt;&lt;24) + ('C'&lt;&lt;16) + ('B'&lt;&lt;8) + 'A'). This is used to work around some encoder bugs. A demuxer should set this to what is stored in the field used to identify the codec. If there are multiple such fields in a container then the demuxer should choose the one which maximizes the information about the used codec. If the codec tag field in a container is larger than 32 bits then the demuxer should remap the longer ID to 32 bits with a table or other structure. Alternatively a new extra_codec_tag + size could be added but for this a clear advantage must be demonstrated first. - encoding: Set by user, if not then the default based on codec_id will be used. - decoding: Set by user, will be converted to uppercase by libavcodec during init.</para>
    /// <see cref="AVCodecContext.codec_tag" />
    /// </summary>
    public uint CodecTag
    {
        get => _ptr->codec_tag;
        set => _ptr->codec_tag = value;
    }
    
    /// <summary>
    /// <para>original type: void*</para>
    /// <see cref="AVCodecContext.priv_data" />
    /// </summary>
    public IntPtr PrivateData
    {
        get => (IntPtr)_ptr->priv_data;
        set => _ptr->priv_data = (void*)value;
    }
    
    /// <summary>
    /// <para>Private context used for internal data.</para>
    /// <see cref="AVCodecContext.@internal" />
    /// </summary>
    public AVCodecInternal* Internal
    {
        get => _ptr->@internal;
        set => _ptr->@internal = value;
    }
    
    /// <summary>
    /// <para>original type: void*</para>
    /// <para>Private data of the user, can be used to carry app specific stuff. - encoding: Set by user. - decoding: Set by user.</para>
    /// <see cref="AVCodecContext.opaque" />
    /// </summary>
    public IntPtr Opaque
    {
        get => (IntPtr)_ptr->opaque;
        set => _ptr->opaque = (void*)value;
    }
    
    /// <summary>
    /// <para>the average bitrate - encoding: Set by user; unused for constant quantizer encoding. - decoding: Set by user, may be overwritten by libavcodec if this info is available in the stream</para>
    /// <see cref="AVCodecContext.bit_rate" />
    /// </summary>
    public long BitRate
    {
        get => _ptr->bit_rate;
        set => _ptr->bit_rate = value;
    }
    
    /// <summary>
    /// <para>number of bits the bitstream is allowed to diverge from the reference. the reference can be CBR (for CBR pass1) or VBR (for pass2) - encoding: Set by user; unused for constant quantizer encoding. - decoding: unused</para>
    /// <see cref="AVCodecContext.bit_rate_tolerance" />
    /// </summary>
    public int BitRateTolerance
    {
        get => _ptr->bit_rate_tolerance;
        set => _ptr->bit_rate_tolerance = value;
    }
    
    /// <summary>
    /// <para>Global quality for codecs which cannot change it per frame. This should be proportional to MPEG-1/2/4 qscale. - encoding: Set by user. - decoding: unused</para>
    /// <see cref="AVCodecContext.global_quality" />
    /// </summary>
    public int GlobalQuality
    {
        get => _ptr->global_quality;
        set => _ptr->global_quality = value;
    }
    
    /// <summary>
    /// <para>- encoding: Set by user. - decoding: unused</para>
    /// <see cref="AVCodecContext.compression_level" />
    /// </summary>
    public int CompressionLevel
    {
        get => _ptr->compression_level;
        set => _ptr->compression_level = value;
    }
    
    /// <summary>
    /// <para>original type: int</para>
    /// <para>AV_CODEC_FLAG_*. - encoding: Set by user. - decoding: Set by user.</para>
    /// <see cref="AVCodecContext.flags" />
    /// </summary>
    public AV_CODEC_FLAG Flags
    {
        get => (AV_CODEC_FLAG)_ptr->flags;
        set => _ptr->flags = (int)value;
    }
    
    /// <summary>
    /// <para>AV_CODEC_FLAG2_* - encoding: Set by user. - decoding: Set by user.</para>
    /// <see cref="AVCodecContext.flags2" />
    /// </summary>
    public int Flags2
    {
        get => _ptr->flags2;
        set => _ptr->flags2 = value;
    }
    
    /// <summary>
    /// <para>original type: byte*</para>
    /// <para>some codecs need / can use extradata like Huffman tables. MJPEG: Huffman tables rv10: additional flags MPEG-4: global headers (they can be in the bitstream or here) The allocated memory should be AV_INPUT_BUFFER_PADDING_SIZE bytes larger than extradata_size to avoid problems if it is read with the bitstream reader. The bytewise contents of extradata must not depend on the architecture or CPU endianness. Must be allocated with the av_malloc() family of functions. - encoding: Set/allocated/freed by libavcodec. - decoding: Set/allocated/freed by user.</para>
    /// <see cref="AVCodecContext.extradata" />
    /// </summary>
    public IntPtr Extradata
    {
        get => (IntPtr)_ptr->extradata;
        set => _ptr->extradata = (byte*)value;
    }
    
    /// <summary>
    /// <see cref="AVCodecContext.extradata_size" />
    /// </summary>
    public int ExtradataSize
    {
        get => _ptr->extradata_size;
        set => _ptr->extradata_size = value;
    }
    
    /// <summary>
    /// <para>This is the fundamental unit of time (in seconds) in terms of which frame timestamps are represented. For fixed-fps content, timebase should be 1/framerate and timestamp increments should be identically 1. This often, but not always is the inverse of the frame rate or field rate for video. 1/time_base is not the average frame rate if the frame rate is not constant.</para>
    /// <see cref="AVCodecContext.time_base" />
    /// </summary>
    public AVRational TimeBase
    {
        get => _ptr->time_base;
        set => _ptr->time_base = value;
    }
    
    /// <summary>
    /// <para>For some codecs, the time base is closer to the field rate than the frame rate. Most notably, H.264 and MPEG-2 specify time_base as half of frame duration if no telecine is used ...</para>
    /// <see cref="AVCodecContext.ticks_per_frame" />
    /// </summary>
    public int TicksPerFrame
    {
        get => _ptr->ticks_per_frame;
        set => _ptr->ticks_per_frame = value;
    }
    
    /// <summary>
    /// <para>Codec delay.</para>
    /// <see cref="AVCodecContext.delay" />
    /// </summary>
    public int Delay
    {
        get => _ptr->delay;
        set => _ptr->delay = value;
    }
    
    /// <summary>
    /// <para>picture width / height.</para>
    /// <see cref="AVCodecContext.width" />
    /// </summary>
    public int Width
    {
        get => _ptr->width;
        set => _ptr->width = value;
    }
    
    /// <summary>
    /// <para>picture width / height.</para>
    /// <see cref="AVCodecContext.height" />
    /// </summary>
    public int Height
    {
        get => _ptr->height;
        set => _ptr->height = value;
    }
    
    /// <summary>
    /// <para>Bitstream width / height, may be different from width/height e.g. when the decoded frame is cropped before being output or lowres is enabled.</para>
    /// <see cref="AVCodecContext.coded_width" />
    /// </summary>
    public int CodedWidth
    {
        get => _ptr->coded_width;
        set => _ptr->coded_width = value;
    }
    
    /// <summary>
    /// <para>Bitstream width / height, may be different from width/height e.g. when the decoded frame is cropped before being output or lowres is enabled.</para>
    /// <see cref="AVCodecContext.coded_height" />
    /// </summary>
    public int CodedHeight
    {
        get => _ptr->coded_height;
        set => _ptr->coded_height = value;
    }
    
    /// <summary>
    /// <para>the number of pictures in a group of pictures, or 0 for intra_only - encoding: Set by user. - decoding: unused</para>
    /// <see cref="AVCodecContext.gop_size" />
    /// </summary>
    public int GopSize
    {
        get => _ptr->gop_size;
        set => _ptr->gop_size = value;
    }
    
    /// <summary>
    /// <para>Pixel format, see AV_PIX_FMT_xxx. May be set by the demuxer if known from headers. May be overridden by the decoder if it knows better.</para>
    /// <see cref="AVCodecContext.pix_fmt" />
    /// </summary>
    public AVPixelFormat PixelFormat
    {
        get => _ptr->pix_fmt;
        set => _ptr->pix_fmt = value;
    }
    
    /// <summary>
    /// <para>maximum number of B-frames between non-B-frames Note: The output will be delayed by max_b_frames+1 relative to the input. - encoding: Set by user. - decoding: unused</para>
    /// <see cref="AVCodecContext.max_b_frames" />
    /// </summary>
    public int MaxBFrames
    {
        get => _ptr->max_b_frames;
        set => _ptr->max_b_frames = value;
    }
    
    /// <summary>
    /// <para>qscale factor between IP and B-frames If &gt; 0 then the last P-frame quantizer will be used (q= lastp_q*factor+offset). If &lt; 0 then normal ratecontrol will be done (q= -normal_q*factor+offset). - encoding: Set by user. - decoding: unused</para>
    /// <see cref="AVCodecContext.b_quant_factor" />
    /// </summary>
    public float BQuantFactor
    {
        get => _ptr->b_quant_factor;
        set => _ptr->b_quant_factor = value;
    }
    
    /// <summary>
    /// <para>qscale offset between IP and B-frames - encoding: Set by user. - decoding: unused</para>
    /// <see cref="AVCodecContext.b_quant_offset" />
    /// </summary>
    public float BQuantOffset
    {
        get => _ptr->b_quant_offset;
        set => _ptr->b_quant_offset = value;
    }
    
    /// <summary>
    /// <para>Size of the frame reordering buffer in the decoder. For MPEG-2 it is 1 IPB or 0 low delay IP. - encoding: Set by libavcodec. - decoding: Set by libavcodec.</para>
    /// <see cref="AVCodecContext.has_b_frames" />
    /// </summary>
    public int HasBFrames
    {
        get => _ptr->has_b_frames;
        set => _ptr->has_b_frames = value;
    }
    
    /// <summary>
    /// <para>qscale factor between P- and I-frames If &gt; 0 then the last P-frame quantizer will be used (q = lastp_q * factor + offset). If &lt; 0 then normal ratecontrol will be done (q= -normal_q*factor+offset). - encoding: Set by user. - decoding: unused</para>
    /// <see cref="AVCodecContext.i_quant_factor" />
    /// </summary>
    public float IQuantFactor
    {
        get => _ptr->i_quant_factor;
        set => _ptr->i_quant_factor = value;
    }
    
    /// <summary>
    /// <para>qscale offset between P and I-frames - encoding: Set by user. - decoding: unused</para>
    /// <see cref="AVCodecContext.i_quant_offset" />
    /// </summary>
    public float IQuantOffset
    {
        get => _ptr->i_quant_offset;
        set => _ptr->i_quant_offset = value;
    }
    
    /// <summary>
    /// <para>luminance masking (0-&gt; disabled) - encoding: Set by user. - decoding: unused</para>
    /// <see cref="AVCodecContext.lumi_masking" />
    /// </summary>
    public float LumiMasking
    {
        get => _ptr->lumi_masking;
        set => _ptr->lumi_masking = value;
    }
    
    /// <summary>
    /// <para>temporary complexity masking (0-&gt; disabled) - encoding: Set by user. - decoding: unused</para>
    /// <see cref="AVCodecContext.temporal_cplx_masking" />
    /// </summary>
    public float TemporalCplxMasking
    {
        get => _ptr->temporal_cplx_masking;
        set => _ptr->temporal_cplx_masking = value;
    }
    
    /// <summary>
    /// <para>spatial complexity masking (0-&gt; disabled) - encoding: Set by user. - decoding: unused</para>
    /// <see cref="AVCodecContext.spatial_cplx_masking" />
    /// </summary>
    public float SpatialCplxMasking
    {
        get => _ptr->spatial_cplx_masking;
        set => _ptr->spatial_cplx_masking = value;
    }
    
    /// <summary>
    /// <para>p block masking (0-&gt; disabled) - encoding: Set by user. - decoding: unused</para>
    /// <see cref="AVCodecContext.p_masking" />
    /// </summary>
    public float PMasking
    {
        get => _ptr->p_masking;
        set => _ptr->p_masking = value;
    }
    
    /// <summary>
    /// <para>darkness masking (0-&gt; disabled) - encoding: Set by user. - decoding: unused</para>
    /// <see cref="AVCodecContext.dark_masking" />
    /// </summary>
    public float DarkMasking
    {
        get => _ptr->dark_masking;
        set => _ptr->dark_masking = value;
    }
    
    /// <summary>
    /// <para>slice count - encoding: Set by libavcodec. - decoding: Set by user (or 0).</para>
    /// <see cref="AVCodecContext.slice_count" />
    /// </summary>
    public int SliceCount
    {
        get => _ptr->slice_count;
        set => _ptr->slice_count = value;
    }
    
    /// <summary>
    /// <para>slice offsets in the frame in bytes - encoding: Set/allocated by libavcodec. - decoding: Set/allocated by user (or NULL).</para>
    /// <see cref="AVCodecContext.slice_offset" />
    /// </summary>
    public int* SliceOffset
    {
        get => _ptr->slice_offset;
        set => _ptr->slice_offset = value;
    }
    
    /// <summary>
    /// <para>sample aspect ratio (0 if unknown) That is the width of a pixel divided by the height of the pixel. Numerator and denominator must be relatively prime and smaller than 256 for some video standards. - encoding: Set by user. - decoding: Set by libavcodec.</para>
    /// <see cref="AVCodecContext.sample_aspect_ratio" />
    /// </summary>
    public AVRational SampleAspectRatio
    {
        get => _ptr->sample_aspect_ratio;
        set => _ptr->sample_aspect_ratio = value;
    }
    
    /// <summary>
    /// <para>motion estimation comparison function - encoding: Set by user. - decoding: unused</para>
    /// <see cref="AVCodecContext.me_cmp" />
    /// </summary>
    public int MeCmp
    {
        get => _ptr->me_cmp;
        set => _ptr->me_cmp = value;
    }
    
    /// <summary>
    /// <para>subpixel motion estimation comparison function - encoding: Set by user. - decoding: unused</para>
    /// <see cref="AVCodecContext.me_sub_cmp" />
    /// </summary>
    public int MeSubCmp
    {
        get => _ptr->me_sub_cmp;
        set => _ptr->me_sub_cmp = value;
    }
    
    /// <summary>
    /// <para>macroblock comparison function (not supported yet) - encoding: Set by user. - decoding: unused</para>
    /// <see cref="AVCodecContext.mb_cmp" />
    /// </summary>
    public int MbCmp
    {
        get => _ptr->mb_cmp;
        set => _ptr->mb_cmp = value;
    }
    
    /// <summary>
    /// <para>interlaced DCT comparison function - encoding: Set by user. - decoding: unused</para>
    /// <see cref="AVCodecContext.ildct_cmp" />
    /// </summary>
    public int IldctCmp
    {
        get => _ptr->ildct_cmp;
        set => _ptr->ildct_cmp = value;
    }
    
    /// <summary>
    /// <para>ME diamond size &amp; shape - encoding: Set by user. - decoding: unused</para>
    /// <see cref="AVCodecContext.dia_size" />
    /// </summary>
    public int DiaSize
    {
        get => _ptr->dia_size;
        set => _ptr->dia_size = value;
    }
    
    /// <summary>
    /// <para>amount of previous MV predictors (2a+1 x 2a+1 square) - encoding: Set by user. - decoding: unused</para>
    /// <see cref="AVCodecContext.last_predictor_count" />
    /// </summary>
    public int LastPredictorCount
    {
        get => _ptr->last_predictor_count;
        set => _ptr->last_predictor_count = value;
    }
    
    /// <summary>
    /// <para>motion estimation prepass comparison function - encoding: Set by user. - decoding: unused</para>
    /// <see cref="AVCodecContext.me_pre_cmp" />
    /// </summary>
    public int MePreCmp
    {
        get => _ptr->me_pre_cmp;
        set => _ptr->me_pre_cmp = value;
    }
    
    /// <summary>
    /// <para>ME prepass diamond size &amp; shape - encoding: Set by user. - decoding: unused</para>
    /// <see cref="AVCodecContext.pre_dia_size" />
    /// </summary>
    public int PreDiaSize
    {
        get => _ptr->pre_dia_size;
        set => _ptr->pre_dia_size = value;
    }
    
    /// <summary>
    /// <para>subpel ME quality - encoding: Set by user. - decoding: unused</para>
    /// <see cref="AVCodecContext.me_subpel_quality" />
    /// </summary>
    public int MeSubpelQuality
    {
        get => _ptr->me_subpel_quality;
        set => _ptr->me_subpel_quality = value;
    }
    
    /// <summary>
    /// <para>maximum motion estimation search range in subpel units If 0 then no limit.</para>
    /// <see cref="AVCodecContext.me_range" />
    /// </summary>
    public int MeRange
    {
        get => _ptr->me_range;
        set => _ptr->me_range = value;
    }
    
    /// <summary>
    /// <para>slice flags - encoding: unused - decoding: Set by user.</para>
    /// <see cref="AVCodecContext.slice_flags" />
    /// </summary>
    public int SliceFlags
    {
        get => _ptr->slice_flags;
        set => _ptr->slice_flags = value;
    }
    
    /// <summary>
    /// <para>macroblock decision mode - encoding: Set by user. - decoding: unused</para>
    /// <see cref="AVCodecContext.mb_decision" />
    /// </summary>
    public int MbDecision
    {
        get => _ptr->mb_decision;
        set => _ptr->mb_decision = value;
    }
    
    /// <summary>
    /// <para>custom intra quantization matrix Must be allocated with the av_malloc() family of functions, and will be freed in avcodec_free_context(). - encoding: Set/allocated by user, freed by libavcodec. Can be NULL. - decoding: Set/allocated/freed by libavcodec.</para>
    /// <see cref="AVCodecContext.intra_matrix" />
    /// </summary>
    public ushort* IntraMatrix
    {
        get => _ptr->intra_matrix;
        set => _ptr->intra_matrix = value;
    }
    
    /// <summary>
    /// <para>custom inter quantization matrix Must be allocated with the av_malloc() family of functions, and will be freed in avcodec_free_context(). - encoding: Set/allocated by user, freed by libavcodec. Can be NULL. - decoding: Set/allocated/freed by libavcodec.</para>
    /// <see cref="AVCodecContext.inter_matrix" />
    /// </summary>
    public ushort* InterMatrix
    {
        get => _ptr->inter_matrix;
        set => _ptr->inter_matrix = value;
    }
    
    /// <summary>
    /// <para>precision of the intra DC coefficient - 8 - encoding: Set by user. - decoding: Set by libavcodec</para>
    /// <see cref="AVCodecContext.intra_dc_precision" />
    /// </summary>
    public int IntraDcPrecision
    {
        get => _ptr->intra_dc_precision;
        set => _ptr->intra_dc_precision = value;
    }
    
    /// <summary>
    /// <para>Number of macroblock rows at the top which are skipped. - encoding: unused - decoding: Set by user.</para>
    /// <see cref="AVCodecContext.skip_top" />
    /// </summary>
    public int SkipTop
    {
        get => _ptr->skip_top;
        set => _ptr->skip_top = value;
    }
    
    /// <summary>
    /// <para>Number of macroblock rows at the bottom which are skipped. - encoding: unused - decoding: Set by user.</para>
    /// <see cref="AVCodecContext.skip_bottom" />
    /// </summary>
    public int SkipBottom
    {
        get => _ptr->skip_bottom;
        set => _ptr->skip_bottom = value;
    }
    
    /// <summary>
    /// <para>minimum MB Lagrange multiplier - encoding: Set by user. - decoding: unused</para>
    /// <see cref="AVCodecContext.mb_lmin" />
    /// </summary>
    public int MbLmin
    {
        get => _ptr->mb_lmin;
        set => _ptr->mb_lmin = value;
    }
    
    /// <summary>
    /// <para>maximum MB Lagrange multiplier - encoding: Set by user. - decoding: unused</para>
    /// <see cref="AVCodecContext.mb_lmax" />
    /// </summary>
    public int MbLmax
    {
        get => _ptr->mb_lmax;
        set => _ptr->mb_lmax = value;
    }
    
    /// <summary>
    /// <para>- encoding: Set by user. - decoding: unused</para>
    /// <see cref="AVCodecContext.bidir_refine" />
    /// </summary>
    public int BidirRefine
    {
        get => _ptr->bidir_refine;
        set => _ptr->bidir_refine = value;
    }
    
    /// <summary>
    /// <para>minimum GOP size - encoding: Set by user. - decoding: unused</para>
    /// <see cref="AVCodecContext.keyint_min" />
    /// </summary>
    public int KeyintMin
    {
        get => _ptr->keyint_min;
        set => _ptr->keyint_min = value;
    }
    
    /// <summary>
    /// <para>number of reference frames - encoding: Set by user. - decoding: Set by lavc.</para>
    /// <see cref="AVCodecContext.refs" />
    /// </summary>
    public int Refs
    {
        get => _ptr->refs;
        set => _ptr->refs = value;
    }
    
    /// <summary>
    /// <para>Note: Value depends upon the compare function used for fullpel ME. - encoding: Set by user. - decoding: unused</para>
    /// <see cref="AVCodecContext.mv0_threshold" />
    /// </summary>
    public int Mv0Threshold
    {
        get => _ptr->mv0_threshold;
        set => _ptr->mv0_threshold = value;
    }
    
    /// <summary>
    /// <para>Chromaticity coordinates of the source primaries. - encoding: Set by user - decoding: Set by libavcodec</para>
    /// <see cref="AVCodecContext.color_primaries" />
    /// </summary>
    public AVColorPrimaries ColorPrimaries
    {
        get => _ptr->color_primaries;
        set => _ptr->color_primaries = value;
    }
    
    /// <summary>
    /// <para>Color Transfer Characteristic. - encoding: Set by user - decoding: Set by libavcodec</para>
    /// <see cref="AVCodecContext.color_trc" />
    /// </summary>
    public AVColorTransferCharacteristic ColorTrc
    {
        get => _ptr->color_trc;
        set => _ptr->color_trc = value;
    }
    
    /// <summary>
    /// <para>YUV colorspace type. - encoding: Set by user - decoding: Set by libavcodec</para>
    /// <see cref="AVCodecContext.colorspace" />
    /// </summary>
    public AVColorSpace Colorspace
    {
        get => _ptr->colorspace;
        set => _ptr->colorspace = value;
    }
    
    /// <summary>
    /// <para>MPEG vs JPEG YUV range. - encoding: Set by user - decoding: Set by libavcodec</para>
    /// <see cref="AVCodecContext.color_range" />
    /// </summary>
    public AVColorRange ColorRange
    {
        get => _ptr->color_range;
        set => _ptr->color_range = value;
    }
    
    /// <summary>
    /// <para>This defines the location of chroma samples. - encoding: Set by user - decoding: Set by libavcodec</para>
    /// <see cref="AVCodecContext.chroma_sample_location" />
    /// </summary>
    public AVChromaLocation ChromaSampleLocation
    {
        get => _ptr->chroma_sample_location;
        set => _ptr->chroma_sample_location = value;
    }
    
    /// <summary>
    /// <para>Number of slices. Indicates number of picture subdivisions. Used for parallelized decoding. - encoding: Set by user - decoding: unused</para>
    /// <see cref="AVCodecContext.slices" />
    /// </summary>
    public int Slices
    {
        get => _ptr->slices;
        set => _ptr->slices = value;
    }
    
    /// <summary>
    /// <para>Field order - encoding: set by libavcodec - decoding: Set by user.</para>
    /// <see cref="AVCodecContext.field_order" />
    /// </summary>
    public AVFieldOrder FieldOrder
    {
        get => _ptr->field_order;
        set => _ptr->field_order = value;
    }
    
    /// <summary>
    /// <para>samples per second</para>
    /// <see cref="AVCodecContext.sample_rate" />
    /// </summary>
    public int SampleRate
    {
        get => _ptr->sample_rate;
        set => _ptr->sample_rate = value;
    }
    
    /// <summary>
    /// <para>number of audio channels</para>
    /// <see cref="AVCodecContext.channels" />
    /// </summary>
    [Obsolete("use ch_layout.nb_channels")]
    public int Channels
    {
        get => _ptr->channels;
        set => _ptr->channels = value;
    }
    
    /// <summary>
    /// <para>sample format</para>
    /// <see cref="AVCodecContext.sample_fmt" />
    /// </summary>
    public AVSampleFormat SampleFormat
    {
        get => _ptr->sample_fmt;
        set => _ptr->sample_fmt = value;
    }
    
    /// <summary>
    /// <para>Number of samples per channel in an audio frame.</para>
    /// <see cref="AVCodecContext.frame_size" />
    /// </summary>
    public int FrameSize
    {
        get => _ptr->frame_size;
        set => _ptr->frame_size = value;
    }
    
    /// <summary>
    /// <para>Frame counter, set by libavcodec.</para>
    /// <see cref="AVCodecContext.frame_number" />
    /// </summary>
    public int FrameNumber
    {
        get => _ptr->frame_number;
        set => _ptr->frame_number = value;
    }
    
    /// <summary>
    /// <para>number of bytes per packet if constant and known or 0 Used by some WAV based audio codecs.</para>
    /// <see cref="AVCodecContext.block_align" />
    /// </summary>
    public int BlockAlign
    {
        get => _ptr->block_align;
        set => _ptr->block_align = value;
    }
    
    /// <summary>
    /// <para>Audio cutoff bandwidth (0 means "automatic") - encoding: Set by user. - decoding: unused</para>
    /// <see cref="AVCodecContext.cutoff" />
    /// </summary>
    public int Cutoff
    {
        get => _ptr->cutoff;
        set => _ptr->cutoff = value;
    }
    
    /// <summary>
    /// <para>Audio channel layout. - encoding: set by user. - decoding: set by user, may be overwritten by libavcodec.</para>
    /// <see cref="AVCodecContext.channel_layout" />
    /// </summary>
    [Obsolete("use ch_layout")]
    public ulong ChannelLayout
    {
        get => _ptr->channel_layout;
        set => _ptr->channel_layout = value;
    }
    
    /// <summary>
    /// <para>Request decoder to use this channel layout if it can (0 for default) - encoding: unused - decoding: Set by user.</para>
    /// <see cref="AVCodecContext.request_channel_layout" />
    /// </summary>
    [Obsolete("use \"downmix\" codec private option")]
    public ulong RequestChannelLayout
    {
        get => _ptr->request_channel_layout;
        set => _ptr->request_channel_layout = value;
    }
    
    /// <summary>
    /// <para>Type of service that the audio stream conveys. - encoding: Set by user. - decoding: Set by libavcodec.</para>
    /// <see cref="AVCodecContext.audio_service_type" />
    /// </summary>
    public AVAudioServiceType AudioServiceType
    {
        get => _ptr->audio_service_type;
        set => _ptr->audio_service_type = value;
    }
    
    /// <summary>
    /// <para>desired sample format - encoding: Not used. - decoding: Set by user. Decoder will decode to this format if it can.</para>
    /// <see cref="AVCodecContext.request_sample_fmt" />
    /// </summary>
    public AVSampleFormat RequestSampleFormat
    {
        get => _ptr->request_sample_fmt;
        set => _ptr->request_sample_fmt = value;
    }
    
    /// <summary>
    /// <para>amount of qscale change between easy &amp; hard scenes (0.0-1.0)</para>
    /// <see cref="AVCodecContext.qcompress" />
    /// </summary>
    public float Qcompress
    {
        get => _ptr->qcompress;
        set => _ptr->qcompress = value;
    }
    
    /// <summary>
    /// <para>amount of qscale smoothing over time (0.0-1.0)</para>
    /// <see cref="AVCodecContext.qblur" />
    /// </summary>
    public float Qblur
    {
        get => _ptr->qblur;
        set => _ptr->qblur = value;
    }
    
    /// <summary>
    /// <para>minimum quantizer - encoding: Set by user. - decoding: unused</para>
    /// <see cref="AVCodecContext.qmin" />
    /// </summary>
    public int Qmin
    {
        get => _ptr->qmin;
        set => _ptr->qmin = value;
    }
    
    /// <summary>
    /// <para>maximum quantizer - encoding: Set by user. - decoding: unused</para>
    /// <see cref="AVCodecContext.qmax" />
    /// </summary>
    public int Qmax
    {
        get => _ptr->qmax;
        set => _ptr->qmax = value;
    }
    
    /// <summary>
    /// <para>maximum quantizer difference between frames - encoding: Set by user. - decoding: unused</para>
    /// <see cref="AVCodecContext.max_qdiff" />
    /// </summary>
    public int MaxQdiff
    {
        get => _ptr->max_qdiff;
        set => _ptr->max_qdiff = value;
    }
    
    /// <summary>
    /// <para>decoder bitstream buffer size - encoding: Set by user. - decoding: unused</para>
    /// <see cref="AVCodecContext.rc_buffer_size" />
    /// </summary>
    public int RcBufferSize
    {
        get => _ptr->rc_buffer_size;
        set => _ptr->rc_buffer_size = value;
    }
    
    /// <summary>
    /// <para>ratecontrol override, see RcOverride - encoding: Allocated/set/freed by user. - decoding: unused</para>
    /// <see cref="AVCodecContext.rc_override_count" />
    /// </summary>
    public int RcOverrideCount
    {
        get => _ptr->rc_override_count;
        set => _ptr->rc_override_count = value;
    }
    
    /// <summary>
    /// <see cref="AVCodecContext.rc_override" />
    /// </summary>
    public RcOverride* RcOverride
    {
        get => _ptr->rc_override;
        set => _ptr->rc_override = value;
    }
    
    /// <summary>
    /// <para>maximum bitrate - encoding: Set by user. - decoding: Set by user, may be overwritten by libavcodec.</para>
    /// <see cref="AVCodecContext.rc_max_rate" />
    /// </summary>
    public long RcMaxRate
    {
        get => _ptr->rc_max_rate;
        set => _ptr->rc_max_rate = value;
    }
    
    /// <summary>
    /// <para>minimum bitrate - encoding: Set by user. - decoding: unused</para>
    /// <see cref="AVCodecContext.rc_min_rate" />
    /// </summary>
    public long RcMinRate
    {
        get => _ptr->rc_min_rate;
        set => _ptr->rc_min_rate = value;
    }
    
    /// <summary>
    /// <para>Ratecontrol attempt to use, at maximum, &lt;value&gt; of what can be used without an underflow. - encoding: Set by user. - decoding: unused.</para>
    /// <see cref="AVCodecContext.rc_max_available_vbv_use" />
    /// </summary>
    public float RcMaxAvailableVbvUse
    {
        get => _ptr->rc_max_available_vbv_use;
        set => _ptr->rc_max_available_vbv_use = value;
    }
    
    /// <summary>
    /// <para>Ratecontrol attempt to use, at least, &lt;value&gt; times the amount needed to prevent a vbv overflow. - encoding: Set by user. - decoding: unused.</para>
    /// <see cref="AVCodecContext.rc_min_vbv_overflow_use" />
    /// </summary>
    public float RcMinVbvOverflowUse
    {
        get => _ptr->rc_min_vbv_overflow_use;
        set => _ptr->rc_min_vbv_overflow_use = value;
    }
    
    /// <summary>
    /// <para>Number of bits which should be loaded into the rc buffer before decoding starts. - encoding: Set by user. - decoding: unused</para>
    /// <see cref="AVCodecContext.rc_initial_buffer_occupancy" />
    /// </summary>
    public int RcInitialBufferOccupancy
    {
        get => _ptr->rc_initial_buffer_occupancy;
        set => _ptr->rc_initial_buffer_occupancy = value;
    }
    
    /// <summary>
    /// <para>trellis RD quantization - encoding: Set by user. - decoding: unused</para>
    /// <see cref="AVCodecContext.trellis" />
    /// </summary>
    public int Trellis
    {
        get => _ptr->trellis;
        set => _ptr->trellis = value;
    }
    
    /// <summary>
    /// <para>original type: byte*</para>
    /// <para>pass1 encoding statistics output buffer - encoding: Set by libavcodec. - decoding: unused</para>
    /// <see cref="AVCodecContext.stats_out" />
    /// </summary>
    public IntPtr StatsOut
    {
        get => (IntPtr)_ptr->stats_out;
        set => _ptr->stats_out = (byte*)value;
    }
    
    /// <summary>
    /// <para>original type: byte*</para>
    /// <para>pass2 encoding statistics input buffer Concatenated stuff from stats_out of pass1 should be placed here. - encoding: Allocated/set/freed by user. - decoding: unused</para>
    /// <see cref="AVCodecContext.stats_in" />
    /// </summary>
    public IntPtr StatsIn
    {
        get => (IntPtr)_ptr->stats_in;
        set => _ptr->stats_in = (byte*)value;
    }
    
    /// <summary>
    /// <para>Work around bugs in encoders which sometimes cannot be detected automatically. - encoding: Set by user - decoding: Set by user</para>
    /// <see cref="AVCodecContext.workaround_bugs" />
    /// </summary>
    public int WorkaroundBugs
    {
        get => _ptr->workaround_bugs;
        set => _ptr->workaround_bugs = value;
    }
    
    /// <summary>
    /// <para>strictly follow the standard (MPEG-4, ...). - encoding: Set by user. - decoding: Set by user. Setting this to STRICT or higher means the encoder and decoder will generally do stupid things, whereas setting it to unofficial or lower will mean the encoder might produce output that is not supported by all spec-compliant decoders. Decoders don't differentiate between normal, unofficial and experimental (that is, they always try to decode things when they can) unless they are explicitly asked to behave stupidly (=strictly conform to the specs)</para>
    /// <see cref="AVCodecContext.strict_std_compliance" />
    /// </summary>
    public int StrictStdCompliance
    {
        get => _ptr->strict_std_compliance;
        set => _ptr->strict_std_compliance = value;
    }
    
    /// <summary>
    /// <para>error concealment flags - encoding: unused - decoding: Set by user.</para>
    /// <see cref="AVCodecContext.error_concealment" />
    /// </summary>
    public int ErrorConcealment
    {
        get => _ptr->error_concealment;
        set => _ptr->error_concealment = value;
    }
    
    /// <summary>
    /// <para>debug - encoding: Set by user. - decoding: Set by user.</para>
    /// <see cref="AVCodecContext.debug" />
    /// </summary>
    public int Debug
    {
        get => _ptr->debug;
        set => _ptr->debug = value;
    }
    
    /// <summary>
    /// <para>Error recognition; may misdetect some more or less valid parts as errors. - encoding: Set by user. - decoding: Set by user.</para>
    /// <see cref="AVCodecContext.err_recognition" />
    /// </summary>
    public int ErrRecognition
    {
        get => _ptr->err_recognition;
        set => _ptr->err_recognition = value;
    }
    
    /// <summary>
    /// <para>opaque 64-bit number (generally a PTS) that will be reordered and output in AVFrame.reordered_opaque - encoding: Set by libavcodec to the reordered_opaque of the input frame corresponding to the last returned packet. Only supported by encoders with the AV_CODEC_CAP_ENCODER_REORDERED_OPAQUE capability. - decoding: Set by user.</para>
    /// <see cref="AVCodecContext.reordered_opaque" />
    /// </summary>
    public long ReorderedOpaque
    {
        get => _ptr->reordered_opaque;
        set => _ptr->reordered_opaque = value;
    }
    
    /// <summary>
    /// <para>Hardware accelerator in use - encoding: unused. - decoding: Set by libavcodec</para>
    /// <see cref="AVCodecContext.hwaccel" />
    /// </summary>
    public AVHWAccel* Hwaccel
    {
        get => _ptr->hwaccel;
        set => _ptr->hwaccel = value;
    }
    
    /// <summary>
    /// <para>original type: void*</para>
    /// <para>Hardware accelerator context. For some hardware accelerators, a global context needs to be provided by the user. In that case, this holds display-dependent data FFmpeg cannot instantiate itself. Please refer to the FFmpeg HW accelerator documentation to know how to fill this. - encoding: unused - decoding: Set by user</para>
    /// <see cref="AVCodecContext.hwaccel_context" />
    /// </summary>
    public IntPtr HwaccelContext
    {
        get => (IntPtr)_ptr->hwaccel_context;
        set => _ptr->hwaccel_context = (void*)value;
    }
    
    /// <summary>
    /// <para>error - encoding: Set by libavcodec if flags &amp; AV_CODEC_FLAG_PSNR. - decoding: unused</para>
    /// <see cref="AVCodecContext.error" />
    /// </summary>
    public ref ulong_array8 Error
    {
        get => ref _ptr->error;
    }
    
    /// <summary>
    /// <para>DCT algorithm, see FF_DCT_* below - encoding: Set by user. - decoding: unused</para>
    /// <see cref="AVCodecContext.dct_algo" />
    /// </summary>
    public int DctAlgo
    {
        get => _ptr->dct_algo;
        set => _ptr->dct_algo = value;
    }
    
    /// <summary>
    /// <para>IDCT algorithm, see FF_IDCT_* below. - encoding: Set by user. - decoding: Set by user.</para>
    /// <see cref="AVCodecContext.idct_algo" />
    /// </summary>
    public int IdctAlgo
    {
        get => _ptr->idct_algo;
        set => _ptr->idct_algo = value;
    }
    
    /// <summary>
    /// <para>bits per sample/pixel from the demuxer (needed for huffyuv). - encoding: Set by libavcodec. - decoding: Set by user.</para>
    /// <see cref="AVCodecContext.bits_per_coded_sample" />
    /// </summary>
    public int BitsPerCodedSample
    {
        get => _ptr->bits_per_coded_sample;
        set => _ptr->bits_per_coded_sample = value;
    }
    
    /// <summary>
    /// <para>Bits per sample/pixel of internal libavcodec pixel/sample format. - encoding: set by user. - decoding: set by libavcodec.</para>
    /// <see cref="AVCodecContext.bits_per_raw_sample" />
    /// </summary>
    public int BitsPerRawSample
    {
        get => _ptr->bits_per_raw_sample;
        set => _ptr->bits_per_raw_sample = value;
    }
    
    /// <summary>
    /// <para>low resolution decoding, 1-&gt; 1/2 size, 2-&gt;1/4 size - encoding: unused - decoding: Set by user.</para>
    /// <see cref="AVCodecContext.lowres" />
    /// </summary>
    public int Lowres
    {
        get => _ptr->lowres;
        set => _ptr->lowres = value;
    }
    
    /// <summary>
    /// <para>thread count is used to decide how many independent tasks should be passed to execute() - encoding: Set by user. - decoding: Set by user.</para>
    /// <see cref="AVCodecContext.thread_count" />
    /// </summary>
    public int ThreadCount
    {
        get => _ptr->thread_count;
        set => _ptr->thread_count = value;
    }
    
    /// <summary>
    /// <para>Which multithreading methods to use. Use of FF_THREAD_FRAME will increase decoding delay by one frame per thread, so clients which cannot provide future frames should not use it.</para>
    /// <see cref="AVCodecContext.thread_type" />
    /// </summary>
    public int ThreadType
    {
        get => _ptr->thread_type;
        set => _ptr->thread_type = value;
    }
    
    /// <summary>
    /// <para>Which multithreading methods are in use by the codec. - encoding: Set by libavcodec. - decoding: Set by libavcodec.</para>
    /// <see cref="AVCodecContext.active_thread_type" />
    /// </summary>
    public int ActiveThreadType
    {
        get => _ptr->active_thread_type;
        set => _ptr->active_thread_type = value;
    }
    
    /// <summary>
    /// <para>Set by the client if its custom get_buffer() callback can be called synchronously from another thread, which allows faster multithreaded decoding. draw_horiz_band() will be called from other threads regardless of this setting. Ignored if the default get_buffer() is used. - encoding: Set by user. - decoding: Set by user.</para>
    /// <see cref="AVCodecContext.thread_safe_callbacks" />
    /// </summary>
    [Obsolete("the custom get_buffer2() callback should always be thread-safe. Thread-unsafe get_buffer2() implementations will be invalid starting with LIBAVCODEC_VERSION_MAJOR=60; in other words, libavcodec will behave as if this field was always set to 1. Callers that want to be forward compatible with future libavcodec versions should wrap access to this field in #if LIBAVCODEC_VERSION_MAJOR < 60")]
    public int ThreadSafeCallbacks
    {
        get => _ptr->thread_safe_callbacks;
        set => _ptr->thread_safe_callbacks = value;
    }
    
    /// <summary>
    /// <para>noise vs. sse weight for the nsse comparison function - encoding: Set by user. - decoding: unused</para>
    /// <see cref="AVCodecContext.nsse_weight" />
    /// </summary>
    public int NsseWeight
    {
        get => _ptr->nsse_weight;
        set => _ptr->nsse_weight = value;
    }
    
    /// <summary>
    /// <para>profile - encoding: Set by user. - decoding: Set by libavcodec.</para>
    /// <see cref="AVCodecContext.profile" />
    /// </summary>
    public int Profile
    {
        get => _ptr->profile;
        set => _ptr->profile = value;
    }
    
    /// <summary>
    /// <para>level - encoding: Set by user. - decoding: Set by libavcodec.</para>
    /// <see cref="AVCodecContext.level" />
    /// </summary>
    public int Level
    {
        get => _ptr->level;
        set => _ptr->level = value;
    }
    
    /// <summary>
    /// <para>Skip loop filtering for selected frames. - encoding: unused - decoding: Set by user.</para>
    /// <see cref="AVCodecContext.skip_loop_filter" />
    /// </summary>
    public AVDiscard SkipLoopFilter
    {
        get => _ptr->skip_loop_filter;
        set => _ptr->skip_loop_filter = value;
    }
    
    /// <summary>
    /// <para>Skip IDCT/dequantization for selected frames. - encoding: unused - decoding: Set by user.</para>
    /// <see cref="AVCodecContext.skip_idct" />
    /// </summary>
    public AVDiscard SkipIdct
    {
        get => _ptr->skip_idct;
        set => _ptr->skip_idct = value;
    }
    
    /// <summary>
    /// <para>Skip decoding for selected frames. - encoding: unused - decoding: Set by user.</para>
    /// <see cref="AVCodecContext.skip_frame" />
    /// </summary>
    public AVDiscard SkipFrame
    {
        get => _ptr->skip_frame;
        set => _ptr->skip_frame = value;
    }
    
    /// <summary>
    /// <para>original type: byte*</para>
    /// <para>Header containing style information for text subtitles. For SUBTITLE_ASS subtitle type, it should contain the whole ASS [Script Info] and [V4+ Styles] section, plus the [Events] line and the Format line following. It shouldn't include any Dialogue line. - encoding: Set/allocated/freed by user (before avcodec_open2()) - decoding: Set/allocated/freed by libavcodec (by avcodec_open2())</para>
    /// <see cref="AVCodecContext.subtitle_header" />
    /// </summary>
    public IntPtr SubtitleHeader
    {
        get => (IntPtr)_ptr->subtitle_header;
        set => _ptr->subtitle_header = (byte*)value;
    }
    
    /// <summary>
    /// <see cref="AVCodecContext.subtitle_header_size" />
    /// </summary>
    public int SubtitleHeaderSize
    {
        get => _ptr->subtitle_header_size;
        set => _ptr->subtitle_header_size = value;
    }
    
    /// <summary>
    /// <para>Audio only. The number of "priming" samples (padding) inserted by the encoder at the beginning of the audio. I.e. this number of leading decoded samples must be discarded by the caller to get the original audio without leading padding.</para>
    /// <see cref="AVCodecContext.initial_padding" />
    /// </summary>
    public int InitialPadding
    {
        get => _ptr->initial_padding;
        set => _ptr->initial_padding = value;
    }
    
    /// <summary>
    /// <para>- decoding: For codecs that store a framerate value in the compressed bitstream, the decoder may export it here. { 0, 1} when unknown. - encoding: May be used to signal the framerate of CFR content to an encoder.</para>
    /// <see cref="AVCodecContext.framerate" />
    /// </summary>
    public AVRational Framerate
    {
        get => _ptr->framerate;
        set => _ptr->framerate = value;
    }
    
    /// <summary>
    /// <para>Nominal unaccelerated pixel format, see AV_PIX_FMT_xxx. - encoding: unused. - decoding: Set by libavcodec before calling get_format()</para>
    /// <see cref="AVCodecContext.sw_pix_fmt" />
    /// </summary>
    public AVPixelFormat SwPixelFormat
    {
        get => _ptr->sw_pix_fmt;
        set => _ptr->sw_pix_fmt = value;
    }
    
    /// <summary>
    /// <para>Timebase in which pkt_dts/pts and AVPacket.dts/pts are. - encoding unused. - decoding set by user.</para>
    /// <see cref="AVCodecContext.pkt_timebase" />
    /// </summary>
    public AVRational PktTimebase
    {
        get => _ptr->pkt_timebase;
        set => _ptr->pkt_timebase = value;
    }
    
    /// <summary>
    /// <para>AVCodecDescriptor - encoding: unused. - decoding: set by libavcodec.</para>
    /// <see cref="AVCodecContext.codec_descriptor" />
    /// </summary>
    public AVCodecDescriptor* CodecDescriptor
    {
        get => _ptr->codec_descriptor;
        set => _ptr->codec_descriptor = value;
    }
    
    /// <summary>
    /// <para>Current statistics for PTS correction. - decoding: maintained and used by libavcodec, not intended to be used by user apps - encoding: unused</para>
    /// <see cref="AVCodecContext.pts_correction_num_faulty_pts" />
    /// </summary>
    public long PtsCorrectionNumFaultyPts
    {
        get => _ptr->pts_correction_num_faulty_pts;
        set => _ptr->pts_correction_num_faulty_pts = value;
    }
    
    /// <summary>
    /// <para>Number of incorrect PTS values so far</para>
    /// <see cref="AVCodecContext.pts_correction_num_faulty_dts" />
    /// </summary>
    public long PtsCorrectionNumFaultyDts
    {
        get => _ptr->pts_correction_num_faulty_dts;
        set => _ptr->pts_correction_num_faulty_dts = value;
    }
    
    /// <summary>
    /// <para>Number of incorrect DTS values so far</para>
    /// <see cref="AVCodecContext.pts_correction_last_pts" />
    /// </summary>
    public long PtsCorrectionLastPts
    {
        get => _ptr->pts_correction_last_pts;
        set => _ptr->pts_correction_last_pts = value;
    }
    
    /// <summary>
    /// <para>PTS of the last frame</para>
    /// <see cref="AVCodecContext.pts_correction_last_dts" />
    /// </summary>
    public long PtsCorrectionLastDts
    {
        get => _ptr->pts_correction_last_dts;
        set => _ptr->pts_correction_last_dts = value;
    }
    
    /// <summary>
    /// <para>original type: byte*</para>
    /// <para>Character encoding of the input subtitles file. - decoding: set by user - encoding: unused</para>
    /// <see cref="AVCodecContext.sub_charenc" />
    /// </summary>
    public IntPtr SubCharenc
    {
        get => (IntPtr)_ptr->sub_charenc;
        set => _ptr->sub_charenc = (byte*)value;
    }
    
    /// <summary>
    /// <para>Subtitles character encoding mode. Formats or codecs might be adjusting this setting (if they are doing the conversion themselves for instance). - decoding: set by libavcodec - encoding: unused</para>
    /// <see cref="AVCodecContext.sub_charenc_mode" />
    /// </summary>
    public int SubCharencMode
    {
        get => _ptr->sub_charenc_mode;
        set => _ptr->sub_charenc_mode = value;
    }
    
    /// <summary>
    /// <para>Skip processing alpha if supported by codec. Note that if the format uses pre-multiplied alpha (common with VP6, and recommended due to better video quality/compression) the image will look as if alpha-blended onto a black background. However for formats that do not use pre-multiplied alpha there might be serious artefacts (though e.g. libswscale currently assumes pre-multiplied alpha anyway).</para>
    /// <see cref="AVCodecContext.skip_alpha" />
    /// </summary>
    public int SkipAlpha
    {
        get => _ptr->skip_alpha;
        set => _ptr->skip_alpha = value;
    }
    
    /// <summary>
    /// <para>Number of samples to skip after a discontinuity - decoding: unused - encoding: set by libavcodec</para>
    /// <see cref="AVCodecContext.seek_preroll" />
    /// </summary>
    public int SeekPreroll
    {
        get => _ptr->seek_preroll;
        set => _ptr->seek_preroll = value;
    }
    
    /// <summary>
    /// <see cref="AVCodecContext.debug_mv" />
    /// </summary>
    [Obsolete("unused")]
    public int DebugMv
    {
        get => _ptr->debug_mv;
        set => _ptr->debug_mv = value;
    }
    
    /// <summary>
    /// <para>custom intra quantization matrix - encoding: Set by user, can be NULL. - decoding: unused.</para>
    /// <see cref="AVCodecContext.chroma_intra_matrix" />
    /// </summary>
    public ushort* ChromaIntraMatrix
    {
        get => _ptr->chroma_intra_matrix;
        set => _ptr->chroma_intra_matrix = value;
    }
    
    /// <summary>
    /// <para>original type: byte*</para>
    /// <para>dump format separator. can be ", " or " " or anything else - encoding: Set by user. - decoding: Set by user.</para>
    /// <see cref="AVCodecContext.dump_separator" />
    /// </summary>
    public IntPtr DumpSeparator
    {
        get => (IntPtr)_ptr->dump_separator;
        set => _ptr->dump_separator = (byte*)value;
    }
    
    /// <summary>
    /// <para>original type: byte*</para>
    /// <para>',' separated list of allowed decoders. If NULL then all are allowed - encoding: unused - decoding: set by user</para>
    /// <see cref="AVCodecContext.codec_whitelist" />
    /// </summary>
    public IntPtr CodecWhitelist
    {
        get => (IntPtr)_ptr->codec_whitelist;
        set => _ptr->codec_whitelist = (byte*)value;
    }
    
    /// <summary>
    /// <para>Properties of the stream that gets decoded - encoding: unused - decoding: set by libavcodec</para>
    /// <see cref="AVCodecContext.properties" />
    /// </summary>
    public uint Properties
    {
        get => _ptr->properties;
        set => _ptr->properties = value;
    }
    
    /// <summary>
    /// <para>original type: AVPacketSideData*</para>
    /// <para>Additional data associated with the entire coded stream.</para>
    /// <see cref="AVCodecContext.coded_side_data" />
    /// </summary>
    public PacketSideData? CodedSideData
    {
        get => PacketSideData.FromNativeOrNull(_ptr->coded_side_data);
        set => _ptr->coded_side_data = (AVPacketSideData*)value;
    }
    
    /// <summary>
    /// <see cref="AVCodecContext.nb_coded_side_data" />
    /// </summary>
    public int NbCodedSideData
    {
        get => _ptr->nb_coded_side_data;
        set => _ptr->nb_coded_side_data = value;
    }
    
    /// <summary>
    /// <para>original type: AVBufferRef*</para>
    /// <para>A reference to the AVHWFramesContext describing the input (for encoding) or output (decoding) frames. The reference is set by the caller and afterwards owned (and freed) by libavcodec - it should never be read by the caller after being set.</para>
    /// <see cref="AVCodecContext.hw_frames_ctx" />
    /// </summary>
    public BufferRef? HwFramesContext
    {
        get => BufferRef.FromNativeOrNull(_ptr->hw_frames_ctx, false);
        set => _ptr->hw_frames_ctx = value != null ? (AVBufferRef*)value : null;
    }
    
    /// <summary>
    /// <see cref="AVCodecContext.sub_text_format" />
    /// </summary>
    [Obsolete("unused")]
    public int SubTextFormat
    {
        get => _ptr->sub_text_format;
        set => _ptr->sub_text_format = value;
    }
    
    /// <summary>
    /// <para>Audio only. The amount of padding (in samples) appended by the encoder to the end of the audio. I.e. this number of decoded samples must be discarded by the caller from the end of the stream to get the original audio without any trailing padding.</para>
    /// <see cref="AVCodecContext.trailing_padding" />
    /// </summary>
    public int TrailingPadding
    {
        get => _ptr->trailing_padding;
        set => _ptr->trailing_padding = value;
    }
    
    /// <summary>
    /// <para>The number of pixels per image to maximally accept.</para>
    /// <see cref="AVCodecContext.max_pixels" />
    /// </summary>
    public long MaxPixels
    {
        get => _ptr->max_pixels;
        set => _ptr->max_pixels = value;
    }
    
    /// <summary>
    /// <para>original type: AVBufferRef*</para>
    /// <para>A reference to the AVHWDeviceContext describing the device which will be used by a hardware encoder/decoder. The reference is set by the caller and afterwards owned (and freed) by libavcodec.</para>
    /// <see cref="AVCodecContext.hw_device_ctx" />
    /// </summary>
    public BufferRef? HwDeviceContext
    {
        get => BufferRef.FromNativeOrNull(_ptr->hw_device_ctx, false);
        set => _ptr->hw_device_ctx = value != null ? (AVBufferRef*)value : null;
    }
    
    /// <summary>
    /// <para>Bit set of AV_HWACCEL_FLAG_* flags, which affect hardware accelerated decoding (if active). - encoding: unused - decoding: Set by user (either before avcodec_open2(), or in the AVCodecContext.get_format callback)</para>
    /// <see cref="AVCodecContext.hwaccel_flags" />
    /// </summary>
    public int HwaccelFlags
    {
        get => _ptr->hwaccel_flags;
        set => _ptr->hwaccel_flags = value;
    }
    
    /// <summary>
    /// <para>Video decoding only. Certain video codecs support cropping, meaning that only a sub-rectangle of the decoded frame is intended for display. This option controls how cropping is handled by libavcodec.</para>
    /// <see cref="AVCodecContext.apply_cropping" />
    /// </summary>
    public int ApplyCropping
    {
        get => _ptr->apply_cropping;
        set => _ptr->apply_cropping = value;
    }
    
    /// <summary>
    /// <see cref="AVCodecContext.extra_hw_frames" />
    /// </summary>
    public int ExtraHwFrames
    {
        get => _ptr->extra_hw_frames;
        set => _ptr->extra_hw_frames = value;
    }
    
    /// <summary>
    /// <para>The percentage of damaged samples to discard a frame.</para>
    /// <see cref="AVCodecContext.discard_damaged_percentage" />
    /// </summary>
    public int DiscardDamagedPercentage
    {
        get => _ptr->discard_damaged_percentage;
        set => _ptr->discard_damaged_percentage = value;
    }
    
    /// <summary>
    /// <para>The number of samples per frame to maximally accept.</para>
    /// <see cref="AVCodecContext.max_samples" />
    /// </summary>
    public long MaxSamples
    {
        get => _ptr->max_samples;
        set => _ptr->max_samples = value;
    }
    
    /// <summary>
    /// <para>Bit set of AV_CODEC_EXPORT_DATA_* flags, which affects the kind of metadata exported in frame, packet, or coded stream side data by decoders and encoders.</para>
    /// <see cref="AVCodecContext.export_side_data" />
    /// </summary>
    public int ExportSideData
    {
        get => _ptr->export_side_data;
        set => _ptr->export_side_data = value;
    }
    
    /// <summary>
    /// <para>Audio channel layout. - encoding: must be set by the caller, to one of AVCodec.ch_layouts. - decoding: may be set by the caller if known e.g. from the container. The decoder can then override during decoding as needed.</para>
    /// <see cref="AVCodecContext.ch_layout" />
    /// </summary>
    public AVChannelLayout ChLayout
    {
        get => _ptr->ch_layout;
        set => _ptr->ch_layout = value;
    }
}
