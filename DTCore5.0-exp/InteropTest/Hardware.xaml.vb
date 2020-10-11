Imports DataTools.Interop
Imports System.Collections.ObjectModel
Imports System.Windows
Imports System.Windows.Input

Public Class Hardware

    Public ReadOnly Property Devices As ObservableCollection(Of Object)
        Get
            Return GetValue(Hardware.DevicesProperty)
        End Get
    End Property

    Private Shared ReadOnly DevicesPropertyKey As DependencyPropertyKey = _
                            DependencyProperty.RegisterReadOnly("Devices", _
                            GetType(ObservableCollection(Of Object)), GetType(Hardware), _
                            New PropertyMetadata(Nothing))

    Public Shared ReadOnly DevicesProperty As DependencyProperty = _
                           DevicesPropertyKey.DependencyProperty

    Public Sub New()

        InitializeComponent()

        'Dim drv() As DiskDeviceInfo = EnumDisks()

        'For Each d In drv
        '    If d.VolumePaths IsNot Nothing AndAlso d.VolumePaths(0) = "J:\" Then

        '        Dim c2 As Long = d.SizeFree

        '        Dim c1 As Long = d.Size
        '        Dim c3 As Long = d.SizeUsed

        '        Dim s As String = d.Size


        '    End If
        'Next
        'drv(0).ToString()


    End Sub

    Private Sub _Quit_Click(sender As Object, e As RoutedEventArgs) Handles _Quit.Click
        End
    End Sub

    Private Sub _Close_Click(sender As Object, e As RoutedEventArgs) Handles _Close.Click
        Me.Close()
    End Sub

    Private Sub ProgramList_MouseDoubleClick(sender As Object, e As MouseButtonEventArgs) Handles ProgramList.MouseDoubleClick
        If TypeOf ProgramList.SelectedItem Is DeviceInfo Then
            CType(ProgramList.SelectedItem, DeviceInfo).ShowDevicePropertiesDialog()
        End If
    End Sub



    Private Sub Hardware_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded

        Dim hc As HardwareCollection = HardwareCollection.CreateComputerHierarchy

        SetValue(DevicesPropertyKey, hc)

    End Sub
End Class
