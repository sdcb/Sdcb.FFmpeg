using System.Collections.Generic;

namespace Sdcb.FFmpeg.AutoGen.Gen2.TransformDefs
{
    internal record FieldDef
    {
        public string Name { get; init; }

        public string NewName { get; init; }

        public TypeCastDef? TypeCast { get; init; }

        public bool? ReadOnly { get; init; }

        public bool Display { get; init; } = true;

        public bool IsRenamed => Name != NewName;

        public FieldDef(string name, string newName, TypeCastDef? typeCast = null, bool? isReadonly = null, bool display = true)
        {
            Name = name;
            NewName = newName;
            TypeCast = typeCast;
            ReadOnly = isReadonly;
            Display = display;
        }

        public virtual IEnumerable<string> GetPropertyBody(string fieldName, TypeCastDef typeCastDef, PropStatus propStatus)
        {
            const string _ptr = "_ptr";
            string oldName = StringExtensions.CSharpKeywordTransform(fieldName);
            string newName = propStatus.Name;

            if (propStatus.IsReadonly)
            {
                yield return $"public {typeCastDef.ReturnType} {newName} => {typeCastDef.GetPropertyGetter($"{_ptr}->{oldName}", newName)};";
            }
            else
            {
                yield return $"public {typeCastDef.ReturnType} {newName}";
                yield return "{";
                yield return $"    get => {typeCastDef.GetPropertyGetter($"{_ptr}->{oldName}", newName)};";
                yield return $"    set => {_ptr}->{oldName} = {typeCastDef.PropertySetter};";
                yield return "}";
            }
        }

        public static FieldDef CreateTypeCast(string name, TypeCastDef typeCast) => new FieldDef(name, name, typeCast);

        public static FieldDef CreateHide(string name) => new FieldDef(name, name, display: false);

        public static FieldDef CreateRename(string name, string newName) => new FieldDef(name, newName);

        public static FieldDef CreateDefault(string name) => new FieldDef(name, name);
    }
}
