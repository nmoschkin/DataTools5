// ' ************************************************* ''
// ' DataTools Visual Basic Utility Library - Interop
// '
// ' Module: System file association utility classes.
// ' 
// ' Copyright (C) 2011-2020 Nathan Moschkin
// ' All Rights Reserved
// '
// ' Licensed Under the Microsoft Public License   
// ' ************************************************* ''

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using DataTools.Hardware.Native;
using DataTools.Hardware.Native.Menu;
using DataTools.Shell.Native;
using Microsoft.Win32;

namespace DataTools.Desktop
{

    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    /// <summary>
    /// Represents a registered file-type handler program.
    /// </summary>
    public sealed class UIHandler : INotifyPropertyChanged, IDisposable
    {
        private string _UIName;
        private string _ExePath;
        private Icon _Icon;
        private System.Windows.Media.Imaging.BitmapSource _Image;
        private AllSystemFileTypes _Parent;
        private IAssocHandler _Handler;
        private List<string> _ExtList = new List<string>();
        private bool _Preferred;
        private System.Collections.ObjectModel.ObservableCollection<SystemFileType> _AssocList = new System.Collections.ObjectModel.ObservableCollection<SystemFileType>();

        /// <summary>
        /// Gets the list of supported extensions, separated by commas.
        /// </summary>
        /// <returns></returns>
        public string ExtListString
        {
            get
            {
                var sb = new System.Text.StringBuilder();
                int cc = 0;
                int x = 0;
                foreach (var s in _ExtList)
                {
                    if (x > 0)
                        sb.Append(", ");
                    x += 1;
                    if (cc >= 80)
                    {
                        sb.Append("\r\n");
                        cc = 0;
                    }

                    cc += s.Length;
                    sb.Append(s);
                }

                return sb.ToString();
            }
        }

        /// <summary>
        /// Gets a value indicating that this is a recommended file handler.
        /// </summary>
        /// <returns></returns>
        public bool Preferred
        {
            get
            {
                return _Preferred;
            }

            internal set
            {
                _Preferred = value;
            }
        }

        /// <summary>
        /// Returns the size of the program icon.
        /// </summary>
        /// <returns></returns>
        public StandardIcons IconSize
        {
            get
            {
                if (_Parent is null)
                    return StandardIcons.Icon48;
                return _Parent.IconSize;
            }
        }

        internal UIHandler(IAssocHandler handler, AllSystemFileTypes parent)
        {
            _Parent = parent;
            Refresh(handler);
        }

        /// <summary>
        /// Retrieves an array of support extensions.
        /// </summary>
        /// <returns></returns>
        public string[] ExtensionList
        {
            get
            {
                return _ExtList.ToArray();
            }

            internal set
            {
                _ExtList.Clear();
                _ExtList.AddRange(value);
            }
        }

