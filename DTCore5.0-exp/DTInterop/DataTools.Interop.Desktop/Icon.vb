'' ************************************************* ''
'' DataTools Visual Basic Utility Library - Interop
''
'' Module: Icon File.
''         Icon image file format structure classes
''         Capable of using Windows Vista and greater .PNG icon images
''         Can create a complete icon file from scratch using images you add
''
'' Icons are an old old format.  They have been adapted for modern use,
'' and the reason that they endure is because of the ability to succintly
'' store multiple image sizes in multiple formats, in a single file.
''
'' But, because the 32-bit bitmap standard came around slightly afterward,
'' certain acrobatic programming translations had to be made to get one from
'' the other, and back again.
''
'' Remember, back in the day, icon painting and design software was its own thing.
''
'' Copyright (C) 2011-2017 Nathan Moschkin
'' All Rights Reserved
''
'' Licensed Under the Microsoft Public License   
'' ************************************************* ''

Option Explicit On

Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Drawing

Imports CoreCT.Memory
Imports DataTools.Interop.Native


Namespace Desktop

    ''' <summary>
    ''' A short enumeration of standard icon type sizes coded for direct insertion into an ICONDIRENTRY structure.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum StandardIcons As UShort
        Icon256 = 0
        Icon128 = &H8080
        Icon64 = &H4040
        Icon48 = &H3030
        Icon32 = &H2020
        Icon16 = &H1010
    End Enum

    ''' <summary>
    ''' Returns the icon image type in a ICONDIRENTRY structure.
    ''' </summary>
    ''' <remarks></remarks>
    <Flags>
    Public Enum IconImageType As Short
        Invalid = 0
        Icon = 1
        Cursor = 2
        IsValid = 3
    End Enum

    Module IconFileStructures

        'ICONDIR structure
        'Offset#	Size (in bytes)	Purpose
        '0	2	Reserved. Must always be 0.
        '2	2	Specifies image type: 1 for icon (.ICO) image, 2 for cursor (.CUR) image. Other values are invalid.
        '4	2	Specifies number of images in the file.
        Public Structure ICONDIR
            Public wReserved As Short
            Public wIconType As IconImageType
            Public nImages As Short
        End Structure

        'ICONDIRENTRY structure
        'Offset#	Size (in bytes)	Purpose
        '0	1	Specifies image width in pixels. Can be any number between 0 and 255. Value 0 means image width is 256 pixels.
        '1	1	Specifies image height in pixels. Can be any number between 0 and 255. Value 0 means image height is 256 pixels.
        '2	1	Specifies number of colors in the color palette. Should be 0 if the image does not use a color palette.
        '3	1	Reserved. Should be 0.[Notes 2]
        '4	2	In ICO format: Specifies color planes. Should be 0 or 1.[Notes 3]
        'In CUR format: Specifies the horizontal coordinates of the hotspot in number of pixels from the left.
        '6	2	In ICO format: Specifies bits per pixel. [Notes 4]
        'In CUR format: Specifies the vertical coordinates of the hotspot in number of pixels from the top.
        '8	4	Specifies the size of the image's data in bytes
        '12	4	Specifies the offset of BMP or PNG data from the beginning of the ICO/CUR file
        <StructLayout(LayoutKind.Explicit)>
        Public Structure ICONDIRENTRY
            <FieldOffset(0)>
            Public wIconType As StandardIcons

            <FieldOffset(0)>
            Public cWidth As Byte

            <FieldOffset(1)>
            Public cHeight As Byte

            <FieldOffset(2)>
            Public cColors As Byte

            <FieldOffset(3)>
            Public cReserved As Byte

            <FieldOffset(4)>
            Public wColorPlanes As Short

            <FieldOffset(4)>
            Public wHotspotX As Short

            <FieldOffset(6)>
            Public wBitsPixel As Short

            <FieldOffset(6)>
            Public wHotspotY As Short

            <FieldOffset(8)>
            Public dwImageSize As Integer

            <FieldOffset(12)>
            Public dwOffset As Integer
        End Structure

    End Module

    ''' <summary>
    ''' Represents a single icon image.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class IconImageEntry
        Implements IDisposable

        Friend _entry As ICONDIRENTRY
        Friend _image As Byte()
        Friend _hIcon As IntPtr = IntPtr.Zero

        Friend Sub New()

        End Sub

        ''' <summary>
        ''' Gets the raw ICONDIRENTRY structure.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Friend ReadOnly Property EntryInfo As ICONDIRENTRY
            Get
                Return _entry
            End Get
        End Property

        ''' <summary>
        ''' Create a new image from the pointer.
        ''' </summary>
        ''' <param name="ptr">Pointer to the start of the ICONDIRENTRY structure.</param>
        ''' <remarks></remarks>
        Friend Sub New(ptr As IntPtr)

            Dim mm As MemPtr = ptr

            _entry = mm.ToStruct(Of ICONDIRENTRY)
            ptr += _entry.dwOffset

            If _entry.wBitsPixel < 24 Then
                ' Throw New InvalidDataException("Reading low-bit icons is not supported")
            End If

            ReDim _image(_entry.dwImageSize - 1)
            Marshal.Copy(ptr, _image, 0, _entry.dwImageSize)

            'MemCpy(_image, ptr, _entry.dwImageSize)

        End Sub

        ''' <summary>
        ''' Extract an icon from an entry and a bits pointer.
        ''' </summary>
        ''' <param name="entry">Icon entry structure.</param>
        ''' <param name="ptr">Pointer to the bitmap.</param>
        ''' <remarks></remarks>
        Friend Sub New(entry As ICONDIRENTRY, ptr As IntPtr)

            _entry = entry

            If _entry.wBitsPixel < 24 Then
                '  Throw New InvalidDataException("Reading low-bit icons is not supported")
            End If

            ptr += _entry.dwOffset

            ReDim _image(_entry.dwImageSize - 1)
            Marshal.Copy(ptr, _image, 0, _entry.dwImageSize)

            'MemCpy(_image, ptr, _entry.dwImageSize)

        End Sub

        ''' <summary>
        ''' Returns the icon type.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property StandardIconType As StandardIcons
            Get
                Return _entry.wIconType
            End Get
        End Property

        ''' <summary>
        ''' Converts this raw icon source into a managed System.Drawing.Icon image.
        ''' </summary>
        ''' <returns>A new Icon object.</returns>
        ''' <remarks></remarks>
        Public Function ToIcon() As Icon
            If IsPngFormat Then
                Dim bmp As Bitmap = CType(ToImage(), Bitmap)
                Dim bmi As New Bitmap(bmp.Width, bmp.Height, Imaging.PixelFormat.Format1bppIndexed)
                Dim lpicon As ICONINFO

                Dim i As Integer

                Dim bm As Drawing.Imaging.BitmapData = bmi.LockBits(New Rectangle(0, 0, bmi.Width, bmi.Height), Imaging.ImageLockMode.ReadWrite, Imaging.PixelFormat.Format1bppIndexed)
                Dim mm As MemPtr = bm.Scan0
                Dim z As Integer = CInt((Math.Max(bmp.Width, 32) * bmp.Height) / 8)

                For i = 0 To z - 1
                    mm.ByteAt(i) = 255
                Next

                bmi.UnlockBits(bm)

                lpicon.hbmColor = bmp.GetHbitmap()
                lpicon.hbmMask = bmi.GetHbitmap()
                lpicon.fIcon = 1

                Dim hIcon As IntPtr = CreateIconIndirect(lpicon)
                If hIcon <> IntPtr.Zero Then
                    ToIcon = CType(Icon.FromHandle(hIcon).Clone, Icon)
                    DestroyIcon(hIcon)
                Else
                    ToIcon = Nothing
                End If

                DeleteObject(lpicon.hbmMask)
                DeleteObject(lpicon.hbmColor)

            Else
                ToIcon = _constructIcon()
            End If
        End Function

        ''' <summary>
        ''' Retrieves the width of the icon.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Width As Integer
            Get
                If _entry.cWidth = 0 Then Return 256 Else Return _entry.cWidth
            End Get
        End Property

        ''' <summary>
        ''' Retrieves the height of the icon.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Height As Integer
            Get
                If _entry.cHeight = 0 Then Return 256 Else Return _entry.cHeight
            End Get
        End Property

        ''' <summary>
        ''' Returns True if the icon image data is in compressed PNG format.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property IsPngFormat As Boolean
            Get
                If _image Is Nothing Then Return False
                Dim mm As SafePtr = CType(_image, SafePtr)

                Dim q As Integer = mm.IntAt(0)

                '' The PNG moniker
                IsPngFormat = q = &H474E5089

            End Get
        End Property

        ''' <summary>
        ''' Returns a new System.Drawing.Image from this raw icon.
        ''' </summary>
        ''' <returns>A new Image object.</returns>
        ''' <remarks></remarks>
        Public Function ToImage() As Image

            If IsPngFormat Then
                Dim s As New MemoryStream(_image)

                ToImage = Image.FromStream(s)
                s.Close()
            Else
                ToImage = IconToTransparentBitmap(_constructIcon)
            End If

        End Function

        ''' <summary>
        ''' Create a bitmap structure with bits from this raw icon image.
        ''' </summary>
        ''' <returns>An array of bytes that represent data to create a bitmap.</returns>
        ''' <remarks></remarks>
        Private Function _makeBitmap() As Byte()

            If Not IsPngFormat Then
                _makeBitmap = _image
                Exit Function
            End If

            Dim bmp As IntPtr = Nothing
            Dim hbmp As IntPtr = MakeDIBSection(CType(ToImage(), Bitmap), bmp)

            Dim mm As New SafePtr
            Dim bm As New BITMAPINFOHEADER
            Dim maskSize As Integer
            Dim w As Integer = _entry.cWidth
            Dim h As Integer = _entry.cHeight

            If w = 0 Then w = 256
            If h = 0 Then h = 256

            bm.biSize = 40
            bm.biWidth = w
            bm.biHeight = h * 2
            bm.biPlanes = 1
            bm.biBitCount = 32
            bm.biSizeImage = w * h * 4

            maskSize = CInt((Math.Max(w, 32) * h) / 8)
            mm.Alloc(bm.biSizeImage + 40 + maskSize)

            Dim ptr1 As IntPtr = mm.DangerousGetHandle + 40
            Dim ptr2 As IntPtr = mm.DangerousGetHandle + 40 + (bm.biSizeImage)

            Marshal.StructureToPtr(bm, mm.DangerousGetHandle, False)

            NativeLib.Native.MemCpy(bmp, ptr1, bm.biSizeImage)

            bm = mm.ToStruct(Of BITMAPINFOHEADER)
            _setMask(ptr1, ptr2, w, h)

            _entry.dwImageSize = CInt(mm.Size)

            _makeBitmap = CType(mm, Byte())
            mm.Free()

            DeleteObject(hbmp)

        End Function

        ''' <summary>
        ''' Construct an icon from the raw image data.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function _constructIcon() As Icon

            If _hIcon <> IntPtr.Zero Then
                DestroyIcon(_hIcon)
                _hIcon = IntPtr.Zero
            End If

            Dim mm As MemPtr = CType(_image, MemPtr)
            Dim bmp As BITMAPINFOHEADER = mm.ToStruct(Of BITMAPINFOHEADER)
            Dim hBmp As IntPtr
            Dim ptr As IntPtr
            Dim ppBits As IntPtr
            Dim lpicon As ICONINFO
            Dim hicon As IntPtr
            Dim hBmpMask As IntPtr
            Dim hasMask As Boolean

            If bmp.biHeight = bmp.biWidth * 2 Then
                hasMask = True
                bmp.biHeight = CInt(bmp.biHeight / 2)
            Else
                hasMask = False
            End If

            bmp.biSizeImage = CInt(bmp.biWidth * bmp.biHeight * (bmp.biBitCount / 8))
            bmp.biXPelsPerMeter = CInt(24.5 * 1000)
            bmp.biYPelsPerMeter = CInt(24.5 * 1000)
            bmp.biClrUsed = 0
            bmp.biClrImportant = 0
            bmp.biPlanes = 1

            Marshal.StructureToPtr(bmp, mm.Handle, False)
            ptr = mm.Handle + bmp.biSize

            If bmp.biSize <> 40 Then Return Nothing

            hBmp = CreateDIBSection(IntPtr.Zero, mm.Handle, 0, ppBits, IntPtr.Zero, 0)
            NativeLib.Native.MemCpy(ptr, ppBits, bmp.biSizeImage)

            If hasMask Then
                ptr += bmp.biSizeImage

                bmp.biBitCount = 1
                bmp.biSizeImage = 0
                Marshal.StructureToPtr(bmp, mm.Handle, False)
                hBmpMask = CreateDIBSection(IntPtr.Zero, mm.Handle, 0, ppBits, IntPtr.Zero, 0)

                NativeLib.Native.MemCpy(ptr, ppBits, CLng((Math.Max(bmp.biWidth, 32) * bmp.biHeight) / 8))
            End If

            lpicon.fIcon = 1
            lpicon.hbmColor = hBmp
            lpicon.hbmMask = hBmpMask

            hicon = CreateIconIndirect(lpicon)

            DeleteObject(hBmp)
            If hasMask Then DeleteObject(hBmpMask)

            _constructIcon = Icon.FromHandle(hicon)

            _hIcon = hicon
            mm.Free()

        End Function

        ''' <summary>
        ''' Apply transparency mask bits to the output image (for converting to a bitmap)
        ''' </summary>
        ''' <param name="hBits">A pointer to the memory address of the bitmap bits.</param>
        ''' <param name="hMask">A pointer to the memory address of the mask bits.</param>
        ''' <param name="Width">The width of the image.</param>
        ''' <param name="Height">The height of the image.</param>
        ''' <remarks></remarks>
        Private Sub _applyMask(hBits As MemPtr, hMask As MemPtr, Width As Integer, Height As Integer)
            '' Masks in icon images are bitstreams wherein a single bit represents a 1 or 0 transparency
            '' for an entire pixel on the screen.  In order to convert an icon into a 32 bit images,
            '' we need to access each individual bit, and apply the NOT of the value to the byte-length alpha mask
            '' of the bitmap.

            Dim x As Integer,
            y As Integer

            Dim shift As Integer
            Dim bit As Integer
            Dim shift2 As Integer
            Dim mask As Integer

            '' in transparency masks for icons, the minimum stride is 32 pixels/bits, no matter the actual size of the image.
            Dim boundary As Integer = Math.Max(32, Width)

            '' walk every pixel of the image.
            For y = 0 To Height - 1
                For x = 0 To Width - 1

                    '' the first shift is our position in the bitmap output.
                    '' 4 bytes is 32 bits ... then we add 3 to get directly 
                    '' to the alpha mask.
                    shift = 4 * (x + y * Width) + 3

                    '' we find the exact bit-wise position by modulus with 8 (the length of a byte, in bits)
                    bit = 7 - (x Mod 8)

                    '' the second shift is the position in the bitmask, byte-wise.  We subtract 1 from y before subtracting it from the
                    '' height because the first scan line is 0.
                    shift2 = CInt((x + (Height - y - 1) * boundary) / 8)

                    '' we get a number that is either 1 or 0 from the mask by accessing the exact byte, and then
                    '' accessing the exact bit by left-shifting its value into the 1 position.
                    mask = (1 And (hMask.ByteAt(shift2) >> bit))

                    '' we do a quick logical AND via multiplication with the inverse of the mask.
                    '' we do this because alpha channel 0 is transparent, but transparent mask 1 is also transparent.
                    hBits.ByteAt(shift) *= CByte(1 - mask)
                Next
            Next

        End Sub

        ''' <summary>
        ''' Create a transparency mask from the transparent bits in an image.
        ''' </summary>
        ''' <param name="hBits">A pointer to the memory address of the bitmap bits.</param>
        ''' <param name="hMask">A pointer to the memory address of the mask bits.</param>
        ''' <param name="Width">The width of the image.</param>
        ''' <param name="Height">The height of the image.</param>
        ''' <remarks></remarks>
        Private Sub _setMask(hBits As MemPtr, hMask As MemPtr, Width As Integer, Height As Integer)
            ' this never changes
            Dim numBits As Integer = 32

            Dim x As Integer,
            y As Integer

            Dim bit As Byte = 0
            Dim mask As Byte = 0

            Dim d As Integer
            Dim e As Integer
            Dim f As Integer

            Dim move = numBits / 8


            Dim stride As Integer = CInt(Width * (numBits / 8))
            Dim msize As Integer = CInt((Math.Max(32, Width) * Height) / 8)
            Dim isize As Integer = CInt(((Width * Height) * (numBits / 8)))

            Dim bb() As Byte
            Dim imgb() As Byte

            ReDim imgb(isize - 1)
            ReDim bb(msize - 1)

            Marshal.Copy(hBits.Handle, imgb, 0, isize)

            For y = 0 To Height - 1
                d = y * stride
                For x = 0 To Width - 1
                    f = CInt(d + (x * move))
                    e = CInt(Math.Floor(x / 8))
                    bit = CByte(7 - (x Mod 8))

                    If imgb(f + 3) = 0 Then
                        bb(e) = CByte(bb(e) Or (1 << bit))
                    End If

                Next
            Next

            'MemCpy(hMask.Handle, bb, msize)

            hMask.FromByteArray(bb, 0)

            hBits = IntPtr.Zero
            hMask = IntPtr.Zero

        End Sub

        ''' <summary>
        ''' Initialize a new raw icon image from a standard image with the specified size as either a bitmap or PNG icon.
        ''' </summary>
        ''' <param name="Image">Image from which to construct the icon.</param>
        ''' <param name="size">The standard size of the new icon.</param>
        ''' <param name="asBmp">Whether to create a bitmap icon (if false, a PNG icon is created).</param>
        ''' <remarks></remarks>
        Public Sub New(Image As Image, size As StandardIcons, Optional asBmp As Boolean = False)

            Dim sz As Integer = If(size <> 0, CType(size, Integer) And &HFF, 256)
            Dim im As Bitmap = CType(Image, Bitmap)

            If Image.Width <> sz Or Image.Height <> sz Then
                im = New Bitmap(sz, sz, Imaging.PixelFormat.Format32bppArgb)
                Dim g As Graphics = Graphics.FromImage(im)

                g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
                g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
                g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality

                g.DrawImage(Image, New Rectangle(0, 0, sz, sz))
                g.Dispose()
            End If

            Dim s As New MemoryStream
            im.Save(s, Imaging.ImageFormat.Png)

            _entry.wIconType = size
            _entry.wBitsPixel = 32
            _entry.wColorPlanes = 1

            ReDim _image(CInt(s.Length - 1))
            _image = s.ToArray

            If asBmp Then
                _image = _makeBitmap()
            End If

            _entry.dwImageSize = _image.Length
            s.Close()

        End Sub

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then

                If _hIcon <> IntPtr.Zero Then
                    DestroyIcon(_hIcon)
                End If
            End If
            Me.disposedValue = True
        End Sub

        Protected Overrides Sub Finalize()
            Dispose(False)
            MyBase.Finalize()
        End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

    End Class

    ''' <summary>
    ''' Represents an entire icon image file.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class IconImage

        Private _dir As ICONDIR
        Private _entries As New List(Of IconImageEntry)

        Private _FileName As String
        Private _ShowSaveDialog As Boolean = False

        ''' <summary>
        ''' Gets or sets the filename of the icon file.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property FileName As String
            Get
                FileName = _FileName
            End Get
            Set(value As String)
                _FileName = value
                LoadIcon()
            End Set
        End Property

        ''' <summary>
        ''' Retrieves a list of icon images stored in the icon file.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Entries As List(Of IconImageEntry)
            Get
                Return _entries
            End Get
        End Property

        ''' <summary>
        ''' Finds an icon by standard size.
        ''' </summary>
        ''' <param name="StandardIconType"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function FindIcon(StandardIconType As StandardIcons) As IconImageEntry
            For Each e In _entries
                If e.StandardIconType = StandardIconType Then
                    FindIcon = e
                    Exit Function
                End If
            Next
            FindIcon = Nothing
        End Function

        ''' <summary>
        ''' Loads the icon from the file specified in the Filename property.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function LoadIcon() As Boolean
            Return LoadIcon(_FileName)
        End Function

        ''' <summary>
        ''' Loads an icon from a stream.
        ''' </summary>
        ''' <param name="stream">The stream to load.</param>
        ''' <param name="closeStream">Whether or not to close the stream when finished.</param>
        ''' <returns>True if successful.</returns>
        ''' <remarks></remarks>
        Public Function LoadIcon(stream As Stream, Optional closeStream As Boolean = True) As Boolean
            LoadIcon = _internalLoadFromStream(stream, closeStream)
        End Function

        ''' <summary>
        ''' Loads an icon from a memory pointer.
        ''' </summary>
        ''' <param name="ptr">The memory pointer to load.</param>
        ''' <returns>True if successful.</returns>
        ''' <remarks></remarks>
        Public Function LoadIcon(ptr As IntPtr) As Boolean
            LoadIcon = _internalLoadFromPtr(ptr)
        End Function

        ''' <summary>
        ''' Loads an icon from a byte array.
        ''' </summary>
        ''' <param name="bytes">Buffer to load.</param>
        ''' <returns>True if successful.</returns>
        ''' <remarks></remarks>
        Public Function LoadIcon(bytes As Byte()) As Boolean
            LoadIcon = _internalLoadFromBytes(bytes)
        End Function

        ''' <summary>
        ''' Loads an icon from the specified file.
        ''' </summary>
        ''' <param name="fileName">Filename of the icon to load.</param>
        ''' <returns>True if successful.</returns>
        ''' <remarks></remarks>
        Public Function LoadIcon(fileName As String) As Boolean

            _FileName = fileName
            LoadIcon = _internalLoadFromFile(fileName)
        End Function

        ''' <summary>
        ''' Loads an icon from a file using an OpenFileDialog.
        ''' </summary>
        ''' <param name="fileName">Sets or receives the selected file.</param>
        ''' <param name="prompt">Specify whether or not to raise the OpenFileDialog.</param>
        ''' <returns>True if successful.</returns>
        ''' <remarks></remarks>
        Public Function LoadIcon(ByRef fileName As String, prompt As Boolean) As Boolean
            If prompt Then
                Dim ofd As New System.Windows.Forms.OpenFileDialog

                ofd.Filter = "Icon Files (*.ico)|*.ico|Cursor Files (*.cur)|*.cur|All Files (*.*)|*.*"
                ofd.Title = "Open Icon"
                If ofd.ShowDialog = System.Windows.Forms.DialogResult.OK Then
                    LoadIcon = LoadIcon(ofd.FileName)
                Else
                    LoadIcon = False
                End If

            Else
                LoadIcon = LoadIcon(fileName)
            End If
        End Function

        ''' <summary>
        ''' Save the icon to the filename specified in the Filename property.
        ''' </summary>
        ''' <returns>True if successful.</returns>
        ''' <remarks></remarks>
        Public Function SaveIcon() As Boolean
            SaveIcon = SaveIcon(_FileName)
        End Function

        ''' <summary>
        ''' Saves the icon to the specified file.
        ''' </summary>
        ''' <param name="fileName">The file to save.</param>
        ''' <returns>True if successful.</returns>
        ''' <remarks></remarks>
        Public Function SaveIcon(fileName As String) As Boolean
            SaveIcon = _internalSaveToFile(fileName)
        End Function

        ''' <summary>
        ''' Saves an icon to the specified stream.
        ''' </summary>
        ''' <param name="stream">The stream to save.</param>
        ''' <returns>True if successful.</returns>
        ''' <remarks></remarks>
        Public Function SaveIcon(stream As Stream) As Boolean
            SaveIcon = _internalSaveToStream(stream)
        End Function

        ''' <summary>
        ''' Saves an icon to the specified file with a SaveFileDialog.
        ''' </summary>
        ''' <param name="fileName">Sets or receives the selected file.</param>
        ''' <param name="prompt">True to raise the SaveFileDialog.</param>
        ''' <returns>True if successful.</returns>
        ''' <remarks></remarks>
        Public Function SaveIcon(ByRef fileName As String, prompt As Boolean) As Boolean
            If prompt Then
                Dim sfd As New System.Windows.Forms.SaveFileDialog

                sfd.Filter = "Icon Files (*.ico)|*.ico|Cursor Files (*.cur)|*.cur"
                sfd.Title = "Save Icon"
                If sfd.ShowDialog = System.Windows.Forms.DialogResult.OK Then
                    fileName = sfd.FileName
                    SaveIcon = SaveIcon(fileName)
                Else
                    SaveIcon = False
                End If
            Else
                SaveIcon = SaveIcon(fileName)
            End If
        End Function

        ''' <summary>
        ''' Internal load icon.
        ''' </summary>
        ''' <param name="ptr">The pointer to load.</param>
        ''' <returns>True if successful.</returns>
        ''' <remarks></remarks>
        Private Function _internalLoadFromPtr(ptr As IntPtr) As Boolean

            ' get the icon file header directory.
            Dim mm As MemPtr = ptr

            _dir = mm.ToStruct(Of ICONDIR)

            Dim i As Integer,
            c As Integer = _dir.nImages - 1

            Dim f As Integer = Marshal.SizeOf(Of ICONDIRENTRY)
            Dim e As Integer = Marshal.SizeOf(Of ICONDIR)

            Dim optr As IntPtr = ptr

            If _dir.nImages <= 0 OrElse _dir.wReserved <> 0 OrElse 0 = (_dir.wIconType And IconImageType.IsValid) Then
                Return False
            End If

            _entries = New List(Of IconImageEntry)

            mm += e
            For i = 0 To c
                '' load all images in sequence.
                _entries.Add(New IconImageEntry(mm.ToStruct(Of ICONDIRENTRY), optr))
                ptr += f
            Next

            _internalLoadFromPtr = True

        End Function

        ''' <summary>
        ''' Internal load from bytes.
        ''' </summary>
        ''' <param name="bytes"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function _internalLoadFromBytes(bytes() As Byte) As Boolean
            Dim mm As SafePtr = CType(bytes, SafePtr)
            _internalLoadFromBytes = _internalLoadFromPtr(mm)
            mm.Dispose()
        End Function

        ''' <summary>
        ''' Internal load from stream.
        ''' </summary>
        ''' <param name="stream"></param>
        ''' <param name="closeStream"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function _internalLoadFromStream(stream As Stream, closeStream As Boolean) As Boolean
            Dim b() As Byte
            ReDim b(CInt(stream.Length - 1))

            stream.Seek(0, SeekOrigin.Begin)
            stream.Read(b, 0, CInt(stream.Length))
            If closeStream Then stream.Close()

            _internalLoadFromStream = _internalLoadFromBytes(b)
        End Function

        ''' <summary>
        ''' Internal save by file.
        ''' </summary>
        ''' <param name="fileName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function _internalLoadFromFile(fileName As String) As Boolean
            Dim fs As New FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read, 10240)

            _internalLoadFromFile = _internalLoadFromStream(fs, True)
        End Function

        ''' <summary>
        ''' Internally saves the icon to the specified stream.
        ''' </summary>
        ''' <param name="stream">Stream to save.</param>
        ''' <returns>True if successful.</returns>
        ''' <remarks></remarks>
        Private Function _internalSaveToStream(stream As Stream) As Boolean

            Dim bl As New SafePtr
            Dim f As Integer = Marshal.SizeOf(_dir)
            Dim g As Integer = Marshal.SizeOf(Of ICONDIRENTRY)

            _dir.nImages = CShort(_entries.Count)
            _dir.wIconType = IconImageType.Icon
            _dir.wReserved = 0

            ' get the index to the first image's image data
            Dim offset As Integer = f + (g * _dir.nImages)

            bl += StructToBytes(_dir)

            For Each e In _entries
                e._entry.dwOffset = offset
                bl += StructToBytes(e._entry)
                offset += e._entry.dwImageSize
            Next

            For Each e In _entries
                bl += (e._image)
            Next

            '' write the icon file
            stream.Write(CType(bl, Byte()), 0, CInt(bl.Size))
            stream.Close()
            bl.Dispose()

            _internalSaveToStream = True

        End Function

        Private Function _internalSaveToFile(fileName As String) As Boolean

            Dim fs As New FileStream(fileName, FileMode.CreateNew, FileAccess.Write, FileShare.None, 10240)
            _internalSaveToFile = _internalSaveToStream(fs)

        End Function

        Public Sub New()

        End Sub

        ''' <summary>
        ''' Create a new instance of this object from a stream
        ''' </summary>
        ''' <param name="stream"></param>
        ''' <param name="closeStream"></param>
        ''' <remarks></remarks>
        Public Sub New(stream As Stream, Optional closeStream As Boolean = True)
            _internalLoadFromStream(stream, closeStream)
        End Sub

        ''' <summary>
        ''' Create a new instance of this object from a memory pointer
        ''' </summary>
        ''' <param name="ptr"></param>
        ''' <remarks></remarks>
        Public Sub New(ptr As IntPtr)
            _internalLoadFromPtr(ptr)
        End Sub

        ''' <summary>
        ''' Create a new instance of this object from the byte array
        ''' </summary>
        ''' <param name="bytes"></param>
        ''' <remarks></remarks>
        Public Sub New(bytes As Byte())
            _internalLoadFromBytes(bytes)
        End Sub

        ''' <summary>
        ''' Create a new instance of this object from the specified file
        ''' </summary>
        ''' <param name="fileName"></param>
        ''' <remarks></remarks>
        Public Sub New(fileName As String)
            If _internalLoadFromFile(fileName) Then
                _FileName = fileName
            End If
        End Sub

    End Class

End Namespace