'' ************************************************* ''
'' DataTools Visual Basic Utility Library 
''
'' Module: Blob
''         Global Utility Methods and Enums
''         
'' Copyright (C) 2011-2020 Nathan Moschkin
'' All Rights Reserved
''
'' Licensed Under the Microsoft Public License   
'' ************************************************* ''

Imports System.ComponentModel
Imports DataTools.Memory.Internal

Namespace Memory

#Region "Bytes and Enums"

    Module BlobUtil

        Public Function BytesToEnum(b() As Byte, t As System.Type) As Object
            Dim sp As SafePtr = b

            Dim x As Integer = Blob.BlobTypeSize(Blob.TypeToBlobType(t.GetEnumUnderlyingType))

            Select Case x

                Case 1
                    If Unsigned(t) Then
                        Return [Enum].ToObject(t, sp.ByteAt(0))
                    Else
                        Return [Enum].ToObject(t, sp.SByteAt(0))
                    End If

                Case 2
                    If Unsigned(t) Then
                        Return [Enum].ToObject(t, sp.UShortAt(0))
                    Else
                        Return [Enum].ToObject(t, sp.ShortAt(0))
                    End If

                Case 4
                    If Unsigned(t) Then
                        Return [Enum].ToObject(t, sp.UIntegerAt(0))
                    Else
                        Return [Enum].ToObject(t, sp.IntegerAt(0))
                    End If

                Case 8
                    If Unsigned(t) Then
                        Return [Enum].ToObject(t, sp.ULongAt(0))
                    Else
                        Return [Enum].ToObject(t, sp.LongAt(0))
                    End If

            End Select

            Return Nothing

        End Function

        Public Function EnumToBytes(val As Object) As Byte()

            Dim sp As SafePtr = Nothing
            Dim t As System.Type = val.GetType

            Dim x As Integer = Blob.BlobTypeSize(Blob.TypeToBlobType(t.GetEnumUnderlyingType))

            Select Case x

                Case 1
                    If Unsigned(t) Then
                        sp = CType(val, Byte)
                    Else
                        sp = CType(val, SByte)
                    End If

                Case 2
                    If Unsigned(t) Then
                        sp = CType(val, UShort)
                    Else
                        sp = CType(val, Short)
                    End If

                Case 4
                    If Unsigned(t) Then
                        sp = CType(val, UInteger)
                    Else
                        sp = CType(val, Integer)
                    End If

                Case 8
                    If Unsigned(t) Then
                        sp = CType(val, ULong)
                    Else
                        sp = CType(val, Long)
                    End If

            End Select

            Return sp

        End Function

        'Friend BlobMasterHeap As BlobHeap

        Sub New()
            'BlobMasterHeap = New BlobHeap(SystemInfo.SystemInfo.dwPageSize * 76800)
        End Sub

    End Module

#End Region

