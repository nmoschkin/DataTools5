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
// Licensed Under the Microsoft Public License   
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
using DataTools.Memory;
using DataTools.Text;

namespace DataTools.SystemInformation
{
    /// <summary>
    /// Logical processor relationship
    /// </summary>
    public enum LogicalProcessorRelationship
    {

        /// <summary>
        /// Processor core
        /// </summary>
        RelationProcessorCore,


        /// <summary>
        /// Numa Node
        /// </summary>
        RelationNumaNode,

        /// <summary>
        /// Cache
        /// </summary>
        RelationCache,

        /// <summary>
        /// Processor Package
        /// </summary>
        RelationProcessorPackage,

        /// <summary>
        /// Processor Group
        /// </summary>
        RelationGroup,

        /// <summary>
        /// All
        /// </summary>
        RelationAll = 0xFFFF
    }
}
