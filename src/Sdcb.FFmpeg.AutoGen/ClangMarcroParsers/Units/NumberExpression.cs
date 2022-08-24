using static FParsec.CharParsers;

namespace Sdcb.FFmpeg.AutoGen.ClangMarcroParsers.Units
{
    public record NumberExpression(NumberLiteral Number) : IExpression
    {
        public string Serialize() => Number.String;
    }
}
