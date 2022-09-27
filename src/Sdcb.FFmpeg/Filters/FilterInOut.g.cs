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
/// <para>A linked-list of the inputs/outputs of the filter chain.</para>
/// <see cref="AVFilterInOut" />
/// </summary>
public unsafe partial class FilterInOut : SafeHandle
{
    protected AVFilterInOut* _ptr => (AVFilterInOut*)handle;
    
    public static implicit operator AVFilterInOut*(FilterInOut data) => data != null ? (AVFilterInOut*)data.handle : null;
    
    protected FilterInOut(AVFilterInOut* ptr, bool isOwner): base(NativeUtils.NotNull((IntPtr)ptr), isOwner)
    {
    }
    
    public static FilterInOut FromNative(AVFilterInOut* ptr, bool isOwner) => new FilterInOut(ptr, isOwner);
    
    internal static FilterInOut FromNative(IntPtr ptr, bool isOwner) => new FilterInOut((AVFilterInOut*)ptr, isOwner);
    
    public static FilterInOut? FromNativeOrNull(AVFilterInOut* ptr, bool isOwner) => ptr == null ? null : new FilterInOut(ptr, isOwner);
    
    public override bool IsInvalid => handle == IntPtr.Zero;
    
    /// <summary>
    /// <para>original type: byte*</para>
    /// <para>unique name for this input/output in the list</para>
    /// <see cref="AVFilterInOut.name" />
    /// </summary>
    public string Name
    {
        get => PtrExtensions.PtrToStringUTF8((IntPtr)_ptr->name)!;
        set => _ptr->name = ffmpeg.av_strdup(value);
    }
    
    /// <summary>
    /// <para>original type: AVFilterContext*</para>
    /// <para>filter context associated to this input/output</para>
    /// <see cref="AVFilterInOut.filter_ctx" />
    /// </summary>
    public FilterContext FilterContext
    {
        get => FilterContext.FromNative(_ptr->filter_ctx, false);
        set => _ptr->filter_ctx = (AVFilterContext*)value;
    }
    
    /// <summary>
    /// <para>index of the filt_ctx pad to use for linking</para>
    /// <see cref="AVFilterInOut.pad_idx" />
    /// </summary>
    public int PadIdx
    {
        get => _ptr->pad_idx;
        set => _ptr->pad_idx = value;
    }
    
    /// <summary>
    /// <para>original type: AVFilterInOut*</para>
    /// <para>next input/input in the list, NULL if this is the last</para>
    /// <see cref="AVFilterInOut.next" />
    /// </summary>
    public FilterInOut? Next
    {
        get => FilterInOut.FromNativeOrNull(_ptr->next, false);
        set => _ptr->next = value != null ? (AVFilterInOut*)value : null;
    }
}
