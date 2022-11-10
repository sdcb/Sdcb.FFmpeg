using Sdcb.FFmpeg.Codecs;
using Sdcb.FFmpeg.Formats;
using Sdcb.FFmpeg.Raw;
using Sdcb.FFmpeg.Swresamples;
using Sdcb.FFmpeg.Swscales;
using Sdcb.FFmpeg.Toolboxs.FilterTools;
using Sdcb.FFmpeg.Utils;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static Sdcb.FFmpeg.Raw.ffmpeg;

namespace Sdcb.FFmpeg.Toolboxs.Extensions;

public static class FramesExtensions
{
    /// <summary>
    /// Calling every frame with following apis:
    /// <list type="bullet">
    /// <item><see cref="av_frame_clone"/></item>
    /// <item><see cref="av_frame_make_writable"/></item>
    /// </list>
    /// </summary>
    /// <returns>The result must free manually, and frames can be stored.</returns>
    public static IEnumerable<Frame> CloneMakeWritable(this IEnumerable<Frame> frames, bool unref = true)
    {
        foreach (Frame frame in frames)
        {
            Frame cloned = frame.Clone();
            if (unref) frame.Unref();
            cloned.MakeWritable();
            yield return cloned;
        }
    }

    /// <summary>
    /// <para>For video, convert frame pixel format, width, height into the same as CodecContext</para>
    /// <para>For audio, convert frame sames format into the same as CodecContext</para>
    /// <see cref="sws_getCachedContext(SwsContext*, int, int, AVPixelFormat, int, int, AVPixelFormat, int, SwsFilter*, SwsFilter*, double*)"/>
    /// <see cref="sws_scale(SwsContext*, byte*[], int[], int, int, byte*[], int[])"/>
    /// </summary>
    /// <returns>Caller must call <see cref="Frame.Unref"/> to the result when not used.</returns>
    public static IEnumerable<Frame> ConvertFrames(this IEnumerable<Frame> sourceFrames, CodecContext c, SWS swsFlags = SWS.Bilinear, bool unref = true)
    {
        if (c.Codec.Type == AVMediaType.Video)
        {
            using Frame dest = c.CreateVideoFrame();
            using Frame destRef = new Frame();
            int pts = 0;
            using VideoFrameConverter frameConverter = new();
            foreach (Frame sourceFrame in sourceFrames)
            {
                if (sourceFrame.Width > 0)
                {
                    dest.MakeWritable();
                    frameConverter.ConvertFrame(sourceFrame, dest, swsFlags);
                    if (unref) sourceFrame.Unref();
                    dest.Pts = pts++;
                    destRef.Ref(dest);
                    yield return destRef;
                }
                else
                {
                    yield return sourceFrame;
                }
            }
        }
        else if (c.Codec.Type == AVMediaType.Audio)
        {
            using Frame dest = c.CreateAudioFrame();
            using Frame destRef = new Frame();
            int pts = 0;
            using SampleConverter frameConverter = new();
            foreach (Frame sourceFrame in sourceFrames)
            {
                if (sourceFrame.SampleRate > 0)
                {
                    if (!frameConverter.Initialized)
                    {
                        frameConverter.Options.Set("in_chlayout", sourceFrame.ChLayout, default(AV_OPT_SEARCH));
                        frameConverter.Options.Set("in_sample_rate", sourceFrame.SampleRate, default(AV_OPT_SEARCH));
                        frameConverter.Options.Set("in_sample_fmt", (AVSampleFormat)sourceFrame.Format, default(AV_OPT_SEARCH));
                        frameConverter.Options.Set("out_chlayout", dest.ChLayout, default(AV_OPT_SEARCH));
                        frameConverter.Options.Set("out_sample_rate", dest.SampleRate, default(AV_OPT_SEARCH));
                        frameConverter.Options.Set("out_sample_fmt", (AVSampleFormat)dest.Format, default(AV_OPT_SEARCH));
                        frameConverter.Initialize();
                    }
                    int destSampleCount = (int)av_rescale_rnd(frameConverter.GetDelay(sourceFrame.SampleRate) + sourceFrame.NbSamples, sourceFrame.SampleRate, dest.SampleRate, AVRounding.Up);
                    dest.MakeWritable();
                    int converted = frameConverter.Convert(dest.Data, destSampleCount, sourceFrame.Data, sourceFrame.NbSamples);
                    if (unref) sourceFrame.Unref();
                    dest.Pts = pts;
                    dest.NbSamples = converted;
                    pts += converted;
                    destRef.Ref(dest);
                    yield return destRef;
                }
                else
                {
                    yield return sourceFrame;
                }
            }
        }
    }

