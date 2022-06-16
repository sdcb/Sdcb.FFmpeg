using NUnit.Framework;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;

namespace Sdcb.FFmpeg.Tests
{
    [SetUpFixture]
    public class AssemblySetup
    {
        [OneTimeSetUp]
        public void Setup()
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                string FFmpegBinDir = SetupFFmpegBinaries(".", "https://github.com/GyanD/codexffmpeg/releases/download/5.0.1/ffmpeg-5.0.1-full_build-shared.zip");
                Environment.SetEnvironmentVariable("PATH", $"{FFmpegBinDir};{Environment.GetEnvironmentVariable("PATH")}");
            }
        }

        static string SetupFFmpegBinaries(string solutionDir, string ffmpegBinaryUrl)
        {
            string zippedFilename = new Uri(ffmpegBinaryUrl).Segments.Last();
            string FFmpegDir = Path.Combine(solutionDir, "FFmpeg-binary-cache");
            string FFmpegBinDir = Path.Combine(FFmpegDir, "bin/x64");
            Directory.CreateDirectory(FFmpegDir);

            string destinationCacheFile = Path.Combine(FFmpegDir, zippedFilename);
            string cacheGoodFile = Path.Combine(FFmpegDir, zippedFilename + ".cache-good");
            if (File.Exists(cacheGoodFile))
            {
                Console.WriteLine($"Detected cache good file in {FFmpegDir}, skipped downloading binary.");
                return FFmpegBinDir;
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
                if (Directory.Exists(FFmpegBinDir)) Directory.Delete(FFmpegBinDir, recursive: true);
                Console.WriteLine("Done.");
            }

            Console.WriteLine($"(3 of 4) Extracting {destinationCacheFile}...");
            {
                using ZipArchive zip = ZipFile.OpenRead(destinationCacheFile);
                ExtractPrefixToDest(zip, "/bin/", FFmpegBinDir);

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
                return FFmpegBinDir;
            }
        }
    }
}
