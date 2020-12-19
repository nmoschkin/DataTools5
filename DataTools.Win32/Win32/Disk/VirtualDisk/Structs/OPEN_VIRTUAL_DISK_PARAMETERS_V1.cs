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
    // Versioned OpenVirtualDisk parameter structure
    public struct OPEN_VIRTUAL_DISK_PARAMETERS_V1
    {
        public OPEN_VIRTUAL_DISK_VERSION Version;
        public uint RWDepth;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 20)]
        public byte[] res;
    }
}
