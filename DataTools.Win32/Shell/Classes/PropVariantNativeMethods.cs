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
    internal sealed class PropVariantNativeMethods
    {
        private PropVariantNativeMethods()
        {
        }
        // returns hresult
        [DllImport("Ole32.dll", PreserveSig = false)]
        internal static extern void PropVariantClear(PropVariant pvar);

        // psa is actually returned, not hresult
        [DllImport("OleAut32.dll", PreserveSig = true)]
        internal static extern IntPtr SafeArrayCreateVector(ushort vt, int lowerBound, uint cElems);

        // returns hresult
        [DllImport("OleAut32.dll", PreserveSig = false)]
        internal static extern IntPtr SafeArrayAccessData(IntPtr psa);

        // returns hresult
        [DllImport("OleAut32.dll", PreserveSig = false)]
        internal static extern void SafeArrayUnaccessData(IntPtr psa);

        // retuns uint32
        [DllImport("OleAut32.dll", PreserveSig = true)]
        internal static extern uint SafeArrayGetDim(IntPtr psa);

        // returns hresult
        [DllImport("OleAut32.dll", PreserveSig = false)]
        internal static extern int SafeArrayGetLBound(IntPtr psa, uint nDim);

        // returns hresult
        [DllImport("OleAut32.dll", PreserveSig = false)]
        internal static extern int SafeArrayGetUBound(IntPtr psa, uint nDim);

        // This decl for SafeArrayGetElement is only valid for cDims==1!
        // returns hresult
        [DllImport("OleAut32.dll", PreserveSig = false)]
        internal static extern object SafeArrayGetElement(IntPtr psa, ref int rgIndices);
        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void InitPropVariantFromPropVariantVectorElem([In] PropVariant propvarIn, uint iElem, PropVariant ppropvar);
        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void InitPropVariantFromFileTime([In] ref System.Runtime.InteropServices.ComTypes.FILETIME pftIn, PropVariant ppropvar);
        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern int PropVariantGetElementCount([In] PropVariant propVar);
        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void PropVariantGetBooleanElem([In] PropVariant propVar, [In] uint iElem, [MarshalAs(UnmanagedType.Bool)] out bool pfVal);
        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void PropVariantGetInt16Elem([In] PropVariant propVar, [In] uint iElem, out short pnVal);
        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void PropVariantGetUInt16Elem([In] PropVariant propVar, [In] uint iElem, out ushort pnVal);
        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void PropVariantGetInt32Elem([In] PropVariant propVar, [In] uint iElem, out int pnVal);
        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void PropVariantGetUInt32Elem([In] PropVariant propVar, [In] uint iElem, out uint pnVal);
        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void PropVariantGetInt64Elem([In] PropVariant propVar, [In] uint iElem, out long pnVal);
        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void PropVariantGetUInt64Elem([In] PropVariant propVar, [In] uint iElem, out ulong pnVal);
        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void PropVariantGetDoubleElem([In] PropVariant propVar, [In] uint iElem, out double pnVal);
        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void PropVariantGetFileTimeElem([In] PropVariant propVar, [In] uint iElem, [MarshalAs(UnmanagedType.Struct)] out System.Runtime.InteropServices.ComTypes.FILETIME pftVal);
        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void PropVariantGetStringElem([In] PropVariant propVar, [In] uint iElem, [MarshalAs(UnmanagedType.LPWStr)] ref string ppszVal);
        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void InitPropVariantFromBooleanVector([In][MarshalAs(UnmanagedType.LPArray)] bool[] prgf, uint cElems, PropVariant ppropvar);
        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void InitPropVariantFromInt16Vector([In] short[] prgn, uint cElems, PropVariant ppropvar);
        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void InitPropVariantFromUInt16Vector([In] ushort[] prgn, uint cElems, PropVariant ppropvar);
        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void InitPropVariantFromInt32Vector([In] int[] prgn, uint cElems, PropVariant propVar);
        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void InitPropVariantFromUInt32Vector([In] uint[] prgn, uint cElems, PropVariant ppropvar);
        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void InitPropVariantFromInt64Vector([In] long[] prgn, uint cElems, PropVariant ppropvar);
        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void InitPropVariantFromUInt64Vector([In] ulong[] prgn, uint cElems, PropVariant ppropvar);
        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void InitPropVariantFromDoubleVector([In] double[] prgn, uint cElems, PropVariant propvar);
        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void InitPropVariantFromFileTimeVector([In] System.Runtime.InteropServices.ComTypes.FILETIME[] prgft, uint cElems, PropVariant ppropvar);
        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void InitPropVariantFromStringVector([In] string[] prgsz, uint cElems, PropVariant ppropvar);
    }
}
