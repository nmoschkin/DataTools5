'' ************************************************* ''
'' DataTools Visual Basic Utility Library - Interop
''
'' Module: NativeMenu
''         Wrappers for the native Win32 API menu system
''
'' Started in 2000 on Windows 98/ME (and then later 2000).
''
'' Still kicking in 2014 on Windows 8.1!
'' A whole bunch of pInvoke/Const/Declare/Struct and associated utility functions that have been collected over the years.

'' Some enum documentation copied from the MSDN (and in some cases, updated).
'' 
'' Copyright (C) 2011-2020 Nathan Moschkin
'' All Rights Reserved
''
'' Licensed Under the Microsoft Public License   
'' ************************************************* ''




'' Some notes: The menu items are dynamic.  They are not statically maintained in any collection or structure.

'' When you fetch an item object from the virtual collection, that object is only alive in your program for as long as you reference it.
'' If the menu gets destroyed while you are still working with an item, it will fail.


Option Explicit On

Imports System.Runtime.InteropServices
Imports System.Drawing
Imports System.Threading

Imports CoreCT.Memory
Imports DataTools.Interop.Desktop
Imports System.Windows.Forms

Namespace Native.Menu

#Region "Native Menu Wrapper Classes"

    <Flags()>
    Public Enum MenuItemType
        [String] = &H0
        Bitmap = &H4
        OwnerDraw = &H100
        MenuBarBreak = &H20
        MenuBreak = &H40
        Separator = &H400
        RightJustify = &H4000
        RadioGroup = &H200
    End Enum

    Public Class MenuItemBag
        Public CmdId As IntPtr
        Public Data As Object
        Public Item As NativeMenuItem

        '' More room for stuff
        Public Misc As New KeyValuePair(Of String, Object)

        Public Sub New(cmd As IntPtr, data As Object)
            CmdId = cmd
            Me.Data = data
        End Sub

        Public Sub New(item As NativeMenuItem, data As Object)
            CmdId = CType(item.Id, IntPtr)
            Me.Item = item
            Me.Data = data
        End Sub
    End Class

    Public Class MenuItemBagCollection
        Implements ICollection(Of MenuItemBag)

        Private mList As New List(Of MenuItemBag)

        Public Sub New()

        End Sub

        Public Function FindBag(cmdId As IntPtr) As MenuItemBag
            For Each b In Me
                If b.CmdId = cmdId Then Return b
            Next

            Return Nothing
        End Function

        Default Public ReadOnly Property Item(index As Integer) As MenuItemBag
            Get
                If (mList Is Nothing) OrElse (mList.Count = 0) Then Return Nothing
                Return mList(index)
            End Get
        End Property

        Public Sub Add(item As MenuItemBag) Implements ICollection(Of MenuItemBag).Add
            mList.Add(item)
        End Sub

        Public Sub Clear() Implements ICollection(Of MenuItemBag).Clear
            mList = New List(Of MenuItemBag)
        End Sub

        Public Function Contains(item As MenuItemBag) As Boolean Implements ICollection(Of MenuItemBag).Contains
            If (mList Is Nothing) OrElse (mList.Count = 0) Then Return False
            Dim c As Integer = mList.Count

            For i = 0 To c
                If mList(i) Is item Then Return True
            Next
            Return False
        End Function

        Public Sub CopyTo(array() As MenuItemBag, arrayIndex As Integer) Implements ICollection(Of MenuItemBag).CopyTo
            If (mList Is Nothing) OrElse (mList.Count = 0) Then Return
            mList.CopyTo(array, arrayIndex)
        End Sub

        Public ReadOnly Property Count As Integer Implements ICollection(Of MenuItemBag).Count
            Get
                If (mList Is Nothing) OrElse (mList.Count = 0) Then Return 0
                Return mList.Count
            End Get
        End Property

        Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of MenuItemBag).IsReadOnly
            Get
                Return False
            End Get
        End Property

        Public Function Remove(item As MenuItemBag) As Boolean Implements ICollection(Of MenuItemBag).Remove
            If (mList Is Nothing) OrElse (mList.Count = 0) Then Return False
            Dim c As Integer = mList.Count

            For i = 0 To c
                If mList(i) Is item Then
                    mList.RemoveAt(i)
                    Return True
                End If

            Next
            Return False
        End Function

        Public Function GetEnumerator() As IEnumerator(Of MenuItemBag) Implements IEnumerable(Of MenuItemBag).GetEnumerator
            Return New MenuItemBagEnumerator(Me)
        End Function

        Public Function GetEnumerator1() As IEnumerator Implements IEnumerable.GetEnumerator
            Return New MenuItemBagEnumerator(Me)
        End Function
    End Class


    Public Class MenuItemBagEnumerator
        Implements IEnumerator(Of MenuItemBag)

        Dim subj As MenuItemBagCollection
        Dim pos As Integer = -1

        Friend Sub New(subject As MenuItemBagCollection)
            subj = subject
        End Sub

        Public ReadOnly Property Current As MenuItemBag Implements IEnumerator(Of MenuItemBag).Current
            Get
                Return subj(pos)
            End Get
        End Property

        Public ReadOnly Property Current1 As Object Implements IEnumerator.Current
            Get
                Return subj(pos)
            End Get
        End Property

        Public Function MoveNext() As Boolean Implements IEnumerator.MoveNext
            pos += 1
            Return (pos < subj.Count)
        End Function

        Public Sub Reset() Implements IEnumerator.Reset
            pos = -1
        End Sub

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then

                End If
                subj = Nothing
            End If
            Me.disposedValue = True
        End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

    End Class


    Public Class NativeMenuItemCollection
        Implements IEnumerable(Of NativeMenuItem)

        Private hMenu As IntPtr


        ''' <summary>
        ''' Gets the.DangerousGetHandle of the owner menu.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Handle As IntPtr
            Get
                Return hMenu
            End Get
        End Property

        Default Public ReadOnly Property Item(index As Integer) As NativeMenuItem
            Get
                If (index < 0) OrElse (index >= Count) Then
                    Throw New ArgumentOutOfRangeException
                End If

                Return New NativeMenuItem(hMenu, index)
            End Get
        End Property

        Public Function FindById(itemId As Integer) As NativeMenuItem
            Dim subm As NativeMenu,
            ii As NativeMenuItem

            For Each ii In Me
                If ii.Id = itemId Then
                    Return ii
                End If
                subm = ii.SubMenu
                If subm IsNot Nothing Then
                    ii = subm.Items.FindById(itemId)
                    If ii IsNot Nothing Then Return ii
                End If
            Next

            Return Nothing
        End Function

        Public Function OffsetById(itemId As Integer) As Integer
            Dim subm As NativeMenu,
            ii As NativeMenuItem
            Dim ui As Integer = 0

            For Each ii In Me
                If ii.Id = itemId Then
                    Return ui
                End If
                subm = ii.SubMenu
                If subm IsNot Nothing Then
                    ii = subm.Items.FindById(itemId)
                    If ii IsNot Nothing Then Return ui
                End If
                ui += 1
            Next

            Return -1
        End Function

        Public Function RemoveAt(index As Integer) As Boolean
            If (index < 0) OrElse (index >= Count) Then
                Throw New ArgumentOutOfRangeException
            End If

            Return (RemoveMenu(hMenu, index, MF_BYPOSITION) <> 0)
        End Function

        Public Function Remove(itemId As Integer) As Boolean
            Return (RemoveMenu(hMenu, itemId, MF_BYCOMMAND) <> 0)
        End Function

        Public ReadOnly Property Count As Integer
            Get
                Dim i As Integer = GetMenuItemCount(hMenu)
                If i <= 0 Then Return 0
                Return i
            End Get
        End Property

        Public Function Add(text As String, Optional data() As Byte = Nothing) As NativeMenuItem
            Dim mii As New MENUITEMINFO
            Return Insert(Count, text, True, IntPtr.Zero)
        End Function

        Public Function Add(text As String, bmp As Bitmap, Optional data() As Byte = Nothing) As NativeMenuItem
            Dim mii As New MENUITEMINFO
            Return Insert(Count, text, bmp, True)
        End Function

        Public Function Add(text As String, icon As Icon, Optional data() As Byte = Nothing) As NativeMenuItem
            Return Insert(Count, text, icon)
        End Function

        Public Function Insert(insertAfter As Integer, text As String, bmp As Bitmap, fbyPos As Boolean) As NativeMenuItem
            Return Insert(insertAfter, text, bmp, fbyPos, IntPtr.Zero)
        End Function

        Public Function Insert(insertAfter As Integer, text As String, bmp As Bitmap, fbyPos As Boolean, data As IntPtr) As NativeMenuItem
            Dim mii As New MENUITEMINFO
            Dim mm As MemPtr
            Dim nmi As NativeMenuItem = Nothing
            'If insertAfter = -1 Then insertAfter = 0

            mii.cbSize = Marshal.SizeOf(mii)

            ' if the text is nothing or '-' we'll assume they want it to be a separator
            If (text Is Nothing) OrElse (text = "-") Then
                mii.dwTypeData = IntPtr.Zero
                mii.fType = MFT_MENUBREAK
            Else
                mm = CType(text, MemPtr)
                mii.cch = text.Length

                mii.dwTypeData = mm.Handle
                mii.fType = MFT_STRING
            End If

            mii.fMask = MIIM_FTYPE Or MIIM_STRING Or MIIM_ID
            mii.wID = insertAfter + &H2000

            If bmp IsNot Nothing Then
                mii.hbmpItem = MakeDIBSection(bmp)
                mii.fMask += MIIM_BITMAP
            End If

            If (InsertMenuItem(hMenu, insertAfter, fbyPos, mii) <> 0) Then
                nmi = New NativeMenuItem(hMenu, insertAfter + &H2000, False)
                nmi.Data = data
            Else
                MsgBox(FormatLastError)
            End If
            mm.Free()
            Return nmi
        End Function

        Public Function Insert(insertAfter As Integer, text As String, fbyPos As Boolean, data As IntPtr) As NativeMenuItem
            Return Insert(insertAfter, text, CType(Nothing, Bitmap), fbyPos, data)
        End Function

        Public Function Insert(insertAfter As Integer, text As String, icon As Icon, Optional fbyPos As Boolean = True) As NativeMenuItem
            Return Insert(insertAfter, text, IconToTransparentBitmap(icon), fbyPos, IntPtr.Zero)
        End Function

        Public Function Clear() As Boolean
            Try
                Dim c As Integer = GetMenuItemCount(hMenu) - 1

                For i = c To 0 Step -1
                    DeleteMenu(hMenu, i, MF_BYPOSITION)
                Next
            Catch ex As Exception
                Return False
            End Try

            Return True
        End Function

        Public Function GetEnumerator() As IEnumerator(Of NativeMenuItem) Implements IEnumerable(Of NativeMenuItem).GetEnumerator
            Return New NativeMenuItemEnumerator(Me)
        End Function

        Public Function GetEnumerator1() As IEnumerator Implements IEnumerable.GetEnumerator
            Return New NativeMenuItemEnumerator(Me)
        End Function

        ' We don't want this collection to be created publicly
        Friend Sub New(hMenu As IntPtr)
            Me.hMenu = hMenu
        End Sub
    End Class

    Public Class NativeMenuItemEnumerator
        Implements IEnumerator(Of NativeMenuItem)

        Private pos As Integer = -1,
            subj As NativeMenuItemCollection

        Friend Sub New(subject As NativeMenuItemCollection)
            subj = subject
        End Sub

        Public ReadOnly Property Current As NativeMenuItem Implements IEnumerator(Of NativeMenuItem).Current
            Get
                Return subj(pos)
            End Get
        End Property

        Public ReadOnly Property Current1 As Object Implements IEnumerator.Current
            Get
                Return subj(pos)
            End Get
        End Property

        Public Function MoveNext() As Boolean Implements IEnumerator.MoveNext
            pos += 1
            If pos >= subj.Count Then Return False
            Return True
        End Function

        Public Sub Reset() Implements IEnumerator.Reset
            pos = -1
        End Sub

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    subj = Nothing
                End If
            End If
            Me.disposedValue = True
        End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub

