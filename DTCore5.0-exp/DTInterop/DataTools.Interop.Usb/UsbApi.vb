'' ************************************************* ''
'' DataTools Visual Basic Utility Library - Interop
''
'' Module: UsbApi
''         HID Feature Code Manipulation
''
''         Enums are documented in part from the API documentation at MSDN.
''         Other knowledge and references obtained through various sources
''         and all is considered public domain/common knowledge.
''
'' Copyright (C) 2011-2020 Nathan Moschkin
'' All Rights Reserved
''
'' Licensed Under the Microsoft Public License   
'' ************************************************* ''

Option Explicit On

Imports System.Runtime.InteropServices
Imports CoreCT.Memory
Imports System.Reflection
Imports System.ComponentModel
Imports CoreCT.Text
Imports DataTools.Interop.Native

Namespace Usb

    Public Module HidFeatures

        Public Class HIDFeatureResult
            Public Property data As MemPtr
            Public Property code As Integer

            Public ReadOnly Property singleVal As Single
                Get

                    If code.ToString.ToLower.IndexOf("Percent") <> -1 Then
                        Dim m2 As MemPtr
                        m2.Handle = data.Handle + 1

                        singleVal = m2.SingleAt(0)
                    Else
                        singleVal = CSng(intVal)
                    End If

                End Get
            End Property


            Public ReadOnly Property longVal As Long
                Get
                    Dim m2 As MemPtr
                    m2.Handle = data.Handle + 1

                    longVal = m2.LongAt(0)
                End Get
            End Property

            Public ReadOnly Property intVal As Integer
                Get
                    Dim m2 As MemPtr
                    m2.Handle = data.Handle + 1

                    intVal = m2.IntAt(0)
                End Get
            End Property

            Public ReadOnly Property bytes() As Byte()
                Get
                    Return CType(data, Byte())
                End Get
            End Property

            Protected Overrides Sub Finalize()
                data.Free()
            End Sub

            Public Sub New(i As Integer, m As MemPtr)
                code = i
                data = m.Clone
            End Sub

            Public Sub New()

            End Sub

            Public Shared Widening Operator CType(operand As Byte()) As HIDFeatureResult
                Dim q As New HIDFeatureResult
                q.data = CType(operand, MemPtr)
                Return q
            End Operator

            Public Overrides Function ToString() As String
                Return code.ToString & " (" & intVal & ")"

            End Function

        End Class

        ''' <summary>
        ''' Enumerates all HID devices in a specific HID class.
        ''' </summary>
        ''' <param name="u">The HID usage page type devices to return.</param>
        ''' <returns>An array of HidDeviceInfo objects.</returns>
        ''' <remarks></remarks>
        Public Function HidDevicesByUsage(u As HidUsagePage) As HidDeviceInfo()

            Dim devs() As HidDeviceInfo = EnumerateDevices(Of HidDeviceInfo)(GUID_DEVINTERFACE_HID)
            Dim devOut() As HidDeviceInfo = Nothing
            Dim c As Integer = 0

            For Each blurb In devs
                If blurb.HidUsagePage = u Then
                    ReDim Preserve devOut(c)
                    devOut(c) = blurb
                    c += 1
                End If
            Next

            Return devOut
        End Function

        ''' <summary>
        ''' Opens a HID device for access.
        ''' </summary>
        ''' <param name="device">The HidDeviceInfo object of the device.</param>
        ''' <returns>A handle to the open device (close with CloseHid).</returns>
        ''' <remarks></remarks>
        Public Function OpenHid(device As HidDeviceInfo) As IntPtr
            Try
                OpenHid = CreateFile(device.DevicePath,
                         GENERIC_READ,
                         FILE_SHARE_READ Or FILE_SHARE_WRITE,
                         IntPtr.Zero,
                         OPEN_EXISTING,
                         FILE_ATTRIBUTE_NORMAL,
                         Nothing)
            Catch ex As Exception
                Return IntPtr.Zero
            End Try
        End Function

        ''' <summary>
        ''' Closes a HID device handle.
        ''' </summary>
        ''' <param name="handle">The handle of the device to be freed.</param>
        ''' <remarks></remarks>
        Public Sub CloseHid(handle As IntPtr)
            If handle <> CType(-1, IntPtr) AndAlso handle <> IntPtr.Zero Then CloseHandle(handle)
        End Sub

        ''' <summary>
        ''' Retrieves a feature from the device.
        ''' </summary>
        ''' <param name="device"></param>
        ''' <param name="code"></param>
        ''' <param name="datalen"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetHIDFeature(device As HidDeviceInfo, code As Integer, Optional datalen As Integer = 16) As HIDFeatureResult

            Dim hFile As IntPtr

            hFile = CreateFile(device.DevicePath,
                     GENERIC_READ,
                     FILE_SHARE_READ Or FILE_SHARE_WRITE,
                     IntPtr.Zero,
                     OPEN_EXISTING,
                     FILE_ATTRIBUTE_NORMAL,
                     Nothing)

            If hFile = IntPtr.Zero Then Return Nothing

            GetHIDFeature = GetHIDFeature(hFile, code, datalen)

            CloseHandle(hFile)

        End Function

        ''' <summary>
        ''' Retrieves a feature from the device.
        ''' </summary>
        ''' <param name="device"></param>
        ''' <param name="code"></param>
        ''' <param name="datalen"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetHIDFeature(device As IntPtr, code As Integer, Optional datalen As Integer = 16) As HIDFeatureResult
            Dim mm As MemPtr
            Dim i As Integer = code
            Try
                mm.AllocZero(datalen)
                mm.ByteAt(0) = CByte(i)

                If HidD_GetFeature(device, mm.Handle, CInt(mm.Length)) Then
                    GetHIDFeature = New HIDFeatureResult(i, mm)
                Else
                    GetHIDFeature = Nothing
                End If
                mm.Free()
            Catch ex As Exception
                mm.Free()
                Return Nothing
            End Try
        End Function

    End Module


End Namespace