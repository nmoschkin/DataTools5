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
// Licensed Under the MIT License   
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
    /// Specifies the property description grouping ranges.
    /// </summary>
    public enum PropertyGroupingRange
    {
        /// <summary>
        /// The individual values.
        /// </summary>
        Discrete = 0,

        /// <summary>
        /// The static alphanumeric ranges.
        /// </summary>
        Alphanumeric = 1,

        /// <summary>
        /// The static size ranges.
        /// </summary>
        Size = 2,

        /// <summary>
        /// The dynamically-created ranges.
        /// </summary>
        Dynamic = 3,

        /// <summary>
        /// The month and year groups.
        /// </summary>
        Date = 4,

        /// <summary>
        /// The percent groups.
        /// </summary>
        Percent = 5,

        /// <summary>
        /// The enumerated groups.
        /// </summary>
        Enumerated = 6
    }
}
