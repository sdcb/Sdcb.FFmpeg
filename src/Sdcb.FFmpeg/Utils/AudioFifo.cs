using Sdcb.FFmpeg.Common;
using Sdcb.FFmpeg.Raw;
using System.Runtime.InteropServices;
using static Sdcb.FFmpeg.Raw.ffmpeg;

namespace Sdcb.FFmpeg.Utils;

public unsafe partial class AudioFifo : SafeHandle
{
    /// <summary>
    /// <see cref="av_audio_fifo_alloc"/>
    /// </summary>
    public AudioFifo(AVSampleFormat sampleFormat, int channels, int sampleSize)
        : this(av_audio_fifo_alloc(sampleFormat, channels, sampleSize), isOwner: true)
    {
    }

    /// <summary>
    /// <see cref="av_audio_fifo_realloc"/>
    /// </summary>
    public void Realloc(int sampleSize)
    {
        av_audio_fifo_realloc(this, sampleSize).ThrowIfError();
    }

    /// <summary>
    /// <para>Write data to an AVAudioFifo.</para>
    /// <para>The AVAudioFifo will be reallocated automatically if the available space is less than sampleCount.</para>
    /// <see cref="av_audio_fifo_write(AVAudioFifo*, void**, int)"/>
    /// </summary>
    /// <param name="data">audio data plane pointers</param>
    /// <param name="sampleCount">number of samples to write</param>
    /// <returns>number of samples actually written, or negative AVERROR code on failure. If successful, the number of samples actually written will always be nb_samples.</returns>
    public int Write(void** data, int sampleCount)
    {
        return av_audio_fifo_write(this, data, sampleCount).ThrowIfError();
    }

    public int Write(Frame frame)
    {
        byte_ptrArray8 data = frame.Data;
        void** ptr = (void**)&data;
        return av_audio_fifo_write(this, ptr, frame.NbSamples).ThrowIfError();
    }

    /// <summary>
    /// <para>Peek data from an AVAudioFifo.</para>
    /// <para>The AVAudioFifo will be reallocated automatically if the available space is less than sampleCount.</para>
    /// <see cref="av_audio_fifo_peek(AVAudioFifo*, void**, int)" />
    /// </summary>
    /// <param name="data">audio data plane pointers</param>
    /// <param name="sampleCount">number of samples to write</param>
    /// <returns>number of samples actually peek, or negative AVERROR code on failure. The number of samples actually peek will not be greater than nb_samples, and will only be less than nb_samples if av_audio_fifo_size is less than nb_samples.</returns>
    public int Peek(void** data, int sampleCount)
    {
        return av_audio_fifo_peek(this, data, sampleCount).ThrowIfError();
    }

    public int Peek(Frame frame)
    {
        byte_ptrArray8 data = frame.Data;
        void** ptr = (void**)&data;
        return av_audio_fifo_peek(this, ptr, frame.NbSamples).ThrowIfError();
    }

    /// <summary>
    /// <para>Peek data from an AVAudioFifo.</para>
    /// <para>The AVAudioFifo will be reallocated automatically if the available space is less than sampleCount.</para>
    /// <see cref="av_audio_fifo_peek(AVAudioFifo*, void**, int)" />
    /// </summary>
    /// <param name="data">audio data plane pointers</param>
    /// <param name="sampleCount">number of samples to write</param>
    /// <param name="offset">offset from current read position</param>
    /// <returns>number of samples actually peek, or negative AVERROR code on failure. The number of samples actually peek will not be greater than nb_samples, and will only be less than nb_samples if av_audio_fifo_size is less than nb_samples.</returns>
    public int PeekAt(void** data, int sampleCount, int offset)
    {
        return av_audio_fifo_peek_at(this, data, sampleCount, offset).ThrowIfError();
    }

    public int PeekAt(Frame frame, int offset)
    {
        byte_ptrArray8 data = frame.Data;
        void** ptr = (void**)&data;
        return PeekAt(ptr, frame.NbSamples, offset);
    }

    /// <summary>
    /// <para>Read data from an AVAudioFifo.</para>
    /// <see cref="av_audio_fifo_read(AVAudioFifo*, void**, int)"/>
    /// </summary>
    /// <param name="data">audio data plane pointers</param>
    /// <param name="sampleCount">number of samples to read</param>
    /// <returns>number of samples actually read, or negative AVERROR code on failure. The number of samples actually read will not be greater than nb_samples, and will only be less than nb_samples if av_audio_fifo_size is less than nb_samples.</returns>
    public int Read(void** data, int sampleCount)
    {
        return av_audio_fifo_read(this, data, sampleCount).ThrowIfError();
    }

    public int Read(Frame frame)
    {
        byte_ptrArray8 data = frame.Data;
        void** ptr = (void**)&data;
        return Read(ptr, frame.NbSamples);
    }

    /// <summary>
    /// <para>Drain data from an AVAudioFifo.</para>
    /// <para>Removes the data without reading it.</para>
    /// <see cref="av_audio_fifo_drain"/>
    /// </summary>
    /// <param name="sampleCount">number of samples to drain</param>
    public void Drain(int sampleCount)
    {
        av_audio_fifo_drain(this, sampleCount).ThrowIfError();
    }

    /// <summary>
    /// <para>Reset the AVAudioFifo buffer.</para>
    /// <para>This empties all data in the buffer.</para>
    /// <see cref="av_audio_fifo_reset"/>
    /// </summary>
    public void Reset()
    {
        av_audio_fifo_reset(this);
    }

    /// <summary>
    /// Get the current number of samples in the AVAudioFifo available for reading.
    /// <see cref="av_audio_fifo_size"/>
    /// </summary>
    public int Size => av_audio_fifo_size(this);

    /// <summary>
    /// Get the current number of samples in the AVAudioFifo available for writing.
    /// <see cref="av_audio_fifo_space"/>
    /// </summary>
    public int Space => av_audio_fifo_space(this);

    /// <summary>
    /// <see cref="av_audio_fifo_free"/>
    /// </summary>
    public void Free()
    {
        av_audio_fifo_free(this);
        SetHandleAsInvalid();
    }

    protected override bool ReleaseHandle()
    {
        Free();
        return true;
    }
}
