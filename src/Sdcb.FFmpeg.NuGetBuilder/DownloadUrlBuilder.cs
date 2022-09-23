using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sdcb.FFmpeg.NuGetBuilder
{
    public static class DownloadUrlBuilder
    {
        public const string DownloadVersion = "4.4.1";

        public const string Url = $"https://github.com/GyanD/codexffmpeg/releases/download/{DownloadVersion}/ffmpeg-{DownloadVersion}-full_build-shared.zip";
    }
}
