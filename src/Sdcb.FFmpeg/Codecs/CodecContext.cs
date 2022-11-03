using Sdcb.FFmpeg.Common;
using Sdcb.FFmpeg.Raw;
using Sdcb.FFmpeg.Utils;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static Sdcb.FFmpeg.Raw.ffmpeg;

namespace Sdcb.FFmpeg.Codecs;

public unsafe partial class CodecContext : SafeHandle
{
    public const int CompressionDefault = FF_COMPRESSION_DEFAULT;

    public FFmpegOptions Options => new FFmpegOptions(this);
    public FFmpegOptions PrivateOptions => new FFmpegOptions(_ptr->priv_data);

    /// <summary>
    /// <see cref="avcodec_alloc_context3(AVCodec*)"/>
    /// </summary>
    /// <param name="codec"></param>
    /// <returns></returns>
    /// <exception cref="FFmpegException"></exception>
    public CodecContext(Codec? codec) : this(
        avcodec_alloc_context3(codec) switch
        {
            null => throw new FFmpegException($"Failed to create {nameof(AVCodecContext)} from codec {codec?.Id}"),
            var x => x,
        },
        isOwner: true)
    { }

    /// <summary>
    /// <see cref="avcodec_get_class"/>
    /// </summary>
    public static FFmpegClass TypeClass => FFmpegClass.FromNative(avcodec_get_class())!;

    /// <summary>
    /// <see cref="avcodec_parameters_to_context(AVCodecContext*, AVCodecParameters*)"/>
    /// </summary>
    public void FillParameters(CodecParameters parameters)
    {
        avcodec_parameters_to_context(this, parameters).ThrowIfError();
    }

    /// <summary>
    /// <see cref="avcodec_open2(AVCodecContext*, AVCodec*, AVDictionary**)"/>
    /// </summary>
    public void Open(Codec? codec = null, MediaDictionary? options = null)
    {
        AVDictionary* ptrDict = options;
        avcodec_open2(this, codec, &ptrDict).ThrowIfError();
        if (options != null)
        {
            options.Reset(ptrDict);
        }
    }

    /// <summary>
    /// <see cref="avcodec_send_packet(AVCodecContext*, AVPacket*)"/>
    /// </summary>
    public void SendPacket(Packet? packet) => avcodec_send_packet(this, packet != null ? (AVPacket*)packet : null).ThrowIfError("Error sending a packet for decoding");

    /// <summary>
    /// <see cref="avcodec_receive_packet(AVCodecContext*, AVPacket*)"/>
    /// </summary>
    public CodecResult ReceivePacket(Packet packet) => ToCodecResult(avcodec_receive_packet(this, packet));

    /// <summary>
    /// <see cref="avcodec_send_frame(AVCodecContext*, AVFrame*)"/>
    /// </summary>
    public void SendFrame(Frame? frame) => avcodec_send_frame(this, frame != null ? (AVFrame*)frame : null).ThrowIfError("Error sending a frame for encoding");

    /// <summary>
    /// <see cref="avcodec_receive_frame(AVCodecContext*, AVFrame*)"/>
    /// </summary>
    public CodecResult ReceiveFrame(Frame frame) => ToCodecResult(avcodec_receive_frame(this, frame));

    internal static CodecResult ToCodecResult(int result, [CallerMemberName] string? callerMember = null) => result switch
    {
        0 => CodecResult.Success,
        var x when x == AVERROR_EOF => CodecResult.EOF,
        var x when x == AVERROR(EAGAIN) => CodecResult.Again,
        var x when x < 0 => throw FFmpegException.FromErrorCode(x, $"{callerMember} failed."),
        _ => throw new FFmpegException($"Unknown {nameof(callerMember)} status: {result}"),
    };

    internal Frame CreateVideoFrame() => Frame.CreateVideo(Width, Height, PixelFormat);
    internal Frame CreateAudioFrame() => Frame.CreateAudio(SampleFormat, ChLayout, SampleRate,
        Codec.Capabilities.HasFlag(AV_CODEC_CAP.VariableFrameSize) ? 10000 : FrameSize);

    public Frame CreateFrame() => Width > 0 ? CreateVideoFrame() : CreateAudioFrame();

    /// <summary>
    /// <see cref="avcodec_free_context(AVCodecContext**)"/>
    /// </summary>
    public void Free()
    {
        AVCodecContext* ptr = this;
        avcodec_free_context(&ptr);
        handle = (IntPtr)ptr;
        SetHandleAsInvalid();
    }

    protected override bool ReleaseHandle()
    {
        Free();
        return true;
    }
}