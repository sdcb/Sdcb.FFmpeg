using Sdcb.FFmpeg.Common;
using Sdcb.FFmpeg.Raw;
using Sdcb.FFmpeg.Utils;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sdcb.FFmpeg.Tests.Common;

public class MediaDictionaryTest
{
    [Fact]
    public void CanMultiDispose()
    {
        using MediaDictionary dict = new();
        dict["One"] = "一";
        dict.Free();

        dict["One"] = "1";
        dict.Dispose();
    }

    [Fact]
    public void CanQueryPrefix()
    {
        using MediaDictionary dict = new()
        {
            ["A1001"] = "100-One",
            ["a1002"] = "100-Two",
            ["A1003"] = "100-Three",
            ["a2001"] = "200-One", 
        };
        List<KeyValuePair<string, string>> data = dict.QueryPrefix("A100").ToList();
        Assert.Equal(new Dictionary<string, string>
        {
            ["A1001"] = "100-One",
            ["A1003"] = "100-Three", 
        }, data);
    }

    [Fact]
    public void CanMatchIgnoreCase()
    {
        using MediaDictionary dict = new()
        {
            ["A1001"] = "100-One",
            ["a1002"] = "100-Two",
            ["A1003"] = "100-Three",
            ["a2001"] = "200-One",
        };
        List<KeyValuePair<string, string>> data = dict.QueryPrefix("A100", caseSensitive: false).ToList();
        Assert.Equal(new Dictionary<string, string>
        {
            ["A1001"] = "100-One",
            ["a1002"] = "100-Two",
            ["A1003"] = "100-Three",
        }, data);
    }

    [Fact]
    public void CanRemove()
    {
        using MediaDictionary dict = new()
        {
            ["A1001"] = "100-One",
            ["a1002"] = "100-Two",
            ["A1003"] = "100-Three",
            ["a2001"] = "200-One",
        };
        dict.Remove("A1001");
        Assert.Equal(3, dict.Count);
    }

    [Fact]
    public void CanSet()
    {
        using MediaDictionary dict = new()
        {
            ["A1001"] = "100-One",
            ["a1002"] = "100-Two",
            ["A1003"] = "100-Three",
            ["a2001"] = "200-One",
        };
        dict.Set("A1001", "A1001-val");
        Assert.Equal("A1001-val", dict["A1001"]);
    }

    [Fact]
    public void CanClear()
    {
        using MediaDictionary dict = new()
        {
            ["A1001"] = "100-One",
            ["a1002"] = "100-Two",
            ["A1003"] = "100-Three",
            ["a2001"] = "200-One",
        };
        dict.Clear();
        Assert.Empty(dict);
    }

    [Fact]
    public void CanClone()
    {
        using MediaDictionary dict = new()
        {
            ["A1001"] = "100-One",
            ["a1002"] = "100-Two",
            ["A1003"] = "100-Three",
            ["a2001"] = "200-One",
        };
        using MediaDictionary dict2 = dict.Clone();
        dict.Clear();
        Assert.Empty(dict);
        Assert.Equal(4, dict2.Count);
    }

    [Fact]
    public void CanAdd()
    {
        using MediaDictionary dict = new()
        {
            ["A1001"] = "100-One",
        };
        dict.Add("A1002", "100-Two");
        Assert.Equal(2, dict.Count);
    }

    [Fact]
    public void CanContainsKey()
    {
        using MediaDictionary dict = new()
        {
            ["A1001"] = "100-One",
        };
        Assert.True(dict.ContainsKey("A1001"));
        Assert.False(dict.ContainsKey("A1002"));
    }

    [Fact]
    public unsafe void CanResetAfterFree()
    {
        using MediaDictionary dict = new()
        {
            ["A1001"] = "100-One",
        };
        AVDictionary* ptr = (AVDictionary*)dict;
        ffmpeg.av_dict_free(&ptr);
        dict.Reset(ptr);
    }

    [Fact]
    public unsafe void ValuesTest()
    {
        using MediaDictionary dict = new()
        {
            ["A1001"] = "100-One",
        };
        string[] values = dict.Values.ToArray();
        Assert.Equal("100-One", values[0]);
    }
}
