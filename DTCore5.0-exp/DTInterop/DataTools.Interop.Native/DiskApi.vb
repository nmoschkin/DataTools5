'' ************************************************* ''
'' DataTools Visual Basic Utility Library - Interop
''
'' Module: DiskApi
''         Native Disk Serivces.
'' 
'' Copyright (C) 2011-2020 Nathan Moschkin
'' All Rights Reserved
''
'' Licensed Under the Microsoft Public License   
'' ************************************************* ''


Imports System.ComponentModel
Imports System.Runtime.InteropServices
Imports System.Security
Imports System.Security.Permissions
Imports System.Security.Principal
Imports DataTools.SystemInformation
Imports CoreCT.Memory
Imports DataTools.Interop.Native
Imports DataTools.Interop.Disk
Imports CoreCT.Text

Namespace Native

    <HideModuleName>
    <SecurityCritical()>
    Friend Module DiskApi

#Region "DeviceIoControl"

        Public Const METHOD_BUFFERED = 0
        Public Const METHOD_IN_DIRECT = 1
        Public Const METHOD_OUT_DIRECT = 2
        Public Const METHOD_NEITHER = 3

        Public Const IOCTL_STORAGE_BASE = &H2D

        Public Const FILE_ANY_ACCESS = 0
        Public Const FILE_SPECIAL_ACCESS = (FILE_ANY_ACCESS)
        Public Const FILE_READ_ACCESS = (1)    ' file & pipe
        Public Const FILE_WRITE_ACCESS = (2)    ' file & pipe

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure CTL_CODE
            Public Value As UInteger

            Public ReadOnly Property DeviceType As UInteger
                Get
                    Return CUInt(Value And &HFFFF0000) >> 16
                End Get
            End Property

            Public ReadOnly Property Method As UInteger
                Get
                    Return Value And CUInt(3)
                End Get
            End Property

            Public Sub New(DeviceType As UInteger, [Function] As UInteger, Method As UInteger, Access As UInteger)
                Value = (DeviceType << 16) Or (Access << 14) Or ([Function] << 2) Or Method
            End Sub

            Public Overrides Function ToString() As String
                Return Value.ToString
            End Function

            Public Shared Narrowing Operator CType(operand As UInteger) As CTL_CODE
                Dim c As CTL_CODE
                c.Value = operand
                Return c
            End Operator

            Public Shared Widening Operator CType(operand As CTL_CODE) As UInteger
                Return operand.Value
            End Operator

        End Structure

