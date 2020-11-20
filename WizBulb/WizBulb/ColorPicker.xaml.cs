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
using DataTools.Desktop;
using DataTools.Desktop.Unified;

namespace WizBulb
{

    public class ColorHitEventArgs : EventArgs
    {
        public Color Color { get; private set; }

        public ColorHitEventArgs(int rawColor)
        {
            byte[] c = BitConverter.GetBytes(rawColor);
            Color = Color.FromArgb(c[0], c[1], c[2], c[3]);
        }

        public ColorHitEventArgs(System.Drawing.Color color)
        {
            Color = Color.FromArgb(color.A, color.R, color.G, color.B);
        }
    }

    /// <summary>
    /// Interaction logic for ColorPicker.xaml
    /// </summary>
    public partial class ColorPicker : UserControl
    {
        ColorWheel cwheel;

        public delegate void ColorHitEvent(object sender, ColorHitEventArgs e);
        public event ColorHitEvent ColorHit;

        public ColorPicker()
        {
            InitializeComponent();

            this.SizeChanged += ColorPicker_SizeChanged;
            PickerSite.MouseMove += PickerSite_MouseMove;
        }

        private void PickerSite_MouseMove(object sender, MouseEventArgs e)
        {
            if (ColorHit != null)
            {
                var pt = e.GetPosition(PickerSite);
                var c = cwheel.HitTest((int)pt.X, (int)pt.Y);
                ColorHit.Invoke(this, new ColorHitEventArgs(c));
            }
        }

        private void PickerSite_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (ColorHit != null)
            {
                var pt = e.GetPosition(PickerSite);
                var c = cwheel.HitTest((int)pt.X, (int)pt.Y);
                ColorHit.Invoke(this, new ColorHitEventArgs(c));
            }

        }

        private void ColorPicker_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            int h = (int)e.NewSize.Height;
            int w = (int)e.NewSize.Width;

            if (h <= 0 || w <= 0) return;

            int rad;

            if (h < w)
            {
                rad = h / 2;
            }
            else
            {
                rad = w / 2;
            }

            cwheel = new ColorWheel(rad);
            PickerSite.Source = BitmapTools.MakeWPFImage(cwheel.Bitmap);
        }
    }
}
