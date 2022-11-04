using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
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
                TypeCastDef.StaticCastStruct("AVCodec*", "Codec"),
                TypeCastDef.StaticCastStruct("AVClass*", "FFmpegClass"),
                TypeCastDef.StaticCastClass("AVDictionary*", "MediaDictionary", isOwner: false),
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
                        ClassTransformDef => TypeCastDef.StaticCastClass(srcType, knownType.NewName, isOwner: false),
                        StructTransformDef => TypeCastDef.StaticCastStruct(srcType, knownType.NewName),
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

        public static TypeCastDef StaticCastClass(string oldType, string newType, bool isOwner) => new TypeStaticCastDef(oldType, newType, IsClass: true, IsOwner: isOwner);

        public static TypeCastDef StaticCastStruct(string oldType, string newType) => new TypeStaticCastDef(oldType, newType, IsClass: false);

        public static TypeCastDef CustomReadonly(string oldType, string newType, string readFormat) => new ReadOnlyFunctionCallCastDef(oldType, newType, readFormat);

        public static TypeCastDef Custom(string oldType, string newType, string readFormat, string writeFormat) => new FunctionCallCastDef(oldType, newType, readFormat, writeFormat);

        public static TypeCastDef ReadSequence(string elementType, string exitCondition = "p == default")
        {
            return new ReadOnlyFunctionCallCastDef(elementType + '*', $"IEnumerable<{elementType}>", $@"NativeUtils.ReadSequence(
            p: (IntPtr){{0}},
            unitSize: sizeof({elementType}),
            exitCondition: p => *({elementType}*){exitCondition}, 
            valGetter: p => *({elementType}*)p)");
        }

        public static TypeCastDef ReadSequenceAndCast(string elementType, string finalType, string exitCondition, string getter = "*{0}")
        {
            string finalGetter = string.Format(getter, $"({elementType}*)p");
            return new ReadOnlyFunctionCallCastDef(elementType + '*', $"IEnumerable<{finalType}>", $@"NativeUtils.ReadSequence(
            p: (IntPtr){{0}},
            unitSize: sizeof({elementType}),
            exitCondition: p => *({elementType}*){exitCondition}, 
            valGetter: p => {finalGetter})");
        }

        public static TypeCastDef ReadSequenceRaw(string elementType, string finalType, string unitSize, string exitCondition, string getter)
        {
            return new ReadOnlyFunctionCallCastDef(elementType + '*', $"IEnumerable<{finalType}>", $@"NativeUtils.ReadSequence(
            p: (IntPtr){{0}},
            unitSize: {unitSize},
            exitCondition: p => {exitCondition}, 
            valGetter: p => {getter})");
        }

        public static TypeCastDef ReadonlyPtrList(string elementType, string returnElementType, string countElement, string converterMethod)
        {
            return new ReadOnlyFunctionCallCastDef(
                elementType + "**", 
                $"IReadOnlyList<{returnElementType}>", 
                $@"new ReadOnlyPtrList<{elementType}, {returnElementType}>({{0}}, (int)_ptr->{countElement}, {converterMethod})");
        }

        public static TypeCastDef DataPointer(string elementType, string countElement, string countType)
        {
            string outCvt = countType switch
            {
                "int" => "", 
                _ => "(int)", 
            };
            string inCvt = countType switch
            {
                "int" => "", 
                _ => $"({countType})"
            };
            return new FunctionCallCastDef(elementType + "*", $"DataPointer",
                $@"new DataPointer({{0}}, (int)_ptr->{countElement})",
                $"((IntPtr)({{ptr}}->{{oldName}} = ({elementType})value.Pointer) + {outCvt}({{ptr}}->{countElement} = {inCvt}value.Length)).ToPointer()");
        }

        public static TypeCastDef ReadonlyNativeListWithCast(string elementType, string returnElementType, string countAccessor, string converterMethod)
        {
            return new ReadOnlyFunctionCallCastDef(
                elementType + "*",
                $"IReadOnlyList<{returnElementType}>",
                $@"new ReadOnlyNativeListWithCast<{elementType}, {returnElementType}>({{0}}, {countAccessor}, {returnElementType}.{converterMethod})");
        }

        public static TypeCastDef ReadonlyNativeList(string elementType, string countElement)
        {
            return new ReadOnlyFunctionCallCastDef(
                elementType + "*",
                $"IReadOnlyList<{elementType}>",
                $@"new ReadOnlyNativeList<{elementType}>({{0}}, (int)_ptr->{countElement})");
        }


        public static TypeCastDef ReadOnlyUtf8String() => CustomReadonly("byte*", "string", $"PtrExtensions.PtrToStringUTF8((IntPtr){{0}})");

        public static TypeCastDef OptUtf8String() => Custom("byte*", "string", $"PtrExtensions.PtrToStringUTF8((IntPtr){{0}})", $"Options.Set(\"{{oldName}}\", value)");

        public static TypeCastDef DupUtf8String() => Custom("byte*", "string", $"PtrExtensions.PtrToStringUTF8((IntPtr){{0}})", $"{{ptr}}->{{oldName}} = ffmpeg.av_strdup(value)");

        internal protected virtual string GetPropertyGetter(string ptr, string oldName, PropStatus prop)
        {
            return IsChanged switch
            {
                false => $"{ptr}->{oldName}",
                true => $"({NewType}){ptr}->{oldName}",
            };
        }

        internal protected virtual string GetPropertySetter(string ptr, string oldName, PropStatus prop) => $"{ptr}->{oldName} = {GetPropertySetterRight(ptr, oldName, prop)}";

        internal protected virtual string GetPropertySetterRight(string ptr, string oldName, PropStatus prop) => IsChanged switch
        {
            false => $"value",
            true => $"({OldType})value",
        };

        internal protected virtual string GetReturnType(PropStatus prop) => NewType;

        private record TypeStaticCastDef(string OldType, string NewType, bool IsClass, bool IsOwner = false) : TypeCastDef(OldType, NewType)
        {
            private string AdditionalText => IsClass switch
            {
                true => $", {IsOwner.ToString().ToLowerInvariant()}",
                false => "",
            };

            private string GetStaticMethod(PropStatus prop) => prop.IsNullable ? "FromNativeOrNull" : "FromNative";

            internal protected override string GetReturnType(PropStatus prop) => prop.IsNullable ? NewType + '?' : NewType;

            private bool IsOldTypePointer => OldType.EndsWith('*');

            private string PointerOriginalType => IsOldTypePointer ? OldType.Substring(0, OldType.Length - 1) : throw new InvalidOperationException();

            internal protected override string GetPropertyGetter(string ptr, string oldName, PropStatus prop) =>
                (prop.Name == NewType && prop.IsNullable && IsOldTypePointer, $"{NewType}.{GetStaticMethod(prop)}({ptr}->{oldName}{AdditionalText})") switch
                {
                    (true, string res) => G2Center.KnownClasses.TryGetValue(PointerOriginalType, out G2TransformDef? def) switch
                    {
                        true => def.Namespace + "." + res,
                        false => res
                    },
                    (false, string res) => res,
                };

            internal protected override string GetPropertySetterRight(string ptr, string oldName, PropStatus prop) => (IsClass && prop.IsNullable) switch
            {
                true => $"value != null ? {base.GetPropertySetterRight(ptr, oldName, prop)} : null",
                false => base.GetPropertySetterRight(ptr, oldName, prop),
            };
        }

        private record ReadOnlyFunctionCallCastDef(string OldType, string NewType, string ReadCallFormat) : TypeCastDef(OldType, NewType)
        {
            internal protected override string GetReturnType(PropStatus prop) => prop.IsNullable ? NewType + '?' : NewType;

            internal protected override string GetPropertyGetter(string ptr, string oldName, PropStatus prop) => prop.IsNullable switch
            {
                true => $"{ptr}->{oldName} != null ? {string.Format(ReadCallFormat, $"{ptr}->{oldName}")}! : null",
                false => string.Format(ReadCallFormat, $"{ptr}->{oldName}") + "!",
            };
        }

        private record FunctionCallCastDef(string OldType, string NewType, string ReadCallFormat, string WriteCallFormat) : ReadOnlyFunctionCallCastDef(OldType, NewType, ReadCallFormat)
        {
            protected internal override string GetPropertySetter(string ptr, string oldName, PropStatus prop) => WriteCallFormat
                .Replace("{ptr}", ptr)
                .Replace("{oldName}", oldName);
        }
    }
}
