'' ************************************************* ''
'' DataTools Visual Basic Utility Library - Interop
''
'' Module: AdapterCollection
''         Encapsulates the network interface environment
''         of the currently running system.
''
'' Copyright (C) 2011-2020 Nathan Moschkin
'' All Rights Reserved
''
'' Licensed Under the Microsoft Public License   
'' ************************************************* ''

Imports DataTools.Interop.Native
Imports System.Security
Imports System.Runtime.InteropServices
Imports CoreCT.Memory
Imports System.ComponentModel
Imports System.Collections.ObjectModel
Imports DataTools.Interop.Desktop
Imports CoreCT.Text

Namespace Network


    '''' <summary>
    '''' System network adapter information thin wrappers.
    '''' </summary>
    '''' <remarks>
    '''' The observable collection is more suitable for use as a WPF data source.
    '''' 
    '''' The NetworkAdapter class cannot be created independently.
    '''' 
    '''' For most usage cases, the AdaptersCollection object should be used.
    '''' 
    '''' The <see cref="NetworkAdapters"/> collection is also a viable option
    '''' and possibly of a lighter variety.
    '''' </remarks>
    'Public Module NetworkWrappers

#Region "Managed Wrapper Enumerator For All Adapters"

    ''' <summary>
    ''' Managed wrapper collection for all adapters.
    ''' </summary>
    <Category("Devices"), Description("Network adapter collection."), Browsable(True)>
    Public Class NetworkAdapters
        Implements ICollection(Of IP_ADAPTER_ADDRESSES), IDisposable

        Private _Adapters() As IP_ADAPTER_ADDRESSES
        Private _origPtr As MemPtr
        Private _Col As New Collection

        ''' <summary>
        ''' Returns an array of <see cref="IP_ADAPTER_ADDRESSES" /> structures 
        ''' </summary>
        ''' <returns></returns>
        <Category("Devices"), Description("Network adapter collection."), Browsable(True)>
        Public Property Adapters As IP_ADAPTER_ADDRESSES()
            Get
                Return _Adapters
            End Get
            Set(value As IP_ADAPTER_ADDRESSES())
                Clear()
                _Adapters = value
            End Set
        End Property

        Public Sub New()
            Refresh()
        End Sub

        ''' <summary>
        ''' Refresh the list by calling <see cref="DevEnumPublic.EnumerateNetworkDevices()"/>
        ''' </summary>
        Public Sub Refresh()
            Free()
            Dim di As DeviceInfo() = EnumerateNetworkDevices()

            _Adapters = GetAdapters(_origPtr, True)
            For Each adap In _Adapters
                Dim newp As New NetworkAdapter(adap)

                For Each de In di
                    If de.Description = adap.Description Then
                        newp.DeviceInfo = de
                        Exit For
                    End If
                Next
                _Col.Add(newp)
            Next
        End Sub

        <Browsable(True), Category("Collections")>
        Public Property AdapterCollection As Collection
            Get
                Return _Col
            End Get
            Set(value As Collection)
                _Col = value
            End Set
        End Property

        ''' <summary>
        ''' Returns the <see cref="IP_ADAPTER_ADDRESSES" /> structure at the specified index 
        ''' </summary>
        ''' <param name="index">Index of item to return.</param>
        ''' <returns><see cref="IP_ADAPTER_ADDRESSES" /> structure</returns>
        Default Public ReadOnly Property Item(index As Integer) As IP_ADAPTER_ADDRESSES
            Get
                Return _Adapters(index)
            End Get
        End Property

        Public Function GetEnumerator() As IEnumerator(Of IP_ADAPTER_ADDRESSES) Implements IEnumerable(Of IP_ADAPTER_ADDRESSES).GetEnumerator
            Return New NetAdapterEnum(Me)
        End Function

        Public Function GetEnumerator1() As IEnumerator Implements IEnumerable.GetEnumerator
            Return New NetAdapterEnum(Me)
        End Function

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    _Adapters = Nothing
                End If

                Free()
            End If
            Me.disposedValue = True
        End Sub

        Protected Overrides Sub Finalize()
            Dispose(False)
            MyBase.Finalize()
        End Sub

        Protected Sub Free()
            If _origPtr.Handle <> IntPtr.Zero Then
                _origPtr.Free(True)
            End If
            _Adapters = Nothing
            _Col.Clear()
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub

