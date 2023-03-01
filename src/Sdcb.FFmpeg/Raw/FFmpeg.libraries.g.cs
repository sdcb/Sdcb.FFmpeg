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
            ["avcodec"] = 60,
            ["avdevice"] = 60,
            ["avfilter"] = 9,
            ["avformat"] = 60,
            ["avutil"] = 58,
            ["postproc"] = 57,
            ["swresample"] = 4,
            ["swscale"] = 7,
        };
    }
}
