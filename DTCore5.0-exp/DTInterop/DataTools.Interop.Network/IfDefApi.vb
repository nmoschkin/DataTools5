'' ************************************************* ''
'' DataTools Visual Basic Utility Library - Interop
''
'' Module: IfDefApi
''         The almighty network interface native API.
''         Some enum documentation comes from the MSDN.
''
'' (and an exercise in creative problem solving and data-structure marshaling.)
''
'' Copyright (C) 2011-2020 Nathan Moschkin
'' All Rights Reserved
''
'' Licensed Under the Microsoft Public License   
'' ************************************************* ''


Imports System
Imports System.IO
Imports System.Net
Imports System.Runtime.InteropServices
Imports System.ComponentModel
Imports DataTools.Interop.Native
Imports CoreCT.Memory

Namespace Network

#Region "Address Family"

    ''' <summary>
    ''' Network adapter address families.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum AddressFamily As UShort

        ''' <summary>
        ''' unspecified
        ''' </summary>
        ''' <remarks></remarks>
        AfUnspecified = 0

        ''' <summary>
        ''' local to host (pipes, portals)
        ''' </summary>
        ''' <remarks></remarks>
        AfUNIX = 1

        ''' <summary>
        ''' internetwork: UDP, TCP, etc.
        ''' </summary>
        ''' <remarks></remarks>
        AfInet = 2

        ''' <summary>
        ''' arpanet imp addresses
        ''' </summary>
        ''' <remarks></remarks>
        AfIMPLINK = 3

        ''' <summary>
        ''' pup protocols: e.g. BSP
        ''' </summary>
        ''' <remarks></remarks>
        AfPUP = 4

        ''' <summary>
        ''' mit CHAOS protocols
        ''' </summary>
        ''' <remarks></remarks>
        AfCHAOS = 5

        ''' <summary>
        ''' XEROX NS protocols
        ''' </summary>
        ''' <remarks></remarks>
        AfNS = 6

        ''' <summary>
        ''' IPX protocols: IPX, SPX, etc.
        ''' </summary>
        ''' <remarks></remarks>
        AfIPX = AfNS

        ''' <summary>
        ''' ISO protocols
        ''' </summary>
        ''' <remarks></remarks>
        AfISO = 7

        ''' <summary>
        ''' OSI is ISO
        ''' </summary>
        ''' <remarks></remarks>
        AfOSI = AfISO

        ''' <summary>
        ''' european computer manufacturers
        ''' </summary>
        ''' <remarks></remarks>
        AfECMA = 8

        ''' <summary>
        ''' datakit protocols
        ''' </summary>
        ''' <remarks></remarks>
        AfDataKit = 9

        ''' <summary>
        ''' CCITT protocols, X.25 etc
        ''' </summary>
        ''' <remarks></remarks>
        AfCCITT = 10

        ''' <summary>
        ''' IBM SNA
        ''' </summary>
        ''' <remarks></remarks>
        AfSNA = 11

        ''' <summary>
        ''' DECnet
        ''' </summary>
        ''' <remarks></remarks>
        AfDECnet = 12

        ''' <summary>
        ''' Direct data link interface
        ''' </summary>
        ''' <remarks></remarks>
        AfDLI = 13

        ''' <summary>
        ''' LAT
        ''' </summary>
        ''' <remarks></remarks>
        AfLAT = 14

        ''' <summary>
        ''' NSC Hyperchannel
        ''' </summary>
        ''' <remarks></remarks>
        AfHYLINK = 15

        ''' <summary>
        ''' AppleTalk
        ''' </summary>
        ''' <remarks></remarks>
        AfAppleTalk = 16

        ''' <summary>
        ''' NetBios-style addresses
        ''' </summary>
        ''' <remarks></remarks>
        AfNetBios = 17

        ''' <summary>
        ''' VoiceView
        ''' </summary>
        ''' <remarks></remarks>
        AfVoiceView = 18

        ''' <summary>
        ''' Protocols from Firefox
        ''' </summary>
        ''' <remarks></remarks>
        AfFirefox = 19

        ''' <summary>
        ''' Somebody is using this!
        ''' </summary>
        ''' <remarks></remarks>
        AfUnknown1 = 20

        ''' <summary>
        ''' Banyan
        ''' </summary>
        ''' <remarks></remarks>
        AfBAN = 21

        ''' <summary>
        ''' Native ATM Services
        ''' </summary>
        ''' <remarks></remarks>
        AfATM = 22

        ''' <summary>
        ''' Internetwork Version 6
        ''' </summary>
        ''' <remarks></remarks>
        AfInet6 = 23

        ''' <summary>
        ''' Microsoft Wolfpack
        ''' </summary>
        ''' <remarks></remarks>
        AfCLUSTER = 24

        ''' <summary>
        ''' IEEE 1284.4 WG AF
        ''' </summary>
        ''' <remarks></remarks>
        Af12844 = 25

        ''' <summary>
        ''' IrDA
        ''' </summary>
        ''' <remarks></remarks>
        AfIRDA = 26

        ''' <summary>
        ''' Network Designers OSI &amp; gateway
        ''' </summary>
        ''' <remarks></remarks>
        AfNETDES = 28

        ''' <summary>
        '''
        ''' </summary>
        ''' <remarks></remarks>
        AfTCNProcess = 29

        ''' <summary>
        '''
        ''' </summary>
        ''' <remarks></remarks>
        AfTCNMessage = 30

        ''' <summary>
        '''
        ''' </summary>
        ''' <remarks></remarks>
        AfICLFXBM = 31

    End Enum

#End Region

#Region "AfENUM"

    ''' <summary>
    ''' Network adapter enumerator-allowed address families.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum AfENUM As UInt32

        ''' <summary>
        ''' Return both IPv4 and IPv6 addresses associated with adapters with IPv4 or IPv6 enabled.
        ''' </summary>
        ''' <remarks></remarks>
        AfUnspecified = 0

        ''' <summary>
        ''' Return only IPv4 addresses associated with adapters with IPv4 enabled.
        ''' </summary>
        ''' <remarks></remarks>
        AfInet = 2

        ''' <summary>
        ''' Return only IPv6 addresses associated with adapters with IPv6 enabled.
        ''' </summary>
        ''' <remarks></remarks>
        AfInet6 = 23

    End Enum

#End Region

