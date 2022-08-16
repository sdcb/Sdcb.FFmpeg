namespace Sdcb.FFmpeg.AutoGen.ClangMarcroParser.Units
{
    record NegativeExpression(Expression val) : Expression
    {
        public override string Serialize() => $"-{val.Serialize()}";
    }
}
