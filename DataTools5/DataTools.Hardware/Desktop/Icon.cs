// ************************************************* ''
// DataTools C# Native Utility Library For Windows - Interop
//
// Module: Icon File.
//         Icon image file format structure classes
//         Capable of using Windows Vista and greater .PNG icon images
//         Can create a complete icon file from scratch using images you add
//
// Icons are an old old format.  They have been adapted for modern use,
// and the reason that they endure is because of the ability to succintly
// store multiple image sizes in multiple formats, in a single file.
//
// But, because the 32-bit bitmap standard came around slightly afterward,
// certain acrobatic programming translations had to be made to get one from
// the other, and back again.
//
// Remember, back in the day, icon painting and design software was its own thing.
//
// Copyright (C) 2011-2017 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the Microsoft Public License   
// ************************************************* ''

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using DataTools.Memory;
using DataTools.Win32Api;
using DataTools.Shell.Native;

namespace DataTools.Desktop
{

    /// <summary>
    /// A short enumeration of standard icon type sizes coded for direct insertion into an ICONDIRENTRY structure.
    /// </summary>
    /// <remarks></remarks>
    public enum StandardIcons : ushort
    {
        Icon256 = 0,
        Icon128 = 0x8080,
        Icon64 = 0x4040,
        Icon48 = 0x3030,
        Icon32 = 0x2020,
        Icon16 = 0x1010
    }

    /// <summary>
    /// Returns the icon image type in a ICONDIRENTRY structure.
    /// </summary>
    /// <remarks></remarks>
    [Flags]
    public enum IconImageType : short
    {
        Invalid = 0,
        Icon = 1,
        Cursor = 2,
        IsValid = 3
    }

    static class IconFileStructures
    {

        // ICONDIR structure
        // Offset#	Size (in bytes)	Purpose
        // 0	2	Reserved. Must always be 0.
        // 2	2	Specifies image type: 1 for icon (.ICO) image, 2 for cursor (.CUR) image. Other values are invalid.
        // 4	2	Specifies number of images in the file.
        public struct ICONDIR
        {
            public short wReserved;
            public IconImageType wIconType;
            public short nImages;
        }

        // ICONDIRENTRY structure
        // Offset#	Size (in bytes)	Purpose
        // 0	1	Specifies image width in pixels. Can be any number between 0 and 255. Value 0 means image width is 256 pixels.
        // 1	1	Specifies image height in pixels. Can be any number between 0 and 255. Value 0 means image height is 256 pixels.
        // 2	1	Specifies number of colors in the color palette. Should be 0 if the image does not use a color palette.
        // 3	1	Reserved. Should be 0.[Notes 2]
        // 4	2	In ICO format: Specifies color planes. Should be 0 or 1.[Notes 3]
        // In CUR format: Specifies the horizontal coordinates of the hotspot in number of pixels from the left.
        // 6	2	In ICO format: Specifies bits per pixel. [Notes 4]
        // In CUR format: Specifies the vertical coordinates of the hotspot in number of pixels from the top.
        // 8	4	Specifies the size of the image's data in bytes
        // 12	4	Specifies the offset of BMP or PNG data from the beginning of the ICO/CUR file
        [StructLayout(LayoutKind.Explicit)]
        public struct ICONDIRENTRY
        {
            [FieldOffset(0)]
            public StandardIcons wIconType;
            [FieldOffset(0)]
            public byte cWidth;
            [FieldOffset(1)]
            public byte cHeight;
            [FieldOffset(2)]
            public byte cColors;
            [FieldOffset(3)]
            public byte cReserved;
            [FieldOffset(4)]
            public short wColorPlanes;
            [FieldOffset(4)]
            public short wHotspotX;
            [FieldOffset(6)]
            public short wBitsPixel;
            [FieldOffset(6)]
            public short wHotspotY;
            [FieldOffset(8)]
            public int dwImageSize;
            [FieldOffset(12)]
            public int dwOffset;
        }
    }

