using Sdcb.FFmpeg.Common;
using Sdcb.FFmpeg.Raw;
using Sdcb.FFmpeg.Utils;
using System;
using System.Buffers;
using System.Runtime.InteropServices;
using System.Text;
using static Sdcb.FFmpeg.Raw.ffmpeg;

namespace Sdcb.FFmpeg.Formats;

public partial class IOContext : SafeHandle
{
    public unsafe FFmpegOptions Options => new FFmpegOptions(this);

    /// <summary>
    /// <para>Similar to feof() but also returns nonzero on read errors.</para>
    /// <para>@return non zero if and only if at end of file or a read error happened when reading.</para>
    /// <see cref="avio_feof(AVIOContext*)"/>
    /// </summary>
    public unsafe int EOF => avio_feof(this);

    /// <summary>
    /// <see cref="avio_size(AVIOContext*)"/>
    /// </summary>
    public unsafe long Size => avio_size(this).ThrowIfError();

    /// <summary>
    /// <see cref="avio_read(AVIOContext*, byte*, int)"/>
    /// </summary>
    public unsafe int Read(Span<byte> data)
    {
        fixed (byte* pin = data)
        {
            return avio_read(this, pin, data.Length).ThrowIfError();
        }
    }

    /// <summary>
    /// <see cref="avio_read_partial(AVIOContext*, byte*, int)"/>
    /// </summary>
    public unsafe int ReadPartial(Span<byte> data)
    {
        fixed (byte* p = data)
        {
            return avio_read_partial(this, p, data.Length).ThrowIfError();
        }
    }

    /// <summary>
    /// <see cref="avio_r8(AVIOContext*)"/>
    /// </summary>
    /// <returns>return 0 if EOF, so you cannot use it if EOF handling is necessary</returns>
    public unsafe byte ReadByte() => (byte)avio_r8(this);

    /// <summary>
    /// <see cref="avio_rl16(AVIOContext*)"/>
    /// </summary>
    /// <returns>return 0 if EOF, so you cannot use it if EOF handling is necessary</returns>
    public unsafe ushort ReadUInt16LittleEndian() => (ushort)avio_rl16(this);

    /// <summary>
    /// <see cref="avio_rl24(AVIOContext*)"/>
    /// </summary>
    /// <returns>return 0 if EOF, so you cannot use it if EOF handling is necessary</returns>
    public unsafe uint ReadUInt24LittleEndian() => avio_rl24(this);

    /// <summary>
    /// <see cref="avio_rl32(AVIOContext*)"/>
    /// </summary>
    /// <returns>return 0 if EOF, so you cannot use it if EOF handling is necessary</returns>
    public unsafe uint ReadUInt32LittleEndian() => avio_rl32(this);

    /// <summary>
    /// <see cref="avio_rl64(AVIOContext*)"/>
    /// </summary>
    /// <returns>return 0 if EOF, so you cannot use it if EOF handling is necessary</returns>
    public unsafe ulong ReadUInt64LittleEndian() => avio_rl64(this);

    /// <summary>
    /// <see cref="avio_rb16(AVIOContext*)"/>
    /// </summary>
    /// <returns>return 0 if EOF, so you cannot use it if EOF handling is necessary</returns>
    public unsafe ushort ReadUInt16BigEndian() => (ushort)avio_rb16(this);

    /// <summary>
    /// <see cref="avio_rb24(AVIOContext*)"/>
    /// </summary>
    /// <returns>return 0 if EOF, so you cannot use it if EOF handling is necessary</returns>
    public unsafe uint ReadUInt24BigEndian() => avio_rb24(this);

    /// <summary>
    /// <see cref="avio_rb32(AVIOContext*)"/>
    /// </summary>
    /// <returns>return 0 if EOF, so you cannot use it if EOF handling is necessary</returns>
    public unsafe uint ReadUInt32BigEndian() => avio_rb32(this);

    /// <summary>
    /// <see cref="avio_rb64(AVIOContext*)"/>
    /// </summary>
    /// <returns>return 0 if EOF, so you cannot use it if EOF handling is necessary</returns>
    public unsafe ulong ReadUInt64BigEndian() => avio_rb64(this);

    /// <summary>
    /// <see cref="avio_get_str(AVIOContext*, int, byte*, int)"/>
    /// </summary>
    public unsafe int ReadString(Span<byte> data, int maxLength)
    {
        fixed (byte* p = data)
        {
            return avio_get_str(this, maxLength, p, data.Length).ThrowIfError();
        }
    }

    /// <summary>
    /// <see cref="avio_get_str(AVIOContext*, int, byte*, int)"/>
    /// </summary>
    public unsafe string ReadString(int maxLength = 4096)
    {
        byte[] data = ArrayPool<byte>.Shared.Rent(maxLength);
        try
        {
            fixed (byte* p = data)
            {
                int c = avio_get_str(this, data.Length, p, data.Length).ThrowIfError();
                return Encoding.UTF8.GetString(data, 0, c - 1);
            }
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(data);
        }
    }

