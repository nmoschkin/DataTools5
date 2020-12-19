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
}
