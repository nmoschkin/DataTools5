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
    /// <summary>
    /// IP adapter common structure header.
    /// </summary>
    /// <remarks></remarks>
    [StructLayout(LayoutKind.Sequential)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public struct IP_ADAPTER_HEADER_UNION
    {
        public ulong Alignment;

        /// <summary>
        /// Length of the structure
        /// </summary>
        /// <returns></returns>
        public uint Length
        {
            get
            {
                return (uint)((long)Alignment & 0xFFFFFFFFL);
            }
        }

        /// <summary>
        /// Interface index
        /// </summary>
        /// <returns></returns>
        public uint IfIndex
        {
            get
            {
                return (uint)((long)(Alignment >> 32) & 0xFFFFFFFFL);
            }
        }
    }
}
