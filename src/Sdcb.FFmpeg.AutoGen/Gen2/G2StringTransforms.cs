using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable CS8509 // switch 表达式不会处理属于其输入类型的所有可能值(它并非详尽无遗)。
namespace Sdcb.FFmpeg.AutoGen.Gen2
{
    internal static class G2StringTransforms
    {
        public static string NameTransform(string name) => string.Concat(name
           .Split('_')
           .Select(x => KeywordTransform(x switch
           {
               [var c, ..] when char.IsDigit(c) => $"_{x}",
               [var c, .. var rest] => char.ToUpper(c) + rest.ToLowerInvariant(),
           })));

        private static readonly Dictionary<string, string> _g2KeywordMappings = new()
        {
            ["Pos"] = "Position",
            ["Pix"] = "Pixel",
            ["Fmt"] = "Format",
            ["Fmts"] = "Formats",
            ["Ctx"] = "Context", 
            ["Priv"] = "Private", 
        };

        private static string KeywordTransform(string name)
        {
            return _g2KeywordMappings.TryGetValue(name, out var result) switch
            {
                true => result,
                false => name,
            };
        }
    }
}
