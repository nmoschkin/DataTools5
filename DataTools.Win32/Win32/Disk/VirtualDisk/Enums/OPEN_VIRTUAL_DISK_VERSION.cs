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
    public enum OPEN_VIRTUAL_DISK_VERSION
    {
        OPEN_VIRTUAL_DISK_VERSION_UNSPECIFIED = 0,
        OPEN_VIRTUAL_DISK_VERSION_1 = 1,
        OPEN_VIRTUAL_DISK_VERSION_2 = 2
    }
}
