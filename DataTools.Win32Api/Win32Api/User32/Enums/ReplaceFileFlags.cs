// ************************************************* ''
// DataTools C# Native Utility Library For Windows - Interop
//
// Module: Native
//         Myriad Windows API Declares
//
// Started in 2000 on Windows 98/ME (and then later 2000).
//
// Still kicking in 2014 on Windows 8.1!
// A whole bunch of pInvoke/Const/Declare/Struct and associated utility functions that have been collected over the years.

// Some enum documentation copied from the MSDN (and in some cases, updated).
// 
// Copyright (C) 2011-2020 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the Microsoft Public License   
// ************************************************* ''



using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using DataTools.Memory;
using DataTools.Win32Api;

namespace DataTools.Win32Api
{
    public enum ReplaceFileFlags : uint
    {

        /// <summary>
        /// This value Is Not supported.
        /// </summary>
        REPLACEFILE_WRITE_THROUGH = 0x1U,

        /// <summary>
        /// Ignores errors that occur While merging information (such As attributes And ACLs) from the replaced file To the replacement file. Therefore, If you specify this flag And Do Not have WRITE_DAC access, the Function succeeds but the ACLs are Not preserved.
        /// </summary>
        REPLACEFILE_IGNORE_MERGE_ERRORS = 0x2U,

        /// <summary>
        /// Ignores errors that occur while merging ACL information from the replaced file to the replacement file. Therefore, if you specify this flag And do Not have WRITE_DAC access, the function succeeds but the ACLs are Not preserved. To compile an application that uses this value, define the _WIN32_WINNT macro as 0x0600 Or later.
        /// 
        /// Windows Server 2003 And Windows XP:This value Is Not supported.
        /// </summary>
        REPLACEFILE_IGNORE_ACL_ERRORS = 0x4U
    }
}
