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
    [Flags]
    public enum ShellFolderEnumerationOptions : ushort
    {
        CheckingForChildren = 0x10,
        Folders = 0x20,
        NonFolders = 0x40,
        IncludeHidden = 0x80,
        InitializeOnFirstNext = 0x100,
        NetPrinterSearch = 0x200,
        Shareable = 0x400,
        Storage = 0x800,
        NavigationEnum = 0x1000,
        FastItems = 0x2000,
        FlatList = 0x4000,
        EnableAsync = 0x8000
    }
}
