// See https://aka.ms/new-console-template for more information
using Sdcb.FFmpeg.NuGetBuilder;
using SharpCompress.Archives;
using System.Diagnostics;
using System.IO.Compression;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

string solutionDir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory)
    .Parent?.Parent?.Parent?.Parent?.Parent?.FullName ?? throw new Exception();

await SetupFFmpegBinaries(solutionDir, PackageInfo.Url);

const string namePrefix = "Sdcb.FFmpeg.runtime";

string binaryRoot = Path.Combine(solutionDir, "FFmpeg-binary-cache");

string[] dlls = Directory.EnumerateFiles(binaryRoot, "*.dll", SearchOption.AllDirectories)
    .Select(x => x.Replace(binaryRoot, "").Replace(@"\", "/") switch { var v => "." + v })
    .ToArray();
string nuspecFile = BuildNuspec(dlls, "win-x64", "windows-x64");

string nupkgsRoot = Path.Combine(binaryRoot, "nupkgs");
Directory.CreateDirectory(nupkgsRoot);
Process.Start(new ProcessStartInfo($"nuget", $"pack {nuspecFile} -version {PackageInfo.Version} -OutputDirectory {nupkgsRoot}")
{
    WorkingDirectory = binaryRoot
})!.WaitForExit();

string BuildNuspec(string[] libs, string rid, string titleRid)
{
    // props
    {
        XDocument props = XDocument.Parse(File
            .ReadAllText($"./assets/{namePrefix}.props"));
        string ns = props.Root!.GetDefaultNamespace().NamespaceName;
        XmlNamespaceManager nsr = new(new NameTable());
        nsr.AddNamespace("p", ns);

        string platform = rid.Split("-").Last();

        XElement itemGroup = props.XPathSelectElement("/p:Project/p:ItemGroup", nsr)!;
        itemGroup.Add(
        libs
            .Select(x => Path.GetFileName(x))
            .Select(x => new XElement(XName.Get("Content", ns), new XAttribute("Include", $@"$(SdcbFFmpegDirectory)\{rid}\native\{x}"),
                new XElement(XName.Get("Link", ns), @$"dll\{platform}\{x}"),
                new XElement(XName.Get("CopyToOutputDirectory", ns), "PreserveNewest")))
                );
        props.Save(Path.Combine(binaryRoot, @$"./{namePrefix}.{titleRid}.props"));
    }

    // nuspec
    {
        XDocument nuspec = XDocument.Parse(File
            .ReadAllText($"./assets/{namePrefix}.nuspec")
            .Replace("{rid}", rid)
            .Replace("{titleRid}", titleRid)
            .Replace("{version}", PackageInfo.Version)
            .Replace("{year}", DateTime.Now.Year.ToString()));

        string ns = nuspec.Root!.GetDefaultNamespace().NamespaceName;
        XmlNamespaceManager nsr = new(new NameTable());
        nsr.AddNamespace("p", ns);

        XElement files = nuspec!.XPathSelectElement("/p:package/p:files", nsr)!;
        files.Add(libs.Select(x => new XElement(
            XName.Get("file", ns),
            new XAttribute("src", x.Replace($@"{titleRid}\", "")),
            new XAttribute("target", @$"runtimes\{rid}\native"))));
        files.Add(new[] { "net", "netstandard", "netcoreapp" }.Select(x => new XElement(
            XName.Get("file", ns),
            new XAttribute("src", $"{namePrefix}.{titleRid}.props"),
            new XAttribute("target", @$"build\{x}\{namePrefix}.{titleRid}.props"))));

        string destinationFile = Path.Combine(binaryRoot, @$"./{rid}.nuspec");
        nuspec.Save(destinationFile);
        return destinationFile;
    }
}

async Task SetupFFmpegBinaries(string solutionDir, string ffmpegBinaryUrl)
{
    string zippedFilename = new Uri(ffmpegBinaryUrl).Segments.Last();
    string FFmpegDir = Path.Combine(solutionDir, "FFmpeg-binary-cache");
    string FFmpegBinDir = Path.Combine(FFmpegDir, "bin/x64");
    string FFmpegIncludeDir = Path.Combine(FFmpegDir, "include");
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
        Stream msg = await http.GetStreamAsync(ffmpegBinaryUrl);
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
        using IArchive archive = ArchiveFactory.Open(destinationCacheFile);
        ExtractPrefixToDest(archive.Entries, "/bin", FFmpegBinDir);
        ExtractPrefixToDest(archive.Entries, "/include", FFmpegIncludeDir);

        static void ExtractPrefixToDest(IEnumerable<IArchiveEntry> entries, string prefix, string dest)
        {
            IArchiveEntry zipPrefixEntry = entries.Single(x => x.Key.EndsWith(prefix) && x.IsDirectory);
            string zipPrefix = zipPrefixEntry.Key + "/";
            Directory.CreateDirectory(dest);

            foreach (IArchiveEntry entry in entries.Where(x => x.Key.StartsWith(zipPrefix) && x.Key != zipPrefix))
            {
                string path = Path.Combine(dest, entry.Key.Replace(zipPrefix, ""));
                if (entry.IsDirectory)
                {
                    // folder
                    Console.WriteLine($"Creating folder {path}...");
                    Directory.CreateDirectory(path);
                }
                else
                {
                    // file
                    Console.WriteLine($"Extract file {path}...");
                    using (var entryStream = entry.OpenEntryStream())
                    {
                        using (var fileStream = File.Create(path))
                        {
                            entryStream.CopyTo(fileStream);
                        }
                    }
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