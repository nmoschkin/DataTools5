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
    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    /// <summary>
    /// IP Adapter enumeration function flags.
    /// </summary>
    /// <remarks></remarks>
    [Flags]
    public enum GAA_FLAGS
    {

        /// <summary>
        /// Do not return unicast addresses.
        /// </summary>
        /// <remarks></remarks>
        GAA_FLAG_SKIP_UNICAST = 0x1,

        /// <summary>
        /// Do not return IPv6 anycast addresses.
        /// </summary>
        /// <remarks></remarks>
        GAA_FLAG_SKIP_ANYCAST = 0x2,

        /// <summary>
        /// Do not return multicast addresses.
        /// </summary>
        /// <remarks></remarks>
        GAA_FLAG_SKIP_MULTICAST = 0x4,

        /// <summary>
        /// Do not return addresses of DNS servers.
        /// </summary>
        /// <remarks></remarks>
        GAA_FLAG_SKIP_DNS_SERVER = 0x8,

        /// <summary>
        /// Return a list of IP address prefixes on this adapter. When this flag is set, IP address prefixes are returned for both IPv6 and IPv4 addresses.
        /// </summary>
        /// <remarks></remarks>
        GAA_FLAG_INCLUDE_PREFIX = 0x10,

        /// <summary>
        /// Do not return the adapter friendly name.
        /// This flag is supported on Windows XP with SP1 and later.
        /// </summary>
        /// <remarks></remarks>
        GAA_FLAG_SKIP_FRIENDLY_NAME = 0x20,

        /// <summary>
        /// Return addresses of Windows Internet Name Service (WINS) servers.
        /// </summary>
        /// <remarks></remarks>
        GAA_FLAG_INCLUDE_WINS_INFO = 0x40,

        /// <summary>
        /// Return the addresses of default gateways.
        /// This flag is supported on Windows Vista and later.
        /// </summary>
        /// <remarks></remarks>
        GAA_FLAG_INCLUDE_GATEWAYS = 0x80,

        /// <summary>
        /// Return addresses for all NDIS interfaces.
        /// This flag is supported on Windows Vista and later.
        /// </summary>
        /// <remarks></remarks>
        GAA_FLAG_INCLUDE_ALL_INTERFACES = 0x100,

        /// <summary>
        /// Return addresses in all routing compartments.
        /// </summary>
        /// <remarks></remarks>
        GAA_FLAG_INCLUDE_ALL_COMPARTMENTS = 0x200,

        /// <summary>
        /// Return the adapter addresses sorted in tunnel binding order. This flag is supported on Windows Vista and later.
        /// </summary>
        /// <remarks></remarks>
        GAA_FLAG_INCLUDE_TUNNEL_BINDINGORDER = 0x400
    }
}
