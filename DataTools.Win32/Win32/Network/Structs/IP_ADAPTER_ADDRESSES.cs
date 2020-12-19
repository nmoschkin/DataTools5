// ************************************************* ''
// DataTools C# Native Utility Library For Windows - Interop
//
// Module: IfDefApi
//         The almighty network interface native API.
//         Some enum documentation comes from the MSDN.
//
// (and an exercise in creative problem solving and data-structure marshaling.)
//
// Copyright (C) 2011-2020 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the Microsoft Public License   
// ************************************************* ''


using System;
using System.ComponentModel;
using System.Net;
using System.Runtime.InteropServices;
using DataTools.Memory;
using DataTools.Win32;

namespace DataTools.Win32.Network
{
    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    // typedef struct _IP_ADAPTER_ADDRESSES {
    // union {
    // ULONGLONG Alignment;
    // struct {
    // ULONG Length;
    // DWORD IfIndex;
    // };
    // };
    // struct _IP_ADAPTER_ADDRESSES  *Next;
    // PCHAR                              AdapterName;
    // PIP_ADAPTER_UNICAST_ADDRESS        FirstUnicastAddress;
    // PIP_ADAPTER_ANYCAST_ADDRESS        FirstAnycastAddress;
    // PIP_ADAPTER_MULTICAST_ADDRESS      FirstMulticastAddress;
    // PIP_ADAPTER_DNS_SERVER_ADDRESS     FirstDnsServerAddress;
    // PWCHAR                             DnsSuffix;
    // PWCHAR                             Description;
    // PWCHAR                             FriendlyName;
    // BYTE                               PhysicalAddress[MAX_ADAPTER_ADDRESS_LENGTH];
    // DWORD                              PhysicalAddressLength;
    // DWORD                              Flags;
    // DWORD                              Mtu;
    // DWORD                              IfType;
    // IF_OPER_STATUS                     OperStatus;
    // DWORD                              Ipv6IfIndex;
    // DWORD                              ZoneIndices[16];
    // PIP_ADAPTER_PREFIX                 FirstPrefix;
    // ULONG64                            TransmitLinkSpeed;
    // ULONG64                            ReceiveLinkSpeed;
    // PIP_ADAPTER_WINS_SERVER_ADDRESS_LH FirstWinsServerAddress;
    // PIP_ADAPTER_GATEWAY_ADDRESS_LH     FirstGatewayAddress;
    // ULONG                              Ipv4Metric;
    // ULONG                              Ipv6Metric;
    // IF_LUID                            Luid;
    // SOCKET_ADDRESS                     Dhcpv4Server;
    // NET_IF_COMPARTMENT_ID              CompartmentId;
    // NET_IF_NETWORK_GUID                NetworkGuid;
    // NET_IF_CONNECTION_TYPE             ConnectionType;
    // TUNNEL_TYPE                        TunnelType;
    // SOCKET_ADDRESS                     Dhcpv6Server;
    // BYTE                               Dhcpv6ClientDuid[MAX_DHCPV6_DUID_LENGTH];
    // ULONG                              Dhcpv6ClientDuidLength;
    // ULONG                              Dhcpv6Iaid;
    // PIP_ADAPTER_DNS_SUFFIX             FirstDnsSuffix;
    // } IP_ADAPTER_ADDRESSES, *PIP_ADAPTER_ADDRESSES;

    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    /// <summary>
    /// The almighty IP_ADAPTER_ADDRESSES structure.
    /// </summary>
    /// <remarks>Do not use this structure with wanton abandon.</remarks>
    [StructLayout(LayoutKind.Sequential)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public struct IP_ADAPTER_ADDRESSES
    {
        /// <summary>
        /// The header of type <see cref="IP_ADAPTER_HEADER_UNION"/>
        /// </summary>
        [MarshalAs(UnmanagedType.Struct)]
        [Browsable(true)]
        public IP_ADAPTER_HEADER_UNION Header;

        /// <summary>
        /// Pointer to the next IP_ADAPTER_ADDRESSES structure.
        /// </summary>
        /// <remarks></remarks>
        public LPIP_ADAPTER_ADDRESSES Next;

        /// <summary>
        /// The GUID name of the adapter.
        /// </summary>
        /// <remarks></remarks>
        [MarshalAs(UnmanagedType.LPStr)]
        [Browsable(true)]
        public string AdapterName;

        /// <summary>
        /// What most people think of as their IP address is stored here, in a chain of addresses.
        /// The element in the structure typically refers to an IPv6 address,
        /// while the next one in the chain (FirstUnicastAddress.Next) referers to
        /// your IPv4 address.
        /// </summary>
        /// <remarks></remarks>
        [Browsable(true)]
        public LPADAPTER_UNICAST_ADDRESS FirstUnicastAddress;


        /// <summary>
        /// First anycast address
        /// </summary>
        [Browsable(true)]
        public LPADAPTER_MULTICAST_ADDRESS FirstAnycastAddress;

        /// <summary>
        /// First multicast address
        /// </summary>
        [Browsable(true)]
        public LPADAPTER_MULTICAST_ADDRESS FirstMulticastAddress;

        /// <summary>
        /// For DNS server address
        /// </summary>
        [Browsable(true)]
        public LPADAPTER_MULTICAST_ADDRESS FirstDnsServerAddress;

        /// <summary>
        /// This is your domain name, typically your ISP (poolxxxx-verizon.net, 2wire.att.net, etc...)
        /// </summary>
        /// <remarks></remarks>
        [MarshalAs(UnmanagedType.LPWStr)]
        [Browsable(true)]
        public string DnsSuffix;

        /// <summary>
        /// This is always the friendly name of the hardware instance of the network adapter.
        /// </summary>
        /// <remarks></remarks>
        [MarshalAs(UnmanagedType.LPWStr)]
        [Browsable(true)]
        public string Description;

        /// <summary>
        /// Friendly name of the network connection (e.g. Ethernet 2, Wifi 1, etc..)
        /// </summary>
        /// <remarks></remarks>
        [MarshalAs(UnmanagedType.LPWStr)]
        [Browsable(true)]
        public string FriendlyName;

        /// <summary>
        /// The adapter's MAC address.
        /// </summary>
        /// <remarks></remarks>
        [Browsable(true)]
        public MACADDRESS PhysicalAddress;

        /// <summary>
        /// The length of the adapter's MAC address.
        /// </summary>
        /// <remarks></remarks>
        [Browsable(true)]
        public uint PhysicalAddressLength;

        /// <summary>
        /// The adapter's capabilities and flags.
        /// </summary>
        /// <remarks></remarks>
        [Browsable(true)]
        public IPAdapterAddressesFlags Flags;

        /// <summary>
        /// The maximum transmission unit of the connection.
        /// </summary>
        /// <remarks></remarks>
        [Browsable(true)]
        public int Mtu;

        /// <summary>
        /// The adapter interface type.  Typically either 'ETHERNET_CSMACD' for
        /// wired adapters and 'IEEE80211' for wifi adapters.
        /// </summary>
        /// <remarks></remarks>
        [Browsable(true)]
        public IFTYPE IfType;

        /// <summary>
        /// The current operational status (up/down) of the device.
        /// </summary>
        /// <remarks></remarks>
        [Browsable(true)]
        public IF_OPER_STATUS OperStatus;

        /// <summary>
        /// Ipv6 Interface Index
        /// </summary>
        [Browsable(true)]
        public uint Ipv6IfIndex;


        /// <summary>
        /// Zone indices
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U4, SizeConst = 16)]
        public uint[] ZoneIndices;

