'' ************************************************* ''
'' DataTools Visual Basic Utility Library 
''
'' Module: LPWSTR
''         Null-terminated text string.
'' 
'' Copyright (C) 2011-2020 Nathan Moschkin
'' All Rights Reserved
''
'' Licensed Under the Microsoft Public License   
'' ************************************************* ''

Imports DataTools.Memory.Internal

Namespace Memory


#Region "LPWSTR"

    ''' <summary>
    ''' LPWSTR implementation
    ''' </summary>
    ''' <remarks></remarks>
    Public Class LPWSTR
        Implements IDisposable

        Friend _ptr As IntPtr

        Public Overrides Function ToString() As String
            Return CType(Me, String)
        End Function

        Public Sub New(ptr As IntPtr)
            _ptr = ptr
        End Sub

        Public Sub New()

        End Sub

        Public Property Text As String
            Get
                Return CType(Me, String)
            End Get
            Set(value As String)
                Dim l As Integer = value.Length + 2

                '' get the process heap.
                Dim hHeap As IntPtr = GetProcessHeap

                If _ptr <> IntPtr.Zero Then
                    '' get the current heap size.
                    Dim bl As Long = HeapSize(hHeap, 0, _ptr)

                    _ptr = HeapReAlloc(hHeap, 0, _ptr, l)

                    '' get the real new heap size
                    Dim bl2 As Long = HeapSize(hHeap, 0, _ptr)

                    '' tell the garbage collector
                    If bl2 > bl2 Then
                        GC.AddMemoryPressure(bl - bl2)
                    Else
                        GC.RemoveMemoryPressure(bl2 - bl)
                    End If
                Else
                    '' allocate a new memory block
                    _ptr = HeapAlloc(hHeap, 0, l)

                    '' tell the garbage collector
                    Dim bl As Long = HeapSize(hHeap, 0, _ptr)
                    GC.AddMemoryPressure(bl)

                End If

                CopyMemory(_ptr, value, CIntPtr(l - 2))
            End Set
        End Property

        Public ReadOnly Property Handle As IntPtr
            Get
                Return _ptr
            End Get
        End Property

        Public ReadOnly Property Length As Integer
            Get
                If _ptr = IntPtr.Zero Then Return -1

                Length = -1
                'Dim hlen As Long = HeapSize(GetProcessHeap, 0, _ptr)
                'If hlen < 0 Then Exit Property

                Dim i As Char = ChrW(0)
                Dim pc As IntPtr = _ptr
                Do
                    CopyMemory(i, pc, CIntPtr(2))
                    pc = IntPtr.Add(pc, 2)
                    Length += 1
                Loop Until i = ChrW(0) 'OrElse Length > hlen

            End Get
        End Property

        Public Shared Narrowing Operator CType(operand As LPWSTR) As IntPtr
            Return operand._ptr
        End Operator

        Public Shared Narrowing Operator CType(operand As IntPtr) As LPWSTR
            Return New LPWSTR(operand)
        End Operator

        Public Shared Narrowing Operator CType(operand As LPWSTR) As UIntPtr
            Return New UIntPtr(CULng(operand._ptr))
        End Operator

        Public Shared Narrowing Operator CType(operand As UIntPtr) As LPWSTR
            Return New LPWSTR(New IntPtr(CType(operand.ToUInt64, Long)))
        End Operator

        Public Shared Narrowing Operator CType(operand As String) As LPWSTR
            Dim mm As MemPtr = CType(operand, MemPtr),
                lpw As New LPWSTR

            lpw._ptr = CType(mm, IntPtr)
            Return lpw
        End Operator

        Public Shared Narrowing Operator CType(operand As Char()) As LPWSTR
            Dim mm As MemPtr = CType(operand, MemPtr),
                lpw As New LPWSTR

            lpw._ptr = CType(mm, IntPtr)
            Return lpw
        End Operator

        Public Shared Narrowing Operator CType(operand As LPWSTR) As String
            Dim mm As New MemPtr(operand._ptr)
            Return CType(mm, String)
        End Operator

        Public Shared Narrowing Operator CType(operand As LPWSTR) As Char()
            Dim mm As New MemPtr(operand._ptr)
            Return CType(mm, Char())
        End Operator

        Public Shared Narrowing Operator CType(operand As LPWSTR) As Byte()
            Dim mm As New MemPtr(operand._ptr)
            Return CType(mm, Byte())
        End Operator

        Public Shared Narrowing Operator CType(operand As Byte()) As LPWSTR
            Dim lpw As New LPWSTR,
                mm As MemPtr = CType(operand, MemPtr)

            lpw._ptr = mm.Handle
            Return lpw
        End Operator

        Public Shared Widening Operator CType(operand As LPWSTR) As BSTR
            Return CType(CType(operand, String), BSTR)
        End Operator

        Public Shared Widening Operator CType(operand As BSTR) As LPWSTR
            Return CType(CType(operand, String), LPWSTR)
        End Operator


#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then

                If _ptr <> IntPtr.Zero Then
                    Dim hHeap As IntPtr = GetProcessHeap
                    '' free up the unmanaged buffer 
                    Dim bl As Long = HeapSize(hHeap, 0, _ptr)

                    If HeapFree(hHeap, 0, _ptr) = IntPtr.Zero Then
                        GC.RemoveMemoryPressure(bl)
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