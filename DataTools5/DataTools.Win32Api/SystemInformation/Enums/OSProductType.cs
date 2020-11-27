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
    /// Windows product type information.
    /// </summary>
    /// <remarks></remarks>
    [Flags]
    public enum OSProductType : byte
    {

        /// <summary>
        /// The system is a domain controller and the operating system is Windows Server 2012 R2, Windows Server 2012, Windows Server 2008 R2, Windows Server 2008, Windows Server 2003, or Windows 2000 Server.
        /// </summary>
        [Description("The system is a domain controller and the operating system is Windows Server 2012 R2, Windows Server 2012, Windows Server 2008 R2, Windows Server 2008, Windows Server 2003, or Windows 2000 Server.")]
        NTDomainController = 0x2,

        /// <summary>
        /// The operating system is Windows Server 2012 R2, Windows Server 2012, Windows Server 2008 R2, Windows Server 2008, Windows Server 2003, or Windows 2000 Server.
        /// </summary>
        [Description("The operating system is Windows Server 2012 R2, Windows Server 2012, Windows Server 2008 R2, Windows Server 2008, Windows Server 2003, or Windows 2000 Server.")]
        NTServer = 0x3,

        /// <summary>
        /// The operating system is Windows 8.1, Windows 8, Windows 7, Windows Vista, Windows XP Professional, Windows XP Home Edition, or Windows 2000 Professional.
        /// </summary>
        [Description("The operating system is Windows 8.1, Windows 8, Windows 7, Windows Vista, Windows XP Professional, Windows XP Home Edition, or Windows 2000 Professional.")]
        NTWorkstation = 0x1
    }
}
