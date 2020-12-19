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
        
    }
}
