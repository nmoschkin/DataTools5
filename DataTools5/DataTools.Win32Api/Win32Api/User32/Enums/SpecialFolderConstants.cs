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
    public enum SpecialFolderConstants
    {
        Desktop = 0x0,
        Internet = 0x1,
        Programs = 0x2,
        Controls = 0x3,
        Printers = 0x4,
        Personal = 0x5,
        Favorites = 0x6,
        StartUp = 0x7,
        Recent = 0x8,
        SendTo = 0x9,
        BitBucket = 0xA,
        StartMenu = 0xB,
        DesktopDirectory = 0x10,
        Drives = 0x11,
        Network = 0x12,
        Nethood = 0x13,
        Fonts = 0x14,
        Templates = 0x15,
        CommonStartMenu = 0x16,
        CommonPrograms = 0x17,
        CommonStartup = 0x18,
        CommonDesktopDirectory = 0x19,
        AppData = 0x1A,
        PrintHood = 0x1B,
        AltStartup = 0x1D,                          // DBCS
        CommonAltStartup = 0x1E,                   // DBCS
        CommonFavorites = 0x1F,
        InternetCache = 0x20,
        Cookies = 0x21,
        History = 0x22
    }
}
