using FluentAssertions;
using NUnit.Framework;
using Sdcb.FFmpeg.Raw;

namespace Sdcb.FFmpeg.Tests
{
    [TestFixture]
    public class BasicTests
    {
        /// <summary>
        /// version.h
        /// </summary>
        [Test]
        public void Test_avcodec_version()
        {
            var version = ffmpeg.avcodec_version();
            var major = version >> 16;
            major.Should().Be(59);
        }
    }
}
