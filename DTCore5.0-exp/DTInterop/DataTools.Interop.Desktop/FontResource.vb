
Imports System.IO
Imports System.Runtime.InteropServices


<Flags>
Public Enum FONTRESOURCE_FLAGS As UInteger

    '               DFF_FIXED            equ  0001h ; font Is fixed pitch
    DFF_FIXED = 1
    '               DFF_PROPORTIONAL     equ  0002h ; font Is proportional
    DFF_PROPORTIONAL = 2
    '                                               ; pitch
    '               DFF_ABCFIXED         equ  0004h ; font Is an ABC fixed
    DFF_ABCFIXED = 4
    '                                               ; font
    '               DFF_ABCPROPORTIONAL  equ  0008h ; font Is an ABC pro-
    DFF_ABCFIXEDPROPORTIONAL = 8
    '                                               ; portional font
    '               DFF_1COLOR           equ  0010h ; font Is one color
    DFF_1COLOR = &H10
    '               DFF_16COLOR          equ  0020h ; font Is 16 color
    DFF_16COLOR = &H20
    '               DFF_256COLOR         equ  0040h ; font Is 256 color
    DFF_255COLOR = &H40
    '               DFF_RGBCOLOR         equ  0080h ; font Is RGB color
    DFF_RGBCOLOR = &H80


End Enum

'Field          Description
'-----          -----------

