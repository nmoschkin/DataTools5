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
using DataTools.Memory;
using DataTools.Text;

namespace DataTools.SystemInformation
{
    /// <summary>
    /// Windows operating system suite masks.
    /// </summary>
    /// <remarks></remarks>
    [Flags]
    public enum OSSuiteMask : ushort
    {

        /// <summary>
        /// Microsoft BackOffice components are installed.
        /// </summary>
        [Description("Microsoft BackOffice components are installed.")]
        BackOffice = 0x4,

        /// <summary>
        /// Windows Server 2003, Web Edition is installed.
        /// </summary>
        [Description("Windows Server 2003, Web Edition is installed.")]
        Blade = 0x400,

        /// <summary>
        /// Windows Server 2003, Compute Cluster Edition is installed.
        /// </summary>
        [Description("Windows Server 2003, Compute Cluster Edition is installed.")]
        ComputeServer = 0x4000,

        /// <summary>
        /// Windows Server 2008 Datacenter, Windows Server 2003, Datacenter Edition, or Windows 2000 Datacenter Server is installed.
        /// </summary>
        [Description("Windows Server 2008 Datacenter, Windows Server 2003, Datacenter Edition, or Windows 2000 Datacenter Server is installed.")]
        DataCenter = 0x80,

        /// <summary>
        /// Windows Server 2008 Enterprise, Windows Server 2003, Enterprise Edition, or Windows 2000 Advanced Server is installed. Refer to the Remarks section for more information about this bit flag.
        /// </summary>
        [Description("Windows Server 2008 Enterprise, Windows Server 2003, Enterprise Edition, or Windows 2000 Advanced Server is installed. Refer to the Remarks section for more information about this bit flag.")]
        Enterprise = 0x2,

        /// <summary>
        /// Windows XP Embedded is installed.
        /// </summary>
        [Description("Windows XP Embedded is installed.")]
        EmbeddedNT = 0x40,

        /// <summary>
        /// Windows Vista Home Premium, Windows Vista Home Basic, or Windows XP Home Edition is installed.
        /// </summary>
        [Description("Windows Vista Home Premium, Windows Vista Home Basic, or Windows XP Home Edition is installed.")]
        Personal = 0x200,

        /// <summary>
        /// Remote Desktop is supported, but only one interactive session is supported. This value is set unless the system is running in application server mode.
        /// </summary>
        [Description("Remote Desktop is supported, but only one interactive session is supported. This value is set unless the system is running in application server mode.")]
        SingleUser = 0x100,

        /// <summary>
        /// Microsoft Small Business Server was once installed on the system, but may have been upgraded to another version of Windows. Refer to the Remarks section for more information about this bit flag.
        /// </summary>
        [Description("Microsoft Small Business Server was once installed on the system, but may have been upgraded to another version of Windows. Refer to the Remarks section for more information about this bit flag.")]
        SmallBusiness = 0x1,

        /// <summary>
        /// Microsoft Small Business Server is installed with the restrictive client license in force. Refer to the Remarks section for more information about this bit flag.
        /// </summary>
        [Description("Microsoft Small Business Server is installed with the restrictive client license in force. Refer to the Remarks section for more information about this bit flag.")]
        SmallBusinessRestricted = 0x20,

        /// <summary>
        /// Windows Storage Server 2003 R2 or Windows Storage Server 2003is installed.
        /// </summary>
        [Description("Windows Storage Server 2003 R2 or Windows Storage Server 2003is installed.")]
        StorageServer = 0x2000,

        /// <summary>
        /// Terminal Services is installed. This value is always set.
        /// If TERMINAL is set but SINGLEUSERTS is not set, the system is running in application server mode.
        /// </summary>
        [Description("Terminal Services is installed. This value is always set.")]
        Terminal = 0x10,

        /// <summary>
        /// Windows Home Server is installed.
        /// </summary>
        [Description("Windows Home Server is installed.")]
        HomeServer = 0x8000
    }
}
