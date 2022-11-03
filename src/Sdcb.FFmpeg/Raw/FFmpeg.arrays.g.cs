using System;
using System.Runtime.InteropServices;

#pragma warning disable 169
#pragma warning disable CS0649
#pragma warning disable CS0108
namespace Sdcb.FFmpeg.Raw
{
    
    public unsafe struct AVRational_array2
    {
        public const int Size = 2;
        public AVRational _0, _1;
        
        public AVRational this[int i]
        {
            get
            {
                if (i < 0 || i >= Size) throw new ArgumentOutOfRangeException($"i({i}) should in [0, {Size}]");
                fixed (AVRational* p0 = &_0)
                {
                    return *(p0 + i);
                }
            }
            set
            {
                if (i >= Size) throw new ArgumentOutOfRangeException($"i({i}) should < {Size}");
                fixed (AVRational* p0 = &_0)
                {
                    *(p0 + i) = value;
                }
            }
        }
        
        public AVRational[] ToArray() => new [] { _0, _1 };
        
        public void UpdateFrom(AVRational[] array)
        {
            if (array.Length != Size)
            {
                throw new ArgumentOutOfRangeException($"array size({array.Length}) should == {Size}");
            }
            
            fixed (AVRational* p = array)
            {
                _0 = p[0];
                _1 = p[1];
            }
        }
    }
    
    public unsafe struct short_array2
    {
        public const int Size = 2;
        public fixed short _[2];
        
        public short this[int i]
        {
            get => i switch
            {
                >= 0 and < Size => _[i],
                _ => throw new ArgumentOutOfRangeException($"i({i}) should in [0, {Size})"),
            };
            set => _[i] = i switch
            {
                >= 0 and < Size => value,
                _ => throw new ArgumentOutOfRangeException($"i({i}) should in [0, {Size})"),
            };
        }
        
        public short[] ToArray() => new [] { _[0], _[1] };
        
        
        public void UpdateFrom(short[] array)
        {
            if (array.Length != Size)
            {
                throw new ArgumentOutOfRangeException($"array size({array.Length}) should == {Size}");
            }
            
            fixed (short* p = array)
            {
                _[0] = p[0];
                _[1] = p[1];
            }
        }
    }
    
    public unsafe struct void_ptrArray2
    {
        public const int Size = 2;
        /// <summary>original type: void*</summary>
        public IntPtr _0, _1;
        
        /// <summary>original type: void*</summary>
        public IntPtr this[int i]
        {
            get
            {
                if (i < 0 || i >= Size) throw new ArgumentOutOfRangeException($"i({i}) should in [0, {Size}]");
                fixed (IntPtr* p0 = &_0)
                {
                    return *(p0 + i);
                }
            }
            set
            {
                if (i >= Size) throw new ArgumentOutOfRangeException($"i({i}) should < {Size}");
                fixed (IntPtr* p0 = &_0)
                {
                    *(p0 + i) = value;
                }
            }
        }
        
        public void*[] ToRawArray() => new [] { (void*)_0, (void*)_1 };
        
        /// <summary>original type: void*</summary>
        public IntPtr[] ToArray() => new [] { _0, _1 };
        
        /// <summary>original type: void*</summary>
        public void UpdateFrom(IntPtr[] array)
        {
            if (array.Length != Size)
            {
                throw new ArgumentOutOfRangeException($"array size({array.Length}) should == {Size}");
            }
            
            fixed (IntPtr* p = array)
            {
                _0 = p[0];
                _1 = p[1];
            }
        }
    }
    
    public unsafe struct AVHDRPlusColorTransformParams_array3
    {
        public const int Size = 3;
        public AVHDRPlusColorTransformParams _0, _1, _2;
        
        public AVHDRPlusColorTransformParams this[int i]
        {
            get
            {
                if (i < 0 || i >= Size) throw new ArgumentOutOfRangeException($"i({i}) should in [0, {Size}]");
                fixed (AVHDRPlusColorTransformParams* p0 = &_0)
                {
                    return *(p0 + i);
                }
            }
            set
            {
                if (i >= Size) throw new ArgumentOutOfRangeException($"i({i}) should < {Size}");
                fixed (AVHDRPlusColorTransformParams* p0 = &_0)
                {
                    *(p0 + i) = value;
                }
            }
        }
        
