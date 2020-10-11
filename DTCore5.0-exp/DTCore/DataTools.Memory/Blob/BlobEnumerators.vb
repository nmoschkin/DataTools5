'' ************************************************* ''
'' DataTools Visual Basic Utility Library 
''
'' Module: Enumerators for Blob
''         
'' Copyright (C) 2011-2020 Nathan Moschkin
'' All Rights Reserved
''
'' Licensed Under the Microsoft Public License   
'' ************************************************* ''

Namespace Memory


#Region "Enumerators"

    Public Class BlobEnumeratorByte
        Implements IEnumerator(Of Byte)

        Dim mm As Blob
        Dim pos As Integer = -1

        Friend Sub New(subj As Blob)
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

    Public Class BlobEnumeratorChar
        Implements IEnumerator(Of Char)

        Dim mm As Blob
        Dim pos As Integer = -1

        Friend Sub New(subj As Blob)
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