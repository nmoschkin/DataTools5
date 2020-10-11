Option Explicit On
Option Strict On

Imports System.Drawing.Printing
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Windows
Imports System.IO
Imports System.Text
Imports System.Runtime.InteropServices
Imports System.ComponentModel

Namespace TextMapping

#Region "LineSegment"

    ''' <summary>
    ''' Line-segment sorting options.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum LineSegmentCompareTypes
        ''' <summary>
        ''' Sorts by line number (Y coordinate)
        ''' </summary>
        ''' <remarks></remarks>
        ByIndex

        ''' <summary>
        ''' Sorts by Origin (Left X coordinate)
        ''' </summary>
        ''' <remarks></remarks>
        ByOrigin

        ''' <summary>
        ''' Sorts by length of segment.
        ''' </summary>
        ''' <remarks></remarks>
        ByLength

        ''' <summary>
        ''' Sorts by EndPoint (Right X coordinate)
        ''' </summary>
        ''' <remarks></remarks>
        ByEndPoint

    End Enum

    ''' <summary>
    ''' Describes a line segment.
    ''' </summary>
    ''' <remarks></remarks>
    <StructLayout(LayoutKind.Sequential, Pack:=4)>
    Public Structure LineSegment

        ''' <summary>
        ''' The X-Origin of the line.
        ''' </summary>
        ''' <remarks></remarks>
        Public Origin As Integer

        ''' <summary>
        ''' The length of the line.
        ''' </summary>
        ''' <remarks></remarks>
        Public Length As Integer

        ''' <summary>
        ''' The index of the line.
        ''' </summary>
        ''' <remarks></remarks>
        Public Index As Integer

        Public ReadOnly Property X1 As Integer
            Get
                Return Origin
            End Get
        End Property

        Public ReadOnly Property EndPoint As Integer
            Get
                Return (Origin + Length) - 1
            End Get
        End Property

        Public Sub New(Origin As Integer, length As Integer, Optional index As Integer = 0)
            Me.Origin = Origin
            Me.Length = length
            Me.Index = index
        End Sub

        Public Overrides Function ToString() As String
            Return "ORIGIN=" & Origin & ", LENGTH=" & Length & If(Index <> 0, ", LINE INDEX=" & Index, "")
        End Function

    End Structure

    Public Class LineSegmentsComparer
        Implements IComparer(Of LineSegment)

        ''' <summary>
        ''' Specifies whether to sort in descending order.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Descending As Boolean

        Public Property SortType As LineSegmentCompareTypes

        ''' <summary>
        ''' Compare two line segments according to the configured criteria.
        ''' </summary>
        ''' <param name="x"></param>
        ''' <param name="y"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Compare(x As LineSegment, y As LineSegment) As Integer Implements IComparer(Of LineSegment).Compare

            Dim c As Integer = 0

            Select Case _SortType

                Case LineSegmentCompareTypes.ByIndex
                    c = x.Index - y.Index

                Case LineSegmentCompareTypes.ByLength
                    c = x.Length - y.Length

                Case LineSegmentCompareTypes.ByOrigin
                    c = x.Origin - y.Origin

                Case LineSegmentCompareTypes.ByEndPoint
                    c = x.EndPoint - y.EndPoint

            End Select

            If _Descending Then c = -c

            Return c
        End Function

        ''' <summary>
        ''' Creats a new LineSegmentComparer
        ''' </summary>
        ''' <param name="descending">Whether to sort descending</param>
        ''' <remarks></remarks>
        Public Sub New(Optional descending As Boolean = False, Optional sortType As LineSegmentCompareTypes = LineSegmentCompareTypes.ByIndex)
            Me.Descending = descending
            Me.SortType = sortType
        End Sub

    End Class

#End Region

