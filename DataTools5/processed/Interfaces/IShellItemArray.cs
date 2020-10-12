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
    Guid(ShellIIDGuid.IShellItemArray),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IShellItemArray
    {
        // Not supported: IBindCtx.
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult BindToHandler(
            [In, MarshalAs(UnmanagedType.Interface)] IntPtr pbc,
            [In] ref Guid rbhid,
            [In] ref Guid riid,
            out IntPtr ppvOut);

        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult GetPropertyStore(
            [In] int Flags,
            [In] ref Guid riid,
            out IntPtr ppv);

        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult GetPropertyDescriptionList(
            [In] ref PropertyKey keyType,
            [In] ref Guid riid,
            out IntPtr ppv);

        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult GetAttributes(
            [In] ShellItemAttributeOptions dwAttribFlags,
            [In] ShellFileGetAttributesOptions sfgaoMask,
            out ShellFileGetAttributesOptions psfgaoAttribs);

        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult GetCount(out uint pdwNumItems);

        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult GetItemAt(
            [In] uint dwIndex,
            [MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);

        // Not supported: IEnumShellItems (will use GetCount and GetItemAt instead).
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult EnumItems([MarshalAs(UnmanagedType.Interface)] out IntPtr ppenumShellItems);
    }
}
