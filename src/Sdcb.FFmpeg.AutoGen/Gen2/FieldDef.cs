namespace Sdcb.FFmpeg.AutoGen.Gen2
{
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
}
