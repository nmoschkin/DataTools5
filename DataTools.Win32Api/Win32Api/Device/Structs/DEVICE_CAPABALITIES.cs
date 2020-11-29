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
    internal struct DEVICE_CAPABALITIES
    {
        public ushort Size;
        public ushort Version;
        public DeviceCapabilities Capabilities;
        public int Address;
        public int UINumber;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I4, SizeConst = 7)]
        public DEVICE_POWER_STATE[] DeviceState;
        public SYSTEM_POWER_STATE SystemWake;
        public DEVICE_POWER_STATE DeviceWake;
        public int D1Latency;
        public int D2Latency;
        public int D3Latency;
    }
}
