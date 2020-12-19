// ************************************************* ''
// DataTools C# Native Utility Library For Windows - Interop
//
// Module: NetInfoApi
//         Windows Networking Api
//
//         Enums are documented in part from the API documentation at MSDN.
//
// Copyright (C) 2011-2020 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the MIT License   
// ************************************************* ''


using System;
using System.Runtime.InteropServices;

using DataTools.Text;
using DataTools.Win32;

namespace DataTools.Win32.Network
{
    /// <summary>
    /// User privilege levels.
    /// </summary>
    /// <remarks></remarks>
    public enum UserPrivilegeLevel
    {

        /// <summary>
        /// Guest user
        /// </summary>
        Guest,

        /// <summary>
        /// Normal user
        /// </summary>
        User,

        /// <summary>
        /// Administrator
        /// </summary>
        Administrator
    }
}
