// This file was genereated from Sdcb.FFmpeg.AutoGen, DO NOT CHANGE DIRECTLY.
#nullable enable
using Sdcb.FFmpeg.Common;
using Sdcb.FFmpeg.Codecs;
using Sdcb.FFmpeg.Formats;
using Sdcb.FFmpeg.Filters;
using Sdcb.FFmpeg.Raw;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Sdcb.FFmpeg.Utils;

/// <summary>
/// <para>This structure describes decoded (raw) audio or video data.</para>
/// <see cref="AVFrame" />
/// </summary>
public unsafe partial class Frame : SafeHandle
{
    protected AVFrame* _ptr => (AVFrame*)handle;
    
    public static implicit operator AVFrame*(Frame data) => data != null ? (AVFrame*)data.handle : null;
    
    protected Frame(AVFrame* ptr, bool isOwner): base(NativeUtils.NotNull((IntPtr)ptr), isOwner)
    {
    }
    
    public static Frame FromNative(AVFrame* ptr, bool isOwner) => new Frame(ptr, isOwner);
    
    internal static Frame FromNative(IntPtr ptr, bool isOwner) => new Frame((AVFrame*)ptr, isOwner);
    
    public static Frame? FromNativeOrNull(AVFrame* ptr, bool isOwner) => ptr == null ? null : new Frame(ptr, isOwner);
    
    public override bool IsInvalid => handle == IntPtr.Zero;
    
    /// <summary>
    /// <para>pointer to the picture/channel planes. This might be different from the first allocated byte. For video, it could even point to the end of the image data.</para>
    /// <see cref="AVFrame.data" />
    /// </summary>
    public ref byte_ptrArray8 Data
    {
        get => ref _ptr->data;
    }
    
    /// <summary>
    /// <para>For video, a positive or negative value, which is typically indicating the size in bytes of each picture line, but it can also be: - the negative byte size of lines for vertical flipping (with data[n] pointing to the end of the data - a positive or negative multiple of the byte size as for accessing even and odd fields of a frame (possibly flipped)</para>
    /// <see cref="AVFrame.linesize" />
    /// </summary>
    public ref int_array8 Linesize
    {
        get => ref _ptr->linesize;
    }
    
    /// <summary>
    /// <para>pointers to the data planes/channels.</para>
    /// <see cref="AVFrame.extended_data" />
    /// </summary>
    public byte** ExtendedData
    {
        get => _ptr->extended_data;
        set => _ptr->extended_data = value;
    }
    
    /// <summary>
    /// <para>Video frames only. The coded dimensions (in pixels) of the video frame, i.e. the size of the rectangle that contains some well-defined values.</para>
    /// <see cref="AVFrame.width" />
    /// </summary>
    public int Width
    {
        get => _ptr->width;
        set => _ptr->width = value;
    }
    
    /// <summary>
    /// <para>Video frames only. The coded dimensions (in pixels) of the video frame, i.e. the size of the rectangle that contains some well-defined values.</para>
    /// <see cref="AVFrame.height" />
    /// </summary>
    public int Height
    {
        get => _ptr->height;
        set => _ptr->height = value;
    }
    
    /// <summary>
    /// <para>number of audio samples (per channel) described by this frame</para>
    /// <see cref="AVFrame.nb_samples" />
    /// </summary>
    public int NbSamples
    {
        get => _ptr->nb_samples;
        set => _ptr->nb_samples = value;
    }
    
    /// <summary>
    /// <para>format of the frame, -1 if unknown or unset Values correspond to enum AVPixelFormat for video frames, enum AVSampleFormat for audio)</para>
    /// <see cref="AVFrame.format" />
    /// </summary>
    public int Format
    {
        get => _ptr->format;
        set => _ptr->format = value;
    }
    
    /// <summary>
    /// <para>1 -&gt; keyframe, 0-&gt; not</para>
    /// <see cref="AVFrame.key_frame" />
    /// </summary>
    public int KeyFrame
    {
        get => _ptr->key_frame;
        set => _ptr->key_frame = value;
    }
    
    /// <summary>
    /// <para>Picture type of the frame.</para>
    /// <see cref="AVFrame.pict_type" />
    /// </summary>
    public AVPictureType PictType
    {
        get => _ptr->pict_type;
        set => _ptr->pict_type = value;
    }
    
    /// <summary>
    /// <para>Sample aspect ratio for the video frame, 0/1 if unknown/unspecified.</para>
    /// <see cref="AVFrame.sample_aspect_ratio" />
    /// </summary>
    public AVRational SampleAspectRatio
    {
        get => _ptr->sample_aspect_ratio;
        set => _ptr->sample_aspect_ratio = value;
    }
    