#Region "Socket Structure"

    ''' <summary>
    ''' Represents an IPv4 socket address.
    ''' </summary>
    ''' <remarks></remarks>
    <StructLayout(LayoutKind.Sequential), TypeConverter(GetType(ExpandableObjectConverter))>
    Public Structure SOCKADDR
        ''' <summary>
        ''' Address family.
        ''' </summary>
        ''' <remarks></remarks>
        Public AddressFamily As AddressFamily

        ''' <summary>
        ''' Address port.
        ''' </summary>
        ''' <remarks></remarks>
        Public Port As UShort

        ''' <summary>
        ''' Address data.
        ''' </summary>
        ''' <remarks></remarks>
        <MarshalAs(UnmanagedType.ByValArray, ArraySubType:=UnmanagedType.U1, SizeConst:=4)>
        Public Data() As Byte

        ''' <summary>
        ''' Reserved, must be zero.
        ''' </summary>
        ''' <remarks></remarks>
        <MarshalAs(UnmanagedType.ByValArray, ArraySubType:=UnmanagedType.U1, SizeConst:=8)>
        Public Zero() As Byte

        ''' <summary>
        ''' Gets the IP address for this structure from the Data buffer.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Address As IPAddress
            Get
                Return New IPAddress(Data)
            End Get
        End Property

        Public Overrides Function ToString() As String
            If Data Is Nothing Then Return "NULL"
            Return "" & New IPAddress(Data).ToString & " (" & AddressFamily.ToString() & ")"
        End Function

    End Structure

    ''' <summary>
    ''' Represents an IPv6 socket address.
    ''' </summary>
    ''' <remarks></remarks>
    <StructLayout(LayoutKind.Sequential), TypeConverter(GetType(ExpandableObjectConverter))>
    Public Structure SOCKADDRV6

        ''' <summary>
        ''' Address family.
        ''' </summary>
        ''' <remarks></remarks>
        Public AddressFamily As AddressFamily

        ''' <summary>
        ''' Address port.
        ''' </summary>
        ''' <remarks></remarks>
        Public Port As UShort

        ''' <summary>
        ''' Address data.
        ''' </summary>
        ''' <remarks></remarks>
        <MarshalAs(UnmanagedType.ByValArray, ArraySubType:=UnmanagedType.U1, SizeConst:=16)>
        Public Data() As Byte

        ''' <summary>
        ''' Reserved, must be zero.
        ''' </summary>
        ''' <remarks></remarks>
        <MarshalAs(UnmanagedType.ByValArray, ArraySubType:=UnmanagedType.U1, SizeConst:=8)>
        Public Zero() As Byte

        ''' <summary>
        ''' Gets the IP address for this structure from the Data buffer.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Address As IPAddress
            Get
                Return New IPAddress(Data)
            End Get
        End Property

        Public Overrides Function ToString() As String
            If Data Is Nothing Then Return "NULL"
            Return "" & New IPAddress(Data).ToString & " (" & AddressFamily.ToString() & ")"
        End Function

    End Structure

    ''' <summary>
    ''' Structure that encapsulates the marshaling of a live memory pointer to either a SOCKADDR or a SOCKADDRV6
    ''' </summary>
    ''' <remarks></remarks>
    <StructLayout(LayoutKind.Sequential), TypeConverter(GetType(ExpandableObjectConverter))>
    Public Structure LPSOCKADDR
        Public Handle As MemPtr

        Public Overrides Function ToString() As String
            If Handle.Handle = IntPtr.Zero Then Return "NULL"
            Return "" & IPAddress.ToString & " (" & AddressFamily.ToString() & ")"
        End Function

        Public ReadOnly Property IPAddress As IPAddress
            Get
                If Data Is Nothing Then Return Nothing
                Return New IPAddress(Data)
            End Get
        End Property

        Public ReadOnly Property IPAddrV4 As SOCKADDR
            Get
                If AddressFamily = AddressFamily.AfInet6 Then Return Nothing
                IPAddrV4 = ToSockAddr()
            End Get
        End Property

        Public ReadOnly Property IPAddrV6 As SOCKADDRV6
            Get
                If AddressFamily = AddressFamily.AfInet Then Return New SOCKADDRV6
                IPAddrV6 = ToSockAddr6()
            End Get
        End Property

        Public Function ToSockAddr() As SOCKADDR
            If Handle = IntPtr.Zero Then Return New SOCKADDR
            ToSockAddr = Handle.ToStruct(Of SOCKADDR)()
        End Function

        Public Function ToSockAddr6() As SOCKADDRV6
            If Handle = IntPtr.Zero Then Return Nothing
            ToSockAddr6 = Handle.ToStruct(Of SOCKADDRV6)()
        End Function

        Public Sub Dispose()
            Handle.Free()
        End Sub

        Public ReadOnly Property AddressFamily As AddressFamily
            Get
                If Handle.Handle = IntPtr.Zero Then Return AddressFamily.AfUnspecified
                Return ToSockAddr.AddressFamily
            End Get
        End Property

        Public ReadOnly Property Data() As Byte()
            Get
                Select Case AddressFamily
                    Case AddressFamily.AfInet
                        Return IPAddrV4.Data

                    Case Else
                        Return IPAddrV6.Data

                End Select
            End Get
        End Property

        Public Shared Widening Operator CType(operand As IntPtr) As LPSOCKADDR
            Dim a As New LPSOCKADDR
            a.Handle = operand
            Return a
        End Operator

        Public Shared Widening Operator CType(operand As LPSOCKADDR) As IntPtr
            Return operand.Handle.Handle
        End Operator

        Public Shared Widening Operator CType(operand As MemPtr) As LPSOCKADDR
            Dim a As New LPSOCKADDR
            a.Handle = operand
            Return a
        End Operator

        Public Shared Widening Operator CType(operand As LPSOCKADDR) As MemPtr
            Return operand.Handle
        End Operator

        Public Shared Widening Operator CType(operand As SOCKADDR) As LPSOCKADDR
            Dim a As New LPSOCKADDR
            a.Handle.Alloc(Marshal.SizeOf(operand))
            Marshal.StructureToPtr(operand, a.Handle.Handle, True)
            Return a
        End Operator

        Public Shared Widening Operator CType(operand As LPSOCKADDR) As SOCKADDR
            Return operand.ToSockAddr
        End Operator

        Public Shared Widening Operator CType(operand As SOCKADDRV6) As LPSOCKADDR
            Dim a As New LPSOCKADDR
            a.Handle.Alloc(Marshal.SizeOf(operand))
            Marshal.StructureToPtr(operand, a.Handle.Handle, True)
            Return a
        End Operator

        Public Shared Widening Operator CType(operand As LPSOCKADDR) As SOCKADDRV6
            Return operand.ToSockAddr6
        End Operator

    End Structure

    <StructLayout(LayoutKind.Sequential), TypeConverter(GetType(ExpandableObjectConverter))>
    Public Structure SOCKET_ADDRESS
        Public lpSockaddr As LPSOCKADDR
        Public iSockaddrLength As Integer
        Public Overrides Function ToString() As String
            If lpSockaddr.Handle.Handle = IntPtr.Zero Then Return "NULL"
            ToString = lpSockaddr.ToString
        End Function
    End Structure

#End Region

