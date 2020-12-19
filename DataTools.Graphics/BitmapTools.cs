using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTools.MathTools;
using DataTools.Standard.Memory;
using System.Drawing;
using System.Runtime.InteropServices;

namespace DataTools.Graphics
{
    public static class BitmapTools
    {

        public const long BI_RGB = 0L;
        public const long BI_RLE8 = 1L;
        public const long BI_RLE4 = 2L;
        public const long BI_BITFIELDS = 3L;
        public const long BI_JPEG = 4L;
        public const long BI_PNG = 5L;

        /// <summary>
        /// Gray out an icon.
        /// </summary>
        /// <param name="icn">The input icon.</param>
        /// <returns>The grayed out icon.</returns>
        /// <remarks></remarks>
        public static Image GrayIcon(Icon icn)
        {
            var n = new Bitmap(icn.Width, icn.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var g = System.Drawing.Graphics.FromImage(n);
            g.FillRectangle(Brushes.Transparent, new Rectangle(0, 0, n.Width, n.Height));
            g.DrawIcon(icn, 0, 0);
            g.Dispose();
            var bm = new System.Drawing.Imaging.BitmapData();
            var mm = new MemPtr(n.Width * n.Height * 4);
            bm.Stride = n.Width * 4;
            bm.Scan0 = mm;
            bm.PixelFormat = System.Drawing.Imaging.PixelFormat.Format32bppArgb;
            bm.Width = n.Width;
            bm.Height = n.Height;
            bm = n.LockBits(new Rectangle(0, 0, n.Width, n.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite | System.Drawing.Imaging.ImageLockMode.UserInputBuffer, System.Drawing.Imaging.PixelFormat.Format32bppArgb, bm);
            int i;
            int c;

            // Dim b() As Byte

            // ReDim b((bm.Stride * bm.Height) - 1)
            // MemCpy(b, bm.Scan0, bm.Stride * bm.Height)
            c = bm.Stride * bm.Height - 1;
            int stp = (int)(bm.Stride / (double)bm.Width);

            // For i = 3 To c Step stp
            // If b(i) > &H7F Then b(i) = &H7F
            // Next

            for (i = 3; stp >= 0 ? i <= c : i >= c; i += stp)
            {
                if (mm.ByteAt(i) > 0x7F)
                    mm.ByteAt(i) = 0x7F;
            }

            // MemCpy(bm.Scan0, b, bm.Stride * bm.Height)
            n.UnlockBits(bm);
            mm.Free();
            return n;
        }


        /// <summary>
        /// Highlight the specified icon with the specified color.
        /// </summary>
        /// <param name="icn">Input icon.</param>
        /// <param name="liteColor">Highlight base color.</param>
        /// <returns>A new highlighted Image object.</returns>
        /// <remarks></remarks>
        public static Image HiliteIcon(Icon icn, Color liteColor)
        {
            var n = new Bitmap(icn.Width, icn.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var g = System.Drawing.Graphics.FromImage(n);
            int lc = liteColor.ToArgb();
            g.FillRectangle(Brushes.Transparent, new Rectangle(0, 0, n.Width, n.Height));
            g.DrawIcon(icn, 0, 0);
            g.Dispose();
            var bm = new System.Drawing.Imaging.BitmapData();
            n.LockBits(new Rectangle(0, 0, n.Width, n.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb, bm);
            int[] b;
            int i;
            int c;
            b = new int[(bm.Width * bm.Height)];

            // take the unmanaged memory and make it something manageable and VB-like.

            Marshal.Copy(bm.Scan0, b, 0, bm.Stride * bm.Height);
            // NativeLib.Native.MemCpy(bm.Scan0, b, bm.Stride * bm.Height)

            c = b.Length;
            int stp = (int)(bm.Stride / (double)bm.Width);
            var hsv = new HSVDATA();
            var hsv2 = new HSVDATA();

            // convert the color to HSV.
            ColorMath.ColorToHSV(liteColor, ref hsv);

            for (i = 0; i < c; i++)
            {
                if (b[i] == 0)
                    continue;
                ColorMath.ColorToHSV(Color.FromArgb(b[i]), ref hsv2);

                hsv2.Hue = hsv.Hue;
                hsv2.Saturation = hsv.Saturation;
                hsv2.Value *= 1.1d;

                b[i] = ColorMath.HSVToColor(hsv2);
            }

            Marshal.Copy(b, 0, bm.Scan0, bm.Stride * bm.Height);
            n.UnlockBits(bm);
            return n;
        }

        /// <summary>
        /// Converts a 32 bit icon into a 32 bit Argb transparent bitmap.
        /// </summary>
        /// <param name="icn">Input icon.</param>
        /// <returns>A 32-bit Argb Bitmap object.</returns>
        /// <remarks></remarks>
        public static Bitmap IconToTransparentBitmap(Icon icn)
        {
            if (icn is null)
                return null;
            var n = new Bitmap(icn.Width, icn.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var g = System.Drawing.Graphics.FromImage(n);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            g.Clear(Color.Transparent);
            g.DrawIcon(icn, 0, 0);
            g.Dispose();
            return n;
        }


    }
}
