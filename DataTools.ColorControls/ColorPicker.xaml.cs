using DataTools.Graphics;

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

namespace DataTools.ColorControls
{
    /// <summary>
    /// Interaction logic for ColorPicker.xaml
    /// </summary>
    public partial class ColorPicker : UserControl
    {

        ColorViewModel vm;

        public ColorViewModel ViewModel => vm;




        public System.Windows.Media.Color SelectedColor
        {
            get { return (System.Windows.Media.Color)GetValue(SelectedColorProperty); }
            set { SetValue(SelectedColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register("SelectedColor", typeof(System.Windows.Media.Color), typeof(ColorPicker), new PropertyMetadata(Colors.Black, OnColorChanged));

        private static void OnColorChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is ColorPicker cp)
            {
                cp.vm.SelectedColor = (Color)e.NewValue;
            }
        }

        public ColorPicker()
        {
            InitializeComponent();
            vm = new ColorViewModel(SelectedColor.GetUniColor());
            ControlGrid.DataContext = vm;
        }
    }
}
