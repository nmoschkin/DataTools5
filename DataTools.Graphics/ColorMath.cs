// ColorMath class.  Copyright (C) 1999-2014 Nathaniel Moschkin.
// This is the 4th writing of this class.

// This most recent version uses clarified equations harvested from Wikipedia.
// The remainder of the code was written much prior... and was correctly
// achieved through much trial and error.

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using DataTools.Desktop.Unified;

namespace DataTools.Graphics
{
    public class ColorMath
    {
        public const int SF_VALUE = 0x0;
        public const int SF_SATURATION = 0x1;
        public const int SF_HUE = 0x2;

        public static string Cex(UniColor color)
        {
            string s = "";
            byte[] b = (byte[])color;
            for (int i = 0; i <= 3; i++)
                s += b[i].ToString("X2");
            return "#" + s.ToLower();
        }

        public static UniColor UnCex(string value)
        {
            string[] s;
            s = new string[4];
            s[0] = "FF";
            if (value.Substring(0, 1) != "#")
                return 0;
            value = value.Substring(1);
            if (value.Length == 2)
            {
                s[1] = value.Substring(0, 2);
                s[2] = value.Substring(0, 2);
                s[3] = value.Substring(0, 2);
            }
            else if (value.Length == 3)
            {
                s[1] = value.Substring(0, 1) + value.Substring(0, 1);
                s[2] = value.Substring(1, 1) + value.Substring(1, 1);
                s[3] = value.Substring(2, 1) + value.Substring(2, 1);
            }
            else if (value.Length == 4)
            {
                s[0] = value.Substring(0, 1) + value.Substring(0, 1);
                s[1] = value.Substring(0, 1) + value.Substring(1, 1);
                s[2] = value.Substring(1, 1) + value.Substring(2, 1);
                s[3] = value.Substring(2, 1) + value.Substring(3, 1);
            }
            else if (value.Length == 6)
            {
                s[1] = value.Substring(0, 2);
                s[2] = value.Substring(2, 2);
                s[3] = value.Substring(4, 2);
            }
            else if (value.Length == 8)
            {
                s[0] = value.Substring(0, 2);
                s[1] = value.Substring(2, 2);
                s[2] = value.Substring(4, 2);
                s[3] = value.Substring(6, 2);
            }
            else
            {
                throw new ArgumentException("Color is in an incorrect format.", nameof(value));
            }

            try
            {
                byte a = byte.Parse(s[0], System.Globalization.NumberStyles.HexNumber);
                byte r = byte.Parse(s[1], System.Globalization.NumberStyles.HexNumber);
                byte g = byte.Parse(s[2], System.Globalization.NumberStyles.HexNumber);
                byte b = byte.Parse(s[3], System.Globalization.NumberStyles.HexNumber);
                return new UniColor(a, r, g, b);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Color is in an incorrect format.", nameof(value), ex);
            }
        }

        public static void GetRGB(UniColor color, out byte red, out byte green, out byte blue)
        {
            int crColor = ((Color)color).ToArgb();
            red = (byte)(crColor & 0xFF);
            green = (byte)(crColor >> 8 & 0xFF);
            blue = (byte)(crColor >> 16 & 0xFF);
        }

        // Single Convert ColorRef to RGB

        public static void ColorToRGB(UniColor color, out RGBDATA bits)
        {
            byte[] b;
            b = BitConverter.GetBytes(((Color)color).ToArgb());
            bits.Red = b[2];
            bits.Green = b[1];
            bits.Blue = b[0];
        }

        public static void ColorToARGB(UniColor color, out ARGBDATA bits)
        {
            byte[] b;
            b = BitConverter.GetBytes(((Color)color).ToArgb());
            bits.Alpha = b[3];
            bits.Red = b[2];
            bits.Green = b[1];
            bits.Blue = b[0];
        }

        // Single Convert RGB to ColorRef

        public static UniColor RGBToColor(RGBDATA bits)
        {
            return Color.FromArgb(255, bits.Red, bits.Green, bits.Blue);
        }

        public static UniColor ARGBToColor(ARGBDATA bits)
        {
            return Color.FromArgb(bits.Alpha, bits.Red, bits.Green, bits.Blue);
        }

        // Single Convert ColorRef to RGB-reversed

        public static void ColorToBGR(UniColor color, out BGRDATA bits)
        {
            var tibs = new RGBDATA();
            ColorToRGB(color, out tibs);
            bits.Blue = tibs.Blue;
            bits.Red = tibs.Red;
            bits.Green = tibs.Green;
        }