#End Region

        Public Sub Add(item As IP_ADAPTER_ADDRESSES) Implements ICollection(Of IP_ADAPTER_ADDRESSES).Add
            Throw New AccessViolationException("Cannot add items to a system managed list")
        End Sub

        Public Sub Clear() Implements ICollection(Of IP_ADAPTER_ADDRESSES).Clear
            _Adapters = Nothing
            Free()
        End Sub

        Public Function Contains(item As IP_ADAPTER_ADDRESSES) As Boolean Implements ICollection(Of IP_ADAPTER_ADDRESSES).Contains
            If _Adapters Is Nothing Then Return False
            For Each aa In _Adapters
                If aa.NetworkGuid = item.NetworkGuid Then Return True
            Next

            Return False
        End Function

        Public Sub CopyTo(array() As IP_ADAPTER_ADDRESSES, arrayIndex As Integer) Implements ICollection(Of IP_ADAPTER_ADDRESSES).CopyTo
            If _Adapters Is Nothing Then
                Throw New NullReferenceException
            End If
            _Adapters.CopyTo(array, arrayIndex)
        End Sub

        Public ReadOnly Property Count As Integer Implements ICollection(Of IP_ADAPTER_ADDRESSES).Count
            Get
                If _Adapters Is Nothing Then Return 0 Else Return _Adapters.Count
            End Get
        End Property

        Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of IP_ADAPTER_ADDRESSES).IsReadOnly
            Get
                Return True
            End Get
        End Property

        Public Function Remove(item As IP_ADAPTER_ADDRESSES) As Boolean Implements ICollection(Of IP_ADAPTER_ADDRESSES).Remove
            Return False
        End Function

    End Class

    Public Class NetAdapterEnum
        Implements IEnumerator(Of IP_ADAPTER_ADDRESSES)

        Private pos As Integer = -1
        Private subj As NetworkAdapters

        Friend Sub New(subject As NetworkAdapters)
            subj = subject
        End Sub

        Public ReadOnly Property Current As IP_ADAPTER_ADDRESSES Implements IEnumerator(Of IP_ADAPTER_ADDRESSES).Current
            Get
                Return subj(pos)
            End Get
        End Property

        Public ReadOnly Property Current1 As Object Implements IEnumerator.Current
            Get
                Return subj(pos)
            End Get
        End Property

        Public Function MoveNext() As Boolean Implements IEnumerator.MoveNext
            pos += 1
            If pos >= subj.Count Then Return False
            Return True
        End Function

        Public Sub Reset() Implements IEnumerator.Reset
            pos = -1
        End Sub

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    subj = Nothing
                    pos = -1
                End If

            End If
            Me.disposedValue = True
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

    End Class

#End Region

#Region "Observable Collection"


    ''' <summary>
    ''' A managed observable collection wrapper of NetworkAdapter wrapper objects.  This collection wraps the 
    ''' Windows Network Interface Api.  All objects are thin wrappers around the original unmanaged
    ''' memory objects. 
    ''' </summary>
    ''' <remarks>
    ''' The array memory is allocated as one very long block by the GetAdapters function.
    ''' We keep it in this collection and the members in the unmanaged memory source serve 
    ''' as the backbone for the collection of NetworkAdapter objects.
    ''' 
    ''' For this reason, the NetworkAdapter object cannot be created publically, as the 
    ''' AdaptersCollection object is managing a single block of unmanaged memory for the entire collection. 
    ''' Therefore, there can be no singleton instances of the NetworkAdapter object.  
    ''' 
    ''' We will use Finalize() to free this (rather large) resource when this class is destroyed.
    ''' </remarks>
    <SecurityCritical()>
    Public Class AdaptersCollection
        Implements IDisposable

        Private _Col As New ObservableCollection(Of NetworkAdapter)
        Private _Adapters() As IP_ADAPTER_ADDRESSES
        Private _origPtr As MemPtr

        Public ReadOnly Property Collection As ObservableCollection(Of NetworkAdapter)
            Get
                Return _Col
            End Get
        End Property

        Public Sub New()
            MyBase.New()
            Refresh()
        End Sub

        Public Sub Refresh()
            Free()
            _Adapters = Nothing
            _Col = New ObservableCollection(Of NetworkAdapter)

            Dim di As DeviceInfo() = EnumerateNetworkDevices()

            '' Get the array of unmanaged IP_ADAPTER_ADDRESSES structures 
            _Adapters = GetAdapters(_origPtr, True)

            For Each adap In _Adapters
                Dim newp As New NetworkAdapter(adap)

                For Each de In di
                    If de.Description = adap.Description OrElse de.FriendlyName = adap.FriendlyName OrElse de.FriendlyName = adap.Description OrElse de.Description = adap.FriendlyName Then
                        newp.DeviceInfo = de
                        _Col.Add(newp)
                        Exit For
                    End If
                Next
            Next

        End Sub

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    _Adapters = Nothing
                End If

                '' free up the unmanaged memory and release the memory pressure on the garbage collector.
                _origPtr.Free(True)
            End If
            Me.disposedValue = True
        End Sub

        Protected Overrides Sub Finalize()
            Dispose(False)
            MyBase.Finalize()
        End Sub

        Protected Sub Free()
            '' free up the unmanaged memory and release the memory pressure on the garbage collector.
            _origPtr.Free(True)
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub

