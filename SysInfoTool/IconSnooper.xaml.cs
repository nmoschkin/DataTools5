using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;    
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

using DataTools.Desktop;
using System.Collections.ObjectModel;

namespace SysInfoTool
{

    public class IconSize
    {
        public string Name { get; set; }
        public StandardIcons Value { get; set; }

        public IconSize(string name, StandardIcons value)
        {
            Name = name;
            Value = value;
        }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            if (obj is IconSize i)
            {
                return i.Value == Value && i.Name == Name;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return (int)Value + (Name?.GetHashCode() ?? (int)Value);
        }

    }

    /// <summary>
    /// Interaction logic for IconSnooper.xaml
    /// </summary>
    public partial class IconSnooper : Window
    {

        private ObservableCollection<IconSize> IconSizes = new ObservableCollection<IconSize>();

        public IconSnooper()
        {
            InitializeComponent();

            var flds = typeof(StandardIcons).GetFields(BindingFlags.Public | BindingFlags.Static);

            foreach (var f in flds)
            {
                StandardIcons si = (StandardIcons)f.GetValue(null);

                var s = si.ToString().Replace("Icon", "");

                int d = int.Parse(s);
                string n;

                switch (d)
                {
                    case 16:
                        n = "Small";
                        break;

                    case 32:
                        n = "Large";
                        break;

                    case 48:
                        n = "Extra Large";
                        break;

                    case 64:
                        n = "Extra Extra Large";
                        break;

                    case 128:
                        n = "Jumbo";
                        break;

                    case 256:
                        n = "Super Jumbo";
                        break;

                    default:
                        n = "";
                        break;

                }

                IconSizes.Add(new IconSize($"{d}x{d} {n} Icon", si));

            }

            cbIconSizes.ItemsSource = IconSizes;
            _Close.Click += _Close_Click;
            _Quit.Click += _Quit_Click;
        }

        private void _Quit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void _Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {

            var sb = new StringBuilder();

            sb.Append("Compatible Files (*.exe;*.dll;*.lib;*.ico)|*.exe;*.dll;*.lib;*.ico");
            sb.Append("|Executable Files (*.exe)|*.exe");
            sb.Append("|Dynamic Link Libraries (*.dll)|*.dll");
            sb.Append("|Libraries (*.lib)|*.lib");
            sb.Append("|Icons (*.ico)|*.ico");
            sb.Append("|Any File (*.*)|*.*");

            var dlg = new System.Windows.Forms.OpenFileDialog()
            {
                Filter = sb.ToString(),
                CheckFileExists = true,
                CheckPathExists = true,
                Title = "Open Resource"
            };


            if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            txtFilename.Text = dlg.FileName;

        }

        private void IconSizes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFilename.Text) || !File.Exists(txtFilename.Text))
            {
                MessageBox.Show("Select a valid file to continue.");
                return;
            }

            if (cbIconSizes.SelectedItem is IconSize sz)
            {
                ViewingArea.ItemsSource = null;
                ViewingArea.ItemsSource = DataTools.Desktop.Resources.LoadAllLibraryIcons(txtFilename.Text, sz.Value);
            }
            else
            {
                MessageBox.Show("Select an icon size to continue.");
                return;
            }


        }

        private void txtFilename_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFilename.Text) || File.Exists(txtFilename.Text))
            {
                txtFilename.BorderBrush = SystemColors.ActiveBorderBrush;
            }
            else
            {
                txtFilename.BorderBrush = Brushes.Red;
            }
        }
    }
}
