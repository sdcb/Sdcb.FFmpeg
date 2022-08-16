namespace Sdcb.FFmpeg.AutoGen.ClangMarcroParser.Units
{
    public record CharLiteralExpression(char C) : Expression
    {
        public override string Serialize() => $"'{C}'";
    }
}
