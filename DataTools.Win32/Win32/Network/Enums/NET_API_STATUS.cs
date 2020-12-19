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
    /// Network API status result enum
    /// </summary>
    [Flags]
    public enum NET_API_STATUS : uint
    {

        /// <summary>
        /// Success
        /// </summary>
        NERR_Success = 0U,

        /// <summary>
        /// Invalid computer
        /// </summary>
        NERR_InvalidComputer = 2351U,


        /// <summary>
        /// Not primary user
        /// </summary>
        NERR_NotPrimary = 2226U,

        /// <summary>
        /// Special Group Operation exception
        /// </summary>
        NERR_SpeGroupOp = 2234U,

        /// <summary>
        /// Last Admin cannot be deleted error
        /// </summary>
        NERR_LastAdmin = 2452U,

        /// <summary>
        /// Bad Password
        /// </summary>
        NERR_BadPassword = 2203U,

        /// <summary>
        /// Password too short
        /// </summary>
        NERR_PasswordTooShort = 2245U,

        /// <summary>
        /// User not found
        /// </summary>
        NERR_UserNotFound = 2221U,

        /// <summary>
        /// Access denied
        /// </summary>
        ERROR_ACCESS_DENIED = 5U,

        /// <summary>
        /// Out of memory
        /// </summary>
        ERROR_NOT_ENOUGH_MEMORY = 8U,

        /// <summary>
        /// Invalid parameters
        /// </summary>
        ERROR_INVALID_PARAMETER = 87U,

        /// <summary>
        /// Invalid name
        /// </summary>
        ERROR_INVALID_NAME = 123U,

        /// <summary>
        /// Invalid level
        /// </summary>
        ERROR_INVALID_LEVEL = 124U,

        /// <summary>
        /// Session credential conflict
        /// </summary>
        ERROR_SESSION_CREDENTIAL_CONFLICT = 1219U
    }
}
