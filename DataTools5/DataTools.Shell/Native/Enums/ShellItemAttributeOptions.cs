// ' ************************************************* ''
// ' DataTools Visual Basic Utility Library - Interop
// '
// ' Module: NativeShell
// '         Wrappers for native and COM shell interfaces.
// '
// ' Some enum documentation copied from the MSDN (and in some cases, updated).
// ' Some classes and interfaces were ported from the WindowsAPICodePack.
// ' 
// ' Copyright (C) 2011-2020 Nathan Moschkin
// ' All Rights Reserved
// '
// ' Licensed Under the Microsoft Public License   
// ' ************************************************* ''

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
    public enum ShellItemAttributeOptions
    {
        // if multiple items and the attirbutes together.
        And = 0x1,
        // if multiple items or the attributes together.
        Or = 0x2,
        // Call GetAttributes directly on the 
        // ShellFolder for multiple attributes.
        AppCompat = 0x3,

        // A mask for SIATTRIBFLAGS_AND, SIATTRIBFLAGS_OR, and SIATTRIBFLAGS_APPCOMPAT. Callers normally do not use this value.
        Mask = 0x3,

        // Windows 7 and later. Examine all items in the array to compute the attributes. 
        // Note that this can result in poor performance over large arrays and therefore it 
        // should be used only when needed. Cases in which you pass this flag should be extremely rare.
        AllItems = 0x4000
    }
}
