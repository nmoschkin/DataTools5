'' ************************************************* ''
'' DataTools Visual Basic Utility Library - Interop
''
'' Module: WNet
''         Back-end Windows Networking Resources
''
'' Copyright (C) 2011-2020 Nathan Moschkin
'' All Rights Reserved
''
'' Licensed Under the Microsoft Public License   
'' ************************************************* ''

Option Explicit On

Imports System
Imports System.Runtime.InteropServices
Imports CoreCT.Memory

Namespace Native
    ''
    ''  Network Resources.
    ''
    <HideModuleName>
    Friend Module WNet

        Public Const RESOURCE_CONNECTED = &H1
        Public Const RESOURCE_GLOBALNET = &H2
        Public Const RESOURCE_REMEMBERED = &H3
        Public Const RESOURCE_RECENT = &H4
        Public Const RESOURCE_CONTEXT = &H5

        Public Const RESOURCETYPE_ANY = &H0
        Public Const RESOURCETYPE_DISK = &H1
        Public Const RESOURCETYPE_PRINT = &H2
        Public Const RESOURCETYPE_RESERVED = &H8
        Public Const RESOURCETYPE_UNKNOWN = &HFFFFFFFF

        Public Const RESOURCEUSAGE_CONNECTABLE = &H1
        Public Const RESOURCEUSAGE_CONTAINER = &H2
        Public Const RESOURCEUSAGE_NOLOCALDEVICE = &H4
        Public Const RESOURCEUSAGE_SIBLING = &H8
        Public Const RESOURCEUSAGE_ATTACHED = &H10
        Public Const RESOURCEUSAGE_ALL = (RESOURCEUSAGE_CONNECTABLE Or RESOURCEUSAGE_CONTAINER Or RESOURCEUSAGE_ATTACHED)
        Public Const RESOURCEUSAGE_RESERVED = &H80000000

        Public Const RESOURCEDISPLAYTYPE_GENERIC = &H0
        Public Const RESOURCEDISPLAYTYPE_DOMAIN = &H1
        Public Const RESOURCEDISPLAYTYPE_SERVER = &H2
        Public Const RESOURCEDISPLAYTYPE_SHARE = &H3
        Public Const RESOURCEDISPLAYTYPE_FILE = &H4
        Public Const RESOURCEDISPLAYTYPE_GROUP = &H5
        Public Const RESOURCEDISPLAYTYPE_NETWORK = &H6
        Public Const RESOURCEDISPLAYTYPE_ROOT = &H7
        Public Const RESOURCEDISPLAYTYPE_SHAREADMIN = &H8
        Public Const RESOURCEDISPLAYTYPE_DIRECTORY = &H9
        Public Const RESOURCEDISPLAYTYPE_TREE = &HA
        Public Const RESOURCEDISPLAYTYPE_NDSCONTAINER = &HB


        Public Const CONNECT_UPDATE_PROFILE = &H1
        Public Const CONNECT_UPDATE_RECENT = &H2
        Public Const CONNECT_TEMPORARY = &H4
        Public Const CONNECT_INTERACTIVE = &H8
        Public Const CONNECT_PROMPT = &H10
        Public Const CONNECT_NEED_DRIVE = &H20
        Public Const CONNECT_REFCOUNT = &H40
        Public Const CONNECT_REDIRECT = &H80
        Public Const CONNECT_LOCALDRIVE = &H100
        Public Const CONNECT_CURRENT_MEDIA = &H200
        Public Const CONNECT_DEFERRED = &H400
        Public Const CONNECT_RESERVED = &HFF000000
        Public Const CONNECT_COMMANDLINE = &H800
        Public Const CONNECT_CMD_SAVECRED = &H1000
        Public Const CONNECT_CRED_RESET = &H2000


        Public Const ERROR_CALL_NOT_IMPLEMENTED = 120
        Public Const ERROR_NO_MORE_ITEMS = 259
        Public Const ERROR_NO_NETWORK = 1222
        Public Const ERROR_MORE_DATA = 234
        Public Const ERROR_INVALID_PARAMETER = 87

        Public Const ERROR_CONNECTED_OTHER_PASSWORD = 2108
        Public Const ERROR_CONNECTED_OTHER_PASSWORD_DEFAULT = 2109
        Public Const ERROR_BAD_USERNAME = 2202
        Public Const ERROR_NOT_CONNECTED = 2250
        Public Const ERROR_OPEN_FILES = 2401
        Public Const ERROR_ACTIVE_CONNECTIONS = 2402
        Public Const ERROR_DEVICE_IN_USE = 2404

        Public Const ERROR_INVALID_PASSWORD = 86


        Public Const NO_ERROR = 0




        Public Structure NETRESOURCE

            Public dwScope As Integer
            Public dwType As Integer
            Public dwDisplayType As Integer
            Public dwUsage As Integer

            <MarshalAs(UnmanagedType.LPWStr)>
            Public lpLocalName As String

            <MarshalAs(UnmanagedType.LPWStr)>
            Public lpRemoteName As String

            <MarshalAs(UnmanagedType.LPWStr)>
            Public lpComment As String

            <MarshalAs(UnmanagedType.LPWStr)>
            Public lpProvider As String

            Public Overrides Function ToString() As String
                Return lpRemoteName
            End Function

        End Structure

        Public Declare Unicode Function WNetOpenEnum Lib "Mpr.dll" _
            Alias "WNetOpenEnumW" (dwScope As Integer,
                                    dwType As Integer,
                                    dwUsage As Integer,
                                    lpNetResource As IntPtr,
                                    ByRef lphEnum As IntPtr) As Integer

        Public Declare Unicode Function WNetEnumResource Lib "Mpr.dll" _
            Alias "WNetEnumResourceW" (hEnum As IntPtr,
                                       ByRef lpcCount As Integer,
                                       lpBuffer As IntPtr,
                                       ByRef lpBufferSize As Integer) As Integer

        Public Declare Unicode Function WNetCloseEnum Lib "Mpr.dll" (hEnum As IntPtr) As Integer


        '        DWORD WNetAddConnection3(
        '  _In_ HWND          hwndOwner,
        '  _In_ LPNETRESOURCE lpNetResource,
        '  _In_ LPTSTR        lpPassword,
        '  _In_ LPTSTR        lpUserName,
        '  _In_ DWORD         dwFlags
        ');


        Public Declare Unicode Function WNetAddConnection3 Lib "Mpr.dll" _
            Alias "WNetAddConnection3W" (hwndOwner As IntPtr,
                                         lpNetResource As IntPtr,
                                         <MarshalAs(UnmanagedType.LPTStr)>
                                         lpPassword As String,
                                         <MarshalAs(UnmanagedType.LPTStr)>
                                         lpusername As String,
                                         dwFlags As Integer
                                         ) As Integer


        Public Function EnumNetwork() As NETRESOURCE()

            EnumNetwork = DoEnum(IntPtr.Zero)

        End Function

        Public Function EnumComputer(computer As String, Optional username As String = Nothing, Optional password As String = Nothing) As NETRESOURCE()
            Dim lpnet As New NETRESOURCE
            Dim mm As MemPtr

            If computer.Substring(0, 2) = "\\" Then computer = computer.Substring(2)

            mm.ReAlloc(10240)


            If username IsNot Nothing AndAlso password IsNot Nothing Then

                lpnet.lpRemoteName = "\\" & computer

                Marshal.StructureToPtr(lpnet, mm.Handle, False)

                Dim res = WNetAddConnection3(IntPtr.Zero, mm.Handle, password, username, CONNECT_INTERACTIVE)

                mm.Free()

                If (res <> 0) Then
                    Return Nothing
                End If

                mm.ReAlloc(10240)

            End If


            lpnet.dwDisplayType = RESOURCEDISPLAYTYPE_SERVER
            lpnet.lpRemoteName = "\\" & computer
            lpnet.dwScope = RESOURCE_CONTEXT
            lpnet.dwUsage = RESOURCEUSAGE_CONTAINER

            Marshal.StructureToPtr(lpnet, mm.Handle, False)

            EnumComputer = DoEnum(mm.Handle)

            mm.Free()
        End Function

        Function DoEnum(lpNet As IntPtr) As NETRESOURCE()
            Dim mm As MemPtr
            Dim cb As Integer = 10240
            Dim bb() As NETRESOURCE = Nothing
            Dim nin() As NETRESOURCE = Nothing
            Dim hEnum As IntPtr
            Dim e As Integer = 0

            e = WNetOpenEnum(RESOURCE_GLOBALNET, RESOURCETYPE_DISK, RESOURCEUSAGE_ALL, lpNet, hEnum)

            If e <> NO_ERROR Then
                Return Nothing
            End If
            e = 0
            mm.ReAlloc(10240)

            While WNetEnumResource(hEnum, 1, mm, cb) = NO_ERROR
                ReDim Preserve bb(e)
                nin = DoEnum(mm.Handle)
                bb(e) = mm.ToStruct(Of NETRESOURCE)
                If nin IsNot Nothing Then
                    bb = WNACat(bb, nin)
                    nin = Nothing
                End If
                If (bb IsNot Nothing) Then e = bb.Length Else e = 0
            End While

            mm.Free()

            WNetCloseEnum(hEnum)
            DoEnum = bb
        End Function

        Private Function WNACat(a1 As NETRESOURCE(), a2 As NETRESOURCE()) As NETRESOURCE()
            Dim e As Integer = If(a1 IsNot Nothing, a1.Length, 0) + If(a2 IsNot Nothing, a2.Length, 0)
            Dim a3() As NETRESOURCE = Nothing
            ReDim a3(e - 1)
            Dim c As Integer = 0

            If a1 IsNot Nothing Then
                a1.CopyTo(a3, 0)
                c = a1.Length
            End If

            If a2 IsNot Nothing Then
                a2.CopyTo(a3, c)
            End If

            WNACat = a3
        End Function

    End Module

End Namespace