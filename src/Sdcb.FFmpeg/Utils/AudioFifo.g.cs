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
/// <para>Context for an Audio FIFO Buffer.</para>
/// <see cref="AVAudioFifo" />
/// </summary>
public unsafe partial class AudioFifo : SafeHandle
{
    protected AVAudioFifo* _ptr => (AVAudioFifo*)handle;
    
    public static implicit operator AVAudioFifo*(AudioFifo data) => data != null ? (AVAudioFifo*)data.handle : null;
    
    protected AudioFifo(AVAudioFifo* ptr, bool isOwner): base(NativeUtils.NotNull((IntPtr)ptr), isOwner)
    {
    }
    
    public static AudioFifo FromNative(AVAudioFifo* ptr, bool isOwner) => new AudioFifo(ptr, isOwner);
    
    internal static AudioFifo FromNative(IntPtr ptr, bool isOwner) => new AudioFifo((AVAudioFifo*)ptr, isOwner);
    
    public static AudioFifo? FromNativeOrNull(AVAudioFifo* ptr, bool isOwner) => ptr == null ? null : new AudioFifo(ptr, isOwner);
    
    public override bool IsInvalid => handle == IntPtr.Zero;
    
}
