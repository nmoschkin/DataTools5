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
    // Parameter structure for GetStorageDependencyInformation
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct STORAGE_DEPENDENCY_INFO_V1
    {
        public STORAGE_DEPENDENCY_INFO_VERSION Version;
        public uint NumberEntries;
        [MarshalAs(UnmanagedType.Struct)]
        public STORAGE_DEPENDENCY_INFO_TYPE_1 Version1Entries;
    }
}
