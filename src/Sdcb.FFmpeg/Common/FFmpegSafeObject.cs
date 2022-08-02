using System;

namespace Sdcb.FFmpeg.Common
{
    public abstract class FFmpegSafeObject : IDisposable
    {
        protected internal IntPtr _nativePointer;
        protected bool _isOwner;

        protected FFmpegSafeObject(IntPtr nativePointer, bool isOwner)
        {
            _nativePointer = nativePointer;
            _isOwner = isOwner;
        }

        protected FFmpegSafeObject()
        {
        }

        protected abstract void DisposeNative();

        private void Dispose(bool disposing)
        {
            if (_nativePointer != IntPtr.Zero)
            {
                if (_isOwner)
                {
                    if (!disposing)
                    {
                        LogMemoryLeakWarning?.Invoke($"Warning: potential memory leak: [0x{_nativePointer:X}]");
                    }
                    DisposeNative();
#if DEBUG
                    if (_nativePointer != IntPtr.Zero) throw new FFmpegException($"_handle should reset in {nameof(DisposeNative)}.");
#endif
                }

                _nativePointer = IntPtr.Zero;
            }
        }

        ~FFmpegSafeObject() => Dispose(disposing: false);

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public static Action<string>? LogMemoryLeakWarning;
    }
}
