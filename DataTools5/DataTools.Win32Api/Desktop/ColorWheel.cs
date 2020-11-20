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

    public enum ColorWheelShapes
    {
        Point = 1,
        Hexagon = 2
    }

    public class ColorWheel
    {
        public List<ColorWheelElement> Elements { get; set; } = new List<ColorWheelElement>();
        public Rectangle Bounds { get; set; }

        private byte[] imageBytes;
        
        public byte[] ImageBytes
        {
            get => imageBytes;
            set
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

        public ColorWheel(int pixelRadius)
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

            for (int i = x1; i < x2; i++)
            {
                for (int j = y1; j < y2; j++)
                {
                    pc = PolarCoordinates.ToPolarCoordinates(i - pixelRadius, j - pixelRadius);
                    if (pc.Radius > pixelRadius)
                    {
                        rawColors.Add(0);
                        continue;
                    }

                    hsv = new HSVDATA()
                    {
                        Hue = pc.Arc,
                        Saturation = pc.Radius,
                        Value = 1
                    };

                    color = ColorMath.HSVToColorRaw(hsv);
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
