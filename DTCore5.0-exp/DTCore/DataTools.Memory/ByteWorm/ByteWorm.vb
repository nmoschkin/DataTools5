'' ************************************************* ''
'' DataTools Visual Basic Utility Library 
''
'' Module: ByteWorm
''         Divides a stream of bytes into 
''         a virtual collection Of pieces.
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

Namespace Memory

#Region "ByteWorm"

    Public Enum ByteWormStatus As Integer
        OutOfBounds = -1
        None = 0
        NoMore = 1
        More = 2
        Less = 3
        NoLess = 4
        JustOne = 5
    End Enum

    <StructLayout(LayoutKind.Sequential, Pack:=1)> _
    Public Structure WormRecord
        Public Guid As Guid
        Public Length As Integer
        <MarshalAs(UnmanagedType.LPWStr)> _
        Public Data() As Byte


#Region "CType with Blob"

        Public Shared Narrowing Operator CType(operand As Blob) As WormRecord
            Dim w As New WormRecord
            w.Guid = operand.GuidAt(0)
            w.Length = operand.IntegerAtAbsolute(16)

            ReDim w.Data(w.Length - 1)
            w.Data = operand.GrabBytes(20, w.Length)

            Return w
        End Operator

        Public Shared Narrowing Operator CType(operand As WormRecord) As Blob
            Dim bl As New Blob

            bl &= operand.Guid
            bl &= operand.Length
            bl &= operand.Data

            Return bl
        End Operator

