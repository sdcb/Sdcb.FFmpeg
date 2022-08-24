#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using CppSharp.AST;
using Sdcb.FFmpeg.AutoGen.ClangMarcroParsers.Units;
using Sdcb.FFmpeg.AutoGen.Definitions;
using MacroDefinition = CppSharp.AST.MacroDefinition;

namespace Sdcb.FFmpeg.AutoGen.Processors
{
    internal class ASTProcessor
    {
        public ASTProcessor(Dictionary<string, FunctionExport> functionExportMap)
        {
            IgnoreUnitNames = new HashSet<string>();
            FunctionProcessor = new FunctionProcessor(this);
            StructureProcessor = new StructureProcessor(this);
            EnumerationProcessor = new EnumerationProcessor(this);
            FunctionExportMap = functionExportMap;
        }

        public HashSet<string> IgnoreUnitNames { get; }

        public EnumerationProcessor EnumerationProcessor { get; }
        public StructureProcessor StructureProcessor { get; }
        public FunctionProcessor FunctionProcessor { get; }

        public Dictionary<string, FunctionExport> FunctionExportMap { get; }

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
            MacroDefinitionRaw[]? rawMacros = default;

            MetricHelper.RecordTime("Macro/Enumeration/Structure/Functions Process", () =>
            {
                rawMacros = units.SelectMany(unit => unit.PreprocessedEntities
                        .OfType<MacroDefinition>()
                        .Where(x => !string.IsNullOrWhiteSpace(x.Expression))
                        .Select(macro => new MacroDefinitionRaw(macro.Name, macro.Expression))
                    )
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
                List<EnumerationDefinition> enums = Units.OfType<EnumerationDefinition>().ToList();
                (IEnumerable<MacroDefinitionBase> macros, Dictionary<string, IExpression?> macroParsedMap) = MacroPostProcessor.Process(rawMacros!, enums);

                (IEnumerable<MacroDefinitionBase> processedMacros, IEnumerable<EnumerationDefinition> macroEnums) = MacroEnumPostProcessor.Process(macros);
                enums.AddRange(macroEnums);
                foreach (EnumerationDefinition @enum in macroEnums)
                {
                    AddUnit(@enum);
                }

                foreach (MacroDefinitionBase macroDefinition in MacroPostProcessor.MakeExpression(processedMacros, enums, macroParsedMap))
                {
                    AddUnit(macroDefinition);
                }
            });
        }
    }
}