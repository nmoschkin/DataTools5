using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using DataTools.Text;

namespace DataTools.Graphics
{
    /// <summary>
    /// Unified size structure for WinForms, WPF and the Win32 API.
    /// </summary>
    /// <remarks></remarks>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct UniSize
    {
        private double _cx;

        private double _cy;

        public double cx
        {
            get => _cx;
            set => _cx = value;
        }

        public double cy
        {
            get => _cy;
            set => _cy = value;
            
        }

        public double Width
        {
            get => _cx;
            set => _cx = value;
        }

        public double Height
        {
            get => _cy;
            set => _cy = value;

        }
        public static explicit operator SizeF(UniSize operand)
        {
            return new SizeF((float)operand.Width, (float)operand.Height);
        }

        public static implicit operator UniSize(SizeF operand)
        {
            return new UniSize(operand.Width, operand.Height);
        }

        public static explicit operator Size(UniSize operand)
        {
            return new Size((int)operand.Width, (int)operand.Height);
        }

        public static implicit operator UniSize(Size operand)
        {
            return new UniSize(operand);
        }

        public static implicit operator (double, double)(UniSize operand)
        {
            return (operand._cx, operand._cy);
        }

        public static implicit operator UniSize((double, double) operand)
        {
            return new UniSize(operand.Item1, operand.Item2);
        }

        public static explicit operator (float, float)(UniSize operand)
        {
            return ((float)operand._cx, (float)operand._cy);
        }

        public static implicit operator UniSize((float, float) operand)
        {
            return new UniSize(operand.Item1, operand.Item2);
        }

        public static explicit operator (int, int)(UniSize operand)
        {
            return ((int)operand._cx, (int)operand._cy);
        }

        public static implicit operator UniSize((int, int) operand)
        {
            return new UniSize(operand.Item1, operand.Item2);
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
            _cx = p.Width;
            _cy = p.Height;
        }

        public UniSize(int cx, int cy)
        {
            _cx = cx;
            _cy = cy;
        }

        public UniSize(double cx, double cy)
        {
            _cx = cx;
            _cy = cy;
        }

        public override string ToString()
        {
            string sx = Width.ToString();
            string sy = Height.ToString();
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
        /// Gets the bytes of this structure as two 64-bit floating point numbers.
        /// </summary>
        /// <returns></returns>
        public byte[] GetDoubleBytes()
        {
            int st = sizeof(double);
            byte[] ret = new byte[2 * st];

            Array.Copy(BitConverter.GetBytes(_cx), 0, ret, 0, st);
            Array.Copy(BitConverter.GetBytes(_cy), 0, ret, st, st);

            return ret;
        }

        /// <summary>
        /// Gets the bytes of this structure as two 32-bit floating point numbers.
        /// </summary>
        /// <returns></returns>
        public byte[] GetFloatBytes()
        {
            int st = sizeof(float);
            byte[] ret = new byte[2 * st];

            Array.Copy(BitConverter.GetBytes((float)_cx), 0, ret, 0, st);
            Array.Copy(BitConverter.GetBytes((float)_cy), 0, ret, st, st);

            return ret;
        }

        /// <summary>
        /// Gets the bytes of this structure as two 32-bit integers.
        /// </summary>
        /// <returns></returns>
        public byte[] GetIntBytes()
        {
            int st = sizeof(int);
            byte[] ret = new byte[2 * st];

            Array.Copy(BitConverter.GetBytes((int)_cx), 0, ret, 0, st);
            Array.Copy(BitConverter.GetBytes((int)_cy), 0, ret, st, st);

            return ret;
        }

        /// <summary>
        /// Create a new <see cref="UniSize"/> structure from bytes.
        /// </summary>
        /// <param name="bytes">The bytes to convert.</param>
        /// <param name="isIntBytes">True if the bytes represent 2 32-bit integers.</param>
        /// <returns></returns>
        public static UniSize FromBytes(byte[] bytes, bool isIntBytes)
        {
            var up = new UniSize();

            if (isIntBytes && bytes.Length >= sizeof(int) * 2)
            {
                up._cx = BitConverter.ToInt32(bytes, 0);
                up._cy = BitConverter.ToInt32(bytes, sizeof(int));

                return up;
            }
            else if (bytes.Length >= sizeof(float) * 2)
            {
                if (bytes.Length >= (sizeof(double) * 2))
                {
                    up._cx = BitConverter.ToDouble(bytes, 0);
                    up._cy = BitConverter.ToDouble(bytes, sizeof(double));

                    return up;

                }
                else
                {
                    up._cx = BitConverter.ToSingle(bytes, 0);
                    up._cy = BitConverter.ToSingle(bytes, sizeof(float));

                    return up;
                }
            }
            else
            {
                throw new ArgumentException("Not enough bytes", nameof(bytes));
            }
        }

        /// <summary>
        /// Copy the structure to the pointer.
        /// </summary>
        /// <param name="ptr"></param>
        public unsafe void CopyTo(int* ptr)
        {
            (int, int) b = ((int, int))this;
            *ptr = b.Item1;
            *(ptr + sizeof(int)) = b.Item2;
        }

        /// <summary>
        /// Copy the structure to the pointer.
        /// </summary>
        /// <param name="ptr"></param>
        public void CopyToInt32SizePtr(IntPtr ptr)
        {
            unsafe
            {
                CopyTo((int*)ptr);
            }
        }

        /// <summary>
        /// Copy the structure to the pointer.
        /// </summary>
        /// <param name="ptr"></param>
        public unsafe void CopyTo(double* ptr)
        {
            (double, double) b = ((double, double))this;
            *ptr = b.Item1;
            *(ptr + sizeof(double)) = b.Item2;
        }

        /// <summary>
        /// Copy the structure to the pointer.
        /// </summary>
        /// <param name="ptr"></param>
        public void CopyToDoubleSizePtr(IntPtr ptr)
        {
            unsafe
            {
                CopyTo((double*)ptr);
            }
        }

        /// <summary>
        /// Copy the structure to the pointer.
        /// </summary>
        /// <param name="ptr"></param>
        public unsafe void CopyTo(float* ptr)
        {
            (float, float) b = ((float, float))this;
            *ptr = b.Item1;
            *(ptr + sizeof(float)) = b.Item2;
        }


        /// <summary>
        /// Copy the structure to the pointer.
        /// </summary>
        /// <param name="ptr"></param>
        public void CopyToFloatSizePtr(IntPtr ptr)
        {
            unsafe
            {
                CopyTo((float*)ptr);
            }
        }


        /// <summary>
        /// Create a new <see cref="UniSize"/> from a pointer.
        /// </summary>
        /// <param name="ptr"></param>
        /// <returns></returns>
        public unsafe static UniSize FromSizePtr(int* ptr)
        {
            var up = new UniSize();

            up._cx = *ptr;
            up._cy = *(ptr + sizeof(int));

            return up;
        }

        /// <summary>
        /// Create a new <see cref="UniSize"/> from a pointer.
        /// </summary>
        /// <param name="ptr"></param>
        /// <returns></returns>
        public static UniSize FromInt32SizePtr(IntPtr ptr)
        {
            unsafe
            {
                return FromSizePtr((int*)ptr);
            }
        }

        /// <summary>
        /// Create a new <see cref="UniSize"/> from a pointer.
        /// </summary>
        /// <param name="ptr"></param>
        /// <returns></returns>
        public unsafe static UniSize FromSizePtr(double* ptr)
        {
            var up = new UniSize();

            up._cx = *ptr;
            up._cy = *(ptr + sizeof(double));

            return up;
        }

        /// <summary>
        /// Create a new <see cref="UniSize"/> from a pointer.
        /// </summary>
        /// <param name="ptr"></param>
        /// <returns></returns>
        public static UniSize FromDoubleSizePtr(IntPtr ptr)
        {
            unsafe
            {
                return FromSizePtr((double*)ptr);
            }
        }


        /// <summary>
        /// Create a new <see cref="UniSize"/> from a pointer.
        /// </summary>
        /// <param name="ptr"></param>
        /// <returns></returns>
        public unsafe static UniSize FromSizePtr(float* ptr)
        {
            var up = new UniSize();

            up._cx = *ptr;
            up._cy = *(ptr + sizeof(float));

            return up;
        }

        /// <summary>
        /// Create a new <see cref="UniSize"/> from a pointer.
        /// </summary>
        /// <param name="ptr"></param>
        /// <returns></returns>
        public static UniSize FromFloatSizePtr(IntPtr ptr)
        {
            unsafe
            {
                return FromSizePtr((float*)ptr);
            }
        }



    }
}
