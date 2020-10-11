'' ************************************************* ''
'' DataTools Visual Basic Utility Library 
''
'' Module: Blob
''         Exhaustive Memory Manipulation Object
''         With adaptive buffering.
'' 
'' Copyright (C) 2011-2020 Nathan Moschkin
'' All Rights Reserved
''
'' Licensed Under the Microsoft Public License   
'' ************************************************* ''

Option Explicit On
Option Compare Binary
Option Strict Off

Imports System.Text
Imports System.IO

Imports System.ComponentModel
Imports System.Runtime.InteropServices
Imports System.Numerics

Imports System.Drawing
Imports System.Runtime.CompilerServices
Imports DataTools.Memory.Internal
Imports DataTools.BitStream
Imports DataTools.Strings

Namespace Memory

    '' *********************************
    '' *********************************
    '' Blob
    '' *********************************
    '' *********************************

#Region "Blob"

    ''' <summary>
    ''' Exhaustive multipurpose memory manipulation class.
    ''' </summary>
    ''' <remarks></remarks>
    <TypeConverter(GetType(BlobConverter))>
    Public Class Blob
        Inherits SafeHandle

        Implements IEquatable(Of SafeHandle),
                   IEquatable(Of UIntPtr),
                   IEquatable(Of MemPtr),
                   IEquatable(Of IntPtr),
                   IEquatable(Of SafePtr),
                   IEquatable(Of Blob)

#Region "Friend Shared Variables"

        Friend Shared ReadOnly Converter As New BlobConverter

        Friend Shared defaultHeap As IntPtr = GetProcessHeap()
        Friend Shared Types() As System.Type
        Friend Shared ArrayTypes() As System.Type
        Friend Shared DumbInstance() As Object

#End Region

#Region "Public Shared Variables"

        ''' <summary>
        ''' Gets the system page size.
        ''' </summary>
        Public Shared ReadOnly SystemPageSize = SystemInformation.SystemInfo.dwPageSize

        ''' <summary>
        ''' Sets the global behavior for attempting to parse string input for content.
        ''' </summary>
        ''' <returns></returns>
        Public Shared Property ParseStrings As Boolean = False

#End Region

#Region "Private Variables"

        ''' <summary>
        ''' Represents the length of the buffer as presented to the outside world, regardless
        ''' of allocation mode.
        ''' </summary>
        Private virtLen As Long = 0

        ''' <summary>
        ''' Represents the actual length of the allocated buffer, in memory.
        ''' </summary>
        Private actualLen As Long = 0

        ''' <summary>
        ''' represents the default amount by which to extend the buffer in buffering mode.
        ''' </summary>
        Private _bufferExtend As Long = SystemInformation.SystemInfo.dwPageSize

        ''' <summary>
        ''' Represents a value indicating that buffering mode is active.
        ''' </summary>
        Private _inBuffer As Boolean = False

        ''' <summary>
        ''' Represents a value indicating that the BufferExtend property will be doubled
        ''' Whenever a buffer reallocation occurs (not usually recommended.)
        ''' </summary>
        Private _AutoDouble As Boolean = False

        ''' <summary>
        ''' Represents the type of data this blob represents.
        ''' </summary>
        Private _BlobType As BlobTypes

        ''' <summary>
        ''' Indicates that string concatenations will not append a ChrW(0) to the end.
        ''' </summary>
        Private _StringCatNoNull As Boolean = False

        ''' <summary>
        ''' Maximum number of byte values to print in the ToString method before suspending that behavior.
        ''' </summary>
        Private _MaxBlobPrintNum As Integer = 128

        ''' <summary>
        ''' Indicates that the blob is locked.
        ''' </summary>
        Private _Locked As Boolean = False

        ''' <summary>
        ''' The handle allocation type.
        ''' </summary>
        Private _MemType As MemAllocType = MemAllocType.Invalid

        ''' <summary>
        ''' Basically the seek/position of the buffer in the Clip stream.
        ''' </summary>
        Private _ClipNext As Integer = 0

#End Region

#Region "Friend Variables"

        ''' <summary>
        ''' contains the length of the primitive type (if applicable)
        ''' </summary>
        Friend TypeLen As Integer

        ''' <summary>
        ''' Contains the system type representation of the blob's contents.
        ''' </summary>
        Friend Type As System.Type

        ''' <summary>
        ''' Indicates that we own the handle.
        ''' </summary>
        Friend fOwn As Boolean = True

        ''' <summary>
        ''' The heap object of the heap that the blob is currently allocated on.
        ''' </summary>
        Friend activeHeap As BlobHeap = Nothing

        ''' <summary>
        ''' The pointer to the active heap, either a private heap or the process heap.
        ''' Default is process heap.
        ''' </summary>
        Friend hHeap As IntPtr = defaultHeap

#End Region

#Region "Heap"

        ''' <summary>
        ''' Gets the heap that the current blob is created on.
        ''' </summary>
        ''' <returns></returns>
        Friend ReadOnly Property Heap As BlobHeap
            Get
                If activeHeap Is Nothing Then Return BlobHeap.DefaultHeap
                Return activeHeap
            End Get
        End Property

        ''' <summary>
        ''' Moves the specified blob to the specified heap, copying and freeing memory, if necessary.
        ''' </summary>
        ''' <param name="bl">The blob</param>
        ''' <param name="heap">The heap</param>
        ''' <returns>True if successful.</returns>
        Public Shared Function MoveToHeap(bl As Blob, heap As BlobHeap) As Boolean

            If Threading.Monitor.TryEnter(bl) Then
                If Threading.Monitor.TryEnter(heap) Then

                    If bl.Length = 0 Then

                        If bl.activeHeap IsNot Nothing Then
                            bl.activeHeap.RemoveSelf(bl)
                        End If

                        bl.virtLen = 0
                        bl.actualLen = 0
                        bl.hHeap = heap.DangerousGetHandle
                        bl.activeHeap = heap
                        bl.activeHeap.AddSelf(bl)

                    Else
                        Dim ptr As IntPtr = HeapAlloc(heap.DangerousGetHandle, 0, bl.actualLen)

                        If ptr Then
                            If bl.Length > UInt32.MaxValue Then
                                n_memcpy(ptr, bl.handle, bl.virtLen)
                            Else
                                MemCpy(ptr, bl.handle, bl.virtLen)
                            End If

                            If HeapFree(bl.hHeap, 0, bl.handle) Then
                                If bl.activeHeap IsNot Nothing Then
                                    bl.activeHeap.RemoveSelf(bl)
                                End If

                                bl.handle = ptr
                                bl.hHeap = heap.DangerousGetHandle
                                bl.activeHeap = heap

                                bl.activeHeap.AddSelf(bl)
                            Else
                                Threading.Monitor.Exit(heap)
                                Threading.Monitor.Exit(bl)
                                Return False
                            End If
                        Else
                            Threading.Monitor.Exit(heap)
                            Threading.Monitor.Exit(bl)
                            Return False
                        End If

                    End If

                    Threading.Monitor.Exit(heap)
                    Threading.Monitor.Exit(bl)
                    Return True
                Else
                    Threading.Monitor.Exit(bl)
                    Return False

                End If
            Else
                Return False
            End If

        End Function

#End Region

#Region "SafeHandle implementation"

        Public Overrides ReadOnly Property IsInvalid As Boolean
            Get
                Return (handle = 0)
            End Get
        End Property

        Protected Overrides Function ReleaseHandle() As Boolean
            If activeHeap IsNot Nothing Then
                activeHeap.RemoveSelf(Me)
            End If
            ReleaseHandle = Free()
        End Function

#End Region

#Region "Instantiation"

        Public Sub New()
            MyBase.New(0, True)
        End Sub

        ''' <summary>
        ''' Initialize a new blob from a string.
        ''' </summary>
        ''' <param name="value"></param>
        ''' <remarks></remarks>
        Public Sub New(value As String)
            MyBase.New(0, True)

            Dim mm As MemPtr = value
            handle = mm.Handle
        End Sub

        ''' <summary>
        ''' Initialize a new blob with a pre-existing memory handle of the specified kind.
        ''' </summary>
        ''' <param name="value">Memory handle.</param>
        ''' <param name="kind">Kind of memory handle.</param>
        ''' <param name="fOwn">Whether or not to own the handle and release on object destruction.</param>
        ''' <remarks></remarks>
        Public Sub New(value As IntPtr, kind As MemAllocType, Optional fOwn As Boolean = True)
            MyBase.New(value, fOwn)

            Me.fOwn = fOwn
            _MemType = kind
        End Sub

        Public Sub New(bytes() As Byte)
            MyBase.New(0, True)

            Alloc(bytes.Length)
            MemCpy(handle, bytes, bytes.Length)
        End Sub

        ''' <summary>
        ''' Initialize a new blob from the specified structure.
        ''' </summary>
        ''' <param name="struct"></param>
        ''' <remarks></remarks>
        Public Sub New(struct As Object)
            MyBase.New(0, True)

            If struct.GetType.IsValueType = False Then Throw New ArgumentException("Must be a structure")

            Dim l As Integer = Marshal.SizeOf(struct)
            If Alloc(l) Then
                Marshal.StructureToPtr(struct, handle, False)
            Else
                Throw New OutOfMemoryException("Could not initialize memory buffer.")
            End If
        End Sub


        ''' <summary>
        ''' Initialize a new blob by copying an existing blob
        ''' </summary>
        ''' <param name="blob"></param>
        ''' <remarks></remarks>
        Public Sub New(blob As Blob)
            MyBase.New(0, True)

            Me.Alloc(blob.Length)

            If blob.Length > UInt32.MaxValue Then
                n_memcpy(Me.handle, blob.handle, blob.Length)
            Else
                MemCpy(Me.handle, blob.handle, blob.Length)
            End If

            TypeLen = blob.TypeLen
            _BlobType = blob._BlobType

            Align()
        End Sub

#End Region '' Instantiation

#Region "Private Shared Functions"

        Private Shared Sub InitTypeSize()
            ReDim Types(20)
            ReDim ArrayTypes(20)
            ReDim DumbInstance(20)

            Types(0) = GetType(SByte)
            Types(1) = GetType(Byte)
            Types(2) = GetType(Short)
            Types(3) = GetType(UShort)
            Types(4) = GetType(Integer)
            Types(5) = GetType(UInteger)
            Types(6) = GetType(Long)
            Types(7) = GetType(ULong)
            Types(8) = GetType(BigInteger)
            Types(9) = GetType(Single)
            Types(10) = GetType(Double)
            Types(11) = GetType(Decimal)
            Types(12) = GetType(Date)
            Types(13) = GetType(Char)
            Types(14) = GetType(String)
            Types(15) = GetType(Guid)
            Types(16) = GetType(System.Drawing.Image)
            Types(17) = GetType(System.Drawing.Color)
            Types(18) = GetType(WormRecord)
            Types(19) = GetType(String)
            Types(20) = GetType(Boolean)

            Dim i As BlobTypes,
                o As Object

            For i = 0 To BlobConst.UBound

                Select Case i
                    Case BlobTypes.BigInteger, BlobTypes.Image, BlobTypes.String, BlobTypes.NtString, BlobTypes.WormRecord
                        Continue For

                    Case Else
                        o = Array.CreateInstance(Types(i), 1)
                        ArrayTypes(i) = o.GetType
                        DumbInstance(i) = o(0)
                        Erase o

                End Select

            Next

        End Sub

        Shared Sub New()
            InitTypeSize()
        End Sub

#End Region

#Region "Public Shared Functions"

        ''' <summary>
        ''' Converts a System.Type to a BlobTypes value
        ''' </summary>
        <Description("Converts a System.Type to a BlobTypes value")>
        Public Shared Function TypeToBlobType(type As System.Type) As BlobTypes
            Dim t As Type = If(type.IsArray, type.GetElementType, type)
            Dim i As BlobTypes

            If type Is GetType(Boolean) Then Return BlobTypes.Boolean

            If t.IsEnum = True Then
                t = t.GetEnumUnderlyingType
            ElseIf t.IsClass AndAlso (t.BaseType = GetType(System.Drawing.Image) OrElse t = GetType(System.Drawing.Image)) Then
                t = GetType(System.Drawing.Image)
            End If

            i = BlobTypes.Invalid

            For i = 0 To BlobConst.UBound
                If Types(i) = t OrElse Types(i).ToString = t.ToString.Replace("&", "") Then
                    Exit For
                End If
            Next

            If i > BlobConst.UBound Then Return BlobTypes.Invalid

            If type.IsArray Then
                Select Case i
                    '' actual arrays of strings and BigIntegers are not supported because their size is not intrinsically knowable.
                    Case BlobTypes.BigInteger, BlobTypes.String, BlobTypes.NtString, BlobTypes.Image
                        Return BlobTypes.Invalid

                End Select
            End If

            Return i
        End Function

        ''' <summary>
        ''' Converts a BlobTypes value to a System.Type
        ''' </summary>
        <Description("Converts a BlobTypes value to a System.Type")>
        Public Shared Function BlobTypeToType(type As BlobTypes) As System.Type
            If type = BlobTypes.Boolean Then Return GetType(Boolean)
            If type = BlobTypes.Invalid Then Return GetType(Byte())
            If type = BlobTypes.NtString Then Return Types(BlobTypes.String)
            Return Types(type)
        End Function

        ''' <summary>
        ''' Returns the element size of the specified BlobType
        ''' </summary>
        <Description("Returns the element size of the specified BlobType")>
        Public Shared Function BlobTypeSize(type As BlobTypes) As Integer
            '' returns 0 if the size is indeterminate

            Select Case type

                Case BlobTypes.Byte, BlobTypes.SByte, BlobTypes.BigInteger, BlobTypes.Image, BlobTypes.WormRecord, BlobTypes.Boolean
                    Return 1

                Case BlobTypes.Short, BlobTypes.UShort
                    Return 2

                Case BlobTypes.Char, BlobTypes.String, BlobTypes.NtString
                    Dim ch As Char
                    Return Len(ch)

                Case BlobTypes.Integer, BlobTypes.UInteger, BlobTypes.Single, BlobTypes.Color
                    Return 4

                Case BlobTypes.Long, BlobTypes.ULong, BlobTypes.Double, BlobTypes.Date
                    Return 8

                Case BlobTypes.Decimal, BlobTypes.Guid
                    Return 16

                Case Else
                    Return 1

            End Select
        End Function

        ''' <summary>
        ''' Rerank one blob to match another blob, using the maximum type length of the two.
        ''' </summary>
        <Description("Rerank one blob to match another blob, using the maximum type length of the two.")>
        Public Overloads Shared Sub ReRankMax(ByRef blob1 As Blob, blob2 As Blob)

            Dim rs1 = BlobTypeSize(blob1.BlobType)
            Dim rs2 = BlobTypeSize(blob2.BlobType)

            If rs1 = rs2 Then
                '' they are the same size, we'll make them the same type.
                blob2.Type = blob1.Type
                Return
            End If

            If rs1 > rs2 Then
                ReRank(blob2, blob1.BlobType)
                blob2.BlobType = blob1.BlobType
            Else
                ReRank(blob1, blob1.BlobType)
                blob1.BlobType = blob2.BlobType
            End If

        End Sub

        ''' <summary>
        ''' Attempts to re-rank the specified blob to the maximum of two types, increasing the type length as needed.  This is not a coercion; the array, itself, is redimmed.
        ''' </summary>
        <Description("Attempts to re-rank the specified blob to the maximum of two types, increasing the type length as needed.  This is not a coercion; the array, itself, is redimmed.")>
        Public Overloads Shared Sub ReRankMax(ByRef blob As Blob, type1 As System.Type, type2 As System.Type)
            ReRankMax(blob, TypeToBlobType(type1), TypeToBlobType(type2))
        End Sub

        ''' <summary>
        ''' Attempts to re-rank the specified blob to the maximum of two types, increasing the type length as needed.  This is not a coercion; the array, itself, is redimmed.
        ''' </summary>
        <Description("Attempts to re-rank the specified blob to the maximum of two types, increasing the type length as needed.  This is not a coercion; the array, itself, is redimmed.")>
        Public Overloads Shared Sub ReRankMax(ByRef blob As Blob, type1 As BlobTypes, type2 As BlobTypes)
            ReRank(blob, CInt(Math.Max(type1, type2)))
        End Sub

        ''' <summary>
        ''' Rerank one blob to match another blob, using the minimum type length of the two.
        ''' </summary>
        <Description("Rerank one blob to match another blob, using the minimum type length of the two.")>
        Public Overloads Shared Sub ReRankMin(ByRef blob1 As Blob, blob2 As Blob)

            Dim rs1 = BlobTypeSize(blob1.BlobType)
            Dim rs2 = BlobTypeSize(blob2.BlobType)

            If rs1 = rs2 Then Return

            If rs1 = rs2 Then
                '' they are the same size, we'll make them the same type.
                blob2.Type = blob1.Type
                Return
            End If

            If rs1 > rs2 Then
                ReRank(blob2, blob1.BlobType)
                blob2.Type = blob1.Type
            Else
                ReRank(blob1, blob1.BlobType)
                blob1.Type = blob2.Type
            End If

        End Sub

        ''' <summary>
        ''' Attempts to re-rank the specified blob to the minimum of two types, decreasing the type length as needed.  This is not a coercion; the array, itself, is redimmed.
        ''' </summary>
        <Description("Attempts to re-rank the specified blob to the minimum of two types, decreasing the type length as needed.  This is not a coercion; the array, itself, is redimmed.")>
        Public Overloads Shared Sub ReRankMin(ByRef blob As Blob, type1 As System.Type, type2 As System.Type)
            ReRankMin(blob, TypeToBlobType(type1), TypeToBlobType(type2))
        End Sub

        ''' <summary>
        ''' Attempts to re-rank the specified blob to the minimum of two types, decreasing the type length as needed.  This is not a coercion; the array, itself, is redimmed.
        ''' </summary>
        <Description("Attempts to re-rank the specified blob to the minimum of two types, decreasing the type length as needed.  This is not a coercion; the array, itself, is redimmed.")>
        Public Overloads Shared Sub ReRankMin(ByRef blob As Blob, type1 As BlobTypes, type2 As BlobTypes)
            ReRank(blob, CInt(Math.Min(type1, type2)))
        End Sub

        ''' <summary>
        ''' Attempts to re-rank the specified blob, increasing or decreasing the type length as needed.  This is not a coercion; the array, itself, is redimmed.
        ''' </summary>
        <Description("Attempts to re-rank the specified blob, increasing or decreasing the type length as needed.  This is not a coercion; the array, itself, is redimmed.")>
        Public Overloads Shared Sub ReRank(ByRef blob As Blob, type As System.Type)
            ReRank(blob, TypeToBlobType(type))
        End Sub

        ''' <summary>
        ''' Attempts to re-rank the specified blob, increasing or decreasing the type length as needed.  This is not a coercion; the array, itself, is redimmed.
        ''' </summary>
        <Description("Attempts to re-rank the specified blob, increasing or decreasing the type length as needed.  This is not a coercion; the array, itself, is redimmed.")>
        Public Overloads Shared Sub ReRank(ByRef blob As Blob, type As BlobTypes)

            Select Case blob.BlobType

                Case BlobTypes.BigInteger, BlobTypes.Guid, BlobTypes.Invalid, BlobTypes.String, BlobTypes.NtString, BlobTypes.Char
                    Throw New ArgumentException("Blobs that do not contain purely numeric values cannot be reranked.  Call TypeAlign, first, to coerce the blob into a numeric type.")
                    Return

            End Select

            Select Case type

                Case BlobTypes.BigInteger, BlobTypes.Guid, BlobTypes.Invalid
                    Throw New ArgumentException("Cannot rerank to non numeric types.  Use TypeAlign, instead.")
                    Return

                Case BlobTypes.String, BlobTypes.NtString, BlobTypes.Char

                    If blob.BlobType = BlobTypes.Byte Then Exit Select

                    Throw New ArgumentException("Cannot rerank to non numeric types.  Use TypeAlign, instead.")
                    Return

            End Select

            Dim rs1 As Integer = BlobTypeSize(blob.BlobType)
            Dim rs2 As Integer = BlobTypeSize(type)

            If rs1 = rs2 Then
                blob.TypeAlign(BlobTypeToType(type))
                Return
            End If

            Dim c As Integer = blob.Count,
                e As Integer,
                d As Integer = 0,
                f As Integer = 0,
                i As Integer

            Dim cb As New Blob(blob)

            e = Math.Min(rs1, rs2)

            cb.Length = c * rs2
            cb.BlobType = type

            c -= 1
            For i = 0 To c
                MemCpy(cb.handle + f, blob.handle + d, e)
                d += rs1
                f += rs2
            Next

            blob.handle = cb.handle
            cb.handle = 0

            blob.TypeAlign(BlobTypeToType(type))

        End Sub

        Public Shared Function CreateMemoryStream(blob As Blob) As System.IO.MemoryStream
            Return New System.IO.MemoryStream(CType(blob, Byte()))
        End Function

#End Region

#Region "Clip"

        ''' <summary>
        ''' Returns the start position of the next clip
        ''' </summary>
        <Description("Returns the start position of the next clip")>
        Public ReadOnly Property ClipNext As Integer
            Get
                Return _ClipNext
            End Get
        End Property

        ''' <summary>
        ''' Seeks to the specified clip position.
        ''' </summary>
        ''' <param name="position"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ClipSeek(position As Long) As Long
            If position >= Length Then
                _ClipNext = Length
            Else
                _ClipNext = position
            End If

            ClipSeek = _ClipNext
        End Function

        ''' <summary>
        ''' Returns a clip of the buffer as the given type
        ''' </summary>
        <Description("Returns a clip of the buffer as the given type")>
        Public Overloads Function Clip(length As Integer, type As System.Type) As Object
            Return Clip(_ClipNext, length, type)
        End Function

        ''' <summary>
        ''' Returns a clip of the buffer as the given type
        ''' </summary>
        <Description("Returns a clip of the buffer as the given type")>
        Public Overloads Function Clip(length As Integer, type As BlobTypes) As Object
            Return Clip(_ClipNext, length, type)
        End Function

        ''' <summary>
        ''' Returns a clip of the buffer as the given type
        ''' </summary>
        <Description("Returns a clip of the buffer as the given type")>
        Public Overloads Function Clip(startIndex As Integer, length As Integer, type As BlobTypes) As Object
            Return Clip(startIndex, length, BlobTypeToType(type))
        End Function

        ''' <summary>
        ''' Returns a clip of the buffer as the given type
        ''' </summary>
        <Description("Returns a clip of the buffer as the given type")>
        Public Overloads Function Clip(startIndex As Integer, length As Integer, type As System.Type) As Object

            If length = 0 AndAlso type = GetType(String) Then

                Clip = GrabString(startIndex)
                _ClipNext = startIndex + (CStr(Clip).Length * 2) + 2
                Exit Function
            End If

            If startIndex < 0 Then
                Throw New ArgumentOutOfRangeException("startIndex", "Index cannot be a negative number. If you need to substract from a pointer, use MemPtr, instead")
            ElseIf startIndex + length > Me.Length Then
                Throw New ArgumentOutOfRangeException("length", "Parameter exceeds buffer size.")
            End If

            _ClipNext = startIndex + length

            If type = GetType(WormRecord) Then
                Clip = CType(New Blob(GrabBytes(0, length)), WormRecord)
            Else
                Select Case type

                    Case GetType(String)
                        Clip = GrabString(startIndex, length >> 1)

                    Case GetType(Boolean)
                        If length > 1 Then
                            Clip = GrabBytes(startIndex, length)
                        Else
                            Clip = CBool(ByteAt(startIndex))
                        End If

                    Case GetType(Byte)
                        If length > 1 Then
                            Clip = GrabBytes(startIndex, length)
                        Else
                            Clip = ByteAt(startIndex)
                        End If

                    Case GetType(Short)
                        If length > 2 Then
                            Clip = GetShortArray(startIndex, length >> 1)
                        Else
                            Clip = ShortAtAbsolute(startIndex)
                        End If

                    Case GetType(UShort)
                        If length > 2 Then
                            Clip = GetUShortArray(startIndex, length >> 1)
                        Else
                            Clip = UShortAtAbsolute(startIndex)
                        End If

                    Case GetType(Integer)
                        If length > 4 Then
                            Clip = GetIntegerArray(startIndex, length >> 2)
                        Else
                            Clip = IntegerAtAbsolute(startIndex)
                        End If

                    Case GetType(UInteger)
                        If length > 4 Then
                            Clip = GetUIntegerArray(startIndex, length >> 2)
                        Else
                            Clip = UIntegerAtAbsolute(startIndex)
                        End If

                    Case GetType(Single)
                        If length > 4 Then
                            Clip = GetSingleArray(startIndex, length >> 2)
                        Else
                            Clip = SingleAtAbsolute(startIndex)
                        End If

                    Case GetType(Long)
                        If length > 8 Then
                            Clip = GetLongArray(startIndex, length >> 3)
                        Else
                            Clip = LongAtAbsolute(startIndex)
                        End If

                    Case GetType(ULong)
                        If length > 8 Then
                            Clip = GetULongArray(startIndex, length >> 3)
                        Else
                            Clip = ULongAtAbsolute(startIndex)
                        End If

                    Case GetType(Double)
                        If length > 8 Then
                            Clip = GetDoubleArray(startIndex, length >> 3)
                        Else
                            Clip = DoubleAtAbsolute(startIndex)
                        End If

                    Case GetType(Decimal)
                        If length > 16 Then
                            Clip = GetDecimalArray(startIndex, length >> 4)
                        Else
                            Clip = DecimalAtAbsolute(startIndex)
                        End If

                    Case GetType(Guid)
                        If length > 16 Then
                            Clip = GetGuidArray(startIndex, length >> 4)
                        Else
                            Clip = GuidAtAbsolute(startIndex)
                        End If

                    Case Else
                        Clip = Blob.Converter.ConvertTo(New Blob(handle + startIndex, MemAllocType.Heap, False) With {.hHeap = hHeap, .virtLen = length, .actualLen = length}, type)

                End Select

            End If

        End Function

        ''' <summary>
        ''' Sets the bytes of the specified object to the blob.
        ''' </summary>
        ''' <param name="index">Start index, in bytes to set the data</param>
        ''' <param name="value">Object to set</param>
        ''' <remarks></remarks>
        Public Sub SetAt(index As Long, value As Object)
            If TypeToBlobType(value.GetType) = BlobTypes.Invalid Then Return

            Dim bl As Blob = Converter.ConvertFrom(value)
            MemCpy(handle + index, bl.handle, bl.Length)
            bl.Dispose()
        End Sub

        ''' <summary>
        ''' Sets the specified character array into the blob at the specified index
        ''' </summary>
        ''' <param name="index">Start index, in characters to set the data</param>
        ''' <param name="value">Object to set</param>
        ''' <remarks></remarks>
        Public Sub SetAt(index As Long, value As Char())
            If index + (2 * value.Length) > Length Then Throw New ArgumentOutOfRangeException
            MemCpy(handle + (index * 2), value, value.Length * 2)
        End Sub

        ''' <summary>
        ''' Sets the specified character array into the blob at the specified index
        ''' </summary>
        ''' <param name="index">Start index, in characters to set the data</param>
        ''' <param name="value">Object to set</param>
        ''' <remarks></remarks>
        Public Sub SetAt(index As Long, value As String)
            If index + (2 * value.Length) > Length Then Throw New ArgumentOutOfRangeException
            SetString(index, value)
        End Sub

        ''' <summary>
        ''' Skips a clip.
        ''' </summary>
        <Description("Skips a clip.")>
        Public Sub Skip(length As Integer)
            _ClipNext += length
            If _ClipNext > Me.Length Then
                _ClipNext = 0
            End If
        End Sub

        ''' <summary>
        ''' Resets the clip count
        ''' </summary>
        <Description("Resets the clip count")>
        Public Sub ClipReset()
            _ClipNext = 0
        End Sub

