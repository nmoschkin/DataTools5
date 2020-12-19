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
// Licensed Under the Microsoft Public License   
// ************************************************* ''



using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using DataTools.Memory;
using DataTools.Win32;

namespace DataTools.Win32
{
    public enum DeviceBroadcastType
    {

        /// <summary>
        /// A request to change the current configuration (dock or undock) has been canceled.
        /// </summary>
        [Description("A request to change the current configuration (dock or undock) has been canceled.")]
        DBT_CONFIGCHANGECANCELED = 0x19,

        /// <summary>
        /// The current configuration has changed, due to a dock or undock.
        /// </summary>
        [Description("The current configuration has changed, due to a dock or undock.")]
        DBT_CONFIGCHANGED = 0x18,

        /// <summary>
        /// A custom event has occurred.
        /// </summary>
        [Description("A custom event has occurred.")]
        DBT_CUSTOMEVENT = 0x8006,

        /// <summary>
        /// A device or piece of media has been inserted and is now available.
        /// </summary>
        [Description("A device or piece of media has been inserted and is now available.")]
        DBT_DEVICEARRIVAL = 0x8000,

        /// <summary>
        /// Permission is requested to remove a device or piece of media. Any application can deny this request and cancel the removal.
        /// </summary>
        [Description("Permission is requested to remove a device or piece of media. Any application can deny this request and cancel the removal.")]
        DBT_DEVICEQUERYREMOVE = 0x8001,

        /// <summary>
        /// A request to remove a device or piece of media has been canceled.
        /// </summary>
        [Description("A request to remove a device or piece of media has been canceled.")]
        DBT_DEVICEQUERYREMOVEFAILED = 0x8002,

        /// <summary>
        /// A device or piece of media has been removed.
        /// </summary>
        [Description("A device or piece of media has been removed.")]
        DBT_DEVICEREMOVECOMPLETE = 0x8004,

        /// <summary>
        /// A device or piece of media is about to be removed. Cannot be denied.
        /// </summary>
        [Description("A device or piece of media is about to be removed. Cannot be denied.")]
        DBT_DEVICEREMOVEPENDING = 0x8003,

        /// <summary>
        /// A device-specific event has occurred.
        /// </summary>
        [Description("A device-specific event has occurred.")]
        DBT_DEVICETYPESPECIFIC = 0x8005,

        /// <summary>
        /// A device has been added to or removed from the system.
        /// </summary>
        [Description("A device has been added to or removed from the system.")]
        DBT_DEVNODES_CHANGED = 0x7,

        /// <summary>
        /// Permission is requested to change the current configuration (dock or undock).
        /// </summary>
        [Description("Permission is requested to change the current configuration (dock or undock).")]
        DBT_QUERYCHANGECONFIG = 0x17,

        /// <summary>
        /// The meaning of this message is user-defined.
        /// </summary>
        [Description("The meaning of this message is user-defined.")]
        DBT_USERDEFINED = 0xFFFF
    }
}
