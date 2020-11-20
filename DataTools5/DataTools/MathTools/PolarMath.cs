// Polar coordinate translation
// Copyright (C) 2014 Nathaniel Moschkin

// This is the fourth rewriting of this module since 1999.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;

namespace DataTools.MathTools.PolarMath
{

    public enum PolarCoordinatesFormattingFlags
    {
        Default = 0x0,
        UseDegreeSymbol = 0x1,
        UsePolarSymbol = 0x2,
        UsePiSymbol = 0x4,
        UseRadianIndicator = 0x8,
        UseParenthesis = 0x10,
        UseBrackets = 0x20,
        DisplayInRadians = 0x40,
        Standard = 0x1,
        Radians = 0x4 | 0x8 | 0x40,
        StandardScientific = 0x2,
        RadiansScientific = 0x2 | 0x4 | 0x8 | 0x40
    }

    public struct LinearCoordinates
    {
        public double X;
        public double Y;

        public LinearCoordinates(double x, double y)
        {
            X = x;
            Y = y;
        }
    }

    public struct LinearSize
    {
        public double Width;
        public double Height;

        public LinearSize(double width, double height)
        {
            this.Width = width;
            this.Height = height;
        }
    }

    public struct LinearRect
    {
        public double Left;
        public double Top;
        public double Right;
        public double Bottom;

        public LinearRect(double left, double top, double right, double bottom)
        {
            this.Left = left;
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;
        }

        public LinearRect(LinearCoordinates leftTop, LinearSize size)
        {
            Left = leftTop.X;
            Top = leftTop.Y;

            Right = (leftTop.X + size.Width) - 1;
            Bottom = (leftTop.Y + size.Height) - 1;
        }

        public double Width
        {
            get => (Right - Left) + 1;
            set
            {
                Right = (Left + value) - 1;
            }
        }


        public double Height
        {
            get => (Bottom - Top) + 1;
            set
            {
                Bottom = (Top + value) - 1;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PolarCoordinates
    {
        public const string PolarSymbol = "φ";
        public const string PiSymbol = "π";
        public const string DegreeSymbol = "°";
        public const double RadianConst = 180.0d / 3.1415926535897931d;

        public double Arc { get; set; }
        public double Radius { get; set; }

        public PolarCoordinates(double radius, double angle)
        {
            Radius = radius;
            Arc = angle;
        }

        public PolarCoordinates(PolarCoordinates p)
        {
            Radius = p.Radius;
            Arc = p.Arc;
        }

        public string ToString(PolarCoordinatesFormattingFlags formatting, int precision = 2)
        {
            string s = "";
            
            double a = Arc;
            double r = Radius;
        
            if ((formatting & PolarCoordinatesFormattingFlags.UseParenthesis) == PolarCoordinatesFormattingFlags.UseParenthesis)
            {
                s += "(";
            }

            if ((formatting & PolarCoordinatesFormattingFlags.UseBrackets) == PolarCoordinatesFormattingFlags.UseBrackets)
            {
                s += "{";
            }

            s += Math.Round(r, precision).ToString() + ", ";
            if ((formatting & PolarCoordinatesFormattingFlags.DisplayInRadians) == PolarCoordinatesFormattingFlags.DisplayInRadians)
            {
                a *= RadianConst;
            }

            s += Math.Round(a, precision).ToString();
            if ((formatting & PolarCoordinatesFormattingFlags.UsePiSymbol) == PolarCoordinatesFormattingFlags.UsePiSymbol)
            {
                s += PiSymbol;
            }

            if ((formatting & PolarCoordinatesFormattingFlags.UseDegreeSymbol) == PolarCoordinatesFormattingFlags.UseDegreeSymbol)
            {
                s += DegreeSymbol;
            }

            if ((formatting & PolarCoordinatesFormattingFlags.UsePolarSymbol) == PolarCoordinatesFormattingFlags.UsePolarSymbol)
            {
                s += PolarSymbol;
            }

            if ((formatting & PolarCoordinatesFormattingFlags.UseRadianIndicator) == PolarCoordinatesFormattingFlags.UseRadianIndicator)
            {
                s += " rad";
            }

            if ((formatting & PolarCoordinatesFormattingFlags.UseBrackets) == PolarCoordinatesFormattingFlags.UseBrackets)
            {
                s += "}";
            }

            if ((formatting & PolarCoordinatesFormattingFlags.UseParenthesis) == PolarCoordinatesFormattingFlags.UseParenthesis)
            {
                s += ")";
            }

            return s;

        }

        public override string ToString()
        {
            return ToString(PolarCoordinatesFormattingFlags.Standard);
        }

        public static LinearCoordinates ToLinearCoordinates(PolarCoordinates p)
        {
            return ToLinearCoordinates(p.Radius, p.Arc);
        }

        public static LinearCoordinates ToLinearCoordinates(double r, double a)
        {
            double x;
            double y;
            
            a /= RadianConst;

            y = r * Math.Cos(a);
            x = r * Math.Sin(a);

            return new LinearCoordinates(x, -y);
        }

        public static LinearCoordinates ToLinearCoordinates(PolarCoordinates p, LinearRect rect)
        {
            if (rect.Width < p.Radius * 2d + 1d || rect.Height < p.Radius * 2d + 1d)
            {
                // fit to rectangle
                p = new PolarCoordinates(Math.Min(rect.Width, rect.Height) / 2d - 1d, p.Arc);
            }

            var pt = ToLinearCoordinates(p.Radius, p.Arc);

            double x;
            double y;

            x = (rect.Width / 2d) + pt.X;
            y = (rect.Height / 2d) + pt.Y;

            return new LinearCoordinates(x + rect.Left, y + rect.Top);
        }

        public static PolarCoordinates ToPolarCoordinates(LinearCoordinates p)
        {
            return ToPolarCoordinates(p.X, p.Y);

        }

        public static PolarCoordinates ToPolarCoordinates(double x, double y)
        {
            double r;
            double a;

            r = Math.Sqrt((x * x) + (y * y));

            // screen coordinates are funny, had to reverse this.
            a = Math.Atan(x / y);
            
            a *= RadianConst;
            
            if (x < 0 && y < 0)
            {
                a = 360 - a;
            }
            else if (x >= 0 && y < 0)
            {
                // do nothing
            }
            else if (x >= 0 && y >= 0)
            {
                a = 180 - a;
            }
            else if (x < 0 && y >= 0)
            {
                a =90 + (90 - a);
            }


            if (a < 0.0d)
                a = 360.0d - a;

            if (a > 360.0d)
                a = a - 360.0d;

            return new PolarCoordinates(r, a);
        }

        public static bool InWheel(LinearCoordinates pt, double rad)
        {
            pt.X -= rad;
            pt.Y -= rad;
            var p = ToPolarCoordinates(pt);
            return p.Radius <= rad;
        }
       
       
        public static explicit operator PolarCoordinates(LinearCoordinates operand)
        {
            return ToPolarCoordinates(operand);
        }
    }

}