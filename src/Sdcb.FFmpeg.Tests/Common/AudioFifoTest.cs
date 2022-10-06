using Sdcb.FFmpeg.Utils;
using Xunit;
using Xunit.Abstractions;

namespace Sdcb.FFmpeg.Tests.Common
{
    public class AudioFifoTest
    {
        private readonly ITestOutputHelper _console;

        public AudioFifoTest(ITestOutputHelper console)
        {
            _console = console;
        }

        [Fact]
        public void CanAllocAndFree()
        {
            using AudioFifo af = new(FFmpeg.Raw.AVSampleFormat.Fltp, 2, 48000);
        }
    }
}
