using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sdcb.FFmpeg.Codecs;

public enum CodecResult
{
    Success = 0,
    Again = 1,
    EOF = 2,
}
