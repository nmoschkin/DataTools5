using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataTools.Graphics;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DataTools.XamarinForms.ColorControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ColorPicker : ContentView
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

        public static readonly BindableProperty HueOffsetProperty =
            BindableProperty.Create(nameof(HueOffset), typeof(double), typeof(ColorPicker), defaultValue: 0d, propertyChanged: HueOffsetPropertyChanged);

        private static void HueOffsetPropertyChanged(BindableObject d, object oldValue, object newValue)
        {
            if (d is ColorPicker p)
            {
                if ((double)oldValue != (double)newValue)
                {
                    //p.RenderPicker();
                    p.InvalidateLayout();
                }
            }
        }

        public string SelectedColorName
        {
            get { return (string)GetValue(SelectedColorNameProperty); }
            set { SetValue(SelectedColorNameProperty, value); }
        }

        public static readonly BindableProperty SelectedColorNameProperty =
            BindableProperty.Create(nameof(SelectedColorName), typeof(string), typeof(ColorPicker), defaultValue: "", propertyChanged: SelectedColorNamePropertyChanged);

        private static void SelectedColorNamePropertyChanged(BindableObject d, object oldValue, object newValue)
        {
            if (d is ColorPicker p)
            {
                string nv = (string)newValue;
                string ov = (string)oldValue;

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

        public Color SelectedColor
        {
            get { return (Color)GetValue(SelectedColorProperty); }
            set { SetValue(SelectedColorProperty, value); }
        }

        public static readonly BindableProperty SelectedColorProperty =
            BindableProperty.Create(nameof(SelectedColor), typeof(Color), typeof(ColorPicker), defaultValue: Color.Transparent, propertyChanged: SelectedColorPropertyChanged);

        private static void SelectedColorPropertyChanged(BindableObject d, object oldValue, object newValue)
        {
            if (d is ColorPicker p)
            {
                if ((Color)oldValue != (Color)newValue)
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

        public static readonly BindableProperty SelectedNamedColorsProperty =
            BindableProperty.Create(nameof(SelectedNamedColors), typeof(IReadOnlyCollection<NamedColor>), typeof(ColorPicker), defaultValue: new List<NamedColor>(), propertyChanged: SelectedNamedColorsPropertyChanged);

        private static void SelectedNamedColorsPropertyChanged(BindableObject d, object oldValue, object newValue)
        {
            if (d is ColorPicker p)
            {
                if ((IReadOnlyCollection<NamedColor>)oldValue != (IReadOnlyCollection<NamedColor>)newValue)
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

        // Using a BindableProperty as the backing store for InvertSaturation.  This enables animation, styling, binding, etc...
        public static readonly BindableProperty InvertSaturationProperty =
            BindableProperty.Create(nameof(InvertSaturation), typeof(bool), typeof(ColorPicker), defaultValue: false, propertyChanged: InvertSaturationPropertyChanged);

        private static void InvertSaturationPropertyChanged(BindableObject d, object oldValue, object newValue)
        {
            if (d is ColorPicker p)
            {
                if ((bool)oldValue != (bool)newValue)
                {
                    //p.RenderPicker();
                    p.InvalidateLayout();
                }
            }
        }




        public float ElementSize
        {
            get { return (float)GetValue(ElementSizeProperty); }
            set { SetValue(ElementSizeProperty, value); }
        }

        // Using a BindableProperty as the backing store for HexagonSize.  This enables animation, styling, binding, etc...
        public static readonly BindableProperty ElementSizeProperty =
            BindableProperty.Create(nameof(ElementSize), typeof(float), typeof(ColorPicker), defaultValue: 1f, propertyChanged: ElementSizePropertyChanged);

        private static void ElementSizePropertyChanged(BindableObject d, object oldValue, object newValue)
        {
            if (d is ColorPicker p)
            {
                if ((float)oldValue != (float)newValue)
                {
                    //p.RenderPicker();
                    p.InvalidateLayout();
                }
            }
        }


        public double ColorValue
        {
            get { return (double)GetValue(ColorValueProperty); }
            set { SetValue(ColorValueProperty, value); }
        }

        // Using a BindableProperty as the backing store for ColorValue.  This enables animation, styling, binding, etc...
        public static readonly BindableProperty ColorValueProperty =
            BindableProperty.Create(nameof(ColorValue), typeof(double), typeof(ColorPicker), defaultValue: 1d, propertyChanged: ColorValuePropertyChanged);

        private static void ColorValuePropertyChanged(BindableObject d, object oldValue, object newValue)
        {
            if (d is ColorPicker p)
            {
                if ((double)oldValue != (double)newValue)
                {
                    double nv = (double)newValue;

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
                    p.InvalidateLayout();
                }
            }
        }





        public ColorPickerMode Mode
        {
            get { return (ColorPickerMode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }

        // Using a BindableProperty as the backing store for Mode.  This enables animation, styling, binding, etc...
        public static readonly BindableProperty ModeProperty =
            BindableProperty.Create(nameof(Mode), typeof(ColorPickerMode), typeof(ColorPicker), defaultValue: ColorPickerMode.Wheel, propertyChanged: ModePropertyChanged);


        private static void ModePropertyChanged(BindableObject d, object oldValue, object newValue)
        {
            if (d is ColorPicker p)
            {
                if ((ColorPickerMode)oldValue != (ColorPickerMode)newValue)
                {
                    //p.RenderPicker();
                    p.InvalidateLayout();
                }
            }
        }

        public ColorPicker()
        {
            InitializeComponent();
        }


        protected override void OnParentSet()
        {
            base.OnParentSet();
            RenderPicker();

        }

        //protected override void OnRender(DrawingContext drawingContext)
        //{
        //    base.OnRender(drawingContext);
        //    RenderPicker();
        //    var p = new Pen();
        //}
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            RenderPicker((int)width, (int)height);
        }

        private void SetSelectedColor(UniColor? selc = null)
        {
            if (selc is UniColor clr)
            {
                SelectedColor = clr.GetXamarinColor();

                var nc = NamedColor.FindColor(clr, true);

                if (nc != null)
                {
                    SelectedColorName = nc.Name;
                }
                else
                {
                    SelectedColorName = clr.ToString(UniColorFormatOptions.HexRgbWebFormat);
                }

            }

            foreach (var c in cpRender.Elements)
            {
                UniColor uc = c.Color;

                if (selc == uc)
                {
                    Point.IsVisible = Surround.IsVisible = true;

                    Rectangle pb = new Rectangle(c.Center.X, c.Center.Y, 1, 1);

                    Point.SetValue(AbsoluteLayout.LayoutBoundsProperty, pb);

                    pb = new Rectangle(c.Center.X - 8, c.Center.Y - 8, 16, 16);

                    Surround.SetValue(AbsoluteLayout.LayoutBoundsProperty, pb);

                    Surround.Stroke = Point.Stroke = new SolidColorBrush(SelectedColor);

                    return;
                }
            }

            Point.IsVisible = Surround.IsVisible = false;
        }

        //private void PickerSite_MouseMove(object sender, MouseEventArgs e)
        //{
        //    if (ColorOver != null || ((e.LeftButton == MouseButtonState.Pressed) && (ColorHit != null)))
        //    {
        //        var pt = e.GetPosition(PickerSite);
        //        var c = cpRender.HitTest((int)pt.X, (int)pt.Y);
        //        var ev = new ColorHitEventArgs(c);

        //        ColorOver?.Invoke(this, ev);

        //        if (e.LeftButton == MouseButtonState.Pressed)
        //        {
        //            SetSelectedColor(c);
        //            ColorHit?.Invoke(this, ev);
        //        }

        //    }
        //}

        //private void PickerSite_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    var pt = e.GetPosition(PickerSite);
        //    var c = cpRender.HitTest((int)pt.X, (int)pt.Y);

        //    SetSelectedColor(c);
        //    ColorHit?.Invoke(this, new ColorHitEventArgs(c));

        //}

        private void RenderPicker(int w = 0, int h = 0)
        {
            var disp = Dispatcher;

            double width = this.Width;
            double height = this.Height;
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

                Device.BeginInvokeOnMainThread(() =>
                {
                    //CursorCanvas.Width = w;
                    //CursorCanvas.Height = h;

                    ////CursorCanvas.RenderSize = new Size(w, h);
                    //cpRender = cw;
                    //PickerSite.Source = DataTools.Desktop.BitmapTools.MakeWPFImage(cpRender.Bitmap);
                    //SetSelectedColor();
                });

            });
        }

        private void Tapped_Tapped(object sender, EventArgs e)
        {

        }
    }
}