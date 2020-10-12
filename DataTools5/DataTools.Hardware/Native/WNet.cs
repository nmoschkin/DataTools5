// ' ************************************************* ''
// ' DataTools Visual Basic Utility Library - Interop
// '
// ' Module: WNet
// '         Back-end Windows Networking Resources
// '
// ' Copyright (C) 2011-2020 Nathan Moschkin
// ' All Rights Reserved
// '
// ' Licensed Under the Microsoft Public License   
// ' ************************************************* ''

using System;
using System.Runtime.InteropServices;
using DataTools.Memory;
using Microsoft.VisualBasic;

namespace DataTools.Hardware.Native
{
    // '
    // '  Network Resources.
    // '
    internal static class WNet
    {
        public const int RESOURCE_CONNECTED = 0x1;
        public const int RESOURCE_GLOBALNET = 0x2;
        public const int RESOURCE_REMEMBERED = 0x3;
        public const int RESOURCE_RECENT = 0x4;
        public const int RESOURCE_CONTEXT = 0x5;
        public const int RESOURCETYPE_ANY = 0x0;
        public const int RESOURCETYPE_DISK = 0x1;
        public const int RESOURCETYPE_PRINT = 0x2;
        public const int RESOURCETYPE_RESERVED = 0x8;
        public const int RESOURCETYPE_UNKNOWN = unchecked((int)0xFFFFFFFF);
        public const int RESOURCEUSAGE_CONNECTABLE = 0x1;
        public const int RESOURCEUSAGE_CONTAINER = 0x2;
        public const int RESOURCEUSAGE_NOLOCALDEVICE = 0x4;
        public const int RESOURCEUSAGE_SIBLING = 0x8;
        public const int RESOURCEUSAGE_ATTACHED = 0x10;
        public const int RESOURCEUSAGE_ALL = RESOURCEUSAGE_CONNECTABLE | RESOURCEUSAGE_CONTAINER | RESOURCEUSAGE_ATTACHED;
        public const int RESOURCEUSAGE_RESERVED = unchecked((int)0x80000000);
        public const int RESOURCEDISPLAYTYPE_GENERIC = 0x0;
        public const int RESOURCEDISPLAYTYPE_DOMAIN = 0x1;
        public const int RESOURCEDISPLAYTYPE_SERVER = 0x2;
        public const int RESOURCEDISPLAYTYPE_SHARE = 0x3;
        public const int RESOURCEDISPLAYTYPE_FILE = 0x4;
        public const int RESOURCEDISPLAYTYPE_GROUP = 0x5;
        public const int RESOURCEDISPLAYTYPE_NETWORK = 0x6;
        public const int RESOURCEDISPLAYTYPE_ROOT = 0x7;
        public const int RESOURCEDISPLAYTYPE_SHAREADMIN = 0x8;
        public const int RESOURCEDISPLAYTYPE_DIRECTORY = 0x9;
        public const int RESOURCEDISPLAYTYPE_TREE = 0xA;
        public const int RESOURCEDISPLAYTYPE_NDSCONTAINER = 0xB;
        public const int CONNECT_UPDATE_PROFILE = 0x1;
        public const int CONNECT_UPDATE_RECENT = 0x2;
        public const int CONNECT_TEMPORARY = 0x4;
        public const int CONNECT_INTERACTIVE = 0x8;
        public const int CONNECT_PROMPT = 0x10;
        public const int CONNECT_NEED_DRIVE = 0x20;
        public const int CONNECT_REFCOUNT = 0x40;
        public const int CONNECT_REDIRECT = 0x80;
        public const int CONNECT_LOCALDRIVE = 0x100;
        public const int CONNECT_CURRENT_MEDIA = 0x200;
        public const int CONNECT_DEFERRED = 0x400;
        public const int CONNECT_RESERVED = unchecked((int)0xFF000000);
        public const int CONNECT_COMMANDLINE = 0x800;
        public const int CONNECT_CMD_SAVECRED = 0x1000;
        public const int CONNECT_CRED_RESET = 0x2000;
        public const int ERROR_CALL_NOT_IMPLEMENTED = 120;
        public const int ERROR_NO_MORE_ITEMS = 259;
        public const int ERROR_NO_NETWORK = 1222;
        public const int ERROR_MORE_DATA = 234;
        public const int ERROR_INVALID_PARAMETER = 87;
        public const int ERROR_CONNECTED_OTHER_PASSWORD = 2108;
        public const int ERROR_CONNECTED_OTHER_PASSWORD_DEFAULT = 2109;
        public const int ERROR_BAD_USERNAME = 2202;
        public const int ERROR_NOT_CONNECTED = 2250;
        public const int ERROR_OPEN_FILES = 2401;
        public const int ERROR_ACTIVE_CONNECTIONS = 2402;
        public const int ERROR_DEVICE_IN_USE = 2404;
        public const int ERROR_INVALID_PASSWORD = 86;
        public const int NO_ERROR = 0;

