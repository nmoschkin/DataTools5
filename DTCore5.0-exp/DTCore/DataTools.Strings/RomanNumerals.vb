'' ************************************************* ''
'' DataTools Visual Basic Utility Library 
''
'' Module: Print and Interpret Roman Numerals
'' 
'' Copyright (C) 2011-2020 Nathan Moschkin
'' All Rights Reserved
''
'' Licensed Under the Microsoft Public License   
'' ************************************************* ''

Option Strict On
Option Explicit On

Imports System.Text

Namespace Strings

    ''' <summary>
    ''' Methods to output roman numeral characters.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum RomanNumeralStyle

        ''' <summary>
        ''' Modern, conventional roman numeral notation.
        ''' </summary>
        ''' <remarks></remarks>
        Modern

        ''' <summary>
        ''' Antique roman numeral notation.
        ''' </summary>
        ''' <remarks></remarks>
        Antique
    End Enum

    ''' <summary>
    ''' Static class to provide roman numeral translation.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class RomanNumerals

        ''' <summary>
        ''' This class is not creatable.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub New()
        End Sub

        ''' <summary>
        ''' Converts a roman numeral string into an integer.
        ''' </summary>
        ''' <param name="x"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ToInteger(x As String) As Integer

            Dim ch() As Char = TextTools.NoSpace(x.ToUpper.Trim).ToCharArray

            Dim c As Integer = 0
            Dim d As Integer = ch.Length - 1

            Dim vals As New List(Of Integer)

            For i = 0 To d
                Select Case ch(i)

                    Case "M"c
                        vals.Add(1000)
                    Case "D"c
                        vals.Add(500)
                    Case "C"c
                        vals.Add(100)
                    Case "L"c
                        vals.Add(50)
                    Case "X"c
                        vals.Add(10)
                    Case "V"c
                        vals.Add(5)
                    Case "I"c
                        vals.Add(1)

                End Select

            Next

            d = vals.Count - 1

            For i = d To 0 Step -1
                If i < d Then
                    If vals(i + 1) > vals(i) Then
                        c -= vals(i)
                        Continue For
                    End If
                End If

                c += vals(i)
            Next

            Return c
        End Function

        ''' <summary>
        ''' Converts an integer into a roman numeral string.
        ''' </summary>
        ''' <param name="x"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ToNumerals(x As Integer, Optional style As RomanNumeralStyle = RomanNumeralStyle.Modern) As String

            If x = 0 Then Return "NVL"
            If x < 0 Then x = -x

            Dim s As String = "" & x

            Dim out As String = ""

            Dim ch() As Char = s.ToCharArray()

            Select Case ch.Length

                Case 1
                    Return FirstToNum(ch(0), style)

                Case 2
                    Return SecondToNum(ch(0), style) & FirstToNum(ch(1), style)

                Case 3
                    Return ThirdToNum(ch(0), style) & SecondToNum(ch(1), style) & FirstToNum(ch(2), style)

                Case Is >= 4
                    Return FourthToNum(s) & ThirdToNum(ch(1), style) & SecondToNum(ch(2), style) & FirstToNum(ch(3), style)

            End Select

            Return Nothing
        End Function

#Region "Roman Numeral Out Private Functions"

        Private Shared ReadOnly FirstNums As String() = {"", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX"}
        Private Shared ReadOnly FirstNumsAntique As String() = {"", "I", "II", "III", "IIII", "V", "VI", "VII", "VIII", "VIIII"}

        ''' <summary>
        ''' Converts the first integer (from right-to-left) into a roman numeral.
        ''' </summary>
        ''' <param name="ch"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Shared Function FirstToNum(ch As Char, Optional style As RomanNumeralStyle = RomanNumeralStyle.Modern) As String
            Return If(style = RomanNumeralStyle.Antique, FirstNumsAntique(Integer.Parse(ch)), FirstNums(Integer.Parse(ch)))
        End Function

        Private Shared ReadOnly SecondNums As String() = {"", "X", "XX", "XXX", "XL", "L", "LX", "LXX", "LXXX", "XC"}
        Private Shared ReadOnly SecondNumsAntique As String() = {"", "X", "XX", "XXX", "XXXX", "L", "LX", "LXX", "LXXX", "LXXXX"}

        ''' <summary>
        ''' Converts the second integer (from right-to-left) into a roman numeral.
        ''' </summary>
        ''' <param name="ch"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Shared Function SecondToNum(ch As Char, Optional style As RomanNumeralStyle = RomanNumeralStyle.Modern) As String
            Return If(style = RomanNumeralStyle.Antique, SecondNumsAntique(Integer.Parse(ch)), SecondNums(Integer.Parse(ch)))
        End Function

        Private Shared ReadOnly ThirdNums As String() = {"", "C", "CC", "CCC", "CD", "D", "DC", "DCC", "DCCC", "CM"}
        Private Shared ReadOnly ThirdNumsAntique As String() = {"", "C", "CC", "CCC", "CCCC", "D", "DC", "DCC", "DCCC", "DCCCC"}

        ''' <summary>
        ''' Converts the third integer (from right-to-left) into a roman numeral.
        ''' </summary>
        ''' <param name="ch"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Shared Function ThirdToNum(ch As Char, Optional style As RomanNumeralStyle = RomanNumeralStyle.Modern) As String
            Return If(style = RomanNumeralStyle.Antique, ThirdNumsAntique(Integer.Parse(ch)), ThirdNums(Integer.Parse(ch)))
        End Function

        ''' <summary>
        ''' Converts subsequent integers into a series of M's.
        ''' </summary>
        ''' <param name="textNum"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Shared Function FourthToNum(textNum As String) As String

            Try
                Dim s As String = textNum.Substring(0, textNum.Length - 3)

                Dim o As New StringBuilder

                Dim v As Integer = CInt(TextTools.FVal(s))

                For i = 1 To v
                    o.Append("M")
                Next

                Return o.ToString
            Catch ex As Exception
                Return ""
            End Try

        End Function

#End Region

    End Class

End Namespace