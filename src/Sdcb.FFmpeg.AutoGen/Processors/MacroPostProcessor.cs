#nullable enable
#pragma warning disable CS8509 // switch 表达式不会处理属于其输入类型的所有可能值(它并非详尽无遗)。
#pragma warning disable CS8655 // Switch 表达式不会处理某些为 null 的输入。
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Sdcb.FFmpeg.AutoGen.ClangMarcroParsers;
using Sdcb.FFmpeg.AutoGen.Definitions;
using Sdcb.FFmpeg.AutoGen.ClangMarcroParsers.Units;
using static FParsec.CharParsers;

namespace Sdcb.FFmpeg.AutoGen.Processors
{
    internal static class MacroPostProcessor
    {
        private static readonly Regex EolEscapeRegex =
            new(@"\\\s*[\r\n|\r|\n]\s*", RegexOptions.Compiled | RegexOptions.Multiline);

        public static void Process(IReadOnlyList<MacroDefinition> macros, IReadOnlyList<EnumerationDefinition> enums, Dictionary<string, string> wellKnownMacros)
        {
            Func<string, IExpression> parser = ClangMacroParser.MakeParser();
            Stopwatch sw = Stopwatch.StartNew();
            Dictionary<string, IExpression?> macroExpressionMap = macros
                .ToDictionary(k => k.Name, v =>
                {
                    try
                    {
                        return parser(v.Expression);
                    }
                    catch (NotSupportedException e)
                    {
                        //Console.WriteLine(e.ToString());
                        return null;
                    }
                });
            int goodCount = macroExpressionMap.Values.Count(x => x != null);
            Console.WriteLine($"Parsing macro done, elapsed={sw.ElapsedMilliseconds}ms, total/good/failed={macroExpressionMap.Count}/{goodCount}/{macroExpressionMap.Count - goodCount}");
            sw.Restart();

            var rewriter = MakeRewriter(macroExpressionMap, enums);
            var isConst = MakeIsConst(macroExpressionMap);
            var typeDeductor = MakeDeduceType(macroExpressionMap, enums, wellKnownMacros);
            foreach (MacroDefinition macro in macros)
            {
                macro.Expression = CleanUp(macro.Expression);

                if (!macroExpressionMap.TryGetValue(macro.Name, out var expression) || expression == null) continue;

                string? type = typeDeductor(expression);
                if (type == null) continue;

                IExpression rewritedExpression = rewriter(expression);

                macro.TypeName = type;
                macro.Content = $"{macro.Name} = {macro.Expression}";
                macro.Expression = rewritedExpression.Serialize();
                macro.IsConst = isConst(rewritedExpression);
                macro.IsValid = TypeHelper.IsKnownType(type);
            }
            Console.WriteLine($"Macro postprocess done, elapsed={sw.ElapsedMilliseconds}ms, total/valid={macros.Count}/{macros.Count(x => x.IsValid)}");
        }

        static string CleanUp(string expression)
        {
            var oneLine = EolEscapeRegex.Replace(expression, string.Empty);
            var trimmed = oneLine.Trim();
            return trimmed;
        }

