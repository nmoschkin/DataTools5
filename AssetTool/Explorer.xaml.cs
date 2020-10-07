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
            SystemTree.ItemsSource = (DirectoryObject)DirectoryObject.CreateRootView(StandardIcons.Icon16);
        }

        public DirectoryObject RootView
        {
            get { return (DirectoryObject)GetValue(RootViewProperty); }
            set { SetValue(RootViewProperty, value); }
        }

        public static readonly DependencyProperty RootViewProperty =
            DependencyProperty.Register("RootView", typeof(DirectoryObject), typeof(Explorer), new PropertyMetadata(null));


        public DirectoryObject SelectedFolder
        {
            get { return (DirectoryObject)GetValue(SelectedFolderProperty); }
            set { SetValue(SelectedFolderProperty, value); }
        }

        public static readonly DependencyProperty SelectedFolderProperty =
            DependencyProperty.Register("SelectedFolder", typeof(DirectoryObject), typeof(Explorer), new PropertyMetadata(null, OnSelectedFolderChanged));

        private static void OnSelectedFolderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Explorer explore)
            {
                explore.InternalNavigateNow();
            }
        }

        private void InternalNavigateNow()
        {
            
            FolderView.ItemsSource = SelectedFolder;
        }

        private void SystemTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is ISimpleShellItem item)
            {

                if (SelectedFolder != item)
                {
                    item.Refresh();
                    SelectedFolder = item as DirectoryObject;
                }
            }
        }

        private void SystemTree_Expanded(object sender, RoutedEventArgs e)
        {
            var ti = e.OriginalSource as TreeViewItem;

            var x = ti.Header as DirectoryObject;
            if (x != null) x.Refresh();

        }
    }
}
