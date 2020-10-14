using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace DataTools.Win32Api
{
    [StructLayout(LayoutKind.Explicit)]
    internal unsafe struct LARGE_INTEGER
    {
        [FieldOffset(0)]
        internal int LowPart;

        [FieldOffset(4)]
        internal int HighPart;

        [FieldOffset(0)]
        internal long QuadPart;
    }
}
