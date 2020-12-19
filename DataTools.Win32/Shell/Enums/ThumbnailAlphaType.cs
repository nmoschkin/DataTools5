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
    /// Thumbnail Alpha Types
    /// </summary>
    public enum ThumbnailAlphaType
    {
        /// <summary>
        /// Let the system decide.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// No transparency
        /// </summary>
        NoAlphaChannel = 1,

        /// <summary>
        /// Has transparency
        /// </summary>
        HasAlphaChannel = 2
    }
}
