#nullable enable

using Sdcb.FFmpeg.AutoGen.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sdcb.FFmpeg.AutoGen.Processors
{
    internal static class MacroEnumPostProcessor
    {
        public static (IReadOnlyList<MacroDefinition>, IReadOnlyList<EnumerationDefinition>) Process(IEnumerable<MacroDefinition> macros)
        {
            MacroEnumDef[] knownConstEnums = new MacroEnumDef[]
            {
                            new ("AV_CODEC_FLAG_", "AV_CODEC_FLAG"),
                            new ("AV_CODEC_FLAG2_", "AV_CODEC_FLAG2"),
                            //new ("SLICE_FLAG_", "SLICE_FLAG"),
                            //new ("AV_CH_", "Channels"),
                            //new ("AV_CODEC_CAP_", "CodecCompability"),
                            //new ("FF_MB_DECISION_", "FFMacroblockDecision"),
                            //new ("FF_CMP_", "FFComparison"),
                            //new ("PARSER_FLAG_", "ParserFlags"),
                            new ("AVIO_FLAG_", "AVIO_FLAG"),
                            //new ("FF_PROFILE_", "FFProfile"),
                            //new ("AVSEEK_FLAG_", "SeekFlags"),
                            //new ("AV_PIX_FMT_FLAG_", "PixelFormatFlags"),
                            new ("AV_OPT_FLAG_", "AV_OPT_FLAG"),
                            new ("AV_LOG_", "LogFlags", Only: new []{ "AV_LOG_SKIP_REPEATED", "AV_LOG_PRINT_LEVEL" }.ToHashSet()),
                            new ("AV_LOG_", "LogLevel", Except: new []{ "AV_LOG_C" }.ToHashSet()),
                //new ("AV_CPU_FLAG_", "CpuFlags"),
                //new ("AV_PKT_FLAG_", "PacketFlags"),
                //new ("AVFMT_FLAG_", "FormatFlags"), 
            };
            Dictionary<string, MacroEnumDef> knownConstEnumMapping = knownConstEnums.ToDictionary(k => k.EnumName, v => v);

            List<MacroDefinition> processedMacros = new();
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

        private static EnumerationDefinition MakeMacroEnum(IGrouping<string, MacroDefinition> group, MacroEnumDef enumDef)
        {
            List<MacroDefinition> macros = group.ToList();
            HashSet<string> allTypes = macros
                .Select(x => x.TypeName)
                .Distinct()
                .ToHashSet();
            string typeExpr = DetectBestTypeForEnum(allTypes) switch
            {
                "int" => "",
                var x => $" : {x}",
            };

            Dictionary<string, string> macroShortcutMapping = macros
                .OrderByDescending(k => k.Name.Length)
                .ToDictionary(k => k.Name, v => StringExtensions.EnumNameTransform(v.Name[enumDef.Prefix.Length..]));

            return new EnumerationDefinition
            {
                Name = enumDef.EnumName, 
                XmlDocument = $"Macro enum, prefix: {enumDef.Prefix}", 
                IsFlags = enumDef.IsFlags,
                TypeName = typeExpr, 
                Obsoletion = new Obsoletion { IsObsolete = false }, 
                Items = group.Select(macro => new EnumerationItem
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

            static string DetectBestTypeForEnum(HashSet<string> allTypes)
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
}