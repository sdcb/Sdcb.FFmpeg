using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sdcb.FFmpeg.AutoGen.Gen2
{
    internal record ClassTransformDefinition(string OldName, string NewName, ClassCategories ClassCategory)
    {
        public string GetDestFolder(string outputDir) => Path.Combine(Path.GetDirectoryName(outputDir), ClassCategory.ToString());
        public string GetDestFile(string outputDir) => Path.Combine(GetDestFolder(outputDir), $"{NewName}.g.cs");
        public string Namespace => $"Sdcb.FFmpeg.{ClassCategory}";
    }

    internal enum ClassCategories
    {
        Codecs, 
    }
}
