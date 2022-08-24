namespace Sdcb.FFmpeg.AutoGen.ClangMarcroParsers.Units
{
    public record StringExpression(string Str) : IExpression
    {
        public string Serialize() => $"\"{Str}\"";
    }
}