        public static void ColorToBGRA(UniColor Color, out BGRADATA bits)
        {
            var tibs = new ARGBDATA();
            ColorToARGB(Color, out tibs);
            bits.Alpha = tibs.Blue;
            bits.Blue = tibs.Blue;
            bits.Red = tibs.Red;
            bits.Green = tibs.Green;
        }

        // Single Convert RGB-reversed to ColorRef

        public static UniColor BGRAToColor(BGRADATA Bits)
        {
            var tibs = new ARGBDATA();
            tibs.Alpha = Bits.Alpha;
            tibs.Red = Bits.Red;
            tibs.Blue = Bits.Blue;
            tibs.Green = Bits.Green;
            return ARGBToColor(tibs);
        }

        public static UniColor BGRToColor(BGRDATA Bits)
        {
            var tibs = new RGBDATA();
            tibs.Red = Bits.Red;
            tibs.Blue = Bits.Blue;
            tibs.Green = Bits.Green;
            return RGBToColor(tibs);
        }

        public static double Min(params double[] vars)
        {
            double d;
            int i;
            int c = vars.Length;
            if (c <= 0)
                return double.NaN;
            d = vars[0];
            for (i = 1; i < c; i++)
                d = Math.Min(d, vars[i]);
            return d;
        }

        public static double Max(params double[] vars)
        {
            double d;
            int i;
            int c = vars.Length;
            if (c <= 0)
                return double.NaN;
            d = vars[0];

            for (i = 1; i < c; i++)
                d = Math.Max(d, vars[i]);
            return d;
        }

        public static float Min(params float[] vars)
        {
            float d;
            int i;
            int c = vars.Length;
            if (c <= 0)
                return float.NaN;
            d = vars[0];
            for (i = 1; i < c; i++)
                d = Math.Min(d, vars[i]);
            return d;
        }

        public static float Max(params float[] vars)
        {
            float d;
            int i;
            int c = vars.Length;
            if (c <= 0)
                return float.NaN;
            d = vars[0];
            for (i = 1; i < c; i++)
                d = Math.Max(d, vars[i]);
            return d;
        }

        public static decimal Min(params decimal[] vars)
        {
            decimal d;
            int i;
            int c = vars.Length;
            if (c <= 0)
                return decimal.Zero;
            d = vars[0];
            for (i = 1; i < c; i++)
                d = Math.Min(d, vars[i]);
            return d;
        }

        public static decimal Max(params decimal[] vars)
        {
            decimal d;
            int i;
            int c = vars.Length;
            if (c <= 0)
                return decimal.Zero;
            d = vars[0];
            for (i = 1; i < c; i++)
                d = Math.Max(d, vars[i]);
            return d;
        }

        public static long Min(params long[] vars)
        {
            long d;
            int i;
            int c = vars.Length;
            if (c <= 0)
                return 0L;
            d = vars[0];
            for (i = 1; i < c; i++)
                d = Math.Min(d, vars[i]);
            return d;
        }

        public static long Max(params long[] vars)
        {
            long d;
            int i;
            int c = vars.Length;
            if (c <= 0)
                return 0L;
            d = vars[0];
            for (i = 1; i < c; i++)
                d = Math.Max(d, vars[i]);
            return d;
        }

        public static int Min(params int[] vars)
        {
            int d;
            int i;
            int c = vars.Length;
            if (c <= 0)
                return 0;
            d = vars[0];
            for (i = 1; i < c; i++)
                d = Math.Min(d, vars[i]);
            return d;
        }

        public static int Max(params int[] vars)
        {
            int d;
            int i;
            int c = vars.Length;
            if (c <= 0)
                return 0;
            d = vars[0];
            for (i = 1; i < c; i++)
                d = Math.Max(d, vars[i]);
            return d;
        }

        public static short Min(params short[] vars)
        {
            short d;
            int i;
            int c = vars.Length;
            if (c <= 0)
                return 0;
            d = vars[0];
            for (i = 1; i < c; i++)
                d = Math.Min(d, vars[i]);
            return d;
        }

        public static short Max(params short[] vars)
        {
            short d;
            int i;
            int c = vars.Length;
            if (c <= 0)
                return 0;
            d = vars[0];
            for (i = 1; i < c; i++)
                d = Math.Max(d, vars[i]);
            return d;
        }

        public static byte Min(params byte[] vars)
        {
            byte d;
            int i;
            int c = vars.Length;
            if (c <= 0)
                return 0;
            d = vars[0];
            for (i = 1; i < c; i++)
                d = Math.Min(d, vars[i]);
            return d;
        }

