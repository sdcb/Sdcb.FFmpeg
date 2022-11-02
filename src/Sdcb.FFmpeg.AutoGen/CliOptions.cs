using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using CommandLine;

#nullable disable

namespace Sdcb.FFmpeg.AutoGen
{
    /// <summary>
    ///     Command line options.
    /// </summary>
    public class CliOptions
    {
        [Option('n', "namespace", Default = "Sdcb.FFmpeg.Raw",
            HelpText = "The namespace that will contain the generated symbols.")]
        public string Namespace { get; set; }

        [Option('c', "class", Default = "ffmpeg",
            HelpText = "The name of the class that contains the FFmpeg unmanaged method calls.")]
        public string ClassName { get; set; }

        /// <summary>
        ///     See http://ybeernet.blogspot.ro/2011/03/techniques-of-calling-unmanaged-code.html.
        /// </summary>
        [Option('f', "SuppressUnmanagedCodeSecurity",
            HelpText = "Add the [SuppressUnmanagedCodeSecurity] attribute to unmanaged method calls " +
                       "(faster invocation).")]
        public bool SuppressUnmanagedCodeSecurity { get; set; }

        [Option('i', "input", Required = false,
            HelpText = "The path to the directory that contains the FFmpeg header and binary files " +
                       "(must have the default structure).")]
        public string FFmpegDir { get; set; }

        [Option('h', "headers", Required = false,
            HelpText = "The path to the directory that contains the FFmpeg header files.")]
        public string FFmpegIncludesDir { get; set; }

        [Option('b', "bin", Required = false,
            HelpText = "The path to the directory that contains the FFmpeg binaries.")]
        public string FFmpegBinDir { get; set; }

        [Option('o', "output", Required = false,
            HelpText = "The path to the directory where to output the generated files.")]
        public string OutputDir { get; set; }

        [Option('v',
            HelpText = "Print details during execution.")]
        public bool Verbose { get; set; }

        [Option("using-online-ffmpeg-binaries", Required = false, Default = true)]
        public bool UsingOnlineFFmpegBinaries { get; set; }

        [Option("ffmpeg-binary-url", Required = false, Default = NuGetBuilder.PackageInfo.Url)]
        public string FFmpegBinaryUrl { get; set; }

        public static CliOptions ParseArgumentsStrict(string[] args)
        {
            var result = Parser.Default.ParseArguments<CliOptions>(args);
            var options = result.MapResult(x => x, x => new CliOptions());
            options.Normalize();
            return options;
        }

        private void Normalize()
        {
            // Support for the original path setup
            string solutionDir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory)
                .Parent.Parent.Parent.Parent.Parent.FullName;

            if (string.IsNullOrWhiteSpace(OutputDir)) OutputDir = Path.Combine(solutionDir, "src/Sdcb.FFmpeg/Raw");

            if (UsingOnlineFFmpegBinaries)
            {
                if (!Uri.TryCreate(FFmpegBinaryUrl, UriKind.Absolute, out Uri _))
                {
                    Console.WriteLine($"Invalid FFmpegBinaryUrl: {FFmpegBinaryUrl}");
                    Environment.Exit(1);
                }
                SetupFFmpegBinaries(solutionDir, FFmpegBinaryUrl);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(FFmpegDir) &&
                string.IsNullOrWhiteSpace(FFmpegIncludesDir) &&
                string.IsNullOrWhiteSpace(FFmpegBinDir))
                    FFmpegDir = Path.Combine(solutionDir, "FFmpeg");

                // If the FFmpegDir option is specified, it will take precedence
                if (!string.IsNullOrWhiteSpace(FFmpegDir))
                {
                    FFmpegIncludesDir = Path.Combine(FFmpegDir, "include");
                    FFmpegBinDir = Path.Combine(FFmpegDir, "bin/x64");
                    FFmpegDir = null;
                }
            }


            // Fail if required options are not specified
            if (string.IsNullOrWhiteSpace(FFmpegBinDir))
            {
                Console.WriteLine("The path to the directory that contains " +
                                  "the FFmpeg binaries is missing (specify it using -b or --bin).");
                Environment.Exit(1);
            }

