using Sdcb.FFmpeg.Common;
using Sdcb.FFmpeg.Raw;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static Sdcb.FFmpeg.Raw.ffmpeg;

namespace Sdcb.FFmpeg.Formats;

public partial class IOContext
{
    /// <summary>
    /// <see cref="avio_open2(AVIOContext**, string, int, AVIOInterruptCB*, AVDictionary**)"/>
    /// </summary>
    public static unsafe IOContext Open(string url, IOFlags flags = IOFlags.Read, MediaDictionary? options = null)
    {
        AVIOContext* ctx = null;
        AVDictionary* dictPtr = options;
        avio_open2(&ctx, url, (int)flags, null, &dictPtr).ThrowIfError();
        options.Reset(dictPtr);

        return new IOContext(ctx, isOwner: true);
    }

    public static unsafe IOContext OpenRead(string url, MediaDictionary? options = null) => Open(url, IOFlags.Read, options);
    public static unsafe IOContext OpenWrite(string url, MediaDictionary? options = null) => Open(url, IOFlags.Write, options);

    /// <summary>
    /// <see cref="avio_open_dyn_buf(AVIOContext**)"/>
    /// </summary>
    public static unsafe DynamicIOContext OpenDynamic()
    {
        AVIOContext* ctx = null;
        avio_open_dyn_buf(&ctx).ThrowIfError();
        return new DynamicIOContext(ctx, isOwner: true);
    }

    /// <summary>
    /// <see cref="avio_alloc_context(byte*, int, int, void*, avio_alloc_context_read_packet_func, avio_alloc_context_write_packet_func, avio_alloc_context_seek_func)"/>
    /// </summary>
    public static IOContext ReadStream(Stream stream, int bufferSize = 32768) => FromStream(stream, 0, bufferSize);

    /// <summary>
    /// <see cref="avio_alloc_context(byte*, int, int, void*, avio_alloc_context_read_packet_func, avio_alloc_context_write_packet_func, avio_alloc_context_seek_func)"/>
    /// </summary>
    public static IOContext WriteStream(Stream stream, int bufferSize = 32768) => FromStream(stream, 1, bufferSize);

    private static unsafe IOContext FromStream(Stream stream, int writeFlag, int bufferSize = 32768)
    {
        byte* buffer = (byte*)av_malloc((ulong)bufferSize);
        if (buffer == null)
        {
            throw FFmpegException.NoMemory("Failed to alloc MediaIO buffer");
        }
        var callbackObject = new
        {
            ReadPacket = new avio_alloc_context_read_packet(Read),
            WritePacket = new avio_alloc_context_write_packet(Write),
            Seek = new avio_alloc_context_seek(Seek)
        };
        AVIOContext* ctx = avio_alloc_context(buffer, bufferSize, writeFlag,
            opaque: null,
            read_packet: callbackObject.ReadPacket,
            write_packet: callbackObject.WritePacket,
            seek: callbackObject.Seek);
        if (ctx == null)
        {
            throw FFmpegException.NoMemory("Failed to alloc AVIOContext");
        }

        return new StreamMediaIO(ctx, isOwner: true, callbackObject);

        int Read(void* opaque, byte* buffer, int length)
        {
            int c = stream.Read(new Span<byte>(buffer, length));
            return c == 0 ? AVERROR_EOF : c;
        }
        int Write(void* opaque, byte* buffer, int length)
        {
            stream.Write(new Span<byte>(buffer, length));
            return length;
        }
        long Seek(void* opaque, long position, int origin) => (MediaIOSeek)origin switch
        {
            MediaIOSeek.Begin => stream.Seek(position, SeekOrigin.Begin),
            MediaIOSeek.Current => stream.Seek(position, SeekOrigin.Current),
            MediaIOSeek.End => stream.Seek(position, SeekOrigin.End),
            MediaIOSeek.Size => stream.Length,
            _ => throw new NotSupportedException(),
        };
    }

    /// <summary>
    /// <see cref="avio_find_protocol_name(string)"/>
    /// </summary>
    public static string GetUrlProtocol(string url) => avio_find_protocol_name(url);

    /// <summary>
    /// <see cref="avio_check(string, int)"/>
    /// </summary>
    public static IOFlags Check(string url, IOFlags flags) => (IOFlags)avio_check(url, (int)flags).ThrowIfError();

    /// <summary>
    /// <see cref="avio_enum_protocols(void**, int)"/>
    /// </summary>
    public static IEnumerable<string> InputProtocols => EnumerateProtocols(output: 0);

    /// <summary>
    /// <see cref="avio_enum_protocols(void**, int)"/>
    /// </summary>
    public static IEnumerable<string> OutputProtocols => EnumerateProtocols(output: 1);

    private static IEnumerable<string> EnumerateProtocols(int output)
    {
        IntPtr opaque = AllocOpaque();
        try
        {
            while (true)
            {
                string result = avio_enum_protocols_internal(opaque, output);
                if (result == null) break;
                yield return result;
            }
        }
        finally
        {
            Marshal.FreeHGlobal(opaque);
        }

        unsafe static IntPtr AllocOpaque()
        {
            IntPtr address = Marshal.AllocHGlobal(IntPtr.Size);
            (*(IntPtr*)(address)) = new IntPtr(0);
            return address;
        }
    }

    private unsafe static string avio_enum_protocols_internal(IntPtr opaque, int output)
    {
        return avio_enum_protocols((void**)opaque, output);
    }

    public static unsafe FFmpegClass? GetProtocolClass(string protocol) => FFmpegClass.FromNativeOrNull(avio_protocol_get_class(protocol));
}
