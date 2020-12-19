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
    /// An in-memory property store cache
    /// </summary>
    [ComImport]
    [Guid(ShellIIDGuid.IPropertyStoreCache)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IPropertyStoreCache
    {
        /// <summary>
        /// Gets the state of a property stored in the cache
        /// </summary>
        /// <param name="key"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult GetState(ref PropertyKey key, out PropertyStoreCacheState state);

        /// <summary>
        /// Gets the valeu and state of a property in the cache
        /// </summary>
        /// <param name="propKey"></param>
        /// <param name="pv"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult GetValueAndState(ref PropertyKey propKey, out PropVariant pv, out PropertyStoreCacheState state);

        /// <summary>
        /// Sets the state of a property in the cache.
        /// </summary>
        /// <param name="propKey"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult SetState(ref PropertyKey propKey, PropertyStoreCacheState state);

        /// <summary>
        /// Sets the value and state in the cache.
        /// </summary>
        /// <param name="propKey"></param>
        /// <param name="pv"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult SetValueAndState(ref PropertyKey propKey, [In] PropVariant pv, PropertyStoreCacheState state);
    }
}
