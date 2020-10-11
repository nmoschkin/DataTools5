' Basic Math Functions
' Copyright (C) 2015 Nathaniel Moschkin

Option Explicit On
Option Strict On

Imports CoreCT.Memory
Imports CoreCT.Text

<HideModuleName()>
Public Module MathLib
    '' Extended mathematic process library for Visual Basic
    ''
    '' Duplication is prohibited, as is commercial use without prior written permission.
    '' Copyright (C) 2015 Nathaniel Moschkin.  All Rights Reserved.

    Public Const MAX_VALUE = 1.7976931348623157E+308
    Public Const MIN_VALUE = 4.94065645841247E-324

    Public Const ProgressDiv = 17

    Public OnBits(0 To 31) As Integer

    Public LShiftTab(0 To 255, 0 To 7) As Byte
    Public RShiftTab(0 To 255, 0 To 7) As Byte

#Region "Inches <-> Millimeters"

    Public Function InchesToMillimeters(value As Double) As Double
        Return value * 25.4
    End Function

    Public Function MillimetersToInches(value As Double) As Double
        Return value / 25.4
    End Function

    Public Function InchesToMillimeters(value As Double()) As Double()

        Dim i As Integer,
        c As Integer = value.Count - 1

        For i = 0 To c
            value(i) *= 25.4
        Next

        Return value

    End Function

    Public Function MillimetersToInches(value As Double()) As Double()

        Dim i As Integer,
        c As Integer = value.Count - 1

        For i = 0 To c
            value(i) /= 25.4
        Next

        Return value
    End Function

