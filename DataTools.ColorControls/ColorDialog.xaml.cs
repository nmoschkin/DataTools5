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
using System.Windows.Shapes;

using DataTools.ColorControls;
using DataTools.Graphics;

namespace DataTools.ColorControls
{
    /// <summary>
    /// Interaction logic for ColorDialog.xaml
    /// </summary>
    public partial class ColorDialog : Window
    {
        public ColorDialog()
        {
            InitializeComponent();

        }

        public static Color? OpenDialog(Color? current = null)
        {
            var dlg = new ColorDialog();
            dlg.SelectedColor = current ?? Colors.White;
            dlg.ShowDialog();

            if (dlg.DialogResult == true)
            {
                return dlg.SelectedColor;
            }
            else
            {
                return null;
            }

        }

        public Color SelectedColor
        {
            get { return (Color)GetValue(SelectedColorProperty); }
            set { SetValue(SelectedColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register("SelectedColor", typeof(Color), typeof(ColorDialog), new PropertyMetadata(Colors.Black));

        private void OKBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }
    }
}
