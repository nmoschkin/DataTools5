// ' ************************************************* ''
// ' DataTools Visual Basic Utility Library - Interop
// '
// ' Module: VDiskDecl
// '         Port of virtdisk.h (in its entirety)
// '
// ' Copyright (C) 2011-2020 Nathan Moschkin
// ' All Rights Reserved
// '
// ' Licensed Under the Microsoft Public License   
// ' ************************************************* ''


using System;
using System.Runtime.InteropServices;
using DataTools.Memory;

namespace DataTools.Hardware.Native
{
    internal static class VDiskDecl
    {
        public static readonly Guid VIRTUAL_STORAGE_TYPE_VENDOR_UNKNOWN = Guid.Empty;
        public static readonly Guid VIRTUAL_STORAGE_TYPE_VENDOR_MICROSOFT = new Guid("EC984AEC-A0F9-47e9-901F-71415A66345B");

        public const int VIRTUAL_STORAGE_TYPE_DEVICE_UNKNOWN = 0;
        public const int VIRTUAL_STORAGE_TYPE_DEVICE_ISO = 1;
        public const int VIRTUAL_STORAGE_TYPE_DEVICE_VHD = 2;
        public const int VIRTUAL_STORAGE_TYPE_DEVICE_VHDX = 3;

        public enum CREATE_VIRTUAL_DISK_VERSION
        {
            CREATE_VIRTUAL_DISK_VERSION_UNSPECIFIED = 0,
            CREATE_VIRTUAL_DISK_VERSION_1 = 1,
            CREATE_VIRTUAL_DISK_VERSION_2 = 2
        }

        public enum OPEN_VIRTUAL_DISK_VERSION
        {
            OPEN_VIRTUAL_DISK_VERSION_UNSPECIFIED = 0,
            OPEN_VIRTUAL_DISK_VERSION_1 = 1,
            OPEN_VIRTUAL_DISK_VERSION_2 = 2
        }

        [Flags()]
        public enum OPEN_VIRTUAL_DISK_FLAG
        {
            OPEN_VIRTUAL_DISK_FLAG_NONE = 0x0,
            // ' Open the backing store without opening any differencing chain parents.
            // ' This allows one to fixup broken parent links.
            OPEN_VIRTUAL_DISK_FLAG_NO_PARENTS = 0x1,

            // ' The backing store being opened is an empty file. Do not perform virtual
            // ' disk verification.
            OPEN_VIRTUAL_DISK_FLAG_BLANK_FILE = 0x2,

            // ' This flag is only specified at boot time to load the system disk
            // ' during virtual disk boot.  Must be kernel mode to specify this flag.
            OPEN_VIRTUAL_DISK_FLAG_BOOT_DRIVE = 0x4,

            // ' This flag causes the backing file to be opened in cached mode.
            OPEN_VIRTUAL_DISK_FLAG_CACHED_IO = 0x8,

            // ' Open the backing store without opening any differencing chain parents.
            // ' This allows one to fixup broken parent links temporarily without updating
            // ' the parent locator.
            OPEN_VIRTUAL_DISK_FLAG_CUSTOM_DIFF_CHAIN = 0x10,

            // ' This flag causes all backing stores except the leaf backing store to
            // ' be opened in cached mode.
            OPEN_VIRTUAL_DISK_FLAG_PARENT_CACHED_IO = 0x20
        }

        // '
        // '  Access Mask for OpenVirtualDisk and CreateVirtualDisk.  The virtual
        // '  disk drivers expose file objects as handles therefore we map
        // '  it into that AccessMask space.
        [Flags()]
        public enum VIRTUAL_DISK_ACCESS_MASK
        {
            VIRTUAL_DISK_ACCESS_NONE = 0x0,
            VIRTUAL_DISK_ACCESS_ATTACH_RO = 0x10000,
            VIRTUAL_DISK_ACCESS_ATTACH_RW = 0x20000,
            VIRTUAL_DISK_ACCESS_DETACH = 0x40000,
            VIRTUAL_DISK_ACCESS_GET_INFO = 0x80000,
            VIRTUAL_DISK_ACCESS_CREATE = 0x100000,
            VIRTUAL_DISK_ACCESS_METAOPS = 0x200000,
            VIRTUAL_DISK_ACCESS_READ = 0xD0000,
            VIRTUAL_DISK_ACCESS_ALL = 0x3F0000,
            // '
            // '
            // ' A special flag to be used to test if the virtual disk needs to be
            // ' opened for write.
            // '
            VIRTUAL_DISK_ACCESS_WRITABLE = 0x320000
        }

