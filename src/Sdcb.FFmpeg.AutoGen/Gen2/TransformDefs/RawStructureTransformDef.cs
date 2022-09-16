using System.Collections.Generic;
using System.Collections.Immutable;

namespace Sdcb.FFmpeg.AutoGen.Gen2.TransformDefs
{
    internal class RawStructureTransformDef
    {
        public static readonly ImmutableHashSet<string> IgnoredStructures = new HashSet<string>(new[] 
        { 
            "AVRational",
        }).ToImmutableHashSet();
    }
}
