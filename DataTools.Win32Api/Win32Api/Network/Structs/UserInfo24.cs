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
    /// Windows 8+ Identity Structure for Windows Logon
    /// </summary>
    /// <remarks></remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct UserInfo24
    {

        /// <summary>
        /// Is an internet identity / Microsoft account
        /// </summary>
        [MarshalAs(UnmanagedType.Bool)]
        public bool InternetIdentity;

        /// <summary>
        /// Flags
        /// </summary>
        public int Flags;

        /// <summary>
        /// Internet provider name
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string InternetProviderName;

        /// <summary>
        /// Internet principle username
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string InternetPrincipalName;
        public IntPtr UserSid;

        public override string ToString()
        {
            return InternetPrincipalName;
        }
    }
}
