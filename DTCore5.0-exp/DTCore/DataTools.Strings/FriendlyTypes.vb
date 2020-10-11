'' ************************************************* ''
'' DataTools Visual Basic Utility Library 
''
'' Module: Friendly numeric types.
'' 
'' Copyright (C) 2011-2020 Nathan Moschkin
'' All Rights Reserved
''
'' Licensed Under the Microsoft Public License   
'' ************************************************* ''

Imports System.Runtime.InteropServices
Imports System.ComponentModel

Namespace Strings

#Region "Friendly Types"

    Public Module TicksAndSecs

        ''' <summary>
        ''' Convert ticks to seconds (divide by 10,000,000 and round)
        ''' </summary>
        ''' <param name="ticks"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function TicksToSecs(ticks As Long) As UInteger
            Dim dd As Double = ticks
            dd /= 10000000
            Return CUInt(dd)
        End Function

        ''' <summary>
        ''' Convert seconds to ticks (multiply by 10,000,000)
        ''' </summary>
        ''' <param name="secs"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SecsToTicks(secs As UInteger) As Long
            Dim dd As Double = secs
            dd *= 10000000
            Return CLng(dd)
        End Function

    End Module

    ''' <summary>
    ''' A friendly-formatting Unix time to CLI TimeSpan marshaling structure.
    ''' </summary>
    ''' <remarks></remarks>
    <StructLayout(LayoutKind.Sequential)>
    Public Structure FriendlySeconds
        Private _value As UInteger

        Public Overloads Overrides Function ToString() As String
            Dim ts As New TimeSpan(0, 0, _value)
            Return ts.ToString()
        End Function

        Public Overloads Function ToString(format As String) As String
            Dim ts As New TimeSpan(0, 0, _value)
            Return ts.ToString(format)
        End Function

        Public Sub New(val As UInteger)
            _value = val
        End Sub

        Public Shared Widening Operator CType(operand As TimeSpan) As FriendlySeconds
            Return New FriendlySeconds(operand.TotalSeconds)
        End Operator

        Public Shared Widening Operator CType(operand As FriendlySeconds) As TimeSpan
            Return New TimeSpan(0, 0, operand._value)
        End Operator

        Public Shared Widening Operator CType(operand As UInteger) As FriendlySeconds
            Return New FriendlySeconds(operand)
        End Operator

        Public Shared Widening Operator CType(operand As FriendlySeconds) As UInteger
            Return operand._value
        End Operator

        Public Shared Widening Operator CType(operand As Integer) As FriendlySeconds
            Return New FriendlySeconds(operand)
        End Operator

        Public Shared Widening Operator CType(operand As FriendlySeconds) As Integer
            Return operand._value
        End Operator

        Public Shared Widening Operator CType(operand As UShort) As FriendlySeconds
            Return New FriendlySeconds(operand)
        End Operator

        Public Shared Widening Operator CType(operand As FriendlySeconds) As UShort
            Return operand._value
        End Operator

        Public Shared Widening Operator CType(operand As Short) As FriendlySeconds
            Return New FriendlySeconds(operand)
        End Operator

        Public Shared Widening Operator CType(operand As FriendlySeconds) As Short
            Return operand._value
        End Operator

        Public Shared Widening Operator CType(operand As ULong) As FriendlySeconds
            Return New FriendlySeconds(operand)
        End Operator

        Public Shared Widening Operator CType(operand As FriendlySeconds) As ULong
            Return operand._value
        End Operator

        Public Shared Widening Operator CType(operand As Long) As FriendlySeconds
            Return New FriendlySeconds(operand)
        End Operator

        Public Shared Widening Operator CType(operand As FriendlySeconds) As Long
            Return operand._value
        End Operator

        Public Shared Widening Operator CType(operand As Byte) As FriendlySeconds
            Return New FriendlySeconds(operand)
        End Operator

        Public Shared Widening Operator CType(operand As FriendlySeconds) As Byte
            Return operand._value
        End Operator

        Public Shared Widening Operator CType(operand As SByte) As FriendlySeconds
            Return New FriendlySeconds(operand)
        End Operator

        Public Shared Widening Operator CType(operand As FriendlySeconds) As SByte
            Return operand._value
        End Operator

    End Structure

    ''' <summary>
    ''' A friendly-formatting Unix time to CLI DateTime marshaling structure.
    ''' </summary>
    ''' <remarks></remarks>
    <StructLayout(LayoutKind.Sequential)>
    Public Structure FriendlyUnixTime
        Private _value As UInteger

        Public Overrides Function ToString() As String
            Return New DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(_value).ToLocalTime.ToString()
        End Function

        Public Overloads Function ToString(format As String) As String
            Return New DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(_value).ToLocalTime.ToString(format)
        End Function

        Public Sub New(val As UInteger)
            _value = val
        End Sub

        Public Shared Widening Operator CType(operand As Date) As FriendlyUnixTime
            Return New FriendlyUnixTime(TicksToSecs(operand.ToUniversalTime.Subtract(New DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).Ticks))
        End Operator

        Public Shared Widening Operator CType(operand As FriendlyUnixTime) As Date
            Return New DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(operand._value).ToLocalTime
        End Operator

        Public Shared Widening Operator CType(operand As UInteger) As FriendlyUnixTime
            Return New FriendlyUnixTime(operand)
        End Operator

        Public Shared Widening Operator CType(operand As FriendlyUnixTime) As UInteger
            Return operand._value
        End Operator

        Public Shared Widening Operator CType(operand As ULong) As FriendlyUnixTime
            Return New FriendlyUnixTime(operand)
        End Operator

        Public Shared Widening Operator CType(operand As FriendlyUnixTime) As ULong
            Return operand._value
        End Operator

        Public Shared Widening Operator CType(operand As Long) As FriendlyUnixTime
            Return New FriendlyUnixTime(operand)
        End Operator

        Public Shared Widening Operator CType(operand As FriendlyUnixTime) As Long
            Return operand._value
        End Operator

        Public Shared Widening Operator CType(operand As FriendlyUnixTime) As String
            Return New DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(operand._value).ToLocalTime.ToString("O")
        End Operator

        Public Shared Narrowing Operator CType(operand As String) As FriendlyUnixTime
            Return New FriendlyUnixTime(TicksToSecs(Date.Parse(operand).ToUniversalTime.Subtract(New DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).Ticks))
        End Operator

        Public Shared Operator +(operand1 As FriendlyUnixTime, operand2 As Short) As FriendlyUnixTime
            operand1._value += operand2
            Return operand1
        End Operator

        Public Shared Operator -(operand1 As FriendlyUnixTime, operand2 As Short) As FriendlyUnixTime
            operand1._value -= operand2
            Return operand1
        End Operator

        Public Shared Operator +(operand1 As FriendlyUnixTime, operand2 As UShort) As FriendlyUnixTime
            operand1._value += operand2
            Return operand1
        End Operator

        Public Shared Operator -(operand1 As FriendlyUnixTime, operand2 As UShort) As FriendlyUnixTime
            operand1._value -= operand2
            Return operand1
        End Operator

        Public Shared Operator +(operand1 As FriendlyUnixTime, operand2 As Integer) As FriendlyUnixTime
            operand1._value += operand2
            Return operand1
        End Operator

        Public Shared Operator -(operand1 As FriendlyUnixTime, operand2 As Integer) As FriendlyUnixTime
            operand1._value -= operand2
            Return operand1
        End Operator

        Public Shared Operator +(operand1 As FriendlyUnixTime, operand2 As UInteger) As FriendlyUnixTime
            operand1._value += operand2
            Return operand1
        End Operator

        Public Shared Operator -(operand1 As FriendlyUnixTime, operand2 As UInteger) As FriendlyUnixTime
            operand1._value -= operand2
            Return operand1
        End Operator

        Public Shared Operator +(operand1 As FriendlyUnixTime, operand2 As Long) As FriendlyUnixTime
            operand1._value += operand2
            Return operand1
        End Operator

        Public Shared Operator -(operand1 As FriendlyUnixTime, operand2 As Long) As FriendlyUnixTime
            operand1._value -= operand2
            Return operand1
        End Operator

        Public Shared Operator +(operand1 As FriendlyUnixTime, operand2 As ULong) As FriendlyUnixTime
            operand1._value += operand2
            Return operand1
        End Operator

        Public Shared Operator -(operand1 As FriendlyUnixTime, operand2 As ULong) As FriendlyUnixTime
            operand1._value -= operand2
            Return operand1
        End Operator

    End Structure

    ''' <summary>
    ''' Stores a number whose default ToString() format is pretty size.
    ''' </summary>
    ''' <remarks></remarks>
    <StructLayout(LayoutKind.Sequential)>
    Public Structure FriendlySizeLong
        Implements INotifyPropertyChanged

        Private _Value As ULong
        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        Public Property Value As ULong
            Get
                Return _Value
            End Get
            Set(value As ULong)
                _Value = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Double"))
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Value"))
            End Set
        End Property

        Public Property DoubleValue As Double
            Get
                Return CDbl(Value)
            End Get
            Set(value As Double)
                Me.Value = CLng(value)
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Double"))
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Value"))
            End Set
        End Property

        Public Sub New(v As Long)
            Value = CULng(v And &H7FFFFFFFFFFFFFFFL)
        End Sub

        Public Sub New(v As ULong)
            Value = v
        End Sub

        Public Shared Widening Operator CType(operand As Long) As FriendlySizeLong
            Return New FriendlySizeLong(operand)
        End Operator

        Public Shared Widening Operator CType(operand As FriendlySizeLong) As Long
            Return CLng(operand.Value And &H7FFFFFFFFFFFFFFFUL)
        End Operator

        Public Shared Widening Operator CType(operand As ULong) As FriendlySizeLong
            Return New FriendlySizeLong(operand)
        End Operator

        Public Shared Widening Operator CType(operand As FriendlySizeLong) As ULong
            Return operand.Value
        End Operator

        Public Shared Narrowing Operator CType(operand As FriendlySizeLong) As String
            Return operand.ToString
        End Operator

        Public Shared Widening Operator CType(operand As Double) As FriendlySizeLong
            Return New FriendlySizeLong With {.DoubleValue = operand}
        End Operator

        Public Shared Widening Operator CType(operand As FriendlySizeLong) As Double
            Return operand.DoubleValue
        End Operator

        Public Overrides Function ToString() As String
            Return PrintFriendlySize(Value)
        End Function
    End Structure

    ''' <summary>
    ''' Stores a number whose default ToString() format is pretty speed.
    ''' </summary>
    ''' <remarks></remarks>
    <StructLayout(LayoutKind.Sequential)>
    Public Structure FriendlySpeedLong
        Implements INotifyPropertyChanged

        Private _Value As ULong
        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        Public Property Value As ULong
            Get
                Return _Value
            End Get
            Set(value As ULong)
                _Value = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Double"))
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Value"))
            End Set
        End Property

        Public Property DoubleValue As Double
            Get
                Return CDbl(Value)
            End Get
            Set(value As Double)
                Me.Value = CLng(value)
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Double"))
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Value"))
            End Set
        End Property

        Public Sub New(v As Long)
            Value = CULng(v And &H7FFFFFFFFFFFFFFFL)
        End Sub

        Public Sub New(v As ULong)
            Value = v
        End Sub

        Public Shared Widening Operator CType(operand As Long) As FriendlySpeedLong
            Return New FriendlySpeedLong(operand)
        End Operator

        Public Shared Widening Operator CType(operand As FriendlySpeedLong) As Long
            Return CLng(operand.Value And &H7FFFFFFFFFFFFFFFUL)
        End Operator

        Public Shared Widening Operator CType(operand As ULong) As FriendlySpeedLong
            Return New FriendlySpeedLong(operand)
        End Operator

        Public Shared Widening Operator CType(operand As FriendlySpeedLong) As ULong
            Return operand.Value
        End Operator

        Public Shared Narrowing Operator CType(operand As FriendlySpeedLong) As String
            Return operand.ToString
        End Operator

        Public Shared Widening Operator CType(operand As Double) As FriendlySpeedLong
            Return New FriendlySpeedLong With {.DoubleValue = operand}
        End Operator

        Public Shared Widening Operator CType(operand As FriendlySpeedLong) As Double
            Return operand.DoubleValue
        End Operator

        Public Overrides Function ToString() As String
            Return PrintFriendlySpeed(Value)
        End Function

    End Structure



    ''' <summary>
    ''' Stores a number whose default ToString() format is pretty size.
    ''' </summary>
    ''' <remarks></remarks>
    <StructLayout(LayoutKind.Sequential)>
    Public Structure FriendlySizeInteger
        Implements INotifyPropertyChanged

        Private _Value As UInteger
        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        Public Property Value As UInteger
            Get
                Return _Value
            End Get
            Set(value As UInteger)
                _Value = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Single"))
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Value"))
            End Set
        End Property

        Public Property SingleValue As Single
            Get
                Return CInt(Value)
            End Get
            Set(value As Single)
                Me.Value = CInt(value)
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Single"))
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Value"))
            End Set
        End Property

        Public Sub New(v As Integer)
            Value = CUInt(v And &H7FFFFFFFI)
        End Sub

        Public Sub New(v As UInteger)
            Value = v
        End Sub

        Public Shared Widening Operator CType(operand As Integer) As FriendlySizeInteger
            Return New FriendlySizeInteger(operand)
        End Operator

        Public Shared Widening Operator CType(operand As FriendlySizeInteger) As Integer
            Return CLng(operand.Value And &H7FFFFFFFUI)
        End Operator

        Public Shared Widening Operator CType(operand As UInteger) As FriendlySizeInteger
            Return New FriendlySizeInteger(operand)
        End Operator

        Public Shared Widening Operator CType(operand As FriendlySizeInteger) As UInteger
            Return operand.Value
        End Operator

        Public Shared Narrowing Operator CType(operand As FriendlySizeInteger) As String
            Return operand.ToString
        End Operator

        Public Shared Widening Operator CType(operand As Single) As FriendlySizeInteger
            Return New FriendlySizeInteger With {.SingleValue = operand}
        End Operator

        Public Shared Widening Operator CType(operand As FriendlySizeInteger) As Single
            Return operand.SingleValue
        End Operator

        Public Overrides Function ToString() As String
            Return PrintFriendlySize(Value)
        End Function
    End Structure

    ''' <summary>
    ''' Stores a number whose default ToString() format is pretty speed.
    ''' </summary>
    ''' <remarks></remarks>
    <StructLayout(LayoutKind.Sequential)>
    Public Structure FriendlySpeedInteger
        Implements INotifyPropertyChanged

        Private _Value As UInteger
        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        Public Property Value As UInteger
            Get
                Return _Value
            End Get
            Set(value As UInteger)
                _Value = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Single"))
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Value"))
            End Set
        End Property

        Public Property SingleValue As Single
            Get
                Return CInt(Value)
            End Get
            Set(value As Single)
                Me.Value = CInt(value)
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Single"))
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Value"))
            End Set
        End Property

        Public Sub New(v As Integer)
            Value = CUInt(v And &H7FFFFFFFI)
        End Sub

        Public Sub New(v As UInteger)
            Value = v
        End Sub

        Public Shared Widening Operator CType(operand As Integer) As FriendlySpeedInteger
            Return New FriendlySpeedInteger(operand)
        End Operator

        Public Shared Widening Operator CType(operand As FriendlySpeedInteger) As Integer
            Return CLng(operand.Value And &H7FFFFFFFUI)
        End Operator

        Public Shared Widening Operator CType(operand As UInteger) As FriendlySpeedInteger
            Return New FriendlySpeedInteger(operand)
        End Operator

        Public Shared Widening Operator CType(operand As FriendlySpeedInteger) As UInteger
            Return operand.Value
        End Operator

        Public Shared Narrowing Operator CType(operand As FriendlySpeedInteger) As String
            Return operand.ToString
        End Operator

        Public Shared Widening Operator CType(operand As Single) As FriendlySpeedInteger
            Return New FriendlySpeedInteger With {.SingleValue = operand}
        End Operator

        Public Shared Widening Operator CType(operand As FriendlySpeedInteger) As Single
            Return operand.SingleValue
        End Operator

        Public Overrides Function ToString() As String
            Return PrintFriendlySpeed(Value)
        End Function

    End Structure


#End Region

End Namespace