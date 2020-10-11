'' ************************************************* ''
'' DataTools Visual Basic Utility Library 
''
'' Module: SafePtr 
''         Exhaustive in-place replacement
''         for IntPtr, safe version.
'' 
'' Copyright (C) Nathan Moschkin
'' All Rights Reserved
''
'' Licensed Under the Microsoft Public License   
'' ************************************************* ''

Option Explicit On
Option Compare Binary
Option Strict Off

Imports DataTools.BitStream
Imports System.Runtime.InteropServices
Imports System.Drawing
Imports DataTools.Memory.Internal
Imports System.Runtime.CompilerServices

Namespace Memory

    ''' <summary>
    ''' The SafePtr class.  Drop-in replacement for IntPtr.
    ''' Use anywhere you use IntPtr.
    ''' 
    ''' Inherits from SafeHandle ... will self-dispose, if necessary.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class SafePtr
        Inherits SafeHandle

        Implements IDisposable,  _
                   ICloneable,  _
                   IEnumerable(Of Byte),  _
                   IEnumerable(Of Char),  _
                   IEquatable(Of SafePtr),  _
                   IEquatable(Of IntPtr),  _
                   IEquatable(Of MemPtr)

        '' A List Key/Value pairs of Integral types and their atomic sizes (in bytes).
        Protected Shared _primitiveCache As New List(Of KeyValuePair(Of System.Type, Integer))

        Protected hHeap As IntPtr = GetProcessHeap
        Protected _MemType As MemAllocType = MemAllocType.Invalid
        Protected _buffer As NativeInt

        Friend Shadows Property handle As IntPtr
            Get
                Return MyBase.handle
            End Get
            Set(value As IntPtr)
                MyBase.handle = value
            End Set
        End Property


#Region "Copying"

        ''' <summary>
        ''' Copies memory from this memory pointer into the pointer specified.
        ''' </summary>
        ''' <param name="ptr">The pointer to which to copy the memory.</param>
        ''' <param name="size">The size of the buffer to copy.</param>
        ''' <remarks></remarks>
        Public Sub CopyTo(ptr As IntPtr, size As IntPtr)
            If size.ToInt64 <= UInt32.MaxValue Then
                MemCpy(ptr, handle, CUInt(size))
            Else
                Native.CopyMemory(ptr, handle, size)
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
            If handle <> IntPtr.Zero OrElse Alloc(size.ToInt64) Then
                If size.ToInt64 <= UInt32.MaxValue Then
                    MemCpy(handle, ptr, CUInt(size))
                Else
                    Native.CopyMemory(handle, ptr, size)
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
            Native.CopyMemory(dest, src, CType(length, IntPtr))
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
            If handle = IntPtr.Zero Then Return Nothing
            If length <= 0 Then Throw New IndexOutOfRangeException("length must be greater than zero")

            GrabString = New String(ChrW(0), length)
            QuickCopyObject(Of String)(GrabString, New IntPtr(CLng(handle) + byteIndex.ToInt64), CType(length << 1, UInteger))

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

            byteArrAtget(ba, New IntPtr(CLng(handle) + CLng(byteIndex)), CUInt(e))
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

            If handle = IntPtr.Zero Then Return Nothing
            If length <= 0 Then Throw New IndexOutOfRangeException("length must be greater than zero")

            Dim ba() As Byte
            ReDim ba(length - 1)

            byteArrAtget(ba, New IntPtr(CLng(handle) + CLng(byteIndex)), CUInt(length))
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
            If handle = IntPtr.Zero Then Return Nothing

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

            Dim p As New IntPtr(CLng(handle) + byteIndex.ToInt64)
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
            If handle = IntPtr.Zero Then Return

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
            If handle = IntPtr.Zero Then Return

            If data Is Nothing Then
                Throw New ArgumentNullException("data cannot be null or Nothing.")
            ElseIf length + arrayIndex > data.Length Then
                Throw New ArgumentOutOfRangeException("data buffer length is too small.")
            End If

            Dim gh As GCHandle = GCHandle.Alloc(data, GCHandleType.Pinned)
            Dim pdest As IntPtr = CType(CLng(gh.AddrOfPinnedObject) + arrayIndex, IntPtr)

            MemCpy(pdest, New IntPtr(CLng(handle) + CLng(byteIndex)), CUInt(length))
            gh.Free()
        End Sub

        ''' <summary>
        ''' Returns the results of the buffer as if it were a BSTR type String.
        ''' </summary>
        ''' <param name="comPtr">Specifies whether or not the current MemPtr is an actual COM pointer to a BSTR.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function BSTR(Optional comPtr As Boolean = True) As String
            Dim d As Integer, _
                s As String

            Dim p As IntPtr = If(comPtr, CType(CLng(handle) - 4, IntPtr), handle)

            intAtget(d, p)
            s = New String(ChrW(0), d)
            QuickCopyObject(Of String)(s, CType(CLng(p) + 4, IntPtr), CUInt(d << 1))
            BSTR = s
        End Function

        ''' <summary>
        ''' Returns the contents of this buffer as a string.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function LpwStr() As String
            LpwStr = GrabString(IntPtr.Zero)
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

        Public Overridable Property IsString As Boolean

        ''' <summary>
        ''' Gets or sets the length of the simple buffer.
        ''' If the buffer is already allocated in a certain mode,
        ''' that mode is retained.  Newly allocated memory is automatically
        ''' zeroed-out.  
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Property Length As Long
            Get

                Return _buffer
            End Get
            Set(value As Long)
                Alloc(value)
            End Set
        End Property


        ''' <summary>
        ''' Sets the length of the memory block.
        ''' </summary>
        ''' <param name="value"></param>
        ''' <remarks></remarks>
        Public Overridable Sub SetLength(value As Long)
            If hHeap = 0 Then hHeap = GetProcessHeap
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
            If (handle = IntPtr.Zero) Then Alloc(cb)
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
            ToStructAt = CType(Marshal.PtrToStructure(CType(CLng(handle) + CLng(byteIndex), IntPtr), GetType(T)), T)
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
            Marshal.StructureToPtr(val, CType(CLng(handle) + CLng(byteIndex), IntPtr), False)
        End Sub

        ''' <summary>
        ''' Copies the contents of the buffer at the specified index into a blittable structure array.
        ''' </summary>
        ''' <typeparam name="T">The structure type.</typeparam>
        ''' <param name="byteIndex">The index at which to begin copying.</param>
        ''' <returns>An array of T.</returns>
        ''' <remarks></remarks>
        Public Overridable Function ToBlittableStructArrayAt(Of T As Structure)(byteIndex As IntPtr) As T()

            If (handle = IntPtr.Zero) Then Return Nothing

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

            If (handle = IntPtr.Zero) AndAlso (byteIndex <> IntPtr.Zero) Then Return

            Dim l As Long
            Dim cb = Marshal.SizeOf(New T)
            Dim c As Integer = value.Count

            l = c * cb

            If (handle = IntPtr.Zero) Then
                If Not Alloc(l) Then Return
            End If

            Dim p = CType(CLng(handle) + CLng(byteIndex), IntPtr)

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
            Return ToBlittableStructArrayAt(Of T)(IntPtr.Zero)
        End Function

        ''' <summary>
        ''' Copies a blittable structure array into the buffer, initializing a new buffer, if necessary.
        ''' </summary>
        ''' <typeparam name="T">The structure type.</typeparam>
        ''' <param name="value">The structure array to copy.</param>
        ''' <remarks></remarks>
        Public Overridable Sub FromBlittableStructArray(Of T As Structure)(value As T())
            FromBlittableStructArrayAt(Of T)(IntPtr.Zero, value)
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
                'If handle = IntPtr.Zero Then Return Guid.Empty
                'guidAtget(GuidAtAbsolute, CType(CLng(handle) + index, IntPtr))
            End Get
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Set(value As Guid)
                'If handle = IntPtr.Zero Then Return
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
                'If handle = IntPtr.Zero Then Return Guid.Empty
                'guidAtget(GuidAt, CType(CLng(handle) + (index * 16), IntPtr))
            End Get
            <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
            Set(value As Guid)
                'If handle = IntPtr.Zero Then Return
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
        Default Public Property ByteAt(index As Long) As Byte
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
        Public Overridable Function Reverse() As Boolean
            If (handle = IntPtr.Zero) Then Return False

            Dim l As Long = Length()
            If l > Int32.MaxValue Then Return False

            Dim b1() As Byte
            Dim b2() As Byte

            Dim i As Integer = 0, _
                c As Integer = CInt(l) - 1, _
                e As Integer = c

            ReDim b1(c)
            ReDim b2(c)

            byteArrAtget(b1, handle, CUInt(l))

            Do Until i = c
                b2(e) = b1(i)
                e -= 1
                i += 1
            Loop

            byteArrAtset(handle, b2, CUInt(l))

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

            Dim p1 As IntPtr, _
                p2 As IntPtr

            p1 = CType(CLng(handle) + index, IntPtr)
            p2 = CType(CLng(handle) + index + offset, IntPtr)

            Dim a As Long = Math.Abs(offset)

            Dim m As New MemPtr(length), _
                n As New MemPtr(a)

            MemCpy(m.Handle, p1, CUInt(length))
            MemCpy(n.Handle, p2, CUInt(a))

            p1 = CType(CLng(handle) + index + offset + length, IntPtr)
            MemCpy(p1, n.Handle, CUInt(a))
            MemCpy(p2, m.Handle, CUInt(length))

            m.Free()
            n.Free()
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

            If 0 > index OrElse index > (hl - 1) Then
                Throw New IndexOutOfRangeException("Index out of bounds DataTools.Memory.MemPtr.PushOut().")
                Return -1
            End If

            Dim ol As Long = Length() - index
            ReAlloc(hl + amount)
            Slide(index, ol, amount)

            If bytes IsNot Nothing Then
                byteArrAtset(handle + index, bytes, CUInt(amount))
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
            Return PullIn(index << 1, amount * 1)
        End Function

        ''' <summary>
        ''' Extend the buffer from the specified character index.
        ''' </summary>
        ''' <param name="index">The index where expansion begins. The expansion starts at this position.</param>
        ''' <param name="amount">Number of characters to push out.</param>
        ''' <param name="addPressure">Specify whether to notify the garbage collector.</param>
        ''' <remarks></remarks>
        Public Overridable Function PushOutChar(index As Long, amount As Long, Optional chars() As Char = Nothing, Optional addPressure As Boolean = False) As Long
            Return PushOut(index << 1, amount * 1, CType(CType(chars, MemPtr), Byte()))
        End Function

        ''' <summary>
        ''' Parts the string in both directions from index.  
        ''' </summary>
        ''' <param name="index">The index from which to expand.</param>
        ''' <param name="amount">The amount of expansion, in both directions, so the total expansion will be amount * 1.</param>
        ''' <param name="addPressure">Specify whether to notify the garbage collector.</param>
        ''' <remarks></remarks>
        Public Overridable Sub Part(index As Long, amount As Long, Optional addPressure As Boolean = False)
            If handle = IntPtr.Zero Then
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

        Public Shared Operator Mod(operand1 As NativeInt, operand2 As SafePtr) As NativeInt
            Return operand1 Mod operand2.handle
        End Operator

        Public Shared Operator +(operand1 As NativeInt, operand2 As SafePtr) As NativeInt
            Return operand1 + operand2.handle
        End Operator

        Public Shared Operator -(operand1 As NativeInt, operand2 As SafePtr) As NativeInt
            Return operand1 - operand2.handle
        End Operator

        Public Shared Operator *(operand1 As NativeInt, operand2 As SafePtr) As NativeInt
            Return CLng(operand1) * CLng(operand2.handle)
        End Operator

        Public Shared Operator /(operand1 As NativeInt, operand2 As SafePtr) As NativeInt
            Return operand1 / operand2.handle
        End Operator

        Public Shared Operator \(operand1 As NativeInt, operand2 As SafePtr) As NativeInt
            Return operand1 \ operand2.handle
        End Operator

