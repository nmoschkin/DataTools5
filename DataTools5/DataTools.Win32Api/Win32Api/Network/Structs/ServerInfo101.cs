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
    /// Windows API SERVER_INFO_101 structure.  Contains extended, vital information
    /// about a computer on the network.
    /// </summary>
    /// <remarks></remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct ServerInfo101
    {



        /// <summary>
        /// Platform ID
        /// </summary>
        public int PlatformId;


        /// <summary>
        /// Server Name
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Name;

        /// <summary>
        /// Server software version Major number
        /// </summary>
        public int VersionMajor;
        /// <summary>
        /// Server software version Minor number
        /// </summary>
        public int VersionMinor;

        /// <summary>
        /// Server type
        /// </summary>
        public ServerTypes Type;

        /// <summary>
        /// Comments
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Comment;

        /// <summary>
        /// Returns the server name
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name;
        }
    }
}