        public AVHDRPlusColorTransformParams[] ToArray() => new [] { _0, _1, _2 };
        
        public void UpdateFrom(AVHDRPlusColorTransformParams[] array)
        {
            if (array.Length != Size)
            {
                throw new ArgumentOutOfRangeException($"array size({array.Length}) should == {Size}");
            }
            
            fixed (AVHDRPlusColorTransformParams* p = array)
            {
                _0 = p[0];
                _1 = p[1];
                _2 = p[2];
            }
        }
    }
    
    public unsafe struct AVRational_array3
    {
        public const int Size = 3;
        public AVRational _0, _1, _2;
        
        public AVRational this[int i]
        {
            get
            {
                if (i < 0 || i >= Size) throw new ArgumentOutOfRangeException($"i({i}) should in [0, {Size}]");
                fixed (AVRational* p0 = &_0)
                {
                    return *(p0 + i);
                }
            }
            set
            {
                if (i >= Size) throw new ArgumentOutOfRangeException($"i({i}) should < {Size}");
                fixed (AVRational* p0 = &_0)
                {
                    *(p0 + i) = value;
                }
            }
        }
        
        public AVRational[] ToArray() => new [] { _0, _1, _2 };
        
        public void UpdateFrom(AVRational[] array)
        {
            if (array.Length != Size)
            {
                throw new ArgumentOutOfRangeException($"array size({array.Length}) should == {Size}");
            }
            
            fixed (AVRational* p = array)
            {
                _0 = p[0];
                _1 = p[1];
                _2 = p[2];
            }
        }
    }
    
    public unsafe struct AVRational_array3x2
    {
        public const int Size = 3;
        public AVRational_array2 _0, _1, _2;
        
        public AVRational_array2 this[int i]
        {
            get
            {
                if (i < 0 || i >= Size) throw new ArgumentOutOfRangeException($"i({i}) should in [0, {Size}]");
                fixed (AVRational_array2* p0 = &_0)
                {
                    return *(p0 + i);
                }
            }
            set
            {
                if (i >= Size) throw new ArgumentOutOfRangeException($"i({i}) should < {Size}");
                fixed (AVRational_array2* p0 = &_0)
                {
                    *(p0 + i) = value;
                }
            }
        }
        
        public AVRational_array2[] ToArray() => new [] { _0, _1, _2 };
        
        public void UpdateFrom(AVRational_array2[] array)
        {
            if (array.Length != Size)
            {
                throw new ArgumentOutOfRangeException($"array size({array.Length}) should == {Size}");
            }
            
            fixed (AVRational_array2* p = array)
            {
                _0 = p[0];
                _1 = p[1];
                _2 = p[2];
            }
        }
    }
    
    public unsafe struct byte_ptrArray3
    {
        public const int Size = 3;
        /// <summary>original type: byte*</summary>
        public IntPtr _0, _1, _2;
        
        /// <summary>original type: byte*</summary>
        public IntPtr this[int i]
        {
            get
            {
                if (i < 0 || i >= Size) throw new ArgumentOutOfRangeException($"i({i}) should in [0, {Size}]");
                fixed (IntPtr* p0 = &_0)
                {
                    return *(p0 + i);
                }
            }
            set
            {
                if (i >= Size) throw new ArgumentOutOfRangeException($"i({i}) should < {Size}");
                fixed (IntPtr* p0 = &_0)
                {
                    *(p0 + i) = value;
                }
            }
        }
        
        public byte*[] ToRawArray() => new [] { (byte*)_0, (byte*)_1, (byte*)_2 };
        
        /// <summary>original type: byte*</summary>
        public IntPtr[] ToArray() => new [] { _0, _1, _2 };
        
        /// <summary>original type: byte*</summary>
        public void UpdateFrom(IntPtr[] array)
        {
            if (array.Length != Size)
            {
                throw new ArgumentOutOfRangeException($"array size({array.Length}) should == {Size}");
            }
            
            fixed (IntPtr* p = array)
            {
                _0 = p[0];
                _1 = p[1];
                _2 = p[2];
            }
        }
    }
    
