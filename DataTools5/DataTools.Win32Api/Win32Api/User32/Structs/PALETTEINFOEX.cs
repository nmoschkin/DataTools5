using System;
using System.Runtime.InteropServices;

namespace DataTools.Win32Api
{
    public struct PALETTEINFOEX
    {
        public bool FlipHorizontal;
        public bool FlipVertical;
        public bool GrayTones;
        public int StaticField;
        public int Value;
        public int BlockSize;
    }
}