            if (string.IsNullOrWhiteSpace(FFmpegIncludesDir))
            {
                Console.WriteLine("The path to the directory that contains " +
                                  "the FFmpeg headers is missing (specify it using -h or --headers).");
                Environment.Exit(1);
            }

            // Check paths exist
            if (!Directory.Exists(FFmpegBinDir))
            {
                Console.WriteLine("The path to the directory that contains " +
                                  "the FFmpeg binaries does not exist.");
                Environment.Exit(1);
            }

            if (!Directory.Exists(FFmpegIncludesDir))
            {
                Console.WriteLine("The path to the directory that contains " +
                                  "the FFmpeg headers does not exist.");
                Environment.Exit(1);
            }

            if (!Directory.Exists(OutputDir))
            {
                Console.WriteLine("The output directory does not exist.");
                Environment.Exit(1);
            }

            // Resolve paths
            FFmpegIncludesDir = Path.GetFullPath(FFmpegIncludesDir);
            FFmpegBinDir = Path.GetFullPath(FFmpegBinDir);
            OutputDir = Path.GetFullPath(OutputDir);
        }

        private void SetupFFmpegBinaries(string solutionDir, string ffmpegBinaryUrl)
        {
            string zippedFilename = new Uri(ffmpegBinaryUrl).Segments.Last();
            FFmpegDir = Path.Combine(solutionDir, "FFmpeg-binary-cache");
            FFmpegIncludesDir = Path.Combine(FFmpegDir, "include");
            FFmpegBinDir = Path.Combine(FFmpegDir, "bin/x64");
            Directory.CreateDirectory(FFmpegDir);

            string destinationCacheFile = Path.Combine(FFmpegDir, zippedFilename);
            string cacheGoodFile = Path.Combine(FFmpegDir, zippedFilename + ".cache-good");
            if (File.Exists(cacheGoodFile))
            {
                Console.WriteLine($"Detected cache good file in {FFmpegDir}, skipped downloading binary.");
                return;
            }

            Console.WriteLine($"(1 of 4) Downloading {ffmpegBinaryUrl} to {destinationCacheFile} ...");
            {
                HttpClient http = new();
                Stream msg = http.GetStreamAsync(ffmpegBinaryUrl).GetAwaiter().GetResult();
                using FileStream file = File.OpenWrite(destinationCacheFile);
                msg.CopyTo(file);
                file.SetLength(file.Position);
                Console.WriteLine($"Done.");
            }
            
            Console.WriteLine($"(2 of 4) Cleanup {FFmpegDir} folder...");
            {
                if (Directory.Exists(FFmpegIncludesDir)) Directory.Delete(FFmpegIncludesDir, recursive: true);
                if (Directory.Exists(FFmpegBinDir)) Directory.Delete(FFmpegBinDir, recursive: true);
                Console.WriteLine("Done.");
            }

            Console.WriteLine($"(3 of 4) Extracting {destinationCacheFile}...");
            {
                using ZipArchive zip = ZipFile.OpenRead(destinationCacheFile);
                ExtractPrefixToDest(zip, "/bin/", FFmpegBinDir);
                ExtractPrefixToDest(zip, "/include/", FFmpegIncludesDir);

                static void ExtractPrefixToDest(ZipArchive zip, string prefix, string dest)
                {
                    string zipPrefix = zip.Entries.Single(x => x.FullName.EndsWith(prefix) && x.Length == 0).FullName;
                    Directory.CreateDirectory(dest);

                    foreach (ZipArchiveEntry entry in zip.Entries.Where(x => x.FullName.StartsWith(zipPrefix) && x.FullName != zipPrefix))
                    {
                        string path = Path.Combine(dest, entry.FullName.Replace(zipPrefix, ""));
                        if (entry.Length == 0 && entry.FullName.EndsWith("/"))
                        {
                            // folder
                            Console.WriteLine($"Creating folder {path}...");
                            Directory.CreateDirectory(path);
                        }
                        else
                        {
                            // file
                            Console.WriteLine($"Extract file {path}...");
                            entry.ExtractToFile(path);
                        }
                    }
                }
            }

            Console.WriteLine($"(4 of 4) Writing .cache-good file...");
            {
                File.WriteAllText(cacheGoodFile, "1");
                Console.WriteLine("Done.");
            }
        }
    }
}