        /// <summary>
        /// Rebuild the association halder list.
        /// </summary>
        internal void RebuildAssocList()
        {
            _AssocList.Clear();
            _ExtList.Sort();
            foreach (var s in _ExtList)
            {
                foreach (var f in _Parent.FileTypes)
                {
                    if ((f.Extension ?? "") == (s ?? ""))
                    {
                        _AssocList.Add(f);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Retrieves the list of associated file handlers.
        /// </summary>
        /// <returns></returns>
        public System.Collections.ObjectModel.ObservableCollection<SystemFileType> Items
        {
            get
            {
                if (_AssocList.Count == 0)
                    RebuildAssocList();
                return _AssocList;
            }
        }


        /// <summary>
        /// Retrieves the list of associated file handlers.
        /// </summary>
        /// <returns></returns>
        public System.Collections.ObjectModel.ObservableCollection<SystemFileType> AssocList
        {
            get
            {
                if (_AssocList.Count == 0)
                    RebuildAssocList();
                return _AssocList;
            }
        }

        /// <summary>
        /// Clears the extension list.
        /// </summary>
        internal void ClearExtList()
        {
            _ExtList.Clear();
        }

        /// <summary>
        /// Add an extension.
        /// </summary>
        /// <param name="e"></param>
        internal void AddExt(string e)
        {
            if (_ExtList.Contains(e) == false)
                _ExtList.Add(e);
        }

        /// <summary>
        /// Refresh using the IAssocHandler
        /// </summary>
        /// <param name="handler"></param>
        internal void Refresh(IAssocHandler handler)
        {
            string pth = null;
            int idx = 0;
            uint sz = 0U;
            _Handler = handler;
            Preferred = _Handler.IsRecommended() == HResult.Ok;
            string argppsz = ExePath;
            handler.GetName(out argppsz);
            ExePath = argppsz;
            if (File.Exists(ExePath) == false)
                throw new SystemException("Program path not found");
            string argppsz1 = UIName;
            handler.GetUIName(out argppsz1);
            UIName = argppsz1;
            handler.GetIconLocation(out pth, out idx);
            Icon = Resources.LoadLibraryIcon(pth, idx, IconSize);
            if (Icon is null)
            {
                int iix = (int)NativeShell.Shell_GetCachedImageIndex(pth, idx, 0U);
                switch (IconSize)
                {
                    case StandardIcons.Icon256:
                        {
                            Icon = Resources.GetFileIconFromIndex(iix, Resources.SystemIconSizes.Jumbo);
                            break;
                        }

                    case StandardIcons.Icon48:
                        {
                            Icon = Resources.GetFileIconFromIndex(iix, Resources.SystemIconSizes.ExtraLarge);
                            break;
                        }

                    case StandardIcons.Icon32:
                        {
                            Icon = Resources.GetFileIconFromIndex(iix, Resources.SystemIconSizes.Large);
                            break;
                        }

                    default:
                        {
                            Icon = Resources.GetFileIconFromIndex(iix, Resources.SystemIconSizes.Small);
                            break;
                        }
                }
            }
        }

        internal void Refresh()
        {
            Refresh(_Handler);
        }

        /// <summary>
        /// The friendly name of the program.
        /// </summary>
        /// <returns></returns>
        public string UIName
        {
            get
            {
                return _UIName;
            }

            internal set
            {
                _UIName = value;
                OnPropertyChanged("UIName");
            }
        }

        /// <summary>
        /// The executable path of the program.
        /// </summary>
        /// <returns></returns>
        public string ExePath
        {
            get
            {
                return _ExePath;
            }

            internal set
            {
                _ExePath = value;
                OnPropertyChanged("ExePath");
            }
        }

        /// <summary>
        /// The icon for the executable handler.
        /// </summary>
        /// <returns></returns>
        public Icon Icon
        {
            get
            {
                return _Icon;
            }

            internal set
            {
                _Icon = value;
                Image = Resources.MakeWPFImage(_Icon);
                OnPropertyChanged("Icon");
            }
        }

        /// <summary>
        /// The WPF image for the executable handler.
        /// </summary>
        /// <returns></returns>
        public System.Windows.Media.Imaging.BitmapSource Image
        {
            get
            {
                return _Image;
            }

            internal set
            {
                _Image = value;
                OnPropertyChanged("Image");
            }
        }

        private bool disposedValue; // To detect redundant calls

        public event PropertyChangedEventHandler PropertyChanged;

        protected void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _Handler = null;
                }
            }

            disposedValue = true;
        }

        public void Dispose()
        {
            // Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        public override string ToString()
        {
            return UIName;
        }

        protected void OnPropertyChanged([CallerMemberName] string e = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(e));
        }
    }

    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    /// <summary>
    /// Class that describes information for an event fired by the <see cref="AllSystemFileTypes"/> class for a general enumeration of system file types.
    /// </summary>
    public class FileTypeEnumEventArgs : EventArgs
    {
        private SystemFileType _sft;
        private int _index;
        private int _count;

        /// <summary>
        /// The current index of the system file type that is being processed.
        /// </summary>
        /// <returns></returns>
        public int Index
        {
            get
            {
                return _index;
            }
        }