    public unsafe struct int_array3
    {
        public const int Size = 3;
        public fixed int _[3];
        
        public int this[int i]
        {
            get => i switch
            {
                >= 0 and < Size => _[i],
                _ => throw new ArgumentOutOfRangeException($"i({i}) should in [0, {Size})"),
            };
            set => _[i] = i switch
            {
                >= 0 and < Size => value,
                _ => throw new ArgumentOutOfRangeException($"i({i}) should in [0, {Size})"),
            };
        }
        
        public int[] ToArray() => new [] { _[0], _[1], _[2] };
        
        
        public void UpdateFrom(int[] array)
        {
            if (array.Length != Size)
            {
                throw new ArgumentOutOfRangeException($"array size({array.Length}) should == {Size}");
            }
            
            fixed (int* p = array)
            {
                _[0] = p[0];
                _[1] = p[1];
                _[2] = p[2];
            }
        }
    }
    
    public unsafe struct short_array3x2
    {
        public const int Size = 3;
        public short_array2 _0, _1, _2;
        
        public short_array2 this[int i]
        {
            get
            {
                if (i < 0 || i >= Size) throw new ArgumentOutOfRangeException($"i({i}) should in [0, {Size}]");
                fixed (short_array2* p0 = &_0)
                {
                    return *(p0 + i);
                }
            }
            set
            {
                if (i >= Size) throw new ArgumentOutOfRangeException($"i({i}) should < {Size}");
                fixed (short_array2* p0 = &_0)
                {
                    *(p0 + i) = value;
                }
            }
        }
        
        public short_array2[] ToArray() => new [] { _0, _1, _2 };
        
        public void UpdateFrom(short_array2[] array)
        {
            if (array.Length != Size)
            {
                throw new ArgumentOutOfRangeException($"array size({array.Length}) should == {Size}");
            }
            
            fixed (short_array2* p = array)
            {
                _0 = p[0];
                _1 = p[1];
                _2 = p[2];
            }
        }
    }
    
    public unsafe struct AVComponentDescriptor_array4
    {
        public const int Size = 4;
        public AVComponentDescriptor _0, _1, _2, _3;
        
        public AVComponentDescriptor this[int i]
        {
            get
            {
                if (i < 0 || i >= Size) throw new ArgumentOutOfRangeException($"i({i}) should in [0, {Size}]");
                fixed (AVComponentDescriptor* p0 = &_0)
                {
                    return *(p0 + i);
                }
            }
            set
            {
                if (i >= Size) throw new ArgumentOutOfRangeException($"i({i}) should < {Size}");
                fixed (AVComponentDescriptor* p0 = &_0)
                {
                    *(p0 + i) = value;
                }
            }
        }
        
        public AVComponentDescriptor[] ToArray() => new [] { _0, _1, _2, _3 };
        
        public void UpdateFrom(AVComponentDescriptor[] array)
        {
            if (array.Length != Size)
            {
                throw new ArgumentOutOfRangeException($"array size({array.Length}) should == {Size}");
            }
            
            fixed (AVComponentDescriptor* p = array)
            {
                _0 = p[0];
                _1 = p[1];
                _2 = p[2];
                _3 = p[3];
            }
        }
    }
    
    public unsafe struct byte_ptrArray4
    {
        public const int Size = 4;
        /// <summary>original type: byte*</summary>
        public IntPtr _0, _1, _2, _3;
        
        /// <summary>original type: byte*</summary>
        public IntPtr this[int i]
        {
            get
            {
                if (i < 0 || i >= Size) throw new ArgumentOutOfRangeException($"i({i}) should in [0, {Size}]");
                fixed (IntPtr* p0 = &_0)
                {
                    return *(p0 + i);
                }
            }
            set
            {
                if (i >= Size) throw new ArgumentOutOfRangeException($"i({i}) should < {Size}");
                fixed (IntPtr* p0 = &_0)
                {
                    *(p0 + i) = value;
                }
            }
        }
        
        public byte*[] ToRawArray() => new [] { (byte*)_0, (byte*)_1, (byte*)_2, (byte*)_3 };
        
        /// <summary>original type: byte*</summary>
        public IntPtr[] ToArray() => new [] { _0, _1, _2, _3 };
        
