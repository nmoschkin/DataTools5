using DataTools.Memory;
using DataTools.Win32Api.Network;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Navigation;

namespace DataTools.Win32Api.Network
{

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct LPMIB_IFTABLE : IEnumerable<MIB_IFROW>, IReadOnlyCollection<MIB_IFROW>, IDisposable
    {
        private SafePtr ptr;
        private static readonly int RowSize = Marshal.SizeOf<MIB_IFROW>();

        public int Count
        {
            get => ptr.IntAt(0);
        }

        public MIB_IFROW this[int index]
        {
            get
            {
                if (index >= Count) throw new IndexOutOfRangeException();
                int idx = sizeof(int) + (index * RowSize);
                return ptr.ToStructAt<MIB_IFROW>(idx);
            }
        }

        public bool Alloc(int length)
        {
            if (ptr != null) return false;

            ptr = new SafePtr();

            return ptr.Alloc(length);
        }

        public void Dispose()
        {
            ptr?.Free();
            ptr = null;
        }

        public IEnumerator<MIB_IFROW> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        private class Enumerator : IEnumerator<MIB_IFROW>
        {
            LPMIB_IFTABLE src;

            int count = 0;
            int cIdx = -1;

            public MIB_IFROW Current => src[cIdx];

            object IEnumerator.Current => src[cIdx];

            public void Dispose()
            {
                src = default;
                count = cIdx = 0;
            }

            public bool MoveNext() => (++cIdx < count);

            public void Reset()
            {
                cIdx = -1;
            }

            public Enumerator(LPMIB_IFTABLE source)
            {
                src = source;
                count = src.Count;
            }
        }


    }




}