#Region "Address Structures"

    Public Enum IpDadState
        IpDadStateInvalid = 0
        IpDadStateTentative
        IpDadStateDuplicate
        IpDadStateDeprecated
        IpDadStatePreferred
    End Enum

    Public Enum IpPrefixOrigin
        IpPrefixOriginOther = 0
        IpPrefixOriginManual
        IpPrefixOriginWellKnown
        IpPrefixOriginDhcp
        IpPrefixOriginRouterAdvertisement
        IpPrefixOriginUnchanged = &H10000
    End Enum

    Public Enum IpSuffixOrigin
        IpSuffixOriginOther = 0
        IpSuffixOriginManual
        IpSuffixOriginWellKnown
        IpSuffixOriginDhcp
        IpSuffixOriginLinkLayerAddress
        IpSuffixOriginRandom
        IpSuffixOriginUnchanged = &H10000
    End Enum

    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode), TypeConverter(GetType(ExpandableObjectConverter))>
    Public Structure IP_ADAPTER_DNS_SUFFIX
        Public [Next] As LPIP_ADAPTER_DNS_SUFFIX

        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=256)>
        Public [String] As String
    End Structure

    ''' <summary>
    ''' Pointerized IP_ADAPTER_DNS_SUFFIX structure.
    ''' </summary>
    ''' <remarks></remarks>
    <StructLayout(LayoutKind.Sequential), TypeConverter(GetType(ExpandableObjectConverter))>
    Public Structure LPIP_ADAPTER_DNS_SUFFIX
        Public Handle As MemPtr

        Public ReadOnly Property Chain As LPIP_ADAPTER_DNS_SUFFIX()
            Get
                If Handle = IntPtr.Zero Then Return Nothing
                Dim c As Integer = 0
                Dim mx As LPIP_ADAPTER_DNS_SUFFIX = Me
                Dim ac() As LPIP_ADAPTER_DNS_SUFFIX

                Do
                    ReDim Preserve ac(c)
                    ac(c) = mx
                    mx = mx.Next
                    c += 1
                Loop Until mx.Handle.Handle = IntPtr.Zero

                Return ac
            End Get
        End Property

        Public ReadOnly Property [Next] As LPIP_ADAPTER_DNS_SUFFIX
            Get
                If Handle = IntPtr.Zero Then Return Nothing
                Return Struct.Next
            End Get
        End Property

        Public Overrides Function ToString() As String
            If Handle.Handle = IntPtr.Zero Then Return "NULL"
            Return Struct.String
        End Function

        Public ReadOnly Property Struct As IP_ADAPTER_DNS_SUFFIX
            Get
                If Handle = IntPtr.Zero Then Return Nothing
                Struct = ToStruct()
            End Get
        End Property

        Public Function ToStruct() As IP_ADAPTER_DNS_SUFFIX
            If Handle = IntPtr.Zero Then Return Nothing
            ToStruct = Handle.ToStruct(Of IP_ADAPTER_DNS_SUFFIX)
        End Function

        Public Sub Dispose()
            Handle.Free()
        End Sub

        Public Shared Widening Operator CType(operand As IntPtr) As LPIP_ADAPTER_DNS_SUFFIX
            Dim a As New LPIP_ADAPTER_DNS_SUFFIX
            a.Handle = operand
            Return a
        End Operator

        Public Shared Widening Operator CType(operand As LPIP_ADAPTER_DNS_SUFFIX) As IntPtr
            Return operand.Handle
        End Operator

        Public Shared Widening Operator CType(operand As MemPtr) As LPIP_ADAPTER_DNS_SUFFIX
            Dim a As New LPIP_ADAPTER_DNS_SUFFIX
            a.Handle = operand
            Return a
        End Operator

        Public Shared Widening Operator CType(operand As LPIP_ADAPTER_DNS_SUFFIX) As MemPtr
            Return operand.Handle
        End Operator

        Public Shared Widening Operator CType(operand As IP_ADAPTER_DNS_SUFFIX) As LPIP_ADAPTER_DNS_SUFFIX
            Dim a As New LPIP_ADAPTER_DNS_SUFFIX
            a.Handle.Alloc(Marshal.SizeOf(operand))
            Marshal.StructureToPtr(operand, a.Handle.Handle, True)
            Return a
        End Operator

        Public Shared Widening Operator CType(operand As LPIP_ADAPTER_DNS_SUFFIX) As IP_ADAPTER_DNS_SUFFIX
            Return operand.Struct
        End Operator

    End Structure

    ''' <summary>
    ''' IP adapter common structure header.
    ''' </summary>
    ''' <remarks></remarks>
    <StructLayout(LayoutKind.Sequential), TypeConverter(GetType(ExpandableObjectConverter))>
    Public Structure IP_ADAPTER_HEADER_UNION
        Public Alignment As ULong

        ''' <summary>
        ''' Length of the structure
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Length As UInteger
            Get
                Return CUInt(CLng(Alignment) And &HFFFFFFFF)
            End Get
        End Property

        ''' <summary>
        ''' Interface index
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property IfIndex As UInteger
            Get
                Return CUInt(CLng(Alignment >> 32) And &HFFFFFFFFL)
            End Get
        End Property

    End Structure

    <StructLayout(LayoutKind.Sequential), TypeConverter(GetType(ExpandableObjectConverter))>
    Public Structure IP_ADAPTER_PREFIX
        Public Header As IP_ADAPTER_HEADER_UNION
        Public [Next] As LPIP_ADAPTER_PREFIX
        Public Address As SOCKET_ADDRESS
        Public Prefixlength As UInteger

        Public Overrides Function ToString() As String
            ToString = Address.ToString
        End Function
    End Structure

    <StructLayout(LayoutKind.Sequential), TypeConverter(GetType(ExpandableObjectConverter))>
    Public Structure LPIP_ADAPTER_PREFIX
        Public Handle As MemPtr

        Public ReadOnly Property AddressChain As LPIP_ADAPTER_PREFIX()
            Get
                If Handle = IntPtr.Zero Then Return Nothing
                Dim c As Integer = 0
                Dim mx As LPIP_ADAPTER_PREFIX = Me
                Dim ac() As LPIP_ADAPTER_PREFIX

                Do
                    ReDim Preserve ac(c)
                    ac(c) = mx
                    mx = mx.Next
                    c += 1
                Loop Until mx.Handle.Handle = IntPtr.Zero

                Return ac
            End Get
        End Property

        Public ReadOnly Property [Next] As LPIP_ADAPTER_PREFIX
            Get
                If Handle = IntPtr.Zero Then Return Nothing
                Return Struct.Next
            End Get
        End Property

        Public Overrides Function ToString() As String
            If Handle.Handle = IntPtr.Zero Then Return "NULL"
            Return "" & IPAddress.ToString & " (" & AddressFamily.ToString() & ")"
        End Function

        Public ReadOnly Property IPAddress As Net.IPAddress
            Get
                If Handle = IntPtr.Zero Then Return Nothing
                Return Struct.Address.lpSockaddr.IPAddress
            End Get
        End Property

        Public ReadOnly Property Struct As IP_ADAPTER_PREFIX
            Get
                If Handle = IntPtr.Zero Then Return Nothing
                Struct = ToAddress()
            End Get
        End Property

        Public Function ToAddress() As IP_ADAPTER_PREFIX
            If Handle = IntPtr.Zero Then Return Nothing
            ToAddress = Handle.ToStruct(Of IP_ADAPTER_PREFIX)
        End Function

        Public Sub Dispose()
            Handle.Free()
        End Sub

        Public ReadOnly Property AddressFamily As AddressFamily
            Get
                If Handle = IntPtr.Zero Then Return Nothing
                Return Struct.Address.lpSockaddr.AddressFamily
            End Get
        End Property

        Public ReadOnly Property Data() As Byte()
            Get
                If Handle = IntPtr.Zero Then Return Nothing
                Return Struct.Address.lpSockaddr.Data
            End Get
        End Property

        Public Shared Widening Operator CType(operand As IntPtr) As LPIP_ADAPTER_PREFIX
            Dim a As New LPIP_ADAPTER_PREFIX
            a.Handle = operand
            Return a
        End Operator

        Public Shared Widening Operator CType(operand As LPIP_ADAPTER_PREFIX) As IntPtr
            Return operand.Handle
        End Operator

        Public Shared Widening Operator CType(operand As MemPtr) As LPIP_ADAPTER_PREFIX
            Dim a As New LPIP_ADAPTER_PREFIX
            a.Handle = operand
            Return a
        End Operator

        Public Shared Widening Operator CType(operand As LPIP_ADAPTER_PREFIX) As MemPtr
            Return operand.Handle
        End Operator

        Public Shared Widening Operator CType(operand As IP_ADAPTER_PREFIX) As LPIP_ADAPTER_PREFIX
            Dim a As New LPIP_ADAPTER_PREFIX
            a.Handle.Alloc(Marshal.SizeOf(operand))
            Marshal.StructureToPtr(operand, a.Handle.Handle, True)
            Return a
        End Operator

        Public Shared Widening Operator CType(operand As LPIP_ADAPTER_PREFIX) As IP_ADAPTER_PREFIX
            Return operand.ToAddress
        End Operator

    End Structure

    <StructLayout(LayoutKind.Sequential), TypeConverter(GetType(ExpandableObjectConverter))>
    Public Structure ADAPTER_UNICAST_ADDRESS
        Public Header As IP_ADAPTER_HEADER_UNION
        Public [Next] As LPADAPTER_UNICAST_ADDRESS
        Public Address As SOCKET_ADDRESS
        Public PrefixOrigin As IpPrefixOrigin
        Public SuffixOrigin As IpSuffixOrigin
        Public ValidLifetime As UInteger
        Public PreferredLifetime As UInteger
        Public LeaseLifetime As UInteger

        Public Overrides Function ToString() As String
            ToString = Address.ToString
        End Function
    End Structure

    <StructLayout(LayoutKind.Sequential), TypeConverter(GetType(ExpandableObjectConverter))>
    Public Structure LPADAPTER_UNICAST_ADDRESS
        Public Handle As MemPtr

        Public ReadOnly Property AddressChain As LPADAPTER_UNICAST_ADDRESS()
            Get
                Dim c As Integer = 0
                Dim mx As LPADAPTER_UNICAST_ADDRESS = Me
                Dim ac() As LPADAPTER_UNICAST_ADDRESS

                Do
                    ReDim Preserve ac(c)
                    ac(c) = mx
                    mx = mx.Next
                    c += 1
                Loop Until mx.Handle.Handle = IntPtr.Zero

                Return ac
            End Get
        End Property

        Public ReadOnly Property [Next] As LPADAPTER_UNICAST_ADDRESS
            Get
                Return Struct.Next
            End Get
        End Property

        Public Overrides Function ToString() As String
            If Handle.Handle = IntPtr.Zero Then Return "NULL"
            Return "" & IPAddress.ToString & " (" & AddressFamily.ToString() & ")"
        End Function

        Public ReadOnly Property IPAddress As Net.IPAddress
            Get
                Return Struct.Address.lpSockaddr.IPAddress
            End Get
        End Property

        Public ReadOnly Property Struct As ADAPTER_UNICAST_ADDRESS
            Get
                Struct = ToAddress()
            End Get
        End Property

        Public Function ToAddress() As ADAPTER_UNICAST_ADDRESS
            If Handle = IntPtr.Zero Then Return Nothing
            ToAddress = Handle.ToStruct(Of ADAPTER_UNICAST_ADDRESS)
        End Function

        Public Sub Dispose()
            Handle.Free()
        End Sub

        Public ReadOnly Property AddressFamily As AddressFamily
            Get
                Return Struct.Address.lpSockaddr.AddressFamily
            End Get
        End Property

        Public ReadOnly Property Data() As Byte()
            Get
                Return Struct.Address.lpSockaddr.Data
            End Get
        End Property

        Public Shared Widening Operator CType(operand As IntPtr) As LPADAPTER_UNICAST_ADDRESS
            Dim a As New LPADAPTER_UNICAST_ADDRESS
            a.Handle = operand
            Return a
        End Operator

        Public Shared Widening Operator CType(operand As LPADAPTER_UNICAST_ADDRESS) As IntPtr
            Return operand.Handle
        End Operator

        Public Shared Widening Operator CType(operand As MemPtr) As LPADAPTER_UNICAST_ADDRESS
            Dim a As New LPADAPTER_UNICAST_ADDRESS
            a.Handle = operand
            Return a
        End Operator

        Public Shared Widening Operator CType(operand As LPADAPTER_UNICAST_ADDRESS) As MemPtr
            Return operand.Handle
        End Operator

        Public Shared Widening Operator CType(operand As ADAPTER_UNICAST_ADDRESS) As LPADAPTER_UNICAST_ADDRESS
            Dim a As New LPADAPTER_UNICAST_ADDRESS
            a.Handle.Alloc(Marshal.SizeOf(operand))
            Marshal.StructureToPtr(operand, a.Handle.Handle, True)
            Return a
        End Operator

        Public Shared Widening Operator CType(operand As LPADAPTER_UNICAST_ADDRESS) As ADAPTER_UNICAST_ADDRESS
            Return operand.ToAddress
        End Operator

    End Structure

    <StructLayout(LayoutKind.Sequential), TypeConverter(GetType(ExpandableObjectConverter))>
    Public Structure ADAPTER_MULTICAST_ADDRESS
        Public Header As IP_ADAPTER_HEADER_UNION
        Public [Next] As LPADAPTER_MULTICAST_ADDRESS
        Public Address As SOCKET_ADDRESS

        Public Overrides Function ToString() As String
            ToString = Address.ToString
        End Function
    End Structure

    <StructLayout(LayoutKind.Sequential), TypeConverter(GetType(ExpandableObjectConverter))>
    Public Structure LPADAPTER_MULTICAST_ADDRESS
        Public Handle As MemPtr

        Public ReadOnly Property AddressChain As LPADAPTER_MULTICAST_ADDRESS()
            Get
                If Handle = IntPtr.Zero Then Return Nothing

                Dim c As Integer = 0
                Dim mx As LPADAPTER_MULTICAST_ADDRESS = Me
                Dim ac() As LPADAPTER_MULTICAST_ADDRESS

                Do
                    ReDim Preserve ac(c)
                    ac(c) = mx
                    mx = mx.Next
                    c += 1
                Loop Until mx.Handle.Handle = IntPtr.Zero

                Return ac
            End Get
        End Property

        Public ReadOnly Property [Next] As LPADAPTER_MULTICAST_ADDRESS
            Get
                If Handle = IntPtr.Zero Then Return Nothing
                Return Struct.Next
            End Get
        End Property

        Public Overrides Function ToString() As String
            If Handle.Handle = IntPtr.Zero Then Return "NULL"
            Return "" & IPAddress.ToString & " (" & AddressFamily.ToString() & ")"
        End Function

        Public ReadOnly Property IPAddress As Net.IPAddress
            Get
                If Handle = IntPtr.Zero Then Return Nothing
                Return Struct.Address.lpSockaddr.IPAddress
            End Get
        End Property

        Public ReadOnly Property Struct As ADAPTER_MULTICAST_ADDRESS
            Get
                If Handle = IntPtr.Zero Then Return Nothing
                Struct = ToAddress()
            End Get
        End Property

        Public Function ToAddress() As ADAPTER_MULTICAST_ADDRESS
            If Handle = IntPtr.Zero Then Return Nothing
            ToAddress = Handle.ToStruct(Of ADAPTER_MULTICAST_ADDRESS)
        End Function

        Public Sub Dispose()
            Handle.Free()
        End Sub

        Public ReadOnly Property AddressFamily As AddressFamily
            Get
                If Handle = IntPtr.Zero Then Return AddressFamily.AfUnspecified
                Return Struct.Address.lpSockaddr.AddressFamily
            End Get
        End Property

        Public ReadOnly Property Data() As Byte()
            Get
                If Handle = IntPtr.Zero Then Return Nothing
                Return Struct.Address.lpSockaddr.Data
            End Get
        End Property

        Public Shared Widening Operator CType(operand As IntPtr) As LPADAPTER_MULTICAST_ADDRESS
            Dim a As New LPADAPTER_MULTICAST_ADDRESS
            a.Handle = operand
            Return a
        End Operator

        Public Shared Widening Operator CType(operand As LPADAPTER_MULTICAST_ADDRESS) As IntPtr
            Return operand.Handle
        End Operator

        Public Shared Widening Operator CType(operand As MemPtr) As LPADAPTER_MULTICAST_ADDRESS
            Dim a As New LPADAPTER_MULTICAST_ADDRESS
            a.Handle = operand
            Return a
        End Operator

        Public Shared Widening Operator CType(operand As LPADAPTER_MULTICAST_ADDRESS) As MemPtr
            Return operand.Handle
        End Operator

        Public Shared Widening Operator CType(operand As ADAPTER_MULTICAST_ADDRESS) As LPADAPTER_MULTICAST_ADDRESS
            Dim a As New LPADAPTER_MULTICAST_ADDRESS
            a.Handle.Alloc(Marshal.SizeOf(operand))
            Marshal.StructureToPtr(operand, a.Handle.Handle, True)
            Return a
        End Operator

        Public Shared Widening Operator CType(operand As LPADAPTER_MULTICAST_ADDRESS) As ADAPTER_MULTICAST_ADDRESS
            Return operand.ToAddress
        End Operator

    End Structure

    <StructLayout(LayoutKind.Sequential), TypeConverter(GetType(ExpandableObjectConverter))>
    Public Structure LUID
        Public Value As ULong

        Public ReadOnly Property Reserved As Integer
            Get
                Return CInt(CLng(Value) And &HFFFFFF)
            End Get
        End Property

        Public ReadOnly Property NetLuidIndex As Integer
            Get
                Return CInt(CLng(Value >> 24) And &HFFFFFF)
            End Get
        End Property

        Public ReadOnly Property IfType As IFTYPE
            Get
                Return CType(CLng(Value >> 48) And &HFFFF, IFTYPE)
            End Get
        End Property

        Public Overrides Function ToString() As String
            Return IfType.ToString
        End Function
    End Structure


    ''' <summary>
    ''' Represents a network adapter MAC address.
    ''' </summary>
    ''' <remarks></remarks>
    <StructLayout(LayoutKind.Sequential), TypeConverter(GetType(ExpandableObjectConverter))>
    Public Structure MACADDRESS
        <MarshalAs(UnmanagedType.ByValArray, ArraySubType:=UnmanagedType.U1, SizeConst:=MAX_ADAPTER_ADDRESS_LENGTH)>
        Public Data() As Byte

        Public Overrides Function ToString() As String
            Dim s As String = ""
            Dim b As Byte

            If Data Is Nothing Then Return "NULL"

            '' let's get a clean string without extraneous zeros at the end:

            Dim i As Integer,
            c As Integer = Data.Length - 1

            For i = c To 0 Step -1
                If Data(i) <> 0 Then Exit For
            Next

            c = i
            i = 0

            For i = 0 To c
                b = Data(i)
                If s <> "" Then s &= ":"
                s &= b.ToString("X2")
            Next

            If s = "" Then s = "NULL"
            ToString = s

        End Function

    End Structure

