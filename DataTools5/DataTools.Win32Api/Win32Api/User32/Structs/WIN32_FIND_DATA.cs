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
    /// Native WIN32_FIND_DATA structure used in FindFirstFile/FindFirstFileEx and FindNextFile
    /// </summary>
    /// <remarks></remarks>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct WIN32_FIND_DATA
    {
        public FileAttributes dwFileAttributes;
        [MarshalAs(UnmanagedType.Struct)]
        public FILETIME ftCreationTime;
        [MarshalAs(UnmanagedType.Struct)]
        public FILETIME ftLastAccessTime;
        [MarshalAs(UnmanagedType.Struct)]
        public FILETIME ftLastWriteTime;
        public int nFileSizeHigh;
        public uint nFileSizeLow;
        public int dwReserved1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 260)]
        private char[] _cFilename;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 14)]
        private char[] _cAlternate;

        public string cFilename
        {
            get
            {
                return new string(_cFilename).Trim('\0');
            }

            set
            {
                if (value.Length > 260)
                    throw new ArgumentException();

                var mm = new SafePtr();

                mm.Alloc(260 * 2);
                mm.SetString(0L, value, false);
                Marshal.Copy(mm.handle, _cFilename, 0, (int)mm.Size);
                mm.Free();
            }
        }

        public string cAlternate
        {
            get
            {
                return new string(_cAlternate).Trim('\0');
            }

            set
            {
                SafePtr mm = new SafePtr();

                if (value.Length > 14)
                    throw new ArgumentException();

                mm.Alloc(14 * 2);
                mm.SetString(0L, value, false);

                Marshal.Copy(mm, _cAlternate, 0, (int)mm.Size);
                mm.Free();
            }
        }

        public long nFileSize
        {
            get
            {
                long nFileSizeRet = default;
                nFileSizeRet = (long)nFileSizeHigh << 32 | nFileSizeLow;
                return nFileSizeRet;
            }

            set
            {
                nFileSizeHigh = (int)(value >> 32 & 0xFFFFFFFFL);
                nFileSizeLow = (uint)(value & 0xFFFFFFFFL);
            }
        }
    }
}
