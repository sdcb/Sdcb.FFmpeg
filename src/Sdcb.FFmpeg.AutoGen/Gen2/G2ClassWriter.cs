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
    internal static void WriteOne(StructureDefinition structure, ClassTransformDefinition def, string outputDir)
    {
        using FileStream fileStream = File.OpenWrite(def.GetDestFile(outputDir));
        using StreamWriter streamWriter = new(fileStream);
        using IndentedTextWriter ind = new IndentedTextWriter(streamWriter);
        WriteFileHeader(ind, def.Namespace);
        ind.WriteLine();
        WriteXmlComment(ind, BuildCommentForStructure(structure));
        ind.WriteLine($"public unsafe partial class {def.NewName} : SafeHandle");
        using (BeginBlock(ind))
        {
            WriteCommonClassHeaders(structure, def, ind);
            ind.WriteLine();
            foreach (StructureField field in structure.Fields)
            {
                WriteProperties(structure, ind, field);
                if (field != structure.Fields.Last()) ind.WriteLine();
            }
        }
        fileStream.SetLength(fileStream.Position);
    }

    private static void WriteCommonClassHeaders(StructureDefinition structure, ClassTransformDefinition def, IndentedTextWriter ind)
    {
        ind.WriteLine($"protected {def.OldName}* Pointer => this;");
        ind.WriteLine();
        ind.WriteLine($"public static implicit operator {structure.Name}*({def.NewName} data) => data != null ? data.Pointer : null;");
        ind.WriteLine();
        ind.WriteLine($"protected {def.NewName}({structure.Name}* ptr, bool isOwner): base(NativeUtils.NotNull((IntPtr)ptr), isOwner)");
        using (BeginBlock(ind)) { }
        ind.WriteLine();
        ind.WriteLine($"public static {def.NewName} FromNative({structure.Name}* ptr, bool isOwner) => new {def.NewName}(ptr, isOwner);");
        ind.WriteLine();
        ind.WriteLine($"public override bool IsInvalid => handle == IntPtr.Zero;");
    }

    private static void WriteProperties(StructureDefinition structure, IndentedTextWriter ind, StructureField field)
    {
        (string newType, bool isTypeChanged) = G2StringTransforms.TypeTransform(field.FieldType.Name);
        WriteXmlComment(ind, BuildCommentForField(structure, field, isTypeChanged));
        ind.WriteLine($"public {newType} {G2StringTransforms.NameTransform(field.Name)}");
        using (BeginBlock(ind))
        {
            if (isTypeChanged)
            {
                ind.WriteLine($"get => ({newType})Pointer->{field.Name};");
                ind.WriteLine($"set => Pointer->{field.Name} = ({field.FieldType.Name})value;"); 
            }
            else
            {
                ind.WriteLine($"get => Pointer->{field.Name};");
                ind.WriteLine($"set => Pointer->{field.Name} = value;");
            }
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
