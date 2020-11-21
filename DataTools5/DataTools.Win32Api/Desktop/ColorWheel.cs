using DataTools.MathTools.PolarMath;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTools.Win32Api;
using System.Runtime.InteropServices;
using DataTools.Memory;

namespace DataTools.Desktop
{
    public enum ColorPickerMode
    {
        Wheel = 0,
        LinearHorizontal = 1,
        LinearVertical = 2
    }

    public enum ColorWheelShapes
    {
        Point = 1,
        Hexagon = 2
    }

    public class ColorWheel
    {
        public List<ColorWheelElement> Elements { get; private set; } = new List<ColorWheelElement>();

        public ColorPickerMode Mode { get; private set; }

        public Rectangle Bounds { get; private set; }

        public bool InvertSaturation { get; private set; }

        private byte[] imageBytes;
        
        public byte[] ImageBytes
        {
            get => imageBytes;
            private set
            {
                imageBytes = value;
            }
        }

        public Bitmap Bitmap { get; private set; }

        private void ToBitmap()
        {
            if (imageBytes == null) return;

            var mm = new MemPtr();
            var bmp = new Bitmap(Bounds.Width, Bounds.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            
            mm.Alloc(bmp.Width * bmp.Height * 4);

            var bm = new System.Drawing.Imaging.BitmapData();

            bm.Scan0 = mm.Handle;
            bm.Stride = bmp.Width * 4;

            bm = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite | System.Drawing.Imaging.ImageLockMode.UserInputBuffer, System.Drawing.Imaging.PixelFormat.Format32bppArgb, bm);

            mm.FromByteArray(ImageBytes);

            bmp.UnlockBits(bm);
            mm.Free();

            Bitmap = bmp;
        }

        public Color HitTest(int x, int y)
        {
            foreach (ColorWheelElement e in Elements)
            {
                foreach (Point f in e.FillPoints)
                {
                    if (f.X == x & f.Y == y)
                        return e.Color;
                }
            }

            return Color.Empty;
        }

        public Color HitTest(Point pt)
        {
            return HitTest(pt.X, pt.Y);
        }

        public ColorWheel(int width, int height, double value = 1d, double offset = 0d, bool invert = false, bool vertical = false)
        {

            List<int> rawColors = new List<int>();

            int x1 = 0;
            int x2 = width;

            int y1 = 0;
            int y2 = height;

            PolarCoordinates pc;
            HSVDATA hsv;

            int color;

            Bounds = new Rectangle(0, 0, x2, y2);

            InvertSaturation = invert;

            if (vertical)
            {
                Mode = ColorPickerMode.LinearVertical;
            }
            else
            {
                Mode = ColorPickerMode.LinearHorizontal;
            }

            for (int j = y1; j < y2; j++)
            {
                for (int i = x1; i < x2; i++)
                {
                    double arc;

                    if (vertical)
                    {
                        arc = ((double)j / y2) * 360;
                        arc -= offset;
                        if (arc < 0) arc += 360;

                        hsv = new HSVDATA()
                        {
                            Hue = arc,
                            Saturation = invert ? 1 - ((double)i / x2) : ((double)i / x2),
                            Value = value
                        };


                    }
                    else
                    {
                        arc = ((double)i / x2) * 360;
                        arc -= offset;
                        if (arc < 0) arc += 360;

                        hsv = new HSVDATA()
                        {
                            Hue = arc,
                            Saturation = invert ? 1 - ((double)j/y2) : ((double)j/y2),
                            Value = value
                        };
                    }

                    color = ColorMath.HSVToColorRaw(hsv);

                    var el = new ColorWheelElement();

                    el.FillPoints = new Point[1] { new Point(i, j) };
                    el.Color = Color.FromArgb(color);
                    el.Shape = ColorWheelShapes.Point;
                    el.Bounds = new Rectangle(i, j, 1, 1);
                    el.Center = el.FillPoints[0];

                    Elements.Add(el);
                    rawColors.Add(color);
                }
            }

            var arrColors = rawColors.ToArray();
            imageBytes = new byte[arrColors.Length * sizeof(int)];

            unsafe
            {
                var gch1 = GCHandle.Alloc(arrColors, GCHandleType.Pinned);
                var gch2 = GCHandle.Alloc(imageBytes, GCHandleType.Pinned);

                Buffer.MemoryCopy((void*)gch1.AddrOfPinnedObject(), (void*)gch2.AddrOfPinnedObject(), imageBytes.Length, imageBytes.Length);

                gch1.Free();
                gch2.Free();

            }

            ToBitmap();
        }

        public ColorWheel(int pixelRadius, double rotation = 0d, double value = 1d, bool invert = false)
        {
            List<int> rawColors = new List<int>();

            int x1 = 0;
            int x2 = pixelRadius * 2;

            int y1 = 0;
            int y2 = pixelRadius * 2;

            PolarCoordinates pc;
            HSVDATA hsv;

            int color;
            
            Bounds = new Rectangle(0, 0, x2, y2);
            InvertSaturation = invert;
            Mode = ColorPickerMode.Wheel;

            for (int j = y1; j < y2; j++)
            {
                for (int i = x1; i < x2; i++)
                {

                    if (i == pixelRadius && j == pixelRadius)
                    {
                        var s = true;
                    }

                    pc = PolarCoordinates.ToPolarCoordinates(i - pixelRadius, j - pixelRadius);
                    if (pc.Radius > pixelRadius)
                    {
                        rawColors.Add(0);
                        continue;
                    }
                    if (double.IsNaN(pc.Arc))
                    {
                        color = -1;
                    }
                    else
                    {
                        double arc = pc.Arc - rotation;
                        if (arc < 0) arc += 360;

                        hsv = new HSVDATA()
                        {
                            Hue = arc,
                            Saturation = invert ? 1 - (pc.Radius / pixelRadius) : (pc.Radius / pixelRadius),
                            Value = value
                        };

                        color = ColorMath.HSVToColorRaw(hsv);
                    }

                    var el = new ColorWheelElement();

                    el.FillPoints = new Point[1] { new Point(i, j) };
                    el.Color = Color.FromArgb(color);
                    el.Polar = pc;
                    el.Shape = ColorWheelShapes.Point;
                    el.Bounds = new Rectangle(i, j, 1, 1);
                    el.Center = el.FillPoints[0];
                    
                    Elements.Add(el);
                    rawColors.Add(color);
                }

            }

            var arrColors = rawColors.ToArray();
            imageBytes = new byte[arrColors.Length * sizeof(int)];

            unsafe
            {
                var gch1 = GCHandle.Alloc(arrColors, GCHandleType.Pinned);
                var gch2 = GCHandle.Alloc(imageBytes, GCHandleType.Pinned);

                Buffer.MemoryCopy((void*)gch1.AddrOfPinnedObject(), (void*)gch2.AddrOfPinnedObject(), imageBytes.Length, imageBytes.Length);

                gch1.Free();
                gch2.Free();

            }

            ToBitmap();
        }
    }

    public struct ColorWheelElement
    {
        public Color Color;
        public PolarCoordinates Polar;
        public Point Center;
        public Rectangle Bounds;
        public Point[] FillPoints;
        public ColorWheelShapes Shape;
    }

}
