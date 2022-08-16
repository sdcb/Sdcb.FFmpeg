using System;
using static FParsec.CharParsers;

namespace Sdcb.FFmpeg.AutoGen.ClangMarcroParser.Units
{
    public abstract record Expression : Token
    {
        public static Expression MakeNumberLiteral(NumberLiteral number) => new NumberLiteralExpression(number);
        public static Expression MakeStringLiteral(string text) => new StringLiteralExpression(text);
        public static Expression MakeStringConcat(StringLiteralExpression str, Expression exp) => new StringConcatExpression(str, exp);
        public static Expression MakeCharLiteral(char c) => new CharLiteralExpression(c);
        public static Expression MakeNegative(Expression e) => new NegativeExpression(e);
        public static Expression MakeIdentifier(string identifier) => new IdentifierExpression(identifier);
        public static Expression MakeBinary(Expression left, Operator op, Expression right) => new BinaryExpression(left, op, right);
        public static Expression MakeParenthese(Expression val) => new ParentheseExpression(val);
        public static Expression MakeFunctionCall(string functionName, Expression[] args) => new FunctionCallExpression(functionName, args);
        public static Expression MakeTypeConvert(string destType, Expression exp) => new TypeConvertExpression(destType, exp);

        public static Expression FromImplicitBinary(Expression left, Expression right)
        {
            return (left, right) switch
            {
                (StringLiteralExpression str, Expression exp) => new StringConcatExpression(str, exp),
                //_ => left,
                _ => throw new NotSupportedException($"left {left.GetType().Name}({left.Serialize()}), right: {right.GetType().Name}({right.Serialize()}) is not supported"),
            };
        }
    }
}