        /// <summary>original type: byte*</summary>
        public void UpdateFrom(IntPtr[] array)
        {
            if (array.Length != Size)
            {
                throw new ArgumentOutOfRangeException($"array size({array.Length}) should == {Size}");
            }
            
            fixed (IntPtr* p = array)
            {
                _0 = p[0];
                _1 = p[1];
                _2 = p[2];
                _3 = p[3];
            }
        }
    }
    
    public unsafe struct int_array4
    {
        public const int Size = 4;
        public fixed int _[4];
        
        public int this[int i]
        {
            get => i switch
            {
                >= 0 and < Size => _[i],
                _ => throw new ArgumentOutOfRangeException($"i({i}) should in [0, {Size})"),
            };
            set => _[i] = i switch
            {
                >= 0 and < Size => value,
                _ => throw new ArgumentOutOfRangeException($"i({i}) should in [0, {Size})"),
            };
        }
        
        public int[] ToArray() => new [] { _[0], _[1], _[2], _[3] };
        
        
        public void UpdateFrom(int[] array)
        {
            if (array.Length != Size)
            {
                throw new ArgumentOutOfRangeException($"array size({array.Length}) should == {Size}");
            }
            
            fixed (int* p = array)
            {
                _[0] = p[0];
                _[1] = p[1];
                _[2] = p[2];
                _[3] = p[3];
            }
        }
    }
    
    public unsafe struct long_array4
    {
        public const int Size = 4;
        public fixed long _[4];
        
        public long this[int i]
        {
            get => i switch
            {
                >= 0 and < Size => _[i],
                _ => throw new ArgumentOutOfRangeException($"i({i}) should in [0, {Size})"),
            };
            set => _[i] = i switch
            {
                >= 0 and < Size => value,
                _ => throw new ArgumentOutOfRangeException($"i({i}) should in [0, {Size})"),
            };
        }
        
        public long[] ToArray() => new [] { _[0], _[1], _[2], _[3] };
        
        
        public void UpdateFrom(long[] array)
        {
            if (array.Length != Size)
            {
                throw new ArgumentOutOfRangeException($"array size({array.Length}) should == {Size}");
            }
            
            fixed (long* p = array)
            {
                _[0] = p[0];
                _[1] = p[1];
                _[2] = p[2];
                _[3] = p[3];
            }
        }
    }
    
    public unsafe struct ulong_array4
    {
        public const int Size = 4;
        public fixed ulong _[4];
        
        public ulong this[int i]
        {
            get => i switch
            {
                >= 0 and < Size => _[i],
                _ => throw new ArgumentOutOfRangeException($"i({i}) should in [0, {Size})"),
            };
            set => _[i] = i switch
            {
                >= 0 and < Size => value,
                _ => throw new ArgumentOutOfRangeException($"i({i}) should in [0, {Size})"),
            };
        }
        
        public ulong[] ToArray() => new [] { _[0], _[1], _[2], _[3] };
        
        
        public void UpdateFrom(ulong[] array)
        {
            if (array.Length != Size)
            {
                throw new ArgumentOutOfRangeException($"array size({array.Length}) should == {Size}");
            }
            
            fixed (ulong* p = array)
            {
                _[0] = p[0];
                _[1] = p[1];
                _[2] = p[2];
                _[3] = p[3];
            }
        }
    }
    
    public unsafe struct int_array7
    {
        public const int Size = 7;
        public fixed int _[7];
        
        public int this[int i]
        {
            get => i switch
            {
                >= 0 and < Size => _[i],
                _ => throw new ArgumentOutOfRangeException($"i({i}) should in [0, {Size})"),
            };
            set => _[i] = i switch
            {
                >= 0 and < Size => value,
                _ => throw new ArgumentOutOfRangeException($"i({i}) should in [0, {Size})"),
            };
        }
        
        public int[] ToArray() => new [] { _[0], _[1], _[2], _[3], _[4], _[5], _[6] };
        
        
        public void UpdateFrom(int[] array)
        {
            if (array.Length != Size)
            {
                throw new ArgumentOutOfRangeException($"array size({array.Length}) should == {Size}");
            }
            
            fixed (int* p = array)
            {
                _[0] = p[0];
                _[1] = p[1];
                _[2] = p[2];
                _[3] = p[3];
                _[4] = p[4];
                _[5] = p[5];
                _[6] = p[6];
            }
        }
    }
    
