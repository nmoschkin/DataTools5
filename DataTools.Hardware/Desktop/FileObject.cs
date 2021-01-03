using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;

using DataTools.Win32;
using DataTools.Shell.Native;
using DataTools.Win32.Memory;
using System.Runtime.InteropServices;

namespace DataTools.Desktop
{

    /// <summary>
    /// Provides a file-system-locked object to represent a file.
    /// </summary>
    public class FileObject : ISimpleShellItem
    {
        private IShellItem shellObj;
        private IShellItemPS psshellObj;

        private string displayName;
        private string filename;
        private System.Drawing.Icon icon;
        private BitmapSource iconImage;
        private StandardIcons iconSize = StandardIcons.Icon48;
        private bool isSpecial;
        private ISimpleShellItem parent;
        private SystemFileType type;

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
            this.isSpecial = isSpecial;
            filename = parsingName;
            try
            {

                var mm = new MemPtr();
                var riid = Guid.Parse(ShellIIDGuid.IShellItem);
                HResult res;

                if (this.isSpecial)
                {
                    // let's see if we can parse it.
                    IShellItem shitem = null;
                    IShellItemPS pssh = null;

                    res = NativeShell.SHCreateItemFromParsingName(parsingName, IntPtr.Zero, ref riid, ref shitem);
                    string fp = null;

                    if (res == HResult.Ok)
                    {
                        NativeShell.SHCreateItemFromParsingName(parsingName, IntPtr.Zero, ref riid, ref pssh);
                        psshellObj = pssh;

                        shitem.GetDisplayName(ShellItemDesignNameOptions.DesktopAbsoluteParsing, out mm.handle);
                        fp = (string)mm;

                        ParsingName = fp;
                        CanonicalName = fp;

                        mm.CoTaskMemFree();

                        shitem.GetDisplayName(ShellItemDesignNameOptions.Normal, out mm.handle);

                        DisplayName = (string)mm;

                        mm.CoTaskMemFree();

                        this.isSpecial = true;

                        if (initialize)
                            Refresh();

                        shellObj = shitem;

                        return;
                    }

                    HResult localSHCreateItemFromParsingName() { riid = Guid.Parse(ShellIIDGuid.IShellItem); var ret = NativeShell.SHCreateItemFromParsingName("shell:" + (fp ?? parsingName), IntPtr.Zero, ref riid, ref shitem); return ret; }

                    res = localSHCreateItemFromParsingName();
                    
                    if (res == HResult.Ok)
                    {

                        shitem.GetDisplayName(ShellItemDesignNameOptions.DesktopAbsoluteParsing, out mm.handle);
                        CanonicalName = (string)mm;

                        if (ParsingName is null)
                            ParsingName = (string)mm;

                        filename = ParsingName;
                        mm.CoTaskMemFree();

                        
                        shitem.GetDisplayName(ShellItemDesignNameOptions.Normal, out mm.handle);

                        DisplayName = (string)mm;

                        mm.CoTaskMemFree();
                    }

                    shellObj = shitem;
                    shitem = null;
                    
                    if (!string.IsNullOrEmpty(DisplayName) && !string.IsNullOrEmpty(ParsingName))
                    {
                        this.isSpecial = true;
                        if (initialize)
                            Refresh(this.iconSize);
                        return;
                    }
                }

                if (File.Exists(parsingName) == false)
                {
                    if (!this.isSpecial)
                        throw new FileNotFoundException("File Not Found: " + parsingName);
                }
                else if (initialize)
                    Refresh(this.iconSize);
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
                return FileTools.GetFileAttributes(filename);
            }

            set
            {
                FileTools.SetFileAttributes(filename, value);
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
                FileTools.GetFileTime(filename, ref c, ref a, ref m);
                return c;
            }

            set
            {
                var c = default(DateTime);
                var a = default(DateTime);
                var m = default(DateTime);
                FileTools.GetFileTime(filename, ref c, ref a, ref m);
                FileTools.SetFileTime(filename, value, a, m);
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
                return Path.GetDirectoryName(filename);
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
                return displayName;
            }

