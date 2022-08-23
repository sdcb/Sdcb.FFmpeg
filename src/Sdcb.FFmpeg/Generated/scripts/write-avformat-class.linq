<Query Kind="Statements">
  <NuGetReference Prerelease="true">Sdcb.FFmpeg</NuGetReference>
  <Namespace>Microsoft.CSharp</Namespace>
  <Namespace>System.CodeDom</Namespace>
  <Namespace>System.CodeDom.Compiler</Namespace>
  <Namespace>Sdcb.FFmpeg.Raw</Namespace>
</Query>

#load ".\common"
#load ".\write-class"

string baseDir = Path.Combine(BaseDir, "MediaFormats");
Directory.CreateDirectory(baseDir);
Environment.CurrentDirectory = baseDir;

string ns = "Sdcb.FFmpeg.MediaFormats";
WriteClass(new GenerateOption(typeof(AVFormatContext), ns, "FormatContext")
{
	FieldNameMapping = new()
	{
		["iformat"] = "InputFormat",
		["oformat"] = "OutputFormat", 
		["pb"] = "IO",
	},
	FieldTypeMapping = new()
	{
		["url"] = str(),
		["flags"] = force("FormatFlag"),
		["ctx_flags"] = force("FormatContextFlag"), 
		["event_flags"] = force("EventFlag"),
		["avio_flags"] = force("MediaIOFlags"),
	},
	FieldOptions = new()
	{
		["streams"] = FieldOption.Hide, 
		["nb_streams"] = FieldOption.Hide
	},
	AdditionalNamespaces = new string[] {"Sdcb.FFmpeg.MediaCodecs" }, 
	WriteStub = true, 
});

WriteStruct(new GenerateOption(typeof(AVInputFormat), ns, "InputFormat")
{
	FieldTypeMapping = new()
	{
		["name"] = str(),
		["long_name"] = str(),
		["extensions"] = str(),
		["mime_type"] = str(),
		["flags"] = force("FormatInputFlag"), 
	}, 
	PrivateMemberFrom = nameof(AVInputFormat.raw_codec_id),
	WriteStub = true, 
});

WriteStruct(new GenerateOption(typeof(AVOutputFormat), ns, "OutputFormat")
{
	FieldTypeMapping = new()
	{
		["name"] = str(),
		["long_name"] = str(),
		["extensions"] = str(),
		["mime_type"] = str(),
		["flags"] = force("FormatOutputFlag"), 
	},
	AdditionalNamespaces = new string[] { "Sdcb.FFmpegAPIWrapper.MediaCodecs" },
	PrivateMemberFrom = nameof(AVOutputFormat.priv_data_size),
	WriteStub = true, 
});

WriteStruct(new GenerateOption(typeof(AVStream), ns, "MediaStream")
{
	AdditionalNamespaces = new string[] { "Sdcb.FFmpegAPIWrapper.MediaCodecs" },
	FieldNameMapping = new ()
	{
		["r"] = "Real", 
	},
	WriteStub = true, 
});

WriteStruct(new GenerateOption(typeof(AVProgram), ns, "MediaProgram")
{
	AdditionalNamespaces = new string[] { "Sdcb.FFmpegAPIWrapper.MediaCodecs" },
});