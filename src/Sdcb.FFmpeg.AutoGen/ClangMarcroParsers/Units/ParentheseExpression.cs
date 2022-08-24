namespace Sdcb.FFmpeg.AutoGen.ClangMarcroParsers.Units
{
    public record ParentheseExpression(IExpression Content) : IExpression
    {
        public string Serialize() => $"({Content.Serialize()})";
    }
}
