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
    /// MEMORYSTATUSEX structure
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MemoryStatusDetails
    {

        /// <summary>
        /// Length of this structure
        /// </summary>
        internal int dwLength;

        /// <summary>
        /// Memory load
        /// </summary>
        public FriendlySizeInteger MemoryLoad;

        /// <summary>
        /// Total physical memory on the machine
        /// </summary>
        public FriendlySizeLong TotalPhysicalMemory;

        /// <summary>
        /// Total available memroy on the machine
        /// </summary>
        public FriendlySizeLong AvailPhysicalMemory;

        /// <summary>
        /// Total paging file capacity
        /// </summary>
        public FriendlySizeLong TotalPageFile;

        /// <summary>
        /// Available paging file capacity
        /// </summary>
        public FriendlySizeLong AvailPageFile;

        /// <summary>
        /// Total virtual memory
        /// </summary>
        public FriendlySizeLong TotalVirtualMemory;

        /// <summary>
        /// Available virtual memory
        /// </summary>
        public FriendlySizeLong AvailVirtualMemory;

        /// <summary>
        /// Available extended virtual memory
        /// </summary>
        public FriendlySizeLong AvailExtendedVirtualMemory;
    }
}
