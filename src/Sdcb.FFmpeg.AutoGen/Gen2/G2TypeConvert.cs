using System.Collections.Generic;
using System.Linq;

namespace Sdcb.FFmpeg.AutoGen.Gen2
{
    internal static class G2TypeConvert
    {
        public static G2TypeConverter Create(Dictionary<string, G2TransformDefinition> knownDefinitions)
        {
            Dictionary<string, string> commonTypeMaps = new()
            {
                ["void*"] = "IntPtr",
                ["byte*"] = "IntPtr",
            };

            Dictionary<string, G2TransformDefinition> knownMappings = knownDefinitions
                .ToDictionary(k => k.Key + '*', v => v.Value);

            return Change;

            TypeTransformResult Change(string srcType)
            {
                return knownMappings.TryGetValue(srcType, out G2TransformDefinition knownType) switch
                {
                    true => new TypeTransformResult(knownType.NewName, true),
                    false => commonTypeMaps.TryGetValue(srcType, out string commonType) switch
                    {
                        true => new TypeTransformResult(commonType, true),
                        false => new TypeTransformResult(srcType, false),
                    }
                };  
            }
        }
    }

    internal delegate TypeTransformResult G2TypeConverter(string srcType);

    internal record TypeTransformResult(string NewType, bool IsChanged);
}
