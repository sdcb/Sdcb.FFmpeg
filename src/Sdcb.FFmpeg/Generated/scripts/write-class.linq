<Query Kind="Program">
  <Namespace>Microsoft.CSharp</Namespace>
  <Namespace>System.CodeDom</Namespace>
  <Namespace>System.CodeDom.Compiler</Namespace>
</Query>

#load ".\common"
#nullable enable

void Main()
{
}

void WriteClass(GenerateOption option)
{
	using var _file = new StreamWriter($"{option.NewName}.g.cs");
	using var writer = new IndentedTextWriter(_file, new string(' ', 4));

	WriteBasic(writer, option.Namespace, () =>
	{
		WriteMultiLines(writer, BuildTypeDocument(option.TargetType));
		writer.WriteLine($"public unsafe partial class {option.NewName} : FFmpegSafeObject");
		PushIndent(writer, WriteClassBodies);
	}, additionalNamespaces: option.AdditionalNamespaces);

	if (option.WriteStub && !File.Exists(option.NewName + ".stub.cs"))
	{
		using var placeholder = new StreamWriter($"{option.NewName}.stub.cs");
		using var placeholderWriter = new IndentedTextWriter(placeholder, new string(' ', 4));
		WriteBasic(placeholderWriter, option.Namespace, () =>
		{
			placeholderWriter.WriteLine($"public unsafe partial class {option.NewName}");
			PushIndent(placeholderWriter, () =>
			{
				placeholderWriter.WriteLine("protected override void DisposeNative()");
				PushIndent(placeholderWriter, () =>
				{
					placeholderWriter.WriteLine("throw new NotImplementedException();");
				});
			});
		}, withHeader: false, additionalNamespaces: option.AdditionalNamespaces);
	}

	void WriteClassBodies()
	{
		writer.WriteLine($"protected {option.TargetType.Name}* Pointer => this;");
		writer.WriteLine();
		writer.WriteLine($"public static implicit operator {option.TargetType.Name}*({option.NewName} data) => data != null ? ({option.TargetType.Name}*)data._nativePointer : null;");
		writer.WriteLine();

		writer.WriteLine($"protected {option.NewName}({option.TargetType.Name}* ptr, bool isOwner): base(NativeUtils.NotNull((IntPtr)ptr), isOwner)");
		writer.WriteLine("{");
		writer.WriteLine("}");
		writer.WriteLine();
		writer.WriteLine($"public static {option.NewName} FromNative({option.TargetType.Name}* ptr, bool isOwner) => new {option.NewName}(ptr, isOwner);");
		writer.WriteLine();

		foreach (string line in string.Join("\r\n\r\n", option.GetFields()
			.Select(x => Convert(x, "Pointer", option.FieldNameMapping, option.FieldTypeMapping)))
			.Split("\r\n"))
		{
			writer.WriteLine(line);
		};
	}
}

void WriteStruct(GenerateOption option)
{
	using var _file = new StreamWriter($"{option.NewName}.g.cs");
	using var writer = new IndentedTextWriter(_file, new string(' ', 4));

	WriteBasic(writer, option.Namespace, () =>
	{
		WriteMultiLines(writer, BuildTypeDocument(option.TargetType));
		writer.WriteLine($"public unsafe partial struct {option.NewName}");
		PushIndent(writer, WriteClassBodies);
	}, additionalNamespaces: option.AdditionalNamespaces);

	if (option.WriteStub && !File.Exists(option.NewName + ".stub.cs"))
	{
		using var placeholder = new StreamWriter($"{option.NewName}.stub.cs");
		using var placeholderWriter = new IndentedTextWriter(placeholder, new string(' ', 4));
		WriteBasic(placeholderWriter, option.Namespace, () =>
		{
			placeholderWriter.WriteLine($"public unsafe partial struct {option.NewName}");
			PushIndent(placeholderWriter, () =>
			{
				placeholderWriter.WriteLine("// write something here");
			});
		}, withHeader: false, additionalNamespaces: option.AdditionalNamespaces);
	}

	void WriteClassBodies()
	{
		writer.WriteLine($"private {option.TargetType.Name}* _ptr;");
		writer.WriteLine();
		writer.WriteLine($"public static implicit operator {option.TargetType.Name}*({option.NewName}? data)");
		writer.WriteLine($"    => data.HasValue ? ({option.TargetType.Name}*)data.Value._ptr : null;");
		writer.WriteLine();
		writer.WriteLine($"private {option.NewName}({option.TargetType.Name}* ptr)");
		writer.WriteLine("{");
		writer.WriteLine("    if (ptr == null)");
		writer.WriteLine("    {");
		writer.WriteLine("        throw new ArgumentNullException(nameof(ptr));");
		writer.WriteLine("    }");
		writer.WriteLine("    _ptr = ptr;");
		writer.WriteLine("}");
		writer.WriteLine();
		writer.WriteLine($"public static {option.NewName} FromNative({option.TargetType.Name}* ptr) => new {option.NewName}(ptr);");
		writer.WriteLine($"public static {option.NewName} FromNative(IntPtr ptr) => new {option.NewName}(({option.TargetType.Name}*)ptr);");
		writer.WriteLine($"internal static {option.NewName}? FromNativeOrNull({option.TargetType.Name}* ptr)");
		writer.WriteLine($"    => ptr != null ? new {option.NewName}?(new {option.NewName}(ptr)) : null;");
		writer.WriteLine();

		foreach (string line in string.Join("\r\n\r\n", option.GetFields()
			.Select((x, i) => Convert(x, "_ptr", option.FieldNameMapping, option.FieldTypeMapping)))
			.Split("\r\n"))
		{
			writer.WriteLine(line);
		};
	}
}

