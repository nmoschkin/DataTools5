using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using DataTools.Text;

namespace DataTools.Graphics
{
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
    public struct UniColor : IComparable<UniColor>
    {
        public static readonly UniColor Empty = new UniColor(0, 0, 0, 0);
        public static readonly UniColor Transparent = new UniColor(0, 0, 0, 0);

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

        /// <summary>
        /// Get the ARGB 32-bit color value.
        /// </summary>
        /// <returns></returns>
        public int ToArgb() => _intvalue;

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
            _A = _R = _G = _B = 0;
            _intvalue = color;

        }

        public UniColor(string color)
        {
            bool succeed = false;

            UniColor c;
            succeed = TryFindNamedColor(color, out c);

            if (!succeed)
            {
                succeed = TryFindNamedWebColor(color, out c);

                if (!succeed)
                {
                    _Value = 0;
                    _intvalue = 0;
                    _A = _R = _G = _B = 0;

                    return;
                }
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
            var nc = NamedColor.SearchAll(Color);

            if (nc == null || nc.Count == 0)
            {
                _A = _R = _G = _B = 0;
                _Value = 0;
                _intvalue = 0;

                return;
            }
            else
            {
                _A = _R = _G = _B = 0;
                _Value = 0;
                _intvalue = ((UniColor)nc[0]).ToArgb();
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

        public HSVDATA ToHSV() => ColorMath.ColorToHSV(this);

        public CMYDATA ToCMY()
        {
            var cmy = new CMYDATA();
            ColorMath.ColorToCMY(this, ref cmy);
            return cmy;
        }

        
        
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
                
                if ((format & UniColorFormatOptions.ClosestNamedColor) == UniColorFormatOptions.ClosestNamedColor)
                {
                    str1 = TryGetColorName(this, true);
                }
                else
                {
                    str1 = TryGetColorName(this);
                }


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
                    for (i = 0; i <= c; i++)
                        str1 += "h";
                }
                else if ((format & UniColorFormatOptions.CStyleHex) == UniColorFormatOptions.CStyleHex)
                {
                    for (i = 0; i <= c; i++)
                        str1 = "0x" + str1;
                }
                else if ((format & UniColorFormatOptions.VBStyleHex) == UniColorFormatOptions.VBStyleHex)
                {
                    for (i = 0; i <= c; i++)
                        str1 = "&H" + str1;
                }
                else if ((format & UniColorFormatOptions.WebStyleHex) == UniColorFormatOptions.WebStyleHex)
                {
                    for (i = 0; i <= c; i++)
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

            for (i = 0; i <= c; i++)
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

                c = s.Count();
                l = new int[c];

                bool b = true;
                byte by;

                float f;

                for (i = 0; i < c; i++)
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

            bool b1;
            bool b2;

            UniColor c1; 
            b1 = TryFindNamedColor(value, out c1);
            
            UniColor c2; 
            b2 = TryFindNamedWebColor(value, out c2);

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
        /// Try to find the named web color for the given color name.
        /// </summary>
        /// <param name="name">The name to search for</param>
        /// <param name="color">The color, if found</param>
        /// <returns></returns>
        public static bool TryFindNamedWebColor(string name, out UniColor color)
        {
            var c = new Color();
            var succeed = SharedProp.NameToSharedProp(name, ref c, typeof(Color), false);

            color = c;
            return succeed;
        }

        /// <summary>
        /// Try to find the named color for the given color name.
        /// </summary>
        /// <param name="name">The name to search for</param>
        /// <param name="color">The color, if found</param>
        /// <returns></returns>
        public static bool TryFindNamedColor(string name, out UniColor color)
        {
            var l = NamedColor.SearchAll(name);

            if (l != null && l.Count > 0)
            {
                color = l[0];
                return true;
            }
            else
            {
                color = Color.Transparent;
                return false;
            }
        }


        /// <summary>
        /// Attempt to retrieve a color name for a specific color.
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string TryGetColorName(UniColor color, bool closest = false)
        {
            var cc = color;

            // Make sure we have nothing errant and transparent.
            cc.A = 255;

            string s = SharedProp.SharedPropToName((Color)color, typeof(Color));

            if (s == null && closest)
            {
                var nc = NamedColor.GetClosestColor(color);

                if (nc != null)
                {
                    s = nc.Name;
                    color = nc.Color;
                }
            }

            return s;
        }

        public int CompareTo(UniColor other)
        {
            HSVDATA hsv1 = ColorMath.ColorToHSV(this);
            HSVDATA hsv2 = ColorMath.ColorToHSV(other);

            if (double.IsNaN(hsv1.Saturation)) hsv1.Saturation = 0;
            if (double.IsNaN(hsv1.Value)) hsv1.Value = 0;

            if (double.IsNaN(hsv2.Saturation)) hsv2.Saturation = 0;
            if (double.IsNaN(hsv2.Value)) hsv2.Value = 0;

            if (hsv1.Hue == hsv2.Hue)
            {
                if (hsv1.Saturation == hsv2.Saturation)
                {
                    return hsv1.Value > hsv2.Value ? 1 : hsv1.Value < hsv2.Value ? -1 : 0;
                }
                else
                {
                    return hsv1.Saturation > hsv2.Saturation ? 1 : hsv1.Saturation < hsv2.Saturation ? -1 : 0;
                }
            }
            else
            {
                return hsv1.Hue > hsv2.Hue ? 1 : hsv1.Hue < hsv2.Hue ? -1 : 0;
            }

        }

        /// <summary>
        /// Copy the 32 bit value to a memory buffer.
        /// </summary>
        /// <param name="ptr"></param>
        public void CopyTo(IntPtr ptr)
        {
            unsafe
            {
                CopyTo((void*)ptr);
            }
        }

        /// <summary>
        /// Copy the 32 bit value to a memory buffer.
        /// </summary>
        /// <param name="ptr"></param>
        public unsafe void CopyTo(void *ptr)
        {
            *((int*)ptr) = _intvalue;
        }

        /// <summary>
        /// Initialize a new <see cref="UniColor"/> from a memory buffer.
        /// </summary>
        /// <param name="ptr"></param>
        public static UniColor FromPointer(IntPtr ptr)
        {
            unsafe
            {
                return FromPointer((void*)ptr);
            }
        }

        /// <summary>
        /// Initialize a new <see cref="UniColor"/> from a memory buffer.
        /// </summary>
        /// <param name="ptr"></param>
        public static unsafe UniColor FromPointer(void *ptr)
        {
            return new UniColor(*((int*)ptr));
        }

        public static explicit operator byte[](UniColor value)
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
            return val1._intvalue == val2._intvalue;
        }

        public static bool operator !=(UniColor val1, UniColor val2)
        {
            return val1._intvalue != val2._intvalue;
        }

        
    }
}
