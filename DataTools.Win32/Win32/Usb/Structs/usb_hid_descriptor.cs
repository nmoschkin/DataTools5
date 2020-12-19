// ************************************************* ''
// DataTools C# Native Utility Library For Windows - Interop
//
// Module: UsbHid
//         HID-related structures, enums and functions.
//
// Copyright (C) 2011-2020 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the MIT License   
// ************************************************* ''

using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace DataTools.Win32
{
    [StructLayout(LayoutKind.Sequential, Pack = LibUsb.gPack)]
    public struct usb_hid_descriptor
    {

        /// <summary>
        /// Size of this descriptor in bytes.
        /// </summary>
        /// <remarks>0x09</remarks>
        public byte bLength;

        /// <summary>
        /// HID descriptor type (assigned by USB).
        /// </summary>
        /// <remarks>0x21</remarks>
        public byte bDescriptorType;

        /// <summary>
        /// HID Class Specification release number in binarycoded decimal. For example, 2.10 is 0x210.
        /// </summary>
        /// <remarks>0x100</remarks>
        public short bcdHID;

        /// <summary>
        /// Hardware target country.
        /// </summary>
        /// <remarks>0x00</remarks>
        public byte bCountryCode;

        /// <summary>
        /// Number of HID class descriptors to follow.
        /// </summary>
        /// <remarks>0x01</remarks>
        public byte bNumDescriptors;

        /// <summary>
        /// Report descriptor type.
        /// </summary>
        /// <remarks>0x22</remarks>
        public byte bReportDescriptorType;

        /// <summary>
        /// Total length of Report descriptor.
        /// </summary>
        /// <remarks>0x????</remarks>
        public short wDescriptorLength;
    }
}
