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
    /// A property store
    /// </summary>
    [ComImport]
    [Guid(ShellIIDGuid.IPropertyStore)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IPropertyStore
    {
        /// <summary>
        /// Gets the number of properties contained in the property store.
        /// </summary>
        /// <param name="propertyCount"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult GetCount(out uint propertyCount);

        /// <summary>
        /// Get a property key located at a specific index.
        /// </summary>
        /// <param name="propertyIndex"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult GetAt([In] uint propertyIndex, ref PropertyKey key);

        /// <summary>
        /// Gets the value of a property from the store
        /// </summary>
        /// <param name="key"></param>
        /// <param name="pv"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult GetValue([In] ref PropertyKey key, [Out] PropVariant pv);

        /// <summary>
        /// Sets the value of a property in the store
        /// </summary>
        /// <param name="key"></param>
        /// <param name="pv"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [PreserveSig]
        HResult SetValue([In] ref PropertyKey key, [In] PropVariant pv);

        /// <summary>
        /// Commits the changes.
        /// </summary>
        /// <returns></returns>
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult Commit();
    }
}
