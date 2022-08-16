using System.Linq;

namespace Sdcb.FFmpeg.AutoGen.ClangMarcroParser.Units
{
    public record FunctionCallExpression(string FunctionName, Expression[] Arguments) : Expression
    {
        public override string Serialize() => $"{FunctionName}({string.Join(", ", Arguments.Select(x => x.Serialize()))})";
    }
}
