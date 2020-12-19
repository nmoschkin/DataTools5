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
    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */

    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    internal sealed class PropertySystemNativeMethods
    {
        private PropertySystemNativeMethods()
        {
        }
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public enum RelativeDescriptionType
        {
            General,
            Date,
            Size,
            Count,
            Revision,
            Length,
            Duration,
            Speed,
            Rate,
            Rating,
            Priority
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int PSGetNameFromPropertyKey(ref PropertyKey propkey, [MarshalAs(UnmanagedType.LPWStr)] out string ppszCanonicalName);
        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern HResult PSGetPropertyDescription(ref PropertyKey propkey, ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out IPropertyDescription ppv);
        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int PSGetPropertyKeyFromName([In][MarshalAs(UnmanagedType.LPWStr)] string pszCanonicalName, ref PropertyKey propkey);
        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int PSGetPropertyDescriptionListFromString([In][MarshalAs(UnmanagedType.LPWStr)] string pszPropList, [In] ref Guid riid, ref IPropertyDescriptionList ppv);



        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    }
}
