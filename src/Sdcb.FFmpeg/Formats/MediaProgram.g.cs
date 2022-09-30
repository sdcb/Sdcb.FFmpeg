// This file was genereated from Sdcb.FFmpeg.AutoGen, DO NOT CHANGE DIRECTLY.
#nullable enable
using Sdcb.FFmpeg.Common;
using Sdcb.FFmpeg.Codecs;
using Sdcb.FFmpeg.Utils;
using Sdcb.FFmpeg.Filters;
using Sdcb.FFmpeg.Raw;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Sdcb.FFmpeg.Formats;

/// <summary>
/// <para>New fields can be added to the end with minor version bumps. Removal, reordering and changes to existing fields require a major version bump. sizeof(AVProgram) must not be used outside libav*.</para>
/// <see cref="AVProgram" />
/// </summary>
public unsafe partial struct MediaProgram
{
    private AVProgram* _ptr;
    
    public static implicit operator AVProgram*(MediaProgram? data)
        => data.HasValue ? (AVProgram*)data.Value._ptr : null;
    
    private MediaProgram(AVProgram* ptr)
    {
        if (ptr == null)
        {
            throw new ArgumentNullException(nameof(ptr));
        }
        _ptr = ptr;
    }
    
    public static MediaProgram FromNative(AVProgram* ptr) => new MediaProgram(ptr);
    public static MediaProgram FromNative(IntPtr ptr) => new MediaProgram((AVProgram*)ptr);
    internal static MediaProgram? FromNativeOrNull(AVProgram* ptr)
        => ptr != null ? new MediaProgram?(new MediaProgram(ptr)) : null;
    
    /// <summary>
    /// <see cref="AVProgram.id" />
    /// </summary>
    public int Id
    {
        get => _ptr->id;
        set => _ptr->id = value;
    }
    
    /// <summary>
    /// <see cref="AVProgram.flags" />
    /// </summary>
    public int Flags
    {
        get => _ptr->flags;
        set => _ptr->flags = value;
    }
    
    /// <summary>
    /// <para>selects which program to discard and which to feed to the caller</para>
    /// <see cref="AVProgram.discard" />
    /// </summary>
    public AVDiscard Discard
    {
        get => _ptr->discard;
        set => _ptr->discard = value;
    }
    
    /// <summary>
    /// <para>original type: uint*</para>
    /// <see cref="AVProgram.stream_index" />
    /// </summary>
    public IReadOnlyList<uint> StreamIndex => new ReadOnlyNativeList<uint>(_ptr->stream_index, (int)_ptr->nb_stream_indexes)!;
    
    /// <summary>
    /// <para>original type: AVDictionary*</para>
    /// <see cref="AVProgram.metadata" />
    /// </summary>
    public MediaDictionary Metadata
    {
        get => MediaDictionary.FromNative(_ptr->metadata, false);
        set => _ptr->metadata = (AVDictionary*)value;
    }
    
    /// <summary>
    /// <see cref="AVProgram.program_num" />
    /// </summary>
    public int ProgramNum
    {
        get => _ptr->program_num;
        set => _ptr->program_num = value;
    }
    
    /// <summary>
    /// <see cref="AVProgram.pmt_pid" />
    /// </summary>
    public int PmtPid
    {
        get => _ptr->pmt_pid;
        set => _ptr->pmt_pid = value;
    }
    
    /// <summary>
    /// <see cref="AVProgram.pcr_pid" />
    /// </summary>
    public int PcrPid
    {
        get => _ptr->pcr_pid;
        set => _ptr->pcr_pid = value;
    }
    
    /// <summary>
    /// <see cref="AVProgram.pmt_version" />
    /// </summary>
    public int PmtVersion
    {
        get => _ptr->pmt_version;
        set => _ptr->pmt_version = value;
    }
    
    /// <summary>
    /// <para>*************************************************************** All fields below this line are not part of the public API. They may not be used outside of libavformat and can be changed and removed at will. New public fields should be added right above. ****************************************************************</para>
    /// <see cref="AVProgram.start_time" />
    /// </summary>
    public long StartTime
    {
        get => _ptr->start_time;
        set => _ptr->start_time = value;
    }
    
    /// <summary>
    /// <see cref="AVProgram.end_time" />
    /// </summary>
    public long EndTime
    {
        get => _ptr->end_time;
        set => _ptr->end_time = value;
    }
    
    /// <summary>
    /// <para>reference dts for wrap detection</para>
    /// <see cref="AVProgram.pts_wrap_reference" />
    /// </summary>
    public long PtsWrapReference
    {
        get => _ptr->pts_wrap_reference;
        set => _ptr->pts_wrap_reference = value;
    }
    
    /// <summary>
    /// <para>behavior on wrap detection</para>
    /// <see cref="AVProgram.pts_wrap_behavior" />
    /// </summary>
    public int PtsWrapBehavior
    {
        get => _ptr->pts_wrap_behavior;
        set => _ptr->pts_wrap_behavior = value;
    }
}
