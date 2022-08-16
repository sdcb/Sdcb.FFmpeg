namespace Sdcb.FFmpeg.AutoGen.ClangMarcroParsers.Units
{
    public record NegativeExpression(IExpression val) : IExpression
    {
        public string Serialize() => $"-{val.Serialize()}";
    }
}