    /// <summary>
    /// Represents a single icon image.
    /// </summary>
    /// <remarks></remarks>
    public class IconImageEntry : IDisposable
    {
        internal IconFileStructures.ICONDIRENTRY _entry;
        internal byte[] _image;
        internal IntPtr _hIcon = IntPtr.Zero;

        internal IconImageEntry()
        {
        }

        /// <summary>
        /// Gets the raw ICONDIRENTRY structure.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        internal IconFileStructures.ICONDIRENTRY EntryInfo
        {
            get
            {
                return _entry;
            }
        }

        /// <summary>
        /// Create a new image from the pointer.
        /// </summary>
        /// <param name="ptr">Pointer to the start of the ICONDIRENTRY structure.</param>
        /// <remarks></remarks>
        internal IconImageEntry(IntPtr ptr)
        {
            MemPtr mm = ptr;
            _entry = mm.ToStruct<IconFileStructures.ICONDIRENTRY>();
            ptr = ptr + _entry.dwOffset;
            if (_entry.wBitsPixel < 24)
            {
                // Throw New InvalidDataException("Reading low-bit icons is not supported")
            }

            _image = new byte[_entry.dwImageSize];
            Marshal.Copy(ptr, _image, 0, _entry.dwImageSize);

            // MemCpy(_image, ptr, _entry.dwImageSize)

        }

        /// <summary>
        /// Extract an icon from an entry and a bits pointer.
        /// </summary>
        /// <param name="entry">Icon entry structure.</param>
        /// <param name="ptr">Pointer to the bitmap.</param>
        /// <remarks></remarks>
        internal IconImageEntry(IconFileStructures.ICONDIRENTRY entry, IntPtr ptr)
        {
            _entry = entry;
            if (_entry.wBitsPixel < 24)
            {
                // Throw New InvalidDataException("Reading low-bit icons is not supported")
            }

            ptr = ptr + _entry.dwOffset;
            _image = new byte[_entry.dwImageSize];
            Marshal.Copy(ptr, _image, 0, _entry.dwImageSize);

            // MemCpy(_image, ptr, _entry.dwImageSize)

        }

        /// <summary>
        /// Returns the icon type.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public StandardIcons StandardIconType
        {
            get
            {
                return _entry.wIconType;
            }
        }

