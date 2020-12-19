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
// Licensed Under the MIT License   
// ************************************************* ''


using System;
using System.ComponentModel;
using System.Net;
using System.Runtime.InteropServices;

using DataTools.Win32;

namespace DataTools.Win32.Network
{
    
    
    /// <summary>
    /// Represents an IPv4 socket address.
    /// </summary>
    /// <remarks></remarks>
    [StructLayout(LayoutKind.Sequential)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public struct SOCKADDR
    {
        /// <summary>
        /// Address family.
        /// </summary>
        /// <remarks></remarks>
        public AddressFamily AddressFamily;

        /// <summary>
        /// Address port.
        /// </summary>
        /// <remarks></remarks>
        public ushort Port;

        /// <summary>
        /// Address data.
        /// </summary>
        /// <remarks></remarks>
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 4)]
        public byte[] Data;

        /// <summary>
        /// Reserved, must be zero.
        /// </summary>
        /// <remarks></remarks>
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 8)]
        public byte[] Zero;

        /// <summary>
        /// Gets the IP address for this structure from the Data buffer.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public IPAddress Address
        {
            get
            {
                return new IPAddress(Data);
            }
        }

        public override string ToString()
        {
            if (Data is null)
                return "NULL";
            return "" + new IPAddress(Data).ToString() + " (" + AddressFamily.ToString() + ")";
        }
    }
}
