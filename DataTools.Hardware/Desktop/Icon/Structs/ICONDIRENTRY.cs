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
// Licensed Under the Microsoft Public License   
// ************************************************* ''

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using DataTools.Memory;
using DataTools.Win32;
using DataTools.Shell.Native;

namespace DataTools.Desktop
{
        // ICONDIRENTRY structure
        // Offset#	Size (in bytes)	Purpose
        // 0	1	Specifies image width in pixels. Can be any number between 0 and 255. Value 0 means image width is 256 pixels.
        // 1	1	Specifies image height in pixels. Can be any number between 0 and 255. Value 0 means image height is 256 pixels.
        // 2	1	Specifies number of colors in the color palette. Should be 0 if the image does not use a color palette.
        // 3	1	Reserved. Should be 0.[Notes 2]
        // 4	2	In ICO format: Specifies color planes. Should be 0 or 1.[Notes 3]
        // In CUR format: Specifies the horizontal coordinates of the hotspot in number of pixels from the left.
        // 6	2	In ICO format: Specifies bits per pixel. [Notes 4]
        // In CUR format: Specifies the vertical coordinates of the hotspot in number of pixels from the top.
        // 8	4	Specifies the size of the image's data in bytes
        // 12	4	Specifies the offset of BMP or PNG data from the beginning of the ICO/CUR file
        [StructLayout(LayoutKind.Explicit)]
        public struct ICONDIRENTRY
        {
            [FieldOffset(0)]
            public StandardIcons wIconType;
            [FieldOffset(0)]
            public byte cWidth;
            [FieldOffset(1)]
            public byte cHeight;
            [FieldOffset(2)]
            public byte cColors;
            [FieldOffset(3)]
            public byte cReserved;
            [FieldOffset(4)]
            public short wColorPlanes;
            [FieldOffset(4)]
            public short wHotspotX;
            [FieldOffset(6)]
            public short wBitsPixel;
            [FieldOffset(6)]
            public short wHotspotY;
            [FieldOffset(8)]
            public int dwImageSize;
            [FieldOffset(12)]
            public int dwOffset;
        }
}
