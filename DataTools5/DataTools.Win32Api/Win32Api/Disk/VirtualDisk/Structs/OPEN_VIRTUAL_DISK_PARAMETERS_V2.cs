// ************************************************* ''
// DataTools C# Native Utility Library For Windows - Interop
//
// Module: VDiskDecl
//         Port of virtdisk.h (in its entirety)
//
// Copyright (C) 2011-2020 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the Microsoft Public License   
// ************************************************* ''


using System;
using System.Runtime.InteropServices;
using DataTools.Memory;

namespace DataTools.Win32Api.Disk.VirtualDisk
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct OPEN_VIRTUAL_DISK_PARAMETERS_V2
    {
        public OPEN_VIRTUAL_DISK_VERSION Version;
        [MarshalAs(UnmanagedType.Bool)]
        public bool GetInfoOnly;
        [MarshalAs(UnmanagedType.Bool)]
        public bool ReadOnly;
        [MarshalAs(UnmanagedType.Struct)]
        public Guid ResiliencyGuid;
    }
}