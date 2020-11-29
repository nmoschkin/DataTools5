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
    /// Property Aggregation Type
    /// </summary>
    public enum PropertyAggregationType
    {
        /// <summary>
        /// The string "Multiple Values" is displayed.
        /// </summary>
        Default = 0,

        /// <summary>
        /// The first value in the selection is displayed.
        /// </summary>
        First = 1,

        /// <summary>
        /// The sum of the selected values is displayed. This flag is never returned
        /// for data types VT_LPWSTR, VT_BOOL, and VT_FILETIME.
        /// </summary>
        Sum = 2,

        /// <summary>
        /// The numerical average of the selected values is displayed. This flag
        /// is never returned for data types VT_LPWSTR, VT_BOOL, and VT_FILETIME.
        /// </summary>
        Average = 3,

        /// <summary>
        /// The date range of the selected values is displayed. This flag is only
        /// returned for values of the VT_FILETIME data type.
        /// </summary>
        DateRange = 4,

        /// <summary>
        /// A concatenated string of all the values is displayed. The order of
        /// individual values in the string is undefined. The concatenated
        /// string omits duplicate values; if a value occurs more than once,
        /// it only appears a single time in the concatenated string.
        /// </summary>
        Union = 5,

        /// <summary>
        /// The highest of the selected values is displayed.
        /// </summary>
        Max = 6,

        /// <summary>
        /// The lowest of the selected values is displayed.
        /// </summary>
        Min = 7
    }
}
