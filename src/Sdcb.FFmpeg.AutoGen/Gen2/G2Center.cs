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
            G2TypeConverter commonTypeConverter = G2TypeConvert.Create(KnownClasses);

            foreach (StructureDefinition structure in structures.Where(x => KnownClasses.ContainsKey(x.Name)))
            {
                G2TransformDef def = KnownClasses[structure.Name];
                File.WriteAllLines(def.GetDestFile(outputDir), def.GenerateOneCode(structure, commonTypeConverter));
            }
        }

        private static string FormatEscape(string src)
        {
            return src.Replace("{", "{{").Replace("}", "}}");
        }

        internal static Dictionary<string, G2TransformDef> KnownClasses = new G2TransformDef[]
        {
            // codecs
            G2TransformDef.MakeClass(ClassCategories.Codecs, "AVPacket", "Packet", new FieldDef[]
            {
                FieldDef.CreateNullable("side_data"),
            }),
            G2TransformDef.MakeReadonlyStruct(ClassCategories.Codecs, "AVProfile", "MediaProfile", new FieldDef[]
            {
                FieldDef.CreateTypeCast("name", TypeCastDef.Utf8String()) with { Nullable = true },
            }),
            G2TransformDef.MakeClass(ClassCategories.Codecs, "AVFrame", "Frame"),
            G2TransformDef.MakeReadonlyStruct(ClassCategories.Codecs, "AVCodec", "Codec", new FieldDef[]
            {
                FieldDef.CreateTypeCast("capabilities", TypeCastDef.Force("int", "AV_CODEC_CAP")),
                FieldDef.CreateTypeCast("name", TypeCastDef.Utf8String()),
                FieldDef.CreateTypeCast("long_name", TypeCastDef.Utf8String()),
                FieldDef.CreateTypeCast("supported_framerates", TypeCastDef.ReadSequence("AVRational")),
                FieldDef.CreateTypeCast("pix_fmts", TypeCastDef.ReadSequence("AVPixelFormat", "p == AVPixelFormat.None")),
                FieldDef.CreateTypeCast("supported_samplerates", TypeCastDef.ReadSequence("int")),
                FieldDef.CreateTypeCast("sample_fmts", TypeCastDef.ReadSequence("AVSampleFormat", "p == AVSampleFormat.None")),
                FieldDef.CreateTypeCast("channel_layouts", TypeCastDef.ReadSequence("ulong")),
                FieldDef.CreateTypeCast("profiles", TypeCastDef.ReadSequence("AVProfile", FormatEscape("p switch { { profile: 0 } => true, _ => false }"))),
                FieldDef.CreateTypeCast("wrapper_name", TypeCastDef.Utf8String()) with { Nullable = true },
                FieldDef.CreateTypeCast("ch_layouts", TypeCastDef.ReadSequence("AVChannelLayout", FormatEscape("p switch { { order: AVChannelOrder.Unspec, nb_channels: 0 } => true, _ => false }"))),
                FieldDef.CreateNullable("priv_class"), 
            }),
            G2TransformDef.MakeClass(ClassCategories.Codecs, "AVCodecParameters", "CodecParameters"),
            G2TransformDef.MakeClass(ClassCategories.Codecs, "AVCodecContext", "CodecContext", new FieldDef[]
            {
                FieldDef.CreateNullable("coded_side_data"),
                FieldDef.CreateTypeCast("flags", TypeCastDef.Force("int", "AV_CODEC_FLAG")),
            }),
            G2TransformDef.MakeClass(ClassCategories.Codecs, "AVCodecParserContext", "CodecParserContext"),
            G2TransformDef.MakeStruct(ClassCategories.Codecs, "AVPacketSideData", "PacketSideData"), 

            // formats
            G2TransformDef.MakeClass(ClassCategories.Formats, "AVFormatContext", "FormatContext", new FieldDef[]
            {
                FieldDef.CreateRename("iformat", "InputFormat"),
                FieldDef.CreateRename("oformat", "OutputFormat"),
                FieldDef.CreateNullable("pb"),
            }),
            G2TransformDef.MakeStruct(ClassCategories.Formats, "AVProgram", "MediaProgram"),
            G2TransformDef.MakeStruct(ClassCategories.Formats, "AVStream", "MediaStream", new FieldDef[]
            {
                FieldDef.CreateTypeCast("codecpar", TypeCastDef.StaticCastClass("AVCodecParameters*", "CodecParameters", isOwner: false)) with { Nullable = true },
                FieldDef.CreateNullable("side_data"), 
            }),
            G2TransformDef.MakeReadonlyStruct(ClassCategories.Formats, "AVInputFormat", "InputFormat", new FieldDef[]
            {
                FieldDef.CreateTypeCast("name", TypeCastDef.Utf8String()),
                FieldDef.CreateTypeCast("long_name", TypeCastDef.Utf8String()),
                FieldDef.CreateTypeCast("flags", TypeCastDef.Force("int", "AVFMT")),
                FieldDef.CreateTypeCast("mime_type", TypeCastDef.Utf8String()) with { Nullable = true },
            }),
            G2TransformDef.MakeReadonlyStruct(ClassCategories.Formats, "AVOutputFormat", "OutputFormat", new FieldDef[]
            {
                FieldDef.CreateTypeCast("name", TypeCastDef.Utf8String()),
                FieldDef.CreateTypeCast("long_name", TypeCastDef.Utf8String()),
                FieldDef.CreateTypeCast("flags", TypeCastDef.Force("int", "AVFMT")),
                FieldDef.CreateTypeCast("mime_type", TypeCastDef.Utf8String()) with { Nullable = true },
            }),
            G2TransformDef.MakeClass(ClassCategories.Formats, "AVIOContext", "IOContext", new FieldDef[]
            {
                FieldDef.CreateHide("seek"),
            }),
        }.ToDictionary(k => k.OldName, v => v);
    }
}
