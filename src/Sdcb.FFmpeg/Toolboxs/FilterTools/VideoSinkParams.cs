using Sdcb.FFmpeg.Raw;

namespace Sdcb.FFmpeg.Toolboxs.FilterTools;

public record VideoSinkParams(int Width, int Height, AVRational Timebase, AVRational Framerate, AVPixelFormat PixelFormat);