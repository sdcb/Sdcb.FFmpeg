using Sdcb.FFmpeg.AutoGen.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sdcb.FFmpeg.AutoGen.Gen2
{
    internal static class G2Center
    {
        public static void WriteAll(string outputDir, IEnumerable<StructureDefinition> structures)
        {
            Dictionary<string, G2TransformDef> knownClasses = new G2TransformDef[]
            {
                G2TransformDef.MakeClass(ClassCategories.Codecs, "AVPacket", "Packet"),
                G2TransformDef.MakeClass(ClassCategories.Codecs, "AVFrame", "Frame"),
                G2TransformDef.MakeStruct(ClassCategories.Codecs, "AVCodec", "Codec"),
                G2TransformDef.MakeClass(ClassCategories.Codecs, "AVCodecParameters", "CodecParameters"),
                G2TransformDef.MakeClass(ClassCategories.Codecs, "AVCodecContext", "CodecContext"),
                G2TransformDef.MakeClass(ClassCategories.Codecs, "AVCodecParserContext", "CodecParserContext"),
                G2TransformDef.MakeStruct(ClassCategories.Codecs, "AVPacketSideData", "PacketSideData"), 
            }.ToDictionary(k => k.OldName, v => v);

            G2TypeConverter typeConverter = G2TypeConvert.Create(knownClasses);

            foreach (StructureDefinition structure in structures.Where(x => knownClasses.ContainsKey(x.Name)))
            {
                G2ClassWriter.WriteOne(structure, knownClasses[structure.Name], outputDir, typeConverter);
            }
        }
    }
}
