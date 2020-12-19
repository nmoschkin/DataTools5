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
}