        public static byte Max(params byte[] vars)
        {
            byte d;
            int i;
            int c = vars.Length;
            if (c <= 0)
                return 0;
            d = vars[0];
            for (i = 1; i < c; i++)
                d = Math.Max(d, vars[i]);
            return d;
        }

        public static ulong Min(params ulong[] vars)
        {
            ulong d;
            int i;
            int c = vars.Length;
            if (c <= 0)
                return 0UL;
            d = vars[0];
            for (i = 1; i < c; i++)
                d = Math.Min(d, vars[i]);
            return d;
        }

        public static ulong Max(params ulong[] vars)
        {
            ulong d;
            int i;
            int c = vars.Length;
            if (c <= 0)
                return 0UL;
            d = vars[0];
            for (i = 1; i < c; i++)
                d = Math.Max(d, vars[i]);
            return d;
        }

        public static uint Min(params uint[] vars)
        {
            uint d;
            int i;
            int c = vars.Length;
            if (c <= 0)
                return 0U;
            d = vars[0];
            for (i = 1; i < c; i++)
                d = Math.Min(d, vars[i]);
            return d;
        }

        public static uint Max(params uint[] vars)
        {
            uint d;
            int i;
            int c = vars.Length;
            if (c <= 0)
                return 0U;
            d = vars[0];
            for (i = 1; i < c; i++)
                d = Math.Max(d, vars[i]);
            return d;
        }

        public static ushort Min(params ushort[] vars)
        {
            ushort d;
            int i;
            int c = vars.Length;
            if (c <= 0)
                return 0;
            d = vars[0];
            for (i = 1; i < c; i++)
                d = Math.Min(d, vars[i]);
            return d;
        }

        public static ushort Max(params ushort[] vars)
        {
            ushort d;
            int i;
            int c = vars.Length;
            if (c <= 0)
                return 0;
            d = vars[0];
            for (i = 1; i < c; i++)
                d = Math.Max(d, vars[i]);
            return d;
        }

        public static sbyte Min(params sbyte[] vars)
        {
            sbyte d;
            int i;
            int c = vars.Length;
            if (c <= 0)
                return 0;
            d = vars[0];
            for (i = 1; i < c; i++)
                d = Math.Min(d, vars[i]);
            return d;
        }

        public static sbyte Max(params sbyte[] vars)
        {
            sbyte d;
            int i;
            int c = vars.Length;
            if (c <= 0)
                return 0;
            d = vars[0];
            for (i = 1; i < c; i++)
                d = Math.Max(d, vars[i]);
            return d;
        }

       
        public static HSVDATA ColorToHSV(UniColor color)
        {
            HSVDATA hsv = new HSVDATA();
            ColorToHSV(color, ref hsv);
            return hsv;
        }

        public static void ColorToHSV(UniColor color, ref HSVDATA hsv)
        {
            double h, s, v;

            ColorToHSV(color, out h, out s, out v);

            hsv.Hue = h;
            hsv.Saturation = s;
            hsv.Value = v;
        }

        public static void ColorToHSV(UniColor color, out double hue, out double saturation, out double value)
        {
            hue = default(double);

            double sat;
            double val;
            double Mn;
            double Mx;
            double r;
            double g;
            double b;
            double chroma;


            r = color.R / 255d;
            g = color.G / 255d;
            b = color.B / 255d;

            Mn = Min(r, g, b);
            Mx = Max(r, g, b);

            chroma = Mx - Mn;
            val = Mx;

            if (chroma == 0)
            {
                hue = -1;
                
                switch (val)
                {
                    case var @case when @case <= 0.5d:
                        saturation = 1d;
                        value = 510d * val / 360d;
                        break;

                    case var case1 when case1 <= 1d:
                    default:
                        val = 1d - val;
                        value = 1d;
                        saturation = 720d * val / 360d;
                        break;
                }

                return;
            }

            if (Mx == r)
            {
                hue = (g - b) / chroma % 6d;
            }
            else if (Mx == g)
            {
                hue = (b - r) / chroma + 2d;
            }
            else if (Mx == b)
            {
                hue = (r - g) / chroma + 4d;
            }

            hue *= 60d;
            if (hue < 0d)
                hue = 360d + hue;

            sat = val != 0 ? chroma / val : 0;

            value = val;
            saturation = sat;
        }

        public static UniColor HSVToColor(HSVDATA hsv)
        {
            int c = 0;
            HSVToColorRaw(hsv, ref c);
            return c;
        }

