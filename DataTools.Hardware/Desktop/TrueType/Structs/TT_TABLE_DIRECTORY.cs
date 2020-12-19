// ************************************************* ''
// DataTools C# Native Utility Library For Windows - Interop
//
// Module: TrueType.
//         Code to read TrueType font file information
//         Adapted from the CodeProject article: http://www.codeproject.com/Articles/2293/Retrieving-font-name-from-TTF-file?msg=4714219#xx4714219xx
//
// 
// Copyright (C) 2011-2020 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the MIT License   
// ************************************************* ''

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

using DataTools.Win32;

namespace DataTools.Desktop
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct TT_TABLE_DIRECTORY
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] szTag;
        public uint uCheckSum;
        public uint uOffset;
        public uint uLength;

        public string Tag
        {
            get
            {
                return System.Text.Encoding.ASCII.GetString(szTag);
            }
        }
    }
}
