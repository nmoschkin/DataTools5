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
// Licensed Under the MIT License   
// ************************************************* ''


using System;
using System.Runtime.InteropServices;

using DataTools.Text;
using DataTools.Win32;

namespace DataTools.Win32.Network
{
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
}
