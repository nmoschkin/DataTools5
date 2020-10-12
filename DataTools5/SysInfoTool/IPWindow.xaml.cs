using System;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Interop;
using DataTools.Hardware.Network;

namespace SysInfoTool
{
    public partial class IPWindow : Window
    {
        private AdaptersCollection _Adapters;

        // Public Shared Sub Main()

        // System.Windows.Forms.Application.EnableVisualStyles()

        // Dim window As New IPWindow()

        // window.ShowDialog()
        // End Sub


        public FSMonTestWindow FSWindow
        {
            get
            {
                return (FSMonTestWindow)this.GetValue(FSWindowProperty);
            }

            set
            {
                this.SetValue(FSWindowProperty, value);
            }
        }

        public static readonly DependencyProperty FSWindowProperty = DependencyProperty.Register("FSWindow", typeof(FSMonTestWindow), typeof(IPWindow), new PropertyMetadata(null));

        public Programs ProgramsWindow
        {
            get
            {
                return (Programs)this.GetValue(ProgramsWindowProperty);
            }

            set
            {
                this.SetValue(ProgramsWindowProperty, value);
            }
        }

        public static readonly DependencyProperty ProgramsWindowProperty = DependencyProperty.Register("ProgramsWindow", typeof(Programs), typeof(IPWindow), new PropertyMetadata(null));

        public Hardware HardwareWindow
        {
            get
            {
                return (Hardware)this.GetValue(HardwareWindowProperty);
            }

            set
            {
                this.SetValue(HardwareWindowProperty, value);
            }
        }

        public static readonly DependencyProperty HardwareWindowProperty = DependencyProperty.Register("HardwareWindow", typeof(Hardware), typeof(IPWindow), new PropertyMetadata(null));

        public SysInfoWindow SysInfoWindow
        {
            get
            {
                return (SysInfoWindow)this.GetValue(SysInfoWindowProperty);
            }

            set
            {
                this.SetValue(SysInfoWindowProperty, value);
            }
        }

        public static readonly DependencyProperty SysInfoWindowProperty = DependencyProperty.Register("SysInfoWindow", typeof(SysInfoWindow), typeof(IPWindow), new PropertyMetadata(null));

        public CodeExplorer CodeEx
        {
            get
            {
                return (CodeExplorer)this.GetValue(CodeExProperty);
            }

            set
            {
                this.SetValue(CodeExProperty, value);
            }
        }

        public static readonly DependencyProperty CodeExProperty = DependencyProperty.Register("CodeEx", typeof(CodeExplorer), typeof(IPWindow), new PropertyMetadata(null));

        public static VirtualMenu GetViewMenu(DependencyObject element)
        {
            if (element is null)
            {
                throw new ArgumentNullException("element");
            }

            return (VirtualMenu)element.GetValue(ViewMenuProperty);
        }

        public static void SetViewMenu(DependencyObject element, VirtualMenu value)
        {
            if (element is null)
            {
                throw new ArgumentNullException("element");
            }

            element.SetValue(ViewMenuProperty, value);
        }

        public static readonly DependencyProperty ViewMenuProperty = DependencyProperty.RegisterAttached("ViewMenu", typeof(VirtualMenu), typeof(IPWindow), new PropertyMetadata(null));

        public VirtualMenu ViewMenu
        {
            get
            {
                return IPWindow.GetViewMenu(this);
            }

            set
            {
                IPWindow.SetViewMenu(this, value);
            }
        }

