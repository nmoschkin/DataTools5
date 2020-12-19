// ************************************************* ''
// DataTools C# Native Utility Library For Windows - Interop
//
// Module: NativeError
//         GetLastError and related.
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
using DataTools.Memory;
using DataTools.Win32.Memory;

namespace DataTools.Win32
{
    public static class NativeErrorMethods
    {

        /// <summary>
        /// Format a given system error, or the last system error by default.
        /// </summary>
        /// <param name="syserror">Format code to pass. GetLastError is used as by default.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string FormatLastError(uint syserror = 0U)
        {
            uint err = syserror == 0L ? (uint)User32.GetLastError() : syserror;
            var mm = new SafePtr();
            string s;
            mm.Length = 1026L;
            mm.ZeroMemory();
            User32.FormatMessage(0x1000, IntPtr.Zero, err, 0U, mm.handle, 512U, IntPtr.Zero);
            s = "Error &H" + err.ToString("X8") + ": " + mm.ToString();
            mm.Dispose();
            return s;
        }
    }
}
