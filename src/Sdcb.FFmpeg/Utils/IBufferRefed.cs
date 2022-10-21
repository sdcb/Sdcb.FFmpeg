using System;

namespace Sdcb.FFmpeg.Utils;

public interface IBufferRefed : IDisposable
{
    public void Ref(IBufferRefed other);
    public void Unref();
    public void MakeWritable();
    public IBufferRefed Clone();
}
