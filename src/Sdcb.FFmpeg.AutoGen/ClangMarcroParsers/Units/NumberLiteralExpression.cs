using static FParsec.CharParsers;

namespace Sdcb.FFmpeg.AutoGen.ClangMarcroParsers.Units
{
    public record NumberLiteralExpression(NumberLiteral Number) : IExpression
    {
        public string Serialize() => Number.String;
    }
}
