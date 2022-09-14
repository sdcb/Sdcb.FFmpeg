using Sdcb.FFmpeg.Raw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sdcb.FFmpeg.Formats;

unsafe internal class StreamMediaIO : IOContext
{
    private readonly object _callbackObject;

    public StreamMediaIO(AVIOContext* ptr, bool isOwner, object callbackObject) : base(ptr, isOwner)
    {
        _callbackObject = callbackObject;
    }

    protected override bool ReleaseHandle()
    {
        GC.KeepAlive(_callbackObject);
        return base.ReleaseHandle();
    }
}
