'' ************************************************* ''
'' DataTools Visual Basic Utility Library 
''
'' Module: BSTR
''         Length-preambled text string.
'' 
'' Copyright (C) 2011-2020 Nathan Moschkin
'' All Rights Reserved
''
'' Licensed Under the Microsoft Public License   
'' ************************************************* ''

Imports DataTools.Memory.Internal
Imports System.Runtime.InteropServices

Namespace Memory


#Region "BSTR"

    ''' <summary>
    ''' BSTR implementation
    ''' </summary>
    ''' <remarks></remarks>
    Public Class BSTR
        Implements IDisposable

        Friend _ptr As IntPtr
        Friend _preamble As Integer = 4
        Friend _Com As Boolean = False

        Public Overrides Function ToString() As String
            Return _getString()
        End Function

        Public Sub New(ptr As IntPtr)
            _ptr = ptr
        End Sub

        Public Sub New(ptr As IntPtr, fromCom As Boolean)
            _ptr = ptr
            If (fromCom) Then
                _Com = True
                _ptr -= 4
                _preamble = 4
            End If
        End Sub

        Public Sub New()

        End Sub

        Public Property PreambleLength As Integer
            Get
                Return _preamble
            End Get
            Set(value As Integer)
                If _Com Then
                    _preamble = 4
                    Return
                End If

                If (Length <> 0) AndAlso (value < _preamble) AndAlso (Length > _preMax(value)) Then
                    Throw New ArgumentException("Cannot downgrade preamble with long string present.")
                End If

                If _ptr = IntPtr.Zero Then
                    _preamble = value
                    Return
                End If

                Dim tl As Integer = Length
                Dim l As Integer = tl + value
                Dim s As String = Me,
                    pNew As IntPtr

                pNew = HeapAlloc(GetProcessHeap, 0, l)

                CopyMemory(pNew, tl, CIntPtr(value))
                CopyMemory(IntPtr.Add(pNew, value), IntPtr.Add(_ptr, CIntPtr(_preamble)), CIntPtr(tl * 2))
                HeapFree(GetProcessHeap, 0, _ptr)
                _ptr = pNew
                _preamble = value
            End Set
        End Property

        Public Property Com As Boolean
            Get
                Return _Com
            End Get
            Friend Set(value As Boolean)
                If _Com = True Then Return

                _Com = value
                If value Then
                    _preamble = 4
                    If _ptr <> IntPtr.Zero Then
                        _ptr -= 4
                    End If
                Else
                    If _ptr <> IntPtr.Zero Then
                        _ptr += 4
                    End If
                End If
            End Set
        End Property

        Private Function _preMax(val As Integer) As Integer

            Select Case val

                Case 1
                    Return 255

                Case 2
                    Return 32767

                Case 4
                    Return &H7FFFFFFF

                Case Else
                    Return -1L

            End Select

        End Function

        Public ReadOnly Property Handle As IntPtr
            Get
                If (_Com) Then Handle = _ptr + 4 Else Handle = _ptr
            End Get
        End Property

        Public ReadOnly Property Length As Integer
            Get
                If _ptr = IntPtr.Zero Then Return -1
                CopyMemory(Length, _ptr, CIntPtr(_preamble))
            End Get
        End Property

        Public Property Text As String
            Get
                Text = _getString()
            End Get
            Set(value As String)
                _setString(value)
            End Set
        End Property

        Private Sub _setString(value As String)
            If value.Length > _preMax(_preamble) Then
                Throw New ArgumentException("Long string won't fit current preamble.")
            End If

            Dim nl As Integer = (value.Length * 2) + _preamble
            Dim hHeap As IntPtr = GetProcessHeap()

            '' already have a string, let's see what is going on...
            If _ptr <> IntPtr.Zero Then
                '' get the heap buffer size.
                Dim lb As Integer = HeapSize(hHeap, 0, _ptr)

                If value = "" Then
                    ' null string, release all memory and tell the garbage collector
                    HeapFree(hHeap, 0, _ptr)
                    GC.RemoveMemoryPressure(lb)

                    Return
                End If

                '' we're actually reallocating the buffer.
                If lb <> nl Then
                    _ptr = HeapReAlloc(hHeap, 0, _ptr, nl)


                    '' have to check the actual allocated size because it may be different from the calculated length
                    nl = HeapSize(hHeap, 0, _ptr)
                    If nl > lb Then
                        '' tell the garbage collector about the change in size
                        GC.AddMemoryPressure(nl - lb)
                    Else
                        '' tell the garbage collector about the change in size
                        GC.RemoveMemoryPressure(lb - nl)
                    End If
                End If
            Else
                '' allocate a new buffer!
                _ptr = HeapAlloc(hHeap, 0, nl)

                '' have to check the actual allocated size because it may be different from the calculated length
                nl = HeapSize(hHeap, 0, _ptr)

                '' tell the garbage collector about the allocation
                GC.AddMemoryPressure(nl)
            End If

            Dim l As Integer = value.Length
            Dim p As IntPtr = IntPtr.Add(_ptr, CIntPtr(_preamble))

            '' copy the length of the string (in characters) into the preamble
            CopyMemory(_ptr, l, CIntPtr(_preamble))

            '' copy the string into the buffer after the preamble.
            CopyMemory(p, value, CIntPtr(value.Length * 2))
        End Sub

        Private Function _getString() As String
            Dim l As Short
            Dim s As String

            '' get the length of the string out of the preamble.
            CopyMemory(l, _ptr, CIntPtr(_preamble))

            s = New String(ChrW(0), l)
            Dim ptr As IntPtr = IntPtr.Add(_ptr, CIntPtr(_preamble))

            CopyMemory(s, ptr, CIntPtr(l * 2))
            Return s

        End Function

        Public Shared Narrowing Operator CType(operand As BSTR) As IntPtr
            Return operand.Handle
        End Operator

        Public Shared Narrowing Operator CType(operand As IntPtr) As BSTR
            Return New BSTR(operand, True)
        End Operator

        Public Shared Narrowing Operator CType(operand As BSTR) As MemPtr
            Return operand.Handle
        End Operator

        Public Shared Narrowing Operator CType(operand As MemPtr) As BSTR
            Return New BSTR(operand, True)
        End Operator

        Public Shared Narrowing Operator CType(operand As BSTR) As UIntPtr
            Return New UIntPtr(CULng(operand.Handle))
        End Operator

        Public Shared Narrowing Operator CType(operand As UIntPtr) As BSTR
            Return New BSTR(New IntPtr(CType(operand.ToUInt64, Long)), True)
        End Operator

        Public Shared Narrowing Operator CType(operand As String) As BSTR
            Dim b As New BSTR
            b._setString(operand)
            Return b
        End Operator

        Public Shared Narrowing Operator CType(operand As Char()) As BSTR
            Dim b As New BSTR
            b._setString(CStr(operand))
            Return b
        End Operator

        Public Shared Narrowing Operator CType(operand As BSTR) As String
            Return operand._getString()
        End Operator

        Public Shared Narrowing Operator CType(operand As BSTR) As Char()
            Dim l As Short
            Dim ch() As Char

            '' get the size from the preamble
            CopyMemory(l, operand._ptr, CIntPtr(2))

            ReDim ch(l - 1)

            '' get the pointer to memory just past the preamble
            Dim ptr As IntPtr = IntPtr.Add(operand._ptr, CIntPtr(2))

            '' copy the memory into the managed array.
            CopyMemory(ch, ptr, CIntPtr(l * 2))

            '' return the new array
            Return ch
        End Operator

        '' same as above but with bytes.
        Public Shared Narrowing Operator CType(operand As BSTR) As Byte()
            Dim i As Integer = operand.Length,
                b() As Byte
            Dim l As Integer = (i * 2) + operand._preamble

            ReDim b(l - 1)
            CopyMemory(b, operand._ptr, CIntPtr(l))
            Return b
        End Operator

        Public Shared Narrowing Operator CType(operand As Byte()) As BSTR
            Dim bs As New BSTR
            Dim hHeap As IntPtr = GetProcessHeap()

            bs._ptr = HeapAlloc(hHeap, 0, operand.Length)
            CopyMemory(bs._ptr, operand.Length, CIntPtr(bs._preamble))
            CopyMemory(IntPtr.Add(bs._ptr, bs._preamble), operand, CIntPtr(operand.Length))

            '' get the size of the allocated buffer and tell the garbage collector
            Dim bl As Long = HeapSize(hHeap, 0, bs._ptr)
            GC.AddMemoryPressure(bl)

            Return bs
        End Operator

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then

                If _ptr <> IntPtr.Zero Then

                    If _Com Then
                        Marshal.FreeCoTaskMem(_ptr + 4)
                    Else
                        Dim hHeap As IntPtr = GetProcessHeap()
                        '' free up the unmanaged buffer 
                        Dim l As Long = HeapSize(hHeap, 0, _ptr)

                        If HeapFree(hHeap, 0, _ptr) = IntPtr.Zero Then
                            GC.RemoveMemoryPressure(l)
                        End If

                    End If

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