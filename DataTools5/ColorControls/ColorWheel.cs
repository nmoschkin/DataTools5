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
        LinearVertical = 2,
        HexagonWheel = 3
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

        public RectangleF Bounds { get; private set; }

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
            var bmp = new Bitmap((int)Math.Ceiling(Bounds.Width), (int)Math.Ceiling(Bounds.Height), System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            
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

        static bool PointInPolygon(PointF[] fillPoints, float x, float y)
        {
            // being quite honest here... I didn't even try to walk myself through this
            // I just copied it from the internet, and it works.
            // Reference: http://alienryderflex.com/polygon/

            int i, j = fillPoints.Length - 1;
            bool oddNodes = false;

            for (i = 0; i < fillPoints.Length; i++)
            {
                if (fillPoints[i].Y < y && fillPoints[j].Y >= y
                || fillPoints[j].Y < y && fillPoints[i].Y >= y)
                {
                    if (fillPoints[i].X + (y - fillPoints[i].Y) / (fillPoints[j].Y - fillPoints[i].Y) * (fillPoints[j].X - fillPoints[i].X) < x)
                    {
                        oddNodes = !oddNodes;
                    }
                }
                j = i;
            }

            return oddNodes;
        }

        public Color HitTest(int x, int y)
        {
            foreach (ColorWheelElement e in Elements)
            {
                if (e.FillPoints.Length == 1)
                {
                    var f = e.FillPoints[0];                     
                    if (f.X == x & f.Y == y)
                        return e.Color;
                }
                else
                {
                    if (PointInPolygon(e.FillPoints, x, y))
                    {
                        return e.Color;
                    }
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

                    el.FillPoints = new PointF[1] { new PointF(i, j) };
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

        public ColorWheel(int pixelRadius, float elementSize, double value = 1d, bool invert = false)
        {
            float x1 = elementSize / 2;
            float y1 = x1;

            float cor = 0f;

            float x2 = pixelRadius * 2;
            float y2 = x2;

            float stepX = elementSize + (elementSize / 2f);
            float stepY = (elementSize / 2f);

            PointF[] masterPoly = new PointF[6];

            var pc = new PolarCoordinates();
            HSVDATA hsv;

            int color;
            int z;

            Bounds = new RectangleF(0, 0, pixelRadius * 2, pixelRadius * 2);
            InvertSaturation = invert;
            Mode = ColorPickerMode.HexagonWheel;

            var bmp = new Bitmap((int)Math.Ceiling(Bounds.Width), (int)Math.Ceiling(Bounds.Height), System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var g = Graphics.FromImage(bmp);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;

            SolidBrush br;

            br = new SolidBrush(Color.FromArgb(unchecked((int)0xffffffff)));
            g.FillRectangle(br, Bounds);
            br.Dispose();

            bool alt = false;


            pc.Arc = -30;
            pc.Radius = pixelRadius;

            masterPoly[0] = pc.ToScreenCoordinates(Bounds);

            cor = masterPoly[0].Y;

            pc.Arc = 60 - 30;
            masterPoly[1] = pc.ToScreenCoordinates(Bounds);

            pc.Arc = 120 - 30;
            masterPoly[2] = pc.ToScreenCoordinates(Bounds);

            pc.Arc = 180 - 30;
            masterPoly[3] = pc.ToScreenCoordinates(Bounds);

            pc.Arc = 240 - 30;
            masterPoly[4] = pc.ToScreenCoordinates(Bounds);

            pc.Arc = 300 - 30;
            masterPoly[5] = pc.ToScreenCoordinates(Bounds);

            for (z = 0; z < 6; z++)
            {
                masterPoly[z].Y -= cor;
            }

            cor = 0f;

            for (float j = y1; j < y2; j += stepY)
            {
                if (alt)
                {
                    x1 = (elementSize) + (elementSize / 4f);
                }
                else
                {
                    x1 = (elementSize / 2f);
                }

                alt = !alt;

                for (float i = x1; i < x2; i += stepX)
                {

                    if (!PointInPolygon(masterPoly, i, j)) continue;

                    pc = PolarCoordinates.ToPolarCoordinates(i - pixelRadius, j - pixelRadius);

                    if (pc.Radius > pixelRadius)
                    {
                        pc.Radius = pixelRadius;
                    }
                    if (double.IsNaN(pc.Arc))
                    {
                        color = -1;
                    }
                    else
                    {
                        double arc = pc.Arc; // - rotation;
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

                    el.FillPoints = new PointF[6];
                    el.Center = new PointF(i, j);
                    el.Color = Color.FromArgb(color);
                    el.Polar = pc;
                    el.Shape = ColorWheelShapes.Hexagon;
                    el.Bounds = new RectangleF(i - (elementSize / 2f), j - (elementSize / 2f), (float)elementSize, (float)elementSize);
                    el.Center = el.FillPoints[0];

                    pc.Arc = -30;
                    pc.Radius = (double)elementSize / 2;

                    el.FillPoints[0] = pc.ToScreenCoordinates(el.Bounds);

                    if (cor == 0f)
                    {
                        cor = el.FillPoints[0].Y;
                        stepY -= cor;
                    }

                    pc.Arc = 60 - 30;
                    el.FillPoints[1] = pc.ToScreenCoordinates(el.Bounds);

                    pc.Arc = 120 - 30;
                    el.FillPoints[2] = pc.ToScreenCoordinates(el.Bounds);

                    pc.Arc = 180 - 30;
                    el.FillPoints[3] = pc.ToScreenCoordinates(el.Bounds);

                    pc.Arc = 240 - 30;
                    el.FillPoints[4] = pc.ToScreenCoordinates(el.Bounds);

                    pc.Arc = 300 - 30;
                    el.FillPoints[5] = pc.ToScreenCoordinates(el.Bounds);

                    for (z = 0; z < 6; z++)
                    {
                        el.FillPoints[z].Y -= cor;
                    }

                    Elements.Add(el);

                    br = new SolidBrush(el.Color);
                    g.FillPolygon(br, el.FillPoints);
                    br.Dispose();
                }

            }

            g.Dispose();
            Bitmap = bmp;

        }

        public ColorWheel(int pixelRadius, double value = 1d, double rotation = 0d, bool invert = false)
        {
            List<int> rawColors = new List<int>();

            int x1 = 0;
            int x2 = pixelRadius * 2;

            int y1 = 0;
            int y2 = pixelRadius * 2;

            double sx, sy;

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
                    sx = i - pixelRadius;
                    sy = j - pixelRadius;

                    pc = PolarCoordinates.ToPolarCoordinates(sx, sy);

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

                    el.FillPoints = new PointF[1] { new PointF(i, j) };
                    el.Color = Color.FromArgb(color);
                    el.Polar = pc;
                    el.Shape = ColorWheelShapes.Point;
                    el.Bounds = new RectangleF(i, j, 1, 1);
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
        public PointF Center;
        public RectangleF Bounds;
        public PointF[] FillPoints;
        public ColorWheelShapes Shape;
    }

}
