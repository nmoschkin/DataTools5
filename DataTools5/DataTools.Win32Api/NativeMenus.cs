// ************************************************* ''
// DataTools C# Native Utility Library For Windows - Interop
//
// Module: NativeMenu
//         Wrappers for the native Win32 API menu system
//
// Started in 2000 on Windows 98/ME (and then later 2000).
//
// Still kicking in 2014 on Windows 8.1!
// A whole bunch of pInvoke/Const/Declare/Struct and associated utility functions that have been collected over the years.

// Some enum documentation copied from the MSDN (and in some cases, updated).
// 
// Copyright (C) 2011-2020 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the Microsoft Public License   
// ************************************************* ''




// Some notes: The menu items are dynamic.  They are not statically maintained in any collection or structure.

// When you fetch an item object from the virtual collection, that object is only alive in your program for as long as you reference it.
// If the menu gets destroyed while you are still working with an item, it will fail.


using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using DataTools.Memory;
using DataTools.Desktop;
using Microsoft.VisualBasic;

namespace DataTools.Win32Api.Menu
{

    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    [Flags()]
    public enum MenuItemType
    {
        String = 0x0,
        Bitmap = 0x4,
        OwnerDraw = 0x100,
        MenuBarBreak = 0x20,
        MenuBreak = 0x40,
        Separator = 0x400,
        RightJustify = 0x4000,
        RadioGroup = 0x200
    }

    public class MenuItemBag
    {
        public IntPtr CmdId;
        public object Data;
        public NativeMenuItem Item;

        // More room for stuff
        public KeyValuePair<string, object> Misc = new KeyValuePair<string, object>();

        public MenuItemBag(IntPtr cmd, object data)
        {
            CmdId = cmd;
            Data = data;
        }

        public MenuItemBag(NativeMenuItem item, object data)
        {
            CmdId = (IntPtr)item.Id;
            Item = item;
            Data = data;
        }
    }

    public class MenuItemBagCollection : ICollection<MenuItemBag>
    {
        private List<MenuItemBag> mList = new List<MenuItemBag>();

        public MenuItemBagCollection()
        {
        }

        public MenuItemBag FindBag(IntPtr cmdId)
        {
            foreach (var b in this)
            {
                if (b.CmdId == cmdId)
                    return b;
            }

            return null;
        }

        public MenuItemBag this[int index]
        {
            get
            {
                if (mList is null || mList.Count == 0)
                    return null;
                return mList[index];
            }
        }

        public void Add(MenuItemBag item)
        {
            mList.Add(item);
        }

        public void Clear()
        {
            mList = new List<MenuItemBag>();
        }

        public bool Contains(MenuItemBag item)
        {
            if (mList is null || mList.Count == 0)
                return false;
            int c = mList.Count;
            for (int i = 0, loopTo = c; i <= loopTo; i++)
            {
                if (ReferenceEquals(mList[i], item))
                    return true;
            }

            return false;
        }