    /// <summary>
    /// <para>Presentation timestamp in time_base units (time when frame should be shown to user).</para>
    /// <see cref="AVFrame.pts" />
    /// </summary>
    public long Pts
    {
        get => _ptr->pts;
        set => _ptr->pts = value;
    }
    
    /// <summary>
    /// <para>DTS copied from the AVPacket that triggered returning this frame. (if frame threading isn't used) This is also the Presentation time of this AVFrame calculated from only AVPacket.dts values without pts values.</para>
    /// <see cref="AVFrame.pkt_dts" />
    /// </summary>
    public long PktDts
    {
        get => _ptr->pkt_dts;
        set => _ptr->pkt_dts = value;
    }
    
    /// <summary>
    /// <para>Time base for the timestamps in this frame. In the future, this field may be set on frames output by decoders or filters, but its value will be by default ignored on input to encoders or filters.</para>
    /// <see cref="AVFrame.time_base" />
    /// </summary>
    public AVRational TimeBase
    {
        get => _ptr->time_base;
        set => _ptr->time_base = value;
    }
    
    /// <summary>
    /// <para>picture number in bitstream order</para>
    /// <see cref="AVFrame.coded_picture_number" />
    /// </summary>
    public int CodedPictureNumber
    {
        get => _ptr->coded_picture_number;
        set => _ptr->coded_picture_number = value;
    }
    
    /// <summary>
    /// <para>picture number in display order</para>
    /// <see cref="AVFrame.display_picture_number" />
    /// </summary>
    public int DisplayPictureNumber
    {
        get => _ptr->display_picture_number;
        set => _ptr->display_picture_number = value;
    }
    
    /// <summary>
    /// <para>quality (between 1 (good) and FF_LAMBDA_MAX (bad))</para>
    /// <see cref="AVFrame.quality" />
    /// </summary>
    public int Quality
    {
        get => _ptr->quality;
        set => _ptr->quality = value;
    }
    
    /// <summary>
    /// <para>original type: void*</para>
    /// <para>for some private data of the user</para>
    /// <see cref="AVFrame.opaque" />
    /// </summary>
    public IntPtr Opaque
    {
        get => (IntPtr)_ptr->opaque;
        set => _ptr->opaque = (void*)value;
    }
    
    /// <summary>
    /// <para>When decoding, this signals how much the picture must be delayed. extra_delay = repeat_pict / (2*fps)</para>
    /// <see cref="AVFrame.repeat_pict" />
    /// </summary>
    public int RepeatPict
    {
        get => _ptr->repeat_pict;
        set => _ptr->repeat_pict = value;
    }
    
    /// <summary>
    /// <para>The content of the picture is interlaced.</para>
    /// <see cref="AVFrame.interlaced_frame" />
    /// </summary>
    public int InterlacedFrame
    {
        get => _ptr->interlaced_frame;
        set => _ptr->interlaced_frame = value;
    }
    
    /// <summary>
    /// <para>If the content is interlaced, is top field displayed first.</para>
    /// <see cref="AVFrame.top_field_first" />
    /// </summary>
    public int TopFieldFirst
    {
        get => _ptr->top_field_first;
        set => _ptr->top_field_first = value;
    }
    
    /// <summary>
    /// <para>Tell user application that palette has changed from previous frame.</para>
    /// <see cref="AVFrame.palette_has_changed" />
    /// </summary>
    public int PaletteHasChanged
    {
        get => _ptr->palette_has_changed;
        set => _ptr->palette_has_changed = value;
    }
    
    /// <summary>
    /// <para>reordered opaque 64 bits (generally an integer or a double precision float PTS but can be anything). The user sets AVCodecContext.reordered_opaque to represent the input at that time, the decoder reorders values as needed and sets AVFrame.reordered_opaque to exactly one of the values provided by the user through AVCodecContext.reordered_opaque</para>
    /// <see cref="AVFrame.reordered_opaque" />
    /// </summary>
    [Obsolete("Use AV_CODEC_FLAG_COPY_OPAQUE instead")]
    public long ReorderedOpaque
    {
        get => _ptr->reordered_opaque;
        set => _ptr->reordered_opaque = value;
    }
    
    /// <summary>
    /// <para>Sample rate of the audio data.</para>
    /// <see cref="AVFrame.sample_rate" />
    /// </summary>
    public int SampleRate
    {
        get => _ptr->sample_rate;
        set => _ptr->sample_rate = value;
    }
    
    /// <summary>
    /// <para>Channel layout of the audio data.</para>
    /// <see cref="AVFrame.channel_layout" />
    /// </summary>
    [Obsolete("use ch_layout instead")]
    public ulong ChannelLayout
    {
        get => _ptr->channel_layout;
        set => _ptr->channel_layout = value;
    }
    
