<Query Kind="Statements">
  <Namespace>FFmpeg.AutoGen</Namespace>
  <Namespace>Microsoft.CSharp</Namespace>
  <Namespace>Sdcb.FFmpegAPIWrapper.Common</Namespace>
  <Namespace>System.CodeDom</Namespace>
  <Namespace>System.CodeDom.Compiler</Namespace>
  <Namespace>Sdcb.FFmpeg.Raw</Namespace>
</Query>

#load ".\common"
#load ".\write-class"
#nullable enable

string baseDir = Path.GetFullPath(Path.Combine(Util.CurrentQuery.Location, @"..\..\MediaCodecs\GeneratedTypes"));
Directory.CreateDirectory(baseDir);
Environment.CurrentDirectory = baseDir;

string ns = "Sdcb.FFmpegAPIWrapper.MediaCodecs";
WriteClass(new GenerateOption(typeof(AVCodecParameters), ns, "CodecParameters"));
WriteClass(new GenerateOption(typeof(AVFrame), ns, "Frame")
{
	FieldTypeMapping = new()
	{
		["channel_layout"] = force("ChannelLayout")
	}
});
WriteClass(new GenerateOption(typeof(AVPacket), ns, "Packet")
{
	WriteStub = true,
	FieldTypeMapping = new()
	{
		["flags"] = force("PacketFlag")
	}
});
WriteStruct(new GenerateOption(typeof(AVPacketSideData), ns, "PacketSideData"));
WriteClass(new GenerateOption(typeof(AVCodecContext), ns, "CodecContext")
{
	FieldTypeMapping = new()
	{
		["flags"] = force("CodecFlag"),
		["flags2"] = force("CodecFlag2"),
		["ildct_cmp"] = force("DctComparison"),
		["slice_flags"] = force("CodecSliceFlag"),
		["mb_decision"] = force("MacroblockDecision"),
		["export_side_data"] = force("CodecExportData"),
		["channel_layout"] = force("ChannelLayout")
	},
});
WriteClass(new GenerateOption(typeof(AVCodecParserContext), ns, "CodecParserContext")
{
	FieldTypeMapping = new()
	{
		["flags"] = force("ParserFlag")
	}
});