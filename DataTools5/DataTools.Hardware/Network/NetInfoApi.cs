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
using DataTools.Hardware.Native;
using Microsoft.VisualBasic;

namespace DataTools.Hardware.Network
{

    /// <summary>
    /// Server access authorization flags for the current principal.
    /// </summary>
    /// <remarks></remarks>
    [Flags]
    public enum AuthFlags
    {

        /// <summary>
        /// Printing authorization.
        /// </summary>
        /// <remarks></remarks>
        Print = 1,

        /// <summary>
        /// Communications authorization.
        /// </summary>
        /// <remarks></remarks>
        Communications = 2,

        /// <summary>
        /// File server authorization.
        /// </summary>
        /// <remarks></remarks>
        Server = 4,

        /// <summary>
        /// User account remote access authorization.
        /// </summary>
        /// <remarks></remarks>
        Accounts = 8
    }

    /// <summary>
    /// The join status for a computer on a Workgroup or Domain.
    /// </summary>
    /// <remarks></remarks>
    public enum NetworkJoinStatus
    {
        /// <summary>
        /// Join status is unknown.
        /// </summary>
        /// <remarks></remarks>
        Unknown = 0,

        /// <summary>
        /// Computer is not joined to a network.
        /// </summary>
        /// <remarks></remarks>
        Unjoined,

        /// <summary>
        /// Computer is joined to a Workgroup.
        /// </summary>
        /// <remarks></remarks>
        Workgroup,

        /// <summary>
        /// Computer is joined to a full domain.
        /// </summary>
        /// <remarks></remarks>
        Domain
    }

    /// <summary>
    /// Extended username formats.  In Workgroups, only NameSamCompatible is supported.
    /// </summary>
    /// <remarks></remarks>
    public enum ExtendedNameFormat
    {
        /// <summary>
        /// Unknown/invalid format.
        /// </summary>
        /// <remarks></remarks>
        NameUnknown = 0,

        /// <summary>
        /// Fully-qualified domain name.
        /// </summary>
        /// <remarks></remarks>
        NameFullyQualifiedDN = 1,

        /// <summary>
        /// Windows-networking SAM-compatible MACHINE\User formatted string.  This parameter is valid for Workgroups.
        /// </summary>
        /// <remarks></remarks>
        NameSamCompatible = 2,

        /// <summary>
        /// Display name.
        /// </summary>
        /// <remarks></remarks>
        NameDisplay = 3,

        /// <summary>
        /// The user's unique Id.
        /// </summary>
        /// <remarks></remarks>
        NameUniqueId = 6,

        /// <summary>
        /// The canonical name of the user. This usually means all high-bit Unicode characters have been converted to their lower-order representations
        /// and the string has been normalized to UTF-8 or ASCII as much as possible, including case-hardening.
        /// </summary>
        /// <remarks></remarks>
        NameCanonical = 7,

        /// <summary>
        /// The user principal.
        /// </summary>
        /// <remarks></remarks>
        NameUserPrincipal = 8,

        /// <summary>
        /// Extended formatting canonical name.
        /// </summary>
        /// <remarks></remarks>
        NameCanonicalEx = 9,

        /// <summary>
        /// Service principal.
        /// </summary>
        /// <remarks></remarks>
        NameServicePrincipal = 10,

        /// <summary>
        /// The Dns domain.
        /// </summary>
        /// <remarks></remarks>
        NameDnsDomain = 12
    }

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

    /// <summary>
    /// Windows Networking SID Types.
    /// </summary>
    /// <remarks></remarks>
    [Flags()]
    public enum SidUsage
    {
        /// <summary>
        /// A user SID.
        /// </summary>
        /// <remarks></remarks>
        SidTypeUser = 1,

        /// <summary>
        /// A group SID.
        /// </summary>
        /// <remarks></remarks>
        SidTypeGroup,

        /// <summary>
        /// A domain SID.
        /// </summary>
        /// <remarks></remarks>
        SidTypeDomain,

        /// <summary>
        /// An alias SID.
        /// </summary>
        /// <remarks></remarks>
        SidTypeAlias,

        /// <summary>
        /// A SID for a well-known group.
        /// </summary>
        /// <remarks></remarks>
        SidTypeWellKnownGroup,

