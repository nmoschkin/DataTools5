'' ************************************************* ''
'' DataTools Visual Basic Utility Library 
''
'' Module: Enumerators for StringBlob
'' 
'' Copyright (C) 2011-2020 Nathan Moschkin
'' All Rights Reserved
''
'' Licensed Under the Microsoft Public License   
'' ************************************************* ''

Imports DataTools.Memory.Internal

Namespace Memory

#Region "IEnumerator (StringBlob)"

    Public Class StringBlobEnumerator
        Implements IEnumerator(Of String)

        Dim _Sb As StringBlob
        Dim pos As Integer = -1
        Dim c As Integer = 0
        Dim ptr As IntPtr
        Dim ep As IntPtr

        Dim sd As Integer

        Public Function AllStrings() As String()

            Dim ch As Char = ChrW(0)
            Dim p2 As MemPtr = ptr
            Dim s() As String

            Dim c As Integer = _Sb.Count - 1
            ReDim s(c)

            Dim a As ULong

            If _Sb.LpWStr Then
                For j = 0 To c
                    s(j) = p2.GrabString(0)
                    p2 += (s(j).Length * 2) + 2
                Next
            Else

                For j = 0 To c

                    Select Case sd

                        Case 2
                            a = p2.UShortAt(0)

                        Case 4
                            a = p2.UIntegerAt(0)

                        Case 8
                            a = p2.ULongAt(0)
                    End Select

                    s(j) = p2.GrabString(sd, a)
                    p2 += CLng((a * 2) + sd)

                Next
            End If

            AllStrings = s
        End Function

        Public Sub New(subj As StringBlob)
            _Sb = subj
            ptr = _Sb.SafePtr.handle
            sd = _Sb.SizeDescriptorLength
            If _Sb.LpWStr = False Then
                ptr += _Sb.SizeDescriptorLength
            End If

            ep = ptr
            c = _Sb.Count
        End Sub

        Public ReadOnly Property Current As String Implements IEnumerator(Of String).Current
            Get
                Dim ch As Char = ChrW(0)
                Dim p2 As MemPtr = ep
                Dim a As ULong

                If _Sb.LpWStr Then
                    Current = p2.GrabString(0)
                    ep = p2 + (Current.Length * 2) + 2
                Else
                    Select Case sd

                        Case 2
                            a = p2.UShortAt(0)

                        Case 4
                            a = p2.UIntegerAt(0)

                        Case 8
                            a = p2.ULongAt(0)
                    End Select

                    Current = p2.GrabString(sd, a)
                    ep = p2 + CLng((a * 2) + sd)
                End If

            End Get
        End Property

        Public ReadOnly Property Current1 As Object Implements IEnumerator.Current
            Get
                Return Current
            End Get
        End Property

        Public Function MoveNext() As Boolean Implements IEnumerator.MoveNext

            ptr = ep
            If CLng(ptr - _Sb.Handle) >= _Sb.SafePtr.Length Then Return False
            Return True

        End Function

        Public Sub Reset() Implements IEnumerator.Reset
            pos = -1
            ptr = _Sb.Handle
            sd = _Sb.SizeDescriptorLength
            ep = ptr
        End Sub

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
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

    End Class

#End Region



End Namespace