#End Region

#Region "IFTYPE (large enum)"
    '' From Microsoft:

    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ''                                                                          ''
    '' Media types                                                              ''
    ''                                                                          ''
    '' These are enumerated values of the ifType object defined in MIB-II's     ''
    '' ifTable.  They are registered with IANA which publishes this list        ''
    '' periodically, in either the Assigned Numbers RFC, or some derivative     ''
    '' of it specific to Internet Network Management number assignments.        ''
    '' See ftp:''ftp.isi.edu/mib/ianaiftype.mib                                 ''
    ''                                                                          ''
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public Enum IFTYPE

        ''' <summary>
        ''' Minimum IF_TYPE integer value present in this enumeration.
        ''' </summary>
        ''' <remarks></remarks>
        MIN_IF_TYPE = 1

        ''' <summary>
        ''' None of the below
        ''' </summary>
        ''' <remarks></remarks>
        OTHER = 1

        REGULAR_1822 = 2
        HDH_1822 = 3
        DDN_X25 = 4
        RFC877_X25 = 5

        ''' <summary>
        ''' Ethernet adapter
        ''' </summary>
        ''' <remarks></remarks>
        ETHERNET_CSMACD = 6

        IS088023_CSMACD = 7
        ISO88024_TOKENBUS = 8
        ISO88025_TOKENRING = 9
        ISO88026_MAN = 10

        STARLAN = 11

        PROTEON_10MBIT = 12
        PROTEON_80MBIT = 13

        HYPERCHANNEL = 14

        FDDI = 15

        LAP_B = 16

        SDLC = 17

        ''' <summary>
        ''' DS1-MIB
        ''' </summary>
        ''' <remarks></remarks>
        DS1 = 18

        ''' <summary>
        ''' Obsolete; see DS1-MIB
        ''' </summary>
        ''' <remarks></remarks>
        E1 = 19

        BASIC_ISDN = 20
        PRIMARY_ISDN = 21
        ''' <summary>
        ''' proprietary serial
        ''' </summary>
        ''' <remarks></remarks>
        PROP_POINT2POINT_SERIAL = 22
        PPP = 23
        SOFTWARE_LOOPBACK = 24

        ''' <summary>
        ''' CLNP over IP
        ''' </summary>
        ''' <remarks></remarks>
        EON = 25

        ETHERNET_3MBIT = 26

        ''' <summary>
        ''' XNS over IP
        ''' </summary>
        ''' <remarks></remarks>
        NSIP = 27

        ''' <summary>
        ''' Generic Slip
        ''' </summary>
        ''' <remarks></remarks>
        SLIP = 28

        ''' <summary>
        ''' ULTRA Technologies
        ''' </summary>
        ''' <remarks></remarks>
        ULTRA = 29

        ''' <summary>
        ''' DS3-MIB
        ''' </summary>
        ''' <remarks></remarks>
        DS3 = 30

        ''' <summary>
        ''' SMDS, coffee
        ''' </summary>
        ''' <remarks></remarks>
        SIP = 31

        ''' <summary>
        ''' DTE only
        ''' </summary>
        ''' <remarks></remarks>
        FRAMERELAY = 32

        RS232 = 33

        ''' <summary>
        ''' Parallel port
        ''' </summary>
        ''' <remarks></remarks>
        PARA = 34

        ARCNET = 35
        ARCNET_PLUS = 36

        ''' <summary>
        ''' ATM cells
        ''' </summary>
        ''' <remarks></remarks>
        ATM = 37

        MIO_X25 = 38

        ''' <summary>
        ''' SONET or SDH
        ''' </summary>
        ''' <remarks></remarks>
        SONET = 39

        X25_PLE = 40
        ISO88022_LLC = 41
        LOCALTALK = 42
        SMDS_DXI = 43

        ''' <summary>
        ''' FRNETSERV-MIB
        ''' </summary>
        ''' <remarks></remarks>
        FRAMERELAY_SERVICE = 44

        V35 = 45
        HSSI = 46
        HIPPI = 47

        ''' <summary>
        ''' Generic Modem
        ''' </summary>
        ''' <remarks></remarks>
        MODEM = 48

        ''' <summary>
        ''' AAL5 over ATM
        ''' </summary>
        ''' <remarks></remarks>
        AAL5 = 49

        SONET_PATH = 50
        SONET_VT = 51

        ''' <summary>
        ''' SMDS InterCarrier Interface
        ''' </summary>
        ''' <remarks></remarks>
        SMDS_ICIP = 52

        ''' <summary>
        ''' Proprietary virtual/internal
        ''' </summary>
        ''' <remarks></remarks>
        PROP_VIRTUAL = 53

        ''' <summary>
        ''' Proprietary multiplexing
        ''' </summary>
        ''' <remarks></remarks>
        PROP_MULTIPLEXOR = 54

        ''' <summary>
        ''' 100BaseVG
        ''' </summary>
        ''' <remarks></remarks>
        IEEE80212 = 55

        FIBRECHANNEL = 56
        HIPPIINTERFACE = 57

        ''' <summary>
        ''' Obsolete, use 32 or 44
        ''' </summary>
        ''' <remarks></remarks>
        FRAMERELAY_INTERCONNECT = 58

        ''' <summary>
        ''' ATM Emulated LAN for 802.3
        ''' </summary>
        ''' <remarks></remarks>
        AFLANE_8023 = 59

        ''' <summary>
        ''' ATM Emulated LAN for 802.5
        ''' </summary>
        ''' <remarks></remarks>
        AFLANE_8025 = 60

        ''' <summary>
        ''' ATM Emulated circuit
        ''' </summary>
        ''' <remarks></remarks>
        CCTEMUL = 61

        ''' <summary>
        ''' Fast Ethernet (100BaseT)
        ''' </summary>
        ''' <remarks></remarks>
        FASTETHER = 62

        ''' <summary>
        ''' ISDN and X.25
        ''' </summary>
        ''' <remarks></remarks>
        ISDN = 63

        ''' <summary>
        ''' CCITT V.11/X.21
        ''' </summary>
        ''' <remarks></remarks>
        V11 = 64

        ''' <summary>
        ''' CCITT V.36
        ''' </summary>
        ''' <remarks></remarks>
        V36 = 65

        ''' <summary>
        ''' CCITT G703 at 64Kbps
        ''' </summary>
        ''' <remarks></remarks>
        G703_64K = 66

        ''' <summary>
        ''' Obsolete; see DS1-MIB
        ''' </summary>
        ''' <remarks></remarks>
        G703_2MB = 67

        ''' <summary>
        ''' SNA QLLC
        ''' </summary>
        ''' <remarks></remarks>
        QLLC = 68

        ''' <summary>
        ''' Fast Ethernet (100BaseFX)
        ''' </summary>
        ''' <remarks></remarks>
        FASTETHER_FX = 69

        CHANNEL = 70

        ''' <summary>
        ''' Radio spread spectrum - WiFi
        ''' </summary>
        ''' <remarks></remarks>
        IEEE80211 = 71

        ''' <summary>
        ''' IBM System 360/370 OEMI Channel
        ''' </summary>
        ''' <remarks></remarks>
        IBM370PARCHAN = 72

        ''' <summary>
        ''' IBM Enterprise Systems Connection
        ''' </summary>
        ''' <remarks></remarks>
        ESCON = 73

        ''' <summary>
        ''' Data Link Switching
        ''' </summary>
        ''' <remarks></remarks>
        DLSW = 74

        ''' <summary>
        ''' ISDN S/T interface
        ''' </summary>
        ''' <remarks></remarks>
        ISDN_S = 75

        ''' <summary>
        ''' ISDN U interface
        ''' </summary>
        ''' <remarks></remarks>
        ISDN_U = 76

        ''' <summary>
        ''' Link Access Protocol D
        ''' </summary>
        ''' <remarks></remarks>
        LAP_D = 77

        ''' <summary>
        ''' IP Switching Objects
        ''' </summary>
        ''' <remarks></remarks>
        IPSWITCH = 78

        ''' <summary>
        ''' Remote Source Route Bridging
        ''' </summary>
        ''' <remarks></remarks>
        RSRB = 79

        ''' <summary>
        ''' ATM Logical Port
        ''' </summary>
        ''' <remarks></remarks>
        ATM_LOGICAL = 80

        ''' <summary>
        ''' Digital Signal Level 0
        ''' </summary>
        ''' <remarks></remarks>
        DS0 = 81

        ''' <summary>
        ''' Group of ds0s on the same ds1
        ''' </summary>
        ''' <remarks></remarks>
        DS0_BUNDLE = 82

        ''' <summary>
        ''' Bisynchronous Protocol
        ''' </summary>
        ''' <remarks></remarks>
        BSC = 83

        ''' <summary>
        ''' Asynchronous Protocol
        ''' </summary>
        ''' <remarks></remarks>
        ASYNC = 84

        ''' <summary>
        ''' Combat Net Radio
        ''' </summary>
        ''' <remarks></remarks>
        CNR = 85

        ''' <summary>
        ''' ISO 802.5r DTR
        ''' </summary>
        ''' <remarks></remarks>
        ISO88025R_DTR = 86

        ''' <summary>
        ''' Ext Pos Loc Report Sys
        ''' </summary>
        ''' <remarks></remarks>
        EPLRS = 87

        ''' <summary>
        ''' Appletalk Remote Access Protocol
        ''' </summary>
        ''' <remarks></remarks>
        ARAP = 88

        ''' <summary>
        ''' Proprietary Connectionless Proto
        ''' </summary>
        ''' <remarks></remarks>
        PROP_CNLS = 89

        ''' <summary>
        ''' CCITT-ITU X.29 PAD Protocol
        ''' </summary>
        ''' <remarks></remarks>
        HOSTPAD = 90

        ''' <summary>
        ''' CCITT-ITU X.3 PAD Facility
        ''' </summary>
        ''' <remarks></remarks>
        TERMPAD = 91

        ''' <summary>
        ''' Multiproto Interconnect over FR
        ''' </summary>
        ''' <remarks></remarks>
        FRAMERELAY_MPI = 92

        ''' <summary>
        ''' CCITT-ITU X213
        ''' </summary>
        ''' <remarks></remarks>
        X213 = 93

        ''' <summary>
        ''' Asymmetric Digital Subscrbr Loop
        ''' </summary>
        ''' <remarks></remarks>
        ADSL = 94

        ''' <summary>
        ''' Rate-Adapt Digital Subscrbr Loop
        ''' </summary>
        ''' <remarks></remarks>
        RADSL = 95

        ''' <summary>
        ''' Symmetric Digital Subscriber Loop
        ''' </summary>
        ''' <remarks></remarks>
        SDSL = 96

        ''' <summary>
        ''' Very H-Speed Digital Subscrb Loop
        ''' </summary>
        ''' <remarks></remarks>
        VDSL = 97

        ''' <summary>
        ''' ISO 802.5 CRFP
        ''' </summary>
        ''' <remarks></remarks>
        ISO88025_CRFPRINT = 98

        ''' <summary>
        ''' Myricom Myrinet
        ''' </summary>
        ''' <remarks></remarks>
        MYRInet = 99

        ''' <summary>
        ''' Voice recEive and transMit
        ''' </summary>
        ''' <remarks></remarks>
        VOICE_EM = 100

        ''' <summary>
        ''' Voice Foreign Exchange Office
        ''' </summary>
        ''' <remarks></remarks>
        VOICE_FXO = 101

        ''' <summary>
        ''' Voice Foreign Exchange Station
        ''' </summary>
        ''' <remarks></remarks>
        VOICE_FXS = 102

        ''' <summary>
        ''' Voice encapsulation
        ''' </summary>
        ''' <remarks></remarks>
        VOICE_ENCAP = 103

        ''' <summary>
        ''' Voice over IP encapsulation
        ''' </summary>
        ''' <remarks></remarks>
        VOICE_OVERIP = 104

        ''' <summary>
        ''' ATM DXI
        ''' </summary>
        ''' <remarks></remarks>
        ATM_DXI = 105

        ''' <summary>
        ''' ATM FUNI
        ''' </summary>
        ''' <remarks></remarks>
        ATM_FUNI = 106

        ''' <summary>
        ''' ATM IMA
        ''' </summary>
        ''' <remarks></remarks>
        ATM_IMA = 107

        ''' <summary>
        ''' PPP Multilink Bundle
        ''' </summary>
        ''' <remarks></remarks>
        PPPMULTILINKBUNDLE = 108

        ''' <summary>
        ''' IBM ipOverCdlc
        ''' </summary>
        ''' <remarks></remarks>
        IPOVER_CDLC = 109

        ''' <summary>
        ''' IBM Common Link Access to Workstn
        ''' </summary>
        ''' <remarks></remarks>
        IPOVER_CLAW = 110

        ''' <summary>
        ''' IBM stackToStack
        ''' </summary>
        ''' <remarks></remarks>
        STACKTOSTACK = 111

        ''' <summary>
        ''' IBM VIPA
        ''' </summary>
        ''' <remarks></remarks>
        VIRTUALIPADDRESS = 112

        ''' <summary>
        ''' IBM multi-proto channel support
        ''' </summary>
        ''' <remarks></remarks>
        MPC = 113

        ''' <summary>
        ''' IBM ipOverAtm
        ''' </summary>
        ''' <remarks></remarks>
        IPOVER_ATM = 114

        ''' <summary>
        ''' ISO 802.5j Fiber Token Ring
        ''' </summary>
        ''' <remarks></remarks>
        ISO88025_FIBER = 115

        ''' <summary>
        ''' IBM twinaxial data link control
        ''' </summary>
        ''' <remarks></remarks>
        TDLC = 116

        GIGABITETHERNET = 117

        HDLC = 118

        LAP_F = 119

        V37 = 120

        ''' <summary>
        ''' Multi-Link Protocol
        ''' </summary>
        ''' <remarks></remarks>
        X25_MLP = 121

        ''' <summary>
        ''' X.25 Hunt Group
        ''' </summary>
        ''' <remarks></remarks>
        X25_HUNTGROUP = 122

        TRANSPHDLC = 123

        ''' <summary>
        ''' Interleave channel
        ''' </summary>
        ''' <remarks></remarks>
        INTERLEAVE = 124

        ''' <summary>
        ''' Fast channel
        ''' </summary>
        ''' <remarks></remarks>
        FAST = 125

        ''' <summary>
        ''' IP (for APPN HPR in IP networks)
        ''' </summary>
        ''' <remarks></remarks>
        IP = 126

        ''' <summary>
        ''' CATV Mac Layer
        ''' </summary>
        ''' <remarks></remarks>
        DOCSCABLE_MACLAYER = 127

        ''' <summary>
        ''' CATV Downstream interface
        ''' </summary>
        ''' <remarks></remarks>
        DOCSCABLE_DOWNSTREAM = 128

        ''' <summary>
        ''' CATV Upstream interface
        ''' </summary>
        ''' <remarks></remarks>
        DOCSCABLE_UPSTREAM = 129

        ''' <summary>
        ''' Avalon Parallel Processor
        ''' </summary>
        ''' <remarks></remarks>
        A12MPPSWITCH = 130

        ''' <summary>
        ''' Encapsulation interface
        ''' </summary>
        ''' <remarks></remarks>
        TUNNEL = 131

        ''' <summary>
        ''' Coffee pot
        ''' </summary>
        ''' <remarks></remarks>
        COFFEE = 132

        ''' <summary>
        ''' Circuit Emulation Service
        ''' </summary>
        ''' <remarks></remarks>
        CES = 133

        ''' <summary>
        ''' ATM Sub Interface
        ''' </summary>
        ''' <remarks></remarks>
        ATM_SUBINTERFACE = 134

        ''' <summary>
        ''' Layer 2 Virtual LAN using 802.1Q
        ''' </summary>
        ''' <remarks></remarks>
        L2_VLAN = 135

        ''' <summary>
        ''' Layer 3 Virtual LAN using IP
        ''' </summary>
        ''' <remarks></remarks>
        L3_IPVLAN = 136

        ''' <summary>
        ''' Layer 3 Virtual LAN using IPX
        ''' </summary>
        ''' <remarks></remarks>
        L3_IPXVLAN = 137

        ''' <summary>
        ''' IP over Power Lines
        ''' </summary>
        ''' <remarks></remarks>
        DIGITALPOWERLINE = 138

        ''' <summary>
        ''' Multimedia Mail over IP
        ''' </summary>
        ''' <remarks></remarks>
        MEDIAMAILOVERIP = 139

        ''' <summary>
        ''' Dynamic syncronous Transfer Mode
        ''' </summary>
        ''' <remarks></remarks>
        DTM = 140

        ''' <summary>
        ''' Data Communications Network
        ''' </summary>
        ''' <remarks></remarks>
        DCN = 141

        ''' <summary>
        ''' IP Forwarding Interface
        ''' </summary>
        ''' <remarks></remarks>
        IPFORWARD = 142

        ''' <summary>
        ''' Multi-rate Symmetric DSL
        ''' </summary>
        ''' <remarks></remarks>
        MSDSL = 143

        ''' <summary>
        ''' IEEE1394 High Perf Serial Bus
        ''' </summary>
        ''' <remarks></remarks>
        IEEE1394 = 144

        IF_GSN = 145
        DVBRCC_MACLAYER = 146
        DVBRCC_DOWNSTREAM = 147
        DVBRCC_UPSTREAM = 148
        ATM_VIRTUAL = 149
        MPLS_TUNNEL = 150
        SRP = 151
        VOICEOVERATM = 152
        VOICEOVERFRAMERELAY = 153
        IDSL = 154
        COMPOSITELINK = 155
        SS7_SIGLINK = 156
        PROP_WIRELESS_P2P = 157
        FR_FORWARD = 158
        RFC1483 = 159
        USB = 160
        IEEE8023AD_LAG = 161
        BGP_POLICY_ACCOUNTING = 162
        FRF16_MFR_BUNDLE = 163
        H323_GATEKEEPER = 164
        H323_PROXY = 165
        MPLS = 166
        MF_SIGLINK = 167
        HDSL2 = 168
        SHDSL = 169
        DS1_FDL = 170
        POS = 171
        DVB_ASI_IN = 172
        DVB_ASI_OUT = 173
        PLC = 174
        NFAS = 175
        TR008 = 176
        GR303_RDT = 177
        GR303_IDT = 178
        ISUP = 179
        PROP_DOCS_WIRELESS_MACLAYER = 180
        PROP_DOCS_WIRELESS_DOWNSTREAM = 181
        PROP_DOCS_WIRELESS_UPSTREAM = 182
        HIPERLAN2 = 183
        PROP_BWA_P2MP = 184
        SONET_OVERHEAD_CHANNEL = 185
        DIGITAL_WRAPPER_OVERHEAD_CHANNEL = 186
        AAL2 = 187
        RADIO_MAC = 188
        ATM_RADIO = 189
        IMT = 190
        MVL = 191
        REACH_DSL = 192
        FR_DLCI_ENDPT = 193
        ATM_VCI_ENDPT = 194
        OPTICAL_CHANNEL = 195
        OPTICAL_TRANSPORT = 196
        IEEE80216_WMAN = 237

        ''' <summary>
        ''' WWAN devices based on GSM technology
        ''' </summary>
        ''' <remarks></remarks>
        WWANPP = 243

        ''' <summary>
        ''' WWAN devices based on CDMA technology
        ''' </summary>
        ''' <remarks></remarks>
        WWANPP2 = 244

        ''' <summary>
        ''' Maximum IF_TYPE integer value present in this enumeration.
        ''' </summary>
        ''' <remarks></remarks>
        MAX_IF_TYPE = 244

    End Enum