#End Region

    '' I actually needed no guidance coming up with the PrintFraction equation.  It was a logical deduction.


    ''' <summary>
    ''' Prints a fractional number from a decimal value.
    ''' </summary>
    ''' <param name="value">The value to convert to a fraction.</param>
    ''' <param name="maxSignificantDigits">The maximum number of significant digits the number can be rounded to.</param>
    ''' <param name="maxDenominator">The maximum possible value of the denominator.</param>
    ''' <param name="addQuasiMark">Set to True to output the '~' symbol for fractions that are found below the maximum significant digit.</param>
    ''' <returns>A string representing a whole number with a fraction.</returns>
    ''' <remarks>The number of iterations required by this algorithm is greatly influenced by the size of the maximum denominator.</remarks>
    Public Function PrintFraction(value As Decimal, Optional maxSignificantDigits As Integer = 7, Optional maxDenominator As Integer = 25, Optional addQuasiMark As Boolean = True) As String

        Dim wholePart As Decimal = 0.0D

        Dim workVal As Decimal,
            testVal As Decimal

        Dim numerator As Decimal,
            denominator As Decimal

        Dim currSig As Integer,
            hSigFound As Integer = 1

        Dim foundFractions As New List(Of Integer())

        Dim output As String = ""
        Dim lastTest As Decimal

        If value = Decimal.Zero Then
            Return "0"
        End If

        '' test to see if there is a whole-number component, and place it off to the side.
        If value >= 1.0# Then
            wholePart = System.Math.Floor(value)
            value -= wholePart

            '' if there is no fractional component, there's no reason to go on.
            If value = 0 Then Return wholePart.ToString("0")
        End If

        If maxSignificantDigits > 28 Then maxSignificantDigits = 28
        '' Go from 1 to the maximum number of significant digits.
        For currSig = 1 To maxSignificantDigits

            '' get the rounded, working value for the test.
            workVal = System.Math.Round(value, currSig)

            '' iterate the numerator to the maximum denominator value.
            For numerator = 1 To maxDenominator

                '' iterate the denomenator to the maximum denominator value.
                For denominator = 1 To maxDenominator

                    '' create the test value.
                    testVal = System.Math.Round(numerator / denominator, currSig)

                    If testVal = workVal Then
                        '' at this significant digit, the test and working values
                        '' produce the same result, meaning that this is a viable
                        '' fraction.  But it may not be the only viable fraction
                        '' within the range of possible denominators.

                        '' we'll record the highest significant digit for which
                        '' a fraction was found, so far.
                        hSigFound = currSig

                        '' add the compatible pair to a list
                        foundFractions.Add(New Integer() {CInt(numerator), CInt(denominator)})

                        '' break to the next significant digit, there's
                        '' no reason to do any more work.
                        lastTest = testVal
                        GoTo nextSig
                    End If
                Next
            Next

nextSig:
        Next


        '' the best fit will be the last found fraction pair,
        '' which will have the closest approximation to the the original number,
        '' within the bounds of available significant digits.
        numerator = foundFractions(foundFractions.Count - 1)(0)
        denominator = foundFractions(foundFractions.Count - 1)(1)

        '' if the higest found fraction was below the maximum number of significant digits,
        '' we will optionally add the "~" (quasi) mark to the output string to indicate that
        '' this is an approximation (with regard to the number of significant digits).
        If (addQuasiMark = True) AndAlso (hSigFound < maxSignificantDigits) AndAlso (lastTest <> value) Then
            output &= "~ "
        End If

        '' If we have an integer component, add that to the output string.
        If (numerator = 1 AndAlso denominator = 1) Then wholePart = wholePart + 1

        If wholePart > 0 Then
            output &= wholePart & " "
        End If

        '' Finally, add the fraction.
        If Not (numerator = 1 AndAlso denominator = 1) Then _
            output &= String.Format("{0}/{1}", numerator.ToString("0"), denominator.ToString("0"))

        '' We're done!
        Return output

    End Function
    ''' <summary>
    ''' Strips the units from a number, returns both, cleaned.
    ''' </summary>
    ''' <param name="text"></param>
    ''' <param name="unit"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function stripUnit(text As String, ByRef unit As String) As String

        Dim ch() As Char = text.ToCharArray
        Dim i As Integer,
        c As Integer = ch.Count - 1

        Dim l As New List(Of Char)
        Dim d As Decimal = 0

        Dim noL As Boolean = False
        unit = Nothing

        For i = c To 0 Step -1

            If TextTools.IsNumber(ch(i)) Then
                If i = c Then Return text.Trim

                unit = text.Substring(i + 1).Trim
                text = text.Substring(0, i + 1).Trim

                Exit For
            End If
        Next

        Return text

    End Function

    ''' <summary>
    ''' Parse any string into an array of numbers with their optional unit markers.
    ''' </summary>
    ''' <param name="input"></param>
    ''' <param name="returnUnits"></param>
    ''' <param name="validunits"></param>
    ''' <param name="validSeparators"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ParseArrayOfNumbersWithUnits(input As String,
                                             Optional ByRef returnUnits() As String = Nothing,
                                             Optional validunits() As String = Nothing,
                                             Optional validSeparators As String = ",x;:") As Double()

        If validunits Is Nothing Then
            validunits = {"in", "mm", "'", """", "ft"}
        End If

        Dim vc() As Char = validSeparators.ToCharArray

        Dim i As Integer,
        c As Integer
        Dim sb As New System.Text.StringBuilder

        Dim retVal As New List(Of Double)
        Dim ru As New List(Of String)

        Dim s As String
        Dim exp As New MathExpressionParser

        input = input.ToLower

        c = vc.Count - 1

        For i = 1 To c
            input = input.Replace(vc(i), vc(0))
        Next

        Dim parse() As String = TextTools.Split(input, vc(0))

        c = parse.Count - 1

        Dim su As String = Nothing

        For i = 0 To c
            s = parse(i).Trim
            s = stripUnit(s, su)

            ru.Add(su)
            retVal.Add(exp.ParseExpression(s))
        Next

        returnUnits = ru.ToArray
        Return retVal.ToArray

    End Function

    ''' <summary>
    ''' Strips out everything else from a string and returns only the numbers.
    ''' </summary>
    ''' <param name="input"></param>
    ''' <param name="culture"></param>
    ''' <param name="hex"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function NumbersOnly(input As String, Optional culture As System.Globalization.CultureInfo = Nothing, Optional hex As Boolean = False) As String

        If culture Is Nothing Then
            culture = System.Globalization.CultureInfo.CurrentCulture
        End If

        Dim ch() As Char

        Dim sb As New System.Text.StringBuilder,
        i As Integer

        Dim scan As String
        Dim cs As Integer = AscW("0")

        For i = 0 To 9
            sb.Append(ChrW(cs + i))
        Next

        If hex Then
            cs = AscW("A")

            For i = 0 To 5
                sb.Append(ChrW(cs + i))
            Next

            cs = AscW("a")

            For i = 0 To 5
                sb.Append(ChrW(cs + i))
            Next
        End If

        sb.Append(culture.NumberFormat.NumberDecimalSeparator)
        sb.Append(culture.NumberFormat.CurrencyDecimalSeparator)
        sb.Append(culture.NumberFormat.PercentDecimalSeparator)
        sb.Append(culture.NumberFormat.NegativeSign)
        sb.Append(culture.NumberFormat.PositiveSign)

        ch = input.ToCharArray
        scan = sb.ToString

        cs = ch.Length - 1

        For i = 0 To cs

            If scan.Contains(ch(i)) Then
                sb.Append(ch(i))
            End If

        Next

        Return sb.ToString
    End Function

    Public Function AverageColorValue(ARGB As Integer) As Integer
        Dim r As Single,
        g As Single,
        b As Single,
        i As Integer

        Dim o As Single

        i = (ARGB And &HFFFFFF)

        r = i And &HFF
        g = (i >> 8) And &HFF
        b = (i >> 16) And &HFF

        o = (r + g + b) / 3
        Return CInt(Math.Round(o))

    End Function

    ''' <summary>
    ''' Swap nibbles
    ''' </summary>
    ''' <param name="ByteVal"></param>
    ''' <returns></returns>
    Public Function Swan(ByteVal As Byte) As Byte
        Swan = (ByteVal << 4) Or (ByteVal >> 4)
    End Function

    Public Function Endian(Value As UInteger) As UInteger
        Dim b = BitConverter.GetBytes(Value)
        Array.Reverse(b)
        Return BitConverter.ToUInt32(b)
    End Function

    Public Function Endian(Value As Integer) As Integer
        Dim b = BitConverter.GetBytes(Value)
        Array.Reverse(b)
        Return BitConverter.ToInt32(b)
    End Function

    Public Function CalcCRC32(bytesIn() As Byte, dwLen As Integer, Optional ShowProgress As Boolean = False, Optional CryptoCRC As Boolean = False, Optional crc As UInteger = &HFFFFFFFFUI) As UInteger

        Return Crc32.Calculate(bytesIn, 0, dwLen, crc)

    End Function

End Module