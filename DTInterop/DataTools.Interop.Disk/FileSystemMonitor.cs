// ' ************************************************* ''
// ' DataTools Visual Basic Utility Library - Interop
// '
// ' Module: Real-time FileSystemMonitor implementation
// '         With multi-threading and synchronized
// '         with the app thread.
// ' 
// ' Copyright (C) 2011-2020 Nathan Moschkin
// ' All Rights Reserved
// '
// ' Licensed Under the Microsoft Public License   
// ' ************************************************* ''

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using CoreCT.Memory;
using DataTools.Interop.Native;
using Microsoft.VisualBasic.CompilerServices;

namespace DataTools.Interop.Disk
{
    static class FileSystemMonitor
    {

        /// <summary>
        /// Defines the base for all custom messages in this module.
        /// </summary>
        /// <remarks></remarks>
        public const int WM_MYBASE = PInvoke.WM_USER + 0x100;

        /// <summary>
        /// Signals that a change has occurred within the context of a watched folder.
        /// </summary>
        /// <remarks></remarks>
        public const int WM_SIGNAL = WM_MYBASE + 1;

        /// <summary>
        /// Signals the message pump to attempt to clear the cache of processed events.
        /// </summary>
        /// <remarks></remarks>
        public const int WM_SIGNAL_CLEAN = WM_MYBASE + 2;

        /// <summary>
        /// Signals that the monitor has been opened.
        /// </summary>
        /// <remarks></remarks>
        public const int WM_SIGNAL_OPEN = WM_MYBASE + 3;

        /// <summary>
        /// Signals that the monitor has been closed.
        /// </summary>
        /// <remarks></remarks>
        public const int WM_SIGNAL_CLOSE = WM_MYBASE + 4;

        /// <summary>
        /// The beginning of custom error codes.
        /// </summary>
        /// <remarks></remarks>
        public const int ERROR_MIN = 0x20000;

        /// <summary>
        /// Specifies that a mirroring action has failed.
        /// </summary>
        /// <remarks></remarks>
        public const int ERROR_MIRRORFAIL = ERROR_MIN + 1;

        /// <summary>
        /// Specifies that a failure regarding a specific path has occurred.
        /// </summary>
        /// <remarks></remarks>
        public const int ERROR_PATHFAIL = ERROR_MIN + 2;

        /// <summary>
        /// Specifies that a failure regarding a specific file has occurred.
        /// </summary>
        /// <remarks></remarks>
        public const int ERROR_FILEFAIL = ERROR_MIN + 3;

        /// <summary>
        /// Specifies that a failure regarding a specific destination path has occurred.
        /// </summary>
        /// <remarks></remarks>
        public const int ERROR_DESTPATHFAIL = ERROR_MIN + 4;

        /// <summary>
        /// Specifies that a failure regarding a specific destination file has occurred.
        /// </summary>
        /// <remarks></remarks>
        public const int ERROR_DESTFILEFAIL = ERROR_MIN + 5;

        [DllImport("kernel32", EntryPoint = "CreateEventW", CharSet = CharSet.Unicode)]

        public static extern IntPtr CreateEvent(IntPtr lpEventAttributes, bool bManualREset, bool bInitialState, string lpName);
        [DllImport("kernel32")]
        public static extern bool SetEvent(IntPtr hEvent);
        [DllImport("kernel32")]
        public static extern bool ResetEvent(IntPtr hEvent);
        [DllImport("kernel32", EntryPoint = "FindFirstChangeNotificationW", CharSet = CharSet.Unicode)]

        public static extern IntPtr FindFirstChangeNotification(string lpPathName, bool bWatchSubtree, NotifyFilter dwNotifyFilter);