    public unsafe struct AVBufferRef_ptrArray8
    {
        public const int Size = 8;
        /// <summary>original type: AVBufferRef*</summary>
        public IntPtr _0, _1, _2, _3, _4, _5, _6, _7;
        
        /// <summary>original type: AVBufferRef*</summary>
        public IntPtr this[int i]
        {
            get
            {
                if (i < 0 || i >= Size) throw new ArgumentOutOfRangeException($"i({i}) should in [0, {Size}]");
                fixed (IntPtr* p0 = &_0)
                {
                    return *(p0 + i);
                }
            }
            set
            {
                if (i >= Size) throw new ArgumentOutOfRangeException($"i({i}) should < {Size}");
                fixed (IntPtr* p0 = &_0)
                {
                    *(p0 + i) = value;
                }
            }
        }
        
        public AVBufferRef*[] ToRawArray() => new [] { (AVBufferRef*)_0, (AVBufferRef*)_1, (AVBufferRef*)_2, (AVBufferRef*)_3, (AVBufferRef*)_4, (AVBufferRef*)_5, (AVBufferRef*)_6, (AVBufferRef*)_7 };
        
        /// <summary>original type: AVBufferRef*</summary>
        public IntPtr[] ToArray() => new [] { _0, _1, _2, _3, _4, _5, _6, _7 };
        
        /// <summary>original type: AVBufferRef*</summary>
        public void UpdateFrom(IntPtr[] array)
        {
            if (array.Length != Size)
            {
                throw new ArgumentOutOfRangeException($"array size({array.Length}) should == {Size}");
            }
            
            fixed (IntPtr* p = array)
            {
                _0 = p[0];
                _1 = p[1];
                _2 = p[2];
                _3 = p[3];
                _4 = p[4];
                _5 = p[5];
                _6 = p[6];
                _7 = p[7];
            }
        }
    }
    
    public unsafe struct byte_array8
    {
        public const int Size = 8;
        public fixed byte _[8];
        
        public byte this[int i]
        {
            get => i switch
            {
                >= 0 and < Size => _[i],
                _ => throw new ArgumentOutOfRangeException($"i({i}) should in [0, {Size})"),
            };
            set => _[i] = i switch
            {
                >= 0 and < Size => value,
                _ => throw new ArgumentOutOfRangeException($"i({i}) should in [0, {Size})"),
            };
        }
        
        public byte[] ToArray() => new [] { _[0], _[1], _[2], _[3], _[4], _[5], _[6], _[7] };
        
        
        public void UpdateFrom(byte[] array)
        {
            if (array.Length != Size)
            {
                throw new ArgumentOutOfRangeException($"array size({array.Length}) should == {Size}");
            }
            
            fixed (byte* p = array)
            {
                _[0] = p[0];
                _[1] = p[1];
                _[2] = p[2];
                _[3] = p[3];
                _[4] = p[4];
                _[5] = p[5];
                _[6] = p[6];
                _[7] = p[7];
            }
        }
    }
    
    public unsafe struct byte_ptrArray8
    {
        public const int Size = 8;
        /// <summary>original type: byte*</summary>
        public IntPtr _0, _1, _2, _3, _4, _5, _6, _7;
        
        /// <summary>original type: byte*</summary>
        public IntPtr this[int i]
        {
            get
            {
                if (i < 0 || i >= Size) throw new ArgumentOutOfRangeException($"i({i}) should in [0, {Size}]");
                fixed (IntPtr* p0 = &_0)
                {
                    return *(p0 + i);
                }
            }
            set
            {
                if (i >= Size) throw new ArgumentOutOfRangeException($"i({i}) should < {Size}");
                fixed (IntPtr* p0 = &_0)
                {
                    *(p0 + i) = value;
                }
            }
        }
        
        public byte*[] ToRawArray() => new [] { (byte*)_0, (byte*)_1, (byte*)_2, (byte*)_3, (byte*)_4, (byte*)_5, (byte*)_6, (byte*)_7 };
        