        /// <summary>
        /// A SID for a deleted account.
        /// </summary>
        /// <remarks></remarks>
        SidTypeDeletedAccount,

        /// <summary>
        /// A SID that is not valid.
        /// </summary>
        /// <remarks></remarks>
        SidTypeInvalid,

        /// <summary>
        /// A SID of unknown type.
        /// </summary>
        /// <remarks></remarks>
        SidTypeUnknown,

        /// <summary>
        /// A SID for a computer.
        /// </summary>
        /// <remarks></remarks>
        SidTypeComputer,

        /// <summary>
        /// A mandatory integrity label SID.
        /// </summary>
        /// <remarks></remarks>
        SidTypeLabel
    }

    /// <summary>
    /// Windows Networking server/computer types.
    /// </summary>
    /// <remarks></remarks>
    [Flags()]
    public enum ServerTypes
    {
        /// <summary>
        /// A workstation.
        /// </summary>
        /// <remarks></remarks>
        Workstation = 0x1,

        /// <summary>
        /// A server.
        /// </summary>
        /// <remarks></remarks>
        Server = 0x2,

        /// <summary>
        /// A server running with Microsoft SQL Server.
        /// </summary>
        /// <remarks></remarks>
        SqlServer = 0x4,

        /// <summary>
        /// A primary domain controller.
        /// </summary>
        /// <remarks></remarks>
        DomainController = 0x8,

        /// <summary>
        /// A backup domain controller.
        /// </summary>
        /// <remarks></remarks>
        BackupDomainController = 0x10,

        /// <summary>
        /// A server running the Timesource service.
        /// </summary>
        /// <remarks></remarks>
        TimeSource = 0x20,

        /// <summary>
        /// A server running the Apple Filing Protocol (AFP) file service.
        /// </summary>
        /// <remarks></remarks>
        AFPServer = 0x40,

        /// <summary>
        /// A Novell server.
        /// </summary>
        /// <remarks></remarks>
        Novell = 0x80,

        /// <summary>
        /// A LAN Manager 2.x domain member.
        /// </summary>
        /// <remarks></remarks>
        DomainMember = 0x100,

        /// <summary>
        /// A server that shares a print queue.
        /// </summary>
        /// <remarks></remarks>
        PrintQueueServer = 0x200,

        /// <summary>
        /// A server that runs a dial-in service.
        /// </summary>
        /// <remarks></remarks>
        DialInServer = 0x400,

        /// <summary>
        /// A Xenix or Unix server.
        /// </summary>
        /// <remarks></remarks>
        XenixServer = 0x800,

        /// <summary>
        /// A workstation or server.
        /// </summary>
        /// <remarks></remarks>
        WindowsNT = 0x1000,

        /// <summary>
        /// A computer that runs Windows for Workgroups.
        /// </summary>
        /// <remarks></remarks>
        WindowsForWorkgroups = 0x2000,

        /// <summary>
        /// A server that runs the Microsoft File and Print for NetWare service.
        /// </summary>
        /// <remarks></remarks>
        NetwareFilePrintServer = 0x4000,

        /// <summary>
        /// Any server that is not a domain controller.
        /// </summary>
        /// <remarks></remarks>
        NTServer = 0x8000,

        /// <summary>
        /// A computer that can run the browser service.
        /// </summary>
        /// <remarks></remarks>
        PotentialBrowser = 0x10000,

        /// <summary>
        /// A server running a browser service as backup.
        /// </summary>
        /// <remarks></remarks>
        BackupBrowser = 0x20000,

        /// <summary>
        /// A server running the master browser service.
        /// </summary>
        /// <remarks></remarks>
        MasterBrowser = 0x40000,

        /// <summary>
        /// A server running the domain master browser.
        /// </summary>
        /// <remarks></remarks>
        DomainMasterBrowser = 0x80000,

        /// <summary>
        /// A computer that runs OSF.
        /// </summary>
        /// <remarks></remarks>
        OSFServer = 0x100000,

        /// <summary>
        /// A computer that runs VMS.
        /// </summary>
        /// <remarks></remarks>
        VMSServer = 0x200000,

        /// <summary>
        /// A computer that runs Windows.
        /// </summary>
        /// <remarks></remarks>
        Windows = 0x400000,

