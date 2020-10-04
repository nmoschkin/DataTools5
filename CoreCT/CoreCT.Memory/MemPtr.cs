using System;
using System.Collections.Generic;
using System.Linq;
using CoreCT.Memory;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations;
using CoreCT.Memory.NativeLib;

namespace CoreCT.Memory
{

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct MemPtr
    {
        internal IntPtr handle;

        private static IntPtr procHeap = Native.GetProcessHeap();

        public long Size
        {
            get
            {
                if (handle == IntPtr.Zero) return 0;
                return (long)Native.HeapSize(procHeap, 0, Handle);
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
            private set
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

        [MethodImpl( MethodImplOptions.AggressiveInlining)]
        public ref byte ByteAt(long index)
        {
            unsafe
            {
                return ref *(byte*)((long)handle + index);
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte[] ToByteArray(long index = 0, int length = 0)
        {
            long len = length;
            long size = Size;

            if (len == 0) len = (size - index);
            if (size - index < length) len = size - index;

            if (len > int.MaxValue) len = int.MaxValue;

            byte[] output = new byte[len];
                       
            GCHandle gch = GCHandle.Alloc(output, GCHandleType.Pinned);

            unsafe
            {
                void* ptr1 = (void*)((long)handle + index);
                void* ptr2 = (void*)gch.AddrOfPinnedObject();

                Native.MemCpy(ptr1, ptr2, len);
            }

            gch.Free();
            return output;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public char[] ToCharArray(long index = 0, int length = 0)
        {
            long len = length * sizeof(char);
            long size = Size;

            if (len == 0) len = (size - index);
            if (size - index < length) len = size - index;

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

                Native.MemCpy(ptr1, ptr2, len);
            }

            gch.Free();
            return output;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] ToArray<T>(long index = 0, int length = 0) where T: struct
        {
            unsafe
            {
                int tlen = typeof(T) == typeof(char) ? sizeof(char) : Marshal.SizeOf<T>();

                long len = length * tlen;
                long size = Size;

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

                    Native.MemCpy(ptr1, ptr2, len);
                }

                gch.Free();
                return output;
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
        public void SetString(long index, string value)
        {
            unsafe
            {
                internalSetString((char*)((long)handle + index), value);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetStringIndirect(long index, string value)
        {
            unsafe
            {
                char* ptr = (char*)*(IntPtr*)((long)handle + index);
                internalSetString(ptr, value);
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
        public string GetUT8String(long index)
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
        public void SetUTF8String(long index, string value)
        {
            unsafe
            {
                internalSetUTF8String((byte*)((long)handle + index), value);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetUTF8StringIndirect(long index, string value)
        {
            unsafe
            {
                internalSetUTF8String((byte*)*(IntPtr*)((long)handle + index), value);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe void internalSetUTF8String(byte* ptr, string value)
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

            *b1++ = 0;
            gch.Free();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe void internalSetString(char* ptr, string value)
        {
            int slen = value.Length;
            GCHandle gch = GCHandle.Alloc(Encoding.Unicode.GetBytes(value), GCHandleType.Pinned);

            char* b1 = ptr;
            char* b2 = (char*)(gch.AddrOfPinnedObject());

            for (int i = 0; i < slen; i++)
            {
                *b1++ = *b2++;
            }

            *b1++ = '\x0';
            gch.Free();
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

            handle = Native.HeapAlloc((IntPtr)hHeap, zeroMem ? 8 : 0, (IntPtr)size);
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
            if ((alignment == 0) || (alignment & 1) != 0)
                return false;

            if ((handle != IntPtr.Zero))
            {
                if (!Free())
                    return false;
            }

            long l = size + (alignment - 1) + 8;
            if (hHeap == null)
                hHeap = procHeap;

            if ((l < 1))
                return false;

            IntPtr p = Native.HeapAlloc((IntPtr)hHeap, 8, (IntPtr)l);

            if (p == IntPtr.Zero) return false;

            IntPtr p2 = (IntPtr)((long)p + (alignment - 1) + 8);

            if (p == IntPtr.Zero)
                return false;

            p2 = (IntPtr)((long)p2 - (p2.ToInt64() % alignment));

            MemPtr mm = p2;

            mm.LongAt(-1) = p.ToInt64();
            handle = p2;

            if ((addPressure))
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
            if ((handle == IntPtr.Zero))
                return true;
            if (hHeap == null)
                hHeap = procHeap;

            IntPtr p = (IntPtr)LongAt(-1);
            long l = System.Convert.ToInt64(Native.HeapSize((IntPtr)hHeap, 0, p));

            if (!Native.HeapFree((IntPtr)hHeap, 0, p))
            {
                if ((removePressure))
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
            if (hHeap == null) hHeap = procHeap;

            long l = 0;

            // While the function doesn't need to call HeapFree, it hasn't necessarily failed, either.
            if (handle == IntPtr.Zero)
                return true;
            else
            {
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
                return System.Convert.ToInt64(m.RegionSize);

            return 0;
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

        public static explicit operator string(MemPtr val)
        {
            if (val.handle == IntPtr.Zero) return null;
            return val.GetString(0);
        }

        public static explicit operator MemPtr(string val)
        {
            var op = new MemPtr((val.Length + 1) * sizeof(char));
            op.SetString(0, val);
            return op;
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
