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
                G2TransformDef.MakeReadonlyStruct(ClassCategories.Codecs, "AVCodec", "Codec", new FieldDef[]
                {
                    new FieldDef("capabilities", TypeCastDef.Force("int", "AV_CODEC_CAP")),
                    new FieldDef("name", TypeCastDef.Utf8String(nullable: false)), 
                    new FieldDef("long_name", TypeCastDef.Utf8String(nullable: false)), 
                    new FieldDef("supported_framerates", TypeCastDef.CustomReadonly("AVRational*", "IEnumerable<AVRational>", $@"NativeUtils.ReadSequence(
                        p: (IntPtr){{0}},
                        unitSize: sizeof(AVRational),
                        exitCondition: p => ((AVRational*)p)->Num == 0,
                        valGetter: p => *(AVRational*)p)", nullable: false)),
                    new FieldDef("pix_fmts", TypeCastDef.CustomReadonly("AVPixelFormat*", "IEnumerable<AVPixelFormat>", $@"NativeUtils.ReadSequence(
                        p: (IntPtr){{0}},
                        unitSize: sizeof(AVPixelFormat),
                        exitCondition: p => *(AVPixelFormat*)p == AVPixelFormat.None, 
                        valGetter: p => *(AVPixelFormat*)p)", nullable: false)),
                }),
                G2TransformDef.MakeClass(ClassCategories.Codecs, "AVCodecParameters", "CodecParameters"),
                G2TransformDef.MakeClass(ClassCategories.Codecs, "AVCodecContext", "CodecContext"),
                G2TransformDef.MakeClass(ClassCategories.Codecs, "AVCodecParserContext", "CodecParserContext"),
                G2TransformDef.MakeStruct(ClassCategories.Codecs, "AVPacketSideData", "PacketSideData"), 

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
                G2ClassWriter.WriteOne(structure, knownClasses[structure.Name], outputDir, typeConverter);
            }
        }
    }
}
