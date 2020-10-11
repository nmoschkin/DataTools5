// ' ************************************************* ''
// ' DataTools Visual Basic Utility Library 
// '
// ' Module: Enumerators for Blob
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
    public class BlobEnumeratorByte : IEnumerator<byte>
    {
        private Blob mm;
        private int pos = -1;

        internal BlobEnumeratorByte(Blob subj)
        {
            mm = subj;
        }

        public byte Current
        {
            get
            {
                byte CurrentRet = default;
                CurrentRet = mm.get_ByteAt(pos);
                return CurrentRet;
            }
        }

        public object Current
        {
            get
            {
                object CurrentRet = default;
                CurrentRet = mm.get_ByteAt(pos);
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

        ~BlobEnumeratorByte()
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

    public class BlobEnumeratorChar : IEnumerator<char>
    {
        private Blob mm;
        private int pos = -1;

        internal BlobEnumeratorChar(Blob subj)
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

        ~BlobEnumeratorChar()
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