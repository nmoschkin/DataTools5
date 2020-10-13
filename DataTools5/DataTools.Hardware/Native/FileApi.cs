// ************************************************* ''
// DataTools C# Native Utility Library For Windows - Interop
//
// Module: FileApi
//         Native File Services.
// 
// Copyright (C) 2011-2020 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the Microsoft Public License   
// ************************************************* ''

using System;
using System.Runtime.InteropServices;
using DataTools.Memory;
using Microsoft.VisualBasic;
using Microsoft.VisualStudio.OLE.Interop;

namespace DataTools.Hardware.Native
{
    internal static class FileApi
    {

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public const int WAIT_FAILED = unchecked((int)0xFFFFFFFF);
        public const int WAIT_OBJECT_0 = Async.STATUS_WAIT_0 + 0;
        public const int WAIT_ABANDONED = Async.STATUS_ABANDONED_WAIT_0 + 0;
        public const int WAIT_ABANDONED_0 = Async.STATUS_ABANDONED_WAIT_0 + 0;
        public const int WAIT_IO_COMPLETION = Async.STATUS_USER_APC;

        [DllImport("kernel32", EntryPoint = "RtlSecureZeroMemory", CharSet = CharSet.Unicode)]
        public static extern IntPtr SecureZeroMemory(IntPtr ptr, int cnt);
        [DllImport("kernel32", EntryPoint = "RtlCaptureStackBacktrace", CharSet = CharSet.Unicode)]

        public static extern ushort CaptureStackBacktrace(uint FramesToskip, uint FramesToCapture, IntPtr BackTrace, ref uint BackTraceHash);
        //
        // File creation flags must start at the high end since they
        // are combined with the attributes
        //

        //
        //  These are flags supported through CreateFile (W7) and CreateFile2 (W8 and beyond)
        //

        public const int FILE_FLAG_WRITE_THROUGH = unchecked((int)0x80000000);
        public const int FILE_FLAG_OVERLAPPED = 0x40000000;
        public const int FILE_FLAG_NO_BUFFERING = 0x20000000;
        public const int FILE_FLAG_RANDOM_ACCESS = 0x10000000;
        public const int FILE_FLAG_SEQUENTIAL_SCAN = 0x8000000;
        public const int FILE_FLAG_DELETE_ON_CLOSE = 0x4000000;
        public const int FILE_FLAG_BACKUP_SEMANTICS = 0x2000000;
        public const int FILE_FLAG_POSIX_SEMANTICS = 0x1000000;
        public const int FILE_FLAG_SESSION_AWARE = 0x800000;
        public const int FILE_FLAG_OPEN_REPARSE_POINT = 0x200000;
        public const int FILE_FLAG_OPEN_NO_RECALL = 0x100000;
        public const int FILE_FLAG_FIRST_PIPE_INSTANCE = 0x80000;

        // (_WIN32_WINNT >= _WIN32_WINNT_WIN8) Then

        //
        //  These are flags supported only through CreateFile2 (W8 and beyond)
        //
        //  Due to the multiplexing of file creation flags, file attribute flags and
        //  security QoS flags into a single DWORD (dwFlagsAndAttributes) parameter for
        //  CreateFile, there is no way to add any more flags to CreateFile. Additional
        //  flags for the create operation must be added to CreateFile2 only
        //

        public const int FILE_FLAG_OPEN_REQUIRING_OPLOCK = 0x40000;

        //
        // (_WIN32_WINNT >= &H0400)
        //
        // Define possible return codes from the CopyFileEx callback routine
        //

        public const int PROGRESS_CONTINUE = 0;
        public const int PROGRESS_CANCEL = 1;
        public const int PROGRESS_STOP = 2;
        public const int PROGRESS_QUIET = 3;

        //
        // Define CopyFileEx callback routine state change values
        //

        public const int CALLBACK_CHUNK_FINISHED = 0x0;
        public const int CALLBACK_STREAM_SWITCH = 0x1;

        //
        // Define CopyFileEx option flags
        //

        public const int COPY_FILE_FAIL_IF_EXISTS = 0x1;
        public const int COPY_FILE_RESTARTABLE = 0x2;
        public const int COPY_FILE_OPEN_SOURCE_FOR_WRITE = 0x4;
        public const int COPY_FILE_ALLOW_DECRYPTED_DESTINATION = 0x8;

        //
        //  Gap for private copyfile flags
        //

        //  (_WIN32_WINNT >= &H0600)
        public const int COPY_FILE_COPY_SYMLINK = 0x800;
        public const int COPY_FILE_NO_BUFFERING = 0x1000;
        //

        // (_WIN32_WINNT >= _WIN32_WINNT_WIN8) Then

        //
        //  CopyFile2 flags
        //

        public const int COPY_FILE_REQUEST_SECURITY_PRIVILEGES = 0x2000;
        public const int COPY_FILE_RESUME_FROM_PAUSE = 0x4000;
        public const int COPY_FILE_NO_OFFLOAD = 0x40000;

        //

        //  /* _WIN32_WINNT >= &H0400 */

        //  (_WIN32_WINNT >= &H0500)
        //
        // Define ReplaceFile option flags
        //

        public const int REPLACEFILE_WRITE_THROUGH = 0x1;
        public const int REPLACEFILE_IGNORE_MERGE_ERRORS = 0x2;

        //  (_WIN32_WINNT >= &H0600)
        public const int REPLACEFILE_IGNORE_ACL_ERRORS = 0x4;
        //

        //  '' ''  (_WIN32_WINNT >= &H0500)

        //
        // Define the NamedPipe definitions
        //

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        //
        // Define the dwOpenMode values for CreateNamedPipe
        //

        public const int PIPE_ACCESS_INBOUND = 0x1;
        public const int PIPE_ACCESS_OUTBOUND = 0x2;
        public const int PIPE_ACCESS_DUPLEX = 0x3;

        //
        // Define the Named Pipe End flags for GetNamedPipeInfo
        //

        public const int PIPE_CLIENT_END = 0x0;
        public const int PIPE_SERVER_END = 0x1;

        //
        // Define the dwPipeMode values for CreateNamedPipe
        //

        public const int PIPE_WAIT = 0x0;
        public const int PIPE_NOWAIT = 0x1;
        public const int PIPE_READMODE_BYTE = 0x0;
        public const int PIPE_READMODE_MESSAGE = 0x2;
        public const int PIPE_TYPE_BYTE = 0x0;
        public const int PIPE_TYPE_MESSAGE = 0x4;
        public const int PIPE_ACCEPT_REMOTE_CLIENTS = 0x0;
        public const int PIPE_REJECT_REMOTE_CLIENTS = 0x8;

        //
        // Define the well known values for CreateNamedPipe nMaxInstances
        //

        public const int PIPE_UNLIMITED_INSTANCES = 255;

        //
        // Define the Security Quality of Service bits to be passed
        // into CreateFile
        //

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        public const int FILE_BEGIN = 0;
        public const int FILE_CURRENT = 1;
        public const int FILE_END = 2;

        /// <summary>
        /// Move methods for SetFilePointer and SetFilePointerEx
        /// </summary>
        /// <remarks></remarks>
        public enum FilePointerMoveMethod : uint
        {
            /// <summary>
            /// Sets the position relative to the beginning of the file.
            /// If this method is selected, then offset must be a positive number.
            /// </summary>
            /// <remarks></remarks>
            Begin = FILE_BEGIN,

            /// <summary>
            /// Sets the position relative to the current position of the file.
            /// </summary>
            /// <remarks></remarks>
            Current = FILE_CURRENT,

            /// <summary>
            /// Sets the position relative to the end of the file.
            /// </summary>
            /// <remarks></remarks>
            End = FILE_END
        }

        //''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        //                                                                    ''
        //                             ACCESS TYPES                           ''
        //                                                                    ''
        //''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        // begin_wdm
        //
        //  The following are masks for the predefined standard access types
        //

        public const int DELETE = 0x10000;
        public const int READ_CONTROL = 0x20000;
        public const int WRITE_DAC = 0x40000;
        public const int WRITE_OWNER = 0x80000;
        public const int SYNCHRONIZE = 0x100000;
        public const int STANDARD_RIGHTS_REQUIRED = 0xF0000;
        public const int STANDARD_RIGHTS_READ = READ_CONTROL;
        public const int STANDARD_RIGHTS_WRITE = READ_CONTROL;
        public const int STANDARD_RIGHTS_EXECUTE = READ_CONTROL;
        public const int STANDARD_RIGHTS_ALL = 0x1F0000;
        public const int SPECIFIC_RIGHTS_ALL = 0xFFFF;

        //
        // AccessSystemAcl access type
        //

        public const int ACCESS_SYSTEM_SECURITY = 0x1000000;

        //
        // MaximumAllowed access type
        //

        public const int MAXIMUM_ALLOWED = 0x2000000;

        //
        //  These are the generic rights.
        //

        public const int GENERIC_READ = unchecked((int) 0x80000000);
        public const int GENERIC_WRITE = 0x40000000;
        public const int GENERIC_EXECUTE = 0x20000000;
        public const int GENERIC_ALL = 0x10000000;

        //
        //  Define the generic mapping array.  This is used to denote the
        //  mapping of each generic access right to a specific access mask.
        //
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct GENERIC_MAPPING
        {
            public uint GenericRead;
            public uint GenericWrite;
            public uint GenericExecute;
            public uint GenericAll;
        }

