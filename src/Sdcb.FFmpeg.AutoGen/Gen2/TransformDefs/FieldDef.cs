using System.Collections.Generic;

namespace Sdcb.FFmpeg.AutoGen.Gen2.TransformDefs
{
    internal record FieldDef
    {
        public string Name { get; init; }

        public string NewName { get; init; }

        public TypeCastDef? TypeCastDef { get; init; }

        public bool? ReadOnly { get; init; }

        public bool Display { get; init; } = true;

        public bool IsRenamed => Name != NewName;

        public bool? Nullable { get; init; } = null;

        public FieldDef(string name, string newName, TypeCastDef? typeCast = null, bool? isReadonly = null, bool display = true, bool? nullable = null)
        {
            Name = name;
            NewName = newName;
            TypeCastDef = typeCast;
            ReadOnly = isReadonly;
            Display = display;
            Nullable = nullable;
        }

        public static bool ShouldMakeRefReadonly(string newTypeName) => newTypeName switch
        {
            var _ when newTypeName.Contains("_array") => true,
            var _ when newTypeName.Contains("_ptrArray") => true,
            _ => false,
        };

        public virtual IEnumerable<string> GetPropertyBody(string fieldName, TypeCastDef typeCastDef, PropStatus propStatus)
        {
            const string _ptr = "_ptr";
            string oldName = StringExtensions.CSharpKeywordTransform(fieldName);
            string newName = propStatus.Name;

            if (ShouldMakeRefReadonly(typeCastDef.NewType))
            {
                yield return $"public ref {typeCastDef.GetReturnType(propStatus)} {newName}";
                yield return "{";
                yield return $"    get => ref {typeCastDef.GetPropertyGetter(_ptr, oldName, propStatus)};";
                yield return "}";
            }
            else if (propStatus.IsReadonly)
            {
                yield return $"public {typeCastDef.GetReturnType(propStatus)} {newName} => {typeCastDef.GetPropertyGetter(_ptr, oldName, propStatus)};";
            }
            else
            {
                yield return $"public {typeCastDef.GetReturnType(propStatus)} {newName}";
                yield return "{";
                yield return $"    get => {typeCastDef.GetPropertyGetter(_ptr, oldName, propStatus)};";
                yield return $"    set => {typeCastDef.GetPropertySetter(_ptr, oldName, propStatus)};";
                yield return "}";
            }
        }

        public bool CalculateIsNullable()
        {
            if (Nullable.HasValue) return Nullable.Value;
            if (TypeCastDef == null) return false;
            return false;
        }

        public static FieldDef CreateTypeCast(string name, TypeCastDef typeCast) => new FieldDef(name, name, typeCast);

        public static FieldDef CreateTypeCastReadonly(string name, TypeCastDef typeCast) => new FieldDef(name, name, typeCast, isReadonly: true);

        public static FieldDef CreateTypeCastNullable(string name, TypeCastDef typeCast) => new FieldDef(name, name, typeCast, nullable: true);

        public static FieldDef CreateHide(string name) => new FieldDef(name, name, display: false);

        public static FieldDef CreateRenameNullable(string name, string newName) => new FieldDef(name, newName, nullable: true);

        public static FieldDef CreateDefault(string name) => new FieldDef(name, name);

        public static FieldDef CreateNullable(string name) => new FieldDef(name, name, nullable: true);
    }
}