<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Ansi)>
Public Structure FONTRESOURCE


    Public dfVersion As UShort

    'dfVersion      2 bytes specifying the version (0200H Or 0300H) of
    '               the file.

    Public dfSize As UInteger

    'dfSize         4 bytes specifying the total size of the file in
    '               bytes.

    <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=60)>
    Public dfCopyright As String

    'dfCopyright    60 bytes specifying copyright information.

    Public dfType As UShort
    'dfType         2 bytes specifying the type of font file.

    '               The low-order Byte Is exclusively For GDI use. If the
    '               low-order bit Of the WORD Is zero, it Is a bitmap
    '               (raster) font file. If the low-order bit Is 1, it Is a
    '               vector font file. The second bit Is reserved And must
    '               be zero. If no bits follow In the file And the bits are
    '               located in memory at a fixed address specified in
    '               dfBitsOffset, the third bit Is Set To 1; otherwise, the
    '               bit Is set to 0 (zero). The high-order bit of the low
    '               Byte Is set if the font was realized by a device. The
    '               remaining bits In the low Byte are reserved And Set To
    '               zero.

    '               The high Byte Is reserved For device use And will
    '               always be Set To zero For GDI-realized standard fonts.
    '               Physical fonts With the high-order bit Of the low Byte
    '               Set may use this Byte To describe themselves. GDI will
    '               never inspect the high Byte.

    Public dfPoints As UShort
    'dfPoints       2 bytes specifying the nominal point size at which
    '               this character Set looks best.

    Public dfVertRes As UShort
    'dfVertRes      2 bytes specifying the nominal vertical resolution
    '               (dots-per-inch) at which this character set was
    '               digitized.

    Public dfHorizRes As UShort
    'dfHorizRes     2 bytes specifying the nominal horizontal resolution
    '               (dots-per-inch) at which this character set was
    '               digitized.

    Public dfAscent As UShort
    'dfAscent       2 bytes specifying the distance from the top of a
    '               character definition cell To the baseline Of the
    '               typographical font. It Is useful For aligning the
    '               baselines of fonts of different heights.

    Public dfInternalLeading As UShort
    'dfInternalLeading
    '               Specifies the amount Of leading inside the bounds Set
    '               by dfPixHeight. Accent marks may occur In this area.
    '               This may be zero at the designer's option.

    Public dfExternalLeading As UShort
    'dfExternalLeading
    '               Specifies the amount Of extra leading that the designer
    '               requests the application add between rows. Since this
    '               area Is outside Of the font proper, it contains no
    '               marks And will Not be altered by text output calls in
    '               either the OPAQUE Or TRANSPARENT mode. This may be zero
    '               at the designer's option.

    Public dfItalic As Byte
    'dfItalic       1(one) byte specifying whether Or Not the character
    '               definition data represent an italic font. The low-order
    '               bit Is 1 if the flag Is set. All the other bits are
    '               zero.

    Public dfUnderline As Byte
    'dfUnderline    1 byte specifying whether Or Not the character
    '               definition data represent an underlined font. The
    '               low-order bit Is 1 if the flag Is set. All the other
    '               bits are 0 (zero).

    Public dfStrikeout As Byte
    'dfStrikeOut    1 byte specifying whether Or Not the character
    '               definition data represent a struckout font. The low-
    '               order bit Is 1 If the flag Is Set. All the other bits
    '               are zero.

    Public dfWeight As UShort
    'dfWeight       2 bytes specifying the weight of the characters in the
    '               character definition data, On a scale Of 1 To 1000. A
    '               dfWeight of 400 specifies a regular weight.

    Public dfCharSet As Byte
    'dfCharSet      1 byte specifying the character set defined by this
    '               font.

    Public dfPixWidth As UShort
    'dfPixWidth     2 bytes. For vector fonts, specifies the width of the
    '               grid on which the font was digitized. For raster fonts,
    '               If dfPixWidth Is nonzero, it represents the width for
    '               all the characters In the bitmap; If it Is zero, the
    '               font has variable width characters whose widths are
    '               specified in the dfCharTable array.

    Public dfPixHEight As UShort
    'dfPixHeight    2 bytes specifying the height of the character bitmap
    '               (raster fonts), Or the height of the grid on which a
    '               vector font was digitized.

    Public dfPitchAndFamily As Byte
    'dfPitchAndFamily
    '               Specifies the pitch And font family. The low bit Is Set
    '               If the font Is variable pitch. The high four bits give
    '               the family name Of the font. Font families describe In
    '               a general way the look Of a font. They are intended For
    '               specifying fonts When the exact face name desired Is
    '               Not available. The families are as follows:

    '                  Family               Description
    '                  ------               -----------
    '                  FF_DONTCARE(0 << 4)   Don't care or don't know.
    '                  FF_ROMAN(1 << 4)      Proportionally spaced fonts
    '                                       With serifs.
    '                  FF_SWISS(2 << 4)      Proportionally spaced fonts
    '                                       without serifs.
    '                  FF_MODERN(3 << 4)     Fixed-pitch fonts.
    '                  FF_SCRIPT(4 << 4)
    'FF_DECORATIVE(5 << 4)


    Public dfAvgWidth As UShort
    'dfAvgWidth     2 bytes specifying the width of characters in the font.
    '               For fixed - pitch fonts, this Is the same as dfPixWidth.
    '               For variable - pitch fonts, this Is the width of the
    '               character "X."

    Public dfMaxWidth As UShort
    'dfMaxWidth     2 bytes specifying the maximum pixel width of any
    '               character in the font. For fixed-pitch fonts, this Is
    '               simply dfPixWidth.

    Public dfFirstChar As Byte
    'dfFirstChar    1 byte specifying the first character code defined by
    '               this font. Character definitions are stored only For
    '               the characters actually present In a font. Therefore,
    '               use this field When calculating indexes into either
    '               dfBits Or dfCharOffset.

    Public dfLastChar As Byte
    'dfLastChar     1 byte specifying the last character code defined by
    '               this font. Note that all the characters With codes
    '               between dfFirstChar And dfLastChar must be present In
    '               the font character definitions.

    Public dfDefaultChar As Byte
    'dfDefaultChar  1 byte specifying the character to substitute
    '               whenever a String contains a character out Of the
    '               range.The character Is given relative to dfFirstChar
    '               so that dfDefaultChar Is the actual value Of the
    '               character, less dfFirstChar. The dfDefaultChar should
    '               indicate a special character that Is Not a space.

    Public dfBreakChar As Byte
    'dfBreakChar    1 byte specifying the character that will define word
    '               breaks.This character defines word breaks for word
    '               wrapping And word spacing justification. The character
    '               Is given relative to dfFirstChar so that dfBreakChar Is
    '               the actual value Of the character, less that Of
    '               dfFirstChar.The dfBreakChar Is normally(32 -
    '               dfFirstChar), which Is an ASCII space.

    Public dfWidthBytes As UShort
    'dfWidthBytes   2 bytes specifying the number of bytes in each row of
    '               the bitmap. This Is always even, so that the rows start
    '               On WORD boundaries. For vector fonts, this field has no
    '               meaning.

    Public dfDevice As UInteger
    'dfDevice       4 bytes specifying the offset in the file to the string
    '               giving the device name. For a generic font, this value
    '               Is zero.

    Public dfFace As UInteger
    'dfFace         4 bytes specifying the offset in the file to the
    '               null-terminated string that names the face.

    Public dfBitsPoint As UInteger
    'dfBitsPointer  4 bytes specifying the absolute machine address of
    '               the bitmap. This Is Set by GDI at load time. The
    '               dfBitsPointer Is guaranteed to be even.

    Public dfBitsOffset As UInteger
    'dfBitsOffset   4 bytes specifying the offset in the file to the
    '               beginning of the bitmap information. If the 04H bit in
    '               the dfType Is Set, Then dfBitsOffset Is an absolute
    '               address of the bitmap (probably in ROM).

    '               For raster fonts, dfBitsOffset points To a sequence Of
    '               bytes that make up the bitmap Of the font, whose height
    '               Is the height of the font, And whose width Is the sum
    '               of the widths of the characters in the font rounded up
    '               to the next WORD boundary.

    '               For vector fonts, it points To a String Of bytes Or
    '               words(depending on the size of the grid on which the
    '               font was digitized) that specify the strokes For Each
    '               character of the font. The dfBitsOffset field must be
    '               even.

    Public dfReserved As Byte
    'dfReserved     1 Byte, Not used.

    Public dfFlags As FONTRESOURCE_FLAGS
    'dfFlags        4 bytes specifying the bits flags, which are additional
    '               flags that define the format Of the Glyph bitmap, As
    '               follows:

    '               DFF_FIXED            equ  0001h ; font Is fixed pitch
    '               DFF_PROPORTIONAL     equ  0002h ; font Is proportional
    '                                               ; pitch
    '               DFF_ABCFIXED         equ  0004h ; font Is an ABC fixed
    '                                               ; font
    '               DFF_ABCPROPORTIONAL  equ  0008h ; font Is an ABC pro-
    '                                               ; portional font
    '               DFF_1COLOR           equ  0010h ; font Is one color
    '               DFF_16COLOR          equ  0020h ; font Is 16 color
    '               DFF_256COLOR         equ  0040h ; font Is 256 color
    '               DFF_RGBCOLOR         equ  0080h ; font Is RGB color

    Public dfAspace As UShort
    'dfAspace       2 bytes specifying the Global A space, If any. The
    '               dfAspace Is the distance from the current position to
    '               the left edge Of the bitmap.

    Public dfBspace As UShort
    'dfBspace       2 bytes specifying the Global B space, If any. The
    '               dfBspace Is the width of the character.

    Public dfCspace As UShort
    'dfCspace       2 bytes specifying the Global C space, If any. The
    '               dfCspace Is the distance from the right edge of the
    '               bitmap to the New current position. The increment of a
    '               character Is the sum of the three spaces. These apply
    '               to all glyphs And Is the case for DFF_ABCFIXED.

    Public dfColorPointer As UInteger
    'dfColorPointer
    '4 bytes specifying the offset to the color table for
    '               color fonts, if any. The format Of the bits Is similar
    '               to a DIB, but without the header. That Is, the
    '               characters are Not split up into disjoint bytes.
    '               Instead, they are left intact. If no color table Is
    '               needed, this entry Is NULL.
    '               [NOTE: This information Is different from that In the
    '               hard-copy Developer's Notes and reflects a correction.]

    <MarshalAs(UnmanagedType.ByValArray, SizeConst:=16)>
    Public dfReserved1 As Byte()

    'dfReserved1    16 bytes, Not used.
    '               [NOTE: This information Is different from that In the
    '               hard-copy Developer's Notes and reflects a correction.]



    '' The rest of the structure shall be fought with pointers and calculations!!!
    '' Which probably means wrapping it up in a nice package.



    'dfCharTable    For raster fonts, the CharTable Is an array of entries
    '               each consisting of two 2-byte WORDs for Windows 2.x And
    '               three 2 - Byte WORDs for Windows 3.0. The first WORD of
    '               each entry Is the character width. The second WORD of
    '               each entry Is the byte offset from the beginning of the
    '               FONTINFO Structure to the character bitmap. For Windows
    '               3.0, the second And third WORDs are used for the
    '               offset.

    '               There Is one extra entry at the end of this table that
    '               describes an absolute-space character. This entry
    '               corresponds to a character that Is guaranteed to be
    '               blank; this character Is Not part of the normal
    '               character set.

    '               The number Of entries In the table Is calculated As
    '               ((dfLastChar - dfFirstChar) + 2). This includes a
    '               spare, the sentinel offset mentioned in the following
    '               paragraph.

    '               For fixed - pitch vector fonts, each 2-byte entry in this
    '               array specifies the offset from the start Of the bitmap
    '               to the beginning of the string of stroke specification
    '               units for the character. The number of bytes Or WORDs
    '               to be used for a particular character Is calculated by
    '               subtracting its entry from the Next one, so that there
    '               Is a sentinel at the end of the array of values.

    '               For proportionally spaced vector fonts, each 4-byte
    '               entry Is divided into two 2-byte fields. The first
    '               field gives the starting offset from the start Of the
    '               bitmap of the character strokes. The second field gives
    '               the pixel width Of the character.

    '<facename> An ASCII character String specifying the name Of the
    '               font face. The size Of this field Is the length Of the
    '               String plus a NULL terminator.

    '<devicename> An ASCII character String specifying the name Of the
    '               device if this font file Is for a specific device. The
    '               size of this field Is the length of the string plus a
    '               NULL terminator.

    '<bitmaps> This field contains the character bitmap definitions.
    '               Each character Is stored as a contiguous set of bytes.
    '               (In the old font format, this was Not the case.)

    '               The first Byte contains the first 8 bits Of the first
    '               scanline (that Is, the top line of the character). The
    '               second byte contains the first 8 bits of the second
    '               scanline. This continues until a first "column" Is
    '               completely defined.

    '               The following Byte contains the Next 8 bits Of the
    '               first scanline, padded With zeros On the right If
    '               necessary (And so on, down through the second
    '               "column"). If the glyph Is quite narrow, each scanline
    '               Is covered by 1 byte, with bits set to zero as
    '               necessary for padding. If the glyph Is very wide, a
    '               third Or even fourth set of bytes can be present.

    '                  NOTE: The character bitmaps must be stored
    '                  contiguously And arranged in ascending order.

    '               The following Is a Single-character example, In which
    '               are given the bytes For a 12 x 14 pixel character, As
    '               shown here schematically.

    '                  ............
    '                  .....**.....
    '                  ....*..*....
    '                  ...*....*...
    '                  ..*......*..
    '                  ..*......*..
    '                  ..*......*..
    '                  ..********..
    '                  ..*......*..
    '                  ..*......*..
    '                  ..*......*..
    '                  ............
    '                  ............
    '                  ............

    '               The bytes are given here In two sets, because the
    '               character Is less than 17 pixels wide.

    '                  00 06 09 10 20 20 20 3F 20 20 20 00 00 00
    '                  00 00 00 80 40 40 40 C0 40 40 40 00 00 00

    '               Note that In the second Set Of bytes, the second digit
    '               of each Is always zero. It would correspond to the 13th
    '               through 16th pixels on the right side of the character,
    '               If they were present.

    'The Windows 2.x version Of dfCharTable has a GlyphEntry Structure With the following format:
    'GlyphEntry    struc
    'geWidth       dw?    ; width Of character bitmap In pixels
    'geOffset      dw?    ; pointer To the bits
    'GlyphEntry    ends

    'The Windows 3.0 version Of the dfCharTable Is dependent On the format Of the Glyph bitmap. 

    'NOTE: The only formats supported In Windows 3.0 will be DFF_FIXED And DFF_PROPORTIONAL. 

    'DFF_FIXED
    'DFF_PROPORTIONAL
    'GlyphEntry    struc
    'geWidth       dw?    ; width Of character bitmap In pixels
    'geOffset      dd?    ; pointer To the bits
    'GlyphEntry    ends

    'DFF_ABCFIXED
    'DFF_ABCPROPORTIONAL
    'GlyphEntry    struc
    'geWidth       dw?   ; width Of character bitmap In pixels
    'geOffset      dd?   ; pointer To the bits
    'geAspace      dd?   ; A space In fractional pixels (16.16)
    'geBspace      dd?   ; B space In fractional pixels (16.16)
    'geCspace      dw?   ; C space In fractional pixels (16.16)
    'GlyphEntry    ends

    'The fractional pixels are expressed As a 32-bit signed number With an implicit binary point between bits 15 And 16. This Is referred To As a 16.16 ("sixteen dot sixteen") fixed-point number. 

    'The ABC spacing here Is the same As that defined above. However, here there are specific sets For Each character. 

    'DFF_1COLOR
    'DFF_16COLOR
    'DFF_256COLOR
    'DFF_RGBCOLOR
    'GlyphEntry    struc
    'geWidth       dw?   ; width Of character bitmap In pixels
    'geOffset      dd?   ; pointer To the bits
    'geHeight      dw?   ; height Of character bitmap In pixels
    'geAspace      dd?   ; A space In fractional pixels (16.16)
    'geBspace      dd?   ; B space In fractional pixels (16.16)
    'geCspace      dd?   ; C space In fractional pixels (16.16)
    'GlyphEntry    ends

    'DFF_1COLOR means 8 pixels per Byte
    'DFF_16COLOR means 2 pixels per Byte
    'DFF_256COLOR means 1 pixel per Byte
    'DFF_RGBCOLOR means RGBquads 

End Structure

Public Class FonExtract

End Class
