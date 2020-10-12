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
    public enum ShellItemDesignNameOptions : uint
    {
        Normal = 0x0,
        // SIGDN_NORMAL
        ParentRelativeParsing = 0x80018001,
        // SIGDN_INFOLDER | SIGDN_FORPARSING
        DesktopAbsoluteParsing = 0x80028000,
        // SIGDN_FORPARSING
        ParentRelativeEditing = 0x80031001,
        // SIGDN_INFOLDER | SIGDN_FOREDITING
        DesktopAbsoluteEditing = 0x8004C000,
        // SIGDN_FORPARSING | SIGDN_FORADDRESSBAR
        FileSystemPath = 0x80058000,
        // SIGDN_FORPARSING
        Url = 0x80068000,
        // SIGDN_FORPARSING
        ParentRelativeForAddressBar = 0x8007C001,
        // SIGDN_INFOLDER | SIGDN_FORPARSING | SIGDN_FORADDRESSBAR
        ParentRelative = 0x80080001
        // SIGDN_INFOLDER
    }
}
