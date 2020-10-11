// ' ************************************************* ''
// ' DataTools Visual Basic Utility Library 
// '
// ' Module: SafePtr 
// '         Exhaustive in-place replacement
// '         for IntPtr, safe version.
// ' 
// ' Copyright (C) Nathan Moschkin
// ' All Rights Reserved
// '
// ' Licensed Under the Microsoft Public License   
// ' ************************************************* ''

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using DataTools.BitStream;
using DataTools.Memory.Internal;
using Microsoft.VisualBasic.CompilerServices;

namespace DataTools.Memory
{

    /// <summary>
    /// The SafePtr class.  Drop-in replacement for IntPtr.
    /// Use anywhere you use IntPtr.
    /// 
    /// Inherits from SafeHandle ... will self-dispose, if necessary.
    /// </summary>
    /// <remarks></remarks>
    public class SafePtr : SafeHandle, IDisposable, ICloneable, IEnumerable<byte>, IEnumerable<char>, IEquatable<SafePtr>, IEquatable<IntPtr>, IEquatable<MemPtr>





    {

        // ' A List Key/Value pairs of Integral types and their atomic sizes (in bytes).
        protected static List<KeyValuePair<Type, int>> _primitiveCache = new List<KeyValuePair<Type, int>>();
        protected IntPtr hHeap = Native.GetProcessHeap();
        protected MemAllocType _MemType = MemAllocType.Invalid;
        protected NativeInt _buffer;

        internal new IntPtr handle
        {
            get
            {
                return base.handle;
            }

            set
            {
                base.handle = value;
            }
        }


        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Copies memory from this memory pointer into the pointer specified.
        /// </summary>
        /// <param name="ptr">The pointer to which to copy the memory.</param>
        /// <param name="size">The size of the buffer to copy.</param>
        /// <remarks></remarks>
        public void CopyTo(IntPtr ptr, IntPtr size)
        {
            if (size.ToInt64() <= uint.MaxValue)
            {
                Native.MemCpy(ptr, handle, (uint)size);
            }
            else
            {
                Native.CopyMemory(ptr, handle, size);
            }
        }

        /// <summary>
        /// Copies memory from another memory pointer into this one.
        /// If this one is not yet allocated, it will automatically be allocated
        /// to the size specified.
        /// </summary>
        /// <param name="ptr">The pointer from which to copy the memory.</param>
        /// <param name="size">The size of the buffer to copy.</param>
        /// <remarks></remarks>
        public void CopyFrom(IntPtr ptr, IntPtr size)
        {
            if (handle != IntPtr.Zero || Alloc(size.ToInt64()))
            {
                if (size.ToInt64() <= uint.MaxValue)
                {
                    Native.MemCpy(handle, ptr, (uint)size);
                }
                else
                {
                    Native.CopyMemory(handle, ptr, size);
                }
            }
        }

        /// <summary>
        /// Copies one memory location to another.
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="src"></param>
        /// <param name="length"></param>
        /// <remarks></remarks>
        public static void CopyMemory(IntPtr dest, IntPtr src, int length)
        {
            Native.MemCpy(dest, src, (uint)length);
        }


