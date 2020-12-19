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
    //

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct GET_VIRTUAL_DISK_INFO_VIRTUAL_DISK_ID
    {
        public GET_VIRTUAL_DISK_INFO_VERSION Version;
        [MarshalAs(UnmanagedType.Struct)]
        public Guid VirtualDiskId;
    }
}
