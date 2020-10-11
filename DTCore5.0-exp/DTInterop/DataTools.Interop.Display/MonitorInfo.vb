'' ************************************************* ''
'' DataTools Visual Basic Utility Library - Interop
''
'' Module: MonitorInfo
''         Display monitor information encapsulation.
'' 
'' Copyright (C) 2011-2020 Nathan Moschkin
'' All Rights Reserved
''
'' Licensed Under the Microsoft Public License   
'' ************************************************* ''

Imports System.Runtime.InteropServices
Imports System.Collections.ObjectModel
Imports DataTools.Interop.Native


Namespace Native


    Module MultiMon

        Public Const MONITORINFOF_PRIMARY = &H00000001

    End Module


    ''' <summary>
    ''' Represents the internal structure for display monitor information.
    ''' </summary>
    ''' <remarks></remarks>
    <StructLayout(LayoutKind.Sequential, CharSet:=Runtime.InteropServices.CharSet.Unicode)>
    Friend Structure MONITORINFOEX
        Public cbSize As UInteger
        Public rcMonitor As RECT
        Public rcWork As RECT
        Public dwFlags As UInteger

        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=32)>
        Public szDevice As String
    End Structure

End Namespace

Namespace Display

    ''' <summary>
    ''' Multi-monitor hit-test flags.
    ''' </summary>
    Public Enum MultiMonFlags As UInteger

        DefaultToNull = &H00000000
        DefaultToPrimary = &H00000001
        DefaultToNearest = &H00000002

    End Enum

    ''' <summary>
    ''' Represents a collection of all monitors on the system.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Monitors
        Inherits ObservableCollection(Of MonitorInfo)


        Private Delegate Function MonitorEnumProc(hMonitor As IntPtr, hdcMonitor As IntPtr, ByRef lpRect As RECT, lParam As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Private Declare Unicode Function EnumDisplayMonitors Lib "user32" (hdc As IntPtr,
                                                                        lprcClip As IntPtr,
                                                                        <MarshalAs(UnmanagedType.FunctionPtr)>
                                                                        lpfnEnum As MonitorEnumProc,
                                                                        dwData As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Private Declare Unicode Function MonitorFromPoint Lib "user32" (pt As POINT, dwFlags As MultiMonFlags) As IntPtr
        Private Declare Unicode Function MonitorFromRect Lib "user32" (pt As RECT, dwFlags As MultiMonFlags) As IntPtr
        Private Declare Unicode Function MonitorFromWindow Lib "user32" (hWnd As IntPtr, dwFlags As MultiMonFlags) As IntPtr


        Private Function _enum(hMonitor As IntPtr, hdcMonitor As IntPtr, ByRef lpRect As RECT, lParamIn As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
            Dim lParam As CoreCT.Memory.MemPtr = lParamIn

            Me.Add(New MonitorInfo(hMonitor, lParam.IntAt(0)))
            lParam.IntAt(0) += 1

            Return True
        End Function


        ''' <summary>
        ''' Retrieves the monitor at the given point.
        ''' </summary>
        ''' <param name="pt"></param>
        ''' <returns></returns>
        Public Function GetMonitorFromPoint(pt As POINT, Optional flags As MultiMonFlags = MultiMonFlags.DefaultToNull) As MonitorInfo
            If Me.Count = 0 Then Me.Refresh()

            Dim h As IntPtr = MonitorFromPoint(pt, flags)
            If h = IntPtr.Zero Then Return Nothing

            For Each m In Me
                If m.hMonitor = h Then Return m
            Next

            Return Me(0)
        End Function

        ''' <summary>
        ''' Retrieves the monitor at the given point.
        ''' </summary>
        ''' <param name="rc"></param>
        ''' <returns></returns>
        Public Function GetMonitorFromRect(rc As RECT, Optional flags As MultiMonFlags = MultiMonFlags.DefaultToNull) As MonitorInfo
            If Me.Count = 0 Then Me.Refresh()

            Dim h As IntPtr = MonitorFromRect(rc, flags)
            If h = IntPtr.Zero Then Return Nothing

            For Each m In Me
                If m.hMonitor = h Then Return m
            Next

            Return Me(0)
        End Function

        ''' <summary>
        ''' Retrieves the monitor at the specified native window handle.
        ''' </summary>
        ''' <param name="hwnd"></param>
        ''' <param name="flags"></param>
        ''' <returns></returns>
        Public Function GetMonitorFromWindow(hwnd As IntPtr, Optional flags As MultiMonFlags = MultiMonFlags.DefaultToNull) As MonitorInfo
            If Me.Count = 0 Then Me.Refresh()

            Dim h As IntPtr = MonitorFromWindow(hwnd, flags)
            If h = IntPtr.Zero Then Return Nothing

            For Each m In Me
                If m.hMonitor = h Then Return m
            Next

            Return Me(0)
        End Function

        ''' <summary>
        ''' Retrieves the monitor at the specified WPF window.
        ''' </summary>
        ''' <param name="window"></param>
        ''' <param name="flags"></param>
        ''' <returns></returns>
        Public Function GetMonitorFromWindow(window As System.Windows.Window, Optional flags As MultiMonFlags = MultiMonFlags.DefaultToNull) As MonitorInfo
            If Me.Count = 0 Then Me.Refresh()

            Dim ih As New System.Windows.Interop.WindowInteropHelper(window)

            Dim h As IntPtr = MonitorFromWindow(ih.EnsureHandle, flags)
            ih = Nothing

            If h = IntPtr.Zero Then Return Nothing

            For Each m In Me
                If m.hMonitor = h Then Return m
            Next

            Return Me(0)
        End Function

        ''' <summary>
        ''' Refresh the current monitor list.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Refresh() As Boolean

            Me.Clear()

            Dim mm As New CoreCT.Memory.MemPtr(IntPtr.Size)
            mm.IntAt(0) = 1

            Dim i As Integer = mm.IntAt(0)
            Refresh = EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, AddressOf _enum, mm.Handle)

            mm.Free()

        End Function

        Public Sub New()
            Refresh()
        End Sub

    End Class

    ''' <summary>
    ''' Represents a monitor device.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class MonitorInfo

        Private _hMonitor As IntPtr
        Private _data As MONITORINFOEX
        Private _idx As Integer

        Private Declare Unicode Function GetMonitorInfo Lib "user32" Alias "GetMonitorInfoW" (hMonitor As IntPtr, ByRef info As MONITORINFOEX) As Boolean


        ''' <summary>
        ''' Returns the monitor index, or the order in which this monitor was reported to the monitor collection.
        ''' </summary>
        ''' <returns></returns>
        Public Property Index As Integer
            Get
                Return _idx
            End Get
            Friend Set(value As Integer)
                _idx = value
            End Set
        End Property

        ''' <summary>
        ''' Specifies the current monitor's device path.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property DevicePath As String
            Get
                Return _data.szDevice
            End Get
        End Property

        ''' <summary>
        ''' Gets the total desktop screen area and coordinates for this monitor.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property MonitorBounds As RECT
            Get

                With _data.rcMonitor
                    Return New RECT(.left, .top, (.right - .left), (.bottom - .top))
                End With
            End Get
        End Property

        ''' <summary>
        ''' Gets the available desktop area and screen coordinates for this monitor.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property WorkBounds As RECT
            Get

                With _data.rcWork
                    Return New RECT(.left, .top, (.right - .left), (.bottom - .top))
                End With
            End Get
        End Property

        ''' <summary>
        ''' True if this monitor is the primary monitor.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property IsPrimary As Boolean
            Get
                Return (_data.dwFlags And 1) = 1
            End Get
        End Property

        ''' <summary>
        ''' Gets the hMonitor handle for this device.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Friend ReadOnly Property hMonitor As IntPtr
            Get
                Return _hMonitor
            End Get
        End Property

        ''' <summary>
        ''' Refresh the monitor device information.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Refresh() As Boolean
            Return GetMonitorInfo(_hMonitor, _data)
        End Function

        ''' <summary>
        ''' Create a new instance of a monitor object from the given hMonitor.
        ''' </summary>
        ''' <param name="hMonitor"></param>
        ''' <remarks></remarks>
        Friend Sub New(hMonitor As IntPtr, idx As Integer)
            _hMonitor = hMonitor
            _data.cbSize = CUInt(Marshal.SizeOf(_data))
            _idx = idx
            Refresh()
        End Sub

    End Class

End Namespace