        public IPWindow()
        {

            // Dim mon As New DataTools.Interop.Display.Monitors

            // Dim icn = DataTools.Interop.Printers.PrinterObject.GetPrinterInfoObject("Brother MFC-7840W Printer")

            // Dim mm As DataTools.Memory.MemPtr = {"Hello", "Doctor", "Name"}
            // Dim p As String() = mm
            // mm.Free()

            this.InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.Title = "DataTools Interop Library Test Project";

            this.Loaded += IPWindow_Loaded;

            this.Closing += IPWindow_Closing;
            Quit.Click += Quit_Click;
            ShowPrg.Click += ShowPrg_Click;
            ShowHw.Click += ShowHw_Click;
            ShowFS.Click += ShowFS_Click;
            ShowHID.Click += ShowHID_Click;
            ShowSysInfo.Click += ShowSysInfo_Click;
            AdapterList.SelectionChanged += AdapterList_SelectionChanged;


            // Dim x = BOM.UTF16LE

            // Dim txt = CPGlobal.SafeTextWrite("This is some text that I'm going to encode.  It's going to contain çalè", BOMTYPE.UTF8)


            // System.IO.File.WriteAllBytes("BOM32.txt", txt)


            // Dim bl As New MemPtr, bl2 As New MemPtr

            // Dim len = 293397669

            // bl.Alloc(len)

            // Dim rnd = New Random(DateTime.Now.Ticks And &H7FFFFFFF)

            // For i = 0 To len - 1
            // bl.ByteAt(i) = rnd.Next(0, 255)
            // Next


            // bl2.Alloc(len)

            // DataTools.Memory.NativeLib.Native.MemCpy(bl.Handle, bl2.Handle, len)

            // Dim iss = True

            // For i = 0 To len - 1
            // If (bl.ByteAt(i) <> bl2.ByteAt(i)) Then
            // iss = False
            // Exit For
            // End If

            // Next


            // bl.Free()
            // bl2.Free()

            // rnd = Nothing


            // Dim bthRadio = EnumBluetoothRadios()

            // Dim bthDev = EnumBluetoothDevices()

            // BluetoothDeviceInfo.ShowBluetoothSettings()

            // Dim bl As SafePtr = System.Text.UTF8Encoding.UTF8.GetBytes("Hello World")

            // bl.ReAlloc(bl.Length + 2)


            // Dim s As String = bl.GrabUtf8String(0)

            // bl.Free()


            // Dim bl As Blob
            // Blob.TryParseObject("0.94494943511338", bl)


            // Dim fra As Decimal = 5 + 54 / 8
            // Dim fras As String = DataTools.ExtendedMath.PrintFraction(fra, , 40)

            // MsgBox(DataTools.ExtendedMath.PrintFraction(10.1375, , 100))
            // MsgBox("Print Fraction Demo #1: " & fras)


            // fra = 551 + 23 / 129
            // fras = DataTools.ExtendedMath.PrintFraction(fra, 14, 130)

            // MsgBox("Print Fraction Demo #2: " & fras)


        }

        private void AdapterList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            // Dim wr As NetworkAdapter = AdapterList.SelectedItem

            // If wr IsNot Nothing Then
            // _props.SelectedObject = wr
            // End If

            var wr = AdapterList.SelectedItem;

            if (wr != null)
            {
                _props.SelectedObject = wr;
            }
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void ShowPrg_Click(object sender, RoutedEventArgs e)
        {
            ProgramsWindow = new Programs();
            ProgramsWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ProgramsWindow.Show();
        }

        private void ShowHw_Click(object sender, RoutedEventArgs e)
        {
            HardwareWindow = new Hardware();
            HardwareWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            HardwareWindow.Show();
        }

        private void ShowFS_Click(object sender, RoutedEventArgs e)
        {
            FSWindow = new FSMonTestWindow();
            FSWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            FSWindow.Show();
        }

        private void IPWindow_Closing(object sender, CancelEventArgs e)
        {
            if (HardwareWindow is object)
                HardwareWindow.Close();

            if (ProgramsWindow is object)
                ProgramsWindow.Close();
        }

        private void ShowHID_Click(object sender, RoutedEventArgs e)
        {
            CodeEx = new CodeExplorer();
            CodeEx.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            CodeEx.Show();
        }

        private void ShowSysInfo_Click(object sender, RoutedEventArgs e)
        {
            SysInfoWindow = new SysInfoWindow();
            SysInfoWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            SysInfoWindow.Show();
        }

