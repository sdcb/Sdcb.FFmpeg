<Query Kind="Statements">
  <Namespace>FFmpeg.AutoGen</Namespace>
  <Namespace>Microsoft.CSharp</Namespace>
  <Namespace>Sdcb.FFmpegAPIWrapper.Common</Namespace>
  <Namespace>static FFmpeg.AutoGen.ffmpeg</Namespace>
  <Namespace>System.CodeDom</Namespace>
  <Namespace>System.CodeDom.Compiler</Namespace>
  <Namespace>Sdcb.FFmpeg.Raw</Namespace>
</Query>

#load ".\common"

{
	SetDir(@"\MediaCodecs\GeneratedEnums");

	string ns = "Sdcb.FFmpegAPIWrapper.MediaCodecs";
	WriteEnum(typeof(AVSampleFormat), ns, "SampleFormat");
	WriteEnum(typeof(AVPixelFormat), ns, "PixelFormat");
	WriteEnum(typeof(AVMediaType), ns, "MediaType");
	WriteEnum(typeof(AVAudioServiceType), ns, "AudioServiceType");
	WriteEnum(typeof(AVCodecID), ns, "CodecID");
	WriteEnum(typeof(AVDiscard), ns, "MediaDiscard");
	WriteEnum(typeof(AVFieldOrder), ns, "FieldOrder");
	WriteEnum(typeof(AVColorRange), ns, "ColorRange");
	WriteEnum(typeof(AVColorPrimaries), ns, "ColorPrimaries");
	WriteEnum(typeof(AVColorTransferCharacteristic), ns, "ColorTransferCharacteristic");
	WriteEnum(typeof(AVColorSpace), ns, "ColorSpace");
	WriteEnum(typeof(AVChromaLocation), ns, "ChromaLocation");
	WriteEnum(typeof(AVPictureType), ns, "PictureType");
	WriteEnum(typeof(AVPacketSideDataType), ns, "PacketSideDataType");

	WriteConstEnum("AV_CODEC_FLAG_", ns, "CodecFlag");
	WriteConstEnum("AV_CODEC_FLAG2_", ns, "CodecFlag2");
	WriteConstEnum("AV_PKT_FLAG_", ns, "PacketFlag");
	WriteConstEnum("SLICE_FLAG_", ns, "CodecSliceFlag");
	WriteConstEnum("AV_CH_LAYOUT_", ns, "ChannelLayout");
	WriteConstEnum("AV_CODEC_CAP_", ns, "CodecCompability");
	WriteConstEnum("FF_MB_DECISION_", ns, "MacroblockDecision");
	WriteConstEnum("FF_CMP_", ns, "DctComparison");
	WriteConstEnum("AV_CODEC_EXPORT_DATA_", ns, "CodecExportData");
	WriteConstEnum("PARSER_FLAG_", ns, "ParserFlag");
}

{
	SetDir(@"\Common\GeneratedEnums");
	string ns = "Sdcb.FFmpegAPIWrapper.Common";
	WriteEnum(typeof(AVClassCategory), ns, "FFmpegCategory");
	WriteEnum(typeof(AVOptionType), ns, "FFmpegOptionType");
	WriteConstEnum("AV_OPT_FLAG_", ns, "FFmpegOptionFlags");
	WriteConstEnum("AV_OPT_SEARCH_", ns, "OptionSearchFlags");
}

{
	SetDir(@"\MediaFormats\GeneratedEnums");
	string ns = "Sdcb.FFmpegAPIWrapper.MediaFormats";
	WriteEnum(typeof(AVDurationEstimationMethod), ns, "DurationEstimationMethod");
	WriteEnum(typeof(AVStreamParseType), ns, "StreamParseType");
	WriteConstEnum("AVFMT_FLAG_", ns, "FormatFlag");
	WriteConstEnum("AVFMT_EVENT_FLAG_", ns, "EventFlag");
	WriteConstEnum("AVFMTCTX_", ns, "FormatContextFlag");
	WriteConstEnum("AVSEEK_FLAG_", ns, "MediaSeek", hasDefault: false);
	WriteConstEnum("AVSTREAM_INIT_IN_", ns, "StreamInitIn", hasDefault: false);
	WriteConstEnum("AVFMT_", ns, "FormatInputFlag", onlyKeys: new()
	{ 
		nameof(AVFMT_NOFILE), 
		nameof(AVFMT_NEEDNUMBER),  
		nameof(AVFMT_SHOW_IDS), 
		nameof(AVFMT_NOTIMESTAMPS), 
		nameof(AVFMT_GENERIC_INDEX),  
		nameof(AVFMT_TS_DISCONT), 
		nameof(AVFMT_NOBINSEARCH), 
		nameof(AVFMT_NOGENSEARCH), 
		nameof(AVFMT_NO_BYTE_SEEK),  
		nameof(AVFMT_SEEK_TO_PTS), 
	}, hasDefault: false);
	WriteConstEnum("AVFMT_", ns, "FormatOutputFlag", onlyKeys: new()
	{
		nameof(AVFMT_NOFILE),
		nameof(AVFMT_NEEDNUMBER),
		nameof(AVFMT_GLOBALHEADER),
		nameof(AVFMT_NOTIMESTAMPS),
		nameof(AVFMT_VARIABLE_FPS),
		nameof(AVFMT_NODIMENSIONS), 
		nameof(AVFMT_NOSTREAMS),
		nameof(AVFMT_ALLOW_FLUSH),
		nameof(AVFMT_TS_NONSTRICT), 
		nameof(AVFMT_TS_NEGATIVE),
	}, hasDefault: false);
}

