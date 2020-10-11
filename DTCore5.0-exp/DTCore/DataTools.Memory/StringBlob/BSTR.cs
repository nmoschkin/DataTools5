// ' ************************************************* ''
// ' DataTools Visual Basic Utility Library 
// '
// ' Module: BSTR
// '         Length-preambled text string.
// ' 
// ' Copyright (C) 2011-2020 Nathan Moschkin
// ' All Rights Reserved
// '
// ' Licensed Under the Microsoft Public License   
// ' ************************************************* ''

using System;
using System.Runtime.InteropServices;
using DataTools.Memory.Internal;
using Microsoft.VisualBasic.CompilerServices;

namespace DataTools.Memory
{


    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    /// <summary>
    /// BSTR implementation
    /// </summary>
    /// <remarks></remarks>
    public class BSTR : IDisposable
    {
        internal IntPtr _ptr;
        internal int _preamble = 4;
        internal bool _Com = false;

        public override string ToString()
        {
            return _getString();
        }

        public BSTR(IntPtr ptr)
        {
            _ptr = ptr;
        }

        public BSTR(IntPtr ptr, bool fromCom)
        {
            _ptr = ptr;
            if (fromCom)
            {
                _Com = true;
                _ptr = _ptr - 4;
                _preamble = 4;
            }
        }

        public BSTR()
        {
        }

        public int PreambleLength
        {
            get
            {
                return _preamble;
            }

            set
            {
                if (_Com)
                {
                    _preamble = 4;
                    return;
                }

                if (Length != 0 && value < _preamble && Length > _preMax(value))
                {
                    throw new ArgumentException("Cannot downgrade preamble with long string present.");
                }

                if (_ptr == IntPtr.Zero)
                {
                    _preamble = value;
                    return;
                }

                int tl = Length;
                int l = tl + value;
                string s = (string)this;
                IntPtr pNew;
                pNew = Native.HeapAlloc(Native.GetProcessHeap(), 0U, (IntPtr)l);
                Native.CopyMemory(pNew, ref tl, Native.CIntPtr(value));
                Native.CopyMemory(IntPtr.Add(pNew, value), IntPtr.Add(_ptr, (int)Native.CIntPtr(_preamble)), Native.CIntPtr(tl * 2));
                Native.HeapFree(Native.GetProcessHeap(), 0U, _ptr);
                _ptr = pNew;
                _preamble = value;
            }
        }

        public bool Com
        {
            get
            {
                return _Com;
            }

            internal set
            {
                if (_Com == true)
                    return;
                _Com = value;
                if (value)
                {
                    _preamble = 4;
                    if (_ptr != IntPtr.Zero)
                    {
                        _ptr = _ptr - 4;
                    }
                }
                else if (_ptr != IntPtr.Zero)
                {
                    _ptr = _ptr + 4;
                }
            }
        }

        private int _preMax(int val)
        {
            switch (val)
            {
                case 1:
                    {
                        return 255;
                    }

                case 2:
                    {
                        return 32767;
                    }

                case 4:
                    {
                        return 0x7FFFFFFF;
                    }

                default:
                    {
                        return (int)-1L;
                    }
            }
        }

        public IntPtr Handle
        {
            get
            {
                IntPtr HandleRet = default;
                if (_Com)
                    HandleRet = _ptr + 4;
                else
                    HandleRet = _ptr;
                return HandleRet;
            }
        }

        public int Length
        {
            get
            {
                int LengthRet = default;
                if (_ptr == IntPtr.Zero)
                    return -1;
                Native.CopyMemory(ref LengthRet, _ptr, Native.CIntPtr(_preamble));
                return LengthRet;
            }
        }

        public string Text
        {
            get
            {
                string TextRet = default;
                TextRet = _getString();
                return TextRet;
            }

            set
            {
                _setString(value);
            }
        }

        private void _setString(string value)
        {
            if (value.Length > _preMax(_preamble))
            {
                throw new ArgumentException("Long string won't fit current preamble.");
            }

            int nl = value.Length * 2 + _preamble;
            var hHeap = Native.GetProcessHeap();

            // ' already have a string, let's see what is going on...
            if (_ptr != IntPtr.Zero)
            {
                // ' get the heap buffer size.
                int lb = (int)Native.HeapSize(hHeap, 0U, _ptr);
                if (string.IsNullOrEmpty(value))
                {
                    // null string, release all memory and tell the garbage collector
                    Native.HeapFree(hHeap, 0U, _ptr);
                    GC.RemoveMemoryPressure(lb);
                    return;
                }

                // ' we're actually reallocating the buffer.
                if (lb != nl)
                {
                    _ptr = Native.HeapReAlloc(hHeap, 0, _ptr, (IntPtr)nl);


                    // ' have to check the actual allocated size because it may be different from the calculated length
                    nl = (int)Native.HeapSize(hHeap, 0U, _ptr);
                    if (nl > lb)
                    {
                        // ' tell the garbage collector about the change in size
                        GC.AddMemoryPressure(nl - lb);
                    }
                    else
                    {
                        // ' tell the garbage collector about the change in size
                        GC.RemoveMemoryPressure(lb - nl);
                    }
                }
            }
            else
            {
                // ' allocate a new buffer!
                _ptr = Native.HeapAlloc(hHeap, 0U, (IntPtr)nl);

                // ' have to check the actual allocated size because it may be different from the calculated length
                nl = (int)Native.HeapSize(hHeap, 0U, _ptr);

                // ' tell the garbage collector about the allocation
                GC.AddMemoryPressure(nl);
            }

            int l = value.Length;
            var p = IntPtr.Add(_ptr, (int)Native.CIntPtr(_preamble));

            // ' copy the length of the string (in characters) into the preamble
            Native.CopyMemory(_ptr, ref l, Native.CIntPtr(_preamble));

            // ' copy the string into the buffer after the preamble.
            Native.CopyMemory(p, value, Native.CIntPtr(value.Length * 2));
        }

