'' ************************************************* ''
'' DataTools Visual Basic Utility Library 
''
'' Module: MemPtr Structure
''         Exhaustive in-place replacement
''         for IntPtr.
'' 
'' Copyright (C) 2011-2020 Nathan Moschkin
'' All Rights Reserved
''
'' Licensed Under the Microsoft Public License   
'' ************************************************* ''

Option Explicit On
Option Compare Binary

Imports DataTools.BitStream
Imports System.Runtime.InteropServices
Imports System.Drawing
Imports DataTools.Memory.Internal
Imports System.Runtime.CompilerServices

Namespace Memory

    '' This structure is intended to make it more convenient to marshal 
    '' with the operating system.

    '' There is no mechanism in .NET for the 
    '' managed disposal of structures.  This
    '' structure is intended to be used responsibly as a 
    '' quick replacement for IntPtr that features 
    '' some utility. 

    <HideModuleName>
    Module MemPtrStrings

        Public Const MemTooBig As String = "Memory pointer is too big."

    End Module

    ''' <summary>
    ''' The MemPtr structure.  Drop-in replacement for IntPtr.
    ''' Use anywhere you use IntPtr.  Be sure to dispose of your
    ''' unmanaged resources.  No garbage collection is done on this
    ''' structure (because it is a structure), so all freeing of memory 
    ''' must be handled, in code.
    ''' </summary>
    ''' <remarks></remarks>
    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)> _
    Public Structure MemPtr
        Implements IDisposable,  _
                   ICloneable,  _
                   IEnumerable(Of Byte),  _
                   IEnumerable(Of Char),  _
                   IEquatable(Of MemPtr),  _
                   IEquatable(Of IntPtr),  _
                   IEquatable(Of UIntPtr)


        ''' <summary>
        ''' The internal pointer value of the structure.
        ''' </summary>
        ''' <remarks></remarks>
        Friend _ptr As IntPtr

#Region "Shared"

        '' A List Key/Value pairs of Integral types and their atomic sizes (in bytes).
        Private Shared _primitiveCache As New List(Of KeyValuePair(Of System.Type, Integer))

        Private Shared _procHeap As IntPtr = GetProcessHeap

        ''' <summary>
        ''' Returns a null pointer.
        ''' </summary>
        Public Shared ReadOnly Empty As MemPtr = New MemPtr(0)

        ''' <summary>
        ''' Returns INVALID_HANDLE_VALUE (-1)
        ''' </summary>
        Public Shared ReadOnly InvalidHandle As MemPtr = New MemPtr(New IntPtr(-1L))

#End Region

#Region "Copying"

        ''' <summary>
        ''' Copies memory from this memory pointer into the pointer specified.
        ''' </summary>
        ''' <param name="ptr">The pointer to which to copy the memory.</param>
        ''' <param name="size">The size of the buffer to copy.</param>
        ''' <remarks></remarks>
        Public Sub CopyTo(ptr As IntPtr, size As IntPtr)
            If size.ToInt64 <= UInt32.MaxValue Then
                MemCpy(ptr, _ptr, CUInt(size))
            Else
                n_memcpy(ptr, _ptr, CType(size.ToInt64, UIntPtr))
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
            If _ptr <> 0 OrElse Alloc(size.ToInt64) Then
                If size.ToInt64 <= UInt32.MaxValue Then
                    MemCpy(_ptr, ptr, CUInt(size))
                Else
                    n_memcpy(ptr, _ptr, CType(size.ToInt64, UIntPtr))
                End If
            End If
        End Sub

#End Region '' Copying

#Region "Pinned Objects"

        ''' <summary>
        ''' Pin an object, associate the MemPtr with the address of the pinned
        ''' object and return the GCHandle. 
        ''' </summary>
        ''' <param name="obj">Object to pin and associate.</param>
        ''' <returns>A GCHandle to the pinned object.</returns>
        ''' <remarks>MemPtr must be empty for this function to succeed.</remarks>
        Public Function Pin(obj As Object) As GCHandle
            If _ptr <> 0 Then Return Nothing

            Dim g As GCHandle = GCHandle.Alloc(obj, GCHandleType.Pinned)
            _ptr = g.AddrOfPinnedObject
            Return g
        End Function

        ''' <summary>
        ''' Frees the GCHandle and disassociates the internal pointer from the address of the pinned object.
        ''' </summary>
        ''' <param name="gc">GCHandle of the object to be disassociated and freed.</param>
        ''' <remarks></remarks>
        Public Sub Free(gc As GCHandle)
            If _ptr <> gc.AddrOfPinnedObject Then Return
            gc.Free()
            _ptr = 0
        End Sub

#End Region '' Pinned Objects

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
            StrLen = CharZero(_ptr + byteIndex)
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
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function GrabString(byteIndex As IntPtr, length As Integer) As String
            Return Nothing
            'If _ptr = 0 Then Return Nothing

            'If length <= 0 Then Throw New IndexOutOfRangeException("length must be greater than zero")

            'GrabString = New String(ChrW(0), length)
            'QuickCopyObject(Of String)(GrabString, New IntPtr(CLng(_ptr) + byteIndex.ToInt64), CType(length << 1, UInteger))
        End Function

        ''' <summary>
        ''' Grabs a null-terminated ASCII string from a position relative to the memory pointer.
        ''' </summary>
        ''' <param name="byteIndex">A 32 or 64-bit number indicating the starting position relative to the pointer.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GrabAsciiString(byteIndex As IntPtr) As String

            Dim tp As New IntPtr(CLng(_ptr) + byteIndex.ToInt64)
            Dim e As Integer = ByteZero(tp)
            Dim ba() As Byte

            If e = 0 Then Return ""
            ReDim ba(e - 1)

            byteArrAtget(ba, New IntPtr(CLng(_ptr) + CLng(byteIndex)), CUInt(e))
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

            If _ptr = 0 Then Return Nothing
            If length <= 0 Then Throw New IndexOutOfRangeException("length must be greater than zero")

            Dim ba() As Byte
            ReDim ba(length - 1)

            byteArrAtget(ba, New IntPtr(CLng(_ptr) + CLng(byteIndex)), CUInt(length))
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
            If _ptr = 0 Then Return Nothing

            Dim b As Char
            Dim i As Integer = 0
            Dim sb As Long = CLng(byteIndex)

            Dim sout() As String = Nothing
            Dim ct As Integer = 0
            Dim l As Long = CLng(byteIndex)
            Dim tp As IntPtr = New IntPtr(l + CLng(_ptr))

            b = CharAtAbsolute(l)

            While b <> ChrW(0)
                i = CharZero(tp)
                ReDim Preserve sout(ct)
                sout(ct) = New String(ChrW(0), i)
                QuickCopyObject(Of String)(sout(ct), tp, CUInt(i << 1))

                l += CInt((i << 1) + 2)
                tp += CInt((i << 1) + 2)

                b = CharAtAbsolute(l)

                ct += 1
            End While

            Return sout
        End Function

        ''' <summary>
        ''' Sets the memory at the specified byte index to the specified string using the optional specified encoding.
        ''' A null termination character is appended to the string before the encoding conversion.
        ''' </summary>
        ''' <param name="byteIndex"></param>
        ''' <param name="s"></param>
        ''' <param name="enc">Optional System.Text.Encoding object (default is Windows Unicode = UTF16LE / wchar_t).</param>
        ''' <remarks></remarks>
        Public Sub SetString(byteIndex As IntPtr, s As String, enc As System.Text.Encoding)
            If enc Is Nothing Then enc = System.Text.Encoding.Unicode

            Dim p As New IntPtr(CLng(_ptr) + byteIndex.ToInt64)
            Dim b() As Byte = enc.GetBytes(s & ChrW(0))

            byteArrAtset(p, b, CUInt(b.Length))
        End Sub

        ''' <summary>
        ''' Sets the memory at the specified byte index to the specified string.
        ''' A null termination character is appended to the string.
        ''' </summary>
        ''' <param name="byteIndex"></param>
        ''' <param name="s"></param>
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
        Public Sub SetBytes(byteIndex As IntPtr, data() As Byte)
        End Sub

        ''' <summary>
        ''' Get an array of bytes at the specified position of the specified length.
        ''' </summary>
        ''' <param name="byteIndex">A 32 or 64-bit number indicating the starting position relative to the pointer.</param>
        ''' <param name="length">The number of bytes to grab.</param>
        ''' <returns>A new byte array with the requested data.</returns>
        ''' <remarks></remarks>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function GrabBytes(byteIndex As IntPtr, length As Integer) As Byte()
            Return Nothing
        End Function


        ''' <summary>
        ''' Sets the memory at the specified index to the specified sbyte array.
        ''' </summary>
        ''' <param name="byteIndex"></param>
        ''' <param name="data"></param>
        ''' <remarks></remarks>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Sub SetSBytes(byteIndex As IntPtr, data() As Byte)
        End Sub

        ''' <summary>
        ''' Get an array of sbytes at the specified position of the specified length.
        ''' </summary>
        ''' <param name="byteIndex">A 32 or 64-bit number indicating the starting position relative to the pointer.</param>
        ''' <param name="length">The number of bytes to grab.</param>
        ''' <returns>A new byte array with the requested data.</returns>
        ''' <remarks></remarks>
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function GrabSBytes(byteIndex As IntPtr, length As Integer) As Byte()
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
        Public Sub GrabBytes(byteIndex As IntPtr, length As Integer, ByRef data() As Byte)
            If _ptr = 0 Then Return

            If data Is Nothing Then
                ReDim data(length - 1)
            ElseIf data.Length < length Then
                ReDim data(length - 1)
            End If

            byteGet(byteIndex, length, data)
        End Sub

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Private Sub byteGet(byteIndex As IntPtr, length As Integer, ByRef data() As Byte)
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
        Public Sub GrabBytes(byteIndex As IntPtr, length As Integer, ByRef data() As Byte, arrayIndex As Integer)
            If _ptr = 0 Then Return

            If data Is Nothing Then
                Throw New ArgumentNullException("data cannot be null or Nothing.")
            ElseIf length + arrayIndex > data.Length Then
                Throw New ArgumentOutOfRangeException("data buffer length is too small.")
            End If

            Dim gh As GCHandle = GCHandle.Alloc(data, GCHandleType.Pinned)
            Dim pdest As IntPtr = gh.AddrOfPinnedObject + arrayIndex

            MemCpy(pdest, New IntPtr(CLng(_ptr) + CLng(byteIndex)), CUInt(length))
            gh.Free()
        End Sub

        ''' <summary>
        ''' Returns the results of the buffer as if it were a BSTR type String.
        ''' </summary>
        ''' <param name="comPtr">Specifies whether or not the current MemPtr is an actual COM pointer to a BSTR.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function BSTR(Optional comPtr As Boolean = True) As String
            Dim d As Integer, _
                s As String

            Dim p As IntPtr = If(comPtr, _ptr - 4, _ptr)

            intAtget(d, p)
            s = New String(ChrW(0), d)
            QuickCopyObject(Of String)(s, p + 4, CUInt(d << 1))
            BSTR = s
        End Function

        ''' <summary>
        ''' Returns the contents of this buffer as a string.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function LpwStr() As String
            LpwStr = GrabString(0)
        End Function

