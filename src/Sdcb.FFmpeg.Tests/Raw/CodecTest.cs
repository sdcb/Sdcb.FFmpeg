using Sdcb.FFmpeg.Raw;
using Xunit;

namespace Sdcb.FFmpeg.Tests.Raw;

public class CodecTest
{
    [Fact]
    public void Test_avcodec_version()
    {
        uint version = ffmpeg.avcodec_version();
        uint major = version >> 16;
        Assert.Equal(ffmpeg.LIBAVCODEC_VERSION_MAJOR, major);
    }

    [Fact]
    public void Test_av_version()
    {
        string version = ffmpeg.av_version_info();
        Assert.NotNull(version);
    }
}
