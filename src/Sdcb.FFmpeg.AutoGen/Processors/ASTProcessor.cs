using System;
using System.Collections.Generic;
using System.Linq;
using CppSharp.AST;
using Sdcb.FFmpeg.AutoGen.Definitions;
using MacroDefinition = CppSharp.AST.MacroDefinition;

namespace Sdcb.FFmpeg.AutoGen.Processors
{
    internal class ASTProcessor
    {
        public ASTProcessor()
        {
            IgnoreUnitNames = new HashSet<string>();
            FunctionProcessor = new FunctionProcessor(this);
            StructureProcessor = new StructureProcessor(this);
            EnumerationProcessor = new EnumerationProcessor(this);
        }

        public HashSet<string> IgnoreUnitNames { get; }
        public Dictionary<string, string> TypeAliasMap { get; } = new Dictionary<string, string>
        {
            ["int64_t"] = "long",
            ["UINT64_C"] = "ulong",
        };

        public EnumerationProcessor EnumerationProcessor { get; }
        public StructureProcessor StructureProcessor { get; }
        public FunctionProcessor FunctionProcessor { get; }

        public Dictionary<string, FunctionExport> FunctionExportMap { get; init; }

        public List<IDefinition> Units { get; init; } = new();

        public bool IsKnownUnitName(string name)
        {
            return Units.Any(x => x.Name == name);
        }

        public void AddUnit(IDefinition definition)
        {
            if (IgnoreUnitNames.Contains(definition.Name)) return;

            switch (definition)
            {
                // for
                case FunctionDefinitionBase df:
                    {
                        // check for existing functions with same parameters
                        // we care about the parameters, as we want to allow functions with same name but different parameters (overloads)
                        var existingWithSameName = Units.OfType<FunctionDefinitionBase>().Where(x => x.Name == definition.Name);
                        var existingWithSameParameters = existingWithSameName.Where(v => v.Parameters.SequenceEqual(df.Parameters)).ToList();

                        foreach (var d in existingWithSameParameters)
                        {
                            Units.Remove(d);
                        }

                        break;
                    }
                default:
                    {
                        // don't allow adding if existing definition with same name
                        var existing = Units.FirstOrDefault(x => x.Name == definition.Name);
                        if (existing != null)
                            Units.Remove(existing);
                        break;
                    }
            }
            Units.Add(definition);
        }

        public void Process(IEnumerable<TranslationUnit> units)
        {
            MacroDefinitionRaw[] rawMacros = default;

            MetricHelper.RecordTime("Macro/Enumeration/Structure/Functions Process", () =>
            {
                rawMacros = units.SelectMany(unit => unit.PreprocessedEntities
                    .OfType<MacroDefinition>()
                    .Where(x => !string.IsNullOrWhiteSpace(x.Expression))
                    .Select(macro => new MacroDefinitionRaw
                    {
                        Name = macro.Name,
                        ExpressionText = macro.Expression
                    }))
                    .GroupBy(x => x.Name)
                    .Select(x => x.Last())
                    .ToArray();

                foreach (var translationUnit in units)
                {
                    EnumerationProcessor.Process(translationUnit);
                    StructureProcessor.Process(translationUnit);
                    FunctionProcessor.Process(translationUnit);
                }
            });

            MetricHelper.RecordTime("MacroPostProcess", () =>
            {
                var wellKnownMacros = new Dictionary<string, string>();
                wellKnownMacros.Add("FFERRTAG", "int");
                wellKnownMacros.Add("MKTAG", "int");
                wellKnownMacros.Add("UINT64_C", "ulong");
                wellKnownMacros.Add("AV_VERSION_INT", "int");
                wellKnownMacros.Add("AV_VERSION", "string");

                EnumerationDefinition[] enums = Units.OfType<EnumerationDefinition>().ToArray();
                IEnumerable<Definitions.MacroDefinition> macros = MacroPostProcessor.Process(rawMacros, enums, TypeAliasMap, wellKnownMacros);

                (IEnumerable<Definitions.MacroDefinition> processedMacros, IEnumerable<EnumerationDefinition> macroEnums) = MacroEnumPostProcessor.Process(macros);
                foreach (EnumerationDefinition @enum in macroEnums)
                {
                    AddUnit(@enum);
                }

                foreach (Definitions.MacroDefinition macroDefinition in processedMacros)
                {
                    AddUnit(macroDefinition);
                }
            });
        }
    }
}