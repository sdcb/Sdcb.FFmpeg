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
    internal class MacroPostProcessor
    {
        private static readonly Regex EolEscapeRegex =
            new(@"\\\s*[\r\n|\r|\n]\s*", RegexOptions.Compiled | RegexOptions.Multiline);

        private readonly ASTProcessor _astProcessor;
        private Dictionary<string, IExpression> _macroExpressionMap;
        private Dictionary<string, string> _enums;

        public MacroPostProcessor(ASTProcessor astProcessor) => _astProcessor = astProcessor;

        public void Process(IReadOnlyList<MacroDefinition> macros, IReadOnlyList<EnumerationDefinition> enums)
        {
            _enums = enums
                .SelectMany(x => x.Items.Select(i => new
                {
                    Name = i.Name, 
                    Type = x.TypeName, 
                }))
                .ToDictionary(k => k.Name, v => v.Type);
            Func<string, IExpression> parser = ClangMacroParser.MakeParser();
            Stopwatch sw = Stopwatch.StartNew();
            _macroExpressionMap = macros
                .ToDictionary(k => k.Name, v =>
                {
                    try
                    {
                        return parser(v.Expression);
                    }
                    catch (NotSupportedException e)
                    {
                        Console.WriteLine(e.ToString());
                        return null;
                    }
                });
            int goodCount = _macroExpressionMap.Values.Count(x => x != null);
            int unsupportedFailCount = _macroExpressionMap.Count - goodCount;
            Console.WriteLine($"Parsing macro done, good={goodCount}, unsupported={unsupportedFailCount}");

            foreach (var macro in macros) Process(macro);
        }

        private void Process(MacroDefinition macro)
        {
            macro.Expression = CleanUp(macro.Expression);

            if (!_macroExpressionMap.TryGetValue(macro.Name, out var expression) || expression == null) return;

            var typeOrAlias = DeduceTypeOne(expression, _macroExpressionMap, _enums, _astProcessor.WellKnownMacros);
            if (typeOrAlias == null) return;

            //IExpression rewritedExpression = Rewrite(expression);

            macro.TypeName = typeOrAlias.ToString();
            macro.Content = $"{macro.Name} = {macro.Expression}";
            macro.Expression = expression.Serialize();
            macro.IsConst = IsConst(expression);
            macro.IsValid = TypeHelper.IsKnownType(typeOrAlias);
        }

        private static string CleanUp(string expression)
        {
            var oneLine = EolEscapeRegex.Replace(expression, string.Empty);
            var trimmed = oneLine.Trim();
            return trimmed;
        }

        private static string DeduceTypeOne(IExpression expression, Dictionary<string, IExpression> macroExpressionMap, Dictionary<string, string> enumTypeMapping, Dictionary<string, string> wellKnownMacroMapping)
        {
            return DeduceType(expression);

            string DeduceType(IExpression expression) => expression switch
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
                FunctionCallExpression func => throw new NotImplementedException(),
                IdentifierExpression id => DeduceTypeForId(id),
                NegativeExpression e => DeduceType(e.Val),
#pragma warning disable CS8509 // switch 表达式不会处理属于其输入类型的所有可能值(它并非详尽无遗)。
                NumberLiteralExpression e => e.Number switch
                {
                    { Info: NumberLiteralResultFlags.IsDecimal | NumberLiteralResultFlags.HasIntegerPart } x => "int",
                    { Info: NumberLiteralResultFlags.IsDecimal | NumberLiteralResultFlags.HasIntegerPart | NumberLiteralResultFlags.HasMinusSign } x => "int",
                    { Info: NumberLiteralResultFlags.HasIntegerPart | NumberLiteralResultFlags.IsHexadecimal } x => "int",
                    { Info: NumberLiteralResultFlags.IsDecimal | NumberLiteralResultFlags.HasIntegerPart | NumberLiteralResultFlags.HasFraction } x => "double",
                    { Info: NumberLiteralResultFlags.IsDecimal | NumberLiteralResultFlags.HasIntegerPart | NumberLiteralResultFlags.HasFraction | NumberLiteralResultFlags.HasExponent } x => "double",
                    { SuffixChar1: 'f' } x => "float",
                    { SuffixLength: 1, SuffixChar1: 'L' } x => "int",
                    { SuffixLength: 2, SuffixChar1: 'L', SuffixChar2: 'L' } x => "long",
                    { SuffixLength: 3, SuffixChar1: 'U', SuffixChar2: 'L', SuffixChar3: 'L' } x => "ulong",
                },
#pragma warning restore CS8509
                ParentheseExpression p => DeduceType(p.Content),
                StringConcatExpression => "string",
                StringLiteralExpression => "string",
                TypeConvertExpression tc => tc.DestType,
                _ => throw new NotSupportedException()
            };

            string DeduceTypeForId(IdentifierExpression expression)
            {
                if (macroExpressionMap.TryGetValue(expression.Name, out IExpression nested) && nested != null)
                {
                    return DeduceType(nested);
                }

                if (enumTypeMapping.TryGetValue(expression.Name, out string val))
                {
                    return val;
                }

                return wellKnownMacroMapping.TryGetValue(expression.Name, out string alias) ? alias : null;
            }
        }
        

        //private IExpression Rewrite(IExpression expression)
        //{
        //    switch (expression)
        //    {
        //        case BinaryExpression e:
        //        {
        //            IExpression left = Rewrite(e.Left);
        //            IExpression right = Rewrite(e.Right);
        //            TypeOrAlias leftType = DeduceType(left);
        //            TypeOrAlias rightType = DeduceType(right);

        //            if (e.IsBitwise && leftType.Precedence != rightType.Precedence)
        //            {
        //                var toType = leftType.Precedence > rightType.Precedence ? rightType : leftType;
        //                if (leftType != toType) left = new CastExpression(toType.ToString(), left);
        //                if (rightType != toType) right = new CastExpression(toType.ToString(), right);
        //            }

        //            return new BinaryExpression(left, e.OperationType, right);
        //        }
        //        case UnaryExpression e: return new UnaryExpression(e.OperationType, Rewrite(e.Operand));
        //        case CastExpression e: return new CastExpression(e.TargetType, Rewrite(e.Operand));
        //        case CallExpression e: return new CallExpression(e.Name, e.Arguments.Select(Rewrite));
        //        case VariableExpression e: return e;
        //        case ConstantExpression e: return e;
        //        default: return expression;
        //    }
        //}

        private bool IsConst(IExpression expression)
        {
            return expression switch
            {
                BinaryExpression e => IsConst(e.Left) && IsConst(e.Right), 
                CharLiteralExpression => true,
                FunctionCallExpression func => throw new NotImplementedException(),
                IdentifierExpression id => _macroExpressionMap.TryGetValue(id.Name, out var nested) && nested != null && IsConst(nested),
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