        public static explicit operator byte_ptrArray4(byte_ptrArray8 me)
        {
            byte_ptrArray4 r = new ();
            r._0 = me._0;
            r._1 = me._1;
            r._2 = me._2;
            r._3 = me._3;
            return r;
        }
        
        /// <summary>original type: byte*</summary>
        public IntPtr[] ToArray() => new [] { _0, _1, _2, _3, _4, _5, _6, _7 };
        
        /// <summary>original type: byte*</summary>
        public void UpdateFrom(IntPtr[] array)
        {
            if (array.Length != Size)
            {
                throw new ArgumentOutOfRangeException($"array size({array.Length}) should == {Size}");
            }
            
            fixed (IntPtr* p = array)
            {
                _0 = p[0];
                _1 = p[1];
                _2 = p[2];
                _3 = p[3];
                _4 = p[4];
                _5 = p[5];
                _6 = p[6];
                _7 = p[7];
            }
        }
    }
    
    public unsafe struct int_array8
    {
        public const int Size = 8;
        public fixed int _[8];
        
        public int this[int i]
        {
            get => i switch
            {
                >= 0 and < Size => _[i],
                _ => throw new ArgumentOutOfRangeException($"i({i}) should in [0, {Size})"),
            };
            set => _[i] = i switch
            {
                >= 0 and < Size => value,
                _ => throw new ArgumentOutOfRangeException($"i({i}) should in [0, {Size})"),
            };
        }
        
        public int[] ToArray4() => new [] { _[0], _[1], _[2], _[3] };
        
        public static unsafe explicit operator int_array4(int_array8 me)
        {
            int_array4 r = new ();
            r._[0] = me._[0];
            r._[1] = me._[1];
            r._[2] = me._[2];
            r._[3] = me._[3];
            return r;
        }
        public int[] ToArray() => new [] { _[0], _[1], _[2], _[3], _[4], _[5], _[6], _[7] };
        
        
        public void UpdateFrom(int[] array)
        {
            if (array.Length != Size)
            {
                throw new ArgumentOutOfRangeException($"array size({array.Length}) should == {Size}");
            }
            
            fixed (int* p = array)
            {
                _[0] = p[0];
                _[1] = p[1];
                _[2] = p[2];
                _[3] = p[3];
                _[4] = p[4];
                _[5] = p[5];
                _[6] = p[6];
                _[7] = p[7];
            }
        }
    }
    
    public unsafe struct ulong_array8
    {
        public const int Size = 8;
        public fixed ulong _[8];
        
        public ulong this[int i]
        {
            get => i switch
            {
                >= 0 and < Size => _[i],
                _ => throw new ArgumentOutOfRangeException($"i({i}) should in [0, {Size})"),
            };
            set => _[i] = i switch
            {
                >= 0 and < Size => value,
                _ => throw new ArgumentOutOfRangeException($"i({i}) should in [0, {Size})"),
            };
        }
        
        public ulong[] ToArray4() => new [] { _[0], _[1], _[2], _[3] };
        
        public static unsafe explicit operator ulong_array4(ulong_array8 me)
        {
            ulong_array4 r = new ();
            r._[0] = me._[0];
            r._[1] = me._[1];
            r._[2] = me._[2];
            r._[3] = me._[3];
            return r;
        }
        public ulong[] ToArray() => new [] { _[0], _[1], _[2], _[3], _[4], _[5], _[6], _[7] };
        
        
        public void UpdateFrom(ulong[] array)
        {
            if (array.Length != Size)
            {
                throw new ArgumentOutOfRangeException($"array size({array.Length}) should == {Size}");
            }
            
            fixed (ulong* p = array)
            {
                _[0] = p[0];
                _[1] = p[1];
                _[2] = p[2];
                _[3] = p[3];
                _[4] = p[4];
                _[5] = p[5];
                _[6] = p[6];
                _[7] = p[7];
            }
        }
    }
    
    public unsafe struct AVHDRPlusPercentile_array15
    {
        public const int Size = 15;
        public AVHDRPlusPercentile _0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14;
        
