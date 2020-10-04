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
using System.IO;
using DataTools.Interop.Desktop;

namespace AssetTool
{
    /// <summary>
    /// Interaction logic for Explorer.xaml
    /// </summary>
    public partial class Explorer : UserControl
    {
        public Explorer()
        {
            InitializeComponent();
            Path = new DirectoryObject(@"MyComputerFolder", true);
        }

        public DirectoryObject Path
        {
            get { return (DirectoryObject)GetValue(PathProperty); }
            set { SetValue(PathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Path.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PathProperty =
            DependencyProperty.Register("Path", typeof(DirectoryObject), typeof(Explorer), new PropertyMetadata(null, OnPathPropertyChanged));

        private static void OnPathPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Explorer explore)
            {
                explore.InternalNavigateNow();
            }
        }

        private void InternalNavigateNow()
        {

        }
    }
}
