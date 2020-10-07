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

using DataTools.Interop.Desktop;
using DataTools.Memory.Internal;
using System.IO;

namespace AssetTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Explore.ItemClicked += Explore_ItemClicked;
            
        }

        private void Explore_ItemClicked(object sender, ExplorerItemClickedEventArgs e)
        {
            if (e.Item.IsFolder == false)
            {
                switch(Path.GetExtension(e.Item.ParsingName).ToLower())
                {
                    case ".jpg":
                    case ".bmp":
                    case ".png":
                    case ".jpeg":

                        var bmp = (System.Drawing.Bitmap)System.Drawing.Image.FromFile(e.Item.ParsingName);
                        IntPtr bit = IntPtr.Zero;

                        var wpf = DataTools.Interop.Desktop.Resources.MakeWPFImage(bmp, ref bit);

                        CurrentImage.Source = wpf;
                        NoImageLabel.Visibility = Visibility.Collapsed;
                        CurrentImage.Visibility = Visibility.Visible;

                        break;

                    default:

                        NoImageLabel.Visibility = Visibility.Visible;
                        CurrentImage.Visibility = Visibility.Collapsed;
                        
                        CurrentImage.Source = null;

                        break;

                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            Chopper.StartChop();
        }
    }
}
