using Sdcb.FFmpeg.Common;
using Sdcb.FFmpeg.Utils;
using Sdcb.FFmpeg.Raw;
using System;
using System.Runtime.InteropServices;
using static Sdcb.FFmpeg.Raw.ffmpeg;

namespace Sdcb.FFmpeg.Filters;


public unsafe partial class FilterGraph : SafeHandle
{
    /// <summary>
    /// Allocate a filter graph.
    /// <see cref="avfilter_graph_alloc"/>
    /// </summary>
    public FilterGraph() : this(avfilter_graph_alloc(), isOwner: true)
    {
    }

    public FFmpegOptions Options => new FFmpegOptions(this);

    /// <summary>
    /// <para>Create a new filter instance in a filter graph.</para>
    /// <see cref="avfilter_graph_alloc_filter"/>
    /// </summary>
    /// <param name="filter">the filter to create an instance of</param>
    /// <param name="name">Name to give to the new instance (will be copied to AVFilterContext.name). This may be used by the caller to identify different filters, libavfilter itself assigns no semantics to this parameter. May be NULL.</param>
    public FilterContext AllocFilter(Filter filter, string? name = null)
    {
        return FilterContext.FromNative(avfilter_graph_alloc_filter(this, filter, name), isOwner: false);
    }

    /// <summary>
    /// <para>Create a new filter instance in a filter graph.</para>
    /// <see cref="avfilter_graph_alloc_filter"/>
    /// </summary>
    /// <param name="filterName">the filter name to create an instance of</param>
    /// <param name="name">Name to give to the new instance (will be copied to AVFilterContext.name). This may be used by the caller to identify different filters, libavfilter itself assigns no semantics to this parameter. May be NULL.</param>
    public FilterContext AllocFilter(string filterName, string? name = null) => AllocFilter(Filter.GetByName(filterName), name);

    /// <summary>
    /// <para>original API: <see cref="avfilter_graph_create_filter(AVFilterContext**, AVFilter*, string, string, void*, AVFilterGraph*)"/></para>
    /// short-hand API for calling following APIs at once:
    /// <para><see cref="AllocFilter(Filter, string?)"/></para>
    /// <para><see cref="FilterContext.InitializeFromString"/></para>
    /// </summary>
    /// <param name="name">the instance name to give to the created filter instance</param>
    public FilterContext CreateFilter(Filter filter, string? name = null, string? args = null)
    {
        AVFilterContext* resultPtr;
        avfilter_graph_create_filter(&resultPtr, filter, name, args, null, this).ThrowIfError();
        return FilterContext.FromNative(resultPtr, isOwner: false);
    }

    /// <summary>
    /// Short hand api for <see cref="AllocFilter(Filter, string?)"/> and <see cref="FilterContext.InitializeFromDictionary"/>
    /// </summary>
    public FilterContext CreateFilter(string filterName, string name, MediaDictionary opts)
    {
        FilterContext ctx = AllocFilter(filterName, name);
        ctx.InitializeFromDictionary(opts);
        return ctx;
    }

    /// <summary>
    /// Short hand api for <see cref="AllocFilter(Filter, string?)"/> and <see cref="FilterContext.InitializeFromDictionary"/>
    /// </summary>
    public FilterContext CreateFilter(Filter filter, string name, MediaDictionary opts)
    {
        FilterContext ctx = AllocFilter(filter, name);
        ctx.InitializeFromDictionary(opts);
        return ctx;
    }

    /// <summary>
    /// <para>original API: <see cref="avfilter_graph_create_filter(AVFilterContext**, AVFilter*, string, string, void*, AVFilterGraph*)"/></para>
    /// short-hand API for calling following APIs at once:
    /// <para><see cref="AllocFilter(string, string?)"/></para>
    /// <para><see cref="FilterContext.InitializeFromString"/></para>
    /// </summary>
    /// <param name="name">the instance name to give to the created filter instance</param>
    public FilterContext CreateFilter(string filterName, string? name = null, string? args = null) => CreateFilter(Filter.GetByName(filterName), name, args);

