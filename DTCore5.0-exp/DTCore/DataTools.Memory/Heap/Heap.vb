'' ************************************************* ''
'' DataTools Visual Basic Utility Library 
''
'' Module: Heap
''         Wrapper for system memory heaps.
'' 
'' Copyright (C) 2011-2020 Nathan Moschkin
'' All Rights Reserved
''
'' Licensed Under the Microsoft Public License   
'' ************************************************* ''

Imports System.Runtime.InteropServices
Imports DataTools.Memory.Internal

Namespace Memory

    '' *********************************
    '' *********************************
    '' I N T E R F A C E S
    '' *********************************
    '' *********************************

    Public Interface IMemoryHeap(Of T As IDisposable)

        ''' <summary>
        ''' Returns the underlying pointer of this heap.
        ''' </summary>
        ''' <returns></returns>
        Function DangerousGetHandle() As IntPtr

        ''' <summary>
        ''' Gets a value indicating whether the heap has been destroyed.
        ''' </summary>
        ''' <returns></returns>
        ReadOnly Property IsDestroyed As Boolean

        ''' <summary>
        ''' Returns true if this heap has a maximum size.
        ''' </summary>
        ''' <returns></returns>
        ReadOnly Property HasMaxSize As Boolean

        ''' <summary>
        ''' Returns the maximum size of the heap.
        ''' </summary>
        ''' <returns></returns>
        ReadOnly Property MaxSize As IntPtr

        ''' <summary>
        ''' Returns the initial size of the heap.
        ''' </summary>
        ''' <returns></returns>
        ReadOnly Property InitialSize As IntPtr

        ''' <summary>
        ''' Returns an array of objects created from this heap
        ''' </summary>
        ''' <returns></returns>
        Function GetItems() As T()

        ''' <summary>
        ''' Create a new memory object from this heap.
        ''' </summary>
        ''' <returns></returns>
        Function CreateItem() As T

        ''' <summary>
        ''' Destroys the allocation associated with the object, and calls its Dispose method.
        ''' </summary>
        ''' <param name="item"></param>
        Sub DestroyItem(item As T)

        ''' <summary>
        ''' Destroys the buffer and all of its contents.
        ''' </summary>
        ''' <returns></returns>
        Function DestroyHeap() As Boolean

    End Interface

    '' *********************************
    '' *********************************
    '' C L A S S E S  S T A R T  H E R E
    '' *********************************
    '' *********************************

    '' *********************************
    '' *********************************
    '' BlobHeap
    '' *********************************
    '' *********************************

