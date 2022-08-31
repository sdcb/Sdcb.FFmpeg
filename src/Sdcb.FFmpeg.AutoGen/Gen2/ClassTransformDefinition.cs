using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sdcb.FFmpeg.AutoGen.Gen2
{
    internal record G2TransformDefinition(string OldName, string NewName, ClassCategories ClassCategory)
    {
        public string GetDestFolder(string outputDir) => Path.Combine(Path.GetDirectoryName(outputDir), ClassCategory.ToString());
        public string GetDestFile(string outputDir) => Path.Combine(GetDestFolder(outputDir), $"{NewName}.g.cs");
        public string Namespace => $"Sdcb.FFmpeg.{ClassCategory}";
    }

    internal record ClassTransformDefinition(string OldName, string NewName, ClassCategories ClassCategory) : G2TransformDefinition(OldName, NewName, ClassCategory)
    {
    }

    internal record StructTransformDefinition(string OldName, string NewName, ClassCategories ClassCategory) : G2TransformDefinition(OldName, NewName, ClassCategory)
    {
    }

    internal enum ClassCategories
    {
        Codecs, 
    }
}
