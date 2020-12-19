// ************************************************* ''
// DataTools C# Native Utility Library For Windows - Interop
//
// Module: IfDefApi
//         The almighty network interface native API.
//         Some enum documentation comes from the MSDN.
//
// (and an exercise in creative problem solving and data-structure marshaling.)
//
// Copyright (C) 2011-2020 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the Microsoft Public License   
// ************************************************* ''


using System;
using System.ComponentModel;
using System.Net;
using System.Runtime.InteropServices;
using DataTools.Memory;
using DataTools.Win32;

namespace DataTools.Win32.Network
{
    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    /* TODO ERROR: Skipped RegionDirectiveTrivia */

    /// <summary>
    /// Adapter enumeration result
    /// </summary>
    public enum ADAPTER_ENUM_RESULT
    {
        /// <summary>
        /// Success
        /// </summary>
        /// <remarks></remarks>
        NO_ERROR = 0,

        /// <summary>
        /// An address has not yet been associated with the network endpoint.DHCP lease information was available.
        /// </summary>
        /// <remarks></remarks>
        ERROR_ADDRESS_NOT_ASSOCIATED = 1228,

        /// <summary>
        /// The buffer size indicated by the SizePointer parameter is too small to hold the adapter information or the AdapterAddresses parameter is NULL.The SizePointer parameter returned points to the required size of the buffer to hold the adapter information.
        /// </summary>
        /// <remarks></remarks>
        ERROR_BUFFER_OVERFLOW = 111,

        /// <summary>
        /// One of the parameters is invalid.This error is returned for any of the following conditions : the SizePointer parameter is NULL, the Address parameter is not AfInet, AfInet6, or AfUnspecified, or the address information for the parameters requested is greater than ULONG_MAX.
        /// </summary>
        /// <remarks></remarks>
        ERROR_INVALID_PARAMETER = 87,

        /// <summary>
        /// Insufficient memory resources are available to complete the operation.
        /// </summary>
        /// <remarks></remarks>
        ERROR_NOT_ENOUGH_MEMORY = 8,

        /// <summary>
        /// No addresses were found for the requested parameters.
        /// </summary>
        /// <remarks></remarks>
        ERROR_NO_DATA = 232
    }
}
