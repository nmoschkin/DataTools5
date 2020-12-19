

// The guts of the device enumeration system
// Some outward-facing functions and a lot of little internal ones.

// I tried to make it as clean as self-explanatory as possible.  

// Copyright (C) 2014 Nathan Moschkin

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Windows.Forms;
using DataTools.SystemInformation;
using DataTools.Hardware.Disk;
using DataTools.Win32;
using DataTools.Hardware.Printers;
using DataTools.Hardware.Processor;
using DataTools.Hardware.Display;
using System.Runtime.InteropServices;

using static DataTools.Hardware.DevEnumApi;

using DataTools.Win32.Memory;

namespace DataTools.Hardware
{

    /// <summary>
    /// Public device enum functions module.
    /// </summary>
    /// <remarks></remarks>
    [SecurityCritical]
    public static class DeviceEnum
    {


        
        internal readonly static bool DevLog = false;
        // Friend xfLog = My.Computer.FileSystem.OpenTextFileWriter("hardware.log", True)

        internal static void WriteLog(string text)
        {

            // Dim d As String = Date.Now().ToString("R")

            // Dim s As String = String.Format("[{0}] : {1}", d, text)
            // xfLog.WriteLine(s)

        }

        

        static DeviceEnum()
        {
            if (DevLog)
            {
            }
        }

        /// <summary>
        /// Returns an exhaustive hardware tree of the entire computer with as much information as can be obtained. Each object descends from DeviceInfo.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static ObservableCollection<object> EnumComputerExhaustive()
        {
            return _internalEnumComputerExhaustive();
        }

        /// <summary>
        /// <para>
        /// Enumerate all network devices installed and recognized by the system.
        /// 
        /// For much more detailed information about network devices,
        /// use the managed <see cref="Network.AdaptersCollection" /> object in this assembly.
        /// This function is used internally by that object.
        /// </para>
        /// </summary>
        /// <returns>An array of DeviceInfo() objects.</returns>
        /// <remarks></remarks>
        public static DeviceInfo[] EnumerateNetworkDevices()
        {
            return EnumerateDevices<DeviceInfo>(DevProp.GUID_DEVINTERFACE_NET);
        }

        /// <summary>
        /// Enumerate all physical hard drives and optical drives, and virtual drives.
        /// </summary>
        /// <returns>An array of DiskDeviceInfo objects.</returns>
        /// <remarks></remarks>
        public static DiskDeviceInfo[] EnumDisks()
        {
            var d = _internalEnumDisks();
            var e = _internalEnumDisks(DevProp.GUID_DEVINTERFACE_CDROM);
            int c = d.Count();
            if (e is null || e.Count() == 0)
                return d;
            try
            {
                foreach (var x in e)
                {
                    try
                    {
                        Array.Resize(ref d, c + 1);
                        d[c] = x;
                        c += 1;
                    }
                    catch
                    {
                        e = null;
                        return d;
                    }
                }

                e = null;
                return d;
            }
            catch
            {
            }

            return null;
        }



