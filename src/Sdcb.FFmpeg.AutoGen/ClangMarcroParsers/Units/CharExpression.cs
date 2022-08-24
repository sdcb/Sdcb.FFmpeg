namespace Sdcb.FFmpeg.AutoGen.ClangMarcroParsers.Units
{
    public record CharExpression(char C) : IExpression
    {
        public string Serialize() => $"'{C}'";
    }
}
