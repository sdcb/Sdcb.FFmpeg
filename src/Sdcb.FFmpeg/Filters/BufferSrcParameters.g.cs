// This file was genereated from Sdcb.FFmpeg.AutoGen, DO NOT CHANGE DIRECTLY.
#nullable enable
using Sdcb.FFmpeg.Common;
using Sdcb.FFmpeg.Codecs;
using Sdcb.FFmpeg.Formats;
using Sdcb.FFmpeg.Utils;
using Sdcb.FFmpeg.Raw;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Sdcb.FFmpeg.Filters;

/// <summary>
/// <para>This structure contains the parameters describing the frames that will be passed to this filter.</para>
/// <see cref="AVBufferSrcParameters" />
/// </summary>
public unsafe partial class BufferSrcParameters : SafeHandle
{
    protected AVBufferSrcParameters* _ptr => (AVBufferSrcParameters*)handle;
    
    public static implicit operator AVBufferSrcParameters*(BufferSrcParameters data) => data != null ? (AVBufferSrcParameters*)data.handle : null;
    
    protected BufferSrcParameters(AVBufferSrcParameters* ptr, bool isOwner): base(NativeUtils.NotNull((IntPtr)ptr), isOwner)
    {
    }
    
    public static BufferSrcParameters FromNative(AVBufferSrcParameters* ptr, bool isOwner) => new BufferSrcParameters(ptr, isOwner);
    
    internal static BufferSrcParameters FromNative(IntPtr ptr, bool isOwner) => new BufferSrcParameters((AVBufferSrcParameters*)ptr, isOwner);
    
    public static BufferSrcParameters? FromNativeOrNull(AVBufferSrcParameters* ptr, bool isOwner) => ptr == null ? null : new BufferSrcParameters(ptr, isOwner);
    
    public override bool IsInvalid => handle == IntPtr.Zero;
    
    /// <summary>
    /// <para>video: the pixel format, value corresponds to enum AVPixelFormat audio: the sample format, value corresponds to enum AVSampleFormat</para>
    /// <see cref="AVBufferSrcParameters.format" />
    /// </summary>
    public int Format
    {
        get => _ptr->format;
        set => _ptr->format = value;
    }
    
    /// <summary>
    /// <para>The timebase to be used for the timestamps on the input frames.</para>
    /// <see cref="AVBufferSrcParameters.time_base" />
    /// </summary>
    public AVRational TimeBase
    {
        get => _ptr->time_base;
        set => _ptr->time_base = value;
    }
    
    /// <summary>
    /// <para>Video only, the display dimensions of the input frames.</para>
    /// <see cref="AVBufferSrcParameters.width" />
    /// </summary>
    public int Width
    {
        get => _ptr->width;
        set => _ptr->width = value;
    }
    
    /// <summary>
    /// <para>Video only, the display dimensions of the input frames.</para>
    /// <see cref="AVBufferSrcParameters.height" />
    /// </summary>
    public int Height
    {
        get => _ptr->height;
        set => _ptr->height = value;
    }
    
    /// <summary>
    /// <para>Video only, the sample (pixel) aspect ratio.</para>
    /// <see cref="AVBufferSrcParameters.sample_aspect_ratio" />
    /// </summary>
    public AVRational SampleAspectRatio
    {
        get => _ptr->sample_aspect_ratio;
        set => _ptr->sample_aspect_ratio = value;
    }
    
    /// <summary>
    /// <para>Video only, the frame rate of the input video. This field must only be set to a non-zero value if input stream has a known constant framerate and should be left at its initial value if the framerate is variable or unknown.</para>
    /// <see cref="AVBufferSrcParameters.frame_rate" />
    /// </summary>
    public AVRational FrameRate
    {
        get => _ptr->frame_rate;
        set => _ptr->frame_rate = value;
    }
    
    /// <summary>
    /// <para>original type: AVBufferRef*</para>
    /// <para>Video with a hwaccel pixel format only. This should be a reference to an AVHWFramesContext instance describing the input frames.</para>
    /// <see cref="AVBufferSrcParameters.hw_frames_ctx" />
    /// </summary>
    public BufferRef? HwFramesContext
    {
        get => BufferRef.FromNativeOrNull(_ptr->hw_frames_ctx, false);
        set => _ptr->hw_frames_ctx = value != null ? (AVBufferRef*)value : null;
    }
    
    /// <summary>
    /// <para>Audio only, the audio sampling rate in samples per second.</para>
    /// <see cref="AVBufferSrcParameters.sample_rate" />
    /// </summary>
    public int SampleRate
    {
        get => _ptr->sample_rate;
        set => _ptr->sample_rate = value;
    }
    
    /// <summary>
    /// <para>Audio only, the audio channel layout</para>
    /// <see cref="AVBufferSrcParameters.channel_layout" />
    /// </summary>
    [Obsolete("use ch_layout")]
    public ulong ChannelLayout
    {
        get => _ptr->channel_layout;
        set => _ptr->channel_layout = value;
    }
    
    /// <summary>
    /// <para>Audio only, the audio channel layout</para>
    /// <see cref="AVBufferSrcParameters.ch_layout" />
    /// </summary>
    public AVChannelLayout ChLayout
    {
        get => _ptr->ch_layout;
        set => _ptr->ch_layout = value;
    }
}
