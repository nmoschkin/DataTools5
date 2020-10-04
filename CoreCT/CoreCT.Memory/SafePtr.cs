using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Data;
using CoreCT.Memory;
using System.Runtime.CompilerServices;
using CoreCT.Memory.NativeLib;

namespace CoreCT.Memory
{
    public class SafePtr : SafeHandle
    {
        private static IntPtr procHeap = Native.GetProcessHeap();

        private IntPtr currentHeap = procHeap;

        public override bool IsInvalid => handle == (IntPtr)0;

        public long Size { get; private set; }

        public MemoryType MemoryType { get; private set; }

        public bool IsOwner { get; private set; }
       
        public bool HasGCPressure { get; private set; }

        public IntPtr CurrentHeap
        {
            get
            {
                if (currentHeap == IntPtr.Zero)
                {
                    currentHeap = procHeap;
                }

                return currentHeap;
            }
            private set
            {
                
                if (value == IntPtr.Zero)
                {
                    currentHeap = procHeap;
                }
                else if (currentHeap == value)
                {
                    return;
                }
                else
                {
                    currentHeap = value;
                }
            }
        }

        public SafePtr(IntPtr ptr, int size, MemoryType t, bool fOwn) : base((IntPtr)0, fOwn)
        {
            handle = ptr;
            Size = size;
            MemoryType = t;
            IsOwner = fOwn;
        }
        
        public SafePtr(IntPtr ptr, long size) : base((IntPtr)0, false)
        {
            handle = ptr;
            Size = size;
        }

        public SafePtr(IntPtr ptr, int size) : base((IntPtr)0, false)
        {
            handle = ptr;
            Size = size;
        }

        public SafePtr(IntPtr ptr) : base((IntPtr)0, false)
        {
            handle = ptr;
        }

        public SafePtr(IntPtr ptr, bool fOwn) : base((IntPtr)0, fOwn)
        {
            handle = ptr;
            IsOwner = fOwn;
        }

        public SafePtr(IntPtr ptr, long size, bool fOwn) : base((IntPtr)0, fOwn)
        {
            handle = ptr;
            Size = size;
            IsOwner = fOwn;
        }

        public SafePtr(IntPtr ptr, int size, bool fOwn) : base((IntPtr)0, fOwn)
        {
            handle = ptr;
            Size = size;
            IsOwner = fOwn;
        }

        public unsafe SafePtr(void* ptr, int size) : base((IntPtr)0, false)
        {
            handle = (IntPtr)ptr;
            Size = size;
        }

        public unsafe SafePtr(void* ptr, long size) : base((IntPtr)0, false)
        {
            handle = (IntPtr)ptr;
            Size = size;
        }

        public unsafe SafePtr(void* ptr) : base((IntPtr)0, false)
        {
            handle = (IntPtr)ptr;
        }

        public unsafe SafePtr(void* ptr, bool fOwn) : base((IntPtr)0, fOwn)
        {
            handle = (IntPtr)ptr;
            IsOwner = fOwn;
        }

        public unsafe SafePtr(void* ptr, long size, bool fOwn) : base((IntPtr)0, fOwn)
        {
            handle = (IntPtr)ptr;
            Size = size;
            IsOwner = fOwn;
        }

        public unsafe SafePtr(void* ptr, int size, bool fOwn) : base((IntPtr)0, fOwn)
        {
            handle = (IntPtr)ptr;
            Size = size;
            IsOwner = fOwn;
        }

        public SafePtr() : base((IntPtr)0, true)
        {
            IsOwner = true;
        }

