// ************************************************* ''
// DataTools C# Native Utility Library For Windows - Interop
//
// Module: AdapterCollection
//         Encapsulates the network interface environment
//         of the currently running system.
//
// Copyright (C) 2011-2020 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the Microsoft Public License   
// ************************************************* ''

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using DataTools.Memory;
using DataTools.Text;
using DataTools.Desktop;
using DataTools.Hardware.Native;
using DataTools.Shell.Native;

namespace DataTools.Hardware.Network
{


    //'' <summary>
    //'' System network adapter information thin wrappers.
    //'' </summary>
    //'' <remarks>
    //'' The observable collection is more suitable for use as a WPF data source.
    //'' 
    //'' The NetworkAdapter class cannot be created independently.
    //'' 
    //'' For most usage cases, the AdaptersCollection object should be used.
    //'' 
    //'' The <see cref="NetworkAdapters"/> collection is also a viable option
    //'' and possibly of a lighter variety.
    //'' </remarks>
    // Public Module NetworkWrappers

    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    /// <summary>
    /// Managed wrapper collection for all adapters.
    /// </summary>
    [Category("Devices")]
    [Description("Network adapter collection.")]
    [Browsable(true)]
    public class NetworkAdapters : ICollection<IP_ADAPTER_ADDRESSES>, IDisposable
    {
        private IP_ADAPTER_ADDRESSES[] _Adapters;
        private MemPtr _origPtr;

        private Collection<NetworkAdapter> _Col = new Collection<NetworkAdapter>();

        /// <summary>
        /// Returns an array of <see cref="IP_ADAPTER_ADDRESSES" /> structures
        /// </summary>
        /// <returns></returns>
        [Category("Devices")]
        [Description("Network adapter collection.")]
        [Browsable(true)]
        public IP_ADAPTER_ADDRESSES[] Adapters
        {
            get
            {
                return _Adapters;
            }

            set
            {
                Clear();
                _Adapters = value;
            }
        }

        public NetworkAdapters()
        {
            Refresh();
        }

        /// <summary>
        /// Refresh the list by calling <see cref="DevEnumPublic.EnumerateNetworkDevices()"/>
        /// </summary>
        public void Refresh()
        {
            Free();
            
            var di = DevEnumPublic.EnumerateNetworkDevices();
            
            _Adapters = IfDefApi.GetAdapters(ref _origPtr, true);

            foreach (var adap in _Adapters)
            {
                var newp = new NetworkAdapter(adap);

                foreach (var de in di)
                {
                    if ((de.Description ?? "") == (adap.Description ?? ""))
                    {
                        newp.DeviceInfo = de;
                        break;
                    }
                }

                _Col.Add(newp);
            }
        }

        [Browsable(true)]
        [Category("Collections")]
        public Collection<NetworkAdapter> AdapterCollection
        {
            get
            {
                return _Col;
            }
            internal set
            {
                _Col = value;
            }
        }

        /// <summary>
        /// Returns the <see cref="IP_ADAPTER_ADDRESSES" /> structure at the specified index
        /// </summary>
        /// <param name="index">Index of item to return.</param>
        /// <returns><see cref="IP_ADAPTER_ADDRESSES" /> structure</returns>
        public IP_ADAPTER_ADDRESSES this[int index]
        {
            get
            {
                return _Adapters[index];
            }
        }

        public IEnumerator<IP_ADAPTER_ADDRESSES> GetEnumerator()
        {
            return new IP_ADAPTER_ADDRESSES_Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new IP_ADAPTER_ADDRESSES_Enumerator(this);
        }

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        private bool disposedValue; // To detect redundant calls

        // IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _Adapters = null;
                }

