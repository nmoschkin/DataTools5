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
    [ComImport,
    Guid(ShellIIDGuid.IQueryParser),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IQueryParser
    {
        // Parse parses an input string, producing a query solution.
        // pCustomProperties should be an enumeration of IRichChunk objects, one for each custom property
        // the application has recognized. pCustomProperties may be NULL, equivalent to an empty enumeration.
        // For each IRichChunk, the position information identifies the character span of the custom property,
        // the string value should be the name of an actual property, and the PROPVARIANT is completely ignored.
        [PreserveSig]
        HResult Parse([In, MarshalAs(UnmanagedType.LPWStr)] string pszInputString, [In] IEnumUnknown pCustomProperties, [Out] out IQuerySolution ppSolution);

        // Set a single option. See STRUCTURED_QUERY_SINGLE_OPTION above.
        [PreserveSig]
        HResult SetOption([In] StructuredQuerySingleOption option, [In] PropVariant pOptionValue);

        [PreserveSig]
        HResult GetOption([In] StructuredQuerySingleOption option, [Out] PropVariant pOptionValue);

        // Set a multi option. See STRUCTURED_QUERY_MULTIOPTION above.
        [PreserveSig]
        HResult SetMultiOption([In] StructuredQueryMultipleOption option, [In, MarshalAs(UnmanagedType.LPWStr)] string pszOptionKey, [In] PropVariant pOptionValue);

        // Get a schema provider for browsing the currently loaded schema.
        [PreserveSig]
        HResult GetSchemaProvider([Out] out /*ISchemaProvider*/ IntPtr ppSchemaProvider);

        // Restate a condition as a query string according to the currently selected syntax.
        // The parameter fUseEnglish is reserved for future use; must be FALSE.
        [PreserveSig]
        HResult RestateToString([In] ICondition pCondition, [In] bool fUseEnglish, [Out, MarshalAs(UnmanagedType.LPWStr)] out string ppszQueryString);

        // Parse a condition for a given property. It can be anything that would go after 'PROPERTY:' in an AQS expession.
        [PreserveSig]
        HResult ParsePropertyValue([In, MarshalAs(UnmanagedType.LPWStr)] string pszPropertyName, [In, MarshalAs(UnmanagedType.LPWStr)] string pszInputString, [Out] out IQuerySolution ppSolution);

        // Restate a condition for a given property. If the condition contains a leaf with any other property name, or no property name at all,
        // E_INVALIDARG will be returned.
        [PreserveSig]
        HResult RestatePropertyValueToString([In] ICondition pCondition, [In] bool fUseEnglish, [Out, MarshalAs(UnmanagedType.LPWStr)] out string ppszPropertyName, [Out, MarshalAs(UnmanagedType.LPWStr)] out string ppszQueryString);
    }
}