#Region "IOCTL_STORAGE"

        Public ReadOnly IOCTL_STORAGE_CHECK_VERIFY As New CTL_CODE(IOCTL_STORAGE_BASE, &H200, METHOD_BUFFERED, FILE_READ_ACCESS)
        Public ReadOnly IOCTL_STORAGE_CHECK_VERIFY2 As New CTL_CODE(IOCTL_STORAGE_BASE, &H200, METHOD_BUFFERED, FILE_ANY_ACCESS)
        Public ReadOnly IOCTL_STORAGE_MEDIA_REMOVAL As New CTL_CODE(IOCTL_STORAGE_BASE, &H201, METHOD_BUFFERED, FILE_READ_ACCESS)
        Public ReadOnly IOCTL_STORAGE_EJECT_MEDIA As New CTL_CODE(IOCTL_STORAGE_BASE, &H202, METHOD_BUFFERED, FILE_READ_ACCESS)
        Public ReadOnly IOCTL_STORAGE_LOAD_MEDIA As New CTL_CODE(IOCTL_STORAGE_BASE, &H203, METHOD_BUFFERED, FILE_READ_ACCESS)
        Public ReadOnly IOCTL_STORAGE_LOAD_MEDIA2 As New CTL_CODE(IOCTL_STORAGE_BASE, &H203, METHOD_BUFFERED, FILE_ANY_ACCESS)
        Public ReadOnly IOCTL_STORAGE_RESERVE As New CTL_CODE(IOCTL_STORAGE_BASE, &H204, METHOD_BUFFERED, FILE_READ_ACCESS)
        Public ReadOnly IOCTL_STORAGE_RELEASE As New CTL_CODE(IOCTL_STORAGE_BASE, &H205, METHOD_BUFFERED, FILE_READ_ACCESS)
        Public ReadOnly IOCTL_STORAGE_FIND_NEW_DEVICES As New CTL_CODE(IOCTL_STORAGE_BASE, &H206, METHOD_BUFFERED, FILE_READ_ACCESS)

        Public ReadOnly IOCTL_STORAGE_EJECTION_CONTROL As New CTL_CODE(IOCTL_STORAGE_BASE, &H250, METHOD_BUFFERED, FILE_ANY_ACCESS)
        Public ReadOnly IOCTL_STORAGE_MCN_CONTROL As New CTL_CODE(IOCTL_STORAGE_BASE, &H251, METHOD_BUFFERED, FILE_ANY_ACCESS)

        Public ReadOnly IOCTL_STORAGE_GET_MEDIA_TYPES As New CTL_CODE(IOCTL_STORAGE_BASE, &H300, METHOD_BUFFERED, FILE_ANY_ACCESS)
        Public ReadOnly IOCTL_STORAGE_GET_MEDIA_TYPES_EX As New CTL_CODE(IOCTL_STORAGE_BASE, &H301, METHOD_BUFFERED, FILE_ANY_ACCESS)
        Public ReadOnly IOCTL_STORAGE_GET_MEDIA_SERIAL_NUMBER As New CTL_CODE(IOCTL_STORAGE_BASE, &H304, METHOD_BUFFERED, FILE_ANY_ACCESS)
        Public ReadOnly IOCTL_STORAGE_GET_HOTPLUG_INFO As New CTL_CODE(IOCTL_STORAGE_BASE, &H305, METHOD_BUFFERED, FILE_ANY_ACCESS)
        Public ReadOnly IOCTL_STORAGE_SET_HOTPLUG_INFO As New CTL_CODE(IOCTL_STORAGE_BASE, &H306, METHOD_BUFFERED, FILE_READ_ACCESS Or FILE_WRITE_ACCESS)

        Public ReadOnly IOCTL_STORAGE_RESET_BUS As New CTL_CODE(IOCTL_STORAGE_BASE, &H400, METHOD_BUFFERED, FILE_READ_ACCESS)
        Public ReadOnly IOCTL_STORAGE_RESET_DEVICE As New CTL_CODE(IOCTL_STORAGE_BASE, &H401, METHOD_BUFFERED, FILE_READ_ACCESS)
        Public ReadOnly IOCTL_STORAGE_BREAK_RESERVATION As New CTL_CODE(IOCTL_STORAGE_BASE, &H405, METHOD_BUFFERED, FILE_READ_ACCESS)
        Public ReadOnly IOCTL_STORAGE_PERSISTENT_RESERVE_IN As New CTL_CODE(IOCTL_STORAGE_BASE, &H406, METHOD_BUFFERED, FILE_READ_ACCESS)
        Public ReadOnly IOCTL_STORAGE_PERSISTENT_RESERVE_OUT As New CTL_CODE(IOCTL_STORAGE_BASE, &H407, METHOD_BUFFERED, FILE_READ_ACCESS Or FILE_WRITE_ACCESS)

        Public ReadOnly IOCTL_STORAGE_GET_DEVICE_NUMBER As New CTL_CODE(IOCTL_STORAGE_BASE, &H420, METHOD_BUFFERED, FILE_ANY_ACCESS)
        Public ReadOnly IOCTL_STORAGE_PREDICT_FAILURE As New CTL_CODE(IOCTL_STORAGE_BASE, &H440, METHOD_BUFFERED, FILE_ANY_ACCESS)
        Public ReadOnly IOCTL_STORAGE_FAILURE_PREDICTION_CONFIG As New CTL_CODE(IOCTL_STORAGE_BASE, &H441, METHOD_BUFFERED, FILE_ANY_ACCESS)
        Public ReadOnly IOCTL_STORAGE_READ_CAPACITY As New CTL_CODE(IOCTL_STORAGE_BASE, &H450, METHOD_BUFFERED, FILE_READ_ACCESS)

        ''
        '' IOCTLs &H0463 to &H0468 reserved for dependent disk support.
        ''


        ''
        '' IOCTLs &H0470 to &H047f reserved for device and stack telemetry interfaces
        ''

        Public ReadOnly IOCTL_STORAGE_GET_DEVICE_TELEMETRY As New CTL_CODE(IOCTL_STORAGE_BASE, &H470, METHOD_BUFFERED, FILE_READ_ACCESS Or FILE_WRITE_ACCESS)
        Public ReadOnly IOCTL_STORAGE_DEVICE_TELEMETRY_NOTIFY As New CTL_CODE(IOCTL_STORAGE_BASE, &H471, METHOD_BUFFERED, FILE_READ_ACCESS Or FILE_WRITE_ACCESS)
        Public ReadOnly IOCTL_STORAGE_DEVICE_TELEMETRY_QUERY_CAPS As New CTL_CODE(IOCTL_STORAGE_BASE, &H472, METHOD_BUFFERED, FILE_READ_ACCESS Or FILE_WRITE_ACCESS)
        Public ReadOnly IOCTL_STORAGE_GET_DEVICE_TELEMETRY_RAW As New CTL_CODE(IOCTL_STORAGE_BASE, &H473, METHOD_BUFFERED, FILE_READ_ACCESS Or FILE_WRITE_ACCESS)


        Public ReadOnly IOCTL_STORAGE_QUERY_PROPERTY As New CTL_CODE(IOCTL_STORAGE_BASE, &H500, METHOD_BUFFERED, FILE_ANY_ACCESS)
        Public ReadOnly IOCTL_STORAGE_MANAGE_DATA_SET_ATTRIBUTES As New CTL_CODE(IOCTL_STORAGE_BASE, &H501, METHOD_BUFFERED, FILE_WRITE_ACCESS)
        Public ReadOnly IOCTL_STORAGE_GET_LB_PROVISIONING_MAP_RESOURCES As New CTL_CODE(IOCTL_STORAGE_BASE, &H502, METHOD_BUFFERED, FILE_READ_ACCESS)

        ''
        '' IOCTLs &H0503 to &H0580 reserved for Enhanced Storage devices.
        ''


        ''
        '' IOCTLs for bandwidth contracts on storage devices
        '' (Move this to ntddsfio if we decide to use a new base)
        ''

        Public ReadOnly IOCTL_STORAGE_GET_BC_PROPERTIES As New CTL_CODE(IOCTL_STORAGE_BASE, &H600, METHOD_BUFFERED, FILE_READ_ACCESS)
        Public ReadOnly IOCTL_STORAGE_ALLOCATE_BC_STREAM As New CTL_CODE(IOCTL_STORAGE_BASE, &H601, METHOD_BUFFERED, FILE_READ_ACCESS Or FILE_WRITE_ACCESS)
        Public ReadOnly IOCTL_STORAGE_FREE_BC_STREAM As New CTL_CODE(IOCTL_STORAGE_BASE, &H602, METHOD_BUFFERED, FILE_READ_ACCESS Or FILE_WRITE_ACCESS)

        ''
        '' IOCTL to check for priority support
        ''
        Public ReadOnly IOCTL_STORAGE_CHECK_PRIORITY_HINT_SUPPORT As New CTL_CODE(IOCTL_STORAGE_BASE, &H620, METHOD_BUFFERED, FILE_ANY_ACCESS)

        ''
        '' IOCTL for data integrity check support
        ''

        Public ReadOnly IOCTL_STORAGE_START_DATA_INTEGRITY_CHECK As New CTL_CODE(IOCTL_STORAGE_BASE, &H621, METHOD_BUFFERED, FILE_READ_ACCESS Or FILE_WRITE_ACCESS)
        Public ReadOnly IOCTL_STORAGE_STOP_DATA_INTEGRITY_CHECK As New CTL_CODE(IOCTL_STORAGE_BASE, &H622, METHOD_BUFFERED, FILE_READ_ACCESS Or FILE_WRITE_ACCESS)

        '' begin_winioctl

        ''
        '' IOCTLs &H0643 to &H0655 reserved for VHD disk support.
        ''

        ''
        '' IOCTL to support Idle Power Management, including Device Wake
        ''
        Public ReadOnly IOCTL_STORAGE_ENABLE_IDLE_POWER As New CTL_CODE(IOCTL_STORAGE_BASE, &H720, METHOD_BUFFERED, FILE_ANY_ACCESS)
        Public ReadOnly IOCTL_STORAGE_GET_IDLE_POWERUP_REASON As New CTL_CODE(IOCTL_STORAGE_BASE, &H721, METHOD_BUFFERED, FILE_ANY_ACCESS)

        ''
        '' IOCTLs to allow class drivers to acquire and release active references on
        '' a unit.  These should only be used if the class driver previously sent a
        '' successful IOCTL_STORAGE_ENABLE_IDLE_POWER request to the port driver.
        ''
        Public ReadOnly IOCTL_STORAGE_POWER_ACTIVE As New CTL_CODE(IOCTL_STORAGE_BASE, &H722, METHOD_BUFFERED, FILE_ANY_ACCESS)
        Public ReadOnly IOCTL_STORAGE_POWER_IDLE As New CTL_CODE(IOCTL_STORAGE_BASE, &H723, METHOD_BUFFERED, FILE_ANY_ACCESS)

        ''
        '' This IOCTL indicates that the physical device has triggered some sort of event.
        ''
        Public ReadOnly IOCTL_STORAGE_EVENT_NOTIFICATION As New CTL_CODE(IOCTL_STORAGE_BASE, &H724, METHOD_BUFFERED, FILE_ANY_ACCESS)

        Public Const IOCTL_VOLUME_BASE As Integer = 86 ' Asc("V")
        Public Const IOCTL_VOLUME_GET_VOLUME_DISK_EXTENTS As UInteger = (IOCTL_VOLUME_BASE << 16) Or (FILE_ANY_ACCESS << 14) Or (0 << 2) Or (METHOD_BUFFERED)


        ''
        '' IoControlCode values for disk devices.
        ''

        Public Const IOCTL_DISK_BASE = 7
        Public ReadOnly IOCTL_DISK_GET_DRIVE_GEOMETRY As New CTL_CODE(IOCTL_DISK_BASE, &H0, METHOD_BUFFERED, FILE_ANY_ACCESS)
        Public ReadOnly IOCTL_DISK_GET_PARTITION_INFO As New CTL_CODE(IOCTL_DISK_BASE, &H1, METHOD_BUFFERED, FILE_READ_ACCESS)
        Public ReadOnly IOCTL_DISK_SET_PARTITION_INFO As New CTL_CODE(IOCTL_DISK_BASE, &H2, METHOD_BUFFERED, FILE_READ_ACCESS Or FILE_WRITE_ACCESS)
        Public ReadOnly IOCTL_DISK_GET_DRIVE_LAYOUT As New CTL_CODE(IOCTL_DISK_BASE, &H3, METHOD_BUFFERED, FILE_READ_ACCESS)
        Public ReadOnly IOCTL_DISK_SET_DRIVE_LAYOUT As New CTL_CODE(IOCTL_DISK_BASE, &H4, METHOD_BUFFERED, FILE_READ_ACCESS Or FILE_WRITE_ACCESS)
        Public ReadOnly IOCTL_DISK_VERIFY As New CTL_CODE(IOCTL_DISK_BASE, &H5, METHOD_BUFFERED, FILE_ANY_ACCESS)
        Public ReadOnly IOCTL_DISK_FORMAT_TRACKS As New CTL_CODE(IOCTL_DISK_BASE, &H6, METHOD_BUFFERED, FILE_READ_ACCESS Or FILE_WRITE_ACCESS)
        Public ReadOnly IOCTL_DISK_REASSIGN_BLOCKS As New CTL_CODE(IOCTL_DISK_BASE, &H7, METHOD_BUFFERED, FILE_READ_ACCESS Or FILE_WRITE_ACCESS)
        Public ReadOnly IOCTL_DISK_PERFORMANCE As New CTL_CODE(IOCTL_DISK_BASE, &H8, METHOD_BUFFERED, FILE_ANY_ACCESS)
        Public ReadOnly IOCTL_DISK_IS_WRITABLE As New CTL_CODE(IOCTL_DISK_BASE, &H9, METHOD_BUFFERED, FILE_ANY_ACCESS)
        Public ReadOnly IOCTL_DISK_LOGGING As New CTL_CODE(IOCTL_DISK_BASE, &HA, METHOD_BUFFERED, FILE_ANY_ACCESS)
        Public ReadOnly IOCTL_DISK_FORMAT_TRACKS_EX As New CTL_CODE(IOCTL_DISK_BASE, &HB, METHOD_BUFFERED, FILE_READ_ACCESS Or FILE_WRITE_ACCESS)
        Public ReadOnly IOCTL_DISK_HISTOGRAM_STRUCTURE As New CTL_CODE(IOCTL_DISK_BASE, &HC, METHOD_BUFFERED, FILE_ANY_ACCESS)
        Public ReadOnly IOCTL_DISK_HISTOGRAM_DATA As New CTL_CODE(IOCTL_DISK_BASE, &HD, METHOD_BUFFERED, FILE_ANY_ACCESS)
        Public ReadOnly IOCTL_DISK_HISTOGRAM_RESET As New CTL_CODE(IOCTL_DISK_BASE, &HE, METHOD_BUFFERED, FILE_ANY_ACCESS)
        Public ReadOnly IOCTL_DISK_REQUEST_STRUCTURE As New CTL_CODE(IOCTL_DISK_BASE, &HF, METHOD_BUFFERED, FILE_ANY_ACCESS)
        Public ReadOnly IOCTL_DISK_REQUEST_DATA As New CTL_CODE(IOCTL_DISK_BASE, &H10, METHOD_BUFFERED, FILE_ANY_ACCESS)
        Public ReadOnly IOCTL_DISK_PERFORMANCE_OFF As New CTL_CODE(IOCTL_DISK_BASE, &H18, METHOD_BUFFERED, FILE_ANY_ACCESS)



        ''if(_WIN32_WINNT >= &H0400)
        Public ReadOnly IOCTL_DISK_CONTROLLER_NUMBER As New CTL_CODE(IOCTL_DISK_BASE, &H11, METHOD_BUFFERED, FILE_ANY_ACCESS)

        ''
        '' IOCTL support for SMART drive fault prediction.
        ''

        Public ReadOnly SMART_GET_VERSION As New CTL_CODE(IOCTL_DISK_BASE, &H20, METHOD_BUFFERED, FILE_READ_ACCESS)
        Public ReadOnly SMART_SEND_DRIVE_COMMAND As New CTL_CODE(IOCTL_DISK_BASE, &H21, METHOD_BUFFERED, FILE_READ_ACCESS Or FILE_WRITE_ACCESS)
        Public ReadOnly SMART_RCV_DRIVE_DATA As New CTL_CODE(IOCTL_DISK_BASE, &H22, METHOD_BUFFERED, FILE_READ_ACCESS Or FILE_WRITE_ACCESS)

        ''endif /* _WIN32_WINNT >= &H0400 */

        ''if (_WIN32_WINNT >= &H500)

        ''
        '' New IOCTLs for GUID Partition tabled disks.
        ''

        Public ReadOnly IOCTL_DISK_GET_PARTITION_INFO_EX As New CTL_CODE(IOCTL_DISK_BASE, &H12, METHOD_BUFFERED, FILE_ANY_ACCESS)
        Public ReadOnly IOCTL_DISK_SET_PARTITION_INFO_EX As New CTL_CODE(IOCTL_DISK_BASE, &H13, METHOD_BUFFERED, FILE_READ_ACCESS Or FILE_WRITE_ACCESS)
        Public ReadOnly IOCTL_DISK_GET_DRIVE_LAYOUT_EX As New CTL_CODE(IOCTL_DISK_BASE, &H14, METHOD_BUFFERED, FILE_ANY_ACCESS)
        Public ReadOnly IOCTL_DISK_SET_DRIVE_LAYOUT_EX As New CTL_CODE(IOCTL_DISK_BASE, &H15, METHOD_BUFFERED, FILE_READ_ACCESS Or FILE_WRITE_ACCESS)
        Public ReadOnly IOCTL_DISK_CREATE_DISK As New CTL_CODE(IOCTL_DISK_BASE, &H16, METHOD_BUFFERED, FILE_READ_ACCESS Or FILE_WRITE_ACCESS)
        Public ReadOnly IOCTL_DISK_GET_LENGTH_INFO As New CTL_CODE(IOCTL_DISK_BASE, &H17, METHOD_BUFFERED, FILE_READ_ACCESS)
        Public ReadOnly IOCTL_DISK_GET_DRIVE_GEOMETRY_EX As New CTL_CODE(IOCTL_DISK_BASE, &H28, METHOD_BUFFERED, FILE_ANY_ACCESS)

        ''endif /* _WIN32_WINNT >= &H0500 */


        ''if (_WIN32_WINNT >= &H0502)

        ''
        '' New IOCTL for disk devices that support 8 byte LBA
        ''
        Public ReadOnly IOCTL_DISK_REASSIGN_BLOCKS_EX As New CTL_CODE(IOCTL_DISK_BASE, &H29, METHOD_BUFFERED, FILE_READ_ACCESS Or FILE_WRITE_ACCESS)

        ''End If ''_WIN32_WINNT >= &H0502

        ''if(_WIN32_WINNT >= &H0500)
        Public ReadOnly IOCTL_DISK_UPDATE_DRIVE_SIZE As New CTL_CODE(IOCTL_DISK_BASE, &H32, METHOD_BUFFERED, FILE_READ_ACCESS Or FILE_WRITE_ACCESS)
        Public ReadOnly IOCTL_DISK_GROW_PARTITION As New CTL_CODE(IOCTL_DISK_BASE, &H34, METHOD_BUFFERED, FILE_READ_ACCESS Or FILE_WRITE_ACCESS)

        Public ReadOnly IOCTL_DISK_GET_CACHE_INFORMATION As New CTL_CODE(IOCTL_DISK_BASE, &H35, METHOD_BUFFERED, FILE_READ_ACCESS)
        Public ReadOnly IOCTL_DISK_SET_CACHE_INFORMATION As New CTL_CODE(IOCTL_DISK_BASE, &H36, METHOD_BUFFERED, FILE_READ_ACCESS Or FILE_WRITE_ACCESS)
        ''If (NTDDI_VERSION < NTDDI_WS03) Then
        Public ReadOnly IOCTL_DISK_GET_WRITE_CACHE_STATE As New CTL_CODE(IOCTL_DISK_BASE, &H37, METHOD_BUFFERED, FILE_READ_ACCESS)
        ''Else
        Public ReadOnly OBSOLETE_DISK_GET_WRITE_CACHE_STATE As New CTL_CODE(IOCTL_DISK_BASE, &H37, METHOD_BUFFERED, FILE_READ_ACCESS)
        ''End If
        Public ReadOnly IOCTL_DISK_DELETE_DRIVE_LAYOUT As New CTL_CODE(IOCTL_DISK_BASE, &H40, METHOD_BUFFERED, FILE_READ_ACCESS Or FILE_WRITE_ACCESS)

        ''
        '' Called to flush cached information that the driver may have about this
        '' device's characteristics.  Not all drivers cache characteristics, and not
        '' cached properties can be flushed.  This simply serves as an update to the
        '' driver that it may want to do an expensive reexamination of the device's
        '' characteristics now (fixed media size, partition table, etc...)
        ''

        Public ReadOnly IOCTL_DISK_UPDATE_PROPERTIES As New CTL_CODE(IOCTL_DISK_BASE, &H50, METHOD_BUFFERED, FILE_ANY_ACCESS)

        ''
        ''  Special IOCTLs needed to support PC-98 machines in Japan
        ''

        Public ReadOnly IOCTL_DISK_FORMAT_DRIVE As New CTL_CODE(IOCTL_DISK_BASE, &HF3, METHOD_BUFFERED, FILE_READ_ACCESS Or FILE_WRITE_ACCESS)
        Public ReadOnly IOCTL_DISK_SENSE_DEVICE As New CTL_CODE(IOCTL_DISK_BASE, &HF8, METHOD_BUFFERED, FILE_ANY_ACCESS)

        ''endif /* _WIN32_WINNT >= &H0500 */

        ''
        '' The following device control codes are common for all class drivers.  The
        '' functions codes defined here must match all of the other class drivers.
        ''
        '' Warning: these codes will be replaced in the future by equivalent
        '' IOCTL_STORAGE codes
        ''

        Public ReadOnly IOCTL_DISK_CHECK_VERIFY As New CTL_CODE(IOCTL_DISK_BASE, &H200, METHOD_BUFFERED, FILE_READ_ACCESS)
        Public ReadOnly IOCTL_DISK_MEDIA_REMOVAL As New CTL_CODE(IOCTL_DISK_BASE, &H201, METHOD_BUFFERED, FILE_READ_ACCESS)
        Public ReadOnly IOCTL_DISK_EJECT_MEDIA As New CTL_CODE(IOCTL_DISK_BASE, &H202, METHOD_BUFFERED, FILE_READ_ACCESS)
        Public ReadOnly IOCTL_DISK_LOAD_MEDIA As New CTL_CODE(IOCTL_DISK_BASE, &H203, METHOD_BUFFERED, FILE_READ_ACCESS)
        Public ReadOnly IOCTL_DISK_RESERVE As New CTL_CODE(IOCTL_DISK_BASE, &H204, METHOD_BUFFERED, FILE_READ_ACCESS)
        Public ReadOnly IOCTL_DISK_RELEASE As New CTL_CODE(IOCTL_DISK_BASE, &H205, METHOD_BUFFERED, FILE_READ_ACCESS)
        Public ReadOnly IOCTL_DISK_FIND_NEW_DEVICES As New CTL_CODE(IOCTL_DISK_BASE, &H206, METHOD_BUFFERED, FILE_READ_ACCESS)
        Public ReadOnly IOCTL_DISK_GET_MEDIA_TYPES As New CTL_CODE(IOCTL_DISK_BASE, &H300, METHOD_BUFFERED, FILE_ANY_ACCESS)

        ''