        /// <summary>
        /// A server that is the root of a DFS tree.
        /// </summary>
        /// <remarks></remarks>
        DFSRootServer = 0x800000,

        /// <summary>
        /// A server cluster available in the domain.
        /// </summary>
        /// <remarks></remarks>
        NTServerCluster = 0x1000000,

        /// <summary>
        /// A server that runs the Terminal Server service.
        /// </summary>
        /// <remarks></remarks>
        TerminalServer = 0x2000000,

        /// <summary>
        /// Cluster virtual servers available in the domain.
        /// </summary>
        /// <remarks></remarks>
        NTVSServerCluster = 0x4000000,

        /// <summary>
        /// A server that runs the DCE Directory and Security Services or equivalent.
        /// </summary>
        /// <remarks></remarks>
        DCEServer = 0x10000000,

        /// <summary>
        /// A server that is returned by an alternate transport.
        /// </summary>
        /// <remarks></remarks>
        AlternateTransport = 0x20000000,

        /// <summary>
        /// A server that is maintained by the browser.
        /// </summary>
        /// <remarks></remarks>
        LocalListOnly = 0x40000000,

        /// <summary>
        /// A primary domain.
        /// </summary>
        /// <remarks></remarks>
        DomainEnum = unchecked((int)0x80000000)
    }


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

