namespace Sdcb.FFmpeg.AutoGen.ClangMarcroParser.Units
{
    public record TypeConvertExpression(string DestType, Expression Exp) : Expression
    {
        public override string Serialize() => $"({DestType}){Exp.Serialize()}";
    }
}