        private string _getString()
        {
            var l = default(short);
            string s;

            // ' get the length of the string out of the preamble.
            Native.CopyMemory(ref l, _ptr, Native.CIntPtr(_preamble));
            s = new string('\0', l);
            var ptr = IntPtr.Add(_ptr, (int)Native.CIntPtr(_preamble));
            Native.CopyMemory(s, ptr, Native.CIntPtr(l * 2));
            return s;
        }

        public static explicit operator IntPtr(BSTR operand)
        {
            return operand.Handle;
        }

        public static explicit operator BSTR(IntPtr operand)
        {
            return new BSTR(operand, true);
        }

        public static explicit operator MemPtr(BSTR operand)
        {
            return operand.Handle;
        }

        public static explicit operator BSTR(MemPtr operand)
        {
            return new BSTR(operand, true);
        }

        public static explicit operator UIntPtr(BSTR operand)
        {
            return new UIntPtr((ulong)operand.Handle);
        }

        public static explicit operator BSTR(UIntPtr operand)
        {
            return new BSTR(new IntPtr(Conversions.ToLong(operand.ToUInt64())), true);
        }

        public static explicit operator BSTR(string operand)
        {
            var b = new BSTR();
            b._setString(operand);
            return b;
        }

        public static explicit operator BSTR(char[] operand)
        {
            var b = new BSTR();
            b._setString(Conversions.ToString(operand));
            return b;
        }

        public static explicit operator string(BSTR operand)
        {
            return operand._getString();
        }

        public static explicit operator char[](BSTR operand)
        {
            var l = default(short);
            char[] ch;

            // ' get the size from the preamble
            Native.CopyMemory(ref l, operand._ptr, Native.CIntPtr(2));
            ch = new char[l];

            // ' get the pointer to memory just past the preamble
            var ptr = IntPtr.Add(operand._ptr, (int)Native.CIntPtr(2));

            // ' copy the memory into the managed array.
            Native.CopyMemory(ch, ptr, Native.CIntPtr(l * 2));

            // ' return the new array
            return ch;
        }

        // ' same as above but with bytes.
        public static explicit operator byte[](BSTR operand)
        {
            int i = operand.Length;
            byte[] b;
            int l = i * 2 + operand._preamble;
            b = new byte[l];
            Native.CopyMemory(b, operand._ptr, Native.CIntPtr(l));
            return b;
        }

        public static explicit operator BSTR(byte[] operand)
        {
            var bs = new BSTR();
            var hHeap = Native.GetProcessHeap();
            bs._ptr = Native.HeapAlloc(hHeap, 0U, (IntPtr)operand.Length);
            int argpSrc = operand.Length;
            Native.CopyMemory(bs._ptr, ref argpSrc, Native.CIntPtr(bs._preamble));
            operand.Length = argpSrc;
            Native.CopyMemory(IntPtr.Add(bs._ptr, bs._preamble), operand, Native.CIntPtr(operand.Length));

            // ' get the size of the allocated buffer and tell the garbage collector
            long bl = (long)Native.HeapSize(hHeap, 0U, bs._ptr);
            GC.AddMemoryPressure(bl);
            return bs;
        }

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        private bool disposedValue; // To detect redundant calls

        // IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (_ptr != IntPtr.Zero)
                {
                    if (_Com)
                    {
                        Marshal.FreeCoTaskMem(_ptr + 4);
                    }
                    else
                    {
                        var hHeap = Native.GetProcessHeap();
                        // ' free up the unmanaged buffer 
                        long l = (long)Native.HeapSize(hHeap, 0U, _ptr);
                        if ((IntPtr)Native.HeapFree(hHeap, 0U, _ptr) == IntPtr.Zero)
                        {
                            GC.RemoveMemoryPressure(l);
                        }
                    }
                }
            }

            disposedValue = true;
        }

        ~BSTR()
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