string BuildPrefix(FieldInfo field, ObsoleteAttribute? obsolete)
{
	if (obsolete != null)
	{
		if (obsolete.Message != null)
		{
			return $"[Obsolete(\"{obsolete.Message}\")]\r\n";
		}
		else
		{
			return $"[Obsolete]\r\n";
		}
	}
	return "";
}

string Convert(FieldInfo field, string pointerName, Dictionary<string, string> propNameMapping, Dictionary<string, (string, string?)> propTypeMapping)
{
	string fieldName = IdentifierConvert(field.Name);
	string propName = FieldConvert(field.Name, propNameMapping);
	(string destType, string? method) = FromTypeString(field, propTypeMapping);
	ObsoleteAttribute? obsolete = field.GetCustomAttribute<ObsoleteAttribute>();

	bool isCallback = field.FieldType.Name.EndsWith("_func");
	bool isObsolete = obsolete != null;
	bool isUnused = field.Name.Contains("unused");
	string modifier = (isCallback || isObsolete || isUnused) ? "internal" : "public";

	return BuildFieldDocument(field) + BuildPrefix(field, obsolete) + method switch
	{
		null =>
			$"{modifier} {destType} {propName}\r\n" +
			$"{{\r\n" +
			$"    get => {pointerName}->{fieldName};\r\n" +
			$"    set => {pointerName}->{fieldName} = value;\r\n" +
			$"}}",
		"str" =>
			$"{modifier} {destType} {propName}\r\n" +
			$"{{\r\n" +
			$"    get => System.Runtime.InteropServices.Marshal.PtrToStringUTF8((IntPtr){pointerName}->{fieldName});\r\n" +
			$"    set\r\n" +
			$"    {{\r\n" +
			$"        if ({pointerName}->{fieldName} != null)\r\n" +
			$"        {{\r\n" +
			$"            av_free({pointerName}->{fieldName});\r\n" +
			$"            {pointerName}->{fieldName} = null;\r\n" +
			$"        }}\r\n" +
			$"\r\n" +
			$"        if (value != null)\r\n" +
			$"        {{\r\n" +
			$"            {pointerName}->{fieldName} = av_strdup(value);\r\n" +
			$"        }}\r\n" +
			$"    }}\r\n" +
			$"}}\r\n",
		"force" =>
			$"{modifier} {destType} {propName}\r\n" +
			$"{{\r\n" +
			$"    get => ({destType}){pointerName}->{fieldName};\r\n" +
			$"    set => {pointerName}->{fieldName} = ({GetFriendlyTypeName(field.FieldType)})value;\r\n" +
			$"}}",
		".FromNativeNotOwner" =>
			$"{modifier} {destType} {propName}\r\n" +
			$"{{\r\n" +
			$"    get => {destType}.FromNative({pointerName}->{fieldName}, isOwner: false);\r\n" +
			$"    set => {pointerName}->{fieldName} = value;\r\n" +
			$"}}",
		var x when x.StartsWith('.') =>
			$"{modifier} {destType} {propName}\r\n" +
			$"{{\r\n" +
			$"    get => {destType}{method}({pointerName}->{fieldName});\r\n" +
			$"    set => {pointerName}->{fieldName} = value;\r\n" +
			$"}}",
		_ => throw new ArgumentOutOfRangeException(method),
	};
}