        [Flags()]
        public enum CREATE_VIRTUAL_DISK_FLAGS
        {
            CREATE_VIRTUAL_DISK_FLAG_NONE = 0x0,
            CREATE_VIRTUAL_DISK_FLAG_FULL_PHYSICAL_ALLOCATION = 0x1,
            CREATE_VIRTUAL_DISK_FLAG_PREVENT_WRITES_TO_SOURCE_DISK = 0x2,
            CREATE_VIRTUAL_DISK_FLAG_DO_NOT_COPY_METADATA_FROM_PARENT = 0x4
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct VIRTUAL_STORAGE_TYPE
        {
            public uint DeviceId;

            // <MarshalAs(UnmanagedType.ByValArray, ArraySubType:=UnmanagedType.U1, SizeConst:=16)> _

            [MarshalAs(UnmanagedType.Struct)]
            public Guid VendorId;
        }

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

        /// <summary>
        /// Only supported for Windows 8/Server 2012 and greater
        /// </summary>
        /// <remarks></remarks>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct CREATE_VIRTUAL_DISK_PARAMETERS_V2
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
            public OPEN_VIRTUAL_DISK_FLAG OpenFlags;
            [MarshalAs(UnmanagedType.Struct)]
            public VIRTUAL_STORAGE_TYPE ParentVirtualStorageType;
            [MarshalAs(UnmanagedType.Struct)]
            public VIRTUAL_STORAGE_TYPE SourceVirtualStorageType;
            [MarshalAs(UnmanagedType.Struct)]
            public Guid ResiliencyGuid;
        }

        // ' Versioned OpenVirtualDisk parameter structure
        public struct OPEN_VIRTUAL_DISK_PARAMETERS_V1
        {
            public OPEN_VIRTUAL_DISK_VERSION Version;
            public uint RWDepth;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 20)]
            public byte[] res;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct OPEN_VIRTUAL_DISK_PARAMETERS_V2
        {
            public OPEN_VIRTUAL_DISK_VERSION Version;
            [MarshalAs(UnmanagedType.Bool)]
            public bool GetInfoOnly;
            [MarshalAs(UnmanagedType.Bool)]
            public bool ReadOnly;
            [MarshalAs(UnmanagedType.Struct)]
            public Guid ResiliencyGuid;
        }

        // ' Functions
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

        // '
        // ' This value causes the implementation defaults to be used for block size:
        // '
        // ' Fixed VHDs: 0 is the only valid value since block size is N/A.
        // ' Dynamic VHDs: The default block size will be used (2 MB, subject to change).
        // ' Differencing VHDs: 0 causes the parent VHD's block size to be used.
        // '
        public const int CREATE_VIRTUAL_DISK_PARAMETERS_DEFAULT_BLOCK_SIZE = 0;

        // ' Default logical sector size is 512B
        public const int CREATE_VIRTUAL_DISK_PARAMETERS_DEFAULT_SECTOR_SIZE = 0;

        // '
        // ' AttachVirtualDisk
        // '

        // ' Version definitions
        [Flags()]
        public enum ATTACH_VIRTUAL_DISK_VERSION
        {
            ATTACH_VIRTUAL_DISK_VERSION_UNSPECIFIED = 0,
            ATTACH_VIRTUAL_DISK_VERSION_1 = 1
        }

        // ' Versioned parameter structure for AttachVirtualDisk
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct ATTACH_VIRTUAL_DISK_PARAMETERS
        {
            public ATTACH_VIRTUAL_DISK_VERSION Version;
            public int Reserved;
        }

        // ' Flags for AttachVirtualDisk
        [Flags()]
        public enum ATTACH_VIRTUAL_DISK_FLAG
        {
            ATTACH_VIRTUAL_DISK_FLAG_NONE = 0x0,

            // ' Attach the disk as read only
            ATTACH_VIRTUAL_DISK_FLAG_READ_ONLY = 0x1,

            // ' Will cause all volumes on the disk to be mounted
            // ' without drive letters.
            ATTACH_VIRTUAL_DISK_FLAG_NO_DRIVE_LETTER = 0x2,

            // ' Will decouple the disk lifetime from that of the VirtualDiskHandle.
            // ' The disk will be attached until an explicit call is made to
            // ' DetachVirtualDisk even if all handles are closed.
            ATTACH_VIRTUAL_DISK_FLAG_PERMANENT_LIFETIME = 0x4,

            // ' Indicates that the drive will not be attached to
            // ' the local system (but rather to a VM).
            ATTACH_VIRTUAL_DISK_FLAG_NO_LOCAL_HOST = 0x8
        }

