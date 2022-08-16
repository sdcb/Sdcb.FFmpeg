namespace Sdcb.FFmpeg.AutoGen.ClangMarcroParser.Units
{
    public record ParentheseExpression(Expression Content) : Expression
    {
        public override string Serialize() => $"({Content.Serialize()})";
    }
}
