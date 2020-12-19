using System;
using System.Runtime.InteropServices;

using DataTools.Desktop.Unified;
using DataTools.Desktop;

namespace DataTools.Graphics
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

        public static explicit operator UniColor(HSVDATA h)
        {
            return ColorMath.HSVToColor(h);
        }

        public static explicit operator HSVDATA(UniColor c)
        {
            return ColorMath.ColorToHSV(c);
        }
    }
}
