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
    /// <para>Get the number of failed requests.</para>
    /// <para>A failed request is when the request_frame method is called while no frame is present in the buffer.</para>
    /// <para>The number is reset when a frame is added.</para>
    /// <see cref="av_buffersrc_get_nb_failed_requests"/>
    /// </summary>
    public int GetFailedRequestCount()
    {
        return (int)av_buffersrc_get_nb_failed_requests(this);
    }

    /// <summary>
    /// <para>Initialize the buffersrc or abuffersrc filter with the provided parameters.</para>
    /// <para>This function may be called multiple times, the later calls override the previous ones. Some of the parameters may also be set through AVOptions, then whatever method is used last takes precedence.</para>
    /// <see cref="av_buffersrc_parameters_set"/>
    /// </summary>
    /// <param name="param">the stream parameters. The frames later passed to this filter must conform to those parameters. All the allocated fields in param remain owned by the caller, libavfilter will make internal copies or references when necessary.</param>
    public void SetParameters(BufferSrcParameters param)
    {
        av_buffersrc_parameters_set(this, param).ThrowIfError();
    }

    /// <summary>
    /// <para>Add a frame to the buffer source.</para>
    /// <para>This function is equivalent to av_buffersrc_add_frame_flags() with the AV_BUFFERSRC_FLAG_KEEP_REF flag.</para>
    /// <see cref="av_buffersrc_write_frame"/>
    /// </summary>
    /// <param name="frame">frame to be added. If the frame is reference counted, this function will make a new reference to it. Otherwise the frame data will be copied.</param>
    public void WriteFrame(Frame? frame)
    {
        av_buffersrc_write_frame(this, frame!).ThrowIfError();
    }

    /// <summary>
    /// <para>Add a frame to the buffer source.</para>
    /// <para>This function is equivalent to av_buffersrc_add_frame_flags() without the AV_BUFFERSRC_FLAG_KEEP_REF flag.</para>
    /// <para>the difference between this function and av_buffersrc_write_frame() is that av_buffersrc_write_frame() creates a new reference to the input frame, while this function takes ownership of the reference passed to it.</para>
    /// </summary>
    /// <param name="frame">frame to be added. If the frame is reference counted, this function will make a new reference to it. Otherwise the frame data will be copied. If this function returns an error, the input frame is not touched.</param>
    public void AddFrame(Frame? frame)
    {
        av_buffersrc_add_frame(this, frame!).ThrowIfError();
    }

    /// <summary>
    /// <para>Add a frame to the buffer source.</para>
    /// <para>By default, if the frame is reference-counted, this function will take ownership of the reference(s) and reset the frame. This can be controlled using the flags.</para>
    /// <para>If this function returns an error, the input frame is not touched.</para>
    /// <see cref="av_buffersrc_add_frame_flags"/>
    /// </summary>
    /// <param name="frame">a frame, or NULL to mark EOF</param>
    public void AddFrame(Frame? frame, AV_BUFFERSRC_FLAG flags)
    {
        av_buffersrc_add_frame_flags(this, frame!, (int)flags).ThrowIfError();
    }

    /// <summary>
    /// <para>Close the buffer source after EOF.</para>
    /// <see cref="av_buffersrc_close"/>
    /// </summary>
    public void CloseBufferSource(long pts, AV_BUFFERSRC_FLAG flags = AV_BUFFERSRC_FLAG.Push)
    {
        av_buffersrc_close(this, pts, (uint)flags).ThrowIfError();
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
