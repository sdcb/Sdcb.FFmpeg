using Sdcb.FFmpeg.Codecs;
using Sdcb.FFmpeg.Filters;
using Sdcb.FFmpeg.Raw;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Sdcb.FFmpeg.Tests.Filters;

public class FiltersTest
{
    private readonly ITestOutputHelper _console;

    public FiltersTest(ITestOutputHelper console)
    {
        _console = console;
    }

    [Fact]
    public void Version()
    {
        Assert.Equal(ffmpeg.LIBAVFILTER_VERSION_INT, Filter.Version);
    }

    [Fact]
    public void ListAll()
    {
        _console.WriteLine(string.Join("\n", Filter.All.Select(x => x.Name)));
    }
}
