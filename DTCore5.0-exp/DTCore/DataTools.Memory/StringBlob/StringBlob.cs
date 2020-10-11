// ' ************************************************* ''
// ' DataTools Visual Basic Utility Library 
// '
// ' Module: StringBlob
// '         Collection of strings in unmanaged memory.
// ' 
// ' Copyright (C) 2011-2020 Nathan Moschkin
// ' All Rights Reserved
// '
// ' Licensed Under the Microsoft Public License   
// ' ************************************************* ''

using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;
using DataTools.ByteOrderMark;
using DataTools.Memory.Internal;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace DataTools.Memory
{

    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    /// <summary>
    /// StringBlob manages unmanaged arrays of strings in memory (either the LPWSTR or BSTR varierty.)
    /// </summary>
    /// <remarks></remarks>
    [StructLayout(LayoutKind.Sequential)]
    public class StringBlob : IEnumerable, ICloneable, IDisposable
    {
        private BOM _BOM;
        private BlobOrdinalTypes _sizeDescriptorType = BlobOrdinalTypes.Integer;
        private int _sizeDescriptorLength = 4;
        private bool _lpwstr = false;
        private SafePtr _mem = new SafePtr();
        private IntPtr[] _index;
        private int _count;
        private bool _dynamic = true;

        /// <summary>
        /// Creates a new empty StringBlob
        /// </summary>
        public StringBlob()
        {
        }

        /// <summary>
        /// Creates a new StringBlob from an array of strings.
        /// </summary>
        /// <param name="strings"></param>
        public StringBlob(string[] strings)
        {
            AddStrings(strings);
        }

        /// <summary>
        /// Creates a new StringBlob and initialize the data with the contents of a Blob object.
        /// </summary>
        /// <param name="blob"></param>
        public StringBlob(Blob blob)
        {
            _mem = new SafePtr(blob.Length);
            _mem.CopyFrom(blob.DangerousGetHandle(), (IntPtr)blob.Length);
        }

        /// <summary>
        /// Gets or sets a value indicating that the string-blob is dynamic.
        /// Dynamic string-blobs always return freshly indexed information on
        /// the number and size of strings. Additionally, the SafePtr backing object
        /// can be hot-swapped.
        /// </summary>
        /// <returns></returns>
        public bool Dynamic
        {
            get
            {
                return _dynamic;
            }

            set
            {
                if (value == _dynamic)
                    return;
                if (value)
                {
                    Refresh();
                    GetCount();
                }
                else
                {
                    _count = -1;
                    _index = null;
                }

                _dynamic = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating that strings are stored as null-terminated strings.
        /// Otherwise, the strings are preambled with a length descriptor.
        /// </summary>
        /// <returns></returns>
        public bool LpWStr
        {
            get
            {
                return _lpwstr;
            }

            set
            {
                if (_lpwstr == value)
                    return;
                int c = Count - 1;
                if (c <= 0)
                {
                    _lpwstr = value;
                    return;
                }

                if (!value)
                {
                    var lpsz = new LPWSTR();
                    var mm = new SafePtr();
                    IntPtr px;
                    mm.Length = c * 2 + SizeDescriptorLength + SizeDescriptorLength;
                    lpsz._ptr = _mem.handle;
                    px = mm.handle;
                    for (int i = 0, loopTo = c; i <= loopTo; i++)
                    {
                        Native.MemCpy(px, lpsz._ptr, (uint)lpsz.Length);
                        px = px + (lpsz.Length + 2);
                        lpsz._ptr = lpsz._ptr + (lpsz.Length * 2 + 2);
                    }

                    GC.SuppressFinalize(lpsz);
                    _mem.Dispose();
                    _mem = mm;
                }
                else
                {
                    var bs = new BSTR();
                    var mm = new SafePtr();
                    IntPtr px;
                    mm.Length = (c + 1) * 2 + 2;
                    bs._preamble = SizeDescriptorLength;
                    bs._ptr = _mem.handle + bs._preamble;
                    px = mm.handle;
                    for (int i = 0, loopTo1 = c; i <= loopTo1; i++)
                    {
                        Native.MemCpy(px, IntPtr.Add(bs._ptr, bs._preamble), (uint)bs.Length);
                        px = px + (bs.Length * 2 + bs._preamble);
                    }

                    GC.SuppressFinalize(bs);
                    _mem.Dispose();
                    _mem = mm;
                }
            }
        }

        /// <summary>
        /// Gets the length, in bytes, of the size descriptor preamble.
        /// Valid values are 2, 4 and 8. A value of 0 will default to 4.
        /// </summary>
        /// <returns></returns>
        public int SizeDescriptorLength
        {
            get
            {
                if (_sizeDescriptorLength == 0)
                {
                    _sizeDescriptorType = BlobOrdinalTypes.Integer;
                    _sizeDescriptorLength = 4;
                }

                return _sizeDescriptorLength;
            }
        }

        /// <summary>
        /// Returns the ordinal type of the size descriptor.
        /// </summary>
        /// <returns></returns>
        public BlobOrdinalTypes SizeDescriptorType
        {
            get
            {
                switch (_sizeDescriptorType)
                {
                    case BlobOrdinalTypes.Byte:
                    case BlobOrdinalTypes.Short:
                    case BlobOrdinalTypes.Integer:
                        {
                            return _sizeDescriptorType;
                        }

                    default:
                        {
                            _sizeDescriptorType = BlobOrdinalTypes.Integer;
                            _sizeDescriptorLength = 4;
                            return _sizeDescriptorType;
                        }
                }
            }

            set
            {
                switch (value)
                {
                    case BlobOrdinalTypes.Byte:
                    case BlobOrdinalTypes.Short:
                    case BlobOrdinalTypes.Integer:
                        {
                            _sizeDescriptorType = value;
                            _sizeDescriptorLength = Blob.BlobTypeSize((BlobTypes)value);
                            break;
                        }

                    default:
                        {
                            _sizeDescriptorType = (BlobOrdinalTypes)BlobTypes.Integer;
                            _sizeDescriptorLength = 4;
                            break;
                        }
                }
            }
        }

        /// <summary>
        /// Gets the count of strings.
        /// </summary>
        /// <returns></returns>
        public int Count
        {
            get
            {
                if (_dynamic)
                {
                    return GetCount();
                }
                else
                {
                    if (!_lpwstr)
                        return (int)BCount();
                    return _count;
                }
            }
        }

        /// <summary>
        /// Truncate the string blob to the specified last index.
        /// </summary>
        /// <param name="lastIndex">Index of last string to retain.</param>
        public void Truncate(int lastIndex)
        {
            if (_index is null)
                Refresh();
            var l = _index[lastIndex] + get_LengthAt(lastIndex) * 2;
            if (_lpwstr)
                l = l + 2;
            else
                l = l + SizeDescriptorLength;
            Array.Resize(ref _index, lastIndex + 1);
            SafePtr.Length = l.ToInt64() - SafePtr.handle.ToInt64();
            _count = _index.Length;
            if (!_lpwstr)
            {
                switch (SizeDescriptorLength)
                {
                    case 2:
                        {
                            _mem.set_UShortAt(0L, (ushort)_count);
                            break;
                        }

                    case 4:
                        {
                            _mem.set_UIntegerAt(0L, (uint)_count);
                            break;
                        }

                    case 8:
                        {
                            _mem.set_ULongAt(0L, (ulong)_count);
                            break;
                        }
                }
            }
        }

        /// <summary>
        /// Returns the BSTR preamble count of strings from the head of the memory buffer.
        /// </summary>
        /// <returns></returns>
        private long BCount()
        {
            if (SafePtr is null || SafePtr.handle == IntPtr.Zero)
                return 0L;
            switch (SizeDescriptorLength)
            {
                case 2:
                    {
                        return SafePtr.get_UShortAt(0L);
                    }

                case 4:
                    {
                        return SafePtr.get_UIntegerAt(0L);
                    }

                case 8:
                    {
                        return (long)SafePtr.get_ULongAt(0L);
                    }

                default:
                    {
                        return 0L;
                    }
            }
        }

        /// <summary>
        /// Returns the length of the buffer of the StringBlob, in bytes.
        /// </summary>
        /// <returns></returns>
        public long ByteLength
        {
            get
            {
                return _mem.Length;
            }
        }

        /// <summary>
        /// Returns the memory handle for the StringBlob
        /// </summary>
        /// <returns></returns>
        public IntPtr Handle
        {
            get
            {
                return _mem.handle;
            }
        }

        /// <summary>
        /// Returns the string at the specified index.
        /// </summary>
        /// <param name="index">The index of the string.</param>
        /// <returns></returns>
        public string this[int index]
        {
            get
            {
                string StringAtRet = default;
                if (index >= Count)
                {
                    throw new ArgumentOutOfRangeException();
                }

                if (_lpwstr)
                {
                    IntPtr lpsz;
                    IntPtr l2;
                    lpsz = _index[index];
                    l2 = lpsz;
                    char ch = '\0';
                    int i = 0;
                    do
                    {
                        Native.charAtget(ref ch, l2);
                        l2 = l2 + 2;
                        i += 1;
                    }
                    while (ch != '\0');
                    StringAtRet = new string('\0', i);
                    QuickCopyObject<string>(ref StringAtRet, lpsz, (uint)(i * 2));
                }
                else
                {
                    IntPtr lpsz;
                    MemPtr mm;
                    lpsz = _index[index];
                    mm = lpsz;
                    var i = default(long);
                    switch (SizeDescriptorLength)
                    {
                        case 2:
                            {
                                i = mm.get_UShortAt(0L);
                                break;
                            }

                        case 4:
                            {
                                i = mm.get_UIntegerAt(0L);
                                break;
                            }

                        case 8:
                            {
                                i = mm.get_LongAt(0L);
                                break;
                            }
                    }

                    lpsz = lpsz + SizeDescriptorLength;
                    StringAtRet = new string('\0', (int)i);
                    QuickCopyObject<string>(ref StringAtRet, lpsz, (uint)(i * 2L));
                }

                return StringAtRet;
            }
            // Set(value As String)
            // ChangeString(index, value)
            // End Set
        }

        /// <summary>
        /// Returns the absolute byte offset of the string at the specified index.
        /// </summary>
        /// <param name="index">The index of the string.</param>
        /// <returns></returns>
        public IntPtr get_ByteIndexOf(int index)
        {
            if (index >= Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (_dynamic)
                Refresh();
            return _index[index];
        }

        /// <summary>
        /// Recounts the strings and updates all preamble data and metadata.
        /// </summary>
        /// <returns>An array of IntPtr's to the absolute memory addresses of all strings.</returns>
        public IntPtr[] Refresh()
        {
            IntPtr[] RefreshRet = default;
            int c = GetCount();
            IntPtr[] by;
            by = new IntPtr[c];
            if (_lpwstr)
            {
                var lpsz = new LPWSTR();
                lpsz._ptr = _mem;
                for (int l = 0, loopTo = c - 1; l <= loopTo; l++)
                {
                    by[l] = (IntPtr)lpsz._ptr.ToInt64();
                    lpsz._ptr = lpsz._ptr + (lpsz.Length * 2 + 2);
                }

                RefreshRet = by;
                GC.SuppressFinalize(lpsz);
            }
            else
            {
                var lpsz = new BSTR();
                lpsz._preamble = SizeDescriptorLength;
                lpsz._ptr = _mem.handle + SizeDescriptorLength;
                for (int l = 0, loopTo1 = c - 1; l <= loopTo1; l++)
                {
                    by[l] = (IntPtr)lpsz._ptr.ToInt64();
                    lpsz._ptr = lpsz._ptr + (lpsz.Length * 2 + lpsz._preamble);
                }

                RefreshRet = by;
                GC.SuppressFinalize(lpsz);
            }

            _index = by;
            return RefreshRet;
        }

        /// <summary>
        /// Returns the current count of strings, walking the buffer, if necessary.
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {
            int GetCountRet = default;
            int l = 0;
            if (SafePtr.handle == IntPtr.Zero)
                return 0;
            if (_lpwstr)
            {
                var lpsz = new LPWSTR();
                MemPtr m;
                lpsz._ptr = _mem.handle;
                do
                {
                    if (Conversions.ToBoolean(lpsz.Length))
                        l += 1;
                    lpsz._ptr = lpsz._ptr + (lpsz.Length * 2 + 2);
                    m = lpsz._ptr;
                    if (m.get_ShortAt(0L) == 0)
                        break;
                }
                while (true);
                GC.SuppressFinalize(lpsz);
            }
            else
            {
                switch (SizeDescriptorLength)
                {
                    case 2:
                        {
                            l = _mem.get_UShortAt(0L);
                            break;
                        }

                    case 4:
                        {
                            l = (int)_mem.get_UIntegerAt(0L);
                            break;
                        }

                    case 8:
                        {
                            l = (int)_mem.get_LongAt(0L);
                            break;
                        }
                }
            }

            GetCountRet = l;
            _count = l;
            return GetCountRet;
        }

        /// <summary>
        /// Returns an array of all strings in the StringBlob
        /// </summary>
        /// <returns></returns>
        public string[] Strings
        {
            get
            {
                return ToStringArray(this);
            }
        }

        /// <summary>
        /// Returns an array of all byte indexes for all strings in the string blob.
        /// </summary>
        /// <returns></returns>
        public IntPtr[] ByteIndices
        {
            get
            {
                IntPtr[] ByteIndicesRet = default;
                if (_dynamic)
                    ByteIndicesRet = Refresh();
                else
                    ByteIndicesRet = _index;
                return ByteIndicesRet;
            }
        }

        /// <summary>
        /// Replace the string at the specified index with the specified string.
        /// </summary>
        /// <param name="index">Index of string to replace.</param>
        /// <param name="s">The replacement string.</param>
        public void ChangeString(int index, string s)
        {
            int la = get_LengthAt(index) * 2;
            int newlength = s.Length * 2;
            if (newlength == la)
                return;
            long b = _index[index].ToInt64() - SafePtr.handle.ToInt64();
            long dif = newlength - la;
            if (!_lpwstr)
            {
                Native.byteArrAtset(IntPtr.Add(SafePtr.handle, (int)b), BitConverter.GetBytes((long)s.Length), (uint)SizeDescriptorLength);
                b += SizeDescriptorLength;
            }

            if (dif > 0L)
            {
                _mem.PushOut(b, dif);
            }
            else
            {
                _mem.PullIn(b, -dif);
            }

            QuickCopyObject<string>(IntPtr.Add(SafePtr.handle, (int)b), s, (uint)newlength);
            Refresh();
        }

        /// <summary>
        /// Returns the length of the string at the specified index.
        /// </summary>
        /// <param name="index">The length of the specified string.</param>
        /// <returns></returns>
        public int get_LengthAt(int index)
        {
            int LengthAtRet = default;
            if (index >= Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (_dynamic)
                Refresh();
            if (_lpwstr)
            {
                var lpsz = new LPWSTR();
                lpsz._ptr = _index[index];
                LengthAtRet = lpsz.Length;
                GC.SuppressFinalize(lpsz);
            }
            else
            {
                var lpsz = new BSTR();
                lpsz._ptr = _index[index];
                LengthAtRet = lpsz.Length;
                GC.SuppressFinalize(lpsz);
            }

            return LengthAtRet;
        }

        /// <summary>
        /// Removes the string at the specified index.
        /// </summary>
        /// <param name="index">Index of string to remove.</param>
        public void RemoveAt(int index)
        {
            if (index < 0 || index > Count - 1)
                return;
            IntPtr b = (IntPtr)(_index[index].ToInt64() - SafePtr.handle.ToInt64());
            if (_lpwstr)
            {
                int c = get_LengthAt(index) * 2 + 2;
                _mem.PullIn((long)b, c);
            }
            else
            {
                int c = get_LengthAt(index) * 2 + SizeDescriptorLength;
                _mem.PullIn((long)b, c);
                switch (SizeDescriptorLength)
                {
                    case 2:
                        {
                            _mem.get_UShortAt(0L);
                            break;
                        }

                    case 4:
                        {
                            _mem.get_UIntegerAt(0L);
                            break;
                        }

                    case 8:
                        {
                            _mem.get_ULongAt(0L);
                            break;
                        }
                }
            }

            Refresh();
        }

        /// <summary>
        /// Insert a string at the specified index.
        /// </summary>
        /// <param name="s">String to insert.</param>
        /// <param name="index">Index at which to insert the string.</param>
        public void InsertAt(string s, int index)
        {
            if (index < 0)
                index = 0;
            if (index > Count - 1)
            {
                AddString(s);
                return;
            }

            var b = _index[index] - (int)SafePtr.handle;
            if (_lpwstr)
            {
                int c = s.Length * 2 + 2;
                _mem.PushOut((long)b, c);
                QuickCopyObject<string>(b + (int)SafePtr.handle, s, (uint)(s.Length * 2));
            }
            else
            {
                int c = s.Length * 2 + SizeDescriptorLength;
                _mem.PushOut((long)b, c);
                Native.byteArrAtset(b + (int)SafePtr.handle, BitConverter.GetBytes((long)s.Length), (uint)SizeDescriptorLength);
                b = b + SizeDescriptorLength;
                QuickCopyObject<string>(b + (int)SafePtr.handle, s, (uint)(s.Length * 2));
                switch (SizeDescriptorLength)
                {
                    case 2:
                        {
                            _mem.get_UShortAt(0L);
                            break;
                        }

                    case 4:
                        {
                            _mem.get_UIntegerAt(0L);
                            break;
                        }

                    case 8:
                        {
                            _mem.get_ULongAt(0L);
                            break;
                        }
                }
            }

            Refresh();
        }

        /// <summary>
        /// Gets or sets the Unicode Byte Order Mark preamble for the string collection.
        /// </summary>
        /// <returns></returns>
        public BOMTYPE BOM
        {
            get
            {
                return _BOM.Type;
            }

            set
            {
                _BOM.SetBOM(value);
            }
        }

        /// <summary>
        /// Gets or sets the backing SafePtr object for this StringBlob.
        /// </summary>
        /// <returns></returns>
        public SafePtr SafePtr
        {
            get
            {
                if (!_dynamic)
                    throw new FieldAccessException("Field can only be accessed in dynamic mode.");
                return _mem;
            }

            set
            {
                if (!_dynamic)
                    throw new FieldAccessException("Field can only be accessed in dynamic mode.");
                _mem = value;
            }
        }

        /// <summary>
        /// Add a string to the end of the collection.
        /// </summary>
        /// <param name="value">The string to add.</param>
        public void AddString(string value)
        {
            int ol;
            if (_lpwstr)
            {
                ol = (int)(_mem.Length - 2L);
                if (ol <= 0)
                {
                    _mem.Length = 2L;
                    ol = 0;
                }

                _mem.Length += value.Length + 2;
                QuickCopyObject<string>(IntPtr.Add(_mem.handle, ol), value, (uint)(value.Length * 2));
            }
            else
            {
                int l = SizeDescriptorLength + value.Length * 2;
                if (_mem.Length == 0L)
                    _mem.Length = SizeDescriptorLength;
                ol = (int)_mem.Length;
                switch (SizeDescriptorLength)
                {
                    case 1:
                        {
                            _mem[0L] = (byte)(_mem[0L] + 1);
                            break;
                        }

                    case 2:
                        {
                            _mem.get_ShortAt(0L);
                            break;
                        }

                    case 4:
                        {
                            _mem.get_UIntegerAt(0L);
                            break;
                        }
                }

                _mem.Length += l;
                IntPtr ap;
                IntPtr at;
                ap = IntPtr.Add(_mem.handle, ol);
                at = IntPtr.Add(ap, SizeDescriptorLength);
                MemPtr mm = ap;
                switch (SizeDescriptorLength)
                {
                    case 2:
                        {
                            mm.set_UShortAt(0L, (ushort)value.Length);
                            break;
                        }

                    case 4:
                        {
                            mm.set_UIntegerAt(0L, (uint)value.Length);
                            break;
                        }

                    case 8:
                        {
                            mm.set_LongAt(0L, value.Length);
                            break;
                        }
                }

                QuickCopyObject<string>(at, value, (uint)(value.Length * 2));
            }

            if (!_dynamic)
            {
                _index = new IntPtr[_count + 1];
                _index[_count] = IntPtr.Add(_mem.handle, ol);
                _count += 1;
            }
        }

        /// <summary>
        /// Add an array of strings to the end of the collection.
        /// </summary>
        /// <param name="values">Array of strings.</param>
        public void AddStrings(string[] values)
        {
            if (values is null)
                return;
            int sdl = SizeDescriptorLength;
            int dl = sdl * values.Length;
            foreach (var s in values)
                dl += s.Length * 2;
            if (_lpwstr)
            {
                dl += 2 * values.Length;
            }

            long sl;
            IntPtr sp;
            long oldLen;
            if (SafePtr.Length <= 0L)
            {
                SafePtr.Length = sdl;
            }

            oldLen = SafePtr.Length;
            switch (sdl)
            {
                case 2:
                    {
                        SafePtr.get_UShortAt(0L);
                        break;
                    }

                case 4:
                    {
                        SafePtr.get_UIntegerAt(0L);
                        break;
                    }

                case 8:
                    {
                        SafePtr.get_ULongAt(0L);
                        break;
                    }
            }

            SafePtr.Length += dl;
            sp = SafePtr.handle + (int)oldLen;
            int oi;
            if (_index is null)
            {
                oi = 0;
                _index = new IntPtr[values.Length];
            }
            else
            {
                oi = _index.Length;
                Array.Resize(ref _index, _index.Length + (values.Length - 1) + 1);
            }

            if (_lpwstr)
            {
                foreach (var s in values)
                {
                    sl = s.Length;
                    _index[oi] = sp;
                    oi += 1;
                    QuickCopyObject<string>(sp, s, (uint)(sl + sl));
                    sp = sp + (int)(sl + sl);
                    MemPtr mm = sp;
                    mm.set_ShortAt(0L, 0);
                    sp = sp + 2;
                }
            }
            else
            {
                foreach (var s in values)
                {
                    sl = s.Length;
                    _index[oi] = sp;
                    oi += 1;
                    MemPtr mm = sp;
                    switch (sdl)
                    {
                        case 2:
                            {
                                mm.set_UShortAt(0L, (ushort)sl);
                                break;
                            }

                        case 4:
                            {
                                mm.set_UIntegerAt(0L, (uint)sl);
                                break;
                            }

                        case 8:
                            {
                                mm.set_LongAt(0L, sl);
                                break;
                            }
                    }

                    sp = sp + sdl;
                    QuickCopyObject<string>(sp, s, (uint)(sl + sl));
                    sp = sp + (int)(sl + sl);
                }
            }

            _count = _index.Length;
        }

        /// <summary>
        /// Formats the StringBlob into a single string using the specified criteria.
        /// </summary>
        /// <param name="format">A combination of <see cref="StringBlobFormats"/> values that indicate how the string will be rendered.</param>
        /// <param name="customFormat">(NOT IMPLEMENTED)</param>
        /// <returns></returns>
        public string ToFormattedString(StringBlobFormats format, string customFormat = "")
        {
            var c = default(long);
            long d;
            long x = 0L;
            var sb = new StringBuilder();
            if (_dynamic)
                Refresh();
            if (Conversions.ToBoolean(format & StringBlobFormats.Commas))
            {
                c += (_count - 1) * 2;
            }

            if (Conversions.ToBoolean(format & StringBlobFormats.Quoted))
            {
                c += 4 * _count;
            }

            if (Conversions.ToBoolean(format & StringBlobFormats.CrLf))
            {
                c += 4 * _count;
            }

            if (Conversions.ToBoolean(format & StringBlobFormats.Spaced))
            {
                c += (_count - 1) * 2;
            }

            d = _mem.Length;
            if (_lpwstr)
            {
                d -= 2L;
                d -= 2 * _count;
            }
            else
            {
                d -= SizeDescriptorLength;
                d -= SizeDescriptorLength * _count;
            }

            c += d;
            sb.Capacity = (int)c;
            if (_lpwstr)
            {
                var lpstr = new LPWSTR();
                for (int i = 0, loopTo = _count - 1; i <= loopTo; i++)
                {
                    if (i > 0)
                    {
                        if (Conversions.ToBoolean(format & StringBlobFormats.Commas))
                        {
                            sb.Append(",");
                            x += 1L;
                        }

                        if (Conversions.ToBoolean(format & StringBlobFormats.Spaced))
                        {
                            sb.Append(" ");
                            x += 1L;
                        }
                    }

                    if (Conversions.ToBoolean(format & StringBlobFormats.Quoted))
                    {
                        sb.Append("\"");
                        x += 1L;
                    }

                    lpstr._ptr = (IntPtr)_index[i].ToInt64();
                    sb.Append(lpstr.Text);
                    x += lpstr.Length;
                    if (Conversions.ToBoolean(format & StringBlobFormats.Quoted))
                    {
                        sb.Append("\"");
                        x += 1L;
                    }

                    if (Conversions.ToBoolean(format & StringBlobFormats.CrLf))
                    {
                        sb.Append(Constants.vbCr);
                        x += 1L;
                        sb.Append(Constants.vbLf);
                        x += 1L;
                    }
                }

                GC.SuppressFinalize(lpstr);
            }
            else
            {
                var lpstr = new BSTR();
                lpstr._preamble = SizeDescriptorLength;
                for (int i = 0, loopTo1 = _count - 1; i <= loopTo1; i++)
                {
                    if (i > 0)
                    {
                        if (Conversions.ToBoolean(format & StringBlobFormats.Commas))
                        {
                            sb.Append(",");
                            x += 1L;
                        }

                        if (Conversions.ToBoolean(format & StringBlobFormats.Spaced))
                        {
                            sb.Append(" ");
                            x += 1L;
                        }
                    }

                    if (Conversions.ToBoolean(format & StringBlobFormats.Quoted))
                    {
                        sb.Append("\"");
                        x += 1L;
                    }

                    lpstr._ptr = (IntPtr)_index[i].ToInt64();
                    sb.Append(lpstr.Text);
                    x += lpstr.Length;
                    if (Conversions.ToBoolean(format & StringBlobFormats.Quoted))
                    {
                        sb.Append("\"");
                        x += 1L;
                    }

                    if (Conversions.ToBoolean(format & StringBlobFormats.CrLf))
                    {
                        sb.Append(Constants.vbCr);
                        x += 1L;
                        sb.Append(Constants.vbLf);
                        x += 1L;
                    }
                }

                GC.SuppressFinalize(lpstr);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Returns a plain-text reprentation of the string blob, with no formatting.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToFormattedString(StringBlobFormats.None);
        }

        /// <summary>
        /// Returns the specified StringBlob as a byte array.
        /// </summary>
        /// <param name="operand">Object whose bytes to retrieve.</param>
        /// <returns></returns>
        public static byte[] GetBytes(StringBlob operand)
        {
            return operand._mem.GrabBytes((IntPtr)0, (int)operand._mem.Length);
        }


        /// <summary>
        /// Sets the specified byte array into the specified StringBlob.
        /// This will overwrite the contents in the StringBlob.
        /// </summary>
        /// <param name="operand">Target object.</param>
        /// <param name="value">Byte array.</param>
        public static void SetBytes(ref StringBlob operand, byte[] value)
        {
            if (operand is null)
            {
                operand = new StringBlob();
            }

            operand._mem.Alloc(value.Length);
            operand._mem.SetBytes(Native.CIntPtr(0), value);
        }

        /// <summary>
        /// Converts the StringBlob into an array of characters.
        /// </summary>
        /// <param name="operand"></param>
        /// <returns></returns>
        public static char[] ToCharArray(StringBlob operand)
        {
            return Conversions.ToCharArrayRankOne(operand.ToString());
        }

        /// <summary>
        /// Converts an array of characters into a StringBlob.
        /// </summary>
        /// <param name="operand"></param>
        /// <returns></returns>
        public static StringBlob FromCharArray(char[] operand)
        {
            var sb = new StringBlob();
            sb._mem = operand;
            return sb;
        }

        /// <summary>
        /// Converts the StringBlob into a byte array.
        /// </summary>
        /// <param name="operand"></param>
        /// <returns></returns>
        public static byte[] ToByteArray(StringBlob operand)
        {
            return operand._mem.GrabBytes();
        }

        /// <summary>
        /// Converts a byte array into a string blob, with the optional size-descriptor size.
        /// </summary>
        /// <param name="operand">Byte array to copy.</param>
        /// <param name="sizeDesc">Size descriptor kind.</param>
        /// <returns></returns>
        public static StringBlob FromByteArray(byte[] operand, BlobOrdinalTypes sizeDesc = BlobOrdinalTypes.Integer)
        {
            var sb = new StringBlob();
            sb.SizeDescriptorType = sizeDesc;
            sb._mem.SetBytes(Native.CIntPtr(0), operand);
            sb.GetCount();
            return sb;
        }

        /// <summary>
        /// Gets a string array for the specified StringBlob
        /// </summary>
        /// <param name="operand">StringBlob whose strings to return.</param>
        /// <returns></returns>
        public static string[] ToStringArray(StringBlob operand)
        {
            string[] ToStringArrayRet = default;
            ToStringArrayRet = new StringBlobEnumerator(operand).AllStrings();
            return ToStringArrayRet;
        }

        /// <summary>
        /// Copies the strings from the StringBlob into the specified array starting at the specified index.
        /// </summary>
        /// <param name="sb">Source object.</param>
        /// <param name="array">Destination array.</param>
        /// <param name="startIndex">Index within array to begin copying.</param>
        public static void ToStringArray(StringBlob sb, string[] array, int startIndex)
        {
            int c = startIndex;
            foreach (string n in sb)
            {
                array[c] = n;
                c += 1;
            }
        }

        /// <summary>
        /// Creates a StringBlob from a string array.
        /// </summary>
        /// <param name="operand">Source array.</param>
        /// <returns></returns>
        public static StringBlob FromStringArray(string[] operand)
        {
            var sb = new StringBlob();
            sb.AddStrings(operand);
            return sb;
        }

        /// <summary>
        /// Completely clear the object and free its resources.
        /// </summary>
        public void Clear()
        {
            _mem.Length = 0L;
        }

        public static StringBlob operator +(StringBlob operand1, string[] operand2)
        {
            if (operand2 is null || operand2.Length == 0)
                return operand1;
            int i;
            int c = operand2.Length - 1;
            var loopTo = c;
            for (i = 0; i <= loopTo; i++)
                operand1.AddString(operand2[i]);
            return operand1;
        }

        public static StringBlob operator +(StringBlob operand1, string operand2)
        {
            if (operand2 is null || operand2.Length == 0)
                return operand1;
            operand1.AddString(operand2);
            return operand1;
        }

        public static Blob operator +(Blob operand1, StringBlob operand2)
        {
            long d = operand1.Length;
            long c = operand1.Length + operand2.ByteLength;
            operand1.Length += c;
            if (c - d < uint.MaxValue)
            {
                Native.MemCpy(operand1.DangerousGetHandle() + (int)d, operand2.Handle, (uint)(c - d));
            }
            else
            {
                Native.CopyMemory(operand1.DangerousGetHandle() + (int)d, operand2.Handle, Native.CIntPtr(c - d));
            }

            operand1.BlobType = BlobTypes.Char;
            return operand1;
        }

        public static explicit operator StringBlob(string[] operand)
        {
            return FromStringArray(operand);
        }

        public static implicit operator string[](StringBlob operand)
        {
            return ToStringArray(operand);
        }

        public static explicit operator StringBlob(string operand)
        {
            return FromCharArray(operand.ToCharArray());
        }

        public static implicit operator string(StringBlob operand)
        {
            return Conversions.ToString(ToCharArray(operand));
        }

        public static explicit operator StringBlob(char[] operand)
        {
            return FromCharArray(operand);
        }

        public static implicit operator char[](StringBlob operand)
        {
            return ToCharArray(operand);
        }

        public static explicit operator StringBlob(byte[] operand)
        {
            return FromByteArray(operand);
        }

        public static implicit operator byte[](StringBlob operand)
        {
            return ToByteArray(operand);
        }

        public IEnumerator GetEnumerator()
        {
            return new StringBlobEnumerator(this);
        }

        public object Clone()
        {
            var sb = new StringBlob();
            sb.LpWStr = LpWStr;
            sb.Dynamic = Dynamic;
            sb._mem.Length = _mem.Length;
            Native.CopyMemory(sb._mem.handle, _mem.handle, Native.CIntPtr(_mem.Length));
            sb.Refresh();
            return sb;
        }

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        private bool disposedValue; // To detect redundant calls

        // IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_mem is object)
                        _mem.Dispose();
                }
            }

            disposedValue = true;
        }

        ~StringBlob()
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