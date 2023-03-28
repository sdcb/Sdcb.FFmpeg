using System;

using Sdcb.FFmpeg.Codecs;
using Sdcb.FFmpeg.Formats;
using Sdcb.FFmpeg.Raw;

namespace Sdcb.FFmpeg.Toolboxs.Extensions
{
    public static class FormatContextExtensions
    {
        public static (FormatContext context,CodecContext videoEncoder, CodecContext audioEncoder) CreactEncoderContextByInputFormat(this FormatContext inputFormat, OutputFormat? format = null, string? formatName = null, string? fileName = null)
        {
            MediaStream inAudioStream = inputFormat.GetAudioStream();
            MediaStream inVideoStream = inputFormat.GetVideoStream();

            FormatContext outFc = FormatContext.AllocOutput(format,formatName,fileName);
            // dest encoder and streams
            outFc.AudioCodec = Codec.FindEncoderById(inAudioStream.Codecpar!.CodecId);
            MediaStream outAudioStream = outFc.NewStream(outFc.AudioCodec);
            CodecContext audioEncoder = new(outFc.AudioCodec);
            audioEncoder.TimeBase = inAudioStream.TimeBase;
            outAudioStream.Codecpar!.CopyFrom(inAudioStream.Codecpar);
            audioEncoder.FillParameters(outAudioStream.Codecpar);
            audioEncoder.Open(outFc.AudioCodec);
            outAudioStream.TimeBase = audioEncoder.TimeBase;

            outFc.VideoCodec = Codec.FindEncoderById(inVideoStream.Codecpar!.CodecId);
            MediaStream outVideoStream = outFc.NewStream(outFc.VideoCodec);
            CodecContext videoEncoder = new(outFc.VideoCodec)
            {
                Flags = AV_CODEC_FLAG.GlobalHeader,
                ThreadCount = Environment.ProcessorCount - 1,
                ThreadType = ffmpeg.FF_THREAD_FRAME,
            };
            videoEncoder.Framerate = inVideoStream.AvgFrameRate;
            videoEncoder.TimeBase = inVideoStream.TimeBase;
            outVideoStream.Codecpar!.CopyFrom(inVideoStream.Codecpar);
            videoEncoder.FillParameters(outVideoStream.Codecpar);
            videoEncoder.Open(outFc.VideoCodec, null);
            outVideoStream.TimeBase = videoEncoder.TimeBase;
            return (outFc,videoEncoder,audioEncoder);
        }
    }
}
