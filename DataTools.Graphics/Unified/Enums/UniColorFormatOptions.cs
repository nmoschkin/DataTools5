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
    public static class UniColorFormatter
    {
        static Dictionary<char, int> FmtRef = new Dictionary<char, int>();

        static UniColorFormatter()
        {
            FmtRef.Add('g', 0);
            FmtRef.Add('D', 1);
            FmtRef.Add('h', 2 | 0x8000);
            FmtRef.Add('H', 2);
            FmtRef.Add('C', 4);
            FmtRef.Add('V', 8);
            FmtRef.Add('A', 16);
            FmtRef.Add('S', 32);
            FmtRef.Add('d', 64);
            FmtRef.Add('r', 128);
            FmtRef.Add('a', 256);
            FmtRef.Add('w', 512);
            FmtRef.Add('N', 0x2000);
            FmtRef.Add('R', 0x4000);
            FmtRef.Add('M', 0x10000);
        }

        public static UniColorFormatOptions ProvideFormatOptions(string format)
        {
            var chars = format?.ToCharArray() ?? throw new ArgumentNullException(nameof(format));
            int val = 0;

            foreach (var ch in chars)
            {
                if (FmtRef.TryGetValue(ch, out int found))
                {
                    val |= found;
                }
            }

            return (UniColorFormatOptions)val;
        }
    }


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
        Default = 0, // g, <null>

        /// <summary>
        /// Prints the decimal number for the value.
        /// </summary>
        /// <remarks></remarks>
        DecimalDigit = 1, // D

        /// <summary>
        /// Returns HTML-style hex code color syntax.
        /// </summary>
        /// <remarks></remarks>
        Hex = 2, // h/H

        /// <summary>
        /// Returns C-style hex code.
        /// </summary>
        /// <remarks></remarks>
        CStyleHex = 2 | 4,  // C

        /// <summary>
        /// Returns VB-style hex code.
        /// </summary>
        /// <remarks></remarks>
        VBStyleHex = 2 | 8,  // V

        /// <summary>
        /// Returns assembly-style hex code.
        /// </summary>
        /// <remarks></remarks>
        AsmStyleHex = 2 | 16,  // AH

        /// <summary>
        /// Returns a web style hex code.
        /// </summary>
        /// <remarks></remarks>
        WebStyleHex = 2 | 512, // WH

        /// <summary>
        /// Returns space-separated values.
        /// </summary>
        /// <remarks></remarks>
        Spaced = 32, // S

        /// <summary>
        /// Returns comma-separated values.
        /// </summary>
        /// <remarks></remarks>
        CommaDelimited = 64, // d

        /// <summary>
        /// Returns the RGB decimal values.
        /// </summary>
        /// <remarks></remarks>
        Rgb = 128, // r

        /// <summary>
        /// Returns the ARGB decimal values.
        /// </summary>
        /// <remarks></remarks>
        Argb = 256, // a

        /// <summary>
        /// Prints in web-ready format.  Cannot be used alone.
        /// </summary>
        /// <remarks></remarks>
        WebFormat = 512, // w

        /// <summary>
        /// Adds rgb() enclosure for the web.
        /// </summary>
        /// <remarks></remarks>
        RgbWebFormat = 512 | 128 | 64, // rw

        /// <summary>
        /// Adds argb() enclosure for the web.
        /// </summary>
        /// <remarks></remarks>
        ArgbWebFormat = 512 | 256 | 64, // aw

        /// <summary>
        /// Prints the #RRGGBB web format color code.
        /// </summary>
        /// <remarks></remarks>
        HexRgbWebFormat = 512 | 2 | 128, // rwh

        /// <summary>
        /// Prints the #AARRGGBB web format color code.
        /// </summary>
        /// <remarks></remarks>
        HexArgbWebFormat = 512 | 2 | 256, // awh

        /// <summary>
        /// Add details to a named color such as opacity level and hex code in brackets (if Hex is specifed).
        /// </summary>
        /// <remarks></remarks>
        DetailNamedColors = 0x2000, // N

        /// <summary>
        /// Reverses the order of the numbers.
        /// </summary>
        /// <remarks></remarks>
        Reverse = 0x4000, // R

        /// <summary>
        /// Print hex letters in lower case.
        /// </summary>
        /// <remarks></remarks>
        LowerCase = 0x8000, // h as opposed to H

        /// <summary>
        /// Returns the closest named color
        /// </summary>
        ClosestNamedColor = 0x10000  // M
    }
}
