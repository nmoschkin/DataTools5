// ************************************************* ''
// DataTools C# Native Utility Library For Windows - Interop
//
// Module: TrueType.
//         Code to read TrueType font file information
//         Adapted from the CodeProject article: http://www.codeproject.com/Articles/2293/Retrieving-font-name-from-TTF-file?msg=4714219#xx4714219xx
//
// 
// Copyright (C) 2011-2020 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the MIT License   
// ************************************************* ''

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

using DataTools.Win32;
using DataTools.Win32.Memory;

namespace DataTools.Desktop
{
    /// <summary>
    /// A static collection of fonts returned by the <see cref="FontCollection.GetFonts"/> static method according to the specified criteria.
    /// If you require a list of all fonts on the system in the default character set, reference <see cref="FontCollection.SystemFonts"/>, instead.
    /// </summary>
    public sealed class FontCollection : ICollection<FontInfo>
    {
        public enum FontSearchOptions
        {
            Contains = 0,
            BeginsWith = 1,
            EndsWith = 2
        }

        private List<FontInfo> _List = new List<FontInfo>();

        private delegate int EnumFontFamExProc(ref ENUMLOGFONTEX lpelfe, IntPtr lpntme, uint FontType, IntPtr lparam);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        private static extern int EnumFontFamiliesEx(IntPtr hdc, IntPtr lpLogFont, [MarshalAs(UnmanagedType.FunctionPtr)] EnumFontFamExProc lpEnumFontFamExProc, IntPtr lparam, uint dwflags);

        /// <summary>
        /// Returns a static collection of all fonts on the system in the default character set.
        /// </summary>
        /// <returns></returns>
        public static FontCollection SystemFonts { get; private set; }

        private static bool CheckFamily(LOGFONT lf, FontFamilies families)
        {
            FontPitchAndFamily v = (FontPitchAndFamily)(lf.lfPitchAndFamily >> 2 << 2);
            switch (v)
            {
                case FontPitchAndFamily.FF_DECORATIVE:
                    {
                        if ((families & FontFamilies.Decorative) == 0)
                            return false;
                        break;
                    }

                case FontPitchAndFamily.FF_DONTCARE:
                    {
                        return families == FontFamilies.DontCare ? true : false;
                    }

                case FontPitchAndFamily.FF_MODERN:
                    {
                        if ((families & FontFamilies.Modern) == 0)
                            return false;
                        break;
                    }

                case FontPitchAndFamily.FF_ROMAN:
                    {
                        if ((families & FontFamilies.Roman) == 0)
                            return false;
                        break;
                    }

                case FontPitchAndFamily.FF_SWISS:
                    {
                        if ((families & FontFamilies.Swiss) == 0)
                            return false;
                        break;
                    }

                case FontPitchAndFamily.FF_SCRIPT:
                    {
                        if ((families & FontFamilies.Script) == 0)
                            return false;
                        break;
                    }
            }

            return true;
        }

