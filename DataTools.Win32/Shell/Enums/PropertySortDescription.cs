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
    /// Describes the particular wordings of sort offerings.
    /// </summary>
    /// <remarks>
    /// Note that the strings shown are English versions only;
    /// localized strings are used for other locales.
    /// </remarks>
    public enum PropertySortDescription
    {
        /// <summary>
        /// The default ascending or descending property sort, "Sort going up", "Sort going down".
        /// </summary>
        General,

        /// <summary>
        /// The alphabetical sort, "A on top", "Z on top".
        /// </summary>
        AToZ,

        /// <summary>
        /// The numerical sort, "Lowest on top", "Highest on top".
        /// </summary>
        LowestToHighest,

        /// <summary>
        /// The size sort, "Smallest on top", "Largest on top".
        /// </summary>
        SmallestToBiggest,

        /// <summary>
        /// The chronological sort, "Oldest on top", "Newest on top".
        /// </summary>
        OldestToNewest
    }
}