    /// <summary>
    /// Get a filter instance identified by instance name from graph.
    /// <see cref="avfilter_graph_get_filter"/>
    /// </summary>
    /// <param name="name">filter instance name (should be unique in the graph).</param>
    public FilterContext GetFilter(string name)
    {
        return FilterContext.FromNative(avfilter_graph_get_filter(this, name), isOwner: false);
    }

    /// <summary>
    /// <para>Enable or disable automatic format conversion inside the graph.</para>
    /// <see cref="avfilter_graph_set_auto_convert"/>
    /// </summary>
    public void SetAutoConvert(bool enabled)
    {
        avfilter_graph_set_auto_convert(this, (uint)(enabled ? 0 : -1));
    }

    /// <summary>
    /// <para>Check validity and configure all the links and formats in the graph.</para>
    /// <see cref="avfilter_graph_config"/>
    /// </summary>
    /// <param name="loggingContext">context used for logging</param>
    public void Configure(FFmpegClass? loggingContext = null)
    {
        avfilter_graph_config(this, loggingContext != null ? (AVClass*)loggingContext : null).ThrowIfError();
    }

    /// <summary>
    /// <para>Add a graph described by a string to a graph.</para>
    /// <see cref="avfilter_graph_parse(AVFilterGraph*, string, AVFilterInOut*, AVFilterInOut*, void*)"/>
    /// <para>Note:
    /// <list type="bullet">
    /// <item>The caller must provide the lists of inputs and outputs, which therefore must be known before calling the function.</item>
    /// <item>The inputs parameter describes inputs of the already existing part of the graph; i.e. from the point of view of the newly created part, they are outputs. Similarly the outputs parameter describes outputs of the already existing filters, which are provided as inputs to the parsed filters.</item>
    /// </list>
    /// </para>
    /// </summary>
    /// <param name="filters">string to be parsed</param>
    /// <param name="inputs">linked list to the inputs of the graph</param>
    /// <param name="outputs">linked list to the outputs of the graph</param>
    public void Parse(string filters, FilterInOut inputs, FilterInOut outputs, FFmpegClass? loggingContext = null)
    {
        AVFilterInOut* ptrInputs = inputs;
        AVFilterInOut* ptrOutputs = outputs;
        avfilter_graph_parse_ptr(this, filters, &ptrInputs, &ptrOutputs, loggingContext != null ? (AVClass*)loggingContext : null).ThrowIfError();
        inputs.Reset((IntPtr)ptrInputs);
        outputs.Reset((IntPtr)ptrOutputs);
    }

    /// <summary>
    /// <para>Add a graph described by a string to a graph.</para>
    /// <see cref="avfilter_graph_parse_ptr(AVFilterGraph*, string, AVFilterInOut**, AVFilterInOut**, void*)"/>
    /// </summary>
    /// <param name="filters">string to be parsed</param>
    /// <param name="inputs">pointer to a linked list to the inputs of the graph, may be NULL. If non-NULL, *inputs is updated to contain the list of open inputs after the parsing, should be freed with avfilter_inout_free().</param>
    /// <param name="outputs">pointer to a linked list to the outputs of the graph, may be NULL. If non-NULL, *outputs is updated to contain the list of open outputs after the parsing, should be freed with avfilter_inout_free().</param>
    public void ParsePtr(string filters, FilterInOut inputs, FilterInOut outputs, FFmpegClass? loggingContext = null)
    {
        AVFilterInOut* ptrInputs = inputs;
        AVFilterInOut* ptrOutputs = outputs;
        avfilter_graph_parse_ptr(this, filters, &ptrInputs, &ptrOutputs, loggingContext != null ? (AVClass*)loggingContext : null).ThrowIfError();
        inputs.Reset((IntPtr)ptrInputs);
        outputs.Reset((IntPtr)ptrOutputs);
    }

