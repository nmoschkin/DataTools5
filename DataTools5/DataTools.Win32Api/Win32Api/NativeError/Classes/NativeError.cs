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

namespace DataTools.Win32Api
{
    public sealed class NativeError
    {

        /// <summary>
        /// Returns the current last native error.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public static int Error
        {
            get
            {
                return User32.GetLastError();
            }
        }

        /// <summary>
        /// returns the current last native error formatted message.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string Message
        {
            get
            {
                return NativeErrorMethods.FormatLastError();
            }
        }
    }
}
