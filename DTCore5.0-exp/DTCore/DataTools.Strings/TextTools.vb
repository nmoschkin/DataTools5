'' ************************************************* ''
'' DataTools Visual Basic Utility Library 
''
'' Module: Text Processing Utilities
'' 
'' Copyright (C) 2011-2020 Nathan Moschkin
'' All Rights Reserved
''
'' Licensed Under the Microsoft Public License   
'' ************************************************* ''

Option Explicit On
Option Strict Off
Option Compare Binary

Imports System.Text
Imports System.ComponentModel
Imports DataTools.Memory
Imports System.Drawing
Imports System.Reflection


Namespace Strings

    ''' <summary>
    ''' A large collection of text processing tools.
    ''' </summary>
    ''' <remarks></remarks>
    Public Module TextTools

        Public Enum CODETYPES
            ctJavascript = 0
            ctHTML = 1
            ctPHP = 2
        End Enum

        Public Const vbDblQuote = Chr(34)

        ''' <summary>
        ''' All allowed mathematical characters.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const MathChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789.()+-\=/*^%"

        ''' <summary>
        ''' All canonical letters and numbers.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const AlphaNumericChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"

        ''' <summary>
        ''' All canonical letters.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const AlphaChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz"

        ''' <summary>
        ''' All standard non-alphanumeric characters.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const NonAlphas = "-._~:/?#[]@!$&'()*+,;="

        ''' <summary>
        ''' Characters allowed in a url.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const UrlAllowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-._~:/?#[]@!$&'()*+,;="

        ''' <summary>
        ''' Match condition flags.
        ''' </summary>
        ''' <remarks></remarks>
        <Flags>
        Public Enum MatchCondition

            ''' <summary>
            ''' The match must be exact
            ''' </summary>
            ''' <remarks></remarks>
            Exact = &H0

            ''' <summary>
            ''' The match must be exact up until the length of the 
            ''' requested expression (if it is shorter than the matched index)
            ''' </summary>
            ''' <remarks></remarks>
            FirstOfSearch = &H1

            ''' <summary>
            ''' The match must be exact up until the length of the
            ''' matched index (if it is shorter than the search expression)
            ''' </summary>
            ''' <remarks></remarks>
            FirstOfMatch = &H2

            ''' <summary>
            ''' Instead of returning the index matched, return the string
            ''' </summary>
            ''' <remarks></remarks>
            ReturnString = &H4
        End Enum

        Public Enum SortOrder
            Ascending = &H0
            Descending = &H1
        End Enum


        ''' <summary>
        ''' Flags for use with NoSpace
        ''' </summary>
        ''' <remarks></remarks>
        <Flags>
        Public Enum NoSpaceModifiers
            None = 0

            BeforeToLower = 1
            BeforeToUpper = 2

            AfterToLower = 4
            AfterToUpper = 8

            FirstToLower = 16
            FirstToUpper = 32
        End Enum

        Public Structure PageLines
            Public Page As Integer
            Public Lines() As String
        End Structure

        Public Structure ATTRVAL
            Public [Name] As String
            Public Value As String
        End Structure

        Public Structure XMLTAG
            Public [Name] As String
            Public Attributes() As ATTRVAL

            Public Nodes() As XMLTAG
        End Structure

        ''' <summary>
        ''' Determine if a string consists only of numbers.
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsAllNumbers(value As String) As Boolean
            Dim ch() As Char = value
            Dim i As Integer = 0,
            c As Integer = ch.Length - 1

            For i = 0 To c
                If ch(i) < "0"c OrElse ch(i) > "9"c Then
                    IsAllNumbers = False
                    Exit Function
                End If
            Next

            IsAllNumbers = True
        End Function

        ''' <summary>
        ''' Returns true if the given object is a byte array.
        ''' </summary>
        ''' <param name="val"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsByteArray(val As Object) As Boolean
            If val Is Nothing Then Return False
            If val.GetType() = GetType(Byte()) Then Return True
            Return False
        End Function

        ''' <summary>
        ''' Compares two array of bytes for equality.
        ''' </summary>
        ''' <param name="a">First array</param>
        ''' <param name="b">Second array</param>
        ''' <param name="result">Relative disposition</param>
        ''' <returns>True if equal</returns>
        ''' <remarks></remarks>
        Public Function CompareBytes(a() As Byte, b() As Byte, Optional ByRef result As Integer = 0) As Boolean
            If a.Length <> b.Length Then Return False

            If a Is Nothing And b Is Nothing Then Return True
            If a IsNot Nothing And b Is Nothing OrElse b IsNot Nothing And a Is Nothing Then Return False

            Dim i As Integer,
            c As Integer = a.Length - 1

            For i = 0 To c
                If a(i) <> b(i) Then
                    If a(i) > b(i) Then result = 1 Else result = -1
                    Return False
                End If
            Next

            result = 0
            Return True
        End Function

        ''' <summary>
        ''' Parses a point back from Point.ToString()
        ''' </summary>
        ''' <param name="ptString"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ParsePoint(ptString As String) As System.Drawing.Point
            Dim pt As PointF = ParsePointF(ptString)
            Return New System.Drawing.Point(Fix(pt.X), Fix(pt.Y))
        End Function

        ''' <summary>
        ''' Parses a point back from PointF.ToString()
        ''' </summary>
        ''' <param name="ptString"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ParsePointF(ptString As String) As System.Drawing.PointF
            Dim s As String = TextBetween(ptString, "{", "}")
            Dim p() As String,
            v() As String

            Dim x As Single,
            y As Single

            p = BatchParse(s, ",")
            v = BatchParse(p(0), "=")
            x = Val(Trim(v(1)))

            v = BatchParse(p(1), "=")
            y = Val(Trim(v(1)))

            Return New System.Drawing.PointF(x, y)

        End Function

        ''' <summary>
        ''' Removes all spaces from a string using default modifiers.
        ''' </summary>
        ''' <param name="subject"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function NoSpace(subject As String) As String
            Return StripChars(subject, " " & vbTab)
        End Function

        ''' <summary>
        ''' Remove all spaces from a string and alters the output results according to NoSpaceModifiers
        ''' </summary>
        ''' <param name="subject">String to alter.</param>
        ''' <param name="modifiers">Modifiers.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function NoSpace(subject As String, modifiers As NoSpaceModifiers) As String
            Dim ch() As Char,
            i As Integer,
            j As Integer = subject.Length - 1

            Dim e As Integer = 0
            Dim exclusions As String = " " & vbTab
            Dim ws As Boolean = False

            ReDim ch(j)

            For i = 0 To j
                If (i = 0) AndAlso (modifiers And (NoSpaceModifiers.FirstToLower Or NoSpaceModifiers.FirstToUpper)) <> 0 AndAlso
                 (exclusions.IndexOf(subject.Chars(i)) = -1) Then

                    Select Case modifiers
                        Case NoSpaceModifiers.FirstToUpper
                            ch(e) = Char.ToUpper(subject.Chars(i))

                        Case NoSpaceModifiers.FirstToLower
                            ch(e) = Char.ToLower(subject.Chars(i))

                    End Select

                End If

                If exclusions.IndexOf(subject.Chars(i)) <> -1 Then
                    If modifiers Then
                        If i > 0 Then
                            If modifiers And NoSpaceModifiers.BeforeToLower Then
                                ch(e - 1) = Char.ToLower(ch(e - 1))
                            ElseIf modifiers And NoSpaceModifiers.BeforeToUpper Then
                                ch(e - 1) = Char.ToUpper(ch(e - 1))
                            End If
                        End If
                    End If
                    ws = True
                ElseIf ws Then
                    If modifiers Then
                        If modifiers And NoSpaceModifiers.AfterToLower Then
                            ch(e) = Char.ToLower(subject.Chars(i))
                        ElseIf modifiers And NoSpaceModifiers.AfterToUpper Then
                            ch(e) = Char.ToUpper(subject.Chars(i))
                        Else
                            ch(e) = subject.Chars(i)
                            e += 1
                        End If
                    End If
                    ws = False
                Else
                    ch(e) = subject.Chars(i)
                    e += 1
                End If

            Next

            Return ch

        End Function

        ''' <summary>
        ''' Counts occurrences of 'character'
        ''' </summary>
        ''' <param name="subject">The string to count.</param>
        ''' <param name="character">The character to count.</param>
        ''' <returns>The number of occurrences of 'character' in 'subject'</returns>
        ''' <remarks></remarks>
        Public Function CountChar(subject As String, character As Char) As Integer
            Dim ch() As Char = subject
            Dim c As Integer = ch.Length - 1
            Dim d As Integer = 0

            For i = 0 To c
                If (ch(i) = character) Then d += 1
            Next

            Return d
        End Function

        ''' <summary>
        ''' Exclude a set of characters from a string.
        ''' </summary>
        ''' <param name="subject">The text to search.</param>
        ''' <param name="exclusions">The characters to remove.</param>
        ''' <returns>Processed text.</returns>
        ''' <remarks></remarks>
        Public Function StripChars(subject As String, exclusions As String) As String

        <ThreadStatic>
        Static ch As New StringBuilder

            Dim i As Integer,
            j As Integer = subject.Length - 1

            Dim cha() As Char = subject

            ch.Clear()

            For i = 0 To j
                If (exclusions.IndexOf(cha(i)) = -1) Then _
                ch.Append(subject.Substring(i, 1))
            Next

            Return ch.ToString
        End Function

        ''' <summary>
        ''' Removes surrounding quotes from a single quoted expression.
        ''' </summary>
        ''' <param name="v"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Unquote(v As String) As String

            Try
                Dim s As String = v.Trim

                If s.Length = 0 Then Return ""

                If s.IndexOf("""") = 0 Then
                    s = s.Substring(1)
                End If

                If s.Length = 0 Then Return ""

                If s.LastIndexOf("""") = s.Length - 1 Then
                    If s.Length = 1 Then Return ""
                    s = s.Substring(0, s.Length - 1)
                End If

                Return s

            Catch ex As Exception
                Return v
            End Try

        End Function

        ''' <summary>
        ''' Finds and returns the first occurrence of text between startString and stopString
        ''' </summary>
        ''' <param name="subject">The text to process.</param>
        ''' <param name="startString">The starting string to scan.</param>
        ''' <param name="stopString">The ending string to scan.</param>
        ''' <param name="startIndex">The index at which to start scanning.</param>
        ''' <returns>The first occurrence of text between two seperator strings.</returns>
        ''' <remarks></remarks>
        Public Function TextBetween(subject As String, startString As String, stopString As String, Optional ByRef startIndex As Integer = -1) As String

            Dim i As Integer,
            j As Integer

            Dim s As String

            If (startIndex = -1) Then
                i = subject.IndexOf(startString)
            Else
                i = subject.IndexOf(startString, startIndex)
            End If

            If i = -1 Then Return Nothing

            j = subject.IndexOf(stopString, i + startString.Length + 1)

            i = i + startString.Length

            If (j - i) < 1 Then Return ""
            s = subject.Substring(i, (j - i))

            If (startIndex <> -1) Then
                startIndex = j + stopString.Length
            End If

            Return s

        End Function

        ''' <summary>
        ''' Batches TextBetween calls, and separates all text before, between, and after each separator pair.
        ''' </summary>
        ''' <param name="subject">The string to scan.</param>
        ''' <param name="startString">The starting string.</param>
        ''' <param name="stopString">The stopping string.</param>
        ''' <param name="withTokens">Return tokens in array.</param>
        ''' <param name="trimStrings">Trim returned strings.</param>
        ''' <returns>Array of strings.</returns>
        Public Function BatchBetween(subject As String, startString As String, stopString As String, Optional withTokens As Boolean = False, Optional trimStrings As Boolean = True) As String()

            If String.IsNullOrEmpty(subject) Then Return Nothing
            If String.IsNullOrEmpty(startString) Then Return Nothing
            If String.IsNullOrEmpty(stopString) Then Return Nothing

            Dim st() As String = BatchParse(subject, startString, False, False, , , withTokens, False, False)
            Dim stb() As String

            Dim ls As New List(Of String)


            For i = 0 To st.Length - 1

                stb = BatchParse(st(i), stopString, False, False, , , withTokens, False, False)

                For Each s In stb
                    If trimStrings Then
                        ls.Add(s.Trim)
                    Else
                        ls.Add(s)
                    End If
                Next

            Next



            Return ls.ToArray

        End Function


        ''' <summary>
        ''' Determines if something really is a number. Supports C-style hex strings.
        ''' </summary>
        ''' <param name="subject">The subject to test.</param>
        ''' <param name="noTrim">Whether to skip tripping white space around the text.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsNumber(subject As String, Optional noTrim As Boolean = False) As Boolean

            If Not noTrim Then subject = Trim(subject)

            Dim b As Boolean = IsNumeric(subject)
            If b Then Return True

            If subject.IndexOf("0x") = 0 Then
                subject = "&H" & subject.Substring(2)
                If IsNumeric(subject) Then Return (Val(subject) <> 0)
            End If
            Return False
        End Function


        ''' <summary>
        ''' Gets a clean filename extension from a string.
        ''' </summary>
        ''' <param name="s">String to parse.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CleanExtension(s As String) As String
            Dim i As Integer = s.LastIndexOf(".")
            If i = -1 Then Return "." & s.Trim.ToLower
            Return s.Substring(i).ToLower
        End Function

        ''' <summary>
        ''' Tightens up a text string by removing extra "stuff" for parsing (possibly in a lexer).
        ''' </summary>
        ''' <param name="Input">The input to process.</param>
        ''' <param name="RemoveOperatorGaps">Specify whether to remove the gaps between equals signs and values.</param>
        ''' <param name="RemoveComments">Specifies whether to remove comments.</param>
        ''' <param name="CommentChars">Comment characters to use to discern comments.</param>
        ''' <returns>Processed text.</returns>
        ''' <remarks></remarks>
        Public Function TightenText(Input As String, Optional RemoveOperatorGaps As Boolean = True, Optional RemoveComments As Boolean = True, Optional CommentChars As String = "'") As String
            On Error Resume Next

            ' as efficiently as possible
            Dim c() As Char,
            a As Integer,
            b As Integer

            Dim i As Integer,
            j As Integer

            Dim cmt() As Char = CommentChars

            Dim ops() As Char = "="

            Dim d() As Char,
            p As Integer = 0

            Dim spc As Integer = 0

            Dim t As Byte = 0,
            f As Byte = 0

            c = Input
            b = c.Length - 1

            ReDim d(b)

            For a = 0 To b


                If (RemoveComments = True) Then
                    j = cmt.Length - 1

                    For i = 0 To j
                        If (c(a) = cmt(i)) Then Exit For
                    Next
                End If

                If (t = 0) And (c(a) = " ") Then
                    Continue For
                ElseIf (t = 0) Then
                    t = 1
                End If

                If (c(a) = " ") Then
                    spc += 1
                ElseIf (spc > 0) And (f = 0) Then
                    d(p) = " "
                    d(p + 1) = c(a)
                    p += 2
                    spc = 0
                Else
                    d(p) = c(a)
                    If (f = 1) Then
                        f = 0
                        spc = 0
                    End If
                    p += 1
                End If

                If (RemoveOperatorGaps) Then
                    j = ops.Length - 1

                    For i = 0 To j
                        If (c(a) = ops(i)) Then
                            f = 1
                            If (a > 0) Then
                                If d(p - 2) = " " Then
                                    d(p - 2) = c(a)
                                    p -= 1

                                    Exit For
                                End If
                            End If
                        End If
                    Next
                End If

            Next

            ReDim Preserve d(p - 1)
            Input = d
            Return d

        End Function

        ''' <summary>
        ''' Returns a string suitable for parsing by Val() or <see cref="FVal" />.
        ''' The default behavior processes the string exactly as the Val function looks at it, but it is customizable.
        ''' </summary>
        ''' <param name="value">String to process.</param>
        ''' <param name="justFirst">Whether to process just the first discovered word.</param>
        ''' <param name="allInOne">Whether to merge all word blocks together before processing.</param>
        ''' <param name="maxSkip">The maximum number of words to skip in search of a number.</param>
        ''' <param name="skipChars">Specific characters to ignore or step over in search of a number (default is common currency).</param>
        ''' <param name="values">Receives the string values of all discovered individual numbers based upon the selected configuration.</param>
        ''' <returns>A result ready to be parsed by a numeric parser.</returns>
        ''' <remarks></remarks>
        Public Function JustNumbers(value As String, Optional justFirst As Boolean = True, Optional allInOne As Boolean = True, Optional maxSkip As Integer = 0, Optional skipChars As String = "$£€#""", Optional ByRef values() As String = Nothing, Optional decimalChar As Char = "."c) As String
            Dim sn As Char() = value
            Dim sc() As Char

            Dim i As Integer,
            c As Integer = sn.Length - 1

            Dim e As Integer = 0

            Dim skip As Integer = -1

            Dim d As Integer = 0,
            t As Boolean = False

            Dim za As Integer = 0

            Dim firstScan As String = "&0123456789+-" & decimalChar & " "
            Dim scan = " 1234567890-+" & decimalChar & "eEHhOo"

            If decimalChar = "."c Then skipChars &= "," Else skipChars &= "."

            ReDim sc(c)

            For i = 0 To c
                If Not t Then
                    If firstScan.IndexOf(sn(i)) >= 0 Then
                        t = True
                    Else
                        If justFirst Then
                            If maxSkip > -1 AndAlso skip > maxSkip Then
                                If values IsNot Nothing Then
                                    values = {}
                                End If
                                Return vbNullString
                            Else
                                skip += 1
                            End If
                        End If
                    End If
                End If

                If t Then

                    If scan.IndexOf(sn(i)) >= 0 Then
                        If sn(i) = " "c Then
                            If justFirst AndAlso d Then
                                Exit For
                            End If

                            d += 1
                            t = False

                            If Not allInOne Then
                                sc(e) = sn(i)
                                e += 1
                            End If
                        Else
                            sc(e) = sn(i)
                            e += 1
                        End If
                    End If
                ElseIf justFirst AndAlso skipChars <> "" AndAlso skipChars.IndexOf(sn(i)) = -1 Then
                    Return ""
                End If
            Next

            If e = 0 Then
                If values IsNot Nothing Then
                    values = {}
                End If
                Return vbNullString
            End If

            If e < c Then
                ReDim Preserve sc(e)
            End If

            If values IsNot Nothing Then
                values = BatchParse(sc, " ")
            End If

            Return sc
        End Function

        ''' <summary>
        ''' A better Val() function.  Will parse hexadecimal with 0x or &amp;H markers, octal with &amp;O markers or binary digits with a 'b' marker.
        ''' </summary>
        ''' <param name="value">The string value to parse.</param>
        ''' <returns>An numeric primitive (either a Long or a Double).</returns>
        ''' <remarks></remarks>
        Public Function FVal(value As String) As Object

            Dim o As Double = 0.0#,
            so As Single = 0.0,
            n As Long = 0

            value = Trim(value)

            If value.Length < 2 Then
                If IsNumeric(value) = False Then Return Nothing
                If Double.TryParse(value, o) Then Return o
            End If

            If value.Substring(0, 2) = "&H" Or value.Substring(0, 2) = "0x" Then
                value = value.Substring(2)
                Return Long.Parse(value, Globalization.NumberStyles.AllowHexSpecifier)
            ElseIf value.Substring(value.Length - 1, 1) = "h" Then
                value = value.Substring(0, value.Length - 1)
                Return Long.Parse(value, Globalization.NumberStyles.AllowHexSpecifier)
            ElseIf value.Substring(0, 1) = "h" Then
                value = value.Substring(1)
                Return Long.Parse(value, Globalization.NumberStyles.AllowHexSpecifier)
            ElseIf value.Substring(0, 2) = "&O" Then
                Return CLng(Val(value))
            ElseIf value.ToLower.Chars(0) = "b" Then
                Dim i As Integer,
                c As Integer,
                ch() As Char,
                v As Long = 0

                ch = value.Substring(1)
                c = ch.Length - 1
                For i = 0 To c
                    If ch(i) = "1" Then
                        v += 1
                    ElseIf ch(i) <> "0" Then
                        Return v
                    End If
                    v <<= 1
                Next

                Return (v)
            ElseIf value.ToLower.Chars(value.Length - 1) = "b" Then
                Dim i As Integer,
                c As Integer,
                ch() As Char,
                v As Long = 0

                ch = value.Substring(0, value.Length - 1)
                c = ch.Length - 1
                For i = 0 To c
                    If ch(i) = "1" Then
                        v += 1
                    ElseIf ch(i) <> "0" Then
                        Return v
                    End If
                    v <<= 1
                Next

                Return (v)
            End If

            n = value.IndexOf(" ")
            If n <> -1 Then
                value = JustNumbers(value.Substring(0, n))
            End If

            If IsNumeric(value) = False Then Return Nothing

            If Double.TryParse(value, Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CurrentCulture.NumberFormat, o) Then Return CDbl(o)

            Try
                FVal = Val(value)
            Catch ex As Exception
                Return Nothing
            End Try

        End Function


        ''' <summary>
        ''' Escape text for use in a CSV file.
        ''' </summary>
        ''' <param name="s"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function TextEscapeCSV(s As String) As String
            Dim b() As Char,
            i As Integer

            If s Is Nothing Then Return ""

            Dim sb As New StringBuilder

            b = s.ToCharArray()

            For i = 0 To b.Length - 1

                Select Case b(i)

                    Case """"
                        sb.Append("""""")
                        Exit Select

                    Case vbCr
                        sb.Append(vbCr)

                    Case vbLf

                    Case Else
                        sb.Append(b(i))
                        Exit Select

                End Select
            Next

            Return sb.ToString

        End Function

        ''' <summary>
        ''' Escape text for use in a Json file.
        ''' </summary>
        ''' <param name="s"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function TextEscapeJson(s As String) As String
            Dim b() As Char,
            i As Integer

            Dim sOut As New StringBuilder

            If s Is Nothing Then Return ""

            b = s.ToCharArray()

            For i = 0 To b.Length - 1

                If i < b.Length - 1 Then
                    If b(i) = "\" AndAlso b(i + 1) = """" Then
                        sOut.Append("\""")
                        i += 1
                        Continue For
                    End If
                End If

                Select Case b(i)

                    Case "\", """", "/"
                        sOut.Append("\")

                End Select

                sOut.Append(b(i))
            Next

            Return sOut.ToString

        End Function


        Public Function GetXMLTag(a() As Char, b As Integer, Optional ByRef e As Object = Nothing) As XMLTAG

            Dim i As Integer,
            j As Integer = 0

            Dim sName As String = "",
            sAtt() As ATTRVAL = Nothing

            Dim tOut As XMLTAG = New XMLTAG

            Dim iaN As Boolean = False,
            iaK As Boolean = False,
            iaV As Boolean = False

            Dim iaQ As Boolean = False

            Dim iX As Boolean = False

            For i = b To a.Length - 1
                If (a(i) = "<") And (iaQ = False) Then
                    iaN = True
                    iX = True
                Else

                    If (a(i) = ">") And ((iaV = False) And (iaQ = False)) Then
                        iX = False
                        Exit For
                    End If

                    If (iX = True) Then
                        If (iaN = True) Then
                            If (a(i) = " ") Then
                                iaN = False
                            Else
                                sName += a(i)
                            End If
                        Else
                            If ((iaV = False) And (iaK = False)) Then
                                If (a(i) <> " ") Then
                                    iaK = True
                                    ReDim Preserve sAtt(j)
                                End If
                            ElseIf (iaK = True) And (a(i) = "=") Then
                                iaK = False
                                iaV = True
                                Continue For
                            ElseIf (iaK = True) And (a(i) = " ") Then
                                j += 1
                                iaK = False
                                Continue For

                            End If

                            If (iaV = True) Then
                                If (a(i) = vbDblQuote) Then
                                    If iaQ = True Then
                                        iaQ = False
                                        iaV = False
                                    Else
                                        iaQ = True
                                        i += 1
                                    End If
                                ElseIf (a(i) = " ") Then
                                    If (iaQ = False) Then
                                        iaV = False
                                    End If
                                End If
                            End If

                            If (a(i) = ">") Then
                                If (iaQ = False) Then
                                    iaV = False
                                    iX = False
                                    j += 1
                                    Continue For
                                End If
                            End If

                            If (iaK = True) Then
                                sAtt(j).Name += a(i)
                            ElseIf (iaV = True) Then
                                sAtt(j).Value += a(i)
                            ElseIf (iaK = False) And (iaV = False) And (a(i) <> " ") Then
                                j += 1
                            End If


                        End If

                    End If

                End If

            Next

            tOut.Name = sName
            tOut.Attributes = sAtt

            If (Not e Is Nothing) Then
                e = i - 1
            End If

            Return tOut

        End Function

        ''' <summary>
        ''' Clear all null characters from a string
        ''' </summary>
        ''' <param name="input">String to process.</param>
        ''' <returns>Processed text.</returns>
        ''' <remarks></remarks>
        Public Function RemoveNulls(input As String) As String
            Return input.Trim(ChrW(0))
        End Function

        ''' <summary>
        ''' Reduces extraneous spacing, and ensures only one space exists at any given place.
        ''' </summary>
        ''' <param name="input">The string to process.</param>
        ''' <param name="spaceChars">The characters to interpret as space characters.</param>
        ''' <param name="PreserveQuotedText">Whether to preserve multiple spaces within quoted text.</param>
        ''' <param name="quoteChar">The quote character to use for determining the location of quoted text.</param>
        ''' <param name="escapeChar">The escape character to use to recognize the escaping of quote characters.</param>
        ''' <param name="Quick">Whether to perform a quick search and replace.  If this parameter is set to true, all other optional parameters are ignored.</param>
        ''' <returns>Processed text.</returns>
        ''' <remarks></remarks>
        Public Function OneSpace(input As String, Optional spaceChars As String = " ", Optional PreserveQuotedText As Boolean = True, Optional quoteChar As Char = vbDblQuote, Optional escapeChar As Char = "\", Optional Quick As Boolean = True) As String

            Dim a As Integer,
            b As Integer

            Dim varOut As New StringBuilder,
            isP As Boolean,
            iQ As Boolean

            Dim ch() As Char = input

            If Quick Then
                If input.IndexOf("  ") = -1 Then
                    Return input
                End If

                Do Until input.IndexOf("  ") = -1
                    input = input.Replace("  ", " ")
                Loop

                Return input
            End If

            b = ch.Length - 1
            For a = 0 To b

                If (iQ = True) Then
                    varOut.Append(ch(a))
                    If (ch(a) = quoteChar) Then
                        If a > 0 Then
                            If ch(a - 1) = escapeChar Then
                                Continue For
                            End If
                        End If
                        iQ = False
                    End If
                Else
                    If (spaceChars.IndexOf(ch(a)) <> -1) And (isP = False) Then
                        isP = True
                        varOut.Append(ch(a))
                    ElseIf (spaceChars.IndexOf(ch(a)) = -1) Then
                        varOut.Append(ch(a))
                        If (isP = True) Then isP = False
                        If (ch(a) = quoteChar) And (PreserveQuotedText = True) Then
                            iQ = True
                        End If
                    End If

                End If
            Next

            OneSpace = varOut.ToString

        End Function

        ''' <summary>
        ''' Get all text within the first occurance of a specified bracket set.  Discards text outside.
        ''' </summary>
        ''' <param name="szText">String to scan.</param>
        ''' <param name="startIndex">Index in the string at which to start scanning.</param>
        ''' <param name="newIndex">Receives the index of the first character after the closing bracket.</param>
        ''' <param name="BracketPair">Bracket pair (must consist of exactly 2 characters, for other division pairs, use <see cref="TextBetween"/>.)</param>
        ''' <param name="ErrorText"></param>
        ''' <returns>The text inside the first complete bracket, excluding the outer-most pair.</returns>
        ''' <remarks></remarks>
        Public Function Bracket(szText As String, Optional ByRef startIndex As Integer = Nothing, Optional ByRef newIndex As Integer = Nothing, Optional BracketPair As String = "()", Optional ByRef ErrorText As String = Nothing) As String
            '' returns the text inside the first complete bracket, excluding the outer-most pair. newIndex is set to the first character after the closing bracket

            Dim ch() As Char = szText,
            i As Integer = 0,
            c As Integer = ch.Length - 1

            Dim sOut As String = ""
            Dim n As Integer = 0,
            bc As Integer = 0,
            ec As Integer = 0

            Dim open As String,
            close As String

            If BracketPair.Length <> 2 Then
                ErrorText = "Invalid bracket pair string. The bracket pair string must consist of exactly one open character and exactly one close character."
                Return Nothing
            End If

            open = BracketPair.Chars(0)
            close = BracketPair.Chars(1)

            Try

                newIndex = startIndex

                If szText = "" Then Return ""
                i = szText.IndexOf(open, startIndex)

                If i = -1 Then
                    '' there are no brackets, so we shall assume that entire string from startIndex to finish is a "bracket",
                    '' we'll return that text and set newIndex to the end of the line

                    If newIndex <> Nothing Then newIndex = 0
                    i = szText.IndexOf(close)

                    If i <> -1 Then
                        ErrorText = "Syntax error.  Unexpected closing bracket at column " & i + 1
                        Return Nothing
                    End If

                    newIndex = ch.Length
                    Return szText.Substring(startIndex)

                End If

                For n = i To c

                    sOut &= ch(n)

                    If (ch(n) = open) Then
                        bc += 1
                    ElseIf (ch(n) = close) Then
                        bc -= 1
                        If (bc < 0) Then
                            ErrorText = "Syntax error.  Unexpected closing bracket at column " & i + 1
                            Return Nothing
                        ElseIf bc = 0 Then
                            Exit For
                        End If
                    End If

                Next

                If bc <> 0 Then
                    ErrorText = "Syntax error.  Unmatched closing bracket at column " & i + 1
                    Return Nothing
                End If

                sOut = sOut.Substring(1, sOut.Length - 2)

                newIndex = n + 1
                startIndex = i

                Return sOut

            Catch ex As Exception
                ErrorText = ex.Message
                Return Nothing
            End Try

        End Function


        ''' <summary>
        ''' Space out operators in preparation for mathematical parsing.
        ''' </summary>
        ''' <param name="value">The text to process.</param>
        ''' <param name="Operators">The list of operator characters to use.</param>
        ''' <param name="SepChars">The list of separator characters to use (default is all white space characters in the current culture).</param>
        ''' <param name="StickyCharsLeft">Character operators that should stick to the text on their right if it is adjacent (not separated by a separator character).</param>
        ''' <param name="StickyCharsRight">Character operators that should stick to the text on their left if it is adjacent (not separated by a separator character).</param>
        ''' <param name="NoStickyChars">Characters that under no circumstances should stick to adjacent characters.</param>
        ''' <returns>Processed text.</returns>
        ''' <remarks></remarks>
        Public Function SpaceOperators(value As String, Optional Operators As String = "/\&^%*-+()[]{}", Optional SepChars As String = Nothing, Optional StickyCharsLeft As String = "", Optional StickyCharsRight As String = "+-", Optional NoStickyChars As String = "") As String
            Dim i As Integer,
            c As Integer

            Dim s As String,
            ch As Char
            Dim inq As Boolean = False

            Dim sp() As Char

            If SepChars = Nothing Then
                SepChars = ""
                For i = 0 To 255
                    ch = Chr(i)
                    If Char.IsWhiteSpace(ch) Then
                        SepChars &= ch
                    End If
                Next
            End If

            If SepChars = "" Then Return value

            sp = value
            c = sp.Length - 1
            s = ""

            If StickyCharsLeft = Nothing Then StickyCharsLeft = ""
            If StickyCharsRight = Nothing Then StickyCharsRight = ""
            If NoStickyChars = Nothing Then NoStickyChars = ""

            For i = 0 To c

                If (sp(i) = vbDblQuote) Then
                    inq = Not inq
                End If

                If Not inq AndAlso Operators.IndexOf(sp(i)) <> -1 Then

                    If (i > 0) AndAlso (Operators.IndexOf(sp(i - 1)) <> -1) AndAlso (NoStickyChars.IndexOf(sp(i)) = -1) AndAlso (NoStickyChars.IndexOf(sp(i - 1)) = -1) Then

                        If (StickyCharsRight.IndexOf(sp(i - 1)) <= -1) Then
                            s = s.Substring(0, s.Length - 1)
                        End If

                        s &= sp(i)
                    Else
                        If StickyCharsLeft.IndexOf(sp(i)) = -1 OrElse (NoStickyChars.IndexOf(sp(i)) <> -1) Then
                            s &= " "
                        End If

                        s &= sp(i)
                    End If

                    If StickyCharsRight.IndexOf(sp(i)) = -1 OrElse (NoStickyChars.IndexOf(sp(i)) <> -1) Then
                        s &= " "
                    End If

                Else
                    s &= sp(i)
                End If

            Next

            Return Trim(OneSpace(s, SepChars))

        End Function

        ''' <summary>
        ''' Returns all words in a string as an array of strings.
        ''' </summary>
        ''' <param name="Text">Text to split.</param>
        ''' <param name="SepChars">Separator characters to use.</param>
        ''' <param name="AdditionalSepChars">Any additional separator characters to use.</param>
        ''' <param name="SkipQuotes">Skip over quoted text.</param>
        ''' <param name="Unescape">Unescape quoted text.</param>
        ''' <param name="QuoteChar">Quote character to use.</param>
        ''' <param name="EscapeChar">Escape character to use.</param>
        ''' <returns>Array of strings.</returns>
        ''' <remarks></remarks>
        Public Function Words(Text As String, Optional SepChars As String = Nothing, Optional AdditionalSepChars As String = Nothing, Optional SkipQuotes As Boolean = False, Optional Unescape As Boolean = False, Optional QuoteChar As String = vbDblQuote, Optional EscapeChar As String = "\") As String()
            Dim i As Integer,
            c As Integer

            Dim n As Integer

            Dim s As String,
            ch As Char

            Dim stout() As String = Nothing,
            stwork() As String = Nothing,
            stwork2() As String = Nothing

            Dim sep() As Char

            If SepChars = Nothing Then
                SepChars = ""
                For i = 0 To 255
                    ch = Chr(i)
                    If Char.IsWhiteSpace(ch) Then
                        If ch <> vbCr AndAlso ch <> vbLf Then SepChars &= ch
                    End If
                Next
            End If

            If SepChars = "" Then Return {Text}

            sep = SepChars
            If AdditionalSepChars <> Nothing Then
                sep &= AdditionalSepChars
            End If

            ch = sep(0)

            s = Text
            s = Trim(OneSpace(s, ch, SkipQuotes))

            stout = BatchParse(s, ch, SkipQuotes, Unescape, QuoteChar, EscapeChar)
            If stout Is Nothing Then Return {s}

            c = stout.Length - 1

            If SepChars.Length = 1 Then
                Return stout
            End If

            SepChars = SepChars.Substring(1)

            For i = 0 To c

                stwork = Words(stout(i), SepChars, , SkipQuotes, Unescape, QuoteChar, EscapeChar)
                If stwork2 Is Nothing Then
                    stwork2 = stwork
                    Continue For
                End If

                n = stwork2.Length
                Array.Resize(stwork2, n + stwork.Length)
                Array.Copy(stwork, 0, stwork2, n, stwork.Length)

            Next

            Return stwork2

        End Function

        ''' <summary>
        ''' Wrap the input text by the given columns.
        ''' </summary>
        ''' <param name="szText">Text to wrap.</param>
        ''' <param name="Cols">Maximum character columns.</param>
        ''' <returns>Wrapped text.</returns>
        ''' <remarks></remarks>
        Public Function Wrap(szText As String, Optional Cols As Integer = 60) As String
            Dim st() As String,
            xTot As Integer = 0,
            sOut As String = "",
            i As Integer,
            j As Integer

            If (szText.Length < Cols) Then Return szText

            st = Words(szText)

            For i = 0 To st.Length - 1

                If (st(i).Length >= Cols) Then
                    sOut += vbCrLf
                    j = 0
                    While (j < st(i).Length)

                        Try
                            sOut += st(i).Substring(j, Cols)
                        Catch ex As Exception
                            sOut += st(i).Substring(j)
                        End Try

                        j += Cols
                        sOut += vbCrLf
                    End While
                    xTot = 0

                    Continue For

                ElseIf (xTot + st(i).Length) > Cols Then
                    sOut += vbCrLf
                    xTot = 0
                End If

                xTot += st(i).Length
                sOut += st(i)

                If (i < (st.Length - 1)) Then
                    xTot += 1
                    sOut += " "
                End If

            Next

            Return sOut
        End Function

        ''' <summary>
        ''' Returns all lines in a string.
        ''' </summary>
        ''' <param name="szText"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetLines(szText As String) As String()

            szText = szText.Replace(vbCrLf, vbLf)
            szText = szText.Replace(vbCr, vbLf)
            szText = szText.Replace(vbLf, (ChrW(0)))

            Return szText.Split(ChrW(0))

        End Function

        ''' <summary>
        ''' Return a padded hexadecimal value.
        ''' </summary>
        ''' <param name="value"></param>
        ''' <param name="width"></param>
        ''' <param name="prefix"></param>
        ''' <param name="lowercase"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function PadHex(value As Object, Optional width As Integer = 8, Optional prefix As String = "", Optional lowercase As Boolean = False) As String

            Dim s As String = Hex(value)
            If (lowercase) Then s = s.ToLower

            If (width - s.Length > 0) Then
                Return prefix + New String("0", width - s.Length) + s
            Else
                Return prefix + s
            End If

        End Function

        ''' <summary>
        ''' Returns true if the value in a hexadecimal number. Accepts &amp;H and 0x prefixes.
        ''' </summary>
        ''' <param name="hin">String to scan</param>
        ''' <param name="value">Optionally receives the parsed value.</param>
        ''' <returns>True if the string can be parsed as hex.</returns>
        ''' <remarks></remarks>
        Public Function IsHex(hin As String, Optional ByRef value As Integer = Nothing) As Boolean

            Dim b() As Char,
            i As Integer

            Dim c As Boolean = True

            b = hin.ToCharArray

            If hin.IndexOf("&H") = -1 And hin.IndexOf("0x") = -1 Then Return False

            hin = hin.Replace("&H", "")
            hin = hin.Replace("0x", "")

            For i = 0 To b.Length - 1
                Select Case b(i)

                    Case "a", "A", "b", "B", "c", "C", "d", "D", "e", "E", "f", "F", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9"
                        Exit Select

                    Case Else
                        c = False
                        Exit For

                End Select
            Next

            If (c = True) And (value <> Nothing) Then
                value = CInt("&H" + hin)
            End If

            Return c

        End Function

        ''' <summary>
        ''' Removes comments from a line of text.
        ''' </summary>
        ''' <param name="input">Text to parse.</param>
        ''' <param name="commentchar">Comment marker</param>
        ''' <returns>A string with no comments.</returns>
        ''' <remarks></remarks>
        Public Function NoComment(input As String, Optional commentchar As String = "//") As String
            Dim a As Integer,
            b As Integer

            Dim varOut As String = "",
            isP As Boolean,
            iQ As Boolean

            On Error Resume Next

            b = input.Length - 1
            For a = 0 To b

                If (iQ = True) Then
                    varOut += input.Chars(a)
                    If (input.Chars(a) = vbDblQuote) Then
                        iQ = False
                    End If
                Else
                    If (a < (b - commentchar.Length)) Then
                        If (input.Substring(a, commentchar.Length) = commentchar) Then
                            Exit For
                        End If
                    End If
                    varOut += input.Chars(a)

                    If (isP = True) Then isP = False
                    If (input.Chars(a) = vbDblQuote) Then
                        iQ = True
                    End If

                End If
            Next

            Return varOut
        End Function

        Public Function FieldToProp(fieldname As String) As String
            Return fieldname.Substring(1)
        End Function

        Public Function PropToField(propname As String) As String
            Return "_" & propname
        End Function

        ''' <summary>
        ''' Space out a camel-cased string.
        ''' </summary>
        ''' <param name="value">String to separate.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SeparateCamel(value As String) As String
            Dim ch() As Char = value

            Dim i As Integer,
            c As Integer

            Dim sb As New StringBuilder

            c = ch.Length - 1
            For i = 0 To c

                If ch(i).ToString.ToUpper = ch(i).ToString AndAlso i > 0 AndAlso (IsNumber(ch(i)) = False) Then

                    sb.Append(" ")
                End If
                sb.Append(ch(i))

            Next
            Return sb.ToString
        End Function

        ''' <summary>
        ''' Convert spaced or underscored lines to camel case. This function is an alias for TitleCase.
        ''' </summary>
        ''' <param name="input">The input text</param>
        ''' <param name="stripSpaces">Strip spaces and present a camel-case version of the string.</param>
        ''' <param name="noDecaps">Do not decapitalize already capitalized characters if they occur alone.</param>
        ''' <returns>A newly formatted string</returns>
        ''' <remarks></remarks>
        Public Function CamelCase(input As String, Optional stripSpaces As Boolean = False, Optional noDecaps As Boolean = False) As String
            Return TitleCase(input, stripSpaces, noDecaps)
        End Function

        ''' <summary>
        ''' Convert spaced or underscored lines to camel case.
        ''' </summary>
        ''' <param name="input">The input text</param>
        ''' <param name="stripSpaces">Strip spaces and present a camel-case version of the string.</param>
        ''' <param name="noDecaps">Do not decapitalize already capitalized characters if they occur alone.</param>
        ''' <returns>A newly formatted string.</returns>
        ''' <remarks></remarks>
        Public Function TitleCase(input As String, Optional stripSpaces As Boolean = False, Optional noDecaps As Boolean = False) As String
            Dim a As Integer,
            b As Integer

            Dim varOut As String = "",
            isP As Boolean,
            iQ As Boolean

            Dim dec As Boolean = False

            If input = Nothing Then Return ""
            input = SearchReplace(input, "_", " ")

            b = input.Length - 1

            For a = 0 To b
                If (iQ = True) Then
                    varOut += input.Chars(a)
                    If (input.Chars(a) = vbDblQuote) Then
                        iQ = False
                    End If
                Else
                    If ((input.Chars(a) = Chr(32)) OrElse (input.Chars(a) = "-")) AndAlso (isP = False) Then

                        isP = True
                        If input.Chars(a) = "-" Then varOut &= "-" Else varOut &= " "

                    ElseIf (input.Chars(a) <> Chr(32)) AndAlso (input.Chars(a) <> "-") Then

                        If (CStr(input.Chars(a)) = CStr(input.Chars(a)).ToUpper) AndAlso (noDecaps) AndAlso
                        dec = False Then
                            isP = True
                            dec = True
                        ElseIf dec = True Then
                            dec = False
                        End If

                        If (a = 0) Or (isP = True) Then
                            varOut += input.Chars(a).ToString.ToUpper()
                        ElseIf (isP = False) Then
                            varOut += input.Chars(a).ToString.ToLower()
                        End If

                        If (isP = True) Then isP = False
                        If (input.Chars(a) = vbDblQuote) Then
                            iQ = True
                        End If
                    End If

                End If
            Next

            If stripSpaces Then Return SearchReplace(varOut, " ", "") Else Return varOut
        End Function

        ''' <summary>
        ''' Concats all strings in a string array into one string.
        ''' </summary>
        ''' <param name="Text">Array to combine.</param>
        ''' <returns>A string.</returns>
        ''' <remarks></remarks>
        Public Function Stream(Text() As String) As String
            Dim i As Long,
            b As Long

            Dim sb As New StringBuilder
            On Error Resume Next
            Stream = ""

            i = -1&
            i = UBound(Text)
            For b = 0 To i
                If sb.Length <> 0 Then sb.Append(vbCrLf)
                sb.Append(Text(b))
            Next

            Stream = sb.ToString

        End Function


        ''' <summary>
        ''' Filters text using odd pairs of characters, when the beginning and end bounds have different constituents.
        ''' </summary>
        ''' <param name="Text">The text to filter.</param>
        ''' <param name="FilterPair">Exactly 2 characters that represent the pair to filter.</param>
        ''' <param name="Escape">Escape character to use.</param>
        ''' <param name="FirstIsFilter">Receives a value indicating that the first character of the input text was an opening filter character.</param>
        ''' <returns>Filtered text.</returns>
        ''' <remarks></remarks>
        Public Function Filter(Text As String, Optional FilterPair As String = vbDblQuote, Optional Escape As String = "\", Optional ByRef FirstIsFilter As Boolean = Nothing, Optional OnlyBetween As Boolean = False) As String()
            Dim lnOut() As String = Nothing

            Dim i As Integer,
            j As Integer,
            l As Integer,
            m As Integer,
            n As Integer

            Dim c() As Char,
            fp() As Char,
            e As Boolean

            On Error Resume Next

            If (FilterPair.Length = 0) Then FilterPair = vbDblQuote
            If (FilterPair.Length = 1) Then
                FilterPair = FilterPair + FilterPair
            End If

            fp = FilterPair

            c = Text
            j = -1
            j = UBound(c)
            m = -1

            For i = 0 To j
                If (e = False) Then
                    If (c(i) = fp(0)) Then
                        If (m = -1) Then
                            FirstIsFilter = True
                        End If
                        m += 1
                        e = True
                        ReDim Preserve lnOut(m)
                        lnOut(m) = c(i)
                    Else
                        If (m = -1) Then
                            FirstIsFilter = False
                            m += 1
                            ReDim Preserve lnOut(m)
                        End If

                        lnOut(m) += c(i)
                    End If
                Else
                    If (c(i) = fp(1)) Then
                        If (i > 0) Then
                            If (Escape <> "") And (Escape <> Nothing) Then
                                If (c(i - 1) = Escape.Chars(0)) Then
                                    If (lnOut(m).Length > 1) Then
                                        lnOut(m) = lnOut(m).Substring(0, lnOut(m).Length - 1)
                                    End If
                                End If
                            End If
                        End If

                        lnOut(m) += c(i)

                        m += 1
                        e = False
                        ReDim Preserve lnOut(m)
                    Else
                        lnOut(m) += c(i)

                    End If

                End If
            Next

            Filter = lnOut

        End Function

        ''' <summary>
        ''' Replaces all instances of the Search string with the Replace string.
        ''' </summary>
        ''' <param name="InputStr">The string in which to search.</param>
        ''' <param name="Search">The search string.</param>
        ''' <param name="Replace">The replacement string.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SearchReplace(InputStr As String, Search As String, Replace As String) As String

            Dim sOut As String
            Dim e As Integer = 0
            sOut = InputStr
            If sOut Is Nothing OrElse sOut.Length = 0 Then Return ""

            Do Until e = -1
                e = sOut.IndexOf(Search)
                If e >= 0 Then sOut = sOut.Replace(Search, Replace) Else Exit Do
            Loop

            SearchReplace = sOut

        End Function

        ''' <summary>
        ''' Returns the string in double quotes.
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function DblQuote(value As String) As String
            Return vbDblQuote & value & vbDblQuote
        End Function

        ''' <summary>
        ''' Removes all blank lines from a string.
        ''' </summary>
        ''' <param name="i"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function NoNullLines(i As String) As String
            Dim st() As String,
            l As Integer

            Dim sOut As New StringBuilder

            i = SearchReplace(i, vbCrLf, vbCr)
            i = SearchReplace(i, vbLf, vbCr)

            st = BatchParse(i, vbCr)

            For l = 0 To st.Length - 1
                If (st(l) <> "") Then
                    If (sOut.Length <> 0) Then sOut.Append(vbCrLf)
                    sOut.Append(st(l))
                End If

            Next

            Return sOut.ToString

        End Function

        ''' <summary>
        ''' Returns 'true' if the string is in all capitals.
        ''' </summary>
        ''' <param name="text"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsCaps(text As String) As Boolean
            Return text = text.ToUpper
        End Function

        Public Structure HTMLTAG

            Public [Name] As String

            Public Attributes() As String
            Public Style As String

        End Structure

        ''' <summary>
        ''' Trims internal white-space characters inside of a string to a single space.
        ''' </summary>
        ''' <param name="val">String to trim.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function TrimWSpace(val As String) As String

            Dim i As Long,
            j As Long

            Dim sStr As New StringBuilder,
            sp As Boolean

            j = val.Length - 1

            For i = 0 To j

                If val.Chars(i) = " " Then
                    If (sp = False) Then
                        sp = True
                    End If
                Else
                    If (sp = True) Then
                        sStr.Append(" ")
                    End If

                    sStr.Append(val.Chars(i))
                    sp = False
                End If

            Next i

            TrimWSpace = sStr.ToString

        End Function

        ''' <summary>
        ''' String Token Function (works like in C/C++).  Note: not thread-safe when used with SyncTok.  Enclose in a class to make thread-safe.
        ''' </summary>
        ''' <param name="ScanString">The string to scan.</param>
        ''' <param name="Token">The token/separator to use.</param>
        ''' <param name="SyncTok">Whether to use synchronized static storage (for use with BatchParse).</param>
        ''' <param name="SkipQuotes">Whether to skip over quoted text.</param>
        ''' <param name="QuoteChar">The quote character to use.</param>
        ''' <param name="EscapeChar">The character used to escape quotation marks.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function StrTok(Optional ScanString As String = "", Optional Token As String = "", Optional SyncTok As Boolean = False, Optional SkipQuotes As Boolean = False, Optional QuoteChar As String = vbDblQuote, Optional EscapeChar As String = "\") As String
            Static Stored As String = Nothing

            Dim i As Long,
            l As Long,
            vCh As String,
            OutStr As String = "",
            sLen As Long

            Dim inQ As Boolean,
            skipQ As Boolean,
            qChar As String,
            eChar As String

            On Error Resume Next

            StrTok = ""

            If (QuoteChar = Nothing) Then qChar = vbDblQuote Else qChar = QuoteChar
            If (EscapeChar = Nothing) Then eChar = "\" Else eChar = EscapeChar

            If (SyncTok = True) Then
                If (ScanString = "") Then
                    StrTok = Stored
                Else
                    Stored = ScanString
                End If

                Exit Function
            End If

            If (Token = "") Then Return Nothing

            If ScanString <> vbNullString Then
                Stored = ScanString
            End If

            If Stored = vbNullString Then Return Nothing

            If (Stored.Length >= Token.Length) Then
                If Stored.Substring(0, Token.Length - 1) = Token Then
                    Stored = Stored.Substring(Token.Length - 1)
                    StrTok = ""
                    Exit Function
                End If
            End If

            skipQ = SkipQuotes

            sLen = Stored.Length - 1

            For i = 0 To sLen

                vCh = Stored.Substring(i, Len(Token))
                If (vCh = Token) And ((inQ = False) Or (skipQ = False)) Then

                    If ((i + Len(Token)) <= sLen) Then
                        StrTok = OutStr
                        Stored = Stored.Substring(i + Len(Token))
                        Exit Function
                    Else
                        Exit For
                    End If

                Else
                    vCh = Stored.Chars(i)
                    OutStr += vCh

                    If (qChar.IndexOf(vCh) > -1) And (skipQ = True) Then
                        inQ = (Not inQ)

                        If (i > 0) Then
                            vCh = Stored.Chars(i - 1)
                            If (vCh = eChar) Then
                                inQ = (Not inQ)
                            End If
                        End If
                    End If
                End If
            Next i

            Stored = Nothing
            Return OutStr

        End Function


        ''' <summary>
        ''' Function to retrieve a quote from a string of data. 
        ''' The quote character must be: exactly one before, exactly at, or anywhere after the location specified by 'Pos'.  
        ''' Text outside of the first quoted string is discarded.
        ''' </summary>
        ''' <param name="StrData">The string to scan.</param>
        ''' <param name="Pos">The position to begin scanning.</param>
        ''' <param name="QuoteChar">The quote character to use.</param>
        ''' <param name="EscapeChar">The escape character to use.</param>
        ''' <param name="WithQuotes">Return the string in quotes.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function QuoteFromHere(StrData As String, Pos As Long, Optional QuoteChar As String = vbDblQuote, Optional EscapeChar As String = "\", Optional WithQuotes As Boolean = False) As String

            Dim i As Long,
            l As Long,
            vCh As String,
            OutStr As New StringBuilder

            Dim inQ As Boolean,
            qChar As String,
            eChar As String
            Dim qch As Char

            If (QuoteChar = "") Then qChar = vbDblQuote Else qChar = QuoteChar
            eChar = EscapeChar

            qch = qChar.Chars(0)

            If (eChar = "") Then eChar = "\"

            If (Pos > 0) Then l = (Pos - 1) Else l = Pos
            For Each ch As Char In QuoteChar
                l = StrData.IndexOf(ch)
                If l > -1 Then Exit For
            Next

            If (l = -1) Then Return Nothing

            For i = l To Len(StrData)

                vCh = StrData.Chars(i)

                If (qChar.IndexOf(vCh) > -1) Then
                    inQ = (Not inQ)
                    qch = vCh

                    If (i > 0) Then
                        If (i > 1) Then
                            vCh = StrData.Substring(i - 2, 2)
                        Else
                            vCh = StrData.Chars(i - 1)
                        End If

                        If ((vCh.Substring(1) = eChar) And (vCh.IndexOf(eChar + eChar) = -1)) Then
                            OutStr.Append(qch)
                            inQ = (Not inQ)
                        End If
                    End If

                    If (inQ = False) Then Exit For

                ElseIf (vCh = eChar) Then

                    If (i > 0) Then
                        vCh = StrData.Chars(i - 1)
                        If (vCh = eChar) Then
                            OutStr.Append(eChar)
                        End If
                    End If

                ElseIf (vCh <> eChar) Then
                    OutStr.Append(vCh)

                End If

            Next i

            If (WithQuotes = True) Then
                QuoteFromHere = (qch + OutStr.ToString + qch)
            Else
                QuoteFromHere = OutStr.ToString
            End If

        End Function

        ''' <summary>
        ''' A new implementation of BatchParse designed to be much faster (and thread-safe)
        ''' See <see cref="BatchParse" /> for more information.
        ''' </summary>
        ''' <param name="Scan">String to scan</param>
        ''' <param name="Separator">Separator string</param>
        ''' <param name="SkipQuote">Whether to ignore separator strings within the quoted blocks.</param>
        ''' <param name="Unescape">Whether to unescape quotes.</param>
        ''' <param name="QuoteChar">Quote character to use.</param>
        ''' <param name="EscapeChar">Escape character to use.</param>
        ''' <param name="WithToken">Include the token in the return array.</param>
        ''' <param name="WithTokenIn">Attach the token to the beginning of every string separated by a token (except for string 0).  Requires WithToken to also be set to True.</param>
        ''' <param name="Unquote">Unquote quoted strings.</param>
        ''' <returns>An array of strings.</returns>
        ''' <remarks></remarks>
        Public Function BatchParse(Scan As String, Separator As String, Optional SkipQuote As Boolean = False, Optional Unescape As Boolean = False, Optional QuoteChar As Char = """"c, Optional EscapeChar As Char = "\"c, Optional WithToken As Boolean = False, Optional WithTokenIn As Boolean = False, Optional Unquote As Boolean = False) As String()

            '' abspos = absolute position
            '' seppos = position if within a potential separator sequence
            '' sepubound = highest possible index within the seperator sequence
            '' fieldpos = character position relative to the field that is currently being parsed
            '' saveidx = saved start index within a separator scan
            '' outubound = the current upper bound of the output string array
            '' inq = inside quotation boundary

            '' chOut = character buffer
            '' sOut = output string array
            '' chrs = input buffer converted to a Char array
            '' sep = input separator string converted to a Char array

            '' Now, follow the bouncing ball. :-)  

            Dim abspos As Integer,
            seppos As Integer

            Dim sepubound As Integer,
            fieldpos As Integer

            Dim sOut() As String = Nothing,
            ubound As Integer

            Dim chOut As New StringBuilder,
            saveidx As Integer = 0,
            outubound As Integer = 0

            Dim inq As Boolean = False

            Dim chrs() As Char = Scan,
            sep() As Char = Separator

            ubound = chrs.Length - 1
            sepubound = sep.Length - 1

            seppos = 0
            chOut.Capacity = chrs.Length

            For abspos = 0 To ubound
                If (SkipQuote) AndAlso (chrs(abspos) = QuoteChar) Then

                    '' meticulously check the next character for quotes, escapes, and quote-escape-quotes.
                    If (Not inq) AndAlso (abspos < ubound) AndAlso (chrs(abspos) = QuoteChar) AndAlso (chrs(abspos + 1) = QuoteChar) Then
                        abspos += 1

                        '' check to see if the first character is also an escaped quote charter.
                        If (abspos < ubound) AndAlso (EscapeChar = QuoteChar) Then

                            If chrs(abspos + 1) = QuoteChar Then
                                '' it is!  three quotes in a row!
                                '' Okay, we can actually continue with this string, and we'll proceed down the code.
                                inq = True
                            Else
                                '' nope, empty string ... Next!
                                Continue For
                            End If
                        Else
                            '' nope, empty string ... Next!
                            Continue For
                        End If
                    End If

                    If (inq) AndAlso (abspos < ubound) AndAlso (chrs(abspos) = EscapeChar) AndAlso (chrs(abspos + 1) = QuoteChar) Then
                        If Unescape Then
                            '' we just want the escaped character, we don't want the escape character.
                            abspos += 1
                            chOut.Append(chrs(abspos))
                            fieldpos += 1
                        Else
                            '' we want the escape character in addition to the escaped character.
                            chOut.Append(chrs(abspos))
                            fieldpos += 1

                            abspos += 1

                            chOut.Append(chrs(abspos))
                            fieldpos += 1

                            '' don't advance abspos a second time, it gets advanced with the For ... Next
                        End If

                        Continue For
                    End If

                    If (inq) AndAlso (chrs(abspos) = QuoteChar) Then
                        inq = False
                        If Unquote Then
                            Continue For
                        End If
                    ElseIf inq = False Then
                        inq = True
                        If Unquote Then
                            Continue For
                        End If
                    End If
                End If

                If (Not inq) AndAlso (chrs(abspos) = sep(seppos)) Then

                    ' Save the starting index.

                    ' In the case of multi-character separators, we need this to remember where we left off if the 
                    ' characters that are found form an incomplete separator.

                    ' For example, say we find a comma, where the separator is exactly a comma and a space... 
                    ' we have to abort that and return us to our regularly scheduled programming ... 

                    ' Conversely, if the separator is exactly one character, then the 'saveidx' variable is not used, at all.
                    saveidx = abspos

                    For seppos = 0 To sepubound
                        If abspos > ubound Then Exit For

                        '' If we succeed to Exit For, here, we'll have to subtract 1 from the abspos, later... and we do.
                        If chrs(abspos) = sep(seppos) Then abspos += 1 Else Exit For
                    Next

                    If seppos = sepubound + 1 Then

                        '' the grand expansion.
                        ReDim Preserve sOut(outubound)

                        '' if you need the tokens, themselves, attached to the output string array...
                        If WithToken AndAlso WithTokenIn Then
                            chOut.Append(sep)
                            fieldpos += sep.Length
                            'For Each cs As Char In sep
                            '    chOut.Append(cs)
                            '    fieldpos += 1
                            'Next
                        End If

                        '' we've got a new field, and we're going to store it.
                        sOut(outubound) = chOut.ToString
                        chOut.Clear()

                        '' reset the field counter to zero
                        fieldpos = 0

                        '' increment the outbound array ubound
                        outubound += 1

                        '' If you need the tokens, themselves, intermingled with the output string array...
                        If WithToken AndAlso Not WithTokenIn Then
                            ReDim Preserve sOut(outubound)
                            sOut(outubound) = Separator
                            outubound += 1
                        End If

                        '' Back-track from the overrun from earlier so that the For ... Next loop can advance the counter, and pick it up for the next field.
                        abspos -= 1

                    Else
                        '' ho-hum character in a current field ... Next!
                        chOut.Append(chrs(saveidx))
                        fieldpos += 1
                        abspos = saveidx
                    End If

                    seppos = 0
                Else
                    '' ho-hum character in a current field ... Next!
                    chOut.Append(chrs(abspos))
                    fieldpos += 1
                End If

            Next

            '' wrap up the final field
            If fieldpos Then
                ReDim Preserve sOut(outubound)
                sOut(outubound) = chOut.ToString
                chOut.Clear()
            End If

            '' go home
            Return sOut
        End Function

        ''' <summary>
        ''' Link an array of strings together, separated by the specified separator.
        ''' </summary>
        ''' <param name="input"></param>
        ''' <param name="separator"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function BatchLink(input() As String, separator As String) As String
            Dim sb As New StringBuilder
            Dim i As Integer = 0,
            c As Integer = input.Count - 1

            For i = 0 To c
                If i > 0 Then sb.Append(separator)
                sb.Append(input(i))
            Next

            Return sb.ToString
        End Function


        ''' <summary>
        ''' Friendly size/speed kilo type.
        ''' </summary>
        ''' <remarks></remarks>
        Public Enum FriendlyKBType As ULong

            ''' <summary>
            ''' The measurement is automatically determined.
            ''' </summary>
            ''' <remarks></remarks>
            Auto = 0

            ''' <summary>
            ''' A kilobyte is 1024 bytes.
            ''' </summary>
            ''' <remarks></remarks>
            Kilo1024 = 1024

            ''' <summary>
            ''' A kilobyte is 1000 bytes.
            ''' </summary>
            ''' <remarks></remarks>
            Kilo1000 = 1000
        End Enum

        ''' <summary>
        ''' Prints a number value as a friendly byte size in TiB, GiB, MiB, KiB or B.
        ''' </summary>
        ''' <param name="size">The size to format.</param>
        ''' <param name="format">Optional numeric format for the resulting value.</param>
        ''' <param name="factor">The 1K measuring unit.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function PrintFriendlySize(size As ULong, Optional format As String = Nothing, Optional factor As FriendlyKBType = FriendlyKBType.Kilo1024, Optional printKi As Boolean = True) As String
            Dim fs As Double,
            nom As String

            Dim ki As String

            If (factor = FriendlyKBType.Auto) Then
                If size Mod CLng(FriendlyKBType.Kilo1024) = 0 Then
                    factor = FriendlyKBType.Kilo1024
                Else
                    factor = FriendlyKBType.Kilo1000
                End If
            End If

            If factor = FriendlyKBType.Kilo1024 Then
                If printKi Then
                    ki = "iB"
                Else
                    ki = "B"
                End If
            Else
                ki = "B"
            End If

            If (size >= (factor * factor * factor * factor * factor * factor)) Then
                fs = (size / (factor * factor * factor * factor * factor * factor))
                nom = "E" & ki
            ElseIf (size >= (factor * factor * factor * factor * factor)) Then
                fs = (size / (factor * factor * factor * factor * factor))
                nom = "P" & ki
            ElseIf (size >= (factor * factor * factor * factor)) Then
                fs = (size / (factor * factor * factor * factor))
                nom = "T" & ki
            ElseIf (size >= (factor * factor * factor)) Then
                fs = (size / (factor * factor * factor))
                nom = "G" & ki
            ElseIf (size >= (factor * factor)) Then
                fs = (size / (factor * factor))
                nom = "M" & ki
            ElseIf (size >= (factor)) Then
                fs = (size / (factor))
                nom = "K" & ki
            Else
                fs = size
                nom = "B"
            End If

            If format IsNot Nothing Then
                Return Math.Round(fs, 2).ToString(format) & " " & nom
            Else
                Return Math.Round(fs, 2) & " " & nom
            End If

        End Function

        ''' <summary>
        ''' Prints a number value as a friendly byte speed in TiB, GiB, MiB, KiB or B.
        ''' </summary>
        ''' <param name="speed">The speed to format.</param>
        ''' <param name="format">Optional numeric format for the resulting value.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function PrintFriendlySpeed(speed As ULong, Optional format As String = Nothing, Optional factor As FriendlyKBType = FriendlyKBType.Auto, Optional asBits As Boolean = True) As String
            Dim fs As Double,
            nom As String

            If (factor = FriendlyKBType.Auto) Then
                If speed Mod CLng(FriendlyKBType.Kilo1000) = 0 Then
                    factor = FriendlyKBType.Kilo1000
                Else
                    factor = FriendlyKBType.Kilo1024
                End If
            End If

            Dim ki As String = If(asBits, "b/s", "B/s")

            If (speed >= (factor * factor * factor * factor * factor * factor)) Then
                fs = (speed / (factor * factor * factor * factor * factor * factor))
                nom = "E" & ki ''wow
            ElseIf (speed >= (factor * factor * factor * factor * factor)) Then
                fs = (speed / (factor * factor * factor * factor * factor))
                nom = "P" & ki ''wow
            ElseIf (speed >= (factor * factor * factor * factor)) Then
                fs = (speed / (factor * factor * factor * factor))
                nom = "T" & ki ''wow
            ElseIf (speed >= (factor * factor * factor)) Then
                fs = (speed / (factor * factor * factor))
                nom = "G" & ki '' still wow
            ElseIf (speed >= (factor * factor)) Then
                fs = (speed / (factor * factor))
                nom = "M" & ki '' okay
            ElseIf (speed >= (factor)) Then
                fs = (speed / (factor))
                nom = "K" & ki '' fine.
            Else
                fs = speed
                nom = "B/s" '' wow.
            End If

            If format IsNot Nothing Then
                Return Math.Round(fs, 2).ToString(format) & " " & nom
            Else
                Return Math.Round(fs, 2) & " " & nom
            End If

        End Function

        ''' <summary>
        ''' Encode a string for passing in a URL.
        ''' </summary>
        ''' <param name="input"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function UrlEncode(input As String) As String

            Dim chrs() As Char = input
            Dim asc() As Byte
            Dim sOut As String = ""

            Dim i As Integer,
            c As Integer

            c = chrs.Length - 1

            For i = 0 To c

                If UrlAllowedChars.IndexOf(chrs(i)) <> -1 Then
                    sOut &= chrs(i)
                Else
                    asc = System.Text.UnicodeEncoding.Unicode.GetBytes(chrs(i))
                    For Each b As Byte In asc
                        If b <> 0 Then sOut &= "%" & b.ToString("X2")
                    Next
                End If
            Next

            Return sOut

        End Function

        ''' <summary>
        ''' Decode a string from a URL.
        ''' </summary>
        ''' <param name="input"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function UrlDecode(input As String) As String

            If input.IndexOf("%") = -1 Then Return input

            Dim parse() As String = BatchParse(input, "%", , , , , True, True)

            Dim asc As New Blob

            Dim hv As Integer = 0
            Dim str As String

            For Each str In parse
                If (str.Length = 3) AndAlso (str.Chars(0) = "%") AndAlso (IsHexQ(str.Substring(1), hv)) Then
                    asc &= CByte(hv)
                Else
                    asc &= str
                End If
            Next

            Return asc
        End Function

        Public Function AddSlashes(text As String, Optional quoteChar As Char = """"c, Optional slashChar As Char = "\"c) As String

            Dim ch() As Char = text
            Dim sb As New StringBuilder

            Dim i As Integer,
            c As Integer = ch.Length - 1

            For i = 0 To c

                If ch(i) = quoteChar Then
                    sb.Append(slashChar & quoteChar)
                Else
                    sb.Append(ch(i))
                End If

            Next

            AddSlashes = sb.ToString

        End Function


        ''' <summary>
        ''' Determines of a number can be parsed in hexadecimal 
        ''' (quick version, does not accept &amp;H or 0x, use IsHex() to parse those strings).
        ''' </summary>
        ''' <param name="input">Input string to scan.</param>
        ''' <param name="value">Optionally receives the value of the input string.</param>
        ''' <returns>True if the string can be parsed as a hexadecimal number.</returns>
        ''' <remarks></remarks>
        Public Function IsHexQ(input As String, Optional ByRef value As Integer = Nothing) As Boolean

            Dim hx As String = "0123456789ABCDEFabcdef"
            Dim i As Integer,
            c As Integer = input.Length - 1

            For i = 0 To c
                If hx.IndexOf(input.Chars(i)) = -1 Then
                    Return False
                End If
            Next

            If value <> Nothing Then
                value = Integer.Parse(input, Globalization.NumberStyles.HexNumber)
            End If

            Return True
        End Function

        ''' <summary>
        ''' Gets value of the DescriptionAttribute for a static member of an object.
        ''' </summary>
        ''' <param name="val"></param>
        ''' <param name="memberName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetStaticMemberDescription(val As System.Type, memberName As String) As String

            Dim fi() As MemberInfo = val.GetMembers(BindingFlags.Public Or BindingFlags.Static)

            For Each fe In fi
                If fe.Name = memberName Then
                    Dim da As DescriptionAttribute = fe.GetCustomAttribute(GetType(DescriptionAttribute))
                    Return da.Description
                End If
            Next

            Return Nothing
        End Function

        ''' <summary>
        ''' Gets value of the DescriptionAttribute for an instance member of an object.
        ''' </summary>
        ''' <param name="val"></param>
        ''' <param name="memberName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetMemberDescription(val As Object, memberName As String) As String

            Dim fi() As MemberInfo = val.GetType.GetFields(BindingFlags.Public Or BindingFlags.Instance)

            For Each fe In fi
                If fe.Name = memberName Then
                    Dim da As DescriptionAttribute = fe.GetCustomAttribute(GetType(DescriptionAttribute))
                    Return da.Description
                End If
            Next

            Return Nothing
        End Function

        ''' <summary>
        ''' Gets the value of the DescriptionAttribute of an enumeration member
        ''' </summary>
        ''' <param name="val"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetEnumDescription(val As Object) As String

            Dim fi() As FieldInfo = val.GetType.GetFields(BindingFlags.Public Or BindingFlags.Static)

            For Each fe In fi
                If fe.GetValue(val) = val Then
                    Dim da As DescriptionAttribute = fe.GetCustomAttribute(GetType(DescriptionAttribute))
                    Return da.Description
                End If
            Next

            Return Nothing
        End Function

    End Module

    ''' <summary>
    ''' A collection of Key-Value Pair objects.
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <typeparam name="U"></typeparam>
    ''' <remarks></remarks>
    Public Class CollectionOfKeyValue(Of T, U)
        Inherits Collections.ObjectModel.ObservableCollection(Of KeyValuePair(Of T, U))

        Public ReadOnly Property ItemByKey(key As T) As U
            Get
                For Each kv In Me
                    If kv.Key.Equals(key) Then Return kv.Value
                Next
            End Get
        End Property

        Public Function ContainsKey(key As T) As Boolean
            For Each kv In Me
                If kv.Key.Equals(key) Then Return True
            Next
            Return False
        End Function

        Public Sub AddNew(key As T, value As U)
            Me.Add(New KeyValuePair(Of T, U)(key, value))

        End Sub

    End Class


    ''' <summary>
    ''' A class that contains a StringBuilder that does automatic indentation.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class StringBuilderIndenter
        Private _sb As StringBuilder

        ''' <summary>
        ''' This is the base level indent.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property BaseIndent As Integer = 0

        ''' <summary>
        ''' This is the amount of indent per level.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IndentAmount As Integer = 4

        Private _Level As Integer = 1

        Private _atStart As Boolean = True

        ''' <summary>
        ''' Increase indent level by one.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Indent(Optional levels As Integer = 1)
            _Level += levels
        End Sub

        ''' <summary>
        ''' Decrease indent level by one.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub UnIndent(Optional levels As Integer = 1)
            _Level -= levels
            If _Level < 1 Then _Level = 1
        End Sub

        ''' <summary>
        ''' Append the current indentation to the StringBuilder.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub AppendIndent()
            _sb.Append(New String(" ", BaseIndent + (IndentAmount * _Level)))
            _atStart = False
        End Sub

        ''' <summary>
        ''' Append the current indentation and specified text to the StringBuilder.
        ''' If the line is already indented, the indentation is ignored.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Append(text As String)
            If text.IndexOf(vbCrLf) = -1 Then
                If _atStart Then AppendIndent()
                _sb.Append(text)
                _atStart = False

                Return
            End If

            Dim d() As String = BatchParse(text, vbCrLf, , , , , True)

            For Each x In d
                If x = vbCrLf Then
                    _sb.AppendLine()
                    _atStart = True
                Else
                    If _atStart Then AppendIndent()
                    _sb.Append(x)
                    _atStart = True
                End If
            Next
        End Sub

        ''' <summary>
        ''' Appends a line flush to the edge, regardless of the current indentation.
        ''' </summary>
        ''' <param name="text"></param>
        ''' <remarks></remarks>
        Public Sub AppendFlushLine(text As String)
            _sb.AppendLine(text)
        End Sub

        ''' <summary>
        ''' Append a line with the current indentation and specified text to the StringBuilder.
        ''' If the text on this line is already indented, it is not indented, again.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub AppendLine(Optional text As String = Nothing)
            If _atStart Then AppendIndent()
            If text IsNot Nothing Then _sb.AppendLine(text) Else _sb.AppendLine()
            _atStart = True
        End Sub

        ''' <summary>
        ''' Returns the current indent level.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Level As Integer
            Get
                Return _Level
            End Get
        End Property

        ''' <summary>
        ''' Clears the string.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Clear()
            _sb.Clear()
        End Sub

        Public Overrides Function ToString() As String
            Return _sb.ToString
        End Function

        Public Sub New()
            _sb = New StringBuilder
        End Sub

        Public Sub New(sb As StringBuilder)
            _sb = sb
        End Sub

    End Class


End Namespace