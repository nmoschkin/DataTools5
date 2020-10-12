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
    [ComImport]
    [Guid(ShellIIDGuid.IPropertyDescription)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IPropertyDescription
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetPropertyKey(ref PropertyKey pkey);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetCanonicalName([MarshalAs(UnmanagedType.LPWStr)] ref string ppszName);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult GetPropertyType(ref VarEnum pvartype);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [PreserveSig]
        HResult GetDisplayName(ref IntPtr ppszName);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult GetEditInvitation(ref IntPtr ppszInvite);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult GetTypeFlags([In] PropertyTypeOptions mask, ref PropertyTypeOptions ppdtFlags);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult GetViewFlags(ref PropertyViewOptions ppdvFlags);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult GetDefaultColumnWidth(ref uint pcxChars);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult GetDisplayType(ref PropertyDisplayType pdisplaytype);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult GetColumnState(ref PropertyColumnStateOptions pcsFlags);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult GetGroupingRange(ref PropertyGroupingRange pgr);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetRelativeDescriptionType(ref PropertySystemNativeMethods.RelativeDescriptionType prdt);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetRelativeDescription([In] PropVariant propvar1, [In] PropVariant propvar2, [MarshalAs(UnmanagedType.LPWStr)] ref string ppszDesc1, [MarshalAs(UnmanagedType.LPWStr)] ref string ppszDesc2);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult GetSortDescription(ref PropertySortDescription psd);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult GetSortDescriptionLabel([In] bool fDescending, ref IntPtr ppszDescription);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult GetAggregationType(ref PropertyAggregationType paggtype);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult GetConditionType(ref PropertyConditionType pcontype, ref PropertyConditionOperation popDefault);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult GetEnumTypeList([In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out IPropertyEnumTypeList ppv);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void CoerceToCanonicalValue([In] PropVariant propvar);
        // Note: this method signature may be wrong, but it is not used.
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult FormatForDisplay([In] PropVariant propvar, [In] ref PropertyDescriptionFormatOptions pdfFlags, [MarshalAs(UnmanagedType.LPWStr)] ref string ppszDisplay);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult IsValueCanonical([In] PropVariant propvar);
    }
}
