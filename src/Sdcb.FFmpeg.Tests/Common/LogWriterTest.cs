using Sdcb.FFmpeg.Utils;
using Sdcb.FFmpeg.Raw;
using Xunit;
using Xunit.Abstractions;

namespace Sdcb.FFmpeg.Tests.Utils;

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
        string? lastLog = default;
        LogLevel logLevel = default;
        FFmpegLogger.LogWriter = (level, log) => (logLevel, lastLog) = (level, log);
        FFmpegLogger.LogError("hello");
        _console.WriteLine($"{logLevel}: {lastLog}");
    }
}
