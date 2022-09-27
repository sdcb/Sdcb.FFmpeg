using Sdcb.FFmpeg.Codecs;
using Sdcb.FFmpeg.Filters;
using Sdcb.FFmpeg.Raw;
using System.Collections.Generic;
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

    [Fact]
    public void GetByName()
    {
        Filter? formatFilter = Filter.GetByName("format");
        Assert.NotNull(formatFilter);
    }

    [Fact]
    public  void FilterOutputs()
    {
        Filter formatFilter = Filter.GetByName("format").Value;
        _console.WriteLine(string.Join("\n", formatFilter.Outputs.Select(x => $"{x.MediaType}: {x.Name}").Take(2)));
    }

    [Fact]
    public void Configuration()
    {
        HashSet<string> configs = Filter.Configuration;
        Assert.NotEmpty(configs);
    }

    [Fact]
    public void License()
    {
        Assert.NotNull(Filter.License);
    }
}
