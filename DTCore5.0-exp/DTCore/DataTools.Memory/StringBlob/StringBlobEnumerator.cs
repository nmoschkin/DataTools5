// ' ************************************************* ''
// ' DataTools Visual Basic Utility Library 
// '
// ' Module: Enumerators for StringBlob
// ' 
// ' Copyright (C) 2011-2020 Nathan Moschkin
// ' All Rights Reserved
// '
// ' Licensed Under the Microsoft Public License   
// ' ************************************************* ''

using System;
using System.Collections.Generic;

namespace DataTools.Memory
{

    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    public class StringBlobEnumerator : IEnumerator<string>
    {
        private StringBlob _Sb;
        private int pos = -1;
        private int c = 0;
        private IntPtr ptr;
        private IntPtr ep;
        private int sd;

        public string[] AllStrings()
        {
            string[] AllStringsRet = default;
            char ch = '\0';
            MemPtr p2 = ptr;
            string[] s;
            int c = _Sb.Count - 1;
            s = new string[c + 1];
            var a = default(ulong);
            if (_Sb.LpWStr)
            {
                for (int j = 0, loopTo = c; j <= loopTo; j++)
                {
                    s[j] = p2.GrabString((IntPtr)0);
                    p2 += s[j].Length * 2 + 2;
                }
            }
            else
            {
                for (int j = 0, loopTo1 = c; j <= loopTo1; j++)
                {
                    switch (sd)
                    {
                        case 2:
                            {
                                a = p2.get_UShortAt(0L);
                                break;
                            }

                        case 4:
                            {
                                a = p2.get_UIntegerAt(0L);
                                break;
                            }

                        case 8:
                            {
                                a = p2.get_ULongAt(0L);
                                break;
                            }
                    }

                    s[j] = p2.GrabString((IntPtr)sd, (int)a);
                    p2 += (long)(a * 2m + sd);
                }
            }

            AllStringsRet = s;
            return AllStringsRet;
        }

        public StringBlobEnumerator(StringBlob subj)
        {
            _Sb = subj;
            ptr = _Sb.SafePtr.handle;
            sd = _Sb.SizeDescriptorLength;
            if (_Sb.LpWStr == false)
            {
                ptr = ptr + _Sb.SizeDescriptorLength;
            }

            ep = ptr;
            c = _Sb.Count;
        }

        public string Current
        {
            get
            {
                string CurrentRet = default;
                char ch = '\0';
                MemPtr p2 = ep;
                var a = default(ulong);
                if (_Sb.LpWStr)
                {
                    CurrentRet = p2.GrabString((IntPtr)0);
                    ep = p2 + CurrentRet.Length * 2 + 2;
                }
                else
                {
                    switch (sd)
                    {
                        case 2:
                            {
                                a = p2.get_UShortAt(0L);
                                break;
                            }

                        case 4:
                            {
                                a = p2.get_UIntegerAt(0L);
                                break;
                            }

                        case 8:
                            {
                                a = p2.get_ULongAt(0L);
                                break;
                            }
                    }

                    CurrentRet = p2.GrabString((IntPtr)sd, (int)a);
                    ep = p2 + (long)(a * 2m + sd);
                }

                return CurrentRet;
            }
        }

        public object Current
        {
            get
            {
                return this.Current;
            }
        }

        public bool MoveNext()
        {
            ptr = ep;
            if ((long)(ptr - (int)_Sb.Handle) >= _Sb.SafePtr.Length)
                return false;
            return true;
        }

        public void Reset()
        {
            pos = -1;
            ptr = _Sb.Handle;
            sd = _Sb.SizeDescriptorLength;
            ep = ptr;
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
                }
            }

            disposedValue = true;
        }

        // This code added by Visual Basic to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    }

    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */


}