        /// <summary>
        /// Converts this raw icon source into a managed System.Drawing.Icon image.
        /// </summary>
        /// <returns>A new Icon object.</returns>
        /// <remarks></remarks>
        public Icon ToIcon()
        {
            Icon ToIconRet = default;
            if (IsPngFormat)
            {
                Bitmap bmp = (Bitmap)ToImage();
                var bmi = new Bitmap(bmp.Width, bmp.Height, System.Drawing.Imaging.PixelFormat.Format1bppIndexed);
                var lpicon = default(User32.ICONINFO);
                int i;
                var bm = bmi.LockBits(new Rectangle(0, 0, bmi.Width, bmi.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format1bppIndexed);
                MemPtr mm = bm.Scan0;
                int z = (int)(Math.Max(bmp.Width, 32) * bmp.Height / 8d);
                var loopTo = z - 1;
                for (i = 0; i <= loopTo; i++)
                    mm.ByteAt(i) = 255;
                bmi.UnlockBits(bm);
                lpicon.hbmColor = bmp.GetHbitmap();
                lpicon.hbmMask = bmi.GetHbitmap();
                lpicon.fIcon = 1;
                var hIcon = User32.CreateIconIndirect(ref lpicon);
                if (hIcon != IntPtr.Zero)
                {
                    ToIconRet = (Icon)Icon.FromHandle(hIcon).Clone();
                    User32.DestroyIcon(hIcon);
                }
                else
                {
                    ToIconRet = null;
                }

                NativeShell.DeleteObject(lpicon.hbmMask);
                NativeShell.DeleteObject(lpicon.hbmColor);
            }
            else
            {
                ToIconRet = _constructIcon();
            }

            return ToIconRet;
        }

        /// <summary>
        /// Retrieves the width of the icon.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int Width
        {
            get
            {
                if (_entry.cWidth == 0)
                    return 256;
                else
                    return _entry.cWidth;
            }
        }

        /// <summary>
        /// Retrieves the height of the icon.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int Height
        {
            get
            {
                if (_entry.cHeight == 0)
                    return 256;
                else
                    return _entry.cHeight;
            }
        }

        /// <summary>
        /// Returns True if the icon image data is in compressed PNG format.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsPngFormat
        {
            get
            {
                bool IsPngFormatRet = default;
                if (_image is null)
                    return false;
                SafePtr mm = (SafePtr)_image;
                int q = mm.IntAt(0L);

                // The PNG moniker
                IsPngFormatRet = q == 0x474E5089;
                return IsPngFormatRet;
            }
        }

        /// <summary>
        /// Returns a new System.Drawing.Image from this raw icon.
        /// </summary>
        /// <returns>A new Image object.</returns>
        /// <remarks></remarks>
        public Image ToImage()
        {
            Image ToImageRet = default;
            if (IsPngFormat)
            {
                var s = new MemoryStream(_image);
                ToImageRet = Image.FromStream(s);
                s.Close();
            }
            else
            {
                ToImageRet = Resources.IconToTransparentBitmap(_constructIcon());
            }

            return ToImageRet;
        }

        /// <summary>
        /// Create a bitmap structure with bits from this raw icon image.
        /// </summary>
        /// <returns>An array of bytes that represent data to create a bitmap.</returns>
        /// <remarks></remarks>
        private byte[] _makeBitmap()
        {
            byte[] _makeBitmapRet = default;
            if (!IsPngFormat)
            {
                _makeBitmapRet = _image;
                return _makeBitmapRet;
            }

            IntPtr bmp = default;
            var hbmp = Resources.MakeDIBSection((Bitmap)ToImage(), ref bmp);
            var mm = new SafePtr();
            var bm = new User32.BITMAPINFOHEADER();
            int maskSize;
            int w = _entry.cWidth;
            int h = _entry.cHeight;
            if (w == 0)
                w = 256;
            if (h == 0)
                h = 256;
            bm.biSize = 40;
            bm.biWidth = w;
            bm.biHeight = h * 2;
            bm.biPlanes = 1;
            bm.biBitCount = 32;
            bm.biSizeImage = w * h * 4;
            maskSize = (int)(Math.Max(w, 32) * h / 8d);
            mm.Alloc(bm.biSizeImage + 40 + maskSize);
            var ptr1 = mm.DangerousGetHandle() + 40;
            var ptr2 = mm.DangerousGetHandle() + 40 + bm.biSizeImage;
            Marshal.StructureToPtr(bm, mm.DangerousGetHandle(), false);
            DataTools.Memory.NativeLib.Native.MemCpy(bmp, ptr1, bm.biSizeImage);
            bm = mm.ToStruct<User32.BITMAPINFOHEADER>();
            _setMask(ptr1, ptr2, w, h);
            _entry.dwImageSize = (int)mm.Size;
            _makeBitmapRet = (byte[])mm;
            mm.Free();
            NativeShell.DeleteObject(hbmp);
            return _makeBitmapRet;
        }

        /// <summary>
        /// Construct an icon from the raw image data.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        private Icon _constructIcon()
        {
            Icon _constructIconRet = default;
            if (_hIcon != IntPtr.Zero)
            {
                User32.DestroyIcon(_hIcon);
                _hIcon = IntPtr.Zero;
            }

            MemPtr mm = (MemPtr)_image;
            var bmp = mm.ToStruct<User32.BITMAPINFOHEADER>();

            IntPtr hBmp;
            IntPtr ptr;
            IntPtr ppBits = new IntPtr();

            var lpicon = default(User32.ICONINFO);

            IntPtr hicon;
            IntPtr hBmpMask = new IntPtr();

            bool hasMask;
            if (bmp.biHeight == bmp.biWidth * 2)
            {
                hasMask = true;
                bmp.biHeight = (int)(bmp.biHeight / 2d);
            }
            else
            {
                hasMask = false;
            }

            bmp.biSizeImage = (int)(bmp.biWidth * bmp.biHeight * (bmp.biBitCount / 8d));

            bmp.biXPelsPerMeter = (int)(24.5d * 1000d);
            bmp.biYPelsPerMeter = (int)(24.5d * 1000d);

            bmp.biClrUsed = 0;
            bmp.biClrImportant = 0;
            bmp.biPlanes = 1;
            
            Marshal.StructureToPtr(bmp, mm.Handle, false);
            
            ptr = mm.Handle + bmp.biSize;
            
            if (bmp.biSize != 40)
                return null;
            
            hBmp = User32.CreateDIBSection(IntPtr.Zero, mm.Handle, 0U, ref ppBits, IntPtr.Zero, 0);
            
            DataTools.Memory.NativeLib.Native.MemCpy(ptr, ppBits, bmp.biSizeImage);
            
            if (hasMask)
            {
                ptr = ptr + bmp.biSizeImage;
                bmp.biBitCount = 1;
                bmp.biSizeImage = 0;
                Marshal.StructureToPtr(bmp, mm.Handle, false);
                hBmpMask = User32.CreateDIBSection(IntPtr.Zero, mm.Handle, 0U, ref ppBits, IntPtr.Zero, 0);
                DataTools.Memory.NativeLib.Native.MemCpy(ptr, ppBits, (long)(Math.Max(bmp.biWidth, 32) * bmp.biHeight / 8d));
            }

            lpicon.fIcon = 1;
            lpicon.hbmColor = hBmp;
            lpicon.hbmMask = hBmpMask;
            hicon = User32.CreateIconIndirect(ref lpicon);
            NativeShell.DeleteObject(hBmp);
            if (hasMask)
                NativeShell.DeleteObject(hBmpMask);
            _constructIconRet = Icon.FromHandle(hicon);
            _hIcon = hicon;
            mm.Free();
            return _constructIconRet;
        }

        /// <summary>
        /// Apply transparency mask bits to the output image (for converting to a bitmap)
        /// </summary>
        /// <param name="hBits">A pointer to the memory address of the bitmap bits.</param>
        /// <param name="hMask">A pointer to the memory address of the mask bits.</param>
        /// <param name="Width">The width of the image.</param>
        /// <param name="Height">The height of the image.</param>
        /// <remarks></remarks>
        private void _applyMask(MemPtr hBits, MemPtr hMask, int Width, int Height)
        {
            // Masks in icon images are bitstreams wherein a single bit represents a 1 or 0 transparency
            // for an entire pixel on the screen.  In order to convert an icon into a 32 bit images,
            // we need to access each individual bit, and apply the NOT of the value to the byte-length alpha mask
            // of the bitmap.

            int x;
            int y;
            int shift;
            int bit;
            int shift2;
            int mask;

            // in transparency masks for icons, the minimum stride is 32 pixels/bits, no matter the actual size of the image.
            int boundary = Math.Max(32, Width);

            // walk every pixel of the image.
            var loopTo = Height - 1;
            for (y = 0; y <= loopTo; y++)
            {
                var loopTo1 = Width - 1;
                for (x = 0; x <= loopTo1; x++)
                {

                    // the first shift is our position in the bitmap output.
                    // 4 bytes is 32 bits ... then we add 3 to get directly 
                    // to the alpha mask.
                    shift = 4 * (x + y * Width) + 3;

                    // we find the exact bit-wise position by modulus with 8 (the length of a byte, in bits)
                    bit = 7 - x % 8;

                    // the second shift is the position in the bitmask, byte-wise.  We subtract 1 from y before subtracting it from the
                    // height because the first scan line is 0.
                    shift2 = (int)((x + (Height - y - 1) * boundary) / 8d);

                    // we get a number that is either 1 or 0 from the mask by accessing the exact byte, and then
                    // accessing the exact bit by left-shifting its value into the 1 position.
                    mask = 1 & hMask.ByteAt(shift2) >> bit;

                    // we do a quick logical AND via multiplication with the inverse of the mask.
                    // we do this because alpha channel 0 is transparent, but transparent mask 1 is also transparent.
                    hBits.ByteAt(shift) *= (byte)(1 - mask);
                }
            }
        }

        /// <summary>
        /// Create a transparency mask from the transparent bits in an image.
        /// </summary>
        /// <param name="hBits">A pointer to the memory address of the bitmap bits.</param>
        /// <param name="hMask">A pointer to the memory address of the mask bits.</param>
        /// <param name="Width">The width of the image.</param>
        /// <param name="Height">The height of the image.</param>
        /// <remarks></remarks>
        private void _setMask(MemPtr hBits, MemPtr hMask, int Width, int Height)
        {
            // this never changes
            int numBits = 32;
            int x;
            int y;
            byte bit = 0;
            byte mask = 0;
            int d;
            int e;
            int f;
            double move = numBits / 8d;
            int stride = (int)(Width * (numBits / 8d));
            int msize = (int)(Math.Max(32, Width) * Height / 8d);
            int isize = (int)(Width * Height * (numBits / 8d));
            byte[] bb;
            byte[] imgb;
            imgb = new byte[isize];
            bb = new byte[msize];
            Marshal.Copy(hBits.Handle, imgb, 0, isize);
            var loopTo = Height - 1;
            for (y = 0; y <= loopTo; y++)
            {
                d = y * stride;
                var loopTo1 = Width - 1;
                for (x = 0; x <= loopTo1; x++)
                {
                    f = (int)(d + x * move);
                    e = (int)Math.Floor(x / 8d);
                    bit = (byte)(7 - x % 8);
                    if (imgb[f + 3] == 0)
                    {
                        bb[e] = (byte)(bb[e] | 1 << bit);
                    }
                }
            }

            // MemCpy(hMask.Handle, bb, msize)

            hMask.FromByteArray(bb, 0L);
            hBits = IntPtr.Zero;
            hMask = IntPtr.Zero;
        }

        /// <summary>
        /// Initialize a new raw icon image from a standard image with the specified size as either a bitmap or PNG icon.
        /// </summary>
        /// <param name="Image">Image from which to construct the icon.</param>
        /// <param name="size">The standard size of the new icon.</param>
        /// <param name="asBmp">Whether to create a bitmap icon (if false, a PNG icon is created).</param>
        /// <remarks></remarks>
        public IconImageEntry(Image Image, StandardIcons size, bool asBmp = false)
        {
            int sz = (int)size != 0 ? (int)(size) & 0xFF : 256;
            Bitmap im = (Bitmap)Image;
            if (Image.Width != sz | Image.Height != sz)
            {
                im = new Bitmap(sz, sz, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                var g = Graphics.FromImage(im);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                g.DrawImage(Image, new Rectangle(0, 0, sz, sz));
                g.Dispose();
            }

            var s = new MemoryStream();
            im.Save(s, System.Drawing.Imaging.ImageFormat.Png);
            _entry.wIconType = size;
            _entry.wBitsPixel = 32;
            _entry.wColorPlanes = 1;
            _image = new byte[(int)(s.Length - 1L) + 1];
            _image = s.ToArray();
            if (asBmp)
            {
                _image = _makeBitmap();
            }

            _entry.dwImageSize = _image.Length;
            s.Close();
        }

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        private bool disposedValue; // To detect redundant calls

        // IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (_hIcon != IntPtr.Zero)
                {
                    User32.DestroyIcon(_hIcon);
                }
            }

            disposedValue = true;
        }

