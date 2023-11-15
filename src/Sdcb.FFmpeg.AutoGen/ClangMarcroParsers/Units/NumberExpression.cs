using System.Text;
using static FParsec.CharParsers;

namespace Sdcb.FFmpeg.AutoGen.ClangMarcroParsers.Units
{
    public record NumberExpression(NumberLiteral Number) : IExpression
    {
        public string Serialize()
        {
            StringBuilder suffixSb = new(4);

            if (Number.SuffixChar1 != EOS) suffixSb.Append(Number.SuffixChar1);
            if (Number.SuffixChar2 != EOS) suffixSb.Append(Number.SuffixChar2);
            if (Number.SuffixChar3 != EOS) suffixSb.Append(Number.SuffixChar3);
            if (Number.SuffixChar4 != EOS) suffixSb.Append(Number.SuffixChar4);

            string suffix = suffixSb.ToString() switch
            {
                "ULL" => "UL", 
                var x => x, 
            };

            return Number.String + suffix;
        }
    }
}
