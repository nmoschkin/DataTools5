'' ************************************************* ''
'' DataTools Visual Basic Utility Library - Interop
''
'' Module: FileApi
''         Native File Services.
'' 
'' Copyright (C) 2011-2020 Nathan Moschkin
'' All Rights Reserved
''
'' Licensed Under the Microsoft Public License   
'' ************************************************* ''

Imports System.Runtime.InteropServices
Imports CoreCT.Memory
Imports Microsoft.VisualStudio.OLE.Interop
Imports CoreCT.Text.TextTools

Namespace Native

    <HideModuleName>
    Friend Module FileApi

#Region "Constants"

#Region "Asynchronous"

        Public Const WAIT_FAILED = (&HFFFFFFFF)
        Public Const WAIT_OBJECT_0 = ((STATUS_WAIT_0) + 0)

        Public Const WAIT_ABANDONED = ((STATUS_ABANDONED_WAIT_0) + 0)
        Public Const WAIT_ABANDONED_0 = ((STATUS_ABANDONED_WAIT_0) + 0)

        Public Const WAIT_IO_COMPLETION = STATUS_USER_APC

        Public Declare Unicode Function SecureZeroMemory Lib "kernel32" Alias "RtlSecureZeroMemory" (ptr As IntPtr, cnt As Integer) As IntPtr

        Public Declare Unicode Function CaptureStackBacktrace _
    Lib "kernel32" _
    Alias "RtlCaptureStackBacktrace" (
                                     FramesToskip As UInteger,
                                     FramesToCapture As UInteger,
                                     BackTrace As IntPtr,
                                     ByRef BackTraceHash As UInteger) As UShort
        ''
        '' File creation flags must start at the high end since they
        '' are combined with the attributes
        ''

        ''
        ''  These are flags supported through CreateFile (W7) and CreateFile2 (W8 and beyond)
        ''

        Public Const FILE_FLAG_WRITE_THROUGH = &H80000000
        Public Const FILE_FLAG_OVERLAPPED = &H40000000
        Public Const FILE_FLAG_NO_BUFFERING = &H20000000
        Public Const FILE_FLAG_RANDOM_ACCESS = &H10000000
        Public Const FILE_FLAG_SEQUENTIAL_SCAN = &H8000000
        Public Const FILE_FLAG_DELETE_ON_CLOSE = &H4000000
        Public Const FILE_FLAG_BACKUP_SEMANTICS = &H2000000
        Public Const FILE_FLAG_POSIX_SEMANTICS = &H1000000
        Public Const FILE_FLAG_SESSION_AWARE = &H800000
        Public Const FILE_FLAG_OPEN_REPARSE_POINT = &H200000
        Public Const FILE_FLAG_OPEN_NO_RECALL = &H100000
        Public Const FILE_FLAG_FIRST_PIPE_INSTANCE = &H80000

        '' (_WIN32_WINNT >= _WIN32_WINNT_WIN8) Then

        ''
        ''  These are flags supported only through CreateFile2 (W8 and beyond)
        ''
        ''  Due to the multiplexing of file creation flags, file attribute flags and
        ''  security QoS flags into a single DWORD (dwFlagsAndAttributes) parameter for
        ''  CreateFile, there is no way to add any more flags to CreateFile. Additional
        ''  flags for the create operation must be added to CreateFile2 only
        ''

        Public Const FILE_FLAG_OPEN_REQUIRING_OPLOCK = &H40000

        ''
        '' (_WIN32_WINNT >= &H0400)
        ''
        '' Define possible return codes from the CopyFileEx callback routine
        ''

        Public Const PROGRESS_CONTINUE = 0
        Public Const PROGRESS_CANCEL = 1
        Public Const PROGRESS_STOP = 2
        Public Const PROGRESS_QUIET = 3

        ''
        '' Define CopyFileEx callback routine state change values
        ''

        Public Const CALLBACK_CHUNK_FINISHED = &H0
        Public Const CALLBACK_STREAM_SWITCH = &H1

        ''
        '' Define CopyFileEx option flags
        ''

        Public Const COPY_FILE_FAIL_IF_EXISTS = &H1
        Public Const COPY_FILE_RESTARTABLE = &H2
        Public Const COPY_FILE_OPEN_SOURCE_FOR_WRITE = &H4
        Public Const COPY_FILE_ALLOW_DECRYPTED_DESTINATION = &H8

        ''
        ''  Gap for private copyfile flags
        ''

        ''  (_WIN32_WINNT >= &H0600)
        Public Const COPY_FILE_COPY_SYMLINK = &H800
        Public Const COPY_FILE_NO_BUFFERING = &H1000
        ''

        '' (_WIN32_WINNT >= _WIN32_WINNT_WIN8) Then

        ''
        ''  CopyFile2 flags
        ''

        Public Const COPY_FILE_REQUEST_SECURITY_PRIVILEGES = &H2000
        Public Const COPY_FILE_RESUME_FROM_PAUSE = &H4000

        Public Const COPY_FILE_NO_OFFLOAD = &H40000

        ''

        ''  /* _WIN32_WINNT >= &H0400 */

        ''  (_WIN32_WINNT >= &H0500)
        ''
        '' Define ReplaceFile option flags
        ''

        Public Const REPLACEFILE_WRITE_THROUGH = &H1
        Public Const REPLACEFILE_IGNORE_MERGE_ERRORS = &H2

        ''  (_WIN32_WINNT >= &H0600)
        Public Const REPLACEFILE_IGNORE_ACL_ERRORS = &H4
        ''

        ''  '' ''  (_WIN32_WINNT >= &H0500)

        ''
        '' Define the NamedPipe definitions
        ''

#End Region

#Region "Pipes"

        ''
        '' Define the dwOpenMode values for CreateNamedPipe
        ''

        Public Const PIPE_ACCESS_INBOUND = &H1
        Public Const PIPE_ACCESS_OUTBOUND = &H2
        Public Const PIPE_ACCESS_DUPLEX = &H3

        ''
        '' Define the Named Pipe End flags for GetNamedPipeInfo
        ''

        Public Const PIPE_CLIENT_END = &H0
        Public Const PIPE_SERVER_END = &H1

        ''
        '' Define the dwPipeMode values for CreateNamedPipe
        ''

        Public Const PIPE_WAIT = &H0
        Public Const PIPE_NOWAIT = &H1
        Public Const PIPE_READMODE_BYTE = &H0
        Public Const PIPE_READMODE_MESSAGE = &H2
        Public Const PIPE_TYPE_BYTE = &H0
        Public Const PIPE_TYPE_MESSAGE = &H4
        Public Const PIPE_ACCEPT_REMOTE_CLIENTS = &H0
        Public Const PIPE_REJECT_REMOTE_CLIENTS = &H8

        ''
        '' Define the well known values for CreateNamedPipe nMaxInstances
        ''

        Public Const PIPE_UNLIMITED_INSTANCES = 255

        ''
        '' Define the Security Quality of Service bits to be passed
        '' into CreateFile
        ''

