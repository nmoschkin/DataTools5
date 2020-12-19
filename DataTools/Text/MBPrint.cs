// ************************************************* ''
// DataTools C# Native Utility Library For Windows 
// Adapted for C#/Xamarin
//
// Module: Prints and Interprets Numeric Values
//         From Any Printed Base.
// 
// Copyright (C) 2011-2015, 2019 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the Microsoft Public License   
// ************************************************* ''

using System;

namespace DataTools.Text
{
    /// <summary>
    /// Multibase number string representation padding types
    /// </summary>
    public enum PadTypes
    {
        None = 0,
        Byte = 1,
        Short = 2,
        Int = 4,
        Long = 8,
        Auto = 10
    }

    /// <summary>
    /// Multiple base number printing using alphanumeric characters: Up to base 62 (by default).
    /// </summary>
    /// <remarks></remarks>
    public static class MBStrings
    {
        private static string MakeBase(int Number, string workChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz")
        {
            if ((Number > workChars.Length) | (Number < 2))
                throw new ArgumentException("workChars", "Number of working characters does not meet or exceed the desired base.");
            return workChars.Substring(0, Number);
        }

        /// <summary>
        /// Returns a number from a string of the given base.
        /// </summary>
        /// <param name="ValueString">The numeric value string to parse.</param>
        /// <param name="Base">The base to use in order to parse the string.</param>
        /// <param name="workChars">Specifies an alternate set of glyphs to use for translation.</param>
        /// <returns>A 64 bit unsigned number.</returns>
        /// <remarks></remarks>
        public static long GetValue(string ValueString, int Base = 10, string workChars = null)
        {
            long i = 0;
            int b;
            int j;

            string mbStr;

            string s;
            string x;

            if (!string.IsNullOrEmpty(workChars) && workChars.Length >= Base)
                mbStr = MakeBase(Base, workChars);
            else
                mbStr = MakeBase(Base);

            if ((string.IsNullOrEmpty(mbStr)))
                return 0;

            s = ValueString;
            b = s.Length;

            for (j = 0; j < b; j++)
            {
                x = s.Substring(j, 1);

                i = (i * Base) + mbStr.IndexOf(x, 0);
            }

            return i;
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
        public static string GetString(object value, int Base = 10, PadTypes PadType = PadTypes.Auto, string workChars = null)
        {
            string MBStringRet;
            long varWork;
            int b;
            long j = 0;
            string mbStr;
            string s;

            int sLen = 0;

            switch (PadType)
            {
                case PadTypes.Long:
                    sLen = GetMaxPadLong(Base);
                    break;

                case PadTypes.Int:
                    sLen = GetMaxPadInt(Base);
                    break;

                case PadTypes.Short:
                    sLen = GetMaxPadShort(Base);
                    break;

                case PadTypes.Byte:
                    sLen = GetMaxPadByte(Base);
                    break;

                case PadTypes.Auto:

                    if (value is long || value is ulong)
                    {
                        sLen = GetMaxPadLong(Base);
                    }
                    else if (value is int || value is uint)
                    {
                        sLen = GetMaxPadInt(Base);
                    }
                    else if (value is short || value is ushort)
                    {
                        sLen = GetMaxPadShort(Base);
                    }
                    else if (value is byte || value is sbyte)
                    {
                        sLen = GetMaxPadByte(Base);
                    }

                    break;
            }

            varWork = (long)value;
            if (varWork < 0) varWork *= -1;

            b = Base;

            if (!string.IsNullOrEmpty(workChars) && workChars.Length >= Base)
                mbStr = MakeBase(Base, workChars);
            else
                mbStr = MakeBase(Base);

            if ((mbStr == null))
                throw new ArgumentNullException("workChars", "Cannot work with a null glyph set.");
            s = "";

            while (varWork > 0)
            {
                if (varWork >= (long)b)
                {
                    j = (uint)(varWork % (long)b);
                }
                else
                {
                    j = (long)varWork;
                }

                s = mbStr.Substring((int)j, 1) + s;

                if (varWork < (long)b) break;

                varWork = (varWork - j) / (long)b;
            }

            if (sLen > 0 && (sLen - s.Length) > 0)
                s = new string(mbStr[0], sLen - s.Length) + s;

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
            int GetMaxPadLongRet;
            long sVal;

            sVal = 0x7FFFFFFFFFFFFFFF;
            GetMaxPadLongRet = (GetString(sVal, Base, PadTypes.None)).ToString().Length;
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

            sVal = 0xFFFFFFFF;
            return (GetString(sVal, Base, PadTypes.None)).ToString().Length;
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
            return (GetString(sVal, Base, PadTypes.None)).ToString().Length;
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
            return (GetString(sVal, Base, PadTypes.None)).ToString().Length;
        }
    }
}