        /// <summary>
        /// Total number of file types to process.
        /// </summary>
        /// <returns></returns>
        public int Count
        {
            get
            {
                return _count;
            }
        }

        /// <summary>
        /// The current system file-type being processed.
        /// </summary>
        /// <returns></returns>
        public SystemFileType Type
        {
            get
            {
                return _sft;
            }
        }

        /// <summary>
        /// Create a new event object consisting of these variables.
        /// </summary>
        /// <param name="sf">The <see cref="SystemFileType"/> object</param>
        /// <param name="index">The current index</param>
        /// <param name="count">The total number of file types</param>
        internal FileTypeEnumEventArgs(SystemFileType sf, int index, int count)
        {
            _sft = sf;
            _index = index;
            _count = count;
        }
    }

    /// <summary>
    /// Represents a file type.
    /// </summary>
    public sealed class SystemFileType : INotifyPropertyChanged, IDisposable
    {
        private System.Collections.ObjectModel.ObservableCollection<UIHandler> _Col = new System.Collections.ObjectModel.ObservableCollection<UIHandler>();
        private string _Ext;
        private string _Desc;
        private AllSystemFileTypes _Parent;
        private Icon _DefaultIcon;
        private System.Windows.Media.Imaging.BitmapSource _DefaultImage;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the size of the file icon.
        /// </summary>
        /// <returns></returns>
        public StandardIcons IconSize
        {
            get
            {
                return _Parent.IconSize;
            }
        }

        /// <summary>
        /// Returns the default icon.
        /// </summary>
        /// <returns></returns>
        public Icon DefaultIcon
        {
            get
            {
                return _DefaultIcon;
            }

            internal set
            {
                _DefaultIcon = value;
            }
        }

        /// <summary>
        /// Returns the default WPF image.
        /// </summary>
        /// <returns></returns>
        [Browsable(false)]
        public System.Windows.Media.Imaging.BitmapSource DefaultImage
        {
            get
            {
                if (_DefaultImage is null)
                    return PreferredHandler.Image;
                return _DefaultImage;
            }

            internal set
            {
                _DefaultImage = value;
            }
        }

        /// <summary>
        /// Returns the parent object.
        /// </summary>
        /// <returns></returns>
        public AllSystemFileTypes Parent
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
        /// Returns the description of the file extension.
        /// </summary>
        /// <returns></returns>
        public string Description
        {
            get
            {
                return _Desc;
            }
        }

        /// <summary>
        /// Returns the first preferred UIHandler for this extension.
        /// </summary>
        /// <returns></returns>
        public UIHandler PreferredHandler
        {
            get
            {
                foreach (var h in _Col)
                {
                    if (h.Preferred)
                        return h;
                }

                return _Col[0];
            }
        }

        /// <summary>
        /// Gets a list of handlers for this extension.
        /// </summary>
        /// <returns></returns>
        public System.Collections.ObjectModel.ObservableCollection<UIHandler> Handlers
        {
            get
            {
                return _Col;
            }
        }

        /// <summary>
        /// Gets a list of handlers for this extension.
        /// </summary>
        /// <returns></returns>
        public System.Collections.ObjectModel.ObservableCollection<UIHandler> Items
        {
            get
            {
                return _Col;
            }
        }

        /// <summary>
        /// Gets the file extension.
        /// </summary>
        /// <returns></returns>
        public string Extension
        {
            get
            {
                return _Ext;
            }
        }

        internal SystemFileType(AllSystemFileTypes p, string ext)
        {
            _Parent = p;
            _Ext = ext;
        }

        /// <summary>
        /// Create a new <see cref="SystemFileType"/> object for the given extension.
        /// </summary>
        /// <param name="ext"></param>
        public SystemFileType(string ext)
        {
            if (string.IsNullOrEmpty(ext))
                return;
            if (ext[0] != '.')
                _Ext = "." + ext.ToLower();
            else
                _Ext = ext.ToLower();
            OnPropertyChanged("Extension");
        }