#End Region '' String extraction functions

#Region "Get and Set Array Functions" '' Get and Set array functions.

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Sub SetIntegerArray(byteIndex As Long, values() As Integer)
        End Sub

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function GetIntegerArray(byteIndex As Long, Optional length As Integer = 0) As Integer()
            Return Nothing
        End Function

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Sub SetUIntegerArray(byteIndex As Long, values() As UInteger)

        End Sub

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Function GetUIntegerArray(byteIndex As Long, Optional length As UInteger = 0) As UInteger()
            Return Nothing
        End Function

#End Region '' Get and Set array functions

#Region "Properties and Property-Like Methods"

        ''' <summary>
        ''' Gets or sets the pointer to the memory block.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Handle() As IntPtr
            Get
                Handle = _ptr
            End Get
            Set(value As IntPtr)
                If _ptr <> 0 Then
                    Free()
                End If
                _ptr = value
            End Set
        End Property

        ''' <summary>
        ''' Gets the length, in bytes, of the memory block.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Length(Optional hHeap As IntPtr? = Nothing) As Long
            If _ptr = 0 Then Return 0
            If hHeap Is Nothing Then hHeap = _procHeap
            Try
                Return CLng(HeapSize(hHeap, 0, _ptr))
            Catch ex As Exception
                Return -1
            End Try
        End Function

        ''' <summary>
        ''' Sets the length of the memory block.
        ''' </summary>
        ''' <param name="value"></param>
        ''' <param name="hHeap">Optional private heap.</param>
        ''' <remarks></remarks>
        Public Sub SetLength(value As Long, Optional hHeap As IntPtr? = Nothing)
            If hHeap Is Nothing Then hHeap = _procHeap
            Alloc(value, , hHeap)
        End Sub

        ''' <summary>
        ''' Calculate the CRC 32 for the block of memory represented by this structure.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CalculateCrc32() As UInteger
            Dim l As New IntPtr(Me.Length)
            CalculateCrc32 = Crc32.Calculate(_ptr, l)
        End Function

        ''' <summary>
        ''' Calculate the CRC 32 for the block of memory represented by this structure.
        ''' </summary>
        ''' <param name="bufflen">The length, in bytes, of the marshaling buffer.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CalculateCrc32(bufflen As Integer) As UInteger
            Dim l As New IntPtr(Me.Length)
            CalculateCrc32 = Crc32.Calculate(_ptr, l, , bufflen)
        End Function

        ''' <summary>
        ''' Calculate the CRC 32 for the block of memory represented by this structure.
        ''' </summary>
        ''' <param name="length">The length, in bytes, of the buffer to check.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CalculateCrc32(length As IntPtr) As UInteger
            CalculateCrc32 = Crc32.Calculate(_ptr, length)
        End Function

        ''' <summary>
        ''' Calculate the CRC 32 for the block of memory represented by this structure.
        ''' </summary>
        ''' <param name="length">The length, in bytes, of the buffer to check.</param>
        ''' <param name="bufflen">The length, in bytes, of the marshaling buffer.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CalculateCrc32(length As IntPtr, bufflen As Integer) As UInteger
            CalculateCrc32 = Crc32.Calculate(_ptr, length, , bufflen)
        End Function

#End Region '' Properties and Property-Like Methods

#Region "Structure Utility Methods"

        ''' <summary>
        ''' Converts the contents of an unmanaged pointer into a structure.
        ''' </summary>
        ''' <typeparam name="T">The type of structure requested.</typeparam>
        ''' <returns>New instance of T.</returns>
        ''' <remarks></remarks>
        Public Function ToStruct(Of T As Structure)() As T
            ToStruct = CType(Marshal.PtrToStructure(_ptr, GetType(T)), T)
        End Function

        ''' <summary>
        ''' Sets the contents of a structure into an unmanaged pointer.
        ''' </summary>
        ''' <typeparam name="T">The type of structure to set.</typeparam>
        ''' <param name="val">The structure to set.</param>
        ''' <remarks></remarks>
        Public Sub FromStruct(Of T As Structure)(val As T)
            Dim cb As Integer = Marshal.SizeOf(val)
            If (_ptr = 0) Then AllocZero(cb)
            Marshal.StructureToPtr(val, _ptr, False)
        End Sub

        ''' <summary>
        ''' Converts the contents of an unmanaged pointer at the specified byte index into a structure.
        ''' </summary>
        ''' <typeparam name="T">The type of structure requested.</typeparam>
        ''' <param name="byteIndex">The byte index relative to the pointer at which to begin copying.</param>
        ''' <returns>New instance of T.</returns>
        ''' <remarks></remarks>
        Public Function ToStructAt(Of T As Structure)(byteIndex As IntPtr) As T
            ToStructAt = Marshal.PtrToStructure(_ptr + byteIndex, GetType(T))
        End Function

        ''' <summary>
        ''' Sets the contents of a structure into a memory buffer at the specified byte index.
        ''' </summary>
        ''' <typeparam name="T">The type of structure to set.</typeparam>
        ''' <param name="byteIndex">The byte index relative to the pointer at which to begin copying.</param>
        ''' <param name="val">The structure to set.</param>
        ''' <remarks></remarks>
        Public Sub FromStructAt(Of T As Structure)(byteIndex As IntPtr, val As T)
            Dim cb As Integer = Marshal.SizeOf(val)
            Marshal.StructureToPtr(val, _ptr + byteIndex, False)
        End Sub

        ''' <summary>
        ''' Copies the contents of the buffer at the specified index into a blittable structure array.
        ''' </summary>
        ''' <typeparam name="T">The structure type.</typeparam>
        ''' <param name="byteIndex">The index at which to begin copying.</param>
        ''' <returns>An array of T.</returns>
        ''' <remarks></remarks>
        Public Function ToBlittableStructArrayAt(Of T As Structure)(byteIndex As IntPtr) As T()

            If (_ptr = 0) Then Return Nothing

            Dim l As Long = Length() - CType(byteIndex, Long)
            Dim cb = Marshal.SizeOf(New T)
            Dim c As Integer = CInt(l / cb)

            Dim tt() As T
            ReDim tt(c - 1)

            Dim gh As GCHandle = GCHandle.Alloc(tt, GCHandleType.Pinned)

            If l <= UInt32.MaxValue Then
                MemCpy(gh.AddrOfPinnedObject, _ptr, CUInt(l))
            Else
                n_memcpy(gh.AddrOfPinnedObject, _ptr, l)
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
        Public Sub FromBlittableStructArrayAt(Of T As Structure)(byteIndex As IntPtr, value As T())

            If (_ptr = 0) AndAlso (byteIndex <> 0) Then Return

            Dim l As Long
            Dim cb = Marshal.SizeOf(New T)
            Dim c As Integer = value.Count

            l = c * cb

            If (_ptr = 0) Then
                If Not Alloc(l) Then Return
            End If

            Dim p = _ptr + byteIndex

            Dim gh As GCHandle = GCHandle.Alloc(value, GCHandleType.Pinned)

            If l <= UInt32.MaxValue Then
                MemCpy(p, gh.AddrOfPinnedObject, CUInt(l))
            Else
                n_memcpy(p, gh.AddrOfPinnedObject, l)
            End If

            gh.Free()

        End Sub

        ''' <summary>
        ''' Copies the contents of the buffer into a blittable structure array.
        ''' </summary>
        ''' <typeparam name="T">The structure type.</typeparam>
        ''' <returns>An array of T.</returns>
        ''' <remarks></remarks>
        Public Function ToBlittableStructArray(Of T As Structure)() As T()
            Return ToBlittableStructArrayAt(Of T)(0)
        End Function

        ''' <summary>
        ''' Copies a blittable structure array into the buffer, initializing a new buffer, if necessary.
        ''' </summary>
        ''' <typeparam name="T">The structure type.</typeparam>
        ''' <param name="value">The structure array to copy.</param>
        ''' <remarks></remarks>
        Public Sub FromBlittableStructArray(Of T As Structure)(value As T())
            FromBlittableStructArrayAt(Of T)(0, value)
        End Sub

#End Region '' Structure Utility Methods

#Region "Structure Indexer Properties"

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
                'If _ptr = 0 Then Return Guid.Empty
                'guidAtget(GuidAtAbsolute, CType(CLng(_ptr) + index, IntPtr))
            End Get
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Set(value As Guid)
                'If _ptr = 0 Then Return
                'guidAtset(CType(CLng(_ptr) + index, IntPtr), value)
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
                'If _ptr = 0 Then Return Guid.Empty
                'guidAtget(GuidAt, CType(CLng(_ptr) + (index * 16), IntPtr))
            End Get
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Set(value As Guid)
                'If _ptr = 0 Then Return
                'guidAtset(CType(CLng(_ptr) + (index * 16), IntPtr), value)
            End Set
        End Property

