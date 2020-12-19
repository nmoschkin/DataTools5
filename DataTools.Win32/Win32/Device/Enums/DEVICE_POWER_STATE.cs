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

namespace DataTools.Win32
{
    internal enum DEVICE_POWER_STATE
    {
        PowerDeviceUnspecified = 0,
        PowerDeviceD0 = 1,
        PowerDeviceD1 = 2,
        PowerDeviceD2 = 3,
        PowerDeviceD3 = 4,
        PowerDeviceMaximum = 5
    }
}