        public delegate void FileIoCompletionDelegate(int dwErrorCode, int dwNumberOfBytesTransfered, System.Threading.NativeOverlapped lpOverlapped);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        public static extern bool ReadDirectoryChangesW(IntPtr hDirectory, IntPtr lpBuffer, int nBufferLength, [MarshalAs(UnmanagedType.Bool)] bool bWatchSubtree, NotifyFilter dwNotifyFilter, ref int lpBytesReturned, IntPtr lpOverlapped, IntPtr lpCompletionRoutine);
    }

    /// <summary>
    /// Sets the filter criteria for directory change notifications.
    /// </summary>
    /// <remarks></remarks>
    [Flags]
    public enum NotifyFilter
    {

        /// <summary>
        /// Any file name change in the watched directory or subtree causes a change notification wait operation to return. Changes include renaming, creating, or deleting a file.
        /// </summary>
        [Description("Any file name change in the watched directory or subtree causes a change notification wait operation to return. Changes include renaming, creating, or deleting a file.")]
        NotifyFileRename = 0x1,

        /// <summary>
        /// Any directory-name change in the watched directory or subtree causes a change notification wait operation to return. Changes include creating or deleting a directory.
        /// </summary>
        [Description("Any directory-name change in the watched directory or subtree causes a change notification wait operation to return. Changes include creating or deleting a directory.")]
        NotifyDirectoryRename = 0x2,

        /// <summary>
        /// Any attribute change in the watched directory or subtree causes a change notification wait operation to return.
        /// </summary>
        [Description("Any attribute change in the watched directory or subtree causes a change notification wait operation to return.")]
        NotifyAttributesChange = 0x4,

        /// <summary>
        /// Any file-size change in the watched directory or subtree causes a change notification wait operation to return. The operating system detects a change in file size only when the file is written to the disk. For operating systems that use extensive caching, detection occurs only when the cache is sufficiently flushed.
        /// </summary>
        [Description("Any file-size change in the watched directory or subtree causes a change notification wait operation to return. The operating system detects a change in file size only when the file is written to the disk. For operating systems that use extensive caching, detection occurs only when the cache is sufficiently flushed.")]
        NotifySizeChange = 0x8,

        /// <summary>
        /// Any change to the last write-time of files in the watched directory or subtree causes a change notification wait operation to return. The operating system detects a change to the last write-time only when the file is written to the disk. For operating systems that use extensive caching, detection occurs only when the cache is sufficiently flushed.
        /// </summary>
        [Description("Any change to the last write-time of files in the watched directory or subtree causes a change notification wait operation to return. The operating system detects a change to the last write-time only when the file is written to the disk. For operating systems that use extensive caching, detection occurs only when the cache is sufficiently flushed.")]
        NotifyWrite = 0x10,

        /// <summary>
        /// Any change to the last access time of files in the watched directory or subtree causes a change notification wait operation to return.
        /// </summary>
        [Description("Any change to the last access time of files in the watched directory or subtree causes a change notification wait operation to return.")]
        NotifyAccess = 0x20,

        /// <summary>
        /// Any change to the creation time of files in the watched directory or subtree causes a change notification wait operation to return.
        /// </summary>
        [Description("Any change to the creation time of files in the watched directory or subtree causes a change notification wait operation to return.")]
        NotifyCreate = 0x40,

        /// <summary>
        /// Any security-descriptor change in the watched directory or subtree causes a change notification wait operation to return.
        /// </summary>
        [Description("Any security-descriptor change in the watched directory or subtree causes a change notification wait operation to return.")]
        NotifySecurityChange = 0x100
    }

    /// <summary>
    /// Specifies an action on a file or a folder.
    /// </summary>
    /// <remarks></remarks>
    public enum FileActions
    {

        /// <summary>
        /// The file was added to the directory.
        /// </summary>
        [Description(" The file was added to the directory.")]
        Added = 0x1,

        /// <summary>
        /// The file was removed from the directory.
        /// </summary>
        [Description(" The file was removed from the directory.")]
        Removed = 0x2,

        /// <summary>
        /// The file was modified. This can be a change in the time stamp or attributes.
        /// </summary>
        [Description(" The file was modified. This can be a change in the time stamp or attributes.")]
        Modified = 0x3,