            set
            {
                displayName = value;
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
                return filename;
            }

            internal set
            {
                if (filename is object)
                {
                    if (!FileTools.MoveFile(filename, value))
                    {
                        throw new AccessViolationException("Unable to rename/move file.");
                    }
                }
                else if (!File.Exists(value))
                {
                    throw new FileNotFoundException("File Not Found: " + Filename);
                }

                filename = value;
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
                if (type is null)
                    return "Unknown";
                return type.Description;
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
                if (type is null)
                    return null;
                return type.DefaultIcon;
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
                if (type is null)
                    return null;
                return type.DefaultImage;
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
                if (isSpecial | (parent is object && parent.IsSpecial))
                {
                    if (icon is null)
                    {
                        int? argiIndex = null;
                        icon = Resources.GetFileIcon(ParsingName, StandardToSystem(iconSize), iIndex: ref argiIndex);
                    }
                }

                if (icon is object)
                    return icon;
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
                if (isSpecial | (parent is object && parent.IsSpecial))
                {
                    if (iconImage is null)
                    {
                        iconImage = Resources.GetFileIconWPF(ParsingName, StandardToSystem(iconSize));
                    }
                }

                if (iconImage is object)
                    return iconImage;
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
                return iconSize;
            }

            set
            {
                if (iconSize == value)
                    return;
                iconSize = value;
                Refresh(iconSize);
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
                return isSpecial;
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
                FileTools.GetFileTime(filename, ref c, ref a, ref m);
                return a;
            }

            set
            {
                var c = default(DateTime);
                var a = default(DateTime);
                var m = default(DateTime);
                FileTools.GetFileTime(filename, ref c, ref a, ref m);
                FileTools.SetFileTime(filename, c, value, m);
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
                FileTools.GetFileTime(filename, ref c, ref a, ref m);
                return m;
            }

            set
            {
                var c = default(DateTime);
                var a = default(DateTime);
                var m = default(DateTime);
                FileTools.GetFileTime(filename, ref c, ref a, ref m);
                FileTools.SetFileTime(filename, c, a, value);
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
                return Path.GetFileName(filename);
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
                return parent;
            }

            internal set
            {
                parent = value;
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
                return FileTools.GetFileSize(filename);
            }
        }

        public Dictionary<string, PropertyKey> GetPropertiesWithNames()
        {
            return PropertyKeys.PropListToDict(GetPropertyStore());
        }

        public List<PropertyKey> GetPropertyStore()
        {
            if (psshellObj == null) return null;

            var g1 = Guid.Parse(ShellBHIDGuid.PropertyStore);
            var g2 = Guid.Parse(ShellIIDGuid.IPropertyStore);

            IPropertyStore p;

            var h = psshellObj.BindToHandler(IntPtr.Zero, ref g1, ref g2, out p);

            if (h != HResult.Ok) return null;

            PropertyKey pk = new PropertyKey();
            uint count;
            int i;

            h = p.GetCount(out count);
            if (h != HResult.Ok) return null;

            List<PropertyKey> pks = new List<PropertyKey>();
            
            for (i = 0; i < count; i++)
            {
                h = p.GetAt((uint)i, ref pk);
                if (h != HResult.Ok) return null;

                pks.Add(pk);
           
            }

            return pks;
        }

        public object GetPropertyValue(string key)
        {
            var pk = PropertyKeys.FindByName(key);
            if (!pk.HasValue) return null;

            return GetPropertyValue(pk.Value);
        }

        public object GetPropertyValue(PropertyKey key)
        {
            if (psshellObj == null) return null;

            var g1 = Guid.Parse(ShellBHIDGuid.PropertyStore);
            var g2 = Guid.Parse(ShellIIDGuid.IPropertyStore);

            IPropertyStore p;

            var h = psshellObj.BindToHandler(IntPtr.Zero, ref g1, ref g2, out p);

