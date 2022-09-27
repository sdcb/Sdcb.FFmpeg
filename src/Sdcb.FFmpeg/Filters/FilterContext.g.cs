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
/// <para>An instance of a filter</para>
/// <see cref="AVFilterContext" />
/// </summary>
public unsafe partial class FilterContext : SafeHandle
{
    protected AVFilterContext* _ptr => (AVFilterContext*)handle;
    
    public static implicit operator AVFilterContext*(FilterContext data) => data != null ? (AVFilterContext*)data.handle : null;
    
    protected FilterContext(AVFilterContext* ptr, bool isOwner): base(NativeUtils.NotNull((IntPtr)ptr), isOwner)
    {
    }
    
    public static FilterContext FromNative(AVFilterContext* ptr, bool isOwner) => new FilterContext(ptr, isOwner);
    
    internal static FilterContext FromNative(IntPtr ptr, bool isOwner) => new FilterContext((AVFilterContext*)ptr, isOwner);
    
    public static FilterContext? FromNativeOrNull(AVFilterContext* ptr, bool isOwner) => ptr == null ? null : new FilterContext(ptr, isOwner);
    
    public override bool IsInvalid => handle == IntPtr.Zero;
    
    /// <summary>
    /// <para>original type: AVClass*</para>
    /// <para>needed for av_log() and filters common options</para>
    /// <see cref="AVFilterContext.av_class" />
    /// </summary>
    public FFmpegClass AvClass
    {
        get => FFmpegClass.FromNative(_ptr->av_class);
        set => _ptr->av_class = (AVClass*)value;
    }
    
    /// <summary>
    /// <para>original type: AVFilter*</para>
    /// <para>the AVFilter of which this is an instance</para>
    /// <see cref="AVFilterContext.filter" />
    /// </summary>
    public Filter Filter
    {
        get => Filter.FromNative(_ptr->filter);
        set => _ptr->filter = (AVFilter*)value;
    }
    
    /// <summary>
    /// <para>original type: byte*</para>
    /// <para>name of this filter instance</para>
    /// <see cref="AVFilterContext.name" />
    /// </summary>
    public string Name
    {
        get => PtrExtensions.PtrToStringUTF8((IntPtr)_ptr->name)!;
        set => Options.Set("name", value);
    }
    
    /// <summary>
    /// <para>original type: AVFilterPad*</para>
    /// <para>array of input pads</para>
    /// <see cref="AVFilterContext.input_pads" />
    /// </summary>
    public FilterPadList InputPads => new FilterPadList(_ptr->input_pads)!;
    
    /// <summary>
    /// <para>original type: AVFilterLink**</para>
    /// <para>array of pointers to input links</para>
    /// <see cref="AVFilterContext.inputs" />
    /// </summary>
    public IReadOnlyList<FilterLink> Inputs => new ReadOnlyPtrList<AVFilterLink, FilterLink>(_ptr->inputs, (int)_ptr->nb_inputs, p => FilterLink.FromNative(p, isOwner: false))!;
    
    /// <summary>
    /// <para>original type: AVFilterPad*</para>
    /// <para>array of output pads</para>
    /// <see cref="AVFilterContext.output_pads" />
    /// </summary>
    public FilterPadList OutputPads => new FilterPadList(_ptr->output_pads)!;
    
    /// <summary>
    /// <para>original type: AVFilterLink**</para>
    /// <para>array of pointers to output links</para>
    /// <see cref="AVFilterContext.outputs" />
    /// </summary>
    public IReadOnlyList<FilterLink> Outputs => new ReadOnlyPtrList<AVFilterLink, FilterLink>(_ptr->outputs, (int)_ptr->nb_outputs, p => FilterLink.FromNative(p, isOwner: false))!;
    
    /// <summary>
    /// <para>original type: void*</para>
    /// <para>private data for use by the filter</para>
    /// <see cref="AVFilterContext.priv" />
    /// </summary>
    public IntPtr Private
    {
        get => (IntPtr)_ptr->priv;
        set => _ptr->priv = (void*)value;
    }
    