        private void IPWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _Adapters = new AdaptersCollection();
            this.AdapterList.ItemsSource = _Adapters.Collection;
            ViewMenu = new VirtualMenu(this, this.AdapterList);
            this.netMenu.ItemsSource = ViewMenu;
        }
    }

    public class VirtualMenu : ObservableCollection<MenuItem>, INotifyPropertyChanged
    {
        private ListView __View;

        private ListView _View
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return __View;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (__View != null)
                {
                    __View.SelectionChanged -= SelectionChanged;
                }

                __View = value;
                if (__View != null)
                {
                    __View.SelectionChanged += SelectionChanged;
                }
            }
        }

        private WindowInteropHelper _wih;
        private ObservableCollection<MenuItem> _swapCol = new ObservableCollection<MenuItem>();
        private MenuItem _devMenu;
        private MenuItem _netMenu;
        private MenuItem _statusMenu;

        private void CheckDevice()
        {
            _devMenu.IsEnabled = SelectedItem.CanShowDeviceDialog;
            _netMenu.IsEnabled = SelectedItem.CanShowNetworkDialogs;
            _statusMenu.IsEnabled = SelectedItem.CanShowNetworkDialogs;
        }

        internal VirtualMenu(Window window, ListView view)
        {
            _View = view;
            _wih = new WindowInteropHelper(window);
            _netMenu = new MenuItem() { Header = "Show Network Properties Dialog" };
            _statusMenu = new MenuItem() { Header = "Show Network Status Dialog" };
            _devMenu = new MenuItem() { Header = "Show Device Properties Dialog" };
            _netMenu.Click += netConnProps;
            _statusMenu.Click += netStatus;
            _devMenu.Click += netDeviceProps;
            Add(_netMenu);
            Add(_statusMenu);
            Add(_devMenu);


            // Dim tKey As String = "::{A8A91A66-3A7D-4424-8D24-04E180695C7A}"
            // Dim sKey As String = "::{20D04FE0-3AEA-1069-A2D8-08002B30309D}\::{21EC2020-3AEA-1069-A2DD-08002B30309D}\::{A8A91A66-3A7D-4424-8D24-04E180695C7A}"
            // Dim mm As New DataTools.Memory.MemPtr
            // Dim ip As IntPtr

            // Dim lFile As New List(Of IShellItem)
            // Dim lName As New List(Of ShDisplayNames)

            // SHParseDisplayName(sKey, IntPtr.Zero, ip, 0, 0)

            // If ip <> 0 Then
            // Dim fld As IShellFolder = Nothing

            // SHBindToObject(Nothing, ip, 0, New Guid(ShellIIDGuid.IShellFolder), fld)
            // Marshal.FreeCoTaskMem(ip)
            // ip = 0

            // Dim enumer As IEnumIDList = Nothing

            // fld.EnumObjects(0, ShellFolderEnumerationOptions.Folders Or ShellFolderEnumerationOptions.IncludeHidden Or ShellFolderEnumerationOptions.NonFolders, enumer)

            // If enumer IsNot Nothing Then
            // Dim c As Integer = 0
            // Dim iidlist As IntPtr

            // Do
            // iidlist = 0
            // enumer.Next(1, iidlist, c)
            // If iidlist <> 0 Then

            // Dim pfile As IShellItem = Nothing
            // SHCreateItemWithParent(0, fld, iidlist, New Guid(ShellIIDGuid.IShellItem), pfile)
            // Marshal.FreeCoTaskMem(iidlist)

            // If pfile IsNot Nothing Then
            // lFile.Add(pfile)
            // Dim sh As New ShDisplayNames

            // pfile.GetDisplayName(ShellItemDesignNameOptions.DesktopAbsoluteParsing, mm)
            // sh.AbsoluteParsing = mm
            // mm.CoTaskMemFree()

            // pfile.GetDisplayName(ShellItemDesignNameOptions.ParentRelativeParsing, mm)
            // sh.RelativeParsing = mm
            // mm.CoTaskMemFree()

            // pfile.GetDisplayName(ShellItemDesignNameOptions.Normal, mm)
            // sh.Normal = mm
            // mm.CoTaskMemFree()

            // pfile.GetDisplayName(ShellItemDesignNameOptions.ParentRelativeEditing, mm)
            // sh.Editing = mm
            // mm.CoTaskMemFree()

            // lName.Add(sh)

            // End If

            // End If
            // Loop Until c = 0

            // c = 0

            // End If

            // End If

        }

        public struct ShDisplayNames
        {
            public string AbsoluteParsing;
            public string Normal;
            public string RelativeParsing;
            public string Editing;

            public override string ToString()
            {
                return Normal;
            }
        }

        public NetworkAdapter SelectedItem
        {
            get
            {
                return (NetworkAdapter)_View.SelectedItem;
            }
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckDevice();
        }

        private void netStatus(object sender, EventArgs e)
        {
            SelectedItem.ShowNetworkStatusDialog(_wih.Handle);
        }

        private void netConnProps(object sender, EventArgs e)
        {
            SelectedItem.ShowConnectionPropertiesDialog(_wih.Handle);
        }

        private void netDeviceProps(object sender, EventArgs e)
        {
            SelectedItem.DeviceInfo.ShowDevicePropertiesDialog(_wih.Handle);
        }
    }

    public class A
    {
        public string Prop1 { get; set; }
        public string Prop2 { get; set; }

        public static explicit operator A(Z value)
        {
            var n = new A();
            n.Prop1 = value.Prop1;
            n.Prop2 = value.Prop2;
            return n;
        }

        public static implicit operator Z(A value)
        {
            var n = new Z();
            n.Prop1 = value.Prop1;
            n.Prop2 = value.Prop2;
            return n;
        }
    }

    public class Z
    {
        public string Prop1 { get; set; }
        public string Prop2 { get; set; }
        public string PropZ { get; set; }
    }

    public class B : A
    {
        public string Prop3 { get; set; }
        public string Prop4 { get; set; }
    }
}