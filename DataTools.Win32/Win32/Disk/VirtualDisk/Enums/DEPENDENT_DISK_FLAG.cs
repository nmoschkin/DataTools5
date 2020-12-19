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
    // GetStorageDependencyInformation
    //

    // Flags for dependent disks
    [Flags()]
    public enum DEPENDENT_DISK_FLAG
    {
        DEPENDENT_DISK_FLAG_NONE = 0x0,

        //
        // Multiple files backing the virtual storage device
        //
        DEPENDENT_DISK_FLAG_MULT_BACKING_FILES = 0x1,
        DEPENDENT_DISK_FLAG_FULLY_ALLOCATED = 0x2,
        DEPENDENT_DISK_FLAG_READ_ONLY = 0x4,

        //
        //Backing file of the virtual storage device is not local to the machine
        //
        DEPENDENT_DISK_FLAG_REMOTE = 0x8,

        //
        // Volume is the system volume
        //
        DEPENDENT_DISK_FLAG_SYSTEM_VOLUME = 0x10,

        //
        // Volume backing the virtual storage device file is the system volume
        //
        DEPENDENT_DISK_FLAG_SYSTEM_VOLUME_PARENT = 0x20,
        DEPENDENT_DISK_FLAG_REMOVABLE = 0x40,

        //
        // Drive letters are not assigned to the volumes
        // on the virtual disk automatically.
        //
        DEPENDENT_DISK_FLAG_NO_DRIVE_LETTER = 0x80,
        DEPENDENT_DISK_FLAG_PARENT = 0x100,

        //
        // Virtual disk is not attached on the local host
        // (instead attached on a guest VM for instance)
        //
        DEPENDENT_DISK_FLAG_NO_HOST_DISK = 0x200,

        //
        // Indicates the lifetime of the disk is not tied
        // to any system handles
        //
        DEPENDENT_DISK_FLAG_PERMANENT_LIFETIME = 0x400
    }
}
