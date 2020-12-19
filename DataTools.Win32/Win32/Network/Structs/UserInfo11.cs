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

using DataTools.Text;
using DataTools.Win32;
using DataTools.Win32.Memory;

namespace DataTools.Win32.Network
{
    /// <summary>
    /// Windows API USER_INFO_11 structure.  A very verbose
    /// report version of users on a system.
    /// </summary>
    /// <remarks></remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct UserInfo11
    {
        // LPWSTR usri11_name;
        // LPWSTR usri11_comment;
        // LPWSTR usri11_usr_comment;
        // LPWSTR usri11_full_name;

        /// <summary>
        /// Username
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Name;

        /// <summary>
        /// Comments
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Comment;

        /// <summary>
        /// User comments
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string UserComment;

        /// <summary>
        /// User's full name
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string FullName;

        /// <summary>
        /// User privileges
        /// </summary>
        public UserPrivilegeLevel Priv;

        /// <summary>
        /// Server access authorization flags
        /// </summary>
        public AuthFlags AuthFlags;

        /// <summary>
        /// Password age
        /// </summary>
        public FriendlySeconds PasswordAge;

        /// <summary>
        /// Full path to the user's home directory
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string HomeDir;

        /// <summary>
        /// Params
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Parms;

        /// <summary>
        /// Last logon time/date
        /// </summary>
        public FriendlyUnixTime LastLogon;

        /// <summary>
        /// Last logout time/date
        /// </summary>
        public FriendlyUnixTime LastLogout;

        /// <summary>
        /// Number of invalid password attempts
        /// </summary>
        public int BadPwCount;

        /// <summary>
        /// Total number of logons
        /// </summary>
        public int NumLogons;

        /// <summary>
        /// Logon server
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string LogonServer;

        /// <summary>
        /// Country code
        /// </summary>
        public int CountryCode;

        /// <summary>
        /// Workstations
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Workstations;

        /// <summary>
        /// Maximum allowed storage for user
        /// </summary>
        public int MaxStorage;

        /// <summary>
        /// Units per week
        /// </summary>
        public int UnitsPerWeek;

        /// <summary>
        /// Logon hours
        /// </summary>
        public MemPtr LogonHours;

        /// <summary>
        /// Code page
        /// </summary>
        public int CodePage;

        public override string ToString()
        {
            return FullName;
        }
    }
}
