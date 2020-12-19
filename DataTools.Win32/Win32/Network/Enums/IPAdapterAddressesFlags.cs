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
// Licensed Under the MIT License   
// ************************************************* ''


using System;
using System.ComponentModel;
using System.Net;
using System.Runtime.InteropServices;

using DataTools.Win32;

namespace DataTools.Win32.Network
{
    
    
    [Flags]
    public enum IPAdapterAddressesFlags
    {
        DDNS = 0x1,
        RegisterAdapterSuffix = 0x2,
        DHCP = 0x4,
        ReceiveOnly = 0x8,
        NoMulticast = 0x10,
        IPv6OtherStatfulConfig = 0x20,
        NetBiosOverTCPIP = 0x40,
        IPv4Enabled = 0x80,
        IPv6Enabled = 0x100,
        IPv6ManageAddressConfig = 0x200
    }
}
