'' ************************************************* ''
'' DataTools Visual Basic Utility Library - Interop
''
'' Module: VDiskDecl
''         Port of virtdisk.h (in its entirety)
''
'' Copyright (C) 2011-2020 Nathan Moschkin
'' All Rights Reserved
''
'' Licensed Under the Microsoft Public License   
'' ************************************************* ''


Imports System
Imports System.IO
Imports System.ComponentModel
Imports System.Runtime.InteropServices
Imports System.Reflection
Imports System.Security
Imports DataTools.Interop
Imports CoreCT.Memory

Namespace Native

    <HideModuleName>
    Friend Module VDiskDecl

        Public VIRTUAL_STORAGE_TYPE_VENDOR_UNKNOWN As New Guid(0, 0S, 0S, {0, 0, 0, 0, 0, 0, 0, 0})
        Public VIRTUAL_STORAGE_TYPE_VENDOR_MICROSOFT As New Guid("EC984AEC-A0F9-47e9-901F-71415A66345B")

        Public Const VIRTUAL_STORAGE_TYPE_DEVICE_UNKNOWN = 0
        Public Const VIRTUAL_STORAGE_TYPE_DEVICE_ISO = 1
        Public Const VIRTUAL_STORAGE_TYPE_DEVICE_VHD = 2
        Public Const VIRTUAL_STORAGE_TYPE_DEVICE_VHDX = 3

        Public Enum CREATE_VIRTUAL_DISK_VERSION
            CREATE_VIRTUAL_DISK_VERSION_UNSPECIFIED = 0
            CREATE_VIRTUAL_DISK_VERSION_1 = 1
            CREATE_VIRTUAL_DISK_VERSION_2 = 2
        End Enum

        Public Enum OPEN_VIRTUAL_DISK_VERSION
            OPEN_VIRTUAL_DISK_VERSION_UNSPECIFIED = 0
            OPEN_VIRTUAL_DISK_VERSION_1 = 1
            OPEN_VIRTUAL_DISK_VERSION_2 = 2
        End Enum

        <Flags()>
        Public Enum OPEN_VIRTUAL_DISK_FLAG
            OPEN_VIRTUAL_DISK_FLAG_NONE = &H0
            '' Open the backing store without opening any differencing chain parents.
            '' This allows one to fixup broken parent links.
            OPEN_VIRTUAL_DISK_FLAG_NO_PARENTS = &H1

            '' The backing store being opened is an empty file. Do not perform virtual
            '' disk verification.
            OPEN_VIRTUAL_DISK_FLAG_BLANK_FILE = &H2

            '' This flag is only specified at boot time to load the system disk
            '' during virtual disk boot.  Must be kernel mode to specify this flag.
            OPEN_VIRTUAL_DISK_FLAG_BOOT_DRIVE = &H4

            '' This flag causes the backing file to be opened in cached mode.
            OPEN_VIRTUAL_DISK_FLAG_CACHED_IO = &H8

            '' Open the backing store without opening any differencing chain parents.
            '' This allows one to fixup broken parent links temporarily without updating
            '' the parent locator.
            OPEN_VIRTUAL_DISK_FLAG_CUSTOM_DIFF_CHAIN = &H10

            '' This flag causes all backing stores except the leaf backing store to
            '' be opened in cached mode.
            OPEN_VIRTUAL_DISK_FLAG_PARENT_CACHED_IO = &H20
        End Enum

        ''
        ''  Access Mask for OpenVirtualDisk and CreateVirtualDisk.  The virtual
        ''  disk drivers expose file objects as handles therefore we map
        ''  it into that AccessMask space.
        <Flags()>
        Public Enum VIRTUAL_DISK_ACCESS_MASK
            VIRTUAL_DISK_ACCESS_NONE = &H0
            VIRTUAL_DISK_ACCESS_ATTACH_RO = &H10000
            VIRTUAL_DISK_ACCESS_ATTACH_RW = &H20000
            VIRTUAL_DISK_ACCESS_DETACH = &H40000
            VIRTUAL_DISK_ACCESS_GET_INFO = &H80000
            VIRTUAL_DISK_ACCESS_CREATE = &H100000
            VIRTUAL_DISK_ACCESS_METAOPS = &H200000
            VIRTUAL_DISK_ACCESS_READ = &HD0000
            VIRTUAL_DISK_ACCESS_ALL = &H3F0000
            ''
            ''
            '' A special flag to be used to test if the virtual disk needs to be
            '' opened for write.
            ''
            VIRTUAL_DISK_ACCESS_WRITABLE = &H320000
        End Enum

        <Flags()>
        Public Enum CREATE_VIRTUAL_DISK_FLAGS
            CREATE_VIRTUAL_DISK_FLAG_NONE = &H0
            CREATE_VIRTUAL_DISK_FLAG_FULL_PHYSICAL_ALLOCATION = &H1
            CREATE_VIRTUAL_DISK_FLAG_PREVENT_WRITES_TO_SOURCE_DISK = &H2
            CREATE_VIRTUAL_DISK_FLAG_DO_NOT_COPY_METADATA_FROM_PARENT = &H4
        End Enum

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure VIRTUAL_STORAGE_TYPE
            Public DeviceId As UInteger

            '        <MarshalAs(UnmanagedType.ByValArray, ArraySubType:=UnmanagedType.U1, SizeConst:=16)> _

            <MarshalAs(UnmanagedType.Struct)>
            Public VendorId As Guid
        End Structure

        ''' <summary>
        ''' Only supported on Windows 7 and greater
        ''' </summary>
        ''' <remarks></remarks>
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure CREATE_VIRTUAL_DISK_PARAMETERS_V1
            Public Version As CREATE_VIRTUAL_DISK_VERSION

            <MarshalAs(UnmanagedType.Struct)>
            Public UniqueId As Guid

            Public MaximumSize As ULong
            Public BlockSizeInBytes As UInteger
            Public SectorSizeInBytes As Integer

            <MarshalAs(UnmanagedType.LPWStr)>
            Public ParentPath As String

            <MarshalAs(UnmanagedType.LPWStr)>
            Public SourcePath As String

        End Structure

        ''' <summary>
        ''' Only supported for Windows 8/Server 2012 and greater
        ''' </summary>
        ''' <remarks></remarks>
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure CREATE_VIRTUAL_DISK_PARAMETERS_V2
            Public Version As CREATE_VIRTUAL_DISK_VERSION

            <MarshalAs(UnmanagedType.Struct)>
            Public UniqueId As Guid

            Public MaximumSize As ULong
            Public BlockSizeInBytes As UInteger
            Public SectorSizeInBytes As Integer

            <MarshalAs(UnmanagedType.LPWStr)>
            Public ParentPath As String

            <MarshalAs(UnmanagedType.LPWStr)>
            Public SourcePath As String

            Public OpenFlags As OPEN_VIRTUAL_DISK_FLAG

            <MarshalAs(UnmanagedType.Struct)>
            Public ParentVirtualStorageType As VIRTUAL_STORAGE_TYPE

            <MarshalAs(UnmanagedType.Struct)>
            Public SourceVirtualStorageType As VIRTUAL_STORAGE_TYPE

            <MarshalAs(UnmanagedType.Struct)>
            Public ResiliencyGuid As Guid

        End Structure

        '' Versioned OpenVirtualDisk parameter structure
        Public Structure OPEN_VIRTUAL_DISK_PARAMETERS_V1
            Public Version As OPEN_VIRTUAL_DISK_VERSION
            Public RWDepth As UInteger

            <MarshalAs(UnmanagedType.ByValArray, ArraySubType:=UnmanagedType.I1, SizeConst:=20)>
            Public res() As Byte
        End Structure

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure OPEN_VIRTUAL_DISK_PARAMETERS_V2
            Public Version As OPEN_VIRTUAL_DISK_VERSION

            <MarshalAs(UnmanagedType.Bool)>
            Public GetInfoOnly As Boolean

            <MarshalAs(UnmanagedType.Bool)>
            Public [ReadOnly] As Boolean

            <MarshalAs(UnmanagedType.Struct)>
            Public ResiliencyGuid As Guid
        End Structure

        '' Functions
        Public Declare Function CreateVirtualDisk Lib "VirtDisk.dll" _
        (VirtualStorageType As VIRTUAL_STORAGE_TYPE,
         <MarshalAs(UnmanagedType.LPWStr)> Path As String,
         VirtualDiskAccessMask As VIRTUAL_DISK_ACCESS_MASK,
         SecurityDescriptor As SECURITY_DESCRIPTOR,
         Flags As CREATE_VIRTUAL_DISK_FLAGS,
         ProviderSpecificFlags As UInteger,
         Parameters As CREATE_VIRTUAL_DISK_PARAMETERS_V1,
         Overlapped As IntPtr,
         ByRef Handle As IntPtr) As UInteger

        Public Declare Function CreateVirtualDisk Lib "VirtDisk.dll" _
        (VirtualStorageType As VIRTUAL_STORAGE_TYPE,
         <MarshalAs(UnmanagedType.LPWStr)> Path As String,
         VirtualDiskAccessMask As VIRTUAL_DISK_ACCESS_MASK,
         SecurityDescriptor As SECURITY_DESCRIPTOR,
         Flags As CREATE_VIRTUAL_DISK_FLAGS,
         ProviderSpecificFlags As UInteger,
         Parameters As CREATE_VIRTUAL_DISK_PARAMETERS_V2,
         Overlapped As IntPtr,
         ByRef Handle As IntPtr) As UInteger

        Public Declare Function CreateVirtualDisk Lib "VirtDisk.dll" _
        (VirtualStorageType As VIRTUAL_STORAGE_TYPE,
         <MarshalAs(UnmanagedType.LPWStr)> Path As String,
         VirtualDiskAccessMask As VIRTUAL_DISK_ACCESS_MASK,
         SecurityDescriptor As IntPtr,
         Flags As CREATE_VIRTUAL_DISK_FLAGS,
         ProviderSpecificFlags As UInteger,
         Parameters As CREATE_VIRTUAL_DISK_PARAMETERS_V1,
         Overlapped As IntPtr,
         ByRef Handle As IntPtr) As UInteger

        Public Declare Function CreateVirtualDisk Lib "VirtDisk.dll" _
        (VirtualStorageType As VIRTUAL_STORAGE_TYPE,
         <MarshalAs(UnmanagedType.LPWStr)> Path As String,
         VirtualDiskAccessMask As VIRTUAL_DISK_ACCESS_MASK,
         SecurityDescriptor As IntPtr,
         Flags As CREATE_VIRTUAL_DISK_FLAGS,
         ProviderSpecificFlags As UInteger,
         Parameters As CREATE_VIRTUAL_DISK_PARAMETERS_V2,
         Overlapped As IntPtr,
         ByRef Handle As IntPtr) As UInteger


        Public Declare Function OpenVirtualDisk Lib "VirtDisk.dll" _
        (VirtualStorageType As VIRTUAL_STORAGE_TYPE,
         Path As IntPtr,
         VirtualDiskAccessMask As VIRTUAL_DISK_ACCESS_MASK,
         Flags As OPEN_VIRTUAL_DISK_FLAG,
         Paramaters As OPEN_VIRTUAL_DISK_PARAMETERS_V1,
         ByRef Handle As IntPtr) As UInteger

        Public Declare Function OpenVirtualDisk Lib "VirtDisk.dll" _
        (VirtualStorageType As VIRTUAL_STORAGE_TYPE,
         <MarshalAs(UnmanagedType.LPWStr)> Path As String,
         VirtualDiskAccessMask As VIRTUAL_DISK_ACCESS_MASK,
         Flags As OPEN_VIRTUAL_DISK_FLAG,
         Paramaters As OPEN_VIRTUAL_DISK_PARAMETERS_V1,
         ByRef Handle As IntPtr) As UInteger

        Public Declare Function OpenVirtualDisk Lib "VirtDisk.dll" _
        (VirtualStorageType As VIRTUAL_STORAGE_TYPE,
         <MarshalAs(UnmanagedType.LPWStr)> Path As String,
         VirtualDiskAccessMask As VIRTUAL_DISK_ACCESS_MASK,
         Flags As OPEN_VIRTUAL_DISK_FLAG,
         Paramaters As OPEN_VIRTUAL_DISK_PARAMETERS_V1,
         ByRef Handle As Long) As UInteger

        Public Declare Function OpenVirtualDisk Lib "VirtDisk.dll" _
        Alias "OpenVirtualDisk" _
        (VirtualStorageType As VIRTUAL_STORAGE_TYPE,
         <MarshalAs(UnmanagedType.LPWStr)> Path As String,
         VirtualDiskAccessMask As VIRTUAL_DISK_ACCESS_MASK,
         Flags As OPEN_VIRTUAL_DISK_FLAG,
         Paramaters As OPEN_VIRTUAL_DISK_PARAMETERS_V2,
         ByRef Handle As IntPtr) As UInteger

        ''
        '' This value causes the implementation defaults to be used for block size:
        ''
        '' Fixed VHDs: 0 is the only valid value since block size is N/A.
        '' Dynamic VHDs: The default block size will be used (2 MB, subject to change).
        '' Differencing VHDs: 0 causes the parent VHD's block size to be used.
        ''
        Public Const CREATE_VIRTUAL_DISK_PARAMETERS_DEFAULT_BLOCK_SIZE = 0

        '' Default logical sector size is 512B
        Public Const CREATE_VIRTUAL_DISK_PARAMETERS_DEFAULT_SECTOR_SIZE = 0

        ''
        '' AttachVirtualDisk
        ''

        '' Version definitions
        <Flags()>
        Public Enum ATTACH_VIRTUAL_DISK_VERSION
            ATTACH_VIRTUAL_DISK_VERSION_UNSPECIFIED = 0
            ATTACH_VIRTUAL_DISK_VERSION_1 = 1
        End Enum

        '' Versioned parameter structure for AttachVirtualDisk
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure ATTACH_VIRTUAL_DISK_PARAMETERS
            Public Version As ATTACH_VIRTUAL_DISK_VERSION
            Public Reserved As Integer
        End Structure

        '' Flags for AttachVirtualDisk
        <Flags()>
        Public Enum ATTACH_VIRTUAL_DISK_FLAG

            ATTACH_VIRTUAL_DISK_FLAG_NONE = &H0

            '' Attach the disk as read only
            ATTACH_VIRTUAL_DISK_FLAG_READ_ONLY = &H1

            '' Will cause all volumes on the disk to be mounted
            '' without drive letters.
            ATTACH_VIRTUAL_DISK_FLAG_NO_DRIVE_LETTER = &H2

            '' Will decouple the disk lifetime from that of the VirtualDiskHandle.
            '' The disk will be attached until an explicit call is made to
            '' DetachVirtualDisk even if all handles are closed.
            ATTACH_VIRTUAL_DISK_FLAG_PERMANENT_LIFETIME = &H4

            '' Indicates that the drive will not be attached to
            '' the local system (but rather to a VM).
            ATTACH_VIRTUAL_DISK_FLAG_NO_LOCAL_HOST = &H8

        End Enum

        Public Declare Function AttachVirtualDisk Lib "VirtDisk.dll" _
        (Handle As IntPtr,
         SecurityDescriptor As IntPtr,
         Flags As ATTACH_VIRTUAL_DISK_FLAG,
         ProviderSpecificFlags As UInteger,
         Parameters As IntPtr,
         Overlapped As IntPtr) As UInteger

        Public Declare Function AttachVirtualDisk Lib "VirtDisk.dll" _
        (Handle As IntPtr,
         SecurityDescriptor As SECURITY_DESCRIPTOR,
         Flags As ATTACH_VIRTUAL_DISK_FLAG,
         ProviderSpecificFlags As UInteger,
         Parameters As ATTACH_VIRTUAL_DISK_PARAMETERS,
         Overlapped As IntPtr) As UInteger

        ''
        '' DetachVirtualDisk
        ''

        '' Flags for DetachVirtualDisk
        <Flags()>
        Public Enum DETACH_VIRTUAL_DISK_FLAG
            DETACH_VIRTUAL_DISK_FLAG_NONE = 0
        End Enum

        Public Declare Function DetachVirtualDisk Lib "VirtDisk.dll" _
        (Handle As IntPtr,
         Flag As DETACH_VIRTUAL_DISK_FLAG,
         ProviderSpecificFlags As UInteger) As UInteger

        ''
        '' GetVirtualDiskPhysicalPath
        ''

        Public Declare Function GetVirtualDiskPhysicalPath Lib "VirtDisk.dll" _
        (Handle As IntPtr,
         ByRef DiskPathSizeInBytes As UInteger,
         <MarshalAs(UnmanagedType.LPWStr)> ByRef DiskPath As String) As UInteger

        Public Declare Function GetVirtualDiskPhysicalPath Lib "VirtDisk.dll" _
        (Handle As IntPtr,
         ByRef DiskPathSizeInBytes As UInteger,
         DiskPath() As Byte) As UInteger

        Public Declare Function GetVirtualDiskPhysicalPath Lib "VirtDisk.dll" _
        (Handle As IntPtr,
         ByRef DiskPathSizeInBytes As UInteger,
         DiskPath As IntPtr) As UInteger
        ''
        '' GetAllAttachedVirtualDiskPhysicalPaths
        ''

        Public Declare Function GetAllAttachedVirtualDiskPhysicalPaths Lib "VirtDisk.dll" _
        (ByRef PathsBufferSizeInBytes As UInteger,
         <MarshalAs(UnmanagedType.LPWStr)> PathsBuffer As String) As UInteger

        ''
        '' GetStorageDependencyInformation
        ''

        '' Flags for dependent disks
        <Flags()>
        Public Enum DEPENDENT_DISK_FLAG

            DEPENDENT_DISK_FLAG_NONE = &H0

            ''
            '' Multiple files backing the virtual storage device
            ''
            DEPENDENT_DISK_FLAG_MULT_BACKING_FILES = &H1

            DEPENDENT_DISK_FLAG_FULLY_ALLOCATED = &H2

            DEPENDENT_DISK_FLAG_READ_ONLY = &H4

            ''
            ''Backing file of the virtual storage device is not local to the machine
            ''
            DEPENDENT_DISK_FLAG_REMOTE = &H8

            ''
            '' Volume is the system volume
            ''
            DEPENDENT_DISK_FLAG_SYSTEM_VOLUME = &H10

            ''
            '' Volume backing the virtual storage device file is the system volume
            ''
            DEPENDENT_DISK_FLAG_SYSTEM_VOLUME_PARENT = &H20

            DEPENDENT_DISK_FLAG_REMOVABLE = &H40

            ''
            '' Drive letters are not assigned to the volumes
            '' on the virtual disk automatically.
            ''
            DEPENDENT_DISK_FLAG_NO_DRIVE_LETTER = &H80

            DEPENDENT_DISK_FLAG_PARENT = &H100

            ''
            '' Virtual disk is not attached on the local host
            '' (instead attached on a guest VM for instance)
            ''
            DEPENDENT_DISK_FLAG_NO_HOST_DISK = &H200

            ''
            '' Indicates the lifetime of the disk is not tied
            '' to any system handles
            ''
            DEPENDENT_DISK_FLAG_PERMANENT_LIFETIME = &H400

        End Enum

        '' Version definitions
        Public Enum STORAGE_DEPENDENCY_INFO_VERSION
            STORAGE_DEPENDENCY_INFO_VERSION_UNSPECIFIED = 0
            STORAGE_DEPENDENCY_INFO_VERSION_1 = 1
            STORAGE_DEPENDENCY_INFO_VERSION_2 = 2
        End Enum

        '' Parameter structure for GetStorageDependencyInformation
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure STORAGE_DEPENDENCY_INFO_TYPE_1
            Public DependencyTypeFlags As DEPENDENT_DISK_FLAG
            Public ProviderSpecificFlags As UInteger
            Public VirtualStorageType As VIRTUAL_STORAGE_TYPE
        End Structure

        '' Parameter structure for GetStorageDependencyInformation
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode, Pack:=1)>
        Public Structure STORAGE_DEPENDENCY_INFO_TYPE_2
            Public DependencyTypeFlags As DEPENDENT_DISK_FLAG

            Public ProviderSpecificFlags As UInteger

            <MarshalAs(UnmanagedType.Struct)>
            Public VirtualStorageType As VIRTUAL_STORAGE_TYPE

            Public AncestorLevel As UInteger

            <MarshalAs(UnmanagedType.Struct)>
            Public DependencyDeviceName As MemPtr

            <MarshalAs(UnmanagedType.Struct)>
            Public HostVolumeName As MemPtr

            <MarshalAs(UnmanagedType.Struct)>
            Public DependentVolumeName As MemPtr

            <MarshalAs(UnmanagedType.Struct)>
            Public DependentVolumeRelativePath As MemPtr

        End Structure

        '' Parameter structure for GetStorageDependencyInformation
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure STORAGE_DEPENDENCY_INFO_V1
            Public Version As STORAGE_DEPENDENCY_INFO_VERSION
            Public NumberEntries As UInteger
            <MarshalAs(UnmanagedType.Struct)>
            Public Version1Entries As STORAGE_DEPENDENCY_INFO_TYPE_1
        End Structure

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure STORAGE_DEPENDENCY_INFO_V2
            Public Version As STORAGE_DEPENDENCY_INFO_VERSION
            Public NumberEntries As UInteger
            <MarshalAs(UnmanagedType.Struct)>
            Public Version2Entries As STORAGE_DEPENDENCY_INFO_TYPE_2
        End Structure

        '' Flags for GetStorageDependencyInformation
        <Flags()>
        Public Enum GET_STORAGE_DEPENDENCY_FLAG
            GET_STORAGE_DEPENDENCY_FLAG_NONE = 0

            '' Return information for volumes or disks hosting the volume specified
            '' If not set, returns info about volumes or disks being hosted by
            '' the volume or disk specified
            GET_STORAGE_DEPENDENCY_FLAG_HOST_VOLUMES = 1

            ''  The handle provided is to a disk, not volume or file
            GET_STORAGE_DEPENDENCY_FLAG_DISK_HANDLE = 2
        End Enum

        Public Declare Function GetStorageDependencyInformation Lib "VirtDisk.dll" _
        (Handle As IntPtr,
         Flags As GET_STORAGE_DEPENDENCY_FLAG,
         StorageDependencyInfoSize As UInteger,
         StorageDependencyInfo As IntPtr,
         ByRef SizeUsed As UInteger) As UInteger

        ''
        '' GetVirtualDiskInformation
        ''

        '' Version definitions
        Public Enum GET_VIRTUAL_DISK_INFO_VERSION
            GET_VIRTUAL_DISK_INFO_UNSPECIFIED = 0
            GET_VIRTUAL_DISK_INFO_SIZE = 1
            GET_VIRTUAL_DISK_INFO_IDENTIFIER = 2
            GET_VIRTUAL_DISK_INFO_PARENT_LOCATION = 3
            GET_VIRTUAL_DISK_INFO_PARENT_IDENTIFIER = 4
            GET_VIRTUAL_DISK_INFO_PARENT_TIMESTAMP = 5
            GET_VIRTUAL_DISK_INFO_VIRTUAL_STORAGE_TYPE = 6
            GET_VIRTUAL_DISK_INFO_PROVIDER_SUBTYPE = 7
            GET_VIRTUAL_DISK_INFO_IS_4K_ALIGNED = 8
            GET_VIRTUAL_DISK_INFO_PHYSICAL_DISK = 9
            GET_VIRTUAL_DISK_INFO_VHD_PHYSICAL_SECTOR_SIZE = 10
            GET_VIRTUAL_DISK_INFO_SMALLEST_SAFE_VIRTUAL_SIZE = 11
            GET_VIRTUAL_DISK_INFO_FRAGMENTATION = 12
            GET_VIRTUAL_DISK_INFO_IS_LOADED = 13
            GET_VIRTUAL_DISK_INFO_VIRTUAL_DISK_ID = 14
        End Enum

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure GET_VIRTUAL_DISK_INFO_SIZE
            Public Version As GET_VIRTUAL_DISK_INFO_VERSION
            Public VirtualSize As ULong
            Public PhysicalSize As ULong
            Public BlockSize As UInteger
            Public SectorSize As UInteger
        End Structure

        Public Declare Function GetVirtualDiskInformation Lib "VirtDisk.dll" _
    (Handle As IntPtr,
     ByRef VirtualDiskInfoSize As UInteger,
     VirtualDiskInfo As GET_VIRTUAL_DISK_INFO_SIZE,
     ByRef SizeUsed As UInteger) As UInteger

        ''
        ''

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure GET_VIRTUAL_DISK_INFO_IDENTIFIER
            Public Version As GET_VIRTUAL_DISK_INFO_VERSION
            <MarshalAs(UnmanagedType.Struct)>
            Public Identifier As Guid
        End Structure

        Public Declare Function GetVirtualDiskInformation Lib "VirtDisk.dll" _
    (Handle As IntPtr,
     ByRef VirtualDiskInfoSize As UInteger,
     VirtualDiskInfo As GET_VIRTUAL_DISK_INFO_IDENTIFIER,
     ByRef SizeUsed As UInteger) As UInteger

        ''

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure GET_VIRTUAL_DISK_INFO_PARENT_LOCATION
            Public Version As GET_VIRTUAL_DISK_INFO_VERSION
            <MarshalAs(UnmanagedType.Bool)>
            Public ParentResolved As Boolean
            Public ParentLocationBuffer As Char
        End Structure

        Public Declare Function GetVirtualDiskInformation Lib "VirtDisk.dll" _
    (Handle As IntPtr,
     ByRef VirtualDiskInfoSize As UInteger,
     VirtualDiskInfo As GET_VIRTUAL_DISK_INFO_PARENT_LOCATION,
     ByRef SizeUsed As UInteger) As UInteger

        '' Since I foresee problems with the above declare (seems useless) this one will be the catch-all
        Public Declare Function GetVirtualDiskInformation Lib "VirtDisk.dll" _
        (Handle As IntPtr,
         ByRef VirtualDiskInfoSize As UInteger,
         VirtualDiskInfo As IntPtr,
         ByRef SizeUsed As UInteger) As UInteger
        ''

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure GET_VIRTUAL_DISK_INFO_PARENT_IDENTIFIER
            Public Version As GET_VIRTUAL_DISK_INFO_VERSION
            <MarshalAs(UnmanagedType.Struct)>
            Public ParentIdentifier As Guid
        End Structure

        Public Declare Function GetVirtualDiskInformation Lib "VirtDisk.dll" _
    (Handle As IntPtr,
     ByRef VirtualDiskInfoSize As UInteger,
     VirtualDiskInfo As GET_VIRTUAL_DISK_INFO_PARENT_IDENTIFIER,
     ByRef SizeUsed As UInteger) As UInteger

        ''

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure GET_VIRTUAL_DISK_INFO_PARENT_TIMESTAMP
            Public Version As GET_VIRTUAL_DISK_INFO_VERSION
            Public ParentTimestamp As UInteger
        End Structure

        Public Declare Function GetVirtualDiskInformation Lib "VirtDisk.dll" _
    (Handle As IntPtr,
     ByRef VirtualDiskInfoSize As UInteger,
     VirtualDiskInfo As GET_VIRTUAL_DISK_INFO_PARENT_TIMESTAMP,
     ByRef SizeUsed As UInteger) As UInteger

        ''

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure GET_VIRTUAL_DISK_INFO_VIRTUAL_STORAGE_TYPE
            Public Version As GET_VIRTUAL_DISK_INFO_VERSION
            Public VirtualStorageType As VIRTUAL_STORAGE_TYPE
        End Structure

        Public Declare Function GetVirtualDiskInformation Lib "VirtDisk.dll" _
    (Handle As IntPtr,
     ByRef VirtualDiskInfoSize As UInteger,
     VirtualDiskInfo As GET_VIRTUAL_DISK_INFO_VIRTUAL_STORAGE_TYPE,
     ByRef SizeUsed As UInteger) As UInteger

        ''

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure GET_VIRTUAL_DISK_INFO_PROVIDER_SUBTYPE
            Public Version As GET_VIRTUAL_DISK_INFO_VERSION
            Public ProviderSubType As UInteger
        End Structure

        Public Declare Function GetVirtualDiskInformation Lib "VirtDisk.dll" _
    (Handle As IntPtr,
     ByRef VirtualDiskInfoSize As UInteger,
     VirtualDiskInfo As GET_VIRTUAL_DISK_INFO_PROVIDER_SUBTYPE,
     ByRef SizeUsed As UInteger) As UInteger

        ''

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure GET_VIRTUAL_DISK_INFO_IS4KALIGNED
            Public Version As GET_VIRTUAL_DISK_INFO_VERSION
            <MarshalAs(UnmanagedType.Bool)>
            Public Is4kAligned As Boolean
        End Structure

        Public Declare Function GetVirtualDiskInformation Lib "VirtDisk.dll" _
    (Handle As IntPtr,
     ByRef VirtualDiskInfoSize As UInteger,
     VirtualDiskInfo As GET_VIRTUAL_DISK_INFO_IS4KALIGNED,
     ByRef SizeUsed As UInteger) As UInteger

        ''

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure GET_VIRTUAL_DISK_INFO_ISLOADED
            Public Version As GET_VIRTUAL_DISK_INFO_VERSION
            <MarshalAs(UnmanagedType.Bool)>
            Public IsLoaded As Boolean
        End Structure

        Public Declare Function GetVirtualDiskInformation Lib "VirtDisk.dll" _
    (Handle As IntPtr,
     ByRef VirtualDiskInfoSize As UInteger,
     VirtualDiskInfo As GET_VIRTUAL_DISK_INFO_ISLOADED,
     ByRef SizeUsed As UInteger) As UInteger

        ''

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure GET_VIRTUAL_DISK_INFO_LOGICAL_SECTOR_SIZE
            Public Version As GET_VIRTUAL_DISK_INFO_VERSION
            Public LogicalSectorSize As UInteger
            Public PhysicalSectorSize As UInteger
            <MarshalAs(UnmanagedType.Bool)>
            Public IsRemote As Boolean
        End Structure

        Public Declare Function GetVirtualDiskInformation Lib "VirtDisk.dll" _
    (Handle As IntPtr,
     ByRef VirtualDiskInfoSize As UInteger,
     VirtualDiskInfo As GET_VIRTUAL_DISK_INFO_LOGICAL_SECTOR_SIZE,
     ByRef SizeUsed As UInteger) As UInteger

        ''

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure GET_VIRTUAL_DISK_INFO_VHD_PHYSICAL_SECTOR_SIZE
            Public Version As GET_VIRTUAL_DISK_INFO_VERSION
            Public VhdPhysicalSectorSize As UInteger
        End Structure

        Public Declare Function GetVirtualDiskInformation Lib "VirtDisk.dll" _
    (Handle As IntPtr,
     ByRef VirtualDiskInfoSize As UInteger,
     VirtualDiskInfo As GET_VIRTUAL_DISK_INFO_VHD_PHYSICAL_SECTOR_SIZE,
     ByRef SizeUsed As UInteger) As UInteger

        ''

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure GET_VIRTUAL_DISK_INFO_SMALLEST_SAFE_VIRTUAL_SIZE
            Public Version As GET_VIRTUAL_DISK_INFO_VERSION
            Public SmallestSafeVirtualSize As ULong
        End Structure

        Public Declare Function GetVirtualDiskInformation Lib "VirtDisk.dll" _
    (Handle As IntPtr,
     ByRef VirtualDiskInfoSize As UInteger,
     VirtualDiskInfo As GET_VIRTUAL_DISK_INFO_SMALLEST_SAFE_VIRTUAL_SIZE,
     ByRef SizeUsed As UInteger) As UInteger

        ''

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure GET_VIRTUAL_DISK_INFO_FRAGMENTATION
            Public Version As GET_VIRTUAL_DISK_INFO_VERSION
            Public FragmentationPercentage As UInteger
        End Structure

        Public Declare Function GetVirtualDiskInformation Lib "VirtDisk.dll" _
        (Handle As IntPtr,
         ByRef VirtualDiskInfoSize As UInteger,
         VirtualDiskInfo As GET_VIRTUAL_DISK_INFO_FRAGMENTATION,
         ByRef SizeUsed As UInteger) As UInteger

        ''

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure GET_VIRTUAL_DISK_INFO_VIRTUAL_DISK_ID
            Public Version As GET_VIRTUAL_DISK_INFO_VERSION
            <MarshalAs(UnmanagedType.Struct)>
            Public VirtualDiskId As Guid
        End Structure

        Public Declare Function GetVirtualDiskInformation Lib "VirtDisk.dll" _
        (Handle As IntPtr,
         ByRef VirtualDiskInfoSize As UInteger,
         VirtualDiskInfo As GET_VIRTUAL_DISK_INFO_VIRTUAL_DISK_ID,
         ByRef SizeUsed As UInteger) As UInteger

        ''
        '' SetVirtualDiskInformation
        ''

        '' Version definitions
        Public Enum SET_VIRTUAL_DISK_INFO_VERSION
            SET_VIRTUAL_DISK_INFO_UNSPECIFIED = 0
            SET_VIRTUAL_DISK_INFO_PARENT_PATH = 1
            SET_VIRTUAL_DISK_INFO_IDENTIFIER = 2
            SET_VIRTUAL_DISK_INFO_PARENT_PATH_WITH_DEPTH = 3
            SET_VIRTUAL_DISK_INFO_PHYSICAL_SECTOR_SIZE = 4
            SET_VIRTUAL_DISK_INFO_VIRTUAL_DISK_ID = 5
        End Enum

        '' Versioned parameter structure for SetVirtualDiskInformation
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure SET_VIRTUAL_DISK_INFO_PARENT_FILE_PATH
            Public Version As SET_VIRTUAL_DISK_INFO_VERSION

            <MarshalAs(UnmanagedType.LPWStr)>
            Public ParentFilePath As String
        End Structure

        Public Declare Function SetVirtualDiskInformation Lib "VirtDisk.dll" _
        (VirtualDiskHandle As IntPtr,
         VirtualDiskInfo As SET_VIRTUAL_DISK_INFO_PARENT_FILE_PATH) As UInteger

        '' Versioned parameter structure for SetVirtualDiskInformation
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure SET_VIRTUAL_DISK_INFO_UNIQUE_ID
            Public Version As SET_VIRTUAL_DISK_INFO_VERSION

            <MarshalAs(UnmanagedType.Struct)>
            Public UniqueIdentifier As Guid
        End Structure

        Public Declare Function SetVirtualDiskInformation Lib "VirtDisk.dll" _
        (VirtualDiskHandle As IntPtr,
         VirtualDiskInfo As SET_VIRTUAL_DISK_INFO_UNIQUE_ID) As UInteger

        '' Versioned parameter structure for SetVirtualDiskInformation
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure SET_VIRTUAL_DISK_INFO_PARENT_PATH_WITH_DEPTH_INFO
            Public Version As SET_VIRTUAL_DISK_INFO_VERSION

            Public ChildDepth As UInteger
            <MarshalAs(UnmanagedType.LPWStr)>
            Public ParentFilePath As String
        End Structure

        Public Declare Function SetVirtualDiskInformation Lib "VirtDisk.dll" _
        (VirtualDiskHandle As IntPtr,
         VirtualDiskInfo As SET_VIRTUAL_DISK_INFO_PARENT_PATH_WITH_DEPTH_INFO) As UInteger

        '' Versioned parameter structure for SetVirtualDiskInformation
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure SET_VIRTUAL_DISK_INFO_VHD_SECTOR_SIZE
            Public Version As SET_VIRTUAL_DISK_INFO_VERSION

            Public VhdPhysicalSectorSize As UInteger
        End Structure

        Public Declare Function SetVirtualDiskInformation Lib "VirtDisk.dll" _
        (VirtualDiskHandle As IntPtr,
         VirtualDiskInfo As SET_VIRTUAL_DISK_INFO_VHD_SECTOR_SIZE) As UInteger

        '' Points to as list of Guids (in Items)
        Public Declare Function EnumerateVirtualDiskMetadata Lib "VirtDisk.dll" _
        (VirtualDiskHandle As IntPtr,
         ByRef NumberOfItems As UInteger,
         Items As IntPtr) As UInteger

        Public Declare Function GetVirtualDiskMetadata Lib "VirtDisk.dll" _
        (VirtualDiskHandle As IntPtr,
         Item As Guid,
         ByRef MetaDataSize As UInteger,
         MetaData As IntPtr) As UInteger

        Public Declare Function SetVirtualDiskMetadata Lib "VirtDisk.dll" _
        (VirtualDiskHandle As IntPtr,
         Item As Guid,
         MetaDataSize As UInteger,
         MetaData As IntPtr) As UInteger

        Public Declare Function DeleteVirtualDiskMetadata Lib "VirtDisk.dll" _
        (VirtualDiskHandle As IntPtr,
         Item As Guid) As UInteger

        '' NTDDI_VERSION >= NTDDI_WIN8

        ''
        '' GetVirtualDiskOperationProgress
        ''

        Public Structure VIRTUAL_DISK_PROGRESS
            Public OperationStatus As UInteger
            Public CurrentValue As ULong
            Public CompletionValue As ULong
        End Structure

        Public Declare Function GetVirtualDiskOperationProgress Lib "VirtDisk.dll" _
        (VirtualDiskHandle As IntPtr,
         Overlapped As IntPtr,
         Progress As VIRTUAL_DISK_PROGRESS) As UInteger

        ''
        '' CompactVirtualDisk
        ''

        '' Version definitions
        Public Enum COMPACT_VIRTUAL_DISK_VERSION

            COMPACT_VIRTUAL_DISK_VERSION_UNSPECIFIED = 0
            COMPACT_VIRTUAL_DISK_VERSION_1 = 1

        End Enum

        '' Versioned structure for CompactVirtualDisk
        Public Structure COMPACT_VIRTUAL_DISK_PARAMETERS
            Public Version As COMPACT_VIRTUAL_DISK_VERSION
            Public Reserved As UInteger
        End Structure

        '' Flags for CompactVirtualDisk
        Public Enum COMPACT_VIRTUAL_DISK_FLAG

            COMPACT_VIRTUAL_DISK_FLAG_NONE = 0
            COMPACT_VIRTUAL_DISK_FLAG_NO_ZERO_SCAN = 1
            COMPACT_VIRTUAL_DISK_FLAG_NO_BLOCK_MOVES = 2

        End Enum

        Public Declare Function CompactVirtualDisk Lib "VirtDisk.dll" _
        (VirtualDiskHandle As IntPtr,
         Flags As COMPACT_VIRTUAL_DISK_FLAG,
         Parameters As COMPACT_VIRTUAL_DISK_PARAMETERS,
         Overlapped As IntPtr) As UInteger

        ''
        '' MergeVirtualDisk
        ''

        '' Version definitions
        Public Enum MERGE_VIRTUAL_DISK_VERSION
            MERGE_VIRTUAL_DISK_VERSION_UNSPECIFIED = 0
            MERGE_VIRTUAL_DISK_VERSION_1 = 1
            MERGE_VIRTUAL_DISK_VERSION_2 = 2
        End Enum

        '' Versioned parameter structure for MergeVirtualDisk
        Public Const MERGE_VIRTUAL_DISK_DEFAULT_MERGE_DEPTH = 1

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure MERGE_VIRTUAL_DISK_PARAMETERS_V1
            Public Version As MERGE_VIRTUAL_DISK_VERSION
            Public MergeDepth As UInteger
        End Structure

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure MERGE_VIRTUAL_DISK_PARAMETERS_V2
            Public Version As MERGE_VIRTUAL_DISK_VERSION
            Public MergeSourceDepth As UInteger
            Public MergeTargetDepth As UInteger
        End Structure

        '' Flags for MergeVirtualDisk
        Public Enum MERGE_VIRTUAL_DISK_FLAG
            MERGE_VIRTUAL_DISK_FLAG_NONE = 0
        End Enum

        Public Declare Function MergeVirtualDisk Lib "VirtDisk.dll" _
        (VirtualDiskHandle As IntPtr,
         Flags As MERGE_VIRTUAL_DISK_FLAG,
         Parameters As MERGE_VIRTUAL_DISK_PARAMETERS_V2,
         Overlapped As IntPtr) As UInteger

        ''
        '' ExpandVirtualDisk
        ''

        '' Version definitions
        Public Enum EXPAND_VIRTUAL_DISK_VERSION

            EXPAND_VIRTUAL_DISK_VERSION_UNSPECIFIED = 0
            EXPAND_VIRTUAL_DISK_VERSION_1 = 1
        End Enum

        '' Versioned parameter structure for ResizeVirtualDisk
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure EXPAND_VIRTUAL_DISK_PARAMETERS
            Public Version As EXPAND_VIRTUAL_DISK_VERSION
            <MarshalAs(UnmanagedType.LPWStr)>
            Public NewSize As ULong
        End Structure

        '' Flags for ExpandVirtualDisk
        Public Enum EXPAND_VIRTUAL_DISK_FLAG
            EXPAND_VIRTUAL_DISK_FLAG_NONE = 0
        End Enum

        Public Declare Function ExpandVirtualDisk Lib "VirtDisk.dll" _
        (VirtualDiskHandle As IntPtr,
         Flags As EXPAND_VIRTUAL_DISK_FLAG,
         Parameters As EXPAND_VIRTUAL_DISK_PARAMETERS,
         Overlapped As IntPtr) As UInteger

        ''
        '' ResizeVirtualDisk
        ''

        '' Version definitions
        Public Enum RESIZE_VIRTUAL_DISK_VERSION

            RESIZE_VIRTUAL_DISK_VERSION_UNSPECIFIED = 0
            RESIZE_VIRTUAL_DISK_VERSION_1 = 1

        End Enum

        '' Versioned parameter structure for ResizeVirtualDisk
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure RESIZE_VIRTUAL_DISK_PARAMETERS
            Public Version As RESIZE_VIRTUAL_DISK_VERSION
            <MarshalAs(UnmanagedType.LPWStr)>
            Public NewSize As ULong
        End Structure

        '' Flags for ResizeVirtualDisk
        Public Enum RESIZE_VIRTUAL_DISK_FLAG

            RESIZE_VIRTUAL_DISK_FLAG_NONE = 0

            '' If this flag is set, skip checking the virtual disk's partition table
            '' to ensure that this truncation is safe. Setting this flag can cause
            '' unrecoverable data loss; use with care.
            RESIZE_VIRTUAL_DISK_FLAG_ALLOW_UNSAFE_VIRTUAL_SIZE = 1

            '' If this flag is set, resize the disk to the smallest virtual size
            '' possible without truncating past any existing partitions. If this
            '' is set, NewSize in RESIZE_VIRTUAL_DISK_PARAMETERS must be zero.
            RESIZE_VIRTUAL_DISK_FLAG_RESIZE_TO_SMALLEST_SAFE_VIRTUAL_SIZE = 2

        End Enum

        Public Declare Function ResizeVirtualDisk Lib "VirtDisk.dll" _
        (VirtualDiskHandle As IntPtr,
         Flags As RESIZE_VIRTUAL_DISK_FLAG,
         Parameters As RESIZE_VIRTUAL_DISK_PARAMETERS,
         Overlapped As IntPtr) As UInteger

        '' NTDDI_VERSION >= NTDDI_WIN8

        ''
        '' MirrorVirtualDisk
        ''

        '' Version definitions
        Public Enum MIRROR_VIRTUAL_DISK_VERSION
            MIRROR_VIRTUAL_DISK_VERSION_UNSPECIFIED = 0
            MIRROR_VIRTUAL_DISK_VERSION_1 = 1
        End Enum

        '' Versioned parameter structure for MirrorVirtualDisk
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure MIRROR_VIRTUAL_DISK_PARAMETERS
            Public Version As MIRROR_VIRTUAL_DISK_VERSION
            <MarshalAs(UnmanagedType.LPWStr)>
            Public MirrorVirtualDiskPath As String
        End Structure

        '' Flags for MirrorVirtualDisk
        Public Enum MIRROR_VIRTUAL_DISK_FLAG

            MIRROR_VIRTUAL_DISK_FLAG_NONE = 0
            MIRROR_VIRTUAL_DISK_FLAG_EXISTING_FILE = 1

        End Enum

        Public Declare Function MirrorVirtualDisk Lib "VirtDisk.dll" _
        (VirtualDiskHandle As IntPtr,
         Flags As MIRROR_VIRTUAL_DISK_FLAG,
         Parameters As MIRROR_VIRTUAL_DISK_PARAMETERS,
         Overlapped As IntPtr) As UInteger

        '' NTDDI_VERSION >= NTDDI_WIN8

        ''
        '' BreakMirrorVirtualDisk
        ''

        Public Declare Function BreakMirrorVirtualDisk Lib "VirtDisk.dll" _
        (VirtualDiskHandle As IntPtr) As UInteger

        '' NTDDI_VERSION >= NTDDI_WIN8

        ''
        '' AddVirtualDiskParent
        ''

        Public Declare Function AddVirtualDiskParent Lib "VirtDisk.dll" _
        (VirtualDiskHandle As IntPtr,
         <MarshalAs(UnmanagedType.LPWStr)> ParentPath As String) As UInteger

        '' END NTDDI_VERSION >= NTDDI_WIN8

        '' END VIRTDISK_DEFINE_FLAGS

    End Module

End Namespace