'' ************************************************* ''
'' DataTools Visual Basic Utility Library - Interop
''
'' Module: Utility
''         Miscellaneous Utility Functions
''
'' Copyright (C) 2011-2020 Nathan Moschkin
'' All Rights Reserved
''
'' Licensed Under the Microsoft Public License   
'' ************************************************* ''

Option Explicit On

Imports System
Imports System.IO
Imports System.ComponentModel
Imports System.Runtime.InteropServices
Imports System.Drawing
Imports System.Numerics
Imports System.Threading
Imports System.Reflection

Imports DataTools.Interop.Native
Imports System.Windows.Forms
Imports Microsoft.Win32
Imports CoreCT.Memory

Namespace Native


    ''' <summary>
    ''' Public utility methods
    ''' </summary>
    Public Module Utility


#Region "Structure <-> Byte() Copy Utility"

        ''' <summary>
        ''' Convert an array of bytes to a structure
        ''' </summary>
        ''' <param name="input">An arrayou of bytes</param>
        ''' <param name="output">A valid structure object (cannot be null)</param>
        ''' <param name="startingIndex">the starting index in the array of bytes.</param>
        ''' <param name="length">Length in the array of bytes.</param>
        ''' <returns>True if successful</returns>
        Public Function BytesToStruct(input() As Byte, ByRef output As Object, Optional startingIndex As Integer = 0, Optional length As Integer = -1) As Boolean

            Try
                Dim ptr As IntPtr = Marshal.AllocHGlobal(input.Length)

                If length = -1 Then length = input.Length - startingIndex

                Marshal.Copy(input, startingIndex, ptr, length)
                output = Marshal.PtrToStructure(ptr, output.GetType)
                Marshal.FreeHGlobal(ptr)

                Return True

            Catch ex As Exception
                Return False
            End Try

        End Function

        ''' <summary>
        ''' Writes a structure to a byte array
        ''' </summary>
        ''' <param name="input">Structure</param>
        ''' <param name="output">Byte array</param>
        ''' <returns>True if successful</returns>
        Public Function StructToBytes(input As Object, ByRef output() As Byte) As Boolean
            Try
                Dim a As Integer = Marshal.SizeOf(input)
                Dim ptr As IntPtr = Marshal.AllocHGlobal(a)
                ReDim output(a - 1)

                Marshal.StructureToPtr(input, ptr, False)
                Marshal.Copy(ptr, output, 0, a)
                Marshal.FreeHGlobal(ptr)

                Return True

            Catch ex As Exception
                Return False
            End Try

        End Function

        ''' <summary>
        ''' Write a structure to a byte array, and return the byte array.
        ''' </summary>
        ''' <param name="input"></param>
        ''' <returns><see cref="Byte()"/></returns>
        Public Function StructToBytes(input As Object) As Byte()
            Try
                Dim a As Integer = Marshal.SizeOf(input)
                Dim ptr As IntPtr = Marshal.AllocHGlobal(a)
                Dim output() As Byte = Nothing
                ReDim output(a - 1)

                Marshal.StructureToPtr(input, ptr, False)
                Marshal.Copy(ptr, output, 0, a)
                Marshal.FreeHGlobal(ptr)

                Return output

            Catch ex As Exception
                Return Nothing
            End Try

        End Function

#End Region