        /// <summary>
        /// The file was renamed and this is the old name.
        /// </summary>
        [Description(" The file was renamed and this is the old name.")]
        RenamedOldName = 0x4,

        /// <summary>
        /// The file was renamed and this is the new name.
        /// </summary>
        [Description(" The file was renamed and this is the new name.")]
        RenamedNewName = 0x5
    }

    /// <summary>
    /// Class that provides information about an event that has occurred on a watch file system item.
    /// </summary>
    /// <remarks></remarks>
    public class FileNotifyInfo : ICloneable
    {
        private string _Filename;
        private FileActions _Action;
        private FileNotifyInfo _Next;

        /// <summary>
        /// Returns the old filename of a renamed file.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string OldName
        {
            get
            {
                return Filename;
            }
        }

        /// <summary>
        /// Returns the new filename of a renamed file.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string NewName
        {
            get
            {
                if (_Next is object)
                    return _Next.Filename;
                else
                    return null;
            }
        }

        /// <summary>
        /// Returns the filename upon which the action occurred.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Filename
        {
            get
            {
                return _Filename;
            }
        }

        /// <summary>
        /// Specifies the action that occurred.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public FileActions Action
        {
            get
            {
                return _Action;
            }
        }

        /// <summary>
        /// Gets the next entry in a chain of actions.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public FileNotifyInfo NextEntry
        {
            get
            {
                return _Next;
            }
        }

        /// <summary>
        /// Initialize a new instance of this class with the specified FILE_NOTIFY_INFORMATION structure.
        /// </summary>
        /// <param name="fni"></param>
        /// <remarks></remarks>
        internal FileNotifyInfo(FILE_NOTIFY_INFORMATION fni)
        {
            _Filename = fni.Filename;
            _Action = fni.Action;
            if (fni.NextEntryOffset > 0)
            {
                _Next = new FileNotifyInfo(fni.NextEntry);
            }
        }

        public object Clone()
        {
            FileNotifyInfo fni = (FileNotifyInfo)MemberwiseClone();
            if (_Next is object)
            {
                fni._Next = (FileNotifyInfo)_Next.Clone();
            }

            return fni;
        }
    }

    /// <summary>
    /// File notification structure (pointer structure).
    /// </summary>
    /// <remarks></remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct FILE_NOTIFY_INFORMATION
    {
        public MemPtr ptr;

        /// <summary>
        /// Gets the new name of a renamed file.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string NewName
        {
            get
            {
                return NextEntry.Filename;
            }
        }

        /// <summary>
        /// Gets the old name of a renamed file.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string OldName
        {
            get
            {
                return Filename;
            }
        }

        /// <summary>
        /// Gets the byte offset of the next entry relative to the pointer for this item.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int NextEntryOffset
        {
            get
            {
                if (ptr == IntPtr.Zero)
                    return 0;
                return ptr.IntAt(0L);
            }

            set
            {
                if (ptr == IntPtr.Zero)
                    return;
                ptr.IntAt(0L) = value;
            }
        }

        /// <summary>
        /// Specifies the action that occurred.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public FileActions Action
        {
            get
            {
                if (ptr == IntPtr.Zero)
                    return 0;
                return (FileActions)Conversions.ToInteger(ptr.IntAt(1L));
            }
        }

        /// <summary>
        /// Returns the length of the filename.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int FilenameLength
        {
            get
            {
                if (ptr == IntPtr.Zero)
                    return 0;
                return ptr.IntAt(2L);
            }
        }

        /// <summary>
        /// Returns the filename.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Filename
        {
            get
            {
                if (ptr == IntPtr.Zero || FilenameLength == 0)
                    return null;
                return ptr.GetString(12L, FilenameLength);
            }
        }

        /// <summary>
        /// Returns the pointer to the next entry as a new FILE_NOTIFY_INFORMATION pointer structure.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public FILE_NOTIFY_INFORMATION NextEntry
        {
            get
            {
                if (NextEntryOffset <= 0)
                    return default;
                var m = new FILE_NOTIFY_INFORMATION();
                m.ptr.Handle = ptr;
                m.ptr.Handle = m.ptr.Handle + NextEntryOffset;
                return m;
            }
        }
    }

