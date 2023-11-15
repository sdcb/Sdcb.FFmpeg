#nullable enable

using Sdcb.FFmpeg.AutoGen.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Sdcb.FFmpeg.AutoGen.Processors
{
    internal static class MacroEnumPostProcessor
    {
        public static (IReadOnlyList<MacroDefinitionBase>, IReadOnlyList<EnumerationDefinition>) Process(IEnumerable<MacroDefinitionBase> macros)
        {
            MacroEnumDef[] knownConstEnums = new MacroEnumDef[]
            {
                MacroEnumDef.MakeFlags("AV_CODEC_FLAG_"),
                MacroEnumDef.MakeFlags("AV_CODEC_FLAG2_"),
                //new ("SLICE_FLAG_", "SLICE_FLAG"),
                MacroEnumDef.MakeFlags("AV_CH_"),
                MacroEnumDef.MakeFlags("AV_CODEC_CAP_"), 
                //new ("FF_MB_DECISION_", "FFMacroblockDecision"),
                //new ("FF_CMP_", "FFComparison"),
                //new ("PARSER_FLAG_", "ParserFlags"),
                MacroEnumDef.MakeFlags("AVIO_FLAG_"),
                MacroEnumDef.Make("FF_PROFILE_") with { TypeHint = "int" },
                MacroEnumDef.MakeFlags("AVSEEK_FLAG_"),
                MacroEnumDef.MakeFlagsAdditional("AVSEEK_", new EnumerationItem[]
                {
                    EnumerationItem.MakeFake("Begin", "SEEK_SET", 0),
                    EnumerationItem.MakeFake("Current", "SEEK_CUR", 1),
                    EnumerationItem.MakeFake("End", "SEEK_END", 2),
                }, IsBegin: true),
                MacroEnumDef.MakeFlags("AV_PIX_FMT_FLAG_"),
                MacroEnumDef.MakeFlags("AV_OPT_FLAG_"),
                MacroEnumDef.MakeFlags("AV_OPT_SEARCH_"),
                new MacroEnumDef("AV_LOG_", "LogFlags", Only: HashSet("AV_LOG_SKIP_REPEATED", "AV_LOG_PRINT_LEVEL")),
                new MacroEnumDef("AV_LOG_", "LogLevel", Except: HashSet("AV_LOG_C")),
                new MacroEnumDef("AV_DICT_", "AV_DICT_READ", Only: HashSet("AV_DICT_MATCH_CASE", "AV_DICT_IGNORE_SUFFIX"), IsFlags: true),
                new MacroEnumDef("AV_DICT_", "AV_DICT_WRITE", IsFlags: true),
                MacroEnumDef.MakeFlags("AVSTREAM_INIT_IN_"),
                //("AV_CPU_FLAG_", "CpuFlags"),
                //("AV_PKT_FLAG_", "PacketFlags"),
                MacroEnumDef.MakeFlags("AVFMT_FLAG_"),
                MacroEnumDef.MakeFlags("AVFMT_AVOID_NEG_TS_"),
                MacroEnumDef.MakeFlags("AVFMT_"),
                MacroEnumDef.MakeFlags("SWS_CS_"),
                MacroEnumDef.MakeFlagsExcept("SWS_", HashSet("SWS_MAX_REDUCE_CUTOFF")),
                MacroEnumDef.MakeFlags("AVFILTER_FLAG_"), 
                MacroEnumDef.MakeFlags("AVFILTER_CMD_FLAG_"),
                MacroEnumDef.MakeFlags("AV_BUFFERSINK_FLAG_"),
            };
            Dictionary<string, MacroEnumDef> knownConstEnumMapping = knownConstEnums.ToDictionary(k => k.EnumName, v => v);

            List<MacroDefinitionBase> processedMacros = new();
            List<EnumerationDefinition> macroEnums = new();
            macros
                .GroupBy(x => knownConstEnums.FirstOrDefault(known => known.Match(x.Name)) switch
                {
                    null => null,
                    MacroEnumDef prefix => prefix.EnumName,
                })
                .ForEach((group, _) =>
                {
                    if (group.Key == null)
                    {
                        processedMacros.AddRange(group);
                    }
                    else
                    {
                        macroEnums.Add(MakeMacroEnum(group!, knownConstEnumMapping[group.Key]));
                    }
                });

            return (processedMacros, macroEnums);
        }

        private static HashSet<string> HashSet(params string[] strings) => strings.ToHashSet();

        private static EnumerationDefinition MakeMacroEnum(IGrouping<string, MacroDefinitionBase> group, MacroEnumDef enumDef)
        {
            List<MacroDefinitionGood> macros = group.OfType<MacroDefinitionGood>().ToList();

            Dictionary<string, string> macroShortcutMapping = macros
                .OrderByDescending(k => k.Name.Length)
                .ToDictionary(k => k.Name, v => StringExtensions.EnumNameTransform(v.Name[enumDef.Prefix.Length..]));

            IEnumerable<EnumerationItem> existingItems = macros.Select(macro => new EnumerationItem
            {
                Name = macroShortcutMapping[macro.Name],
                RawName = macro.Name,
                Value = ExpressionTransform(macro.ExpressionText, macroShortcutMapping),
                XmlDocument = macro.Name,
            });
            IEnumerable<EnumerationItem> items = enumDef.AdditionalValues != null ? enumDef.AdditionalValues.CombineExistingItems(existingItems) : existingItems;

            return new EnumerationDefinition
            {
                Name = enumDef.EnumName,
                XmlDocument = $"Macro enum, prefix: {enumDef.Prefix}",
                IsFlags = enumDef.IsFlags,
                TypeName = enumDef.TypeHint ?? DeterminBestTypeForMacroEnum(macros
                    .Select(x => x.TypeName)
                    .ToHashSet()),
                Obsoletion = new Obsoletion { IsObsolete = false },
                Items = items.ToArray(),
            };

            static string ExpressionTransform(string expression, Dictionary<string, string> mapping)
            {
                foreach (KeyValuePair<string, string> kv in mapping)
                {
                    expression = expression.Replace(kv.Key, kv.Value);
                }
                return expression;
            }

            static string DeterminBestTypeForMacroEnum(HashSet<string> allTypes)
            {
                string[] priorities = new[]
                {
                    "ulong",
                    "long",
                    "uint",
                    "int",
                    "ushort",
                    "short",
                };

                foreach (string prior in priorities)
                {
                    if (allTypes.Contains(prior))
                        return prior;
                }
                return "int";
            }
        }
    }

    internal record MacroEnumDef(string Prefix, string EnumName, string? TypeHint = null, HashSet<string>? Only = default, HashSet<string>? Except = default, AdditionalMacroDef? AdditionalValues = null, bool IsFlags = false)
    {
        public bool Match(string name) =>
            name.StartsWith(Prefix) &&
            (Except == null || !Except.Contains(name)) &&
            (Only == null || Only.Contains(name));

        internal static MacroEnumDef Make(string prefix) => new MacroEnumDef(prefix, FindName(prefix));

        internal static MacroEnumDef MakeFlags(string prefix) => new MacroEnumDef(prefix, FindName(prefix), IsFlags: true);

        internal static MacroEnumDef MakeFlagsAdditional(string prefix, EnumerationItem[] additionalValues, bool IsBegin) => new MacroEnumDef(prefix, FindName(prefix), AdditionalValues: new (IsBegin, additionalValues), IsFlags: true);

        internal static MacroEnumDef MakeFlagsExcept(string prefix, HashSet<string> except) => new MacroEnumDef(prefix, FindName(prefix), Except: except, IsFlags: true);

        internal static string FindName(string prefix) => prefix.EndsWith('_') ? prefix.Substring(0, prefix.Length - 1) : throw new NotSupportedException("Prefix must ends with _");
    }

    internal record AdditionalMacroDef(bool IsBegin, EnumerationItem[] Items)
    {
        public IEnumerable<EnumerationItem> CombineExistingItems(IEnumerable<EnumerationItem> existingItems)
        {
            if (IsBegin)
            {
                foreach (EnumerationItem item in Items)
                {
                    yield return item;
                }
            }

            foreach (EnumerationItem item in existingItems)
            {
                yield return item;
            }

            if (!IsBegin)
            {
                foreach (EnumerationItem item in Items)
                {
                    yield return item;
                }
            }
        }
    }
}