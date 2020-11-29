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
    // Parameter structure for GetStorageDependencyInformation
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
    public struct STORAGE_DEPENDENCY_INFO_TYPE_2
    {
        public DEPENDENT_DISK_FLAG DependencyTypeFlags;
        public uint ProviderSpecificFlags;
        [MarshalAs(UnmanagedType.Struct)]
        public VIRTUAL_STORAGE_TYPE VirtualStorageType;
        public uint AncestorLevel;
        [MarshalAs(UnmanagedType.Struct)]
        public MemPtr DependencyDeviceName;
        [MarshalAs(UnmanagedType.Struct)]
        public MemPtr HostVolumeName;
        [MarshalAs(UnmanagedType.Struct)]
        public MemPtr DependentVolumeName;
        [MarshalAs(UnmanagedType.Struct)]
        public MemPtr DependentVolumeRelativePath;
    }
}