#End Region '' Structure Indexer Properties

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
        Default Public Property ByteAt(index As Long) As Byte
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Get
                Return 0
                'byteAtget(ByteAt, New IntPtr(clng(_ptr) + index))
            End Get
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Set(value As Byte)
                'byteAtset(New IntPtr(clng(_ptr) + index), value)
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
                'sbyteAtget(SByteAt, New IntPtr(clng(_ptr) + index))
            End Get
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Set(value As SByte)
                'sbyteAtset(New IntPtr(clng(_ptr) + index), value)
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
                'charAtget(CharAt, New IntPtr(clng(_ptr) + (index * 2)))
            End Get
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Set(value As Char)
                'charAtset(New IntPtr(clng(_ptr) + (index * 2)), value)
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
                'shortAtget(ShortAt, New IntPtr(clng(_ptr) + (index * 2)))
            End Get
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Set(value As Short)
                'shortAtset(New IntPtr(clng(_ptr) + (index * 2)), value)
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
                'ushortAtget(UShortAt, New IntPtr(clng(_ptr) + (index * 2)))
            End Get
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Set(value As UShort)
                'ushortAtset(New IntPtr(clng(_ptr) + (index * 2)), value)
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
                'intAtget(IntegerAt, New IntPtr(clng(_ptr) + (index * 4)))
            End Get
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Set(value As Integer)
                'intAtset(New IntPtr(clng(_ptr) + (index * 4)), value)
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
                'uintAtget(UIntegerAt, New IntPtr(clng(_ptr) + (index * 4)))
            End Get
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Set(value As UInteger)
                'uintAtset(New IntPtr(clng(_ptr) + (index * 4)), value)
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
                'longAtget(LongAt, New IntPtr(clng(_ptr) + (index * 8)))
            End Get
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Set(value As Long)
                'longAtset(New IntPtr(clng(_ptr) + (index * 8)), value)
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
                'ulongAtget(ULongAt, New IntPtr(clng(_ptr) + (index * 8)))
            End Get
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Set(value As ULong)
                'ulongAtset(New IntPtr(clng(_ptr) + (index * 8)), value)
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
                'singleAtget(SingleAt, New IntPtr(CLng(_ptr) + (index * 4)))
            End Get
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Set(value As Single)
                'singleAtset(New IntPtr(CLng(_ptr) + (index * 4)), value)
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
                'doubleAtget(DoubleAt, New IntPtr(CLng(_ptr) + (index * 8)))
            End Get
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Set(value As Double)
                'doubleAtset(New IntPtr(CLng(_ptr) + (index * 8)), value)
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
                'decimalAtget(DecimalAt, New IntPtr(CLng(_ptr) + (index * 16)))
            End Get
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Set(value As Decimal)
                'decimalAtset(New IntPtr(CLng(_ptr) + (index * 16)), value)
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
                'charAtget(CharAtAbsolute, New IntPtr(clng(_ptr) + index))
            End Get
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Set(value As Char)
                'charAtset(New IntPtr(clng(_ptr) + index), value)
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
                'shortAtget(ShortAtAbsolute, New IntPtr(clng(_ptr) + (index)))
            End Get
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Set(value As Short)
                'shortAtset(New IntPtr(clng(_ptr) + (index)), value)
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
                'ushortAtget(UShortAtAbsolute, New IntPtr(clng(_ptr) + (index)))
            End Get
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Set(value As UShort)
                'ushortAtset(New IntPtr(clng(_ptr) + (index)), value)
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
                'intAtget(IntegerAtAbsolute, New IntPtr(clng(_ptr) + (index)))
            End Get
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Set(value As Integer)
                'intAtset(New IntPtr(clng(_ptr) + (index)), value)
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
                'uintAtget(UIntegerAtAbsolute, New IntPtr(clng(_ptr) + (index)))
            End Get
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Set(value As UInteger)
                'uintAtset(New IntPtr(clng(_ptr) + (index)), value)
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
                'longAtget(LongAtAbsolute, New IntPtr(clng(_ptr) + (index)))
            End Get
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Set(value As Long)
                'longAtset(New IntPtr(clng(_ptr) + (index)), value)
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
                'ulongAtget(ULongAtAbsolute, New IntPtr(clng(_ptr) + (index)))
            End Get
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Set(value As ULong)
                'ulongAtset(New IntPtr(clng(_ptr) + (index)), value)
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
                'singleAtget(SingleAtAbsolute, New IntPtr(CLng(_ptr) + (index)))
            End Get
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Set(value As Single)
                'singleAtset(New IntPtr(CLng(_ptr) + (index)), value)
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
                'doubleAtget(DoubleAtAbsolute, New IntPtr(CLng(_ptr) + (index)))
            End Get
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Set(value As Double)
                'doubleAtset(New IntPtr(CLng(_ptr) + (index)), value)
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
                'decimalAtget(DecimalAtAbsolute, New IntPtr(CLng(_ptr) + (index)))
            End Get
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Set(value As Decimal)
                'decimalAtset(New IntPtr(CLng(_ptr) + (index)), value)
            End Set
        End Property

#End Region '' Integral Absolute Indexer Properties


#Region "Editing"

        ''' <summary>
        ''' Reverses the entire memory pointer.
        ''' </summary>
        ''' <returns>True if successful.</returns>
        ''' <remarks></remarks>
        Public Function Reverse(Optional asChar As Boolean = False) As Boolean
            If (_ptr = 0) Then Return False

            Dim l As Long = Length()

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
        Public Sub Slide(index As Long, length As Long, offset As Long)
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

            src = _ptr + index
            dest = _ptr + index + offset

            Dim a As Long = If(offset < 0, offset * -1, offset)

            Dim buff As New MemPtr(length + a)
            Dim chunk As MemPtr = (buff + length)

            MemCpy(buff._ptr, src, length)
            MemCpy(chunk._ptr, dest, a)

            src = _ptr + index + offset + length

            MemCpy(src, chunk._ptr, a)
            MemCpy(dest, buff._ptr, length)

            buff.Free()
        End Sub

        ''' <summary>
        ''' Pulls the data in from the specified index.
        ''' </summary>
        ''' <param name="index">The index where contraction begins. The contraction starts at this position.</param>
        ''' <param name="amount">Number of bytes to pull in.</param>
        ''' <param name="removePressure">Specify whether to notify the garbage collector.</param>
        ''' <remarks></remarks>
        Public Function PullIn(index As Long, amount As Long, Optional removePressure As Boolean = False) As Long
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
        Public Function PushOut(index As Long, amount As Long, Optional bytes() As Byte = Nothing, Optional addPressure As Boolean = False) As Long
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



            Dim ol As Long = Length() - index

            ReAlloc(hl + amount)

            If (ol > 0) Then
                Slide(index, ol, amount)
            End If

            If bytes IsNot Nothing Then
                SetByteArray(index, bytes)
            Else
                ZeroMemory(index, amount)
            End If

            Return Length()
        End Function

        ''' <summary>
        ''' Slides a block of memory as Unicode characters toward the beginning or toward the end of the buffer.
        ''' </summary>
        ''' <param name="index">The character index preceding the first character in the affected block.</param>
        ''' <param name="length">The length of the block, in characters.</param>
        ''' <param name="offset">The offset amount of the slide, in characters.  If the amount is negative, the block slides to the left, if it is positive it slides to the right.</param>
        ''' <remarks></remarks>
        Public Sub SlideChar(index As Long, length As Long, offset As Long)
            Slide(index << 1, length << 1, offset << 1)
        End Sub

        ''' <summary>
        ''' Pulls the data in from the specified character index.
        ''' </summary>
        ''' <param name="index">The index where contraction begins. The contraction starts at this position.</param>
        ''' <param name="amount">Number of characters to pull in.</param>
        ''' <param name="removePressure">Specify whether to notify the garbage collector.</param>
        ''' <remarks></remarks>
        Public Function PullInChar(index As Long, amount As Long, Optional removePressure As Boolean = False) As Long
            Return PullIn(index << 1, amount << 1)
        End Function

        ''' <summary>
        ''' Extend the buffer from the specified character index.
        ''' </summary>
        ''' <param name="index">The index where expansion begins. The expansion starts at this position.</param>
        ''' <param name="amount">Number of characters to push out.</param>
        ''' <param name="addPressure">Specify whether to notify the garbage collector.</param>
        ''' <remarks></remarks>
        Public Function PushOutChar(index As Long, amount As Long, Optional chars() As Char = Nothing, Optional addPressure As Boolean = False) As Long
            Return PushOut(index << 1, amount << 1, ToBytes(chars))
        End Function

        ''' <summary>
        ''' Parts the string in both directions from index.
        ''' </summary>
        ''' <param name="index">The index from which to expand.</param>
        ''' <param name="amount">The amount of expansion, in both directions, so the total expansion will be amount * 1.</param>
        ''' <param name="addPressure">Specify whether to notify the garbage collector.</param>
        ''' <remarks></remarks>
        Public Sub Part(index As Long, amount As Long, Optional addPressure As Boolean = False)
            If _ptr = 0 Then
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
        Public Sub Insert(index As Long, value() As Byte, Optional addPressure As Boolean = False)
            PushOut(index, value.Length, value)
        End Sub

        ''' <summary>
        ''' Inserts the specified characters at the specified character index.
        ''' </summary>
        ''' <param name="index">Index at which to insert.</param>
        ''' <param name="value">Character array to insert</param>
        ''' <param name="addPressure">Specify whether to notify the garbage collector.</param>
        ''' <remarks></remarks>
        Public Sub Insert(index As Long, value() As Char, Optional addPressure As Boolean = False)
            PushOutChar(index, value.Length, value)
        End Sub

        ''' <summary>
        ''' Delete the memory from the specified index.  Calls PullIn.
        ''' </summary>
        ''' <param name="index">Index to start the delete.</param>
        ''' <param name="amount">Amount of bytes to delete</param>
        ''' <param name="removePressure">Specify whether to notify the garbage collector.</param>
        ''' <remarks></remarks>
        Public Sub Delete(index As Long, amount As Long, Optional removePressure As Boolean = False)
            PullIn(index, amount)
        End Sub

        ''' <summary>
        ''' Delete the memory from the specified character index.  Calls PullIn.
        ''' </summary>
        ''' <param name="index">Index to start the delete.</param>
        ''' <param name="amount">Amount of characters to delete</param>
        ''' <param name="removePressure">Specify whether to notify the garbage collector.</param>
        ''' <remarks></remarks>
        Public Sub DeleteChar(index As Long, amount As Long, Optional removePressure As Boolean = False)
            PullInChar(index, amount)
        End Sub

        ''' <summary>
        ''' Consumes the buffer in both directions from specified index.
        ''' </summary>
        ''' <param name="index">Index at which consuming begins.</param>
        ''' <param name="amount">Amount of contraction, in both directions, so the total contraction will be amount * 1.</param>
        ''' <param name="removePressure">Specify whether to notify the garbage collector.</param>
        ''' <remarks></remarks>
        Public Sub Consume(index As Long, amount As Long, Optional removePressure As Boolean = False)
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
        Public Sub ConsumeChar(index As Long, amount As Long, Optional removePressure As Boolean = False)
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


        '' Because when setting the memory value we cannot rely on CTypes when private heaps are
        '' being referenced, the only way to ensure that a private heap can initialize this
        '' structure is to provide a way for an external caller to specify the private heap.

        '' There is no equivalent 'getter' function for two reasons:
        '' First:
        '' It is not possible to marshal a late-bound object into CopyMemory.
        '' Second:
        '' Even if it were, the getter is not necessary because the CType can return
        '' a managed object without allocating or deallocating resources.