#End Region '' Basic Math For Pointers

#Region "Equality Operators"

        '' SafePtr
        Public Overrides Function Equals(obj As Object) As Boolean
            If TypeOf obj Is SafePtr Then
                Return CType(obj, SafePtr).handle = handle
            ElseIf TypeOf obj Is IntPtr Then
                Return obj = handle
            ElseIf TypeOf obj Is UIntPtr Then
                Return ToSigned(CType(obj, UIntPtr)) = handle
            Else
                Return False
            End If
        End Function

        Public Overloads Function Equals(other As SafePtr) As Boolean Implements IEquatable(Of SafePtr).Equals
            Return other.handle = handle
        End Function

        Public Overloads Function Equals(other As IntPtr) As Boolean Implements IEquatable(Of IntPtr).Equals
            Return other = handle
        End Function

        Public Overloads Function Equals(other As MemPtr) As Boolean Implements IEquatable(Of MemPtr).Equals
            Return other = handle
        End Function

        Public Shared Operator =(v1 As SafePtr, v2 As SafePtr) As Boolean
            Return (CLng(v1.handle) = CLng(v2.handle))
        End Operator

        Public Shared Operator <>(v1 As SafePtr, v2 As SafePtr) As Boolean
            Return (CLng(v1.handle) <> CLng(v2.handle))
        End Operator

        Public Shared Operator <(v1 As SafePtr, v2 As SafePtr) As Boolean
            Return (CLng(v1.handle) < CLng(v2.handle))
        End Operator

        Public Shared Operator >(v1 As SafePtr, v2 As SafePtr) As Boolean
            Return (CLng(v1.handle) > CLng(v2.handle))
        End Operator

        Public Shared Operator <=(v1 As SafePtr, v2 As SafePtr) As Boolean
            Return (CLng(v1.handle) <= CLng(v2.handle))
        End Operator

        Public Shared Operator >=(v1 As SafePtr, v2 As SafePtr) As Boolean
            Return (CLng(v1.handle) >= CLng(v2.handle))
        End Operator

