using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using DataTools.Text;
using DataTools.Desktop.Structures;

namespace DataTools.Desktop.Unified
{

    /* TODO ERROR: Skipped RegionDirectiveTrivia */

    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    /// <summary>
    /// Formatting flags for UniColor.ToString(format)
    /// </summary>
    /// <remarks></remarks>
    [Flags]
    public enum UniColorFormatOptions
    {
        /// <summary>
        /// Displays the color name of a named color or web-formatted color hex code.
        /// </summary>
        /// <remarks></remarks>
        Default = 0,

        /// <summary>
        /// Prints the decimal number for the value.
        /// </summary>
        /// <remarks></remarks>
        DecimalDigit = 1,

        /// <summary>
        /// Returns HTML-style hex code color syntax.
        /// </summary>
        /// <remarks></remarks>
        Hex = 2,

        /// <summary>
        /// Returns C-style hex code.
        /// </summary>
        /// <remarks></remarks>
        CStyleHex = 2 | 4,

        /// <summary>
        /// Returns VB-style hex code.
        /// </summary>
        /// <remarks></remarks>
        VBStyleHex = 2 | 8,

        /// <summary>
        /// Returns assembly-style hex code.
        /// </summary>
        /// <remarks></remarks>
        AsmStyleHex = 2 | 16,

        /// <summary>
        /// Returns a web style hex code.
        /// </summary>
        /// <remarks></remarks>
        WebStyleHex = 2 | 512,

        /// <summary>
        /// Returns space-separated values.
        /// </summary>
        /// <remarks></remarks>
        Spaced = 32,

        /// <summary>
        /// Returns comma-separated values.
        /// </summary>
        /// <remarks></remarks>
        CommaDelimited = 64,

        /// <summary>
        /// Returns the RGB decimal values.
        /// </summary>
        /// <remarks></remarks>
        Rgb = 128,

        /// <summary>
        /// Returns the ARGB decimal values.
        /// </summary>
        /// <remarks></remarks>
        Argb = 256,

        /// <summary>
        /// Prints in web-ready format.  Cannot be used alone.
        /// </summary>
        /// <remarks></remarks>
        WebFormat = 512,

        /// <summary>
        /// Adds rgb() enclosure for the web.
        /// </summary>
        /// <remarks></remarks>
        RgbWebFormat = 512 | 128 | 64,

        /// <summary>
        /// Adds argb() enclosure for the web.
        /// </summary>
        /// <remarks></remarks>
        ArgbWebFormat = 512 | 256 | 64,

        /// <summary>
        /// Prints the #RRGGBB web format color code.
        /// </summary>
        /// <remarks></remarks>
        HexRgbWebFormat = 512 | 2 | 128,

        /// <summary>
        /// Prints the #AARRGGBB web format color code.
        /// </summary>
        /// <remarks></remarks>
        HexArgbWebFormat = 512 | 2 | 256,

        /// <summary>
        /// Add details to a named color such as opacity level and hex code in brackets (if Hex is specifed).
        /// </summary>
        /// <remarks></remarks>
        DetailNamedColors = 0x2000,

        /// <summary>
        /// Reverses the order of the numbers.
        /// </summary>
        /// <remarks></remarks>
        Reverse = 0x4000,

        /// <summary>
        /// Print hex letters in lower case.
        /// </summary>
        /// <remarks></remarks>
        LowerCase = 0x8000
    }

    /// <summary>
    /// Powerful 32-bit color structure with many features including automatic
    /// casting to System.Windows.Media.(Color And System.Drawing) = System.Drawing.Color and
    /// an array of formatting options and string parsing abilities.
    /// 
    /// Supports the catalog of all named colors for WPF and WinForms
    /// with automatic named-color detection and smart opacity handling.
    /// 
    /// Unlike other structures such as these, the A, R, G, and B channels
    /// can all be set by the user, independently.
    /// 
    /// The structure is binary compatible with 32-bit color values,
    /// and can be used in any interop call that requires such a value,
    /// without any modification or type coercion.
    /// </summary>
    /// <remarks></remarks>
    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
    public struct UniColor
    {
        public static readonly UniColor Empty = new UniColor(0, 0, 0, 0);
        [FieldOffset(0)]
        private uint _Value;
        [FieldOffset(0)]
        private int _intvalue;
        [FieldOffset(3)]
        private byte _A;
        [FieldOffset(2)]
        private byte _R;
        [FieldOffset(1)]
        private byte _G;
        [FieldOffset(0)]
        private byte _B;

        /// <summary>
        /// Indicates whether the default behavior of ToString() is to display a detailed description of the current named color.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool DetailedDefaultToString { get; set; } = false;

        /// <summary>
        /// Gets or sets the 32 bit unsigned integer value of this color.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public uint Value
        {
            get
            {
                uint ValueRet = default;
                ValueRet = _Value;
                return ValueRet;
            }

            set
            {
                _Value = value;
            }
        }