#Region "Blob Heap"

    ''' <summary>
    ''' Represents a private heap in memory for
    ''' a collection of blobs.
    ''' </summary>
    Public Class BlobHeap
        Inherits SafeHandle
        Implements IMemoryHeap(Of Blob)

        Protected _max As IntPtr
        Protected _init As IntPtr

        Private _isDestroyed As Boolean

        Protected _List As New List(Of Blob)

        Private Shared _def As BlobHeap = Nothing

        ''' <summary>
        ''' Gets or sets the default heap for creating all new instances of Blob in this process.
        ''' </summary>
        ''' <returns></returns>
        Public Shared Property DefaultHeap As BlobHeap
            Get
                Return _def
            End Get
            Set(value As BlobHeap)
                _def = value
                Blob.defaultHeap = _def.DangerousGetHandle
            End Set
        End Property

        ''' <summary>
        ''' Returns the process heap wrapped in a BlobHeap object.
        ''' </summary>
        ''' <returns></returns>
        Public Shared ReadOnly Property ProcessHeap As BlobHeap

        ''' <summary>
        ''' Returns true if this heap has a maximum size.
        ''' </summary>
        ''' <returns></returns>
        Public Overridable ReadOnly Property HasMaxSize As Boolean Implements IMemoryHeap(Of Blob).HasMaxSize
            Get
                Return (_max <> 0)
            End Get
        End Property

        ''' <summary>
        ''' Returns the maximum size of the heap.
        ''' </summary>
        ''' <returns></returns>
        Public Overridable ReadOnly Property MaxSize As IntPtr Implements IMemoryHeap(Of Blob).MaxSize
            Get
                Return _max
            End Get
        End Property

        ''' <summary>
        ''' Returns the initial size of the heap.
        ''' </summary>
        ''' <returns></returns>
        Public Overridable ReadOnly Property InitialSize As IntPtr Implements IMemoryHeap(Of Blob).InitialSize
            Get
                Return _init
            End Get
        End Property

        ''' <summary>
        ''' Returns an array of objects created from this heap
        ''' </summary>
        ''' <returns></returns>
        Public Overridable Function GetMembers() As Blob() Implements IMemoryHeap(Of Blob).GetItems
            Return _List.ToArray
        End Function

        ''' <summary>
        ''' Create a new memory object from this heap.
        ''' </summary>
        ''' <returns></returns>
        Public Function CreateObject() As Blob Implements IMemoryHeap(Of Blob).CreateItem
            Return New Blob With {.activeHeap = Me, .hHeap = handle}
        End Function

        ''' <summary>
        ''' Destroys the object, and calls its IDisposable.Dispose method.
        ''' </summary>
        ''' <param name="bl"></param>
        Public Sub DestroyObject(bl As Blob) Implements IMemoryHeap(Of Blob).DestroyItem
            bl.Dispose()
            If _List.Contains(bl) Then _List.Remove(bl)
        End Sub

        Friend Sub AddSelf(bl As Blob)
            If Not _List.Contains(bl) Then _List.Add(bl)
        End Sub

        Friend Sub RemoveSelf(bl As Blob)
            _List.Remove(bl)
        End Sub

        Public Shadows Function DangerousGetHandle() As IntPtr Implements IMemoryHeap(Of Blob).DangerousGetHandle
            Return handle
        End Function

        ''' <summary>
        ''' Returns a value indicating that the heap is invalid.
        ''' </summary>
        ''' <returns></returns>
        Public Overrides ReadOnly Property IsInvalid As Boolean Implements IMemoryHeap(Of Blob).IsDestroyed
            Get
                IsInvalid = (handle = 0)
            End Get
        End Property

        ''' <summary>
        ''' Release the handle and destroy the heap.
        ''' </summary>
        ''' <returns></returns>
        Protected Overrides Function ReleaseHandle() As Boolean Implements IMemoryHeap(Of Blob).DestroyHeap

            If HeapValidate(handle, 0, 0) Then
                'For Each bl In _List
                '    If Threading.Monitor.TryEnter(bl) Then
                '        bl.Dispose()
                '        Threading.Monitor.Exit(bl)
                '    End If
                'Next

                '' All blobs on this heap will now have invalid pointers, please use with caution.
                _List.Clear()

                HeapDestroy(handle)
                handle = 0

                Return True
            Else
                Return False
            End If
        End Function

        ''' <summary>
        ''' Create a new heap with an initial size and a maximum size.
        ''' </summary>
        ''' <param name="initSize">Initial size, in bytes, of the heap.</param>
        ''' <param name="maxSize">Maximum size, in bytes, of the heap.</param>
        Public Sub New(initSize As IntPtr, maxSize As IntPtr)
            MyBase.New(0, True)

            Dim i As Long = initSize
            Dim ps = SystemInformation.SystemInfo.dwPageSize

            If i < ps Then
                i = ps
            Else
                i = i + (ps - (i Mod ps))
            End If

            initSize = i

            handle = HeapCreate(4, initSize, maxSize)

            Me._init = initSize
            Me._max = maxSize
        End Sub

        ''' <summary>
        ''' Create a new heap with an initial size and an unlimited maximum size.
        ''' </summary>
        ''' <param name="initSize">Initial size, in bytes, of the heap.</param>
        Public Sub New(initSize As IntPtr)
            Me.New(initSize, 0)
        End Sub

        ''' <summary>
        ''' Create a new heap with an initial size of the system page size.
        ''' </summary>
        Public Sub New()
            Me.New(SystemInformation.SystemInfo.dwPageSize, 0)
        End Sub

        ''' <summary>
        ''' Wrap the process heap in a BlobHeap class.
        ''' </summary>
        ''' <param name="a"></param>
        Private Sub New(a As Boolean)
            MyBase.New(0, False)
            handle = GetProcessHeap()
        End Sub

        Shared Sub New()
            '' make the process heap object.
            ProcessHeap = New BlobHeap(True)

            '' set the default heap to the process heap.
            DefaultHeap = ProcessHeap
        End Sub

        Public Overrides Function ToString() As String
            Return handle.ToString
        End Function

        Public Shared Widening Operator CType(operand As BlobHeap) As IntPtr
            Return operand.handle
        End Operator

    End Class

#End Region

End Namespace