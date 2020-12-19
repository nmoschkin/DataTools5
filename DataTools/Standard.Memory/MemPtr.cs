using System;
using System.Collections.Generic;
using System.Linq;
using DataTools.Memory;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using DataTools.Standard.Memory.NativeLib;

namespace DataTools.Standard.Memory
{

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct MemPtr
    {
        internal IntPtr handle;
        public static readonly MemPtr Empty = new MemPtr(IntPtr.Zero);

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
            handle = IntPtr.Zero;
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

        public uint CalculateCrc32(int size)
        {
            long c = size;
            if (handle == IntPtr.Zero || c <= 0) return 0;

            unsafe
            {
                return Crc32.Calculate((byte*)handle, c);
            }
        }


        [MethodImpl( MethodImplOptions.AggressiveInlining)]
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
                return ref *(char*)((long)handle + (index * sizeof(char)));
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
                return ref *(short*)((long)handle + (index * sizeof(short)));
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
                return ref *(ushort*)((long)handle + (index * sizeof(ushort)));
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
                return ref *(int*)((long)handle + (index * sizeof(int)));
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
                return ref *(uint*)((long)handle + (index * sizeof(uint)));
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
                return ref *(long*)((long)handle + (index * sizeof(long)));
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
                return ref *(ulong*)((long)handle + (index * sizeof(ulong)));
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
                return ref *(float*)((long)handle + (index * sizeof(float)));
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
                return ref *(double*)((long)handle + (index * sizeof(double)));
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
                return ref *(decimal*)((long)handle + (index * sizeof(decimal)));
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
                return ref *(Guid*)((long)handle + (index * sizeof(Guid)));
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
                return ref *(DateTime*)((long)handle + (index * sizeof(DateTime)));
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
        public byte[] ToByteArray(long index, long length)
        {
            if (handle == IntPtr.Zero || length < 0) return null;

            long len = length;

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
        public char[] ToCharArray(long index, int length)
        {
            if (handle == IntPtr.Zero || length < 0) return null;

            long len = length * sizeof(char);

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
        public T[] ToArray<T>(long index, int length) where T: struct
        {
            if (handle == IntPtr.Zero || length < 0) return null;

            unsafe
            {
                int tlen = typeof(T) == typeof(char) ? sizeof(char) : Marshal.SizeOf<T>();

                long len = length * tlen;
                long size = len;

                if (len == 0) len = (size - index);
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
                ReAlloc((value.Length * sizeof(char)) + index);
            }
            else
            {
                Alloc((value.Length * sizeof(char)) + index);
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
                ReAlloc((value.Length * cb) + index);
            }
            else
            {
                Alloc((value.Length * cb) + index);
            }
            unsafe
            {
                var vl = value.Length * cb;
                GCHandle gch = GCHandle.Alloc(value, GCHandleType.Pinned);
                Buffer.MemoryCopy((void*)gch.AddrOfPinnedObject(), (void*)((long)handle + index), vl, vl);
                gch.Free();
            }
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
            byte* b2 = (byte*)(gch.AddrOfPinnedObject());

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
            char* b2 = (char*)(gch.AddrOfPinnedObject());

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
        public void ZeroMemory(long index, long length)
        {
            long size = length;
            if (size <= 0) return;

            long idx = index == -1 ? 0 : index;
            long len = length == -1 ? size - idx : length;

            // The length cannot be greater than the buffer length.
            if (len <= 0 || (idx + len) > size)
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
        public bool Alloc(long size, bool addPressure = false, bool zeroMem = true)
        {
            bool al;

            handle = Marshal.AllocHGlobal((IntPtr)size);
            al = handle != IntPtr.Zero;

            // see if we need to tell the garbage collector anything.
            if (al && addPressure)
                GC.AddMemoryPressure(size);

            if (zeroMem)
            {
                ZeroMemory(0, size);
            }

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
            return Alloc(size, addPressure, true);
        }


        /// <summary>
        /// Allocate a block of memory on the process heap.  
        /// </summary>
        /// <param name="size">The size to attempt to allocate</param>
        /// <returns>True if successful. If False, call GetLastError or FormatLastError to find out more information.</returns>
        /// <remarks></remarks>
        public bool Alloc(long size)
        {
            return Alloc(size, false, true);
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
        public bool AllocZero(long size, bool addPressure = false)
        {
            return Alloc(size, addPressure, true);
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
        public bool ReAlloc(long size, int oldsize = 0, bool modifyPressure = false)
        {
            if (handle == IntPtr.Zero) return Alloc(size, modifyPressure);

            bool ra;

            // While the function doesn't need to call HeapReAlloc, it hasn't necessarily failed, either.
            
            handle = Marshal.ReAllocHGlobal(handle, new IntPtr(size));

            ra = handle != IntPtr.Zero;

            // see if we need to tell the garbage collector anything.
            if (ra && modifyPressure && oldsize > 0)
            {
                if (size < oldsize)
                    GC.RemoveMemoryPressure(oldsize - size);
                else
                    GC.AddMemoryPressure(size - oldsize);
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
        public bool Free(bool removePressure = false, long oldSize = 0)
        {
            
            // While the function doesn't need to call HeapFree, it hasn't necessarily failed, either.
            if (handle == IntPtr.Zero)
                return true;
            else
            {
                // see if we need to tell the garbage collector anything.
                long l = oldSize;
                Marshal.FreeHGlobal(handle);

                // see if we need to tell the garbage collector anything.
                handle = IntPtr.Zero;
                if (removePressure && oldSize > 0) GC.RemoveMemoryPressure(l);
            }

            return true;
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

        public static bool operator ==(IntPtr val1, MemPtr val2)
        {
            return (val1 == val2.handle);
        }

        public static bool operator !=(IntPtr val1, MemPtr val2)
        {
            return (val1 != val2.handle);
        }

        public static bool operator ==(MemPtr val2, IntPtr val1)
        {
            return (val1 == val2.handle);
        }

        public static bool operator !=(MemPtr val2, IntPtr val1)
        {
            return (val1 != val2.handle);
        }


        public static bool operator ==(MemPtr val2, MemPtr val1)
        {
            return (val1.handle == val2.handle);
        }

        public static bool operator !=(MemPtr val2, MemPtr val1)
        {
            return (val1.handle != val2.handle);
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