#End Region


    End Structure


    ''' <summary>
    ''' Collects a stream of bytes into a virtual collection of pieces. 
    ''' A deliberately light-weight class designed to be used as a player (forward and reverse.)
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class ByteWorm
        Implements IEnumerable(Of Byte()), IDisposable

        '' The stream
        Private mHeap As Blob

        '' The size of the chunks
        Private mSizes() As Integer

        '' The start position of the chunks
        Private mPos() As Integer

        '' the current index
        Private mIdx As Integer = -1

        '' size in virtual elements
        Private mSize As Integer = 0

        '' current status
        Private mStatus As ByteWormStatus = ByteWormStatus.None

#Region "Off State"

        '' the on/off switch, if the switch is off, no commands perform any functions
        '' this is used internally to prevent others from having access while a 
        '' complicated task is being carried out.  Useful for multithreading.
        Private mOff As Boolean = False
        Private mUserOff As Boolean = False

        '' Internally toggle "the big off."  These override any external setting of the Off parameter.
        '' The value will be restored to the user's state when the task is completed.
        Private Sub OffToggle(Optional toggle = Nothing)

            'System.Threading.Monitor.Enter(Off)
            If toggle = Nothing Then mOff = Not mOff Else mOff = CBool(toggle)
            'System.Threading.Monitor.Exit(Off)


        End Sub

#End Region

#Region "Whole-worm operations"

        Public Function GetWorm() As Byte()
            Return mHeap
        End Function

        Public Sub SetWorm(ByRef bytes() As Byte, Optional ByRef sizes() As Integer = Nothing)
            If bytes Is Nothing Then Return
            If Off Then Return Else OffToggle()

            mHeap.SetBytes(0, bytes)
            mHeap.Length = bytes.Length

            OffToggle()

            If sizes IsNot Nothing Then
                Partition(sizes)
            Else
                Partition({mHeap.Length}, ByteWormStatus.JustOne)
            End If

        End Sub

        Public Overloads Sub Partition(sizes() As Integer)
            Partition(sizes, ByteWormStatus.NoMore)
        End Sub


        Friend Overloads Sub Partition(sizes() As Integer, status As ByteWormStatus)
            If sizes Is Nothing Then Return
            If Off Then Return Else OffToggle()

            Dim i As Integer = 0,
                c As Integer = sizes.Length - 1,
                d As Integer = 0,
                e As Integer = 0

            ReDim mPos(c)
            ReDim mSizes(c)

            mSize = 0
            mIdx = -1

            For i = 0 To c
                mPos(i) = e
                mSizes(i) = sizes(i)
                e += sizes(i)
            Next

            mSize = c + 1
            mIdx = c

            mStatus = status
            OffToggle()
        End Sub

        Public Sub Clear()
            If Off Then Return Else OffToggle()

            mHeap.Clear()

            Erase mPos
            Erase mSizes

            mSize = 0
            mIdx = -1

            mStatus = ByteWormStatus.None
            If (Off) Then OffToggle()

        End Sub

#End Region

#Region "Public Properties"

        Default Public ReadOnly Property Item(index As Integer) As Byte()
            Get
                If Off Then Return Nothing Else OffToggle()

                If index < 0 OrElse index >= mSize Then
                    mStatus = ByteWormStatus.OutOfBounds
                    OffToggle()

                    Return Nothing
                End If

                Dim b() As Byte

                b = mHeap.GrabBytes(mPos(index), mSizes(index))
                OffToggle()

                Return b
            End Get
        End Property

        Public ReadOnly Property Piece As Byte()
            Get
                Return Item(mIdx)
            End Get
        End Property

        Public ReadOnly Property Status As ByteWormStatus
            Get
                Return mStatus
            End Get
        End Property

        Public ReadOnly Property Count As Integer
            Get
                Return mSize
            End Get
        End Property

        Public ReadOnly Property BufferLength As Integer
            Get
                Return mHeap.Length
            End Get
        End Property

        Public Property Index As Integer
            Get
                Return mIdx
            End Get
            Set(value As Integer)
                If Off Then Return
                ToIndex(value)
            End Set
        End Property

        Public Property Off As Boolean
            Get
                Return mUserOff Or mOff
            End Get
            Set(value As Boolean)
                mUserOff = value
            End Set
        End Property

#End Region

#Region "Worm Items"

        Public Function Add(value() As Byte) As Integer
            If Off Then Return -1
            '' We don't want empty objects. This is a worm!  It needs pieces! LOL.
            If value Is Nothing Then Return -1
            Dim l As Integer = value.Length

            OffToggle()

            ReDim Preserve mPos(mSize)
            ReDim Preserve mSizes(mSize)

            mPos(mSize) = mHeap.Length
            mSizes(mSize) = l

            mIdx = mSize

            mHeap &= value
            mSize += 1

            mStatus = ByteWormStatus.NoMore

            OffToggle()
            Return mIdx
        End Function

        Public Function Truncate(Optional idx As Integer = -1) As Integer
            If Off Then Return -1
            Dim x As Integer

            If idx = -1 Then
                idx = mIdx
            End If

            If idx > (mSize - 1) Then
                mStatus = ByteWormStatus.OutOfBounds
                Return -1
            ElseIf idx = (mSize - 1) Then
                Return idx
            End If

            OffToggle()

            ReDim Preserve mSizes(idx)
            ReDim Preserve mPos(idx)

            x = mSizes(idx) + mPos(idx)
            mHeap.Length = x

            mSize = idx + 1
            mIdx = idx

            If mSize = 1 Then
                mStatus = ByteWormStatus.JustOne
            Else
                mStatus = ByteWormStatus.NoMore
            End If

            OffToggle()
            Return idx
        End Function

        Public Function Shift() As Integer
            If Off Then Return -1
            If mSize = 0 Then Return -1

            If mSize = 1 Then
                Clear()
                Return -1
            End If

            OffToggle()

            Dim c As Integer

            Dim e As Integer = mSizes(0)

            c = mHeap.Length - e
            Array.Copy(CType(mHeap, Byte()), e, CType(mHeap, Byte()), 0, c)

            mHeap.Length = c

            mSize -= 1
            c = mSize - 1

            Array.Copy(mSizes, 1, mSizes, 0, mSize)
            Array.Copy(mPos, 1, mPos, 0, mSize)

            ReDim Preserve mSizes(c)
            ReDim Preserve mPos(c)

            mIdx = mSize - 1
            OffToggle()

            Return mIdx

        End Function

#End Region

#Region "Traversing"

        Public Function ToIndex(index As Integer, Optional ByRef status As ByteWormStatus = Nothing) As Byte()
            If Off Then Return Nothing

            If index = mIdx Then
                If status <> Nothing Then status = mStatus
                Return Item(mIdx)
            End If

            If (index >= (mSize - 1)) Then
                index = mSize
                mStatus = ByteWormStatus.OutOfBounds
                If status <> Nothing Then status = mStatus
                Return Nothing
            ElseIf (index < 0) Then
                index = -1
                mStatus = ByteWormStatus.OutOfBounds
                If status <> Nothing Then status = mStatus
                Return Nothing
            End If

            If mSize = 1 Then
                mStatus = ByteWormStatus.JustOne
            Else
                If (mIdx < index) Then
                    mStatus = ByteWormStatus.Less
                Else
                    mStatus = ByteWormStatus.More
                End If
            End If

            mIdx = index
            If status <> Nothing Then status = mStatus
            Return Item(index)

        End Function

        Public Function Forward(Optional ByRef status As ByteWormStatus = Nothing) As Byte()
            If Off Then Return Nothing

            mIdx += 1
            If (mIdx >= mSize) Then
                mIdx = mSize - 1
                mStatus = ByteWormStatus.NoMore
                If status <> Nothing Then status = mStatus

                Return Nothing
            End If

            Forward = Item(mIdx)

            If mSize = 1 Then
                mStatus = ByteWormStatus.JustOne
            Else
                mStatus = ByteWormStatus.More
                If status <> Nothing Then status = mStatus
            End If

        End Function

        Public Function Backward(Optional ByRef status As ByteWormStatus = Nothing) As Byte()
            If Off Then Return Nothing

            mIdx -= 1
            If (mIdx < 0) Then
                mIdx = 0
                mStatus = ByteWormStatus.NoLess
                If status <> Nothing Then status = mStatus
                Return Nothing
            End If

            Backward = Item(mIdx)

            If mSize = 1 Then
                mStatus = ByteWormStatus.JustOne
            Else
                mStatus = ByteWormStatus.Less
                If status <> Nothing Then status = mStatus
            End If

        End Function

#End Region

#Region "IEnumerable"

        Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
            If Off Then Return Nothing
            Return New ByteWormEnumerator(Me)
        End Function

        Public Function GetEnumerator1() As System.Collections.Generic.IEnumerator(Of Byte()) Implements System.Collections.Generic.IEnumerable(Of Byte()).GetEnumerator
            If Off Then Return Nothing
            Return New ByteWormEnumerator(Me)
        End Function

#End Region

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If mHeap IsNot Nothing Then _
                    mHeap.Free()

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
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

        Public Sub New()
            mHeap = New Blob
        End Sub

    End Class

    Public Class ByteWormEnumerator
        Implements IEnumerator(Of Byte())

        Dim mIdx As Integer = -1
        Dim mWorm As ByteWorm

        Public Property Worm As ByteWorm
            Get
                Return mWorm
            End Get
            Set(value As ByteWorm)
                mWorm = value
            End Set
        End Property

        Public Sub New(ByRef worm As ByteWorm)
            mWorm = worm
        End Sub

#Region "IEnumerator"

        Public ReadOnly Property Current As Byte() Implements System.Collections.Generic.IEnumerator(Of Byte()).Current
            Get
                Return mWorm(mIdx)
            End Get
        End Property

        Public ReadOnly Property Current1 As Object Implements System.Collections.IEnumerator.Current
            Get
                Return mWorm(mIdx)
            End Get
        End Property

        Public Function MoveNext() As Boolean Implements System.Collections.IEnumerator.MoveNext
            If mIdx >= (mWorm.Count - 1) Then Return False
            mIdx += 1
            Return True
        End Function

        Public Sub Reset() Implements System.Collections.IEnumerator.Reset
            mIdx = -1
        End Sub

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    mWorm = Nothing
                End If

            End If
            Me.disposedValue = True
        End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub

#End Region

#End Region

    End Class

#End Region


End Namespace