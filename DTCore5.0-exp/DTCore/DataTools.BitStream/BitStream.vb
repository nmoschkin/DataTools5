'' ************************************************* ''
'' DataTools Visual Basic Utility Library 
''
'' Module: Bitwise LShift and RShift for Streams
'' 
'' Copyright (C) 2011-2020 Nathan Moschkin
'' All Rights Reserved
''
'' Licensed Under the Microsoft Public License   
'' ************************************************* ''

Namespace BitStream

    Public Module BitStreamShift

        '' Let's make it simple

        Friend Const BadArgs As String = "Valid objects are arrays of byte, sbyte, short, ushort, int, uint, or long, ulong."

        ''' <summary>
        ''' Shifts an array of integral typed elements to the left by the specified number of bits.
        ''' Valid objects are arrays of byte, sbyte, short, ushort, int, uint, or long, ulong.
        ''' </summary>
        ''' <param name="input"></param>
        ''' <param name="shift"></param>
        Public Sub LShiftN(ByRef input As Object, shift As Integer)
            Dim t As Type = input.GetType.GetElementType

            If Not t.IsArray Then
                Throw New ArgumentException(BadArgs, "input")
            End If

            Select Case (t)
                Case GetType(Byte), GetType(SByte), GetType(Short), GetType(UShort), GetType(Integer), GetType(UInteger), GetType(Long), GetType(ULong)
                    Exit Select

                Case Else
                    Throw New ArgumentException(BadArgs, "input")
            End Select

            Dim o As Object = Array.CreateInstance(t, 1)
            Dim e As Integer = Len(o(0)) * 8

            Dim b As Object,
                c As Integer = input.Length

            Dim i As Integer,
                b1 As Object

            If shift >= e Then
                Dim m As Integer = CInt(Math.Floor(shift / e))

                b = Array.CreateInstance(t, c)
                Array.Copy(input, m, b, 0, (c - m))

                Erase input
                input = b
                shift -= (m * e)
            End If

            If shift = 0 Then Return

            c -= 1
            For i = 0 To c
                If i < c Then
                    b1 = input(i)
                    b1 = ((b1 << shift) Or (input(i + 1) >> (e - shift)))
                    input(i) = b1
                Else
                    input(i) <<= shift
                End If
            Next

        End Sub

        ''' <summary>
        ''' Shifts an array of integral typed elements to the right by the specified number of bits.
        ''' Valid objects are arrays of byte, sbyte, short, ushort, int, uint, or long, ulong.
        ''' </summary>
        ''' <param name="input"></param>
        ''' <param name="shift"></param>
        Public Sub RShiftN(ByRef input As Object, shift As Integer)
            Dim t As Type = input.GetType.GetElementType

            If Not t.IsArray Then
                Throw New ArgumentException(BadArgs, "input")
            End If

            Select Case (t)
                Case GetType(Byte), GetType(SByte), GetType(Short), GetType(UShort), GetType(Integer), GetType(UInteger), GetType(Long), GetType(ULong)
                    Exit Select

                Case Else
                    Throw New ArgumentException(BadArgs, "input")
            End Select

            Dim o As Object = Array.CreateInstance(t, 1)
            Dim e As Integer = Len(o(0)) * 8

            Dim b As Object,
                c As Integer = input.Length

            Dim i As Integer,
                b1 As Object

            If shift >= e Then
                Dim m As Integer = CInt(Math.Floor(shift / e))

                b = Array.CreateInstance(t, c)
                Array.Copy(input, 0, b, m, (c - m))

                Erase input
                input = b
                shift -= (m * e)
            End If

            If shift = 0 Then Return

            c -= 1
            For i = c To 0 Step -1
                If i > 0 Then
                    b1 = input(i)
                    b1 = ((b1 >> shift) Or (input(i - 1) << (e - shift)))
                    input(i) = b1
                Else
                    input(i) >>= shift
                End If
            Next

        End Sub

    End Module

End Namespace
