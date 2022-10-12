using Sdcb.FFmpeg.Codecs;
using Sdcb.FFmpeg.Common;
using Sdcb.FFmpeg.Filters;
using Sdcb.FFmpeg.Formats;
using Sdcb.FFmpeg.Raw;
using Sdcb.FFmpeg.Swresamples;
using Sdcb.FFmpeg.Swscales;
using Sdcb.FFmpeg.Utils;
using System;
using System.Collections.Generic;
using static Sdcb.FFmpeg.Raw.ffmpeg;

namespace Sdcb.FFmpeg.Toolboxs.Extensions;

public static class FramesExtensions
{
    /// <summary>
    /// <para>For video, convert frame pixel format, width, height into the same as CodecContext</para>
    /// <para>For audio, convert frame sames format into the same as CodecContext</para>
    /// <see cref="sws_getCachedContext(SwsContext*, int, int, AVPixelFormat, int, int, AVPixelFormat, int, SwsFilter*, SwsFilter*, double*)"/>
    /// <see cref="sws_scale(SwsContext*, byte*[], int[], int, int, byte*[], int[])"/>
    /// </summary>
    public static IEnumerable<Frame> ConvertFrames(this IEnumerable<Frame> sourceFrames, CodecContext c, SWS swsFlags = SWS.Bilinear)
    {
        if (c.Codec.Type == AVMediaType.Video)
        {
            using Frame destFrame = c.CreateVideoFrame();
            int pts = 0;
            using VideoFrameConverter frameConverter = new();
            foreach (Frame sourceFrame in sourceFrames)
            {
                frameConverter.ConvertFrame(sourceFrame, destFrame, swsFlags);
                destFrame.Pts = pts++;
                yield return destFrame;
            }
        }
        else if (c.Codec.Type == AVMediaType.Audio)
        {
            using Frame destFrame = c.CreateAudioFrame();
            int pts = 0;
            using SampleConverter frameConverter = new();
            foreach (Frame sourceFrame in sourceFrames)
            {
                if (!frameConverter.Initialized)
                {
                    frameConverter.Options.Set("in_channel_layout", sourceFrame.ChannelLayout, default(AV_OPT_SEARCH));
                    frameConverter.Options.Set("in_sample_rate", sourceFrame.SampleRate, default(AV_OPT_SEARCH));
                    frameConverter.Options.Set("in_sample_fmt", (AVSampleFormat)sourceFrame.Format, default(AV_OPT_SEARCH));
                    frameConverter.Options.Set("out_channel_layout", destFrame.ChannelLayout, default(AV_OPT_SEARCH));
                    frameConverter.Options.Set("out_sample_rate", destFrame.SampleRate, default(AV_OPT_SEARCH));
                    frameConverter.Options.Set("out_sample_fmt", (AVSampleFormat)destFrame.Format, default(AV_OPT_SEARCH));
                    frameConverter.Initialize();
                }
                int destSampleCount = (int)av_rescale_rnd(frameConverter.GetDelay(sourceFrame.SampleRate) + sourceFrame.NbSamples, sourceFrame.SampleRate, destFrame.SampleRate, AVRounding.Up);
                int converted = frameConverter.Convert(destFrame.Data, destSampleCount, sourceFrame.Data, sourceFrame.NbSamples);
                destFrame.Pts = pts;
                destFrame.NbSamples = converted;
                pts += converted;
                yield return destFrame;
            }
        }
    }

    public static IEnumerable<Frame> ConvertAllFrames(this IEnumerable<Frame> sourceFrames, CodecContext audioContext, CodecContext videoContext, SWS swsFlags = SWS.Bilinear)
    {
        using Frame destAudioFrame = audioContext.CreateFrame();
        using Frame destVideoFrame = videoContext.CreateFrame();
        int videoPts = 0;
        int audioPts = 0;
        using VideoFrameConverter frameConverter = new();
        using SampleConverter sampleConverter = new();

        foreach (Frame sourceFrame in sourceFrames)
        {
            if (sourceFrame.Width > 0)
            {
                frameConverter.ConvertFrame(sourceFrame, destVideoFrame, swsFlags);
                destVideoFrame.Pts = videoPts++;
                yield return destVideoFrame;
            }
            else if (sourceFrame.SampleRate > 0)
            {
                if (!sampleConverter.Initialized)
                {
                    sampleConverter.Options.Set("in_channel_layout", sourceFrame.ChannelLayout, default(AV_OPT_SEARCH));
                    sampleConverter.Options.Set("in_sample_rate", sourceFrame.SampleRate, default(AV_OPT_SEARCH));
                    sampleConverter.Options.Set("in_sample_fmt", (AVSampleFormat)sourceFrame.Format, default(AV_OPT_SEARCH));
                    sampleConverter.Options.Set("out_channel_layout", destAudioFrame.ChannelLayout, default(AV_OPT_SEARCH));
                    sampleConverter.Options.Set("out_sample_rate", destAudioFrame.SampleRate, default(AV_OPT_SEARCH));
                    sampleConverter.Options.Set("out_sample_fmt", (AVSampleFormat)destAudioFrame.Format, default(AV_OPT_SEARCH));
                    sampleConverter.Initialize();
                }
                int destSampleCount = (int)av_rescale_rnd(sampleConverter.GetDelay(sourceFrame.SampleRate) + sourceFrame.NbSamples, sourceFrame.SampleRate, destAudioFrame.SampleRate, AVRounding.Up);
                int converted = sampleConverter.Convert(destAudioFrame.Data, destSampleCount, sourceFrame.Data, sourceFrame.NbSamples);
                destAudioFrame.Pts = audioPts;
                destAudioFrame.NbSamples = converted;
                audioPts += converted;
                yield return destAudioFrame;
            }
            else
            {
                yield return sourceFrame;
            }
        }
    }

