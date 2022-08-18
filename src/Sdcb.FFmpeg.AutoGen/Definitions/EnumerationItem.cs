namespace Sdcb.FFmpeg.AutoGen.Definitions
{
    internal class EnumerationItem : ICanGenerateXmlDoc
    {
        public string Name { get; init; }
        public string RawName { get; init; }
        public string Value { get; init; }
        public string Content { get; set; }
    }
}