#End Region

    End Class

    Public Class NativeMenuItem

        Private hMenu As IntPtr
        Private itemId As Integer

        Private mCol As NativeMenuItemCollection

        Friend Property Col As NativeMenuItemCollection
            Get
                Return mCol
            End Get
            Set(value As NativeMenuItemCollection)
                mCol = value
            End Set
        End Property

        Public ReadOnly Property Handle As IntPtr
            Get
                Return hMenu
            End Get
        End Property

        Public Property Id As Integer
            Get
                Return itemId
            End Get
            Set(value As Integer)
                Dim mii As New MENUITEMINFO
                mii.cbSize = Marshal.SizeOf(mii)

                mii.fMask = MIIM_ID
                mii.wID = value
                SetMenuItemInfo(hMenu, itemId, False, mii)
                itemId = value
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return Text
        End Function

        Public ReadOnly Property SubMenu As NativeMenu
            Get
                Dim mii As New MENUITEMINFO
                mii.cbSize = Marshal.SizeOf(mii)

                mii.fMask = MIIM_SUBMENU
                GetMenuItemInfo(hMenu, itemId, False, mii)
                If mii.hSubMenu = IntPtr.Zero Then Return Nothing

                Return New NativeMenu(mii.hSubMenu)
            End Get
        End Property

        Public Sub AttachSubMenu(hMenu As IntPtr)
            Dim mii As New MENUITEMINFO
            mii.cbSize = Marshal.SizeOf(mii)

            mii.fMask = MIIM_SUBMENU
            GetMenuItemInfo(hMenu, itemId, False, mii)

            If mii.hSubMenu <> IntPtr.Zero Then Return
            mii.hSubMenu = hMenu

            SetMenuItemInfo(hMenu, itemId, False, mii)
        End Sub

        Public Sub CreateSubMenu()
            Dim mii As New MENUITEMINFO
            mii.cbSize = Marshal.SizeOf(mii)

            mii.fMask = MIIM_SUBMENU
            GetMenuItemInfo(hMenu, itemId, False, mii)

            If mii.hSubMenu <> IntPtr.Zero Then Return
            mii.hSubMenu = CreatePopupMenu

            SetMenuItemInfo(hMenu, itemId, False, mii)
        End Sub

        Public Sub DestroySubMenu()
            Dim mii As New MENUITEMINFO
            mii.cbSize = Marshal.SizeOf(mii)

            mii.fMask = MIIM_SUBMENU
            GetMenuItemInfo(hMenu, itemId, False, mii)

            If mii.hSubMenu <> IntPtr.Zero Then
                DestroyMenu(mii.hSubMenu)
            End If

            DestroyMenu(mii.hSubMenu)
            mii.hSubMenu = IntPtr.Zero

            SetMenuItemInfo(hMenu, itemId, False, mii)
        End Sub

        Public Property [Default] As Boolean
            Get
                Dim mii As New MENUITEMINFO
                mii.cbSize = Marshal.SizeOf(mii)

                mii.fMask = MIIM_STATE
                GetMenuItemInfo(hMenu, itemId, False, mii)
                Return Not ((mii.fState And MFS_DEFAULT) = MFS_DEFAULT)
            End Get
            Set(value As Boolean)
                Dim mii As New MENUITEMINFO
                mii.cbSize = Marshal.SizeOf(mii)

                mii.fMask = MIIM_STATE
                GetMenuItemInfo(hMenu, itemId, False, mii)

                If (value = True) Then
                    mii.fState = mii.fState And Not CInt(MFS_DEFAULT)
                Else
                    mii.fState = mii.fState Or CInt(MFS_DEFAULT)
                End If

                SetMenuItemInfo(hMenu, itemId, False, mii)
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the item data, in bytes.
        ''' It is assumed that the data in question will have a size descriptor preamble in memory of type Integer (32 bit signed ordinal).
        ''' The preamble is not returned, and a size-containing preamble should not be set when the value is set to a byte array.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Data As IntPtr
            Get
                Dim mii As New MENUITEMINFO
                mii.cbSize = Marshal.SizeOf(mii)

                mii.fMask = MIIM_DATA
                GetMenuItemInfo(hMenu, itemId, False, mii)

                Return mii.dwItemData
            End Get
            Set(value As IntPtr)
                Dim mii As New MENUITEMINFO

                mii.cbSize = Marshal.SizeOf(mii)
                mii.fMask = MIIM_DATA

                mii.dwItemData = value
                SetMenuItemInfo(hMenu, itemId, False, mii)
            End Set
        End Property

        Public Property Text As String
            Get
                Dim mii As New MENUITEMINFO
                mii.cbSize = Marshal.SizeOf(mii)
                Dim s As String

                mii.fMask = MIIM_FTYPE
                If GetMenuItemInfo(hMenu, itemId, False, mii) = 0 Then Return "-"

                If (mii.fType And MFT_SEPARATOR) = MFT_SEPARATOR Then
                    Return "-"
                End If

                Dim mm As New SafePtr

                mii.fMask = MIIM_STRING
                mii.cch = 0
                GetMenuItemInfo(hMenu, itemId, False, mii)

                mm.Length = (mii.cch + 1) * Len("A"c)
                mii.cch += 1

                mii.dwTypeData = mm.DangerousGetHandle
                mii.fMask = MIIM_STRING

                If GetMenuItemInfo(hMenu, itemId, False, mii) = 0 Then
                    Dim err As Integer = CInt(GetLastError),
                    serr As String = Nothing

                    mm.Length = 1026
                    mm.ZeroMemory()

                    FormatMessage(&H1000, IntPtr.Zero, CUInt(err), 0, mm.DangerousGetHandle, 512, IntPtr.Zero)

                    '                MsgBox("Error 0x" & err.ToString("X8") & ": " & mm.ToString)
                    s = mm.ToString
                    mm.Dispose()

                    Return s

                End If

                s = mm.ToString
                mm.Dispose()
                Return s

            End Get
            Set(value As String)
                Dim mii As New MENUITEMINFO
                mii.cbSize = Marshal.SizeOf(mii)
                mii.fMask = MIIM_STRING
                Dim mm As New SafePtr
                mm = CType(value, SafePtr)
                mm.Length += Len("A"c)

                mii.cch = CInt(mm.Length)
                mii.dwTypeData = mm.DangerousGetHandle

                SetMenuItemInfo(hMenu, itemId, False, mii)
            End Set
        End Property

        ''' <summary>
        ''' Set the.DangerousGetHandle to the item bitmap, directly, without a GDI+ Bitmap object.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property hBitmap As IntPtr
            Get
                Dim mii As New MENUITEMINFO
                mii.cbSize = Marshal.SizeOf(mii)

                mii.fMask = MIIM_BITMAP
                GetMenuItemInfo(hMenu, itemId, False, mii)
                If mii.hbmpItem = IntPtr.Zero Then Return Nothing
                Return mii.hbmpItem
            End Get
            Set(value As IntPtr)
                Dim mii As New MENUITEMINFO
                mii.cbSize = Marshal.SizeOf(mii)

                mii.fMask = MIIM_BITMAP
                GetMenuItemInfo(hMenu, itemId, False, mii)

                If mii.hbmpItem <> IntPtr.Zero Then
                    DeleteObject(mii.hbmpItem)
                    mii.hbmpItem = IntPtr.Zero
                End If

                mii.hbmpItem = value
                SetMenuItemInfo(hMenu, itemId, False, mii)
            End Set
        End Property

        ''' <summary>
        ''' Convert an icon into a bitmap and set it into the menu.  
        ''' </summary>
        ''' <value></value>
        ''' <remarks></remarks>
        Public WriteOnly Property Icon() As System.Drawing.Icon
            Set(value As System.Drawing.Icon)
                Bitmap = IconToTransparentBitmap(value)
            End Set
        End Property

        ''' <summary>
        ''' Dynamically get or set the bitmap for the item.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Bitmap As System.Drawing.Bitmap
            Get
                Dim mii As New MENUITEMINFO
                mii.cbSize = Marshal.SizeOf(mii)

                mii.fMask = MIIM_BITMAP
                GetMenuItemInfo(hMenu, itemId, False, mii)

                If mii.hbmpItem = IntPtr.Zero Then Return Nothing
                Return Bitmap.FromHbitmap(mii.hbmpItem)
            End Get
            Set(value As System.Drawing.Bitmap)

                If value Is Nothing Then
                    hBitmap = IntPtr.Zero
                    Return
                End If

                Dim mii As New MENUITEMINFO
                mii.cbSize = Marshal.SizeOf(mii)

                mii.hbmpItem = MakeDIBSection(value)
                mii.fMask = MIIM_BITMAP

                SetMenuItemInfo(hMenu, itemId, False, mii)
            End Set
        End Property

        Public Property CheckState As CheckState
            Get
                Dim mii As New MENUITEMINFO
                mii.cbSize = Marshal.SizeOf(mii)

                mii.fMask = MIIM_STATE
                GetMenuItemInfo(hMenu, itemId, False, mii)

                If (mii.fState And MFS_CHECKED) = MFS_CHECKED Then
                    Return CheckState.Checked
                End If

                Return System.Windows.Forms.CheckState.Unchecked
            End Get
            Set(value As CheckState)
                Dim mii As New MENUITEMINFO
                mii.cbSize = Marshal.SizeOf(mii)

                mii.fMask = MIIM_STATE
                GetMenuItemInfo(hMenu, itemId, False, mii)

                mii.fState = mii.fState And CInt(Not MFS_CHECKED)

                Select Case value
                    Case System.Windows.Forms.CheckState.Unchecked
                        mii.fState = CInt(mii.fState Or MFS_CHECKED)

                    Case System.Windows.Forms.CheckState.Checked, System.Windows.Forms.CheckState.Indeterminate
                        mii.fState = CInt(mii.fState Or MFS_UNCHECKED)

                End Select

                SetMenuItemInfo(hMenu, itemId, False, mii)
            End Set
        End Property

        Public Property Checked As Boolean
            Get
                Dim mii As New MENUITEMINFO
                mii.cbSize = Marshal.SizeOf(mii)
                Return (CheckState = System.Windows.Forms.CheckState.Checked)
            End Get
            Set(value As Boolean)
                If value = True Then
                    CheckState = System.Windows.Forms.CheckState.Checked
                Else
                    CheckState = System.Windows.Forms.CheckState.Unchecked
                End If
            End Set
        End Property

        Public Property OwnerDrawn As Boolean
            Get
                Dim mii As New MENUITEMINFO
                mii.cbSize = Marshal.SizeOf(mii)
                mii.fMask = MIIM_FTYPE

                GetMenuItemInfo(hMenu, itemId, False, mii)
                If (mii.fType And MFT_OWNERDRAW) = MFT_OWNERDRAW Then
                    Return True
                Else
                    Return False
                End If
            End Get
            Set(value As Boolean)
                Dim mii As New MENUITEMINFO
                mii.cbSize = Marshal.SizeOf(mii)
                mii.fMask = MIIM_FTYPE

                GetMenuItemInfo(hMenu, itemId, False, mii)

                If value Then
                    mii.fType = CInt(mii.fType Or MFT_OWNERDRAW)
                Else
                    mii.fType = CInt(mii.fType And (Not MFT_OWNERDRAW))
                End If

                SetMenuItemInfo(hMenu, itemId, False, mii)
            End Set
        End Property

        Public Property ItemType As MenuItemType
            Get
                Dim mii As New MENUITEMINFO
                mii.cbSize = Marshal.SizeOf(mii)

                mii.cbSize = Marshal.SizeOf(mii)
                mii.fMask = MIIM_FTYPE

                GetMenuItemInfo(hMenu, itemId, False, mii)

                Return CType((mii.fType And MenuTypeMask), MenuItemType)

            End Get
            Set(value As MenuItemType)
                Dim mii As New MENUITEMINFO
                mii.cbSize = Marshal.SizeOf(mii)
                mii.fMask = MIIM_FTYPE

                GetMenuItemInfo(hMenu, itemId, False, mii)

                mii.fType = mii.fType And (Not MenuTypeMask)
                mii.fType = mii.fType Or value

                SetMenuItemInfo(hMenu, itemId, False, mii)
            End Set
        End Property

        Public Property Enabled As Boolean
            Get
                Dim mii As New MENUITEMINFO
                mii.cbSize = Marshal.SizeOf(mii)

                mii.fMask = MIIM_STATE
                GetMenuItemInfo(hMenu, itemId, False, mii)
                Return Not ((mii.fState And MFS_DISABLED) = MFS_DISABLED)
            End Get
            Set(value As Boolean)
                Dim mii As New MENUITEMINFO
                mii.cbSize = Marshal.SizeOf(mii)

                mii.fMask = MIIM_STATE
                GetMenuItemInfo(hMenu, itemId, False, mii)

                mii.fState = CInt(mii.fState And (Not (MFS_DISABLED)))

                If value Then
                    mii.fState = CInt(mii.fState Or MFS_DISABLED)
                End If
                SetMenuItemInfo(hMenu, itemId, False, mii)
            End Set
        End Property

        ''' <summary>
        ''' Initialize the item to a pre-existing native menu item.  
        ''' Use NativeMenuItemCollection.Add to create a new item.
        ''' </summary>
        ''' <param name="hMenu"></param>
        ''' <param name="itemId"></param>
        ''' <param name="byPos"></param>
        ''' <remarks></remarks>
        Public Sub New(hMenu As IntPtr, itemId As Integer, Optional byPos As Boolean = True)

            Me.hMenu = hMenu

            If byPos = True Then
                Dim mii As New MENUITEMINFO
                mii.cbSize = Marshal.SizeOf(mii)

                mii.fMask = MIIM_ID
                GetMenuItemInfo(hMenu, itemId, True, mii)
                Me.itemId = mii.wID
            Else
                Me.itemId = itemId
            End If

        End Sub

    End Class

    Public Class NativeMenu
        Implements IDisposable

        Private hMenu As IntPtr = IntPtr.Zero
        Private mCol As NativeMenuItemCollection
        Private mBag As MenuItemBagCollection

        Public Property Bag As MenuItemBagCollection
            Get
                Return mBag
            End Get
            Set(value As MenuItemBagCollection)
                mBag = value
            End Set
        End Property

        Public ReadOnly Property Handle As IntPtr
            Get
                Return hMenu
            End Get
        End Property

        Public ReadOnly Property Items As NativeMenuItemCollection
            Get
                Return mCol
            End Get
        End Property

        Public Sub CreateHandle()
            If hMenu <> IntPtr.Zero Then
                DestroyMenu(hMenu)
            End If

            hMenu = CreateMenu
        End Sub

        Public Sub Destroy()
            Dim nmi As NativeMenuItem

            For Each nmi In Items
                Dim sb As NativeMenu = nmi.SubMenu
                nmi.Bitmap = Nothing
                nmi.Data = Nothing
                If sb IsNot Nothing Then
                    sb.Destroy()
                End If
            Next

            DestroyMenu(hMenu)
        End Sub

        Public Sub New(Optional createHandle As Boolean = True, Optional isPopup As Boolean = True)

            If createHandle Then
                If isPopup Then
                    hMenu = CreatePopupMenu
                Else
                    hMenu = CreateMenu()
                End If
            End If
            mCol = New NativeMenuItemCollection(hMenu)
        End Sub

        Public Sub New(hMenu As IntPtr)
            Me.hMenu = hMenu
            mCol = New NativeMenuItemCollection(hMenu)
        End Sub

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                'If disposing Then
                'End If

                If hMenu <> IntPtr.Zero Then
                    DestroyMenu(hMenu)
                    hMenu = IntPtr.Zero
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

    End Class

#End Region

#Region "Native -> Managed Menu Conversion (ToolStripMenuItem)"

    Public Module NativeMenuConversion

        Public Const MenuTypeMask = &H4764

        Public Function GetDefaultItem(hMenu As IntPtr, Optional retPos As Boolean = False) As IntPtr

            Dim i As Integer,
            c As Integer = GetMenuItemCount(hMenu)

            Dim mi As MENUITEMINFO

            mi.cbSize = Marshal.SizeOf(mi)
            mi.fMask = MIIM_STATE + MIIM_ID

            For i = 0 To c - 1

                GetMenuItemInfo(hMenu, i, True, mi)

                If (mi.fState And MFS_DEFAULT) = MFS_DEFAULT Then
                    If retPos Then Return CType(i, IntPtr) Else Return CType(mi.wID, IntPtr)
                End If

            Next

            Return IntPtr.Zero
        End Function

        ''' <summary>
        ''' Copy a native hMenu and all its contents into a managed Menu object
        ''' </summary>
        ''' <param name="hMenu"></param>
        ''' <param name="destMenu"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function MenuBarCopyToManaged(hMenu As IntPtr, ByRef destMenu As MenuStrip, Optional destroyOrig As Boolean = True) As Boolean
            Dim mi As MENUINFO,
            c As Integer,
            i As Integer

            Dim min As ToolStripMenuItem = Nothing

            Thread.Sleep(100)

            If destMenu Is Nothing Then
                destMenu = New MenuStrip
            End If

            mi.cbSize = Marshal.SizeOf(mi)
            mi.fMask = MIM_MAXHEIGHT + MIM_STYLE

            GetMenuInfo(hMenu, mi)

            SetMenuInfo(destMenu.Handle, mi)

            c = GetMenuItemCount(hMenu)

            For i = 0 To c - 1
                If MenuItemCopyToManaged(hMenu, i, min) = False Then Return False
                min.Height = mi.cyMax
                destMenu.Items.Add(min)

                min = Nothing
                Thread.Sleep(0)
            Next

            If destroyOrig Then
                DestroyMenu(hMenu)
            End If

            Return True
        End Function


        ''' <summary>
        ''' Copy a native hMenu and all its contents into a managed Menu object
        ''' </summary>
        ''' <param name="hMenu"></param>
        ''' <param name="destMenu"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ContextMenuCopyToManaged(hMenu As IntPtr, ByRef destMenu As ContextMenuStrip, Optional destroyOrig As Boolean = True) As Boolean

            Dim mi As MENUINFO,
            c As Integer,
            i As Integer

            Dim min As ToolStripMenuItem = Nothing

            Thread.Sleep(100)
            If destMenu Is Nothing Then
                destMenu = New ContextMenuStrip
            End If

            mi.cbSize = Marshal.SizeOf(mi)
            mi.fMask = MIM_MAXHEIGHT + MIM_STYLE

            GetMenuInfo(hMenu, mi)

            'SetMenuInfo(destMenu.Handle, mi)

            c = GetMenuItemCount(hMenu)

            For i = 0 To c - 1
                If MenuItemCopyToManaged(hMenu, i, min) = False Then Return False
                min.Height = mi.cyMax
                destMenu.Items.Add(min)

                min = Nothing
                Thread.Sleep(0)
            Next

            If destroyOrig Then
                DestroyMenu(hMenu)
            End If

            Return True

        End Function


        ''' <summary>
        ''' Copy a native hMenu and all its contents into a managed DropDownItemsCollection object
        ''' </summary>
        ''' <param name="hMenu"></param>
        ''' <param name="items"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function MenuItemsToManaged(hMenu As IntPtr, ByRef items As ToolStripItemCollection) As Boolean

            Dim c As Integer,
            i As Integer


            Dim min As ToolStripMenuItem = Nothing
            c = GetMenuItemCount(hMenu)

            For i = 0 To c - 1
                If MenuItemCopyToManaged(hMenu, i, min) = False Then Return False
                items.Add(min)
                min = Nothing
                Thread.Sleep(0)
            Next

            Return True

        End Function

        Public Function GetMenuItemText(hMenu As IntPtr, itemId As Integer, Optional byPos As Boolean = True) As String
            Dim mii As New MENUITEMINFO
            Dim mm As New SafePtr

            mii.cbSize = Marshal.SizeOf(mii)
            mii.cch = 0
            mii.fMask = MIIM_TYPE
            GetMenuItemInfo(hMenu, itemId, byPos, mii)

            mm.Length = (mii.cch + 1) * Len("A"c)
            mii.cch += 1

            mii.dwTypeData = mm.handle

            If GetMenuItemInfo(hMenu, itemId, byPos, mii) = 0 Then
                Dim err As Integer = CInt(GetLastError),
                serr As String = Nothing

                mm.Length = 1026
                mm.ZeroMemory()

                FormatMessage(&H1000, IntPtr.Zero, CUInt(err), 0, mm.handle, 512, IntPtr.Zero)

                MsgBox("Error " & err.ToString("X8") & " " & mm.ToString)
                mm.Dispose()

                Return Nothing

            End If

            If (mii.fType And MFT_SEPARATOR) = MFT_SEPARATOR Then
                Return "-"
            Else
                Dim s As String
                s = mm.ToString
                mm.Dispose()
                Return s
            End If

        End Function

        ''' <summary>
        ''' Copy a native hMenu and all its contents into a managed Menu object
        ''' </summary>
        ''' <param name="hMenu"></param>
        ''' <param name="destItem"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function MenuItemCopyToManaged(hMenu As IntPtr, itemId As Integer, ByRef destItem As ToolStripMenuItem, Optional byPos As Boolean = True) As Boolean

            Dim mii As New MENUITEMINFO
            Dim bmp As Bitmap
            Dim mm As New SafePtr

            If destItem Is Nothing Then
                destItem = New ToolStripMenuItem
            End If

            mii.cbSize = Marshal.SizeOf(mii)
            mii.cch = 0
            mii.fMask = MIIM_TYPE
            GetMenuItemInfo(hMenu, itemId, byPos, mii)

            mm.Length = (mii.cch + 1) * Len("A"c)
            mii.cch += 1

            mii.dwTypeData = mm.handle

            If GetMenuItemInfo(hMenu, itemId, byPos, mii) = 0 Then
                Dim err As Integer = CInt(GetLastError),
                serr As String = Nothing

                mm.Length = 1026
                mm.ZeroMemory()

                FormatMessage(&H1000, IntPtr.Zero, CUInt(err), 0, mm.handle, 512, IntPtr.Zero)

                MsgBox("Error " & err.ToString("X8") & " " & mm.ToString)
                mm.Dispose()

                Return False

            End If

            If (mii.fType And MFT_SEPARATOR) = MFT_SEPARATOR Then
                destItem.Text = "-"
            Else
                destItem.Text = mm.ToString
            End If

            mm.Dispose()

            mii.fMask = MIIM_BITMAP
            GetMenuItemInfo(hMenu, itemId, byPos, mii)

            If mii.hbmpItem <> IntPtr.Zero Then
                bmp = Bitmap.FromHbitmap(mii.hbmpItem)
                destItem.Image = bmp
            End If

            mii.fMask = MIIM_STATE
            GetMenuItemInfo(hMenu, itemId, byPos, mii)

            If (mii.fState And MFS_CHECKED) = MFS_CHECKED Then
                destItem.CheckState = CheckState.Checked
            End If

            If (mii.fState And MFS_DISABLED) = MFS_DISABLED Then
                destItem.Enabled = False
            End If

            If (mii.fState And MFS_DEFAULT) = MFS_DEFAULT Then
                destItem.Font = New Font(destItem.Font.Name, destItem.Font.Size, FontStyle.Bold)
            End If

            mii.fMask = MIIM_SUBMENU
            GetMenuItemInfo(hMenu, itemId, byPos, mii)

            If mii.hSubMenu <> IntPtr.Zero Then
                Return MenuItemsToManaged(mii.hSubMenu, destItem.DropDownItems)
            End If

            Return True

        End Function


    End Module

#End Region

End Namespace