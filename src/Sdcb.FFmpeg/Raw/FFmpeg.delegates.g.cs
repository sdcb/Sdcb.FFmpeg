using System;
using System.Runtime.InteropServices;

#pragma warning disable 169
#pragma warning disable CS0649
#pragma warning disable CS0108
namespace Sdcb.FFmpeg.Raw
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate string AVClass_item_name (void* ctx);
    public unsafe record struct AVClass_item_name_func(IntPtr Pointer)
    {
        public static implicit operator AVClass_item_name_func(AVClass_item_name func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate AVClassCategory AVClass_get_category (void* ctx);
    public unsafe record struct AVClass_get_category_func(IntPtr Pointer)
    {
        public static implicit operator AVClass_get_category_func(AVClass_get_category func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int AVClass_query_ranges (AVOptionRanges** p0, void* obj, [MarshalAs(UnmanagedType.LPUTF8Str)] string key, int flags);
    public unsafe record struct AVClass_query_ranges_func(IntPtr Pointer)
    {
        public static implicit operator AVClass_query_ranges_func(AVClass_query_ranges func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void* AVClass_child_next (void* obj, void* prev);
    public unsafe record struct AVClass_child_next_func(IntPtr Pointer)
    {
        public static implicit operator AVClass_child_next_func(AVClass_child_next func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate AVClass* AVClass_child_class_iterate (void** iter);
    public unsafe record struct AVClass_child_class_iterate_func(IntPtr Pointer)
    {
        public static implicit operator AVClass_child_class_iterate_func(AVClass_child_class_iterate func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void av_log_set_callback_callback (void* p0, int p1, [MarshalAs(UnmanagedType.LPUTF8Str)] string p2, byte* p3);
    public unsafe record struct av_log_set_callback_callback_func(IntPtr Pointer)
    {
        public static implicit operator av_log_set_callback_callback_func(av_log_set_callback_callback func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void av_buffer_create_free (void* opaque, byte* data);
    public unsafe record struct av_buffer_create_free_func(IntPtr Pointer)
    {
        public static implicit operator av_buffer_create_free_func(av_buffer_create_free func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate AVBufferRef* av_buffer_pool_init_alloc (ulong size);
    public unsafe record struct av_buffer_pool_init_alloc_func(IntPtr Pointer)
    {
        public static implicit operator av_buffer_pool_init_alloc_func(av_buffer_pool_init_alloc func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate AVBufferRef* av_buffer_pool_init2_alloc (void* opaque, ulong size);
    public unsafe record struct av_buffer_pool_init2_alloc_func(IntPtr Pointer)
    {
        public static implicit operator av_buffer_pool_init2_alloc_func(av_buffer_pool_init2_alloc func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void av_buffer_pool_init2_pool_free (void* opaque);
    public unsafe record struct av_buffer_pool_init2_pool_free_func(IntPtr Pointer)
    {
        public static implicit operator av_buffer_pool_init2_pool_free_func(av_buffer_pool_init2_pool_free func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int av_tree_find_cmp (void* key, void* b);
    public unsafe record struct av_tree_find_cmp_func(IntPtr Pointer)
    {
        public static implicit operator av_tree_find_cmp_func(av_tree_find_cmp func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int av_tree_insert_cmp (void* key, void* b);
    public unsafe record struct av_tree_insert_cmp_func(IntPtr Pointer)
    {
        public static implicit operator av_tree_insert_cmp_func(av_tree_insert_cmp func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int av_tree_enumerate_cmp (void* opaque, void* elem);
    public unsafe record struct av_tree_enumerate_cmp_func(IntPtr Pointer)
    {
        public static implicit operator av_tree_enumerate_cmp_func(av_tree_enumerate_cmp func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int av_tree_enumerate_enu (void* opaque, void* elem);
    public unsafe record struct av_tree_enumerate_enu_func(IntPtr Pointer)
    {
        public static implicit operator av_tree_enumerate_enu_func(av_tree_enumerate_enu func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void AVHWDeviceContext_free (AVHWDeviceContext* ctx);
    public unsafe record struct AVHWDeviceContext_free_func(IntPtr Pointer)
    {
        public static implicit operator AVHWDeviceContext_free_func(AVHWDeviceContext_free func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void AVHWFramesContext_free (AVHWFramesContext* ctx);
    public unsafe record struct AVHWFramesContext_free_func(IntPtr Pointer)
    {
        public static implicit operator AVHWFramesContext_free_func(AVHWFramesContext_free func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void AVD3D11VADeviceContext_lock (void* lock_ctx);
    public unsafe record struct AVD3D11VADeviceContext_lock_func(IntPtr Pointer)
    {
        public static implicit operator AVD3D11VADeviceContext_lock_func(AVD3D11VADeviceContext_lock func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void AVD3D11VADeviceContext_unlock (void* lock_ctx);
    public unsafe record struct AVD3D11VADeviceContext_unlock_func(IntPtr Pointer)
    {
        public static implicit operator AVD3D11VADeviceContext_unlock_func(AVD3D11VADeviceContext_unlock func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void AVCodecContext_draw_horiz_band (AVCodecContext* s, AVFrame* src, ref int_array8 offset, int y, int type, int height);
    public unsafe record struct AVCodecContext_draw_horiz_band_func(IntPtr Pointer)
    {
        public static implicit operator AVCodecContext_draw_horiz_band_func(AVCodecContext_draw_horiz_band func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate AVPixelFormat AVCodecContext_get_format (AVCodecContext* s, AVPixelFormat* fmt);
    public unsafe record struct AVCodecContext_get_format_func(IntPtr Pointer)
    {
        public static implicit operator AVCodecContext_get_format_func(AVCodecContext_get_format func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int AVCodecContext_get_buffer2 (AVCodecContext* s, AVFrame* frame, int flags);
    public unsafe record struct AVCodecContext_get_buffer2_func(IntPtr Pointer)
    {
        public static implicit operator AVCodecContext_get_buffer2_func(AVCodecContext_get_buffer2 func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int AVHWAccel_alloc_frame (AVCodecContext* avctx, AVFrame* frame);
    public unsafe record struct AVHWAccel_alloc_frame_func(IntPtr Pointer)
    {
        public static implicit operator AVHWAccel_alloc_frame_func(AVHWAccel_alloc_frame func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int AVHWAccel_start_frame (AVCodecContext* avctx, byte* buf, uint buf_size);
    public unsafe record struct AVHWAccel_start_frame_func(IntPtr Pointer)
    {
        public static implicit operator AVHWAccel_start_frame_func(AVHWAccel_start_frame func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int AVHWAccel_decode_params (AVCodecContext* avctx, int type, byte* buf, uint buf_size);
    public unsafe record struct AVHWAccel_decode_params_func(IntPtr Pointer)
    {
        public static implicit operator AVHWAccel_decode_params_func(AVHWAccel_decode_params func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int AVHWAccel_decode_slice (AVCodecContext* avctx, byte* buf, uint buf_size);
    public unsafe record struct AVHWAccel_decode_slice_func(IntPtr Pointer)
    {
        public static implicit operator AVHWAccel_decode_slice_func(AVHWAccel_decode_slice func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int AVHWAccel_end_frame (AVCodecContext* avctx);
    public unsafe record struct AVHWAccel_end_frame_func(IntPtr Pointer)
    {
        public static implicit operator AVHWAccel_end_frame_func(AVHWAccel_end_frame func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int AVHWAccel_init (AVCodecContext* avctx);
    public unsafe record struct AVHWAccel_init_func(IntPtr Pointer)
    {
        public static implicit operator AVHWAccel_init_func(AVHWAccel_init func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int AVHWAccel_uninit (AVCodecContext* avctx);
    public unsafe record struct AVHWAccel_uninit_func(IntPtr Pointer)
    {
        public static implicit operator AVHWAccel_uninit_func(AVHWAccel_uninit func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int AVHWAccel_frame_params (AVCodecContext* avctx, AVBufferRef* hw_frames_ctx);
    public unsafe record struct AVHWAccel_frame_params_func(IntPtr Pointer)
    {
        public static implicit operator AVHWAccel_frame_params_func(AVHWAccel_frame_params func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int AVCodecContext_execute (AVCodecContext* c, func_func func, void* arg2, int* ret, int count, int size);
    public unsafe record struct AVCodecContext_execute_func(IntPtr Pointer)
    {
        public static implicit operator AVCodecContext_execute_func(AVCodecContext_execute func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int AVCodecContext_execute2 (AVCodecContext* c, func_func func, void* arg2, int* ret, int count);
    public unsafe record struct AVCodecContext_execute2_func(IntPtr Pointer)
    {
        public static implicit operator AVCodecContext_execute2_func(AVCodecContext_execute2 func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int AVCodecContext_get_encode_buffer (AVCodecContext* s, AVPacket* pkt, int flags);
    public unsafe record struct AVCodecContext_get_encode_buffer_func(IntPtr Pointer)
    {
        public static implicit operator AVCodecContext_get_encode_buffer_func(AVCodecContext_get_encode_buffer func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int AVCodecParser_parser_init (AVCodecParserContext* s);
    public unsafe record struct AVCodecParser_parser_init_func(IntPtr Pointer)
    {
        public static implicit operator AVCodecParser_parser_init_func(AVCodecParser_parser_init func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int AVCodecParser_parser_parse (AVCodecParserContext* s, AVCodecContext* avctx, byte** poutbuf, int* poutbuf_size, byte* buf, int buf_size);
    public unsafe record struct AVCodecParser_parser_parse_func(IntPtr Pointer)
    {
        public static implicit operator AVCodecParser_parser_parse_func(AVCodecParser_parser_parse func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void AVCodecParser_parser_close (AVCodecParserContext* s);
    public unsafe record struct AVCodecParser_parser_close_func(IntPtr Pointer)
    {
        public static implicit operator AVCodecParser_parser_close_func(AVCodecParser_parser_close func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int AVCodecParser_split (AVCodecContext* avctx, byte* buf, int buf_size);
    public unsafe record struct AVCodecParser_split_func(IntPtr Pointer)
    {
        public static implicit operator AVCodecParser_split_func(AVCodecParser_split func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int avcodec_default_execute_func (AVCodecContext* c2, void* arg2);
    public unsafe record struct avcodec_default_execute_func_func(IntPtr Pointer)
    {
        public static implicit operator avcodec_default_execute_func_func(avcodec_default_execute_func func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int avcodec_default_execute2_func (AVCodecContext* c2, void* arg2, int p2, int p3);
    public unsafe record struct avcodec_default_execute2_func_func(IntPtr Pointer)
    {
        public static implicit operator avcodec_default_execute2_func_func(avcodec_default_execute2_func func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int AVInputFormat_read_probe (AVProbeData* p0);
    public unsafe record struct AVInputFormat_read_probe_func(IntPtr Pointer)
    {
        public static implicit operator AVInputFormat_read_probe_func(AVInputFormat_read_probe func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int AVInputFormat_read_header (AVFormatContext* p0);
    public unsafe record struct AVInputFormat_read_header_func(IntPtr Pointer)
    {
        public static implicit operator AVInputFormat_read_header_func(AVInputFormat_read_header func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int AVInputFormat_read_packet (AVFormatContext* p0, AVPacket* pkt);
    public unsafe record struct AVInputFormat_read_packet_func(IntPtr Pointer)
    {
        public static implicit operator AVInputFormat_read_packet_func(AVInputFormat_read_packet func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int AVInputFormat_read_close (AVFormatContext* p0);
    public unsafe record struct AVInputFormat_read_close_func(IntPtr Pointer)
    {
        public static implicit operator AVInputFormat_read_close_func(AVInputFormat_read_close func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int AVInputFormat_read_seek (AVFormatContext* p0, int stream_index, long timestamp, int flags);
    public unsafe record struct AVInputFormat_read_seek_func(IntPtr Pointer)
    {
        public static implicit operator AVInputFormat_read_seek_func(AVInputFormat_read_seek func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate long AVInputFormat_read_timestamp (AVFormatContext* s, int stream_index, long* pos, long pos_limit);
    public unsafe record struct AVInputFormat_read_timestamp_func(IntPtr Pointer)
    {
        public static implicit operator AVInputFormat_read_timestamp_func(AVInputFormat_read_timestamp func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int AVInputFormat_read_play (AVFormatContext* p0);
    public unsafe record struct AVInputFormat_read_play_func(IntPtr Pointer)
    {
        public static implicit operator AVInputFormat_read_play_func(AVInputFormat_read_play func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int AVInputFormat_read_pause (AVFormatContext* p0);
    public unsafe record struct AVInputFormat_read_pause_func(IntPtr Pointer)
    {
        public static implicit operator AVInputFormat_read_pause_func(AVInputFormat_read_pause func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int AVInputFormat_read_seek2 (AVFormatContext* s, int stream_index, long min_ts, long ts, long max_ts, int flags);
    public unsafe record struct AVInputFormat_read_seek2_func(IntPtr Pointer)
    {
        public static implicit operator AVInputFormat_read_seek2_func(AVInputFormat_read_seek2 func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int AVInputFormat_get_device_list (AVFormatContext* s, AVDeviceInfoList* device_list);
    public unsafe record struct AVInputFormat_get_device_list_func(IntPtr Pointer)
    {
        public static implicit operator AVInputFormat_get_device_list_func(AVInputFormat_get_device_list func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int AVIOContext_read_packet (void* opaque, byte* buf, int buf_size);
    public unsafe record struct AVIOContext_read_packet_func(IntPtr Pointer)
    {
        public static implicit operator AVIOContext_read_packet_func(AVIOContext_read_packet func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int AVIOContext_write_packet (void* opaque, byte* buf, int buf_size);
    public unsafe record struct AVIOContext_write_packet_func(IntPtr Pointer)
    {
        public static implicit operator AVIOContext_write_packet_func(AVIOContext_write_packet func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate long AVIOContext_seek (void* opaque, long offset, int whence);
    public unsafe record struct AVIOContext_seek_func(IntPtr Pointer)
    {
        public static implicit operator AVIOContext_seek_func(AVIOContext_seek func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate ulong AVIOContext_update_checksum (ulong checksum, byte* buf, uint size);
    public unsafe record struct AVIOContext_update_checksum_func(IntPtr Pointer)
    {
        public static implicit operator AVIOContext_update_checksum_func(AVIOContext_update_checksum func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int AVIOContext_read_pause (void* opaque, int pause);
    public unsafe record struct AVIOContext_read_pause_func(IntPtr Pointer)
    {
        public static implicit operator AVIOContext_read_pause_func(AVIOContext_read_pause func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate long AVIOContext_read_seek (void* opaque, int stream_index, long timestamp, int flags);
    public unsafe record struct AVIOContext_read_seek_func(IntPtr Pointer)
    {
        public static implicit operator AVIOContext_read_seek_func(AVIOContext_read_seek func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int AVIOContext_write_data_type (void* opaque, byte* buf, int buf_size, AVIODataMarkerType type, long time);
    public unsafe record struct AVIOContext_write_data_type_func(IntPtr Pointer)
    {
        public static implicit operator AVIOContext_write_data_type_func(AVIOContext_write_data_type func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int AVIOInterruptCB_callback (void* p0);
    public unsafe record struct AVIOInterruptCB_callback_func(IntPtr Pointer)
    {
        public static implicit operator AVIOInterruptCB_callback_func(AVIOInterruptCB_callback func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int AVFormatContext_control_message_cb (AVFormatContext* s, int type, void* data, ulong data_size);
    public unsafe record struct AVFormatContext_control_message_cb_func(IntPtr Pointer)
    {
        public static implicit operator AVFormatContext_control_message_cb_func(AVFormatContext_control_message_cb func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int AVFormatContext_io_open (AVFormatContext* s, AVIOContext** pb, [MarshalAs(UnmanagedType.LPUTF8Str)] string url, int flags, AVDictionary** options);
    public unsafe record struct AVFormatContext_io_open_func(IntPtr Pointer)
    {
        public static implicit operator AVFormatContext_io_open_func(AVFormatContext_io_open func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void AVFormatContext_io_close (AVFormatContext* s, AVIOContext* pb);
    public unsafe record struct AVFormatContext_io_close_func(IntPtr Pointer)
    {
        public static implicit operator AVFormatContext_io_close_func(AVFormatContext_io_close func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int AVFormatContext_io_close2 (AVFormatContext* s, AVIOContext* pb);
    public unsafe record struct AVFormatContext_io_close2_func(IntPtr Pointer)
    {
        public static implicit operator AVFormatContext_io_close2_func(AVFormatContext_io_close2 func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int AVOutputFormat_write_header (AVFormatContext* p0);
    public unsafe record struct AVOutputFormat_write_header_func(IntPtr Pointer)
    {
        public static implicit operator AVOutputFormat_write_header_func(AVOutputFormat_write_header func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int AVOutputFormat_write_packet (AVFormatContext* p0, AVPacket* pkt);
    public unsafe record struct AVOutputFormat_write_packet_func(IntPtr Pointer)
    {
        public static implicit operator AVOutputFormat_write_packet_func(AVOutputFormat_write_packet func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int AVOutputFormat_write_trailer (AVFormatContext* p0);
    public unsafe record struct AVOutputFormat_write_trailer_func(IntPtr Pointer)
    {
        public static implicit operator AVOutputFormat_write_trailer_func(AVOutputFormat_write_trailer func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int AVOutputFormat_interleave_packet (AVFormatContext* s, AVPacket* pkt, int flush, int has_packet);
    public unsafe record struct AVOutputFormat_interleave_packet_func(IntPtr Pointer)
    {
        public static implicit operator AVOutputFormat_interleave_packet_func(AVOutputFormat_interleave_packet func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int AVOutputFormat_query_codec (AVCodecID id, int std_compliance);
    public unsafe record struct AVOutputFormat_query_codec_func(IntPtr Pointer)
    {
        public static implicit operator AVOutputFormat_query_codec_func(AVOutputFormat_query_codec func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void AVOutputFormat_get_output_timestamp (AVFormatContext* s, int stream, long* dts, long* wall);
    public unsafe record struct AVOutputFormat_get_output_timestamp_func(IntPtr Pointer)
    {
        public static implicit operator AVOutputFormat_get_output_timestamp_func(AVOutputFormat_get_output_timestamp func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int AVOutputFormat_control_message (AVFormatContext* s, int type, void* data, ulong data_size);
    public unsafe record struct AVOutputFormat_control_message_func(IntPtr Pointer)
    {
        public static implicit operator AVOutputFormat_control_message_func(AVOutputFormat_control_message func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int AVOutputFormat_write_uncoded_frame (AVFormatContext* p0, int stream_index, AVFrame** frame, uint flags);
    public unsafe record struct AVOutputFormat_write_uncoded_frame_func(IntPtr Pointer)
    {
        public static implicit operator AVOutputFormat_write_uncoded_frame_func(AVOutputFormat_write_uncoded_frame func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int AVOutputFormat_get_device_list (AVFormatContext* s, AVDeviceInfoList* device_list);
    public unsafe record struct AVOutputFormat_get_device_list_func(IntPtr Pointer)
    {
        public static implicit operator AVOutputFormat_get_device_list_func(AVOutputFormat_get_device_list func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int AVOutputFormat_init (AVFormatContext* p0);
    public unsafe record struct AVOutputFormat_init_func(IntPtr Pointer)
    {
        public static implicit operator AVOutputFormat_init_func(AVOutputFormat_init func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void AVOutputFormat_deinit (AVFormatContext* p0);
    public unsafe record struct AVOutputFormat_deinit_func(IntPtr Pointer)
    {
        public static implicit operator AVOutputFormat_deinit_func(AVOutputFormat_deinit func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int AVOutputFormat_check_bitstream (AVFormatContext* s, AVStream* st, AVPacket* pkt);
    public unsafe record struct AVOutputFormat_check_bitstream_func(IntPtr Pointer)
    {
        public static implicit operator AVOutputFormat_check_bitstream_func(AVOutputFormat_check_bitstream func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int avio_alloc_context_read_packet (void* opaque, byte* buf, int buf_size);
    public unsafe record struct avio_alloc_context_read_packet_func(IntPtr Pointer)
    {
        public static implicit operator avio_alloc_context_read_packet_func(avio_alloc_context_read_packet func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int avio_alloc_context_write_packet (void* opaque, byte* buf, int buf_size);
    public unsafe record struct avio_alloc_context_write_packet_func(IntPtr Pointer)
    {
        public static implicit operator avio_alloc_context_write_packet_func(avio_alloc_context_write_packet func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate long avio_alloc_context_seek (void* opaque, long offset, int whence);
    public unsafe record struct avio_alloc_context_seek_func(IntPtr Pointer)
    {
        public static implicit operator avio_alloc_context_seek_func(avio_alloc_context_seek func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int AVFilter_preinit (AVFilterContext* ctx);
    public unsafe record struct AVFilter_preinit_func(IntPtr Pointer)
    {
        public static implicit operator AVFilter_preinit_func(AVFilter_preinit func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int AVFilter_init (AVFilterContext* ctx);
    public unsafe record struct AVFilter_init_func(IntPtr Pointer)
    {
        public static implicit operator AVFilter_init_func(AVFilter_init func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int AVFilter_init_dict (AVFilterContext* ctx, AVDictionary** options);
    public unsafe record struct AVFilter_init_dict_func(IntPtr Pointer)
    {
        public static implicit operator AVFilter_init_dict_func(AVFilter_init_dict func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void AVFilter_uninit (AVFilterContext* ctx);
    public unsafe record struct AVFilter_uninit_func(IntPtr Pointer)
    {
        public static implicit operator AVFilter_uninit_func(AVFilter_uninit func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int _query_func (AVFilterContext* p0);
    public unsafe record struct _query_func_func(IntPtr Pointer)
    {
        public static implicit operator _query_func_func(_query_func func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int AVFilter_process_command (AVFilterContext* p0, [MarshalAs(UnmanagedType.LPUTF8Str)] string cmd, [MarshalAs(UnmanagedType.LPUTF8Str)] string arg, byte* res, int res_len, int flags);
    public unsafe record struct AVFilter_process_command_func(IntPtr Pointer)
    {
        public static implicit operator AVFilter_process_command_func(AVFilter_process_command func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int AVFilter_activate (AVFilterContext* ctx);
    public unsafe record struct AVFilter_activate_func(IntPtr Pointer)
    {
        public static implicit operator AVFilter_activate_func(AVFilter_activate func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int func (AVFilterContext* ctx, void* arg, int jobnr, int nb_jobs);
    public unsafe record struct func_func(IntPtr Pointer)
    {
        public static implicit operator func_func(func func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int AVFilterGraph_execute (AVFilterContext* ctx, func_func func, void* arg, int* ret, int nb_jobs);
    public unsafe record struct AVFilterGraph_execute_func(IntPtr Pointer)
    {
        public static implicit operator AVFilterGraph_execute_func(AVFilterGraph_execute func) => new(func switch
        {
            null => IntPtr.Zero,
            _ => Marshal.GetFunctionPointerForDelegate(func)
        });
    }
}