        public void CopyTo(MenuItemBag[] array, int arrayIndex)
        {
            if (mList is null || mList.Count == 0)
                return;
            mList.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get
            {
                if (mList is null || mList.Count == 0)
                    return 0;
                return mList.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public bool Remove(MenuItemBag item)
        {
            if (mList is null || mList.Count == 0)
                return false;
            int c = mList.Count;
            for (int i = 0, loopTo = c; i <= loopTo; i++)
            {
                if (ReferenceEquals(mList[i], item))
                {
                    mList.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        public IEnumerator<MenuItemBag> GetEnumerator()
        {
            return new MenuItemBagEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new MenuItemBagEnumerator(this);
        }

        public IEnumerator GetEnumerator1() => GetEnumerator();
    }

    public class MenuItemBagEnumerator : IEnumerator<MenuItemBag>
    {
        private MenuItemBagCollection subj;
        private int pos = -1;

        internal MenuItemBagEnumerator(MenuItemBagCollection subject)
        {
            subj = subject;
        }

        public MenuItemBag Current
        {
            get
            {
                return subj[pos];
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return subj[pos];
            }
        }

        public bool MoveNext()
        {
            pos += 1;
            return pos < subj.Count;
        }

        public void Reset()
        {
            pos = -1;
        }

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        private bool disposedValue; // To detect redundant calls

        // IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                subj = null;
            }

            disposedValue = true;
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

    public class NativeMenuItemCollection : IEnumerable<NativeMenuItem>
    {
        private IntPtr hMenu;


        /// <summary>
        /// Gets the.DangerousGetHandle of the owner menu.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public IntPtr Handle
        {
            get
            {
                return hMenu;
            }
        }

        public NativeMenuItem this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return new NativeMenuItem(hMenu, index);
            }
        }

        public NativeMenuItem FindById(int itemId)
        {
            NativeMenu subm;
            foreach (var ii in this)
            {
                if (ii.Id == itemId)
                {
                    return ii;
                }

                subm = ii.SubMenu;
                NativeMenuItem ix;

                if (subm is object)
                {
                    ix = subm.Items.FindById(itemId);
                    if (ix != null)
                        return ix;
                }
            }

            return null;
        }

        public int OffsetById(int itemId)
        {
            NativeMenu subm;
            int ui = 0;
            foreach (var ii in this)
            {
                if (ii.Id == itemId)
                {
                    return ui;
                }

                subm = ii.SubMenu;
                if (subm is object)
                {
                    var ix = subm.Items.FindById(itemId);
                    if (ix != null)
                        return ui;
                }

                ui += 1;
            }

            return -1;
        }

        public bool RemoveAt(int index)
        {
            if (index < 0 || index >= Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            return PInvoke.RemoveMenu(hMenu, index, (int)PInvoke.MF_BYPOSITION) != 0;
        }

        public bool Remove(int itemId)
        {
            return PInvoke.RemoveMenu(hMenu, itemId, (int)PInvoke.MF_BYCOMMAND) != 0;
        }

        public int Count
        {
            get
            {
                int i = PInvoke.GetMenuItemCount(hMenu);
                if (i <= 0)
                    return 0;
                return i;
            }
        }

        public NativeMenuItem Add(string text, byte[] data = null)
        {
            return Insert(Count, text, true, IntPtr.Zero);
        }

        public NativeMenuItem Add(string text, Bitmap bmp, byte[] data = null)
        {
            return Insert(Count, text, bmp, true);
        }

        public NativeMenuItem Add(string text, Icon icon, byte[] data = null)
        {
            return Insert(Count, text, icon);
        }

        public NativeMenuItem Insert(int insertAfter, string text, Bitmap bmp, bool fbyPos)
        {
            return Insert(insertAfter, text, bmp, fbyPos, IntPtr.Zero);
        }

        public NativeMenuItem Insert(int insertAfter, string text, Bitmap bmp, bool fbyPos, IntPtr data)
        {
            var mii = new PInvoke.MENUITEMINFO();
            MemPtr mm = new MemPtr();
            NativeMenuItem nmi = null;
            // If insertAfter = -1 Then insertAfter = 0

            mii.cbSize = Marshal.SizeOf(mii);

            // if the text is nothing or '-' we'll assume they want it to be a separator
            if (text is null || text == "-")
            {
                mii.dwTypeData = IntPtr.Zero;
                mii.fType = (int)PInvoke.MFT_MENUBREAK;
            }
            else
            {
                mm = (MemPtr)text;
                mii.cch = text.Length;
                mii.dwTypeData = mm.Handle;
                mii.fType = (int)PInvoke.MFT_STRING;
            }

            mii.fMask = PInvoke.MIIM_FTYPE | PInvoke.MIIM_STRING | PInvoke.MIIM_ID;
            mii.wID = insertAfter + 0x2000;
            if (bmp is object)
            {
                IntPtr argbitPtr = new IntPtr();
                mii.hbmpItem = BitmapTools.MakeDIBSection(bmp, ref argbitPtr);
                mii.fMask += PInvoke.MIIM_BITMAP;
            }

            if (PInvoke.InsertMenuItem(hMenu, insertAfter, fbyPos, ref mii) != 0)
            {
                nmi = new NativeMenuItem(hMenu, insertAfter + 0x2000, false);
                nmi.Data = data;
            }
            else
            {
                //Interaction.MsgBox(NativeErrorMethods.FormatLastError());
            }

            mm.Free();
            return nmi;
        }

        public NativeMenuItem Insert(int insertAfter, string text, bool fbyPos, IntPtr data)
        {
            return Insert(insertAfter, text, default, fbyPos, data);
        }

        public NativeMenuItem Insert(int insertAfter, string text, Icon icon, bool fbyPos = true)
        {
            return Insert(insertAfter, text, BitmapTools.IconToTransparentBitmap(icon), fbyPos, IntPtr.Zero);
        }

        public bool Clear()
        {
            try
            {
                int c = PInvoke.GetMenuItemCount(hMenu) - 1;
                for (int i = c; i >= 0; i -= 1)
                    PInvoke.DeleteMenu(hMenu, i, (int)PInvoke.MF_BYPOSITION);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public IEnumerator<NativeMenuItem> GetEnumerator()
        {
            return new NativeMenuItemEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new NativeMenuItemEnumerator(this);
        }

        // We don't want this collection to be created publicly
        internal NativeMenuItemCollection(IntPtr hMenu)
        {
            this.hMenu = hMenu;
        }
    }

    public class NativeMenuItemEnumerator : IEnumerator<NativeMenuItem>
    {
        private int pos = -1;
        private NativeMenuItemCollection subj;

        internal NativeMenuItemEnumerator(NativeMenuItemCollection subject)
        {
            subj = subject;
        }

        public NativeMenuItem Current
        {
            get
            {
                return subj[pos];
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return subj[pos];
            }
        }

        public bool MoveNext()
        {
            pos += 1;
            if (pos >= subj.Count)
                return false;
            return true;
        }

        public void Reset()
        {
            pos = -1;
        }

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        private bool disposedValue; // To detect redundant calls

        // IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    subj = null;
                }
            }

            disposedValue = true;
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

    public class NativeMenuItem
    {
        private IntPtr hMenu;
        private int itemId;
        private NativeMenuItemCollection mCol;

        internal NativeMenuItemCollection Col
        {
            get
            {
                return mCol;
            }

            set
            {
                mCol = value;
            }
        }

        public IntPtr Handle
        {
            get
            {
                return hMenu;
            }
        }

        public int Id
        {
            get
            {
                return itemId;
            }

            set
            {
                var mii = new PInvoke.MENUITEMINFO();
                mii.cbSize = Marshal.SizeOf(mii);
                mii.fMask = PInvoke.MIIM_ID;
                mii.wID = value;
                PInvoke.SetMenuItemInfo(hMenu, itemId, false, ref mii);
                itemId = value;
            }
        }

        public override string ToString()
        {
            return Text;
        }

        public NativeMenu SubMenu
        {
            get
            {
                var mii = new PInvoke.MENUITEMINFO();
                mii.cbSize = Marshal.SizeOf(mii);
                mii.fMask = PInvoke.MIIM_SUBMENU;
                PInvoke.GetMenuItemInfo(hMenu, itemId, false, ref mii);
                if (mii.hSubMenu == IntPtr.Zero)
                    return null;
                return new NativeMenu(mii.hSubMenu);
            }
        }

        public void AttachSubMenu(IntPtr hMenu)
        {
            var mii = new PInvoke.MENUITEMINFO();
            mii.cbSize = Marshal.SizeOf(mii);
            mii.fMask = PInvoke.MIIM_SUBMENU;
            PInvoke.GetMenuItemInfo(hMenu, itemId, false, ref mii);
            if (mii.hSubMenu != IntPtr.Zero)
                return;
            mii.hSubMenu = hMenu;
            PInvoke.SetMenuItemInfo(hMenu, itemId, false, ref mii);
        }

        public void CreateSubMenu()
        {
            var mii = new PInvoke.MENUITEMINFO();
            mii.cbSize = Marshal.SizeOf(mii);
            mii.fMask = PInvoke.MIIM_SUBMENU;
            PInvoke.GetMenuItemInfo(hMenu, itemId, false, ref mii);
            if (mii.hSubMenu != IntPtr.Zero)
                return;
            mii.hSubMenu = PInvoke.CreatePopupMenu();
            PInvoke.SetMenuItemInfo(hMenu, itemId, false, ref mii);
        }

        public void DestroySubMenu()
        {
            var mii = new PInvoke.MENUITEMINFO();
            mii.cbSize = Marshal.SizeOf(mii);
            mii.fMask = PInvoke.MIIM_SUBMENU;
            PInvoke.GetMenuItemInfo(hMenu, itemId, false, ref mii);
            if (mii.hSubMenu != IntPtr.Zero)
            {
                PInvoke.DestroyMenu(mii.hSubMenu);
            }

            PInvoke.DestroyMenu(mii.hSubMenu);
            mii.hSubMenu = IntPtr.Zero;
            PInvoke.SetMenuItemInfo(hMenu, itemId, false, ref mii);
        }

        public bool Default
        {
            get
            {
                var mii = new PInvoke.MENUITEMINFO();
                mii.cbSize = Marshal.SizeOf(mii);
                mii.fMask = PInvoke.MIIM_STATE;
                PInvoke.GetMenuItemInfo(hMenu, itemId, false, ref mii);
                return !((mii.fState & PInvoke.MFS_DEFAULT) == PInvoke.MFS_DEFAULT);
            }

            set
            {
                var mii = new PInvoke.MENUITEMINFO();
                mii.cbSize = Marshal.SizeOf(mii);
                mii.fMask = PInvoke.MIIM_STATE;
                PInvoke.GetMenuItemInfo(hMenu, itemId, false, ref mii);
                if (value == true)
                {
                    mii.fState = mii.fState & ~(int)PInvoke.MFS_DEFAULT;
                }
                else
                {
                    mii.fState = mii.fState | (int)PInvoke.MFS_DEFAULT;
                }

                PInvoke.SetMenuItemInfo(hMenu, itemId, false, ref mii);
            }
        }

        /// <summary>
        /// Gets or sets the item data, in bytes.
        /// It is assumed that the data in question will have a size descriptor preamble in memory of type Integer (32 bit signed ordinal).
        /// The preamble is not returned, and a size-containing preamble should not be set when the value is set to a byte array.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public IntPtr Data
        {
            get
            {
                var mii = new PInvoke.MENUITEMINFO();
                mii.cbSize = Marshal.SizeOf(mii);
                mii.fMask = PInvoke.MIIM_DATA;
                PInvoke.GetMenuItemInfo(hMenu, itemId, false, ref mii);
                return mii.dwItemData;
            }

            set
            {
                var mii = new PInvoke.MENUITEMINFO();
                mii.cbSize = Marshal.SizeOf(mii);
                mii.fMask = PInvoke.MIIM_DATA;
                mii.dwItemData = value;
                PInvoke.SetMenuItemInfo(hMenu, itemId, false, ref mii);
            }
        }

        public string Text
        {
            get
            {
                var mii = new PInvoke.MENUITEMINFO();
                mii.cbSize = Marshal.SizeOf(mii);
                string s;
                mii.fMask = PInvoke.MIIM_FTYPE;
                if (PInvoke.GetMenuItemInfo(hMenu, itemId, false, ref mii) == 0)
                    return "-";
                if ((mii.fType & PInvoke.MFT_SEPARATOR) == PInvoke.MFT_SEPARATOR)
                {
                    return "-";
                }

                var mm = new SafePtr();
                mii.fMask = PInvoke.MIIM_STRING;
                mii.cch = 0;
                PInvoke.GetMenuItemInfo(hMenu, itemId, false, ref mii);
                mm.Length = (mii.cch + 1) * sizeof(char);
                mii.cch += 1;
                mii.dwTypeData = mm.DangerousGetHandle();
                mii.fMask = PInvoke.MIIM_STRING;
                if (PInvoke.GetMenuItemInfo(hMenu, itemId, false, ref mii) == 0)
                {
                    int err = PInvoke.GetLastError();

                    mm.Length = 1026L;
                    mm.ZeroMemory();

                    PInvoke.FormatMessage(0x1000U, IntPtr.Zero, (uint)err, 0U, mm.DangerousGetHandle(), 512U, IntPtr.Zero);

                    // MsgBox("Error 0x" & err.ToString("X8") & ": " & mm.ToString)
                    s = mm.ToString();
                    mm.Dispose();

                    return s;
                }

                s = mm.ToString();
                mm.Dispose();
                return s;
            }
            
            set
            {
                var mii = new PInvoke.MENUITEMINFO();
                mii.cbSize = Marshal.SizeOf(mii);
                mii.fMask = PInvoke.MIIM_STRING;
                var mm = new SafePtr();
                mm = (SafePtr)value;
                mm.Length += sizeof(char);
                mii.cch = (int)mm.Length;
                mii.dwTypeData = mm.DangerousGetHandle();
                PInvoke.SetMenuItemInfo(hMenu, itemId, false, ref mii);
            }
        }

        /// <summary>
        /// Set the.DangerousGetHandle to the item bitmap, directly, without a GDI+ Bitmap object.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public IntPtr hBitmap
        {
            get
            {
                var mii = new PInvoke.MENUITEMINFO();
                mii.cbSize = Marshal.SizeOf(mii);
                mii.fMask = PInvoke.MIIM_BITMAP;
                PInvoke.GetMenuItemInfo(hMenu, itemId, false, ref mii);
                if (mii.hbmpItem == IntPtr.Zero)
                    return default;
                return mii.hbmpItem;
            }

            set
            {
                var mii = new PInvoke.MENUITEMINFO();
                mii.cbSize = Marshal.SizeOf(mii);
                mii.fMask = PInvoke.MIIM_BITMAP;
                PInvoke.GetMenuItemInfo(hMenu, itemId, false, ref mii);
                if (mii.hbmpItem != IntPtr.Zero)
                {
                    Shell.Native.NativeShell.DeleteObject(mii.hbmpItem);
                    mii.hbmpItem = IntPtr.Zero;
                }

                mii.hbmpItem = value;
                PInvoke.SetMenuItemInfo(hMenu, itemId, false, ref mii);
            }
        }

        /// <summary>
        /// Convert an icon into a bitmap and set it into the menu.
        /// </summary>
        /// <value></value>
        /// <remarks></remarks>
        public Icon Icon
        {
            set
            {
                Bitmap = BitmapTools.IconToTransparentBitmap(value);
            }
        }

        /// <summary>
        /// Dynamically get or set the bitmap for the item.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public Bitmap Bitmap
        {
            get
            {
                var mii = new PInvoke.MENUITEMINFO();
                mii.cbSize = Marshal.SizeOf(mii);
                mii.fMask = PInvoke.MIIM_BITMAP;
                PInvoke.GetMenuItemInfo(hMenu, itemId, false, ref mii);
                if (mii.hbmpItem == IntPtr.Zero)
                    return null;
                return Image.FromHbitmap(mii.hbmpItem);
            }

            set
            {
                if (value is null)
                {
                    hBitmap = IntPtr.Zero;
                    return;
                }

                var mii = new PInvoke.MENUITEMINFO();
                mii.cbSize = Marshal.SizeOf(mii);

                IntPtr argbitPtr = new IntPtr();

                mii.hbmpItem = BitmapTools.MakeDIBSection(value, ref argbitPtr);

                mii.fMask = PInvoke.MIIM_BITMAP;

                PInvoke.SetMenuItemInfo(hMenu, itemId, false, ref mii);
            }
        }

        public CheckState CheckState
        {
            get
            {
                var mii = new PInvoke.MENUITEMINFO();
                mii.cbSize = Marshal.SizeOf(mii);
                mii.fMask = PInvoke.MIIM_STATE;
                PInvoke.GetMenuItemInfo(hMenu, itemId, false, ref mii);
                if ((mii.fState & PInvoke.MFS_CHECKED) == PInvoke.MFS_CHECKED)
                {
                    return CheckState.Checked;
                }

                return CheckState.Unchecked;
            }

            set
            {
                var mii = new PInvoke.MENUITEMINFO();
                mii.cbSize = Marshal.SizeOf(mii);
                mii.fMask = PInvoke.MIIM_STATE;
                PInvoke.GetMenuItemInfo(hMenu, itemId, false, ref mii);
                mii.fState = mii.fState & (int)~PInvoke.MFS_CHECKED;
                switch (value)
                {
                    case CheckState.Unchecked:
                        {
                            mii.fState = (int)(mii.fState | PInvoke.MFS_CHECKED);
                            break;
                        }

                    case CheckState.Checked:
                    case CheckState.Indeterminate:
                        {
                            mii.fState = (int)(mii.fState | PInvoke.MFS_UNCHECKED);
                            break;
                        }
                }

                PInvoke.SetMenuItemInfo(hMenu, itemId, false, ref mii);
            }
        }

        public bool Checked
        {
            get
            {
                var mii = new PInvoke.MENUITEMINFO();
                mii.cbSize = Marshal.SizeOf(mii);
                return CheckState == CheckState.Checked;
            }

            set
            {
                if (value == true)
                {
                    CheckState = CheckState.Checked;
                }
                else
                {
                    CheckState = CheckState.Unchecked;
                }
            }
        }

        public bool OwnerDrawn
        {
            get
            {
                var mii = new PInvoke.MENUITEMINFO();
                mii.cbSize = Marshal.SizeOf(mii);
                mii.fMask = PInvoke.MIIM_FTYPE;
                PInvoke.GetMenuItemInfo(hMenu, itemId, false, ref mii);
                if ((mii.fType & PInvoke.MFT_OWNERDRAW) == PInvoke.MFT_OWNERDRAW)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            set
            {
                var mii = new PInvoke.MENUITEMINFO();
                mii.cbSize = Marshal.SizeOf(mii);
                mii.fMask = PInvoke.MIIM_FTYPE;
                PInvoke.GetMenuItemInfo(hMenu, itemId, false, ref mii);
                if (value)
                {
                    mii.fType = (int)(mii.fType | PInvoke.MFT_OWNERDRAW);
                }
                else
                {
                    mii.fType = (int)(mii.fType & ~PInvoke.MFT_OWNERDRAW);
                }

                PInvoke.SetMenuItemInfo(hMenu, itemId, false, ref mii);
            }
        }

        public MenuItemType ItemType
        {
            get
            {
                var mii = new PInvoke.MENUITEMINFO();
                mii.cbSize = Marshal.SizeOf(mii);
                mii.cbSize = Marshal.SizeOf(mii);
                mii.fMask = PInvoke.MIIM_FTYPE;
                PInvoke.GetMenuItemInfo(hMenu, itemId, false, ref mii);
                return (MenuItemType)(int)(mii.fType & NativeMenuConversion.MenuTypeMask);
            }

            set
            {
                var mii = new PInvoke.MENUITEMINFO();
                mii.cbSize = Marshal.SizeOf(mii);
                mii.fMask = PInvoke.MIIM_FTYPE;
                PInvoke.GetMenuItemInfo(hMenu, itemId, false, ref mii);
                mii.fType = mii.fType & ~NativeMenuConversion.MenuTypeMask;
                mii.fType = mii.fType | (int)value;
                PInvoke.SetMenuItemInfo(hMenu, itemId, false, ref mii);
            }
        }

        public bool Enabled
        {
            get
            {
                var mii = new PInvoke.MENUITEMINFO();
                mii.cbSize = Marshal.SizeOf(mii);
                mii.fMask = PInvoke.MIIM_STATE;
                PInvoke.GetMenuItemInfo(hMenu, itemId, false, ref mii);
                return !((mii.fState & PInvoke.MFS_DISABLED) == PInvoke.MFS_DISABLED);
            }

            set
            {
                var mii = new PInvoke.MENUITEMINFO();
                mii.cbSize = Marshal.SizeOf(mii);
                mii.fMask = PInvoke.MIIM_STATE;
                PInvoke.GetMenuItemInfo(hMenu, itemId, false, ref mii);
                mii.fState = (int)(mii.fState & ~PInvoke.MFS_DISABLED);
                if (value)
                {
                    mii.fState = (int)(mii.fState | PInvoke.MFS_DISABLED);
                }

                PInvoke.SetMenuItemInfo(hMenu, itemId, false, ref mii);
            }
        }

        /// <summary>
        /// Initialize the item to a pre-existing native menu item.
        /// Use NativeMenuItemCollection.Add to create a new item.
        /// </summary>
        /// <param name="hMenu"></param>
        /// <param name="itemId"></param>
        /// <param name="byPos"></param>
        /// <remarks></remarks>
        public NativeMenuItem(IntPtr hMenu, int itemId, bool byPos = true)
        {
            this.hMenu = hMenu;
            if (byPos == true)
            {
                var mii = new PInvoke.MENUITEMINFO();
                mii.cbSize = Marshal.SizeOf(mii);
                mii.fMask = PInvoke.MIIM_ID;
                PInvoke.GetMenuItemInfo(hMenu, itemId, true, ref mii);
                this.itemId = mii.wID;
            }
            else
            {
                this.itemId = itemId;
            }
        }
    }

    public class NativeMenu : IDisposable
    {
        private IntPtr hMenu = IntPtr.Zero;
        private NativeMenuItemCollection mCol;
        private MenuItemBagCollection mBag;

        public MenuItemBagCollection Bag
        {
            get
            {
                return mBag;
            }

            set
            {
                mBag = value;
            }
        }

        public IntPtr Handle
        {
            get
            {
                return hMenu;
            }
        }

        public NativeMenuItemCollection Items
        {
            get
            {
                return mCol;
            }
        }

        public void CreateHandle()
        {
            if (hMenu != IntPtr.Zero)
            {
                PInvoke.DestroyMenu(hMenu);
            }

            hMenu = PInvoke.CreateMenu();
        }

        public void Destroy()
        {
            foreach (var nmi in Items)
            {
                var sb = nmi.SubMenu;
                nmi.Bitmap = null;
                nmi.Data = default;
                if (sb is object)
                {
                    sb.Destroy();
                }
            }

            PInvoke.DestroyMenu(hMenu);
        }

        public NativeMenu(bool createHandle = true, bool isPopup = true)
        {
            if (createHandle)
            {
                if (isPopup)
                {
                    hMenu = PInvoke.CreatePopupMenu();
                }
                else
                {
                    hMenu = PInvoke.CreateMenu();
                }
            }

            mCol = new NativeMenuItemCollection(hMenu);
        }

        public NativeMenu(IntPtr hMenu)
        {
            this.hMenu = hMenu;
            mCol = new NativeMenuItemCollection(hMenu);
        }

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        private bool disposedValue; // To detect redundant calls

        // IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                // If disposing Then
                // End If

                if (hMenu != IntPtr.Zero)
                {
                    PInvoke.DestroyMenu(hMenu);
                    hMenu = IntPtr.Zero;
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
    }

    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    public static class NativeMenuConversion
    {
        public const int MenuTypeMask = 0x4764;

        public static IntPtr GetDefaultItem(IntPtr hMenu, bool retPos = false)
        {
            int i;
            int c = PInvoke.GetMenuItemCount(hMenu);
            var mi = default(PInvoke.MENUITEMINFO);
            mi.cbSize = Marshal.SizeOf(mi);
            mi.fMask = PInvoke.MIIM_STATE + PInvoke.MIIM_ID;
            var loopTo = c - 1;
            for (i = 0; i <= loopTo; i++)
            {
                PInvoke.GetMenuItemInfo(hMenu, i, true, ref mi);
                if ((mi.fState & PInvoke.MFS_DEFAULT) == PInvoke.MFS_DEFAULT)
                {
                    if (retPos)
                        return (IntPtr)i;
                    else
                        return (IntPtr)mi.wID;
                }
            }

            return IntPtr.Zero;
        }

        /// <summary>
        /// Copy a native hMenu and all its contents into a managed Menu object
        /// </summary>
        /// <param name="hMenu"></param>
        /// <param name="destMenu"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool MenuBarCopyToManaged(IntPtr hMenu, ref MenuStrip destMenu, bool destroyOrig = true)
        {
            var mi = default(PInvoke.MENUINFO);
            int c;
            int i;
            ToolStripMenuItem min = null;
            Thread.Sleep(100);
            if (destMenu is null)
            {
                destMenu = new MenuStrip();
            }

            mi.cbSize = Marshal.SizeOf(mi);
            mi.fMask = PInvoke.MIM_MAXHEIGHT + PInvoke.MIM_STYLE;
            PInvoke.GetMenuInfo(hMenu, mi);
            PInvoke.SetMenuInfo(destMenu.Handle, mi);
            c = PInvoke.GetMenuItemCount(hMenu);
            var loopTo = c - 1;
            for (i = 0; i <= loopTo; i++)
            {
                if (MenuItemCopyToManaged(hMenu, i, ref min) == false)
                    return false;
                min.Height = mi.cyMax;
                destMenu.Items.Add(min);
                min = null;
                Thread.Sleep(0);
            }

            if (destroyOrig)
            {
                PInvoke.DestroyMenu(hMenu);
            }

            return true;
        }


        /// <summary>
        /// Copy a native hMenu and all its contents into a managed Menu object
        /// </summary>
        /// <param name="hMenu"></param>
        /// <param name="destMenu"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool ContextMenuCopyToManaged(IntPtr hMenu, ref ContextMenuStrip destMenu, bool destroyOrig = true)
        {
            var mi = default(PInvoke.MENUINFO);
            int c;
            int i;
            ToolStripMenuItem min = null;
            Thread.Sleep(100);
            if (destMenu is null)
            {
                destMenu = new ContextMenuStrip();
            }

            mi.cbSize = Marshal.SizeOf(mi);
            mi.fMask = PInvoke.MIM_MAXHEIGHT + PInvoke.MIM_STYLE;
            PInvoke.GetMenuInfo(hMenu, mi);

            // SetMenuInfo(destMenu.Handle, mi)

            c = PInvoke.GetMenuItemCount(hMenu);
            var loopTo = c - 1;
            for (i = 0; i <= loopTo; i++)
            {
                if (MenuItemCopyToManaged(hMenu, i, ref min) == false)
                    return false;
                min.Height = mi.cyMax;
                destMenu.Items.Add(min);
                min = null;
                Thread.Sleep(0);
            }

            if (destroyOrig)
            {
                PInvoke.DestroyMenu(hMenu);
            }

            return true;
        }


        /// <summary>
        /// Copy a native hMenu and all its contents into a managed DropDownItemsCollection object
        /// </summary>
        /// <param name="hMenu"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool MenuItemsToManaged(IntPtr hMenu, ToolStripItemCollection items)
        {
            int c;
            int i;
            ToolStripMenuItem min = null;
            c = PInvoke.GetMenuItemCount(hMenu);
            var loopTo = c - 1;
            for (i = 0; i <= loopTo; i++)
            {
                if (MenuItemCopyToManaged(hMenu, i, ref min) == false)
                    return false;
                items.Add(min);
                min = null;
                Thread.Sleep(0);
            }

            return true;
        }

        public static string GetMenuItemText(IntPtr hMenu, int itemId, bool byPos = true)
        {
            var mii = new PInvoke.MENUITEMINFO();
            var mm = new SafePtr();
            
            mii.cbSize = Marshal.SizeOf(mii);
            mii.cch = 0;
            mii.fMask = PInvoke.MIIM_TYPE;
            
            PInvoke.GetMenuItemInfo(hMenu, itemId, byPos, ref mii);
            
            mm.Length = (mii.cch + 1) * sizeof(char);
            
            mii.cch += 1;
            mii.dwTypeData = mm.handle;

            if (PInvoke.GetMenuItemInfo(hMenu, itemId, byPos, ref mii) == 0)
            {
                int err = PInvoke.GetLastError();

                mm.Length = 1026L;
                mm.ZeroMemory();

                PInvoke.FormatMessage(0x1000U, IntPtr.Zero, (uint)err, 0U, mm.handle, 512U, IntPtr.Zero);

                mm.Dispose();

                return null;
            }

            if ((mii.fType & PInvoke.MFT_SEPARATOR) == PInvoke.MFT_SEPARATOR)
            {
                return "-";
            }
            else
            {
                string s;
                s = mm.ToString();
                mm.Dispose();
                return s;
            }
        }

        /// <summary>
        /// Copy a native hMenu and all its contents into a managed Menu object
        /// </summary>
        /// <param name="hMenu"></param>
        /// <param name="destItem"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool MenuItemCopyToManaged(IntPtr hMenu, int itemId, ref ToolStripMenuItem destItem, bool byPos = true)
        {
            var mii = new PInvoke.MENUITEMINFO();

            Bitmap bmp;

            var mm = new SafePtr();

            if (destItem is null)
            {
                destItem = new ToolStripMenuItem();
            }

            mii.cbSize = Marshal.SizeOf(mii);
            mii.cch = 0;
            mii.fMask = PInvoke.MIIM_TYPE;

            PInvoke.GetMenuItemInfo(hMenu, itemId, byPos, ref mii);

            mm.Length = (mii.cch + 1) * sizeof(char);
            mii.cch += 1;
            mii.dwTypeData = mm.handle;

            if (PInvoke.GetMenuItemInfo(hMenu, itemId, byPos, ref mii) == 0)
            {
                int err = PInvoke.GetLastError();

                mm.Length = 1026L;
                mm.ZeroMemory();

                PInvoke.FormatMessage(0x1000U, IntPtr.Zero, (uint)err, 0U, mm.handle, 512U, IntPtr.Zero);

                mm.Dispose();

                return false;
            }

            if ((mii.fType & PInvoke.MFT_SEPARATOR) == PInvoke.MFT_SEPARATOR)
            {
                destItem.Text = "-";
            }
            else
            {
                destItem.Text = mm.ToString();
            }

            mm.Dispose();
            mii.fMask = PInvoke.MIIM_BITMAP;
            PInvoke.GetMenuItemInfo(hMenu, itemId, byPos, ref mii);
            if (mii.hbmpItem != IntPtr.Zero)
            {
                bmp = Image.FromHbitmap(mii.hbmpItem);
                destItem.Image = bmp;
            }

            mii.fMask = PInvoke.MIIM_STATE;
            PInvoke.GetMenuItemInfo(hMenu, itemId, byPos, ref mii);
            if ((mii.fState & PInvoke.MFS_CHECKED) == PInvoke.MFS_CHECKED)
            {
                destItem.CheckState = CheckState.Checked;
            }

            if ((mii.fState & PInvoke.MFS_DISABLED) == PInvoke.MFS_DISABLED)
            {
                destItem.Enabled = false;
            }

            if ((mii.fState & PInvoke.MFS_DEFAULT) == PInvoke.MFS_DEFAULT)
            {
                destItem.Font = new Font(destItem.Font.Name, destItem.Font.Size, FontStyle.Bold);
            }

            mii.fMask = PInvoke.MIIM_SUBMENU;
            PInvoke.GetMenuItemInfo(hMenu, itemId, byPos, ref mii);
            if (mii.hSubMenu != IntPtr.Zero)
            {
                return MenuItemsToManaged(mii.hSubMenu, destItem.DropDownItems);
            }

            return true;
        }
    }

    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
}