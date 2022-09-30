// This file was genereated from Sdcb.FFmpeg.AutoGen, DO NOT CHANGE DIRECTLY.
#nullable enable
using Sdcb.FFmpeg.Common;
using Sdcb.FFmpeg.Codecs;
using Sdcb.FFmpeg.Utils;
using Sdcb.FFmpeg.Filters;
using Sdcb.FFmpeg.Raw;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Sdcb.FFmpeg.Formats;

/// <summary>
/// <see cref="AVChapter" />
/// </summary>
public unsafe partial struct MediaChapter
{
    private AVChapter* _ptr;
    
    public static implicit operator AVChapter*(MediaChapter? data)
        => data.HasValue ? (AVChapter*)data.Value._ptr : null;
    
    private MediaChapter(AVChapter* ptr)
    {
        if (ptr == null)
        {
            throw new ArgumentNullException(nameof(ptr));
        }
        _ptr = ptr;
    }
    
    public static MediaChapter FromNative(AVChapter* ptr) => new MediaChapter(ptr);
    public static MediaChapter FromNative(IntPtr ptr) => new MediaChapter((AVChapter*)ptr);
    internal static MediaChapter? FromNativeOrNull(AVChapter* ptr)
        => ptr != null ? new MediaChapter?(new MediaChapter(ptr)) : null;
    
    /// <summary>
    /// <para>unique ID to identify the chapter</para>
    /// <see cref="AVChapter.id" />
    /// </summary>
    public long Id
    {
        get => _ptr->id;
        set => _ptr->id = value;
    }
    
    /// <summary>
    /// <para>time base in which the start/end timestamps are specified</para>
    /// <see cref="AVChapter.time_base" />
    /// </summary>
    public AVRational TimeBase
    {
        get => _ptr->time_base;
        set => _ptr->time_base = value;
    }
    
    /// <summary>
    /// <para>chapter start/end time in time_base units</para>
    /// <see cref="AVChapter.start" />
    /// </summary>
    public long Start
    {
        get => _ptr->start;
        set => _ptr->start = value;
    }
    
    /// <summary>
    /// <para>chapter start/end time in time_base units</para>
    /// <see cref="AVChapter.end" />
    /// </summary>
    public long End
    {
        get => _ptr->end;
        set => _ptr->end = value;
    }
    
    /// <summary>
    /// <para>original type: AVDictionary*</para>
    /// <see cref="AVChapter.metadata" />
    /// </summary>
    public MediaDictionary Metadata
    {
        get => MediaDictionary.FromNative(_ptr->metadata, false);
        set => _ptr->metadata = (AVDictionary*)value;
    }
}
