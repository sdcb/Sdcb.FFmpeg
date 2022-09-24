using Sdcb.FFmpeg.Common;

namespace Sdcb.FFmpeg.Codecs;

public class CommonEncoders
{
    /// <summary>
    /// x264 is a free software library and application for encoding video streams into the H.264/MPEG-4 AVC compression format, and is released under the terms of the GNU GPL.
    /// </summary>
    public Codec Libx264 => EnsureEncoderAvailable("libx264");

    /// <summary>
    /// x264 is a free software library and application for encoding video streams into the H.264/MPEG-4 AVC compression format, and is released under the terms of the GNU GPL.
    /// </summary>
    public Codec Libx264RGB => EnsureEncoderAvailable("libx264rgb");

    /// <summary>
    /// <para>x265 is a free software library and application for encoding video streams into the H.265/MPEG-H HEVC compression format, and is released under the terms of the GNU GPL.</para>
    /// libx265 can offer around 25–50% bitrate savings compared to H.264 video encoded with libx264, while retaining the same visual quality.
    /// </summary>
    public Codec Libx265 => EnsureEncoderAvailable("libx265");

    /// <summary>
    /// <para>libvpx-vp9 is the VP9 video encoder for ​WebM, an open, royalty-free media file format. </para>
    /// <para>libvpx-vp9 can save about 20–50% bitrate compared to libx264 (the default H.264 encoder), while retaining the same visual quality.</para>
    /// </summary>
    public Codec Libvpx_vp9 => EnsureEncoderAvailable("libvpx-vp9");

    /// <summary>
    /// libvpx is the VP8 video encoder for ​WebM, an open, royalty-free media file format.
    /// </summary>
    public Codec Libvpx => EnsureEncoderAvailable("libvpx");

    /// <summary>
    /// <para>The native FFmpeg AAC encoder. </para>
    /// <para>This is currently the second highest-quality AAC encoder available in FFmpeg and does not require an external library like the other AAC encoders described here. </para>
    /// <para>This is the default AAC encoder.</para>
    /// </summary>
    public Codec AAC => EnsureEncoderAvailable("aac");

    /// <summary>
    /// LAME is a high quality MPEG Audio Layer III (MP3) encoder licensed under the LGPL.
    /// </summary>
    public Codec Libmp3lame => EnsureEncoderAvailable("libmp3lame");

    /// <summary>
    /// <para>Opus is a totally open, royalty-free, highly versatile audio codec. </para>
    /// <para>Opus is unmatched for interactive speech and music transmission over the Internet, but is also intended for storage and streaming applications. </para>
    /// <para>It is standardized by the Internet Engineering Task Force (IETF) as RFC 6716 which incorporated technology from Skype’s SILK codec and Xiph.Org’s CELT codec.</para>
    /// </summary>
    public Codec Libopus => EnsureEncoderAvailable("libopus");

    /// <summary>
    /// GIF image/animation encoder.
    /// </summary>
    public Codec Gif => EnsureEncoderAvailable("gif");

    private Codec EnsureEncoderAvailable(string name)
    {
        Codec? codec = Codec.FindEncoderByName(name);
        if (codec == null)
        {
            throw new FFmpegException($"Encoder {name} not found.");
        }

        return codec.Value;
    }
}
