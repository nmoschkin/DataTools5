'' ************************************************* ''
'' DataTools Visual Basic Utility Library 
''
'' Module: NativeInt
''         Sign-independent replacement for IntPtr.
'' 
'' Copyright (C) 2011-2020 Nathan Moschkin
'' All Rights Reserved
''
'' Licensed Under the Microsoft Public License   
'' ************************************************* ''

Namespace Memory.Internal

#Region "NativeInt: A Rational Number"

    ''' <summary>
    ''' A small structure to make having a variable-sized buffer less of a pain.
    ''' Represents either a 64-bit or a 32-bit integer.
    ''' </summary>
    ''' <remarks></remarks>
    Public Structure NativeInt
        Private nativeValue As Long

        Public Shared ReadOnly Zero As IntPtr = IntPtr.Zero

        Public Shared ReadOnly Size As Integer = IntPtr.Size

        ''' <summary>
        ''' Initialize this object with an IntPtr
        ''' </summary>
        ''' <param name="value"></param>
        ''' <remarks></remarks>
        Public Sub New(value As IntPtr)
            nativeValue = value
        End Sub

        ''' <summary>
        ''' Tests if this object is equal to another.
        ''' </summary>
        ''' <param name="obj"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function Equals(obj As Object) As Boolean
            Return CType(obj, NativeInt).nativeValue = Me.nativeValue
        End Function

        ''' <summary>
        ''' Converts the number to a signed long.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ToInt64() As Long
            Return nativeValue
        End Function

        ''' <summary>
        ''' Returns a string representation of this number.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function ToString() As String
            Return nativeValue.ToString
        End Function

        ''' <summary>
        ''' Returns the number in the specified format.
        ''' </summary>
        ''' <param name="format"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Function ToString(format As String) As String
            Return nativeValue.ToString(format)
        End Function

        ''' <summary>
        ''' Converts the number to a signed 32.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ToInt32() As Integer
            Dim ul As ULong = ToUnsigned(nativeValue)
            ul = ul And &HFFFFFFFFUL
            Dim us As UInteger = CUInt(ul)
            Return ToSigned(us)
        End Function

        Public Shared Function FromInt32(value As Integer) As NativeInt
            Dim ni As New NativeInt
            Dim us As UInteger = ToUnsigned(value)
            Dim ul As ULong = us
            ni.nativeValue = ToSigned(ul)
            Return ni
        End Function

#Region "Bunches of Operators"

        Public Shared Widening Operator CType(operand As NativeInt) As IntPtr
            If IntPtr.Size = 4 Then
                Return CType(operand.ToInt32, IntPtr)
            Else
                Return CType(operand.ToInt64, IntPtr)
            End If
        End Operator

        Public Shared Widening Operator CType(operand As IntPtr) As NativeInt
            If IntPtr.Size = 8 Then
                Return New NativeInt With {.nativeValue = operand}
            Else
                Return NativeInt.FromInt32(operand.ToInt32)
            End If
        End Operator

        Public Shared Widening Operator CType(operand As NativeInt) As Long
            Return operand.nativeValue
        End Operator

        Public Shared Widening Operator CType(operand As Long) As NativeInt
            Dim n As New NativeInt
            n.nativeValue = operand
            Return n
        End Operator

        Public Shared Narrowing Operator CType(operand As NativeInt) As Short
            Return operand.nativeValue And &HFFFFS
        End Operator

        Public Shared Narrowing Operator CType(operand As Short) As NativeInt
            Return New NativeInt With {.nativeValue = operand}
        End Operator

        Public Shared Narrowing Operator CType(operand As NativeInt) As Integer
            Return operand.ToInt32
        End Operator

        Public Shared Narrowing Operator CType(operand As Integer) As NativeInt
            Return New NativeInt With {.nativeValue = operand}
        End Operator

        Public Shared Widening Operator CType(operand As NativeInt) As Double
            If IntPtr.Size = 4 Then Return CDbl(operand.ToInt32) Else Return CDbl(operand.ToInt64)
        End Operator

        Public Shared Widening Operator CType(operand As Double) As NativeInt
            Dim b As New NativeInt
            b.nativeValue = operand
            Return b
        End Operator

        Public Shared Narrowing Operator CType(operand As NativeInt) As Single
            Return CSng(operand.ToInt32)
        End Operator

        Public Shared Narrowing Operator CType(operand As Single) As NativeInt
            Return New NativeInt With {.nativeValue = CInt(operand)}
        End Operator



        '' bitwise operations.
        Public Shared Operator And(a As NativeInt, b As NativeInt) As Long
            Return (a.ToInt64 And b.ToInt64)
        End Operator

        Public Shared Operator Or(a As NativeInt, b As NativeInt) As Long
            Return (a.ToInt64 Or b.ToInt64)
        End Operator

        Public Shared Operator Xor(a As NativeInt, b As NativeInt) As Long
            Return (a.ToInt64 Xor b.ToInt64)
        End Operator

        Public Shared Operator Not(a As NativeInt) As Long
            Return Not a.ToInt64
        End Operator

        Public Shared Operator Mod(a As NativeInt, b As NativeInt) As Long
            Return a.ToInt64 Mod b.ToInt64
        End Operator

        Public Shared Operator \(a As NativeInt, b As NativeInt) As Long
            Return a.ToInt64 \ b.ToInt64
        End Operator

        Public Shared Operator /(a As NativeInt, b As NativeInt) As Long
            Return a.ToInt64 / b.ToInt64
        End Operator

        Public Shared Operator +(a As NativeInt, b As NativeInt) As Long
            Return a.ToInt64 + b.ToInt64
        End Operator

        Public Shared Operator -(a As NativeInt, b As NativeInt) As Long
            Return a.ToInt64 - b.ToInt64
        End Operator

        Public Shared Operator =(a As NativeInt, b As NativeInt) As Boolean
            Return a.ToInt64 = b.ToInt64
        End Operator

        Public Shared Operator <>(a As NativeInt, b As NativeInt) As Boolean
            Return a.ToInt64 <> b.ToInt64
        End Operator

        Public Shared Operator <(a As NativeInt, b As NativeInt) As Boolean
            Return a.ToInt64 < b.ToInt64
        End Operator

        Public Shared Operator >(a As NativeInt, b As NativeInt) As Boolean
            Return a.ToInt64 > b.ToInt64
        End Operator

        Public Shared Operator <=(a As NativeInt, b As NativeInt) As Boolean
            Return a.ToInt64 <= b.ToInt64
        End Operator

        Public Shared Operator >=(a As NativeInt, b As NativeInt) As Boolean
            Return a.ToInt64 >= b.ToInt64
        End Operator

#End Region

    End Structure

#End Region

End Namespace
