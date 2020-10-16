using System;
using System.Runtime.InteropServices;

namespace DataTools.Win32Api
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct W32POINT
    {
        public int x;
        public int y;

        public W32POINT(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
