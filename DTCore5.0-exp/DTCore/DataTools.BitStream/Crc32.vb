'' ************************************************* ''
'' DataTools Visual Basic Utility Library 
''
'' Module: Implements ISO-3309 CRC-32 Calculator
''         And Validator
'' 
'' Copyright (C) 2011-2020 Nathan Moschkin
'' All Rights Reserved
''
'' Licensed Under the Microsoft Public License   
'' ************************************************* ''

Imports DataTools.Memory


Namespace BitStream

#Region "CRC32"

    ''' <summary>
    ''' ISO 3309 CRC-32 Calculator.
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class Crc32

        Private Shared ReadOnly CRC32Poly As UInteger = &HEDB88320UI
        Private Shared Crc32Table(0 To 255) As UInteger

        Private Sub New()
            ' this is not a creatable object.
        End Sub

        ''' <summary>
        ''' Initialize the CRC table from the polynomial.
        ''' </summary>
        ''' <remarks></remarks>
        Shared Sub New()

            Dim i As UInteger,
            j As UInteger,
            l As UInteger

            For i = 0 To 255
                j = i
                For l = 0 To 7
                    If (j And 1) Then
                        j = ((j >> 1) Xor CRC32Poly)
                    Else
                        j >>= 1
                    End If
                Next
                Crc32Table(i) = j
            Next i

        End Sub

        ''' <summary>
        ''' Validates a byte array against an input CRC.
        ''' </summary>
        ''' <param name="data">The byte array to validate.</param>
        ''' <param name="inputCrc">The CRC value against which the validation should occur.</param>
        ''' <returns>True if the input CRC matches the calculated CRC of the data.</returns>
        ''' <remarks></remarks>
        Public Shared Function Validate(data() As Byte, inputCrc As UInteger) As Boolean
            Return Calculate(data) = inputCrc
        End Function

        ''' <summary>
        ''' Validates a memory block against an input CRC.
        ''' </summary>
        ''' <param name="data">The memory block validate.</param>
        ''' <param name="length">The length of the memory block to validate.</param>
        ''' <param name="inputCrc">The CRC value against which the validation should occur.</param>
        ''' <returns>True if the input CRC matches the calculated CRC of the data.</returns>
        ''' <remarks></remarks>
        Public Shared Function Validate(data As IntPtr, length As IntPtr, inputCrc As UInteger) As Boolean
            Return Calculate(data, length) = inputCrc
        End Function

        ''' <summary>
        ''' Validates a file against an input CRC.
        ''' </summary>
        ''' <param name="fileName">Filename of the file to validate.</param>
        ''' <param name="inputCrc">The CRC value against which the validation should occur.</param>
        ''' <returns>True if the input CRC matches the calculated CRC of the data.</returns>
        ''' <remarks></remarks>
        Public Shared Function Validate(fileName As String, inputCrc As UInteger) As Boolean
            Return Calculate(fileName) = inputCrc
        End Function

        ''' <summary>
        ''' Calculate the CRC-32 of an array of bytes.
        ''' </summary>
        ''' <param name="data">Byte array containing the bytes to calculate.</param>
        ''' <param name="startIndex">Specifies the starting index to begin the calculation (default is 0).</param>
        ''' <param name="length">Specify the length of the byte array to check (default is -1, or all bytes).</param>
        ''' <param name="crc">Input CRC value for ongoing calculations (default is FFFFFFFFh).</param>
        ''' <returns>A 32-bit unsigned integer representing the calculated CRC.</returns>
        ''' <remarks></remarks>
        Public Shared Function Calculate(data() As Byte, Optional startIndex As Integer = 0, Optional length As Integer = -1, Optional crc As UInteger = &HFFFFFFFFUI) As UInteger
            If data Is Nothing Then Throw New ArgumentNullException("data", "data cannot be equal to null.")

            If length = -1 Then length = data.Length - startIndex
            If length <= 0 Then Throw New ArgumentOutOfRangeException("length", "length must be -1 or a positive number.")

            Dim j As Integer
            Dim c As Integer = length - 1

            For j = startIndex To c
                crc = Crc32Table((crc Xor data(j)) And &HFF) Xor (crc >> 8)
            Next j

            Calculate = crc Xor &HFFFFFFFFUI
        End Function

        ''' <summary>
        ''' Calculate the CRC-32 of a memory pointer. 
        ''' </summary>
        ''' <param name="data">Pointer containing the bytes to calculate.</param>
        ''' <param name="length">Specify the length, in bytes, of the data to be checked.</param>
        ''' <param name="crc">Input CRC value for ongoing calculations (default is FFFFFFFFh).</param>
        ''' <param name="bufflen">Specify the size, in bytes, of the marshaling buffer to be used (default is 1k).</param>
        ''' <returns>A 32-bit unsigned integer representing the calculated CRC.</returns>
        ''' <remarks></remarks>
        Public Shared Function Calculate(data As IntPtr, length As IntPtr, Optional crc As UInteger = &HFFFFFFFFUI, Optional bufflen As Integer = 1024) As UInteger
            If length.ToInt64 <= 0 Then Throw New ArgumentOutOfRangeException("length", "length must be a positive number.")
            If data = IntPtr.Zero Then Throw New ArgumentNullException("data", "data cannot be equal to null.")

            '' our working marshal buffer will be 1k, this is a good compromise between eating up memory and efficiency.
            Dim blen As Integer = bufflen

            Dim b() As Byte
            Dim mm As New MemPtr(data)

            Dim i As Long
            Dim l As Long = length.ToInt64
            Dim c As Long = l - 1

            Dim e As Integer
            Dim j As Integer

            ReDim b(blen - 1)

            For i = 0 To c Step blen

                If (l - i) > blen Then
                    e = blen
                Else
                    e = CInt(l - i)
                End If

                mm.GrabBytes(i, e, b)

                e -= 1
                For j = 0 To e
                    crc = Crc32Table((crc Xor b(j)) And &HFF) Xor (crc >> 8)
                Next j
            Next

            Calculate = crc Xor &HFFFFFFFFUI
        End Function

        ''' <summary>
        ''' Calculate the CRC-32 of a file.
        ''' </summary>
        ''' <param name="fileName">Filename of the file to calculate.</param>
        ''' <param name="bufflen">Specify the size, in bytes, of the read buffer to be used (default is 1k).</param>
        ''' <returns>A 32-bit unsigned integer representing the calculated CRC.</returns>
        ''' <remarks></remarks>
        Public Shared Function Calculate(fileName As String, Optional bufflen As Integer = 1024) As UInteger
            If (Not IO.File.Exists(fileName)) Then
                Throw New IO.FileNotFoundException(fileName & " could not be found.")
                Return 0
            End If

            Calculate = Calculate(New IO.FileStream(fileName, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.Read), bufflen)
        End Function

        ''' <summary>
        ''' Calculate the CRC-32 of a Stream.
        ''' </summary>
        ''' <param name="stream">The Stream to calculate.</param>
        ''' <param name="bufflen">Specify the size, in bytes, of the read buffer to be used (default is 1k).</param>
        ''' <returns>A 32-bit unsigned integer representing the calculated CRC.</returns>
        ''' <remarks></remarks>
        Public Shared Function Calculate(stream As System.IO.Stream, Optional bufflen As Integer = 1024, Optional closeStream As Boolean = True) As UInteger
            '' our working marshal buffer will be 1k, this is a good compromise between eating up memory and efficiency.
            Dim blen As Integer = bufflen
            Dim crc As UInteger = &HFFFFFFFFUI

            Dim b() As Byte

            Dim i As Long
            Dim l As Long = stream.Length
            Dim c As Long = l - 1

            Dim e As Integer
            Dim j As Integer

            ReDim b(blen - 1)

            For i = 0 To c Step blen

                If (l - i) > blen Then
                    e = blen
                Else
                    e = CInt(l - i)
                End If

                If (stream.Position <> i) Then stream.Seek(i, IO.SeekOrigin.Begin)
                stream.Read(b, 0, e)

                e -= 1
                For j = 0 To e
                    crc = Crc32Table((crc Xor b(j)) And &HFF) Xor (crc >> 8)
                Next j
            Next

            If (closeStream) Then stream.Close()
            Calculate = crc Xor &HFFFFFFFFUI
        End Function


    End Class

#End Region

End Namespace