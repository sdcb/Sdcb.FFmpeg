namespace Sdcb.FFmpeg.AutoGen.ClangMarcroParser.Units
{

    public record Operator(string Op) : Token
    {
        public override string Serialize() => Op;
    }
}
