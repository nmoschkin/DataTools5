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
}
