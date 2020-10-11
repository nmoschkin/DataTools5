// ' ************************************************* ''
// ' DataTools Visual Basic Utility Library 
// '
// ' Module: MemPtr Structure
// '         Exhaustive in-place replacement
// '         for IntPtr.
// ' 
// ' Copyright (C) 2011-2020 Nathan Moschkin
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
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace DataTools.Memory
{

    // ' This structure is intended to make it more convenient to marshal 
    // ' with the operating system.

    // ' There is no mechanism in .NET for the 
    // ' managed disposal of structures.  This
    // ' structure is intended to be used responsibly as a 
    // ' quick replacement for IntPtr that features 
    // ' some utility. 

    [HideModuleName]
    static class MemPtrStrings
    {
        public const string MemTooBig = "Memory pointer is too big.";
    }

    /// <summary>
    /// The MemPtr structure.  Drop-in replacement for IntPtr.
    /// Use anywhere you use IntPtr.  Be sure to dispose of your
    /// unmanaged resources.  No garbage collection is done on this
    /// structure (because it is a structure), so all freeing of memory
    /// must be handled, in code.
    /// </summary>
    /// <remarks></remarks>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct MemPtr : IDisposable, ICloneable, IEnumerable<byte>, IEnumerable<char>, IEquatable<MemPtr>, IEquatable<IntPtr>, IEquatable<UIntPtr>





    {


        /// <summary>
        /// The internal pointer value of the structure.
        /// </summary>
        /// <remarks></remarks>
        internal IntPtr _ptr;

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        // ' A List Key/Value pairs of Integral types and their atomic sizes (in bytes).
        private static List<KeyValuePair<Type, int>> _primitiveCache = new List<KeyValuePair<Type, int>>();
        private static IntPtr _procHeap = Native.GetProcessHeap();

        /// <summary>
        /// Returns a null pointer.
        /// </summary>
        public static readonly MemPtr Empty = new MemPtr(0);

        /// <summary>
        /// Returns INVALID_HANDLE_VALUE (-1)
        /// </summary>
        public static readonly MemPtr InvalidHandle = new MemPtr(new IntPtr(-1L));

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
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
                Native.MemCpy(ptr, _ptr, (uint)size);
            }
            else
            {
                Native.n_memcpy(ptr, _ptr, (UIntPtr)size.ToInt64());
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
            if (_ptr != (IntPtr)0 || Alloc(size.ToInt64()))
            {
                if (size.ToInt64() <= uint.MaxValue)
                {
                    Native.MemCpy(_ptr, ptr, (uint)size);
                }
                else
                {
                    Native.n_memcpy(ptr, _ptr, (UIntPtr)size.ToInt64());
                }
            }
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Pin an object, associate the MemPtr with the address of the pinned
        /// object and return the GCHandle.
        /// </summary>
        /// <param name="obj">Object to pin and associate.</param>
        /// <returns>A GCHandle to the pinned object.</returns>
        /// <remarks>MemPtr must be empty for this function to succeed.</remarks>
        public GCHandle Pin(object obj)
        {
            if (_ptr != (IntPtr)0)
                return default;
            var g = GCHandle.Alloc(obj, GCHandleType.Pinned);
            _ptr = g.AddrOfPinnedObject();
            return g;
        }

        /// <summary>
        /// Frees the GCHandle and disassociates the internal pointer from the address of the pinned object.
        /// </summary>
        /// <param name="gc">GCHandle of the object to be disassociated and freed.</param>
        /// <remarks></remarks>
        public void Free(GCHandle gc)
        {
            if (_ptr != gc.AddrOfPinnedObject())
                return;
            gc.Free();
            _ptr = (IntPtr)0;
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
            StrLenRet = Native.CharZero(_ptr + (int)byteIndex);
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
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public string GrabString(IntPtr byteIndex, int length)
        {
            return null;
            // If _ptr = 0 Then Return Nothing

            // If length <= 0 Then Throw New IndexOutOfRangeException("length must be greater than zero")

            // GrabString = New String(ChrW(0), length)
            // QuickCopyObject(Of String)(GrabString, New IntPtr(CLng(_ptr) + byteIndex.ToInt64), CType(length << 1, UInteger))
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
            var tp = new IntPtr((long)_ptr + byteIndex.ToInt64());
            int e = Native.ByteZero(tp);
            byte[] ba;
            if (e == 0)
                return "";
            ba = new byte[e];
            Native.byteArrAtget(ba, new IntPtr((long)_ptr + (long)byteIndex), (uint)e);
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
            if (_ptr == (IntPtr)0)
                return null;
            if (length <= 0)
                throw new IndexOutOfRangeException("length must be greater than zero");
            byte[] ba;
            ba = new byte[length];
            Native.byteArrAtget(ba, new IntPtr((long)_ptr + (long)byteIndex), (uint)length);
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
            if (_ptr == (IntPtr)0)
                return null;
            char b;
            int i = 0;
            long sb = (long)byteIndex;
            string[] sout = null;
            int ct = 0;
            long l = (long)byteIndex;
            var tp = new IntPtr(l + (long)_ptr);
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
            var p = new IntPtr((long)_ptr + byteIndex.ToInt64());
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
        public void SetBytes(IntPtr byteIndex, byte[] data)
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
        public byte[] GrabBytes(IntPtr byteIndex, int length)
        {
            return null;
        }


        /// <summary>
        /// Sets the memory at the specified index to the specified sbyte array.
        /// </summary>
        /// <param name="byteIndex"></param>
        /// <param name="data"></param>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public void SetSBytes(IntPtr byteIndex, byte[] data)
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
        public byte[] GrabSBytes(IntPtr byteIndex, int length)
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
        public void GrabBytes(IntPtr byteIndex, int length, ref byte[] data)
        {
            if (_ptr == (IntPtr)0)
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
        private void byteGet(IntPtr byteIndex, int length, ref byte[] data)
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
        public void GrabBytes(IntPtr byteIndex, int length, ref byte[] data, int arrayIndex)
        {
            if (_ptr == (IntPtr)0)
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
            var pdest = gh.AddrOfPinnedObject() + arrayIndex;
            Native.MemCpy(pdest, new IntPtr((long)_ptr + (long)byteIndex), (uint)length);
            gh.Free();
        }

        /// <summary>
        /// Returns the results of the buffer as if it were a BSTR type String.
        /// </summary>
        /// <param name="comPtr">Specifies whether or not the current MemPtr is an actual COM pointer to a BSTR.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string BSTR(bool comPtr = true)
        {
            string BSTRRet = default;
            var d = default(int);
            string s;
            var p = comPtr ? _ptr - 4 : _ptr;
            Native.intAtget(ref d, p);
            s = new string('\0', d);
            QuickCopyObject<string>(ref s, p + 4, (uint)(d << 1));
            BSTRRet = s;
            return BSTRRet;
        }

        /// <summary>
        /// Returns the contents of this buffer as a string.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public string LpwStr()
        {
            string LpwStrRet = default;
            LpwStrRet = GrabString((IntPtr)0);
            return LpwStrRet;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public void SetIntegerArray(long byteIndex, int[] values)
        {
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public int[] GetIntegerArray(long byteIndex, int length = 0)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public void SetUIntegerArray(long byteIndex, uint[] values)
        {
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public uint[] GetUIntegerArray(long byteIndex, uint length = 0U)
        {
            return null;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Gets or sets the pointer to the memory block.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public IntPtr Handle
        {
            get
            {
                IntPtr HandleRet = default;
                HandleRet = _ptr;
                return HandleRet;
            }

            set
            {
                if (_ptr != (IntPtr)0)
                {
                    Free();
                }

                _ptr = value;
            }
        }

        /// <summary>
        /// Gets the length, in bytes, of the memory block.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public long Length(IntPtr? hHeap = default)
        {
            if (_ptr == (IntPtr)0)
                return 0L;
            if (hHeap is null)
                hHeap = _procHeap;
            try
            {
                return (long)Native.HeapSize((IntPtr)hHeap, 0U, _ptr);
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        /// <summary>
        /// Sets the length of the memory block.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="hHeap">Optional private heap.</param>
        /// <remarks></remarks>
        public void SetLength(long value, IntPtr? hHeap = default)
        {
            if (hHeap is null)
                hHeap = _procHeap;
            Alloc(value, hHeap: hHeap);
        }

        /// <summary>
        /// Calculate the CRC 32 for the block of memory represented by this structure.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public uint CalculateCrc32()
        {
            uint CalculateCrc32Ret = default;
            var l = new IntPtr(Length());
            CalculateCrc32Ret = Crc32.Calculate(_ptr, l);
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
            var l = new IntPtr(Length());
            CalculateCrc32Ret = Crc32.Calculate(_ptr, l, bufflen: bufflen);
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
            CalculateCrc32Ret = Crc32.Calculate(_ptr, length);
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
            CalculateCrc32Ret = Crc32.Calculate(_ptr, length, bufflen: bufflen);
            return CalculateCrc32Ret;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Converts the contents of an unmanaged pointer into a structure.
        /// </summary>
        /// <typeparam name="T">The type of structure requested.</typeparam>
        /// <returns>New instance of T.</returns>
        /// <remarks></remarks>
        public T ToStruct<T>() where T : struct
        {
            T ToStructRet = default;
            ToStructRet = (T)Marshal.PtrToStructure(_ptr, typeof(T));
            return ToStructRet;
        }

        /// <summary>
        /// Sets the contents of a structure into an unmanaged pointer.
        /// </summary>
        /// <typeparam name="T">The type of structure to set.</typeparam>
        /// <param name="val">The structure to set.</param>
        /// <remarks></remarks>
        public void FromStruct<T>(T val) where T : struct
        {
            int cb = Marshal.SizeOf(val);
            if (_ptr == (IntPtr)0)
                AllocZero(cb);
            Marshal.StructureToPtr(val, _ptr, false);
        }

        /// <summary>
        /// Converts the contents of an unmanaged pointer at the specified byte index into a structure.
        /// </summary>
        /// <typeparam name="T">The type of structure requested.</typeparam>
        /// <param name="byteIndex">The byte index relative to the pointer at which to begin copying.</param>
        /// <returns>New instance of T.</returns>
        /// <remarks></remarks>
        public T ToStructAt<T>(IntPtr byteIndex) where T : struct
        {
            T ToStructAtRet = default;
            ToStructAtRet = (T)Marshal.PtrToStructure(_ptr + (int)byteIndex, typeof(T));
            return ToStructAtRet;
        }

        /// <summary>
        /// Sets the contents of a structure into a memory buffer at the specified byte index.
        /// </summary>
        /// <typeparam name="T">The type of structure to set.</typeparam>
        /// <param name="byteIndex">The byte index relative to the pointer at which to begin copying.</param>
        /// <param name="val">The structure to set.</param>
        /// <remarks></remarks>
        public void FromStructAt<T>(IntPtr byteIndex, T val) where T : struct
        {
            int cb = Marshal.SizeOf(val);
            Marshal.StructureToPtr(val, _ptr + (int)byteIndex, false);
        }

        /// <summary>
        /// Copies the contents of the buffer at the specified index into a blittable structure array.
        /// </summary>
        /// <typeparam name="T">The structure type.</typeparam>
        /// <param name="byteIndex">The index at which to begin copying.</param>
        /// <returns>An array of T.</returns>
        /// <remarks></remarks>
        public T[] ToBlittableStructArrayAt<T>(IntPtr byteIndex) where T : struct
        {
            if (_ptr == (IntPtr)0)
                return null;
            long l = Length() - Conversions.ToLong(byteIndex);
            int cb = Marshal.SizeOf(new T());
            int c = (int)(l / (double)cb);
            T[] tt;
            tt = new T[c];
            var gh = GCHandle.Alloc(tt, GCHandleType.Pinned);
            if (l <= uint.MaxValue)
            {
                Native.MemCpy(gh.AddrOfPinnedObject(), _ptr, (uint)l);
            }
            else
            {
                Native.n_memcpy(gh.AddrOfPinnedObject(), _ptr, (UIntPtr)l);
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
        public void FromBlittableStructArrayAt<T>(IntPtr byteIndex, T[] value) where T : struct
        {
            if (_ptr == (IntPtr)0 && byteIndex != (IntPtr)0)
                return;
            long l;
            int cb = Marshal.SizeOf(new T());
            int c = value.Count();
            l = c * cb;
            if (_ptr == (IntPtr)0)
            {
                if (!Alloc(l))
                    return;
            }

            var p = _ptr + (int)byteIndex;
            var gh = GCHandle.Alloc(value, GCHandleType.Pinned);
            if (l <= uint.MaxValue)
            {
                Native.MemCpy(p, gh.AddrOfPinnedObject(), (uint)l);
            }
            else
            {
                Native.n_memcpy(p, gh.AddrOfPinnedObject(), (UIntPtr)l);
            }

            gh.Free();
        }

        /// <summary>
        /// Copies the contents of the buffer into a blittable structure array.
        /// </summary>
        /// <typeparam name="T">The structure type.</typeparam>
        /// <returns>An array of T.</returns>
        /// <remarks></remarks>
        public T[] ToBlittableStructArray<T>() where T : struct
        {
            return ToBlittableStructArrayAt<T>((IntPtr)0);
        }

        /// <summary>
        /// Copies a blittable structure array into the buffer, initializing a new buffer, if necessary.
        /// </summary>
        /// <typeparam name="T">The structure type.</typeparam>
        /// <param name="value">The structure array to copy.</param>
        /// <remarks></remarks>
        public void FromBlittableStructArray<T>(T[] value) where T : struct
        {
            FromBlittableStructArrayAt((IntPtr)0, value);
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
            // If _ptr = 0 Then Return Guid.Empty
            // guidAtget(GuidAtAbsolute, CType(CLng(_ptr) + index, IntPtr))
            // If _ptr = 0 Then Return
            // guidAtset(CType(CLng(_ptr) + index, IntPtr), value)
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
            // If _ptr = 0 Then Return Guid.Empty
            // guidAtget(GuidAt, CType(CLng(_ptr) + (index * 16), IntPtr))
            // If _ptr = 0 Then Return
            // guidAtset(CType(CLng(_ptr) + (index * 16), IntPtr), value)
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
                // byteAtget(ByteAt, New IntPtr(clng(_ptr) + index))
            }

            [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
            set
            {
                // byteAtset(New IntPtr(clng(_ptr) + index), value)
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
            // sbyteAtget(SByteAt, New IntPtr(clng(_ptr) + index))
            // sbyteAtset(New IntPtr(clng(_ptr) + index), value)
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
            // charAtget(CharAt, New IntPtr(clng(_ptr) + (index * 2)))
            // charAtset(New IntPtr(clng(_ptr) + (index * 2)), value)
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
            // shortAtget(ShortAt, New IntPtr(clng(_ptr) + (index * 2)))
            // shortAtset(New IntPtr(clng(_ptr) + (index * 2)), value)
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
            // ushortAtget(UShortAt, New IntPtr(clng(_ptr) + (index * 2)))
            // ushortAtset(New IntPtr(clng(_ptr) + (index * 2)), value)
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
            // intAtget(IntegerAt, New IntPtr(clng(_ptr) + (index * 4)))
            // intAtset(New IntPtr(clng(_ptr) + (index * 4)), value)
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
            // uintAtget(UIntegerAt, New IntPtr(clng(_ptr) + (index * 4)))
            // uintAtset(New IntPtr(clng(_ptr) + (index * 4)), value)
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
            // longAtget(LongAt, New IntPtr(clng(_ptr) + (index * 8)))
            // longAtset(New IntPtr(clng(_ptr) + (index * 8)), value)
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
            // ulongAtget(ULongAt, New IntPtr(clng(_ptr) + (index * 8)))
            // ulongAtset(New IntPtr(clng(_ptr) + (index * 8)), value)
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
            // singleAtget(SingleAt, New IntPtr(CLng(_ptr) + (index * 4)))
            // singleAtset(New IntPtr(CLng(_ptr) + (index * 4)), value)
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
            // doubleAtget(DoubleAt, New IntPtr(CLng(_ptr) + (index * 8)))
            // doubleAtset(New IntPtr(CLng(_ptr) + (index * 8)), value)
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
            // decimalAtget(DecimalAt, New IntPtr(CLng(_ptr) + (index * 16)))
            // decimalAtset(New IntPtr(CLng(_ptr) + (index * 16)), value)
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
            // charAtget(CharAtAbsolute, New IntPtr(clng(_ptr) + index))
            // charAtset(New IntPtr(clng(_ptr) + index), value)
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
            // shortAtget(ShortAtAbsolute, New IntPtr(clng(_ptr) + (index)))
            // shortAtset(New IntPtr(clng(_ptr) + (index)), value)
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
            // ushortAtget(UShortAtAbsolute, New IntPtr(clng(_ptr) + (index)))
            // ushortAtset(New IntPtr(clng(_ptr) + (index)), value)
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
            // intAtget(IntegerAtAbsolute, New IntPtr(clng(_ptr) + (index)))
            // intAtset(New IntPtr(clng(_ptr) + (index)), value)
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
            // uintAtget(UIntegerAtAbsolute, New IntPtr(clng(_ptr) + (index)))
            // uintAtset(New IntPtr(clng(_ptr) + (index)), value)
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
            // longAtget(LongAtAbsolute, New IntPtr(clng(_ptr) + (index)))
            // longAtset(New IntPtr(clng(_ptr) + (index)), value)
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
            // ulongAtget(ULongAtAbsolute, New IntPtr(clng(_ptr) + (index)))
            // ulongAtset(New IntPtr(clng(_ptr) + (index)), value)
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
            // singleAtget(SingleAtAbsolute, New IntPtr(CLng(_ptr) + (index)))
            // singleAtset(New IntPtr(CLng(_ptr) + (index)), value)
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
            // doubleAtget(DoubleAtAbsolute, New IntPtr(CLng(_ptr) + (index)))
            // doubleAtset(New IntPtr(CLng(_ptr) + (index)), value)
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
            // decimalAtget(DecimalAtAbsolute, New IntPtr(CLng(_ptr) + (index)))
            // decimalAtset(New IntPtr(CLng(_ptr) + (index)), value)
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
        public bool Reverse(bool asChar = false)
        {
            if (_ptr == (IntPtr)0)
                return false;
            long l = Length();
            long i = 0L;
            long j;
            char ch;
            byte b;
            if (asChar)
            {
                l = l >> 1;
                j = l - 1L;
                do
                {
                    ch = get_CharAt(i);
                    set_CharAt(i, get_CharAt(j));
                    set_CharAt(j, ch);
                    i += 1L;
                    j -= 1L;
                }
                while (i < l);
            }
            else
            {
                j = l - 1L;
                do
                {
                    b = this[i];
                    this[i] = this[j];
                    this[j] = b;
                    i += 1L;
                    j -= 1L;
                }
                while (i < l);
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
            if (offset == 0L)
                return;
            long hl = Length();
            if (hl <= 0L)
                return;
            if (0L > index + length + offset || index + length + offset > hl)
            {
                throw new IndexOutOfRangeException("Index out of bounds DataTools.Memory.MemPtr.Slide().");
                return;
            }

            // ' if it's short and sweet, let's make it short and sweet
            // ' no need to call p/Invoke ...
            if (length == 1L)
            {
                if (offset == 1L || offset == -1)
                {
                    byte ch;
                    ch = this[index];
                    this[index] = this[index + offset];
                    this[index + offset] = ch;
                    return;
                }
            }
            else if (length == 2L)
            {
                if (offset == 2L || offset == -2)
                {
                    char ch;
                    ch = get_CharAtAbsolute(index);
                    set_CharAtAbsolute(index, get_CharAtAbsolute(index + offset));
                    set_CharAtAbsolute(index + offset, ch);
                    return;
                }
            }
            else if (length == 4L)
            {
                if (offset == 4L || offset == -4)
                {
                    int ch;
                    ch = get_IntegerAtAbsolute(index);
                    set_IntegerAtAbsolute(index, get_IntegerAtAbsolute(index + offset));
                    set_IntegerAtAbsolute(index + offset, ch);
                    return;
                }
            }
            else if (length == 8L)
            {
                if (offset == 8L || offset == -8)
                {
                    long ch;
                    ch = get_LongAtAbsolute(index);
                    set_LongAtAbsolute(index, get_LongAtAbsolute(index + offset));
                    set_LongAtAbsolute(index + offset, ch);
                    return;
                }
            }

            IntPtr src;
            IntPtr dest;
            src = _ptr + (int)index;
            dest = _ptr + (int)index + (int)offset;
            long a = offset < 0L ? offset * -1 : offset;
            var buff = new MemPtr(length + a);
            var chunk = buff + length;
            Native.MemCpy(buff._ptr, src, (uint)length);
            Native.MemCpy(chunk._ptr, dest, (uint)a);
            src = _ptr + (int)index + (int)offset + (int)length;
            Native.MemCpy(src, chunk._ptr, (uint)a);
            Native.MemCpy(dest, buff._ptr, (uint)length);
            buff.Free();
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
            long hl = Length();
            if (Length() == 0L || 0L > index || index >= hl - 1L)
            {
                throw new IndexOutOfRangeException("Index out of bounds DataTools.Memory.MemPtr.PullIn().");
                return -1;
            }

            long a = index + amount;
            long b = Length() - a;
            Slide(a, b, -amount);
            ReAlloc(hl - amount);
            return Length();
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
            long PushOutRet = default;
            long hl = Length();
            if (hl <= 0L)
            {
                SetLength(amount);
                PushOutRet = amount;
                return PushOutRet;
            }

            if (0L > index)
            {
                throw new IndexOutOfRangeException("Index out of bounds DataTools.Memory.MemPtr.PushOut().");
                return -1;
            }

            long ol = Length() - index;
            ReAlloc(hl + amount);
            if (ol > 0L)
            {
                Slide(index, ol, amount);
            }

            if (bytes is object)
            {
                SetByteArray((IntPtr)index, bytes);
            }
            else
            {
                ZeroMemory(index, amount);
            }

            return Length();
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
            return PullIn(index << 1, amount << 1);
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
            return PushOut(index << 1, amount << 1, Native.ToBytes(chars));
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
            if (_ptr == (IntPtr)0)
            {
                SetLength(amount);
                return;
            }

            long l = Length();
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
            long hl = Length();
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
        public void ConsumeChar(long index, long amount, bool removePressure = false)
        {
            long hl = Length();
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

        // ' Because when setting the memory value we cannot rely on CTypes when private heaps are
        // ' being referenced, the only way to ensure that a private heap can initialize this
        // ' structure is to provide a way for an external caller to specify the private heap.

        // ' There is no equivalent 'getter' function for two reasons:
        // ' First:
        // ' It is not possible to marshal a late-bound object into CopyMemory.
        // ' Second:
        // ' Even if it were, the getter is not necessary because the CType can return
        // ' a managed object without allocating or deallocating resources.
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Set the buffer with the specified supported primitive object
        /// </summary>
        /// <param name="value">Object to copy to the buffer.</param>
        /// <param name="addPressure">Specify whether to tell the garbage collector.</param>
        /// <param name="hHeap">Specify an optional alternate heap.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool SetIntegral(object value, bool addPressure = false, IntPtr? hHeap = default)
        {
            bool SetIntegralRet = default;
            long l = _checkType(value.GetType());
            if (l == 0L)
                return false;
            if (value.GetType().IsArray)
            {
                l *= ((Array)value).Length;
            }
            else if (value is string)
            {
                l *= Conversions.ToString(value).Length + 1;
            }

            if (!Alloc(l, addPressure, hHeap))
            {
                throw new InsufficientMemoryException();
            }
            else
            {
                ZeroMemory();
            }

            switch (value.GetType())
            {
                case var @case when @case == typeof(sbyte):
                    {
                        sbyte argsrc = Conversions.ToSByte(value);
                        Native.sbyteAtset(_ptr, ref argsrc);
                        break;
                    }

                case var case1 when case1 == typeof(byte):
                    {
                        byte argsrc1 = Conversions.ToByte(value);
                        Native.byteAtset(_ptr, ref argsrc1);
                        break;
                    }

                case var case2 when case2 == typeof(char):
                    {
                        char argsrc2 = Conversions.ToChar(value);
                        Native.charAtset(_ptr, ref argsrc2);
                        break;
                    }

                case var case3 when case3 == typeof(short):
                    {
                        short argsrc3 = Conversions.ToShort(value);
                        Native.shortAtset(_ptr, ref argsrc3);
                        break;
                    }

                case var case4 when case4 == typeof(ushort):
                    {
                        ushort argsrc4 = Conversions.ToUShort(value);
                        Native.ushortAtset(_ptr, ref argsrc4);
                        break;
                    }

                case var case5 when case5 == typeof(int):
                    {
                        int argsrc5 = Conversions.ToInteger(value);
                        Native.intAtset(_ptr, ref argsrc5);
                        break;
                    }

                case var case6 when case6 == typeof(uint):
                    {
                        uint argsrc6 = Conversions.ToUInteger(value);
                        Native.uintAtset(_ptr, ref argsrc6);
                        break;
                    }

                case var case7 when case7 == typeof(long):
                    {
                        long argsrc7 = Conversions.ToLong(value);
                        Native.longAtset(_ptr, ref argsrc7);
                        break;
                    }

                case var case8 when case8 == typeof(ulong):
                    {
                        ulong argsrc8 = Conversions.ToULong(value);
                        Native.ulongAtset(_ptr, ref argsrc8);
                        break;
                    }

                case var case9 when case9 == typeof(float):
                    {
                        float argsrc9 = Conversions.ToSingle(value);
                        Native.singleAtset(_ptr, ref argsrc9);
                        break;
                    }

                case var case10 when case10 == typeof(double):
                    {
                        double argsrc10 = Conversions.ToDouble(value);
                        Native.doubleAtset(_ptr, ref argsrc10);
                        break;
                    }

                case var case11 when case11 == typeof(DateTime):
                    {
                        DateTime argsrc11 = Conversions.ToDate(value);
                        Native.dateAtset(_ptr, ref argsrc11);
                        break;
                    }

                case var case12 when case12 == typeof(Color):
                    {
                        Native.CopyMemory(_ptr, (Color)value, l);
                        break;
                    }

                case var case13 when case13 == typeof(Guid):
                    {
                        Guid argsrc12 = (Guid)value;
                        Native.guidAtset(_ptr, ref argsrc12);
                        break;
                    }

                case var case14 when case14 == typeof(decimal):
                    {
                        decimal argsrc13 = Conversions.ToDecimal(value);
                        Native.decimalAtset(_ptr, ref argsrc13);
                        break;
                    }

                case var case15 when case15 == typeof(sbyte[]):
                    {
                        Native.sbyteArrAtset(_ptr, (sbyte[])value, (uint)l);
                        break;
                    }

                case var case16 when case16 == typeof(byte[]):
                    {
                        Native.byteArrAtset(_ptr, (byte[])value, (uint)l);
                        break;
                    }

                case var case17 when case17 == typeof(char[]):
                    {
                        Native.charArrAtset(_ptr, Conversions.ToCharArrayRankOne(value), (uint)l);
                        break;
                    }

                case var case18 when case18 == typeof(short[]):
                    {
                        Native.shortArrAtset(_ptr, (short[])value, (uint)l);
                        break;
                    }

                case var case19 when case19 == typeof(ushort[]):
                    {
                        Native.ushortArrAtset(_ptr, (ushort[])value, (uint)l);
                        break;
                    }

                case var case20 when case20 == typeof(int[]):
                    {
                        Native.intArrAtset(_ptr, (int[])value, (uint)l);
                        break;
                    }

                case var case21 when case21 == typeof(uint[]):
                    {
                        Native.uintArrAtset(_ptr, (uint[])value, (uint)l);
                        break;
                    }

                case var case22 when case22 == typeof(long[]):
                    {
                        Native.longArrAtset(_ptr, (long[])value, (uint)l);
                        break;
                    }

                case var case23 when case23 == typeof(ulong[]):
                    {
                        Native.ulongArrAtset(_ptr, (ulong[])value, (uint)l);
                        break;
                    }

                case var case24 when case24 == typeof(float[]):
                    {
                        Native.singleArrAtset(_ptr, (float[])value, (uint)l);
                        break;
                    }

                case var case25 when case25 == typeof(double[]):
                    {
                        Native.doubleArrAtset(_ptr, (double[])value, (uint)l);
                        break;
                    }

                case var case26 when case26 == typeof(DateTime[]):
                    {
                        Native.dateArrAtset(_ptr, (DateTime[])value, (uint)l);
                        break;
                    }

                case var case27 when case27 == typeof(Color[]):
                    {
                        Native.CopyMemory(_ptr, (Color[])value, l);
                        break;
                    }

                case var case28 when case28 == typeof(Guid[]):
                    {
                        Native.guidArrAtset(_ptr, (Guid[])value, (uint)l);
                        break;
                    }

                case var case29 when case29 == typeof(decimal[]):
                    {
                        Native.decimalArrAtset(_ptr, (decimal[])value, (uint)l);
                        break;
                    }

                case var case30 when case30 == typeof(string):
                    {
                        QuickCopyObject<string>(_ptr, Conversions.ToString(value), (uint)(l - 2L));
                        break;
                    }
            }

            SetIntegralRet = true;
            return SetIntegralRet;
        }

        /// <summary>
        /// Check the type against the cached system types that are accepted as input.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private int _checkType(Type type)
        {
            foreach (var kv in _primitiveCache)
            {
                if (kv.Key == type)
                {
                    return kv.Value;
                }
            }

            return 0;
        }

        private static void _buildIntegralCache()
        {

            // ' build atomic primitive type cache
            _primitiveCache.Add(new KeyValuePair<Type, int>(typeof(sbyte), 1));
            _primitiveCache.Add(new KeyValuePair<Type, int>(typeof(byte), 1));
            _primitiveCache.Add(new KeyValuePair<Type, int>(typeof(char), 2));
            _primitiveCache.Add(new KeyValuePair<Type, int>(typeof(short), 2));
            _primitiveCache.Add(new KeyValuePair<Type, int>(typeof(ushort), 2));
            _primitiveCache.Add(new KeyValuePair<Type, int>(typeof(int), 4));
            _primitiveCache.Add(new KeyValuePair<Type, int>(typeof(uint), 4));
            _primitiveCache.Add(new KeyValuePair<Type, int>(typeof(long), 8));
            _primitiveCache.Add(new KeyValuePair<Type, int>(typeof(ulong), 8));
            _primitiveCache.Add(new KeyValuePair<Type, int>(typeof(float), 4));
            _primitiveCache.Add(new KeyValuePair<Type, int>(typeof(double), 8));
            _primitiveCache.Add(new KeyValuePair<Type, int>(typeof(DateTime), 8));
            _primitiveCache.Add(new KeyValuePair<Type, int>(typeof(Color), 4));
            _primitiveCache.Add(new KeyValuePair<Type, int>(typeof(Guid), 16));
            _primitiveCache.Add(new KeyValuePair<Type, int>(typeof(decimal), 16));

            // ' build array primitive type cache
            _primitiveCache.Add(new KeyValuePair<Type, int>(typeof(sbyte[]), 1));
            _primitiveCache.Add(new KeyValuePair<Type, int>(typeof(byte[]), 1));
            _primitiveCache.Add(new KeyValuePair<Type, int>(typeof(char[]), 2));
            _primitiveCache.Add(new KeyValuePair<Type, int>(typeof(short[]), 2));
            _primitiveCache.Add(new KeyValuePair<Type, int>(typeof(ushort[]), 2));
            _primitiveCache.Add(new KeyValuePair<Type, int>(typeof(int[]), 4));
            _primitiveCache.Add(new KeyValuePair<Type, int>(typeof(uint[]), 4));
            _primitiveCache.Add(new KeyValuePair<Type, int>(typeof(long[]), 8));
            _primitiveCache.Add(new KeyValuePair<Type, int>(typeof(ulong[]), 8));
            _primitiveCache.Add(new KeyValuePair<Type, int>(typeof(float[]), 4));
            _primitiveCache.Add(new KeyValuePair<Type, int>(typeof(double[]), 8));
            _primitiveCache.Add(new KeyValuePair<Type, int>(typeof(DateTime[]), 8));
            _primitiveCache.Add(new KeyValuePair<Type, int>(typeof(Color[]), 4));
            _primitiveCache.Add(new KeyValuePair<Type, int>(typeof(Guid[]), 16));
            _primitiveCache.Add(new KeyValuePair<Type, int>(typeof(decimal[]), 16));

            // ' string primitive
            _primitiveCache.Add(new KeyValuePair<Type, int>(typeof(string), 2));
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
        // ' Math with other MemPtr
        public static MemPtr operator +(MemPtr operand1, MemPtr operand2)
        {
            operand1._ptr = new IntPtr((long)operand1._ptr + (long)operand2._ptr);
            return operand1;
        }

        public static MemPtr operator -(MemPtr operand1, MemPtr operand2)
        {
            operand1._ptr = new IntPtr((long)operand1._ptr - (long)operand2._ptr);
            return operand1;
        }

        public static MemPtr operator *(MemPtr operand1, MemPtr operand2)
        {
            operand1._ptr = new IntPtr((long)operand1._ptr * (long)operand2._ptr);
            return operand1;
        }

        public static MemPtr operator /(MemPtr operand1, MemPtr operand2)
        {
            operand1._ptr = new IntPtr((long)((long)operand1._ptr / (double)(long)operand2._ptr));
            return operand1;
        }

        public static MemPtr operator /(MemPtr operand1, MemPtr operand2)
        {
            operand1._ptr = new IntPtr((long)operand1._ptr / (long)operand2._ptr);
            return operand1;
        }


        // ' Math with IntPtr
        public static MemPtr operator +(MemPtr operand1, IntPtr operand2)
        {
            operand1._ptr = new IntPtr((long)operand1._ptr + operand2.ToInt64());
            return operand1;
        }

        public static MemPtr operator -(MemPtr operand1, IntPtr operand2)
        {
            operand1._ptr = new IntPtr((long)operand1._ptr - operand2.ToInt64());
            return operand1;
        }

        public static MemPtr operator *(MemPtr operand1, IntPtr operand2)
        {
            operand1._ptr = new IntPtr((long)operand1._ptr * operand2.ToInt64());
            return operand1;
        }

        public static MemPtr operator /(MemPtr operand1, IntPtr operand2)
        {
            operand1._ptr = new IntPtr((long)((long)operand1._ptr / (double)operand2.ToInt64()));
            return operand1;
        }

        public static MemPtr operator /(MemPtr operand1, IntPtr operand2)
        {
            operand1._ptr = new IntPtr((long)operand1._ptr / operand2.ToInt64());
            return operand1;
        }



        // ' math with UIntPtr
        public static MemPtr operator +(MemPtr operand1, UIntPtr operand2)
        {
            operand1._ptr = new IntPtr((long)operand1._ptr + (long)(ulong)operand2);
            return operand1;
        }

        public static MemPtr operator -(MemPtr operand1, UIntPtr operand2)
        {
            operand1._ptr = new IntPtr((long)operand1._ptr - (long)(ulong)operand2);
            return operand1;
        }

        public static MemPtr operator *(MemPtr operand1, UIntPtr operand2)
        {
            operand1._ptr = new IntPtr((long)operand1._ptr * (long)(ulong)operand2);
            return operand1;
        }

        public static MemPtr operator /(MemPtr operand1, UIntPtr operand2)
        {
            operand1._ptr = new IntPtr((long)((long)operand1._ptr / (double)(long)(ulong)operand2));
            return operand1;
        }

        public static MemPtr operator /(MemPtr operand1, UIntPtr operand2)
        {
            operand1._ptr = new IntPtr((long)operand1._ptr / (long)(ulong)operand2);
            return operand1;
        }


        // ' Signed ordinals

        // ' we can add long ints to the _ptr 
        public static MemPtr operator +(MemPtr operand1, long operand2)
        {
            operand1._ptr = new IntPtr((long)operand1._ptr + operand2);
            return operand1;
        }

        public static MemPtr operator -(MemPtr operand1, long operand2)
        {
            operand1._ptr = new IntPtr((long)operand1._ptr - operand2);
            return operand1;
        }

        public static MemPtr operator *(MemPtr operand1, long operand2)
        {
            operand1._ptr = new IntPtr((long)operand1._ptr * operand2);
            return operand1;
        }

        public static MemPtr operator /(MemPtr operand1, long operand2)
        {
            operand1._ptr = new IntPtr((long)((long)operand1._ptr / (double)operand2));
            return operand1;
        }

        public static MemPtr operator /(MemPtr operand1, long operand2)
        {
            operand1._ptr = new IntPtr((long)operand1._ptr / operand2);
            return operand1;
        }

        // ' we can add ints to the _ptr 
        public static MemPtr operator +(MemPtr operand1, int operand2)
        {
            operand1._ptr = new IntPtr((long)operand1._ptr + operand2);
            return operand1;
        }

        public static MemPtr operator -(MemPtr operand1, int operand2)
        {
            operand1._ptr = new IntPtr((long)operand1._ptr - operand2);
            return operand1;
        }

        public static MemPtr operator *(MemPtr operand1, int operand2)
        {
            operand1._ptr = new IntPtr((long)operand1._ptr * operand2);
            return operand1;
        }

        public static MemPtr operator /(MemPtr operand1, int operand2)
        {
            operand1._ptr = new IntPtr((long)((long)operand1._ptr / (double)operand2));
            return operand1;
        }

        public static MemPtr operator /(MemPtr operand1, int operand2)
        {
            operand1._ptr = new IntPtr((long)operand1._ptr / operand2);
            return operand1;
        }


        // ' we can add shorts to the _ptr 
        public static MemPtr operator +(MemPtr operand1, short operand2)
        {
            operand1._ptr = new IntPtr((long)operand1._ptr + operand2);
            return operand1;
        }

        public static MemPtr operator -(MemPtr operand1, short operand2)
        {
            operand1._ptr = new IntPtr((long)operand1._ptr - operand2);
            return operand1;
        }

        public static MemPtr operator *(MemPtr operand1, short operand2)
        {
            operand1._ptr = new IntPtr((long)operand1._ptr * operand2);
            return operand1;
        }

        public static MemPtr operator /(MemPtr operand1, short operand2)
        {
            operand1._ptr = new IntPtr((long)((long)operand1._ptr / (double)operand2));
            return operand1;
        }

        public static MemPtr operator /(MemPtr operand1, short operand2)
        {
            operand1._ptr = new IntPtr((long)operand1._ptr / operand2);
            return operand1;
        }


        // ' we can add signed sbytes to the _ptr 
        public static MemPtr operator +(MemPtr operand1, sbyte operand2)
        {
            operand1._ptr = new IntPtr((long)operand1._ptr + operand2);
            return operand1;
        }

        public static MemPtr operator -(MemPtr operand1, sbyte operand2)
        {
            operand1._ptr = new IntPtr((long)operand1._ptr - operand2);
            return operand1;
        }

        public static MemPtr operator *(MemPtr operand1, sbyte operand2)
        {
            operand1._ptr = new IntPtr((long)operand1._ptr * operand2);
            return operand1;
        }

        public static MemPtr operator /(MemPtr operand1, sbyte operand2)
        {
            operand1._ptr = new IntPtr((long)((long)operand1._ptr / (double)operand2));
            return operand1;
        }

        public static MemPtr operator /(MemPtr operand1, sbyte operand2)
        {
            operand1._ptr = new IntPtr((long)operand1._ptr / operand2);
            return operand1;
        }


        // ' Unsigned ordinals

        // ' we can add ulong ints to the _ptr 
        public static MemPtr operator +(MemPtr operand1, ulong operand2)
        {
            operand1._ptr = new IntPtr((long)operand1._ptr + (long)operand2);
            return operand1;
        }

        public static MemPtr operator -(MemPtr operand1, ulong operand2)
        {
            operand1._ptr = new IntPtr((long)operand1._ptr - (long)operand2);
            return operand1;
        }

        public static MemPtr operator *(MemPtr operand1, ulong operand2)
        {
            operand1._ptr = new IntPtr((long)operand1._ptr * (long)operand2);
            return operand1;
        }

        public static MemPtr operator /(MemPtr operand1, ulong operand2)
        {
            operand1._ptr = new IntPtr((long)((long)operand1._ptr / (double)(long)operand2));
            return operand1;
        }

        public static MemPtr operator /(MemPtr operand1, ulong operand2)
        {
            operand1._ptr = new IntPtr((long)operand1._ptr / (long)operand2);
            return operand1;
        }


        // ' we can add uints to the _ptr 
        public static MemPtr operator +(MemPtr operand1, uint operand2)
        {
            operand1._ptr = new IntPtr((long)operand1._ptr + operand2);
            return operand1;
        }

        public static MemPtr operator -(MemPtr operand1, uint operand2)
        {
            operand1._ptr = new IntPtr((long)operand1._ptr - operand2);
            return operand1;
        }

        public static MemPtr operator *(MemPtr operand1, uint operand2)
        {
            operand1._ptr = new IntPtr((long)operand1._ptr * operand2);
            return operand1;
        }

        public static MemPtr operator /(MemPtr operand1, uint operand2)
        {
            operand1._ptr = new IntPtr((long)((long)operand1._ptr / (double)operand2));
            return operand1;
        }

        public static MemPtr operator /(MemPtr operand1, uint operand2)
        {
            operand1._ptr = new IntPtr((long)operand1._ptr / operand2);
            return operand1;
        }


        // ' we can add ushorts to the _ptr 
        public static MemPtr operator +(MemPtr operand1, ushort operand2)
        {
            operand1._ptr = new IntPtr((long)operand1._ptr + operand2);
            return operand1;
        }

        public static MemPtr operator -(MemPtr operand1, ushort operand2)
        {
            operand1._ptr = new IntPtr((long)operand1._ptr - operand2);
            return operand1;
        }

        public static MemPtr operator *(MemPtr operand1, ushort operand2)
        {
            operand1._ptr = new IntPtr((long)operand1._ptr * operand2);
            return operand1;
        }

        public static MemPtr operator /(MemPtr operand1, ushort operand2)
        {
            operand1._ptr = new IntPtr((long)((long)operand1._ptr / (double)operand2));
            return operand1;
        }

        public static MemPtr operator /(MemPtr operand1, ushort operand2)
        {
            operand1._ptr = new IntPtr((long)operand1._ptr / operand2);
            return operand1;
        }


        // ' we can add bytes to the _ptr 
        public static MemPtr operator +(MemPtr operand1, byte operand2)
        {
            operand1._ptr = new IntPtr((long)operand1._ptr + operand2);
            return operand1;
        }

        public static MemPtr operator -(MemPtr operand1, byte operand2)
        {
            operand1._ptr = new IntPtr((long)operand1._ptr - operand2);
            return operand1;
        }

        public static MemPtr operator *(MemPtr operand1, byte operand2)
        {
            operand1._ptr = new IntPtr((long)operand1._ptr * operand2);
            return operand1;
        }

        public static MemPtr operator /(MemPtr operand1, byte operand2)
        {
            operand1._ptr = new IntPtr((long)((long)operand1._ptr / (double)operand2));
            return operand1;
        }

        public static MemPtr operator /(MemPtr operand1, byte operand2)
        {
            operand1._ptr = new IntPtr((long)operand1._ptr / operand2);
            return operand1;
        }


        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        // ' MemPtr
        public override bool Equals(object obj)
        {
            if (obj is MemPtr)
            {
                return ((MemPtr)obj)._ptr == _ptr;
            }
            else if (obj is IntPtr)
            {
                return (IntPtr)obj == _ptr;
            }
            else if (obj is UIntPtr)
            {
                return Native.ToSigned((UIntPtr)obj) == _ptr;
            }
            else
            {
                return false;
            }
        }

        public bool Equals(MemPtr other)
        {
            return other._ptr == _ptr;
        }

        public bool Equals(IntPtr other)
        {
            return other == _ptr;
        }

        public bool Equals(UIntPtr other)
        {
            return Native.ToSigned(other) == _ptr;
        }

        public static bool operator ==(MemPtr v1, MemPtr v2)
        {
            return (long)v1._ptr == (long)v2._ptr;
        }

        public static bool operator !=(MemPtr v1, MemPtr v2)
        {
            return (long)v1._ptr != (long)v2._ptr;
        }

        public static bool operator <(MemPtr v1, MemPtr v2)
        {
            return (long)v1._ptr < (long)v2._ptr;
        }

        public static bool operator >(MemPtr v1, MemPtr v2)
        {
            return (long)v1._ptr > (long)v2._ptr;
        }

        public static bool operator <=(MemPtr v1, MemPtr v2)
        {
            return (long)v1._ptr <= (long)v2._ptr;
        }

        public static bool operator >=(MemPtr v1, MemPtr v2)
        {
            return (long)v1._ptr >= (long)v2._ptr;
        }

        // ' IntPtr
        public static bool operator ==(MemPtr v1, IntPtr v2)
        {
            return (long)v1._ptr == (long)v2;
        }

        public static bool operator !=(MemPtr v1, IntPtr v2)
        {
            return (long)v1._ptr != (long)v2;
        }

        public static bool operator <(MemPtr v1, IntPtr v2)
        {
            return (long)v1._ptr < (long)v2;
        }

        public static bool operator >(MemPtr v1, IntPtr v2)
        {
            return (long)v1._ptr > (long)v2;
        }

        public static bool operator <=(MemPtr v1, IntPtr v2)
        {
            return (long)v1._ptr <= (long)v2;
        }

        public static bool operator >=(MemPtr v1, IntPtr v2)
        {
            return (long)v1._ptr >= (long)v2;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public static implicit operator IntPtr(MemPtr operand)
        {
            return operand._ptr;
        }

        public static implicit operator MemPtr(IntPtr operand)
        {
            return new MemPtr(operand);
        }

        public static explicit operator UIntPtr(MemPtr operand)
        {
            return Native.ToUnsigned(operand._ptr);
        }

        public static explicit operator MemPtr(UIntPtr operand)
        {
            return new MemPtr(operand);
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        // ' in
        public static implicit operator MemPtr(sbyte operand)
        {
            var mm = new MemPtr();
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

        public static implicit operator MemPtr(byte operand)
        {
            var mm = new MemPtr();
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

        public static implicit operator MemPtr(short operand)
        {
            var mm = new MemPtr();
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

        public static implicit operator MemPtr(ushort operand)
        {
            var mm = new MemPtr();
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

        public static implicit operator MemPtr(int operand)
        {
            var mm = new MemPtr();
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

        public static implicit operator MemPtr(uint operand)
        {
            var mm = new MemPtr();
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

        public static implicit operator MemPtr(long operand)
        {
            var mm = new MemPtr();
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

        public static implicit operator MemPtr(ulong operand)
        {
            var mm = new MemPtr();
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

        public static implicit operator MemPtr(float operand)
        {
            var mm = new MemPtr();
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

        public static implicit operator MemPtr(double operand)
        {
            var mm = new MemPtr();
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
        public static implicit operator sbyte(MemPtr operand)
        {
            return operand.get_SByteAt(0L);
        }

        public static implicit operator byte(MemPtr operand)
        {
            return operand[0L];
        }

        public static implicit operator char(MemPtr operand)
        {
            return operand.get_CharAt(0L);
        }

        public static implicit operator short(MemPtr operand)
        {
            return operand.get_ShortAt(0L);
        }

        public static implicit operator ushort(MemPtr operand)
        {
            return operand.get_UShortAt(0L);
        }

        public static implicit operator int(MemPtr operand)
        {
            return operand.get_IntegerAt(0L);
        }

        public static implicit operator uint(MemPtr operand)
        {
            return operand.get_UIntegerAt(0L);
        }

        public static implicit operator long(MemPtr operand)
        {
            return operand.get_LongAt(0L);
        }

        public static implicit operator ulong(MemPtr operand)
        {
            return operand.get_ULongAt(0L);
        }

        public static implicit operator float(MemPtr operand)
        {
            return operand.get_SingleAt(0L);
        }

        public static implicit operator double(MemPtr operand)
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
        public static implicit operator MemPtr(byte[] operand)
        {
            return Empty;
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator MemPtr(sbyte[] operand)
        {
            return Empty;
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator MemPtr(short[] operand)
        {
            return Empty;
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator MemPtr(ushort[] operand)
        {
            return Empty;
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator MemPtr(int[] operand)
        {
            return Empty;
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator MemPtr(uint[] operand)
        {
            return Empty;
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator MemPtr(long[] operand)
        {
            return Empty;
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator MemPtr(ulong[] operand)
        {
            return Empty;
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator MemPtr(float[] operand)
        {
            return Empty;
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator MemPtr(double[] operand)
        {
            return Empty;
        }

        // ' Out
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator sbyte[](MemPtr operand)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator byte[](MemPtr operand)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator short[](MemPtr operand)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator ushort[](MemPtr operand)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator int[](MemPtr operand)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator uint[](MemPtr operand)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator long[](MemPtr operand)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator ulong[](MemPtr operand)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator float[](MemPtr operand)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator double[](MemPtr operand)
        {
            return null;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        // ' Returns a pretty string, based on null-termination logic, instead of
        // ' returning every character in the allocated block.
        public static implicit operator string(MemPtr operand)
        {
            return operand.GrabString((IntPtr)0);
        }

        // ' We add 2 bytes to give us a proper null-terminated string in memory.
        public static implicit operator MemPtr(string operand)
        {
            var mm = new MemPtr();
            mm.SetString((IntPtr)0, operand);
            return mm;

            // Dim i As Integer = operand.Length << 1
            // Dim mm As New MemPtr(i + 2)
            // QuickCopyObject(Of String)(mm.Handle, operand, CUInt(i))
            // Return mm
        }

        // ' Here we return every character in the allocated block.
        [MethodImpl(MethodImplOptions.ForwardRef)]
        public static implicit operator char[](MemPtr operand)
        {
            return null;
        }

        // ' We just set the character information into the memory buffer, verbatim.
        [MethodImpl(MethodImplOptions.ForwardRef)]
        public static implicit operator MemPtr(char[] operand)
        {
            return Empty;
        }

        public static implicit operator MemPtr(string[] operand)
        {
            if (operand is null || operand.Length == 0)
                return Empty;
            var mm = new MemPtr();
            int c = operand.Length - 1;
            long l = 2L;
            for (int i = 0, loopTo = c; i <= loopTo; i++)
                l += (operand[i].Length + 1) * 2;
            if (!mm.Alloc(l))
                return Empty;
            var p = mm._ptr;
            for (int i = 0, loopTo1 = c; i <= loopTo1; i++)
            {
                QuickCopyObject<string>(p, operand[i], Conversions.ToUInteger(operand[i].Length * 2));
                p = p + (operand[i].Length * 2 + 2);
            }

            return mm;
        }

        public static implicit operator string[](MemPtr operand)
        {
            return operand.GrabStringArray((IntPtr)0);
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public static explicit operator MemPtr(Guid operand)
        {
            var mm = new MemPtr();
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
        public static explicit operator MemPtr(Guid[] operand)
        {
            return Empty;
        }

        public static explicit operator Guid(MemPtr operand)
        {
            return operand.get_GuidAt(0L);
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static explicit operator Guid[](MemPtr operand)
        {
            return null;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public static implicit operator MemPtr(decimal operand)
        {
            var mm = new MemPtr();
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
        public static implicit operator MemPtr(decimal[] operand)
        {
            return Empty;
        }

        public static implicit operator decimal(MemPtr operand)
        {
            return operand.get_DecimalAt(0L);
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator decimal[](MemPtr operand)
        {
            return null;
        }


        // ' Color

        public static implicit operator MemPtr(Color operand)
        {
            var mm = new MemPtr();
            if (mm.Alloc(4L))
            {
                mm.set_IntegerAt(0L, operand.ToArgb());
            }
            else
            {
                throw new OutOfMemoryException();
            }

            return mm;
        }

        public static implicit operator MemPtr(Color[] operand)
        {
            var mm = new MemPtr();
            if (mm.Alloc(4 * operand.Length))
            {
                Native.MemCpy(mm._ptr, operand, (uint)(4 * operand.Length));
            }
            else
            {
                throw new OutOfMemoryException();
            }

            return mm;
        }

        public static implicit operator Color(MemPtr operand)
        {
            return Color.FromArgb(operand.get_IntegerAt(0L));
        }

        public static implicit operator Color[](MemPtr operand)
        {
            Color[] g;
            long l = operand.Length();
            long a = l >> 2;
            if (a > int.MaxValue)
                throw new ArgumentOutOfRangeException(MemPtrStrings.MemTooBig);
            g = (Color[])Array.CreateInstance(typeof(long), (int)a);
            Native.MemCpy(g, operand, (uint)l);
            return g;
        }

        // ' DateTime

        public static implicit operator MemPtr(DateTime operand)
        {
            var mm = new MemPtr();
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

        public static implicit operator MemPtr(DateTime[] operand)
        {
            var mm = new MemPtr();
            if (mm.Alloc(8 * operand.Length))
            {
                QuickCopyObject<DateTime[]>(mm.Handle, operand, (uint)(8 * operand.Length));
            }
            else
            {
                throw new OutOfMemoryException();
            }

            return mm;
        }

        public static implicit operator DateTime(MemPtr operand)
        {
            return DateTime.FromBinary(operand.get_LongAt(0L));
        }

        public static implicit operator DateTime[](MemPtr operand)
        {
            DateTime[] clr;
            long l = (long)(operand.Length() / 8Ld);
            if (l > uint.MaxValue)
                throw new InvalidCastException();
            clr = new DateTime[((int)l)];
            QuickCopyObject<DateTime[]>(ref clr, operand, (uint)(l * 8L));
            return clr;
        }


        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        // ' These are the normal (canonical) memory allocation functions.
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Set all bytes in the buffer to zero at the optional index with the optional length.
        /// </summary>
        /// <param name="index">Start position of the buffer to zero, default is 0.</param>
        /// <param name="length">Size of the buffer to zero.  Default is to the end of the buffer.</param>
        /// <remarks></remarks>
        public void ZeroMemory(long index = -1, long length = -1)
        {
            long bl = Length();
            if (bl <= 0L)
                return;
            long p = index == -1 ? 0L : index;
            long l = length == -1 ? bl - p : length;

            // ' The length cannot be greater than the buffer length.
            if (l <= 0L || p + l > bl)
                return;
            var ptr = _ptr + (int)p;
            if (Conversions.ToBoolean(l & 0xFFFFFFFF00000000L))
            {
                Native.n_memset(_ptr, 0, (IntPtr)l);
            }
            else
            {
                Native.MemSet(_ptr, 0, (uint)l);
            }
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
        public bool Alloc(long size, bool addPressure = false, IntPtr? hHeap = default, bool zeroMem = true)
        {
            bool AllocRet = default;
            long l = Length();
            if (hHeap is null)
                hHeap = _procHeap;

            // ' While the function doesn't need to call HeapAlloc, it hasn't necessarily failed, either.
            if (size == l)
            {
                AllocRet = true;
                return AllocRet;
            }

            if (l > 0L)
            {
                // ' we already have a pointer, so we will call realloc, instead.
                AllocRet = ReAlloc(size);
                return AllocRet;
            }

            _ptr = Native.HeapAlloc((IntPtr)hHeap, (uint)(zeroMem ? 8 : 0), Native.CIntPtr(size));
            AllocRet = _ptr != (IntPtr)0;

            // ' see if we need to tell the garbage collector anything.
            if (AllocRet && addPressure)
            {
                GC.AddMemoryPressure(Length());
            }

            return AllocRet;
        }

        public bool AllocCoTaskMem(int size)
        {
            _ptr = Marshal.AllocCoTaskMem(size);
            return _ptr != (IntPtr)0;
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
            return Alloc(size, addPressure, default, true);
        }


        /// <summary>
        /// Allocate a block of memory on the process heap.
        /// </summary>
        /// <param name="size">The size to attempt to allocate</param>
        /// <returns>True if successful. If False, call GetLastError or FormatLastError to find out more information.</returns>
        /// <remarks></remarks>
        public bool Alloc(long size)
        {
            return Alloc(size, false, default, true);
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
        public bool AllocZero(long size, bool addPressure = false, IntPtr? hHeap = default)
        {
            bool AllocZeroRet = default;
            AllocZeroRet = Alloc(size, addPressure, hHeap, true);
            return AllocZeroRet;
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
        public bool AlignedAlloc(long size, long alignment = 512L, bool addPressure = false, IntPtr? hHeap = default)
        {
            if (alignment == 0L || (alignment & 1L) != 0L)
            {
                return false;
            }

            if (_ptr != (IntPtr)0)
            {
                if (!Free())
                    return false;
            }

            long l = size + (alignment - 1L) + 8L;
            if (hHeap is null)
                hHeap = _procHeap;
            if (l < 1L)
                return false;
            var p = Native.HeapAlloc((IntPtr)hHeap, 8U, (IntPtr)l);
            if (p == (IntPtr)0)
                return false;
            IntPtr p2 = (IntPtr)(p.ToInt64() + alignment - 1L + 8L);
            if (p == (IntPtr)0)
                return false;
            p2 = (IntPtr)(p2.ToInt64() - p2.ToInt64() % alignment);
            MemPtr mm = p2;
            mm.set_LongAt(-1, p.ToInt64());
            _ptr = p2;
            if (addPressure)
            {
                GC.AddMemoryPressure(l);
            }

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
        public bool AlignedFree(bool removePressure = false, IntPtr? hHeap = default)
        {
            if (_ptr == (IntPtr)0)
                return true;
            if (hHeap is null)
                hHeap = _procHeap;
            IntPtr p = (IntPtr)get_LongAt(-1);
            long l = Conversions.ToLong(Native.HeapSize((IntPtr)hHeap, 0U, p));
            if (Conversions.ToInteger(Native.HeapFree((IntPtr)hHeap, 0U, p)) != 0)
            {
                if (removePressure)
                {
                    GC.RemoveMemoryPressure(l);
                }

                _ptr = (IntPtr)0;
                return true;
            }
            else
            {
                return false;
            }
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
        public bool ReAlloc(long size, bool modifyPressure = false, IntPtr? hHeap = default)
        {
            bool ReAllocRet = default;
            long l = Length();
            if (hHeap is null)
                hHeap = _procHeap;

            // ' While the function doesn't need to call HeapReAlloc, it hasn't necessarily failed, either.
            if (size == l)
            {
                ReAllocRet = true;
                return ReAllocRet;
            }

            if (l <= 0L)
            {
                // ' we don't have a pointer yet, so we have to call alloc instead.
                ReAllocRet = Alloc(size);
                return ReAllocRet;
            }

            _ptr = Native.HeapReAlloc((IntPtr)hHeap, 8, _ptr, new IntPtr(size));
            ReAllocRet = _ptr != (IntPtr)0;

            // ' see if we need to tell the garbage collector anything.
            if (ReAllocRet && modifyPressure)
            {
                size = Length();
                if (size < l)
                {
                    GC.RemoveMemoryPressure(l - size);
                }
                else
                {
                    GC.AddMemoryPressure(size - l);
                }
            }

            return ReAllocRet;
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
        public bool Free(bool removePressure = false, IntPtr? hHeap = default)
        {
            bool FreeRet = default;
            if (hHeap is null)
                hHeap = _procHeap;
            var l = default(long);

            // ' While the function doesn't need to call HeapFree, it hasn't necessarily failed, either.
            if (_ptr == (IntPtr)0)
            {
                FreeRet = true;
            }
            else
            {
                // ' see if we need to tell the garbage collector anything.
                if (removePressure)
                    l = Length();
                FreeRet = Conversions.ToInteger(Native.HeapFree((IntPtr)hHeap, 0U, _ptr)) != 0;

                // ' see if we need to tell the garbage collector anything.
                if (FreeRet)
                {
                    _ptr = (IntPtr)0;
                    if (removePressure)
                    {
                        GC.RemoveMemoryPressure(l);
                    }
                }
            }

            return FreeRet;
        }

        /// <summary>
        /// Validates whether the pointer referenced by this structure
        /// points to a valid and accessible block of memory.
        /// </summary>
        /// <returns>True if the memory block is valid, or False if the pointer is invalid or zero.</returns>
        /// <remarks></remarks>
        public bool Validate()
        {
            bool ValidateRet = default;
            if (_ptr == (IntPtr)0)
            {
                ValidateRet = false;
                return ValidateRet;
            }

            ValidateRet = Native.HeapValidate(_procHeap, 0U, _ptr);
            return ValidateRet;
        }

        /// <summary>
        /// Frees a previously allocated block of memory on the task heap with LocalFree()
        /// </summary>
        /// <returns>True if successful. If False, call GetLastError or FormatLastError to find out more information.</returns>
        /// <remarks></remarks>
        public bool LocalFree()
        {
            bool LocalFreeRet = default;
            if (_ptr == (IntPtr)0)
            {
                LocalFreeRet = false;
            }
            else
            {
                _ptr = Native.LocalFree(_ptr);
                LocalFreeRet = _ptr != (IntPtr)0;
            }

            return LocalFreeRet;
        }

        /// <summary>
        /// Frees a previously allocated block of memory on the task heap with GlobalFree()
        /// </summary>
        /// <returns>True if successful. If False, call GetLastError or FormatLastError to find out more information.</returns>
        /// <remarks></remarks>
        public bool GlobalFree()
        {
            bool GlobalFreeRet = default;
            if (_ptr == (IntPtr)0)
            {
                GlobalFreeRet = false;
            }
            else
            {
                _ptr = Native.GlobalFree(_ptr);
                GlobalFreeRet = _ptr == (IntPtr)0;
            }

            return GlobalFreeRet;
        }

        /// <summary>
        /// Frees a block of memory previously allocated by COM.
        /// </summary>
        /// <remarks></remarks>
        public void CoTaskMemFree()
        {
            Marshal.FreeCoTaskMem(_ptr);
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        // ' NetApi Memory functions should be used carefully and not within the context
        // ' of any scenario when you may accidentally call normal memory management functions
        // ' on any region of memory allocated with the network memory functions. 
        // ' Be mindful of usage.
        // ' Some normal functions such as Length and SetLength cannot be used.
        // ' Normal allocation and deallocation functions cannot be used, at all.
        // ' NetApi memory is not reallocatable.
        // ' The size of a NetApi memory buffer cannot be retrieved.
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Allocate a network API compatible memory buffer.
        /// </summary>
        /// <param name="size">Size of the buffer to allocate, in bytes.</param>
        /// <remarks></remarks>
        public void NetAlloc(int size)
        {
            // ' just ignore an allocated buffer.
            if (_ptr != (IntPtr)0)
                return;
            Native.NetApiBufferAllocate(size, ref _ptr);
        }

        /// <summary>
        /// Free a network API compatible memory buffer previously allocated with NetAlloc.
        /// </summary>
        /// <remarks></remarks>
        public void NetFree()
        {
            if (_ptr == (IntPtr)0)
                return;
            Native.NetApiBufferFree(_ptr);
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        // ' Virtual Memory should be used carefully and not within the context
        // ' of any scenario when you may accidentally call normal memory management functions
        // ' on any region of memory allocated with the Virtual functions. 
        // ' Be mindful of usage.
        // ' Some normal functions such as Length and SetLength cannot be used (use VirtualLength).
        // ' Normal allocation and deallocation functions cannot be used, at all.
        // ' Virtual memory is not reallocatable.
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Allocates a region of virtual memory.
        /// </summary>
        /// <param name="size">The size of the region of memory to allocate.</param>
        /// <param name="addPressure">Whether to call GC.AddMemoryPressure</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool VirtualAlloc(long size, bool addPressure = true)
        {
            bool VirtualAllocRet = default;
            var l = default(long);

            // ' While the function doesn't need to call VirtualAlloc, it hasn't necessarily failed, either.
            if (size == l && _ptr != (IntPtr)0)
            {
                VirtualAllocRet = true;
                return VirtualAllocRet;
            }

            _ptr = Native.VirtualAlloc((IntPtr)0, (IntPtr)size, VMemAllocFlags.MEM_COMMIT | VMemAllocFlags.MEM_RESERVE, MemoryProtectionFlags.PAGE_READWRITE);
            VirtualAllocRet = _ptr != (IntPtr)0;
            if (VirtualAllocRet && addPressure)
            {
                GC.AddMemoryPressure(VirtualLength());
            }

            return VirtualAllocRet;
        }

        /// <summary>
        /// Frees a region of memory previously allocated with VirtualAlloc.
        /// </summary>
        /// <param name="removePressure">Whether to call GC.RemoveMemoryPressure</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool VirtualFree(bool removePressure = true)
        {
            bool VirtualFreeRet = default;
            var l = default(long);

            // ' While the function doesn't need to call VirtualFree, it hasn't necessarily failed, either.
            if (_ptr == (IntPtr)0)
            {
                VirtualFreeRet = true;
            }
            else
            {
                // ' see if we need to tell the garbage collector anything.
                if (removePressure)
                    l = VirtualLength();
                VirtualFreeRet = Native.VirtualFree(_ptr);

                // ' see if we need to tell the garbage collector anything.
                if (VirtualFreeRet)
                {
                    _ptr = (IntPtr)0;
                    if (removePressure)
                        GC.RemoveMemoryPressure(l);
                }
            }

            return VirtualFreeRet;
        }

        /// <summary>
        /// Returns the size of a region of virtual memory previously allocated with VirtualAlloc.
        /// </summary>
        /// <returns>The size of a virtual memory region or zero.</returns>
        /// <remarks></remarks>
        public long VirtualLength()
        {
            if (_ptr == (IntPtr)0)
                return 0L;
            var m = new MEMORY_BASIC_INFORMATION();
            if (Native.VirtualQuery(_ptr, ref m, (IntPtr)Marshal.SizeOf(m)) != (IntPtr)0)
            {
                return Conversions.ToLong(m.RegionSize);
            }

            return 0L;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        // ' In a structure this needs to be called manually.
        /// <summary>
        /// Free all unmanaged resources.
        /// </summary>
        /// <remarks></remarks>
        public void Dispose()
        {
            Free();
        }

        /// <summary>
        /// Creates an exact copy of the memory associated with this pointer.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public object Clone()
        {
            object CloneRet = default;
            var mm = new MemPtr();
            long l = Length();
            if (l <= 0L)
            {
                CloneRet = mm;
                return CloneRet;
            }

            mm.Alloc(l);
            if (l <= uint.MaxValue)
            {
                Native.MemCpy(mm._ptr, _ptr, (uint)l);
            }
            else
            {
                Native.CopyMemory(mm._ptr, _ptr, (IntPtr)l);
            }

            CloneRet = mm;
            return CloneRet;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        static MemPtr()
        {
            // ' initialize the cache of primitive types that are supported
            _buildIntegralCache();
        }

        /// <summary>
        /// Initialize a new instance of this structure and allocate a memory block
        /// of the specified size.
        /// </summary>
        /// <param name="size">Size of the memory block to allocate.</param>
        /// <remarks></remarks>
        public MemPtr(long size)
        {
            Alloc(size);
        }

        /// <summary>
        /// Initialize a new instance of this structure and allocate a memory block
        /// of the specified size.
        /// </summary>
        /// <param name="size">Size of the memory block to allocate.</param>
        /// <remarks></remarks>
        public MemPtr(int size)
        {
            Alloc(size);
        }

        /// <summary>
        /// Initialize a new instance of this structure with the specified memory pointer.
        /// </summary>
        /// <param name="ptr">Pointer to a block of memory.</param>
        /// <remarks></remarks>
        public MemPtr(UIntPtr ptr)
        {
            _ptr = Native.ToSigned(ptr);
        }

        /// <summary>
        /// Initialize a new instance of this structure with the specified memory pointer.
        /// </summary>
        /// <param name="ptr">Pointer to a block of memory.</param>
        /// <remarks></remarks>
        public MemPtr(IntPtr ptr)
        {
            _ptr = ptr;
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
        public IEnumerator<byte> GetEnumerator()
        {
            IEnumerator<byte> GetEnumeratorRet = default;
            GetEnumeratorRet = new MemPtrEnumeratorByte(this);
            return GetEnumeratorRet;
        }

        public IEnumerator GetEnumerator()
        {
            IEnumerator GetEnumeratorRet = default;
            GetEnumeratorRet = new MemPtrEnumeratorByte(this);
            return GetEnumeratorRet;
        }

        public IEnumerator GetEnumerator1() => GetEnumerator();

        public IEnumerator<char> GetEnumerator()
        {
            IEnumerator<char> GetEnumeratorRet = default;
            GetEnumeratorRet = new MemPtrEnumeratorChar(this);
            return GetEnumeratorRet;
        }

        public IEnumerator<char> GetEnumerator2() => GetEnumerator();

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    } // ' MemPtr

    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    public class MemPtrEnumeratorByte : IEnumerator<byte>
    {
        private MemPtr mm;
        private int pos = -1;

        internal MemPtrEnumeratorByte(MemPtr subj)
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
            if (pos >= mm.Length())
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
                mm = default;
            }

            disposedValue = true;
        }

        ~MemPtrEnumeratorByte()
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

    public class MemPtrEnumeratorChar : IEnumerator<char>
    {
        private MemPtr mm;
        private int pos = -1;

        internal MemPtrEnumeratorChar(MemPtr subj)
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
            if (pos >= mm.Length() >> 1)
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
                mm = default;
            }

            disposedValue = true;
        }

        ~MemPtrEnumeratorChar()
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