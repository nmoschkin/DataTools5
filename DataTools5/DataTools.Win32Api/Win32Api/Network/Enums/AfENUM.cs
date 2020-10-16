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
    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    /// <summary>
    /// Network adapter enumerator-allowed address families.
    /// </summary>
    /// <remarks></remarks>
    public enum AfENUM : uint
    {

        /// <summary>
        /// Return both IPv4 and IPv6 addresses associated with adapters with IPv4 or IPv6 enabled.
        /// </summary>
        /// <remarks></remarks>
        AfUnspecified = 0U,

        /// <summary>
        /// Return only IPv4 addresses associated with adapters with IPv4 enabled.
        /// </summary>
        /// <remarks></remarks>
        AfInet = 2U,

        /// <summary>
        /// Return only IPv6 addresses associated with adapters with IPv6 enabled.
        /// </summary>
        /// <remarks></remarks>
        AfInet6 = 23U
    }
}
