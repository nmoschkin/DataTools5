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
    // SetVirtualDiskInformation
    //

    // Version definitions
    public enum SET_VIRTUAL_DISK_INFO_VERSION
    {
        SET_VIRTUAL_DISK_INFO_UNSPECIFIED = 0,
        SET_VIRTUAL_DISK_INFO_PARENT_PATH = 1,
        SET_VIRTUAL_DISK_INFO_IDENTIFIER = 2,
        SET_VIRTUAL_DISK_INFO_PARENT_PATH_WITH_DEPTH = 3,
        SET_VIRTUAL_DISK_INFO_PHYSICAL_SECTOR_SIZE = 4,
        SET_VIRTUAL_DISK_INFO_VIRTUAL_DISK_ID = 5
    }
}