    /// <summary>
    /// <para>Read a UTF-16 string and convert it to UTF-8.</para>
    /// <see cref="avio_get_str16le(AVIOContext*, int, byte*, int)"/>
    /// </summary>
    public unsafe int ReadStringUtf16LE(Span<byte> data, int maxLength)
    {
        fixed (byte* p = data)
        {
            return avio_get_str16le(this, maxLength, p, data.Length).ThrowIfError();
        }
    }

    /// <summary>
    /// <para>Read a UTF-16 string and convert it to UTF-8.</para>
    /// <see cref="avio_get_str16le(AVIOContext*, int, byte*, int)"/>
    /// </summary>
    public unsafe int ReadStringUtf16BE(Span<byte> data, int maxLength)
    {
        fixed (byte* p = data)
        {
            return avio_get_str16be(this, maxLength, p, data.Length).ThrowIfError();
        }
    }

    /// <summary>
    /// <see cref="avio_write(AVIOContext*, byte*, int)"/>
    /// </summary>
    public unsafe void Write(ReadOnlySpan<byte> data)
    {
        fixed (byte* pin = data)
        {
            avio_write(this, pin, data.Length);
        }
    }

    /// <summary>
    /// <see cref="avio_put_str(AVIOContext*, string)"/>
    /// </summary>
    /// <param name="text">NULL-terminated string.</param>
    /// <returns>number of bytes written.</returns>
    public unsafe int WriteString(string text) => avio_put_str(this, text);

    /// <summary>
    /// Convert an string to UTF-16LE and write it.
    /// <see cref="avio_put_str16le(AVIOContext*, string)"/>
    /// </summary>
    /// <param name="text">NULL-terminated UTF-8 string</param>
    /// <returns>number of bytes written.</returns>
    public unsafe int WriteStringUtf16LE(string text) => avio_put_str16le(this, text);

    /// <summary>
    /// Convert an string to UTF-16BE and write it.
    /// <see cref="avio_put_str16be(AVIOContext*, string)"/>
    /// </summary>
    /// <param name="text">NULL-terminated UTF-8 string</param>
    /// <returns>number of bytes written.</returns>
    public unsafe int WriteStringUtf16BE(string text) => avio_put_str16be(this, text);

    /// <summary>
    /// Mark the written bytestream as a specific type.
    /// <para>Zero-length ranges are omitted from the output.</para>
    /// <see cref="avio_write_marker(AVIOContext*, long, AVIODataMarkerType)"/>
    /// </summary>
    /// <param name="time">the stream time the current bytestream pos corresponds to 
    /// (in AV_TIME_BASE units), or AV_NOPTS_VALUE if unknown or not applicable</param>
    /// <param name="type">the kind of data written starting at the current pos</param>
    public unsafe void WriteMarker(long time, AVIODataMarkerType type) => avio_write_marker(this, time, type);

    /// <summary>
    /// <see cref="avio_flush(AVIOContext*)"/>
    /// </summary>
    public unsafe void Flush() => avio_flush(this);

    /// <summary>
    /// <see cref="avio_seek(AVIOContext*, long, int)"/>
    /// </summary>
    public unsafe long Seek(long offset, AVSEEK origin)
    {
        return avio_seek(this, offset, (int)origin).ThrowIfError();
    }

    /// <summary>
    /// <para>stream_index: -1</para>
    /// <see cref="avio_seek_time(AVIOContext*, int, long, int)"/>
    /// </summary>
    public unsafe long SeekTime(long timestamp, int flags) => avio_seek_time(this, -1, timestamp, flags);

    /// <summary>
    /// <see cref="avio_seek_time(AVIOContext*, int, long, int)"/>
    /// </summary>
    public unsafe long SeekStreamTime(int streamIndex, long timestamp, int flags) => avio_seek_time(this, streamIndex, timestamp, flags).ThrowIfError();

    /// <summary>
    /// Skip given number of bytes forward
    /// </summary>
    /// <param name="offset"></param>
    /// <returns>new position or AVERROR.</returns>
    public unsafe long Skip(long offset) => avio_skip(this, offset).ThrowIfError();

    /// <summary>
    /// <see cref="avio_pause(AVIOContext*, int)"/>
    /// </summary>
    public unsafe void Pause() => avio_pause(this, 1).ThrowIfError();

    /// <summary>
    /// <see cref="avio_pause(AVIOContext*, int)"/>
    /// </summary>
    public unsafe void Resume() => avio_pause(this, 0).ThrowIfError();

    /// <summary>
    /// <see cref="avio_accept(AVIOContext*, AVIOContext**)"/>
    /// </summary>
    public unsafe IOContext Accept()
    {
        AVIOContext* ctx;
        avio_accept(this, &ctx).ThrowIfError();
        return FromNative(ctx, isOwner: true);
    }

    /// <summary>
    /// <see cref="avio_handshake(AVIOContext*)"/>
    /// </summary>
    public unsafe int Handshake() => avio_handshake(this).ThrowIfError();

    /// <summary>
    /// <see cref="avio_close(AVIOContext*)"/>
    /// </summary>
    public unsafe void Close()
    {
        avio_close((AVIOContext*)handle).ThrowIfError();
        handle = IntPtr.Zero;
        SetHandleAsInvalid();
    }

    protected override bool ReleaseHandle()
    {
        Close();
        return true;
    }
}
