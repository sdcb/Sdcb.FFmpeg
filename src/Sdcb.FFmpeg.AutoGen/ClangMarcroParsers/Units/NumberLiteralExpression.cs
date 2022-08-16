using static FParsec.CharParsers;

namespace Sdcb.FFmpeg.AutoGen.ClangMarcroParser.Units
{
    public record NumberLiteralExpression(NumberLiteral Number) : Expression
    {
        public override string Serialize() => Number.String;
    }
}
