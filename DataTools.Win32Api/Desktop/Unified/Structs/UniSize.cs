using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using DataTools.Text;
using DataTools.Win32Api;

namespace DataTools.Desktop.Unified
{
    /// <summary>
    /// Unified size structure for WinForms, WPF and the Win32 API.
    /// </summary>
    /// <remarks></remarks>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct UniSize
    {
        private System.Windows.Size _size;

        public double cx
        {
            get
            {
                return _size.Width;
            }

            set
            {
                _size.Width = value;
            }
        }

        public double cy
        {
            get
            {
                return _size.Height;
            }

            set
            {
                _size.Height = value;
            }
        }

        public double Width
        {
            get
            {
                return _size.Width;
            }

            set
            {
                _size.Width = value;
            }
        }

        public double Height
        {
            get
            {
                return _size.Height;
            }

            set
            {
                _size.Height = value;
            }
        }

        public static explicit operator SizeF(UniSize operand)
        {
            return new SizeF((float)operand.cx, (float)operand.cy);
        }

        public static implicit operator UniSize(SizeF operand)
        {
            return new UniSize(operand.Width, operand.Height);
        }

        public static explicit operator Size(UniSize operand)
        {
            return new Size((int)operand.cx, (int)operand.cy);
        }

        public static explicit operator UniSize(Size operand)
        {
            return new UniSize(operand);
        }

        public static implicit operator System.Windows.Size(UniSize operand)
        {
            return new System.Windows.Size(operand.cx, operand.cy);
        }

        public static implicit operator UniSize(System.Windows.Size operand)
        {
            return new UniSize(operand);
        }

        public static explicit operator W32SIZE(UniSize operand)
        {
            return new W32SIZE((int)operand.cx, (int)operand.cy);
        }

        public static explicit operator UniSize(W32SIZE operand)
        {
            return new UniSize(operand);
        }

        public static explicit operator UniSize(string operand)
        {
            var st = TextTools.Split(operand, ",");

            if (st.Length != 2)
                throw new InvalidCastException("That string cannot be converted into a width/height pair.");

            var p = new UniSize();

            p.cx = double.Parse(st[0].Trim());
            p.cy = double.Parse(st[1].Trim());

            return p;
        }

        public UniSize(Size p)
        {
            _size = new System.Windows.Size();

            cx = p.Width;
            cy = p.Height;
        }

        public UniSize(System.Windows.Size p)
        {
            _size = new System.Windows.Size();

            cx = p.Width;
            cy = p.Height;
        }

        public UniSize(W32SIZE p)
        {
            _size = new System.Windows.Size();

            cx = p.cx;
            cy = p.cy;
        }

        public UniSize(int cx, int cy)
        {
            _size = new System.Windows.Size();

            this.cx = cx;
            this.cy = cy;
        }

        public UniSize(double cx, double cy)
        {
            _size = new System.Windows.Size();

            this.cx = cx;
            this.cy = cy;
        }

        public override string ToString()
        {
            string sx = _size.Width.ToString();
            string sy = _size.Height.ToString();
            return sx + "," + sy;
        }


        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// This will universally compare whether this is equals to any object that has valid width and height properties.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsEquals(object obj)
        {
            var pi = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
            var fi = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
            bool xmatch = false;
            bool ymatch = false;

            // compare fields, first.  These sorts of objects are structures, more often than not.
            foreach (var fe in fi)
            {
                switch (fe.Name.ToLower() ?? "")
                {
                    case "cx":
                    case "width":
                    case "x":
                    case "dx":
                    case "_cx":
                    case "_height":
                    case "_x":
                    case "_dx":

                        if (fe.FieldType.IsPrimitive)
                        {
                            if ((double)(fe.GetValue(obj)) == Width)
                            {
                                xmatch = true;
                            }
                        }

                        break;

                    case "cy":
                    case "height":
                    case "y":
                    case "dy":
                    case "_cy":
                    case var @case when @case == "_height":
                    case "_y":
                    case "_dy":

                        if (fe.FieldType.IsPrimitive)
                        {
                            if ((double)(fe.GetValue(obj)) == Height)
                            {
                                ymatch = true;
                            }
                        }

                        break;

                    default:
                        continue;

                }

                if (xmatch & ymatch)
                    return true;
            }

            // now, properties.
            foreach (var pe in pi)
            {
                switch (pe.Name.ToLower() ?? "")
                {
                    case "cx":
                    case "width":
                    case "x":
                    case "dx":
                    case "_cx":
                    case "_height":
                    case "_x":
                    case "_dx":

                        if (pe.PropertyType.IsPrimitive)
                        {
                            if ((double)(pe.GetValue(obj)) == Width)
                            {
                                xmatch = true;
                            }
                        }

                        break;

                    case "cy":
                    case "height":
                    case "y":
                    case "dy":
                    case "_cy":
                    case var case1 when case1 == "_height":
                    case "_y":
                    case "_dy":

                        if (pe.PropertyType.IsPrimitive)
                        {
                            if ((double)(pe.GetValue(obj)) == Height)
                            {
                                ymatch = true;
                            }
                        }

                        break;

                    default:
                        continue;
                }

                if (xmatch & ymatch)
                    break;
            }

            return xmatch & ymatch;
        }

        /// <summary>
        /// More experient functions for known "size" types.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool Equals(System.Windows.Size other)
        {
            return Width == other.Width && Height == other.Height;
        }

        /// <summary>
        /// More experient functions for known "size" types.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool Equals(Size other)
        {
            return Width == other.Width && Height == other.Height;
        }

        /// <summary>
        /// More experient functions for known "size" types.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool Equals(UniSize other)
        {
            return Width == other.Width && Height == other.Height;
        }

        /// <summary>
        /// More experient functions for known "size" types.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool Equals(W32SIZE other)
        {
            return Width == other.cx && Height == other.cy;
        }
    }
}
