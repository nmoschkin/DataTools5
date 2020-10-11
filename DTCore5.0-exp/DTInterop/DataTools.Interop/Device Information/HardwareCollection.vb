'' ************************************************* ''
'' DataTools Visual Basic Utility Library - Interop
''
'' Module: HardwareCollection 
''         Computer information collection class.
'' 
'' Copyright (C) 2011-2020 Nathan Moschkin
'' All Rights Reserved
''
'' Licensed Under the Microsoft Public License   
'' ************************************************* ''


Imports System.Collections.ObjectModel
Imports System.Windows.Media.Imaging
Imports DataTools.Interop.Desktop

#Region "Hardware Collection"

''' <summary>
''' Encapsulates a hierarchical representation of all visible devices on the system.
''' </summary>
''' <remarks></remarks>
Public Class HardwareCollection
    Inherits ObservableCollection(Of Object)

    Private _Class As DeviceClassEnum
    Private _Icon As BitmapSource

    ''' <summary>
    ''' Returns the class device enumeration for this device collection.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property DeviceClass As DeviceClassEnum
        Get
            Return _Class
        End Get
        Private Set(value As DeviceClassEnum)
            _Class = value
        End Set
    End Property

    ''' <summary>
    ''' Returns the class description associated with this device collection.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Description As String
        Get
            Return DataTools.Interop.Native.Utility.GetEnumDescription(DeviceClass)
        End Get
    End Property

    ''' <summary>
    ''' Returns the class icon associated with this device collection.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ClassIcon As BitmapSource
        Get
            Return _Icon
        End Get
        Private Set(value As BitmapSource)
            _Icon = value
        End Set
    End Property

    ''' <summary>
    ''' For the heirarchical data arrangement, returns the members of this collection instance.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property LinkedChildren As ObservableCollection(Of Object)
        Get
            Return CType(Me, ObservableCollection(Of Object))
        End Get
    End Property

    ''' <summary>
    ''' Enumerates all computer devices on the system and creates a coherent hierarchy divided into device class sections.
    ''' </summary>
    ''' <returns>A hierarchical enumeration of all visible devices on the system.</returns>
    ''' <remarks></remarks>
    Public Shared Function CreateComputerHierarchy() As HardwareCollection

        Dim c As New List(Of DeviceInfo)
        Dim e As New List(Of DeviceInfo)

        Dim f As New HardwareCollection
        Dim g As HardwareCollection = Nothing

        Dim chw As DeviceClassEnum = DeviceClassEnum.Undefined

        '' do the initial enumeration.
        Dim d As ObservableCollection(Of Object) = EnumComputerExhaustive()

        '' Filter out all non-top-level devices to create the top level.
        For Each x In d
            If CType(x, DeviceInfo).LinkedParent Is Nothing Then
                c.Add(CType(x, DeviceInfo))
            End If
        Next

        '' sort all top-level devices by their device class.
        c.Sort(New HardwareObjectSorter)

        For Each x In c

            '' If we don't already have an object devoted to particular type create it
            If x.DeviceClass <> chw Then
                chw = x.DeviceClass
                f.Add(g)
                g = New HardwareCollection
                g.DeviceClass = chw
                g.ClassIcon = MakeWPFImage(x.DeviceClassIcon)
            End If

            g.Add(x)
        Next

        If g IsNot Nothing AndAlso g.Count > 0 Then f.Add(g)

        Return f

    End Function

End Class

''' <summary>
''' Compare two DeviceInfo-derived objects by DeviceClass (alphabetically).
''' </summary>
''' <remarks></remarks>
Public Class HardwareObjectSorter
    Implements IComparer(Of DeviceInfo)

    ''' <summary>
    ''' Compare two DeviceInfo-derived objects by DeviceClass (alphabetically).
    ''' </summary>
    ''' <param name="x"></param>
    ''' <param name="y"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Compare(x As DeviceInfo, y As DeviceInfo) As Integer Implements IComparer(Of DeviceInfo).Compare
        Return String.Compare(x.DeviceClass.ToString, y.DeviceClass.ToString)
    End Function
End Class

#End Region