    /// <returns>Caller must call <see cref="Frame.Unref"/> to the result when not used.</returns>
    public static IEnumerable<Frame> ConvertAllFrames(this IEnumerable<Frame> sourceFrames, CodecContext audioContext, CodecContext videoContext, SWS swsFlags = SWS.Bilinear, bool unref = true)
    {
        using Frame destAudioFrame = audioContext.CreateFrame();
        using Frame destVideoFrame = videoContext.CreateFrame();
        using Frame destRef = new Frame();
        int videoPts = 0;
        int audioPts = 0;
        using VideoFrameConverter frameConverter = new();
        using SampleConverter sampleConverter = new();

        foreach (Frame src in sourceFrames)
        {
            if (src.Width > 0)
            {
                destVideoFrame.MakeWritable();
                frameConverter.ConvertFrame(src, destVideoFrame, swsFlags);
                if (unref) src.Unref();

                destVideoFrame.Pts = videoPts++;
                destRef.Ref(destVideoFrame);
                yield return destRef;
            }
            else if (src.SampleRate > 0)
            {
                if (!sampleConverter.Initialized)
                {
                    sampleConverter.Options.Set("in_chlayout", src.ChLayout, default(AV_OPT_SEARCH));
                    sampleConverter.Options.Set("in_sample_rate", src.SampleRate, default(AV_OPT_SEARCH));
                    sampleConverter.Options.Set("in_sample_fmt", (AVSampleFormat)src.Format, default(AV_OPT_SEARCH));
                    sampleConverter.Options.Set("out_chlayout", destAudioFrame.ChLayout, default(AV_OPT_SEARCH));
                    sampleConverter.Options.Set("out_sample_rate", destAudioFrame.SampleRate, default(AV_OPT_SEARCH));
                    sampleConverter.Options.Set("out_sample_fmt", (AVSampleFormat)destAudioFrame.Format, default(AV_OPT_SEARCH));
                    sampleConverter.Initialize();
                }
                int destSampleCount = (int)av_rescale_rnd(sampleConverter.GetDelay(src.SampleRate) + src.NbSamples, src.SampleRate, destAudioFrame.SampleRate, AVRounding.Up);
                destAudioFrame.MakeWritable();
                int converted = sampleConverter.Convert(destAudioFrame.Data, destSampleCount, src.Data, src.NbSamples);
                if (unref) src.Unref();

                destAudioFrame.Pts = audioPts;
                destAudioFrame.NbSamples = converted;
                audioPts += converted;
                destRef.Ref(destAudioFrame);
                yield return destRef;
            }
            else
            {
                yield return src;
            }
        }
    }

