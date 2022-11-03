using Sdcb.FFmpeg.Common;
using Sdcb.FFmpeg.Raw;
using System;
using System.Runtime.InteropServices;
using static Sdcb.FFmpeg.Raw.ffmpeg;

namespace Sdcb.FFmpeg.Utils;

public unsafe partial class Frame : SafeHandle, IBufferRefed
{
    /// <summary>
    /// <see cref="av_frame_alloc"/>
    /// </summary>
    public Frame() : base(NativeUtils.NotNull((IntPtr)av_frame_alloc()), ownsHandle: true)
    {
    }

    public static Frame CreateVideo(int width, int height, AVPixelFormat pixelFormat)
    {
        var frame = new Frame
        {
            Format = (int)pixelFormat,
            Width = width,
            Height = height,
        };
        frame.EnsureBuffer();
        return frame;
    }

    public static Frame CreateAudio(AVSampleFormat sampleFormat, AVChannelLayout chLayout, int sampleRate, int sampleCount)
    {
        var frame = new Frame
        {
            Format = (int)sampleFormat,
            ChLayout = chLayout,
            SampleRate = sampleRate,
            NbSamples = sampleCount,
        };
        frame.EnsureBuffer();
        return frame;
    }

    /// <summary>
    /// <see cref="av_frame_is_writable(AVFrame*)"/>
    /// </summary>
    public bool IsWritable => av_frame_is_writable(this) != 0;

    /// <summary>
    /// <para>Set up a new reference to the data described by the source frame.</para>
    /// <para>Copy frame properties from src to dst and create a new reference for each AVBufferRef from src.</para>
    /// <para>If src is not reference counted, new buffers are allocated and the data is copied.</para>
    /// <see cref="av_frame_ref(AVFrame*, AVFrame*)"/>
    /// </summary>
    public void Ref(Frame other) => av_frame_ref(this, other).ThrowIfError();

    /// <summary>
    /// <para>Unreference all the buffers referenced by frame and reset the frame fields.</para>
    /// <see cref="av_frame_unref(AVFrame*)"/>
    /// </summary>
    public void Unref() => av_frame_unref(this);

    /// <summary>
    /// <para>Create a new frame that references the same data as src.</para>
    /// <para>This is a shortcut for av_frame_alloc()+av_frame_ref().</para>
    /// <see cref="av_frame_clone(AVFrame*)"/>
    /// </summary>
    public Frame Clone() => FromNative(av_frame_clone(this), isOwner: true);

    /// <summary>
    /// <see cref="av_frame_copy(AVFrame*, AVFrame*)"/>
    /// </summary>
    public void CopyTo(Frame other) => av_frame_copy(other, this).ThrowIfError();

    /// <summary>
    /// <see cref="av_frame_copy_props(AVFrame*, AVFrame*)"/>
    /// </summary>
    public void CopyPropsTo(Frame other) => av_frame_copy_props(other, this).ThrowIfError();

    /// <summary>
    /// <see cref="av_frame_move_ref(AVFrame*, AVFrame*)"/>
    /// </summary>
    public void MoveRef(Frame dest) => av_frame_move_ref(dest, this);

    /// <summary>
    /// <see cref="av_frame_get_buffer(AVFrame*, int)"/>
    /// </summary>
    public void EnsureBuffer(int align = 0) => av_frame_get_buffer(this, align).ThrowIfError();

    /// <summary>
    /// <see cref="av_frame_make_writable(AVFrame*)"/>
    /// </summary>
    public void MakeWritable() => av_frame_make_writable(this).ThrowIfError();

    /// <summary>
    /// <see cref="av_frame_free(AVFrame**)"/>
    /// </summary>
    public void Free()
    {
        AVFrame* ptr = this;
        av_frame_free(&ptr);
        handle = (IntPtr)ptr;
    }

    public byte[] ToImageBuffer(int align = 1)
    {
        if (Width == 0 || Height == 0) throw new FFmpegException("Frame is not image.");

        var buffer = new byte[ImageUtils.GetBufferSize((AVPixelFormat)Format, Width, Height, align)];
        fixed (byte* ptr = buffer)
        {
            ImageUtils.CopyToBuffer((AVPixelFormat)Format, Width, Height, new DataPointer(ptr, buffer.Length), (byte_ptrArray4)Data, (int_array4)Linesize, align);
        }
        return buffer;
    }

    public void FillImageBuffer(byte[] buffer, int align = 1) => FillImageBuffer(buffer.AsSpan(), align);

    public void FillImageBuffer(Span<byte> buffer, int align = 1)
    {
        if (Width == 0 || Height == 0) throw new FFmpegException("Frame is not image.");

        fixed (byte* ptr = buffer)
        {
            ImageUtils.CopyToBuffer((AVPixelFormat)Format, Width, Height, new DataPointer(ptr, buffer.Length), (byte_ptrArray4)Data, (int_array4)Linesize, align);
        }
    }

    protected override bool ReleaseHandle()
    {
        Free();
        return true;
    }

    public static string GetColorspaceName(AVColorSpace val) => av_color_space_name((AVColorSpace)val);

    void IBufferRefed.Ref(IBufferRefed other)
    {
        if (other is Frame frame)
        {
            Ref(frame);
        }
        throw new ArgumentException($"{other} is not a frame.");
    }

    void IBufferRefed.Unref() => Unref();

    void IBufferRefed.MakeWritable() => MakeWritable();

    IBufferRefed IBufferRefed.Clone() => Clone();
}
