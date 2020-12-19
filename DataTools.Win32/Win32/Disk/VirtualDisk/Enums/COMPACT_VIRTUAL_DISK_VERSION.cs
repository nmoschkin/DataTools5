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

namespace DataTools.Win32.Disk.VirtualDisk
{
    //
    // CompactVirtualDisk
    //

    // Version definitions
    public enum COMPACT_VIRTUAL_DISK_VERSION
    {
        COMPACT_VIRTUAL_DISK_VERSION_UNSPECIFIED = 0,
        COMPACT_VIRTUAL_DISK_VERSION_1 = 1
    }
}
