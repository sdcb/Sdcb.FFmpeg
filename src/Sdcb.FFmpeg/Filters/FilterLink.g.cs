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
/// <para>A link between two filters. This contains pointers to the source and destination filters between which this link exists, and the indexes of the pads involved. In addition, this link also contains the parameters which have been negotiated and agreed upon between the filter, such as image dimensions, format, etc.</para>
/// <see cref="AVFilterLink" />
/// </summary>
public unsafe partial class FilterLink : SafeHandle
{
    protected AVFilterLink* _ptr => (AVFilterLink*)handle;
    
    public static implicit operator AVFilterLink*(FilterLink data) => data != null ? (AVFilterLink*)data.handle : null;
    
    protected FilterLink(AVFilterLink* ptr, bool isOwner): base(NativeUtils.NotNull((IntPtr)ptr), isOwner)
    {
    }
    
    public static FilterLink FromNative(AVFilterLink* ptr, bool isOwner) => new FilterLink(ptr, isOwner);
    
    internal static FilterLink FromNative(IntPtr ptr, bool isOwner) => new FilterLink((AVFilterLink*)ptr, isOwner);
    
    public static FilterLink? FromNativeOrNull(AVFilterLink* ptr, bool isOwner) => ptr == null ? null : new FilterLink(ptr, isOwner);
    
    public override bool IsInvalid => handle == IntPtr.Zero;
    
    /// <summary>
    /// <para>original type: AVFilterContext*</para>
    /// <para>source filter</para>
    /// <see cref="AVFilterLink.src" />
    /// </summary>
    public FilterContext Src
    {
        get => FilterContext.FromNative(_ptr->src, false);
        set => _ptr->src = (AVFilterContext*)value;
    }
    
    /// <summary>
    /// <para>original type: AVFilterPad*</para>
    /// <para>output pad on the source filter</para>
    /// <see cref="AVFilterLink.srcpad" />
    /// </summary>
    public FilterPad? Srcpad => FilterPad.FromNativeOrNull(_ptr->srcpad);
    
    /// <summary>
    /// <para>original type: AVFilterContext*</para>
    /// <para>dest filter</para>
    /// <see cref="AVFilterLink.dst" />
    /// </summary>
    public FilterContext Dst
    {
        get => FilterContext.FromNative(_ptr->dst, false);
        set => _ptr->dst = (AVFilterContext*)value;
    }
    
    /// <summary>
    /// <para>original type: AVFilterPad*</para>
    /// <para>input pad on the dest filter</para>
    /// <see cref="AVFilterLink.dstpad" />
    /// </summary>
    public FilterPad? Dstpad => FilterPad.FromNativeOrNull(_ptr->dstpad);
    
    /// <summary>
    /// <para>filter media type</para>
    /// <see cref="AVFilterLink.type" />
    /// </summary>
    public AVMediaType Type
    {
        get => _ptr->type;
        set => _ptr->type = value;
    }
    
    /// <summary>
    /// <para>agreed upon image width</para>
    /// <see cref="AVFilterLink.w" />
    /// </summary>
    public int W
    {
        get => _ptr->w;
        set => _ptr->w = value;
    }
    
    /// <summary>
    /// <para>agreed upon image height</para>
    /// <see cref="AVFilterLink.h" />
    /// </summary>
    public int H
    {
        get => _ptr->h;
        set => _ptr->h = value;
    }
    
    /// <summary>
    /// <para>agreed upon sample aspect ratio</para>
    /// <see cref="AVFilterLink.sample_aspect_ratio" />
    /// </summary>
    public AVRational SampleAspectRatio
    {
        get => _ptr->sample_aspect_ratio;
        set => _ptr->sample_aspect_ratio = value;
    }
    
    /// <summary>
    /// <para>channel layout of current buffer (see libavutil/channel_layout.h)</para>
    /// <see cref="AVFilterLink.channel_layout" />
    /// </summary>
    [Obsolete("use ch_layout")]
    public ulong ChannelLayout
    {
        get => _ptr->channel_layout;
        set => _ptr->channel_layout = value;
    }
    
    /// <summary>
    /// <para>samples per second</para>
    /// <see cref="AVFilterLink.sample_rate" />
    /// </summary>
    public int SampleRate
    {
        get => _ptr->sample_rate;
        set => _ptr->sample_rate = value;
    }
    
    /// <summary>
    /// <para>agreed upon media format</para>
    /// <see cref="AVFilterLink.format" />
    /// </summary>
    public int Format
    {
        get => _ptr->format;
        set => _ptr->format = value;
    }
    
    /// <summary>
    /// <para>Define the time base used by the PTS of the frames/samples which will pass through this link. During the configuration stage, each filter is supposed to change only the output timebase, while the timebase of the input link is assumed to be an unchangeable property.</para>
    /// <see cref="AVFilterLink.time_base" />
    /// </summary>
    public AVRational TimeBase
    {
        get => _ptr->time_base;
        set => _ptr->time_base = value;
    }
    
    /// <summary>
    /// <para>channel layout of current buffer (see libavutil/channel_layout.h)</para>
    /// <see cref="AVFilterLink.ch_layout" />
    /// </summary>
    public AVChannelLayout ChLayout
    {
        get => _ptr->ch_layout;
        set => _ptr->ch_layout = value;
    }
    
    /// <summary>
    /// <para>Lists of supported formats / etc. supported by the input filter.</para>
    /// <see cref="AVFilterLink.incfg" />
    /// </summary>
    public AVFilterFormatsConfig Incfg
    {
        get => _ptr->incfg;
        set => _ptr->incfg = value;
    }
    
