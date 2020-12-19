// ************************************************* ''
// DataTools C# Native Utility Library For Windows - Interop
//
// Module: UsbHid
//         HID-related structures, enums and functions.
//
// Copyright (C) 2011-2020 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the Microsoft Public License   
// ************************************************* ''

using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace DataTools.Win32
{
    public enum hid_usage_type
    {

        /// <summary>
        /// Application collection
        /// </summary>
        /// <remarks></remarks>
        CA,

        /// <summary>
        /// Logical collection
        /// </summary>
        /// <remarks></remarks>
        CL,

        /// <summary>
        /// Physical collection
        /// </summary>
        /// <remarks></remarks>
        CP,

        /// <summary>
        /// Dynamic Flag
        /// </summary>
        /// <remarks></remarks>
        DF,

        /// <summary>
        /// Dynamic Value
        /// </summary>
        /// <remarks></remarks>
        DV,

        /// <summary>
        /// Static Flag
        /// </summary>
        /// <remarks></remarks>
        SF,

        /// <summary>
        /// Static Value
        /// </summary>
        /// <remarks></remarks>
        SV
    }
}
