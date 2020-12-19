using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTools.MathTools;
using DataTools.Memory;
using DataTools.Win32;
using System.Drawing;
using System.Runtime.InteropServices;
using DataTools.Win32.Memory;

namespace DataTools.Desktop
{
    public static class BitmapTools
    {

        public const long BI_RGB = 0L;
        public const long BI_RLE8 = 1L;
        public const long BI_RLE4 = 2L;
        public const long BI_BITFIELDS = 3L;
        public const long BI_JPEG = 4L;
        public const long BI_PNG = 5L;


        [DllImport("gdi32", CharSet = CharSet.Unicode)]
        internal static extern IntPtr CreateDIBSection(IntPtr hdc, IntPtr pbmi, uint usage, ref IntPtr ppvBits, IntPtr hSection, int offset);

        /// <summary>
        /// Gray out an icon.
        /// </summary>
        /// <param name="icn">The input icon.</param>
        /// <returns>The grayed out icon.</returns>
        /// <remarks></remarks>
        public static Image GrayIcon(Icon icn)
        {
            var n = new Bitmap(icn.Width, icn.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var g = Graphics.FromImage(n);
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
        /// Create a Device Independent Bitmap from an icon.
        /// </summary>
        /// <param name="icn"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static IntPtr DIBFromIcon(Icon icn)
        {
            var bmp = IconToTransparentBitmap(icn);
            var _d = new IntPtr();

            return MakeDIBSection(bmp, ref _d);
        }

        public static System.Windows.Media.Imaging.BitmapSource MakeWPFImage(Icon img)
        {
            var _d = new IntPtr();
            return MakeWPFImage(img, ref _d);
        }

        public static System.Windows.Media.Imaging.BitmapSource MakeWPFImage(Bitmap img)
        {
            var _d = new IntPtr();
            return MakeWPFImage(img, ref _d);
        }

        /// <summary>
        /// Creates a WPF BitmapSource from a Bitmap.
        /// </summary>
        /// <param name="img">The <see cref="System.Drawing.Icon"/> object to convert.</param>
        /// <param name="bitPtr">Set this to zero.</param>
        /// <param name="dpiX">The X DPI to use to create the new image (default is 96.0)</param>
        /// <param name="dpiY">The Y DPI to use to create the new image (default is 96.0)</param>
        /// <param name="createOnApplicationThread"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static System.Windows.Media.Imaging.BitmapSource MakeWPFImage(Icon img, ref IntPtr bitPtr, double dpiX = 96.0d, double dpiY = 96.0d, bool createOnApplicationThread = true)
        {
            return MakeWPFImage(IconToTransparentBitmap(img), ref bitPtr, dpiX, dpiY, createOnApplicationThread);
        }

        /// <summary>
        /// Creates a WPF BitmapSource from a Bitmap.
        /// </summary>
        /// <param name="img">The <see cref="System.Drawing.Image"/> object to convert.</param>
        /// <param name="bitPtr">Set this to zero.</param>
        /// <param name="dpiX">The X DPI to use to create the new image (default is 96.0)</param>
        /// <param name="dpiY">The Y DPI to use to create the new image (default is 96.0)</param>
        /// <param name="createOnApplicationThread"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static System.Windows.Media.Imaging.BitmapSource MakeWPFImage(Bitmap img, ref IntPtr bitPtr, double dpiX = 96.0d, double dpiY = 96.0d, bool createOnApplicationThread = true)
        {
            if (img is null)
                return null;
            if (img.Width == 0)
                return null;
            int BytesPerRow = (int)((double)(img.Width * 32 + 31 & ~31) / 8d);
            int size = img.Height * BytesPerRow;
            var bm = new System.Drawing.Imaging.BitmapData();
            System.Windows.Media.Imaging.BitmapSource bmp = null;
            try
            {
                bm = img.LockBits(new Rectangle(0, 0, img.Width, img.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppPArgb, bm);
                if (createOnApplicationThread && System.Windows.Application.Current is object)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke(() => bmp = System.Windows.Media.Imaging.BitmapSource.Create(img.Width, img.Height, dpiX, dpiY, System.Windows.Media.PixelFormats.Bgra32, null, bm.Scan0, size, BytesPerRow));
                }
                else
                {
                    bmp = System.Windows.Media.Imaging.BitmapSource.Create(img.Width, img.Height, dpiX, dpiY, System.Windows.Media.PixelFormats.Bgra32, null, bm.Scan0, size, BytesPerRow);
                }
            }
            catch
            {
                if (bm is object)
                    img.UnlockBits(bm);
                //Interaction.MsgBox("Error: " + ex.Message);
                return null;
            }

            img.UnlockBits(bm);
            return bmp;
        }

        /// <summary>
        /// Creates a System.Drawing.Bitmap image from a WPF source.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Bitmap MakeBitmapFromWPF(System.Windows.Media.Imaging.BitmapSource source)
        {
            var mm = new MemPtr();
            Bitmap bmp = null;
            if (System.Windows.Application.Current is object)
            {
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    bmp = new Bitmap(source.PixelWidth, source.PixelHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    mm.Alloc(bmp.Width * bmp.Height * 4);
                    var bm = new System.Drawing.Imaging.BitmapData();
                    bm.Scan0 = mm.Handle;
                    bm.Stride = bmp.Width * 4;
                    bm = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite | System.Drawing.Imaging.ImageLockMode.UserInputBuffer, System.Drawing.Imaging.PixelFormat.Format32bppArgb, bm);
                    source.CopyPixels(System.Windows.Int32Rect.Empty, mm.Handle, (int)mm.Length, bmp.Width * 4);
                    bmp.UnlockBits(bm);
                    mm.Free();
                });
            }
            else
            {
                bmp = new Bitmap(source.PixelWidth, source.PixelHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                mm.Alloc(bmp.Width * bmp.Height * 4);
                var bm = new System.Drawing.Imaging.BitmapData();
                bm.Scan0 = mm.Handle;
                bm.Stride = bmp.Width * 4;
                bm = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite | System.Drawing.Imaging.ImageLockMode.UserInputBuffer, System.Drawing.Imaging.PixelFormat.Format32bppArgb, bm);
                source.CopyPixels(System.Windows.Int32Rect.Empty, mm.Handle, (int)mm.Length, bmp.Width * 4);
                bmp.UnlockBits(bm);
                mm.Free();
            }

            return bmp;
        }