#End Region '' Equality Operators

#Region "CTypes for IntPtr and MemPtr"

        Public Shared Widening Operator CType(operand As SafePtr) As IntPtr
            Return operand.handle
        End Operator

        Public Shared Widening Operator CType(operand As IntPtr) As SafePtr
            Return New SafePtr(operand)
        End Operator

        Public Shared Narrowing Operator CType(operand As MemPtr) As SafePtr
            Return (operand.Handle)
        End Operator

        Public Shared Narrowing Operator CType(operand As SafePtr) As MemPtr
            Return New MemPtr(operand.handle)
        End Operator

        Public Shared Narrowing Operator CType(operand As NativeInt) As SafePtr
            Return New SafePtr(operand, False)
        End Operator

        Public Shared Narrowing Operator CType(operand As SafePtr) As NativeInt
            Return operand.handle
        End Operator


#End Region '' CType IntPtr/UIntPtr

#Region "Integral CTypes"

        '' in
        Public Shared Widening Operator CType(operand As SByte) As SafePtr
            Dim mm As New SafePtr
            If mm.Alloc(1) Then
                mm.SByteAt(0) = operand
            Else
                Throw New InsufficientMemoryException
            End If

            Return mm
        End Operator

        Public Shared Widening Operator CType(operand As Byte) As SafePtr
            Dim mm As New SafePtr
            If mm.Alloc(1) Then
                mm.ByteAt(0) = operand
            Else
                Throw New InsufficientMemoryException
            End If

            Return mm
        End Operator

        Public Shared Widening Operator CType(operand As Short) As SafePtr
            Dim mm As New SafePtr
            If mm.Alloc(2) Then
                mm.ShortAt(0) = operand
            Else
                Throw New InsufficientMemoryException
            End If

            Return mm
        End Operator

        Public Shared Widening Operator CType(operand As UShort) As SafePtr
            Dim mm As New SafePtr
            If mm.Alloc(2) Then
                mm.UShortAt(0) = operand
            Else
                Throw New InsufficientMemoryException
            End If

            Return mm
        End Operator

        Public Shared Widening Operator CType(operand As Integer) As SafePtr
            Dim mm As New SafePtr
            If mm.Alloc(4) Then
                mm.IntegerAt(0) = operand
            Else
                Throw New InsufficientMemoryException
            End If

            Return mm
        End Operator

        Public Shared Widening Operator CType(operand As UInteger) As SafePtr
            Dim mm As New SafePtr
            If mm.Alloc(4) Then
                mm.UIntegerAt(0) = operand
            Else
                Throw New InsufficientMemoryException
            End If

            Return mm
        End Operator

        Public Shared Widening Operator CType(operand As Long) As SafePtr
            Dim mm As New SafePtr
            If mm.Alloc(8) Then
                mm.LongAt(0) = operand
            Else
                Throw New InsufficientMemoryException
            End If

            Return mm
        End Operator

        Public Shared Widening Operator CType(operand As ULong) As SafePtr
            Dim mm As New SafePtr
            If mm.Alloc(8) Then
                mm.ULongAt(0) = operand
            Else
                Throw New InsufficientMemoryException
            End If

            Return mm
        End Operator

        Public Shared Widening Operator CType(operand As Single) As SafePtr
            Dim mm As New SafePtr
            If mm.Alloc(4) Then
                mm.SingleAt(0) = operand
            Else
                Throw New InsufficientMemoryException
            End If

            Return mm
        End Operator

        Public Shared Widening Operator CType(operand As Double) As SafePtr
            Dim mm As New SafePtr
            If mm.Alloc(8) Then
                mm.DoubleAt(0) = operand
            Else
                Throw New InsufficientMemoryException
            End If

            Return mm
        End Operator


        '' out
        Public Shared Widening Operator CType(operand As SafePtr) As SByte
            Return operand.SByteAt(0)
        End Operator

        Public Shared Widening Operator CType(operand As SafePtr) As Byte
            Return operand.ByteAt(0)
        End Operator

        Public Shared Widening Operator CType(operand As SafePtr) As Char
            Return operand.CharAt(0)
        End Operator

        Public Shared Widening Operator CType(operand As SafePtr) As Short
            Return operand.ShortAt(0)
        End Operator

        Public Shared Widening Operator CType(operand As SafePtr) As UShort
            Return operand.UShortAt(0)
        End Operator

        Public Shared Widening Operator CType(operand As SafePtr) As Integer
            Return operand.IntegerAt(0)
        End Operator

        Public Shared Widening Operator CType(operand As SafePtr) As UInteger
            Return operand.UIntegerAt(0)
        End Operator

        Public Shared Widening Operator CType(operand As SafePtr) As Long
            Return operand.LongAt(0)
        End Operator

        Public Shared Widening Operator CType(operand As SafePtr) As ULong
            Return operand.ULongAt(0)
        End Operator

        Public Shared Widening Operator CType(operand As SafePtr) As Single
            Return operand.SingleAt(0)
        End Operator

        Public Shared Widening Operator CType(operand As SafePtr) As Double
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
        Public Shared Widening Operator CType(operand As Byte()) As SafePtr
            Return Nothing
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As SByte()) As SafePtr
            Return Nothing
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As Short()) As SafePtr
            Return Nothing
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As UShort()) As SafePtr
            Return Nothing
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As Integer()) As SafePtr
            Return Nothing
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As UInteger()) As SafePtr
            Return Nothing
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As Long()) As SafePtr
            Return Nothing
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As ULong()) As SafePtr
            Return Nothing
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As Single()) As SafePtr
            Return Nothing
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As Double()) As SafePtr
            Return Nothing
        End Operator

        '' Out
        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As SafePtr) As SByte()
            Return Nothing
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As SafePtr) As Byte()
            Return Nothing
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As SafePtr) As Short()
            Return Nothing
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As SafePtr) As UShort()
            Return Nothing
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As SafePtr) As Integer()
            Return Nothing
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As SafePtr) As UInteger()
            Return Nothing
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As SafePtr) As Long()
            Return Nothing
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As SafePtr) As ULong()
            Return Nothing
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As SafePtr) As Single()
            Return Nothing
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As SafePtr) As Double()
            Return Nothing
        End Operator

