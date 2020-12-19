using System;
using System.Runtime.InteropServices;

namespace DataTools.Win32
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct W32RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;

        public W32RECT(int l, int t, int r, int b)
        {
            left = l;
            top = t;
            right = r;
            bottom = b;
        }

        public override string ToString()
        {
            return $"{left}, {top}, {right}, {bottom}";

        }
    }
}
