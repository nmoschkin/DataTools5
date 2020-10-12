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
    Guid(ShellIIDGuid.IShellLibrary),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IShellLibrary
    {
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult LoadLibraryFromItem(
            [In, MarshalAs(UnmanagedType.Interface)] IShellItem library,
            [In] AccessModes grfMode);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void LoadLibraryFromKnownFolder(
            [In] ref Guid knownfidLibrary,
            [In] AccessModes grfMode);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void AddFolder([In, MarshalAs(UnmanagedType.Interface)] IShellItem location);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void RemoveFolder([In, MarshalAs(UnmanagedType.Interface)] IShellItem location);

        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult GetFolders(
            [In] LibraryFolderFilter lff,
            [In] ref Guid riid,
            [MarshalAs(UnmanagedType.Interface)] out IShellItemArray ppv);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void ResolveFolder(
            [In, MarshalAs(UnmanagedType.Interface)] IShellItem folderToResolve,
            [In] uint timeout,
            [In] ref Guid riid,
            [MarshalAs(UnmanagedType.Interface)] out IShellItem ppv);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetDefaultSaveFolder(
            [In] DefaultSaveFolderType dsft,
            [In] ref Guid riid,
            [MarshalAs(UnmanagedType.Interface)] out IShellItem ppv);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void SetDefaultSaveFolder(
            [In] DefaultSaveFolderType dsft,
            [In, MarshalAs(UnmanagedType.Interface)] IShellItem si);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetOptions(
            out LibraryOptions lofOptions);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void SetOptions(
            [In] LibraryOptions lofMask,
            [In] LibraryOptions lofOptions);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetFolderType(out Guid ftid);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void SetFolderType([In] ref Guid ftid);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetIcon([MarshalAs(UnmanagedType.LPWStr)] out string icon);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void SetIcon([In, MarshalAs(UnmanagedType.LPWStr)] string icon);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void Commit();

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void Save(
            [In, MarshalAs(UnmanagedType.Interface)] IShellItem folderToSaveIn,
            [In, MarshalAs(UnmanagedType.LPWStr)] string libraryName,
            [In] LibrarySaveOptions lsf,
            [MarshalAs(UnmanagedType.Interface)] out IShellItem2 savedTo);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void SaveInKnownFolder(
            [In] ref Guid kfidToSaveIn,
            [In, MarshalAs(UnmanagedType.LPWStr)] string libraryName,
            [In] LibrarySaveOptions lsf,
            [MarshalAs(UnmanagedType.Interface)] out IShellItem2 savedTo);
    };
}
