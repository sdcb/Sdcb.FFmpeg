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
            #region codecs
            G2TransformDef.MakeClass(ClassCategories.Codecs, "AVPacket", "Packet", new FieldDef[]
            {
                FieldDef.CreateNullable("side_data"),
                FieldDef.CreateNullable("buf"),
                FieldDef.CreateNullable("opaque_ref"),
                FieldDef.CreateTypeCast("data", TypeCastDef.ReadonlyDataPointer("byte*", "size")) with { ReadOnly = true },
                FieldDef.CreateHide("size"),
            }),
            G2TransformDef.MakeReadonlyStruct(ClassCategories.Codecs, "AVProfile", "MediaProfile", new FieldDef[]
            {
                FieldDef.CreateTypeCast("name", TypeCastDef.ReadOnlyUtf8String()) with { Nullable = true },
            }),
            G2TransformDef.MakeReadonlyStruct(ClassCategories.Codecs, "AVCodec", "Codec", new FieldDef[]
            {
                FieldDef.CreateTypeCast("capabilities", TypeCastDef.Force("int", "AV_CODEC_CAP")),
                FieldDef.CreateTypeCast("name", TypeCastDef.ReadOnlyUtf8String()),
                FieldDef.CreateTypeCast("long_name", TypeCastDef.ReadOnlyUtf8String()),
                FieldDef.CreateTypeCast("supported_framerates", TypeCastDef.ReadSequence("AVRational", FormatEscape("p switch { { Num: 0 } => true, _ => false }"))),
                FieldDef.CreateTypeCast("pix_fmts", TypeCastDef.ReadSequence("AVPixelFormat", "p == AVPixelFormat.None")),
                FieldDef.CreateTypeCast("supported_samplerates", TypeCastDef.ReadSequence("int")),
                FieldDef.CreateTypeCast("sample_fmts", TypeCastDef.ReadSequence("AVSampleFormat", "p == AVSampleFormat.None")),
                FieldDef.CreateTypeCast("channel_layouts", TypeCastDef.ReadSequence("ulong")),
                FieldDef.CreateTypeCast("profiles", TypeCastDef.ReadSequenceAndCast("AVProfile", "MediaProfile",
                    FormatEscape("p switch { { profile: (int)FF_PROFILE.Unknown } => true, _ => false }"),
                    getter: "MediaProfile.FromNative({0})")),
                FieldDef.CreateTypeCast("wrapper_name", TypeCastDef.ReadOnlyUtf8String()) with { Nullable = true },
                FieldDef.CreateTypeCast("ch_layouts", TypeCastDef.ReadSequence("AVChannelLayout", FormatEscape("p switch { { order: AVChannelOrder.Unspec, nb_channels: 0 } => true, _ => false }"))),
                FieldDef.CreateNullable("priv_class"),
                FieldDef.CreateNullable("next"), 
            }),
            G2TransformDef.MakeClass(ClassCategories.Codecs, "AVCodecParameters", "CodecParameters"),
            G2TransformDef.MakeClass(ClassCategories.Codecs, "AVCodecContext", "CodecContext", new FieldDef[]
            {
                FieldDef.CreateNullable("coded_side_data"),
                FieldDef.CreateTypeCast("flags", TypeCastDef.Force("int", "AV_CODEC_FLAG")),
                FieldDef.CreateNullable("hw_frames_ctx"), 
                FieldDef.CreateNullable("hw_device_ctx"), 
            }),
            G2TransformDef.MakeClass(ClassCategories.Codecs, "AVCodecParserContext", "CodecParserContext"),
            G2TransformDef.MakeStruct(ClassCategories.Codecs, "AVPacketSideData", "PacketSideData", new FieldDef[]
            {
                FieldDef.CreateTypeCast("data", TypeCastDef.ReadonlyDataPointer("byte*", "size")) with { ReadOnly = true },
                FieldDef.CreateHide("size"),
            }),
            #endregion

            #region formats
            G2TransformDef.MakeClass(ClassCategories.Formats, "AVFormatContext", "FormatContext", new FieldDef[]
            {
                FieldDef.CreateRenameNullable("iformat", "InputFormat") with { Nullable = true },
                FieldDef.CreateRenameNullable("oformat", "OutputFormat") with { Nullable = true },
                FieldDef.CreateNullable("video_codec"),
                FieldDef.CreateNullable("audio_codec"),
                FieldDef.CreateNullable("subtitle_codec"),
                FieldDef.CreateNullable("data_codec"),
                FieldDef.CreateNullable("pb"),
                FieldDef.CreateTypeCastNullable("dump_separator", TypeCastDef.OptUtf8String()),
                FieldDef.CreateTypeCastNullable("protocol_whitelist", TypeCastDef.OptUtf8String()),
                FieldDef.CreateTypeCastNullable("protocol_blacklist", TypeCastDef.OptUtf8String()),
                FieldDef.CreateTypeCast("flags", TypeCastDef.Force("int", "AVFMT_FLAG")),
                FieldDef.CreateTypeCast("streams", TypeCastDef.ReadonlyPtrList("AVStream", "MediaStream", "nb_streams", "FromNative")) with { ReadOnly = true },
                FieldDef.CreateHide("nb_streams"),
                FieldDef.CreateTypeCast("programs", TypeCastDef.ReadonlyPtrList("AVProgram", "MediaProgram", "nb_programs", "FromNative")) with { ReadOnly = true },
                FieldDef.CreateHide("nb_programs"),
                FieldDef.CreateTypeCast("chapters", TypeCastDef.ReadonlyPtrList("AVChapter", "MediaChapter", "nb_chapters", "FromNative")) with { ReadOnly = true },
                FieldDef.CreateHide("nb_chapters"),
            }),
            G2TransformDef.MakeStruct(ClassCategories.Formats, "AVProgram", "MediaProgram", new FieldDef[]
            {
                FieldDef.CreateTypeCast("stream_index", TypeCastDef.ReadonlyNativeList("uint", "nb_stream_indexes")) with { ReadOnly = true },
                FieldDef.CreateHide("nb_stream_indexes"),
            }),
            G2TransformDef.MakeStruct(ClassCategories.Formats, "AVChapter", "MediaChapter"),
            G2TransformDef.MakeStruct(ClassCategories.Formats, "AVStream", "MediaStream", new FieldDef[]
            {
                FieldDef.CreateTypeCastNullable("codecpar", TypeCastDef.StaticCastClass("AVCodecParameters*", "CodecParameters", isOwner: false)),
                FieldDef.CreateNullable("side_data"),
            }),
            G2TransformDef.MakeReadonlyStruct(ClassCategories.Formats, "AVInputFormat", "InputFormat", new FieldDef[]
            {
                FieldDef.CreateTypeCast("name", TypeCastDef.ReadOnlyUtf8String()),
                FieldDef.CreateTypeCast("long_name", TypeCastDef.ReadOnlyUtf8String()),
                FieldDef.CreateTypeCastNullable("extensions", TypeCastDef.ReadOnlyUtf8String()),
                FieldDef.CreateTypeCast("flags", TypeCastDef.Force("int", "AVFMT")),
                FieldDef.CreateTypeCastNullable("mime_type", TypeCastDef.ReadOnlyUtf8String()),
                FieldDef.CreateNullable("priv_class"),
                FieldDef.CreateNullable("next"),
            }),
            G2TransformDef.MakeReadonlyStruct(ClassCategories.Formats, "AVOutputFormat", "OutputFormat", new FieldDef[]
            {
                FieldDef.CreateTypeCast("name", TypeCastDef.ReadOnlyUtf8String()),
                FieldDef.CreateTypeCast("long_name", TypeCastDef.ReadOnlyUtf8String()),
                FieldDef.CreateTypeCastNullable("extensions", TypeCastDef.ReadOnlyUtf8String()),
                FieldDef.CreateTypeCast("flags", TypeCastDef.Force("int", "AVFMT")),
                FieldDef.CreateTypeCastNullable("mime_type", TypeCastDef.ReadOnlyUtf8String()),
                FieldDef.CreateNullable("priv_class"),
                FieldDef.CreateNullable("next"),
            }),
            G2TransformDef.MakeClass(ClassCategories.Formats, "AVIOContext", "IOContext", new FieldDef[]
            {
                FieldDef.CreateHide("seek"),
            }),
            #endregion

            #region utils
            G2TransformDef.MakeClass(ClassCategories.Utils, "AVFrame", "Frame", new FieldDef[]
            {
                FieldDef.CreateTypeCast("side_data", TypeCastDef.ReadonlyPtrList("AVFrameSideData", "FrameSideData", "nb_side_data", "FromNative")) with { ReadOnly = true },
                FieldDef.CreateHide("nb_side_data"),
                FieldDef.CreateNullable("hw_frames_ctx"),
                FieldDef.CreateNullable("opaque_ref"),
                FieldDef.CreateNullable("private_ref"),
            }),
            G2TransformDef.MakeStruct(ClassCategories.Utils, "AVFrameSideData", "FrameSideData", new FieldDef[]
            {
                FieldDef.CreateNullable("buf"),
                FieldDef.CreateTypeCast("data", TypeCastDef.ReadonlyDataPointer("byte*", "size")) with { ReadOnly = true },
                FieldDef.CreateHide("size"),
            }),
            G2TransformDef.MakeClass(ClassCategories.Utils, "AVBufferRef", "BufferRef", new FieldDef[]
            {
                FieldDef.CreateTypeCast("buffer", TypeCastDef.Force("AVBuffer*", "IntPtr")), 
                FieldDef.CreateTypeCast("data", TypeCastDef.ReadonlyDataPointer("byte*", "size")) with { ReadOnly = true },
                FieldDef.CreateHide("size"),
            }),
            #endregion
        }.ToDictionary(k => k.OldName, v => v);
    }
}
