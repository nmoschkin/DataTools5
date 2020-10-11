'' ************************************************* ''
'' DataTools Visual Basic Utility Library - Interop
''
'' Module: HidDeviceInfo
''         USB HID Device derived class.
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
Imports DataTools.Interop.Desktop
Imports CoreCT.Text


Namespace Usb

#Region "HidDeviceInfo"

    ''' <summary>
    ''' An object that represents a Human Interface Device USB device on the system.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class HidDeviceInfo
        Inherits DeviceInfo

        Protected _HidPage As HidUsagePage

        Protected _SerialNumber As String
        Protected _ProductString As String
        Protected _PhysicalDescriptor As String
        Protected _HidManufacturer As String

#Region "Hid Feature Codes"

        ''' <summary>
        ''' Returns the raw byte data for a Hid feature code.
        ''' </summary>
        ''' <param name="featureCode">The Hid feature code to retrieve.</param>
        ''' <param name="result">Receives the result of the operation.</param>
        ''' <param name="expectedSize">The expected size, in bytes, of the result.</param>
        ''' <returns>True if successful.</returns>
        ''' <remarks></remarks>
        Public Function HidGetFeature(featureCode As Byte, ByRef result As Byte(), expectedSize As Integer) As Boolean

            Dim hfile As IntPtr = OpenHid(Me)
            If hfile = IntPtr.Zero Then Return False

            Dim mm As New MemPtr
            mm.Alloc(expectedSize + 1)

            mm.ByteAt(0) = featureCode

            If Not HidD_GetFeature(hfile, mm, expectedSize) Then
                HidGetFeature = False
            Else
                HidGetFeature = True
                result = mm.ToByteArray(1, expectedSize)
            End If

            CloseHid(hfile)
            mm.Free()

        End Function

        ''' <summary>
        ''' Returns the short value of a Hid feature code.
        ''' </summary>
        ''' <param name="featureCode">The Hid feature code to retrieve.</param>
        ''' <param name="result">Receives the result of the operation.</param>
        ''' <returns>True if successful.</returns>
        ''' <remarks></remarks>
        Public Function HidGetFeature(featureCode As Byte, ByRef result As Short) As Boolean

            Dim hfile As IntPtr = OpenHid(Me)
            If hfile = IntPtr.Zero Then Return False

            Dim mm As New MemPtr
            mm.Alloc(3)
            mm.ByteAt(0) = featureCode

            If Not HidD_GetFeature(hfile, mm, 3) Then
                HidGetFeature = False
            Else
                HidGetFeature = True
                result = mm.ShortAtAbsolute(1)
            End If

            mm.Free()
            CloseHid(hfile)

        End Function

        ''' <summary>
        ''' Returns the integer value of a Hid feature code.
        ''' </summary>
        ''' <param name="featureCode">The Hid feature code to retrieve.</param>
        ''' <param name="result">Receives the result of the operation.</param>
        ''' <returns>True if successful.</returns>
        ''' <remarks></remarks>
        Public Function HidGetFeature(featureCode As Byte, ByRef result As Integer) As Boolean

            Dim hfile As IntPtr = OpenHid(Me)
            If hfile = IntPtr.Zero Then Return False

            Dim mm As New MemPtr
            mm.Alloc(5)
            mm.ByteAt(0) = featureCode

            If Not HidD_GetFeature(hfile, mm, 5) Then
                HidGetFeature = False
            Else
                HidGetFeature = True
                result = mm.IntAtAbsolute(1)
            End If

            mm.Free()
            CloseHid(hfile)

        End Function

        ''' <summary>
        ''' Returns the long value of a Hid feature code.
        ''' </summary>
        ''' <param name="featureCode">The Hid feature code to retrieve.</param>
        ''' <param name="result">Receives the result of the operation.</param>
        ''' <returns>True if successful.</returns>
        ''' <remarks></remarks>
        Public Function HidGetFeature(featureCode As Byte, ByRef result As Long) As Boolean

            Dim hfile As IntPtr = OpenHid(Me)
            If hfile = IntPtr.Zero Then Return False

            Dim mm As New MemPtr
            mm.Alloc(9)
            mm.ByteAt(0) = featureCode

            If Not HidD_GetFeature(hfile, mm, 9) Then
                HidGetFeature = False
            Else
                HidGetFeature = True
                result = mm.LongAtAbsolute(1)
            End If

            mm.Free()
            CloseHid(hfile)

        End Function

        ''' <summary>
        ''' Sets the raw byte value of a Hid feature code.
        ''' </summary>
        ''' <param name="featureCode">The Hid feature code to set.</param>
        ''' <param name="value">The value to set.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function HidSetFeature(featureCode As Byte, value As Byte()) As Boolean

            Dim hfile As IntPtr = OpenHid(Me)
            If hfile = IntPtr.Zero Then Return False

            Dim mm As New MemPtr
            mm.Alloc(value.Length + 1)
            mm.FromByteArray(value, 1)
            mm.ByteAt(0) = featureCode

            If Not HidD_SetFeature(hfile, mm, CInt(mm.Length)) Then
                HidSetFeature = False
            Else
                HidSetFeature = True
            End If

            mm.Free()
            CloseHid(hfile)

        End Function

        ''' <summary>
        ''' Sets the short value of a Hid feature code.
        ''' </summary>
        ''' <param name="featureCode">The Hid feature code to set.</param>
        ''' <param name="value">The value to set.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function HidSetFeature(featureCode As Byte, value As Short) As Boolean

            Dim hfile As IntPtr = OpenHid(Me)
            If hfile = IntPtr.Zero Then Return False

            Dim mm As New MemPtr

            mm.Alloc(3)
            mm.ByteAt(0) = featureCode
            mm.ShortAtAbsolute(1) = value

            If Not HidD_SetFeature(hfile, mm, 3) Then
                HidSetFeature = False
            Else
                HidSetFeature = True
            End If

            CloseHid(hfile)
            mm.Free()

        End Function

        ''' <summary>
        ''' Sets the integer value of a Hid feature code.
        ''' </summary>
        ''' <param name="featureCode">The Hid feature code to set.</param>
        ''' <param name="value">The value to set.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function HidSetFeature(featureCode As Byte, value As Integer) As Boolean

            Dim hfile As IntPtr = OpenHid(Me)
            If hfile = IntPtr.Zero Then Return False

            Dim mm As New MemPtr

            mm.Alloc(5)
            mm.ByteAt(0) = featureCode
            mm.IntAtAbsolute(1) = value

            If Not HidD_SetFeature(hfile, mm, 5) Then
                HidSetFeature = False
            Else
                HidSetFeature = True

            End If

            CloseHid(hfile)
            mm.Free()

        End Function

        ''' <summary>
        ''' Sets the long value of a Hid feature code.
        ''' </summary>
        ''' <param name="featureCode">The Hid feature code to set.</param>
        ''' <param name="value">The value to set.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function HidSetFeature(featureCode As Byte, value As Long) As Boolean

            Dim hfile As IntPtr = OpenHid(Me)
            If hfile = IntPtr.Zero Then Return False

            Dim mm As New MemPtr

            mm.Alloc(9)
            mm.ByteAt(0) = featureCode
            mm.LongAtAbsolute(1) = value

            If Not HidD_SetFeature(hfile, mm, 9) Then
                HidSetFeature = False
            Else
                HidSetFeature = True

            End If

            CloseHid(hfile)
            mm.Free()

        End Function

#End Region '' Hid Feature Codes

        ''' <summary>
        ''' Returns the HID device manufacturer.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property HidManufacturer As String
            Get
                If _HidManufacturer Is Nothing Then
                    Dim dev As IntPtr = OpenHid(Me)
                    Dim mm As MemPtr
                    mm.AllocZero(512)
                    HidD_GetManufacturerString(dev, mm, CInt(mm.Length))
                    _HidManufacturer = mm.GetString(0)
                    mm.Free()
                    CloseHid(dev)
                End If
                Return _HidManufacturer
            End Get
            Friend Set(value As String)
                _HidManufacturer = value
            End Set
        End Property

        ''' <summary>
        ''' Returns the HID device serial number.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SerialNumber As String
            Get
                If _SerialNumber Is Nothing Then
                    Dim dev As IntPtr = OpenHid(Me)
                    Dim mm As MemPtr
                    mm.AllocZero(512)
                    HidD_GetSerialNumberString(dev, mm, CInt(mm.Length))
                    _SerialNumber = CStr(mm)
                    mm.Free()
                    CloseHid(dev)
                End If
                Return _SerialNumber
            End Get
            Friend Set(value As String)
                _SerialNumber = value
            End Set
        End Property

        ''' <summary>
        ''' Returns the HID device product descriptor.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ProductString As String
            Get
                If _ProductString Is Nothing Then
                    Dim dev As IntPtr = OpenHid(Me)
                    Dim mm As MemPtr
                    mm.AllocZero(512)
                    HidD_GetProductString(dev, mm, CInt(mm.Length))
                    _ProductString = CStr(mm)
                    mm.Free()
                    CloseHid(dev)
                End If
                Return _ProductString
            End Get
            Friend Set(value As String)
                _ProductString = value
            End Set
        End Property

        ''' <summary>
        ''' Returns the ProductString if available, or the FriendlyName, otherwise.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Property FriendlyName As String
            Get
                If String.IsNullOrEmpty(ProductString) = False Then Return ProductString Else Return MyBase.FriendlyName
            End Get
            Friend Set(value As String)
                MyBase.FriendlyName = value
            End Set
        End Property

        ''' <summary>
        ''' Returns the HID device physical descriptor.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PhysicalDescriptor As String
            Get
                If _PhysicalDescriptor Is Nothing Then
                    Dim dev As IntPtr = OpenHid(Me)
                    Dim mm As MemPtr
                    mm.AllocZero(512)
                    HidD_GetPhysicalDescriptor(dev, mm, CInt(mm.Length))
                    _PhysicalDescriptor = CStr(mm)
                    mm.Free()
                    CloseHid(dev)
                End If
                Return _PhysicalDescriptor
            End Get
            Friend Set(value As String)
                _PhysicalDescriptor = value
            End Set
        End Property

        ''' <summary>
        ''' Returns the HID usage description.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property HidUsageDescription As String
            Get
                Return Utility.GetEnumDescription(_HidPage)
            End Get
        End Property

        ''' <summary>
        ''' Returns the HID usage page type.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property HidUsagePage As HidUsagePage
            Get
                Return _HidPage
            End Get
        End Property

        Protected Overrides Sub ParseHw()
            MyBase.ParseHw()

            Dim v() As String

            '' this is how we determine the HID class of the device. I've found this to be a very reliable method.
            For Each hw As String In _HardwareIds
                Dim i As Integer = hw.IndexOf("HID_DEVICE_UP:")

                If i >= 0 Then
                    v = TextTools.Split(hw.Substring(i), ":")
                    If v.Length > 1 Then

                        Dim hp As UShort

                        If UShort.TryParse(v(1).Replace("_U", ""), Globalization.NumberStyles.AllowHexSpecifier, Globalization.CultureInfo.CurrentCulture.NumberFormat, hp) Then
                            _HidPage = CType(hp, HidUsagePage)

                            If _HidPage > &HFF Then
                                _HidPage = HidUsagePage.Reserved

                                If v.Length > 2 Then
                                    If UShort.TryParse(v(1).Replace("_U", ""), Globalization.NumberStyles.AllowHexSpecifier, Globalization.CultureInfo.CurrentCulture.NumberFormat, hp) Then
                                        _HidPage = CType(hp, HidUsagePage)
                                        If _HidPage > &HFF Then _HidPage = HidUsagePage.Reserved
                                    End If
                                End If

                                Return
                            Else

                                Return
                            End If

                        End If
                    End If
                End If
            Next
        End Sub

        Public Overrides ReadOnly Property UIDescription As String
            Get
                If String.IsNullOrEmpty(ProductString) = False Then
                    Return ProductString
                ElseIf String.IsNullOrEmpty(Description) = False Then
                    Return Description
                ElseIf String.IsNullOrEmpty(FriendlyName) = False Then
                    Return FriendlyName
                Else
                    Return ToString()
                End If
            End Get
        End Property

        Public Overrides Function ToString() As String
            Return Description & " (" & HidUsageDescription & ")"
        End Function

    End Class

#End Region

End Namespace