        public static void HSVToColor(HSVDATA hsv, ref UniColor dest)
        {
            // preserve alpha
            var a = dest.A;
            dest.SetValue(HSVToColorRaw(hsv));
            dest.A = a;
        }

        /// <summary>
        /// Convert Hue, Saturation, Value to a raw 32 bit color value.
        /// </summary>
        /// <param name="hsv">The hue, saturation, value to convert.</param>
        /// <returns></returns>
        public static int HSVToColorRaw(HSVDATA hsv)
        {
            int c = 0;
            HSVToColorRaw(hsv, ref c);
            return c;
        }
        public static void HSVToColorRaw(HSVDATA hsv, ref int retColor)
        {
            HSVToColorRaw(hsv.Hue, hsv.Saturation, hsv.Value, ref retColor);            
        }

        // http://en.wikipedia.org/wiki/HSL_and_HSV#Hue_and_chroma
        // I adapted the equation from the one in Wikipedia.
        // I wish I could offer a better explanation.  But this isn't wikipedia, and they'd do a better job.
        //
        public static void HSVToColorRaw(double hue, double saturation, double value, ref int retColor)
        {
            unchecked
            {
                double a;
                double b;
                double c;
                
                int ab;
                int bb;
                int cb;
                
                double chroma;
                double Mx;
                double Mn;
                
                int j = (int)0xFF000000;
                
                double n;
                
                if (hue >= 360d)
                    hue -= 360d;
                
                if (hue == -1)
                {
                    if (saturation > value)
                    {
                        saturation = 1d;
                        n = value * 360d / 510d;
                    }
                    else
                    {
                        n = 1d - saturation * 360d / 720d;
                    }

                    ab = (int)(n * 255d);
                    
                    
                    retColor = j | (ab << 16) | (ab << 8) | ab;
                    return;
                }

                chroma = value * saturation;
                
                Mn = Math.Abs(value - chroma);
                Mx = value;
            
                n = hue / 60d;
                
                a = Mx;
                
                c = Mn;
                
                b = chroma * (1d - Math.Abs(n % 2d - 1d));
                b += c;

                // fit the color space in to byte space.
                ab = (int)Math.Round(a * 255d);
                bb = (int)Math.Round(b * 255d);
                cb = (int)Math.Round(c * 255d);

                // Get the floored value of n
                n = Math.Floor(n);
                switch (n)
                {
                    case 0d:
                    case 6d: // 0, 360 - Red
                        j = j | ab << 16 | bb << 8 | cb;
                        break;

                    case 1d: // 60 - Yellow
                        j = j | bb << 16 | ab << 8 | cb;
                        break;

                    case 2d: // 120 - Green
                        j = j | cb << 16 | ab << 8 | bb;
                        break;

                    case 3d: // 180 - Cyan
                        j = j | cb << 16 | bb << 8 | ab;
                        break;

                    case 4d: // 240 - Blue
                        j = j | bb << 16 | cb << 8 | ab;
                        break;

                    case 5d: // 300 - Magenta
                        j = j | ab << 16 | cb << 8 | bb;
                        break;
                }

                retColor = j;
                return;
            }
        }

        public static void ColorToCMY(UniColor Color, ref CMYDATA cmy)
        {
            //
            byte r;
            byte g;
            byte b;
            
            int x;
            
            GetRGB(Color, out r, out g, out b);
            
            x = Max(r, g, b);
            
            r = (byte)Math.Abs(1 - r);
            g = (byte)Math.Abs(1 - g);
            b = (byte)Math.Abs(1 - b);
        
            cmy.Magenta = r;
            cmy.Yellow = g;
            cmy.Cyan = b;
        }

        public static UniColor CMYToColor(CMYDATA cmy)
        {
            UniColor CMYToColorRet = default;
            //
            int c;
            int m;
            int y;

            byte r;
            byte g;
            byte b;

            c = cmy.Cyan;
            m = cmy.Magenta;
            y = cmy.Yellow;

            r = (byte)Math.Abs(1 - m);
            g = (byte)Math.Abs(1 - y);
            b = (byte)Math.Abs(1 - c);

            return new UniColor(255, r, g, b);
        }

