namespace Sdcb.FFmpeg.AutoGen.Gen2.TransformDefs
{
    internal record FieldDef
    {
        public string Name { get; init; }

        public string NewName { get; init; }

        private TypeCastDef? _typeCast;

        public bool? ReadOnly { get; init; }

        public bool Display { get; init; } = true;

        public bool IsRenamed => Name != NewName;

        public FieldDef(string name, string newName, TypeCastDef? typeCast = null, bool? isReadonly = null, bool display = true)
        {
            Name = name;
            NewName = newName;
            _typeCast = typeCast;
            ReadOnly = isReadonly;
            Display = display;
        }

        public TypeCastDef? TypeCast => _typeCast switch
        {
            { } x => x with { FieldName = Name },
            null => null,
        };

        public static FieldDef CreateTypeCast(string name, TypeCastDef typeCast) => new FieldDef(name, name, typeCast);

        public static FieldDef CreateHide(string name) => new FieldDef(name, name, display: false);

        public static FieldDef CreateRename(string name, string newName) => new FieldDef(name, newName);

        public static FieldDef CreateDefault(string name) => new FieldDef(name, name);
    }
}
