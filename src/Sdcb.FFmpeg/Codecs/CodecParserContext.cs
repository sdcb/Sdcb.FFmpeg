using Sdcb.FFmpeg.Common;
using Sdcb.FFmpeg.Raw;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static Sdcb.FFmpeg.Raw.ffmpeg;

namespace Sdcb.FFmpeg.Codecs;

public unsafe partial class CodecParserContext : SafeHandle
{
    private const long NoPts = long.MinValue;

    /// <summary>
    /// <see cref="av_parser_init(int)"/>
    /// </summary>
    public CodecParserContext(AVCodecID codecId) : base(NativeUtils.NotNull((IntPtr)av_parser_init((int)codecId)), ownsHandle: true)
    {
    }

    /// <summary>
    /// <see cref="av_parser_parse2(AVCodecParserContext*, AVCodecContext*, byte**, int*, byte*, int, long, long, long)"/>
    /// </summary>
    public int Parse(CodecContext codecContext, DataPointer data, out DataPointer result, long pts = NoPts, long dts = NoPts, long pos = 0)
    {
        byte* outBuffer;
        int outBufferSize;

        int offset = av_parser_parse2(this, codecContext, &outBuffer, &outBufferSize, (byte*)data.Pointer, data.Length, pts, dts, pos).ThrowIfError();
        result = new DataPointer(outBuffer, outBufferSize);
        return offset;
    }

    public IEnumerable<DataPointer> Parse(CodecContext codecContext, Stream sourceStream, int bufferSize = 4096, long pts = NoPts, long dts = NoPts, long pos = 0)
    {
        byte[] data = new byte[bufferSize + AV_INPUT_BUFFER_PADDING_SIZE];
        GCHandle dataHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
        try
        {
            while (true)
            {
                int thisBufferLength = sourceStream.Read(data, 0, bufferSize);
                if (thisBufferLength == 0) break;

                DataPointer thisBuffer = new DataPointer(dataHandle.AddrOfPinnedObject(), thisBufferLength);
                while (thisBuffer.Length > 0)
                {
                    int offset = Parse(codecContext, thisBuffer, out DataPointer dataPointer, pts, dts, pos);
                    if (dataPointer.Length > 0) yield return dataPointer;
                    thisBuffer = thisBuffer[offset..];
                }
            }
        }
        finally
        {
            dataHandle.Free();
        }
    }

    /// <summary>
    /// <see cref="av_parser_close(AVCodecParserContext*)"/>
    /// </summary>
    public void Free()
    {
        av_parser_close(this);
        handle = IntPtr.Zero;
    }

    protected override bool ReleaseHandle()
    {
        Free();
        return true;
    }
}
