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
    /// <summary>
    /// Only supported on Windows 7 and greater
    /// </summary>
    /// <remarks></remarks>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct CREATE_VIRTUAL_DISK_PARAMETERS_V1
    {
        public CREATE_VIRTUAL_DISK_VERSION Version;
        [MarshalAs(UnmanagedType.Struct)]
        public Guid UniqueId;
        public ulong MaximumSize;
        public uint BlockSizeInBytes;
        public int SectorSizeInBytes;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string ParentPath;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string SourcePath;
    }
}
