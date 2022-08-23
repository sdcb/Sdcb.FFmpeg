using Sdcb.FFmpeg.Raw;
using System;
using System.Runtime.InteropServices;
using static Sdcb.FFmpeg.Raw.ffmpeg;

namespace Sdcb.FFmpeg.Common
{
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

	internal static class FFmpegValidation
    {
		public static int ThrowIfError(this int errorCode, string? message = null)
        {
			if (errorCode < 0)
            {
				throw FFmpegException.FromErrorCode(errorCode, message);
            }
			return errorCode;
        }

		public static long ThrowIfError(this long errorCode, string? message = null)
		{
			if (errorCode < 0)
			{
				throw FFmpegException.FromErrorCode((int)errorCode, message);
			}
			return errorCode;
		}
	}
}
