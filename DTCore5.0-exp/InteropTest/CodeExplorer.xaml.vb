Option Strict Off
Option Explicit On


Imports DataTools.Interop
Imports System.Collections.ObjectModel
Imports CoreCT.Memory
Imports System.Runtime.InteropServices
Imports System.Windows
Imports DataTools.Interop.Usb
Imports System.Windows.Controls
Imports System.Threading

Public Class CodeExplorer
    Private Declare Function HidD_GetFeature Lib "hid.dll" ( _
                                                          HidDeviceObject As IntPtr, _
                                                          Buffer As IntPtr, _
                                                          BufferLength As Integer) As <MarshalAs(UnmanagedType.Bool)> Boolean

    Public Property Devices As ObservableCollection(Of DeviceInfo)
        Get
            Return GetValue(DevicesProperty)
        End Get

        Set(value As ObservableCollection(Of DeviceInfo))
            SetValue(DevicesProperty, value)
        End Set
    End Property

    Public Shared ReadOnly DevicesProperty As DependencyProperty = _
                           DependencyProperty.Register("Devices", _
                           GetType(ObservableCollection(Of DeviceInfo)), GetType(CodeExplorer), _
                           New PropertyMetadata(Nothing))

    Public Sub New(devices As ObservableCollection(Of DeviceInfo))

        InitializeComponent()

        Me.Devices = devices

        internalInit()

    End Sub

    Public Sub New()

        InitializeComponent()

        Me.Devices = New ObservableCollection(Of DeviceInfo)

        internalInit()

    End Sub

    Private Sub internalInit()

        Dim devs() As HidDeviceInfo = HidFeatures.HidDevicesByUsage(HidUsagePage.PowerDevice1)

        If devs Is Nothing Then Return

        For Each d In devs
            Me.Devices.Add(d)
        Next

    End Sub

    Private Sub _Quit_Click(sender As Object, e As RoutedEventArgs) Handles _Quit.Click
        End
    End Sub

    Private Sub _Close_Click(sender As Object, e As RoutedEventArgs) Handles _Close.Click
        Me.Close()
    End Sub

    Private Sub CodeExplorer_Closing(sender As Object, e As ComponentModel.CancelEventArgs) Handles Me.Closing
        If _devThread IsNot Nothing Then
            StopWatching()
        End If
    End Sub

    Private Sub CodeExplorer_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded

    End Sub

    Private _devThread As System.Threading.Thread
    Private _lastDevice As HidDeviceInfo

    Private Sub DeviceSelect_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles DeviceSelect.SelectionChanged
        Dim d As HidDeviceInfo = DeviceSelect.SelectedItem
        StartWatching(d)
    End Sub

    Private cts As CancellationTokenSource

    Private Sub StartWatching(d As HidDeviceInfo)

        Dim h As IntPtr
        Dim x As Integer = 0
        Dim i As Integer = 0

        If _devThread IsNot Nothing Then StopWatching()

        Dim s As New ObservableCollection(Of String)

        _lastDevice = d
        ViewingArea.ItemsSource = s

        For i = 0 To 255
            s.Add("")
        Next



        Dim th As New System.Threading.Thread( _
            Sub()

                cts = New CancellationTokenSource()

                h = HidFeatures.OpenHid(d)
                If CLng(h) <= 0 Then Return

                Dim mm As New MemPtr(65)
                Try

                    Do

                        Dispatcher.Invoke( _
                            Sub()
                                For i = 0 To 255

                                    mm.LongAtAbsolute(1) = 0
                                    mm.ByteAt(0) = CByte(i)

                                    If HidD_GetFeature(h, mm, 65) Then
                                        s.Item(i) = "HID CODE " & Hex(i) & " = " & mm.IntAtAbsolute(1)
                                    End If

                                    x = 0
                                Next

                            End Sub)

                        System.Threading.Thread.Sleep(1000)

                        If (cts Is Nothing OrElse cts.IsCancellationRequested) Then Exit Do
                    Loop

                    mm.Free()
                    HidFeatures.CloseHid(h)
                    cts = Nothing
                    Return

                Catch tx As System.Threading.ThreadAbortException

                    mm.Free()
                    HidFeatures.CloseHid(h)
                    cts = Nothing
                    Return

                Catch ex As Exception

                    mm.Free()
                    HidFeatures.CloseHid(h)
                    cts = Nothing
                    Return

                End Try

            End Sub)


        th.IsBackground = True
        th.SetApartmentState(System.Threading.ApartmentState.STA)

        _devThread = th

        th.Start()

    End Sub

    Private Sub StopWatching()

        If _devThread IsNot Nothing AndAlso cts IsNot Nothing Then
            cts.Cancel()

            ViewingArea.ItemsSource = Nothing
            _devThread = Nothing
        End If

    End Sub

    Private Sub _Stop_Click(sender As Object, e As RoutedEventArgs) Handles _Stop.Click

        If CType(sender, MenuItem).Header = "_Stop Watching Device" Then
            StopWatching()
            CType(sender, MenuItem).Header = "_Start Watching Device"
        Else
            If _lastDevice Is Nothing Then Return

            StartWatching(_lastDevice)
            CType(sender, MenuItem).Header = "_Stop Watching Device"
        End If

    End Sub

End Class
