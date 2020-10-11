﻿

// ' The guts of the device enumeration system
// ' Some outward-facing functions and a lot of little internal ones.

// ' I tried to make it as clean as self-explanatory as possible.  

// ' Copyright (C) 2014 Nathan Moschkin

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security;
using DataTools.Interop.Disk;
using DataTools.Interop.Native;
using DataTools.Interop.Printers;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace DataTools.Interop
{

    /// <summary>
/// Public device enum functions module.
/// </summary>
/// <remarks></remarks>
    [HideModuleName]
    [SecurityCritical]
    public static class DevEnumPublic
    {


        /* TODO ERROR: Skipped IfDirectiveTrivia */
        internal readonly static bool DevLog = false;
        // Friend xfLog = My.Computer.FileSystem.OpenTextFileWriter("hardware.log", True)

        internal static void WriteLog(string text)
        {

            // Dim d As String = Date.Now().ToString("R")

            // Dim s As String = String.Format("[{0}] : {1}", d, text)
            // xfLog.WriteLine(s)

        }

        /* TODO ERROR: Skipped ElseDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */

        static DevEnumPublic()
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
            return DevEnumApi._internalEnumComputerExhaustive();
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
            var d = DevEnumApi._internalEnumDisks();
            var e = DevEnumApi._internalEnumDisks(DevProp.GUID_DEVINTERFACE_CDROM);
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
                    catch (Exception ex)
                    {
                        e = null;
                        return d;
                    }
                }

                e = null;
                return d;
            }
            catch (Exception ex)
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
            var bth = BluetoothApi._internalEnumBluetoothRadios();
            var p = DevEnumApi._internalEnumerateDevices<BluetoothDeviceInfo>(BluetoothApi.GUID_BTHPORT_DEVICE_INTERFACE, ClassDevFlags.DeviceInterface | ClassDevFlags.Present);
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
                                if (res == Conversions.ToULong(y.address))
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
            var bth = BluetoothApi._internalEnumBluetoothDevices();
            var p = DevEnumApi._internalEnumerateDevices<BluetoothDeviceInfo>(DevProp.GUID_DEVCLASS_BLUETOOTH, ClassDevFlags.Present);
            if (p is object && p.Count() > 0)
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
                                if (res == Conversions.ToULong(y.Address))
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
            Array.Sort(p, new Comparison<DeviceInfo>((x, y) => { if (x.FriendlyName is object && y.FriendlyName is object) { return string.Compare(x.FriendlyName, y.FriendlyName); } else { return string.Compare(x.Description, y.Description); } }));
            return p;
        }

        /// <summary>
    /// Enumerate COM Ports
    /// </summary>
    /// <returns></returns>
        public static DeviceInfo[] EnumComPorts()
        {
            var p = DevEnumApi._internalEnumerateDevices<DeviceInfo>(DevProp.GUID_DEVINTERFACE_COMPORT, ClassDevFlags.DeviceInterface | ClassDevFlags.Present);
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

        /// <summary>
    /// Enumerate all printer queues available to the local system.
    /// </summary>
    /// <returns>An array of PrinterDeviceInfo objects.</returns>
    /// <remarks></remarks>
        public static PrinterDeviceInfo[] EnumPrinters()
        {
            var p = DevEnumApi._internalEnumerateDevices<PrinterDeviceInfo>(DevProp.GUID_DEVINTERFACE_PRINTER, ClassDevFlags.DeviceInterface | ClassDevFlags.Present);
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
                    catch (Exception ex)
                    {

                        // ' can't connect to that printer!
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
            EnumVolumesRet = DevEnumApi._internalEnumDisks(DevProp.GUID_DEVINTERFACE_VOLUME);
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
            EnumerateDevicesRet = DevEnumApi._internalEnumerateDevices<T>(ClassId, flags);
            return EnumerateDevicesRet;
        }

        /// <summary>
    /// Enumerate all devices on the system.
    /// </summary>
    /// <returns></returns>
    /// <remarks></remarks>
        public static DeviceInfo[] EnumAllDevices()
        {
            return DevEnumApi._internalGetComputer();
        }
    }
}