        /// <summary>
        /// Create a writable device-independent bitmap from the specified image.
        /// </summary>
        /// <param name="img">Image to copy.</param>
        /// <param name="bitPtr">Optional variable to receive a pointer to the bitmap bits.</param>
        /// <returns>A new DIB handle that must be destroyed with DeleteObject().</returns>
        /// <remarks></remarks>
        public static IntPtr MakeDIBSection(Bitmap img, ref IntPtr bitPtr)
        {
            // Build header.
            // adapted from C++ code examples.

            short wBitsPerPixel = 32;
            int BytesPerRow = (int)((double)(img.Width * wBitsPerPixel + 31 & ~31L) / 8d);
            int size = img.Height * BytesPerRow;
            var bmpInfo = default(BITMAPINFO);
            var mm = new MemPtr();
            int bmpSizeOf = Marshal.SizeOf(bmpInfo);
            mm.ReAlloc(bmpSizeOf + size);
            var pbmih = default(BITMAPINFOHEADER);
            pbmih.biSize = Marshal.SizeOf(pbmih);
            pbmih.biWidth = img.Width;
            pbmih.biHeight = img.Height; // positive indicates bottom-up DIB
            pbmih.biPlanes = 1;
            pbmih.biBitCount = wBitsPerPixel;
            pbmih.biCompression = (int)BI_RGB;
            pbmih.biSizeImage = size;
            pbmih.biXPelsPerMeter = (int)(24.5d * 1000d); // pixels per meter! And these values MUST be correct if you want to pass a DIB to a native menu.
            pbmih.biYPelsPerMeter = (int)(24.5d * 1000d); // pixels per meter!
            pbmih.biClrUsed = 0;
            pbmih.biClrImportant = 0;
            var pPixels = IntPtr.Zero;
            int DIB_RGB_COLORS = 0;
            Marshal.StructureToPtr(pbmih, mm.Handle, false);
            var hPreviewBitmap = BitmapTools.CreateDIBSection(IntPtr.Zero, mm.Handle, (uint)DIB_RGB_COLORS, ref pPixels, IntPtr.Zero, 0);
            bitPtr = pPixels;
            var bm = new System.Drawing.Imaging.BitmapData();
            bm = img.LockBits(new Rectangle(0, 0, img.Width, img.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppPArgb, bm);
            var pCurrSource = bm.Scan0;

            // Our DIBsection is bottom-up...start at the bottom row...
            var pCurrDest = pPixels + (img.Width - 1) * BytesPerRow;
            // ... and work our way up
            int DestinationStride = -BytesPerRow;

            for (int curY = 0, ih = img.Height - 1; curY <= ih; curY++)
            {
                Native.MemCpy(pCurrSource, pCurrDest, BytesPerRow);
                pCurrSource = pCurrSource + bm.Stride;
                pCurrDest = pCurrDest + DestinationStride;
            }

            // Free up locked buffer.
            img.UnlockBits(bm);
            return hPreviewBitmap;
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
            var g = Graphics.FromImage(n);
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
            var g = Graphics.FromImage(n);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            g.Clear(Color.Transparent);
            g.DrawIcon(icn, 0, 0);
            g.Dispose();
            return n;
        }


    }
}
