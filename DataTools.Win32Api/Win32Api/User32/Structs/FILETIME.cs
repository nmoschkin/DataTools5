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
    /// System FILETIME structure with automated conversion to CLR DateTime objects and Long integers.
    /// </summary>
    /// <remarks></remarks>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct FILETIME
    {
        public uint lo;
        public int hi;

        /// <summary>
        /// Converts a system DateTime object into a FILETIME structure.
        /// </summary>
        /// <param name="t">System DateTime.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static FILETIME FromDateTime(DateTime t)
        {
            return new FILETIME(t);
        }

        /// <summary>
        /// Converts the current FILETIME structure into a DateTime object.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public DateTime ToDateTime()
        {
            return FileTools.FileToLocal(this);
        }

        public static implicit operator DateTime(FILETIME operand)
        {
            return operand.ToDateTime();
        }

        public static explicit operator FILETIME(DateTime operand)
        {
            return FromDateTime(operand);
        }



        public static implicit operator FILETIME(long operand)
        {
            unchecked
            {
                var ft = new FILETIME();

                ft.lo = (uint)(operand & 0xFFFFFFFFU);
                ft.hi = (int)(((ulong)operand & 0xFFFFFFFF00000000L) >> 32);

                return ft;
            }
        }

        public static implicit operator long(FILETIME operand)
        {
            return FileTools.MakeLong(operand.lo, operand.hi);
        }

        public override string ToString()
        {
            return ToDateTime().ToString();

            /// <summary>
            /// Initialize a new FILETIME structure with the specified DateTime object.
            /// </summary>
            /// <param name="t">System DateTime.</param>
            /// <remarks></remarks>
        }

        public FILETIME(DateTime local)
        {
            FILETIME tf = new FILETIME();

            FileTools.LocalToFileTime(local, ref tf);

            hi = tf.hi;
            lo = tf.lo;
        }

        /// <summary>
        /// Initialize a new FILETIME structure with the specified value.
        /// </summary>
        /// <param name="t">64 bit integer time value.</param>
        /// <remarks></remarks>
        public FILETIME(long t)
        {
            lo = (uint)(t & 0xFFFFFFFFL);
            hi = (int)((ulong)t & (0xFFFFFFFF00000000L >> 32));
        }

        /// <summary>
        /// Initialize a new FILETIME structure with the specified low and high order values.
        /// </summary>
        /// <param name="l">Value of the low-order DWORD</param>
        /// <param name="h">Value of the high-order DWORD</param>
        /// <remarks></remarks>
        public FILETIME(int l, int h)
        {
            lo = (uint)l;
            hi = h;
        }
    }
}
