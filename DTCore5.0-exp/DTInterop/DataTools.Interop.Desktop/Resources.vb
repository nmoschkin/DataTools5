'' ************************************************* ''
'' DataTools Visual Basic Utility Library - Interop
''
'' Module: Resources.
''         Tools for grabbing resources from EXE and DLL modules.
''
'' Copyright (C) 2011-2020 Nathan Moschkin
'' All Rights Reserved
''
'' Licensed Under the Microsoft Public License
'' ************************************************* ''

Imports System.Drawing
Imports System.IO
Imports System.Runtime.InteropServices
Imports DataTools.ExtendedMath.ColorMath
Imports DataTools.Interop.Native
Imports CoreCT.Memory
Imports CoreCT.Text
Imports DataTools.ExtendedMath.ColorMath.ColorStructs

Namespace Desktop

#Region "Icons, Images and Resources"

    ''' <summary>
    ''' Flags to be used with LoadLibraryEx
    ''' </summary>
    ''' <remarks></remarks>
    <Flags>
    Public Enum LoadLibraryExFlags

        ''' <summary>
        '''  If this value is used, and the executable module is a DLL, the system does not call DllMain for process and thread initialization and termination. Also, the system does not load additional executable modules that are referenced by the specified module.
        ''' </summary>
        DONT_RESOLVE_DLL_REFERENCES = &H1

        ''' <summary>
        '''  If this value is used, the system does not check OnlineAppLocker rules or apply OnlineSoftware Restriction Policies for the DLL. This action applies only to the DLL being loaded and not to its dependencies. This value is recommended for use in setup programs that must run extracted DLLs during installation.
        ''' </summary>
        LOAD_IGNORE_CODE_AUTHZ_LEVEL = &H10

        ''' <summary>
        '''  If this value is used, the system maps the file into the calling process's virtual address space as if it were a data file. Nothing is done to execute or prepare to execute the mapped file. Therefore, you cannot call functions like GetModuleFileName, GetModuleHandle or GetProcAddress with this DLL. Using this value causes writes to read-only memory to raise an access violation. Use this flag when you want to load a DLL only to extract messages or resources from it.
        ''' </summary>
        LOAD_LIBRARY_AS_DATAFILE = &H2

        ''' <summary>
        '''  Similar to LOAD_LIBRARY_AS_DATAFILE, except that the DLL file is opened with exclusive write access for the calling process. Other processes cannot open the DLL file for write access while it is in use. However, the DLL can still be opened by other processes.
        ''' </summary>
        LOAD_LIBRARY_AS_DATAFILE_EXCLUSIVE = &H40

        ''' <summary>
        '''  If this value is used, the system maps the file into the process's virtual address space as an image file. However, the loader does not load the static imports or perform the other usual initialization steps. Use this flag when you want to load a DLL only to extract messages or resources from it. If forced integrity checking is desired for the loaded file then LOAD_LIBRARY_AS_IMAGE is recommended instead.
        ''' </summary>
        LOAD_LIBRARY_AS_IMAGE_RESOURCE = &H20

        ''' <summary>
        '''  If this value is used, the application's installation directory is searched for the DLL and its dependencies. Directories in the standard search path are not searched. This value cannot be combined with LOAD_WITH_ALTERED_SEARCH_PATH.
        ''' </summary>
        LOAD_LIBRARY_SEARCH_APPLICATION_DIR = &H200

        ''' <summary>
        '''  This value is a combination of LOAD_LIBRARY_SEARCH_APPLICATION_DIR, LOAD_LIBRARY_SEARCH_SYSTEM32, and LOAD_LIBRARY_SEARCH_USER_DIRS. Directories in the standard search path are not searched. This value cannot be combined with LOAD_WITH_ALTERED_SEARCH_PATH.
        ''' </summary>
        LOAD_LIBRARY_SEARCH_DEFAULT_DIRS = &H1000

        ''' <summary>
        '''  If this value is used, the directory that contains the DLL is temporarily added to the beginning of the list of directories that are searched for the DLL's dependencies. Directories in the standard search path are not searched.
        ''' </summary>
        LOAD_LIBRARY_SEARCH_DLL_LOAD_DIR = &H100

        ''' <summary>
        '''  If this value is used, %windows%\system32 is searched for the DLL and its dependencies. Directories in the standard search path are not searched. This value cannot be combined with LOAD_WITH_ALTERED_SEARCH_PATH.
        ''' </summary>
        LOAD_LIBRARY_SEARCH_SYSTEM32 = &H800

        ''' <summary>
        '''  If this value is used, directories added using the AddDllDirectory or the SetDllDirectory function are searched for the DLL and its dependencies. If more than one directory has been added, the order in which the directories are searched is unspecified. Directories in the standard search path are not searched. This value cannot be combined with LOAD_WITH_ALTERED_SEARCH_PATH.
        ''' </summary>
        LOAD_LIBRARY_SEARCH_USER_DIRS = &H400

        ''' <summary>
        '''  If this value is used and lpFileName specifies an absolute path, the system uses the alternate file search strategy discussed in the Remarks section to find associated executable modules that the specified module causes to be loaded. If this value is used and lpFileName specifies a relative path, the behavior is undefined.
        ''' </summary>
        LOAD_WITH_ALTERED_SEARCH_PATH = &H8

    End Enum

    ''' <summary>
    ''' Tools for retrieving system and individual EXE/DLL resources, and converting resources to WPF format.
    ''' </summary>
    Public Module Resources

#Region "RT_GROUPS"

        Public Const RT_CURSOR = (1)
        Public Const RT_BITMAP = (2)
        Public Const RT_ICON = (3)
        Public Const RT_MENU = (4)
        Public Const RT_DIALOG = (5)
        Public Const RT_STRING = (6)
        Public Const RT_FONTDIR = (7)
        Public Const RT_FONT = (8)
        Public Const RT_ACCELERATOR = (9)
        Public Const RT_RCDATA = (10)
        Public Const RT_MESSAGETABLE = (11)

        Public Const DIFFERENCE = 11
        Public Const RT_GROUP_CURSOR = ((RT_CURSOR) + DIFFERENCE)
        Public Const RT_GROUP_ICON = ((RT_ICON) + DIFFERENCE)
        Public Const RT_VERSION = (16)
        Public Const RT_DLGINCLUDE = (17)
        Public Const RT_PLUGPLAY = (19)
        Public Const RT_VXD = (20)
        Public Const RT_ANICURSOR = (21)
        Public Const RT_ANIICON = (22)

