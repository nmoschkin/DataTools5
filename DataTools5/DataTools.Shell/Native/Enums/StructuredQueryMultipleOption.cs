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
    /// <summary>
    /// Provides a set of flags to be used with IQueryParser::SetMultiOption
    /// to indicate individual options.
    /// </summary>
    public enum StructuredQueryMultipleOption
    {
        /// <summary>
        /// The key should be property name P. The value should be a
        /// VT_UNKNOWN with an IEnumVARIANT which has two values: a VT_BSTR that is another
        /// property name Q and a VT_I4 that is a CONDITION_OPERATION cop. A predicate with
        /// property name P, some operation and a value V will then be replaced by a predicate
        /// with property name Q, operation cop and value V before further processing happens.
        /// </summary>
        VirtualProperty,

        /// <summary>
        /// The key should be a value type name V. The value should be a
        /// VT_LPWSTR with a property name P. A predicate with no property name and a value of type
        /// V (or any subtype of V) will then use property P.
        /// </summary>
        DefaultProperty,

        /// <summary>
        /// The key should be a value type name V. The value should be a
        /// VT_UNKNOWN with a IConditionGenerator G. The GenerateForLeaf method of
        /// G will then be applied to any predicate with value type V and if it returns a query
        /// expression, that will be used. If it returns NULL, normal processing will be used
        /// instead.
        /// </summary>
        GeneratorForType,

        /// <summary>
        /// The key should be a property name P. The value should be a VT_VECTOR|VT_LPWSTR,
        /// where each string is a property name. The count must be at least one. This "map" will be
        /// added to those of the loaded schema and used during resolution. A second call with the
        /// same key will replace the current map. If the value is VT_NULL, the map will be removed.
        /// </summary>
        MapProperty
    }
}
