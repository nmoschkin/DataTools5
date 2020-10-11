// ' ************************************************* ''
// ' DataTools Visual Basic Utility Library 
// '
// ' Module: Text Processing Utilities
// ' 
// ' Copyright (C) 2011-2020 Nathan Moschkin
// ' All Rights Reserved
// '
// ' Licensed Under the Microsoft Public License   
// ' ************************************************* ''

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using DataTools.Memory;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace DataTools.Strings
{

    /// <summary>
    /// A large collection of text processing tools.
    /// </summary>
    /// <remarks></remarks>
    public static class TextTools
    {
        public enum CODETYPES
        {
            ctJavascript = 0,
            ctHTML = 1,
            ctPHP = 2
        }

        public const char vbDblQuote = '"';

        /// <summary>
        /// All allowed mathematical characters.
        /// </summary>
        /// <remarks></remarks>
        public const string MathChars = @"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789.()+-\=/*^%";

        /// <summary>
        /// All canonical letters and numbers.
        /// </summary>
        /// <remarks></remarks>
        public const string AlphaNumericChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        /// <summary>
        /// All canonical letters.
        /// </summary>
        /// <remarks></remarks>
        public const string AlphaChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        /// <summary>
        /// All standard non-alphanumeric characters.
        /// </summary>
        /// <remarks></remarks>
        public const string NonAlphas = "-._~:/?#[]@!$&'()*+,;=";

        /// <summary>
        /// Characters allowed in a url.
        /// </summary>
        /// <remarks></remarks>
        public const string UrlAllowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-._~:/?#[]@!$&'()*+,;=";

        /// <summary>
        /// Match condition flags.
        /// </summary>
        /// <remarks></remarks>
        [Flags]
        public enum MatchCondition
        {

            /// <summary>
            /// The match must be exact
            /// </summary>
            /// <remarks></remarks>
            Exact = 0x0,

            /// <summary>
            /// The match must be exact up until the length of the
            /// requested expression (if it is shorter than the matched index)
            /// </summary>
            /// <remarks></remarks>
            FirstOfSearch = 0x1,

            /// <summary>
            /// The match must be exact up until the length of the
            /// matched index (if it is shorter than the search expression)
            /// </summary>
            /// <remarks></remarks>
            FirstOfMatch = 0x2,

            /// <summary>
            /// Instead of returning the index matched, return the string
            /// </summary>
            /// <remarks></remarks>
            ReturnString = 0x4
        }

        public enum SortOrder
        {
            Ascending = 0x0,
            Descending = 0x1
        }


        /// <summary>
        /// Flags for use with NoSpace
        /// </summary>
        /// <remarks></remarks>
        [Flags]
        public enum NoSpaceModifiers
        {
            None = 0,
            BeforeToLower = 1,
            BeforeToUpper = 2,
            AfterToLower = 4,
            AfterToUpper = 8,
            FirstToLower = 16,
            FirstToUpper = 32
        }

        public struct PageLines
        {
            public int Page;
            public string[] Lines;
        }

        public struct ATTRVAL
        {
            public string Name;
            public string Value;
        }

        public struct XMLTAG
        {
            public string Name;
            public ATTRVAL[] Attributes;
            public XMLTAG[] Nodes;
        }

        /// <summary>
        /// Determine if a string consists only of numbers.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsAllNumbers(string value)
        {
            bool IsAllNumbersRet = default;
            char[] ch = value.ToCharArray();
            int i = 0;
            int c = ch.Length - 1;
            var loopTo = c;
            for (i = 0; i <= loopTo; i++)
            {
                if (ch[i] < '0' || ch[i] > '9')
                {
                    IsAllNumbersRet = false;
                    return IsAllNumbersRet;
                }
            }

            IsAllNumbersRet = true;
            return IsAllNumbersRet;
        }

        /// <summary>
        /// Returns true if the given object is a byte array.
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsByteArray(object val)
        {
            if (val is null)
                return false;
            if (val.GetType() == typeof(byte[]))
                return true;
            return false;
        }

        /// <summary>
        /// Compares two array of bytes for equality.
        /// </summary>
        /// <param name="a">First array</param>
        /// <param name="b">Second array</param>
        /// <param name="result">Relative disposition</param>
        /// <returns>True if equal</returns>
        /// <remarks></remarks>
        public static bool CompareBytes(byte[] a, byte[] b, [Optional, DefaultParameterValue(0)] ref int result)
        {
            if (a.Length != b.Length)
                return false;
            if (a is null & b is null)
                return true;
            if (a is object & b is null || b is object & a is null)
                return false;
            int i;
            int c = a.Length - 1;
            var loopTo = c;
            for (i = 0; i <= loopTo; i++)
            {
                if (a[i] != b[i])
                {
                    if (a[i] > b[i])
                        result = 1;
                    else
                        result = -1;
                    return false;
                }
            }

            result = 0;
            return true;
        }

        /// <summary>
        /// Parses a point back from Point.ToString()
        /// </summary>
        /// <param name="ptString"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Point ParsePoint(string ptString)
        {
            var pt = ParsePointF(ptString);
            return new Point((int)Conversion.Fix(pt.X), (int)Conversion.Fix(pt.Y));
        }

        /// <summary>
        /// Parses a point back from PointF.ToString()
        /// </summary>
        /// <param name="ptString"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static PointF ParsePointF(string ptString)
        {
            int argstartIndex = -1;
            string s = TextBetween(ptString, "{", "}", startIndex: ref argstartIndex);
            string[] p;
            string[] v;
            float x;
            float y;
            p = BatchParse(s, ",");
            v = BatchParse(p[0], "=");
            x = (float)Conversion.Val(Microsoft.VisualBasic.Strings.Trim(v[1]));
            v = BatchParse(p[1], "=");
            y = (float)Conversion.Val(Microsoft.VisualBasic.Strings.Trim(v[1]));
            return new PointF(x, y);
        }

        /// <summary>
        /// Removes all spaces from a string using default modifiers.
        /// </summary>
        /// <param name="subject"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string NoSpace(string subject)
        {
            return StripChars(subject, " " + Constants.vbTab);
        }

        /// <summary>
        /// Remove all spaces from a string and alters the output results according to NoSpaceModifiers
        /// </summary>
        /// <param name="subject">String to alter.</param>
        /// <param name="modifiers">Modifiers.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string NoSpace(string subject, NoSpaceModifiers modifiers)
        {
            char[] ch;
            int i;
            int j = subject.Length - 1;
            int e = 0;
            string exclusions = " " + Constants.vbTab;
            bool ws = false;
            ch = new char[j + 1];
            var loopTo = j;
            for (i = 0; i <= loopTo; i++)
            {
                if (i == 0 && (modifiers & (NoSpaceModifiers.FirstToLower | NoSpaceModifiers.FirstToUpper)) != 0 && exclusions.IndexOf(subject[i]) == -1)
                {
                    switch (modifiers)
                    {
                        case NoSpaceModifiers.FirstToUpper:
                            {
                                ch[e] = char.ToUpper(subject[i]);
                                break;
                            }

                        case NoSpaceModifiers.FirstToLower:
                            {
                                ch[e] = char.ToLower(subject[i]);
                                break;
                            }
                    }
                }

                if (exclusions.IndexOf(subject[i]) != -1)
                {
                    if (Conversions.ToBoolean(modifiers))
                    {
                        if (i > 0)
                        {
                            if (Conversions.ToBoolean(modifiers & NoSpaceModifiers.BeforeToLower))
                            {
                                ch[e - 1] = char.ToLower(ch[e - 1]);
                            }
                            else if (Conversions.ToBoolean(modifiers & NoSpaceModifiers.BeforeToUpper))
                            {
                                ch[e - 1] = char.ToUpper(ch[e - 1]);
                            }
                        }
                    }

                    ws = true;
                }
                else if (ws)
                {
                    if (Conversions.ToBoolean(modifiers))
                    {
                        if (Conversions.ToBoolean(modifiers & NoSpaceModifiers.AfterToLower))
                        {
                            ch[e] = char.ToLower(subject[i]);
                        }
                        else if (Conversions.ToBoolean(modifiers & NoSpaceModifiers.AfterToUpper))
                        {
                            ch[e] = char.ToUpper(subject[i]);
                        }
                        else
                        {
                            ch[e] = subject[i];
                            e += 1;
                        }
                    }

                    ws = false;
                }
                else
                {
                    ch[e] = subject[i];
                    e += 1;
                }
            }

            return Conversions.ToString(ch);
        }

        /// <summary>
        /// Counts occurrences of 'character'
        /// </summary>
        /// <param name="subject">The string to count.</param>
        /// <param name="character">The character to count.</param>
        /// <returns>The number of occurrences of 'character' in 'subject'</returns>
        /// <remarks></remarks>
        public static int CountChar(string subject, char character)
        {
            char[] ch = subject.ToCharArray();
            int c = ch.Length - 1;
            int d = 0;
            for (int i = 0, loopTo = c; i <= loopTo; i++)
            {
                if (ch[i] == character)
                    d += 1;
            }

            return d;
        }

        /// <summary>
        /// Exclude a set of characters from a string.
        /// </summary>
        /// <param name="subject">The text to search.</param>
        /// <param name="exclusions">The characters to remove.</param>
        /// <returns>Processed text.</returns>
        /// <remarks></remarks>

        /* TODO ERROR: Skipped SkippedTokensTrivia */
        public static string StripChars(string subject, string exclusions)
        {
            ;
#error Cannot convert LocalDeclarationStatementSyntax - see comment for details
            /* Cannot convert LocalDeclarationStatementSyntax, System.NotSupportedException: StaticKeyword not supported!
               at ICSharpCode.CodeConverter.CSharp.SyntaxKindExtensions.ConvertToken(SyntaxKind t, TokenContext context)
               at ICSharpCode.CodeConverter.CSharp.CommonConversions.ConvertModifier(SyntaxToken m, TokenContext context)
               at ICSharpCode.CodeConverter.CSharp.CommonConversions.<ConvertModifiersCore>d__43.MoveNext()
               at System.Linq.Enumerable.<ConcatIterator>d__59`1.MoveNext()
               at System.Linq.Enumerable.WhereEnumerableIterator`1.MoveNext()
               at System.Linq.Buffer`1..ctor(IEnumerable`1 source)
               at System.Linq.OrderedEnumerable`1.<GetEnumerator>d__1.MoveNext()
               at Microsoft.CodeAnalysis.SyntaxTokenList.CreateNode(IEnumerable`1 tokens)
               at ICSharpCode.CodeConverter.CSharp.CommonConversions.ConvertModifiers(SyntaxNode node, IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isVariableOrConst, SyntaxKind[] extraCsModifierKinds)
               at ICSharpCode.CodeConverter.CSharp.MethodBodyExecutableStatementVisitor.<VisitLocalDeclarationStatement>d__31.MoveNext()
            --- End of stack trace from previous location where exception was thrown ---
               at ICSharpCode.CodeConverter.CSharp.HoistedNodeStateVisitor.<AddLocalVariablesAsync>d__6.MoveNext()
            --- End of stack trace from previous location where exception was thrown ---
               at ICSharpCode.CodeConverter.CSharp.CommentConvertingMethodBodyVisitor.<DefaultVisitInnerAsync>d__3.MoveNext()

            Input:

                    <ThreadStatic>
                    Static ch As New Global.System.Text.StringBuilder

             */
            int i;
            int j = subject.Length - 1;
            char[] cha = subject.ToCharArray();
            ch.Clear();
            var loopTo = j;
            for (i = 0; i <= loopTo; i++)
            {
                if (exclusions.IndexOf(cha[i]) == -1)
                    ch.Append(subject.Substring(i, 1));
            }

            return ch.ToString();
        }

        /// <summary>
        /// Removes surrounding quotes from a single quoted expression.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string Unquote(string v)
        {
            try
            {
                string s = v.Trim();
                if (s.Length == 0)
                    return "";
                if (s.IndexOf("\"") == 0)
                {
                    s = s.Substring(1);
                }

                if (s.Length == 0)
                    return "";
                if (s.LastIndexOf("\"") == s.Length - 1)
                {
                    if (s.Length == 1)
                        return "";
                    s = s.Substring(0, s.Length - 1);
                }

                return s;
            }
            catch (Exception ex)
            {
                return v;
            }
        }

        /// <summary>
        /// Finds and returns the first occurrence of text between startString and stopString
        /// </summary>
        /// <param name="subject">The text to process.</param>
        /// <param name="startString">The starting string to scan.</param>
        /// <param name="stopString">The ending string to scan.</param>
        /// <param name="startIndex">The index at which to start scanning.</param>
        /// <returns>The first occurrence of text between two seperator strings.</returns>
        /// <remarks></remarks>
        public static string TextBetween(string subject, string startString, string stopString, [Optional, DefaultParameterValue(-1)] ref int startIndex)
        {
            int i;
            int j;
            string s;
            if (startIndex == -1)
            {
                i = subject.IndexOf(startString);
            }
            else
            {
                i = subject.IndexOf(startString, startIndex);
            }

            if (i == -1)
                return null;
            j = subject.IndexOf(stopString, i + startString.Length + 1);
            i = i + startString.Length;
            if (j - i < 1)
                return "";
            s = subject.Substring(i, j - i);
            if (startIndex != -1)
            {
                startIndex = j + stopString.Length;
            }

            return s;
        }

        /// <summary>
        /// Batches TextBetween calls, and separates all text before, between, and after each separator pair.
        /// </summary>
        /// <param name="subject">The string to scan.</param>
        /// <param name="startString">The starting string.</param>
        /// <param name="stopString">The stopping string.</param>
        /// <param name="withTokens">Return tokens in array.</param>
        /// <param name="trimStrings">Trim returned strings.</param>
        /// <returns>Array of strings.</returns>
        public static string[] BatchBetween(string subject, string startString, string stopString, bool withTokens = false, bool trimStrings = true)
        {
            if (string.IsNullOrEmpty(subject))
                return null;
            if (string.IsNullOrEmpty(startString))
                return null;
            if (string.IsNullOrEmpty(stopString))
                return null;
            var st = BatchParse(subject, startString, false, false, WithToken: withTokens, WithTokenIn: false, Unquote: false);
            string[] stb;
            var ls = new List<string>();
            for (int i = 0, loopTo = st.Length - 1; i <= loopTo; i++)
            {
                stb = BatchParse(st[i], stopString, false, false, WithToken: withTokens, WithTokenIn: false, Unquote: false);
                foreach (var s in stb)
                {
                    if (trimStrings)
                    {
                        ls.Add(s.Trim());
                    }
                    else
                    {
                        ls.Add(s);
                    }
                }
            }

            return ls.ToArray();
        }


        /// <summary>
        /// Determines if something really is a number. Supports C-style hex strings.
        /// </summary>
        /// <param name="subject">The subject to test.</param>
        /// <param name="noTrim">Whether to skip tripping white space around the text.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsNumber(string subject, bool noTrim = false)
        {
            if (!noTrim)
                subject = Microsoft.VisualBasic.Strings.Trim(subject);
            bool b = Information.IsNumeric(subject);
            if (b)
                return true;
            if (subject.IndexOf("0x") == 0)
            {
                subject = "&H" + subject.Substring(2);
                if (Information.IsNumeric(subject))
                    return Conversion.Val(subject) != 0d;
            }

            return false;
        }


        /// <summary>
        /// Gets a clean filename extension from a string.
        /// </summary>
        /// <param name="s">String to parse.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string CleanExtension(string s)
        {
            int i = s.LastIndexOf(".");
            if (i == -1)
                return "." + s.Trim().ToLower();
            return s.Substring(i).ToLower();
        }

        /// <summary>
        /// Tightens up a text string by removing extra "stuff" for parsing (possibly in a lexer).
        /// </summary>
        /// <param name="Input">The input to process.</param>
        /// <param name="RemoveOperatorGaps">Specify whether to remove the gaps between equals signs and values.</param>
        /// <param name="RemoveComments">Specifies whether to remove comments.</param>
        /// <param name="CommentChars">Comment characters to use to discern comments.</param>
        /// <returns>Processed text.</returns>
        /// <remarks></remarks>
        public static string TightenText(string Input, bool RemoveOperatorGaps = true, bool RemoveComments = true, string CommentChars = "'")
        {
            ;

            // as efficiently as possible
            char[] c;
            int a;
            int b;
            int i;
            int j;
            char[] cmt = CommentChars.ToCharArray();
            char[] ops = "=".ToCharArray();
            char[] d;
            int p = 0;
            int spc = 0;
            byte t = 0;
            byte f = 0;
            c = Input.ToCharArray();
            b = c.Length - 1;
            d = new char[b + 1];
            var loopTo = b;
            for (a = 0; a <= loopTo; a++)
            {
                if (RemoveComments == true)
                {
                    j = cmt.Length - 1;
                    var loopTo1 = j;
                    for (i = 0; i <= loopTo1; i++)
                    {
                        if (c[a] == cmt[i])
                            break;
                    }
                }

                if (t == 0 & Conversions.ToString(c[a]) == " ")
                {
                    continue;
                }
                else if (t == 0)
                {
                    t = 1;
                }

                if (Conversions.ToString(c[a]) == " ")
                {
                    spc += 1;
                }
                else if (spc > 0 & f == 0)
                {
                    d[p] = ' ';
                    d[p + 1] = c[a];
                    p += 2;
                    spc = 0;
                }
                else
                {
                    d[p] = c[a];
                    if (f == 1)
                    {
                        f = 0;
                        spc = 0;
                    }

                    p += 1;
                }

                if (RemoveOperatorGaps)
                {
                    j = ops.Length - 1;
                    var loopTo2 = j;
                    for (i = 0; i <= loopTo2; i++)
                    {
                        if (c[a] == ops[i])
                        {
                            f = 1;
                            if (a > 0)
                            {
                                if (Conversions.ToString(d[p - 2]) == " ")
                                {
                                    d[p - 2] = c[a];
                                    p -= 1;
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            Array.Resize(ref d, p);
            Input = Conversions.ToString(d);
            return Conversions.ToString(d);
        }

        /// <summary>
        /// Returns a string suitable for parsing by Val() or <see cref="FVal" />.
        /// The default behavior processes the string exactly as the Val function looks at it, but it is customizable.
        /// </summary>
        /// <param name="value">String to process.</param>
        /// <param name="justFirst">Whether to process just the first discovered word.</param>
        /// <param name="allInOne">Whether to merge all word blocks together before processing.</param>
        /// <param name="maxSkip">The maximum number of words to skip in search of a number.</param>
        /// <param name="skipChars">Specific characters to ignore or step over in search of a number (default is common currency).</param>
        /// <param name="values">Receives the string values of all discovered individual numbers based upon the selected configuration.</param>
        /// <returns>A result ready to be parsed by a numeric parser.</returns>
        /// <remarks></remarks>
        public static string JustNumbers(string value, bool justFirst = true, bool allInOne = true, int maxSkip = 0, string skipChars = "$£€#\"", [Optional, DefaultParameterValue(null)] ref string[] values, char decimalChar = '.')
        {
            char[] sn = value.ToCharArray();
            char[] sc;
            int i;
            int c = sn.Length - 1;
            int e = 0;
            int skip = -1;
            int d = 0;
            bool t = false;
            int za = 0;
            string firstScan = "&0123456789+-" + decimalChar + " ";
            string scan = " 1234567890-+" + decimalChar + "eEHhOo";
            if (decimalChar == '.')
                skipChars += ",";
            else
                skipChars += ".";
            sc = new char[c + 1];
            var loopTo = c;
            for (i = 0; i <= loopTo; i++)
            {
                if (!t)
                {
                    if (firstScan.IndexOf(sn[i]) >= 0)
                    {
                        t = true;
                    }
                    else if (justFirst)
                    {
                        if (maxSkip > -1 && skip > maxSkip)
                        {
                            if (values is object)
                            {
                                values = Array.Empty<string>();
                            }

                            return Constants.vbNullString;
                        }
                        else
                        {
                            skip += 1;
                        }
                    }
                }

                if (t)
                {
                    if (scan.IndexOf(sn[i]) >= 0)
                    {
                        if (sn[i] == ' ')
                        {
                            if (justFirst && Conversions.ToBoolean(d))
                            {
                                break;
                            }

                            d += 1;
                            t = false;
                            if (!allInOne)
                            {
                                sc[e] = sn[i];
                                e += 1;
                            }
                        }
                        else
                        {
                            sc[e] = sn[i];
                            e += 1;
                        }
                    }
                }
                else if (justFirst && !string.IsNullOrEmpty(skipChars) && skipChars.IndexOf(sn[i]) == -1)
                {
                    return "";
                }
            }

            if (e == 0)
            {
                if (values is object)
                {
                    values = Array.Empty<string>();
                }

                return Constants.vbNullString;
            }

            if (e < c)
            {
                Array.Resize(ref sc, e + 1);
            }

            if (values is object)
            {
                values = BatchParse(Conversions.ToString(sc), " ");
            }

            return Conversions.ToString(sc);
        }

        /// <summary>
        /// A better Val() function.  Will parse hexadecimal with 0x or &amp;H markers, octal with &amp;O markers or binary digits with a 'b' marker.
        /// </summary>
        /// <param name="value">The string value to parse.</param>
        /// <returns>An numeric primitive (either a Long or a Double).</returns>
        /// <remarks></remarks>
        public static object FVal(string value)
        {
            object FValRet = default;
            double o = 0.0d;
            float so = 0.0f;
            long n = 0L;
            value = Microsoft.VisualBasic.Strings.Trim(value);
            if (value.Length < 2)
            {
                if (Information.IsNumeric(value) == false)
                    return null;
                if (double.TryParse(value, out o))
                    return o;
            }

            if (value.Substring(0, 2) == "&H" | value.Substring(0, 2) == "0x")
            {
                value = value.Substring(2);
                return long.Parse(value, System.Globalization.NumberStyles.AllowHexSpecifier);
            }
            else if (value.Substring(value.Length - 1, 1) == "h")
            {
                value = value.Substring(0, value.Length - 1);
                return long.Parse(value, System.Globalization.NumberStyles.AllowHexSpecifier);
            }
            else if (value.Substring(0, 1) == "h")
            {
                value = value.Substring(1);
                return long.Parse(value, System.Globalization.NumberStyles.AllowHexSpecifier);
            }
            else if (value.Substring(0, 2) == "&O")
            {
                return (long)Conversion.Val(value);
            }
            else if (Conversions.ToString(value.ToLower()[0]) == "b")
            {
                int i;
                int c;
                char[] ch;
                long v = 0L;
                ch = value.Substring(1).ToCharArray();
                c = ch.Length - 1;
                var loopTo = c;
                for (i = 0; i <= loopTo; i++)
                {
                    if (Conversions.ToString(ch[i]) == "1")
                    {
                        v += 1L;
                    }
                    else if (Conversions.ToString(ch[i]) != "0")
                    {
                        return v;
                    }

                    v = v << 1;
                }

                return v;
            }
            else if (Conversions.ToString(value.ToLower()[value.Length - 1]) == "b")
            {
                int i;
                int c;
                char[] ch;
                long v = 0L;
                ch = value.Substring(0, value.Length - 1).ToCharArray();
                c = ch.Length - 1;
                var loopTo1 = c;
                for (i = 0; i <= loopTo1; i++)
                {
                    if (Conversions.ToString(ch[i]) == "1")
                    {
                        v += 1L;
                    }
                    else if (Conversions.ToString(ch[i]) != "0")
                    {
                        return v;
                    }

                    v = v << 1;
                }

                return v;
            }

            n = value.IndexOf(" ");
            if (n != -1)
            {
                string[] argvalues = null;
                value = JustNumbers(value.Substring(0, (int)n), values: ref argvalues);
            }

            if (Information.IsNumeric(value) == false)
                return null;
            if (double.TryParse(value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CurrentCulture.NumberFormat, out o))
                return o;
            try
            {
                FValRet = Conversion.Val(value);
            }
            catch (Exception ex)
            {
                return null;
            }

            return FValRet;
        }


        /// <summary>
        /// Escape text for use in a CSV file.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string TextEscapeCSV(string s)
        {
            char[] b;
            int i;
            if (s is null)
                return "";
            var sb = new StringBuilder();
            b = s.ToCharArray();
            var loopTo = b.Length - 1;
            for (i = 0; i <= loopTo; i++)
            {
                switch (b[i])
                {
                    case '"':
                        {
                            sb.Append("\"\"");
                            break;
                        }

                    case Constants.vbCr:
                        {
                            sb.Append(Constants.vbCr);
                            break;
                        }

                    case Constants.vbLf:
                        {
                            break;
                        }

                    default:
                        {
                            sb.Append(b[i]);
                            break;
                        }
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Escape text for use in a Json file.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string TextEscapeJson(string s)
        {
            char[] b;
            int i;
            var sOut = new StringBuilder();
            if (s is null)
                return "";
            b = s.ToCharArray();
            var loopTo = b.Length - 1;
            for (i = 0; i <= loopTo; i++)
            {
                if (i < b.Length - 1)
                {
                    if (Conversions.ToString(b[i]) == @"\" && Conversions.ToString(b[i + 1]) == "\"")
                    {
                        sOut.Append(@"\""");
                        i += 1;
                        continue;
                    }
                }

                switch (b[i])
                {
                    case '\\':
                    case '"':
                    case '/':
                        {
                            sOut.Append(@"\");
                            break;
                        }
                }

                sOut.Append(b[i]);
            }

            return sOut.ToString();
        }

        public static XMLTAG GetXMLTag(char[] a, int b, [Optional, DefaultParameterValue(null)] ref object e)
        {
            int i;
            int j = 0;
            string sName = "";
            ATTRVAL[] sAtt = null;
            var tOut = new XMLTAG();
            bool iaN = false;
            bool iaK = false;
            bool iaV = false;
            bool iaQ = false;
            bool iX = false;
            var loopTo = a.Length - 1;
            for (i = b; i <= loopTo; i++)
            {
                if (Conversions.ToString(a[i]) == "<" & iaQ == false)
                {
                    iaN = true;
                    iX = true;
                }
                else
                {
                    if (Conversions.ToString(a[i]) == ">" & iaV == false & iaQ == false)
                    {
                        iX = false;
                        break;
                    }

                    if (iX == true)
                    {
                        if (iaN == true)
                        {
                            if (Conversions.ToString(a[i]) == " ")
                            {
                                iaN = false;
                            }
                            else
                            {
                                sName += Conversions.ToString(a[i]);
                            }
                        }
                        else
                        {
                            if (iaV == false & iaK == false)
                            {
                                if (Conversions.ToString(a[i]) != " ")
                                {
                                    iaK = true;
                                    Array.Resize(ref sAtt, j + 1);
                                }
                            }
                            else if (iaK == true & Conversions.ToString(a[i]) == "=")
                            {
                                iaK = false;
                                iaV = true;
                                continue;
                            }
                            else if (iaK == true & Conversions.ToString(a[i]) == " ")
                            {
                                j += 1;
                                iaK = false;
                                continue;
                            }

                            if (iaV == true)
                            {
                                if (a[i] == vbDblQuote)
                                {
                                    if (iaQ == true)
                                    {
                                        iaQ = false;
                                        iaV = false;
                                    }
                                    else
                                    {
                                        iaQ = true;
                                        i += 1;
                                    }
                                }
                                else if (Conversions.ToString(a[i]) == " ")
                                {
                                    if (iaQ == false)
                                    {
                                        iaV = false;
                                    }
                                }
                            }

                            if (Conversions.ToString(a[i]) == ">")
                            {
                                if (iaQ == false)
                                {
                                    iaV = false;
                                    iX = false;
                                    j += 1;
                                    continue;
                                }
                            }

                            if (iaK == true)
                            {
                                sAtt[j].Name += Conversions.ToString(a[i]);
                            }
                            else if (iaV == true)
                            {
                                sAtt[j].Value += Conversions.ToString(a[i]);
                            }
                            else if (iaK == false & iaV == false & Conversions.ToString(a[i]) != " ")
                            {
                                j += 1;
                            }
                        }
                    }
                }
            }

            tOut.Name = sName;
            tOut.Attributes = sAtt;
            if (e is object)
            {
                e = i - 1;
            }

            return tOut;
        }

        /// <summary>
        /// Clear all null characters from a string
        /// </summary>
        /// <param name="input">String to process.</param>
        /// <returns>Processed text.</returns>
        /// <remarks></remarks>
        public static string RemoveNulls(string input)
        {
            return input.Trim('\0');
        }

        /// <summary>
        /// Reduces extraneous spacing, and ensures only one space exists at any given place.
        /// </summary>
        /// <param name="input">The string to process.</param>
        /// <param name="spaceChars">The characters to interpret as space characters.</param>
        /// <param name="PreserveQuotedText">Whether to preserve multiple spaces within quoted text.</param>
        /// <param name="quoteChar">The quote character to use for determining the location of quoted text.</param>
        /// <param name="escapeChar">The escape character to use to recognize the escaping of quote characters.</param>
        /// <param name="Quick">Whether to perform a quick search and replace.  If this parameter is set to true, all other optional parameters are ignored.</param>
        /// <returns>Processed text.</returns>
        /// <remarks></remarks>
        public static string OneSpace(string input, string spaceChars = " ", bool PreserveQuotedText = true, char quoteChar = vbDblQuote, char escapeChar = '\\', bool Quick = true)
        {
            string OneSpaceRet = default;
            int a;
            int b;
            var varOut = new StringBuilder();
            var isP = default(bool);
            var iQ = default(bool);
            char[] ch = input.ToCharArray();
            if (Quick)
            {
                if (input.IndexOf("  ") == -1)
                {
                    return input;
                }

                while (input.IndexOf("  ") != -1)
                    input = input.Replace("  ", " ");
                return input;
            }

            b = ch.Length - 1;
            var loopTo = b;
            for (a = 0; a <= loopTo; a++)
            {
                if (iQ == true)
                {
                    varOut.Append(ch[a]);
                    if (ch[a] == quoteChar)
                    {
                        if (a > 0)
                        {
                            if (ch[a - 1] == escapeChar)
                            {
                                continue;
                            }
                        }

                        iQ = false;
                    }
                }
                else if (spaceChars.IndexOf(ch[a]) != -1 & isP == false)
                {
                    isP = true;
                    varOut.Append(ch[a]);
                }
                else if (spaceChars.IndexOf(ch[a]) == -1)
                {
                    varOut.Append(ch[a]);
                    if (isP == true)
                        isP = false;
                    if (ch[a] == quoteChar & PreserveQuotedText == true)
                    {
                        iQ = true;
                    }
                }
            }

            OneSpaceRet = varOut.ToString();
            return OneSpaceRet;
        }

        /// <summary>
        /// Get all text within the first occurance of a specified bracket set.  Discards text outside.
        /// </summary>
        /// <param name="szText">String to scan.</param>
        /// <param name="startIndex">Index in the string at which to start scanning.</param>
        /// <param name="newIndex">Receives the index of the first character after the closing bracket.</param>
        /// <param name="BracketPair">Bracket pair (must consist of exactly 2 characters, for other division pairs, use <see cref="TextBetween"/>.)</param>
        /// <param name="ErrorText"></param>
        /// <returns>The text inside the first complete bracket, excluding the outer-most pair.</returns>
        /// <remarks></remarks>
        public static string Bracket(string szText, [Optional, DefaultParameterValue(default(int))] ref int startIndex, [Optional, DefaultParameterValue(default(int))] ref int newIndex, string BracketPair = "()", [Optional, DefaultParameterValue(null)] ref string ErrorText)
        {
            // ' returns the text inside the first complete bracket, excluding the outer-most pair. newIndex is set to the first character after the closing bracket

            char[] ch = szText.ToCharArray();
            int i = 0;
            int c = ch.Length - 1;
            string sOut = "";
            int n = 0;
            int bc = 0;
            int ec = 0;
            string open;
            string close;
            if (BracketPair.Length != 2)
            {
                ErrorText = "Invalid bracket pair string. The bracket pair string must consist of exactly one open character and exactly one close character.";
                return null;
            }

            open = Conversions.ToString(BracketPair[0]);
            close = Conversions.ToString(BracketPair[1]);
            try
            {
                newIndex = startIndex;
                if (string.IsNullOrEmpty(szText))
                    return "";
                i = szText.IndexOf(open, startIndex);
                if (i == -1)
                {
                    // ' there are no brackets, so we shall assume that entire string from startIndex to finish is a "bracket",
                    // ' we'll return that text and set newIndex to the end of the line

                    if (newIndex != default)
                        newIndex = 0;
                    i = szText.IndexOf(close);
                    if (i != -1)
                    {
                        ErrorText = "Syntax error.  Unexpected closing bracket at column " + (i + 1);
                        return null;
                    }

                    newIndex = ch.Length;
                    return szText.Substring(startIndex);
                }

                var loopTo = c;
                for (n = i; n <= loopTo; n++)
                {
                    sOut += Conversions.ToString(ch[n]);
                    if (Conversions.ToString(ch[n]) == open)
                    {
                        bc += 1;
                    }
                    else if (Conversions.ToString(ch[n]) == close)
                    {
                        bc -= 1;
                        if (bc < 0)
                        {
                            ErrorText = "Syntax error.  Unexpected closing bracket at column " + (i + 1);
                            return null;
                        }
                        else if (bc == 0)
                        {
                            break;
                        }
                    }
                }

                if (bc != 0)
                {
                    ErrorText = "Syntax error.  Unmatched closing bracket at column " + (i + 1);
                    return null;
                }

                sOut = sOut.Substring(1, sOut.Length - 2);
                newIndex = n + 1;
                startIndex = i;
                return sOut;
            }
            catch (Exception ex)
            {
                ErrorText = ex.Message;
                return null;
            }
        }


        /// <summary>
        /// Space out operators in preparation for mathematical parsing.
        /// </summary>
        /// <param name="value">The text to process.</param>
        /// <param name="Operators">The list of operator characters to use.</param>
        /// <param name="SepChars">The list of separator characters to use (default is all white space characters in the current culture).</param>
        /// <param name="StickyCharsLeft">Character operators that should stick to the text on their right if it is adjacent (not separated by a separator character).</param>
        /// <param name="StickyCharsRight">Character operators that should stick to the text on their left if it is adjacent (not separated by a separator character).</param>
        /// <param name="NoStickyChars">Characters that under no circumstances should stick to adjacent characters.</param>
        /// <returns>Processed text.</returns>
        /// <remarks></remarks>
        public static string SpaceOperators(string value, string Operators = @"/\&^%*-+()[]{}", string SepChars = null, string StickyCharsLeft = "", string StickyCharsRight = "+-", string NoStickyChars = "")
        {
            int i;
            int c;
            string s;
            char ch;
            bool inq = false;
            char[] sp;
            if (SepChars == null)
            {
                SepChars = "";
                for (i = 0; i <= 255; i++)
                {
                    ch = (char)i;
                    if (char.IsWhiteSpace(ch))
                    {
                        SepChars += Conversions.ToString(ch);
                    }
                }
            }

            if (string.IsNullOrEmpty(SepChars))
                return value;
            sp = value.ToCharArray();
            c = sp.Length - 1;
            s = "";
            if (StickyCharsLeft == null)
                StickyCharsLeft = "";
            if (StickyCharsRight == null)
                StickyCharsRight = "";
            if (NoStickyChars == null)
                NoStickyChars = "";
            var loopTo = c;
            for (i = 0; i <= loopTo; i++)
            {
                if (sp[i] == vbDblQuote)
                {
                    inq = !inq;
                }

                if (!inq && Operators.IndexOf(sp[i]) != -1)
                {
                    if (i > 0 && Operators.IndexOf(sp[i - 1]) != -1 && NoStickyChars.IndexOf(sp[i]) == -1 && NoStickyChars.IndexOf(sp[i - 1]) == -1)
                    {
                        if (StickyCharsRight.IndexOf(sp[i - 1]) <= -1)
                        {
                            s = s.Substring(0, s.Length - 1);
                        }

                        s += Conversions.ToString(sp[i]);
                    }
                    else
                    {
                        if (StickyCharsLeft.IndexOf(sp[i]) == -1 || NoStickyChars.IndexOf(sp[i]) != -1)
                        {
                            s += " ";
                        }

                        s += Conversions.ToString(sp[i]);
                    }

                    if (StickyCharsRight.IndexOf(sp[i]) == -1 || NoStickyChars.IndexOf(sp[i]) != -1)
                    {
                        s += " ";
                    }
                }
                else
                {
                    s += Conversions.ToString(sp[i]);
                }
            }

            return Microsoft.VisualBasic.Strings.Trim(OneSpace(s, SepChars));
        }

        /// <summary>
        /// Returns all words in a string as an array of strings.
        /// </summary>
        /// <param name="Text">Text to split.</param>
        /// <param name="SepChars">Separator characters to use.</param>
        /// <param name="AdditionalSepChars">Any additional separator characters to use.</param>
        /// <param name="SkipQuotes">Skip over quoted text.</param>
        /// <param name="Unescape">Unescape quoted text.</param>
        /// <param name="QuoteChar">Quote character to use.</param>
        /// <param name="EscapeChar">Escape character to use.</param>
        /// <returns>Array of strings.</returns>
        /// <remarks></remarks>
        public static string[] Words(string Text, string SepChars = null, string AdditionalSepChars = null, bool SkipQuotes = false, bool Unescape = false, string QuoteChar = vbDblQuote, string EscapeChar = @"\")
        {
            int i;
            int c;
            int n;
            string s;
            char ch;
            string[] stout = null;
            string[] stwork = null;
            string[] stwork2 = null;
            char[] sep;
            if (SepChars == null)
            {
                SepChars = "";
                for (i = 0; i <= 255; i++)
                {
                    ch = (char)i;
                    if (char.IsWhiteSpace(ch))
                    {
                        if (Conversions.ToString(ch) != Constants.vbCr && Conversions.ToString(ch) != Constants.vbLf)
                            SepChars += Conversions.ToString(ch);
                    }
                }
            }

            if (string.IsNullOrEmpty(SepChars))
                return new[] { Text };
            sep = SepChars.ToCharArray();
            if (AdditionalSepChars != null)
            {
                sep = sep + AdditionalSepChars.ToCharArray();
            }

            ch = sep[0];
            s = Text;
            s = Microsoft.VisualBasic.Strings.Trim(OneSpace(s, Conversions.ToString(ch), SkipQuotes));
            stout = BatchParse(s, Conversions.ToString(ch), SkipQuotes, Unescape, Conversions.ToChar(QuoteChar), Conversions.ToChar(EscapeChar));
            if (stout is null)
                return new[] { s };
            c = stout.Length - 1;
            if (SepChars.Length == 1)
            {
                return stout;
            }

            SepChars = SepChars.Substring(1);
            var loopTo = c;
            for (i = 0; i <= loopTo; i++)
            {
                stwork = Words(stout[i], SepChars, SkipQuotes: SkipQuotes, Unescape: Unescape, QuoteChar: QuoteChar, EscapeChar: EscapeChar);
                if (stwork2 is null)
                {
                    stwork2 = stwork;
                    continue;
                }

                n = stwork2.Length;
                Array.Resize(ref stwork2, n + stwork.Length);
                Array.Copy(stwork, 0, stwork2, n, stwork.Length);
            }

            return stwork2;
        }

        /// <summary>
        /// Wrap the input text by the given columns.
        /// </summary>
        /// <param name="szText">Text to wrap.</param>
        /// <param name="Cols">Maximum character columns.</param>
        /// <returns>Wrapped text.</returns>
        /// <remarks></remarks>
        public static string Wrap(string szText, int Cols = 60)
        {
            string[] st;
            int xTot = 0;
            string sOut = "";
            int i;
            int j;
            if (szText.Length < Cols)
                return szText;
            st = Words(szText);
            var loopTo = st.Length - 1;
            for (i = 0; i <= loopTo; i++)
            {
                if (st[i].Length >= Cols)
                {
                    sOut += Constants.vbCrLf;
                    j = 0;
                    while (j < st[i].Length)
                    {
                        try
                        {
                            sOut += st[i].Substring(j, Cols);
                        }
                        catch (Exception ex)
                        {
                            sOut += st[i].Substring(j);
                        }

                        j += Cols;
                        sOut += Constants.vbCrLf;
                    }

                    xTot = 0;
                    continue;
                }
                else if (xTot + st[i].Length > Cols)
                {
                    sOut += Constants.vbCrLf;
                    xTot = 0;
                }

                xTot += st[i].Length;
                sOut += st[i];
                if (i < st.Length - 1)
                {
                    xTot += 1;
                    sOut += " ";
                }
            }

            return sOut;
        }

        /// <summary>
        /// Returns all lines in a string.
        /// </summary>
        /// <param name="szText"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string[] GetLines(string szText)
        {
            szText = szText.Replace(Constants.vbCrLf, Constants.vbLf);
            szText = szText.Replace(Constants.vbCr, Constants.vbLf);
            szText = szText.Replace(Constants.vbLf, Conversions.ToString('\0'));
            return szText.Split('\0');
        }

        /// <summary>
        /// Return a padded hexadecimal value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="width"></param>
        /// <param name="prefix"></param>
        /// <param name="lowercase"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string PadHex(object value, int width = 8, string prefix = "", bool lowercase = false)
        {
            string s = Conversion.Hex(value);
            if (lowercase)
                s = s.ToLower();
            if (width - s.Length > 0)
            {
                return prefix + new string('0', width - s.Length) + s;
            }
            else
            {
                return prefix + s;
            }
        }

        /// <summary>
        /// Returns true if the value in a hexadecimal number. Accepts &amp;H and 0x prefixes.
        /// </summary>
        /// <param name="hin">String to scan</param>
        /// <param name="value">Optionally receives the parsed value.</param>
        /// <returns>True if the string can be parsed as hex.</returns>
        /// <remarks></remarks>
        public static bool IsHex(string hin, [Optional, DefaultParameterValue(default(int))] ref int value)
        {
            char[] b;
            int i;
            bool c = true;
            b = hin.ToCharArray();
            if (hin.IndexOf("&H") == -1 & hin.IndexOf("0x") == -1)
                return false;
            hin = hin.Replace("&H", "");
            hin = hin.Replace("0x", "");
            var loopTo = b.Length - 1;
            for (i = 0; i <= loopTo; i++)
            {
                switch (b[i])
                {
                    case 'a':
                    case 'A':
                    case 'b':
                    case 'B':
                    case 'c':
                    case 'C':
                    case 'd':
                    case 'D':
                    case 'e':
                    case 'E':
                    case 'f':
                    case 'F':
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        {
                            break;
                        }

                    default:
                        {
                            c = false;
                            break;
                        }
                }
            }

            if (c == true & value != default)
            {
                value = Conversions.ToInteger("&H" + hin);
            }

            return c;
        }

        /// <summary>
        /// Removes comments from a line of text.
        /// </summary>
        /// <param name="input">Text to parse.</param>
        /// <param name="commentchar">Comment marker</param>
        /// <returns>A string with no comments.</returns>
        /// <remarks></remarks>
        public static string NoComment(string input, string commentchar = "//")
        {
            int a;
            int b;
            string varOut = "";
            var isP = default(bool);
            var iQ = default(bool);
            ;
#error Cannot convert OnErrorResumeNextStatementSyntax - see comment for details
            /* Cannot convert OnErrorResumeNextStatementSyntax, CONVERSION ERROR: Conversion for OnErrorResumeNextStatement not implemented, please report this issue in 'On Error Resume Next' at character 53972


            Input:

                        On Error Resume Next

             */
            b = input.Length - 1;
            var loopTo = b;
            for (a = 0; a <= loopTo; a++)
            {
                if (iQ == true)
                {
                    varOut += Conversions.ToString(input[a]);
                    if (input[a] == vbDblQuote)
                    {
                        iQ = false;
                    }
                }
                else
                {
                    if (a < b - commentchar.Length)
                    {
                        if ((input.Substring(a, commentchar.Length) ?? "") == (commentchar ?? ""))
                        {
                            break;
                        }
                    }

                    varOut += Conversions.ToString(input[a]);
                    if (isP == true)
                        isP = false;
                    if (input[a] == vbDblQuote)
                    {
                        iQ = true;
                    }
                }
            }

            return varOut;
        }

        public static string FieldToProp(string fieldname)
        {
            return fieldname.Substring(1);
        }

        public static string PropToField(string propname)
        {
            return "_" + propname;
        }

        /// <summary>
        /// Space out a camel-cased string.
        /// </summary>
        /// <param name="value">String to separate.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string SeparateCamel(string value)
        {
            char[] ch = value.ToCharArray();
            int i;
            int c;
            var sb = new StringBuilder();
            c = ch.Length - 1;
            var loopTo = c;
            for (i = 0; i <= loopTo; i++)
            {
                if ((ch[i].ToString().ToUpper() ?? "") == (ch[i].ToString() ?? "") && i > 0 && IsNumber(Conversions.ToString(ch[i])) == false)
                {
                    sb.Append(" ");
                }

                sb.Append(ch[i]);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Convert spaced or underscored lines to camel case. This function is an alias for TitleCase.
        /// </summary>
        /// <param name="input">The input text</param>
        /// <param name="stripSpaces">Strip spaces and present a camel-case version of the string.</param>
        /// <param name="noDecaps">Do not decapitalize already capitalized characters if they occur alone.</param>
        /// <returns>A newly formatted string</returns>
        /// <remarks></remarks>
        public static string CamelCase(string input, bool stripSpaces = false, bool noDecaps = false)
        {
            return TitleCase(input, stripSpaces, noDecaps);
        }

        /// <summary>
        /// Convert spaced or underscored lines to camel case.
        /// </summary>
        /// <param name="input">The input text</param>
        /// <param name="stripSpaces">Strip spaces and present a camel-case version of the string.</param>
        /// <param name="noDecaps">Do not decapitalize already capitalized characters if they occur alone.</param>
        /// <returns>A newly formatted string.</returns>
        /// <remarks></remarks>
        public static string TitleCase(string input, bool stripSpaces = false, bool noDecaps = false)
        {
            int a;
            int b;
            string varOut = "";
            var isP = default(bool);
            var iQ = default(bool);
            bool dec = false;
            if (input == null)
                return "";
            input = SearchReplace(input, "_", " ");
            b = input.Length - 1;
            var loopTo = b;
            for (a = 0; a <= loopTo; a++)
            {
                if (iQ == true)
                {
                    varOut += Conversions.ToString(input[a]);
                    if (input[a] == vbDblQuote)
                    {
                        iQ = false;
                    }
                }
                else if ((input[a] == ' ' || Conversions.ToString(input[a]) == "-") && isP == false)
                {
                    isP = true;
                    if (Conversions.ToString(input[a]) == "-")
                        varOut += "-";
                    else
                        varOut += " ";
                }
                else if (input[a] != ' ' && Conversions.ToString(input[a]) != "-")
                {
                    if ((Conversions.ToString(input[a]) ?? "") == (Conversions.ToString(input[a]).ToUpper() ?? "") && noDecaps && dec == false)
                    {
                        isP = true;
                        dec = true;
                    }
                    else if (dec == true)
                    {
                        dec = false;
                    }

                    if (a == 0 | isP == true)
                    {
                        varOut += input[a].ToString().ToUpper();
                    }
                    else if (isP == false)
                    {
                        varOut += input[a].ToString().ToLower();
                    }

                    if (isP == true)
                        isP = false;
                    if (input[a] == vbDblQuote)
                    {
                        iQ = true;
                    }
                }
            }

            if (stripSpaces)
                return SearchReplace(varOut, " ", "");
            else
                return varOut;
        }

        /// <summary>
        /// Concats all strings in a string array into one string.
        /// </summary>
        /// <param name="Text">Array to combine.</param>
        /// <returns>A string.</returns>
        /// <remarks></remarks>
        public static string Stream(string[] Text)
        {
            string StreamRet = default;
            long i;
            long b;
            var sb = new StringBuilder();
            ;
#error Cannot convert OnErrorResumeNextStatementSyntax - see comment for details
            /* Cannot convert OnErrorResumeNextStatementSyntax, CONVERSION ERROR: Conversion for OnErrorResumeNextStatement not implemented, please report this issue in 'On Error Resume Next' at character 60011


            Input:
                        On Error Resume Next

             */
            StreamRet = "";
            i = -1&L;
            i = Information.UBound(Text);
            var loopTo = i;
            for (b = 0L; b <= loopTo; b++)
            {
                if (sb.Length != 0)
                    sb.Append(Constants.vbCrLf);
                sb.Append(Text[(int)b]);
            }

            StreamRet = sb.ToString();
            return StreamRet;
        }


        /// <summary>
        /// Filters text using odd pairs of characters, when the beginning and end bounds have different constituents.
        /// </summary>
        /// <param name="Text">The text to filter.</param>
        /// <param name="FilterPair">Exactly 2 characters that represent the pair to filter.</param>
        /// <param name="Escape">Escape character to use.</param>
        /// <param name="FirstIsFilter">Receives a value indicating that the first character of the input text was an opening filter character.</param>
        /// <returns>Filtered text.</returns>
        /// <remarks></remarks>
        public static string[] Filter(string Text, string FilterPair = vbDblQuote, string Escape = @"\", [Optional, DefaultParameterValue(default(bool))] ref bool FirstIsFilter, bool OnlyBetween = false)
        {
            string[] FilterRet = default;
            string[] lnOut = null;
            int i;
            int j;
            int l;
            int m;
            int n;
            char[] c;
            char[] fp;
            var e = default(bool);
            ;
#error Cannot convert OnErrorResumeNextStatementSyntax - see comment for details
            /* Cannot convert OnErrorResumeNextStatementSyntax, CONVERSION ERROR: Conversion for OnErrorResumeNextStatement not implemented, please report this issue in 'On Error Resume Next' at character 61576


            Input:

                        On Error Resume Next

             */
            if (FilterPair.Length == 0)
                FilterPair = Conversions.ToString(vbDblQuote);
            if (FilterPair.Length == 1)
            {
                FilterPair = FilterPair + FilterPair;
            }

            fp = FilterPair.ToCharArray();
            c = Text.ToCharArray();
            j = -1;
            j = Information.UBound(c);
            m = -1;
            var loopTo = j;
            for (i = 0; i <= loopTo; i++)
            {
                if (e == false)
                {
                    if (c[i] == fp[0])
                    {
                        if (m == -1)
                        {
                            FirstIsFilter = true;
                        }

                        m += 1;
                        e = true;
                        Array.Resize(ref lnOut, m + 1);
                        lnOut[m] = Conversions.ToString(c[i]);
                    }
                    else
                    {
                        if (m == -1)
                        {
                            FirstIsFilter = false;
                            m += 1;
                            Array.Resize(ref lnOut, m + 1);
                        }

                        lnOut[m] += Conversions.ToString(c[i]);
                    }
                }
                else if (c[i] == fp[1])
                {
                    if (i > 0)
                    {
                        if (!string.IsNullOrEmpty(Escape) & Escape != null)
                        {
                            if (c[i - 1] == Escape[0])
                            {
                                if (lnOut[m].Length > 1)
                                {
                                    lnOut[m] = lnOut[m].Substring(0, lnOut[m].Length - 1);
                                }
                            }
                        }
                    }

                    lnOut[m] += Conversions.ToString(c[i]);
                    m += 1;
                    e = false;
                    Array.Resize(ref lnOut, m + 1);
                }
                else
                {
                    lnOut[m] += Conversions.ToString(c[i]);
                }
            }

            FilterRet = lnOut;
            return FilterRet;
        }

        /// <summary>
        /// Replaces all instances of the Search string with the Replace string.
        /// </summary>
        /// <param name="InputStr">The string in which to search.</param>
        /// <param name="Search">The search string.</param>
        /// <param name="Replace">The replacement string.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string SearchReplace(string InputStr, string Search, string Replace)
        {
            string SearchReplaceRet = default;
            string sOut;
            int e = 0;
            sOut = InputStr;
            if (sOut is null || sOut.Length == 0)
                return "";
            while (e != -1)
            {
                e = sOut.IndexOf(Search);
                if (e >= 0)
                    sOut = sOut.Replace(Search, Replace);
                else
                    break;
            }

            SearchReplaceRet = sOut;
            return SearchReplaceRet;
        }

        /// <summary>
        /// Returns the string in double quotes.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string DblQuote(string value)
        {
            return vbDblQuote + value + vbDblQuote;
        }

        /// <summary>
        /// Removes all blank lines from a string.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string NoNullLines(string i)
        {
            string[] st;
            int l;
            var sOut = new StringBuilder();
            i = SearchReplace(i, Constants.vbCrLf, Constants.vbCr);
            i = SearchReplace(i, Constants.vbLf, Constants.vbCr);
            st = BatchParse(i, Constants.vbCr);
            var loopTo = st.Length - 1;
            for (l = 0; l <= loopTo; l++)
            {
                if (!string.IsNullOrEmpty(st[l]))
                {
                    if (sOut.Length != 0)
                        sOut.Append(Constants.vbCrLf);
                    sOut.Append(st[l]);
                }
            }

            return sOut.ToString();
        }

        /// <summary>
        /// Returns 'true' if the string is in all capitals.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsCaps(string text)
        {
            return (text ?? "") == (text.ToUpper() ?? "");
        }

        public struct HTMLTAG
        {
            public string Name;
            public string[] Attributes;
            public string Style;
        }

        /// <summary>
        /// Trims internal white-space characters inside of a string to a single space.
        /// </summary>
        /// <param name="val">String to trim.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string TrimWSpace(string val)
        {
            string TrimWSpaceRet = default;
            long i;
            long j;
            var sStr = new StringBuilder();
            var sp = default(bool);
            j = val.Length - 1;
            var loopTo = j;
            for (i = 0L; i <= loopTo; i++)
            {
                if (Conversions.ToString(val[(int)i]) == " ")
                {
                    if (sp == false)
                    {
                        sp = true;
                    }
                }
                else
                {
                    if (sp == true)
                    {
                        sStr.Append(" ");
                    }

                    sStr.Append(val[(int)i]);
                    sp = false;
                }
            }

            TrimWSpaceRet = sStr.ToString();
            return TrimWSpaceRet;
        }

        /// <summary>
        /// String Token Function (works like in C/C++).  Note: not thread-safe when used with SyncTok.  Enclose in a class to make thread-safe.
        /// </summary>
        /// <param name="ScanString">The string to scan.</param>
        /// <param name="Token">The token/separator to use.</param>
        /// <param name="SyncTok">Whether to use synchronized static storage (for use with BatchParse).</param>
        /// <param name="SkipQuotes">Whether to skip over quoted text.</param>
        /// <param name="QuoteChar">The quote character to use.</param>
        /// <param name="EscapeChar">The character used to escape quotation marks.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string StrTok(string ScanString = "", string Token = "", bool SyncTok = false, bool SkipQuotes = false, string QuoteChar = vbDblQuote, string EscapeChar = @"\")
        {
            string StrTokRet = default;
            ;
#error Cannot convert LocalDeclarationStatementSyntax - see comment for details
            /* Cannot convert LocalDeclarationStatementSyntax, System.NotSupportedException: StaticKeyword not supported!
               at ICSharpCode.CodeConverter.CSharp.SyntaxKindExtensions.ConvertToken(SyntaxKind t, TokenContext context)
               at ICSharpCode.CodeConverter.CSharp.CommonConversions.ConvertModifier(SyntaxToken m, TokenContext context)
               at ICSharpCode.CodeConverter.CSharp.CommonConversions.<ConvertModifiersCore>d__43.MoveNext()
               at System.Linq.Enumerable.<ConcatIterator>d__59`1.MoveNext()
               at System.Linq.Enumerable.WhereEnumerableIterator`1.MoveNext()
               at System.Linq.Buffer`1..ctor(IEnumerable`1 source)
               at System.Linq.OrderedEnumerable`1.<GetEnumerator>d__1.MoveNext()
               at Microsoft.CodeAnalysis.SyntaxTokenList.CreateNode(IEnumerable`1 tokens)
               at ICSharpCode.CodeConverter.CSharp.CommonConversions.ConvertModifiers(SyntaxNode node, IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isVariableOrConst, SyntaxKind[] extraCsModifierKinds)
               at ICSharpCode.CodeConverter.CSharp.MethodBodyExecutableStatementVisitor.<VisitLocalDeclarationStatement>d__31.MoveNext()
            --- End of stack trace from previous location where exception was thrown ---
               at ICSharpCode.CodeConverter.CSharp.HoistedNodeStateVisitor.<AddLocalVariablesAsync>d__6.MoveNext()
            --- End of stack trace from previous location where exception was thrown ---
               at ICSharpCode.CodeConverter.CSharp.CommentConvertingMethodBodyVisitor.<DefaultVisitInnerAsync>d__3.MoveNext()

            Input:
                        Static Stored As String = Nothing

             */
            long i;
            long l;
            string vCh;
            string OutStr = "";
            long sLen;
            var inQ = default(bool);
            bool skipQ;
            string qChar;
            string eChar;
            ;
#error Cannot convert OnErrorResumeNextStatementSyntax - see comment for details
            /* Cannot convert OnErrorResumeNextStatementSyntax, CONVERSION ERROR: Conversion for OnErrorResumeNextStatement not implemented, please report this issue in 'On Error Resume Next' at character 69005


            Input:

                        On Error Resume Next

             */
            StrTokRet = "";
            if (QuoteChar == null)
                qChar = Conversions.ToString(vbDblQuote);
            else
                qChar = QuoteChar;
            if (EscapeChar == null)
                eChar = @"\";
            else
                eChar = EscapeChar;
            if (SyncTok == true)
            {
                if (string.IsNullOrEmpty(ScanString))
                {
                    StrTokRet = Stored;
                }
                else
                {
                    Stored = ScanString;
                }

                return StrTokRet;
            }

            if (string.IsNullOrEmpty(Token))
                return null;
            if (!string.IsNullOrEmpty(ScanString))
            {
                Stored = ScanString;
            }

            if (string.IsNullOrEmpty(Stored))
                return null;
            if (Stored.Length >= Token.Length)
            {
                if ((Stored.Substring(0, Token.Length - 1) ?? "") == (Token ?? ""))
                {
                    Stored = Stored.Substring(Token.Length - 1);
                    StrTokRet = "";
                    return StrTokRet;
                }
            }

            skipQ = SkipQuotes;
            sLen = (long)(Stored.Length - 1);
            var loopTo = sLen;
            for (i = 0L; i <= loopTo; i++)
            {
                vCh = Stored.Substring((int)i, Microsoft.VisualBasic.Strings.Len(Token));
                if ((vCh ?? "") == (Token ?? "") & (inQ == false | skipQ == false))
                {
                    if (i + Microsoft.VisualBasic.Strings.Len(Token) <= sLen)
                    {
                        StrTokRet = OutStr;
                        Stored = Stored.Substring((int)(i + Microsoft.VisualBasic.Strings.Len(Token)));
                        return StrTokRet;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    vCh = Conversions.ToString(Stored[(int)i]);
                    OutStr += vCh;
                    if (qChar.IndexOf(vCh) > -1 & skipQ == true)
                    {
                        inQ = !inQ;
                        if (i > 0L)
                        {
                            vCh = Conversions.ToString(Stored[(int)(i - 1L)]);
                            if ((vCh ?? "") == (eChar ?? ""))
                            {
                                inQ = !inQ;
                            }
                        }
                    }
                }
            }

            Stored = null;
            return OutStr;
        }


        /// <summary>
        /// Function to retrieve a quote from a string of data.
        /// The quote character must be: exactly one before, exactly at, or anywhere after the location specified by 'Pos'.
        /// Text outside of the first quoted string is discarded.
        /// </summary>
        /// <param name="StrData">The string to scan.</param>
        /// <param name="Pos">The position to begin scanning.</param>
        /// <param name="QuoteChar">The quote character to use.</param>
        /// <param name="EscapeChar">The escape character to use.</param>
        /// <param name="WithQuotes">Return the string in quotes.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string QuoteFromHere(string StrData, long Pos, string QuoteChar = vbDblQuote, string EscapeChar = @"\", bool WithQuotes = false)
        {
            string QuoteFromHereRet = default;
            long i;
            long l;
            string vCh;
            var OutStr = new StringBuilder();
            var inQ = default(bool);
            string qChar;
            string eChar;
            char qch;
            if (string.IsNullOrEmpty(QuoteChar))
                qChar = Conversions.ToString(vbDblQuote);
            else
                qChar = QuoteChar;
            eChar = EscapeChar;
            qch = qChar[0];
            if (string.IsNullOrEmpty(eChar))
                eChar = @"\";
            if (Pos > 0L)
                l = Pos - 1L;
            else
                l = Pos;
            foreach (char ch in QuoteChar)
            {
                l = StrData.IndexOf(ch);
                if (l > -1)
                    break;
            }

            if (l == -1)
                return null;
            var loopTo = (long)Microsoft.VisualBasic.Strings.Len(StrData);
            for (i = l; i <= loopTo; i++)
            {
                vCh = Conversions.ToString(StrData[(int)i]);
                if (qChar.IndexOf(vCh) > -1)
                {
                    inQ = !inQ;
                    qch = Conversions.ToChar(vCh);
                    if (i > 0L)
                    {
                        if (i > 1L)
                        {
                            vCh = StrData.Substring((int)(i - 2L), 2);
                        }
                        else
                        {
                            vCh = Conversions.ToString(StrData[(int)(i - 1L)]);
                        }

                        if ((vCh.Substring(1) ?? "") == (eChar ?? "") & vCh.IndexOf(eChar + eChar) == -1)
                        {
                            OutStr.Append(qch);
                            inQ = !inQ;
                        }
                    }

                    if (inQ == false)
                        break;
                }
                else if ((vCh ?? "") == (eChar ?? ""))
                {
                    if (i > 0L)
                    {
                        vCh = Conversions.ToString(StrData[(int)(i - 1L)]);
                        if ((vCh ?? "") == (eChar ?? ""))
                        {
                            OutStr.Append(eChar);
                        }
                    }
                }
                else if ((vCh ?? "") != (eChar ?? ""))
                {
                    OutStr.Append(vCh);
                }
            }

            if (WithQuotes == true)
            {
                QuoteFromHereRet = Conversions.ToString(qch) + OutStr.ToString() + Conversions.ToString(qch);
            }
            else
            {
                QuoteFromHereRet = OutStr.ToString();
            }

            return QuoteFromHereRet;
        }

        /// <summary>
        /// A new implementation of BatchParse designed to be much faster (and thread-safe)
        /// See <see cref="BatchParse" /> for more information.
        /// </summary>
        /// <param name="Scan">String to scan</param>
        /// <param name="Separator">Separator string</param>
        /// <param name="SkipQuote">Whether to ignore separator strings within the quoted blocks.</param>
        /// <param name="Unescape">Whether to unescape quotes.</param>
        /// <param name="QuoteChar">Quote character to use.</param>
        /// <param name="EscapeChar">Escape character to use.</param>
        /// <param name="WithToken">Include the token in the return array.</param>
        /// <param name="WithTokenIn">Attach the token to the beginning of every string separated by a token (except for string 0).  Requires WithToken to also be set to True.</param>
        /// <param name="Unquote">Unquote quoted strings.</param>
        /// <returns>An array of strings.</returns>
        /// <remarks></remarks>
        public static string[] BatchParse(string Scan, string Separator, bool SkipQuote = false, bool Unescape = false, char QuoteChar = '"', char EscapeChar = '\\', bool WithToken = false, bool WithTokenIn = false, bool Unquote = false)
        {

            // ' abspos = absolute position
            // ' seppos = position if within a potential separator sequence
            // ' sepubound = highest possible index within the seperator sequence
            // ' fieldpos = character position relative to the field that is currently being parsed
            // ' saveidx = saved start index within a separator scan
            // ' outubound = the current upper bound of the output string array
            // ' inq = inside quotation boundary

            // ' chOut = character buffer
            // ' sOut = output string array
            // ' chrs = input buffer converted to a Char array
            // ' sep = input separator string converted to a Char array

            // ' Now, follow the bouncing ball. :-)  

            int abspos;
            int seppos;
            int sepubound;
            var fieldpos = default(int);
            string[] sOut = null;
            int ubound;
            var chOut = new StringBuilder();
            int saveidx = 0;
            int outubound = 0;
            bool inq = false;
            char[] chrs = Scan.ToCharArray();
            char[] sep = Separator.ToCharArray();
            ubound = chrs.Length - 1;
            sepubound = sep.Length - 1;
            seppos = 0;
            chOut.Capacity = chrs.Length;
            var loopTo = ubound;
            for (abspos = 0; abspos <= loopTo; abspos++)
            {
                if (SkipQuote && chrs[abspos] == QuoteChar)
                {

                    // ' meticulously check the next character for quotes, escapes, and quote-escape-quotes.
                    if (!inq && abspos < ubound && chrs[abspos] == QuoteChar && chrs[abspos + 1] == QuoteChar)
                    {
                        abspos += 1;

                        // ' check to see if the first character is also an escaped quote charter.
                        if (abspos < ubound && EscapeChar == QuoteChar)
                        {
                            if (chrs[abspos + 1] == QuoteChar)
                            {
                                // ' it is!  three quotes in a row!
                                // ' Okay, we can actually continue with this string, and we'll proceed down the code.
                                inq = true;
                            }
                            else
                            {
                                // ' nope, empty string ... Next!
                                continue;
                            }
                        }
                        else
                        {
                            // ' nope, empty string ... Next!
                            continue;
                        }
                    }

                    if (inq && abspos < ubound && chrs[abspos] == EscapeChar && chrs[abspos + 1] == QuoteChar)
                    {
                        if (Unescape)
                        {
                            // ' we just want the escaped character, we don't want the escape character.
                            abspos += 1;
                            chOut.Append(chrs[abspos]);
                            fieldpos += 1;
                        }
                        else
                        {
                            // ' we want the escape character in addition to the escaped character.
                            chOut.Append(chrs[abspos]);
                            fieldpos += 1;
                            abspos += 1;
                            chOut.Append(chrs[abspos]);
                            fieldpos += 1;

                            // ' don't advance abspos a second time, it gets advanced with the For ... Next
                        }

                        continue;
                    }

                    if (inq && chrs[abspos] == QuoteChar)
                    {
                        inq = false;
                        if (Unquote)
                        {
                            continue;
                        }
                    }
                    else if (inq == false)
                    {
                        inq = true;
                        if (Unquote)
                        {
                            continue;
                        }
                    }
                }

                if (!inq && chrs[abspos] == sep[seppos])
                {

                    // Save the starting index.

                    // In the case of multi-character separators, we need this to remember where we left off if the 
                    // characters that are found form an incomplete separator.

                    // For example, say we find a comma, where the separator is exactly a comma and a space... 
                    // we have to abort that and return us to our regularly scheduled programming ... 

                    // Conversely, if the separator is exactly one character, then the 'saveidx' variable is not used, at all.
                    saveidx = abspos;
                    var loopTo1 = sepubound;
                    for (seppos = 0; seppos <= loopTo1; seppos++)
                    {
                        if (abspos > ubound)
                            break;

                        // ' If we succeed to Exit For, here, we'll have to subtract 1 from the abspos, later... and we do.
                        if (chrs[abspos] == sep[seppos])
                            abspos += 1;
                        else
                            break;
                    }

                    if (seppos == sepubound + 1)
                    {

                        // ' the grand expansion.
                        Array.Resize(ref sOut, outubound + 1);

                        // ' if you need the tokens, themselves, attached to the output string array...
                        if (WithToken && WithTokenIn)
                        {
                            chOut.Append(sep);
                            fieldpos += sep.Length;
                            // For Each cs As Char In sep
                            // chOut.Append(cs)
                            // fieldpos += 1
                            // Next
                        }

                        // ' we've got a new field, and we're going to store it.
                        sOut[outubound] = chOut.ToString();
                        chOut.Clear();

                        // ' reset the field counter to zero
                        fieldpos = 0;

                        // ' increment the outbound array ubound
                        outubound += 1;

                        // ' If you need the tokens, themselves, intermingled with the output string array...
                        if (WithToken && !WithTokenIn)
                        {
                            Array.Resize(ref sOut, outubound + 1);
                            sOut[outubound] = Separator;
                            outubound += 1;
                        }

                        // ' Back-track from the overrun from earlier so that the For ... Next loop can advance the counter, and pick it up for the next field.
                        abspos -= 1;
                    }
                    else
                    {
                        // ' ho-hum character in a current field ... Next!
                        chOut.Append(chrs[saveidx]);
                        fieldpos += 1;
                        abspos = saveidx;
                    }

                    seppos = 0;
                }
                else
                {
                    // ' ho-hum character in a current field ... Next!
                    chOut.Append(chrs[abspos]);
                    fieldpos += 1;
                }
            }

            // ' wrap up the final field
            if (Conversions.ToBoolean(fieldpos))
            {
                Array.Resize(ref sOut, outubound + 1);
                sOut[outubound] = chOut.ToString();
                chOut.Clear();
            }

            // ' go home
            return sOut;
        }

        /// <summary>
        /// Link an array of strings together, separated by the specified separator.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string BatchLink(string[] input, string separator)
        {
            var sb = new StringBuilder();
            int i = 0;
            int c = input.Count() - 1;
            var loopTo = c;
            for (i = 0; i <= loopTo; i++)
            {
                if (i > 0)
                    sb.Append(separator);
                sb.Append(input[i]);
            }

            return sb.ToString();
        }


        /// <summary>
        /// Friendly size/speed kilo type.
        /// </summary>
        /// <remarks></remarks>
        public enum FriendlyKBType : ulong
        {

            /// <summary>
            /// The measurement is automatically determined.
            /// </summary>
            /// <remarks></remarks>
            Auto = 0UL,

            /// <summary>
            /// A kilobyte is 1024 bytes.
            /// </summary>
            /// <remarks></remarks>
            Kilo1024 = 1024UL,

            /// <summary>
            /// A kilobyte is 1000 bytes.
            /// </summary>
            /// <remarks></remarks>
            Kilo1000 = 1000UL
        }

        /// <summary>
        /// Prints a number value as a friendly byte size in TiB, GiB, MiB, KiB or B.
        /// </summary>
        /// <param name="size">The size to format.</param>
        /// <param name="format">Optional numeric format for the resulting value.</param>
        /// <param name="factor">The 1K measuring unit.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string PrintFriendlySize(ulong size, string format = null, FriendlyKBType factor = FriendlyKBType.Kilo1024, bool printKi = true)
        {
            double fs;
            string nom;
            string ki;
            if (factor == FriendlyKBType.Auto)
            {
                if (size % (decimal)(long)FriendlyKBType.Kilo1024 == 0m)
                {
                    factor = FriendlyKBType.Kilo1024;
                }
                else
                {
                    factor = FriendlyKBType.Kilo1000;
                }
            }

            if (factor == FriendlyKBType.Kilo1024)
            {
                if (printKi)
                {
                    ki = "iB";
                }
                else
                {
                    ki = "B";
                }
            }
            else
            {
                ki = "B";
            }

            if (size >= (ulong)factor * (ulong)factor * (ulong)factor * (ulong)factor * (ulong)factor * (ulong)factor)
            {
                fs = size / (double)((ulong)factor * (ulong)factor * (ulong)factor * (ulong)factor * (ulong)factor * (ulong)factor);
                nom = "E" + ki;
            }
            else if (size >= (ulong)factor * (ulong)factor * (ulong)factor * (ulong)factor * (ulong)factor)
            {
                fs = size / (double)((ulong)factor * (ulong)factor * (ulong)factor * (ulong)factor * (ulong)factor);
                nom = "P" + ki;
            }
            else if (size >= (ulong)factor * (ulong)factor * (ulong)factor * (ulong)factor)
            {
                fs = size / (double)((ulong)factor * (ulong)factor * (ulong)factor * (ulong)factor);
                nom = "T" + ki;
            }
            else if (size >= (ulong)factor * (ulong)factor * (ulong)factor)
            {
                fs = size / (double)((ulong)factor * (ulong)factor * (ulong)factor);
                nom = "G" + ki;
            }
            else if (size >= (ulong)factor * (ulong)factor)
            {
                fs = size / (double)((ulong)factor * (ulong)factor);
                nom = "M" + ki;
            }
            else if (size >= (ulong)factor)
            {
                fs = size / (double)factor;
                nom = "K" + ki;
            }
            else
            {
                fs = size;
                nom = "B";
            }

            if (format is object)
            {
                return Math.Round(fs, 2).ToString(format) + " " + nom;
            }
            else
            {
                return Math.Round(fs, 2) + " " + nom;
            }
        }

        /// <summary>
        /// Prints a number value as a friendly byte speed in TiB, GiB, MiB, KiB or B.
        /// </summary>
        /// <param name="speed">The speed to format.</param>
        /// <param name="format">Optional numeric format for the resulting value.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string PrintFriendlySpeed(ulong speed, string format = null, FriendlyKBType factor = FriendlyKBType.Auto, bool asBits = true)
        {
            double fs;
            string nom;
            if (factor == FriendlyKBType.Auto)
            {
                if (speed % (decimal)(long)FriendlyKBType.Kilo1000 == 0m)
                {
                    factor = FriendlyKBType.Kilo1000;
                }
                else
                {
                    factor = FriendlyKBType.Kilo1024;
                }
            }

            string ki = asBits ? "b/s" : "B/s";
            if (speed >= (ulong)factor * (ulong)factor * (ulong)factor * (ulong)factor * (ulong)factor * (ulong)factor)
            {
                fs = speed / (double)((ulong)factor * (ulong)factor * (ulong)factor * (ulong)factor * (ulong)factor * (ulong)factor);
                nom = "E" + ki; // 'wow
            }
            else if (speed >= (ulong)factor * (ulong)factor * (ulong)factor * (ulong)factor * (ulong)factor)
            {
                fs = speed / (double)((ulong)factor * (ulong)factor * (ulong)factor * (ulong)factor * (ulong)factor);
                nom = "P" + ki; // 'wow
            }
            else if (speed >= (ulong)factor * (ulong)factor * (ulong)factor * (ulong)factor)
            {
                fs = speed / (double)((ulong)factor * (ulong)factor * (ulong)factor * (ulong)factor);
                nom = "T" + ki; // 'wow
            }
            else if (speed >= (ulong)factor * (ulong)factor * (ulong)factor)
            {
                fs = speed / (double)((ulong)factor * (ulong)factor * (ulong)factor);
                nom = "G" + ki; // ' still wow
            }
            else if (speed >= (ulong)factor * (ulong)factor)
            {
                fs = speed / (double)((ulong)factor * (ulong)factor);
                nom = "M" + ki; // ' okay
            }
            else if (speed >= (ulong)factor)
            {
                fs = speed / (double)factor;
                nom = "K" + ki; // ' fine.
            }
            else
            {
                fs = speed;
                nom = "B/s";
            } // ' wow.

            if (format is object)
            {
                return Math.Round(fs, 2).ToString(format) + " " + nom;
            }
            else
            {
                return Math.Round(fs, 2) + " " + nom;
            }
        }

        /// <summary>
        /// Encode a string for passing in a URL.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string UrlEncode(string input)
        {
            char[] chrs = input.ToCharArray();
            byte[] asc;
            string sOut = "";
            int i;
            int c;
            c = chrs.Length - 1;
            var loopTo = c;
            for (i = 0; i <= loopTo; i++)
            {
                if (UrlAllowedChars.IndexOf(chrs[i]) != -1)
                {
                    sOut += Conversions.ToString(chrs[i]);
                }
                else
                {
                    asc = Encoding.Unicode.GetBytes(Conversions.ToString(chrs[i]));
                    foreach (byte b in asc)
                    {
                        if (b != 0)
                            sOut += "%" + b.ToString("X2");
                    }
                }
            }

            return sOut;
        }

        /// <summary>
        /// Decode a string from a URL.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string UrlDecode(string input)
        {
            if (input.IndexOf("%") == -1)
                return input;
            var parse = BatchParse(input, "%", WithToken: true, WithTokenIn: true);
            var asc = new Blob();
            int hv = 0;
            foreach (var str in parse)
            {
                if (str.Length == 3 && Conversions.ToString(str[0]) == "%" && IsHexQ(str.Substring(1), ref hv))
                {
                    asc += (byte)hv;
                }
                else
                {
                    asc += str;
                }
            }

            return asc;
        }

        public static string AddSlashes(string text, char quoteChar = '"', char slashChar = '\\')
        {
            string AddSlashesRet = default;
            char[] ch = text.ToCharArray();
            var sb = new StringBuilder();
            int i;
            int c = ch.Length - 1;
            var loopTo = c;
            for (i = 0; i <= loopTo; i++)
            {
                if (ch[i] == quoteChar)
                {
                    sb.Append(Conversions.ToString(slashChar) + quoteChar);
                }
                else
                {
                    sb.Append(ch[i]);
                }
            }

            AddSlashesRet = sb.ToString();
            return AddSlashesRet;
        }


        /// <summary>
        /// Determines of a number can be parsed in hexadecimal
        /// (quick version, does not accept &amp;H or 0x, use IsHex() to parse those strings).
        /// </summary>
        /// <param name="input">Input string to scan.</param>
        /// <param name="value">Optionally receives the value of the input string.</param>
        /// <returns>True if the string can be parsed as a hexadecimal number.</returns>
        /// <remarks></remarks>
        public static bool IsHexQ(string input, [Optional, DefaultParameterValue(default(int))] ref int value)
        {
            string hx = "0123456789ABCDEFabcdef";
            int i;
            int c = input.Length - 1;
            var loopTo = c;
            for (i = 0; i <= loopTo; i++)
            {
                if (hx.IndexOf(input[i]) == -1)
                {
                    return false;
                }
            }

            if (value != default)
            {
                value = int.Parse(input, System.Globalization.NumberStyles.HexNumber);
            }

            return true;
        }

        /// <summary>
        /// Gets value of the DescriptionAttribute for a static member of an object.
        /// </summary>
        /// <param name="val"></param>
        /// <param name="memberName"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetStaticMemberDescription(Type val, string memberName)
        {
            var fi = val.GetMembers(BindingFlags.Public | BindingFlags.Static);
            foreach (var fe in fi)
            {
                if ((fe.Name ?? "") == (memberName ?? ""))
                {
                    DescriptionAttribute da = (DescriptionAttribute)fe.GetCustomAttribute(typeof(DescriptionAttribute));
                    return da.Description;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets value of the DescriptionAttribute for an instance member of an object.
        /// </summary>
        /// <param name="val"></param>
        /// <param name="memberName"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetMemberDescription(object val, string memberName)
        {
            MemberInfo[] fi = val.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (var fe in fi)
            {
                if ((fe.Name ?? "") == (memberName ?? ""))
                {
                    DescriptionAttribute da = (DescriptionAttribute)fe.GetCustomAttribute(typeof(DescriptionAttribute));
                    return da.Description;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the value of the DescriptionAttribute of an enumeration member
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetEnumDescription(object val)
        {
            var fi = val.GetType().GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (var fe in fi)
            {
                if (Conversions.ToBoolean(Operators.ConditionalCompareObjectEqual(fe.GetValue(val), val, false)))
                {
                    DescriptionAttribute da = (DescriptionAttribute)fe.GetCustomAttribute(typeof(DescriptionAttribute));
                    return da.Description;
                }
            }

            return null;
        }
    }

    /// <summary>
    /// A collection of Key-Value Pair objects.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <remarks></remarks>
    public class CollectionOfKeyValue<T, U> : System.Collections.ObjectModel.ObservableCollection<KeyValuePair<T, U>>
    {
        public U get_ItemByKey(T key)
        {
            foreach (var kv in this)
            {
                if (kv.Key.Equals(key))
                    return kv.Value;
            }

            return default;
        }

        public bool ContainsKey(T key)
        {
            foreach (var kv in this)
            {
                if (kv.Key.Equals(key))
                    return true;
            }

            return false;
        }

        public void AddNew(T key, U value)
        {
            Add(new KeyValuePair<T, U>(key, value));
        }
    }


    /// <summary>
    /// A class that contains a StringBuilder that does automatic indentation.
    /// </summary>
    /// <remarks></remarks>
    public class StringBuilderIndenter
    {
        private StringBuilder _sb;

        /// <summary>
        /// This is the base level indent.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int BaseIndent { get; set; } = 0;

        /// <summary>
        /// This is the amount of indent per level.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int IndentAmount { get; set; } = 4;

        private int _Level = 1;
        private bool _atStart = true;

        /// <summary>
        /// Increase indent level by one.
        /// </summary>
        /// <remarks></remarks>
        public void Indent(int levels = 1)
        {
            _Level += levels;
        }

        /// <summary>
        /// Decrease indent level by one.
        /// </summary>
        /// <remarks></remarks>
        public void UnIndent(int levels = 1)
        {
            _Level -= levels;
            if (_Level < 1)
                _Level = 1;
        }

        /// <summary>
        /// Append the current indentation to the StringBuilder.
        /// </summary>
        /// <remarks></remarks>
        private void AppendIndent()
        {
            _sb.Append(new string(' ', BaseIndent + IndentAmount * _Level));
            _atStart = false;
        }

        /// <summary>
        /// Append the current indentation and specified text to the StringBuilder.
        /// If the line is already indented, the indentation is ignored.
        /// </summary>
        /// <remarks></remarks>
        public void Append(string text)
        {
            if (text.IndexOf(Constants.vbCrLf) == -1)
            {
                if (_atStart)
                    AppendIndent();
                _sb.Append(text);
                _atStart = false;
                return;
            }

            var d = TextTools.BatchParse(text, Constants.vbCrLf, WithToken: true);
            foreach (var x in d)
            {
                if ((x ?? "") == Constants.vbCrLf)
                {
                    _sb.AppendLine();
                    _atStart = true;
                }
                else
                {
                    if (_atStart)
                        AppendIndent();
                    _sb.Append(x);
                    _atStart = true;
                }
            }
        }

        /// <summary>
        /// Appends a line flush to the edge, regardless of the current indentation.
        /// </summary>
        /// <param name="text"></param>
        /// <remarks></remarks>
        public void AppendFlushLine(string text)
        {
            _sb.AppendLine(text);
        }

        /// <summary>
        /// Append a line with the current indentation and specified text to the StringBuilder.
        /// If the text on this line is already indented, it is not indented, again.
        /// </summary>
        /// <remarks></remarks>
        public void AppendLine(string text = null)
        {
            if (_atStart)
                AppendIndent();
            if (text is object)
                _sb.AppendLine(text);
            else
                _sb.AppendLine();
            _atStart = true;
        }

        /// <summary>
        /// Returns the current indent level.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int Level
        {
            get
            {
                return _Level;
            }
        }

        /// <summary>
        /// Clears the string.
        /// </summary>
        /// <remarks></remarks>
        public void Clear()
        {
            _sb.Clear();
        }

        public override string ToString()
        {
            return _sb.ToString();
        }

        public StringBuilderIndenter()
        {
            _sb = new StringBuilder();
        }

        public StringBuilderIndenter(StringBuilder sb)
        {
            _sb = sb;
        }
    }
}