#End Region

        Declare Function ExtractAssociatedIconEx Lib "shell32.dll" Alias "ExtractAssociatedIconExW" (hInst As IntPtr, <MarshalAs(UnmanagedType.LPTStr)> lpIconPath As String, ByRef lpiIconIndex As Integer, ByRef lpiIconId As Integer) As IntPtr

        Declare Unicode Function LoadString Lib "user32" Alias "LoadStringW" _
    (hInstance As IntPtr, uID As Integer, lpBuffer As MemPtr, nBufferMax As Integer) As Integer

        Declare Function LockResource Lib "kernel32" (hResData As IntPtr) As IntPtr

        Declare Function LoadResource Lib "kernel32" (hModule As IntPtr, hResInfo As IntPtr) As IntPtr

        Declare Function SizeofResource Lib "kernel32" (hModule As IntPtr, hResInfo As IntPtr) As Integer

        Declare Unicode Function LoadLibraryEx Lib "kernel32" _
    Alias "LoadLibraryExW" _
    (lpFileName As String,
     hFile As IntPtr,
     flags As LoadLibraryExFlags) As IntPtr

        Delegate Function EnumResNameProc _
    (hModule As IntPtr,
     lpszType As String,
     lpszName As String,
     lParam As IntPtr) As Boolean

        Delegate Function EnumResNameProcPtr _
    (hModule As IntPtr,
     lpszType As MemPtr,
     lpszName As MemPtr,
     lParam As IntPtr) As Boolean

        Declare Unicode Function EnumResourceNamesEx Lib "kernel32" _
    Alias "EnumResourceNamesExW" _
    (hModule As IntPtr,
     lpszType As String,
     lpEnumFunc As EnumResNameProc,
     lParam As IntPtr,
     dwFlags As Integer,
     langId As Integer) As Boolean

        Declare Unicode Function EnumResourceNamesEx Lib "kernel32" _
    Alias "EnumResourceNamesExW" _
    (hModule As IntPtr,
     lpszType As IntPtr,
     lpEnumFunc As EnumResNameProcPtr,
     lParam As IntPtr,
     dwFlags As Integer,
     langId As Integer) As Boolean

        Declare Function LookupIconIdFromDirectoryEx Lib "user32" _
    (presbits As IntPtr,
     <MarshalAs(UnmanagedType.Bool)>
     fIcon As Boolean,
     cxDesired As Integer,
     cyDesired As Integer,
     Flags As Integer) As Integer

        Declare Unicode Function FindResourceEx Lib "kernel32" _
    Alias "FindResourceExW" _
    (hModule As IntPtr,
     <MarshalAs(UnmanagedType.LPWStr)>
     lpType As String,
     <MarshalAs(UnmanagedType.LPWStr)>
     lpName As String,
     wLanguage As UShort) As IntPtr

        Declare Unicode Function FindResourceEx Lib "kernel32" _
    Alias "FindResourceExW" _
    (hModule As IntPtr,
     lpType As IntPtr,
     lpName As IntPtr,
     wLanguage As UShort) As IntPtr

        Declare Function CreateIconFromResourceEx Lib "user32" _
    (phIconBits As IntPtr,
     cbIconBits As Integer,
     <MarshalAs(UnmanagedType.Bool)>
     fIcon As Boolean,
     dwVersion As Integer,
     cxDesired As Integer,
     cyDesired As Integer,
     uFlags As Integer) As IntPtr

        Public Enum SystemIconSizes
            Small = SHIL_SMALL
            Large = SHIL_LARGE
            ExtraLarge = SHIL_EXTRALARGE
            Jumbo = SHIL_JUMBO
        End Enum

        <StructLayout(LayoutKind.Sequential)>
        Public Structure NEWHEADER
            Public Reserved As UShort
            Public ResType As UShort
            Public ResCount As UShort
        End Structure

        <StructLayout(LayoutKind.Sequential)>
        Public Structure ICONRESDIR
            Public Width As Byte
            Public Height As Byte
            Public ColorCount As Byte
            Public reserved As Byte
        End Structure

        <StructLayout(LayoutKind.Sequential)>
        Public Structure RESOURCEHEADER
            Public DataSize As Integer
            Public HeaderSize As Integer
            Public Type As Integer
            Public Name As Integer
            Public DataVersion As Integer
            Public MemoryFlags As Short
            Public LanguageId As Short
            Public Version As Integer
            Public Characteristics As Integer
        End Structure

        ''' <summary>
        ''' Holds a collection of MemPtr.
        ''' </summary>
        ''' <remarks></remarks>
        Public Class ResCol
            Inherits List(Of MemPtr)

        End Class

        ''' <summary>
        ''' Private icon WPFLibrary cache.
        ''' </summary>
        ''' <remarks></remarks>
        Private WPFLibCache As New Dictionary(Of String, Windows.Media.Imaging.BitmapSource)

        ''' <summary>
        ''' Add an icon to the WPFLibrary cache.
        ''' </summary>
        ''' <param name="resId">The entire icon resource identifier, including filename and resource number.</param>
        ''' <param name="icn">The Icon object to add.</param>
        ''' <remarks></remarks>
        Public Sub AddToWPFLibCache(resId As String, icn As Windows.Media.Imaging.BitmapSource)
            If resId Is Nothing Then Return
            WPFLibCache.Add(resId, icn)
        End Sub

        ''' <summary>
        ''' Lookup a WPFLibrary Icon object from the private cache.
        ''' </summary>
        ''' <param name="resId">The entire icon resource identifier, including filename and resource number.</param>
        ''' <param name="icn">Variable that receives the Icon object.</param>
        ''' <returns>True if the resource was found.</returns>
        ''' <remarks></remarks>
        Public Function LookupWPFLibIcon(resId As String, ByRef icn As Windows.Media.Imaging.BitmapSource) As Boolean
            If resId Is Nothing Then Return False
            Return WPFLibCache.TryGetValue(resId, icn)
        End Function


        ''' <summary>
        ''' Private icon library cache.
        ''' </summary>
        ''' <remarks></remarks>
        Private LibCache As New Dictionary(Of String, Icon)

        ''' <summary>
        ''' Add an icon to the library cache.
        ''' </summary>
        ''' <param name="resId">The entire icon resource identifier, including filename and resource number.</param>
        ''' <param name="icn">The Icon object to add.</param>
        ''' <remarks></remarks>
        Public Sub AddToLibCache(resId As String, icn As Icon)
            If resId Is Nothing Then Return
            LibCache.Add(resId, icn)
        End Sub

        ''' <summary>
        ''' Lookup a library Icon object from the private cache.
        ''' </summary>
        ''' <param name="resId">The entire icon resource identifier, including filename and resource number.</param>
        ''' <param name="icn">Variable that receives the Icon object.</param>
        ''' <returns>True if the resource was found.</returns>
        ''' <remarks></remarks>
        Public Function LookupLibIcon(resId As String, ByRef icn As Icon) As Boolean
            If resId Is Nothing Then Return False
            Return LibCache.TryGetValue(resId, icn)
        End Function

        ''' <summary>
        ''' Clear the private library cache.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub ClearLibCache()
            LibCache.Clear()
            GC.Collect(0)
        End Sub

        ''' <summary>
        ''' Parses a resource filename and attempts to distill a valid absolute file path and optional resource identifier from the input string.
        ''' </summary>
        ''' <param name="fileName">Filename to parse.</param>
        ''' <param name="resId">Optional variable to receive the resource identifier.</param>
        ''' <returns>A cleaned filename.</returns>
        ''' <remarks></remarks>
        Public Function ParseResourceFilename(fileName As String, Optional ByRef resId As Integer = -0) As String
            If fileName Is Nothing Then
                Return Nothing
            End If

            '' let's do some jiggery to figure out what the fileName string actually points to:
            '' strip the double quotes sometimes passed in (especially from Registry variables)
            If fileName.Substring(0, 1) = """" Then
                fileName = fileName.Replace("""", "")
            End If

            '' if we have an @ sign (as is common of resource identifiers) we strip that out.
            If fileName.Substring(0, 1) = "@" Then fileName = fileName.Substring(1)

            '' does this file have an attached resource identifier?
            Dim i As Integer = fileName.LastIndexOf(",")

            If i <> -1 Then
                Integer.TryParse(fileName.Substring(i + 1), resId)
                fileName = fileName.Substring(0, i)
            End If

            '' finally, we expand any embedded environment variables.
            fileName = Environment.ExpandEnvironmentVariables(fileName)

            '' after all that, we still don't have a path?  We'll assume system32 is the path.
            If fileName.IndexOf("\") = -1 Then
                fileName = Environment.ExpandEnvironmentVariables("%systemroot%\system32\" & fileName)
            End If

            If fileName.IndexOf("%") >= 0 Then Return Nothing

            Try
                If File.Exists(fileName) = False Then Return Nothing Else Return fileName
            Catch ex As Exception
                Return Nothing
            End Try

        End Function

        ''' <summary>
        ''' Enumerate the resource directory of all of the resources of the given type in the specified module.
        ''' </summary>
        ''' <param name="hmod">Handle to a valid, open module from which to retrieve the resource directory.</param>
        ''' <param name="restype">The resource type.</param>
        ''' <returns>An array of MemPtr structures.</returns>
        ''' <remarks></remarks>
        Public Function EnumResources(hmod As IntPtr, restype As IntPtr) As ResCol

            Dim _ptrs As New ResCol
            Dim c As Integer = 0

            EnumResourceNamesEx(hmod, restype,
                        New EnumResNameProcPtr(
                            Function(hModule As IntPtr, lpszType As MemPtr, lpszName As MemPtr, lParam As IntPtr) As Boolean

                                _ptrs.Add(lpszName)
                                c += 1

                                Return True
                            End Function),
                         IntPtr.Zero, 0, 0)

            Return _ptrs

        End Function

        ''' <summary>
        ''' Returns true if the given pointer is an INTRESOURCE identifier.
        ''' </summary>
        ''' <param name="ptr"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsIntResource(ptr As IntPtr) As Boolean
            Return (ptr.ToInt64 And &HFFFFL) = ptr.ToInt64
        End Function

        ''' <summary>
        ''' Loads a string resource from the given library at the specified resource identifier location.
        ''' </summary>
        ''' <param name="fileName">Resource string containing filename and resource id.</param>
        ''' <param name="uFlags">Optional flags.</param>
        ''' <param name="parseResName">Specify whether to parse the resource id from the filename.</param>
        ''' <param name="maxBuffer">Specifies the maximum size of the return value, in bytes.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function LoadStringResource(fileName As String, Optional uFlags As LoadLibraryExFlags = LoadLibraryExFlags.LOAD_LIBRARY_AS_DATAFILE Or LoadLibraryExFlags.LOAD_LIBRARY_AS_IMAGE_RESOURCE, Optional parseResName As Boolean = False, Optional maxBuffer As Integer = 4096) As String
            LoadStringResource = LoadStringResource(fileName, 0, uFlags, True, maxBuffer)
        End Function

        ''' <summary>
        ''' Loads a string resource from the given library at the specified resource identifier location.
        ''' </summary>
        ''' <param name="fileName">Library from which to load the resource.</param>
        ''' <param name="resId">Resource identifier.</param>
        ''' <param name="uFlags">Optional flags.</param>
        ''' <param name="parseResName">Specify whether to parse the resource id from the filename.</param>
        ''' <param name="maxBuffer">Specifies the maximum size of the return value, in bytes.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function LoadStringResource(fileName As String, resId As Integer, Optional uFlags As LoadLibraryExFlags = LoadLibraryExFlags.LOAD_LIBRARY_AS_DATAFILE Or LoadLibraryExFlags.LOAD_LIBRARY_AS_IMAGE_RESOURCE, Optional parseResName As Boolean = False, Optional maxBuffer As Integer = 4096) As String
            Dim hmod As IntPtr

            If parseResName Then
                fileName = ParseResourceFilename(fileName, resId)
            Else
                fileName = ParseResourceFilename(fileName)
            End If

            Try
                hmod = LoadLibraryEx(fileName, IntPtr.Zero, uFlags)
            Catch ex As Exception
                Return Nothing
            End Try

            If hmod = IntPtr.Zero Then
                Throw New NativeException(CInt(GetLastError))
            End If

            Dim mm As MemPtr
            mm.AllocZero(maxBuffer)

            LoadString(hmod, resId, mm, maxBuffer)
            LoadStringResource = CType(mm, String)

            mm.Free()
            FreeLibrary(hmod)
        End Function

        ''' <summary>
        ''' A simple class to hold WPF-compatible icon resources.
        ''' </summary>
        ''' <remarks></remarks>
        Public Class LibraryIcon

            ''' <summary>
            ''' The name of the library icon (or the resource identifier).
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property Name As String

            ''' <summary>
            ''' The BitmapSource of the icon.
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property Image As System.Windows.Media.Imaging.BitmapSource

            ''' <summary>
            ''' Instantiate a new LibraryIcon object.
            ''' </summary>
            ''' <remarks></remarks>
            Public Sub New()

            End Sub

            ''' <summary>
            ''' Instantiate a new LibraryIcon object.
            ''' </summary>
            ''' <param name="name">Name or resource id of the icon.</param>
            ''' <param name="image">BitmapSource object of the icon.</param>
            ''' <remarks></remarks>
            Public Sub New(name As String, image As System.Windows.Media.Imaging.BitmapSource)
                _Name = name
                _Image = image
            End Sub

        End Class

        ''' <summary>
        ''' Loads all icons from a specified file.  Returns an observable collection of LibraryIcon.
        ''' </summary>
        ''' <param name="fileName">The filename from which to load the icons.</param>
        ''' <param name="desiredSize">The desired size of the icon.</param>
        ''' <param name="uFlags">Optional flags.</param>
        ''' <returns>An ObservableCollection(Of LibraryIcon)</returns>
        ''' <remarks></remarks>
        Public Function LoadAllLibraryIcons(fileName As String, desiredSize As StandardIcons, Optional uFlags As LoadLibraryExFlags = LoadLibraryExFlags.LOAD_LIBRARY_AS_DATAFILE Or LoadLibraryExFlags.LOAD_LIBRARY_AS_IMAGE_RESOURCE) As Collections.ObjectModel.ObservableCollection(Of LibraryIcon)
            fileName = Environment.ExpandEnvironmentVariables(fileName)

            Dim l As New Collections.ObjectModel.ObservableCollection(Of LibraryIcon)
            Dim hmod As IntPtr

            Try
                hmod = LoadLibraryEx(fileName, IntPtr.Zero, uFlags)
            Catch ex As Exception
                Return Nothing
            End Try

            If hmod = IntPtr.Zero Then
                Throw New NativeException
            End If

            Dim enumres As ResCol = EnumResources(hmod, CType(RT_GROUP_ICON, IntPtr))

            Dim i As Integer,
        c As Integer = enumres.Count - 1

            Dim s As String

            For i = 0 To c
                s = "#" & (i + 1)
                l.Add(New LibraryIcon(s, MakeWPFImage(_internalLoadLibraryIcon(fileName, i, Nothing, desiredSize, uFlags, enumres, False, hmod))))
            Next

            FreeLibrary(hmod)

            Return l
        End Function

        ''' <summary>
        ''' Loads an icon resource from a file.
        ''' </summary>
        ''' <param name="fileName">Icon resource string containing filename and resource id.</param>
        ''' <param name="desiredSize">The desired size of the icon in one of the standard icon sizes.</param>
        ''' <param name="uFlags">Optional flags.</param>
        ''' <param name="enumRes">Optional pre-enumerated resource directory.</param>
        ''' <returns>A managed-code .NET Framework Icon object.</returns>
        ''' <remarks></remarks>
        Public Function LoadLibraryIcon(fileName As String, desiredSize As StandardIcons, Optional uFlags As LoadLibraryExFlags = LoadLibraryExFlags.LOAD_LIBRARY_AS_DATAFILE Or LoadLibraryExFlags.LOAD_LIBRARY_AS_IMAGE_RESOURCE, Optional enumRes As ResCol = Nothing) As Icon
            LoadLibraryIcon = _internalLoadLibraryIcon(fileName, &H80000, Nothing, desiredSize, uFlags, enumRes, True, IntPtr.Zero)
        End Function

        ''' <summary>
        ''' Loads an icon resource from a file.
        ''' </summary>
        ''' <param name="fileName">Library, executable or icon file.</param>
        ''' <param name="iIcon">The icon index.  A negative value means the index is the name of the icon, a positive value indicates the absolute position in the resource table.</param>
        ''' <param name="desiredSize">The desired size of the icon in one of the standard icon sizes.</param>
        ''' <param name="uFlags">Optional flags.</param>
        ''' <param name="enumRes">Optional pre-enumerated resource directory.</param>
        ''' <returns>A managed-code .NET Framework Icon object.</returns>
        ''' <remarks></remarks>
        Public Function LoadLibraryIcon(fileName As String, iIcon As Integer, desiredSize As StandardIcons, Optional uFlags As LoadLibraryExFlags = LoadLibraryExFlags.LOAD_LIBRARY_AS_DATAFILE Or LoadLibraryExFlags.LOAD_LIBRARY_AS_IMAGE_RESOURCE, Optional enumRes As ResCol = Nothing) As Icon
            LoadLibraryIcon = _internalLoadLibraryIcon(fileName, iIcon, Nothing, desiredSize, uFlags, enumRes, False, IntPtr.Zero)
        End Function

        ''' <summary>
        ''' Loads an icon resource from a file.
        ''' </summary>
        ''' <param name="fileName">Library, executable or icon file.</param>
        ''' <param name="resIcon">The icon index string identifier.</param>
        ''' <param name="desiredSize">The desired size of the icon in one of the standard icon sizes.</param>
        ''' <param name="uFlags">Optional flags.</param>
        ''' <param name="enumRes">Optional pre-enumerated resource directory.</param>
        ''' <returns>A managed-code .NET Framework Icon object.</returns>
        ''' <remarks></remarks>
        Public Function LoadLibraryIcon(fileName As String, resIcon As String, desiredSize As StandardIcons, Optional uFlags As LoadLibraryExFlags = LoadLibraryExFlags.LOAD_LIBRARY_AS_DATAFILE Or LoadLibraryExFlags.LOAD_LIBRARY_AS_IMAGE_RESOURCE, Optional enumRes As ResCol = Nothing) As Icon
            LoadLibraryIcon = _internalLoadLibraryIcon(fileName, &H80000, resIcon, desiredSize, uFlags, enumRes, False, IntPtr.Zero)
        End Function

        ''' <summary>
        ''' Internal function to load an icon resource from a file, somehow.
        ''' </summary>
        ''' <param name="fileName">Library, executable or icon file.</param>
        ''' <param name="iIcon">The icon index to open. This parameter is ignored if resIcon is not null.</param>
        ''' <param name="resIcon">The icon index string identifier.</param>
        ''' <param name="desiredSize">The desired size of the icon in one of the standard icon sizes.</param>
        ''' <param name="uFlags">Flags.</param>
        ''' <param name="enumRes">pre-enumerated resource directory.</param>
        ''' <param name="parseIconIndex">Whether to parse the icon index directly from the filename.  The resIcon parameter is ignored if this is true.</param>
        ''' <param name="hMod">The handle to an open module or 0.</param>
        ''' <returns>A managed-code .NET Framework Icon object.</returns>
        ''' <remarks></remarks>
        Private Function _internalLoadLibraryIcon(fileName As String, iIcon As Integer, resIcon As String, desiredSize As StandardIcons, uFlags As LoadLibraryExFlags, enumRes As ResCol, parseIconIndex As Boolean, hMod As IntPtr) As Icon

            Dim lk As String = Nothing
            Dim icn As Icon = Nothing
            Dim noh As Boolean = hMod = IntPtr.Zero
            Dim idata As BITMAPINFOHEADER

            If parseIconIndex Then
                fileName = ParseResourceFilename(fileName, iIcon)
            Else
                fileName = ParseResourceFilename(fileName)
            End If

            If resIcon IsNot Nothing Then
                fileName = ParseResourceFilename(fileName)

                If resIcon.Chars(0) = "#"c AndAlso TextTools.IsNumber(resIcon.Substring(1)) Then
                    iIcon = -Integer.Parse(resIcon.Substring(1))
                End If
            Else
                fileName = ParseResourceFilename(fileName)
            End If

            If fileName Is Nothing Then Return Nothing

            lk = fileName & "," & -iIcon

            If LookupLibIcon(lk, icn) Then
                Return icn
            End If

            Dim cx As Integer
            Dim cy As Integer

            ' a shortcut.  Every value in the StandardIcons enum begins with 'Icon' and ends with a number.
            cx = CInt(Val(desiredSize.ToString.Substring(4)))
            cy = cx

            Dim hicon As IntPtr
            Dim idx As Integer = iIcon

            Dim hres As IntPtr,
        hdata As IntPtr

            Dim hglob As IntPtr

            '' if it's an actual icon file, we use our own IconImage class to read it.
            If Path.GetExtension(fileName) = ".ico" Then
                Dim fi As New IconImage(fileName)
                Dim fa As IconImageEntry = Nothing

                For Each fe In fi.Entries

                    '' find the closest size greater than the requested size image.
                    If fe.EntryInfo.wBitsPixel = 32 Then
                        '' store the image if it's bigger.
                        If fa Is Nothing OrElse fa.Width <= fe.Width Then fa = fe
                        If fe.StandardIconType = desiredSize Then Return fe.ToIcon
                    End If

                Next
                '' we're done, no need to open a resource.
                If fa IsNot Nothing Then Return fa.ToIcon Else Return Nothing
            End If

            '' open the library.
            If noh Then
                Try
                    hMod = LoadLibraryEx(fileName, IntPtr.Zero, uFlags)
                Catch ex As Exception
                    Return Nothing
                End Try

                If hMod = IntPtr.Zero Then
                    Throw New NativeException
                End If
            End If

            '' pre-initialize the return value with null.
            _internalLoadLibraryIcon = Nothing

            '' if enumRes was already passed in we don't need to do this step...
            '' also, if enumRes was passed in, it was by a particular function enumerating only RT_GROUP_ICON,
            '' so a distinction does not have to be made in that case.

            '' we want RT_GROUP_ICON resources.
            If enumRes Is Nothing Then enumRes = EnumResources(hMod, CType(RT_GROUP_ICON, IntPtr))

            '' no RT_GROUP_ICONs, they must want an actual ICON.
            If enumRes Is Nothing Then
                enumRes = EnumResources(hMod, CType(RT_ICON, IntPtr))

                '' are we looking for the integer name of the icon resource?
                If iIcon < 0 Then
                    idx = 0
                    For Each n In enumRes
                        '' looking for resources with this INTRESOURCE id
                        If n.Handle.ToInt64 = -iIcon Then
                            Exit For
                        ElseIf Not IsIntResource(n) Then
                            If n.CharAt(0) = "#"c Then
                                '' a resource with an string integer name?
                                Dim s As String = n.GetString(1)
                                Dim i As Integer

                                If Integer.TryParse(s, i) Then
                                    If i = -iIcon Then Exit For
                                End If
                            ElseIf n.ToString = resIcon Then
                                '' a plain string resource name?
                                Exit For
                            End If
                        End If
                        '' iterate the index to find the absolute position of the named resource.
                        idx += 1
                    Next
                End If

                If enumRes Is Nothing Then
                    If noh Then FreeLibrary(hMod)
                    Exit Function
                End If

                If idx > enumRes.Count Then
                    If noh Then FreeLibrary(hMod)
                    Exit Function
                End If

                '' find the icon.
                hres = FindResourceEx(hMod, New IntPtr(RT_ICON), enumRes(idx).Handle, 0)

                '' load the resource.
                hglob = LoadResource(hMod, hres)

                '' grab the memory handle.
                hdata = LockResource(hglob)

                '' Grab the raw bitmap structure from the icon resource, so we
                '' can use the actual width and height as opposed to the
                '' system-stretched return result.
                idata = CType(Marshal.PtrToStructure(hdata, GetType(BITMAPINFOHEADER)), BITMAPINFOHEADER)

                '' create the icon from the data in the resource.
                '' I read a great many articles before I finally figured out that &H30000 MUST be passed, just because.
                '' There is no specified reason.  That's simply the magic number and it won't work without it.
                hicon = CreateIconFromResourceEx(hdata, SizeofResource(hMod, hres), True, &H30000, idata.biWidth, idata.biWidth, 0)

                '' clone the unmanaged icon.
                icn = CType(Icon.FromHandle(hicon).Clone, Icon)
                '' add to the library cache
                AddToLibCache(lk, icn)

                '' set return value
                _internalLoadLibraryIcon = icn

                '' free our unmanaged resources.
                DestroyIcon(hicon)
                If noh Then FreeLibrary(hMod)

                Exit Function
            End If

            '' are we looking for the integer name of an icon resource?
            If iIcon < 0 Then
                idx = 0
                For Each n In enumRes
                    '' looking for resources with this INTRESOURCE id
                    If n.Handle.ToInt64 = -iIcon Then
                        Exit For
                    ElseIf Not IsIntResource(n) Then
                        If n.CharAt(0) = "#"c Then
                            '' a resource with an string integer name?
                            Dim s As String = n.GetString(1)
                            Dim i As Integer

                            If Integer.TryParse(s, i) Then
                                If i = -iIcon Then Exit For
                            End If
                        ElseIf n.ToString = resIcon Then
                            '' a plain string resource name?
                            Exit For
                        End If
                    End If
                    '' iterate the index to find the absolute position of the named resource.
                    idx += 1
                Next
            End If

            '' we have resources, but the index is too high.
            If idx >= enumRes.Count Then
                If noh Then FreeLibrary(hMod)
                Exit Function
            End If

            '' find the group icon resource.
            hres = FindResourceEx(hMod, New IntPtr(RT_GROUP_ICON), enumRes(idx).Handle, 0)

            If hres <> IntPtr.Zero Then
                '' load the resource.
                hglob = LoadResource(hMod, hres)

                '' grab the handle
                hdata = LockResource(hglob)

                Dim i As Integer

                '' lookup the icon by size.
                i = LookupIconIdFromDirectoryEx(hdata, True, cx, cy, 0)
                If i = 0 Then
                    If noh Then FreeLibrary(hMod)
                    Exit Function
                End If

                '' find THAT icon resource.
                hres = FindResourceEx(hMod, New IntPtr(RT_ICON), New IntPtr(i), 0)

                ''load that resource.
                hglob = LoadResource(hMod, hres)

                ''grab that handle.
                hdata = LockResource(hglob)

                '' Grab the raw bitmap structure from the icon resource, so we
                '' can use the actual width and height as opposed to the
                '' system-stretched return result.
                idata = CType(Marshal.PtrToStructure(hdata, GetType(BITMAPINFOHEADER)), BITMAPINFOHEADER)

                '' create the icon.
                hicon = CreateIconFromResourceEx(hdata, SizeofResource(hMod, hres), True, &H30000, idata.biWidth, idata.biWidth, 0)

                '' clone the unmanaged icon.

                If hicon <> IntPtr.Zero Then
                    icn = CType(Icon.FromHandle(hicon).Clone, Icon)

                    '' add to the library cache
                    AddToLibCache(lk, icn)

                    '' set return value
                    _internalLoadLibraryIcon = icn

                    '' destroy the unmanaged icon.
                    DestroyIcon(hicon)
                End If

            End If

            '' free the library.
            If noh Then FreeLibrary(hMod)

        End Function

        ''' <summary>
        ''' Gets the number of icon resources in a specified file or library.
        ''' </summary>
        ''' <param name="library"></param>
        ''' <returns>Total number of icon resources.</returns>
        ''' <remarks></remarks>
        Public Function GetLibraryIconCount(Optional library As String = "%systemroot%\system32\shell32.dll") As Integer
            Return ExtractIconEx(Environment.ExpandEnvironmentVariables(library), -1, IntPtr.Zero, IntPtr.Zero, 0)
        End Function

        ''' <summary>
        ''' Retrieves a Bitmap resource from a library.
        ''' </summary>
        ''' <param name="iBmp"></param>
        ''' <param name="iSize"></param>
        ''' <param name="uFlags"></param>
        ''' <param name="library"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetLibraryBitmap(iBmp As Integer, iSize As Size, Optional uFlags As Integer = LR_DEFAULTCOLOR + LR_CREATEDIBSECTION, Optional library As String = "%systemroot%\system32\shell32.dll") As Bitmap

            Dim hInst As IntPtr = LoadLibrary(Environment.ExpandEnvironmentVariables(library))
            Dim hBmp As IntPtr

            If hInst = IntPtr.Zero Then Return Nothing

            hBmp = LoadImage(hInst, CType(iBmp, IntPtr), IMAGE_BITMAP, iSize.Width, iSize.Height, uFlags)

            If hBmp = IntPtr.Zero Then Return Nothing
            FreeLibrary(hInst)

            GetLibraryBitmap = Bitmap.FromHbitmap(hBmp)
            DeleteObject(hBmp)

        End Function

        Private Function MakeKey(iIcon As Integer, shil As SystemIconSizes) As String
            Return $"{iIcon},{shil}"
        End Function

        Private Function GetIconIndex(filename As String) As Integer

            Dim sgfin As ShellFileGetAttributesOptions,
                sgfout As ShellFileGetAttributesOptions

            Dim lpInfo As New SHFILEINFO,
                x As IntPtr

            Dim iFlags As Integer = 0
            iFlags = iFlags Or SHGFI_SYSICONINDEX Or SHGFI_PIDL

            Dim mm As SafePtr = CType(filename, SafePtr)

            mm.Length += 2

            SHParseDisplayName(mm.handle, IntPtr.Zero, x, sgfin, sgfout)
            mm.Free()

            SHGetItemInfo(x, 0&, lpInfo, Marshal.SizeOf(lpInfo), iFlags)
            Marshal.FreeCoTaskMem(x)

            Return lpInfo.iIcon
        End Function

        ''' <summary>
        ''' Retrieves the icon for the file from the shell system image list.
        ''' </summary>
        ''' <param name="lpFilename">Filename for which to retrieve the file icon.</param>
        ''' <param name="shil">The desired size of the icon.</param>
        ''' <param name="iIndex">Receives the index of the image in the system image list.</param>
        ''' <returns>A System.Drawing.Icon image.</returns>
        ''' <remarks></remarks>
        Public Function GetFileIcon(lpFilename As String, Optional shil As SystemIconSizes = SystemIconSizes.ExtraLarge, Optional ByRef iIndex? As Integer = Nothing) As System.Drawing.Icon
            '' The shell system image list
            Dim riid As New Guid("46EB5926-582E-4017-9FDF-E8998DAA0950")
            Dim icn As Icon = Nothing
            Dim i As IntPtr

            Dim iIcon As Integer

            iIcon = If(iIndex IsNot Nothing AndAlso iIndex > 0, CInt(iIndex), GetIconIndex(lpFilename))

            If (iIcon = 0) Then Return Nothing

            Dim key = MakeKey(iIcon, shil)

            If (LookupLibIcon(key, icn)) Then
                Return icn
            End If

            SHGetImageList(shil, riid, i)

            i = CType(ImageList_GetIcon(i, iIcon, 0), IntPtr)

            If i <> IntPtr.Zero Then

                icn = CType(Icon.FromHandle(i).Clone, Icon)
                DestroyIcon(i)

                If iIndex IsNot Nothing Then
                    iIndex = iIcon
                End If

                AddToLibCache(key, icn)
                Return icn
            Else
                Return Nothing
            End If

        End Function

        ''' <summary>
        ''' Retrieves the icon for the file from the shell system image list as WPF <see cref="System.Windows.Media.Imaging.BitmapSource"/>.
        ''' </summary>
        ''' <param name="fileName">Filename for which to retrieve the file icon.</param>
        ''' <param name="shil">The desired size of the icon.</param>
        ''' <returns>A <see cref="System.Windows.Media.Imaging.BitmapSource"/> object.</returns>
        ''' <remarks></remarks>
        Public Function GetFileIconWPF(fileName As String, Optional shil As SystemIconSizes = SystemIconSizes.Large) As System.Windows.Media.Imaging.BitmapSource
            Dim iIcon As Integer

            iIcon = GetIconIndex(fileName)

            Dim bs As Windows.Media.Imaging.BitmapSource = Nothing
            Dim key = MakeKey(iIcon, shil)

            If iIcon <> 0 AndAlso Not (LookupWPFLibIcon(key, bs)) Then
                bs = MakeWPFImage(GetFileIcon(fileName, shil))
                AddToWPFLibCache(key, bs)
            End If

            Return bs
        End Function


        ''' <summary>
        ''' Gets the system imagelist cache index for the icon for the shell item pointed to by lpItemID
        ''' </summary>
        ''' <param name="lpItemID">A pointer to a SHITEMID structure.</param>
        ''' <param name="fSmall">Retrieve a small icon.</param>
        ''' <param name="hIml">Unused</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetItemIconIndex(lpItemID As IntPtr, fSmall As Boolean) As Integer

            Dim lpInfo As New SHFILEINFO,
                i As Integer,
                fFlags As Integer

            If (fSmall = False) Then
                fFlags = (SHGFI_SYSICONINDEX + SHGFI_ICON)
            Else
                fFlags = (SHGFI_SYSICONINDEX + SHGFI_SMALLICON)
            End If

            i = CInt(SHGetItemInfo(lpItemID, 0&, lpInfo, Len(lpInfo), fFlags Or SHGFI_PIDL))

            If (i <> 0&) Then
                Return lpInfo.iIcon
            End If

            Return 0
        End Function

        ''' <summary>
        ''' Gets the system imagelist cache index for the icon for the shell item pointed to by lpFilename
        ''' </summary>
        ''' <param name="lpFilename">Filename for which to retrieve the icon index.</param>
        ''' <param name="shil">Size of the icon for which to retrieve the index.</param>
        ''' <returns>The shell image list index for the file icon.</returns>
        ''' <remarks></remarks>
        Public Function GetFileIconIndex(lpFilename As String, Optional shil As SystemIconSizes = SystemIconSizes.ExtraLarge) As Integer
            Dim lpInfo As New SHFILEINFO
            Dim iFlags As Integer = 0

            iFlags = iFlags Or SHGFI_SYSICONINDEX

            Dim mm As SafePtr = CType(lpFilename, SafePtr)
            mm.Length += Len("A"c)

            SHGetItemInfo(mm.handle, 0&, lpInfo, Marshal.SizeOf(lpInfo), iFlags)
            Return lpInfo.iIcon
        End Function

        ''' <summary>
        ''' Retrieves a pointer to a system image list.
        ''' </summary>
        ''' <param name="shil">The image size for the image list to retrieve.</param>
        ''' <returns>A pointer to a system image list for images of the specified size.</returns>
        ''' <remarks></remarks>
        Public Function GetSystemImageList(shil As SystemIconSizes) As IntPtr
            Dim lpInfo As New SHFILEINFO,
        i As IntPtr
            Dim riid As New Guid("46EB5926-582E-4017-9FDF-E8998DAA0950")
            'iFlags = SHGFI_ICON

            SHGetImageList(shil, riid, i)
            Return i
        End Function

        ''' <summary>
        ''' Retrieves a file icon from its index in the system image list.
        ''' </summary>
        ''' <param name="index">Index of icon to retrieve.</param>
        ''' <param name="shil">Size of icon to retrieve.</param>
        ''' <returns>A System.Drawing.Icon object.</returns>
        ''' <remarks></remarks>
        Public Function GetFileIconFromIndex(index As Integer, Optional shil As SystemIconSizes = SystemIconSizes.ExtraLarge) As System.Drawing.Icon
            Dim lpInfo As New SHFILEINFO,
        i As IntPtr
            Dim icn As Icon
            Dim riid As New Guid("46EB5926-582E-4017-9FDF-E8998DAA0950")
            'iFlags = SHGFI_ICON
            Dim iFlags As Integer = 0
            iFlags = iFlags Or SHGFI_SYSICONINDEX

            SHGetImageList(shil, riid, i)
            i = CType(ImageList_GetIcon(i, index, 0), IntPtr)

            If i <> IntPtr.Zero Then
                icn = CType(Icon.FromHandle(i).Clone, Icon)
                DestroyIcon(i)
                Return icn
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Retrieves the icon for the specified shell item handle.
        ''' </summary>
        ''' <param name="lpItemID">A pointer to a SHITEMID structure.</param>
        ''' <param name="shil">The size of the icon to retrieve.</param>
        ''' <returns>A System.Drawing.Icon object.</returns>
        ''' <remarks></remarks>
        Public Function GetItemIcon(lpItemID As IntPtr, Optional shil As SystemIconSizes = SystemIconSizes.ExtraLarge) As System.Drawing.Icon
            Dim lpInfo As New SHFILEINFO,
        i As IntPtr
            Dim icn As Icon
            Dim riid As New Guid("46EB5926-582E-4017-9FDF-E8998DAA0950")
            'iFlags = SHGFI_ICON
            Dim iFlags As Integer = 0
            iFlags = iFlags Or SHGFI_SYSICONINDEX Or SHGFI_PIDL

            i = SHGetItemInfo(lpItemID, 0&, lpInfo, Marshal.SizeOf(lpInfo), iFlags)

            If lpInfo.iIcon = 0 Then
                Return Nothing
            End If

            SHGetImageList(shil, riid, i)

            i = CType(ImageList_GetIcon(i, lpInfo.iIcon, 0), IntPtr)

            If i <> IntPtr.Zero Then
                icn = CType(Icon.FromHandle(i).Clone, Icon)

                DestroyIcon(i)

                Return icn
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Gray out an icon.
        ''' </summary>
        ''' <param name="icn">The input icon.</param>
        ''' <returns>The grayed out icon.</returns>
        ''' <remarks></remarks>
        Public Function GrayIcon(icn As Icon) As Image
            Dim n As Bitmap = New Bitmap(icn.Width, icn.Height, Imaging.PixelFormat.Format32bppArgb)
            Dim g As Graphics = Graphics.FromImage(n)

            g.FillRectangle(Brushes.Transparent, New Rectangle(0, 0, n.Width, n.Height))
            g.DrawIcon(icn, 0, 0)
            g.Dispose()

            Dim bm As New Imaging.BitmapData,
                mm As New MemPtr(n.Width * n.Height * 4)

            bm.Stride = n.Width * 4
            bm.Scan0 = mm
            bm.PixelFormat = Imaging.PixelFormat.Format32bppArgb
            bm.Width = n.Width
            bm.Height = n.Height

            bm = n.LockBits(New Rectangle(0, 0, n.Width, n.Height), Imaging.ImageLockMode.ReadWrite Or Imaging.ImageLockMode.UserInputBuffer, Imaging.PixelFormat.Format32bppArgb, bm)

            Dim i As Integer,
                c As Integer

            'Dim b() As Byte

            'ReDim b((bm.Stride * bm.Height) - 1)
            'MemCpy(b, bm.Scan0, bm.Stride * bm.Height)
            c = (bm.Stride * bm.Height) - 1

            Dim stp As Integer = CInt(bm.Stride / bm.Width)

            'For i = 3 To c Step stp
            '    If b(i) > &H7F Then b(i) = &H7F
            'Next

            For i = 3 To c Step stp
                If mm.ByteAt(i) > &H7F Then mm.ByteAt(i) = &H7F
            Next

            'MemCpy(bm.Scan0, b, bm.Stride * bm.Height)
            n.UnlockBits(bm)
            mm.Free()
            Return n
        End Function

        ''' <summary>
        ''' Create a Device Independent Bitmap from an icon.
        ''' </summary>
        ''' <param name="icn"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function DIBFromIcon(icn As Icon) As IntPtr
            Dim bmp As Bitmap = IconToTransparentBitmap(icn)
            Return MakeDIBSection(bmp)
        End Function


        Public Function MakeWPFImage(img As Icon) As System.Windows.Media.Imaging.BitmapSource
            Dim _d As IntPtr
            Return MakeWPFImage(img, _d)
        End Function

        Public Function MakeWPFImage(img As Bitmap) As System.Windows.Media.Imaging.BitmapSource
            Dim _d As IntPtr
            Return MakeWPFImage(img, _d)
        End Function

        ''' <summary>
        ''' Creates a WPF BitmapSource from a Bitmap.
        ''' </summary>
        ''' <param name="img">The <see cref="System.Drawing.Icon"/> object to convert.</param>
        ''' <param name="bitPtr">Set this to zero.</param>
        ''' <param name="dpiX">The X DPI to use to create the new image (default is 96.0)</param>
        ''' <param name="dpiY">The Y DPI to use to create the new image (default is 96.0)</param>
        ''' <param name="createOnApplicationThread"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function MakeWPFImage(img As Icon, ByRef bitPtr As IntPtr, Optional dpiX As Double = 96.0#, Optional dpiY As Double = 96.0#, Optional createOnApplicationThread As Boolean = True) As System.Windows.Media.Imaging.BitmapSource
            Return MakeWPFImage(IconToTransparentBitmap(img), bitPtr, dpiX, dpiY, createOnApplicationThread)
        End Function

        ''' <summary>
        ''' Creates a WPF BitmapSource from a Bitmap.
        ''' </summary>
        ''' <param name="img">The <see cref="System.Drawing.Image"/> object to convert.</param>
        ''' <param name="bitPtr">Set this to zero.</param>
        ''' <param name="dpiX">The X DPI to use to create the new image (default is 96.0)</param>
        ''' <param name="dpiY">The Y DPI to use to create the new image (default is 96.0)</param>
        ''' <param name="createOnApplicationThread"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function MakeWPFImage(img As System.Drawing.Bitmap, ByRef bitPtr As IntPtr, Optional dpiX As Double = 96.0#, Optional dpiY As Double = 96.0#, Optional createOnApplicationThread As Boolean = True) As System.Windows.Media.Imaging.BitmapSource

            If img Is Nothing Then Return Nothing
            If img.Width = 0 Then Return Nothing

            Dim BytesPerRow As Integer = CInt((((img.Width * 32 + 31) And (Not 31)) / 8))
            Dim size As Integer = img.Height * BytesPerRow

            Dim bm As New Imaging.BitmapData
            Dim bmp As System.Windows.Media.Imaging.BitmapSource = Nothing

            Try

                bm = img.LockBits(New Rectangle(0, 0, img.Width, img.Height), Imaging.ImageLockMode.ReadWrite, Imaging.PixelFormat.Format32bppPArgb, bm)

                If createOnApplicationThread AndAlso System.Windows.Application.Current IsNot Nothing Then

                    System.Windows.Application.Current.Dispatcher.Invoke(
                Sub()
                    bmp = System.Windows.Media.Imaging.BitmapSource.Create(
                         img.Width,
                         img.Height,
                         dpiX,
                         dpiY,
                         System.Windows.Media.PixelFormats.Bgra32,
                         Nothing, bm.Scan0, size, BytesPerRow)

                End Sub)
                Else
                    bmp = System.Windows.Media.Imaging.BitmapSource.Create(
                 img.Width,
                 img.Height,
                 dpiX,
                 dpiY,
                 System.Windows.Media.PixelFormats.Bgra32,
                 Nothing, bm.Scan0, size, BytesPerRow)

                End If
            Catch ex As Exception

                If bm IsNot Nothing Then img.UnlockBits(bm)
                MsgBox("Error: " & ex.Message)

                Return Nothing

            End Try

            img.UnlockBits(bm)
            Return bmp

        End Function

        ''' <summary>
        ''' Creates a System.Drawing.Bitmap image from a WPF source.
        ''' </summary>
        ''' <param name="source"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function MakeBitmapFromWPF(source As System.Windows.Media.Imaging.BitmapSource) As System.Drawing.Bitmap

            Dim mm As New CoreCT.Memory.MemPtr
            Dim bmp As System.Drawing.Bitmap = Nothing

            If System.Windows.Application.Current IsNot Nothing Then

                System.Windows.Application.Current.Dispatcher.Invoke(
            Sub()
                bmp = New System.Drawing.Bitmap(source.PixelWidth, source.PixelHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb)
                mm.Alloc(bmp.Width * bmp.Height * 4)

                Dim bm As New System.Drawing.Imaging.BitmapData

                bm.Scan0 = mm.Handle
                bm.Stride = bmp.Width * 4

                bm = bmp.LockBits(New System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite Or System.Drawing.Imaging.ImageLockMode.UserInputBuffer, System.Drawing.Imaging.PixelFormat.Format32bppArgb, bm)
                source.CopyPixels(System.Windows.Int32Rect.Empty, mm.Handle, CInt(mm.Length), bmp.Width * 4)

                bmp.UnlockBits(bm)
                mm.Free()
            End Sub)
            Else
                bmp = New System.Drawing.Bitmap(source.PixelWidth, source.PixelHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb)
                mm.Alloc(bmp.Width * bmp.Height * 4)

                Dim bm As New System.Drawing.Imaging.BitmapData

                bm.Scan0 = mm.Handle
                bm.Stride = bmp.Width * 4

                bm = bmp.LockBits(New System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite Or System.Drawing.Imaging.ImageLockMode.UserInputBuffer, System.Drawing.Imaging.PixelFormat.Format32bppArgb, bm)
                source.CopyPixels(System.Windows.Int32Rect.Empty, mm.Handle, CInt(mm.Length), bmp.Width * 4)

                bmp.UnlockBits(bm)
                mm.Free()
            End If

            Return bmp
        End Function

        ''' <summary>
        ''' Create a writable device-independent bitmap from the specified image.
        ''' </summary>
        ''' <param name="img">Image to copy.</param>
        ''' <param name="bitPtr">Optional variable to receive a pointer to the bitmap bits.</param>
        ''' <returns>A new DIB handle that must be destroyed with DeleteObject().</returns>
        ''' <remarks></remarks>
        Public Function MakeDIBSection(img As Bitmap, Optional ByRef bitPtr As IntPtr = Nothing) As IntPtr
            ' Build header.
            ' adapted from C++ code examples.

            Dim wBitsPerPixel As Short = 32
            Dim BytesPerRow As Integer = CInt(((img.Width * wBitsPerPixel + 31) And (Not 31L)) / 8L)
            Dim size As Integer = img.Height * BytesPerRow
            Dim bmpInfo As BITMAPINFO
            Dim mm As New MemPtr
            Dim bmpSizeOf As Integer = Marshal.SizeOf(bmpInfo)
            mm.ReAlloc(bmpSizeOf + size)
            Dim pbmih As BITMAPINFOHEADER

            pbmih.biSize = Marshal.SizeOf(pbmih)
            pbmih.biWidth = img.Width
            pbmih.biHeight = img.Height '' positive indicates bottom-up DIB
            pbmih.biPlanes = 1
            pbmih.biBitCount = wBitsPerPixel
            pbmih.biCompression = BI_RGB
            pbmih.biSizeImage = size
            pbmih.biXPelsPerMeter = CInt(24.5 * 1000) ' pixels per meter! And these values MUST be correct if you want to pass a DIB to a native menu.
            pbmih.biYPelsPerMeter = CInt(24.5 * 1000) ' pixels per meter!
            pbmih.biClrUsed = 0
            pbmih.biClrImportant = 0

            Dim pPixels As IntPtr = IntPtr.Zero
            Dim DIB_RGB_COLORS As Integer = 0

            Marshal.StructureToPtr(pbmih, mm.Handle, False)
            Dim hPreviewBitmap As IntPtr = CreateDIBSection(IntPtr.Zero, mm.Handle, CUInt(DIB_RGB_COLORS), pPixels, IntPtr.Zero, 0)

            bitPtr = pPixels

            Dim bm As New Imaging.BitmapData

            bm = img.LockBits(New Rectangle(0, 0, img.Width, img.Height), Imaging.ImageLockMode.ReadWrite, Imaging.PixelFormat.Format32bppPArgb, bm)

            Dim pCurrSource As IntPtr = bm.Scan0

            '' Our DIBsection is bottom-up...start at the bottom row...
            Dim pCurrDest As IntPtr = pPixels + ((img.Width - 1) * BytesPerRow)
            '' ... and work our way up
            Dim DestinationStride As Integer = -BytesPerRow

            For curY As Integer = 0 To img.Height - 1
                NativeLib.Native.MemCpy(pCurrSource, pCurrDest, BytesPerRow)
                pCurrSource += bm.Stride
                pCurrDest += DestinationStride
            Next

            ' Free up locked buffer.
            img.UnlockBits(bm)
            Return hPreviewBitmap

        End Function

        ''' <summary>
        ''' Highlight the specified icon with the specified color.
        ''' </summary>
        ''' <param name="icn">Input icon.</param>
        ''' <param name="liteColor">Highlight base color.</param>
        ''' <returns>A new highlighted Image object.</returns>
        ''' <remarks></remarks>
        Public Function HiliteIcon(icn As Icon, liteColor As Color) As Image
            Dim n As Bitmap = New Bitmap(icn.Width, icn.Height, Imaging.PixelFormat.Format32bppArgb)
            Dim g As Graphics = Graphics.FromImage(n)
            Dim lc As Integer = (liteColor.ToArgb)

            g.FillRectangle(Brushes.Transparent, New Rectangle(0, 0, n.Width, n.Height))
            g.DrawIcon(icn, 0, 0)
            g.Dispose()

            Dim bm As New Imaging.BitmapData

            n.LockBits(New Rectangle(0, 0, n.Width, n.Height), Imaging.ImageLockMode.ReadWrite, Imaging.PixelFormat.Format32bppArgb, bm)

            Dim b() As Integer
            Dim i As Integer,
        c As Integer
            ReDim b((bm.Width * bm.Height) - 1)

            '' take the unmanaged memory and make it something manageable and VB-like.

            Marshal.Copy(bm.Scan0, b, 0, bm.Stride * bm.Height)
            'NativeLib.Native.MemCpy(bm.Scan0, b, bm.Stride * bm.Height)

            c = b.Length - 1

            Dim stp As Integer = CInt(bm.Stride / bm.Width)
            Dim hsv As New HSVDATA,
                hsv2 As New HSVDATA

            'convert the color to HSV.
            ColorMath.ColorToHSV(liteColor, hsv)

            For i = 0 To c
                If b(i) = 0 Then Continue For

                ColorMath.ColorToHSV(Color.FromArgb(b(i)), hsv2)

                hsv2.Hue = hsv.Hue
                hsv2.Saturation = hsv.Saturation
                hsv2.Value *= 1.1

                b(i) = ColorMath.HSVToColor(hsv2)
            Next

            Marshal.Copy(b, 0, bm.Scan0, bm.Stride * bm.Height)
            n.UnlockBits(bm)

            Return n

        End Function

        ''' <summary>
        ''' Converts a 32 bit icon into a 32 bit Argb transparent bitmap.
        ''' </summary>
        ''' <param name="icn">Input icon.</param>
        ''' <returns>A 32-bit Argb Bitmap object.</returns>
        ''' <remarks></remarks>
        Public Function IconToTransparentBitmap(icn As Icon) As Bitmap
            If icn Is Nothing Then Return Nothing

            Dim n As Bitmap = New Bitmap(icn.Width, icn.Height, Imaging.PixelFormat.Format32bppArgb)
            Dim g As Graphics = Graphics.FromImage(n)
            g.InterpolationMode = Drawing2D.InterpolationMode.Bicubic
            g.SmoothingMode = Drawing2D.SmoothingMode.None
            g.Clear(Color.Transparent)
            g.DrawIcon(icn, 0, 0)
            g.Dispose()
            Return n
        End Function

#End Region '' Icons, Images and Resources

    End Module

End Namespace