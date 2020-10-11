'' ************************************************* ''
'' DataTools Visual Basic Utility Library - Interop
''
'' Module: System file association utility classes.
'' 
'' Copyright (C) 2011-2020 Nathan Moschkin
'' All Rights Reserved
''
'' Licensed Under the Microsoft Public License   
'' ************************************************* ''

Imports System.IO
Imports System.ComponentModel
Imports System.Drawing
Imports DataTools.Interop.Native
Imports Microsoft.Win32
Imports DataTools.Interop.Native.Menu
Imports CoreCT.Text

Namespace Desktop

#Region "System File Type Wrappers"

#Region "UIHandler"

    ''' <summary>
    ''' Represents a registered file-type handler program.
    ''' </summary>
    Public NotInheritable Class UIHandler
        Implements INotifyPropertyChanged, IDisposable

        Private _UIName As String
        Private _ExePath As String
        Private _Icon As Icon
        Private _Image As System.Windows.Media.Imaging.BitmapSource
        Private _Parent As AllSystemFileTypes

        Private _Handler As IAssocHandler
        Private _ExtList As New List(Of String)

        Private _Preferred As Boolean

        Private _AssocList As New System.Collections.ObjectModel.ObservableCollection(Of SystemFileType)

        ''' <summary>
        ''' Gets the list of supported extensions, separated by commas.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property ExtListString As String
            Get
                Dim sb As New System.Text.StringBuilder
                Dim cc As Integer = 0
                Dim x As Integer = 0
                For Each s In _ExtList
                    If x > 0 Then sb.Append(", ")
                    x += 1
                    If cc >= 80 Then
                        sb.Append(vbCrLf)
                        cc = 0
                    End If
                    cc += s.Length
                    sb.Append(s)
                Next
                Return sb.ToString
            End Get
        End Property

        ''' <summary>
        ''' Gets a value indicating that this is a recommended file handler.
        ''' </summary>
        ''' <returns></returns>
        Public Property Preferred As Boolean
            Get
                Return _Preferred
            End Get
            Friend Set(value As Boolean)
                _Preferred = value
            End Set
        End Property

        ''' <summary>
        ''' Returns the size of the program icon.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property IconSize As StandardIcons
            Get
                If _Parent Is Nothing Then Return StandardIcons.Icon48
                Return _Parent.IconSize
            End Get
        End Property

        Friend Sub New(handler As IAssocHandler, parent As AllSystemFileTypes)
            _Parent = parent
            Refresh(handler)
        End Sub

        ''' <summary>
        ''' Retrieves an array of support extensions.
        ''' </summary>
        ''' <returns></returns>
        Public Property ExtensionList As String()
            Get
                Return _ExtList.ToArray
            End Get
            Friend Set(value As String())
                _ExtList.Clear()
                _ExtList.AddRange(value)
            End Set
        End Property

        ''' <summary>
        ''' Rebuild the association halder list.
        ''' </summary>
        Friend Sub RebuildAssocList()
            _AssocList.Clear()
            _ExtList.Sort()

            For Each s In _ExtList
                For Each f In _Parent.FileTypes
                    If f.Extension = s Then
                        _AssocList.Add(f)
                        Exit For
                    End If
                Next
            Next
        End Sub

        ''' <summary>
        ''' Retrieves the list of associated file handlers.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Items As System.Collections.ObjectModel.ObservableCollection(Of SystemFileType)
            Get
                If _AssocList.Count = 0 Then RebuildAssocList()
                Return _AssocList
            End Get
        End Property


        ''' <summary>
        ''' Retrieves the list of associated file handlers.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property AssocList As System.Collections.ObjectModel.ObservableCollection(Of SystemFileType)
            Get
                If _AssocList.Count = 0 Then RebuildAssocList()
                Return _AssocList
            End Get
        End Property

        ''' <summary>
        ''' Clears the extension list.
        ''' </summary>
        Friend Sub ClearExtList()
            _ExtList.Clear()
        End Sub

        ''' <summary>
        ''' Add an extension.
        ''' </summary>
        ''' <param name="e"></param>
        Friend Sub AddExt(e As String)
            If _ExtList.Contains(e) = False Then _ExtList.Add(e)
        End Sub

        ''' <summary>
        ''' Refresh using the IAssocHandler
        ''' </summary>
        ''' <param name="handler"></param>
        Friend Sub Refresh(handler As IAssocHandler)
            Dim pth As String = Nothing,
            idx As Integer = 0

            Dim sz As UInteger = 0

            _Handler = handler
            Preferred = _Handler.IsRecommended = HResult.Ok

            handler.GetName(ExePath)
            If File.Exists(ExePath) = False Then Throw New SystemException("Program path not found")

            handler.GetUIName(UIName)
            handler.GetIconLocation(pth, idx)

            Icon = LoadLibraryIcon(pth, idx, IconSize)

            If Icon Is Nothing Then

                Dim iix = CInt(Shell_GetCachedImageIndex(pth, idx, 0))

                Select Case IconSize
                    Case StandardIcons.Icon256
                        Icon = GetFileIconFromIndex(iix, SystemIconSizes.Jumbo)

                    Case StandardIcons.Icon48
                        Icon = GetFileIconFromIndex(iix, SystemIconSizes.ExtraLarge)

                    Case StandardIcons.Icon32
                        Icon = GetFileIconFromIndex(iix, SystemIconSizes.Large)

                    Case Else
                        Icon = GetFileIconFromIndex(iix, SystemIconSizes.Small)

                End Select

            End If

        End Sub

        Friend Sub Refresh()
            Refresh(_Handler)
        End Sub

        ''' <summary>
        ''' The friendly name of the program.
        ''' </summary>
        ''' <returns></returns>
        Public Property UIName As String
            Get
                Return _UIName
            End Get
            Friend Set(value As String)
                _UIName = value
                OnPropertyChanged("UIName")
            End Set
        End Property

        ''' <summary>
        ''' The executable path of the program.
        ''' </summary>
        ''' <returns></returns>
        Public Property ExePath As String
            Get
                Return _ExePath
            End Get
            Friend Set(value As String)
                _ExePath = value
                OnPropertyChanged("ExePath")
            End Set
        End Property

        ''' <summary>
        ''' The icon for the executable handler.
        ''' </summary>
        ''' <returns></returns>
        Public Property Icon As System.Drawing.Icon
            Get
                Return _Icon
            End Get
            Friend Set(value As System.Drawing.Icon)
                _Icon = value
                Image = MakeWPFImage(_Icon)
                OnPropertyChanged("Icon")
            End Set
        End Property

        ''' <summary>
        ''' The WPF image for the executable handler.
        ''' </summary>
        ''' <returns></returns>
        Public Property Image As System.Windows.Media.Imaging.BitmapSource
            Get
                Return _Image
            End Get
            Friend Set(value As System.Windows.Media.Imaging.BitmapSource)
                _Image = value
                OnPropertyChanged("Image")
            End Set
        End Property

        Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    _Handler = Nothing
                End If

            End If
            Me.disposedValue = True
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

        Public Overrides Function ToString() As String
            Return UIName
        End Function


        Protected Overloads Sub OnPropertyChanged(e As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(e))
        End Sub

        Protected Overloads Sub OnPropertyChanged(e As PropertyChangedEventArgs)
            RaiseEvent PropertyChanged(Me, e)
        End Sub

    End Class

#End Region

    ''' <summary>
    ''' Class that describes information for an event fired by the <see cref="AllSystemFileTypes"/> class for a general enumeration of system file types.
    ''' </summary>
    Public Class FileTypeEnumEventArgs
        Inherits EventArgs

        Private _sft As SystemFileType
        Private _index As Integer
        Private _count As Integer

        ''' <summary>
        ''' The current index of the system file type that is being processed.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Index As Integer
            Get
                Return _index
            End Get
        End Property

        ''' <summary>
        ''' Total number of file types to process.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Count As Integer
            Get
                Return _count
            End Get
        End Property

        ''' <summary>
        ''' The current system file-type being processed.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Type As SystemFileType
            Get
                Return _sft
            End Get
        End Property

        ''' <summary>
        ''' Create a new event object consisting of these variables.
        ''' </summary>
        ''' <param name="sf">The <see cref="SystemFileType"/> object</param>
        ''' <param name="index">The current index</param>
        ''' <param name="count">The total number of file types</param>
        Friend Sub New(sf As SystemFileType, index As Integer, count As Integer)
            _sft = sf
            _index = index
            _count = count
        End Sub

    End Class

    ''' <summary>
    ''' Represents a file type.
    ''' </summary>
    Public NotInheritable Class SystemFileType
        Implements INotifyPropertyChanged, IDisposable

        Private _Col As New System.Collections.ObjectModel.ObservableCollection(Of UIHandler)
        Private _Ext As String
        Private _Desc As String
        Private _Parent As AllSystemFileTypes

        Private _DefaultIcon As Icon
        Private _DefaultImage As System.Windows.Media.Imaging.BitmapSource

        Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged

        ''' <summary>
        ''' Gets the size of the file icon.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property IconSize As StandardIcons
            Get
                Return _Parent.IconSize
            End Get
        End Property

        ''' <summary>
        ''' Returns the default icon.
        ''' </summary>
        ''' <returns></returns>
        Public Property DefaultIcon As Icon
            Get
                Return _DefaultIcon
            End Get
            Friend Set(value As Icon)
                _DefaultIcon = value
            End Set
        End Property

        ''' <summary>
        ''' Returns the default WPF image.
        ''' </summary>
        ''' <returns></returns>
        <Browsable(False)>
        Public Property DefaultImage As System.Windows.Media.Imaging.BitmapSource
            Get
                If _DefaultImage Is Nothing Then Return PreferredHandler.Image
                Return _DefaultImage
            End Get
            Friend Set(value As System.Windows.Media.Imaging.BitmapSource)
                _DefaultImage = value
            End Set
        End Property

        ''' <summary>
        ''' Returns the parent object.
        ''' </summary>
        ''' <returns></returns>
        Public Property Parent As AllSystemFileTypes
            Get
                Return _Parent
            End Get
            Friend Set(value As AllSystemFileTypes)
                _Parent = value
            End Set
        End Property

        ''' <summary>
        ''' Returns the description of the file extension.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Description As String
            Get
                Return _Desc
            End Get
        End Property

        ''' <summary>
        ''' Returns the first preferred UIHandler for this extension.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property PreferredHandler As UIHandler
            Get
                Dim h As UIHandler = Nothing
                For Each h In _Col
                    If h.Preferred Then Return h
                Next
                Return _Col(0)
            End Get
        End Property

        ''' <summary>
        ''' Gets a list of handlers for this extension.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Handlers As System.Collections.ObjectModel.ObservableCollection(Of UIHandler)
            Get
                Return _Col
            End Get
        End Property

        ''' <summary>
        ''' Gets a list of handlers for this extension.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Items As System.Collections.ObjectModel.ObservableCollection(Of UIHandler)
            Get
                Return _Col
            End Get
        End Property

        ''' <summary>
        ''' Gets the file extension.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Extension As String
            Get
                Return _Ext
            End Get
        End Property

        Friend Sub New(p As AllSystemFileTypes, ext As String)
            _Parent = p
            _Ext = ext
        End Sub

        ''' <summary>
        ''' Create a new <see cref="SystemFileType"/> object for the given extension.
        ''' </summary>
        ''' <param name="ext"></param>
        Public Sub New(ext As String)
            If (String.IsNullOrEmpty(ext)) Then Return

            If ext.Chars(0) <> "."c Then _Ext = "." & ext.ToLower Else _Ext = ext.ToLower
            OnPropertyChanged("Extension")
        End Sub

        ''' <summary>
        ''' Creates a new <see cref="SystemFileType"/> object from the given extension with the specified parameters.
        ''' </summary>
        ''' <param name="ext">The file extension.</param>
        ''' <param name="parent">The parent <see cref="AllSystemFileTypes"/> object.</param>
        ''' <param name="size">The default icon size.</param>
        ''' <returns></returns>
        Public Shared Function FromExtension(ext As String, Optional parent As AllSystemFileTypes = Nothing, Optional size As StandardIcons = StandardIcons.Icon48) As SystemFileType
            Dim c As New SystemFileType(ext)
            If parent IsNot Nothing Then c.Parent = parent

            Dim assoc() As IAssocHandler = EnumFileHandlers(ext)
            If assoc Is Nothing OrElse assoc.Count = 0 Then Return Nothing

            c.Populate(assoc, size)
            If c.Handlers.Count = 0 Then Return Nothing Else Return c
        End Function

        ''' <summary>
        ''' Creates a new <see cref="System.Windows.Media.Imaging.BitmapSource"/> object from the given extension with the specified parameters.
        ''' </summary>
        ''' <param name="ext">The file extension.</param>
        ''' <param name="size">The default icon size.</param>
        ''' <returns></returns>
        Public Shared Function ImageFromExtension(ext As String, Optional size As StandardIcons = StandardIcons.Icon48) As System.Windows.Media.Imaging.BitmapSource

            Dim sft = FromExtension(ext, , size)
            If sft Is Nothing Then Return Nothing
            Return sft.DefaultImage

        End Function

        ''' <summary>
        ''' Populate the information for this object.
        ''' </summary>
        ''' <param name="assoc">Optional array of previously-enumerated IAssocHandlers.</param>
        ''' <param name="size">The default icon size.</param>
        ''' <returns></returns>
        <CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification:="<Pending>")>
        Public Function Populate(Optional assoc() As IAssocHandler = Nothing, Optional size As StandardIcons = StandardIcons.Icon48) As Boolean
            If assoc Is Nothing Then assoc = EnumFileHandlers(_Ext)
            If assoc Is Nothing Then Return False

            _Col.Clear()
            Dim p As String = Nothing

            If _Parent Is Nothing Then
                For Each a In assoc
                    p = Nothing
                    a.GetName(p)
                    If File.Exists(p) = False Then Continue For
                    _Col.Add(New UIHandler(a, _Parent))
                Next
            Else
                For Each a In assoc
                    p = Nothing
                    a.GetName(p)
                    If File.Exists(p) = False Then Continue For
                    _Col.Add(_Parent.HandlerFromAssocHandler(a, _Ext))
                Next
            End If

            OnPropertyChanged("Handlers")

            Try
                Dim pk As RegistryKey = Registry.ClassesRoot.OpenSubKey(_Ext)
                Dim pk2 As RegistryKey

                If pk IsNot Nothing AndAlso CStr(pk.GetValue(Nothing)) IsNot Nothing Then
                    pk2 = Registry.ClassesRoot.OpenSubKey(CStr(pk.GetValue(Nothing)))
                    If pk2 IsNot Nothing Then
                        Dim d As String = CStr(pk2.GetValue(Nothing))

                        If String.Equals(d, _Desc) = False Then
                            _Desc = CStr(pk2.GetValue(Nothing))
                            OnPropertyChanged("Description")
                        End If

                        pk2.Close()

                        pk2 = Registry.ClassesRoot.OpenSubKey(CStr(pk.GetValue(Nothing)) & "\DefaultIcon")
                        pk.Close()

                        If pk2 IsNot Nothing Then

                            d = CStr(pk2.GetValue(Nothing))
                            pk2.Close()
                            If d IsNot Nothing Then

                                Dim i As Integer = d.LastIndexOf(",")
                                Dim c As Integer

                                If i = -1 Then
                                    c = 0
                                Else
                                    c = Integer.Parse(d.Substring(i + 1))
                                    d = d.Substring(0, i)
                                End If

                                _DefaultIcon = LoadLibraryIcon(d, c, size)
                                If _DefaultIcon IsNot Nothing Then
                                    _DefaultImage = MakeWPFImage(_DefaultIcon)

                                    OnPropertyChanged("DefaultImage")
                                    OnPropertyChanged("DefaultIcon")
                                End If

                            End If

                        End If

                    End If

                End If

                If _Desc Is Nothing OrElse _Desc = "" Then _Desc = _Ext & " file"

            Catch ex As Exception

            End Try

            Dim cn() As UIHandler = _Col.ToArray

            _Col.Clear()
            Array.Sort(cn, New UIHandlerComp)

            For Each cxn In cn
                _Col.Add(cxn)
            Next

            Return True
        End Function

        Public Overrides Function ToString() As String
            Return Me.Description
        End Function


