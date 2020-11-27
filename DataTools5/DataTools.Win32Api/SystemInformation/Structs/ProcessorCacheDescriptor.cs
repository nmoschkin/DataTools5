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
    /// Cache descriptor
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ProcessorCacheDescriptor
    {
        /// <summary>
        /// Level
        /// </summary>
        public byte Level;

        /// <summary>
        /// Associativity
        /// </summary>
        public byte Associativity;

        /// <summary>
        /// Line size
        /// </summary>
        public short LineSize;

        /// <summary>
        /// Size
        /// </summary>
        public int Size;

        /// <summary>
        /// Type
        /// </summary>
        public ProcessorCacheType Type;

        public override string ToString()
        {
            string ct = Type.ToString().Replace("Cache", "");

            string s = $"L{Level} {ct} Cache, {TextTools.PrintFriendlySize(Size)}, Line Size {TextTools.PrintFriendlySize(LineSize)}";
            if (Associativity == 0xff) s += ", Fully Associative";

            return s;
        }
    }
}
