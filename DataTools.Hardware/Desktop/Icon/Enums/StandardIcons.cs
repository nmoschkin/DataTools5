// ************************************************* ''
// DataTools C# Native Utility Library For Windows - Interop
//
// Module: Icon File.
//         Icon image file format structure classes
//         Capable of using Windows Vista and greater .PNG icon images
//         Can create a complete icon file from scratch using images you add
//
// Icons are an old old format.  They have been adapted for modern use,
// and the reason that they endure is because of the ability to succintly
// store multiple image sizes in multiple formats, in a single file.
//
// But, because the 32-bit bitmap standard came around slightly afterward,
// certain acrobatic programming translations had to be made to get one from
// the other, and back again.
//
// Remember, back in the day, icon painting and design software was its own thing.
//
// Copyright (C) 2011-2017 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the MIT License   
// ************************************************* ''

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;

using DataTools.Win32;
using DataTools.Shell.Native;

namespace DataTools.Desktop
{
    /// <summary>
    /// A short enumeration of standard icon type sizes coded for direct insertion into an ICONDIRENTRY structure.
    /// </summary>
    /// <remarks></remarks>
    public enum StandardIcons : ushort
    {
        Icon256 = 0,
        Icon128 = 0x8080,
        Icon64 = 0x4040,
        Icon48 = 0x3030,
        Icon32 = 0x2020,
        Icon16 = 0x1010
    }
}