        ~IconImageEntry()
        {
            Dispose(false);
        }

        // This code added by Visual Basic to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    }

    /// <summary>
    /// Represents an entire icon image file.
    /// </summary>
    /// <remarks></remarks>
    public class IconImage
    {
        private IconFileStructures.ICONDIR _dir;
        private List<IconImageEntry> _entries = new List<IconImageEntry>();
        private string _FileName;
        private bool _ShowSaveDialog = false;

        /// <summary>
        /// Gets or sets the filename of the icon file.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string FileName
        {
            get
            {
                string FileNameRet = default;
                FileNameRet = _FileName;
                return FileNameRet;
            }

            set
            {
                _FileName = value;
                LoadIcon();
            }
        }

        /// <summary>
        /// Retrieves a list of icon images stored in the icon file.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public List<IconImageEntry> Entries
        {
            get
            {
                return _entries;
            }
        }

        /// <summary>
        /// Finds an icon by standard size.
        /// </summary>
        /// <param name="StandardIconType"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public IconImageEntry FindIcon(StandardIcons StandardIconType)
        {
            IconImageEntry FindIconRet = default;
            foreach (var e in _entries)
            {
                if (e.StandardIconType == StandardIconType)
                {
                    FindIconRet = e;
                    return FindIconRet;
                }
            }

            FindIconRet = null;
            return FindIconRet;
        }

