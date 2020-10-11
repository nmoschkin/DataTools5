Imports DataTools.Interop.Native

Interface IDeviceInfo

    ''' <summary>
    ''' Gets the device instance id which can be passed to RunDLL property sheet functions.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property InstanceId As String

    ''' <summary>
    ''' Returns the type of bus that the device is hosted on.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property BusType As BusType

    ''' <summary>
    ''' Get the physical device path that can be used by CreateFile
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property DevicePath As String

    ''' <summary>
    ''' Returns the parent instance id of this device.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property Parent As String

    ''' <summary>
    ''' Returns all children instance ids of this device.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property Children As String()

    ''' <summary>
    ''' Retrieves the install date for the device.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property InstallDate As Date
    ''' <summary>
    ''' Retrieves any device characteristcs associated with the device.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property Characteristics As DeviceCharacteristcs

    ''' <summary>
    ''' Specifies whether or not the device must be removed safely.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property SafeRemovalRequired As Boolean

    ''' <summary>
    ''' Specifies the removal policy of the device.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property RemovalPolicy As DeviceRemovalPolicy

    ''' <summary>
    ''' Gets all hardware location paths.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property LocationPaths As String()

    ''' <summary>
    ''' Gets location information.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property LocationInfo As String

    ''' <summary>
    ''' Gets all hardware Ids
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property HardwareIds As String()

    ''' <summary>
    ''' Gets the PDO name.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property PDOName As String

    ''' <summary>
    ''' Gets the device interface class guid.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property DeviceInterfaceClassGuid As Guid

    ''' <summary>
    ''' Gets the device class type.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property DeviceInterfaceClass As DeviceInterfaceClassEnum

    ''' <summary>
    ''' Gets the device class guid.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property DeviceClassGuid As Guid

    ''' <summary>
    ''' Gets the device class type.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property DeviceClass As DeviceClassEnum

    ''' <summary>
    ''' Gets the device class name.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property ClassName As String

    ''' <summary>
    ''' Gets the device class description.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property ClassDescription As String

    ''' <summary>
    ''' Retrieve a WPF BitmapSource image for use in binding.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property DeviceIcon As System.Windows.Media.Imaging.BitmapSource

    ''' <summary>
    ''' Gets the vendor Id hexadecimal string
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property VendorId As String

    ''' <summary>
    ''' Gets the product Id hexadecimal string
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property ProductId As String

    ''' <summary>
    ''' Display the system device properties dialog page for this device.
    ''' </summary>
    ''' <remarks></remarks>
    Sub ShowDevicePropertiesDialog(Optional hwnd As IntPtr = Nothing)

    ''' <summary>
    ''' Retrieves a string suitable for display in a user interface.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property UIDescription As String

    ''' <summary>
    ''' Gets the friendly name for the device
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property FriendlyName As String

    ''' <summary>
    ''' Gets the description of the device.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property Description As String

    ''' <summary>
    ''' Gets the manufacturer.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property Manufacturer As String

End Interface

