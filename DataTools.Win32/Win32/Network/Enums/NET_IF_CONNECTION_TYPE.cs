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
    
    
    /// <summary>
    /// Interface connection type.
    /// </summary>
    /// <remarks></remarks>
    public enum NET_IF_CONNECTION_TYPE
    {

        /// <summary>
        /// Undefined
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// Dedicated connection.  This is a typical connection.
        /// </summary>
        /// <remarks></remarks>
        Dedicated = 1,

        /// <summary>
        /// Passive
        /// </summary>
        Passive = 2,

        /// <summary>
        /// On demand
        /// </summary>
        Demand = 3,

        /// <summary>
        /// Maximum
        /// </summary>
        Maximum = 4
    }
}