    /// <summary>
    /// <para>AVBuffer references backing the data for this frame. All the pointers in data and extended_data must point inside one of the buffers in buf or extended_buf. This array must be filled contiguously -- if buf[i] is non-NULL then buf[j] must also be non-NULL for all j &lt; i.</para>
    /// <see cref="AVFrame.buf" />
    /// </summary>
    public ref AVBufferRef_ptrArray8 Buf
    {
        get => ref _ptr->buf;
    }
    
    /// <summary>
    /// <para>For planar audio which requires more than AV_NUM_DATA_POINTERS AVBufferRef pointers, this array will hold all the references which cannot fit into AVFrame.buf.</para>
    /// <see cref="AVFrame.extended_buf" />
    /// </summary>
    public AVBufferRef** ExtendedBuf
    {
        get => _ptr->extended_buf;
        set => _ptr->extended_buf = value;
    }
    
    /// <summary>
    /// <para>Number of elements in extended_buf.</para>
    /// <see cref="AVFrame.nb_extended_buf" />
    /// </summary>
    public int NbExtendedBuf
    {
        get => _ptr->nb_extended_buf;
        set => _ptr->nb_extended_buf = value;
    }
    
    /// <summary>
    /// <para>original type: AVFrameSideData**</para>
    /// <see cref="AVFrame.side_data" />
    /// </summary>
    public IReadOnlyList<FrameSideData> SideData => new ReadOnlyPtrList<AVFrameSideData, FrameSideData>(_ptr->side_data, (int)_ptr->nb_side_data, FrameSideData.FromNative)!;
    
    /// <summary>
    /// <para>Frame flags, a combination of lavu_frame_flags</para>
    /// <see cref="AVFrame.flags" />
    /// </summary>
    public int Flags
    {
        get => _ptr->flags;
        set => _ptr->flags = value;
    }
    
    /// <summary>
    /// <para>MPEG vs JPEG YUV range. - encoding: Set by user - decoding: Set by libavcodec</para>
    /// <see cref="AVFrame.color_range" />
    /// </summary>
    public AVColorRange ColorRange
    {
        get => _ptr->color_range;
        set => _ptr->color_range = value;
    }
    
    /// <summary>
    /// <see cref="AVFrame.color_primaries" />
    /// </summary>
    public AVColorPrimaries ColorPrimaries
    {
        get => _ptr->color_primaries;
        set => _ptr->color_primaries = value;
    }
    
    /// <summary>
    /// <see cref="AVFrame.color_trc" />
    /// </summary>
    public AVColorTransferCharacteristic ColorTrc
    {
        get => _ptr->color_trc;
        set => _ptr->color_trc = value;
    }
    
    /// <summary>
    /// <para>YUV colorspace type. - encoding: Set by user - decoding: Set by libavcodec</para>
    /// <see cref="AVFrame.colorspace" />
    /// </summary>
    public AVColorSpace Colorspace
    {
        get => _ptr->colorspace;
        set => _ptr->colorspace = value;
    }
    
    /// <summary>
    /// <see cref="AVFrame.chroma_location" />
    /// </summary>
    public AVChromaLocation ChromaLocation
    {
        get => _ptr->chroma_location;
        set => _ptr->chroma_location = value;
    }
    
    /// <summary>
    /// <para>frame timestamp estimated using various heuristics, in stream time base - encoding: unused - decoding: set by libavcodec, read by user.</para>
    /// <see cref="AVFrame.best_effort_timestamp" />
    /// </summary>
    public long BestEffortTimestamp
    {
        get => _ptr->best_effort_timestamp;
        set => _ptr->best_effort_timestamp = value;
    }
    
    /// <summary>
    /// <para>reordered pos from the last AVPacket that has been input into the decoder - encoding: unused - decoding: Read by user.</para>
    /// <see cref="AVFrame.pkt_pos" />
    /// </summary>
    public long PktPosition
    {
        get => _ptr->pkt_pos;
        set => _ptr->pkt_pos = value;
    }
    
    /// <summary>
    /// <para>duration of the corresponding packet, expressed in AVStream-&gt;time_base units, 0 if unknown. - encoding: unused - decoding: Read by user.</para>
    /// <see cref="AVFrame.pkt_duration" />
    /// </summary>
    [Obsolete("use duration instead")]
    public long PktDuration
    {
        get => _ptr->pkt_duration;
        set => _ptr->pkt_duration = value;
    }
    
    /// <summary>
    /// <para>original type: AVDictionary*</para>
    /// <para>metadata. - encoding: Set by user. - decoding: Set by libavcodec.</para>
    /// <see cref="AVFrame.metadata" />
    /// </summary>
    public MediaDictionary Metadata
    {
        get => MediaDictionary.FromNative(_ptr->metadata, false);
        set => _ptr->metadata = (AVDictionary*)value;
    }
    