    /// <returns>Caller must call <see cref="Frame.Unref"/> to the result when not used.</returns>
    public static IEnumerable<Frame> ApplyVideoFilters(this IEnumerable<Frame> srcFrames, AVRational srcTimebase, AVPixelFormat destPixelFormat, string filterText, bool unref = true)
    {
        VideoFilterContext? ctx = null;

        try
        {
            using Frame destRef = new();
            foreach (Frame srcFrame in srcFrames)
            {
                if (srcFrame.Width > 0)
                {
                    if (ctx == null)
                    {
                        ctx = VideoFilterContext.Create(srcFrame, srcTimebase, filterText, destPixelFormat);
                    }

                    foreach (Frame frame in ctx.WriteFrame(destRef, srcFrame, unref))
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
            foreach (Frame frame in ctx.WriteFrame(destRef, null, unref))
            {
                yield return frame;
            }
        }
        finally
        {
            ctx?.Dispose();
        }
    }

    /// <returns>Caller must call <see cref="Frame.Unref"/> to the result when not used.</returns>
    public static IEnumerable<Frame> ApplyVideoFilters(this IEnumerable<Frame> srcFrames, VideoFilterContext ctx, bool unref = true)
    {
        using Frame destRef = new();
        foreach (Frame srcFrame in srcFrames)
        {
            if (srcFrame.Width > 0)
            {
                foreach (Frame frame in ctx.WriteFrame(destRef, srcFrame, unref))
                {
                    yield return frame;
                }
            }
            else
            {
                yield return srcFrame;
            }
        }

        foreach (Frame frame in ctx.WriteFrame(destRef, null, unref))
        {
            yield return frame;
        }
    }

    /// <returns>Caller must call <see cref="Frame.Unref"/> to the result when not used.</returns>
    public static IEnumerable<Frame> ApplyAudioFilters(this IEnumerable<Frame> srcFrames, AudioSinkParams sinkParams, string filterText, bool unref = true)
    {
        AudioFilterContext? ctx = null;

        try
        {
            using Frame destRef = new();
            foreach (Frame srcFrame in srcFrames)
            {
                if (srcFrame.SampleRate > 0)
                {
                    if (ctx == null)
                    {
                        ctx = AudioFilterContext.Create(srcFrame, filterText, sinkParams);
                    }

                    foreach (Frame frame in ctx.WriteFrame(destRef, srcFrame, unref))
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
            foreach (Frame frame in ctx.WriteFrame(destRef, null, unref))
            {
                yield return frame;
            }
        }
        finally
        {
            ctx?.Dispose();
        }
    }

    /// <returns>Caller must call <see cref="Frame.Unref"/> to the result when not used.</returns>
    public static IEnumerable<Frame> ApplyAudioFilters(this IEnumerable<Frame> srcFrames, AudioFilterContext ctx, bool unref = true)
    {
        using Frame destRef = new();
        foreach (Frame srcFrame in srcFrames)
        {
            if (srcFrame.SampleRate > 0)
            {
                foreach (Frame frame in ctx.WriteFrame(destRef, srcFrame, unref))
                {
                    yield return frame;
                }
            }
            else
            {
                yield return srcFrame;
            }
        }

        foreach (Frame frame in ctx.WriteFrame(destRef, null, unref))
        {
            yield return frame;
        }
    }

    /// <returns>Caller must call <see cref="Frame.Unref"/> to the result when not used.</returns>
    public static IEnumerable<Frame> ApplyFilters(this IEnumerable<Frame> srcFrames,
        AudioFilterContext? audioCtx = null,
        VideoFilterContext? videoCtx = null,
        bool unref = true)
    {
        using Frame destRef = new();
        foreach (Frame srcFrame in srcFrames)
        {
            if (srcFrame.SampleRate > 0 && audioCtx != null)
            {
                foreach (Frame frame in audioCtx.WriteFrame(destRef, srcFrame, unref))
                {
                    yield return frame;
                }
            }
            else if (srcFrame.Width > 0 && videoCtx != null)
            {
                foreach (Frame frame in videoCtx.WriteFrame(destRef, srcFrame, unref))
                {
                    yield return frame;
                }
            }
            else
            {
                yield return srcFrame;
            }
        }

        if (audioCtx != null)
        {
            foreach (Frame frame in audioCtx.WriteFrame(destRef, null, unref))
            {
                yield return frame;
            }
        }
        if (videoCtx != null)
        {
            foreach (Frame frame in videoCtx.WriteFrame(destRef, null, unref))
            {
                yield return frame;
            }
        }
    }

    /// <returns>Caller must call <see cref="Frame.Unref"/> to the result when not used.</returns>
    public static IEnumerable<Frame> AudioFifo(this IEnumerable<Frame> frames, CodecContext encoder, bool unref = true)
    {
        using AudioFifo fifo = new AudioFifo(encoder.SampleFormat, encoder.ChLayout.nb_channels, sampleSize: 1);
        int frameSize = encoder.FrameSize;
        using Frame dest = Frame.CreateAudio(encoder.SampleFormat, encoder.ChLayout, encoder.SampleRate, frameSize);
        using Frame destRef = new Frame();
        int nextPts = 0;
        foreach (Frame frame in frames)
        {
            if (frame.SampleRate > 0)
            {
                if (fifo.Size < frameSize)
                {
                    fifo.Write(frame);
                    if (unref) frame.Unref();
                }
                while (fifo.Size >= frameSize)
                {
                    fifo.Read(dest);
                    dest.Pts = nextPts;
                    nextPts += dest.NbSamples;

                    destRef.Ref(dest);
                    yield return destRef;
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
            fifo.Read(dest);
            dest.Pts = nextPts;
            nextPts += dest.NbSamples;

            destRef.Ref(dest);
            yield return destRef;
        }
    }

    /// <summary>
    /// frames -> packets
    /// </summary>
    /// <returns>Caller must call <see cref="Packet.Unref"/> to the result when not used.</returns>
    public static IEnumerable<Packet> EncodeFrames(this IEnumerable<Frame> frames, CodecContext c, bool makeSequential = false)
    {
        using Packet packetRef = new Packet();
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

            foreach (Packet packet in c.EncodeFrame(frame, packetRef))
                yield return packet;
        }

        foreach (Packet packet in c.EncodeFrame(null, packetRef))
            yield return packet;
    }

    /// <summary>
    /// frames -> packets
    /// </summary>
    /// <returns>Caller must call <see cref="Packet.Unref"/> to the result when not used.</returns>
    public static IEnumerable<Packet> EncodeAllFrames(this IEnumerable<Frame> frames, FormatContext fc,
        CodecContext? audioEncoder = null,
        CodecContext? videoEncoder = null,
        bool allowSkipFrame = true)
    {
        using Packet packetRef = new();
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
            foreach (Packet packet in c.EncodeFrame(frame, packetRef))
            {
                packet.RescaleTimestamp(c.TimeBase, ctx.Value.stream.TimeBase);
                packet.StreamIndex = stream.Index;
                yield return packet;
            }
        }

        bool encodeVideo = video != null;
        bool encodeAudio = audio != null;
        while (encodeVideo || encodeAudio)
        {
            if (encodeVideo && (!encodeAudio || av_compare_ts(videoPts, videoEncoder!.TimeBase, audioPts, audioEncoder!.TimeBase) <= 0))
            {
                (MediaStream stream, CodecContext c) = video!.Value;
                foreach (Packet packet in c.EncodeFrame(null, packetRef))
                {
                    packet.RescaleTimestamp(c.TimeBase, stream.TimeBase);
                    packet.StreamIndex = stream.Index;
                    yield return packetRef;
                }
                encodeVideo = false;
            }
            else
            {
                (MediaStream stream, CodecContext c) = audio!.Value;
                foreach (Packet packet in c.EncodeFrame(null, packetRef))
                {
                    packet.RescaleTimestamp(c.TimeBase, stream.TimeBase);
                    packet.StreamIndex = stream.Index;
                    yield return packet;
                }
                encodeAudio = false;
            }
        }
    }

    public static IEnumerable<Frame> ConvertVideoFrames(this IEnumerable<Frame> sourceFrames, Func<(int width, int height)> sizeAccessor, AVPixelFormat pixelFormat, SWS swsFlags = SWS.Bilinear, bool unref = true)
    {
        Frame dest;
        {
            (int width, int height) = sizeAccessor();
            dest = Frame.CreateVideo(width, height, pixelFormat);
        }
        using Frame destRef = new();

        try
        {
            int pts = 0;
            using VideoFrameConverter frameConverter = new();
            foreach (Frame sourceFrame in sourceFrames)
            {
                if (sourceFrame.Width > 0)
                {
                    (int width, int height) = sizeAccessor();
                    if (dest.Width != width || dest.Height != height)
                    {
                        dest.Dispose();
                        dest = Frame.CreateVideo(width, height, pixelFormat);
                    }

                    dest.MakeWritable();
                    frameConverter.ConvertFrame(sourceFrame, dest, swsFlags);
                    if (unref) sourceFrame.Unref();
                    dest.Pts = pts++;
                    destRef.Ref(dest);
                    yield return destRef;
                }
                else
                {
                    // bypass not a video frame
                    yield return sourceFrame;
                }
            }
        }
        finally
        {
            dest.Dispose();
        }
    }
}