    /// <summary>
    /// Class that specifies details and arguments for an FSMonitor event.
    /// </summary>
    /// <remarks></remarks>
    public class FSMonitorEventArgs : EventArgs, IDisposable
    {
        internal bool _Handled = false;
        private string _Storage;
        internal FileNotifyInfo _Info;
        private FSMonitor _sender;

        /// <summary>
        /// Create a new event arguments from the specified information.
        /// </summary>
        /// <param name="inf"></param>
        /// <param name="wnd"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        internal static FSMonitorEventArgs FromPtr(FILE_NOTIFY_INFORMATION inf, FSMonitor wnd)
        {
            var fsm = new FSMonitorEventArgs();
            fsm._Info = new FileNotifyInfo(inf);
            fsm._sender = wnd;
            fsm._Storage = wnd.Storage;
            return fsm;
        }

        /// <summary>
        /// Specifies the action for the event.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public FileActions Action
        {
            get
            {
                return _Info.Action;
            }
        }

        /// <summary>
        /// Gets the FileNotifyInfo object for the event.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public FileNotifyInfo Info
        {
            get
            {
                return _Info;
            }
        }

        /// <summary>
        /// Returns the associated String implementation for the event.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Storage
        {
            get
            {
                return _Storage;
            }

            internal set
            {
                _Storage = value;
            }
        }

        /// <summary>
        /// Specifies the owning FSMonitor object.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public FSMonitor Sender
        {
            get
            {
                return _sender;
            }

            internal set
            {
                _sender = value;
            }
        }

        internal FSMonitorEventArgs()
        {
        }

        internal FSMonitorEventArgs(FSMonitor sender, string stor, FILE_NOTIFY_INFORMATION n, FileActions a)
        {
            _sender = sender;
            _Storage = stor;
        }

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        private bool disposedValue; // To detect redundant calls

        // IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                _Info = null;
            }