    public static IEnumerable<Frame> ApplyVideoFilters(this IEnumerable<Frame> srcFrames, AVRational srcTimebase, AVPixelFormat destPixelFormat, string filterText)
    {
        VideoFilterContext? ctx = null;

        try
        {
            using Frame destFrame = new();
            foreach (Frame srcFrame in srcFrames)
            {
                if (srcFrame.Width > 0)
                {
                    if (ctx == null)
                    {
                        ctx = VideoFilterContext.Create(srcFrame, srcTimebase, filterText, destPixelFormat);
                    }

                    foreach (Frame frame in ctx.WriteFrame(destFrame, srcFrame))
                    {
                        yield return frame;
                    }
                }
                else
                {
                    yield return srcFrame;
                }
            }

            if (ctx == null) throw new InvalidOperationException($"Unable to apply filter, no frame provided.");
            foreach (Frame frame in ctx.WriteFrame(destFrame, null))
            {
                yield return frame;
            }
        }
        finally
        {
            ctx?.Dispose();
        }
    }

    public static IEnumerable<Frame> ApplyVideoFilters(this IEnumerable<Frame> srcFrames, VideoFilterContext ctx)
    {
        using Frame destFrame = new();
        foreach (Frame srcFrame in srcFrames)
        {
            if (srcFrame.Width > 0)
            {
                foreach (Frame frame in ctx.WriteFrame(destFrame, srcFrame))
                {
                    yield return frame;
                }
            }
            else
            {
                yield return srcFrame;
            }
        }

        foreach (Frame frame in ctx.WriteFrame(destFrame, null))
        {
            yield return frame;
        }
    }

    public static IEnumerable<Frame> ApplyAudioFilters(this IEnumerable<Frame> srcFrames, AudioSinkParams sinkParams, string filterText)
    {
        AudioFilterContext? ctx = null;

        try
        {
            using Frame destFrame = new();
            foreach (Frame srcFrame in srcFrames)
            {
                if (srcFrame.SampleRate > 0)
                {
                    if (ctx == null)
                    {
                        ctx = AudioFilterContext.Create(srcFrame, filterText, sinkParams);
                    }

                    foreach (Frame frame in ctx.WriteFrame(destFrame, srcFrame))
                    {
                        yield return frame;
                    }
                }
                else
                {
                    yield return srcFrame;
                }
            }

            if (ctx == null) throw new InvalidOperationException($"Unable to apply filter, no frame provided.");
            foreach (Frame frame in ctx.WriteFrame(destFrame, null))
            {
                yield return frame;
            }
        }
        finally
        {
            ctx?.Dispose();
        }
    }

    public static IEnumerable<Frame> ApplyAudioFilters(this IEnumerable<Frame> srcFrames, AudioFilterContext ctx)
    {
        using Frame destFrame = new();
        foreach (Frame srcFrame in srcFrames)
        {
            if (srcFrame.SampleRate > 0)
            {
                foreach (Frame frame in ctx.WriteFrame(destFrame, srcFrame))
                {
                    yield return frame;
                }
            }
            else
            {
                yield return srcFrame;
            }
        }

        foreach (Frame frame in ctx.WriteFrame(destFrame, null))
        {
            yield return frame;
        }
    }

    public static IEnumerable<Frame> AudioFifo(this IEnumerable<Frame> frames, CodecContext encoder)
    {
        using AudioFifo fifo = new AudioFifo(encoder.SampleFormat, encoder.Channels, 1);
        int frameSize = encoder.FrameSize;
        using Frame result = Frame.CreateWritableAudio(encoder.SampleFormat, encoder.ChannelLayout, encoder.SampleRate, frameSize);
        int nextPts = 0;
        foreach (Frame frame in frames)
        {
            if (frame.SampleRate > 0)
            {
                if (fifo.Size < frameSize)
                {
                    fifo.Write(frame);
                }
                while (fifo.Size >= frameSize)
                {
                    fifo.Read(result);
                    result.Pts = nextPts;
                    nextPts += result.NbSamples;
                    yield return result;
                }
            }
            else
            {
                // bypass all video or other frames
                yield return frame;
            }
        }
        while (fifo.Size > 0)
        {
            fifo.Read(result);
            result.Pts = nextPts;
            nextPts += result.NbSamples;
            yield return result;
        }
    }

