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
    // NTDDI_VERSION >= NTDDI_WIN8

    //
    // GetVirtualDiskOperationProgress
    //

    public struct VIRTUAL_DISK_PROGRESS
    {
        public uint OperationStatus;
        public ulong CurrentValue;
        public ulong CompletionValue;
    }
}
