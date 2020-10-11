using System.IO;
using System.Runtime.InteropServices;

namespace DataTools.Memory
{
    public sealed class VirtualMemoryStream : Stream
    {
        public VirtualMemoryStream()
        {
            _Blob = new Blob() { InBufferMode = true, BufferExtend = 65536L, MemoryType = MemAllocType.Virtual };
        }

        private Blob _Blob;

        public override bool CanRead
        {
            get
            {
                return true;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return true;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return true;
            }
        }

        public override long Length
        {
            get
            {
                return _Blob.Length;
            }
        }

        public override long Position
        {
            get
            {
                return _Blob.ClipNext;
            }

            set
            {
                _Blob.ClipSeek(value);
            }
        }

        public override void Flush()
        {
            return;
        }

        public override void SetLength(long value)
        {
            _Blob.Length = value;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            var gch = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            var cptr = gch.AddrOfPinnedObject() + offset;
            if (_Blob.Length - _Blob.ClipNext < count)
            {
                _Blob.Length += count - _Blob.ClipNext;
            }

            Internal.Native.MemCpy(_Blob.DangerousGetHandle() + _Blob.ClipNext, cptr, (uint)count);
            gch.Free();
            _Blob.ClipSeek(_Blob.ClipNext + count);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            var gch = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            var cptr = gch.AddrOfPinnedObject() + offset;
            if (_Blob.Length - _Blob.ClipNext < count)
            {
                count = (int)(_Blob.Length - _Blob.ClipNext);
            }

            if (count <= 0)
                return 0;
            Internal.Native.MemCpy(_Blob.DangerousGetHandle() + _Blob.ClipNext, cptr, (uint)count);
            _Blob.ClipSeek(_Blob.ClipNext + count);
            return count;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    {
                        _Blob.ClipSeek(offset);
                        break;
                    }

                case SeekOrigin.Current:
                    {
                        _Blob.ClipSeek(_Blob.ClipNext + offset);
                        break;
                    }

                case SeekOrigin.End:
                    {
                        _Blob.ClipSeek(_Blob.Length + offset);
                        break;
                    }
            }

            return _Blob.ClipNext;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                _Blob.Free();
            }
        }
    }
}