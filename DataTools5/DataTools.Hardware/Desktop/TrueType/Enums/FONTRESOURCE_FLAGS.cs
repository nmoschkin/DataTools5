using System;
using System.Runtime.InteropServices;

namespace DataTools.Desktop
{
    [Flags]
    public enum FONTRESOURCE_FLAGS : uint
    {

        // DFF_FIXED            equ  0001h ; font Is fixed pitch
        DFF_FIXED = 1U,
        // DFF_PROPORTIONAL     equ  0002h ; font Is proportional
        DFF_PROPORTIONAL = 2U,
        // ; pitch
        // DFF_ABCFIXED         equ  0004h ; font Is an ABC fixed
        DFF_ABCFIXED = 4U,
        // ; font
        // DFF_ABCPROPORTIONAL  equ  0008h ; font Is an ABC pro-
        DFF_ABCFIXEDPROPORTIONAL = 8U,
        // ; portional font
        // DFF_1COLOR           equ  0010h ; font Is one color
        DFF_1COLOR = 0x10U,
        // DFF_16COLOR          equ  0020h ; font Is 16 color
        DFF_16COLOR = 0x20U,
        // DFF_256COLOR         equ  0040h ; font Is 256 color
        DFF_255COLOR = 0x40U,
        // DFF_RGBCOLOR         equ  0080h ; font Is RGB color
        DFF_RGBCOLOR = 0x80U
    }
}
