using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;
using CoreCT.Memory;
using DataTools.Interop.Native;
using Microsoft.VisualBasic.CompilerServices;

namespace DataTools.Interop.Desktop
{

    /// <summary>
    /// Provides a file-system-locked object to represent the contents of a directory.
    /// </summary>
    public class DirectoryObject : ICollection<ISimpleShellItem>, ISimpleShellItem
    {
        private IShellFolder _SysInterface;
        private List<ISimpleShellItem> _Children = new List<ISimpleShellItem>();
        private List<ISimpleShellItem> _Folders = new List<ISimpleShellItem>();
        private string _DisplayName;
        private System.Drawing.Icon _Icon;
        private BitmapSource _IconImage;
        private StandardIcons _IconSize = StandardIcons.Icon48;
        private bool _IsSpecial;
        private ISimpleShellItem _Parent;
        private string _Path;

        public event PropertyChangedEventHandler PropertyChanged;

        public static ISimpleShellItem CreateRootView(StandardIcons iconSize = StandardIcons.Icon48)
        {
            var d = new DirectoryObject();

            // 'd.Add(New DirectoryObject("QuickAccessFolder", True, True))
            d.Add(new DirectoryObject("MyComputerFolder", true, true, iconSize));
            d.Add(new DirectoryObject("NetworkPlacesFolder", true, false, iconSize));
            // 'd.Add(New DirectoryObject("ControlPanelFolder", True, True))

            return d;
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private DirectoryObject()
        {
            DisplayName = "";
        }

        /// <summary>
        /// Create a new file-system-linked directory object
        /// </summary>
        /// <param name="path"></param>
        public DirectoryObject(string parsingName, bool isSpecial = false, bool initialize = true, StandardIcons iconSize = StandardIcons.Icon48)
        {
            if (string.IsNullOrEmpty(parsingName))
            {
                throw new ArgumentNullException(nameof(Path), "Path is null or not found.");
            }

            _IsSpecial = isSpecial;
            _IconSize = iconSize;

            // Dim mm As New MemPtr
            // Dim res As HResult = SHCreateItemFromParsingName(path, Nothing, Guid.Parse(ShellIIDGuid.IShellItem), shitem)
            // Dim fp As String = Nothing
            // Dim parts As String() = Nothing

            // If (res = HResult.Ok) Then
            // shitem.GetDisplayName(ShellItemDesignNameOptions.DesktopAbsoluteParsing, mm)

            // fp = mm
            // ParsingName = fp

            // mm.CoTaskMemFree()
            // ElseIf (path.Substring(0, 2) = "::") Then
            // parts = path.Split("\")

            // res = SHCreateItemFromParsingName(parts(parts.Length - 1), Nothing, Guid.Parse(ShellIIDGuid.IShellItem), shitem)
            // If res = HResult.Ok Then
            // shitem.GetDisplayName(ShellItemDesignNameOptions.DesktopAbsoluteParsing, mm)

            // fp = mm
            // ParsingName = fp

            // mm.CoTaskMemFree()
            // End If
            // End If

            // res = SHCreateItemFromParsingName("shell:" + If(parts IsNot Nothing, parts(parts.Length - 1), If(fp, path)), Nothing, Guid.Parse(ShellIIDGuid.IShellItem), shitem)

            // If (res = HResult.Ok) Then
            // Dim ip As IntPtr

            // res = shitem.GetDisplayName(ShellItemDesignNameOptions.DesktopAbsoluteParsing, ip)
            // mm = ip

            // If (res = HResult.Ok) Then
            // CanonicalName = If(fp Is Nothing, "shell:" + path, mm.ToString())
            // If (ParsingName Is Nothing) Then ParsingName = mm

            // _Path = ParsingName

            // mm.CoTaskMemFree()
            // shitem.GetDisplayName(ShellItemDesignNameOptions.Normal, mm)

            // DisplayName = mm
            // mm.CoTaskMemFree()
            // End If
            // End If


            // shitem = Nothing

            if (_IsSpecial)
            {
                // ' let's see if we can parse it.
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
                    {
                        Refresh(_IconSize);
                    }
                    else
                    {
                        _Folders.Add(new DirectoryObject());
                        OnPropertyChanged("Folders");
                    }

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
                    _Path = ParsingName;
                    mm.CoTaskMemFree();

                    shitem.GetDisplayName(ShellItemDesignNameOptions.Normal, out mm.handle);

                    DisplayName = (string)mm;
                    mm.CoTaskMemFree();
                }

                shitem = null;
                if (!string.IsNullOrEmpty(DisplayName) && !string.IsNullOrEmpty(parsingName))
                {
                    _IsSpecial = true;
                    if (initialize)
                    {
                        Refresh(_IconSize);
                    }
                    else
                    {
                        _Folders.Add(new DirectoryObject());
                        OnPropertyChanged("Folders");
                    }

                    return;
                }
            }

            if (!string.IsNullOrEmpty(ParsingName))
            {
                _Path = ParsingName;
            }
            else
            {
                _Path = parsingName;
                ParsingName = parsingName;
            }

            if (System.IO.Directory.Exists(_Path) == false)
            {
                return;
            }
            // Throw New DirectoryNotFoundException("Directory Not Found: " & _Path)
            else
            {
                DisplayName = Path.GetFileName(_Path);
                CanonicalName = _Path;
                parsingName = _Path;
            }

            if (initialize)
            {
                Refresh(_IconSize);
            }
            else
            {
                _Folders.Add(new DirectoryObject());
                OnPropertyChanged("Folders");
            }
        }

