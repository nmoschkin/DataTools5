// ************************************************* ''
// DataTools C# Native Utility Library For Windows 
// Adapted for C#/Xamarin
//
// Module: TextTools
//         Text processing and managling library.
// 
// Copyright (C) 2011-2015, 2019 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the Microsoft Public License   
// ************************************************* ''

using System;
using System.Text;

using System.Reflection;
using System.Windows.Documents;
using System.Collections.Generic;

namespace DataTools.Text
{
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

    /// <summary>
    /// Generic sort order.
    /// </summary>
    public enum SortOrder
    {
        /// <summary>
        /// Sort ascending.
        /// </summary>
        Ascending = 0x0,

        /// <summary>
        /// Sort descending.
        /// </summary>
        Descending = 0x1
    }

    /// <summary>
    /// Flags for use with NoSpace
    /// </summary>
    /// <remarks></remarks>
    [Flags]
    public enum NoSpaceModifiers
    {
        /// <summary>
        /// No modifiers.
        /// </summary>
        None = 0,

        /// <summary>
        /// Trim space and convert character before to lowercase.
        /// </summary>
        BeforeToLower = 1,

        /// <summary>
        /// Trim space and convert character before to uppercase.
        /// </summary>
        BeforeToUpper = 2,

        /// <summary>
        /// Trim space and convert character after to lowercase.
        /// </summary>
        AfterToLower = 4,

        /// <summary>
        /// Trim space and convert character after to uppercase.
        /// </summary>
        AfterToUpper = 8,

        /// <summary>
        /// Convert the first character to lowercase.
        /// </summary>
        FirstToLower = 16,

        /// <summary>
        /// Convert the first character to uppercase.
        /// </summary>
        FirstToUpper = 32
    }

    /// <summary>
    /// Good all-purpose text mangling library.
    /// </summary>
    public static class TextTools
    {

