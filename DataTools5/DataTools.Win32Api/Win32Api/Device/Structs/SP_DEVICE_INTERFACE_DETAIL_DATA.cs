// ************************************************* ''
// DataTools C# Native Utility Library For Windows - Interop
//
// Module: DevProp
//         Native Device Properites.
// 
// Copyright (C) 2011-2020 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the Microsoft Public License   
// ************************************************* ''

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using DataTools.Text;

namespace DataTools.Win32Api
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct SP_DEVICE_INTERFACE_DETAIL_DATA
    {
        public uint cbSize;

        // this is a array of unknown size.
        public char DevicePath;
    }
}
