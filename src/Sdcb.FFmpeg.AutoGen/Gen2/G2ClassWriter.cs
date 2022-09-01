using Sdcb.FFmpeg.AutoGen.Definitions;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Sdcb.FFmpeg.AutoGen.Gen2;

internal class G2ClassWriter
{
    internal static void WriteOne(StructureDefinition structure, G2TransformDef def, string outputDir, G2TypeConverter typeConverter)
    {
        using FileStream fileStream = File.OpenWrite(def.GetDestFile(outputDir));
        using StreamWriter streamWriter = new(fileStream);
        using IndentedTextWriter ind = new IndentedTextWriter(streamWriter);
        WriteFileHeader(ind, def.Namespace);
        ind.WriteLine();
        WriteXmlComment(ind, BuildCommentForStructure(structure));
        WriteDefinitionLine(def, ind);
        using (BeginBlock(ind))
        {
            WriteCommonHeaders(def, ind);
            ind.WriteLine();
            G2TypeConverter thisTypeConverter = G2TypeConvert.Combine(def.TypeConversions, typeConverter);
            foreach (StructureField field in structure.Fields)
            {
                WriteProperties(structure, ind, field, thisTypeConverter);
                if (field != structure.Fields.Last()) ind.WriteLine();
            }
        }
        fileStream.SetLength(fileStream.Position);
    }

    private static void WriteDefinitionLine(G2TransformDef def, IndentedTextWriter ind)
    {
        if (def is ClassTransformDef)
        {
            ind.WriteLine($"public unsafe partial class {def.NewName} : SafeHandle");
        }
        else if (def is StructTransformDef)
        {
            ind.WriteLine($"public unsafe partial struct {def.NewName}");
        }
    }

    private static void WriteCommonHeaders(G2TransformDef def, IndentedTextWriter ind)
    {
        if (def is ClassTransformDef)
        {
            ind.WriteLine($"protected {def.OldName}* _ptr => ({def.OldName}*)handle;");
            ind.WriteLine();
            ind.WriteLine($"public static implicit operator {def.OldName}*({def.NewName} data) => data != null ? ({def.OldName}*)data.handle : null;");
            ind.WriteLine();
            ind.WriteLine($"protected {def.NewName}({def.OldName}* ptr, bool isOwner): base(NativeUtils.NotNull((IntPtr)ptr), isOwner)");
            using (BeginBlock(ind)) { }
            ind.WriteLine();
            ind.WriteLine($"public static {def.NewName} FromNative({def.OldName}* ptr, bool isOwner) => new {def.NewName}(ptr, isOwner);");
            ind.WriteLine();
            ind.WriteLine($"public static {def.NewName}? FromNativeOrNull({def.OldName}* ptr, bool isOwner) => ptr == null ? null : new {def.NewName}(ptr, isOwner);");
            ind.WriteLine();
            ind.WriteLine($"public override bool IsInvalid => handle == IntPtr.Zero;");
        }
        else if (def is StructTransformDef)
        {
            ind.WriteLine($"private {def.OldName}* _ptr;");
            ind.WriteLine();
            ind.WriteLine($"public static implicit operator {def.OldName}*({def.NewName}? data)");
            ind.WriteLine($"    => data.HasValue ? ({def.OldName}*)data.Value._ptr : null;");
            ind.WriteLine();
            ind.WriteLine($"private {def.NewName}({def.OldName}* ptr)");
            using (BeginBlock(ind))
            {
                ind.WriteLine(@"if (ptr == null)");
                ind.WriteLine(@"{");
                ind.WriteLine(@"    throw new ArgumentNullException(nameof(ptr));");
                ind.WriteLine(@"}");
                ind.WriteLine(@"_ptr = ptr;");
            }
            ind.WriteLine();
            ind.WriteLine($"public static {def.NewName} FromNative({def.OldName}* ptr) => new {def.NewName}(ptr);");
            ind.WriteLine($"public static {def.NewName} FromNative(IntPtr ptr) => new {def.NewName}(({def.OldName}*)ptr);");
            ind.WriteLine($"internal static {def.NewName}? FromNativeOrNull({def.OldName}* ptr)");
            ind.WriteLine($"    => ptr != null ? new {def.NewName}?(new {def.NewName}(ptr)) : null;");
        }
    }

    private static void WriteProperties(StructureDefinition structure, IndentedTextWriter ind, StructureField field, G2TypeConverter typeConverter)
    {
        TypeCastDef typeCastDef = typeConverter(field.FieldType.Name);
        WriteXmlComment(ind, BuildCommentForField(structure, field, typeCastDef.IsChanged));
        foreach (string line in typeCastDef.GetPropertyBody(field.Name))
        {
            ind.WriteLine(line);
        }
    }

    private static XElement BuildCommentForField(StructureDefinition structure, StructureField field, bool isTypeChanged)
    {
        List<object> contents = new ();

        if (isTypeChanged)
        {
            contents.Add(new XElement("para", $"original type: {field.FieldType.Name}"));
        }

        contents.AddRange(field.XmlDocument switch
        {
            null => new object[0],
            _ => field.XmlDocument
                .Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .Select(line => new XElement("para", line))
        });

        contents.Add(new XElement("see", new XAttribute("cref", $"{structure.Name}.{field.Name}")));

        return new XElement("summary", contents);
    }

    private static XElement BuildCommentForStructure(StructureDefinition structure)
    {
        List<object> contents = new();
        contents.AddRange(structure.XmlDocument switch
        {
            null => new object[0],
            _ => structure.XmlDocument
                .Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .Select(line => new XElement("para", line))
        });
        contents.Add(new XElement("see", new XAttribute("cref", $"{structure.Name}")));

        return new XElement("summary", contents);
    }


    static void WriteXmlComment(IndentedTextWriter ind, XElement xml)
    {
        foreach (string line in xml.ToString().Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
        {
            ind.WriteLine($"/// {line}");
        }
    }

    static void WriteFileHeader(IndentedTextWriter ind, string destNamespace)
    {
        ind.WriteLine($@"// This file was genereated from Sdcb.FFmpeg.AutoGen, DO NOT CHANGE DIRECTLY.
#nullable enable
using Sdcb.FFmpeg.Common;
using Sdcb.FFmpeg.Raw;
using System;
using System.Runtime.InteropServices;

namespace {destNamespace};");
    }

    static IDisposable BeginBlock(IndentedTextWriter ind, bool inline = false)
    {
        ind.WriteLine("{");
        ind.Indent++;
        return new End(() =>
        {
            ind.Indent--;

            if (inline)
                ind.Write("}");
            else
                ind.WriteLine("}");
        });
    }

    private class End : IDisposable
    {
        private readonly Action _action;

        public End(Action action) => _action = action;

        public void Dispose() => _action();
    }
}