#Region "Setter For Integral Types"

        ''' <summary>
        ''' Set the buffer with the specified supported primitive object
        ''' </summary>
        ''' <param name="value">Object to copy to the buffer.</param>
        ''' <param name="addPressure">Specify whether to tell the garbage collector.</param>
        ''' <param name="hHeap">Specify an optional alternate heap.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SetIntegral(value As Object, Optional addPressure As Boolean = False, Optional hHeap As IntPtr? = Nothing) As Boolean
            Dim l As Long = _checkType(value.GetType)
            If l = 0 Then Return False

            If value.GetType.IsArray Then
                l *= CType(value, Array).Length
            ElseIf TypeOf value Is String Then
                l *= (CType(value, String).Length + 1)
            End If

            If Not Alloc(l, addPressure, hHeap) Then
                Throw New InsufficientMemoryException
            Else
                ZeroMemory()
            End If

            Select Case value.GetType

                Case GetType(SByte)
                    sbyteAtset(_ptr, CType(value, SByte))

                Case GetType(Byte)
                    byteAtset(_ptr, CType(value, Byte))

                Case GetType(Char)
                    charAtset(_ptr, CType(value, Char))

                Case GetType(Short)
                    shortAtset(_ptr, CType(value, Short))

                Case GetType(UShort)
                    ushortAtset(_ptr, CType(value, UShort))

                Case GetType(Integer)
                    intAtset(_ptr, CType(value, Integer))

                Case GetType(UInteger)
                    uintAtset(_ptr, CUInt(value))

                Case GetType(Long)
                    longAtset(_ptr, CType(value, Long))

                Case GetType(ULong)
                    ulongAtset(_ptr, CType(value, ULong))

                Case GetType(Single)
                    singleAtset(_ptr, CType(value, Single))

                Case GetType(Double)
                    doubleAtset(_ptr, CType(value, Double))

                Case GetType(Date)
                    dateAtset(_ptr, CType(value, Date))

                Case GetType(Color)
                    CopyMemory(_ptr, CType(value, Color), l)

                Case GetType(Guid)
                    guidAtset(_ptr, CType(value, Guid))

                Case GetType(Decimal)
                    decimalAtset(_ptr, CType(value, Decimal))

                Case GetType(SByte())
                    sbyteArrAtset(_ptr, CType(value, SByte()), CUInt(l))

                Case GetType(Byte())
                    byteArrAtset(_ptr, CType(value, Byte()), CUInt(l))

                Case GetType(Char())
                    charArrAtset(_ptr, CType(value, Char()), CUInt(l))

                Case GetType(Short())
                    shortArrAtset(_ptr, CType(value, Short()), CUInt(l))

                Case GetType(UShort())
                    ushortArrAtset(_ptr, CType(value, UShort()), CUInt(l))

                Case GetType(Integer())
                    intArrAtset(_ptr, CType(value, Integer()), CUInt(l))

                Case GetType(UInteger())
                    uintArrAtset(_ptr, CType(value, UInteger()), CUInt(l))

                Case GetType(Long())
                    longArrAtset(_ptr, CType(value, Long()), CUInt(l))

                Case GetType(ULong())
                    ulongArrAtset(_ptr, CType(value, ULong()), CUInt(l))

                Case GetType(Single())
                    singleArrAtset(_ptr, CType(value, Single()), CUInt(l))

                Case GetType(Double())
                    doubleArrAtset(_ptr, CType(value, Double()), CUInt(l))

                Case GetType(Date())
                    dateArrAtset(_ptr, CType(value, Date()), CUInt(l))

                Case GetType(Color())
                    CopyMemory(_ptr, CType(value, Color()), l)

                Case GetType(Guid())
                    guidArrAtset(_ptr, CType(value, Guid()), CUInt(l))

                Case GetType(Decimal())
                    decimalArrAtset(_ptr, CType(value, Decimal()), CUInt(l))

                Case GetType(String)
                    QuickCopyObject(Of String)(_ptr, CType(value, String), CUInt(l - 2))

            End Select

            SetIntegral = True
        End Function

        ''' <summary>
        ''' Check the type against the cached system types that are accepted as input.
        ''' </summary>
        ''' <param name="type">The type to check.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function _checkType(type As System.Type) As Integer
            For Each kv In _primitiveCache
                If kv.Key = type Then
                    Return kv.Value
                End If
            Next

            Return 0
        End Function

        Private Shared Sub _buildIntegralCache()

            '' build atomic primitive type cache
            _primitiveCache.Add(New KeyValuePair(Of System.Type, Integer)(GetType(SByte), 1))
            _primitiveCache.Add(New KeyValuePair(Of System.Type, Integer)(GetType(Byte), 1))
            _primitiveCache.Add(New KeyValuePair(Of System.Type, Integer)(GetType(Char), 2))
            _primitiveCache.Add(New KeyValuePair(Of System.Type, Integer)(GetType(Short), 2))
            _primitiveCache.Add(New KeyValuePair(Of System.Type, Integer)(GetType(UShort), 2))
            _primitiveCache.Add(New KeyValuePair(Of System.Type, Integer)(GetType(Integer), 4))
            _primitiveCache.Add(New KeyValuePair(Of System.Type, Integer)(GetType(UInteger), 4))
            _primitiveCache.Add(New KeyValuePair(Of System.Type, Integer)(GetType(Long), 8))
            _primitiveCache.Add(New KeyValuePair(Of System.Type, Integer)(GetType(ULong), 8))
            _primitiveCache.Add(New KeyValuePair(Of System.Type, Integer)(GetType(Single), 4))
            _primitiveCache.Add(New KeyValuePair(Of System.Type, Integer)(GetType(Double), 8))
            _primitiveCache.Add(New KeyValuePair(Of System.Type, Integer)(GetType(Date), 8))
            _primitiveCache.Add(New KeyValuePair(Of System.Type, Integer)(GetType(Color), 4))
            _primitiveCache.Add(New KeyValuePair(Of System.Type, Integer)(GetType(Guid), 16))
            _primitiveCache.Add(New KeyValuePair(Of System.Type, Integer)(GetType(Decimal), 16))

            '' build array primitive type cache
            _primitiveCache.Add(New KeyValuePair(Of System.Type, Integer)(GetType(SByte()), 1))
            _primitiveCache.Add(New KeyValuePair(Of System.Type, Integer)(GetType(Byte()), 1))
            _primitiveCache.Add(New KeyValuePair(Of System.Type, Integer)(GetType(Char()), 2))
            _primitiveCache.Add(New KeyValuePair(Of System.Type, Integer)(GetType(Short()), 2))
            _primitiveCache.Add(New KeyValuePair(Of System.Type, Integer)(GetType(UShort()), 2))
            _primitiveCache.Add(New KeyValuePair(Of System.Type, Integer)(GetType(Integer()), 4))
            _primitiveCache.Add(New KeyValuePair(Of System.Type, Integer)(GetType(UInteger()), 4))
            _primitiveCache.Add(New KeyValuePair(Of System.Type, Integer)(GetType(Long()), 8))
            _primitiveCache.Add(New KeyValuePair(Of System.Type, Integer)(GetType(ULong()), 8))
            _primitiveCache.Add(New KeyValuePair(Of System.Type, Integer)(GetType(Single()), 4))
            _primitiveCache.Add(New KeyValuePair(Of System.Type, Integer)(GetType(Double()), 8))
            _primitiveCache.Add(New KeyValuePair(Of System.Type, Integer)(GetType(Date()), 8))
            _primitiveCache.Add(New KeyValuePair(Of System.Type, Integer)(GetType(Color()), 4))
            _primitiveCache.Add(New KeyValuePair(Of System.Type, Integer)(GetType(Guid()), 16))
            _primitiveCache.Add(New KeyValuePair(Of System.Type, Integer)(GetType(Decimal()), 16))

            '' string primitive
            _primitiveCache.Add(New KeyValuePair(Of System.Type, Integer)(GetType(String), 2))

        End Sub

