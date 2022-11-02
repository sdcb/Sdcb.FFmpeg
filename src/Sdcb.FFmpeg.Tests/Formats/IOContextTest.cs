using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sdcb.FFmpeg.Common;
using Sdcb.FFmpeg.Formats;
using Sdcb.FFmpeg.Raw;
using Sdcb.FFmpeg.Utils;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Sdcb.FFmpeg.Tests.Formats;

public class IOContextTest
{
    private readonly ITestOutputHelper _console;

    public IOContextTest(ITestOutputHelper console)
    {
        _console = console;
    }

    [Fact]
    public void FileReadWrite()
    {
        string desktop = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "test.txt");

        WriteToFile(desktop, "我是中国人，我爱中国\n");
        _console.WriteLine(ReadFromFile(desktop));

        File.Delete(desktop);

        void WriteToFile(string path, string text)
        {
            using IOContext io = IOContext.Open(path, AVIO_FLAG.Write);
            io.WriteString(text);
        }

        string ReadFromFile(string path)
        {
            using IOContext io = IOContext.Open(path, AVIO_FLAG.Read);
            return io.ReadString();
        }
    }

    [Fact]
    public void StreamWrite()
    {
        using MemoryStream ms = new();
        {
            using IOContext io = IOContext.WriteStream(ms);
            for (var i = 0; i < 5; ++i)
            {
                io.WriteString("Hello World");
            }
        }

        _console.WriteLine(Encoding.UTF8.GetString(ms.ToArray()));
    }

    [Fact]
    public void StreamRead()
    {
        using MemoryStream ms = new(Encoding.UTF8.GetBytes("Hello World"));

        using IOContext io = IOContext.ReadStream(ms);
        _console.WriteLine(io.ReadString());
    }

    [Fact]
    public void ListInputProtocols()
    {
        _console.WriteLine(string.Join("\n", IOContext.InputProtocols));
    }

    [Fact]
    public void CheckRead()
    {
        AVIO_FLAG result = IOContext.Check(Environment.CurrentDirectory, AVIO_FLAG.Read);
        Assert.Equal(AVIO_FLAG.Read, result);
    }

    [Fact]
    public void ProtocolClassHttpsExists()
    {
        FFmpegClass? cls = IOContext.GetProtocolClass("https");
        Assert.NotNull(cls);
    }

    [Fact]
    public void ConnectHttp()
    {
        CancellationTokenSource cts = new();
        string url = "http://localhost:5000";
        Task web = EchoIPWeb(cts.Token);
        using IOContext io = IOContext.Open(url, options: new MediaDictionary
        {
            ["user_agent"] = "Test/1.0"
        });
        io.ReadString();
        cts.Cancel();

        async Task EchoIPWeb(CancellationToken cancellationToken)
        {
            WebApplicationBuilder wab = WebApplication.CreateBuilder();
            wab.Services.RemoveAll<ILoggerProvider>();
            await using WebApplication app = wab.Build();
            app.MapGet("/", async ctx => await ctx.Response.WriteAsync(ctx.Request.Headers.UserAgent));
            app.Urls.Add(url);
            await app.RunAsync(cancellationToken);
        }
    }
}
