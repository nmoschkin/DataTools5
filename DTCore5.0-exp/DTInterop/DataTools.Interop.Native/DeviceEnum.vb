'' ************************************************* ''
'' DataTools Visual Basic Utility Library - Interop
''
'' Module: DeviceEnum
''         Native.
'' 
'' Copyright (C) 2011-2020 Nathan Moschkin
'' All Rights Reserved
''
'' Licensed Under the Microsoft Public License   
'' ************************************************* ''

Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Security
Imports CoreCT.Memory
Imports System.Collections.ObjectModel
Imports DataTools.Interop.Desktop

Imports DataTools.Interop.Disk
Imports DataTools.Interop.Network
Imports DataTools.Interop.Display
Imports DataTools.Interop.Printers
Imports DataTools.Interop.Usb
Imports CoreCT.Text

Namespace Native
    ''' <summary>
    ''' Internal device enumeration functions module.
    ''' </summary>
    ''' <remarks></remarks>
    <HideModuleName, SecurityCritical>
    Friend Module DevEnumApi

        Public Const DICLASSPROP_INTERFACE = 2
        Public Const DICLASSPROP_INSTALLER = 1

        ''' <summary>
        ''' Returns an exhaustive hardware tree of the entire computer with as much information as can be obtained. Each object descends from DeviceInfo.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function _internalEnumComputerExhaustive() As ObservableCollection(Of Object)

            Dim col As New ObservableCollection(Of Object)

            Dim comp As DeviceInfo() = _internalGetComputer()

            Dim vol As DiskDeviceInfo() = EnumVolumes()
            Dim dsk As DiskDeviceInfo() = EnumDisks()
            Dim net As DeviceInfo() = EnumerateNetworkDevices()
            Dim hid As HidDeviceInfo() = EnumerateDevices(Of HidDeviceInfo)(GUID_DEVINTERFACE_HID)
            Dim prt As PrinterDeviceInfo() = EnumPrinters()

            Dim adpt As New AdaptersCollection

            'Dim part As DeviceInfo() = EnumerateDevices(Of DeviceInfo)(GUID_DEVINTERFACE_PARTITION)

            '' this collection will never be empty.
            Dim i As Integer,
            c As Integer = comp.Count - 1

            '' We are going to match up the raw device enumeration with the detailed device interface enumerations
            '' for the specific kinds of hardware that we know about (so far).  this is a work in progress
            '' and I will be doing more classes going forward.

            Try
                For i = 0 To c

                    If comp(i).DeviceClass = DeviceClassEnum.DiskDrive OrElse comp(i).DeviceClass = DeviceClassEnum.CdRom Then

                        Try
                            For Each d In dsk
                                Try
                                    If d.InstanceId.ToUpper.Trim = comp(i).InstanceId.ToUpper.Trim OrElse
                                d.PDOName.ToUpper.Trim = comp(i).PDOName.ToUpper.Trim Then

                                        d.DeviceClassIcon = comp(i).DeviceClassIcon
                                        comp(i) = d
                                        Exit For
                                    End If
                                Catch ex As Exception

                                End Try
                            Next

                        Catch ex As Exception

                        End Try

                    ElseIf comp(i).DeviceClass = DeviceClassEnum.Net Then

                        For Each nt In net
                            If nt.InstanceId.ToUpper.Trim = comp(i).InstanceId.ToUpper.Trim Then
                                nt.DeviceClassIcon = comp(i).DeviceClassIcon

                                For Each ad In adpt.Collection
                                    If ad.DeviceInfo Is Nothing Then Continue For
                                    If ad.DeviceInfo.InstanceId = nt.InstanceId Then
                                        nt.DeviceIcon = ad.DeviceIcon
                                    End If
                                Next

                                comp(i) = nt
                                Exit For
                            End If
                        Next

                    ElseIf comp(i).DeviceClass = DeviceClassEnum.Volume Then

                        For Each vl In vol
                            If vl.InstanceId.ToUpper.Trim = comp(i).InstanceId.ToUpper.Trim OrElse
                        vl.PDOName.ToUpper.Trim = comp(i).PDOName.ToUpper.Trim Then

                                If vl.VolumePaths IsNot Nothing AndAlso vl.VolumePaths.Count > 0 Then
                                    vl.DeviceIcon = MakeWPFImage(GetFileIcon(vl.VolumePaths(0), CType(SHIL_EXTRALARGE, SystemIconSizes)))
                                End If

                                vl.DeviceClassIcon = comp(i).DeviceClassIcon

                                comp(i) = vl
                                Exit For
                            End If
                        Next

                    ElseIf comp(i).DeviceClass = DeviceClassEnum.HidClass OrElse comp(i).BusType = BusType.HID Then

                        For Each hd In hid
                            If hd.InstanceId.ToUpper.Trim = comp(i).InstanceId.ToUpper.Trim Then
                                hd.DeviceClassIcon = comp(i).DeviceClassIcon
                                comp(i) = hd
                                Exit For
                            End If
                        Next
                    ElseIf comp(i).DeviceClass = DeviceClassEnum.PrinterQueue Then

                        For Each pr In prt
                            If comp(i).FriendlyName = pr.FriendlyName Then
                                comp(i) = pr
                                Exit For
                            End If
                        Next

                    Else
                        If comp(i).InstanceId.Substring(0, 8) = "BTHENUM\" Then
                            comp(i).DeviceClass = DeviceClassEnum.Bluetooth
                        End If

                        If comp(i).DeviceClass = DeviceClassEnum.Bluetooth Then

                            comp(i) = DeviceInfo.Duplicate(Of BluetoothDeviceInfo)(comp(i))

                        End If

                    End If

                Next


            Catch ex2 As Exception

            End Try

            '' Call the shared LinkDevices function to pair parents and offspring to create a coherent tree of the system.
            DeviceInfo.LinkDevices(comp)

            For Each dad In comp
                col.Add(dad)
            Next

            Return col

        End Function

        ''' <summary>
        ''' Enumerate (and optionally link) all computer hardware on the system.
        ''' </summary>
        ''' <param name="noLink">Optionally specify not to link parents to children.</param>
        ''' <returns>An array of DeviceInfo objects.</returns>
        ''' <remarks></remarks>
        Public Function _internalGetComputer(Optional noLink As Boolean = False, Optional classOnly As Guid = Nothing) As DeviceInfo()

            Dim devOut() As DeviceInfo = Nothing

            Dim c As Integer = 0
            Dim devInfo As SP_DEVINFO_DATA
            Dim devInterface As SP_DEVICE_INTERFACE_DATA
            Dim lIcon As New Dictionary(Of Guid, System.Drawing.Icon)
            Dim mm As New SafePtr

            Dim hicon As IntPtr = IntPtr.Zero,
            picon As Integer = 0

            Dim icn As Drawing.Icon = Nothing

            Dim hDev As IntPtr
            'classOnly = Guid.Empty
            If classOnly <> Guid.Empty Then
                hDev = SetupDiGetClassDevs(classOnly, IntPtr.Zero, IntPtr.Zero, CType(DIGCF_PRESENT, ClassDevFlags))

            Else
                hDev = SetupDiGetClassDevs(Nothing, IntPtr.Zero, IntPtr.Zero, CType(DIGCF_ALLCLASSES Or DIGCF_PRESENT Or DIGCF_DEVICEINTERFACE, ClassDevFlags))

            End If

            If hDev = INVALID_HANDLE_VALUE Then
                Return Nothing
            End If

            devInfo.cbSize = CUInt(Marshal.SizeOf(devInfo))
            devInterface.cbSize = CUInt(Marshal.SizeOf(devInterface))

            While SetupDiEnumDeviceInfo(hDev, CUInt(c), devInfo)

                Try
                    ReDim Preserve devOut(c)

                    devOut(c) = _internalPopulateDeviceInfo(Of DeviceInfo)(hDev, Nothing, devInfo, devInterface, mm)
                    SetupDiEnumDeviceInterfaces(hDev, IntPtr.Zero, devOut(c).DeviceClassGuid, CUInt(c), devInterface)

                    If devInterface.InterfaceClassGuid <> Guid.Empty Then
                        devOut(c).DeviceInterfaceClassGuid = devInterface.InterfaceClassGuid
                        devOut(c).DeviceInterfaceClass = GetDevInterfaceClassEnumFromGuid(devOut(c).DeviceInterfaceClassGuid)
                    End If

                    If Not lIcon.ContainsKey(devOut(c).DeviceClassGuid) Then
                        SetupDiLoadClassIcon(devOut(c).DeviceClassGuid, hicon, picon)
                        If hicon <> IntPtr.Zero Then
                            icn = CType(Drawing.Icon.FromHandle(hicon).Clone, Drawing.Icon)
                            DestroyIcon(hicon)
                        End If

                        lIcon.Add(devOut(c).DeviceClassGuid, icn)
                        devOut(c).DeviceClassIcon = icn
                    Else
                        lIcon.TryGetValue(devOut(c).DeviceClassGuid, devOut(c).DeviceClassIcon)
                    End If

                Catch ex As Exception

                    MsgBox(ex.Message & vbCrLf & vbCrLf & "Stack Trace: " & ex.StackTrace, MsgBoxStyle.Exclamation)
                End Try

                c += 1
            End While

            SetupDiDestroyDeviceInfoList(hDev)
            If Not noLink Then DeviceInfo.LinkDevices(devOut)

            Return devOut

        End Function

        Public Function _internalListDeviceProperties(info As DeviceInfo, Optional flags As ClassDevFlags = ClassDevFlags.Present Or ClassDevFlags.DeviceInterface, Optional useClassId As Boolean = True) As List(Of DEVPROPKEY)

            Dim nkey As New List(Of DEVPROPKEY)

            Dim c As UInteger = 0
            Dim mm As MemPtr

            Dim hDev As IntPtr

            Dim cid As Guid

            If useClassId Then
                cid = info.DeviceClassGuid
            Else
                cid = info.DeviceInterfaceClassGuid
            End If

            hDev = SetupDiGetClassDevs(cid, IntPtr.Zero, IntPtr.Zero, flags)

            If hDev = INVALID_HANDLE_VALUE Then
                Return Nothing
            End If

            SetupDiGetDevicePropertyKeys(hDev,
                                 info._devInfo,
                                 IntPtr.Zero,
                                 0UI,
                                 c,
                                 0)

            If c = 0 Then
                SetupDiDestroyDeviceInfoList(hDev)
                Return Nothing
            End If

            Dim devpsize As Integer = Marshal.SizeOf(DEVPKEY_Device_Address)
            mm.Alloc(c * devpsize)

            SetupDiGetDevicePropertyKeys(hDev,
                                 info._devInfo,
                                 mm,
                                 c,
                                 c,
                                 0)

            SetupDiDestroyDeviceInfoList(hDev)

            For i = 0 To CInt(c) - 1
                nkey.Add(mm.ToStructAt(Of DEVPROPKEY)(i * devpsize))
            Next

            mm.Free()

            Return nkey

        End Function

        ''' <summary>
        ''' Get an arbitrary device property from a previously-enumerated device.
        ''' </summary>
        ''' <param name="info">The DeviceInfo object of the device.</param>
        ''' <param name="prop">The DevPropKey value to retrieve.</param>
        ''' <param name="type">The property type of the value to retrieve.</param>
        ''' <param name="flags">Optional flags to pass.</param>
        ''' <param name="useClassId">Optional alternate class Id or interface Id to use.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function _internalGetProperty(info As DeviceInfo, prop As DEVPROPKEY, type As DevPropTypes, Optional flags As ClassDevFlags = ClassDevFlags.Present Or ClassDevFlags.DeviceInterface, Optional useClassId As Boolean = False) As Object

            Dim c As UInteger = 0
            Dim mm As MemPtr

            Dim hDev As IntPtr

            Dim cid As Guid

            If useClassId Then
                cid = info.DeviceClassGuid
            Else
                cid = info.DeviceInterfaceClassGuid
            End If

            hDev = SetupDiGetClassDevs(cid, IntPtr.Zero, IntPtr.Zero, flags)

            If hDev = INVALID_HANDLE_VALUE Then
                Return Nothing
            End If

            SetupDiGetDeviceProperty(hDev,
                                 info._devInfo,
                                 prop,
                                 CUInt(type),
                                 IntPtr.Zero,
                                 0,
                                 c,
                                 0)

            If c = 0 Then
                ' MsgBox(FormatLastError(Marshal.GetLastWin32Error))
                SetupDiDestroyDeviceInfoList(hDev)
                Return Nothing
            End If
            mm.Alloc(c)
            mm.ZeroMemory()

            SetupDiGetDeviceProperty(hDev,
                                 info._devInfo,
                                 prop,
                                 CUInt(type),
                                 mm,
                                 c,
                                 c,
                                 0)

            SetupDiDestroyDeviceInfoList(hDev)
            _internalGetProperty = _internalDevPropToObject(type, mm, CInt(c))
            mm.Free()

        End Function

        ''' <summary>
        ''' Enumerate devices and return an instance of a specific descendent of DeviceInfo.
        ''' </summary>
        ''' <typeparam name="T">The DeviceInfo type of objects to return.</typeparam>
        ''' <param name="ClassId">The Class Id or Interface Id to enumerate.</param>
        ''' <param name="flags">Optional flags.</param>
        ''' <returns>An array of T</returns>
        ''' <remarks></remarks>
        Public Function _internalEnumerateDevices(Of T As {DeviceInfo, New})(ClassId As Guid, Optional flags As ClassDevFlags = ClassDevFlags.Present Or ClassDevFlags.DeviceInterface) As T()
            Dim devOut() As T = Nothing

            Dim c As Integer = 0, cu As UInteger = 0
            Dim devInfo As SP_DEVINFO_DATA
            Dim devInterface As SP_DEVICE_INTERFACE_DATA
            Dim lIcon As New Dictionary(Of Guid, System.Drawing.Icon)
            Dim mm As New SafePtr

            Dim hicon As IntPtr = IntPtr.Zero,
            picon As Integer = 0

            Dim icn As Drawing.Icon = Nothing

            Dim hDev As IntPtr = SetupDiGetClassDevs(ClassId, IntPtr.Zero, IntPtr.Zero, flags)

            If hDev = INVALID_HANDLE_VALUE Then

                Return Nothing
            End If

            devInfo.cbSize = CUInt(Marshal.SizeOf(devInfo))
            devInterface.cbSize = CUInt(Marshal.SizeOf(devInterface))

            SetupDiLoadClassIcon(ClassId, hicon, picon)

            If hicon <> IntPtr.Zero Then
                icn = CType(Drawing.Icon.FromHandle(hicon).Clone, Drawing.Icon)
                DestroyIcon(hicon)
            End If

            If (flags And ClassDevFlags.DeviceInterface) = ClassDevFlags.DeviceInterface Then

                While SetupDiEnumDeviceInterfaces(hDev, IntPtr.Zero, ClassId, cu, devInterface)
                    c = CInt(cu)

                    ReDim Preserve devOut(c)
                    SetupDiEnumDeviceInfo(hDev, cu, devInfo)

                    devOut(c) = _internalPopulateDeviceInfo(Of T)(hDev, ClassId, devInfo, devInterface, mm)

                    devOut(c).DeviceClassIcon = icn
                    cu += CUInt(1)
                End While

            Else

                While SetupDiEnumDeviceInfo(hDev, cu, devInfo)
                    c = CInt(cu)
                    ReDim Preserve devOut(c)

                    devOut(c) = _internalPopulateDeviceInfo(Of T)(hDev, Nothing, devInfo, devInterface, mm)
                    SetupDiEnumDeviceInterfaces(hDev, IntPtr.Zero, devOut(c).DeviceClassGuid, cu, devInterface)

                    If devInterface.InterfaceClassGuid <> Guid.Empty Then
                        devOut(c).DeviceInterfaceClassGuid = devInterface.InterfaceClassGuid
                        devOut(c).DeviceInterfaceClass = GetDevInterfaceClassEnumFromGuid(devOut(c).DeviceInterfaceClassGuid)
                    End If

                    devOut(c).DeviceClassIcon = icn
                    cu += CUInt(1)
                End While

                'If c = 0 Then
                '    MsgBox("Internal error: " & FormatLastError(Marshal.GetLastWin32Error))
                'End If

            End If

            mm.Dispose()
            SetupDiDestroyDeviceInfoList(hDev)
            Return devOut

        End Function

        Public Function GetClassIcon(ClassId As Guid) As System.Drawing.Icon

            Dim mm As New SafePtr

            Dim hicon As IntPtr = IntPtr.Zero,
            picon As Integer = 0

            Dim icn As Drawing.Icon = Nothing

            Dim hDev As IntPtr = SetupDiGetClassDevs(ClassId, IntPtr.Zero, IntPtr.Zero, 0)

            If hDev = INVALID_HANDLE_VALUE Then
                Return Nothing
            End If

            SetupDiLoadClassIcon(ClassId, hicon, Nothing)

            If hicon <> IntPtr.Zero Then
                icn = CType(Drawing.Icon.FromHandle(hicon).Clone, Drawing.Icon)
                DestroyIcon(hicon)
            End If

            SetupDiDestroyDeviceInfoList(hDev)
            Return icn

        End Function

        ''' <summary>
        ''' Enumerate all local physical and virtual disk drives, including optical drives or volumes, depending on the class Id.
        ''' </summary>
        ''' <param name="DiskClass">The disk-type class Id or interface Id to use.</param>
        ''' <returns>An array of DiskDeviceInfo objects.</returns>
        ''' <remarks></remarks>
        Public Function _internalEnumDisks(Optional DiskClass As Guid = Nothing) As DiskDeviceInfo()

            Dim hHeap As New SafePtr
            'hHeap.IsString = True

            Try


                Dim info As DiskDeviceInfo() = Nothing
                If DiskClass = Guid.Empty Then DiskClass = GUID_DEVINTERFACE_DISK

                Dim disk As IntPtr = INVALID_HANDLE_VALUE
                Dim diskNumber As New STORAGE_DEVICE_NUMBER
                Dim bytesReturned As UInteger = 0
                Dim storageDeviceNumber As New STORAGE_DEVICE_NUMBER

                info = EnumerateDevices(Of DiskDeviceInfo)(DiskClass)
                If info Is Nothing OrElse info.Count = 0 Then Return {}

                For Each inf In info

                    inf.Capabilities = CType(_internalGetProperty(inf, DEVPKEY_Device_Capabilities, DevPropTypes.Int32), DeviceCapabilities)
                    If inf.Capabilities = 0 Then
                        inf.Capabilities = CType(_internalGetProperty(inf, DEVPKEY_Device_Capabilities, DevPropTypes.Int32, , True), DeviceCapabilities)
                    End If

                    If inf.DeviceClass = DeviceClassEnum.CdRom Then
                        inf.Type = StorageType.Optical
                    ElseIf inf.RemovalPolicy <> DeviceRemovalPolicy.ExpectNoRemoval Then

                        '' this is a conundrum because these values are not predictable in some cases.
                        '' we'll leave it this way, for now.
                        inf.Type = StorageType.Removable

                        'If inf.Capabilities And DeviceCapabilities.Removable Then
                        '    inf.Type = StorageType.RemovableHardDisk
                        'Else
                        '    inf.Type = StorageType.Removable
                        'End If

                    End If

                    If (DiskClass = GUID_DEVINTERFACE_VOLUME) OrElse (DiskClass = GUID_DEVCLASS_VOLUME) Then
                        inf.IsVolume = True

                        disk = CreateFile(inf.DevicePath,
                        GENERIC_READ,
                        FILE_SHARE_READ Or FILE_SHARE_WRITE,
                        IntPtr.Zero,
                        OPEN_EXISTING,
                        FILE_ATTRIBUTE_NORMAL,
                        IntPtr.Zero)

                        If disk <> INVALID_HANDLE_VALUE Then
                            PopulateVolumeInfo(inf, disk)

                            hHeap.Length = Marshal.SizeOf(diskNumber)
                            hHeap.ZeroMemory()
                            bytesReturned = 0

                            DeviceIoControl(disk,
                            IOCTL_STORAGE_GET_DEVICE_NUMBER,
                            Nothing,
                            0,
                            hHeap.handle,
                            CUInt(hHeap.Length),
                            bytesReturned,
                            Nothing)

                            If bytesReturned > 0 Then
                                diskNumber = hHeap.ToStruct(Of STORAGE_DEVICE_NUMBER)()
                                inf.PhysicalDevice = CInt(diskNumber.DeviceNumber)

                                If diskNumber.PartitionNumber < 1024 Then
                                    inf.PartitionNumber = CInt(diskNumber.PartitionNumber)
                                End If
                            End If

                            CloseHandle(disk)
                        Else
                            PopulateVolumeInfo(inf)
                        End If

                    ElseIf inf.Type <> StorageType.Optical Then

                        disk = CreateFile(inf.DevicePath,
                        GENERIC_READ,
                        FILE_SHARE_READ Or FILE_SHARE_WRITE,
                        IntPtr.Zero,
                        OPEN_EXISTING,
                        FILE_ATTRIBUTE_NORMAL,
                        IntPtr.Zero)

                        If disk <> INVALID_HANDLE_VALUE Then

                            hHeap.Length = 128
                            hHeap.ZeroMemory()

                            DeviceIoControl(disk,
                            IOCTL_DISK_GET_LENGTH_INFO,
                            Nothing,
                            0,
                            hHeap.handle,
                            CUInt(hHeap.Length),
                            bytesReturned,
                            Nothing)

                            inf.DiskLayout = DiskLayoutInfo.CreateLayout(disk)
                            inf.Size = hHeap.LongAt(0)

                            hHeap.Length = Marshal.SizeOf(diskNumber)
                            hHeap.ZeroMemory()
                            bytesReturned = 0

                            DeviceIoControl(disk,
                            IOCTL_STORAGE_GET_DEVICE_NUMBER,
                            Nothing,
                            0,
                            hHeap.handle,
                            CUInt(hHeap.Length),
                            bytesReturned,
                            Nothing)

                            If bytesReturned > 0 Then
                                diskNumber = hHeap.ToStruct(Of STORAGE_DEVICE_NUMBER)()
                                inf.PhysicalDevice = CInt(diskNumber.DeviceNumber)
                            End If

                            CloseHandle(disk)
                            disk = INVALID_HANDLE_VALUE

                        End If
                    End If

                    If inf.FriendlyName = "Microsoft Virtual Disk" Then
                        inf.Type = StorageType.Virtual

                        disk = CreateFile("\\.\PhysicalDrive" & inf.PhysicalDevice,
                        GENERIC_READ,
                        FILE_SHARE_READ Or FILE_SHARE_WRITE,
                        IntPtr.Zero,
                        OPEN_EXISTING,
                        FILE_ATTRIBUTE_NORMAL,
                        IntPtr.Zero)

                        If disk = INVALID_HANDLE_VALUE Then Continue For

                        Dim sdi As STORAGE_DEPENDENCY_INFO_V2
                        Dim mm As New SafePtr
                        Dim su As UInteger = 0
                        Dim r As UInteger
                        Dim sdic As Integer = Marshal.SizeOf(sdi)

                        mm.Length = sdic

                        sdi.Version = STORAGE_DEPENDENCY_INFO_VERSION.STORAGE_DEPENDENCY_INFO_VERSION_2

                        mm.IntAt(0) = STORAGE_DEPENDENCY_INFO_VERSION.STORAGE_DEPENDENCY_INFO_VERSION_2
                        mm.IntAt(1) = 1

                        r = GetStorageDependencyInformation(disk, GET_STORAGE_DEPENDENCY_FLAG.GET_STORAGE_DEPENDENCY_FLAG_HOST_VOLUMES Or GET_STORAGE_DEPENDENCY_FLAG.GET_STORAGE_DEPENDENCY_FLAG_DISK_HANDLE, CUInt(mm.Length), mm, su)
                        If su <> sdic Then
                            mm.Length = su
                            r = GetStorageDependencyInformation(disk, GET_STORAGE_DEPENDENCY_FLAG.GET_STORAGE_DEPENDENCY_FLAG_HOST_VOLUMES Or GET_STORAGE_DEPENDENCY_FLAG.GET_STORAGE_DEPENDENCY_FLAG_DISK_HANDLE, CUInt(mm.Length), mm, su)
                        End If

                        If r <> 0 Then
                            MsgBox(FormatLastError, vbExclamation)

                        Else
                            sdi.NumberEntries = mm.UIntAt(1)
                            ReDim inf.BackingStore(CInt(sdi.NumberEntries) - 1)

                            For d = 0 To sdi.NumberEntries - 1
                                sdi.Version2Entries = mm.ToStruct(Of STORAGE_DEPENDENCY_INFO_TYPE_2)
                                inf.BackingStore(CInt(d)) = (Path.GetFullPath(sdi.Version2Entries.DependentVolumeRelativePath.ToString()))
                            Next
                        End If

                        mm.Dispose()

                        CloseHandle(disk)

                        inf.VirtualDisk = New VirtualDisk(inf, False)
                        disk = INVALID_HANDLE_VALUE
                    End If
                Next

                If hHeap IsNot Nothing Then hHeap.Dispose()
                Return info

            Catch ex As Exception

                If hHeap IsNot Nothing Then hHeap.Dispose()
                Return Nothing

            End Try

        End Function

        Friend ReadOnly StandardStagingClasses As DeviceClassEnum() =
                             {DeviceClassEnum.Adapter,
                              DeviceClassEnum.Battery,
                              DeviceClassEnum.Biometric,
                              DeviceClassEnum.Bluetooth,
                              DeviceClassEnum.Infrared,
                              DeviceClassEnum.HidClass,
                              DeviceClassEnum.Infrared,
                              DeviceClassEnum.Keyboard,
                              DeviceClassEnum.Media,
                              DeviceClassEnum.Monitor,
                              DeviceClassEnum.Mouse,
                              DeviceClassEnum.Multifunction,
                              DeviceClassEnum.PnpPrinters,
                              DeviceClassEnum.Printer,
                              DeviceClassEnum.PrinterQueue,
                              DeviceClassEnum.Sound,
                              DeviceClassEnum.Usb}

        Friend ReadOnly ExtraStagingClasses As DeviceClassEnum() =
                             {DeviceClassEnum.Adapter,
                              DeviceClassEnum.Battery,
                              DeviceClassEnum.Biometric,
                              DeviceClassEnum.Bluetooth,
                              DeviceClassEnum.CdRom,
                              DeviceClassEnum.DiskDrive,
                              DeviceClassEnum.FloppyDisk,
                              DeviceClassEnum.Infrared,
                              DeviceClassEnum.HidClass,
                              DeviceClassEnum.Infrared,
                              DeviceClassEnum.Keyboard,
                              DeviceClassEnum.Media,
                              DeviceClassEnum.MediumChanger,
                              DeviceClassEnum.Modem,
                              DeviceClassEnum.Monitor,
                              DeviceClassEnum.Mouse,
                              DeviceClassEnum.Multifunction,
                              DeviceClassEnum.Pcmcia,
                              DeviceClassEnum.PnpPrinters,
                              DeviceClassEnum.Printer,
                              DeviceClassEnum.PrinterQueue,
                              DeviceClassEnum.PrinterUpgrade,
                              DeviceClassEnum.SmartCardReader,
                              DeviceClassEnum.Sound,
                              DeviceClassEnum.TapeDrive,
                              DeviceClassEnum.Usb}

        ''' <summary>
        ''' Gets the device icon in some way, including looking at the device stage to get the photo-realistic icons from the Devices and Printers control panel folder.
        ''' </summary>
        ''' <param name="hDev"></param>
        ''' <param name="devInfo"></param>
        ''' <param name="infoOut"></param>
        ''' <remarks></remarks>
        Public Sub _internalGetDeviceIcon(hDev As IntPtr, devInfo As SP_DEVINFO_DATA, infoOut As DeviceInfo, Optional stagingClasses As DeviceClassEnum() = Nothing, Optional noStaging As Boolean = False)

            Dim hIcon As IntPtr

            If stagingClasses Is Nothing Then
                stagingClasses = StandardStagingClasses
            End If

            If infoOut.DeviceIcon Is Nothing Then

                Dim sKey As String = "::{20D04FE0-3AEA-1069-A2D8-08002B30309D}\::{21EC2020-3AEA-1069-A2DD-08002B30309D}\::{A8A91A66-3A7D-4424-8D24-04E180695C7A}"
                Dim pKey = "\Provider%5CMicrosoft.Base.DevQueryObjects//DDO:"

                If infoOut.ContainerId <> Guid.Empty And noStaging = False Then

                    If stagingClasses.Contains(infoOut.DeviceClass) Then
                        Dim rKey As String = "%7B" & infoOut.ContainerId.ToString("D").ToUpper & "%7D"

                        Dim mm As New MemPtr
                        Dim s As String = sKey & pKey & rKey

                        SHParseDisplayName(s, IntPtr.Zero, mm, 0, 0)

                        If mm <> IntPtr.Zero Then
                            '' Get a WPFImage
                            infoOut.DeviceIcon = MakeWPFImage(GetItemIcon(mm, CType(SHIL_EXTRALARGE, SystemIconSizes)))
                            mm.Free()

                        End If

                    End If
                End If

                If infoOut.DeviceIcon Is Nothing Then
                    If SetupDiLoadDeviceIcon(hDev, devInfo, 48, 48, 0, hIcon) Then

                        Dim icn = System.Drawing.Icon.FromHandle(hIcon)
                        Dim icn2 As System.Drawing.Icon = CType(icn.Clone, Drawing.Icon)

                        DestroyIcon(hIcon)
                        icn.Dispose()

                        infoOut.DeviceIcon = MakeWPFImage(icn2)
                        icn2.Dispose()

                    End If

                End If

            End If

        End Sub

        ''' <summary>
        ''' Instantiates and populates a DeviceInfo-derived object with most of the common properties using an open handle to a device enumerator.
        ''' </summary>
        ''' <typeparam name="T">The type of DeviceInfo-derived object to return.</typeparam>
        ''' <param name="hDev">A valid device enumerator handle.</param>
        ''' <param name="ClassId">The class or interface Id.</param>
        ''' <param name="devInfo">The raw SP_DEVINFO_DATA</param>
        ''' <param name="devInterface">The raw SP_DEVICE_INTERFACE_DATA</param>
        ''' <param name="mm">An open memory object.</param>
        ''' <returns>A new instance of T.</returns>
        ''' <remarks></remarks>
        Public Function _internalPopulateDeviceInfo(Of T As {DeviceInfo, New})(hDev As IntPtr,
                                                                    ClassId As Guid,
                                                                    devInfo As SP_DEVINFO_DATA,
                                                                    devInterface As SP_DEVICE_INTERFACE_DATA,
                                                                    mm As SafePtr) As T

            Dim dumpFile As String = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\datatools-error.log"
            Dim devOut As New T
            Dim cbId As Integer = 0
            Dim cbSize As UInteger = 0
            Dim details As SP_DEVICE_INTERFACE_DETAIL_DATA
            Dim pt As MemPtr

            Dim sb As New System.Text.StringBuilder

            Try
                sb.AppendLine("Class Guid: " & ClassId.ToString("B"))
                sb.AppendLine("Getting DeviceInterfaceDetail")
                sb.AppendLine("DevInfo cbSize: " & devInfo.cbSize)
                sb.AppendLine("DevInfo DevInst: " & devInfo.DevInst)

                devOut._devInfo = devInfo
                devOut.DeviceInterfaceClassGuid = devInterface.InterfaceClassGuid
                devOut.DeviceInterfaceClass = GetDevInterfaceClassEnumFromGuid(devOut.DeviceInterfaceClassGuid)

                '' Get the DevicePath from DeviceInterfaceDetail
                mm.Length = 0
                mm.Length = Marshal.SizeOf(details)

                SetupDiGetDeviceInterfaceDetail(hDev,
                            devInterface,
                            IntPtr.Zero,
                            0,
                            cbSize,
                            IntPtr.Zero)

                If cbSize > 0 Then

                    mm.Length = cbSize
                    mm.IntAt(0) = If(IntPtr.Size = 4, 6, 8)

                    If cbSize > 4 Then

                        If SetupDiGetDeviceInterfaceDetail(hDev,
                            devInterface,
                            mm.handle,
                            cbSize,
                            Nothing,
                            Nothing) Then

                            'mm.PullIn(0, 4)
                            devOut.DevicePath = mm.GetString(4)

                        End If

                    End If

                    sb.AppendLine("Getting DeviceInterfaceDetail Succeed")

                End If
                '' ClassGuid

                sb.AppendLine("Property ClassGuid")

                cbSize = 0
                SetupDiGetDeviceProperty(hDev,
                devInfo,
                DEVPKEY_Device_ClassGuid,
                DEVPROP_TYPE_GUID,
                IntPtr.Zero,
                0UI,
                cbSize,
                0)

                If cbSize > 0 Then
                    mm.Length = cbSize
                    mm.ZeroMemory()

                    SetupDiGetDeviceProperty(hDev,
                    devInfo,
                    DEVPKEY_Device_ClassGuid,
                    DEVPROP_TYPE_GUID,
                    mm,
                    cbSize,
                    cbSize,
                    0)

                    If cbSize > 0 Then
                        devOut.DeviceClassGuid = New Guid(mm.ToByteArray(0, 16))
                        devOut.DeviceClass = GetDevClassEnumFromGuid(devOut.DeviceClassGuid)
                    End If
                End If
                '' InterfaceClassGuid

                sb.AppendLine("Property InterfaceClassGuid")
                cbSize = 0
                SetupDiGetDeviceProperty(hDev,
                devInfo,
                DEVPKEY_DeviceInterface_ClassGuid,
                DEVPROP_TYPE_GUID,
                IntPtr.Zero,
                0UI,
                cbSize,
                0)

                If cbSize > 0 Then
                    mm.Length = cbSize
                    mm.ZeroMemory()

                    SetupDiGetDeviceProperty(hDev,
                    devInfo,
                    DEVPKEY_DeviceInterface_ClassGuid,
                    DEVPROP_TYPE_GUID,
                    mm,
                    cbSize,
                    cbSize,
                    0)

                    If cbSize > 0 Then
                        devOut.DeviceInterfaceClassGuid = New Guid(mm.ToByteArray(0, 16))
                        devOut.DeviceInterfaceClass = GetDevInterfaceClassEnumFromGuid(devOut.DeviceInterfaceClassGuid)
                    End If
                End If

                '' InstallDate

                sb.AppendLine("Property InstallDate")
                cbSize = 0
                SetupDiGetDeviceProperty(hDev,
                devInfo,
                DEVPKEY_Device_InstallDate,
                DEVPROP_TYPE_FILETIME,
                IntPtr.Zero,
                0UI,
                cbSize,
                0)

                If cbSize > 0 Then
                    mm.Length = cbSize
                    mm.ZeroMemory()

                    SetupDiGetDeviceProperty(hDev,
                    devInfo,
                    DEVPKEY_Device_InstallDate,
                    DEVPROP_TYPE_FILETIME,
                    mm,
                    cbSize,
                    cbSize,
                    0)

                    If cbSize > 0 Then devOut.InstallDate = CDate(_internalDevPropToObject(DevPropTypes.FileTime, mm, CInt(cbSize)))
                End If
                '' Characteristics

                sb.AppendLine("Property Characteristics")
                cbSize = 0
                SetupDiGetDeviceProperty(hDev,
                devInfo,
                DEVPKEY_Device_Characteristics,
                DEVPROP_TYPE_INT32,
                IntPtr.Zero,
                0UI,
                cbSize,
                0)

                If cbSize > 0 Then
                    mm.Length = cbSize
                    mm.ZeroMemory()

                    SetupDiGetDeviceProperty(hDev,
                    devInfo,
                    DEVPKEY_Device_Characteristics,
                    DEVPROP_TYPE_INT32,
                    mm,
                    cbSize,
                    cbSize,
                    0)

                    If cbSize > 0 Then devOut.Characteristics = CType(mm.IntAt(0), DeviceCharacteristcs)
                End If
                '' Removal Policy

                sb.AppendLine("Property Removal Policy")
                cbSize = 0
                SetupDiGetDeviceProperty(hDev,
                devInfo,
                DEVPKEY_Device_RemovalPolicy,
                DEVPROP_TYPE_INT32,
                IntPtr.Zero,
                0UI,
                cbSize,
                0)

                If cbSize > 0 Then
                    mm.Length = cbSize
                    mm.ZeroMemory()

                    SetupDiGetDeviceProperty(hDev,
                    devInfo,
                    DEVPKEY_Device_RemovalPolicy,
                    DEVPROP_TYPE_INT32,
                    mm,
                    cbSize,
                    cbSize,
                    0)

                    If cbSize > 0 Then devOut.RemovalPolicy = CType(mm.IntAt(0), DeviceRemovalPolicy)
                End If
                '' Safe Removal Required

                sb.AppendLine("Property Safe Removal Required")
                cbSize = 0
                SetupDiGetDeviceProperty(hDev,
                devInfo,
                DEVPKEY_Device_SafeRemovalRequired,
                DEVPROP_TYPE_BOOLEAN,
                IntPtr.Zero,
                0UI,
                cbSize,
                0)

                If cbSize > 0 Then
                    mm.Length = cbSize
                    mm.ZeroMemory()

                    SetupDiGetDeviceProperty(hDev,
                    devInfo,
                    DEVPKEY_Device_SafeRemovalRequired,
                    DEVPROP_TYPE_BOOLEAN,
                    mm,
                    cbSize,
                    cbSize,
                    0)

                    If cbSize > 0 Then devOut.SafeRemovalRequired = CBool(mm.ByteAt(0))
                End If

                '' BusType

                sb.AppendLine("Property BusType")
                cbSize = 0
                SetupDiGetDeviceProperty(hDev,
                devInfo,
                DEVPKEY_Device_BusTypeGuid,
                DEVPROP_TYPE_GUID,
                IntPtr.Zero,
                0UI,
                cbSize,
                0)

                If cbSize > 0 Then
                    mm.Length = cbSize
                    mm.ZeroMemory()

                    SetupDiGetDeviceProperty(hDev,
                    devInfo,
                    DEVPKEY_Device_BusTypeGuid,
                    DEVPROP_TYPE_GUID,
                    mm,
                    cbSize,
                    cbSize,
                    0)

                    If cbSize > 0 Then devOut.BusType = GuidToBusType(New Guid(mm.ToByteArray(0, 16)))
                End If
                '' ContainerId

                sb.AppendLine("Property ContainerId")
                cbSize = 0
                SetupDiGetDeviceProperty(hDev,
                devInfo,
                DEVPKEY_Device_ContainerId,
                DEVPROP_TYPE_GUID,
                IntPtr.Zero,
                0UI,
                cbSize,
                0)

                If cbSize > 0 Then
                    mm.Length = cbSize
                    mm.ZeroMemory()

                    SetupDiGetDeviceProperty(hDev,
                    devInfo,
                    DEVPKEY_Device_ContainerId,
                    DEVPROP_TYPE_GUID,
                    mm,
                    cbSize,
                    cbSize,
                    0)

                    If cbSize > 0 Then devOut.ContainerId = New Guid(mm.ToByteArray(0, 16))
                End If

                '' Children

                sb.AppendLine("Property Children")
                cbSize = 0
                SetupDiGetDeviceProperty(hDev,
                devInfo,
                DEVPKEY_Device_Children,
                DEVPROP_TYPE_STRING_LIST,
                IntPtr.Zero,
                0UI,
                cbSize,
                0)

                If cbSize > 0 Then
                    mm.Length = (cbSize + 4) * 2
                    mm.ZeroMemory()

                    SetupDiGetDeviceProperty(hDev,
                    devInfo,
                    DEVPKEY_Device_Children,
                    DEVPROP_TYPE_STRING_LIST,
                    mm.handle,
                    cbSize,
                    cbSize,
                    0)

                    pt = mm.handle
                    If cbSize > 0 Then devOut.Children = pt.GetStringArray(0)
                End If
                '' HardwareIds

                sb.AppendLine("Property HardwareIds")
                cbSize = 0
                SetupDiGetDeviceProperty(hDev,
                devInfo,
                DEVPKEY_Device_HardwareIds,
                DEVPROP_TYPE_STRING_LIST,
                IntPtr.Zero,
                0UI,
                cbSize,
                0)

                If cbSize > 0 Then
                    mm.Length = (cbSize + 4) * 2
                    mm.ZeroMemory()

                    SetupDiGetDeviceProperty(hDev,
                    devInfo,
                    DEVPKEY_Device_HardwareIds,
                    DEVPROP_TYPE_STRING_LIST,
                    mm,
                    cbSize,
                    cbSize,
                    0)

                    pt = mm.handle

                    If cbSize > 0 Then devOut.HardwareIds = pt.GetStringArray(0)
                End If
                '' LocationPaths

                sb.AppendLine("Property LocationPaths")
                cbSize = 0
                SetupDiGetDeviceProperty(hDev,
                devInfo,
                DEVPKEY_Device_LocationPaths,
                DEVPROP_TYPE_STRING_LIST,
                IntPtr.Zero,
                0UI,
                cbSize,
                0)

                If cbSize > 0 Then
                    mm.Length = cbSize
                    mm.ZeroMemory()

                    SetupDiGetDeviceProperty(hDev,
                    devInfo,
                    DEVPKEY_Device_LocationPaths,
                    DEVPROP_TYPE_STRING_LIST,
                    mm,
                    cbSize,
                    cbSize,
                    0)

                    pt = mm.handle
                    If cbSize > 0 Then devOut.LocationPaths = pt.GetStringArray(0)
                End If
                '' Parent Device

                sb.AppendLine("Property Parent Device")
                cbSize = 0
                SetupDiGetDeviceProperty(hDev,
                devInfo,
                DEVPKEY_Device_Parent,
                DEVPROP_TYPE_STRING,
                IntPtr.Zero,
                0UI,
                cbSize,
                0)

                If cbSize > 0 Then
                    mm.Length = cbSize
                    mm.ZeroMemory()

                    SetupDiGetDeviceProperty(hDev,
                    devInfo,
                    DEVPKEY_Device_Parent,
                    DEVPROP_TYPE_STRING,
                    mm,
                    cbSize,
                    cbSize,
                    0)

                    If cbSize > 0 Then devOut.Parent = mm.ToString
                End If
                '' Location Info

                sb.AppendLine("Property Location Info")
                cbSize = 0
                SetupDiGetDeviceProperty(hDev,
                devInfo,
                DEVPKEY_Device_LocationInfo,
                DEVPROP_TYPE_STRING,
                IntPtr.Zero,
                0UI,
                cbSize,
                0)

                If cbSize > 0 Then
                    mm.Length = cbSize
                    mm.ZeroMemory()

                    SetupDiGetDeviceProperty(hDev,
                    devInfo,
                    DEVPKEY_Device_LocationInfo,
                    DEVPROP_TYPE_STRING,
                    mm,
                    cbSize,
                    cbSize,
                    0)

                    If cbSize > 0 Then devOut.LocationInfo = mm.ToString
                End If
                '' Physical Device Location

                sb.AppendLine("Property Physical Device Location")
                sb.AppendLine("Getting cbSize")
                cbSize = 0
                SetupDiGetDeviceProperty(hDev,
                devInfo,
                DEVPKEY_Device_PhysicalDeviceLocation,
                DEVPROP_TYPE_BINARY,
                IntPtr.Zero,
                0UI,
                cbSize,
                0)

                sb.AppendLine("cbSize is " & cbSize)

                If cbSize > 0 Then

                    mm.Length = cbSize
                    mm.ZeroMemory()

                    sb.AppendLine("Calling to get Physical Device Location")

                    SetupDiGetDeviceProperty(hDev,
                    devInfo,
                    DEVPKEY_Device_PhysicalDeviceLocation,
                    DEVPROP_TYPE_BINARY,
                    mm,
                    cbSize,
                    cbSize,
                    0)

                    If cbSize > 0 Then
                        sb.AppendLine("Grabbing bytes")
                        devOut.PhysicalPath = mm.ToByteArray(0, CInt(cbSize))
                    End If

                End If
                '' PDOName

                sb.AppendLine("Property PDOName")
                cbSize = 0
                SetupDiGetDeviceProperty(hDev,
                devInfo,
                DEVPKEY_Device_PDOName,
                DEVPROP_TYPE_STRING,
                IntPtr.Zero,
                0UI,
                cbSize,
                0)

                If cbSize > 0 Then
                    mm.Length = cbSize
                    mm.ZeroMemory()

                    SetupDiGetDeviceProperty(hDev,
                    devInfo,
                    DEVPKEY_Device_PDOName,
                    DEVPROP_TYPE_STRING,
                    mm,
                    cbSize,
                    cbSize,
                    0)

                    If cbSize > 0 Then devOut.PDOName = mm.ToString
                End If
                '' Description

                sb.AppendLine("Property Description")
                cbSize = 0
                SetupDiGetDeviceProperty(hDev,
                devInfo,
                DEVPKEY_Device_DeviceDesc,
                DEVPROP_TYPE_STRING,
                IntPtr.Zero,
                0UI,
                cbSize,
                0)

                If cbSize > 0 Then
                    mm.Length = cbSize
                    mm.ZeroMemory()

                    SetupDiGetDeviceProperty(hDev,
                    devInfo,
                    DEVPKEY_Device_DeviceDesc,
                    DEVPROP_TYPE_STRING,
                    mm,
                    cbSize,
                    cbSize,
                    0)

                    If cbSize > 0 Then devOut.Description = mm.ToString
                End If
                '' ClassName

                sb.AppendLine("Property ClassName")
                cbSize = 0
                SetupDiGetDeviceProperty(hDev,
                devInfo,
                DEVPKEY_Device_Class,
                DEVPROP_TYPE_STRING,
                IntPtr.Zero,
                0UI,
                cbSize,
                0)

                If cbSize > 0 Then

                    mm.Length = cbSize
                    mm.ZeroMemory()

                    SetupDiGetDeviceProperty(hDev,
                    devInfo,
                    DEVPKEY_Device_Class,
                    DEVPROP_TYPE_STRING,
                    mm,
                    cbSize,
                    cbSize,
                    0)

                    If cbSize > 0 Then devOut.ClassName = mm.ToString
                End If

                '' Manufacturer

                sb.AppendLine("Property Manufacturer")
                cbSize = 0
                SetupDiGetDeviceProperty(hDev,
                devInfo,
                DEVPKEY_Device_Manufacturer,
                DEVPROP_TYPE_STRING,
                IntPtr.Zero,
                0UI,
                cbSize,
                0)

                If cbSize > 0 Then
                    mm.Length = cbSize
                    mm.ZeroMemory()

                    SetupDiGetDeviceProperty(hDev,
                    devInfo,
                    DEVPKEY_Device_Manufacturer,
                    DEVPROP_TYPE_STRING,
                    mm,
                    cbSize,
                    cbSize,
                    0)

                    If cbSize > 0 Then devOut.Manufacturer = mm.ToString
                End If

                '' Model

                sb.AppendLine("Property BusReportedDeviceDesc (string)")
                cbSize = 0
                SetupDiGetDeviceProperty(hDev,
                devInfo,
                DEVPKEY_Device_BusReportedDeviceDesc,
                DEVPROP_TYPE_STRING,
                IntPtr.Zero,
                0UI,
                cbSize,
                0)

                If cbSize > 0 Then
                    mm.Length = cbSize
                    mm.ZeroMemory()

                    SetupDiGetDeviceProperty(hDev,
                    devInfo,
                    DEVPKEY_Device_BusReportedDeviceDesc,
                    DEVPROP_TYPE_STRING,
                    mm,
                    cbSize,
                    cbSize,
                    0)

                    If cbSize > 0 Then devOut.BusReportedDeviceDesc = mm.ToString
                End If

                '' ModelId

                sb.AppendLine("Property ModelId")
                cbSize = 0
                SetupDiGetDeviceProperty(hDev,
                devInfo,
                DEVPKEY_Device_ModelId,
                DEVPROP_TYPE_GUID,
                IntPtr.Zero,
                0UI,
                cbSize,
                0)

                If cbSize > 0 Then
                    mm.Length = cbSize
                    mm.ZeroMemory()

                    SetupDiGetDeviceProperty(hDev,
                    devInfo,
                    DEVPKEY_Device_ModelId,
                    DEVPROP_TYPE_GUID,
                    mm,
                    cbSize,
                    cbSize,
                    0)

                    If cbSize > 0 Then
                        devOut.ModelId = mm.GuidAt(0)
                    End If
                End If

                '' UINumber

                sb.AppendLine("Property UINumber")
                cbSize = 0
                SetupDiGetDeviceProperty(hDev,
                devInfo,
                DEVPKEY_Device_UINumber,
                DEVPROP_TYPE_INT32,
                IntPtr.Zero,
                0UI,
                cbSize,
                0)

                If cbSize > 0 Then

                    mm.Length = cbSize
                    mm.ZeroMemory()

                    SetupDiGetDeviceProperty(hDev,
                    devInfo,
                    DEVPKEY_Device_UINumber,
                    DEVPROP_TYPE_INT32,
                    mm,
                    cbSize,
                    cbSize,
                    0)

                    If cbSize > 0 Then devOut.UINumber = mm.IntAt(0)
                End If

                '' FriendlyName
                sb.AppendLine("Property FriendlyName")
                cbSize = 0

                SetupDiGetDeviceProperty(hDev,
                devInfo,
                DEVPKEY_Device_FriendlyName,
                DEVPROP_TYPE_STRING,
                IntPtr.Zero,
                0UI,
                cbSize,
                0)

                If cbSize > 0 Then
                    mm.Length = cbSize
                    mm.ZeroMemory()

                    SetupDiGetDeviceProperty(hDev,
                    devInfo,
                    DEVPKEY_Device_FriendlyName,
                    DEVPROP_TYPE_STRING,
                    mm,
                    cbSize,
                    cbSize,
                    0)

                    If cbSize > 0 Then devOut.FriendlyName = mm.ToString
                End If
                '' InstanceId

                sb.AppendLine("Property InstanceId")
                cbSize = 0
                SetupDiGetDeviceProperty(hDev,
                devInfo,
                DEVPKEY_Device_InstanceId,
                DEVPROP_TYPE_STRING,
                IntPtr.Zero,
                0UI,
                cbSize,
                0)

                If cbSize > 0 Then
                    mm.Length = cbSize
                    mm.ZeroMemory()

                    SetupDiGetDeviceProperty(hDev,
                    devInfo,
                    DEVPKEY_Device_InstanceId,
                    DEVPROP_TYPE_STRING,
                    mm,
                    cbSize,
                    cbSize,
                    0)

                    If cbSize > 0 Then devOut.InstanceId = mm.ToString
                End If

                '' Get the device icon
                _internalGetDeviceIcon(hDev, devInfo, devOut)

                File.AppendAllText(dumpFile, sb.ToString)

                Return devOut

            Catch ex As Exception

                sb.AppendLine(ex.Message)
                sb.AppendLine(FormatLastError(CUInt(Marshal.GetLastWin32Error)))

                MsgBox(ex.Message & " : See " & dumpFile & " for more clues." & vbCrLf & FormatLastError(CUInt(Marshal.GetLastWin32Error)))
                File.AppendAllText(dumpFile, sb.ToString)

                Return devOut
            End Try

        End Function

        ''' <summary>
        ''' Digests a device property type and raw data and returns the equivalent CLR object.
        ''' </summary>
        ''' <param name="type">The property type.</param>
        ''' <param name="data">The raw data pointer.</param>
        ''' <param name="length">The length of the data.</param>
        ''' <returns>A managed-memory object equivalent.</returns>
        ''' <remarks></remarks>
        Public Function _internalDevPropToObject(type As DevPropTypes, data As IntPtr, Optional length As Integer = 0) As Object

            Dim mm As MemPtr = data

            Select Case type

                Case DevPropTypes.Binary
                    Return mm.ToByteArray(0, length)

                Case DevPropTypes.Boolean
                    Return CBool(mm.ByteAt(0))

                Case DevPropTypes.Byte
                    Return mm.ByteAt(0)

                Case DevPropTypes.SByte
                    Return mm.SByteAt(0)

                Case DevPropTypes.Int16
                    Return mm.ShortAt(0)

                Case DevPropTypes.UInt16
                    Return mm.UShortAt(0)

                Case DevPropTypes.Int32
                    Return mm.IntAt(0)

                Case DevPropTypes.UInt32
                    Return mm.UIntAt(0)

                Case DevPropTypes.Int64
                    Return mm.LongAt(0)

                Case DevPropTypes.UInt64
                    Return mm.ULongAt(0)

                Case DevPropTypes.Currency

                    '' I had to read the documentation on MSDN very carefully to understand why this needs to be.
                    Return (mm.DoubleAt(0) * 10000.0#)

                Case DevPropTypes.Float
                    Return mm.SingleAt(0)

                Case DevPropTypes.Date

                    '' based on what the MSDN describes of this property format, this is what
                    '' I believe needs to be done to make the value into an acceptable CLR DateTime object.
                    Dim d As Double = mm.DoubleAt(0)

                    Dim t As New TimeSpan(CInt(d * 24), 0, 0)
                    Dim dt As Date = #12/31/1899#

                    dt.Add(t)
                    Return dt

                Case DevPropTypes.Decimal
                    Return mm.DecimalAt(0)

                Case DevPropTypes.FileTime

                    Dim ft As Native.FILETIME = mm.ToStruct(Of Native.FILETIME)()
                    Return ft.ToDateTime

                Case DevPropTypes.DevPropKey
                    Dim dk As DEVPROPKEY
                    dk = mm.ToStruct(Of DEVPROPKEY)()
                    Return dk

                Case DevPropTypes.Guid
                    Return mm.GuidAt(0)

                Case DevPropTypes.SecurityDescriptor
                    Dim sd As SECURITY_DESCRIPTOR = mm.ToStruct(Of SECURITY_DESCRIPTOR)()
                    Return sd

                Case DevPropTypes.String
                    Return mm.ToString

                Case DevPropTypes.StringList
                    Return mm.GetStringArray(0)

                Case DevPropTypes.DevPropType
                    Return mm.IntAt(0)

                Case DevPropTypes.SecurityDescriptorString
                    Return mm.ToString

                Case DevPropTypes.StringIndirect
                    '' load the string resource, itself, from the file.
                    Return LoadStringResource(mm.ToString)

                Case DevPropTypes.NTStatus
                    Return mm.IntAt(0)

            End Select

            Return Nothing

        End Function

    End Module

End Namespace