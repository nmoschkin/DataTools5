// ************************************************* ''
// DataTools C# Native Utility Library For Windows - Interop
//
// Module: DeviceEnum
//         Native.
// 
// Copyright (C) 2011-2020 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the MIT License   
// ************************************************* ''

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using DataTools.Desktop;
using DataTools.Shell.Native;
using DataTools.Hardware.Disk;
using DataTools.Hardware.Network;
using DataTools.Hardware.Usb;
using DataTools.Text;
using DataTools.Win32.Disk.Partition;
using DataTools.Win32;
using DataTools.Win32.Disk.VirtualDisk;
using DataTools.Win32.Memory;

namespace DataTools.Hardware
{
    /// <summary>
    /// Internal device enumeration functions module.
    /// </summary>
    /// <remarks></remarks>
    [SecurityCritical]
    internal static class DevEnumApi
    {
        public const int DICLASSPROP_INTERFACE = 2;
        public const int DICLASSPROP_INSTALLER = 1;

        [ThreadStatic]
        private static DEVPROPKEY dkref;

        private static ref DEVPROPKEY GetDevPropRef(DEVPROPKEY val)
        {
            dkref = val;
            return ref dkref;
        }

        /// <summary>
        /// Returns an exhaustive hardware tree of the entire computer with as much information as can be obtained. Each object descends from DeviceInfo.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static ObservableCollection<object> _internalEnumComputerExhaustive()
        {
            var col = new ObservableCollection<object>();
            var comp = _internalGetComputer();
            var vol = DeviceEnum.EnumVolumes();
            var dsk = DeviceEnum.EnumDisks();
            var net = DeviceEnum.EnumerateNetworkDevices();
            var hid = DeviceEnum.EnumerateDevices<HidDeviceInfo>(DevProp.GUID_DEVINTERFACE_HID);
            var prt = DeviceEnum.EnumPrinters();
            var bth = DeviceEnum.EnumBluetoothDevices();
            var procs = DeviceEnum.EnumProcessors();
            var mons = DeviceEnum.EnumMonitors();
            var adpt = new AdaptersCollection();

            // Dim part As DeviceInfo() = EnumerateDevices(Of DeviceInfo)(GUID_DEVINTERFACE_PARTITION)

            // this collection will never be empty.
            int i;
            int c = comp.Length;

            // We are going to match up the raw device enumeration with the detailed device interface enumerations
            // for the specific kinds of hardware that we know about (so far).  this is a work in progress
            // and I will be doing more classes going forward.

            try
            {
                for (i = 0; i < c; i++)
                {
                    if (comp[i].DeviceClass == DeviceClassEnum.DiskDrive || comp[i].DeviceClass == DeviceClassEnum.CdRom)
                    {
                        try
                        {
                            foreach (var d in dsk)
                            {
                                try
                                {
                                    if ((d.InstanceId.ToUpper().Trim() ?? "") == (comp[i].InstanceId.ToUpper().Trim() ?? "") || (d.PDOName.ToUpper().Trim() ?? "") == (comp[i].PDOName.ToUpper().Trim() ?? ""))
                                    {
                                        d.DeviceClassIcon = comp[i].DeviceClassIcon;
                                        comp[i] = d;
                                        break;
                                    }
                                }
                                catch
                                {
                                }
                            }
                        }
                        catch
                        {
                        }
                    }
                    else if (comp[i].DeviceClass == DeviceClassEnum.Net)
                    {
                        foreach (var nt in net)
                        {
                            if ((nt.InstanceId.ToUpper().Trim() ?? "") == (comp[i].InstanceId.ToUpper().Trim() ?? ""))
                            {
                                nt.DeviceClassIcon = comp[i].DeviceClassIcon;
                                foreach (var ad in adpt.Adapters)
                                {
                                    if (ad.DeviceInfo is null)
                                        continue;
                                    if ((ad.DeviceInfo.InstanceId ?? "") == (nt.InstanceId ?? ""))
                                    {
                                        nt.DeviceIcon = ad.DeviceIcon;
                                    }
                                }

                                comp[i] = nt;
                                break;
                            }
                        }
                    }
                    else if (comp[i].DeviceClass == DeviceClassEnum.Volume)
                    {
                        foreach (var vl in vol)
                        {
                            if ((vl.InstanceId.ToUpper().Trim() ?? "") == (comp[i].InstanceId.ToUpper().Trim() ?? "") || (vl.PDOName.ToUpper().Trim() ?? "") == (comp[i].PDOName.ToUpper().Trim() ?? ""))
                            {
                                if (vl.VolumePaths is object && vl.VolumePaths.Count() > 0)
                                {
                                    int? argiIndex = null;
                                    vl.DeviceIcon = Resources.MakeWPFImage(Resources.GetFileIcon(vl.VolumePaths[0], (Resources.SystemIconSizes)(int)(User32.SHIL_EXTRALARGE), iIndex: ref argiIndex));
                                }

                                vl.DeviceClassIcon = comp[i].DeviceClassIcon;
                                comp[i] = vl;
                                break;
                            }
                        }
                    }
                    else if (comp[i].DeviceClass == DeviceClassEnum.HidClass || comp[i].BusType == BusType.HID)
                    {
                        foreach (var hd in hid)
                        {
                            if ((hd.InstanceId.ToUpper().Trim() ?? "") == (comp[i].InstanceId.ToUpper().Trim() ?? ""))
                            {
                                hd.DeviceClassIcon = comp[i].DeviceClassIcon;
                                comp[i] = hd;
                                break;
                            }
                        }
                    }
                    else if (comp[i].DeviceClass == DeviceClassEnum.PrinterQueue)
                    {
                        foreach (var pr in prt)
                        {
                            if ((comp[i].FriendlyName ?? "") == (pr.FriendlyName ?? ""))
                            {
                                comp[i] = pr;
                                break;
                            }
                        }
                    }
                    else if (comp[i].DeviceClass == DeviceClassEnum.Monitor)
                    {
                        foreach (var mon in mons)
                        {
                            if ((comp[i].InstanceId ?? "") == (mon.InstanceId ?? ""))
                            {
                                comp[i] = mon;
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (comp[i].InstanceId.Substring(0, 8) == @"BTHENUM\")
                        {
                            comp[i].DeviceClass = DeviceClassEnum.Bluetooth;
                        }

                        if (comp[i].DeviceClass == DeviceClassEnum.Bluetooth)
                        {
                            foreach (var bt in bth)
                            {
                                if ((comp[i].InstanceId ?? "") == (bt.InstanceId ?? ""))
                                {
                                    comp[i] = bt;
                                    break;
                                }
                            }
                        }
                        else if (comp[i].DeviceClass == DeviceClassEnum.Processor)
                        {
                            foreach (var proc in procs)
                            {
                                if (comp[i].InstanceId == proc.InstanceId)
                                {
                                    comp[i] = proc;
                                    break;
                                }
                            }
                        }

                    }
                }
            }
            catch (Exception)
            {
            }

            // Call the shared LinkDevices function to pair parents and offspring to create a coherent tree of the system.
            DeviceInfo.LinkDevices(ref comp);
            foreach (var dad in comp)
                col.Add(dad);
            return col;
        }

        /// <summary>
        /// Enumerate (and optionally link) all computer hardware on the system.
        /// </summary>
        /// <param name="noLink">Optionally specify not to link parents to children.</param>
        /// <returns>An array of DeviceInfo objects.</returns>
        /// <remarks></remarks>
        public static DeviceInfo[] _internalGetComputer(bool noLink = false, Guid classOnly = default)
        {
            DeviceInfo[] devOut = null;
            int c = 0;
            var devInfo = default(SP_DEVINFO_DATA);
            var devInterface = default(SP_DEVICE_INTERFACE_DATA);
            var lIcon = new Dictionary<Guid, System.Drawing.Icon>();
            var mm = new SafePtr();
            var hicon = IntPtr.Zero;
            int picon = 0;
            
            System.Drawing.Icon icn = null;
            IntPtr hDev;
            
            // classOnly = Guid.Empty

            if (classOnly != Guid.Empty)
            {
                hDev = DevProp.SetupDiGetClassDevs(classOnly, IntPtr.Zero, IntPtr.Zero, (ClassDevFlags)(int)(DevProp.DIGCF_PRESENT));
            }
            else
            {
                hDev = DevProp.SetupDiGetClassDevs(Guid.Empty, IntPtr.Zero, IntPtr.Zero, (ClassDevFlags)(int)(DevProp.DIGCF_ALLCLASSES | DevProp.DIGCF_PRESENT | DevProp.DIGCF_DEVICEINTERFACE));
            }

            if (hDev == DevProp.INVALID_HANDLE_VALUE)
            {
                return null;
            }

            devInfo.cbSize = (uint)Marshal.SizeOf(devInfo);
            devInterface.cbSize = (uint)Marshal.SizeOf(devInterface);

            while (DevProp.SetupDiEnumDeviceInfo(hDev, (uint)c, out devInfo))
            {
                try
                {
                    Array.Resize(ref devOut, c + 1);

                    devOut[c] = _internalPopulateDeviceInfo<DeviceInfo>(hDev, default, devInfo, devInterface, mm);
                    DevProp.SetupDiEnumDeviceInterfaces(hDev, IntPtr.Zero, devOut[c].DeviceClassGuid, (uint)c, ref devInterface);

                    if (devInterface.InterfaceClassGuid != Guid.Empty)
                    {
                        devOut[c].DeviceInterfaceClassGuid = devInterface.InterfaceClassGuid;
                        devOut[c].DeviceInterfaceClass = DevEnumHelpers.GetDevInterfaceClassEnumFromGuid(devOut[c].DeviceInterfaceClassGuid);
                    }

                    if (!lIcon.ContainsKey(devOut[c].DeviceClassGuid))
                    {

                        DevProp.SetupDiLoadClassIcon(devOut[c].DeviceClassGuid, ref hicon, ref picon);

                        if (hicon != IntPtr.Zero)
                        {
                            icn = (System.Drawing.Icon)System.Drawing.Icon.FromHandle(hicon).Clone();
                            User32.DestroyIcon(hicon);
                        }

                        lIcon.Add(devOut[c].DeviceClassGuid, icn);
                        devOut[c].DeviceClassIcon = icn;
                    }
                    else
                    {
                        var argvalue = devOut[c].DeviceClassIcon;
                        lIcon.TryGetValue(devOut[c].DeviceClassGuid, out argvalue);
                        devOut[c].DeviceClassIcon = argvalue;
                    }
                }
                catch
                {
                    //Interaction.MsgBox(ex.Message + "\r\n" + "\r\n" + "Stack Trace: " + ex.StackTrace, MsgBoxStyle.Exclamation);
                }

                c += 1;
            }

            DevProp.SetupDiDestroyDeviceInfoList(hDev);
            if (!noLink)
                DeviceInfo.LinkDevices(ref devOut);
            return devOut;
        }

        public static List<DEVPROPKEY> _internalListDeviceProperties(DeviceInfo info, ClassDevFlags flags = ClassDevFlags.Present | ClassDevFlags.DeviceInterface, bool useClassId = true)
        {
            var nkey = new List<DEVPROPKEY>();
            uint c = 0U;

            MemPtr mm = new MemPtr();

            IntPtr hDev;
            Guid cid;

            if (useClassId)
            {
                cid = info.DeviceClassGuid;
            }
            else
            {
                cid = info.DeviceInterfaceClassGuid;
            }

            hDev = DevProp.SetupDiGetClassDevs(cid, IntPtr.Zero, IntPtr.Zero, flags);
            if (hDev == DevProp.INVALID_HANDLE_VALUE)
            {
                return null;
            }

            DevProp.SetupDiGetDevicePropertyKeys(hDev, ref info._devInfo, IntPtr.Zero, 0U, ref c, 0U);
            if (c == 0L)
            {
                DevProp.SetupDiDestroyDeviceInfoList(hDev);
                return null;
            }

            int devpsize = Marshal.SizeOf(DevProp.DEVPKEY_Device_Address);
            
            mm.Alloc(c * devpsize);
            
            DevProp.SetupDiGetDevicePropertyKeys(hDev, ref info._devInfo, mm, c, ref c, 0U);
            DevProp.SetupDiDestroyDeviceInfoList(hDev);
            
            for (int i = 0; i < c; i++)
                nkey.Add(mm.ToStructAt<DEVPROPKEY>(i * devpsize));

            mm.Free();
            return nkey;
        }

        /// <summary>
        /// Get an arbitrary device property from a previously-enumerated device.
        /// </summary>
        /// <param name="info">The DeviceInfo object of the device.</param>
        /// <param name="prop">The DevPropKey value to retrieve.</param>
        /// <param name="type">The property type of the value to retrieve.</param>
        /// <param name="flags">Optional flags to pass.</param>
        /// <param name="useClassId">Optional alternate class Id or interface Id to use.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static object _internalGetProperty(DeviceInfo info, DEVPROPKEY prop, DevPropTypes type, ClassDevFlags flags = ClassDevFlags.Present | ClassDevFlags.DeviceInterface, bool useClassId = false)
        {
            object ires;

            uint c;

            MemPtr mm = new MemPtr();
            IntPtr hDev;
            Guid cid;

            if (useClassId)
            {
                cid = info.DeviceClassGuid;
            }
            else
            {
                cid = info.DeviceInterfaceClassGuid;
            }

            hDev = DevProp.SetupDiGetClassDevs(cid, IntPtr.Zero, IntPtr.Zero, flags);
            
            if (hDev == DevProp.INVALID_HANDLE_VALUE)
            {
                return null;
            }

            uint utype = (uint)type;

            DevProp.SetupDiGetDeviceProperty(hDev, ref info._devInfo, ref prop, out utype, IntPtr.Zero, 0U, out c, 0U);
            type = unchecked((DevPropTypes)utype);

            if (c == 0L)
            {
                // MsgBox(FormatLastError(Marshal.GetLastWin32Error))
                DevProp.SetupDiDestroyDeviceInfoList(hDev);
                return null;
            }

            mm.Alloc(c);
            mm.ZeroMemory();

            uint argPropertyType1 = (uint)type;

            DevProp.SetupDiGetDeviceProperty(hDev, ref info._devInfo, ref prop, out argPropertyType1, mm, c, out c, 0U);
            DevProp.SetupDiDestroyDeviceInfoList(hDev);

            ires = _internalDevPropToObject(type, mm, (int)c);

            mm.Free();
            return ires;
        }

        /// <summary>
        /// Enumerate devices and return an instance of a specific descendent of DeviceInfo.
        /// </summary>
        /// <typeparam name="T">The DeviceInfo type of objects to return.</typeparam>
        /// <param name="ClassId">The Class Id or Interface Id to enumerate.</param>
        /// <param name="flags">Optional flags.</param>
        /// <returns>An array of T</returns>
        /// <remarks></remarks>
        public static T[] _internalEnumerateDevices<T>(Guid ClassId, ClassDevFlags flags = ClassDevFlags.Present | ClassDevFlags.DeviceInterface) where T : DeviceInfo, new()
        {
            T[] devOut = null;
            
            int c = 0;
           
            uint cu = 0U;
            
            var devInfo = default(SP_DEVINFO_DATA);
            var devInterface = default(SP_DEVICE_INTERFACE_DATA);

            var lIcon = new Dictionary<Guid, System.Drawing.Icon>();

            var mm = new SafePtr();
            var hicon = IntPtr.Zero;
            
            int picon = 0;
            
            System.Drawing.Icon icn = null;
            
            var hDev = DevProp.SetupDiGetClassDevs(ClassId, IntPtr.Zero, IntPtr.Zero, flags);
            
            if (hDev == DevProp.INVALID_HANDLE_VALUE)
            {
                return null;
            }

            devInfo.cbSize = (uint)Marshal.SizeOf(devInfo);
            devInterface.cbSize = (uint)Marshal.SizeOf(devInterface);
            
            DevProp.SetupDiLoadClassIcon(ClassId, ref hicon, ref picon);

            if (hicon != IntPtr.Zero)
            {
                icn = (System.Drawing.Icon)System.Drawing.Icon.FromHandle(hicon).Clone();
                User32.DestroyIcon(hicon);
            }

            if ((flags & ClassDevFlags.DeviceInterface) == ClassDevFlags.DeviceInterface)
            {
                while (DevProp.SetupDiEnumDeviceInterfaces(hDev, IntPtr.Zero, ClassId, cu, ref devInterface))
                {
                    c = (int)cu;

                    Array.Resize(ref devOut, c + 1);
                    DevProp.SetupDiEnumDeviceInfo(hDev, cu, out devInfo);

                    devOut[c] = _internalPopulateDeviceInfo<T>(hDev, ClassId, devInfo, devInterface, mm);
                    devOut[c].DeviceClassIcon = icn;

                    cu += 1;
                }
            }
            else
            {
                while (DevProp.SetupDiEnumDeviceInfo(hDev, cu, out devInfo))
                {
                    c = (int)cu;
                    Array.Resize(ref devOut, c + 1);

                    devOut[c] = _internalPopulateDeviceInfo<T>(hDev, default, devInfo, devInterface, mm);

                    DevProp.SetupDiEnumDeviceInterfaces(hDev, IntPtr.Zero, devOut[c].DeviceClassGuid, cu, ref devInterface);

                    if (devInterface.InterfaceClassGuid != Guid.Empty)
                    {
                        devOut[c].DeviceInterfaceClassGuid = devInterface.InterfaceClassGuid;
                        devOut[c].DeviceInterfaceClass = DevEnumHelpers.GetDevInterfaceClassEnumFromGuid(devOut[c].DeviceInterfaceClassGuid);
                    }

                    devOut[c].DeviceClassIcon = icn;
                    cu += 1;
                }

                // If c = 0 Then
                // MsgBox("Internal error: " & FormatLastError(Marshal.GetLastWin32Error))
                // End If

            }

            mm.Dispose();
            DevProp.SetupDiDestroyDeviceInfoList(hDev);
            return devOut;
        }

        public static System.Drawing.Icon GetClassIcon(Guid ClassId)
        {
            var hicon = IntPtr.Zero;

            System.Drawing.Icon icn = null;

            var hDev = DevProp.SetupDiGetClassDevs(ClassId, IntPtr.Zero, IntPtr.Zero, 0);

            if (hDev == DevProp.INVALID_HANDLE_VALUE)
            {
                return null;
            }

            int atmp = default;

            DevProp.SetupDiLoadClassIcon(ClassId, ref hicon, ref atmp);

            if (hicon != IntPtr.Zero)
            {
                icn = (System.Drawing.Icon)System.Drawing.Icon.FromHandle(hicon).Clone();
                User32.DestroyIcon(hicon);
            }

            DevProp.SetupDiDestroyDeviceInfoList(hDev);
            return icn;
        }

        /// <summary>
        /// Enumerate all local physical and virtual disk drives, including optical drives or volumes, depending on the class Id.
        /// </summary>
        /// <param name="DiskClass">The disk-type class Id or interface Id to use.</param>
        /// <returns>An array of DiskDeviceInfo objects.</returns>
        /// <remarks></remarks>
        public static DiskDeviceInfo[] _internalEnumDisks(Guid DiskClass = default)
        {
            var hHeap = new SafePtr();
            // hHeap.IsString = True

            try
            {
                DiskDeviceInfo[] info = null;

                if (DiskClass == Guid.Empty)
                    DiskClass = DevProp.GUID_DEVINTERFACE_DISK;

                var disk = DevProp.INVALID_HANDLE_VALUE;
                var diskNumber = new NativeDisk.STORAGE_DEVICE_NUMBER();

                uint bytesReturned = 0U;

                info = DeviceEnum.EnumerateDevices<DiskDeviceInfo>(DiskClass);

                if (info is null || info.Count() == 0)
                    return Array.Empty<DiskDeviceInfo>();

                foreach (var inf in info)
                {

                    object caps = _internalGetProperty(inf, DevProp.DEVPKEY_Device_Capabilities, DevPropTypes.Int32);

                    if (caps != null)
                    {
                        inf.Capabilities = (DeviceCapabilities)caps;
                    }
                    if (inf.Capabilities == DeviceCapabilities.None)
                    {

                        caps = (_internalGetProperty(inf, DevProp.DEVPKEY_Device_Capabilities, DevPropTypes.Int32, useClassId: true));

                        if (caps != null)
                        {
                            inf.Capabilities = (DeviceCapabilities)caps;
                        }
                    }

                    if (inf.DeviceClass == DeviceClassEnum.CdRom)
                    {
                        inf.Type = StorageType.Optical;
                    }
                    else if (inf.RemovalPolicy != DeviceRemovalPolicy.ExpectNoRemoval)
                    {

                        // this is a conundrum because these values are not predictable in some cases.
                        // we'll leave it this way, for now.
                        inf.Type = StorageType.Removable;

                        // If inf.Capabilities And DeviceCapabilities.Removable Then
                        // inf.Type = StorageType.RemovableHardDisk
                        // Else
                        // inf.Type = StorageType.Removable
                        // End If

                    }

                    if (DiskClass == DevProp.GUID_DEVINTERFACE_VOLUME || DiskClass == DevProp.GUID_DEVCLASS_VOLUME)
                    {
                        inf.IsVolume = true;

                        disk = IO.CreateFile(inf.DevicePath, IO.GENERIC_READ, IO.FILE_SHARE_READ | IO.FILE_SHARE_WRITE, IntPtr.Zero, IO.OPEN_EXISTING, IO.FILE_ATTRIBUTE_NORMAL, IntPtr.Zero);
                        
                        if (disk != DevProp.INVALID_HANDLE_VALUE)
                        {
                            PopulateVolumeInfo(inf, disk);

                            hHeap.Length = Marshal.SizeOf(diskNumber);
                            hHeap.ZeroMemory();
                            
                            bytesReturned = 0U;

                            NativeDisk.DeviceIoControl(disk, NativeDisk.IOCTL_STORAGE_GET_DEVICE_NUMBER, default, 0U, hHeap.handle, (uint)hHeap.Length, ref bytesReturned, default);
                            
                            if (bytesReturned > 0L)
                            {
                                diskNumber = hHeap.ToStruct<NativeDisk.STORAGE_DEVICE_NUMBER>();
                                inf.PhysicalDevice = (int)diskNumber.DeviceNumber;
                                if (diskNumber.PartitionNumber < 1024L)
                                {
                                    inf.PartitionNumber = (int)diskNumber.PartitionNumber;
                                }
                            }

                            User32.CloseHandle(disk);
                        }
                        else
                        {
                            PopulateVolumeInfo(inf);
                        }
                    }
                    else if (inf.Type != StorageType.Optical)
                    {
                        disk = IO.CreateFile(inf.DevicePath, IO.GENERIC_READ, IO.FILE_SHARE_READ | IO.FILE_SHARE_WRITE, IntPtr.Zero, IO.OPEN_EXISTING, IO.FILE_ATTRIBUTE_NORMAL, IntPtr.Zero);
                        if (disk != DevProp.INVALID_HANDLE_VALUE)
                        {
                            hHeap.Length = 128L;
                            hHeap.ZeroMemory();
                            NativeDisk.DeviceIoControl(disk, NativeDisk.IOCTL_DISK_GET_LENGTH_INFO, default, 0U, hHeap.handle, (uint)hHeap.Length, ref bytesReturned, default);
                            inf.DiskLayout = DiskLayoutInfo.CreateLayout(disk);
                            inf.Size = hHeap.LongAt(0L);
                            hHeap.Length = Marshal.SizeOf(diskNumber);
                            hHeap.ZeroMemory();
                            bytesReturned = 0U;
                            NativeDisk.DeviceIoControl(disk, NativeDisk.IOCTL_STORAGE_GET_DEVICE_NUMBER, default, 0U, hHeap.handle, (uint)hHeap.Length, ref bytesReturned, default);
                            if (bytesReturned > 0L)
                            {
                                diskNumber = hHeap.ToStruct<NativeDisk.STORAGE_DEVICE_NUMBER>();
                                inf.PhysicalDevice = (int)diskNumber.DeviceNumber;
                            }

                            User32.CloseHandle(disk);
                            disk = DevProp.INVALID_HANDLE_VALUE;
                        }
                    }

                    if (inf.FriendlyName == "Microsoft Virtual Disk")
                    {
                        inf.Type = StorageType.Virtual;
                        disk = IO.CreateFile(@"\\.\PhysicalDrive" + inf.PhysicalDevice, IO.GENERIC_READ, IO.FILE_SHARE_READ | IO.FILE_SHARE_WRITE, IntPtr.Zero, IO.OPEN_EXISTING, IO.FILE_ATTRIBUTE_NORMAL, IntPtr.Zero);
                        if (disk == DevProp.INVALID_HANDLE_VALUE)
                            continue;
                        var sdi = default(STORAGE_DEPENDENCY_INFO_V2);
                        var mm = new SafePtr();
                        uint su = 0U;
                        uint r;
                        int sdic = Marshal.SizeOf(sdi);
                        mm.Length = sdic;
                        sdi.Version = STORAGE_DEPENDENCY_INFO_VERSION.STORAGE_DEPENDENCY_INFO_VERSION_2;
                        mm.IntAt(0L) = (int)STORAGE_DEPENDENCY_INFO_VERSION.STORAGE_DEPENDENCY_INFO_VERSION_2;
                        mm.IntAt(1L) = 1;
                        r = VirtDisk.GetStorageDependencyInformation(disk, GET_STORAGE_DEPENDENCY_FLAG.GET_STORAGE_DEPENDENCY_FLAG_HOST_VOLUMES | GET_STORAGE_DEPENDENCY_FLAG.GET_STORAGE_DEPENDENCY_FLAG_DISK_HANDLE, (uint)mm.Length, mm, ref su);
                        if (su != sdic)
                        {
                            mm.Length = su;
                            r = VirtDisk.GetStorageDependencyInformation(disk, GET_STORAGE_DEPENDENCY_FLAG.GET_STORAGE_DEPENDENCY_FLAG_HOST_VOLUMES | GET_STORAGE_DEPENDENCY_FLAG.GET_STORAGE_DEPENDENCY_FLAG_DISK_HANDLE, (uint)mm.Length, mm, ref su);
                        }

                        if (r != 0L)
                        {
                            //Interaction.MsgBox(NativeErrorMethods.FormatLastError(), Constants.vbExclamation);
                        }
                        else
                        {
                            sdi.NumberEntries = mm.UIntAt(1L);
                            inf.BackingStore = new string[((int)sdi.NumberEntries)];
                            
                            var sne = sdi.NumberEntries;

                            for (long d = 0; d < sne; d++)
                            {
                                sdi.Version2Entries = mm.ToStruct<STORAGE_DEPENDENCY_INFO_TYPE_2>();
                                inf.BackingStore[(int)d] = Path.GetFullPath(sdi.Version2Entries.DependentVolumeRelativePath.ToString());
                            }
                        }

                        mm.Dispose();
                        User32.CloseHandle(disk);
                        inf.VirtualDisk = new VirtualDisk(inf, false);
                        disk = DevProp.INVALID_HANDLE_VALUE;
                    }
                }

                if (hHeap is object)
                    hHeap.Dispose();
                return info;
            }
            catch // (Exception ex)
            {
                if (hHeap is object)
                    hHeap.Dispose();
                return null;
            }
        }



        /// <summary>
        /// Populates a DiskDeviceInfo object with extended volume information.
        /// </summary>
        /// <param name="disk">The disk device object to populate.</param>
        /// <param name="handle">Optional handle to an open disk.</param>
        /// <remarks></remarks>
        public static void PopulateVolumeInfo(DiskDeviceInfo disk, IntPtr handle = default)
        {
            int pLen = (IO.MAX_PATH + 1) * 2;

            MemPtr mm1 = new MemPtr();
            MemPtr mm2 = new MemPtr();

            int mc = 0;

            mm1.Alloc(pLen);
            mm2.Alloc(pLen);

            mm1.ZeroMemory();
            mm2.ZeroMemory();

            string pp = new string('\0', 1024);
            IO.GetVolumeNameForVolumeMountPoint(disk.DevicePath + @"\", mm1, 1024U);

            // just get rid of the extra nulls (they like to stick around).

            disk.Type = StorageType.Volume;
            disk.VolumeGuidPath = (string)mm1;
            disk.VolumePaths = NativeDisk.GetVolumePaths((string)mm1);

            mm1.ZeroMemory();

            if (handle == IntPtr.Zero || handle == DevProp.INVALID_HANDLE_VALUE)
            {
                string arglpRootPathName = disk.VolumeGuidPath;
                uint arglpVolumeSerialNumber = disk.SerialNumber;
                var arglpFileSystemFlags = disk.VolumeFlags;

                NativeDisk.GetVolumeInformation(arglpRootPathName, mm1, pLen, ref arglpVolumeSerialNumber, ref mc, ref arglpFileSystemFlags, mm2, pLen);

                disk.VolumeGuidPath = arglpRootPathName;
                disk.SerialNumber = arglpVolumeSerialNumber;
                disk.VolumeFlags = arglpFileSystemFlags;
            }
            else
            {
                uint arglpVolumeSerialNumber1 = disk.SerialNumber;
                var arglpFileSystemFlags1 = disk.VolumeFlags;

                NativeDisk.GetVolumeInformationByHandle(handle, mm1, pLen, ref arglpVolumeSerialNumber1, ref mc, ref arglpFileSystemFlags1, mm2, pLen);

                disk.SerialNumber = arglpVolumeSerialNumber1;
                disk.VolumeFlags = arglpFileSystemFlags1;
            }

            disk.DiskExtents = NativeDisk.GetDiskExtentsFor(disk.DevicePath);
            disk.FriendlyName = (string)mm1;
            disk.FileSystem = (string)mm2;

            var a = default(int);
            var b = default(int);
            var c = default(int);
            var d = default(int);

            string arglpRootPathName1 = disk.VolumeGuidPath;

            IO.GetDiskFreeSpace(arglpRootPathName1, ref a, ref b, ref c, ref d);

            disk.VolumeGuidPath = arglpRootPathName1;
            disk.SectorSize = b;

            mm1.Free();
            mm2.Free();
        }


        internal readonly static DeviceClassEnum[] StandardStagingClasses = new[] { DeviceClassEnum.Adapter, DeviceClassEnum.Battery, DeviceClassEnum.Biometric, DeviceClassEnum.Bluetooth, DeviceClassEnum.Infrared, DeviceClassEnum.HidClass, DeviceClassEnum.Infrared, DeviceClassEnum.Keyboard, DeviceClassEnum.Media, DeviceClassEnum.Monitor, DeviceClassEnum.Mouse, DeviceClassEnum.Multifunction, DeviceClassEnum.PnpPrinters, DeviceClassEnum.Printer, DeviceClassEnum.PrinterQueue, DeviceClassEnum.Sound, DeviceClassEnum.Usb };
        internal readonly static DeviceClassEnum[] ExtraStagingClasses = new[] { DeviceClassEnum.Adapter, DeviceClassEnum.Battery, DeviceClassEnum.Biometric, DeviceClassEnum.Bluetooth, DeviceClassEnum.CdRom, DeviceClassEnum.DiskDrive, DeviceClassEnum.FloppyDisk, DeviceClassEnum.Infrared, DeviceClassEnum.HidClass, DeviceClassEnum.Infrared, DeviceClassEnum.Keyboard, DeviceClassEnum.Media, DeviceClassEnum.MediumChanger, DeviceClassEnum.Modem, DeviceClassEnum.Monitor, DeviceClassEnum.Mouse, DeviceClassEnum.Multifunction, DeviceClassEnum.Pcmcia, DeviceClassEnum.PnpPrinters, DeviceClassEnum.Printer, DeviceClassEnum.PrinterQueue, DeviceClassEnum.PrinterUpgrade, DeviceClassEnum.SmartCardReader, DeviceClassEnum.Sound, DeviceClassEnum.TapeDrive, DeviceClassEnum.Usb };

        /// <summary>
        /// Gets the device icon in some way, including looking at the device stage to get the photo-realistic icons from the Devices and Printers control panel folder.
        /// </summary>
        /// <param name="hDev"></param>
        /// <param name="devInfo"></param>
        /// <param name="infoOut"></param>
        /// <remarks></remarks>
        public static void _internalGetDeviceIcon(IntPtr hDev, SP_DEVINFO_DATA devInfo, DeviceInfo infoOut, DeviceClassEnum[] stagingClasses = null, bool noStaging = false)
        {
            IntPtr hIcon = new IntPtr();
            if (stagingClasses is null)
            {
                stagingClasses = StandardStagingClasses;
            }

            if (infoOut.DeviceIcon is null)
            {
                string sKey = @"::{20D04FE0-3AEA-1069-A2D8-08002B30309D}\::{21EC2020-3AEA-1069-A2DD-08002B30309D}\::{A8A91A66-3A7D-4424-8D24-04E180695C7A}";
                string pKey = @"\Provider%5CMicrosoft.Base.DevQueryObjects//DDO:";

                if (infoOut.ContainerId != Guid.Empty & noStaging == false)
                {
                    if (stagingClasses.Contains(infoOut.DeviceClass))
                    {
                        string rKey = "%7B" + infoOut.ContainerId.ToString("D").ToUpper() + "%7D";

                        var mm = new MemPtr();

                        string s = sKey + pKey + rKey;

                        ShellFileGetAttributesOptions argpsfgaoOut = 0;
                        NativeShell.SHParseDisplayName(s, IntPtr.Zero, out mm.handle, (ShellFileGetAttributesOptions)0, out argpsfgaoOut);

                        if (mm != IntPtr.Zero)
                        {
                            // Get a WPFImage
                            infoOut.DeviceIcon = Resources.MakeWPFImage(Resources.GetItemIcon(mm, (Resources.SystemIconSizes)(User32.SHIL_EXTRALARGE)));
                            mm.Free();
                        }
                    }
                }

                if (infoOut.DeviceIcon is null)
                {
                    if (DevProp.SetupDiLoadDeviceIcon(hDev, ref devInfo, 48U, 48U, 0U, ref hIcon))
                    {
                        var icn = System.Drawing.Icon.FromHandle(hIcon);
                        System.Drawing.Icon icn2 = (System.Drawing.Icon)icn.Clone();
                        User32.DestroyIcon(hIcon);
                        icn.Dispose();
                        infoOut.DeviceIcon = Resources.MakeWPFImage(icn2);
                        icn2.Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// Instantiates and populates a DeviceInfo-derived object with most of the common properties using an open handle to a device enumerator.
        /// </summary>
        /// <typeparam name="T">The type of DeviceInfo-derived object to return.</typeparam>
        /// <param name="hDev">A valid device enumerator handle.</param>
        /// <param name="ClassId">The class or interface Id.</param>
        /// <param name="devInfo">The raw SP_DEVINFO_DATA</param>
        /// <param name="devInterface">The raw SP_DEVICE_INTERFACE_DATA</param>
        /// <param name="mm">An open memory object.</param>
        /// <returns>A new instance of T.</returns>
        /// <remarks></remarks>
        public static T _internalPopulateDeviceInfo<T>(IntPtr hDev, Guid ClassId, SP_DEVINFO_DATA devInfo, SP_DEVICE_INTERFACE_DATA devInterface, SafePtr mm) where T : DeviceInfo, new()
        {
            string dumpFile = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\datatools-error.log";

            var devOut = new T();

            uint cbSize;

            var details = default(SP_DEVICE_INTERFACE_DETAIL_DATA);

            MemPtr pt;

            var sb = new System.Text.StringBuilder();

            try
            {
                sb.AppendLine("Class Guid: " + ClassId.ToString("B"));
                sb.AppendLine("Getting DeviceInterfaceDetail");
                sb.AppendLine("DevInfo cbSize: " + devInfo.cbSize);
                sb.AppendLine("DevInfo DevInst: " + devInfo.DevInst);
                devOut._devInfo = devInfo;
                devOut.DeviceInterfaceClassGuid = devInterface.InterfaceClassGuid;
                devOut.DeviceInterfaceClass = DevEnumHelpers.GetDevInterfaceClassEnumFromGuid(devOut.DeviceInterfaceClassGuid);

                // Get the DevicePath from DeviceInterfaceDetail
                mm.Length = 0L;
                mm.Length = Marshal.SizeOf(details);
                DevProp.SetupDiGetDeviceInterfaceDetail(hDev, ref devInterface, IntPtr.Zero, 0U, out cbSize, IntPtr.Zero);
                if (cbSize > 0L)
                {
                    mm.Length = cbSize;
                    mm.IntAt(0L) = IntPtr.Size == 4 ? 6 : 8;
                    if (cbSize > 4L)
                    {
                        uint argRequiredSize = default;
                        if (DevProp.SetupDiGetDeviceInterfaceDetail(hDev, ref devInterface, mm.handle, cbSize, out argRequiredSize, default(IntPtr)))
                        {

                            // mm.PullIn(0, 4)
                            devOut.DevicePath = mm.GetString(4L);
                        }
                    }

                    sb.AppendLine("Getting DeviceInterfaceDetail Succeed");
                }
                // ClassGuid

                sb.AppendLine("Property ClassGuid");
                cbSize = 0U;
                uint argPropertyType = DevProp.DEVPROP_TYPE_GUID;

                DEVPROPKEY prop = DevProp.DEVPKEY_Device_ClassGuid;

                DevProp.SetupDiGetDeviceProperty(hDev, ref devInfo, ref GetDevPropRef(DevProp.DEVPKEY_Device_ClassGuid), out argPropertyType, IntPtr.Zero, 0U, out cbSize, 0U);
                if (cbSize > 0L)
                {
                    mm.Length = cbSize;
                    mm.ZeroMemory();
                    uint argPropertyType1 = DevProp.DEVPROP_TYPE_GUID;
                    DevProp.SetupDiGetDeviceProperty(hDev, ref devInfo, ref GetDevPropRef(DevProp.DEVPKEY_Device_ClassGuid), out argPropertyType1, mm, cbSize, out cbSize, 0U);
                    if (cbSize > 0L)
                    {
                        devOut.DeviceClassGuid = new Guid(mm.ToByteArray(0L, 16));
                        devOut.DeviceClass = DevEnumHelpers.GetDevClassEnumFromGuid(devOut.DeviceClassGuid);
                    }
                }
                // InterfaceClassGuid

                sb.AppendLine("Property InterfaceClassGuid");
                cbSize = 0U;
                uint argPropertyType2 = DevProp.DEVPROP_TYPE_GUID;
                DevProp.SetupDiGetDeviceProperty(hDev, ref devInfo, ref GetDevPropRef(DevProp.DEVPKEY_DeviceInterface_ClassGuid), out argPropertyType2, IntPtr.Zero, 0U, out cbSize, 0U);
                if (cbSize > 0L)
                {
                    mm.Length = cbSize;
                    mm.ZeroMemory();
                    uint argPropertyType3 = DevProp.DEVPROP_TYPE_GUID;
                    DevProp.SetupDiGetDeviceProperty(hDev, ref devInfo, ref GetDevPropRef(DevProp.DEVPKEY_DeviceInterface_ClassGuid), out argPropertyType3, mm, cbSize, out cbSize, 0U);
                    if (cbSize > 0L)
                    {
                        devOut.DeviceInterfaceClassGuid = new Guid(mm.ToByteArray(0L, 16));
                        devOut.DeviceInterfaceClass = DevEnumHelpers.GetDevInterfaceClassEnumFromGuid(devOut.DeviceInterfaceClassGuid);
                    }
                }

                // InstallDate

                sb.AppendLine("Property InstallDate");
                cbSize = 0U;
                uint argPropertyType4 = DevProp.DEVPROP_TYPE_FILETIME;
                DevProp.SetupDiGetDeviceProperty(hDev, ref devInfo, ref GetDevPropRef(DevProp.DEVPKEY_Device_InstallDate), out argPropertyType4, IntPtr.Zero, 0U, out cbSize, 0U);
                if (cbSize > 0L)
                {
                    mm.Length = cbSize;
                    mm.ZeroMemory();
                    uint argPropertyType5 = DevProp.DEVPROP_TYPE_FILETIME;
                    DevProp.SetupDiGetDeviceProperty(hDev, ref devInfo, ref GetDevPropRef(DevProp.DEVPKEY_Device_InstallDate), out argPropertyType5, mm, cbSize, out cbSize, 0U);
                    if (cbSize > 0L)
                        devOut.InstallDate = (DateTime)(_internalDevPropToObject(DevPropTypes.FileTime, mm, (int)cbSize));
                }
                // Characteristics

                sb.AppendLine("Property Characteristics");
                cbSize = 0U;
                uint argPropertyType6 = DevProp.DEVPROP_TYPE_INT32;
                DevProp.SetupDiGetDeviceProperty(hDev, ref devInfo, ref GetDevPropRef(DevProp.DEVPKEY_Device_Characteristics), out argPropertyType6, IntPtr.Zero, 0U, out cbSize, 0U);
                if (cbSize > 0L)
                {
                    mm.Length = cbSize;
                    mm.ZeroMemory();
                    uint argPropertyType7 = DevProp.DEVPROP_TYPE_INT32;
                    DevProp.SetupDiGetDeviceProperty(hDev, ref devInfo, ref GetDevPropRef(DevProp.DEVPKEY_Device_Characteristics), out argPropertyType7, mm, cbSize, out cbSize, 0U);
                    if (cbSize > 0L)
                        devOut.Characteristics = (DeviceCharacteristcs)(int)(mm.IntAt(0L));
                }
                // Removal Policy

                sb.AppendLine("Property Removal Policy");
                cbSize = 0U;
                uint argPropertyType8 = DevProp.DEVPROP_TYPE_INT32;
                DevProp.SetupDiGetDeviceProperty(hDev, ref devInfo, ref GetDevPropRef(DevProp.DEVPKEY_Device_RemovalPolicy), out argPropertyType8, IntPtr.Zero, 0U, out cbSize, 0U);
                if (cbSize > 0L)
                {
                    mm.Length = cbSize;
                    mm.ZeroMemory();
                    uint argPropertyType9 = DevProp.DEVPROP_TYPE_INT32;
                    DevProp.SetupDiGetDeviceProperty(hDev, ref devInfo, ref GetDevPropRef(DevProp.DEVPKEY_Device_RemovalPolicy), out argPropertyType9, mm, cbSize, out cbSize, 0U);
                    if (cbSize > 0L)
                        devOut.RemovalPolicy = (DeviceRemovalPolicy)(int)(mm.IntAt(0L));
                }
                // Safe Removal Required

                sb.AppendLine("Property Safe Removal Required");
                cbSize = 0U;
                uint argPropertyType10 = DevProp.DEVPROP_TYPE_BOOLEAN;
                DevProp.SetupDiGetDeviceProperty(hDev, ref devInfo, ref GetDevPropRef(DevProp.DEVPKEY_Device_SafeRemovalRequired), out argPropertyType10, IntPtr.Zero, 0U, out cbSize, 0U);
                if (cbSize > 0L)
                {
                    mm.Length = cbSize;
                    mm.ZeroMemory();
                    uint argPropertyType11 = DevProp.DEVPROP_TYPE_BOOLEAN;
                    DevProp.SetupDiGetDeviceProperty(hDev, ref devInfo, ref GetDevPropRef(DevProp.DEVPKEY_Device_SafeRemovalRequired), out argPropertyType11, mm, cbSize, out cbSize, 0U);
                    if (cbSize > 0L)
                        devOut.SafeRemovalRequired = (mm.ByteAt(0L) == 1 ? true : false);
                }

                // BusType

                sb.AppendLine("Property BusType");
                cbSize = 0U;
                uint argPropertyType12 = DevProp.DEVPROP_TYPE_GUID;
                DevProp.SetupDiGetDeviceProperty(hDev, ref devInfo, ref GetDevPropRef(DevProp.DEVPKEY_Device_BusTypeGuid), out argPropertyType12, IntPtr.Zero, 0U, out cbSize, 0U);
                if (cbSize > 0L)
                {
                    mm.Length = cbSize;
                    mm.ZeroMemory();
                    uint argPropertyType13 = DevProp.DEVPROP_TYPE_GUID;
                    DevProp.SetupDiGetDeviceProperty(hDev, ref devInfo, ref GetDevPropRef(DevProp.DEVPKEY_Device_BusTypeGuid), out argPropertyType13, mm, cbSize, out cbSize, 0U);
                    if (cbSize > 0L)
                        devOut.BusType = DevPropDialog.GuidToBusType(new Guid(mm.ToByteArray(0L, 16)));
                }
                // ContainerId

                sb.AppendLine("Property ContainerId");
                cbSize = 0U;
                uint argPropertyType14 = DevProp.DEVPROP_TYPE_GUID;
                DevProp.SetupDiGetDeviceProperty(hDev, ref devInfo, ref GetDevPropRef(DevProp.DEVPKEY_Device_ContainerId), out argPropertyType14, IntPtr.Zero, 0U, out cbSize, 0U);
                if (cbSize > 0L)
                {
                    mm.Length = cbSize;
                    mm.ZeroMemory();
                    uint argPropertyType15 = DevProp.DEVPROP_TYPE_GUID;
                    DevProp.SetupDiGetDeviceProperty(hDev, ref devInfo, ref GetDevPropRef(DevProp.DEVPKEY_Device_ContainerId), out argPropertyType15, mm, cbSize, out cbSize, 0U);
                    if (cbSize > 0L)
                        devOut.ContainerId = new Guid(mm.ToByteArray(0L, 16));
                }

                // Children

                sb.AppendLine("Property Children");
                cbSize = 0U;
                uint argPropertyType16 = DevProp.DEVPROP_TYPE_STRING_LIST;
                DevProp.SetupDiGetDeviceProperty(hDev, ref devInfo, ref GetDevPropRef(DevProp.DEVPKEY_Device_Children), out argPropertyType16, IntPtr.Zero, 0U, out cbSize, 0U);
                if (cbSize > 0L)
                {
                    mm.Length = (cbSize + 4L) * 2L;
                    mm.ZeroMemory();
                    uint argPropertyType17 = DevProp.DEVPROP_TYPE_STRING_LIST;
                    DevProp.SetupDiGetDeviceProperty(hDev, ref devInfo, ref GetDevPropRef(DevProp.DEVPKEY_Device_Children), out argPropertyType17, mm.handle, cbSize, out cbSize, 0U);
                    pt = mm.handle;
                    if (cbSize > 0L)
                        devOut.Children = pt.GetStringArray(0L);
                }
                // HardwareIds

                sb.AppendLine("Property HardwareIds");
                cbSize = 0U;
                uint argPropertyType18 = DevProp.DEVPROP_TYPE_STRING_LIST;
                DevProp.SetupDiGetDeviceProperty(hDev, ref devInfo, ref GetDevPropRef(DevProp.DEVPKEY_Device_HardwareIds), out argPropertyType18, IntPtr.Zero, 0U, out cbSize, 0U);
                if (cbSize > 0L)
                {
                    mm.Length = (cbSize + 4L) * 2L;
                    mm.ZeroMemory();
                    uint argPropertyType19 = DevProp.DEVPROP_TYPE_STRING_LIST;
                    DevProp.SetupDiGetDeviceProperty(hDev, ref devInfo, ref GetDevPropRef(DevProp.DEVPKEY_Device_HardwareIds), out argPropertyType19, mm, cbSize, out cbSize, 0U);
                    pt = mm.handle;
                    if (cbSize > 0L)
                        devOut.HardwareIds = pt.GetStringArray(0L);
                }
                // LocationPaths

                sb.AppendLine("Property LocationPaths");
                cbSize = 0U;
                uint argPropertyType20 = DevProp.DEVPROP_TYPE_STRING_LIST;
                DevProp.SetupDiGetDeviceProperty(hDev, ref devInfo, ref GetDevPropRef(DevProp.DEVPKEY_Device_LocationPaths), out argPropertyType20, IntPtr.Zero, 0U, out cbSize, 0U);
                if (cbSize > 0L)
                {
                    mm.Length = cbSize;
                    mm.ZeroMemory();
                    uint argPropertyType21 = DevProp.DEVPROP_TYPE_STRING_LIST;
                    DevProp.SetupDiGetDeviceProperty(hDev, ref devInfo, ref GetDevPropRef(DevProp.DEVPKEY_Device_LocationPaths), out argPropertyType21, mm, cbSize, out cbSize, 0U);
                    pt = mm.handle;
                    if (cbSize > 0L)
                        devOut.LocationPaths = pt.GetStringArray(0L);
                }
                // Parent Device

                sb.AppendLine("Property Parent Device");
                cbSize = 0U;
                uint argPropertyType22 = DevProp.DEVPROP_TYPE_STRING;
                DevProp.SetupDiGetDeviceProperty(hDev, ref devInfo, ref GetDevPropRef(DevProp.DEVPKEY_Device_Parent), out argPropertyType22, IntPtr.Zero, 0U, out cbSize, 0U);
                if (cbSize > 0L)
                {
                    mm.Length = cbSize;
                    mm.ZeroMemory();
                    uint argPropertyType23 = DevProp.DEVPROP_TYPE_STRING;
                    DevProp.SetupDiGetDeviceProperty(hDev, ref devInfo, ref GetDevPropRef(DevProp.DEVPKEY_Device_Parent), out argPropertyType23, mm, cbSize, out cbSize, 0U);
                    if (cbSize > 0L)
                        devOut.Parent = mm.ToString();
                }
                // Location Info

                sb.AppendLine("Property Location Info");
                cbSize = 0U;
                uint argPropertyType24 = DevProp.DEVPROP_TYPE_STRING;
                DevProp.SetupDiGetDeviceProperty(hDev, ref devInfo, ref GetDevPropRef(DevProp.DEVPKEY_Device_LocationInfo), out argPropertyType24, IntPtr.Zero, 0U, out cbSize, 0U);
                if (cbSize > 0L)
                {
                    mm.Length = cbSize;
                    mm.ZeroMemory();
                    uint argPropertyType25 = DevProp.DEVPROP_TYPE_STRING;
                    DevProp.SetupDiGetDeviceProperty(hDev, ref devInfo, ref GetDevPropRef(DevProp.DEVPKEY_Device_LocationInfo), out argPropertyType25, mm, cbSize, out cbSize, 0U);
                    if (cbSize > 0L)
                        devOut.LocationInfo = mm.ToString();
                }
                // Physical Device Location

                sb.AppendLine("Property Physical Device Location");
                sb.AppendLine("Getting cbSize");
                cbSize = 0U;
                uint argPropertyType26 = DevProp.DEVPROP_TYPE_BINARY;
                DevProp.SetupDiGetDeviceProperty(hDev, ref devInfo, ref GetDevPropRef(DevProp.DEVPKEY_Device_PhysicalDeviceLocation), out argPropertyType26, IntPtr.Zero, 0U, out cbSize, 0U);
                sb.AppendLine("cbSize is " + cbSize);
                if (cbSize > 0L)
                {
                    mm.Length = cbSize;
                    mm.ZeroMemory();
                    sb.AppendLine("Calling to get Physical Device Location");
                    uint argPropertyType27 = DevProp.DEVPROP_TYPE_BINARY;
                    DevProp.SetupDiGetDeviceProperty(hDev, ref devInfo, ref GetDevPropRef(DevProp.DEVPKEY_Device_PhysicalDeviceLocation), out argPropertyType27, mm, cbSize, out cbSize, 0U);
                    if (cbSize > 0L)
                    {
                        sb.AppendLine("Grabbing bytes");
                        devOut.PhysicalPath = mm.ToByteArray(0L, (int)cbSize);
                    }
                }
                // PDOName

                sb.AppendLine("Property PDOName");
                cbSize = 0U;
                uint argPropertyType28 = DevProp.DEVPROP_TYPE_STRING;
                DevProp.SetupDiGetDeviceProperty(hDev, ref devInfo, ref GetDevPropRef(DevProp.DEVPKEY_Device_PDOName), out argPropertyType28, IntPtr.Zero, 0U, out cbSize, 0U);
                if (cbSize > 0L)
                {
                    mm.Length = cbSize;
                    mm.ZeroMemory();
                    uint argPropertyType29 = DevProp.DEVPROP_TYPE_STRING;
                    DevProp.SetupDiGetDeviceProperty(hDev, ref devInfo, ref GetDevPropRef(DevProp.DEVPKEY_Device_PDOName), out argPropertyType29, mm, cbSize, out cbSize, 0U);
                    if (cbSize > 0L)
                        devOut.PDOName = mm.ToString();
                }
                // Description

                sb.AppendLine("Property Description");
                cbSize = 0U;
                uint argPropertyType30 = DevProp.DEVPROP_TYPE_STRING;
                DevProp.SetupDiGetDeviceProperty(hDev, ref devInfo, ref GetDevPropRef(DevProp.DEVPKEY_Device_DeviceDesc), out argPropertyType30, IntPtr.Zero, 0U, out cbSize, 0U);
                if (cbSize > 0L)
                {
                    mm.Length = cbSize;
                    mm.ZeroMemory();
                    uint argPropertyType31 = DevProp.DEVPROP_TYPE_STRING;
                    DevProp.SetupDiGetDeviceProperty(hDev, ref devInfo, ref GetDevPropRef(DevProp.DEVPKEY_Device_DeviceDesc), out argPropertyType31, mm, cbSize, out cbSize, 0U);
                    if (cbSize > 0L)
                        devOut.Description = mm.ToString();
                }
                // ClassName

                sb.AppendLine("Property ClassName");
                cbSize = 0U;
                uint argPropertyType32 = DevProp.DEVPROP_TYPE_STRING;
                DevProp.SetupDiGetDeviceProperty(hDev, ref devInfo, ref GetDevPropRef(DevProp.DEVPKEY_Device_Class), out argPropertyType32, IntPtr.Zero, 0U, out cbSize, 0U);
                if (cbSize > 0L)
                {
                    mm.Length = cbSize;
                    mm.ZeroMemory();
                    uint argPropertyType33 = DevProp.DEVPROP_TYPE_STRING;
                    DevProp.SetupDiGetDeviceProperty(hDev, ref devInfo, ref GetDevPropRef(DevProp.DEVPKEY_Device_Class), out argPropertyType33, mm, cbSize, out cbSize, 0U);
                    if (cbSize > 0L)
                        devOut.ClassName = mm.ToString();
                }

                // Manufacturer

                sb.AppendLine("Property Manufacturer");
                cbSize = 0U;
                uint argPropertyType34 = DevProp.DEVPROP_TYPE_STRING;
                DevProp.SetupDiGetDeviceProperty(hDev, ref devInfo, ref GetDevPropRef(DevProp.DEVPKEY_Device_Manufacturer), out argPropertyType34, IntPtr.Zero, 0U, out cbSize, 0U);
                if (cbSize > 0L)
                {
                    mm.Length = cbSize;
                    mm.ZeroMemory();
                    uint argPropertyType35 = DevProp.DEVPROP_TYPE_STRING;
                    DevProp.SetupDiGetDeviceProperty(hDev, ref devInfo, ref GetDevPropRef(DevProp.DEVPKEY_Device_Manufacturer), out argPropertyType35, mm, cbSize, out cbSize, 0U);
                    if (cbSize > 0L)
                        devOut.Manufacturer = mm.ToString();
                }

                // Model

                sb.AppendLine("Property BusReportedDeviceDesc (string)");
                cbSize = 0U;
                uint argPropertyType36 = DevProp.DEVPROP_TYPE_STRING;
                DevProp.SetupDiGetDeviceProperty(hDev, ref devInfo, ref GetDevPropRef(DevProp.DEVPKEY_Device_BusReportedDeviceDesc), out argPropertyType36, IntPtr.Zero, 0U, out cbSize, 0U);
                if (cbSize > 0L)
                {
                    mm.Length = cbSize;
                    mm.ZeroMemory();
                    uint argPropertyType37 = DevProp.DEVPROP_TYPE_STRING;
                    DevProp.SetupDiGetDeviceProperty(hDev, ref devInfo, ref GetDevPropRef(DevProp.DEVPKEY_Device_BusReportedDeviceDesc), out argPropertyType37, mm, cbSize, out cbSize, 0U);
                    if (cbSize > 0L)
                        devOut.BusReportedDeviceDesc = mm.ToString();
                }

                // ModelId

                sb.AppendLine("Property ModelId");
                cbSize = 0U;
                uint argPropertyType38 = DevProp.DEVPROP_TYPE_GUID;
                DevProp.SetupDiGetDeviceProperty(hDev, ref devInfo, ref GetDevPropRef(DevProp.DEVPKEY_Device_ModelId), out argPropertyType38, IntPtr.Zero, 0U, out cbSize, 0U);
                if (cbSize > 0L)
                {
                    mm.Length = cbSize;
                    mm.ZeroMemory();
                    uint argPropertyType39 = DevProp.DEVPROP_TYPE_GUID;
                    DevProp.SetupDiGetDeviceProperty(hDev, ref devInfo, ref GetDevPropRef(DevProp.DEVPKEY_Device_ModelId), out argPropertyType39, mm, cbSize, out cbSize, 0U);
                    if (cbSize > 0L)
                    {
                        devOut.ModelId = mm.GuidAt(0L);
                    }
                }

                // UINumber

                sb.AppendLine("Property UINumber");
                cbSize = 0U;
                uint argPropertyType40 = DevProp.DEVPROP_TYPE_INT32;
                DevProp.SetupDiGetDeviceProperty(hDev, ref devInfo, ref GetDevPropRef(DevProp.DEVPKEY_Device_UINumber), out argPropertyType40, IntPtr.Zero, 0U, out cbSize, 0U);
                if (cbSize > 0L)
                {
                    mm.Length = cbSize;
                    mm.ZeroMemory();
                    uint argPropertyType41 = DevProp.DEVPROP_TYPE_INT32;
                    DevProp.SetupDiGetDeviceProperty(hDev, ref devInfo, ref GetDevPropRef(DevProp.DEVPKEY_Device_UINumber), out argPropertyType41, mm, cbSize, out cbSize, 0U);
                    if (cbSize > 0L)
                        devOut.UINumber = mm.IntAt(0L);
                }

                // FriendlyName
                sb.AppendLine("Property FriendlyName");
                cbSize = 0U;
                uint argPropertyType42 = DevProp.DEVPROP_TYPE_STRING;
                DevProp.SetupDiGetDeviceProperty(hDev, ref devInfo, ref GetDevPropRef(DevProp.DEVPKEY_Device_FriendlyName), out argPropertyType42, IntPtr.Zero, 0U, out cbSize, 0U);
                if (cbSize > 0L)
                {
                    mm.Length = cbSize;
                    mm.ZeroMemory();
                    uint argPropertyType43 = DevProp.DEVPROP_TYPE_STRING;
                    DevProp.SetupDiGetDeviceProperty(hDev, ref devInfo, ref GetDevPropRef(DevProp.DEVPKEY_Device_FriendlyName), out argPropertyType43, mm, cbSize, out cbSize, 0U);
                    if (cbSize > 0L)
                        devOut.FriendlyName = mm.ToString();
                }
                // InstanceId

                sb.AppendLine("Property InstanceId");
                cbSize = 0U;
                uint argPropertyType44 = DevProp.DEVPROP_TYPE_STRING;
                DevProp.SetupDiGetDeviceProperty(hDev, ref devInfo, ref GetDevPropRef(DevProp.DEVPKEY_Device_InstanceId), out argPropertyType44, IntPtr.Zero, 0U, out cbSize, 0U);
                if (cbSize > 0L)
                {
                    mm.Length = cbSize;
                    mm.ZeroMemory();
                    uint argPropertyType45 = DevProp.DEVPROP_TYPE_STRING;
                    DevProp.SetupDiGetDeviceProperty(hDev, ref devInfo, ref GetDevPropRef(DevProp.DEVPKEY_Device_InstanceId), out argPropertyType45, mm, cbSize, out cbSize, 0U);
                    if (cbSize > 0L)
                        devOut.InstanceId = mm.ToString();
                }

                // Get the device icon
                _internalGetDeviceIcon(hDev, devInfo, devOut);
                File.AppendAllText(dumpFile, sb.ToString());
                return devOut;
            }
            catch (Exception ex)
            {
                sb.AppendLine(ex.Message);
                sb.AppendLine(NativeErrorMethods.FormatLastError((uint)Marshal.GetLastWin32Error()));
                
                //Interaction.MsgBox(ex.Message + " : See " + dumpFile + " for more clues." + "\r\n" + NativeErrorMethods.FormatLastError((uint)Marshal.GetLastWin32Error()));
                
                File.AppendAllText(dumpFile, sb.ToString());
                return devOut;
            }
        }

        /// <summary>
        /// Digests a device property type and raw data and returns the equivalent CLR object.
        /// </summary>
        /// <param name="type">The property type.</param>
        /// <param name="data">The raw data pointer.</param>
        /// <param name="length">The length of the data.</param>
        /// <returns>A managed-memory object equivalent.</returns>
        /// <remarks></remarks>
        public static object _internalDevPropToObject(DevPropTypes type, IntPtr data, int length = 0)
        {
            MemPtr mm = data;
            switch (type)
            {
                case DevPropTypes.Binary:
                    {
                        return mm.ToByteArray(0L, length);
                    }

                case DevPropTypes.Boolean:
                    {
                        return (mm.ByteAt(0L) == 0 ? false : true);
                    }

                case DevPropTypes.Byte:
                    {
                        return mm.ByteAt(0L);
                    }

                case DevPropTypes.SByte:
                    {
                        return mm.SByteAt(0L);
                    }

                case DevPropTypes.Int16:
                    {
                        return mm.ShortAt(0L);
                    }

                case DevPropTypes.UInt16:
                    {
                        return mm.UShortAt(0L);
                    }

                case DevPropTypes.Int32:
                    {
                        return mm.IntAt(0L);
                    }

                case DevPropTypes.UInt32:
                    {
                        return mm.UIntAt(0L);
                    }

                case DevPropTypes.Int64:
                    {
                        return mm.LongAt(0L);
                    }

                case DevPropTypes.UInt64:
                    {
                        return mm.ULongAt(0L);
                    }

                case DevPropTypes.Currency:
                    {

                        // I had to read the documentation on MSDN very carefully to understand why this needs to be.
                        return mm.DoubleAt(0L) * 10000.0d;
                    }

                case DevPropTypes.Float:
                    {
                        return mm.SingleAt(0L);
                    }

                case DevPropTypes.Date:
                    {

                        // based on what the MSDN describes of this property format, this is what
                        // I believe needs to be done to make the value into an acceptable CLR DateTime object.
                        double d = mm.DoubleAt(0L);
                        var t = new TimeSpan((int)(d * 24d), 0, 0);
                        var dt = DateTime.Parse("1899-12-31");
                        dt.Add(t);
                        return dt;
                    }

                case DevPropTypes.Decimal:
                    {
                        return mm.DecimalAt(0L);
                    }

                case DevPropTypes.FileTime:
                    {
                        var ft = mm.ToStruct<FILETIME>();
                        return ft.ToDateTime();
                    }

                case DevPropTypes.DevPropKey:
                    {
                        DEVPROPKEY dk;
                        dk = mm.ToStruct<DEVPROPKEY>();
                        return dk;
                    }

                case DevPropTypes.Guid:
                    {
                        return mm.GuidAt(0L);
                    }

                case DevPropTypes.SecurityDescriptor:
                    {
                        var sd = mm.ToStruct<SecurityDescriptor.SECURITY_DESCRIPTOR>();
                        return sd;
                    }

                case DevPropTypes.String:
                    {
                        return mm.ToString();
                    }

                case DevPropTypes.StringList:
                    {
                        return mm.GetStringArray(0L);
                    }

                case DevPropTypes.DevPropType:
                    {
                        return mm.IntAt(0L);
                    }

                case DevPropTypes.SecurityDescriptorString:
                    {
                        return mm.ToString();
                    }

                case DevPropTypes.StringIndirect:
                    {
                        // load the string resource, itself, from the file.
                        return Resources.LoadStringResource(mm.ToString());
                    }

                case DevPropTypes.NTStatus:
                    {
                        return mm.IntAt(0L);
                    }
            }

            return null;
        }
    }
}