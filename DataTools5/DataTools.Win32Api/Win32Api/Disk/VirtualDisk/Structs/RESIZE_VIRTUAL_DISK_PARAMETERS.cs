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
    // Versioned parameter structure for ResizeVirtualDisk
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct RESIZE_VIRTUAL_DISK_PARAMETERS
    {
        public RESIZE_VIRTUAL_DISK_VERSION Version;
        [MarshalAs(UnmanagedType.LPWStr)]
        public ulong NewSize;
    }
}