#End Region '' IFTYPE (large enum)

#Region "Other Network Enums"

    ''' <summary>
    ''' Interface connection type.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum NET_IF_CONNECTION_TYPE

        ''' <summary>
        ''' Undefined
        ''' </summary>
        Undefined = 0

        ''' <summary>
        ''' Dedicated connection.  This is a typical connection.
        ''' </summary>
        ''' <remarks></remarks>
        Dedicated = 1

        ''' <summary>
        ''' Passive
        ''' </summary>
        Passive = 2

        ''' <summary>
        ''' On demand
        ''' </summary>
        Demand = 3

        ''' <summary>
        ''' Maximum
        ''' </summary>
        Maximum = 4
    End Enum

    ''' <summary>
    ''' Interface tunnel type.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum TUNNEL_TYPE

        ''' <summary>
        ''' None
        ''' </summary>
        None = 0

        ''' <summary>
        ''' Other
        ''' </summary>
        Other = 1

        ''' <summary>
        ''' Direct
        ''' </summary>
        Direct = 2

        ''' <summary>
        ''' Ipv6 to Ipv4 tunnel
        ''' </summary>
        IPv6ToIPv4 = 11

        ''' <summary>
        ''' ISATAP tunnel
        ''' </summary>
        ISATAP = 13

        ''' <summary>
        ''' Teredo Ipv6 tunnel
        ''' </summary>
        Teredo = 14

        ''' <summary>
        ''' IPHTTPS tunnel
        ''' </summary>
        IPHTTPS = 15
    End Enum

    ''' <summary>
    ''' Interface operational status.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum IF_OPER_STATUS
        ''' <summary>
        ''' The network device is up
        ''' </summary>
        ''' <remarks></remarks>
        IfOperStatusUp = 1

        ''' <summary>
        ''' The network device is down
        ''' </summary>
        ''' <remarks></remarks>
        IfOperStatusDown

        ''' <summary>
        ''' The network device is performing a self-test
        ''' </summary>
        IfOperStatusTesting

        ''' <summary>
        ''' The state of the network device is unknown
        ''' </summary>
        IfOperStatusUnknown

        ''' <summary>
        ''' The network device is asleep
        ''' </summary>
        IfOperStatusDormant

        ''' <summary>
        ''' The network device is not present
        ''' </summary>
        IfOperStatusNotPresent

        ''' <summary>
        ''' Network device lower-layer is down
        ''' </summary>
        IfOperStatusLowerLayerDown
    End Enum

