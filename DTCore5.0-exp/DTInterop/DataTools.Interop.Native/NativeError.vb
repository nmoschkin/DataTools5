'' ************************************************* ''
'' DataTools Visual Basic Utility Library - Interop
''
'' Module: NativeError
''         GetLastError and related.
''
'' Started in 2000 on Windows 98/ME (and then later 2000).
''
'' Still kicking in 2014 on Windows 8.1!
'' A whole bunch of pInvoke/Const/Declare/Struct and associated utility functions that have been collected over the years.

'' Some enum documentation copied from the MSDN (and in some cases, updated).
'' 
'' Copyright (C) 2011-2020 Nathan Moschkin
'' All Rights Reserved
''
'' Licensed Under the Microsoft Public License   
'' ************************************************* ''


Imports CoreCT.Memory
Imports System.Runtime.InteropServices
Imports DataTools.Interop

Namespace Native

    Public Module NativeErrorMethods

        ''' <summary>
        ''' Format a given system error, or the last system error by default.
        ''' </summary>
        ''' <param name="syserror">Format code to pass. GetLastError is used as by default.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function FormatLastError(Optional syserror As UInteger = 0) As String

            Dim err As UInteger = If(syserror = 0, CUInt(GetLastError), syserror),
                        serr As String = Nothing

            Dim mm As New SafePtr,
                s As String

            mm.Length = 1026
            mm.ZeroMemory()

            FormatMessage(&H1000S, IntPtr.Zero, err, 0, mm.handle, 512, IntPtr.Zero)

            s = ("Error &H" & err.ToString("X8") & ": " & mm.ToString)
            mm.Dispose()

            Return s
        End Function

    End Module

    ''' <summary>
    ''' Throw an exception based on a native Windows system error.
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class NativeException
        Inherits Exception

        Private _Err As Integer

        ''' <summary>
        ''' Instantiate a new exception with a system error value.
        ''' </summary>
        ''' <param name="err"></param>
        ''' <remarks></remarks>
        Public Sub New(err As Integer)
            _Err = err
        End Sub

        ''' <summary>
        ''' Instantiate a new exception with the current system error value.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            _Err = CInt(GetLastError)
        End Sub

        ''' <summary>
        ''' Returns the error message.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides ReadOnly Property Message As String
            Get
                Return "p/Invoke Error: " & _Err & ": " & FormatLastError(CUInt(_Err))
            End Get
        End Property

    End Class

    Public NotInheritable Class NativeError

        ''' <summary>
        ''' Returns the current last native error.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property [Error] As Integer
            Get
                Return CInt(GetLastError)
            End Get
        End Property

        ''' <summary>
        ''' returns the current last native error formatted message.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property Message As String
            Get
                Return FormatLastError()
            End Get
        End Property

    End Class

End Namespace
