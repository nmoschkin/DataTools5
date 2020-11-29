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
    //  Access Mask for OpenVirtualDisk and CreateVirtualDisk.  The virtual
    //  disk drivers expose file objects as handles therefore we map
    //  it into that AccessMask space.
    [Flags()]
    public enum VIRTUAL_DISK_ACCESS_MASK
    {
        VIRTUAL_DISK_ACCESS_NONE = 0x0,
        VIRTUAL_DISK_ACCESS_ATTACH_RO = 0x10000,
        VIRTUAL_DISK_ACCESS_ATTACH_RW = 0x20000,
        VIRTUAL_DISK_ACCESS_DETACH = 0x40000,
        VIRTUAL_DISK_ACCESS_GET_INFO = 0x80000,
        VIRTUAL_DISK_ACCESS_CREATE = 0x100000,
        VIRTUAL_DISK_ACCESS_METAOPS = 0x200000,
        VIRTUAL_DISK_ACCESS_READ = 0xD0000,
        VIRTUAL_DISK_ACCESS_ALL = 0x3F0000,
        //
        //
        // A special flag to be used to test if the virtual disk needs to be
        // opened for write.
        //
        VIRTUAL_DISK_ACCESS_WRITABLE = 0x320000
    }
}
