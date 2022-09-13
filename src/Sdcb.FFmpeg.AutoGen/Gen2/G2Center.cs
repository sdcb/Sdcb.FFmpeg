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
                G2TransformDef.MakeStruct(ClassCategories.Codecs, "AVCodec", "Codec", new TypeCastDef[]
                {
                    TypeCastDef.Force("int", "AV_CODEC_CAP") with { FieldName = "capabilities" },
                }),
                G2TransformDef.MakeClass(ClassCategories.Codecs, "AVCodecParameters", "CodecParameters"),
                G2TransformDef.MakeClass(ClassCategories.Codecs, "AVCodecContext", "CodecContext"),
                G2TransformDef.MakeClass(ClassCategories.Codecs, "AVCodecParserContext", "CodecParserContext"),
                G2TransformDef.MakeStruct(ClassCategories.Codecs, "AVPacketSideData", "PacketSideData"),

                G2TransformDef.MakeClass(ClassCategories.Formats, "AVFormatContext", "FormatContext"),
                G2TransformDef.MakeStruct(ClassCategories.Formats, "AVProgram", "MediaProgram"),
                G2TransformDef.MakeStruct(ClassCategories.Formats, "AVStream", "MediaStream", new TypeCastDef[]
                {
                    TypeCastDef.StaticCastClass("AVCodecParameters*", "CodecParameters", nullable: true, isOwner: false),
                }),
                G2TransformDef.MakeStruct(ClassCategories.Formats, "AVInputFormat", "InputFormat"),
                G2TransformDef.MakeStruct(ClassCategories.Formats, "AVOutputFormat", "OutputFormat"),
            }.ToDictionary(k => k.OldName, v => v);

            G2TypeConverter typeConverter = G2TypeConvert.Create(knownClasses);

            foreach (StructureDefinition structure in structures.Where(x => knownClasses.ContainsKey(x.Name)))
            {
                G2ClassWriter.WriteOne(structure, knownClasses[structure.Name], outputDir, typeConverter);
            }
        }
    }
}
