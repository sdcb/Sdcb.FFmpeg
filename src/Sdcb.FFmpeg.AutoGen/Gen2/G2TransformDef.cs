﻿using Sdcb.FFmpeg.AutoGen.Definitions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using static FParsec.Primitives;

namespace Sdcb.FFmpeg.AutoGen.Gen2
{
    internal abstract record G2TransformDef(ClassCategories ClassCategory, string OldName, string NewName, FieldDef[] FieldDefs, bool AllReadOnly = false)
    {
        internal const string NsBase = "Sdcb.FFmpeg";

        public string GetDestFolder(string outputDir) => Path.Combine(Path.GetDirectoryName(outputDir)!, ClassCategory.ToString());
        public string GetDestFile(string outputDir) => Path.Combine(GetDestFolder(outputDir), $"{NewName}.g.cs");
        public string Namespace => $"{NsBase}.{ClassCategory}";

        public IEnumerable<TypeCastDef> TypeConversions => FieldDefs
            .Where(x => x.TypeCast != null)
            .Select(x => x.TypeCast)!;

        public static ClassTransformDef MakeClass(ClassCategories category, string oldName, string newName, FieldDef[]? typeConversions = null) =>
            new ClassTransformDef(category, oldName, newName, typeConversions ?? new FieldDef[0], AllReadOnly: false);
        public static StructTransformDef MakeStruct(ClassCategories category, string oldName, string newName, FieldDef[]? typeConversions = null) =>
            new StructTransformDef(category, oldName, newName, typeConversions ?? new FieldDef[0], AllReadOnly: false);

        public static StructTransformDef MakeReadonlyStruct(ClassCategories category, string oldName, string newName, FieldDef[]? typeConversions = null) =>
            new StructTransformDef(category, oldName, newName, typeConversions ?? new FieldDef[0], AllReadOnly: true);

        public virtual IEnumerable<string> GenerateOneCode(StructureDefinition structure, G2TypeConverter typeConverter)
        {
            IndentManager indentManager = new IndentManager();
            Func<string, string> ind = indentManager.Wrap;

            foreach (string line in GetFileHeader())
            {
                yield return ind(line);
            }
            yield return ind("");
            foreach (string line in GetDefinitionComment(structure))
            {
                yield return ind(line);
            }
            yield return ind(DefinitionLineCode);
            yield return ind("{");
            using (indentManager.BeginScope())
            {
                ReadonlyIndexer readonlyIndexer = MakeReadonlyIndexer();
                foreach (string line in GetCommonHeaderCode())
                {
                    yield return ind(line);
                }
                yield return ind("");
                G2TypeConverter thisTypeConverter = G2TypeConvert.Combine(TypeConversions, typeConverter);
                foreach (StructureField field in structure.Fields)
                {
                    foreach (string line in GetPropertyCode(structure, field, thisTypeConverter, readonlyIndexer))
                    {
                        yield return ind(line);
                    }
                    if (field != structure.Fields.Last()) yield return ind("");
                }
            }
            yield return ind("}");
        }

        protected delegate bool ReadonlyIndexer(string field);

        private ReadonlyIndexer MakeReadonlyIndexer()
        {
            Dictionary<string, bool> cache = FieldDefs
                .Where(x => x.ReadOnly != null)
                .ToDictionary(k => k.Name, v => v.ReadOnly!.Value);

            return Indexer;

            bool Indexer(string field) => cache.TryGetValue(field, out bool isReadonly) switch
            {
                true => isReadonly,
                false => AllReadOnly,
            };
        }

        protected virtual IEnumerable<string> GetFileHeader()
        {
            yield return "// This file was genereated from Sdcb.FFmpeg.AutoGen, DO NOT CHANGE DIRECTLY.";
            yield return $"#nullable enable";
            yield return $"using {NsBase}.Common;";
            foreach (string otherNs in Enum
                .GetNames<ClassCategories>()
                .Where(x => x != ClassCategory.ToString()))
            {
                yield return $"using {NsBase}.{otherNs};";
            }
            yield return $"using {NsBase}.Raw;";
            yield return "using System;";
            yield return "using System.Collections.Generic;";
            yield return "using System.Runtime.InteropServices;";
            yield return "";
            yield return $"namespace {Namespace};";
        }
        protected abstract string DefinitionLineCode { get; }
        protected abstract IEnumerable<string> GetCommonHeaderCode();
        protected virtual IEnumerable<string> GetDefinitionComment(StructureDefinition structure)
        {
            return BuildXmlComment(BuildCommentForStructure(structure));
        }
        protected virtual IEnumerable<string> GetPropertyCode(StructureDefinition structure, StructureField field, G2TypeConverter typeConverter, ReadonlyIndexer readonlyIndexer)
        {
            TypeCastDef typeCastDef = typeConverter(field.FieldType.Name, field.Name);
            foreach (string line in BuildXmlComment(BuildCommentForField(structure, field, typeCastDef.IsChanged)))
            {
                yield return line;
            }
            if (field.Obsoletion.IsObsolete)
            {
                yield return $"[Obsolete(\"{StringExtensions.DoubleQuoteEscape(field.Obsoletion.Message)}\")]";
            }
            foreach (string line in typeCastDef.GetPropertyBody(field.Name, isReadonly: readonlyIndexer(field.Name)))
            {
                yield return line;
            }
        }

        protected static XElement BuildCommentForField(StructureDefinition structure, StructureField field, bool isTypeChanged)
        {
            List<object> contents = new();

            if (isTypeChanged)
            {
                contents.Add(new XElement("para", $"original type: {field.FieldType.Name}"));
            }

            contents.AddRange(field.XmlDocument switch
            {
                null => new object[0],
                _ => field.XmlDocument
                    .Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                    .Select(line => new XElement("para", line))
            });

            contents.Add(new XElement("see", new XAttribute("cref", $"{structure.Name}.{StringExtensions.CSharpKeywordTransform(field.Name)}")));

            return new XElement("summary", contents);
        }

        protected static XElement BuildCommentForStructure(StructureDefinition structure)
        {
            List<object> contents = new();
            contents.AddRange(structure.XmlDocument switch
            {
                null => new object[0],
                _ => structure.XmlDocument
                    .Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                    .Select(line => new XElement("para", line))
            });
            contents.Add(new XElement("see", new XAttribute("cref", $"{structure.Name}")));

            return new XElement("summary", contents);
        }

        protected static IEnumerable<string> BuildXmlComment(XElement xml)
        {
            return xml
                .ToString()
                .Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .Select(x => $"/// {x}");
        }
    }
}
