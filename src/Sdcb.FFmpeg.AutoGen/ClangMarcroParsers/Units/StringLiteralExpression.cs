namespace Sdcb.FFmpeg.AutoGen.ClangMarcroParser.Units
{
    public record StringLiteralExpression(string Str) : Expression
    {
        public override string Serialize() => $"\"{Str}\"";
    }
}
