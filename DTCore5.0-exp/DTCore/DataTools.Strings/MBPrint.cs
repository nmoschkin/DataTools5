// ' ************************************************* ''
// ' DataTools Visual Basic Utility Library 
// '
// ' Module: Prints and Interprets Numeric Values
// '         From Any Printed Base.
// ' 
// ' Copyright (C) 2011-2020 Nathan Moschkin
// ' All Rights Reserved
// '
// ' Licensed Under the Microsoft Public License   
// ' ************************************************* ''

using System;
using Microsoft.VisualBasic.CompilerServices;

namespace DataTools.Strings
{

    /// <summary>
    /// Multiple base number printing using alphanumeric characters: Up to base 62 (by default).
    /// </summary>
    /// <remarks></remarks>
    public static class Strings
    {
        public enum PadTypes
        {
            None = 0,
            Byte = 1,
            Short = 2,
            Int = 4,
            Long = 8,
            Auto = 10
        }

        private static string MakeBase(int Number, string workChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz")
        {
            if (Number > workChars.Length | Number < 2)
                throw new ArgumentException("workChars", "Number of working characters does not meet or exceed the desired base.");
            return workChars.Substring(0, Number);
        }

        /// <summary>
        /// Returns a number from a string of the given base.
        /// </summary>
        /// <param name="s">The numeric value string to parse.</param>
        /// <param name="Base">The base to use in order to parse the string.</param>
        /// <param name="workChars">Specifies an alternate set of glyphs to use for translation.</param>
        /// <returns>A 64 bit unsigned number.</returns>
        /// <remarks></remarks>
        public static decimal MBValue(string s, int Base = 10, string workChars = null)
        {
            var outNum = default(decimal);
            int c;
            int i;
            string mbStr;
            string x;
            if (!string.IsNullOrEmpty(workChars) && workChars.Length >= Base)
            {
                mbStr = workChars;
            }
            else
            {
                mbStr = MakeBase(Base);
            }

            if (mbStr.Length != Base)
                return 0m;
            c = s.Length;
            var loopTo = c - 1;
            for (i = 0; i <= loopTo; i++)
            {
                x = s.Substring(i, 1);
                outNum = outNum * Base + mbStr.IndexOf(x);
            }

            return outNum;
        }

        /// <summary>
        /// Prints a number as a string of the given base.
        /// </summary>
        /// <param name="value">The value to print (must be an integer type of 8, 16, 32 or 64 bits; floating point values are not allowed).</param>
        /// <param name="Base">Specifies the numeric base used to calculated the printed characters.</param>
        /// <param name="PadType">Specifies the type of padding to use.</param>
        /// <param name="workChars">Specifies an alternate set of glyphs to use for printing.</param>
        /// <returns>A character string representing the input value as printed text in the desired base.</returns>
        /// <remarks></remarks>
        public static string MBString(object value, int Base = 10, PadTypes PadType = PadTypes.Auto, string workChars = null)
        {
            string MBStringRet = default;
            decimal varWork;
            int i;
            int b;
            decimal j;
            string mbStr;
            string s;
            int sLen = 0;
            ;
#error Cannot convert OnErrorResumeNextStatementSyntax - see comment for details
            /* Cannot convert OnErrorResumeNextStatementSyntax, CONVERSION ERROR: Conversion for OnErrorResumeNextStatement not implemented, please report this issue in 'On Error Resume Next' at character 3727


            Input:

                        On Error Resume Next

             */
            if (PadType == PadTypes.Auto)
            {
                switch (value.GetType())
                {
                    case var @case when @case == typeof(long):
                    case var case1 when case1 == typeof(ulong):
                        {
                            PadType = PadTypes.Long;
                            break;
                        }

                    case var case2 when case2 == typeof(int):
                    case var case3 when case3 == typeof(uint):
                        {
                            PadType = PadTypes.Int;
                            break;
                        }

                    case var case4 when case4 == typeof(short):
                    case var case5 when case5 == typeof(ushort):
                        {
                            PadType = PadTypes.Short;
                            break;
                        }

                    case var case6 when case6 == typeof(byte):
                    case var case7 when case7 == typeof(sbyte):
                        {
                            PadType = PadTypes.Byte;
                            break;
                        }

                    default:
                        {
                            return "";
                        }
                }
            }

            switch (PadType)
            {
                case PadTypes.Long:
                    {
                        sLen = GetMaxPadLong(Base);
                        break;
                    }

                case PadTypes.Int:
                    {
                        sLen = GetMaxPadInt(Base);
                        break;
                    }

                case PadTypes.Short:
                    {
                        sLen = GetMaxPadShort(Base);
                        break;
                    }

                case PadTypes.Byte:
                    {
                        sLen = GetMaxPadByte(Base);
                        break;
                    }
            }

            varWork = Math.Abs(Conversions.ToDecimal(value));
            b = Base;
            if (workChars is object && workChars.Length == 0)
            {
                workChars = null;
            }

            if (workChars is null)
            {
                mbStr = MakeBase(Base);
            }
            else
            {
                mbStr = MakeBase(Base, workChars);
            }

            if (mbStr is null)
                throw new ArgumentException("workChars", "Cannot work with a null glyph set.");
            s = "";
            while (varWork > 0m)
            {
                if (varWork >= b)
                {
                    j = varWork % b;
                }
                else
                {
                    j = varWork;
                }

                s = mbStr.Substring((int)j, 1) + s;
                if (varWork < b)
                    break;
                varWork = (varWork - j) / b;
            }

            if (sLen > 0 && sLen - s.Length > 0)
            {
                s = new string(Conversions.ToChar(mbStr.Substring(0, 1)), sLen - s.Length) + s;
            }

            MBStringRet = s;
            return MBStringRet;
        }

        /// <summary>
        /// Calculate the maximum number of glyphs needed to represent a 64-bit number of the given base.
        /// </summary>
        /// <param name="Base"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static int GetMaxPadLong(int Base)
        {
            int GetMaxPadLongRet = default;
            ulong sVal;
            sVal = 0xFFFFFFFFFFFFFFFFUL;
            GetMaxPadLongRet = Microsoft.VisualBasic.Strings.Len(MBString(sVal, Base, PadTypes.None));
            return GetMaxPadLongRet;
        }

        /// <summary>
        /// Calculate the maximum number of glyphs needed to represent a 32-bit number of the given base.
        /// </summary>
        /// <param name="Base"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static int GetMaxPadInt(int Base)
        {
            uint sVal;
            sVal = 0xFFFFFFFFU;
            return Microsoft.VisualBasic.Strings.Len(MBString(sVal, Base, PadTypes.None));
        }

        /// <summary>
        /// Calculate the maximum number of glyphs needed to represent a 16-bit number of the given base.
        /// </summary>
        /// <param name="Base"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static int GetMaxPadShort(int Base)
        {
            ushort sVal;
            sVal = 0xFFFF;
            return Microsoft.VisualBasic.Strings.Len(MBString(sVal, Base, PadTypes.None));
        }

        /// <summary>
        /// Calculate the maximum number of glyphs needed to represent a 8-bit number of the given base.
        /// </summary>
        /// <param name="Base"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static int GetMaxPadByte(int Base)
        {
            byte sVal;
            sVal = 0xFF;
            return Microsoft.VisualBasic.Strings.Len(MBString(sVal, Base, PadTypes.None));
        }
    }
}