#End Region '' Integral Array CTypes

#Region "String and Char CTypes"

        '' Returns a pretty string, based on null-termination logic, instead of
        '' returning every character in the allocated block.
        Public Shared Widening Operator CType(operand As SafePtr) As String
            Return operand.GrabString(0)
        End Operator

        '' We add 2 bytes to give us a proper null-terminated string in memory.
        Public Shared Widening Operator CType(operand As String) As SafePtr
            Dim mm As New SafePtr
            mm.SetString(0, operand)
            Return mm

            'Dim i As Integer = operand.Length << 1
            'Dim mm As New SafePtr(i + 2)
            'QuickCopyObject(Of String)(mm.Handle, operand, CUInt(i))
            'Return mm
        End Operator

        '' Here we return every character in the allocated block.
        <MethodImpl(MethodImplOptions.ForwardRef)>
        Public Shared Widening Operator CType(operand As SafePtr) As Char()
            Return Nothing
        End Operator

        '' We just set the character information into the memory buffer, verbatim.
        <MethodImpl(MethodImplOptions.ForwardRef)>
        Public Shared Widening Operator CType(operand As Char()) As SafePtr
            Return Nothing
        End Operator

        Public Shared Widening Operator CType(operand As String()) As SafePtr
            If operand Is Nothing OrElse operand.Length = 0 Then Return Nothing

            Dim mm As New SafePtr
            Dim c As Integer = operand.Length - 1
            Dim l As Long = 2

            For i = 0 To c
                l += (operand(i).Length + 1) * 2
            Next

            If Not mm.Alloc(l) Then Return Nothing

            Dim p As IntPtr = mm.handle

            For i = 0 To c
                QuickCopyObject(Of String)(p, operand(i), CType(operand(i).Length * 2, UInteger))
                p += (operand(i).Length * 2) + 2
            Next

            Return mm
        End Operator

        Public Shared Widening Operator CType(operand As SafePtr) As String()
            Return operand.GrabStringArray(0)
        End Operator

