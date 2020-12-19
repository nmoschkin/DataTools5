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

namespace DataTools.Win32.Menu
{
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
            for (int i = 0; i < c; i++)
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
            for (int i = 0; i < c; i++)
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
}