#End Region '' Getter and Setter For Integral Types

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

        '' Math with other MemPtr
        Public Shared Operator +(operand1 As MemPtr, operand2 As MemPtr) As MemPtr
            operand1._ptr = New IntPtr(CLng(operand1._ptr) + CLng(operand2._ptr))
            Return operand1
        End Operator

        Public Shared Operator -(operand1 As MemPtr, operand2 As MemPtr) As MemPtr
            operand1._ptr = New IntPtr(CLng(operand1._ptr) - CLng(operand2._ptr))
            Return operand1
        End Operator

        Public Shared Operator *(operand1 As MemPtr, operand2 As MemPtr) As MemPtr
            operand1._ptr = New IntPtr(CLng(operand1._ptr) * CLng(operand2._ptr))
            Return operand1
        End Operator

        Public Shared Operator /(operand1 As MemPtr, operand2 As MemPtr) As MemPtr
            operand1._ptr = New IntPtr(CLng(CLng(operand1._ptr) / CLng(operand2._ptr)))
            Return operand1
        End Operator

        Public Shared Operator \(operand1 As MemPtr, operand2 As MemPtr) As MemPtr
            operand1._ptr = New IntPtr(CLng(operand1._ptr) \ CLng(operand2._ptr))
            Return operand1
        End Operator


        '' Math with IntPtr
        Public Shared Operator +(operand1 As MemPtr, operand2 As IntPtr) As MemPtr
            operand1._ptr = New IntPtr(CLng(operand1._ptr) + operand2.ToInt64)
            Return operand1
        End Operator

        Public Shared Operator -(operand1 As MemPtr, operand2 As IntPtr) As MemPtr
            operand1._ptr = New IntPtr(CLng(operand1._ptr) - operand2.ToInt64)
            Return operand1
        End Operator

        Public Shared Operator *(operand1 As MemPtr, operand2 As IntPtr) As MemPtr
            operand1._ptr = New IntPtr(CLng(operand1._ptr) * operand2.ToInt64)
            Return operand1
        End Operator

        Public Shared Operator /(operand1 As MemPtr, operand2 As IntPtr) As MemPtr
            operand1._ptr = New IntPtr(CLng(CLng(operand1._ptr) / operand2.ToInt64))
            Return operand1
        End Operator

        Public Shared Operator \(operand1 As MemPtr, operand2 As IntPtr) As MemPtr
            operand1._ptr = New IntPtr(CLng(operand1._ptr) \ operand2.ToInt64)
            Return operand1
        End Operator



        '' math with UIntPtr
        Public Shared Operator +(operand1 As MemPtr, operand2 As UIntPtr) As MemPtr
            operand1._ptr = New IntPtr(CLng(CLng(operand1._ptr) + CLng(CULng(operand2))))
            Return operand1
        End Operator

        Public Shared Operator -(operand1 As MemPtr, operand2 As UIntPtr) As MemPtr
            operand1._ptr = New IntPtr(CLng(CLng(operand1._ptr) - CLng(CULng(operand2))))
            Return operand1
        End Operator

        Public Shared Operator *(operand1 As MemPtr, operand2 As UIntPtr) As MemPtr
            operand1._ptr = New IntPtr(CLng(CLng(operand1._ptr) * CLng(CULng(operand2))))
            Return operand1
        End Operator

        Public Shared Operator /(operand1 As MemPtr, operand2 As UIntPtr) As MemPtr
            operand1._ptr = New IntPtr(CLng(CLng(operand1._ptr) / CLng(CULng(operand2))))
            Return operand1
        End Operator

        Public Shared Operator \(operand1 As MemPtr, operand2 As UIntPtr) As MemPtr
            operand1._ptr = New IntPtr(CLng(CLng(operand1._ptr) \ CLng(CLng(CULng(operand2)))))
            Return operand1
        End Operator


        '' Signed ordinals

        '' we can add long ints to the _ptr 
        Public Shared Operator +(operand1 As MemPtr, operand2 As Long) As MemPtr
            operand1._ptr = New IntPtr(CLng(operand1._ptr) + operand2)
            Return operand1
        End Operator

        Public Shared Operator -(operand1 As MemPtr, operand2 As Long) As MemPtr
            operand1._ptr = New IntPtr(CLng(operand1._ptr) - operand2)
            Return operand1
        End Operator

        Public Shared Operator *(operand1 As MemPtr, operand2 As Long) As MemPtr
            operand1._ptr = New IntPtr(CLng(operand1._ptr) * operand2)
            Return operand1
        End Operator

        Public Shared Operator /(operand1 As MemPtr, operand2 As Long) As MemPtr
            operand1._ptr = New IntPtr(CLng(CLng(operand1._ptr) / operand2))
            Return operand1
        End Operator

        Public Shared Operator \(operand1 As MemPtr, operand2 As Long) As MemPtr
            operand1._ptr = New IntPtr(CLng(operand1._ptr) \ operand2)
            Return operand1
        End Operator

        '' we can add ints to the _ptr 
        Public Shared Operator +(operand1 As MemPtr, operand2 As Integer) As MemPtr
            operand1._ptr = New IntPtr(CLng(operand1._ptr) + operand2)
            Return operand1
        End Operator

        Public Shared Operator -(operand1 As MemPtr, operand2 As Integer) As MemPtr
            operand1._ptr = New IntPtr(CLng(operand1._ptr) - operand2)
            Return operand1
        End Operator

        Public Shared Operator *(operand1 As MemPtr, operand2 As Integer) As MemPtr
            operand1._ptr = New IntPtr(CLng(operand1._ptr) * operand2)
            Return operand1
        End Operator

        Public Shared Operator /(operand1 As MemPtr, operand2 As Integer) As MemPtr
            operand1._ptr = New IntPtr(CLng(CLng(operand1._ptr) / operand2))
            Return operand1
        End Operator

        Public Shared Operator \(operand1 As MemPtr, operand2 As Integer) As MemPtr
            operand1._ptr = New IntPtr(CLng(operand1._ptr) \ operand2)
            Return operand1
        End Operator


        '' we can add shorts to the _ptr 
        Public Shared Operator +(operand1 As MemPtr, operand2 As Short) As MemPtr
            operand1._ptr = New IntPtr(CLng(operand1._ptr) + operand2)
            Return operand1
        End Operator

        Public Shared Operator -(operand1 As MemPtr, operand2 As Short) As MemPtr
            operand1._ptr = New IntPtr(CLng(operand1._ptr) - operand2)
            Return operand1
        End Operator

        Public Shared Operator *(operand1 As MemPtr, operand2 As Short) As MemPtr
            operand1._ptr = New IntPtr(CLng(operand1._ptr) * operand2)
            Return operand1
        End Operator

        Public Shared Operator /(operand1 As MemPtr, operand2 As Short) As MemPtr
            operand1._ptr = New IntPtr(CLng(CLng(operand1._ptr) / operand2))
            Return operand1
        End Operator

        Public Shared Operator \(operand1 As MemPtr, operand2 As Short) As MemPtr
            operand1._ptr = New IntPtr(CLng(operand1._ptr) \ operand2)
            Return operand1
        End Operator


        '' we can add signed sbytes to the _ptr 
        Public Shared Operator +(operand1 As MemPtr, operand2 As SByte) As MemPtr
            operand1._ptr = New IntPtr(CLng(operand1._ptr) + operand2)
            Return operand1
        End Operator

        Public Shared Operator -(operand1 As MemPtr, operand2 As SByte) As MemPtr
            operand1._ptr = New IntPtr(CLng(operand1._ptr) - operand2)
            Return operand1
        End Operator

        Public Shared Operator *(operand1 As MemPtr, operand2 As SByte) As MemPtr
            operand1._ptr = New IntPtr(CLng(operand1._ptr) * operand2)
            Return operand1
        End Operator

        Public Shared Operator /(operand1 As MemPtr, operand2 As SByte) As MemPtr
            operand1._ptr = New IntPtr(CLng(CLng(operand1._ptr) / operand2))
            Return operand1
        End Operator

        Public Shared Operator \(operand1 As MemPtr, operand2 As SByte) As MemPtr
            operand1._ptr = New IntPtr(CLng(operand1._ptr) \ operand2)
            Return operand1
        End Operator


        '' Unsigned ordinals

        '' we can add ulong ints to the _ptr 
        Public Shared Operator +(operand1 As MemPtr, operand2 As ULong) As MemPtr
            operand1._ptr = New IntPtr(CLng(operand1._ptr) + CLng(operand2))
            Return operand1
        End Operator

        Public Shared Operator -(operand1 As MemPtr, operand2 As ULong) As MemPtr
            operand1._ptr = New IntPtr(CLng(operand1._ptr) - CLng(operand2))
            Return operand1
        End Operator

        Public Shared Operator *(operand1 As MemPtr, operand2 As ULong) As MemPtr
            operand1._ptr = New IntPtr(CLng(operand1._ptr) * CLng(operand2))
            Return operand1
        End Operator

        Public Shared Operator /(operand1 As MemPtr, operand2 As ULong) As MemPtr
            operand1._ptr = New IntPtr(CLng(CLng(operand1._ptr) / CLng(operand2)))
            Return operand1
        End Operator

        Public Shared Operator \(operand1 As MemPtr, operand2 As ULong) As MemPtr
            operand1._ptr = New IntPtr(CLng(operand1._ptr) \ CLng(operand2))
            Return operand1
        End Operator


        '' we can add uints to the _ptr 
        Public Shared Operator +(operand1 As MemPtr, operand2 As UInteger) As MemPtr
            operand1._ptr = New IntPtr(CLng(operand1._ptr) + operand2)
            Return operand1
        End Operator

        Public Shared Operator -(operand1 As MemPtr, operand2 As UInteger) As MemPtr
            operand1._ptr = New IntPtr(CLng(operand1._ptr) - operand2)
            Return operand1
        End Operator

        Public Shared Operator *(operand1 As MemPtr, operand2 As UInteger) As MemPtr
            operand1._ptr = New IntPtr(CLng(operand1._ptr) * operand2)
            Return operand1
        End Operator

        Public Shared Operator /(operand1 As MemPtr, operand2 As UInteger) As MemPtr
            operand1._ptr = New IntPtr(CLng(CLng(operand1._ptr) / operand2))
            Return operand1
        End Operator

        Public Shared Operator \(operand1 As MemPtr, operand2 As UInteger) As MemPtr
            operand1._ptr = New IntPtr(CLng(operand1._ptr) \ operand2)
            Return operand1
        End Operator


        '' we can add ushorts to the _ptr 
        Public Shared Operator +(operand1 As MemPtr, operand2 As UShort) As MemPtr
            operand1._ptr = New IntPtr(CLng(operand1._ptr) + operand2)
            Return operand1
        End Operator

        Public Shared Operator -(operand1 As MemPtr, operand2 As UShort) As MemPtr
            operand1._ptr = New IntPtr(CLng(operand1._ptr) - operand2)
            Return operand1
        End Operator

        Public Shared Operator *(operand1 As MemPtr, operand2 As UShort) As MemPtr
            operand1._ptr = New IntPtr(CLng(operand1._ptr) * operand2)
            Return operand1
        End Operator

        Public Shared Operator /(operand1 As MemPtr, operand2 As UShort) As MemPtr
            operand1._ptr = New IntPtr(CLng(CLng(operand1._ptr) / operand2))
            Return operand1
        End Operator

        Public Shared Operator \(operand1 As MemPtr, operand2 As UShort) As MemPtr
            operand1._ptr = New IntPtr(CLng(operand1._ptr) \ operand2)
            Return operand1
        End Operator


        '' we can add bytes to the _ptr 
        Public Shared Operator +(operand1 As MemPtr, operand2 As Byte) As MemPtr
            operand1._ptr = New IntPtr(CLng(operand1._ptr) + operand2)
            Return operand1
        End Operator

        Public Shared Operator -(operand1 As MemPtr, operand2 As Byte) As MemPtr
            operand1._ptr = New IntPtr(CLng(operand1._ptr) - operand2)
            Return operand1
        End Operator

        Public Shared Operator *(operand1 As MemPtr, operand2 As Byte) As MemPtr
            operand1._ptr = New IntPtr(CLng(operand1._ptr) * operand2)
            Return operand1
        End Operator

        Public Shared Operator /(operand1 As MemPtr, operand2 As Byte) As MemPtr
            operand1._ptr = New IntPtr(CLng(CLng(operand1._ptr) / operand2))
            Return operand1
        End Operator

        Public Shared Operator \(operand1 As MemPtr, operand2 As Byte) As MemPtr
            operand1._ptr = New IntPtr(CLng(operand1._ptr) \ operand2)
            Return operand1
        End Operator


#End Region '' Basic Math For Pointers

#Region "Equality Operators"

        '' MemPtr
        Public Overrides Function Equals(obj As Object) As Boolean
            If TypeOf obj Is MemPtr Then
                Return CType(obj, MemPtr)._ptr = _ptr
            ElseIf TypeOf obj Is IntPtr Then
                Return CType(obj, IntPtr) = _ptr
            ElseIf TypeOf obj Is UIntPtr Then
                Return ToSigned(CType(obj, UIntPtr)) = _ptr
            Else
                Return False
            End If
        End Function


        Public Overloads Function Equals(other As MemPtr) As Boolean Implements IEquatable(Of MemPtr).Equals
            Return other._ptr = _ptr
        End Function


        Public Overloads Function Equals(other As IntPtr) As Boolean Implements IEquatable(Of IntPtr).Equals
            Return other = _ptr
        End Function

        Public Overloads Function Equals(other As UIntPtr) As Boolean Implements IEquatable(Of UIntPtr).Equals
            Return ToSigned(other) = _ptr
        End Function

        Public Shared Operator =(v1 As MemPtr, v2 As MemPtr) As Boolean
            Return (CLng(v1._ptr) = CLng(v2._ptr))
        End Operator

        Public Shared Operator <>(v1 As MemPtr, v2 As MemPtr) As Boolean
            Return (CLng(v1._ptr) <> CLng(v2._ptr))
        End Operator

        Public Shared Operator <(v1 As MemPtr, v2 As MemPtr) As Boolean
            Return (CLng(v1._ptr) < CLng(v2._ptr))
        End Operator

        Public Shared Operator >(v1 As MemPtr, v2 As MemPtr) As Boolean
            Return (CLng(v1._ptr) > CLng(v2._ptr))
        End Operator

        Public Shared Operator <=(v1 As MemPtr, v2 As MemPtr) As Boolean
            Return (CLng(v1._ptr) <= CLng(v2._ptr))
        End Operator

        Public Shared Operator >=(v1 As MemPtr, v2 As MemPtr) As Boolean
            Return (CLng(v1._ptr) >= CLng(v2._ptr))
        End Operator

        '' IntPtr
        Public Shared Operator =(v1 As MemPtr, v2 As IntPtr) As Boolean
            Return (CLng(v1._ptr) = CLng(v2))
        End Operator

        Public Shared Operator <>(v1 As MemPtr, v2 As IntPtr) As Boolean
            Return (CLng(v1._ptr) <> CLng(v2))
        End Operator

        Public Shared Operator <(v1 As MemPtr, v2 As IntPtr) As Boolean
            Return (CLng(v1._ptr) < CLng(v2))
        End Operator

        Public Shared Operator >(v1 As MemPtr, v2 As IntPtr) As Boolean
            Return (CLng(v1._ptr) > CLng(v2))
        End Operator

        Public Shared Operator <=(v1 As MemPtr, v2 As IntPtr) As Boolean
            Return (CLng(v1._ptr) <= CLng(v2))
        End Operator

        Public Shared Operator >=(v1 As MemPtr, v2 As IntPtr) As Boolean
            Return (CLng(v1._ptr) >= CLng(v2))
        End Operator

