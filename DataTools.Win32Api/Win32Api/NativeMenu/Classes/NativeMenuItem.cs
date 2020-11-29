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
                var mii = new MENUITEMINFO();
                mii.cbSize = Marshal.SizeOf(mii);
                mii.fMask = User32.MIIM_ID;
                mii.wID = value;
                User32.SetMenuItemInfo(hMenu, itemId, false, ref mii);
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
                var mii = new MENUITEMINFO();
                mii.cbSize = Marshal.SizeOf(mii);
                mii.fMask = User32.MIIM_SUBMENU;
                User32.GetMenuItemInfo(hMenu, itemId, false, ref mii);
                if (mii.hSubMenu == IntPtr.Zero)
                    return null;
                return new NativeMenu(mii.hSubMenu);
            }
        }

        public void AttachSubMenu(IntPtr hMenu)
        {
            var mii = new MENUITEMINFO();
            mii.cbSize = Marshal.SizeOf(mii);
            mii.fMask = User32.MIIM_SUBMENU;
            User32.GetMenuItemInfo(hMenu, itemId, false, ref mii);
            if (mii.hSubMenu != IntPtr.Zero)
                return;
            mii.hSubMenu = hMenu;
            User32.SetMenuItemInfo(hMenu, itemId, false, ref mii);
        }

        public void CreateSubMenu()
        {
            var mii = new MENUITEMINFO();
            mii.cbSize = Marshal.SizeOf(mii);
            mii.fMask = User32.MIIM_SUBMENU;
            User32.GetMenuItemInfo(hMenu, itemId, false, ref mii);
            if (mii.hSubMenu != IntPtr.Zero)
                return;
            mii.hSubMenu = User32.CreatePopupMenu();
            User32.SetMenuItemInfo(hMenu, itemId, false, ref mii);
        }

        public void DestroySubMenu()
        {
            var mii = new MENUITEMINFO();
            mii.cbSize = Marshal.SizeOf(mii);
            mii.fMask = User32.MIIM_SUBMENU;
            User32.GetMenuItemInfo(hMenu, itemId, false, ref mii);
            if (mii.hSubMenu != IntPtr.Zero)
            {
                User32.DestroyMenu(mii.hSubMenu);
            }

            User32.DestroyMenu(mii.hSubMenu);
            mii.hSubMenu = IntPtr.Zero;
            User32.SetMenuItemInfo(hMenu, itemId, false, ref mii);
        }

        public bool Default
        {
            get
            {
                var mii = new MENUITEMINFO();
                mii.cbSize = Marshal.SizeOf(mii);
                mii.fMask = User32.MIIM_STATE;
                User32.GetMenuItemInfo(hMenu, itemId, false, ref mii);
                return !((mii.fState & User32.MFS_DEFAULT) == User32.MFS_DEFAULT);
            }

            set
            {
                var mii = new MENUITEMINFO();
                mii.cbSize = Marshal.SizeOf(mii);
                mii.fMask = User32.MIIM_STATE;
                User32.GetMenuItemInfo(hMenu, itemId, false, ref mii);
                if (value == true)
                {
                    mii.fState = mii.fState & ~(int)User32.MFS_DEFAULT;
                }
                else
                {
                    mii.fState = mii.fState | (int)User32.MFS_DEFAULT;
                }

                User32.SetMenuItemInfo(hMenu, itemId, false, ref mii);
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
                var mii = new MENUITEMINFO();
                mii.cbSize = Marshal.SizeOf(mii);
                mii.fMask = User32.MIIM_DATA;
                User32.GetMenuItemInfo(hMenu, itemId, false, ref mii);
                return mii.dwItemData;
            }

            set
            {
                var mii = new MENUITEMINFO();
                mii.cbSize = Marshal.SizeOf(mii);
                mii.fMask = User32.MIIM_DATA;
                mii.dwItemData = value;
                User32.SetMenuItemInfo(hMenu, itemId, false, ref mii);
            }
        }

        public string Text
        {
            get
            {
                var mii = new MENUITEMINFO();
                mii.cbSize = Marshal.SizeOf(mii);
                string s;
                mii.fMask = User32.MIIM_FTYPE;
                if (User32.GetMenuItemInfo(hMenu, itemId, false, ref mii) == 0)
                    return "-";
                if ((mii.fType & User32.MFT_SEPARATOR) == User32.MFT_SEPARATOR)
                {
                    return "-";
                }

                var mm = new SafePtr();
                mii.fMask = User32.MIIM_STRING;
                mii.cch = 0;
                User32.GetMenuItemInfo(hMenu, itemId, false, ref mii);
                mm.Length = (mii.cch + 1) * sizeof(char);
                mii.cch += 1;
                mii.dwTypeData = mm.DangerousGetHandle();
                mii.fMask = User32.MIIM_STRING;
                if (User32.GetMenuItemInfo(hMenu, itemId, false, ref mii) == 0)
                {
                    int err = User32.GetLastError();

                    mm.Length = 1026L;
                    mm.ZeroMemory();

                    User32.FormatMessage(0x1000U, IntPtr.Zero, (uint)err, 0U, mm.DangerousGetHandle(), 512U, IntPtr.Zero);

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
                var mii = new MENUITEMINFO();
                mii.cbSize = Marshal.SizeOf(mii);
                mii.fMask = User32.MIIM_STRING;
                var mm = new SafePtr();
                mm = (SafePtr)value;
                mm.Length += sizeof(char);
                mii.cch = (int)mm.Length;
                mii.dwTypeData = mm.DangerousGetHandle();
                User32.SetMenuItemInfo(hMenu, itemId, false, ref mii);
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
                var mii = new MENUITEMINFO();
                mii.cbSize = Marshal.SizeOf(mii);
                mii.fMask = User32.MIIM_BITMAP;
                User32.GetMenuItemInfo(hMenu, itemId, false, ref mii);
                if (mii.hbmpItem == IntPtr.Zero)
                    return default;
                return mii.hbmpItem;
            }

            set
            {
                var mii = new MENUITEMINFO();
                mii.cbSize = Marshal.SizeOf(mii);
                mii.fMask = User32.MIIM_BITMAP;
                User32.GetMenuItemInfo(hMenu, itemId, false, ref mii);
                if (mii.hbmpItem != IntPtr.Zero)
                {
                    Shell.Native.NativeShell.DeleteObject(mii.hbmpItem);
                    mii.hbmpItem = IntPtr.Zero;
                }

                mii.hbmpItem = value;
                User32.SetMenuItemInfo(hMenu, itemId, false, ref mii);
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
                var mii = new MENUITEMINFO();
                mii.cbSize = Marshal.SizeOf(mii);
                mii.fMask = User32.MIIM_BITMAP;
                User32.GetMenuItemInfo(hMenu, itemId, false, ref mii);
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

                var mii = new MENUITEMINFO();
                mii.cbSize = Marshal.SizeOf(mii);

                IntPtr argbitPtr = new IntPtr();

                mii.hbmpItem = BitmapTools.MakeDIBSection(value, ref argbitPtr);

                mii.fMask = User32.MIIM_BITMAP;

                User32.SetMenuItemInfo(hMenu, itemId, false, ref mii);
            }
        }

        public CheckState CheckState
        {
            get
            {
                var mii = new MENUITEMINFO();
                mii.cbSize = Marshal.SizeOf(mii);
                mii.fMask = User32.MIIM_STATE;
                User32.GetMenuItemInfo(hMenu, itemId, false, ref mii);
                if ((mii.fState & User32.MFS_CHECKED) == User32.MFS_CHECKED)
                {
                    return CheckState.Checked;
                }

                return CheckState.Unchecked;
            }

            set
            {
                var mii = new MENUITEMINFO();
                mii.cbSize = Marshal.SizeOf(mii);
                mii.fMask = User32.MIIM_STATE;
                User32.GetMenuItemInfo(hMenu, itemId, false, ref mii);
                mii.fState = mii.fState & (int)~User32.MFS_CHECKED;
                switch (value)
                {
                    case CheckState.Unchecked:
                        {
                            mii.fState = (int)(mii.fState | User32.MFS_CHECKED);
                            break;
                        }

                    case CheckState.Checked:
                    case CheckState.Indeterminate:
                        {
                            mii.fState = (int)(mii.fState | User32.MFS_UNCHECKED);
                            break;
                        }
                }

                User32.SetMenuItemInfo(hMenu, itemId, false, ref mii);
            }
        }

        public bool Checked
        {
            get
            {
                var mii = new MENUITEMINFO();
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
                var mii = new MENUITEMINFO();
                mii.cbSize = Marshal.SizeOf(mii);
                mii.fMask = User32.MIIM_FTYPE;
                User32.GetMenuItemInfo(hMenu, itemId, false, ref mii);
                if ((mii.fType & User32.MFT_OWNERDRAW) == User32.MFT_OWNERDRAW)
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
                var mii = new MENUITEMINFO();
                mii.cbSize = Marshal.SizeOf(mii);
                mii.fMask = User32.MIIM_FTYPE;
                User32.GetMenuItemInfo(hMenu, itemId, false, ref mii);
                if (value)
                {
                    mii.fType = (int)(mii.fType | User32.MFT_OWNERDRAW);
                }
                else
                {
                    mii.fType = (int)(mii.fType & ~User32.MFT_OWNERDRAW);
                }

                User32.SetMenuItemInfo(hMenu, itemId, false, ref mii);
            }
        }

        public MenuItemType ItemType
        {
            get
            {
                var mii = new MENUITEMINFO();
                mii.cbSize = Marshal.SizeOf(mii);
                mii.cbSize = Marshal.SizeOf(mii);
                mii.fMask = User32.MIIM_FTYPE;
                User32.GetMenuItemInfo(hMenu, itemId, false, ref mii);
                return (MenuItemType)(int)(mii.fType & NativeMenuConversion.MenuTypeMask);
            }

            set
            {
                var mii = new MENUITEMINFO();
                mii.cbSize = Marshal.SizeOf(mii);
                mii.fMask = User32.MIIM_FTYPE;
                User32.GetMenuItemInfo(hMenu, itemId, false, ref mii);
                mii.fType = mii.fType & ~NativeMenuConversion.MenuTypeMask;
                mii.fType = mii.fType | (int)value;
                User32.SetMenuItemInfo(hMenu, itemId, false, ref mii);
            }
        }

        public bool Enabled
        {
            get
            {
                var mii = new MENUITEMINFO();
                mii.cbSize = Marshal.SizeOf(mii);
                mii.fMask = User32.MIIM_STATE;
                User32.GetMenuItemInfo(hMenu, itemId, false, ref mii);
                return !((mii.fState & User32.MFS_DISABLED) == User32.MFS_DISABLED);
            }

            set
            {
                var mii = new MENUITEMINFO();
                mii.cbSize = Marshal.SizeOf(mii);
                mii.fMask = User32.MIIM_STATE;
                User32.GetMenuItemInfo(hMenu, itemId, false, ref mii);
                mii.fState = (int)(mii.fState & ~User32.MFS_DISABLED);
                if (value)
                {
                    mii.fState = (int)(mii.fState | User32.MFS_DISABLED);
                }

                User32.SetMenuItemInfo(hMenu, itemId, false, ref mii);
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
                var mii = new MENUITEMINFO();
                mii.cbSize = Marshal.SizeOf(mii);
                mii.fMask = User32.MIIM_ID;
                User32.GetMenuItemInfo(hMenu, itemId, true, ref mii);
                this.itemId = mii.wID;
            }
            else
            {
                this.itemId = itemId;
            }
        }
    }
}
