using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
