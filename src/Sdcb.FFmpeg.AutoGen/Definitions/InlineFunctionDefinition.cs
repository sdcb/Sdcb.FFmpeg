#nullable disable

namespace Sdcb.FFmpeg.AutoGen.Definitions
{
    internal record InlineFunctionDefinition : FunctionDefinitionBase
    {
        public string Body { get; set; }
        public string OriginalBodyHash { get; set; }
    }
}