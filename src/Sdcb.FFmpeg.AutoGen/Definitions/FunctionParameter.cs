#nullable disable

namespace Sdcb.FFmpeg.AutoGen.Definitions
{
    internal record FunctionParameter : ICanGenerateXmlDoc
    {
        public string Name { get; init; }
        public TypeDefinition Type { get; init; }
        public string XmlDocument { get; set; }
    }
}