            if (h != HResult.Ok) return null;

            try
            {
                PropVariant pv = new PropVariant();

                h = p.GetValue(ref key, pv);

                if (h == HResult.Ok) return pv.Value;
                else return null;

            }
            catch(Exception ex)
            {
                return null;
            }

        }

        public bool SetPropertyValue(PropertyKey key, object value)
        {
            if (psshellObj == null) return false;

            var g1 = Guid.Parse(ShellBHIDGuid.PropertyStore);
            var g2 = Guid.Parse(ShellIIDGuid.IPropertyStore);

            IPropertyStore p;

            var h = psshellObj.BindToHandler(IntPtr.Zero, ref g1, ref g2, out p);

            if (h != HResult.Ok) return false;

            try
            {
                PropVariant pv = PropVariant.FromObject(value);
                h = p.SetValue(ref key, pv);

                if (h != HResult.Ok) return false;
                return true;
            }
            catch
            {
                return false;
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
                return type;
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
            if (isSpecial)
                return false;
            if (newDirectory.Substring(newDirectory.Length - 1, 1) == @"\")
                newDirectory = newDirectory.Substring(0, newDirectory.Length - 1);
            if (!System.IO.Directory.Exists(newDirectory))
                return false;
            string p = Path.GetFileName(filename);
            string f = newDirectory + @"\" + p;
            if (FileTools.MoveFile(filename, f))
            {
                filename = f;
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
                iconSize = this.iconSize;
            else
                this.iconSize = (StandardIcons)iconSize;
            
            if (!File.Exists(filename))
                return;

            type = SystemFileType.FromExtension(Path.GetExtension(filename), size: this.iconSize);

            if (isSpecial | (parent is object && parent.IsSpecial))
            {
                var st = StandardToSystem(this.iconSize);
                iconImage = Resources.GetFileIconWPF(ParsingName, st);
                int? argiIndex = null;
                icon = Resources.GetFileIcon(ParsingName, st, iIndex: ref argiIndex);
            }

            // if we are no longer in the directory of the original parent, set to null
            if (!isSpecial && parent is object)
            {
                DirectoryObject v = (DirectoryObject)parent;
                if ((v.Directory.ToLower() ?? "") != (Directory.ToLower() ?? ""))
                {
                    v.Remove(this);
                    parent = null;
                }
            }

            // get the shell interface for the specified non-special file item.
            // we can use this to get more interfaces from the system.

            if (shellObj == null && !isSpecial)
            {
                IShellItem shitem = null;
                IShellItemPS pssh = null;

                var riid = Guid.Parse(ShellIIDGuid.IShellItem);

                HResult res = NativeShell.SHCreateItemFromParsingName(filename, IntPtr.Zero, ref riid, ref shitem);

                if (res == HResult.Ok)
                {
                    shellObj = shitem;

                    NativeShell.SHCreateItemFromParsingName(filename, IntPtr.Zero, ref riid, ref pssh);
                    psshellObj = pssh;
                }
            }

            OnPropertyChanged(nameof(Icon));
            OnPropertyChanged(nameof(IconImage));
            OnPropertyChanged(nameof(IconSize));
            OnPropertyChanged(nameof(ParsingName));
            OnPropertyChanged(nameof(DisplayName));
            OnPropertyChanged(nameof(Size));
            OnPropertyChanged(nameof(LastWriteTime));
            OnPropertyChanged(nameof(LastAccessTime));
            OnPropertyChanged(nameof(CreationTime));
        }

        /// <summary>
        /// Attempt to rename the file.
        /// </summary>
        /// <param name="newName">The new name of the file.</param>
        /// <returns>True if successful</returns>
        public bool Rename(string newName)
        {
            if (isSpecial)
                return false;
            string p = Path.GetDirectoryName(filename);
            string f = p + @"\" + newName;
            if (!FileTools.MoveFile(filename, f))
            {
                return false;
            }

            filename = f;
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
                return shellObj;
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