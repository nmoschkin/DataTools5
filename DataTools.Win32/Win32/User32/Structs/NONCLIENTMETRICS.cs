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
using DataTools.Win32;

namespace DataTools.Win32
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct NONCLIENTMETRICS
    {
        public int cbSize;
        public int iBorderWidth;
        public int iScrollWidth;
        public int iScrollHeight;
        public int iCaptionWidth;
        public int iCaptionHeight;
        [MarshalAs(UnmanagedType.Struct)]
        public LOGFONT lfCaptionFont;
        public int iSMCaptionWidth;
        public int iSMCaptionHeight;
        [MarshalAs(UnmanagedType.Struct)]
        public LOGFONT lfSMCaptionFont;
        public int iMenuWidth;
        public int iMenuHeight;
        [MarshalAs(UnmanagedType.Struct)]
        public LOGFONT lfMenuFont;
        [MarshalAs(UnmanagedType.Struct)]
        public LOGFONT lfStatusFont;
        [MarshalAs(UnmanagedType.Struct)]
        public LOGFONT lfMessageFont;
    }
}