#Region "LineSegments Collection"

    ''' <summary>
    ''' Describes a region of the page in terms of a collection of line segments.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class LineSegments
        Inherits ObjectModel.Collection(Of LineSegment)

        Private _isContiguous As Boolean = False

        Private _Area As Double = Double.NaN

        Private _lct As Integer = 140
        Private _oct As Integer = 100

        ''' <summary>
        ''' Retrieve the calculated bounding rectangle from the specified configuration options.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Bounds As Rectangle
            Get
                Dim y As Integer = Me.Count - 1
                If y < 1 Then Return Nothing

                Return New Rectangle(MaxOrigin, Me(0).Index, MinEndPoint - MaxOrigin, y)
            End Get
        End Property

        ''' <summary>
        ''' Gets the area of the rectangle.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Area As Double
            Get
                If Double.IsNaN(_Area) Then
                    _Area = (MinEndPoint - MaxOrigin) * Me.Count
                End If

                Return _Area
            End Get
            Friend Set(value As Double)
                _Area = value
            End Set
        End Property

        ''' <summary>
        ''' The maxmimum distance in length between two lines that could trigger a discontinuity.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property LengthContinuityThreshold As Integer
            Get
                Return _lct
            End Get
            Set(value As Integer)
                _lct = value
                Me.TestContiguous()
            End Set
        End Property

        ''' <summary>
        ''' The maxmimum distance in origin between two lines that could trigger a discontinuity.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property OriginContinuityThreshold As Integer
            Get
                Return _oct
            End Get
            Set(value As Integer)
                _oct = value
                Me.TestContiguous()
            End Set
        End Property

        ''' <summary>
        ''' Returns the minimum screen coordinate value for the right-hand boundary.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property MinEndPoint As Integer
            Get
                Dim x As Integer = -1

                If Me.Count = 0 Then Return -1
                For Each p In Me
                    If p.EndPoint < x OrElse x = -1 Then
                        x = p.EndPoint
                    End If
                Next

                Return x
            End Get
        End Property

        ''' <summary>
        ''' Returns the maximum screen coordinate value for the right-hand boundary.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property MaxEndPoint As Integer
            Get
                Dim x As Integer = -1

                If Me.Count = 0 Then Return -1
                For Each p In Me
                    If p.EndPoint > x OrElse x = -1 Then
                        x = p.EndPoint
                    End If
                Next

                Return x
            End Get
        End Property

        ''' <summary>
        ''' Returns the average EndPoint
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property AverageEndPoint As Double
            Get
                Return (MaxEndPoint + MinEndPoint) / 2
            End Get
        End Property

        ''' <summary>
        ''' Returns the median EndPoint, where half the values are greater, and half the values are less.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property MedianEndPoint As LineSegment
            Get
                If Count = 0 Then Return Nothing Else If Count < 3 Then Return Me(0)

                Dim l As List(Of LineSegment) = ToList

                l.Sort(New LineSegmentsComparer(False, LineSegmentCompareTypes.ByEndPoint))

                Dim c As Integer = l.Count

                If (c And 1) = 1 Then c += 1
                c = CInt(c / 2) - 1

                Return l(c)
            End Get
        End Property

        ''' <summary>
        ''' Returns the minimum x-coordinate Origin of a set of lines.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property MinOrigin As Integer
            Get
                Dim x As Integer = -1

                If Me.Count = 0 Then Return -1
                For Each p In Me
                    If p.Origin < x OrElse x = -1 Then
                        x = p.Origin
                    End If
                Next

                Return x
            End Get
        End Property

        ''' <summary>
        ''' Returns the maximum x-coordinate Origin of a set of lines.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property MaxOrigin As Integer
            Get
                Dim x As Integer = -1

                If Me.Count = 0 Then Return -1
                For Each p In Me
                    If p.Origin > x OrElse x = -1 Then
                        x = p.Origin
                    End If
                Next

                Return x
            End Get
        End Property

        ''' <summary>
        ''' Returns the average X-coordinate Origin of a set of lines.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property AverageOrigin As Single
            Get
                Dim d As Single = 0.0
                For Each p In Me

                    d += p.Origin
                Next

                d /= Me.Count
                Return d
            End Get
        End Property

        ''' <summary>
        ''' Returns the median Origin, where half the values are greater, and half the values are less.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property MedianOrigin As LineSegment
            Get
                If Count = 0 Then Return Nothing Else If Count < 3 Then Return Me(0)

                Dim l As List(Of LineSegment) = ToList

                l.Sort(New LineSegmentsComparer(False, LineSegmentCompareTypes.ByOrigin))

                Dim c As Integer = l.Count

                If (c And 1) = 1 Then c += 1
                c = CInt(c / 2) - 1

                Return l(c)
            End Get
        End Property

        ''' <summary>
        ''' Returns the minimum x-coordinate length of a set of lines.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property MinLength As Integer
            Get
                Dim x As Integer = -1

                If Me.Count = 0 Then Return -1
                For Each p In Me
                    If p.Length < x OrElse x = -1 Then
                        x = p.Length
                    End If
                Next

                Return x
            End Get
        End Property

        ''' <summary>
        ''' Returns the maximum x-coordinate length of a set of lines.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property MaxLength As Integer
            Get
                Dim x As Integer = -1

                If Me.Count = 0 Then Return -1
                For Each p In Me
                    If p.Length > x OrElse x = -1 Then
                        x = p.Length
                    End If
                Next

                Return x
            End Get
        End Property

        ''' <summary>
        ''' Returns the average X-coordinate length of a set of lines.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property AverageLength As Single
            Get
                Dim d As Single = 0.0
                For Each p In Me
                    d += p.Length
                Next

                d /= Me.Count
                Return d
            End Get
        End Property

        ''' <summary>
        ''' Returns the median length, where half the values are greater, and half the values are less.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property MedianLength As LineSegment
            Get
                If Count = 0 Then Return Nothing Else If Count < 3 Then Return Me(0)

                Dim l As List(Of LineSegment) = ToList

                l.Sort(New LineSegmentsComparer(False, LineSegmentCompareTypes.ByLength))

                Dim c As Integer = l.Count

                If (c And 1) = 1 Then c += 1
                c = CInt(c / 2) - 1

                Return l(c)
            End Get
        End Property

        ''' <summary>
        ''' Returns true of this data set represents a single continguous region.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property IsContiguous As Boolean
            Get
                Return _isContiguous
            End Get
        End Property

        ''' <summary>
        ''' test the collection for linear continuity.
        ''' Normally, this should not need to be called by the consumer, since continuity is tested whenever the colleciton is modified.
        ''' </summary>
        ''' <param name="startFrom"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function TestContiguous(Optional startFrom As Integer = 0) As Boolean
            If Me.Count = 0 Then Return False Else If Me.Count = 1 Then Return True

            Dim idx As Integer = Me(startFrom).Index

            Dim i As Integer = startFrom, _
                c As Integer = Me.Count - 2

            If startFrom <> 0 Then startFrom -= 1

            For i = startFrom To c

                If (Me(i).Index <> Me(i + 1).Index - 1) _
                    OrElse (Math.Abs(Me(i).EndPoint - Me(i + 1).EndPoint) >= _lct) _
                    OrElse (Math.Abs(Me(i).Origin - Me(i + 1).Origin) >= _oct) Then
                    _isContiguous = False
                    Return False
                End If

            Next

            _isContiguous = True
            Return True
        End Function

        ''' <summary>
        ''' Returns an array of all contiguous segments in a non-contiguous collection.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function FindAllContiguousRegions() As LineSegments()

            Dim l As New List(Of LineSegments)
            Dim pps As LineSegments

            Dim y As Integer = 0

            Do
                Try
                    pps = GetContiguousRegion(y, y)
                    If pps IsNot Nothing Then l.Add(pps)
                    y += 1
                Catch ex As Exception
                    Exit Do
                End Try
            Loop

            Return l.ToArray

        End Function

        ''' <summary>
        ''' Gets a contiguous region starting at the specified Y coordinate.
        ''' </summary>
        ''' <param name="startY">The starting Y coordinate.</param>
        ''' <param name="stopY">Receives the stopping Y coordinate.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetContiguousRegion(startY As Integer, Optional ByRef stopY As Integer = 0) As LineSegments

            Dim i As Integer = 0, _
                c As Integer = Me.Count

            Dim inCont As Boolean = False
            Dim newPairs As New LineSegments
            Dim pp As LineSegment
            Dim y As Integer = startY
            Dim len As Integer = 0
            Dim org As Integer = 0

            newPairs._isContiguous = True
            If startY > c OrElse startY < 0 Then Throw New ArgumentOutOfRangeException("startY")

            Do
                pp = Me(i)

                If Not inCont Then
                    If pp.Index >= startY Then
                        inCont = True
                        newPairs.Add(pp)
                        y = pp.Index
                        len = pp.EndPoint
                        org = pp.Origin
                    End If
                ElseIf ((pp.Index = y + 1) OrElse (pp.Index = y)) AndAlso (Math.Abs(pp.EndPoint - len) < _lct) AndAlso (Math.Abs(pp.Origin - org) < _oct) Then
                    y = pp.Index
                    len = pp.EndPoint
                    org = pp.Origin
                    newPairs.Add(pp)
                Else
                    Exit Do
                End If

                i += 1

            Loop Until i = c

            stopY = y
            Return newPairs

        End Function

        ''' <summary>
        ''' Finds a set of lines who all meet the specified criteria.
        ''' </summary>
        ''' <param name="containsX1">The line must contain the first x coordinate.</param>
        ''' <param name="containsX2">The line must contain the second x coordinate.</param>
        ''' <param name="startY">Optional starting line.</param>
        ''' <param name="makeContiguous">If set to true, all items will have the same X1 and X2 and the collection will be marked contiguous.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function FindSet(containsX1 As Integer, containsX2 As Integer, Optional startY As Integer = 0, Optional makeContiguous As Boolean = False, Optional reverseScan As Boolean = False) As LineSegments

            Dim i As Integer, _
                c As Integer = Me.Count

            Dim pp As LineSegment
            Dim inCont As Boolean = False
            Dim startIdx As Integer = -1
            Dim stopIdx As Integer = -1

            'Dim tCol As New List(Of LineSegment)

            'For i = 0 To Me.Count - 1
            '    tCol.Add(MergeSegments(GetLine(Me(i).Index)))
            'Next
            Dim tcol As LineSegments = Me

            c = tcol.Count

            If c < 2 Then Return Nothing

            If reverseScan Then


                i = c - 1
                Do
                    pp = tcol(i)

                    If inCont = False Then
                        If pp.Index <= startY Then

                            If pp.Origin <= containsX1 AndAlso pp.EndPoint >= containsX1 _
                                AndAlso pp.EndPoint >= containsX2 AndAlso pp.Origin <= containsX2 Then
                                inCont = True
                                startIdx = i
                                stopIdx = i
                            End If
                        End If
                    Else

                        If pp.Origin <= containsX1 AndAlso pp.EndPoint >= containsX1 _
                           AndAlso pp.EndPoint >= containsX2 AndAlso pp.Origin <= containsX2 Then

                            stopIdx = i
                        Else
                            Exit Do
                        End If
                    End If

                    i -= 1
                Loop Until i = -1

                i = stopIdx
                stopIdx = startIdx
                startIdx = i

            Else


                i = 0
                Do
                    pp = tcol(i)

                    If inCont = False Then
                        If pp.Index >= startY Then

                            If pp.Origin <= containsX1 AndAlso pp.EndPoint >= containsX1 _
                                AndAlso pp.EndPoint >= containsX2 AndAlso pp.Origin <= containsX2 Then
                                inCont = True
                                startIdx = i
                                stopIdx = i
                            End If
                        End If
                    Else
                        If pp.Origin <= containsX1 AndAlso pp.EndPoint >= containsX1 _
                           AndAlso pp.EndPoint >= containsX2 AndAlso pp.Origin <= containsX2 Then

                            stopIdx = i
                        Else
                            Exit Do
                        End If
                    End If

                    i += 1
                Loop Until i = c

            End If

            If stopIdx >= 0 AndAlso startIdx >= 0 AndAlso stopIdx <> startIdx Then

                Dim p As New LineSegments

                For i = startIdx To stopIdx
                    pp = tcol(i)
                    If makeContiguous Then
                        pp.Origin = containsX1
                        pp.Length = containsX2 - pp.Origin
                    End If

                    p.Add(pp)
                Next

                If makeContiguous Then p._isContiguous = True

                Return p
            End If

            Return Nothing
        End Function

        ''' <summary>
        ''' Sort the collection according the the specified criteria.
        ''' </summary>
        ''' <param name="descending">Order descending.</param>
        ''' <param name="sortType">Ordering mode.</param>
        ''' <remarks></remarks>
        Public Sub Sort(Optional descending As Boolean = False, Optional sortType As LineSegmentCompareTypes = LineSegmentCompareTypes.ByIndex)

            Dim l As List(Of LineSegment) = Me.ToList
            l.Sort(New LineSegmentsComparer(descending, sortType))

            Dim i As Integer = 0, _
                c As Integer = Me.Count

            If c = 0 Then Return
            Do
                Me(i) = l(i)
                i += 1
            Loop Until i = c

            Return
        End Sub

        ''' <summary>
        ''' Returns all positions in a line.
        ''' </summary>
        ''' <param name="y"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function GetLine(y As Integer, Optional sortFirst As Boolean = False) As LineSegment()

            Dim l As LineSegments = Me
            Dim lOut As New List(Of LineSegment)

            If sortFirst Then l.Sort()

            Dim i As Integer, _
                c As Integer = l.Count - 1

            For i = 0 To c
                If l(i).Index = y Then

                    Do While l(i).Index = y
                        lOut.Add(l(i))
                        i += 1
                        If i > c Then Exit Do
                    Loop

                    Return lOut.ToArray
                End If
            Next

            Return Nothing
        End Function

        Public Shared Function MergeSegments(ls1 As LineSegments, ls2 As LineSegments) As LineSegments

            Dim n As New LineSegments

            For Each l In ls1
                n.Add(l)
            Next

            For Each l In ls2
                n.Add(l)
            Next

            n.Sort()
            Return n
        End Function

        Private Function MergeSegments(segments As LineSegment()) As LineSegment

            Dim l As New LineSegment

            Dim i As Integer, _
                c As Integer = segments.Count - 1

            Dim o As Integer = -1
            Dim ep As Integer = -1

            For i = 0 To c

                If segments(i).Origin < o OrElse o = -1 Then
                    o = segments(i).Origin
                End If

                If segments(i).EndPoint > ep OrElse ep = -1 Then
                    ep = segments(i).EndPoint
                End If

            Next

            l.Origin = o
            l.Length = ep - o
            l.Index = segments(0).Index

            Return l

        End Function

        Protected Overrides Sub InsertItem(index As Integer, item As LineSegment)
            MyBase.InsertItem(index, item)
            _Area = Double.NaN
            TestContiguous(If(_isContiguous, index, 0))
        End Sub

        Protected Overrides Sub RemoveItem(index As Integer)
            MyBase.RemoveItem(index)
            _Area = Double.NaN
            TestContiguous()
        End Sub

        Protected Overrides Sub ClearItems()
            MyBase.ClearItems()
            _Area = Double.NaN
            _isContiguous = False
        End Sub

    End Class

#End Region

End Namespace