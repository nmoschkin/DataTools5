
Imports System.IO
Imports DataTools.Interop.Native
Imports System.Collections.ObjectModel
Imports System.Collections
Imports CoreCT.Memory
Imports System.Runtime.InteropServices.ComTypes
Imports System.Windows.Media.Imaging
Imports System.ComponentModel
Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices

Namespace Desktop

    ''' <summary>
    ''' Provides a file-system-locked object to represent the contents of a directory.
    ''' </summary>
    Public Class DirectoryObject
        Implements ICollection(Of ISimpleShellItem), ISimpleShellItem

        Private _SysInterface As IShellFolder

        Private _Children As New List(Of ISimpleShellItem)
        Private _Folders As New List(Of ISimpleShellItem)

        Private _DisplayName As String

        Private _Icon As System.Drawing.Icon
        Private _IconImage As Windows.Media.Imaging.BitmapSource
        Private _IconSize As StandardIcons = StandardIcons.Icon48
        Private _IsSpecial As Boolean
        Private _Parent As ISimpleShellItem
        Private _Path As String

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        Public Shared Function CreateRootView(Optional iconSize As StandardIcons = StandardIcons.Icon48) As ISimpleShellItem

            Dim d As New DirectoryObject()

            ''d.Add(New DirectoryObject("QuickAccessFolder", True, True))
            d.Add(New DirectoryObject("MyComputerFolder", True, True, iconSize))
            d.Add(New DirectoryObject("NetworkPlacesFolder", True, False, iconSize))
            ''d.Add(New DirectoryObject("ControlPanelFolder", True, True))

            Return d
        End Function

        Public Sub OnPropertyChanged(<CallerMemberName> Optional propertyName As String = Nothing)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
        End Sub

        Private Sub New()
            DisplayName = ""
        End Sub

        ''' <summary>
        ''' Create a new file-system-linked directory object 
        ''' </summary>
        ''' <param name="path"></param>
        Public Sub New(parsingName As String, Optional isSpecial As Boolean = False, Optional initialize As Boolean = True, Optional iconSize As StandardIcons = StandardIcons.Icon48)

            If (String.IsNullOrEmpty(parsingName)) Then
                Throw New ArgumentNullException(NameOf(Path), "Path is null or not found.")
            End If

            _IsSpecial = isSpecial
            _IconSize = iconSize

            'Dim mm As New MemPtr
            '    Dim res As HResult = SHCreateItemFromParsingName(path, Nothing, Guid.Parse(ShellIIDGuid.IShellItem), shitem)
            '    Dim fp As String = Nothing
            '    Dim parts As String() = Nothing

            'If (res = HResult.Ok) Then
            '    shitem.GetDisplayName(ShellItemDesignNameOptions.DesktopAbsoluteParsing, mm)

            '    fp = mm
            '    ParsingName = fp

            '    mm.CoTaskMemFree()
            'ElseIf (path.Substring(0, 2) = "::") Then
            '    parts = path.Split("\")

            '    res = SHCreateItemFromParsingName(parts(parts.Length - 1), Nothing, Guid.Parse(ShellIIDGuid.IShellItem), shitem)
            '    If res = HResult.Ok Then
            '        shitem.GetDisplayName(ShellItemDesignNameOptions.DesktopAbsoluteParsing, mm)

            '        fp = mm
            '        ParsingName = fp

            '        mm.CoTaskMemFree()
            '    End If
            'End If

            'res = SHCreateItemFromParsingName("shell:" + If(parts IsNot Nothing, parts(parts.Length - 1), If(fp, path)), Nothing, Guid.Parse(ShellIIDGuid.IShellItem), shitem)

            'If (res = HResult.Ok) Then
            '    Dim ip As IntPtr

            '    res = shitem.GetDisplayName(ShellItemDesignNameOptions.DesktopAbsoluteParsing, ip)
            '    mm = ip

            '    If (res = HResult.Ok) Then
            '        CanonicalName = If(fp Is Nothing, "shell:" + path, mm.ToString())
            '        If (ParsingName Is Nothing) Then ParsingName = mm

            '        _Path = ParsingName

            '        mm.CoTaskMemFree()
            '        shitem.GetDisplayName(ShellItemDesignNameOptions.Normal, mm)

            '        DisplayName = mm
            '        mm.CoTaskMemFree()
            '    End If
            'End If


            'shitem = Nothing

            If (_IsSpecial) Then
                '' let's see if we can parse it.
                Dim shitem As IShellItem = Nothing

                Dim mm As New MemPtr
                Dim res As HResult = SHCreateItemFromParsingName(parsingName, IntPtr.Zero, Guid.Parse(ShellIIDGuid.IShellItem), shitem)
                Dim fp As String = Nothing

                If (res = HResult.Ok) Then
                    shitem.GetDisplayName(ShellItemDesignNameOptions.DesktopAbsoluteParsing, mm)

                    fp = CStr(mm)

                    Me.ParsingName = fp
                    CanonicalName = fp

                    mm.CoTaskMemFree()

                    shitem.GetDisplayName(ShellItemDesignNameOptions.Normal, mm)

                    DisplayName = CStr(mm)
                    mm.CoTaskMemFree()

                    _IsSpecial = True

                    If initialize Then
                        Refresh(_IconSize)
                    Else
                        _Folders.Add(New DirectoryObject())
                        OnPropertyChanged("Folders")
                    End If

                    Return
                End If

                res = SHCreateItemFromParsingName("shell:" + If(fp, parsingName), IntPtr.Zero, Guid.Parse(ShellIIDGuid.IShellItem), shitem)

                If (res = HResult.Ok) Then
                    shitem.GetDisplayName(ShellItemDesignNameOptions.DesktopAbsoluteParsing, mm)

                    CanonicalName = CStr(mm)
                    If (Me.ParsingName Is Nothing) Then Me.ParsingName = CStr(mm)

                    _Path = Me.ParsingName

                    mm.CoTaskMemFree()
                    shitem.GetDisplayName(ShellItemDesignNameOptions.Normal, mm)

                    DisplayName = CStr(mm)
                    mm.CoTaskMemFree()
                End If

                shitem = Nothing

                If (Not String.IsNullOrEmpty(DisplayName) AndAlso Not String.IsNullOrEmpty(parsingName)) Then
                    _IsSpecial = True

                    If initialize Then
                        Refresh(_IconSize)
                    Else
                        _Folders.Add(New DirectoryObject())
                        OnPropertyChanged("Folders")
                    End If

                    Return
                End If

            End If

            If (Not String.IsNullOrEmpty(Me.ParsingName)) Then
                _Path = Me.ParsingName
            Else
                _Path = parsingName
                Me.ParsingName = parsingName
            End If

            If IO.Directory.Exists(_Path) = False Then
                Return
                'Throw New DirectoryNotFoundException("Directory Not Found: " & _Path)
            Else
                DisplayName = System.IO.Path.GetFileName(_Path)
                CanonicalName = _Path
                parsingName = _Path
            End If

            If initialize Then
                Refresh(_IconSize)
            Else
                _Folders.Add(New DirectoryObject())
                OnPropertyChanged("Folders")
            End If
        End Sub

        ''' <summary>
        ''' Gets or sets the directory attributes.
        ''' </summary>
        ''' <returns></returns>
        Public Property Attributes As IO.FileAttributes Implements ISimpleShellItem.Attributes
            Get
                Return GetFileAttributes(ParsingName)
            End Get
            Set(value As IO.FileAttributes)
                SetFileAttributes(ParsingName, value)
            End Set
        End Property

        ''' <summary>
        ''' Gets the canonical name of a known folder
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property CanonicalName As String

        ''' <summary>
        ''' Get or set the creation time of the directory.
        ''' </summary>
        ''' <returns></returns>
        Public Property CreationTime As Date Implements ISimpleShellItem.CreationTime
            Get
                Dim c As Date, a As Date, m As Date

                GetFileTime(ParsingName, c, a, m)
                Return c
            End Get
            Set(value As Date)
                Dim c As Date, a As Date, m As Date

                GetFileTime(ParsingName, c, a, m)
                SetFileTime(ParsingName, value, a, m)
            End Set
        End Property

        ''' <summary>
        ''' Returns the full path of the directory.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Directory As String
            Get
                Return _Path
            End Get
        End Property

        ''' <summary>
        ''' Gets the display name of the folder
        ''' </summary>
        ''' <returns></returns>
        Public Property DisplayName As String Implements ISimpleShellItem.DisplayName
            Get
                Return _DisplayName
            End Get
            Set(value As String)
                _DisplayName = value
            End Set
        End Property

        ''' <summary>
        ''' Returns a Windows Forms compatible icon for the directory
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Icon As System.Drawing.Icon
            Get
                If _Icon Is Nothing Then
                    _Icon = GetFileIcon(ParsingName, FileObject.StandardToSystem(_IconSize))
                End If

                Return _Icon
            End Get
        End Property

        ''' <summary>
        ''' Returns a WPF-compatible icon image for the directory
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property IconImage As Windows.Media.Imaging.BitmapSource Implements ISimpleShellItem.Icon
            Get
                If _IconImage Is Nothing Then
                    _IconImage = GetFileIconWPF(ParsingName, FileObject.StandardToSystem(_IconSize))
                End If

                Return _IconImage
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets the default icon size for the directory.
        ''' Individual files can override this setting, but they will be reset if this setting is changed while this directory is the parent of the file.
        ''' </summary>
        ''' <returns></returns>
        Public Property IconSize As StandardIcons Implements ISimpleShellItem.IconSize
            Get
                Return _IconSize
            End Get
            Set(value As StandardIcons)
                If _IconSize = value AndAlso _Icon IsNot Nothing AndAlso _IconImage IsNot Nothing Then Return

                _IconSize = value
                _Icon = GetFileIcon(ParsingName, FileObject.StandardToSystem(_IconSize))
                _IconImage = GetFileIconWPF(ParsingName, FileObject.StandardToSystem(_IconSize))

                If _Children IsNot Nothing AndAlso _Children.Count > 0 Then
                    For Each f In _Children
                        f.IconSize = _IconSize
                    Next
                End If
            End Set
        End Property

        Private ReadOnly Property IsFolder As Boolean = True Implements ISimpleShellItem.IsFolder

        ''' <summary>
        ''' Returns whether or not this directory is a special folder
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property IsSpecial As Boolean Implements ISimpleShellItem.IsSpecial
            Get
                Return _IsSpecial
            End Get
        End Property

        ''' <summary>
        ''' Get or set the last access time of the directory.
        ''' </summary>
        ''' <returns></returns>
        Public Property LastAccessTime As Date Implements ISimpleShellItem.LastAccessTime
            Get
                Dim c As Date, a As Date, m As Date

                GetFileTime(ParsingName, c, a, m)
                Return a
            End Get
            Set(value As Date)
                Dim c As Date, a As Date, m As Date

                GetFileTime(ParsingName, c, a, m)
                SetFileTime(ParsingName, c, value, m)
            End Set
        End Property

        ''' <summary>
        ''' Get or set the last write time of the directory.
        ''' </summary>
        ''' <returns></returns>
        Public Property LastWriteTime As Date Implements ISimpleShellItem.LastWriteTime
            Get
                Dim c As Date, a As Date, m As Date

                GetFileTime(ParsingName, c, a, m)
                Return m
            End Get
            Set(value As Date)
                Dim c As Date, a As Date, m As Date

                GetFileTime(ParsingName, c, a, m)
                SetFileTime(ParsingName, c, a, value)
            End Set
        End Property

        ''' <summary>
        ''' Returns the parent directory object if one exists.
        ''' </summary>
        ''' <returns></returns>
        Public Property Parent As ISimpleShellItem Implements ISimpleShellItem.Parent
            Get
                Return _Parent
            End Get
            Friend Set(value As ISimpleShellItem)
                _Parent = value
            End Set
        End Property

        ''' <summary>
        ''' Gets the shell parsing name of a special folder
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property ParsingName As String Implements ISimpleShellItem.ParsingName

        Private ReadOnly Property Children As ICollection(Of ISimpleShellItem) Implements ISimpleShellItem.Children
            Get
                Return Me
            End Get
        End Property

        Public ReadOnly Property Folders As ICollection(Of ISimpleShellItem) Implements ISimpleShellItem.Folders
            Get
                Return _Folders
            End Get
        End Property

        ''' <summary>
        ''' Not implemented in directory
        ''' </summary>
        ''' <returns></returns>
        Private ReadOnly Property Size As Long Implements ISimpleShellItem.Size
            Get
                Throw New NotImplementedException
            End Get
        End Property

        ''' <summary>
        ''' Get a directory object from a file object.
        ''' The <see cref="FileObject"/> instance passed is retained in the returned <see cref="DirectoryObject"/> object.
        ''' </summary>
        ''' <param name="fileObj">A <see cref="FileObject"/></param>
        ''' <returns>A <see cref="DirectoryObject"/>.</returns>
        Public Shared Function FromFileObject(fileObj As FileObject) As DirectoryObject
            Dim dir As New DirectoryObject(fileObj.Directory, fileObj.IsSpecial)
            Return dir
        End Function

        ''' <summary>
        ''' Refresh the contents of the directory.
        ''' </summary>
        Public Sub Refresh(Optional iconSize As StandardIcons? = Nothing) Implements ISimpleShellItem.Refresh
            If (iconSize Is Nothing) Then iconSize = _IconSize

            _Children.Clear()
            _Folders.Clear()

            Dim fobj As FileObject
            Dim dobj As DirectoryObject

            Dim shitem As IShellItem = Nothing
            Dim shfld As IShellFolder2 = Nothing
            Dim enumer As IEnumIDList = Nothing

            Dim mm As New MemPtr
            Dim mm2 As New MemPtr

            Dim mc As New STRRET
            Dim fp As String
            Dim objret As Object = Nothing

            Dim pname = ParsingName
            If pname IsNot Nothing AndAlso (pname.LastIndexOf("\") = pname.Length - 1) Then pname = pname.Substring(0, pname.Length - 1)

            Dim res As HResult = SHCreateItemFromParsingName(ParsingName, IntPtr.Zero, Guid.Parse(ShellIIDGuid.IShellItem), shitem)

            _IconSize = CType(iconSize, StandardIcons)
            _Icon = GetFileIcon(ParsingName, FileObject.StandardToSystem(_IconSize))
            _IconImage = GetFileIconWPF(ParsingName, FileObject.StandardToSystem(_IconSize))

            If (res = HResult.Ok) Then

                shitem.BindToHandler(IntPtr.Zero, Guid.Parse(ShellBHIDGuid.ShellFolderObject), Guid.Parse(ShellIIDGuid.IShellFolder2), objret)
                shfld = CType(objret, IShellFolder2)

                _SysInterface = shfld
                shfld.EnumObjects(IntPtr.Zero, ShellFolderEnumerationOptions.Folders Or
                                  ShellFolderEnumerationOptions.IncludeHidden Or
                                  ShellFolderEnumerationOptions.NonFolders Or
                                  ShellFolderEnumerationOptions.InitializeOnFirstNext, enumer)

                If (enumer IsNot Nothing) Then

                    Dim glist As New List(Of String)
                    Dim cf As UInteger
                    Dim x As IntPtr = IntPtr.Zero
                    Dim pout As String

                    'mm.AllocCoTaskMem((MAX_PATH * 2) + 8)

                    mm2.Alloc((MAX_PATH * 2) + 8)

                    Do
                        cf = 0

                        mm2.ZeroMemory(0, (MAX_PATH * 2) + 8)
                        res = enumer.Next(1, x, cf)
                        mm = x

                        If (cf = 0) Then Exit Do
                        If (res <> HResult.Ok) Then Exit Do

                        mm2.IntAt(0) = 2

                        'shfld.GetAttributesOf(1, mm, attr)
                        shfld.GetDisplayNameOf(mm, ShellItemDesignNameOptions.ParentRelativeParsing, mm2)

                        Dim inv As MemPtr

                        If (IntPtr.Size = 4) Then
                            inv = CType(mm2.IntAt(1), IntPtr)
                        Else
                            inv = CType(mm2.LongAt(1), IntPtr)
                        End If

                        If (inv.Handle <> IntPtr.Zero) Then

                            If (inv.CharAt(0) <> Chr(0)) Then
                                fp = CStr(inv)
                                Dim lpInfo As New SHFILEINFO

                                'Dim sgfin As ShellFileGetAttributesOptions = 0,
                                '    sgfout As ShellFileGetAttributesOptions = 0

                                Dim iFlags As Integer = SHGFI_PIDL Or SHGFI_ATTRIBUTES
                                lpInfo.dwAttributes = 0

                                x = SHGetItemInfo(mm.Handle, 0&, lpInfo, Marshal.SizeOf(lpInfo), iFlags)

                                If (ParsingName IsNot Nothing) Then
                                    If (pname.LastIndexOf("\") = pname.Length - 1) Then pname = pname.Substring(0, pname.Length - 1)
                                    pout = $"{pname}\{fp}"
                                Else
                                    pout = fp
                                End If

                                If (lpInfo.dwAttributes = 0) Then
                                    lpInfo.dwAttributes = Utility.GetFileAttributes(pout)
                                End If

                                Dim drat As FileAttributes = CType(lpInfo.dwAttributes, FileAttributes)

                                If ((lpInfo.dwAttributes And FileAttributes.Directory) = FileAttributes.Directory) AndAlso Not System.IO.File.Exists(pout) Then

                                    dobj = New DirectoryObject(pout, _IsSpecial, False)

                                    dobj.Parent = Me
                                    dobj.IconSize = _IconSize

                                    _Children.Add(dobj)
                                    _Folders.Add(dobj)
                                Else
                                    fobj = New FileObject(pout, _IsSpecial, True, _IconSize)

                                    fobj.Parent = Me
                                    fobj.IconSize = _IconSize

                                    _Children.Add(fobj)
                                End If
                            End If

                            inv.CoTaskMemFree()
                        End If

                        mm.CoTaskMemFree()
                    Loop While res = HResult.Ok

                    mm2.Free()
                End If

            End If

            OnPropertyChanged("Children")
            OnPropertyChanged("Folders")
            OnPropertyChanged("Icon")
            OnPropertyChanged("IconImage")
            OnPropertyChanged("IconSize")
            OnPropertyChanged("ParsingName")
            OnPropertyChanged("DisplayName")

        End Sub

        Friend ReadOnly Property SysInterface As IShellFolder
            Get
                Return _SysInterface
            End Get
        End Property

#Region "Collection Implementation"

        ''' <summary>
        ''' Returns the number of files in the directory.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Count As Integer Implements ICollection(Of ISimpleShellItem).Count
            Get
                Return _Children.Count
            End Get
        End Property

        Private ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of ISimpleShellItem).IsReadOnly
            Get
                Return True
            End Get
        End Property

        ''' <summary>
        ''' Returns an item in the collection.
        ''' </summary>
        ''' <param name="index"></param>
        ''' <returns></returns>
        Default Public ReadOnly Property Item(index As Integer) As ISimpleShellItem
            Get
                Return _Children(index)
            End Get
        End Property
        Public Function Contains(name As String, Optional useParsingName As Boolean = False, Optional ByRef obj As ISimpleShellItem = Nothing) As Boolean

            Dim fn As String = name.ToLower
            Dim fn2 As String

            For Each f In _Children
                fn2 = If(useParsingName, f.ParsingName.ToLower, f.DisplayName.ToLower)

                If fn2 = fn Then
                    obj = f
                    Return True
                End If
            Next

            Return False
        End Function

        Public Function Contains(item As ISimpleShellItem) As Boolean Implements ICollection(Of ISimpleShellItem).Contains
            Return _Children.Contains(item)
        End Function

        Public Sub CopyTo(array() As ISimpleShellItem, arrayIndex As Integer) Implements ICollection(Of ISimpleShellItem).CopyTo
            _Children.CopyTo(array, arrayIndex)
        End Sub

        Public Function GetEnumerator() As IEnumerator(Of ISimpleShellItem) Implements IEnumerable(Of ISimpleShellItem).GetEnumerator
            Return New DirEnumer(Me)
        End Function

        Friend Sub Add(item As ISimpleShellItem) Implements ICollection(Of ISimpleShellItem).Add
            _Children.Add(item)
        End Sub

        Friend Sub Clear() Implements ICollection(Of ISimpleShellItem).Clear
            _Children.Clear()
        End Sub
        Friend Function Remove(item As ISimpleShellItem) As Boolean Implements ICollection(Of ISimpleShellItem).Remove
            Return _Children.Remove(item)
        End Function
        Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Return New DirEnumer(Me)
        End Function


        Private Class DirEnumer
            Implements IEnumerator(Of ISimpleShellItem)

            Private _obj As DirectoryObject
            Private _pos As Integer = -1


            Public Sub New(obj As DirectoryObject)
                _obj = obj
            End Sub

            Public ReadOnly Property Current As ISimpleShellItem Implements IEnumerator(Of ISimpleShellItem).Current
                Get
                    Return _obj(_pos)
                End Get
            End Property

            Private ReadOnly Property IEnumerator_Current As Object Implements IEnumerator.Current
                Get
                    Return _obj(_pos)
                End Get
            End Property

            Public Function MoveNext() As Boolean Implements IEnumerator.MoveNext
                _pos += 1
                If (_pos >= _obj.Count) Then Return False
                Return True
            End Function

            Public Sub Reset() Implements IEnumerator.Reset
                _pos = -1
            End Sub
#End Region

#Region "IDisposable Support"
            Private disposedValue As Boolean ' To detect redundant calls

            ' This code added by Visual Basic to correctly implement the disposable pattern.
            Public Sub Dispose() Implements IDisposable.Dispose
                Dispose(True)
            End Sub

            ' IDisposable
            Protected Overridable Sub Dispose(disposing As Boolean)
                If Not disposedValue Then
                    If disposing Then
                        _obj = Nothing
                        _pos = -1
                    End If

                End If
                disposedValue = True
            End Sub
#End Region

        End Class


    End Class

End Namespace