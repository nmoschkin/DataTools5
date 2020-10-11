using System.Runtime.InteropServices;
using Microsoft.VisualBasic;

namespace DataTools.ExtendedMath.ColorMath
{
    [HideModuleName]
    public static class ColorStructs
    {

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
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
        }

        public struct PALETTEINFOEX
        {
            public bool FlipHorizontal;
            public bool FlipVertical;
            public bool GrayTones;
            public int StaticField;
            public int Value;
            public int BlockSize;
        }

        public struct RGBDATA
        {
            public byte Blue;
            public byte Green;
            public byte Red;
        }

        public struct ARGBDATA
        {
            public byte Blue;
            public byte Green;
            public byte Red;
            public byte Alpha;
        }

        public struct BGRADATA
        {
            public byte Alpha;
            public byte Red;
            public byte Green;
            public byte Blue;
        }

        public struct BGRDATA
        {
            public byte Red;
            public byte Green;
            public byte Blue;
        }

        public struct CMYDATA
        {
            public byte Cyan;
            public byte Magenta;
            public byte Yellow;
        }

        public struct HSVDATA
        {
            public double Hue;
            public double Saturation;
            public double Value;
        }

        public struct HSLDATA
        {
            public double Hue;
            public double Saturation;
            public double Lightness;
        }

        public struct CARTESEAN2D
        {
            public System.Drawing.Point Origin;
            public System.Drawing.Point Points;
            public double Arc;
            public double Distance;
        }


        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    }
}