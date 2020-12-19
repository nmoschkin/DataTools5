// ************************************************* ''
// DataTools C# Native Utility Library For Windows - Interop
//
// Module: UsbApi
//         USB-related structures, enums and functions.
//
// Copyright (C) 2011-2020 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the MIT License   
// ************************************************* ''

using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;


namespace DataTools.Win32
{
    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    internal static class UsbLibHelpers
    {
        public static LibUsb.usb_bus[] busses;
        public static LibUsb.usb_device[] devices;

        [DllImport("hid.dll")]
        public static extern bool HidD_GetProductString(IntPtr HidDeviceObject, IntPtr Buffer, int BufferLength);
        [DllImport("hid.dll")]
        public static extern bool HidD_GetInputReport(IntPtr HidDeviceObject, IntPtr ReportBuffer, int ReportBufferLength);
        [DllImport("hid.dll")]
        public static extern bool HidD_GetFeature(IntPtr HidDeviceObject, IntPtr Buffer, int BufferLength);
        [DllImport("hid.dll", EntryPoint = "HidD_GetFeature")]
        public static extern bool HidD_GetFeatureL(IntPtr HidDeviceObject, ref long Buffer, int BufferLength);
        [DllImport("hid.dll")]
        public static extern bool HidD_SetFeature(IntPtr HidDeviceObject, IntPtr Buffer, int BufferLength);
        [DllImport("hid.dll")]
        public static extern bool HidD_GetManufacturerString(IntPtr HidDeviceObject, IntPtr Buffer, int BufferLength);
        [DllImport("hid.dll")]
        public static extern bool HidD_GetSerialNumberString(IntPtr HidDeviceObject, IntPtr Buffer, int BufferLength);
        [DllImport("hid.dll")]
        public static extern bool HidD_GetPhysicalDescriptor(IntPtr HidDeviceObject, IntPtr Buffer, int BufferLength);
        [DllImport("hid.dll")]
        public static extern bool HidD_GetPreparsedData(IntPtr HidDeviceObject, ref IntPtr PreparsedData);
        [DllImport("hid.dll")]
        public static extern bool HidD_FreePreparsedData(IntPtr PreparsedData);
    }
}
