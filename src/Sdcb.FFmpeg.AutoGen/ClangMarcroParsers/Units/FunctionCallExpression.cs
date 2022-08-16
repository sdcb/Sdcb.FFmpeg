using System.Linq;

namespace Sdcb.FFmpeg.AutoGen.ClangMarcroParsers.Units
{
    public record FunctionCallExpression(string FunctionName, IExpression[] Arguments) : IExpression
    {
        public string Serialize() => $"{FunctionName}({string.Join(", ", Arguments.Select(x => x.Serialize()))})";
    }
}
