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
    /// Interface operational status.
    /// </summary>
    /// <remarks></remarks>
    public enum IF_OPER_STATUS
    {
        /// <summary>
        /// The network device is up
        /// </summary>
        /// <remarks></remarks>
        IfOperStatusUp = 1,

        /// <summary>
        /// The network device is down
        /// </summary>
        /// <remarks></remarks>
        IfOperStatusDown,

        /// <summary>
        /// The network device is performing a self-test
        /// </summary>
        IfOperStatusTesting,

        /// <summary>
        /// The state of the network device is unknown
        /// </summary>
        IfOperStatusUnknown,

        /// <summary>
        /// The network device is asleep
        /// </summary>
        IfOperStatusDormant,

        /// <summary>
        /// The network device is not present
        /// </summary>
        IfOperStatusNotPresent,

        /// <summary>
        /// Network device lower-layer is down
        /// </summary>
        IfOperStatusLowerLayerDown
    }
}
