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
// Licensed Under the Microsoft Public License   
// ************************************************* ''

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using DataTools.Memory;
using DataTools.Win32Api;

namespace DataTools.Desktop
{
    public enum FontCharSet : byte
    {
        ANSI = 0,
        Default = 1,
        Symbol = 2,
        ShiftJIS = 128,
        Hangeul = 129,
        Hangul = 129,
        GB2312 = 134,
        ChineseBIG5 = 136,
        OEM = 255,
        Johab = 130,
        Hebrew = 177,
        Arabic = 178,
        Greek = 161,
        Turkish = 162,
        Vietnamese = 163,
        Thai = 222,
        EasternEurope = 238,
        Russian = 204,
        Mac = 77,
        Baltic = 186
    }
}
