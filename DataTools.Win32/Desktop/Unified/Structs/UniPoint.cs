using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using DataTools.Text;
using DataTools.Win32;

namespace DataTools.Desktop.Unified
{
    
    /// <summary>
    /// Unified point structure for WinForms, WPF and the Win32 API.
    /// </summary>
    /// <remarks></remarks>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct UniPoint
    {
        private double _x;
        private double _y;

        public double X
        {
            get
            {
                return _x;
            }

            set
            {
                _x = value;
            }
        }

        public double Y
        {
            get
            {
                return _y;
            }

            set
            {
                _y = value;
            }
        }

        public static explicit operator PointF(UniPoint operand)
        {
            return new PointF((float)operand.X, (float)operand.Y);
        }

        public static implicit operator UniPoint(PointF operand)
        {
            return new UniPoint(operand.X, operand.Y);
        }

        public static explicit operator Point(UniPoint operand)
        {
            return new Point((int)operand.X, (int)operand.Y);
        }

        public static implicit operator UniPoint(Point operand)
        {
            return new UniPoint(operand);
        }

        public static explicit operator System.Windows.Point(UniPoint operand)
        {
            return new System.Windows.Point(operand.X, operand.Y);
        }

        public static implicit operator UniPoint(System.Windows.Point operand)
        {
            return new UniPoint(operand);
        }

        public static explicit operator W32POINT(UniPoint operand)
        {
            return new W32POINT((int)operand.X, (int)operand.Y);
        }

        public static implicit operator UniPoint(W32POINT operand)
        {
            return new UniPoint(operand);
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", X, Y);
        }

        public UniPoint(Point p)
        {
            _x = p.X;
            _y = p.Y;
        }

        public UniPoint(System.Windows.Point p)
        {
            _x = p.X;
            _y = p.Y;
        }

        public UniPoint(W32POINT p)
        {
            _x = p.x;
            _y = p.y;
        }

        public UniPoint(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public UniPoint(double x, double y)
        {
            _x = x;
            _y = y;
        }
    }
}
