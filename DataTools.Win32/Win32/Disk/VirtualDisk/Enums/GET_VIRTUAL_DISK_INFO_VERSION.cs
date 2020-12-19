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
    //
    //
    // GetVirtualDiskInformation
    //

    // Version definitions
    public enum GET_VIRTUAL_DISK_INFO_VERSION
    {
        GET_VIRTUAL_DISK_INFO_UNSPECIFIED = 0,
        GET_VIRTUAL_DISK_INFO_SIZE = 1,
        GET_VIRTUAL_DISK_INFO_IDENTIFIER = 2,
        GET_VIRTUAL_DISK_INFO_PARENT_LOCATION = 3,
        GET_VIRTUAL_DISK_INFO_PARENT_IDENTIFIER = 4,
        GET_VIRTUAL_DISK_INFO_PARENT_TIMESTAMP = 5,
        GET_VIRTUAL_DISK_INFO_VIRTUAL_STORAGE_TYPE = 6,
        GET_VIRTUAL_DISK_INFO_PROVIDER_SUBTYPE = 7,
        GET_VIRTUAL_DISK_INFO_IS_4K_ALIGNED = 8,
        GET_VIRTUAL_DISK_INFO_PHYSICAL_DISK = 9,
        GET_VIRTUAL_DISK_INFO_VHD_PHYSICAL_SECTOR_SIZE = 10,
        GET_VIRTUAL_DISK_INFO_SMALLEST_SAFE_VIRTUAL_SIZE = 11,
        GET_VIRTUAL_DISK_INFO_FRAGMENTATION = 12,
        GET_VIRTUAL_DISK_INFO_IS_LOADED = 13,
        GET_VIRTUAL_DISK_INFO_VIRTUAL_DISK_ID = 14
    }
}
