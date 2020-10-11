' Polar coordinate translation
' Copyright (C) 2014 Nathaniel Moschkin

' This is the fourth rewriting of this module since 1999.

Option Explicit On
Option Compare Binary
Option Strict On

Imports System.Drawing
Imports DataTools.ExtendedMath.ColorMath

Public Module pMath

    Public Const vbPolarChar = "φ"
    Public Const vbPiChar = "π"
    Public Const vbDegreeChar = "°"
    Public Const vbRadianConst = (180.0# / 3.1415926535897931#)

    Public Enum PolarCoordinateFormatting
        [Default] = &H0

        UseDegreeSymbol = &H1
        UsePolarSymbol = &H2
        UsePiSymbol = &H4
        UseRadianIndicator = &H8

        UseParenthesis = &H10
        UseBrackets = &H20

        DisplayInRadians = &H40

        Standard = &H1
        Radians = &H4 Or &H8 Or &H40

        StandardScientific = &H2
        RadiansScientific = &H2 Or &H4 Or &H8 Or &H40
    End Enum

End Module

Public Class Polar

    Public Enum ColorWheelShapes
        Point = 1
        Hexagon = 2
    End Enum

    Public Structure ColorWheel
        Public Elements() As ColorWheelElement
        Public Bounds As Rectangle
        Public Bitmap() As Byte

        Public Overloads Function HitTest(x As Integer, y As Integer) As System.Drawing.Color
            For Each e As ColorWheelElement In Elements
                For Each f As Point In e.FillPoints
                    If f.X = x And f.Y = y Then Return e.Color
                Next
            Next

            Return Color.Empty
        End Function

        Public Overloads Function HitTest(pt As Point) As System.Drawing.Color
            Return HitTest(pt.X, pt.Y)
        End Function
    End Structure

    Public Structure ColorWheelElement
        Public Color As System.Drawing.Color
        Public Polar As PolarCoordinates
        Public Center As Point
        Public Bounds As Rectangle
        Public FillPoints() As Point
        Public Shape As ColorWheelShapes
    End Structure

    Public Structure PolarCoordinates

        Public Property Formatting As PolarCoordinateFormatting
        Public Property Rounding As Integer
        Public Property Angle As Double
        Public Property Radius As Double

        Public Sub New(radius As Double, angle As Double)
            Formatting = PolarCoordinateFormatting.Standard
            Rounding = 2
            _Radius = radius
            _Angle = angle
        End Sub

        Public Sub New(p As PolarCoordinates)
            Formatting = PolarCoordinateFormatting.Standard
            Rounding = 2
            _Radius = p.Radius
            _Angle = p.Angle
        End Sub

        Public Overrides Function ToString() As String

            Dim s As String = ""
            Dim a As Double = _Angle,
                r As Double = _Radius

            If (Formatting And PolarCoordinateFormatting.UseParenthesis) = PolarCoordinateFormatting.UseParenthesis Then
                s &= "("
            End If

            If (Formatting And PolarCoordinateFormatting.UseBrackets) = PolarCoordinateFormatting.UseBrackets Then
                s &= "{"
            End If

            s &= Format(Math.Round(r, Rounding)) & ", "

            If (Formatting And PolarCoordinateFormatting.DisplayInRadians) = PolarCoordinateFormatting.DisplayInRadians Then
                a *= vbRadianConst
            End If

            s &= Format(Math.Round(a, Rounding))

            If (Formatting And PolarCoordinateFormatting.UsePiSymbol) = PolarCoordinateFormatting.UsePiSymbol Then
                s &= vbPiChar
            End If

            If (Formatting And PolarCoordinateFormatting.UseDegreeSymbol) = PolarCoordinateFormatting.UseDegreeSymbol Then
                s &= vbDegreeChar
            End If

            If (Formatting And PolarCoordinateFormatting.UsePolarSymbol) = PolarCoordinateFormatting.UsePolarSymbol Then
                s &= vbPolarChar
            End If

            If (Formatting And PolarCoordinateFormatting.UseRadianIndicator) = PolarCoordinateFormatting.UseRadianIndicator Then
                s &= " rad"
            End If

            If (Formatting And PolarCoordinateFormatting.UseBrackets) = PolarCoordinateFormatting.UseBrackets Then
                s &= "}"
            End If

            If (Formatting And PolarCoordinateFormatting.UseParenthesis) = PolarCoordinateFormatting.UseParenthesis Then
                s &= ")"
            End If

            Return s

        End Function

        Public Overloads Shared Function rad(p As PolarCoordinates) As UniPoint
            Return rad(p, New Rectangle(0, 0, CInt(p.Radius * 2) + 1, CInt(p.Radius * 2) + 1))
        End Function

        Public Overloads Shared Function rad(p As PolarCoordinates, rect As UniRect) As UniPoint
            If rect.Width < ((p.Radius * 2) + 1) OrElse rect.Height < ((p.Radius * 2) + 1) Then
                '' fit to rectangle
                p = New PolarCoordinates((Math.Min(rect.Width, rect.Height) / 2) - 1, p.Angle)
            End If

            Dim pt As System.Windows.Point = ToScreenCoordinates(p)

            Dim r As Double = p.Radius,
                x As Double,
                y As Double

            Dim c As Point = New Point(CInt(rect.Width / 2), CInt(rect.Height / 2))

            x = c.X + pt.X
            y = c.Y + pt.Y

            pt = New System.Windows.Point(x - 1, y - 1)
            Return New UniPoint(x + rect.Left, y + rect.Top)

        End Function

        Public Shared Widening Operator CType(operand As PolarCoordinates) As System.Windows.Point
            Return ToScreenCoordinates(operand)
        End Operator

        Public Shared Narrowing Operator CType(operand As System.Windows.Point) As PolarCoordinates
            Return ToPolarCoordinates(operand)
        End Operator

    End Structure

    Public Overloads Shared Function ToScreenCoordinates(p As PolarCoordinates) As System.Windows.Point
        Return ToScreenCoordinates(p.Radius, p.Angle)
    End Function

    Public Overloads Shared Function ToScreenCoordinates(r As Double, a As Double) As System.Windows.Point
        Dim x As Double, _
            y As Double

        Dim radConst As Double = (180.0# / 3.1415926535897931#)

        a /= radConst

        y = r * Math.Cos(a)
        x = r * Math.Sin(a)

        Return New System.Windows.Point(x, -y)

    End Function

    Public Overloads Shared Function ToPolarCoordinates(p As System.Windows.Point) As PolarCoordinates
        Return ToPolarCoordinates(p.X, p.Y)
    End Function

    Public Overloads Shared Function ToPolarCoordinates(x As Double, y As Double) As PolarCoordinates

        Dim r As Double, _
            a As Double

        Dim radConst As Double = (180.0# / 3.1415926535897931#)

        r = Math.Sqrt((x * x) + (y * y))

        '' screen coordinates are funny, had to reverse this.
        a = Math.Atan(x / y)

        a *= radConst
        a = (a - 180.0#)
        If a < 0.0# Then a = 360.0# - a
        If a > 360.0# Then a = a - 360.0#

        Return New PolarCoordinates(r, a)

    End Function

    Public Shared Function InWheel(pt As System.Windows.Point, rad As Double) As Boolean
        pt.X -= rad
        pt.Y -= rad
        Dim p As PolarCoordinates = ToPolarCoordinates(pt)

        Return (p.Radius) <= (rad)

    End Function

End Class
