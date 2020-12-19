// ************************************************* ''
// DataTools C# Native Utility Library For Windows - Interop
//
// Module: DevProp
//         Native Device Properites.
// 
// Copyright (C) 2011-2020 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the Microsoft Public License   
// ************************************************* ''

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using DataTools.Text;

namespace DataTools.Win32
{
    /// <summary>
    /// Enumeration flags for the SetupDiGetClassDevs
    /// </summary>
    /// <remarks></remarks>
    [Flags]
    public enum ClassDevFlags : int
    {

        /// <summary>
        /// Return only the device that is associated with the system default device interface, if one is set, for the specified device interface classes.
        /// </summary>
        /// <remarks></remarks>
        Default = DevProp.DIGCF_DEFAULT,

        /// <summary>
        /// Return only devices that are currently present in a system.
        /// </summary>
        /// <remarks></remarks>
        Present = DevProp.DIGCF_PRESENT,

        /// <summary>
        /// Return a list of installed devices for all device setup classes or all device interface classes.
        /// </summary>
        /// <remarks></remarks>
        AllClasses = DevProp.DIGCF_ALLCLASSES,

        /// <summary>
        /// Return only devices that are a part of the current hardware profile.
        /// </summary>
        /// <remarks></remarks>
        Profile = DevProp.DIGCF_PROFILE,

        /// <summary>
        /// Return devices that support device interfaces for the specified device interface classes.
        /// This flag must be set in the Flags parameter if the Enumerator parameter specifies a device instance ID.
        /// </summary>
        /// <remarks></remarks>
        DeviceInterface = DevProp.DIGCF_DEVICEINTERFACE
    }
}