        public const int FILE_READ_DATA = 0x1;    // file & pipe
        public const int FILE_LIST_DIRECTORY = 0x1;    // directory
        public const int FILE_WRITE_DATA = 0x2;    // file & pipe
        public const int FILE_ADD_FILE = 0x2;    // directory
        public const int FILE_APPEND_DATA = 0x4;    // file
        public const int FILE_ADD_SUBDIRECTORY = 0x4;    // directory
        public const int FILE_CREATE_PIPE_INSTANCE = 0x4;    // named pipe
        public const int FILE_READ_EA = 0x8;    // file & directory
        public const int FILE_WRITE_EA = 0x10;    // file & directory
        public const int FILE_EXECUTE = 0x20;    // file
        public const int FILE_TRAVERSE = 0x20;    // directory
        public const int FILE_DELETE_CHILD = 0x40;    // directory
        public const int FILE_READ_ATTRIBUTES = 0x80;    // all
        public const int FILE_WRITE_ATTRIBUTES = 0x100;    // all
        public const int FILE_ALL_ACCESS = STANDARD_RIGHTS_REQUIRED | SYNCHRONIZE | 0x1FF;
        public const int FILE_GENERIC_READ = STANDARD_RIGHTS_READ | FILE_READ_DATA | FILE_READ_ATTRIBUTES | FILE_READ_EA | SYNCHRONIZE;
        public const int FILE_GENERIC_WRITE = STANDARD_RIGHTS_WRITE | FILE_WRITE_DATA | FILE_WRITE_ATTRIBUTES | FILE_WRITE_EA | FILE_APPEND_DATA | SYNCHRONIZE;
        public const int FILE_GENERIC_EXECUTE = STANDARD_RIGHTS_EXECUTE | FILE_READ_ATTRIBUTES | FILE_EXECUTE | SYNCHRONIZE;
        public const int FILE_SHARE_READ = 0x1;
        public const int FILE_SHARE_WRITE = 0x2;
        public const int FILE_SHARE_DELETE = 0x4;

        // Public Enum FileAttributes
        // [ReadOnly] = &H1
        // Hidden = &H2
        // System = &H4
        // Directory = &H10
        // Archive = &H20
        // Device = &H40
        // Normal = &H80
        // Temporary = &H100
        // SparseFile = &H200
        // ReparsePoint = &H400
        // Compressed = &H800
        // Offline = &H1000
        // NotContentIndexed = &H2000
        // Encrypted = &H4000
        // IntegrityStream = &H8000
        // Virtual = &H10000
        // NoScrubData = &H20000
        // End Enum

        public const int FILE_ATTRIBUTE_READONLY = 0x1;
        public const int FILE_ATTRIBUTE_HIDDEN = 0x2;
        public const int FILE_ATTRIBUTE_SYSTEM = 0x4;
        public const int FILE_ATTRIBUTE_DIRECTORY = 0x10;
        public const int FILE_ATTRIBUTE_ARCHIVE = 0x20;
        public const int FILE_ATTRIBUTE_DEVICE = 0x40;
        public const int FILE_ATTRIBUTE_NORMAL = 0x80;
        public const int FILE_ATTRIBUTE_TEMPORARY = 0x100;
        public const int FILE_ATTRIBUTE_SPARSE_FILE = 0x200;
        public const int FILE_ATTRIBUTE_REPARSE_POINT = 0x400;
        public const int FILE_ATTRIBUTE_COMPRESSED = 0x800;
        public const int FILE_ATTRIBUTE_OFFLINE = 0x1000;
        public const int FILE_ATTRIBUTE_NOT_CONTENT_INDEXED = 0x2000;
        public const int FILE_ATTRIBUTE_ENCRYPTED = 0x4000;
        public const int FILE_ATTRIBUTE_INTEGRITY_STREAM = 0x8000;
        public const int FILE_ATTRIBUTE_VIRTUAL = 0x10000;
        public const int FILE_ATTRIBUTE_NO_SCRUB_DATA = 0x20000;
        public const int FILE_NOTIFY_CHANGE_FILE_NAME = 0x1;
        public const int FILE_NOTIFY_CHANGE_DIR_NAME = 0x2;
        public const int FILE_NOTIFY_CHANGE_ATTRIBUTES = 0x4;
        public const int FILE_NOTIFY_CHANGE_SIZE = 0x8;
        public const int FILE_NOTIFY_CHANGE_LAST_WRITE = 0x10;
        public const int FILE_NOTIFY_CHANGE_LAST_ACCESS = 0x20;
        public const int FILE_NOTIFY_CHANGE_CREATION = 0x40;
        public const int FILE_NOTIFY_CHANGE_SECURITY = 0x100;
        public const int FILE_ACTION_ADDED = 0x1;
        public const int FILE_ACTION_REMOVED = 0x2;
        public const int FILE_ACTION_MODIFIED = 0x3;
        public const int FILE_ACTION_RENAMED_OLD_NAME = 0x4;
        public const int FILE_ACTION_RENAMED_NEW_NAME = 0x5;
        public const int MAILSLOT_NO_MESSAGE = -1;
        public const int MAILSLOT_WAIT_FOREVER = -1;
        public const int FILE_CASE_SENSITIVE_SEARCH = 0x1;
        public const int FILE_CASE_PRESERVED_NAMES = 0x2;
        public const int FILE_UNICODE_ON_DISK = 0x4;
        public const int FILE_PERSISTENT_ACLS = 0x8;
        public const int FILE_FILE_COMPRESSION = 0x10;
        public const int FILE_VOLUME_QUOTAS = 0x20;
        public const int FILE_SUPPORTS_SPARSE_FILES = 0x40;
        public const int FILE_SUPPORTS_REPARSE_POINTS = 0x80;
        public const int FILE_SUPPORTS_REMOTE_STORAGE = 0x100;
        public const int FILE_VOLUME_IS_COMPRESSED = 0x8000;
        public const int FILE_SUPPORTS_OBJECT_IDS = 0x10000;
        public const int FILE_SUPPORTS_ENCRYPTION = 0x20000;
        public const int FILE_NAMED_STREAMS = 0x40000;
        public const int FILE_READ_ONLY_VOLUME = 0x80000;
        public const int FILE_SEQUENTIAL_WRITE_ONCE = 0x100000;
        public const int FILE_SUPPORTS_TRANSACTIONS = 0x200000;
        public const int FILE_SUPPORTS_HARD_LINKS = 0x400000;
        public const int FILE_SUPPORTS_EXTENDED_ATTRIBUTES = 0x800000;
        public const int FILE_SUPPORTS_OPEN_BY_FILE_ID = 0x1000000;
        public const int FILE_SUPPORTS_USN_JOURNAL = 0x2000000;
        public const int FILE_SUPPORTS_INTEGRITY_STREAMS = 0x4000000;
        public const long FILE_INVALID_FILE_ID = -1L;

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        // begin_1_0
        // begin_2_0
        // begin_2_1
        // /********************************************************************************
        // *                                                                               *
        // * FileApi.h -- ApiSet Contract for api-ms-win-core-file-l1                      *
        // *                                                                               *
        // * Copyright (c) Microsoft Corporation. All rights reserved.                     *
        // *                                                                               *
        // ********************************************************************************/

        //
        // Constants
        //

        public const int MAX_PATH = 260;
        public const int CREATE_NEW = 1;
        public const int CREATE_ALWAYS = 2;
        public const int OPEN_EXISTING = 3;
        public const int OPEN_ALWAYS = 4;
        public const int TRUNCATE_EXISTING = 5;

        public enum CreateDisposition
        {
            CreateNew = 1,
            CreateAlways = 2,
            OpenExisting = 3,
            OpenAlways = 4,
            TruncateExisting = 5
        }

        public const int INVALID_FILE_SIZE = unchecked((int)0xFFFFFFFF);
        public const int INVALID_SET_FILE_POINTER = -1;
        public const int INVALID_FILE_ATTRIBUTES = -1;

        public enum FINDEX_INFO_LEVELS
        {
            FindExInfoStandard,
            FindExInfoMaxInfoLevel
        }

        public enum FINDEX_SEARCH_OPS
        {
            FindExSearchNameMatch,
            FindExSearchLimitToDirectories,
            FindExSearchLimitToDevices
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern int CompareFileTime(PInvoke.FILETIME lpFileTime1, PInvoke.FILETIME lpFileTime2);
        [DllImport("kernel32.dll", EntryPoint = "CreateDirectoryW", CharSet = CharSet.Unicode)]

        public static extern bool CreateDirectory([MarshalAs(UnmanagedType.LPWStr)] string lpPathName, SecurityDescriptor.SECURITY_ATTRIBUTES lpSecurityAttributes);
        [DllImport("kernel32.dll", EntryPoint = "CreateFileW", CharSet = CharSet.Unicode)]

        public static extern IntPtr CreateFile([MarshalAs(UnmanagedType.LPWStr)] string lpFileName, int dwDesiredAccess, int dwShareMode, SecurityDescriptor.SECURITY_ATTRIBUTES lpSecurityAttributes, int dwCreationDisposition, int dwFlagsAndAttributes, IntPtr hTemplateFile);
        [DllImport("kernel32.dll", EntryPoint = "CreateFileW", CharSet = CharSet.Unicode)]

        public static extern IntPtr CreateFile([MarshalAs(UnmanagedType.LPWStr)] string lpFileName, int dwDesiredAccess, int dwShareMode, IntPtr lpSecurityAttributes, int dwCreationDisposition, int dwFlagsAndAttributes, IntPtr hTemplateFile);

        /// <summary>
        /// If this value is specified along with DDD_REMOVE_DEFINITION, the function will use an exact match to determine which mapping to remove. Use this value to ensure that you do not delete something that you did not define.
        /// </summary>
        /// <remarks></remarks>
        public const int DDD_EXACT_MATCH_ON_REMOVE = 4;

        /// <summary>
        /// Do not broadcast the WM_SETTINGCHANGE message. By default, this message is broadcast to notify the shell and applications of the change.
        /// </summary>
        /// <remarks></remarks>
        public const int DDD_NO_BROADCAST_SYSTEM = 8;

        /// <summary>
        /// Uses the lpTargetPath string as is. Otherwise, it is converted from an MS-DOS path to a path.
        /// </summary>
        /// <remarks></remarks>
        public const int DDD_RAW_TARGET_PATH = 1;

        /// <summary>
        /// Removes the specified definition for the specified device. To determine which definition to remove, the function walks the list of mappings for the device, looking for a match of lpTargetPath against a prefix of each mapping associated with this device. The first mapping that matches is the one removed, and then the function returns.
        /// If lpTargetPath is NULL or a pointer to a NULL string, the function will remove the first mapping associated with the device and pop the most recent one pushed. If there is nothing left to pop, the device name will be removed.
        /// If this value is not specified, the string pointed to by the lpTargetPath parameter will become the new mapping for this device.
        /// </summary>
        /// <remarks></remarks>
        public const int DDD_REMOVE_DEFINITION = 2;

        [DllImport("kernel32.dll", EntryPoint = "DefineDosDeviceW", CharSet = CharSet.Unicode)]

