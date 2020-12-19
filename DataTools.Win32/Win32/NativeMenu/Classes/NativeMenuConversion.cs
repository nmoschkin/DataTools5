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
// Licensed Under the MIT License   
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

using DataTools.Desktop;
using Microsoft.VisualBasic;
using DataTools.Win32.Memory;

namespace DataTools.Win32.Menu
{
    
    
    public static class NativeMenuConversion
    {
        public const int MenuTypeMask = 0x4764;

        public static IntPtr GetDefaultItem(IntPtr hMenu, bool retPos = false)
        {
            int i;
            int c = User32.GetMenuItemCount(hMenu);
            
            var mi = default(MENUITEMINFO);
            
            mi.cbSize = Marshal.SizeOf(mi);
            mi.fMask = User32.MIIM_STATE + User32.MIIM_ID;

            for (i = 0; i < c; i++)
            {
                User32.GetMenuItemInfo(hMenu, i, true, ref mi);
                if ((mi.fState & User32.MFS_DEFAULT) == User32.MFS_DEFAULT)
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
            var mi = default(MENUINFO);
            int c;
            int i;
            ToolStripMenuItem min = null;
            Thread.Sleep(100);
            if (destMenu is null)
            {
                destMenu = new MenuStrip();
            }

            mi.cbSize = Marshal.SizeOf(mi);
            mi.fMask = User32.MIM_MAXHEIGHT + User32.MIM_STYLE;
            User32.GetMenuInfo(hMenu, mi);
            User32.SetMenuInfo(destMenu.Handle, mi);
            c = User32.GetMenuItemCount(hMenu);

            for (i = 0; i < c; i++)
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
                User32.DestroyMenu(hMenu);
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
            var mi = default(MENUINFO);
            int c;
            int i;
            ToolStripMenuItem min = null;
            Thread.Sleep(100);
            if (destMenu is null)
            {
                destMenu = new ContextMenuStrip();
            }

            mi.cbSize = Marshal.SizeOf(mi);
            mi.fMask = User32.MIM_MAXHEIGHT + User32.MIM_STYLE;
            User32.GetMenuInfo(hMenu, mi);

            // SetMenuInfo(destMenu.Handle, mi)

            c = User32.GetMenuItemCount(hMenu);

            for (i = 0; i < c; i++)
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
                User32.DestroyMenu(hMenu);
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
            c = User32.GetMenuItemCount(hMenu);
            
            for (i = 0; i < c; i++)
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
            var mii = new MENUITEMINFO();
            var mm = new SafePtr();
            
            mii.cbSize = Marshal.SizeOf(mii);
            mii.cch = 0;
            mii.fMask = User32.MIIM_TYPE;
            
            User32.GetMenuItemInfo(hMenu, itemId, byPos, ref mii);
            
            mm.Length = (mii.cch + 1) * sizeof(char);
            
            mii.cch += 1;
            mii.dwTypeData = mm.handle;

            if (User32.GetMenuItemInfo(hMenu, itemId, byPos, ref mii) == 0)
            {
                int err = User32.GetLastError();

                mm.Length = 1026L;
                mm.ZeroMemory();

                User32.FormatMessage(0x1000U, IntPtr.Zero, (uint)err, 0U, mm.handle, 512U, IntPtr.Zero);

                mm.Dispose();

                return null;
            }

            if ((mii.fType & User32.MFT_SEPARATOR) == User32.MFT_SEPARATOR)
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
            var mii = new MENUITEMINFO();

            Bitmap bmp;

            var mm = new SafePtr();

            if (destItem is null)
            {
                destItem = new ToolStripMenuItem();
            }

            mii.cbSize = Marshal.SizeOf(mii);
            mii.cch = 0;
            mii.fMask = User32.MIIM_TYPE;

            User32.GetMenuItemInfo(hMenu, itemId, byPos, ref mii);

            mm.Length = (mii.cch + 1) * sizeof(char);
            mii.cch += 1;
            mii.dwTypeData = mm.handle;

            if (User32.GetMenuItemInfo(hMenu, itemId, byPos, ref mii) == 0)
            {
                int err = User32.GetLastError();

                mm.Length = 1026L;
                mm.ZeroMemory();

                User32.FormatMessage(0x1000U, IntPtr.Zero, (uint)err, 0U, mm.handle, 512U, IntPtr.Zero);

                mm.Dispose();

                return false;
            }

            if ((mii.fType & User32.MFT_SEPARATOR) == User32.MFT_SEPARATOR)
            {
                destItem.Text = "-";
            }
            else
            {
                destItem.Text = mm.ToString();
            }

            mm.Dispose();
            mii.fMask = User32.MIIM_BITMAP;
            User32.GetMenuItemInfo(hMenu, itemId, byPos, ref mii);
            if (mii.hbmpItem != IntPtr.Zero)
            {
                bmp = Image.FromHbitmap(mii.hbmpItem);
                destItem.Image = bmp;
            }

            mii.fMask = User32.MIIM_STATE;
            User32.GetMenuItemInfo(hMenu, itemId, byPos, ref mii);
            if ((mii.fState & User32.MFS_CHECKED) == User32.MFS_CHECKED)
            {
                destItem.CheckState = CheckState.Checked;
            }

            if ((mii.fState & User32.MFS_DISABLED) == User32.MFS_DISABLED)
            {
                destItem.Enabled = false;
            }

            if ((mii.fState & User32.MFS_DEFAULT) == User32.MFS_DEFAULT)
            {
                destItem.Font = new Font(destItem.Font.Name, destItem.Font.Size, FontStyle.Bold);
            }

            mii.fMask = User32.MIIM_SUBMENU;
            User32.GetMenuItemInfo(hMenu, itemId, byPos, ref mii);
            if (mii.hSubMenu != IntPtr.Zero)
            {
                return MenuItemsToManaged(mii.hSubMenu, destItem.DropDownItems);
            }

            return true;
        }
    }
}
