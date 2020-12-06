using System;
using System.Runtime.InteropServices;

namespace DataTools.Win32Api
{
    public struct HSVDATA
    {
        public double Hue;
        public double Saturation;
        public double Value;

        public HSVDATA(double h, double s, double v)
        {
            Hue = h;
            Saturation = s;
            Value = v;
        }
    }
}