        /// <summary>
        /// All allowed mathematical characters.
        /// </summary>
        /// <remarks></remarks>
        public const string MathChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789.()+-\\=/*^%";

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
        /// Compares a string to many other strings.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="comp"></param>
        /// <returns>True if found</returns>
        public static bool MultiComp(string v, string[] comp)
        {

            foreach (string x in comp)
            {
                if (x == v) return true;
            }

            return false;
        }

        /// <summary>
        /// Compares a character to many other characters.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="comp"></param>
        /// <returns>True if found</returns>
        public static bool MultiComp(char v, char[] comp)
        {

            foreach (char x in comp)
            {
                if (x == v) return true;
            }


            return false;
        }


        /// <summary>
        /// Trim all public, writable, instance string properties in a class object.
        /// </summary>
        /// <param name="obj">Object to scan and trim</param>
        /// <param name="trimChars">Optional trim characters</param>
        public static void ClassTrimTool(object obj, char[] trimChars = null)
        {

            if (obj == null || obj.GetType().IsClass == false) throw new ArgumentException();


            PropertyInfo[] props = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty);

            foreach (var prop in props)
            {
                string val;

                if (prop.PropertyType == typeof(string) && prop.SetMethod != null)
                {
                    val = (string)prop.GetValue(obj);
                    if (val == null) continue;

                    if (trimChars != null)
                    {
                        val = val.Trim(trimChars);
                    }
                    else
                    {
                        val = val.Trim();
                    }

                    prop.SetValue(obj, val);
                }
            }
        }

        /// <summary>
        /// Parse a string into an array of strings using the specified parameters
        /// </summary>
        /// <param name="Scan">String to scan</param>
        /// <param name="Separator">Separator string</param>
        /// <param name="SkipQuote">Whether to skip over quote blocks</param>
        /// <param name="Unescape">Whether to unescape quotes.</param>
        /// <param name="QuoteChar">Quote character to use.</param>
        /// <param name="EscapeChar">Escape character to use.</param>
        /// <param name="WithToken">Include the token in the return array.</param>
        /// <param name="WithTokenIn">Attach the token to the beginning of every string separated by a token (except for string 0).  Requires WithToken to also be set to True.</param>
        /// <param name="Unquote">Remove quotes from around characters.</param>
        /// <returns>An array of strings.</returns>
        /// <remarks></remarks>
        public static string[] Split(string Scan, string Separator, bool SkipQuote = false, bool Unescape = false, char QuoteChar = '"', char EscapeChar = '\\', bool Unquote = false, bool WithToken = false, bool WithTokenIn = false)
        {

            int i = 0;
            int f = 0;

            int e = 0;
            int g = 0;

            List<string> sOut = new List<string>();
            int c = 0;

            char[] chOut = null;
            int h = 0;
            int d = 0;

            bool inq = false;

            char[] chrs = Scan.ToCharArray();
            char[] sep = Separator.ToCharArray();

            c = chrs.Length - 1;
            e = sep.Length - 1;

            f = 0;
            chOut = new char[c + 1];

            for (i = 0; i <= c; i++)
            {
                if ((SkipQuote) && (chrs[i] == QuoteChar))
                {
                    if ((!inq) && (i < c) && (chrs[i] == QuoteChar) && (chrs[i + 1] == QuoteChar))
                    {
                        i++;
                        continue;
                    }

                    if ((inq) && (i < c) && (chrs[i] == EscapeChar) && (chrs[i + 1] == QuoteChar))
                    {
                        if (Unescape)
                        {
                            i++;

                            chOut[g] = chrs[i];
                            g++;
                        }
                        else
                        {
                            chOut[g] = chrs[i];
                            g++;
                            i++;

                            chOut[g] = chrs[i];
                            g++;
                        }
                        continue;
                    }

                    if ((inq) && (chrs[i] == QuoteChar))
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

                if ((!inq) && (chrs[i] == sep[f]))
                {
                    // save the starting index
                    h = i;
                    for (f = 0; f <= e; f++)
                    {
                        if (i >= c) break;

                        if (chrs[i] == sep[f])
                            i++;

                        else break;
                    }


                    if (f == e + 1)
                    {
                        if (WithToken && !WithTokenIn && d != 0)
                        {
                            sOut.Add(Separator);
                            d++;
                        }

                        if (WithToken && WithTokenIn)
                        {
                            foreach (char cs in sep)
                            {
                                chOut[g] = cs;
                                g++;
                            }
                        }

                        sOut.Add(new string(chOut, 0, g));
                        g = 0;

                        d++;
                        i--;

                    }
                    else
                    {
                        chOut[g] = chrs[h];
                        g++;
                        i = h;
                    }
                    f = 0;
                }
                else
                {
                    chOut[g] = chrs[i];
                    g++;
                }

            }

            if (g != 0)
            {
                sOut.Add(new string(chOut, 0, g));
            }

            return sOut.ToArray();
        }

        /// <summary>
        /// Determine if a string consists only of numbers.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsAllNumbers(string value)
        {

            char[] ch = value.ToCharArray();
            int i, c = ch.Length - 1;

            for (i = 0; i < c; i++)
            {
                if ((ch[i] < '0') || (ch[i] > '9'))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Compares two array of bytes for equality.
        /// </summary>
        /// <param name="a">First array</param>
        /// <param name="b">Second array</param>
        /// <param name="result">Relative disposition</param>
        /// <returns>True if equal</returns>
        /// <remarks></remarks>
        public static bool CompareBytes(byte[] a, byte[] b, ref int result)
        {
            if (a.Length != b.Length)
                return false;

            if ((a == null) && (b == null))
                return true;

            if (((a != null) && (b == null)) || ((b != null) && (a == null)))
                return false;

            int i = 0;
            int c = a.Length - 1;

            for (i = 0; i <= c; i++)
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
        /// Removes all spaces from a string using default modifiers.
        /// </summary>
        /// <param name="subject"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string NoSpace(string subject)
        {
            return StripChars(subject, " " + (char)9);
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
            char[] ch = null;
            int i = 0;
            int j = subject.Length - 1;

            int e = 0;
            string exclusions = " " + (char)9;
            bool ws = false;

            ch = new char[j + 1];

            for (i = 0; i <= j; i++)
            {

                if ((i == 0) && (modifiers & (NoSpaceModifiers.FirstToLower | NoSpaceModifiers.FirstToUpper)) != 0 && (exclusions.IndexOf(subject[i]) == -1))
                {
                    switch (modifiers)
                    {
                        case NoSpaceModifiers.FirstToUpper:
                            ch[e] = char.ToUpper(subject[i]);

                            break;
                        case NoSpaceModifiers.FirstToLower:
                            ch[e] = char.ToLower(subject[i]);

                            break;
                    }

                }

                if (exclusions.IndexOf(subject[i]) != -1)
                {
                    if (modifiers != NoSpaceModifiers.None)
                    {
                        if (i > 0)
                        {
                            if ((modifiers & NoSpaceModifiers.BeforeToLower) != 0)
                            {
                                ch[e - 1] = char.ToLower(ch[e - 1]);
                            }
                            else if ((modifiers & NoSpaceModifiers.BeforeToUpper) != 0)
                            {
                                ch[e - 1] = char.ToUpper(ch[e - 1]);
                            }
                        }
                    }
                    ws = true;
                }
                else if (ws)
                {
                    if (modifiers != NoSpaceModifiers.None)
                    {
                        if ((modifiers & NoSpaceModifiers.AfterToLower) != 0)
                        {
                            ch[e] = char.ToLower(subject[i]);
                        }
                        else if ((modifiers & NoSpaceModifiers.AfterToUpper) != 0)
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

            return new string(ch);

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
            int c = ch.Length;
            int d = 0;

            for (int i = 0; i < c; i++)
            {
                if ((ch[i] == character))
                    d++;
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
        public static string StripChars(string subject, string exclusions)
        {
            string ch = "";
            int i = 0;
            int j = subject.Length - 1;

            for (i = 0; i <= j; i++)
            {
                if (exclusions.IndexOf(subject.Substring(i, 1)) != -1)
                    continue;
                ch += subject.Substring(i, 1);
            }

            return ch;
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
        public static string TextBetween(string subject, string startString, string stopString, ref int startIndex)
        {

            int i = 0;
            int j = 0;

            string s = null;

            if (startIndex != -1)
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

            if ((j - i) < 1)
                return "";
            s = subject.Substring(i, (j - i));

            if ((startIndex != -1))
            {
                startIndex = j + stopString.Length;
            }

            return s;


        }

        /// <summary>
        /// Finds and returns the first occurrence of text between startString and stopString
        /// </summary>
        /// <param name="subject">The text to process.</param>
        /// <param name="startString">The starting string to scan.</param>
        /// <param name="stopString">The ending string to scan.</param>
        /// <returns>The first occurrence of text between two seperator strings.</returns>
        /// <remarks></remarks>
        public static string TextBetween(string subject, string startString, string stopString)
        {
            int i = -1;
            return TextBetween(subject, startString, stopString, ref i);
        }


        /// <summary>
        /// Function to retrieve a quote from a string of data. 
        /// The quote character must be: exactly one before, exactly at, or anywhere after the location specified by 'index'.  
        /// Text outside of the first quoted string is discarded.
        /// </summary>
        /// <param name="s">The string to scan.</param>
        /// <param name="index">The position to begin scanning.</param>
        /// <param name="qc">The quote character to use.</param>
        /// <param name="ec">The escape character to use.</param>
        /// <param name="wc">Return the string in quotes.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string QuoteFromHere(string s, int index, char qc = '\"', char ec = '\\', bool wc = false)
        {
            char[] buffIn = s.ToCharArray();

            int i, c = buffIn.Length;
            bool inq = false;

            StringBuilder sb = new StringBuilder();

            for (i = ((index > 0) ? (index - 1) : 0); i < c; i++)
            {
                if (!inq)
                {
                    if (buffIn[i] == qc)
                    {
                        inq = true;
                        if (wc) sb.Append(qc);
                    }
                }
                else
                {
                    if (i < c - 1)
                    {
                        if ((buffIn[i] == ec) && (buffIn[i + 1] == qc))
                        {
                            sb.Append(qc);
                            i++;

                            continue;
                        }
                        else if ((buffIn[i] == ec) && (buffIn[i + 1] == ec))
                        {
                            sb.Append(ec);
                            i++;

                            continue;
                        }
                    }

                    if (buffIn[i] == qc)
                    {
                        if (wc) sb.Append(qc);
                        break;
                    }
                    sb.Append(buffIn[i]);
                }

            }

            return sb.ToString();
        }

        /// <summary>
        /// Determines if something really is a number. Supports C-style hex strings.
        /// </summary>
        /// <param name="subject">The subject to test.</param>
        /// <param name="noTrim">Whether to skip tripping white space around the text.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsNumber(object o, bool noTrim = false)
        {

            if (o is string subject)
            {
                if (!noTrim)
                    subject = subject.Trim();

                double d = 0.0;
                ulong v;

                bool b = double.TryParse(subject, out d);

                if (b)
                    return true;

                if ((subject.IndexOf("0x") == 0) || (subject.IndexOf("&H") == 0))
                {
                    return ulong.TryParse(subject.Substring(2), System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.CurrentCulture, out v);
                }
                return false;
            }
            else
            {
                return o.GetType().IsPrimitive;
            }
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
        /// <param name="Input">The unput to process.</param>
        /// <param name="RemoveOperatorGaps">Specify whether to remove the gaps between equals signs and values.</param>
        /// <param name="RemoveComments">Specifies whether to remove comments.</param>
        /// <param name="CommentChars">Comment characters to use to discern comments.</param>
        /// <returns>Processed text.</returns>
        /// <remarks></remarks>
        public static string TightenText(string Input, bool RemoveOperatorGaps = true, bool RemoveComments = true, string CommentChars = "'")
        {
            // ERROR: Not supported in C#: OnErrorStatement


            // as efficiently as possible
            char[] c = null;
            int a = 0;
            int b = 0;

            int i = 0;
            int j = 0;

            char[] cmt = CommentChars.ToCharArray();

            char[] ops = "=".ToCharArray();

            char[] d = null;
            int p = 0;

            int spc = 0;

            byte t = 0;
            byte f = 0;

            c = Input.ToCharArray();
            b = c.Length - 1;

            d = new char[b + 1];


            for (a = 0; a <= b; a++)
            {

                if ((RemoveComments == true))
                {
                    j = cmt.Length - 1;

                    for (i = 0; i <= j; i++)
                    {
                        if ((c[a] == cmt[i]))
                            break;
                    }
                }

                if ((t == 0) & (c[a] == ' '))
                {
                    continue;
                }
                else if ((t == 0))
                {
                    t = 1;
                }

                if ((c[a] == ' '))
                {
                    spc += 1;
                }
                else if ((spc > 0) && (f == 0))
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
                    p++;
                }

                if ((RemoveOperatorGaps))
                {
                    j = ops.Length - 1;

                    for (i = 0; i <= j; i++)
                    {
                        if ((c[a] == ops[i]))
                        {
                            f = 1;
                            if ((a > 0))
                            {
                                if (d[p - 2] == ' ')
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
            return new string(d);

        }

        /// <summary>
        /// Returns a string suitable for parsing by Val() or <see cref="FVal" />.
        /// The default behavior processes the string exactly as the Val function looks at it, but it is customizable.
        /// </summary>
        /// <param name="value">String to process.</param>
        /// <param name="values">Receives the string values of all discovered individual numbers based upon the selected configuration.</param>
        /// <param name="justFirst">Whether to process just the first discovered word.</param>
        /// <param name="allInOne">Whether to merge all word blocks together before processing.</param>
        /// <param name="maxSkip">The maximum number of words to skip in search of a number.</param>
        /// <param name="skipChars">Specific characters to ignore or step over in search of a number (default is common currency).</param>
        /// <returns>A result ready to be parsed by a numeric parser.</returns>
        /// <remarks></remarks>
        public static string JustNumbers(string value, ref string[] values, bool justFirst = true, bool allInOne = true, int maxSkip = 0, string skipChars = "$£€#")
        {
            char[] sn = value.ToCharArray();
            char[] sc = null;

            int i = 0;
            int c = sn.Length - 1;

            int e = 0;

            int skip = -1;

            int d = 0;
            bool t = false;

            string firstScan = "&0123456789+-. ";
            string scan = " 1234567890-+.eEHhOo";

            sc = new char[c + 1];

            for (i = 0; i <= c; i++)
            {
                if (!t)
                {
                    if (firstScan.IndexOf(sn[i]) >= 0)
                    {
                        t = true;
                    }
                    else
                    {
                        if (justFirst)
                        {
                            if (maxSkip > -1 && skip > maxSkip)
                            {
                                if (values != null)
                                {
                                    values = (string[])Array.CreateInstance(typeof(string), 0);
                                }
                                return "";
                            }
                            else
                            {
                                skip += 1;
                            }
                        }
                    }
                }


                if (t)
                {
                    if (scan.IndexOf(sn[i]) >= 0)
                    {
                        if (sn[i] == ' ')
                        {
                            if (justFirst && d != 0)
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
                if (values != null)
                {
                    values = (string[])Array.CreateInstance(typeof(string), 0);
                }
                return "";
            }

            if (e < c)
            {
                Array.Resize(ref sc, e + 1);
            }

            if (values != null)
            {
                values = Split(new string(sc), " ");
            }

            return new string(sc);
        }


        /// <summary>
        /// A better Val() function.  Will parse hexadecimal with 0x or &amp;H markers or binary digits with a 'b' marker.
        /// </summary>
        /// <param name="value">The string value to parse.</param>
        /// <returns>An numeric primitive (either a Long or a Double).</returns>
        /// <remarks></remarks>
        public static double? FVal(string value)
        {

            double o;

            value = value.Trim();

            if (value.Length < 2)
            {
                if (IsNumber(value) == false)
                    return double.NaN;

                if (double.TryParse(value, out o))
                    return o;
            }

            if ((value.Substring(0, 2) == "&H") || (value.Substring(0, 2) == "0x"))
            {
                value = value.Substring(2);
                o = (double)long.Parse(value, System.Globalization.NumberStyles.AllowHexSpecifier);
                return o;
            }
            else if (value.ToLower()[0] == 'b')
            {
                int i;
                int c;
                char[] ch;
                long v = 0;

                ch = value.Substring(1).ToCharArray();
                c = ch.Length - 1;

                for (i = 0; i <= c; i++)
                {
                    if (ch[i] == '1')
                    {
                        v += 1;
                    }
                    else if (ch[i] != '0')
                    {
                        return v;
                    }
                    v <<= 1;
                }

                return (v);
            }

            if (IsNumber(value) == false)
                return double.NaN;

            if (double.TryParse(value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CurrentCulture.NumberFormat, out o))
                return (double)o;

            return null;

        }


        /// <summary>
        /// Escape text for use in a CSV file.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string TextEscapeCSV(string s)
        {
            char[] b = null;
            int i = 0;

            if (s == null) return "";

            StringBuilder sb = new StringBuilder();

            b = s.ToCharArray();

            for (i = 0; i <= b.Length - 1; i++)
            {
                switch (b[i])
                {

                    case '\"':
                        sb.Append("\"\"");
                        break;

                    default:
                        sb.Append(b[i]);
                        break;

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

            StringBuilder sOut = new StringBuilder();

            if (s == null) return "";

            b = s.ToCharArray();

            for (i = 0; i < b.Length; i++)
            {


                if (i < b.Length - 1)
                {
                    if ((b[i] == '\\') && (b[i + 1] == '\"'))
                    {
                        sOut.Append("\\\"");
                        i++;
                        continue;
                    }
                }

                if (MultiComp(b[i], ("\\\"/").ToCharArray()))
                {
                    sOut.Append("\\");
                }

                sOut.Append(b[i]);
            }

            return sOut.ToString();

        }

        /// <summary>
        /// Replaces all instances of 'search' with 'replace in 'subj'
        /// </summary>
        /// <param name="subj">The string to scan.</param>
        /// <param name="search">The text for which to scan.</param>
        /// <param name="replace">The replacement text.</param>
        /// <returns></returns>
        public static string SearchReplace(string subj, string search, string replace)
        {
            int i;

            do
            {
                i = subj.IndexOf(search);
                if (i != -1) subj = subj.Replace(search, replace);

            } while (i != -1);

            return subj;
        }

        /// <summary>
        /// Removes all blank lines from a string.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string NoNullLines(string i)
        {
            string[] st = null;
            int l = 0;

            StringBuilder sOut = new StringBuilder();

            i = SearchReplace(i, "\r\n", "\r");
            i = SearchReplace(i, "\n", "\r");

            st = Split(i, "\r");

            for (l = 0; l <= st.Length - 1; l++)
            {
                if ((!string.IsNullOrEmpty(st[l])))
                {
                    if ((sOut.Length != 0))
                        sOut.Append("\r\n");
                    sOut.Append(st[l]);
                }

            }

            return sOut.ToString();

        }

        /// <summary>
        /// Clear all null characters from a string
        /// </summary>
        /// <param name="input">String to process.</param>
        /// <returns>Processed text.</returns>
        /// <remarks></remarks>
        public static string RemoveNulls(string input)
        {
            return input.Trim((char)0);
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
        public static string OneSpace(string input, string spaceChars = " ", bool PreserveQuotedText = true, char quoteChar = '\"', char escapeChar = '\\', bool Quick = true)
        {

            int a = 0;
            int b = 0;

            StringBuilder varOut = new StringBuilder();
            bool isP = false;
            bool iQ = false;

            char[] ch = input.ToCharArray();

            if (Quick)
            {
                if (input.IndexOf("  ") == -1)
                {
                    return input;
                }

                while (!(input.IndexOf("  ") == -1))
                {
                    input = input.Replace("  ", " ");
                }

                return input;
            }

            b = ch.Length - 1;

            for (a = 0; a <= b; a++)
            {
                if ((iQ == true))
                {
                    varOut.Append(ch[a]);
                    if ((ch[a] == quoteChar))
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
                else
                {
                    if ((spaceChars.IndexOf(ch[a]) != -1) & (isP == false))
                    {
                        isP = true;
                        varOut.Append(ch[a]);
                    }
                    else if ((spaceChars.IndexOf(ch[a]) == -1))
                    {
                        varOut.Append(ch[a]);
                        if ((isP == true))
                            isP = false;
                        if ((ch[a] == quoteChar) & (PreserveQuotedText == true))
                        {
                            iQ = true;
                        }
                    }

                }
            }

            return varOut.ToString();

        }

        public static string Bracket(string szText, ref int startIndex, ref int newIndex, ref string ErrorText)
        {
            return Bracket(szText, ref startIndex, ref newIndex, "()", ref ErrorText);
        }

        public static string Bracket(string szText, ref int startIndex, ref int newIndex)
        {
            string _d = null;
            return Bracket(szText, ref startIndex, ref newIndex, "()", ref _d);
        }

        /// <summary>
        /// Get all text within the first occurance of a specified bracket set.  Discards text outside.
        /// </summary>
        /// <param name="szText">String to scan.</param>
        /// <param name="startIndex">Index in the string at which to start scanning.</param>
        /// <param name="newIndex">Receives the index of the first character after the closing bracket.</param>
        /// <param name="BracketPair">Bracket pair (must consist of exactly 2 characters, for other division pairs, use TextBetween.)</param>
        /// <param name="ErrorText"></param>
        /// <returns>The text inside the first complete bracket, excluding the outer-most pair.</returns>
        /// <remarks></remarks>
        public static string Bracket(string szText, ref int startIndex, ref int newIndex, string BracketPair, ref string ErrorText)
        {
            // returns the text inside the first complete bracket, excluding the outer-most pair. newIndex is set to the first character after the closing bracket

            char[] ch = szText.ToCharArray();
            char[] bp = BracketPair.ToCharArray();
            int i = 0;
            int c = ch.Length - 1;

            string sOut = "";
            int n = 0;
            int bc = 0;

            char open;
            char close;

            if (BracketPair.Length != 2)
            {
                ErrorText = "Invalid bracket pair string. The bracket pair string must consist of exactly one open character and exactly one close character.";
                return null;
            }

            open = bp[0];
            close = bp[1];

            try
            {
                newIndex = startIndex;

                if (string.IsNullOrEmpty(szText))
                    return "";
                i = szText.IndexOf(open, startIndex);

                if (i == -1)
                {
                    // there are no brackets, so we shall assume that entire string from startIndex to finish is a "bracket",
                    // we'll return that text and set newIndex to the end of the line

                    newIndex = 0;
                    i = szText.IndexOf(close);

                    if (i != -1)
                    {
                        ErrorText = "Syntax error.  Unexpected closing bracket at column " + i + 1;
                        return null;
                    }

                    newIndex = ch.Length;
                    return szText.Substring(startIndex);

                }


                for (n = i; n <= c; n++)
                {
                    sOut += ch[n];

                    if ((ch[n] == open))
                    {
                        bc += 1;
                    }
                    else if ((ch[n] == close))
                    {
                        bc -= 1;
                        if ((bc < 0))
                        {
                            ErrorText = "Syntax error.  Unexpected closing bracket at column " + i + 1;
                            return null;
                        }
                        else if (bc == 0)
                        {
                            break; // TODO: might not be correct. Was : Exit For
                        }
                    }

                }

                if (bc != 0)
                {
                    ErrorText = "Syntax error.  Unmatched closing bracket at column " + i + 1;
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
        /// Get all text within the first occurance of a specified bracket set.  Discards text outside.
        /// </summary>
        /// <param name="szText">String to scan.</param>
        /// <param name="BracketPair">Bracket pair (must consist of exactly 2 characters, for other division pairs, use TextBetween.)</param>
        /// <returns>The text inside the first complete bracket, excluding the outer-most pair.</returns>
        /// <remarks></remarks>
        public static string Bracket(string szText, string BracketPair = "()")
        {
            string et = null;
            int i = 0, j = 0;
            return Bracket(szText, ref i, ref j, BracketPair, ref et);

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
        public static string SpaceOperators(string value, string Operators = "/\\&^%*-+()[]{}", string SepChars = null, string StickyCharsLeft = "", string StickyCharsRight = "+-", string NoStickyChars = "")
        {
            int i = 0;
            int c = 0;

            string s = null;
            char ch = '\0';
            bool inq = false;

            char[] sp = null;

            if (SepChars == null)
            {
                SepChars = "";
                for (i = 0; i <= 255; i++)
                {
                    ch = (char)i;
                    if (char.IsWhiteSpace(ch))
                    {
                        SepChars += ch;
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


            for (i = 0; i <= c; i++)
            {
                if ((sp[i] == '\"'))
                {
                    inq = !inq;
                }


                if (!inq && Operators.IndexOf(sp[i]) != -1)
                {

                    if ((i > 0) && (Operators.IndexOf(sp[i - 1]) != -1) && (NoStickyChars.IndexOf(sp[i]) == -1) && (NoStickyChars.IndexOf(sp[i - 1]) == -1))
                    {
                        if ((StickyCharsRight.IndexOf(sp[i - 1]) <= -1))
                        {
                            s = s.Substring(0, s.Length - 1);
                        }

                        s += sp[i];
                    }
                    else
                    {
                        if (StickyCharsLeft.IndexOf(sp[i]) == -1 || (NoStickyChars.IndexOf(sp[i]) != -1))
                        {
                            s += " ";
                        }

                        s += sp[i];
                    }

                    if (StickyCharsRight.IndexOf(sp[i]) == -1 || (NoStickyChars.IndexOf(sp[i]) != -1))
                    {
                        s += " ";
                    }

                }
                else
                {
                    s += sp[i];
                }

            }

            return (OneSpace(s, SepChars)).Trim();

        }

        /// <summary>
        /// Returns all words in a string as an array of strings.
        /// </summary>
        /// <param name="Text">Text to split.</param>
        /// <param name="SepChars">Separator characters to use.</param>
        /// <param name="AdditionalSepChars">Any additional separator characters to use.</param>
        /// <param name="SkipQuotes">Skip over quoted text.</param>
        /// <param name="Unescape">Unescape quoted text.</param>
        /// <param name="qc">Quote character to use.</param>
        /// <param name="ec">Escape character to use.</param>
        /// <returns>Array of strings.</returns>
        /// <remarks></remarks>
        public static string[] Words(string Text, string SepChars = null, string AdditionalSepChars = null, bool SkipQuotes = false, bool Unescape = false, char qc = '\"', char ec = '\\')
        {
            int i = 0;
            int c = 0;

            int n = 0;

            string s = null;
            char ch = '\0';

            string[] stout = null;
            string[] stwork = null;
            string[] stwork2 = null;

            char[] sep = null;

            if (SepChars == null)
            {
                SepChars = "";
                for (i = 0; i <= 255; i++)
                {
                    ch = (char)(i);
                    if (char.IsWhiteSpace(ch))
                    {
                        //if (ch != '\r' && ch != '\n')
                            SepChars += ch;
                    }
                }
            }

            if (string.IsNullOrEmpty(SepChars))
                return new string[] { Text };

            s = SepChars;
            if (AdditionalSepChars != null) s += AdditionalSepChars;

            sep = s.ToCharArray();
            ch = sep[0];

            s = Text;
            s = OneSpace(s, "" + ch, SkipQuotes).Trim();

            stout = Split(s, "" + ch, SkipQuotes, Unescape, qc, ec);

            if (stout == null)
                return new string[] { s };

            c = stout.Length - 1;

            if (SepChars.Length == 1)
            {
                return stout;
            }

            SepChars = SepChars.Substring(1);


            for (i = 0; i <= c; i++)
            {
                stwork = Words(stout[i], SepChars, null, SkipQuotes, Unescape, qc, ec);
                if (stwork2 == null)
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
            string[] st = null;
            int xTot = 0;
            string sOut = "";
            int i = 0;
            int j = 0;

            if (szText == null) return null;

            if ((szText.Length <= Cols))
                return szText;

            st = Words(szText);

            for (i = 0; i < st.Length; i++)
            {
                if ((st[i].Length >= Cols))
                {
                    if (sOut.Substring(sOut.Length - 1, 1) == " ")
                        sOut = sOut.Substring(0, sOut.Length - 1);

                    sOut += "\r\n";
                    j = 0;

                    while ((j < st[i].Length))
                    {
                        try
                        {
                            sOut += st[i].Substring(j, Cols);
                        }
                        catch (Exception)
                        {
                            sOut += st[i].Substring(j);
                        }

                        j += Cols;

                        if (sOut.Substring(sOut.Length - 1, 1) == " ")
                            sOut = sOut.Substring(0, sOut.Length - 1);

                        sOut += "\r\n";
                    }
                    xTot = 0;

                    continue;

                }
                else if ((xTot + st[i].Length) > Cols)
                {
                    if (sOut.Substring(sOut.Length - 1, 1) == " ")
                        sOut = sOut.Substring(0, sOut.Length - 1);

                    sOut += "\r\n";
                    xTot = 0;
                }

                xTot += st[i].Length;
                sOut += st[i];

                if ((i < (st.Length - 1)))
                {
                    xTot++;
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
            return szText.Replace("\r", "").Replace("\n", "\0").Split('\0');
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
            string s;
            s = ((ulong)value).ToString("X");

            if ((lowercase))
                s = s.ToLower();

            if ((width - s.Length > 0))
            {
                return prefix + new string('0', width - s.Length) + s;
            }
            else
            {
                return prefix + s;
            }

        }

        /// <summary>
        /// Determines of a number can be parsed in hexadecimal 
        /// (quick version, does not accept &amp;H or 0x, use IsHex() to parse those strings).
        /// </summary>
        /// <param name="input">Input string to scan.</param>
        /// <param name="value">Optionally receives the value of the input string.</param>
        /// <returns>True if the string can be parsed as a hexadecimal number.</returns>
        /// <remarks></remarks>
        public static bool IsHexQ(string input, ref int value)
        {

            string hx = "0123456789ABCDEFabcdef";
            int i = 0;
            int c = input.Length - 1;

            for (i = 0; i <= c; i++)
            {
                if (hx.IndexOf(input[i]) == -1)
                {
                    return false;
                }
            }

            value = int.Parse(input, System.Globalization.NumberStyles.HexNumber);
            return true;
        }

        /// <summary>
        /// Determines of a number can be parsed in hexadecimal 
        /// (quick version, does not accept &amp;H or 0x, use IsHex() to parse those strings).
        /// </summary>
        /// <param name="input">Input string to scan.</param>
        /// <returns>True if the string can be parsed as a hexadecimal number.</returns>
        /// <remarks></remarks>
        public static bool IsHexQ(string input)
        {
            int i = 0;
            return IsHexQ(input, ref i);
        }

        /// <summary>
        /// Returns true if the value in a hexadecimal number. Accepts &amp;H and 0x prefixes.
        /// </summary>
        /// <param name="hin">String to scan</param>
        /// <param name="value">Optionally receives the parsed value.</param>
        /// <returns>True if the string can be parsed as hex.</returns>
        /// <remarks></remarks>
        public static bool IsHex(string hin, ref int value)
        {

            char[] b = null;
            int i = 0;

            bool c = true;

            b = hin.ToCharArray();

            if (hin.IndexOf("&H") == -1 && hin.IndexOf("0x") == -1)
                return IsHexQ(hin, ref value);

            hin = hin.Replace("&H", "");
            hin = hin.Replace("0x", "");

            for (i = 0; i <= b.Length - 1; i++)
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

                        break;

                    default:

                        c = false;
                        break;
                }
            }

            if (c)
            {
                value = (int)FVal("&H" + hin);
            }

            return c;

        }

        /// <summary>
        /// Returns true if the value in a hexadecimal number. Accepts &amp;H and 0x prefixes.
        /// </summary>
        /// <param name="hin">String to scan</param>
        /// <returns>True if the string can be parsed as hex.</returns>
        /// <remarks></remarks>
        public static bool IsHex(string hin)
        {
            int i = 0;
            return IsHex(hin, ref i);
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
            int a = 0;
            int b = 0;

            string varOut = "";

            bool isP = false;
            bool iQ = false;

            b = input.Length - 1;

            for (a = 0; a <= b; a++)
            {
                if ((iQ == true))
                {
                    varOut += input[a];
                    if ((input[a] == '\"'))
                    {
                        iQ = false;
                    }
                }
                else
                {
                    if ((a < (b - commentchar.Length)))
                    {
                        if ((input.Substring(a, commentchar.Length) == commentchar))
                        {
                            break;
                        }
                    }
                    varOut += input[a];

                    if ((isP == true))
                        isP = false;
                    if ((input[a] == '\"'))
                    {
                        iQ = true;
                    }

                }
            }

            return varOut;
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

            int i = 0;
            int c = 0;

            StringBuilder sb = new StringBuilder();

            c = ch.Length - 1;

            for (i = 0; i <= c; i++)
            {

                if (ch[i].ToString().ToUpper() == ch[i].ToString() && i > 0)
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
        /// <param name="input"></param>
        /// <param name="stripSpaces">Specify whether or not to remove space characters.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string CamelCase(string input, bool stripSpaces = true)
        {
            return TitleCase(input, stripSpaces);
        }

        /// <summary>
        /// Convert spaced or underscored lines to camel case.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="stripSpaces">Specify whether or not to remove space characters.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string TitleCase(string input, bool stripSpaces = true)
        {
            int a = 0;
            int b = 0;

            string varOut = "";
            bool isP = false;
            bool iQ = false;

            if (input == null)
                return "";
            input = SearchReplace(input, "_", " ");

            b = input.Length - 1;
            for (a = 0; a <= b; a++)
            {
                if ((iQ == true))
                {
                    varOut += input[a];
                    if ((input[a] == '\"'))
                    {
                        iQ = false;
                    }
                }
                else
                {
                    if ((input[a] == (char)(32)) && (isP == false))
                    {
                        isP = true;
                        varOut += " ";

                    }
                    else if ((input[a] != (char)(32)))
                    {
                        if ((a == 0) | (isP == true))
                        {
                            varOut += input[a].ToString().ToUpper();

                        }
                        else if ((isP == false))
                        {
                            varOut += input[a].ToString().ToLower();
                        }

                        if ((isP == true))
                            isP = false;
                        if ((input[a] == '\"'))
                        {
                            iQ = true;
                        }
                    }

                }
            }

            if (stripSpaces)
                return SearchReplace(varOut, " ", "");
            else
                return varOut;
        }

        /// <summary>
        /// Converts an IP address string into a 32 bit signed integer.
        /// </summary>
        /// <param name="ipaddr"></param>
        /// <returns></returns>
        public static int IPInt(string ipaddr)
        {
            try
            {
                int output = 0;
                var toks = ipaddr.Split(".");

                if (toks.Length != 4) return 0;

                for (int i = 3; i >= 0; i--) 
                {
                    output = (output << 8) | int.Parse(toks[i]);
                }

                return output;
            }
            catch
            {
                return 0;
            }
            

        }

        /// <summary>
        /// Converts a signed 32 bit integer into an IP address string.
        /// </summary>
        /// <param name="ipaddr"></param>
        /// <returns></returns>

        public static string IPStr(int ipaddr)
        {
            string output = "";

            for (int i = 3; i >= 0; i--)
            {
                if (output != "") output += ".";
                output += ((ipaddr >> (8 * i)) & 0xff).ToString();
            }

            return output;
        }


        /// <summary>
        /// Concats all strings in a string array into one string.
        /// </summary>
        /// <param name="Text">Array to combine.</param>
        /// <returns>A string.</returns>
        /// <remarks></remarks>
        public static string Stream(string[] Text)
        {
            long i = 0;
            long b = 0;

            StringBuilder sb = new StringBuilder();

            i = -1L;
            i = Text.Length - 1;
            for (b = 0; b < i; b++)
            {
                if (sb.Length != 0)
                    sb.Append("\r\n");
                sb.Append(Text[b]);
            }

            return sb.ToString();

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
        public static string[] Filter(string Text, ref bool FirstIsFilter, string FilterPair = "\"", string Escape = "\\")
        {
            string[] lnOut = null;

            int i = 0;
            int j = 0;
            int m = 0;

            char[] c = null;
            char[] fp = null;
            bool e = false;

            // ERROR: Not supported in C#: OnErrorStatement


            if ((FilterPair.Length == 0))
                FilterPair = "\"";

            if ((FilterPair.Length == 1))
            {
                FilterPair = FilterPair + FilterPair;
            }

            fp = FilterPair.ToCharArray();

            c = Text.ToCharArray();
            j = -1;
            j = c.Length - 1;

            m = -1;

            for (i = 0; i <= j; i++)
            {
                if ((e == false))
                {
                    if ((c[i] == fp[0]))
                    {
                        if ((m == -1))
                        {
                            FirstIsFilter = true;
                        }
                        m += 1;
                        e = true;
                        Array.Resize(ref lnOut, m + 1);
                        lnOut[m] = "" + c[i];
                    }
                    else
                    {
                        if ((m == -1))
                        {
                            FirstIsFilter = false;
                            m += 1;
                            Array.Resize(ref lnOut, m + 1);
                        }

                        lnOut[m] += c[i];
                    }
                }
                else
                {
                    if ((c[i] == fp[1]))
                    {
                        if ((i > 0))
                        {
                            if ((!string.IsNullOrEmpty(Escape)) & (Escape != null))
                            {
                                if ((c[i - 1] == Escape[0]))
                                {
                                    if ((lnOut[m].Length > 1))
                                    {
                                        lnOut[m] = lnOut[m].Substring(0, lnOut[m].Length - 1);
                                    }
                                }
                            }
                        }

                        lnOut[m] += c[i];

                        m += 1;
                        e = false;
                        Array.Resize(ref lnOut, m + 1);
                    }
                    else
                    {
                        lnOut[m] += c[i];

                    }

                }
            }

            return lnOut;

        }

        /// <summary>
        /// Prints a number value as a friendly byte size in TiB, GiB, MiB, KiB or B.
        /// </summary>
        /// <param name="size">The size to format.</param>
        /// <param name="format">Optional numeric format for the resulting value.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string PrintFriendlySize(double size, string format = null, bool binary = true)
        {
            double fs;
            string nom;

            if (binary)
            {
                if ((size >= (1024D * 1024 * 1024 * 1024 * 1024 * 1024 * 1024 * 1024)))
                {
                    fs = (size / (1024D * 1024 * 1024 * 1024 * 1024 * 1024 * 1024 * 1024));
                    nom = "YiB";
                }
                else if ((size >= (1024D * 1024 * 1024 * 1024 * 1024 * 1024 * 1024)))
                {
                    fs = (size / (1024D * 1024 * 1024 * 1024 * 1024 * 1024 * 1024));
                    nom = "ZiB";
                }
                else if ((size >= (1024L * 1024 * 1024 * 1024 * 1024 * 1024)))
                {
                    fs = (size / (1024L * 1024 * 1024 * 1024 * 1024 * 1024));
                    nom = "EiB";
                }
                else if ((size >= (1024L * 1024 * 1024 * 1024 * 1024)))
                {
                    fs = (size / (1024L * 1024 * 1024 * 1024 * 1024));
                    nom = "PiB";
                }
                else if ((size >= (1024L * 1024 * 1024 * 1024)))
                {
                    fs = (size / (1024L * 1024 * 1024 * 1024));
                    nom = "TiB";
                }
                else if ((size >= (1024 * 1024 * 1024)))
                {
                    fs = (size / (1024 * 1024 * 1024));
                    nom = "GiB";
                }
                else if ((size >= (1024 * 1024)))
                {
                    fs = (size / (1024 * 1024));
                    nom = "MiB";
                }
                else if ((size >= (1024)))
                {
                    fs = (size / (1024));
                    nom = "KiB";
                }
                else
                {
                    fs = size;
                    nom = "B";
                }
            }
            else
            {
                if ((size >= (1000D * 1000 * 1000 * 1000 * 1000 * 1000 * 1000 * 1000)))
                {
                    fs = (size / (1000D * 1000 * 1000 * 1000 * 1000 * 1000 * 1000 * 1000));
                    nom = "YB";
                }
                else if ((size >= (1000D * 1000 * 1000 * 1000 * 1000 * 1000 * 1000)))
                {
                    fs = (size / (1000D * 1000 * 1000 * 1000 * 1000 * 1000 * 1000));
                    nom = "ZB";
                }
                else if ((size >= (1000L * 1000 * 1000 * 1000 * 1000 * 1000)))
                {
                    fs = (size / (1000L * 1000 * 1000 * 1000 * 1000 * 1000));
                    nom = "EB";
                }
                else if ((size >= (1000L * 1000 * 1000 * 1000 * 1000)))
                {
                    fs = (size / (1000L * 1000 * 1000 * 1000 * 1000));
                    nom = "PB";
                }
                else if ((size >= (1000L * 1000 * 1000 * 1000)))
                {
                    fs = (size / (1000L * 1000 * 1000 * 1000));
                    nom = "TB";
                }
                else if ((size >= (1000 * 1000 * 1000)))
                {
                    fs = (size / (1000 * 1000 * 1000));
                    nom = "GB";
                }
                else if ((size >= (1000 * 1000)))
                {
                    fs = (size / (1000 * 1000));
                    nom = "MB";
                }
                else if ((size >= (1000)))
                {
                    fs = (size / (1000));
                    nom = "KB";
                }
                else
                {
                    fs = size;
                    nom = "B";
                }
            }

            if (format != null)
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
        public static string PrintFriendlySpeed(long speed, string format = null)
        {
            double fs = 0;
            string nom = null;

            if ((speed >= (1024L * 1024 * 1024 * 1024)))
            {
                fs = (speed / (1024L * 1024 * 1024 * 1024));
                nom = "TB/s";
                //wow
            }
            else if ((speed >= (1024 * 1024 * 1024)))
            {
                fs = (speed / (1024 * 1024 * 1024));
                nom = "GB/s";
                // still wow
            }
            else if ((speed >= (1024 * 1024)))
            {
                fs = (speed / (1024 * 1024));
                nom = "MB/s";
                // okay
            }
            else if ((speed >= (1024)))
            {
                fs = (speed / (1024));
                nom = "KB/s";
                // fine.
            }
            else
            {
                fs = speed;
                nom = "B/s";
                // wow.
            }

            if (format != null)
            {
                return System.Math.Round(fs, 2).ToString(format) + " " + nom;
            }
            else
            {
                return System.Math.Round(fs, 2) + " " + nom;
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
            byte[] asc = null;
            string sOut = "";

            int i = 0;
            int c = 0;

            c = chrs.Length - 1;


            for (i = 0; i <= c; i++)
            {
                if (UrlAllowedChars.IndexOf(chrs[i]) != -1)
                {
                    sOut += chrs[i];
                }
                else
                {
                    asc = System.Text.UnicodeEncoding.Unicode.GetBytes("" + chrs[i]);
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

            string[] parse = Split(input, "%", WithToken: true, WithTokenIn: true);

            StringBuilder asc = new StringBuilder();

            int hv = 0;
            char[] str = null;

            foreach (string lv in parse)
            {
                str = lv.ToCharArray();
                if ((str.Length == 3) && (str[0] == '%') && (IsHexQ("" + str[1], ref hv)))
                {
                    asc.Append((char)hv);
                }
                else
                {
                    asc.Append(str);
                }
            }

            return asc.ToString();
        }

    }


    public static class StringExtensions
    {
        /// <summary>
        /// Parse a string into an array of strings using the specified parameters
        /// </summary>
        /// <param name="Scan">String to scan</param>
        /// <param name="Separator">Separator string</param>
        /// <param name="SkipQuote">Whether to skip over quote blocks</param>
        /// <param name="Unescape">Whether to unescape quotes.</param>
        /// <param name="QuoteChar">Quote character to use.</param>
        /// <param name="EscapeChar">Escape character to use.</param>
        /// <param name="WithToken">Include the token in the return array.</param>
        /// <param name="WithTokenIn">Attach the token to the beginning of every string separated by a token (except for string 0).  Requires WithToken to also be set to True.</param>
        /// <param name="Unquote">Remove quotes from around characters.</param>
        /// <returns>An array of strings.</returns>
        /// <remarks></remarks>
        public static string[] Split(this string Scan, string Separator, bool SkipQuote = false, bool Unescape = false, char QuoteChar = '"', char EscapeChar = '\\', bool Unquote = false, bool WithToken = false, bool WithTokenIn = false)
        {
            return TextTools.Split(Scan, Separator, SkipQuote, Unescape, QuoteChar, EscapeChar, Unquote, WithToken, WithTokenIn);
        }

   }


}