        /// <summary>
        /// Enumerate Bluetooth Radios
        /// </summary>
        /// <returns>Array of <see cref="BluetoothDeviceInfo"/> objects.</returns>
        public static BluetoothDeviceInfo[] EnumBluetoothRadios()
        {
            var bth = Bluetooth._internalEnumBluetoothRadios();
            var p = _internalEnumerateDevices<BluetoothDeviceInfo>(Bluetooth.GUID_BTHPORT_DEVICE_INTERFACE, ClassDevFlags.DeviceInterface | ClassDevFlags.Present);
            if (p is object && p.Count() > 0)
            {
                foreach (var x in p)
                {
                    foreach (var y in bth)
                    {
                        int i = x.InstanceId.LastIndexOf(@"\");
                        if (i > -1)
                        {
                            string s = x.InstanceId.Substring(i + 1);
                            ulong res;
                            bool b = ulong.TryParse(s, System.Globalization.NumberStyles.AllowHexSpecifier, System.Globalization.CultureInfo.CurrentCulture, out res);
                            if (b)
                            {
                                if (res == (ulong)(y.address))
                                {
                                    x.IsRadio = true;
                                    x.BluetoothAddress = y.address;
                                    x.BluetoothDeviceClass = y.ulClassofDevice;
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            if (p is null)
                return null;
            Array.Sort(p, new Comparison<DeviceInfo>((x, y) => { if (x.FriendlyName is object && y.FriendlyName is object) { return string.Compare(x.FriendlyName, y.FriendlyName); } else { return string.Compare(x.Description, y.Description); } }));
            return p;
        }


        /// <summary>
        /// Enumerate Bluetooth devices
        /// </summary>
        /// <returns>Array of <see cref="BluetoothDeviceInfo"/> objects.</returns>
        public static BluetoothDeviceInfo[] EnumBluetoothDevices()
        {
            var lOut = new List<BluetoothDeviceInfo>();

            var bth = Bluetooth._internalEnumBluetoothDevices();

            var p = _internalEnumerateDevices<BluetoothDeviceInfo>(DevProp.GUID_DEVCLASS_BLUETOOTH, ClassDevFlags.Present);

            if (p != null && p.Length > 0)
            {
                foreach (var x in p)
                {
                    foreach (var y in bth)
                    {
                        int i = x.InstanceId.LastIndexOf("_");

                        if (i > -1)
                        {
                            string s = x.InstanceId.Substring(i + 1);

                            ulong res;

                            bool b = ulong.TryParse(s, System.Globalization.NumberStyles.AllowHexSpecifier, System.Globalization.CultureInfo.CurrentCulture, out res);

                            if (b)
                            {
                                if (res == (ulong)y.Address)
                                {
                                    x.IsRadio = false;
                                    
                                    x.BluetoothAddress = y.Address;
                                    x.BluetoothDeviceClass = y.ulClassofDevice;
                                    
                                    lOut.Add(x);

                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return lOut.ToArray();

            //Array.Sort(p, new Comparison<DeviceInfo>((x, y) => { if (x.FriendlyName is object && y.FriendlyName is object) { return string.Compare(x.FriendlyName, y.FriendlyName); } else { return string.Compare(x.Description, y.Description); } }));
            //return p;
        }

        /// <summary>
        /// Enumerate COM Ports
        /// </summary>
        /// <returns></returns>
        public static DeviceInfo[] EnumComPorts()
        {
            var p = _internalEnumerateDevices<DeviceInfo>(DevProp.GUID_DEVINTERFACE_COMPORT, ClassDevFlags.DeviceInterface | ClassDevFlags.Present);
            if (p is object && p.Count() > 0)
            {
                foreach (var x in p)
                {
                    if (string.IsNullOrEmpty(x.FriendlyName))
                        continue;
                }
            }

            if (p is null)
                return null;
            Array.Sort(p, new Comparison<DeviceInfo>((x, y) => { if (x.FriendlyName is object && y.FriendlyName is object) { return string.Compare(x.FriendlyName, y.FriendlyName); } else { return string.Compare(x.Description, y.Description); } }));
            return p;
        }



        public static ProcessorDeviceInfo[] EnumProcessors()
        {
            var p = _internalEnumerateDevices<ProcessorDeviceInfo>(DevProp.GUID_DEVCLASS_PROCESSOR, ClassDevFlags.Present);

            if (p != null && p.Length > 0)
            {
                if (p is null) return null;

                var procs = SysInfo.LogicalProcessors;

                Array.Sort(procs, new Comparison<SystemLogicalProcessorInformation>((x, y) =>
                {
                    if (x.Relationship == y.Relationship)
                    {
                        return (int)(x.ProcessorMask - y.ProcessorMask);
                    }
                    else
                    {
                        return ((int)x.Relationship - (int)y.Relationship);
                    }

                }));

                Array.Sort(p, new Comparison<ProcessorDeviceInfo>((x, y) =>
                {
                    return string.Compare(x.InstanceId, y.InstanceId);
                }));

                int c = p.Length;
                int d = procs.Length;

                List<CacheInfo>[] pci = new List<CacheInfo>[c];

                for (int i = 0; i < c; i++)
                {
                    ProcessorDeviceInfo pInfo = p[i];

                    pInfo.LogicalProcessor = (i + 1);

                    int ccore = 1;
                    foreach (var proc in procs)
                    {
                        if ((proc.ProcessorMask & (1 << i)) == (1 << i))
                        {
                            if ((proc.Relationship | LogicalProcessorRelationship.RelationProcessorCore) == LogicalProcessorRelationship.RelationProcessorCore)
                            {
                                    pInfo.Core = ccore;
                                    pInfo.Source = proc;
                            }
                            else if ((proc.Relationship | LogicalProcessorRelationship.RelationCache) == LogicalProcessorRelationship.RelationCache)
                            {
                                var cd = new CacheInfo(proc.CacheDescriptor);
                                
                                if (pci[i] == null)
                                {
                                    pci[i] = new List<CacheInfo>();
                                }

                                switch(cd.Level)
                                {
                                    case 1:
                                        pInfo.HasL1Cache = true;
                                        break;

                                    case 2:
                                        pInfo.HasL2Cache = true;
                                        break;

                                    case 3:
                                        pInfo.HasL3Cache = true;
                                        break;
                                }

                                pInfo.TotalCacheSize += cd.Size;
                                pInfo.TotalLineSize += cd.LineSize;

                                pci[i].Add(cd);
                            }
                        }

                        if ((proc.Relationship | LogicalProcessorRelationship.RelationProcessorCore) == LogicalProcessorRelationship.RelationProcessorCore)
                        {
                            ccore++;
                        }
                    }

                    pInfo.Caches = new ReadOnlyCollection<CacheInfo>(pci[i]);
                }

            }

            return p;

        }


        /// <summary>
        /// Enumerate all display monitors.
        /// </summary>
        /// <returns>An array of PrinterDeviceInfo objects.</returns>
        /// <remarks></remarks>
        public static MonitorDeviceInfo[] EnumMonitors()
        {
            var minf = _internalEnumerateDevices<MonitorDeviceInfo>(DevProp.GUID_DEVINTERFACE_MONITOR, ClassDevFlags.DeviceInterface | ClassDevFlags.Present);
            var mon = new Monitors();
            
            DISPLAY_DEVICE dd;
            
            dd.cb = (uint)Marshal.SizeOf<DISPLAY_DEVICE>();
            var mm = new MemPtr();
            
            mm.Alloc(dd.cb);
            mm.UIntAt(0) = dd.cb;

            if (minf is object && minf.Count() > 0)
            {
                foreach (var x in minf)
                {
                    foreach (var y in mon)
                    {
                        if (MultiMon.EnumDisplayDevices(y.DevicePath, 0, mm, MultiMon.EDD_GET_DEVICE_INTERFACE_NAME))
                        {
                            dd = mm.ToStruct<DISPLAY_DEVICE>();
                            DEVMODE dev = new DEVMODE();

                            dev.dmSize = (ushort)Marshal.SizeOf<DEVMODE>();
                            dev.dmDriverExtra = 0;

                            var mm2 = new MemPtr(65535 + dev.dmSize);

                            

                            var b = MultiMon.EnumDisplaySettingsEx(y.DevicePath, 0xffffffff, ref dev, 0);
                            if (!b)
                            {
                                var s = NativeErrorMethods.FormatLastError();
                            }


                            mm2.Free();

                            if (dd.DeviceID.ToUpper() == x.DevicePath.ToUpper())
                            {
                                x.Source = y;
                                break;
                            }
                        }
                    }
                }
            }

            mm.Free();

            if (minf is null)
                return null;

            Array.Sort(minf, new Comparison<MonitorDeviceInfo>((x, y) => { if (x.FriendlyName is object && y.FriendlyName is object) { return string.Compare(x.FriendlyName, y.FriendlyName); } else { return string.Compare(x.Description, y.Description); } }));
            return minf;
        }


        /// <summary>
        /// Enumerate all printer queues available to the local system.
        /// </summary>
        /// <returns>An array of PrinterDeviceInfo objects.</returns>
        /// <remarks></remarks>
        public static PrinterDeviceInfo[] EnumPrinters()
        {
            var p = _internalEnumerateDevices<PrinterDeviceInfo>(DevProp.GUID_DEVINTERFACE_PRINTER, ClassDevFlags.DeviceInterface | ClassDevFlags.Present);
            if (p is object && p.Count() > 0)
            {
                foreach (var x in p)
                {
                    if (string.IsNullOrEmpty(x.FriendlyName))
                        continue;
                    try
                    {
                        x.PrinterInfo = PrinterObject.GetPrinterInfoObject(x.FriendlyName);
                    }
                    catch
                    {

                        // can't connect to that printer!
                        continue;
                    }
                }
            }

            if (p is null)
                return null;
            Array.Sort(p, new Comparison<PrinterDeviceInfo>((x, y) => { if (x.FriendlyName is object && y.FriendlyName is object) { return string.Compare(x.FriendlyName, y.FriendlyName); } else { return string.Compare(x.Description, y.Description); } }));
            return p;
        }

        /// <summary>
        /// Enumerate all local volumes (with or without mount points).
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DiskDeviceInfo[] EnumVolumes()
        {
            DiskDeviceInfo[] EnumVolumesRet = default;
            EnumVolumesRet = _internalEnumDisks(DevProp.GUID_DEVINTERFACE_VOLUME);
            return EnumVolumesRet;
        }

        /// <summary>
        /// Enumerate devices of DeviceInfo T with the specified hardware class Id.
        /// </summary>
        /// <typeparam name="T">A DeviceInfo-based object.</typeparam>
        /// <param name="ClassId">A system GUID_DEVINTERFACE or GUID_DEVCLASS value.</param>
        /// <param name="flags">Optional flags to pass to the enumerator.</param>
        /// <returns>An array of T</returns>
        /// <remarks></remarks>
        public static T[] EnumerateDevices<T>(Guid ClassId, ClassDevFlags flags = ClassDevFlags.Present | ClassDevFlags.DeviceInterface) where T : DeviceInfo, new()
        {
            T[] EnumerateDevicesRet = default;
            EnumerateDevicesRet = _internalEnumerateDevices<T>(ClassId, flags);
            return EnumerateDevicesRet;
        }

        /// <summary>
        /// Enumerate all devices on the system.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DeviceInfo[] EnumAllDevices()
        {
            return _internalGetComputer();
        }
    }
}