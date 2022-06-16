using System;
using System.Runtime.InteropServices;

#pragma warning disable 169
#pragma warning disable CS0649
#pragma warning disable CS0108
namespace Sdcb.FFmpeg.Raw
{
    using System.Collections.Generic;
    
    public unsafe static partial class ffmpeg
    {
        public static Dictionary<string, int> LibraryVersionMap =  new ()
        {
            ["avcodec"] = 59,
            ["avdevice"] = 59,
            ["avfilter"] = 8,
            ["avformat"] = 59,
            ["avutil"] = 57,
            ["postproc"] = 56,
            ["swresample"] = 4,
            ["swscale"] = 6,
        };
    }
}
