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
    /// Set of flags to be used with SearchConditionFactory.
    /// </summary>
    public enum SearchConditionType
    {
        /// <summary>
        /// Indicates that the values of the subterms are combined by "AND".
        /// </summary>
        And = 0,

        /// <summary>
        /// Indicates that the values of the subterms are combined by "OR".
        /// </summary>
        Or = 1,

        /// <summary>
        /// Indicates a "NOT" comparison of subterms.
        /// </summary>
        Not = 2,

        /// <summary>
        /// Indicates that the node is a comparison between a property and a
        /// constant value using a SearchConditionOperation.
        /// </summary>
        Leaf = 3
    }
}