        public SafePtr(long size) : this()
        {
            if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size));
            Alloc(size);
        }

        public SafePtr(int size) : this()
        {
            if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size));
            Alloc(size);
        }

        public SafePtr(long size, MemoryType t) : this()
        {
            if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size));

            MemoryType = t;
            TAlloc(size);
        }

        public SafePtr(int size, MemoryType t) : this()
        {
            if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size));

            MemoryType = t;
            TAlloc(size);
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
        public string GetUTF8String(long index)
        {
            unsafe
            {
                byte* b1 = (byte*)((long)handle + index);
                byte* b2 = b1;

                while (*b2 != 0) b2++;
                if (b1 == b2) return "";

                return Encoding.UTF8.GetString(b1, (int)(b2 - b1));
            }
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
        protected unsafe void internalSetUTF8String(byte* ptr, string value)
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
        protected unsafe void internalSetString(char* ptr, string value)
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

            if (handle != IntPtr.Zero)
            {
                if (MemoryType == MemoryType.HGlobal)
                    return ReAlloc(size);
                else
                    return false;
            }

            long l = Size;
            bool al;

            if (hHeap == null || (IntPtr)hHeap == IntPtr.Zero)
                hHeap = currentHeap;

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
            if (al)
            {
                if (addPressure) GC.AddMemoryPressure(size);
                HasGCPressure = addPressure;

                if (hHeap != null) currentHeap = (IntPtr)hHeap;
                MemoryType = MemoryType.HGlobal;

                Size = (long)Native.HeapSize(currentHeap, 0, handle);
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

            if (handle != IntPtr.Zero) return false;

            if ((alignment == 0) || (alignment & 1) != 0)
                return false;

            if ((handle != IntPtr.Zero))
            {
                if (!Free())
                    return false;
            }

            long l = size + (alignment - 1) + 8;

            if (hHeap == null || (IntPtr)hHeap == IntPtr.Zero)
                hHeap = currentHeap;

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

            HasGCPressure = addPressure;

            MemoryType = MemoryType.Aligned;
            if (hHeap != null) currentHeap = (IntPtr)hHeap;

            Size = size;

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
        public bool AlignedFree()
        {
            if ((handle == IntPtr.Zero))
                return true;

            if (MemoryType != MemoryType.HGlobal && MemoryType != MemoryType.Aligned) return false;

            IntPtr p = (IntPtr)LongAt(-1);
            long l = System.Convert.ToInt64(Native.HeapSize(currentHeap, 0, p));

            if (Native.HeapFree(currentHeap, 0, p))
            {
                if (HasGCPressure) GC.RemoveMemoryPressure(l);

                handle = IntPtr.Zero;

                HasGCPressure = false;
                currentHeap = procHeap;
                MemoryType = MemoryType.Invalid;
                Size = 0;

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
        public bool ReAlloc(long size)
        {
            if (MemoryType != MemoryType.HGlobal && MemoryType != MemoryType.Aligned) return false;

            long l = Size;
            bool ra;

            
            // While the function doesn't need to call HeapReAlloc, it hasn't necessarily failed, either.
            if (size == l) return true;

            if (l <= 0)
            {
                // we don't have a pointer yet, so we have to call alloc instead.
                return Alloc(size);
            }

            handle = Native.HeapReAlloc(currentHeap, 8, handle, new IntPtr(size));
            ra = handle != IntPtr.Zero;

            // see if we need to tell the garbage collector anything.
            if (ra && HasGCPressure)
            {
                if (size < l)
                    GC.RemoveMemoryPressure(l - size);
                else
                    GC.AddMemoryPressure(size - l);

            }

            Size = size;
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
        public bool Free()
        {
            long l = 0;

            // While the function doesn't need to call HeapFree, it hasn't necessarily failed, either.
            if (handle == IntPtr.Zero)
                return true;
            else
            {
                // see if we need to tell the garbage collector anything.
                if (HasGCPressure) l = Size;

                var res = Native.HeapFree(currentHeap, 0, handle);

                // see if we need to tell the garbage collector anything.
                if (res)
                {
                    handle = IntPtr.Zero;
                    MemoryType = MemoryType.Invalid;

                    currentHeap = procHeap;

                    if (HasGCPressure) GC.RemoveMemoryPressure(l);

                    MemoryType = MemoryType.Invalid;
                    HasGCPressure = false;

                    Size = 0;

                    currentHeap = procHeap;
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
            if (handle == IntPtr.Zero || (MemoryType != MemoryType.HGlobal) && (MemoryType != MemoryType.Aligned))
            {
                return false;
            }

            return Native.HeapValidate(currentHeap, 0, handle);
        }

        /// <summary>
        /// Frees a previously allocated block of memory on the task heap with LocalFree()
        /// </summary>
        /// <returns>True if successful. If False, call GetLastError or FormatLastError to find out more information.</returns>
        /// <remarks></remarks>
        public bool LocalFree()
        {
            if (handle == IntPtr.Zero)
                return true;
            else
            {
                Native.LocalFree(handle);

                handle = IntPtr.Zero;
                MemoryType = MemoryType.Invalid;
                HasGCPressure = false;
                Size = 0;

                return true;
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
                Native.GlobalFree(handle);

                handle = IntPtr.Zero;
                MemoryType = MemoryType.Invalid;
                HasGCPressure = false;
               
                Size = 0;

                return true;
            }
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
        public bool NetAlloc(int size)
        {
            // just ignore an allocated buffer.
            if (handle != IntPtr.Zero)
                return true;

            int r = Native.NetApiBufferAllocate(size, ref handle);
            
            if (r == 0)
            {
                MemoryType = MemoryType.Network;
                Size = size;
                return true;
            }
            else
            {
                return false;
            }

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
            MemoryType = MemoryType.Invalid;
            handle = IntPtr.Zero;
            Size = 0;
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

            Size = VirtualLength();

            if (va && addPressure)
                GC.AddMemoryPressure(Size);
            
            HasGCPressure = addPressure;

            return va;
        }

        /// <summary>
        /// Frees a region of memory previously allocated with VirtualAlloc.
        /// </summary>
        /// <param name="removePressure">Whether to call GC.RemoveMemoryPressure</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool VirtualFree()
        {
            long l = 0;
            bool vf;

            // While the function doesn't need to call vf, it hasn't necessarily failed, either.
            if (handle == IntPtr.Zero)
                vf = true;
            else
            {
                // see if we need to tell the garbage collector anything.
                if (HasGCPressure)
                    l = VirtualLength();

                vf = Native.VirtualFree(handle);

                // see if we need to tell the garbage collector anything.
                if (vf)
                {
                    handle = IntPtr.Zero;
                    if (HasGCPressure)
                        GC.RemoveMemoryPressure(l);

                    HasGCPressure = false;
                    MemoryType = MemoryType.Invalid;

                    currentHeap = procHeap;
                    Size = 0;

                }
            }

            return vf;
        }

        /// <summary>
        /// Returns the size of a region of virtual memory previously allocated with VirtualAlloc.
        /// </summary>
        /// <returns>The size of a virtual memory region or zero.</returns>
        /// <remarks></remarks>
        private long VirtualLength()
        {
            if (handle == IntPtr.Zero)
                return 0;

            MEMORY_BASIC_INFORMATION m = new MEMORY_BASIC_INFORMATION();

            if (Native.VirtualQuery(handle, ref m, (IntPtr)Marshal.SizeOf(m)) != IntPtr.Zero)
                return System.Convert.ToInt64(m.RegionSize);

            return 0;
        }

        public void FreeCoTaskMem()
        {
            Size = 0;
            currentHeap = procHeap;
            HasGCPressure = false;
            Marshal.FreeCoTaskMem(handle);
            handle = IntPtr.Zero;
        }

        public void AllocCoTaskMem(int size)
        {
            handle = Marshal.AllocCoTaskMem(size);
            if (handle != IntPtr.Zero)
            {
                Size = size;
                MemoryType = MemoryType.CoTaskMem;
                currentHeap = procHeap;
                HasGCPressure = false;
            }
        }

        private void TAlloc(long size)
        {
            switch (MemoryType)
            {
                case MemoryType.HGlobal:
                    Alloc(size, (size > 1024));
                    return;

                case MemoryType.Aligned:
                    AlignedAlloc(size, default, (size > 1024));
                    return;

                case MemoryType.CoTaskMem:

                    if ((size & 0x7fff_ffff_0000_0000L) != 0L) throw new ArgumentOutOfRangeException(nameof(size), "Size is too big for memory type.");
                    AllocCoTaskMem((int)size);
                    return;

                case MemoryType.Virtual:
                    VirtualAlloc(size, (size > 1024));
                    return;

                case MemoryType.Network:
                    if ((size & 0x7fff_ffff_0000_0000L) != 0L) throw new ArgumentOutOfRangeException(nameof(size), "Size is too big for memory type.");
                    NetAlloc((int)size);
                    return;

                default:
                    Alloc(size, (size > 1024));
                    return;
            }
        }

        private void TFree()
        {
            switch (MemoryType)
            {
                case MemoryType.HGlobal:
                    Free();
                    return;

                case MemoryType.Aligned:
                    AlignedFree();
                    return;

                case MemoryType.CoTaskMem:
                    FreeCoTaskMem();
                    return;

                case MemoryType.Virtual:
                    VirtualFree();
                    return;

                case MemoryType.Network:
                    NetFree();
                    return;

                default:
                    Free();
                    return;
            }
        }


        protected override bool ReleaseHandle()
        {
            TFree();
            return true;
        }

        public override string ToString()
        {
            if (handle == (IntPtr)0) return "";
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
        public static explicit operator string(SafePtr val)
        {
            if (val?.handle == (IntPtr)0) return null;
            return val.GetString(0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator SafePtr(string val)
        {
            var op = new SafePtr((val.Length + 1) * sizeof(char));
            op.SetString(0, val);
            return op;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(IntPtr val1, SafePtr val2)
        {
            return (val1 == val2?.handle);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(IntPtr val1, SafePtr val2)
        {
            return (val1 != val2?.handle);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(SafePtr val2, IntPtr val1)
        {
            return (val1 == val2?.handle);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(SafePtr val2, IntPtr val1)
        {
            return (val1 != val2?.handle);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator IntPtr(SafePtr val)
        {
            return val?.handle ?? IntPtr.Zero;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator SafePtr(IntPtr val)
        {
            return new SafePtr(val);
        }

    }
}
