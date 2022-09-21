using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.FSharp.Core;
using Sdcb.FFmpeg.AutoGen.Definitions;
using Sdcb.FFmpeg.AutoGen.Processors;

namespace Sdcb.FFmpeg.AutoGen
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            Console.WriteLine($"App started using args: {String.Join(",", args)}");
            var options = CliOptions.ParseArgumentsStrict(args);

            if (options.Verbose)
            {
                Console.WriteLine($"Working dir: {Environment.CurrentDirectory}");
                Console.WriteLine($"Output dir: {options.OutputDir}");
                Console.WriteLine($"FFmpeg headers dir: {options.FFmpegIncludesDir}");
                Console.WriteLine($"FFmpeg bin dir: {options.FFmpegBinDir}");
                Console.WriteLine($"Namespace name: {options.Namespace}");
                Console.WriteLine($"Class name: {options.ClassName}");
            }

            var existingInlineFunctions =
                MetricHelper.RecordTime("Loading inline functions", () => ExistingInlineFunctionsHelper.LoadInlineFunctions(Path.Combine(options.OutputDir,
                    "FFmpeg.functions.inline.g.cs")));

            FunctionExport[] exports = MetricHelper.RecordTime("Loading functions", () => FunctionExportHelper.LoadFunctionExports(options.FFmpegBinDir).ToArray());

            ASTProcessor astProcessor = new (exports
                    .GroupBy(x => x.Name).Select(x => x.First()) // Eliminate duplicated names
                    .ToDictionary(x => x.Name));
            astProcessor.IgnoreUnitNames.Add("__NSConstantString_tag");

            var defines = new[] { "__STDC_CONSTANT_MACROS" };

            var g = new Generator(astProcessor)
            {
                IncludeDirs = new[] { options.FFmpegIncludesDir },
                Defines = defines,
                Exports = exports,
                Namespace = options.Namespace,
                ClassName = options.ClassName,
                ExistingInlineFunctions = existingInlineFunctions,
                SuppressUnmanagedCodeSecurity = options.SuppressUnmanagedCodeSecurity
            };

            g.Parse(
                "libavutil/avutil.h",
                "libavutil/audio_fifo.h",
                "libavutil/channel_layout.h",
                "libavutil/cpu.h",
                "libavutil/file.h",
                "libavutil/frame.h",
                "libavutil/opt.h",
                "libavutil/imgutils.h",
                "libavutil/time.h",
                "libavutil/timecode.h",
                "libavutil/tree.h",
                "libavutil/hwcontext.h",
                "libavutil/hwcontext_dxva2.h",
                "libavutil/hwcontext_d3d11va.h",
                "libavutil/hdr_dynamic_metadata.h",
                "libavutil/mastering_display_metadata.h",

                "libswresample/swresample.h",

                "libpostproc/postprocess.h",

                "libswscale/swscale.h",

                "libavcodec/avcodec.h",
                "libavcodec/dxva2.h",
                "libavcodec/d3d11va.h",

                "libavformat/avformat.h",

                "libavfilter/avfilter.h",
                "libavfilter/buffersrc.h",
                "libavfilter/buffersink.h",

                "libavdevice/avdevice.h");

            g.WriteLibraries(Path.Combine(options.OutputDir, "FFmpeg.libraries.g.cs"));
            g.WriteMacros(Path.Combine(options.OutputDir, "FFmpeg.macros.g.cs"));
            g.WriteEnums(Path.Combine(options.OutputDir, "FFmpeg.enums.g.cs"));
            g.WriteDelegates(Path.Combine(options.OutputDir, "FFmpeg.delegates.g.cs"));
            g.WriteArrays(Path.Combine(options.OutputDir, "FFmpeg.arrays.g.cs"));
            g.WriteStructures(Path.Combine(options.OutputDir, "FFmpeg.structs.g.cs"));
            g.WriteIncompleteStructures(Path.Combine(options.OutputDir, "FFmpeg.structs.incomplete.g.cs"));
            g.WriteExportFunctions(Path.Combine(options.OutputDir, "FFmpeg.functions.export.g.cs"));
            g.WriteInlineFunctions(Path.Combine(options.OutputDir, "FFmpeg.functions.inline.g.cs"));
            Gen2.G2Center.WriteAll(options.OutputDir, astProcessor.Units.OfType<StructureDefinition>());
        }
    }
}
