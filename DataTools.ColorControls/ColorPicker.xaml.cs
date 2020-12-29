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
using DataTools.Graphics;

using static DataTools.Graphics.ColorMath;

namespace DataTools.ColorControls
{
 
    /// <summary>
    /// Interaction logic for ColorPicker.xaml
    /// </summary>
    public partial class ColorPicker : UserControl
    {
        ColorPickerRenderer cpRender;

        public delegate void ColorHitEvent(object sender, ColorHitEventArgs e);
        public event ColorHitEvent ColorHit;
        public event ColorHitEvent ColorOver;

        
        public double HueOffset
        {
            get { return (double)GetValue(HueOffsetProperty); }
            set { SetValue(HueOffsetProperty, value); }
        }

        public static readonly DependencyProperty HueOffsetProperty =
            DependencyProperty.Register(nameof(HueOffset), typeof(double), typeof(ColorPicker), new PropertyMetadata(0d, HueOffsetPropertyChanged));

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

        public string SelectedColorName
        {
            get { return (string)GetValue(SelectedColorNameProperty); }
            set { SetValue(SelectedColorNameProperty, value); }
        }

        public static readonly DependencyProperty SelectedColorNameProperty =
            DependencyProperty.Register(nameof(SelectedColorName), typeof(string), typeof(ColorPicker), new PropertyMetadata("", SelectedColorNamePropertyChanged));

