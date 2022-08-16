using System;
using static FParsec.CharParsers;

namespace Sdcb.FFmpeg.AutoGen.ClangMarcroParsers.Units
{
    public interface IExpression
    {
        public string Serialize();

        public static IExpression MakeNumberLiteral(NumberLiteral number) => new NumberLiteralExpression(number);
        public static IExpression MakeStringLiteral(string text) => new StringLiteralExpression(text);
        public static IExpression MakeStringConcat(StringLiteralExpression str, IExpression exp) => new StringConcatExpression(str, exp);
        public static IExpression MakeCharLiteral(char c) => new CharLiteralExpression(c);
        public static IExpression MakeNegative(IExpression e) => new NegativeExpression(e);
        public static IExpression MakeIdentifier(string identifier) => new IdentifierExpression(identifier);
        public static IExpression MakeBinary(IExpression left, string op, IExpression right) => new BinaryExpression(left, op, right);
        public static IExpression MakeParenthese(IExpression val) => new ParentheseExpression(val);
        public static IExpression MakeFunctionCall(string functionName, IExpression[] args) => new FunctionCallExpression(functionName, args);
        public static IExpression MakeTypeConvert(string destType, IExpression exp) => new TypeConvertExpression(destType, exp);

        public static IExpression FromImplicitBinary(IExpression left, IExpression right)
        {
            return (left, right) switch
            {
                (StringLiteralExpression str, IExpression exp) => new StringConcatExpression(str, exp),
                //_ => left,
                _ => throw new NotSupportedException($"left {left.GetType().Name}({left.Serialize()}), right: {right.GetType().Name}({right.Serialize()}) is not supported"),
            };
        }
    }
}
