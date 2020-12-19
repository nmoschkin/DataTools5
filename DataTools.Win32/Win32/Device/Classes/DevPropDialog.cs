// ************************************************* ''
// DataTools C# Native Utility Library For Windows - Interop
//
// Module: DevProp
//         Native Device Properites.
// 
// Copyright (C) 2011-2020 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the MIT License   
// ************************************************* ''

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using DataTools.Text;

namespace DataTools.Win32
{
    /// <summary>
    /// Device properties dialog
    /// </summary>
    public static class DevPropDialog
    {
        [DllImport("devmgr.dll", EntryPoint = "DeviceProperties_RunDLLW", CharSet = CharSet.Unicode)]
        static extern bool DeviceProperties_RunDLL(IntPtr hwnd, IntPtr hAppInstance, [MarshalAs(UnmanagedType.LPWStr)] string cmdLine, int nCmdShow);

        /// <summary>
        /// Opens the hardware properties dialog box for the specified instance id.
        /// </summary>
        /// <param name="InstanceId"></param>
        /// <param name="hwnd"></param>
        /// <remarks></remarks>
        public static void OpenDeviceProperties(string InstanceId, IntPtr hwnd = default)
        {
            if (InstanceId is null)
                return;
            
            string argcmdLine = "/DeviceId \"" + InstanceId + "\"";
            
            DeviceProperties_RunDLL(hwnd, Process.GetCurrentProcess().Handle, argcmdLine, User32.SW_SHOWNORMAL);
        }

        internal static BusType GuidToBusType(Guid g)
        {
            if (g == DevProp.GUID_BUS_TYPE_INTERNAL)
                return BusType.Internal;
            if (g == DevProp.GUID_BUS_TYPE_PCMCIA)
                return BusType.PCMCIA;
            if (g == DevProp.GUID_BUS_TYPE_PCI)
                return BusType.PCI;
            if (g == DevProp.GUID_BUS_TYPE_ISAPNP)
                return BusType.ISAPnP;
            if (g == DevProp.GUID_BUS_TYPE_EISA)
                return BusType.EISA;
            if (g == DevProp.GUID_BUS_TYPE_MCA)
                return BusType.MCA;
            if (g == DevProp.GUID_BUS_TYPE_SERENUM)
                return BusType.Serenum;
            if (g == DevProp.GUID_BUS_TYPE_USB)
                return BusType.USB;
            if (g == DevProp.GUID_BUS_TYPE_LPTENUM)
                return BusType.ParallelPort;
            if (g == DevProp.GUID_BUS_TYPE_USBPRINT)
                return BusType.USBPrinter;
            if (g == DevProp.GUID_BUS_TYPE_DOT4PRT)
                return BusType.DOT4Printer;
            if (g == DevProp.GUID_BUS_TYPE_1394)
                return BusType.Bus1394;
            if (g == DevProp.GUID_BUS_TYPE_HID)
                return BusType.HID;
            if (g == DevProp.GUID_BUS_TYPE_AVC)
                return BusType.AVC;
            if (g == DevProp.GUID_BUS_TYPE_IRDA)
                return BusType.IRDA;
            if (g == DevProp.GUID_BUS_TYPE_SD)
                return BusType.SD;
            if (g == DevProp.GUID_BUS_TYPE_ACPI)
                return BusType.ACPI;
            if (g == DevProp.GUID_BUS_TYPE_SW_DEVICE)
                return BusType.SoftwareDevice;
            return 0;
        }
    }
}
