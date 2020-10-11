'' ************************************************* ''
'' DataTools Visual Basic Utility Library - Interop
''
'' Module: TrueType.
''         Code to read TrueType font file information
''         Adapted from the CodeProject article: http://www.codeproject.com/Articles/2293/Retrieving-font-name-from-TTF-file?msg=4714219#xx4714219xx
''
'' 
'' Copyright (C) 2011-2020 Nathan Moschkin
'' All Rights Reserved
''
'' Licensed Under the Microsoft Public License   
'' ************************************************* ''

Imports System.IO
Imports System.Runtime.InteropServices
Imports DataTools.Interop.Native
Imports CoreCT.Memory
Imports System.Collections.ObjectModel
Imports CoreCT.Text.TextTools

Namespace Desktop

    Public Module TrueTypeFont

#Region "Structure Definitions"

        '' This is TTF file header
        <StructLayout(LayoutKind.Sequential)>
        Public Structure TT_OFFSET_TABLE
            Public uMajorVersion As UShort
            Public uMinorVersion As UShort
            Public uNumOfTables As UShort
            Public uSearchRange As UShort
            Public uEntrySelector As UShort
            Public uRangeShift As UShort
        End Structure

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Ansi)>
        Public Structure TT_TABLE_DIRECTORY

            <MarshalAs(UnmanagedType.ByValArray, SizeConst:=4)>
            Public szTag() As Byte

            Public uCheckSum As UInteger
            Public uOffset As UInteger
            Public uLength As UInteger

            Public ReadOnly Property Tag As String
                Get
                    Return System.Text.ASCIIEncoding.ASCII.GetString(szTag)
                End Get
            End Property

        End Structure

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Ansi)>
        Public Structure TT_NAME_TABLE_HEADER
            Public uFSelector As UShort     ' format selector. Always 0
            Public uNRCount As UShort       ' Name Records count
            Public uStorageOffset As UShort ' Offset for strings storage, from start of the table
        End Structure

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Ansi)>
        Public Structure TT_NAME_RECORD
            Public uPlatformID As UShort
            Public uEncodingID As UShort
            Public uLanguageID As UShort
            Public uNameID As UShort
            Public uStringLength As UShort
            Public uStringOffset As UShort ' from start of storage area
        End Structure

#End Region

#Region "Endian Swapping Functions"

        ''' <summary>
        ''' Change the Endianness of a 16-bit unsigned integer
        ''' </summary>
        ''' <param name="val">A 16-bit number</param>
        ''' <returns>The reverse endian format of val.</returns>
        ''' <remarks></remarks>
        Public Function Swap(val As UShort) As UShort
            Dim v1 As Byte,
            v2 As Byte

            v1 = CByte(val And &HFF)
            v2 = CByte((val >> 8) And &HFF)

            Return ((CUShort(v1) << 8) Or CUShort(v2))
        End Function

        ''' <summary>
        ''' Change the Endianness of a 32-bit unsigned integer
        ''' </summary>
        ''' <param name="val">A 32-bit number</param>
        ''' <returns>The reverse endian format of val.</returns>
        ''' <remarks></remarks>
        Public Function Swap(val As UInteger) As UInteger
            Dim v1 As UShort,
            v2 As UShort

            v1 = CUShort(val And &HFFFF)
            v2 = CUShort((val >> 16) And &HFFFF)

            Return ((CUInt(Swap(v1)) << 16) Or CUInt(Swap(v2)))
        End Function

        ''' <summary>
        ''' Change the Endianness of a 64-bit unsigned integer
        ''' </summary>
        ''' <param name="val">A 64-bit number</param>
        ''' <returns>The reverse endian format of val.</returns>
        ''' <remarks></remarks>
        Public Function Swap(val As ULong) As ULong
            Dim v1 As UInteger,
            v2 As UInteger

            v1 = CUInt(val And &HFFFFFFFFUL)
            v2 = CUInt((val >> 32) And &HFFFFFFFFUL)

            Return ((CULng(Swap(v1)) << 32) Or CULng(Swap(v2)))
        End Function

#End Region

