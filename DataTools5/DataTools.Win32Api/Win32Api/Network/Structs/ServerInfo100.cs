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
// Licensed Under the Microsoft Public License   
// ************************************************* ''


using System;
using System.Runtime.InteropServices;
using DataTools.Memory;
using DataTools.Text;
using DataTools.Win32Api;

namespace DataTools.Win32Api.Network
{
    /// <summary>
    /// Windows API SERVER_INFO_100 structure.  Name and platform Id for a computer.
    /// </summary>
    /// <remarks></remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct ServerInfo100
    {

        /// <summary>
        /// Platform Id
        /// </summary>
        public int PlatformId;

        /// <summary>
        /// Name
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Name;

        /// <summary>
        /// Returns <see cref="Name"/>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name;
        }
    }
}
