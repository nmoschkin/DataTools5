using DataTools.Graphics;

using Microsoft.UI;
using Microsoft.UI.Composition;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Hosting;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

using Newtonsoft.Json.Converters;

using SkiaSharp;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.EnterpriseData;
using Windows.UI;
using Windows.UI.Core;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DataTools.WinUI.ColorControls
{
    public sealed partial class ColorElement : UserControl
    {
        ColorPickerRenderer cpRender;

        public delegate void ColorHitEvent(object sender, ColorHitEventArgs e);
        public event ColorHitEvent ColorHit;
        public event ColorHitEvent ColorOver;

        UniPoint? selCoord;

        public ColorElement()
        {
            InitializeComponent();
        }

        public bool SnapToNamedColor
        {
            get { return (bool)GetValue(SnapToNamedColorProperty); }
            set { SetValue(SnapToNamedColorProperty, value); }
        }

        public static readonly DependencyProperty SnapToNamedColorProperty =
            DependencyProperty.Register(nameof(SnapToNamedColor), typeof(bool), typeof(ColorElement), new PropertyMetadata(false, SnapToNamedColorPropertyChanged));

        private static void SnapToNamedColorPropertyChanged(object d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ColorElement p)
            {
                if ((bool)e.OldValue != (bool)e.NewValue)
                {
                    p.SetSelectedColor();
                }
            }
        }



        public double HueOffset
        {
            get { return (double)GetValue(HueOffsetProperty); }
            set { SetValue(HueOffsetProperty, value); }
        }

        public static readonly DependencyProperty HueOffsetProperty =
            DependencyProperty.Register(nameof(HueOffset), typeof(double), typeof(ColorElement), new PropertyMetadata(0d, HueOffsetPropertyChanged));

        private static void HueOffsetPropertyChanged(object d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ColorElement p)
            {
                if ((double)e.OldValue != (double)e.NewValue)
                {
                    p.RenderPicker();
                }
            }
        }

        public string SelectedColorName
        {
            get { return (string)GetValue(SelectedColorNameProperty); }
            set { SetValue(SelectedColorNameProperty, value); }
        }

        public static readonly DependencyProperty SelectedColorNameProperty =
            DependencyProperty.Register(nameof(SelectedColorName), typeof(string), typeof(ColorElement), new PropertyMetadata("", SelectedColorNamePropertyChanged));

        private static void SelectedColorNamePropertyChanged(object d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ColorElement p)
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

        public Color SelectedColor
        {
            get { return (Color)GetValue(SelectedColorProperty); }
            set { SetValue(SelectedColorProperty, value); }
        }

        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register(nameof(SelectedColor), typeof(Color), typeof(ColorElement), new PropertyMetadata(Colors.Transparent, SelectedColorPropertyChanged));

        private static void SelectedColorPropertyChanged(object d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ColorElement p)
            {
                if ((Color)e.OldValue != (Color)e.NewValue)
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
            DependencyProperty.Register(nameof(SelectedNamedColors), typeof(IReadOnlyCollection<NamedColor>), typeof(ColorElement), new PropertyMetadata(new List<NamedColor>(), SelectedNamedColorsPropertyChanged));

        private static void SelectedNamedColorsPropertyChanged(object d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ColorElement p)
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
            DependencyProperty.Register(nameof(InvertSaturation), typeof(bool), typeof(ColorElement), new PropertyMetadata(false, InvertSaturationPropertyChanged));

        private static void InvertSaturationPropertyChanged(object d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ColorElement p)
            {
                if ((bool)e.OldValue != (bool)e.NewValue)
                {
                    p.RenderPicker();
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
            DependencyProperty.Register(nameof(ElementSize), typeof(float), typeof(ColorElement), new PropertyMetadata(1f, ElementSizePropertyChanged));

        private static void ElementSizePropertyChanged(object d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ColorElement p)
            {
                if ((float)e.OldValue != (float)e.NewValue)
                {
                    p.RenderPicker();
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
            DependencyProperty.Register(nameof(ColorValue), typeof(double), typeof(ColorElement), new PropertyMetadata(1d, ColorValuePropertyChanged));

        private static void ColorValuePropertyChanged(object d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ColorElement p)
            {
                bool dee;

                if (e.OldValue == null && e.NewValue != null || e.OldValue != null && e.NewValue != null)
                    dee = true;
                else
                    dee = false;

                if (dee || (e.OldValue == null && e.NewValue == null) || ((double)e.OldValue != (double)e.NewValue))
                {
                    var nv = (double)e.NewValue;

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

                    p.RenderPicker();
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
            DependencyProperty.Register(nameof(Mode), typeof(ColorPickerMode), typeof(ColorElement), new PropertyMetadata(ColorPickerMode.Wheel, ModePropertyChanged));


        private static void ModePropertyChanged(object d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ColorElement p)
            {
                if ((ColorPickerMode)e.OldValue != (ColorPickerMode)e.NewValue)
                {
                    p.RenderPicker();
                }
            }
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var fs = base.ArrangeOverride(finalSize);
            var width = fs.Width;
            var height = fs.Height; 

            if (cpRender != null)
            {
                if (cpRender.Mode == ColorPickerMode.HexagonWheel || cpRender.Mode == ColorPickerMode.Wheel)
                {
                    float mn1 = width < height ? (float)width : (float)height;
                    float mn2 = cpRender.Bounds.Width;

                    if (mn1 == mn2) return fs;
                }
                else if (cpRender.Bounds.Width == width && cpRender.Bounds.Height == height)
                {
                    return fs;
                }
                else
                {
                    cpRender = null;
                }
            }

            RenderPicker((int)width, (int)height);
            return fs;
        }

        private void SetSelectedColor(UniColor? selc = null)
        {

            UniColor clr;
            string cname;

            if (selc != null)
            {
                clr = (UniColor)selc;
            }
            else
            {
                clr = SelectedColor.GetUniColor();
            }

            NamedColor nc;

            if (SnapToNamedColor)
            {
                nc = NamedColor.GetClosestColor(clr, 0.05, false);
            }
            else
            {
                nc = NamedColor.FindColor(clr);
            }

            if (nc != null)
            {
                cname = nc.Name;
                clr = nc.Color;
            }
            else
            {
                cname = clr.ToString("rwH");
            }

            if (SelectedColor != clr.GetWinUIColor())
            {
                SelectedColor = clr.GetWinUIColor();
            }

            if (SelectedColorName != cname)
            {
                SelectedColorName = cname;
            }

        }

        private void RenderPicker(int w = 0, int h = 0)
        {
            var disp = Dispatcher;

            double width = w > 0 ? w : this.Width;
            double height = h > 0 ? h : this.Height;

            if (width == -1 || height == -1) return;

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

                    cw = new ColorPickerRenderer(rad, colorVal, offset, invert, true);

                }
                else
                {

                    cw = new ColorPickerRenderer(w, h, colorVal, offset, invert, mode == ColorPickerMode.LinearVertical, true);

                }

                DispatcherQueue.TryEnqueue(async () =>
                {

                    SKImage img;
                    SKBitmap bmp = new SKBitmap((int)cw.Bounds.Width, (int)cw.Bounds.Height, SKColorType.Bgra8888, SKAlphaType.Premul);

                    var ptr = bmp.GetPixels();

                    unsafe
                    {
                        var gch = GCHandle.Alloc(cw.ImageBytes, GCHandleType.Pinned);

                        Buffer.MemoryCopy((void*)gch.AddrOfPinnedObject(), (void*)ptr, cw.ImageBytes.Length, cw.ImageBytes.Length);
                        gch.Free();
                    }

                    bmp.SetImmutable();
                    img = SKImage.FromBitmap(bmp);

                    SKData encoded = img.Encode();
                    Stream stream = encoded.AsStream();

                    //var ret = ImageSource.FromStream(() => stream);

                    cpRender = cw;
                    PickerSite.Source = await ImageLoader.LoadFromStream(stream);

                });

            });
        }
        private ContainerVisual GetVisual()
        {
            var hostVisual = ElementCompositionPreview.GetElementVisual(PickerSite);
            var root = hostVisual.Compositor.CreateContainerVisual();
            ElementCompositionPreview.SetElementChildVisual(PickerSite, root);

            return root;
        }

        CompositionEllipseGeometry currentGeo;
        CompositionSpriteShape currentSprite;
        private void RenderPickerZone()
        {

            var pt = (UniPoint)selCoord;
            var uc = SelectedColor.GetUniColor();

            uc = ((uint)uc) ^ 0x00ffffff;


            var container = GetVisual();
            var comp = container.Compositor;

            if (currentGeo != null)
            {
                currentGeo.Center = new Vector2((float)pt.X, (float)pt.Y);
                currentGeo.Radius = new Vector2(16f, 16f);

                if (currentSprite != null)
                {
                    currentSprite.StrokeBrush = comp.CreateColorBrush(uc.GetWinUIColor());
                    currentSprite.StrokeThickness = 1f;
                    currentSprite.FillBrush = comp.CreateColorBrush(((UniColor)0).GetWinUIColor());

                    return;
                }
            }


            var shape = comp.CreateShapeVisual();
            var ellipseGeo = comp.CreateEllipseGeometry();

            ellipseGeo.Center = new Vector2((float)pt.X, (float)pt.Y);
            ellipseGeo.Radius = new Vector2(16f, 16f);

            var sprite = comp.CreateSpriteShape(ellipseGeo);

            sprite.StrokeBrush = comp.CreateColorBrush(uc.GetWinUIColor());
            sprite.StrokeThickness = 1f;
            sprite.FillBrush = comp.CreateColorBrush(((UniColor)0).GetWinUIColor());

            shape.Shapes.Add(sprite);
            container.Children.InsertAtTop(shape);

            currentSprite = sprite;
            currentGeo = ellipseGeo;

        }

        bool colorDragging = false;

        private void Grid_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var pt = new UniPoint(e.GetCurrentPoint(this).Position.X, e.GetCurrentPoint(this).Position.Y);
            selCoord = pt;

            pt.X -= (Width - cpRender.Bounds.Width) / 2;
            pt.Y -= (Height - cpRender.Bounds.Height) / 2;

            if (!cpRender.Bounds.Contains((System.Drawing.PointF)pt))
            {
                selCoord = null;
                // PickerCanvas.InvalidateArrange();

                return;
            }

            var c = cpRender.HitTest((int)pt.X, (int)pt.Y);
            if (c == System.Drawing.Color.Empty)
            {
                selCoord = null;
                // PickerCanvas.InvalidateArrange();
                return;
            }

            // PickerCanvas.InvalidateArrange();
            SetSelectedColor(c);
            RenderPickerZone();
            CapturePointer(e.Pointer);
            colorDragging = true;

            ColorHit?.Invoke(this, new ColorHitEventArgs(c));
        }

        private void Grid_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            ReleasePointerCapture(e.Pointer);
            colorDragging = false;
        }

        private void Grid_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            var pt = new UniPoint(e.GetCurrentPoint(this).Position.X, e.GetCurrentPoint(this).Position.Y);
            selCoord = pt;

            pt.X -= (Width - cpRender.Bounds.Width) / 2;
            pt.Y -= (Height - cpRender.Bounds.Height) / 2;

            if (!cpRender.Bounds.Contains((System.Drawing.PointF)pt))
            {
                selCoord = null;
                // PickerCanvas.InvalidateArrange();

                return;
            }

            var c = cpRender.HitTest((int)pt.X, (int)pt.Y);

            if (c == System.Drawing.Color.Empty)
            {
                selCoord = null;
                if (ProtectedCursor == null || ProtectedCursor.Type != CoreCursorType.Arrow)
                {
                    ProtectedCursor = new CoreCursor(CoreCursorType.Arrow, 1);
                }
                // PickerCanvas.InvalidateArrange();
                return;
            }

            if (ProtectedCursor == null || ProtectedCursor.Type != CoreCursorType.Cross)
            {
                ProtectedCursor = new CoreCursor(CoreCursorType.Cross, 2);
            }


            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed && colorDragging)
            {
                RenderPickerZone();
                // PickerCanvas.InvalidateArrange();
                SetSelectedColor(c);
            }
        }
    }
}
