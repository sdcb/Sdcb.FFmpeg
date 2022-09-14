using Sdcb.FFmpeg.AutoGen.Definitions;
using Sdcb.FFmpeg.AutoGen.Gen2.TransformDefs;
using System;
using System.Collections.Generic;
using System.IO;
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
                // codecs
                G2TransformDef.MakeClass(ClassCategories.Codecs, "AVPacket", "Packet"),
                G2TransformDef.MakeClass(ClassCategories.Codecs, "AVFrame", "Frame"),
                G2TransformDef.MakeReadonlyStruct(ClassCategories.Codecs, "AVCodec", "Codec", new FieldDef[]
                {
                    new FieldDef("capabilities", TypeCastDef.Force("int", "AV_CODEC_CAP")),
                    new FieldDef("name", TypeCastDef.Utf8String(nullable: false)), 
                    new FieldDef("long_name", TypeCastDef.Utf8String(nullable: false)), 
                    new FieldDef("supported_framerates", TypeCastDef.ReadSequence("AVRational")),
                    new FieldDef("pix_fmts", TypeCastDef.ReadSequence("AVPixelFormat", "p == AVPixelFormat.None")),
                    new FieldDef("supported_samplerates", TypeCastDef.ReadSequence("int")),
                    new FieldDef("sample_fmts", TypeCastDef.ReadSequence("AVSampleFormat")),
                    new FieldDef("channel_layouts", TypeCastDef.ReadSequence("ulong")), 
                    new FieldDef("profiles", TypeCastDef.ReadSequence("AVProfile", FormatEscape("p switch { { profile: 0 } => true, _ => false }"))),
                    new FieldDef("wrapper_name", TypeCastDef.Utf8String(nullable: false)),
                    new FieldDef("ch_layouts", TypeCastDef.ReadSequence("AVChannelLayout", FormatEscape("p switch { { order: AVChannelOrder.Unspec, nb_channels: 0 } => true, _ => false }"))),
                    // sample_fmts
                }),
                G2TransformDef.MakeClass(ClassCategories.Codecs, "AVCodecParameters", "CodecParameters"),
                G2TransformDef.MakeClass(ClassCategories.Codecs, "AVCodecContext", "CodecContext"),
                G2TransformDef.MakeClass(ClassCategories.Codecs, "AVCodecParserContext", "CodecParserContext"),
                G2TransformDef.MakeStruct(ClassCategories.Codecs, "AVPacketSideData", "PacketSideData"), 

                // formats
                G2TransformDef.MakeClass(ClassCategories.Formats, "AVFormatContext", "FormatContext"),
                G2TransformDef.MakeStruct(ClassCategories.Formats, "AVProgram", "MediaProgram"),
                G2TransformDef.MakeStruct(ClassCategories.Formats, "AVStream", "MediaStream", new FieldDef[]
                {
                    new FieldDef("codecpar", TypeCastDef.StaticCastClass("AVCodecParameters*", "CodecParameters", nullable: true, isOwner: false)),
                }),
                G2TransformDef.MakeStruct(ClassCategories.Formats, "AVInputFormat", "InputFormat"),
                G2TransformDef.MakeStruct(ClassCategories.Formats, "AVOutputFormat", "OutputFormat"),
            }.ToDictionary(k => k.OldName, v => v);

            G2TypeConverter typeConverter = G2TypeConvert.Create(knownClasses);

            foreach (StructureDefinition structure in structures.Where(x => knownClasses.ContainsKey(x.Name)))
            {
                G2TransformDef def = knownClasses[structure.Name];
                File.WriteAllLines(def.GetDestFile(outputDir), def.GenerateOneCode(structure, typeConverter));
            }
        }

        private static string FormatEscape(string src)
        {
            return src.Replace("{", "{{").Replace("}", "}}");
        }
    }
}