        /// <summary>
        /// Loads the icon from the file specified in the Filename property.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool LoadIcon()
        {
            return LoadIcon(_FileName);
        }

        /// <summary>
        /// Loads an icon from a stream.
        /// </summary>
        /// <param name="stream">The stream to load.</param>
        /// <param name="closeStream">Whether or not to close the stream when finished.</param>
        /// <returns>True if successful.</returns>
        /// <remarks></remarks>
        public bool LoadIcon(Stream stream, bool closeStream = true)
        {
            bool LoadIconRet = default;
            LoadIconRet = _internalLoadFromStream(stream, closeStream);
            return LoadIconRet;
        }

        /// <summary>
        /// Loads an icon from a memory pointer.
        /// </summary>
        /// <param name="ptr">The memory pointer to load.</param>
        /// <returns>True if successful.</returns>
        /// <remarks></remarks>
        public bool LoadIcon(IntPtr ptr)
        {
            bool LoadIconRet = default;
            LoadIconRet = _internalLoadFromPtr(ptr);
            return LoadIconRet;
        }

        /// <summary>
        /// Loads an icon from a byte array.
        /// </summary>
        /// <param name="bytes">Buffer to load.</param>
        /// <returns>True if successful.</returns>
        /// <remarks></remarks>
        public bool LoadIcon(byte[] bytes)
        {
            bool LoadIconRet = default;
            LoadIconRet = _internalLoadFromBytes(bytes);
            return LoadIconRet;
        }

