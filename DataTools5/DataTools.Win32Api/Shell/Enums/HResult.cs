// ************************************************* ''
// DataTools C# Native Utility Library For Windows - Interop
//
// Module: NativeShell
//         Wrappers for native and COM shell interfaces.
//
// Some enum documentation copied from the MSDN (and in some cases, updated).
// Some classes and interfaces were ported from the WindowsAPICodePack.
// 
// Copyright (C) 2011-2020 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the Microsoft Public License   
// ************************************************* ''

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

//using DataTools.Hardware.MessageResources;
//using DataTools.Hardware;
//using DataTools.Hardware.Native;

namespace DataTools.Shell.Native
{
    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */

    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    public enum HResult : uint
    {
        /// <summary>
        /// S_OK
        /// </summary>
        Ok = 0x0,

        /// <summary>
        /// S_FALSE
        /// </summary>
        False = 0x1,

        /// <summary>
        /// E_INVALIDARG
        /// </summary>
        InvalidArguments = 0x80070057,

        /// <summary>
        /// E_OUTOFMEMORY
        /// </summary>
        OutOfMemory = 0x8007000E,

        /// <summary>
        /// E_NOINTERFACE
        /// </summary>
        NoInterface = 0x80004002,

        /// <summary>
        /// E_FAIL
        /// </summary>
        Fail = 0x80004005,

        /// <summary>
        /// E_ELEMENTNOTFOUND
        /// </summary>
        ElementNotFound = 0x80070490,

        /// <summary>
        /// TYPE_E_ELEMENTNOTFOUND
        /// </summary>
        TypeElementNotFound = 0x8002802B,

        /// <summary>
        /// NO_OBJECT
        /// </summary>
        NoObject = 0x800401E5,

        /// <summary>
        /// Win32 Error code: ERROR_CANCELLED
        /// </summary>
        Win32ErrorCanceled = 1223,

        /// <summary>
        /// ERROR_CANCELLED
        /// </summary>
        Canceled = 0x800704C7,

        /// <summary>
        /// The requested resource is in use
        /// </summary>
        ResourceInUse = 0x800700AA,

        /// <summary>
        /// The requested resource is read-only.
        /// </summary>
        AccessDenied = 0x80030005
    }
}