#Region "Blob-Related Enums"

    ''' <summary>
    ''' Specifies the type of memory allocated for the given blob.
    ''' </summary>
    ''' <remarks></remarks>
    <Flags>
    Public Enum MemAllocType

        ''' <summary>
        ''' Invalid memory.
        ''' </summary>
        ''' <remarks></remarks>
        Invalid = 0

        ''' <summary>
        ''' Normal heap memory.
        ''' </summary>
        ''' <remarks></remarks>
        Heap = 1

        ''' <summary>
        ''' NetApi memory.
        ''' </summary>
        ''' <remarks></remarks>
        Network = 2

        ''' <summary>
        ''' Virtual memory.
        ''' </summary>
        ''' <remarks></remarks>
        Virtual = 4

        ''' <summary>
        ''' COM memory.
        ''' </summary>
        ''' <remarks></remarks>
        Com = 8

        ''' <summary>
        ''' The memory is aligned.
        ''' </summary>
        ''' <remarks></remarks>
        Aligned = 32

        ''' <summary>
        ''' Other/Unknown (or unowned) memory.
        ''' </summary>
        ''' <remarks></remarks>
        Other = 64

    End Enum

    ''' <summary>
    ''' Specifies the blob parsing method to use for string content.
    ''' </summary>
    ''' <remarks></remarks>
    <DefaultValue(0)>
    Public Enum BlobParsingMethod

        ''' <summary>
        ''' Unicode UTF-16 (default)
        ''' </summary>
        ''' <remarks></remarks>
        Unicode = 0

        ''' <summary>
        ''' ASCII or UTF-8.
        ''' </summary>
        ''' <remarks></remarks>
        Ascii = 1

        ''' <summary>
        ''' Use the byte-order-mark to determine.
        ''' </summary>
        ''' <remarks></remarks>
        BOM = 2

    End Enum

    ''' <summary>
    ''' Indicates the type of content within the blob (loosely represented).
    ''' </summary>
    ''' <remarks></remarks>
    <DefaultValue(-1)>
    Public Enum BlobTypes As Integer

        ''' <summary>
        ''' Content is invalid.
        ''' </summary>
        ''' <remarks></remarks>
        [Invalid] = &HFFFFFF20I

        ''' <summary>
        ''' SByte
        ''' </summary>
        ''' <remarks></remarks>
        [SByte] = 0

        ''' <summary>
        ''' Byte
        ''' </summary>
        ''' <remarks></remarks>
        [Byte] = 1

        ''' <summary>
        ''' Short
        ''' </summary>
        ''' <remarks></remarks>
        [Short] = 2

        ''' <summary>
        ''' UShort
        ''' </summary>
        ''' <remarks></remarks>
        [UShort] = 3

        ''' <summary>
        ''' Integer
        ''' </summary>
        ''' <remarks></remarks>
        [Integer] = 4

        ''' <summary>
        ''' UInteger
        ''' </summary>
        ''' <remarks></remarks>
        [UInteger] = 5

        ''' <summary>
        ''' Long
        ''' </summary>
        ''' <remarks></remarks>
        [Long] = 6

        ''' <summary>
        ''' ULong
        ''' </summary>
        ''' <remarks></remarks>
        [ULong] = 7

        ''' <summary>
        ''' System.Numerics.BigInteger
        ''' </summary>
        ''' <remarks></remarks>
        [BigInteger] = 8

        ''' <summary>
        ''' Single-precision floating-point number.
        ''' </summary>
        ''' <remarks></remarks>
        [Single] = 9

        ''' <summary>
        ''' Double-precision floating-point number.
        ''' </summary>
        ''' <remarks></remarks>
        [Double] = &HA

        ''' <summary>
        ''' Quadruple-precision floating-point number.
        ''' </summary>
        ''' <remarks></remarks>
        [Decimal] = &HB

        ''' <summary>
        ''' DateTime object.
        ''' </summary>
        ''' <remarks></remarks>
        [Date] = &HC

        ''' <summary>
        ''' Char
        ''' </summary>
        ''' <remarks></remarks>
        [Char] = &HD

        ''' <summary>
        ''' String
        ''' </summary>
        ''' <remarks></remarks>
        [String] = &HE

        ''' <summary>
        ''' System.Guid
        ''' </summary>
        ''' <remarks></remarks>
        [Guid] = &HF

        ''' <summary>
        ''' PNG Image
        ''' </summary>
        ''' <remarks></remarks>
        [Image] = &H10

        ''' <summary>
        ''' Color
        ''' </summary>
        ''' <remarks></remarks>
        [Color] = &H11

        ''' <summary>
        ''' Worm Record
        ''' </summary>
        ''' <remarks></remarks>
        [WormRecord] = &H12

        ''' <summary>
        ''' Null-terminated string(s)
        ''' </summary>
        ''' <remarks></remarks>
        [NtString] = &H13

        ''' <summary>
        ''' Boolean
        ''' </summary>
        ''' <remarks></remarks>
        [Boolean] = &H14

        '' For each of the following in serialization, the
        '' stored data shall be: array-type moniker, type moniker, number of elements, data.
        '' and where data is a length of parcel moniker at a 32 bit integer (and no more... for now), and then the data.

        '' some of these may require the calling program to fulfill the requirement
        '' of creating an instance of the required object via the InstanceHelper event.

        ''' <summary>
        ''' Denotes the base type to be an array.
        ''' </summary>
        [Array] = &H15

        ''' <summary>
        ''' Denotes the base type to be an array.
        ''' </summary>
        [List] = &H16

        ''' <summary>
        ''' Denotes the base type to be an array.
        ''' </summary>
        [ListOf] = &H17

        ''' <summary>
        ''' Denotes the base type to be an array.
        ''' </summary>
        [Collection] = &H18

        ''' <summary>
        ''' Denotes the base type to be an array.
        ''' </summary>
        [CollectionOf] = &H19

        ''' <summary>
        ''' Denotes the base type to be an array.
        ''' </summary>
        [ObservableCollection] = &H1A

        ''' <summary>
        ''' Denotes the base type to be an array.
        ''' </summary>
        [ObservableCollectionOf] = &H1B

        ''' <summary>
        ''' Indicates numeric array, generally only used by SerializerAdapter.
        ''' </summary>
        [NumericArray] = &H40000000

        ''' <summary>
        ''' Generic byte[] based blob of data, generally only used by SerializerAdapter.
        ''' </summary>
        [Blob] = &H2000001C

    End Enum

    ''' <summary>
    ''' Indicates a pure ordinal integer blob type.
    ''' </summary>
    ''' <remarks></remarks>
    <DefaultValue(&HFFFFFFF8)>
    Public Enum BlobOrdinalTypes As Integer
        ''' <summary>
        ''' Invalid value
        ''' </summary>
        ''' <remarks></remarks>
        [Invalid] = &HFFFFFFF8

        ''' <summary>
        ''' SByte
        ''' </summary>
        ''' <remarks></remarks>
        [SByte] = 0

        ''' <summary>
        ''' Byte
        ''' </summary>
        ''' <remarks></remarks>
        [Byte] = 1

        ''' <summary>
        ''' Short
        ''' </summary>
        ''' <remarks></remarks>
        [Short] = 2

        ''' <summary>
        ''' UShort
        ''' </summary>
        ''' <remarks></remarks>
        [UShort] = 3

        ''' <summary>
        ''' Integer
        ''' </summary>
        ''' <remarks></remarks>
        [Integer] = 4

        ''' <summary>
        ''' UInteger
        ''' </summary>
        ''' <remarks></remarks>
        [UInteger] = 5

        ''' <summary>
        ''' Long
        ''' </summary>
        ''' <remarks></remarks>
        [Long] = 6

        ''' <summary>
        ''' ULong
        ''' </summary>
        ''' <remarks></remarks>
        [ULong] = 7

    End Enum

    <DefaultValue(-1)>
    Friend Enum SystemBlobTypes As Integer

        ''' <summary>
        ''' Content is invalid.
        ''' </summary>
        ''' <remarks></remarks>
        [Invalid] = &HFFFFFF20I

        ''' <summary>
        ''' SByte
        ''' </summary>
        ''' <remarks></remarks>
        [SByte] = 0
        Int8 = 0

        ''' <summary>
        ''' Byte
        ''' </summary>
        ''' <remarks></remarks>
        [Byte] = 1
        UInt8 = 1

        ''' <summary>
        ''' Short
        ''' </summary>
        ''' <remarks></remarks>
        [Short] = 2
        Int16 = 2

        ''' <summary>
        ''' UShort
        ''' </summary>
        ''' <remarks></remarks>
        [UShort] = 3
        UInt16 = 3

        ''' <summary>
        ''' Integer
        ''' </summary>
        ''' <remarks></remarks>
        [Integer] = 4
        Int32 = 4

        ''' <summary>
        ''' UInteger
        ''' </summary>
        ''' <remarks></remarks>
        [UInteger] = 5
        UInt32 = 5

        ''' <summary>
        ''' Long
        ''' </summary>
        ''' <remarks></remarks>
        [Long] = 6
        Int64 = 6

        ''' <summary>
        ''' ULong
        ''' </summary>
        ''' <remarks></remarks>
        [ULong] = 7
        UInt64 = 7

        ''' <summary>
        ''' Single-precision floating-point number.
        ''' </summary>
        ''' <remarks></remarks>
        [Single] = 9
        Float = 9

        ''' <summary>
        ''' Double-precision floating-point number.
        ''' </summary>
        ''' <remarks></remarks>
        [Double] = &HA

        ''' <summary>
        ''' Quadruple-precision floating-point number.
        ''' </summary>
        ''' <remarks></remarks>
        [Decimal] = &HB

        ''' <summary>
        ''' DateTime object.
        ''' </summary>
        ''' <remarks></remarks>
        [Date] = &HC

        ''' <summary>
        ''' Char
        ''' </summary>
        ''' <remarks></remarks>
        [Char] = &HD

        ''' <summary>
        ''' String
        ''' </summary>
        ''' <remarks></remarks>
        [String] = &HE

        ''' <summary>
        ''' System.Guid
        ''' </summary>
        ''' <remarks></remarks>
        [Guid] = &HF

        ''' <summary>
        ''' PNG Image
        ''' </summary>
        ''' <remarks></remarks>
        [Image] = &H10

        ''' <summary>
        ''' Color
        ''' </summary>
        ''' <remarks></remarks>
        [Color] = &H11

        ''' <summary>
        ''' Boolean
        ''' </summary>
        ''' <remarks></remarks>
        [Boolean] = &H14

    End Enum

    ''' <summary>
    ''' Blob constants
    ''' </summary>
    ''' <remarks></remarks>
    Friend Enum BlobConst
        ''' <summary>
        ''' Maximum blob-type integer value for mathematical operations.
        ''' </summary>
        ''' <remarks></remarks>
        [MaxMath] = 11

        ''' <summary>
        ''' Maximum blob-type integer value for bitwi
        ''' </summary>
        ''' <remarks></remarks>
        [MaxBit] = 7
        [UBound] = 19
    End Enum

#End Region


End Namespace