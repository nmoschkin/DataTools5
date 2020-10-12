using DataTools.Memory;
using DataTools.Hardware;
using DataTools.Hardware.Usb;
using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace SysInfoTool
{
    public partial class CodeExplorer : Window
    {
        [DllImport("hid.dll")]
        private static extern bool HidD_GetFeature(IntPtr HidDeviceObject, IntPtr Buffer, int BufferLength);

        public ObservableCollection<DeviceInfo> Devices
        {
            get
            {
                return (ObservableCollection<DeviceInfo>)this.GetValue(DevicesProperty);
            }

            set
            {
                this.SetValue(DevicesProperty, value);
            }
        }

        public static readonly DependencyProperty DevicesProperty = DependencyProperty.Register("Devices", typeof(ObservableCollection<DeviceInfo>), typeof(CodeExplorer), new PropertyMetadata(null));

        public CodeExplorer(ObservableCollection<DeviceInfo> devices)
        {
            this.InitializeComponent();
            Devices = devices;
            internalInit();
        }

        public CodeExplorer()
        {
            this.InitializeComponent();

            _Quit.Click += _Quit_Click;
            _Close.Click += _Close_Click;
            this.Loaded += CodeExplorer_Loaded;
            this.Closing += CodeExplorer_Closing;
            DeviceSelect.SelectionChanged += DeviceSelect_SelectionChanged;
            _Stop.Click += _Stop_Click;

            Devices = new ObservableCollection<DeviceInfo>();
            internalInit();
        }

        private void internalInit()
        {
            var devs = HidFeatures.HidDevicesByUsage(HidUsagePage.Undefined);
            if (devs is null)
                return;
            foreach (var d in devs)
                Devices.Add(d);
        }

        private void _Quit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void _Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CodeExplorer_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_devThread is object)
            {
                StopWatching();
            }
        }

        private void CodeExplorer_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private Thread _devThread;
        private HidDeviceInfo _lastDevice;

        private void DeviceSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            HidDeviceInfo d = (HidDeviceInfo)this.DeviceSelect.SelectedItem;
            StartWatching(d);
        }

        private CancellationTokenSource cts;

        private void StartWatching(HidDeviceInfo d)
        {
            IntPtr h;
            int x = 0;
            int i = 0;
            if (_devThread is object)
                StopWatching();
            var s = new ObservableCollection<string>();
            _lastDevice = d;
            this.ViewingArea.ItemsSource = s;
            for (i = 0; i <= 255; i++)
                s.Add("");
            var th = new Thread(() =>
            {
                cts = new CancellationTokenSource();
                h = HidFeatures.OpenHid(d);
                if ((long)h <= 0L)
                    return;
                var mm = new MemPtr(65L);
                try
                {
                    do
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            for (i = 0; i <= 255; i++)
                            {
                                mm.LongAtAbsolute(1L) = 0L;
                                mm.ByteAt(0L) = (byte)i;
                                if (HidD_GetFeature(h, mm, 65))
                                {
                                    s[i] = "HID CODE " + i.ToString("X2") + " = " + mm.IntAtAbsolute(1L);
                                }
                                x = 0;

                            }
                        });

                        Thread.Sleep(1000);
                        if (cts is null || cts.IsCancellationRequested)
                            break;
                    }
                    while (true);
                    mm.Free();
                    HidFeatures.CloseHid(h);
                    cts = null;
                    return;
                }
                catch (ThreadAbortException tx)
                {
                    mm.Free();
                    HidFeatures.CloseHid(h);
                    cts = null;
                    return;
                }
                catch (Exception ex)
                {
                    mm.Free();
                    HidFeatures.CloseHid(h);
                    cts = null;
                    return;
                }
            });
            th.IsBackground = true;
            th.SetApartmentState(ApartmentState.STA);
            _devThread = th;
            th.Start();
        }

        private void StopWatching()
        {
            if (_devThread is object && cts is object)
            {
                cts.Cancel();
                this.ViewingArea.ItemsSource = null;
                _devThread = null;
            }
        }

        private void _Stop_Click(object sender, RoutedEventArgs e)
        {
            if ((string)((MenuItem)sender).Header == "_Stop Watching Device")
            {
                StopWatching();
                ((MenuItem)sender).Header = "_Start Watching Device";
            }
            else
            {
                if (_lastDevice is null)
                    return;
                StartWatching(_lastDevice);
                ((MenuItem)sender).Header = "_Stop Watching Device";
            }
        }
    }
}