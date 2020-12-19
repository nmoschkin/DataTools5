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
// Licensed Under the Microsoft Public License   
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
    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    /// <summary>
    /// OSVERSIONINFOEX structure with additional utility and functionality.
    /// </summary>
    /// <remarks></remarks>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct OSVERSIONINFOEX
    {

        /// <summary>
        /// The size of this structure, in bytes.
        /// </summary>
        /// <remarks></remarks>
        public int dwOSVersionInfoSize;

        /// <summary>
        /// The major version of the current operating system.
        /// </summary>
        /// <remarks></remarks>
        public int dwMajorVersion;

        /// <summary>
        /// The minor verison of the current operating system.
        /// </summary>
        /// <remarks></remarks>
        public int dwMinorVersion;

        /// <summary>
        /// The build number of the current operating system.
        /// </summary>
        /// <remarks></remarks>
        public int dwBuildNumber;

        /// <summary>
        /// The platform Id of the current operating system.
        /// Currently, this value should always be 2.
        /// </summary>
        /// <remarks></remarks>
        public int dwPlatformId;

        /// <summary>
        /// Private character buffer.
        /// </summary>
        /// <remarks></remarks>
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U2, SizeConst = 128)]
        private char[] szCSDVersionChar;

        /// <summary>
        /// The Service Pack name (if applicable)
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string szCSDVersion
        {
            get
            {
                GCHandle gch = GCHandle.Alloc(szCSDVersionChar, GCHandleType.Pinned);
                MemPtr mm = gch.AddrOfPinnedObject();

                string vs = (string)mm;
                gch.Free();

                return vs;
            }
        }

        /// <summary>
        /// Service pack major verison number.
        /// </summary>
        /// <remarks></remarks>
        public short wServicePackMajor;

        /// <summary>
        /// Server pack minor verison number.
        /// </summary>
        /// <remarks></remarks>
        public short wServicePackMinor;

        /// <summary>
        /// The Windows Suite mask.
        /// </summary>
        /// <remarks></remarks>
        public OSSuiteMask wSuiteMask;

        /// <summary>
        /// The product type flags.
        /// </summary>
        /// <remarks></remarks>
        public OSProductType wProductType;

        /// <summary>
        /// Reserved
        /// </summary>
        /// <remarks></remarks>
        public byte wReserved;

    }
}