        /// <summary>
        /// Gets or sets the directory attributes.
        /// </summary>
        /// <returns></returns>
        public FileAttributes Attributes
        {
            get
            {
                return Utility.GetFileAttributes(ParsingName);
            }

            set
            {
                Utility.SetFileAttributes(ParsingName, value);
            }
        }

        /// <summary>
        /// Gets the canonical name of a known folder
        /// </summary>
        /// <returns></returns>
        public string CanonicalName { get; private set; }

        /// <summary>
        /// Get or set the creation time of the directory.
        /// </summary>
        /// <returns></returns>
        public DateTime CreationTime
        {
            get
            {
                var c = default(DateTime);
                var a = default(DateTime);
                var m = default(DateTime);
                Utility.GetFileTime(ParsingName, ref c, ref a, ref m);
                return c;
            }

            set
            {
                var c = default(DateTime);
                var a = default(DateTime);
                var m = default(DateTime);
                Utility.GetFileTime(ParsingName, ref c, ref a, ref m);
                Utility.SetFileTime(ParsingName, value, a, m);
            }
        }

        /// <summary>
        /// Returns the full path of the directory.
        /// </summary>
        /// <returns></returns>
        public string Directory
        {
            get
            {
                return _Path;
            }
        }

        /// <summary>
        /// Gets the display name of the folder
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
        /// Returns a Windows Forms compatible icon for the directory
        /// </summary>
        /// <returns></returns>
        public System.Drawing.Icon Icon
        {
            get
            {
                if (_Icon is null)
                {
                    int? argiIndex = null;
                    _Icon = Resources.GetFileIcon(ParsingName, FileObject.StandardToSystem(_IconSize), iIndex: ref argiIndex);
                }

                return _Icon;
            }
        }

        /// <summary>
        /// Returns a WPF-compatible icon image for the directory
        /// </summary>
        /// <returns></returns>
        public BitmapSource IconImage
        {
            get
            {
                if (_IconImage is null)
                {
                    _IconImage = Resources.GetFileIconWPF(ParsingName, FileObject.StandardToSystem(_IconSize));
                }

                return _IconImage;
            }
        }