        static Func<IExpression, string?> MakeDeduceType(Dictionary<string, IExpression?> macroExpressionMap, IReadOnlyList<EnumerationDefinition> enums, Dictionary<string, string> wellKnownMacroMapping)
        {
            Dictionary<string, string> enumTypeMapping = enums
                .SelectMany(k => k.Items.Select(v => new { Key = v.RawName, Value = k.TypeName }))
                .ToDictionary(k => k.Key, v => v.Value);
            return DeduceType;

            string? DeduceType(IExpression expression) => expression switch
            {
                BinaryExpression e => e switch
                {
                    { Op: ">" or "<" or "==" or "!=" or "&&" or "||" } => "bool",
                    _ => (DeduceType(e.Left), DeduceType(e.Right)) switch
                    {
                        (string left, string right) => TypeHelper.CalculatePrecedence(left) < TypeHelper.CalculatePrecedence(left) ? left : right,
                    }
                },
                CharLiteralExpression => "char",
                FunctionCallExpression func => func.FunctionName switch
                {
                    "AV_VERSION" => "string",
                    "AV_CHANNEL_LAYOUT_MASK" => "AVChannelLayout", 
                    _ => null, 
                }, 
                IdentifierExpression id => DeduceTypeForId(id),
                NegativeExpression e => DeduceType(e.Val) switch
                {
                    "uint" => "int", 
                    "ulong" => "long", 
                    var x => x, 
                },
                NumberLiteralExpression e => e.Number switch
                {
                    { Info: NumberLiteralResultFlags.IsDecimal | NumberLiteralResultFlags.HasIntegerPart } x => "int",
                    { Info: NumberLiteralResultFlags.IsDecimal | NumberLiteralResultFlags.HasIntegerPart | NumberLiteralResultFlags.HasMinusSign } x => "int",
                    { Info: NumberLiteralResultFlags.HasIntegerPart | NumberLiteralResultFlags.IsHexadecimal } x => "uint",
                    { Info: NumberLiteralResultFlags.IsDecimal | NumberLiteralResultFlags.HasIntegerPart | NumberLiteralResultFlags.HasFraction } x => "double",
                    { Info: NumberLiteralResultFlags.IsDecimal | NumberLiteralResultFlags.HasIntegerPart | NumberLiteralResultFlags.HasFraction | NumberLiteralResultFlags.HasExponent } x => "double",
                    { SuffixChar1: 'f' } x => "float",
                    { SuffixLength: 1, SuffixChar1: 'L' } x => "int",
                    { SuffixLength: 1, SuffixChar1: 'U' } x => "uint",
                    { SuffixLength: 2, SuffixChar1: 'L', SuffixChar2: 'L' } x => "long",
                    { SuffixLength: 3, SuffixChar1: 'U', SuffixChar2: 'L', SuffixChar3: 'L' } x => "ulong",
                },
                ParentheseExpression p => DeduceType(p.Content),
                StringConcatExpression => "string",
                StringLiteralExpression => "string",
                TypeConvertExpression tc => tc.DestType,
            };

            string? DeduceTypeForId(IdentifierExpression expression)
            {
                if (macroExpressionMap.TryGetValue(expression.Name, out IExpression? nested) && nested != null)
                {
                    return DeduceType(nested);
                }

                if (enumTypeMapping.TryGetValue(expression.Name, out string? val))
                {
                    return val;
                }

                return wellKnownMacroMapping.TryGetValue(expression.Name, out string? alias) ? alias : null;
            }
        }

        static Func<IExpression, IExpression> MakeRewriter(Dictionary<string, IExpression?> macros, IReadOnlyList<EnumerationDefinition> enums)
        {
            Dictionary<string, (EnumerationDefinition Enum, EnumerationItem Item)> enumMapping = enums
                .SelectMany(x => x.Items.Select(v => new { Key = v.RawName, Enum = x, Item = v }))
                .ToDictionary(k => k.Key, v => (v.Enum, v.Item));

            return Rewrite;

            IExpression Rewrite(IExpression src) => src switch
            {
                BinaryExpression e => new BinaryExpression(Rewrite(e.Left), e.Op, Rewrite(e.Right)),
                FunctionCallExpression func => func.FunctionName switch
                {
                    "AV_STRINGIFY" => IExpression.MakeStringLiteral(func.Arguments.OfType<IdentifierExpression>().Single().Name), 
                    _ => new FunctionCallExpression(func.FunctionName, func.Arguments.Select(Rewrite).ToArray()),
                },
                IdentifierExpression id => id switch
                {
                    var _ when macros.TryGetValue(id.Name, out IExpression? value) && value != null => id,
                    var _ when enumMapping.TryGetValue(id.Name, out (EnumerationDefinition Enum, EnumerationItem Item) v) => IExpression.MakeTypeConvert(v.Enum.TypeName, IExpression.MakeIdentifier($"{v.Enum.Name}.{v.Item.Name}")),
                    var x => x,
                },
                NegativeExpression e => new NegativeExpression(Rewrite(e.Val)),
                ParentheseExpression p => Rewrite(p.Content),
                StringConcatExpression e => new StringConcatExpression(e.Str, Rewrite(e.Exp)),
                TypeConvertExpression tc => new TypeConvertExpression(tc.DestType, Rewrite(tc.Exp)),
                var x => x, 
            };
        }

        static Func<IExpression, bool> MakeIsConst(Dictionary<string, IExpression?> macroExpressionMap)
        {
            return IsConst;

            bool IsConst(IExpression expression) => expression switch
            {
                BinaryExpression e => IsConst(e.Left) && IsConst(e.Right), 
                CharLiteralExpression => true,
                FunctionCallExpression func => false,
                IdentifierExpression id => macroExpressionMap!.TryGetValue(id.Name, out var nested) && nested != null && IsConst(nested),
                NegativeExpression e => IsConst(e.Val),
                NumberLiteralExpression => true, 
                ParentheseExpression p => IsConst(p.Content),
                StringConcatExpression p => IsConst(p.Exp),
                StringLiteralExpression => true,
                TypeConvertExpression => false,
                _ => throw new NotSupportedException()
            };
        }
    }
}