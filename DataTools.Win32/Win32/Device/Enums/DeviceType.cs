// ************************************************* ''
// DataTools C# Native Utility Library For Windows - Interop
//
// Module: Miscellaneous enums to support devices.
// 
// Copyright (C) 2011-2020 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the MIT License   
// ************************************************* ''


using System;
using System.ComponentModel;
using DataTools.Text;
using DataTools.Win32;

namespace DataTools.Win32
{
    /// <summary>
    /// Specifies the type of the device.
    /// </summary>
    /// <remarks></remarks>
    public enum DeviceType
    {
        Disk,
        Network,
        Usb,
        Volume
    }
}
