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
    // Summary:
    //     Provides the managed definition of the IPersistStream interface, with functionality
    //     from IPersist.
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("00000109-0000-0000-C000-000000000046")]
    internal interface IPersistStream
    {
        // Summary:
        //     Retrieves the class identifier (CLSID) of an object.
        //
        // Parameters:
        //   pClassID:
        //     When this method returns, contains a reference to the CLSID. This parameter
        //     is passed uninitialized.
        [PreserveSig]
        void GetClassID(out Guid pClassID);
        //
        // Summary:
        //     Checks an object for changes since it was last saved to its current file.
        //
        // Returns:
        //     S_OK if the file has changed since it was last saved; S_FALSE if the file
        //     has not changed since it was last saved.
        [PreserveSig]
        HResult IsDirty();

        [PreserveSig]
        HResult Load([In, MarshalAs(UnmanagedType.Interface)] IStream stm);

        [PreserveSig]
        HResult Save([In, MarshalAs(UnmanagedType.Interface)] IStream stm, bool fRemember);

        [PreserveSig]
        HResult GetSizeMax(out ulong cbSize);
    }
}
