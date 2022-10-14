using Sdcb.FFmpeg.Raw;

namespace Sdcb.FFmpeg.Toolboxs.FilterTools;

public record AudioSinkParams(int SampleRate = -1, AVSampleFormat SampleFormat = AVSampleFormat.None, ulong ChannelLayout = unchecked((ulong)-1), int Channels = -1);