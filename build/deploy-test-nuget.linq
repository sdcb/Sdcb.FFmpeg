<Query Kind="Statements" />

Environment.CurrentDirectory = Path.GetDirectoryName(Util.CurrentQueryPath);
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
Encoding encoding = Encoding.GetEncoding(936);

string projFile = @"..\src\Sdcb.FFmpeg\Sdcb.FFmpeg.csproj";
string nugetVersion = IncrementBuildVersion(projFile);
Util.Cmd("dotnet", @$"build {projFile} -c Release", encoding);
string progetApiKey = Util.GetPassword("proget-api-key");
string progetApiUrl = Util.GetPassword("proget-api-test");

string nugetPath = @$"..\src\Sdcb.FFmpeg\bin\Release\Sdcb.FFmpeg.{nugetVersion}.nupkg";
Util.Cmd($@"nuget", @$"push {nugetPath} {progetApiKey} -Source {progetApiUrl}", encoding);

string IncrementBuildVersion(string projFile)
{
	string allText = File.ReadAllText(projFile);
	XDocument xml = XDocument.Parse(allText);
	XElement versionSuffixNode = xml.XPathSelectElement(@"/Project/PropertyGroup/VersionSuffix");
	XElement versionPrefixNode = xml.XPathSelectElement(@"/Project/PropertyGroup/VersionPrefix");
	
	string versionPrefix = (versionPrefixNode.FirstNode as XText).Value;
	string versionSuffix = (versionSuffixNode.FirstNode as XText).Value;
	int buildNumber = int.Parse(versionSuffix.Split('.')[1]);
	int newBuildNumber = buildNumber + 1;
	string newVersionSuffix = "preview." + newBuildNumber;
	
	versionSuffixNode.ReplaceNodes(newVersionSuffix);
	
	File.WriteAllText(projFile, allText.Replace(versionSuffix, newVersionSuffix));
	
	return versionPrefix + "-" + newVersionSuffix;
}