    /// <summary>
    /// <para>original type: AVFilterGraph*</para>
    /// <para>filtergraph this filter belongs to</para>
    /// <see cref="AVFilterContext.graph" />
    /// </summary>
    public FilterGraph Graph
    {
        get => FilterGraph.FromNative(_ptr->graph, false);
        set => _ptr->graph = (AVFilterGraph*)value;
    }
    
    /// <summary>
    /// <para>Type of multithreading being allowed/used. A combination of AVFILTER_THREAD_* flags.</para>
    /// <see cref="AVFilterContext.thread_type" />
    /// </summary>
    public int ThreadType
    {
        get => _ptr->thread_type;
        set => _ptr->thread_type = value;
    }
    
    /// <summary>
    /// <para>An opaque struct for libavfilter internal use.</para>
    /// <see cref="AVFilterContext.@internal" />
    /// </summary>
    public AVFilterInternal* Internal
    {
        get => _ptr->@internal;
        set => _ptr->@internal = value;
    }
    
    /// <summary>
    /// <see cref="AVFilterContext.command_queue" />
    /// </summary>
    public AVFilterCommand* CommandQueue
    {
        get => _ptr->command_queue;
        set => _ptr->command_queue = value;
    }
    
    /// <summary>
    /// <para>original type: byte*</para>
    /// <para>enable expression string</para>
    /// <see cref="AVFilterContext.enable_str" />
    /// </summary>
    public string? EnableStr
    {
        get => _ptr->enable_str != null ? PtrExtensions.PtrToStringUTF8((IntPtr)_ptr->enable_str)! : null;
        set => Options.Set("enable_str", value);
    }
    
    /// <summary>
    /// <para>original type: void*</para>
    /// <para>parsed expression (AVExpr*)</para>
    /// <see cref="AVFilterContext.enable" />
    /// </summary>
    public IntPtr Enable
    {
        get => (IntPtr)_ptr->enable;
        set => _ptr->enable = (void*)value;
    }
    
    /// <summary>
    /// <para>variable values for the enable expression</para>
    /// <see cref="AVFilterContext.var_values" />
    /// </summary>
    public double* VarValues
    {
        get => _ptr->var_values;
        set => _ptr->var_values = value;
    }
    
    /// <summary>
    /// <para>the enabled state from the last expression evaluation</para>
    /// <see cref="AVFilterContext.is_disabled" />
    /// </summary>
    public int IsDisabled
    {
        get => _ptr->is_disabled;
        set => _ptr->is_disabled = value;
    }
    
    /// <summary>
    /// <para>original type: AVBufferRef*</para>
    /// <para>For filters which will create hardware frames, sets the device the filter should create them in. All other filters will ignore this field: in particular, a filter which consumes or processes hardware frames will instead use the hw_frames_ctx field in AVFilterLink to carry the hardware context information.</para>
    /// <see cref="AVFilterContext.hw_device_ctx" />
    /// </summary>
    public BufferRef? HwDeviceContext
    {
        get => BufferRef.FromNativeOrNull(_ptr->hw_device_ctx, false);
        set => _ptr->hw_device_ctx = value != null ? (AVBufferRef*)value : null;
    }
    
    /// <summary>
    /// <para>Max number of threads allowed in this filter instance. If &lt;= 0, its value is ignored. Overrides global number of threads set per filter graph.</para>
    /// <see cref="AVFilterContext.nb_threads" />
    /// </summary>
    public int NbThreads
    {
        get => _ptr->nb_threads;
        set => _ptr->nb_threads = value;
    }
    
    /// <summary>
    /// <para>Ready status of the filter. A non-0 value means that the filter needs activating; a higher value suggests a more urgent activation.</para>
    /// <see cref="AVFilterContext.ready" />
    /// </summary>
    public uint Ready
    {
        get => _ptr->ready;
        set => _ptr->ready = value;
    }
    
    /// <summary>
    /// <para>Sets the number of extra hardware frames which the filter will allocate on its output links for use in following filters or by the caller.</para>
    /// <see cref="AVFilterContext.extra_hw_frames" />
    /// </summary>
    public int ExtraHwFrames
    {
        get => _ptr->extra_hw_frames;
        set => _ptr->extra_hw_frames = value;
    }
}
