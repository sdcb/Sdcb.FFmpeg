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
/// <see cref="AVCodecParserContext" />
/// </summary>
public unsafe partial class CodecParserContext : SafeHandle
{
    protected AVCodecParserContext* _ptr => (AVCodecParserContext*)handle;
    
    public static implicit operator AVCodecParserContext*(CodecParserContext data) => data != null ? (AVCodecParserContext*)data.handle : null;
    
    protected CodecParserContext(AVCodecParserContext* ptr, bool isOwner): base(NativeUtils.NotNull((IntPtr)ptr), isOwner)
    {
    }
    
    public static CodecParserContext FromNative(AVCodecParserContext* ptr, bool isOwner) => new CodecParserContext(ptr, isOwner);
    
    internal static CodecParserContext FromNative(IntPtr ptr, bool isOwner) => new CodecParserContext((AVCodecParserContext*)ptr, isOwner);
    
    public static CodecParserContext? FromNativeOrNull(AVCodecParserContext* ptr, bool isOwner) => ptr == null ? null : new CodecParserContext(ptr, isOwner);
    
    public override bool IsInvalid => handle == IntPtr.Zero;
    
    /// <summary>
    /// <para>original type: void*</para>
    /// <see cref="AVCodecParserContext.priv_data" />
    /// </summary>
    public IntPtr PrivateData
    {
        get => (IntPtr)_ptr->priv_data;
        set => _ptr->priv_data = (void*)value;
    }
    
    /// <summary>
    /// <see cref="AVCodecParserContext.parser" />
    /// </summary>
    public AVCodecParser* Parser
    {
        get => _ptr->parser;
        set => _ptr->parser = value;
    }
    
    /// <summary>
    /// <see cref="AVCodecParserContext.frame_offset" />
    /// </summary>
    public long FrameOffset
    {
        get => _ptr->frame_offset;
        set => _ptr->frame_offset = value;
    }
    
    /// <summary>
    /// <see cref="AVCodecParserContext.cur_offset" />
    /// </summary>
    public long CurOffset
    {
        get => _ptr->cur_offset;
        set => _ptr->cur_offset = value;
    }
    
    /// <summary>
    /// <see cref="AVCodecParserContext.next_frame_offset" />
    /// </summary>
    public long NextFrameOffset
    {
        get => _ptr->next_frame_offset;
        set => _ptr->next_frame_offset = value;
    }
    
    /// <summary>
    /// <see cref="AVCodecParserContext.pict_type" />
    /// </summary>
    public int PictType
    {
        get => _ptr->pict_type;
        set => _ptr->pict_type = value;
    }
    
    /// <summary>
    /// <para>This field is used for proper frame duration computation in lavf. It signals, how much longer the frame duration of the current frame is compared to normal frame duration.</para>
    /// <see cref="AVCodecParserContext.repeat_pict" />
    /// </summary>
    public int RepeatPict
    {
        get => _ptr->repeat_pict;
        set => _ptr->repeat_pict = value;
    }
    
    /// <summary>
    /// <see cref="AVCodecParserContext.pts" />
    /// </summary>
    public long Pts
    {
        get => _ptr->pts;
        set => _ptr->pts = value;
    }
    
    /// <summary>
    /// <see cref="AVCodecParserContext.dts" />
    /// </summary>
    public long Dts
    {
        get => _ptr->dts;
        set => _ptr->dts = value;
    }
    
    /// <summary>
    /// <see cref="AVCodecParserContext.last_pts" />
    /// </summary>
    public long LastPts
    {
        get => _ptr->last_pts;
        set => _ptr->last_pts = value;
    }
    
    /// <summary>
    /// <see cref="AVCodecParserContext.last_dts" />
    /// </summary>
    public long LastDts
    {
        get => _ptr->last_dts;
        set => _ptr->last_dts = value;
    }
    
    /// <summary>
    /// <see cref="AVCodecParserContext.fetch_timestamp" />
    /// </summary>
    public int FetchTimestamp
    {
        get => _ptr->fetch_timestamp;
        set => _ptr->fetch_timestamp = value;
    }
    
    /// <summary>
    /// <see cref="AVCodecParserContext.cur_frame_start_index" />
    /// </summary>
    public int CurFrameStartIndex
    {
        get => _ptr->cur_frame_start_index;
        set => _ptr->cur_frame_start_index = value;
    }
    
    /// <summary>
    /// <see cref="AVCodecParserContext.cur_frame_offset" />
    /// </summary>
    public ref long_array4 CurFrameOffset
    {
        get => ref _ptr->cur_frame_offset;
    }
    
    /// <summary>
    /// <see cref="AVCodecParserContext.cur_frame_pts" />
    /// </summary>
    public ref long_array4 CurFramePts
    {
        get => ref _ptr->cur_frame_pts;
    }
    
    /// <summary>
    /// <see cref="AVCodecParserContext.cur_frame_dts" />
    /// </summary>
    public ref long_array4 CurFrameDts
    {
        get => ref _ptr->cur_frame_dts;
    }
    
    /// <summary>
    /// <see cref="AVCodecParserContext.flags" />
    /// </summary>
    public int Flags
    {
        get => _ptr->flags;
        set => _ptr->flags = value;
    }
    