        [DllImport("VirtDisk.dll")]
        public static extern uint AttachVirtualDisk(IntPtr Handle, IntPtr SecurityDescriptor, ATTACH_VIRTUAL_DISK_FLAG Flags, uint ProviderSpecificFlags, IntPtr Parameters, IntPtr Overlapped);
        [DllImport("VirtDisk.dll")]
        public static extern uint AttachVirtualDisk(IntPtr Handle, SecurityDescriptor.SECURITY_DESCRIPTOR SecurityDescriptor, ATTACH_VIRTUAL_DISK_FLAG Flags, uint ProviderSpecificFlags, ATTACH_VIRTUAL_DISK_PARAMETERS Parameters, IntPtr Overlapped);

        // '
        // ' DetachVirtualDisk
        // '

        // ' Flags for DetachVirtualDisk
        [Flags()]
        public enum DETACH_VIRTUAL_DISK_FLAG
        {
            DETACH_VIRTUAL_DISK_FLAG_NONE = 0
        }

        [DllImport("VirtDisk.dll")]
        public static extern uint DetachVirtualDisk(IntPtr Handle, DETACH_VIRTUAL_DISK_FLAG Flag, uint ProviderSpecificFlags);

        // '
        // ' GetVirtualDiskPhysicalPath
        // '

        [DllImport("VirtDisk.dll")]
        public static extern uint GetVirtualDiskPhysicalPath(IntPtr Handle, ref uint DiskPathSizeInBytes, [MarshalAs(UnmanagedType.LPWStr)] ref string DiskPath);
        [DllImport("VirtDisk.dll")]
        public static extern uint GetVirtualDiskPhysicalPath(IntPtr Handle, ref uint DiskPathSizeInBytes, byte[] DiskPath);
        [DllImport("VirtDisk.dll")]
        public static extern uint GetVirtualDiskPhysicalPath(IntPtr Handle, ref uint DiskPathSizeInBytes, IntPtr DiskPath);
        // '
        // ' GetAllAttachedVirtualDiskPhysicalPaths
        // '

        [DllImport("VirtDisk.dll")]
        public static extern uint GetAllAttachedVirtualDiskPhysicalPaths(ref uint PathsBufferSizeInBytes, [MarshalAs(UnmanagedType.LPWStr)] string PathsBuffer);

        // '
        // ' GetStorageDependencyInformation
        // '

        // ' Flags for dependent disks
        [Flags()]
        public enum DEPENDENT_DISK_FLAG
        {
            DEPENDENT_DISK_FLAG_NONE = 0x0,

            // '
            // ' Multiple files backing the virtual storage device
            // '
            DEPENDENT_DISK_FLAG_MULT_BACKING_FILES = 0x1,
            DEPENDENT_DISK_FLAG_FULLY_ALLOCATED = 0x2,
            DEPENDENT_DISK_FLAG_READ_ONLY = 0x4,

            // '
            // 'Backing file of the virtual storage device is not local to the machine
            // '
            DEPENDENT_DISK_FLAG_REMOTE = 0x8,

            // '
            // ' Volume is the system volume
            // '
            DEPENDENT_DISK_FLAG_SYSTEM_VOLUME = 0x10,

            // '
            // ' Volume backing the virtual storage device file is the system volume
            // '
            DEPENDENT_DISK_FLAG_SYSTEM_VOLUME_PARENT = 0x20,
            DEPENDENT_DISK_FLAG_REMOVABLE = 0x40,

            // '
            // ' Drive letters are not assigned to the volumes
            // ' on the virtual disk automatically.
            // '
            DEPENDENT_DISK_FLAG_NO_DRIVE_LETTER = 0x80,
            DEPENDENT_DISK_FLAG_PARENT = 0x100,

            // '
            // ' Virtual disk is not attached on the local host
            // ' (instead attached on a guest VM for instance)
            // '
            DEPENDENT_DISK_FLAG_NO_HOST_DISK = 0x200,

            // '
            // ' Indicates the lifetime of the disk is not tied
            // ' to any system handles
            // '
            DEPENDENT_DISK_FLAG_PERMANENT_LIFETIME = 0x400
        }

        // ' Version definitions
        public enum STORAGE_DEPENDENCY_INFO_VERSION
        {
            STORAGE_DEPENDENCY_INFO_VERSION_UNSPECIFIED = 0,
            STORAGE_DEPENDENCY_INFO_VERSION_1 = 1,
            STORAGE_DEPENDENCY_INFO_VERSION_2 = 2
        }

        // ' Parameter structure for GetStorageDependencyInformation
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct STORAGE_DEPENDENCY_INFO_TYPE_1
        {
            public DEPENDENT_DISK_FLAG DependencyTypeFlags;
            public uint ProviderSpecificFlags;
            public VIRTUAL_STORAGE_TYPE VirtualStorageType;
        }

        // ' Parameter structure for GetStorageDependencyInformation
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