#End Region

#Region "IPAdapterAddressess Flags"

    <Flags>
    Public Enum IPAdapterAddressesFlags
        DDNS = &H1
        RegisterAdapterSuffix = &H2
        DHCP = &H4
        ReceiveOnly = &H8
        NoMulticast = &H10
        IPv6OtherStatfulConfig = &H20
        NetBiosOverTCPIP = &H40
        IPv4Enabled = &H80
        IPv6Enabled = &H100
        IPv6ManageAddressConfig = &H200
    End Enum

#End Region

#Region "IPAdapterAddresses"

#Region "Original C Structure For Reference"

    'typedef struct _IP_ADAPTER_ADDRESSES {
    '  union {
    '    ULONGLONG Alignment;
    '    struct {
    '      ULONG Length;
    '      DWORD IfIndex;
    '    };
    '  };
    '  struct _IP_ADAPTER_ADDRESSES  *Next;
    '  PCHAR                              AdapterName;
    '  PIP_ADAPTER_UNICAST_ADDRESS        FirstUnicastAddress;
    '  PIP_ADAPTER_ANYCAST_ADDRESS        FirstAnycastAddress;
    '  PIP_ADAPTER_MULTICAST_ADDRESS      FirstMulticastAddress;
    '  PIP_ADAPTER_DNS_SERVER_ADDRESS     FirstDnsServerAddress;
    '  PWCHAR                             DnsSuffix;
    '  PWCHAR                             Description;
    '  PWCHAR                             FriendlyName;
    '  BYTE                               PhysicalAddress[MAX_ADAPTER_ADDRESS_LENGTH];
    '  DWORD                              PhysicalAddressLength;
    '  DWORD                              Flags;
    '  DWORD                              Mtu;
    '  DWORD                              IfType;
    '  IF_OPER_STATUS                     OperStatus;
    '  DWORD                              Ipv6IfIndex;
    '  DWORD                              ZoneIndices[16];
    '  PIP_ADAPTER_PREFIX                 FirstPrefix;
    '  ULONG64                            TransmitLinkSpeed;
    '  ULONG64                            ReceiveLinkSpeed;
    '  PIP_ADAPTER_WINS_SERVER_ADDRESS_LH FirstWinsServerAddress;
    '  PIP_ADAPTER_GATEWAY_ADDRESS_LH     FirstGatewayAddress;
    '  ULONG                              Ipv4Metric;
    '  ULONG                              Ipv6Metric;
    '  IF_LUID                            Luid;
    '  SOCKET_ADDRESS                     Dhcpv4Server;
    '  NET_IF_COMPARTMENT_ID              CompartmentId;
    '  NET_IF_NETWORK_GUID                NetworkGuid;
    '  NET_IF_CONNECTION_TYPE             ConnectionType;
    '  TUNNEL_TYPE                        TunnelType;
    '  SOCKET_ADDRESS                     Dhcpv6Server;
    '  BYTE                               Dhcpv6ClientDuid[MAX_DHCPV6_DUID_LENGTH];
    '  ULONG                              Dhcpv6ClientDuidLength;
    '  ULONG                              Dhcpv6Iaid;
    '  PIP_ADAPTER_DNS_SUFFIX             FirstDnsSuffix;
    '} IP_ADAPTER_ADDRESSES, *PIP_ADAPTER_ADDRESSES;