    /// <summary>
    /// frames -> packets
    /// </summary>
    public static IEnumerable<Packet> EncodeFrames(this IEnumerable<Frame> frames, CodecContext c, bool makeWritable = true, bool makeSequential = false)
    {
        using var packet = new Packet();
        int pts = 0;
        foreach (Frame frame in frames)
        {
            if (makeSequential && c.Codec.Type == AVMediaType.Video)
                frame.Pts = pts++;
            else if (makeSequential && c.SampleRate > 0 && c.Codec.Type == AVMediaType.Audio)
            {
                frame.Pts = pts;
                pts += c.FrameSize;
            }

            foreach (var _ in c.EncodeFrame(frame, packet))
                yield return packet;

            if (makeWritable) frame.MakeWritable();
        }

        foreach (var _ in c.EncodeFrame(null, packet))
            yield return packet;
    }

    /// <summary>
    /// frames -> packets
    /// </summary>
    public static IEnumerable<Packet> EncodeAllFrames(this IEnumerable<Frame> frames, FormatContext fc,
        CodecContext? audioEncoder = null,
        CodecContext? videoEncoder = null,
        bool makeWritable = true, 
        bool allowSkipFrame = true)
    {
        using Packet packet = new();
        int audioPts = 0, videoPts = 0;
        (MediaStream stream, CodecContext c)? audio = fc.FindBestStreamOrNull(AVMediaType.Audio) switch
        {
            null => null,
            var x => (x.Value, audioEncoder ?? throw new ArgumentNullException(nameof(audioEncoder)))
        };
        (MediaStream stream, CodecContext c)? video = fc.FindBestStreamOrNull(AVMediaType.Video) switch
        {
            null => null,
            var x => (x.Value, videoEncoder ?? throw new ArgumentNullException(nameof(videoEncoder)))
        };

        foreach (Frame frame in frames)
        {
            (MediaStream stream, CodecContext c)? ctx = frame switch
            {
                { Width: > 0 } => video,
                { SampleRate: > 0 } => audio,
                _ => null,
            };
            if (ctx == null)
            {
                if (allowSkipFrame)
                {
                    continue;
                }
                else
                {
                    throw frame switch
                    {
                        { Width: > 0 } => new InvalidOperationException($"Received a video frame but no {nameof(videoEncoder)} provided and {nameof(allowSkipFrame)}=false"),
                        { SampleRate: > 0 } => new InvalidOperationException($"Received a audio frame but no {nameof(videoEncoder)} provided and {nameof(allowSkipFrame)}=false"),
                        _ => new InvalidOperationException($"Received a frame but no correspoding encoder provided and {nameof(allowSkipFrame)}=false"),
                    };
                }
            }

            if (frame.Width > 0)
            {
                frame.Pts = videoPts++;
            }
            else if (frame.SampleRate > 0)
            {
                frame.Pts = audioPts;
                audioPts += frame.NbSamples;
            }

            (MediaStream stream, CodecContext c) = ctx.Value;
            foreach (var _ in c.EncodeFrame(frame, packet))
            {
                packet.RescaleTimestamp(c.TimeBase, ctx.Value.stream.TimeBase);
                packet.StreamIndex = stream.Index;
                yield return packet;
            }

            if (makeWritable) frame.MakeWritable();
        }

        bool encodeVideo = video != null;
        bool encodeAudio = audio != null;
        while (encodeVideo || encodeAudio)
        {
            if (encodeVideo && (!encodeAudio || av_compare_ts(videoPts, videoEncoder!.TimeBase, audioPts, audioEncoder!.TimeBase) <= 0))
            {
                (MediaStream stream, CodecContext c) = video!.Value;
                foreach (var _ in c.EncodeFrame(null, packet))
                {
                    packet.RescaleTimestamp(c.TimeBase, stream.TimeBase);
                    packet.StreamIndex = stream.Index;
                    yield return packet;
                }
                encodeVideo = false;
            }
            else
            {
                (MediaStream stream, CodecContext c) = audio!.Value;
                foreach (var _ in c.EncodeFrame(null, packet))
                {
                    packet.RescaleTimestamp(c.TimeBase, stream.TimeBase);
                    packet.StreamIndex = stream.Index;
                    yield return packet;
                }
                encodeAudio = false;
            }
        }
    }
}
