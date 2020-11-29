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

            return User32.RemoveMenu(hMenu, index, (int)User32.MF_BYPOSITION) != 0;
        }

        public bool Remove(int itemId)
        {
            return User32.RemoveMenu(hMenu, itemId, (int)User32.MF_BYCOMMAND) != 0;
        }

        public int Count
        {
            get
            {
                int i = User32.GetMenuItemCount(hMenu);
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
            var mii = new MENUITEMINFO();
            MemPtr mm = new MemPtr();
            NativeMenuItem nmi = null;
            // If insertAfter = -1 Then insertAfter = 0

            mii.cbSize = Marshal.SizeOf(mii);

            // if the text is nothing or '-' we'll assume they want it to be a separator
            if (text is null || text == "-")
            {
                mii.dwTypeData = IntPtr.Zero;
                mii.fType = (int)User32.MFT_MENUBREAK;
            }
            else
            {
                mm = (MemPtr)text;
                mii.cch = text.Length;
                mii.dwTypeData = mm.Handle;
                mii.fType = (int)User32.MFT_STRING;
            }

            mii.fMask = User32.MIIM_FTYPE | User32.MIIM_STRING | User32.MIIM_ID;
            mii.wID = insertAfter + 0x2000;
            if (bmp is object)
            {
                IntPtr argbitPtr = new IntPtr();
                mii.hbmpItem = BitmapTools.MakeDIBSection(bmp, ref argbitPtr);
                mii.fMask += User32.MIIM_BITMAP;
            }

            if (User32.InsertMenuItem(hMenu, insertAfter, fbyPos, ref mii) != 0)
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
                int c = User32.GetMenuItemCount(hMenu) - 1;
                for (int i = c; i >= 0; i -= 1)
                    User32.DeleteMenu(hMenu, i, (int)User32.MF_BYPOSITION);
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
}
