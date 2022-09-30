using Sdcb.FFmpeg.Codecs;
using Sdcb.FFmpeg.Raw;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Sdcb.FFmpeg.Tests.Codecs;

public class CodecTest
{
    private readonly ITestOutputHelper _console;

    public CodecTest(ITestOutputHelper console)
    {
        _console = console;
    }

    [Fact]
    public void ListAll()
    {
        _console.WriteLine(string.Join("\n", Codec.Encoders.Select(x => x.Name)));
    }

    [Fact]
    public void EncoderExistsLibx264()
    {
        Codec? c = Codec.FindEncoderByName("libx264");
        Assert.NotNull(c);
    }

    [Fact]
    public void EncoderNotExistsAbcXyz()
    {
        Codec? c = Codec.FindEncoderByName("abc-xyz");
        Assert.Null(c);
    }

    [Fact]
    public void DecoderNotExistsAbcXyz()
    {
        Codec? c = Codec.FindDecoderByName("abc-xyz");
        Assert.Null(c);
    }

    [Fact]
    public void DecoderExistsHevc()
    {
        Codec? c = Codec.FindDecoderByName("hevc");
        Assert.NotNull(c);
    }

    [Fact]
    public void FindEncoderByIdWithHevc()
    {
        Codec? c = Codec.FindEncoderById(AVCodecID.Hevc);
        Assert.NotNull(c);
    }

    [Fact]
    public void LicenseShouldNotNull()
    {
        string license = Codec.License;
        Assert.NotNull(license);
    }
}
