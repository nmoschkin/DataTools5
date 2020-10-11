

'' The guts of the device enumeration system
'' Some outward-facing functions and a lot of little internal ones.

'' I tried to make it as clean as self-explanatory as possible.  

'' Copyright (C) 2014 Nathan Moschkin

Imports System.Security
Imports DataTools.Interop.Native
Imports DataTools.Interop.Disk
Imports DataTools.Interop.Printers
Imports System.Collections.ObjectModel

''' <summary>
''' Public device enum functions module.
''' </summary>
''' <remarks></remarks>
<HideModuleName, SecurityCritical>
Public Module DevEnumPublic


#If CONFIG = "Debug" Then

    Friend ReadOnly DevLog As Boolean = False
    '    Friend xfLog = My.Computer.FileSystem.OpenTextFileWriter("hardware.log", True)

    Friend Sub WriteLog(text As String)

        'Dim d As String = Date.Now().ToString("R")

        'Dim s As String = String.Format("[{0}] : {1}", d, text)
        'xfLog.WriteLine(s)

    End Sub

#Else
    Friend ReadOnly DevLog As Boolean = False
#End If


    Sub New()
        If DevLog Then

        End If
    End Sub

    ''' <summary>
    ''' Returns an exhaustive hardware tree of the entire computer with as much information as can be obtained. Each object descends from DeviceInfo.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function EnumComputerExhaustive() As ObservableCollection(Of Object)
        Return _internalEnumComputerExhaustive()
    End Function

    ''' <summary>
    ''' <para>
    ''' Enumerate all network devices installed and recognized by the system.
    ''' 
    ''' For much more detailed information about network devices, 
    ''' use the managed <see cref="Network.AdaptersCollection" /> object in this assembly.
    ''' This function is used internally by that object.
    ''' </para>
    ''' </summary>
    ''' <returns>An array of DeviceInfo() objects.</returns>
    ''' <remarks></remarks>
    Public Function EnumerateNetworkDevices() As DeviceInfo()
        Return EnumerateDevices(Of DeviceInfo)(GUID_DEVINTERFACE_NET)
    End Function

    ''' <summary>
    ''' Enumerate all physical hard drives and optical drives, and virtual drives.
    ''' </summary>
    ''' <returns>An array of DiskDeviceInfo objects.</returns>
    ''' <remarks></remarks>
    Public Function EnumDisks() As DiskDeviceInfo()
        Dim d() As DiskDeviceInfo = _internalEnumDisks()
        Dim e() As DiskDeviceInfo = _internalEnumDisks(GUID_DEVINTERFACE_CDROM)

        Dim c As Integer = d.Count

        If (e Is Nothing) OrElse (e.Count = 0) Then Return d

        Try
            For Each x In e

                Try
                    ReDim Preserve d(c)
                    d(c) = x
                    c += 1
                Catch ex As Exception
                    Erase e
                    Return d
                End Try
            Next

            Erase e
            Return d
        Catch ex As Exception

        End Try

        Return Nothing
    End Function



    ''' <summary>
    ''' Enumerate Bluetooth Radios
    ''' </summary>
    ''' <returns>Array of <see cref="BluetoothDeviceInfo"/> objects.</returns>
    Public Function EnumBluetoothRadios() As BluetoothDeviceInfo()


        Dim bth = _internalEnumBluetoothRadios()
        Dim p = _internalEnumerateDevices(Of BluetoothDeviceInfo)(BluetoothApi.GUID_BTHPORT_DEVICE_INTERFACE, ClassDevFlags.DeviceInterface Or ClassDevFlags.Present)

        If p IsNot Nothing AndAlso p.Count > 0 Then
            For Each x In p


                For Each y In bth

                    Dim i = x.InstanceId.LastIndexOf("\")
                    If (i > -1) Then
                        Dim s = x.InstanceId.Substring(i + 1)

                        Dim res As ULong
                        Dim b As Boolean = ULong.TryParse(s, Globalization.NumberStyles.AllowHexSpecifier, Globalization.CultureInfo.CurrentCulture, res)

                        If (b) Then
                            If res = CType(y.address, ULong) Then

                                x.IsRadio = True
                                x.BluetoothAddress = y.address
                                x.BluetoothDeviceClass = y.ulClassofDevice

                                Exit For

                            End If
                        End If

                    End If

                Next

            Next

        End If

        If p Is Nothing Then Return Nothing

        Array.Sort(p, New Comparison(Of DeviceInfo)(
                   Function(x As DeviceInfo, y As DeviceInfo) As Integer
                       If x.FriendlyName IsNot Nothing AndAlso y.FriendlyName IsNot Nothing Then
                           Return String.Compare(x.FriendlyName, y.FriendlyName)
                       Else
                           Return String.Compare(x.Description, y.Description)
                       End If
                   End Function))

        Return p
    End Function


    ''' <summary>
    ''' Enumerate Bluetooth devices
    ''' </summary>
    ''' <returns>Array of <see cref="BluetoothDeviceInfo"/> objects.</returns>
    Public Function EnumBluetoothDevices() As BluetoothDeviceInfo()

        Dim lOut As New List(Of BluetoothDeviceInfo)

        Dim bth = _internalEnumBluetoothDevices()
        Dim p = _internalEnumerateDevices(Of BluetoothDeviceInfo)(GUID_DEVCLASS_BLUETOOTH, ClassDevFlags.Present)

        If p IsNot Nothing AndAlso p.Count > 0 Then
            For Each x In p

                For Each y In bth

                    Dim i = x.InstanceId.LastIndexOf("_")
                    If (i > -1) Then
                        Dim s = x.InstanceId.Substring(i + 1)

                        Dim res As ULong
                        Dim b As Boolean = ULong.TryParse(s, Globalization.NumberStyles.AllowHexSpecifier, Globalization.CultureInfo.CurrentCulture, res)

                        If (b) Then
                            If res = CType(y.address, ULong) Then

                                x.IsRadio = False
                                x.BluetoothAddress = y.address
                                x.BluetoothDeviceClass = y.ulClassofDevice

                                lOut.Add(x)
                                Exit For

                            End If
                        End If

                    End If

                Next

            Next

        End If

        Return lOut.ToArray()

        Array.Sort(p, New Comparison(Of DeviceInfo)(
                   Function(x As DeviceInfo, y As DeviceInfo) As Integer
                       If x.FriendlyName IsNot Nothing AndAlso y.FriendlyName IsNot Nothing Then
                           Return String.Compare(x.FriendlyName, y.FriendlyName)
                       Else
                           Return String.Compare(x.Description, y.Description)
                       End If
                   End Function))

        Return p
    End Function

    ''' <summary>
    ''' Enumerate COM Ports
    ''' </summary>
    ''' <returns></returns>
    Public Function EnumComPorts() As DeviceInfo()
        Dim p = _internalEnumerateDevices(Of DeviceInfo)(GUID_DEVINTERFACE_COMPORT, ClassDevFlags.DeviceInterface Or ClassDevFlags.Present)

        If p IsNot Nothing AndAlso p.Count > 0 Then
            For Each x In p
                If String.IsNullOrEmpty(x.FriendlyName) Then Continue For
            Next
        End If

        If p Is Nothing Then Return Nothing

        Array.Sort(p, New Comparison(Of DeviceInfo)(
                   Function(x As DeviceInfo, y As DeviceInfo) As Integer
                       If x.FriendlyName IsNot Nothing AndAlso y.FriendlyName IsNot Nothing Then
                           Return String.Compare(x.FriendlyName, y.FriendlyName)
                       Else
                           Return String.Compare(x.Description, y.Description)
                       End If
                   End Function))

        Return p
    End Function

    ''' <summary>
    ''' Enumerate all printer queues available to the local system.
    ''' </summary>
    ''' <returns>An array of PrinterDeviceInfo objects.</returns>
    ''' <remarks></remarks>
    Public Function EnumPrinters() As PrinterDeviceInfo()

        Dim p = _internalEnumerateDevices(Of PrinterDeviceInfo)(GUID_DEVINTERFACE_PRINTER, ClassDevFlags.DeviceInterface Or ClassDevFlags.Present)

        If p IsNot Nothing AndAlso p.Count > 0 Then
            For Each x In p

                If String.IsNullOrEmpty(x.FriendlyName) Then Continue For
                Try

                    x.PrinterInfo = Printers.PrinterObject.GetPrinterInfoObject(x.FriendlyName)
                Catch ex As Exception

                    '' can't connect to that printer!
                    Continue For
                End Try
            Next
        End If

        If p Is Nothing Then Return Nothing

        Array.Sort(p, New Comparison(Of PrinterDeviceInfo)( _
                   Function(x As PrinterDeviceInfo, y As PrinterDeviceInfo) As Integer
                       If x.FriendlyName IsNot Nothing AndAlso y.FriendlyName IsNot Nothing Then
                           Return String.Compare(x.FriendlyName, y.FriendlyName)
                       Else
                           Return String.Compare(x.Description, y.Description)
                       End If
                   End Function))

        Return p
    End Function

    ''' <summary>
    ''' Enumerate all local volumes (with or without mount points).
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function EnumVolumes() As DiskDeviceInfo()
        EnumVolumes = _internalEnumDisks(GUID_DEVINTERFACE_VOLUME)
    End Function

    ''' <summary>
    ''' Enumerate devices of DeviceInfo T with the specified hardware class Id.
    ''' </summary>
    ''' <typeparam name="T">A DeviceInfo-based object.</typeparam>
    ''' <param name="ClassId">A system GUID_DEVINTERFACE or GUID_DEVCLASS value.</param>
    ''' <param name="flags">Optional flags to pass to the enumerator.</param>
    ''' <returns>An array of T</returns>
    ''' <remarks></remarks>
    Public Function EnumerateDevices(Of T As {DeviceInfo, New})(ClassId As Guid, Optional flags As ClassDevFlags = ClassDevFlags.Present Or ClassDevFlags.DeviceInterface) As T()
        EnumerateDevices = _internalEnumerateDevices(Of T)(ClassId, flags)
    End Function

    ''' <summary>
    ''' Enumerate all devices on the system.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function EnumAllDevices() As DeviceInfo()
        Return _internalGetComputer()
    End Function

End Module
