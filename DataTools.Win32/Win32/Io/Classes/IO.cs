// ************************************************* ''
// DataTools C# Native Utility Library For Windows - Interop
//
// Module: FileApi
//         Native File Services.
// 
// Copyright (C) 2011-2020 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the MIT License   
// ************************************************* ''

using System;
using System.Runtime.InteropServices;

using DataTools.Win32.Memory;

namespace DataTools.Win32
{
    internal static class IO
    {

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public const int WAIT_FAILED = unchecked((int)0xFFFFFFFF);
        public const int WAIT_OBJECT_0 = Async.STATUS_WAIT_0 + 0;
        public const int WAIT_ABANDONED = Async.STATUS_ABANDONED_WAIT_0 + 0;
        public const int WAIT_ABANDONED_0 = Async.STATUS_ABANDONED_WAIT_0 + 0;
        public const int WAIT_IO_COMPLETION = Async.STATUS_USER_APC;

        public const int METHOD_BUFFERED = 0;
        public const int METHOD_IN_DIRECT = 1;
        public const int METHOD_OUT_DIRECT = 2;
        public const int METHOD_NEITHER = 3;
        public const int FILE_ANY_ACCESS = 0;
        public const int FILE_SPECIAL_ACCESS = FILE_ANY_ACCESS;
        public const int FILE_READ_ACCESS = 1;    // file & pipe
        public const int FILE_WRITE_ACCESS = 2;    // file & pipe


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
        public static extern int CompareFileTime(FILETIME lpFileTime1, FILETIME lpFileTime2);
        [DllImport("kernel32.dll", EntryPoint = "CreateDirectoryW", CharSet = CharSet.Unicode)]

        public static extern bool CreateDirectory([MarshalAs(UnmanagedType.LPWStr)] string lpPathName, SECURITY_ATTRIBUTES lpSecurityAttributes);
        [DllImport("kernel32.dll", EntryPoint = "CreateFileW", CharSet = CharSet.Unicode)]

        public static extern IntPtr CreateFile([MarshalAs(UnmanagedType.LPWStr)] string lpFileName, int dwDesiredAccess, int dwShareMode, SECURITY_ATTRIBUTES lpSecurityAttributes, int dwCreationDisposition, int dwFlagsAndAttributes, IntPtr hTemplateFile);
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
        public static extern IntPtr FindFirstFile([MarshalAs(UnmanagedType.LPWStr)] string lpFilename, [MarshalAs(UnmanagedType.Struct)] ref WIN32_FIND_DATA lpFindFileData);
        [DllImport("kernel32", EntryPoint = "FindNextFileW", CharSet = CharSet.Unicode)]
        public static extern bool FindNextFile(IntPtr hFindFile, [MarshalAs(UnmanagedType.Struct)] ref WIN32_FIND_DATA lpFindFileData);
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        public static extern int FindClose(IntPtr hFindFile);
        [DllImport("kernel32", EntryPoint = "FindFirstFileExW", CharSet = CharSet.Unicode)]


