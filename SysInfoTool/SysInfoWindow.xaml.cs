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

using DataTools.ColorControls;
using DataTools.Desktop.Unified;

namespace SysInfoTool
{
    public partial class SysInfoWindow : Window
    {
        public SysInfoWindow()
        {

            // This call is required by the designer.
            this.InitializeComponent();
            _Close.Click += _Close_Click;
            _Quit.Click += _Quit_Click;

            // Add any initialization after the InitializeComponent() call.

            this._props3.SelectedObject = DataTools.SystemInformation.SysInfo.OSInfo;

            this._props4.SelectedObject = DataTools.SystemInformation.SysInfo.LogicalProcessors;
            this._props.SelectedObject = DataTools.SystemInformation.SysInfo.NativeEnvironment;
            this._props2.SelectedObject = DataTools.SystemInformation.SysInfo.MemoryInfo;

        }

        private void _Quit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void _Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}