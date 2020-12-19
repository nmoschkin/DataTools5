// ************************************************* ''
// DataTools C# Native Utility Library For Windows - Interop
//
// Module: IfDefApi
//         The almighty network interface native API.
//         Some enum documentation comes from the MSDN.
//
// (and an exercise in creative problem solving and data-structure marshaling.)
//
// Copyright (C) 2011-2020 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the Microsoft Public License   
// ************************************************* ''


using System;
using System.ComponentModel;
using System.Net;
using System.Runtime.InteropServices;
using DataTools.Memory;
using DataTools.Win32;

namespace DataTools.Win32.Network
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public struct IP_ADAPTER_DNS_SUFFIX
    {
        public LPIP_ADAPTER_DNS_SUFFIX Next;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string String;
    }
}
