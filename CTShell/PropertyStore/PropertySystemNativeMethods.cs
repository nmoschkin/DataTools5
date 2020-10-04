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
    public sealed class PropertySystemNativeMethods
    {

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
