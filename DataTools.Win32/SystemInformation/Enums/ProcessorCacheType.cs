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
// Licensed Under the MIT License   
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

using DataTools.Text;

namespace DataTools.SystemInformation
{
    /// <summary>
    /// Processor cache type
    /// </summary>
    public enum ProcessorCacheType
    {

        /// <summary>
        /// Unified
        /// </summary>
        CacheUnified,

        /// <summary>
        /// Instruction
        /// </summary>
        CacheInstruction,

        /// <summary>
        /// Data
        /// </summary>
        CacheData,

        /// <summary>
        /// Trace
        /// </summary>
        CacheTrace
    }
}
