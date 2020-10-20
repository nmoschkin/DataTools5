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
    [Flags]
    public enum FileOpenOptions
    {
        OverwritePrompt = 0x2,
        StrictFileTypes = 0x4,
        NoChangeDirectory = 0x8,
        PickFolders = 0x20,
        // Ensure that items returned are filesystem items.
        ForceFilesystem = 0x40,
        // Allow choosing items that have no storage.
        AllNonStorageItems = 0x80,
        NoValidate = 0x100,
        AllowMultiSelect = 0x200,
        PathMustExist = 0x800,
        FileMustExist = 0x1000,
        CreatePrompt = 0x2000,
        ShareAware = 0x4000,
        NoReadOnlyReturn = 0x8000,
        NoTestFileCreate = 0x10000,
        HideMruPlaces = 0x20000,
        HidePinnedPlaces = 0x40000,
        NoDereferenceLinks = 0x100000,
        DontAddToRecent = 0x2000000,
        ForceShowHidden = 0x10000000,
        DefaultNoMiniMode = 0x20000000
    }
}