#End Region

        ''' <summary>
        ''' Storage device number information.
        ''' </summary>
        ''' <remarks></remarks>
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure STORAGE_DEVICE_NUMBER
            Public DeviceType As UInteger
            Public DeviceNumber As UInteger
            Public PartitionNumber As UInteger
        End Structure

        Public Declare Function DeviceIoControl Lib "kernel32.dll" _
        (hDevice As IntPtr,
         dwIoControlCode As UInteger,
         lpInBuffer As IntPtr,
         nInBufferSize As UInteger,
         lpOutBuffer As IntPtr,
         nOutBufferSize As UInteger,
         ByRef lpBytesReturned As UInteger,
         lpOverlapped As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Function DeviceIoControl Lib "kernel32.dll" _
        (hDevice As IntPtr,
         dwIoControlCode As UInteger,
         lpInBuffer As IntPtr,
         nInBufferSize As UInteger,
         ByRef lpOutBuffer As STORAGE_DEVICE_NUMBER,
         nOutBufferSize As UInteger,
         ByRef lpBytesReturned As UInteger,
         lpOverlapped As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean

#End Region

#Region "Volume Management"

        Public Const ERROR_MORE_DATA = 234

        Public Const ERROR_INSUFFICIENT_BUFFER = &H7A

        ''' <summary>
        ''' Describes a volume disk extent.
        ''' </summary>
        ''' <remarks></remarks>
        <StructLayout(LayoutKind.Sequential)>
        Structure DISK_EXTENT
            <MarshalAs(UnmanagedType.I4)>
            Public DiskNumber As Integer
            Public Space As Integer

            <MarshalAs(UnmanagedType.I8)>
            Public StartingOffset As Long

            <MarshalAs(UnmanagedType.I8)>
            Public ExtentLength As Long
        End Structure

        ''' <summary>
        ''' Describes volume disk extents.
        ''' </summary>
        ''' <remarks></remarks>
        <StructLayout(LayoutKind.Sequential)>
        Structure VOLUME_DISK_EXTENTS
            <MarshalAs(UnmanagedType.I4)>
            Public NumberOfExtents As Integer
            Public Space As Integer

            Public Extents() As DISK_EXTENT

            Public Shared Function FromPtr(ptr As IntPtr) As VOLUME_DISK_EXTENTS
                Dim ve As New VOLUME_DISK_EXTENTS
                Dim cb As Integer = Marshal.SizeOf(Of DISK_EXTENT)()
                Dim m As MemPtr = ptr
                Dim i As Integer

                ve.NumberOfExtents = m.IntAt(0)
                ve.Space = m.IntAt(1)
                ReDim ve.Extents(ve.NumberOfExtents - 1)

                m += 8
                For i = 0 To ve.NumberOfExtents - 1
                    ve.Extents(i) = m.ToStruct(Of DISK_EXTENT)()

                    m += cb
                Next
                FromPtr = ve
            End Function

        End Structure


        Public Declare Function GetVolumeInformation Lib "kernel32.dll" _
        Alias "GetVolumeInformationW" _
        (<MarshalAs(UnmanagedType.LPWStr)> lpRootPathName As String,
         lpVolumeNameBuffer As IntPtr,
         nVolumeNameSize As Integer,
         ByRef lpVolumeSerialNumber As UInteger,
         ByRef lpMaximumComponentLength As Integer,
         ByRef lpFileSystemFlags As FileSystemFlags,
         lpFileSystemNameBuffer As IntPtr,
         nFileSystemNameSize As Integer) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Function GetVolumeInformationByHandle Lib "kernel32.dll" _
        Alias "GetVolumeInformationByHandleW" _
        (hFile As IntPtr,
         lpVolumeNameBuffer As IntPtr,
         nVolumeNameSize As Integer,
         ByRef lpVolumeSerialNumber As UInteger,
         ByRef lpMaximumComponentLength As Integer,
         ByRef lpFileSystemFlags As FileSystemFlags,
         lpFileSystemNameBuffer As IntPtr,
         nFileSystemNameSize As Integer) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Function GetVolumePathNamesForVolumeName _
        Lib "kernel32.dll" _
        Alias "GetVolumePathNamesForVolumeNameW" (
            <MarshalAs(UnmanagedType.LPWStr)> lpszVolumeName As String,
            lpszVolumePathNames As IntPtr,
            cchBufferLength As Integer,
            ByRef lpcchReturnLength As Integer) As <MarshalAs(UnmanagedType.Bool)> Boolean


        ''' <summary>
        ''' Get volume disk extents for volumes that may or may not span more than one physical drive.
        ''' </summary>
        ''' <param name="devicePath">The device path of the volume.</param>
        ''' <returns>An array of DiskExtent structures.</returns>
        ''' <remarks></remarks>
        Function GetDiskExtentsFor(devicePath As String) As DiskExtent()
            Dim deOut() As DiskExtent = Nothing

            Dim inBuff As MemPtr
            Dim inSize As Integer
            Dim file As IntPtr,
            h As Integer = 0
            Dim de As New DISK_EXTENT,
            ve As New VOLUME_DISK_EXTENTS
            Dim r As Boolean

            file = CreateFile(devicePath, GENERIC_READ, FILE_SHARE_READ Or FILE_SHARE_WRITE, IntPtr.Zero, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, IntPtr.Zero)

            If file = INVALID_HANDLE_VALUE Then
                Return Nothing
            End If

            inSize = Marshal.SizeOf(de) + Marshal.SizeOf(ve)
            inBuff.Length = inSize

            r = DeviceIoControl(file, IOCTL_VOLUME_GET_VOLUME_DISK_EXTENTS, IntPtr.Zero, CUInt(0), inBuff, CUInt(inSize), CUInt(h), IntPtr.Zero)

            If Not r AndAlso GetLastError = ERROR_MORE_DATA Then
                inBuff.Length = (inSize * inBuff.IntAt(0))
                r = DeviceIoControl(file, IOCTL_VOLUME_GET_VOLUME_DISK_EXTENTS, IntPtr.Zero, CUInt(0), inBuff, CUInt(inSize), CUInt(h), IntPtr.Zero)
            End If

            If Not r Then
                inBuff.Free()
                CloseHandle(file)
                Return Nothing
            End If

            CloseHandle(file)

            ve = VOLUME_DISK_EXTENTS.FromPtr(inBuff)
            inBuff.Free()
            h = 0

            ReDim deOut(ve.Extents.Length - 1)

            For Each de In ve.Extents
                deOut(h).PhysicalDevice = de.DiskNumber
                deOut(h).Space = de.Space
                deOut(h).Size = de.ExtentLength
                deOut(h).Offset = de.StartingOffset

                h += 1
            Next

            Return deOut

        End Function

        ''' <summary>
        ''' Populates a DiskDeviceInfo object with extended volume information.
        ''' </summary>
        ''' <param name="disk">The disk device object to populate.</param>
        ''' <param name="handle">Optional handle to an open disk.</param>
        ''' <remarks></remarks>
        Public Sub PopulateVolumeInfo(ByRef disk As DiskDeviceInfo, Optional handle As IntPtr = Nothing)

            Dim pLen As Integer = (MAX_PATH + 1) * 2
            Dim mm1 As MemPtr
            Dim mm2 As MemPtr
            Dim mc As Integer = 0

            mm1.Alloc(pLen)
            mm2.Alloc(pLen)
            mm1.ZeroMemory()
            mm2.ZeroMemory()

            Dim pp As New String(ChrW(0), 1024)
            GetVolumeNameForVolumeMountPoint(disk.DevicePath & "\", CType(mm1, IntPtr), 1024)

            '' just get rid of the extra nulls (they like to stick around).

            disk.Type = StorageType.Volume

            disk.VolumeGuidPath = CStr(mm1)
            disk.VolumePaths = GetVolumePaths(CStr(mm1))

            mm1.ZeroMemory()

            If handle = IntPtr.Zero OrElse handle = INVALID_HANDLE_VALUE Then
                GetVolumeInformation(disk.VolumeGuidPath, mm1, pLen, disk.SerialNumber, mc, disk.VolumeFlags, mm2, pLen)
            Else
                GetVolumeInformationByHandle(handle, mm1, pLen, disk.SerialNumber, mc, disk.VolumeFlags, mm2, pLen)
            End If

            disk.DiskExtents = GetDiskExtentsFor(disk.DevicePath)

            disk.FriendlyName = CStr(mm1)
            disk.FileSystem = CStr(mm2)

            Dim a As Integer,
            b As Integer,
            c As Integer,
            d As Integer

            GetDiskFreeSpace(disk.VolumeGuidPath, a, b, c, d)
            disk.SectorSize = b

            mm1.Free()
            mm2.Free()

        End Sub

        ''' <summary>
        ''' Get all mount points for a volume.
        ''' </summary>
        ''' <param name="path">Volume Guid Path.</param>
        ''' <returns>An array of strings that represent mount points.</returns>
        ''' <remarks></remarks>
        Public Function GetVolumePaths(path As String) As String()

            Dim cc As Integer = 1024
            Dim retc As Integer = 0

            Dim mm As New MemPtr
            Dim r As Boolean

            mm.Alloc(cc)

            r = GetVolumePathNamesForVolumeName(
            path,
            mm.Handle,
            cc,
            retc)

            If Not r Then Return Nothing

            If retc > 1024 Then
                mm.ReAlloc(retc)

                r = GetVolumePathNamesForVolumeName(
                path,
                mm,
                retc,
                retc)

            End If

            GetVolumePaths = mm.GetStringArray(0)
            mm.Free()

        End Function

#End Region

    End Module

    ''' <summary>
    ''' Raw disk access.  Currently not exposed.
    ''' </summary>
    ''' <remarks></remarks>
    <HideModuleName>
    <SecurityCritical()>
    Friend Module RawDisk

        ''' <summary>
        ''' Disk Geometry Media Types
        ''' </summary>
        ''' <remarks></remarks>
        Public Enum MEDIA_TYPE
            Unknown = &H0
            F5_1Pt2_512 = &H1
            F3_1Pt44_512 = &H2
            F3_2Pt88_512 = &H3
            F3_20Pt8_512 = &H4
            F3_720_512 = &H5
            F5_360_512 = &H6
            F5_320_512 = &H7
            F5_320_1024 = &H8
            F5_180_512 = &H9
            F5_160_512 = &HA
            RemovableMedia = &HB
            FixedMedia = &HC
            F3_120M_512 = &HD
            F3_640_512 = &HE
            F5_640_512 = &HF
            F5_720_512 = &H10
            F3_1Pt2_512 = &H11
            F3_1Pt23_1024 = &H12
            F5_1Pt23_1024 = &H13
            F3_128Mb_512 = &H14
            F3_230Mb_512 = &H15
            F8_256_128 = &H16
            F3_200Mb_512 = &H17
            F3_240M_512 = &H18
            F3_32M_512 = &H19
        End Enum

        <StructLayout(LayoutKind.Sequential)>
        Public Structure DISK_GEOMETRY
            Public Cylinders As Long
            Public MediaType As MEDIA_TYPE
            Public TracksPerCylinder As UInteger
            Public SectorsPerTrack As UInteger
            Public BytesPerSector As UInteger
        End Structure

        <StructLayout(LayoutKind.Sequential)>
        Public Structure DISK_GEOMETRY_EX
            Public Geometry As DISK_GEOMETRY
            Public DiskSize As ULong
            Public Data As Byte
        End Structure

        ''' <summary>
        ''' Retrieves the disk geometry of the specified disk.
        ''' </summary>
        ''' <param name="hfile">Handle to a valid, open disk.</param>
        ''' <param name="geo">Receives the disk geometry information.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function DiskGeometry(hfile As IntPtr, ByRef geo As DISK_GEOMETRY_EX) As Boolean
            If hfile = INVALID_HANDLE_VALUE Then Return False

            Dim mm As MemPtr
            Dim l As UInteger = 0
            Dim cb As UInteger = 0

            l = CUInt(Marshal.SizeOf(Of DISK_GEOMETRY_EX))
            mm.Alloc(l)

            DeviceIoControl(hfile,
                         IOCTL_DISK_GET_DRIVE_GEOMETRY_EX,
                         IntPtr.Zero,
                         CUInt(0),
                         mm.Handle,
                         l,
                         cb,
                         IntPtr.Zero)

            geo = mm.ToStruct(Of DISK_GEOMETRY_EX)()
            mm.Free()

            Return True
        End Function

        ''' <summary>
        ''' Retrieve the partition table of a GPT-layout disk, manually.
        ''' Must be Administrator.
        ''' </summary>
        ''' <param name="inf">DiskDeviceInfo object to the physical disk to read.</param>
        ''' <param name="gptInfo">Receives the drive layout information.</param>
        ''' <returns>True if successful.</returns>
        ''' <remarks></remarks>
        Public Function ReadRawGptDisk(inf As DiskDeviceInfo, ByRef gptInfo As RAW_GPT_DISK) As Boolean


            ' Demand Administrator for accessing a raw disk.
            AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal)
            'Dim principalPerm As New PrincipalPermission(Nothing, "Administrators")
            'principalPerm.Demand()


            Dim hfile As IntPtr = CreateFile(inf.DevicePath, GENERIC_READ Or GENERIC_WRITE, FILE_SHARE_READ Or FILE_SHARE_WRITE, IntPtr.Zero, OPEN_EXISTING, FILE_FLAG_NO_BUFFERING Or FILE_FLAG_RANDOM_ACCESS, IntPtr.Zero)
            If hfile = INVALID_HANDLE_VALUE Then Return False

            Dim geo As DISK_GEOMETRY_EX = Nothing

            '' get the disk geometry to retrieve the sector (LBA) size.
            If Not DiskGeometry(hfile, geo) Then
                CloseHandle(hfile)
                Return False
            End If

            ' sector size (usually 512 bytes)
            Dim bps As UInteger = geo.Geometry.BytesPerSector

            Dim br As UInteger = 0
            Dim lp As Long = 0
            Dim lp2 As Long = 0
            Dim mm As New MemPtr(bps * 2)

            SetFilePointerEx(hfile, 0, lp, FilePointerMoveMethod.Begin)
            ReadFile(hfile, mm.Handle, bps * CUInt(2), br, IntPtr.Zero)

            Dim mbr As New RAW_MBR
            Dim gpt As New RAW_GPT_HEADER
            Dim gpp() As RAW_GPT_PARTITION = Nothing

            ' read the master boot record.
            mbr = mm.ToStructAt(Of RAW_MBR)(446)

            ' read the GPT structure header.
            gpt = mm.ToStructAt(Of RAW_GPT_HEADER)(bps)

            '' check the partition header CRC.
            If gpt.IsValid Then

                Dim lr As Long = CLng(br)

                ' seek to the LBA of the partition information.
                SetFilePointerEx(hfile, CUInt(bps * gpt.PartitionEntryLBA), lr, FilePointerMoveMethod.Begin)

                br = CUInt(lr)

                ' calculate the size of the partition table buffer.
                lp = gpt.NumberOfPartitions * gpt.PartitionEntryLength

                ' byte align to the sector size.
                If (lp Mod bps) <> 0 Then
                    lp += (bps - (lp Mod bps))
                End If

                '' bump up the memory pointer.
                mm.ReAlloc(lp)
                mm.ZeroMemory()

                '' read the partition information into the pointer.
                ReadFile(hfile, mm.Handle, CUInt(lp), br, IntPtr.Zero)

                '' check the partition table CRC.
                If mm.CalculateCrc32 = gpt.PartitionArrayCRC32 Then
                    '' disk is valid.

                    lp = CUInt(Marshal.SizeOf(Of RAW_GPT_PARTITION)())
                    br = 0

                    Dim i As Integer
                    Dim c As Integer = CInt(gpt.NumberOfPartitions) - 1

                    ReDim gpp(c)

                    '' populate the drive information.
                    For i = 0 To c

                        gpp(i) = mm.ToStructAt(Of RAW_GPT_PARTITION)(lp2)

                        ' break on empty GUID, we are past the last partition.
                        If gpp(i).PartitionTypeGuid = Guid.Empty Then Exit For

                        lp2 += lp
                    Next

                    '' trim off excess records from the array.
                    If i < c Then
                        If i = 0 Then
                            gpp = {}
                        Else
                            ReDim Preserve gpp(i - 1)
                        End If
                    End If
                End If
            End If

            '' free the resources.
            mm.Free()
            CloseHandle(hfile)

            '' if gpp is nothing then some error occurred somewhere and we did not succeed.
            If gpp Is Nothing Then Return False

            '' create a new RAW_GPT_DISK structure.
            gptInfo = New RAW_GPT_DISK
            gptInfo.Header = gpt
            gptInfo.Partitions = gpp

            '' we have succeeded.
            Return True
        End Function

        ''' <summary>
        ''' Raw disk Master Boot Record entry.
        ''' </summary>
        ''' <remarks></remarks>
        <StructLayout(LayoutKind.Sequential, Pack:=1)>
        Public Structure RAW_MBR

            Public BootIndicator As Byte

            <MarshalAs(UnmanagedType.ByValArray, ArraySubType:=UnmanagedType.U1, SizeConst:=3)>
            Private _startingChs As Byte()

            Public PartitionType As Byte

            <MarshalAs(UnmanagedType.ByValArray, ArraySubType:=UnmanagedType.U1, SizeConst:=3)>
            Private _endingChs As Byte()

            Public StartingLBA As UInteger

            Public SizeInLBA As UInteger

            Public ReadOnly Property StartingCHS As Integer
                Get
                    Dim mm As New MemPtr(4)
                    Marshal.Copy(_startingChs, 0, mm.Handle, 3)
                    StartingCHS = mm.IntAt(0)
                    mm.Free()
                End Get
            End Property

            Public ReadOnly Property EndingCHS As Integer
                Get
                    Dim mm As New MemPtr(4)
                    Marshal.Copy(_endingChs, 0, mm.Handle, 3)
                    EndingCHS = mm.IntAt(0)
                    mm.Free()
                End Get
            End Property

        End Structure

        ''' <summary>
        ''' Raw disk GPT partition table header.
        ''' </summary>
        ''' <remarks></remarks>
        <StructLayout(LayoutKind.Sequential, Pack:=1)>
        Public Structure RAW_GPT_HEADER
            Public Signature As ULong
            Public Revision As UInteger
            Public HeaderSize As UInteger
            Public HeaderCRC32 As UInteger
            Public Reserved As UInteger
            Public MyLBA As ULong
            Public AlternateLBA As ULong
            Public FirstUsableLBA As ULong
            Public MaxUsableLBA As ULong
            Public DiskGuid As Guid
            Public PartitionEntryLBA As ULong
            Public NumberOfPartitions As UInteger
            Public PartitionEntryLength As UInteger
            Public PartitionArrayCRC32 As UInteger

            ''' <summary>
            ''' True if this structure contains a CRC-32 valid GPT partition header.
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public ReadOnly Property IsValid As Boolean
                Get
                    IsValid = Validate()
                End Get
            End Property

            ''' <summary>
            ''' Validate the header and CRC-32 of this structure.
            ''' </summary>
            ''' <returns>True if the structure is valid.</returns>
            ''' <remarks></remarks>
            Public Function Validate() As Boolean
                Dim mm As New MemPtr
                mm.FromStruct(Of RAW_GPT_HEADER)(Me)
                mm.UIntAt(4) = 0

                '' validate the crc and the signature moniker 
                Validate = (HeaderCRC32 = mm.CalculateCrc32) AndAlso Signature = &H5452415020494645
                mm.Free()
            End Function

        End Structure

        ''' <summary>
        ''' Raw GPT partition information.
        ''' </summary>
        ''' <remarks></remarks>
        <StructLayout(LayoutKind.Sequential, Pack:=1)>
        Public Structure RAW_GPT_PARTITION
            Public PartitionTypeGuid As Guid
            Public UniquePartitionGuid As Guid
            Public StartingLBA As ULong
            Public EndingLBA As ULong
            Public Attributes As GptPartitionAttributes

            <MarshalAs(UnmanagedType.ByValArray, ArraySubType:=UnmanagedType.U2, SizeConst:=36)>
            Private _Name() As Char

            ''' <summary>
            ''' Returns the name of this partition (if any).
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public ReadOnly Property Name As String
                Get
                    Return CStr(_Name).Trim(ChrW(0))
                End Get
            End Property

            ''' <summary>
            ''' Retrieve the partition code information for this partition type (if any).
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public ReadOnly Property PartitionCode As GptCodeInfo
                Get
                    Return GptCodeInfo.FindByCode(PartitionTypeGuid)
                End Get
            End Property

            ''' <summary>
            ''' Converts this object into its string representation.
            ''' </summary>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Overrides Function ToString() As String
                If (Name <> "") Then
                    ToString = Name
                ElseIf PartitionCode IsNot Nothing Then
                    ToString = PartitionCode.ToString
                Else
                    ToString = UniquePartitionGuid.ToString("B")
                End If
            End Function

        End Structure

        ''' <summary>
        ''' Contains an entire raw GPT disk layout.
        ''' </summary>
        ''' <remarks></remarks>
        Public Structure RAW_GPT_DISK
            Public Header As RAW_GPT_HEADER
            Public Partitions() As RAW_GPT_PARTITION
        End Structure

    End Module

    <HideModuleName>
    <SecurityCritical()>
    Friend Module Partitioning

        ''' <summary>
        ''' Windows system MBR partition information structure.
        ''' </summary>
        ''' <remarks></remarks>
        <StructLayout(LayoutKind.Sequential)>
        Public Structure PARTITION_INFORMATION_MBR
            Public PartitionType As Byte

            <MarshalAs(UnmanagedType.Bool)>
            Public BootIndicator As Boolean

            <MarshalAs(UnmanagedType.Bool)>
            Public RecognizedPartition As Boolean

            Public HiddenSectors As UInteger

            ''' <summary>
            ''' Returns a list of all partition types that were found for the current partition code.
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public ReadOnly Property PartitionCodes As PartitionCodeInfo()
                Get
                    Return PartitionCodeInfo.FindByCode(PartitionType)
                End Get
            End Property

            ''' <summary>
            ''' Returns the strongest partition type match for Windows NT from the available partition types for the current partition code.
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public ReadOnly Property PartitionCode As PartitionCodeInfo
                Get
                    Dim c As PartitionCodeInfo() = PartitionCodes
                    If c IsNot Nothing Then
                        Return c(0)
                    End If

                    Return Nothing
                End Get
            End Property

            Public Overrides Function ToString() As String
                Dim c As PartitionCodeInfo() = PartitionCodes
                If c IsNot Nothing Then
                    Return c(0).Description
                End If

                Return c(0).PartitionID.ToString
            End Function

        End Structure

        ''' <summary>
        ''' Windows system GPT partition information structure.
        ''' </summary>
        ''' <remarks></remarks>
        <StructLayout(LayoutKind.Sequential)>
        Public Structure PARTITION_INFORMATION_GPT
            Public PartitionType As Guid
            Public PartitionId As Guid
            Public Attributes As GptPartitionAttributes

            <MarshalAs(UnmanagedType.ByValArray, ArraySubType:=UnmanagedType.U2, SizeConst:=36)>
            Private _Name() As Char

            ''' <summary>
            ''' Returns the name of this partition.
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public ReadOnly Property Name As String
                Get
                    Return CStr(_Name).Trim(ChrW(0))
                End Get
            End Property

            ''' <summary>
            ''' Returns the partition code information for this structure (if any).
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public ReadOnly Property PartitionCode As GptCodeInfo
                Get
                    Return GptCodeInfo.FindByCode(PartitionType)
                End Get
            End Property

            Public Overrides Function ToString() As String
                If Name <> "" Then Return Name
                Dim c As GptCodeInfo = PartitionCode
                If c IsNot Nothing Then Return c.Name
                Return Nothing
            End Function

        End Structure

        ''' <summary>
        ''' Contains extended partition information for any kind of disk device with a partition table.
        ''' </summary>
        ''' <remarks></remarks>
        <StructLayout(LayoutKind.Sequential)>
        Public Structure PARTITION_INFORMATION_EX
            Public PartitionStyle As PartitionStyle
            Public StartingOffset As Long
            Public PartitionLength As Long
            Public PartitionNumber As UInteger

            <MarshalAs(UnmanagedType.Bool)>
            Public RewritePartition As Boolean

            <MarshalAs(UnmanagedType.ByValArray, ArraySubType:=UnmanagedType.U1, SizeConst:=112)>
            Private _PartitionInfo() As Byte

            ''' <summary>
            ''' Returns the Mbr structure or nothing if this partition is not an MBR partition.
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public ReadOnly Property Mbr As PARTITION_INFORMATION_MBR
                Get
                    If Me.PartitionStyle = PartitionStyle.Gpt Then Return Nothing

                    Dim mm As SafePtr = CType(_PartitionInfo, SafePtr)
                    Return mm.ToStruct(Of PARTITION_INFORMATION_MBR)()
                End Get
            End Property

            ''' <summary>
            ''' Returns the Gpt structure or nothing if this partition is not a GPT partition.
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public ReadOnly Property Gpt As PARTITION_INFORMATION_GPT
                Get
                    If Me.PartitionStyle = PartitionStyle.Mbr Then Return Nothing

                    Dim mm As SafePtr = CType(_PartitionInfo, SafePtr)
                    Return mm.ToStruct(Of PARTITION_INFORMATION_GPT)()
                End Get
            End Property

            Public Overrides Function ToString() As String
                If StartingOffset = 0 AndAlso PartitionLength = 0 Then Return Nothing
                Dim fs = TextTools.PrintFriendlySize(PartitionLength)

                If PartitionStyle = PartitionStyle.Mbr Then
                    ToString = Mbr.ToString & " [" & fs & "]"
                Else
                    ToString = Gpt.ToString & " [" & fs & "]"
                End If
            End Function

        End Structure

        ''' <summary>
        ''' Drive layout information for an MBR partition table.
        ''' </summary>
        ''' <remarks></remarks>
        <StructLayout(LayoutKind.Sequential)>
        Public Structure DRIVE_LAYOUT_INFORMATION_MBR
            Public Signature As UInteger
        End Structure

        ''' <summary>
        ''' Drive layout information for a GPT partition table.
        ''' </summary>
        ''' <remarks></remarks>
        <StructLayout(LayoutKind.Sequential)>
        Public Structure DRIVE_LAYOUT_INFORMATION_GPT
            Public DiskId As Guid
            Public StartingUsableOffset As Long
            Public UsableLength As Long
            Public MaxPartitionCount As UInteger
        End Structure

        ''' <summary>
        ''' Windows system disk drive partition layout information for any kind of disk.
        ''' </summary>
        ''' <remarks></remarks>
        <StructLayout(LayoutKind.Sequential)>
        Public Structure DRIVE_LAYOUT_INFORMATION_EX
            Public PartitionStyle As PartitionStyle
            Public ParititionCount As UInteger

            <MarshalAs(UnmanagedType.ByValArray, ArraySubType:=UnmanagedType.U1, SizeConst:=40)>
            Private _LayoutInfo() As Byte

            Public ReadOnly Property Mbr As DRIVE_LAYOUT_INFORMATION_MBR
                Get
                    Dim mm As SafePtr = CType(_LayoutInfo, SafePtr)
                    Return mm.ToStruct(Of DRIVE_LAYOUT_INFORMATION_MBR)()
                End Get
            End Property

            Public ReadOnly Property Gpt As DRIVE_LAYOUT_INFORMATION_GPT
                Get
                    Dim mm As SafePtr = CType(_LayoutInfo, SafePtr)
                    Return mm.ToStruct(Of DRIVE_LAYOUT_INFORMATION_GPT)()
                End Get
            End Property

        End Structure

        ''' <summary>
        ''' Enumerates all the partitions on a physical device.
        ''' </summary>
        ''' <param name="devicePath">The disk device path to query.</param>
        ''' <param name="hfile">Optional valid disk handle.</param>
        ''' <param name="layInfo">Optionally receives the layout information.</param>
        ''' <returns>An array of PARTITION_INFORMATION_EX structures.</returns>
        ''' <remarks></remarks>
        Public Function GetPartitions(devicePath As String, Optional hfile As IntPtr = Nothing, Optional ByRef layInfo As DRIVE_LAYOUT_INFORMATION_EX = Nothing) As PARTITION_INFORMATION_EX()

            Dim hf As Boolean = False

            If hfile <> IntPtr.Zero Then
                hf = True
            Else
                hfile = CreateFile(devicePath, GENERIC_READ, FILE_SHARE_READ Or FILE_SHARE_WRITE, IntPtr.Zero, OPEN_EXISTING, 0, IntPtr.Zero)
            End If

            Dim pex As New MemPtr
            Dim pexBegin As New MemPtr

            Dim pOut() As PARTITION_INFORMATION_EX = Nothing
            Dim lay As DRIVE_LAYOUT_INFORMATION_EX

            Dim pexLen As Integer = Marshal.SizeOf(Of PARTITION_INFORMATION_EX)

            Dim i As Integer
            Dim c As Integer
            Dim cb As UInteger = 0
            Dim sbs As Integer = 32768

            Dim succeed As Boolean = False

            If hfile = INVALID_HANDLE_VALUE Then Return Nothing

            Do
                pex.ReAlloc(sbs)
                succeed = DeviceIoControl(hfile,
                            IOCTL_DISK_GET_DRIVE_LAYOUT_EX,
                            IntPtr.Zero,
                            0,
                            pex.Handle,
                            CUInt(pex.Length),
                            cb,
                            IntPtr.Zero)

                If (Not succeed) Then
                    Dim xErr = GetLastError()

                    If (xErr <> DiskApi.ERROR_MORE_DATA) And (xErr <> DiskApi.ERROR_INSUFFICIENT_BUFFER) Then
                        Dim s = NativeError.Message
                        sbs = -1
                        Exit Do
                    End If
                End If

                sbs *= 2
            Loop Until succeed

            If (sbs = -1) Then
                pex.Free()
                If Not hf Then CloseHandle(hfile)

                Return Nothing
            End If

            lay = pex.ToStruct(Of DRIVE_LAYOUT_INFORMATION_EX)

            pexBegin.Handle = pex.Handle + 48

            c = CInt(lay.ParititionCount) - 1
            ReDim pOut(c)

            For i = 0 To c
                pOut(i) = pexBegin.ToStruct(Of PARTITION_INFORMATION_EX)
                pexBegin += pexLen
            Next

            pex.Free()

            If Not hf Then CloseHandle(hfile)

            layInfo = lay
            Return pOut

        End Function


    End Module

End Namespace

