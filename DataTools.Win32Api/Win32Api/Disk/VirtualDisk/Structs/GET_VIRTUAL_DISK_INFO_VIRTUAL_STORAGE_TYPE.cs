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
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct GET_VIRTUAL_DISK_INFO_VIRTUAL_STORAGE_TYPE
    {
        public GET_VIRTUAL_DISK_INFO_VERSION Version;
        public VIRTUAL_STORAGE_TYPE VirtualStorageType;
    }
}
