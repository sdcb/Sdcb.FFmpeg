namespace Sdcb.FFmpeg.AutoGen.ClangMarcroParser.Units
{
    public record IdentifierExpression(string Name) : Expression
    {
        public override string Serialize() => Name;
    }
}
