'' ************************************************* ''
'' DataTools Visual Basic Utility Library - Interop
''
'' Module: Real-time FileSystemMonitor implementation
''         With multi-threading and synchronized
''         with the app thread.
'' 
'' Copyright (C) 2011-2020 Nathan Moschkin
'' All Rights Reserved
''
'' Licensed Under the Microsoft Public License   
'' ************************************************* ''

Option Explicit On
Option Strict On

Imports System.ComponentModel
Imports CoreCT.Memory
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports DataTools.Interop.Native

Namespace Disk

    Module FileSystemMonitor

        ''' <summary>
        ''' Defines the base for all custom messages in this module.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const WM_MYBASE = WM_USER + &H100

        ''' <summary>
        ''' Signals that a change has occurred within the context of a watched folder.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const WM_SIGNAL = WM_MYBASE + 1

        ''' <summary>
        ''' Signals the message pump to attempt to clear the cache of processed events.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const WM_SIGNAL_CLEAN = WM_MYBASE + 2

        ''' <summary>
        ''' Signals that the monitor has been opened.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const WM_SIGNAL_OPEN = WM_MYBASE + 3

        ''' <summary>
        ''' Signals that the monitor has been closed.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const WM_SIGNAL_CLOSE = WM_MYBASE + 4

        ''' <summary>
        ''' The beginning of custom error codes.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const ERROR_MIN = &H20000

        ''' <summary>
        ''' Specifies that a mirroring action has failed.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const ERROR_MIRRORFAIL = ERROR_MIN + 1

        ''' <summary>
        ''' Specifies that a failure regarding a specific path has occurred.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const ERROR_PATHFAIL = ERROR_MIN + 2

        ''' <summary>
        ''' Specifies that a failure regarding a specific file has occurred.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const ERROR_FILEFAIL = ERROR_MIN + 3

        ''' <summary>
        ''' Specifies that a failure regarding a specific destination path has occurred.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const ERROR_DESTPATHFAIL = ERROR_MIN + 4

        ''' <summary>
        ''' Specifies that a failure regarding a specific destination file has occurred.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const ERROR_DESTFILEFAIL = ERROR_MIN + 5

        Public Declare Unicode Function CreateEvent Lib "kernel32" _
        Alias "CreateEventW" _
        (lpEventAttributes As IntPtr,
         bManualREset As Boolean,
         bInitialState As Boolean,
         lpName As String) As IntPtr

        Public Declare Function SetEvent Lib "kernel32" (hEvent As IntPtr) As Boolean

        Public Declare Function ResetEvent Lib "kernel32" (hEvent As IntPtr) As Boolean

        Public Declare Unicode Function FindFirstChangeNotification Lib "kernel32" _
        Alias "FindFirstChangeNotificationW" _
        (lpPathName As String,
         bWatchSubtree As Boolean,
         dwNotifyFilter As NotifyFilter) As IntPtr

        Public Delegate Sub FileIoCompletionDelegate(dwErrorCode As Integer, dwNumberOfBytesTransfered As Integer, lpOverlapped As Threading.NativeOverlapped)

        Public Declare Unicode Function ReadDirectoryChangesW Lib "kernel32" (
                                            hDirectory As IntPtr,
                                            lpBuffer As IntPtr,
                                            nBufferLength As Integer,
                                            <MarshalAs(UnmanagedType.Bool)>
                                            bWatchSubtree As Boolean,
                                            dwNotifyFilter As NotifyFilter,
                                            ByRef lpBytesReturned As Integer,
                                            lpOverlapped As IntPtr,
                                            lpCompletionRoutine As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean

    End Module

    ''' <summary>
    ''' Sets the filter criteria for directory change notifications.
    ''' </summary>
    ''' <remarks></remarks>
    <Flags>
    Public Enum NotifyFilter

        ''' <summary>
        '''  Any file name change in the watched directory or subtree causes a change notification wait operation to return. Changes include renaming, creating, or deleting a file.
        ''' </summary>
        <Description("Any file name change in the watched directory or subtree causes a change notification wait operation to return. Changes include renaming, creating, or deleting a file.")>
        NotifyFileRename = &H1

        ''' <summary>
        '''  Any directory-name change in the watched directory or subtree causes a change notification wait operation to return. Changes include creating or deleting a directory.
        ''' </summary>
        <Description("Any directory-name change in the watched directory or subtree causes a change notification wait operation to return. Changes include creating or deleting a directory.")>
        NotifyDirectoryRename = &H2

        ''' <summary>
        '''  Any attribute change in the watched directory or subtree causes a change notification wait operation to return.
        ''' </summary>
        <Description("Any attribute change in the watched directory or subtree causes a change notification wait operation to return.")>
        NotifyAttributesChange = &H4

        ''' <summary>
        '''  Any file-size change in the watched directory or subtree causes a change notification wait operation to return. The operating system detects a change in file size only when the file is written to the disk. For operating systems that use extensive caching, detection occurs only when the cache is sufficiently flushed.
        ''' </summary>
        <Description("Any file-size change in the watched directory or subtree causes a change notification wait operation to return. The operating system detects a change in file size only when the file is written to the disk. For operating systems that use extensive caching, detection occurs only when the cache is sufficiently flushed.")>
        NotifySizeChange = &H8

        ''' <summary>
        '''  Any change to the last write-time of files in the watched directory or subtree causes a change notification wait operation to return. The operating system detects a change to the last write-time only when the file is written to the disk. For operating systems that use extensive caching, detection occurs only when the cache is sufficiently flushed.
        ''' </summary>
        <Description("Any change to the last write-time of files in the watched directory or subtree causes a change notification wait operation to return. The operating system detects a change to the last write-time only when the file is written to the disk. For operating systems that use extensive caching, detection occurs only when the cache is sufficiently flushed.")>
        NotifyWrite = &H10

        ''' <summary>
        '''  Any change to the last access time of files in the watched directory or subtree causes a change notification wait operation to return.
        ''' </summary>
        <Description("Any change to the last access time of files in the watched directory or subtree causes a change notification wait operation to return.")>
        NotifyAccess = &H20

        ''' <summary>
        '''  Any change to the creation time of files in the watched directory or subtree causes a change notification wait operation to return.
        ''' </summary>
        <Description("Any change to the creation time of files in the watched directory or subtree causes a change notification wait operation to return.")>
        NotifyCreate = &H40

        ''' <summary>
        '''  Any security-descriptor change in the watched directory or subtree causes a change notification wait operation to return.
        ''' </summary>
        <Description("Any security-descriptor change in the watched directory or subtree causes a change notification wait operation to return.")>
        NotifySecurityChange = &H100

    End Enum

    ''' <summary>
    ''' Specifies an action on a file or a folder.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum FileActions

        ''' <summary>
        '''  The file was added to the directory.
        ''' </summary>
        <Description(" The file was added to the directory.")>
        Added = &H1

        ''' <summary>
        '''  The file was removed from the directory.
        ''' </summary>
        <Description(" The file was removed from the directory.")>
        Removed = &H2

        ''' <summary>
        '''  The file was modified. This can be a change in the time stamp or attributes.
        ''' </summary>
        <Description(" The file was modified. This can be a change in the time stamp or attributes.")>
        Modified = &H3

        ''' <summary>
        '''  The file was renamed and this is the old name.
        ''' </summary>
        <Description(" The file was renamed and this is the old name.")>
        RenamedOldName = &H4

        ''' <summary>
        '''  The file was renamed and this is the new name.
        ''' </summary>
        <Description(" The file was renamed and this is the new name.")>
        RenamedNewName = &H5

    End Enum

    ''' <summary>
    ''' Class that provides information about an event that has occurred on a watch file system item.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class FileNotifyInfo
        Implements ICloneable

        Private _Filename As String
        Private _Action As FileActions
        Private _Next As FileNotifyInfo

        ''' <summary>
        ''' Returns the old filename of a renamed file.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property OldName As String
            Get
                Return Filename
            End Get
        End Property

        ''' <summary>
        ''' Returns the new filename of a renamed file.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property NewName As String
            Get
                If _Next IsNot Nothing Then Return _Next.Filename Else Return Nothing
            End Get
        End Property

        ''' <summary>
        ''' Returns the filename upon which the action occurred.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Filename As String
            Get
                Return _Filename
            End Get
        End Property

        ''' <summary>
        ''' Specifies the action that occurred.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Action As FileActions
            Get
                Return _Action
            End Get
        End Property

        ''' <summary>
        ''' Gets the next entry in a chain of actions.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property NextEntry As FileNotifyInfo
            Get
                Return _Next
            End Get
        End Property

        ''' <summary>
        ''' Initialize a new instance of this class with the specified FILE_NOTIFY_INFORMATION structure.
        ''' </summary>
        ''' <param name="fni"></param>
        ''' <remarks></remarks>
        Friend Sub New(fni As FILE_NOTIFY_INFORMATION)

            _Filename = fni.Filename
            _Action = fni.Action
            If fni.NextEntryOffset > 0 Then
                _Next = New FileNotifyInfo(fni.NextEntry)
            End If
        End Sub

        Public Function Clone() As Object Implements ICloneable.Clone
            Dim fni As FileNotifyInfo = CType(Me.MemberwiseClone(), FileNotifyInfo)

            If _Next IsNot Nothing Then
                fni._Next = CType(_Next.Clone(), FileNotifyInfo)
            End If

            Return fni
        End Function

    End Class

    ''' <summary>
    ''' File notification structure (pointer structure).
    ''' </summary>
    ''' <remarks></remarks>
    <StructLayout(LayoutKind.Sequential)>
    Public Structure FILE_NOTIFY_INFORMATION
        Public ptr As MemPtr

        ''' <summary>
        ''' Gets the new name of a renamed file.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property NewName As String
            Get
                Return NextEntry.Filename
            End Get
        End Property

        ''' <summary>
        ''' Gets the old name of a renamed file.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property OldName As String
            Get
                Return Filename
            End Get
        End Property

        ''' <summary>
        ''' Gets the byte offset of the next entry relative to the pointer for this item.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NextEntryOffset As Integer
            Get
                If ptr = IntPtr.Zero Then Return 0
                Return ptr.IntAt(0)
            End Get
            Set(value As Integer)
                If ptr = IntPtr.Zero Then Return
                ptr.IntAt(0) = value
            End Set
        End Property

        ''' <summary>
        ''' Specifies the action that occurred.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Action As FileActions
            Get
                If ptr = IntPtr.Zero Then Return 0
                Return CType(ptr.IntAt(1), FileActions)
            End Get
        End Property

        ''' <summary>
        ''' Returns the length of the filename.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property FilenameLength As Integer
            Get
                If ptr = IntPtr.Zero Then Return 0
                Return ptr.IntAt(2)
            End Get
        End Property

        ''' <summary>
        ''' Returns the filename.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Filename As String
            Get
                If ptr = IntPtr.Zero OrElse FilenameLength = 0 Then Return Nothing
                Return ptr.GetString(12, FilenameLength)
            End Get
        End Property

        ''' <summary>
        ''' Returns the pointer to the next entry as a new FILE_NOTIFY_INFORMATION pointer structure.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property NextEntry As FILE_NOTIFY_INFORMATION
            Get
                If NextEntryOffset <= 0 Then Return Nothing

                Dim m As New FILE_NOTIFY_INFORMATION

                m.ptr.Handle = ptr
                m.ptr.Handle += NextEntryOffset

                Return m
            End Get
        End Property

    End Structure

    ''' <summary>
    ''' Class that specifies details and arguments for an FSMonitor event.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class FSMonitorEventArgs
        Inherits EventArgs
        Implements IDisposable

        Friend _Handled As Boolean = False

        Private _Storage As String
        Friend _Info As FileNotifyInfo
        Private _sender As FSMonitor

        ''' <summary>
        ''' Create a new event arguments from the specified information.
        ''' </summary>
        ''' <param name="inf"></param>
        ''' <param name="wnd"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Friend Shared Function FromPtr(inf As FILE_NOTIFY_INFORMATION, wnd As FSMonitor) As FSMonitorEventArgs
            Dim fsm As New FSMonitorEventArgs

            fsm._Info = New FileNotifyInfo(inf)
            fsm._sender = wnd
            fsm._Storage = wnd.Storage

            Return fsm
        End Function

        ''' <summary>
        ''' Specifies the action for the event.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Action As FileActions
            Get
                Return _Info.Action
            End Get
        End Property

        ''' <summary>
        ''' Gets the FileNotifyInfo object for the event.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Info As FileNotifyInfo
            Get
                Return _Info
            End Get
        End Property

        ''' <summary>
        ''' Returns the associated String implementation for the event.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Storage As String
            Get
                Return _Storage
            End Get
            Friend Set(value As String)
                _Storage = value
            End Set
        End Property

        ''' <summary>
        ''' Specifies the owning FSMonitor object.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Sender As FSMonitor
            Get
                Return _sender
            End Get
            Friend Set(value As FSMonitor)
                _sender = value
            End Set
        End Property

        Friend Sub New()

        End Sub

        Friend Sub New(sender As FSMonitor, stor As String, n As FILE_NOTIFY_INFORMATION, a As FileActions)
            _sender = sender
            _Storage = stor
        End Sub

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                _Info = Nothing
            End If
            Me.disposedValue = True
        End Sub

        Protected Overrides Sub Finalize()
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(False)
            MyBase.Finalize()
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

    End Class

    ''' <summary>
    ''' Reasons for monitor closure.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum MonitorClosedState

        ''' <summary>
        ''' The monitor was closed by a user action or a normal program event.
        ''' </summary>
        ''' <remarks></remarks>
        Closed

        ''' <summary>
        ''' The monitor was closed because of some error.
        ''' </summary>
        ''' <remarks></remarks>
        ClosedOnError

        ''' <summary>
        ''' The monitor was closed because the directory it was watching was deleted.
        ''' </summary>
        ''' <remarks></remarks>
        ClosedOnRemove
    End Enum

    ''' <summary>
    ''' Class to describe a MonitorClosed event.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class MonitorClosedEventArgs
        Inherits EventArgs

        Private _cs As MonitorClosedState

        Private _ec As Integer = 0

        Private _em As String

        ''' <summary>
        ''' Gets the error message, if any.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ErrorMessage As String
            Get
                Return _em
            End Get
        End Property

        ''' <summary>
        ''' Gets the error code, if any.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ErrorCode As Integer
            Get
                Return _ec
            End Get
        End Property

        ''' <summary>
        ''' Gets the closed state of the object.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ClosedState As MonitorClosedState
            Get
                Return _cs
            End Get
        End Property

        ''' <summary>
        ''' Create a new instance of this class with the specified closed state.
        ''' </summary>
        ''' <param name="cs">The monitor closed state.</param>
        ''' <remarks></remarks>
        Friend Sub New(cs As MonitorClosedState)
            _cs = cs
        End Sub

        ''' <summary>
        ''' Create a new instance of this class with the specified closed state and error code.
        ''' </summary>
        ''' <param name="cs">The monitor closed state.</param>
        ''' <param name="ec">The error code.</param>
        ''' <remarks></remarks>
        Friend Sub New(cs As MonitorClosedState, ec As Integer)
            _cs = cs
            _ec = ec
        End Sub

        ''' <summary>
        ''' Create a new instance of this class with the specified closed state, error code and error message.
        ''' </summary>
        ''' <param name="cs">The monitor closed state.</param>
        ''' <param name="ec">The error code.</param>
        ''' <param name="em">The error message.</param>
        ''' <remarks></remarks>
        Friend Sub New(cs As MonitorClosedState, ec As Integer, em As String)
            _cs = cs
            _ec = ec
            _em = em
        End Sub

    End Class

    ''' <summary>
    ''' Class to watch the file system.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class FSMonitor
        Inherits NativeWindow

        Implements IDisposable

        Public Const DefaultBufferSize = 128000

        Protected _stor As String
        Protected _hFile As IntPtr
        Protected _isWatching As Boolean
        Protected _thread As System.Threading.Thread
        Protected _Buff As MemPtr

        ' the default filter will be write, and rename (which includes delete, create and move).
        Protected _Filter As NotifyFilter = NotifyFilter.NotifyWrite Or NotifyFilter.NotifyFileRename Or NotifyFilter.NotifyDirectoryRename

        ' this is the action buffer that gets handled by the message pump.
        Protected _WaitList As New List(Of FSMonitorEventArgs)

        Protected _SigAdd As FSMonitorEventArgs

        Protected _WaitLock As Integer = 0
        Protected _lastIndex As Integer = 0

        Protected _owner As IntPtr

#Region "Public Events"

        ''' <summary>
        ''' The event that get fired when a change is detected in the monitored path.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event WatchNotifyChange(sender As Object, e As FSMonitorEventArgs)

        ''' <summary>
        ''' The event that gets fired when the monitor is opened.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event MonitorOpened(sender As Object, e As EventArgs)

        ''' <summary>
        ''' The event that gets fired when the monitor is closed.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event MonitorClosed(sender As Object, e As MonitorClosedEventArgs)

#End Region

#Region "Public Properties"

        ''' <summary>
        ''' Gets or sets the NotifyFilter criteria for this monitor object.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Filter As NotifyFilter
            Get
                Return _Filter
            End Get
            Set(value As NotifyFilter)
                _Filter = value
                If _isWatching Then
                    StopWatching()
                    Watch()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets a value indicating if the monitor thread is running.
        ''' </summary>
        ''' <value></value>
        ''' <returns>True if the monitor is open.</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property IsWatching As Boolean
            Get
                Return _isWatching
            End Get
        End Property

        ''' <summary>
        ''' Retrieves the open file handle.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property FileHandle As IntPtr
            Get
                Return _hFile
            End Get
        End Property

        ''' <summary>
        ''' Retrieves the associated String object.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Storage As String
            Get
                Return _stor
            End Get
        End Property

        ''' <summary>
        ''' Returns the owner window of this monitor, if any.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Owner As IntPtr
            Get
                Owner = _owner
            End Get
        End Property

#End Region

#Region "Public Methods"

        ''' <summary>
        ''' Create and activate the file system monitor thread.
        ''' </summary>
        ''' <returns>
        ''' True if the thread was successfully created.
        ''' To ensure the thread was successfully activated, handle the MonitorOpened event.
        ''' </returns>
        ''' <remarks></remarks>
        Public Function Watch() As Boolean
            If _isWatching Then Return False

            Return internalWatch()
        End Function

        ''' <summary>
        ''' Deactivate and destroy the file system monitor thread.
        ''' </summary>
        ''' <returns>True if the thread was successfully deactivated and the file handle was closed.</returns>
        ''' <remarks></remarks>
        Public Function StopWatching() As Boolean
            If Not _isWatching Then Return False

            internalCloseFile()
            Return _hFile = IntPtr.Zero

        End Function

#End Region

#Region "Instantiation"

        ''' <summary>
        ''' Initialize a new instance of the FSMonitor class with the specified String object.
        ''' </summary>
        ''' <param name="stor">The String object whose StorageRoot will be the target of the monitor.</param>
        ''' <remarks></remarks>
        Public Sub New(stor As String)
            _stor = stor
            internalCreate()
        End Sub

        ''' <summary>
        ''' Initialize a new instance of the FSMonitor class with the specified String object.
        ''' </summary>
        ''' <param name="stor">The String object whose StorageRoot will be the target of the monitor.</param>
        ''' <param name="buffLen">Length of the file system changes buffer.</param>
        ''' <remarks></remarks>
        Public Sub New(stor As String, buffLen As Integer)
            _stor = stor
            internalCreate(buffLen)
        End Sub

        ''' <summary>
        ''' Initialize a new instance of the FSMonitor class with the specified String object and parent window handle.
        ''' </summary>
        ''' <param name="stor">The String object whose StorageRoot will be the target of the monitor.</param>
        ''' <param name="hwndOwner">The handle to the owner window.</param>
        ''' <remarks></remarks>
        Public Sub New(stor As String, hwndOwner As IntPtr)
            _stor = stor
            _owner = hwndOwner
            internalCreate()
        End Sub

        ''' <summary>
        ''' Initialize a new instance of the FSMonitor class with the specified String object and parent window handle.
        ''' </summary>
        ''' <param name="stor">The String object whose StorageRoot will be the target of the monitor.</param>
        ''' <param name="hwndOwner">The handle to the owner window.</param>
        ''' <param name="buffLen">Length of the file system changes buffer.</param>
        ''' <remarks></remarks>
        Public Sub New(stor As String, hwndOwner As IntPtr, buffLen As Integer)
            _stor = stor
            _owner = hwndOwner
            internalCreate(buffLen)
        End Sub

#End Region

#Region "Internal Methods"

        ''' <summary>
        ''' Creates the window.
        ''' </summary>
        ''' <param name="buffLen">Optional length of the file system changes buffer.</param>
        ''' <returns>True if successful.</returns>
        Private Function internalCreate(Optional buffLen As Integer = DefaultBufferSize) As Boolean

            If buffLen < 1024 Then
                Throw New ArgumentException("Buffer length cannot be smaller than 1k.")
            End If

            Try

                Dim cp As New CreateParams

                If _owner <> IntPtr.Zero Then
                    cp.Style = WS_CHILDWINDOW
                    cp.Parent = _owner
                End If

                Me.CreateHandle(cp)

            Catch ex As Exception
                Return False
            End Try

            '' 128k is definitely enough when the thread is running continuously.
            '' barring something funky blocking us ...
            Return _Buff.AllocZero(buffLen, True)
        End Function

        ''' <summary>
        ''' Internally perform the actions necessary to open the target directory.
        ''' </summary>
        ''' <returns>True if a file handle was successfully acquired.</returns>
        ''' <remarks></remarks>
        Protected Function internalOpenFile() As Boolean
            _hFile = CreateFile(_stor,
                            GENERIC_READ Or
                            FILE_READ_ATTRIBUTES Or
                            FILE_READ_DATA Or
                            FILE_READ_EA Or
                            FILE_LIST_DIRECTORY Or
                            FILE_TRAVERSE,
                            FILE_SHARE_READ Or
                            FILE_SHARE_WRITE Or
                            FILE_SHARE_DELETE,
                            IntPtr.Zero,
                            OPEN_EXISTING,
                            FILE_FLAG_BACKUP_SEMANTICS Or
                            FILE_FLAG_OVERLAPPED,
                            IntPtr.Zero)

            If _hFile = CType(-1, IntPtr) Then
                Return False
            End If

            Return True
        End Function

        ''' <summary>
        ''' Internally performs the actions necessary to close the file handle to the associated folder.
        ''' </summary>
        ''' <remarks></remarks>
        Protected Sub internalCloseFile()
            If CloseHandle(_hFile) Then
                _hFile = IntPtr.Zero
            End If
        End Sub

        ''' <summary>
        ''' Internally creates and starts the monitor thread.
        ''' </summary>
        ''' <returns>
        ''' True if the thread was successfully created.
        ''' To ensure the monitor was successfully activated, handle the MonitorOpened event.
        ''' </returns>
        ''' <remarks></remarks>
        Protected Function internalWatch() As Boolean

            Dim blen As Integer = 0
            Dim bufflen As Integer = CInt(_Buff.Size)
            Dim tbuff As IntPtr = _Buff.Handle

            If _thread IsNot Nothing Then Return False

            If Not internalOpenFile() Then Return False

            Dim fn As FILE_NOTIFY_INFORMATION
            fn.ptr = _Buff

            _thread = New Threading.Thread(
            Sub()

                Dim notice As IntPtr = IntPtr.Zero
                PostMessage(Handle, WM_SIGNAL_OPEN, IntPtr.Zero, IntPtr.Zero)

                Do

                    Try
                        ' let's clean up the memory before the next execute.
                        If blen > 0 Then
                            _Buff.ZeroMemory(0, blen)
                            blen = 0
                        End If

                        If Not ReadDirectoryChangesW(_hFile,
                                    tbuff,
                                    bufflen,
                                    True,
                                    _Filter,
                                    blen,
                                    IntPtr.Zero,
                                    IntPtr.Zero) Then

                            notice = CType(GetLastError, IntPtr)
                            Exit Do
                        End If
                    Catch ex As Threading.ThreadAbortException

                        Exit Do

                    Catch ex2 As Exception

                        notice = CType(1, IntPtr)
                        Exit Do

                    End Try

                    ' block until the lock is acquired.  Hopefully the
                    ' UI thread will not take that long to clean the list.
                    Threading.Monitor.Enter(_WaitList)

                    _WaitList.Add(FSMonitorEventArgs.FromPtr(fn, Me))
                    ' and we're done ...
                    Threading.Monitor.Exit(_WaitList)

                    ' post to the UI thread that there are items to dequeue and continue!
                    PostMessage(Handle, WM_SIGNAL, IntPtr.Zero, IntPtr.Zero)
                Loop

                _thread = Nothing
                PostMessage(Handle, WM_SIGNAL_CLOSE, IntPtr.Zero, IntPtr.Zero)
            End Sub)

            _thread.SetApartmentState(Threading.ApartmentState.STA)
            _thread.IsBackground = True

            If _thread.IsAlive = False Then _thread.Start()

            Return True

        End Function

#End Region

#Region "Window Proc"

        ''' <summary>
        ''' Internal message pump handler and event dispatcher.
        ''' </summary>
        ''' <param name="m"></param>
        ''' <remarks></remarks>
        Protected Overrides Sub WndProc(ByRef m As Message)

            Select Case m.Msg

                Case WM_SIGNAL
                    '' don't block on the main thread, block on the watching thread, instead.
                    If (Threading.Monitor.TryEnter(_WaitList)) Then
                        Dim c As Integer,
                        i As Integer

                        '' there are items waiting to be dequeued, let's dequeue one.
                        i = _lastIndex
                        c = _WaitList.Count - 1

                        '' make sure we're not jumping ahead of a previous cleaning.
                        If (c >= i) Then

                            If (_WaitList(i) IsNot Nothing) Then

                                '' post the events so that whatever is watching this folder can do its thing.

                                _WaitList(i)._Info = CType(_WaitList(i)._Info.Clone, FileNotifyInfo)
                                RaiseEvent WatchNotifyChange(Me, _WaitList(i))

                                '' remove the item from its slot in the queue, thereby
                                '' eliminating any chance the same event will be fired, again.
                                _WaitList(i) = Nothing

                            End If

                            '' post a message to the queue cleaner.  if there are more files, it will send the pump back this way.
                            _lastIndex = i + 1
                            PostMessage(Handle, WM_SIGNAL_CLEAN, IntPtr.Zero, IntPtr.Zero)
                        End If

                        Threading.Monitor.Exit(_WaitList)
                    Else
                        '' going too fast?  we'll get there, eventually.  At least we know they're queuing.
                        If (_WaitList.Count > 0) Then PostMessage(Handle, WM_SIGNAL, IntPtr.Zero, IntPtr.Zero)
                    End If

                Case WM_SIGNAL_CLEAN

                    ' don't block on the main thread, block on the watching thread, instead.
                    If Threading.Monitor.TryEnter(_WaitList) Then

                        ' we have a lock, let's clean up the queue
                        Dim i As Integer,
                        c As Integer = _WaitList.Count - 1

                        For i = c To 0 Step -1
                            '' we want to only remove slots that have been dequeued.
                            If _WaitList(i) Is Nothing Then
                                _WaitList.RemoveAt(i)
                            End If
                        Next

                        '' reset the lastindex to 0, indicating that any items still in the queue have not fired, yet.
                        _lastIndex = 0
                        Threading.Monitor.Exit(_WaitList)

                        '' if we still have more signals in the queue, tell the message pump to keep on truckin'.
                        If _WaitList.Count > 0 Then PostMessage(Handle, WM_SIGNAL, IntPtr.Zero, IntPtr.Zero)
                    Else
                        ' oh snap!  can't lock it, let's send another clean message to make sure we do finally execute, eventually.
                        PostMessage(Handle, WM_SIGNAL_CLEAN, IntPtr.Zero, IntPtr.Zero)
                    End If

                Case WM_SIGNAL_OPEN

                    _isWatching = True
                    RaiseEvent MonitorOpened(Me, New EventArgs)

                Case WM_SIGNAL_CLOSE

                    _isWatching = False

                    If m.LParam.ToInt32 >= 1 Then
                        RaiseEvent MonitorClosed(Me, New MonitorClosedEventArgs(MonitorClosedState.ClosedOnError, m.LParam.ToInt32, FormatLastError(CUInt(m.LParam.ToInt32))))
                    Else
                        RaiseEvent MonitorClosed(Me, New MonitorClosedEventArgs(MonitorClosedState.Closed))
                    End If

                Case Else
                    MyBase.WndProc(m)

            End Select

        End Sub

#End Region

#Region "IDisposable Support"

        ''' <summary>
        ''' Returns true if the monitor has been disposed.
        ''' If it has been disposed, it may not be reused.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Disposed As Boolean
            Get
                Return disposedValue
            End Get
        End Property

        Private disposedValue As Boolean ' To detect redundant calls
        Private shuttingDown As Boolean

        ''' <summary>
        ''' Dispose of the managed and unmanaged resources.
        ''' </summary>
        ''' <param name="disposing"></param>
        ''' <remarks></remarks>
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                shuttingDown = True

                If _isWatching Then StopWatching()
                _lastIndex = 0

                _WaitList = Nothing
                _WaitLock = 0

                '' destroy the window handle
                Me.DestroyHandle()

                '' free the buffer
                _Buff.Free(True)

                '' release the String handle
                _stor = Nothing
            End If

            Me.disposedValue = True
            Me.shuttingDown = False
        End Sub

        Protected Overrides Sub Finalize()
            Dispose(False)
            MyBase.Finalize()
        End Sub

        ''' <summary>
        ''' Deactivate the monitor, destroy the window handle and dispose of any managed or unmanaged resources.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

    End Class

End Namespace