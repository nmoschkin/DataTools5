// ' ************************************************* ''
// ' DataTools Visual Basic Utility Library 
// '
// ' Module: Bitwise LShift and RShift for Streams
// ' 
// ' Copyright (C) 2011-2020 Nathan Moschkin
// ' All Rights Reserved
// '
// ' Licensed Under the Microsoft Public License   
// ' ************************************************* ''

using System;
using Microsoft.VisualBasic.CompilerServices;

namespace DataTools.BitStream
{
    public static class BitStreamShift
    {

        // ' Let's make it simple

        internal const string BadArgs = "Valid objects are arrays of byte, sbyte, short, ushort, int, uint, or long, ulong.";

        /// <summary>
        /// Shifts an array of integral typed elements to the left by the specified number of bits.
        /// Valid objects are arrays of byte, sbyte, short, ushort, int, uint, or long, ulong.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="shift"></param>
        public static void LShiftN(ref object input, int shift)
        {
            var t = input.GetType().GetElementType();
            if (!t.IsArray)
            {
                throw new ArgumentException(BadArgs, "input");
            }

            switch (t)
            {
                case var @case when @case == typeof(byte):
                case var case1 when case1 == typeof(sbyte):
                case var case2 when case2 == typeof(short):
                case var case3 when case3 == typeof(ushort):
                case var case4 when case4 == typeof(int):
                case var case5 when case5 == typeof(uint):
                case var case6 when case6 == typeof(long):
                case var case7 when case7 == typeof(ulong):
                    {
                        break;
                    }

                default:
                    {
                        throw new ArgumentException(BadArgs, "input");
                        break;
                    }
            }

            object o = Array.CreateInstance(t, 1);
            int e = Microsoft.VisualBasic.Strings.Len(o((object)0)) * 8;
            object b;
            int c = Conversions.ToInteger(input.Length);
            int i;
            object b1;
            if (shift >= e)
            {
                int m = (int)Math.Floor(shift / (double)e);
                b = Array.CreateInstance(t, c);
                Array.Copy(input, m, b, (object)0, (object)(c - m));
                input = null;
                input = b;
                shift -= m * e;
            }

            if (shift == 0)
                return;
            c -= 1;
            var loopTo = c;
            for (i = 0; i <= loopTo; i++)
            {
                if (i < c)
                {
                    b1 = input(i);
                    b1 = Operators.OrObject(Operators.LeftShiftObject(b1, shift), Operators.RightShiftObject(input((object)(i + 1)), e - shift));
                    input(i) = b1;
                }
                else
                {
                    input(i) <<= shift;
                }
            }
        }

        /// <summary>
        /// Shifts an array of integral typed elements to the right by the specified number of bits.
        /// Valid objects are arrays of byte, sbyte, short, ushort, int, uint, or long, ulong.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="shift"></param>
        public static void RShiftN(ref object input, int shift)
        {
            var t = input.GetType().GetElementType();
            if (!t.IsArray)
            {
                throw new ArgumentException(BadArgs, "input");
            }

            switch (t)
            {
                case var @case when @case == typeof(byte):
                case var case1 when case1 == typeof(sbyte):
                case var case2 when case2 == typeof(short):
                case var case3 when case3 == typeof(ushort):
                case var case4 when case4 == typeof(int):
                case var case5 when case5 == typeof(uint):
                case var case6 when case6 == typeof(long):
                case var case7 when case7 == typeof(ulong):
                    {
                        break;
                    }

                default:
                    {
                        throw new ArgumentException(BadArgs, "input");
                        break;
                    }
            }

            object o = Array.CreateInstance(t, 1);
            int e = Microsoft.VisualBasic.Strings.Len(o((object)0)) * 8;
            object b;
            int c = Conversions.ToInteger(input.Length);
            int i;
            object b1;
            if (shift >= e)
            {
                int m = (int)Math.Floor(shift / (double)e);
                b = Array.CreateInstance(t, c);
                Array.Copy(input, (object)0, b, m, (object)(c - m));
                input = null;
                input = b;
                shift -= m * e;
            }

            if (shift == 0)
                return;
            c -= 1;
            for (i = c; i >= 0; i -= 1)
            {
                if (i > 0)
                {
                    b1 = input(i);
                    b1 = Operators.OrObject(Operators.RightShiftObject(b1, shift), Operators.LeftShiftObject(input((object)(i - 1)), e - shift));
                    input(i) = b1;
                }
                else
                {
                    input(i) >>= shift;
                }
            }
        }
    }
}