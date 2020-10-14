using System;
using System.Runtime.InteropServices;

namespace DataTools.Desktop.Structures
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



    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct RGBQUAD
    {
        public byte rgbBlue;
        public byte rgbGreen;
        public byte rgbRed;
        public byte rgbReserved;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct RGBTRIPLE
    {
        public byte rgbtBlue;
        public byte rgbtGreen;
        public byte rgbtRed;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BITMAPINFOHEADER
    {
        public int biSize;
        public int biWidth;
        public int biHeight;
        public short biPlanes;
        public short biBitCount;
        public int biCompression;
        public int biSizeImage;
        public int biXPelsPerMeter;
        public int biYPelsPerMeter;
        public int biClrUsed;
        public int biClrImportant;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BITMAPINFO
    {
        public BITMAPINFOHEADER bmiHeader;
        public RGBQUAD bmiColors;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BITMAPFILEHEADER
    {
        public short bfType;
        public int bfSize;
        public short bfReserved1;
        public short bfReserved2;
        public int bfOffBits;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BITMAPCOREHEADER
    {
        public int bcSize;
        public short bcWidth;
        public short bcHeight;
        public short bcPlanes;
        public short bcBitCount;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BITMAPCOREINFO
    {
        public BITMAPCOREHEADER bmciHeader;
        public RGBTRIPLE bmciColors;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BITMAPSTRUCT
    {
        public int bmStructure;
        public int bmWidth;
        public int bmHeight;
        public int bmWidthBytes;
        public short bmPlanes;
        public short bmBitsPixel;
        public IntPtr bmBits;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ICONINFO
    {
        public int fIcon;
        public int xHotspot;
        public int yHotspot;
        public IntPtr hbmMask;
        public IntPtr hbmColor;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct ICONMETRICS
    {
        public int cbSize;
        public int iHorzSpacing;
        public int iVertSpacing;
        public int iTitleWrap;
        public LOGFONT lfFont;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct BITDATA
    {
        public byte Blue;
        public byte Green;
        public byte Red;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct BITDATAREV
    {
        public byte Red;
        public byte Green;
        public byte Blue;
    }


    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct LOGFONT
    {
        public int lfHeight;
        public int lfWidth;
        public int lfEscapement;
        public int lfOrientation;
        public int lfWeight;
        public byte lfItalic;
        public byte lfUnderline;
        public byte lfStrikeOut;
        public byte lfCharSet;
        public byte lfOutPrecision;
        public byte lfClipPrecision;
        public byte lfQuality;
        public byte lfPitchAndFamily;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string lfFaceName;
    }



}