Public Class DeviceInfoBase
    Implements IDeviceInfo

    Friend _InstanceId As String

    Friend _BusType As BusType

    Friend _DevicePath As String

    Friend _Parent As String

    Friend _Children As String()

    Friend _InstallDate As Date

    Friend _Characteristics As DeviceCharacteristcs

    Friend _SafeRemovalRequired As Boolean

    Friend _RemovalPolicy As DeviceRemovalPolicy

    Friend _LocationPaths As String()

    Friend _LocationInfo As String

    Friend _HardwareIds As String()

    Friend _PDOName As String

    Friend _DeviceInterfaceClassGuid As Guid

    Friend _DeviceInterfaceClass As DeviceInterfaceClassEnum

    Friend _DeviceClassGuid As Guid

    Friend _DeviceClass As DeviceClassEnum

    Friend _ClassName As String

    Friend _ClassDescription As String

    Friend _DeviceIcon As System.Windows.Media.Imaging.BitmapSource

    Friend _VendorId As String

    Friend _ProductId As String

    Friend _UIDescription As String

    Friend _FriendlyName As String

    Friend _Description As String

    Friend _Manufacturer As String

    Public ReadOnly Property BusType As BusType Implements IDeviceInfo.BusType
        Get
            Return _BusType
        End Get
    End Property

    Public ReadOnly Property Characteristics As DeviceCharacteristcs Implements IDeviceInfo.Characteristics
        Get
            Return _Characteristics
        End Get
    End Property

    Public ReadOnly Property Children As String() Implements IDeviceInfo.Children
        Get
            Return _Children
        End Get
    End Property

    Public ReadOnly Property ClassDescription As String Implements IDeviceInfo.ClassDescription
        Get
            Return _ClassDescription
        End Get
    End Property

    Public ReadOnly Property ClassName As String Implements IDeviceInfo.ClassName
        Get
            Return _ClassName
        End Get
    End Property

    Public ReadOnly Property Description As String Implements IDeviceInfo.Description
        Get
            Return _Description
        End Get
    End Property

    Public ReadOnly Property DeviceClass As DeviceClassEnum Implements IDeviceInfo.DeviceClass
        Get
            Return _DeviceClass
        End Get
    End Property

    Public ReadOnly Property DeviceClassGuid As Guid Implements IDeviceInfo.DeviceClassGuid
        Get
            Return _DeviceClassGuid
        End Get
    End Property

    Public ReadOnly Property DeviceIcon As System.Windows.Media.Imaging.BitmapSource Implements IDeviceInfo.DeviceIcon
        Get
            Return _DeviceIcon
        End Get
    End Property

    Public ReadOnly Property DeviceInterfaceClass As DeviceInterfaceClassEnum Implements IDeviceInfo.DeviceInterfaceClass
        Get
            Return _DeviceInterfaceClass
        End Get
    End Property

    Public ReadOnly Property DeviceInterfaceClassGuid As Guid Implements IDeviceInfo.DeviceInterfaceClassGuid
        Get
            Return _DeviceInterfaceClassGuid
        End Get
    End Property

    Public ReadOnly Property DevicePath As String Implements IDeviceInfo.DevicePath
        Get
            Return _DevicePath
        End Get
    End Property

    Public ReadOnly Property FriendlyName As String Implements IDeviceInfo.FriendlyName
        Get
            Return _FriendlyName
        End Get
    End Property

    Public ReadOnly Property HardwareIds As String() Implements IDeviceInfo.HardwareIds
        Get
            Return _HardwareIds
        End Get
    End Property

    Public ReadOnly Property InstallDate As Date Implements IDeviceInfo.InstallDate
        Get
            Return _InstallDate
        End Get
    End Property

    Public ReadOnly Property InstanceId As String Implements IDeviceInfo.InstanceId
        Get
            Return _InstanceId
        End Get
    End Property

    Public ReadOnly Property LocationInfo As String Implements IDeviceInfo.LocationInfo
        Get
            Return _LocationInfo
        End Get
    End Property

    Public ReadOnly Property LocationPaths As String() Implements IDeviceInfo.LocationPaths
        Get
            Return _LocationPaths
        End Get
    End Property

    Public ReadOnly Property Manufacturer As String Implements IDeviceInfo.Manufacturer
        Get
            Return _Manufacturer
        End Get
    End Property

    Public ReadOnly Property Parent As String Implements IDeviceInfo.Parent
        Get
            Return _Parent
        End Get
    End Property

    Public ReadOnly Property PDOName As String Implements IDeviceInfo.PDOName
        Get
            Return _PDOName
        End Get
    End Property

    Public ReadOnly Property ProductId As String Implements IDeviceInfo.ProductId
        Get
            Return _ProductId
        End Get
    End Property

    Public ReadOnly Property RemovalPolicy As DeviceRemovalPolicy Implements IDeviceInfo.RemovalPolicy
        Get
            Return _RemovalPolicy
        End Get
    End Property

    Public ReadOnly Property SafeRemovalRequired As Boolean Implements IDeviceInfo.SafeRemovalRequired
        Get
            Return _SafeRemovalRequired
        End Get
    End Property

    Public Sub ShowDevicePropertiesDialog(Optional hwnd As IntPtr = Nothing) Implements IDeviceInfo.ShowDevicePropertiesDialog
        OpenDeviceProperties(InstanceId, hwnd)
    End Sub

    Public ReadOnly Property UIDescription As String Implements IDeviceInfo.UIDescription
        Get
            Return _UIDescription
        End Get
    End Property

    Public ReadOnly Property VendorId As String Implements IDeviceInfo.VendorId
        Get
            Return _VendorId
        End Get
    End Property

End Class