#nullable disable

namespace Sdcb.FFmpeg.AutoGen.Definitions
{
    internal class EnumerationItem : ICanGenerateXmlDoc
    {
        public string Name { get; init; }
        public string RawName { get; init; }
        public string Value { get; init; }
        public string XmlDocument { get; set; }

        public static EnumerationItem MakeFake(string name, string rawName, int value)
        {
            return new EnumerationItem
            {
                Name = name, 
                RawName = rawName, 
                Value = value.ToString(), 
                XmlDocument = name != rawName ? rawName : null, 
            };
        }
    }
}
