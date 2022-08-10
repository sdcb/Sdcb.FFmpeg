using Sdcb.FFmpeg.Common;
using Xunit;
using Xunit.Abstractions;
using static Sdcb.FFmpeg.Raw.ffmpeg;

namespace Sdcb.FFmpeg.Tests.Common
{
    public class LogWriterTest
    {
        private readonly ITestOutputHelper _console;

        public LogWriterTest(ITestOutputHelper console)
        {
            _console = console;
        }

        [Fact]
        public void WriteLog()
        {
            string lastLog = default;
            LogLevel logLevel = default;
            FFmpegLogger.LogWriter = (level, log) => (logLevel, lastLog) = (level, log);
            FFmpegLogger.LogError("hello");
            _console.WriteLine($"{logLevel}: {lastLog}");
        }
    }
}
