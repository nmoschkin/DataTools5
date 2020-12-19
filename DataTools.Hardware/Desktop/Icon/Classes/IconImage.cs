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
// Licensed Under the MIT License   
// ************************************************* ''

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using DataTools.Win32;
using DataTools.Shell.Native;
using DataTools.Win32.Memory;

namespace DataTools.Desktop
{
    /// <summary>
    /// Represents an entire icon image file.
    /// </summary>
    /// <remarks></remarks>
    public class IconImage
    {
        private ICONDIR _dir;
        private List<IconImageEntry> _entries = new List<IconImageEntry>();
        private string _FileName;
        
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
            foreach (var e in _entries)
            {
                if (e.StandardIconType == StandardIconType)
                {
                    return e;
                }
            }

            return null;
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
            _dir = mm.ToStruct<ICONDIR>();
            int i;
            int c = _dir.nImages - 1;
            int f = Marshal.SizeOf<ICONDIRENTRY>();
            int e = Marshal.SizeOf<ICONDIR>();
            var optr = ptr;
            if (_dir.nImages <= 0 || _dir.wReserved != 0 || 0 == (int)(_dir.wIconType & IconImageType.IsValid))
            {
                return false;
            }

            _entries = new List<IconImageEntry>();
            mm = mm + e;
 
            for (i = 0; i < c; i++)
            {
                // load all images in sequence.
                _entries.Add(new IconImageEntry(mm.ToStruct<ICONDIRENTRY>(), optr));
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
            int g = Marshal.SizeOf<ICONDIRENTRY>();
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
            stream.Write((byte[])bl, 0, (int)bl.Length);
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