#End Region

        Public Const FILE_BEGIN = 0
        Public Const FILE_CURRENT = 1
        Public Const FILE_END = 2

        ''' <summary>
        ''' Move methods for SetFilePointer and SetFilePointerEx
        ''' </summary>
        ''' <remarks></remarks>
        Public Enum FilePointerMoveMethod As UInteger
            ''' <summary>
            ''' Sets the position relative to the beginning of the file.
            ''' If this method is selected, then offset must be a positive number.
            ''' </summary>
            ''' <remarks></remarks>
            Begin = FILE_BEGIN

            ''' <summary>
            ''' Sets the position relative to the current position of the file.
            ''' </summary>
            ''' <remarks></remarks>
            Current = FILE_CURRENT

            ''' <summary>
            ''' Sets the position relative to the end of the file.
            ''' </summary>
            ''' <remarks></remarks>
            [End] = FILE_END
        End Enum

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ''                                                                    ''
        ''                             ACCESS TYPES                           ''
        ''                                                                    ''
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        '' begin_wdm
        ''
        ''  The following are masks for the predefined standard access types
        ''

        Public Const DELETE = (&H10000I)
        Public Const READ_CONTROL = (&H20000I)
        Public Const WRITE_DAC = (&H40000I)
        Public Const WRITE_OWNER = (&H80000I)
        Public Const SYNCHRONIZE = (&H100000I)

        Public Const STANDARD_RIGHTS_REQUIRED = (&HF0000I)

        Public Const STANDARD_RIGHTS_READ = (READ_CONTROL)
        Public Const STANDARD_RIGHTS_WRITE = (READ_CONTROL)
        Public Const STANDARD_RIGHTS_EXECUTE = (READ_CONTROL)

        Public Const STANDARD_RIGHTS_ALL = (&H1F0000I)

        Public Const SPECIFIC_RIGHTS_ALL = (&HFFFFI)

        ''
        '' AccessSystemAcl access type
        ''

        Public Const ACCESS_SYSTEM_SECURITY = (&H1000000I)

        ''
        '' MaximumAllowed access type
        ''

        Public Const MAXIMUM_ALLOWED = (&H2000000I)

        ''
        ''  These are the generic rights.
        ''

        Public Const GENERIC_READ = (&H80000000I)
        Public Const GENERIC_WRITE = (&H40000000I)
        Public Const GENERIC_EXECUTE = (&H20000000I)
        Public Const GENERIC_ALL = (&H10000000I)

        ''
        ''  Define the generic mapping array.  This is used to denote the
        ''  mapping of each generic access right to a specific access mask.
        ''
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure GENERIC_MAPPING
            Public GenericRead As UInteger
            Public GenericWrite As UInteger
            Public GenericExecute As UInteger
            Public GenericAll As UInteger
        End Structure

        Public Const FILE_READ_DATA = (&H1)    '' file & pipe
        Public Const FILE_LIST_DIRECTORY = (&H1)    '' directory

        Public Const FILE_WRITE_DATA = (&H2)    '' file & pipe
        Public Const FILE_ADD_FILE = (&H2)    '' directory

        Public Const FILE_APPEND_DATA = (&H4)    '' file
        Public Const FILE_ADD_SUBDIRECTORY = (&H4)    '' directory
        Public Const FILE_CREATE_PIPE_INSTANCE = (&H4)    '' named pipe

        Public Const FILE_READ_EA = (&H8)    '' file & directory

        Public Const FILE_WRITE_EA = (&H10)    '' file & directory

        Public Const FILE_EXECUTE = (&H20)    '' file
        Public Const FILE_TRAVERSE = (&H20)    '' directory

        Public Const FILE_DELETE_CHILD = (&H40)    '' directory

        Public Const FILE_READ_ATTRIBUTES = (&H80)    '' all

        Public Const FILE_WRITE_ATTRIBUTES = (&H100)    '' all

        Public Const FILE_ALL_ACCESS = (STANDARD_RIGHTS_REQUIRED Or SYNCHRONIZE Or &H1FF)

        Public Const FILE_GENERIC_READ = (STANDARD_RIGHTS_READ Or
                                   FILE_READ_DATA Or
                                   FILE_READ_ATTRIBUTES Or
                                   FILE_READ_EA Or
                                   SYNCHRONIZE)

        Public Const FILE_GENERIC_WRITE = (STANDARD_RIGHTS_WRITE Or
                                   FILE_WRITE_DATA Or
                                   FILE_WRITE_ATTRIBUTES Or
                                   FILE_WRITE_EA Or
                                   FILE_APPEND_DATA Or
                                   SYNCHRONIZE)

        Public Const FILE_GENERIC_EXECUTE = (STANDARD_RIGHTS_EXECUTE Or
                                   FILE_READ_ATTRIBUTES Or
                                   FILE_EXECUTE Or
                                   SYNCHRONIZE)

        Public Const FILE_SHARE_READ = &H1
        Public Const FILE_SHARE_WRITE = &H2
        Public Const FILE_SHARE_DELETE = &H4

        'Public Enum FileAttributes
        '    [ReadOnly] = &H1
        '    Hidden = &H2
        '    System = &H4
        '    Directory = &H10
        '    Archive = &H20
        '    Device = &H40
        '    Normal = &H80
        '    Temporary = &H100
        '    SparseFile = &H200
        '    ReparsePoint = &H400
        '    Compressed = &H800
        '    Offline = &H1000
        '    NotContentIndexed = &H2000
        '    Encrypted = &H4000
        '    IntegrityStream = &H8000
        '    Virtual = &H10000
        '    NoScrubData = &H20000
        'End Enum

        Public Const FILE_ATTRIBUTE_READONLY = &H1
        Public Const FILE_ATTRIBUTE_HIDDEN = &H2
        Public Const FILE_ATTRIBUTE_SYSTEM = &H4
        Public Const FILE_ATTRIBUTE_DIRECTORY = &H10
        Public Const FILE_ATTRIBUTE_ARCHIVE = &H20
        Public Const FILE_ATTRIBUTE_DEVICE = &H40
        Public Const FILE_ATTRIBUTE_NORMAL = &H80
        Public Const FILE_ATTRIBUTE_TEMPORARY = &H100
        Public Const FILE_ATTRIBUTE_SPARSE_FILE = &H200
        Public Const FILE_ATTRIBUTE_REPARSE_POINT = &H400
        Public Const FILE_ATTRIBUTE_COMPRESSED = &H800
        Public Const FILE_ATTRIBUTE_OFFLINE = &H1000
        Public Const FILE_ATTRIBUTE_NOT_CONTENT_INDEXED = &H2000
        Public Const FILE_ATTRIBUTE_ENCRYPTED = &H4000
        Public Const FILE_ATTRIBUTE_INTEGRITY_STREAM = &H8000
        Public Const FILE_ATTRIBUTE_VIRTUAL = &H10000
        Public Const FILE_ATTRIBUTE_NO_SCRUB_DATA = &H20000

        Public Const FILE_NOTIFY_CHANGE_FILE_NAME = &H1
        Public Const FILE_NOTIFY_CHANGE_DIR_NAME = &H2
        Public Const FILE_NOTIFY_CHANGE_ATTRIBUTES = &H4
        Public Const FILE_NOTIFY_CHANGE_SIZE = &H8
        Public Const FILE_NOTIFY_CHANGE_LAST_WRITE = &H10
        Public Const FILE_NOTIFY_CHANGE_LAST_ACCESS = &H20
        Public Const FILE_NOTIFY_CHANGE_CREATION = &H40
        Public Const FILE_NOTIFY_CHANGE_SECURITY = &H100
        Public Const FILE_ACTION_ADDED = &H1
        Public Const FILE_ACTION_REMOVED = &H2
        Public Const FILE_ACTION_MODIFIED = &H3
        Public Const FILE_ACTION_RENAMED_OLD_NAME = &H4
        Public Const FILE_ACTION_RENAMED_NEW_NAME = &H5
        Public Const MAILSLOT_NO_MESSAGE = -1
        Public Const MAILSLOT_WAIT_FOREVER = -1
        Public Const FILE_CASE_SENSITIVE_SEARCH = &H1
        Public Const FILE_CASE_PRESERVED_NAMES = &H2
        Public Const FILE_UNICODE_ON_DISK = &H4
        Public Const FILE_PERSISTENT_ACLS = &H8
        Public Const FILE_FILE_COMPRESSION = &H10
        Public Const FILE_VOLUME_QUOTAS = &H20
        Public Const FILE_SUPPORTS_SPARSE_FILES = &H40
        Public Const FILE_SUPPORTS_REPARSE_POINTS = &H80
        Public Const FILE_SUPPORTS_REMOTE_STORAGE = &H100
        Public Const FILE_VOLUME_IS_COMPRESSED = &H8000
        Public Const FILE_SUPPORTS_OBJECT_IDS = &H10000
        Public Const FILE_SUPPORTS_ENCRYPTION = &H20000
        Public Const FILE_NAMED_STREAMS = &H40000
        Public Const FILE_READ_ONLY_VOLUME = &H80000
        Public Const FILE_SEQUENTIAL_WRITE_ONCE = &H100000
        Public Const FILE_SUPPORTS_TRANSACTIONS = &H200000
        Public Const FILE_SUPPORTS_HARD_LINKS = &H400000
        Public Const FILE_SUPPORTS_EXTENDED_ATTRIBUTES = &H800000
        Public Const FILE_SUPPORTS_OPEN_BY_FILE_ID = &H1000000
        Public Const FILE_SUPPORTS_USN_JOURNAL = &H2000000
        Public Const FILE_SUPPORTS_INTEGRITY_STREAMS = &H4000000
        Public Const FILE_INVALID_FILE_ID = -1L

#End Region

        '' begin_1_0
        '' begin_2_0
        '' begin_2_1
        '/********************************************************************************
        '*                                                                               *
        '* FileApi.h -- ApiSet Contract for api-ms-win-core-file-l1                      *
        '*                                                                               *
        '* Copyright (c) Microsoft Corporation. All rights reserved.                     *
        '*                                                                               *
        '********************************************************************************/

        ''
        '' Constants
        ''

        Public Const MAX_PATH = 260

        Public Const CREATE_NEW = 1
        Public Const CREATE_ALWAYS = 2
        Public Const OPEN_EXISTING = 3
        Public Const OPEN_ALWAYS = 4
        Public Const TRUNCATE_EXISTING = 5

        Public Enum CreateDisposition
            CreateNew = 1
            CreateAlways = 2
            OpenExisting = 3
            OpenAlways = 4
            TruncateExisting = 5
        End Enum

        Public Const INVALID_FILE_SIZE = (&HFFFFFFFF)
        Public Const INVALID_SET_FILE_POINTER = -1I
        Public Const INVALID_FILE_ATTRIBUTES = -1I

        Public Enum FINDEX_INFO_LEVELS
            FindExInfoStandard
            FindExInfoMaxInfoLevel
        End Enum

        Public Enum FINDEX_SEARCH_OPS
            FindExSearchNameMatch
            FindExSearchLimitToDirectories
            FindExSearchLimitToDevices
        End Enum

        Public Declare Unicode Function CompareFileTime Lib "kernel32.dll" _
    (lpFileTime1 As FILETIME,
     lpFileTime2 As FILETIME) As Integer

        Public Declare Unicode Function CreateDirectory Lib "kernel32.dll" _
    Alias "CreateDirectoryW" _
    (<MarshalAs(UnmanagedType.LPWStr)> lpPathName As String,
    lpSecurityAttributes As SECURITY_ATTRIBUTES
    ) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Unicode Function CreateFile Lib "kernel32.dll" _
    Alias "CreateFileW" _
    (<MarshalAs(UnmanagedType.LPWStr)> lpFileName As String,
     dwDesiredAccess As Integer,
     dwShareMode As Integer,
     lpSecurityAttributes As SECURITY_ATTRIBUTES,
     dwCreationDisposition As Integer,
     dwFlagsAndAttributes As Integer,
     hTemplateFile As IntPtr
     ) As IntPtr

        Public Declare Unicode Function CreateFile Lib "kernel32.dll" _
 Alias "CreateFileW" _
 (<MarshalAs(UnmanagedType.LPWStr)> lpFileName As String,
  dwDesiredAccess As Integer,
  dwShareMode As Integer,
  lpSecurityAttributes As IntPtr,
  dwCreationDisposition As Integer,
  dwFlagsAndAttributes As Integer,
  hTemplateFile As IntPtr
  ) As IntPtr

        ''' <summary>
        ''' If this value is specified along with DDD_REMOVE_DEFINITION, the function will use an exact match to determine which mapping to remove. Use this value to ensure that you do not delete something that you did not define.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const DDD_EXACT_MATCH_ON_REMOVE = 4

        ''' <summary>
        ''' Do not broadcast the WM_SETTINGCHANGE message. By default, this message is broadcast to notify the shell and applications of the change.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const DDD_NO_BROADCAST_SYSTEM = 8

        ''' <summary>
        ''' Uses the lpTargetPath string as is. Otherwise, it is converted from an MS-DOS path to a path.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const DDD_RAW_TARGET_PATH = 1

        ''' <summary>
        ''' Removes the specified definition for the specified device. To determine which definition to remove, the function walks the list of mappings for the device, looking for a match of lpTargetPath against a prefix of each mapping associated with this device. The first mapping that matches is the one removed, and then the function returns.
        ''' If lpTargetPath is NULL or a pointer to a NULL string, the function will remove the first mapping associated with the device and pop the most recent one pushed. If there is nothing left to pop, the device name will be removed.
        ''' If this value is not specified, the string pointed to by the lpTargetPath parameter will become the new mapping for this device.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const DDD_REMOVE_DEFINITION = 2

        Public Declare Unicode Function DefineDosDevice Lib "kernel32.dll" _
    Alias "DefineDosDeviceW" _
    (dwFlags As Integer,
    <MarshalAs(UnmanagedType.LPWStr)> lpDeviceName As String,
    <MarshalAs(UnmanagedType.LPWStr)> lpTargetPath As String
    ) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Unicode Function DeleteFile Lib "kernel32.dll" _
    Alias "DeleteFileW" _
    (<MarshalAs(UnmanagedType.LPWStr)> lpFileName As String
    ) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Unicode Function MoveFile Lib "kernel32" _
    Alias "MoveFileW" _
    (<MarshalAs(UnmanagedType.LPWStr)> lpExistingFilename As String,
     <MarshalAs(UnmanagedType.LPWStr)> lpNewFilename As String
     ) As Integer

        Public Declare Unicode Function CopyFile Lib "kernel32" _
    Alias "CopyFileW" _
    (<MarshalAs(UnmanagedType.LPWStr)> lpExistingFilename As String,
     <MarshalAs(UnmanagedType.LPWStr)> lpNewFilename As String,
     bFailIfExists As Integer
     ) As Integer

        Public Declare Unicode Function DeleteVolumeMointPoint Lib "kernel32.dll" _
    Alias "DeleteVolumeMointPointW" _
    (<MarshalAs(UnmanagedType.LPWStr)> lpszVolumeMointPoint As String
    ) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Unicode Function FindCloseChangeNotification Lib "kernel32.dll" _
    (hChangeHandle As IntPtr
    ) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Unicode Function FindFirstChangeNotification Lib "kernel32.dll" _
    Alias "FindFirstChangeNotificationW" (
        <MarshalAs(UnmanagedType.LPWStr)> lpPathName As String,
        <MarshalAs(UnmanagedType.Bool)> bWatchSubtree As Boolean,
        dwNotifyFilter As Integer
        ) As IntPtr

        Public Declare Unicode Function FindFirstVolume Lib "kernel32.dll" _
    Alias "FindFirstVolumeW" _
        (<MarshalAs(UnmanagedType.LPWStr)> lpszVolumeName As String,
        cchBufferLength As Integer
        ) As IntPtr

        Public Declare Unicode Function FindNextChangeNotification Lib "kernel32.dll" _
    (hChangeHandle As IntPtr
    ) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Unicode Function FindFirstFile Lib "kernel32" Alias "FindFirstFileW" (<MarshalAs(UnmanagedType.LPWStr)> lpFilename As String, <MarshalAs(UnmanagedType.Struct)> ByRef lpFindFileData As WIN32_FIND_DATA) As IntPtr
        Public Declare Unicode Function FindNextFile Lib "kernel32" Alias "FindNextFileW" (hFindFile As IntPtr, <MarshalAs(UnmanagedType.Struct)> ByRef lpFindFileData As WIN32_FIND_DATA) As <MarshalAs(UnmanagedType.Bool)> Boolean
        Public Declare Unicode Function FindClose Lib "kernel32" (hFindFile As IntPtr) As Integer

        Public Declare Unicode Function FindFirstFileEx _
    Lib "kernel32" _
    Alias "FindFirstFileExW" _
    (<MarshalAs(UnmanagedType.LPWStr)> lpFilename As String,
     fInfoLevelId As FINDEX_INFO_LEVELS,
     ByRef lpFindFileData As WIN32_FIND_DATA,
     fSearchOp As FINDEX_SEARCH_OPS,
     lpSearchFilter As IntPtr,
     dwAdditionalFlags As Integer) As IntPtr

        Public Declare Unicode Function FindNextVolume Lib "kernel32.dll" _
    Alias "FindNextVolumeW" _
        (hFindVolume As IntPtr,
         <MarshalAs(UnmanagedType.LPWStr)> lpszVolumeName As String,
         cchBufferLength As Integer
         ) As Boolean

        Public Declare Unicode Function FindVolumeClose Lib "kernel32.dll" _
    (hFindVolume As IntPtr
    ) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Unicode Function FlushFileBuffers Lib "kernel32.dll" _
    (hFile As IntPtr
    ) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Unicode Function GetDiskFreeSpace Lib "kernel32.dll" _
    Alias "GetDiskFreeSpaceW" _
    (lpRootPathName As String,
    ByRef lpSectorsPerCluster As UInteger,
    ByRef lpBytesPerSector As UInteger,
    ByRef lpNumberOfFreeClusters As UInteger,
    ByRef lpTotalNumberOfClusters As UInteger
    ) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Unicode Function GetDiskFreeSpaceEx Lib "kernel32.dll" _
    Alias "GetDiskFreeSpaceExW" _
    (lpRootPathName As String,
    ByRef lpFreeBytesAvailableToCaller As ULong,
    ByRef lpTotalNumberOfBytes As ULong,
    ByRef lpTotalNumberOfFreeBytes As ULong
    ) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Unicode Function GetDiskFreeSpace Lib "kernel32.dll" _
    Alias "GetDiskFreeSpaceW" _
    (lpRootPathName As String,
     ByRef lpSectorsPerCluster As Integer,
     ByRef lpBytesPerSector As Integer,
     ByRef lpNumberOfFreeClusters As Integer,
     ByRef lpTotalNumberofClusters As Integer) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Unicode Function GetDriveType Lib "kernel32.dll" _
    Alias "GetDriveTypeW" _
    (<MarshalAs(UnmanagedType.LPWStr)> lpRootPathName As String
    ) As UInteger

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure BY_HANDLE_FILE_INFORMATION
            Public dwFileAttributesInteger As Integer

            <MarshalAs(UnmanagedType.Struct)>
            Public ftCreationTimeInteger As FILETIME

            <MarshalAs(UnmanagedType.Struct)>
            Public ftLastAccessTimeInteger As FILETIME

            <MarshalAs(UnmanagedType.Struct)>
            Public ftLastWriteTimeInteger As FILETIME

            Public dwVolumeSerialNumberInteger As Integer
            Public nFileSizeHighInteger As Integer
            Public nFileSizeLowInteger As Integer
            Public nNumberOfLinksInteger As Integer
            Public nFileIndexHighInteger As Integer
            Public nFileIndexLowInteger As Integer
        End Structure

        Public Declare Unicode Function GetFileInformationByHandle Lib "kernel32.dll" _
    (hFile As IntPtr,
     lpFileInformation As BY_HANDLE_FILE_INFORMATION) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Unicode Function GetFileType Lib "kernel32.dll" _
(hFile As IntPtr) As UInteger

        Public Declare Unicode Function GetFinalPathNameByHandle Lib "kernel32.dll" _
    Alias "GetFinalPathNameByHandle" _
    (hFile As IntPtr,
      <MarshalAs(UnmanagedType.LPWStr)> lpszFilePath As String,
      cchFilePath As UInteger,
      dwFlags As UInteger
    ) As UInteger

        Public Declare Unicode Function GetCurrentDirectory Lib "kernel32" Alias "GetCurrentDirectoryW" (bLen As Integer, <MarshalAs(UnmanagedType.LPWStr)> ByRef lpszBuffer As String) As Integer

        Public Declare Unicode Function GetFullPathName Lib "kernel32" Alias "GetFullPathNameW" (
        <MarshalAs(UnmanagedType.LPWStr)> lpFilename As String,
        nBufferLength As Integer,
        lpBuffer As IntPtr,
        ByRef lpFilepart As IntPtr) As Integer

        Public Declare Unicode Function GetFullPathName Lib "kernel32.dll" _
    Alias "GetFullPathNameW" _
    (<MarshalAs(UnmanagedType.LPWStr)> lpszFilePath As String,
      nBufferLength As UInteger,
      <MarshalAs(UnmanagedType.LPWStr)> lpBuffer As String,
      <MarshalAs(UnmanagedType.LPWStr)> ByRef lpFilePart As String
    ) As UInteger

        Public Declare Unicode Function GetLogicalDrives Lib "kernel32.dll" () As UInteger

        Public Declare Unicode Function GetLogicalDriveStrings Lib "kernel32.dll" _
    Alias "GetLogicalDriveStringsW" (
    nBufferLength As UInteger,
    <MarshalAs(UnmanagedType.LPWStr)> lpBuffer As String) As UInteger

        Public Declare Unicode Function GetLogicalDriveStrings Lib "kernel32.dll" _
    Alias "GetLogicalDriveStringsW" (
    nBufferLength As UInteger,
    lpBuffer As IntPtr) As UInteger

        Public Declare Unicode Function GetTempFileName Lib "kernel32.dll" _
    Alias "GetTempFileNameW" _
    (<MarshalAs(UnmanagedType.LPWStr)> lpPathName As String,
     <MarshalAs(UnmanagedType.LPWStr)> lpPrefixString As String,
     uUnique As UInteger,
     <MarshalAs(UnmanagedType.LPWStr)> lpTempFileName As String) As UInteger

        Public Declare Unicode Function GetVolumeInformationByHandle Lib "kernel32.dll" _
    Alias "GetVolumeInformationByHandleW" _
    (hFile As IntPtr,
     <MarshalAs(UnmanagedType.LPWStr)> lpVolumeNameBuffer As String,
     nVolumeNameSize As UInteger,
     ByRef lpVolumeSerialNumber As UInteger,
     ByRef lpMaximumComponentLength As UInteger,
     ByRef lpFileSystemFlags As UInteger,
     <MarshalAs(UnmanagedType.LPWStr)> lpFileSystemNameBuffer As String,
     nFileSystemNameSize As UInteger) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Unicode Function GetVolumePathName Lib "kernel32.dll" _
    Alias "GetVolumePathName" _
    (<MarshalAs(UnmanagedType.LPWStr)> lpszFileName As String,
      <MarshalAs(UnmanagedType.LPWStr)> lpszVolumePathName As String,
      cchBufferLength As UInteger) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Unicode Function CancelIoEx Lib "kernel32.dll" (
                                                      hFile As IntPtr,
                                                      lpOverlapped As OVERLAPPED) As Boolean

        Public Declare Unicode Function CancelIoEx Lib "kernel32.dll" (
                                                      hFile As IntPtr,
                                                      lpOverlapped As IntPtr) As Boolean

        Public Declare Unicode Function CancelSynchronousIo Lib "kernel32.dll" (
                                                               hThread As IntPtr) As Boolean

        Public Declare Unicode Function LockFile Lib "kernel32.dll" (
    hFile As IntPtr,
    dwFileOffsetLow As UInteger,
    dwFileOffsetHigh As UInteger,
    nNumberOfBytesToLockLow As UInteger,
    nNumberOfBytesToLockHigh As UInteger
    ) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Unicode Function LockFileEx Lib "kernel32.dll" (
   hFile As IntPtr,
   dwFlags As UInteger,
   dwReserved As UInteger,
   nNumberOfBytesToLockLow As UInteger,
   nNumberOfBytesToLockHigh As UInteger,
   lpOverlapped As IntPtr
   ) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Unicode Function QueryDosDevice Lib "kernel32.dll" _
    Alias "QueryDosDeviceW" _
    (<MarshalAs(UnmanagedType.LPWStr)> lpDeviceName As String,
     <MarshalAs(UnmanagedType.LPWStr)> lpTargetPath As String,
     ucchMax As UInteger) As UInteger

        Public Delegate Sub OVERLAPPED_COMPLETION_ROUTINE(dwErrorCode As Integer,
                                                  dwNumberOfBytesTransfered As Integer,
                                                  lpOverlapped As OVERLAPPED)

        Public Delegate Sub OVERLAPPED_COMPLETION_ROUTINE_PTR(dwErrorCode As Integer,
                                                  dwNumberOfBytesTransfered As Integer,
                                                  lpOverlapped As IntPtr)

        Public Declare Unicode Function ReadFile Lib "kernel32.dll" _
    (hFile As IntPtr,
     lpBuffer As IntPtr,
     nNumberOfBytesToRead As UInteger,
     ByRef lpNumberOfBytesRead As UInteger,
     lpOverlapped As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Unicode Function ReadFileEx Lib "kernel32.dll" _
    (hFile As IntPtr,
     lpBuffer As IntPtr,
     nNumberOfBytesToRead As UInteger,
     ByRef lpNumberOfBytesRead As UInteger,
     lpOverlapped As IntPtr,
     lpCompletionRoutine As OVERLAPPED_COMPLETION_ROUTINE) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Unicode Function ReadFileEx Lib "kernel32.dll" _
    (hFile As IntPtr,
     lpBuffer As IntPtr,
     nNumberOfBytesToRead As UInteger,
     ByRef lpNumberOfBytesRead As UInteger,
     lpOverlapped As IntPtr,
     lpCompletionRoutine As OVERLAPPED_COMPLETION_ROUTINE_PTR) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Unicode Function ReadFile Lib "kernel32.dll" _
    (hFile As IntPtr,
     lpBuffer As Byte(),
     nNumberOfBytesToRead As UInteger,
     ByRef lpNumberOfBytesRead As UInteger,
     lpOverlapped As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Unicode Function ReadFileEx Lib "kernel32.dll" _
    (hFile As IntPtr,
     lpBuffer As Byte(),
     nNumberOfBytesToRead As UInteger,
     ByRef lpNumberOfBytesRead As UInteger,
     lpOverlapped As IntPtr,
     lpCompletionRoutine As OVERLAPPED_COMPLETION_ROUTINE) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Unicode Function ReadFileEx Lib "kernel32.dll" _
    (hFile As IntPtr,
     lpBuffer As Byte(),
     nNumberOfBytesToRead As UInteger,
     ByRef lpNumberOfBytesRead As UInteger,
     lpOverlapped As IntPtr,
     lpCompletionRoutine As OVERLAPPED_COMPLETION_ROUTINE_PTR) As <MarshalAs(UnmanagedType.Bool)> Boolean

        '#endif '' WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)
        '#pragma endregion

        '#pragma region Application Family

        '#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP)

        Public Declare Unicode Function RemoveDirectory Lib "kernel32.dll" _
    Alias "RemoveDirectoryW" _
    (<MarshalAs(UnmanagedType.LPWStr)> lpPathName As String
    ) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Unicode Function SetFileInformationByHandle Lib "kernel32.dll" _
    (hFile As IntPtr,
     FileInformationClass As Object,
     lpFileInformation As Byte(),
     dwBufferSize As UInteger
    ) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Unicode Function SetFilePointer Lib "kernel32.dll" _
    (hFile As IntPtr,
     lDistanceToMove As Integer,
     ByRef lpDistanceToMoveHigh As Integer,
     dwMoveMethod As FilePointerMoveMethod
    ) As UInteger

        Public Declare Unicode Function SetFilePointerEx Lib "kernel32.dll" _
    (hFile As IntPtr,
     liDistanceToMove As Long,
     ByRef lpNewFilePointer As Long,
     dwMoveMethod As FilePointerMoveMethod
    ) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Unicode Function SetFileValidData Lib "kernel32.dll" _
(hFile As IntPtr,
 ValidDataLength As Long) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Unicode Function UnlockFile Lib "kernel32.dll" (
    hFile As IntPtr,
    dwFileOffsetLow As UInteger,
    dwFileOffsetHigh As UInteger,
    nNumberOfBytesToLockLow As UInteger,
    nNumberOfBytesToLockHigh As UInteger
    ) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Unicode Function UnlockFileEx Lib "kernel32.dll" (
   hFile As IntPtr,
   dwReserved As UInteger,
   nNumberOfBytesToLockLow As UInteger,
   nNumberOfBytesToLockHigh As UInteger,
   lpOverlapped As IntPtr
   ) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Unicode Function WriteFile Lib "kernel32.dll" _
    (hFile As IntPtr,
     lpBuffer As IntPtr,
     nNumberOfBytesToWrite As UInteger,
     ByRef lpNumberOfBytesWritten As UInteger,
     lpOverlapped As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Unicode Function WriteFile Lib "kernel32.dll" _
    (hFile As IntPtr,
     lpBuffer() As Byte,
     nNumberOfBytesToWrite As UInteger,
     ByRef lpNumberOfBytesWritten As UInteger,
     lpOverlapped As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Unicode Function WriteFileEx Lib "kernel32.dll" _
    (hFile As IntPtr,
     lpBuffer As IntPtr,
     nNumberOfBytesToWrite As UInteger,
     lpOverlapped As IntPtr,
     lpCompletionRoutine As OVERLAPPED_COMPLETION_ROUTINE) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Unicode Function WriteFileEx Lib "kernel32.dll" _
    (hFile As IntPtr,
     lpBuffer As IntPtr,
     nNumberOfBytesToWrite As UInteger,
     lpOverlapped As IntPtr,
     lpCompletionRoutine As OVERLAPPED_COMPLETION_ROUTINE_PTR) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Unicode Function WriteFileEx Lib "kernel32.dll" _
    (hFile As IntPtr,
     lpBuffer() As Byte,
     nNumberOfBytesToWrite As UInteger,
     lpOverlapped As IntPtr,
     lpCompletionRoutine As OVERLAPPED_COMPLETION_ROUTINE) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Unicode Function WriteFileEx Lib "kernel32.dll" _
    (hFile As IntPtr,
     lpBuffer() As Byte,
     nNumberOfBytesToWrite As UInteger,
     lpOverlapped As IntPtr,
     lpCompletionRoutine As OVERLAPPED_COMPLETION_ROUTINE_PTR) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Unicode Function GetTempPath Lib "kernel32.dll" _
    Alias "GetTempPathW" _
    (nBufferLength As UInteger,
     lpBuffer As String) As UInteger

        Public Declare Unicode Function GetVolumeNameForVolumeMountPoint Lib "kernel32.dll" _
    Alias "GetVolumeNameForVolumeMountPointW" _
    (<MarshalAs(UnmanagedType.LPWStr)> lpszVolumeMountPoint As String,
     <MarshalAs(UnmanagedType.LPWStr)> lpszVolumeName As String,
     cchBufferLength As UInteger) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Unicode Function GetVolumeNameForVolumeMountPoint Lib "kernel32.dll" _
    Alias "GetVolumeNameForVolumeMountPointW" _
    (<MarshalAs(UnmanagedType.LPWStr)> lpszVolumeMountPoint As String,
     lpszVolumeName As IntPtr,
     cchBufferLength As UInteger) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Function FileTimeToDosDateTime Lib "kernel32" (ByRef lpFileTime As FILETIME, lpFatDate As Integer, lpFatTime As Integer) As Integer
        Public Declare Function FileTimeToLocalFileTime Lib "kernel32" (ByRef lpFileTime As FILETIME, ByRef lpLocalFileTime As FILETIME) As Integer
        Public Declare Function FileTimeToSystemTime Lib "kernel32" (ByRef lpFileTime As FILETIME, ByRef lpSystemTime As SYSTEMTIME) As Integer

        Public Declare Function LocalFileTimeToFileTime Lib "kernel32" (ByRef lpLocalFileTime As FILETIME, ByRef lpFileTime As FILETIME) As Integer
        Public Declare Function SystemTimeToFileTime Lib "kernel32" (ByRef lpSystemTime As SYSTEMTIME, ByRef lpFileTime As FILETIME) As Integer

        Public Declare Unicode Function pGetFileAttributes Lib "kernel32" Alias "GetFileAttributesW" (<MarshalAs(UnmanagedType.LPWStr)> lpFilename As String) As Integer
        Public Declare Unicode Function pSetFileAttributes Lib "kernel32" Alias "SetFileAttributesW" (<MarshalAs(UnmanagedType.LPWStr)> lpFilename As String, dwFileAttributes As Integer) As Integer

        Public Declare Function pGetFileTime Lib "kernel32" Alias "GetFileTime" (hFile As IntPtr, lpCreationTime As FILETIME, lpLastAccessTime As FILETIME, lpLastWriteTime As FILETIME) As Integer
        Public Declare Function pSetFileTime Lib "kernel32" Alias "SetFileTime" (hFile As IntPtr, lpCreationTime As FILETIME, lpLastAccessTime As FILETIME, lpLastWriteTime As FILETIME) As Integer

        Public Declare Function pSetFileTime2 Lib "kernel32" Alias "SetFileTime" (hFile As IntPtr, ByRef lpCreationTime As FILETIME, ByRef lpLastAccessTime As FILETIME, ByRef lpLastWriteTime As FILETIME) As Integer

        Public Declare Function pGetFileSize Lib "kernel32" Alias "GetFileSize" (hFile As IntPtr, ByRef lpFileSizeHigh As UInteger) As UInteger
        Public Declare Unicode Function pGetFileTitle Lib "comdlg32.dll" Alias "GetFileTitleW" (<MarshalAs(UnmanagedType.LPWStr)> lpszFile As String, lpszTitle As String, cbBuf As Short) As Short
        Public Declare Function pGetFileType Lib "kernel32" Alias "GetFileType" (hFile As IntPtr) As Integer

        Public Declare Unicode Function GetFileVersionInfo Lib "version.dll" Alias "GetFileVersionInfoW" (<MarshalAs(UnmanagedType.LPWStr)> lptstrFilename As String, dwHandle As Integer, dwLen As Integer, lpData As IntPtr) As Integer
        Public Declare Unicode Function GetFileVersionInfoSize Lib "version.dll" Alias "GetFileVersionInfoSizeW" (<MarshalAs(UnmanagedType.LPWStr)> lptstrFilename As String, lpdwHandle As Integer) As Integer

        '#ifdef UNICODE
        '#define GetVolumeNameForVolumeMountPoint  GetVolumeNameForVolumeMountPointW
        '#End If

        '#if (_WIN32_WINNT >= 0x0501)

        Public Declare Unicode Function GetVolumePathNamesForVolumeNameW Lib "kernel32.dll" _
    (<MarshalAs(UnmanagedType.LPWStr)> lpszVolumeName As String,
     lpszVolumePathNames As IntPtr,
     cchBufferLength As UInteger,
     ByRef lpcchReturnLength As UInteger) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Function GetVolumePathNamesForVolumeName(lpszVolumeName As String, ByRef lpszVolumePathNames() As String) As Boolean
            Dim sp As New MemPtr
            Dim ul As UInteger = 0

            GetVolumePathNamesForVolumeNameW(lpszVolumeName, IntPtr.Zero, 0, ul)

            sp.Alloc((ul + 1) * Len("A"c))
            GetVolumePathNamesForVolumeNameW(lpszVolumeName, sp, CUInt(sp.Length()), ul)

            lpszVolumePathNames = sp.GetStringArray(0)

            sp.Free()

            Return True
        End Function

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure CREATEFILE2_EXTENDED_PARAMETERS
            Public dwSize As UInteger
            Public dwFileAttributes As UInteger
            Public dwFileFlags As UInteger
            Public dwSecurityQosFlags As UInteger
            Public lpSecurityAttributes As SECURITY_ATTRIBUTES
            Public hTemplateFile As IntPtr
        End Structure

        Public Declare Unicode Function CreateFile2 Lib "kernel32.dll" _
    (lpFileName As String,
     dwDesiredAccess As UInteger,
     dwShareMode As UInteger,
     dwCreationDisposition As UInteger,
     pCreateExParams As CREATEFILE2_EXTENDED_PARAMETERS) As IntPtr

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure OVERLAPPED
            Public Internal As IntPtr
            Public InternalHigh As IntPtr
            Public Offset As Integer
            Public OffsetHigh As Integer
            Public hEvent As IntPtr

            Public ReadOnly Property LongOffset As Long
                Get
                    Return MakeLong(CUInt(Offset), OffsetHigh)
                End Get
            End Property

        End Structure

    End Module

    Friend Module IoControl

#Region "File Device Constants"

        Public Const FILE_DEVICE_BEEP = &H1
        Public Const FILE_DEVICE_CD_ROM = &H2
        Public Const FILE_DEVICE_CD_ROM_FILE_SYSTEM = &H3
        Public Const FILE_DEVICE_CONTROLLER = &H4
        Public Const FILE_DEVICE_DATALINK = &H5
        Public Const FILE_DEVICE_DFS = &H6
        Public Const FILE_DEVICE_DISK = &H7
        Public Const FILE_DEVICE_DISK_FILE_SYSTEM = &H8
        Public Const FILE_DEVICE_FILE_SYSTEM = &H9
        Public Const FILE_DEVICE_INPORT_PORT = &HA
        Public Const FILE_DEVICE_KEYBOARD = &HB
        Public Const FILE_DEVICE_MAILSLOT = &HC
        Public Const FILE_DEVICE_MIDI_IN = &HD
        Public Const FILE_DEVICE_MIDI_OUT = &HE
        Public Const FILE_DEVICE_MOUSE = &HF
        Public Const FILE_DEVICE_MULTI_UNC_PROVIDER = &H10
        Public Const FILE_DEVICE_NAMED_PIPE = &H11
        Public Const FILE_DEVICE_NETWORK = &H12
        Public Const FILE_DEVICE_NETWORK_BROWSER = &H13
        Public Const FILE_DEVICE_NETWORK_FILE_SYSTEM = &H14
        Public Const FILE_DEVICE_NULL = &H15
        Public Const FILE_DEVICE_PARALLEL_PORT = &H16
        Public Const FILE_DEVICE_PHYSICAL_NETCARD = &H17
        Public Const FILE_DEVICE_PRINTER = &H18
        Public Const FILE_DEVICE_SCANNER = &H19
        Public Const FILE_DEVICE_SERIAL_MOUSE_PORT = &H1A
        Public Const FILE_DEVICE_SERIAL_PORT = &H1B
        Public Const FILE_DEVICE_SCREEN = &H1C
        Public Const FILE_DEVICE_SOUND = &H1D
        Public Const FILE_DEVICE_STREAMS = &H1E
        Public Const FILE_DEVICE_TAPE = &H1F
        Public Const FILE_DEVICE_TAPE_FILE_SYSTEM = &H20
        Public Const FILE_DEVICE_TRANSPORT = &H21
        Public Const FILE_DEVICE_UNKNOWN = &H22
        Public Const FILE_DEVICE_VIDEO = &H23
        Public Const FILE_DEVICE_VIRTUAL_DISK = &H24
        Public Const FILE_DEVICE_WAVE_IN = &H25
        Public Const FILE_DEVICE_WAVE_OUT = &H26
        Public Const FILE_DEVICE_8042_PORT = &H27
        Public Const FILE_DEVICE_NETWORK_REDIRECTOR = &H28
        Public Const FILE_DEVICE_BATTERY = &H29
        Public Const FILE_DEVICE_BUS_EXTENDER = &H2A
        Public Const FILE_DEVICE_MODEM = &H2B
        Public Const FILE_DEVICE_VDM = &H2C
        Public Const FILE_DEVICE_MASS_STORAGE = &H2D
        Public Const FILE_DEVICE_SMB = &H2E
        Public Const FILE_DEVICE_KS = &H2F
        Public Const FILE_DEVICE_CHANGER = &H30
        Public Const FILE_DEVICE_SMARTCARD = &H31
        Public Const FILE_DEVICE_ACPI = &H32
        Public Const FILE_DEVICE_DVD = &H33
        Public Const FILE_DEVICE_FULLSCREEN_VIDEO = &H34
        Public Const FILE_DEVICE_DFS_FILE_SYSTEM = &H35
        Public Const FILE_DEVICE_DFS_VOLUME = &H36
        Public Const FILE_DEVICE_SERENUM = &H37
        Public Const FILE_DEVICE_TERMSRV = &H38
        Public Const FILE_DEVICE_KSEC = &H39
        Public Const FILE_DEVICE_FIPS = &H3A
        Public Const FILE_DEVICE_INFINIBAND = &H3B
        Public Const FILE_DEVICE_VMBUS = &H3E
        Public Const FILE_DEVICE_CRYPT_PROVIDER = &H3F
        Public Const FILE_DEVICE_WPD = &H40
        Public Const FILE_DEVICE_BLUETOOTH = &H41
        Public Const FILE_DEVICE_MT_COMPOSITE = &H42
        Public Const FILE_DEVICE_MT_TRANSPORT = &H43
        Public Const FILE_DEVICE_BIOMETRIC = &H44
        Public Const FILE_DEVICE_PMI = &H45
        Public Const FILE_DEVICE_EHSTOR = &H46
        Public Const FILE_DEVICE_DEVAPI = &H47
        Public Const FILE_DEVICE_GPIO = &H48
        Public Const FILE_DEVICE_USBEX = &H49
        Public Const FILE_DEVICE_CONSOLE = &H50
        Public Const FILE_DEVICE_NFP = &H51
        Public Const FILE_DEVICE_SYSENV = &H52
        Public Const FILE_DEVICE_VIRTUAL_BLOCK = &H53
        Public Const FILE_DEVICE_POINT_OF_SERVICE = &H54
        Public Const FILE_DEVICE_AVIO = &H99

#End Region

#Region "IO CONTROL CODES"

        ''
        '' The following is a list of the native file system fsctls followed by
        '' additional network file system fsctls.  Some values have been
        '' decommissioned.
        ''

        Public ReadOnly FSCTL_REQUEST_OPLOCK_LEVEL_1 As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 0, METHOD_BUFFERED, FILE_ANY_ACCESS)

        Public ReadOnly FSCTL_REQUEST_OPLOCK_LEVEL_2 As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 1, METHOD_BUFFERED, FILE_ANY_ACCESS)

        Public ReadOnly FSCTL_REQUEST_BATCH_OPLOCK As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 2, METHOD_BUFFERED, FILE_ANY_ACCESS)

        Public ReadOnly FSCTL_OPLOCK_BREAK_ACKNOWLEDGE As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 3, METHOD_BUFFERED, FILE_ANY_ACCESS)

        Public ReadOnly FSCTL_OPBATCH_ACK_CLOSE_PENDING As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 4, METHOD_BUFFERED, FILE_ANY_ACCESS)

        Public ReadOnly FSCTL_OPLOCK_BREAK_NOTIFY As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 5, METHOD_BUFFERED, FILE_ANY_ACCESS)

        Public ReadOnly FSCTL_LOCK_VOLUME As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 6, METHOD_BUFFERED, FILE_ANY_ACCESS)

        Public ReadOnly FSCTL_UNLOCK_VOLUME As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 7, METHOD_BUFFERED, FILE_ANY_ACCESS)

        Public ReadOnly FSCTL_DISMOUNT_VOLUME As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 8, METHOD_BUFFERED, FILE_ANY_ACCESS)

        '' decommissioned fsctl value                                              9
        Public ReadOnly FSCTL_IS_VOLUME_MOUNTED As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 10, METHOD_BUFFERED, FILE_ANY_ACCESS)

        Public ReadOnly FSCTL_IS_PATHNAME_VALID As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 11, METHOD_BUFFERED, FILE_ANY_ACCESS) '' PATHNAME_BUFFER,

        Public ReadOnly FSCTL_MARK_VOLUME_DIRTY As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 12, METHOD_BUFFERED, FILE_ANY_ACCESS)

        '' decommissioned fsctl value                                             13
        Public ReadOnly FSCTL_QUERY_RETRIEVAL_POINTERS As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 14, METHOD_NEITHER, FILE_ANY_ACCESS)

        Public ReadOnly FSCTL_GET_COMPRESSION As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 15, METHOD_BUFFERED, FILE_ANY_ACCESS)

        Public ReadOnly FSCTL_SET_COMPRESSION As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 16, METHOD_BUFFERED, FILE_READ_DATA Or FILE_WRITE_DATA)

        '' decommissioned fsctl value                                             17
        '' decommissioned fsctl value                                             18
        Public ReadOnly FSCTL_SET_BOOTLOADER_ACCESSED As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 19, METHOD_NEITHER, FILE_ANY_ACCESS)

        Public FSCTL_MARK_AS_SYSTEM_HIVE As CTL_CODE = FSCTL_SET_BOOTLOADER_ACCESSED
        Public ReadOnly FSCTL_OPLOCK_BREAK_ACK_NO_2 As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 20, METHOD_BUFFERED, FILE_ANY_ACCESS)

        Public ReadOnly FSCTL_INVALIDATE_VOLUMES As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 21, METHOD_BUFFERED, FILE_ANY_ACCESS)

        Public ReadOnly FSCTL_QUERY_FAT_BPB As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 22, METHOD_BUFFERED, FILE_ANY_ACCESS) '' FSCTL_QUERY_FAT_BPB_BUFFER

        Public ReadOnly FSCTL_REQUEST_FILTER_OPLOCK As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 23, METHOD_BUFFERED, FILE_ANY_ACCESS)

        Public ReadOnly FSCTL_FILESYSTEM_GET_STATISTICS As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 24, METHOD_BUFFERED, FILE_ANY_ACCESS) '' FILESYSTEM_STATISTICS

        '' if  (_WIN32_WINNT >= _WIN32_WINNT_NT4)
        Public ReadOnly FSCTL_GET_NTFS_VOLUME_DATA As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 25, METHOD_BUFFERED, FILE_ANY_ACCESS) '' NTFS_VOLUME_DATA_BUFFER

        Public ReadOnly FSCTL_GET_NTFS_FILE_RECORD As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 26, METHOD_BUFFERED, FILE_ANY_ACCESS) '' NTFS_FILE_RECORD_INPUT_BUFFER, NTFS_FILE_RECORD_OUTPUT_BUFFER

        Public ReadOnly FSCTL_GET_VOLUME_BITMAP As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 27, METHOD_NEITHER, FILE_ANY_ACCESS) '' STARTING_LCN_INPUT_BUFFER, VOLUME_BITMAP_BUFFER

        Public ReadOnly FSCTL_GET_RETRIEVAL_POINTERS As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 28, METHOD_NEITHER, FILE_ANY_ACCESS) '' STARTING_VCN_INPUT_BUFFER, RETRIEVAL_POINTERS_BUFFER

        Public ReadOnly FSCTL_MOVE_FILE As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 29, METHOD_BUFFERED, FILE_SPECIAL_ACCESS) '' MOVE_FILE_DATA,

        Public ReadOnly FSCTL_IS_VOLUME_DIRTY As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 30, METHOD_BUFFERED, FILE_ANY_ACCESS)

        '' decommissioned fsctl value                                             31
        Public ReadOnly FSCTL_ALLOW_EXTENDED_DASD_IO As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 32, METHOD_NEITHER, FILE_ANY_ACCESS)

        '' endif  /* _WIN32_WINNT >= _WIN32_WINNT_NT4 */

        '' if  (_WIN32_WINNT >= _WIN32_WINNT_WIN2K)
        '' decommissioned fsctl value                                             33
        '' decommissioned fsctl value                                             34
        Public ReadOnly FSCTL_FIND_FILES_BY_SID As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 35, METHOD_NEITHER, FILE_ANY_ACCESS)

        '' decommissioned fsctl value                                             36
        '' decommissioned fsctl value                                             37
        Public ReadOnly FSCTL_SET_OBJECT_ID As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 38, METHOD_BUFFERED, FILE_SPECIAL_ACCESS) '' FILE_OBJECTID_BUFFER

        Public ReadOnly FSCTL_GET_OBJECT_ID As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 39, METHOD_BUFFERED, FILE_ANY_ACCESS) '' FILE_OBJECTID_BUFFER

        Public ReadOnly FSCTL_DELETE_OBJECT_ID As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 40, METHOD_BUFFERED, FILE_SPECIAL_ACCESS)

        Public ReadOnly FSCTL_SET_REPARSE_POINT As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 41, METHOD_BUFFERED, FILE_SPECIAL_ACCESS) '' REPARSE_DATA_BUFFER,

        Public ReadOnly FSCTL_GET_REPARSE_POINT As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 42, METHOD_BUFFERED, FILE_ANY_ACCESS) '' REPARSE_DATA_BUFFER

        Public ReadOnly FSCTL_DELETE_REPARSE_POINT As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 43, METHOD_BUFFERED, FILE_SPECIAL_ACCESS) '' REPARSE_DATA_BUFFER,

        Public ReadOnly FSCTL_ENUM_USN_DATA As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 44, METHOD_NEITHER, FILE_ANY_ACCESS) '' MFT_ENUM_DATA,

        Public ReadOnly FSCTL_SECURITY_ID_CHECK As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 45, METHOD_NEITHER, FILE_READ_DATA)  '' BULK_SECURITY_TEST_DATA,

        Public ReadOnly FSCTL_READ_USN_JOURNAL As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 46, METHOD_NEITHER, FILE_ANY_ACCESS) '' READ_USN_JOURNAL_DATA, USN

        Public ReadOnly FSCTL_SET_OBJECT_ID_EXTENDED As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 47, METHOD_BUFFERED, FILE_SPECIAL_ACCESS)

        Public ReadOnly FSCTL_CREATE_OR_GET_OBJECT_ID As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 48, METHOD_BUFFERED, FILE_ANY_ACCESS) '' FILE_OBJECTID_BUFFER

        Public ReadOnly FSCTL_SET_SPARSE As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 49, METHOD_BUFFERED, FILE_SPECIAL_ACCESS)

        Public ReadOnly FSCTL_SET_ZERO_DATA As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 50, METHOD_BUFFERED, FILE_WRITE_DATA) '' FILE_ZERO_DATA_INFORMATION,

        Public ReadOnly FSCTL_QUERY_ALLOCATED_RANGES As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 51, METHOD_NEITHER, FILE_READ_DATA)  '' FILE_ALLOCATED_RANGE_BUFFER, FILE_ALLOCATED_RANGE_BUFFER

        Public ReadOnly FSCTL_ENABLE_UPGRADE As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 52, METHOD_BUFFERED, FILE_WRITE_DATA)

        Public ReadOnly FSCTL_SET_ENCRYPTION As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 53, METHOD_NEITHER, FILE_ANY_ACCESS) '' ENCRYPTION_BUFFER, DECRYPTION_STATUS_BUFFER

        Public ReadOnly FSCTL_ENCRYPTION_FSCTL_IO As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 54, METHOD_NEITHER, FILE_ANY_ACCESS)

        Public ReadOnly FSCTL_WRITE_RAW_ENCRYPTED As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 55, METHOD_NEITHER, FILE_SPECIAL_ACCESS) '' ENCRYPTED_DATA_INFO, EXTENDED_ENCRYPTED_DATA_INFO

        Public ReadOnly FSCTL_READ_RAW_ENCRYPTED As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 56, METHOD_NEITHER, FILE_SPECIAL_ACCESS) '' REQUEST_RAW_ENCRYPTED_DATA, ENCRYPTED_DATA_INFO, EXTENDED_ENCRYPTED_DATA_INFO

        Public ReadOnly FSCTL_CREATE_USN_JOURNAL As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 57, METHOD_NEITHER, FILE_ANY_ACCESS) '' CREATE_USN_JOURNAL_DATA,

        Public ReadOnly FSCTL_READ_FILE_USN_DATA As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 58, METHOD_NEITHER, FILE_ANY_ACCESS) '' Read the Usn Record for a file

        Public ReadOnly FSCTL_WRITE_USN_CLOSE_RECORD As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 59, METHOD_NEITHER, FILE_ANY_ACCESS) '' Generate Close Usn Record

        Public ReadOnly FSCTL_EXTEND_VOLUME As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 60, METHOD_BUFFERED, FILE_ANY_ACCESS)

        Public ReadOnly FSCTL_QUERY_USN_JOURNAL As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 61, METHOD_BUFFERED, FILE_ANY_ACCESS)

        Public ReadOnly FSCTL_DELETE_USN_JOURNAL As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 62, METHOD_BUFFERED, FILE_ANY_ACCESS)

        Public ReadOnly FSCTL_MARK_HANDLE As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 63, METHOD_BUFFERED, FILE_ANY_ACCESS)

        Public ReadOnly FSCTL_SIS_COPYFILE As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 64, METHOD_BUFFERED, FILE_ANY_ACCESS)

        Public ReadOnly FSCTL_SIS_LINK_FILES As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 65, METHOD_BUFFERED, FILE_READ_DATA Or FILE_WRITE_DATA)

        '' decommissional fsctl value                                             66
        '' decommissioned fsctl value                                             67
        '' decommissioned fsctl value                                             68
        Public ReadOnly FSCTL_RECALL_FILE As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 69, METHOD_NEITHER, FILE_ANY_ACCESS)

        '' decommissioned fsctl value                                             70
        Public ReadOnly FSCTL_READ_FROM_PLEX As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 71, METHOD_OUT_DIRECT, FILE_READ_DATA)

        Public ReadOnly FSCTL_FILE_PREFETCH As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 72, METHOD_BUFFERED, FILE_SPECIAL_ACCESS) '' FILE_PREFETCH

        '' endif  /* _WIN32_WINNT >= _WIN32_WINNT_WIN2K */

        '' if  (_WIN32_WINNT >= _WIN32_WINNT_VISTA)
        Public ReadOnly FSCTL_MAKE_MEDIA_COMPATIBLE As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 76, METHOD_BUFFERED, FILE_WRITE_DATA) '' UDFS R/W

        Public ReadOnly FSCTL_SET_DEFECT_MANAGEMENT As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 77, METHOD_BUFFERED, FILE_WRITE_DATA) '' UDFS R/W

        Public ReadOnly FSCTL_QUERY_SPARING_INFO As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 78, METHOD_BUFFERED, FILE_ANY_ACCESS) '' UDFS R/W

        Public ReadOnly FSCTL_QUERY_ON_DISK_VOLUME_INFO As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 79, METHOD_BUFFERED, FILE_ANY_ACCESS) '' C/UDFS

        Public ReadOnly FSCTL_SET_VOLUME_COMPRESSION_STATE As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 80, METHOD_BUFFERED, FILE_SPECIAL_ACCESS) '' VOLUME_COMPRESSION_STATE

        '' decommissioned fsctl value                                                 80
        Public ReadOnly FSCTL_TXFS_MODIFY_RM As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 81, METHOD_BUFFERED, FILE_WRITE_DATA) '' TxF

        Public ReadOnly FSCTL_TXFS_QUERY_RM_INFORMATION As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 82, METHOD_BUFFERED, FILE_READ_DATA)  '' TxF

        '' decommissioned fsctl value                                                 83
        Public ReadOnly FSCTL_TXFS_ROLLFORWARD_REDO As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 84, METHOD_BUFFERED, FILE_WRITE_DATA) '' TxF

        Public ReadOnly FSCTL_TXFS_ROLLFORWARD_UNDO As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 85, METHOD_BUFFERED, FILE_WRITE_DATA) '' TxF

        Public ReadOnly FSCTL_TXFS_START_RM As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 86, METHOD_BUFFERED, FILE_WRITE_DATA) '' TxF

        Public ReadOnly FSCTL_TXFS_SHUTDOWN_RM As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 87, METHOD_BUFFERED, FILE_WRITE_DATA) '' TxF

        Public ReadOnly FSCTL_TXFS_READ_BACKUP_INFORMATION As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 88, METHOD_BUFFERED, FILE_READ_DATA)  '' TxF

        Public ReadOnly FSCTL_TXFS_WRITE_BACKUP_INFORMATION As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 89, METHOD_BUFFERED, FILE_WRITE_DATA) '' TxF

        Public ReadOnly FSCTL_TXFS_CREATE_SECONDARY_RM As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 90, METHOD_BUFFERED, FILE_WRITE_DATA) '' TxF

        Public ReadOnly FSCTL_TXFS_GET_METADATA_INFO As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 91, METHOD_BUFFERED, FILE_READ_DATA)  '' TxF

        Public ReadOnly FSCTL_TXFS_GET_TRANSACTED_VERSION As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 92, METHOD_BUFFERED, FILE_READ_DATA)  '' TxF

        '' decommissioned fsctl value                                                 93
        Public ReadOnly FSCTL_TXFS_SAVEPOINT_INFORMATION As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 94, METHOD_BUFFERED, FILE_WRITE_DATA) '' TxF

        Public ReadOnly FSCTL_TXFS_CREATE_MINIVERSION As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 95, METHOD_BUFFERED, FILE_WRITE_DATA) '' TxF

        '' decommissioned fsctl value                                                 96
        '' decommissioned fsctl value                                                 97
        '' decommissioned fsctl value                                                 98
        Public ReadOnly FSCTL_TXFS_TRANSACTION_ACTIVE As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 99, METHOD_BUFFERED, FILE_READ_DATA)  '' TxF

        Public ReadOnly FSCTL_SET_ZERO_ON_DEALLOCATION As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 101, METHOD_BUFFERED, FILE_SPECIAL_ACCESS)

        Public ReadOnly FSCTL_SET_REPAIR As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 102, METHOD_BUFFERED, FILE_ANY_ACCESS)

        Public ReadOnly FSCTL_GET_REPAIR As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 103, METHOD_BUFFERED, FILE_ANY_ACCESS)

        Public ReadOnly FSCTL_WAIT_FOR_REPAIR As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 104, METHOD_BUFFERED, FILE_ANY_ACCESS)

        '' decommissioned fsctl value                                                 105
        Public ReadOnly FSCTL_INITIATE_REPAIR As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 106, METHOD_BUFFERED, FILE_ANY_ACCESS)

        Public ReadOnly FSCTL_CSC_INTERNAL As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 107, METHOD_NEITHER, FILE_ANY_ACCESS) '' CSC internal implementation

        Public ReadOnly FSCTL_SHRINK_VOLUME As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 108, METHOD_BUFFERED, FILE_SPECIAL_ACCESS) '' SHRINK_VOLUME_INFORMATION

        Public ReadOnly FSCTL_SET_SHORT_NAME_BEHAVIOR As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 109, METHOD_BUFFERED, FILE_ANY_ACCESS)

        Public ReadOnly FSCTL_DFSR_SET_GHOST_HANDLE_STATE As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 110, METHOD_BUFFERED, FILE_ANY_ACCESS)

        ''
        ''  Values 111 - 119 are reserved for FSRM.
        ''

        Public ReadOnly FSCTL_TXFS_LIST_TRANSACTION_LOCKED_FILES As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 120, METHOD_BUFFERED, FILE_READ_DATA) '' TxF
        Public ReadOnly FSCTL_TXFS_LIST_TRANSACTIONS As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 121, METHOD_BUFFERED, FILE_READ_DATA) '' TxF

        Public ReadOnly FSCTL_QUERY_PAGEFILE_ENCRYPTION As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 122, METHOD_BUFFERED, FILE_ANY_ACCESS)

        '' endif  /* _WIN32_WINNT >= _WIN32_WINNT_VISTA */

        '' if  (_WIN32_WINNT >= _WIN32_WINNT_VISTA)
        Public ReadOnly FSCTL_RESET_VOLUME_ALLOCATION_HINTS As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 123, METHOD_BUFFERED, FILE_ANY_ACCESS)

        '' endif  /* _WIN32_WINNT >= _WIN32_WINNT_VISTA */

        '' if  (_WIN32_WINNT >= _WIN32_WINNT_WIN7)
        Public ReadOnly FSCTL_QUERY_DEPENDENT_VOLUME As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 124, METHOD_BUFFERED, FILE_ANY_ACCESS)    '' Dependency File System Filter

        Public ReadOnly FSCTL_SD_GLOBAL_CHANGE As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 125, METHOD_BUFFERED, FILE_ANY_ACCESS) '' Query/Change NTFS Security Descriptors

        '' endif  /* _WIN32_WINNT >= _WIN32_WINNT_WIN7 */

        '' if  (_WIN32_WINNT >= _WIN32_WINNT_VISTA)
        Public ReadOnly FSCTL_TXFS_READ_BACKUP_INFORMATION2 As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 126, METHOD_BUFFERED, FILE_ANY_ACCESS) '' TxF

        '' endif  /* _WIN32_WINNT >= _WIN32_WINNT_VISTA */

        '' if  (_WIN32_WINNT >= _WIN32_WINNT_WIN7)
        Public ReadOnly FSCTL_LOOKUP_STREAM_FROM_CLUSTER As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 127, METHOD_BUFFERED, FILE_ANY_ACCESS)

        Public ReadOnly FSCTL_TXFS_WRITE_BACKUP_INFORMATION2 As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 128, METHOD_BUFFERED, FILE_ANY_ACCESS) '' TxF

        Public ReadOnly FSCTL_FILE_TYPE_NOTIFICATION As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 129, METHOD_BUFFERED, FILE_ANY_ACCESS)

        '' endif

        '' if  (_WIN32_WINNT >= _WIN32_WINNT_WIN8)
        Public ReadOnly FSCTL_FILE_LEVEL_TRIM As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 130, METHOD_BUFFERED, FILE_WRITE_DATA)

        '' endif  /*_WIN32_WINNT >= _WIN32_WINNT_WIN8 */

        ''
        ''  Values 131 - 139 are reserved for FSRM.
        ''

        '' if  (_WIN32_WINNT >= _WIN32_WINNT_WIN7)
        Public ReadOnly FSCTL_GET_BOOT_AREA_INFO As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 140, METHOD_BUFFERED, FILE_ANY_ACCESS) '' BOOT_AREA_INFO

        Public ReadOnly FSCTL_GET_RETRIEVAL_POINTER_BASE As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 141, METHOD_BUFFERED, FILE_ANY_ACCESS) '' RETRIEVAL_POINTER_BASE

        Public ReadOnly FSCTL_SET_PERSISTENT_VOLUME_STATE As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 142, METHOD_BUFFERED, FILE_ANY_ACCESS)  '' FILE_FS_PERSISTENT_VOLUME_INFORMATION

        Public ReadOnly FSCTL_QUERY_PERSISTENT_VOLUME_STATE As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 143, METHOD_BUFFERED, FILE_ANY_ACCESS)  '' FILE_FS_PERSISTENT_VOLUME_INFORMATION

        Public ReadOnly FSCTL_REQUEST_OPLOCK As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 144, METHOD_BUFFERED, FILE_ANY_ACCESS)

        Public ReadOnly FSCTL_CSV_TUNNEL_REQUEST As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 145, METHOD_BUFFERED, FILE_ANY_ACCESS) '' CSV_TUNNEL_REQUEST

        Public ReadOnly FSCTL_IS_CSV_FILE As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 146, METHOD_BUFFERED, FILE_ANY_ACCESS) '' IS_CSV_FILE

        Public ReadOnly FSCTL_QUERY_FILE_SYSTEM_RECOGNITION As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 147, METHOD_BUFFERED, FILE_ANY_ACCESS) ''

        Public ReadOnly FSCTL_CSV_GET_VOLUME_PATH_NAME As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 148, METHOD_BUFFERED, FILE_ANY_ACCESS)

        Public ReadOnly FSCTL_CSV_GET_VOLUME_NAME_FOR_VOLUME_MOUNT_POINT As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 149, METHOD_BUFFERED, FILE_ANY_ACCESS)

        Public ReadOnly FSCTL_CSV_GET_VOLUME_PATH_NAMES_FOR_VOLUME_NAME As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 150, METHOD_BUFFERED, FILE_ANY_ACCESS)

        Public ReadOnly FSCTL_IS_FILE_ON_CSV_VOLUME As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 151, METHOD_BUFFERED, FILE_ANY_ACCESS)

        '' endif  /* _WIN32_WINNT >= _WIN32_WINNT_WIN7 */

        '' if  (_WIN32_WINNT >= _WIN32_WINNT_WIN8)
        Public ReadOnly FSCTL_CORRUPTION_HANDLING As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 152, METHOD_BUFFERED, FILE_ANY_ACCESS)

        Public ReadOnly FSCTL_OFFLOAD_READ As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 153, METHOD_BUFFERED, FILE_READ_ACCESS)

        Public ReadOnly FSCTL_OFFLOAD_WRITE As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 154, METHOD_BUFFERED, FILE_WRITE_ACCESS)

        '' endif  /*_WIN32_WINNT >= _WIN32_WINNT_WIN8 */

        '' if  (_WIN32_WINNT >= _WIN32_WINNT_WIN7)
        Public ReadOnly FSCTL_CSV_INTERNAL As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 155, METHOD_BUFFERED, FILE_ANY_ACCESS)

        '' endif  /* _WIN32_WINNT >= _WIN32_WINNT_WIN7 */

        '' if  (_WIN32_WINNT >= _WIN32_WINNT_WIN8)
        Public ReadOnly FSCTL_SET_PURGE_FAILURE_MODE As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 156, METHOD_BUFFERED, FILE_ANY_ACCESS)

        Public ReadOnly FSCTL_QUERY_FILE_LAYOUT As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 157, METHOD_NEITHER, FILE_ANY_ACCESS)

        Public ReadOnly FSCTL_IS_VOLUME_OWNED_BYCSVFS As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 158, METHOD_BUFFERED, FILE_ANY_ACCESS)

        Public ReadOnly FSCTL_GET_INTEGRITY_INFORMATION As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 159, METHOD_BUFFERED, FILE_ANY_ACCESS)                  '' FSCTL_GET_INTEGRITY_INFORMATION_BUFFER

        Public ReadOnly FSCTL_SET_INTEGRITY_INFORMATION As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 160, METHOD_BUFFERED, FILE_READ_DATA Or FILE_WRITE_DATA) '' FSCTL_SET_INTEGRITY_INFORMATION_BUFFER

        Public ReadOnly FSCTL_QUERY_FILE_REGIONS As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 161, METHOD_BUFFERED, FILE_ANY_ACCESS)

        '' endif  /*_WIN32_WINNT >= _WIN32_WINNT_WIN8 */

        ''
        '' Dedup FSCTLs
        '' Values 162 - 170 are reserved for Dedup.
        ''

        '' if  (_WIN32_WINNT >= _WIN32_WINNT_WIN8)
        Public ReadOnly FSCTL_DEDUP_FILE As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 165, METHOD_BUFFERED, FILE_ANY_ACCESS)

        Public ReadOnly FSCTL_DEDUP_QUERY_FILE_HASHES As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 166, METHOD_NEITHER, FILE_READ_DATA)

        Public ReadOnly FSCTL_DEDUP_QUERY_RANGE_STATE As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 167, METHOD_NEITHER, FILE_READ_DATA)

        Public ReadOnly FSCTL_DEDUP_QUERY_REPARSE_INFO As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 168, METHOD_NEITHER, FILE_ANY_ACCESS)

        '' endif  /*_WIN32_WINNT >= _WIN32_WINNT_WIN8 */

        '' if  (_WIN32_WINNT >= _WIN32_WINNT_WIN8)
        Public ReadOnly FSCTL_RKF_INTERNAL As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 171, METHOD_NEITHER, FILE_ANY_ACCESS) '' Resume Key Filter

        Public ReadOnly FSCTL_SCRUB_DATA As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 172, METHOD_BUFFERED, FILE_ANY_ACCESS)

        Public ReadOnly FSCTL_REPAIR_COPIES As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 173, METHOD_BUFFERED, FILE_READ_DATA Or FILE_WRITE_DATA)

        Public ReadOnly FSCTL_DISABLE_LOCAL_BUFFERING As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 174, METHOD_BUFFERED, FILE_ANY_ACCESS)

        Public ReadOnly FSCTL_CSV_MGMT_LOCK As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 175, METHOD_BUFFERED, FILE_ANY_ACCESS)

        Public ReadOnly FSCTL_CSV_QUERY_DOWN_LEVEL_FILE_SYSTEM_CHARACTERISTICS As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 176, METHOD_BUFFERED, FILE_ANY_ACCESS)

        Public ReadOnly FSCTL_ADVANCE_FILE_ID As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 177, METHOD_BUFFERED, FILE_ANY_ACCESS)

        Public ReadOnly FSCTL_CSV_SYNC_TUNNEL_REQUEST As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 178, METHOD_BUFFERED, FILE_ANY_ACCESS)

        Public ReadOnly FSCTL_CSV_QUERY_VETO_FILE_DIRECT_IO As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 179, METHOD_BUFFERED, FILE_ANY_ACCESS)

        Public ReadOnly FSCTL_WRITE_USN_REASON As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 180, METHOD_BUFFERED, FILE_ANY_ACCESS)

        Public ReadOnly FSCTL_CSV_CONTROL As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 181, METHOD_BUFFERED, FILE_ANY_ACCESS)

        Public ReadOnly FSCTL_GET_REFS_VOLUME_DATA As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 182, METHOD_BUFFERED, FILE_ANY_ACCESS)

        Public ReadOnly FSCTL_CSV_H_BREAKING_SYNC_TUNNEL_REQUEST As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 185, METHOD_BUFFERED, FILE_ANY_ACCESS)

        '' endif  /*_WIN32_WINNT >= _WIN32_WINNT_WIN8 */

        '' if  (_WIN32_WINNT >= _WIN32_WINNT_WINBLUE)
        Public ReadOnly FSCTL_QUERY_STORAGE_CLASSES As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 187, METHOD_BUFFERED, FILE_ANY_ACCESS)

        Public ReadOnly FSCTL_QUERY_REGION_INFO As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 188, METHOD_BUFFERED, FILE_ANY_ACCESS)

        Public ReadOnly FSCTL_USN_TRACK_MODIFIED_RANGES As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 189, METHOD_BUFFERED, FILE_ANY_ACCESS) '' USN_TRACK_MODIFIED_RANGES

        '' endif  /* (_WIN32_WINNT >= _WIN32_WINNT_WINBLUE) */
        '' if  (_WIN32_WINNT >= _WIN32_WINNT_WINBLUE)
        Public ReadOnly FSCTL_QUERY_SHARED_VIRTUAL_DISK_SUPPORT As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 192, METHOD_BUFFERED, FILE_ANY_ACCESS)

        Public ReadOnly FSCTL_SVHDX_SYNC_TUNNEL_REQUEST As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 193, METHOD_BUFFERED, FILE_ANY_ACCESS)

        Public ReadOnly FSCTL_SVHDX_SET_INITIATOR_INFORMATION As New CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 194, METHOD_BUFFERED, FILE_ANY_ACCESS)

        '' endif  /* (_WIN32_WINNT >= _WIN32_WINNT_WINBLUE) */
        ''
        '' AVIO IOCTLS.
        ''

        Public ReadOnly IOCTL_AVIO_ALLOCATE_STREAM As New CTL_CODE(FILE_DEVICE_AVIO, 1, METHOD_BUFFERED, FILE_SPECIAL_ACCESS)

        Public ReadOnly IOCTL_AVIO_FREE_STREAM As New CTL_CODE(FILE_DEVICE_AVIO, 2, METHOD_BUFFERED, FILE_SPECIAL_ACCESS)

        Public ReadOnly IOCTL_AVIO_MODIFY_STREAM As New CTL_CODE(FILE_DEVICE_AVIO, 3, METHOD_BUFFERED, FILE_SPECIAL_ACCESS)

#End Region

#Region "Move File With Progress"

        Public Enum MOVE_FILE_FLAGS As UInteger

            ''' <summary>
            ''' If the file is to be moved to a different volume, the function simulates the move by using the CopyFile and DeleteFile functions.
            ''' If the file is successfully copied to a different volume and the original file is unable to be deleted, the function succeeds leaving the source file intact.
            ''' This value cannot be used with MOVEFILE_DELAY_UNTIL_REBOOT.
            ''' </summary>
            MOVEFILE_COPY_ALLOWED = 2

            ''' <summary>
            ''' Reserved for future use.
            ''' </summary>
            MOVEFILE_CREATE_HARDLINK = 16


            ''' <summary>
            ''' The system does not move the file until the operating system is restarted. The system moves the file immediately after AUTOCHK is executed, but before creating any paging files. Consequently, this parameter enables the function to delete paging files from previous startups.
            ''' This value can only be used if the process is in the context of a user who belongs to the administrators group or the LocalSystem account.
            ''' This value cannot be used with MOVEFILE_COPY_ALLOWED.
            ''' </summary>
            MOVEFILE_DELAY_UNTIL_REBOOT = 4


            ''' <summary>
            ''' The function fails if the source file is a link source, but the file cannot be tracked after the move. This situation can occur if the destination is a volume formatted with the FAT file system.
            ''' </summary>
            MOVEFILE_FAIL_IF_NOT_TRACKABLE = 32


            ''' <summary>
            ''' If a file named lpNewFileName exists, the function replaces its contents with the contents of the lpExistingFileName file.
            ''' This value cannot be used if lpNewFileName or lpExistingFileName names a directory.
            ''' </summary>
            MOVEFILE_REPLACE_EXISTING = 1


            ''' <summary>
            ''' The function does not return until the file has actually been moved on the disk.
            ''' Setting this value guarantees that a move performed as a copy and delete operation is flushed to disk before the function returns. The flush occurs at the end of the copy operation.
            ''' This value has no effect if MOVEFILE_DELAY_UNTIL_REBOOT is set.
            ''' </summary>
            MOVEFILE_WRITE_THROUGH = 8


        End Enum


        '        DWORD CALLBACK CopyProgressRoutine(
        '  _In_      LARGE_INTEGER TotalFileSize,
        '  _In_      LARGE_INTEGER TotalBytesTransferred,
        '  _In_      LARGE_INTEGER StreamSize,
        '  _In_      LARGE_INTEGER StreamBytesTransferred,
        '  _In_      DWORD dwStreamNumber,
        '  _In_      DWORD dwCallbackReason,
        '  _In_      HANDLE hSourceFile,
        '  _In_      HANDLE hDestinationFile,
        '  _In_opt_  LPVOID lpData
        ');

        ''' <summary>
        ''' Copy Progress Callback Reason
        ''' </summary>
        Public Enum CALLBACK_REASON As UInteger
            ''' <summary>
            ''' Another part of the data file was copied.
            ''' </summary>
            CALLBACK_CHUNK_FINISHED = 0

            ''' <summary>
            ''' Another stream was created and is about to be copied. 
            ''' This is the callback reason given when the callback routine is first invoked.
            ''' </summary>
            CALLBACK_STREAM_SWITCH = 1

        End Enum


        Public Delegate Function CopyProgressRoutine(
                                                    TotalFileSize As LARGE_INTEGER,
                                                    TotalBytesTrasnferred As LARGE_INTEGER,
                                                    StreamSize As LARGE_INTEGER,
                                                    StreambytesTransferred As LARGE_INTEGER,
                                                    dwStreamNumber As UInteger,
                                                    dwCallbackReason As CALLBACK_REASON,
                                                    hSourceFile As IntPtr,
                                                    hDestinationFile As IntPtr,
                                                    lpData As IntPtr
                                                    ) As UInteger





        '        BOOL WINAPI MoveFileWithProgress(
        '  _In_     LPCTSTR            lpExistingFileName,
        '  _In_opt_ LPCTSTR            lpNewFileName,
        '  _In_opt_ LPPROGRESS_ROUTINE lpProgressRoutine,
        '  _In_opt_ LPVOID             lpData,
        '  _In_     DWORD              dwFlags
        ');


        <DllImport("kernel32", EntryPoint:="MoveFileWithProgressW", CharSet:=CharSet.Unicode, PreserveSig:=True)>
        Public Function MoveFileWithProgress(lpExistingFilename As String,
                                             lpNewFilename As String,
                                             <MarshalAs(UnmanagedType.FunctionPtr)>
                                             lpPRogressRoutine As CopyProgressRoutine,
                                             lpData As IntPtr,
                                             dwFlag As MOVE_FILE_FLAGS) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function

        '        BOOL WINAPI CopyFileEx(
        '  _In_      LPCTSTR lpExistingFileName,
        '  _In_      LPCTSTR lpNewFileName,
        '  _In_opt_  LPPROGRESS_ROUTINE lpProgressRoutine,
        '  _In_opt_  LPVOID lpData,
        '  _In_opt_  LPBOOL pbCancel,
        '  _In_      DWORD dwCopyFlags
        ');


        <DllImport("kernel32", EntryPoint:="CopyFilExW", CharSet:=CharSet.Unicode, PreserveSig:=True)>
        Public Function CopyFileEx(lpExistingFilename As String,
                                   lpNewFilename As String,
                                   <MarshalAs(UnmanagedType.FunctionPtr)>
                                   lpProgressRoutine As CopyProgressRoutine,
                                   lpDAta As IntPtr,
                                   <MarshalAs(UnmanagedType.Bool)>
                                   ByRef pbCancel As Boolean,
                                   dwCopyFlags As UInteger) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function



#End Region

    End Module

End Namespace
