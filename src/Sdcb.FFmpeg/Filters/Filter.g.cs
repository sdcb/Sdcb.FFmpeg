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
/// <para>Filter definition. This defines the pads a filter contains, and all the callback functions used to interact with the filter.</para>
/// <see cref="AVFilter" />
/// </summary>
public unsafe partial struct Filter
{
    private AVFilter* _ptr;
    
    public static implicit operator AVFilter*(Filter? data)
        => data.HasValue ? (AVFilter*)data.Value._ptr : null;
    
    private Filter(AVFilter* ptr)
    {
        if (ptr == null)
        {
            throw new ArgumentNullException(nameof(ptr));
        }
        _ptr = ptr;
    }
    
    public static Filter FromNative(AVFilter* ptr) => new Filter(ptr);
    public static Filter FromNative(IntPtr ptr) => new Filter((AVFilter*)ptr);
    internal static Filter? FromNativeOrNull(AVFilter* ptr)
        => ptr != null ? new Filter?(new Filter(ptr)) : null;
    
    /// <summary>
    /// <para>original type: byte*</para>
    /// <para>Filter name. Must be non-NULL and unique among filters.</para>
    /// <see cref="AVFilter.name" />
    /// </summary>
    public string Name => PtrExtensions.PtrToStringUTF8((IntPtr)_ptr->name)!;
    
    /// <summary>
    /// <para>original type: byte*</para>
    /// <para>A description of the filter. May be NULL.</para>
    /// <see cref="AVFilter.description" />
    /// </summary>
    public string? Description => _ptr->description != null ? PtrExtensions.PtrToStringUTF8((IntPtr)_ptr->description)! : null;
    
    /// <summary>
    /// <para>List of inputs, terminated by a zeroed element.</para>
    /// <see cref="AVFilter.inputs" />
    /// </summary>
    public AVFilterPad* Inputs => _ptr->inputs;
    
    /// <summary>
    /// <para>List of outputs, terminated by a zeroed element.</para>
    /// <see cref="AVFilter.outputs" />
    /// </summary>
    public AVFilterPad* Outputs => _ptr->outputs;
    
    /// <summary>
    /// <para>original type: AVClass*</para>
    /// <para>A class for the private data, used to declare filter private AVOptions. This field is NULL for filters that do not declare any options.</para>
    /// <see cref="AVFilter.priv_class" />
    /// </summary>
    public FFmpegClass? PrivateClass => FFmpegClass.FromNativeOrNull(_ptr->priv_class);
    
    /// <summary>
    /// <para>original type: int</para>
    /// <para>A combination of AVFILTER_FLAG_*</para>
    /// <see cref="AVFilter.flags" />
    /// </summary>
    public AVFILTER_FLAG Flags => (AVFILTER_FLAG)_ptr->flags;
    
    /// <summary>
    /// <para>size of private data to allocate for the filter</para>
    /// <see cref="AVFilter.priv_size" />
    /// </summary>
    public int PrivateSize => _ptr->priv_size;
    
    /// <summary>
    /// <para>Additional flags for avfilter internal use only.</para>
    /// <see cref="AVFilter.flags_internal" />
    /// </summary>
    public int FlagsInternal => _ptr->flags_internal;
    
}
