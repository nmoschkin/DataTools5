Imports DataTools.Interop.Network
Imports DataTools.Interop
Imports DataTools.Memory
Imports System.ComponentModel
Imports System.Windows.Interop
Imports System.Collections.ObjectModel
Imports System.Windows
Imports System.Windows.Controls
Imports CoreCT.Text.ByteOrderMark

Public Class IPWindow

    Private _Adapters As AdaptersCollection

    'Public Shared Sub Main()

    '    System.Windows.Forms.Application.EnableVisualStyles()

    '    Dim window As New IPWindow()

    '    window.ShowDialog()
    'End Sub


    Public Property FSWindow As FSMonTestWindow
        Get
            Return GetValue(FSWindowProperty)
        End Get

        Set(value As FSMonTestWindow)
            SetValue(FSWindowProperty, value)
        End Set
    End Property

    Public Shared ReadOnly FSWindowProperty As DependencyProperty =
                           DependencyProperty.Register("FSWindow",
                           GetType(FSMonTestWindow), GetType(IPWindow),
                           New PropertyMetadata(Nothing))





    Public Property ProgramsWindow As Programs
        Get
            Return GetValue(ProgramsWindowProperty)
        End Get

        Set(value As Programs)
            SetValue(ProgramsWindowProperty, value)
        End Set
    End Property

    Public Shared ReadOnly ProgramsWindowProperty As DependencyProperty =
                           DependencyProperty.Register("ProgramsWindow",
                           GetType(Programs), GetType(IPWindow),
                           New PropertyMetadata(Nothing))

    Public Property HardwareWindow As Hardware
        Get
            Return GetValue(HardwareWindowProperty)
        End Get

        Set(value As Hardware)
            SetValue(HardwareWindowProperty, value)
        End Set
    End Property

    Public Shared ReadOnly HardwareWindowProperty As DependencyProperty =
                           DependencyProperty.Register("HardwareWindow",
                           GetType(Hardware), GetType(IPWindow),
                           New PropertyMetadata(Nothing))

    Public Property CodeEx As CodeExplorer
        Get
            Return GetValue(CodeExProperty)
        End Get

        Set(value As CodeExplorer)
            SetValue(CodeExProperty, value)
        End Set
    End Property

    Public Shared ReadOnly CodeExProperty As DependencyProperty =
                           DependencyProperty.Register("CodeEx",
                           GetType(CodeExplorer), GetType(IPWindow),
                           New PropertyMetadata(Nothing))


    Public Shared Function GetViewMenu(element As DependencyObject) As VirtualMenu
        If element Is Nothing Then
            Throw New ArgumentNullException("element")
        End If

        Return element.GetValue(ViewMenuProperty)
    End Function

    Public Shared Sub SetViewMenu(element As DependencyObject, value As VirtualMenu)
        If element Is Nothing Then
            Throw New ArgumentNullException("element")
        End If

        element.SetValue(ViewMenuProperty, value)
    End Sub

    Public Shared ReadOnly ViewMenuProperty As _
                           DependencyProperty = DependencyProperty.RegisterAttached("ViewMenu",
                           GetType(VirtualMenu), GetType(IPWindow),
                           New PropertyMetadata(Nothing))

    Public Property ViewMenu As VirtualMenu
        Get
            Return GetViewMenu(Me)
        End Get
        Set(value As VirtualMenu)
            SetViewMenu(Me, value)
        End Set
    End Property

    Public Sub New()

        'Dim mon As New DataTools.Interop.Display.Monitors

        'Dim icn = DataTools.Interop.Printers.PrinterObject.GetPrinterInfoObject("Brother MFC-7840W Printer")

        'Dim mm As DataTools.Memory.MemPtr = {"Hello", "Doctor", "Name"}
        'Dim p As String() = mm
        'mm.Free()

        InitializeComponent()
        WindowStartupLocation = WindowStartupLocation.CenterScreen
        Title = "DataTools Interop Library Test Project"

        Dim x = BOM.UTF16LE

        Dim txt = CPGlobal.SafeTextWrite("This is some text that I'm going to encode.  It's going to contain çalè", BOMTYPE.UTF8)


        System.IO.File.WriteAllBytes("BOM32.txt", txt)


        'Dim bl As New MemPtr, bl2 As New MemPtr

        'Dim len = 293397669

        'bl.Alloc(len)

        'Dim rnd = New Random(DateTime.Now.Ticks And &H7FFFFFFF)

        'For i = 0 To len - 1
        '    bl.ByteAt(i) = rnd.Next(0, 255)
        'Next


        'bl2.Alloc(len)

        'CoreCT.Memory.NativeLib.Native.MemCpy(bl.Handle, bl2.Handle, len)

        'Dim iss = True

        'For i = 0 To len - 1
        '    If (bl.ByteAt(i) <> bl2.ByteAt(i)) Then
        '        iss = False
        '        Exit For
        '    End If

        'Next


        'bl.Free()
        'bl2.Free()

        'rnd = Nothing


        'Dim bthRadio = EnumBluetoothRadios()

        'Dim bthDev = EnumBluetoothDevices()

        'BluetoothDeviceInfo.ShowBluetoothSettings()

        'Dim bl As SafePtr = System.Text.UTF8Encoding.UTF8.GetBytes("Hello World")

        'bl.ReAlloc(bl.Length + 2)


        'Dim s As String = bl.GrabUtf8String(0)

        'bl.Free()


        'Dim bl As Blob
        'Blob.TryParseObject("0.94494943511338", bl)


        'Dim fra As Decimal = 5 + 54 / 8
        'Dim fras As String = DataTools.ExtendedMath.PrintFraction(fra, , 40)

        'MsgBox(DataTools.ExtendedMath.PrintFraction(10.1375, , 100))
        'MsgBox("Print Fraction Demo #1: " & fras)


        'fra = 551 + 23 / 129
        'fras = DataTools.ExtendedMath.PrintFraction(fra, 14, 130)

        'MsgBox("Print Fraction Demo #2: " & fras)


    End Sub

    Private Sub AdapterList_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles AdapterList.SelectionChanged

        'Dim wr As NetworkAdapter = AdapterList.SelectedItem

        'If wr IsNot Nothing Then
        '    _props.SelectedObject = wr
        'End If

    End Sub

    Private Sub Quit_Click(sender As Object, e As RoutedEventArgs) Handles Quit.Click
        End
    End Sub

    Private Sub ShowPrg_Click(sender As Object, e As RoutedEventArgs) Handles ShowPrg.Click
        ProgramsWindow = New Programs
        ProgramsWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen
        ProgramsWindow.Show()
    End Sub

    Private Sub ShowHw_Click(sender As Object, e As RoutedEventArgs) Handles ShowHw.Click
        HardwareWindow = New Hardware
        HardwareWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen
        HardwareWindow.Show()
    End Sub


    Private Sub ShowFS_Click(sender As Object, e As RoutedEventArgs) Handles ShowFS.Click
        FSWindow = New FSMonTestWindow
        FSWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen
        FSWindow.Show()
    End Sub

    Private Sub IPWindow_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        If HardwareWindow IsNot Nothing Then HardwareWindow.Close()
        If ProgramsWindow IsNot Nothing Then ProgramsWindow.Close()
    End Sub


    Private Sub ShowHID_Click(sender As Object, e As RoutedEventArgs) Handles ShowHID.Click
        CodeEx = New CodeExplorer()
        CodeEx.WindowStartupLocation = WindowStartupLocation.CenterScreen
        CodeEx.Show()
    End Sub

    Private Sub IPWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded

        _Adapters = New AdaptersCollection
        AdapterList.ItemsSource = _Adapters.Collection

        ViewMenu = New VirtualMenu(Me, AdapterList)
        netMenu.ItemsSource = ViewMenu

    End Sub
