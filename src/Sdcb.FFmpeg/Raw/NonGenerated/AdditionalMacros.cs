using System;

namespace Sdcb.FFmpeg.Raw
{
    [Flags]
    public enum AV_BUFFERSRC_FLAG
    {
        Default = 0, 

        /// <summary>
        /// <para>Do not check for format changes.</para>
        /// AV_BUFFERSRC_FLAG_NO_CHECK_FORMAT
        /// </summary>
        NoCheckFormat = 1,

        /// <summary>
        /// <para>Immediately push the frame to the output.</para>
        /// AV_BUFFERSRC_FLAG_PUSH
        /// </summary>
        Push = 4,

        /// <summary>
        /// <para>Keep a reference to the frame.</para>
        /// <para>If the frame if reference-counted, create a new reference; otherwise copy the frame data.</para>
        /// AV_BUFFERSRC_FLAG_KEEP_REF
        /// </summary>
        KeepRef = 8
    }
}
