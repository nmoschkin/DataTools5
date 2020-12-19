// ************************************************* ''
// DataTools C# Native Utility Library For Windows - Interop
//
// Module: VDiskDecl
//         Port of virtdisk.h (in its entirety)
//
// Copyright (C) 2011-2020 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the MIT License   
// ************************************************* ''


using System;
using System.Runtime.InteropServices;


namespace DataTools.Win32.Disk.VirtualDisk
{
    // Flags for GetStorageDependencyInformation
    [Flags()]
    public enum GET_STORAGE_DEPENDENCY_FLAG
    {
        GET_STORAGE_DEPENDENCY_FLAG_NONE = 0,

        // Return information for volumes or disks hosting the volume specified
        // If not set, returns info about volumes or disks being hosted by
        // the volume or disk specified
        GET_STORAGE_DEPENDENCY_FLAG_HOST_VOLUMES = 1,

        //  The handle provided is to a disk, not volume or file
        GET_STORAGE_DEPENDENCY_FLAG_DISK_HANDLE = 2
    }
}
