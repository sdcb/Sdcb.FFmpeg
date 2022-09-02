using System.Collections.Generic;
using System.Linq;

#pragma warning disable CS8509 // switch 表达式不会处理属于其输入类型的所有可能值(它并非详尽无遗)。
namespace Sdcb.FFmpeg.AutoGen.Gen2
{
    internal static class G2TypeConvert
    {
        public static G2TypeConverter Create(Dictionary<string, G2TransformDef> knownDefinitions)
        {
            Dictionary<string, TypeCastDef> commonConverters = new[]
            {
                TypeCastDef.StaticCastStruct("AVCodec*", "Codec", nullable: false),
                TypeCastDef.StaticCastStruct("AVClass*", "FFmpegClass", nullable: false),
                TypeCastDef.Force("void*", "IntPtr"),
                TypeCastDef.Force("byte*", "IntPtr"),
            }.ToDictionary(k => k.OldType, v => v);

            Dictionary<string, G2TransformDef> knownMappings = knownDefinitions
                .ToDictionary(k => k.Key + '*', v => v.Value);

            return Convert;

            TypeCastDef Convert(string oldType) => commonConverters.TryGetValue(oldType, out TypeCastDef? commonType) switch
            {
                true => commonType,
                false => knownMappings.TryGetValue(oldType, out G2TransformDef? knownType) switch
                {
                    true => knownType switch
                    {
                        ClassTransformDef => TypeCastDef.StaticCastClass(oldType, knownType.NewName, nullable: true, isOwner: false),
                        StructTransformDef => TypeCastDef.StaticCastStruct(oldType, knownType.NewName, nullable: true),
                    },
                    false => TypeCastDef.NotChanged(oldType),
                }
            };
        }

        public static G2TypeConverter Combine(TypeCastDef[] specialConverters, G2TypeConverter baseConverter)
        {
            Dictionary<string, TypeCastDef> specialConverterMapping = specialConverters
                .ToDictionary(k => k.OldType, v => v);

            return Convert;
            TypeCastDef Convert(string oldType)
            {
                return specialConverterMapping.TryGetValue(oldType, out TypeCastDef? converted) switch
                {
                    true => converted,
                    false => baseConverter(oldType),
                };
            }
        }
    }

    internal delegate TypeCastDef G2TypeConverter(string srcType);

    internal record TypeCastDef(string OldType, string NewType)
    {
        public bool IsChanged => OldType != NewType;

        public static TypeCastDef NotChanged(string type) => new TypeCastDef(type, type);

        public static TypeCastDef Force(string oldType, string newType) => new TypeCastDef(oldType, newType);

        public static TypeCastDef StaticCastClass(string oldType, string newType, bool nullable, bool isOwner) => new TypeStaticCastDef(oldType, newType, nullable, IsClass: true, IsOwner: isOwner);

        public static TypeCastDef StaticCastStruct(string oldType, string newType, bool nullable) => new TypeStaticCastDef(oldType, newType, nullable, IsClass: false);

        protected virtual string GetPropertyGetter(string expression)
        {
            return IsChanged switch
            {
                false => expression,
                true => $"({NewType}){expression}",
            };
        }

        protected virtual string PropertySetter => IsChanged switch
        {
            false => $"value",
            true => $"({OldType})value",
        };

        protected virtual string ReturnType => NewType;

        private const string _ptr = "_ptr";

        public virtual IEnumerable<string> GetPropertyBody(string fieldName)
        {
            string transformedName = StringExtensions.CSharpKeywordTransform(fieldName);

            yield return $"public {ReturnType} {G2StringTransforms.NameTransform(fieldName)}";
            yield return "{";
            yield return $"    get => {GetPropertyGetter($"{_ptr}->{transformedName}")};";
            yield return $"    set => {_ptr}->{transformedName} = {PropertySetter};";
            yield return "}";
        }

        private record TypeStaticCastDef(string OldType, string NewType, bool Nullable, bool IsClass, bool IsOwner = false) : TypeCastDef(OldType, NewType)
        {
            private string AdditionalText => IsClass switch
            {
                true => $", {IsOwner.ToString().ToLowerInvariant()}",
                false => "",
            };

            private string StaticMethod => Nullable ? "FromNativeOrNull" : "FromNative";

            protected override string ReturnType => Nullable ? NewType + '?' : NewType;

            protected override string GetPropertyGetter(string expression) => $"{NewType}.{StaticMethod}({expression}{AdditionalText})";
        }
    }
}