        /// <summary>
        /// Gets a collection of fonts based on the specified criteria.
        /// </summary>
        /// <param name="families">Bit Field representing which font families to retrieve.</param>
        /// <param name="pitch">Specify the desired pitch.</param>
        /// <param name="charset">Specify the desired character set.</param>
        /// <param name="weight">Specify the desired weight.</param>
        /// <param name="Script">Specify the desired script(s) (this can be a String or an array of Strings).</param>
        /// <param name="Style">Specify the desired style(s) (this can be a String or an array of Strings).</param>
        /// <returns></returns>
        public static FontCollection GetFonts(FontFamilies families = FontFamilies.DontCare, FontPitch pitch = FontPitch.Default, FontCharSet charset = FontCharSet.Default, FontWeight weight = FontWeight.DontCare, object Script = null, object Style = null)
        {
            IntPtr hdc;

            var fonts = new List<ENUMLOGFONTEX>();

            var lf = new LOGFONT();

            string s;

            MemPtr mm = new MemPtr();

            string[] wscript;
            string[] wstyle;

            if (Script is null)
            {
                wscript = new[] { "Western" };
            }
            else if (Script is string)
            {
                wscript = new[] { (string)(Script) };
            }
            else if (Script is string[])
            {
                wscript = (string[])Script;
            }
            else
            {
                throw new ArgumentException("Invalid parameter type for Script");
            }

            if (Style is null)
            {
                wstyle = new[] { "", "Normal", "Regular" };
            }
            else if (Style is string)
            {
                wstyle = new[] { (string)(Style) };
            }
            else if (Style is string[])
            {
                wstyle = (string[])Style;
            }
            else
            {
                throw new ArgumentException("Invalid parameter type for Style");
            }

            lf.lfCharSet = (byte)charset;
            lf.lfFaceName = "";
            mm.Alloc(Marshal.SizeOf(lf));
            mm.FromStruct(lf);
            hdc = User32.CreateDC("DISPLAY", null, IntPtr.Zero, IntPtr.Zero);

            int e;
            bool bo = false;

            e = EnumFontFamiliesEx(hdc, mm, (ref ENUMLOGFONTEX lpelfe, IntPtr lpntme, uint FontType, IntPtr lParam) =>
            {
                int z;
                if (fonts is null)
                    z = 0;
                else
                    z = fonts.Count;


                // make sure it's the normal, regular version

                bo = false;
                foreach (var y in wstyle)
                {
                    if ((y.ToLower() ?? "") == (lpelfe.elfStyle.ToLower() ?? ""))
                    {
                        bo = true;
                        break;
                    }
                }

                if (bo == false)
                    return 1;
                bo = false;
                foreach (var y in wscript)
                {
                    if ((y.ToLower() ?? "") == (lpelfe.elfScript.ToLower() ?? ""))
                    {
                        bo = true;
                        break;
                    }
                }

                if (bo == false)
                    return 1;
                bo = false;
                if (weight != FontWeight.DontCare && lpelfe.elfLogFont.lfWeight != (int)weight)
                    return 1;

                // we don't really need two of the same font.
                if (z > 0)
                {
                    if ((lpelfe.elfFullName ?? "") == (fonts[z - 1].elfFullName ?? ""))
                        return 1;
                }

                // the @ indicates a vertical writing font which we definitely do not want.
                if (lpelfe.elfFullName.Substring(0, 1) == "@")
                    return 1;
                if (!CheckFamily(lpelfe.elfLogFont, families))
                    return 1;

                // lpelfe.elfLogFont.lfCharSet = charset
                // If (lpelfe.elfLogFont.lfCharSet <> charset) Then Return 1

                if (pitch != FontPitch.Default && (lpelfe.elfLogFont.lfPitchAndFamily & 3) != (int)pitch)
                    return 1;
                fonts.Add(lpelfe);
                return 1;
            }, IntPtr.Zero, 0U);
            User32.DeleteDC(hdc);
            mm.Free();
            if (e == 0)
            {
                e = User32.GetLastError();
                s = NativeError.Message;
            }

            FontInfo nf;
            var ccol = new FontCollection();
            foreach (var f in fonts)
            {
                nf = new FontInfo(f);
                ccol.Add(nf);
            }

            ccol.Sort();
            return ccol;
        }


        /// <summary>
        /// Merges two font collections, returning a new collection object.
        /// </summary>
        /// <param name="col1">The first font collection.</param>
        /// <param name="col2">The second font collection.</param>
        /// <param name="sortProperty">Optionally specify a sort property ("Name" is the default).  If you specify a property that cannot be found, "Name" will be used.</param>
        /// <param name="SortOrder">Optionally specify ascending or descending order.</param>
        /// <returns></returns>
        public static FontCollection MergeCollections(FontCollection col1, FontCollection col2, string SortProperty = "Name", SortOrder SortOrder = SortOrder.Ascending)
        {
            var col3 = new FontCollection();
            foreach (var c in col1)
                col3.Add(c);
            foreach (var c in col2)
                col3.Add(c);
            col3.Sort(SortProperty, SortOrder);
            return col3;
        }

        /// <summary>
        /// Sort this collection by the given property and sort order.
        /// </summary>
        /// <param name="SortProperty">The property name to sort by.</param>
        /// <param name="SortOrder">The sort order.</param>
        public void Sort(string SortProperty = "Name", SortOrder SortOrder = SortOrder.Ascending)
        {
            System.Reflection.PropertyInfo pi = null;
            try
            {
                pi = typeof(FontInfo).GetProperty(SortProperty);
            }
            catch
            {
            }

            if (pi is null)
            {
                pi = typeof(FontInfo).GetProperty(nameof(FontInfo.Name));

            }

            _List.Sort(new Comparison<FontInfo>((a, b) =>
            {
                int x = 0;
                string o1;
                string o2;
                o1 = (string)(pi.GetValue(a));
                o2 = (string)(pi.GetValue(b));
                if (o1 is string)
                {
                    x = string.Compare(o1, o2);
                }
                else if (string.Compare(o1, o2, true) < 0)
                {
                    x = -1;
                }
                else if (string.Compare(o2, o1, true) < 0)
                {
                    x = 1;
                }

                if (SortOrder == SortOrder.Descending)
                    x = -x;
                return x;
            }));
        }

