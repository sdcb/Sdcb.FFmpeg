using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Sdcb.FFmpeg.AutoGen.Gen2.TransformDefs;

#pragma warning disable CS8509 // switch 表达式不会处理属于其输入类型的所有可能值(它并非详尽无遗)。
namespace Sdcb.FFmpeg.AutoGen.Gen2
{
    internal static class G2TypeConvert
    {
        public static G2TypeConverter Create(Dictionary<string, G2TransformDef> knownDefinitions)
        {
            Indexer commonConverters = MakeIndexer(new[]
            {
                TypeCastDef.StaticCastStruct("AVCodec*", "Codec", nullable: false),
                TypeCastDef.StaticCastStruct("AVClass*", "FFmpegClass", nullable: false),
                TypeCastDef.Force("void*", "IntPtr"),
                TypeCastDef.Force("byte*", "IntPtr"),
            });

            Dictionary<string, G2TransformDef> knownMappings = knownDefinitions
                .ToDictionary(k => k.Key + '*', v => v.Value);

            return Convert;

            TypeCastDef Convert(string srcType, string srcName) => commonConverters(srcType, srcName, out TypeCastDef? commonType) switch
            {
                true => commonType,
                false => knownMappings.TryGetValue(srcType, out G2TransformDef? knownType) switch
                {
                    true => knownType switch
                    {
                        ClassTransformDef => TypeCastDef.StaticCastClass(srcType, knownType.NewName, nullable: true, isOwner: false),
                        StructTransformDef => TypeCastDef.StaticCastStruct(srcType, knownType.NewName, nullable: true),
                    },
                    false => TypeCastDef.NotChanged(srcType),
                }
            };
        }

        static Indexer MakeIndexer(IEnumerable<TypeCastDef> data)
        {
            Dictionary<string, TypeCastDef> typeMapping = data
                .ToDictionary(k => k.OldType, v => v);

            return Indexer;

            bool Indexer(string srcType, string srcName, [NotNullWhen(returnValue: true)] out TypeCastDef? value)
            {
                if (typeMapping.TryGetValue(srcType, out TypeCastDef? typeValue))
                {
                    value = typeValue;
                    return true;
                }
                value = null;
                return false;
            }
        }

        private delegate bool Indexer(string srcType, string srcName, [NotNullWhen(returnValue: true)] out TypeCastDef? value);
    }

    internal delegate TypeCastDef G2TypeConverter(string srcType, string srcName);

    internal record TypeCastDef(string OldType, string NewType)
    {
        public bool IsChanged => OldType != NewType;

        public static TypeCastDef NotChanged(string type) => new TypeCastDef(type, type);

        public static TypeCastDef Force(string oldType, string newType) => new TypeCastDef(oldType, newType);

        public static TypeCastDef StaticCastClass(string oldType, string newType, bool nullable, bool isOwner) => new TypeStaticCastDef(oldType, newType, nullable, IsClass: true, IsOwner: isOwner);

        public static TypeCastDef StaticCastStruct(string oldType, string newType, bool nullable) => new TypeStaticCastDef(oldType, newType, nullable, IsClass: false);

        public static TypeCastDef CustomReadonly(string oldType, string newType, string readFormat, bool nullable) => new FunctionCallCastDef(oldType, newType, readFormat, nullable);

        public static TypeCastDef ReadSequence(string elementType, string exitCondition = "p == default") => new FunctionCallCastDef(elementType + '*', $"IEnumerable<{elementType}>", $@"NativeUtils.ReadSequence(
            p: (IntPtr){{0}},
            unitSize: sizeof({elementType}),
            exitCondition: p => *({elementType}*){exitCondition}, 
            valGetter: p => *({elementType}*)p)", Nullable: false);

        public static TypeCastDef Utf8String(bool nullable) => CustomReadonly("byte*", "string", $"PtrExtensions.PtrToStringUTF8((IntPtr){{0}})", nullable);

        internal protected virtual string GetPropertyGetter(string expression, string newName)
        {
            return IsChanged switch
            {
                false => expression,
                true => $"({NewType}){expression}",
            };
        }

        internal protected virtual string PropertySetter => IsChanged switch
        {
            false => $"value",
            true => $"({OldType})value",
        };

        internal protected virtual string ReturnType => NewType;

        private record TypeStaticCastDef(string OldType, string NewType, bool Nullable, bool IsClass, bool IsOwner = false) : TypeCastDef(OldType, NewType)
        {
            private string AdditionalText => IsClass switch
            {
                true => $", {IsOwner.ToString().ToLowerInvariant()}",
                false => "",
            };

            private string StaticMethod => Nullable ? "FromNativeOrNull" : "FromNative";

            internal protected override string ReturnType => Nullable ? NewType + '?' : NewType;

            private bool IsOldTypePointer => OldType.EndsWith('*');

            private string PointerOriginalType => IsOldTypePointer ? OldType.Substring(0, OldType.Length - 1) : throw new InvalidOperationException();

            internal protected override string GetPropertyGetter(string expression, string newName) =>
                (newName == NewType && Nullable && IsOldTypePointer, $"{NewType}.{StaticMethod}({expression}{AdditionalText})") switch
                {
                    (true, string res) => G2Center.KnownClasses.TryGetValue(PointerOriginalType, out G2TransformDef? def) switch
                    {
                        true => def.Namespace + "." + res, 
                        false => res
                    },
                    (false, string res) => res,
                };

            internal protected override string PropertySetter => (IsClass && Nullable) switch
            {
                true => $"value != null ? {base.PropertySetter} : null",
                false => base.PropertySetter,
            };
        }

        private record FunctionCallCastDef(string OldType, string NewType, string ReadCallFormat, bool Nullable) : TypeCastDef(OldType, NewType)
        {
            internal protected override string ReturnType => Nullable ? NewType + '?' : NewType;

            internal protected override string GetPropertyGetter(string expression, string newName) => Nullable switch
            {
                true => $"{{0}} != null ? {string.Format(ReadCallFormat, expression)}! : null",
                false => string.Format(ReadCallFormat, expression) + "!",
            };
        }
    }
}
