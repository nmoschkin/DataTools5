using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Security.Policy;
using DataTools.Streams;

[assembly: InternalsVisibleTo("DataTools.Memory")]
[assembly: InternalsVisibleTo("DataTools.Win32")]
[assembly: InternalsVisibleTo("DataTools.Hardware")]
namespace DataTools.Win32.Memory
{

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct MemPtr
    {
        internal IntPtr handle;

        public static readonly MemPtr Empty = new MemPtr((IntPtr)0);

        private static IntPtr procHeap = Native.GetProcessHeap();

        public long Size
        {
            get
            {
                if (handle == IntPtr.Zero) return 0;

                try
                {
                    return (long)Native.HeapSize(procHeap, 0, handle);
                }
                catch
                {
                    return 0;
                }
            }
        }

        public long Length
        {
            get => Size;
            set
            {
                if (Size == value) return;

                if (value == 0)
                {
                    Free();
                    return;
                }
                else
                {
                    ReAlloc(value);
                }
            }
        }

        public IntPtr Handle
        {
            get
            {
                unsafe
                {
                    return handle;
                }
            }
            set
            {
                unsafe
                {
                    handle = value;
                }
            }
        }

        public MemPtr(long size = 1024)
        {
            if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size));
            handle = (IntPtr)0;
            Alloc(size);
        }

        public MemPtr(IntPtr ptr)
        {
            handle = ptr;
        }

        public unsafe MemPtr(void* ptr)
        {
            handle = (IntPtr)ptr;
        }

        public uint CalculateCrc32()
        {
            long c = Size;
            if (handle == IntPtr.Zero || c <= 0) return 0;

            unsafe
            {
                return Crc32.Calculate((byte*)handle, c);
            }
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref byte ByteAt(long index)
        {
            unsafe
            {
                return ref *(byte*)((long)handle + index);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref sbyte SByteAt(long index)
        {
            unsafe
            {
                return ref *(sbyte*)((long)handle + index);
            }
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref char CharAt(long index)
        {
            unsafe
            {
                return ref *(char*)((long)handle + index * sizeof(char));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref char CharAtAbsolute(long index)
        {
            unsafe
            {
                return ref *(char*)((long)handle + index);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref short ShortAt(long index)
        {
            unsafe
            {
                return ref *(short*)((long)handle + index * sizeof(short));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref short ShortAtAbsolute(long index)
        {
            unsafe
            {
                return ref *(short*)((long)handle + index);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref ushort UShortAt(long index)
        {
            unsafe
            {
                return ref *(ushort*)((long)handle + index * sizeof(ushort));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref ushort UShortAtAbsolute(long index)
        {
            unsafe
            {
                return ref *(ushort*)((long)handle + index);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref int IntAt(long index)
        {
            unsafe
            {
                return ref *(int*)((long)handle + index * sizeof(int));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref int IntAtAbsolute(long index)
        {
            unsafe
            {
                return ref *(int*)((long)handle + index);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref uint UIntAt(long index)
        {
            unsafe
            {
                return ref *(uint*)((long)handle + index * sizeof(uint));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref uint UIntAtAbsolute(long index)
        {
            unsafe
            {
                return ref *(uint*)((long)handle + index);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref long LongAt(long index)
        {
            unsafe
            {
                return ref *(long*)((long)handle + index * sizeof(long));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref long LongAtAbsolute(long index)
        {
            unsafe
            {
                return ref *(long*)((long)handle + index);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref ulong ULongAt(long index)
        {
            unsafe
            {
                return ref *(ulong*)((long)handle + index * sizeof(ulong));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref ulong ULongAtAbsolute(long index)
        {
            unsafe
            {
                return ref *(ulong*)((long)handle + index);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref float FloatAt(long index)
        {
            unsafe
            {
                return ref *(float*)((long)handle + index * sizeof(float));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref float FloatAtAbsolute(long index)
        {
            unsafe
            {
                return ref *(float*)((long)handle + index);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref float SingleAt(long index) => ref FloatAt(index);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref float SingleAtAbsolute(long index) => ref FloatAtAbsolute(index);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref double DoubleAt(long index)
        {
            unsafe
            {
                return ref *(double*)((long)handle + index * sizeof(double));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref double DoubleAtAbsolute(long index)
        {
            unsafe
            {
                return ref *(double*)((long)handle + index);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref decimal DecimalAt(long index)
        {
            unsafe
            {
                return ref *(decimal*)((long)handle + index * sizeof(decimal));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref decimal DecimalAtAbsolute(long index)
        {
            unsafe
            {
                return ref *(decimal*)((long)handle + index);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref Guid GuidAt(long index)
        {
            unsafe
            {
                return ref *(Guid*)((long)handle + index * sizeof(Guid));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref Guid GuidAtAbsolute(long index)
        {
            unsafe
            {
                return ref *(Guid*)((long)handle + index);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref DateTime DateTimeAt(long index)
        {
            unsafe
            {
                return ref *(DateTime*)((long)handle + index * sizeof(DateTime));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref DateTime DateTimeAtAbsolute(long index)
        {
            unsafe
            {
                return ref *(DateTime*)((long)handle + index);
            }
        }

        /// <summary>
        /// Copies one structure into another.  
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="val1"></param>
        /// <param name="val2"></param>
        /// <param name="size"></param>
        public static void Union<T, U>(ref T val1, ref U val2)
            where T : struct
            where U : struct
        {
            GCHandle gc1, gc2;

            gc1 = GCHandle.Alloc(val1, GCHandleType.Pinned);
            gc2 = GCHandle.Alloc(val2, GCHandleType.Pinned);

            int x = Marshal.SizeOf<T>();
            int y = Marshal.SizeOf<U>();

            unsafe
            {
                void* h1 = (void*)gc1.AddrOfPinnedObject();
                void* h2 = (void*)gc2.AddrOfPinnedObject();

                Buffer.MemoryCopy(h1, h2, y, x);
            }

            gc1.Free();
            gc2.Free();
        }

        /// <summary>
        /// Copies a stucture into an array of structures.  
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="val1"></param>
        /// <param name="val2"></param>
        /// <param name="size"></param>
        public static void Union<T, U>(ref T val1, ref U[] val2)
            where T : struct
            where U : struct
        {
            GCHandle gc1, gc2;

            gc1 = GCHandle.Alloc(val1, GCHandleType.Pinned);
            gc2 = GCHandle.Alloc(val2, GCHandleType.Pinned);

            int x = Marshal.SizeOf<T>();
            int y = Marshal.SizeOf<U>() * val2.Length;

            unsafe
            {
                void* h1 = (void*)gc1.AddrOfPinnedObject();
                void* h2 = (void*)gc2.AddrOfPinnedObject();

                Buffer.MemoryCopy(h1, h2, y, x);
            }

            gc1.Free();
            gc2.Free();
        }

        /// <summary>
        /// Copies an array of structures into a structure.  
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="val1"></param>
        /// <param name="val2"></param>
        /// <param name="size"></param>
        public static void Union<T, U>(ref T[] val1, out U val2)
            where T : struct
            where U : struct
        {
            GCHandle gc1;
            //U outstr = new U();

            gc1 = GCHandle.Alloc(val1, GCHandleType.Pinned);
            //gc2 = GCHandle.Alloc(outstr, GCHandleType.Pinned);

            int x = Marshal.SizeOf<T>() * val1.Length;
            int y = Marshal.SizeOf<U>();

            unsafe
            {
                IntPtr h1 = gc1.AddrOfPinnedObject();
                //void* h2 = (void*)gc2.AddrOfPinnedObject();

                val2 = Marshal.PtrToStructure<U>(h1);

                //Buffer.MemoryCopy(h1, h2, y, x);
            }

            gc1.Free();
            //gc2.Free();

            //val2 = outstr;
        }

        /// <summary>
        /// Converts the contents of an unmanaged pointer into a structure.
        /// </summary>
        /// <typeparam name="T">The type of structure requested.</typeparam>
        /// <returns>New instance of T.</returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T ToStruct<T>() where T : struct
        {
            if (handle == IntPtr.Zero) return default;
            return (T)Marshal.PtrToStructure(handle, typeof(T));
        }

        /// <summary>
        /// Sets the contents of a structure into an unmanaged pointer.
        /// </summary>
        /// <typeparam name="T">The type of structure to set.</typeparam>
        /// <param name="val">The structure to set.</param>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FromStruct<T>(T val) where T : struct
        {
            if (handle == IntPtr.Zero)
            {
                Alloc(Marshal.SizeOf(val));
            }

            Marshal.StructureToPtr(val, handle, false);
        }

        /// <summary>
        /// Converts the contents of an unmanaged pointer at the specified byte index into a structure.
        /// </summary>
        /// <typeparam name="T">The type of structure requested.</typeparam>
        /// <param name="byteIndex">The byte index relative to the pointer at which to begin copying.</param>
        /// <returns>New instance of T.</returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T ToStructAt<T>(long byteIndex) where T : struct
        {
            return (T)Marshal.PtrToStructure((IntPtr)((long)handle + byteIndex), typeof(T));
        }

        /// <summary>
        /// Sets the contents of a structure into a memory buffer at the specified byte index.
        /// </summary>
        /// <typeparam name="T">The type of structure to set.</typeparam>
        /// <param name="byteIndex">The byte index relative to the pointer at which to begin copying.</param>
        /// <param name="val">The structure to set.</param>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FromStructAt<T>(long byteIndex, T val) where T : struct
        {
            if (handle == IntPtr.Zero)
            {
                Alloc(Marshal.SizeOf(val) + byteIndex);
            }

            Marshal.StructureToPtr(val, (IntPtr)((long)handle + byteIndex), false);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte[] ToByteArray(long index = 0, long length = 0)
        {
            if (handle == IntPtr.Zero || length < 0) return null;

            long len = length;
            //long size = Size;

            //if (len == 0) len = (size - index);
            //if (size - index < length) len = size - index;

            //if (len > int.MaxValue) len = int.MaxValue;

            byte[] output = new byte[len];

            GCHandle gch = GCHandle.Alloc(output, GCHandleType.Pinned);

            unsafe
            {
                void* ptr1 = (void*)((long)handle + index);
                void* ptr2 = (void*)gch.AddrOfPinnedObject();

                Buffer.MemoryCopy(ptr1, ptr2, len, len);
            }

            gch.Free();
            return output;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public char[] ToCharArray(long index = 0, int length = 0)
        {
            if (handle == IntPtr.Zero || length < 0) return null;

            long len = length * sizeof(char);
            long size;

            if (length == 0)
            {
                size = Size;
                len = size - index;
                if (size - index < length) len = size - index;
            }

            if (len > int.MaxValue) len = int.MaxValue;

            if (len % sizeof(char) != 0)
            {
                len -= len % sizeof(char);
            }

            char[] output = new char[len / sizeof(char)];

            GCHandle gch = GCHandle.Alloc(output, GCHandleType.Pinned);

            unsafe
            {
                void* ptr1 = (void*)((long)handle + index);
                void* ptr2 = (void*)gch.AddrOfPinnedObject();

                Buffer.MemoryCopy(ptr1, ptr2, len, len);
            }

            gch.Free();
            return output;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] ToArray<T>(long index = 0, int length = 0) where T : struct
        {
            if (handle == IntPtr.Zero || length < 0) return null;

            unsafe
            {
                int tlen = typeof(T) == typeof(char) ? sizeof(char) : Marshal.SizeOf<T>();

                long len = length * tlen;
                long size = Size;

                if (len == 0) len = size - index;
                if (size - index < length) len = size - index;

                if (len > int.MaxValue) len = int.MaxValue;

                if (len % tlen != 0)
                {
                    len -= len % tlen;
                }

                T[] output = new T[len / tlen];

                GCHandle gch = GCHandle.Alloc(output, GCHandleType.Pinned);

                unsafe
                {
                    void* ptr1 = (void*)((long)handle + index);
                    void* ptr2 = (void*)gch.AddrOfPinnedObject();

                    Buffer.MemoryCopy(ptr1, ptr2, len, len);
                }

                gch.Free();
                return output;
            }
        }


        public void FromByteArray(byte[] value, long index = 0)
        {
            if (handle != IntPtr.Zero)
            {
                ReAlloc(value.Length + index);
            }
            else
            {
                Alloc(value.Length + index);
            }
            unsafe
            {
                var vl = value.Length;
                GCHandle gch = GCHandle.Alloc(value, GCHandleType.Pinned);
                Buffer.MemoryCopy((void*)gch.AddrOfPinnedObject(), (void*)((long)handle + index), vl, vl);
                gch.Free();
            }
        }

        public void FromCharArray(char[] value, long index = 0)
        {
            if (handle != IntPtr.Zero)
            {
                ReAlloc(value.Length * sizeof(char) + index);
            }
            else
            {
                Alloc(value.Length * sizeof(char) + index);
            }
            unsafe
            {
                var vl = value.Length * 2;
                GCHandle gch = GCHandle.Alloc(value, GCHandleType.Pinned);
                Buffer.MemoryCopy((void*)gch.AddrOfPinnedObject(), (void*)((long)handle + index), vl, vl);
                gch.Free();
            }
        }

        public void FromArray<T>(T[] value, long index = 0) where T : struct
        {
            var cb = Marshal.SizeOf<T>();

            if (handle != IntPtr.Zero)
            {
                ReAlloc(value.Length * cb + index);
            }
            else
            {
                Alloc(value.Length * cb + index);
            }
            unsafe
            {
                var vl = value.Length * cb;
                GCHandle gch = GCHandle.Alloc(value, GCHandleType.Pinned);
                Buffer.MemoryCopy((void*)gch.AddrOfPinnedObject(), (void*)((long)handle + index), vl, vl);
                gch.Free();
            }
        }

        public MemPtr Clone()
        {
            if (handle == IntPtr.Zero) return new MemPtr(IntPtr.Zero);

            var cb = Size;
            var mm = new MemPtr(cb);
            Native.MemCpy(handle, mm.handle, cb);

            return mm;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetString(long index)
        {
            unsafe
            {
                return new string((char*)((long)handle + index));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetString(long index, int length)
        {
            unsafe
            {
                return new string((char*)((long)handle + index), 0, length);
            }
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetString(long index, string value, bool addNull = true)
        {
            unsafe
            {
                internalSetString((char*)((long)handle + index), value, addNull);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetStringIndirect(long index, string value, bool addNull = true)
        {
            unsafe
            {
                char* ptr = (char*)*(IntPtr*)((long)handle + index);
                internalSetString(ptr, value, addNull);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetStringIndirect(long index)
        {
            unsafe
            {
                char* ptr = (char*)*(IntPtr*)((long)handle + index);
                return new string(ptr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetUTF8String(long index)
        {
            unsafe
            {
                return internalGetUTF8String((byte*)((long)handle + index));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetUTF8StringIndirect(long index)
        {
            unsafe
            {
                return internalGetUTF8String((byte*)*(IntPtr*)((long)handle + index));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe string internalGetUTF8String(byte* ptr)
        {
            byte* b2 = ptr;

            while (*b2 != 0) b2++;
            if (ptr == b2) return "";

            return Encoding.UTF8.GetString(ptr, (int)(b2 - ptr));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetUTF8String(long index, string value, bool addNull = true)
        {
            unsafe
            {
                internalSetUTF8String((byte*)((long)handle + index), value, addNull);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetUTF8StringIndirect(long index, string value, bool addNull = true)
        {
            unsafe
            {
                internalSetUTF8String((byte*)*(IntPtr*)((long)handle + index), value, addNull);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe void internalSetUTF8String(byte* ptr, string value, bool addNull)
        {
            byte[] data = Encoding.UTF8.GetBytes(value);
            int slen = data.Length;

            GCHandle gch = GCHandle.Alloc(data, GCHandleType.Pinned);

            byte* b1 = ptr;
            byte* b2 = (byte*)gch.AddrOfPinnedObject();

            for (int i = 0; i < slen; i++)
            {
                *b1++ = *b2++;
            }

            if (addNull) *b1++ = 0;
            gch.Free();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe void internalSetString(char* ptr, string value, bool addNull)
        {
            int slen = value.Length;
            GCHandle gch = GCHandle.Alloc(Encoding.Unicode.GetBytes(value), GCHandleType.Pinned);

            char* b1 = ptr;
            char* b2 = (char*)gch.AddrOfPinnedObject();

            for (int i = 0; i < slen; i++)
            {
                *b1++ = *b2++;
            }

            if (addNull) *b1++ = '\x0';
            gch.Free();
        }


        /// <summary>
        /// Returns the string array at the byteIndex.
        /// </summary>
        /// <param name="byteIndex">Index at which to start copying.</param>
        /// <returns></returns>
        public string[] GetStringArray(long byteIndex)
        {
            unsafe
            {

                if (handle == IntPtr.Zero) return null;

                string s = null;

                char* cp = (char*)((ulong)handle + (ulong)byteIndex);
                char* ap = cp;

                int x = 0;

                List<string> o = new List<string>();

                while (true)
                {

                    if (*ap == (char)0)
                    {
                        if (x != 0)
                        {
                            s = new string(cp, 0, x);
                            o.Add(s);

                            s = null;
                            x = 0;

                            ap++;
                            cp = ap;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        x++;
                        ap++;
                    }
                }

                return o.ToArray();
            }

        }


        // These are the normal (canonical) memory allocation functions.

        /// <summary>
        /// Set all bytes in the buffer to zero at the optional index with the optional length.
        /// </summary>
        /// <param name="index">Start position of the buffer to zero, default is 0.</param>
        /// <param name="length">Size of the buffer to zero.  Default is to the end of the buffer.</param>
        /// <remarks></remarks>
        public void ZeroMemory(long index = -1, long length = -1)
        {
            long size = Size;
            if (size <= 0) return;

            long idx = index == -1 ? 0 : index;
            long len = length == -1 ? size - idx : length;

            // The length cannot be greater than the buffer length.
            if (len <= 0 || idx + len > size)
                return;

            unsafe
            {
                byte* bp1 = (byte*)((long)handle + idx);
                byte* bep = (byte*)((long)handle + idx + len);

                if (len >= IntPtr.Size)
                {
                    if (IntPtr.Size == 8)
                    {
                        long* lp1 = (long*)bp1;
                        long* lep = (long*)bep;

                        do
                        {
                            *lp1++ = 0L;
                        } while (lp1 < lep);

                        if (lp1 == lep) return;

                        lp1--;
                        bp1 = (byte*)lp1;
                    }
                    else
                    {
                        int* ip1 = (int*)bp1;
                        int* lep = (int*)bep;

                        do
                        {
                            *ip1++ = 0;
                        } while (ip1 < lep);

                        if (ip1 == lep) return;

                        ip1--;
                        bp1 = (byte*)ip1;
                    }
                }

                do
                {
                    *bp1++ = 0;
                } while (bp1 < bep);

            }

            // Native.n_memset(handle, 0, (IntPtr)len);
        }

        #region Editing

        /// <summary>
        /// Reverses the entire memory pointer.
        /// </summary>
        /// <returns>True if successful.</returns>
        /// <remarks></remarks>
        public bool Reverse()
        {
            if (handle == IntPtr.Zero || Size == 0)
                return false;

            long l = Size;

            unsafe
            {
                byte* b1, b2;

                b1 = (byte*)handle;
                b2 = (byte*)((long)handle + (l - 1));

                byte x;

                for (long i = 0; i < l; i++)
                {
                    x = *b1;

                    *b1 = *b2;
                    *b2 = x;

                    b1++;
                    b2--;
                }
            }

            return true;
        }

        /// <summary>
        /// Slides a block of memory toward the beginning or toward the end of the memory buffer,
        /// moving the memory around it to the other side.
        /// </summary>
        /// <param name="index">The index of the first byte in the affected block.</param>
        /// <param name="length">The length of the block.</param>
        /// <param name="offset">
        /// The offset amount of the slide.  If the amount is negative, 
        /// the block slides toward the beginning of the memory buffer. 
        /// If it is positive, it slides to the right.
        /// </param>
        /// <remarks></remarks>
        public void Slide(long index, long length, long offset)
        {
            if (offset == 0)
                return;
            long hl = Length;
            if (hl <= 0)
                return;

            if (0 > index + length + offset || index + length + offset > hl)
            {
                throw new IndexOutOfRangeException("Index out of bounds DataTools.Memory.MemPtr.Slide().");
            }

            IntPtr p1;
            IntPtr p2;

            p1 = (IntPtr)((long)handle + index);
            p2 = (IntPtr)((long)handle + index + offset);

            long a = offset < 0 ? offset * -1 : offset;

            MemPtr m = new MemPtr(length);
            MemPtr n = new MemPtr(a);

            Native.MemCpy(p1, m.Handle, length);
            Native.MemCpy(p2, n.Handle, a);

            p1 = (IntPtr)((long)handle + index + offset + length);
            Native.MemCpy(n.Handle, p1, a);
            Native.MemCpy(m.Handle, p2, length);

            m.Free();
            n.Free();
        }

        /// <summary>
        /// Pulls the data in from the specified index. 
        /// </summary>
        /// <param name="index">The index where contraction begins. The contraction starts at this position.</param>
        /// <param name="amount">Number of bytes to pull in.</param>
        /// <param name="removePressure">Specify whether to notify the garbage collector.</param>
        /// <remarks></remarks>
        public long PullIn(long index, long amount, bool removePressure = false)
        {
            long hl = Size;
            if (Size == 0 || 0 > index || index >= hl - 1)
            {
                throw new IndexOutOfRangeException("Index out of bounds DataTools.Memory.MemPtr.PullIn().");
            }

            long a = index + amount;
            long b = Size - a;
            Slide(a, b, -amount);
            ReAlloc(hl - amount);
            return Size;
        }

        /// <summary>
        /// Extend the buffer from the specified index.
        /// </summary>
        /// <param name="index">The index where expansion begins. The expansion starts at this position.</param>
        /// <param name="amount">Number of bytes to push out.</param>
        /// <param name="addPressure">Specify whether to notify the garbage collector.</param>
        /// <remarks></remarks>
        public long PushOut(long index, long amount, byte[] bytes = null, bool addPressure = false)
        {
            long hl = Length;
            if (hl <= 0)
            {
                Alloc(amount);
                return amount;
            }

            if (0 > index || index > hl - 1)
            {
                throw new IndexOutOfRangeException("Index out of bounds DataTools.Memory.MemPtr.PushOut().");
            }

            long ol = Size - index;
            ReAlloc(hl + amount);
            Slide(index, ol, amount);

            if (bytes != null && bytes.Length <= amount)
            {
                FromByteArray(bytes, index);
            }
            else
            {
                unsafe
                {
                    Native.ZeroMemory((void*)((long)handle + index), amount);
                }
            }

            return Size;
        }

        /// <summary>
        /// Slides a block of memory as Unicode characters toward the beginning or toward the end of the buffer.
        /// </summary>
        /// <param name="index">The character index preceding the first character in the affected block.</param>
        /// <param name="length">The length of the block, in characters.</param>
        /// <param name="offset">The offset amount of the slide, in characters.  If the amount is negative, the block slides to the left, if it is positive it slides to the right.</param>
        /// <remarks></remarks>
        public void SlideChar(long index, long length, long offset)
        {
            Slide(index << 1, length << 1, offset << 1);
        }

        /// <summary>
        /// Pulls the data in from the specified character index. 
        /// </summary>
        /// <param name="index">The index where contraction begins. The contraction starts at this position.</param>
        /// <param name="amount">Number of characters to pull in.</param>
        /// <param name="removePressure">Specify whether to notify the garbage collector.</param>
        /// <remarks></remarks>
        public long PullInChar(long index, long amount, bool removePressure = false)
        {
            return PullIn(index << 1, amount * 1);
        }

        /// <summary>
        /// Extend the buffer from the specified character index.
        /// </summary>
        /// <param name="index">The index where expansion begins. The expansion starts at this position.</param>
        /// <param name="amount">Number of characters to push out.</param>
        /// <param name="addPressure">Specify whether to notify the garbage collector.</param>
        /// <remarks></remarks>
        public long PushOutChar(long index, long amount, char[] chars = null, bool addPressure = false)
        {
            return PushOut(index << 1, amount * 1, Encoding.Unicode.GetBytes(chars));
        }

        /// <summary>
        /// Parts the string in both directions from index.  
        /// </summary>
        /// <param name="index">The index from which to expand.</param>
        /// <param name="amount">The amount of expansion, in both directions, so the total expansion will be amount * 1.</param>
        /// <param name="addPressure">Specify whether to notify the garbage collector.</param>
        /// <remarks></remarks>
        public void Part(long index, long amount, bool addPressure = false)
        {
            if (handle == IntPtr.Zero)
            {
                Alloc(amount);
                return;
            }

            long l = Size;
            if (l <= 0)
                return;

            long ol = l - index;
            ReAlloc(l + amount * 1);

            Slide(index, ol, amount);
            Slide(index + amount + 1, ol, amount);
        }

        /// <summary>
        /// Inserts the specified bytes at the specified index.
        /// </summary>
        /// <param name="index">Index at which to insert.</param>
        /// <param name="value">Byte array to insert</param>
        /// <param name="addPressure">Specify whether to notify the garbage collector.</param>
        /// <remarks></remarks>
        public void Insert(long index, byte[] value, bool addPressure = false)
        {
            PushOut(index, value.Length, value);
        }

        /// <summary>
        /// Inserts the specified characters at the specified character index.
        /// </summary>
        /// <param name="index">Index at which to insert.</param>
        /// <param name="value">Character array to insert</param>
        /// <param name="addPressure">Specify whether to notify the garbage collector.</param>
        /// <remarks></remarks>
        public void Insert(long index, char[] value, bool addPressure = false)
        {
            PushOutChar(index, value.Length, value);
        }

        /// <summary>
        /// Delete the memory from the specified index.  Calls PullIn.
        /// </summary>
        /// <param name="index">Index to start the delete.</param>
        /// <param name="amount">Amount of bytes to delete</param>
        /// <param name="removePressure">Specify whether to notify the garbage collector.</param>
        /// <remarks></remarks>
        public void Delete(long index, long amount, bool removePressure = false)
        {
            PullIn(index, amount);
        }

        /// <summary>
        /// Delete the memory from the specified character index.  Calls PullIn.
        /// </summary>
        /// <param name="index">Index to start the delete.</param>
        /// <param name="amount">Amount of characters to delete</param>
        /// <param name="removePressure">Specify whether to notify the garbage collector.</param>
        /// <remarks></remarks>
        public void DeleteChar(long index, long amount, bool removePressure = false)
        {
            PullInChar(index, amount);
        }

        /// <summary>
        /// Consumes the buffer in both directions from specified index.
        /// </summary>
        /// <param name="index">Index at which consuming begins.</param>
        /// <param name="amount">Amount of contraction, in both directions, so the total contraction will be amount * 1.</param>
        /// <param name="removePressure">Specify whether to notify the garbage collector.</param>
        /// <remarks></remarks>
        public void Consume(long index, long amount, bool removePressure = false)
        {
            long hl = Size;
            if (hl <= 0 || amount > index || index >= hl - amount + 1)
            {
                throw new IndexOutOfRangeException("Index out of bounds DataTools.Memory.Heap:Consume.");
            }

            index -= amount + 1;
            PullIn(index, amount);
            index += amount + 1;
            PullIn(index, amount);
        }

        /// <summary>
        /// Consumes the buffer in both directions from specified character index.
        /// </summary>
        /// <param name="index">Index at which consuming begins.</param>
        /// <param name="amount">Amount of contraction, in both directions, so the total contraction will be amount * 1.</param>
        /// <param name="removePressure">Specify whether to notify the garbage collector.</param>
        /// <remarks></remarks>
        public void ConsumeChar(long index, long amount, bool removePressure = false)
        {
            long hl = Size;
            if (hl <= 0 || amount > index || index >= Convert.ToInt64(hl >> 1) - (amount + 1))
            {
                throw new IndexOutOfRangeException("Index out of bounds DataTools.Memory.Heap:Consume.");
            }

            index -= amount + 1 << 1;
            PullIn(index, amount);
            index += amount + 1 << 1;
            PullIn(index, amount);
        }

        #endregion

        /// <summary>
        /// Allocate a block of memory on a heap (typically the process heap).  
        /// </summary>
        /// <param name="size">The size to attempt to allocate</param>
        /// <param name="addPressure">Whether or not to call GC.AddMemoryPressure</param>
        /// <param name="hHeap">
        /// Optional handle to an alternate heap.  The process heap is used if this is set to null.
        /// If you use an alternate heap handle, you will need to free the memory using the same heap handle or an error will occur.
        /// </param>
        /// <param name="zeroMem">Whether or not to zero the contents of the memory on allocation.</param>
        /// <returns>True if successful. If False, call GetLastError or FormatLastError to find out more information.</returns>
        /// <remarks></remarks>
        public bool Alloc(long size, bool addPressure = false, IntPtr? hHeap = null, bool zeroMem = true)
        {
            long l = Size;
            bool al;

            if (hHeap == null)
                hHeap = procHeap;

            // While the function doesn't need to call HeapAlloc, it hasn't necessarily failed, either.
            if (size == l) return true;

            if (l > 0)
            {
                // we already have a pointer, so we will call realloc, instead.
                return ReAlloc(size);
            }

            handle = Native.HeapAlloc((IntPtr)hHeap, zeroMem ? 8u : 0, (IntPtr)size);
            al = handle != IntPtr.Zero;

            // see if we need to tell the garbage collector anything.
            if (al && addPressure)
                GC.AddMemoryPressure(size);

            return al;
        }

        /// <summary>
        /// Allocate a block of memory on the process heap.  
        /// </summary>
        /// <param name="size">The size to attempt to allocate</param>
        /// <param name="addPressure">Whether or not to call GC.AddMemoryPressure</param>
        /// <returns>True if successful. If False, call GetLastError or FormatLastError to find out more information.</returns>
        /// <remarks></remarks>
        public bool Alloc(long size, bool addPressure)
        {
            return Alloc(size, addPressure, null, true);
        }


        /// <summary>
        /// Allocate a block of memory on the process heap.  
        /// </summary>
        /// <param name="size">The size to attempt to allocate</param>
        /// <returns>True if successful. If False, call GetLastError or FormatLastError to find out more information.</returns>
        /// <remarks></remarks>
        public bool Alloc(long size)
        {
            return Alloc(size, false, null, true);
        }

        /// <summary>
        /// (Deprecated) Allocate a block of memory and set its contents to zero.
        /// </summary>
        /// <param name="size">The size to attempt to allocate</param>
        /// <param name="addPressure">Whether or not to call GC.AddMemoryPressure</param>
        /// <param name="hHeap">
        /// Optional handle to an alternate heap.  The process heap is used if this is set to null.
        /// If you use an alternate heap handle, you will need to free the memory using the same heap handle or an error will occur.
        /// </param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool AllocZero(long size, bool addPressure = false, IntPtr? hHeap = null)
        {
            return Alloc(size, addPressure, hHeap, true);
        }


        /// <summary>
        /// Allocates memory aligned to a particular byte boundary.
        /// Memory allocated in this way must be freed with AlignedFree()
        /// </summary>
        /// <param name="size">Size of the memory to allocate.</param>
        /// <param name="alignment">The byte alignment of the memory.</param>
        /// <param name="addPressure">Specify whether or not to add memory pressure to the garbage collector.</param>
        /// <param name="hHeap">
        /// Optional handle to an alternate heap.  The process heap is used if this is set to null.
        /// If you use an alternate heap handle, you will need to free the memory using the same heap handle or an error will occur.
        /// </param>
        /// <returns></returns>
        public bool AlignedAlloc(long size, long alignment = 512, bool addPressure = false, IntPtr? hHeap = null)
        {
            if (alignment == 0 || (alignment & 1) != 0)
                return false;

            if (handle != IntPtr.Zero)
            {
                if (!Free())
                    return false;
            }

            long l = size + (alignment - 1) + 8;
            if (hHeap == null)
                hHeap = procHeap;

            if (l < 1)
                return false;

            IntPtr p = Native.HeapAlloc((IntPtr)hHeap, 8, (IntPtr)l);

            if (p == IntPtr.Zero) return false;

            IntPtr p2 = (IntPtr)((long)p + (alignment - 1) + 8);

            if (p == IntPtr.Zero)
                return false;

            p2 = (IntPtr)((long)p2 - p2.ToInt64() % alignment);

            MemPtr mm = p2;

            mm.LongAt(-1) = p.ToInt64();
            handle = p2;

            if (addPressure)
                GC.AddMemoryPressure(l);

            return true;
        }

        /// <summary>
        /// Frees a previously allocated block of aligned memory.
        /// </summary>
        /// <param name="removePressure">Specify whether or not to remove memory pressure from the garbage collector.</param>
        /// <param name="hHeap">
        /// Optional handle to an alternate heap.  The process heap is used if this is set to null.
        /// If you use an alternate heap handle, you will need to free the memory using the same heap handle or an error will occur.
        /// </param>
        /// <returns></returns>
        public bool AlignedFree(bool removePressure = false, IntPtr? hHeap = null)
        {
            if (handle == IntPtr.Zero)
                return true;
            if (hHeap == null)
                hHeap = procHeap;

            IntPtr p = (IntPtr)LongAt(-1);
            long l = Convert.ToInt64(Native.HeapSize((IntPtr)hHeap, 0, p));

            if (!Native.HeapFree((IntPtr)hHeap, 0, p))
            {
                if (removePressure)
                    GC.RemoveMemoryPressure(l);

                handle = IntPtr.Zero;
                return true;
            }
            else
                return false;
        }


        /// <summary>
        /// Reallocate a block of memory to a different size on the task heap.
        /// </summary>
        /// <param name="size">The size to attempt to allocate</param>
        /// <param name="modifyPressure">Whether or not to call GC.AddMemoryPressure or GC.RemoveMemoryPressure.</param>
        /// <param name="hHeap">
        /// Optional handle to an alternate heap.  The process heap is used if this is set to null.
        /// </param>
        /// <returns>True if successful. If False, call GetLastError or FormatLastError to find out more information.</returns>
        /// <remarks></remarks>
        public bool ReAlloc(long size, bool modifyPressure = false, IntPtr? hHeap = null)
        {
            if (handle == IntPtr.Zero) return Alloc(size, modifyPressure, hHeap);

            long l = Size;
            bool ra;

            if (hHeap == null)
                hHeap = procHeap;

            // While the function doesn't need to call HeapReAlloc, it hasn't necessarily failed, either.
            if (size == l) return true;

            if (l <= 0)
            {
                // we don't have a pointer yet, so we have to call alloc instead.
                return Alloc(size);
            }

            handle = Native.HeapReAlloc((IntPtr)hHeap, 8, handle, new IntPtr(size));
            ra = handle != IntPtr.Zero;

            // see if we need to tell the garbage collector anything.
            if (ra && modifyPressure)
            {
                size = Size;
                if (size < l)
                    GC.RemoveMemoryPressure(l - size);
                else
                    GC.AddMemoryPressure(size - l);
            }

            return ra;
        }

        /// <summary>
        /// Frees a previously allocated block of memory on the task heap.
        /// </summary>
        /// <returns>True if successful. If False, call GetLastError or FormatLastError to find out more information.</returns>
        /// <param name="removePressure">Whether or not to call GC.RemoveMemoryPressure</param>
        /// <param name="hHeap">
        /// Optional handle to an alternate heap.  The process heap is used if this is set to null.
        /// The handle pointed to by the internal pointer must have been previously allocated with the same heap handle.
        /// </param>
        /// <remarks></remarks>
        public bool Free(bool removePressure = false, IntPtr? hHeap = null)
        {
            // While the function doesn't need to call HeapFree, it hasn't necessarily failed, either.
            if (handle == IntPtr.Zero)
                return true;
            else
            {

                if (hHeap == null) hHeap = procHeap;

                long l = 0;

                // see if we need to tell the garbage collector anything.
                if (removePressure) l = Size;

                var res = Native.HeapFree((IntPtr)hHeap, 0, handle);

                // see if we need to tell the garbage collector anything.
                if (res)
                {
                    handle = IntPtr.Zero;
                    if (removePressure) GC.RemoveMemoryPressure(l);
                }

                return res;
            }
        }

        /// <summary>
        /// Validates whether the pointer referenced by this structure
        /// points to a valid and accessible block of memory.
        /// </summary>
        /// <returns>True if the memory block is valid, or False if the pointer is invalid or zero.</returns>
        /// <remarks></remarks>
        public bool Validate()
        {
            if (handle == IntPtr.Zero)
            {
                return false;
            }

            return Native.HeapValidate(procHeap, 0, handle);
        }

        /// <summary>
        /// Frees a previously allocated block of memory on the task heap with LocalFree()
        /// </summary>
        /// <returns>True if successful. If False, call GetLastError or FormatLastError to find out more information.</returns>
        /// <remarks></remarks>
        public bool LocalFree()
        {
            if (handle == IntPtr.Zero)
                return false;
            else
            {
                handle = Native.LocalFree(handle);
                return handle != IntPtr.Zero;
            }
        }

        /// <summary>
        /// Frees a previously allocated block of memory on the task heap with GlobalFree()
        /// </summary>
        /// <returns>True if successful. If False, call GetLastError or FormatLastError to find out more information.</returns>
        /// <remarks></remarks>
        public bool GlobalFree()
        {
            if (handle == IntPtr.Zero)
                return false;
            else
            {
                handle = Native.GlobalFree(handle);
                return handle == IntPtr.Zero;
            }
        }

        /// <summary>
        /// Frees a block of memory previously allocated by COM.
        /// </summary>
        /// <remarks></remarks>
        public void CoTaskMemFree()
        {
            Marshal.FreeCoTaskMem(handle);
        }


        // NetApi Memory functions should be used carefully and not within the context
        // of any scenario when you may accidentally call normal memory management functions
        // on any region of memory allocated with the network memory functions. 
        // Be mindful of usage.
        // Some normal functions such as Length and SetLength cannot be used.
        // Normal allocation and deallocation functions cannot be used, at all.
        // NetApi memory is not reallocatable.
        // The size of a NetApi memory buffer cannot be retrieved.

        /// <summary>
        /// Allocate a network API compatible memory buffer.
        /// </summary>
        /// <param name="size">Size of the buffer to allocate, in bytes.</param>
        /// <remarks></remarks>
        public void NetAlloc(int size)
        {
            // just ignore an allocated buffer.
            if (handle != IntPtr.Zero)
                return;

            Native.NetApiBufferAllocate(size, ref handle);
        }

        /// <summary>
        /// Free a network API compatible memory buffer previously allocated with NetAlloc.
        /// </summary>
        /// <remarks></remarks>
        public void NetFree()
        {
            if (handle == IntPtr.Zero)
                return;

            Native.NetApiBufferFree(handle);
            handle = IntPtr.Zero;
        }


        // Virtual Memory should be used carefully and not within the context
        // of any scenario when you may accidentally call normal memory management functions
        // on any region of memory allocated with the Virtual functions. 
        // Be mindful of usage.
        // Some normal functions such as Length and SetLength cannot be used (use VirtualLength).
        // Normal allocation and deallocation functions cannot be used, at all.
        // Virtual memory is not reallocatable.

        /// <summary>
        /// Allocates a region of virtual memory.
        /// </summary>
        /// <param name="size">The size of the region of memory to allocate.</param>
        /// <param name="addPressure">Whether to call GC.AddMemoryPressure</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool VirtualAlloc(long size, bool addPressure = true)
        {
            long l = 0;
            bool va;

            // While the function doesn't need to call VirtualAlloc, it hasn't necessarily failed, either.
            if (size == l && handle != IntPtr.Zero) return true;


            handle = Native.VirtualAlloc(IntPtr.Zero, (IntPtr)size, VMemAllocFlags.MEM_COMMIT | VMemAllocFlags.MEM_RESERVE, MemoryProtectionFlags.PAGE_READWRITE);

            va = handle != IntPtr.Zero;

            if (va && addPressure)
                GC.AddMemoryPressure(VirtualLength());

            return va;
        }

        /// <summary>
        /// Frees a region of memory previously allocated with VirtualAlloc.
        /// </summary>
        /// <param name="removePressure">Whether to call GC.RemoveMemoryPressure</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool VirtualFree(bool removePressure = true)
        {
            long l = 0;
            bool vf;

            // While the function doesn't need to call vf, it hasn't necessarily failed, either.
            if (handle == IntPtr.Zero)
                vf = true;
            else
            {
                // see if we need to tell the garbage collector anything.
                if (removePressure)
                    l = VirtualLength();

                vf = Native.VirtualFree(handle);

                // see if we need to tell the garbage collector anything.
                if (vf)
                {
                    handle = IntPtr.Zero;
                    if (removePressure)
                        GC.RemoveMemoryPressure(l);
                }
            }

            return vf;
        }

        /// <summary>
        /// Returns the size of a region of virtual memory previously allocated with VirtualAlloc.
        /// </summary>
        /// <returns>The size of a virtual memory region or zero.</returns>
        /// <remarks></remarks>
        public long VirtualLength()
        {
            if (handle == IntPtr.Zero)
                return 0;

            MEMORY_BASIC_INFORMATION m = new MEMORY_BASIC_INFORMATION();

            if (Native.VirtualQuery(handle, ref m, (IntPtr)Marshal.SizeOf(m)) != IntPtr.Zero)
                return Convert.ToInt64(m.RegionSize);

            return 0;
        }


        public void ZeroMemory()
        {
            unsafe
            {
                void* p = (void*)handle;
                if (p == null || Size == 0) return;

                Native.ZeroMemory(p, Size);
            }
        }


        public override string ToString()
        {
            if (handle == IntPtr.Zero) return "";
            return GetString(0);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator string(MemPtr val)
        {
            if (val.handle == (IntPtr)0) return null;
            return val.GetString(0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator MemPtr(string val)
        {
            var op = new MemPtr((val.Length + 1) * sizeof(char));
            op.SetString(0, val);
            return op;
        }



        public static explicit operator byte[](MemPtr val)
        {
            return val.ToByteArray();
        }

        public static explicit operator MemPtr(byte[] val)
        {
            var n = new MemPtr();
            n.FromByteArray(val);
            return n;
        }



        public static explicit operator char[](MemPtr val)
        {
            return val.ToCharArray();
        }

        public static explicit operator MemPtr(char[] val)
        {
            var n = new MemPtr();
            n.FromCharArray(val);
            return n;
        }


        public static explicit operator sbyte[](MemPtr val)
        {
            return val.ToArray<sbyte>();
        }

        public static explicit operator MemPtr(sbyte[] val)
        {
            var n = new MemPtr();
            n.FromArray(val);
            return n;
        }

        public static explicit operator short[](MemPtr val)
        {
            return val.ToArray<short>();
        }

        public static explicit operator MemPtr(short[] val)
        {
            var n = new MemPtr();
            n.FromArray(val);
            return n;
        }


        public static explicit operator ushort[](MemPtr val)
        {
            return val.ToArray<ushort>();
        }

        public static explicit operator MemPtr(ushort[] val)
        {
            var n = new MemPtr();
            n.FromArray(val);
            return n;
        }


        public static explicit operator int[](MemPtr val)
        {
            return val.ToArray<int>();
        }

        public static explicit operator MemPtr(int[] val)
        {
            var n = new MemPtr();
            n.FromArray(val);
            return n;
        }


        public static explicit operator uint[](MemPtr val)
        {
            return val.ToArray<uint>();
        }

        public static explicit operator MemPtr(uint[] val)
        {
            var n = new MemPtr();
            n.FromArray(val);
            return n;
        }



        public static explicit operator long[](MemPtr val)
        {
            return val.ToArray<long>();
        }

        public static explicit operator MemPtr(long[] val)
        {
            var n = new MemPtr();
            n.FromArray(val);
            return n;
        }


        public static explicit operator ulong[](MemPtr val)
        {
            return val.ToArray<ulong>();
        }

        public static explicit operator MemPtr(ulong[] val)
        {
            var n = new MemPtr();
            n.FromArray(val);
            return n;
        }



        public static explicit operator float[](MemPtr val)
        {
            return val.ToArray<float>();
        }

        public static explicit operator MemPtr(float[] val)
        {
            var n = new MemPtr();
            n.FromArray(val);
            return n;
        }


        public static explicit operator double[](MemPtr val)
        {
            return val.ToArray<double>();
        }

        public static explicit operator MemPtr(double[] val)
        {
            var n = new MemPtr();
            n.FromArray(val);
            return n;
        }



        public static explicit operator decimal[](MemPtr val)
        {
            return val.ToArray<decimal>();
        }

        public static explicit operator MemPtr(decimal[] val)
        {
            var n = new MemPtr();
            n.FromArray(val);
            return n;
        }


        public static explicit operator DateTime[](MemPtr val)
        {
            return val.ToArray<DateTime>();
        }

        public static explicit operator MemPtr(DateTime[] val)
        {
            var n = new MemPtr();
            n.FromArray(val);
            return n;
        }


        public static explicit operator Guid[](MemPtr val)
        {
            return val.ToArray<Guid>();
        }

        public static explicit operator MemPtr(Guid[] val)
        {
            var n = new MemPtr();
            n.FromArray(val);
            return n;
        }


        public static MemPtr operator +(MemPtr val1, byte[] val2)
        {
            var c = val1.Size;

            val1.Alloc(val1.Size + val2.Length);
            val1.FromByteArray(val2, c);

            return val1;
        }

        public static MemPtr operator +(MemPtr val1, short val2)
        {
            val1.handle += val2;
            return val1;
        }

        public static MemPtr operator -(MemPtr val1, short val2)
        {
            val1.handle -= val2;
            return val1;
        }

        public static MemPtr operator +(MemPtr val1, ushort val2)
        {
            val1.handle += val2;
            return val1;
        }

        public static MemPtr operator -(MemPtr val1, ushort val2)
        {
            val1.handle -= val2;
            return val1;
        }

        public static MemPtr operator +(MemPtr val1, int val2)
        {
            val1.handle += val2;
            return val1;
        }

        public static MemPtr operator -(MemPtr val1, int val2)
        {
            val1.handle -= val2;
            return val1;
        }

        public static MemPtr operator +(MemPtr val1, long val2)
        {
            val1.handle = (IntPtr)((long)val1.handle + val2);
            return val1;
        }

        public static MemPtr operator -(MemPtr val1, long val2)
        {
            val1.handle = (IntPtr)((long)val1.handle - val2);
            return val1;
        }


        public static MemPtr operator +(MemPtr val1, uint val2)
        {
            val1.handle = (IntPtr)((uint)val1.handle + val2);
            return val1;
        }

        public static MemPtr operator -(MemPtr val1, uint val2)
        {
            val1.handle = (IntPtr)((uint)val1.handle - val2);
            return val1;
        }


        public static MemPtr operator +(MemPtr val1, ulong val2)
        {
            val1.handle = (IntPtr)((ulong)val1.handle + val2);
            return val1;
        }

        public static MemPtr operator -(MemPtr val1, ulong val2)
        {
            val1.handle = (IntPtr)((ulong)val1.handle - val2);
            return val1;
        }

        public static MemPtr operator +(MemPtr val1, IntPtr val2)
        {
            val1.handle = (IntPtr)((long)val1.handle + (long)val2);
            return val1;
        }

        public static MemPtr operator -(MemPtr val1, IntPtr val2)
        {
            val1.handle = (IntPtr)((long)val1.handle - (long)val2);
            return val1;
        }

        public static bool operator ==(MemPtr val1, MemPtr val2)
        {
            return val1.Handle == val2.handle;
        }

        public static bool operator !=(MemPtr val1, MemPtr val2)
        {
            return val1.Handle != val2.handle;
        }

        public static bool operator ==(IntPtr val1, MemPtr val2)
        {
            return val1 == val2.handle;
        }

        public static bool operator !=(IntPtr val1, MemPtr val2)
        {
            return val1 != val2.handle;
        }

        public static bool operator ==(MemPtr val2, IntPtr val1)
        {
            return val1 == val2.handle;
        }

        public static bool operator !=(MemPtr val2, IntPtr val1)
        {
            return val1 != val2.handle;
        }

        public static implicit operator IntPtr(MemPtr val)
        {
            unsafe
            {
                return val.handle;
            }
        }

        public static implicit operator MemPtr(IntPtr val)
        {
            unsafe
            {
                return new MemPtr
                {
                    handle = (IntPtr)(void*)val
                };
            }
        }

        public static implicit operator UIntPtr(MemPtr val)
        {
            unsafe
            {
                return (UIntPtr)(void*)val.handle;
            }
        }

        public static implicit operator MemPtr(UIntPtr val)
        {
            unsafe
            {
                return new MemPtr
                {
                    handle = (IntPtr)(void*)val
                };
            }
        }

    }
}
