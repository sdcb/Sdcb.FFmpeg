using System;
using static Sdcb.FFmpeg.Raw.ffmpeg;

namespace Sdcb.FFmpeg.Common;

public class FFmpegException : Exception
{
	public FFmpegException(string message) : base(message)
	{
	}

	private FFmpegException(int errorCode, string message) : base(message)
	{
		HResult = errorCode;
	}

	public unsafe static FFmpegException FromErrorCode(int errorCode, string? info)
	{
		byte* buffer = stackalloc byte[AV_ERROR_MAX_STRING_SIZE];
		av_strerror(errorCode, buffer, AV_ERROR_MAX_STRING_SIZE);
		string prefix = info ?? $"FFmpeg error {errorCode}";
		string errorMessage = $"[{prefix}]: {((IntPtr)buffer).PtrToStringUTF8()}";
		return new FFmpegException(errorCode, errorMessage);
	}

	public unsafe static FFmpegException NoMemory(string message) => FromErrorCode(AVERROR(ENOMEM), message);
}
