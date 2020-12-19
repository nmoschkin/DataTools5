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
    [Flags()]
    public enum OPEN_VIRTUAL_DISK_FLAG
    {
        OPEN_VIRTUAL_DISK_FLAG_NONE = 0x0,
        // Open the backing store without opening any differencing chain parents.
        // This allows one to fixup broken parent links.
        OPEN_VIRTUAL_DISK_FLAG_NO_PARENTS = 0x1,

        // The backing store being opened is an empty file. Do not perform virtual
        // disk verification.
        OPEN_VIRTUAL_DISK_FLAG_BLANK_FILE = 0x2,

        // This flag is only specified at boot time to load the system disk
        // during virtual disk boot.  Must be kernel mode to specify this flag.
        OPEN_VIRTUAL_DISK_FLAG_BOOT_DRIVE = 0x4,

        // This flag causes the backing file to be opened in cached mode.
        OPEN_VIRTUAL_DISK_FLAG_CACHED_IO = 0x8,

        // Open the backing store without opening any differencing chain parents.
        // This allows one to fixup broken parent links temporarily without updating
        // the parent locator.
        OPEN_VIRTUAL_DISK_FLAG_CUSTOM_DIFF_CHAIN = 0x10,

        // This flag causes all backing stores except the leaf backing store to
        // be opened in cached mode.
        OPEN_VIRTUAL_DISK_FLAG_PARENT_CACHED_IO = 0x20
    }
}