        public static extern bool DefineDosDevice(int dwFlags, [MarshalAs(UnmanagedType.LPWStr)] string lpDeviceName, [MarshalAs(UnmanagedType.LPWStr)] string lpTargetPath);
        [DllImport("kernel32.dll", EntryPoint = "DeleteFileW", CharSet = CharSet.Unicode)]

        public static extern bool DeleteFile([MarshalAs(UnmanagedType.LPWStr)] string lpFileName);
        [DllImport("kernel32", EntryPoint = "MoveFileW", CharSet = CharSet.Unicode)]

        public static extern int MoveFile([MarshalAs(UnmanagedType.LPWStr)] string lpExistingFilename, [MarshalAs(UnmanagedType.LPWStr)] string lpNewFilename);
        [DllImport("kernel32", EntryPoint = "CopyFileW", CharSet = CharSet.Unicode)]

        public static extern int CopyFile([MarshalAs(UnmanagedType.LPWStr)] string lpExistingFilename, [MarshalAs(UnmanagedType.LPWStr)] string lpNewFilename, int bFailIfExists);
        [DllImport("kernel32.dll", EntryPoint = "DeleteVolumeMointPointW", CharSet = CharSet.Unicode)]

        public static extern bool DeleteVolumeMointPoint([MarshalAs(UnmanagedType.LPWStr)] string lpszVolumeMointPoint);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern bool FindCloseChangeNotification(IntPtr hChangeHandle);
        [DllImport("kernel32.dll", EntryPoint = "FindFirstChangeNotificationW", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindFirstChangeNotification([MarshalAs(UnmanagedType.LPWStr)] string lpPathName, [MarshalAs(UnmanagedType.Bool)] bool bWatchSubtree, int dwNotifyFilter);
        [DllImport("kernel32.dll", EntryPoint = "FindFirstVolumeW", CharSet = CharSet.Unicode)]

        public static extern IntPtr FindFirstVolume([MarshalAs(UnmanagedType.LPWStr)] string lpszVolumeName, int cchBufferLength);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern bool FindNextChangeNotification(IntPtr hChangeHandle);
        [DllImport("kernel32", EntryPoint = "FindFirstFileW", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindFirstFile([MarshalAs(UnmanagedType.LPWStr)] string lpFilename, [MarshalAs(UnmanagedType.Struct)] ref PInvoke.WIN32_FIND_DATA lpFindFileData);
        [DllImport("kernel32", EntryPoint = "FindNextFileW", CharSet = CharSet.Unicode)]
        public static extern bool FindNextFile(IntPtr hFindFile, [MarshalAs(UnmanagedType.Struct)] ref PInvoke.WIN32_FIND_DATA lpFindFileData);
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        public static extern int FindClose(IntPtr hFindFile);
        [DllImport("kernel32", EntryPoint = "FindFirstFileExW", CharSet = CharSet.Unicode)]


        public static extern IntPtr FindFirstFileEx([MarshalAs(UnmanagedType.LPWStr)] string lpFilename, FINDEX_INFO_LEVELS fInfoLevelId, ref PInvoke.WIN32_FIND_DATA lpFindFileData, FINDEX_SEARCH_OPS fSearchOp, IntPtr lpSearchFilter, int dwAdditionalFlags);
        [DllImport("kernel32.dll", EntryPoint = "FindNextVolumeW", CharSet = CharSet.Unicode)]

        public static extern bool FindNextVolume(IntPtr hFindVolume, [MarshalAs(UnmanagedType.LPWStr)] string lpszVolumeName, int cchBufferLength);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern bool FindVolumeClose(IntPtr hFindVolume);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern bool FlushFileBuffers(IntPtr hFile);
        [DllImport("kernel32.dll", EntryPoint = "GetDiskFreeSpaceW", CharSet = CharSet.Unicode)]

        public static extern bool GetDiskFreeSpace(string lpRootPathName, ref uint lpSectorsPerCluster, ref uint lpBytesPerSector, ref uint lpNumberOfFreeClusters, ref uint lpTotalNumberOfClusters);
        [DllImport("kernel32.dll", EntryPoint = "GetDiskFreeSpaceExW", CharSet = CharSet.Unicode)]

        public static extern bool GetDiskFreeSpaceEx(string lpRootPathName, ref ulong lpFreeBytesAvailableToCaller, ref ulong lpTotalNumberOfBytes, ref ulong lpTotalNumberOfFreeBytes);
        [DllImport("kernel32.dll", EntryPoint = "GetDiskFreeSpaceW", CharSet = CharSet.Unicode)]

        public static extern bool GetDiskFreeSpace(string lpRootPathName, ref int lpSectorsPerCluster, ref int lpBytesPerSector, ref int lpNumberOfFreeClusters, ref int lpTotalNumberofClusters);
        [DllImport("kernel32.dll", EntryPoint = "GetDriveTypeW", CharSet = CharSet.Unicode)]

        public static extern uint GetDriveType([MarshalAs(UnmanagedType.LPWStr)] string lpRootPathName);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct BY_HANDLE_FILE_INFORMATION
        {
            public int dwFileAttributesInteger;
            [MarshalAs(UnmanagedType.Struct)]
            public PInvoke.FILETIME ftCreationTimeInteger;
            [MarshalAs(UnmanagedType.Struct)]
            public PInvoke.FILETIME ftLastAccessTimeInteger;
            [MarshalAs(UnmanagedType.Struct)]
            public PInvoke.FILETIME ftLastWriteTimeInteger;
            public int dwVolumeSerialNumberInteger;
            public int nFileSizeHighInteger;
            public int nFileSizeLowInteger;
            public int nNumberOfLinksInteger;
            public int nFileIndexHighInteger;
            public int nFileIndexLowInteger;
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern bool GetFileInformationByHandle(IntPtr hFile, BY_HANDLE_FILE_INFORMATION lpFileInformation);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern uint GetFileType(IntPtr hFile);
        [DllImport("kernel32.dll", EntryPoint = "GetFinalPathNameByHandle", CharSet = CharSet.Unicode)]

        public static extern uint GetFinalPathNameByHandle(IntPtr hFile, [MarshalAs(UnmanagedType.LPWStr)] string lpszFilePath, uint cchFilePath, uint dwFlags);
        [DllImport("kernel32", EntryPoint = "GetCurrentDirectoryW", CharSet = CharSet.Unicode)]
        public static extern int GetCurrentDirectory(int bLen, [MarshalAs(UnmanagedType.LPWStr)] ref string lpszBuffer);
        [DllImport("kernel32", EntryPoint = "GetFullPathNameW", CharSet = CharSet.Unicode)]
        public static extern int GetFullPathName([MarshalAs(UnmanagedType.LPWStr)] string lpFilename, int nBufferLength, IntPtr lpBuffer, ref IntPtr lpFilepart);
        [DllImport("kernel32.dll", EntryPoint = "GetFullPathNameW", CharSet = CharSet.Unicode)]

        public static extern uint GetFullPathName([MarshalAs(UnmanagedType.LPWStr)] string lpszFilePath, uint nBufferLength, [MarshalAs(UnmanagedType.LPWStr)] string lpBuffer, [MarshalAs(UnmanagedType.LPWStr)] ref string lpFilePart);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern uint GetLogicalDrives();
        [DllImport("kernel32.dll", EntryPoint = "GetLogicalDriveStringsW", CharSet = CharSet.Unicode)]
        public static extern uint GetLogicalDriveStrings(uint nBufferLength, [MarshalAs(UnmanagedType.LPWStr)] string lpBuffer);
        [DllImport("kernel32.dll", EntryPoint = "GetLogicalDriveStringsW", CharSet = CharSet.Unicode)]
        public static extern uint GetLogicalDriveStrings(uint nBufferLength, IntPtr lpBuffer);
        [DllImport("kernel32.dll", EntryPoint = "GetTempFileNameW", CharSet = CharSet.Unicode)]

        public static extern uint GetTempFileName([MarshalAs(UnmanagedType.LPWStr)] string lpPathName, [MarshalAs(UnmanagedType.LPWStr)] string lpPrefixString, uint uUnique, [MarshalAs(UnmanagedType.LPWStr)] string lpTempFileName);
        [DllImport("kernel32.dll", EntryPoint = "GetVolumeInformationByHandleW", CharSet = CharSet.Unicode)]

        public static extern bool GetVolumeInformationByHandle(IntPtr hFile, [MarshalAs(UnmanagedType.LPWStr)] string lpVolumeNameBuffer, uint nVolumeNameSize, ref uint lpVolumeSerialNumber, ref uint lpMaximumComponentLength, ref uint lpFileSystemFlags, [MarshalAs(UnmanagedType.LPWStr)] string lpFileSystemNameBuffer, uint nFileSystemNameSize);
        [DllImport("kernel32.dll", EntryPoint = "GetVolumePathName", CharSet = CharSet.Unicode)]

        public static extern bool GetVolumePathName([MarshalAs(UnmanagedType.LPWStr)] string lpszFileName, [MarshalAs(UnmanagedType.LPWStr)] string lpszVolumePathName, uint cchBufferLength);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern bool CancelIoEx(IntPtr hFile, OVERLAPPED lpOverlapped);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern bool CancelIoEx(IntPtr hFile, IntPtr lpOverlapped);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern bool CancelSynchronousIo(IntPtr hThread);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern bool LockFile(IntPtr hFile, uint dwFileOffsetLow, uint dwFileOffsetHigh, uint nNumberOfBytesToLockLow, uint nNumberOfBytesToLockHigh);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern bool LockFileEx(IntPtr hFile, uint dwFlags, uint dwReserved, uint nNumberOfBytesToLockLow, uint nNumberOfBytesToLockHigh, IntPtr lpOverlapped);
        [DllImport("kernel32.dll", EntryPoint = "QueryDosDeviceW", CharSet = CharSet.Unicode)]

        public static extern uint QueryDosDevice([MarshalAs(UnmanagedType.LPWStr)] string lpDeviceName, [MarshalAs(UnmanagedType.LPWStr)] string lpTargetPath, uint ucchMax);

        public delegate void OVERLAPPED_COMPLETION_ROUTINE(int dwErrorCode, int dwNumberOfBytesTransfered, OVERLAPPED lpOverlapped);

        public delegate void OVERLAPPED_COMPLETION_ROUTINE_PTR(int dwErrorCode, int dwNumberOfBytesTransfered, IntPtr lpOverlapped);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern bool ReadFile(IntPtr hFile, IntPtr lpBuffer, uint nNumberOfBytesToRead, ref uint lpNumberOfBytesRead, IntPtr lpOverlapped);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern bool ReadFileEx(IntPtr hFile, IntPtr lpBuffer, uint nNumberOfBytesToRead, ref uint lpNumberOfBytesRead, IntPtr lpOverlapped, OVERLAPPED_COMPLETION_ROUTINE lpCompletionRoutine);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern bool ReadFileEx(IntPtr hFile, IntPtr lpBuffer, uint nNumberOfBytesToRead, ref uint lpNumberOfBytesRead, IntPtr lpOverlapped, OVERLAPPED_COMPLETION_ROUTINE_PTR lpCompletionRoutine);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern bool ReadFile(IntPtr hFile, byte[] lpBuffer, uint nNumberOfBytesToRead, ref uint lpNumberOfBytesRead, IntPtr lpOverlapped);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern bool ReadFileEx(IntPtr hFile, byte[] lpBuffer, uint nNumberOfBytesToRead, ref uint lpNumberOfBytesRead, IntPtr lpOverlapped, OVERLAPPED_COMPLETION_ROUTINE lpCompletionRoutine);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern bool ReadFileEx(IntPtr hFile, byte[] lpBuffer, uint nNumberOfBytesToRead, ref uint lpNumberOfBytesRead, IntPtr lpOverlapped, OVERLAPPED_COMPLETION_ROUTINE_PTR lpCompletionRoutine);

        // #endif '' WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)
        // #pragma endregion

        // #pragma region Application Family

        // #if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP)

        [DllImport("kernel32.dll", EntryPoint = "RemoveDirectoryW", CharSet = CharSet.Unicode)]

        public static extern bool RemoveDirectory([MarshalAs(UnmanagedType.LPWStr)] string lpPathName);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern bool SetFileInformationByHandle(IntPtr hFile, object FileInformationClass, byte[] lpFileInformation, uint dwBufferSize);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern uint SetFilePointer(IntPtr hFile, int lDistanceToMove, ref int lpDistanceToMoveHigh, FilePointerMoveMethod dwMoveMethod);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern bool SetFilePointerEx(IntPtr hFile, long liDistanceToMove, ref long lpNewFilePointer, FilePointerMoveMethod dwMoveMethod);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern bool SetFileValidData(IntPtr hFile, long ValidDataLength);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern bool UnlockFile(IntPtr hFile, uint dwFileOffsetLow, uint dwFileOffsetHigh, uint nNumberOfBytesToLockLow, uint nNumberOfBytesToLockHigh);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern bool UnlockFileEx(IntPtr hFile, uint dwReserved, uint nNumberOfBytesToLockLow, uint nNumberOfBytesToLockHigh, IntPtr lpOverlapped);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern bool WriteFile(IntPtr hFile, IntPtr lpBuffer, uint nNumberOfBytesToWrite, ref uint lpNumberOfBytesWritten, IntPtr lpOverlapped);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern bool WriteFile(IntPtr hFile, byte[] lpBuffer, uint nNumberOfBytesToWrite, ref uint lpNumberOfBytesWritten, IntPtr lpOverlapped);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern bool WriteFileEx(IntPtr hFile, IntPtr lpBuffer, uint nNumberOfBytesToWrite, IntPtr lpOverlapped, OVERLAPPED_COMPLETION_ROUTINE lpCompletionRoutine);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern bool WriteFileEx(IntPtr hFile, IntPtr lpBuffer, uint nNumberOfBytesToWrite, IntPtr lpOverlapped, OVERLAPPED_COMPLETION_ROUTINE_PTR lpCompletionRoutine);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern bool WriteFileEx(IntPtr hFile, byte[] lpBuffer, uint nNumberOfBytesToWrite, IntPtr lpOverlapped, OVERLAPPED_COMPLETION_ROUTINE lpCompletionRoutine);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern bool WriteFileEx(IntPtr hFile, byte[] lpBuffer, uint nNumberOfBytesToWrite, IntPtr lpOverlapped, OVERLAPPED_COMPLETION_ROUTINE_PTR lpCompletionRoutine);
        [DllImport("kernel32.dll", EntryPoint = "GetTempPathW", CharSet = CharSet.Unicode)]

        public static extern uint GetTempPath(uint nBufferLength, string lpBuffer);
        [DllImport("kernel32.dll", EntryPoint = "GetVolumeNameForVolumeMountPointW", CharSet = CharSet.Unicode)]

        public static extern bool GetVolumeNameForVolumeMountPoint([MarshalAs(UnmanagedType.LPWStr)] string lpszVolumeMountPoint, [MarshalAs(UnmanagedType.LPWStr)] string lpszVolumeName, uint cchBufferLength);
        [DllImport("kernel32.dll", EntryPoint = "GetVolumeNameForVolumeMountPointW", CharSet = CharSet.Unicode)]

        public static extern bool GetVolumeNameForVolumeMountPoint([MarshalAs(UnmanagedType.LPWStr)] string lpszVolumeMountPoint, IntPtr lpszVolumeName, uint cchBufferLength);
        [DllImport("kernel32")]
        public static extern int FileTimeToDosDateTime(ref PInvoke.FILETIME lpFileTime, int lpFatDate, int lpFatTime);
        [DllImport("kernel32")]
        public static extern int FileTimeToLocalFileTime(ref PInvoke.FILETIME lpFileTime, ref PInvoke.FILETIME lpLocalFileTime);
        [DllImport("kernel32")]
        public static extern int FileTimeToSystemTime(ref PInvoke.FILETIME lpFileTime, ref PInvoke.SYSTEMTIME lpSystemTime);
        [DllImport("kernel32")]
        public static extern int LocalFileTimeToFileTime(ref PInvoke.FILETIME lpLocalFileTime, ref PInvoke.FILETIME lpFileTime);
        [DllImport("kernel32")]
        public static extern int SystemTimeToFileTime(ref PInvoke.SYSTEMTIME lpSystemTime, ref PInvoke.FILETIME lpFileTime);
        [DllImport("kernel32", EntryPoint = "GetFileAttributesW", CharSet = CharSet.Unicode)]
        public static extern int pGetFileAttributes([MarshalAs(UnmanagedType.LPWStr)] string lpFilename);
        [DllImport("kernel32", EntryPoint = "SetFileAttributesW", CharSet = CharSet.Unicode)]
        public static extern int pSetFileAttributes([MarshalAs(UnmanagedType.LPWStr)] string lpFilename, int dwFileAttributes);
        [DllImport("kernel32", EntryPoint = "GetFileTime")]
        public static extern int pGetFileTime(IntPtr hFile, PInvoke.FILETIME lpCreationTime, PInvoke.FILETIME lpLastAccessTime, PInvoke.FILETIME lpLastWriteTime);
        [DllImport("kernel32", EntryPoint = "SetFileTime")]
        public static extern int pSetFileTime(IntPtr hFile, PInvoke.FILETIME lpCreationTime, PInvoke.FILETIME lpLastAccessTime, PInvoke.FILETIME lpLastWriteTime);
        [DllImport("kernel32", EntryPoint = "SetFileTime")]
        public static extern int pSetFileTime2(IntPtr hFile, ref PInvoke.FILETIME lpCreationTime, ref PInvoke.FILETIME lpLastAccessTime, ref PInvoke.FILETIME lpLastWriteTime);
        [DllImport("kernel32", EntryPoint = "GetFileSize")]
        public static extern uint pGetFileSize(IntPtr hFile, ref uint lpFileSizeHigh);
        [DllImport("comdlg32.dll", EntryPoint = "GetFileTitleW", CharSet = CharSet.Unicode)]
        public static extern short pGetFileTitle([MarshalAs(UnmanagedType.LPWStr)] string lpszFile, string lpszTitle, short cbBuf);
        [DllImport("kernel32", EntryPoint = "GetFileType")]
        public static extern int pGetFileType(IntPtr hFile);
        [DllImport("version.dll", EntryPoint = "GetFileVersionInfoW", CharSet = CharSet.Unicode)]
        public static extern int GetFileVersionInfo([MarshalAs(UnmanagedType.LPWStr)] string lptstrFilename, int dwHandle, int dwLen, IntPtr lpData);
        [DllImport("version.dll", EntryPoint = "GetFileVersionInfoSizeW", CharSet = CharSet.Unicode)]
        public static extern int GetFileVersionInfoSize([MarshalAs(UnmanagedType.LPWStr)] string lptstrFilename, int lpdwHandle);

        // #ifdef UNICODE
        // #define GetVolumeNameForVolumeMountPoint  GetVolumeNameForVolumeMountPointW
        // #End If

        // #if (_WIN32_WINNT >= 0x0501)

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern bool GetVolumePathNamesForVolumeNameW([MarshalAs(UnmanagedType.LPWStr)] string lpszVolumeName, IntPtr lpszVolumePathNames, uint cchBufferLength, ref uint lpcchReturnLength);

        public static bool GetVolumePathNamesForVolumeName(string lpszVolumeName, ref string[] lpszVolumePathNames)
        {
            var sp = new MemPtr();
            uint ul = 0U;

            FileApi.GetVolumePathNamesForVolumeNameW(lpszVolumeName, IntPtr.Zero, 0U, ref ul);

            sp.Alloc((ul + 1L) * sizeof(char));

            FileApi.GetVolumePathNamesForVolumeNameW(lpszVolumeName, sp, (uint)sp.Length, ref ul);
            lpszVolumePathNames = sp.GetStringArray(0L);
            sp.Free();

            return true;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct CREATEFILE2_EXTENDED_PARAMETERS
        {
            public uint dwSize;
            public uint dwFileAttributes;
            public uint dwFileFlags;
            public uint dwSecurityQosFlags;
            public SecurityDescriptor.SECURITY_ATTRIBUTES lpSecurityAttributes;
            public IntPtr hTemplateFile;
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr CreateFile2(string lpFileName, uint dwDesiredAccess, uint dwShareMode, uint dwCreationDisposition, CREATEFILE2_EXTENDED_PARAMETERS pCreateExParams);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct OVERLAPPED
        {
            public IntPtr Internal;
            public IntPtr InternalHigh;
            public int Offset;
            public int OffsetHigh;
            public IntPtr hEvent;

            public long LongOffset
            {
                get
                {
                    return Utility.MakeLong((uint)Offset, OffsetHigh);
                }
            }
        }
    }

    internal static class IoControl
    {

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public const int FILE_DEVICE_BEEP = 0x1;
        public const int FILE_DEVICE_CD_ROM = 0x2;
        public const int FILE_DEVICE_CD_ROM_FILE_SYSTEM = 0x3;
        public const int FILE_DEVICE_CONTROLLER = 0x4;
        public const int FILE_DEVICE_DATALINK = 0x5;
        public const int FILE_DEVICE_DFS = 0x6;
        public const int FILE_DEVICE_DISK = 0x7;
        public const int FILE_DEVICE_DISK_FILE_SYSTEM = 0x8;
        public const int FILE_DEVICE_FILE_SYSTEM = 0x9;
        public const int FILE_DEVICE_INPORT_PORT = 0xA;
        public const int FILE_DEVICE_KEYBOARD = 0xB;
        public const int FILE_DEVICE_MAILSLOT = 0xC;
        public const int FILE_DEVICE_MIDI_IN = 0xD;
        public const int FILE_DEVICE_MIDI_OUT = 0xE;
        public const int FILE_DEVICE_MOUSE = 0xF;
        public const int FILE_DEVICE_MULTI_UNC_PROVIDER = 0x10;
        public const int FILE_DEVICE_NAMED_PIPE = 0x11;
        public const int FILE_DEVICE_NETWORK = 0x12;
        public const int FILE_DEVICE_NETWORK_BROWSER = 0x13;
        public const int FILE_DEVICE_NETWORK_FILE_SYSTEM = 0x14;
        public const int FILE_DEVICE_NULL = 0x15;
        public const int FILE_DEVICE_PARALLEL_PORT = 0x16;
        public const int FILE_DEVICE_PHYSICAL_NETCARD = 0x17;
        public const int FILE_DEVICE_PRINTER = 0x18;
        public const int FILE_DEVICE_SCANNER = 0x19;
        public const int FILE_DEVICE_SERIAL_MOUSE_PORT = 0x1A;
        public const int FILE_DEVICE_SERIAL_PORT = 0x1B;
        public const int FILE_DEVICE_SCREEN = 0x1C;
        public const int FILE_DEVICE_SOUND = 0x1D;
        public const int FILE_DEVICE_STREAMS = 0x1E;
        public const int FILE_DEVICE_TAPE = 0x1F;
        public const int FILE_DEVICE_TAPE_FILE_SYSTEM = 0x20;
        public const int FILE_DEVICE_TRANSPORT = 0x21;
        public const int FILE_DEVICE_UNKNOWN = 0x22;
        public const int FILE_DEVICE_VIDEO = 0x23;
        public const int FILE_DEVICE_VIRTUAL_DISK = 0x24;
        public const int FILE_DEVICE_WAVE_IN = 0x25;
        public const int FILE_DEVICE_WAVE_OUT = 0x26;
        public const int FILE_DEVICE_8042_PORT = 0x27;
        public const int FILE_DEVICE_NETWORK_REDIRECTOR = 0x28;
        public const int FILE_DEVICE_BATTERY = 0x29;
        public const int FILE_DEVICE_BUS_EXTENDER = 0x2A;
        public const int FILE_DEVICE_MODEM = 0x2B;
        public const int FILE_DEVICE_VDM = 0x2C;
        public const int FILE_DEVICE_MASS_STORAGE = 0x2D;
        public const int FILE_DEVICE_SMB = 0x2E;
        public const int FILE_DEVICE_KS = 0x2F;
        public const int FILE_DEVICE_CHANGER = 0x30;
        public const int FILE_DEVICE_SMARTCARD = 0x31;
        public const int FILE_DEVICE_ACPI = 0x32;
        public const int FILE_DEVICE_DVD = 0x33;
        public const int FILE_DEVICE_FULLSCREEN_VIDEO = 0x34;
        public const int FILE_DEVICE_DFS_FILE_SYSTEM = 0x35;
        public const int FILE_DEVICE_DFS_VOLUME = 0x36;
        public const int FILE_DEVICE_SERENUM = 0x37;
        public const int FILE_DEVICE_TERMSRV = 0x38;
        public const int FILE_DEVICE_KSEC = 0x39;
        public const int FILE_DEVICE_FIPS = 0x3A;
        public const int FILE_DEVICE_INFINIBAND = 0x3B;
        public const int FILE_DEVICE_VMBUS = 0x3E;
        public const int FILE_DEVICE_CRYPT_PROVIDER = 0x3F;
        public const int FILE_DEVICE_WPD = 0x40;
        public const int FILE_DEVICE_BLUETOOTH = 0x41;
        public const int FILE_DEVICE_MT_COMPOSITE = 0x42;
        public const int FILE_DEVICE_MT_TRANSPORT = 0x43;
        public const int FILE_DEVICE_BIOMETRIC = 0x44;
        public const int FILE_DEVICE_PMI = 0x45;
        public const int FILE_DEVICE_EHSTOR = 0x46;
        public const int FILE_DEVICE_DEVAPI = 0x47;
        public const int FILE_DEVICE_GPIO = 0x48;
        public const int FILE_DEVICE_USBEX = 0x49;
        public const int FILE_DEVICE_CONSOLE = 0x50;
        public const int FILE_DEVICE_NFP = 0x51;
        public const int FILE_DEVICE_SYSENV = 0x52;
        public const int FILE_DEVICE_VIRTUAL_BLOCK = 0x53;
        public const int FILE_DEVICE_POINT_OF_SERVICE = 0x54;
        public const int FILE_DEVICE_AVIO = 0x99;

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        //
        // The following is a list of the native file system fsctls followed by
        // additional network file system fsctls.  Some values have been
        // decommissioned.
        //

        public readonly static DiskApi.CTL_CODE FSCTL_REQUEST_OPLOCK_LEVEL_1 = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 0U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);
        public readonly static DiskApi.CTL_CODE FSCTL_REQUEST_OPLOCK_LEVEL_2 = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 1U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);
        public readonly static DiskApi.CTL_CODE FSCTL_REQUEST_BATCH_OPLOCK = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 2U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);
        public readonly static DiskApi.CTL_CODE FSCTL_OPLOCK_BREAK_ACKNOWLEDGE = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 3U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);
        public readonly static DiskApi.CTL_CODE FSCTL_OPBATCH_ACK_CLOSE_PENDING = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 4U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);
        public readonly static DiskApi.CTL_CODE FSCTL_OPLOCK_BREAK_NOTIFY = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 5U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);
        public readonly static DiskApi.CTL_CODE FSCTL_LOCK_VOLUME = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 6U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);
        public readonly static DiskApi.CTL_CODE FSCTL_UNLOCK_VOLUME = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 7U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);
        public readonly static DiskApi.CTL_CODE FSCTL_DISMOUNT_VOLUME = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 8U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);