        // ' Parameter structure for GetStorageDependencyInformation
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct STORAGE_DEPENDENCY_INFO_V1
        {
            public STORAGE_DEPENDENCY_INFO_VERSION Version;
            public uint NumberEntries;
            [MarshalAs(UnmanagedType.Struct)]
            public STORAGE_DEPENDENCY_INFO_TYPE_1 Version1Entries;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct STORAGE_DEPENDENCY_INFO_V2
        {
            public STORAGE_DEPENDENCY_INFO_VERSION Version;
            public uint NumberEntries;
            [MarshalAs(UnmanagedType.Struct)]
            public STORAGE_DEPENDENCY_INFO_TYPE_2 Version2Entries;
        }

        // ' Flags for GetStorageDependencyInformation
        [Flags()]
        public enum GET_STORAGE_DEPENDENCY_FLAG
        {
            GET_STORAGE_DEPENDENCY_FLAG_NONE = 0,

            // ' Return information for volumes or disks hosting the volume specified
            // ' If not set, returns info about volumes or disks being hosted by
            // ' the volume or disk specified
            GET_STORAGE_DEPENDENCY_FLAG_HOST_VOLUMES = 1,

            // '  The handle provided is to a disk, not volume or file
            GET_STORAGE_DEPENDENCY_FLAG_DISK_HANDLE = 2
        }

        [DllImport("VirtDisk.dll")]
        public static extern uint GetStorageDependencyInformation(IntPtr Handle, GET_STORAGE_DEPENDENCY_FLAG Flags, uint StorageDependencyInfoSize, IntPtr StorageDependencyInfo, ref uint SizeUsed);

        // '
        // ' GetVirtualDiskInformation
        // '

        // ' Version definitions
        public enum GET_VIRTUAL_DISK_INFO_VERSION
        {
            GET_VIRTUAL_DISK_INFO_UNSPECIFIED = 0,
            GET_VIRTUAL_DISK_INFO_SIZE = 1,
            GET_VIRTUAL_DISK_INFO_IDENTIFIER = 2,
            GET_VIRTUAL_DISK_INFO_PARENT_LOCATION = 3,
            GET_VIRTUAL_DISK_INFO_PARENT_IDENTIFIER = 4,
            GET_VIRTUAL_DISK_INFO_PARENT_TIMESTAMP = 5,
            GET_VIRTUAL_DISK_INFO_VIRTUAL_STORAGE_TYPE = 6,
            GET_VIRTUAL_DISK_INFO_PROVIDER_SUBTYPE = 7,
            GET_VIRTUAL_DISK_INFO_IS_4K_ALIGNED = 8,
            GET_VIRTUAL_DISK_INFO_PHYSICAL_DISK = 9,
            GET_VIRTUAL_DISK_INFO_VHD_PHYSICAL_SECTOR_SIZE = 10,
            GET_VIRTUAL_DISK_INFO_SMALLEST_SAFE_VIRTUAL_SIZE = 11,
            GET_VIRTUAL_DISK_INFO_FRAGMENTATION = 12,
            GET_VIRTUAL_DISK_INFO_IS_LOADED = 13,
            GET_VIRTUAL_DISK_INFO_VIRTUAL_DISK_ID = 14
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct GET_VIRTUAL_DISK_INFO_SIZE
        {
            public GET_VIRTUAL_DISK_INFO_VERSION Version;
            public ulong VirtualSize;
            public ulong PhysicalSize;
            public uint BlockSize;
            public uint SectorSize;
        }

        [DllImport("VirtDisk.dll")]
        public static extern uint GetVirtualDiskInformation(IntPtr Handle, ref uint VirtualDiskInfoSize, GET_VIRTUAL_DISK_INFO_SIZE VirtualDiskInfo, ref uint SizeUsed);

        // '
        // '

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct GET_VIRTUAL_DISK_INFO_IDENTIFIER
        {
            public GET_VIRTUAL_DISK_INFO_VERSION Version;
            [MarshalAs(UnmanagedType.Struct)]
            public Guid Identifier;
        }

        [DllImport("VirtDisk.dll")]
        public static extern uint GetVirtualDiskInformation(IntPtr Handle, ref uint VirtualDiskInfoSize, GET_VIRTUAL_DISK_INFO_IDENTIFIER VirtualDiskInfo, ref uint SizeUsed);

        // '

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct GET_VIRTUAL_DISK_INFO_PARENT_LOCATION
        {
            public GET_VIRTUAL_DISK_INFO_VERSION Version;
            [MarshalAs(UnmanagedType.Bool)]
            public bool ParentResolved;
            public char ParentLocationBuffer;
        }

        [DllImport("VirtDisk.dll")]
        public static extern uint GetVirtualDiskInformation(IntPtr Handle, ref uint VirtualDiskInfoSize, GET_VIRTUAL_DISK_INFO_PARENT_LOCATION VirtualDiskInfo, ref uint SizeUsed);

        // ' Since I foresee problems with the above declare (seems useless) this one will be the catch-all
        [DllImport("VirtDisk.dll")]
        public static extern uint GetVirtualDiskInformation(IntPtr Handle, ref uint VirtualDiskInfoSize, IntPtr VirtualDiskInfo, ref uint SizeUsed);
        // '

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct GET_VIRTUAL_DISK_INFO_PARENT_IDENTIFIER
        {
            public GET_VIRTUAL_DISK_INFO_VERSION Version;
            [MarshalAs(UnmanagedType.Struct)]
            public Guid ParentIdentifier;
        }

        [DllImport("VirtDisk.dll")]
        public static extern uint GetVirtualDiskInformation(IntPtr Handle, ref uint VirtualDiskInfoSize, GET_VIRTUAL_DISK_INFO_PARENT_IDENTIFIER VirtualDiskInfo, ref uint SizeUsed);

        // '

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct GET_VIRTUAL_DISK_INFO_PARENT_TIMESTAMP
        {
            public GET_VIRTUAL_DISK_INFO_VERSION Version;
            public uint ParentTimestamp;
        }

        [DllImport("VirtDisk.dll")]
        public static extern uint GetVirtualDiskInformation(IntPtr Handle, ref uint VirtualDiskInfoSize, GET_VIRTUAL_DISK_INFO_PARENT_TIMESTAMP VirtualDiskInfo, ref uint SizeUsed);

        // '

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct GET_VIRTUAL_DISK_INFO_VIRTUAL_STORAGE_TYPE
        {
            public GET_VIRTUAL_DISK_INFO_VERSION Version;
            public VIRTUAL_STORAGE_TYPE VirtualStorageType;
        }

        [DllImport("VirtDisk.dll")]
        public static extern uint GetVirtualDiskInformation(IntPtr Handle, ref uint VirtualDiskInfoSize, GET_VIRTUAL_DISK_INFO_VIRTUAL_STORAGE_TYPE VirtualDiskInfo, ref uint SizeUsed);

        // '

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct GET_VIRTUAL_DISK_INFO_PROVIDER_SUBTYPE
        {
            public GET_VIRTUAL_DISK_INFO_VERSION Version;
            public uint ProviderSubType;
        }

        [DllImport("VirtDisk.dll")]
        public static extern uint GetVirtualDiskInformation(IntPtr Handle, ref uint VirtualDiskInfoSize, GET_VIRTUAL_DISK_INFO_PROVIDER_SUBTYPE VirtualDiskInfo, ref uint SizeUsed);

        // '

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct GET_VIRTUAL_DISK_INFO_IS4KALIGNED
        {
            public GET_VIRTUAL_DISK_INFO_VERSION Version;
            [MarshalAs(UnmanagedType.Bool)]
            public bool Is4kAligned;
        }

        [DllImport("VirtDisk.dll")]
        public static extern uint GetVirtualDiskInformation(IntPtr Handle, ref uint VirtualDiskInfoSize, GET_VIRTUAL_DISK_INFO_IS4KALIGNED VirtualDiskInfo, ref uint SizeUsed);

        // '

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct GET_VIRTUAL_DISK_INFO_ISLOADED
        {
            public GET_VIRTUAL_DISK_INFO_VERSION Version;
            [MarshalAs(UnmanagedType.Bool)]
            public bool IsLoaded;
        }

        [DllImport("VirtDisk.dll")]
        public static extern uint GetVirtualDiskInformation(IntPtr Handle, ref uint VirtualDiskInfoSize, GET_VIRTUAL_DISK_INFO_ISLOADED VirtualDiskInfo, ref uint SizeUsed);

        // '

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct GET_VIRTUAL_DISK_INFO_LOGICAL_SECTOR_SIZE
        {
            public GET_VIRTUAL_DISK_INFO_VERSION Version;
            public uint LogicalSectorSize;
            public uint PhysicalSectorSize;
            [MarshalAs(UnmanagedType.Bool)]
            public bool IsRemote;
        }

        [DllImport("VirtDisk.dll")]
        public static extern uint GetVirtualDiskInformation(IntPtr Handle, ref uint VirtualDiskInfoSize, GET_VIRTUAL_DISK_INFO_LOGICAL_SECTOR_SIZE VirtualDiskInfo, ref uint SizeUsed);

        // '

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct GET_VIRTUAL_DISK_INFO_VHD_PHYSICAL_SECTOR_SIZE
        {
            public GET_VIRTUAL_DISK_INFO_VERSION Version;
            public uint VhdPhysicalSectorSize;
        }

        [DllImport("VirtDisk.dll")]
        public static extern uint GetVirtualDiskInformation(IntPtr Handle, ref uint VirtualDiskInfoSize, GET_VIRTUAL_DISK_INFO_VHD_PHYSICAL_SECTOR_SIZE VirtualDiskInfo, ref uint SizeUsed);

        // '

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct GET_VIRTUAL_DISK_INFO_SMALLEST_SAFE_VIRTUAL_SIZE
        {
            public GET_VIRTUAL_DISK_INFO_VERSION Version;
            public ulong SmallestSafeVirtualSize;
        }

        [DllImport("VirtDisk.dll")]
        public static extern uint GetVirtualDiskInformation(IntPtr Handle, ref uint VirtualDiskInfoSize, GET_VIRTUAL_DISK_INFO_SMALLEST_SAFE_VIRTUAL_SIZE VirtualDiskInfo, ref uint SizeUsed);

        // '

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct GET_VIRTUAL_DISK_INFO_FRAGMENTATION
        {
            public GET_VIRTUAL_DISK_INFO_VERSION Version;
            public uint FragmentationPercentage;
        }

        [DllImport("VirtDisk.dll")]
        public static extern uint GetVirtualDiskInformation(IntPtr Handle, ref uint VirtualDiskInfoSize, GET_VIRTUAL_DISK_INFO_FRAGMENTATION VirtualDiskInfo, ref uint SizeUsed);

        // '

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct GET_VIRTUAL_DISK_INFO_VIRTUAL_DISK_ID
        {
            public GET_VIRTUAL_DISK_INFO_VERSION Version;
            [MarshalAs(UnmanagedType.Struct)]
            public Guid VirtualDiskId;
        }

        [DllImport("VirtDisk.dll")]
        public static extern uint GetVirtualDiskInformation(IntPtr Handle, ref uint VirtualDiskInfoSize, GET_VIRTUAL_DISK_INFO_VIRTUAL_DISK_ID VirtualDiskInfo, ref uint SizeUsed);

        // '
        // ' SetVirtualDiskInformation
        // '

        // ' Version definitions
        public enum SET_VIRTUAL_DISK_INFO_VERSION
        {
            SET_VIRTUAL_DISK_INFO_UNSPECIFIED = 0,
            SET_VIRTUAL_DISK_INFO_PARENT_PATH = 1,
            SET_VIRTUAL_DISK_INFO_IDENTIFIER = 2,
            SET_VIRTUAL_DISK_INFO_PARENT_PATH_WITH_DEPTH = 3,
            SET_VIRTUAL_DISK_INFO_PHYSICAL_SECTOR_SIZE = 4,
            SET_VIRTUAL_DISK_INFO_VIRTUAL_DISK_ID = 5
        }

        // ' Versioned parameter structure for SetVirtualDiskInformation
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct SET_VIRTUAL_DISK_INFO_PARENT_FILE_PATH
        {
            public SET_VIRTUAL_DISK_INFO_VERSION Version;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string ParentFilePath;
        }

        [DllImport("VirtDisk.dll")]
        public static extern uint SetVirtualDiskInformation(IntPtr VirtualDiskHandle, SET_VIRTUAL_DISK_INFO_PARENT_FILE_PATH VirtualDiskInfo);

        // ' Versioned parameter structure for SetVirtualDiskInformation
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct SET_VIRTUAL_DISK_INFO_UNIQUE_ID
        {
            public SET_VIRTUAL_DISK_INFO_VERSION Version;
            [MarshalAs(UnmanagedType.Struct)]
            public Guid UniqueIdentifier;
        }

        [DllImport("VirtDisk.dll")]
        public static extern uint SetVirtualDiskInformation(IntPtr VirtualDiskHandle, SET_VIRTUAL_DISK_INFO_UNIQUE_ID VirtualDiskInfo);

        // ' Versioned parameter structure for SetVirtualDiskInformation
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct SET_VIRTUAL_DISK_INFO_PARENT_PATH_WITH_DEPTH_INFO
        {
            public SET_VIRTUAL_DISK_INFO_VERSION Version;
            public uint ChildDepth;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string ParentFilePath;
        }

        [DllImport("VirtDisk.dll")]
        public static extern uint SetVirtualDiskInformation(IntPtr VirtualDiskHandle, SET_VIRTUAL_DISK_INFO_PARENT_PATH_WITH_DEPTH_INFO VirtualDiskInfo);

        // ' Versioned parameter structure for SetVirtualDiskInformation
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct SET_VIRTUAL_DISK_INFO_VHD_SECTOR_SIZE
        {
            public SET_VIRTUAL_DISK_INFO_VERSION Version;
            public uint VhdPhysicalSectorSize;
        }

        [DllImport("VirtDisk.dll")]
        public static extern uint SetVirtualDiskInformation(IntPtr VirtualDiskHandle, SET_VIRTUAL_DISK_INFO_VHD_SECTOR_SIZE VirtualDiskInfo);

        // ' Points to as list of Guids (in Items)
        [DllImport("VirtDisk.dll")]
        public static extern uint EnumerateVirtualDiskMetadata(IntPtr VirtualDiskHandle, ref uint NumberOfItems, IntPtr Items);
        [DllImport("VirtDisk.dll")]
        public static extern uint GetVirtualDiskMetadata(IntPtr VirtualDiskHandle, Guid Item, ref uint MetaDataSize, IntPtr MetaData);
        [DllImport("VirtDisk.dll")]
        public static extern uint SetVirtualDiskMetadata(IntPtr VirtualDiskHandle, Guid Item, uint MetaDataSize, IntPtr MetaData);
        [DllImport("VirtDisk.dll")]
        public static extern uint DeleteVirtualDiskMetadata(IntPtr VirtualDiskHandle, Guid Item);

        // ' NTDDI_VERSION >= NTDDI_WIN8

        // '
        // ' GetVirtualDiskOperationProgress
        // '

        public struct VIRTUAL_DISK_PROGRESS
        {
            public uint OperationStatus;
            public ulong CurrentValue;
            public ulong CompletionValue;
        }

        [DllImport("VirtDisk.dll")]
        public static extern uint GetVirtualDiskOperationProgress(IntPtr VirtualDiskHandle, IntPtr Overlapped, VIRTUAL_DISK_PROGRESS Progress);

        // '
        // ' CompactVirtualDisk
        // '

        // ' Version definitions
        public enum COMPACT_VIRTUAL_DISK_VERSION
        {
            COMPACT_VIRTUAL_DISK_VERSION_UNSPECIFIED = 0,
            COMPACT_VIRTUAL_DISK_VERSION_1 = 1
        }

        // ' Versioned structure for CompactVirtualDisk
        public struct COMPACT_VIRTUAL_DISK_PARAMETERS
        {
            public COMPACT_VIRTUAL_DISK_VERSION Version;
            public uint Reserved;
        }

        // ' Flags for CompactVirtualDisk
        public enum COMPACT_VIRTUAL_DISK_FLAG
        {
            COMPACT_VIRTUAL_DISK_FLAG_NONE = 0,
            COMPACT_VIRTUAL_DISK_FLAG_NO_ZERO_SCAN = 1,
            COMPACT_VIRTUAL_DISK_FLAG_NO_BLOCK_MOVES = 2
        }

        [DllImport("VirtDisk.dll")]
        public static extern uint CompactVirtualDisk(IntPtr VirtualDiskHandle, COMPACT_VIRTUAL_DISK_FLAG Flags, COMPACT_VIRTUAL_DISK_PARAMETERS Parameters, IntPtr Overlapped);

        // '
        // ' MergeVirtualDisk
        // '

        // ' Version definitions
        public enum MERGE_VIRTUAL_DISK_VERSION
        {
            MERGE_VIRTUAL_DISK_VERSION_UNSPECIFIED = 0,
            MERGE_VIRTUAL_DISK_VERSION_1 = 1,
            MERGE_VIRTUAL_DISK_VERSION_2 = 2
        }

        // ' Versioned parameter structure for MergeVirtualDisk
        public const int MERGE_VIRTUAL_DISK_DEFAULT_MERGE_DEPTH = 1;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct MERGE_VIRTUAL_DISK_PARAMETERS_V1
        {
            public MERGE_VIRTUAL_DISK_VERSION Version;
            public uint MergeDepth;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct MERGE_VIRTUAL_DISK_PARAMETERS_V2
        {
            public MERGE_VIRTUAL_DISK_VERSION Version;
            public uint MergeSourceDepth;
            public uint MergeTargetDepth;
        }

        // ' Flags for MergeVirtualDisk
        public enum MERGE_VIRTUAL_DISK_FLAG
        {
            MERGE_VIRTUAL_DISK_FLAG_NONE = 0
        }

        [DllImport("VirtDisk.dll")]
        public static extern uint MergeVirtualDisk(IntPtr VirtualDiskHandle, MERGE_VIRTUAL_DISK_FLAG Flags, MERGE_VIRTUAL_DISK_PARAMETERS_V2 Parameters, IntPtr Overlapped);

        // '
        // ' ExpandVirtualDisk
        // '

        // ' Version definitions
        public enum EXPAND_VIRTUAL_DISK_VERSION
        {
            EXPAND_VIRTUAL_DISK_VERSION_UNSPECIFIED = 0,
            EXPAND_VIRTUAL_DISK_VERSION_1 = 1
        }

        // ' Versioned parameter structure for ResizeVirtualDisk
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct EXPAND_VIRTUAL_DISK_PARAMETERS
        {
            public EXPAND_VIRTUAL_DISK_VERSION Version;
            [MarshalAs(UnmanagedType.LPWStr)]
            public ulong NewSize;
        }

        // ' Flags for ExpandVirtualDisk
        public enum EXPAND_VIRTUAL_DISK_FLAG
        {
            EXPAND_VIRTUAL_DISK_FLAG_NONE = 0
        }

        [DllImport("VirtDisk.dll")]
        public static extern uint ExpandVirtualDisk(IntPtr VirtualDiskHandle, EXPAND_VIRTUAL_DISK_FLAG Flags, EXPAND_VIRTUAL_DISK_PARAMETERS Parameters, IntPtr Overlapped);

        // '
        // ' ResizeVirtualDisk
        // '

        // ' Version definitions
        public enum RESIZE_VIRTUAL_DISK_VERSION
        {
            RESIZE_VIRTUAL_DISK_VERSION_UNSPECIFIED = 0,
            RESIZE_VIRTUAL_DISK_VERSION_1 = 1
        }

        // ' Versioned parameter structure for ResizeVirtualDisk
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct RESIZE_VIRTUAL_DISK_PARAMETERS
        {
            public RESIZE_VIRTUAL_DISK_VERSION Version;
            [MarshalAs(UnmanagedType.LPWStr)]
            public ulong NewSize;
        }

        // ' Flags for ResizeVirtualDisk
        public enum RESIZE_VIRTUAL_DISK_FLAG
        {
            RESIZE_VIRTUAL_DISK_FLAG_NONE = 0,

            // ' If this flag is set, skip checking the virtual disk's partition table
            // ' to ensure that this truncation is safe. Setting this flag can cause
            // ' unrecoverable data loss; use with care.
            RESIZE_VIRTUAL_DISK_FLAG_ALLOW_UNSAFE_VIRTUAL_SIZE = 1,

            // ' If this flag is set, resize the disk to the smallest virtual size
            // ' possible without truncating past any existing partitions. If this
            // ' is set, NewSize in RESIZE_VIRTUAL_DISK_PARAMETERS must be zero.
            RESIZE_VIRTUAL_DISK_FLAG_RESIZE_TO_SMALLEST_SAFE_VIRTUAL_SIZE = 2
        }

        [DllImport("VirtDisk.dll")]
        public static extern uint ResizeVirtualDisk(IntPtr VirtualDiskHandle, RESIZE_VIRTUAL_DISK_FLAG Flags, RESIZE_VIRTUAL_DISK_PARAMETERS Parameters, IntPtr Overlapped);

        // ' NTDDI_VERSION >= NTDDI_WIN8

        // '
        // ' MirrorVirtualDisk
        // '

        // ' Version definitions
        public enum MIRROR_VIRTUAL_DISK_VERSION
        {
            MIRROR_VIRTUAL_DISK_VERSION_UNSPECIFIED = 0,
            MIRROR_VIRTUAL_DISK_VERSION_1 = 1
        }

        // ' Versioned parameter structure for MirrorVirtualDisk
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct MIRROR_VIRTUAL_DISK_PARAMETERS
        {
            public MIRROR_VIRTUAL_DISK_VERSION Version;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string MirrorVirtualDiskPath;
        }

        // ' Flags for MirrorVirtualDisk
        public enum MIRROR_VIRTUAL_DISK_FLAG
        {
            MIRROR_VIRTUAL_DISK_FLAG_NONE = 0,
            MIRROR_VIRTUAL_DISK_FLAG_EXISTING_FILE = 1
        }

        [DllImport("VirtDisk.dll")]
        public static extern uint MirrorVirtualDisk(IntPtr VirtualDiskHandle, MIRROR_VIRTUAL_DISK_FLAG Flags, MIRROR_VIRTUAL_DISK_PARAMETERS Parameters, IntPtr Overlapped);

        // ' NTDDI_VERSION >= NTDDI_WIN8

        // '
        // ' BreakMirrorVirtualDisk
        // '

        [DllImport("VirtDisk.dll")]
        public static extern uint BreakMirrorVirtualDisk(IntPtr VirtualDiskHandle);

        // ' NTDDI_VERSION >= NTDDI_WIN8

        // '
        // ' AddVirtualDiskParent
        // '

        [DllImport("VirtDisk.dll")]
        public static extern uint AddVirtualDiskParent(IntPtr VirtualDiskHandle, [MarshalAs(UnmanagedType.LPWStr)] string ParentPath);

        // ' END NTDDI_VERSION >= NTDDI_WIN8

        // ' END VIRTDISK_DEFINE_FLAGS

    }
}