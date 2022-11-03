using Sdcb.FFmpeg.Raw;

namespace Sdcb.FFmpeg.Toolboxs.FilterTools;

public record AudioSinkParams(AVChannelLayout ChLayout, int SampleRate = -1, AVSampleFormat SampleFormat = AVSampleFormat.None);