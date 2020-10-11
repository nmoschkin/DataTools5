'' ************************************************* ''
'' DataTools Visual Basic Utility Library - Interop
''
'' Module: VirtualDisk
''         Create, Mount, and Unmount .vhd and .vhdx
''         Virtual Disks.
'' 
'' Copyright (C) 2011-2020 Nathan Moschkin
'' All Rights Reserved
''
'' Licensed Under the Microsoft Public License   
'' ************************************************* ''

Imports System
Imports System.IO
Imports System.Runtime.InteropServices
Imports DataTools.Interop.Native
Imports CoreCT.Memory
Imports CoreCT.Text

Namespace Disk

    ''' <summary>
    ''' Virtual storage type.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum VirtualStorageType As Integer

        ''' <summary>
        ''' Unknown
        ''' </summary>
        ''' <remarks></remarks>
        Unknown = 0

        ''' <summary>
        ''' CD-ROM/DVD-ROM ISO image file.
        ''' </summary>
        ''' <remarks></remarks>
        ISO = 1

        ''' <summary>
        ''' Virtual drive (Windows 7)
        ''' </summary>
        ''' <remarks></remarks>
        VHD = 2

        ''' <summary>
        ''' Virtual drive (Windows 8+)
        ''' </summary>
        ''' <remarks></remarks>
        VHDX = 3

    End Enum

    ''' <summary>
    ''' Encapsulates a virtual disk device (iso, vhd, or vhdx).
    ''' </summary>
    ''' <remarks></remarks>
    Public Class VirtualDisk

        Friend _info As DiskDeviceInfo
        Friend _Handle As IntPtr = IntPtr.Zero
        Private _ImageFile As String

        ''' <summary>
        ''' Indicates whether the virtual drive is attached.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Attached As Boolean
            Get
                Return (DevicePath <> "")
            End Get
        End Property

        ''' <summary>
        ''' Indicates whether the virtual drive is open.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property IsOpen As Boolean
            Get
                Return (_Handle <> IntPtr.Zero)
            End Get
        End Property

        ''' <summary>
        ''' Displays drive information.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function ToString() As String

            ToString = TextTools.PrintFriendlySize(Size) & " Virtual Drive [" & If(Attached, "Attached", "Not Attached") & "]"

        End Function

        ''' <summary>
        ''' Returns the actual physical size of the the virtual drive, as stored on the primary media.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property PhysicalSize As Long
            Get
                Dim info As GET_VIRTUAL_DISK_INFO_SIZE
                Dim iSize As UInteger,
                sizeUSed As UInteger

                Dim mm As New MemPtr
                info.Version = GET_VIRTUAL_DISK_INFO_VERSION.GET_VIRTUAL_DISK_INFO_SIZE
                iSize = CUInt(Marshal.SizeOf(Of GET_VIRTUAL_DISK_INFO_SIZE))

                mm.FromStruct(info)

                Dim r As UInteger =
                GetVirtualDiskInformation(_Handle, iSize, mm, sizeUSed)

                info = mm.ToStruct(Of GET_VIRTUAL_DISK_INFO_SIZE)
                PhysicalSize = CLng(info.PhysicalSize)
                mm.Free()
            End Get
        End Property

        ''' <summary>
        ''' Returns the virtual size of the drive.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Size As Long
            Get
                Dim info As GET_VIRTUAL_DISK_INFO_SIZE
                Dim iSize As UInteger,
                sizeUSed As UInteger

                Dim mm As New MemPtr
                info.Version = GET_VIRTUAL_DISK_INFO_VERSION.GET_VIRTUAL_DISK_INFO_SIZE
                iSize = CUInt(Marshal.SizeOf(info))

                mm.FromStruct(Of GET_VIRTUAL_DISK_INFO_SIZE)(info)

                Dim r As UInteger =
                GetVirtualDiskInformation(_Handle, iSize, mm, sizeUSed)

                info = mm.ToStruct(Of GET_VIRTUAL_DISK_INFO_SIZE)
                Size = CLng(info.VirtualSize)
                mm.Free()

            End Get
        End Property

        ''' <summary>
        ''' Returns the physical hardware path of the device.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property PhysicalPath As String
            Get
                Dim szpath As UInteger = MAX_PATH
                Dim mm As New MemPtr

                mm.Alloc(MAX_PATH * 2)
                Dim r As UInteger = GetVirtualDiskPhysicalPath(_Handle, szpath, mm.Handle)
                PhysicalPath = mm.ToString
                mm.Free()
            End Get
        End Property

        ''' <summary>
        ''' Returns the unique Guid identifier of the virtual drive.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Identifier As Guid
            Get
                Dim info As GET_VIRTUAL_DISK_INFO_SIZE
                Dim iSize As UInteger,
                sizeUSed As UInteger

                Dim mm As New MemPtr
                info.Version = GET_VIRTUAL_DISK_INFO_VERSION.GET_VIRTUAL_DISK_INFO_IDENTIFIER
                iSize = CUInt(Marshal.SizeOf(Of GET_VIRTUAL_DISK_INFO_SIZE))

                mm.FromStruct(Of GET_VIRTUAL_DISK_INFO_SIZE)(info)

                Dim r As UInteger =
                GetVirtualDiskInformation(_Handle, iSize, mm, sizeUSed)

                Identifier = mm.GuidAtAbsolute(8)
                mm.Free()
            End Get
        End Property

        ''' <summary>
        ''' Returns the handle to the open virtual disk.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Handle As IntPtr
            Get
                Return _Handle
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets the VHD/VHDX/ISO image file for the current object.
        ''' If set, any current image is closed.
        ''' </summary>
        ''' <returns></returns>
        Public Property ImageFile As String
            Get
                Return _ImageFile
            End Get
            Set(value As String)
                If Me.Open Then Me.Close()
                _ImageFile = value
            End Set
        End Property

        ''' <summary>
        ''' Creates a new virtual disk device file.
        ''' </summary>
        ''' <param name="imageFile">Full path to the destination image, including file extension (.vhd or .vhdx)</param>
        ''' <param name="diskSize">The virtual size of the new disk.</param>
        ''' <param name="fixedSize">Indicates whether or not the size is fixed or virtual.</param>
        ''' <param name="id">Receives the Guid for the new drive.</param>
        ''' <param name="resiliencyId">The Resiliancy Guid.</param>
        ''' <param name="blockSize">The transfer block size.</param>
        ''' <param name="sectorSize">The virtual sector size.</param>
        ''' <param name="sourcePath">The cloning source (if any).</param>
        ''' <param name="sourceDiskType">The cloning source disk type (if applicable).</param>
        ''' <returns>A new VirtualDisk object.</returns>
        ''' <remarks></remarks>
        Public Shared Function Create(imageFile As String,
                                    diskSize As Long,
                                    fixedSize As Boolean,
                                    ByRef id As Guid,
                                    Optional ByRef resiliencyId As Guid = Nothing,
                                    Optional blockSize As Integer = 2097152,
                                    Optional sectorSize As Integer = 512,
                                    Optional sourcePath As String = Nothing,
                                    Optional sourceDiskType As VirtualStorageType = VirtualStorageType.Unknown) As VirtualDisk

            Dim ext As String = Path.GetExtension(imageFile).ToLower

            Dim cp2 As CREATE_VIRTUAL_DISK_PARAMETERS_V2 = New CREATE_VIRTUAL_DISK_PARAMETERS_V2,
            cp1 As CREATE_VIRTUAL_DISK_PARAMETERS_V1 = New CREATE_VIRTUAL_DISK_PARAMETERS_V1

            Dim vst As VIRTUAL_STORAGE_TYPE

            Dim r As UInteger
            Dim handleNew As IntPtr

            vst.VendorId = VIRTUAL_STORAGE_TYPE_VENDOR_MICROSOFT

            Select Case ext

                Case ".vhd"
                    cp1.BlockSizeInBytes = CUInt(blockSize)
                    cp1.Version = CREATE_VIRTUAL_DISK_VERSION.CREATE_VIRTUAL_DISK_VERSION_1
                    cp1.MaximumSize = CULng(diskSize)
                    cp1.UniqueId = id
                    cp1.SourcePath = sourcePath
                    cp1.SectorSizeInBytes = 512
                    vst.DeviceId = VIRTUAL_STORAGE_TYPE_DEVICE_VHD

                    r = CreateVirtualDisk(vst,
                                      imageFile,
                                      VIRTUAL_DISK_ACCESS_MASK.VIRTUAL_DISK_ACCESS_ALL,
                                      IntPtr.Zero,
                                      If(fixedSize, CREATE_VIRTUAL_DISK_FLAGS.CREATE_VIRTUAL_DISK_FLAG_FULL_PHYSICAL_ALLOCATION, CREATE_VIRTUAL_DISK_FLAGS.CREATE_VIRTUAL_DISK_FLAG_NONE),
                                      0,
                                      cp1,
                                      IntPtr.Zero,
                                      handleNew)

                Case ".vhdx"
                    cp2.BlockSizeInBytes = CUInt(blockSize)
                    cp2.Version = CREATE_VIRTUAL_DISK_VERSION.CREATE_VIRTUAL_DISK_VERSION_2
                    cp2.MaximumSize = CULng(diskSize)
                    cp2.UniqueId = id
                    cp2.SourcePath = sourcePath
                    If sourcePath IsNot Nothing Then
                        cp2.SourceVirtualStorageType.DeviceId = CUInt(sourceDiskType)
                        cp2.SourceVirtualStorageType.VendorId = VIRTUAL_STORAGE_TYPE_VENDOR_MICROSOFT
                    End If
                    Dim x As Integer = Marshal.SizeOf(cp2)

                    cp2.SectorSizeInBytes = sectorSize

                    If resiliencyId <> Guid.Empty Then
                        cp2.ResiliencyGuid = resiliencyId
                        'Else
                        '    cp2.ResiliencyGuid = Guid.NewGuid
                        '    resiliencyId = cp2.ResiliencyGuid
                    End If

                    cp2.OpenFlags = OPEN_VIRTUAL_DISK_FLAG.OPEN_VIRTUAL_DISK_FLAG_NONE
                    vst.DeviceId = VIRTUAL_STORAGE_TYPE_DEVICE_VHDX

                    r = CreateVirtualDisk(vst,
                                      imageFile,
                                      VIRTUAL_DISK_ACCESS_MASK.VIRTUAL_DISK_ACCESS_NONE,
                                      IntPtr.Zero,
                                      If(fixedSize, CREATE_VIRTUAL_DISK_FLAGS.CREATE_VIRTUAL_DISK_FLAG_FULL_PHYSICAL_ALLOCATION, CREATE_VIRTUAL_DISK_FLAGS.CREATE_VIRTUAL_DISK_FLAG_NONE),
                                      0,
                                      cp2,
                                      IntPtr.Zero,
                                      handleNew)


            End Select

            If r <> 0 Then
                Create = Nothing
            Else
                Create = New VirtualDisk
                Create._Handle = handleNew
                Create._ImageFile = imageFile
            End If

        End Function

        ''' <summary>
        ''' Open the virtual disk.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Open() As Boolean
            Open = Open(False)
        End Function

        ''' <summary>
        ''' Open the virtual disk.
        ''' </summary>
        ''' <param name="openReadOnly">Open as read-only.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Open(openReadOnly As Boolean) As Boolean
            Open = Open(_ImageFile, openReadOnly)
        End Function

        ''' <summary>
        ''' Open the specified virtual disk image file.
        ''' </summary>
        ''' <param name="imageFile">The vhd or vhdx file to open.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Open(imageFile As String) As Boolean
            Open = Open(imageFile, False)
        End Function

        ''' <summary>
        ''' Open the specified disk image file.
        ''' </summary>
        ''' <param name="imageFile">The vhd or vhdx file to open.</param>
        ''' <param name="openReadOnly">Open as read-only.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Open(imageFile As String, openReadOnly As Boolean) As Boolean

            Dim ext As String = Path.GetExtension(imageFile).ToLower
            Dim vst As VIRTUAL_STORAGE_TYPE
            If _Handle <> IntPtr.Zero Then Close()
            Dim h As Long = 0
            Dim vdp1 As New OPEN_VIRTUAL_DISK_PARAMETERS_V1
            Dim vdp2 As New OPEN_VIRTUAL_DISK_PARAMETERS_V2
            Dim r As UInteger

            Dim am As VIRTUAL_DISK_ACCESS_MASK = VIRTUAL_DISK_ACCESS_MASK.VIRTUAL_DISK_ACCESS_GET_INFO Or VIRTUAL_DISK_ACCESS_MASK.VIRTUAL_DISK_ACCESS_DETACH

            If Not openReadOnly Then
                am = am Or VIRTUAL_DISK_ACCESS_MASK.VIRTUAL_DISK_ACCESS_ATTACH_RW
            End If

            vdp2.Version = OPEN_VIRTUAL_DISK_VERSION.OPEN_VIRTUAL_DISK_VERSION_2
            vdp2.ResiliencyGuid = Guid.NewGuid

            vdp2.ReadOnly = False
            vdp2.GetInfoOnly = False

            vdp1.RWDepth = 1
            vdp1.Version = OPEN_VIRTUAL_DISK_VERSION.OPEN_VIRTUAL_DISK_VERSION_1

            vst.VendorId = VIRTUAL_STORAGE_TYPE_VENDOR_MICROSOFT

            Select Case ext.ToLower

                Case ".vhd"
                    vst.DeviceId = VIRTUAL_STORAGE_TYPE_DEVICE_VHD

                    r = OpenVirtualDisk(vst,
                                    imageFile,
                                    am,
                                    OPEN_VIRTUAL_DISK_FLAG.OPEN_VIRTUAL_DISK_FLAG_NONE,
                                    vdp1,
                                    _Handle)

                Case ".vhdx"
                    vst.DeviceId = VIRTUAL_STORAGE_TYPE_DEVICE_VHDX

                    r = OpenVirtualDisk(vst,
                                    imageFile,
                                    am,
                                    OPEN_VIRTUAL_DISK_FLAG.OPEN_VIRTUAL_DISK_FLAG_NONE,
                                    vdp1,
                                    _Handle)

                Case Else

                    Return False
            End Select

            If r = 0 Then
                _ImageFile = imageFile
            End If

            Open = r = 0

        End Function

        ''' <summary>
        ''' Mount the virtual drive.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Attach() As Boolean
            Attach = Attach(True)
        End Function

        ''' <summary>
        ''' Mount the virtual drive, permanently.  The virtual drive will stay mounted beyond the lifetime of this instance.
        ''' </summary>
        ''' <param name="makePermanent"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Attach(makePermanent As Boolean) As Boolean
            If _Handle = IntPtr.Zero OrElse Attached Then Return False
            Dim sdsize As UInteger = 0
            Dim mm As New MemPtr(8)

            mm.ByteAt(0) = 1
            Dim r As UInteger = AttachVirtualDisk(_Handle, IntPtr.Zero, If(makePermanent, ATTACH_VIRTUAL_DISK_FLAG.ATTACH_VIRTUAL_DISK_FLAG_PERMANENT_LIFETIME, ATTACH_VIRTUAL_DISK_FLAG.ATTACH_VIRTUAL_DISK_FLAG_NONE),
                                                  0, mm.Handle, IntPtr.Zero)
            mm.Free()
            Attach = r = 0

        End Function

        ''' <summary>
        ''' Dismount the drive.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Detach() As Boolean
            If Not Attached Then Return False
            Dim r As UInteger = DetachVirtualDisk(_Handle, DETACH_VIRTUAL_DISK_FLAG.DETACH_VIRTUAL_DISK_FLAG_NONE, 0)
            Detach = r = 0
        End Function

        ''' <summary>
        ''' Close the disk instance.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Close() As Boolean
            If _Handle = IntPtr.Zero Then Return False
            Dim r = CloseHandle(_Handle)
            If r Then _Handle = IntPtr.Zero
            Return r
        End Function

        ''' <summary>
        ''' Initialize the virtual disk object with an existing virtual disk.
        ''' </summary>
        ''' <param name="imageFile">The disk image file to load.</param>
        ''' <remarks></remarks>
        Public Sub New(imageFile As String)
            Me.New(imageFile, False, False)
        End Sub

        ''' <summary>
        ''' Initialize the virtual disk object with an existing virtual disk, and optionally open the disk.
        ''' </summary>
        ''' <param name="imageFile">The disk image file to load.</param>
        ''' <param name="openDisk">Whether or not to open the disk.</param>
        ''' <remarks></remarks>
        Public Sub New(imageFile As String, openDisk As Boolean)
            Me.New(imageFile, openDisk, False)
        End Sub

        ''' <summary>
        ''' Initialize the virtual disk object with an existing virtual disk, and optionally open the disk.
        ''' </summary>
        ''' <param name="imageFile">The disk image file to load.</param>
        ''' <param name="openDisk">Whether or not to open the disk.</param>
        ''' <param name="openReadOnly">Whether or not to open the disk read-only.</param>
        ''' <remarks></remarks>
        Public Sub New(imageFile As String, openDisk As Boolean, openReadOnly As Boolean)
            _ImageFile = imageFile
            If openDisk Then Open(openReadOnly)
        End Sub

        ''' <summary>
        ''' Initializes a new instance of a virtual drive with the specified DiskDeviceInfo object.
        ''' </summary>
        ''' <param name="inf">DiskDeviceInfo object.</param>
        ''' <param name="open">Whether or not to open the drive.</param>
        ''' <remarks></remarks>
        Public Sub New(inf As DiskDeviceInfo, open As Boolean)
            Me.New(inf.BackingStore(0), open)
            _info = inf
        End Sub

        ''' <summary>
        ''' Returns the DeviceCapabilities enumeration.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Capabilities As DeviceCapabilities
            Get
                Return _info.Capabilities
            End Get
        End Property

        ''' <summary>
        ''' Returns the device friendly name.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property FriendlyName As String
            Get
                Return _info.FriendlyName
            End Get
        End Property

        ''' <summary>
        ''' Returns the PhysicalDriveX number.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property PhysicalDevice As Integer
            Get
                Return _info.PhysicalDevice
            End Get
        End Property

        ''' <summary>
        ''' Returns the device path.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property DevicePath As String
            Get
                Return _info.DevicePath
            End Get
        End Property

        ''' <summary>
        ''' Returns the backing store paths.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property BackingStore As String()
            Get
                Return _info.BackingStore
            End Get
        End Property

        Public Property Tag As Object

        ''' <summary>
        ''' Returns the storage device type.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Type As StorageType
            Get
                Return StorageType.Virtual
            End Get
        End Property

        ''' <summary>
        ''' Returns the hardware instance id.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property InstanceId As String
            Get
                Return _info.InstanceId
            End Get
        End Property

        ''' <summary>
        ''' Returns the device type.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property DeviceType As DeviceType
            Get
                Return DeviceType.Disk
            End Get
        End Property

        ''' <summary>
        ''' Tests the string object for equality.
        ''' </summary>
        ''' <param name="obj"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function Equals(obj As Object) As Boolean
            If obj Is Nothing Then Return False
            Return obj.ToString = ToString()
        End Function


#Region "IDisposable Pattern"
        Private disposedValue As Boolean

        ''' <summary>
        ''' Dispose of the current instance and close all handles.
        ''' </summary>
        ''' <param name="disposing"></param>
        ''' <remarks></remarks>
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then

                End If
                Close()
            End If
            Me.disposedValue = True
        End Sub

        Protected Overrides Sub Finalize()
            Dispose(False)
            MyBase.Finalize()
        End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose()
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub

#End Region

        Private Sub New()

        End Sub

    End Class

End Namespace