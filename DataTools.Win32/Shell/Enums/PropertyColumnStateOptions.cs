// ************************************************* ''
// DataTools C# Native Utility Library For Windows - Interop
//
// Module: NativeShell
//         Wrappers for native and COM shell interfaces.
//
// Some enum documentation copied from the MSDN (and in some cases, updated).
// Some classes and interfaces were ported from the WindowsAPICodePack.
// 
// Copyright (C) 2011-2020 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the Microsoft Public License   
// ************************************************* ''

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

//using DataTools.Hardware.MessageResources;
//using DataTools.Hardware;
//using DataTools.Hardware.Native;

namespace DataTools.Shell.Native
{
    /// <summary>
    /// Describes how a property should be treated for display purposes.
    /// </summary>
    [Flags]
    public enum PropertyColumnStateOptions
    {
        /// <summary>
        /// Default value
        /// </summary>
        None = 0x0,

        /// <summary>
        /// The value is displayed as a string.
        /// </summary>
        StringType = 0x1,

        /// <summary>
        /// The value is displayed as an integer.
        /// </summary>
        IntegerType = 0x2,

        /// <summary>
        /// The value is displayed as a date/time.
        /// </summary>
        DateType = 0x3,

        /// <summary>
        /// A mask for display type values StringType, IntegerType, and DateType.
        /// </summary>
        TypeMask = 0xF,

        /// <summary>
        /// The column should be on by default in Details view.
        /// </summary>
        OnByDefault = 0x10,

        /// <summary>
        /// Will be slow to compute. Perform on a background thread.
        /// </summary>
        Slow = 0x20,

        /// <summary>
        /// Provided by a handler, not the folder.
        /// </summary>
        Extended = 0x40,

        /// <summary>
        /// Not displayed in the context menu, but is listed in the More... dialog.
        /// </summary>
        SecondaryUI = 0x80,

        /// <summary>
        /// Not displayed in the user interface (I).
        /// </summary>
        Hidden = 0x100,

        /// <summary>
        /// VarCmp produces same result as IShellFolder::CompareIDs.
        /// </summary>
        PreferVariantCompare = 0x200,

        /// <summary>
        /// PSFormatForDisplay produces same result as IShellFolder::CompareIDs.
        /// </summary>
        PreferFormatForDisplay = 0x400,

        /// <summary>
        /// Do not sort folders separately.
        /// </summary>
        NoSortByFolders = 0x800,

        /// <summary>
        /// Only displayed in the UI.
        /// </summary>
        ViewOnly = 0x10000,

        /// <summary>
        /// Marks columns with values that should be read in a batch.
        /// </summary>
        BatchRead = 0x20000,

        /// <summary>
        /// Grouping is disabled for this column.
        /// </summary>
        NoGroupBy = 0x40000,

        /// <summary>
        /// Can't resize the column.
        /// </summary>
        FixedWidth = 0x1000,

        /// <summary>
        /// The width is the same in all dots per inch (dpi)s.
        /// </summary>
        NoDpiScale = 0x2000,

        /// <summary>
        /// Fixed width and height ratio.
        /// </summary>
        FixedRatio = 0x4000,

        /// <summary>
        /// Filters out new display flags.
        /// </summary>
        DisplayMask = 0xF000
    }
}