        /// <summary>
        /// This object is not creatable.
        /// </summary>
        private FontCollection()
        {
        }

        /// <summary>
        /// Initialize the master system font list.
        /// </summary>
        static FontCollection()
        {
            SystemFonts = GetFonts();
        }

        /// <summary>
        /// Search for fonts whose names contain the specified string.
        /// </summary>
        /// <param name="pattern">String to look for.</param>
        /// <param name="caseSensitive">Specifies whether the search is case-sensitive.</param>
        /// <returns></returns>
        public FontCollection Search(string pattern, bool caseSensitive = false, FontSearchOptions searchOptions = FontSearchOptions.Contains)
        {
            var l = new FontCollection();
            string s;
            string t;
            int i = pattern.Length;
            int j;
            s = pattern;
            if (caseSensitive == false)
                s = s.ToLower();
            foreach (var f in this)
            {
                t = f.elf.elfFullName;
                if (caseSensitive == false)
                    t = t.ToLower();
                switch (searchOptions)
                {
                    case FontSearchOptions.Contains:
                        {
                            if (t.Contains(s))
                            {
                                l.Add(f);
                            }

                            break;
                        }

                    case FontSearchOptions.BeginsWith:
                        {
                            if (t.Length >= s.Length)
                            {
                                if ((t.Substring(0, i) ?? "") == (s ?? ""))
                                {
                                    l.Add(f);
                                }
                            }

                            break;
                        }

                    case FontSearchOptions.EndsWith:
                        {
                            if (t.Length >= s.Length)
                            {
                                j = t.Length - s.Length;
                                if ((t.Substring(j, i) ?? "") == (s ?? ""))
                                {
                                    l.Add(f);
                                }
                            }

                            break;
                        }
                }
            }

            if (l.Count == 0)
                return null;
            return l;
        }

        /// <summary>
        /// Returns the FontInfo object at the specified index.
        /// </summary>
        /// <param name="index">Index</param>
        /// <returns></returns>
        public FontInfo this[int index]
        {
            get
            {
                return _List[index];
            }
        }

        /// <summary>
        /// Returns the number of items in this collection.
        /// </summary>
        /// <returns></returns>
        public int Count
        {
            get
            {
                return _List.Count;
            }
        }


        public bool IsReadOnly => true;

        /// <summary>
        /// Indicates whether this collection contains the specified FontInfo object.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(FontInfo item)
        {
            return _List.Contains(item);
        }

        /// <summary>
        /// Copies the entire collection into a compatible 1-dimensional array
        /// </summary>
        /// <param name="array">The array into which to copy the data.</param>
        /// <param name="arrayIndex">The zero-based array index at which copying begins.</param>
        public void CopyTo(FontInfo[] array, int arrayIndex)
        {
            _List.CopyTo(array, arrayIndex);
        }

        public IEnumerator<FontInfo> GetEnumerator()
        {
            return new FontEnumer(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new FontEnumer(this);
        }

        public void Add(FontInfo item)
        {
            ((ICollection<FontInfo>)_List).Add(item);
        }

        public void Clear()
        {
            ((ICollection<FontInfo>)_List).Clear();
        }

        public bool Remove(FontInfo item)
        {
            return ((ICollection<FontInfo>)_List).Remove(item);
        }

        private class FontEnumer : IEnumerator<FontInfo>
        {
            private FontCollection _obj;
            private int _pos = -1;

            public FontEnumer(FontCollection obj)
            {
                _obj = obj;
            }

            public FontInfo Current
            {
                get
                {
                    return _obj[_pos];
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    return _obj[_pos];
                }
            }

            public void Reset()
            {
                _pos = -1;
            }

            public bool MoveNext()
            {
                _pos += 1;
                if (_pos >= _obj.Count)
                    return false;
                return true;
            }

            protected bool disposedValue; // To detect redundant calls

            protected virtual void Dispose(bool disposing)
            {
                disposedValue = true;

                if (disposing)
                {
                    _pos = -1;
                    _obj = null;
                }
            }

            public void Dispose()
            {
                if (disposedValue) return;
                Dispose(true);
            }
        }
    }
}
