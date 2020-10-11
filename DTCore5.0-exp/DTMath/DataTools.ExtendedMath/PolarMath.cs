// Polar coordinate translation
// Copyright (C) 2014 Nathaniel Moschkin

// This is the fourth rewriting of this module since 1999.

using System;
using System.Drawing;
using DataTools.ExtendedMath.ColorMath;
using Microsoft.VisualBasic;

namespace DataTools.ExtendedMath
{
    public static class pMath
    {
        public const string vbPolarChar = "φ";
        public const string vbPiChar = "π";
        public const string vbDegreeChar = "°";
        public const double vbRadianConst = 180.0d / 3.1415926535897931d;

        public enum PolarCoordinateFormatting
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
    }

    public class Polar
    {
        public enum ColorWheelShapes
        {
            Point = 1,
            Hexagon = 2
        }

        public struct ColorWheel
        {
            public ColorWheelElement[] Elements;
            public Rectangle Bounds;
            public byte[] Bitmap;

            public Color HitTest(int x, int y)
            {
                foreach (ColorWheelElement e in Elements)
                {
                    foreach (Point f in e.FillPoints)
                    {
                        if (f.X == x & f.Y == y)
                            return e.Color;
                    }
                }

                return Color.Empty;
            }

            public Color HitTest(Point pt)
            {
                return HitTest(pt.X, pt.Y);
            }
        }

        public struct ColorWheelElement
        {
            public Color Color;
            public PolarCoordinates Polar;
            public Point Center;
            public Rectangle Bounds;
            public Point[] FillPoints;
            public ColorWheelShapes Shape;
        }

        public struct PolarCoordinates
        {
            public pMath.PolarCoordinateFormatting Formatting { get; set; }
            public int Rounding { get; set; }
            public double Angle { get; set; }
            public double Radius { get; set; }

            public PolarCoordinates(double radius, double angle)
            {
                Formatting = pMath.PolarCoordinateFormatting.Standard;
                Rounding = 2;
                Radius = radius;
                Angle = angle;
            }

            public PolarCoordinates(PolarCoordinates p)
            {
                Formatting = pMath.PolarCoordinateFormatting.Standard;
                Rounding = 2;
                Radius = p.Radius;
                Angle = p.Angle;
            }

            public override string ToString()
            {
                string s = "";
                double a = Angle;
                double r = Radius;
                if ((Formatting & pMath.PolarCoordinateFormatting.UseParenthesis) == pMath.PolarCoordinateFormatting.UseParenthesis)
                {
                    s += "(";
                }

                if ((Formatting & pMath.PolarCoordinateFormatting.UseBrackets) == pMath.PolarCoordinateFormatting.UseBrackets)
                {
                    s += "{";
                }

                s += Strings.Format(Math.Round(r, Rounding)) + ", ";
                if ((Formatting & pMath.PolarCoordinateFormatting.DisplayInRadians) == pMath.PolarCoordinateFormatting.DisplayInRadians)
                {
                    a *= pMath.vbRadianConst;
                }

                s += Strings.Format(Math.Round(a, Rounding));
                if ((Formatting & pMath.PolarCoordinateFormatting.UsePiSymbol) == pMath.PolarCoordinateFormatting.UsePiSymbol)
                {
                    s += pMath.vbPiChar;
                }

                if ((Formatting & pMath.PolarCoordinateFormatting.UseDegreeSymbol) == pMath.PolarCoordinateFormatting.UseDegreeSymbol)
                {
                    s += pMath.vbDegreeChar;
                }

                if ((Formatting & pMath.PolarCoordinateFormatting.UsePolarSymbol) == pMath.PolarCoordinateFormatting.UsePolarSymbol)
                {
                    s += pMath.vbPolarChar;
                }

                if ((Formatting & pMath.PolarCoordinateFormatting.UseRadianIndicator) == pMath.PolarCoordinateFormatting.UseRadianIndicator)
                {
                    s += " rad";
                }

                if ((Formatting & pMath.PolarCoordinateFormatting.UseBrackets) == pMath.PolarCoordinateFormatting.UseBrackets)
                {
                    s += "}";
                }

                if ((Formatting & pMath.PolarCoordinateFormatting.UseParenthesis) == pMath.PolarCoordinateFormatting.UseParenthesis)
                {
                    s += ")";
                }

                return s;
            }

            public static UniPoint rad(PolarCoordinates p)
            {
                return rad(p, new Rectangle(0, 0, (int)(p.Radius * 2d) + 1, (int)(p.Radius * 2d) + 1));
            }

            public static UniPoint rad(PolarCoordinates p, UniRect rect)
            {
                if (rect.Width < p.Radius * 2d + 1d || rect.Height < p.Radius * 2d + 1d)
                {
                    // ' fit to rectangle
                    p = new PolarCoordinates(Math.Min(rect.Width, rect.Height) / 2d - 1d, p.Angle);
                }

                var pt = ToScreenCoordinates(p);
                double r = p.Radius;
                double x;
                double y;
                var c = new Point((int)(rect.Width / 2d), (int)(rect.Height / 2d));
                x = c.X + pt.X;
                y = c.Y + pt.Y;
                pt = new System.Windows.Point(x - 1d, y - 1d);
                return new UniPoint(x + rect.Left, y + rect.Top);
            }

            public static implicit operator System.Windows.Point(PolarCoordinates operand)
            {
                return ToScreenCoordinates(operand);
            }

            public static explicit operator PolarCoordinates(System.Windows.Point operand)
            {
                return ToPolarCoordinates(operand);
            }
        }

        public static System.Windows.Point ToScreenCoordinates(PolarCoordinates p)
        {
            return ToScreenCoordinates(p.Radius, p.Angle);
        }

        public static System.Windows.Point ToScreenCoordinates(double r, double a)
        {
            double x;
            double y;
            double radConst = 180.0d / 3.1415926535897931d;
            a /= radConst;
            y = r * Math.Cos(a);
            x = r * Math.Sin(a);
            return new System.Windows.Point(x, -y);
        }

        public static PolarCoordinates ToPolarCoordinates(System.Windows.Point p)
        {
            return ToPolarCoordinates(p.X, p.Y);
        }

        public static PolarCoordinates ToPolarCoordinates(double x, double y)
        {
            double r;
            double a;
            double radConst = 180.0d / 3.1415926535897931d;
            r = Math.Sqrt(x * x + y * y);

            // ' screen coordinates are funny, had to reverse this.
            a = Math.Atan(x / y);
            a *= radConst;
            a = a - 180.0d;
            if (a < 0.0d)
                a = 360.0d - a;
            if (a > 360.0d)
                a = a - 360.0d;
            return new PolarCoordinates(r, a);
        }

        public static bool InWheel(System.Windows.Point pt, double rad)
        {
            pt.X -= rad;
            pt.Y -= rad;
            var p = ToPolarCoordinates(pt);
            return p.Radius <= rad;
        }
    }
}