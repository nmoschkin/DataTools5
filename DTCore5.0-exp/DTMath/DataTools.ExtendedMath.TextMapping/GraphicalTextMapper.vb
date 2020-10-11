Option Strict On
Option Explicit On

Imports System.Drawing.Printing
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Windows
Imports System.IO
Imports System.Text
Imports System.Runtime.InteropServices
Imports System.ComponentModel
Imports DataTools.ExtendedMath.ColorMath

Namespace TextMapping

    Public Class CalculatedEventArgs
        Inherits EventArgs

        Private _Start As Date

        Public Sub StartTimer()
            _Start = Now
        End Sub

        Public Sub StopTimer()
            _Stop = Now
        End Sub

        Public ReadOnly Property Elapsed As TimeSpan
            Get
                Return (CType(_Stop - _Start, TimeSpan))
            End Get
        End Property

        Public ReadOnly Property Start As Date
            Get
                Return _Start
            End Get
        End Property

        Private _Stop As Date

        Public Property [Stop] As Date
            Get
                Return _Stop
            End Get
            Friend Set(value As Date)
                _Stop = value
            End Set
        End Property

        Private _Regions As List(Of UniRect)

        Public Property Regions As List(Of UniRect)
            Get
                Return _Regions
            End Get
            Friend Set(value As List(Of UniRect))
                _Regions = value
            End Set
        End Property

        Private _TotalCalculations As Integer

        Public Property TotalCalculations As Integer
            Get
                Return _TotalCalculations
            End Get
            Friend Set(value As Integer)
                _TotalCalculations = value
            End Set
        End Property

        Friend Sub New(Optional r As List(Of UniRect) = Nothing, Optional t As Integer = 0, Optional b As Date = #1/1/1901#, Optional e As Date = #1/1/1901#)
            _Start = If(b = #1/1/1901#, Date.Now, b)
            _Stop = If(e = #1/1/1901#, Date.Now, e)
            _Regions = r
            _TotalCalculations = t
        End Sub

    End Class

    ''' <summary>
    ''' Maps blank areas of an image that could be used to render text.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class GraphicalTextMapper
        Inherits ObjectModel.ObservableCollection(Of LineSegments)

        Private _rawData As LineSegments

        Private _img As System.Drawing.Bitmap

        Private _rendered As System.Drawing.Bitmap

        Private _Scale As Double

        Private _maxX As Integer = 800

        Private _maxY As Integer = 800

        Private _calcSize As System.Windows.Size

        Private _minUsableX As Double = 0.5

        Private _minUsableY As Double = 0.25

        Private _purgeOverlaps As Boolean = True

        Private _Regions As New List(Of UniRect)

        Public Event Rendered(sender As Object, e As EventArgs)
        Public Event Calculated(sender As Object, e As CalculatedEventArgs)

#Region "Public ReadOnly Properties"

        ''' <summary>
        ''' Returns all of the regions that were discovered during the last run.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Regions As List(Of UniRect)
            Get
                Return _Regions
            End Get
        End Property

        ''' <summary>
        ''' The scaled, rendered image.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property RenderedImage As Bitmap
            Get
                Return _rendered
            End Get
        End Property

        ''' <summary>
        ''' Returns the raw, unprocessed data from the image scan as a LineSegments object.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property RawOutput As LineSegments
            Get
                Return _rawData
            End Get
        End Property

        ''' <summary>
        ''' The size of the scaled image.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ScaleSize As System.Windows.Size
            Get
                Return _calcSize
            End Get
        End Property

        ''' <summary>
        ''' The scale factor.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property RenderScale As Double
            Get
                Return _Scale
            End Get
        End Property

#End Region

#Region "Public Properties"

        ''' <summary>
        ''' Specifies whether to purge overlapping rectangles from the final collection.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PurgeOverlapping As Boolean
            Get
                Return _purgeOverlaps
            End Get
            Set(value As Boolean)
                _purgeOverlaps = value
                OnPropertyChanged("PurgeOverlapping")
                Refresh()
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the minimum usable width (in percentage of page size) to be considered when performing shape identification.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MinUsableWidth As Double
            Get
                Return _minUsableX
            End Get
            Set(value As Double)
                _minUsableX = value
                OnPropertyChanged("MinUsableWidth")
                Refresh()
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the minimum usable height (in percentage of page size) to be considered when performing shape identification.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MinUsableHeight As Double
            Get
                Return _minUsableY
            End Get
            Set(value As Double)
                _minUsableY = value
                OnPropertyChanged("MinUsableHeight")
                Refresh()
            End Set
        End Property

        ''' <summary>
        ''' The maximum allowable X extent.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MaxX As Integer
            Get
                Return _maxX
            End Get
            Set(value As Integer)
                _maxX = value
                OnPropertyChanged("MaxX")
                Refresh()
            End Set
        End Property

        ''' <summary>
        ''' The maximum allowable Y-extent.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MaxY As Integer
            Get
                Return _maxY
            End Get
            Set(value As Integer)
                _maxY = value
                OnPropertyChanged("MaxY")
                Refresh()
            End Set
        End Property

        ''' <summary>
        ''' The active processing image.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Image As Bitmap
            Get
                Return _img
            End Get
            Set(value As Bitmap)
                _img = value
                OnPropertyChanged("Image")
                Refresh()
            End Set
        End Property

#End Region

#Region "Public Methods"

        ''' <summary>
        ''' Recalculate and redraw all.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Refresh()
            BuildRegions()
        End Sub

        ''' <summary>
        ''' Create a new instance of the GraphicalTextMapper.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()

        End Sub

        ''' <summary>
        ''' Create a new instance of the GraphicalTextMapper from the specified image.
        ''' </summary>
        ''' <param name="image">The image to use.</param>
        ''' <param name="calcImmediately">Specify whether or not to process the image upon instantiation.</param>
        ''' <remarks></remarks>
        Public Sub New(image As System.Drawing.Image, Optional calcImmediately As Boolean = True)
            _img = CType(image, Bitmap)
            If calcImmediately Then Refresh()
        End Sub

#End Region

#Region "Protected Methods"

        ''' <summary>
        ''' Calculate the dimensions for rendering.
        ''' </summary>
        ''' <remarks></remarks>
        Protected Sub CalcDims()

            Dim x As Double, _
                y As Double

            If _img.Height > _img.Width Then
                _Scale = (MaxY / _img.Height)
                y = MaxY
                x = _img.Width * _Scale
            Else
                _Scale = (MaxX / _img.Width)
                x = MaxX
                y = _img.Height * _Scale
            End If

            _calcSize = New System.Windows.Size(x, y)

            OnPropertyChanged("RenderScale")
        End Sub

        ''' <summary>
        ''' Builds all of the discovered open areas of an image.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function BuildRegions() As Boolean
            Try
                If _img Is Nothing Then Return False
                Calculate()

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Process the image and render the output.
        ''' </summary>
        ''' <param name="drawRectangles">Specifies whether to draw the discovered rectangles on the output image.</param>
        ''' <remarks></remarks>
        Public Function RenderImage(Optional drawRectangles As Boolean = True) As System.Drawing.Bitmap
            If _img Is Nothing Then Return Nothing

            CalcDims()

            If _Regions Is Nothing OrElse _Regions.Count = 0 Then
                Calculate()
            End If

            Dim rc() As UniRect = _Regions.ToArray
            If _rendered IsNot Nothing Then
                _rendered.Dispose()
            End If

            Dim p As New Pen(Brushes.Black, 2)

            Dim x As Double, _
                y As Double

            Dim img2 As New Bitmap(_img.Width, _img.Height, PixelFormat.Format32bppArgb)

            Dim g As Graphics, _
                g2 As Graphics = Graphics.FromImage(img2)

            g2.DrawImage(_img, 0, 0, _img.Width, _img.Height)

            If drawRectangles Then
                For Each r In rc
                    g2.DrawRectangle(p, CType(r, Rectangle))
                Next
            End If

            p.Dispose()
            g2.Dispose()

            If _img.Height > _img.Width Then
                y = MaxY
                x = _img.Width * (MaxY / _img.Height)
            Else
                x = MaxX
                y = _img.Height * (MaxX / _img.Width)
            End If

            _rendered = New Bitmap(CInt(x), CInt(y), PixelFormat.Format32bppArgb)

            g = Graphics.FromImage(_rendered)
            g.DrawImage(img2, 0, 0, CSng(x), CSng(y))
            img2.Dispose()
            g.Dispose()

            OnPropertyChanged("RenderedImage")

            RaiseEvent Rendered(Me, New EventArgs)

            Return _rendered
        End Function

        ''' <summary>
        ''' Draws a grid onto an image.
        ''' </summary>
        ''' <param name="img">The image to work with.</param>
        ''' <param name="GridX">The spacing of the X grid, in pixels.</param>
        ''' <param name="GridY">The spacing of the Y grid, in pixels.</param>
        ''' <param name="ColorX">The color of the X grid.</param>
        ''' <param name="ColorY">The color of the Y grid.</param>
        ''' <param name="Thickness">The line thickness in pixels.</param>
        ''' <param name="GridType">The grid type.</param>
        ''' <param name="DashPatternX">The X dash pattern.</param>
        ''' <param name="DashPatternY">The Y dash pattern.</param>
        ''' <remarks></remarks>
        Public Shared Sub RenderGrid( _
                                    img As System.Drawing.Bitmap, _
                                    GridX As Single, _
                                    GridY As Single, _
                                    ColorX As UniColor, _
                                    ColorY As UniColor, _
                                    Optional Thickness As Double = 1.0#, _
                                    Optional GridType As GridLinesType = GridLinesType.Dotted, _
                                    Optional DashPatternX() As Single = Nothing, _
                                    Optional DashPatternY() As Single = Nothing)

            Dim g As System.Drawing.Graphics = Graphics.FromImage(img)

            Dim x As Integer, _
                y As Integer

            Dim cx As Integer, _
                cy As Integer

            Dim dashValuesX() As Single = {1, 0, 1, 0}
            Dim dashValuesY() As Single = {1, 0, 1, 0}

            If DashPatternX IsNot Nothing Then
                dashValuesX = DashPatternX
            End If

            If DashPatternY IsNot Nothing Then
                dashValuesY = DashPatternY
            End If

            Dim pnX As System.Drawing.Pen
            Dim pnY As System.Drawing.Pen

            pnX = New System.Drawing.Pen(ColorX, CSng(Thickness))
            pnY = New System.Drawing.Pen(ColorY, CSng(Thickness))

            '        g.FillRectangle(br2, 0, 0, Me.Width, Me.Height)

            Select Case GridType

                Case GridLinesType.Dashed

                    dashValuesX(0) = Math.Max(1, (GridX / 4) - 2)
                    dashValuesX(1) = 2
                    dashValuesX(2) = Math.Max(1, (GridX / 4) - 2)
                    dashValuesX(3) = 2

                    dashValuesY(0) = Math.Max(1, (GridY / 4) - 2)
                    dashValuesY(1) = 2
                    dashValuesY(2) = Math.Max(1, (GridY / 4) - 2)
                    dashValuesY(3) = 2

                    pnX.DashPattern = dashValuesX
                    pnY.DashPattern = dashValuesY

                Case GridLinesType.Dotted
                    dashValuesX(0) = 1
                    dashValuesX(1) = Math.Max(1, GridX - 1)
                    dashValuesX(2) = 1
                    dashValuesX(3) = Math.Max(1, GridX - 1)

                    dashValuesY(0) = 1
                    dashValuesY(1) = Math.Max(1, GridY - 1)
                    dashValuesY(2) = 1
                    dashValuesY(3) = Math.Max(1, GridY - 1)

                    pnX.DashPattern = dashValuesX
                    pnY.DashPattern = dashValuesY
            End Select

            cx = img.Width
            cy = img.Height

            For x = CInt(GridX / 2) To cx Step CInt(GridX)
                If (GridType = GridLinesType.Dotted) Then
                    g.DrawLine(pnX, x, CInt(Math.Floor(GridY / 2)), x, cy)
                Else
                    'If (x = (GridX / 2)) Then Continue For
                    g.DrawLine(pnX, x, 0, x, cy)

                End If
            Next

            For y = CInt(GridY / 2) To cy Step CInt(GridY)
                If (GridType = GridLinesType.Dotted) Then
                    g.DrawLine(pnX, CInt(Math.Floor(GridX / 2)), y, cx, y)
                Else
                    'If (y = (GridY / 2)) Then Continue For
                    g.DrawLine(pnX, 0, y, cx, y)

                End If
            Next

            pnX.Dispose()
            pnY.Dispose()

            g.Dispose()

        End Sub

        ''' <summary>
        ''' RenderScale the given rectangle by the current scale factor
        ''' </summary>
        ''' <param name="rcIn"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Function ScaleRect(rcIn As Rectangle) As Rectangle
            Return New Rectangle(CInt(rcIn.X * _Scale), CInt(rcIn.Y * _Scale), CInt(rcIn.Width * _Scale), CInt(rcIn.Height * _Scale))
        End Function

        ''' <summary>
        ''' Perform all the calculations necessary to determine the
        ''' usable text area from the given template and stationory image.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Function Calculate() As Rectangle()

            Dim bmp As Bitmap

            '' what we're going to do is map every possible rectangular region in the image.
            '' we will accept a certain percentage of white space to add that rectangle to the image.

            Dim minX As Double, _
                minY As Double


            Dim x As Integer, _
                y As Integer

            Dim cx As Integer, _
                cy As Integer

            Dim bInUsable As Boolean = False

            Dim useStart As New PointF
            Dim useStop As New PointF

            Dim l As Integer
            Dim c As Integer = 0, _
                d As Integer = 0

            Dim bPass As Boolean = False
            Dim lastUseStart As New PointF

            Dim rcCol As New List(Of Rectangle)

            Dim lineParts As New LineSegments
            Dim hsv As New ColorMath.HSVDATA
            Dim ceArgs As New CalculatedEventArgs

            ceArgs.StartTimer()

            'Dim sMult As Double = 1.0#
            'Dim maxImg As Integer = 2400

            bmp = _img

            'If _img.Width > maxImg OrElse _img.Height > maxImg Then

            '    If _img.Width > _img.Height Then
            '        x = maxImg
            '        y = _img.Height * (maxImg / _img.Width)
            '        sMult = maxImg / _img.Width
            '    Else
            '        y = maxImg
            '        x = _img.Width * (maxImg / _img.Height)
            '        sMult = maxImg / _img.Height
            '    End If

            '    bmp = New Bitmap(x, y, PixelFormat.Format32bppArgb)
            '    Dim g As Graphics = Graphics.FromImage(bmp)

            '    g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
            '    g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            '    g.SmoothingMode = Drawing2D.SmoothingMode.None

            '    g.DrawImage(_img, 0, 0, x, y)

            '    g.Dispose()
            '    _img = bmp
            'Else
            '    bmp = _img
            'End If

            cx = bmp.Width
            cy = bmp.Height

            minX = _minUsableX * cx
            minY = _minUsableY * cy

            Dim bm As Drawing.Imaging.BitmapData = bmp.LockBits(New Rectangle(0, 0, bmp.Width, bmp.Height), Imaging.ImageLockMode.ReadWrite, Imaging.PixelFormat.Format32bppArgb)
            Dim mm As CoreCT.Memory.MemPtr = bm.Scan0

            ceArgs.TotalCalculations = (cx * cy) * 2

            For y = 0 To cy - 1

                c = -1
                d = -1

                For x = 0 To cx - 1

                    l = ((y * cx) + x) * 4


                    If mm.ByteAt(l) >= 250 AndAlso _
                       mm.ByteAt(l + 1) >= 250 AndAlso _
                       mm.ByteAt(l + 2) >= 250 Then
                        If c = -1 Then
                            c = x
                            d = 0
                        Else
                            d += 1
                        End If
                    ElseIf d <> -1 Then
                        If d > minX Then
                            lineParts.Add(New LineSegment(c, d, y))
                        End If
                        c = -1
                        d = -1
                    End If
                Next

                If d > minX Then
                    lineParts.Add(New LineSegment(c, d, y))
                End If
            Next

            lineParts.Sort()
            _rawData = lineParts

            Dim experiment As New List(Of LineSegments)
            Dim temp As LineSegments
            'Dim temp2 As LineSegments

            Dim minArea As Double = minX * minY
            minArea *= 0.5

            ceArgs.TotalCalculations += (cx * cy)


            For x = 0 To cx - 1 Step CInt(cx * 0.0375)

                For y = 0 To cy - 1 Step CInt(cy * 0.0375)

                    l = ((y * cx) + x) * 4

                    temp = _rawData.FindSet(x, cx - x, y, False)

                    If (temp Is Nothing) OrElse (temp.Area < minArea) Then Continue For

                    For l = temp.Count - 2 To 1 Step -1

                        If temp(l).Index - temp(l - 1).Index > temp.OriginContinuityThreshold Then
                            temp.RemoveAt(l - 1)

                        End If

                    Next


                    ceArgs.TotalCalculations += temp.Count + _rawData.Count
                    experiment.Add(temp)

                Next
            Next

            experiment.Sort(New SortByArea)
            experiment.Reverse()

            Dim pps() As LineSegments '= lineParts.FindAllContiguousRegions
            pps = experiment.ToArray

            Me.Clear()

            bmp.UnlockBits(bm)

            ceArgs.TotalCalculations += pps.Count + _rawData.Count

            _Regions.Clear()

            For Each px In pps

                Dim rc As Rectangle = px.Bounds

                temp = _rawData.FindSet(rc.Left, rc.Right, rc.Top, True, False)
                If temp Is Nothing Then Continue For
                rc = temp.Bounds

                If rc.Width < minX Then Continue For
                If rc.Height < minY Then Continue For

                rc.Inflate(-20, -20)

                If _purgeOverlaps Then

refor:
                    For Each rcChk In rcCol
                        If rcChk.IntersectsWith(rc) Then

                            If ((rcChk.Width * rcChk.Height) > (rc.Width * rc.Height)) Then '  AndAlso (rcChk.Width > rc.Width) Then
                                rc = rcChk
                            ElseIf (TestSquareness(rcChk) < TestSquareness(rc)) Then 'AndAlso (rc.Width >= rcChk.Width) Then
                                rcCol.Remove(rcChk)
                                GoTo refor
                            Else
                                rc = Rectangle.Empty
                            End If
                            Exit For
                        End If
                    Next
                End If

                If temp IsNot Nothing Then Me.Add(temp)
                If rc <> Rectangle.Empty Then
                    ' If CheckRect(rc, cx, cy, mm) = False Then Continue For
                    rcCol.Add(rc)
                    _Regions.Add(CType(rc, UniRect))
                End If
            Next

            ceArgs.StopTimer()

            Calculate = rcCol.ToArray

            ceArgs.Regions = _Regions

            OnPropertyChanged("Regions")
            OnPropertyChanged("RawOutput")

            RaiseEvent Calculated(Me, ceArgs)

        End Function

        Protected Function CheckRect(rc As Rectangle, cx As Integer, cy As Integer, mm As CoreCT.Memory.MemPtr) As Boolean

            Dim x As Integer,
                y As Integer

            Dim l As Integer

            For y = rc.Top To rc.Bottom
                For x = rc.Left To rc.Right

                    l = ((y * cx) + x) * 4

                    If mm.ByteAt(l) < 250 OrElse
                       mm.ByteAt(l + 1) < 250 OrElse
                       mm.ByteAt(l + 2) < 250 Then

                        Return False
                    End If

                Next
            Next

            Return True
        End Function

        Protected Function TestSquareness(rect As Rectangle) As Double

            Try
                Dim avg As Double = (rect.Width + rect.Height) / 2

                Dim wpct As Double = rect.Width / avg
                Dim hpct As Double = rect.Height / avg

                Return 1 - Math.Abs(wpct - hpct)

            Catch ex As Exception
                Return Double.NaN
            End Try

        End Function

        Protected Overloads Sub OnPropertyChanged(name As String)
            OnPropertyChanged(New PropertyChangedEventArgs(name))
        End Sub


#End Region

    End Class


    Public Class SortByArea
        Implements IComparer(Of LineSegments)

        Public Function Compare(x As LineSegments, y As LineSegments) As Integer Implements IComparer(Of LineSegments).Compare
            Return CInt(x.Area - y.Area)
        End Function

    End Class

    Public Enum GridLinesType
        None
        Dotted
        Dashed
        Solid
    End Enum

End Namespace