    /// <summary>
    /// <para>decode error flags of the frame, set to a combination of FF_DECODE_ERROR_xxx flags if the decoder produced a frame, but there were errors during the decoding. - encoding: unused - decoding: set by libavcodec, read by user.</para>
    /// <see cref="AVFrame.decode_error_flags" />
    /// </summary>
    public int DecodeErrorFlags
    {
        get => _ptr->decode_error_flags;
        set => _ptr->decode_error_flags = value;
    }
    
    /// <summary>
    /// <para>number of audio channels, only used for audio. - encoding: unused - decoding: Read by user.</para>
    /// <see cref="AVFrame.channels" />
    /// </summary>
    [Obsolete("use ch_layout instead")]
    public int Channels
    {
        get => _ptr->channels;
        set => _ptr->channels = value;
    }
    
    /// <summary>
    /// <para>size of the corresponding packet containing the compressed frame. It is set to a negative value if unknown. - encoding: unused - decoding: set by libavcodec, read by user.</para>
    /// <see cref="AVFrame.pkt_size" />
    /// </summary>
    public int PktSize
    {
        get => _ptr->pkt_size;
        set => _ptr->pkt_size = value;
    }
    
    /// <summary>
    /// <para>original type: AVBufferRef*</para>
    /// <para>For hwaccel-format frames, this should be a reference to the AVHWFramesContext describing the frame.</para>
    /// <see cref="AVFrame.hw_frames_ctx" />
    /// </summary>
    public BufferRef? HwFramesContext
    {
        get => BufferRef.FromNativeOrNull(_ptr->hw_frames_ctx, false);
        set => _ptr->hw_frames_ctx = value != null ? (AVBufferRef*)value : null;
    }
    
    /// <summary>
    /// <para>original type: AVBufferRef*</para>
    /// <para>AVBufferRef for free use by the API user. FFmpeg will never check the contents of the buffer ref. FFmpeg calls av_buffer_unref() on it when the frame is unreferenced. av_frame_copy_props() calls create a new reference with av_buffer_ref() for the target frame's opaque_ref field.</para>
    /// <see cref="AVFrame.opaque_ref" />
    /// </summary>
    public BufferRef? OpaqueRef
    {
        get => BufferRef.FromNativeOrNull(_ptr->opaque_ref, false);
        set => _ptr->opaque_ref = value != null ? (AVBufferRef*)value : null;
    }
    
    /// <summary>
    /// <para>cropping Video frames only. The number of pixels to discard from the the top/bottom/left/right border of the frame to obtain the sub-rectangle of the frame intended for presentation. @{</para>
    /// <see cref="AVFrame.crop_top" />
    /// </summary>
    public ulong CropTop
    {
        get => _ptr->crop_top;
        set => _ptr->crop_top = value;
    }
    
    /// <summary>
    /// <see cref="AVFrame.crop_bottom" />
    /// </summary>
    public ulong CropBottom
    {
        get => _ptr->crop_bottom;
        set => _ptr->crop_bottom = value;
    }
    
    /// <summary>
    /// <see cref="AVFrame.crop_left" />
    /// </summary>
    public ulong CropLeft
    {
        get => _ptr->crop_left;
        set => _ptr->crop_left = value;
    }
    
    /// <summary>
    /// <see cref="AVFrame.crop_right" />
    /// </summary>
    public ulong CropRight
    {
        get => _ptr->crop_right;
        set => _ptr->crop_right = value;
    }
    
    /// <summary>
    /// <para>original type: AVBufferRef*</para>
    /// <para>AVBufferRef for internal use by a single libav* library. Must not be used to transfer data between libraries. Has to be NULL when ownership of the frame leaves the respective library.</para>
    /// <see cref="AVFrame.private_ref" />
    /// </summary>
    public BufferRef? PrivateRef
    {
        get => BufferRef.FromNativeOrNull(_ptr->private_ref, false);
        set => _ptr->private_ref = value != null ? (AVBufferRef*)value : null;
    }
    
    /// <summary>
    /// <para>Channel layout of the audio data.</para>
    /// <see cref="AVFrame.ch_layout" />
    /// </summary>
    public AVChannelLayout ChLayout
    {
        get => _ptr->ch_layout;
        set => _ptr->ch_layout = value;
    }
    
    /// <summary>
    /// <para>Duration of the frame, in the same units as pts. 0 if unknown.</para>
    /// <see cref="AVFrame.duration" />
    /// </summary>
    public long Duration
    {
        get => _ptr->duration;
        set => _ptr->duration = value;
    }
}