#End Region

    ''' <summary>
    ''' The almighty IP_ADAPTER_ADDRESSES structure.
    ''' </summary>
    ''' <remarks>Do not use this structure with wanton abandon.</remarks>
    <StructLayout(LayoutKind.Sequential), TypeConverter(GetType(ExpandableObjectConverter))>
    Public Structure IP_ADAPTER_ADDRESSES

        ''' <summary>
        ''' The header of type <see cref="IP_ADAPTER_HEADER_UNION"/>
        ''' </summary>
        <MarshalAs(UnmanagedType.Struct), Browsable(True)>
        Public Header As IP_ADAPTER_HEADER_UNION

        ''' <summary>
        ''' Pointer to the next IP_ADAPTER_ADDRESSES structure.
        ''' </summary>
        ''' <remarks></remarks>
        Public [Next] As LPIP_ADAPTER_ADDRESSES

        ''' <summary>
        ''' The GUID name of the adapter.
        ''' </summary>
        ''' <remarks></remarks>
        <MarshalAs(UnmanagedType.LPStr), Browsable(True)>
        Public AdapterName As String

        ''' <summary>
        ''' What most people think of as their IP address is stored here, in a chain of addresses.
        ''' The element in the structure typically refers to an IPv6 address,
        ''' while the next one in the chain (FirstUnicastAddress.Next) referers to
        ''' your IPv4 address.
        ''' </summary>
        ''' <remarks></remarks>
        <Browsable(True)>
        Public FirstUnicastAddress As LPADAPTER_UNICAST_ADDRESS


        ''' <summary>
        ''' First anycast address
        ''' </summary>
        <Browsable(True)>
        Public FirstAnycastAddress As LPADAPTER_MULTICAST_ADDRESS

        ''' <summary>
        ''' First multicast address
        ''' </summary>
        <Browsable(True)>
        Public FirstMulticastAddress As LPADAPTER_MULTICAST_ADDRESS

        ''' <summary>
        ''' For DNS server address
        ''' </summary>
        <Browsable(True)>
        Public FirstDnsServerAddress As LPADAPTER_MULTICAST_ADDRESS

        ''' <summary>
        ''' This is your domain name, typically your ISP (poolxxxx-verizon.net, 2wire.att.net, etc...)
        ''' </summary>
        ''' <remarks></remarks>
        <MarshalAs(UnmanagedType.LPWStr), Browsable(True)>
        Public DnsSuffix As String

        ''' <summary>
        ''' This is always the friendly name of the hardware instance of the network adapter.
        ''' </summary>
        ''' <remarks></remarks>
        <MarshalAs(UnmanagedType.LPWStr), Browsable(True)>
        Public Description As String

        ''' <summary>
        ''' Friendly name of the network connection (e.g. Ethernet 2, Wifi 1, etc..)
        ''' </summary>
        ''' <remarks></remarks>
        <MarshalAs(UnmanagedType.LPWStr), Browsable(True)>
        Public FriendlyName As String

        ''' <summary>
        ''' The adapter's MAC address.
        ''' </summary>
        ''' <remarks></remarks>
        <Browsable(True)>
        Public PhysicalAddress As MACADDRESS

        ''' <summary>
        ''' The length of the adapter's MAC address.
        ''' </summary>
        ''' <remarks></remarks>
        <Browsable(True)>
        Public PhysicalAddressLength As UInteger

        ''' <summary>
        ''' The adapter's capabilities and flags.
        ''' </summary>
        ''' <remarks></remarks>
        <Browsable(True)>
        Public Flags As IPAdapterAddressesFlags

        ''' <summary>
        ''' The maximum transmission unit of the connection.
        ''' </summary>
        ''' <remarks></remarks>
        <Browsable(True)>
        Public Mtu As Integer

        ''' <summary>
        ''' The adapter interface type.  Typically either 'ETHERNET_CSMACD' for
        ''' wired adapters and 'IEEE80211' for wifi adapters.
        ''' </summary>
        ''' <remarks></remarks>
        <Browsable(True)>
        Public IfType As IFTYPE

        ''' <summary>
        ''' The current operational status (up/down) of the device.
        ''' </summary>
        ''' <remarks></remarks>
        <Browsable(True)>
        Public OperStatus As IF_OPER_STATUS

        ''' <summary>
        ''' Ipv6 Interface Index
        ''' </summary>
        <Browsable(True)>
        Public Ipv6IfIndex As UInteger


        ''' <summary>
        ''' Zone indices
        ''' </summary>
        <MarshalAs(UnmanagedType.ByValArray, ArraySubType:=UnmanagedType.U4, SizeConst:=16)>
        Public ZoneIndices() As UInteger

        ''' <summary>
        ''' First <see cref="LPIP_ADAPTER_PREFIX"/>
        ''' </summary>
        <Browsable(True)>
        Public FirstPrefix As LPIP_ADAPTER_PREFIX

        ''' <summary>
        ''' Transmit link speed
        ''' </summary>
        <Browsable(True)>
        Public TransmitLinkSpeed As ULong

        ''' <summary>
        ''' Receive link speed
        ''' </summary>
        <Browsable(True)>
        Public ReceiveLinkSpeed As ULong


        ''' <summary>
        ''' First WINS server address
        ''' </summary>
        <Browsable(True)>
        Public FirstWinsServerAddress As LPADAPTER_MULTICAST_ADDRESS

        ''' <summary>
        ''' First gateway address
        ''' </summary>
        <Browsable(True)>
        Public FirstGatewayAddress As LPADAPTER_MULTICAST_ADDRESS

        ''' <summary>
        ''' Ipv4 Metric
        ''' </summary>
        <Browsable(True)>
        Public Ipv4Metric As UInteger

        ''' <summary>
        ''' Ipv6 Metric
        ''' </summary>
        <Browsable(True)>
        Public Ipv6Metric As UInteger

        ''' <summary>
        ''' LUID
        ''' </summary>
        <Browsable(True)>
        Public Luid As LUID

        ''' <summary>
        ''' DHCP v4 server
        ''' </summary>
        <Browsable(True)>
        Public Dhcp4Server As SOCKET_ADDRESS

        ''' <summary>
        ''' Compartment ID
        ''' </summary>
        <Browsable(True)>
        Public CompartmentId As UInt32

        ''' <summary>
        ''' Network GUID
        ''' </summary>
        <Browsable(True)>
        Public NetworkGuid As Guid

        ''' <summary>
        ''' Connection type
        ''' </summary>
        <Browsable(True)>
        Public ConnectionType As NET_IF_CONNECTION_TYPE

        ''' <summary>
        ''' Tunnel type
        ''' </summary>
        <Browsable(True)>
        Public TunnelType As TUNNEL_TYPE

        ''' <summary>
        ''' DHCP v6 server
        ''' </summary>
        <Browsable(True)>
        Public Dhcpv6Server As SOCKET_ADDRESS

        ''' <summary>
        ''' DHCP v6 Client DUID
        ''' </summary>
        <MarshalAs(UnmanagedType.ByValArray, ArraySubType:=UnmanagedType.U1, SizeConst:=MAX_DHCPV6_DUID_LENGTH)>
        Public Dhcpv6ClientDuid As Byte()

        ''' <summary>
        ''' DHCP v6 DUID Length
        ''' </summary>
        <Browsable(True)>
        Public Dhcpv6ClientDuidLength As UInteger

        ''' <summary>
        ''' DHCP v6 AIID
        ''' </summary>
        <Browsable(True)>
        Public Dhcpv6Iaid As UInteger

        ''' <summary>
        ''' First DNS suffix
        ''' </summary>
        <Browsable(True)>
        Public FirstDnsSuffix As LPIP_ADAPTER_DNS_SUFFIX

        ''' <summary>
        ''' Returns the adapter's friendly name.
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function ToString() As String
            Return FriendlyName
        End Function

    End Structure

    ''' <summary>
    ''' Creatively marshaled pointerized structure for the IP_ADAPTER_ADDRESSES structure.
    ''' </summary>
    ''' <remarks></remarks>
    <StructLayout(LayoutKind.Sequential), TypeConverter(GetType(ExpandableObjectConverter))>
    Public Structure LPIP_ADAPTER_ADDRESSES
        Public Handle As MemPtr

        Public Overrides Function ToString() As String
            If Handle.Handle = IntPtr.Zero Then Return "NULL"
            Return Struct.FriendlyName
        End Function

        Public ReadOnly Property [Next] As LPIP_ADAPTER_ADDRESSES
            Get
                Return Struct.Next
            End Get
        End Property

        Public ReadOnly Property Struct As IP_ADAPTER_ADDRESSES
            Get
                Struct = ToAdapterStruct()
            End Get
        End Property

        Public Function ToAdapterStruct() As IP_ADAPTER_ADDRESSES
            If Handle = IntPtr.Zero Then Return Nothing
            ToAdapterStruct = Handle.ToStruct(Of IP_ADAPTER_ADDRESSES)
        End Function

        Public Sub Dispose()
            Handle.Free()
        End Sub

        Public Shared Widening Operator CType(operand As IP_ADAPTER_ADDRESSES) As LPIP_ADAPTER_ADDRESSES
            Dim a As New LPIP_ADAPTER_ADDRESSES
            Dim cb As Integer = Marshal.SizeOf(a)
            a.Handle.Alloc(cb)
            Marshal.StructureToPtr(operand, a.Handle, True)
            Return a
        End Operator

        Public Shared Widening Operator CType(operand As LPIP_ADAPTER_ADDRESSES) As IP_ADAPTER_ADDRESSES
            Dim a As IP_ADAPTER_ADDRESSES = operand.Handle.ToStruct(Of IP_ADAPTER_ADDRESSES)
            Return a
        End Operator

        Public Shared Widening Operator CType(operand As IntPtr) As LPIP_ADAPTER_ADDRESSES
            Dim a As New LPIP_ADAPTER_ADDRESSES
            a.Handle = operand
            Return a
        End Operator

        Public Shared Widening Operator CType(operand As LPIP_ADAPTER_ADDRESSES) As IntPtr
            Return operand.Handle.Handle
        End Operator

        Public Shared Widening Operator CType(operand As MemPtr) As LPIP_ADAPTER_ADDRESSES
            Dim a As New LPIP_ADAPTER_ADDRESSES
            a.Handle = operand
            Return a
        End Operator

        Public Shared Widening Operator CType(operand As LPIP_ADAPTER_ADDRESSES) As MemPtr
            Return operand.Handle
        End Operator

    End Structure