        /// <summary>
        /// Copies one memory location to another, long version.
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="src"></param>
        /// <param name="length"></param>
        /// <remarks></remarks>
        public static void CopyMemoryLong(IntPtr dest, IntPtr src, long length)
        {
            Native.CopyMemory(dest, src, (IntPtr)length);
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Returns a string from a pointer stored at a memory location in this object's pointer.
        /// </summary>
        /// <param name="index"></param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string get_StringIndirectAt(IntPtr index)
        {
            return GrabStringFromPointerAt(index);
        }

        public void set_StringIndirectAt(IntPtr index, string value)
        {
            SetStringAtPointerIndex(index, value);
        }

        /// <summary>
        /// Returns the length of a null-terminated Unicode string at the specified byteIndex.
        /// </summary>
        /// <param name="byteIndex"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public int StrLen(IntPtr byteIndex)
        {
            int StrLenRet = default;
            StrLenRet = Native.CharZero(byteIndex + (int)handle);
            return StrLenRet;
        }

        /// <summary>
        /// Grabs a null-terminated Unicode string from a position relative to the memory pointer.
        /// </summary>
        /// <param name="byteIndex">A 32 or 64-bit number indicating the starting byte position relative to the pointer.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public string GrabString(IntPtr byteIndex)
        {
            return null;
        }

        /// <summary>
        /// Grabs a null-terminated Unicode string from a pointer at a the specified position relative to the memory pointer.
        /// </summary>
        /// <param name="byteIndex">A 32 or 64-bit number indicating the starting byte position relative to the pointer.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public string GrabStringFromPointerAtAbsolute(IntPtr byteIndex)
        {
            return null;
        }

        /// <summary>
        /// Grabs a null-terminated Unicode string from a pointer at a the specified position, in an array of pointers, relative to the memory pointer.
        /// </summary>
        /// <param name="index">A 32 or 64-bit number indicating the starting pointer collection position relative to the pointer.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public string GrabStringFromPointerAt(IntPtr index)
        {
            return null;
        }

        /// <summary>
        /// Grabs a null-terminated Unicode string from a position relative to the memory pointer with the exact specified length.
        /// No null-termination checking is performed.
        /// </summary>
        /// <param name="byteIndex">A 32 or 64-bit number indicating the starting byte position relative to the pointer.</param>
        /// <param name="length">The length of the string, in characters.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string GrabString(IntPtr byteIndex, int length)
        {
            string GrabStringRet = default;
            if (handle == IntPtr.Zero)
                return null;
            if (length <= 0)
                throw new IndexOutOfRangeException("length must be greater than zero");
            GrabStringRet = new string('\0', length);
            QuickCopyObject<string>(ref GrabStringRet, new IntPtr((long)handle + byteIndex.ToInt64()), Conversions.ToUInteger(length << 1));
            return GrabStringRet;
        }

        /// <summary>
        /// Grabs a null-terminated ASCII string from a position relative to the memory pointer.
        /// </summary>
        /// <param name="byteIndex">A 32 or 64-bit number indicating the starting position relative to the pointer.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string GrabAsciiString(IntPtr byteIndex)
        {
            string GrabAsciiStringRet = default;
            var tp = new IntPtr((long)handle + byteIndex.ToInt64());
            int e = Native.ByteZero(tp);
            byte[] ba;
            if (e == 0)
                return "";
            ba = new byte[e];
            Native.byteArrAtget(ba, new IntPtr((long)handle + (long)byteIndex), (uint)e);
            GrabAsciiStringRet = System.Text.Encoding.ASCII.GetString(ba);
            return GrabAsciiStringRet;
        }

        /// <summary>
        /// Grabs a null-terminated ASCII string from a position relative to the memory pointer with the exact specified length.
        /// No null-termination checking is performed.
        /// </summary>
        /// <param name="byteIndex">A 32 or 64-bit number indicating the starting byte position relative to the pointer.</param>
        /// <param name="length">The length of the string, in characters.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string GrabAsciiString(IntPtr byteIndex, int length)
        {
            string GrabAsciiStringRet = default;
            if (handle == IntPtr.Zero)
                return null;
            if (length <= 0)
                throw new IndexOutOfRangeException("length must be greater than zero");
            byte[] ba;
            ba = new byte[length];
            Native.byteArrAtget(ba, new IntPtr((long)handle + (long)byteIndex), (uint)length);
            GrabAsciiStringRet = System.Text.Encoding.ASCII.GetString(ba);
            return GrabAsciiStringRet;
        }

        /// <summary>
        /// Grabs a null-terminated UTF-8 string from a position relative to the memory pointer.
        /// </summary>
        /// <param name="byteIndex">A 32 or 64-bit number indicating the starting position relative to the pointer.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public string GrabUtf8String(IntPtr byteIndex)
        {
            return null;
        }

        /// <summary>
        /// Grabs a null-terminated UTF8 string from a position relative to the memory pointer with the exact specified length.
        /// No null-termination checking is performed.
        /// </summary>
        /// <param name="byteIndex">A 32 or 64-bit number indicating the starting byte position relative to the pointer.</param>
        /// <param name="length">The length of the string, in characters.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string GrabUTF8String(IntPtr byteIndex, int length)
        {
            string GrabUTF8StringRet = default;
            GrabUTF8StringRet = System.Text.Encoding.UTF8.GetString(GrabBytes(byteIndex, length));
            return GrabUTF8StringRet;
        }


        /// <summary>
        /// Grabs a null-terminated Unicode string array (MULTISZ) from a position relative to the memory pointer.
        /// </summary>
        /// <param name="byteIndex">A 32 or 64-bit number indicating the starting position relative to the pointer.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string[] GrabStringArray(IntPtr byteIndex)
        {
            if (handle == IntPtr.Zero)
                return null;
            char b;
            int i = 0;
            long sb = (long)byteIndex;
            string[] sout = null;
            int ct = 0;
            long l = (long)byteIndex;
            var tp = new IntPtr(l + (long)handle);
            b = get_CharAtAbsolute(l);
            while (b != '\0')
            {
                i = Native.CharZero(tp);
                Array.Resize(ref sout, ct + 1);
                sout[ct] = new string('\0', i);
                QuickCopyObject<string>(ref sout[ct], tp, (uint)(i << 1));
                l += (i << 1) + 2;
                tp = tp + ((i << 1) + 2);
                b = get_CharAtAbsolute(l);
                ct += 1;
            }

            return sout;
        }

        /// <summary>
        /// Sets the memory at the specified byte index to the specified string using the optional specified encoding.
        /// A null termination character is appended to the string before the encoding conversion.
        /// </summary>
        /// <param name="byteIndex"></param>
        /// <param name="s"></param>
        /// <param name="enc">Optional System.Text.Encoding object (default is Windows Unicode = UTF16LE / wchar_t).</param>
        /// <remarks></remarks>
        public void SetString(IntPtr byteIndex, string s, System.Text.Encoding enc)
        {
            if (enc is null)
                enc = System.Text.Encoding.Unicode;
            var p = new IntPtr((long)handle + byteIndex.ToInt64());
            var b = enc.GetBytes(s + '\0');
            Native.byteArrAtset(p, b, (uint)b.Length);
        }

        /// <summary>
        /// Sets the memory at the specified byte index to the specified string.
        /// A null termination character is appended to the string.
        /// </summary>
        /// <param name="byteIndex"></param>
        /// <param name="s"></param>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public void SetString(IntPtr byteIndex, string s)
        {
        }

        /// <summary>
        /// Sets a buffer referenced by the memory at the specified byte index to the specified string.
        /// A null termination character is appended to the string.
        /// </summary>
        /// <param name="byteIndex">The absolute position in the buffer.</param>
        /// <param name="s">The string to set.</param>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public void SetStringAtPointer(IntPtr byteIndex, string s)
        {
        }

        /// <summary>
        /// Sets a buffer referenced by the memory at the specified handle index to the specified string.
        /// A null termination character is appended to the string.
        /// </summary>
        /// <param name="index">The handle index.</param>
        /// <param name="s">The string to set.</param>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public void SetStringAtPointerIndex(IntPtr index, string s)
        {
        }


        /// <summary>
        /// Sets the memory at the specified index to the specified byte array.
        /// </summary>
        /// <param name="byteIndex"></param>
        /// <param name="data"></param>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public virtual void SetBytes(IntPtr byteIndex, byte[] data)
        {
        }


        /// <summary>
        /// Sets the memory at the specified index to the specified byte array for the specified length.
        /// </summary>
        /// <param name="byteIndex"></param>
        /// <param name="data"></param>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public virtual void SetBytes(IntPtr byteIndex, byte[] data, int length)
        {
        }

        /// <summary>
        /// Get an array of bytes at the specified position of the specified length.
        /// </summary>
        /// <param name="byteIndex">A 32 or 64-bit number indicating the starting position relative to the pointer.</param>
        /// <param name="length">The number of bytes to grab.</param>
        /// <returns>A new byte array with the requested data.</returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public virtual byte[] GrabBytes(IntPtr byteIndex, int length)
        {
            return null;
        }

        public virtual byte[] GrabBytes()
        {
            return GrabBytes((IntPtr)0, (int)Length);
        }

        /// <summary>
        /// Sets the memory at the specified index to the specified sbyte array.
        /// </summary>
        /// <param name="byteIndex"></param>
        /// <param name="data"></param>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public virtual void SetSBytes(IntPtr byteIndex, byte[] data)
        {
        }

        /// <summary>
        /// Get an array of sbytes at the specified position of the specified length.
        /// </summary>
        /// <param name="byteIndex">A 32 or 64-bit number indicating the starting position relative to the pointer.</param>
        /// <param name="length">The number of bytes to grab.</param>
        /// <returns>A new byte array with the requested data.</returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public virtual byte[] GrabSBytes(IntPtr byteIndex, int length)
        {
            return null;
        }


        /// <summary>
        /// Get an array of bytes at the specified position of the specified length.
        /// </summary>
        /// <param name="byteIndex">A 32 or 64-bit number indicating the starting position relative to the pointer.</param>
        /// <param name="length">The number of bytes to grab.</param>
        /// <param name="data">
        /// The data buffer into which the memory will be copied.
        /// If this value is Nothing or the size of the buffer is too small, then a new buffer will be allocated.
        /// </param>
        /// <remarks></remarks>
        public virtual void GrabBytes(IntPtr byteIndex, int length, ref byte[] data)
        {
            if (handle == IntPtr.Zero)
                return;
            if (data is null)
            {
                data = new byte[length];
            }
            else if (data.Length < length)
            {
                data = new byte[length];
            }

            byteGet(byteIndex, length, ref data);
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        protected void byteGet(IntPtr byteIndex, int length, ref byte[] data)
        {
        }

        /// <summary>
        /// Get an array of bytes at the specified position of the specified length.
        /// </summary>
        /// <param name="byteIndex">A 32 or 64-bit number indicating the starting position relative to the pointer.</param>
        /// <param name="length">The number of bytes to grab.</param>
        /// <param name="data">
        /// The data buffer into which the memory will be copied.
        /// If this value is Nothing or the size of the buffer is too small, then the method will fail.
        /// </param>
        /// <param name="arrayIndex">The position in the buffer at which to begin copying.</param>
        /// <remarks></remarks>
        public virtual void GrabBytes(IntPtr byteIndex, int length, ref byte[] data, int arrayIndex)
        {
            if (handle == IntPtr.Zero)
                return;
            if (data is null)
            {
                throw new ArgumentNullException("data cannot be null or Nothing.");
            }
            else if (length + arrayIndex > data.Length)
            {
                throw new ArgumentOutOfRangeException("data buffer length is too small.");
            }

            var gh = GCHandle.Alloc(data, GCHandleType.Pinned);
            IntPtr pdest = (IntPtr)((long)gh.AddrOfPinnedObject() + arrayIndex);
            Native.MemCpy(pdest, new IntPtr((long)handle + (long)byteIndex), (uint)length);
            gh.Free();
        }

        /// <summary>
        /// Returns the results of the buffer as if it were a BSTR type String.
        /// </summary>
        /// <param name="comPtr">Specifies whether or not the current MemPtr is an actual COM pointer to a BSTR.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public virtual string BSTR(bool comPtr = true)
        {
            string BSTRRet = default;
            var d = default(int);
            string s;
            var p = comPtr ? (IntPtr)((long)handle - 4L) : handle;
            Native.intAtget(ref d, p);
            s = new string('\0', d);
            QuickCopyObject<string>(ref s, (IntPtr)((long)p + 4L), (uint)(d << 1));
            BSTRRet = s;
            return BSTRRet;
        }

        /// <summary>
        /// Returns the contents of this buffer as a string.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public virtual string LpwStr()
        {
            string LpwStrRet = default;
            LpwStrRet = GrabString(IntPtr.Zero);
            return LpwStrRet;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Gets or sets a value indicating the kind of memory to allocate.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public virtual MemAllocType MemoryType
        {
            get
            {
                MemAllocType MemoryTypeRet = default;
                if (IsInvalid)
                    return MemAllocType.Invalid;
                MemoryTypeRet = _MemType;
                return MemoryTypeRet;
            }

            set
            {
                if (!IsInvalid)
                    throw new AccessViolationException("Cannot change memory type on an allocated object.  Free your object, first.");
                _MemType = value;
            }
        }

        public virtual bool IsString { get; set; }

        /// <summary>
        /// Gets or sets the length of the simple buffer.
        /// If the buffer is already allocated in a certain mode,
        /// that mode is retained.  Newly allocated memory is automatically
        /// zeroed-out.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public virtual long Length
        {
            get
            {
                return _buffer;
            }

            set
            {
                Alloc(value);
            }
        }


        /// <summary>
        /// Sets the length of the memory block.
        /// </summary>
        /// <param name="value"></param>
        /// <remarks></remarks>
        public virtual void SetLength(long value)
        {
            if (hHeap == (IntPtr)0)
                hHeap = Native.GetProcessHeap();
            Alloc(value);
        }

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Calculate the CRC 32 for the block of memory represented by this structure.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public uint CalculateCrc32()
        {
            uint CalculateCrc32Ret = default;
            var l = new IntPtr(Length);
            CalculateCrc32Ret = Crc32.Calculate(handle, l);
            return CalculateCrc32Ret;
        }

        /// <summary>
        /// Calculate the CRC 32 for the block of memory represented by this structure.
        /// </summary>
        /// <param name="bufflen">The length, in bytes, of the marshaling buffer.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public uint CalculateCrc32(int bufflen)
        {
            uint CalculateCrc32Ret = default;
            var l = new IntPtr(Length);
            CalculateCrc32Ret = Crc32.Calculate(handle, l, bufflen: bufflen);
            return CalculateCrc32Ret;
        }

        /// <summary>
        /// Calculate the CRC 32 for the block of memory represented by this structure.
        /// </summary>
        /// <param name="length">The length, in bytes, of the buffer to check.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public uint CalculateCrc32(IntPtr length)
        {
            uint CalculateCrc32Ret = default;
            CalculateCrc32Ret = Crc32.Calculate(handle, length);
            return CalculateCrc32Ret;
        }

        /// <summary>
        /// Calculate the CRC 32 for the block of memory represented by this structure.
        /// </summary>
        /// <param name="length">The length, in bytes, of the buffer to check.</param>
        /// <param name="bufflen">The length, in bytes, of the marshaling buffer.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public uint CalculateCrc32(IntPtr length, int bufflen)
        {
            uint CalculateCrc32Ret = default;
            CalculateCrc32Ret = Crc32.Calculate(handle, length, bufflen: bufflen);
            return CalculateCrc32Ret;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Converts the contents of an unmanaged pointer into a structure.
        /// </summary>
        /// <typeparam name="T">The type of structure requested.</typeparam>
        /// <returns>New instance of T.</returns>
        /// <remarks></remarks>
        public virtual T ToStruct<T>() where T : struct
        {
            T ToStructRet = default;
            ToStructRet = (T)Marshal.PtrToStructure(handle, typeof(T));
            return ToStructRet;
        }

        /// <summary>
        /// Sets the contents of a structure into an unmanaged pointer.
        /// </summary>
        /// <typeparam name="T">The type of structure to set.</typeparam>
        /// <param name="val">The structure to set.</param>
        /// <remarks></remarks>
        public virtual void FromStruct<T>(T val) where T : struct
        {
            int cb = Marshal.SizeOf(val);
            if (handle == IntPtr.Zero)
                Alloc(cb);
            Marshal.StructureToPtr(val, handle, false);
        }

        /// <summary>
        /// Converts the contents of an unmanaged pointer at the specified byte index into a structure.
        /// </summary>
        /// <typeparam name="T">The type of structure requested.</typeparam>
        /// <param name="byteIndex">The byte index relative to the pointer at which to begin copying.</param>
        /// <returns>New instance of T.</returns>
        /// <remarks></remarks>
        public virtual T ToStructAt<T>(IntPtr byteIndex) where T : struct
        {
            T ToStructAtRet = default;
            ToStructAtRet = (T)Marshal.PtrToStructure((IntPtr)((long)handle + (long)byteIndex), typeof(T));
            return ToStructAtRet;
        }

        /// <summary>
        /// Sets the contents of a structure into a memory buffer at the specified byte index.
        /// </summary>
        /// <typeparam name="T">The type of structure to set.</typeparam>
        /// <param name="byteIndex">The byte index relative to the pointer at which to begin copying.</param>
        /// <param name="val">The structure to set.</param>
        /// <remarks></remarks>
        public virtual void FromStructAt<T>(IntPtr byteIndex, T val) where T : struct
        {
            int cb = Marshal.SizeOf(val);
            Marshal.StructureToPtr(val, (IntPtr)((long)handle + (long)byteIndex), false);
        }

        /// <summary>
        /// Copies the contents of the buffer at the specified index into a blittable structure array.
        /// </summary>
        /// <typeparam name="T">The structure type.</typeparam>
        /// <param name="byteIndex">The index at which to begin copying.</param>
        /// <returns>An array of T.</returns>
        /// <remarks></remarks>
        public virtual T[] ToBlittableStructArrayAt<T>(IntPtr byteIndex) where T : struct
        {
            if (handle == IntPtr.Zero)
                return null;
            long l = Length - Conversions.ToLong(byteIndex);
            int cb = Marshal.SizeOf(new T());
            int c = (int)(l / (double)cb);
            T[] tt;
            tt = new T[c];
            var gh = GCHandle.Alloc(tt, GCHandleType.Pinned);
            if (l <= uint.MaxValue)
            {
                Native.MemCpy(gh.AddrOfPinnedObject(), handle, (uint)l);
            }
            else
            {
                CopyMemory(gh.AddrOfPinnedObject(), handle, (int)l);
            }

            gh.Free();
            return tt;
        }

        /// <summary>
        /// Copies a blittable structure array into the buffer at the specified index, initializing a new buffer, if necessary.
        /// </summary>
        /// <typeparam name="T">The structure type.</typeparam>
        /// <param name="byteIndex">The index at which to begin copying.</param>
        /// <param name="value">The structure array to copy.</param>
        /// <remarks></remarks>
        public virtual void FromBlittableStructArrayAt<T>(IntPtr byteIndex, T[] value) where T : struct
        {
            if (handle == IntPtr.Zero && byteIndex != IntPtr.Zero)
                return;
            long l;
            int cb = Marshal.SizeOf(new T());
            int c = value.Count();
            l = c * cb;
            if (handle == IntPtr.Zero)
            {
                if (!Alloc(l))
                    return;
            }

            IntPtr p = (IntPtr)((long)handle + (long)byteIndex);
            var gh = GCHandle.Alloc(value, GCHandleType.Pinned);
            if (l <= uint.MaxValue)
            {
                Native.MemCpy(p, gh.AddrOfPinnedObject(), (uint)l);
            }
            else
            {
                CopyMemory(p, gh.AddrOfPinnedObject(), (int)l);
            }

            gh.Free();
        }

        /// <summary>
        /// Copies the contents of the buffer into a blittable structure array.
        /// </summary>
        /// <typeparam name="T">The structure type.</typeparam>
        /// <returns>An array of T.</returns>
        /// <remarks></remarks>
        public virtual T[] ToBlittableStructArray<T>() where T : struct
        {
            return ToBlittableStructArrayAt<T>(IntPtr.Zero);
        }

        /// <summary>
        /// Copies a blittable structure array into the buffer, initializing a new buffer, if necessary.
        /// </summary>
        /// <typeparam name="T">The structure type.</typeparam>
        /// <param name="value">The structure array to copy.</param>
        /// <remarks></remarks>
        public virtual void FromBlittableStructArray<T>(T[] value) where T : struct
        {
            FromBlittableStructArrayAt(IntPtr.Zero, value);
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Retrieves or sets an individual GUID structure at the specified absolute byte index in the buffer.
        /// </summary>
        /// <param name="index">The position to return.</param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public Guid get_GuidAtAbsolute(long index)
        {
            return Guid.Empty;
            // If handle = IntPtr.Zero Then Return Guid.Empty
            // guidAtget(GuidAtAbsolute, CType(CLng(handle) + index, IntPtr))
            // If handle = IntPtr.Zero Then Return
            // guidAtset(CType(CLng(handle) + index, IntPtr), value)
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public void set_GuidAtAbsolute(long index, Guid value)
        {
        }


        /// <summary>
        /// Retrieves or sets an individual GUID structure at the specified index in the buffer treated as an array of GUIDs.
        /// </summary>
        /// <param name="index">The position to return.</param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public Guid get_GuidAt(long index)
        {
            return Guid.Empty;
            // If handle = IntPtr.Zero Then Return Guid.Empty
            // guidAtget(GuidAt, CType(CLng(handle) + (index * 16), IntPtr))
            // If handle = IntPtr.Zero Then Return
            // guidAtset(CType(CLng(handle) + (index * 16), IntPtr), value)
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public void set_GuidAt(long index, Guid value)
        {
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        // ' Integral types accessable via logical indexer.  
        // ' A logical index means if you were treat a block
        // ' of memory as an array of the requested type,
        // ' then the result you get will be the element
        // ' at the logical position in that array.
        // ' So a character at logical index 1 has a byte offset of 2, 
        // ' for an integer at index 1 the byte offset is 4, etc...
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Retrieves or sets an individual byte at the specified logical index in the buffer.
        /// </summary>
        /// <param name="index">The index to return.</param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte this[long index]
        {
            [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
            get
            {
                return 0;
                // byteAtget(ByteAt, New IntPtr(clng(handle) + index))
            }

            [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
            set
            {
                // byteAtset(New IntPtr(clng(handle) + index), value)
            }
        }

        /// <summary>
        /// Retrieves or sets an individual signed byte at the specified logical index in the buffer.
        /// </summary>
        /// <param name="index">The index to return.</param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public sbyte get_SByteAt(long index)
        {
            return 0;
            // sbyteAtget(SByteAt, New IntPtr(clng(handle) + index))
            // sbyteAtset(New IntPtr(clng(handle) + index), value)
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public void set_SByteAt(long index, sbyte value)
        {
        }

        /// <summary>
        /// Retrieves or sets an individual Unicode character at the specified logical index in the buffer.
        /// </summary>
        /// <param name="index">The index to return.</param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public char get_CharAt(long index)
        {
            return '\0';
            // charAtget(CharAt, New IntPtr(clng(handle) + (index * 2)))
            // charAtset(New IntPtr(clng(handle) + (index * 2)), value)
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public void set_CharAt(long index, char value)
        {
        }

        /// <summary>
        /// Retrieves or sets an individual Short at the specified logical index in the buffer.
        /// </summary>
        /// <param name="index">The index to return.</param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public short get_ShortAt(long index)
        {
            return 0;
            // shortAtget(ShortAt, New IntPtr(clng(handle) + (index * 2)))
            // shortAtset(New IntPtr(clng(handle) + (index * 2)), value)
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public void set_ShortAt(long index, short value)
        {
        }

        /// <summary>
        /// Retrieves or sets an individual UShort at the specified logical index in the buffer.
        /// </summary>
        /// <param name="index">The index to return.</param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public ushort get_UShortAt(long index)
        {
            return 0;
            // ushortAtget(UShortAt, New IntPtr(clng(handle) + (index * 2)))
            // ushortAtset(New IntPtr(clng(handle) + (index * 2)), value)
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public void set_UShortAt(long index, ushort value)
        {
        }

        /// <summary>
        /// Retrieves or sets an individual Integer at the specified logical index in the buffer.
        /// </summary>
        /// <param name="index">The index to return.</param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public int get_IntegerAt(long index)
        {
            return 0;
            // intAtget(IntegerAt, New IntPtr(clng(handle) + (index * 4)))
            // intAtset(New IntPtr(clng(handle) + (index * 4)), value)
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public void set_IntegerAt(long index, int value)
        {
        }

        /// <summary>
        /// Retrieves or sets an individual UInteger at the specified logical index in the buffer.
        /// </summary>
        /// <param name="index">The index to return.</param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public uint get_UIntegerAt(long index)
        {
            return 0U;
            // uintAtget(UIntegerAt, New IntPtr(clng(handle) + (index * 4)))
            // uintAtset(New IntPtr(clng(handle) + (index * 4)), value)
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public void set_UIntegerAt(long index, uint value)
        {
        }


        /// <summary>
        /// Retrieves or sets an individual Long at the specified logical index in the buffer.
        /// </summary>
        /// <param name="index">The index to return.</param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public long get_LongAt(long index)
        {
            return 0L;
            // longAtget(LongAt, New IntPtr(clng(handle) + (index * 8)))
            // longAtset(New IntPtr(clng(handle) + (index * 8)), value)
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public void set_LongAt(long index, long value)
        {
        }

        /// <summary>
        /// Retrieves or sets an individual ULong at the specified logical index in the buffer.
        /// </summary>
        /// <param name="index">The index to return.</param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public ulong get_ULongAt(long index)
        {
            return 0UL;
            // ulongAtget(ULongAt, New IntPtr(clng(handle) + (index * 8)))
            // ulongAtset(New IntPtr(clng(handle) + (index * 8)), value)
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public void set_ULongAt(long index, ulong value)
        {
        }

        /// <summary>
        /// Retrieves or sets an individual Single at the specified logical index in the buffer.
        /// </summary>
        /// <param name="index">The index to return.</param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public float get_SingleAt(long index)
        {
            return 0f;
            // singleAtget(SingleAt, New IntPtr(CLng(handle) + (index * 4)))
            // singleAtset(New IntPtr(CLng(handle) + (index * 4)), value)
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public void set_SingleAt(long index, float value)
        {
        }

        /// <summary>
        /// Retrieves or sets an individual Double at the specified logical index in the buffer.
        /// </summary>
        /// <param name="index">The index to return.</param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public double get_DoubleAt(long index)
        {
            return 0d;
            // doubleAtget(DoubleAt, New IntPtr(CLng(handle) + (index * 8)))
            // doubleAtset(New IntPtr(CLng(handle) + (index * 8)), value)
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public void set_DoubleAt(long index, double value)
        {
        }

        /// <summary>
        /// Retrieves or sets an individual Decimal at the specified logical index in the buffer.
        /// </summary>
        /// <param name="index">The index to return.</param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public decimal get_DecimalAt(long index)
        {
            return 0m;
            // decimalAtget(DecimalAt, New IntPtr(CLng(handle) + (index * 16)))
            // decimalAtset(New IntPtr(CLng(handle) + (index * 16)), value)
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public void set_DecimalAt(long index, decimal value)
        {
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        // ' Integral types accessable via absolute byte position.
        // ' The value returned is the desired integral value
        // ' at the specified absolute byte position
        // ' in the buffer.
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Retrieves or sets an individual Unicode character at the specified byte position in the buffer.
        /// </summary>
        /// <param name="index">The byte position to return.</param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public char get_CharAtAbsolute(long index)
        {
            return '\0';
            // charAtget(CharAtAbsolute, New IntPtr(clng(handle) + index))
            // charAtset(New IntPtr(clng(handle) + index), value)
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public void set_CharAtAbsolute(long index, char value)
        {
        }

        /// <summary>
        /// Retrieves or sets an individual Short at the specified byte position in the buffer.
        /// </summary>
        /// <param name="index">The byte position to return.</param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public short get_ShortAtAbsolute(long index)
        {
            return 0;
            // shortAtget(ShortAtAbsolute, New IntPtr(clng(handle) + (index)))
            // shortAtset(New IntPtr(clng(handle) + (index)), value)
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public void set_ShortAtAbsolute(long index, short value)
        {
        }

        /// <summary>
        /// Retrieves or sets an individual UShort at the specified byte position in the buffer.
        /// </summary>
        /// <param name="index">The byte position to return.</param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public ushort get_UShortAtAbsolute(long index)
        {
            return 0;
            // ushortAtget(UShortAtAbsolute, New IntPtr(clng(handle) + (index)))
            // ushortAtset(New IntPtr(clng(handle) + (index)), value)
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public void set_UShortAtAbsolute(long index, ushort value)
        {
        }


        /// <summary>
        /// Retrieves or sets an individual Integer at the specified byte position in the buffer.
        /// </summary>
        /// <param name="index">The byte position to return.</param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public int get_IntegerAtAbsolute(long index)
        {
            return 0;
            // intAtget(IntegerAtAbsolute, New IntPtr(clng(handle) + (index)))
            // intAtset(New IntPtr(clng(handle) + (index)), value)
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public void set_IntegerAtAbsolute(long index, int value)
        {
        }

        /// <summary>
        /// Retrieves or sets an individual UInteger at the specified byte position in the buffer.
        /// </summary>
        /// <param name="index">The byte position to return.</param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public uint get_UIntegerAtAbsolute(long index)
        {
            return 0U;
            // uintAtget(UIntegerAtAbsolute, New IntPtr(clng(handle) + (index)))
            // uintAtset(New IntPtr(clng(handle) + (index)), value)
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public void set_UIntegerAtAbsolute(long index, uint value)
        {
        }


        /// <summary>
        /// Retrieves or sets an individual Long at the specified byte position in the buffer.
        /// </summary>
        /// <param name="index">The byte position to return.</param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public long get_LongAtAbsolute(long index)
        {
            return 0L;
            // longAtget(LongAtAbsolute, New IntPtr(clng(handle) + (index)))
            // longAtset(New IntPtr(clng(handle) + (index)), value)
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public void set_LongAtAbsolute(long index, long value)
        {
        }

        /// <summary>
        /// Retrieves or sets an individual ULong at the specified byte position in the buffer.
        /// </summary>
        /// <param name="index">The byte position to return.</param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public ulong get_ULongAtAbsolute(long index)
        {
            return 0UL;
            // ulongAtget(ULongAtAbsolute, New IntPtr(clng(handle) + (index)))
            // ulongAtset(New IntPtr(clng(handle) + (index)), value)
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public void set_ULongAtAbsolute(long index, ulong value)
        {
        }

        /// <summary>
        /// Retrieves or sets an individual Single at the specified byte position in the buffer.
        /// </summary>
        /// <param name="index">The byte position to return.</param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public float get_SingleAtAbsolute(long index)
        {
            return 0f;
            // singleAtget(SingleAtAbsolute, New IntPtr(CLng(handle) + (index)))
            // singleAtset(New IntPtr(CLng(handle) + (index)), value)
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public void set_SingleAtAbsolute(long index, float value)
        {
        }

        /// <summary>
        /// Retrieves or sets an individual Double at the specified byte position in the buffer.
        /// </summary>
        /// <param name="index">The byte position to return.</param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public double get_DoubleAtAbsolute(long index)
        {
            return 0d;
            // doubleAtget(DoubleAtAbsolute, New IntPtr(CLng(handle) + (index)))
            // doubleAtset(New IntPtr(CLng(handle) + (index)), value)
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public void set_DoubleAtAbsolute(long index, double value)
        {
        }

        /// <summary>
        /// Retrieves or sets an individual Decimal at the specified byte position in the buffer.
        /// </summary>
        /// <param name="index">The byte position to return.</param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public decimal get_DecimalAtAbsolute(long index)
        {
            return 0m;
            // decimalAtget(DecimalAtAbsolute, New IntPtr(CLng(handle) + (index)))
            // decimalAtset(New IntPtr(CLng(handle) + (index)), value)
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public void set_DecimalAtAbsolute(long index, decimal value)
        {
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Reverses the entire memory pointer.
        /// </summary>
        /// <returns>True if successful.</returns>
        /// <remarks></remarks>
        public virtual bool Reverse()
        {
            if (handle == IntPtr.Zero)
                return false;
            long l = Length;
            if (l > int.MaxValue)
                return false;
            byte[] b1;
            byte[] b2;
            int i = 0;
            int c = (int)l - 1;
            int e = c;
            b1 = new byte[c + 1];
            b2 = new byte[c + 1];
            Native.byteArrAtget(b1, handle, (uint)l);
            while (i != c)
            {
                b2[e] = b1[i];
                e -= 1;
                i += 1;
            }

            Native.byteArrAtset(handle, b2, (uint)l);
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
        public virtual void Slide(long index, long length, long offset)
        {
            if (offset == 0L)
                return;
            long hl = Length;
            if (hl <= 0L)
                return;
            if (0L > index + length + offset || index + length + offset > hl)
            {
                throw new IndexOutOfRangeException("Index out of bounds DataTools.Memory.MemPtr.Slide().");
                return;
            }

            IntPtr p1;
            IntPtr p2;
            p1 = (IntPtr)((long)handle + index);
            p2 = (IntPtr)((long)handle + index + offset);
            long a = Math.Abs(offset);
            var m = new MemPtr(length);
            var n = new MemPtr(a);
            Native.MemCpy(m.Handle, p1, (uint)length);
            Native.MemCpy(n.Handle, p2, (uint)a);
            p1 = (IntPtr)((long)handle + index + offset + length);
            Native.MemCpy(p1, n.Handle, (uint)a);
            Native.MemCpy(p2, m.Handle, (uint)length);
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
        public virtual long PullIn(long index, long amount, bool removePressure = false)
        {
            long hl = Length;
            if (Length == 0L || 0L > index || index >= hl - 1L)
            {
                throw new IndexOutOfRangeException("Index out of bounds DataTools.Memory.MemPtr.PullIn().");
                return -1;
            }

            long a = index + amount;
            long b = Length - a;
            Slide(a, b, -amount);
            ReAlloc(hl - amount);
            return Length;
        }

        /// <summary>
        /// Extend the buffer from the specified index.
        /// </summary>
        /// <param name="index">The index where expansion begins. The expansion starts at this position.</param>
        /// <param name="amount">Number of bytes to push out.</param>
        /// <param name="addPressure">Specify whether to notify the garbage collector.</param>
        /// <remarks></remarks>
        public virtual long PushOut(long index, long amount, byte[] bytes = null, bool addPressure = false)
        {
            long PushOutRet = default;
            long hl = Length;
            if (hl <= 0L)
            {
                SetLength(amount);
                PushOutRet = amount;
                return PushOutRet;
            }

            if (0L > index || index > hl - 1L)
            {
                throw new IndexOutOfRangeException("Index out of bounds DataTools.Memory.MemPtr.PushOut().");
                return -1;
            }

            long ol = Length - index;
            ReAlloc(hl + amount);
            Slide(index, ol, amount);
            if (bytes is object)
            {
                Native.byteArrAtset(handle + (int)index, bytes, (uint)amount);
            }
            else
            {
                ZeroMemory(index, amount);
            }

            return Length;
        }

        /// <summary>
        /// Slides a block of memory as Unicode characters toward the beginning or toward the end of the buffer.
        /// </summary>
        /// <param name="index">The character index preceding the first character in the affected block.</param>
        /// <param name="length">The length of the block, in characters.</param>
        /// <param name="offset">The offset amount of the slide, in characters.  If the amount is negative, the block slides to the left, if it is positive it slides to the right.</param>
        /// <remarks></remarks>
        public virtual void SlideChar(long index, long length, long offset)
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
        public virtual long PullInChar(long index, long amount, bool removePressure = false)
        {
            return PullIn(index << 1, amount * 1L);
        }

        /// <summary>
        /// Extend the buffer from the specified character index.
        /// </summary>
        /// <param name="index">The index where expansion begins. The expansion starts at this position.</param>
        /// <param name="amount">Number of characters to push out.</param>
        /// <param name="addPressure">Specify whether to notify the garbage collector.</param>
        /// <remarks></remarks>
        public virtual long PushOutChar(long index, long amount, char[] chars = null, bool addPressure = false)
        {
            return PushOut(index << 1, amount * 1L, (MemPtr)chars);
        }

        /// <summary>
        /// Parts the string in both directions from index.
        /// </summary>
        /// <param name="index">The index from which to expand.</param>
        /// <param name="amount">The amount of expansion, in both directions, so the total expansion will be amount * 1.</param>
        /// <param name="addPressure">Specify whether to notify the garbage collector.</param>
        /// <remarks></remarks>
        public virtual void Part(long index, long amount, bool addPressure = false)
        {
            if (handle == IntPtr.Zero)
            {
                SetLength(amount);
                return;
            }

            long l = Length;
            if (l <= 0L)
                return;
            long ol = l - index;
            ReAlloc(l + amount * 1L);
            Slide(index, ol, amount);
            Slide(index + amount + 1L, ol, amount);
        }

        /// <summary>
        /// Inserts the specified bytes at the specified index.
        /// </summary>
        /// <param name="index">Index at which to insert.</param>
        /// <param name="value">Byte array to insert</param>
        /// <param name="addPressure">Specify whether to notify the garbage collector.</param>
        /// <remarks></remarks>
        public virtual void Insert(long index, byte[] value, bool addPressure = false)
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
        public virtual void Insert(long index, char[] value, bool addPressure = false)
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
        public virtual void Delete(long index, long amount, bool removePressure = false)
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
        public virtual void DeleteChar(long index, long amount, bool removePressure = false)
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
        public virtual void Consume(long index, long amount, bool removePressure = false)
        {
            long hl = Length;
            if (hl <= 0L || amount > index || index >= hl - amount + 1L)
            {
                throw new IndexOutOfRangeException("Index out of bounds DataTools.Memory.Heap:Consume.");
                return;
            }

            index -= amount + 1L;
            PullIn(index, amount);
            index += amount + 1L;
            PullIn(index, amount);
        }

        /// <summary>
        /// Consumes the buffer in both directions from specified character index.
        /// </summary>
        /// <param name="index">Index at which consuming begins.</param>
        /// <param name="amount">Amount of contraction, in both directions, so the total contraction will be amount * 1.</param>
        /// <param name="removePressure">Specify whether to notify the garbage collector.</param>
        /// <remarks></remarks>
        public virtual void ConsumeChar(long index, long amount, bool removePressure = false)
        {
            long hl = Length;
            if (hl <= 0L || amount > index || index >= (hl >> 1) - (amount + 1L))
            {
                throw new IndexOutOfRangeException("Index out of bounds DataTools.Memory.Heap:Consume.");
                return;
            }

            index -= amount + 1L << 1;
            PullIn(index, amount);
            index += amount + 1L << 1;
            PullIn(index, amount);
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */

        // ' Math for pointers is not intended to be used with memory pointers that you have
        // ' allocated with this structure, yourself. The math operators are intended to be 
        // ' used with a pointer that is being casually referenced.
        // '
        // ' If you want to do math with the pointer value (increment or iterate, for example)
        // ' then you will need to make a copy of the old pointer (in order to free it, later)
        // ' as this is the only variable contained in this structure
        // ' (so as to keep it suitable for substiting IntPtr
        // ' in structures passed to p/Invoke).
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public static NativeInt operator %(NativeInt operand1, SafePtr operand2)
        {
            return operand1 % operand2.handle;
        }

        public static NativeInt operator +(NativeInt operand1, SafePtr operand2)
        {
            return operand1 + operand2.handle;
        }

        public static NativeInt operator -(NativeInt operand1, SafePtr operand2)
        {
            return operand1 - operand2.handle;
        }

        public static NativeInt operator *(NativeInt operand1, SafePtr operand2)
        {
            return operand1 * (long)operand2.handle;
        }

        public static NativeInt operator /(NativeInt operand1, SafePtr operand2)
        {
            return operand1 / operand2.handle;
        }

        public static NativeInt operator /(NativeInt operand1, SafePtr operand2)
        {
            return operand1 / operand2.handle;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        // ' SafePtr
        public override bool Equals(object obj)
        {
            if (obj is SafePtr)
            {
                return ((SafePtr)obj).handle == handle;
            }
            else if (obj is IntPtr)
            {
                return Operators.ConditionalCompareObjectEqual(obj, handle, false);
            }
            else if (obj is UIntPtr)
            {
                return Native.ToSigned((UIntPtr)obj) == handle;
            }
            else
            {
                return false;
            }
        }

        public bool Equals(SafePtr other)
        {
            return other.handle == handle;
        }

        public bool Equals(IntPtr other)
        {
            return other == handle;
        }

        public bool Equals(MemPtr other)
        {
            return other == handle;
        }

        public static bool operator ==(SafePtr v1, SafePtr v2)
        {
            return (long)v1.handle == (long)v2.handle;
        }

        public static bool operator !=(SafePtr v1, SafePtr v2)
        {
            return (long)v1.handle != (long)v2.handle;
        }

        public static bool operator <(SafePtr v1, SafePtr v2)
        {
            return (long)v1.handle < (long)v2.handle;
        }

        public static bool operator >(SafePtr v1, SafePtr v2)
        {
            return (long)v1.handle > (long)v2.handle;
        }

        public static bool operator <=(SafePtr v1, SafePtr v2)
        {
            return (long)v1.handle <= (long)v2.handle;
        }

        public static bool operator >=(SafePtr v1, SafePtr v2)
        {
            return (long)v1.handle >= (long)v2.handle;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public static implicit operator IntPtr(SafePtr operand)
        {
            return operand.handle;
        }

        public static implicit operator SafePtr(IntPtr operand)
        {
            return new SafePtr(operand);
        }

        public static explicit operator SafePtr(MemPtr operand)
        {
            return operand.Handle;
        }

        public static explicit operator MemPtr(SafePtr operand)
        {
            return new MemPtr(operand.handle);
        }

        public static explicit operator SafePtr(NativeInt operand)
        {
            return new SafePtr(operand, false);
        }

        public static explicit operator NativeInt(SafePtr operand)
        {
            return operand.handle;
        }


        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        // ' in
        public static implicit operator SafePtr(sbyte operand)
        {
            var mm = new SafePtr();
            if (mm.Alloc(1L))
            {
                mm.set_SByteAt(0L, operand);
            }
            else
            {
                throw new InsufficientMemoryException();
            }

            return mm;
        }

        public static implicit operator SafePtr(byte operand)
        {
            var mm = new SafePtr();
            if (mm.Alloc(1L))
            {
                mm[0L] = operand;
            }
            else
            {
                throw new InsufficientMemoryException();
            }

            return mm;
        }

        public static implicit operator SafePtr(short operand)
        {
            var mm = new SafePtr();
            if (mm.Alloc(2L))
            {
                mm.set_ShortAt(0L, operand);
            }
            else
            {
                throw new InsufficientMemoryException();
            }

            return mm;
        }

        public static implicit operator SafePtr(ushort operand)
        {
            var mm = new SafePtr();
            if (mm.Alloc(2L))
            {
                mm.set_UShortAt(0L, operand);
            }
            else
            {
                throw new InsufficientMemoryException();
            }

            return mm;
        }

        public static implicit operator SafePtr(int operand)
        {
            var mm = new SafePtr();
            if (mm.Alloc(4L))
            {
                mm.set_IntegerAt(0L, operand);
            }
            else
            {
                throw new InsufficientMemoryException();
            }

            return mm;
        }

        public static implicit operator SafePtr(uint operand)
        {
            var mm = new SafePtr();
            if (mm.Alloc(4L))
            {
                mm.set_UIntegerAt(0L, operand);
            }
            else
            {
                throw new InsufficientMemoryException();
            }

            return mm;
        }

        public static implicit operator SafePtr(long operand)
        {
            var mm = new SafePtr();
            if (mm.Alloc(8L))
            {
                mm.set_LongAt(0L, operand);
            }
            else
            {
                throw new InsufficientMemoryException();
            }

            return mm;
        }

        public static implicit operator SafePtr(ulong operand)
        {
            var mm = new SafePtr();
            if (mm.Alloc(8L))
            {
                mm.set_ULongAt(0L, operand);
            }
            else
            {
                throw new InsufficientMemoryException();
            }

            return mm;
        }

        public static implicit operator SafePtr(float operand)
        {
            var mm = new SafePtr();
            if (mm.Alloc(4L))
            {
                mm.set_SingleAt(0L, operand);
            }
            else
            {
                throw new InsufficientMemoryException();
            }

            return mm;
        }

        public static implicit operator SafePtr(double operand)
        {
            var mm = new SafePtr();
            if (mm.Alloc(8L))
            {
                mm.set_DoubleAt(0L, operand);
            }
            else
            {
                throw new InsufficientMemoryException();
            }

            return mm;
        }


        // ' out
        public static implicit operator sbyte(SafePtr operand)
        {
            return operand.get_SByteAt(0L);
        }

        public static implicit operator byte(SafePtr operand)
        {
            return operand[0L];
        }

        public static implicit operator char(SafePtr operand)
        {
            return operand.get_CharAt(0L);
        }

        public static implicit operator short(SafePtr operand)
        {
            return operand.get_ShortAt(0L);
        }

        public static implicit operator ushort(SafePtr operand)
        {
            return operand.get_UShortAt(0L);
        }

        public static implicit operator int(SafePtr operand)
        {
            return operand.get_IntegerAt(0L);
        }

        public static implicit operator uint(SafePtr operand)
        {
            return operand.get_UIntegerAt(0L);
        }

        public static implicit operator long(SafePtr operand)
        {
            return operand.get_LongAt(0L);
        }

        public static implicit operator ulong(SafePtr operand)
        {
            return operand.get_ULongAt(0L);
        }

        public static implicit operator float(SafePtr operand)
        {
            return operand.get_SingleAt(0L);
        }

        public static implicit operator double(SafePtr operand)
        {
            return operand.get_DoubleAt(0L);
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        [MethodImpl(MethodImplOptions.ForwardRef)]
        public byte[] GetByteArray(IntPtr byteIndex, int length)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public void SetByteArray(IntPtr byteIndex, byte[] values)
        {
        }

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public sbyte[] GetSByteArray(IntPtr byteIndex, int length)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public void SetSByteArray(IntPtr byteIndex, sbyte[] values)
        {
        }

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public char[] GetCharArray(IntPtr byteIndex, int length)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public void SetCharArray(IntPtr byteIndex, char[] values)
        {
        }

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public ushort[] GetUShortArray(IntPtr byteIndex, int length)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public void SetUShortArray(IntPtr byteIndex, ushort[] values)
        {
        }

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public short[] GetShortArray(IntPtr byteIndex, int length)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public void SetShortArray(IntPtr byteIndex, short[] values)
        {
        }

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public uint[] GetUIntegerArray(IntPtr byteIndex, int length)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public void SetUIntegerArray(IntPtr byteIndex, uint[] values)
        {
        }

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public int[] GetIntegerArray(IntPtr byteIndex, int length)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public void SetIntegerArray(IntPtr byteIndex, int[] values)
        {
        }

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public ulong[] GetULongArray(IntPtr byteIndex, int length)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public void SetULongArray(IntPtr byteIndex, ulong[] values)
        {
        }

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public long[] GetLongArray(IntPtr byteIndex, int length)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public void SetLongArray(IntPtr byteIndex, long[] values)
        {
        }

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public float[] GetSingleArray(IntPtr byteIndex, int length)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public void SetSingleArray(IntPtr byteIndex, float[] values)
        {
        }

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public double[] GetDoubleArray(IntPtr byteIndex, int length)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public void SetDoubleArray(IntPtr byteIndex, double[] values)
        {
        }

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public decimal[] GetDecimalArray(IntPtr byteIndex, int length)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public void SetDecimalArray(IntPtr byteIndex, decimal[] values)
        {
        }

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public Guid[] GetGuidArray(IntPtr byteIndex, int length)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public void SetGuidArray(IntPtr byteIndex, Guid[] values)
        {
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        // ' In

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator SafePtr(byte[] operand)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator SafePtr(sbyte[] operand)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator SafePtr(short[] operand)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator SafePtr(ushort[] operand)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator SafePtr(int[] operand)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator SafePtr(uint[] operand)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator SafePtr(long[] operand)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator SafePtr(ulong[] operand)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator SafePtr(float[] operand)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator SafePtr(double[] operand)
        {
            return null;
        }

        // ' Out
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator sbyte[](SafePtr operand)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator byte[](SafePtr operand)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator short[](SafePtr operand)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator ushort[](SafePtr operand)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator int[](SafePtr operand)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator uint[](SafePtr operand)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator long[](SafePtr operand)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator ulong[](SafePtr operand)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator float[](SafePtr operand)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator double[](SafePtr operand)
        {
            return null;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        // ' Returns a pretty string, based on null-termination logic, instead of
        // ' returning every character in the allocated block.
        public static implicit operator string(SafePtr operand)
        {
            return operand.GrabString((IntPtr)0);
        }

        // ' We add 2 bytes to give us a proper null-terminated string in memory.
        public static implicit operator SafePtr(string operand)
        {
            var mm = new SafePtr();
            mm.SetString((IntPtr)0, operand);
            return mm;

            // Dim i As Integer = operand.Length << 1
            // Dim mm As New SafePtr(i + 2)
            // QuickCopyObject(Of String)(mm.Handle, operand, CUInt(i))
            // Return mm
        }

        // ' Here we return every character in the allocated block.
        [MethodImpl(MethodImplOptions.ForwardRef)]
        public static implicit operator char[](SafePtr operand)
        {
            return null;
        }

        // ' We just set the character information into the memory buffer, verbatim.
        [MethodImpl(MethodImplOptions.ForwardRef)]
        public static implicit operator SafePtr(char[] operand)
        {
            return null;
        }

        public static implicit operator SafePtr(string[] operand)
        {
            if (operand is null || operand.Length == 0)
                return null;
            var mm = new SafePtr();
            int c = operand.Length - 1;
            long l = 2L;
            for (int i = 0, loopTo = c; i <= loopTo; i++)
                l += (operand[i].Length + 1) * 2;
            if (!mm.Alloc(l))
                return null;
            var p = mm.handle;
            for (int i = 0, loopTo1 = c; i <= loopTo1; i++)
            {
                QuickCopyObject<string>(p, operand[i], Conversions.ToUInteger(operand[i].Length * 2));
                p = p + (operand[i].Length * 2 + 2);
            }

            return mm;
        }

        public static implicit operator string[](SafePtr operand)
        {
            return operand.GrabStringArray((IntPtr)0);
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public static explicit operator SafePtr(Guid operand)
        {
            var mm = new SafePtr();
            if (mm.Alloc(16L))
            {
                mm.set_GuidAt(0L, operand);
            }
            else
            {
                throw new OutOfMemoryException();
            }

            return mm;
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static explicit operator SafePtr(Guid[] operand)
        {
            return null;
        }

        public static explicit operator Guid(SafePtr operand)
        {
            return operand.get_GuidAt(0L);
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static explicit operator Guid[](SafePtr operand)
        {
            return null;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public static implicit operator SafePtr(decimal operand)
        {
            var mm = new SafePtr();
            if (mm.Alloc(16L))
            {
                mm.set_DecimalAt(0L, operand);
            }
            else
            {
                throw new OutOfMemoryException();
            }

            return mm;
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator SafePtr(decimal[] operand)
        {
            return null;
        }

        public static implicit operator decimal(SafePtr operand)
        {
            return operand.get_DecimalAt(0L);
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator decimal[](SafePtr operand)
        {
            return null;
        }


        // ' Color

        public static implicit operator SafePtr(Color operand)
        {
            var mm = new SafePtr();
            if (mm.Alloc(4L))
            {
                QuickCopyObject<Color>(mm, operand, 4U);
            }
            else
            {
                throw new OutOfMemoryException();
            }

            return mm;
        }

        public static implicit operator SafePtr(Color[] operand)
        {
            var mm = new SafePtr();
            if (mm.Alloc(4 * operand.Length))
            {
                QuickCopyObject<Color[]>(ref operand, mm, (uint)(4 * operand.Length));
            }
            else
            {
                throw new OutOfMemoryException();
            }

            return mm;
        }

        public static implicit operator Color(SafePtr operand)
        {
            var g = new Color();
            QuickCopyObject<Color>(ref g, operand, 4U);
            return g;
        }

        public static implicit operator Color[](SafePtr operand)
        {
            Color[] clr;
            long l = (long)(operand.Length / 4d);
            if (l > uint.MaxValue)
                throw new InvalidCastException();
            clr = new Color[(int)(l - 1L + 1)];
            QuickCopyObject<Color[]>(ref clr, operand, (uint)(l * 4L));
            return clr;
        }


        // ' DateTime

        public static implicit operator SafePtr(DateTime operand)
        {
            var mm = new SafePtr();
            if (mm.Alloc(8L))
            {
                mm.set_LongAt(0L, operand.ToBinary());
            }
            else
            {
                throw new OutOfMemoryException();
            }

            return mm;
        }

        public static implicit operator SafePtr(DateTime[] operand)
        {
            var mm = new SafePtr();
            if (mm.Alloc(8 * operand.Length))
            {
                QuickCopyObject<DateTime[]>(ref operand, mm, (uint)(8 * operand.Length));
            }
            else
            {
                throw new OutOfMemoryException();
            }

            return mm;
        }

        public static implicit operator DateTime(SafePtr operand)
        {
            return DateTime.FromBinary(operand.get_LongAt(0L));
        }

        public static implicit operator DateTime[](SafePtr operand)
        {
            DateTime[] clr;
            long l = (long)(operand.Length / 8d);
            if (l > uint.MaxValue)
                throw new InvalidCastException();
            clr = new DateTime[(int)(l - 1L + 1)];
            QuickCopyObject<DateTime[]>(ref clr, operand, (uint)(l * 8L));
            return clr;
        }


        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Clears the entire object.
        /// </summary>
        /// <remarks></remarks>
        public virtual void Clear()
        {
            Free();
        }

        /// <summary>
        /// Allocate a new memory buffer on the process heap.
        /// </summary>
        /// <param name="length">Length of the new buffer, in bytes.</param>
        /// <returns>True if successful.</returns>
        /// <remarks></remarks>
        public virtual bool Alloc(long length)
        {
            bool AllocRet = default;
            if (length == 0L)
                return true;
            if (handle != (IntPtr)0)
            {
                return ReAlloc(length);
            }

            switch (_MemType)
            {
                case MemAllocType.Virtual:
                    {
                        if (VirtualAlloc(length))
                        {
                            _buffer = length;
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                        break;
                    }

                case MemAllocType.Network:
                    {
                        if (NetAlloc((int)length))
                        {
                            _buffer = length;
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                        break;
                    }

                default:
                    {
                        handle = Native.HeapAlloc(hHeap, 8U, (IntPtr)length);
                        AllocRet = handle != (IntPtr)0;
                        if (AllocRet)
                        {
                            _buffer = Native.HeapSize(hHeap, 0U, handle);
                        }

                        break;
                    }
            }

            if (AllocRet)
            {
                _MemType = MemAllocType.Heap;
                GC.AddMemoryPressure(_buffer);
            }
            else
            {
                _buffer = 0;
            }

            return AllocRet;
        }

        /// <summary>
        /// Reallocate a memory buffer with a new size on the process heap.
        /// </summary>
        /// <param name="length">New length of the memory buffer.</param>
        /// <returns>True if successful.</returns>
        /// <remarks></remarks>
        public virtual bool ReAlloc(long length)
        {
            bool ReAllocRet = default;
            if (IsInvalid)
                return Alloc(length);

            // If length <= 0 Then
            // Throw New ArgumentOutOfRangeException("Length cannot be less than or equal to zero.")
            // End If

            long l = _buffer;
            if (length == l)
                return true;
            if (_MemType == MemAllocType.Heap)
            {
                var p = Native.HeapReAlloc(hHeap, 8, handle, (IntPtr)length);
                if (p != (IntPtr)0)
                {
                    handle = p;
                    _buffer = Native.HeapSize(hHeap, 0U, p);
                    ReAllocRet = true;
                }
                else
                {
                    ReAllocRet = false;
                }
            }
            else if (_MemType == MemAllocType.Network)
            {
                var mm = new MemPtr();
                mm.NetAlloc((int)length);
                if (mm.Handle == (IntPtr)0)
                {
                    ReAllocRet = false;
                }
                else
                {
                    _buffer = length;
                    GC.AddMemoryPressure(_buffer);
                    Native.MemCpy(mm.Handle, handle, (uint)l);
                    Native.NetApiBufferFree(handle);
                    GC.RemoveMemoryPressure(l);
                    handle = mm.Handle;
                    _buffer = Length;
                    return true;
                }
            }
            else if (_MemType == MemAllocType.Virtual)
            {
                var mm = new MemPtr();
                mm.VirtualAlloc(length);
                if (mm.Handle == (IntPtr)0)
                {
                    ReAllocRet = false;
                }
                else
                {
                    _buffer = length;
                    Native.MemCpy(mm.Handle, handle, (uint)l);
                    VirtualFree();
                    _MemType = MemAllocType.Virtual;
                    handle = mm.Handle;
                    _buffer = Length;
                    return true;
                }
            }
            else if (_MemType == MemAllocType.Com)
            {
                MemPtr mm = Marshal.AllocCoTaskMem((int)length);
                if (mm.Handle == (IntPtr)0)
                {
                    ReAllocRet = false;
                }
                else
                {
                    _buffer = length;
                    Native.MemCpy(mm.Handle, handle, (uint)l);
                    Marshal.FreeCoTaskMem(handle);
                    handle = mm.Handle;
                    ReAllocRet = true;
                }
            }
            else
            {
                return false;
            }

            if (ReAllocRet)
            {
                if (IntPtr.Size == 4)
                {
                    int x = (int)_buffer;
                    if (x > l)
                    {
                        GC.AddMemoryPressure(x - l);
                    }
                    else
                    {
                        GC.RemoveMemoryPressure(l - x);
                    }
                }
                else
                {
                    long x = _buffer;
                    if (x > l)
                    {
                        GC.AddMemoryPressure(x - l);
                    }
                    else
                    {
                        GC.RemoveMemoryPressure(l - x);
                    }
                }
            }

            return ReAllocRet;
        }

        /// <summary>
        /// Frees the resources allocated by the current object.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public virtual bool Free()
        {
            bool FreeRet = default;
            FreeRet = handle == (IntPtr)0;
            switch (_MemType)
            {
                case MemAllocType.Heap:
                    {
                        try
                        {
                            if (Native.HeapValidate(hHeap, 0U, handle) == true)
                            {
                                FreeRet = Conversions.ToInteger(Native.HeapFree(hHeap, 0U, handle)) != 0;
                            }
                        }
                        catch (Exception ex)
                        {
                        }

                        break;
                    }

                case MemAllocType.Virtual:
                    {
                        return VirtualFree();
                    }

                case MemAllocType.Network:
                    {
                        return NetFree();
                    }

                case MemAllocType.Com:
                    {
                        try
                        {
                            Marshal.FreeCoTaskMem(handle);
                            FreeRet = true;
                        }
                        catch (Exception ex)
                        {
                        }

                        break;
                    }
            }

            if (FreeRet)
            {
                GC.RemoveMemoryPressure(_buffer);
                _buffer = 0;
                handle = (IntPtr)0;
                _MemType = MemAllocType.Invalid;
            }

            return FreeRet;
        }



        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Allocate a network API compatible memory buffer.
        /// </summary>
        /// <param name="size">Size of the buffer to allocate, in bytes.</param>
        /// <remarks></remarks>
        public virtual bool NetAlloc(int size)
        {
            bool NetAllocRet = default;
            // ' just ignore a full buffer.
            if (handle != (IntPtr)0)
                return false;
            var argBuffer = handle;
            Native.NetApiBufferAllocate(size, ref argBuffer);
            handle = argBuffer;
            if (handle != (IntPtr)0)
            {
                _buffer = size;
                NetAllocRet = true;
                _MemType = MemAllocType.Network;
                GC.AddMemoryPressure(_buffer);
            }
            else
            {
                _buffer = 0;
                handle = (IntPtr)0;
                NetAllocRet = false;
                _MemType = MemAllocType.Invalid;
            }

            return NetAllocRet;
        }

        /// <summary>
        /// Free a network API compatible memory buffer previously allocated with NetAlloc.
        /// </summary>
        /// <remarks></remarks>
        public virtual bool NetFree()
        {
            bool NetFreeRet = default;
            if (_MemType != MemAllocType.Network)
                return false;
            if (handle == (IntPtr)0)
                return true;
            NetFreeRet = Native.NetApiBufferFree(handle) == 0;
            if (NetFreeRet)
            {
                GC.RemoveMemoryPressure(_buffer);
                _buffer = 0;
                handle = (IntPtr)0;
            }

            _MemType = MemAllocType.Invalid;
            return NetFreeRet;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Allocats a region of virtual memory.
        /// </summary>
        /// <param name="size">The size of the region of memory to allocate.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public virtual bool VirtualAlloc(long size)
        {
            bool VirtualAllocRet = default;
            if (handle != (IntPtr)0)
                return false;


            // ' While the function doesn't need to call VirtualAlloc, it hasn't necessarily failed, either.
            if (size == 0L)
            {
                VirtualAllocRet = true;
                return VirtualAllocRet;
            }

            long l = size;
            _buffer = l;
            handle = Native.VirtualAlloc((IntPtr)0, (IntPtr)l, VMemAllocFlags.MEM_COMMIT | VMemAllocFlags.MEM_RESERVE, MemoryProtectionFlags.PAGE_READWRITE);
            VirtualAllocRet = handle != (IntPtr)0;
            _MemType = MemAllocType.Virtual;
            if (VirtualAllocRet)
            {
                GC.AddMemoryPressure(_buffer);
            }
            else
            {
                _buffer = 0;
                handle = (IntPtr)0;
            }

            return VirtualAllocRet;
        }

        /// <summary>
        /// Frees a region of memory previously allocated with VirtualAlloc.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public virtual bool VirtualFree()
        {
            bool VirtualFreeRet = default;
            if (_MemType != MemAllocType.Virtual)
                return false;
            long l;

            // ' While the function doesn't need to call VirtualFree, it hasn't necessarily failed, either.
            if (handle == (IntPtr)0)
            {
                VirtualFreeRet = true;
            }
            else
            {
                // ' see if we need to tell the garbage collector anything.
                l = VirtualLength();
                VirtualFreeRet = Native.VirtualFree(handle);

                // ' see if we need to tell the garbage collector anything.
                if (VirtualFreeRet)
                {
                    handle = (IntPtr)0;
                    GC.RemoveMemoryPressure(l);
                }
            }

            _MemType = MemAllocType.Invalid;
            return VirtualFreeRet;
        }

        /// <summary>
        /// Returns the size of a region of virtual memory previously allocated with VirtualAlloc.
        /// </summary>
        /// <returns>The size of a virtual memory region or zero.</returns>
        /// <remarks></remarks>
        public virtual long VirtualLength()
        {
            if (_MemType != MemAllocType.Virtual)
                return 0L;
            if (handle == (IntPtr)0)
                return 0L;
            var m = new MEMORY_BASIC_INFORMATION();
            if (Native.VirtualQuery(handle, ref m, (IntPtr)Marshal.SizeOf(m)) != (IntPtr)0)
            {
                return Conversions.ToLong(m.RegionSize);
            }

            return 0L;
        }
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Clears all the memory
        /// </summary>
        /// <remarks></remarks>
        public virtual void ZeroMemory()
        {
            try
            {
                long l = Length;
                if (Conversions.ToBoolean(l & 0xFFFFFFFF00000000L))
                {
                    Native.n_memset(handle, 0, (IntPtr)l);
                }
                else
                {
                    Native.MemSet(handle, 0, (uint)l);
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Clears all memory to the specified length.
        /// </summary>
        /// <param name="length">Length of memory to clear.</param>
        /// <remarks></remarks>
        public virtual void ZeroMemory(long length)
        {
            try
            {
                if (Conversions.ToBoolean(length & 0xFFFFFFFF00000000L))
                {
                    Native.n_memset(handle, 0, (IntPtr)length);
                }
                else
                {
                    Native.MemSet(handle, 0, (uint)length);
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Clears all the memory starting at the specified byte index for the specified length.
        /// </summary>
        /// <param name="byteIndex">Byte index, relative to the memory pointer, at which to begin clearing.</param>
        /// <param name="length">Length of memory to clear.</param>
        /// <remarks></remarks>
        public virtual void ZeroMemory(long byteIndex, long length)
        {
            try
            {
                if (Conversions.ToBoolean(length & 0xFFFFFFFF00000000L))
                {
                    Native.n_memset(handle + (int)byteIndex, 0, (IntPtr)length);
                }
                else
                {
                    Native.MemSet(handle + (int)byteIndex, 0, (uint)length);
                }
            }
            catch (Exception ex)
            {
            }
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Creates an exact copy of the memory associated with this pointer.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public virtual object Clone()
        {
            object CloneRet = default;
            var mm = new SafePtr();
            long l = Length;
            if (l <= 0L)
            {
                CloneRet = mm;
                return CloneRet;
            }

            switch (_MemType)
            {
                case MemAllocType.Heap:
                    {
                        mm.Alloc(l);
                        break;
                    }

                case MemAllocType.Network:
                    {
                        mm.NetAlloc((int)l);
                        break;
                    }

                case MemAllocType.Virtual:
                    {
                        mm.VirtualAlloc(l);
                        break;
                    }

                case MemAllocType.Aligned:
                    {
                        mm.Alloc(l);
                        break;
                    }

                case MemAllocType.Com:
                    {
                        mm.Alloc(l);
                        break;
                    }

                case MemAllocType.Other:
                case MemAllocType.Invalid:
                    {
                        mm.Alloc(l);
                        break;
                    }
            }

            if (l <= uint.MaxValue)
            {
                Native.MemCpy(mm.handle, handle, (uint)l);
            }
            else
            {
                CopyMemory(mm.handle, handle, (int)l);
            }

            CloneRet = mm;
            return CloneRet;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Initialize a new instance of this structure and allocate a memory block
        /// of the specified size.
        /// </summary>
        /// <param name="size">Size of the memory block to allocate.</param>
        /// <remarks></remarks>
        public SafePtr(long size) : base(Native.CIntPtr(0), true)
        {
            Alloc(size);
        }

        /// <summary>
        /// Initialize a new instance of this structure and allocate a memory block
        /// of the specified size.
        /// </summary>
        /// <param name="size">Size of the memory block to allocate.</param>
        /// <remarks></remarks>
        public SafePtr(int size) : base(Native.CIntPtr(0), true)
        {
            Alloc(size);
        }

        /// <summary>
        /// Initialize a new instance of this structure with the specified memory pointer.
        /// </summary>
        /// <param name="ptr">Pointer to a block of memory.</param>
        /// <remarks></remarks>
        public SafePtr(UIntPtr ptr, bool fOwn = true) : base(Native.CIntPtr(0), true)
        {
            handle = Native.ToSigned(ptr);
        }

        /// <summary>
        /// Initialize a new instance of this structure with the specified memory pointer.
        /// </summary>
        /// <param name="ptr">Pointer to a block of memory.</param>
        /// <remarks></remarks>
        public SafePtr(IntPtr ptr, bool fOwn = true) : base(Native.CIntPtr(0), fOwn)
        {
            handle = ptr;
        }

        public SafePtr() : base(Native.CIntPtr(0), true)
        {
        }

        /// <summary>
        /// Copies the memory pointer into a string and returns the value.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            return Conversions.ToString(this);
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public virtual IEnumerator<byte> GetEnumerator()
        {
            IEnumerator<byte> GetEnumeratorRet = default;
            GetEnumeratorRet = new SafePtrEnumeratorByte(this);
            return GetEnumeratorRet;
        }

        public virtual IEnumerator GetEnumerator()
        {
            IEnumerator GetEnumeratorRet = default;
            GetEnumeratorRet = new SafePtrEnumeratorByte(this);
            return GetEnumeratorRet;
        }

        public virtual IEnumerator GetEnumerator1() => GetEnumerator();

        public virtual IEnumerator<char> GetEnumerator()
        {
            IEnumerator<char> GetEnumeratorRet = default;
            GetEnumeratorRet = new SafePtrEnumeratorChar(this);
            return GetEnumeratorRet;
        }

        public virtual IEnumerator<char> GetEnumerator2() => GetEnumerator();

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public override bool IsInvalid
        {
            get
            {
                return handle == (IntPtr)0;
            }
        }

        protected override bool ReleaseHandle()
        {
            return Free();
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    } // ' SafePtr

    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    public class SafePtrEnumeratorByte : IEnumerator<byte>
    {
        private SafePtr mm;
        private int pos = -1;

        internal SafePtrEnumeratorByte(SafePtr subj)
        {
            mm = subj;
        }

        public byte Current
        {
            get
            {
                byte CurrentRet = default;
                CurrentRet = mm[pos];
                return CurrentRet;
            }
        }

        public object Current
        {
            get
            {
                object CurrentRet = default;
                CurrentRet = mm[pos];
                return CurrentRet;
            }
        }

        public bool MoveNext()
        {
            pos += 1;
            if (pos >= mm.Length)
                return false;
            else
                return true;
        }

        public void Reset()
        {
            pos = -1;
        }

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        private bool disposedValue; // To detect redundant calls

        // IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                mm = null;
            }

            disposedValue = true;
        }

        ~SafePtrEnumeratorByte()
        {
            // Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(false);
        }

        // This code added by Visual Basic to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    }

    public class SafePtrEnumeratorChar : IEnumerator<char>
    {
        private SafePtr mm;
        private int pos = -1;

        internal SafePtrEnumeratorChar(SafePtr subj)
        {
            mm = subj;
        }

        public char Current
        {
            get
            {
                char CurrentRet = default;
                CurrentRet = mm.get_CharAt(pos);
                return CurrentRet;
            }
        }

        public object Current
        {
            get
            {
                object CurrentRet = default;
                CurrentRet = mm.get_CharAt(pos);
                return CurrentRet;
            }
        }

        public bool MoveNext()
        {
            pos += 1;
            if (pos >= mm.Length >> 1)
                return false;
            else
                return true;
        }

        public void Reset()
        {
            pos = -1;
        }

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        private bool disposedValue; // To detect redundant calls

        // IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                mm = null;
            }

            disposedValue = true;
        }

        ~SafePtrEnumeratorChar()
        {
            // Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(false);
        }

        // This code added by Visual Basic to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    }

    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
}












// // operator SafePtr to string
// .method public specialname static string op_Implicit(class DataTools.Memory.SafePtr operand) cil managed 
// {
// // THIS IS GOOD
// .maxstack 8
// .locals init
// (
// int64 lenIn
// )

// ldarg.s    0
// call       instance int64 DataTools.Memory.SafePtr::get_Length()

// stloc.s    lenIn
// ldloc.s    lenIn
// ldc.i8     0
// ble  NULRET


// ldarg.s    0

// ldc.i4.0
// conv.i

// ldloc.s    lenIn
// conv.i4
// ldc.i4.1
// shr

// call       instance class string DataTools.Memory.SafePtr::GetString(native int, int32)

// br		   RET
// NULRET:
// ldc.i4.0
// conv.i

// RET:
// ret
// }


// // function SafePtr to string
// .method public instance class string GetString(native int byteIndex, int32 length) cil managed 
// {
// .maxstack 8
// .locals init
// (
// string x
// )

// ldc.i4.0
// conv.u2
// ldarg.s     length
// newobj      [System.Runtime]System.String::.ctor(char, int32)
// stloc.s     x
// ldloc.s     x
// ldc.i4.0
// ldelema     [System.Runtime]System.Char

// ldarg.s     0
// ldfld       native int [System.Runtime]System.Runtime.InteropServices.SafeHandle::handle
// ldarg.s     byteIndex
// add

// sizeof      native int
// ldc.i4.4
// beq INT32

// ldc.i4.4
// sub

// INT32:
// ldarg.s length
// ldc.i4 0.1
// shl()
// conv.u4()

// cpblk()

// ldloc.s x
// ret()

// }


// // operator string to SafePtr
// .method public specialname static class DataTools.Memory.SafePtr op_Implicit(string operand) cil managed 
// {
// // THIS IS GOOD
// .maxstack 3
// .locals init
// (
// class DataTools.Memory.SafePtr mm
// )
// newobj       instance void DataTools.Memory.SafePtr::.ctor()

// stloc.s      0
// ldloc.s      0

// ldc.i4.0
// conv.i
// ldarg.s      0
// call instance void DataTools.Memory.SafePtr::SetString(native int, string)
// ldloc.s      0
// ret

// }