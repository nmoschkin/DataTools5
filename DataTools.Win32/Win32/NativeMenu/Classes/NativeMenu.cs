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

namespace DataTools.Win32.Menu
{
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
                User32.DestroyMenu(hMenu);
            }

            hMenu = User32.CreateMenu();
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

            User32.DestroyMenu(hMenu);
        }

        public NativeMenu(bool createHandle = true, bool isPopup = true)
        {
            if (createHandle)
            {
                if (isPopup)
                {
                    hMenu = User32.CreatePopupMenu();
                }
                else
                {
                    hMenu = User32.CreateMenu();
                }
            }

            mCol = new NativeMenuItemCollection(hMenu);
        }

        public NativeMenu(IntPtr hMenu)
        {
            this.hMenu = hMenu;
            mCol = new NativeMenuItemCollection(hMenu);
        }

        
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
                    User32.DestroyMenu(hMenu);
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
        
    }
}
