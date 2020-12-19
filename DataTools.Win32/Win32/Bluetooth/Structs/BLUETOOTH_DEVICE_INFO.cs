// ************************************************* ''
// DataTools C# Native Utility Library For Windows 
//
// Module: Bluetooth API
//         Complete Translation of
//         BluetoothAPI.h
// 
// Copyright (C) 2011-2020 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the MIT License   
// ************************************************* ''

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using DataTools.Text;

namespace DataTools.Win32
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct BLUETOOTH_DEVICE_INFO
    {
        public long dwSize;
        public BLUETOOTH_ADDRESS Address;
        public uint ulClassofDevice;
        [MarshalAs(UnmanagedType.Bool)]
        public bool fConnected;
        [MarshalAs(UnmanagedType.Bool)]
        public bool fRemembered;
        [MarshalAs(UnmanagedType.Bool)]
        public bool fAuthenticated;
        [MarshalAs(UnmanagedType.Struct)]
        public SYSTEMTIME stLastSeen;
        [MarshalAs(UnmanagedType.Struct)]
        public SYSTEMTIME stLastUsed;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Bluetooth.BLUETOOTH_MAX_NAME_SIZE)]
        public string szName;
    }
}