#End Region

        '' if you're going to have an unmanaged buffer then there is
        '' no reason for a managed byte array as a middle man.
        '' it wastes many resources for large files.
#Region "Direct FileSystem Interaction"

#Region "FileSystem Declarations"

        Declare Unicode Function CreateFile Lib "kernel32.dll" _
             Alias "CreateFileW" _
             (<MarshalAs(UnmanagedType.LPWStr)> lpFileName As String,
              dwDesiredAccess As Integer,
              dwShareMode As Integer,
              lpSecurityAttributes As IntPtr,
              dwCreationDisposition As Integer,
              dwFlagsAndAttributes As Integer,
              hTemplateFile As IntPtr
              ) As IntPtr

        Declare Unicode Function WriteFile Lib "kernel32.dll" _
            (hFile As IntPtr,
             lpBuffer As IntPtr,
             nNumberOfBytesToWrite As UInteger,
             ByRef lpNumberOfBytesWritten As UInteger,
             lpOverlapped As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Declare Unicode Function ReadFile Lib "kernel32.dll" _
            (hFile As IntPtr,
             lpBuffer As IntPtr,
             nNumberOfBytesToRead As UInteger,
             ByRef lpNumberOfBytesRead As UInteger,
             lpOverlapped As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Declare Unicode Function FlushFileBuffers Lib "kernel32.dll" _
            (hFile As IntPtr
            ) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Declare Function GetFileSize Lib "kernel32" Alias "GetFileSize" (hFile As IntPtr, ByRef lpFileSizeHigh As UInteger) As UInteger

        Declare Function CloseHandle Lib "kernel32" (handle As IntPtr) As Boolean

        Const GENERIC_READ = (&H80000000I)
        Const GENERIC_WRITE = (&H40000000I)
        Const GENERIC_EXECUTE = (&H20000000I)
        Const GENERIC_ALL = (&H10000000I)

        Const FILE_ATTRIBUTE_NORMAL = &H80

        Const CREATE_NEW = 1
        Const CREATE_ALWAYS = 2
        Const OPEN_EXISTING = 3
        Const OPEN_ALWAYS = 4
        Const TRUNCATE_EXISTING = 5

        Const FILE_SHARE_READ = &H1
        Const FILE_SHARE_WRITE = &H2
        Const FILE_SHARE_DELETE = &H4

#End Region

        Public Function Read(fileName As String, Optional append As Boolean = False) As Long
            Dim h As IntPtr = CreateFile(fileName, GENERIC_READ, 0, 0, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, FILE_SHARE_READ)

            If h Then
                Dim cptr As IntPtr

                Dim bytesread As UInteger = 0

                Dim hi As UInteger
                Dim lo As UInteger = GetFileSize(h, hi)

                Dim len As Long = (CLng(hi) << 32) Or CLng(lo)

                If append Then
                    Dim oldVirt = virtLen

                    If Not Alloc(virtLen + len) Then
                        CloseHandle(h)
                        Return 0
                    End If

                    cptr = handle + oldVirt
                Else
                    If Not Alloc(len) Then
                        CloseHandle(h)
                        Return 0
                    End If

                    cptr = handle
                End If

                ClipReset()

                Dim blen As Long = len
                Dim ofs As Long = 0
                Dim inpLen As UInteger = Math.Min(UInteger.MaxValue, len)

                Do Until ofs >= blen
                    ReadFile(h, cptr, inpLen, bytesread, 0)
                    If bytesread < inpLen Then
                        ofs += bytesread
                        Exit Do
                    End If

                    ofs += inpLen
                    cptr += inpLen

                    inpLen = Math.Min(blen - ofs, inpLen)
                Loop

                FlushFileBuffers(h)
                CloseHandle(h)
                Return ofs
            Else
                Return 0
            End If
        End Function

        Public Function Read(fileName As String, length As Long, Optional append As Boolean = False) As Long
            Dim h As IntPtr = CreateFile(fileName, GENERIC_READ, 0, 0, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, FILE_SHARE_READ)

            If h Then
                Dim cptr As IntPtr

                Dim bytesread As UInteger = 0

                Dim hi As UInteger
                Dim lo As UInteger = GetFileSize(h, hi)

                Dim len As Long = Math.Min((CLng(hi) << 32) Or CLng(lo), length)

                If append Then
                    Dim oldVirt = virtLen

                    If Not Alloc(virtLen + len) Then
                        CloseHandle(h)
                        Return 0
                    End If

                    cptr = handle + oldVirt
                Else
                    If Not Alloc(len) Then
                        CloseHandle(h)
                        Return 0
                    End If

                    cptr = handle
                End If

                ClipReset()

                Dim blen As Long = len
                Dim ofs As Long = 0
                Dim inpLen As UInteger = Math.Min(UInteger.MaxValue, len)

                Do Until ofs >= blen
                    ReadFile(h, cptr, inpLen, bytesread, 0)
                    If bytesread < inpLen Then
                        ofs += bytesread
                        Exit Do
                    End If

                    ofs += inpLen
                    cptr += inpLen

                    inpLen = Math.Min(blen - ofs, inpLen)
                Loop

                FlushFileBuffers(h)
                CloseHandle(h)
                Return ofs
            Else
                Return 0
            End If
        End Function

        Public Function Write(fileName As String, Optional fOverwrite As Boolean = True) As Long

            If Not fOverwrite And File.Exists(fileName) Then Return False

            Dim h As IntPtr = CreateFile(fileName, GENERIC_WRITE, 0, 0, CREATE_ALWAYS, FILE_ATTRIBUTE_NORMAL, FILE_SHARE_READ)

            If h Then
                Dim byteswritten As UInteger = 0

                Dim blen As Long = virtLen
                Dim ofs As Long = 0
                Dim inpLen As UInteger = Math.Min(UInteger.MaxValue, virtLen)

                Dim cptr As IntPtr = handle

                Do Until ofs >= blen
                    WriteFile(h, cptr, inpLen, byteswritten, 0)
                    If byteswritten < inpLen Then
                        ofs += byteswritten
                        Exit Do
                    End If

                    ofs += inpLen
                    cptr += inpLen

                    inpLen = Math.Min(blen - ofs, inpLen)
                Loop

                FlushFileBuffers(h)
                CloseHandle(h)
                Return ofs
            Else
                Return False
            End If
        End Function

#End Region

#Region "Structures"

        Public Sub ToStructure(ByRef dest As Object)
            Marshal.PtrToStructure(Me.handle, dest)
        End Sub

        Public Sub FromStructure(src As Object)
            Dim cb As Integer = Marshal.SizeOf(src)

            If (cb <= 0) Then Return
            If Length < cb Then Length = cb

            Marshal.StructureToPtr(src, Me.handle, False)
        End Sub

        Public Sub AppendStructure(src As Object)
            Dim cb As Integer = Marshal.SizeOf(src)
            Dim myptr As IntPtr = handle + Length

            If (cb <= 0) Then Return
            Length += cb

            Marshal.StructureToPtr(src, myptr, False)
        End Sub

#End Region

#Region "Parse and ToString"

        ''' <summary>
        ''' Parses a string into the byte array.
        ''' </summary>
        ''' <param name="input">String to parse.</param>
        ''' <returns>Resulting Blob structure or Nothing.</returns>
        ''' <remarks></remarks>
        <Description("Parses a string into the byte array.")>
        Public Overloads Shared Function Parse(input() As Char) As Blob
            Return Parse(input.ToString)
        End Function

        ''' <summary>
        ''' Parses a string into the byte array.
        ''' </summary>
        ''' <param name="input">String to parse.</param>
        ''' <returns>Resulting Blob structure or Nothing.</returns>
        ''' <remarks></remarks>
        <Description("Parses a string into the byte array.")>
        Public Overloads Shared Function Parse(input As String) As Blob
            On Error Resume Next

            Dim b As New Blob
            Dim ch As Char = "A"c
            Dim gu As Guid

            Dim d As Object = Nothing

            If ParseStrings Then

                If Date.TryParse(input, d) Then

                    b = BlobConverter.DateToBytes(d)
                    b.Type = GetType(Date)
                    b.TypeLen = Len(d)

                    Return b
                End If

                If Guid.TryParse(input, gu) Then
                    b.Type = GetType(Guid)
                    b.TypeLen = 16
                    b = gu.ToByteArray
                    b.Align()

                    Return b
                End If

                If TryParseObject(input, b) Then Return b

            End If

            b = CType(input, Blob)
            b.BlobType = BlobTypes.Char

            Return b

        End Function

        ''' <summary>
        ''' Attempts to parse a supported object represented by the string into the Blob.
        ''' </summary>
        ''' <param name="input">String to attempt to parse.</param>
        ''' <param name="blob">Resulting Blob structure or Nothing.</param>
        ''' <returns>A value of True for success, otherwise False.</returns>
        ''' <remarks></remarks>
        Public Shared Function TryParseObject(input As String, ByRef blob As Blob) As Boolean

            Dim ft As SystemBlobTypes = SystemBlobTypes.Invalid
            '' look for numbers
            If input.Chars(0) = "{"c OrElse (IsNumeric(JustNumbers(input, , , 1))) Then
                Dim s As String = input
                Dim p() As String

                If input.Chars(0) = "{"c Then s = TextBetween(input, "{", "}")
                p = BatchParse(s, ",")

                Dim i As Integer,
                    c As Integer = p.Length - 1

                Dim objN() As Object,
                    maxB As Byte = 4

                Dim arrOut As Object

                ' Temporary variables for determining floating-point and unsigned characteristics.
                Dim flo As Byte = 0,
                    uns As Boolean = True

                Dim t As Type,
                    bt As BlobTypes

                ' Temporary value containers.
                Dim dec As Decimal,
                    db As Double,
                    sn As Single

                Dim gu As System.Guid

                Dim bi As BigInteger = Nothing
                ' Temporary value containers.

                ReDim objN(c)

                For i = 0 To c

                    If ft = SystemBlobTypes.Guid Then
                        If Not Guid.TryParse(Trim(p(i)), gu) Then
                            ft = SystemBlobTypes.Invalid
                        Else
                            objN(i) = gu
                            Continue For
                        End If
                    End If

                    s = JustNumbers(p(i), , , 1)
                    If s = vbNullString Then Return False

                    If (flo = 0) AndAlso (s.IndexOf(".") >= 0 OrElse s.IndexOf("e") >= 0 OrElse s.IndexOf("E") >= 0) Then flo = 1

                    If (Single.TryParse(s, sn)) Then
                        Double.TryParse(s, db)

                        If (db <> sn) Then maxB = 8
                    End If


                    If maxB > 4 OrElse Not Single.TryParse(s, sn) Then



                        If maxB > 8 OrElse Not Double.TryParse(s, db) Then
                            If Not Decimal.TryParse(s, dec) Then
                                '' There can't be a number that big with a decimal point, and a BigInteger sits by itself in a blob
                                If flo OrElse i Then Return False

                                If Not BigInteger.TryParse(s, bi) Then
                                    Return False
                                Else
                                    maxB = 100
                                    flo = 0
                                    Exit For
                                End If

                            End If

                            If flo = 0 Then flo = 1
                            If maxB < 16 Then maxB = 16

                            objN(i) = dec
                        Else
                            If uns AndAlso db < 0 Then uns = False
                            If (flo = 0) AndAlso ((uns AndAlso Math.Abs(db) > ULong.MaxValue) OrElse (Not uns AndAlso Math.Abs(db) > Long.MaxValue)) Then flo = 1
                            If maxB < 8 Then maxB = 8
                            objN(i) = db
                        End If
                    Else
                        If uns AndAlso sn < 0 Then uns = False
                        If (flo = 0) AndAlso ((uns AndAlso Math.Abs(sn) > UInteger.MaxValue) OrElse (Not uns AndAlso Math.Abs(sn) > Integer.MaxValue)) Then maxB = 8
                        objN(i) = sn
                        If maxB < 4 Then maxB = 4
                    End If
                Next

                If maxB = 100 Then
                    blob = Converter.ConvertFrom(bi)
                    Return True
                ElseIf maxB = 16 Then
                    bt = BlobTypes.Decimal
                Else
                    If flo Then
                        If maxB = 4 Then
                            bt = (BlobTypes.Single)
                        Else
                            bt = (BlobTypes.Double)
                        End If
                    Else
                        If maxB = 4 Then
                            If uns Then bt = (BlobTypes.UInteger) Else bt = (BlobTypes.Integer)
                        Else
                            If uns Then bt = (BlobTypes.ULong) Else bt = (BlobTypes.Long)
                        End If
                    End If
                End If

                Select Case ft
                    Case SystemBlobTypes.UInt16, SystemBlobTypes.UInt32, SystemBlobTypes.UInt64
                        If Not uns Then ft -= 1
                End Select

                If ft = BlobTypes.Invalid Then t = BlobTypeToType(bt) Else t = BlobTypeToType(ft)
                arrOut = Array.CreateInstance(t, c + 1)

                For i = 0 To c
                    arrOut(i) = (objN(i))
                Next

                If c = 0 Then
                    blob = Converter.ConvertFrom(arrOut(0))
                Else
                    blob = Converter.ConvertFrom(arrOut)
                End If
                Erase arrOut

                Return True
            End If

            '' Look for supported parsing types.
            If input.IndexOf("System.") = 0 Then
                Dim s As String = input.Substring(7)
                Dim i As Integer = 0

                Do Until " [{".IndexOf(s.Chars(i)) > -1
                    i += 1
                    If i >= s.Length Then Exit Do
                Loop
                If i < s.Length Then

                    If s.Chars(i) = "[" Then
                        If s.Chars(i + 1) <> "]" Then Return False
                        input = s.Substring(i + 2)
                    Else
                        input = s.Substring(i)
                    End If
                    s = s.Substring(0, i)

                Else
                    Return False
                End If

                If Not System.Enum.TryParse(Of SystemBlobTypes)(s, ft) Then
                    ft = SystemBlobTypes.Invalid
                End If
            End If

            Dim cc As System.Drawing.Color

            '' Try parsing color?
            cc = Color.FromName(input)
            If cc.ToArgb = 0 OrElse input.Length >= 7 Then
                If input.IndexOf("argb(") = 0 Then
                    Dim cv() As String

                    cv = BatchParse(TextBetween(input, "(", ")"), ",")
                    cc = Color.FromArgb(CInt(Val(cv(0))), CInt(Val(cv(1))), CInt(Val(cv(2))), CInt(Val(cv(3))))
                ElseIf input.IndexOf("rgba(") = 0 Then
                    Dim cv() As String

                    cv = BatchParse(TextBetween(input, "(", ")"), ",")
                    cc = Color.FromArgb(CInt(Val(cv(0)) * 255), CInt(Val(cv(1))), CInt(Val(cv(2))), CInt(Val(cv(3))))
                ElseIf input.IndexOf("rgb(") = 0 Then
                    Dim cv() As String

                    cv = BatchParse(TextBetween(input, "(", ")"), ",")
                    cc = Color.FromArgb(255, CInt(Val(cv(1))), CInt(Val(cv(2))), CInt(Val(cv(3))))
                ElseIf input.IndexOf("#") = 0 AndAlso input.Length = 7 Then
                    Dim cs As String = input.Substring(1)
                    Dim cv(2) As Integer

                    cv(0) = Integer.Parse(cs.Substring(0, 2), Globalization.NumberStyles.AllowHexSpecifier)
                    cv(1) = Integer.Parse(cs.Substring(2, 2), Globalization.NumberStyles.AllowHexSpecifier)
                    cv(2) = Integer.Parse(cs.Substring(4, 2), Globalization.NumberStyles.AllowHexSpecifier)

                    cc = Color.FromArgb(255, cv(0), cv(1), cv(2))

                End If
            End If

            If cc.ToArgb <> 0 Then
                blob.Length = 4
                blob.BlobType = BlobTypes.Color
                blob.IntegerAt(0) = cc.ToArgb

                Return True
            End If

            Return False
        End Function

        Public Overloads Function ToString() As String
            Return ToString(False)
        End Function

        Public Overloads Function ToString(plainNum As Boolean) As String

            If BlobType = BlobTypes.String Then
                ToString = GrabString(0)
                Exit Function
            End If

            If StringLength = 0 Then Return New String("")

            If Type = Nothing Then
                Type = GetType(Byte())
                TypeLen = 1
            End If

            If Type.IsEnum = True OrElse (Type.IsArray = True AndAlso ElementType.IsEnum = True) Then
                If TypeLen = Length Then
                    Dim o As Object = BytesToEnum(Me, Type)
                    If plainNum Then Return o.ToString Else Return o.ToString & " {" & CLng(o) & "}"
                Else
                    Dim o As Object = BytesToEnum(Me, Type)
                    Dim x As Object
                    Dim s As String = ""

                    If Not plainNum Then s &= Type.FullName

                    s &= "{"

                    For Each x In o
                        If s.Length > 1 Then s &= ", "
                        s &= x.ToString
                    Next
                    s &= "}"

                    Return s
                End If
            End If

            Select Case Type
                Case GetType(Date)
                    Return CStr(BlobConverter.BytesToDate(Me))

                Case GetType(Char()), GetType(String), GetType(Byte)
                    Return UnicodeEncoding.Unicode.GetString(Me)

                Case GetType(System.Drawing.Color)

                    Return CType(Me, System.Drawing.Color).ToString

                Case Else
                    If IsArray Then

                        If MaxBlobPrintNum AndAlso Count > MaxBlobPrintNum Then
                            Return Type.FullName & "[" & Count & "]"
                        End If

                        Dim ct As Type = Array.CreateInstance(ElementType, 0).GetType

                        Dim a As Object = Converter.ConvertTo(Me, ct)

                        Dim i As Integer,
                            c As Integer = a.Length - 1

                        Dim s As String = ""

                        If Not plainNum Then s &= Type.FullName
                        s &= "{"

                        For i = 0 To c
                            If (i) Then s &= ", "
                            s &= (a(i).ToString)
                        Next
                        s &= "}"

                        Return s

                    Else

                        Dim a As Object = Converter.ConvertTo(Me, Type)
                        Return a.ToString

                    End If
            End Select

        End Function

#End Region

#Region "Properties"

        ''' <summary>
        ''' Specifies that we do not append null terminators to strings that are concatenated with this Blob.
        ''' Applies only when concatenating a string to an existing blob.
        ''' </summary>
        ''' <returns></returns>
        Public Property StringCatNoNull As Boolean
            Get
                Return _StringCatNoNull
            End Get
            Set(value As Boolean)
                _StringCatNoNull = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the number of elements. This property's value depends
        ''' on the BlobType.  If the BlobType is Integer, then Length = Count * 4.
        ''' </summary>
        <Description("Gets or sets the number of elements.")>
        Public Property Count As Integer
            Get
                If Type Is Nothing Then Align()
                If Length = 0 Then Return 0
                If IsBigInteger Then Return 1
                Return Length / TypeLen
            End Get
            Set(value As Integer)
                If _Locked Then Return
                If Type Is Nothing Then Align()

                If value = 0 Then
                    Free()
                    Return
                End If

                If IsBigInteger Then Return

                Me.Length = (value * TypeLen)
                Me.Align()
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a value indicating whether to double the size of the BufferExtend with each subsequent increase.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AutoDouble As Boolean
            Get
                Return _AutoDouble
            End Get
            Set(value As Boolean)
                _AutoDouble = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a value indicating whether or not the blob behaves as an extending buffer or an absolute memory representation.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property InBufferMode As Boolean
            Get
                InBufferMode = _inBuffer
            End Get
            Set(value As Boolean)
                _inBuffer = value
                If virtLen Then Recommit(virtLen)
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the length of the Blob data stream.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Length As Long
            Get
                Return virtLen
            End Get
            Set(value As Long)
                If Not ReAlloc(value) Then
                    Throw New OutOfMemoryException("Could not allocate " & value.ToString("N0") & " bytes of data. Try allocating larger contiguous chunks instead of attempting to repeatedly allocate small chunks as memory can be fragmented.")
                End If
            End Set
        End Property

        Public ReadOnly Property StringLength As Integer
            Get
                If handle = 0 Then Return 0
                Return CharZero(handle)
            End Get
        End Property

        ''' <summary>
        ''' Returns the actual length of the buffer as initialized by the operating system.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ActualLength As Long
            Get
                Return actualLen
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets a value that indicates the buffer extension threshold, in bytes.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property BufferExtend As Long
            Get
                Return _bufferExtend
            End Get
            Set(value As Long)
                If value <> _bufferExtend Then
                    _bufferExtend = value
                    ReAlloc(virtLen)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Returns the BlobType.
        ''' </summary>
        <Description("Returns the BlobType.")>
        Public Property BlobType As BlobTypes
            Get
                If Type = Nothing Then Align()
                Return _BlobType
            End Get
            Set(value As BlobTypes)
                If value <= BlobTypes.Invalid Or value > BlobConst.UBound Then Return
                TypeAlign(BlobTypeToType(value))
            End Set
        End Property

        ''' <summary>
        ''' Defines a threshold after which arrays will not convert to strings, or zero for no limit (default).
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Description("Defines a threshold after which arrays will not convert to strings, or zero for no limit (default).")>
        Public Property MaxBlobPrintNum() As Integer
            Get
                Return _MaxBlobPrintNum
            End Get
            Set(value As Integer)
                _MaxBlobPrintNum = value
            End Set
        End Property

        ''' <summary>
        ''' Gets each value as if part of an array.  Returns a new blob containing the bytes for exactly that element.
        ''' </summary>
        ''' <param name="index"></param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Description("Gets each value as if part of an array.  Returns a new blob containing the bytes for exactly that element.")>
        Default Public Shadows ReadOnly Property Item(index As Integer) As Blob
            Get
                If Length = 0 Then Return Nothing

                Dim c As Integer = index * TypeLen
                Dim bl As New Blob

                bl.Length = TypeLen
                bl.SetBytes(0, Me.GrabBytes(c, TypeLen))

                bl.Type = ElementType
                bl.TypeLen = TypeLen

                Return bl
            End Get
        End Property

        ''' <summary>
        ''' Gets the last known type of the contained value.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Description("Gets the last known type of the contained value.")>
        Public ReadOnly Property BaseType() As System.Type
            Get
                If Type Is Nothing Then Align()
                Return Me.Type
            End Get
        End Property

        ''' <summary>
        ''' Gets a value indicating whether the virtual element is a floating point number.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Description("Gets a value indicating whether the virtual element is a floating point number.")>
        Public ReadOnly Property IsFloat() As Boolean
            Get
                If Type Is Nothing Then Align()
                Dim t As BlobTypes = BlobType
                Return ((t >= BlobTypes.Single) And (t <= BlobTypes.Decimal))
            End Get
        End Property

        ''' <summary>
        ''' Gets a value indicating whether the virtual element is an array. Returns true for strings with more than one character since all strings are interally referred to as Char
        ''' </summary>
        <Description("Gets a value indicating whether the virtual element is an array. Returns true for strings with more than one character since all strings are interally referred to as Char[]")>
        Public Property IsArray() As Boolean
            Get
                If Type Is Nothing Then Align()
                Return Me.BaseType.IsArray
            End Get
            Set(value As Boolean)
                If _Locked Then Return
                Dim o As Object

                If value = False AndAlso Type IsNot Nothing AndAlso Type.IsArray = True Then
                    Type = Type.GetElementType
                    o = Array.CreateInstance(Type, 1)
                    Me.Length = Len(o(0))
                    Erase o
                ElseIf value = True AndAlso Type IsNot Nothing AndAlso Type.IsArray = False Then
                    o = Array.CreateInstance(Type, 1)
                    Type = o.GetType
                    Erase o
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets a value indicating whether the virtual element is a System.Numerics.BigInteger.
        ''' </summary>
        <Description("Gets a value indicating whether the virtual element is a System.Numerics.BigInteger.")>
        Public ReadOnly Property IsBigInteger As Boolean
            Get
                If Type Is Nothing Then Align()
                Return (BlobType = BlobTypes.BigInteger)
            End Get
        End Property

        ''' <summary>
        ''' Gets a value indicating whether the virtual element is numeric
        ''' </summary>
        <Description("Gets a value indicating whether the virtual element is numeric (this behavior differs from the system IsNumeric function in that Char is treated as a string. Use the TypeAlign method to force the blob to consider itself a short.")>
        Public ReadOnly Property IsNumber() As Boolean
            Get
                If Type Is Nothing Then Align()
                Return (BlobType <= BlobConst.MaxMath)
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets
        ''' </summary>
        <Description("Gets or sets = a value indicating whether the virtual element is treated as a string.  If set to false, the blob converts to UShort.")>
        Public Property IsString() As Boolean
            Get
                Select Case _BlobType
                    Case BlobTypes.Char, BlobTypes.String, BlobTypes.NtString
                        Return True
                    Case Else
                        Return False
                End Select
            End Get
            Set(value As Boolean)
                If value = True Then
                    Type = Types(BlobTypes.Char)
                ElseIf ElementType = GetType(Char) Then
                    Type = Types(BlobTypes.UShort)
                End If

                Align()
            End Set
        End Property

        ''' <summary>
        ''' Gets the last known element type of the contained value.
        ''' </summary>
        <Description("Gets the last known element type of the contained value.")>
        Public ReadOnly Property ElementType() As System.Type
            Get
                If Type Is Nothing Then Align()
                Return IIf(IsArray, Type.GetElementType, Type)
            End Get
        End Property

        ''' <summary>
        ''' Gets the size
        ''' </summary>
        <Description("Gets the size (in bytes) of each element.")>
        Public ReadOnly Property ElementSize() As Integer
            Get
                If Type Is Nothing Then Align()
                Return Me.TypeLen
            End Get
        End Property

#End Region '' Properties and Property-Like Methods

#Region "Alignment"

        ''' <summary>
        ''' Coerces the blob into the specific type.  This can result in unpredictable behavior if not used carefully.  Align() is always called in TypeAlign.
        ''' </summary>
        <Description("Coerces the blob into the specific type.  This can result in unpredictable behavior if not used carefully. Align() is always called in TypeAlign.")>
        Public Sub TypeAlign(type As System.Type)
            Me.Type = type

            Align()
        End Sub

        ''' <summary>
        ''' Align the byte array to the byte length of the current type.  If the byte array is empty, it is initialized to contain exactly 1 element of the current type.  Align also performs array to scalar conversion when the condition that Length
        ''' </summary>
        <Description("Align the byte array to the byte length of the current type.  If the byte array is empty, it is initialized to contain exactly 1 element of the current type.  Align also performs array to scalar conversion when the condition that Length=TypeLen is met, and scalar to array conversion when the condition is not met.")>
        Public Sub Align()

            If Type = Nothing Then Type = GetType(Byte())
            '            If Type = GetType(String) Then Type = GetType(Char())

            TypeLen = BlobTypeSize(TypeToBlobType(Type))
            _BlobType = TypeToBlobType(ElementType)

            If Length = 0 Then
                Return
            End If

            Dim n = (Length Mod TypeLen)

            If Not _Locked Then
                If n <> 0 Then
                    n = (Length + (TypeLen - n))
                    Length = n
                End If
            End If

            If ArrayTypes(_BlobType) IsNot Nothing AndAlso Length > TypeLen AndAlso Not Type.IsArray Then
                Type = ArrayTypes(_BlobType)
            End If

        End Sub

#End Region '' Alignment

#Region "WinForms Color and Image"

        Public Function GetImage(byteIndex As IntPtr) As System.Drawing.Image

            Dim mm As MemPtr = handle + byteIndex
            GetImage = System.Drawing.Bitmap.FromStream(New System.IO.MemoryStream(CType(mm, Byte())))

        End Function

        Public Sub SetImage(byteIndex As IntPtr, image As System.Drawing.Image, Optional format As System.Drawing.Imaging.ImageFormat = Nothing)

            If format Is Nothing Then
                format = System.Drawing.Imaging.ImageFormat.Png
            End If

            Dim ms As New MemoryStream

            image.Save(ms, format)

            Dim by() As Byte
            ReDim by(ms.Length - 1)

            ms.Read(by, 0, by.Length)
            ms.Dispose()

            If IsInvalid OrElse Length < (by.Length + CType(byteIndex, Int64)) Then
                Length = by.Length + byteIndex
            End If

            MemCpy(handle + byteIndex, by, by.Length)

        End Sub

        Public Property ImageAt(byteIndex As Long) As System.Drawing.Image
            Get
                ImageAt = GetImage(byteIndex)
            End Get
            Set(value As System.Drawing.Image)
                SetImage(byteIndex, value)
            End Set
        End Property

        Public Property ColorAt(byteIndex As Long) As System.Drawing.Color
            Get
                Return System.Drawing.Color.FromArgb(IntegerAt(byteIndex))
            End Get
            Set(value As System.Drawing.Color)
                IntegerAt(byteIndex) = value.ToArgb
            End Set
        End Property

        Public Property ColorAtAbsolute(byteIndex As Long) As System.Drawing.Color
            Get
                Return System.Drawing.Color.FromArgb(IntegerAtAbsolute(byteIndex))
            End Get
            Set(value As System.Drawing.Color)
                IntegerAtAbsolute(byteIndex) = value.ToArgb
            End Set
        End Property

#End Region '' WinForms Color and Image

#Region "WPF Color and Image"

        '' To do!

#End Region '' WPF Color and Image

#Region "Allocation and Deallocation"

#Region "Normal Memory"

        ''' <summary>
        ''' Clears the entire object.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Clear()
            Free()
            BlobType = BlobTypes.Byte
        End Sub

        ''' <summary>
        ''' Allocate a new memory buffer on the process heap.
        ''' </summary>
        ''' <param name="length">Length of the new buffer, in bytes.</param>
        ''' <returns>True if successful.</returns>
        ''' <remarks></remarks>
        Public Function Alloc(length As Long) As Boolean

            If length <= 0 Then Return Free()

            If IsInvalid = False Then
                Return ReAlloc(length)
            End If

            Recommit(length)

            Select Case _MemType

                Case MemAllocType.Virtual

                    Return VirtualAlloc(actualLen)

                Case MemAllocType.Network

                    Return NetAlloc(CInt(actualLen And &H7FFFFFFFI))

                Case Else
                    ' Threading.Monitor.Enter(Me)

                    If HeapValidate(hHeap, 0, 0) Then
                        handle = HeapAlloc(hHeap, 8, actualLen)
                        actualLen = HeapSize(hHeap, 0, handle)
                        Alloc = (handle <> 0)
                    Else
                        Alloc = False
                    End If

                    ' Threading.Monitor.Exit(Me)

            End Select

            If Alloc Then
                _MemType = MemAllocType.Heap
                GC.AddMemoryPressure(actualLen)
            Else
                actualLen = 0
                virtLen = 0
            End If
        End Function

        ''' <summary>
        ''' Reallocate a memory buffer with a new size on the process heap.
        ''' </summary>
        ''' <param name="length">New length of the memory buffer.</param>
        ''' <returns>True if successful.</returns>
        ''' <remarks></remarks>
        Public Function ReAlloc(length As Long) As Boolean
            If IsInvalid Then Return Alloc(length)

            Dim oldVirt As Long = virtLen
            Dim oldLen As Long = actualLen

            Recommit(length)

            If actualLen = oldLen Then
                '' it is very important to be conservative here:
                If virtLen < oldVirt Then ZeroMemory(virtLen, oldVirt - virtLen)

                Return True
            End If

            If _MemType = MemAllocType.Heap Then

                Try
                    ' Threading.Monitor.Enter(Me)
                    If HeapValidate(hHeap, 0, handle) Then
                        Dim p As IntPtr = HeapReAlloc(hHeap, 8, handle, actualLen)

                        If p <> 0 Then
                            handle = p
                            actualLen = HeapSize(hHeap, 0, handle)

                            ReAlloc = True
                        Else
                            actualLen = oldLen
                            virtLen = oldVirt
                            ReAlloc = False
                        End If
                    Else
                        actualLen = oldLen
                        virtLen = oldVirt
                        ReAlloc = False
                    End If

                    ' Threading.Monitor.Exit(Me)

                Catch ex As Exception
                    actualLen = oldLen
                    virtLen = oldVirt
                    ReAlloc = False
                End Try

            ElseIf _MemType = MemAllocType.Network Then

                ' Threading.Monitor.Enter(Me)

                Dim mm As New MemPtr
                mm.NetAlloc(actualLen)

                If mm.Handle = 0 Then
                    actualLen = oldLen
                    virtLen = oldVirt
                    ReAlloc = False
                Else
                    GC.AddMemoryPressure(actualLen)
                    MemCpy(mm.Handle, handle, oldVirt)

                    NetApiBufferFree(handle)
                    GC.RemoveMemoryPressure(oldLen)

                    handle = mm.Handle

                    ReAlloc = True
                End If

                ' Threading.Monitor.Exit(Me)
                Exit Function

            ElseIf _MemType = MemAllocType.Virtual Then

                ' Threading.Monitor.Enter(Me)

                Dim mm As New MemPtr

                mm.VirtualAlloc(actualLen)

                If mm.Handle = 0 Then
                    actualLen = oldLen
                    virtLen = oldVirt
                    ReAlloc = False
                Else

                    MemCpy(mm.Handle, handle, oldVirt)
                    Me.VirtualFree()
                    _MemType = MemAllocType.Virtual

                    handle = mm.Handle
                    actualLen = VirtualLength()

                    ReAlloc = True
                End If

                ' Threading.Monitor.Exit(Me)

            ElseIf _MemType = MemAllocType.Com Then

                ' Threading.Monitor.Enter(Me)

                Dim mm As MemPtr = Marshal.AllocCoTaskMem(actualLen)

                If mm.Handle = 0 Then
                    actualLen = oldLen
                    virtLen = oldVirt
                    ReAlloc = False
                Else
                    MemCpy(mm.Handle, handle, oldVirt)

                    Marshal.FreeCoTaskMem(handle)
                    handle = mm.Handle

                    ReAlloc = True
                End If

                ' Threading.Monitor.Exit(Me)
            Else
                Return False
            End If

            If ReAlloc Then
                actualLen = ActualLength

                If actualLen > oldLen Then
                    GC.AddMemoryPressure(actualLen - oldLen)
                Else
                    GC.RemoveMemoryPressure(oldLen - actualLen)
                End If
            End If

        End Function

        ''' <summary>
        ''' Frees the resources allocated by the current object.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Free() As Boolean

            Free = (handle = 0)
            Dim al As Long

            al = ActualLength

            Select Case _MemType

                Case MemAllocType.Heap

                    ' Threading.Monitor.Enter(Me)

                    If HeapValidate(hHeap, 0, handle) Then
                        Free = HeapFree(hHeap, 0, handle) <> 0
                    End If

                ' Threading.Monitor.Exit(Me)

                Case MemAllocType.Virtual
                    Free = VirtualFree()
                    Exit Function

                Case MemAllocType.Network
                    Free = NetFree()
                    Exit Function

                Case MemAllocType.Com

                    Try
                        Marshal.FreeCoTaskMem(handle)
                        Free = True
                    Catch ex As Exception

                    End Try

            End Select

            If Free Then
                handle = IntPtr.Zero
                virtLen = 0
                actualLen = 0

                If (al) Then GC.RemoveMemoryPressure(al)

                _MemType = MemAllocType.Invalid
            End If

        End Function

        ''' <summary>
        ''' Calculate the comitted memory based on buffering specifications.
        ''' This function should only be used as part of an internal allocation, because it changes global variables.
        ''' </summary>
        ''' <returns>The number of bytes to actually commit using the given memory allocation function.</returns>
        ''' <remarks></remarks>
        Private Function Recommit(len As Long) As Long

            If Not _inBuffer Then
                actualLen = len
                virtLen = len
                Recommit = len

                Exit Function
            End If

            If (len <= actualLen) Then
                virtLen = len
            Else
                actualLen += len + (_bufferExtend - (len Mod _bufferExtend))
                If _AutoDouble Then _bufferExtend *= 2
                virtLen = len
            End If

            If _MemType = MemAllocType.Network AndAlso _bufferExtend > Int32.MaxValue Then _bufferExtend = SystemInformation.SystemInfo.dwPageSize

            Recommit = actualLen

        End Function

        Private Function CleanupBuffer() As Boolean
            If Not _inBuffer Then
                If virtLen <> 0 Then
                    ReAlloc(virtLen)
                End If
                Return False
            End If

            If virtLen < _bufferExtend Then
                CleanupBuffer = ReAlloc(_bufferExtend)

                If _AutoDouble Then
                    _bufferExtend = SystemInformation.SystemInfo.dwPageSize
                End If

                Exit Function
            End If

            Return True
        End Function

#End Region

#Region "Network Memory"

        ''' <summary>
        ''' Allocate a network API compatible memory buffer.
        ''' </summary>
        ''' <param name="size">Size of the buffer to allocate, in bytes.</param>
        ''' <remarks></remarks>
        Public Function NetAlloc(size As Integer) As Boolean
            '' just ignore a full buffer.
            If handle <> 0 Then Return True

            Dim l As Long = Recommit(size)

            NetApiBufferAllocate(actualLen, handle)

            If handle <> 0 Then
                NetAlloc = True
                _MemType = MemAllocType.Network
                GC.AddMemoryPressure(actualLen)
            Else
                actualLen = 0
                virtLen = 0
                handle = 0
                NetAlloc = False
            End If
        End Function

        ''' <summary>
        ''' Free a network API compatible memory buffer previously allocated with NetAlloc.
        ''' </summary>
        ''' <remarks></remarks>
        Public Function NetFree() As Boolean
            If _MemType <> MemAllocType.Network Then Return False
            If handle = 0 Then Return True
            NetFree = NetApiBufferFree(handle) = 0

            If NetFree Then
                GC.RemoveMemoryPressure(actualLen)
                virtLen = 0
                actualLen = 0
                handle = 0
            End If

        End Function

#End Region

#Region "Virtual Memory"

        ''' <summary>
        ''' Allocates a region of virtual memory.
        ''' </summary>
        ''' <param name="size">The size of the region of memory to allocate.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function VirtualAlloc(size As Long) As Boolean
            If handle <> 0 Then Return False

            If size = 0 Then
                Return VirtualFree()
            End If

            Dim l As Long = Recommit(size)

            handle = Native.VirtualAlloc(0, l,
                                       VMemAllocFlags.MEM_COMMIT Or
                                       VMemAllocFlags.MEM_RESERVE,
                                       MemoryProtectionFlags.PAGE_READWRITE)

            VirtualAlloc = handle <> 0

            If VirtualAlloc Then
                actualLen = VirtualLength()
                virtLen = size
                GC.AddMemoryPressure(actualLen)
            Else
                actualLen = 0
                virtLen = 0
                handle = 0
            End If

        End Function

        ''' <summary>
        ''' Frees a region of memory previously allocated with VirtualAlloc.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function VirtualFree() As Boolean
            If _MemType <> MemAllocType.Virtual Then Return False

            Dim l As Long

            '' While the function doesn't need to call VirtualFree, it hasn't necessarily failed, either.
            If handle = 0 Then
                VirtualFree = True
            Else
                '' see if we need to tell the garbage collector anything.
                l = VirtualLength()
                VirtualFree = Native.VirtualFree(handle)

                '' see if we need to tell the garbage collector anything.
                If VirtualFree Then
                    handle = 0
                    GC.RemoveMemoryPressure(l)
                End If
            End If

        End Function

        ''' <summary>
        ''' Returns the size of a region of virtual memory previously allocated with VirtualAlloc.
        ''' </summary>
        ''' <returns>The size of a virtual memory region or zero.</returns>
        ''' <remarks></remarks>
        Public Function VirtualLength() As Long
            If _MemType <> MemAllocType.Virtual Then Return 0
            If handle = 0 Then Return 0

            Dim m As New MEMORY_BASIC_INFORMATION

            If VirtualQuery(handle, m, Marshal.SizeOf(m)) <> 0 Then
                Return m.RegionSize
            End If

            Return 0
        End Function

#End Region

#Region "Zero Memory"

        ''' <summary>
        ''' Clears all the memory
        ''' </summary>
        ''' <remarks></remarks>
        Public Overridable Sub ZeroMemory()
            Try
                Dim l = Length

                If l And &HFFFFFFFF00000000L Then
                    n_memset(handle, 0, l)
                Else
                    MemSet(handle, 0, l)
                End If
            Catch ex As Exception

            End Try
        End Sub

        ''' <summary>
        ''' Clears all memory to the specified length.
        ''' </summary>
        ''' <param name="length">Length of memory to clear.</param>
        ''' <remarks></remarks>
        Public Overridable Sub ZeroMemory(length As Long)
            Try
                If length And &HFFFFFFFF00000000L Then
                    n_memset(handle, 0, length)
                Else
                    MemSet(handle, 0, length)
                End If
            Catch ex As Exception

            End Try
        End Sub

        ''' <summary>
        ''' Clears all the memory starting at the specified byte index for the specified length.
        ''' </summary>
        ''' <param name="byteIndex">Byte index, relative to the memory pointer, at which to begin clearing.</param>
        ''' <param name="length">Length of memory to clear.</param>
        ''' <remarks></remarks>
        Public Overridable Sub ZeroMemory(byteIndex As Long, length As Long)
            Try
                If length And &HFFFFFFFF00000000L Then
                    n_memset(handle + byteIndex, 0, length)
                Else
                    MemSet(handle + byteIndex, 0, length)
                End If
            Catch ex As Exception

            End Try
        End Sub

#End Region

#End Region '' Allocation and Deallocation

#Region "Equality Operators"

        '' Blob
        Public Overloads Function Equals(obj As Object) As Boolean
            If TypeOf obj Is Blob Then
                Return CType(obj, Blob).handle = handle
            ElseIf TypeOf obj Is IntPtr Then
                Return CType(obj, IntPtr) = handle
            ElseIf TypeOf obj Is UIntPtr Then
                Return ToSigned(CType(obj, UIntPtr)) = handle
            Else
                Return False
            End If
        End Function

        Public Overloads Function Equals(other As SafeHandle) As Boolean Implements IEquatable(Of SafeHandle).Equals
            Return other.DangerousGetHandle = handle
        End Function

        Public Overloads Function Equals(other As UIntPtr) As Boolean Implements IEquatable(Of UIntPtr).Equals
            Return ToSigned(other) = handle
        End Function

        Public Overloads Function Equals(other As Blob) As Boolean Implements IEquatable(Of Blob).Equals
            Return other.handle = handle
        End Function

        Public Overloads Function Equals(other As IntPtr) As Boolean Implements IEquatable(Of IntPtr).Equals
            Return other = handle
        End Function

        Public Overloads Function Equals(other As MemPtr) As Boolean Implements IEquatable(Of MemPtr).Equals
            Return other = handle
        End Function

        Public Overloads Function Equals(other As SafePtr) As Boolean Implements IEquatable(Of SafePtr).Equals
            Return other.DangerousGetHandle = handle
        End Function

        Public Shared Operator =(v1 As Blob, v2 As Blob) As Boolean
            Return (CLng(v1.handle) = CLng(v2.handle))
        End Operator

        Public Shared Operator <>(v1 As Blob, v2 As Blob) As Boolean
            Return (CLng(v1.handle) <> CLng(v2.handle))
        End Operator

        Public Shared Operator <(v1 As Blob, v2 As Blob) As Boolean
            Return (CLng(v1.handle) < CLng(v2.handle))
        End Operator

        Public Shared Operator >(v1 As Blob, v2 As Blob) As Boolean
            Return (CLng(v1.handle) > CLng(v2.handle))
        End Operator

        Public Shared Operator <=(v1 As Blob, v2 As Blob) As Boolean
            Return (CLng(v1.handle) <= CLng(v2.handle))
        End Operator

        Public Shared Operator >=(v1 As Blob, v2 As Blob) As Boolean
            Return (CLng(v1.handle) >= CLng(v2.handle))
        End Operator

#End Region '' Equality Operators

        '' Math on blob may or may not be reimplemented. Time will tell.
#Region "Math on Blob"

#Region "Math Engine"

#End Region

#Region "Math Operators"

#End Region

#End Region '' Math on Blob

#Region "Concatenation"

#Region "Concatenation To Blob"

        Public Shared Operator &(operand1 As Blob, operand2 As Blob) As Blob

            If operand2 Is Nothing OrElse operand2.Length = 0 Then Return operand1
            If operand1 Is Nothing Then operand1 = New Blob

            Dim l As IntPtr = operand1.Length
            Dim l2 As IntPtr = operand2.Length

            operand1.Length += l2

            If l2.ToInt64 > UInt32.MaxValue Then
                n_memcpy(operand1.handle + l, operand2.handle, l2.ToInt64)
            Else
                MemCpy(operand1.handle + l, operand2.handle, l2)
            End If

            Return operand1
        End Operator

        Public Shared Operator &(operand1 As Blob, operand2 As Byte()) As Blob
            If operand2 Is Nothing OrElse operand2.Length = 0 Then Return operand1
            If operand1 Is Nothing Then operand1 = New Blob

            Dim l As Long = operand1.Length
            Dim l2 As Integer = operand2.Length

            operand1.Length += l2
            MemCpy(operand1.handle + l, operand2, l2)

            Return operand1
        End Operator

        Public Shared Operator &(operand1 As Blob, operand2 As SByte()) As Blob
            If operand2 Is Nothing OrElse operand2.Length = 0 Then Return operand1
            If operand1 Is Nothing Then operand1 = New Blob

            Dim l As Long = operand1.Length
            Dim l2 As Integer = operand2.Length

            operand1.Length += l2
            MemCpy(operand1.handle + l, operand2, l2)

            Return operand1
        End Operator

        Public Shared Operator &(operand1 As Blob, operand2 As Short()) As Blob
            If operand2 Is Nothing OrElse operand2.Length = 0 Then Return operand1
            If operand1 Is Nothing Then operand1 = New Blob

            Dim l As Long = operand1.Length
            Dim l2 As Integer = operand2.Length * 2

            operand1.Length += l2
            MemCpy(operand1.handle + l, operand2, l2)

            Return operand1
        End Operator

        Public Shared Operator &(operand1 As Blob, operand2 As UShort()) As Blob
            If operand2 Is Nothing OrElse operand2.Length = 0 Then Return operand1
            If operand1 Is Nothing Then operand1 = New Blob

            Dim l As Long = operand1.Length
            Dim l2 As Integer = operand2.Length * 2

            operand1.Length += l2
            MemCpy(operand1.handle + l, operand2, l2)

            Return operand1
        End Operator

        Public Shared Operator &(operand1 As Blob, operand2 As Integer()) As Blob
            If operand2 Is Nothing OrElse operand2.Length = 0 Then Return operand1
            If operand1 Is Nothing Then operand1 = New Blob

            Dim l As Long = operand1.Length
            Dim l2 As Integer = operand2.Length * 4

            operand1.Length += l2
            MemCpy(operand1.handle + l, operand2, l2)

            Return operand1
        End Operator

        Public Shared Operator &(operand1 As Blob, operand2 As UInteger()) As Blob
            If operand2 Is Nothing OrElse operand2.Length = 0 Then Return operand1
            If operand1 Is Nothing Then operand1 = New Blob

            Dim l As Long = operand1.Length
            Dim l2 As Integer = operand2.Length * 4

            operand1.Length += l2
            MemCpy(operand1.handle + l, operand2, l2)

            Return operand1
        End Operator

        Public Shared Operator &(operand1 As Blob, operand2 As Long()) As Blob
            If operand2 Is Nothing OrElse operand2.Length = 0 Then Return operand1
            If operand1 Is Nothing Then operand1 = New Blob

            Dim l As Long = operand1.Length
            Dim l2 As Integer = operand2.Length * 8

            operand1.Length += l2
            MemCpy(operand1.handle + l, operand2, l2)

            Return operand1
        End Operator

        Public Shared Operator &(operand1 As Blob, operand2 As ULong()) As Blob
            If operand2 Is Nothing OrElse operand2.Length = 0 Then Return operand1
            If operand1 Is Nothing Then operand1 = New Blob

            Dim l As Long = operand1.Length
            Dim l2 As Integer = operand2.Length * 8

            operand1.Length += l2
            MemCpy(operand1.handle + l, operand2, l2)

            Return operand1
        End Operator

        Public Shared Operator &(operand1 As Blob, operand2 As Single()) As Blob
            If operand2 Is Nothing OrElse operand2.Length = 0 Then Return operand1
            If operand1 Is Nothing Then operand1 = New Blob

            Dim l As Long = operand1.Length
            Dim l2 As Integer = operand2.Length * 4

            operand1.Length += l2
            MemCpy(operand1.handle + l, operand2, l2)

            Return operand1
        End Operator

        Public Shared Operator &(operand1 As Blob, operand2 As Double()) As Blob
            If operand2 Is Nothing OrElse operand2.Length = 0 Then Return operand1
            If operand1 Is Nothing Then operand1 = New Blob

            Dim l As Long = operand1.Length
            Dim l2 As Integer = operand2.Length * 8

            operand1.Length += l2
            MemCpy(operand1.handle + l, operand2, l2)

            Return operand1
        End Operator

        Public Shared Operator &(operand1 As Blob, operand2 As Decimal()) As Blob
            If operand2 Is Nothing OrElse operand2.Length = 0 Then Return operand1
            If operand1 Is Nothing Then operand1 = New Blob

            Dim l As Long = operand1.Length
            Dim l2 As Integer = operand2.Length * 16

            operand1.Length += l2
            MemCpy(operand1.handle + l, operand2, l2)

            Return operand1
        End Operator

        Public Shared Operator &(operand1 As Blob, operand2 As Guid()) As Blob
            If operand2 Is Nothing OrElse operand2.Length = 0 Then Return operand1
            If operand1 Is Nothing Then operand1 = New Blob

            Dim l As Long = operand1.Length
            Dim l2 As Integer = operand2.Length * 16

            operand1.Length += l2
            MemCpy(operand1.handle + l, operand2, l2)

            Return operand1
        End Operator

        Public Shared Operator &(operand1 As Blob, operand2 As System.Drawing.Color()) As Blob
            If operand2 Is Nothing OrElse operand2.Length = 0 Then Return operand1
            If operand1 Is Nothing Then operand1 = New Blob

            Dim l As Long = operand1.Length
            Dim l2 As Integer = operand2.Length * 4

            operand1.Length += l2
            QuickCopyObject(Of System.Drawing.Color())(operand1.handle + l, operand2, l2)

            Return operand1
        End Operator

        Public Shared Operator &(operand1 As Blob, operand2 As Byte) As Blob
            If operand1 Is Nothing Then operand1 = New Blob

            Dim l As Long = operand1.Length

            operand1.Length += 1
            operand1.ByteAt(l) = operand2

            Return operand1
        End Operator

        Public Shared Operator &(operand1 As Blob, operand2 As SByte) As Blob
            If operand1 Is Nothing Then operand1 = New Blob

            Dim l As Long = operand1.Length

            operand1.Length += 1
            operand1.SByteAt(l) = operand2

            Return operand1
        End Operator

        Public Shared Operator &(operand1 As Blob, operand2 As Short) As Blob
            If operand1 Is Nothing Then operand1 = New Blob

            Dim l As Long = operand1.Length

            operand1.Length += 2
            operand1.ShortAtAbsolute(l) = operand2

            Return operand1
        End Operator

        Public Shared Operator &(operand1 As Blob, operand2 As UShort) As Blob
            If operand1 Is Nothing Then operand1 = New Blob

            Dim l As Long = operand1.Length

            operand1.Length += 2
            operand1.UShortAtAbsolute(l) = operand2

            Return operand1
        End Operator

        Public Shared Operator &(operand1 As Blob, operand2 As Integer) As Blob
            If operand1 Is Nothing Then operand1 = New Blob

            Dim p As Long = operand1.virtLen
            Dim l As Long = p + 4

            If l > operand1.actualLen Then
                operand1.ReAlloc(l)
            Else
                operand1.virtLen = l
            End If

            operand1.IntegerAtAbsolute(p) = operand2

            Return operand1
        End Operator

        Public Shared Operator &(operand1 As Blob, operand2 As UInteger) As Blob
            If operand1 Is Nothing Then operand1 = New Blob

            Dim l As Long = operand1.Length

            operand1.Length += 4
            operand1.UIntegerAtAbsolute(l) = operand2

            Return operand1
        End Operator

        Public Shared Operator &(operand1 As Blob, operand2 As Long) As Blob
            If operand1 Is Nothing Then operand1 = New Blob

            Dim l As Long = operand1.Length

            operand1.Length += 8
            operand1.LongAtAbsolute(l) = operand2

            Return operand1
        End Operator

        Public Shared Operator &(operand1 As Blob, operand2 As ULong) As Blob
            If operand1 Is Nothing Then operand1 = New Blob

            Dim l As Long = operand1.Length

            operand1.Length += 8
            operand1.ULongAtAbsolute(l) = operand2

            Return operand1
        End Operator

        Public Shared Operator &(operand1 As Blob, operand2 As Single) As Blob
            If operand1 Is Nothing Then operand1 = New Blob

            Dim l As Long = operand1.Length

            operand1.Length += 4
            operand1.SingleAtAbsolute(l) = operand2

            Return operand1
        End Operator

        Public Shared Operator &(operand1 As Blob, operand2 As Double) As Blob
            If operand1 Is Nothing Then operand1 = New Blob

            Dim l As Long = operand1.Length

            operand1.Length += 8
            operand1.DoubleAtAbsolute(l) = operand2

            Return operand1
        End Operator

        Public Shared Operator &(operand1 As Blob, operand2 As Decimal) As Blob
            If operand1 Is Nothing Then operand1 = New Blob

            Dim l As Long = operand1.Length

            operand1.Length += 16
            operand1.DecimalAtAbsolute(l) = operand2

            Return operand1
        End Operator

        Public Shared Operator &(operand1 As Blob, operand2 As Guid) As Blob
            If operand1 Is Nothing Then operand1 = New Blob

            Dim l As Long = operand1.Length

            operand1.Length += 16
            operand1.GuidAtAbsolute(l) = operand2

            Return operand1
        End Operator

        Public Shared Operator &(operand1 As Blob, operand2 As String) As Blob
            If operand1 Is Nothing Then operand1 = New Blob

            Dim l As Long = operand1.Length
            Dim c As Long

            c = ((operand2.Length * 2))
            If (operand1.StringCatNoNull = False) Then
                c += 2
            End If

            operand1.Length += c
            operand1.SetString(l, operand2)

            Return operand1
        End Operator

        Public Shared Operator &(operand1 As Blob, operand2 As System.Drawing.Color) As Blob
            If operand1 Is Nothing Then operand1 = New Blob

            Dim l As Long = operand1.Length

            operand1.Length += 4
            operand1.ColorAtAbsolute(l) = operand2

            Return operand1
        End Operator

        Public Shared Operator &(operand1 As Blob, operand2 As System.Drawing.Image) As Blob
            If operand1 Is Nothing Then operand1 = New Blob
            Dim bl As New Blob
            bl.SetImage(0, operand2)

            Dim l As Long = operand1.Length
            operand1.Length += bl.Length

            MemCpy(operand1.handle + l, bl.handle, bl.Length)
            bl.Free()

            Return operand1
        End Operator

#End Region

#Region "Concatenation From Blob"

        Public Shared Operator &(operand1 As Byte, operand2 As Blob) As Byte
            Dim res As Byte = operand2.ByteAt(operand2._ClipNext)
            operand2._ClipNext += 1
            Return res
        End Operator

        Public Shared Operator &(operand1 As SByte, operand2 As Blob) As SByte
            Dim res As SByte = operand2.SByteAt(operand2._ClipNext)
            operand2._ClipNext += 1
            Return res
        End Operator

        Public Shared Operator &(operand1 As UShort, operand2 As Blob) As UShort
            Dim res As UShort = operand2.UShortAtAbsolute(operand2._ClipNext)
            operand2._ClipNext += 2
            Return res
        End Operator

        Public Shared Operator &(operand1 As Short, operand2 As Blob) As Short
            Dim res As Short = operand2.ShortAtAbsolute(operand2._ClipNext)
            operand2._ClipNext += 2
            Return res
        End Operator

        Public Shared Operator &(operand1 As UInteger, operand2 As Blob) As UInteger
            Dim res As UInteger = operand2.UIntegerAtAbsolute(operand2._ClipNext)
            operand2._ClipNext += 4
            Return res
        End Operator

        Public Shared Operator &(operand1 As Integer, operand2 As Blob) As Integer
            Dim res As Integer = operand2.IntegerAtAbsolute(operand2._ClipNext)
            operand2._ClipNext += 4
            Return res
        End Operator

        Public Shared Operator &(operand1 As ULong, operand2 As Blob) As ULong
            Dim res As ULong = operand2.ULongAtAbsolute(operand2._ClipNext)
            operand2._ClipNext += 8
            Return res
        End Operator

        Public Shared Operator &(operand1 As Long, operand2 As Blob) As Long
            Dim res As Long = operand2.LongAtAbsolute(operand2._ClipNext)
            operand2._ClipNext += 8
            Return res
        End Operator

        Public Shared Operator &(operand1 As Single, operand2 As Blob) As Single
            Dim res As Single = operand2.SingleAtAbsolute(operand2._ClipNext)
            operand2._ClipNext += 4
            Return res
        End Operator

        Public Shared Operator &(operand1 As Double, operand2 As Blob) As Double
            Dim res As Double = operand2.DoubleAtAbsolute(operand2._ClipNext)
            operand2._ClipNext += 8
            Return res
        End Operator

        Public Shared Operator &(operand1 As Decimal, operand2 As Blob) As Decimal
            Dim res As Decimal = operand2.DecimalAtAbsolute(operand2._ClipNext)
            operand2._ClipNext += 16
            Return res
        End Operator

        Public Shared Operator &(operand1 As System.Guid, operand2 As Blob) As System.Guid
            Dim res As System.Guid = operand2.GuidAtAbsolute(operand2._ClipNext)
            operand2._ClipNext += 16
            Return res
        End Operator

        Public Shared Operator &(operand1 As String, operand2 As Blob) As String
            Dim res As String = operand2.GrabString(operand2._ClipNext)
            operand2._ClipNext += ((res.Length + 1) * 2)
            Return res
        End Operator

        Public Shared Operator &(operand1 As Short(), operand2 As Blob) As Short()
            Dim res As Short() = operand2
            Dim l As Integer = operand1.Length

            ReDim Preserve operand1((operand1.Length + res.Length) - 1)
            Array.Copy(res, 0, operand1, l, res.Length)

            Return operand1
        End Operator

        Public Shared Operator &(operand1 As UShort(), operand2 As Blob) As UShort()
            Dim res As UShort() = operand2
            Dim l As Integer = operand1.Length

            ReDim Preserve operand1((operand1.Length + res.Length) - 1)
            Array.Copy(res, 0, operand1, l, res.Length)

            Return operand1
        End Operator

        Public Shared Operator &(operand1 As Integer(), operand2 As Blob) As Integer()
            Dim res As Integer() = operand2
            Dim l As Integer = operand1.Length

            ReDim Preserve operand1((operand1.Length + res.Length) - 1)
            Array.Copy(res, 0, operand1, l, res.Length)

            Return operand1
        End Operator

        Public Shared Operator &(operand1 As UInteger(), operand2 As Blob) As UInteger()
            Dim res As UInteger() = operand2
            Dim l As Integer = operand1.Length

            ReDim Preserve operand1((operand1.Length + res.Length) - 1)
            Array.Copy(res, 0, operand1, l, res.Length)

            Return operand1
        End Operator

        Public Shared Operator &(operand1 As Long(), operand2 As Blob) As Long()
            Dim res As Long() = operand2
            Dim l As Integer = operand1.Length

            ReDim Preserve operand1((operand1.Length + res.Length) - 1)
            Array.Copy(res, 0, operand1, l, res.Length)

            Return operand1
        End Operator

        Public Shared Operator &(operand1 As ULong(), operand2 As Blob) As ULong()
            Dim res As ULong() = operand2
            Dim l As Integer = operand1.Length

            ReDim Preserve operand1((operand1.Length + res.Length) - 1)
            Array.Copy(res, 0, operand1, l, res.Length)

            Return operand1
        End Operator

        Public Shared Operator &(operand1 As Single(), operand2 As Blob) As Single()
            Dim res As Single() = operand2
            Dim l As Integer = operand1.Length

            ReDim Preserve operand1((operand1.Length + res.Length) - 1)
            Array.Copy(res, 0, operand1, l, res.Length)

            Return operand1
        End Operator

        Public Shared Operator &(operand1 As Double(), operand2 As Blob) As Double()
            Dim res As Double() = operand2
            Dim l As Integer = operand1.Length

            ReDim Preserve operand1((operand1.Length + res.Length) - 1)
            Array.Copy(res, 0, operand1, l, res.Length)

            Return operand1
        End Operator

        Public Shared Operator &(operand1 As Guid(), operand2 As Blob) As Guid()
            Dim res As Guid() = operand2
            Dim l As Integer = operand1.Length

            ReDim Preserve operand1((operand1.Length + res.Length) - 1)
            Array.Copy(res, 0, operand1, l, res.Length)

            Return operand1
        End Operator

        Public Shared Operator &(operand1 As Decimal(), operand2 As Blob) As Decimal()
            Dim res As Decimal() = operand2
            Dim l As Integer = operand1.Length

            ReDim Preserve operand1((operand1.Length + res.Length) - 1)
            Array.Copy(res, 0, operand1, l, res.Length)

            Return operand1
        End Operator

#End Region

#End Region '' Concatenation

#Region "IEnumerable Implementation"

        Public Function GetEnumerator() As IEnumerator(Of Byte)
            GetEnumerator = New BlobEnumeratorByte(Me)
        End Function

        Public Function GetEnumerator1() As IEnumerator
            GetEnumerator1 = New BlobEnumeratorByte(Me)
        End Function

        Public Function GetEnumerator2() As IEnumerator(Of Char)
            GetEnumerator2 = New BlobEnumeratorChar(Me)
        End Function

#End Region '' IEnumerable Implementation

#Region "ICloneable Implementation"

        Public Function Clone() As Object
            Dim bl As New Blob
            Dim l As Long = Length

            bl._MemType = _MemType

            Select Case _MemType

                Case MemAllocType.Virtual
                    bl.VirtualAlloc(l)

                Case MemAllocType.Com
                    bl.handle = Marshal.AllocCoTaskMem(l)
                    bl.virtLen = l

                Case MemAllocType.Network
                    bl.NetAlloc(l)

                Case Else
                    bl.Alloc(l)

            End Select

            If bl.IsInvalid Then Return Nothing

            If l > Int32.MaxValue Then
                n_memcpy(bl.handle, handle, l)
            Else
                MemCpy(bl.handle, handle, l)
            End If

            Clone = bl
        End Function

#End Region '' ICloneable Implementation

#Region "Copying"

        ''' <summary>
        ''' Copies the buffer into the specified byte array starting at the optional startIndex
        ''' </summary>
        <Description("Copies the buffer into the specified byte array starting at the optional startIndex")>
        Public Overloads Sub CopyTo(destinationArray() As Byte, Optional startIndex As Integer = 0)
            Dim gh As GCHandle = GCHandle.Alloc(destinationArray, GCHandleType.Pinned)
            Dim gpt = gh.AddrOfPinnedObject + startIndex

            MemCpy(gpt, handle, Length)
            gh.Free()
        End Sub

        ''' <summary>
        ''' Copies the buffer into the specified byte array starting at the startIndex with the specified length.
        ''' </summary>
        <Description("Copies the buffer into the specified byte array starting at the optional startIndex")>
        Public Overloads Sub CopyTo(destinationArray() As Byte, startIndex As Integer, length As Integer)
            Dim gh As GCHandle = GCHandle.Alloc(destinationArray, GCHandleType.Pinned)
            Dim gpt = gh.AddrOfPinnedObject + startIndex

            MemCpy(gpt, handle, length)
            gh.Free()
        End Sub

        ''' <summary>
        ''' Copies the buffer into the specified byte array starting at the startIndex with the specified length.
        ''' </summary>
        <Description("Copies the buffer into the specified byte array starting at the optional startIndex")>
        Public Overloads Sub CopyTo(blobStartIndex As Integer, destinationArray() As Byte, startIndex As Integer, length As Integer)
            Dim gh As GCHandle = GCHandle.Alloc(destinationArray, GCHandleType.Pinned)
            Dim gpt = gh.AddrOfPinnedObject

            If (blobStartIndex + length) > Me.Length Then
                length = Me.Length - blobStartIndex
            End If

            MemCpy(gpt, handle + blobStartIndex, length)
            gh.Free()
        End Sub

        ''' <summary>
        ''' Concat into this byte array.
        ''' </summary>
        <Description("Concat into this byte array.")>
        Public Sub Concat(second() As Byte)
            If Length = 0 Then
                Length = second.Length
                SetBytes(0, second)
            Else
                If _Locked Then Return
                Dim c As Integer = Length
                Length += second.Length
                MemCpy(handle + c, second, second.Length)
            End If

            Align()
        End Sub

        ''' <summary>
        ''' Copies memory from this memory pointer into the pointer specified.
        ''' </summary>
        ''' <param name="ptr">The pointer to which to copy the memory.</param>
        ''' <param name="size">The size of the buffer to copy.</param>
        ''' <remarks></remarks>
        Public Overloads Sub CopyTo(ptr As IntPtr, size As IntPtr)
            If size.ToInt64 <= UInt32.MaxValue Then
                MemCpy(ptr, handle, CUInt(size))
            Else
                n_memcpy(ptr, handle, CLng(size))
            End If
        End Sub

        ''' <summary>
        ''' Copies memory from another memory pointer into this one.
        ''' If this one is not yet allocated, it will automatically be allocated
        ''' to the size specified.
        ''' </summary>
        ''' <param name="ptr">The pointer from which to copy the memory.</param>
        ''' <param name="size">The size of the buffer to copy.</param>
        ''' <remarks></remarks>
        Public Sub CopyFrom(ptr As IntPtr, size As IntPtr)
            If handle <> 0 OrElse Alloc(size.ToInt64) Then
                If size.ToInt64 <= UInt32.MaxValue Then
                    MemCpy(handle, ptr, CUInt(size))
                Else
                    n_memcpy(handle, ptr, CLng(size))
                End If
            End If
        End Sub

        ''' <summary>
        ''' Copies one memory location to another.
        ''' </summary>
        ''' <param name="dest"></param>
        ''' <param name="src"></param>
        ''' <param name="length"></param>
        ''' <remarks></remarks>
        Public Shared Sub CopyMemory(dest As IntPtr, src As IntPtr, length As Integer)
            Native.MemCpy(dest, src, length)
        End Sub

        ''' <summary>
        ''' Copies one memory location to another, long version.
        ''' </summary>
        ''' <param name="dest"></param>
        ''' <param name="src"></param>
        ''' <param name="length"></param>
        ''' <remarks></remarks>
        Public Shared Sub CopyMemoryLong(dest As IntPtr, src As IntPtr, length As Long)
            n_memcpy(dest, src, length)
        End Sub

#End Region '' Copying

#Region "String and byte extraction functions"

        ''' <summary>
        ''' Returns a string from a pointer stored at a memory location in this object's pointer.
        ''' </summary>
        ''' <param name="index"></param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StringIndirectAt(index As IntPtr) As String
            Get
                Return GrabStringFromPointerAt(index)
            End Get
            Set(value As String)
                SetStringAtPointerIndex(index, value)
            End Set
        End Property

        ''' <summary>
        ''' Returns the length of a null-terminated Unicode string at the specified byteIndex.
        ''' </summary>
        ''' <param name="byteIndex"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function StrLen(byteIndex As IntPtr) As Integer
            StrLen = CharZero(byteIndex + handle)
        End Function

        ''' <summary>
        ''' Grabs a null-terminated Unicode string from a position relative to the memory pointer.
        ''' </summary>
        ''' <param name="byteIndex">A 32 or 64-bit number indicating the starting byte position relative to the pointer.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function GrabString(byteIndex As IntPtr) As String
            Return Nothing
        End Function

        ''' <summary>
        ''' Grabs a null-terminated Unicode string from a pointer at a the specified position relative to the memory pointer.
        ''' </summary>
        ''' <param name="byteIndex">A 32 or 64-bit number indicating the starting byte position relative to the pointer.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function GrabStringFromPointerAtAbsolute(byteIndex As IntPtr) As String
            Return Nothing
        End Function

        ''' <summary>
        ''' Grabs a null-terminated Unicode string from a pointer at a the specified position, in an array of pointers, relative to the memory pointer.
        ''' </summary>
        ''' <param name="index">A 32 or 64-bit number indicating the starting pointer collection position relative to the pointer.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function GrabStringFromPointerAt(index As IntPtr) As String
            Return Nothing
        End Function

        ''' <summary>
        ''' Grabs a null-terminated Unicode string from a position relative to the memory pointer with the exact specified length.
        ''' No null-termination checking is performed.
        ''' </summary>
        ''' <param name="byteIndex">A 32 or 64-bit number indicating the starting byte position relative to the pointer.</param>
        ''' <param name="length">The length of the string, in characters.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GrabString(byteIndex As IntPtr, length As Integer) As String
            GrabString = GetCharArray(byteIndex, length)
        End Function

        ''' <summary>
        ''' Grabs a null-terminated ASCII string from a position relative to the memory pointer.
        ''' </summary>
        ''' <param name="byteIndex">A 32 or 64-bit number indicating the starting position relative to the pointer.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GrabAsciiString(byteIndex As IntPtr) As String

            Dim tp As New IntPtr(CLng(handle) + byteIndex.ToInt64)
            Dim e As Integer = ByteZero(tp)
            Dim ba() As Byte

            If e = 0 Then Return ""
            ReDim ba(e - 1)

            MemCpy(ba, handle + byteIndex, e)
            GrabAsciiString = System.Text.Encoding.ASCII.GetString(ba)

        End Function

        ''' <summary>
        ''' Grabs a null-terminated ASCII string from a position relative to the memory pointer with the exact specified length.
        ''' No null-termination checking is performed.
        ''' </summary>
        ''' <param name="byteIndex">A 32 or 64-bit number indicating the starting byte position relative to the pointer.</param>
        ''' <param name="length">The length of the string, in characters.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GrabAsciiString(byteIndex As IntPtr, length As Integer) As String

            If handle = 0 Then Return Nothing
            If length <= 0 Then Throw New IndexOutOfRangeException("length must be greater than zero")

            Dim ba() As Byte
            ReDim ba(length - 1)

            MemCpy(ba, handle + byteIndex, length)
            GrabAsciiString = System.Text.Encoding.ASCII.GetString(ba)

        End Function

        ''' <summary>
        ''' Grabs a null-terminated UTF-8 string from a position relative to the memory pointer.
        ''' </summary>
        ''' <param name="byteIndex">A 32 or 64-bit number indicating the starting position relative to the pointer.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function GrabUtf8String(byteIndex As IntPtr) As String
            Return Nothing
        End Function

        ''' <summary>
        ''' Grabs a null-terminated UTF8 string from a position relative to the memory pointer with the exact specified length.
        ''' No null-termination checking is performed.
        ''' </summary>
        ''' <param name="byteIndex">A 32 or 64-bit number indicating the starting byte position relative to the pointer.</param>
        ''' <param name="length">The length of the string, in characters.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GrabUTF8String(byteIndex As IntPtr, length As Integer) As String
            GrabUTF8String = System.Text.Encoding.UTF8.GetString(GrabBytes(byteIndex, length))
        End Function

        ''' <summary>
        ''' Grabs a null-terminated Unicode string array (MULTISZ) from a position relative to the memory pointer.
        ''' </summary>
        ''' <param name="byteIndex">A 32 or 64-bit number indicating the starting position relative to the pointer.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GrabStringArray(byteIndex As IntPtr) As String()
            If handle = 0 Then Return Nothing

            Dim b As Char
            Dim i As Integer = 0
            Dim sb As Long = CLng(byteIndex)

            Dim sout() As String = Nothing
            Dim ct As Integer = 0
            Dim l As Long = CLng(byteIndex)
            Dim tp As IntPtr = New IntPtr(l + CLng(handle))

            b = CharAtAbsolute(l)

            While b <> ChrW(0)
                i = CharZero(tp)
                ReDim Preserve sout(ct)
                sout(ct) = New String(ChrW(0), i)
                QuickCopyObject(sout(ct), tp, CUInt(i << 1))

                l += CInt((i << 1) + 2)
                tp += CInt((i << 1) + 2)

                b = CharAtAbsolute(l)

                ct += 1
            End While

            Return sout
        End Function

        ''' <summary>
        ''' Writes the specified string array to the specified byte index using the specified optional character encoding.
        ''' A null termination character is appended to the string before the encoding conversion.
        ''' This function attempts to ensure sufficient memory allocation.
        ''' </summary>
        ''' <param name="byteIndex">The byte index within the memory buffer to begin copying.</param>
        ''' <param name="strings">The string array to set.</param>
        ''' <param name="enc">Optional <see cref="System.Text.Encoding" /> object (default is Windows Unicode = UTF16LE / wchar_t).</param>
        ''' <returns>The total number of bytes that were stored, including the null termination character or characters.</returns>
        ''' <remarks></remarks>
        Public Function SetStringArray(byteIndex As IntPtr, strings() As String, enc As System.Text.Encoding) As Integer
            Dim x As Long
            x = byteIndex

            For Each s In strings
                x += SetString(x, s, enc)
            Next

            Return x
        End Function

        ''' <summary>
        ''' Sets the memory at the specified byte index to the specified string using the optional specified encoding.
        ''' A null termination character is appended to the string before the encoding conversion.
        ''' This function attempts to ensure sufficient memory allocation.
        ''' </summary>
        ''' <param name="byteIndex">The byte index within the memory buffer to begin copying.</param>
        ''' <param name="s">The string value to set.</param>
        ''' <param name="enc">Optional <see cref="System.Text.Encoding" /> object (default is Windows Unicode = UTF16LE / wchar_t).</param>
        ''' <returns>The total number of bytes that were stored, including the null termination character or characters.</returns>
        ''' <remarks></remarks>
        Public Function SetString(byteIndex As IntPtr, s As String, enc As System.Text.Encoding) As Integer
            If enc Is Nothing Then enc = System.Text.Encoding.Unicode

            Dim p As IntPtr = handle + byteIndex
            Dim b() As Byte = enc.GetBytes(s & ChrW(0))
            Dim x As Integer = b.Length

            If Length < CLng(x + byteIndex) Then Length = CLng(x + byteIndex)

            SetByteArray(p, b)
            Return x
        End Function


        ''' <summary>
        ''' Writes the specified string array to the specified byte index using the specified optional character encoding.
        ''' A null termination character is appended to the string before the encoding conversion.
        ''' This function will auto-allocate memory.
        ''' </summary>
        ''' <param name="byteIndex">The byte index within the memory buffer to begin copying.</param>
        ''' <param name="strings">The string array to set.</param>
        ''' <returns>The total number of bytes that were stored, including the null termination character or characters.</returns>
        ''' <remarks></remarks>
        Public Function SetStringArray(byteIndex As IntPtr, strings() As String) As Integer
            Dim x As Long = 0

            For Each s In strings
                x += ((s.Length << 1) + 2)
            Next

            If Length < CLng(x + byteIndex) Then Length = CLng(x + byteIndex)

            x = byteIndex

            For Each s In strings
                SetString(x, s)
                x += ((s.Length << 1) + 2)
            Next

            Return x
        End Function

        ''' <summary>
        ''' Sets the memory at the specified byte index to the specified string.
        ''' A null termination character is appended to the string.
        ''' 
        ''' This method only auto-allocates if the buffer pointer is null.  If the buffer pointer is not null,
        ''' it is up to the caller to make sure that sufficient memory has been allocated to execute this task.
        ''' </summary>
        ''' <param name="byteIndex">The byte index within the memory buffer to begin copying.</param>
        ''' <param name="s">The string to set.</param>
        ''' <remarks></remarks>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Sub SetString(byteIndex As IntPtr, s As String)
        End Sub

        ''' <summary>
        ''' Sets a buffer referenced by the memory at the specified byte index to the specified string.
        ''' A null termination character is appended to the string.
        ''' </summary>
        ''' <param name="byteIndex">The absolute position in the buffer.</param>
        ''' <param name="s">The string to set.</param>
        ''' <remarks></remarks>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Sub SetStringAtPointer(byteIndex As IntPtr, s As String)
        End Sub

        ''' <summary>
        ''' Sets a buffer referenced by the memory at the specified handle index to the specified string.
        ''' A null termination character is appended to the string.
        ''' </summary>
        ''' <param name="index">The handle index.</param>
        ''' <param name="s">The string to set.</param>
        ''' <remarks></remarks>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Sub SetStringAtPointerIndex(index As IntPtr, s As String)
        End Sub

        ''' <summary>
        ''' Sets the memory at the specified index to the specified byte array.
        ''' </summary>
        ''' <param name="byteIndex"></param>
        ''' <param name="data"></param>
        ''' <remarks></remarks>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Overridable Sub SetBytes(byteIndex As IntPtr, data() As Byte)
        End Sub

        ''' <summary>
        ''' Sets the memory at the specified index to the specified byte array for the specified length.
        ''' </summary>
        ''' <param name="byteIndex"></param>
        ''' <param name="data"></param>
        ''' <remarks></remarks>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Overridable Sub SetBytes(byteIndex As IntPtr, data() As Byte, length As Integer)
        End Sub

        ''' <summary>
        ''' Get an array of bytes at the specified position of the specified length.
        ''' </summary>
        ''' <param name="byteIndex">A 32 or 64-bit number indicating the starting position relative to the pointer.</param>
        ''' <param name="length">The number of bytes to grab.</param>
        ''' <returns>A new byte array with the requested data.</returns>
        ''' <remarks></remarks>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Overridable Function GrabBytes(byteIndex As IntPtr, length As Integer) As Byte()
            Return Nothing
        End Function

        Public Overridable Function GrabBytes() As Byte()
            Return GrabBytes(0, Length)
        End Function

        ''' <summary>
        ''' Sets the memory at the specified index to the specified sbyte array.
        ''' </summary>
        ''' <param name="byteIndex"></param>
        ''' <param name="data"></param>
        ''' <remarks></remarks>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Overridable Sub SetSBytes(byteIndex As IntPtr, data() As Byte)
        End Sub

        ''' <summary>
        ''' Get an array of sbytes at the specified position of the specified length.
        ''' </summary>
        ''' <param name="byteIndex">A 32 or 64-bit number indicating the starting position relative to the pointer.</param>
        ''' <param name="length">The number of bytes to grab.</param>
        ''' <returns>A new byte array with the requested data.</returns>
        ''' <remarks></remarks>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Overridable Function GrabSBytes(byteIndex As IntPtr, length As Integer) As Byte()
            Return Nothing
        End Function

        ''' <summary>
        ''' Get an array of bytes at the specified position of the specified length.
        ''' </summary>
        ''' <param name="byteIndex">A 32 or 64-bit number indicating the starting position relative to the pointer.</param>
        ''' <param name="length">The number of bytes to grab.</param>
        ''' <param name="data">
        ''' The data buffer into which the memory will be copied.
        ''' If this value is Nothing or the size of the buffer is too small, then a new buffer will be allocated.
        ''' </param>
        ''' <remarks></remarks>
        Public Overridable Sub GrabBytes(byteIndex As IntPtr, length As Integer, ByRef data() As Byte)
            If handle = 0 Then Return

            If data Is Nothing Then
                ReDim data(length - 1)
            ElseIf data.Length < length Then
                ReDim data(length - 1)
            End If

            byteGet(byteIndex, length, data)
        End Sub

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Protected Sub byteGet(byteIndex As IntPtr, length As Integer, ByRef data() As Byte)
        End Sub

        ''' <summary>
        ''' Get an array of bytes at the specified position of the specified length.
        ''' </summary>
        ''' <param name="byteIndex">A 32 or 64-bit number indicating the starting position relative to the pointer.</param>
        ''' <param name="length">The number of bytes to grab.</param>
        ''' <param name="data">
        ''' The data buffer into which the memory will be copied.
        ''' If this value is Nothing or the size of the buffer is too small, then the method will fail.
        ''' </param>
        ''' <param name="arrayIndex">The position in the buffer at which to begin copying.</param>
        ''' <remarks></remarks>
        Public Overridable Sub GrabBytes(byteIndex As IntPtr, length As Integer, ByRef data() As Byte, arrayIndex As Integer)
            If handle = 0 Then Return

            If data Is Nothing Then
                Throw New ArgumentNullException("data cannot be null or Nothing.")
            ElseIf length + arrayIndex > data.Length Then
                Throw New ArgumentOutOfRangeException("data buffer length is too small.")
            End If

            Dim gh As GCHandle = GCHandle.Alloc(data, GCHandleType.Pinned)
            Dim pdest As IntPtr = gh.AddrOfPinnedObject + arrayIndex

            MemCpy(pdest, handle + byteIndex, CUInt(length))
            gh.Free()
        End Sub

        ''' <summary>
        ''' Returns the results of the buffer as if it were a BSTR type String.
        ''' </summary>
        ''' <param name="comPtr">Specifies whether or not the current MemPtr is an actual COM pointer to a BSTR.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function BSTR(Optional comPtr As Boolean = True) As String

            If comPtr Then
                Return GrabString(0, IntegerAt(-4))
            Else
                Return GrabString(4, IntegerAt(0))
            End If

        End Function

        ''' <summary>
        ''' Returns the contents of this buffer as a string.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function LpwStr() As String
            LpwStr = GrabString(0)
        End Function

#End Region '' String extraction functions

#Region "Properties and Property-Like Methods"

        ''' <summary>
        ''' Gets or sets a value indicating the kind of memory to allocate.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Property MemoryType As MemAllocType
            Get
                If IsInvalid Then Return MemAllocType.Invalid
                MemoryType = _MemType
            End Get
            Set(value As MemAllocType)
                If Not IsInvalid Then Throw New AccessViolationException("Cannot change memory type on an allocated object.  Free your object, first.")
                _MemType = value
            End Set
        End Property

        ''' <summary>
        ''' Sets the length of the memory block.
        ''' </summary>
        ''' <param name="value"></param>
        ''' <remarks></remarks>
        Public Overridable Sub SetLength(value As Long)
            If hHeap = 0 Then hHeap = GetProcessHeap()
            Alloc(value)
        End Sub

#Region "Crc-32"

        ''' <summary>
        ''' Calculate the CRC 32 for the block of memory represented by this structure.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CalculateCrc32() As UInteger
            Dim l As New IntPtr(Me.Length)
            CalculateCrc32 = Crc32.Calculate(handle, l)
        End Function

        ''' <summary>
        ''' Calculate the CRC 32 for the block of memory represented by this structure.
        ''' </summary>
        ''' <param name="bufflen">The length, in bytes, of the marshaling buffer.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CalculateCrc32(bufflen As Integer) As UInteger
            Dim l As New IntPtr(Me.Length)
            CalculateCrc32 = Crc32.Calculate(handle, l, , bufflen)
        End Function

        ''' <summary>
        ''' Calculate the CRC 32 for the block of memory represented by this structure.
        ''' </summary>
        ''' <param name="length">The length, in bytes, of the buffer to check.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CalculateCrc32(length As IntPtr) As UInteger
            CalculateCrc32 = Crc32.Calculate(handle, length)
        End Function

        ''' <summary>
        ''' Calculate the CRC 32 for the block of memory represented by this structure.
        ''' </summary>
        ''' <param name="length">The length, in bytes, of the buffer to check.</param>
        ''' <param name="bufflen">The length, in bytes, of the marshaling buffer.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CalculateCrc32(length As IntPtr, bufflen As Integer) As UInteger
            CalculateCrc32 = Crc32.Calculate(handle, length, , bufflen)
        End Function

#End Region

#End Region '' Properties and Property-Like Methods

#Region "Structure Utility Methods"

        ''' <summary>
        ''' Converts the contents of an unmanaged pointer into a structure.
        ''' </summary>
        ''' <typeparam name="T">The type of structure requested.</typeparam>
        ''' <returns>New instance of T.</returns>
        ''' <remarks></remarks>
        Public Overridable Function ToStruct(Of T As Structure)() As T
            ToStruct = CType(Marshal.PtrToStructure(handle, GetType(T)), T)
        End Function

        ''' <summary>
        ''' Sets the contents of a structure into an unmanaged pointer.
        ''' </summary>
        ''' <typeparam name="T">The type of structure to set.</typeparam>
        ''' <param name="val">The structure to set.</param>
        ''' <remarks></remarks>
        Public Overridable Sub FromStruct(Of T As Structure)(val As T)
            Dim cb As Integer = Marshal.SizeOf(val)
            If (handle = 0) Then Alloc(cb)
            Marshal.StructureToPtr(val, handle, False)
        End Sub

        ''' <summary>
        ''' Converts the contents of an unmanaged pointer at the specified byte index into a structure.
        ''' </summary>
        ''' <typeparam name="T">The type of structure requested.</typeparam>
        ''' <param name="byteIndex">The byte index relative to the pointer at which to begin copying.</param>
        ''' <returns>New instance of T.</returns>
        ''' <remarks></remarks>
        Public Overridable Function ToStructAt(Of T As Structure)(byteIndex As IntPtr) As T
            ToStructAt = CType(Marshal.PtrToStructure(handle + byteIndex, GetType(T)), T)
        End Function

        ''' <summary>
        ''' Sets the contents of a structure into a memory buffer at the specified byte index.
        ''' </summary>
        ''' <typeparam name="T">The type of structure to set.</typeparam>
        ''' <param name="byteIndex">The byte index relative to the pointer at which to begin copying.</param>
        ''' <param name="val">The structure to set.</param>
        ''' <remarks></remarks>
        Public Overridable Sub FromStructAt(Of T As Structure)(byteIndex As IntPtr, val As T)
            Dim cb As Integer = Marshal.SizeOf(val)
            Marshal.StructureToPtr(val, handle + byteIndex, False)
        End Sub

        ''' <summary>
        ''' Copies the contents of the buffer at the specified index into a blittable structure array.
        ''' </summary>
        ''' <typeparam name="T">The structure type.</typeparam>
        ''' <param name="byteIndex">The index at which to begin copying.</param>
        ''' <returns>An array of T.</returns>
        ''' <remarks></remarks>
        Public Overridable Function ToBlittableStructArrayAt(Of T As Structure)(byteIndex As IntPtr) As T()

            If (handle = 0) Then Return Nothing

            Dim l As Long = Length() - CType(byteIndex, Long)
            Dim cb = Marshal.SizeOf(New T)
            Dim c As Integer = CInt(l / cb)

            Dim tt() As T
            ReDim tt(c - 1)

            Dim gh As GCHandle = GCHandle.Alloc(tt, GCHandleType.Pinned)

            If l <= UInt32.MaxValue Then
                MemCpy(gh.AddrOfPinnedObject, handle, CUInt(l))
            Else
                CopyMemory(gh.AddrOfPinnedObject, handle, l)
            End If

            gh.Free()

            Return tt
        End Function

        ''' <summary>
        ''' Copies a blittable structure array into the buffer at the specified index, initializing a new buffer, if necessary.
        ''' </summary>
        ''' <typeparam name="T">The structure type.</typeparam>
        ''' <param name="byteIndex">The index at which to begin copying.</param>
        ''' <param name="value">The structure array to copy.</param>
        ''' <remarks></remarks>
        Public Overridable Sub FromBlittableStructArrayAt(Of T As Structure)(byteIndex As IntPtr, value As T())

            If (handle = 0) AndAlso (byteIndex <> 0) Then Return

            Dim l As Long
            Dim cb = Marshal.SizeOf(New T)
            Dim c As Integer = value.Count

            l = c * cb

            If (handle = 0) Then
                If Not Alloc(l) Then Return
            End If

            Dim p = handle + byteIndex

            Dim gh As GCHandle = GCHandle.Alloc(value, GCHandleType.Pinned)

            If l <= UInt32.MaxValue Then
                MemCpy(p, gh.AddrOfPinnedObject, CUInt(l))
            Else
                CopyMemory(p, gh.AddrOfPinnedObject, l)
            End If

            gh.Free()

        End Sub

        ''' <summary>
        ''' Copies the contents of the buffer into a blittable structure array.
        ''' </summary>
        ''' <typeparam name="T">The structure type.</typeparam>
        ''' <returns>An array of T.</returns>
        ''' <remarks></remarks>
        Public Overridable Function ToBlittableStructArray(Of T As Structure)() As T()
            Return ToBlittableStructArrayAt(Of T)(0)
        End Function

        ''' <summary>
        ''' Copies a blittable structure array into the buffer, initializing a new buffer, if necessary.
        ''' </summary>
        ''' <typeparam name="T">The structure type.</typeparam>
        ''' <param name="value">The structure array to copy.</param>
        ''' <remarks></remarks>
        Public Overridable Sub FromBlittableStructArray(Of T As Structure)(value As T())
            FromBlittableStructArrayAt(Of T)(0, value)
        End Sub

#End Region '' Structure Utility Methods

#Region "Guid Properties"

        ''' <summary>
        ''' Retrieves or sets an individual GUID structure at the specified absolute byte index in the buffer.
        ''' </summary>
        ''' <param name="index">The position to return.</param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property GuidAtAbsolute(index As Long) As Guid
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Get
                Return Guid.Empty
                'If handle = 0 Then Return Guid.Empty
                'guidAtget(GuidAtAbsolute, CType(CLng(handle) + index, IntPtr))
            End Get
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Set(value As Guid)
                'If handle = 0 Then Return
                'guidAtset(CType(CLng(handle) + index, IntPtr), value)
            End Set
        End Property

        ''' <summary>
        ''' Retrieves or sets an individual GUID structure at the specified index in the buffer treated as an array of GUIDs.
        ''' </summary>
        ''' <param name="index">The position to return.</param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property GuidAt(index As Long) As Guid
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Get
                Return Guid.Empty
                'If handle = 0 Then Return Guid.Empty
                'guidAtget(GuidAt, CType(CLng(handle) + (index * 16), IntPtr))
            End Get
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Set(value As Guid)
                'If handle = 0 Then Return
                'guidAtset(CType(CLng(handle) + (index * 16), IntPtr), value)
            End Set
        End Property

#End Region '' Guid Properties

        '' Integral types accessable via logical indexer.
        '' A logical index means if you were treat a block
        '' of memory as an array of the requested type,
        '' then the result you get will be the element
        '' at the logical position in that array.
        '' So a character at logical index 1 has a byte offset of 2,
        '' for an integer at index 1 the byte offset is 4, etc...
#Region "Integral Indexer Properties"

        ''' <summary>
        ''' Retrieves or sets an individual byte at the specified logical index in the buffer.
        ''' </summary>
        ''' <param name="index">The index to return.</param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ByteAt(index As Long) As Byte
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Get
                Return 0
                'byteAtget(ByteAt, New IntPtr(clng(handle) + index))
            End Get
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Set(value As Byte)
                'byteAtset(New IntPtr(clng(handle) + index), value)
            End Set
        End Property

        ''' <summary>
        ''' Retrieves or sets an individual signed byte at the specified logical index in the buffer.
        ''' </summary>
        ''' <param name="index">The index to return.</param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SByteAt(index As Long) As SByte
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Get
                Return 0
                'sbyteAtget(SByteAt, New IntPtr(clng(handle) + index))
            End Get
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Set(value As SByte)
                'sbyteAtset(New IntPtr(clng(handle) + index), value)
            End Set
        End Property

        ''' <summary>
        ''' Retrieves or sets an individual Unicode character at the specified logical index in the buffer.
        ''' </summary>
        ''' <param name="index">The index to return.</param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CharAt(index As Long) As Char
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Get
                Return ChrW(0)
                'charAtget(CharAt, New IntPtr(clng(handle) + (index * 2)))
            End Get
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Set(value As Char)
                'charAtset(New IntPtr(clng(handle) + (index * 2)), value)
            End Set
        End Property

        ''' <summary>
        ''' Retrieves or sets an individual Short at the specified logical index in the buffer.
        ''' </summary>
        ''' <param name="index">The index to return.</param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ShortAt(index As Long) As Short
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Get
                Return 0
                'shortAtget(ShortAt, New IntPtr(clng(handle) + (index * 2)))
            End Get
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Set(value As Short)
                'shortAtset(New IntPtr(clng(handle) + (index * 2)), value)
            End Set
        End Property

        ''' <summary>
        ''' Retrieves or sets an individual UShort at the specified logical index in the buffer.
        ''' </summary>
        ''' <param name="index">The index to return.</param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property UShortAt(index As Long) As UShort
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Get
                Return 0
                'ushortAtget(UShortAt, New IntPtr(clng(handle) + (index * 2)))
            End Get
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Set(value As UShort)
                'ushortAtset(New IntPtr(clng(handle) + (index * 2)), value)
            End Set
        End Property

        ''' <summary>
        ''' Retrieves or sets an individual Integer at the specified logical index in the buffer.
        ''' </summary>
        ''' <param name="index">The index to return.</param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IntegerAt(index As Long) As Integer
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Get
                Return 0
                'intAtget(IntegerAt, New IntPtr(clng(handle) + (index * 4)))
            End Get
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Set(value As Integer)
                'intAtset(New IntPtr(clng(handle) + (index * 4)), value)
            End Set
        End Property

        ''' <summary>
        ''' Retrieves or sets an individual UInteger at the specified logical index in the buffer.
        ''' </summary>
        ''' <param name="index">The index to return.</param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property UIntegerAt(index As Long) As UInteger
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Get
                Return 0
                'uintAtget(UIntegerAt, New IntPtr(clng(handle) + (index * 4)))
            End Get
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Set(value As UInteger)
                'uintAtset(New IntPtr(clng(handle) + (index * 4)), value)
            End Set
        End Property

        ''' <summary>
        ''' Retrieves or sets an individual Long at the specified logical index in the buffer.
        ''' </summary>
        ''' <param name="index">The index to return.</param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property LongAt(index As Long) As Long
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Get
                Return 0
                'longAtget(LongAt, New IntPtr(clng(handle) + (index * 8)))
            End Get
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Set(value As Long)
                'longAtset(New IntPtr(clng(handle) + (index * 8)), value)
            End Set
        End Property

        ''' <summary>
        ''' Retrieves or sets an individual ULong at the specified logical index in the buffer.
        ''' </summary>
        ''' <param name="index">The index to return.</param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ULongAt(index As Long) As ULong
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Get
                Return 0
                'ulongAtget(ULongAt, New IntPtr(clng(handle) + (index * 8)))
            End Get
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Set(value As ULong)
                'ulongAtset(New IntPtr(clng(handle) + (index * 8)), value)
            End Set
        End Property

        ''' <summary>
        ''' Retrieves or sets an individual Single at the specified logical index in the buffer.
        ''' </summary>
        ''' <param name="index">The index to return.</param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SingleAt(index As Long) As Single
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Get
                Return 0
                'singleAtget(SingleAt, New IntPtr(CLng(handle) + (index * 4)))
            End Get
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Set(value As Single)
                'singleAtset(New IntPtr(CLng(handle) + (index * 4)), value)
            End Set
        End Property

        ''' <summary>
        ''' Retrieves or sets an individual Double at the specified logical index in the buffer.
        ''' </summary>
        ''' <param name="index">The index to return.</param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DoubleAt(index As Long) As Double
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Get
                Return 0
                'doubleAtget(DoubleAt, New IntPtr(CLng(handle) + (index * 8)))
            End Get
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Set(value As Double)
                'doubleAtset(New IntPtr(CLng(handle) + (index * 8)), value)
            End Set
        End Property

        ''' <summary>
        ''' Retrieves or sets an individual Decimal at the specified logical index in the buffer.
        ''' </summary>
        ''' <param name="index">The index to return.</param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DecimalAt(index As Long) As Decimal
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Get
                Return 0
                'decimalAtget(DecimalAt, New IntPtr(CLng(handle) + (index * 16)))
            End Get
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Set(value As Decimal)
                'decimalAtset(New IntPtr(CLng(handle) + (index * 16)), value)
            End Set
        End Property

#End Region '' Integral Indexer Properties

        '' Integral types accessable via absolute byte position.
        '' The value returned is the desired integral value
        '' at the specified absolute byte position
        '' in the buffer.
#Region "Integral Absolute Indexer Properties"

        ''' <summary>
        ''' Retrieves or sets an individual Unicode character at the specified byte position in the buffer.
        ''' </summary>
        ''' <param name="index">The byte position to return.</param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CharAtAbsolute(index As Long) As Char
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Get
                Return ChrW(0)
                'charAtget(CharAtAbsolute, New IntPtr(clng(handle) + index))
            End Get
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Set(value As Char)
                'charAtset(New IntPtr(clng(handle) + index), value)
            End Set
        End Property

        ''' <summary>
        ''' Retrieves or sets an individual Short at the specified byte position in the buffer.
        ''' </summary>
        ''' <param name="index">The byte position to return.</param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ShortAtAbsolute(index As Long) As Short
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Get
                Return 0
                'shortAtget(ShortAtAbsolute, New IntPtr(clng(handle) + (index)))
            End Get
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Set(value As Short)
                'shortAtset(New IntPtr(clng(handle) + (index)), value)
            End Set
        End Property

        ''' <summary>
        ''' Retrieves or sets an individual UShort at the specified byte position in the buffer.
        ''' </summary>
        ''' <param name="index">The byte position to return.</param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property UShortAtAbsolute(index As Long) As UShort
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Get
                Return 0
                'ushortAtget(UShortAtAbsolute, New IntPtr(clng(handle) + (index)))
            End Get
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Set(value As UShort)
                'ushortAtset(New IntPtr(clng(handle) + (index)), value)
            End Set
        End Property

        ''' <summary>
        ''' Retrieves or sets an individual Integer at the specified byte position in the buffer.
        ''' </summary>
        ''' <param name="index">The byte position to return.</param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IntegerAtAbsolute(index As Long) As Integer
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Get
                Return 0
                'intAtget(IntegerAtAbsolute, New IntPtr(clng(handle) + (index)))
            End Get
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Set(value As Integer)
                'intAtset(New IntPtr(clng(handle) + (index)), value)
            End Set
        End Property

        ''' <summary>
        ''' Retrieves or sets an individual UInteger at the specified byte position in the buffer.
        ''' </summary>
        ''' <param name="index">The byte position to return.</param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property UIntegerAtAbsolute(index As Long) As UInteger
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Get
                Return 0
                'uintAtget(UIntegerAtAbsolute, New IntPtr(clng(handle) + (index)))
            End Get
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Set(value As UInteger)
                'uintAtset(New IntPtr(clng(handle) + (index)), value)
            End Set
        End Property

        ''' <summary>
        ''' Retrieves or sets an individual Long at the specified byte position in the buffer.
        ''' </summary>
        ''' <param name="index">The byte position to return.</param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property LongAtAbsolute(index As Long) As Long
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Get
                Return 0
                'longAtget(LongAtAbsolute, New IntPtr(clng(handle) + (index)))
            End Get
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Set(value As Long)
                'longAtset(New IntPtr(clng(handle) + (index)), value)
            End Set
        End Property

        ''' <summary>
        ''' Retrieves or sets an individual ULong at the specified byte position in the buffer.
        ''' </summary>
        ''' <param name="index">The byte position to return.</param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ULongAtAbsolute(index As Long) As ULong
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Get
                Return 0
                'ulongAtget(ULongAtAbsolute, New IntPtr(clng(handle) + (index)))
            End Get
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Set(value As ULong)
                'ulongAtset(New IntPtr(clng(handle) + (index)), value)
            End Set
        End Property

        ''' <summary>
        ''' Retrieves or sets an individual Single at the specified byte position in the buffer.
        ''' </summary>
        ''' <param name="index">The byte position to return.</param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SingleAtAbsolute(index As Long) As Single
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Get
                Return 0
                'singleAtget(SingleAtAbsolute, New IntPtr(CLng(handle) + (index)))
            End Get
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Set(value As Single)
                'singleAtset(New IntPtr(CLng(handle) + (index)), value)
            End Set
        End Property

        ''' <summary>
        ''' Retrieves or sets an individual Double at the specified byte position in the buffer.
        ''' </summary>
        ''' <param name="index">The byte position to return.</param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DoubleAtAbsolute(index As Long) As Double
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Get
                Return 0
                'doubleAtget(DoubleAtAbsolute, New IntPtr(CLng(handle) + (index)))
            End Get
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Set(value As Double)
                'doubleAtset(New IntPtr(CLng(handle) + (index)), value)
            End Set
        End Property

        ''' <summary>
        ''' Retrieves or sets an individual Decimal at the specified byte position in the buffer.
        ''' </summary>
        ''' <param name="index">The byte position to return.</param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DecimalAtAbsolute(index As Long) As Decimal
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Get
                Return 0
                'decimalAtget(DecimalAtAbsolute, New IntPtr(CLng(handle) + (index)))
            End Get
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Set(value As Decimal)
                'decimalAtset(New IntPtr(CLng(handle) + (index)), value)
            End Set
        End Property

#End Region '' Integral Absolute Indexer Properties

#Region "Editing"

        ''' <summary>
        ''' Reverses the entire memory pointer.
        ''' </summary>
        ''' <returns>True if successful.</returns>
        ''' <remarks></remarks>
        Public Overridable Function Reverse(Optional asChar As Boolean = False) As Boolean
            If (handle = 0) Then Return False

            Dim l As Long = Length

            Dim i As Long = 0,
                j As Long

            Dim ch As Char,
                b As Byte


            If asChar Then
                l >>= 1
                j = l - 1

                Do
                    ch = CharAt(i)
                    CharAt(i) = CharAt(j)
                    CharAt(j) = ch
                    i += 1
                    j -= 1
                Loop While i < l
            Else
                j = l - 1

                Do
                    b = ByteAt(i)
                    ByteAt(i) = ByteAt(j)
                    ByteAt(j) = b
                    i += 1
                    j -= 1
                Loop While i < l
            End If

            Return True
        End Function

        ''' <summary>
        ''' Slides a block of memory toward the beginning or toward the end of the memory buffer,
        ''' moving the memory around it to the other side.
        ''' </summary>
        ''' <param name="index">The index of the first byte in the affected block.</param>
        ''' <param name="length">The length of the block.</param>
        ''' <param name="offset">
        ''' The offset amount of the slide.  If the amount is negative,
        ''' the block slides toward the beginning of the memory buffer.
        ''' If it is positive, it slides to the right.
        ''' </param>
        ''' <remarks></remarks>
        Public Overridable Sub Slide(index As Long, length As Long, offset As Long)
            If offset = 0 Then Return
            Dim hl As Long = Me.Length
            If hl <= 0 Then Return

            If 0 > (index + length + offset) OrElse (index + length + offset) > hl Then
                Throw New IndexOutOfRangeException("Index out of bounds DataTools.Memory.MemPtr.Slide().")
                Return
            End If

            '' if it's short and sweet, let's make it short and sweet
            '' no need to call p/Invoke ...
            If (length = 1) Then
                If offset = 1 OrElse offset = -1 Then
                    Dim ch As Byte
                    ch = ByteAt(index)
                    ByteAt(index) = ByteAt(index + offset)
                    ByteAt(index + offset) = ch
                    Return
                End If
            ElseIf (length = 2) Then
                If offset = 2 OrElse offset = -2 Then
                    Dim ch As Char
                    ch = CharAtAbsolute(index)
                    CharAtAbsolute(index) = CharAtAbsolute(index + offset)
                    CharAtAbsolute(index + offset) = ch
                    Return
                End If
            ElseIf (length = 4) Then
                If offset = 4 OrElse offset = -4 Then
                    Dim ch As Integer
                    ch = IntegerAtAbsolute(index)
                    IntegerAtAbsolute(index) = IntegerAtAbsolute(index + offset)
                    IntegerAtAbsolute(index + offset) = ch
                    Return
                End If
            ElseIf (length = 8) Then
                If offset = 8 OrElse offset = -8 Then
                    Dim ch As Long
                    ch = LongAtAbsolute(index)
                    LongAtAbsolute(index) = LongAtAbsolute(index + offset)
                    LongAtAbsolute(index + offset) = ch
                    Return
                End If
            End If

            Dim src As IntPtr,
                dest As IntPtr

            src = handle + index
            dest = handle + index + offset

            Dim a As Long = If(offset < 0, offset * -1, offset)

            Dim buff As New MemPtr(length)
            Dim chunk As New MemPtr(a)

            MemCpy(buff.Handle, src, length)
            MemCpy(chunk.Handle, dest, a)

            src = handle + index + offset + length

            MemCpy(src, chunk.Handle, a)
            MemCpy(dest, buff.Handle, length)

            chunk.Free()
            buff.Free()
        End Sub

        ''' <summary>
        ''' Pulls the data in from the specified index.
        ''' </summary>
        ''' <param name="index">The index where contraction begins. The contraction starts at this position.</param>
        ''' <param name="amount">Number of bytes to pull in.</param>
        ''' <param name="removePressure">Specify whether to notify the garbage collector.</param>
        ''' <remarks></remarks>
        Public Overridable Function PullIn(index As Long, amount As Long, Optional removePressure As Boolean = False) As Long
            Dim hl As Long = Length()
            If Length() = 0 OrElse 0 > index OrElse index >= (hl - 1) Then
                Throw New IndexOutOfRangeException("Index out of bounds DataTools.Memory.MemPtr.PullIn().")
                Return -1
            End If

            Dim a As Long = index + amount
            Dim b As Long = Length() - a
            Slide(a, b, -amount)
            ReAlloc(hl - amount)
            Return Length()
        End Function

        ''' <summary>
        ''' Extend the buffer from the specified index.
        ''' </summary>
        ''' <param name="index">The index where expansion begins. The expansion starts at this position.</param>
        ''' <param name="amount">Number of bytes to push out.</param>
        ''' <param name="addPressure">Specify whether to notify the garbage collector.</param>
        ''' <remarks></remarks>
        Public Overridable Function PushOut(index As Long, amount As Long, Optional bytes() As Byte = Nothing, Optional addPressure As Boolean = False) As Long
            Dim hl As Long = Me.Length
            If hl <= 0 Then
                SetLength(amount)
                PushOut = amount
                Exit Function
            End If

            If 0 > index Then
                Throw New IndexOutOfRangeException("Index out of bounds DataTools.Memory.MemPtr.PushOut().")
                Return -1
            End If



            Dim ol As Long = Length - index

            ReAlloc(hl + amount)

            If (ol > 0) Then
                Slide(index, ol, amount)
            End If

            If bytes IsNot Nothing Then
                SetByteArray(index, bytes)
            Else
                ZeroMemory(index, amount)
            End If

            Return Length
        End Function

        ''' <summary>
        ''' Slides a block of memory as Unicode characters toward the beginning or toward the end of the buffer.
        ''' </summary>
        ''' <param name="index">The character index preceding the first character in the affected block.</param>
        ''' <param name="length">The length of the block, in characters.</param>
        ''' <param name="offset">The offset amount of the slide, in characters.  If the amount is negative, the block slides to the left, if it is positive it slides to the right.</param>
        ''' <remarks></remarks>
        Public Overridable Sub SlideChar(index As Long, length As Long, offset As Long)
            Slide(index << 1, length << 1, offset << 1)
        End Sub

        ''' <summary>
        ''' Pulls the data in from the specified character index.
        ''' </summary>
        ''' <param name="index">The index where contraction begins. The contraction starts at this position.</param>
        ''' <param name="amount">Number of characters to pull in.</param>
        ''' <param name="removePressure">Specify whether to notify the garbage collector.</param>
        ''' <remarks></remarks>
        Public Overridable Function PullInChar(index As Long, amount As Long, Optional removePressure As Boolean = False) As Long
            Return PullIn(index << 1, amount << 1)
        End Function

        ''' <summary>
        ''' Extend the buffer from the specified character index.
        ''' </summary>
        ''' <param name="index">The index where expansion begins. The expansion starts at this position.</param>
        ''' <param name="amount">Number of characters to push out.</param>
        ''' <param name="addPressure">Specify whether to notify the garbage collector.</param>
        ''' <remarks></remarks>
        Public Overridable Function PushOutChar(index As Long, amount As Long, Optional chars() As Char = Nothing, Optional addPressure As Boolean = False) As Long
            Return PushOut(index << 1, amount << 1, ToBytes(chars))
        End Function

        ''' <summary>
        ''' Parts the string in both directions from index.
        ''' </summary>
        ''' <param name="index">The index from which to expand.</param>
        ''' <param name="amount">The amount of expansion, in both directions, so the total expansion will be amount * 1.</param>
        ''' <param name="addPressure">Specify whether to notify the garbage collector.</param>
        ''' <remarks></remarks>
        Public Overridable Sub Part(index As Long, amount As Long, Optional addPressure As Boolean = False)
            If handle = 0 Then
                SetLength(amount)
                Exit Sub
            End If

            Dim l As Long = Length()
            If l <= 0 Then Return

            Dim ol As Long = l - index
            ReAlloc(l + (amount * 1))

            Slide(index, ol, amount)
            Slide(index + amount + 1, ol, amount)
        End Sub

        ''' <summary>
        ''' Inserts the specified bytes at the specified index.
        ''' </summary>
        ''' <param name="index">Index at which to insert.</param>
        ''' <param name="value">Byte array to insert</param>
        ''' <param name="addPressure">Specify whether to notify the garbage collector.</param>
        ''' <remarks></remarks>
        Public Overridable Sub Insert(index As Long, value() As Byte, Optional addPressure As Boolean = False)
            PushOut(index, value.Length, value)
        End Sub

        ''' <summary>
        ''' Inserts the specified characters at the specified character index.
        ''' </summary>
        ''' <param name="index">Index at which to insert.</param>
        ''' <param name="value">Character array to insert</param>
        ''' <param name="addPressure">Specify whether to notify the garbage collector.</param>
        ''' <remarks></remarks>
        Public Overridable Sub Insert(index As Long, value() As Char, Optional addPressure As Boolean = False)
            PushOutChar(index, value.Length, value)
        End Sub

        ''' <summary>
        ''' Delete the memory from the specified index.  Calls PullIn.
        ''' </summary>
        ''' <param name="index">Index to start the delete.</param>
        ''' <param name="amount">Amount of bytes to delete</param>
        ''' <param name="removePressure">Specify whether to notify the garbage collector.</param>
        ''' <remarks></remarks>
        Public Overridable Sub Delete(index As Long, amount As Long, Optional removePressure As Boolean = False)
            PullIn(index, amount)
        End Sub

        ''' <summary>
        ''' Delete the memory from the specified character index.  Calls PullIn.
        ''' </summary>
        ''' <param name="index">Index to start the delete.</param>
        ''' <param name="amount">Amount of characters to delete</param>
        ''' <param name="removePressure">Specify whether to notify the garbage collector.</param>
        ''' <remarks></remarks>
        Public Overridable Sub DeleteChar(index As Long, amount As Long, Optional removePressure As Boolean = False)
            PullInChar(index, amount)
        End Sub

        ''' <summary>
        ''' Consumes the buffer in both directions from specified index.
        ''' </summary>
        ''' <param name="index">Index at which consuming begins.</param>
        ''' <param name="amount">Amount of contraction, in both directions, so the total contraction will be amount * 1.</param>
        ''' <param name="removePressure">Specify whether to notify the garbage collector.</param>
        ''' <remarks></remarks>
        Public Overridable Sub Consume(index As Long, amount As Long, Optional removePressure As Boolean = False)
            Dim hl As Long = Length()
            If hl <= 0 OrElse amount > index OrElse index >= ((hl - amount) + 1) Then
                Throw New IndexOutOfRangeException("Index out of bounds DataTools.Memory.Heap:Consume.")
                Return
            End If

            index -= (amount + 1)
            PullIn(index, amount)
            index += (amount + 1)
            PullIn(index, amount)
        End Sub

        ''' <summary>
        ''' Consumes the buffer in both directions from specified character index.
        ''' </summary>
        ''' <param name="index">Index at which consuming begins.</param>
        ''' <param name="amount">Amount of contraction, in both directions, so the total contraction will be amount * 1.</param>
        ''' <param name="removePressure">Specify whether to notify the garbage collector.</param>
        ''' <remarks></remarks>
        Public Overridable Sub ConsumeChar(index As Long, amount As Long, Optional removePressure As Boolean = False)
            Dim hl As Long = Length()
            If hl <= 0 OrElse amount > index OrElse index >= (CLng(hl >> 1) - (amount + 1)) Then
                Throw New IndexOutOfRangeException("Index out of bounds DataTools.Memory.Heap:Consume.")
                Return
            End If

            index -= (amount + 1) << 1
            PullIn(index, amount)
            index += (amount + 1) << 1
            PullIn(index, amount)
        End Sub

#End Region '' Editing

        '' Math for pointers is not intended to be used with memory pointers that you have
        '' allocated with this structure, yourself. The math operators are intended to be
        '' used with a pointer that is being casually referenced.
        ''
        '' If you want to do math with the pointer value (increment or iterate, for example)
        '' then you will need to make a copy of the old pointer (in order to free it, later)
        '' as this is the only variable contained in this structure
        '' (so as to keep it suitable for substiting IntPtr
        '' in structures passed to p/Invoke).
#Region "Basic Math For Pointer Types"

        Public Shared Operator Mod(operand1 As NativeInt, operand2 As Blob) As NativeInt
            Return operand1 Mod operand2.handle
        End Operator

        Public Shared Operator +(operand1 As NativeInt, operand2 As Blob) As NativeInt
            Return operand1 + operand2.handle
        End Operator

        Public Shared Operator -(operand1 As NativeInt, operand2 As Blob) As NativeInt
            Return operand1 - operand2.handle
        End Operator

        Public Shared Operator *(operand1 As NativeInt, operand2 As Blob) As NativeInt
            Return CLng(operand1) * CLng(operand2.handle)
        End Operator

        Public Shared Operator /(operand1 As NativeInt, operand2 As Blob) As NativeInt
            Return operand1 / operand2.handle
        End Operator

        Public Shared Operator \(operand1 As NativeInt, operand2 As Blob) As NativeInt
            Return operand1 \ operand2.handle
        End Operator

#End Region '' Basic Math For Pointers

#Region "CTypes for IntPtr and MemPtr"

        Public Shared Widening Operator CType(operand As Blob) As IntPtr
            Return operand.handle
        End Operator

        Public Shared Widening Operator CType(operand As IntPtr) As Blob
            Return New Blob(operand, MemAllocType.Heap, False)
        End Operator

        Public Shared Narrowing Operator CType(operand As MemPtr) As Blob
            Return (operand.Handle)
        End Operator

        Public Shared Narrowing Operator CType(operand As Blob) As MemPtr
            Return New MemPtr(operand.handle)
        End Operator

        Public Shared Narrowing Operator CType(operand As NativeInt) As Blob
            Return New Blob(operand, False)
        End Operator

        Public Shared Narrowing Operator CType(operand As Blob) As NativeInt
            Return operand.handle
        End Operator

#End Region '' CType IntPtr/UIntPtr

#Region "Integral CTypes"

        '' in
        Public Shared Widening Operator CType(operand As SByte) As Blob
            Dim mm As New Blob
            If mm.Alloc(1) Then
                mm.SByteAt(0) = operand
                mm.BlobType = BlobTypes.SByte
            Else
                Throw New InsufficientMemoryException
            End If

            Return mm
        End Operator

        Public Shared Widening Operator CType(operand As Byte) As Blob
            Dim mm As New Blob
            If mm.Alloc(1) Then
                mm.ByteAt(0) = operand
                mm.BlobType = BlobTypes.Byte
            Else
                Throw New InsufficientMemoryException
            End If

            Return mm
        End Operator

        Public Shared Widening Operator CType(operand As Short) As Blob
            Dim mm As New Blob
            If mm.Alloc(2) Then
                mm.ShortAt(0) = operand
                mm.BlobType = BlobTypes.Short
            Else
                Throw New InsufficientMemoryException
            End If

            Return mm
        End Operator

        Public Shared Widening Operator CType(operand As UShort) As Blob
            Dim mm As New Blob
            If mm.Alloc(2) Then
                mm.UShortAt(0) = operand
                mm.BlobType = BlobTypes.UShort
            Else
                Throw New InsufficientMemoryException
            End If

            Return mm
        End Operator

        Public Shared Widening Operator CType(operand As Integer) As Blob
            Dim mm As New Blob
            If mm.Alloc(4) Then
                mm.IntegerAt(0) = operand
                mm.BlobType = BlobTypes.Integer
            Else
                Throw New InsufficientMemoryException
            End If

            Return mm
        End Operator

        Public Shared Widening Operator CType(operand As UInteger) As Blob
            Dim mm As New Blob
            If mm.Alloc(4) Then
                mm.UIntegerAt(0) = operand
                mm.BlobType = BlobTypes.UInteger
            Else
                Throw New InsufficientMemoryException
            End If

            Return mm
        End Operator

        Public Shared Widening Operator CType(operand As Long) As Blob
            Dim mm As New Blob
            If mm.Alloc(8) Then
                mm.LongAt(0) = operand
                mm.BlobType = BlobTypes.Long
            Else
                Throw New InsufficientMemoryException
            End If

            Return mm
        End Operator

        Public Shared Widening Operator CType(operand As ULong) As Blob
            Dim mm As New Blob
            If mm.Alloc(8) Then
                mm.ULongAt(0) = operand
                mm.BlobType = BlobTypes.ULong
            Else
                Throw New InsufficientMemoryException
            End If

            Return mm
        End Operator

        Public Shared Widening Operator CType(operand As Single) As Blob
            Dim mm As New Blob
            If mm.Alloc(4) Then
                mm.SingleAt(0) = operand
                mm.BlobType = BlobTypes.Single
            Else
                Throw New InsufficientMemoryException
            End If

            Return mm
        End Operator

        Public Shared Widening Operator CType(operand As Double) As Blob
            Dim mm As New Blob
            If mm.Alloc(8) Then
                mm.DoubleAt(0) = operand
                mm.BlobType = BlobTypes.Double
            Else
                Throw New InsufficientMemoryException
            End If

            Return mm
        End Operator


        '' out
        Public Shared Widening Operator CType(operand As Blob) As SByte
            Return operand.SByteAt(0)
        End Operator

        Public Shared Widening Operator CType(operand As Blob) As Byte
            Return operand.ByteAt(0)
        End Operator

        Public Shared Widening Operator CType(operand As Blob) As Char
            Return operand.CharAt(0)
        End Operator

        Public Shared Widening Operator CType(operand As Blob) As Short
            Return operand.ShortAt(0)
        End Operator

        Public Shared Widening Operator CType(operand As Blob) As UShort
            Return operand.UShortAt(0)
        End Operator

        Public Shared Widening Operator CType(operand As Blob) As Integer
            Return operand.IntegerAt(0)
        End Operator

        Public Shared Widening Operator CType(operand As Blob) As UInteger
            Return operand.UIntegerAt(0)
        End Operator

        Public Shared Widening Operator CType(operand As Blob) As Long
            Return operand.LongAt(0)
        End Operator

        Public Shared Widening Operator CType(operand As Blob) As ULong
            Return operand.ULongAt(0)
        End Operator

        Public Shared Widening Operator CType(operand As Blob) As Single
            Return operand.SingleAt(0)
        End Operator

        Public Shared Widening Operator CType(operand As Blob) As Double
            Return operand.DoubleAt(0)
        End Operator

#End Region '' Integral CTypes

#Region "Integral Array Getter and Setter Functions"

        <MethodImpl(MethodImplOptions.ForwardRef)>
        Public Function GetByteArray(byteIndex As IntPtr, length As Integer) As Byte()
            Return Nothing
        End Function

        <MethodImpl(MethodImplOptions.ForwardRef)>
        Public Sub SetByteArray(byteIndex As IntPtr, values() As Byte)
        End Sub

        <MethodImpl(MethodImplOptions.ForwardRef)>
        Public Function GetSByteArray(byteIndex As IntPtr, length As Integer) As SByte()
            Return Nothing
        End Function

        <MethodImpl(MethodImplOptions.ForwardRef)>
        Public Sub SetSByteArray(byteIndex As IntPtr, values() As SByte)
        End Sub

        <MethodImpl(MethodImplOptions.ForwardRef)>
        Public Function GetCharArray(byteIndex As IntPtr, length As Integer) As Char()
            Return Nothing
        End Function

        <MethodImpl(MethodImplOptions.ForwardRef)>
        Public Sub SetCharArray(byteIndex As IntPtr, values() As Char)
        End Sub

        <MethodImpl(MethodImplOptions.ForwardRef)>
        Public Function GetUShortArray(byteIndex As IntPtr, length As Integer) As UShort()
            Return Nothing
        End Function

        <MethodImpl(MethodImplOptions.ForwardRef)>
        Public Sub SetUShortArray(byteIndex As IntPtr, values() As UShort)
        End Sub

        <MethodImpl(MethodImplOptions.ForwardRef)>
        Public Function GetShortArray(byteIndex As IntPtr, length As Integer) As Short()
            Return Nothing
        End Function

        <MethodImpl(MethodImplOptions.ForwardRef)>
        Public Sub SetShortArray(byteIndex As IntPtr, values() As Short)
        End Sub

        <MethodImpl(MethodImplOptions.ForwardRef)>
        Public Function GetUIntegerArray(byteIndex As IntPtr, length As Integer) As UInteger()
            Return Nothing
        End Function

        <MethodImpl(MethodImplOptions.ForwardRef)>
        Public Sub SetUIntegerArray(byteIndex As IntPtr, values() As UInteger)
        End Sub

        <MethodImpl(MethodImplOptions.ForwardRef)>
        Public Function GetIntegerArray(byteIndex As IntPtr, length As Integer) As Integer()
            Return Nothing
        End Function

        <MethodImpl(MethodImplOptions.ForwardRef)>
        Public Sub SetIntegerArray(byteIndex As IntPtr, values() As Integer)
        End Sub

        <MethodImpl(MethodImplOptions.ForwardRef)>
        Public Function GetULongArray(byteIndex As IntPtr, length As Integer) As ULong()
            Return Nothing
        End Function

        <MethodImpl(MethodImplOptions.ForwardRef)>
        Public Sub SetULongArray(byteIndex As IntPtr, values() As ULong)
        End Sub

        <MethodImpl(MethodImplOptions.ForwardRef)>
        Public Function GetLongArray(byteIndex As IntPtr, length As Integer) As Long()
            Return Nothing
        End Function

        <MethodImpl(MethodImplOptions.ForwardRef)>
        Public Sub SetLongArray(byteIndex As IntPtr, values() As Long)
        End Sub

        <MethodImpl(MethodImplOptions.ForwardRef)>
        Public Function GetSingleArray(byteIndex As IntPtr, length As Integer) As Single()
            Return Nothing
        End Function

        <MethodImpl(MethodImplOptions.ForwardRef)>
        Public Sub SetSingleArray(byteIndex As IntPtr, values() As Single)
        End Sub

        <MethodImpl(MethodImplOptions.ForwardRef)>
        Public Function GetDoubleArray(byteIndex As IntPtr, length As Integer) As Double()
            Return Nothing
        End Function

        <MethodImpl(MethodImplOptions.ForwardRef)>
        Public Sub SetDoubleArray(byteIndex As IntPtr, values() As Double)
        End Sub

        <MethodImpl(MethodImplOptions.ForwardRef)>
        Public Function GetDecimalArray(byteIndex As IntPtr, length As Integer) As Decimal()
            Return Nothing
        End Function

        <MethodImpl(MethodImplOptions.ForwardRef)>
        Public Sub SetDecimalArray(byteIndex As IntPtr, values() As Decimal)
        End Sub

        <MethodImpl(MethodImplOptions.ForwardRef)>
        Public Function GetGuidArray(byteIndex As IntPtr, length As Integer) As Guid()
            Return Nothing
        End Function

        <MethodImpl(MethodImplOptions.ForwardRef)>
        Public Sub SetGuidArray(byteIndex As IntPtr, values() As Guid)
        End Sub

#End Region '' Integral Array Getter and Setter Functions

#Region "Integral Array CTypes"

        '' In

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As Byte()) As Blob
            Return Nothing
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As SByte()) As Blob
            Return Nothing
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As Short()) As Blob
            Return Nothing
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As UShort()) As Blob
            Return Nothing
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As Integer()) As Blob
            Return Nothing
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As UInteger()) As Blob
            Return Nothing
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As Long()) As Blob
            Return Nothing
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As ULong()) As Blob
            Return Nothing
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As Single()) As Blob
            Return Nothing
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As Double()) As Blob
            Return Nothing
        End Operator

        '' Out
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As Blob) As SByte()
            Return Nothing
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As Blob) As Byte()
            Return Nothing
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As Blob) As Short()
            Return Nothing
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As Blob) As UShort()
            Return Nothing
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As Blob) As Integer()
            Return Nothing
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As Blob) As UInteger()
            Return Nothing
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As Blob) As Long()
            Return Nothing
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As Blob) As ULong()
            Return Nothing
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As Blob) As Single()
            Return Nothing
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As Blob) As Double()
            Return Nothing
        End Operator

#End Region '' Integral Array CTypes

#Region "String and Char CTypes"

        '' Returns a pretty string, based on null-termination logic, instead of
        '' returning every character in the allocated block.
        Public Shared Widening Operator CType(operand As Blob) As String
            Return operand.GrabString(0)
        End Operator

        '' We add 2 bytes to give us a proper null-terminated string in memory.
        Public Shared Widening Operator CType(operand As String) As Blob
            Dim mm As New Blob
            mm.SetString(0, operand)
            Return mm

            'Dim i As Integer = operand.Length << 1
            'Dim mm As New Blob(i + 2)
            'QuickCopyObject(Of String)(mm.Handle, operand, CUInt(i))
            'Return mm
        End Operator

        '' Here we return every character in the allocated block.
        <MethodImpl(MethodImplOptions.ForwardRef)>
        Public Shared Widening Operator CType(operand As Blob) As Char()
            Return Nothing
        End Operator

        '' We just set the character information into the memory buffer, verbatim.
        <MethodImpl(MethodImplOptions.ForwardRef)>
        Public Shared Widening Operator CType(operand As Char()) As Blob
            Return Nothing
        End Operator

        Public Shared Widening Operator CType(operand As String()) As Blob
            If operand Is Nothing OrElse operand.Length = 0 Then Return Nothing

            Dim mm As New Blob

            Dim x As Long
            For Each s In operand
                x += ((s.Length << 1) + 2)
            Next

            mm.Length = x

            x = 0

            For Each s In operand
                mm.SetString(x, s)
                x += ((s.Length << 1) + 2)
            Next

            Return mm
        End Operator

        Public Shared Widening Operator CType(operand As Blob) As String()
            Return operand.GrabStringArray(0)
        End Operator

#End Region '' String and Char CTypes

#Region "Guid Scalar and Array CTypes"

        Public Shared Narrowing Operator CType(operand As Guid) As Blob
            Dim mm As New Blob
            If mm.Alloc(16) Then
                mm.GuidAt(0) = operand
                mm.BlobType = BlobTypes.Guid
            Else
                Throw New OutOfMemoryException
            End If
            Return mm
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Narrowing Operator CType(operand As Guid()) As Blob
            Return Nothing
        End Operator

        Public Shared Narrowing Operator CType(operand As Blob) As Guid
            Return operand.GuidAt(0)
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Narrowing Operator CType(operand As Blob) As Guid()
            Return Nothing
        End Operator

#End Region '' Guid Scalar and Array CTypes

#Region "Color, Decimal, Date"

        Public Shared Widening Operator CType(operand As Decimal) As Blob
            Dim mm As New Blob
            If mm.Alloc(16) Then
                mm.DecimalAt(0) = operand
                mm.BlobType = BlobTypes.Decimal
            Else
                Throw New OutOfMemoryException
            End If
            Return mm
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As Decimal()) As Blob
            Return Nothing
        End Operator

        Public Shared Widening Operator CType(operand As Blob) As Decimal
            Return operand.DecimalAt(0)
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As Blob) As Decimal()
            Return Nothing
        End Operator

        '' Color

        Public Shared Widening Operator CType(operand As Color) As Blob
            Dim mm As New Blob
            If mm.Alloc(4) Then
                mm.IntegerAt(0) = operand.ToArgb
                mm.BlobType = BlobTypes.Color
            Else
                Throw New OutOfMemoryException
            End If
            Return mm
        End Operator

        Public Shared Widening Operator CType(operand As Color()) As Blob
            Dim mm As New Blob
            If mm.Alloc(operand.Length << 2) Then
                QuickCopyObject(operand, mm, operand.Length << 2)
                mm.BlobType = BlobTypes.Color
            Else
                Throw New OutOfMemoryException
            End If
            Return mm
        End Operator

        Public Shared Widening Operator CType(operand As Blob) As Color
            Return Color.FromArgb(operand.IntegerAt(0))
        End Operator

        Public Shared Widening Operator CType(operand As Blob) As Color()

            Dim clr() As Color

            Dim l As Long = operand.Length >> 2
            If l > UInt32.MaxValue Then Throw New InvalidCastException

            ReDim clr(l - 1)

            QuickCopyObject(clr, operand, l << 2)
            Return clr
        End Operator

        '' DateTime

        Public Shared Widening Operator CType(operand As Date) As Blob
            Dim mm As New Blob
            If mm.Alloc(8) Then
                mm.LongAt(0) = operand.ToBinary()
                mm.BlobType = BlobTypes.Date
            Else
                Throw New OutOfMemoryException
            End If
            Return mm
        End Operator

        Public Shared Widening Operator CType(operand As Date()) As Blob
            Dim mm As New Blob
            If mm.Alloc(operand.Length << 3) Then
                MemCpy(mm.handle, operand, operand.Length << 3)
                mm.BlobType = BlobTypes.Date
            Else
                Throw New OutOfMemoryException
            End If
            Return mm
        End Operator

        Public Shared Widening Operator CType(operand As Blob) As Date
            Return Date.FromBinary(operand.LongAt(0))
        End Operator

        Public Shared Widening Operator CType(operand As Blob) As Date()
            Dim dNew() As Date
            Dim i As Integer = operand.Length >> 3

            ReDim dNew(i - 1)
            MemCpy(dNew, operand.handle, dNew.Length << 3)
            Return dNew
        End Operator

#End Region '' Color, Decimal, and Date CTypes

    End Class

#End Region

End Namespace