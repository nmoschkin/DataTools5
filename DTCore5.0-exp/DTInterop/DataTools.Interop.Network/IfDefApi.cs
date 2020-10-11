// ' ************************************************* ''
// ' DataTools Visual Basic Utility Library - Interop
// '
// ' Module: IfDefApi
// '         The almighty network interface native API.
// '         Some enum documentation comes from the MSDN.
// '
// ' (and an exercise in creative problem solving and data-structure marshaling.)
// '
// ' Copyright (C) 2011-2020 Nathan Moschkin
// ' All Rights Reserved
// '
// ' Licensed Under the Microsoft Public License   
// ' ************************************************* ''


using System;
using System.ComponentModel;
using System.Net;
using System.Runtime.InteropServices;
using CoreCT.Memory;
using DataTools.Interop.Native;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace DataTools.Interop.Network
{

    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    /// <summary>
    /// Network adapter address families.
    /// </summary>
    /// <remarks></remarks>
    public enum AddressFamily : ushort
    {

        /// <summary>
        /// unspecified
        /// </summary>
        /// <remarks></remarks>
        AfUnspecified = 0,

        /// <summary>
        /// local to host (pipes, portals)
        /// </summary>
        /// <remarks></remarks>
        AfUNIX = 1,

        /// <summary>
        /// internetwork: UDP, TCP, etc.
        /// </summary>
        /// <remarks></remarks>
        AfInet = 2,

        /// <summary>
        /// arpanet imp addresses
        /// </summary>
        /// <remarks></remarks>
        AfIMPLINK = 3,

        /// <summary>
        /// pup protocols: e.g. BSP
        /// </summary>
        /// <remarks></remarks>
        AfPUP = 4,

        /// <summary>
        /// mit CHAOS protocols
        /// </summary>
        /// <remarks></remarks>
        AfCHAOS = 5,

        /// <summary>
        /// XEROX NS protocols
        /// </summary>
        /// <remarks></remarks>
        AfNS = 6,

        /// <summary>
        /// IPX protocols: IPX, SPX, etc.
        /// </summary>
        /// <remarks></remarks>
        AfIPX = AfNS,

        /// <summary>
        /// ISO protocols
        /// </summary>
        /// <remarks></remarks>
        AfISO = 7,

        /// <summary>
        /// OSI is ISO
        /// </summary>
        /// <remarks></remarks>
        AfOSI = AfISO,

        /// <summary>
        /// european computer manufacturers
        /// </summary>
        /// <remarks></remarks>
        AfECMA = 8,

        /// <summary>
        /// datakit protocols
        /// </summary>
        /// <remarks></remarks>
        AfDataKit = 9,

        /// <summary>
        /// CCITT protocols, X.25 etc
        /// </summary>
        /// <remarks></remarks>
        AfCCITT = 10,

        /// <summary>
        /// IBM SNA
        /// </summary>
        /// <remarks></remarks>
        AfSNA = 11,

        /// <summary>
        /// DECnet
        /// </summary>
        /// <remarks></remarks>
        AfDECnet = 12,

        /// <summary>
        /// Direct data link interface
        /// </summary>
        /// <remarks></remarks>
        AfDLI = 13,

        /// <summary>
        /// LAT
        /// </summary>
        /// <remarks></remarks>
        AfLAT = 14,

        /// <summary>
        /// NSC Hyperchannel
        /// </summary>
        /// <remarks></remarks>
        AfHYLINK = 15,

        /// <summary>
        /// AppleTalk
        /// </summary>
        /// <remarks></remarks>
        AfAppleTalk = 16,

        /// <summary>
        /// NetBios-style addresses
        /// </summary>
        /// <remarks></remarks>
        AfNetBios = 17,

        /// <summary>
        /// VoiceView
        /// </summary>
        /// <remarks></remarks>
        AfVoiceView = 18,

        /// <summary>
        /// Protocols from Firefox
        /// </summary>
        /// <remarks></remarks>
        AfFirefox = 19,

        /// <summary>
        /// Somebody is using this!
        /// </summary>
        /// <remarks></remarks>
        AfUnknown1 = 20,

        /// <summary>
        /// Banyan
        /// </summary>
        /// <remarks></remarks>
        AfBAN = 21,

        /// <summary>
        /// Native ATM Services
        /// </summary>
        /// <remarks></remarks>
        AfATM = 22,

        /// <summary>
        /// Internetwork Version 6
        /// </summary>
        /// <remarks></remarks>
        AfInet6 = 23,

        /// <summary>
        /// Microsoft Wolfpack
        /// </summary>
        /// <remarks></remarks>
        AfCLUSTER = 24,

        /// <summary>
        /// IEEE 1284.4 WG AF
        /// </summary>
        /// <remarks></remarks>
        Af12844 = 25,

        /// <summary>
        /// IrDA
        /// </summary>
        /// <remarks></remarks>
        AfIRDA = 26,

        /// <summary>
        /// Network Designers OSI &amp; gateway
        /// </summary>
        /// <remarks></remarks>
        AfNETDES = 28,

        /// <summary>
        /// 
        /// </summary>
        /// <remarks></remarks>
        AfTCNProcess = 29,

        /// <summary>
        /// 
        /// </summary>
        /// <remarks></remarks>
        AfTCNMessage = 30,

        /// <summary>
        /// 
        /// </summary>
        /// <remarks></remarks>
        AfICLFXBM = 31
    }

    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    /// <summary>
    /// Network adapter enumerator-allowed address families.
    /// </summary>
    /// <remarks></remarks>
    public enum AfENUM : uint
    {

        /// <summary>
        /// Return both IPv4 and IPv6 addresses associated with adapters with IPv4 or IPv6 enabled.
        /// </summary>
        /// <remarks></remarks>
        AfUnspecified = 0U,

        /// <summary>
        /// Return only IPv4 addresses associated with adapters with IPv4 enabled.
        /// </summary>
        /// <remarks></remarks>
        AfInet = 2U,

        /// <summary>
        /// Return only IPv6 addresses associated with adapters with IPv6 enabled.
        /// </summary>
        /// <remarks></remarks>
        AfInet6 = 23U
    }

    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    /// <summary>
    /// Represents an IPv4 socket address.
    /// </summary>
    /// <remarks></remarks>
    [StructLayout(LayoutKind.Sequential)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public struct SOCKADDR
    {
        /// <summary>
        /// Address family.
        /// </summary>
        /// <remarks></remarks>
        public AddressFamily AddressFamily;

        /// <summary>
        /// Address port.
        /// </summary>
        /// <remarks></remarks>
        public ushort Port;

        /// <summary>
        /// Address data.
        /// </summary>
        /// <remarks></remarks>
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 4)]
        public byte[] Data;

        /// <summary>
        /// Reserved, must be zero.
        /// </summary>
        /// <remarks></remarks>
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 8)]
        public byte[] Zero;

        /// <summary>
        /// Gets the IP address for this structure from the Data buffer.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public IPAddress Address
        {
            get
            {
                return new IPAddress(Data);
            }
        }

        public override string ToString()
        {
            if (Data is null)
                return "NULL";
            return "" + new IPAddress(Data).ToString() + " (" + AddressFamily.ToString() + ")";
        }
    }

    /// <summary>
    /// Represents an IPv6 socket address.
    /// </summary>
    /// <remarks></remarks>
    [StructLayout(LayoutKind.Sequential)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public struct SOCKADDRV6
    {

        /// <summary>
        /// Address family.
        /// </summary>
        /// <remarks></remarks>
        public AddressFamily AddressFamily;

        /// <summary>
        /// Address port.
        /// </summary>
        /// <remarks></remarks>
        public ushort Port;

        /// <summary>
        /// Address data.
        /// </summary>
        /// <remarks></remarks>
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 16)]
        public byte[] Data;

        /// <summary>
        /// Reserved, must be zero.
        /// </summary>
        /// <remarks></remarks>
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 8)]
        public byte[] Zero;

        /// <summary>
        /// Gets the IP address for this structure from the Data buffer.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public IPAddress Address
        {
            get
            {
                return new IPAddress(Data);
            }
        }

        public override string ToString()
        {
            if (Data is null)
                return "NULL";
            return "" + new IPAddress(Data).ToString() + " (" + AddressFamily.ToString() + ")";
        }
    }

    /// <summary>
    /// Structure that encapsulates the marshaling of a live memory pointer to either a SOCKADDR or a SOCKADDRV6
    /// </summary>
    /// <remarks></remarks>
    [StructLayout(LayoutKind.Sequential)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public struct LPSOCKADDR
    {
        public MemPtr Handle;

        public override string ToString()
        {
            if (Handle.Handle == IntPtr.Zero)
                return "NULL";
            return "" + IPAddress.ToString() + " (" + AddressFamily.ToString() + ")";
        }

        public IPAddress IPAddress
        {
            get
            {
                if (Data is null)
                    return null;
                return new IPAddress(Data);
            }
        }

        public SOCKADDR IPAddrV4
        {
            get
            {
                SOCKADDR IPAddrV4Ret = default;
                if (AddressFamily == AddressFamily.AfInet6)
                    return default;
                IPAddrV4Ret = ToSockAddr();
                return IPAddrV4Ret;
            }
        }

        public SOCKADDRV6 IPAddrV6
        {
            get
            {
                SOCKADDRV6 IPAddrV6Ret = default;
                if (AddressFamily == AddressFamily.AfInet)
                    return new SOCKADDRV6();
                IPAddrV6Ret = ToSockAddr6();
                return IPAddrV6Ret;
            }
        }

        public SOCKADDR ToSockAddr()
        {
            SOCKADDR ToSockAddrRet = default;
            if (Handle == IntPtr.Zero)
                return new SOCKADDR();
            ToSockAddrRet = Handle.ToStruct<SOCKADDR>();
            return ToSockAddrRet;
        }

        public SOCKADDRV6 ToSockAddr6()
        {
            SOCKADDRV6 ToSockAddr6Ret = default;
            if (Handle == IntPtr.Zero)
                return default;
            ToSockAddr6Ret = Handle.ToStruct<SOCKADDRV6>();
            return ToSockAddr6Ret;
        }

        public void Dispose()
        {
            Handle.Free();
        }

        public AddressFamily AddressFamily
        {
            get
            {
                if (Handle.Handle == IntPtr.Zero)
                    return AddressFamily.AfUnspecified;
                return ToSockAddr().AddressFamily;
            }
        }

        public byte[] Data
        {
            get
            {
                switch (AddressFamily)
                {
                    case AddressFamily.AfInet:
                        {
                            return IPAddrV4.Data;
                        }

                    default:
                        {
                            return IPAddrV6.Data;
                        }
                }
            }
        }

        public static implicit operator LPSOCKADDR(IntPtr operand)
        {
            var a = new LPSOCKADDR();
            a.Handle = operand;
            return a;
        }

        public static implicit operator IntPtr(LPSOCKADDR operand)
        {
            return operand.Handle.Handle;
        }

        public static implicit operator LPSOCKADDR(MemPtr operand)
        {
            var a = new LPSOCKADDR();
            a.Handle = operand;
            return a;
        }

        public static implicit operator MemPtr(LPSOCKADDR operand)
        {
            return operand.Handle;
        }

        public static implicit operator LPSOCKADDR(SOCKADDR operand)
        {
            var a = new LPSOCKADDR();
            a.Handle.Alloc(Marshal.SizeOf(operand));
            Marshal.StructureToPtr(operand, a.Handle.Handle, true);
            return a;
        }

        public static implicit operator SOCKADDR(LPSOCKADDR operand)
        {
            return operand.ToSockAddr();
        }

        public static implicit operator LPSOCKADDR(SOCKADDRV6 operand)
        {
            var a = new LPSOCKADDR();
            a.Handle.Alloc(Marshal.SizeOf(operand));
            Marshal.StructureToPtr(operand, a.Handle.Handle, true);
            return a;
        }

        public static implicit operator SOCKADDRV6(LPSOCKADDR operand)
        {
            return operand.ToSockAddr6();
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public struct SOCKET_ADDRESS
    {
        public LPSOCKADDR lpSockaddr;
        public int iSockaddrLength;

        public override string ToString()
        {
            string ToStringRet = default;
            if (lpSockaddr.Handle.Handle == IntPtr.Zero)
                return "NULL";
            ToStringRet = lpSockaddr.ToString();
            return ToStringRet;
        }
    }

    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    public enum IpDadState
    {
        IpDadStateInvalid = 0,
        IpDadStateTentative,
        IpDadStateDuplicate,
        IpDadStateDeprecated,
        IpDadStatePreferred
    }

    public enum IpPrefixOrigin
    {
        IpPrefixOriginOther = 0,
        IpPrefixOriginManual,
        IpPrefixOriginWellKnown,
        IpPrefixOriginDhcp,
        IpPrefixOriginRouterAdvertisement,
        IpPrefixOriginUnchanged = 0x10000
    }

    public enum IpSuffixOrigin
    {
        IpSuffixOriginOther = 0,
        IpSuffixOriginManual,
        IpSuffixOriginWellKnown,
        IpSuffixOriginDhcp,
        IpSuffixOriginLinkLayerAddress,
        IpSuffixOriginRandom,
        IpSuffixOriginUnchanged = 0x10000
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public struct IP_ADAPTER_DNS_SUFFIX
    {
        public LPIP_ADAPTER_DNS_SUFFIX Next;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string String;
    }

    /// <summary>
    /// Pointerized IP_ADAPTER_DNS_SUFFIX structure.
    /// </summary>
    /// <remarks></remarks>
    [StructLayout(LayoutKind.Sequential)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public struct LPIP_ADAPTER_DNS_SUFFIX
    {
        public MemPtr Handle;

        public LPIP_ADAPTER_DNS_SUFFIX[] Chain
        {
            get
            {
                if (Handle == IntPtr.Zero)
                    return null;
                int c = 0;
                var mx = this;
                var ac = default(LPIP_ADAPTER_DNS_SUFFIX[]);
                do
                {
                    Array.Resize(ref ac, c + 1);
                    ac[c] = mx;
                    mx = mx.Next;
                    c += 1;
                }
                while (mx.Handle.Handle != IntPtr.Zero);
                return ac;
            }
        }

        public LPIP_ADAPTER_DNS_SUFFIX Next
        {
            get
            {
                if (Handle == IntPtr.Zero)
                    return default;
                return Struct.Next;
            }
        }

        public override string ToString()
        {
            if (Handle.Handle == IntPtr.Zero)
                return "NULL";
            return Struct.String;
        }

        public IP_ADAPTER_DNS_SUFFIX Struct
        {
            get
            {
                IP_ADAPTER_DNS_SUFFIX StructRet = default;
                if (Handle == IntPtr.Zero)
                    return default;
                StructRet = ToStruct();
                return StructRet;
            }
        }

        public IP_ADAPTER_DNS_SUFFIX ToStruct()
        {
            IP_ADAPTER_DNS_SUFFIX ToStructRet = default;
            if (Handle == IntPtr.Zero)
                return default;
            ToStructRet = Handle.ToStruct<IP_ADAPTER_DNS_SUFFIX>();
            return ToStructRet;
        }

        public void Dispose()
        {
            Handle.Free();
        }

        public static implicit operator LPIP_ADAPTER_DNS_SUFFIX(IntPtr operand)
        {
            var a = new LPIP_ADAPTER_DNS_SUFFIX();
            a.Handle = operand;
            return a;
        }

        public static implicit operator IntPtr(LPIP_ADAPTER_DNS_SUFFIX operand)
        {
            return operand.Handle;
        }

        public static implicit operator LPIP_ADAPTER_DNS_SUFFIX(MemPtr operand)
        {
            var a = new LPIP_ADAPTER_DNS_SUFFIX();
            a.Handle = operand;
            return a;
        }

        public static implicit operator MemPtr(LPIP_ADAPTER_DNS_SUFFIX operand)
        {
            return operand.Handle;
        }

        public static implicit operator LPIP_ADAPTER_DNS_SUFFIX(IP_ADAPTER_DNS_SUFFIX operand)
        {
            var a = new LPIP_ADAPTER_DNS_SUFFIX();
            a.Handle.Alloc(Marshal.SizeOf(operand));
            Marshal.StructureToPtr(operand, a.Handle.Handle, true);
            return a;
        }

        public static implicit operator IP_ADAPTER_DNS_SUFFIX(LPIP_ADAPTER_DNS_SUFFIX operand)
        {
            return operand.Struct;
        }
    }

    /// <summary>
    /// IP adapter common structure header.
    /// </summary>
    /// <remarks></remarks>
    [StructLayout(LayoutKind.Sequential)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public struct IP_ADAPTER_HEADER_UNION
    {
        public ulong Alignment;

        /// <summary>
        /// Length of the structure
        /// </summary>
        /// <returns></returns>
        public uint Length
        {
            get
            {
                return (uint)((long)Alignment & 0xFFFFFFFFL);
            }
        }

        /// <summary>
        /// Interface index
        /// </summary>
        /// <returns></returns>
        public uint IfIndex
        {
            get
            {
                return (uint)((long)(Alignment >> 32) & 0xFFFFFFFFL);
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public struct IP_ADAPTER_PREFIX
    {
        public IP_ADAPTER_HEADER_UNION Header;
        public LPIP_ADAPTER_PREFIX Next;
        public SOCKET_ADDRESS Address;
        public uint Prefixlength;

        public override string ToString()
        {
            string ToStringRet = default;
            ToStringRet = Address.ToString();
            return ToStringRet;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public struct LPIP_ADAPTER_PREFIX
    {
        public MemPtr Handle;

        public LPIP_ADAPTER_PREFIX[] AddressChain
        {
            get
            {
                if (Handle == IntPtr.Zero)
                    return null;
                int c = 0;
                var mx = this;
                var ac = default(LPIP_ADAPTER_PREFIX[]);
                do
                {
                    Array.Resize(ref ac, c + 1);
                    ac[c] = mx;
                    mx = mx.Next;
                    c += 1;
                }
                while (mx.Handle.Handle != IntPtr.Zero);
                return ac;
            }
        }

        public LPIP_ADAPTER_PREFIX Next
        {
            get
            {
                if (Handle == IntPtr.Zero)
                    return default;
                return Struct.Next;
            }
        }

        public override string ToString()
        {
            if (Handle.Handle == IntPtr.Zero)
                return "NULL";
            return "" + IPAddress.ToString() + " (" + AddressFamily.ToString() + ")";
        }

        public IPAddress IPAddress
        {
            get
            {
                if (Handle == IntPtr.Zero)
                    return null;
                return Struct.Address.lpSockaddr.IPAddress;
            }
        }

        public IP_ADAPTER_PREFIX Struct
        {
            get
            {
                IP_ADAPTER_PREFIX StructRet = default;
                if (Handle == IntPtr.Zero)
                    return default;
                StructRet = ToAddress();
                return StructRet;
            }
        }

        public IP_ADAPTER_PREFIX ToAddress()
        {
            IP_ADAPTER_PREFIX ToAddressRet = default;
            if (Handle == IntPtr.Zero)
                return default;
            ToAddressRet = Handle.ToStruct<IP_ADAPTER_PREFIX>();
            return ToAddressRet;
        }

        public void Dispose()
        {
            Handle.Free();
        }

        public AddressFamily AddressFamily
        {
            get
            {
                if (Handle == IntPtr.Zero)
                    return default;
                return Struct.Address.lpSockaddr.AddressFamily;
            }
        }

        public byte[] Data
        {
            get
            {
                if (Handle == IntPtr.Zero)
                    return null;
                return Struct.Address.lpSockaddr.Data;
            }
        }

        public static implicit operator LPIP_ADAPTER_PREFIX(IntPtr operand)
        {
            var a = new LPIP_ADAPTER_PREFIX();
            a.Handle = operand;
            return a;
        }

        public static implicit operator IntPtr(LPIP_ADAPTER_PREFIX operand)
        {
            return operand.Handle;
        }

        public static implicit operator LPIP_ADAPTER_PREFIX(MemPtr operand)
        {
            var a = new LPIP_ADAPTER_PREFIX();
            a.Handle = operand;
            return a;
        }

        public static implicit operator MemPtr(LPIP_ADAPTER_PREFIX operand)
        {
            return operand.Handle;
        }

        public static implicit operator LPIP_ADAPTER_PREFIX(IP_ADAPTER_PREFIX operand)
        {
            var a = new LPIP_ADAPTER_PREFIX();
            a.Handle.Alloc(Marshal.SizeOf(operand));
            Marshal.StructureToPtr(operand, a.Handle.Handle, true);
            return a;
        }

        public static implicit operator IP_ADAPTER_PREFIX(LPIP_ADAPTER_PREFIX operand)
        {
            return operand.ToAddress();
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public struct ADAPTER_UNICAST_ADDRESS
    {
        public IP_ADAPTER_HEADER_UNION Header;
        public LPADAPTER_UNICAST_ADDRESS Next;
        public SOCKET_ADDRESS Address;
        public IpPrefixOrigin PrefixOrigin;
        public IpSuffixOrigin SuffixOrigin;
        public uint ValidLifetime;
        public uint PreferredLifetime;
        public uint LeaseLifetime;

        public override string ToString()
        {
            string ToStringRet = default;
            ToStringRet = Address.ToString();
            return ToStringRet;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public struct LPADAPTER_UNICAST_ADDRESS
    {
        public MemPtr Handle;

        public LPADAPTER_UNICAST_ADDRESS[] AddressChain
        {
            get
            {
                int c = 0;
                var mx = this;
                var ac = default(LPADAPTER_UNICAST_ADDRESS[]);
                do
                {
                    Array.Resize(ref ac, c + 1);
                    ac[c] = mx;
                    mx = mx.Next;
                    c += 1;
                }
                while (mx.Handle.Handle != IntPtr.Zero);
                return ac;
            }
        }

        public LPADAPTER_UNICAST_ADDRESS Next
        {
            get
            {
                return Struct.Next;
            }
        }

        public override string ToString()
        {
            if (Handle.Handle == IntPtr.Zero)
                return "NULL";
            return "" + IPAddress.ToString() + " (" + AddressFamily.ToString() + ")";
        }

        public IPAddress IPAddress
        {
            get
            {
                return Struct.Address.lpSockaddr.IPAddress;
            }
        }

        public ADAPTER_UNICAST_ADDRESS Struct
        {
            get
            {
                ADAPTER_UNICAST_ADDRESS StructRet = default;
                StructRet = ToAddress();
                return StructRet;
            }
        }

        public ADAPTER_UNICAST_ADDRESS ToAddress()
        {
            ADAPTER_UNICAST_ADDRESS ToAddressRet = default;
            if (Handle == IntPtr.Zero)
                return default;
            ToAddressRet = Handle.ToStruct<ADAPTER_UNICAST_ADDRESS>();
            return ToAddressRet;
        }

        public void Dispose()
        {
            Handle.Free();
        }

        public AddressFamily AddressFamily
        {
            get
            {
                return Struct.Address.lpSockaddr.AddressFamily;
            }
        }

        public byte[] Data
        {
            get
            {
                return Struct.Address.lpSockaddr.Data;
            }
        }

        public static implicit operator LPADAPTER_UNICAST_ADDRESS(IntPtr operand)
        {
            var a = new LPADAPTER_UNICAST_ADDRESS();
            a.Handle = operand;
            return a;
        }

        public static implicit operator IntPtr(LPADAPTER_UNICAST_ADDRESS operand)
        {
            return operand.Handle;
        }

        public static implicit operator LPADAPTER_UNICAST_ADDRESS(MemPtr operand)
        {
            var a = new LPADAPTER_UNICAST_ADDRESS();
            a.Handle = operand;
            return a;
        }

        public static implicit operator MemPtr(LPADAPTER_UNICAST_ADDRESS operand)
        {
            return operand.Handle;
        }

        public static implicit operator LPADAPTER_UNICAST_ADDRESS(ADAPTER_UNICAST_ADDRESS operand)
        {
            var a = new LPADAPTER_UNICAST_ADDRESS();
            a.Handle.Alloc(Marshal.SizeOf(operand));
            Marshal.StructureToPtr(operand, a.Handle.Handle, true);
            return a;
        }

        public static implicit operator ADAPTER_UNICAST_ADDRESS(LPADAPTER_UNICAST_ADDRESS operand)
        {
            return operand.ToAddress();
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public struct ADAPTER_MULTICAST_ADDRESS
    {
        public IP_ADAPTER_HEADER_UNION Header;
        public LPADAPTER_MULTICAST_ADDRESS Next;
        public SOCKET_ADDRESS Address;

        public override string ToString()
        {
            string ToStringRet = default;
            ToStringRet = Address.ToString();
            return ToStringRet;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public struct LPADAPTER_MULTICAST_ADDRESS
    {
        public MemPtr Handle;

        public LPADAPTER_MULTICAST_ADDRESS[] AddressChain
        {
            get
            {
                if (Handle == IntPtr.Zero)
                    return null;
                int c = 0;
                var mx = this;
                var ac = default(LPADAPTER_MULTICAST_ADDRESS[]);
                do
                {
                    Array.Resize(ref ac, c + 1);
                    ac[c] = mx;
                    mx = mx.Next;
                    c += 1;
                }
                while (mx.Handle.Handle != IntPtr.Zero);
                return ac;
            }
        }

        public LPADAPTER_MULTICAST_ADDRESS Next
        {
            get
            {
                if (Handle == IntPtr.Zero)
                    return default;
                return Struct.Next;
            }
        }

        public override string ToString()
        {
            if (Handle.Handle == IntPtr.Zero)
                return "NULL";
            return "" + IPAddress.ToString() + " (" + AddressFamily.ToString() + ")";
        }

        public IPAddress IPAddress
        {
            get
            {
                if (Handle == IntPtr.Zero)
                    return null;
                return Struct.Address.lpSockaddr.IPAddress;
            }
        }

        public ADAPTER_MULTICAST_ADDRESS Struct
        {
            get
            {
                ADAPTER_MULTICAST_ADDRESS StructRet = default;
                if (Handle == IntPtr.Zero)
                    return default;
                StructRet = ToAddress();
                return StructRet;
            }
        }

        public ADAPTER_MULTICAST_ADDRESS ToAddress()
        {
            ADAPTER_MULTICAST_ADDRESS ToAddressRet = default;
            if (Handle == IntPtr.Zero)
                return default;
            ToAddressRet = Handle.ToStruct<ADAPTER_MULTICAST_ADDRESS>();
            return ToAddressRet;
        }

        public void Dispose()
        {
            Handle.Free();
        }

        public AddressFamily AddressFamily
        {
            get
            {
                if (Handle == IntPtr.Zero)
                    return AddressFamily.AfUnspecified;
                return Struct.Address.lpSockaddr.AddressFamily;
            }
        }

        public byte[] Data
        {
            get
            {
                if (Handle == IntPtr.Zero)
                    return null;
                return Struct.Address.lpSockaddr.Data;
            }
        }

        public static implicit operator LPADAPTER_MULTICAST_ADDRESS(IntPtr operand)
        {
            var a = new LPADAPTER_MULTICAST_ADDRESS();
            a.Handle = operand;
            return a;
        }

        public static implicit operator IntPtr(LPADAPTER_MULTICAST_ADDRESS operand)
        {
            return operand.Handle;
        }

        public static implicit operator LPADAPTER_MULTICAST_ADDRESS(MemPtr operand)
        {
            var a = new LPADAPTER_MULTICAST_ADDRESS();
            a.Handle = operand;
            return a;
        }

        public static implicit operator MemPtr(LPADAPTER_MULTICAST_ADDRESS operand)
        {
            return operand.Handle;
        }

        public static implicit operator LPADAPTER_MULTICAST_ADDRESS(ADAPTER_MULTICAST_ADDRESS operand)
        {
            var a = new LPADAPTER_MULTICAST_ADDRESS();
            a.Handle.Alloc(Marshal.SizeOf(operand));
            Marshal.StructureToPtr(operand, a.Handle.Handle, true);
            return a;
        }

        public static implicit operator ADAPTER_MULTICAST_ADDRESS(LPADAPTER_MULTICAST_ADDRESS operand)
        {
            return operand.ToAddress();
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public struct LUID
    {
        public ulong Value;

        public int Reserved
        {
            get
            {
                return (int)((long)Value & 0xFFFFFFL);
            }
        }

        public int NetLuidIndex
        {
            get
            {
                return (int)((long)(Value >> 24) & 0xFFFFFFL);
            }
        }

        public IFTYPE IfType
        {
            get
            {
                return (IFTYPE)Conversions.ToInteger((long)(Value >> 48) & 0xFFFFL);
            }
        }

        public override string ToString()
        {
            return IfType.ToString();
        }
    }


    /// <summary>
    /// Represents a network adapter MAC address.
    /// </summary>
    /// <remarks></remarks>
    [StructLayout(LayoutKind.Sequential)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public struct MACADDRESS
    {
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = IfDefApi.MAX_ADAPTER_ADDRESS_LENGTH)]
        public byte[] Data;

        public override string ToString()
        {
            string ToStringRet = default;
            string s = "";
            byte b;
            if (Data is null)
                return "NULL";

            // ' let's get a clean string without extraneous zeros at the end:

            int i;
            int c = Data.Length - 1;
            for (i = c; i >= 0; i -= 1)
            {
                if (Data[i] != 0)
                    break;
            }

            c = i;
            i = 0;
            var loopTo = c;
            for (i = 0; i <= loopTo; i++)
            {
                b = Data[i];
                if (!string.IsNullOrEmpty(s))
                    s += ":";
                s += b.ToString("X2");
            }

            if (string.IsNullOrEmpty(s))
                s = "NULL";
            ToStringRet = s;
            return ToStringRet;
        }
    }

    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    /* TODO ERROR: Skipped RegionDirectiveTrivia */    // ' From Microsoft:

    // '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    // '                                                                          ''
    // ' Media types                                                              ''
    // '                                                                          ''
    // ' These are enumerated values of the ifType object defined in MIB-II's     ''
    // ' ifTable.  They are registered with IANA which publishes this list        ''
    // ' periodically, in either the Assigned Numbers RFC, or some derivative     ''
    // ' of it specific to Internet Network Management number assignments.        ''
    // ' See ftp:''ftp.isi.edu/mib/ianaiftype.mib                                 ''
    // '                                                                          ''
    // '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    public enum IFTYPE
    {

        /// <summary>
        /// Minimum IF_TYPE integer value present in this enumeration.
        /// </summary>
        /// <remarks></remarks>
        MIN_IF_TYPE = 1,

        /// <summary>
        /// None of the below
        /// </summary>
        /// <remarks></remarks>
        OTHER = 1,
        REGULAR_1822 = 2,
        HDH_1822 = 3,
        DDN_X25 = 4,
        RFC877_X25 = 5,

        /// <summary>
        /// Ethernet adapter
        /// </summary>
        /// <remarks></remarks>
        ETHERNET_CSMACD = 6,
        IS088023_CSMACD = 7,
        ISO88024_TOKENBUS = 8,
        ISO88025_TOKENRING = 9,
        ISO88026_MAN = 10,
        STARLAN = 11,
        PROTEON_10MBIT = 12,
        PROTEON_80MBIT = 13,
        HYPERCHANNEL = 14,
        FDDI = 15,
        LAP_B = 16,
        SDLC = 17,

        /// <summary>
        /// DS1-MIB
        /// </summary>
        /// <remarks></remarks>
        DS1 = 18,

        /// <summary>
        /// Obsolete; see DS1-MIB
        /// </summary>
        /// <remarks></remarks>
        E1 = 19,
        BASIC_ISDN = 20,
        PRIMARY_ISDN = 21,
        /// <summary>
        /// proprietary serial
        /// </summary>
        /// <remarks></remarks>
        PROP_POINT2POINT_SERIAL = 22,
        PPP = 23,
        SOFTWARE_LOOPBACK = 24,

        /// <summary>
        /// CLNP over IP
        /// </summary>
        /// <remarks></remarks>
        EON = 25,
        ETHERNET_3MBIT = 26,

        /// <summary>
        /// XNS over IP
        /// </summary>
        /// <remarks></remarks>
        NSIP = 27,

        /// <summary>
        /// Generic Slip
        /// </summary>
        /// <remarks></remarks>
        SLIP = 28,

        /// <summary>
        /// ULTRA Technologies
        /// </summary>
        /// <remarks></remarks>
        ULTRA = 29,

        /// <summary>
        /// DS3-MIB
        /// </summary>
        /// <remarks></remarks>
        DS3 = 30,

        /// <summary>
        /// SMDS, coffee
        /// </summary>
        /// <remarks></remarks>
        SIP = 31,

        /// <summary>
        /// DTE only
        /// </summary>
        /// <remarks></remarks>
        FRAMERELAY = 32,
        RS232 = 33,

        /// <summary>
        /// Parallel port
        /// </summary>
        /// <remarks></remarks>
        PARA = 34,
        ARCNET = 35,
        ARCNET_PLUS = 36,

        /// <summary>
        /// ATM cells
        /// </summary>
        /// <remarks></remarks>
        ATM = 37,
        MIO_X25 = 38,

        /// <summary>
        /// SONET or SDH
        /// </summary>
        /// <remarks></remarks>
        SONET = 39,
        X25_PLE = 40,
        ISO88022_LLC = 41,
        LOCALTALK = 42,
        SMDS_DXI = 43,

        /// <summary>
        /// FRNETSERV-MIB
        /// </summary>
        /// <remarks></remarks>
        FRAMERELAY_SERVICE = 44,
        V35 = 45,
        HSSI = 46,
        HIPPI = 47,

        /// <summary>
        /// Generic Modem
        /// </summary>
        /// <remarks></remarks>
        MODEM = 48,

        /// <summary>
        /// AAL5 over ATM
        /// </summary>
        /// <remarks></remarks>
        AAL5 = 49,
        SONET_PATH = 50,
        SONET_VT = 51,

        /// <summary>
        /// SMDS InterCarrier Interface
        /// </summary>
        /// <remarks></remarks>
        SMDS_ICIP = 52,

        /// <summary>
        /// Proprietary virtual/internal
        /// </summary>
        /// <remarks></remarks>
        PROP_VIRTUAL = 53,

        /// <summary>
        /// Proprietary multiplexing
        /// </summary>
        /// <remarks></remarks>
        PROP_MULTIPLEXOR = 54,

        /// <summary>
        /// 100BaseVG
        /// </summary>
        /// <remarks></remarks>
        IEEE80212 = 55,
        FIBRECHANNEL = 56,
        HIPPIINTERFACE = 57,

        /// <summary>
        /// Obsolete, use 32 or 44
        /// </summary>
        /// <remarks></remarks>
        FRAMERELAY_INTERCONNECT = 58,

        /// <summary>
        /// ATM Emulated LAN for 802.3
        /// </summary>
        /// <remarks></remarks>
        AFLANE_8023 = 59,

        /// <summary>
        /// ATM Emulated LAN for 802.5
        /// </summary>
        /// <remarks></remarks>
        AFLANE_8025 = 60,

        /// <summary>
        /// ATM Emulated circuit
        /// </summary>
        /// <remarks></remarks>
        CCTEMUL = 61,

        /// <summary>
        /// Fast Ethernet (100BaseT)
        /// </summary>
        /// <remarks></remarks>
        FASTETHER = 62,

        /// <summary>
        /// ISDN and X.25
        /// </summary>
        /// <remarks></remarks>
        ISDN = 63,

        /// <summary>
        /// CCITT V.11/X.21
        /// </summary>
        /// <remarks></remarks>
        V11 = 64,

        /// <summary>
        /// CCITT V.36
        /// </summary>
        /// <remarks></remarks>
        V36 = 65,

        /// <summary>
        /// CCITT G703 at 64Kbps
        /// </summary>
        /// <remarks></remarks>
        G703_64K = 66,

        /// <summary>
        /// Obsolete; see DS1-MIB
        /// </summary>
        /// <remarks></remarks>
        G703_2MB = 67,

        /// <summary>
        /// SNA QLLC
        /// </summary>
        /// <remarks></remarks>
        QLLC = 68,

        /// <summary>
        /// Fast Ethernet (100BaseFX)
        /// </summary>
        /// <remarks></remarks>
        FASTETHER_FX = 69,
        CHANNEL = 70,

        /// <summary>
        /// Radio spread spectrum - WiFi
        /// </summary>
        /// <remarks></remarks>
        IEEE80211 = 71,

        /// <summary>
        /// IBM System 360/370 OEMI Channel
        /// </summary>
        /// <remarks></remarks>
        IBM370PARCHAN = 72,

        /// <summary>
        /// IBM Enterprise Systems Connection
        /// </summary>
        /// <remarks></remarks>
        ESCON = 73,

        /// <summary>
        /// Data Link Switching
        /// </summary>
        /// <remarks></remarks>
        DLSW = 74,

        /// <summary>
        /// ISDN S/T interface
        /// </summary>
        /// <remarks></remarks>
        ISDN_S = 75,

        /// <summary>
        /// ISDN U interface
        /// </summary>
        /// <remarks></remarks>
        ISDN_U = 76,

        /// <summary>
        /// Link Access Protocol D
        /// </summary>
        /// <remarks></remarks>
        LAP_D = 77,

        /// <summary>
        /// IP Switching Objects
        /// </summary>
        /// <remarks></remarks>
        IPSWITCH = 78,

        /// <summary>
        /// Remote Source Route Bridging
        /// </summary>
        /// <remarks></remarks>
        RSRB = 79,

        /// <summary>
        /// ATM Logical Port
        /// </summary>
        /// <remarks></remarks>
        ATM_LOGICAL = 80,

        /// <summary>
        /// Digital Signal Level 0
        /// </summary>
        /// <remarks></remarks>
        DS0 = 81,

        /// <summary>
        /// Group of ds0s on the same ds1
        /// </summary>
        /// <remarks></remarks>
        DS0_BUNDLE = 82,

        /// <summary>
        /// Bisynchronous Protocol
        /// </summary>
        /// <remarks></remarks>
        BSC = 83,

        /// <summary>
        /// Asynchronous Protocol
        /// </summary>
        /// <remarks></remarks>
        ASYNC = 84,

        /// <summary>
        /// Combat Net Radio
        /// </summary>
        /// <remarks></remarks>
        CNR = 85,

        /// <summary>
        /// ISO 802.5r DTR
        /// </summary>
        /// <remarks></remarks>
        ISO88025R_DTR = 86,

        /// <summary>
        /// Ext Pos Loc Report Sys
        /// </summary>
        /// <remarks></remarks>
        EPLRS = 87,

        /// <summary>
        /// Appletalk Remote Access Protocol
        /// </summary>
        /// <remarks></remarks>
        ARAP = 88,

        /// <summary>
        /// Proprietary Connectionless Proto
        /// </summary>
        /// <remarks></remarks>
        PROP_CNLS = 89,

        /// <summary>
        /// CCITT-ITU X.29 PAD Protocol
        /// </summary>
        /// <remarks></remarks>
        HOSTPAD = 90,

        /// <summary>
        /// CCITT-ITU X.3 PAD Facility
        /// </summary>
        /// <remarks></remarks>
        TERMPAD = 91,

        /// <summary>
        /// Multiproto Interconnect over FR
        /// </summary>
        /// <remarks></remarks>
        FRAMERELAY_MPI = 92,

        /// <summary>
        /// CCITT-ITU X213
        /// </summary>
        /// <remarks></remarks>
        X213 = 93,

        /// <summary>
        /// Asymmetric Digital Subscrbr Loop
        /// </summary>
        /// <remarks></remarks>
        ADSL = 94,

        /// <summary>
        /// Rate-Adapt Digital Subscrbr Loop
        /// </summary>
        /// <remarks></remarks>
        RADSL = 95,

        /// <summary>
        /// Symmetric Digital Subscriber Loop
        /// </summary>
        /// <remarks></remarks>
        SDSL = 96,

        /// <summary>
        /// Very H-Speed Digital Subscrb Loop
        /// </summary>
        /// <remarks></remarks>
        VDSL = 97,

        /// <summary>
        /// ISO 802.5 CRFP
        /// </summary>
        /// <remarks></remarks>
        ISO88025_CRFPRINT = 98,

        /// <summary>
        /// Myricom Myrinet
        /// </summary>
        /// <remarks></remarks>
        MYRInet = 99,

        /// <summary>
        /// Voice recEive and transMit
        /// </summary>
        /// <remarks></remarks>
        VOICE_EM = 100,

        /// <summary>
        /// Voice Foreign Exchange Office
        /// </summary>
        /// <remarks></remarks>
        VOICE_FXO = 101,

        /// <summary>
        /// Voice Foreign Exchange Station
        /// </summary>
        /// <remarks></remarks>
        VOICE_FXS = 102,

        /// <summary>
        /// Voice encapsulation
        /// </summary>
        /// <remarks></remarks>
        VOICE_ENCAP = 103,

        /// <summary>
        /// Voice over IP encapsulation
        /// </summary>
        /// <remarks></remarks>
        VOICE_OVERIP = 104,

        /// <summary>
        /// ATM DXI
        /// </summary>
        /// <remarks></remarks>
        ATM_DXI = 105,

        /// <summary>
        /// ATM FUNI
        /// </summary>
        /// <remarks></remarks>
        ATM_FUNI = 106,

        /// <summary>
        /// ATM IMA
        /// </summary>
        /// <remarks></remarks>
        ATM_IMA = 107,

        /// <summary>
        /// PPP Multilink Bundle
        /// </summary>
        /// <remarks></remarks>
        PPPMULTILINKBUNDLE = 108,

        /// <summary>
        /// IBM ipOverCdlc
        /// </summary>
        /// <remarks></remarks>
        IPOVER_CDLC = 109,

        /// <summary>
        /// IBM Common Link Access to Workstn
        /// </summary>
        /// <remarks></remarks>
        IPOVER_CLAW = 110,

        /// <summary>
        /// IBM stackToStack
        /// </summary>
        /// <remarks></remarks>
        STACKTOSTACK = 111,

        /// <summary>
        /// IBM VIPA
        /// </summary>
        /// <remarks></remarks>
        VIRTUALIPADDRESS = 112,

        /// <summary>
        /// IBM multi-proto channel support
        /// </summary>
        /// <remarks></remarks>
        MPC = 113,

        /// <summary>
        /// IBM ipOverAtm
        /// </summary>
        /// <remarks></remarks>
        IPOVER_ATM = 114,

        /// <summary>
        /// ISO 802.5j Fiber Token Ring
        /// </summary>
        /// <remarks></remarks>
        ISO88025_FIBER = 115,

        /// <summary>
        /// IBM twinaxial data link control
        /// </summary>
        /// <remarks></remarks>
        TDLC = 116,
        GIGABITETHERNET = 117,
        HDLC = 118,
        LAP_F = 119,
        V37 = 120,

        /// <summary>
        /// Multi-Link Protocol
        /// </summary>
        /// <remarks></remarks>
        X25_MLP = 121,

        /// <summary>
        /// X.25 Hunt Group
        /// </summary>
        /// <remarks></remarks>
        X25_HUNTGROUP = 122,
        TRANSPHDLC = 123,

        /// <summary>
        /// Interleave channel
        /// </summary>
        /// <remarks></remarks>
        INTERLEAVE = 124,

        /// <summary>
        /// Fast channel
        /// </summary>
        /// <remarks></remarks>
        FAST = 125,

        /// <summary>
        /// IP (for APPN HPR in IP networks)
        /// </summary>
        /// <remarks></remarks>
        IP = 126,

        /// <summary>
        /// CATV Mac Layer
        /// </summary>
        /// <remarks></remarks>
        DOCSCABLE_MACLAYER = 127,

        /// <summary>
        /// CATV Downstream interface
        /// </summary>
        /// <remarks></remarks>
        DOCSCABLE_DOWNSTREAM = 128,

        /// <summary>
        /// CATV Upstream interface
        /// </summary>
        /// <remarks></remarks>
        DOCSCABLE_UPSTREAM = 129,

        /// <summary>
        /// Avalon Parallel Processor
        /// </summary>
        /// <remarks></remarks>
        A12MPPSWITCH = 130,

        /// <summary>
        /// Encapsulation interface
        /// </summary>
        /// <remarks></remarks>
        TUNNEL = 131,

        /// <summary>
        /// Coffee pot
        /// </summary>
        /// <remarks></remarks>
        COFFEE = 132,

        /// <summary>
        /// Circuit Emulation Service
        /// </summary>
        /// <remarks></remarks>
        CES = 133,

        /// <summary>
        /// ATM Sub Interface
        /// </summary>
        /// <remarks></remarks>
        ATM_SUBINTERFACE = 134,

        /// <summary>
        /// Layer 2 Virtual LAN using 802.1Q
        /// </summary>
        /// <remarks></remarks>
        L2_VLAN = 135,

        /// <summary>
        /// Layer 3 Virtual LAN using IP
        /// </summary>
        /// <remarks></remarks>
        L3_IPVLAN = 136,

        /// <summary>
        /// Layer 3 Virtual LAN using IPX
        /// </summary>
        /// <remarks></remarks>
        L3_IPXVLAN = 137,

        /// <summary>
        /// IP over Power Lines
        /// </summary>
        /// <remarks></remarks>
        DIGITALPOWERLINE = 138,

        /// <summary>
        /// Multimedia Mail over IP
        /// </summary>
        /// <remarks></remarks>
        MEDIAMAILOVERIP = 139,

        /// <summary>
        /// Dynamic syncronous Transfer Mode
        /// </summary>
        /// <remarks></remarks>
        DTM = 140,

        /// <summary>
        /// Data Communications Network
        /// </summary>
        /// <remarks></remarks>
        DCN = 141,

        /// <summary>
        /// IP Forwarding Interface
        /// </summary>
        /// <remarks></remarks>
        IPFORWARD = 142,

        /// <summary>
        /// Multi-rate Symmetric DSL
        /// </summary>
        /// <remarks></remarks>
        MSDSL = 143,

        /// <summary>
        /// IEEE1394 High Perf Serial Bus
        /// </summary>
        /// <remarks></remarks>
        IEEE1394 = 144,
        IF_GSN = 145,
        DVBRCC_MACLAYER = 146,
        DVBRCC_DOWNSTREAM = 147,
        DVBRCC_UPSTREAM = 148,
        ATM_VIRTUAL = 149,
        MPLS_TUNNEL = 150,
        SRP = 151,
        VOICEOVERATM = 152,
        VOICEOVERFRAMERELAY = 153,
        IDSL = 154,
        COMPOSITELINK = 155,
        SS7_SIGLINK = 156,
        PROP_WIRELESS_P2P = 157,
        FR_FORWARD = 158,
        RFC1483 = 159,
        USB = 160,
        IEEE8023AD_LAG = 161,
        BGP_POLICY_ACCOUNTING = 162,
        FRF16_MFR_BUNDLE = 163,
        H323_GATEKEEPER = 164,
        H323_PROXY = 165,
        MPLS = 166,
        MF_SIGLINK = 167,
        HDSL2 = 168,
        SHDSL = 169,
        DS1_FDL = 170,
        POS = 171,
        DVB_ASI_IN = 172,
        DVB_ASI_OUT = 173,
        PLC = 174,
        NFAS = 175,
        TR008 = 176,
        GR303_RDT = 177,
        GR303_IDT = 178,
        ISUP = 179,
        PROP_DOCS_WIRELESS_MACLAYER = 180,
        PROP_DOCS_WIRELESS_DOWNSTREAM = 181,
        PROP_DOCS_WIRELESS_UPSTREAM = 182,
        HIPERLAN2 = 183,
        PROP_BWA_P2MP = 184,
        SONET_OVERHEAD_CHANNEL = 185,
        DIGITAL_WRAPPER_OVERHEAD_CHANNEL = 186,
        AAL2 = 187,
        RADIO_MAC = 188,
        ATM_RADIO = 189,
        IMT = 190,
        MVL = 191,
        REACH_DSL = 192,
        FR_DLCI_ENDPT = 193,
        ATM_VCI_ENDPT = 194,
        OPTICAL_CHANNEL = 195,
        OPTICAL_TRANSPORT = 196,
        IEEE80216_WMAN = 237,

        /// <summary>
        /// WWAN devices based on GSM technology
        /// </summary>
        /// <remarks></remarks>
        WWANPP = 243,

        /// <summary>
        /// WWAN devices based on CDMA technology
        /// </summary>
        /// <remarks></remarks>
        WWANPP2 = 244,

        /// <summary>
        /// Maximum IF_TYPE integer value present in this enumeration.
        /// </summary>
        /// <remarks></remarks>
        MAX_IF_TYPE = 244
    }

    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    /// <summary>
    /// Interface connection type.
    /// </summary>
    /// <remarks></remarks>
    public enum NET_IF_CONNECTION_TYPE
    {

        /// <summary>
        /// Undefined
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// Dedicated connection.  This is a typical connection.
        /// </summary>
        /// <remarks></remarks>
        Dedicated = 1,

        /// <summary>
        /// Passive
        /// </summary>
        Passive = 2,

        /// <summary>
        /// On demand
        /// </summary>
        Demand = 3,

        /// <summary>
        /// Maximum
        /// </summary>
        Maximum = 4
    }

    /// <summary>
    /// Interface tunnel type.
    /// </summary>
    /// <remarks></remarks>
    public enum TUNNEL_TYPE
    {

        /// <summary>
        /// None
        /// </summary>
        None = 0,

        /// <summary>
        /// Other
        /// </summary>
        Other = 1,

        /// <summary>
        /// Direct
        /// </summary>
        Direct = 2,

        /// <summary>
        /// Ipv6 to Ipv4 tunnel
        /// </summary>
        IPv6ToIPv4 = 11,

        /// <summary>
        /// ISATAP tunnel
        /// </summary>
        ISATAP = 13,

        /// <summary>
        /// Teredo Ipv6 tunnel
        /// </summary>
        Teredo = 14,

        /// <summary>
        /// IPHTTPS tunnel
        /// </summary>
        IPHTTPS = 15
    }

    /// <summary>
    /// Interface operational status.
    /// </summary>
    /// <remarks></remarks>
    public enum IF_OPER_STATUS
    {
        /// <summary>
        /// The network device is up
        /// </summary>
        /// <remarks></remarks>
        IfOperStatusUp = 1,

        /// <summary>
        /// The network device is down
        /// </summary>
        /// <remarks></remarks>
        IfOperStatusDown,

        /// <summary>
        /// The network device is performing a self-test
        /// </summary>
        IfOperStatusTesting,

        /// <summary>
        /// The state of the network device is unknown
        /// </summary>
        IfOperStatusUnknown,

        /// <summary>
        /// The network device is asleep
        /// </summary>
        IfOperStatusDormant,

        /// <summary>
        /// The network device is not present
        /// </summary>
        IfOperStatusNotPresent,

        /// <summary>
        /// Network device lower-layer is down
        /// </summary>
        IfOperStatusLowerLayerDown
    }

    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    [Flags]
    public enum IPAdapterAddressesFlags
    {
        DDNS = 0x1,
        RegisterAdapterSuffix = 0x2,
        DHCP = 0x4,
        ReceiveOnly = 0x8,
        NoMulticast = 0x10,
        IPv6OtherStatfulConfig = 0x20,
        NetBiosOverTCPIP = 0x40,
        IPv4Enabled = 0x80,
        IPv6Enabled = 0x100,
        IPv6ManageAddressConfig = 0x200
    }

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

    /// <summary>
    /// Creatively marshaled pointerized structure for the IP_ADAPTER_ADDRESSES structure.
    /// </summary>
    /// <remarks></remarks>
    [StructLayout(LayoutKind.Sequential)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public struct LPIP_ADAPTER_ADDRESSES
    {
        public MemPtr Handle;

        public override string ToString()
        {
            if (Handle.Handle == IntPtr.Zero)
                return "NULL";
            return Struct.FriendlyName;
        }

        public LPIP_ADAPTER_ADDRESSES Next
        {
            get
            {
                return Struct.Next;
            }
        }

        public IP_ADAPTER_ADDRESSES Struct
        {
            get
            {
                IP_ADAPTER_ADDRESSES StructRet = default;
                StructRet = ToAdapterStruct();
                return StructRet;
            }
        }

        public IP_ADAPTER_ADDRESSES ToAdapterStruct()
        {
            IP_ADAPTER_ADDRESSES ToAdapterStructRet = default;
            if (Handle == IntPtr.Zero)
                return default;
            ToAdapterStructRet = Handle.ToStruct<IP_ADAPTER_ADDRESSES>();
            return ToAdapterStructRet;
        }

        public void Dispose()
        {
            Handle.Free();
        }

        public static implicit operator LPIP_ADAPTER_ADDRESSES(IP_ADAPTER_ADDRESSES operand)
        {
            var a = new LPIP_ADAPTER_ADDRESSES();
            int cb = Marshal.SizeOf(a);
            a.Handle.Alloc(cb);
            Marshal.StructureToPtr(operand, a.Handle, true);
            return a;
        }

        public static implicit operator IP_ADAPTER_ADDRESSES(LPIP_ADAPTER_ADDRESSES operand)
        {
            var a = operand.Handle.ToStruct<IP_ADAPTER_ADDRESSES>();
            return a;
        }

        public static implicit operator LPIP_ADAPTER_ADDRESSES(IntPtr operand)
        {
            var a = new LPIP_ADAPTER_ADDRESSES();
            a.Handle = operand;
            return a;
        }

        public static implicit operator IntPtr(LPIP_ADAPTER_ADDRESSES operand)
        {
            return operand.Handle.Handle;
        }

        public static implicit operator LPIP_ADAPTER_ADDRESSES(MemPtr operand)
        {
            var a = new LPIP_ADAPTER_ADDRESSES();
            a.Handle = operand;
            return a;
        }

        public static implicit operator MemPtr(LPIP_ADAPTER_ADDRESSES operand)
        {
            return operand.Handle;
        }
    }

    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    /// <summary>
    /// IP Adapter enumeration function flags.
    /// </summary>
    /// <remarks></remarks>
    [Flags]
    public enum GAA_FLAGS
    {

        /// <summary>
        /// Do not return unicast addresses.
        /// </summary>
        /// <remarks></remarks>
        GAA_FLAG_SKIP_UNICAST = 0x1,

        /// <summary>
        /// Do not return IPv6 anycast addresses.
        /// </summary>
        /// <remarks></remarks>
        GAA_FLAG_SKIP_ANYCAST = 0x2,

        /// <summary>
        /// Do not return multicast addresses.
        /// </summary>
        /// <remarks></remarks>
        GAA_FLAG_SKIP_MULTICAST = 0x4,

        /// <summary>
        /// Do not return addresses of DNS servers.
        /// </summary>
        /// <remarks></remarks>
        GAA_FLAG_SKIP_DNS_SERVER = 0x8,

        /// <summary>
        /// Return a list of IP address prefixes on this adapter. When this flag is set, IP address prefixes are returned for both IPv6 and IPv4 addresses.
        /// </summary>
        /// <remarks></remarks>
        GAA_FLAG_INCLUDE_PREFIX = 0x10,

        /// <summary>
        /// Do not return the adapter friendly name.
        /// This flag is supported on Windows XP with SP1 and later.
        /// </summary>
        /// <remarks></remarks>
        GAA_FLAG_SKIP_FRIENDLY_NAME = 0x20,

        /// <summary>
        /// Return addresses of Windows Internet Name Service (WINS) servers.
        /// </summary>
        /// <remarks></remarks>
        GAA_FLAG_INCLUDE_WINS_INFO = 0x40,

        /// <summary>
        /// Return the addresses of default gateways.
        /// This flag is supported on Windows Vista and later.
        /// </summary>
        /// <remarks></remarks>
        GAA_FLAG_INCLUDE_GATEWAYS = 0x80,

        /// <summary>
        /// Return addresses for all NDIS interfaces.
        /// This flag is supported on Windows Vista and later.
        /// </summary>
        /// <remarks></remarks>
        GAA_FLAG_INCLUDE_ALL_INTERFACES = 0x100,

        /// <summary>
        /// Return addresses in all routing compartments.
        /// </summary>
        /// <remarks></remarks>
        GAA_FLAG_INCLUDE_ALL_COMPARTMENTS = 0x200,

        /// <summary>
        /// Return the adapter addresses sorted in tunnel binding order. This flag is supported on Windows Vista and later.
        /// </summary>
        /// <remarks></remarks>
        GAA_FLAG_INCLUDE_TUNNEL_BINDINGORDER = 0x400
    }

    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    /* TODO ERROR: Skipped RegionDirectiveTrivia */

    /// <summary>
    /// Adapter enumeration result
    /// </summary>
    public enum ADAPTER_ENUM_RESULT
    {
        /// <summary>
        /// Success
        /// </summary>
        /// <remarks></remarks>
        NO_ERROR = 0,

        /// <summary>
        /// An address has not yet been associated with the network endpoint.DHCP lease information was available.
        /// </summary>
        /// <remarks></remarks>
        ERROR_ADDRESS_NOT_ASSOCIATED = 1228,

        /// <summary>
        /// The buffer size indicated by the SizePointer parameter is too small to hold the adapter information or the AdapterAddresses parameter is NULL.The SizePointer parameter returned points to the required size of the buffer to hold the adapter information.
        /// </summary>
        /// <remarks></remarks>
        ERROR_BUFFER_OVERFLOW = 111,

        /// <summary>
        /// One of the parameters is invalid.This error is returned for any of the following conditions : the SizePointer parameter is NULL, the Address parameter is not AfInet, AfInet6, or AfUnspecified, or the address information for the parameters requested is greater than ULONG_MAX.
        /// </summary>
        /// <remarks></remarks>
        ERROR_INVALID_PARAMETER = 87,

        /// <summary>
        /// Insufficient memory resources are available to complete the operation.
        /// </summary>
        /// <remarks></remarks>
        ERROR_NOT_ENOUGH_MEMORY = 8,

        /// <summary>
        /// No addresses were found for the requested parameters.
        /// </summary>
        /// <remarks></remarks>
        ERROR_NO_DATA = 232
    }

    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    public static class IfDefApi
    {

        // ' A lot of creative marshaling is used here.

        public const int MAX_ADAPTER_ADDRESS_LENGTH = 8;
        public const int MAX_DHCPV6_DUID_LENGTH = 130;

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        [DllImport("Iphlpapi.dll")]
        static extern ADAPTER_ENUM_RESULT GetAdaptersAddresses(AfENUM Family, GAA_FLAGS Flags, IntPtr Reserved, LPIP_ADAPTER_ADDRESSES Addresses, ref uint SizePointer);

        /// <summary>
        /// Retrieves a linked, unmanaged structure array of IP_ADAPTER_ADDRESSES, enumerating all network interfaces on the system.
        /// This function is internal to the managed API in this assembly and is not intended to be used independently from it.
        /// The results of this function are abstracted into the managed <see cref="AdaptersCollection" /> class. Use that, instead.
        /// </summary>
        /// <param name="origPtr">Receives the memory pointer for the memory allocated to retrieve the information from the system.</param>
        /// <param name="noRelease">Specifies that the memory will not be released after usage (this is a typical scenario).</param>
        /// <returns>A linked, unmanaged structure array of IP_ADAPTER_ADDRESSES.</returns>
        /// <remarks></remarks>
        internal static IP_ADAPTER_ADDRESSES[] GetAdapters(ref MemPtr origPtr, bool noRelease = true)
        {
            var lpadapt = new LPIP_ADAPTER_ADDRESSES();
            IP_ADAPTER_ADDRESSES adapt;

            // ' and this is barely enough on a typical system.
            lpadapt.Handle.Alloc(65536L, noRelease);
            lpadapt.Handle.ZeroMemory();
            int ft = 0;
            uint cblen = 65536U;
            int cb = Marshal.SizeOf(lpadapt);
            var res = GetAdaptersAddresses(AfENUM.AfUnspecified, GAA_FLAGS.GAA_FLAG_INCLUDE_GATEWAYS | GAA_FLAGS.GAA_FLAG_INCLUDE_WINS_INFO | GAA_FLAGS.GAA_FLAG_INCLUDE_ALL_COMPARTMENTS | GAA_FLAGS.GAA_FLAG_INCLUDE_ALL_INTERFACES, IntPtr.Zero, lpadapt, ref cblen);


            // Dim res As ADAPTER_ENUM_RESULT = GetAdaptersAddresses(AfENUM.AfUnspecified,
            // 0, IntPtr.Zero,
            // lpadapt, cblen)


            // ' we have a buffer overflow?  We need to get more memory.
            if (res == ADAPTER_ENUM_RESULT.ERROR_BUFFER_OVERFLOW)
            {
                while (res == ADAPTER_ENUM_RESULT.ERROR_BUFFER_OVERFLOW)
                {
                    lpadapt.Handle.ReAlloc(cblen, noRelease);
                    res = GetAdaptersAddresses(AfENUM.AfUnspecified, GAA_FLAGS.GAA_FLAG_INCLUDE_GATEWAYS | GAA_FLAGS.GAA_FLAG_INCLUDE_WINS_INFO, IntPtr.Zero, lpadapt, ref cblen);

                    // ' to make sure that we don't loop forever, in some weird scenario.
                    ft += 1;
                    if (ft > 300)
                        break;
                }
            }
            else if (res != ADAPTER_ENUM_RESULT.NO_ERROR)
            {
                lpadapt.Dispose();
                if (Information.IsNumeric(res.ToString()))
                {
                    throw new NativeException();
                }
                else
                {
                    Interaction.MsgBox("ADAPTER ENUMERATION ERROR: " + res.ToString());
                }
            }

            // ' trim any excess memory.
            if (cblen < 65536L)
            {
                lpadapt.Handle.ReAlloc(cblen, noRelease);
            }

            origPtr = lpadapt;
            IP_ADAPTER_ADDRESSES[] adapters = null;
            int c = 0;
            int cc = 0;
            adapt = lpadapt;
            do
            {
                if (string.IsNullOrEmpty(adapt.Description) | adapt.FirstDnsServerAddress.Handle == IntPtr.Zero)
                {
                    c += 1;
                    adapt = adapt.Next;
                    if (adapt.Next.Handle.Handle == IntPtr.Zero)
                        break;
                    continue;
                }

                Array.Resize(ref adapters, cc + 1);
                adapters[cc] = adapt;
                adapt = adapt.Next;
                cc += 1;
                c += 1;
            }
            while (adapt.Next.Handle.Handle != IntPtr.Zero);

            // ' there is currently no reason for this function to free this pointer,
            // ' but we reserve the right to do so, in the future.
            if (!noRelease)
                origPtr.Free();
            return adapters;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    }
}