        /// <summary>
        /// Gets or sets the default icon size for the directory.
        /// Individual files can override this setting, but they will be reset if this setting is changed while this directory is the parent of the file.
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
                if (_IconSize == value && _Icon is object && _IconImage is object)
                    return;
                _IconSize = value;
                int? argiIndex = null;
                _Icon = Resources.GetFileIcon(ParsingName, FileObject.StandardToSystem(_IconSize), iIndex: ref argiIndex);
                _IconImage = Resources.GetFileIconWPF(ParsingName, FileObject.StandardToSystem(_IconSize));
                if (_Children is object && _Children.Count > 0)
                {
                    foreach (var f in _Children)
                        f.IconSize = _IconSize;
                }
            }
        }

        bool ISimpleShellItem.IsFolder { get => true; }

        /// <summary>
        /// Returns whether or not this directory is a special folder
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
        /// Get or set the last access time of the directory.
        /// </summary>
        /// <returns></returns>
        public DateTime LastAccessTime
        {
            get
            {
                var c = default(DateTime);
                var a = default(DateTime);
                var m = default(DateTime);
                Utility.GetFileTime(ParsingName, ref c, ref a, ref m);
                return a;
            }

            set
            {
                var c = default(DateTime);
                var a = default(DateTime);
                var m = default(DateTime);
                Utility.GetFileTime(ParsingName, ref c, ref a, ref m);
                Utility.SetFileTime(ParsingName, c, value, m);
            }
        }

        /// <summary>
        /// Get or set the last write time of the directory.
        /// </summary>
        /// <returns></returns>
        public DateTime LastWriteTime
        {
            get
            {
                var c = default(DateTime);
                var a = default(DateTime);
                var m = default(DateTime);
                Utility.GetFileTime(ParsingName, ref c, ref a, ref m);
                return m;
            }

            set
            {
                var c = default(DateTime);
                var a = default(DateTime);
                var m = default(DateTime);
                Utility.GetFileTime(ParsingName, ref c, ref a, ref m);
                Utility.SetFileTime(ParsingName, c, a, value);
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
        /// Gets the shell parsing name of a special folder
        /// </summary>
        /// <returns></returns>
        public string ParsingName { get; private set; }

        ICollection<ISimpleShellItem> ISimpleShellItem.Children
        {
            get
            {
                return this;
            }
        }

        public ICollection<ISimpleShellItem> Folders
        {
            get
            {
                return _Folders;
            }
        }

        /// <summary>
        /// Not implemented in directory
        /// </summary>
        /// <returns></returns>
        long ISimpleShellItem.Size
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Get a directory object from a file object.
        /// The <see cref="FileObject"/> instance passed is retained in the returned <see cref="DirectoryObject"/> object.
        /// </summary>
        /// <param name="fileObj">A <see cref="FileObject"/></param>
        /// <returns>A <see cref="DirectoryObject"/>.</returns>
        public static DirectoryObject FromFileObject(FileObject fileObj)
        {
            var dir = new DirectoryObject(fileObj.Directory, fileObj.IsSpecial);
            return dir;
        }

        /// <summary>
        /// Refresh the contents of the directory.
        /// </summary>
        public void Refresh(StandardIcons? iconSize = default)
        {
            if (iconSize is null)
                iconSize = _IconSize;

            _Children.Clear();
            _Folders.Clear();

            FileObject fobj;
            DirectoryObject dobj;
            IShellItem shitem = null;
            IShellFolder shfld = null;
            IEnumIDList enumer = null;

            var mm = new MemPtr();
            var mm2 = new MemPtr();
            var mc = new NativeShell.STRRET();

            string fp;

            object objret = null;
            string pname = ParsingName;

            if (pname is object && pname.LastIndexOf(@"\") == pname.Length - 1)
                pname = pname.Substring(0, pname.Length - 1);

            var argriid = Guid.Parse(ShellIIDGuid.IShellItem);
            var res = NativeShell.SHCreateItemFromParsingName(ParsingName, IntPtr.Zero, ref argriid, ref shitem);

            _IconSize = (StandardIcons)Conversions.ToUShort(iconSize);

            int? argiIndex = null;

            _Icon = Resources.GetFileIcon(ParsingName, FileObject.StandardToSystem(_IconSize), iIndex: ref argiIndex);
            _IconImage = Resources.GetFileIconWPF(ParsingName, FileObject.StandardToSystem(_IconSize));

            if (res == HResult.Ok)
            {
                var argbhid = Guid.Parse(ShellBHIDGuid.ShellFolderObject);
                var argriid1 = Guid.Parse(ShellIIDGuid.IShellFolder2);

                shitem.BindToHandler(IntPtr.Zero, ref argbhid, ref argriid1, out shfld);
                _SysInterface = shfld;

                shfld.EnumObjects(IntPtr.Zero, ShellFolderEnumerationOptions.Folders | ShellFolderEnumerationOptions.IncludeHidden | ShellFolderEnumerationOptions.NonFolders | ShellFolderEnumerationOptions.InitializeOnFirstNext, out enumer);

                if (enumer != null)
                {
                    var glist = new List<string>();
                    uint cf;
                    var x = IntPtr.Zero;
                    string pout;

                    // mm.AllocCoTaskMem((MAX_PATH * 2) + 8)

                    mm2.Alloc(FileApi.MAX_PATH * 2 + 8);

                    do
                    {
                        cf = 0U;
                        mm2.ZeroMemory(0L, FileApi.MAX_PATH * 2 + 8);
                        res = enumer.Next(1U, out x, out cf);
                        mm = x;

                        if (cf == 0L)
                            break;

                        if (res != HResult.Ok)
                            break;

                        mm2.IntAt(0L) = 2;

                        // shfld.GetAttributesOf(1, mm, attr)
                        shfld.GetDisplayNameOf(mm, (uint)ShellItemDesignNameOptions.ParentRelativeParsing, mm2.handle);
                        MemPtr inv;

                        if (IntPtr.Size == 4)
                        {
                            inv = (IntPtr)mm2.IntAt(1L);
                        }
                        else
                        {
                            inv = (IntPtr)mm2.LongAt(1L);
                        }

                        if (inv.Handle != IntPtr.Zero)
                        {
                            if (inv.CharAt(0L) != '\0')
                            {
                                fp = (string)inv;
                                var lpInfo = new PInvoke.SHFILEINFO();

                                // Dim sgfin As ShellFileGetAttributesOptions = 0,
                                // sgfout As ShellFileGetAttributesOptions = 0

                                int iFlags = PInvoke.SHGFI_PIDL | PInvoke.SHGFI_ATTRIBUTES;
                                lpInfo.dwAttributes = 0;
                                x = PInvoke.SHGetItemInfo(mm.Handle, 0, ref lpInfo, Marshal.SizeOf(lpInfo), iFlags);
                                if (ParsingName is object)
                                {
                                    if (pname.LastIndexOf(@"\") == pname.Length - 1)
                                        pname = pname.Substring(0, pname.Length - 1);
                                    pout = $@"{pname}\{fp}";
                                }
                                else
                                {
                                    pout = fp;
                                }

                                if (lpInfo.dwAttributes == 0)
                                {
                                    lpInfo.dwAttributes = (int)Utility.GetFileAttributes(pout);
                                }

                                FileAttributes drat = (FileAttributes)Conversions.ToInteger(lpInfo.dwAttributes);
                                if ((lpInfo.dwAttributes & (int)FileAttributes.Directory) == (int)FileAttributes.Directory && !File.Exists(pout))
                                {
                                    dobj = new DirectoryObject(pout, _IsSpecial, false);
                                    dobj.Parent = this;
                                    dobj.IconSize = _IconSize;
                                    _Children.Add(dobj);
                                    _Folders.Add(dobj);
                                }
                                else
                                {
                                    fobj = new FileObject(pout, _IsSpecial, true, _IconSize);
                                    fobj.Parent = this;
                                    fobj.IconSize = _IconSize;
                                    _Children.Add(fobj);
                                }
                            }

                            inv.CoTaskMemFree();
                        }

                        mm.CoTaskMemFree();
                    }
                    while (res == HResult.Ok);
                    mm2.Free();
                }
            }

            OnPropertyChanged("Children");
            OnPropertyChanged("Folders");
            OnPropertyChanged("Icon");
            OnPropertyChanged("IconImage");
            OnPropertyChanged("IconSize");
            OnPropertyChanged("ParsingName");
            OnPropertyChanged("DisplayName");
        }

        internal IShellFolder SysInterface
        {
            get
            {
                return _SysInterface;
            }
        }

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Returns the number of files in the directory.
        /// </summary>
        /// <returns></returns>
        public int Count
        {
            get
            {
                return _Children.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Returns an item in the collection.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ISimpleShellItem this[int index]
        {
            get
            {
                return _Children[index];
            }
        }

        public bool Contains(string name, bool useParsingName, ref ISimpleShellItem obj)
        {
            string fn = name.ToLower();
            string fn2;
            foreach (var f in _Children)
            {
                fn2 = useParsingName ? f.ParsingName.ToLower() : f.DisplayName.ToLower();
                if ((fn2 ?? "") == (fn ?? ""))
                {
                    obj = f;
                    return true;
                }
            }

            return false;
        }

        public bool Contains(ISimpleShellItem item)
        {
            return _Children.Contains(item);
        }

        public void CopyTo(ISimpleShellItem[] array, int arrayIndex)
        {
            _Children.CopyTo(array, arrayIndex);
        }

        public IEnumerator<ISimpleShellItem> GetEnumerator()
        {
            return new DirEnumer(this);
        }

        internal void Add(ISimpleShellItem item)
        {
            _Children.Add(item);
        }

        internal void Clear()
        {
            _Children.Clear();
        }

        internal bool Remove(ISimpleShellItem item)
        {
            return _Children.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new DirEnumer(this);
        }

        void ICollection<ISimpleShellItem>.Add(ISimpleShellItem item)
        {
            ((ICollection<ISimpleShellItem>)_Children).Add(item);
        }

        void ICollection<ISimpleShellItem>.Clear()
        {
            ((ICollection<ISimpleShellItem>)_Children).Clear();
        }

        bool ICollection<ISimpleShellItem>.Remove(ISimpleShellItem item)
        {
            return ((ICollection<ISimpleShellItem>)_Children).Remove(item);
        }

        private class DirEnumer : IEnumerator<ISimpleShellItem>
        {
            private DirectoryObject _obj;
            private int _pos = -1;

            public DirEnumer(DirectoryObject obj)
            {
                _obj = obj;
            }

            public ISimpleShellItem Current
            {
                get
                {
                    return _obj[_pos];
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    return _obj[_pos];
                }
            }

            public bool MoveNext()
            {
                _pos += 1;
                if (_pos >= _obj.Count)
                    return false;
                return true;
            }

            public void Reset()
            {
                _pos = -1;
            }
            /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
            /* TODO ERROR: Skipped RegionDirectiveTrivia */
            private bool disposedValue; // To detect redundant calls

            // This code added by Visual Basic to correctly implement the disposable pattern.
            public void Dispose()
            {
                Dispose(true);
            }

            // IDisposable
            protected virtual void Dispose(bool disposing)
            {
                if (!disposedValue)
                {
                    if (disposing)
                    {
                        _obj = null;
                        _pos = -1;
                    }
                }

                disposedValue = true;
            }
            /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        }
    }
}