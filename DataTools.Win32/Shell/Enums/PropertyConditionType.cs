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
    /// Specifies the condition type to use when displaying the property in the query builder user interface (I).
    /// </summary>
    public enum PropertyConditionType
    {
        /// <summary>
        /// The default condition type.
        /// </summary>
        None = 0,

        /// <summary>
        /// The string type.
        /// </summary>
        String = 1,

        /// <summary>
        /// The size type.
        /// </summary>
        Size = 2,

        /// <summary>
        /// The date/time type.
        /// </summary>
        DateTime = 3,

        /// <summary>
        /// The Boolean type.
        /// </summary>
        Boolean = 4,

        /// <summary>
        /// The number type.
        /// </summary>
        Number = 5
    }
}