(string destType, string? method) FromTypeString(FieldInfo field, Dictionary<string, (string destType, string? method)> propTypeMapping)
{
	Type fieldType = field.FieldType;
	return (fieldTypeName: fieldType.Name, fieldName: field.Name) switch
	{
		var x when propTypeMapping.TryGetValue(x.fieldName, out (string, string?) val) => val,
		("AVClass*", _) => call("FFmpegClass", "FromNativeOrNull"),
		("AVCodec*", _) => call("Codec", "FromNative"),
		("AVIOContext*", _) => call("MediaIO", "FromNativeNotOwner"),
		("AVRational", _) => direct("MediaRational"),
		("AVDictionary*", _) => call("MediaDictionary", "FromNativeNotOwner"),
		("Void*", _) => force("IntPtr"),
		("Byte*", _) => force("IntPtr"),
		("byte_ptrArray4", _) => force("Ptr4"),
		("void_ptrArray4", _) => force("Ptr4"),
		("byte_ptrArray8", _) => force("Ptr8"),
		("void_ptrArray8", _) => force("Ptr8"),
		("int_array4", _) => force("Int32x4"),
		("int_array8", _) => force("Int32x8"),
		("AVBufferRef*", _) => call("BufferReference", "FromNativeNotOwner"),
		("AVPixelFormat", _) => force("PixelFormat"),
		("AVSampleFormat", _) => force("SampleFormat"),
		("AVCodecID", _) => force("CodecID"),
		("AVMediaType", _) => force("MediaType"),
		("AVDiscard", _) => force("MediaDiscard"),
		("AVFieldOrder", _) => force("FieldOrder"),
		("AVColorRange", _) => force("ColorRange"),
		("AVColorPrimaries", _) => force("ColorPrimaries"),
		("AVColorTransferCharacteristic", _) => force("ColorTransferCharacteristic"),
		("AVColorSpace", _) => force("ColorSpace"),
		("AVChromaLocation", _) => force("ChromaLocation"),
		("AVPictureType", _) => force("PictureType"),
		("AVPacketSideDataType", _) => force("PacketSideDataType"),
		("AVDurationEstimationMethod", _) => force("DurationEstimationMethod"),
		("AVInputFormat*", _) => call("InputFormat", "FromNative"),
		("AVOutputFormat*", _) => call("OutputFormat", "FromNative"),
		("AVCodecParameters*", _) => call("CodecParameters", "FromNativeNotOwner"),
		("AVStreamParseType", _) => force("StreamParseType"),
		("AVCodecParserContext*", _) => call("CodecParserContext", "FromNativeNotOwner"), 
		("AVProgram*", _) => call("MediaProgram", "FromNative"), 
		var x when GetFriendlyTypeName(fieldType) != x.fieldTypeName => direct(GetFriendlyTypeName(fieldType)),
		var x => direct(x.fieldTypeName),
	};
}

(string, string) str() => ("string", "str");
(string, string) force(string s) => (s, "force");
(string, string?) direct(string s) => (s, null);
(string, string) call(string s, string m) => (s, "." + m);

record GenerateOption
{
	public Type TargetType { get; init; }
	public string Namespace { get; init; }
	public string NewName { get; init; }
	public Dictionary<string, (string destType, string? method)> FieldTypeMapping { get; init; } = new();
	public Dictionary<string, string> FieldNameMapping { get; init; } = new();
	public Dictionary<string, FieldOption> FieldOptions { get;init;} = new();
	public string[] AdditionalNamespaces { get; init; } = new string[0];
	public bool WriteStub { get; init; } = false;
	public string? PrivateMemberFrom { private get; init; }
	public bool KeepObsolete { private get; init; } = false;
	public bool KeepFunctionPointers { private get; init; } = false;
	
	private bool FieldShouldGenerate(FieldInfo x, int i, int privateIndex)
	{
		if (i >= privateIndex) return false;
		if (!KeepObsolete && x.GetCustomAttribute<ObsoleteAttribute>() != null) return false;
		if (!KeepFunctionPointers && x.FieldType.Name.EndsWith("_func")) return false;
		if (FieldOptions.TryGetValue(x.Name, out var fo) && ((fo & FieldOption.Hide) != FieldOption.None)) return false;
		return true;
	}

	public IEnumerable<FieldInfo> GetFields()
	{
		int privateIndex = PrivateMemberFrom != null ? TargetType.GetFields()
			.Select(x => x.Name)
			.ToList()
			.IndexOf(PrivateMemberFrom) : int.MaxValue;
		return TargetType
			.GetFields()
			.Where((x, i) => FieldShouldGenerate(x, i, privateIndex));
	}

	public GenerateOption(Type targetType, string ns, string newName)
	{
		TargetType = targetType;
		Namespace = ns;
		NewName = newName;
	}
}

[Flags]
enum FieldOption
{
	None = 0, 
	Hide = 1, 
}