        /// <summary>
        /// Creates a new <see cref="SystemFileType"/> object from the given extension with the specified parameters.
        /// </summary>
        /// <param name="ext">The file extension.</param>
        /// <param name="parent">The parent <see cref="AllSystemFileTypes"/> object.</param>
        /// <param name="size">The default icon size.</param>
        /// <returns></returns>
        public static SystemFileType FromExtension(string ext, AllSystemFileTypes parent = null, StandardIcons size = StandardIcons.Icon48)
        {
            var c = new SystemFileType(ext);
            if (parent is object)
                c.Parent = parent;
            var assoc = NativeShell.EnumFileHandlers(ext);
            if (assoc is null || assoc.Count() == 0)
                return null;
            c.Populate(assoc, size);
            if (c.Handlers.Count == 0)
                return null;
            else
                return c;
        }

        /// <summary>
        /// Creates a new <see cref="System.Windows.Media.Imaging.BitmapSource"/> object from the given extension with the specified parameters.
        /// </summary>
        /// <param name="ext">The file extension.</param>
        /// <param name="size">The default icon size.</param>
        /// <returns></returns>
        public static System.Windows.Media.Imaging.BitmapSource ImageFromExtension(string ext, StandardIcons size = StandardIcons.Icon48)
        {
            var sft = FromExtension(ext, size: size);
            if (sft is null)
                return null;
            return sft.DefaultImage;
        }

        /// <summary>
        /// Populate the information for this object.
        /// </summary>
        /// <param name="assoc">Optional array of previously-enumerated IAssocHandlers.</param>
        /// <param name="size">The default icon size.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
        internal bool Populate(IAssocHandler[] assoc = null, StandardIcons size = StandardIcons.Icon48)
        {
            if (assoc is null)
                assoc = NativeShell.EnumFileHandlers(_Ext);
            if (assoc is null)
                return false;
            _Col.Clear();
            string p = null;
            if (_Parent is null)
            {
                foreach (var a in assoc)
                {
                    p = null;
                    a.GetName(out p);
                    if (File.Exists(p) == false)
                        continue;
                    _Col.Add(new UIHandler(a, _Parent));
                }
            }
            else
            {
                foreach (var a in assoc)
                {
                    p = null;
                    a.GetName(out p);
                    if (File.Exists(p) == false)
                        continue;
                    _Col.Add(_Parent.HandlerFromAssocHandler(a, _Ext));
                }
            }

            OnPropertyChanged("Handlers");
            try
            {
                var pk = Registry.ClassesRoot.OpenSubKey(_Ext);
                RegistryKey pk2;
                if (pk is object && (string)(pk.GetValue(null)) is object)
                {
                    pk2 = Registry.ClassesRoot.OpenSubKey((string)(pk.GetValue(null)));
                    if (pk2 is object)
                    {
                        string d = (string)(pk2.GetValue(null));
                        if (string.Equals(d, _Desc) == false)
                        {
                            _Desc = (string)(pk2.GetValue(null));
                            OnPropertyChanged("Description");
                        }

                        pk2.Close();
                        pk2 = Registry.ClassesRoot.OpenSubKey((string)(pk.GetValue(null)) + @"\DefaultIcon");
                        pk.Close();
                        if (pk2 is object)
                        {
                            d = (string)(pk2.GetValue(null));
                            pk2.Close();
                            if (d is object)
                            {
                                int i = d.LastIndexOf(",");
                                int c;
                                if (i == -1)
                                {
                                    c = 0;
                                }
                                else
                                {
                                    c = int.Parse(d.Substring(i + 1));
                                    d = d.Substring(0, i);
                                }

                                _DefaultIcon = Resources.LoadLibraryIcon(d, c, size);
                                if (_DefaultIcon is object)
                                {
                                    _DefaultImage = Resources.MakeWPFImage(_DefaultIcon);
                                    OnPropertyChanged("DefaultImage");
                                    OnPropertyChanged("DefaultIcon");
                                }
                            }
                        }
                    }
                }

                if (_Desc is null || string.IsNullOrEmpty(_Desc))
                    _Desc = _Ext + " file";
            }
            catch (Exception ex)
            {
            }

            var cn = _Col.ToArray();
            _Col.Clear();
            Array.Sort(cn, new UIHandlerComp());
            foreach (var cxn in cn)
                _Col.Add(cxn);
            return true;
        }

