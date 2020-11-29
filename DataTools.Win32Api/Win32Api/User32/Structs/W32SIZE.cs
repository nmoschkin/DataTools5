using System;
using System.Runtime.InteropServices;

namespace DataTools.Win32Api
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct W32SIZE
    {
        public int cx;
        public int cy;

        public W32SIZE(int cx, int cy)
        {
            this.cx = cx;
            this.cy = cy;
        }
    }
}
