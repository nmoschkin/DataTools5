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
    [ComImport]
    [Guid("F04061AC-1659-4a3f-A954-775AA57FC083")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComConversionLoss]
    internal interface IAssocHandler
    {
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult GetName([MarshalAs(UnmanagedType.LPWStr)] out string ppsz);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult GetUIName([MarshalAs(UnmanagedType.LPWStr)] out string ppsz);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult GetIconLocation([MarshalAs(UnmanagedType.LPWStr)] out string ppszPath, out int pIndex);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult IsRecommended();
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult MakeDefault([MarshalAs(UnmanagedType.LPWStr)][In] string pszDescription);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult Invoke([In] IDataObject pdo);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult CreateInvoker([In] IDataObject pdo, out IAssocHandlerInvoker ppInvoker);
    }
}
