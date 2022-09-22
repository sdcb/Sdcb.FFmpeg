using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sdcb.FFmpeg.Common;
using Sdcb.FFmpeg.Formats;
using Sdcb.FFmpeg.Raw;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Sdcb.FFmpeg.Tests.Formats;

public class FormatTest
{
    private readonly ITestOutputHelper _console;

    public FormatTest(ITestOutputHelper console)
    {
        _console = console;
    }

    [Fact]
    public void InputFormatTest()
    {
        foreach (InputFormat fmt in InputFormat.All)
        {
            _console.WriteLine(fmt.Name);
        }
    }

    [Fact]
    public void OutputFormatTest()
    {
        foreach (OutputFormat fmt in OutputFormat.All)
        {
            _console.WriteLine(fmt.Name);
        }
    }
}
