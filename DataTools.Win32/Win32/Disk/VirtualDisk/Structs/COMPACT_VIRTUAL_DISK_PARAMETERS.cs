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
    // Versioned structure for CompactVirtualDisk
    public struct COMPACT_VIRTUAL_DISK_PARAMETERS
    {
        public COMPACT_VIRTUAL_DISK_VERSION Version;
        public uint Reserved;
    }
}
