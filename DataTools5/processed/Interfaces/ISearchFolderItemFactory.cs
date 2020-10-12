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
    [ComImport,
    Guid(ShellIIDGuid.ISearchFolderItemFactory),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface ISearchFolderItemFactory
    {
        [PreserveSig]
        HResult SetDisplayName([In, MarshalAs(UnmanagedType.LPWStr)] string pszDisplayName);

        [PreserveSig]
        HResult SetFolderTypeID([In] Guid ftid);

        [PreserveSig]
        HResult SetFolderLogicalViewMode([In] FolderLogicalViewMode flvm);

        [PreserveSig]
        HResult SetIconSize([In] int iIconSize);

        [PreserveSig]
        HResult SetVisibleColumns([In] uint cVisibleColumns, [In, MarshalAs(UnmanagedType.LPArray)] PropertyKey[] rgKey);

        [PreserveSig]
        HResult SetSortColumns([In] uint cSortColumns, [In, MarshalAs(UnmanagedType.LPArray)] SortColumn[] rgSortColumns);

        [PreserveSig]
        HResult SetGroupColumn([In] ref PropertyKey keyGroup);

        [PreserveSig]
        HResult SetStacks([In] uint cStackKeys, [In, MarshalAs(UnmanagedType.LPArray)] PropertyKey[] rgStackKeys);

        [PreserveSig]
        HResult SetScope([In, MarshalAs(UnmanagedType.Interface)] IShellItemArray ppv);

        [PreserveSig]
        HResult SetCondition([In] ICondition pCondition);

        [PreserveSig]
        int GetShellItem(ref Guid riid, [Out, MarshalAs(UnmanagedType.Interface)] out IShellItem ppv);

        [PreserveSig]
        HResult GetIDList([Out] IntPtr ppidl);
    };
}
