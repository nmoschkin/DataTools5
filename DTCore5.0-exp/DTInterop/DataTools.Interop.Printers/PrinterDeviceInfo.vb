'' ************************************************* ''
'' DataTools Visual Basic Utility Library - Interop
''
'' Module: PrinterDeviceInfo
''         Descendant class of DeviceInfo for 
''         printers.
''
'' Copyright (C) 2011-2020 Nathan Moschkin
'' All Rights Reserved
''
'' Licensed Under the Microsoft Public License   
'' ************************************************* ''


Imports DataTools.Interop.Native
Imports DataTools.Interop


Namespace Printers

#Region "Printer Device Info"

    ''' <summary>
    ''' Describes a printer.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class PrinterDeviceInfo
        Inherits DeviceInfo

        Private _printInfo As PrinterObject

#Region "Shared"

        Private Shared _allPrinters As IEnumerable(Of PrinterDeviceInfo) = Nothing

        ''' <summary>
        ''' Refreshes the list of all system printers.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function RefreshPrinters() As Boolean

            Dim r As New List(Of PrinterDeviceInfo)
            Dim p() As PrinterDeviceInfo = Interop.EnumPrinters()

            If p Is Nothing OrElse p.Count < 1 Then
                Dim pr As IEnumerable(Of PrinterObject) = PrinterObjects.Printers
                Dim ap As New List(Of PrinterDeviceInfo)

                Dim icn As System.Drawing.Icon = GetClassIcon(GUID_DEVCLASS_PRINTER)

                For Each pe In pr
                    Dim f As New PrinterDeviceInfo
                    f.FriendlyName = pe.PrinterName
                    f.PrinterInfo = pe
                    f.DeviceClassIcon = icn
                    ap.Add(f)
                Next

                _allPrinters = ap
            Else
                r.AddRange(p)

                If r(0).FriendlyName.Contains("Root Print Queue") Then
                    r.RemoveAt(0)
                End If

                _allPrinters = r
            End If

            Return ((_allPrinters IsNot Nothing) AndAlso (_allPrinters.Count > 0))

        End Function

        ''' <summary>
        ''' Returns the list of all system printers.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property AllPrinters() As IEnumerable(Of PrinterDeviceInfo)
            Get
                Return _allPrinters
            End Get
        End Property

        ''' <summary>
        ''' Returns a PrinterDeviceInfo object based on the printer name.
        ''' </summary>
        ''' <param name="printerName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetPrinterFromName(printerName As String) As PrinterDeviceInfo

            Dim l = AllPrinters
            For Each p In l
                If p.FriendlyName = printerName Then Return p
            Next

            Return Nothing
        End Function

#End Region

        ''' <summary>
        ''' Returns the detailed printer information object.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PrinterInfo As PrinterObject
            Get
                Return _printInfo
            End Get
            Friend Set(value As PrinterObject)
                _printInfo = value
            End Set
        End Property

        Public Overrides ReadOnly Property UIDescription As String
            Get
                If String.IsNullOrEmpty(FriendlyName) Then Return MyBase.UIDescription Else Return FriendlyName
            End Get
        End Property

        Public Overrides Function ToString() As String
            If String.IsNullOrEmpty(FriendlyName) Then Return MyBase.ToString Else Return FriendlyName
        End Function

        Public Overrides Function Equals(obj As Object) As Boolean
            If Not TypeOf obj Is PrinterDeviceInfo Then Return False
            Return CType(obj, PrinterDeviceInfo).PrinterInfo.PrinterName = PrinterInfo.PrinterName
        End Function

    End Class

#End Region

End Namespace