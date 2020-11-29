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
    // Flags for AttachVirtualDisk
    [Flags()]
    public enum ATTACH_VIRTUAL_DISK_FLAG
    {
        ATTACH_VIRTUAL_DISK_FLAG_NONE = 0x0,

        // Attach the disk as read only
        ATTACH_VIRTUAL_DISK_FLAG_READ_ONLY = 0x1,

        // Will cause all volumes on the disk to be mounted
        // without drive letters.
        ATTACH_VIRTUAL_DISK_FLAG_NO_DRIVE_LETTER = 0x2,

        // Will decouple the disk lifetime from that of the VirtualDiskHandle.
        // The disk will be attached until an explicit call is made to
        // DetachVirtualDisk even if all handles are closed.
        ATTACH_VIRTUAL_DISK_FLAG_PERMANENT_LIFETIME = 0x4,

        // Indicates that the drive will not be attached to
        // the local system (but rather to a VM).
        ATTACH_VIRTUAL_DISK_FLAG_NO_LOCAL_HOST = 0x8
    }
}