#End Region '' Equality Operators

#Region "CTypes for IntPtr and UIntPtr"

        Public Shared Widening Operator CType(operand As MemPtr) As IntPtr
            Return operand._ptr
        End Operator

        Public Shared Widening Operator CType(operand As IntPtr) As MemPtr
            Return New MemPtr(operand)
        End Operator

        Public Shared Narrowing Operator CType(operand As MemPtr) As UIntPtr
            Return ToUnsigned(operand._ptr)
        End Operator

        Public Shared Narrowing Operator CType(operand As UIntPtr) As MemPtr
            Return New MemPtr(operand)
        End Operator

#End Region '' CType IntPtr/UIntPtr

#Region "Integral CTypes"

        '' in
        Public Shared Widening Operator CType(operand As SByte) As MemPtr
            Dim mm As New MemPtr
            If mm.Alloc(1) Then
                mm.SByteAt(0) = operand
            Else
                Throw New InsufficientMemoryException
            End If

            Return mm
        End Operator

        Public Shared Widening Operator CType(operand As Byte) As MemPtr
            Dim mm As New MemPtr
            If mm.Alloc(1) Then
                mm.ByteAt(0) = operand
            Else
                Throw New InsufficientMemoryException
            End If

            Return mm
        End Operator

        Public Shared Widening Operator CType(operand As Short) As MemPtr
            Dim mm As New MemPtr
            If mm.Alloc(2) Then
                mm.ShortAt(0) = operand
            Else
                Throw New InsufficientMemoryException
            End If

            Return mm
        End Operator

        Public Shared Widening Operator CType(operand As UShort) As MemPtr
            Dim mm As New MemPtr
            If mm.Alloc(2) Then
                mm.UShortAt(0) = operand
            Else
                Throw New InsufficientMemoryException
            End If

            Return mm
        End Operator

        Public Shared Widening Operator CType(operand As Integer) As MemPtr
            Dim mm As New MemPtr
            If mm.Alloc(4) Then
                mm.IntegerAt(0) = operand
            Else
                Throw New InsufficientMemoryException
            End If

            Return mm
        End Operator

        Public Shared Widening Operator CType(operand As UInteger) As MemPtr
            Dim mm As New MemPtr
            If mm.Alloc(4) Then
                mm.UIntegerAt(0) = operand
            Else
                Throw New InsufficientMemoryException
            End If

            Return mm
        End Operator

        Public Shared Widening Operator CType(operand As Long) As MemPtr
            Dim mm As New MemPtr
            If mm.Alloc(8) Then
                mm.LongAt(0) = operand
            Else
                Throw New InsufficientMemoryException
            End If

            Return mm
        End Operator

        Public Shared Widening Operator CType(operand As ULong) As MemPtr
            Dim mm As New MemPtr
            If mm.Alloc(8) Then
                mm.ULongAt(0) = operand
            Else
                Throw New InsufficientMemoryException
            End If

            Return mm
        End Operator

        Public Shared Widening Operator CType(operand As Single) As MemPtr
            Dim mm As New MemPtr
            If mm.Alloc(4) Then
                mm.SingleAt(0) = operand
            Else
                Throw New InsufficientMemoryException
            End If

            Return mm
        End Operator

        Public Shared Widening Operator CType(operand As Double) As MemPtr
            Dim mm As New MemPtr
            If mm.Alloc(8) Then
                mm.DoubleAt(0) = operand
            Else
                Throw New InsufficientMemoryException
            End If

            Return mm
        End Operator


        '' out
        Public Shared Widening Operator CType(operand As MemPtr) As SByte
            Return operand.SByteAt(0)
        End Operator

        Public Shared Widening Operator CType(operand As MemPtr) As Byte
            Return operand.ByteAt(0)
        End Operator

        Public Shared Widening Operator CType(operand As MemPtr) As Char
            Return operand.CharAt(0)
        End Operator

        Public Shared Widening Operator CType(operand As MemPtr) As Short
            Return operand.ShortAt(0)
        End Operator

        Public Shared Widening Operator CType(operand As MemPtr) As UShort
            Return operand.UShortAt(0)
        End Operator

        Public Shared Widening Operator CType(operand As MemPtr) As Integer
            Return operand.IntegerAt(0)
        End Operator

        Public Shared Widening Operator CType(operand As MemPtr) As UInteger
            Return operand.UIntegerAt(0)
        End Operator

        Public Shared Widening Operator CType(operand As MemPtr) As Long
            Return operand.LongAt(0)
        End Operator

        Public Shared Widening Operator CType(operand As MemPtr) As ULong
            Return operand.ULongAt(0)
        End Operator

        Public Shared Widening Operator CType(operand As MemPtr) As Single
            Return operand.SingleAt(0)
        End Operator

        Public Shared Widening Operator CType(operand As MemPtr) As Double
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
        Public Shared Widening Operator CType(operand As Byte()) As MemPtr
            Return MemPtr.Empty
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As SByte()) As MemPtr
            Return MemPtr.Empty
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As Short()) As MemPtr
            Return MemPtr.Empty
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As UShort()) As MemPtr
            Return MemPtr.Empty
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As Integer()) As MemPtr
            Return MemPtr.Empty
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As UInteger()) As MemPtr
            Return MemPtr.Empty
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As Long()) As MemPtr
            Return MemPtr.Empty
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As ULong()) As MemPtr
            Return MemPtr.Empty
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As Single()) As MemPtr
            Return MemPtr.Empty
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As Double()) As MemPtr
            Return MemPtr.Empty
        End Operator

        '' Out
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As MemPtr) As SByte()
            Return Nothing
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As MemPtr) As Byte()
            Return Nothing
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As MemPtr) As Short()
            Return Nothing
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As MemPtr) As UShort()
            Return Nothing
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As MemPtr) As Integer()
            Return Nothing
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As MemPtr) As UInteger()
            Return Nothing
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As MemPtr) As Long()
            Return Nothing
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As MemPtr) As ULong()
            Return Nothing
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As MemPtr) As Single()
            Return Nothing
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As MemPtr) As Double()
            Return Nothing
        End Operator

#End Region '' Integral Array CTypes

#Region "String and Char CTypes"

        '' Returns a pretty string, based on null-termination logic, instead of
        '' returning every character in the allocated block.
        Public Shared Widening Operator CType(operand As MemPtr) As String
            Return operand.GrabString(CType(0, IntPtr))
        End Operator

        '' We add 2 bytes to give us a proper null-terminated string in memory.
        Public Shared Widening Operator CType(operand As String) As MemPtr
            Dim mm As New MemPtr
            mm.SetString(CType(0, IntPtr), operand)
            Return mm

            'Dim i As Integer = operand.Length << 1
            'Dim mm As New MemPtr(i + 2)
            'QuickCopyObject(Of String)(mm.Handle, operand, CUInt(i))
            'Return mm
        End Operator

        '' Here we return every character in the allocated block.
        <MethodImpl(MethodImplOptions.ForwardRef)>
        Public Shared Widening Operator CType(operand As MemPtr) As Char()
            Return Nothing
        End Operator

        '' We just set the character information into the memory buffer, verbatim.
        <MethodImpl(MethodImplOptions.ForwardRef)>
        Public Shared Widening Operator CType(operand As Char()) As MemPtr
            Return MemPtr.Empty
        End Operator

        Public Shared Widening Operator CType(operand As String()) As MemPtr
            If operand Is Nothing OrElse operand.Length = 0 Then Return MemPtr.Empty

            Dim mm As New MemPtr
            Dim c As Integer = operand.Length - 1
            Dim l As Long = 2

            For i = 0 To c
                l += (operand(i).Length + 1) * 2
            Next

            If Not mm.Alloc(l) Then Return MemPtr.Empty

            Dim p As IntPtr = mm._ptr

            For i = 0 To c
                QuickCopyObject(Of String)(p, operand(i), CType(operand(i).Length * 2, UInteger))
                p += (operand(i).Length * 2) + 2
            Next

            Return mm
        End Operator

        Public Shared Widening Operator CType(operand As MemPtr) As String()
            Return operand.GrabStringArray(CType(0, IntPtr))
        End Operator

#End Region '' String and Char CTypes

#Region "Guid Scalar and Array CTypes"

        Public Shared Narrowing Operator CType(operand As Guid) As MemPtr
            Dim mm As New MemPtr
            If mm.Alloc(16) Then
                mm.GuidAt(0) = operand
            Else
                Throw New OutOfMemoryException
            End If
            Return mm
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Narrowing Operator CType(operand As Guid()) As MemPtr
            Return MemPtr.Empty
        End Operator

        Public Shared Narrowing Operator CType(operand As MemPtr) As Guid
            Return operand.GuidAt(0)
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Narrowing Operator CType(operand As MemPtr) As Guid()
            Return Nothing
        End Operator

#End Region '' Guid Scalar and Array CTypes

#Region "Color, Decimal, and Date CTypes"

        Public Shared Widening Operator CType(operand As Decimal) As MemPtr
            Dim mm As New MemPtr
            If mm.Alloc(16) Then
                mm.DecimalAt(0) = operand
            Else
                Throw New OutOfMemoryException
            End If
            Return mm
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As Decimal()) As MemPtr
            Return MemPtr.Empty
        End Operator

        Public Shared Widening Operator CType(operand As MemPtr) As Decimal
            Return operand.DecimalAt(0)
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As MemPtr) As Decimal()
            Return Nothing
        End Operator


        '' Color

        Public Shared Widening Operator CType(operand As Color) As MemPtr
            Dim mm As New MemPtr
            If mm.Alloc(4) Then
                mm.IntegerAt(0) = operand.ToArgb
            Else
                Throw New OutOfMemoryException
            End If
            Return mm
        End Operator

        Public Shared Widening Operator CType(operand As Color()) As MemPtr
            Dim mm As New MemPtr

            If mm.Alloc(4 * operand.Length) Then
                MemCpy(mm._ptr, operand, 4 * operand.Length)
            Else
                Throw New OutOfMemoryException
            End If

            Return mm
        End Operator

        Public Shared Widening Operator CType(operand As MemPtr) As Color
            Return Color.FromArgb(operand.IntegerAt(0))
        End Operator

        Public Shared Widening Operator CType(operand As MemPtr) As Color()
            Dim g() As Color
            Dim l As Long = operand.Length

            Dim a As Long = CLng(l >> 2)

            If a > Int32.MaxValue Then Throw New ArgumentOutOfRangeException(MemTooBig)

            g = CType(Array.CreateInstance(GetType(Long), CInt(a)), Color())

            MemCpy(g, CType(operand, IntPtr), l)
            Return g
        End Operator

        '' DateTime

        Public Shared Widening Operator CType(operand As Date) As MemPtr
            Dim mm As New MemPtr
            If mm.Alloc(8) Then
                mm.LongAt(0) = operand.ToBinary()
            Else
                Throw New OutOfMemoryException
            End If
            Return mm
        End Operator

        Public Shared Widening Operator CType(operand As Date()) As MemPtr
            Dim mm As New MemPtr
            If mm.Alloc(8 * operand.Length) Then
                QuickCopyObject(Of Date())(mm.Handle, operand, CUInt(8 * operand.Length))
            Else
                Throw New OutOfMemoryException
            End If
            Return mm
        End Operator

        Public Shared Widening Operator CType(operand As MemPtr) As Date
            Return Date.FromBinary(operand.LongAt(0))
        End Operator

        Public Shared Widening Operator CType(operand As MemPtr) As Date()

            Dim clr() As Date

            Dim l As Long = CLng(operand.Length / 8L)
            If l > UInt32.MaxValue Then Throw New InvalidCastException

            ReDim clr(CInt(l) - 1)
            QuickCopyObject(Of Date())(clr, operand, CUInt(l * 8))
            Return clr
        End Operator


