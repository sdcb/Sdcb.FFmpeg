namespace Sdcb.FFmpeg.AutoGen.ClangMarcroParsers.Units
{
    public record CharLiteralExpression(char C) : IExpression
    {
        public string Serialize() => $"'{C}'";
    }
}