        /// <summary>
        /// Gets or sets the 32 bit signed integer value of this color.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int IntValue
        {
            get
            {
                return _intvalue;
            }

            set
            {
                _intvalue = value;
            }
        }

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Alpha channel
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte A
        {
            get
            {
                return _A;
            }

            set
            {
                _A = value;
            }
        }

        /// <summary>
        /// Red channel
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte R
        {
            get
            {
                return _R;
            }

            set
            {
                _R = value;
            }
        }

        /// <summary>
        /// Green channel
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte G
        {
            get
            {
                return _G;
            }

            set
            {
                _G = value;
            }
        }

        /// <summary>
        /// Blue channel
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte B
        {
            get
            {
                return _B;
            }

            set
            {
                _B = value;
            }
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Gets the byte array for the current color.
        /// </summary>
        /// <param name="reverse">True to return a reversed array.</param>
        /// <returns>An array of 4 bytes.</returns>
        /// <remarks></remarks>
        public byte[] GetBytes(bool reverse = false)
        {
            var gb = BitConverter.GetBytes(_Value);
            if (!reverse)
                Array.Reverse(gb);
            return gb;
        }

        /// <summary>
        /// Sets the color from the byte array.
        /// </summary>
        /// <param name="b">The input byte array.</param>
        /// <param name="reversed">True if the input array is reversed.</param>
        /// <returns>True if successful.</returns>
        /// <remarks></remarks>
        public bool SetBytes(byte[] b, bool reversed = false)
        {
            if (reversed)
                Array.Reverse(b);
            _Value = BitConverter.ToUInt32(b, 0);
            return true;
        }

        /// <summary>
        /// Gets the values as an array of integers
        /// </summary>
        /// <param name="reverse">True to return a reversed array.</param>
        /// <returns>An array of 4 integers.</returns>
        /// <remarks></remarks>
        public int[] GetIntegers(bool reverse = false)
        {
            var b = GetBytes(reverse);
            int[] i;
            i = new int[b.Length];
            for (int x = 0; x <= 3; x++)
                i[x] = b[x];
            return i;
        }

        /// <summary>
        /// Sets the color values from an integer array.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="reversed">True if the input array is reversed.</param>
        /// <returns>True if successful.  Overflows are not excepted.</returns>
        /// <remarks></remarks>
        public bool SetIntegers(int[] i, bool reversed = false)
        {
            if (i.Length != 4)
                return false;
            var b = new byte[4];
            for (int x = 0; x <= 3; x++)
            {
                if (i[x] < 0 || i[x] > 255)
                    return false;
                b[x] = (byte)i[x];
            }

            SetBytes(b, reversed);
            return true;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Initialize a new UniColor structure with the given UInteger
        /// </summary>
        /// <param name="color"></param>
        /// <remarks></remarks>
        public UniColor(uint color)
        {
            _intvalue = 0;
            _A = _R = _G = _B = 0;

            _Value = color;
        }

        /// <summary>
        /// Initialize a new UniColor structure with the given Integer
        /// </summary>
        /// <param name="color"></param>
        /// <remarks></remarks>
        public UniColor(int color)
        {
            _Value = 0;
            _intvalue = color;
            _A = _R = _G = _B = 0;

        }

        public UniColor(string color)
        {
            bool succeed = false;

            Color c;
            System.Windows.Media.Color c2;
            c = TryFindNamedWinFormsColor(color, ref succeed);

            if (!succeed)
            {
                c2 = TryFindNamedWPFColor(color, ref succeed);

                _intvalue = 0;
                _Value = 0;

                if (succeed)
                {
                    _A = c2.A;
                    _R = c2.R;
                    _G = c2.G;
                    _B = c2.B;

                    return;
                }

                _A = _R = _G = _B = 0;
                return;
            }

            _Value = 0;
            _A = _R = _G = _B = 0;

            _intvalue = c.ToArgb();
        }

        /// <summary>
        /// Initialize a new UniColor structure with a color of the given name.
        /// If the name is not found, an argument exception is throw.
        /// </summary>
        /// <param name="Color">The name of the color to create.</param>
        /// <remarks></remarks>
        public UniColor(string Color, ref bool succeed)
        {
            Color c;
            System.Windows.Media.Color c2;
            c = TryFindNamedWinFormsColor(Color, ref succeed);
            if (!succeed)
            {
                c2 = TryFindNamedWPFColor(Color, ref succeed);
                if (!succeed)
                {
                    _A = _R = _G = _B = 0;
                    _Value = 0;
                    _intvalue = 0;

                    return;
                }
                else
                {
                    _Value = 0;
                    _intvalue = 0;

                    _A = c2.A;
                    _R = c2.R;
                    _G = c2.G;
                    _B = c2.B;
                }
            }
            else
            {
                _A = _R = _G = _B = 0;
                _Value = 0;
                _intvalue = c.ToArgb();
            }
        }

        /// <summary>
        /// Initialize a new UniColor structure with the given ARGB values.
        /// </summary>
        /// <param name="a">Alpha</param>
        /// <param name="r">Red</param>
        /// <param name="g">Green</param>
        /// <param name="b">Blue</param>
        /// <remarks></remarks>
        public UniColor(byte a, byte r, byte g, byte b)
        {
            _intvalue = 0;
            _Value = 0;
            _A = a;
            _R = r;
            _G = g;
            _B = b;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Converts this color structure into its string representation.
        /// DetailedDefaultToString affects the behavior of this function.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            return ToString(UniColorFormatOptions.Default);
        }

        /// <summary>
        /// Richly format the color for a variety of scenarios including named color detection.
        /// </summary>
        /// <param name="format">Extensive formatting flags.  Some may not be used in conjunction with others.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string ToString(UniColorFormatOptions format)
        {
            string hexCase = (format & UniColorFormatOptions.LowerCase) == UniColorFormatOptions.LowerCase ? "x" : "X";
            var argbVals = GetIntegers((format & UniColorFormatOptions.Reverse) == UniColorFormatOptions.Reverse);
            string[] argbStrs = null;
            string str1 = "";
            string str2 = "";
            int i;
            int c;
            if ((format & UniColorFormatOptions.DecimalDigit) == UniColorFormatOptions.DecimalDigit)
            {
                if ((format & UniColorFormatOptions.CommaDelimited) == UniColorFormatOptions.CommaDelimited)
                {
                    return _Value.ToString("#,##0");
                }
                else
                {
                    return _Value.ToString();
                }
            }
            else if ((format & UniColorFormatOptions.HexRgbWebFormat) == UniColorFormatOptions.HexRgbWebFormat)
            {
                return "#" + (_Value & 0xFFFFFFL).ToString(hexCase + "6");
            }
            else if ((format & UniColorFormatOptions.HexArgbWebFormat) == UniColorFormatOptions.HexArgbWebFormat || format == UniColorFormatOptions.Default || (format & (UniColorFormatOptions.DetailNamedColors | UniColorFormatOptions.Hex)) > UniColorFormatOptions.Hex)
            {
                str2 = "#" + (_Value & 0xFFFFFFFFL).ToString(hexCase + "8");
                str1 = TryGetColorName(this);
                if ((format & (UniColorFormatOptions.DetailNamedColors | UniColorFormatOptions.Hex)) > UniColorFormatOptions.Hex || format == UniColorFormatOptions.Default & DetailedDefaultToString == true)
                {
                    if (str1 is object)
                    {
                        if (A == 255)
                        {
                            return str1 + " [" + str2 + "]";
                        }

                        double ax = A;
                        ax = ax / 255d * 100d;
                        str1 += " (" + ax.ToString("0") + "% Opacity";
                        if ((format & UniColorFormatOptions.Hex) == UniColorFormatOptions.Hex)
                        {
                            str1 += ", [" + str2 + "])";
                        }
                        else
                        {
                            str1 += ")";
                        }

                        return str1;
                    }
                }
                else if (format == UniColorFormatOptions.Default && str1 is object)
                {
                    return str1;
                }

                return str2;
            }
            else if ((format & UniColorFormatOptions.Hex) == UniColorFormatOptions.Hex)
            {
                str1 = "";
                if ((format & UniColorFormatOptions.Argb) == UniColorFormatOptions.Argb)
                {
                    c = 3;
                    argbStrs = new string[4];
                    for (i = 0; i <= 3; i++)
                        str1 += argbVals[i].ToString(hexCase + "2");
                }
                else if ((format & UniColorFormatOptions.Rgb) == UniColorFormatOptions.Rgb)
                {
                    c = 2;
                    argbStrs = new string[3];
                    for (i = 0; i <= 2; i++)
                        str1 += argbVals[i].ToString(hexCase + "2");
                }
                else
                {
                    throw new ArgumentException("Must specify either Argb or Rgb in the format flags.");
                }

                if ((format & UniColorFormatOptions.AsmStyleHex) == UniColorFormatOptions.AsmStyleHex)
                {
                    var loopTo = c;
                    for (i = 0; i <= loopTo; i++)
                        str1 += "h";
                }
                else if ((format & UniColorFormatOptions.CStyleHex) == UniColorFormatOptions.CStyleHex)
                {
                    var loopTo1 = c;
                    for (i = 0; i <= loopTo1; i++)
                        str1 = "0x" + str1;
                }
                else if ((format & UniColorFormatOptions.VBStyleHex) == UniColorFormatOptions.VBStyleHex)
                {
                    var loopTo2 = c;
                    for (i = 0; i <= loopTo2; i++)
                        str1 = "&H" + str1;
                }
                else if ((format & UniColorFormatOptions.WebStyleHex) == UniColorFormatOptions.WebStyleHex)
                {
                    var loopTo3 = c;
                    for (i = 0; i <= loopTo3; i++)
                        str1 = "#" + str1;
                }

                return str1;
            }
            else if ((format & UniColorFormatOptions.Argb) == UniColorFormatOptions.Argb)
            {
                c = 3;
                argbStrs = new string[4];
                for (i = 0; i <= 3; i++)
                    argbStrs[i] = argbVals[i].ToString();
            }
            else if ((format & UniColorFormatOptions.Rgb) == UniColorFormatOptions.Rgb)
            {
                c = 2;
                argbStrs = new string[3];
                for (i = 0; i <= 2; i++)
                    argbStrs[i] = argbVals[i].ToString();
            }
            else
            {
                throw new ArgumentException("Must specify either Argb or Rgb in the format flags.");
            }

            if ((format & UniColorFormatOptions.ArgbWebFormat) == UniColorFormatOptions.ArgbWebFormat)
            {
                if ((format & UniColorFormatOptions.Reverse) == UniColorFormatOptions.Reverse)
                {
                    str1 = "bgra(";
                }
                else
                {
                    str1 = "argb(";
                }

                str2 = ")";
            }
            else if ((format & UniColorFormatOptions.RgbWebFormat) == UniColorFormatOptions.RgbWebFormat)
            {
                if ((format & UniColorFormatOptions.Reverse) == UniColorFormatOptions.Reverse)
                {
                    str1 = "bgr(";
                }
                else
                {
                    str1 = "rgb(";
                }

                str2 = ")";
            }

            var loopTo4 = c;
            for (i = 0; i <= loopTo4; i++)
            {
                if (i > 0)
                {
                    if ((format & UniColorFormatOptions.CommaDelimited) == UniColorFormatOptions.CommaDelimited)
                    {
                        str1 += ",";
                    }

                    if ((format & UniColorFormatOptions.Spaced) == UniColorFormatOptions.Spaced)
                    {
                        str1 += " ";
                    }
                }

                str1 += argbStrs[i];
            }

            str1 += str2;
            return str1;
        }

        /// <summary>
        /// Parse a string value into a new UniColor structure.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static UniColor Parse(string value)
        {
            var ch = new List<char>();
            var a = default(int);
            string[] s;
            int i = 0;
            int c;
            string t;
            int[] l;
            bool flip = false;
            bool alf = false;

            // if this is a straight integer value, we can return a new color right away.
            bool x = int.TryParse(value.Trim().Trim('#'), System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.CurrentCulture, out i);

            unchecked
            {
                if (x)
                {
                    if (i == 0)
                    {
                        i = (int)0xFF000000;
                    }
                    else if (i == 0xFFF)
                    {
                        i = (int)0xFFFFFFFF;
                    }
                    else if ((i & 0xFF000000) == 0 && (i & 0xFFFFFF) != 0)
                    {
                        i = (int)(i | 0xFF000000);
                    }

                    return new UniColor(i);
                }

            }

            // on with the show!

            // first let's parse some separated values, here.

            value = value.ToLower();
            if (value.Substring(0, 5) == "argb(")
            {
                value = value.Substring(5).Replace(")", "");
            }
            else if (value.Substring(0, 4) == "rgb(")
            {
                value = value.Substring(4).Replace(")", "");
            }
            else if (value.Substring(0, 5) == "rgba(")
            {
                value = value.Substring(5).Replace(")", "");
                alf = true;
            }
            else if (value.Substring(0, 5) == "bgra(")
            {
                value = value.Substring(5).Replace(")", "");
                flip = true;
            }
            else if (value.Substring(0, 4) == "bgr(")
            {
                value = value.Substring(4).Replace(")", "");
                flip = true;
            }

            if (value.IndexOf(",") >= 0 || value.IndexOf(" ") >= 0)
            {
                if (value.IndexOf(",") >= 0)
                {
                    s = TextTools.Split(value, ",");
                }
                else
                {
                    s = TextTools.Split(value, " ");
                }

                if (s.Count() < 3 || s.Count() > 4)
                {
                    throw new InvalidCastException($"That string '{value}' cannot be converted into a color, {s.Count()} parameters found.");
                }

                c = s.Count() - 1;
                l = new int[c + 1];
                bool b = true;
                byte by;
                float f;
                var loopTo = c;
                for (i = 0; i <= loopTo; i++)
                {
                    t = s[i];
                    t = t.Trim();
                    if (alf && i == 3 && float.TryParse(t, out f))
                    {
                        by = (byte)(f * 255f);
                        l[i] = by;
                    }
                    else if (byte.TryParse(t, out by) == true)
                    {
                        l[i] = by;
                    }
                    else
                    {
                        b = false;
                        break;
                    }
                }

                if (flip)
                    Array.Reverse(l);
                if (b == true)
                {
                    var u = new UniColor();
                    switch (c + 1)
                    {
                        case 3:
                            {
                                u.A = 255;
                                u.R = (byte)l[0];
                                u.G = (byte)l[1];
                                u.B = (byte)l[2];
                                break;
                            }

                        case 4:
                            {
                                if (alf)
                                {
                                    u.R = (byte)l[0];
                                    u.G = (byte)l[1];
                                    u.B = (byte)l[2];
                                    u.A = (byte)l[3];
                                }
                                else
                                {
                                    u.A = (byte)l[0];
                                    u.R = (byte)l[1];
                                    u.G = (byte)l[2];
                                    u.B = (byte)l[3];
                                }

                                break;
                            }
                    }

                    return u;
                }
                else
                {
                    throw new InvalidCastException($"That string '{value}' cannot be converted into a color");
                }
            }

            value = TextTools.NoSpace(value);

            // First, let's see if it's a name:
            bool b1 = false;
            bool b2 = false;
            var c1 = TryFindNamedWPFColor(value, ref b1);
            var c2 = TryFindNamedWinFormsColor(value, ref b2);
            if (b1)
                return c1;
            if (b2)
                return c2;

            // okay, it's not a name, let's see if it's some kind of number.
            string chIn = value;
            c = chIn.Length - 1;
            if (IsHex(chIn, ref a))
            {
                return new UniColor(a);
            }
            else
            {
                throw new InvalidCastException("That string cannot be converted into a color");
            }
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Determine if something is hex, and optionally return its value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        /// <remarks>This may replace my main IsHex function in the DataTools library.</remarks>
        private static bool IsHex(string value, [Optional, DefaultParameterValue(0)] ref int result)
        {
            string chIn = value;
            switch (chIn[0])
            {
                case '#':
                    {
                        if (chIn.Length == 1)
                            return false;
                        chIn = chIn.Substring(1);
                        break;
                    }

                case '0':
                    {
                        if (chIn.Length == 1)
                            break;
                        if (chIn[1] != 'x' && TextTools.IsNumber((chIn[1])) == false)
                        {
                            return false;
                        }
                        else if (chIn[1] == 'x')
                        {
                            chIn = chIn.Substring(2);
                        }

                        break;
                    }

                case '&':
                    {
                        if (chIn.Length == 1)
                            return false;
                        if (chIn[1] != 'H')
                        {
                            return false;
                        }

                        chIn = chIn.Substring(2);
                        break;
                    }
            }

            if (chIn.Length > 1)
            {
                if (chIn[chIn.Length - 1] == 'h')
                {
                    chIn = chIn.Substring(0, chIn.Length - 1);
                }
            }

            int n = 0;
            bool b;
            b = int.TryParse(chIn, System.Globalization.NumberStyles.AllowHexSpecifier, System.Globalization.CultureInfo.CurrentCulture, out n);
            if (b)
                result = n;
            return b;
        }

        /// <summary>
        /// Try to find the System.Windows.Media.Color for the given color name.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="succeed"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static System.Windows.Media.Color TryFindNamedWPFColor(string name, ref bool succeed)
        {
            var c = new System.Windows.Media.Color();
            succeed = SharedProp.NameToSharedProp(name, ref c, typeof(System.Windows.Media.Colors), false);
            return c;
        }

        /// <summary>
        /// Try to find the System.Drawing.Color for the given color name.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="succeed"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Color TryFindNamedWinFormsColor(string name, ref bool succeed)
        {
            var c = new Color();
            succeed = SharedProp.NameToSharedProp(name, ref c, typeof(Color), false);
            return c;
        }

        /// <summary>
        /// Attempt to retrieve a color name for a specific color.
        /// </summary>
        /// <param name="Color"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string TryGetColorName(UniColor Color)
        {
            var cc = Color;

            // Make sure we have nothing errant and transparent.
            cc.A = 255;
            string s = SharedProp.SharedPropToName((System.Windows.Media.Color)cc, typeof(System.Windows.Media.Colors));
            if (s is null || string.IsNullOrEmpty(s))
            {
                s = SharedProp.SharedPropToName((Color)Color, typeof(Color));
            }

            return s;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public static explicit operator System.Windows.Media.Color(UniColor value)
        {
            return new System.Windows.Media.Color() { A = value.A, R = value.R, G = value.G, B = value.B };
        }

        public static implicit operator UniColor(System.Windows.Media.Color value)
        {
            var uc = new UniColor();
            uc.R = value.R;
            uc.G = value.G;
            uc.B = value.B;
            uc.A = value.A;
            return uc;
        }

        public static implicit operator byte[](UniColor value)
        {
            return BitConverter.GetBytes(value._intvalue);
        }

        public static explicit operator UniColor(byte[] value)
        {
            byte[] vin;
            vin = new byte[4];
            for (int i = 0; i <= 3; i++)
            {
                if (value.Length <= i)
                    break;
                vin[i] = value[i];
            }

            return new UniColor(BitConverter.ToInt32(vin, 0));
        }

        public static implicit operator Color(UniColor value)
        {
            return Color.FromArgb(value.IntValue);
        }

        public static implicit operator UniColor(Color value)
        {
            return new UniColor(value.ToArgb());
        }

        public static implicit operator int(UniColor value)
        {
            return value.IntValue;
        }

        public static implicit operator UniColor(int value)
        {
            return new UniColor(value);
        }

        public static implicit operator uint(UniColor value)
        {
            return value._Value;
        }

        public static implicit operator UniColor(uint value)
        {
            return new UniColor(value);
        }

        public static explicit operator UniColor(string value)
        {
            return Parse(value);
        }

        public static implicit operator string(UniColor value)
        {
            return value.ToString();
        }

        public static bool operator ==(UniColor val1, UniColor val2)
        {
            return val1._intvalue == val2.IntValue;
        }

        public static bool operator !=(UniColor val1, UniColor val2)
        {
            return val1._intvalue != val2.IntValue;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    }

    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    /// <summary>
    /// Unified point structure for WinForms, WPF and the Win32 API.
    /// </summary>
    /// <remarks></remarks>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct UniPoint
    {
        private double _x;
        private double _y;

        public double X
        {
            get
            {
                return _x;
            }

            set
            {
                _x = value;
            }
        }

        public double Y
        {
            get
            {
                return _y;
            }

            set
            {
                _y = value;
            }
        }

        public static explicit operator PointF(UniPoint operand)
        {
            return new PointF((float)operand.X, (float)operand.Y);
        }

        public static implicit operator UniPoint(PointF operand)
        {
            return new UniPoint(operand.X, operand.Y);
        }

        public static explicit operator Point(UniPoint operand)
        {
            return new Point((int)operand.X, (int)operand.Y);
        }

        public static implicit operator UniPoint(Point operand)
        {
            return new UniPoint(operand);
        }

        public static explicit operator System.Windows.Point(UniPoint operand)
        {
            return new System.Windows.Point(operand.X, operand.Y);
        }

        public static implicit operator UniPoint(System.Windows.Point operand)
        {
            return new UniPoint(operand);
        }

        public static explicit operator W32POINT(UniPoint operand)
        {
            return new W32POINT((int)operand.X, (int)operand.Y);
        }

        public static implicit operator UniPoint(W32POINT operand)
        {
            return new UniPoint(operand);
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", X, Y);
        }

        public UniPoint(Point p)
        {
            _x = p.X;
            _y = p.Y;
        }

        public UniPoint(System.Windows.Point p)
        {
            _x = p.X;
            _y = p.Y;
        }

        public UniPoint(W32POINT p)
        {
            _x = p.x;
            _y = p.y;
        }

        public UniPoint(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public UniPoint(double x, double y)
        {
            _x = x;
            _y = y;
        }
    }

    /// <summary>
    /// Unified size structure for WinForms, WPF and the Win32 API.
    /// </summary>
    /// <remarks></remarks>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct UniSize
    {
        private System.Windows.Size _size;

        public double cx
        {
            get
            {
                return _size.Width;
            }

            set
            {
                _size.Width = value;
            }
        }

        public double cy
        {
            get
            {
                return _size.Height;
            }

            set
            {
                _size.Height = value;
            }
        }

        public double Width
        {
            get
            {
                return _size.Width;
            }

            set
            {
                _size.Width = value;
            }
        }

        public double Height
        {
            get
            {
                return _size.Height;
            }

            set
            {
                _size.Height = value;
            }
        }

        public static explicit operator SizeF(UniSize operand)
        {
            return new SizeF((float)operand.cx, (float)operand.cy);
        }

        public static implicit operator UniSize(SizeF operand)
        {
            return new UniSize(operand.Width, operand.Height);
        }

        public static explicit operator Size(UniSize operand)
        {
            return new Size((int)operand.cx, (int)operand.cy);
        }

        public static explicit operator UniSize(Size operand)
        {
            return new UniSize(operand);
        }

        public static implicit operator System.Windows.Size(UniSize operand)
        {
            return new System.Windows.Size(operand.cx, operand.cy);
        }

        public static implicit operator UniSize(System.Windows.Size operand)
        {
            return new UniSize(operand);
        }

        public static explicit operator W32SIZE(UniSize operand)
        {
            return new W32SIZE((int)operand.cx, (int)operand.cy);
        }

        public static explicit operator UniSize(W32SIZE operand)
        {
            return new UniSize(operand);
        }

        public static explicit operator UniSize(string operand)
        {
            var st = TextTools.Split(operand, ",");

            if (st.Length != 2)
                throw new InvalidCastException("That string cannot be converted into a width/height pair.");

            var p = new UniSize();

            p.cx = double.Parse(st[0].Trim());
            p.cy = double.Parse(st[1].Trim());

            return p;
        }

        public UniSize(Size p)
        {
            _size = new System.Windows.Size();

            cx = p.Width;
            cy = p.Height;
        }

        public UniSize(System.Windows.Size p)
        {
            _size = new System.Windows.Size();

            cx = p.Width;
            cy = p.Height;
        }

        public UniSize(W32SIZE p)
        {
            _size = new System.Windows.Size();

            cx = p.cx;
            cy = p.cy;
        }

        public UniSize(int cx, int cy)
        {
            _size = new System.Windows.Size();

            this.cx = cx;
            this.cy = cy;
        }

        public UniSize(double cx, double cy)
        {
            _size = new System.Windows.Size();

            this.cx = cx;
            this.cy = cy;
        }

        public override string ToString()
        {
            string sx = _size.Width.ToString();
            string sy = _size.Height.ToString();
            return sx + "," + sy;
        }


        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// This will universally compare whether this is equals to any object that has valid width and height properties.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsEquals(object obj)
        {
            var pi = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
            var fi = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
            bool xmatch = false;
            bool ymatch = false;

            // compare fields, first.  These sorts of objects are structures, more often than not.
            foreach (var fe in fi)
            {
                switch (fe.Name.ToLower() ?? "")
                {
                    case "cx":
                    case "width":
                    case "x":
                    case "dx":
                    case "_cx":
                    case "_height":
                    case "_x":
                    case "_dx":

                        if (fe.FieldType.IsPrimitive)
                        {
                            if ((double)(fe.GetValue(obj)) == Width)
                            {
                                xmatch = true;
                            }
                        }

                        break;

                    case "cy":
                    case "height":
                    case "y":
                    case "dy":
                    case "_cy":
                    case var @case when @case == "_height":
                    case "_y":
                    case "_dy":

                        if (fe.FieldType.IsPrimitive)
                        {
                            if ((double)(fe.GetValue(obj)) == Height)
                            {
                                ymatch = true;
                            }
                        }

                        break;

                    default:
                        continue;

                }

                if (xmatch & ymatch)
                    return true;
            }

            // now, properties.
            foreach (var pe in pi)
            {
                switch (pe.Name.ToLower() ?? "")
                {
                    case "cx":
                    case "width":
                    case "x":
                    case "dx":
                    case "_cx":
                    case "_height":
                    case "_x":
                    case "_dx":

                        if (pe.PropertyType.IsPrimitive)
                        {
                            if ((double)(pe.GetValue(obj)) == Width)
                            {
                                xmatch = true;
                            }
                        }

                        break;

                    case "cy":
                    case "height":
                    case "y":
                    case "dy":
                    case "_cy":
                    case var case1 when case1 == "_height":
                    case "_y":
                    case "_dy":

                        if (pe.PropertyType.IsPrimitive)
                        {
                            if ((double)(pe.GetValue(obj)) == Height)
                            {
                                ymatch = true;
                            }
                        }

                        break;

                    default:
                        continue;
                }

                if (xmatch & ymatch)
                    break;
            }

            return xmatch & ymatch;
        }

        /// <summary>
        /// More experient functions for known "size" types.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool Equals(System.Windows.Size other)
        {
            return Width == other.Width && Height == other.Height;
        }

        /// <summary>
        /// More experient functions for known "size" types.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool Equals(Size other)
        {
            return Width == other.Width && Height == other.Height;
        }

        /// <summary>
        /// More experient functions for known "size" types.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool Equals(UniSize other)
        {
            return Width == other.Width && Height == other.Height;
        }

        /// <summary>
        /// More experient functions for known "size" types.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool Equals(W32SIZE other)
        {
            return Width == other.cx && Height == other.cy;
        }
    }

    /// <summary>
    /// Unified rectangle structure for WinForms, WPF and the Win32 API.
    /// </summary>
    /// <remarks></remarks>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct UniRect : INotifyPropertyChanged
    {
        private double _Left;

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public double Left
        {
            get
            {
                return _Left;
            }

            set
            {
                _Left = value;
                OnPropertyChanged("Left");
                OnPropertyChanged("X");
            }
        }

        private double _Top;

        public double Top
        {
            get
            {
                return _Top;
            }

            set
            {
                _Top = value;
                OnPropertyChanged("Top");
                OnPropertyChanged("Y");
            }
        }

        private double _Width;

        public double Width
        {
            get
            {
                return _Width;
            }

            set
            {
                _Width = value;
                OnPropertyChanged("Width");
                OnPropertyChanged("Right");
                OnPropertyChanged("CX");
            }
        }

        private double _Height;

        public double Height
        {
            get
            {
                return _Height;
            }

            set
            {
                _Height = value;
                OnPropertyChanged("Height");
                OnPropertyChanged("Bottom");
                OnPropertyChanged("CY");
            }
        }

        public double Right
        {
            get
            {
                return _Width - _Left - 1d;
            }

            set
            {
                _Width = value - _Left + 1d;
                OnPropertyChanged("Height");
                OnPropertyChanged("Bottom");
                OnPropertyChanged("CY");
            }
        }

        public double Bottom
        {
            get
            {
                return _Height - _Top - 1d;
            }

            set
            {
                _Height = value - _Top + 1d;
                OnPropertyChanged("Width");
                OnPropertyChanged("Right");
                OnPropertyChanged("CX");
            }
        }

        public double X
        {
            get
            {
                return _Left;
            }
        }

        public double Y
        {
            get
            {
                return _Top;
            }
        }

        public double CX
        {
            get
            {
                return _Width;
            }
        }

        public double CY
        {
            get
            {
                return _Height;
            }
        }

        public UniRect(double x, double y, double width, double height)
        {
            _Left = x;
            _Top = y;
            _Width = width;
            _Height = height;

            PropertyChanged = null;
        }

        public UniRect(System.Windows.Point location, System.Windows.Size size)
        {
            _Left = location.X;
            _Top = location.Y;
            _Width = size.Width;
            _Height = size.Height;
            PropertyChanged = null;
        }

        public UniRect(Point location, Size size)
        {
            _Left = location.X;
            _Top = location.Y;
            _Width = size.Width;
            _Height = size.Height;
            PropertyChanged = null;
        }

        public UniRect(PointF location, SizeF size)
        {
            _Left = location.X;
            _Top = location.Y;
            _Width = size.Width;
            _Height = size.Height;
            PropertyChanged = null;
        }

        public UniRect(W32RECT rectStruct)
        {
            _Left = rectStruct.left;
            _Top = rectStruct.top;

            _Width = (rectStruct.right - rectStruct.left) + 1;
            _Height = (rectStruct.bottom - rectStruct.top) + 1;

            PropertyChanged = null;
        }

        public UniRect(Rectangle rectStruct)
        {
            _Left = rectStruct.Left;
            _Top = rectStruct.Top;
            _Width = rectStruct.Width;
            _Height = rectStruct.Height;
            PropertyChanged = null;
        }

        public UniRect(RectangleF rectStruct)
        {
            _Left = rectStruct.Left;
            _Top = rectStruct.Top;
            _Width = rectStruct.Width;
            _Height = rectStruct.Height;
            PropertyChanged = null;
        }

        public UniRect(System.Windows.Rect rectStruct)
        {
            _Left = rectStruct.Left;
            _Top = rectStruct.Top;
            _Width = rectStruct.Width;
            _Height = rectStruct.Height;
            PropertyChanged = null;
        }

        public UniRect(System.Windows.Int32Rect rectStruct)
        {
            _Left = rectStruct.X;
            _Top = rectStruct.Y;
            _Width = rectStruct.Width;
            _Height = rectStruct.Height;
            PropertyChanged = null;
        }

        event System.ComponentModel.PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}; {2}x{3}", _Left, _Top, _Width, _Height);
        }

        public static implicit operator System.Windows.Rect(UniRect operand)
        {
            return new System.Windows.Rect(operand._Left, operand._Top, operand._Width, operand._Height);
        }

        public static implicit operator UniRect(System.Windows.Rect operand)
        {
            return new UniRect(operand);
        }

        public static explicit operator RectangleF(UniRect operand)
        {
            return new RectangleF((float)operand._Left, (float)operand._Top, (float)operand._Width, (float)operand._Height);
        }

        public static implicit operator UniRect(RectangleF operand)
        {
            return new UniRect(operand);
        }

        public static explicit operator Rectangle(UniRect operand)
        {
            return new Rectangle((int)operand._Left, (int)operand._Top, (int)operand._Width, (int)operand._Height);
        }

        public static implicit operator UniRect(Rectangle operand)
        {
            return new UniRect(operand);
        }

        public static explicit operator W32RECT(UniRect operand)
        {
            return new W32RECT() { left = (int)operand._Left, top = (int)operand._Top, right = (int)operand.Right, bottom = (int)operand.Bottom };
        }

        public static implicit operator UniRect(W32RECT operand)
        {
            return new UniRect(operand);
        }

        public static explicit operator System.Windows.Int32Rect(UniRect operand)
        {
            return new System.Windows.Int32Rect((int)operand.Left, (int)operand.Top, (int)operand.Width, (int)operand.Height);
        }

        public static implicit operator UniRect(System.Windows.Int32Rect operand)
        {
            return new UniRect(operand);
        }

        public static implicit operator double[](UniRect operand)
        {
            return new[] { operand.Left, operand.Top, operand.Right, operand.Bottom };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public delegate void PropertyChangedEventHandler(object sender, PropertyChangedEventArgs e);
    }

    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
}