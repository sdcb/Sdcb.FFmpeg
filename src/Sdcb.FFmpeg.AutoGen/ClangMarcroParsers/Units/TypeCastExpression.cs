namespace Sdcb.FFmpeg.AutoGen.ClangMarcroParsers.Units
{
    public record TypeCastExpression(string DestType, IExpression Exp) : IExpression
    {
        public string Serialize() => $"({DestType}){Exp.Serialize()}";
    }
}
