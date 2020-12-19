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
    /// Represents information about a font on the current system.
    /// </summary>
    public sealed class FontInfo
    {
        internal ENUMLOGFONTEX elf;
        internal LOGFONT lf;

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
                FontPitchAndFamily v = (FontPitchAndFamily)(lf.lfPitchAndFamily >> 2 << 2);
                switch (v)
                {
                    case FontPitchAndFamily.FF_DECORATIVE:
                        {
                            return FontFamilies.Decorative;
                        }

                    case FontPitchAndFamily.FF_DONTCARE:
                        {
                            return FontFamilies.DontCare;
                        }

                    case FontPitchAndFamily.FF_MODERN:
                        {
                            return FontFamilies.Modern;
                        }

                    case FontPitchAndFamily.FF_ROMAN:
                        {
                            return FontFamilies.Roman;
                        }

                    case FontPitchAndFamily.FF_SWISS:
                        {
                            return FontFamilies.Swiss;
                        }

                    case FontPitchAndFamily.FF_SCRIPT:
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

        internal FontInfo(ENUMLOGFONTEX elf)
        {
            this.elf = elf;
            lf = elf.elfLogFont;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