End Class

Public Class VirtualMenu
    Inherits ObservableCollection(Of MenuItem)
    Implements INotifyPropertyChanged

    Private WithEvents _View As ListView
    Private _wih As WindowInteropHelper
    Private _swapCol As New ObservableCollection(Of MenuItem)

    Private _devMenu As MenuItem
    Private _netMenu As MenuItem
    Private _statusMenu As MenuItem

    Private Sub CheckDevice()

        _devMenu.IsEnabled = SelectedItem.CanShowDeviceDialog

        _netMenu.IsEnabled = SelectedItem.CanShowNetworkDialogs
        _statusMenu.IsEnabled = SelectedItem.CanShowNetworkDialogs

    End Sub

    Friend Sub New(window As Window, view As ListView)
        _View = view
        _wih = New WindowInteropHelper(window)

        _netMenu = New MenuItem With {.Header = "Show Network Properties Dialog"}
        _statusMenu = New MenuItem With {.Header = "Show Network Status Dialog"}
        _devMenu = New MenuItem With {.Header = "Show Device Properties Dialog"}

        AddHandler _netMenu.Click, AddressOf netConnProps
        AddHandler _statusMenu.Click, AddressOf netStatus
        AddHandler _devMenu.Click, AddressOf netDeviceProps

        Me.Add(_netMenu)
        Me.Add(_statusMenu)
        Me.Add(_devMenu)


        'Dim tKey As String = "::{A8A91A66-3A7D-4424-8D24-04E180695C7A}"
        'Dim sKey As String = "::{20D04FE0-3AEA-1069-A2D8-08002B30309D}\::{21EC2020-3AEA-1069-A2DD-08002B30309D}\::{A8A91A66-3A7D-4424-8D24-04E180695C7A}"
        'Dim mm As New DataTools.Memory.MemPtr
        'Dim ip As IntPtr

        'Dim lFile As New List(Of IShellItem)
        'Dim lName As New List(Of ShDisplayNames)

        'SHParseDisplayName(sKey, IntPtr.Zero, ip, 0, 0)

        'If ip <> 0 Then
        '    Dim fld As IShellFolder = Nothing

        '    SHBindToObject(Nothing, ip, 0, New Guid(ShellIIDGuid.IShellFolder), fld)
        '    Marshal.FreeCoTaskMem(ip)
        '    ip = 0

        '    Dim enumer As IEnumIDList = Nothing

        '    fld.EnumObjects(0, ShellFolderEnumerationOptions.Folders Or ShellFolderEnumerationOptions.IncludeHidden Or ShellFolderEnumerationOptions.NonFolders, enumer)

        '    If enumer IsNot Nothing Then
        '        Dim c As Integer = 0
        '        Dim iidlist As IntPtr

        '        Do
        '            iidlist = 0
        '            enumer.Next(1, iidlist, c)
        '            If iidlist <> 0 Then

        '                Dim pfile As IShellItem = Nothing
        '                SHCreateItemWithParent(0, fld, iidlist, New Guid(ShellIIDGuid.IShellItem), pfile)
        '                Marshal.FreeCoTaskMem(iidlist)

        '                If pfile IsNot Nothing Then
        '                    lFile.Add(pfile)
        '                    Dim sh As New ShDisplayNames

        '                    pfile.GetDisplayName(ShellItemDesignNameOptions.DesktopAbsoluteParsing, mm)
        '                    sh.AbsoluteParsing = mm
        '                    mm.CoTaskMemFree()

        '                    pfile.GetDisplayName(ShellItemDesignNameOptions.ParentRelativeParsing, mm)
        '                    sh.RelativeParsing = mm
        '                    mm.CoTaskMemFree()

        '                    pfile.GetDisplayName(ShellItemDesignNameOptions.Normal, mm)
        '                    sh.Normal = mm
        '                    mm.CoTaskMemFree()

        '                    pfile.GetDisplayName(ShellItemDesignNameOptions.ParentRelativeEditing, mm)
        '                    sh.Editing = mm
        '                    mm.CoTaskMemFree()

        '                    lName.Add(sh)

        '                End If

        '            End If
        '        Loop Until c = 0

        '        c = 0

        '    End If

        'End If

    End Sub

    Public Structure ShDisplayNames
        Public AbsoluteParsing As String
        Public Normal As String
        Public RelativeParsing As String
        Public Editing As String
        Public Overrides Function ToString() As String
            Return Normal
        End Function
    End Structure

    Public ReadOnly Property SelectedItem As NetworkAdapter
        Get
            Return CType(_View.SelectedItem, NetworkAdapter)
        End Get
    End Property

    Private Sub SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles _View.SelectionChanged
        CheckDevice()
    End Sub

    Private Sub netStatus(sender As Object, e As EventArgs)
        SelectedItem.ShowNetworkStatusDialog(_wih.Handle)
    End Sub

    Private Sub netConnProps(sender As Object, e As EventArgs)
        SelectedItem.ShowConnectionPropertiesDialog(_wih.Handle)
    End Sub

    Private Sub netDeviceProps(sender As Object, e As EventArgs)
        SelectedItem.DeviceInfo.ShowDevicePropertiesDialog(_wih.Handle)
    End Sub

End Class


Public Class A

    Public Property Prop1 As String
    Public Property Prop2 As String

    Public Shared Narrowing Operator CType(value As Z) As A
        Dim n As New A
        n.Prop1 = value.Prop1
        n.Prop2 = value.Prop2
        Return n
    End Operator

    Public Shared Widening Operator CType(value As A) As Z
        Dim n As New Z
        n.Prop1 = value.Prop1
        n.Prop2 = value.Prop2
        Return n
    End Operator

End Class

Public Class Z

    Public Property Prop1 As String
    Public Property Prop2 As String

    Public Property PropZ As String

End Class

Public Class B
    Inherits A

    Public Property Prop3 As String
    Public Property Prop4 As String

End Class