        // decommissioned fsctl value                                              9
        public readonly static DiskApi.CTL_CODE FSCTL_IS_VOLUME_MOUNTED = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 10U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);
        public readonly static DiskApi.CTL_CODE FSCTL_IS_PATHNAME_VALID = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 11U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS); // PATHNAME_BUFFER,
        public readonly static DiskApi.CTL_CODE FSCTL_MARK_VOLUME_DIRTY = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 12U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);

        // decommissioned fsctl value                                             13
        public readonly static DiskApi.CTL_CODE FSCTL_QUERY_RETRIEVAL_POINTERS = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 14U, DiskApi.METHOD_NEITHER, DiskApi.FILE_ANY_ACCESS);
        public readonly static DiskApi.CTL_CODE FSCTL_GET_COMPRESSION = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 15U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);
        public readonly static DiskApi.CTL_CODE FSCTL_SET_COMPRESSION = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 16U, DiskApi.METHOD_BUFFERED, FileApi.FILE_READ_DATA | FileApi.FILE_WRITE_DATA);

        // decommissioned fsctl value                                             17
        // decommissioned fsctl value                                             18
        public readonly static DiskApi.CTL_CODE FSCTL_SET_BOOTLOADER_ACCESSED = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 19U, DiskApi.METHOD_NEITHER, DiskApi.FILE_ANY_ACCESS);
        public static DiskApi.CTL_CODE FSCTL_MARK_AS_SYSTEM_HIVE = FSCTL_SET_BOOTLOADER_ACCESSED;
        public readonly static DiskApi.CTL_CODE FSCTL_OPLOCK_BREAK_ACK_NO_2 = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 20U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);
        public readonly static DiskApi.CTL_CODE FSCTL_INVALIDATE_VOLUMES = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 21U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);
        public readonly static DiskApi.CTL_CODE FSCTL_QUERY_FAT_BPB = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 22U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS); // FSCTL_QUERY_FAT_BPB_BUFFER
        public readonly static DiskApi.CTL_CODE FSCTL_REQUEST_FILTER_OPLOCK = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 23U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);
        public readonly static DiskApi.CTL_CODE FSCTL_FILESYSTEM_GET_STATISTICS = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 24U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS); // FILESYSTEM_STATISTICS

        // if  (_WIN32_WINNT >= _WIN32_WINNT_NT4)
        public readonly static DiskApi.CTL_CODE FSCTL_GET_NTFS_VOLUME_DATA = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 25U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS); // NTFS_VOLUME_DATA_BUFFER
        public readonly static DiskApi.CTL_CODE FSCTL_GET_NTFS_FILE_RECORD = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 26U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS); // NTFS_FILE_RECORD_INPUT_BUFFER, NTFS_FILE_RECORD_OUTPUT_BUFFER
        public readonly static DiskApi.CTL_CODE FSCTL_GET_VOLUME_BITMAP = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 27U, DiskApi.METHOD_NEITHER, DiskApi.FILE_ANY_ACCESS); // STARTING_LCN_INPUT_BUFFER, VOLUME_BITMAP_BUFFER
        public readonly static DiskApi.CTL_CODE FSCTL_GET_RETRIEVAL_POINTERS = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 28U, DiskApi.METHOD_NEITHER, DiskApi.FILE_ANY_ACCESS); // STARTING_VCN_INPUT_BUFFER, RETRIEVAL_POINTERS_BUFFER
        public readonly static DiskApi.CTL_CODE FSCTL_MOVE_FILE = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 29U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_SPECIAL_ACCESS); // MOVE_FILE_DATA,
        public readonly static DiskApi.CTL_CODE FSCTL_IS_VOLUME_DIRTY = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 30U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);

        // decommissioned fsctl value                                             31
        public readonly static DiskApi.CTL_CODE FSCTL_ALLOW_EXTENDED_DASD_IO = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 32U, DiskApi.METHOD_NEITHER, DiskApi.FILE_ANY_ACCESS);

        // endif  /* _WIN32_WINNT >= _WIN32_WINNT_NT4 */

        // if  (_WIN32_WINNT >= _WIN32_WINNT_WIN2K)
        // decommissioned fsctl value                                             33
        // decommissioned fsctl value                                             34
        public readonly static DiskApi.CTL_CODE FSCTL_FIND_FILES_BY_SID = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 35U, DiskApi.METHOD_NEITHER, DiskApi.FILE_ANY_ACCESS);

        // decommissioned fsctl value                                             36
        // decommissioned fsctl value                                             37
        public readonly static DiskApi.CTL_CODE FSCTL_SET_OBJECT_ID = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 38U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_SPECIAL_ACCESS); // FILE_OBJECTID_BUFFER
        public readonly static DiskApi.CTL_CODE FSCTL_GET_OBJECT_ID = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 39U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS); // FILE_OBJECTID_BUFFER
        public readonly static DiskApi.CTL_CODE FSCTL_DELETE_OBJECT_ID = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 40U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_SPECIAL_ACCESS);
        public readonly static DiskApi.CTL_CODE FSCTL_SET_REPARSE_POINT = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 41U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_SPECIAL_ACCESS); // REPARSE_DATA_BUFFER,
        public readonly static DiskApi.CTL_CODE FSCTL_GET_REPARSE_POINT = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 42U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS); // REPARSE_DATA_BUFFER
        public readonly static DiskApi.CTL_CODE FSCTL_DELETE_REPARSE_POINT = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 43U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_SPECIAL_ACCESS); // REPARSE_DATA_BUFFER,
        public readonly static DiskApi.CTL_CODE FSCTL_ENUM_USN_DATA = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 44U, DiskApi.METHOD_NEITHER, DiskApi.FILE_ANY_ACCESS); // MFT_ENUM_DATA,
        public readonly static DiskApi.CTL_CODE FSCTL_SECURITY_ID_CHECK = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 45U, DiskApi.METHOD_NEITHER, FileApi.FILE_READ_DATA);  // BULK_SECURITY_TEST_DATA,
        public readonly static DiskApi.CTL_CODE FSCTL_READ_USN_JOURNAL = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 46U, DiskApi.METHOD_NEITHER, DiskApi.FILE_ANY_ACCESS); // READ_USN_JOURNAL_DATA, USN
        public readonly static DiskApi.CTL_CODE FSCTL_SET_OBJECT_ID_EXTENDED = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 47U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_SPECIAL_ACCESS);
        public readonly static DiskApi.CTL_CODE FSCTL_CREATE_OR_GET_OBJECT_ID = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 48U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS); // FILE_OBJECTID_BUFFER
        public readonly static DiskApi.CTL_CODE FSCTL_SET_SPARSE = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 49U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_SPECIAL_ACCESS);
        public readonly static DiskApi.CTL_CODE FSCTL_SET_ZERO_DATA = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 50U, DiskApi.METHOD_BUFFERED, FileApi.FILE_WRITE_DATA); // FILE_ZERO_DATA_INFORMATION,
        public readonly static DiskApi.CTL_CODE FSCTL_QUERY_ALLOCATED_RANGES = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 51U, DiskApi.METHOD_NEITHER, FileApi.FILE_READ_DATA);  // FILE_ALLOCATED_RANGE_BUFFER, FILE_ALLOCATED_RANGE_BUFFER
        public readonly static DiskApi.CTL_CODE FSCTL_ENABLE_UPGRADE = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 52U, DiskApi.METHOD_BUFFERED, FileApi.FILE_WRITE_DATA);
        public readonly static DiskApi.CTL_CODE FSCTL_SET_ENCRYPTION = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 53U, DiskApi.METHOD_NEITHER, DiskApi.FILE_ANY_ACCESS); // ENCRYPTION_BUFFER, DECRYPTION_STATUS_BUFFER
        public readonly static DiskApi.CTL_CODE FSCTL_ENCRYPTION_FSCTL_IO = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 54U, DiskApi.METHOD_NEITHER, DiskApi.FILE_ANY_ACCESS);
        public readonly static DiskApi.CTL_CODE FSCTL_WRITE_RAW_ENCRYPTED = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 55U, DiskApi.METHOD_NEITHER, DiskApi.FILE_SPECIAL_ACCESS); // ENCRYPTED_DATA_INFO, EXTENDED_ENCRYPTED_DATA_INFO
        public readonly static DiskApi.CTL_CODE FSCTL_READ_RAW_ENCRYPTED = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 56U, DiskApi.METHOD_NEITHER, DiskApi.FILE_SPECIAL_ACCESS); // REQUEST_RAW_ENCRYPTED_DATA, ENCRYPTED_DATA_INFO, EXTENDED_ENCRYPTED_DATA_INFO
        public readonly static DiskApi.CTL_CODE FSCTL_CREATE_USN_JOURNAL = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 57U, DiskApi.METHOD_NEITHER, DiskApi.FILE_ANY_ACCESS); // CREATE_USN_JOURNAL_DATA,
        public readonly static DiskApi.CTL_CODE FSCTL_READ_FILE_USN_DATA = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 58U, DiskApi.METHOD_NEITHER, DiskApi.FILE_ANY_ACCESS); // Read the Usn Record for a file
        public readonly static DiskApi.CTL_CODE FSCTL_WRITE_USN_CLOSE_RECORD = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 59U, DiskApi.METHOD_NEITHER, DiskApi.FILE_ANY_ACCESS); // Generate Close Usn Record
        public readonly static DiskApi.CTL_CODE FSCTL_EXTEND_VOLUME = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 60U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);
        public readonly static DiskApi.CTL_CODE FSCTL_QUERY_USN_JOURNAL = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 61U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);
        public readonly static DiskApi.CTL_CODE FSCTL_DELETE_USN_JOURNAL = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 62U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);
        public readonly static DiskApi.CTL_CODE FSCTL_MARK_HANDLE = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 63U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);
        public readonly static DiskApi.CTL_CODE FSCTL_SIS_COPYFILE = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 64U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);
        public readonly static DiskApi.CTL_CODE FSCTL_SIS_LINK_FILES = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 65U, DiskApi.METHOD_BUFFERED, FileApi.FILE_READ_DATA | FileApi.FILE_WRITE_DATA);

        // decommissional fsctl value                                             66
        // decommissioned fsctl value                                             67
        // decommissioned fsctl value                                             68
        public readonly static DiskApi.CTL_CODE FSCTL_RECALL_FILE = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 69U, DiskApi.METHOD_NEITHER, DiskApi.FILE_ANY_ACCESS);

        // decommissioned fsctl value                                             70
        public readonly static DiskApi.CTL_CODE FSCTL_READ_FROM_PLEX = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 71U, DiskApi.METHOD_OUT_DIRECT, FileApi.FILE_READ_DATA);
        public readonly static DiskApi.CTL_CODE FSCTL_FILE_PREFETCH = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 72U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_SPECIAL_ACCESS); // FILE_PREFETCH

        // endif  /* _WIN32_WINNT >= _WIN32_WINNT_WIN2K */

        // if  (_WIN32_WINNT >= _WIN32_WINNT_VISTA)
        public readonly static DiskApi.CTL_CODE FSCTL_MAKE_MEDIA_COMPATIBLE = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 76U, DiskApi.METHOD_BUFFERED, FileApi.FILE_WRITE_DATA); // UDFS R/W
        public readonly static DiskApi.CTL_CODE FSCTL_SET_DEFECT_MANAGEMENT = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 77U, DiskApi.METHOD_BUFFERED, FileApi.FILE_WRITE_DATA); // UDFS R/W
        public readonly static DiskApi.CTL_CODE FSCTL_QUERY_SPARING_INFO = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 78U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS); // UDFS R/W
        public readonly static DiskApi.CTL_CODE FSCTL_QUERY_ON_DISK_VOLUME_INFO = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 79U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS); // C/UDFS
        public readonly static DiskApi.CTL_CODE FSCTL_SET_VOLUME_COMPRESSION_STATE = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 80U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_SPECIAL_ACCESS); // VOLUME_COMPRESSION_STATE

        // decommissioned fsctl value                                                 80
        public readonly static DiskApi.CTL_CODE FSCTL_TXFS_MODIFY_RM = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 81U, DiskApi.METHOD_BUFFERED, FileApi.FILE_WRITE_DATA); // TxF
        public readonly static DiskApi.CTL_CODE FSCTL_TXFS_QUERY_RM_INFORMATION = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 82U, DiskApi.METHOD_BUFFERED, FileApi.FILE_READ_DATA);  // TxF

        // decommissioned fsctl value                                                 83
        public readonly static DiskApi.CTL_CODE FSCTL_TXFS_ROLLFORWARD_REDO = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 84U, DiskApi.METHOD_BUFFERED, FileApi.FILE_WRITE_DATA); // TxF
        public readonly static DiskApi.CTL_CODE FSCTL_TXFS_ROLLFORWARD_UNDO = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 85U, DiskApi.METHOD_BUFFERED, FileApi.FILE_WRITE_DATA); // TxF
        public readonly static DiskApi.CTL_CODE FSCTL_TXFS_START_RM = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 86U, DiskApi.METHOD_BUFFERED, FileApi.FILE_WRITE_DATA); // TxF
        public readonly static DiskApi.CTL_CODE FSCTL_TXFS_SHUTDOWN_RM = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 87U, DiskApi.METHOD_BUFFERED, FileApi.FILE_WRITE_DATA); // TxF
        public readonly static DiskApi.CTL_CODE FSCTL_TXFS_READ_BACKUP_INFORMATION = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 88U, DiskApi.METHOD_BUFFERED, FileApi.FILE_READ_DATA);  // TxF
        public readonly static DiskApi.CTL_CODE FSCTL_TXFS_WRITE_BACKUP_INFORMATION = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 89U, DiskApi.METHOD_BUFFERED, FileApi.FILE_WRITE_DATA); // TxF
        public readonly static DiskApi.CTL_CODE FSCTL_TXFS_CREATE_SECONDARY_RM = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 90U, DiskApi.METHOD_BUFFERED, FileApi.FILE_WRITE_DATA); // TxF
        public readonly static DiskApi.CTL_CODE FSCTL_TXFS_GET_METADATA_INFO = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 91U, DiskApi.METHOD_BUFFERED, FileApi.FILE_READ_DATA);  // TxF
        public readonly static DiskApi.CTL_CODE FSCTL_TXFS_GET_TRANSACTED_VERSION = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 92U, DiskApi.METHOD_BUFFERED, FileApi.FILE_READ_DATA);  // TxF

        // decommissioned fsctl value                                                 93
        public readonly static DiskApi.CTL_CODE FSCTL_TXFS_SAVEPOINT_INFORMATION = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 94U, DiskApi.METHOD_BUFFERED, FileApi.FILE_WRITE_DATA); // TxF
        public readonly static DiskApi.CTL_CODE FSCTL_TXFS_CREATE_MINIVERSION = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 95U, DiskApi.METHOD_BUFFERED, FileApi.FILE_WRITE_DATA); // TxF

        // decommissioned fsctl value                                                 96
        // decommissioned fsctl value                                                 97
        // decommissioned fsctl value                                                 98
        public readonly static DiskApi.CTL_CODE FSCTL_TXFS_TRANSACTION_ACTIVE = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 99U, DiskApi.METHOD_BUFFERED, FileApi.FILE_READ_DATA);  // TxF
        public readonly static DiskApi.CTL_CODE FSCTL_SET_ZERO_ON_DEALLOCATION = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 101U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_SPECIAL_ACCESS);
        public readonly static DiskApi.CTL_CODE FSCTL_SET_REPAIR = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 102U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);
        public readonly static DiskApi.CTL_CODE FSCTL_GET_REPAIR = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 103U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);
        public readonly static DiskApi.CTL_CODE FSCTL_WAIT_FOR_REPAIR = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 104U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);

        // decommissioned fsctl value                                                 105
        public readonly static DiskApi.CTL_CODE FSCTL_INITIATE_REPAIR = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 106U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);
        public readonly static DiskApi.CTL_CODE FSCTL_CSC_INTERNAL = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 107U, DiskApi.METHOD_NEITHER, DiskApi.FILE_ANY_ACCESS); // CSC internal implementation
        public readonly static DiskApi.CTL_CODE FSCTL_SHRINK_VOLUME = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 108U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_SPECIAL_ACCESS); // SHRINK_VOLUME_INFORMATION
        public readonly static DiskApi.CTL_CODE FSCTL_SET_SHORT_NAME_BEHAVIOR = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 109U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);
        public readonly static DiskApi.CTL_CODE FSCTL_DFSR_SET_GHOST_HANDLE_STATE = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 110U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);

        //
        //  Values 111 - 119 are reserved for FSRM.
        //

        public readonly static DiskApi.CTL_CODE FSCTL_TXFS_LIST_TRANSACTION_LOCKED_FILES = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 120U, DiskApi.METHOD_BUFFERED, FileApi.FILE_READ_DATA); // TxF
        public readonly static DiskApi.CTL_CODE FSCTL_TXFS_LIST_TRANSACTIONS = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 121U, DiskApi.METHOD_BUFFERED, FileApi.FILE_READ_DATA); // TxF
        public readonly static DiskApi.CTL_CODE FSCTL_QUERY_PAGEFILE_ENCRYPTION = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 122U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);

        // endif  /* _WIN32_WINNT >= _WIN32_WINNT_VISTA */

        // if  (_WIN32_WINNT >= _WIN32_WINNT_VISTA)
        public readonly static DiskApi.CTL_CODE FSCTL_RESET_VOLUME_ALLOCATION_HINTS = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 123U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);

        // endif  /* _WIN32_WINNT >= _WIN32_WINNT_VISTA */

        // if  (_WIN32_WINNT >= _WIN32_WINNT_WIN7)
        public readonly static DiskApi.CTL_CODE FSCTL_QUERY_DEPENDENT_VOLUME = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 124U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);    // Dependency File System Filter
        public readonly static DiskApi.CTL_CODE FSCTL_SD_GLOBAL_CHANGE = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 125U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS); // Query/Change NTFS Security Descriptors

        // endif  /* _WIN32_WINNT >= _WIN32_WINNT_WIN7 */

        // if  (_WIN32_WINNT >= _WIN32_WINNT_VISTA)
        public readonly static DiskApi.CTL_CODE FSCTL_TXFS_READ_BACKUP_INFORMATION2 = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 126U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS); // TxF

        // endif  /* _WIN32_WINNT >= _WIN32_WINNT_VISTA */

        // if  (_WIN32_WINNT >= _WIN32_WINNT_WIN7)
        public readonly static DiskApi.CTL_CODE FSCTL_LOOKUP_STREAM_FROM_CLUSTER = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 127U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);
        public readonly static DiskApi.CTL_CODE FSCTL_TXFS_WRITE_BACKUP_INFORMATION2 = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 128U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS); // TxF
        public readonly static DiskApi.CTL_CODE FSCTL_FILE_TYPE_NOTIFICATION = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 129U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);

        // endif

        // if  (_WIN32_WINNT >= _WIN32_WINNT_WIN8)
        public readonly static DiskApi.CTL_CODE FSCTL_FILE_LEVEL_TRIM = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 130U, DiskApi.METHOD_BUFFERED, FileApi.FILE_WRITE_DATA);

        // endif  /*_WIN32_WINNT >= _WIN32_WINNT_WIN8 */

        //
        //  Values 131 - 139 are reserved for FSRM.
        //

        // if  (_WIN32_WINNT >= _WIN32_WINNT_WIN7)
        public readonly static DiskApi.CTL_CODE FSCTL_GET_BOOT_AREA_INFO = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 140U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS); // BOOT_AREA_INFO
        public readonly static DiskApi.CTL_CODE FSCTL_GET_RETRIEVAL_POINTER_BASE = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 141U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS); // RETRIEVAL_POINTER_BASE
        public readonly static DiskApi.CTL_CODE FSCTL_SET_PERSISTENT_VOLUME_STATE = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 142U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);  // FILE_FS_PERSISTENT_VOLUME_INFORMATION
        public readonly static DiskApi.CTL_CODE FSCTL_QUERY_PERSISTENT_VOLUME_STATE = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 143U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);  // FILE_FS_PERSISTENT_VOLUME_INFORMATION
        public readonly static DiskApi.CTL_CODE FSCTL_REQUEST_OPLOCK = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 144U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);
        public readonly static DiskApi.CTL_CODE FSCTL_CSV_TUNNEL_REQUEST = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 145U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS); // CSV_TUNNEL_REQUEST
        public readonly static DiskApi.CTL_CODE FSCTL_IS_CSV_FILE = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 146U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS); // IS_CSV_FILE
        public readonly static DiskApi.CTL_CODE FSCTL_QUERY_FILE_SYSTEM_RECOGNITION = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 147U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS); //
        public readonly static DiskApi.CTL_CODE FSCTL_CSV_GET_VOLUME_PATH_NAME = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 148U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);
        public readonly static DiskApi.CTL_CODE FSCTL_CSV_GET_VOLUME_NAME_FOR_VOLUME_MOUNT_POINT = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 149U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);
        public readonly static DiskApi.CTL_CODE FSCTL_CSV_GET_VOLUME_PATH_NAMES_FOR_VOLUME_NAME = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 150U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);
        public readonly static DiskApi.CTL_CODE FSCTL_IS_FILE_ON_CSV_VOLUME = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 151U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);

        // endif  /* _WIN32_WINNT >= _WIN32_WINNT_WIN7 */

        // if  (_WIN32_WINNT >= _WIN32_WINNT_WIN8)
        public readonly static DiskApi.CTL_CODE FSCTL_CORRUPTION_HANDLING = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 152U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);
        public readonly static DiskApi.CTL_CODE FSCTL_OFFLOAD_READ = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 153U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_READ_ACCESS);
        public readonly static DiskApi.CTL_CODE FSCTL_OFFLOAD_WRITE = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 154U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_WRITE_ACCESS);

        // endif  /*_WIN32_WINNT >= _WIN32_WINNT_WIN8 */

        // if  (_WIN32_WINNT >= _WIN32_WINNT_WIN7)
        public readonly static DiskApi.CTL_CODE FSCTL_CSV_INTERNAL = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 155U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);

        // endif  /* _WIN32_WINNT >= _WIN32_WINNT_WIN7 */

        // if  (_WIN32_WINNT >= _WIN32_WINNT_WIN8)
        public readonly static DiskApi.CTL_CODE FSCTL_SET_PURGE_FAILURE_MODE = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 156U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);
        public readonly static DiskApi.CTL_CODE FSCTL_QUERY_FILE_LAYOUT = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 157U, DiskApi.METHOD_NEITHER, DiskApi.FILE_ANY_ACCESS);
        public readonly static DiskApi.CTL_CODE FSCTL_IS_VOLUME_OWNED_BYCSVFS = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 158U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);
        public readonly static DiskApi.CTL_CODE FSCTL_GET_INTEGRITY_INFORMATION = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 159U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);                  // FSCTL_GET_INTEGRITY_INFORMATION_BUFFER
        public readonly static DiskApi.CTL_CODE FSCTL_SET_INTEGRITY_INFORMATION = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 160U, DiskApi.METHOD_BUFFERED, FileApi.FILE_READ_DATA | FileApi.FILE_WRITE_DATA); // FSCTL_SET_INTEGRITY_INFORMATION_BUFFER
        public readonly static DiskApi.CTL_CODE FSCTL_QUERY_FILE_REGIONS = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 161U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);

        // endif  /*_WIN32_WINNT >= _WIN32_WINNT_WIN8 */

        //
        // Dedup FSCTLs
        // Values 162 - 170 are reserved for Dedup.
        //

        // if  (_WIN32_WINNT >= _WIN32_WINNT_WIN8)
        public readonly static DiskApi.CTL_CODE FSCTL_DEDUP_FILE = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 165U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);
        public readonly static DiskApi.CTL_CODE FSCTL_DEDUP_QUERY_FILE_HASHES = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 166U, DiskApi.METHOD_NEITHER, FileApi.FILE_READ_DATA);
        public readonly static DiskApi.CTL_CODE FSCTL_DEDUP_QUERY_RANGE_STATE = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 167U, DiskApi.METHOD_NEITHER, FileApi.FILE_READ_DATA);
        public readonly static DiskApi.CTL_CODE FSCTL_DEDUP_QUERY_REPARSE_INFO = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 168U, DiskApi.METHOD_NEITHER, DiskApi.FILE_ANY_ACCESS);

        // endif  /*_WIN32_WINNT >= _WIN32_WINNT_WIN8 */

        // if  (_WIN32_WINNT >= _WIN32_WINNT_WIN8)
        public readonly static DiskApi.CTL_CODE FSCTL_RKF_INTERNAL = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 171U, DiskApi.METHOD_NEITHER, DiskApi.FILE_ANY_ACCESS); // Resume Key Filter
        public readonly static DiskApi.CTL_CODE FSCTL_SCRUB_DATA = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 172U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);
        public readonly static DiskApi.CTL_CODE FSCTL_REPAIR_COPIES = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 173U, DiskApi.METHOD_BUFFERED, FileApi.FILE_READ_DATA | FileApi.FILE_WRITE_DATA);
        public readonly static DiskApi.CTL_CODE FSCTL_DISABLE_LOCAL_BUFFERING = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 174U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);
        public readonly static DiskApi.CTL_CODE FSCTL_CSV_MGMT_LOCK = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 175U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);
        public readonly static DiskApi.CTL_CODE FSCTL_CSV_QUERY_DOWN_LEVEL_FILE_SYSTEM_CHARACTERISTICS = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 176U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);
        public readonly static DiskApi.CTL_CODE FSCTL_ADVANCE_FILE_ID = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 177U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);
        public readonly static DiskApi.CTL_CODE FSCTL_CSV_SYNC_TUNNEL_REQUEST = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 178U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);
        public readonly static DiskApi.CTL_CODE FSCTL_CSV_QUERY_VETO_FILE_DIRECT_IO = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 179U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);
        public readonly static DiskApi.CTL_CODE FSCTL_WRITE_USN_REASON = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 180U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);
        public readonly static DiskApi.CTL_CODE FSCTL_CSV_CONTROL = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 181U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);
        public readonly static DiskApi.CTL_CODE FSCTL_GET_REFS_VOLUME_DATA = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 182U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);
        public readonly static DiskApi.CTL_CODE FSCTL_CSV_H_BREAKING_SYNC_TUNNEL_REQUEST = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 185U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);

        // endif  /*_WIN32_WINNT >= _WIN32_WINNT_WIN8 */

        // if  (_WIN32_WINNT >= _WIN32_WINNT_WINBLUE)
        public readonly static DiskApi.CTL_CODE FSCTL_QUERY_STORAGE_CLASSES = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 187U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);
        public readonly static DiskApi.CTL_CODE FSCTL_QUERY_REGION_INFO = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 188U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);
        public readonly static DiskApi.CTL_CODE FSCTL_USN_TRACK_MODIFIED_RANGES = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 189U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS); // USN_TRACK_MODIFIED_RANGES

        // endif  /* (_WIN32_WINNT >= _WIN32_WINNT_WINBLUE) */
        // if  (_WIN32_WINNT >= _WIN32_WINNT_WINBLUE)
        public readonly static DiskApi.CTL_CODE FSCTL_QUERY_SHARED_VIRTUAL_DISK_SUPPORT = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 192U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);
        public readonly static DiskApi.CTL_CODE FSCTL_SVHDX_SYNC_TUNNEL_REQUEST = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 193U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);
        public readonly static DiskApi.CTL_CODE FSCTL_SVHDX_SET_INITIATOR_INFORMATION = new DiskApi.CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 194U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);

        // endif  /* (_WIN32_WINNT >= _WIN32_WINNT_WINBLUE) */
        //
        // AVIO IOCTLS.
        //

        public readonly static DiskApi.CTL_CODE IOCTL_AVIO_ALLOCATE_STREAM = new DiskApi.CTL_CODE(FILE_DEVICE_AVIO, 1U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_SPECIAL_ACCESS);
        public readonly static DiskApi.CTL_CODE IOCTL_AVIO_FREE_STREAM = new DiskApi.CTL_CODE(FILE_DEVICE_AVIO, 2U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_SPECIAL_ACCESS);
        public readonly static DiskApi.CTL_CODE IOCTL_AVIO_MODIFY_STREAM = new DiskApi.CTL_CODE(FILE_DEVICE_AVIO, 3U, DiskApi.METHOD_BUFFERED, DiskApi.FILE_SPECIAL_ACCESS);

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public enum MOVE_FILE_FLAGS : uint
        {

            /// <summary>
            /// If the file is to be moved to a different volume, the function simulates the move by using the CopyFile and DeleteFile functions.
            /// If the file is successfully copied to a different volume and the original file is unable to be deleted, the function succeeds leaving the source file intact.
            /// This value cannot be used with MOVEFILE_DELAY_UNTIL_REBOOT.
            /// </summary>
            MOVEFILE_COPY_ALLOWED = 2U,

            /// <summary>
            /// Reserved for future use.
            /// </summary>
            MOVEFILE_CREATE_HARDLINK = 16U,


            /// <summary>
            /// The system does not move the file until the operating system is restarted. The system moves the file immediately after AUTOCHK is executed, but before creating any paging files. Consequently, this parameter enables the function to delete paging files from previous startups.
            /// This value can only be used if the process is in the context of a user who belongs to the administrators group or the LocalSystem account.
            /// This value cannot be used with MOVEFILE_COPY_ALLOWED.
            /// </summary>
            MOVEFILE_DELAY_UNTIL_REBOOT = 4U,


            /// <summary>
            /// The function fails if the source file is a link source, but the file cannot be tracked after the move. This situation can occur if the destination is a volume formatted with the FAT file system.
            /// </summary>
            MOVEFILE_FAIL_IF_NOT_TRACKABLE = 32U,


            /// <summary>
            /// If a file named lpNewFileName exists, the function replaces its contents with the contents of the lpExistingFileName file.
            /// This value cannot be used if lpNewFileName or lpExistingFileName names a directory.
            /// </summary>
            MOVEFILE_REPLACE_EXISTING = 1U,


            /// <summary>
            /// The function does not return until the file has actually been moved on the disk.
            /// Setting this value guarantees that a move performed as a copy and delete operation is flushed to disk before the function returns. The flush occurs at the end of the copy operation.
            /// This value has no effect if MOVEFILE_DELAY_UNTIL_REBOOT is set.
            /// </summary>
            MOVEFILE_WRITE_THROUGH = 8U
        }


        // DWORD CALLBACK CopyProgressRoutine(
        // _In_      LARGE_INTEGER TotalFileSize,
        // _In_      LARGE_INTEGER TotalBytesTransferred,
        // _In_      LARGE_INTEGER StreamSize,
        // _In_      LARGE_INTEGER StreamBytesTransferred,
        // _In_      DWORD dwStreamNumber,
        // _In_      DWORD dwCallbackReason,
        // _In_      HANDLE hSourceFile,
        // _In_      HANDLE hDestinationFile,
        // _In_opt_  LPVOID lpData
        // );

        /// <summary>
        /// Copy Progress Callback Reason
        /// </summary>
        public enum CALLBACK_REASON : uint
        {
            /// <summary>
            /// Another part of the data file was copied.
            /// </summary>
            CALLBACK_CHUNK_FINISHED = 0U,

            /// <summary>
            /// Another stream was created and is about to be copied.
            /// This is the callback reason given when the callback routine is first invoked.
            /// </summary>
            CALLBACK_STREAM_SWITCH = 1U
        }

        public delegate uint CopyProgressRoutine(LARGE_INTEGER TotalFileSize, LARGE_INTEGER TotalBytesTrasnferred, LARGE_INTEGER StreamSize, LARGE_INTEGER StreambytesTransferred, uint dwStreamNumber, CALLBACK_REASON dwCallbackReason, IntPtr hSourceFile, IntPtr hDestinationFile, IntPtr lpData);





        // BOOL WINAPI MoveFileWithProgress(
        // _In_     LPCTSTR            lpExistingFileName,
        // _In_opt_ LPCTSTR            lpNewFileName,
        // _In_opt_ LPPROGRESS_ROUTINE lpProgressRoutine,
        // _In_opt_ LPVOID             lpData,
        // _In_     DWORD              dwFlags
        // );


        [DllImport("kernel32", EntryPoint = "MoveFileWithProgressW", CharSet = CharSet.Unicode, PreserveSig = true)]
        public static extern bool MoveFileWithProgress(string lpExistingFilename, string lpNewFilename, [MarshalAs(UnmanagedType.FunctionPtr)] CopyProgressRoutine lpPRogressRoutine, IntPtr lpData, MOVE_FILE_FLAGS dwFlag);

        // BOOL WINAPI CopyFileEx(
        // _In_      LPCTSTR lpExistingFileName,
        // _In_      LPCTSTR lpNewFileName,
        // _In_opt_  LPPROGRESS_ROUTINE lpProgressRoutine,
        // _In_opt_  LPVOID lpData,
        // _In_opt_  LPBOOL pbCancel,
        // _In_      DWORD dwCopyFlags
        // );


        [DllImport("kernel32", EntryPoint = "CopyFilExW", CharSet = CharSet.Unicode, PreserveSig = true)]
        public static extern bool CopyFileEx(string lpExistingFilename, string lpNewFilename, [MarshalAs(UnmanagedType.FunctionPtr)] CopyProgressRoutine lpProgressRoutine, IntPtr lpDAta, [MarshalAs(UnmanagedType.Bool)] ref bool pbCancel, uint dwCopyFlags);



        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    }
}