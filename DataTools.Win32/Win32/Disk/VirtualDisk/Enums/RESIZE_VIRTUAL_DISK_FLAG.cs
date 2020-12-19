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
    // Flags for ResizeVirtualDisk
    public enum RESIZE_VIRTUAL_DISK_FLAG
    {
        RESIZE_VIRTUAL_DISK_FLAG_NONE = 0,

        // If this flag is set, skip checking the virtual disk's partition table
        // to ensure that this truncation is safe. Setting this flag can cause
        // unrecoverable data loss; use with care.
        RESIZE_VIRTUAL_DISK_FLAG_ALLOW_UNSAFE_VIRTUAL_SIZE = 1,

        // If this flag is set, resize the disk to the smallest virtual size
        // possible without truncating past any existing partitions. If this
        // is set, NewSize in RESIZE_VIRTUAL_DISK_PARAMETERS must be zero.
        RESIZE_VIRTUAL_DISK_FLAG_RESIZE_TO_SMALLEST_SAFE_VIRTUAL_SIZE = 2
    }
}
