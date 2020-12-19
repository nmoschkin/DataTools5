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
    /// Windows API USER_INFO_1 structure.  A moderately verbose report
    /// version of users on a system.
    /// </summary>
    /// <remarks></remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct UserInfo1
    {

        /// <summary>
        /// Username
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Name;

        /// <summary>
        /// Password (security permitting)
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Password;

        /// <summary>
        /// Password age
        /// </summary>
        public FriendlySeconds PasswordAge;

        /// <summary>
        /// User privilege
        /// </summary>
        public UserPrivilegeLevel Priv;

        /// <summary>
        /// The full path to the user's home directory
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string HomeDir;

        /// <summary>
        /// Comments
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Commant;

        /// <summary>
        /// Flags
        /// </summary>
        public int Flags;

        /// <summary>
        /// Script path
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string ScriptPath;

        public override string ToString()
        {
            return Name;
        }
    }
}