                Free();
            }

            disposedValue = true;
        }

        ~NetworkAdapters()
        {
            Dispose(false);
        }

        protected void Free()
        {
            if (_origPtr.Handle != IntPtr.Zero)
            {
                _origPtr.Free(true);
            }

            _Adapters = null;
            _Col.Clear();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        public void Add(IP_ADAPTER_ADDRESSES item)
        {
            throw new AccessViolationException("Cannot add items to a system managed list");
        }

        public void Clear()
        {
            _Adapters = null;
            Free();
        }

        public bool Contains(IP_ADAPTER_ADDRESSES item)
        {
            if (_Adapters is null)
                return false;
            foreach (var aa in _Adapters)
            {
                if (aa.NetworkGuid == item.NetworkGuid)
                    return true;
            }

            return false;
        }

        public void CopyTo(IP_ADAPTER_ADDRESSES[] array, int arrayIndex)
        {
            if (_Adapters is null)
            {
                throw new NullReferenceException();
            }

            _Adapters.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get
            {
                if (_Adapters is null)
                    return 0;
                else
                    return _Adapters.Count();
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return true;
            }
        }

        public bool Remove(IP_ADAPTER_ADDRESSES item)
        {
            return false;
        }
    }

    public class IP_ADAPTER_ADDRESSES_Enumerator : IEnumerator<IP_ADAPTER_ADDRESSES>
    {
        private int pos = -1;
        private NetworkAdapters subj;

        internal IP_ADAPTER_ADDRESSES_Enumerator(NetworkAdapters subject)
        {
            subj = subject;
        }

        public IP_ADAPTER_ADDRESSES Current
        {
            get
            {
                return subj[pos];
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return subj[pos];
            }
        }

        public bool MoveNext()
        {
            pos += 1;
            if (pos >= subj.Count)
                return false;
            return true;
        }

        public void Reset()
        {
            pos = -1;
        }

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        private bool disposedValue; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    subj = null;
                    pos = -1;
                }
            }

            disposedValue = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    }

    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    /* TODO ERROR: Skipped RegionDirectiveTrivia */

    /// <summary>
    /// A managed observable collection wrapper of NetworkAdapter wrapper objects.  This collection wraps the
    /// Windows Network Interface Api.  All objects are thin wrappers around the original unmanaged
    /// memory objects.
    /// </summary>
    /// <remarks>
    /// The array memory is allocated as one very long block by the GetAdapters function.
    /// We keep it in this collection and the members in the unmanaged memory source serve
    /// as the backbone for the collection of NetworkAdapter objects.
    /// 
    /// For this reason, the NetworkAdapter object cannot be created publically, as the
    /// AdaptersCollection object is managing a single block of unmanaged memory for the entire collection.
    /// Therefore, there can be no singleton instances of the NetworkAdapter object.
    /// 
    /// We will use Finalize() to free this (rather large) resource when this class is destroyed.
    /// </remarks>
    [SecurityCritical()]
    public class AdaptersCollection : IDisposable
    {
        private ObservableCollection<NetworkAdapter> _Col = new ObservableCollection<NetworkAdapter>();
        private IP_ADAPTER_ADDRESSES[] _Adapters;
        private MemPtr _origPtr;

        public ObservableCollection<NetworkAdapter> Collection
        {
            get
            {
                return _Col;
            }
        }

        public AdaptersCollection() : base()
        {
            Refresh();
        }

        public void Refresh()
        {
            Free();
            _Adapters = null;
            _Col = new ObservableCollection<NetworkAdapter>();
            var di = DevEnumPublic.EnumerateNetworkDevices();

            // Get the array of unmanaged IP_ADAPTER_ADDRESSES structures 
            _Adapters = IfDefApi.GetAdapters(ref _origPtr, true);
            foreach (var adap in _Adapters)
            {
                var newp = new NetworkAdapter(adap);
                foreach (var de in di)
                {
                    if ((de.Description ?? "") == (adap.Description ?? "") || (de.FriendlyName ?? "") == (adap.FriendlyName ?? "") || (de.FriendlyName ?? "") == (adap.Description ?? "") || (de.Description ?? "") == (adap.FriendlyName ?? ""))
                    {
                        newp.DeviceInfo = de;
                        _Col.Add(newp);
                        break;
                    }
                }
            }
        }

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        private bool disposedValue; // To detect redundant calls

        // IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _Adapters = null;
                }

                // free up the unmanaged memory and release the memory pressure on the garbage collector.
                _origPtr.Free(true);
            }

            disposedValue = true;
        }

        ~AdaptersCollection()
        {
            Dispose(false);
        }

        protected void Free()
        {
            // free up the unmanaged memory and release the memory pressure on the garbage collector.
            _origPtr.Free(true);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    }

    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    /// <summary>
    /// Managed wrapper class for the native network adapter information API.
    /// </summary>
    /// <remarks></remarks>
    public sealed class NetworkAdapter : IDisposable, INotifyPropertyChanged
    {
        private IP_ADAPTER_ADDRESSES _nativeStruct;
        private DeviceInfo _deviceInfo;
        private bool _canShowNet;
        private System.Windows.Media.Imaging.BitmapSource _Icon;

        // This class should not be created outside of the context of AdaptersCollection.
        internal NetworkAdapter(IP_ADAPTER_ADDRESSES nativeObject)
        {

            // Store the native object.
            _nativeStruct = nativeObject;

            // First thing's first... let's get the icon for the object from its parsing name.
            // Which is magically the parsing name of the network device list and the adapter's GUID name.
            string s = @"::{7007ACC7-3202-11D1-AAD2-00805FC1270E}\" + AdapterName;
            var mm = new MemPtr();
            
            ShellFileGetAttributesOptions argpsfgaoOut = 0;
            
            NativeShell.SHParseDisplayName(s, IntPtr.Zero, out mm.handle, (ShellFileGetAttributesOptions)0, out argpsfgaoOut);

            if (mm.Handle != IntPtr.Zero)
            {
                // Get a WPFImage 
                _Icon = Resources.MakeWPFImage(Resources.GetItemIcon(mm, (Resources.SystemIconSizes)(int)(PInvoke.SHIL_EXTRALARGE)));
                mm.Free();
                _canShowNet = true;
            }
            else
            {
                _canShowNet = false;
            }
        }

        private void OnPropertyChanged([CallerMemberName] string e = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(e));
        }


        /// <summary>
        /// Is true if the device dialog can be displayed for this adapter.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool CanShowDeviceDialog
        {
            get
            {
                return _deviceInfo is object;
            }
        }

        /// <summary>
        /// Returns a BitmapSource of the device's icon.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [Browsable(false)]
        public System.Windows.Media.Imaging.BitmapSource DeviceIcon
        {
            get
            {
                return _Icon;
            }
        }

        /// <summary>
        /// Is true if this device can display the network dialog.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool CanShowNetworkDialogs
        {
            get
            {
                return _canShowNet;
            }
        }

        /// <summary>
        /// Raise the device properties dialog.
        /// </summary>
        /// <remarks></remarks>
        public void ShowDevicePropertiesDialog()
        {
            if (_deviceInfo is null)
                return;
            _deviceInfo.ShowDevicePropertiesDialog();
        }

        /// <summary>
        /// Raise the connection properties dialog.  This may throw the UAC.
        /// </summary>
        /// <param name="hwnd"></param>
        /// <remarks></remarks>
        public void ShowConnectionPropertiesDialog(IntPtr hwnd = default)
        {
            if (_deviceInfo is null)
                return;
            var shex = new PInvoke.SHELLEXECUTEINFO();
            shex.cbSize = Marshal.SizeOf(shex);
            shex.nShow = PInvoke.SW_SHOW;
            shex.hInstApp = Process.GetCurrentProcess().Handle;
            shex.hWnd = hwnd;
            shex.lpVerb = "properties";

            // Set the parsing name exactly this way.
            shex.lpDirectory = "::{7007ACC7-3202-11D1-AAD2-00805FC1270E}";
            shex.lpFile = @"::{7007ACC7-3202-11D1-AAD2-00805FC1270E}\" + AdapterName;
            shex.fMask = PInvoke.SEE_MASK_ASYNCOK | PInvoke.SEE_MASK_FLAG_DDEWAIT | PInvoke.SEE_MASK_UNICODE;
            PInvoke.ShellExecuteEx(ref shex);
        }

        /// <summary>
        /// Raise the connection status dialog.
        /// </summary>
        /// <param name="hwnd"></param>
        /// <remarks></remarks>
        public void ShowNetworkStatusDialog(IntPtr hwnd = default)
        {
            var shex = new PInvoke.SHELLEXECUTEINFO();
            shex.cbSize = Marshal.SizeOf(shex);
            shex.hWnd = hwnd;
            shex.nShow = PInvoke.SW_SHOW;
            shex.lpVerb = "";
            shex.hInstApp = Process.GetCurrentProcess().Handle;
            shex.lpDirectory = "::{7007ACC7-3202-11D1-AAD2-00805FC1270E}";
            shex.lpFile = @"::{7007ACC7-3202-11D1-AAD2-00805FC1270E}\" + AdapterName;
            shex.fMask = PInvoke.SEE_MASK_ASYNCOK | PInvoke.SEE_MASK_FLAG_DDEWAIT | PInvoke.SEE_MASK_UNICODE;
            PInvoke.ShellExecuteEx(ref shex);
        }

        /// <summary>
        /// Retrieves the DeviceInfo object for the system device instance of the network adapter.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [Browsable(true)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public DeviceInfo DeviceInfo
        {
            get
            {
                return _deviceInfo;
            }

            internal set
            {
                _deviceInfo = value;
                if (_Icon is null)
                {
                    // if the adapter doesn't have its own icon, the device class surely will.
                    // let's see if we can get an icon from the device!

                    if (_deviceInfo.DeviceIcon is object)
                    {
                        _Icon = _deviceInfo.DeviceIcon;
                        OnPropertyChanged("DeviceIcon");
                    }
                }
            }
        }

        /// <summary>
        /// The GUID adapter name.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [Browsable(true)]
        public string AdapterName
        {
            get
            {
                return _nativeStruct.AdapterName;
            }
        }

        /// <summary>
        /// The first IP address of this device.  Usually IPv6. The IPv4 address resides at FirstUnicastAddress.Next.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [Browsable(true)]
        public LPADAPTER_UNICAST_ADDRESS FirstUnicastAddress
        {
            get
            {
                return _nativeStruct.FirstUnicastAddress;
            }
        }

        /// <summary>
        /// The first Anycast address.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [Browsable(true)]
        public LPADAPTER_MULTICAST_ADDRESS FirstAnycastAddress
        {
            get
            {
                return _nativeStruct.FirstAnycastAddress;
            }
        }

        /// <summary>
        /// First multicast address.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [Browsable(true)]
        public LPADAPTER_MULTICAST_ADDRESS FirstMulticastAddress
        {
            get
            {
                return _nativeStruct.FirstMulticastAddress;
            }
        }

        /// <summary>
        /// First Dns server address.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [Browsable(true)]
        public LPADAPTER_MULTICAST_ADDRESS FirstDnsServerAddress
        {
            get
            {
                return _nativeStruct.FirstDnsServerAddress;
            }
        }

        /// <summary>
        /// Dns Suffix. This is typically the name of your ISP's internal domain if you are connected to an ISP.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [Browsable(true)]
        public string DnsSuffix
        {
            get
            {
                return _nativeStruct.DnsSuffix;
            }
        }

        /// <summary>
        /// This is always the friendly name of the device instance of the network adapter.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [Browsable(true)]
        public string Description
        {
            get
            {
                return _nativeStruct.Description;
            }
        }

        /// <summary>
        /// Friendly name of the network connection (Ethernet 1, WiFi 2, etc).
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [Browsable(true)]
        public string FriendlyName
        {
            get
            {
                return _nativeStruct.FriendlyName;
            }
        }

        /// <summary>
        /// The MAC address of the adapter.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [Browsable(true)]
        public MACADDRESS PhysicalAddress
        {
            get
            {
                return _nativeStruct.PhysicalAddress;
            }
        }

        [Browsable(true)]
        public uint PhysicalAddressLength
        {
            get
            {
                return _nativeStruct.PhysicalAddressLength;
            }
        }

        /// <summary>
        /// Adapter configuration flags and capabilities.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [Browsable(true)]
        public IPAdapterAddressesFlags Flags
        {
            get
            {
                return _nativeStruct.Flags;
            }
        }

        /// <summary>
        /// Maximum transmission unit, in bytes.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [Browsable(true)]
        public int Mtu
        {
            get
            {
                return _nativeStruct.Mtu;
            }
        }


        /// <summary>
        /// Interface type.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [Browsable(true)]
        public IFTYPE IfType
        {
            get
            {
                return _nativeStruct.IfType;
            }
        }

        /// <summary>
        /// Operational status.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [Browsable(true)]
        public IF_OPER_STATUS OperStatus
        {
            get
            {
                return _nativeStruct.OperStatus;
            }
        }

        /// <summary>
        /// Ipv6 IF Index
        /// </summary>
        /// <returns></returns>
        [Browsable(true)]
        public uint Ipv6IfIndex
        {
            get
            {
                return _nativeStruct.Ipv6IfIndex;
            }
        }


        /// <summary>
        /// Zone Indices
        /// </summary>
        /// <returns></returns>
        [Browsable(true)]
        public uint[] ZoneIndices
        {
            get
            {
                return _nativeStruct.ZoneIndices;
            }
        }


        /// <summary>
        /// Get the first <see cref="LPIP_ADAPTER_PREFIX" />
        /// </summary>
        /// <returns></returns>
        [Browsable(true)]
        public LPIP_ADAPTER_PREFIX FirstPrefix
        {
            get
            {
                return _nativeStruct.FirstPrefix;
            }
        }

        /// <summary>
        /// Current upstream link speed (in bytes).
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [Browsable(true)]
        public FriendlySpeedLong TransmitLinkSpeed
        {
            get
            {
                return _nativeStruct.TransmitLinkSpeed;
            }
        }

        /// <summary>
        /// Current downstream link speed (in bytes).
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [Browsable(true)]
        public FriendlySpeedLong ReceiveLinkSpeed
        {
            get
            {
                return _nativeStruct.ReceiveLinkSpeed;
            }
        }


        /// <summary>
        /// First WINS server address.
        /// </summary>
        /// <returns></returns>
        [Browsable(true)]
        public LPADAPTER_MULTICAST_ADDRESS FirstWinsServerAddress
        {
            get
            {
                return _nativeStruct.FirstWinsServerAddress;
            }
        }

        /// <summary>
        /// First gateway address.
        /// </summary>
        /// <returns></returns>
        [Browsable(true)]
        public LPADAPTER_MULTICAST_ADDRESS FirstGatewayAddress
        {
            get
            {
                return _nativeStruct.FirstGatewayAddress;
            }
        }


        /// <summary>
        /// Ipv4 Metric
        /// </summary>
        /// <returns></returns>
        [Browsable(true)]
        public uint Ipv4Metric
        {
            get
            {
                return _nativeStruct.Ipv4Metric;
            }
        }

        /// <summary>
        /// Ipv6 Metric
        /// </summary>
        /// <returns></returns>
        [Browsable(true)]
        public uint Ipv6Metric
        {
            get
            {
                return _nativeStruct.Ipv6Metric;
            }
        }

        /// <summary>
        /// LUID
        /// </summary>
        /// <returns>A <see cref="LUID"/> structure</returns>
        [Browsable(true)]
        public LUID Luid
        {
            get
            {
                return _nativeStruct.Luid;
            }
        }

        /// <summary>
        /// Ipv4 DHCP server
        /// </summary>
        /// <returns>A <see cref="SOCKET_ADDRESS"/> structure</returns>
        [Browsable(true)]
        public SOCKET_ADDRESS Dhcp4Server
        {
            get
            {
                return _nativeStruct.Dhcp4Server;
            }
        }

        /// <summary>
        /// Compartment ID
        /// </summary>
        /// <returns><see cref="UInt32"/></returns>

        [Browsable(true)]
        public uint CompartmentId
        {
            get
            {
                return _nativeStruct.CompartmentId;
            }
        }

        /// <summary>
        /// Network <see cref="Guid"/>
        /// </summary>
        /// <returns>A <see cref="Guid"/></returns>
        [Browsable(true)]
        public Guid NetworkGuid
        {
            get
            {
                return _nativeStruct.NetworkGuid;
            }
        }

        /// <summary>
        /// Network connection type
        /// </summary>
        /// <returns>A <see cref="NET_IF_CONNECTION_TYPE"/> structure</returns>
        [Browsable(true)]
        public NET_IF_CONNECTION_TYPE ConnectionType
        {
            get
            {
                return _nativeStruct.ConnectionType;
            }
        }

        /// <summary>
        /// Tunnel type
        /// </summary>
        /// <returns>A <see cref="TUNNEL_TYPE"/> value.</returns>
        [Browsable(true)]
        public TUNNEL_TYPE TunnelType
        {
            get
            {
                return _nativeStruct.TunnelType;
            }
        }

        /// <summary>
        /// DHCP v6 server
        /// </summary>
        /// <returns></returns>
        [Browsable(true)]
        public SOCKET_ADDRESS Dhcpv6Server
        {
            get
            {
                return _nativeStruct.Dhcpv6Server;
            }
        }


        /// <summary>
        /// DHCP v6 Client DUID
        /// </summary>
        /// <returns></returns>
        [Browsable(true)]
        public byte[] Dhcpv6ClientDuid
        {
            get
            {
                return _nativeStruct.Dhcpv6ClientDuid;
            }
        }

        /// <summary>
        /// DHCP v6 Client DUID Length
        /// </summary>
        /// <returns></returns>
        [Browsable(true)]
        public uint Dhcpv6ClientDuidLength
        {
            get
            {
                return _nativeStruct.Dhcpv6ClientDuidLength;
            }
        }

        /// <summary>
        /// DHCP v6 AIID
        /// </summary>
        /// <returns></returns>
        [Browsable(true)]
        public uint Dhcpv6Iaid
        {
            get
            {
                return _nativeStruct.Dhcpv6Iaid;
            }
        }

        /// <summary>
        /// First DNS Suffix
        /// </summary>
        /// <returns></returns>
        [Browsable(true)]
        public LPIP_ADAPTER_DNS_SUFFIX FirstDnsSuffix
        {
            get
            {
                return _nativeStruct.FirstDnsSuffix;
            }
        }

        /// <summary>
        /// Returns the adapter's friendly name
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return FriendlyName;
        }

        private bool disposedValue; // To detect redundant calls

        public event PropertyChangedEventHandler PropertyChanged;

        ~NetworkAdapter()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (disposedValue) return;

            if (disposing)
            {
                disposedValue = true;
                _nativeStruct = default;
                _Icon = null;
                _deviceInfo = null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

}