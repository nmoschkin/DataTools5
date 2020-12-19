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
using DataTools.Win32;

namespace DataTools.Win32.Network
{
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
}
