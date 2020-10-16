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
using DataTools.Win32Api;

namespace DataTools.Win32Api.Network
{
    [StructLayout(LayoutKind.Sequential)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public struct ADAPTER_UNICAST_ADDRESS
    {
        public IP_ADAPTER_HEADER_UNION Header;
        public LPADAPTER_UNICAST_ADDRESS Next;
        public SOCKET_ADDRESS Address;
        public IpPrefixOrigin PrefixOrigin;
        public IpSuffixOrigin SuffixOrigin;
        public uint ValidLifetime;
        public uint PreferredLifetime;
        public uint LeaseLifetime;

        public override string ToString()
        {
            string ToStringRet = default;
            ToStringRet = Address.ToString();
            return ToStringRet;
        }
    }
}