#End Region '' Color, Decimal, and Date CTypes

        '' These are the normal (canonical) memory allocation functions.
#Region "Manually-Called Memory Functions"

        ''' <summary>
        ''' Set all bytes in the buffer to zero at the optional index with the optional length.
        ''' </summary>
        ''' <param name="index">Start position of the buffer to zero, default is 0.</param>
        ''' <param name="length">Size of the buffer to zero.  Default is to the end of the buffer.</param>
        ''' <remarks></remarks>
        Public Sub ZeroMemory(Optional index As Long = -1, Optional length As Long = -1)
            Dim bl As Long = Me.Length
            If bl <= 0 Then Return

            Dim p As Long = If(index = -1, 0, index)
            Dim l As Long = If(length = -1, bl - p, length)

            '' The length cannot be greater than the buffer length.
            If l <= 0 OrElse (p + l) > bl Then Return

            Dim ptr = _ptr + p

            If l And &HFFFFFFFF00000000 Then
                n_memset(_ptr, 0, l)
            Else
                MemSet(_ptr, 0, l)
            End If
        End Sub

        ''' <summary>
        ''' Allocate a block of memory on a heap (typically the process heap).  
        ''' </summary>
        ''' <param name="size">The size to attempt to allocate</param>
        ''' <param name="addPressure">Whether or not to call GC.AddMemoryPressure</param>
        ''' <param name="hHeap">
        ''' Optional handle to an alternate heap.  The process heap is used if this is set to null.
        ''' If you use an alternate heap handle, you will need to free the memory using the same heap handle or an error will occur.
        ''' </param>
        ''' <param name="zeroMem">Whether or not to zero the contents of the memory on allocation.</param>
        ''' <returns>True if successful. If False, call GetLastError or FormatLastError to find out more information.</returns>
        ''' <remarks></remarks>
        Public Function Alloc(size As Long, Optional addPressure As Boolean = False, Optional hHeap As IntPtr? = Nothing, Optional zeroMem As Boolean = True) As Boolean
            Dim l As Long = Length()

            If hHeap Is Nothing Then hHeap = _procHeap

            '' While the function doesn't need to call HeapAlloc, it hasn't necessarily failed, either.
            If size = l Then
                Alloc = True
                Exit Function
            End If

            If l > 0 Then
                '' we already have a pointer, so we will call realloc, instead.
                Alloc = ReAlloc(size)
                Exit Function
            End If

            _ptr = HeapAlloc(hHeap, If(zeroMem, 8, 0), CIntPtr(size))
            Alloc = _ptr <> 0

            '' see if we need to tell the garbage collector anything.
            If Alloc AndAlso addPressure Then
                GC.AddMemoryPressure(Length)
            End If

        End Function

        Public Function AllocCoTaskMem(size As Integer) As Boolean
            _ptr = Marshal.AllocCoTaskMem(size)
            Return _ptr <> 0
        End Function

        ''' <summary>
        ''' Allocate a block of memory on the process heap.  
        ''' </summary>
        ''' <param name="size">The size to attempt to allocate</param>
        ''' <param name="addPressure">Whether or not to call GC.AddMemoryPressure</param>
        ''' <returns>True if successful. If False, call GetLastError or FormatLastError to find out more information.</returns>
        ''' <remarks></remarks>
        Public Function Alloc(size As Long, addPressure As Boolean) As Boolean
            Return Alloc(size, addPressure, Nothing, True)
        End Function


        ''' <summary>
        ''' Allocate a block of memory on the process heap.  
        ''' </summary>
        ''' <param name="size">The size to attempt to allocate</param>
        ''' <returns>True if successful. If False, call GetLastError or FormatLastError to find out more information.</returns>
        ''' <remarks></remarks>
        Public Function Alloc(size As Long) As Boolean
            Return Alloc(size, False, Nothing, True)
        End Function

        ''' <summary>
        ''' (Deprecated) Allocate a block of memory and set its contents to zero.
        ''' </summary>
        ''' <param name="size">The size to attempt to allocate</param>
        ''' <param name="addPressure">Whether or not to call GC.AddMemoryPressure</param>
        ''' <param name="hHeap">
        ''' Optional handle to an alternate heap.  The process heap is used if this is set to null.
        ''' If you use an alternate heap handle, you will need to free the memory using the same heap handle or an error will occur.
        ''' </param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function AllocZero(size As Long, Optional addPressure As Boolean = False, Optional hHeap As IntPtr? = Nothing) As Boolean
            AllocZero = Alloc(size, addPressure, hHeap, True)
        End Function


        ''' <summary>
        ''' Allocates memory aligned to a particular byte boundary.
        ''' Memory allocated in this way must be freed with AlignedFree()
        ''' </summary>
        ''' <param name="size">Size of the memory to allocate.</param>
        ''' <param name="alignment">The byte alignment of the memory.</param>
        ''' <param name="addPressure">Specify whether or not to add memory pressure to the garbage collector.</param>
        ''' <param name="hHeap">
        ''' Optional handle to an alternate heap.  The process heap is used if this is set to null.
        ''' If you use an alternate heap handle, you will need to free the memory using the same heap handle or an error will occur.
        ''' </param>
        ''' <returns></returns>
        Public Function AlignedAlloc(size As Long, Optional alignment As Long = 512, Optional addPressure As Boolean = False, Optional hHeap As IntPtr? = Nothing) As Boolean

            If (alignment = 0) OrElse (alignment And 1) <> 0 Then
                Return False
            End If

            If (_ptr <> 0) Then If Not Free() Then Return False

            Dim l As Long = size + (alignment - 1) + 8
            If hHeap Is Nothing Then hHeap = _procHeap

            If (l < 1) Then Return False

            Dim p As IntPtr = HeapAlloc(CType(hHeap, IntPtr), 8, CType(l, IntPtr))

            If (p = 0) Then Return False

            Dim p2 As IntPtr = CType(p.ToInt64 + ((alignment - 1) + 8), IntPtr)

            If p = 0 Then Return False

            p2 = CType(p2.ToInt64 - (p2.ToInt64 Mod alignment), IntPtr)

            Dim mm As MemPtr = p2

            mm.LongAt(-1) = p.ToInt64
            _ptr = p2

            If (addPressure) Then
                GC.AddMemoryPressure(l)
            End If

            Return True

        End Function

        ''' <summary>
        ''' Frees a previously allocated block of aligned memory.
        ''' </summary>
        ''' <param name="removePressure">Specify whether or not to remove memory pressure from the garbage collector.</param>
        ''' <param name="hHeap">
        ''' Optional handle to an alternate heap.  The process heap is used if this is set to null.
        ''' If you use an alternate heap handle, you will need to free the memory using the same heap handle or an error will occur.
        ''' </param>
        ''' <returns></returns>
        Public Function AlignedFree(Optional removePressure As Boolean = False, Optional hHeap As IntPtr? = Nothing) As Boolean

            If (_ptr = 0) Then Return True
            If hHeap Is Nothing Then hHeap = _procHeap

            Dim p As IntPtr = CType(LongAt(-1), IntPtr)
            Dim l As Long = CType(HeapSize(CType(hHeap, IntPtr), 0, p), Long)

            If HeapFree(CType(hHeap, IntPtr), 0, p) <> 0 Then
                If (removePressure) Then
                    GC.RemoveMemoryPressure(l)
                End If

                _ptr = 0
                Return True

            Else
                Return False
            End If

        End Function


        ''' <summary>
        ''' Reallocate a block of memory to a different size on the task heap.
        ''' </summary>
        ''' <param name="size">The size to attempt to allocate</param>
        ''' <param name="modifyPressure">Whether or not to call GC.AddMemoryPressure or GC.RemoveMemoryPressure.</param>
        ''' <param name="hHeap">
        ''' Optional handle to an alternate heap.  The process heap is used if this is set to null.
        ''' </param>
        ''' <returns>True if successful. If False, call GetLastError or FormatLastError to find out more information.</returns>
        ''' <remarks></remarks>
        Public Function ReAlloc(size As Long, Optional modifyPressure As Boolean = False, Optional hHeap As IntPtr? = Nothing) As Boolean
            Dim l As Long = Length()

            If hHeap Is Nothing Then hHeap = _procHeap

            '' While the function doesn't need to call HeapReAlloc, it hasn't necessarily failed, either.
            If size = l Then
                ReAlloc = True
                Exit Function
            End If

            If l <= 0 Then
                '' we don't have a pointer yet, so we have to call alloc instead.
                ReAlloc = Alloc(size)
                Exit Function
            End If

            _ptr = HeapReAlloc(CType(hHeap, IntPtr), 8, _ptr, New IntPtr(size))
            ReAlloc = _ptr <> 0

            '' see if we need to tell the garbage collector anything.
            If ReAlloc AndAlso modifyPressure Then
                size = Length()
                If size < l Then
                    GC.RemoveMemoryPressure(l - size)
                Else
                    GC.AddMemoryPressure(size - l)
                End If
            End If

        End Function

        ''' <summary>
        ''' Frees a previously allocated block of memory on the task heap.
        ''' </summary>
        ''' <returns>True if successful. If False, call GetLastError or FormatLastError to find out more information.</returns>
        ''' <param name="removePressure">Whether or not to call GC.RemoveMemoryPressure</param>
        ''' <param name="hHeap">
        ''' Optional handle to an alternate heap.  The process heap is used if this is set to null.
        ''' The handle pointed to by the internal pointer must have been previously allocated with the same heap handle.
        ''' </param>
        ''' <remarks></remarks>
        Public Function Free(Optional removePressure As Boolean = False, Optional hHeap As IntPtr? = Nothing) As Boolean
            If hHeap Is Nothing Then hHeap = _procHeap
            Dim l As Long

            '' While the function doesn't need to call HeapFree, it hasn't necessarily failed, either.
            If _ptr = 0 Then
                Free = True
            Else
                '' see if we need to tell the garbage collector anything.
                If removePressure Then l = Length()

                Free = HeapFree(CType(hHeap, IntPtr), 0, _ptr) <> 0

                '' see if we need to tell the garbage collector anything.
                If Free Then
                    _ptr = 0
                    If removePressure Then
                        GC.RemoveMemoryPressure(l)
                    End If
                End If
            End If

        End Function

        ''' <summary>
        ''' Validates whether the pointer referenced by this structure
        ''' points to a valid and accessible block of memory.
        ''' </summary>
        ''' <returns>True if the memory block is valid, or False if the pointer is invalid or zero.</returns>
        ''' <remarks></remarks>
        Public Function Validate() As Boolean
            If _ptr = 0 Then
                Validate = False
                Exit Function
            End If

            Validate = HeapValidate(_procHeap, 0, _ptr)
        End Function

        ''' <summary>
        ''' Frees a previously allocated block of memory on the task heap with LocalFree()
        ''' </summary>
        ''' <returns>True if successful. If False, call GetLastError or FormatLastError to find out more information.</returns>
        ''' <remarks></remarks>
        Public Function LocalFree() As Boolean
            If _ptr = 0 Then
                LocalFree = False
            Else
                _ptr = Native.LocalFree(_ptr)
                LocalFree = _ptr <> 0
            End If
        End Function

        ''' <summary>
        ''' Frees a previously allocated block of memory on the task heap with GlobalFree()
        ''' </summary>
        ''' <returns>True if successful. If False, call GetLastError or FormatLastError to find out more information.</returns>
        ''' <remarks></remarks>
        Public Function GlobalFree() As Boolean
            If _ptr = 0 Then
                GlobalFree = False
            Else
                _ptr = Native.GlobalFree(_ptr)
                GlobalFree = _ptr = 0
            End If
        End Function

        ''' <summary>
        ''' Frees a block of memory previously allocated by COM.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub CoTaskMemFree()
            Marshal.FreeCoTaskMem(_ptr)
        End Sub

#End Region '' Manually-Called Memory Functions

        '' NetApi Memory functions should be used carefully and not within the context
        '' of any scenario when you may accidentally call normal memory management functions
        '' on any region of memory allocated with the network memory functions. 
        '' Be mindful of usage.
        '' Some normal functions such as Length and SetLength cannot be used.
        '' Normal allocation and deallocation functions cannot be used, at all.
        '' NetApi memory is not reallocatable.
        '' The size of a NetApi memory buffer cannot be retrieved.
#Region "Network Memory Functions"

        ''' <summary>
        ''' Allocate a network API compatible memory buffer.
        ''' </summary>
        ''' <param name="size">Size of the buffer to allocate, in bytes.</param>
        ''' <remarks></remarks>
        Public Sub NetAlloc(size As Integer)
            '' just ignore an allocated buffer.
            If _ptr <> 0 Then Return

            NetApiBufferAllocate(size, _ptr)
        End Sub

        ''' <summary>
        ''' Free a network API compatible memory buffer previously allocated with NetAlloc.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub NetFree()
            If _ptr = 0 Then Return
            NetApiBufferFree(_ptr)
        End Sub

