namespace Sdcb.FFmpeg.AutoGen.Gen2.TransformDefs
{
    internal record FieldDef
    {
        public string Name { get; init; }

        private TypeCastDef? _typeCast;

        public bool? ReadOnly { get; init; }

        public bool Display { get; init; } = true;

        public FieldDef(string name, TypeCastDef? typeCast = null, bool? isReadonly = null, bool display = true)
        {
            Name = name;
            _typeCast = typeCast;
            ReadOnly = isReadonly;
            Display = display;
        }

        public TypeCastDef? TypeCast => _typeCast switch
        {
            { } x => x with { FieldName = Name },
            null => null,
        };

        public static FieldDef CreateTypeCast(string name, TypeCastDef typeCast) => new FieldDef(name, typeCast);

        public static FieldDef CreateHide(string name) => new FieldDef(name, display: false);
    }
}
