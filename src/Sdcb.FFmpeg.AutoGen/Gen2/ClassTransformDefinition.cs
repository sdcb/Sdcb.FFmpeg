using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sdcb.FFmpeg.AutoGen.Gen2
{
    internal record G2TransformDef(ClassCategories ClassCategory, string OldName, string NewName, Dictionary<string, TypeCastDef> TypeConversions)
    {
        public string GetDestFolder(string outputDir) => Path.Combine(Path.GetDirectoryName(outputDir)!, ClassCategory.ToString());
        public string GetDestFile(string outputDir) => Path.Combine(GetDestFolder(outputDir), $"{NewName}.g.cs");
        public string Namespace => $"Sdcb.FFmpeg.{ClassCategory}";

        public static ClassTransformDef MakeClass(ClassCategories category, string oldName, string newName, Dictionary<string, TypeCastDef>? typeConversions = null) =>
            new ClassTransformDef(category, oldName, newName, typeConversions ?? new());
        public static StructTransformDef MakeStruct(ClassCategories category, string oldName, string newName, Dictionary<string, TypeCastDef>? typeConversions = null) =>
            new StructTransformDef(category, oldName, newName, typeConversions ?? new());
    }

    internal record ClassTransformDef(ClassCategories ClassCategory, string OldName, string NewName, Dictionary<string, TypeCastDef> TypeConversions)
        : G2TransformDef(ClassCategory, OldName, NewName, TypeConversions)
    {
    }

    internal record StructTransformDef(ClassCategories ClassCategory, string OldName, string NewName, Dictionary<string, TypeCastDef> TypeConversions)
        : G2TransformDef(ClassCategory, OldName, NewName, TypeConversions)
    {
    }

    internal enum ClassCategories
    {
        Codecs, 
    }
}
