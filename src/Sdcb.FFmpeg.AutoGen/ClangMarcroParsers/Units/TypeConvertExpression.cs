namespace Sdcb.FFmpeg.AutoGen.ClangMarcroParsers.Units
{
    public record TypeConvertExpression(string DestType, IExpression Exp) : IExpression
    {
        public string Serialize() => $"({DestType}){Exp.Serialize()}";
    }
}
