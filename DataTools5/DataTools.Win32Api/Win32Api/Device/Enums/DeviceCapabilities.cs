// ************************************************* ''
// DataTools C# Native Utility Library For Windows - Interop
//
// Module: Miscellaneous enums to support devices.
// 
// Copyright (C) 2011-2020 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the Microsoft Public License   
// ************************************************* ''


using System;
using System.ComponentModel;
using DataTools.Text;
using DataTools.Win32Api;

namespace DataTools.Win32Api
{
    /// <summary>
    /// Specifies the device capabilities.
    /// </summary>
    /// <remarks></remarks>
    [Flags]
    public enum DeviceCapabilities : uint
    {
        None = 0U,
        DeviceD1 = 0x1U,
        DeviceD2 = 0x2U,
        LockSupported = 0x4U,
        EjectSupported = 0x8U,
        Removable = 0x10U,
        DockDevice = 0x20U,
        UniqueID = 0x40U,
        SilentInstall = 0x80U,
        RawDeviceOK = 0x100U,
        SurpriseRemovalOK = 0x200U,
        WakeFromD0 = 0x400U,
        WakeFromD1 = 0x800U,
        WakeFromD2 = 0x1000U,
        WakeFromD3 = 0x2000U,
        HardwareDisabled = 0x4000U,
        NonDynamic = 0x8000U,
        WarmEjectSupported = 0x10000U,
        NoDisplayInUI = 0x20000U,
        Reserved1 = 0x40000U,
        Reserved = 0xFFF80000U
    }
}
