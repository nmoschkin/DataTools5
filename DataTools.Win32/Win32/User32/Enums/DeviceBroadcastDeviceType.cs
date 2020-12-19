// ************************************************* ''
// DataTools C# Native Utility Library For Windows - Interop
//
// Module: Native
//         Myriad Windows API Declares
//
// Started in 2000 on Windows 98/ME (and then later 2000).
//
// Still kicking in 2014 on Windows 8.1!
// A whole bunch of pInvoke/Const/Declare/Struct and associated utility functions that have been collected over the years.

// Some enum documentation copied from the MSDN (and in some cases, updated).
// 
// Copyright (C) 2011-2020 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the MIT License   
// ************************************************* ''



using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;

using DataTools.Win32;

namespace DataTools.Win32
{
    public enum DeviceBroadcastDeviceType
    {
        /// <summary>
        /// Class of devices. This structure is a DEV_BROADCAST_DEVICEINTERFACE structure.
        /// </summary>
        [Description("Class of devices. This structure is a DEV_BROADCAST_DEVICEINTERFACE structure.")]
        DBT_DEVTYP_DEVICEINTERFACE = 0x5,

        /// <summary>
        /// File system handle. This structure is a DEV_BROADCAST_HANDLE structure.
        /// </summary>
        [Description("File system handle. This structure is a DEV_BROADCAST_HANDLE structure.")]
        DBT_DEVTYP_HANDLE = 0x6,

        /// <summary>
        /// OEM- or IHV-defined device type. This structure is a DEV_BROADCAST_OEM structure.
        /// </summary>
        [Description("OEM- or IHV-defined device type. This structure is a DEV_BROADCAST_OEM structure.")]
        DBT_DEVTYP_OEM = 0x0,

        /// <summary>
        /// Port device (serial or parallel). This structure is a DEV_BROADCAST_PORT structure.
        /// </summary>
        [Description("Port device (serial or parallel). This structure is a DEV_BROADCAST_PORT structure.")]
        DBT_DEVTYP_PORT = 0x3,

        /// <summary>
        /// Logical volume. This structure is a DEV_BROADCAST_VOLUME structure.
        /// </summary>
        [Description("Logical volume. This structure is a DEV_BROADCAST_VOLUME structure.")]
        DBT_DEVTYP_VOLUME = 0x2
    }
}