        public struct NETRESOURCE
        {
            public int dwScope;
            public int dwType;
            public int dwDisplayType;
            public int dwUsage;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string lpLocalName;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string lpRemoteName;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string lpComment;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string lpProvider;

            public override string ToString()
            {
                return lpRemoteName;
            }
        }

        [DllImport("Mpr.dll", EntryPoint = "WNetOpenEnumW", CharSet = CharSet.Unicode)]
        public static extern int WNetOpenEnum(int dwScope, int dwType, int dwUsage, IntPtr lpNetResource, ref IntPtr lphEnum);
        [DllImport("Mpr.dll", EntryPoint = "WNetEnumResourceW", CharSet = CharSet.Unicode)]
        public static extern int WNetEnumResource(IntPtr hEnum, ref int lpcCount, IntPtr lpBuffer, ref int lpBufferSize);
        [DllImport("Mpr.dll", CharSet = CharSet.Unicode)]
        public static extern int WNetCloseEnum(IntPtr hEnum);


        // DWORD WNetAddConnection3(
        // _In_ HWND          hwndOwner,
        // _In_ LPNETRESOURCE lpNetResource,
        // _In_ LPTSTR        lpPassword,
        // _In_ LPTSTR        lpUserName,
        // _In_ DWORD         dwFlags
        // );


        [DllImport("Mpr.dll", EntryPoint = "WNetAddConnection3W", CharSet = CharSet.Unicode)]
        public static extern int WNetAddConnection3(IntPtr hwndOwner, IntPtr lpNetResource, [MarshalAs(UnmanagedType.LPTStr)] string lpPassword, [MarshalAs(UnmanagedType.LPTStr)] string lpusername, int dwFlags);

        public static NETRESOURCE[] EnumNetwork()
        {
            NETRESOURCE[] EnumNetworkRet = default;
            EnumNetworkRet = DoEnum(IntPtr.Zero);
            return EnumNetworkRet;
        }

        public static NETRESOURCE[] EnumComputer(string computer, string username = null, string password = null)
        {
            NETRESOURCE[] EnumComputerRet = default;
            var lpnet = new NETRESOURCE();
            MemPtr mm = new MemPtr();

            if (computer.Substring(0, 2) == @"\\")
                computer = computer.Substring(2);

            mm.ReAlloc(10240L);

            if (username is object && password is object)
            {
                lpnet.lpRemoteName = @"\\" + computer;
                Marshal.StructureToPtr(lpnet, mm.Handle, false);
                int res = WNet.WNetAddConnection3(IntPtr.Zero, mm.Handle,  password, username, CONNECT_INTERACTIVE);
                mm.Free();
                if (res != 0)
                {
                    return null;
                }

                mm.ReAlloc(10240L);
            }

            lpnet.dwDisplayType = RESOURCEDISPLAYTYPE_SERVER;
            lpnet.lpRemoteName = @"\\" + computer;
            lpnet.dwScope = RESOURCE_CONTEXT;
            lpnet.dwUsage = RESOURCEUSAGE_CONTAINER;
            Marshal.StructureToPtr(lpnet, mm.Handle, false);
            EnumComputerRet = DoEnum(mm.Handle);
            mm.Free();
            return EnumComputerRet;
        }

        public static NETRESOURCE[] DoEnum(IntPtr lpNet)
        {
            NETRESOURCE[] DoEnumRet = default;
            MemPtr mm = new MemPtr();
            int cb = 10240;
            NETRESOURCE[] bb = null;
            NETRESOURCE[] nin = null;
            IntPtr hEnum = IntPtr.Zero;
            int e = 0;
            e = WNetOpenEnum(RESOURCE_GLOBALNET, RESOURCETYPE_DISK, RESOURCEUSAGE_ALL, lpNet, ref hEnum);
            if (e != NO_ERROR)
            {
                return null;
            }

            e = 0;
            mm.ReAlloc(10240L);
            int arglpcCount = 1;
            while (WNetEnumResource(hEnum, ref arglpcCount, mm, ref cb) == NO_ERROR)
            {
                Array.Resize(ref bb, e + 1);
                nin = DoEnum(mm.Handle);
                bb[e] = mm.ToStruct<NETRESOURCE>();
                if (nin is object)
                {
                    bb = WNACat(bb, nin);
                    nin = null;
                }

                if (bb is object)
                    e = bb.Length;
                else
                    e = 0;
            }

            mm.Free();
            WNetCloseEnum(hEnum);
            DoEnumRet = bb;
            return DoEnumRet;
        }

        private static NETRESOURCE[] WNACat(NETRESOURCE[] a1, NETRESOURCE[] a2)
        {
            NETRESOURCE[] WNACatRet = default;
            int e = (a1 is object ? a1.Length : 0) + (a2 is object ? a2.Length : 0);
            NETRESOURCE[] a3 = null;
            a3 = new NETRESOURCE[e];
            int c = 0;
            if (a1 is object)
            {
                a1.CopyTo(a3, 0);
                c = a1.Length;
            }

            if (a2 is object)
            {
                a2.CopyTo(a3, c);
            }

            WNACatRet = a3;
            return WNACatRet;
        }
    }
}