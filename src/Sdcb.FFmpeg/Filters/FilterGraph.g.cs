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
/// <see cref="AVFilterGraph" />
/// </summary>
public unsafe partial class FilterGraph : SafeHandle
{
    protected AVFilterGraph* _ptr => (AVFilterGraph*)handle;
    
    public static implicit operator AVFilterGraph*(FilterGraph data) => data != null ? (AVFilterGraph*)data.handle : null;
    
    protected FilterGraph(AVFilterGraph* ptr, bool isOwner): base(NativeUtils.NotNull((IntPtr)ptr), isOwner)
    {
    }
    
    public static FilterGraph FromNative(AVFilterGraph* ptr, bool isOwner) => new FilterGraph(ptr, isOwner);
    
    internal static FilterGraph FromNative(IntPtr ptr, bool isOwner) => new FilterGraph((AVFilterGraph*)ptr, isOwner);
    
    public static FilterGraph? FromNativeOrNull(AVFilterGraph* ptr, bool isOwner) => ptr == null ? null : new FilterGraph(ptr, isOwner);
    
    public override bool IsInvalid => handle == IntPtr.Zero;
    
    /// <summary>
    /// <para>original type: AVClass*</para>
    /// <see cref="AVFilterGraph.av_class" />
    /// </summary>
    public FFmpegClass AvClass
    {
        get => FFmpegClass.FromNative(_ptr->av_class);
        set => _ptr->av_class = (AVClass*)value;
    }
    
    /// <summary>
    /// <para>original type: AVFilterContext**</para>
    /// <see cref="AVFilterGraph.filters" />
    /// </summary>
    public IReadOnlyList<FilterContext> Filters => new ReadOnlyPtrList<AVFilterContext, FilterContext>(_ptr->filters, (int)_ptr->nb_filters, p => FilterContext.FromNative(p, isOwner: false))!;
    
    /// <summary>
    /// <para>original type: byte*</para>
    /// <para>sws options to use for the auto-inserted scale filters</para>
    /// <see cref="AVFilterGraph.scale_sws_opts" />
    /// </summary>
    public IntPtr ScaleSwsOpts
    {
        get => (IntPtr)_ptr->scale_sws_opts;
        set => _ptr->scale_sws_opts = (byte*)value;
    }
    
    /// <summary>
    /// <para>original type: byte*</para>
    /// <para>libavresample options to use for the auto-inserted resample filters</para>
    /// <see cref="AVFilterGraph.resample_lavr_opts" />
    /// </summary>
    public IntPtr ResampleLavrOpts
    {
        get => (IntPtr)_ptr->resample_lavr_opts;
        set => _ptr->resample_lavr_opts = (byte*)value;
    }
    
    /// <summary>
    /// <para>Type of multithreading allowed for filters in this graph. A combination of AVFILTER_THREAD_* flags.</para>
    /// <see cref="AVFilterGraph.thread_type" />
    /// </summary>
    public int ThreadType
    {
        get => _ptr->thread_type;
        set => _ptr->thread_type = value;
    }
    
    /// <summary>
    /// <para>Maximum number of threads used by filters in this graph. May be set by the caller before adding any filters to the filtergraph. Zero (the default) means that the number of threads is determined automatically.</para>
    /// <see cref="AVFilterGraph.nb_threads" />
    /// </summary>
    public int NbThreads
    {
        get => _ptr->nb_threads;
        set => _ptr->nb_threads = value;
    }
    
    /// <summary>
    /// <para>Opaque object for libavfilter internal use.</para>
    /// <see cref="AVFilterGraph.@internal" />
    /// </summary>
    public AVFilterGraphInternal* Internal
    {
        get => _ptr->@internal;
        set => _ptr->@internal = value;
    }
    
    /// <summary>
    /// <para>original type: void*</para>
    /// <para>Opaque user data. May be set by the caller to an arbitrary value, e.g. to be used from callbacks like AVFilterGraph.execute. Libavfilter will not touch this field in any way.</para>
    /// <see cref="AVFilterGraph.opaque" />
    /// </summary>
    public IntPtr Opaque
    {
        get => (IntPtr)_ptr->opaque;
        set => _ptr->opaque = (void*)value;
    }
    
    /// <summary>
    /// <para>original type: byte*</para>
    /// <para>swr options to use for the auto-inserted aresample filters, Access ONLY through AVOptions</para>
    /// <see cref="AVFilterGraph.aresample_swr_opts" />
    /// </summary>
    public IntPtr AresampleSwrOpts
    {
        get => (IntPtr)_ptr->aresample_swr_opts;
        set => _ptr->aresample_swr_opts = (byte*)value;
    }
    
    /// <summary>
    /// <para>original type: AVFilterLink**</para>
    /// <para>Private fields</para>
    /// <see cref="AVFilterGraph.sink_links" />
    /// </summary>
    public IReadOnlyList<FilterLink> SinkLinks => new ReadOnlyPtrList<AVFilterLink, FilterLink>(_ptr->sink_links, (int)_ptr->sink_links_count, p => FilterLink.FromNative(p, isOwner: false))!;
    
    /// <summary>
    /// <see cref="AVFilterGraph.disable_auto_convert" />
    /// </summary>
    public uint DisableAutoConvert
    {
        get => _ptr->disable_auto_convert;
        set => _ptr->disable_auto_convert = value;
    }
}