#End Region

    End Class

#End Region

#Region "Wrapper Class"

    ''' <summary>
    ''' Managed wrapper class for the native network adapter information API.
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class NetworkAdapter
        Implements IDisposable, INotifyPropertyChanged

        Private _nativeStruct As IP_ADAPTER_ADDRESSES
        Private _deviceInfo As DeviceInfo

        Private _canShowNet As Boolean
        Private _canShowDev As Boolean

        Private _Icon As System.Windows.Media.Imaging.BitmapSource

        '' This class should not be created outside of the context of AdaptersCollection.
        Friend Sub New(nativeObject As IP_ADAPTER_ADDRESSES)

            '' Store the native object.
            _nativeStruct = nativeObject

            '' First thing's first... let's get the icon for the object from its parsing name.
            '' Which is magically the parsing name of the network device list and the adapter's GUID name.
            Dim s As String = ("::{7007ACC7-3202-11D1-AAD2-00805FC1270E}\" & AdapterName)
            Dim mm As New MemPtr

            SHParseDisplayName(s, IntPtr.Zero, mm, 0, 0)

            If mm.Handle <> IntPtr.Zero Then
                '' Get a WPFImage 
                _Icon = MakeWPFImage(GetItemIcon(mm, CType(SHIL_EXTRALARGE, SystemIconSizes)))
                mm.Free()

                _canShowNet = True
            Else
                _canShowNet = False
            End If
        End Sub


        Protected Overloads Sub OnPropertyChanged(e As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(e))
        End Sub

        Protected Overloads Sub OnPropertyChanged(e As PropertyChangedEventArgs)
            RaiseEvent PropertyChanged(Me, e)
        End Sub


        ''' <summary>
        ''' Is true if the device dialog can be displayed for this adapter.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property CanShowDeviceDialog As Boolean
            Get
                Return _deviceInfo IsNot Nothing
            End Get
        End Property

        ''' <summary>
        ''' Returns a BitmapSource of the device's icon.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(False)>
        Public ReadOnly Property DeviceIcon As System.Windows.Media.Imaging.BitmapSource
            Get
                Return _Icon
            End Get
        End Property

        ''' <summary>
        ''' Is true if this device can display the network dialog.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property CanShowNetworkDialogs As Boolean
            Get
                Return _canShowNet
            End Get
        End Property

        ''' <summary>
        ''' Raise the device properties dialog.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub ShowDevicePropertiesDialog()
            If _deviceInfo Is Nothing Then Return
            _deviceInfo.ShowDevicePropertiesDialog()
        End Sub

        ''' <summary>
        ''' Raise the connection properties dialog.  This may throw the UAC.
        ''' </summary>
        ''' <param name="hwnd"></param>
        ''' <remarks></remarks>
        Public Sub ShowConnectionPropertiesDialog(Optional hwnd As IntPtr = Nothing)
            If _deviceInfo Is Nothing Then Return

            Dim shex As New SHELLEXECUTEINFO
            shex.cbSize = Marshal.SizeOf(shex)
            shex.nShow = SW_SHOW
            shex.hInstApp = Process.GetCurrentProcess.Handle
            shex.hWnd = hwnd
            shex.lpVerb = "properties"

            '' Set the parsing name exactly this way.
            shex.lpDirectory = "::{7007ACC7-3202-11D1-AAD2-00805FC1270E}"
            shex.lpFile = "::{7007ACC7-3202-11D1-AAD2-00805FC1270E}\" & AdapterName

            shex.fMask = SEE_MASK_ASYNCOK Or SEE_MASK_FLAG_DDEWAIT Or SEE_MASK_UNICODE
            ShellExecuteEx(shex)
        End Sub

        ''' <summary>
        ''' Raise the connection status dialog.
        ''' </summary>
        ''' <param name="hwnd"></param>
        ''' <remarks></remarks>
        Public Sub ShowNetworkStatusDialog(Optional hwnd As IntPtr = Nothing)
            Dim shex As New SHELLEXECUTEINFO
            shex.cbSize = Marshal.SizeOf(shex)
            shex.hWnd = hwnd
            shex.nShow = SW_SHOW
            shex.lpVerb = ""
            shex.hInstApp = Process.GetCurrentProcess.Handle
            shex.lpDirectory = "::{7007ACC7-3202-11D1-AAD2-00805FC1270E}"
            shex.lpFile = "::{7007ACC7-3202-11D1-AAD2-00805FC1270E}\" & AdapterName
            shex.fMask = SEE_MASK_ASYNCOK Or SEE_MASK_FLAG_DDEWAIT Or SEE_MASK_UNICODE
            ShellExecuteEx(shex)
        End Sub

        ''' <summary>
        ''' Retrieves the DeviceInfo object for the system device instance of the network adapter.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(True), TypeConverter(GetType(ExpandableObjectConverter))>
        Public Property DeviceInfo As DeviceInfo
            Get
                Return _deviceInfo
            End Get
            Friend Set(value As DeviceInfo)
                _deviceInfo = value

                If _Icon Is Nothing Then
                    '' if the adapter doesn't have its own icon, the device class surely will.
                    '' let's see if we can get an icon from the device!

                    If _deviceInfo.DeviceIcon IsNot Nothing Then
                        _Icon = _deviceInfo.DeviceIcon
                        OnPropertyChanged("DeviceIcon")
                    End If

                End If
            End Set
        End Property

        ''' <summary>
        ''' The GUID adapter name.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(True)>
        Public ReadOnly Property AdapterName() As String
            Get
                AdapterName = _nativeStruct.AdapterName
            End Get
        End Property

        ''' <summary>
        ''' The first IP address of this device.  Usually IPv6. The IPv4 address resides at FirstUnicastAddress.Next.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(True)>
        Public ReadOnly Property FirstUnicastAddress() As LPADAPTER_UNICAST_ADDRESS
            Get
                FirstUnicastAddress = _nativeStruct.FirstUnicastAddress
            End Get
        End Property

        ''' <summary>
        ''' The first Anycast address.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(True)>
        Public ReadOnly Property FirstAnycastAddress() As LPADAPTER_MULTICAST_ADDRESS
            Get
                FirstAnycastAddress = _nativeStruct.FirstAnycastAddress
            End Get
        End Property

        ''' <summary>
        ''' First multicast address.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(True)>
        Public ReadOnly Property FirstMulticastAddress() As LPADAPTER_MULTICAST_ADDRESS
            Get
                FirstMulticastAddress = _nativeStruct.FirstMulticastAddress
            End Get
        End Property

        ''' <summary>
        ''' First Dns server address.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(True)>
        Public ReadOnly Property FirstDnsServerAddress() As LPADAPTER_MULTICAST_ADDRESS
            Get
                FirstDnsServerAddress = _nativeStruct.FirstDnsServerAddress
            End Get
        End Property

        ''' <summary>
        ''' Dns Suffix. This is typically the name of your ISP's internal domain if you are connected to an ISP.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(True)>
        Public ReadOnly Property DnsSuffix() As String
            Get
                DnsSuffix = _nativeStruct.DnsSuffix
            End Get
        End Property

        ''' <summary>
        ''' This is always the friendly name of the device instance of the network adapter.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(True)>
        Public ReadOnly Property Description() As String
            Get
                Description = _nativeStruct.Description
            End Get
        End Property

        ''' <summary>
        ''' Friendly name of the network connection (Ethernet 1, WiFi 2, etc).
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(True)>
        Public ReadOnly Property FriendlyName() As String
            Get
                FriendlyName = _nativeStruct.FriendlyName
            End Get
        End Property

        ''' <summary>
        ''' The MAC address of the adapter.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(True)>
        Public ReadOnly Property PhysicalAddress() As MACADDRESS
            Get
                PhysicalAddress = _nativeStruct.PhysicalAddress
            End Get
        End Property

        <Browsable(True)>
        Public ReadOnly Property PhysicalAddressLength() As UInt32
            Get
                PhysicalAddressLength = _nativeStruct.PhysicalAddressLength
            End Get
        End Property

        ''' <summary>
        ''' Adapter configuration flags and capabilities.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(True)>
        Public ReadOnly Property Flags() As IPAdapterAddressesFlags
            Get
                Flags = _nativeStruct.Flags
            End Get
        End Property

        ''' <summary>
        ''' Maximum transmission unit, in bytes.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(True)>
        Public ReadOnly Property Mtu() As Int32
            Get
                Mtu = _nativeStruct.Mtu
            End Get
        End Property


        ''' <summary>
        ''' Interface type.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(True)>
        Public ReadOnly Property IfType() As IFTYPE
            Get
                IfType = _nativeStruct.IfType
            End Get
        End Property

        ''' <summary>
        ''' Operational status.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(True)>
        Public ReadOnly Property OperStatus() As IF_OPER_STATUS
            Get
                OperStatus = _nativeStruct.OperStatus
            End Get
        End Property

        ''' <summary>
        ''' Ipv6 IF Index
        ''' </summary>
        ''' <returns></returns>
        <Browsable(True)>
        Public ReadOnly Property Ipv6IfIndex() As UInt32
            Get
                Ipv6IfIndex = _nativeStruct.Ipv6IfIndex
            End Get
        End Property


        ''' <summary>
        ''' Zone Indices
        ''' </summary>
        ''' <returns></returns>
        <Browsable(True)>
        Public ReadOnly Property ZoneIndices() As UInt32()
            Get
                ZoneIndices = _nativeStruct.ZoneIndices
            End Get
        End Property


        ''' <summary>
        ''' Get the first <see cref="LPIP_ADAPTER_PREFIX" />
        ''' </summary>
        ''' <returns></returns>
        <Browsable(True)>
        Public ReadOnly Property FirstPrefix() As LPIP_ADAPTER_PREFIX
            Get
                FirstPrefix = _nativeStruct.FirstPrefix
            End Get
        End Property

        ''' <summary>
        ''' Current upstream link speed (in bytes).
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(True)>
        Public ReadOnly Property TransmitLinkSpeed() As FriendlySpeedLong
            Get
                TransmitLinkSpeed = _nativeStruct.TransmitLinkSpeed
            End Get
        End Property

        ''' <summary>
        ''' Current downstream link speed (in bytes).
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(True)>
        Public ReadOnly Property ReceiveLinkSpeed() As FriendlySpeedLong
            Get
                ReceiveLinkSpeed = _nativeStruct.ReceiveLinkSpeed
            End Get
        End Property


        ''' <summary>
        ''' First WINS server address.
        ''' </summary>
        ''' <returns></returns>
        <Browsable(True)>
        Public ReadOnly Property FirstWinsServerAddress() As LPADAPTER_MULTICAST_ADDRESS
            Get
                FirstWinsServerAddress = _nativeStruct.FirstWinsServerAddress
            End Get
        End Property

        ''' <summary>
        ''' First gateway address.
        ''' </summary>
        ''' <returns></returns>
        <Browsable(True)>
        Public ReadOnly Property FirstGatewayAddress() As LPADAPTER_MULTICAST_ADDRESS
            Get
                FirstGatewayAddress = _nativeStruct.FirstGatewayAddress
            End Get
        End Property


        ''' <summary>
        ''' Ipv4 Metric
        ''' </summary>
        ''' <returns></returns>
        <Browsable(True)>
        Public ReadOnly Property Ipv4Metric() As UInt32
            Get
                Ipv4Metric = _nativeStruct.Ipv4Metric
            End Get
        End Property

        ''' <summary>
        ''' Ipv6 Metric
        ''' </summary>
        ''' <returns></returns>
        <Browsable(True)>
        Public ReadOnly Property Ipv6Metric() As UInt32
            Get
                Ipv6Metric = _nativeStruct.Ipv6Metric
            End Get
        End Property

        ''' <summary>
        ''' LUID
        ''' </summary>
        ''' <returns>A <see cref="LUID"/> structure</returns>
        <Browsable(True)>
        Public ReadOnly Property Luid() As LUID
            Get
                Luid = _nativeStruct.Luid
            End Get
        End Property

        ''' <summary>
        ''' Ipv4 DHCP server
        ''' </summary>
        ''' <returns>A <see cref="SOCKET_ADDRESS"/> structure</returns>
        <Browsable(True)>
        Public ReadOnly Property Dhcp4Server() As SOCKET_ADDRESS
            Get
                Dhcp4Server = _nativeStruct.Dhcp4Server
            End Get
        End Property

        ''' <summary>
        ''' Compartment ID
        ''' </summary>
        ''' <returns><see cref="UInt32"/></returns>

        <Browsable(True)>
        Public ReadOnly Property CompartmentId() As UInt32
            Get
                CompartmentId = _nativeStruct.CompartmentId
            End Get
        End Property

        ''' <summary>
        ''' Network <see cref="Guid"/>
        ''' </summary>
        ''' <returns>A <see cref="Guid"/></returns>
        <Browsable(True)>
        Public ReadOnly Property NetworkGuid() As Guid
            Get
                NetworkGuid = _nativeStruct.NetworkGuid
            End Get
        End Property

        ''' <summary>
        ''' Network connection type
        ''' </summary>
        ''' <returns>A <see cref="NET_IF_CONNECTION_TYPE"/> structure</returns>
        <Browsable(True)>
        Public ReadOnly Property ConnectionType() As NET_IF_CONNECTION_TYPE
            Get
                ConnectionType = _nativeStruct.ConnectionType
            End Get
        End Property

        ''' <summary>
        ''' Tunnel type
        ''' </summary>
        ''' <returns>A <see cref="TUNNEL_TYPE"/> value.</returns>
        <Browsable(True)>
        Public ReadOnly Property TunnelType() As TUNNEL_TYPE
            Get
                TunnelType = _nativeStruct.TunnelType
            End Get
        End Property

        ''' <summary>
        ''' DHCP v6 server
        ''' </summary>
        ''' <returns></returns>
        <Browsable(True)>
        Public ReadOnly Property Dhcpv6Server() As SOCKET_ADDRESS
            Get
                Dhcpv6Server = _nativeStruct.Dhcpv6Server
            End Get
        End Property


        ''' <summary>
        ''' DHCP v6 Client DUID
        ''' </summary>
        ''' <returns></returns>
        <Browsable(True)>
        Public ReadOnly Property Dhcpv6ClientDuid() As Byte()
            Get
                Dhcpv6ClientDuid = _nativeStruct.Dhcpv6ClientDuid
            End Get
        End Property

        ''' <summary>
        ''' DHCP v6 Client DUID Length
        ''' </summary>
        ''' <returns></returns>
        <Browsable(True)>
        Public ReadOnly Property Dhcpv6ClientDuidLength() As UInt32
            Get
                Dhcpv6ClientDuidLength = _nativeStruct.Dhcpv6ClientDuidLength
            End Get
        End Property

        ''' <summary>
        ''' DHCP v6 AIID
        ''' </summary>
        ''' <returns></returns>
        <Browsable(True)>
        Public ReadOnly Property Dhcpv6Iaid() As UInt32
            Get
                Dhcpv6Iaid = _nativeStruct.Dhcpv6Iaid
            End Get
        End Property

        ''' <summary>
        ''' First DNS Suffix
        ''' </summary>
        ''' <returns></returns>
        <Browsable(True)>
        Public ReadOnly Property FirstDnsSuffix() As LPIP_ADAPTER_DNS_SUFFIX
            Get
                FirstDnsSuffix = _nativeStruct.FirstDnsSuffix
            End Get
        End Property

        ''' <summary>
        ''' Returns the adapter's friendly name
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function ToString() As String
            ToString = FriendlyName
        End Function

#Region "Operators"

        '''' <summary>
        '''' Explicit cast from <see cref="IP_ADAPTER_ADDRESSES"/> to <see cref="NetworkAdapter"/> 
        '''' </summary>
        '''' <param name="item1"></param>
        '''' <returns></returns>
        'Shared Narrowing Operator CType(item1 As IP_ADAPTER_ADDRESSES) As NetworkAdapter
        '    Return New NetworkAdapter(item1)
        'End Operator

        '''' <summary>
        '''' Explicit cast from <see cref="NetworkAdapter"/> to <see cref="IP_ADAPTER_ADDRESSES"/>
        '''' </summary>
        '''' <param name="item1"></param>
        '''' <returns></returns>
        'Shared Narrowing Operator CType(item1 As NetworkAdapter) As IP_ADAPTER_ADDRESSES
        '    Return item1._nativeStruct
        'End Operator

#End Region

        '' Disposable support reserved for possible future use.  This object is managed
        '' by AdaptersCollection.

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Sub Dispose(disposing As Boolean)
            Me.disposedValue = True
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

        Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged
    End Class

#End Region

    ''    End Module

End Namespace