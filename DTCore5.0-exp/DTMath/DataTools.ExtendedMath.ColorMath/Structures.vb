
Imports System.Runtime.InteropServices

Namespace ColorMath

    <HideModuleName>
    Public Module ColorStructs

#Region "Color Math Structures"

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure W32POINT
            Public x As Integer
            Public y As Integer

            Public Sub New(x As Integer, y As Integer)
                Me.x = x
                Me.y = y
            End Sub
        End Structure

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure W32SIZE
            Public cx As Integer
            Public cy As Integer

            Public Sub New(cx As Integer, cy As Integer)
                Me.cx = cx
                Me.cy = cy
            End Sub

        End Structure

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure W32RECT
            Public left As Integer
            Public top As Integer
            Public right As Integer
            Public bottom As Integer

            Public Sub New(l As Integer, t As Integer, r As Integer, b As Integer)
                left = l
                top = t
                right = r
                bottom = b
            End Sub

        End Structure



        Public Structure PALETTEINFOEX

            Public FlipHorizontal As Boolean
            Public FlipVertical As Boolean

            Public GrayTones As Boolean

            Public StaticField As Integer
            Public Value As Integer

            Public BlockSize As Integer

        End Structure

        Public Structure RGBDATA
            Public Blue As Byte
            Public Green As Byte
            Public Red As Byte
        End Structure

        Public Structure ARGBDATA
            Public Blue As Byte
            Public Green As Byte
            Public Red As Byte
            Public Alpha As Byte
        End Structure

        Public Structure BGRADATA
            Public Alpha As Byte
            Public Red As Byte
            Public Green As Byte
            Public Blue As Byte
        End Structure

        Public Structure BGRDATA
            Public Red As Byte
            Public Green As Byte
            Public Blue As Byte
        End Structure

        Public Structure CMYDATA
            Public Cyan As Byte
            Public Magenta As Byte
            Public Yellow As Byte
        End Structure

        Public Structure HSVDATA
            Public Hue As Double
            Public Saturation As Double
            Public Value As Double
        End Structure

        Public Structure HSLDATA
            Public Hue As Double
            Public Saturation As Double
            Public Lightness As Double
        End Structure

        Public Structure CARTESEAN2D

            Public Origin As System.Drawing.Point
            Public Points As System.Drawing.Point

            Public Arc As Double
            Public Distance As Double

        End Structure


#End Region

    End Module

End Namespace