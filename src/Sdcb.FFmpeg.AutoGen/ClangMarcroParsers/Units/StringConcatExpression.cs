namespace Sdcb.FFmpeg.AutoGen.ClangMarcroParser.Units
{
    public record StringConcatExpression(StringLiteralExpression Str, Expression Exp) : Expression
    {
        public override string Serialize() => $"{Str.Serialize()} + {Exp.Serialize()}";
    }
}