    /// <summary>
    /// Windows API NET_DISPLAY_MACHINE structure.
    /// </summary>
    /// <remarks></remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct NetDisplayMachine
    {


        /// <summary>
        /// Server name
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Name;

        /// <summary>
        /// Comments
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Comment;

        /// <summary>
        /// Flags
        /// </summary>
        public int Flags;

        /// <summary>
        /// User ID
        /// </summary>
        public int UserId;

        /// <summary>
        /// Next Index
        /// </summary>
        public int NextIndex;

        /// <summary>
        /// Returns Name
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name;
        }
    }

    /// <summary>
    /// Windows API LOCALGROUP_INFO_1 structure.  Basic local group information.
    /// </summary>
    /// <remarks></remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct LocalGroupInfo1
    {

        /// <summary>
        /// Group name
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Name;

        /// <summary>
        /// Comments
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Comment;

        public override string ToString()
        {
            return Name;
        }
    }

    /// <summary>
    /// Windows API GROUP_INFO_0 structure.  Returns only the group name.
    /// </summary>
    /// <remarks></remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct GroupInfo0
    {

        /// <summary>
        /// Group name
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Name;

        public override string ToString()
        {
            return Name;
        }
    }

    /// <summary>
    /// Windows API GROUP_INFO_1 structure.  Basic group information and attributes.
    /// </summary>
    /// <remarks></remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct GroupInfo1
    {

        /// <summary>
        /// Group name
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Name;

        /// <summary>
        /// Comments
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Comment;

        /// <summary>
        /// Group Id
        /// </summary>
        public IntPtr GroupId;

        /// <summary>
        /// Attributes
        /// </summary>
        public int Attributes;

        public override string ToString()
        {
            return Name;
        }
    }

    /// <summary>
    /// Windows API GROUP_INFO_2 structure.  Basic group information and attributes.
    /// </summary>
    /// <remarks></remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct GroupInfo2
    {

        /// <summary>
        /// Group name
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Name;

        /// <summary>
        /// Comments
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Comment;

        /// <summary>
        /// Group ID
        /// </summary>
        public int GroupId;

        /// <summary>
        /// Attributes
        /// </summary>
        public int Attributes;

        public override string ToString()
        {
            return Name;
        }
    }

    /// <summary>
    /// Windows API GROUP_INFO_3 structure.  Basic group information and attributes.
    /// </summary>
    /// <remarks></remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct GroupInfo3
    {

        /// <summary>
        /// Group name
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Name;

        /// <summary>
        /// Comments
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Comment;

        /// <summary>
        /// Group ID
        /// </summary>
        public IntPtr GroupId;

        /// <summary>
        /// Attributes
        /// </summary>
        public int Attributes;

        public override string ToString()
        {
            return Name;
        }
    }

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

    /// <summary>
    /// Network API status result enum
    /// </summary>
    [Flags]
    public enum NET_API_STATUS : uint
    {

        /// <summary>
        /// Success
        /// </summary>
        NERR_Success = 0U,

        /// <summary>
        /// Invalid computer
        /// </summary>
        NERR_InvalidComputer = 2351U,


        /// <summary>
        /// Not primary user
        /// </summary>
        NERR_NotPrimary = 2226U,

        /// <summary>
        /// Special Group Operation exception
        /// </summary>
        NERR_SpeGroupOp = 2234U,

        /// <summary>
        /// Last Admin cannot be deleted error
        /// </summary>
        NERR_LastAdmin = 2452U,

        /// <summary>
        /// Bad Password
        /// </summary>
        NERR_BadPassword = 2203U,

        /// <summary>
        /// Password too short
        /// </summary>
        NERR_PasswordTooShort = 2245U,

        /// <summary>
        /// User not found
        /// </summary>
        NERR_UserNotFound = 2221U,

        /// <summary>
        /// Access denied
        /// </summary>
        ERROR_ACCESS_DENIED = 5U,

        /// <summary>
        /// Out of memory
        /// </summary>
        ERROR_NOT_ENOUGH_MEMORY = 8U,

        /// <summary>
        /// Invalid parameters
        /// </summary>
        ERROR_INVALID_PARAMETER = 87U,

        /// <summary>
        /// Invalid name
        /// </summary>
        ERROR_INVALID_NAME = 123U,

        /// <summary>
        /// Invalid level
        /// </summary>
        ERROR_INVALID_LEVEL = 124U,

        /// <summary>
        /// Session credential conflict
        /// </summary>
        ERROR_SESSION_CREDENTIAL_CONFLICT = 1219U
    }

    /// <summary>
    /// UserGroup0 structure
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct UserGroup0
    {

        /// <summary>
        /// User group name
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Name;
    }

    /// <summary>
    /// UserLocalGroup1 structure
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct UserLocalGroup1
    {
        /// <summary>
        /// Sid
        /// </summary>
        public IntPtr Sid;

        /// <summary>
        /// Sid usage type
        /// </summary>
        public SidUsage SidUsage;

        /// <summary>
        /// Group name
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Name;
    }




    /// <summary>
    /// Network Information static class
    /// </summary>
    public static class NetInfoApi
    {
        [DllImport("Secur32.dll", EntryPoint = "GetUserNameExW", CharSet = CharSet.Unicode)]
        static extern bool GetUserNameEx(ExtendedNameFormat NameFormat, [MarshalAs(UnmanagedType.LPWStr)] string lpNameBuffer, ref int lpnSize);
        [DllImport("Secur32.dll", EntryPoint = "GetUserNameExW", CharSet = CharSet.Unicode)]
        static extern bool GetUserNameEx(ExtendedNameFormat NameFormat, IntPtr lpNameBuffer, ref int lpnSize);
        [DllImport("advapi32.dll", EntryPoint = "GetUserNameW", CharSet = CharSet.Unicode)]
        static extern bool GetUserName(IntPtr lpNameBuffer, ref int lpnSize);
        [DllImport("netapi32.dll")]
        public static extern NET_API_STATUS NetUserGetInfo([MarshalAs(UnmanagedType.LPWStr)] string servername, [MarshalAs(UnmanagedType.LPWStr)] string username, int level, ref MemPtr bufptr);
        [DllImport("netapi32.dll", EntryPoint = "NetServerEnum", CharSet = CharSet.Unicode)]
        static extern NET_API_STATUS NetServerEnum([MarshalAs(UnmanagedType.LPWStr)] string servername, int level, ref MemPtr bufptr, int prefmaxlen, ref int entriesread, ref int totalentries, ServerTypes serverType, [MarshalAs(UnmanagedType.LPWStr)] string domain, ref IntPtr resume_handle);
        [DllImport("netapi32.dll", EntryPoint = "NetServerGetInfo", CharSet = CharSet.Unicode)]
        static extern NET_API_STATUS NetServerGetInfo([MarshalAs(UnmanagedType.LPWStr)] string servername, int level, ref MemPtr bufptr);
        [DllImport("netapi32.dll", EntryPoint = "NetLocalGroupEnum", CharSet = CharSet.Unicode)]
        static extern NET_API_STATUS NetLocalGroupEnum([MarshalAs(UnmanagedType.LPWStr)] string servername, int level, ref MemPtr bufptr, int prefmaxlen, ref int entriesread, ref int totalentries, ref IntPtr resume_handle);
        [DllImport("netapi32.dll", EntryPoint = "NetGroupEnum", CharSet = CharSet.Unicode)]
        static extern NET_API_STATUS NetGroupEnum([MarshalAs(UnmanagedType.LPWStr)] string servername, int level, ref MemPtr bufptr, int prefmaxlen, ref int entriesread, ref int totalentries, ref IntPtr resume_handle);
        [DllImport("netapi32.dll", EntryPoint = "NetUserEnum", CharSet = CharSet.Unicode)]
        static extern NET_API_STATUS NetUserEnum([MarshalAs(UnmanagedType.LPWStr)] string servername, int level, int filter, ref MemPtr bufptr, int prefmaxlen, ref int entriesread, ref int totalentries, ref IntPtr resume_handle);
        [DllImport("netapi32.dll")]
        static extern NET_API_STATUS NetApiBufferFree(IntPtr buffer);
        [DllImport("netapi32.dll")]
        static extern NET_API_STATUS NetApiBufferAllocate(int bytecount, ref IntPtr buffer);
        [DllImport("Netapi32.dll", CharSet = CharSet.Unicode)]
        static extern NET_API_STATUS NetGetJoinInformation(string lpServer, ref IntPtr lpNameBuffer, ref NetworkJoinStatus bufferType);
        [DllImport("netapi32.dll")]
        static extern NET_API_STATUS NetGroupGetUsers([MarshalAs(UnmanagedType.LPWStr)] string servername, [MarshalAs(UnmanagedType.LPWStr)] string groupname, int level, ref MemPtr bufptr, int prefmaxlen, ref int entriesread, ref int totalentries, ref IntPtr ResumeHandle);
        [DllImport("netapi32.dll")]
        static extern NET_API_STATUS NetLocalGroupGetMembers([MarshalAs(UnmanagedType.LPWStr)] string servername, [MarshalAs(UnmanagedType.LPWStr)] string groupname, int level, ref MemPtr bufptr, int prefmaxlen, ref int entriesread, ref int totalentries, ref IntPtr ResumeHandle);



        /// <summary>
        /// Return the current user's full display name
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string CurrentUserFullName(ExtendedNameFormat type = ExtendedNameFormat.NameDisplay)
        {
            string CurrentUserFullNameRet = default;
            MemPtr lps;
            int cb = 10240;

            lps = new MemPtr(10240L);
            lps.ZeroMemory();

            if (GetUserNameEx(type, lps.Handle, ref cb))
            {
                CurrentUserFullNameRet = lps.ToString();
            }
            else
            {
                CurrentUserFullNameRet = null;
            }

            lps.Free();
            return CurrentUserFullNameRet;
        }

        /// <summary>
        /// Return the username for the current user
        /// </summary>
        /// <returns></returns>
        public static string CurrentUserName()
        {
            string CurrentUserNameRet = default;
            MemPtr lps = new MemPtr();
            int cb = 10240;
            lps.ReAlloc(10240L);
            lps.ZeroMemory();
            if (GetUserName(lps.Handle, ref cb))
            {
                CurrentUserNameRet = lps.ToString();
            }
            else
            {
                CurrentUserNameRet = null;
            }

            lps.Free();
            return CurrentUserNameRet;
        }


        /// <summary>
        /// Enumerates all computers visible to the specified computer on the specified domain.
        /// </summary>
        /// <param name="computer">Optional computer name.  The local computer is assumed if this parameter is null.</param>
        /// <param name="domain">Optional domain name.  The primary domain of the specified computer is assumed if this parameter is null.</param>
        /// <returns>An array of ServerInfo1 objects.</returns>
        /// <remarks></remarks>
        public static ServerInfo101[] EnumServers()
        {
            MemPtr adv;
            
            var mm = new MemPtr();
            int en = 0;
            int ten = 0;
            
            ServerInfo101[] servers;
            
            int i;
            int c;

            var inul = new IntPtr();

            NetServerEnum(null, 101, ref mm, -1, ref en, ref ten, ServerTypes.WindowsNT, null, ref inul);

            adv = mm;
            c = ten;

            servers = new ServerInfo101[c + 1];

            for (i = 0; i < c; i++)
            {
                servers[i] = adv.ToStruct<ServerInfo101>();
                adv = adv + Marshal.SizeOf<ServerInfo101>();
            }

            mm.NetFree();
            return servers;
        }

        /// <summary>
        /// Gets network information for the specified computer.
        /// </summary>
        /// <param name="computer">Computer for which to retrieve the information.</param>
        /// <param name="info">A ServerInfo101 structure that receives the information.</param>
        /// <remarks></remarks>
        public static void GetServerInfo(string computer, ref ServerInfo101 info)
        {
            var mm = new MemPtr();
            NetInfoApi.NetServerGetInfo(computer, 101, ref mm);
            info = mm.ToStruct<ServerInfo101>();
            mm.NetFree();
        }

        /// <summary>
        /// Enumerates the groups on a system.
        /// </summary>
        /// <param name="computer">The computer to enumerate.</param>
        /// <returns>An array of GroupInfo2 structures.</returns>
        /// <remarks></remarks>
        public static GroupInfo2[] EnumGroups(string computer)
        {
            var mm = new MemPtr();
            int en = 0;
            int ten = 0;
            
            MemPtr adv;
            GroupInfo2[] grp;

            var inul = new IntPtr();

            NetInfoApi.NetGroupEnum(computer, 2, ref mm, -1, ref en, ref ten, ref inul);
            
            adv = mm;

            int i;
            int c = ten;

            grp = new GroupInfo2[c + 1];

            for (i = 0; i < c; i++)
            {
                grp[i] = adv.ToStruct<GroupInfo2>();
                adv = adv + Marshal.SizeOf<GroupInfo2>();
            }

            mm.NetFree();

            return grp;
        }

        /// <summary>
        /// Enumerates local groups for the specified computer.
        /// </summary>
        /// <param name="computer">The computer to enumerate.</param>
        /// <returns>An array of LocalGroupInfo1 structures.</returns>
        /// <remarks></remarks>
        public static LocalGroupInfo1[] EnumLocalGroups(string computer)
        {
            var mm = new MemPtr();
            int en = 0;
            int ten = 0;

            MemPtr adv;
            LocalGroupInfo1[] grp;

            var inul = new IntPtr();

            NetInfoApi.NetLocalGroupEnum(computer, 1, ref mm, -1, ref en, ref ten, ref inul);
            adv = mm;

            int i;
            int c = ten;

            grp = new LocalGroupInfo1[c + 1];

            for (i = 0; i < c; i++)
            {
                grp[i] = adv.ToStruct<LocalGroupInfo1>();
                adv = adv + Marshal.SizeOf<LocalGroupInfo1>();
            }

            mm.NetFree();

            return grp;
        }

        /// <summary>
        /// Enumerates users of a given group.
        /// </summary>
        /// <param name="computer">Computer for which to retrieve the information.</param>
        /// <param name="group">Group to enumerate.</param>
        /// <returns>An array of user names.</returns>
        /// <remarks></remarks>
        public static string[] GroupUsers(string computer, string group)
        {
            var mm = new MemPtr();

            MemPtr op = new MemPtr();

            int cbt = 0;
            int cb = 0;

            string[] s = null;

            try
            {
                var inul = new IntPtr();

                if (NetInfoApi.NetGroupGetUsers(computer, group, 0, ref mm, -1, ref cb, ref cbt, ref inul) == NET_API_STATUS.NERR_Success)
                {
                    op = mm;
                    UserGroup0 z;
                    int i;

                    s = new string[cb];
                    
                    for (i = 0; i < cb; i++)
                    {
                        z = mm.ToStruct<UserGroup0>();
                        s[i] = z.Name;

                        mm = mm + Marshal.SizeOf<UserGroup0>();
                    }
                }
            }
            catch
            {
                throw new NativeException();
            }

            op.NetFree();
            
            return s;
        }

        /// <summary>
        /// Gets the members of the specified local group on the specified machine.
        /// </summary>
        /// <param name="computer">The computer for which to retrieve the information.</param>
        /// <param name="group">The name of the group to enumerate.</param>
        /// <param name="SidType">The type of group members to return.</param>
        /// <returns>A list of group member names.</returns>
        /// <remarks></remarks>
        public static string[] LocalGroupUsers(string computer, string group, SidUsage SidType = SidUsage.SidTypeUser)
        {
            var mm = new MemPtr();

            MemPtr op = new MemPtr();

            int x = 0;
            int cbt = 0;
            int cb = 0;
            
            string[] s = null;
            
            try
            {
                var inul = new IntPtr();
                if (NetInfoApi.NetLocalGroupGetMembers(computer, group, 1, ref mm, -1, ref cb, ref cbt, ref inul) == NET_API_STATUS.NERR_Success)
                {
                    if (cb == 0)
                    {
                        mm.NetFree();
                        return null;
                    }

                    op = mm;

                    UserLocalGroup1 z;
                    int i;

                    s = new string[cb];
                    
                    for (i = 0; i < cb; i++)
                    {
                        z = mm.ToStruct<UserLocalGroup1>();
                        if (z.SidUsage == SidType)
                        {
                            s[x] = z.Name;
                            mm = mm + Marshal.SizeOf(z);
                            x += 1;
                        }
                    }

                    Array.Resize(ref s, x);
                }
            }
            catch
            {
                throw new NativeException();
            }

            op.NetFree();
            return s;
        }

        /// <summary>
        /// Grab the join status for the specified computer.
        /// </summary>
        /// <param name="joinStatus">Receives the NetworkJoinStatus value.</param>
        /// <param name="Computer">Optional name of a computer for which to retrieve the NetworkJoinStatus information.  If this parameter is null, the local computer is assumed.</param>
        /// <returns>The name of the current domain or workgroup for the specified computer.</returns>
        /// <remarks></remarks>
        public static string GrabJoin(ref NetworkJoinStatus joinStatus, string Computer = null)
        {
            try
            {
                var mm = new MemPtr();
                mm.NetAlloc(1024);

                NetInfoApi.NetGetJoinInformation(Computer, ref mm.handle, ref joinStatus);

                string s = (string)mm;
                mm.NetFree();

                return s;
            }
            catch
            {
                throw new NativeException();
            }
        }

        /// <summary>
        /// Enumerate users into a USER_INFO_11 structure.
        /// </summary>
        /// <param name="machine">Computer on which to perform the enumeration.  If this parameter is null, the local machine is assumed.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static UserInfo11[] EnumUsers11(string machine = null)
        {

            try
            {
                int cb = 0;
                int er = 0;

                MemPtr rh = IntPtr.Zero;

                int te = 0;

                MemPtr buff = new MemPtr();
                UserInfo11[] usas;

                try
                {
                    cb = Marshal.SizeOf<UserInfo11>();
                }
                catch
                {
                    //Interaction.MsgBox(ex.Message + "\r\n" + "\r\n" + "Stack Trace: " + ex.StackTrace, MsgBoxStyle.Exclamation);
                }

                var inul = new IntPtr();
                var err = NetInfoApi.NetUserEnum(machine, 11, 0, ref buff, -1, ref er, ref te, ref inul);

                rh = buff;
                usas = new UserInfo11[te];

                for (int i = 0; i < te; i++)
                {
                    
                    usas[i] = buff.ToStruct<UserInfo11>();
                    usas[i].LogonHours = IntPtr.Zero;
                
                    buff = buff + cb;
                }

                rh.NetFree();
                return usas;

            }
            catch
            {
                throw new NativeException();
            }
        }

        /// <summary>
        /// For Windows 8, retrieves the user's Microsoft login account information.
        /// </summary>
        /// <param name="machine">Computer on which to perform the enumeration.  If this parameter is null, the local machine is assumed.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static UserInfo24[] EnumUsers24(string machine = null)
        {
            try
            {
                MemPtr rh = IntPtr.Zero;

                int i = 0;
                var uorig = EnumUsers11();

                UserInfo24[] usas;

                int c = uorig.Length - 1;

                usas = new UserInfo24[c + 1];
                var loopTo = c;

                for (i = 0; i < c; i++)
                {
                    NetInfoApi.NetUserGetInfo(machine, uorig[i].Name, 24, ref rh);
                    usas[i] = rh.ToStruct<UserInfo24>();

                    rh.NetFree();
                }

                return usas;
            }
            catch
            {
                throw new NativeException();
            }
        }
    }
}