        public AVHDRPlusPercentile this[int i]
        {
            get
            {
                if (i < 0 || i >= Size) throw new ArgumentOutOfRangeException($"i({i}) should in [0, {Size}]");
                fixed (AVHDRPlusPercentile* p0 = &_0)
                {
                    return *(p0 + i);
                }
            }
            set
            {
                if (i >= Size) throw new ArgumentOutOfRangeException($"i({i}) should < {Size}");
                fixed (AVHDRPlusPercentile* p0 = &_0)
                {
                    *(p0 + i) = value;
                }
            }
        }
        
        public AVHDRPlusPercentile[] ToArray() => new [] { _0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14 };
        
        public void UpdateFrom(AVHDRPlusPercentile[] array)
        {
            if (array.Length != Size)
            {
                throw new ArgumentOutOfRangeException($"array size({array.Length}) should == {Size}");
            }
            
            fixed (AVHDRPlusPercentile* p = array)
            {
                _0 = p[0];
                _1 = p[1];
                _2 = p[2];
                _3 = p[3];
                _4 = p[4];
                _5 = p[5];
                _6 = p[6];
                _7 = p[7];
                _8 = p[8];
                _9 = p[9];
                _10 = p[10];
                _11 = p[11];
                _12 = p[12];
                _13 = p[13];
                _14 = p[14];
            }
        }
    }
    
    public unsafe struct AVRational_array15
    {
        public const int Size = 15;
        public AVRational _0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14;
        
        public AVRational this[int i]
        {
            get
            {
                if (i < 0 || i >= Size) throw new ArgumentOutOfRangeException($"i({i}) should in [0, {Size}]");
                fixed (AVRational* p0 = &_0)
                {
                    return *(p0 + i);
                }
            }
            set
            {
                if (i >= Size) throw new ArgumentOutOfRangeException($"i({i}) should < {Size}");
                fixed (AVRational* p0 = &_0)
                {
                    *(p0 + i) = value;
                }
            }
        }
        
        public AVRational[] ToArray() => new [] { _0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14 };
        
        public void UpdateFrom(AVRational[] array)
        {
            if (array.Length != Size)
            {
                throw new ArgumentOutOfRangeException($"array size({array.Length}) should == {Size}");
            }
            
            fixed (AVRational* p = array)
            {
                _0 = p[0];
                _1 = p[1];
                _2 = p[2];
                _3 = p[3];
                _4 = p[4];
                _5 = p[5];
                _6 = p[6];
                _7 = p[7];
                _8 = p[8];
                _9 = p[9];
                _10 = p[10];
                _11 = p[11];
                _12 = p[12];
                _13 = p[13];
                _14 = p[14];
            }
        }
    }
    
    public unsafe struct byte_array16
    {
        public const int Size = 16;
        public fixed byte _[16];
        
        public byte this[int i]
        {
            get => i switch
            {
                >= 0 and < Size => _[i],
                _ => throw new ArgumentOutOfRangeException($"i({i}) should in [0, {Size})"),
            };
            set => _[i] = i switch
            {
                >= 0 and < Size => value,
                _ => throw new ArgumentOutOfRangeException($"i({i}) should in [0, {Size})"),
            };
        }
        
        public byte[] ToArray() => new [] { _[0], _[1], _[2], _[3], _[4], _[5], _[6], _[7], _[8], _[9], _[10], _[11], _[12], _[13], _[14], _[15] };
        
        
        public void UpdateFrom(byte[] array)
        {
            if (array.Length != Size)
            {
                throw new ArgumentOutOfRangeException($"array size({array.Length}) should == {Size}");
            }
            
            fixed (byte* p = array)
            {
                _[0] = p[0];
                _[1] = p[1];
                _[2] = p[2];
                _[3] = p[3];
                _[4] = p[4];
                _[5] = p[5];
                _[6] = p[6];
                _[7] = p[7];
                _[8] = p[8];
                _[9] = p[9];
                _[10] = p[10];
                _[11] = p[11];
                _[12] = p[12];
                _[13] = p[13];
                _[14] = p[14];
                _[15] = p[15];
            }
        }
    }
    
    public unsafe struct AVRational_array25
    {
        public const int Size = 25;
        public AVRational _0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, _19, _20, _21, _22, _23, _24;
        
