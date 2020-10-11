'' ************************************************* ''
'' DataTools Visual Basic Utility Library - Interop
''
'' Module: DiskDeviceInfo derived class for disks and
''         volumes.
'' 
'' Copyright (C) 2011-2020 Nathan Moschkin
'' All Rights Reserved
''
'' Licensed Under the Microsoft Public License   
'' ************************************************* ''

Imports CoreCT.Memory
Imports DataTools.Interop.Native
Imports System.ComponentModel
Imports System.Reflection
Imports System.Collections.ObjectModel
Imports System.Windows.Media.Imaging
Imports DataTools.Interop.Printers
Imports DataTools.SystemInformation
Imports DataTools.Interop
Imports CoreCT.Text

Namespace Disk

#Region "DiskDeviceInfo"

    ''' <summary>
    ''' An object that represents a disk or volume device on the system.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class DiskDeviceInfo
        Inherits DeviceInfo

        Protected _PhysicalDevice As Integer
        Protected _PartitionNumber As Integer
        Protected _Type As StorageType
        Protected _Size As Long
        Protected _Capabilities As DeviceCapabilities
        Protected _BackingStore As String()
        Protected _IsVolume As Boolean
        Protected _SerialNumber As UInteger
        Protected _FileSystem As String
        Protected _VolumeFlags As FileSystemFlags
        Protected _VolumeGuidPath As String
        Protected _VolumePaths As String()
        Protected _DiskExtents As DiskExtent()
        Protected _DiskLayout As IDiskLayout
        Protected _PartInfo As IDiskPartition
        Protected _SectorSize As Integer
        Protected _VirtualDrive As VirtualDisk

        ''' <summary>
        ''' Access the virtual disk object (if any).
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property VirtualDisk As VirtualDisk
            Get
                Return _VirtualDrive
            End Get
            Friend Set(value As VirtualDisk)
                _VirtualDrive = value
            End Set
        End Property

        ''' <summary>
        ''' The sector size of this volume, in bytes.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SectorSize As Integer
            Get
                Return _SectorSize
            End Get
            Friend Set(value As Integer)
                _SectorSize = value
            End Set
        End Property

        ''' <summary>
        ''' Returns the disk layout and partition information on a physical disk where Type is not StorageType.Volume.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DiskLayout As IDiskLayout
            Get
                Return _DiskLayout
            End Get
            Friend Set(value As IDiskLayout)
                _DiskLayout = value
            End Set
        End Property

        ''' <summary>
        ''' Returns partition information for a DiskDeviceInfo object whose Type = StorageType.Volume
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PartitionInfo As IDiskPartition
            Get
                Dim p() As PARTITION_INFORMATION_EX
                If Me.Type <> StorageType.Volume Then Return Nothing
                If _PartInfo Is Nothing Then

                    Try
                        If Not IsVolumeMounted Then Return Nothing
                        p = GetPartitions("\\.\PhysicalDrive" & PhysicalDevice)
                    Catch ex As Exception
                        Return Nothing
                    End Try

                    If p IsNot Nothing Then
                        For Each x In p
                            If x.PartitionNumber = Me.PartitionNumber Then
                                _PartInfo = DiskPartitionInfo.CreateInfo(x)
                                Exit For
                            End If
                        Next
                    End If
                End If

                Return _PartInfo
            End Get
            Friend Set(value As IDiskPartition)
                _PartInfo = value
            End Set
        End Property

        ''' <summary>
        ''' The physical disk drive number.
        ''' </summary>
        ''' <remarks></remarks>
        Public Property PhysicalDevice As Integer
            Get
                Return _PhysicalDevice
            End Get
            Friend Set(value As Integer)
                _PhysicalDevice = value
            End Set
        End Property

        ''' <summary>
        ''' If applicable, the partition number of the device.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PartitionNumber As Integer
            Get
                Return _PartitionNumber
            End Get
            Friend Set(value As Integer)
                _PartitionNumber = value
            End Set
        End Property

        ''' <summary>
        ''' Total capacity of storage device.
        ''' </summary>
        ''' <remarks></remarks>
        Public Property Size As FriendlySizeLong
            Get
                If Type <> StorageType.Volume OrElse String.IsNullOrEmpty(VolumeGuidPath) Then Return _Size

                Dim a As ULong,
                b As ULong,
                c As ULong

                Try
                    If Not IsVolumeMounted Then Return 0
                    GetDiskFreeSpaceEx(Me.VolumeGuidPath, a, b, c)
                Catch ex As Exception
                    Return 0
                End Try
                Return b
            End Get
            Friend Set(value As FriendlySizeLong)
                _Size = value
            End Set
        End Property

        ''' <summary>
        ''' Available capacity of volume.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property SizeFree As FriendlySizeLong
            Get
                If Type <> StorageType.Volume OrElse String.IsNullOrEmpty(VolumeGuidPath) Then Return _Size

                Dim a As ULong,
                b As ULong,
                c As ULong

                Try
                    If Not IsVolumeMounted Then Return 0
                    GetDiskFreeSpaceEx(Me.VolumeGuidPath, a, b, c)
                Catch ex As Exception
                    Return 0
                End Try
                Return c
            End Get
        End Property

        ''' <summary>
        ''' The total used space of the volume.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property SizeUsed As FriendlySizeLong
            Get
                If Type <> StorageType.Volume OrElse String.IsNullOrEmpty(VolumeGuidPath) Then Return _Size

                Dim a As ULong,
                b As ULong,
                c As ULong

                Try
                    If Not IsVolumeMounted Then Return 0
                    GetDiskFreeSpaceEx(Me.VolumeGuidPath, a, b, c)
                    Return b - c
                Catch ex As Exception

                    Return 0
                End Try
            End Get
        End Property

        ''' <summary>
        ''' TYpe of storage.
        ''' </summary>
        ''' <remarks></remarks>
        Public Property Type As StorageType
            Get
                Return _Type
            End Get
            Friend Set(value As StorageType)
                _Type = value
            End Set
        End Property

        ''' <summary>
        ''' Physical device capabilities.
        ''' </summary>
        ''' <remarks></remarks>
        Public Property Capabilities As DeviceCapabilities
            Get
                Capabilities = _Capabilities
            End Get
            Friend Set(value As DeviceCapabilities)
                _Capabilities = value
            End Set
        End Property
        ''' <summary>
        ''' Contains a list of VHD/VHDX files that make up a virtual hard drive.
        ''' </summary>
        ''' <remarks></remarks>
        Public Property BackingStore As String()
            Get
                BackingStore = _BackingStore
            End Get
            Friend Set(value As String())
                _BackingStore = value
            End Set
        End Property
        '' Volume information

        ''' <summary>
        ''' Indicates whether or not this structure refers to a volume or a device.
        ''' </summary>
        ''' <remarks></remarks>
        Public Property IsVolume As Boolean
            Get
                Return _IsVolume
            End Get
            Friend Set(value As Boolean)
                _IsVolume = value
            End Set
        End Property

        ''' <summary>
        ''' Returns a value indicating whether this volume is mounted, if it represents a volume of a removeable drive.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property IsVolumeMounted As Boolean
            Get
                Return GetDriveFlag() <> 0 And GetLogicalDrives <> 0
            End Get
        End Property

        ''' <summary>
        ''' Returns the current logical drive flag.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function GetDriveFlag() As UInteger
            If Not IsVolume Then Return 0

            If _VolumePaths Is Nothing OrElse _VolumePaths.Length = 0 Then Return 0

            Dim ch As Char = "-"c
            Dim vl As UInteger = 0

            For Each vp In _VolumePaths
                If vp.Length <= 3 Then
                    ch = vp.ToCharArray()(0)
                    Exit For
                End If
            Next

            If ch = "-"c Then Return 0

            ch = ch.ToString.ToUpper.Chars(0)

            vl = CUInt((AscW(ch) - AscW("A")))

            Return CUInt(1 << CInt(vl))

        End Function

        ''' <summary>
        ''' The volume serial number.
        ''' </summary>
        ''' <remarks></remarks>
        Public Property SerialNumber As UInteger
            Get
                SerialNumber = _SerialNumber
            End Get
            Friend Set(value As UInteger)
                _SerialNumber = value
            End Set
        End Property

        ''' <summary>
        ''' The name of the file system for this volume.
        ''' </summary>
        ''' <remarks></remarks>
        Public Property FileSystem As String
            Get
                FileSystem = _FileSystem
            End Get
            Friend Set(value As String)
                _FileSystem = value
            End Set
        End Property

        ''' <summary>
        ''' Volume flags and capabilities.
        ''' </summary>
        ''' <remarks></remarks>
        Public Property VolumeFlags As FileSystemFlags
            Get
                VolumeFlags = _VolumeFlags
            End Get
            Friend Set(value As FileSystemFlags)
                _VolumeFlags = value
            End Set
        End Property

        ''' <summary>
        ''' The Volume GUID (parsing) path.  This member can be used in a call to CreateFile for DeviceIoControl (for volumes).
        ''' </summary>
        ''' <remarks></remarks>
        Public Property VolumeGuidPath As String
            Get
                VolumeGuidPath = _VolumeGuidPath
            End Get
            Set(value As String)
                _VolumeGuidPath = value
            End Set
        End Property
        ''' <summary>
        ''' A list of all mount-points for the volume
        ''' </summary>
        ''' <remarks></remarks>
        Public Property VolumePaths As String()
            Get
                VolumePaths = _VolumePaths
            End Get
            Set(value As String())
                _VolumePaths = value
            End Set
        End Property

        ''' <summary>
        ''' Partition locations on the physical disk or disks.
        ''' </summary>
        ''' <remarks></remarks>
        Public Property DiskExtents As DiskExtent()
            Get
                DiskExtents = _DiskExtents
            End Get
            Friend Set(value As DiskExtent())
                _DiskExtents = value
            End Set
        End Property

        ''' <summary>
        ''' Print friendly device information, including friendly name, mount points and the device's friendly size.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function ToString() As String
            If IsVolume Then
                If VolumePaths IsNot Nothing AndAlso VolumePaths.Count > 0 Then

                    Dim slist = String.Join(", ", VolumePaths)

                    ToString = "[" & slist & "] "
                Else
                    ToString = ""
                End If
                ToString &= FriendlyName & " (" & TextTools.PrintFriendlySize(Size) & ")"
            Else
                ToString = "[" & Type.ToString & "] " & FriendlyName & " (" & TextTools.PrintFriendlySize(Size) & ")"
            End If

        End Function

    End Class

#End Region

End Namespace