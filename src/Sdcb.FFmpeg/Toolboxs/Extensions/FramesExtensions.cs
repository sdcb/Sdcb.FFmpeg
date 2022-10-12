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
        using FilterGraph graph = new();
        using FilterContext srcCtx = graph.AllocFilter("buffer", "in");
        using FilterContext sinkCtx = graph.CreateFilter("buffersink", "out");
        bool initialized = false;

        using Frame destFrame = new();
        foreach (Frame srcFrame in srcFrames)
        {
            if (srcFrame.Width > 0)
            {
                if (!initialized)
                {
                    // video
                    srcCtx.InitializeFromDictionary(new MediaDictionary
                    {
                        ["width"] = srcFrame.Width.ToString(),
                        ["height"] = srcFrame.Height.ToString(),
                        ["pix_fmt"] = NameUtils.GetPixelFormatName((AVPixelFormat)srcFrame.Format),
                        ["time_base"] = srcTimebase.ToString(),
                        ["sar"] = srcFrame.SampleAspectRatio.ToString(),
                    });
                    sinkCtx.Options.Set("pix_fmts", new int[] { (int)destPixelFormat }, AV_OPT_SEARCH.Children);
                    graph.ParsePtr(filterText, new FilterInOut("out", sinkCtx), new FilterInOut("in", srcCtx));
                    graph.Configure();
                    initialized = true;
                }

                foreach (Frame frame in WriteFilterFrame(srcCtx, sinkCtx, destFrame, srcFrame))
                {
                    yield return frame;
                }
            }
            else
            {
                yield return srcFrame;
            }
        }

        foreach (Frame frame in WriteFilterFrame(srcCtx, sinkCtx, destFrame, null))
        {
            yield return frame;
        }
    }

    public static IEnumerable<Frame> ApplyVideoFilters(this IEnumerable<Frame> srcFrames, MediaStream sourceStream, AVPixelFormat destPixelFormat, string filterText)
    {
        if (sourceStream.Codecpar == null)
        {
            throw new ArgumentNullException($"{nameof(sourceStream)}.{nameof(sourceStream.Codecpar)} should not be null.");
        }
        CodecParameters codecpar = sourceStream.Codecpar;
        if (codecpar.Width == 0)
        {
            throw new InvalidOperationException($"{nameof(sourceStream)} is not a video.");
        }

        using FilterGraph graph = new();
        using FilterContext srcCtx = graph.CreateFilter("buffer", "in", new MediaDictionary
        {
            ["width"] = codecpar.Width.ToString(),
            ["height"] = codecpar.Height.ToString(),
            ["pix_fmt"] = NameUtils.GetPixelFormatName((AVPixelFormat)codecpar.Format),
            ["time_base"] = sourceStream.TimeBase.ToString(),
            ["sar"] = codecpar.SampleAspectRatio.ToString(),
        });
        using FilterContext sinkCtx = graph.CreateFilter("buffersink", "out");
        sinkCtx.Options.Set("pix_fmts", new int[] { (int)destPixelFormat }, AV_OPT_SEARCH.Children);
        graph.ParsePtr(filterText, new FilterInOut("out", sinkCtx), new FilterInOut("in", srcCtx));
        graph.Configure();

        using Frame destFrame = new();
        foreach (Frame srcFrame in srcFrames)
        {
            if (srcFrame.Width > 0)
            {
                foreach (Frame frame in WriteFilterFrame(srcCtx, sinkCtx, destFrame, srcFrame))
                {
                    yield return frame;
                }
            }
            else
            {
                yield return srcFrame;
            }
        }

        foreach (Frame frame in WriteFilterFrame(srcCtx, sinkCtx, destFrame, null))
        {
            yield return frame;
        }
    }

    private static IEnumerable<Frame> WriteFilterFrame(FilterContext srcCtx, FilterContext sinkCtx, Frame destFrame, Frame? srcFrame)
    {
        srcCtx.WriteFrame(srcFrame);

        while (true)
        {
            CodecResult r = CodecContext.ToCodecResult(sinkCtx.GetFrame(destFrame));
            if (r == CodecResult.Again || r == CodecResult.EOF) break;

            if (r == CodecResult.Success)
            {
                yield return destFrame;
                destFrame.Unreference();
            }
        }
    }

    public static IEnumerable<Frame> ApplyAudioFilters(this IEnumerable<Frame> srcFrames, AVRational srcTimebase, AVPixelFormat destPixelFormat, string filterText)
    {
        using FilterGraph graph = new();
        using FilterContext srcCtx = graph.AllocFilter("abuffer", "in");
        using FilterContext sinkCtx = graph.CreateFilter("abuffersink", "out");
        bool initialized = false;

        using Frame destFrame = new();
        foreach (Frame srcFrame in srcFrames)
        {
            if (srcFrame.SampleRate > 0)
            {
                if (!initialized)
                {
                    // audio
                    srcCtx.InitializeFromDictionary(new MediaDictionary
                    {
                        ["time_base"] = srcTimebase.ToString(),
                        ["sample_rate"] = srcFrame.SampleRate.ToString(),
                        ["sample_fmt"] = NameUtils.GetSampleFormatName((AVSampleFormat)srcFrame.Format),
                        ["channel_layout"] = NameUtils.GetChannelLayoutString(srcFrame.ChannelLayout, srcFrame.Channels),
                        ["channels"] = srcFrame.Channels.ToString(),
                    });
                    sinkCtx.Options.Set("pix_fmts", new int[] { (int)destPixelFormat }, AV_OPT_SEARCH.Children);
                    graph.ParsePtr(filterText, new FilterInOut("out", sinkCtx), new FilterInOut("in", srcCtx));
                    graph.Configure();
                    initialized = true;
                }

                srcCtx.WriteFrame(srcFrame);

                foreach (Frame frame in WriteFilterFrame(srcCtx, sinkCtx, destFrame, null))
                {
                    yield return frame;
                }
            }
            else
            {
                yield return srcFrame;
            }
        }

        foreach (Frame frame in WriteFilterFrame(srcCtx, sinkCtx, destFrame, null))
        {
            yield return frame;
        }
    }

    public static IEnumerable<Frame> ApplyAudioFilters(this IEnumerable<Frame> srcFrames, MediaStream sourceStream, AVPixelFormat destPixelFormat, string filterText)
    {
        if (sourceStream.Codecpar == null)
        {
            throw new ArgumentNullException($"{nameof(sourceStream)}.{nameof(sourceStream.Codecpar)} should not be null.");
        }
        CodecParameters codecpar = sourceStream.Codecpar;
        if (codecpar.SampleRate == 0)
        {
            throw new InvalidOperationException($"{nameof(sourceStream)} is not a audio.");
        }

        using FilterGraph graph = new();
        using FilterContext srcCtx = graph.CreateFilter("abuffer", "in", new MediaDictionary
        {
            ["time_base"] = sourceStream.TimeBase.ToString(),
            ["sample_rate"] = codecpar.SampleRate.ToString(),
            ["sample_fmt"] = NameUtils.GetSampleFormatName((AVSampleFormat)codecpar.Format),
            ["channel_layout"] = NameUtils.GetChannelLayoutString(codecpar.ChannelLayout, codecpar.Channels),
            ["channels"] = codecpar.Channels.ToString(),
        });
        using FilterContext sinkCtx = graph.CreateFilter("abuffersink", "out");

        using Frame destFrame = new();
        foreach (Frame srcFrame in srcFrames)
        {
            if (srcFrame.SampleRate > 0)
            {
                srcCtx.WriteFrame(srcFrame);

                foreach (Frame frame in WriteFilterFrame(srcCtx, sinkCtx, destFrame, null))
                {
                    yield return frame;
                }
            }
            else
            {
                yield return srcFrame;
            }
        }

        foreach (Frame frame in WriteFilterFrame(srcCtx, sinkCtx, destFrame, null))
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
        CodecContext? audioCodec = null,
        CodecContext? videoCodec = null,
        bool makeWritable = true)
    {
        using Packet packet = new();
        int audioPts = 0, videoPts = 0;
        (MediaStream stream, CodecContext c)? audio = fc.FindBestStreamOrNull(AVMediaType.Audio) switch
        {
            null => null,
            var x => (x.Value, audioCodec ?? throw new ArgumentNullException(nameof(audioCodec)))
        };
        (MediaStream stream, CodecContext c)? video = fc.FindBestStreamOrNull(AVMediaType.Video) switch
        {
            null => null,
            var x => (x.Value, videoCodec ?? throw new ArgumentNullException(nameof(videoCodec)))
        };

        foreach (Frame frame in frames)
        {
            (MediaStream stream, CodecContext c) = GetCodecContext(frame, audio, video);

            if (frame.Width > 0)
            {
                frame.Pts = videoPts++;
            }
            else if (frame.SampleRate > 0)
            {
                frame.Pts = audioPts;
                audioPts += frame.NbSamples;
            }


            foreach (var _ in c.EncodeFrame(frame, packet))
            {
                packet.RescaleTimestamp(c.TimeBase, stream.TimeBase);
                packet.StreamIndex = stream.Index;
                yield return packet;
            }

            if (makeWritable) frame.MakeWritable();
        }

        bool encodeVideo = video != null;
        bool encodeAudio = audio != null;
        while (encodeVideo || encodeAudio)
        {
            if (encodeVideo && (!encodeAudio || av_compare_ts(videoPts, videoCodec!.TimeBase, audioPts, audioCodec!.TimeBase) <= 0))
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

        static (MediaStream, CodecContext) GetCodecContext(Frame frame,
            (MediaStream stream, CodecContext codec)? audio,
            (MediaStream stream, CodecContext codec)? video)
        {
            return (frame.Width > 0) switch
            {
                true => video == null ? throw new ArgumentNullException(nameof(video)) : video.Value,
                false => audio == null ? throw new ArgumentNullException(nameof(audio)) : audio.Value,
            };
        }
    }
}