        /// <summary>
        /// First <see cref="LPIP_ADAPTER_PREFIX"/>
        /// </summary>
        [Browsable(true)]
        public LPIP_ADAPTER_PREFIX FirstPrefix;

        /// <summary>
        /// Transmit link speed
        /// </summary>
        [Browsable(true)]
        public ulong TransmitLinkSpeed;

        /// <summary>
        /// Receive link speed
        /// </summary>
        [Browsable(true)]
        public ulong ReceiveLinkSpeed;


        /// <summary>
        /// First WINS server address
        /// </summary>
        [Browsable(true)]
        public LPADAPTER_MULTICAST_ADDRESS FirstWinsServerAddress;

        /// <summary>
        /// First gateway address
        /// </summary>
        [Browsable(true)]
        public LPADAPTER_MULTICAST_ADDRESS FirstGatewayAddress;

        /// <summary>
        /// Ipv4 Metric
        /// </summary>
        [Browsable(true)]
        public uint Ipv4Metric;

        /// <summary>
        /// Ipv6 Metric
        /// </summary>
        [Browsable(true)]
        public uint Ipv6Metric;

        /// <summary>
        /// LUID
        /// </summary>
        [Browsable(true)]
        public LUID Luid;

        /// <summary>
        /// DHCP v4 server
        /// </summary>
        [Browsable(true)]
        public SOCKET_ADDRESS Dhcp4Server;

        /// <summary>
        /// Compartment ID
        /// </summary>
        [Browsable(true)]
        public uint CompartmentId;

        /// <summary>
        /// Network GUID
        /// </summary>
        [Browsable(true)]
        public Guid NetworkGuid;

        /// <summary>
        /// Connection type
        /// </summary>
        [Browsable(true)]
        public NET_IF_CONNECTION_TYPE ConnectionType;

        /// <summary>
        /// Tunnel type
        /// </summary>
        [Browsable(true)]
        public TUNNEL_TYPE TunnelType;

        /// <summary>
        /// DHCP v6 server
        /// </summary>
        [Browsable(true)]
        public SOCKET_ADDRESS Dhcpv6Server;

        /// <summary>
        /// DHCP v6 Client DUID
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = IfDefApi.MAX_DHCPV6_DUID_LENGTH)]
        public byte[] Dhcpv6ClientDuid;

        /// <summary>
        /// DHCP v6 DUID Length
        /// </summary>
        [Browsable(true)]
        public uint Dhcpv6ClientDuidLength;

        /// <summary>
        /// DHCP v6 AIID
        /// </summary>
        [Browsable(true)]
        public uint Dhcpv6Iaid;

        /// <summary>
        /// First DNS suffix
        /// </summary>
        [Browsable(true)]
        public LPIP_ADAPTER_DNS_SUFFIX FirstDnsSuffix;

        /// <summary>
        /// Returns the adapter's friendly name.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return FriendlyName;
        }
    }
}