#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    _Col = Nothing
                    _Ext = Nothing
                    If _DefaultIcon IsNot Nothing Then _DefaultIcon.Dispose()
                    _DefaultImage = Nothing
                End If
            End If
            Me.disposedValue = True
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region


        Protected Overloads Sub OnPropertyChanged(e As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(e))
        End Sub

        Protected Overloads Sub OnPropertyChanged(e As PropertyChangedEventArgs)
            RaiseEvent PropertyChanged(Me, e)
        End Sub

    End Class

#Region "Comparers"

    ''' <summary>
    ''' Compares type SystemFileType objects by extension
    ''' </summary>
    Public Class SysFileTypeComp
        Implements IComparer(Of SystemFileType)

        Public Function Compare(x As SystemFileType, y As SystemFileType) As Integer Implements IComparer(Of SystemFileType).Compare
            Return String.Compare(x.Extension, y.Extension)
        End Function
    End Class

    ''' <summary>
    ''' Compares two UIHandler objects by UIName
    ''' </summary>
    Public Class UIHandlerComp
        Implements IComparer(Of UIHandler)

        Public Function Compare(x As UIHandler, y As UIHandler) As Integer Implements IComparer(Of UIHandler).Compare
            Return String.Compare(x.UIName, y.UIName)
        End Function
    End Class

