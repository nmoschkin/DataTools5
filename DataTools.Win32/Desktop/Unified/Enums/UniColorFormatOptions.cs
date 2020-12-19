using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using DataTools.Text;
using DataTools.Win32;

namespace DataTools.Desktop.Unified
{
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
        LowerCase = 0x8000,

        /// <summary>
        /// Returns the closest named color
        /// </summary>
        ClosestNamedColor = 0x10000
    }
}
