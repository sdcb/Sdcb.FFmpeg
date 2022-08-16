namespace Sdcb.FFmpeg.AutoGen.ClangMarcroParser.Units
{
    public record BinaryExpression(Expression Left, Operator Op, Expression Right) : Expression
    {
        public override string Serialize() => $"{Left.Serialize()} {Op.Serialize()} {Right.Serialize()}";
    }
}
