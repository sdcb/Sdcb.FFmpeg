namespace Sdcb.FFmpeg.Common;

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