#End Region

    ''' <summary>
    ''' Reprents a list of all registered file types on the system, and their handlers.
    ''' </summary>
    Public NotInheritable Class AllSystemFileTypes
        Implements IDisposable, INotifyPropertyChanged

        Private _Col As New System.Collections.ObjectModel.ObservableCollection(Of SystemFileType)
        Private _UICol As New System.Collections.ObjectModel.ObservableCollection(Of UIHandler)

        Private _IconSize As StandardIcons = StandardIcons.Icon48

        Public Event Populating(sender As Object, e As FileTypeEnumEventArgs)

        ''' <summary>
        ''' Sets the uniform standard size for all icons and images in this object graph.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IconSize As StandardIcons
            Get
                Return _IconSize
            End Get
            Friend Set(value As StandardIcons)
                _IconSize = value
                OnPropertyChanged("IconSize")
            End Set
        End Property

        ''' <summary>
        ''' Retrieves the collection of SystemFileType objects.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property FileTypes As System.Collections.ObjectModel.ObservableCollection(Of SystemFileType)
            Get
                Return _Col
            End Get
        End Property

        ''' <summary>
        ''' Retrieves the collection of UIHandler objects.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property UICollection As System.Collections.ObjectModel.ObservableCollection(Of UIHandler)
            Get
                Return _UICol
            End Get
        End Property

        ''' <summary>
        ''' Retrieves a UIHandler object base on the IAssocHandler from a cache or creates and returns a new one if it does not already exist.
        ''' </summary>
        ''' <param name="assoc">The IAssocHandler from which to build the new object.</param>
        ''' <param name="ext">The Extension of the file type the IAssocHandler handles.</param>
        ''' <returns>A new UIHandler object</returns>
        ''' <remarks></remarks>
        Public Function HandlerFromAssocHandler(assoc As IAssocHandler, ext As String) As UIHandler
            Dim exepath As String = Nothing

            assoc.GetName(exepath)

            For Each u In _UICol
                If exepath = u.ExePath Then
                    u.AddExt(ext)
                    Return u
                End If
            Next

            HandlerFromAssocHandler = New UIHandler(assoc, Me)
            HandlerFromAssocHandler.AddExt(ext)

            _UICol.Add(HandlerFromAssocHandler)
        End Function

        ''' <summary>
        ''' Builds the system file type cache.
        ''' </summary>
        ''' <returns>The number of system file type entries enumerated.</returns>
        ''' <remarks></remarks>
        Public Function Populate(Optional fireEvent As Boolean = True) As Integer

            _Col.Clear()
            _UICol.Clear()

            Dim n As String() = Registry.ClassesRoot.GetSubKeyNames()
            Dim sf As SystemFileType
            Dim x As Integer = 0
            Dim y As Integer

            Dim sn2 As New List(Of String)

            For Each sn In n
                If sn.Substring(0, 1) = "." Then
                    sn2.Add(sn)
                End If
            Next

            y = sn2.Count

            For Each sn In sn2

                sf = SystemFileType.FromExtension(sn, Me)

                If sf IsNot Nothing Then
                    _Col.Add(sf)
                    x += 1

                    If fireEvent Then
                        '                        If fireEvent AndAlso x Mod 10 = 0 Then
                        RaiseEvent Populating(Me, New FileTypeEnumEventArgs(sf, x, y))
                        System.Windows.Forms.Application.DoEvents()

                    End If
                End If

            Next

            Dim c() As SystemFileType = _Col.ToArray
            Array.Sort(c, New SysFileTypeComp)

            _Col.Clear()

            For Each cu In c
                _Col.Add(cu)
            Next

            Dim d() As UIHandler = _UICol.ToArray

            Array.Sort(d, New UIHandlerComp)

            _UICol.Clear()

            For Each du In d
                _UICol.Add(du)
            Next

            OnPropertyChanged("UICollection")
            OnPropertyChanged("FileTypes")

            Return x
        End Function

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        Protected Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    _Col = Nothing
                    _UICol = Nothing
                End If
            End If
            Me.disposedValue = True
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

        Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged


        Protected Overloads Sub OnPropertyChanged(e As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(e))
        End Sub

        Protected Overloads Sub OnPropertyChanged(e As PropertyChangedEventArgs)
            RaiseEvent PropertyChanged(Me, e)
        End Sub

    End Class


    Public Module Assoc

        ''' <summary>
        ''' Populates the Open With menu and returns a <see cref="NativeMenu"/> object.
        ''' </summary>
        ''' <param name="fileName">The file name whose menu to retrieve.</param>
        ''' <param name="openWithCmd">The menu item id of the Open With menu item of the parent menu.</param>
        ''' <param name="hMenu">The handle of the parent menu.</param>
        ''' <returns></returns>
        Public Function GetOpenWithMenu(fileName As String, openWithCmd As IntPtr, hMenu As IntPtr) As NativeMenu
            ' Create a native context menu submenu populated with "open with" items
            Dim col As New MenuItemBagCollection

            Dim nm As New NativeMenu(hMenu)
            Dim ext As String = Path.GetExtension(fileName).ToLower
            Dim nmi As NativeMenuItem
            Dim assoc() As IAssocHandler = EnumFileHandlers(ext)

            nm.Items.Clear()

            If assoc Is Nothing Then
                nm.Destroy()
                Return Nothing
            End If

            For Each handler As IAssocHandler In assoc
                Dim icn As Icon,
                pth As String = Nothing,
                idx As Integer

                Dim uiname As String = Nothing,
                pathname As String = Nothing

                Dim sz As UInteger = 0

                handler.GetIconLocation(pth, idx)

                Dim iix = CInt(Shell_GetCachedImageIndex(pth, idx, 0))
                icn = GetFileIconFromIndex(iix, CType(SHIL_SMALL, SystemIconSizes))
                handler.GetName(pathname)
                If File.Exists(pathname) = False Then Continue For

                handler.GetUIName(uiname)

                If icn Is Nothing Then
                    nmi = nm.Items.Add(uiname)
                Else
                    nmi = nm.Items.Add(uiname, icn)
                End If
                col.Add(New MenuItemBag(nmi, handler))
            Next

            nm.Items.Add(Nothing)

            nmi = nm.Items.Add("&Choose default program...")
            nmi.Id = CInt(openWithCmd)
            nm.Bag = col

            Return nm
        End Function

    End Module
#End Region

End Namespace