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
    public class ExplorerItemClickedEventArgs : EventArgs
    {

        public ISimpleShellItem Item { get; private set; }


        public ExplorerItemClickedEventArgs(ISimpleShellItem item)
        {
            Item = item;
        }

    }

    /// <summary>
    /// Interaction logic for Explorer.xaml
    /// </summary>
    public partial class Explorer : UserControl
    {

        public delegate void ItemClickedEvent(object sender, ExplorerItemClickedEventArgs e);
        public event ItemClickedEvent ItemClicked;

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
            if (e.NewValue is DirectoryObject item)
            {
                if (item.Folders.Count == 1 && string.IsNullOrEmpty(item.Folders.ElementAt(0).ParsingName)) item.Refresh();

                if (SelectedFolder != item)
                {
                    SelectedFolder = item;
                    ItemClicked?.Invoke(this, new ExplorerItemClickedEventArgs(item));
                }
            }
        }

        private void SystemTree_Expanded(object sender, RoutedEventArgs e)
        {
            var ti = e.OriginalSource as TreeViewItem;

            var item = ti.Header as DirectoryObject;
            if (item != null && item.Folders.Count == 1 && string.IsNullOrEmpty(item.Folders.ElementAt(0).ParsingName))
                item.Refresh();

        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F5 && e.KeyboardDevice.Modifiers == ModifierKeys.None)
            {
                SelectedFolder?.Refresh();
            }
        }

        private void FolderView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is ISimpleShellItem item)
            {
                ItemClicked?.Invoke(this, new ExplorerItemClickedEventArgs(item));
            }
        }
    }
}