#Region "GetTTFName() Function"

        Public Function GetTTFName(fileName As String) As String
            Dim fs As FileStream
            Dim oft As New TT_OFFSET_TABLE
            Dim tdir As New TT_TABLE_DIRECTORY
            Dim nth As New TT_NAME_TABLE_HEADER
            Dim nr As New TT_NAME_RECORD
            Dim i As Integer,
            p As Integer

            Dim sRet As String = Nothing

            Try
                fs = New FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical)
                Return Nothing
            End Try

            ReadStruct(Of TT_OFFSET_TABLE)(fs, oft)

            oft.uNumOfTables = Swap(oft.uNumOfTables)
            oft.uMajorVersion = Swap(oft.uMajorVersion)
            oft.uMinorVersion = Swap(oft.uMinorVersion)

            '' Not a TrueType v1.0 font!
            If oft.uMajorVersion <> 1 OrElse oft.uMinorVersion <> 0 Then Return Nothing

            For i = 0 To oft.uNumOfTables - 1

                ReadStruct(Of TT_TABLE_DIRECTORY)(fs, tdir)
                If tdir.Tag.ToLower = "name" Then
                    tdir.uLength = Swap(tdir.uLength)
                    tdir.uOffset = Swap(tdir.uOffset)
                    Exit For
                End If

            Next

            '' Exhausted all records, no name record found!
            If i >= oft.uNumOfTables Then Return Nothing

            fs.Seek(tdir.uOffset, SeekOrigin.Begin)

            ReadStruct(Of TT_NAME_TABLE_HEADER)(fs, nth)

            nth.uStorageOffset = Swap(nth.uStorageOffset)
            nth.uNRCount = Swap(nth.uNRCount)

            For i = 0 To nth.uNRCount - 1

                ReadStruct(Of TT_NAME_RECORD)(fs, nr)

                nr.uNameID = Swap(nr.uNameID)
                If nr.uNameID = 1 Then
                    p = CInt(fs.Position)

                    nr.uStringLength = Swap(nr.uStringLength)
                    nr.uStringOffset = Swap(nr.uStringOffset)
                    fs.Seek(tdir.uOffset + nr.uStringOffset + nth.uStorageOffset, SeekOrigin.Begin)
                    nr.uEncodingID = Swap(nr.uEncodingID)
                    nr.uPlatformID = Swap(nr.uPlatformID)

                    Dim b() As Byte
                    ReDim b(nr.uStringLength - 1)

                    fs.Read(b, 0, nr.uStringLength)

                    ' Platform IDs: 0 = Unicode, 1 = Macintosh, 3 = Windows
                    If (nr.uPlatformID = 0) Then
                        sRet = System.Text.Encoding.BigEndianUnicode.GetString(b)
                    Else
                        sRet = System.Text.ASCIIEncoding.ASCII.GetString(b)
                    End If

                    sRet = sRet.Trim(ChrW(0))

                    If sRet <> "" Then
                        Exit For
                    End If

                    sRet = Nothing
                    fs.Seek(p, SeekOrigin.Begin)
                End If

            Next

            Return sRet

        End Function

#End Region

    End Module


