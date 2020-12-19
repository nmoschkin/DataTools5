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
}
