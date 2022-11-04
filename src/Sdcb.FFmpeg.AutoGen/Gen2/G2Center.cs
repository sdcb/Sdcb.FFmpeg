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
                FieldDef.CreateTypeCast("data", TypeCastDef.DataPointer("byte*", "size", "int")),
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
                FieldDef.CreateHide("priv_data_size"),
                FieldDef.CreateHide("next"),
                FieldDef.CreateHide("defaults"),
                FieldDef.CreateHide("caps_internal"),
                FieldDef.CreateHide("bsfs"),
                FieldDef.CreateHide("hw_configs"),
                FieldDef.CreateHide("codec_tags"),
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
                FieldDef.CreateTypeCast("data", TypeCastDef.DataPointer("byte*", "size", "ulong")),
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
                FieldDef.CreateTypeCastReadonly("streams", TypeCastDef.ReadonlyPtrList("AVStream", "MediaStream", "nb_streams", "MediaStream.FromNative")),
                FieldDef.CreateHide("nb_streams"),
                FieldDef.CreateTypeCastReadonly("programs", TypeCastDef.ReadonlyPtrList("AVProgram", "MediaProgram", "nb_programs", "MediaProgram.FromNative")),
                FieldDef.CreateHide("nb_programs"),
                FieldDef.CreateTypeCastReadonly("chapters", TypeCastDef.ReadonlyPtrList("AVChapter", "MediaChapter", "nb_chapters", "MediaChapter.FromNative")),
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
                FieldDef.CreateTypeCastReadonly("side_data", TypeCastDef.ReadonlyPtrList("AVFrameSideData", "FrameSideData", "nb_side_data", "FrameSideData.FromNative")),
                FieldDef.CreateHide("nb_side_data"),
                FieldDef.CreateNullable("hw_frames_ctx"),
                FieldDef.CreateNullable("opaque_ref"),
                FieldDef.CreateNullable("private_ref"),
            }),
            G2TransformDef.MakeStruct(ClassCategories.Utils, "AVFrameSideData", "FrameSideData", new FieldDef[]
            {
                FieldDef.CreateNullable("buf"),
                FieldDef.CreateTypeCast("data", TypeCastDef.DataPointer("byte*", "size", "ulong")),
                FieldDef.CreateHide("size"),
            }),
            G2TransformDef.MakeClass(ClassCategories.Utils, "AVBufferRef", "BufferRef", new FieldDef[]
            {
                FieldDef.CreateTypeCast("buffer", TypeCastDef.Force("AVBuffer*", "IntPtr")),
                FieldDef.CreateTypeCast("data", TypeCastDef.DataPointer("byte*", "size", "ulong")),
                FieldDef.CreateHide("size"),
            }),
            G2TransformDef.MakeClass(ClassCategories.Utils, "AVAudioFifo", "AudioFifo"),
            #endregion

            #region filters
            G2TransformDef.MakeReadonlyStruct(ClassCategories.Filters, "AVFilter", "Filter", new FieldDef[]
            {
                FieldDef.CreateTypeCast("name", TypeCastDef.ReadOnlyUtf8String()),
                FieldDef.CreateTypeCastNullable("description", TypeCastDef.ReadOnlyUtf8String()),
                FieldDef.CreateTypeCast("flags", TypeCastDef.Force("int", "AVFILTER_FLAG")),
                FieldDef.CreateNullable("priv_class"),
                FieldDef.CreateHide("next"),
                FieldDef.CreateTypeCast("inputs", TypeCastDef.CustomReadonly("AVFilterPad*", "FilterPadList", "new FilterPadList({0}, _ptr->nb_inputs)")),
                FieldDef.CreateTypeCast("outputs", TypeCastDef.CustomReadonly("AVFilterPad*", "FilterPadList", "new FilterPadList({0}, _ptr->nb_outputs)")),
                FieldDef.CreateHide("nb_inputs"),
                FieldDef.CreateHide("nb_outputs"),
            }),
            G2TransformDef.MakeClass(ClassCategories.Filters, "AVFilterGraph", "FilterGraph", new FieldDef[]
            {
                FieldDef.CreateTypeCastReadonly("filters", TypeCastDef.ReadonlyPtrList("AVFilterContext", "FilterContext", "nb_filters", "p => FilterContext.FromNative(p, isOwner: false)")),
                FieldDef.CreateHide("nb_filters"),
                FieldDef.CreateTypeCast("internal", TypeCastDef.Force("AVFilterGraphInternal*", "IntPtr")),
                FieldDef.CreateTypeCastReadonly("sink_links", TypeCastDef.ReadonlyPtrList("AVFilterLink", "FilterLink", "sink_links_count", "p => FilterLink.FromNative(p, isOwner: false)")),
                FieldDef.CreateHide("sink_links_count"),
            }),
            G2TransformDef.MakeClass(ClassCategories.Filters, "AVFilterContext", "FilterContext", new FieldDef[]
            {
                FieldDef.CreateTypeCastReadonly("inputs", TypeCastDef.ReadonlyPtrList("AVFilterLink", "FilterLink", "nb_inputs", "p => FilterLink.FromNative(p, isOwner: false)")),
                FieldDef.CreateHide("nb_inputs"),
                FieldDef.CreateTypeCastReadonly("outputs", TypeCastDef.ReadonlyPtrList("AVFilterLink", "FilterLink", "nb_outputs", "p => FilterLink.FromNative(p, isOwner: false)")),
                FieldDef.CreateHide("nb_outputs"),
                FieldDef.CreateTypeCastReadonly("input_pads", TypeCastDef.CustomReadonly("AVFilterPad*", "FilterPadList", "new FilterPadList({0}, (int)_ptr->nb_inputs)")),
                FieldDef.CreateTypeCastReadonly("output_pads", TypeCastDef.CustomReadonly("AVFilterPad*", "FilterPadList", "new FilterPadList({0}, (int)_ptr->nb_outputs)")),
                FieldDef.CreateTypeCast("name", TypeCastDef.OptUtf8String()),
                FieldDef.CreateTypeCastNullable("enable_str", TypeCastDef.OptUtf8String()),
                FieldDef.CreateNullable("hw_device_ctx"),
                FieldDef.CreateTypeCast("internal", TypeCastDef.Force("AVFilterInternal*", "IntPtr")),
            }),
            G2TransformDef.MakeClass(ClassCategories.Filters, "AVFilterLink", "FilterLink", new FieldDef[]
            {
                FieldDef.CreateNullable("hw_frames_ctx"),
                FieldDef.CreateNullable("partial_buf"),
                FieldDef.CreateNullable("graph"),
                FieldDef.CreateHide("reserved"),
                FieldDef.CreateTypeCastReadonly("srcpad", TypeCastDef.StaticCastStruct("AVFilterPad*", "FilterPad")) with { Nullable = true },
                FieldDef.CreateTypeCastReadonly("dstpad", TypeCastDef.StaticCastStruct("AVFilterPad*", "FilterPad")) with { Nullable = true },
            }),            
            G2TransformDef.MakeClass(ClassCategories.Filters, "AVFilterInOut", "FilterInOut", new FieldDef[]
            {
                FieldDef.CreateTypeCast("name", TypeCastDef.DupUtf8String()),
                FieldDef.CreateNullable("next"),
            }),
            G2TransformDef.MakeClass(ClassCategories.Filters, "AVBufferSrcParameters", "BufferSrcParameters", new FieldDef[]
            {
                FieldDef.CreateNullable("hw_frames_ctx"),
            }),
            #endregion filters
        }.ToDictionary(k => k.OldName, v => v);
    }
}
