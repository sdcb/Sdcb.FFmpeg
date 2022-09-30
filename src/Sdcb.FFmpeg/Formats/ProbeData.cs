using Sdcb.FFmpeg.Raw;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Sdcb.FFmpeg.Formats;

public record ProbeData(string? FileName = null, byte[]? Buffer = null, string? MimeType = null)
{
    public static ProbeData FromFile(string fileName) => new ProbeData(FileName: fileName);

    public static ProbeData FromBuffer(byte[] buffer) => new ProbeData(Buffer: buffer);

    public static ProbeData FromMimeType(string mimeType) => new ProbeData(MimeType: mimeType);

    public unsafe IDisposable FillNativeScoped(AVProbeData* ptr)
    {
        if (ptr == null) throw new ArgumentNullException(nameof(ptr));

        GCHandle? fileNameHandle = null;
        if (FileName != null)
        {
            byte[] fileNameBytes = Encoding.UTF8.GetBytes(FileName);
            fileNameHandle = GCHandle.Alloc(fileNameBytes, GCHandleType.Pinned);
            ptr->filename = (byte*)fileNameHandle.Value.AddrOfPinnedObject();
        }

        GCHandle? bufferHandle = null;
        if (Buffer != null)
        {
            byte[] padded = new byte[Buffer.Length + ffmpeg.AVPROBE_PADDING_SIZE];
            Buffer.CopyTo(padded, 0);
            bufferHandle = GCHandle.Alloc(padded, GCHandleType.Pinned);
            ptr->buf = (byte*)bufferHandle.Value.AddrOfPinnedObject();
            ptr->buf_size = Buffer.Length;
        }

        GCHandle? mimeTypeHandle = null;
        if (MimeType != null)
        {
            byte[] mimeTypeBytes = Encoding.UTF8.GetBytes(MimeType);
            mimeTypeHandle = GCHandle.Alloc(mimeTypeBytes, GCHandleType.Pinned);
            ptr->mime_type = (byte*)mimeTypeHandle.Value.AddrOfPinnedObject();
        }

        return new DisposableWrapper(() =>
        {
            fileNameHandle?.Free();
            bufferHandle?.Free();
            mimeTypeHandle?.Free();
        });
    }
}

internal record DisposableWrapper(Action DisposeAction) : IDisposable
{
    public void Dispose() => DisposeAction();
}
