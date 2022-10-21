using Sdcb.FFmpeg.Codecs;
using Sdcb.FFmpeg.Raw;
using System.Collections.Generic;
using System.Linq;

namespace Sdcb.FFmpeg.Toolboxs.Extensions;

public static class CodecExtensions
{
    public static AVSampleFormat NegociateSampleFormat(this Codec codec, AVSampleFormat suggestedSampleFormat) => 
        NegociateOption(codec.SampleFormats, suggestedSampleFormat);

    public static int NegociateSampleRates(this Codec codec, int suggestedSampleRate) =>
        NegociateOption(codec.SupportedSamplerates, suggestedSampleRate);

    private static T NegociateOption<T>(IEnumerable<T> source, T suggestedOption) => source.ToList() switch
    {
        [] => suggestedOption,
        var x when x.Contains(suggestedOption) => suggestedOption,
        [var x, ..] => x,
    };
}
