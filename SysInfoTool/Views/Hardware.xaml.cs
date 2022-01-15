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
using DataTools.Hardware;

namespace SysInfoTool
{
    public partial class Hardware : Window
    {
        public ObservableCollection<object> Devices
        {
            get
            {
                return (ObservableCollection<object>)this.GetValue(DevicesProperty);
            }
        }

        private static readonly DependencyPropertyKey DevicesPropertyKey = DependencyProperty.RegisterReadOnly("Devices", typeof(ObservableCollection<object>), typeof(Hardware), new PropertyMetadata(null));


        public static readonly DependencyProperty DevicesProperty = DevicesPropertyKey.DependencyProperty;

        public Hardware()
        {
            this.InitializeComponent();

            // Dim drv() As DiskDeviceInfo = EnumDisks()

            // For Each d In drv
            // If d.VolumePaths IsNot Nothing AndAlso d.VolumePaths(0) = "J:\" Then

            // Dim c2 As Long = d.SizeFree

            // Dim c1 As Long = d.Size
            // Dim c3 As Long = d.SizeUsed

            // Dim s As String = d.Size


            // End If
            // Next
            // drv(0).ToString()

            this.Loaded += Hardware_Loaded;
            ViewingArea.SelectionChanged += ViewingArea_SelectionChanged;
            ProgramList.MouseDoubleClick += ProgramList_MouseDoubleClick;
            _Quit.Click += _Quit_Click;
            _Close.Click += _Close_Click;
        }

        private void ViewingArea_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _props.SelectedObject = ViewingArea.SelectedItem;
        }

        private void _Quit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void _Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ProgramList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.ProgramList.SelectedItem is DeviceInfo)
            {
                ((DeviceInfo)this.ProgramList.SelectedItem).ShowDevicePropertiesDialog();
            }
        }

        private void Hardware_Loaded(object sender, RoutedEventArgs e)
        {

            Task.Run(() =>
            {
                var hc = HardwareCollection.CreateComputerHierarchy();
                Dispatcher.Invoke(() => this.SetValue(DevicesPropertyKey, hc));
                ;
            });
            
        }
    }
}