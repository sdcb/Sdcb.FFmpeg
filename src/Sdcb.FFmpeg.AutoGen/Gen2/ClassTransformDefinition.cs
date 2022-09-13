using System.IO;

namespace Sdcb.FFmpeg.AutoGen.Gen2
{
    internal record G2TransformDef(ClassCategories ClassCategory, string OldName, string NewName, TypeCastDef[] TypeConversions)
    {
        public string GetDestFolder(string outputDir) => Path.Combine(Path.GetDirectoryName(outputDir)!, ClassCategory.ToString());
        public string GetDestFile(string outputDir) => Path.Combine(GetDestFolder(outputDir), $"{NewName}.g.cs");
        public string Namespace => $"{G2ClassWriter.NsBase}.{ClassCategory}";

        public static ClassTransformDef MakeClass(ClassCategories category, string oldName, string newName, TypeCastDef[]? typeConversions = null) =>
            new ClassTransformDef(category, oldName, newName, typeConversions ?? new TypeCastDef[0]);
        public static StructTransformDef MakeStruct(ClassCategories category, string oldName, string newName, TypeCastDef[]? typeConversions = null) =>
            new StructTransformDef(category, oldName, newName, typeConversions ?? new TypeCastDef[0]);
    }

    internal record ClassTransformDef(ClassCategories ClassCategory, string OldName, string NewName, TypeCastDef[] TypeConversions)
        : G2TransformDef(ClassCategory, OldName, NewName, TypeConversions)
    {
    }

    internal record StructTransformDef(ClassCategories ClassCategory, string OldName, string NewName, TypeCastDef[] TypeConversions)
        : G2TransformDef(ClassCategory, OldName, NewName, TypeConversions)
    {
    }

    internal enum ClassCategories
    {
        Codecs,
        Formats,
    }
}