        private static void SelectedColorNamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ColorPicker p)
            {
                string nv = (string)e.NewValue;
                string ov = (string)e.OldValue;

                if (ov != nv)
                {
                    if (string.IsNullOrEmpty(nv) || nv.StartsWith("#"))
                    {
                        p.SelectedNamedColors = new List<NamedColor>();
                    }
                    else
                    {
                        p.SelectedNamedColors = NamedColor.SearchByName(nv, true);
                    }
                }
            }
        }

        public Color? SelectedColor
        {
            get { return (Color?)GetValue(SelectedColorProperty); }
            set { SetValue(SelectedColorProperty, value); }
        }

        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register(nameof(SelectedColor), typeof(Color?), typeof(ColorPicker), new PropertyMetadata(null, SelectedColorPropertyChanged));

        private static void SelectedColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ColorPicker p)
            {
                if ((Color?)e.OldValue != (Color?)e.NewValue)
                {
                    p.SetSelectedColor();
                }
            }
        }


        public IReadOnlyCollection<NamedColor> SelectedNamedColors
        {
            get { return (IReadOnlyCollection<NamedColor>)GetValue(SelectedNamedColorsProperty); }
            set { SetValue(SelectedNamedColorsProperty, value); }
        }

        public static readonly DependencyProperty SelectedNamedColorsProperty =
            DependencyProperty.Register(nameof(SelectedNamedColors), typeof(IReadOnlyCollection<NamedColor>), typeof(ColorPicker), new PropertyMetadata(new List<NamedColor>(), SelectedNamedColorsPropertyChanged));

        private static void SelectedNamedColorsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ColorPicker p)
            {
                if ((IReadOnlyCollection<NamedColor>)e.OldValue != (IReadOnlyCollection<NamedColor>)e.NewValue)
                {
                    //p.RenderPicker();
                    
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
            DependencyProperty.Register(nameof(InvertSaturation), typeof(bool), typeof(ColorPicker), new PropertyMetadata(false, InvertSaturationPropertyChanged));

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
            get { return (float)GetValue(ElementSizeProperty); }
            set { SetValue(ElementSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HexagonSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ElementSizeProperty =
            DependencyProperty.Register(nameof(ElementSize), typeof(float), typeof(ColorPicker), new PropertyMetadata(1f, ElementSizePropertyChanged));

        private static void ElementSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
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
            DependencyProperty.Register(nameof(ColorValue), typeof(double), typeof(ColorPicker), new PropertyMetadata(1d, ColorValuePropertyChanged));

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
            DependencyProperty.Register(nameof(Mode), typeof(ColorPickerMode), typeof(ColorPicker), new PropertyMetadata(ColorPickerMode.Wheel, ModePropertyChanged));


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

        private void SetSelectedColor(UniColor? selc = null)
        {
            UniColor clr;

            if (selc != null) 
            {
                clr = (UniColor)selc;
                SelectedColor = Color.FromArgb(clr.A, clr.R, clr.G, clr.B);

                return;
            }
            else if (SelectedColor == null)
            {
                SelectedColorName = null;
                Point.Visibility = Surround.Visibility = Visibility.Hidden;
                return;
            }

            clr = ((Color)SelectedColor).GetUniColor();

            var nc = NamedColor.FindColor(clr, true);

            if (nc != null)
            {
                SelectedColorName = nc.Name;
            }
            else
            {
                SelectedColorName = clr.ToString(UniColorFormatOptions.HexRgbWebFormat);
            }


            HSVDATA hsv1 = ColorToHSV(clr);
            HSVDATA hsv2;
            HSVDATA hsv3;
            HSVDATA? hsv4 = null;
            UniColor uc;

            ColorWheelElement cel = new ColorWheelElement();

            foreach (var c in cpRender.Elements)
            {
                uc = c.Color;

                hsv2 = ColorToHSV(uc);
                hsv3 = (hsv1 - hsv2).Abs();

                if (hsv4 == null)
                {
                    hsv4 = hsv3;
                }
                else if (hsv3 < hsv4)
                {
                    hsv4 = hsv3;
                    cel = c;
                }

                if (selc == uc)
                {
                    cel = c;
                    break;
                }
            }

            Point.Visibility = Surround.Visibility = Visibility.Visible;

            Point.SetValue(Canvas.LeftProperty, (double)cel.Center.X);
            Point.SetValue(Canvas.TopProperty, (double)cel.Center.Y);

            Surround.SetValue(Canvas.LeftProperty, (double)cel.Center.X - 8);
            Surround.SetValue(Canvas.TopProperty, (double)cel.Center.Y - 8);

            Surround.Stroke = Point.Stroke = new SolidColorBrush((Color)SelectedColor);

        }

        private void PickerSite_MouseMove(object sender, MouseEventArgs e)
        {
            if (ColorOver != null || ((e.LeftButton == MouseButtonState.Pressed) && (ColorHit != null)))
            {
                var pt = e.GetPosition(PickerSite);
                var c = cpRender.HitTest((int)pt.X, (int)pt.Y);
                var ev = new ColorHitEventArgs(c);

                ColorOver?.Invoke(this, ev);

                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    SetSelectedColor(c);
                    ColorHit?.Invoke(this, ev);
                }

            }
        }

        private void PickerSite_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var pt = e.GetPosition(PickerSite);
            var c = cpRender.HitTest((int)pt.X, (int)pt.Y);
            
            SetSelectedColor(c);
            ColorHit?.Invoke(this, new ColorHitEventArgs(c));

        }

        private void ColorPicker_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //RenderPicker((int)e.NewSize.Width, (int)e.NewSize.Height);
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

        private void RenderPicker(int w = 0, int h = 0)
        {
            var disp = Dispatcher;

            double width = this.ActualWidth;
            double height = this.ActualHeight;
            double colorVal = this.ColorValue;
            double offset = this.HueOffset;
            bool invert = this.InvertSaturation;
            float esize = this.ElementSize;
            ColorPickerMode mode = this.Mode;

            _ = Task.Run(() =>
            {
                ColorPickerRenderer cw;

                if (w <= 0)
                {
                    if (double.IsNaN(width)) return;
                    w = (int)width;
                }
                if (h <= 0)
                {
                    if (double.IsNaN(height)) return;
                    h = (int)height;
                }

                if (w < 32 || h < 32) return;

                if (mode == ColorPickerMode.Wheel || mode == ColorPickerMode.HexagonWheel)
                {
                    int rad;

                    if (h < w)
                    {
                        rad = h / 2;
                        w = h;
                    }
                    else
                    {
                        rad = w / 2;
                        h = w;
                    }

                    if (mode == ColorPickerMode.Wheel)
                    {
                        cw = new ColorPickerRenderer(rad, colorVal, offset, invert);
                    }
                    else
                    {
                        cw = new ColorPickerRenderer(rad, esize, colorVal, invert);
                    }

                }
                else
                {

                    cw = new ColorPickerRenderer(w, h, colorVal, offset, invert, mode == ColorPickerMode.LinearVertical);

                }

                disp.Invoke(() =>
                {
                    CursorCanvas.Width = w;
                    CursorCanvas.Height = h;

                    //CursorCanvas.RenderSize = new Size(w, h);
                    cpRender = cw;
                    PickerSite.Source = DataTools.Desktop.BitmapTools.MakeWPFImage(cpRender.Bitmap);
                    SetSelectedColor();
                });

            });
        }


    }
}