#End Region '' String and Char CTypes

#Region "Guid Scalar and Array CTypes"

        Public Shared Narrowing Operator CType(operand As Guid) As SafePtr
            Dim mm As New SafePtr
            If mm.Alloc(16) Then
                mm.GuidAt(0) = operand
            Else
                Throw New OutOfMemoryException
            End If
            Return mm
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Narrowing Operator CType(operand As Guid()) As SafePtr
            Return Nothing
        End Operator

        Public Shared Narrowing Operator CType(operand As SafePtr) As Guid
            Return operand.GuidAt(0)
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Narrowing Operator CType(operand As SafePtr) As Guid()
            Return Nothing
        End Operator

#End Region '' Guid Scalar and Array CTypes

#Region "Color, Decimal, Date"

        Public Shared Widening Operator CType(operand As Decimal) As SafePtr
            Dim mm As New SafePtr
            If mm.Alloc(16) Then
                mm.DecimalAt(0) = operand
            Else
                Throw New OutOfMemoryException
            End If
            Return mm
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As Decimal()) As SafePtr
            Return Nothing
        End Operator

        Public Shared Widening Operator CType(operand As SafePtr) As Decimal
            Return operand.DecimalAt(0)
        End Operator

        <MethodImpl(MethodImplOptions.ForwardRef Or MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(operand As SafePtr) As Decimal()
            Return Nothing
        End Operator


        '' Color

        Public Shared Widening Operator CType(operand As Color) As SafePtr
            Dim mm As New SafePtr
            If mm.Alloc(4) Then
                QuickCopyObject(Of Color)(mm, operand, 4)
            Else
                Throw New OutOfMemoryException
            End If
            Return mm
        End Operator

        Public Shared Widening Operator CType(operand As Color()) As SafePtr
            Dim mm As New SafePtr
            If mm.Alloc(4 * operand.Length) Then
                QuickCopyObject(Of Color())(operand, mm, 4 * operand.Length)
            Else
                Throw New OutOfMemoryException
            End If
            Return mm
        End Operator

        Public Shared Widening Operator CType(operand As SafePtr) As Color
            Dim g As New Color
            QuickCopyObject(Of Color)(g, operand, 4)
            Return g
        End Operator

        Public Shared Widening Operator CType(operand As SafePtr) As Color()

            Dim clr() As Color

            Dim l As Long = operand.Length / 4
            If l > UInt32.MaxValue Then Throw New InvalidCastException

            ReDim clr(l - 1)

            QuickCopyObject(Of Color())(clr, operand, l * 4)
            Return clr
        End Operator


        '' DateTime

        Public Shared Widening Operator CType(operand As Date) As SafePtr
            Dim mm As New SafePtr
            If mm.Alloc(8) Then
                mm.LongAt(0) = operand.ToBinary()
            Else
                Throw New OutOfMemoryException
            End If
            Return mm
        End Operator

        Public Shared Widening Operator CType(operand As Date()) As SafePtr
            Dim mm As New SafePtr
            If mm.Alloc(8 * operand.Length) Then
                QuickCopyObject(Of Date())(operand, mm, 8 * operand.Length)
            Else
                Throw New OutOfMemoryException
            End If
            Return mm
        End Operator

        Public Shared Widening Operator CType(operand As SafePtr) As Date
            Return Date.FromBinary(operand.LongAt(0))
        End Operator

        Public Shared Widening Operator CType(operand As SafePtr) As Date()

            Dim clr() As Date

            Dim l As Long = operand.Length / 8
            If l > UInt32.MaxValue Then Throw New InvalidCastException

            ReDim clr(l - 1)

            QuickCopyObject(Of Date())(clr, operand, l * 8)
            Return clr
        End Operator


#End Region '' Color, Decimal, and Date CTypes

#Region "Allocation and Deallocation"

#Region "Normal Memory"

        ''' <summary>
        ''' Clears the entire object.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overridable Sub Clear()
            Free()
        End Sub

        ''' <summary>
        ''' Allocate a new memory buffer on the process heap.
        ''' </summary>
        ''' <param name="length">Length of the new buffer, in bytes.</param>
        ''' <returns>True if successful.</returns>
        ''' <remarks></remarks>
        Public Overridable Function Alloc(length As Long) As Boolean

            If length = 0 Then Return True

            If handle <> 0 Then
                Return ReAlloc(length)
            End If


            Select Case _MemType

                Case MemAllocType.Virtual

                    If VirtualAlloc(length) Then
                        _buffer = length
                        Return True
                    Else
                        Return False
                    End If

                Case MemAllocType.Network

                    If NetAlloc(length) Then
                        _buffer = length
                        Return True
                    Else
                        Return False
                    End If

                Case Else

                    handle = HeapAlloc(hHeap, 8, length)
                    Alloc = handle <> 0

                    If Alloc Then
                        _buffer = HeapSize(hHeap, 0, handle)
                    End If

            End Select

            If Alloc Then
                _MemType = MemAllocType.Heap
                GC.AddMemoryPressure(_buffer)
            Else
                _buffer = 0
            End If

        End Function

        ''' <summary>
        ''' Reallocate a memory buffer with a new size on the process heap.
        ''' </summary>
        ''' <param name="length">New length of the memory buffer.</param>
        ''' <returns>True if successful.</returns>
        ''' <remarks></remarks>
        Public Overridable Function ReAlloc(length As Long) As Boolean
            If IsInvalid Then Return Alloc(length)

            'If length <= 0 Then
            '    Throw New ArgumentOutOfRangeException("Length cannot be less than or equal to zero.")
            'End If

            Dim l As Long = _buffer

            If length = l Then Return True

            If _MemType = MemAllocType.Heap Then

                Dim p As IntPtr = HeapReAlloc(hHeap, 8, handle, length)

                If p <> 0 Then
                    handle = p
                    _buffer = HeapSize(hHeap, 0, p)

                    ReAlloc = True
                Else
                    ReAlloc = False
                End If

            ElseIf _MemType = MemAllocType.Network Then

                Dim mm As New MemPtr
                mm.NetAlloc(length)

                If mm.Handle = 0 Then
                    ReAlloc = False
                Else
                    _buffer = length
                    GC.AddMemoryPressure(_buffer)
                    MemCpy(mm.Handle, handle, l)

                    NetApiBufferFree(handle)
                    GC.RemoveMemoryPressure(l)

                    handle = mm.Handle
                    _buffer = Me.Length
                    Return True
                End If

            ElseIf _MemType = MemAllocType.Virtual Then

                Dim mm As New MemPtr

                mm.VirtualAlloc(length)

                If mm.Handle = 0 Then
                    ReAlloc = False
                Else
                    _buffer = length

                    MemCpy(mm.Handle, handle, l)
                    Me.VirtualFree()
                    _MemType = MemAllocType.Virtual

                    handle = mm.Handle
                    _buffer = Me.Length
                    Return True
                End If

            ElseIf _MemType = MemAllocType.Com Then

                Dim mm As MemPtr = Marshal.AllocCoTaskMem(length)

                If mm.Handle = 0 Then
                    ReAlloc = False
                Else
                    _buffer = length
                    MemCpy(mm.Handle, handle, l)

                    Marshal.FreeCoTaskMem(handle)
                    handle = mm.Handle

                    ReAlloc = True
                End If
            Else
                Return False
            End If

            If ReAlloc Then
                If IntPtr.Size = 4 Then
                    Dim x As Integer = _buffer
                    If x > l Then
                        GC.AddMemoryPressure(x - l)
                    Else
                        GC.RemoveMemoryPressure(l - x)
                    End If
                Else
                    Dim x As Long = _buffer
                    If x > l Then
                        GC.AddMemoryPressure(x - l)
                    Else
                        GC.RemoveMemoryPressure(l - x)
                    End If

                End If
            End If

        End Function

        ''' <summary>
        ''' Frees the resources allocated by the current object.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function Free() As Boolean
            Free = (handle = 0)

            Select Case _MemType

                Case MemAllocType.Heap
                    Try
                        If HeapValidate(hHeap, 0, handle) = True Then
                            Free = HeapFree(hHeap, 0, handle) <> 0
                        End If
                    Catch ex As Exception

                    End Try

                Case MemAllocType.Virtual
                    Return VirtualFree()

                Case MemAllocType.Network
                    Return NetFree()

                Case MemAllocType.Com

                    Try
                        Marshal.FreeCoTaskMem(handle)
                        Free = True
                    Catch ex As Exception

                    End Try

            End Select

            If Free Then
                GC.RemoveMemoryPressure(_buffer)
                _buffer = 0
                handle = 0
                _MemType = MemAllocType.Invalid
            End If

        End Function



#End Region

#Region "Network Memory"

        ''' <summary>
        ''' Allocate a network API compatible memory buffer.
        ''' </summary>
        ''' <param name="size">Size of the buffer to allocate, in bytes.</param>
        ''' <remarks></remarks>
        Public Overridable Function NetAlloc(size As Integer) As Boolean
            '' just ignore a full buffer.
            If handle <> 0 Then Return False

            NetApiBufferAllocate(size, handle)

            If handle <> 0 Then
                _buffer = size
                NetAlloc = True
                _MemType = MemAllocType.Network
                GC.AddMemoryPressure(_buffer)
            Else
                _buffer = 0
                handle = 0
                NetAlloc = False
                _MemType = MemAllocType.Invalid
            End If
        End Function

        ''' <summary>
        ''' Free a network API compatible memory buffer previously allocated with NetAlloc.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overridable Function NetFree() As Boolean
            If _MemType <> MemAllocType.Network Then Return False
            If handle = 0 Then Return True
            NetFree = NetApiBufferFree(handle) = 0

            If NetFree Then
                GC.RemoveMemoryPressure(_buffer)
                _buffer = 0
                handle = 0
            End If

            _MemType = MemAllocType.Invalid

        End Function

#End Region

#Region "Virtual Memory"

        ''' <summary>
        ''' Allocats a region of virtual memory.
        ''' </summary>
        ''' <param name="size">The size of the region of memory to allocate.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function VirtualAlloc(size As Long) As Boolean
            If handle <> 0 Then Return False


            '' While the function doesn't need to call VirtualAlloc, it hasn't necessarily failed, either.
            If size = 0 Then
                VirtualAlloc = True
                Exit Function
            End If

            Dim l As Long = size
            _buffer = l

            handle = Native.VirtualAlloc(0, l,
                                       VMemAllocFlags.MEM_COMMIT Or
                                       VMemAllocFlags.MEM_RESERVE,
                                       MemoryProtectionFlags.PAGE_READWRITE)

            VirtualAlloc = handle <> 0

            _MemType = MemAllocType.Virtual
            If VirtualAlloc Then
                GC.AddMemoryPressure(_buffer)
            Else
                _buffer = 0
                handle = 0
            End If

        End Function

        ''' <summary>
        ''' Frees a region of memory previously allocated with VirtualAlloc.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function VirtualFree() As Boolean
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

            _MemType = MemAllocType.Invalid

        End Function

        ''' <summary>
        ''' Returns the size of a region of virtual memory previously allocated with VirtualAlloc.
        ''' </summary>
        ''' <returns>The size of a virtual memory region or zero.</returns>
        ''' <remarks></remarks>
        Public Overridable Function VirtualLength() As Long
            If _MemType <> MemAllocType.Virtual Then Return 0
            If handle = 0 Then Return 0

            Dim m As New MEMORY_BASIC_INFORMATION

            If VirtualQuery(handle, m, CType(Marshal.SizeOf(m), IntPtr)) <> 0 Then
                Return CType(m.RegionSize, Long)
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

#Region "ICloneable Implemenation"

        ''' <summary>
        ''' Creates an exact copy of the memory associated with this pointer.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function Clone() As Object Implements ICloneable.Clone
            Dim mm As New SafePtr, _
                l As Long = Length

            If l <= 0 Then
                Clone = mm
                Exit Function
            End If

            Select Case _MemType

                Case MemAllocType.Heap
                    mm.Alloc(l)

                Case MemAllocType.Network
                    mm.NetAlloc(l)

                Case MemAllocType.Virtual
                    mm.VirtualAlloc(l)

                Case MemAllocType.Aligned
                    mm.Alloc(l)

                Case MemAllocType.Com
                    mm.Alloc(l)

                Case MemAllocType.Other, MemAllocType.Invalid
                    mm.Alloc(l)

            End Select

            If l <= UInt32.MaxValue Then
                MemCpy(mm.handle, handle, CUInt(l))
            Else
                CopyMemory(mm.handle, handle, l)
            End If

            Clone = mm
        End Function

#End Region '' ICloneable Implementation

#Region "Instantiation and ToString"

        ''' <summary>
        ''' Initialize a new instance of this structure and allocate a memory block
        ''' of the specified size.
        ''' </summary>
        ''' <param name="size">Size of the memory block to allocate.</param>
        ''' <remarks></remarks>
        Public Sub New(size As Long)
            MyBase.New(CIntPtr(0), True)
            Alloc(size)
        End Sub

        ''' <summary>
        ''' Initialize a new instance of this structure and allocate a memory block
        ''' of the specified size.
        ''' </summary>
        ''' <param name="size">Size of the memory block to allocate.</param>
        ''' <remarks></remarks>
        Public Sub New(size As Integer)
            MyBase.New(CIntPtr(0), True)
            Alloc(size)
        End Sub

        ''' <summary>
        ''' Initialize a new instance of this structure with the specified memory pointer.
        ''' </summary>
        ''' <param name="ptr">Pointer to a block of memory.</param>
        ''' <remarks></remarks>
        Public Sub New(ptr As UIntPtr, Optional fOwn As Boolean = True)
            MyBase.New(CIntPtr(0), True)
            handle = ToSigned(ptr)
        End Sub

        ''' <summary>
        ''' Initialize a new instance of this structure with the specified memory pointer.
        ''' </summary>
        ''' <param name="ptr">Pointer to a block of memory.</param>
        ''' <remarks></remarks>
        Public Sub New(ptr As IntPtr, Optional fOwn As Boolean = True)
            MyBase.New(CIntPtr(0), fOwn)
            handle = ptr
        End Sub

        Public Sub New()
            MyBase.New(CIntPtr(0), True)

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

        Public Overridable Function GetEnumerator() As IEnumerator(Of Byte) Implements IEnumerable(Of Byte).GetEnumerator
            GetEnumerator = New SafePtrEnumeratorByte(Me)
        End Function

        Public Overridable Function GetEnumerator1() As IEnumerator Implements IEnumerable.GetEnumerator
            GetEnumerator1 = New SafePtrEnumeratorByte(Me)
        End Function

        Public Overridable Function GetEnumerator2() As IEnumerator(Of Char) Implements IEnumerable(Of Char).GetEnumerator
            GetEnumerator2 = New SafePtrEnumeratorChar(Me)
        End Function

#End Region '' IEnumerable Implementation

#Region "SafeHandle implementation"

        Public Overrides ReadOnly Property IsInvalid As Boolean
            Get
                Return (handle = 0)
            End Get
        End Property

        Protected Overrides Function ReleaseHandle() As Boolean
            Return Free()
        End Function

#End Region

    End Class '' SafePtr

#Region "Enumerators"

    Public Class SafePtrEnumeratorByte
        Implements IEnumerator(Of Byte)

        Dim mm As SafePtr
        Dim pos As Integer = -1

        Friend Sub New(subj As SafePtr)
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

    Public Class SafePtrEnumeratorChar
        Implements IEnumerator(Of Char)

        Dim mm As SafePtr
        Dim pos As Integer = -1

        Friend Sub New(subj As SafePtr)
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












'		// operator SafePtr to string
'        .method public specialname static string op_Implicit(class DataTools.Memory.SafePtr operand) cil managed 
'        {
'			// THIS IS GOOD
'            .maxstack 8
'			.locals init
'            (
'				int64 lenIn
'            )

'            ldarg.s    0
'            call       instance int64 DataTools.Memory.SafePtr::get_Length()

'			stloc.s    lenIn
'			ldloc.s    lenIn
'			ldc.i8     0
'			ble  NULRET


'			ldarg.s    0

'		    ldc.i4.0
'			conv.i

'        	ldloc.s    lenIn
'			conv.i4
'			ldc.i4.1
'			shr

'            call       instance class string DataTools.Memory.SafePtr::GetString(native int, int32)

'			br		   RET
'		NULRET:
'			ldc.i4.0
'			conv.i

'		RET:
'            ret
'        }


'		// function SafePtr to string
'        .method public instance class string GetString(native int byteIndex, int32 length) cil managed 
'        {
'            .maxstack 8
'            .locals init
'            (
'                string x
'            )

'            ldc.i4.0
'            conv.u2
'            ldarg.s     length
'            newobj      [System.Runtime]System.String::.ctor(char, int32)
'            stloc.s     x
'            ldloc.s     x
'            ldc.i4.0
'            ldelema     [System.Runtime]System.Char

'            ldarg.s     0
'            ldfld       native int [System.Runtime]System.Runtime.InteropServices.SafeHandle::handle
'            ldarg.s     byteIndex
'            add

'            sizeof      native int
'            ldc.i4.4
'            beq INT32

'            ldc.i4.4
'            sub

'INT32:
'    ldarg.s length
'    ldc.i4 0.1
'    shl()
'    conv.u4()

'    cpblk()

'    ldloc.s x
'    ret()

'        }


'		// operator string to SafePtr
'        .method public specialname static class DataTools.Memory.SafePtr op_Implicit(string operand) cil managed 
'        {
'			// THIS IS GOOD
'    .maxstack 3
'    .locals init
'            (
'                class DataTools.Memory.SafePtr mm
'            )
'            newobj       instance void DataTools.Memory.SafePtr::.ctor()

'            stloc.s      0
'            ldloc.s      0

'            ldc.i4.0
'			conv.i
'            ldarg.s      0
'            call instance void DataTools.Memory.SafePtr::SetString(native int, string)
'            ldloc.s      0
'            ret

'        }