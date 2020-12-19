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
    // Flags for MirrorVirtualDisk
    public enum MIRROR_VIRTUAL_DISK_FLAG
    {
        MIRROR_VIRTUAL_DISK_FLAG_NONE = 0,
        MIRROR_VIRTUAL_DISK_FLAG_EXISTING_FILE = 1
    }
}