#Region "Font Collection"

    Public Enum SortOrder
        Ascending
        Descending
    End Enum

    Public Enum FontWeight As Integer

        DontCare = 0
        Thin = 100
        ExtraLight = 200
        Light = 300
        Normal = 400
        Medium = 500
        SemiBold = 600
        Bold = 700
        ExtraBold = 800
        Heavy = 900
    End Enum

    Public Enum FontCharSet As Byte

        ANSI = 0
        [Default] = 1
        Symbol = 2
        ShiftJIS = 128
        Hangeul = 129
        Hangul = 129
        GB2312 = 134
        ChineseBIG5 = 136
        OEM = 255
        Johab = 130
        Hebrew = 177
        Arabic = 178
        Greek = 161
        Turkish = 162
        Vietnamese = 163
        Thai = 222
        EasternEurope = 238
        Russian = 204
        Mac = 77
        Baltic = 186

    End Enum

    <Flags>
    Public Enum FontFamilies
        Decorative = 1
        DontCare = 2
        Modern = 4
        Roman = 8
        Script = 16
        Swiss = 32
    End Enum

    Public Enum FontPitch
        [Default] = FontPitchAndFamily.DEFAULT_PITCH
        Variable = FontPitchAndFamily.VARIABLE_PITCH
        Fixed = FontPitchAndFamily.FIXED_PITCH
    End Enum


    ''' <summary>
    ''' Represents information about a font on the current system.
    ''' </summary>
    Public NotInheritable Class FontInfo

        Friend elf As ENUMLOGFONTEX
        Friend lf As LOGFONT

        ''' <summary>
        ''' Gets the font name.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Name As String
            Get
                Return elf.elfFullName
            End Get
        End Property

        ''' <summary>
        ''' Gets the font script.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Script As String
            Get
                Return elf.elfScript
            End Get
        End Property

        ''' <summary>
        ''' Gets the font style.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Style As String
            Get
                Return elf.elfStyle
            End Get
        End Property

        ''' <summary>
        ''' Gets the font weight.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Weight As FontWeight
            Get
                Return CType(lf.lfWeight, FontWeight)
            End Get
        End Property

        ''' <summary>
        ''' Gets the font character set.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property CharacterSet As FontCharSet
            Get
                Return CType(lf.lfCharSet, FontCharSet)
            End Get
        End Property

        ''' <summary>
        ''' Gets the font pitch.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Pitch As FontPitch
            Get
                Return CType(lf.lfPitchAndFamily And 3, FontPitch)
            End Get
        End Property

        ''' <summary>
        ''' Gets the font family.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Family As FontFamilies
            Get
                Dim v As FontPitchAndFamily = CType((lf.lfPitchAndFamily >> 2) << 2, FontPitchAndFamily)

                Select Case v
                    Case FontPitchAndFamily.FF_DECORATIVE
                        Return FontFamilies.Decorative

                    Case FontPitchAndFamily.FF_DONTCARE
                        Return FontFamilies.DontCare

                    Case FontPitchAndFamily.FF_MODERN
                        Return FontFamilies.Modern

                    Case FontPitchAndFamily.FF_ROMAN
                        Return FontFamilies.Roman

                    Case FontPitchAndFamily.FF_SWISS
                        Return FontFamilies.Swiss

                    Case FontPitchAndFamily.FF_SCRIPT
                        Return FontFamilies.Script

                End Select

                Return CType(v, FontFamilies)
            End Get
        End Property

        ''' <summary>
        ''' Copy the ENUMLOGFONTEX structure for this object into a memory buffer.
        ''' </summary>
        ''' <param name="lpElf">Pointer to a buffer to receive the ENUMLOGFONTEX structure.  The memory must already be allocated and freed by the caller.</param>
        Public Sub GetElfEx(lpElf As IntPtr)

            Dim mm As New MemPtr(lpElf)
            mm.FromStruct(Of ENUMLOGFONTEX)(elf)

        End Sub

        Friend Sub New(elf As ENUMLOGFONTEX)
            Me.elf = elf
            lf = elf.elfLogFont


        End Sub

        Public Overrides Function ToString() As String
            Return Name
        End Function

    End Class


    ''' <summary>
    ''' A static collection of fonts returned by the <see cref="FontCollection.GetFonts"/> static method according to the specified criteria.
    ''' If you require a list of all fonts on the system in the default character set, reference <see cref="FontCollection.SystemFonts"/>, instead.
    ''' </summary>
    Public NotInheritable Class FontCollection
        Implements ICollection(Of FontInfo)


        Public Enum FontSearchOptions
            Contains = 0
            BeginsWith = 1
            EndsWith = 2
        End Enum

        Private _List As New List(Of FontInfo)



        Private Delegate Function EnumFontFamExProc(ByRef lpelfe As ENUMLOGFONTEX,
                                                   lpntme As IntPtr,
                                                   FontType As UInteger,
                                                   lparam As IntPtr) As Integer



        <DllImport("gdi32.dll", CharSet:=CharSet.Auto)>
        Private Shared Function EnumFontFamiliesEx _
            (hdc As IntPtr,
             lpLogFont As IntPtr,
             <MarshalAs(UnmanagedType.FunctionPtr)>
             lpEnumFontFamExProc As EnumFontFamExProc,
             lparam As IntPtr,
             dwflags As UInteger) As Integer
        End Function

        ''' <summary>
        ''' Returns a static collection of all fonts on the system in the default character set.
        ''' </summary>
        ''' <returns></returns>
        Public Shared ReadOnly Property SystemFonts As FontCollection


        Private Shared Function CheckFamily(lf As LOGFONT, families As FontFamilies) As Boolean

            Dim v As FontPitchAndFamily = CType((lf.lfPitchAndFamily >> 2) << 2, FontPitchAndFamily)

            Select Case v
                Case FontPitchAndFamily.FF_DECORATIVE
                    If (families And FontFamilies.Decorative) = 0 Then Return False

                Case FontPitchAndFamily.FF_DONTCARE
                    Return If(families = FontFamilies.DontCare, True, False)

                Case FontPitchAndFamily.FF_MODERN
                    If (families And FontFamilies.Modern) = 0 Then Return False

                Case FontPitchAndFamily.FF_ROMAN
                    If (families And FontFamilies.Roman) = 0 Then Return False

                Case FontPitchAndFamily.FF_SWISS
                    If (families And FontFamilies.Swiss) = 0 Then Return False

                Case FontPitchAndFamily.FF_SCRIPT
                    If (families And FontFamilies.Script) = 0 Then Return False

            End Select

            Return True

        End Function

        ''' <summary>
        ''' Gets a collection of fonts based on the specified criteria.
        ''' </summary>
        ''' <param name="families">Bit Field representing which font families to retrieve.</param>
        ''' <param name="pitch">Specify the desired pitch.</param>
        ''' <param name="charset">Specify the desired character set.</param>
        ''' <param name="weight">Specify the desired weight.</param>
        ''' <param name="Script">Specify the desired script(s) (this can be a String or an array of Strings).</param>
        ''' <param name="Style">Specify the desired style(s) (this can be a String or an array of Strings).</param>
        ''' <returns></returns>
        Public Shared Function GetFonts(Optional families As FontFamilies = FontFamilies.DontCare, Optional pitch As FontPitch = FontPitch.Default, Optional charset As FontCharSet = FontCharSet.Default, Optional weight As FontWeight = FontWeight.DontCare, Optional Script As Object = Nothing, Optional Style As Object = Nothing) As FontCollection

            Dim hdc As IntPtr
            Dim fonts As New List(Of ENUMLOGFONTEX)

            Dim i As Integer = 0
            Dim lf As LOGFONT = New LOGFONT
            Dim tm As New TEXTMETRIC
            Dim s As String

            Dim mm As MemPtr

            Dim wscript() As String
            Dim wstyle() As String

            If Script Is Nothing Then
                wscript = {"Western"}
            Else
                If TypeOf Script Is String Then
                    wscript = {CStr(Script)}
                ElseIf TypeOf Script Is String() Then
                    wscript = CType(Script, String())
                Else
                    Throw New ArgumentException("Invalid parameter type for Script")
                End If
            End If

            If Style Is Nothing Then
                wstyle = {"", "Normal", "Regular"}
            Else
                If TypeOf Style Is String Then
                    wstyle = {CStr(Style)}
                ElseIf TypeOf Style Is String() Then
                    wstyle = CType(Style, String())
                Else
                    Throw New ArgumentException("Invalid parameter type for Style")
                End If
            End If


            lf.lfCharSet = charset
            lf.lfFaceName = ""

            mm.Alloc(Marshal.SizeOf(lf))
            mm.FromStruct(Of LOGFONT)(lf)


            hdc = CreateDC("DISPLAY", Nothing, IntPtr.Zero, IntPtr.Zero)

            Dim e As Integer
            Dim bo As Boolean = False

            e = EnumFontFamiliesEx(hdc, mm, Function(ByRef lpelfe As ENUMLOGFONTEX, lpntme As IntPtr, FontType As UInteger, lParam As IntPtr) As Integer

                                                Dim z As Integer
                                                If (fonts Is Nothing) Then z = 0 Else z = fonts.Count


                                                ' make sure it's the normal, regular version

                                                bo = False
                                                For Each y In wstyle
                                                    If y.ToLower = lpelfe.elfStyle.ToLower Then
                                                        bo = True
                                                        Exit For
                                                    End If
                                                Next
                                                If bo = False Then Return 1

                                                bo = False
                                                For Each y In wscript
                                                    If y.ToLower = lpelfe.elfScript.ToLower Then
                                                        bo = True
                                                        Exit For
                                                    End If
                                                Next
                                                If bo = False Then Return 1

                                                bo = False

                                                If weight <> FontWeight.DontCare AndAlso lpelfe.elfLogFont.lfWeight <> weight Then Return 1

                                                ' we don't really need two of the same font.
                                                If (z > 0) Then
                                                    If lpelfe.elfFullName = fonts(z - 1).elfFullName Then Return 1
                                                End If

                                                ' the @ indicates a vertical writing font which we definitely do not want.
                                                If (lpelfe.elfFullName.Substring(0, 1) = "@") Then Return 1

                                                If Not CheckFamily(lpelfe.elfLogFont, families) Then Return 1

                                                'lpelfe.elfLogFont.lfCharSet = charset
                                                'If (lpelfe.elfLogFont.lfCharSet <> charset) Then Return 1

                                                If (pitch <> FontPitch.Default) AndAlso ((lpelfe.elfLogFont.lfPitchAndFamily And 3) <> pitch) Then Return 1

                                                fonts.Add(lpelfe)


                                                Return 1
                                            End Function, IntPtr.Zero, 0)

            DeleteDC(hdc)
            mm.Free()

            If (e = 0) Then
                e = CInt(GetLastError)
                s = ErrorToString(e)
            End If

            Dim nf As FontInfo
            Dim ccol As New FontCollection

            For Each f In fonts
                nf = New FontInfo(f)
                ccol.Add(nf)
            Next

            ccol.Sort()
            Return ccol

        End Function


        ''' <summary>
        ''' Merges two font collections, returning a new collection object.
        ''' </summary>
        ''' <param name="col1">The first font collection.</param>
        ''' <param name="col2">The second font collection.</param>
        ''' <param name="sortProperty">Optionally specify a sort property ("Name" is the default).  If you specify a property that cannot be found, "Name" will be used.</param>
        ''' <param name="SortOrder">Optionally specify ascending or descending order.</param>
        ''' <returns></returns>
        Public Shared Function MergeCollections(col1 As FontCollection, col2 As FontCollection, Optional SortProperty As String = "Name", Optional SortOrder As SortOrder = SortOrder.Ascending) As FontCollection

            Dim col3 As New FontCollection

            For Each c In col1
                col3.Add(c)
            Next

            For Each c In col2
                col3.Add(c)
            Next

            col3.Sort(SortProperty, SortOrder)
            Return col3


        End Function

        ''' <summary>
        ''' Sort this collection by the given property and sort order.
        ''' </summary>
        ''' <param name="SortProperty">The property name to sort by.</param>
        ''' <param name="SortOrder">The sort order.</param>
        Public Sub Sort(Optional SortProperty As String = "Name", Optional SortOrder As SortOrder = SortOrder.Ascending)

            Dim pi As Reflection.PropertyInfo = Nothing

            Try
                pi = GetType(FontInfo).GetProperty(SortProperty)
            Catch ex As Exception

            End Try

            If pi Is Nothing Then
                pi = GetType(FontInfo).GetProperty(NameOf(FontInfo.Name))
            End If


            _List.Sort(New Comparison(Of FontInfo)(Function(a As FontInfo, b As FontInfo) As Integer

                                                       Dim x As Integer = 0

                                                       Dim o1 As String,
                                                           o2 As String

                                                       o1 = CStr(pi.GetValue(a))
                                                       o2 = CStr(pi.GetValue(b))

                                                       If TypeOf o1 Is String Then
                                                           x = String.Compare(o1, o2)
                                                       Else
                                                           If o1 < o2 Then
                                                               x = -1
                                                           ElseIf o2 < o1 Then
                                                               x = 1

                                                           End If
                                                       End If

                                                       If (SortOrder = SortOrder.Descending) Then x = -x

                                                       Return x

                                                   End Function))

        End Sub

        ''' <summary>
        ''' This object is not creatable.
        ''' </summary>
        Private Sub New()

        End Sub

        ''' <summary>
        ''' Initialize the master system font list.
        ''' </summary>
        Shared Sub New()
            _SystemFonts = GetFonts()
        End Sub

        ''' <summary>
        ''' Search for fonts whose names contain the specified string.
        ''' </summary>
        ''' <param name="pattern">String to look for.</param>
        ''' <param name="caseSensitive">Specifies whether the search is case-sensitive.</param>
        ''' <returns></returns>
        Public Function Search(pattern As String, Optional caseSensitive As Boolean = False, Optional searchOptions As FontSearchOptions = FontSearchOptions.Contains) As FontCollection
            Dim l As New FontCollection

            Dim s As String,
                t As String

            Dim i As Integer = pattern.Length
            Dim j As Integer


            s = pattern
            If (caseSensitive = False) Then s = s.ToLower

            For Each f In Me
                t = f.elf.elfFullName
                If (caseSensitive = False) Then t = t.ToLower

                Select Case searchOptions
                    Case FontSearchOptions.Contains

                        If t.Contains(s) Then
                            l.Add(f)
                        End If

                    Case FontSearchOptions.BeginsWith

                        If t.Length >= s.Length Then
                            If t.Substring(0, i) = s Then
                                l.Add(f)
                            End If
                        End If

                    Case FontSearchOptions.EndsWith

                        If t.Length >= s.Length Then

                            j = t.Length - s.Length
                            If (t.Substring(j, i) = s) Then
                                l.Add(f)
                            End If

                        End If


                End Select

            Next

            If l.Count = 0 Then Return Nothing
            Return l

        End Function

        ''' <summary>
        ''' Returns the FontInfo object at the specified index.
        ''' </summary>
        ''' <param name="index">Index</param>
        ''' <returns></returns>
        Default Public ReadOnly Property Item(index As Integer) As FontInfo
            Get
                Return _List(index)
            End Get
        End Property

        ''' <summary>
        ''' Returns the number of items in this collection.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Count As Integer Implements ICollection(Of FontInfo).Count
            Get
                Return _List.Count
            End Get
        End Property

        Private ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of FontInfo).IsReadOnly
            Get
                Return True
            End Get
        End Property

        Private Sub Add(item As FontInfo) Implements ICollection(Of FontInfo).Add
            _List.Add(item)
        End Sub

        Private Sub Clear() Implements ICollection(Of FontInfo).Clear
            _List.Clear()
        End Sub

        ''' <summary>
        ''' Indicates whether this collection contains the specified FontInfo object.
        ''' </summary>
        ''' <param name="item"></param>
        ''' <returns></returns>
        Public Function Contains(item As FontInfo) As Boolean Implements ICollection(Of FontInfo).Contains
            Return _List.Contains(item)
        End Function

        ''' <summary>
        ''' Copies the entire collection into a compatible 1-dimensional array
        ''' </summary>
        ''' <param name="array">The array into which to copy the data.</param>
        ''' <param name="arrayIndex">The zero-based array index at which copying begins.</param>
        Public Sub CopyTo(array() As FontInfo, arrayIndex As Integer) Implements ICollection(Of FontInfo).CopyTo
            _List.CopyTo(array, arrayIndex)
        End Sub

        Private Function Remove(item As FontInfo) As Boolean Implements ICollection(Of FontInfo).Remove
            Return _List.Remove(item)
        End Function

        Public Function GetEnumerator() As IEnumerator(Of FontInfo) Implements IEnumerable(Of FontInfo).GetEnumerator
            Return New FontEnumer(Me)
        End Function

        Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Return New FontEnumer(Me)
        End Function


        Private Class FontEnumer
            Implements IEnumerator(Of FontInfo)

            Private _obj As FontCollection
            Private _pos As Integer = -1

            Public Sub New(obj As FontCollection)
                _obj = obj
            End Sub

            Public ReadOnly Property Current As FontInfo Implements IEnumerator(Of FontInfo).Current
                Get
                    Return _obj(_pos)
                End Get
            End Property

            Private ReadOnly Property IEnumerator_Current As Object Implements IEnumerator.Current
                Get
                    Return _obj(_pos)
                End Get
            End Property

            Public Sub Reset() Implements IEnumerator.Reset
                _pos = -1
            End Sub

            Public Function MoveNext() As Boolean Implements IEnumerator.MoveNext
                _pos += 1
                If (_pos >= _obj.Count) Then Return False
                Return True
            End Function

#Region "IDisposable Support"
            Private disposedValue As Boolean ' To detect redundant calls

            ' IDisposable
            Protected Overridable Sub Dispose(disposing As Boolean)
                disposedValue = True
            End Sub

            Public Sub Dispose() Implements IDisposable.Dispose
                Dispose(True)
            End Sub
#End Region
        End Class

    End Class


#End Region

End Namespace