    /// <summary>
    /// <para>byte offset from starting packet start</para>
    /// <see cref="AVCodecParserContext.offset" />
    /// </summary>
    public long Offset
    {
        get => _ptr->offset;
        set => _ptr->offset = value;
    }
    
    /// <summary>
    /// <see cref="AVCodecParserContext.cur_frame_end" />
    /// </summary>
    public ref long_array4 CurFrameEnd
    {
        get => ref _ptr->cur_frame_end;
    }
    
    /// <summary>
    /// <para>Set by parser to 1 for key frames and 0 for non-key frames. It is initialized to -1, so if the parser doesn't set this flag, old-style fallback using AV_PICTURE_TYPE_I picture type as key frames will be used.</para>
    /// <see cref="AVCodecParserContext.key_frame" />
    /// </summary>
    public int KeyFrame
    {
        get => _ptr->key_frame;
        set => _ptr->key_frame = value;
    }
    
    /// <summary>
    /// <para>Synchronization point for start of timestamp generation.</para>
    /// <see cref="AVCodecParserContext.dts_sync_point" />
    /// </summary>
    public int DtsSyncPoint
    {
        get => _ptr->dts_sync_point;
        set => _ptr->dts_sync_point = value;
    }
    
    /// <summary>
    /// <para>Offset of the current timestamp against last timestamp sync point in units of AVCodecContext.time_base.</para>
    /// <see cref="AVCodecParserContext.dts_ref_dts_delta" />
    /// </summary>
    public int DtsRefDtsDelta
    {
        get => _ptr->dts_ref_dts_delta;
        set => _ptr->dts_ref_dts_delta = value;
    }
    
    /// <summary>
    /// <para>Presentation delay of current frame in units of AVCodecContext.time_base.</para>
    /// <see cref="AVCodecParserContext.pts_dts_delta" />
    /// </summary>
    public int PtsDtsDelta
    {
        get => _ptr->pts_dts_delta;
        set => _ptr->pts_dts_delta = value;
    }
    
    /// <summary>
    /// <para>Position of the packet in file.</para>
    /// <see cref="AVCodecParserContext.cur_frame_pos" />
    /// </summary>
    public ref long_array4 CurFramePosition
    {
        get => ref _ptr->cur_frame_pos;
    }
    
    /// <summary>
    /// <para>Byte position of currently parsed frame in stream.</para>
    /// <see cref="AVCodecParserContext.pos" />
    /// </summary>
    public long Position
    {
        get => _ptr->pos;
        set => _ptr->pos = value;
    }
    
    /// <summary>
    /// <para>Previous frame byte position.</para>
    /// <see cref="AVCodecParserContext.last_pos" />
    /// </summary>
    public long LastPosition
    {
        get => _ptr->last_pos;
        set => _ptr->last_pos = value;
    }
    
    /// <summary>
    /// <para>Duration of the current frame. For audio, this is in units of 1 / AVCodecContext.sample_rate. For all other types, this is in units of AVCodecContext.time_base.</para>
    /// <see cref="AVCodecParserContext.duration" />
    /// </summary>
    public int Duration
    {
        get => _ptr->duration;
        set => _ptr->duration = value;
    }
    
    /// <summary>
    /// <see cref="AVCodecParserContext.field_order" />
    /// </summary>
    public AVFieldOrder FieldOrder
    {
        get => _ptr->field_order;
        set => _ptr->field_order = value;
    }
    
    /// <summary>
    /// <para>Indicate whether a picture is coded as a frame, top field or bottom field.</para>
    /// <see cref="AVCodecParserContext.picture_structure" />
    /// </summary>
    public AVPictureStructure PictureStructure
    {
        get => _ptr->picture_structure;
        set => _ptr->picture_structure = value;
    }
    
    /// <summary>
    /// <para>Picture number incremented in presentation or output order. This field may be reinitialized at the first picture of a new sequence.</para>
    /// <see cref="AVCodecParserContext.output_picture_number" />
    /// </summary>
    public int OutputPictureNumber
    {
        get => _ptr->output_picture_number;
        set => _ptr->output_picture_number = value;
    }
    
    /// <summary>
    /// <para>Dimensions of the decoded video intended for presentation.</para>
    /// <see cref="AVCodecParserContext.width" />
    /// </summary>
    public int Width
    {
        get => _ptr->width;
        set => _ptr->width = value;
    }
    
    /// <summary>
    /// <see cref="AVCodecParserContext.height" />
    /// </summary>
    public int Height
    {
        get => _ptr->height;
        set => _ptr->height = value;
    }
    
    /// <summary>
    /// <para>Dimensions of the coded video.</para>
    /// <see cref="AVCodecParserContext.coded_width" />
    /// </summary>
    public int CodedWidth
    {
        get => _ptr->coded_width;
        set => _ptr->coded_width = value;
    }
    
    /// <summary>
    /// <see cref="AVCodecParserContext.coded_height" />
    /// </summary>
    public int CodedHeight
    {
        get => _ptr->coded_height;
        set => _ptr->coded_height = value;
    }
    
    /// <summary>
    /// <para>The format of the coded data, corresponds to enum AVPixelFormat for video and for enum AVSampleFormat for audio.</para>
    /// <see cref="AVCodecParserContext.format" />
    /// </summary>
    public int Format
    {
        get => _ptr->format;
        set => _ptr->format = value;
    }
}