        public override string ToString()
        {
            return Description;
        }


        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        private bool disposedValue; // To detect redundant calls

        // IDisposable
        protected void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _Col = null;
                    _Ext = null;
                    if (_DefaultIcon is object)
                        _DefaultIcon.Dispose();
                    _DefaultImage = null;
                }
            }

            disposedValue = true;
        }

        public void Dispose()
        {
            // Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */

        private void OnPropertyChanged([CallerMemberName] string e = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(e));
        }
    }

    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    /// <summary>
    /// Compares type SystemFileType objects by extension
    /// </summary>
    public class SysFileTypeComp : IComparer<SystemFileType>
    {
        public int Compare(SystemFileType x, SystemFileType y)
        {
            return string.Compare(x.Extension, y.Extension);
        }
    }

    /// <summary>
    /// Compares two UIHandler objects by UIName
    /// </summary>
    public class UIHandlerComp : IComparer<UIHandler>
    {
        public int Compare(UIHandler x, UIHandler y)
        {
            return string.Compare(x.UIName, y.UIName);
        }
    }

    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    /// <summary>
    /// Reprents a list of all registered file types on the system, and their handlers.
    /// </summary>
    public sealed class AllSystemFileTypes : IDisposable, INotifyPropertyChanged
    {
        private System.Collections.ObjectModel.ObservableCollection<SystemFileType> _Col = new System.Collections.ObjectModel.ObservableCollection<SystemFileType>();
        private System.Collections.ObjectModel.ObservableCollection<UIHandler> _UICol = new System.Collections.ObjectModel.ObservableCollection<UIHandler>();
        private StandardIcons _IconSize = StandardIcons.Icon48;

        public event PopulatingEventHandler Populating;
        public event PropertyChangedEventHandler PropertyChanged;

        public delegate void PopulatingEventHandler(object sender, FileTypeEnumEventArgs e);

        /// <summary>
        /// Sets the uniform standard size for all icons and images in this object graph.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public StandardIcons IconSize
        {
            get
            {
                return _IconSize;
            }

            internal set
            {
                _IconSize = value;
                OnPropertyChanged("IconSize");
            }
        }

        /// <summary>
        /// Retrieves the collection of SystemFileType objects.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public System.Collections.ObjectModel.ObservableCollection<SystemFileType> FileTypes
        {
            get
            {
                return _Col;
            }
        }

        /// <summary>
        /// Retrieves the collection of UIHandler objects.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public System.Collections.ObjectModel.ObservableCollection<UIHandler> UICollection
        {
            get
            {
                return _UICol;
            }
        }

        /// <summary>
        /// Retrieves a UIHandler object base on the IAssocHandler from a cache or creates and returns a new one if it does not already exist.
        /// </summary>
        /// <param name="assoc">The IAssocHandler from which to build the new object.</param>
        /// <param name="ext">The Extension of the file type the IAssocHandler handles.</param>
        /// <returns>A new UIHandler object</returns>
        /// <remarks></remarks>
        internal UIHandler HandlerFromAssocHandler(IAssocHandler assoc, string ext)
        {
            UIHandler HandlerFromAssocHandlerRet = default;
            string exepath = null;
            assoc.GetName(out exepath);
            foreach (var u in _UICol)
            {
                if ((exepath ?? "") == (u.ExePath ?? ""))
                {
                    u.AddExt(ext);
                    return u;
                }
            }

            HandlerFromAssocHandlerRet = new UIHandler(assoc, this);
            HandlerFromAssocHandlerRet.AddExt(ext);
            _UICol.Add(HandlerFromAssocHandlerRet);
            return HandlerFromAssocHandlerRet;
        }

        /// <summary>
        /// Builds the system file type cache.
        /// </summary>
        /// <returns>The number of system file type entries enumerated.</returns>
        /// <remarks></remarks>
        public int Populate(bool fireEvent = true)
        {
            _Col.Clear();
            _UICol.Clear();
            var n = Registry.ClassesRoot.GetSubKeyNames();
            SystemFileType sf;
            int x = 0;
            int y;
            var sn2 = new List<string>();
            foreach (var sn in n)
            {
                if (sn.Substring(0, 1) == ".")
                {
                    sn2.Add(sn);
                }
            }

            y = sn2.Count;
            foreach (var sn in sn2)
            {
                sf = SystemFileType.FromExtension(sn, this);
                if (sf is object)
                {
                    _Col.Add(sf);
                    x += 1;
                    if (fireEvent)
                    {
                        // If fireEvent AndAlso x Mod 10 = 0 Then
                        Populating?.Invoke(this, new FileTypeEnumEventArgs(sf, x, y));
                        System.Windows.Forms.Application.DoEvents();
                    }
                }
            }

            var c = _Col.ToArray();
            Array.Sort(c, new SysFileTypeComp());
            _Col.Clear();
            foreach (var cu in c)
                _Col.Add(cu);
            var d = _UICol.ToArray();
            Array.Sort(d, new UIHandlerComp());
            _UICol.Clear();
            foreach (var du in d)
                _UICol.Add(du);
            OnPropertyChanged("UICollection");
            OnPropertyChanged("FileTypes");
            return x;
        }

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        private bool disposedValue; // To detect redundant calls

        protected void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _Col = null;
                    _UICol = null;
                }
            }

            disposedValue = true;
        }

        public void Dispose()
        {
            // Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void OnPropertyChanged([CallerMemberName] string e = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(e));
        }
    }

    public static class Assoc
    {

        /// <summary>
        /// Populates the Open With menu and returns a <see cref="NativeMenu"/> object.
        /// </summary>
        /// <param name="fileName">The file name whose menu to retrieve.</param>
        /// <param name="openWithCmd">The menu item id of the Open With menu item of the parent menu.</param>
        /// <param name="hMenu">The handle of the parent menu.</param>
        /// <returns></returns>
        public static NativeMenu GetOpenWithMenu(string fileName, IntPtr openWithCmd, IntPtr hMenu)
        {
            // Create a native context menu submenu populated with "open with" items
            var col = new MenuItemBagCollection();
            var nm = new NativeMenu(hMenu);
            string ext = Path.GetExtension(fileName).ToLower();
            NativeMenuItem nmi;
            var assoc = NativeShell.EnumFileHandlers(ext);
            nm.Items.Clear();
            if (assoc is null)
            {
                nm.Destroy();
                return null;
            }

            foreach (IAssocHandler handler in assoc)
            {
                Icon icn;
                string pth = null;
                int idx;
                string uiname = null;
                string pathname = null;
                uint sz = 0U;
                handler.GetIconLocation(out pth, out idx);
                int iix = (int)NativeShell.Shell_GetCachedImageIndex(pth, idx, 0U);
                icn = Resources.GetFileIconFromIndex(iix, (Resources.SystemIconSizes)(int)(PInvoke.SHIL_SMALL));
                handler.GetName(out pathname);
                if (File.Exists(pathname) == false)
                    continue;
                handler.GetUIName(out uiname);
                if (icn is null)
                {
                    nmi = nm.Items.Add(uiname);
                }
                else
                {
                    nmi = nm.Items.Add(uiname, icn);
                }

                col.Add(new MenuItemBag(nmi, handler));
            }

            nm.Items.Add(null);
            nmi = nm.Items.Add("&Choose default program...");
            nmi.Id = (int)openWithCmd;
            nm.Bag = col;
            return nm;
        }
    }
    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
}