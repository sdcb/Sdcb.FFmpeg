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
/// <para>AVProfile.</para>
/// <see cref="AVProfile" />
/// </summary>
public unsafe partial struct MediaProfile
{
    private AVProfile* _ptr;
    
    public static implicit operator AVProfile*(MediaProfile? data)
        => data.HasValue ? (AVProfile*)data.Value._ptr : null;
    
    private MediaProfile(AVProfile* ptr)
    {
        if (ptr == null)
        {
            throw new ArgumentNullException(nameof(ptr));
        }
        _ptr = ptr;
    }
    
    public static MediaProfile FromNative(AVProfile* ptr) => new MediaProfile(ptr);
    public static MediaProfile FromNative(IntPtr ptr) => new MediaProfile((AVProfile*)ptr);
    internal static MediaProfile? FromNativeOrNull(AVProfile* ptr)
        => ptr != null ? new MediaProfile?(new MediaProfile(ptr)) : null;
    
    /// <summary>
    /// <see cref="AVProfile.profile" />
    /// </summary>
    public int Profile => _ptr->profile;
    
    /// <summary>
    /// <para>original type: byte*</para>
    /// <para>short name for the profile</para>
    /// <see cref="AVProfile.name" />
    /// </summary>
    public string? Name => _ptr->name != null ? PtrExtensions.PtrToStringUTF8((IntPtr)_ptr->name)! : null;
}
