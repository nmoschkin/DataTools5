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

namespace SysInfoUtil
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

            this._props.SelectedObject = CoreCT.SystemInformation.SysInfo.LogicalProcessors;

            long mcache = 0L;
            var lcache = new int[4];

            foreach (var fp in CoreCT.SystemInformation.SysInfo.LogicalProcessors)
            {
                if (fp.Relationship == CoreCT.SystemInformation.LOGICAL_PROCESSOR_RELATIONSHIP.RelationCache)
                {
                    if (fp.CacheDescriptor.Size > 1)
                    {
                        mcache += fp.CacheDescriptor.Size;
                        lcache[fp.CacheDescriptor.Level] += 1;
                    }

                    fp.ToString();
                }
            }
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