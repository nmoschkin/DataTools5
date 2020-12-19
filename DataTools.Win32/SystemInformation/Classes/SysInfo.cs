// ************************************************* ''
// DataTools C# Native Utility Library For Windows 
//
// Module: SystemInfo
//         Provides basic information about the
//         current computer.
// 
// Copyright (C) 2011-2020 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the MIT License   
// ************************************************* ''

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Navigation;

using DataTools.Text;
using DataTools.Win32.Memory;

namespace DataTools.SystemInformation
{
    
    
    /// <summary>
    /// System information.
    /// </summary>
    public static class SysInfo
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        internal static extern bool IsWindowsXPOrGreater();

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        internal static extern bool IsWindowsXPSP1OrGreater();

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        internal static extern bool IsWindowsXPSP2OrGreater();

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        internal static extern bool IsWindowsXPSP3OrGreater();

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        internal static extern bool IsWindowsVistaOrGreater();

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        internal static extern bool IsWindowsVistaSP1OrGreater();

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        internal static extern bool IsWindowsVistaSP2OrGreater();

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        internal static extern bool IsWindows7OrGreater();

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        internal static extern bool IsWindows7SP1OrGreater();

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        internal static extern bool IsWindows8OrGreater();

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        internal static extern bool IsWindows8Point10OrGreater();

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        internal static extern bool IsWindows10OrGreater();

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        internal static extern bool IsWindowsServer();

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        internal static extern bool IsWindowsVersionOrGreater(ushort maj, ushort min, ushort servicePack);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        internal static extern int GetVersionExW([MarshalAs(UnmanagedType.Struct)] ref OSVERSIONINFO_STRUCT pData);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        internal static extern int GetVersionExW([MarshalAs(UnmanagedType.Struct)] ref OSVERSIONINFOEX pData);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        internal static extern bool GetFirmwareType(ref FirmwareType firmwareType);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        internal static extern bool IsNativeVhdBoot(ref bool nativeVhd);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        internal static extern void GetNativeSystemInfo(ref NativeEnvironment lpSysInfo);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        internal static extern bool GetLogicalProcessorInformation(IntPtr buffer, ref int ReturnLength);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        internal static extern bool GlobalMemoryStatusEx(ref MemoryStatusDetails lpStatusEx);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        internal static extern bool GlobalMemoryStatusEx(MemPtr lpStatusEx);

        /// <summary>
        /// Contains details about the current operating system and computing environment.
        /// </summary>
        /// <remarks></remarks>
        public static OSVersionInfo OSInfo { get; private set; } = GetOSInfo();

        /// <summary>
        /// The ping IP address to use when determining the status of internet connectivity for the local machine.
        /// The default is OpenDNS primary nameserver.
        /// </summary>
        public static string PingAddress { get; set; } = "208.67.222.222";



        internal static NativeEnvironment nativeEnv;

        /// <summary>
        /// Contains specific configuration details about the current native system mode and environment.
        /// </summary>
        /// <returns></returns>
        public static NativeEnvironment NativeEnvironment
        {
            get
            {
                return nativeEnv;
            }
        }

        internal static MemoryStatusDetails _memInfo;

        /// <summary>
        /// Contains detailed memory information.
        /// </summary>
        /// <returns></returns>
        public static MemoryStatusDetails MemoryInfo
        {
            get
            {
                return _memInfo;
            }
        }

        /// <summary>
        /// Returns a value indicating whether or not the local computer can reach the internet.
        /// </summary>
        /// <remarks>
        /// May take up to 2 seconds to return false.
        /// Don't access this property in functions that need to return instantaneously.
        /// </remarks>
        public static async Task<bool> GetHasInternetAsync()
        {
            try
            {
                Ping ping = new Ping();

                var reply = await ping.SendPingAsync(PingAddress, 2000);
                
                if (reply.Status == IPStatus.Success)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch
            {
                return false;
            }
        }

        internal static SystemLogicalProcessorInformation[] _procRaw;

        /// <summary>
        /// Contains information about the logical processors on the system.
        /// </summary>
        /// <returns></returns>
        public static SystemLogicalProcessorInformation[] LogicalProcessors
        {
            get
            {
                return _procRaw;
            }
        }

        static SysInfo()
        {

            // let's get some version information!
            var mm = new MemPtr();

            GetNativeSystemInfo(ref nativeEnv);

            _memInfo.dwLength = Marshal.SizeOf(_memInfo);
            GlobalMemoryStatusEx(ref _memInfo);

            // now let's figure out how many processors we have on this system
            MemPtr org;
            
            var lp = new SystemLogicalProcessorInformation();
            
            SystemLogicalProcessorInformation[] rets;

            int i;
            int c;

            // The maximum number of processors for any version of Windows is 128, we'll allocate more for extra information.
            mm.Alloc(Marshal.SizeOf(lp) * 1024);

            // record the original memory pointer

            org = mm;
            var lRet = (int)mm.Length;

            GetLogicalProcessorInformation(mm, ref lRet);

            c = (int)(lRet / (double)Marshal.SizeOf(lp));

            rets = new SystemLogicalProcessorInformation[c];
            nativeEnv.nop = 0;

            for (i = 0; i < c; i++)
            {
                rets[i] = mm.ToStruct<SystemLogicalProcessorInformation>();
                mm += Marshal.SizeOf(lp);

                // what we're really after are the number of cores.
                if (rets[i].Relationship == LogicalProcessorRelationship.RelationProcessorCore)
                {
                    nativeEnv.nop++;
                }
            }

            // store that data in case we need it for later.
            _procRaw = rets;

            // free our unmanaged resources.
            org.Free();
        }

        /// <summary>
        /// Retrieves the current Operating System information.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        internal static OSVersionInfo GetOSInfo()
        {
            var lpOS = new OSVERSIONINFOEX();
            lpOS.dwOSVersionInfoSize = Marshal.SizeOf(lpOS);
            string dispVer = null;

            GetVersionExW(ref lpOS);

            var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
            if (key is object)
            {
                int maj = (int)(key.GetValue("CurrentMajorVersionNumber"));
                int min = (int)(key.GetValue("CurrentMinorVersionNumber"));

                int cb = int.Parse((string)key.GetValue("CurrentBuildNumber"));
                int build = cb;
                
                dispVer = (string)key.GetValue("DisplayVersion");

                //string name = (string)(key.GetValue("ProductName"));

                lpOS.dwMajorVersion = maj;
                lpOS.dwMinorVersion = min;
                lpOS.dwBuildNumber = build;

                key.Close();
            }

            return new OSVersionInfo(lpOS, dispVer);
        }
    }
}
