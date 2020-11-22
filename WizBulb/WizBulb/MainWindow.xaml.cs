﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Runtime.InteropServices;   
using WizLib;
using System.Net.Sockets;
using System.Net;
using DataTools.Desktop.Unified;
using DataTools.Text;
using DataTools.ColorControls;
using DataTools.Hardware.Network;

namespace WizBulb
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public delegate void BulbDoer();

        [DllImport("kernel32.dll")]
        static extern int AllocConsole();
        
        
        public AdaptersCollection Networks { get; set; }

        public ObservableCollection<Bulb> Bulbs { get; set; }

        public MainWindow()
        {
            Bulb.HasConsole = AllocConsole() != 0;

            InitializeComponent();

            var loc = Settings.LastWindowLocation;
            var size = Settings.LastWindowSize;

            Left = loc.X;
            Top = loc.Y;
            
            Width = size.Width;
            Height = size.Height;

            //var b = new Bulb("192.168.1.12");
            //var b2 = new Bulb("192.168.1.8");

            //PredefinedScene p = PredefinedScene.GoldenWhite;

            //List<Action> paras = new List<Action>();

            //paras.Add(() => { b.SetScene(p, 64); });
            //paras.Add(() => { b2.SetScene(p, 64); });
            //paras.Add(() => { b.SetScene(p, System.Drawing.Color.Aqua, 255); });
            //paras.Add(() => { b2.SetScene(p, System.Drawing.Color.Violet, 255); });

            //Parallel.Invoke(paras.ToArray());

            Picker.ColorHit += Picker_ColorHit;

            this.Loaded += MainWindow_Loaded;
            this.LocationChanged += MainWindow_LocationChanged;
            this.SizeChanged += MainWindow_SizeChanged;


            var disp = Dispatcher;

            _ = Task.Run(() =>
            {
                Networks = new AdaptersCollection();

                disp.Invoke(() =>
                {
                    cbIP.ItemsSource = Networks.Collection;
                   
                });
            });

        }

        private void Picker_ColorHit(object sender, ColorHitEventArgs e)
        {
            UniColor uc = e.Color;
            var nc = NamedColor.FindColor(uc, true);

            if (nc != null)
            {
                ColorText.Text = nc.Name;
                uc = nc.Color;
            }
            else
            {
                ColorText.Text = "";
            }

            ColorSwatch.Background = new SolidColorBrush((Color)uc);
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Settings.LastWindowSize = e.NewSize;
        }

        private void MainWindow_LocationChanged(object sender, EventArgs e)
        {
            Settings.LastWindowLocation = new Point(Left, Top);
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var loc = Settings.LastWindowLocation;
            var size = Settings.LastWindowSize;

            Left = loc.X;
            Top = loc.Y;

            Width = size.Width;
            Height = size.Height;

            var disp = Dispatcher;

            _ = Task.Run(async () =>
            {
                var bulbs = await Bulb.ScanForBulbs("192.168.1.10");

                if (bulbs != null && bulbs.Count > 0)
                {
                    disp.Invoke(() => {
                        Bulbs = new ObservableCollection<Bulb>(bulbs);
                        BulbCounter.Text = Bulbs?.Count.ToString() + " Bulbs";
                        BulbList.ItemsSource = Bulbs;
                    });
                }
            });

        }

        private void ValueSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Picker.ColorValue = ValueSlider.Value / 100;
        }
    }
}

