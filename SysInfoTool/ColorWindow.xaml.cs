﻿using System;
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
using System.Windows.Shapes;

using DataTools.ColorControls;
using DataTools.Desktop.Unified;

namespace SysInfoTool
{
    /// <summary>
    /// Interaction logic for ColorWindow.xaml
    /// </summary>
    public partial class ColorWindow : Window
    {
        public ColorWindow()
        {
            InitializeComponent();
        }


        private void _Quit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void _Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void WheelPicker_ColorHit(object sender, DataTools.ColorControls.ColorHitEventArgs e)
        {
            NamedColor nc = NamedColor.FindColor(e.Color, true);
            WheelColor.Background = new SolidColorBrush(e.Color);

            if (nc != null)
            {
                WheelColorName.Content = nc.Name;
            }
            else
            {
                WheelColorName.Content = ((UniColor)e.Color).ToString();
            }
        }

        private void LinePicker_ColorHit(object sender, DataTools.ColorControls.ColorHitEventArgs e)
        {
            NamedColor nc = NamedColor.FindColor(e.Color, true);
            LineColor.Background = new SolidColorBrush(e.Color);

            if (nc != null)
            {
                LineColorName.Content = nc.Name;
            }
            else
            {
                LineColorName.Content = ((UniColor)e.Color).ToString();
            }
        }

    }
}