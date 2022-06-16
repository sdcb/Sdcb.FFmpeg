using System.Diagnostics;

namespace Sdcb.FFmpeg.AutoGen
{
    [DebuggerDisplay("{Name}, {LibraryName}")]
    internal class FunctionExport
    {
        public string Name { get; init; }
        public string LibraryName { get; init; }
        public int LibraryVersion { get; init; }
    }
}