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
// Licensed Under the Microsoft Public License   
// ************************************************* ''

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using DataTools.Memory;
using DataTools.Win32Api;

namespace DataTools.Desktop
{
    public static class TrueTypeFont
    {

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        // This is TTF file header
        [StructLayout(LayoutKind.Sequential)]
        public struct TT_OFFSET_TABLE
        {
            public ushort uMajorVersion;
            public ushort uMinorVersion;
            public ushort uNumOfTables;
            public ushort uSearchRange;
            public ushort uEntrySelector;
            public ushort uRangeShift;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct TT_TABLE_DIRECTORY
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] szTag;
            public uint uCheckSum;
            public uint uOffset;
            public uint uLength;

            public string Tag
            {
                get
                {
                    return System.Text.Encoding.ASCII.GetString(szTag);
                }
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct TT_NAME_TABLE_HEADER
        {
            public ushort uFSelector;     // format selector. Always 0
            public ushort uNRCount;       // Name Records count
            public ushort uStorageOffset; // Offset for strings storage, from start of the table
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct TT_NAME_RECORD
        {
            public ushort uPlatformID;
            public ushort uEncodingID;
            public ushort uLanguageID;
            public ushort uNameID;
            public ushort uStringLength;
            public ushort uStringOffset; // from start of storage area
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Change the Endianness of a 16-bit unsigned integer
        /// </summary>
        /// <param name="val">A 16-bit number</param>
        /// <returns>The reverse endian format of val.</returns>
        /// <remarks></remarks>
        public static ushort Swap(ushort val)
        {
            byte v1;
            byte v2;
            v1 = (byte)(val & 0xFF);
            v2 = (byte)(val >> 8 & 0xFF);
            return (ushort)(v1 << 8 | v2);
        }

        /// <summary>
        /// Change the Endianness of a 32-bit unsigned integer
        /// </summary>
        /// <param name="val">A 32-bit number</param>
        /// <returns>The reverse endian format of val.</returns>
        /// <remarks></remarks>
        public static uint Swap(uint val)
        {
            ushort v1;
            ushort v2;
            v1 = (ushort)(val & 0xFFFFL);
            v2 = (ushort)(val >> 16 & 0xFFFFL);
            return (uint)Swap(v1) << 16 | Swap(v2);
        }

        /// <summary>
        /// Change the Endianness of a 64-bit unsigned integer
        /// </summary>
        /// <param name="val">A 64-bit number</param>
        /// <returns>The reverse endian format of val.</returns>
        /// <remarks></remarks>
        public static ulong Swap(ulong val)
        {
            uint v1;
            uint v2;
            v1 = (uint)(val & 0xFFFFFFFFUL);
            v2 = (uint)(val >> 32 & 0xFFFFFFFFUL);
            return (ulong)Swap(v1) << 32 | Swap(v2);
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public static string GetTTFName(string fileName)
        {
            FileStream fs;
            var oft = new TT_OFFSET_TABLE();
            var tdir = new TT_TABLE_DIRECTORY();
            var nth = new TT_NAME_TABLE_HEADER();
            var nr = new TT_NAME_RECORD();
            int i;
            int p;
            string sRet = null;
            try
            {
                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            }
            catch
            {
                //Interaction.MsgBox(ex.Message, MsgBoxStyle.Critical);
                return null;
            }

            FileTools.ReadStruct<TT_OFFSET_TABLE>(fs, ref oft);
            oft.uNumOfTables = Swap(oft.uNumOfTables);
            oft.uMajorVersion = Swap(oft.uMajorVersion);
            oft.uMinorVersion = Swap(oft.uMinorVersion);

            // Not a TrueType v1.0 font!
            if (oft.uMajorVersion != 1 || oft.uMinorVersion != 0)
                return null;
            var loopTo = oft.uNumOfTables - 1;
            for (i = 0; i <= loopTo; i++)
            {
                FileTools.ReadStruct<TT_TABLE_DIRECTORY>(fs, ref tdir);
                if (tdir.Tag.ToLower() == "name")
                {
                    tdir.uLength = Swap(tdir.uLength);
                    tdir.uOffset = Swap(tdir.uOffset);
                    break;
                }
            }

            // Exhausted all records, no name record found!
            if (i >= oft.uNumOfTables)
                return null;
            fs.Seek(tdir.uOffset, SeekOrigin.Begin);
            FileTools.ReadStruct<TT_NAME_TABLE_HEADER>(fs, ref nth);
            nth.uStorageOffset = Swap(nth.uStorageOffset);
            nth.uNRCount = Swap(nth.uNRCount);
            var loopTo1 = nth.uNRCount - 1;
            for (i = 0; i <= loopTo1; i++)
            {
                FileTools.ReadStruct<TT_NAME_RECORD>(fs, ref nr);
                nr.uNameID = Swap(nr.uNameID);
                if (nr.uNameID == 1)
                {
                    p = (int)fs.Position;
                    nr.uStringLength = Swap(nr.uStringLength);
                    nr.uStringOffset = Swap(nr.uStringOffset);
                    fs.Seek(tdir.uOffset + nr.uStringOffset + nth.uStorageOffset, SeekOrigin.Begin);
                    nr.uEncodingID = Swap(nr.uEncodingID);
                    nr.uPlatformID = Swap(nr.uPlatformID);
                    byte[] b;
                    b = new byte[nr.uStringLength];
                    fs.Read(b, 0, nr.uStringLength);

                    // Platform IDs: 0 = Unicode, 1 = Macintosh, 3 = Windows
                    if (nr.uPlatformID == 0)
                    {
                        sRet = System.Text.Encoding.BigEndianUnicode.GetString(b);
                    }
                    else
                    {
                        sRet = System.Text.Encoding.ASCII.GetString(b);
                    }

                    sRet = sRet.Trim('\0');
                    if (!string.IsNullOrEmpty(sRet))
                    {
                        break;
                    }

                    sRet = null;
                    fs.Seek(p, SeekOrigin.Begin);
                }
            }

            return sRet;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    }


    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    public enum SortOrder
    {
        Ascending,
        Descending
    }

    public enum FontWeight : int
    {
        DontCare = 0,
        Thin = 100,
        ExtraLight = 200,
        Light = 300,
        Normal = 400,
        Medium = 500,
        SemiBold = 600,
        Bold = 700,
        ExtraBold = 800,
        Heavy = 900
    }

    public enum FontCharSet : byte
    {
        ANSI = 0,
        Default = 1,
        Symbol = 2,
        ShiftJIS = 128,
        Hangeul = 129,
        Hangul = 129,
        GB2312 = 134,
        ChineseBIG5 = 136,
        OEM = 255,
        Johab = 130,
        Hebrew = 177,
        Arabic = 178,
        Greek = 161,
        Turkish = 162,
        Vietnamese = 163,
        Thai = 222,
        EasternEurope = 238,
        Russian = 204,
        Mac = 77,
        Baltic = 186
    }

    [Flags]
    public enum FontFamilies
    {
        Decorative = 1,
        DontCare = 2,
        Modern = 4,
        Roman = 8,
        Script = 16,
        Swiss = 32
    }

    public enum FontPitch
    {
        Default = User32.FontPitchAndFamily.DEFAULT_PITCH,
        Variable = User32.FontPitchAndFamily.VARIABLE_PITCH,
        Fixed = User32.FontPitchAndFamily.FIXED_PITCH
    }


    /// <summary>
    /// Represents information about a font on the current system.
    /// </summary>
    public sealed class FontInfo
    {
        internal User32.ENUMLOGFONTEX elf;
        internal User32.LOGFONT lf;

        /// <summary>
        /// Gets the font name.
        /// </summary>
        /// <returns></returns>
        public string Name
        {
            get
            {
                return elf.elfFullName;
            }
        }

        /// <summary>
        /// Gets the font script.
        /// </summary>
        /// <returns></returns>
        public string Script
        {
            get
            {
                return elf.elfScript;
            }
        }

        /// <summary>
        /// Gets the font style.
        /// </summary>
        /// <returns></returns>
        public string Style
        {
            get
            {
                return elf.elfStyle;
            }
        }

        /// <summary>
        /// Gets the font weight.
        /// </summary>
        /// <returns></returns>
        public FontWeight Weight
        {
            get
            {
                return (FontWeight)(int)(lf.lfWeight);
            }
        }

        /// <summary>
        /// Gets the font character set.
        /// </summary>
        /// <returns></returns>
        public FontCharSet CharacterSet
        {
            get
            {
                return (FontCharSet)(lf.lfCharSet);
            }
        }

        /// <summary>
        /// Gets the font pitch.
        /// </summary>
        /// <returns></returns>
        public FontPitch Pitch
        {
            get
            {
                return (FontPitch)(int)(lf.lfPitchAndFamily & 3);
            }
        }

        /// <summary>
        /// Gets the font family.
        /// </summary>
        /// <returns></returns>
        public FontFamilies Family
        {
            get
            {
                User32.FontPitchAndFamily v = (User32.FontPitchAndFamily)(lf.lfPitchAndFamily >> 2 << 2);
                switch (v)
                {
                    case User32.FontPitchAndFamily.FF_DECORATIVE:
                        {
                            return FontFamilies.Decorative;
                        }

                    case User32.FontPitchAndFamily.FF_DONTCARE:
                        {
                            return FontFamilies.DontCare;
                        }

                    case User32.FontPitchAndFamily.FF_MODERN:
                        {
                            return FontFamilies.Modern;
                        }

                    case User32.FontPitchAndFamily.FF_ROMAN:
                        {
                            return FontFamilies.Roman;
                        }

                    case User32.FontPitchAndFamily.FF_SWISS:
                        {
                            return FontFamilies.Swiss;
                        }

                    case User32.FontPitchAndFamily.FF_SCRIPT:
                        {
                            return FontFamilies.Script;
                        }
                }

                return (FontFamilies)(int)(v);
            }
        }

        /// <summary>
        /// Copy the ENUMLOGFONTEX structure for this object into a memory buffer.
        /// </summary>
        /// <param name="lpElf">Pointer to a buffer to receive the ENUMLOGFONTEX structure.  The memory must already be allocated and freed by the caller.</param>
        public void GetElfEx(IntPtr lpElf)
        {
            var mm = new MemPtr(lpElf);
            mm.FromStruct(elf);
        }

        internal FontInfo(User32.ENUMLOGFONTEX elf)
        {
            this.elf = elf;
            lf = elf.elfLogFont;
        }

        public override string ToString()
        {
            return Name;
        }
    }


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

        private delegate int EnumFontFamExProc(ref User32.ENUMLOGFONTEX lpelfe, IntPtr lpntme, uint FontType, IntPtr lparam);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        private static extern int EnumFontFamiliesEx(IntPtr hdc, IntPtr lpLogFont, [MarshalAs(UnmanagedType.FunctionPtr)] EnumFontFamExProc lpEnumFontFamExProc, IntPtr lparam, uint dwflags);

        /// <summary>
        /// Returns a static collection of all fonts on the system in the default character set.
        /// </summary>
        /// <returns></returns>
        public static FontCollection SystemFonts { get; private set; }

        private static bool CheckFamily(User32.LOGFONT lf, FontFamilies families)
        {
            User32.FontPitchAndFamily v = (User32.FontPitchAndFamily)(lf.lfPitchAndFamily >> 2 << 2);
            switch (v)
            {
                case User32.FontPitchAndFamily.FF_DECORATIVE:
                    {
                        if ((families & FontFamilies.Decorative) == 0)
                            return false;
                        break;
                    }

                case User32.FontPitchAndFamily.FF_DONTCARE:
                    {
                        return families == FontFamilies.DontCare ? true : false;
                    }

                case User32.FontPitchAndFamily.FF_MODERN:
                    {
                        if ((families & FontFamilies.Modern) == 0)
                            return false;
                        break;
                    }

                case User32.FontPitchAndFamily.FF_ROMAN:
                    {
                        if ((families & FontFamilies.Roman) == 0)
                            return false;
                        break;
                    }

                case User32.FontPitchAndFamily.FF_SWISS:
                    {
                        if ((families & FontFamilies.Swiss) == 0)
                            return false;
                        break;
                    }

                case User32.FontPitchAndFamily.FF_SCRIPT:
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
            
            var fonts = new List<User32.ENUMLOGFONTEX>();
            
            var lf = new User32.LOGFONT();
            
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

            e = EnumFontFamiliesEx(hdc, mm, (ref User32.ENUMLOGFONTEX lpelfe, IntPtr lpntme, uint FontType, IntPtr lParam) =>
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

            /* TODO ERROR: Skipped RegionDirectiveTrivia */
            private bool disposedValue; // To detect redundant calls

            // IDisposable
            protected virtual void Dispose(bool disposing)
            {
                disposedValue = true;
            }

            public void Dispose()
            {
                Dispose(true);
            }
            /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        }
    }


    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
}