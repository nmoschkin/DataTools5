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
    //
    // MergeVirtualDisk
    //

    // Version definitions
    public enum MERGE_VIRTUAL_DISK_VERSION
    {
        MERGE_VIRTUAL_DISK_VERSION_UNSPECIFIED = 0,
        MERGE_VIRTUAL_DISK_VERSION_1 = 1,
        MERGE_VIRTUAL_DISK_VERSION_2 = 2
    }
}