#End Region

#Region "GAA_FLAGS"

    ''' <summary>
    ''' IP Adapter enumeration function flags.
    ''' </summary>
    ''' <remarks></remarks>
    <Flags>
    Public Enum GAA_FLAGS

        ''' <summary>
        ''' Do not return unicast addresses.
        ''' </summary>
        ''' <remarks></remarks>
        GAA_FLAG_SKIP_UNICAST = &H1

        ''' <summary>
        ''' Do not return IPv6 anycast addresses.
        ''' </summary>
        ''' <remarks></remarks>
        GAA_FLAG_SKIP_ANYCAST = &H2

        ''' <summary>
        ''' Do not return multicast addresses.
        ''' </summary>
        ''' <remarks></remarks>
        GAA_FLAG_SKIP_MULTICAST = &H4

        ''' <summary>
        ''' Do not return addresses of DNS servers.
        ''' </summary>
        ''' <remarks></remarks>
        GAA_FLAG_SKIP_DNS_SERVER = &H8

        ''' <summary>
        ''' Return a list of IP address prefixes on this adapter. When this flag is set, IP address prefixes are returned for both IPv6 and IPv4 addresses.
        ''' </summary>
        ''' <remarks></remarks>
        GAA_FLAG_INCLUDE_PREFIX = &H10

        ''' <summary>
        ''' Do not return the adapter friendly name.
        ''' This flag is supported on Windows XP with SP1 and later.
        ''' </summary>
        ''' <remarks></remarks>
        GAA_FLAG_SKIP_FRIENDLY_NAME = &H20

        ''' <summary>
        ''' Return addresses of Windows Internet Name Service (WINS) servers.
        ''' </summary>
        ''' <remarks></remarks>
        GAA_FLAG_INCLUDE_WINS_INFO = &H40

        ''' <summary>
        ''' Return the addresses of default gateways.
        ''' This flag is supported on Windows Vista and later.
        ''' </summary>
        ''' <remarks></remarks>
        GAA_FLAG_INCLUDE_GATEWAYS = &H80

        ''' <summary>
        ''' Return addresses for all NDIS interfaces.
        ''' This flag is supported on Windows Vista and later.
        ''' </summary>
        ''' <remarks></remarks>
        GAA_FLAG_INCLUDE_ALL_INTERFACES = &H100

        ''' <summary>
        ''' Return addresses in all routing compartments.
        ''' </summary>
        ''' <remarks></remarks>
        GAA_FLAG_INCLUDE_ALL_COMPARTMENTS = &H200

        ''' <summary>
        ''' Return the adapter addresses sorted in tunnel binding order. This flag is supported on Windows Vista and later.
        ''' </summary>
        ''' <remarks></remarks>
        GAA_FLAG_INCLUDE_TUNNEL_BINDINGORDER = &H400

    End Enum

#End Region

#Region "Adapter Enumeration Result"


    ''' <summary>
    ''' Adapter enumeration result
    ''' </summary>
    Public Enum ADAPTER_ENUM_RESULT
        ''' <summary>
        ''' Success
        ''' </summary>
        ''' <remarks></remarks>
        NO_ERROR = 0

        ''' <summary>
        ''' An address has not yet been associated with the network endpoint.DHCP lease information was available.
        ''' </summary>
        ''' <remarks></remarks>
        ERROR_ADDRESS_NOT_ASSOCIATED = 1228

        ''' <summary>
        ''' The buffer size indicated by the SizePointer parameter is too small to hold the adapter information or the AdapterAddresses parameter is NULL.The SizePointer parameter returned points to the required size of the buffer to hold the adapter information.
        ''' </summary>
        ''' <remarks></remarks>
        ERROR_BUFFER_OVERFLOW = 111

        ''' <summary>
        ''' One of the parameters is invalid.This error is returned for any of the following conditions : the SizePointer parameter is NULL, the Address parameter is not AfInet, AfInet6, or AfUnspecified, or the address information for the parameters requested is greater than ULONG_MAX.
        ''' </summary>
        ''' <remarks></remarks>
        ERROR_INVALID_PARAMETER = 87

        ''' <summary>
        ''' Insufficient memory resources are available to complete the operation.
        ''' </summary>
        ''' <remarks></remarks>
        ERROR_NOT_ENOUGH_MEMORY = 8

        ''' <summary>
        ''' No addresses were found for the requested parameters.
        ''' </summary>
        ''' <remarks></remarks>
        ERROR_NO_DATA = 232

    End Enum

#End Region

    Public Module IfDefApi

        '' A lot of creative marshaling is used here.

        Public Const MAX_ADAPTER_ADDRESS_LENGTH = 8
        Public Const MAX_DHCPV6_DUID_LENGTH = 130

#Region "Functions"

        Declare Function GetAdaptersAddresses Lib "Iphlpapi.dll" _
            (Family As AfENUM,
             Flags As GAA_FLAGS,
             Reserved As IntPtr,
             Addresses As LPIP_ADAPTER_ADDRESSES,
             ByRef SizePointer As UInteger) As ADAPTER_ENUM_RESULT

        ''' <summary>
        ''' Retrieves a linked, unmanaged structure array of IP_ADAPTER_ADDRESSES, enumerating all network interfaces on the system.
        ''' This function is internal to the managed API in this assembly and is not intended to be used independently from it.
        ''' The results of this function are abstracted into the managed <see cref="AdaptersCollection" /> class. Use that, instead.
        ''' </summary>
        ''' <param name="origPtr">Receives the memory pointer for the memory allocated to retrieve the information from the system.</param>
        ''' <param name="noRelease">Specifies that the memory will not be released after usage (this is a typical scenario).</param>
        ''' <returns>A linked, unmanaged structure array of IP_ADAPTER_ADDRESSES.</returns>
        ''' <remarks></remarks>
        Friend Function GetAdapters(Optional ByRef origPtr As MemPtr = Nothing, Optional noRelease As Boolean = True) As IP_ADAPTER_ADDRESSES()
            Dim lpadapt As New LPIP_ADAPTER_ADDRESSES
            Dim adapt As IP_ADAPTER_ADDRESSES

            '' and this is barely enough on a typical system.
            lpadapt.Handle.Alloc(65536, noRelease)
            lpadapt.Handle.ZeroMemory()

            Dim ft As Integer = 0

            Dim cblen As UInteger = 65536
            Dim cb As Integer = Marshal.SizeOf(lpadapt)

            Dim res As ADAPTER_ENUM_RESULT = GetAdaptersAddresses(AfENUM.AfUnspecified,
                                                              GAA_FLAGS.GAA_FLAG_INCLUDE_GATEWAYS Or
                                                              GAA_FLAGS.GAA_FLAG_INCLUDE_WINS_INFO Or
                                                              GAA_FLAGS.GAA_FLAG_INCLUDE_ALL_COMPARTMENTS Or
                                                              GAA_FLAGS.GAA_FLAG_INCLUDE_ALL_INTERFACES,
                                                              IntPtr.Zero,
                                                              lpadapt,
                                                              cblen)


            'Dim res As ADAPTER_ENUM_RESULT = GetAdaptersAddresses(AfENUM.AfUnspecified,
            '                                                      0, IntPtr.Zero,
            '                                                      lpadapt, cblen)


            '' we have a buffer overflow?  We need to get more memory.
            If res = ADAPTER_ENUM_RESULT.ERROR_BUFFER_OVERFLOW Then
                Do While res = ADAPTER_ENUM_RESULT.ERROR_BUFFER_OVERFLOW
                    lpadapt.Handle.ReAlloc(cblen, noRelease)

                    res = GetAdaptersAddresses(AfENUM.AfUnspecified,
                                            GAA_FLAGS.GAA_FLAG_INCLUDE_GATEWAYS Or
                                            GAA_FLAGS.GAA_FLAG_INCLUDE_WINS_INFO,
                                            IntPtr.Zero,
                                            lpadapt,
                                            cblen)

                    '' to make sure that we don't loop forever, in some weird scenario.
                    ft += 1
                    If (ft > 300) Then Exit Do

                Loop
            ElseIf res <> ADAPTER_ENUM_RESULT.NO_ERROR Then
                lpadapt.Dispose()

                If IsNumeric(res.ToString) Then
                    Throw New NativeException
                Else
                    MsgBox("ADAPTER ENUMERATION ERROR: " & res.ToString)
                End If
            End If

            '' trim any excess memory.
            If cblen < 65536 Then
                lpadapt.Handle.ReAlloc(cblen, noRelease)
            End If

            origPtr = lpadapt

            Dim adapters() As IP_ADAPTER_ADDRESSES = Nothing
            Dim c As Integer = 0
            Dim cc As Integer = 0

            adapt = CType(lpadapt, IP_ADAPTER_ADDRESSES)

            Do
                If (adapt.Description = "") Or (adapt.FirstDnsServerAddress.Handle = IntPtr.Zero) Then
                    c += 1
                    adapt = adapt.Next
                    If adapt.Next.Handle.Handle = IntPtr.Zero Then Exit Do
                    Continue Do
                End If

                ReDim Preserve adapters(cc)
                adapters(cc) = adapt
                adapt = adapt.Next
                cc += 1
                c += 1
            Loop Until adapt.Next.Handle.Handle = IntPtr.Zero

            '' there is currently no reason for this function to free this pointer,
            '' but we reserve the right to do so, in the future.
            If Not noRelease Then origPtr.Free()

            Return adapters
        End Function

#End Region

    End Module

End Namespace