using Sdcb.FFmpeg.Common;
using Sdcb.FFmpeg.Utils;
using Sdcb.FFmpeg.Raw;
using System;
using System.Runtime.InteropServices;
using static Sdcb.FFmpeg.Raw.ffmpeg;

namespace Sdcb.FFmpeg.Filters;

public unsafe partial class FilterContext : SafeHandle
{
    public FFmpegOptions Options => new FFmpegOptions(this);

    /// <summary>
    /// <para>Initialize a filter with the supplied parameters.</para>
    /// <see cref="avfilter_init_str"/>
    /// </summary>
    /// <param name="args">
    /// <para>Options to initialize the filter with. This must be a &apos;:&apos;-separated list of options in the &apos;key=value&apos; form.</para>
    /// <para>May be NULL if the options have been set directly using the AVOptions API or there are no options that need to be set.</para>
    /// </param>
    public void InitializeFromString(string? args)
    {
        avfilter_init_str(this, args).ThrowIfError();
    }

    /// <summary>
    /// <para>Initialize a filter with the supplied dictionary of options.</para>
    /// <see cref="avfilter_init_dict(AVFilterContext*, AVDictionary**)"/>
    /// </summary>
    /// <param name="options">An AVDictionary filled with options for this filter. On return this parameter will be destroyed and replaced with a dict containing options that were not found. This dictionary must be freed by the caller. May be NULL, then this function is equivalent to avfilter_init_str() with the second parameter set to NULL.</param>
    public void InitializeFromDictionary(MediaDictionary options)
    {
        AVDictionary* dictPtr = options;
        avfilter_init_dict(this, &dictPtr).ThrowIfError();
        options.Reset(dictPtr);
    }

    /// <summary>
    /// Returns AVClass for AVFilterContext.
    /// <see cref="avfilter_get_class"/>
    /// </summary>
    public static FFmpegClass FFmpegClass => FFmpegClass.FromNative(avfilter_get_class());

    /// <summary>
    /// <para>Link two filters together.</para>
    /// <see cref="avfilter_link"/>
    /// </summary>
    /// <param name="sourcePadIndex">index of the output pad on the source filter</param>
    /// <param name="destination">the destination filter</param>
    /// <param name="destinationPadIndex">index of the input pad on the destination filter</param>
    public void Link(FilterContext destination, int sourcePadIndex = 0, int destinationPadIndex = 0)
    {
        avfilter_link(this, (uint)sourcePadIndex, destination, (uint)destinationPadIndex).ThrowIfError();
    }

    /// <summary>
    /// <para>Negotiate the media format, dimensions, etc of all inputs to a filter.</para>
    /// <see cref="avfilter_config_links"/>
    /// </summary>
    public void ConfigLinks()
    {
        avfilter_config_links(this).ThrowIfError();
    }

    /// <summary>Make the filter instance process a command. It is recommended to use <see cref="FilterGraph.SendCommand"/>.</summary>
    public void ProcessCommand(string command, string args, IntPtr buffer, int bufferLength, AVFILTER_CMD_FLAG flags = default)
    {
        avfilter_process_command(this, command, args, (byte*)buffer, bufferLength, (int)flags).ThrowIfError();
    }

    /// <summary>
    /// <para>Get a frame with filtered data from sink and put it in frame.</para>
    /// <see cref="av_buffersink_get_frame_flags"/>
    /// </summary>
    /// <param name="frame">pointer to an allocated frame that will be filled with data. The data must be freed using av_frame_unref() / av_frame_free()</param>
    /// <returns>
    /// <list type="bullet">
    /// <item>&gt;= 0 if a frame was successfully returned.</item>
    /// <item>AVERROR(EAGAIN) if no frames are available at this point; more input frames must be added to the filtergraph to get more output.</item>
    /// <item>AVERROR_EOF if there will be no more output frames on this sink.</item>
    /// <item>A different negative AVERROR code in other failure cases.</item>
    /// </list>
    /// </returns>
    public int GetFrame(Frame frame, AV_BUFFERSINK_FLAG flags = default)
    {
        return av_buffersink_get_frame_flags(this, frame, (int)flags);
    }

    /// <summary>
    /// <para>Same as av_buffersink_get_frame(), but with the ability to specify the number of samples read.</para>
    /// <para>This function is less efficient than av_buffersink_get_frame(), because it copies the data around.</para>
    /// <see cref="av_buffersink_get_samples" />
    /// </summary>
    /// <param name="frame">
    /// <para>pointer to an allocated frame that will be filled with data. The data must be freed using av_frame_unref() / av_frame_free()</para>
    /// <para>frame will contain exactly nb_samples audio samples, except at the end of stream, when it can contain less than sampleCount.</para>
    /// </param>
    /// <returns>
    /// <list type="bullet">
    /// <item>&gt;= 0 if a frame was successfully returned.</item>
    /// <item>AVERROR(EAGAIN) if no frames are available at this point; more input frames must be added to the filtergraph to get more output.</item>
    /// <item>AVERROR_EOF if there will be no more output frames on this sink.</item>
    /// <item>A different negative AVERROR code in other failure cases.</item>
    /// </list>
    /// </returns>
    public int GetFrameSamples(Frame frame, int sampleCount)
    {
        return av_buffersink_get_samples(this, frame, sampleCount);
    }

    /// <summary>
    /// <para>Set the frame size for an audio buffer sink.</para>
    /// <para>All calls to av_buffersink_get_buffer_ref will return a buffer with exactly the specified number of samples, or AVERROR(EAGAIN) if there is not enough. The last buffer at EOF will be padded with 0.</para>
    /// <see cref="av_buffersink_set_frame_size"/>
    /// </summary>
    /// <param name="size"></param>
    public void SetFrameSize(int size)
    {
        av_buffersink_set_frame_size(this, (uint)size);
    }

    /// <summary>
    /// <para>Free a filter context. This will also remove the filter from its filtergraph&apos;s list of filters.</para>
    /// <see cref="avfilter_free"/>
    /// </summary>
    public void Free() => avfilter_free(this);

    protected override bool ReleaseHandle()
    {
        Free();
        SetHandleAsInvalid();
        return true;
    }
}
