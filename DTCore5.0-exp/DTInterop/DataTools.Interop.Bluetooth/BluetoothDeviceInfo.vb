'' ************************************************* ''
'' DataTools Visual Basic Utility Library - Interop
''
'' Module: BlueTooth information (TO DO)
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
Imports System.Runtime.InteropServices


'' TO DO!  
#Region "Bluetooth Device Info"

Public Class BluetoothDeviceInfo
    Inherits DeviceInfo

    Protected _bthaddr As BLUETOOTH_ADDRESS

    Protected _service As String
    Protected _major As String
    Protected _minor As String

    Protected _btClass As UInteger

    Protected _isradio As Boolean

    Public Overridable Property IsRadio As Boolean
        Get
            Return _isradio
        End Get
        Friend Set(value As Boolean)
            _isradio = value
        End Set
    End Property

    Public Overridable Property BluetoothDeviceClass As UInteger
        Get
            Return _btClass
        End Get
        Friend Set(value As UInteger)
            _btClass = value

            Dim svc As UShort,
                maj As UShort,
                min As UShort

            ParseClass(_btClass, svc, maj, min)

            _service = PrintServiceClass(svc)
            _major = PrintMajorClass(maj)
            _minor = PrintMinorClass(maj, min)

        End Set
    End Property


    ''' <summary>
    ''' Return the Bluetooth MAC address for this radio
    ''' </summary>
    ''' <returns></returns>
    Public Overridable Property BluetoothAddress As BLUETOOTH_ADDRESS
        Get
            Return _bthaddr
        End Get
        Friend Set(value As BLUETOOTH_ADDRESS)
            _bthaddr = value
        End Set
    End Property


    Public Overridable Property BluetoothServiceClasses As String
        Get
            Return _service
        End Get
        Protected Set(value As String)
            _service = value
        End Set
    End Property


    Public Overridable Property BluetoothMajorClasses As String
        Get
            Return _major
        End Get
        Protected Set(value As String)
            _major = value
        End Set
    End Property


    Public Overridable Property BluetoothMinorClasses As String
        Get
            Return _minor
        End Get
        Protected Set(value As String)
            _minor = value
        End Set
    End Property

    Public Overrides ReadOnly Property UIDescription As String
        Get
            If String.IsNullOrEmpty(FriendlyName) Then Return MyBase.UIDescription Else Return FriendlyName
        End Get
    End Property


    Public Shared Sub ShowBluetoothSettings()

        Dim shex As New SHELLEXECUTEINFO
        shex.cbSize = Marshal.SizeOf(shex)
        shex.nShow = SW_SHOW
        shex.hInstApp = Process.GetCurrentProcess.Handle
        'shex.hWnd = 
        'shex.lpVerb = "properties"

        '' Set the parsing name exactly this way.
        shex.lpFile = "ms-settings:bluetooth"

        shex.fMask = SEE_MASK_ASYNCOK Or SEE_MASK_FLAG_DDEWAIT Or SEE_MASK_UNICODE
        ShellExecuteEx(shex)

        'Shell("ms-settings:bluetooth")
    End Sub


    Public Overrides Function ToString() As String
        If String.IsNullOrEmpty(FriendlyName) Then Return MyBase.ToString Else Return FriendlyName
    End Function

End Class

#End Region
