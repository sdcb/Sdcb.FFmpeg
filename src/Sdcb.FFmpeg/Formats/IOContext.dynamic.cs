using Sdcb.FFmpeg.Common;
using Sdcb.FFmpeg.Raw;
using System;
using static Sdcb.FFmpeg.Raw.ffmpeg;

namespace Sdcb.FFmpeg.Formats
{
    public class DynamicIOContext : IOContext
    {
        internal protected unsafe DynamicIOContext(AVIOContext* ptr, bool isOwner) : base(ptr, isOwner)
        {
        }

        /// <summary>
        /// <see cref="avio_get_dyn_buf(AVIOContext*, byte**)"/>
        /// </summary>
        public unsafe Span<byte> GetBuffer()
        {
            byte* buffer = null;
            int length = avio_get_dyn_buf(this, &buffer).ThrowIfError();
            return new Span<byte>(buffer, length);
        }

        /// <summary>
        /// <see cref="avio_close_dyn_buf(AVIOContext*, byte**)"/>
        /// </summary>
        public unsafe DisposableDataPointer GetBufferAndClose()
        {
            byte* buffer = null;
            int length = avio_close_dyn_buf(this, &buffer);
            handle = IntPtr.Zero;
            return new DisposableDataPointer(buffer, length);
        }

        protected override bool ReleaseHandle()
        {
            using var _ = GetBufferAndClose();
            return true;
        }
    }
}
