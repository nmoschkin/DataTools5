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
    /// System device removal policy.
    /// </summary>
    public enum DeviceRemovalPolicy
    {

        /// <summary>
        /// The device is not expected to be removed.
        /// </summary>
        /// <remarks></remarks>
        ExpectNoRemoval = 1,

        /// <summary>
        /// The device can be expected to be removed in an orderly fashion.
        /// </summary>
        /// <remarks></remarks>
        ExpectOrderlyRemoval = 2,

        /// <summary>
        /// The device can be expected to be removed without any preparation for removal.
        /// </summary>
        /// <remarks></remarks>
        ExpectSurpriseRemoval = 3
    }
}
