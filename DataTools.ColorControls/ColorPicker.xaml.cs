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

namespace DataTools.ColorControls
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
        public event ColorHitEvent ColorOver;

        public double HueOffset
        {
            get { return (double)GetValue(HueOffsetProperty); }
            set { SetValue(HueOffsetProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HueOffset.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HueOffsetProperty =
            DependencyProperty.Register("HueOffset", typeof(double), typeof(ColorPicker), new PropertyMetadata(0d, HueOffsetPropertyChanged));

        private static void HueOffsetPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ColorPicker p)
            {
                if ((double)e.OldValue != (double)e.NewValue)
                {
                    //p.RenderPicker();
                    p.InvalidateVisual();
                }
            }
        }



        public System.Windows.Media.Color SelectedColor
        {
            get { return (System.Windows.Media.Color)GetValue(SelectedColorProperty); }
            set { SetValue(SelectedColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register("SelectedColor", typeof(System.Windows.Media.Color), typeof(ColorPicker), new PropertyMetadata(Colors.Transparent, SelectedColorPropertyChanged));

        private static void SelectedColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ColorPicker p)
            {
                if ((System.Windows.Media.Color)e.OldValue != (System.Windows.Media.Color)e.NewValue)
                {
                    //p.RenderPicker();
                    p.SetSelectedColor();
                }
            }
        }

        public bool InvertSaturation
        {
            get { return (bool)GetValue(InvertSaturationProperty); }
            set { SetValue(InvertSaturationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for InvertSaturation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InvertSaturationProperty =
            DependencyProperty.Register("InvertSaturation", typeof(bool), typeof(ColorPicker), new PropertyMetadata(false, InvertSaturationPropertyChanged));

        private static void InvertSaturationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ColorPicker p)
            {
                if ((bool)e.OldValue != (bool)e.NewValue)
                {
                    //p.RenderPicker();
                    p.InvalidateVisual();
                }
            }
        }




        public float ElementSize
        {
            get { return (float)GetValue(HexagonSizeProperty); }
            set { SetValue(HexagonSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HexagonSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HexagonSizeProperty =
            DependencyProperty.Register("HexagonSize", typeof(float), typeof(ColorPicker), new PropertyMetadata(1f, HexagonSizePropertyChanged));

        private static void HexagonSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ColorPicker p)
            {
                if ((float)e.OldValue != (float)e.NewValue)
                {
                    //p.RenderPicker();
                    p.InvalidateVisual();
                }
            }
        }


        public double ColorValue
        {
            get { return (double)GetValue(ColorValueProperty); }
            set { SetValue(ColorValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ColorValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColorValueProperty =
            DependencyProperty.Register("ColorValue", typeof(double), typeof(ColorPicker), new PropertyMetadata(1d, ColorValuePropertyChanged));

        private static void ColorValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ColorPicker p)
            {
                if ((double)e.OldValue != (double)e.NewValue)
                {
                    double nv = (double)e.NewValue;

                    if (nv < 0d)
                    {
                        p.ColorValue = 0d;
                        return;
                    }
                    else if (nv > 1d)
                    {
                        p.ColorValue = 1d;
                        return;
                    }

                    //p.RenderPicker();
                    p.InvalidateVisual();
                }
            }
        }





        public ColorPickerMode Mode
        {
            get { return (ColorPickerMode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Mode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ModeProperty =
            DependencyProperty.Register("Mode", typeof(ColorPickerMode), typeof(ColorPicker), new PropertyMetadata(ColorPickerMode.Wheel, ModePropertyChanged));


        private static void ModePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ColorPicker p)
            {
                if ((ColorPickerMode)e.OldValue != (ColorPickerMode)e.NewValue)
                {
                    //p.RenderPicker();
                    p.InvalidateVisual();
                }
            }
        }

        public ColorPicker()
        {
            InitializeComponent();

            // this.SizeChanged += ColorPicker_SizeChanged;
            CursorCanvas.MouseMove += PickerSite_MouseMove;
            CursorCanvas.MouseDown += PickerSite_MouseDown;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            RenderPicker();
            var p = new Pen();
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            RenderPicker((int)sizeInfo.NewSize.Width, (int)sizeInfo.NewSize.Height);
        }

        private void PickerSite_MouseMove(object sender, MouseEventArgs e)
        {
            if (ColorOver != null || ((e.LeftButton == MouseButtonState.Pressed) && (ColorHit != null)))
            {
                var pt = e.GetPosition(PickerSite);
                var c = cwheel.HitTest((int)pt.X, (int)pt.Y);
                var ev = new ColorHitEventArgs(c);

                ColorOver?.Invoke(this, ev);

                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    ColorHit?.Invoke(this, ev);
                }

            }
        }

        private void PickerSite_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var pt = e.GetPosition(PickerSite);
            var c = cwheel.HitTest((int)pt.X, (int)pt.Y);

            SelectedColor = Color.FromArgb(c.A, c.R, c.G, c.B);

            ColorHit?.Invoke(this, new ColorHitEventArgs(c));

        }

        private void ColorPicker_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            RenderPicker((int)e.NewSize.Width, (int)e.NewSize.Height);
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            Point.Visibility = Surround.Visibility = Visibility.Hidden;
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            SetSelectedColor();
        }

        private void SetSelectedColor()
        {
            UniColor selc = SelectedColor;

            foreach (var c in cwheel.Elements)
            {
                UniColor uc = c.Color;
                if (selc == uc)
                {
                    Point.Visibility = Surround.Visibility = Visibility.Visible;

                    Point.SetValue(Canvas.LeftProperty, (double)c.Center.X);
                    Point.SetValue(Canvas.TopProperty, (double)c.Center.Y);

                    Surround.SetValue(Canvas.LeftProperty, (double)c.Center.X - 8);
                    Surround.SetValue(Canvas.TopProperty, (double)c.Center.Y - 8);

                    Surround.Stroke = Point.Stroke = new SolidColorBrush(SelectedColor);

                    return;
                }
            }

            Point.Visibility = Surround.Visibility = Visibility.Hidden;
        }


        private void RenderPicker(int w = 0, int h = 0)
        {
            if (w <= 0)
            {
                if (double.IsNaN(ActualWidth)) return;
                w = (int)ActualWidth;
            }
            if (h <= 0)
            {
                if (double.IsNaN(ActualHeight)) return;
                h = (int)ActualHeight;
            }

            if (w < 32 || h < 32) return;

            if (Mode == ColorPickerMode.Wheel || Mode == ColorPickerMode.HexagonWheel)
            {
                int rad;

                if (h < w)
                {
                    rad = h / 2;
                }
                else
                {
                    rad = w / 2;
                }

                if (Mode == ColorPickerMode.Wheel)
                {
                    cwheel = new ColorWheel(rad, ColorValue, HueOffset, InvertSaturation);
                }
                else
                {
                    cwheel = new ColorWheel(rad, ElementSize, ColorValue, InvertSaturation);
                }

            }
            else 
            {

                cwheel = new ColorWheel(w, h, ColorValue, HueOffset, InvertSaturation, Mode == ColorPickerMode.LinearVertical);

            }

            PickerSite.Source = BitmapTools.MakeWPFImage(cwheel.Bitmap);
            SetSelectedColor();
        }


    }
}
