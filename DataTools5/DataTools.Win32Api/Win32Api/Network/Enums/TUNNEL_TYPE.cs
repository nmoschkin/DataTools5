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
    /// <summary>
    /// Interface tunnel type.
    /// </summary>
    /// <remarks></remarks>
    public enum TUNNEL_TYPE
    {

        /// <summary>
        /// None
        /// </summary>
        None = 0,

        /// <summary>
        /// Other
        /// </summary>
        Other = 1,

        /// <summary>
        /// Direct
        /// </summary>
        Direct = 2,

        /// <summary>
        /// Ipv6 to Ipv4 tunnel
        /// </summary>
        IPv6ToIPv4 = 11,

        /// <summary>
        /// ISATAP tunnel
        /// </summary>
        ISATAP = 13,

        /// <summary>
        /// Teredo Ipv6 tunnel
        /// </summary>
        Teredo = 14,

        /// <summary>
        /// IPHTTPS tunnel
        /// </summary>
        IPHTTPS = 15
    }
}