        /// <summary>
        /// Loads an icon from the specified file.
        /// </summary>
        /// <param name="fileName">Filename of the icon to load.</param>
        /// <returns>True if successful.</returns>
        /// <remarks></remarks>
        public bool LoadIcon(string fileName)
        {
            bool LoadIconRet = default;
            _FileName = fileName;
            LoadIconRet = _internalLoadFromFile(fileName);
            return LoadIconRet;
        }

        /// <summary>
        /// Loads an icon from a file using an OpenFileDialog.
        /// </summary>
        /// <param name="fileName">Sets or receives the selected file.</param>
        /// <param name="prompt">Specify whether or not to raise the OpenFileDialog.</param>
        /// <returns>True if successful.</returns>
        /// <remarks></remarks>
        public bool LoadIcon(ref string fileName, bool prompt)
        {
            bool LoadIconRet = default;
            if (prompt)
            {
                var ofd = new System.Windows.Forms.OpenFileDialog();
                ofd.Filter = "Icon Files (*.ico)|*.ico|Cursor Files (*.cur)|*.cur|All Files (*.*)|*.*";
                ofd.Title = "Open Icon";
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    LoadIconRet = LoadIcon(ofd.FileName);
                }
                else
                {
                    LoadIconRet = false;
                }
            }
            else
            {
                LoadIconRet = LoadIcon(fileName);
            }

