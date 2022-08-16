using FParsec.CSharp;
using Microsoft.FSharp.Core;
using Sdcb.FFmpeg.AutoGen.ClangMarcroParser.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using static FParsec.CharParsers;
using static FParsec.CSharp.CharParsersCS; // pre-defined parsers
using static FParsec.CSharp.PrimitivesCS; // combinator functions

namespace Sdcb.FFmpeg.AutoGen.ClangMarcroParsers
{
    public class ClangMacroParser
    {
        public static Func<string, Expression> MakeParser()
        {
            HashSet<string> typeLiterals = "int64_t,UINT64_C".Split(',').OrderByDescending(x => x.Length).ToHashSet();
            FSharpFunc<FParsec.CharStream<Unit>, FParsec.Reply<string>> notReserved(string id) => typeLiterals.Contains(id) ? Zero<string>() : Return(id);
            var identifier = Purify(Many1Chars(Choice(Letter, CharP('_')), Choice(Letter, CharP('_'), Digit))).AndTry(notReserved).And(WS).Lbl("identifier");
            var typeSyntax = Choice(typeLiterals.Select(t => StringP(t)).ToArray()).And(WS).Label("typeSyntax");

            NumberLiteralOptions numberLiteralOptions =
                (NumberLiteralOptions.AllowSuffix | NumberLiteralOptions.AllowHexadecimal | NumberLiteralOptions.DefaultFloat)
                & ~(NumberLiteralOptions.AllowMinusSign | NumberLiteralOptions.AllowPlusSign);

            var parser = new OPPBuilder<Unit, Expression, Unit>()
                .WithOperators(ops => ops
                    .AddInfix(">", 10, WS, (x, y) => Expression.MakeBinary(x, new Operator(">"), y))
                    .AddInfix("<", 10, WS, (x, y) => Expression.MakeBinary(x, new Operator("<"), y))
                    .AddInfix("<<", 20, WS, (x, y) => Expression.MakeBinary(x, new Operator("<<"), y))
                    .AddInfix(">>", 20, WS, (x, y) => Expression.MakeBinary(x, new Operator(">>"), y))
                    .AddInfix("|", 30, WS, (x, y) => Expression.MakeBinary(x, new Operator("|"), y))
                    .AddInfix("+", 30, WS, (x, y) => Expression.MakeBinary(x, new Operator("+"), y))
                    .AddInfix("-", 30, WS, (x, y) => Expression.MakeBinary(x, new Operator("-"), y))
                    .AddInfix("*", 40, WS, (x, y) => Expression.MakeBinary(x, new Operator("*"), y))
                    .AddInfix("/", 40, WS, (x, y) => Expression.MakeBinary(x, new Operator("/"), y))
                    .AddPrefix("-", 40, WS, (x) => Expression.MakeNegative(x))
                    )
                .WithImplicitOperator(50, (e1, e2) => Expression.FromImplicitBinary(e1, e2))
                .WithTerms((FSharpFunc<FParsec.CharStream<Unit>, FParsec.Reply<Expression>> term) =>
                {
                    var parenthese1 = Between(CharP('(').And(WS), term, CharP(')').And(WS));
                    var parentheseN = Between(CharP('(').And(WS), Many(term, CharP(',').And(WS)), CharP(')').And(WS));

                    return PrimitivesCS.Choice(
                        Try(Between(CharP('(').And(WS), typeSyntax, CharP(')'))).And(term).And(WS).Map((id, val) => Expression.MakeTypeConvert(id, val)),
                        Try(identifier.And(parentheseN)).Map((id, val) => Expression.MakeFunctionCall(id, val.ToArray())),
                        Between('\'', AnyChar, '\'').And(WS).Map(x => Expression.MakeCharLiteral(x)),
                        Between('"', ManyChars(NoneOf("\"")), '"').And(WS).Map(x => Expression.MakeStringLiteral(x)),
                        NumberLiteral(numberLiteralOptions, "Number").And(WS).Map(x => Expression.MakeNumberLiteral(x)),
                        parenthese1.Map(x => Expression.MakeParenthese(x)),
                        typeSyntax.And(parenthese1).Map((id, val) => Expression.MakeTypeConvert(id, val)),
                        identifier.Map(x => Expression.MakeIdentifier(x))
                    ).Label("expression");
                })
                .Build()
                .ExpressionParser;

            return (string str) => parser.ParseString(Preprocess(str)) switch
            {
                { Status: FParsec.ReplyStatus.Ok } x => x.Result, 
                var x => throw new Exception(string.Join("\n", FParsec.ErrorMessageList.ToHashSet(x.Error).Select(x => x.Type.ToString())))
            };

            static string Preprocess(string raw) => raw.Replace("\\\n", " ");
        }
    }
}
