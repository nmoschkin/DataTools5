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
    /// <summary>
    /// Provides a set of flags to be used with IQueryParser::SetOption and
    /// IQueryParser::GetOption to indicate individual options.
    /// </summary>
    public enum StructuredQuerySingleOption
    {
        /// <summary>
        /// The value should be VT_LPWSTR and the path to a file containing a schema binary.
        /// </summary>
        Schema,

        /// <summary>
        /// The value must be VT_EMPTY (the default) or a VT_UI4 that is an LCID. It is used
        /// as the locale of contents (not keywords) in the query to be searched for, when no
        /// other information is available. The default value is the current keyboard locale.
        /// Retrieving the value always returns a VT_UI4.
        /// </summary>
        Locale,

        /// <summary>
        /// This option is used to override the default word breaker used when identifying keywords
        /// in queries. The default word breaker is chosen according to the language of the keywords
        /// (cf. SQSO_LANGUAGE_KEYWORDS below). When setting this option, the value should be VT_EMPTY
        /// for using the default word breaker, or a VT_UNKNOWN with an object supporting
        /// the IWordBreaker interface. Retrieving the option always returns a VT_UNKNOWN with an object
        /// supporting the IWordBreaker interface.
        /// </summary>
        WordBreaker,

        /// <summary>
        /// The value should be VT_EMPTY or VT_BOOL with VARIANT_TRUE to allow natural query
        /// syntax (the default) or VT_BOOL with VARIANT_FALSE to allow only advanced query syntax.
        /// Retrieving the option always returns a VT_BOOL.
        /// This option is now deprecated, use SQSO_SYNTAX.
        /// </summary>
        NaturalSyntax,

        /// <summary>
        /// The value should be VT_BOOL with VARIANT_TRUE to generate query expressions
        /// as if each word in the query had a star appended to it (unless followed by punctuation
        /// other than a parenthesis), or VT_EMPTY or VT_BOOL with VARIANT_FALSE to
        /// use the words as they are (the default). A word-wheeling application
        /// will generally want to set this option to true.
        /// Retrieving the option always returns a VT_BOOL.
        /// </summary>
        AutomaticWildcard,

        /// <summary>
        /// Reserved. The value should be VT_EMPTY (the default) or VT_I4.
        /// Retrieving the option always returns a VT_I4.
        /// </summary>
        TraceLevel,

        /// <summary>
        /// The value must be a VT_UI4 that is a LANGID. It defaults to the default user UI language.
        /// </summary>
        LanguageKeywords,

        /// <summary>
        /// The value must be a VT_UI4 that is a STRUCTURED_QUERY_SYNTAX value.
        /// It defaults to SQS_NATURAL_QUERY_SYNTAX.
        /// </summary>
        Syntax,

        /// <summary>
        /// The value must be a VT_BLOB that is a copy of a TIME_ZONE_INFORMATION structure.
        /// It defaults to the current time zone.
        /// </summary>
        TimeZone,

        /// <summary>
        /// This setting decides what connector should be assumed between conditions when none is specified.
        /// The value must be a VT_UI4 that is a CONDITION_TYPE. Only CT_AND_CONDITION and CT_OR_CONDITION
        /// are valid. It defaults to CT_AND_CONDITION.
        /// </summary>
        ImplicitConnector,

        /// <summary>
        /// This setting decides whether there are special requirements on the case of connector keywords (such
        /// as AND or OR). The value must be a VT_UI4 that is a CASE_REQUIREMENT value.
        /// It defaults to CASE_REQUIREMENT_UPPER_IF_AQS.
        /// </summary>
        ConnectorCase
    }
}