#End Region '' Network Memory Functions

        '' Virtual Memory should be used carefully and not within the context
        '' of any scenario when you may accidentally call normal memory management functions
        '' on any region of memory allocated with the Virtual functions. 
        '' Be mindful of usage.
        '' Some normal functions such as Length and SetLength cannot be used (use VirtualLength).
        '' Normal allocation and deallocation functions cannot be used, at all.
        '' Virtual memory is not reallocatable.
#Region "Virtual Memory Allocation Functions"

        ''' <summary>
        ''' Allocates a region of virtual memory.
        ''' </summary>
        ''' <param name="size">The size of the region of memory to allocate.</param>
        ''' <param name="addPressure">Whether to call GC.AddMemoryPressure</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function VirtualAlloc(size As Long, Optional addPressure As Boolean = True) As Boolean
            Dim l As Long

            '' While the function doesn't need to call VirtualAlloc, it hasn't necessarily failed, either.
            If size = l AndAlso _ptr <> 0 Then
                VirtualAlloc = True
                Exit Function
            End If


            _ptr = Native.VirtualAlloc(0, CType(size, IntPtr),
                                       VMemAllocFlags.MEM_COMMIT Or
                                       VMemAllocFlags.MEM_RESERVE,
                                       MemoryProtectionFlags.PAGE_READWRITE)

            VirtualAlloc = _ptr <> 0

            If VirtualAlloc AndAlso addPressure Then
                GC.AddMemoryPressure(VirtualLength)
            End If

        End Function

        ''' <summary>
        ''' Frees a region of memory previously allocated with VirtualAlloc.
        ''' </summary>
        ''' <param name="removePressure">Whether to call GC.RemoveMemoryPressure</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function VirtualFree(Optional removePressure As Boolean = True) As Boolean
            Dim l As Long

            '' While the function doesn't need to call VirtualFree, it hasn't necessarily failed, either.
            If _ptr = 0 Then
                VirtualFree = True
            Else
                '' see if we need to tell the garbage collector anything.
                If removePressure Then l = VirtualLength()

                VirtualFree = Native.VirtualFree(_ptr)

                '' see if we need to tell the garbage collector anything.
                If VirtualFree Then
                    _ptr = 0
                    If removePressure Then GC.RemoveMemoryPressure(l)
                End If
            End If

        End Function

        ''' <summary>
        ''' Returns the size of a region of virtual memory previously allocated with VirtualAlloc.
        ''' </summary>
        ''' <returns>The size of a virtual memory region or zero.</returns>
        ''' <remarks></remarks>
        Public Function VirtualLength() As Long
            If _ptr = 0 Then Return 0

            Dim m As New MEMORY_BASIC_INFORMATION

            If VirtualQuery(_ptr, m, CType(Marshal.SizeOf(m), IntPtr)) <> 0 Then
                Return CType(m.RegionSize, Long)
            End If

            Return 0
        End Function

#End Region '' Virtual Memory Allocation Functions

#Region "IDisposable and ICloneable Implemenation"

        '' In a structure this needs to be called manually.
        ''' <summary>
        ''' Free all unmanaged resources.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Dispose() Implements IDisposable.Dispose
            Free()
        End Sub

        ''' <summary>
        ''' Creates an exact copy of the memory associated with this pointer.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Clone() As Object Implements ICloneable.Clone
            Dim mm As New MemPtr, _
                l As Long = Length()

            If l <= 0 Then
                Clone = mm
                Exit Function
            End If

            mm.Alloc(l)

            If l <= UInt32.MaxValue Then
                MemCpy(mm._ptr, _ptr, CUInt(l))
            Else
                CopyMemory(mm._ptr, _ptr, CType(l, IntPtr))
            End If

            Clone = mm
        End Function

#End Region '' IDisposable and ICloneable Implementation

#Region "Instantiation and ToString"

        Shared Sub New()
            '' initialize the cache of primitive types that are supported
            _buildIntegralCache()
        End Sub

        ''' <summary>
        ''' Initialize a new instance of this structure and allocate a memory block
        ''' of the specified size.
        ''' </summary>
        ''' <param name="size">Size of the memory block to allocate.</param>
        ''' <remarks></remarks>
        Public Sub New(size As Long)
            Alloc(size)
        End Sub

        ''' <summary>
        ''' Initialize a new instance of this structure and allocate a memory block
        ''' of the specified size.
        ''' </summary>
        ''' <param name="size">Size of the memory block to allocate.</param>
        ''' <remarks></remarks>
        Public Sub New(size As Integer)
            Alloc(size)
        End Sub

        ''' <summary>
        ''' Initialize a new instance of this structure with the specified memory pointer.
        ''' </summary>
        ''' <param name="ptr">Pointer to a block of memory.</param>
        ''' <remarks></remarks>
        Public Sub New(ptr As UIntPtr)
            _ptr = ToSigned(ptr)
        End Sub

        ''' <summary>
        ''' Initialize a new instance of this structure with the specified memory pointer.
        ''' </summary>
        ''' <param name="ptr">Pointer to a block of memory.</param>
        ''' <remarks></remarks>
        Public Sub New(ptr As IntPtr)
            _ptr = ptr
        End Sub

        ''' <summary>
        ''' Copies the memory pointer into a string and returns the value.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function ToString() As String
            Return CType(Me, String)
        End Function

#End Region '' Instantiation and ToString

#Region "IEnumerable Implementation"

        Public Function GetEnumerator() As IEnumerator(Of Byte) Implements IEnumerable(Of Byte).GetEnumerator
            GetEnumerator = New MemPtrEnumeratorByte(Me)
        End Function

        Public Function GetEnumerator1() As IEnumerator Implements IEnumerable.GetEnumerator
            GetEnumerator1 = New MemPtrEnumeratorByte(Me)
        End Function

        Public Function GetEnumerator2() As IEnumerator(Of Char) Implements IEnumerable(Of Char).GetEnumerator
            GetEnumerator2 = New MemPtrEnumeratorChar(Me)
        End Function

#End Region '' IEnumerable Implementation

    End Structure '' MemPtr

#Region "Enumerators"

    Public Class MemPtrEnumeratorByte
        Implements IEnumerator(Of Byte)

        Dim mm As MemPtr
        Dim pos As Integer = -1

        Friend Sub New(subj As MemPtr)
            mm = subj
        End Sub

        Public ReadOnly Property Current As Byte Implements IEnumerator(Of Byte).Current
            Get
                Current = mm.ByteAt(pos)
            End Get
        End Property

        Public ReadOnly Property Current1 As Object Implements IEnumerator.Current
            Get
                Current1 = mm.ByteAt(pos)
            End Get
        End Property

        Public Function MoveNext() As Boolean Implements IEnumerator.MoveNext
            pos += 1
            If pos >= mm.Length Then Return False Else Return True
        End Function

        Public Sub Reset() Implements IEnumerator.Reset
            pos = -1
        End Sub

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                mm = Nothing
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

    Public Class MemPtrEnumeratorChar
        Implements IEnumerator(Of Char)

        Dim mm As MemPtr
        Dim pos As Integer = -1

        Friend Sub New(subj As MemPtr)
            mm = subj
        End Sub

        Public ReadOnly Property Current As Char Implements IEnumerator(Of Char).Current
            Get
                Current = mm.CharAt(pos)
            End Get
        End Property

        Public ReadOnly Property Current1 As Object Implements IEnumerator.Current
            Get
                Current1 = mm.CharAt(pos)
            End Get
        End Property

        Public Function MoveNext() As Boolean Implements IEnumerator.MoveNext
            pos += 1
            If pos >= CLng(mm.Length >> 1) Then Return False Else Return True
        End Function

        Public Sub Reset() Implements IEnumerator.Reset
            pos = -1
        End Sub

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                mm = Nothing
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

#End Region '' Enumerators

End Namespace
