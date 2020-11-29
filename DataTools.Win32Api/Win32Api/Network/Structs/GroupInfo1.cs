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
// Licensed Under the Microsoft Public License   
// ************************************************* ''


using System;
using System.Runtime.InteropServices;
using DataTools.Memory;
using DataTools.Text;
using DataTools.Win32Api;

namespace DataTools.Win32Api.Network
{
    /// <summary>
    /// Windows API GROUP_INFO_1 structure.  Basic group information and attributes.
    /// </summary>
    /// <remarks></remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct GroupInfo1
    {

        /// <summary>
        /// Group name
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Name;

        /// <summary>
        /// Comments
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Comment;

        /// <summary>
        /// Group Id
        /// </summary>
        public IntPtr GroupId;

        /// <summary>
        /// Attributes
        /// </summary>
        public int Attributes;

        public override string ToString()
        {
            return Name;
        }
    }
}
