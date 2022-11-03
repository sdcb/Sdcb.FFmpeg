using Sdcb.FFmpeg.Formats;

namespace Sdcb.FFmpeg.Toolboxs.Extensions
{
    public static class MediaStreamExtensions
    {
        /// <returns>(stream.TimeBase * stream.Duration).ToDouble()</returns>
        public static double GetDurationInSeconds(this MediaStream stream) => (stream.TimeBase * stream.Duration).ToDouble();
    }
}
