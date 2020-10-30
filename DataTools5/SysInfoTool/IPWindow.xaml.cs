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
using DataTools.Hardware;
using DataTools.Win32Api.Network;
using DataTools.Memory;
using DataTools.MathTools;
using DataTools.Text;

namespace SysInfoTool
{
    public partial class IPWindow : Window
    {
        private AdaptersCollection _Adapters;

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


            //string[] example = new string[] { "Test", "Fox", "Fox", "Feeling", "Table", "Rable", "Goaff", "Rable", "Echo", "Echo", "Echo", "Feeling", "Atomic", "Atomic", "Feeling", "Gable", "Rable", "Forlorn", "Widening", "Mumble", "Joan", "Deco", "Lorentz", "Scat", "Quixotic", "Glenda", "Verdant", "Nexus", "Gable", "Zebulon", "Rubric", "Covert", "Atomic", "Burden" };

            //var s = @"D:\Downloads\sample-2mb-text-file.txt";

            //var x = System.IO.File.ReadAllText(s);

            //var example = TextTools.Words(x, " ");

            //QuickSort.Sort(ref example);

            //var i = BinarySearch.Search(example, "glenda");
            int x = 1;
            var example = new Hazmonic[] { new Hazmonic("Test", x++), new Hazmonic("Feeling", x++), new Hazmonic("Feeling", x++), new Hazmonic("Fable", x++), new Hazmonic("Fox", x++), new Hazmonic("Goaff", x++), new Hazmonic("Rable", x++), new Hazmonic("Echo", x++), new Hazmonic("Echo", x++), new Hazmonic("Echo", x++), new Hazmonic("Feeling", x++), new Hazmonic("Atomic", x++), new Hazmonic("Atomic", x++), new Hazmonic("Feeling", x++), new Hazmonic("Gable", x++), new Hazmonic("Rable", x++), new Hazmonic("Forlorn", x++), new Hazmonic("Widening", x++), new Hazmonic("Mumble", x++), new Hazmonic("Joan", x++), new Hazmonic("Deco", x++), new Hazmonic("Lorentz", x++), new Hazmonic("Scat", x++), new Hazmonic("Quixotic", x++), new Hazmonic("Glenda", x++), new Hazmonic("Verdant", x++), new Hazmonic("Nexus", x++), new Hazmonic("Gable", x++), new Hazmonic("Zebulon", x++), new Hazmonic("Rubric", x++), new Hazmonic("Covert", x++), new Hazmonic("Atomic", x++), new Hazmonic("Burden", x++) };

            QuickSort.Sort(ref example, (a, b) =>
            {
                int z;

                if ((z = string.Compare(a.Text, b.Text)) == 0)
                {
                    z = a.Value - b.Value;
                }

                return z;

            });

            Hazmonic ret;

            var i = BinarySearch.Search(example, "Feeling", "Text", out ret);

        }

        public class Hazmonic
        {
            public string Text { get; set; }

            public int Value { get; set; }

            public Guid Guid { get; set; } = Guid.NewGuid();

            public Hazmonic(string t, int v = 0)
            {
                Text = t;
                Value = v;
            }

            public override string ToString()
            {
                return $"{Text}, Value: {Value}, Guid: {Guid:g}";
            }

        }

        private void AdapterList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
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

            Task.Run(() =>
            {

                var ada = new AdaptersCollection();


                Dispatcher.Invoke(() =>
                {
                    _Adapters = ada;
                    this.AdapterList.ItemsSource = _Adapters.Collection;

                    ViewMenu = new VirtualMenu(this, this.AdapterList);

                    this.netMenu.ItemsSource = ViewMenu;

                });

            });


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

}