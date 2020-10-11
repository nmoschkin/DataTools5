using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using DataTools.Interop;

namespace InteropTest
{
    public partial class Hardware
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
            var hc = HardwareCollection.CreateComputerHierarchy();
            this.SetValue(DevicesPropertyKey, hc);
        }
    }
}