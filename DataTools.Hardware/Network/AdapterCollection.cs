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
// Licensed Under the MIT License   
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
using DataTools.Text;
using DataTools.Desktop;
using DataTools.Win32;
using DataTools.Shell.Native;
using DataTools.Win32.Network;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Reflection;
using DataTools.SortedLists;
using DataTools.Win32.Memory;

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

        internal NetworkAdapters()
        {
            Refresh();
        }

        /// <summary>
        /// Refresh the list by calling <see cref="DeviceEnum.EnumerateNetworkDevices()"/>
        /// </summary>
        public void Refresh()
        {
            Free();
            
            var di = DeviceEnum.EnumerateNetworkDevices();
            
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
        
    }

    
    

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
    public class AdaptersCollection : IDisposable, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<NetworkAdapter> adapters = new ObservableCollection<NetworkAdapter>();
        private MemPtr _origPtr;

        public ObservableCollection<NetworkAdapter> Adapters
        {
            get
            {
                return adapters;
            }
            set
            {
                if (adapters == value) return;
                adapters = value;
                OnPropertyChanged();
            }
        }

        public AdaptersCollection() : base()
        {
            Refresh();
        }

        public void Refresh()
        {
            var nad = new Dictionary<int, NetworkAdapter>();
            var lOut = new List<NetworkAdapter>();
            
            var newmm = new MemPtr();

            // Get the array of unmanaged IP_ADAPTER_ADDRESSES structures 
            var newads = IfDefApi.GetAdapters(ref newmm, true);

            var di = DeviceEnum.EnumerateNetworkDevices();
            var iftab = IfTable.GetIfTable();

            foreach (var adap in newads)
            {
                var newp = new NetworkAdapter(adap);
                foreach (var de in di)
                {
                    if ((de.Description ?? "") == (adap.Description ?? "") || (de.FriendlyName ?? "") == (adap.FriendlyName ?? "") || (de.FriendlyName ?? "") == (adap.Description ?? "") || (de.Description ?? "") == (adap.FriendlyName ?? ""))
                    {
                        newp.DeviceInfo = de;

                        foreach (var iface in iftab)
                        {
                            if (newp.PhysicalAddress == iface.bPhysAddr)
                            {
                                newp.PhysIfaceInternal.Add(iface);
                            }
                        }

                        nad.Add(newp.IfIndex, newp);
                        _ = Task.Run(() => PopulateInternetStatus(newp));

                        break;
                    }
                }
            }

            if (adapters == null)
            {
                adapters = new ObservableCollection<NetworkAdapter>();
            }

            if (adapters.Count == 0)
            {
                foreach (var kv in nad)
                {
                    adapters.Add(kv.Value);
                }

            }
            else
            {
                var kseen = new List<int>();

                int c = adapters.Count - 1;
                int i;

                for (i = c; i >= 0; i--)
                {
                    if (nad.ContainsKey(adapters[i].IfIndex))
                    {
                        adapters[i].AssignNewNativeObject(nad[adapters[i].IfIndex]);
                        kseen.Add(adapters[i].IfIndex);
                    }
                    else
                    {
                        adapters.RemoveAt(i);
                    }
                }

                foreach(var kv in nad)
                {
                    if (!kseen.Contains(kv.Value.IfIndex))
                    {
                        adapters.Add(kv.Value);
                    }
                }

            }

            if (_origPtr != MemPtr.Empty)
            {
                _origPtr.Free(true);
            }
           
            _origPtr = newmm;

            QuickSort.Sort(adapters, new Comparison<NetworkAdapter>((a, b) => a.IfIndex - b.IfIndex));
        }

        private void PopulateInternetStatus(NetworkAdapter adapter)
        {
            var addrs = adapter.FirstUnicastAddress.AddressChain;

            foreach (var addr in addrs)
            {
                if (addr.Address.lpSockaddr.IPAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    Socket socket = new Socket(SocketType.Raw, ProtocolType.Icmp);

                    try
                    {
                        socket.Bind(new IPEndPoint(addr.IPAddress, 0));
                    }
                    catch
                    {
                        continue;
                    }

                    socket.ReceiveTimeout = 2000;
                    socket.SendTimeout = 2000;

                    try
                    {
                        IAsyncResult result = socket.BeginConnect(new IPEndPoint(IPAddress.Parse("8.8.8.8"), 0), null, null);

                        bool success = result.AsyncWaitHandle.WaitOne(2000, true);

                        if (socket.Connected)
                        {
                            socket.EndConnect(result);
                            adapter.HasInternet = InternetStatus.HasInternet;
                        }
                        else
                        {
                            socket.Close();
                            adapter.HasInternet = InternetStatus.NoInternet;
                        }

                    }
                    catch
                    {
                        adapter.HasInternet = InternetStatus.NoInternet;
                    }
                    return;
                }
            }

            adapter.HasInternet = InternetStatus.NoInternet;
        }


        
        private bool disposedValue; // To detect redundant calls

        // IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
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

        
    }


    public enum InternetStatus
    {
        NotDetermined,
        HasInternet,
        NoInternet
    }

    
    
    /// <summary>
    /// Managed wrapper class for the native network adapter information API.
    /// </summary>
    /// <remarks></remarks>
    public sealed class NetworkAdapter : IDisposable, INotifyPropertyChanged
    {
        private IP_ADAPTER_ADDRESSES Source;

        private DeviceInfo _deviceInfo;
        private bool _canShowNet;
        private System.Windows.Media.Imaging.BitmapSource _Icon;

        private List<MIB_IFROW> physifaces = new List<MIB_IFROW>();
        private static readonly PropertyInfo[] allProps = typeof(NetworkAdapter).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        // This class should not be created outside of the context of AdaptersCollection.
        internal NetworkAdapter(IP_ADAPTER_ADDRESSES nativeObject)
        {
            AssignNewNativeObject(nativeObject);
        }

        internal void AssignNewNativeObject(NetworkAdapter newSource)
        {
            bool dnes = Source.OperStatus == newSource.OperStatus;
            AssignNewNativeObject(newSource.Source, dnes);
        }

        private void AssignNewNativeObject(IP_ADAPTER_ADDRESSES nativeObject, bool noCreateIcon = false)
        {

            // Store the native object.
            Source = nativeObject;

            if (!noCreateIcon)
            {
                // First thing's first... let's get the icon for the object from its parsing name.
                // Which is magically the parsing name of the network device list and the adapter's GUID name.
                string s = @"::{7007ACC7-3202-11D1-AAD2-00805FC1270E}\" + AdapterName;
                var mm = new MemPtr();

                NativeShell.SHParseDisplayName(s, IntPtr.Zero, out mm.handle, 0, out _);

                if (mm.Handle != IntPtr.Zero)
                {
                    // Get a WPFImage 

                    // string library = @"%systemroot%\system32\shell32.dll"


                    if (OperStatus == IF_OPER_STATUS.IfOperStatusUp)
                    {
                        //if (HasInternet == InternetStatus.HasInternet)
                        //{
                        //    var icn = Resources.LoadLibraryIcon(Environment.ExpandEnvironmentVariables(@"%systemroot%\system32\netcenter.dll") + ",2", StandardIcons.Icon16);
                        //    var icn2 = Resources.GetItemIcon(mm, Resources.SystemIconSizes.ExtraLarge);

                        //    var bmp = new System.Drawing.Bitmap(icn2.Width, icn2.Height,  System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                        //    var g = System.Drawing.Graphics.FromImage(bmp);

                        //    g.DrawIcon(icn2, 0, 0);
                        //    g.DrawIcon(icn, 0, icn2.Height - 16);

                        //    g.Dispose();

                        //    _Icon = Resources.MakeWPFImage(bmp);
                        //    bmp.Dispose();
                        //    icn.Dispose();
                        //    icn2.Dispose();
                        //}
                        //else
                        //{


                            _Icon = Resources.MakeWPFImage(Resources.GetItemIcon(mm, Resources.SystemIconSizes.ExtraLarge));

                        //}

                    }
                    else
                    {
                        _Icon = Resources.MakeWPFImage((System.Drawing.Bitmap)Resources.GrayIcon(Resources.GetItemIcon(mm, Resources.SystemIconSizes.ExtraLarge)));

                    }
                    mm.Free();

                    _canShowNet = true;
                }
                else
                {
                    _canShowNet = false;
                }
            }

            OnPropertyChanged(nameof(ReceiveLinkSpeed));
            OnPropertyChanged(nameof(TransmitLinkSpeed));

            foreach (var pr in allProps)
            {
                if (pr.Name.Contains("Address"))
                    OnPropertyChanged(pr.Name);
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
            var shex = new SHELLEXECUTEINFO();
            shex.cbSize = Marshal.SizeOf(shex);
            shex.nShow = User32.SW_SHOW;
            shex.hInstApp = Process.GetCurrentProcess().Handle;
            shex.hWnd = hwnd;
            shex.lpVerb = "properties";

            // Set the parsing name exactly this way.
            shex.lpDirectory = "::{7007ACC7-3202-11D1-AAD2-00805FC1270E}";
            shex.lpFile = @"::{7007ACC7-3202-11D1-AAD2-00805FC1270E}\" + AdapterName;
            shex.fMask = User32.SEE_MASK_ASYNCOK | User32.SEE_MASK_FLAG_DDEWAIT | User32.SEE_MASK_UNICODE;
            User32.ShellExecuteEx(ref shex);
        }

        /// <summary>
        /// Raise the connection status dialog.
        /// </summary>
        /// <param name="hwnd"></param>
        /// <remarks></remarks>
        public void ShowNetworkStatusDialog(IntPtr hwnd = default)
        {
            var shex = new SHELLEXECUTEINFO();

            shex.cbSize = Marshal.SizeOf(shex);
            shex.hWnd = hwnd;
            shex.nShow = User32.SW_SHOW;
            shex.lpVerb = "";
            shex.hInstApp = Process.GetCurrentProcess().Handle;
            shex.lpDirectory = "::{7007ACC7-3202-11D1-AAD2-00805FC1270E}";
            shex.lpFile = @"::{7007ACC7-3202-11D1-AAD2-00805FC1270E}\" + AdapterName;
            shex.fMask = User32.SEE_MASK_ASYNCOK | User32.SEE_MASK_FLAG_DDEWAIT | User32.SEE_MASK_UNICODE;

            User32.ShellExecuteEx(ref shex);
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
        /// The interface adapter index.  This can be used in PowerShell calls.
        /// </summary>
        [Browsable(true)]
        public int IfIndex
        {
            get
            {
                return (int)Source.Header.IfIndex;
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
                return Source.AdapterName;
            }
        }

        [Browsable(true)]
        public IPAddress IPV4Address
        {
            get
            {
                var addrs = Source.FirstUnicastAddress.AddressChain;
                if (addrs == null || addrs.Length == 0) return null;

                foreach (var addr in addrs)
                {
                    var ip = addr.IPAddress;
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        return ip;
                    }
                }

                return null;
            }
        }

        [Browsable(true)]
        public IPAddress IPV6Address
        {
            get
            {
                var addrs = Source.FirstUnicastAddress.AddressChain;
                if (addrs == null || addrs.Length == 0) return null;

                foreach (var addr in addrs)
                {
                    var ip = addr.IPAddress;
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                    {
                        return ip;
                    }
                }

                return null;
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
                return Source.FirstUnicastAddress;
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
                return Source.FirstAnycastAddress;
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
                return Source.FirstMulticastAddress;
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
                return Source.FirstDnsServerAddress;
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
                return Source.DnsSuffix;
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
                return Source.Description;
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
                return Source.FriendlyName;
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
                return Source.PhysicalAddress;
            }
        }

        [Browsable(true)]
        public uint PhysicalAddressLength
        {
            get
            {
                return Source.PhysicalAddressLength;
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
                return Source.Flags;
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
                return Source.Mtu;
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
                return Source.IfType;
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
                return Source.OperStatus;
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
                return Source.Ipv6IfIndex;
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
                return Source.ZoneIndices;
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
                return Source.FirstPrefix;
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
                return Source.TransmitLinkSpeed;
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
                return Source.ReceiveLinkSpeed;
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
                return Source.FirstWinsServerAddress;
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
                return Source.FirstGatewayAddress;
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
                return Source.Ipv4Metric;
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
                return Source.Ipv6Metric;
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
                return Source.Luid;
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
                return Source.Dhcp4Server;
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
                return Source.CompartmentId;
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
                return Source.NetworkGuid;
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
                return Source.ConnectionType;
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
                return Source.TunnelType;
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
                return Source.Dhcpv6Server;
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
                return Source.Dhcpv6ClientDuid;
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
                return Source.Dhcpv6ClientDuidLength;
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
                return Source.Dhcpv6Iaid;
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
                return Source.FirstDnsSuffix;
            }
        }


        /// <summary>
        /// First DNS Suffix
        /// </summary>
        /// <returns></returns>
        [Browsable(true)]
        [TypeConverter(typeof(ArrayConverter))]
        public MIB_IFROW[] PhysicalInterfaces
        {
            get
            {
                return physifaces.ToArray();
            }
        }


        private InternetStatus hasInet = InternetStatus.NotDetermined;

        /// <summary>
        /// Returns a value indicating whether or not this interface is connected to the internet.
        /// </summary>
        public InternetStatus HasInternet
        {
            get => hasInet;
            set
            {
                if (hasInet == value) return;
                hasInet = value;

                AssignNewNativeObject(Source);
            }
        }

        internal List<MIB_IFROW> PhysIfaceInternal
        {
            get => physifaces;
            set
            {
                physifaces = value;
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
                Source = default;
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