using Xunit.Abstractions;
using Xunit;
using Sdcb.FFmpeg.Raw;

namespace Sdcb.FFmpeg.Tests.Common;

public class AVRationalTest
{
    private readonly ITestOutputHelper _console;

    public AVRationalTest(ITestOutputHelper console)
    {
        _console = console;
    }

    [Fact]
    public void FromDoubleTest()
    {
        AVRational r = AVRational.FromDouble(1.5, 10);
        Assert.True(new AVRational(3, 2) == r);
    }

    [Fact]
    public void PlusTest()
    {
        AVRational r1 = new AVRational(2);
        AVRational r2 = new AVRational(1, 3);
        Assert.True(new AVRational(7, 3) == r1 + r2);
    }

    [Fact]
    public void ReduceTest()
    {
        AVRational r1 = new AVRational(-4, 8);
        Assert.True(new AVRational(-1, 2) == r1.Reduce());
    }

    [Fact]
    public void InverseTest()
    {
        AVRational r1 = new (-2, 3);
        Assert.Equal(new AVRational(3, -2), r1.Inverse());
    }
}
