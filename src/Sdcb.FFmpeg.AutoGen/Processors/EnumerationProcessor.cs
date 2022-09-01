using System;
using System.Collections.Generic;
using System.Linq;
using CppSharp.AST;
using CppSharp.AST.Extensions;
using Sdcb.FFmpeg.AutoGen.Definitions;

#nullable disable

namespace Sdcb.FFmpeg.AutoGen.Processors
{
    internal class EnumerationProcessor
    {
        private readonly ASTProcessor _context;

        public EnumerationProcessor(ASTProcessor context) => _context = context;

        public void Process(TranslationUnit translationUnit)
        {
            foreach (var enumeration in translationUnit.Enums)
            {
                if (!enumeration.Type.IsPrimitiveType()) throw new NotSupportedException();

                var enumerationName = enumeration.Name;
                if (string.IsNullOrEmpty(enumerationName))
                    continue;

                MakeDefinition(enumeration, enumerationName);
            }
        }

        public void MakeDefinition(Enumeration enumeration, string name)
        {
            name = string.IsNullOrEmpty(enumeration.Name) ? name : enumeration.Name;
            if (_context.IsKnownUnitName(name)) return;

            string commonPrefix = StringExtensions.CommonPrefixOf(enumeration.Items.Select(x => x.Name));
            var definition = new EnumerationDefinition
            {
                Name = name,
                TypeName = TypeHelper.GetTypeName(enumeration.Type),
                XmlDocument = enumeration.Comment?.BriefText,
                Obsoletion = ObsoletionHelper.CreateObsoletion(enumeration),
                Items = enumeration.Items
                    .Select(x =>
                        new EnumerationItem
                        {
                            Name = StringExtensions.EnumNameTransform(x.Name[commonPrefix.Length..]),
                            RawName = x.Name, 
                            Value = ConvertValue(x.Value, enumeration.BuiltinType.Type).ToString(),
                            XmlDocument = x.Comment?.BriefText
                        })
                    .ToArray()
            };

            _context.AddUnit(definition);
        }

        private static object ConvertValue(ulong value, PrimitiveType primitiveType)
        {
            return primitiveType switch
            {
                PrimitiveType.Int => value > int.MaxValue ? (int) value : value,
                PrimitiveType.UInt => value,
                PrimitiveType.Long => value > long.MaxValue ? (long) value : value,
                PrimitiveType.ULong => value,
                _ => throw new NotSupportedException()
            };
        }
    }
}
