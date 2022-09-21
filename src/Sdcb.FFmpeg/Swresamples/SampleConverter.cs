using Sdcb.FFmpeg.Codecs;
using Sdcb.FFmpeg.Common;
using Sdcb.FFmpeg.Raw;
using System;
using System.Runtime.InteropServices;
using static Sdcb.FFmpeg.Raw.ffmpeg;

namespace Sdcb.FFmpeg.Swresamples;

/// <summary>
/// <see cref="SwrContext"/>
/// </summary>
public unsafe class SampleConverter : SafeHandle
{
    /// <summary>
    /// <see cref="swr_get_class"/>
    /// </summary>
    public static FFmpegClass TypeClass = FFmpegClass.FromNative(swr_get_class());

    /// <summary>
    /// <see cref="swr_alloc"/>
    /// </summary>
    public SampleConverter() : base(NativeUtils.NotNull((IntPtr)swr_alloc()), ownsHandle: true)
    {
    }

    public static implicit operator SwrContext*(SampleConverter data) => (SwrContext*)data.handle;

    public FFmpegOptions Options => new FFmpegOptions((void*)handle);

    /// <summary>
    /// <see cref="swr_alloc_set_opts(SwrContext*, long, AVSampleFormat, int, long, AVSampleFormat, int, int, void*)"/>
    /// </summary>
    [Obsolete("use Reset2")]
    public void Reset(
        long outputChannelLayout, AVSampleFormat outputSampleFormat, int outputSampleRate,
        long inputChannelLayout, AVSampleFormat inputSampleFormat, int inputSampleRate)
    {
        handle = NativeUtils.NotNull((IntPtr)swr_alloc_set_opts(this,
            outputChannelLayout, outputSampleFormat, outputSampleRate,
            inputChannelLayout, inputSampleFormat, inputSampleRate,
            log_offset: 0, log_ctx: null));
    }

    /// <summary>
    /// <see cref="swr_alloc_set_opts2(SwrContext**, AVChannelLayout*, AVSampleFormat, int, AVChannelLayout*, AVSampleFormat, int, int, void*)"/>
    /// </summary>
    public void Reset2(
        AVChannelLayout outputChannelLayout, AVSampleFormat outputSampleFormat, int outputSampleRate,
        AVChannelLayout inputChannelLayout, AVSampleFormat inputSampleFormat, int inputSampleRate)
    {
        SwrContext* ctx = this;
        int ret = swr_alloc_set_opts2(&ctx,
            &outputChannelLayout, outputSampleFormat, outputSampleRate,
            &inputChannelLayout, inputSampleFormat, inputSampleRate,
            log_offset: 0, log_ctx: null);
        handle = (IntPtr)ctx;

        if (ret != 0)
        {
            throw FFmpegException.FromErrorCode(ret, $"Error when calling {nameof(swr_alloc_set_opts2)}");
        }
    }

    /// <summary>
    /// <see cref="swr_is_initialized(SwrContext*)"/>
    /// </summary>
    public bool Initialized => swr_is_initialized(this) != 0 ? true : false;

    public override bool IsInvalid => handle == IntPtr.Zero;

    /// <summary>
    /// <see cref="swr_init(SwrContext*)"/>
    /// </summary>
    public void Initialize() => swr_init(this);

    /// <summary>
    /// <see cref="swr_convert(SwrContext*, byte**, int, byte**, int)"/>
    /// </summary>
    public int Convert(byte_ptrArray8 outData, int outCount, byte_ptrArray8 inData, int inCount)
    {
        return swr_convert(this, (byte**)&outData, outCount, (byte**)&inData, inCount).ThrowIfError();
    }

    /// <summary>
    /// <see cref="swr_convert_frame(SwrContext*, AVFrame*, AVFrame*)"/>
    /// </summary>
    public int ConvertFrame(Frame outFrame, Frame inFrame)
    {
        return swr_convert_frame(this, outFrame, inFrame).ThrowIfError();
    }

    /// <summary>
    /// <see cref="swr_next_pts(SwrContext*, long)"/>
    /// </summary>
    public long NextPts(long pts) => swr_next_pts(this, pts);

    /// <summary>
    /// <see cref="swr_free(SwrContext**)"/>
    /// </summary>
    public void Free()
    {
        SwrContext* ptr = this;
        swr_free(&ptr);
        SetHandleAsInvalid();
    }

    /// <summary>
    /// <see cref="swr_close(SwrContext*)"/>
    /// </summary>
    public void Close() => swr_close(this);

    protected override bool ReleaseHandle()
    {
        Free();
        return true;
    }
}
