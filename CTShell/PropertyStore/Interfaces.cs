using CoreCT.Shell32.Enums;
using CoreCT.Shell32.ShellID;
using CoreCT.Shell32.Structs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace CoreCT.Shell32.PropertyStore
{

    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    [ComImport]
    [Guid(ShellIIDGuid.IPropertyStoreCapabilities)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IPropertyStoreCapabilities
    {
        HResult IsPropertyWritable([In] ref PropertyKey propertyKey);
    }

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
        extern HResult GetState(ref PropertyKey key, out PropertyStoreCacheState state);

        /// <summary>
        /// Gets the valeu and state of a property in the cache
        /// </summary>
        /// <param name="propKey"></param>
        /// <param name="pv"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult GetValueAndState(ref PropertyKey propKey, out PropVariant pv, out PropertyStoreCacheState state);

        /// <summary>
        /// Sets the state of a property in the cache.
        /// </summary>
        /// <param name="propKey"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult SetState(ref PropertyKey propKey, PropertyStoreCacheState state);

        /// <summary>
        /// Sets the value and state in the cache.
        /// </summary>
        /// <param name="propKey"></param>
        /// <param name="pv"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult SetValueAndState(ref PropertyKey propKey, [In] PropVariant pv, PropertyStoreCacheState state);
    }

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
        extern HResult GetCount(out uint propertyCount);

        /// <summary>
        /// Get a property key located at a specific index.
        /// </summary>
        /// <param name="propertyIndex"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult GetAt([In] uint propertyIndex, ref PropertyKey key);

        /// <summary>
        /// Gets the value of a property from the store
        /// </summary>
        /// <param name="key"></param>
        /// <param name="pv"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult GetValue([In] ref PropertyKey key, out PropVariant pv);

        /// <summary>
        /// Sets the value of a property in the store
        /// </summary>
        /// <param name="key"></param>
        /// <param name="pv"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [PreserveSig]
        extern HResult SetValue([In] ref PropertyKey key, [In] PropVariant pv);

        /// <summary>
        /// Commits the changes.
        /// </summary>
        /// <returns></returns>
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult Commit();
    }

    [ComImport]
    [Guid(ShellIIDGuid.IPropertyDescriptionList)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IPropertyDescriptionList
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void GetCount(ref uint pcElem);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void GetAt([In] uint iElem, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] ref IPropertyDescription ppv);
    }

    [ComImport]
    [Guid(ShellIIDGuid.IPropertyDescription)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IPropertyDescription
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void GetPropertyKey(ref PropertyKey pkey);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void GetCanonicalName([MarshalAs(UnmanagedType.LPWStr)] ref string ppszName);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult GetPropertyType(ref VarEnum pvartype);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [PreserveSig]
        extern HResult GetDisplayName(ref IntPtr ppszName);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult GetEditInvitation(ref IntPtr ppszInvite);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult GetTypeFlags([In] PropertyTypeOptions mask, ref PropertyTypeOptions ppdtFlags);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult GetViewFlags(ref PropertyViewOptions ppdvFlags);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult GetDefaultColumnWidth(ref uint pcxChars);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult GetDisplayType(ref PropertyDisplayType pdisplaytype);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult GetColumnState(ref PropertyColumnStateOptions pcsFlags);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult GetGroupingRange(ref PropertyGroupingRange pgr);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void GetRelativeDescriptionType(ref RelativeDescriptionType prdt);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void GetRelativeDescription([In] PropVariant propvar1, [In] PropVariant propvar2, [MarshalAs(UnmanagedType.LPWStr)] ref string ppszDesc1, [MarshalAs(UnmanagedType.LPWStr)] ref string ppszDesc2);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult GetSortDescription(ref PropertySortDescription psd);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult GetSortDescriptionLabel([In] bool fDescending, ref IntPtr ppszDescription);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult GetAggregationType(ref PropertyAggregationType paggtype);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult GetConditionType(ref PropertyConditionType pcontype, ref PropertyConditionOperation popDefault);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult GetEnumTypeList([In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out IPropertyEnumTypeList ppv);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void CoerceToCanonicalValue([In] PropVariant propvar);
        // Note: this method signature may be wrong, but it is not used.
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult FormatForDisplay([In] PropVariant propvar, [In] ref PropertyDescriptionFormatOptions pdfFlags, [MarshalAs(UnmanagedType.LPWStr)] ref string ppszDisplay);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult IsValueCanonical([In] PropVariant propvar);
    }

    [ComImport]
    [Guid(ShellIIDGuid.IPropertyDescription2)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IPropertyDescription2 : IPropertyDescription
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void GetImageReferenceForValue([In] PropVariant propvar, [MarshalAs(UnmanagedType.LPWStr)] out string ppszImageRes);
    }

    [ComImport]
    [Guid(ShellIIDGuid.IPropertyEnumType)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IPropertyEnumType
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void GetEnumType(out PropEnumType penumtype);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void GetValue(out PropVariant ppropvar);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void GetRangeMinValue(out PropVariant ppropvar);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void GetRangeSetValue(out PropVariant ppropvar);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void GetDisplayText([MarshalAs(UnmanagedType.LPWStr)] out string ppszDisplay);
    }

    [ComImport]
    [Guid(ShellIIDGuid.IPropertyEnumType2)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IPropertyEnumType2 : IPropertyEnumType
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void GetImageReference([MarshalAs(UnmanagedType.LPWStr)] out string ppszImageRes);
    }

    [ComImport]
    [Guid(ShellIIDGuid.IPropertyEnumTypeList)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IPropertyEnumTypeList
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void GetCount(out uint pctypes);

        // riid may be IID_IPropertyEnumType
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void GetAt([In] uint itype, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out IPropertyEnumType ppv);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void GetConditionAt([In] uint index, [In] ref Guid riid, ref IntPtr ppv);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void FindMatchingIndex([In] PropVariant propvarCmp, out uint pnIndex);
    }



}
