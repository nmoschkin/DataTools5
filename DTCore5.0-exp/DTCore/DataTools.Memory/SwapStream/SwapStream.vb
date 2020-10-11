'' ************************************************* ''
'' DataTools Visual Basic Utility Library 
''
'' Module: SwapStream
''         Wraps FileStream around a temporary file.
'' 
'' Copyright (C) 2011-2020 Nathan Moschkin
'' All Rights Reserved
''
'' Licensed Under the Microsoft Public License   
'' ************************************************* ''

Imports System.IO

Namespace Memory

    ''' <summary>
    ''' A stream that uses a randomly-named temporary file in the current user's application data folder as a storage backing.
    ''' The file is deleted when the stream is closed.
    ''' </summary>
    Public Class SwapStream
        Inherits FileStream

        Private _swapFile As String

        <ThreadStatic>
        Private Shared _tFile As String

        ''' <summary>
        ''' Create a new swap file.
        ''' </summary>
        Public Sub New()
            MyBase.New(GetSwapFile(_tFile), FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None)
            _swapFile = _tFile
            _tFile = Nothing
        End Sub

        ''' <summary>
        ''' Create a new swap file and initialize it with the provided data.
        ''' </summary>
        ''' <param name="data">Data to initialize the swap file with.</param>
        ''' <param name="resetSeek">Specifies whether to seek to the beginning of the file after writing the initial data.</param>
        Public Sub New(data As Byte(), Optional resetSeek As Boolean = True)
            Me.New
            Write(data, 0, data.Length)
            If resetSeek Then Seek(0, SeekOrigin.Begin)
        End Sub

        ''' <summary>
        ''' Gets an unused swap file name in the current user's application data folder.
        ''' </summary>
        ''' <param name="refReturn"></param>
        ''' <returns></returns>
        Private Shared Function GetSwapFile(Optional ByRef refReturn As String = Nothing) As String
            Dim s As String
            Dim pth

            pth = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\Local\Temp"
            If Directory.Exists(pth) = False Then
                MkDir(pth)
            End If

            pth &= "\"
            Do
                s = pth & Strings.MBString(Date.UtcNow.Ticks, 62, Strings.PadTypes.Auto) & ".tmp"
            Loop While File.Exists(s)

            refReturn = s
            Return s
        End Function

        ''' <summary>
        ''' Close the stream and delete the swap file.
        ''' </summary>
        Public Overrides Sub Close()
            MyBase.Close()

            If _swapFile IsNot Nothing Then
                Kill(_swapFile)
                _swapFile = Nothing
            End If
        End Sub

        Protected Overrides Sub Dispose(disposing As Boolean)
            MyBase.Dispose(disposing)

            If _swapFile IsNot Nothing Then
                Kill(_swapFile)
                _swapFile = Nothing
            End If
        End Sub

    End Class

End Namespace