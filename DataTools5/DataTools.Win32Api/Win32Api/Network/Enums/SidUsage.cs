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
using DataTools.Win32Api;

namespace DataTools.Win32Api.Network
{
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
}
