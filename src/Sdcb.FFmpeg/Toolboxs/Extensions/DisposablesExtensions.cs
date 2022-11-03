using System;
using System.Collections.Generic;

namespace Sdcb.FFmpeg.Toolboxs.Extensions
{
    public static class DisposablesExtensions
    {
        public static void DisposeAll(this IEnumerable<IDisposable> source)
        {
            foreach (IDisposable disposable in source)
            {
                disposable.Dispose();
            }
        }
    }
}