            return LoadIconRet;
        }

        /// <summary>
        /// Save the icon to the filename specified in the Filename property.
        /// </summary>
        /// <returns>True if successful.</returns>
        /// <remarks></remarks>
        public bool SaveIcon()
        {
            bool SaveIconRet = default;
            SaveIconRet = SaveIcon(_FileName);
            return SaveIconRet;
        }

        /// <summary>
        /// Saves the icon to the specified file.
        /// </summary>
        /// <param name="fileName">The file to save.</param>
        /// <returns>True if successful.</returns>
        /// <remarks></remarks>
        public bool SaveIcon(string fileName)
        {
            bool SaveIconRet = default;
            SaveIconRet = _internalSaveToFile(fileName);
            return SaveIconRet;
        }

        /// <summary>
        /// Saves an icon to the specified stream.
        /// </summary>
        /// <param name="stream">The stream to save.</param>
        /// <returns>True if successful.</returns>
        /// <remarks></remarks>
        public bool SaveIcon(Stream stream)
        {
            bool SaveIconRet = default;
            SaveIconRet = _internalSaveToStream(stream);
            return SaveIconRet;
        }

        /// <summary>
        /// Saves an icon to the specified file with a SaveFileDialog.
        /// </summary>
        /// <param name="fileName">Sets or receives the selected file.</param>
        /// <param name="prompt">True to raise the SaveFileDialog.</param>
        /// <returns>True if successful.</returns>
        /// <remarks></remarks>
        public bool SaveIcon(ref string fileName, bool prompt)
        {
            bool SaveIconRet = default;
            if (prompt)
            {
                var sfd = new System.Windows.Forms.SaveFileDialog();
                sfd.Filter = "Icon Files (*.ico)|*.ico|Cursor Files (*.cur)|*.cur";
                sfd.Title = "Save Icon";
                if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    fileName = sfd.FileName;
                    SaveIconRet = SaveIcon(fileName);
                }
                else
                {
                    SaveIconRet = false;
                }
            }
            else
            {
                SaveIconRet = SaveIcon(fileName);
            }

            return SaveIconRet;
        }

        /// <summary>
        /// Internal load icon.
        /// </summary>
        /// <param name="ptr">The pointer to load.</param>
        /// <returns>True if successful.</returns>
        /// <remarks></remarks>
        private bool _internalLoadFromPtr(IntPtr ptr)
        {
            bool _internalLoadFromPtrRet = default;

            // get the icon file header directory.
            MemPtr mm = ptr;
            _dir = mm.ToStruct<IconFileStructures.ICONDIR>();
            int i;
            int c = _dir.nImages - 1;
            int f = Marshal.SizeOf<IconFileStructures.ICONDIRENTRY>();
            int e = Marshal.SizeOf<IconFileStructures.ICONDIR>();
            var optr = ptr;
            if (_dir.nImages <= 0 || _dir.wReserved != 0 || 0 == (int)(_dir.wIconType & IconImageType.IsValid))
            {
                return false;
            }

            _entries = new List<IconImageEntry>();
            mm = mm + e;
            var loopTo = c;
            for (i = 0; i <= loopTo; i++)
            {
                // load all images in sequence.
                _entries.Add(new IconImageEntry(mm.ToStruct<IconFileStructures.ICONDIRENTRY>(), optr));
                ptr = ptr + f;
            }

            _internalLoadFromPtrRet = true;
            return _internalLoadFromPtrRet;
        }

