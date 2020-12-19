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
    // END NTDDI_VERSION >= NTDDI_WIN8

    // END VIRTDISK_DEFINE_FLAGS

    internal static class VirtDisk
    {


        public static readonly Guid VIRTUAL_STORAGE_TYPE_VENDOR_UNKNOWN = Guid.Empty;
        public static readonly Guid VIRTUAL_STORAGE_TYPE_VENDOR_MICROSOFT = new Guid("EC984AEC-A0F9-47e9-901F-71415A66345B");

        public const int VIRTUAL_STORAGE_TYPE_DEVICE_UNKNOWN = 0;
        public const int VIRTUAL_STORAGE_TYPE_DEVICE_ISO = 1;
        public const int VIRTUAL_STORAGE_TYPE_DEVICE_VHD = 2;
        public const int VIRTUAL_STORAGE_TYPE_DEVICE_VHDX = 3;

        // Functions
        [DllImport("VirtDisk.dll")]
        public static extern uint CreateVirtualDisk(VIRTUAL_STORAGE_TYPE VirtualStorageType, [MarshalAs(UnmanagedType.LPWStr)] string Path, VIRTUAL_DISK_ACCESS_MASK VirtualDiskAccessMask, SecurityDescriptor.SECURITY_DESCRIPTOR SecurityDescriptor, CREATE_VIRTUAL_DISK_FLAGS Flags, uint ProviderSpecificFlags, CREATE_VIRTUAL_DISK_PARAMETERS_V1 Parameters, IntPtr Overlapped, ref IntPtr Handle);
        [DllImport("VirtDisk.dll")]
        public static extern uint CreateVirtualDisk(VIRTUAL_STORAGE_TYPE VirtualStorageType, [MarshalAs(UnmanagedType.LPWStr)] string Path, VIRTUAL_DISK_ACCESS_MASK VirtualDiskAccessMask, SecurityDescriptor.SECURITY_DESCRIPTOR SecurityDescriptor, CREATE_VIRTUAL_DISK_FLAGS Flags, uint ProviderSpecificFlags, CREATE_VIRTUAL_DISK_PARAMETERS_V2 Parameters, IntPtr Overlapped, ref IntPtr Handle);
        [DllImport("VirtDisk.dll")]
        public static extern uint CreateVirtualDisk(VIRTUAL_STORAGE_TYPE VirtualStorageType, [MarshalAs(UnmanagedType.LPWStr)] string Path, VIRTUAL_DISK_ACCESS_MASK VirtualDiskAccessMask, IntPtr SecurityDescriptor, CREATE_VIRTUAL_DISK_FLAGS Flags, uint ProviderSpecificFlags, CREATE_VIRTUAL_DISK_PARAMETERS_V1 Parameters, IntPtr Overlapped, ref IntPtr Handle);
        [DllImport("VirtDisk.dll")]
        public static extern uint CreateVirtualDisk(VIRTUAL_STORAGE_TYPE VirtualStorageType, [MarshalAs(UnmanagedType.LPWStr)] string Path, VIRTUAL_DISK_ACCESS_MASK VirtualDiskAccessMask, IntPtr SecurityDescriptor, CREATE_VIRTUAL_DISK_FLAGS Flags, uint ProviderSpecificFlags, CREATE_VIRTUAL_DISK_PARAMETERS_V2 Parameters, IntPtr Overlapped, ref IntPtr Handle);
        [DllImport("VirtDisk.dll")]
        public static extern uint OpenVirtualDisk(VIRTUAL_STORAGE_TYPE VirtualStorageType, IntPtr Path, VIRTUAL_DISK_ACCESS_MASK VirtualDiskAccessMask, OPEN_VIRTUAL_DISK_FLAG Flags, OPEN_VIRTUAL_DISK_PARAMETERS_V1 Paramaters, ref IntPtr Handle);
        [DllImport("VirtDisk.dll")]
        public static extern uint OpenVirtualDisk(VIRTUAL_STORAGE_TYPE VirtualStorageType, [MarshalAs(UnmanagedType.LPWStr)] string Path, VIRTUAL_DISK_ACCESS_MASK VirtualDiskAccessMask, OPEN_VIRTUAL_DISK_FLAG Flags, OPEN_VIRTUAL_DISK_PARAMETERS_V1 Paramaters, ref IntPtr Handle);
        [DllImport("VirtDisk.dll")]
        public static extern uint OpenVirtualDisk(VIRTUAL_STORAGE_TYPE VirtualStorageType, [MarshalAs(UnmanagedType.LPWStr)] string Path, VIRTUAL_DISK_ACCESS_MASK VirtualDiskAccessMask, OPEN_VIRTUAL_DISK_FLAG Flags, OPEN_VIRTUAL_DISK_PARAMETERS_V1 Paramaters, ref long Handle);
        [DllImport("VirtDisk.dll", EntryPoint = "OpenVirtualDisk")]

        public static extern uint OpenVirtualDisk(VIRTUAL_STORAGE_TYPE VirtualStorageType, [MarshalAs(UnmanagedType.LPWStr)] string Path, VIRTUAL_DISK_ACCESS_MASK VirtualDiskAccessMask, OPEN_VIRTUAL_DISK_FLAG Flags, OPEN_VIRTUAL_DISK_PARAMETERS_V2 Paramaters, ref IntPtr Handle);

        //
        // This value causes the implementation defaults to be used for block size:
        //
        // Fixed VHDs: 0 is the only valid value since block size is N/A.
        // Dynamic VHDs: The default block size will be used (2 MB, subject to change).
        // Differencing VHDs: 0 causes the parent VHD's block size to be used.
        //
        public const int CREATE_VIRTUAL_DISK_PARAMETERS_DEFAULT_BLOCK_SIZE = 0;

        // Default logical sector size is 512B
        public const int CREATE_VIRTUAL_DISK_PARAMETERS_DEFAULT_SECTOR_SIZE = 0;



        [DllImport("VirtDisk.dll")]
        public static extern uint AttachVirtualDisk(IntPtr Handle, IntPtr SecurityDescriptor, ATTACH_VIRTUAL_DISK_FLAG Flags, uint ProviderSpecificFlags, IntPtr Parameters, IntPtr Overlapped);
        [DllImport("VirtDisk.dll")]
        public static extern uint AttachVirtualDisk(IntPtr Handle, SecurityDescriptor.SECURITY_DESCRIPTOR SecurityDescriptor, ATTACH_VIRTUAL_DISK_FLAG Flags, uint ProviderSpecificFlags, ATTACH_VIRTUAL_DISK_PARAMETERS Parameters, IntPtr Overlapped);

        [DllImport("VirtDisk.dll")]
        public static extern uint DetachVirtualDisk(IntPtr Handle, DETACH_VIRTUAL_DISK_FLAG Flag, uint ProviderSpecificFlags);

        //
        // GetVirtualDiskPhysicalPath
        //

        [DllImport("VirtDisk.dll")]
        public static extern uint GetVirtualDiskPhysicalPath(IntPtr Handle, ref uint DiskPathSizeInBytes, [MarshalAs(UnmanagedType.LPWStr)] ref string DiskPath);
        [DllImport("VirtDisk.dll")]
        public static extern uint GetVirtualDiskPhysicalPath(IntPtr Handle, ref uint DiskPathSizeInBytes, byte[] DiskPath);
        [DllImport("VirtDisk.dll")]
        public static extern uint GetVirtualDiskPhysicalPath(IntPtr Handle, ref uint DiskPathSizeInBytes, IntPtr DiskPath);
        //
        // GetAllAttachedVirtualDiskPhysicalPaths
        //

        [DllImport("VirtDisk.dll")]
        public static extern uint GetAllAttachedVirtualDiskPhysicalPaths(ref uint PathsBufferSizeInBytes, [MarshalAs(UnmanagedType.LPWStr)] string PathsBuffer);

        [DllImport("VirtDisk.dll")]
        public static extern uint GetStorageDependencyInformation(IntPtr Handle, GET_STORAGE_DEPENDENCY_FLAG Flags, uint StorageDependencyInfoSize, IntPtr StorageDependencyInfo, ref uint SizeUsed);

        [DllImport("VirtDisk.dll")]
        public static extern uint GetVirtualDiskInformation(IntPtr Handle, ref uint VirtualDiskInfoSize, GET_VIRTUAL_DISK_INFO_SIZE VirtualDiskInfo, ref uint SizeUsed);

        [DllImport("VirtDisk.dll")]
        public static extern uint GetVirtualDiskInformation(IntPtr Handle, ref uint VirtualDiskInfoSize, GET_VIRTUAL_DISK_INFO_IDENTIFIER VirtualDiskInfo, ref uint SizeUsed);

        [DllImport("VirtDisk.dll")]
        public static extern uint GetVirtualDiskInformation(IntPtr Handle, ref uint VirtualDiskInfoSize, GET_VIRTUAL_DISK_INFO_PARENT_LOCATION VirtualDiskInfo, ref uint SizeUsed);

        // Since I foresee problems with the above declare (seems useless) this one will be the catch-all
        [DllImport("VirtDisk.dll")]
        public static extern uint GetVirtualDiskInformation(IntPtr Handle, ref uint VirtualDiskInfoSize, IntPtr VirtualDiskInfo, ref uint SizeUsed);



        [DllImport("VirtDisk.dll")]
        public static extern uint GetVirtualDiskInformation(IntPtr Handle, ref uint VirtualDiskInfoSize, GET_VIRTUAL_DISK_INFO_PARENT_IDENTIFIER VirtualDiskInfo, ref uint SizeUsed);


        [DllImport("VirtDisk.dll")]
        public static extern uint GetVirtualDiskInformation(IntPtr Handle, ref uint VirtualDiskInfoSize, GET_VIRTUAL_DISK_INFO_PARENT_TIMESTAMP VirtualDiskInfo, ref uint SizeUsed);

        [DllImport("VirtDisk.dll")]
        public static extern uint GetVirtualDiskInformation(IntPtr Handle, ref uint VirtualDiskInfoSize, GET_VIRTUAL_DISK_INFO_VIRTUAL_STORAGE_TYPE VirtualDiskInfo, ref uint SizeUsed);

        [DllImport("VirtDisk.dll")]
        public static extern uint GetVirtualDiskInformation(IntPtr Handle, ref uint VirtualDiskInfoSize, GET_VIRTUAL_DISK_INFO_PROVIDER_SUBTYPE VirtualDiskInfo, ref uint SizeUsed);

        [DllImport("VirtDisk.dll")]
        public static extern uint GetVirtualDiskInformation(IntPtr Handle, ref uint VirtualDiskInfoSize, GET_VIRTUAL_DISK_INFO_IS4KALIGNED VirtualDiskInfo, ref uint SizeUsed);

        [DllImport("VirtDisk.dll")]
        public static extern uint GetVirtualDiskInformation(IntPtr Handle, ref uint VirtualDiskInfoSize, GET_VIRTUAL_DISK_INFO_ISLOADED VirtualDiskInfo, ref uint SizeUsed);

        //

        //
        //
        [DllImport("VirtDisk.dll")]
        public static extern uint GetVirtualDiskInformation(IntPtr Handle, ref uint VirtualDiskInfoSize, GET_VIRTUAL_DISK_INFO_LOGICAL_SECTOR_SIZE VirtualDiskInfo, ref uint SizeUsed);
        [DllImport("VirtDisk.dll")]
        public static extern uint GetVirtualDiskInformation(IntPtr Handle, ref uint VirtualDiskInfoSize, GET_VIRTUAL_DISK_INFO_VHD_PHYSICAL_SECTOR_SIZE VirtualDiskInfo, ref uint SizeUsed);
        [DllImport("VirtDisk.dll")]
        public static extern uint GetVirtualDiskInformation(IntPtr Handle, ref uint VirtualDiskInfoSize, GET_VIRTUAL_DISK_INFO_SMALLEST_SAFE_VIRTUAL_SIZE VirtualDiskInfo, ref uint SizeUsed);
        [DllImport("VirtDisk.dll")]
        public static extern uint GetVirtualDiskInformation(IntPtr Handle, ref uint VirtualDiskInfoSize, GET_VIRTUAL_DISK_INFO_FRAGMENTATION VirtualDiskInfo, ref uint SizeUsed);
        [DllImport("VirtDisk.dll")]
        public static extern uint GetVirtualDiskInformation(IntPtr Handle, ref uint VirtualDiskInfoSize, GET_VIRTUAL_DISK_INFO_VIRTUAL_DISK_ID VirtualDiskInfo, ref uint SizeUsed);
        [DllImport("VirtDisk.dll")]
        public static extern uint SetVirtualDiskInformation(IntPtr VirtualDiskHandle, SET_VIRTUAL_DISK_INFO_PARENT_FILE_PATH VirtualDiskInfo);
        [DllImport("VirtDisk.dll")]
        public static extern uint SetVirtualDiskInformation(IntPtr VirtualDiskHandle, SET_VIRTUAL_DISK_INFO_UNIQUE_ID VirtualDiskInfo);

        [DllImport("VirtDisk.dll")]
        public static extern uint SetVirtualDiskInformation(IntPtr VirtualDiskHandle, SET_VIRTUAL_DISK_INFO_PARENT_PATH_WITH_DEPTH_INFO VirtualDiskInfo);

        [DllImport("VirtDisk.dll")]
        public static extern uint SetVirtualDiskInformation(IntPtr VirtualDiskHandle, SET_VIRTUAL_DISK_INFO_VHD_SECTOR_SIZE VirtualDiskInfo);

        // Points to as list of Guids (in Items)
        [DllImport("VirtDisk.dll")]
        public static extern uint EnumerateVirtualDiskMetadata(IntPtr VirtualDiskHandle, ref uint NumberOfItems, IntPtr Items);
        [DllImport("VirtDisk.dll")]
        public static extern uint GetVirtualDiskMetadata(IntPtr VirtualDiskHandle, Guid Item, ref uint MetaDataSize, IntPtr MetaData);
        [DllImport("VirtDisk.dll")]
        public static extern uint SetVirtualDiskMetadata(IntPtr VirtualDiskHandle, Guid Item, uint MetaDataSize, IntPtr MetaData);
        [DllImport("VirtDisk.dll")]
        public static extern uint DeleteVirtualDiskMetadata(IntPtr VirtualDiskHandle, Guid Item);




        [DllImport("VirtDisk.dll")]
        public static extern uint GetVirtualDiskOperationProgress(IntPtr VirtualDiskHandle, IntPtr Overlapped, VIRTUAL_DISK_PROGRESS Progress);
        [DllImport("VirtDisk.dll")]
        public static extern uint CompactVirtualDisk(IntPtr VirtualDiskHandle, COMPACT_VIRTUAL_DISK_FLAG Flags, COMPACT_VIRTUAL_DISK_PARAMETERS Parameters, IntPtr Overlapped);
        // Versioned parameter structure for MergeVirtualDisk
        public const int MERGE_VIRTUAL_DISK_DEFAULT_MERGE_DEPTH = 1;
        [DllImport("VirtDisk.dll")]
        public static extern uint MergeVirtualDisk(IntPtr VirtualDiskHandle, MERGE_VIRTUAL_DISK_FLAG Flags, MERGE_VIRTUAL_DISK_PARAMETERS_V2 Parameters, IntPtr Overlapped);
        [DllImport("VirtDisk.dll")]
        public static extern uint ExpandVirtualDisk(IntPtr VirtualDiskHandle, EXPAND_VIRTUAL_DISK_FLAG Flags, EXPAND_VIRTUAL_DISK_PARAMETERS Parameters, IntPtr Overlapped);
        [DllImport("VirtDisk.dll")]
        public static extern uint ResizeVirtualDisk(IntPtr VirtualDiskHandle, RESIZE_VIRTUAL_DISK_FLAG Flags, RESIZE_VIRTUAL_DISK_PARAMETERS Parameters, IntPtr Overlapped);
        [DllImport("VirtDisk.dll")]
        public static extern uint MirrorVirtualDisk(IntPtr VirtualDiskHandle, MIRROR_VIRTUAL_DISK_FLAG Flags, MIRROR_VIRTUAL_DISK_PARAMETERS Parameters, IntPtr Overlapped);

        // NTDDI_VERSION >= NTDDI_WIN8

        //
        // BreakMirrorVirtualDisk
        //

        [DllImport("VirtDisk.dll")]
        public static extern uint BreakMirrorVirtualDisk(IntPtr VirtualDiskHandle);

        // NTDDI_VERSION >= NTDDI_WIN8

        //
        // AddVirtualDiskParent
        //

        [DllImport("VirtDisk.dll")]
        public static extern uint AddVirtualDiskParent(IntPtr VirtualDiskHandle, [MarshalAs(UnmanagedType.LPWStr)] string ParentPath);

    }
}
