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
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct STORAGE_DEPENDENCY_INFO_V2
    {
        public STORAGE_DEPENDENCY_INFO_VERSION Version;
        public uint NumberEntries;
        [MarshalAs(UnmanagedType.Struct)]
        public STORAGE_DEPENDENCY_INFO_TYPE_2 Version2Entries;
    }
}
