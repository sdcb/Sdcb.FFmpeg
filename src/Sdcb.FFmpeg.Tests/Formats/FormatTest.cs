using Sdcb.FFmpeg.Formats;
using Xunit;
using Xunit.Abstractions;

namespace Sdcb.FFmpeg.Tests.Formats;

public class FormatTest
{
    private readonly ITestOutputHelper _console;

    public FormatTest(ITestOutputHelper console)
    {
        _console = console;
    }

    [Fact]
    public void InputFormatTest()
    {
        foreach (InputFormat fmt in InputFormat.All)
        {
            _console.WriteLine(fmt.Name);
        }
    }

    [Fact]
    public void OutputFormatTest()
    {
        foreach (OutputFormat fmt in OutputFormat.All)
        {
            _console.WriteLine(fmt.Name);
        }
    }
}
