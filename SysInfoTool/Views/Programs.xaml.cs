using System;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using DataTools.Desktop;

namespace SysInfoTool
{
    public partial class Programs : Window
    {
        public AllSystemFileTypes FileTypes
        {
            get
            {
                return (AllSystemFileTypes)this.GetValue(FileTypesProperty);
            }

            set
            {
                this.SetValue(FileTypesProperty, value);
            }
        }

        public static readonly DependencyProperty FileTypesProperty = DependencyProperty.Register("FileTypes", typeof(AllSystemFileTypes), typeof(Programs), new PropertyMetadata(null));



        public Programs()
        {
            this.InitializeComponent();
            this.Status.Text = "Enumerating File Types...";
            _Quit.Click += _Quit_Click;
            _Close.Click += _Close_Click;
            Loaded += Programs_Loaded;

        }

        private void _Quit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void _Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TypeEnumerated(object sender, FileTypeEnumEventArgs e)
        {
            this.Dispatcher.Invoke(() => this.Status.Text = "Enumerated " + e.Index + " of " + e.Count + " types.  " + e.Type.Extension + " - " + e.Type.Description);
        }

        private void Programs_Loaded(object sender, RoutedEventArgs e)
        {
            var th = new System.Threading.Thread(() => this.Dispatcher.Invoke(() =>
                        {
                            FileTypes = new AllSystemFileTypes();
                            FileTypes.Populating += TypeEnumerated;
                            
                            this.Cursor = Cursors.Wait;
                            this.UpdateLayout();
                            
                            FileTypes.Populate();
                            FileTypes.Populating -= TypeEnumerated;
            
                            this.Cursor = Cursors.Arrow;
                            this.Status.Text = "Finished.";
                        }));

            th.SetApartmentState(System.Threading.ApartmentState.STA);
            th.IsBackground = true;
            th.Start();
        }
    }
}