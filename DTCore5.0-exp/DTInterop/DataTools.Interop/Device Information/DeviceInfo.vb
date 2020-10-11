'' ************************************************* ''
'' DataTools Visual Basic Utility Library - Interop
''
'' Module: DeviceInfo Hardware Information Class
'' 
'' Copyright (C) 2011-2020 Nathan Moschkin
'' All Rights Reserved
''
'' Licensed Under the Microsoft Public License   
'' ************************************************* ''

Imports DataTools.Interop.Native
Imports System.ComponentModel
Imports System.Reflection
Imports DataTools.Interop.Desktop
Imports CoreCT.Text

#Region "DeviceInfo"

''' <summary>
''' An object that represents a hardware device on the system.
''' </summary>
''' <remarks></remarks>
<TypeConverter(GetType(ExpandableObjectConverter))>
Public Class DeviceInfo

    Friend _devInfo As SP_DEVINFO_DATA

    Protected _HardwareIds As String()

    Friend _Vid As UShort
    Friend _Pid As UShort

    Protected _DevicePath As String
    Protected _FriendlyName As String
    Protected _InstanceId As String
    Protected _LocationPaths() As String
    Protected _LocationInfo As String
    Protected _PhysicalPath As Byte()
    Protected _UINumber As Integer
    Protected _Description As String
    Protected _ContainerId As Guid

    Protected _PDOName As String
    Protected _Manufacturer As String
    Protected _ModelId As Guid
    Protected _BusReportedDeviceDesc As String
    Protected _ClassName As String

    Protected _DeviceInterfaceClassGuid As Guid
    Protected _DeviceInterfaceClass As DeviceInterfaceClassEnum

    Protected _DeviceClassGuid As Guid
    Protected _DeviceClass As DeviceClassEnum

    Protected _DeviceClassIcon As System.Drawing.Icon
    Protected _Parent As String
    Protected _Children As String()
    Protected _RemovalPolicy As DeviceRemovalPolicy
    Protected _SafeRemovalRequired As Boolean
    Protected _Characteristics As DeviceCharacteristcs
    Protected _InstallDate As Date
    Protected _BusType As BusType
    Protected _VenderId As String = Nothing
    Protected _ProductId As String = Nothing
    Protected _DeviceIcon As System.Windows.Media.Imaging.BitmapSource

    Protected _LinkedChildren As DeviceInfo()
    Protected _LinkedParent As DeviceInfo

    ''' <summary>
    ''' Create a DeviceInfo-based class based upon the given class, populating common members.
    ''' </summary>
    ''' <typeparam name="T">A DeviceInfo-derived class.</typeparam>
    ''' <param name="inf">The object to duplicate.</param>
    ''' <returns>A new instance of T.</returns>
    ''' <remarks></remarks>
    Friend Shared Function Duplicate(Of T As {New, DeviceInfo})(inf As DeviceInfo) As T

        Dim fi() As FieldInfo = inf.GetType.GetFields(BindingFlags.NonPublic Or BindingFlags.Instance)
        Dim fo As FieldInfo

        Dim objT As New T

        For Each fe In fi
            fo = GetType(T).GetField(fe.Name, BindingFlags.NonPublic Or BindingFlags.Instance)

            If fo IsNot Nothing Then fo.SetValue(objT, fe.GetValue(inf))

        Next

        Return objT
    End Function

    ''' <summary>
    ''' Link all devices in an array of DeviceInfo objects with their relative parent and child objects.
    ''' </summary>
    ''' <param name="devInfo"></param>
    ''' <remarks></remarks>
    Public Shared Sub LinkDevices(ByRef devInfo() As DeviceInfo)

        If devInfo Is Nothing Then Return

        For Each dprep In devInfo
            dprep.LinkedParent = Nothing
            dprep.LinkedChildren = Nothing
        Next

        For Each de In devInfo

            For Each fe In devInfo
                If de.InstanceId.ToUpper.Trim = fe.Parent.ToUpper.Trim Then
                    fe.LinkedParent = de
                    If de._LinkedChildren Is Nothing Then
                        ReDim de._LinkedChildren(0)
                    Else
                        ReDim Preserve de._LinkedChildren(de._LinkedChildren.Count)
                    End If

                    de._LinkedChildren(de._LinkedChildren.Count - 1) = fe
                End If
            Next

        Next

    End Sub

    ''' <summary>
    ''' The linked parent DeviceInfo object.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property LinkedParent As DeviceInfo
        Get
            Return _LinkedParent
        End Get
        Friend Set(value As DeviceInfo)
            _LinkedParent = value
        End Set
    End Property

    ''' <summary>
    ''' Array of linked child DeviceInfo objects.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <TypeConverter(GetType(ExpandableObjectConverter))>
    Public Property LinkedChildren As DeviceInfo()
        Get
            Return _LinkedChildren
        End Get
        Friend Set(value As DeviceInfo())
            _LinkedChildren = value
        End Set
    End Property

    ''' <summary>
    ''' Returns the type of bus that the device is hosted on.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property BusType As BusType
        Get
            Return _BusType
        End Get
        Set(value As BusType)
            _BusType = value
        End Set
    End Property

    ''' <summary>
    ''' Retrieves the install date for the device.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property InstallDate As Date
        Get
            Return _InstallDate
        End Get
        Friend Set(value As Date)
            _InstallDate = value
        End Set
    End Property

    ''' <summary>
    ''' Retrieves any device characteristcs associated with the device.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Characteristics As DeviceCharacteristcs
        Get
            Return _Characteristics
        End Get
        Friend Set(value As DeviceCharacteristcs)
            _Characteristics = value
        End Set
    End Property

    ''' <summary>
    ''' Specifies whether or not the device must be removed safely.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SafeRemovalRequired As Boolean
        Get
            Return _SafeRemovalRequired
        End Get
        Friend Set(value As Boolean)
            _SafeRemovalRequired = value
        End Set
    End Property

    ''' <summary>
    ''' Specifies the removal policy of the device.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property RemovalPolicy As DeviceRemovalPolicy
        Get
            Return _RemovalPolicy
        End Get
        Friend Set(value As DeviceRemovalPolicy)
            _RemovalPolicy = value
        End Set
    End Property

    ''' <summary>
    ''' Returns all child instance ids of this device.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Children As String()
        Get
            Return _Children
        End Get
        Friend Set(value As String())
            _Children = value
        End Set
    End Property

    ''' <summary>
    ''' Returns the parent instance id of this device.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Parent As String
        Get
            Return _Parent
        End Get
        Friend Set(value As String)
            _Parent = value
        End Set
    End Property

    ''' <summary>
    ''' Get the physical device path that can be used by CreateFile
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property DevicePath As String
        Get
            Return _DevicePath
        End Get
        Friend Set(value As String)
            _DevicePath = value
        End Set
    End Property

    ''' <summary>
    ''' Gets the friendly name for the device
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Property FriendlyName As String
        Get
            Return _FriendlyName
        End Get
        Friend Set(value As String)
            _FriendlyName = value
        End Set
    End Property

    ''' <summary>
    ''' Gets the device instance id which is unique and can be passed to RunDLL property sheet functions and to match children with their parents.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property InstanceId As String
        Get
            Return _InstanceId
        End Get
        Friend Set(value As String)
            _InstanceId = value
        End Set
    End Property

    ''' <summary>
    ''' Gets all hardware location paths.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property LocationPaths As String()
        Get
            Return _LocationPaths
        End Get
        Friend Set(value As String())
            _LocationPaths = value
        End Set
    End Property

    ''' <summary>
    ''' Gets location information.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property LocationInfo As String
        Get
            Return _LocationInfo
        End Get
        Friend Set(value As String)
            _LocationInfo = value
        End Set
    End Property

    ''' <summary>
    ''' Gets all hardware Ids. Hardware Ids contain extractable data, including but not limited to
    ''' device vendor id, device product id and USB HID page implementations.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property HardwareIds As String()
        Get
            Return _HardwareIds
        End Get
        Friend Set(value As String())
            _HardwareIds = value
            ParseHw()
        End Set
    End Property

    ''' <summary>
    ''' Gets the binary physical path.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PhysicalPath As Byte()
        Get
            Return _PhysicalPath
        End Get
        Friend Set(value As Byte())
            _PhysicalPath = value
        End Set
    End Property

    ''' <summary>
    ''' Gets the UINumber
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property UINumber As Integer
        Get
            Return _UINumber
        End Get
        Friend Set(value As Integer)
            _UINumber = value
        End Set
    End Property

    ''' <summary>
    ''' Gets the description of the device.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Property Description As String
        Get
            Return _Description
        End Get
        Friend Set(value As String)
            _Description = value
        End Set
    End Property

    ''' <summary>
    ''' Gets the hardware container Id.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ContainerId As Guid
        Get
            Return _ContainerId
        End Get
        Friend Set(value As Guid)
            _ContainerId = value
        End Set
    End Property

    ''' <summary>
    ''' Gets the PDO name. This value is used to match physical disk device instances with their respective interfaces.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PDOName As String
        Get
            Return _PDOName
        End Get
        Friend Set(value As String)
            _PDOName = value
        End Set
    End Property

    ''' <summary>
    ''' Gets the manufacturer.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Property Manufacturer As String
        Get
            Return _Manufacturer
        End Get
        Friend Set(value As String)
            _Manufacturer = value
        End Set
    End Property

    ''' <summary>
    ''' Gets the Model Id.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Property ModelId As Guid
        Get
            Return _ModelId
        End Get
        Friend Set(value As Guid)
            _ModelId = value
        End Set
    End Property

    ''' <summary>
    ''' Get the Bus-Reported device description.
    ''' </summary>
    ''' <returns></returns>
    Public Overridable Property BusReportedDeviceDesc As String
        Get
            Return _BusReportedDeviceDesc
        End Get
        Set(value As String)
            _BusReportedDeviceDesc = value
        End Set
    End Property


    ''' <summary>
    ''' Gets the device class name.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Property ClassName As String
        Get
            Return _ClassName
        End Get
        Friend Set(value As String)
            _ClassName = value
        End Set
    End Property

    ''' <summary>
    ''' Gets the device interface class guid.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property DeviceInterfaceClassGuid As Guid
        Get
            Return _DeviceInterfaceClassGuid
        End Get
        Friend Set(value As Guid)
            _DeviceInterfaceClassGuid = value
        End Set
    End Property

    ''' <summary>
    ''' Gets the device class type.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Property DeviceInterfaceClass As DeviceInterfaceClassEnum
        Get
            Return _DeviceInterfaceClass
        End Get
        Friend Set(value As DeviceInterfaceClassEnum)
            _DeviceInterfaceClass = value
        End Set
    End Property

    ''' <summary>
    ''' Gets the device class guid.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property DeviceClassGuid As Guid
        Get
            Return _DeviceClassGuid
        End Get
        Friend Set(value As Guid)
            _DeviceClassGuid = value
        End Set
    End Property

    ''' <summary>
    ''' Gets the device class type.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Property DeviceClass As DeviceClassEnum
        Get
            Return _DeviceClass
        End Get
        Friend Set(value As DeviceClassEnum)
            _DeviceClass = value
        End Set
    End Property

    ''' <summary>
    ''' Returns a detailed description of the device class.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable ReadOnly Property DeviceClassDescription As String
        Get
            Return Utility.GetEnumDescription(_DeviceClass)
        End Get
    End Property

    ''' <summary>
    ''' Gets the device class icon.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Browsable(False)>
    Public Overridable Property DeviceClassIcon As System.Drawing.Icon
        Get
            Return _DeviceClassIcon
        End Get
        Friend Set(value As System.Drawing.Icon)
            _DeviceClassIcon = value
        End Set
    End Property

    ''' <summary>
    ''' Retrieve a WPF BitmapSource image for use in binding.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Browsable(False)>
    Public Overridable Property DeviceIcon As System.Windows.Media.Imaging.BitmapSource
        Get
            If _DeviceIcon Is Nothing Then
                If _DeviceClassIcon Is Nothing Then Return Nothing
                _DeviceIcon = MakeWPFImage(IconToTransparentBitmap(_DeviceClassIcon))
            End If

            Return _DeviceIcon
        End Get
        Friend Set(value As System.Windows.Media.Imaging.BitmapSource)
            _DeviceIcon = value
        End Set
    End Property

    ''' <summary>
    ''' Gets the device class description.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable ReadOnly Property ClassDescription As String
        Get
            Return Utility.GetEnumDescription(DeviceClass)
        End Get
    End Property

    ''' <summary>
    ''' Gets the vendor Id string, which may or may not be a number.  If it is a number, it is returned as a 4-character (WORD) hexadecimal string.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property VendorId As String
        Get
            If _VenderId IsNot Nothing Then Return _VenderId
            Return _Vid.ToString("X4")
        End Get
    End Property

    ''' <summary>
    ''' Gets the product Id string, which may or may not be a number.  If it is a number, it is returned as a 4-character (WORD) hexadecimal string.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property ProductId As String
        Get
            If _ProductId IsNot Nothing Then Return _ProductId
            Return _Pid.ToString("X4")
        End Get
    End Property

    ''' <summary>
    ''' Gets the vendor id raw number.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Vid As UShort
        Get
            Return _Vid
        End Get
    End Property

    ''' <summary>
    ''' Gets the product id raw number.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Pid As UShort
        Get
            Return _Pid
        End Get
    End Property

    ''' <summary>
    ''' Display the system device properties dialog page for this device.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ShowDevicePropertiesDialog(Optional hwnd As IntPtr = Nothing)
        OpenDeviceProperties(InstanceId, hwnd)
    End Sub

    ''' <summary>
    ''' Parses the hardware Id.
    ''' </summary>
    ''' <remarks></remarks>
    Protected Overridable Sub ParseHw()

        Dim s() As String = TextTools.Split(_HardwareIds(0), "\")
        If s Is Nothing OrElse s.Length < 2 Then Return

        Dim v() As String

        If (s(1).IndexOf(";") > 0) Then

            v = TextTools.Split(s(1), ";")

        Else

            v = TextTools.Split(s(1), "&")

        End If

        For Each vp As String In v

            Dim x() As String = TextTools.Split(vp, "_")
            If x.Length < 2 Then Continue For
            Select Case x(0)
                Case "VID"

                    If Not UShort.TryParse(x(1), Globalization.NumberStyles.AllowHexSpecifier, System.Globalization.CultureInfo.CurrentCulture, _Vid) Then
                        _VenderId = x(1)
                    End If

                Case "PID"
                    If Not UShort.TryParse(x(1), Globalization.NumberStyles.AllowHexSpecifier, System.Globalization.CultureInfo.CurrentCulture, _Pid) Then
                        _ProductId = x(1)
                    End If

                Case "VEN"
                    If Not UShort.TryParse(x(1), Globalization.NumberStyles.AllowHexSpecifier, System.Globalization.CultureInfo.CurrentCulture, _Vid) Then
                        _VenderId = x(1)
                    End If

                Case "DEV"
                    If Not UShort.TryParse(x(1), Globalization.NumberStyles.AllowHexSpecifier, System.Globalization.CultureInfo.CurrentCulture, _Pid) Then
                        _ProductId = x(1)
                    End If

            End Select
        Next

    End Sub

    ''' <summary>
    ''' Returns a description of the device. The default is the value of ToString().
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable ReadOnly Property UIDescription() As String
        Get
            If String.IsNullOrEmpty(Description) = False Then
                Return Description
            ElseIf String.IsNullOrEmpty(FriendlyName) = False Then
                Return FriendlyName
            Else
                Return ToString()
            End If
        End Get
    End Property

    ''' <summary>
    ''' Returns a string representation of this object
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function ToString() As String
        Dim d = Description,
            f = FriendlyName

        If d Is Nothing AndAlso f Is Nothing Then
            Return InstanceId
        ElseIf d Is Nothing AndAlso f IsNot Nothing Then
            Return f
        Else
            Return d
        End If
    End Function

    ''' <summary>
    ''' Tests for device equality in a meaningful way.
    ''' Every device has an instance id.
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function Equals(obj As Object) As Boolean
        Dim d As DeviceInfo = TryCast(obj, DeviceInfo)
        If d Is Nothing Then Return False

        Return d.InstanceId = InstanceId
    End Function

End Class

#End Region