        /// <summary>
        /// Internal load from bytes.
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        private bool _internalLoadFromBytes(byte[] bytes)
        {
            bool _internalLoadFromBytesRet = default;
            SafePtr mm = (SafePtr)bytes;
            _internalLoadFromBytesRet = _internalLoadFromPtr(mm);
            mm.Dispose();
            return _internalLoadFromBytesRet;
        }

        /// <summary>
        /// Internal load from stream.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="closeStream"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        private bool _internalLoadFromStream(Stream stream, bool closeStream)
        {
            bool _internalLoadFromStreamRet = default;
            byte[] b;
            b = new byte[(int)(stream.Length - 1L) + 1];
            stream.Seek(0L, SeekOrigin.Begin);
            stream.Read(b, 0, (int)stream.Length);
            if (closeStream)
                stream.Close();
            _internalLoadFromStreamRet = _internalLoadFromBytes(b);
            return _internalLoadFromStreamRet;
        }

        /// <summary>
        /// Internal save by file.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        private bool _internalLoadFromFile(string fileName)
        {
            bool _internalLoadFromFileRet = default;
            var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read, 10240);
            _internalLoadFromFileRet = _internalLoadFromStream(fs, true);
            return _internalLoadFromFileRet;
        }

        /// <summary>
        /// Internally saves the icon to the specified stream.
        /// </summary>
        /// <param name="stream">Stream to save.</param>
        /// <returns>True if successful.</returns>
        /// <remarks></remarks>
        private bool _internalSaveToStream(Stream stream)
        {
            bool _internalSaveToStreamRet = default;
            var bl = new SafePtr();
            int f = Marshal.SizeOf(_dir);
            int g = Marshal.SizeOf<IconFileStructures.ICONDIRENTRY>();
            _dir.nImages = (short)_entries.Count;
            _dir.wIconType = IconImageType.Icon;
            _dir.wReserved = 0;

            // get the index to the first image's image data
            int offset = f + g * _dir.nImages;
            bl = bl + FileTools.StructToBytes(_dir);
            foreach (var e in _entries)
            {
                e._entry.dwOffset = offset;
                bl = bl + FileTools.StructToBytes(e._entry);
                offset += e._entry.dwImageSize;
            }

            foreach (var e in _entries)
                bl = bl + e._image;

            // write the icon file
            stream.Write((byte[])bl, 0, (int)bl.Size);
            stream.Close();
            bl.Dispose();
            _internalSaveToStreamRet = true;
            return _internalSaveToStreamRet;
        }

        private bool _internalSaveToFile(string fileName)
        {
            bool _internalSaveToFileRet = default;
            var fs = new FileStream(fileName, FileMode.CreateNew, FileAccess.Write, FileShare.None, 10240);
            _internalSaveToFileRet = _internalSaveToStream(fs);
            return _internalSaveToFileRet;
        }

        public IconImage()
        {
        }

        /// <summary>
        /// Create a new instance of this object from a stream
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="closeStream"></param>
        /// <remarks></remarks>
        public IconImage(Stream stream, bool closeStream = true)
        {
            _internalLoadFromStream(stream, closeStream);
        }

        /// <summary>
        /// Create a new instance of this object from a memory pointer
        /// </summary>
        /// <param name="ptr"></param>
        /// <remarks></remarks>
        public IconImage(IntPtr ptr)
        {
            _internalLoadFromPtr(ptr);
        }

        /// <summary>
        /// Create a new instance of this object from the byte array
        /// </summary>
        /// <param name="bytes"></param>
        /// <remarks></remarks>
        public IconImage(byte[] bytes)
        {
            _internalLoadFromBytes(bytes);
        }

        /// <summary>
        /// Create a new instance of this object from the specified file
        /// </summary>
        /// <param name="fileName"></param>
        /// <remarks></remarks>
        public IconImage(string fileName)
        {
            if (_internalLoadFromFile(fileName))
            {
                _FileName = fileName;
            }
        }
    }
}