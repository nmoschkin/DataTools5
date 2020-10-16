using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;
using DataTools.Memory;
using DataTools.Win32Api;
using DataTools.Shell.Native;

namespace DataTools.Desktop
{

    /// <summary>
    /// Provides a file-system-locked object to represent a file.
    /// </summary>
    public class FileObject : ISimpleShellItem
    {
        private IShellItem _SysInterface;
        private string _DisplayName;
        private string _Filename;
        private System.Drawing.Icon _Icon;
        private BitmapSource _IconImage;
        private StandardIcons _IconSize = StandardIcons.Icon48;
        private bool _IsSpecial;
        private ISimpleShellItem _Parent;
        private SystemFileType _Type;

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Advanced initialization of FileObject.  Use this for items in special folders.
        /// </summary>
        /// <param name="parsingName">The shell parsing name of the file.</param>
        /// <param name="isSpecial">Is the file known to be special?</param>
        /// <param name="initialize">True to get file info and load icons.</param>
        /// <param name="iconSize">Default icon size.  This can be changed with the <see cref="IconSize"/> property.</param>
        public FileObject(string parsingName, bool isSpecial, bool initialize, StandardIcons iconSize = StandardIcons.Icon48)
        {
            _IsSpecial = isSpecial;
            _Filename = parsingName;
            try
            {
                if (_IsSpecial)
                {
                    // let's see if we can parse it.
                    IShellItem shitem = null;
                    var mm = new MemPtr();
                    var argriid = Guid.Parse(ShellIIDGuid.IShellItem);
                    var res = NativeShell.SHCreateItemFromParsingName(parsingName, IntPtr.Zero, ref argriid, ref shitem);
                    string fp = null;
                    if (res == HResult.Ok)
                    {

                        shitem.GetDisplayName(ShellItemDesignNameOptions.DesktopAbsoluteParsing, out mm.handle);
                        fp = (string)mm;

                        ParsingName = fp;
                        CanonicalName = fp;

                        mm.CoTaskMemFree();

                        shitem.GetDisplayName(ShellItemDesignNameOptions.Normal, out mm.handle);

                        DisplayName = (string)mm;

                        mm.CoTaskMemFree();

                        _IsSpecial = true;

                        if (initialize)
                            Refresh();

                        _SysInterface = shitem;

                        return;
                    }

                    HResult localSHCreateItemFromParsingName() { var argriid = Guid.Parse(ShellIIDGuid.IShellItem); var ret = NativeShell.SHCreateItemFromParsingName("shell:" + (fp ?? parsingName), IntPtr.Zero, ref argriid, ref shitem); return ret; }

                    res = localSHCreateItemFromParsingName();
                    if (res == HResult.Ok)
                    {

                        shitem.GetDisplayName(ShellItemDesignNameOptions.DesktopAbsoluteParsing, out mm.handle);
                        CanonicalName = (string)mm;

                        if (ParsingName is null)
                            ParsingName = (string)mm;

                        _Filename = ParsingName;
                        mm.CoTaskMemFree();

                        
                        shitem.GetDisplayName(ShellItemDesignNameOptions.Normal, out mm.handle);

                        DisplayName = (string)mm;

                        mm.CoTaskMemFree();
                    }

                    _SysInterface = shitem;
                    shitem = null;
                    if (!string.IsNullOrEmpty(DisplayName) && !string.IsNullOrEmpty(ParsingName))
                    {
                        _IsSpecial = true;
                        if (initialize)
                            Refresh(_IconSize);
                        return;
                    }
                }

                if (File.Exists(parsingName) == false)
                {
                    if (!_IsSpecial)
                        throw new FileNotFoundException("File Not Found: " + parsingName);
                }
                else if (initialize)
                    Refresh(_IconSize);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Create a new FileObject from the given filename.
        /// If the file does not exist, an exception will be thrown.
        /// </summary>
        /// <param name="filename"></param>
        public FileObject(string filename, bool initialize = true) : this(filename, false, initialize)
        {
        }

        /// <summary>
        /// Create a blank file object.
        /// </summary>
        internal FileObject()
        {
        }

        /// <summary>
        /// Gets or sets the file attributes.
        /// </summary>
        /// <returns></returns>
        public FileAttributes Attributes
        {
            get
            {
                return FileTools.GetFileAttributes(_Filename);
            }

            set
            {
                FileTools.SetFileAttributes(_Filename, value);
            }
        }

        /// <summary>
        /// Gets the canonical name of a special file
        /// </summary>
        /// <returns></returns>
        public string CanonicalName { get; private set; }

        /// <summary>
        /// Get or set the creation time of the file.
        /// </summary>
        /// <returns></returns>
        public DateTime CreationTime
        {
            get
            {
                var c = default(DateTime);
                var a = default(DateTime);
                var m = default(DateTime);
                FileTools.GetFileTime(_Filename, ref c, ref a, ref m);
                return c;
            }

            set
            {
                var c = default(DateTime);
                var a = default(DateTime);
                var m = default(DateTime);
                FileTools.GetFileTime(_Filename, ref c, ref a, ref m);
                FileTools.SetFileTime(_Filename, value, a, m);
            }
        }

        /// <summary>
        /// Get the containing directory of the file.
        /// </summary>
        /// <returns></returns>
        public string Directory
        {
            get
            {
                return Path.GetDirectoryName(_Filename);
            }

            set
            {
                if (!Move(value))
                {
                    throw new AccessViolationException("Unable to move file.");
                }
            }
        }

        /// <summary>
        /// Gets the display name of the file
        /// </summary>
        /// <returns></returns>
        public string DisplayName
        {
            get
            {
                return _DisplayName;
            }

            set
            {
                _DisplayName = value;
            }
        }

        /// <summary>
        /// Get the full path of the file.
        /// </summary>
        /// <returns></returns>
        public string Filename
        {
            get
            {
                return _Filename;
            }

            internal set
            {
                if (_Filename is object)
                {
                    if (!FileTools.MoveFile(_Filename, value))
                    {
                        throw new AccessViolationException("Unable to rename/move file.");
                    }
                }
                else if (!File.Exists(value))
                {
                    throw new FileNotFoundException("File Not Found: " + Filename);
                }

                _Filename = value;
                Refresh();
            }
        }

        /// <summary>
        /// Returns the file type description
        /// </summary>
        /// <returns></returns>
        public string FileType
        {
            get
            {
                if (_Type is null)
                    return "Unknown";
                return _Type.Description;
            }
        }

        /// <summary>
        /// Returns the file type icon
        /// </summary>
        /// <returns></returns>
        public System.Drawing.Icon FileTypeIcon
        {
            get
            {
                if (_Type is null)
                    return null;
                return _Type.DefaultIcon;
            }
        }

        /// <summary>
        /// Returns the WPF-compatible file type icon image
        /// </summary>
        /// <returns></returns>
        public BitmapSource FileTypeIconImage
        {
            get
            {
                if (_Type is null)
                    return null;
                return _Type.DefaultImage;
            }
        }

        /// <summary>
        /// Returns a Windows Forms compatible icon for the file
        /// </summary>
        /// <returns></returns>
        public System.Drawing.Icon Icon
        {
            get
            {
                if (_IsSpecial | (_Parent is object && _Parent.IsSpecial))
                {
                    if (_Icon is null)
                    {
                        int? argiIndex = null;
                        _Icon = Resources.GetFileIcon(ParsingName, StandardToSystem(_IconSize), iIndex: ref argiIndex);
                    }
                }

                if (_Icon is object)
                    return _Icon;
                else
                    return FileTypeIcon;
            }
        }

        /// <summary>
        /// Returns a WPF-compatible icon image for the file
        /// </summary>
        /// <returns></returns>
        public BitmapSource IconImage
        {
            get
            {
                if (_IsSpecial | (_Parent is object && _Parent.IsSpecial))
                {
                    if (_IconImage is null)
                    {
                        _IconImage = Resources.GetFileIconWPF(ParsingName, StandardToSystem(_IconSize));
                    }
                }

                if (_IconImage is object)
                    return _IconImage;
                else
                    return FileTypeIconImage;
            }
        }

        /// <summary>
        /// Gets or sets the default icon size for the file.
        /// Individual files can override this setting, but they will be reset if the IconSize property of the parent directory is changed.
        /// </summary>
        /// <returns></returns>
        public StandardIcons IconSize
        {
            get
            {
                return _IconSize;
            }

            set
            {
                if (_IconSize == value)
                    return;
                _IconSize = value;
                Refresh(_IconSize);
            }
        }

        public bool IsFolder { get; private set; } = false;

        /// <summary>
        /// Returns whether or not this file is a special file / in a special folder
        /// </summary>
        /// <returns></returns>
        public bool IsSpecial
        {
            get
            {
                return _IsSpecial;
            }
        }

        /// <summary>
        /// Get or set the last access time of the file.
        /// </summary>
        /// <returns></returns>
        public DateTime LastAccessTime
        {
            get
            {
                var c = default(DateTime);
                var a = default(DateTime);
                var m = default(DateTime);
                FileTools.GetFileTime(_Filename, ref c, ref a, ref m);
                return a;
            }

            set
            {
                var c = default(DateTime);
                var a = default(DateTime);
                var m = default(DateTime);
                FileTools.GetFileTime(_Filename, ref c, ref a, ref m);
                FileTools.SetFileTime(_Filename, c, value, m);
            }
        }

        /// <summary>
        /// Get or set the last write time of the file.
        /// </summary>
        /// <returns></returns>
        public DateTime LastWriteTime
        {
            get
            {
                var c = default(DateTime);
                var a = default(DateTime);
                var m = default(DateTime);
                FileTools.GetFileTime(_Filename, ref c, ref a, ref m);
                return m;
            }

            set
            {
                var c = default(DateTime);
                var a = default(DateTime);
                var m = default(DateTime);
                FileTools.GetFileTime(_Filename, ref c, ref a, ref m);
                FileTools.SetFileTime(_Filename, c, a, value);
            }
        }

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        /// <returns></returns>
        public string Name
        {
            get
            {
                return Path.GetFileName(_Filename);
            }

            internal set
            {
                if (!Rename(value))
                {
                    throw new AccessViolationException("Unable to rename file.");
                }
            }
        }

        /// <summary>
        /// Returns the parent directory object if one exists.
        /// </summary>
        /// <returns></returns>
        public ISimpleShellItem Parent
        {
            get
            {
                return _Parent;
            }

            internal set
            {
                _Parent = value;
            }
        }

        /// <summary>
        /// Gets the shell parsing name of a special file
        /// </summary>
        /// <returns></returns>
        public string ParsingName { get; private set; }

        /// <summary>
        /// Get the size of the file, in bytes.
        /// </summary>
        /// <returns></returns>
        public long Size
        {
            get
            {
                return FileTools.GetFileSize(_Filename);
            }
        }

        /// <summary>
        /// Return the file type object.
        /// </summary>
        /// <returns></returns>
        public SystemFileType TypeObject
        {
            get
            {
                return _Type;
            }
        }

        ICollection<ISimpleShellItem> ISimpleShellItem.Children
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        ICollection<ISimpleShellItem> ISimpleShellItem.Folders
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Attempt to move the file to a new directory.
        /// </summary>
        /// <param name="newDirectory">Destination directory.</param>
        /// <returns>True if successful.</returns>
        public bool Move(string newDirectory)
        {
            if (_IsSpecial)
                return false;
            if (newDirectory.Substring(newDirectory.Length - 1, 1) == @"\")
                newDirectory = newDirectory.Substring(0, newDirectory.Length - 1);
            if (!System.IO.Directory.Exists(newDirectory))
                return false;
            string p = Path.GetFileName(_Filename);
            string f = newDirectory + @"\" + p;
            if (FileTools.MoveFile(_Filename, f))
            {
                _Filename = f;
                Refresh();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Refresh the state of the file object from the disk.
        /// </summary>
        /// <param name="iconSize">The size of the icon to fetch from the system.</param>
        /// <returns></returns>
        public void Refresh(StandardIcons? iconSize = default)
        {
            if (iconSize is null)
                iconSize = _IconSize;
            else
                _IconSize = (StandardIcons)iconSize;
            
            if (!File.Exists(_Filename))
                return;
            
            _Type = SystemFileType.FromExtension(Path.GetExtension(_Filename), size: _IconSize);

            if (_IsSpecial | (_Parent is object && _Parent.IsSpecial))
            {
                var st = StandardToSystem(_IconSize);
                _IconImage = Resources.GetFileIconWPF(ParsingName, st);
                int? argiIndex = null;
                _Icon = Resources.GetFileIcon(ParsingName, st, iIndex: ref argiIndex);
            }

            // if we are no longer in the directory of the original parent, set to null
            if (!_IsSpecial && _Parent is object)
            {
                DirectoryObject v = (DirectoryObject)_Parent;
                if ((v.Directory.ToLower() ?? "") != (Directory.ToLower() ?? ""))
                {
                    v.Remove(this);
                    _Parent = null;
                }
            }

            OnPropertyChanged("Icon");
            OnPropertyChanged("IconImage");
            OnPropertyChanged("IconSize");
            OnPropertyChanged("ParsingName");
            OnPropertyChanged("DisplayName");
            OnPropertyChanged("Size");
            OnPropertyChanged("LastWriteTime");
            OnPropertyChanged("LastAccessTime");
            OnPropertyChanged("CreationTime");
        }

        /// <summary>
        /// Attempt to rename the file.
        /// </summary>
        /// <param name="newName">The new name of the file.</param>
        /// <returns>True if successful</returns>
        public bool Rename(string newName)
        {
            if (_IsSpecial)
                return false;
            string p = Path.GetDirectoryName(_Filename);
            string f = p + @"\" + newName;
            if (!FileTools.MoveFile(_Filename, f))
            {
                return false;
            }

            _Filename = f;
            Refresh();
            return true;
        }

        public override string ToString()
        {
            return DisplayName ?? Filename;
        }

        internal IShellItem SysInterface
        {
            get
            {
                return _SysInterface;
            }
        }

        internal static Resources.SystemIconSizes StandardToSystem(StandardIcons stdIcon)
        {
            Resources.SystemIconSizes st;
            switch (stdIcon)
            {
                case StandardIcons.Icon16:
                    {
                        st = Resources.SystemIconSizes.Small;
                        break;
                    }

                case StandardIcons.Icon32:
                    {
                        st = Resources.SystemIconSizes.Large;
                        break;
                    }

                case StandardIcons.Icon48:
                case StandardIcons.Icon64:
                    {
                        st = Resources.SystemIconSizes.ExtraLarge;
                        break;
                    }

                default:
                    {
                        st = Resources.SystemIconSizes.Jumbo;
                        break;
                    }
            }

            return st;
        }
    }
}