Imports System
Imports System.IO
Imports DataTools.Interop.Native
Imports System.Collections.ObjectModel
Imports System.Collections
Imports CoreCT.Memory
Imports System.Runtime.InteropServices.ComTypes
Imports System.Windows.Media.Imaging
Imports System.ComponentModel
Imports System.Runtime.CompilerServices

Namespace Desktop

    ''' <summary>
    ''' Provides a file-system-locked object to represent a file.
    ''' </summary>
    Public Class FileObject
        Implements ISimpleShellItem

        Private _SysInterface As IShellItem

        Private _DisplayName As String
        Private _Filename As String

        Private _Icon As System.Drawing.Icon
        Private _IconImage As Windows.Media.Imaging.BitmapSource
        Private _IconSize As StandardIcons = StandardIcons.Icon48
        Private _IsSpecial As Boolean
        Private _Parent As ISimpleShellItem
        Private _Type As SystemFileType

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        Public Sub OnPropertyChanged(<CallerMemberName> Optional propertyName As String = Nothing)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
        End Sub

        ''' <summary>
        ''' Advanced initialization of FileObject.  Use this for items in special folders.
        ''' </summary>
        ''' <param name="parsingName">The shell parsing name of the file.</param>
        ''' <param name="isSpecial">Is the file known to be special?</param>
        ''' <param name="initialize">True to get file info and load icons.</param>
        ''' <param name="iconSize">Default icon size.  This can be changed with the <see cref="IconSize"/> property.</param>
        Public Sub New(parsingName As String, isSpecial As Boolean, initialize As Boolean, Optional iconSize As StandardIcons = StandardIcons.Icon48)
            _IsSpecial = isSpecial
            _Filename = parsingName

            Try
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
                        If (initialize) Then Refresh()
                        _SysInterface = shitem
                        Return
                    End If

                    res = SHCreateItemFromParsingName("shell:" + If(fp, parsingName), IntPtr.Zero, Guid.Parse(ShellIIDGuid.IShellItem), shitem)

                    If (res = HResult.Ok) Then
                        shitem.GetDisplayName(ShellItemDesignNameOptions.DesktopAbsoluteParsing, mm)

                        CanonicalName = CStr(mm)
                        If (Me.ParsingName Is Nothing) Then Me.ParsingName = CStr(mm)

                        _Filename = Me.ParsingName

                        mm.CoTaskMemFree()
                        shitem.GetDisplayName(ShellItemDesignNameOptions.Normal, mm)

                        DisplayName = CStr(mm)
                        mm.CoTaskMemFree()
                    End If

                    _SysInterface = shitem
                    shitem = Nothing

                    If (Not String.IsNullOrEmpty(DisplayName) AndAlso Not String.IsNullOrEmpty(Me.ParsingName)) Then
                        _IsSpecial = True
                        If (initialize) Then Refresh(_IconSize)

                        Return
                    End If

                End If


                If File.Exists(parsingName) = False Then
                    If (Not _IsSpecial) Then Throw New FileNotFoundException("File Not Found: " & parsingName)
                Else
                    If (initialize) Then Refresh(_IconSize)
                End If

            Catch ex As Exception

            End Try
        End Sub

        ''' <summary>
        ''' Create a new FileObject from the given filename. 
        ''' If the file does not exist, an exception will be thrown.
        ''' </summary>
        ''' <param name="filename"></param>
        Public Sub New(filename As String, Optional initialize As Boolean = True)
            Me.New(filename, False, initialize)
        End Sub

        ''' <summary>
        ''' Create a blank file object.  
        ''' </summary>
        Friend Sub New()

        End Sub

        ''' <summary>
        ''' Gets or sets the file attributes.
        ''' </summary>
        ''' <returns></returns>
        Public Property Attributes As IO.FileAttributes Implements ISimpleShellItem.Attributes
            Get
                Return GetFileAttributes(_Filename)
            End Get
            Set(value As IO.FileAttributes)
                SetFileAttributes(_Filename, value)
            End Set
        End Property

        ''' <summary>
        ''' Gets the canonical name of a special file
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property CanonicalName As String

        ''' <summary>
        ''' Get or set the creation time of the file.
        ''' </summary>
        ''' <returns></returns>
        Public Property CreationTime As Date Implements ISimpleShellItem.CreationTime
            Get
                Dim c As Date, a As Date, m As Date

                GetFileTime(_Filename, c, a, m)
                Return c
            End Get
            Set(value As Date)
                Dim c As Date, a As Date, m As Date

                GetFileTime(_Filename, c, a, m)
                SetFileTime(_Filename, value, a, m)
            End Set
        End Property

        ''' <summary>
        ''' Get the containing directory of the file.
        ''' </summary>
        ''' <returns></returns>
        Public Property Directory As String
            Get
                Return Path.GetDirectoryName(_Filename)
            End Get
            Set(value As String)
                If Not Move(value) Then
                    Throw New AccessViolationException("Unable to move file.")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets the display name of the file
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
        ''' Get the full path of the file.
        ''' </summary>
        ''' <returns></returns>
        Public Property Filename As String
            Get
                Return _Filename
            End Get
            Friend Set(value As String)

                If _Filename IsNot Nothing Then
                    If Not Utility.MoveFile(_Filename, value) Then
                        Throw New AccessViolationException("Unable to rename/move file.")
                    End If
                ElseIf Not File.Exists(value) Then
                    Throw New FileNotFoundException("File Not Found: " & Filename)
                End If

                _Filename = value
                Refresh()
            End Set
        End Property

        ''' <summary>
        ''' Returns the file type description
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property FileType As String
            Get
                If _Type Is Nothing Then Return "Unknown"
                Return _Type.Description
            End Get
        End Property

        ''' <summary>
        ''' Returns the file type icon
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property FileTypeIcon As System.Drawing.Icon
            Get
                If _Type Is Nothing Then Return Nothing
                Return _Type.DefaultIcon
            End Get
        End Property

        ''' <summary>
        ''' Returns the WPF-compatible file type icon image
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property FileTypeIconImage As Windows.Media.Imaging.BitmapSource
            Get
                If _Type Is Nothing Then Return Nothing
                Return _Type.DefaultImage
            End Get
        End Property

        ''' <summary>
        ''' Returns a Windows Forms compatible icon for the file
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Icon As System.Drawing.Icon
            Get
                If (_IsSpecial Or (_Parent IsNot Nothing AndAlso _Parent.IsSpecial)) Then
                    If (_Icon Is Nothing) Then
                        _Icon = GetFileIcon(ParsingName, StandardToSystem(_IconSize))
                    End If
                End If

                If _Icon IsNot Nothing Then Return _Icon Else Return FileTypeIcon
            End Get
        End Property

        ''' <summary>
        ''' Returns a WPF-compatible icon image for the file
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property IconImage As Windows.Media.Imaging.BitmapSource Implements ISimpleShellItem.Icon
            Get
                If (_IsSpecial Or (_Parent IsNot Nothing AndAlso _Parent.IsSpecial)) Then
                    If (_IconImage Is Nothing) Then
                        _IconImage = GetFileIconWPF(ParsingName, StandardToSystem(_IconSize))
                    End If
                End If

                If _IconImage IsNot Nothing Then Return _IconImage Else Return FileTypeIconImage
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets the default icon size for the file.
        ''' Individual files can override this setting, but they will be reset if the IconSize property of the parent directory is changed.
        ''' </summary>
        ''' <returns></returns>
        Public Property IconSize As StandardIcons Implements ISimpleShellItem.IconSize
            Get
                Return _IconSize
            End Get
            Set(value As StandardIcons)
                If _IconSize = value Then Return
                _IconSize = value
                Refresh(_IconSize)
            End Set
        End Property

        Public ReadOnly Property IsFolder As Boolean = False Implements ISimpleShellItem.IsFolder

        ''' <summary>
        ''' Returns whether or not this file is a special file / in a special folder
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property IsSpecial As Boolean Implements ISimpleShellItem.IsSpecial
            Get
                Return _IsSpecial
            End Get
        End Property

        ''' <summary>
        ''' Get or set the last access time of the file.
        ''' </summary>
        ''' <returns></returns>
        Public Property LastAccessTime As Date Implements ISimpleShellItem.LastAccessTime
            Get
                Dim c As Date, a As Date, m As Date

                GetFileTime(_Filename, c, a, m)
                Return a
            End Get
            Set(value As Date)
                Dim c As Date, a As Date, m As Date

                GetFileTime(_Filename, c, a, m)
                SetFileTime(_Filename, c, value, m)
            End Set
        End Property

        ''' <summary>
        ''' Get or set the last write time of the file.
        ''' </summary>
        ''' <returns></returns>
        Public Property LastWriteTime As Date Implements ISimpleShellItem.LastWriteTime
            Get
                Dim c As Date, a As Date, m As Date

                GetFileTime(_Filename, c, a, m)
                Return m
            End Get
            Set(value As Date)
                Dim c As Date, a As Date, m As Date

                GetFileTime(_Filename, c, a, m)
                SetFileTime(_Filename, c, a, value)
            End Set
        End Property

        ''' <summary>
        ''' Gets the name of the file.
        ''' </summary>
        ''' <returns></returns>
        Public Property Name As String
            Get
                Return Path.GetFileName(_Filename)
            End Get
            Friend Set(value As String)
                If Not Rename(value) Then
                    Throw New AccessViolationException("Unable to rename file.")
                End If
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
        ''' Gets the shell parsing name of a special file
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property ParsingName As String Implements ISimpleShellItem.ParsingName

        ''' <summary>
        ''' Get the size of the file, in bytes.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Size As Long Implements ISimpleShellItem.Size
            Get
                Return GetFileSize(_Filename)
            End Get
        End Property

        ''' <summary>
        ''' Return the file type object.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property TypeObject As SystemFileType
            Get
                Return _Type
            End Get
        End Property
        Private ReadOnly Property Children As ICollection(Of ISimpleShellItem) Implements ISimpleShellItem.Children
            Get
                Throw New NotImplementedException()
            End Get
        End Property
        Private ReadOnly Property Folders As ICollection(Of ISimpleShellItem) Implements ISimpleShellItem.Folders
            Get
                Throw New NotImplementedException()
            End Get
        End Property

        ''' <summary>
        ''' Attempt to move the file to a new directory.
        ''' </summary>
        ''' <param name="newDirectory">Destination directory.</param>
        ''' <returns>True if successful.</returns>
        Public Function Move(newDirectory As String) As Boolean

            If (_IsSpecial) Then Return False

            If newDirectory.Substring(newDirectory.Length - 1, 1) = "\" Then newDirectory = newDirectory.Substring(0, newDirectory.Length - 1)
            If Not IO.Directory.Exists(newDirectory) Then Return False

            Dim p As String = Path.GetFileName(_Filename)
            Dim f = newDirectory & "\" & p

            If Utility.MoveFile(_Filename, f) Then

                _Filename = f
                Refresh()

                Return True
            Else
                Return False
            End If

        End Function

        ''' <summary>
        ''' Refresh the state of the file object from the disk.
        ''' </summary>
        ''' <param name="iconSize">The size of the icon to fetch from the system.</param>
        ''' <returns></returns>
        Public Sub Refresh(Optional iconSize As StandardIcons? = Nothing) Implements ISimpleShellItem.Refresh

            If (iconSize Is Nothing) Then iconSize = _IconSize Else _IconSize = CType(iconSize, StandardIcons)
            If Not File.Exists(_Filename) Then Return

            _Type = SystemFileType.FromExtension(Path.GetExtension(_Filename),, _IconSize)

            If (_IconSize <> iconSize) Then _IconSize = _IconSize

            If (_IsSpecial Or (_Parent IsNot Nothing AndAlso _Parent.IsSpecial)) Then
                Dim st = StandardToSystem(_IconSize)

                _IconImage = GetFileIconWPF(ParsingName, st)
                _Icon = GetFileIcon(ParsingName, st)
            End If

            ' if we are no longer in the directory of the original parent, set to null
            If Not _IsSpecial AndAlso _Parent IsNot Nothing Then
                Dim v As DirectoryObject = CType(_Parent, DirectoryObject)

                If v.Directory.ToLower <> Me.Directory.ToLower Then
                    v.Remove(Me)
                    _Parent = Nothing
                End If
            End If


            OnPropertyChanged("Icon")
            OnPropertyChanged("IconImage")
            OnPropertyChanged("IconSize")
            OnPropertyChanged("ParsingName")
            OnPropertyChanged("DisplayName")
            OnPropertyChanged("Size")
            OnPropertyChanged("LastWriteTime")
            OnPropertyChanged("LastAccessTime")
            OnPropertyChanged("CreationTime")

        End Sub

        ''' <summary>
        ''' Attempt to rename the file.
        ''' </summary>
        ''' <param name="newName">The new name of the file.</param>
        ''' <returns>True if successful</returns>
        Public Function Rename(newName As String) As Boolean

            If (_IsSpecial) Then Return False

            Dim p As String = Path.GetDirectoryName(_Filename)
            Dim f As String = p & "\" & newName

            If Not Utility.MoveFile(_Filename, f) Then
                Return False
            End If

            _Filename = f
            Refresh()
            Return True

        End Function

        Public Overrides Function ToString() As String
            Return If(DisplayName, Filename)
        End Function

        Friend ReadOnly Property SysInterface As IShellItem
            Get
                Return _SysInterface
            End Get
        End Property

        Friend Shared Function StandardToSystem(stdIcon As StandardIcons) As SystemIconSizes
            Dim st As SystemIconSizes

            Select Case stdIcon

                Case StandardIcons.Icon16
                    st = SystemIconSizes.Small
                Case StandardIcons.Icon32
                    st = SystemIconSizes.Large
                Case StandardIcons.Icon48, StandardIcons.Icon64
                    st = SystemIconSizes.ExtraLarge
                Case Else
                    st = SystemIconSizes.Jumbo

            End Select

            Return st
        End Function
    End Class

End Namespace