            disposedValue = true;
        }

        ~FSMonitorEventArgs()
        {
            // Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(false);
        }

        public void Dispose()
        {
            // Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    }

    /// <summary>
    /// Reasons for monitor closure.
    /// </summary>
    /// <remarks></remarks>
    public enum MonitorClosedState
    {

        /// <summary>
        /// The monitor was closed by a user action or a normal program event.
        /// </summary>
        /// <remarks></remarks>
        Closed,

        /// <summary>
        /// The monitor was closed because of some error.
        /// </summary>
        /// <remarks></remarks>
        ClosedOnError,

        /// <summary>
        /// The monitor was closed because the directory it was watching was deleted.
        /// </summary>
        /// <remarks></remarks>
        ClosedOnRemove
    }

    /// <summary>
    /// Class to describe a MonitorClosed event.
    /// </summary>
    /// <remarks></remarks>
    public class MonitorClosedEventArgs : EventArgs
    {
        private MonitorClosedState _cs;
        private int _ec = 0;
        private string _em;

        /// <summary>
        /// Gets the error message, if any.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string ErrorMessage
        {
            get
            {
                return _em;
            }
        }

        /// <summary>
        /// Gets the error code, if any.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int ErrorCode
        {
            get
            {
                return _ec;
            }
        }

        /// <summary>
        /// Gets the closed state of the object.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public MonitorClosedState ClosedState
        {
            get
            {
                return _cs;
            }
        }

        /// <summary>
        /// Create a new instance of this class with the specified closed state.
        /// </summary>
        /// <param name="cs">The monitor closed state.</param>
        /// <remarks></remarks>
        internal MonitorClosedEventArgs(MonitorClosedState cs)
        {
            _cs = cs;
        }

        /// <summary>
        /// Create a new instance of this class with the specified closed state and error code.
        /// </summary>
        /// <param name="cs">The monitor closed state.</param>
        /// <param name="ec">The error code.</param>
        /// <remarks></remarks>
        internal MonitorClosedEventArgs(MonitorClosedState cs, int ec)
        {
            _cs = cs;
            _ec = ec;
        }

        /// <summary>
        /// Create a new instance of this class with the specified closed state, error code and error message.
        /// </summary>
        /// <param name="cs">The monitor closed state.</param>
        /// <param name="ec">The error code.</param>
        /// <param name="em">The error message.</param>
        /// <remarks></remarks>
        internal MonitorClosedEventArgs(MonitorClosedState cs, int ec, string em)
        {
            _cs = cs;
            _ec = ec;
            _em = em;
        }
    }

    /// <summary>
    /// Class to watch the file system.
    /// </summary>
    /// <remarks></remarks>
    public class FSMonitor : NativeWindow, IDisposable
    {
        public const int DefaultBufferSize = 128000;
        protected string _stor;
        protected IntPtr _hFile;
        protected bool _isWatching;
        protected System.Threading.Thread _thread;
        protected MemPtr _Buff;

        // the default filter will be write, and rename (which includes delete, create and move).
        protected NotifyFilter _Filter = NotifyFilter.NotifyWrite | NotifyFilter.NotifyFileRename | NotifyFilter.NotifyDirectoryRename;

        // this is the action buffer that gets handled by the message pump.
        protected List<FSMonitorEventArgs> _WaitList = new List<FSMonitorEventArgs>();
        protected FSMonitorEventArgs _SigAdd;
        protected int _WaitLock = 0;
        protected int _lastIndex = 0;
        protected IntPtr _owner;

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// The event that get fired when a change is detected in the monitored path.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks></remarks>
        public event WatchNotifyChangeEventHandler WatchNotifyChange;

        public delegate void WatchNotifyChangeEventHandler(object sender, FSMonitorEventArgs e);

        /// <summary>
        /// The event that gets fired when the monitor is opened.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks></remarks>
        public event MonitorOpenedEventHandler MonitorOpened;

        public delegate void MonitorOpenedEventHandler(object sender, EventArgs e);

        /// <summary>
        /// The event that gets fired when the monitor is closed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks></remarks>
        public event MonitorClosedEventHandler MonitorClosed;

        public delegate void MonitorClosedEventHandler(object sender, MonitorClosedEventArgs e);

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Gets or sets the NotifyFilter criteria for this monitor object.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public NotifyFilter Filter
        {
            get
            {
                return _Filter;
            }

            set
            {
                _Filter = value;
                if (_isWatching)
                {
                    StopWatching();
                    Watch();
                }
            }
        }

        /// <summary>
        /// Gets a value indicating if the monitor thread is running.
        /// </summary>
        /// <value></value>
        /// <returns>True if the monitor is open.</returns>
        /// <remarks></remarks>
        public bool IsWatching
        {
            get
            {
                return _isWatching;
            }
        }

        /// <summary>
        /// Retrieves the open file handle.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public IntPtr FileHandle
        {
            get
            {
                return _hFile;
            }
        }

        /// <summary>
        /// Retrieves the associated String object.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Storage
        {
            get
            {
                return _stor;
            }
        }

        /// <summary>
        /// Returns the owner window of this monitor, if any.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public IntPtr Owner
        {
            get
            {
                IntPtr OwnerRet = default;
                OwnerRet = _owner;
                return OwnerRet;
            }
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Create and activate the file system monitor thread.
        /// </summary>
        /// <returns>
        /// True if the thread was successfully created.
        /// To ensure the thread was successfully activated, handle the MonitorOpened event.
        /// </returns>
        /// <remarks></remarks>
        public bool Watch()
        {
            if (_isWatching)
                return false;
            return internalWatch();
        }

        /// <summary>
        /// Deactivate and destroy the file system monitor thread.
        /// </summary>
        /// <returns>True if the thread was successfully deactivated and the file handle was closed.</returns>
        /// <remarks></remarks>
        public bool StopWatching()
        {
            if (!_isWatching)
                return false;
            internalCloseFile();
            return _hFile == IntPtr.Zero;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Initialize a new instance of the FSMonitor class with the specified String object.
        /// </summary>
        /// <param name="stor">The String object whose StorageRoot will be the target of the monitor.</param>
        /// <remarks></remarks>
        public FSMonitor(string stor)
        {
            _stor = stor;
            internalCreate();
        }

        /// <summary>
        /// Initialize a new instance of the FSMonitor class with the specified String object.
        /// </summary>
        /// <param name="stor">The String object whose StorageRoot will be the target of the monitor.</param>
        /// <param name="buffLen">Length of the file system changes buffer.</param>
        /// <remarks></remarks>
        public FSMonitor(string stor, int buffLen)
        {
            _stor = stor;
            internalCreate(buffLen);
        }

        /// <summary>
        /// Initialize a new instance of the FSMonitor class with the specified String object and parent window handle.
        /// </summary>
        /// <param name="stor">The String object whose StorageRoot will be the target of the monitor.</param>
        /// <param name="hwndOwner">The handle to the owner window.</param>
        /// <remarks></remarks>
        public FSMonitor(string stor, IntPtr hwndOwner)
        {
            _stor = stor;
            _owner = hwndOwner;
            internalCreate();
        }

        /// <summary>
        /// Initialize a new instance of the FSMonitor class with the specified String object and parent window handle.
        /// </summary>
        /// <param name="stor">The String object whose StorageRoot will be the target of the monitor.</param>
        /// <param name="hwndOwner">The handle to the owner window.</param>
        /// <param name="buffLen">Length of the file system changes buffer.</param>
        /// <remarks></remarks>
        public FSMonitor(string stor, IntPtr hwndOwner, int buffLen)
        {
            _stor = stor;
            _owner = hwndOwner;
            internalCreate(buffLen);
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Creates the window.
        /// </summary>
        /// <param name="buffLen">Optional length of the file system changes buffer.</param>
        /// <returns>True if successful.</returns>
        private bool internalCreate(int buffLen = DefaultBufferSize)
        {
            if (buffLen < 1024)
            {
                throw new ArgumentException("Buffer length cannot be smaller than 1k.");
            }

            try
            {
                var cp = new CreateParams();
                if (_owner != IntPtr.Zero)
                {
                    cp.Style = PInvoke.WS_CHILDWINDOW;
                    cp.Parent = _owner;
                }

                CreateHandle(cp);
            }
            catch (Exception ex)
            {
                return false;
            }

            // ' 128k is definitely enough when the thread is running continuously.
            // ' barring something funky blocking us ...
            return _Buff.AllocZero(buffLen, true);
        }

        /// <summary>
        /// Internally perform the actions necessary to open the target directory.
        /// </summary>
        /// <returns>True if a file handle was successfully acquired.</returns>
        /// <remarks></remarks>
        protected bool internalOpenFile()
        {
            _hFile = FileApi.CreateFile(_stor, FileApi.GENERIC_READ | FileApi.FILE_READ_ATTRIBUTES | FileApi.FILE_READ_DATA | FileApi.FILE_READ_EA | FileApi.FILE_LIST_DIRECTORY | FileApi.FILE_TRAVERSE, FileApi.FILE_SHARE_READ | FileApi.FILE_SHARE_WRITE | FileApi.FILE_SHARE_DELETE, IntPtr.Zero, FileApi.OPEN_EXISTING, FileApi.FILE_FLAG_BACKUP_SEMANTICS | FileApi.FILE_FLAG_OVERLAPPED, IntPtr.Zero);
            if (_hFile == (IntPtr)(-1))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Internally performs the actions necessary to close the file handle to the associated folder.
        /// </summary>
        /// <remarks></remarks>
        protected void internalCloseFile()
        {
            if (PInvoke.CloseHandle(_hFile))
            {
                _hFile = IntPtr.Zero;
            }
        }

        /// <summary>
        /// Internally creates and starts the monitor thread.
        /// </summary>
        /// <returns>
        /// True if the thread was successfully created.
        /// To ensure the monitor was successfully activated, handle the MonitorOpened event.
        /// </returns>
        /// <remarks></remarks>
        protected bool internalWatch()
        {
            int blen = 0;
            int bufflen = (int)_Buff.Size;
            var tbuff = _Buff.Handle;
            if (_thread is object)
                return false;
            if (!internalOpenFile())
                return false;
            FILE_NOTIFY_INFORMATION fn;
            fn.ptr = _Buff;
            _thread = new System.Threading.Thread(() =>
            {
                var notice = IntPtr.Zero;
                PInvoke.PostMessage(Handle, FileSystemMonitor.WM_SIGNAL_OPEN, IntPtr.Zero, IntPtr.Zero);
                do
                {
                    try
                    {
                        // let's clean up the memory before the next execute.
                        if (blen > 0)
                        {
                            _Buff.ZeroMemory(0L, blen);
                            blen = 0;
                        }

                        if (!FileSystemMonitor.ReadDirectoryChangesW(_hFile, tbuff, bufflen, true, _Filter, ref blen, IntPtr.Zero, IntPtr.Zero))
                        {
                            notice = (IntPtr)PInvoke.GetLastError();
                            break;
                        }
                    }
                    catch (System.Threading.ThreadAbortException ex)
                    {
                        break;
                    }
                    catch (Exception ex2)
                    {
                        notice = (IntPtr)1;
                        break;
                    }

                    // block until the lock is acquired.  Hopefully the
                    // UI thread will not take that long to clean the list.
                    System.Threading.Monitor.Enter(_WaitList);
                    _WaitList.Add(FSMonitorEventArgs.FromPtr(fn, this));
                    // and we're done ...
                    System.Threading.Monitor.Exit(_WaitList);

                    // post to the UI thread that there are items to dequeue and continue!
                    PInvoke.PostMessage(Handle, FileSystemMonitor.WM_SIGNAL, IntPtr.Zero, IntPtr.Zero);
                }
                while (true);
                _thread = null;
                PInvoke.PostMessage(Handle, FileSystemMonitor.WM_SIGNAL_CLOSE, IntPtr.Zero, IntPtr.Zero);
            });
            _thread.SetApartmentState(System.Threading.ApartmentState.STA);
            _thread.IsBackground = true;
            if (_thread.IsAlive == false)
                _thread.Start();
            return true;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Internal message pump handler and event dispatcher.
        /// </summary>
        /// <param name="m"></param>
        /// <remarks></remarks>
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case FileSystemMonitor.WM_SIGNAL:
                    {
                        // ' don't block on the main thread, block on the watching thread, instead.
                        if (System.Threading.Monitor.TryEnter(_WaitList))
                        {
                            int c;
                            int i;

                            // ' there are items waiting to be dequeued, let's dequeue one.
                            i = _lastIndex;
                            c = _WaitList.Count - 1;

                            // ' make sure we're not jumping ahead of a previous cleaning.
                            if (c >= i)
                            {
                                if (_WaitList[i] is object)
                                {

                                    // ' post the events so that whatever is watching this folder can do its thing.

                                    _WaitList[i]._Info = (FileNotifyInfo)_WaitList[i]._Info.Clone();
                                    WatchNotifyChange?.Invoke(this, _WaitList[i]);

                                    // ' remove the item from its slot in the queue, thereby
                                    // ' eliminating any chance the same event will be fired, again.
                                    _WaitList[i] = null;
                                }

                                // ' post a message to the queue cleaner.  if there are more files, it will send the pump back this way.
                                _lastIndex = i + 1;
                                PInvoke.PostMessage(Handle, FileSystemMonitor.WM_SIGNAL_CLEAN, IntPtr.Zero, IntPtr.Zero);
                            }

                            System.Threading.Monitor.Exit(_WaitList);
                        }
                        // ' going too fast?  we'll get there, eventually.  At least we know they're queuing.
                        else if (_WaitList.Count > 0)
                            PInvoke.PostMessage(Handle, FileSystemMonitor.WM_SIGNAL, IntPtr.Zero, IntPtr.Zero);
                        break;
                    }

                case FileSystemMonitor.WM_SIGNAL_CLEAN:
                    {

                        // don't block on the main thread, block on the watching thread, instead.
                        if (System.Threading.Monitor.TryEnter(_WaitList))
                        {

                            // we have a lock, let's clean up the queue
                            int i;
                            int c = _WaitList.Count - 1;
                            for (i = c; i >= 0; i -= 1)
                            {
                                // ' we want to only remove slots that have been dequeued.
                                if (_WaitList[i] is null)
                                {
                                    _WaitList.RemoveAt(i);
                                }
                            }

                            // ' reset the lastindex to 0, indicating that any items still in the queue have not fired, yet.
                            _lastIndex = 0;
                            System.Threading.Monitor.Exit(_WaitList);

                            // ' if we still have more signals in the queue, tell the message pump to keep on truckin'.
                            if (_WaitList.Count > 0)
                                PInvoke.PostMessage(Handle, FileSystemMonitor.WM_SIGNAL, IntPtr.Zero, IntPtr.Zero);
                        }
                        else
                        {
                            // oh snap!  can't lock it, let's send another clean message to make sure we do finally execute, eventually.
                            PInvoke.PostMessage(Handle, FileSystemMonitor.WM_SIGNAL_CLEAN, IntPtr.Zero, IntPtr.Zero);
                        }

                        break;
                    }

                case FileSystemMonitor.WM_SIGNAL_OPEN:
                    {
                        _isWatching = true;
                        MonitorOpened?.Invoke(this, new EventArgs());
                        break;
                    }

                case FileSystemMonitor.WM_SIGNAL_CLOSE:
                    {
                        _isWatching = false;
                        if (m.LParam.ToInt32() >= 1)
                        {
                            MonitorClosed?.Invoke(this, new MonitorClosedEventArgs(MonitorClosedState.ClosedOnError, m.LParam.ToInt32(), NativeErrorMethods.FormatLastError((uint)m.LParam.ToInt32())));
                        }
                        else
                        {
                            MonitorClosed?.Invoke(this, new MonitorClosedEventArgs(MonitorClosedState.Closed));
                        }

                        break;
                    }

                default:
                    {
                        base.WndProc(ref m);
                        break;
                    }
            }
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Returns true if the monitor has been disposed.
        /// If it has been disposed, it may not be reused.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool Disposed
        {
            get
            {
                return disposedValue;
            }
        }

        private bool disposedValue; // To detect redundant calls
        private bool shuttingDown;

        /// <summary>
        /// Dispose of the managed and unmanaged resources.
        /// </summary>
        /// <param name="disposing"></param>
        /// <remarks></remarks>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                shuttingDown = true;
                if (_isWatching)
                    StopWatching();
                _lastIndex = 0;
                _WaitList = null;
                _WaitLock = 0;

                // ' destroy the window handle
                DestroyHandle();

                // ' free the buffer
                _Buff.Free(true);

                // ' release the String handle
                _stor = null;
            }

            disposedValue = true;
            shuttingDown = false;
        }

        ~FSMonitor()
        {
            Dispose(false);
        }

        /// <summary>
        /// Deactivate the monitor, destroy the window handle and dispose of any managed or unmanaged resources.
        /// </summary>
        /// <remarks></remarks>
        public void Dispose()
        {
            // Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    }
}