#Region "Read Structure From Stream"

        ''' <summary>
        ''' Read a structure from a stream
        ''' </summary>
        ''' <param name="stream"><see cref="Stream"/> object</param>
        ''' <param name="offset">Offset inside the stream to begin reading the struct.</param>
        ''' <param name="struct">The output struct.</param>
        ''' <returns>True if successful</returns>
        Public Function ReadStruct(stream As System.IO.Stream, offset As Integer, ByRef struct As Object) As Boolean

            Try
                Dim a As Integer = Marshal.SizeOf(struct)
                Dim b() As Byte
                ReDim b(a - 1)

                stream.Read(b, offset, a)
                Return BytesToStruct(b, struct)
            Catch ex As Exception

                Return False
            End Try

        End Function

        ''' <summary>
        ''' Read a structure of type T from a stream
        ''' </summary>
        ''' <param name="stream"><see cref="Stream"/> object</param>
        ''' <param name="struct">The output struct of type T</param>
        ''' <returns>True if successful</returns>
        Public Function ReadStruct(Of T As Structure)(stream As System.IO.Stream, ByRef struct As T) As Boolean

            Try
                Dim a As Integer = Marshal.SizeOf(Of T)

                Dim b() As Byte
                ReDim b(a - 1)

                stream.Read(b, 0, a)
                Dim gch = GCHandle.Alloc(b, GCHandleType.Pinned)

                Dim mm As MemPtr = gch.AddrOfPinnedObject()

                struct = mm.ToStruct(Of T)
                gch.Free()

                Return True

            Catch ex As Exception

                Return False
            End Try

        End Function

#End Region


#Region "Get Description"

        ''' <summary>
        ''' Gets a <see cref="DescriptionAttribute" /> value
        ''' </summary>
        ''' <param name="value">An object</param>
        ''' <returns>A <see cref="DescriptionAttribute" /> value</returns>
        Public Function GetEnumDescription(value As Object) As String

            If value.GetType.BaseType <> GetType([Enum]) Then Return Nothing

            Dim fi() As FieldInfo = value.GetType.GetFields(BindingFlags.Public Or BindingFlags.Static)

            For Each fe In fi
                If CLng(fe.GetValue(value)) = CLng(value) Then
                    Return GetDescription(fe)
                End If
            Next

            Return Nothing
        End Function

        ''' <summary>
        ''' Gets a <see cref="DescriptionAttribute" /> value
        ''' </summary>
        ''' <param name="mi">A <see cref="MemberInfo"/> object</param>
        ''' <returns>A <see cref="DescriptionAttribute" /> value</returns>
        Public Function GetDescription(mi As MemberInfo) As String

            Dim attr As DescriptionAttribute = CType(mi.GetCustomAttribute(GetType(DescriptionAttribute)), DescriptionAttribute)
            If attr Is Nothing Then Return Nothing
            Return attr.Description

        End Function

        ''' <summary>
        ''' Gets a <see cref="DescriptionAttribute" /> value
        ''' </summary>
        ''' <param name="t">A <see cref="System.Type"/></param>
        ''' <returns>A <see cref="DescriptionAttribute" /> value</returns>
        Public Function GetDescription(t As System.Type) As String

            Dim attr As DescriptionAttribute = CType(t.GetCustomAttribute(GetType(DescriptionAttribute)), DescriptionAttribute)
            If attr Is Nothing Then Return Nothing
            Return attr.Description

        End Function

#End Region '' Get Description

