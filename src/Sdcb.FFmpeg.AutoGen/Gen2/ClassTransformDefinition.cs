using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Sdcb.FFmpeg.AutoGen.Gen2
{
    internal record G2TransformDef(ClassCategories ClassCategory, string OldName, string NewName, FieldDef[] FieldDefs, bool AllReadOnly = false)
    {
        public string GetDestFolder(string outputDir) => Path.Combine(Path.GetDirectoryName(outputDir)!, ClassCategory.ToString());
        public string GetDestFile(string outputDir) => Path.Combine(GetDestFolder(outputDir), $"{NewName}.g.cs");
        public string Namespace => $"{G2ClassWriter.NsBase}.{ClassCategory}";

        public IEnumerable<TypeCastDef> TypeConversions => FieldDefs
            .Where(x => x.TypeCast != null)
            .Select(x => x.TypeCast)!;

        public static ClassTransformDef MakeClass(ClassCategories category, string oldName, string newName, FieldDef[]? typeConversions = null) =>
            new ClassTransformDef(category, oldName, newName, typeConversions ?? new FieldDef[0], AllReadOnly: false);
        public static StructTransformDef MakeStruct(ClassCategories category, string oldName, string newName, FieldDef[]? typeConversions = null) =>
            new StructTransformDef(category, oldName, newName, typeConversions ?? new FieldDef[0], AllReadOnly: false);

        public static StructTransformDef MakeReadonlyStruct(ClassCategories category, string oldName, string newName, FieldDef[]? typeConversions = null) =>
            new StructTransformDef(category, oldName, newName, typeConversions ?? new FieldDef[0], AllReadOnly: true);

        public bool IsReadonlyForField(string name)
        {
            FieldDef? def = FieldDefs.FirstOrDefault(x => x.Name == name);
            if (def != null && def.ReadOnly != null) return def.ReadOnly.Value;

            return AllReadOnly;
        }
    }

    internal record ClassTransformDef(ClassCategories ClassCategory, string OldName, string NewName, FieldDef[] FieldDefs, bool AllReadOnly)
        : G2TransformDef(ClassCategory, OldName, NewName, FieldDefs, AllReadOnly)
    {
    }

    internal record StructTransformDef(ClassCategories ClassCategory, string OldName, string NewName, FieldDef[] FieldDefs, bool AllReadOnly)
        : G2TransformDef(ClassCategory, OldName, NewName, FieldDefs, AllReadOnly)
    {
    }

    internal record FieldDef
    {
        public string Name { get; init; }

        private TypeCastDef? _typeCast;

        public bool? ReadOnly { get; init; }

        public FieldDef(string name, TypeCastDef? typeCast = null, bool? isReadonly = null)
        {
            Name = name;
            _typeCast = typeCast;
            ReadOnly = isReadonly;
        }

        public TypeCastDef? TypeCast => _typeCast switch
        {
            { } x => x with { FieldName = Name },
            null => null,
        };
    }

    internal enum ClassCategories
    {
        Codecs,
        Formats,
    }
}
