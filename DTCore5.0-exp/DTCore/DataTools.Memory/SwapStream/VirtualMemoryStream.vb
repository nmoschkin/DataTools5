Imports System.IO
Imports System.Runtime.InteropServices

Namespace Memory

    Public NotInheritable Class VirtualMemoryStream
        Inherits Stream

        Private _Blob As New Blob With {.InBufferMode = True, .BufferExtend = 65536, .MemoryType = MemAllocType.Virtual}

        Public Overrides ReadOnly Property CanRead As Boolean
            Get
                Return True
            End Get
        End Property

        Public Overrides ReadOnly Property CanSeek As Boolean
            Get
                Return True
            End Get
        End Property

        Public Overrides ReadOnly Property CanWrite As Boolean
            Get
                Return True
            End Get
        End Property

        Public Overrides ReadOnly Property Length As Long
            Get
                Return _Blob.Length
            End Get
        End Property

        Public Overrides Property Position As Long
            Get
                Return _Blob.ClipNext
            End Get
            Set(value As Long)
                _Blob.ClipSeek(value)
            End Set
        End Property

        Public Overrides Sub Flush()
            Return
        End Sub

        Public Overrides Sub SetLength(value As Long)
            _Blob.Length = value
        End Sub

        Public Overrides Sub Write(buffer() As Byte, offset As Integer, count As Integer)

            Dim gch As GCHandle = GCHandle.Alloc(buffer, GCHandleType.Pinned)
            Dim cptr As IntPtr = gch.AddrOfPinnedObject + offset

            If (_Blob.Length - _Blob.ClipNext) < count Then
                _Blob.Length += (count - _Blob.ClipNext)
            End If

            DataTools.Memory.Internal.Native.MemCpy(_Blob.DangerousGetHandle() + _Blob.ClipNext, cptr, count)
            gch.Free()

            _Blob.ClipSeek(_Blob.ClipNext + count)

        End Sub

        Public Overrides Function Read(buffer() As Byte, offset As Integer, count As Integer) As Integer

            Dim gch As GCHandle = GCHandle.Alloc(buffer, GCHandleType.Pinned)
            Dim cptr As IntPtr = gch.AddrOfPinnedObject + offset

            If (_Blob.Length - _Blob.ClipNext) < count Then
                count = _Blob.Length - _Blob.ClipNext
            End If

            If count <= 0 Then Return 0

            Internal.Native.MemCpy(_Blob.DangerousGetHandle() + _Blob.ClipNext, cptr, count)
            _Blob.ClipSeek(_Blob.ClipNext + count)

            Return count
        End Function

        Public Overrides Function Seek(offset As Long, origin As SeekOrigin) As Long

            Select Case origin
                Case SeekOrigin.Begin
                    _Blob.ClipSeek(offset)

                Case SeekOrigin.Current
                    _Blob.ClipSeek(_Blob.ClipNext + offset)

                Case SeekOrigin.End
                    _Blob.ClipSeek(_Blob.Length + offset)

            End Select

            Return _Blob.ClipNext
        End Function

        Protected Overrides Sub Dispose(disposing As Boolean)
            MyBase.Dispose(disposing)

            If disposing Then
                _Blob.Free()
            End If

        End Sub

    End Class



End Namespace