    /// <summary>
    /// <para>Add a graph described by a string to a graph.</para>
    /// <see cref="avfilter_graph_parse2(AVFilterGraph*, string, AVFilterInOut**, AVFilterInOut**)"/>
    /// <para>Note:</para>
    /// <list type="bullet">
    /// <item>This function returns the inputs and outputs that are left unlinked after parsing the graph and the caller then deals with them.</item>
    /// <item>This function makes no reference whatsoever to already existing parts of the graph and the inputs parameter will on return contain inputs of the newly parsed part of the graph.  Analogously the outputs parameter will contain outputs of the newly created filters.</item>
    /// </list>
    /// </summary>
    /// <param name="filters">string to be parsed</param>
    /// <param name="inputs">a linked list of all free (unlinked) inputs of the parsed graph will be returned here. It is to be freed by the caller using avfilter_inout_free().</param>
    /// <param name="outputs">a linked list of all free (unlinked) outputs of the parsed graph will be returned here. It is to be freed by the caller using avfilter_inout_free().</param>
    public void Parse2(string filters, FilterInOut inputs, FilterInOut outputs)
    {
        AVFilterInOut* ptrInputs = inputs;
        AVFilterInOut* ptrOutputs = outputs;
        avfilter_graph_parse2(this, filters, &ptrInputs, &ptrOutputs).ThrowIfError();
        inputs.Reset((IntPtr)ptrInputs);
        outputs.Reset((IntPtr)ptrOutputs);
    }

    /// <summary>
    /// <para>Send a command to one or more filter instances.</para>
    /// <see cref="avfilter_graph_send_command"/>
    /// </summary>
    /// <param name="target">the filter(s) to which the command should be sent &quot;all&quot; sends to all filters otherwise it can be a filter or filter instance name which will send the command to all matching filters.</param>
    /// <param name="command">the command to send, for handling simplicity all commands must be alphanumeric only</param>
    /// <param name="args">the argument for the command</param>
    /// <param name="buffer">a buffer with size res_size where the filter(s) can return a response.</param>
    public void SendCommand(string target, string command, string args, IntPtr buffer, int bufferLength, AVFILTER_CMD_FLAG flags = default)
    {
        avfilter_graph_send_command(this, target, command, args, (byte*)buffer, bufferLength, (int)flags).ThrowIfError();
    }

    /// <summary>
    /// <para>Queue a command for one or more filter instances.</para>
    /// <see cref="avfilter_graph_queue_command"/>
    /// </summary>
    /// <param name="target">the filter(s) to which the command should be sent &quot;all&quot; sends to all filters otherwise it can be a filter or filter instance name which will send the command to all matching filters.</param>
    /// <param name="command">the command to sent, for handling simplicity all commands must be alphanumeric only</param>
    /// <param name="args">the argument for the command</param>
    /// <param name="ts">time at which the command should be sent to the filter</param>
    public void QueueCommand(string target, string command, string args, double ts, AVFILTER_CMD_FLAG flags = default)
    {
        avfilter_graph_queue_command(this, target, command, args, (int)flags, ts).ThrowIfError();
    }

    /// <summary>
    /// <para>Dump a graph into a human-readable string representation.</para>
    /// <see cref="avfilter_graph_dump"/>
    /// </summary>
    public DisposableNativeString DumpToString() => new DisposableNativeString(avfilter_graph_dump(this, null));

    /// <summary>
    /// <para>Request a frame on the oldest sink link.</para>
    /// <para>If the request returns AVERROR_EOF, try the next.</para>
    /// <para>Note that this function is not meant to be the sole scheduling mechanism of a filtergraph, only a convenience function to help drain a filtergraph in a balanced way under normal circumstances.</para>
    /// <para>Also note that AVERROR_EOF does not mean that frames did not arrive on some of the sinks during the process. When there are multiple sink links, in case the requested link returns an EOF, this may cause a filter to flush pending frames which are sent to another sink link, although unrequested.</para>
    /// <see cref="avfilter_graph_request_oldest"/>
    /// <returns>the return value of ff_request_frame(), or AVERROR_EOF if all links returned AVERROR_EOF</returns>
    /// </summary>
    public int RequestOldest()
    {
        return avfilter_graph_request_oldest(this);
    }

    /// <summary>Free a graph, destroy its links, and set *graph to NULL. If *graph is NULL, do nothing.</summary>
    /// <see cref="avfilter_graph_free(AVFilterGraph**)"/>
    public void Free()
    {
        AVFilterGraph* graph = this;
        avfilter_graph_free(&graph);
        handle = (IntPtr)graph;
        SetHandleAsInvalid();
    }

    protected override bool ReleaseHandle()
    {
        Free();
        return true;
    }
}
