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
    public struct BLUETOOTH_RADIO_INFO
    {
        public ulong dwSize; // Size, In bytes, Of this entire data Structure
        public BLUETOOTH_ADDRESS address; // Address Of the local radio
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Bluetooth.BLUETOOTH_MAX_NAME_SIZE)]
        public string szName;
        public uint ulClassofDevice; // Class of device for the local radio
        public ushort lmpSubVersion;                    // lmpSubversion, manufacturer specifc.
        public BTH_MFG_INFO manufacturer;                        // Manufacturer Of the radio, BTH_MFG_Xxx value.  For the most up to date
        // list, goto the Bluetooth specification website And get the Bluetooth
        // assigned numbers document.
    }
}