        public static extern IntPtr FindFirstFileEx([MarshalAs(UnmanagedType.LPWStr)] string lpFilename, FINDEX_INFO_LEVELS fInfoLevelId, ref WIN32_FIND_DATA lpFindFileData, FINDEX_SEARCH_OPS fSearchOp, IntPtr lpSearchFilter, int dwAdditionalFlags);
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
            public FILETIME ftCreationTimeInteger;
            [MarshalAs(UnmanagedType.Struct)]
            public FILETIME ftLastAccessTimeInteger;
            [MarshalAs(UnmanagedType.Struct)]
            public FILETIME ftLastWriteTimeInteger;
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
        public static extern int FileTimeToDosDateTime(ref FILETIME lpFileTime, int lpFatDate, int lpFatTime);
        [DllImport("kernel32")]
        public static extern int FileTimeToLocalFileTime(ref FILETIME lpFileTime, ref FILETIME lpLocalFileTime);
        [DllImport("kernel32")]
        public static extern int FileTimeToSystemTime(ref FILETIME lpFileTime, ref SYSTEMTIME lpSystemTime);
        [DllImport("kernel32")]
        public static extern int LocalFileTimeToFileTime(ref FILETIME lpLocalFileTime, ref FILETIME lpFileTime);
        [DllImport("kernel32")]
        public static extern int SystemTimeToFileTime(ref SYSTEMTIME lpSystemTime, ref FILETIME lpFileTime);
        [DllImport("kernel32", EntryPoint = "GetFileAttributesW", CharSet = CharSet.Unicode)]
        public static extern int pGetFileAttributes([MarshalAs(UnmanagedType.LPWStr)] string lpFilename);
        [DllImport("kernel32", EntryPoint = "SetFileAttributesW", CharSet = CharSet.Unicode)]
        public static extern int pSetFileAttributes([MarshalAs(UnmanagedType.LPWStr)] string lpFilename, int dwFileAttributes);
        [DllImport("kernel32", EntryPoint = "GetFileTime")]
        public static extern int pGetFileTime(IntPtr hFile, FILETIME lpCreationTime, FILETIME lpLastAccessTime, FILETIME lpLastWriteTime);
        [DllImport("kernel32", EntryPoint = "SetFileTime")]
        public static extern int pSetFileTime(IntPtr hFile, FILETIME lpCreationTime, FILETIME lpLastAccessTime, FILETIME lpLastWriteTime);
        [DllImport("kernel32", EntryPoint = "SetFileTime")]
        public static extern int pSetFileTime2(IntPtr hFile, ref FILETIME lpCreationTime, ref FILETIME lpLastAccessTime, ref FILETIME lpLastWriteTime);
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

            IO.GetVolumePathNamesForVolumeNameW(lpszVolumeName, IntPtr.Zero, 0U, ref ul);

            sp.Alloc((ul + 1L) * sizeof(char));

            IO.GetVolumePathNamesForVolumeNameW(lpszVolumeName, sp, (uint)sp.Length, ref ul);
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
            public SECURITY_ATTRIBUTES lpSecurityAttributes;
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
                    return FileTools.MakeLong((uint)Offset, OffsetHigh);
                }
            }
        }

        /// <summary>
        /// The time-out interval is the default value specified by the server process in the CreateNamedPipe function.
        /// </summary>
        public const int NMPWAIT_USE_DEFAULT_WAIT = 0x00000000;
        /// <summary>
        /// The function does not return until an instance of the named pipe is available.
        /// </summary>
        public const int NMPWAIT_WAIT_FOREVER = unchecked((int)0xffffffff);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr CreateNamedPipe(
            string lpName,
            int dwOpenMode,
            int dwPipeMode,
            int nMaxInstances,
            int nOutBufferSize,
            int nInBufferSize,
            int nDefaultTimeOut,
            ref SECURITY_ATTRIBUTES lpSecurityAttributes
        );

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ConnectNamedPipe(
            IntPtr hNamedPipe,
            ref OVERLAPPED lpOverlapped
        );


        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DisconnectNamedPipe(
            IntPtr hNamedPipe
        );
        

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetNamedPipeHandleState(
          IntPtr hNamedPipe,
          ref int lpMode,
          ref int lpMaxCollectionCount,
          ref int lpCollectDataTimeout
        );


        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool PeekNamedPipe(
          IntPtr hNamedPipe,
          IntPtr lpBuffer,
          int nBufferSize,
          ref int lpBytesRead,
          ref int lpTotalBytesAvail,
          ref int lpBytesLeftThisMessage
        );


        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetNamedPipeInfo(
          IntPtr hNamedPipe,
          ref int lpFlags,
          ref int lpOutBufferSize,
          ref int lpInBufferSize,
          ref int lpMaxInstances
        );

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool WaitNamedPipe(
          string lpNamedPipeName,
          int nTimeOut
        );

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CallNamedPipe(
          string lpNamedPipeName,
          IntPtr lpInBuffer,
          int nInBufferSize,
          IntPtr lpOutBuffer,
          int nOutBufferSize,
          ref int lpBytesRead,
          int nTimeOut
        );

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern int WaitForSingleObjectEx(
          IntPtr hHandle,
          int dwMilliseconds,
          [MarshalAs(UnmanagedType.Bool)]
          bool bAlertable
        );

    }


}