#Region "System and File System"

        Public Function MoveFile(oldPath As String, newPath As String) As Boolean
            Return CBool(FileApi.MoveFile(oldPath, newPath))

        End Function




        ''' <summary>
        ''' Completely destroy a directory
        ''' </summary>
        ''' <param name="directory"></param>
        ''' <remarks></remarks>
        Public Function DeleteWithExtremePrejudice(directory As String, Optional preserveRoot As Boolean = True, Optional warnConfirm As Boolean = True) As Boolean



            Dim sh As New SHFILEOPSTRUCT

            directory = Path.GetFullPath(directory)

            If warnConfirm Then

                If MsgBox("WARNING!  You are about to entirely destroy:" & vbCrLf & directory & "!" & vbCrLf & vbCrLf & "This action cannot be undone!  Proceed?", vbCritical Or vbYesNo) = vbNo Then Return False

            End If

            sh.hWnd = IntPtr.Zero
            sh.pFrom = directory & vbNullChar
            sh.wFunc = FO_DELETE
            sh.fFlags = FOF_SILENT Or FOF_NOCONFIRMATION Or FOF_NOERRORUI Or FOF_NOCONFIRMMKDIR

            Return SHFileOperation(sh) = 0

        End Function



        ''' <summary>
        ''' Retrieves the combined total file size for every file match string in an array.
        ''' </summary>
        ''' <param name="FileNames">Array of filenames or filename search patterns (wildcards).</param>
        ''' <returns>Total combined size of all discovered files.</returns>
        ''' <remarks></remarks>
        Public Function SizeOfAll(FileNames() As String) As Long

            Dim gt As Long = 0
            Dim h As IntPtr
            Dim fd As New WIN32_FIND_DATA

            For Each s As String In FileNames
                h = FindFirstFile(s, fd)
                If CLng(h) = -1 Then Continue For

                Do
                    gt += fd.nFileSize
                    fd = New WIN32_FIND_DATA
                Loop While FindNextFile(h, fd)

                FindClose(h)
            Next

            Return gt
        End Function

        ''' <summary>
        ''' Starts a new process.
        ''' </summary>
        ''' <param name="fileName"></param>
        ''' <param name="args"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Execute(fileName As String, Optional args As String = Nothing) As Boolean
            Dim proc As New System.Diagnostics.Process

            proc.StartInfo.FileName = fileName
            proc.StartInfo.UseShellExecute = True
            proc.StartInfo.Arguments = args
            proc.Start()

            Return True

        End Function

        ''' <summary>
        ''' Returns true if the file is an exe.
        ''' </summary>
        ''' <param name="fileName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsExe(fileName As String) As Boolean
            Return (fileName.Substring(fileName.Length - 4, 4).ToLower = ".exe")
        End Function

        ''' <summary>
        ''' Returns whether or not a file consists of the given extension.
        ''' </summary>
        ''' <param name="fileName"></param>
        ''' <param name="ext"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsExt(fileName As String, ext As String) As Boolean
            If fileName.Substring(fileName.Length - (ext.Length + 1), (ext.Length + 1)).ToLower = "." & ext Then Return True Else Return False
        End Function

        ' SHBrowseForFolder callback function

        ''' <summary>
        ''' SHBrowseForFolder callback function.
        ''' </summary>
        ''' <param name="hWnd"></param>
        ''' <param name="uMsg"></param>
        ''' <param name="wParam"></param>
        ''' <param name="lParam"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' Since we need the absolute path, we take the path of the most recently
        ''' selected item.  The only way to do that is through processing of the
        ''' BFFM_SELCHANGED event generated by SHBrowseForFolder, and retrieve the
        ''' full path using the SHGetPathFromIDList and wParam
        ''' </remarks>
        Public Function BrowseCallback(hWnd As IntPtr, uMsg As Short, wParam As Integer, lParam As Integer) As Integer

            If uMsg = BFFM_SELCHANGED Then
                mBrowseCurrent = New String(ChrW(0), 260)
                SHGetPathFromIDList(wParam, mBrowseCurrent)
                BrowserLastFolder = wParam
            End If

            Return 0

        End Function




        ''' <summary>
        ''' Gets the file's attributes.
        ''' </summary>
        ''' <param name="FileName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetFileAttributes(FileName As String) As FileAttributes
            Dim wf As New WIN32_FIND_DATA
            Dim i As IntPtr

            i = FindFirstFile(FileName, wf)

            If CLng(i) <> -1& Then
                FindClose(i)
                GetFileAttributes = wf.dwFileAttributes
            Else
                GetFileAttributes = 0
            End If

        End Function

        ''' <summary>
        ''' Set the file's attributes.
        ''' </summary>
        ''' <param name="FileName"></param>
        ''' <param name="Attributes"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SetFileAttributes(FileName As String, Attributes As FileAttributes) As Boolean

            If Not FileExists(FileName) Then Return False
            SetFileAttributes = CBool(pSetFileAttributes(FileName, Attributes))

        End Function

        ' Return the three dates of the given file

        ''' <summary>
        ''' Gets the file time stamps.
        ''' </summary>
        ''' <param name="FileName"></param>
        ''' <param name="CreationTime"></param>
        ''' <param name="LastAccessTime"></param>
        ''' <param name="LastWriteTime"></param>
        ''' <remarks></remarks>
        Public Sub GetFileTime(FileName As String, ByRef CreationTime As Date, ByRef LastAccessTime As Date, ByRef LastWriteTime As Date)
            Dim wf As New WIN32_FIND_DATA
            Dim i As IntPtr

            FileName = FileName.Trim("\"c)
            i = FindFirstFile(FileName, wf)

            If CLng(i) <> -1 Then
                FindClose(i)
                CreationTime = FileToLocal(wf.ftCreationTime)
                LastAccessTime = FileToLocal(wf.ftLastAccessTime)
                LastWriteTime = FileToLocal(wf.ftLastWriteTime)
            End If

        End Sub

        ''' <summary>
        ''' Set the file time stamps to the specified times.
        ''' </summary>
        ''' <param name="FileName"></param>
        ''' <param name="CreationTime"></param>
        ''' <param name="LastAccessTime"></param>
        ''' <param name="LastWriteTime"></param>
        ''' <remarks></remarks>
        Public Sub SetFileTime(FileName As String, CreationTime As FILETIME, LastAccessTime As FILETIME, LastWriteTime As FILETIME)

            Dim hFile As IntPtr

            If Directory.Exists(FileName) Then
                hFile = CreateFile(FileName, FILE_WRITE_ATTRIBUTES, 0&, IntPtr.Zero, OPEN_EXISTING, FILE_FLAG_BACKUP_SEMANTICS, IntPtr.Zero)
            ElseIf File.Exists(FileName) Then
                hFile = CreateFile(FileName, FILE_WRITE_ATTRIBUTES, 0&, IntPtr.Zero, OPEN_EXISTING, 0&, IntPtr.Zero)
            Else
                Return
            End If

            If CLng(hFile) = -1& Then
                Dim g = GetLastError
                Exit Sub
            End If

            Try
                pSetFileTime2(hFile, CreationTime, LastAccessTime, LastWriteTime)
            Catch ex As Exception
                'MsgBox(ex.Message)
            End Try

            CloseHandle(hFile)

        End Sub


        ''' <summary>
        ''' Set the file time stamps to the specified times.
        ''' </summary>
        ''' <param name="FileName"></param>
        ''' <param name="CreationTime"></param>
        ''' <param name="LastAccessTime"></param>
        ''' <param name="LastWriteTime"></param>
        ''' <remarks></remarks>
        Public Sub SetFileTime(FileName As String, CreationTime As Date, LastAccessTime As Date, LastWriteTime As Date)

            Dim wf As WIN32_FIND_DATA

            LocalToFileTime(CreationTime, wf.ftCreationTime)
            LocalToFileTime(LastAccessTime, wf.ftLastAccessTime)
            LocalToFileTime(LastWriteTime, wf.ftLastWriteTime)

            SetFileTime(FileName, wf.ftCreationTime, wf.ftLastAccessTime, wf.ftLastWriteTime)

        End Sub

        ' Get the size of a file

        ''' <summary>
        ''' Returns the file size in the style of the native function, with a high DWORD and a low DWORD.
        ''' </summary>
        ''' <param name="FileName"></param>
        ''' <param name="lpSizeHigh"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetFileSize(FileName As String, ByRef lpSizeHigh As UInteger) As UInteger

            Dim hFile As IntPtr

            Dim SL As UInteger,
            lpSh As UInteger

            If Not FileExists(FileName) Then Return 0


            hFile = CreateFile(FileName, GENERIC_READ, 0&, IntPtr.Zero, OPEN_EXISTING, 0&, IntPtr.Zero)
            If CLng(hFile) = -1& Then Return 0

            SL = pGetFileSize(hFile, lpSh)

            lpSizeHigh = lpSh

            CloseHandle(hFile)
            Return SL
        End Function

        ''' <summary>
        ''' Returns the size of the file using the native function.
        ''' </summary>
        ''' <param name="Filename"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetFileSize(Filename As String) As Long
            Dim h As UInteger
            Dim l As UInteger
            l = GetFileSize(Filename, h)
            Return MakeLong(l, CInt(h))
        End Function

        ' Returns a boolean indicating weather the file exists or not

        ''' <summary>
        ''' Returns true if the file exists.  Users native function.
        ''' </summary>
        ''' <param name="FileName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function FileExists(FileName As String) As Boolean
            Dim wf As New WIN32_FIND_DATA
            Dim i As IntPtr

            i = FindFirstFile(FileName, wf)

            If CLng(i) <> -1& Then
                FindClose(i)
                Return True
            End If

            Return False
        End Function

        ' Returns a boolean indicating weather the folder exists or not

        ''' <summary>
        ''' Returns true if the path exists.  Uses native function.
        ''' </summary>
        ''' <param name="Path"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function PathExists(Path As String) As Boolean

            Dim h As IntPtr,
                i As Integer,
                s As String

            Dim wf As New WIN32_FIND_DATA,
            df As Boolean

            s = Path
            i = s.Length
            If i = 0 Then Return False

            If (s.LastIndexOf("\") = s.Length - 1) And (i <> 3) Then
                s = Mid(s, 1, i - 1)
            ElseIf (i = 3) And (Mid(s, i - 1, 2) = ":\") Then
                s = s + "*.*"
                df = True
            End If

            h = FindFirstFile(s, wf)

            If CInt(h) <> -1& Then

                If (wf.dwFileAttributes And FILE_ATTRIBUTE_DIRECTORY) = FILE_ATTRIBUTE_DIRECTORY Or (df = True) Then
                    FindClose(h)
                    Return True
                End If

                FindClose(h)
            End If

            Return False
        End Function

        ''' <summary>
        ''' Retrieves the location of a special folder.
        ''' </summary>
        ''' <param name="hWnd"></param>
        ''' <param name="fSpcPath"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetSpecialFolder(hWnd As IntPtr, fSpcPath As SpecialFolderConstants) As String
            Dim lpStr As String

            lpStr = New String(ChrW(0), 512)
            SHGetSpecialFolderPath(hWnd, lpStr, fSpcPath, False)

            GetSpecialFolder = lpStr.Trim(ChrW(0))

        End Function


        ''' <summary>
        ''' Converts a FILETIME structure into a DateTime object in the local time zone.
        ''' </summary>
        ''' <param name="lpTime"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function FileToLocal(lpTime As FILETIME) As Date
            Dim pDate As Date,
            pTime As System.TimeSpan

            Dim lpSystem As New SYSTEMTIME,
            ftLocal As New FILETIME

            '' convert the filetime from UTC to local time
            FileTimeToLocalFileTime(lpTime, ftLocal)

            '' convert the FILETIME structure to a system time structure
            FileTimeToSystemTime(ftLocal, lpSystem)

            '' construct a Date variable from the system time structure.
            With lpSystem
                pTime = New TimeSpan(0, .wHour, .wMinute, .wSecond, .wMilliseconds)

                Try
                    pDate = DateSerial(.wYear, .wMonth, .wDay)
                    pDate = pDate.Add(pTime)
                    FileToLocal = pDate
                Catch ex As Exception
                    Return Now
                End Try

            End With

        End Function

        ''' <summary>
        ''' Converts a FILETIME structure into a DateTime object in the system time zone (generally UTC).
        ''' </summary>
        ''' <param name="lpTime"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function FileToSystem(lpTime As FILETIME) As Date
            Dim pDate As Date,
            pTime As System.TimeSpan

            Dim lpSystem As New SYSTEMTIME

            FileTimeToSystemTime(lpTime, lpSystem)

            With lpSystem
                pTime = New TimeSpan(.wHour, .wMinute, .wSecond)
                pDate = DateSerial(.wYear, .wMonth, .wDay)

                pDate = pDate.Add(pTime)
                FileToSystem = pDate

            End With

        End Function

        ''' <summary>
        ''' Returns a date-time string appropriate for injecting into a SQL record.
        ''' </summary>
        ''' <param name="time"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function TimeToSQL(time As Date) As String
            Return time.ToString("yyyy-MM-dd HH:mm:ss")
        End Function

        ''' <summary>
        ''' Converts a DateTime object containing a UTC time value into a FILETIME structure
        ''' </summary>
        ''' <param name="time"></param>
        ''' <param name="lpTime"></param>
        ''' <remarks></remarks>
        Public Sub SystemToFileTime(time As Date, ByRef lpTime As FILETIME)
            Dim st As New SYSTEMTIME,
            lft As New FILETIME

            With st
                .wYear = CUShort(time.Year)
                .wMonth = CUShort(time.Month)
                .wDay = CUShort(time.Day)
                .wHour = CUShort(time.Hour)
                .wMinute = CUShort(time.Minute)
                .wSecond = CUShort(time.Second)
            End With

            SystemTimeToFileTime(st, lpTime)

        End Sub

        ''' <summary>
        ''' Converts a DateTime object containing a local time value into a FILETIME structure
        ''' </summary>
        ''' <param name="time"></param>
        ''' <param name="lpTime"></param>
        ''' <remarks></remarks>
        Public Sub LocalToFileTime(time As Date, ByRef lpTime As FILETIME)
            Dim st As New SYSTEMTIME,
            lft As New FILETIME

            With st
                .wYear = CUShort(time.Year)
                .wMonth = CUShort(time.Month)
                .wDay = CUShort(time.Day)
                .wHour = CUShort(time.Hour)
                .wMinute = CUShort(time.Minute)
                .wSecond = CUShort(time.Second)
                .wMilliseconds = CUShort(time.Millisecond)
            End With

            SystemTimeToFileTime(st, lft)
            LocalFileTimeToFileTime(lft, lpTime)

        End Sub

        ''' <summary>
        ''' Combines two integers into a long.
        ''' </summary>
        ''' <param name="dwLow"></param>
        ''' <param name="dwHigh"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function MakeLong(dwLow As UInteger, dwHigh As Integer) As Long
            Return (CLng(dwHigh) << 32) Or CLng(dwLow)
        End Function

        ''' <summary>
        ''' Combines two shorts into an int.
        ''' </summary>
        ''' <param name="dwLow"></param>
        ''' <param name="dwHigh"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function MakeInt(dwLow As UShort, dwHigh As Short) As Integer
            Return (CInt(dwHigh) << 16) Or CInt(dwLow)
        End Function

        ''' <summary>
        ''' Makes an int resource (basically a new IntPtr with a value of i).
        ''' </summary>
        ''' <param name="i"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function MAKEINTRESOURCE(i As Integer) As IntPtr
            Return New IntPtr(i)
        End Function

#End Region '' System and File System

#Region "DumpInterface"

#If NOOBLE Then
    Public Sub DumpInterface(tInterface As Type, out As System.Windows.Forms.TextBox)

        Dim m() As MemberInfo = tInterface.GetMembers

        out.Text &= vbCrLf & vbCrLf & "<ComImport, Guid(""" & tInterface.GUID.ToString() & """), _" & vbCrLf & "InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _" & vbCrLf
        out.Text &= "Public Interface " & tInterface.Name & vbCrLf & vbCrLf

        For Each menum In m

            If menum.MemberType = MemberTypes.Method Then
                Dim mtd As MethodInfo = CType(menum, MethodInfo)
                Dim pia() As ParameterInfo = mtd.GetParameters
                Dim benum As Boolean = False

                out.Text &= "<PreserveSig> _" & vbCrLf & "Function " & mtd.Name & "("

                For Each pi In pia
                    Dim fatt As String = ""
                    Dim fParam As Boolean = False

                    If benum Then
                        out.Text &= ", "
                    Else
                        benum = True
                    End If
                    If pi.IsIn Then
                        fatt &= If(fatt = "", "<", ", ")
                        fatt &= "[In]"

                    End If

                    If pi.IsOut Then
                        fatt &= If(fatt = "", "<", ",")
                        fatt &= "Out"

                    End If

                    For Each ca In pi.CustomAttributes
                        If ca.AttributeType = GetType(MarshalAsAttribute) Then

                            Dim fir As Boolean = False

                            fatt &= If(fatt = "", "<", ", ")
                            fatt &= "MarshalAs(UnmanagedType."

                            For Each carg In ca.ConstructorArguments
                                If fir Then
                                    fatt &= ", " & carg.ToString.Replace("=", " := ")
                                Else
                                    fatt &= CType(carg.Value, UnmanagedType).ToString()
                                    fir = True
                                End If
                            Next

                            fatt &= ")"
                            Exit For
                        End If
                    Next

                    fatt &= If(fatt = "", "", ">")

                    out.Text &= fatt & " " & If(pi.IsOut, "ByRef ", "") & pi.Name & " As " & If(Blob.TypeToBlobType(pi.ParameterType) = BlobTypes.Invalid, pi.ParameterType.ToString.Replace("&", ""), Blob.TypeToBlobType(pi.ParameterType).ToString)

                Next

                out.Text &= ") As HResult" & vbCrLf & vbCrLf
            End If

        Next

        out.Text &= vbCrLf & vbCrLf & "End Interface"
    End Sub
#End If

#End Region '' DumpInterface

    End Module

End Namespace