'' Example program.  Copyright (C) 2014 Nathan Moschkin

Imports System.Runtime.InteropServices
Imports System.Reflection
Imports System.ComponentModel
Imports System.Windows.Interop
Imports System.Collections.ObjectModel
Imports System.IO
Imports DataTools.Strings
Imports DataTools.Memory
Imports DataTools.Interop
Imports DataTools.Interop.Disk
Imports System.Windows

'' VB example demonstates FSMonitor object.

'' right now we've configured it for adding.  
'' you can comment out the If block, in the event, below, 
'' or change the condition to see other results.


Class FSMonTestWindow

    Private WithEvents _FSMon As FSMonitor

    Private NotifyCol As New ObservableCollection(Of FileNotifyInfo)

    Public Property MonitoredFolder As String = Nothing

    Friend Sub CreateMonitor()

        If _FSMon IsNot Nothing Then
            If _FSMon.IsWatching Then
                If MsgBox("You are currently watching a folder.  Do you want to stop and select another?", vbYesNo) = MsgBoxResult.No Then
                    Return
                Else
                    _FSMon.StopWatching()
                End If
            End If
        End If

        _FSMon = Nothing

        Dim ip As New WindowInteropHelper(Me)
        Dim fb As New System.Windows.Forms.FolderBrowserDialog

        If MonitoredFolder Is Nothing Then
            fb.SelectedPath = CurDir()
        Else
            fb.SelectedPath = MonitoredFolder
        End If

        If fb.ShowDialog = Forms.DialogResult.OK Then

            MonitoredFolder = fb.SelectedPath
            _FSMon = New FSMonitor(MonitoredFolder, ip.EnsureHandle())

        End If

    End Sub

    Public Sub New()
        InitializeComponent()
        ViewingArea.ItemsSource = NotifyCol

    End Sub

    Private Sub _FSMon_MonitorClosed(sender As Object, e As MonitorClosedEventArgs) Handles _FSMon.MonitorClosed

        Select Case e.ClosedState
            Case MonitorClosedState.Closed
                Status.Text = "Monitor for '" & MonitoredFolder & "' Closed"

            Case MonitorClosedState.ClosedOnError
                Status.Text = "Monitor for '" & MonitoredFolder & "'  Closed: " & e.ErrorMessage

        End Select
    End Sub

    Private Sub _FSMon_MonitorOpened(sender As Object, e As EventArgs) Handles _FSMon.MonitorOpened
        Status.Text = "Monitor for '" & MonitoredFolder & "' Opened"
    End Sub

    Private Sub _FSMon_WatchNotifyChange(sender As Object, e As FSMonitorEventArgs) Handles _FSMon.WatchNotifyChange

        Dim inf As FileNotifyInfo = e.Info

        Do

            '' right now we've configured it for adding.  
            '' you can comment out the If block, or change the 
            '' condition to see other results.
            If inf.Action = FileActions.Added Then
                NotifyCol.Add(inf)
            End If

            inf = inf.NextEntry
        Loop Until inf Is Nothing

        Status.Text = ViewingArea.Items.Count & " total items."

    End Sub

    Private Sub IPWindow_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        If _FSMon Is Nothing Then Return
        _FSMon.StopWatching()
    End Sub

    Private Sub StartWatching_Click(sender As Object, e As RoutedEventArgs) Handles StartWatching.Click

        CreateMonitor()
        If _FSMon IsNot Nothing Then _FSMon.Watch()
    End Sub

    Private Sub StopWatching_Click(sender As Object, e As RoutedEventArgs) Handles StopWatching.Click
        If _FSMon Is Nothing Then Return
        _FSMon.StopWatching()
    End Sub

    Private Sub Quitting_Click(sender As Object, e As RoutedEventArgs) Handles Quitting.Click
        Close()
    End Sub
End Class
