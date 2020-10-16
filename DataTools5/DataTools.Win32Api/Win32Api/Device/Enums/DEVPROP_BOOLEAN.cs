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
    internal enum DEVPROP_BOOLEAN : byte
    {
        DEVPROP_FALSE = 0,
        DEVPROP_TRUE = 0xFF
    }
}
