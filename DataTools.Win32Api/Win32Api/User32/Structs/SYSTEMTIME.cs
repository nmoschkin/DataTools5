// ************************************************* ''
// DataTools C# Native Utility Library For Windows - Interop
//
// Module: Native
//         Myriad Windows API Declares
//
// Started in 2000 on Windows 98/ME (and then later 2000).
//
// Still kicking in 2014 on Windows 8.1!
// A whole bunch of pInvoke/Const/Declare/Struct and associated utility functions that have been collected over the years.

// Some enum documentation copied from the MSDN (and in some cases, updated).
// 
// Copyright (C) 2011-2020 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the Microsoft Public License   
// ************************************************* ''



using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using DataTools.Memory;
using DataTools.Win32Api;

namespace DataTools.Win32Api
{
    /// <summary>
    /// SYSTEMTIME native date-time structure.
    /// </summary>
    /// <remarks></remarks>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct SYSTEMTIME
    {
        public ushort wYear;
        public ushort wMonth;
        public ushort wDayOfWeek;
        public ushort wDay;
        public ushort wHour;
        public ushort wMinute;
        public ushort wSecond;
        public ushort wMilliseconds;

        public static explicit operator DateTime(SYSTEMTIME operand)
        {
            return new DateTime(operand.wYear, operand.wMonth, operand.wDay, operand.wHour, operand.wMinute, operand.wSecond, operand.wMilliseconds);
        }

        public static explicit operator SYSTEMTIME(DateTime operand)
        {
            return new SYSTEMTIME(operand);
        }

        /// <summary>
        /// Initialize a new SYSTEMTIME structure with the specified DateTime object.
        /// </summary>
        /// <param name="t">A DateTime object.</param>
        /// <remarks></remarks>
        public SYSTEMTIME(DateTime t)
        {
            wYear = (ushort)t.Year;
            wMonth = (ushort)t.Month;
            wDayOfWeek = (ushort)t.DayOfWeek;
            wDay = (ushort)t.Day;
            wHour = (ushort)t.Hour;
            wMinute = (ushort)t.Minute;
            wSecond = (ushort)t.Second;
            wMilliseconds = (ushort)t.Millisecond;
        }
    }
}
