using System.Diagnostics;

namespace Sdcb.FFmpeg.AutoGen
{
    [DebuggerDisplay("{Name}, {LibraryName}")]
    internal record FunctionExport(string Name, string LibraryName, int LibraryVersion);
}