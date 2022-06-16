using FluentAssertions;
using NUnit.Framework;
using Sdcb.FFmpeg.Raw;

namespace Sdcb.FFmpeg.Tests.Raw
{
    [TestFixture]
    public class CodecTest
    {
        [Test]
        public void Test_avcodec_version()
        {
            uint version = ffmpeg.avcodec_version();
            uint major = version >> 16;
            major.Should().Be(59);
        }

        [Test]
        public void Test_av_version()
        {
            string version = ffmpeg.av_version_info();
            version.Should().NotBeNull();
        }
    }
}
