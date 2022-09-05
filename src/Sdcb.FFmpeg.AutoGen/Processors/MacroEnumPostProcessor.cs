#nullable enable

using Sdcb.FFmpeg.AutoGen.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sdcb.FFmpeg.AutoGen.Processors
{
    internal static class MacroEnumPostProcessor
    {
        public static (IReadOnlyList<MacroDefinitionBase>, IReadOnlyList<EnumerationDefinition>) Process(IEnumerable<MacroDefinitionBase> macros)
        {
            MacroEnumDef[] knownConstEnums = new MacroEnumDef[]
            {
                new ("AV_CODEC_FLAG_", "AV_CODEC_FLAG", IsFlags: true),
                new ("AV_CODEC_FLAG2_", "AV_CODEC_FLAG2", IsFlags: true),
                //new ("SLICE_FLAG_", "SLICE_FLAG"),
                new ("AV_CH_", "AV_CH", IsFlags: true, Except: HashSet("AV_CH_LAYOUT_NATIVE")),
                new ("AV_CODEC_CAP_", "AV_CODEC_CAP", IsFlags: true),
                //new ("FF_MB_DECISION_", "FFMacroblockDecision"),
                //new ("FF_CMP_", "FFComparison"),
                //new ("PARSER_FLAG_", "ParserFlags"),
                new ("AVIO_FLAG_", "AVIO_FLAG", IsFlags: true),
                //new ("FF_PROFILE_", "FFProfile"),
                //new ("AVSEEK_FLAG_", "SeekFlags"),
                //new ("AV_PIX_FMT_FLAG_", "PixelFormatFlags"),
                new ("AV_OPT_FLAG_", "AV_OPT_FLAG", IsFlags: true),
                new ("AV_OPT_SEARCH_", "AV_OPT_SEARCH", IsFlags: true),
                new ("AV_LOG_", "LogFlags", Only: HashSet("AV_LOG_SKIP_REPEATED", "AV_LOG_PRINT_LEVEL")),
                new ("AV_LOG_", "LogLevel", Except: HashSet("AV_LOG_C")),
                new ("AV_DICT_", "AV_DICT_READ", Only: HashSet("AV_DICT_MATCH_CASE", "AV_DICT_IGNORE_SUFFIX"), IsFlags: true),
                new ("AV_DICT_", "AV_DICT_WRITE", IsFlags: true),
                //new ("AV_CPU_FLAG_", "CpuFlags"),
                //new ("AV_PKT_FLAG_", "PacketFlags"),
                //new ("AVFMT_FLAG_", "FormatFlags"), 
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

            return new EnumerationDefinition
            {
                Name = enumDef.EnumName, 
                XmlDocument = $"Macro enum, prefix: {enumDef.Prefix}", 
                IsFlags = enumDef.IsFlags,
                TypeName = DeterminBestTypeForMacroEnum(macros
                    .Select(x => x.TypeName)
                    .ToHashSet()), 
                Obsoletion = new Obsoletion { IsObsolete = false }, 
                Items = macros.Select(macro => new EnumerationItem
                {
                    Name = macroShortcutMapping[macro.Name], 
                    RawName = macro.Name, 
                    Value = ExpressionTransform(macro.ExpressionText, macroShortcutMapping),
                    XmlDocument = macro.Name, 
                }).ToArray(), 
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

    public record MacroEnumDef(string Prefix, string EnumName, HashSet<string>? Only = default, HashSet<string>? Except = default, bool IsFlags = false)
    {
        public bool Match(string name) =>
            name.StartsWith(Prefix) &&
            (Except == null || !Except.Contains(name)) &&
            (Only == null || Only.Contains(name));
    }
}