    /// <summary>
    /// <para>Lists of supported formats / etc. supported by the output filter.</para>
    /// <see cref="AVFilterLink.outcfg" />
    /// </summary>
    public AVFilterFormatsConfig Outcfg
    {
        get => _ptr->outcfg;
        set => _ptr->outcfg = value;
    }
    
    /// <summary>
    /// <see cref="AVFilterLink.init_state" />
    /// </summary>
    public AVFilterLink_init_state InitState
    {
        get => _ptr->init_state;
        set => _ptr->init_state = value;
    }
    
    /// <summary>
    /// <para>original type: AVFilterGraph*</para>
    /// <para>Graph the filter belongs to.</para>
    /// <see cref="AVFilterLink.graph" />
    /// </summary>
    public FilterGraph? Graph
    {
        get => FilterGraph.FromNativeOrNull(_ptr->graph, false);
        set => _ptr->graph = value != null ? (AVFilterGraph*)value : null;
    }
    
    /// <summary>
    /// <para>Current timestamp of the link, as defined by the most recent frame(s), in link time_base units.</para>
    /// <see cref="AVFilterLink.current_pts" />
    /// </summary>
    public long CurrentPts
    {
        get => _ptr->current_pts;
        set => _ptr->current_pts = value;
    }
    
    /// <summary>
    /// <para>Current timestamp of the link, as defined by the most recent frame(s), in AV_TIME_BASE units.</para>
    /// <see cref="AVFilterLink.current_pts_us" />
    /// </summary>
    public long CurrentPtsUs
    {
        get => _ptr->current_pts_us;
        set => _ptr->current_pts_us = value;
    }
    
    /// <summary>
    /// <para>Index in the age array.</para>
    /// <see cref="AVFilterLink.age_index" />
    /// </summary>
    public int AgeIndex
    {
        get => _ptr->age_index;
        set => _ptr->age_index = value;
    }
    
    /// <summary>
    /// <para>Frame rate of the stream on the link, or 1/0 if unknown or variable; if left to 0/0, will be automatically copied from the first input of the source filter if it exists.</para>
    /// <see cref="AVFilterLink.frame_rate" />
    /// </summary>
    public AVRational FrameRate
    {
        get => _ptr->frame_rate;
        set => _ptr->frame_rate = value;
    }
    
    /// <summary>
    /// <para>Minimum number of samples to filter at once. If filter_frame() is called with fewer samples, it will accumulate them in fifo. This field and the related ones must not be changed after filtering has started. If 0, all related fields are ignored.</para>
    /// <see cref="AVFilterLink.min_samples" />
    /// </summary>
    public int MinSamples
    {
        get => _ptr->min_samples;
        set => _ptr->min_samples = value;
    }
    
    /// <summary>
    /// <para>Maximum number of samples to filter at once. If filter_frame() is called with more samples, it will split them.</para>
    /// <see cref="AVFilterLink.max_samples" />
    /// </summary>
    public int MaxSamples
    {
        get => _ptr->max_samples;
        set => _ptr->max_samples = value;
    }
    
    /// <summary>
    /// <para>Number of past frames sent through the link.</para>
    /// <see cref="AVFilterLink.frame_count_in" />
    /// </summary>
    public long FrameCountIn
    {
        get => _ptr->frame_count_in;
        set => _ptr->frame_count_in = value;
    }
    
    /// <summary>
    /// <para>Number of past frames sent through the link.</para>
    /// <see cref="AVFilterLink.frame_count_out" />
    /// </summary>
    public long FrameCountOut
    {
        get => _ptr->frame_count_out;
        set => _ptr->frame_count_out = value;
    }
    
    /// <summary>
    /// <para>Number of past samples sent through the link.</para>
    /// <see cref="AVFilterLink.sample_count_in" />
    /// </summary>
    public long SampleCountIn
    {
        get => _ptr->sample_count_in;
        set => _ptr->sample_count_in = value;
    }
    
    /// <summary>
    /// <para>Number of past samples sent through the link.</para>
    /// <see cref="AVFilterLink.sample_count_out" />
    /// </summary>
    public long SampleCountOut
    {
        get => _ptr->sample_count_out;
        set => _ptr->sample_count_out = value;
    }
    
    /// <summary>
    /// <para>original type: void*</para>
    /// <para>A pointer to a FFFramePool struct.</para>
    /// <see cref="AVFilterLink.frame_pool" />
    /// </summary>
    public IntPtr FramePool
    {
        get => (IntPtr)_ptr->frame_pool;
        set => _ptr->frame_pool = (void*)value;
    }
    
    /// <summary>
    /// <para>True if a frame is currently wanted on the output of this filter. Set when ff_request_frame() is called by the output, cleared when a frame is filtered.</para>
    /// <see cref="AVFilterLink.frame_wanted_out" />
    /// </summary>
    public int FrameWantedOut
    {
        get => _ptr->frame_wanted_out;
        set => _ptr->frame_wanted_out = value;
    }
    
    /// <summary>
    /// <para>original type: AVBufferRef*</para>
    /// <para>For hwaccel pixel formats, this should be a reference to the AVHWFramesContext describing the frames.</para>
    /// <see cref="AVFilterLink.hw_frames_ctx" />
    /// </summary>
    public BufferRef? HwFramesContext
    {
        get => BufferRef.FromNativeOrNull(_ptr->hw_frames_ctx, false);
        set => _ptr->hw_frames_ctx = value != null ? (AVBufferRef*)value : null;
    }
    
}