        public static void GetPercentages(UniColor Color, ref double dpRed, ref double dpGreen, ref double dpBlue)
        {
            double vR;
            double vB;
            double vG;
            var vIn = new byte[3];
            double d;
            GetRGB(Color, out vIn[0], out vIn[1], out vIn[2]);
            vR = vIn[0];
            vG = vIn[1];
            vB = vIn[2];
            if (vR > vG)
                d = vR;
            else
                d = vG;
            if (vB > d)
                d = vB;
            if (d == 0d)
                d = 255.0d;
            dpRed = vR / d;
            dpGreen = vG / d;
            dpBlue = vB / d;
        }

        public static UniColor AbsTone(UniColor Color)
        {
            UniColor AbsToneRet = default;
            var pR = default(double);
            var pB = default(double);
            var pG = default(double);
            double sR;
            double sB;
            double sG;
            GetPercentages(Color, ref pR, ref pG, ref pB);
            sR = pR * 255d;
            sG = pG * 255d;
            sB = pB * 255d;
            AbsToneRet = new UniColor(Color.A, (byte)sR, (byte)sG, (byte)sB);
            return AbsToneRet;
        }

        public static UniColor SetTone(ref RGBDATA rgbData, float pPercent, UniColor Color = default)
        {
            float x;
            if (Color != default)
                ColorToRGB(Color, out rgbData);
            x = Max(rgbData.Red, rgbData.Green, rgbData.Blue);
            if (x == 0f)
            {
                rgbData.Red = 0;
                rgbData.Green = 0;
                rgbData.Blue = 0;
                return UniColor.Empty;
            }

            rgbData.Red = (byte)(rgbData.Red / x * (255f * pPercent));
            rgbData.Green = (byte)(rgbData.Green / x * (255f * pPercent));
            rgbData.Blue = (byte)(rgbData.Blue / x * (255f * pPercent));
            return RGBToColor(rgbData);
        }

        public static UniColor SetTone(ref ARGBDATA argbData, float pPercent, UniColor Color = default)
        {
            float x;
            if (Color != UniColor.Empty)
                ColorToARGB(Color, out argbData);
            x = Max(argbData.Red, argbData.Green, argbData.Blue);
            if (x == 0f)
            {
                argbData.Red = 0;
                argbData.Green = 0;
                argbData.Blue = 0;
                return UniColor.Empty;
            }

            argbData.Red = (byte)(argbData.Red / x * (255f * pPercent));
            argbData.Green = (byte)(argbData.Green / x * (255f * pPercent));
            argbData.Blue = (byte)(argbData.Blue / x * (255f * pPercent));
            return ARGBToColor(argbData);
        }

        public static UniColor GrayTone(UniColor Color)
        {
            UniColor GrayToneRet = default;
            ARGBDATA rgbData;
            int a;
            int b;
            int c;
            byte tone;
            ColorToARGB(Color, out rgbData);
            a = rgbData.Red;
            b = rgbData.Green;
            c = rgbData.Blue;
            tone = (byte)((a + b + c) / 3d);
            GrayToneRet = new UniColor(rgbData.Alpha, tone, tone, tone);
            return GrayToneRet;
        }

        public static UniColor GetAverageColor(UniColor Color1, UniColor Color2)
        {
            var Bits = new RGBDATA[3];
            float df;
            int e;
            float clr2 = Color2.Value;
            byte al = Color1.A;
            byte af = Color2.A;
            ColorToRGB(Color1, out Bits[0]);
            ColorToRGB(Color2, out Bits[1]);
            e = 0;
            if (Math.Round(clr2) != clr2)
            {
                df = Bits[0].Red * clr2;
                e = (int)Math.Round(df, 0);
                Bits[2].Red = (byte)(e & 0xFF);
                df = Bits[0].Green * clr2;
                e = (int)Math.Round(df, 0);
                Bits[2].Green = (byte)(e & 0xFF);
                df = Bits[0].Blue * clr2;
                e = (int)Math.Round(df, 0);
                Bits[2].Blue = (byte)(e & 0xFF);
            }
            else
            {
                df = (Bits[0].Red + (float)Bits[1].Red) / 2f;
                e = (int)Math.Round(df, 0);
                Bits[2].Red = (byte)(e & 0xFF);
                df = (Bits[0].Green + (float)Bits[1].Green) / 2f;
                e = (int)Math.Round(df, 0);
                Bits[2].Green = (byte)(e & 0xFF);
                df = (Bits[0].Blue + (float)Bits[1].Blue) / 2f;
                e = (int)Math.Round(df, 0);
                Bits[2].Blue = (byte)(e & 0xFF);
            }

            // Get the average alpha
            al = (byte)((al + af) / 2d);
            return new UniColor(al, Bits[2].Red, Bits[2].Green, Bits[2].Blue);
        }
    }
}