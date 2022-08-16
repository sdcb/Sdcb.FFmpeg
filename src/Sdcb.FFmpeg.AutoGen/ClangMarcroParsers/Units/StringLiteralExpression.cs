namespace Sdcb.FFmpeg.AutoGen.ClangMarcroParsers.Units
{
    public record StringLiteralExpression(string Str) : IExpression
    {
        public string Serialize() => $"\"{Str}\"";
    }
}