        public AVRational this[int i]
        {
            get
            {
                if (i < 0 || i >= Size) throw new ArgumentOutOfRangeException($"i({i}) should in [0, {Size}]");
                fixed (AVRational* p0 = &_0)
                {
                    return *(p0 + i);
                }
            }
            set
            {
                if (i >= Size) throw new ArgumentOutOfRangeException($"i({i}) should < {Size}");
                fixed (AVRational* p0 = &_0)
                {
                    *(p0 + i) = value;
                }
            }
        }
        
        public AVRational[] ToArray() => new [] { _0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, _19, _20, _21, _22, _23, _24 };
        
        public void UpdateFrom(AVRational[] array)
        {
            if (array.Length != Size)
            {
                throw new ArgumentOutOfRangeException($"array size({array.Length}) should == {Size}");
            }
            
            fixed (AVRational* p = array)
            {
                _0 = p[0];
                _1 = p[1];
                _2 = p[2];
                _3 = p[3];
                _4 = p[4];
                _5 = p[5];
                _6 = p[6];
                _7 = p[7];
                _8 = p[8];
                _9 = p[9];
                _10 = p[10];
                _11 = p[11];
                _12 = p[12];
                _13 = p[13];
                _14 = p[14];
                _15 = p[15];
                _16 = p[16];
                _17 = p[17];
                _18 = p[18];
                _19 = p[19];
                _20 = p[20];
                _21 = p[21];
                _22 = p[22];
                _23 = p[23];
                _24 = p[24];
            }
        }
    }
    
    public unsafe struct AVRational_array25x25
    {
        public const int Size = 25;
        public AVRational_array25 _0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, _19, _20, _21, _22, _23, _24;
        
        public AVRational_array25 this[int i]
        {
            get
            {
                if (i < 0 || i >= Size) throw new ArgumentOutOfRangeException($"i({i}) should in [0, {Size}]");
                fixed (AVRational_array25* p0 = &_0)
                {
                    return *(p0 + i);
                }
            }
            set
            {
                if (i >= Size) throw new ArgumentOutOfRangeException($"i({i}) should < {Size}");
                fixed (AVRational_array25* p0 = &_0)
                {
                    *(p0 + i) = value;
                }
            }
        }
        
        public AVRational_array25[] ToArray() => new [] { _0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, _19, _20, _21, _22, _23, _24 };
        
        public void UpdateFrom(AVRational_array25[] array)
        {
            if (array.Length != Size)
            {
                throw new ArgumentOutOfRangeException($"array size({array.Length}) should == {Size}");
            }
            
            fixed (AVRational_array25* p = array)
            {
                _0 = p[0];
                _1 = p[1];
                _2 = p[2];
                _3 = p[3];
                _4 = p[4];
                _5 = p[5];
                _6 = p[6];
                _7 = p[7];
                _8 = p[8];
                _9 = p[9];
                _10 = p[10];
                _11 = p[11];
                _12 = p[12];
                _13 = p[13];
                _14 = p[14];
                _15 = p[15];
                _16 = p[16];
                _17 = p[17];
                _18 = p[18];
                _19 = p[19];
                _20 = p[20];
                _21 = p[21];
                _22 = p[22];
                _23 = p[23];
                _24 = p[24];
            }
        }
    }
    
    public unsafe struct byte_array61440
    {
        public const int Size = 61440;
        public fixed byte _[61440];
        
        public byte this[int i]
        {
            get => i switch
            {
                >= 0 and < Size => _[i],
                _ => throw new ArgumentOutOfRangeException($"i({i}) should in [0, {Size})"),
            };
            set => _[i] = i switch
            {
                >= 0 and < Size => value,
                _ => throw new ArgumentOutOfRangeException($"i({i}) should in [0, {Size})"),
            };
        }
        
        public byte[] ToArray()
        {
            fixed (byte_array61440* p = &this)
            {
                var a = new byte[Size];
                for (uint i = 0; i < Size; i++)
                {
                    a[i] = p->_[i];
                }
                return a;
            }
        }
        
        public void UpdateFrom(byte[] array)
        {
            if (array.Length != Size)
            {
                throw new ArgumentOutOfRangeException($"array size({array.Length}) should == {Size}");
            }
            
            fixed (byte* p = array)
            {
                for (int i = 0; i < Size; ++i)
                {
                    _[i] = p[i];
                }
            }
        }
    }
    
}
