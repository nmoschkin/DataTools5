// ' ************************************************* ''
// ' DataTools Visual Basic Utility Library 
// '
// ' Module: LPWSTR
// '         Null-terminated text string.
// ' 
// ' Copyright (C) 2011-2020 Nathan Moschkin
// ' All Rights Reserved
// '
// ' Licensed Under the Microsoft Public License   
// ' ************************************************* ''

using System;
using DataTools.Memory.Internal;
using Microsoft.VisualBasic.CompilerServices;

namespace DataTools.Memory
{


    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    /// <summary>
    /// LPWSTR implementation
    /// </summary>
    /// <remarks></remarks>
    public class LPWSTR : IDisposable
    {
        internal IntPtr _ptr;

        public override string ToString()
        {
            return Conversions.ToString(this);
        }

        public LPWSTR(IntPtr ptr)
        {
            _ptr = ptr;
        }

        public LPWSTR()
        {
        }

        public string Text
        {
            get
            {
                return Conversions.ToString(this);
            }

            set
            {
                int l = value.Length + 2;

                // ' get the process heap.
                var hHeap = Native.GetProcessHeap();
                if (_ptr != IntPtr.Zero)
                {
                    // ' get the current heap size.
                    long bl = (long)Native.HeapSize(hHeap, 0U, _ptr);
                    _ptr = Native.HeapReAlloc(hHeap, 0, _ptr, (IntPtr)l);

                    // ' get the real new heap size
                    long bl2 = (long)Native.HeapSize(hHeap, 0U, _ptr);

                    // ' tell the garbage collector
                    if (bl2 > bl2)
                    {
                        GC.AddMemoryPressure(bl - bl2);
                    }
                    else
                    {
                        GC.RemoveMemoryPressure(bl2 - bl);
                    }
                }
                else
                {
                    // ' allocate a new memory block
                    _ptr = Native.HeapAlloc(hHeap, 0U, (IntPtr)l);

                    // ' tell the garbage collector
                    long bl = (long)Native.HeapSize(hHeap, 0U, _ptr);
                    GC.AddMemoryPressure(bl);
                }

                Native.CopyMemory(_ptr, value, Native.CIntPtr(l - 2));
            }
        }

        public IntPtr Handle
        {
            get
            {
                return _ptr;
            }
        }

        public int Length
        {
            get
            {
                int LengthRet = default;
                if (_ptr == IntPtr.Zero)
                    return -1;
                LengthRet = -1;
                // Dim hlen As Long = HeapSize(GetProcessHeap, 0, _ptr)
                // If hlen < 0 Then Exit Property

                char i = '\0';
                var pc = _ptr;
                do
                {
                    Native.CopyMemory(ref i, pc, Native.CIntPtr(2));
                    pc = IntPtr.Add(pc, 2);
                    LengthRet += 1;
                }
                while (i != '\0'); // OrElse Length > hlen
                return LengthRet;
            }
        }

        public static explicit operator IntPtr(LPWSTR operand)
        {
            return operand._ptr;
        }

        public static explicit operator LPWSTR(IntPtr operand)
        {
            return new LPWSTR(operand);
        }

        public static explicit operator UIntPtr(LPWSTR operand)
        {
            return new UIntPtr((ulong)operand._ptr);
        }

        public static explicit operator LPWSTR(UIntPtr operand)
        {
            return new LPWSTR(new IntPtr(Conversions.ToLong(operand.ToUInt64())));
        }

        public static explicit operator LPWSTR(string operand)
        {
            MemPtr mm = operand;
            var lpw = new LPWSTR();
            lpw._ptr = mm;
            return lpw;
        }

        public static explicit operator LPWSTR(char[] operand)
        {
            MemPtr mm = operand;
            var lpw = new LPWSTR();
            lpw._ptr = mm;
            return lpw;
        }

        public static explicit operator string(LPWSTR operand)
        {
            var mm = new MemPtr(operand._ptr);
            return Conversions.ToString(mm);
        }

        public static explicit operator char[](LPWSTR operand)
        {
            var mm = new MemPtr(operand._ptr);
            return Conversions.ToCharArrayRankOne(mm);
        }

        public static explicit operator byte[](LPWSTR operand)
        {
            var mm = new MemPtr(operand._ptr);
            return mm;
        }

        public static explicit operator LPWSTR(byte[] operand)
        {
            var lpw = new LPWSTR();
            MemPtr mm = operand;
            lpw._ptr = mm.Handle;
            return lpw;
        }

        public static implicit operator BSTR(LPWSTR operand)
        {
            return (BSTR)Conversions.ToString(operand);
        }

        public static implicit operator LPWSTR(BSTR operand)
        {
            return (LPWSTR)Conversions.ToString(operand);
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
                    var hHeap = Native.GetProcessHeap();
                    // ' free up the unmanaged buffer 
                    long bl = (long)Native.HeapSize(hHeap, 0U, _ptr);
                    if ((IntPtr)Native.HeapFree(hHeap, 0U, _ptr) == IntPtr.Zero)
                    {
                        GC.RemoveMemoryPressure(bl);
                    }
                }
            }

            disposedValue = true;
        }

        ~LPWSTR()
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