using Sdcb.FFmpeg.Codecs;
using Sdcb.FFmpeg.Filters;
using Sdcb.FFmpeg.Raw;
using Sdcb.FFmpeg.Utils;
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
        Filter? formatFilter = Filter.GetByNameOrNull("format");
        Assert.NotNull(formatFilter);
    }

    [Fact]
    public void FilterOutputs()
    {
        Filter formatFilter = Filter.GetByName("format");
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

    [Fact]
    public void CheckFilterSink()
    {
        using FilterGraph graph = new();
        using FilterContext srcCtx = graph.AllocFilter("buffer", "in");
        using FilterContext sinkCtx = graph.CreateFilter("buffersink", "out");
        int width = 800, height = 600;
        AVPixelFormat pixelFormat = AVPixelFormat.Yuv422p;
        AVRational timebase = new(1, 25), sar = new(1, 1);
        srcCtx.InitializeFromDictionary(new MediaDictionary
        {
            ["width"] = width.ToString(),
            ["height"] = height.ToString(),
            ["pix_fmt"] = NameUtils.GetPixelFormatName(pixelFormat),
            ["time_base"] = timebase.ToString(),
            ["sar"] = new AVRational(1, 1).ToString(),
        });
        sinkCtx.Options.Set("pix_fmts", new int[] { (int)AVPixelFormat.Yuv444p }, AV_OPT_SEARCH.Children);
        graph.ParsePtr("scale=4:-1,fps=15", new FilterInOut("out", sinkCtx), new FilterInOut("in", srcCtx));
        graph.Configure();

        Assert.Equal(3, sinkCtx.Inputs[0].H);
        Assert.Equal(4, sinkCtx.Inputs[0].W);
        Assert.Equal(new AVRational(1, 15), sinkCtx.Inputs[0].TimeBase);
        Assert.Equal(AVPixelFormat.Yuv444p, (AVPixelFormat)sinkCtx.Inputs[0].Format);
        Assert.Equal(sar, sinkCtx.Inputs[0].SampleAspectRatio);
    }
}
