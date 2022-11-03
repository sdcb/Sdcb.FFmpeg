using Sdcb.FFmpeg.Codecs;
using Sdcb.FFmpeg.Common;
using Sdcb.FFmpeg.Formats;
using Sdcb.FFmpeg.Raw;
using Sdcb.FFmpeg.Swscales;
using Sdcb.FFmpeg.Utils;
using System;
using System.IO;
using System.Linq;

namespace Sdcb.FFmpeg.Toolboxs.Extensions;

public unsafe static class FrameExtensions
{
    public static void WriteImageTo(this Frame frame, string url, OutputFormat? format = null, string? formatName = null)
    {
        using FormatContext fc = FormatContext.AllocOutput(format, formatName, url);
        using IOContext io = IOContext.OpenWrite(url);
        fc.Pb = io;

        WriteImageTo(frame, fc);
    }

    public static void WriteImageTo(this Frame frame, Stream stream, OutputFormat? format = null, string? formatName = null, bool leaveOpen = false)
    {
        using FormatContext fc = FormatContext.AllocOutput(format, formatName);
        using IOContext io = IOContext.WriteStream(stream);
        fc.Pb = io;

        WriteImageTo(frame, fc);
        if (!leaveOpen)
        {
            stream.Close();
        }
    }

    public static DisposableDataPointer Encode(this Frame frame, OutputFormat? format = null, string? formatName = null)
    {
        using FormatContext fc = FormatContext.AllocOutput(format, formatName);
        using DynamicIOContext io = IOContext.OpenDynamic();
        fc.Pb = io;
        WriteImageTo(frame, fc);

        return io.GetBufferAndClose();
    }

    public static byte[] EncodeToBytes(this Frame frame, OutputFormat? format = null, string? formatName = null)
    {
        using DisposableDataPointer data = frame.Encode(format, formatName);
        return data.AsSpan().ToArray();
    }

    private static void WriteImageTo(Frame frame, FormatContext fc)
    {
        if (fc.OutputFormat == null)
        {
            throw new FFmpegException($"{nameof(fc.OutputFormat)} must be not null before calling {nameof(WriteImageTo)}");
        }

        Codec codec = Codec.FindEncoderById(fc.OutputFormat.Value.VideoCodec);
        var mediaStream = new MediaStream(fc);
        using CodecContext codecContext = new (codec);
        {
            codecContext.PixelFormat = codec.PixelFormats.First();
            codecContext.Width = frame.Width;
            codecContext.Height = frame.Height;
            codecContext.TimeBase = new AVRational(1, 25);
            codecContext.Flags = fc.OutputFormat.Value.Flags.HasFlag(AVFMT.Globalheader) ? AV_CODEC_FLAG.GlobalHeader : default;
        };
        codecContext.Open(codec);
        mediaStream.Codecpar!.CopyFrom(codecContext);

        if ((AVPixelFormat)frame.Format == codecContext.PixelFormat)
        {
            WriteAction(frame, fc, codecContext);
        }
        else
        {
            using var tempFrame = Frame.CreateVideo(frame.Width, frame.Height, codecContext.PixelFormat);
            using var frameConverter = new VideoFrameConverter();
            frameConverter.ConvertFrame(frame, tempFrame);
            WriteAction(tempFrame, fc, codecContext);
        }
    }

    private static void WriteAction(Frame frame, FormatContext fc, CodecContext codecContext)
    {
        fc.WriteHeader();
        foreach (Packet packet in new[] { frame }.EncodeFrames(codecContext))
        {
            fc.WritePacket(packet);
        }
        fc.WriteTrailer();
    }
}
