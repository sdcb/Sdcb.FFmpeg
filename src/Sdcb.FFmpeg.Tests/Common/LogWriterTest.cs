using Sdcb.FFmpeg.Common;
using Xunit;
using Xunit.Abstractions;

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
            string lastLog = null;
            FFmpegLogger.LogWriter = log => lastLog = log;
            FFmpegLogger.LogError("hello");
            _console.WriteLine(lastLog);
        }
    }
}