{
	SetDir(@"\Swscales\GeneratedEnums");
	string ns = "Sdcb.FFmpegAPIWrapper.Swscales";
	WriteConstEnum("SWS_", ns, "ScaleFlag", selector: SelectScaleFlags);
	bool SelectScaleFlags(FieldInfo f) => !f.Name.StartsWith("SWS_CS") && !f.Name.StartsWith("SWS_MAX");
	
	WriteConstEnum("SWS_CS_", ns, "ScaleColorSpace", hasDefault: false);
}

void WriteConstEnum(string prefix, string ns, string newName, 
	bool hasDefault = true, 
	HashSet<string>? onlyKeys = null, 
	Func<FieldInfo, bool> selector = null!)
{
	selector = selector ?? delegate { return true; };
	using var _file = new StreamWriter(newName + ".g.cs");
	using var writer = new IndentedTextWriter(_file, new string(' ', 4));
	FieldInfo[] fields = typeof(ffmpeg)
		.GetFields(BindingFlags.Static | BindingFlags.Public)
		.Where(x => x.Name.StartsWith(prefix))
		.ToArray();
	Debug.Assert(fields.Length > 0);

	Type underlyingType = FindBestTypeForValues(fields.Select(x => Convert.ToDecimal(x.GetValue(null))));

	WriteBasic(writer, ns, WriteBody);

	void WriteBody()
	{
		string suffix = underlyingType switch
		{
			var x when x == typeof(int) => "",
			_ => " : " + GetFriendlyTypeName(underlyingType),
		};

		writer.WriteLine($"/// <summary>See {prefix}* </summary>");
		if (underlyingType == typeof(uint) || underlyingType == typeof(ulong) || newName.Contains("Flag"))
		{
			writer.WriteLine("[Flags]");
		}
		writer.WriteLine($"public enum {newName}{suffix}");
		PushIndent(writer, WriteElements);
	}

	void WriteElements()
	{
		HashSet<decimal> values = fields
			.Select(x => Convert.ToDecimal(x.GetValue(null)))
			.ToHashSet();
		if (hasDefault && !values.Contains(0))
		{
			writer.WriteLine($"None = {CSharpLiteral(0, underlyingType)},");
			writer.WriteLine();
		}
		
		foreach (FieldInfo field in fields
			.Where(x => selector(x) && (onlyKeys != null ? onlyKeys.Contains(x.Name) : true))
			.OrderBy(x => Convert.ToDecimal(x.GetValue(null))))
		{
			string name = FieldConvert(field.Name.Replace(prefix, ""), nameMapping: new ());

			WriteMultiLines(writer, BuildFieldDocument(field));
			writer.WriteLine($"{name} = {CSharpLiteral(field.GetValue(null), underlyingType)},");
			writer.WriteLine();
		}
	}

	Type FindBestTypeForValues(IEnumerable<decimal> values) => values.Max() switch
	{
		> long.MaxValue => typeof(ulong),
		> uint.MaxValue => typeof(long),
		> int.MaxValue => typeof(uint),
		_ => typeof(int)
	};
}

void WriteEnum(Type enumType, string ns, string newName)
{
	using var _file = new StreamWriter(newName + ".g.cs");
	using var writer = new IndentedTextWriter(_file, new string(' ', 4));

	Type underlyingType = Enum.GetUnderlyingType(enumType);
	WriteBasic(writer, ns, WriteBody);

	void WriteBody()
	{
		string suffix = underlyingType != typeof(int) ? " : " + GetFriendlyTypeName(underlyingType) : "";

		WriteMultiLines(writer, BuildTypeDocument(enumType));
		if (underlyingType == typeof(uint) || underlyingType == typeof(ulong))
		{
			writer.WriteLine("[Flags]");
		}
		writer.WriteLine($"public enum {newName}{suffix}");
		PushIndent(writer, WriteElements);
	}

	void WriteElements()
	{
		string[] names = Enum.GetNames(enumType);
		Array values = Enum.GetValues(enumType);
		string commonPrefix = FindCommonPrefix(names);

		for (var i = 0; i < names.Length; ++i)
		{
			string cname = names[i];
			string name = FieldConvert(cname.Replace(commonPrefix, ""), nameMapping: new());

			WriteMultiLines(writer, BuildFieldDocument(enumType.GetField(cname)));
			writer.WriteLine($"{name} = {CSharpLiteral(values.GetValue(i), underlyingType)},");
			if (i < names.Length - 1)
			{
				writer.WriteLine();
			}
		}
	}

	string FindCommonPrefix(string[] names) => new string(names
		.First()
		.Substring(0, names.Min(s => s.Length))
		.TakeWhile((c, i) => names.All(s => s[i] == c))
		.ToArray());
}

string CSharpLiteral(object val, Type underlyingType) => Type.GetTypeCode(underlyingType) switch
{
	TypeCode.Int32 => $"{(int)val}",
	TypeCode.UInt32 => $"0x{val:X}U",
	TypeCode.Int64 => $"0x{val:X}L",
	TypeCode.UInt64 => $"0x{val:X}UL",
	_ => throw new ArgumentOutOfRangeException(),
};

void SetDir(string dir)
{
	string baseDir = Path.GetFullPath(Path.Combine(Util.CurrentQuery.Location, @"..\.." + dir));
	Directory.CreateDirectory(baseDir);
	Environment.CurrentDirectory = baseDir;

	foreach (var item in Directory.EnumerateFiles(".", "*.g.cs"))
	{
		File.Delete(item);
		Util.FixedFont($"Deleted {item}").Dump();
	}
}