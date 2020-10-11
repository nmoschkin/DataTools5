'' ************************************************* ''
'' DataTools Visual Basic Utility Library 
''
'' Module: StringBlob
''         Collection of strings in unmanaged memory.
'' 
'' Copyright (C) 2011-2020 Nathan Moschkin
'' All Rights Reserved
''
'' Licensed Under the Microsoft Public License   
'' ************************************************* ''

Option Explicit On
Option Compare Binary
Option Strict Off

Imports System
Imports System.Text
Imports System.IO

Imports System.ComponentModel
Imports System.ComponentModel.Design.Serialization
Imports System.Runtime
Imports System.Runtime.InteropServices
Imports System.Numerics

Imports DataTools.Memory.Internal
Imports DataTools.Memory
Imports DataTools.ByteOrderMark

Namespace Memory

#Region "StringBlob"

    ''' <summary>
    ''' StringBlob manages unmanaged arrays of strings in memory (either the LPWSTR or BSTR varierty.)
    ''' </summary>
    ''' <remarks></remarks>
    <StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)> _
    Public Class StringBlob
        Implements IEnumerable, ICloneable, IDisposable

        Private _BOM As BOM
        Private _sizeDescriptorType As BlobOrdinalTypes = BlobOrdinalTypes.Integer
        Private _sizeDescriptorLength As Integer = 4
        Private _lpwstr As Boolean = False
        Private _mem As New SafePtr

        Private _index() As IntPtr
        Private _count As Integer

        Private _dynamic As Boolean = True

        ''' <summary>
        ''' Creates a new empty StringBlob
        ''' </summary>
        Public Sub New()

        End Sub

        ''' <summary>
        ''' Creates a new StringBlob from an array of strings.
        ''' </summary>
        ''' <param name="strings"></param>
        Public Sub New(strings() As String)
            AddStrings(strings)
        End Sub

        ''' <summary>
        ''' Creates a new StringBlob and initialize the data with the contents of a Blob object.
        ''' </summary>
        ''' <param name="blob"></param>
        Public Sub New(blob As Blob)
            _mem = New SafePtr(blob.Length)
            _mem.CopyFrom(blob.DangerousGetHandle, blob.Length)
        End Sub

        ''' <summary>
        ''' Gets or sets a value indicating that the string-blob is dynamic.  
        ''' Dynamic string-blobs always return freshly indexed information on
        ''' the number and size of strings. Additionally, the SafePtr backing object
        ''' can be hot-swapped.
        ''' </summary>
        ''' <returns></returns>
        Public Property Dynamic As Boolean
            Get
                Return _dynamic
            End Get
            Set(value As Boolean)
                If value = _dynamic Then Return

                If value Then
                    Refresh()
                    GetCount()
                Else
                    _count = -1
                    Erase _index
                End If

                _dynamic = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a value indicating that strings are stored as null-terminated strings.
        ''' Otherwise, the strings are preambled with a length descriptor.
        ''' </summary>
        ''' <returns></returns>
        Public Property LpWStr As Boolean
            Get
                Return _lpwstr
            End Get
            Set(value As Boolean)
                If _lpwstr = value Then Return
                Dim c As Integer = Count - 1

                If c <= 0 Then
                    _lpwstr = value
                    Return
                End If

                If Not value Then
                    Dim lpsz As New LPWSTR,
                        mm As New SafePtr,
                        px As IntPtr

                    mm.Length = ((c * 2) + SizeDescriptorLength) + SizeDescriptorLength

                    lpsz._ptr = _mem.handle

                    px = mm.handle

                    For i = 0 To c
                        MemCpy(px, lpsz._ptr, lpsz.Length)
                        px += lpsz.Length + 2
                        lpsz._ptr += (lpsz.Length * 2) + 2
                    Next

                    GC.SuppressFinalize(lpsz)
                    _mem.Dispose()
                    _mem = mm
                Else
                    Dim bs As New BSTR,
                        mm As New SafePtr,
                        px As IntPtr

                    mm.Length = ((c + 1) * 2) + 2

                    bs._preamble = SizeDescriptorLength
                    bs._ptr = _mem.handle + bs._preamble

                    px = mm.handle

                    For i = 0 To c
                        MemCpy(px, IntPtr.Add(bs._ptr, bs._preamble), bs.Length)
                        px += (bs.Length * 2) + bs._preamble
                    Next

                    GC.SuppressFinalize(bs)
                    _mem.Dispose()
                    _mem = mm
                End If

            End Set
        End Property

        ''' <summary>
        ''' Gets the length, in bytes, of the size descriptor preamble.
        ''' Valid values are 2, 4 and 8. A value of 0 will default to 4.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property SizeDescriptorLength As Integer
            Get
                If _sizeDescriptorLength = 0 Then
                    _sizeDescriptorType = BlobOrdinalTypes.Integer
                    _sizeDescriptorLength = 4
                End If

                Return _sizeDescriptorLength
            End Get
        End Property

        ''' <summary>
        ''' Returns the ordinal type of the size descriptor.
        ''' </summary>
        ''' <returns></returns>
        Public Property SizeDescriptorType As BlobOrdinalTypes
            Get
                Select Case _sizeDescriptorType
                    Case BlobOrdinalTypes.Byte, BlobOrdinalTypes.Short, BlobOrdinalTypes.Integer
                        Return _sizeDescriptorType

                    Case Else
                        _sizeDescriptorType = BlobOrdinalTypes.Integer
                        _sizeDescriptorLength = 4
                        Return _sizeDescriptorType
                End Select
            End Get
            Set(value As BlobOrdinalTypes)
                Select Case value
                    Case BlobOrdinalTypes.Byte, BlobOrdinalTypes.Short, BlobOrdinalTypes.Integer
                        _sizeDescriptorType = value
                        _sizeDescriptorLength = Blob.BlobTypeSize(value)

                    Case Else
                        _sizeDescriptorType = BlobTypes.Integer
                        _sizeDescriptorLength = 4
                End Select
            End Set
        End Property

        ''' <summary>
        ''' Gets the count of strings.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Count As Integer
            Get
                If _dynamic Then
                    Return GetCount()
                Else
                    If Not _lpwstr Then Return BCount()
                    Return _count
                End If
            End Get
        End Property

        ''' <summary>
        ''' Truncate the string blob to the specified last index.
        ''' </summary>
        ''' <param name="lastIndex">Index of last string to retain.</param>
        Public Sub Truncate(lastIndex As Integer)

            If _index Is Nothing Then Refresh()

            Dim l As IntPtr = _index(lastIndex) + (LengthAt(lastIndex) * 2)

            If _lpwstr Then l += 2 Else l += SizeDescriptorLength

            ReDim Preserve _index(lastIndex)
            SafePtr.Length = l.ToInt64 - SafePtr.handle.ToInt64
            _count = _index.Length

            If Not _lpwstr Then
                Select Case SizeDescriptorLength

                    Case 2
                        _mem.UShortAt(0) = _count
                    Case 4
                        _mem.UIntegerAt(0) = _count
                    Case 8
                        _mem.ULongAt(0) = _count
                End Select
            End If

        End Sub

        ''' <summary>
        ''' Returns the BSTR preamble count of strings from the head of the memory buffer.
        ''' </summary>
        ''' <returns></returns>
        Private Function BCount() As Long
            If SafePtr Is Nothing OrElse SafePtr.handle = IntPtr.Zero Then Return 0

            Select Case SizeDescriptorLength
                Case 2
                    Return SafePtr.UShortAt(0)
                Case 4
                    Return SafePtr.UIntegerAt(0)
                Case 8
                    Return SafePtr.ULongAt(0)

                Case Else
                    Return 0

            End Select

        End Function

        ''' <summary>
        ''' Returns the length of the buffer of the StringBlob, in bytes.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property ByteLength As Long
            Get
                Return _mem.Length
            End Get
        End Property

        ''' <summary>
        ''' Returns the memory handle for the StringBlob
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Handle As IntPtr
            Get
                Return _mem.handle
            End Get
        End Property

        ''' <summary>
        ''' Returns the string at the specified index.
        ''' </summary>
        ''' <param name="index">The index of the string.</param>
        ''' <returns></returns>
        Default Public ReadOnly Property StringAt(index As Integer) As String
            Get
                If index >= Count Then
                    Throw New ArgumentOutOfRangeException
                End If

                If _lpwstr Then
                    Dim lpsz As IntPtr
                    Dim l2 As IntPtr
                    lpsz = _index(index)
                    l2 = lpsz
                    Dim ch As Char = ChrW(0)
                    Dim i As Integer = 0

                    Do
                        charAtget(ch, l2)
                        l2 += 2
                        i += 1
                    Loop Until ch = ChrW(0)

                    StringAt = New String(ChrW(0), i)
                    QuickCopyObject(Of String)(StringAt, lpsz, CUInt(i * 2))

                Else
                    Dim lpsz As IntPtr
                    Dim mm As MemPtr

                    lpsz = _index(index)
                    mm = lpsz

                    Dim i As Long

                    Select Case SizeDescriptorLength
                        Case 2
                            i = mm.UShortAt(0)
                        Case 4
                            i = mm.UIntegerAt(0)
                        Case 8
                            i = mm.LongAt(0)
                    End Select

                    lpsz += SizeDescriptorLength
                    StringAt = New String(ChrW(0), i)
                    QuickCopyObject(Of String)(StringAt, lpsz, CUInt(i * 2))
                End If
            End Get
            'Set(value As String)
            '    ChangeString(index, value)
            'End Set
        End Property

        ''' <summary>
        ''' Returns the absolute byte offset of the string at the specified index.
        ''' </summary>
        ''' <param name="index">The index of the string.</param>
        ''' <returns></returns>
        Public ReadOnly Property ByteIndexOf(index As Integer) As IntPtr
            Get
                If index >= Count Then
                    Throw New ArgumentOutOfRangeException
                End If

                If _dynamic Then Refresh()
                Return _index(index)
            End Get
        End Property

        ''' <summary>
        ''' Recounts the strings and updates all preamble data and metadata.
        ''' </summary>
        ''' <returns>An array of IntPtr's to the absolute memory addresses of all strings.</returns>
        Public Function Refresh() As IntPtr()

            Dim c As Integer = GetCount(),
                by() As IntPtr

            ReDim by(c - 1)

            If _lpwstr Then
                Dim lpsz As New LPWSTR
                lpsz._ptr = _mem

                For l = 0 To c - 1
                    by(l) = lpsz._ptr.ToInt64
                    lpsz._ptr += (lpsz.Length * 2) + 2
                Next

                Refresh = by
                GC.SuppressFinalize(lpsz)

            Else
                Dim lpsz As New BSTR
                lpsz._preamble = SizeDescriptorLength
                lpsz._ptr = _mem.handle + SizeDescriptorLength

                For l = 0 To c - 1
                    by(l) = lpsz._ptr.ToInt64
                    lpsz._ptr += (lpsz.Length * 2) + lpsz._preamble
                Next

                Refresh = by
                GC.SuppressFinalize(lpsz)

            End If
            _index = by

        End Function

        ''' <summary>
        ''' Returns the current count of strings, walking the buffer, if necessary.
        ''' </summary>
        ''' <returns></returns>
        Public Function GetCount() As Integer
            Dim l As Integer = 0

            If SafePtr.handle = IntPtr.Zero Then Return 0

            If _lpwstr Then
                Dim lpsz As New LPWSTR
                Dim m As MemPtr
                lpsz._ptr = _mem.handle

                Do
                    If lpsz.Length Then l += 1
                    lpsz._ptr += (lpsz.Length * 2) + 2
                    m = lpsz._ptr
                    If m.ShortAt(0) = 0 Then Exit Do
                Loop

                GC.SuppressFinalize(lpsz)
            Else
                Select Case SizeDescriptorLength
                    Case 2
                        l = _mem.UShortAt(0)
                    Case 4
                        l = _mem.UIntegerAt(0)
                    Case 8
                        l = _mem.LongAt(0)

                End Select
            End If

            GetCount = l
            _count = l
        End Function

        ''' <summary>
        ''' Returns an array of all strings in the StringBlob
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Strings() As String()
            Get
                Return ToStringArray(Me)
            End Get
        End Property

        ''' <summary>
        ''' Returns an array of all byte indexes for all strings in the string blob.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property ByteIndices() As IntPtr()
            Get
                If _dynamic Then ByteIndices = Refresh() Else ByteIndices = _index
            End Get
        End Property

        ''' <summary>
        ''' Replace the string at the specified index with the specified string.
        ''' </summary>
        ''' <param name="index">Index of string to replace.</param>
        ''' <param name="s">The replacement string.</param>
        Public Sub ChangeString(index As Integer, s As String)
            Dim la As Integer = LengthAt(index) * 2
            Dim newlength As Integer = s.Length * 2

            If newlength = la Then Return

            Dim b As Long = _index(index).ToInt64 - SafePtr.handle.ToInt64

            Dim dif As Long = newlength - la

            If Not _lpwstr Then
                byteArrAtset(IntPtr.Add(SafePtr.handle, b), BitConverter.GetBytes(CLng(s.Length)), SizeDescriptorLength)
                b += SizeDescriptorLength
            End If

            If dif > 0 Then
                _mem.PushOut(b, dif)
            Else
                _mem.PullIn(b, -dif)
            End If

            QuickCopyObject(Of String)(IntPtr.Add(SafePtr.handle, b), s, CUInt(newlength))
            Refresh()

        End Sub

        ''' <summary>
        ''' Returns the length of the string at the specified index.
        ''' </summary>
        ''' <param name="index">The length of the specified string.</param>
        ''' <returns></returns>
        Public ReadOnly Property LengthAt(index As Integer) As Integer
            Get
                If index >= Count Then
                    Throw New ArgumentOutOfRangeException
                End If

                If _dynamic Then Refresh()

                If _lpwstr Then
                    Dim lpsz As New LPWSTR
                    lpsz._ptr = _index(index)

                    LengthAt = lpsz.Length
                    GC.SuppressFinalize(lpsz)

                Else
                    Dim lpsz As New BSTR
                    lpsz._ptr = _index(index)

                    LengthAt = lpsz.Length
                    GC.SuppressFinalize(lpsz)
                End If
            End Get
        End Property

        ''' <summary>
        ''' Removes the string at the specified index.
        ''' </summary>
        ''' <param name="index">Index of string to remove.</param>
        Public Sub RemoveAt(index As Integer)

            If index < 0 OrElse index > Count - 1 Then Return

            Dim b As IntPtr = _index(index).ToInt64 - SafePtr.handle.ToInt64

            If _lpwstr Then
                Dim c As Integer = LengthAt(index) * 2 + 2
                _mem.PullIn(b, c)
            Else
                Dim c As Integer = LengthAt(index) * 2 + SizeDescriptorLength
                _mem.PullIn(b, c)

                Select Case SizeDescriptorLength
                    Case 2
                        _mem.UShortAt(0) -= 1
                    Case 4
                        _mem.UIntegerAt(0) -= 1
                    Case 8
                        _mem.ULongAt(0) -= 1
                End Select
            End If

            Refresh()

        End Sub

        ''' <summary>
        ''' Insert a string at the specified index.
        ''' </summary>
        ''' <param name="s">String to insert.</param>
        ''' <param name="index">Index at which to insert the string.</param>
        Public Sub InsertAt(s As String, index As Integer)

            If index < 0 Then index = 0

            If index > (Count - 1) Then
                Me.AddString(s)
                Return
            End If

            Dim b As IntPtr = _index(index) - SafePtr.handle

            If _lpwstr Then
                Dim c As Integer = (s.Length * 2) + 2
                _mem.PushOut(b, c)
                QuickCopyObject(Of String)(b + SafePtr.handle, s, CUInt(s.Length * 2))
            Else
                Dim c As Integer = (s.Length * 2) + SizeDescriptorLength
                _mem.PushOut(b, c)
                byteArrAtset(b + SafePtr.handle, BitConverter.GetBytes(CLng(s.Length)), SizeDescriptorLength)
                b += SizeDescriptorLength
                QuickCopyObject(Of String)(b + SafePtr.handle, s, CUInt(s.Length * 2))

                Select Case SizeDescriptorLength
                    Case 2
                        _mem.UShortAt(0) += 1
                    Case 4
                        _mem.UIntegerAt(0) += 1
                    Case 8
                        _mem.ULongAt(0) += 1
                End Select

            End If

            Refresh()
        End Sub

        ''' <summary>
        ''' Gets or sets the Unicode Byte Order Mark preamble for the string collection.
        ''' </summary>
        ''' <returns></returns>
        Public Property BOM() As BOMTYPE
            Get
                Return _BOM.Type
            End Get
            Set(value As BOMTYPE)
                _BOM.SetBOM(value)
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the backing SafePtr object for this StringBlob.
        ''' </summary>
        ''' <returns></returns>
        Public Property SafePtr As SafePtr
            Get
                If Not _dynamic Then Throw New FieldAccessException("Field can only be accessed in dynamic mode.")
                Return _mem
            End Get
            Set(value As SafePtr)
                If Not _dynamic Then Throw New FieldAccessException("Field can only be accessed in dynamic mode.")
                _mem = value
            End Set
        End Property

        ''' <summary>
        ''' Add a string to the end of the collection.
        ''' </summary>
        ''' <param name="value">The string to add.</param>
        Public Sub AddString(value As String)
            Dim ol As Integer

            If _lpwstr Then
                ol = _mem.Length - 2
                If ol <= 0 Then
                    _mem.Length = 2
                    ol = 0
                End If
                _mem.Length += value.Length + 2
                QuickCopyObject(Of String)(IntPtr.Add(_mem.handle, ol), value, CUInt(value.Length * 2))
            Else
                Dim l As Integer = SizeDescriptorLength + (value.Length * 2)

                If _mem.Length = 0 Then _mem.Length = SizeDescriptorLength
                ol = _mem.Length

                Select Case SizeDescriptorLength
                    Case 1
                        _mem.ByteAt(0) += 1
                    Case 2
                        _mem.ShortAt(0) += 1
                    Case 4
                        _mem.UIntegerAt(0) += 1

                End Select

                _mem.Length += l

                Dim ap As IntPtr,
                    at As IntPtr

                ap = IntPtr.Add(_mem.handle, ol)
                at = IntPtr.Add(ap, SizeDescriptorLength)

                Dim mm As MemPtr = ap


                Select Case SizeDescriptorLength
                    Case 2
                        mm.UShortAt(0) = value.Length
                    Case 4
                        mm.UIntegerAt(0) = value.Length
                    Case 8
                        mm.LongAt(0) = value.Length
                End Select

                QuickCopyObject(Of String)(at, value, CUInt(value.Length * 2))
            End If

            If Not _dynamic Then
                ReDim _index(_count)
                _index(_count) = IntPtr.Add(_mem.handle, ol)
                _count += 1
            End If

        End Sub

        ''' <summary>
        ''' Add an array of strings to the end of the collection.
        ''' </summary>
        ''' <param name="values">Array of strings.</param>
        Public Sub AddStrings(values() As String)
            If values Is Nothing Then Return
            Dim sdl As Integer = SizeDescriptorLength

            Dim dl As Integer = sdl * values.Length

            For Each s In values
                dl += (s.Length * 2)
            Next

            If _lpwstr Then
                dl += (2 * values.Length)
            End If
            Dim sl As Long

            Dim sp As IntPtr
            Dim oldLen As Long

            If SafePtr.Length <= 0 Then
                SafePtr.Length = sdl
            End If

            oldLen = SafePtr.Length

            Select Case sdl
                Case 2
                    SafePtr.UShortAt(0) += values.Length
                Case 4
                    SafePtr.UIntegerAt(0) += values.Length
                Case 8
                    SafePtr.ULongAt(0) += values.Length
            End Select

            SafePtr.Length += dl
            sp = SafePtr.handle + oldLen

            Dim oi As Integer
            If _index Is Nothing Then
                oi = 0
                ReDim _index(values.Length - 1)
            Else
                oi = _index.Length
                ReDim Preserve _index(_index.Length + (values.Length - 1))
            End If

            If _lpwstr Then
                For Each s In values
                    sl = s.Length

                    _index(oi) = sp
                    oi += 1

                    QuickCopyObject(Of String)(sp, s, CUInt(sl + sl))
                    sp += sl + sl

                    Dim mm As MemPtr = sp
                    mm.ShortAt(0) = 0
                    sp += 2
                Next
            Else
                For Each s In values
                    sl = s.Length

                    _index(oi) = sp
                    oi += 1

                    Dim mm As MemPtr = sp

                    Select Case sdl
                        Case 2
                            mm.UShortAt(0) = sl
                        Case 4
                            mm.UIntegerAt(0) = sl
                        Case 8
                            mm.LongAt(0) = sl
                    End Select

                    sp += sdl

                    QuickCopyObject(Of String)(sp, s, CUInt(sl + sl))
                    sp += sl + sl
                Next
            End If

            _count = _index.Length
        End Sub

        ''' <summary>
        ''' Formats the StringBlob into a single string using the specified criteria.
        ''' </summary>
        ''' <param name="format">A combination of <see cref="StringBlobFormats"/> values that indicate how the string will be rendered.</param>
        ''' <param name="customFormat">(NOT IMPLEMENTED)</param>
        ''' <returns></returns>
        Public Function ToFormattedString(format As StringBlobFormats, Optional customFormat As String = "") As String

            Dim c As Long,
                d As Long,
                x As Long = 0

            Dim sb As New StringBuilder

            If _dynamic Then Refresh()



            If format And StringBlobFormats.Commas Then
                c += (_count - 1) * 2
            End If

            If format And StringBlobFormats.Quoted Then
                c += 4 * _count
            End If

            If format And StringBlobFormats.CrLf Then
                c += 4 * _count
            End If

            If format And StringBlobFormats.Spaced Then
                c += (_count - 1) * 2
            End If

            d = _mem.Length
            If _lpwstr Then
                d -= 2
                d -= (2 * _count)
            Else
                d -= SizeDescriptorLength
                d -= (SizeDescriptorLength * _count)
            End If

            c += d
            sb.Capacity = c

            If _lpwstr Then
                Dim lpstr As New LPWSTR

                For i = 0 To _count - 1

                    If i > 0 Then
                        If format And StringBlobFormats.Commas Then
                            sb.Append(",")
                            x += 1
                        End If

                        If format And StringBlobFormats.Spaced Then
                            sb.Append(" ")
                            x += 1
                        End If
                    End If

                    If format And StringBlobFormats.Quoted Then
                        sb.Append("""")
                        x += 1
                    End If

                    lpstr._ptr = _index(i).ToInt64
                    sb.Append(lpstr.Text)
                    x += lpstr.Length

                    If format And StringBlobFormats.Quoted Then
                        sb.Append("""")
                        x += 1
                    End If

                    If format And StringBlobFormats.CrLf Then
                        sb.Append(vbCr)
                        x += 1
                        sb.Append(vbLf)
                        x += 1
                    End If

                Next
                GC.SuppressFinalize(lpstr)

            Else
                Dim lpstr As New BSTR
                lpstr._preamble = SizeDescriptorLength
                For i = 0 To _count - 1

                    If i > 0 Then
                        If format And StringBlobFormats.Commas Then
                            sb.Append(",")
                            x += 1
                        End If

                        If format And StringBlobFormats.Spaced Then
                            sb.Append(" ")
                            x += 1
                        End If
                    End If

                    If format And StringBlobFormats.Quoted Then
                        sb.Append("""")
                        x += 1
                    End If

                    lpstr._ptr = _index(i).ToInt64
                    sb.Append(lpstr.Text)
                    x += lpstr.Length

                    If format And StringBlobFormats.Quoted Then
                        sb.Append("""")
                        x += 1
                    End If

                    If format And StringBlobFormats.CrLf Then
                        sb.Append(vbCr)
                        x += 1
                        sb.Append(vbLf)
                        x += 1
                    End If

                Next
                GC.SuppressFinalize(lpstr)

            End If

            Return sb.ToString
        End Function

        ''' <summary>
        ''' Returns a plain-text reprentation of the string blob, with no formatting.
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function ToString() As String
            Return ToFormattedString(StringBlobFormats.None)
        End Function

        ''' <summary>
        ''' Returns the specified StringBlob as a byte array.
        ''' </summary>
        ''' <param name="operand">Object whose bytes to retrieve.</param>
        ''' <returns></returns>
        Public Shared Function GetBytes(operand As StringBlob) As Byte()
            Return operand._mem.GrabBytes(0, operand._mem.Length)
        End Function


        ''' <summary>
        ''' Sets the specified byte array into the specified StringBlob.
        ''' This will overwrite the contents in the StringBlob.
        ''' </summary>
        ''' <param name="operand">Target object.</param>
        ''' <param name="value">Byte array.</param>
        Public Shared Sub SetBytes(ByRef operand As StringBlob, value() As Byte)
            If operand Is Nothing Then
                operand = New StringBlob
            End If

            operand._mem.Alloc(value.Length)
            operand._mem.SetBytes(CIntPtr(0), value)
        End Sub

        ''' <summary>
        ''' Converts the StringBlob into an array of characters.
        ''' </summary>
        ''' <param name="operand"></param>
        ''' <returns></returns>
        Public Shared Function ToCharArray(operand As StringBlob) As Char()
            Return CType(operand.ToString, Char())
        End Function

        ''' <summary>
        ''' Converts an array of characters into a StringBlob.
        ''' </summary>
        ''' <param name="operand"></param>
        ''' <returns></returns>
        Public Shared Function FromCharArray(operand() As Char) As StringBlob
            Dim sb As New StringBlob
            sb._mem = operand
            Return sb
        End Function

        ''' <summary>
        ''' Converts the StringBlob into a byte array.
        ''' </summary>
        ''' <param name="operand"></param>
        ''' <returns></returns>
        Public Shared Function ToByteArray(operand As StringBlob) As Byte()
            Return operand._mem.GrabBytes
        End Function

        ''' <summary>
        ''' Converts a byte array into a string blob, with the optional size-descriptor size.
        ''' </summary>
        ''' <param name="operand">Byte array to copy.</param>
        ''' <param name="sizeDesc">Size descriptor kind.</param>
        ''' <returns></returns>
        Public Overloads Shared Function FromByteArray(operand As Byte(), Optional sizeDesc As BlobOrdinalTypes = BlobOrdinalTypes.Integer) As StringBlob
            Dim sb As New StringBlob
            sb.SizeDescriptorType = sizeDesc
            sb._mem.SetBytes(CIntPtr(0), operand)
            sb.GetCount()
            Return sb
        End Function

        ''' <summary>
        ''' Gets a string array for the specified StringBlob
        ''' </summary>
        ''' <param name="operand">StringBlob whose strings to return.</param>
        ''' <returns></returns>
        Public Overloads Shared Function ToStringArray(operand As StringBlob) As String()
            ToStringArray = (New StringBlobEnumerator(operand).AllStrings)
        End Function

        ''' <summary>
        ''' Copies the strings from the StringBlob into the specified array starting at the specified index.
        ''' </summary>
        ''' <param name="sb">Source object.</param>
        ''' <param name="array">Destination array.</param>
        ''' <param name="startIndex">Index within array to begin copying.</param>
        Public Overloads Shared Sub ToStringArray(sb As StringBlob, array() As String, startIndex As Integer)
            Dim c As Integer = startIndex

            For Each n As String In sb
                array(c) = n
                c += 1
            Next

        End Sub

        ''' <summary>
        ''' Creates a StringBlob from a string array.
        ''' </summary>
        ''' <param name="operand">Source array.</param>
        ''' <returns></returns>
        Public Shared Function FromStringArray(operand() As String) As StringBlob
            Dim sb As New StringBlob
            sb.AddStrings(operand)
            Return sb
        End Function

        ''' <summary>
        ''' Completely clear the object and free its resources.
        ''' </summary>
        Public Sub Clear()
            _mem.Length = 0
        End Sub

        Public Shared Operator &(operand1 As StringBlob, operand2 As String()) As StringBlob

            If operand2 Is Nothing OrElse operand2.Length = 0 Then Return operand1
            Dim i As Integer, _
                c As Integer = operand2.Length - 1

            For i = 0 To c
                operand1.AddString(operand2(i))
            Next

            Return operand1
        End Operator

        Public Shared Operator &(operand1 As StringBlob, operand2 As String) As StringBlob

            If operand2 Is Nothing OrElse operand2.Length = 0 Then Return operand1
            operand1.AddString(operand2)

            Return operand1
        End Operator

        Public Shared Operator &(operand1 As Blob, operand2 As StringBlob) As Blob

            Dim d As Long = operand1.Length
            Dim c As Long = operand1.Length + operand2.ByteLength

            operand1.Length += c

            If (c - d) < UInt32.MaxValue Then
                MemCpy(operand1.DangerousGetHandle + d, operand2.Handle, (c - d))
            Else
                CopyMemory(operand1.DangerousGetHandle + d, operand2.Handle, CIntPtr(c - d))
            End If
            operand1.BlobType = BlobTypes.Char

            Return operand1
        End Operator


        Public Shared Narrowing Operator CType(operand As String()) As StringBlob
            Return FromStringArray(operand)
        End Operator

        Public Shared Widening Operator CType(operand As StringBlob) As String()
            Return ToStringArray(operand)
        End Operator

        Public Shared Narrowing Operator CType(operand As String) As StringBlob
            Return FromCharArray(operand)
        End Operator

        Public Shared Widening Operator CType(operand As StringBlob) As String
            Return ToCharArray(operand)
        End Operator

        Public Shared Narrowing Operator CType(operand As Char()) As StringBlob
            Return FromCharArray(operand)
        End Operator

        Public Shared Widening Operator CType(operand As StringBlob) As Char()
            Return ToCharArray(operand)
        End Operator

        Public Shared Narrowing Operator CType(operand As Byte()) As StringBlob
            Return FromByteArray(operand)
        End Operator

        Public Shared Widening Operator CType(operand As StringBlob) As Byte()
            Return ToByteArray(operand)
        End Operator

        Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
            Return New StringBlobEnumerator(Me)
        End Function

        Public Function Clone() As Object Implements System.ICloneable.Clone
            Dim sb As New StringBlob
            sb.LpWStr = Me.LpWStr
            sb.Dynamic = Me.Dynamic
            sb._mem.Length = _mem.Length
            CopyMemory(sb._mem.handle, _mem.handle, CIntPtr(_mem.Length))
            sb.Refresh()
            Return sb
        End Function

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    If _mem IsNot Nothing Then _mem.Dispose()
                End If
            End If

            Me.disposedValue = True
        End Sub

        Protected Overrides Sub Finalize()
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
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

#End Region

End Namespace