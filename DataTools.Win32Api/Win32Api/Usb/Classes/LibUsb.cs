// ************************************************* ''
// DataTools C# Native Utility Library For Windows - Interop
//
// Module: UsbApi
//         USB-related structures, enums and functions.
//
// Copyright (C) 2011-2020 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the Microsoft Public License   
// ************************************************* ''

using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;
using DataTools.Memory;

namespace DataTools.Win32Api
{
    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    internal static class LibUsb
    {

        /// <summary>
        /// Pack 1 structures for USB.
        /// </summary>
        /// <remarks></remarks>
        public const int gPack = 1;

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public enum USBRESULT : int
        {
            /// <summary>
            /// Operation succeeded
            /// </summary>
            /// <remarks></remarks>
            [Description("Operation succeeded")]
            OK = 0,

            /// <summary>
            /// Operation not permitted
            /// </summary>
            /// <remarks></remarks>
            [Description("Operation not permitted")]
            EPERM = 1,

            /// <summary>
            /// No entry, ENOFILE, no such file or directory
            /// </summary>
            /// <remarks></remarks>
            [Description("No entry, ENOFILE, no such file or directory")]
            ENOENT = 2,

            /// <summary>
            /// No such process
            /// </summary>
            /// <remarks></remarks>
            [Description("No such process")]
            ESRCH = 3,

            /// <summary>
            /// Interrupted function call
            /// </summary>
            /// <remarks></remarks>
            [Description("Interrupted function call")]
            EINTR = 4,

            /// <summary>
            /// Input/output error
            /// </summary>
            /// <remarks></remarks>
            [Description("Input/output error")]
            EIO = 5,

            /// <summary>
            /// No such device or address
            /// </summary>
            /// <remarks></remarks>
            [Description("No such device or address")]
            ENXIO = 6,

            /// <summary>
            /// Arg list too long
            /// </summary>
            /// <remarks></remarks>
            [Description("Arg list too long")]
            E2BIG = 7,

            /// <summary>
            /// Exec format error
            /// </summary>
            /// <remarks></remarks>
            [Description("Exec format error")]
            ENOEXEC = 8,

            /// <summary>
            /// Bad file descriptor
            /// </summary>
            /// <remarks></remarks>
            [Description("Bad file descriptor")]
            EBADF = 9,

            /// <summary>
            /// No child processes
            /// </summary>
            /// <remarks></remarks>
            [Description("No child processes")]
            ECHILD = 10,

            /// <summary>
            /// Resource temporarily unavailable
            /// </summary>
            /// <remarks></remarks>
            [Description("Resource temporarily unavailable")]
            EAGAIN = 11,

            /// <summary>
            /// Not enough space
            /// </summary>
            /// <remarks></remarks>
            [Description("Not enough space")]
            ENOMEM = 12,

            /// <summary>
            /// Permission denied
            /// </summary>
            /// <remarks></remarks>
            [Description("Permission denied")]
            EACCES = 13,

            /// <summary>
            /// Bad address
            /// </summary>
            /// <remarks></remarks>
            [Description("Bad address")]
            EFAULT = 14,

            /// <summary>
            /// strerror reports ""Resource device""
            /// </summary>
            /// <remarks></remarks>
            [Description("strerror reports \"Resource device\"")]
            EBUSY = 16,

            /// <summary>
            /// File exists
            /// </summary>
            /// <remarks></remarks>
            [Description("File exists")]
            EEXIST = 17,

            /// <summary>
            /// Improper link (cross-device link?)
            /// </summary>
            /// <remarks></remarks>
            [Description("Improper link (cross-device link?)")]
            EXDEV = 18,

            /// <summary>
            /// No such device
            /// </summary>
            /// <remarks></remarks>
            [Description("No such device")]
            ENODEV = 19,

            /// <summary>
            /// Not a directory
            /// </summary>
            /// <remarks></remarks>
            [Description("Not a directory")]
            ENOTDIR = 20,

            /// <summary>
            /// Is a directory
            /// </summary>
            /// <remarks></remarks>
            [Description("Is a directory")]
            EISDIR = 21,

            /// <summary>
            /// Invalid argument
            /// </summary>
            /// <remarks></remarks>
            [Description("Invalid argument")]
            EINVAL = 22,

            /// <summary>
            /// Too many open files in system
            /// </summary>
            /// <remarks></remarks>
            [Description("Too many open files in system")]
            ENFILE = 23,

            /// <summary>
            /// Too many open files
            /// </summary>
            /// <remarks></remarks>
            [Description("Too many open files")]
            EMFILE = 24,

            /// <summary>
            /// Inappropriate I/O control operation
            /// </summary>
            /// <remarks></remarks>
            [Description("Inappropriate I/O control operation")]
            ENOTTY = 25,

            /// <summary>
            /// File too large
            /// </summary>
            /// <remarks></remarks>
            [Description("File too large")]
            EFBIG = 27,

            /// <summary>
            /// No space left on device
            /// </summary>
            /// <remarks></remarks>
            [Description("No space left on device")]
            ENOSPC = 28,

            /// <summary>
            /// Invalid seek (seek on a pipe?)
            /// </summary>
            /// <remarks></remarks>
            [Description("Invalid seek (seek on a pipe?)")]
            ESPIPE = 29,

            /// <summary>
            /// Read-only file system
            /// </summary>
            /// <remarks></remarks>
            [Description("Read-only file system")]
            EROFS = 30,

            /// <summary>
            /// Too many links
            /// </summary>
            /// <remarks></remarks>
            [Description("Too many links")]
            EMLINK = 31,

            /// <summary>
            /// Broken pipe
            /// </summary>
            /// <remarks></remarks>
            [Description("Broken pipe")]
            EPIPE = 32,

            /// <summary>
            /// Domain error (math functions)
            /// </summary>
            /// <remarks></remarks>
            [Description("Domain error (math functions)")]
            EDOM = 33,

            /// <summary>
            /// Result too large (possibly too small)
            /// </summary>
            /// <remarks></remarks>
            [Description("Result too large (possibly too small)")]
            ERANGE = 34,

            /// <summary>
            /// Resource deadlock avoided
            /// </summary>
            /// <remarks></remarks>
            [Description("Resource deadlock avoided")]
            EDEADLK = 36,

            /// <summary>
            /// Filename too long
            /// </summary>
            /// <remarks></remarks>
            [Description("Filename too long")]
            ENAMETOOLONG = 38,

            /// <summary>
            /// No locks available
            /// </summary>
            /// <remarks></remarks>
            [Description("No locks available")]
            ENOLCK = 39,

            /// <summary>
            /// Function not implemented
            /// </summary>
            /// <remarks></remarks>
            [Description("Function not implemented")]
            ENOSYS = 40,

            /// <summary>
            /// Directory not empty
            /// </summary>
            /// <remarks></remarks>
            [Description("Directory not empty")]
            ENOTEMPTY = 41
        }

        public static string FormatUsbResult(USBRESULT r)
        {
            var fi = r.GetType().GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (var fe in fi)
            {
                if ((int)(fe.GetValue(r)) == (int)r)
                {
                    DescriptionAttribute attr;
                    attr = (DescriptionAttribute)fe.GetCustomAttribute(typeof(DescriptionAttribute));
                    return attr.Description;
                }
            }

            return null;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        public const int LIBUSB_PATH_MAX = 512;

        //
        // USB spec information
        // 
        // This is all stuff grabbed from various USB specs and is pretty much
        // not subject to change
        //

        //
        // Device and/or Interface Class codes
        //
        public const int USB_CLASS_PER_INTERFACE = 0;  // for DeviceClass ''
        public const int USB_CLASS_AUDIO = 1;
        public const int USB_CLASS_COMM = 2;
        public const int USB_CLASS_HID = 3;
        public const int USB_CLASS_PRINTER = 7;
        public const int USB_CLASS_MASS_STORAGE = 8;
        public const int USB_CLASS_HUB = 9;
        public const int USB_CLASS_DATA = 10;
        public const int USB_CLASS_VENDOR_SPEC = 0xFF;

        //
        // Descriptor types
        //
        public const int USB_DT_DEVICE = 0x1;
        public const int USB_DT_CONFIG = 0x2;
        public const int USB_DT_STRING = 0x3;
        public const int USB_DT_INTERFACE = 0x4;
        public const int USB_DT_ENDPOINT = 0x5;
        public const int USB_DT_HID = 0x21;
        public const int USB_DT_REPORT = 0x22;
        public const int USB_DT_PHYSICAL = 0x23;
        public const int USB_DT_HUB = 0x29;

        //
        // Descriptor sizes per descriptor type
        //
        public const int USB_DT_DEVICE_SIZE = 18;
        public const int USB_DT_CONFIG_SIZE = 9;
        public const int USB_DT_INTERFACE_SIZE = 9;
        public const int USB_DT_ENDPOINT_SIZE = 7;
        public const int USB_DT_ENDPOINT_AUDIO_SIZE = 9;  // Audio extension ''
        public const int USB_DT_HUB_NONVAR_SIZE = 7;

        // ensure byte-packed structures ''

        // All standard descriptors have these 2 fields in common ''
        [StructLayout(LayoutKind.Sequential, Pack = gPack)]
        public struct usb_descriptor_header
        {
            public byte bLength;
            public byte bDescriptorType;
        }

        // String descriptor ''
        [StructLayout(LayoutKind.Sequential, Pack = gPack)]
        public struct usb_string_descriptor
        {
            public byte bLength;
            public byte bDescriptorType;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public ushort[] wData;
        }

        [StructLayout(LayoutKind.Sequential, Pack = gPack)]
        public struct lpusb_string_descriptor
        {
            public MemPtr ptr;

            public byte bLength
            {
                get
                {
                    if (ptr == IntPtr.Zero)
                        return 0;
                    return ptr.ByteAt(0L);
                }
            }

            public byte descriptorType
            {
                get
                {
                    if (ptr == IntPtr.Zero)
                        return 0;
                    return ptr.ByteAt(1L);
                }
            }

            public string data
            {
                get
                {
                    if (ptr == IntPtr.Zero)
                        return null;
                    return System.Text.Encoding.Unicode.GetString(ptr.ToByteArray(2L, bLength));
                }
            }
        }

        //' HID descriptor ''
        // <StructLayout(LayoutKind.Sequential, Pack:=gPack)> _
        // Public Structure usb_hid_descriptor
        // Public bLength As Byte
        // Public bDescriptorType As Byte
        // Public bcdHID As UShort
        // Public bCountryCode As Byte
        // Public bNumDescriptors As Byte
        // End Structure

        // Endpopublic descriptor as integer ''
        public const int USB_MAXENDPOINTS = 32;

        [StructLayout(LayoutKind.Sequential, Pack = gPack)]
        public struct usb_endpoint_descriptor
        {
            public byte bLength;
            public byte bDescriptorType;
            public byte bEndpointAddress;
            public byte bmAttributes;
            public ushort wMaxPacketSize;
            public byte bInterval;
            public byte bRefresh;
            public byte bSynchAddress;
            public IntPtr extra; // Extra descriptors ''
            public int extralen;

            public byte[] extra_desc
            {
                get
                {
                    byte[] extra_descRet = default;
                    MemPtr mm = extra;
                    extra_descRet = mm.ToByteArray(0L, extralen);
                    return extra_descRet;
                }
            }
        }

        public const int USB_ENDPOINT_ADDRESS_MASK = 0xF;    // in bEndpointAddress ''
        public const int USB_ENDPOINT_DIR_MASK = 0x80;
        public const int USB_ENDPOINT_TYPE_MASK = 0x3;    // in bmAttributes ''
        public const int USB_ENDPOINT_TYPE_CONTROL = 0;
        public const int USB_ENDPOINT_TYPE_ISOCHRONOUS = 1;
        public const int USB_ENDPOINT_TYPE_BULK = 2;
        public const int USB_ENDPOINT_TYPE_INTERRUPT = 3;

        // Interface descriptor ''
        public const int USB_MAXINTERFACES = 32;

        [StructLayout(LayoutKind.Sequential, Pack = gPack)]
        public struct usb_interface_descriptor
        {
            public byte bLength;
            public byte bDescriptorType;
            public byte bInterfaceNumber;
            public byte bAlternateSetting;
            public byte bNumEndpoints;
            public byte bInterfaceClass;
            public byte bInterfaceSubClass;
            public byte bInterfaceProtocol;
            public byte iInterface;
            private IntPtr ep; // usb_endpoint_descriptor 'endpopublic Public as integer extra As IntPtr

            public usb_endpoint_descriptor endpoint
            {
                get
                {
                    usb_endpoint_descriptor endpointRet = default;
                    MemPtr mm = ep;
                    endpointRet = mm.ToStruct<usb_endpoint_descriptor>();
                    return endpointRet;
                }
            }

            public IntPtr extra;
            public int extralen;

            public usb_hid_descriptor hidDescriptor
            {
                get
                {
                    SafePtr mm = (SafePtr)extra_desc;
                    return mm.ToStruct<usb_hid_descriptor>();
                }
            }

            public byte[] extra_desc
            {
                get
                {
                    byte[] extra_descRet = default;
                    MemPtr mm = extra;
                    extra_descRet = mm.ToByteArray(0L, extralen);
                    return extra_descRet;
                }
            }
        }

        public const int USB_MAXALTSETTING = 128;    // Hard limit ''

        [StructLayout(LayoutKind.Sequential, Pack = gPack)]
        public struct usb_interface
        {
            public IntPtr altsetting; // usb_interface_descriptor
            public int num_altsetting;

            public usb_interface_descriptor[] altsettings
            {
                get
                {
                    usb_interface_descriptor[] alt;
                    alt = new usb_interface_descriptor[num_altsetting];
                    MemPtr p = altsetting;
                    for (int i = 0, loopTo = num_altsetting - 1; i <= loopTo; i++)
                    {
                        alt[i] = p.ToStruct<usb_interface_descriptor>();
                        p = p + Marshal.SizeOf<usb_interface_descriptor>();
                    }

                    return alt;
                }
            }
        }

        // Configuration descriptor information.. ''
        public const int USB_MAXCONFIG = 8;

        [StructLayout(LayoutKind.Sequential, Pack = gPack)]
        public struct usb_config_descriptor
        {
            public byte bLength;
            public byte bDescriptorType;
            public ushort wTotalLength;
            public byte bNumInterfaces;
            public byte bConfigurationValue;
            public byte iConfiguration;
            public byte bmAttributes;
            public byte MaxPower;
            private IntPtr iface; // usb_interface 'interface

            public usb_interface @interface
            {
                get
                {
                    usb_interface @interfaceRet = default;
                    MemPtr mm = iface;
                    @interfaceRet = mm.ToStruct<usb_interface>();
                    return @interfaceRet;
                }
            }

            public IntPtr extra;  // Extra descriptors ''
            public int extralen;

            public byte[] extra_desc
            {
                get
                {
                    byte[] extra_descRet = default;
                    MemPtr mm = extra;
                    extra_descRet = mm.ToByteArray(0L, extralen);
                    return extra_descRet;
                }
            }
        }

        // Device descriptor ''
        [StructLayout(LayoutKind.Sequential, Pack = gPack)]
        public struct usb_device_descriptor
        {
            public byte bLength;
            public byte bDescriptorType;
            public ushort bcdUSB;
            public byte bDeviceClass;
            public byte bDeviceSubClass;
            public byte bDeviceProtocol;
            public byte bMaxPacketSize0;
            public ushort idVendor;
            public ushort idProduct;
            public ushort bcdDevice;
            public byte iManufacturer;
            public byte iProduct;
            public byte iSerialNumber;
            public byte bNumConfigurations;
        }
        // Device descriptor ''

        [StructLayout(LayoutKind.Sequential, Pack = gPack)]
        public struct usb_device_descriptor_strings
        {
            public string iManufacturer;
            public string iProduct;
            public string iSerialNumber;
        }

        [StructLayout(LayoutKind.Sequential, Pack = gPack)]
        public struct usb_ctrl_setup
        {
            public byte bRequestType;
            public byte bRequest;
            public ushort wValue;
            public ushort wIndex;
            public ushort wLength;
        }

        //
        // Standard requests
        //
        public const int USB_REQ_GET_STATUS = 0x0;
        public const int USB_REQ_CLEAR_FEATURE = 0x1;
        // = &h02 is reserved ''
        public const int USB_REQ_SET_FEATURE = 0x3;
        // = &h04 is reserved ''
        public const int USB_REQ_SET_ADDRESS = 0x5;
        public const int USB_REQ_GET_DESCRIPTOR = 0x6;
        public const int USB_REQ_SET_DESCRIPTOR = 0x7;
        public const int USB_REQ_GET_CONFIGURATION = 0x8;
        public const int USB_REQ_SET_CONFIGURATION = 0x9;
        public const int USB_REQ_GET_INTERFACE = 0xA;
        public const int USB_REQ_SET_INTERFACE = 0xB;
        public const int USB_REQ_SYNCH_FRAME = 0xC;
        public const int USB_TYPE_STANDARD = 0x0 << 5;
        public const int USB_TYPE_CLASS = 0x1 << 5;
        public const int USB_TYPE_VENDOR = 0x2 << 5;
        public const int USB_TYPE_RESERVED = 0x3 << 5;
        public const int USB_RECIP_DEVICE = 0x0;
        public const int USB_RECIP_INTERFACE = 0x1;
        public const int USB_RECIP_ENDPOINT = 0x2;
        public const int USB_RECIP_OTHER = 0x3;

        //
        // Various libusb API related stuff
        //

        public const int USB_ENDPOINT_IN = 0x80;
        public const int USB_ENDPOINT_OUT = 0x0;

        // Error codes ''
        public const int USB_ERROR_BEGIN = 500000;

        //
        // This is supposed to look weird. This file is generated from autoconf
        // and I didn't want to make this too complicated.
        //
        public static ushort USB_LE16_TO_CPU(ushort x)
        {
            return x;
        }

        //
        // Device reset types for usb_reset_ex.
        // http://msdn.microsoft.com/en-us/library/ff537269%28VS.85%29.aspx
        // http://msdn.microsoft.com/en-us/library/ff537243%28v=vs.85%29.aspx
        //
        public const int USB_RESET_TYPE_RESET_PORT = 1 << 0;
        public const int USB_RESET_TYPE_CYCLE_PORT = 1 << 1;
        public const int USB_RESET_TYPE_FULL_RESET = USB_RESET_TYPE_CYCLE_PORT | USB_RESET_TYPE_RESET_PORT;

        // Data types ''
        // public structure usb_device ''
        // public structure usb_bus ''

        [StructLayout(LayoutKind.Sequential, Pack = gPack)]
        public struct usb_device
        {
            public IntPtr next; // usb_device
            public IntPtr prev; // usb_device
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = LIBUSB_PATH_MAX)]
            public string filename;
            public IntPtr bus; // usb_bus
            public usb_device_descriptor descriptor;  // usb_device_descriptor
            public IntPtr config; // usb_config_descriptor 'config
            public IntPtr dev; // void 'dev		'' Darwin support ''
            public byte devnum;
            public byte num_children;
            public IntPtr children;  // usb_device ''children
        }

        [StructLayout(LayoutKind.Sequential, Pack = gPack)]
        public struct lpusb_device
        {
            public MemPtr ptr;

            public lpusb_device next
            {
                get
                {
                    if (ptr == IntPtr.Zero)
                        return default;
                    var lp = new lpusb_device();
                    if (IntPtr.Size == 4)
                    {
                        lp.ptr = new IntPtr(ptr.UIntAt(0L));
                    }
                    else
                    {
                        lp.ptr = new IntPtr(ptr.LongAt(0L));
                    }

                    return lp;
                }
            }

            public lpusb_device prev
            {
                get
                {
                    if (ptr == IntPtr.Zero)
                        return default;
                    var lp = new lpusb_device();
                    if (IntPtr.Size == 4)
                    {
                        lp.ptr = new IntPtr(ptr.UIntAt(1L));
                    }
                    else
                    {
                        lp.ptr = new IntPtr(ptr.LongAt(1L));
                    }

                    return lp;
                }
            }

            public string filename
            {
                get
                {
                    string filenameRet = default;
                    if (ptr == IntPtr.Zero)
                        return null;
                    filenameRet = ptr.GetUTF8String(IntPtr.Size * 2);
                    return filenameRet;
                }
            }

            public lpusb_bus bus
            {
                get
                {
                    lpusb_bus busRet = default;
                    if (ptr == IntPtr.Zero)
                        return default;
                    busRet = new lpusb_bus();
                    int bc = LIBUSB_PATH_MAX + IntPtr.Size * 2;
                    if (IntPtr.Size == 4)
                    {
                        busRet.ptr = new IntPtr(BitConverter.ToUInt32(ptr.ToByteArray(bc, IntPtr.Size), 0));
                    }
                    else
                    {
                        busRet.ptr = new IntPtr(BitConverter.ToInt64(ptr.ToByteArray(bc, IntPtr.Size), 0));
                    }

                    return busRet;
                }
            }

            public usb_device_descriptor descriptor
            {
                get
                {
                    usb_device_descriptor descriptorRet = default;
                    if (ptr == IntPtr.Zero)
                        return default;
                    int bc = LIBUSB_PATH_MAX + IntPtr.Size * 3;
                    descriptorRet = ptr.ToStructAt<usb_device_descriptor>(bc);
                    return descriptorRet;
                }
            }

            public usb_config_descriptor config
            {
                get
                {
                    usb_config_descriptor configRet = default;
                    if (ptr == IntPtr.Zero)
                        return default;
                    int bc = LIBUSB_PATH_MAX + IntPtr.Size * 3 + Marshal.SizeOf<usb_device_descriptor>();
                    var str = new MemPtr();
                    if (IntPtr.Size == 4)
                    {
                        str.Handle = new IntPtr(BitConverter.ToUInt32(ptr.ToByteArray(bc, IntPtr.Size), 0));
                    }
                    else
                    {
                        str.Handle = new IntPtr(BitConverter.ToInt64(ptr.ToByteArray(bc, IntPtr.Size), 0));
                    }

                    if (str.Handle != IntPtr.Zero)
                    {
                        configRet = str.ToStruct<usb_config_descriptor>();
                    }

                    return configRet;
                }
            }

            public IntPtr dev
            {
                get
                {
                    IntPtr devRet = default;
                    if (ptr == IntPtr.Zero)
                        return default;
                    int bc = LIBUSB_PATH_MAX + IntPtr.Size * 4 + Marshal.SizeOf<usb_device_descriptor>();
                    if (IntPtr.Size == 4)
                    {
                        devRet = new IntPtr(BitConverter.ToUInt32(ptr.ToByteArray(bc, IntPtr.Size), 0));
                    }
                    else
                    {
                        devRet = new IntPtr(BitConverter.ToInt64(ptr.ToByteArray(bc, IntPtr.Size), 0));
                    }

                    return devRet;
                }
            }

            public byte devnum
            {
                get
                {
                    if (ptr == IntPtr.Zero)
                        return default;
                    int bc = LIBUSB_PATH_MAX + IntPtr.Size * 5 + Marshal.SizeOf<usb_device_descriptor>();
                    return ptr.ByteAt(bc);
                }
            }

            public byte num_children
            {
                get
                {
                    if (ptr == IntPtr.Zero)
                        return default;
                    int bc = LIBUSB_PATH_MAX + IntPtr.Size * 5 + 1 + Marshal.SizeOf<usb_device_descriptor>();
                    return ptr.ByteAt(bc);
                }
            }

            public lpusb_device children
            {
                get
                {
                    if (ptr == IntPtr.Zero)
                        return default;
                    var lp = new lpusb_device();
                    int bc = LIBUSB_PATH_MAX + IntPtr.Size * 5 + 2 + Marshal.SizeOf<usb_device_descriptor>();
                    MemPtr mm;
                    if (IntPtr.Size == 4)
                    {
                        mm = new IntPtr(BitConverter.ToUInt32(ptr.ToByteArray(bc, IntPtr.Size), 0));
                        if (mm == IntPtr.Zero)
                            return default;
                        lp.ptr = new IntPtr(mm.UIntAt(0L));
                    }
                    else
                    {
                        mm = new IntPtr(BitConverter.ToInt64(ptr.ToByteArray(bc, IntPtr.Size), 0));
                        if (mm == IntPtr.Zero)
                            return default;
                        lp.ptr = new IntPtr(mm.LongAt(0L));
                    }

                    return lp;
                }
            }

            public override string ToString()
            {
                if (ptr == IntPtr.Zero)
                    return "null";
                return filename;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = gPack)]
        public struct usb_bus
        {
            public IntPtr next; // usb_device
            public IntPtr prev; // usb_device
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = LIBUSB_PATH_MAX)]
            public string filename;
            public IntPtr devices; // usb_device 'devices
            public uint location;
            public IntPtr root_dev; // usb_device 'root_dev
        }

        [StructLayout(LayoutKind.Sequential, Pack = gPack)]
        public struct lpusb_bus
        {
            public MemPtr ptr;

            public override string ToString()
            {
                if (ptr == IntPtr.Zero)
                    return "null";
                return filename;
            }

            public lpusb_bus next
            {
                get
                {
                    if (ptr == IntPtr.Zero)
                        return default;
                    var lp = new lpusb_bus();
                    if (IntPtr.Size == 4)
                    {
                        lp.ptr = new IntPtr(ptr.UIntAt(0L));
                    }
                    else
                    {
                        lp.ptr = new IntPtr(ptr.LongAt(0L));
                    }

                    return lp;
                }
            }

            public lpusb_bus prev
            {
                get
                {
                    if (ptr == IntPtr.Zero)
                        return default;
                    var lp = new lpusb_bus();
                    if (IntPtr.Size == 4)
                    {
                        lp.ptr = new IntPtr(ptr.UIntAt(1L));
                    }
                    else
                    {
                        lp.ptr = new IntPtr(ptr.LongAt(1L));
                    }

                    return lp;
                }
            }

            public string filename
            {
                get
                {
                    string filenameRet = default;
                    if (ptr == IntPtr.Zero)
                        return null;
                    filenameRet = ptr.GetUTF8String(IntPtr.Size * 2);
                    return filenameRet;
                }
            }

            public lpusb_device devices
            {
                get
                {
                    if (ptr == IntPtr.Zero)
                        return default;
                    var lp = new lpusb_device();
                    int bc = LIBUSB_PATH_MAX + IntPtr.Size * 2;
                    if (IntPtr.Size == 4)
                    {
                        lp.ptr = new IntPtr(BitConverter.ToUInt32(ptr.ToByteArray(bc, IntPtr.Size), 0));
                    }
                    else
                    {
                        lp.ptr = new IntPtr(BitConverter.ToInt64(ptr.ToByteArray(bc, IntPtr.Size), 0));
                    }

                    return lp;
                }
            }

            public uint location
            {
                get
                {
                    if (ptr == IntPtr.Zero)
                        return default;
                    uint lp;
                    int bc = LIBUSB_PATH_MAX + IntPtr.Size * 3;
                    lp = BitConverter.ToUInt32(ptr.ToByteArray(bc, IntPtr.Size), 0);
                    return lp;
                }
            }

            public lpusb_device root_dev
            {
                get
                {
                    if (ptr == IntPtr.Zero)
                        return default;
                    var lp = new lpusb_device();
                    int bc = LIBUSB_PATH_MAX + IntPtr.Size * 3 + 4;
                    if (IntPtr.Size == 4)
                    {
                        lp.ptr = new IntPtr(BitConverter.ToUInt32(ptr.ToByteArray(bc, IntPtr.Size), 0));
                    }
                    else
                    {
                        lp.ptr = new IntPtr(BitConverter.ToInt64(ptr.ToByteArray(bc, IntPtr.Size), 0));
                    }

                    return lp;
                }
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = gPack)]
        public struct dll
        {
            public int major;
            public int minor;
            public int micro;
            public int nano;
        }

        [StructLayout(LayoutKind.Sequential, Pack = gPack)]
        public struct driver
        {
            public int major;
            public int minor;
            public int micro;
            public int nano;
        }

        // Version information, Windows specific ''

        [StructLayout(LayoutKind.Sequential, Pack = gPack)]
        public struct usb_version
        {
            public dll dll;
            public driver driver;
        }

        [StructLayout(LayoutKind.Sequential, Pack = gPack)]
        public struct usb_dev_handle
        {
            public IntPtr ptr;
        }

        // Function prototypes ''

        // Public Declare Function usb_open Lib "libusb0.dll" (dev As lpusb_device) As IntPtr
        // Public Declare Function usb_close Lib "libusb0.dll" (dev As IntPtr) As USBRESULT

        // Public Declare Function usb_get_string Lib "libusb0.dll" ( _
        // dev As IntPtr, _
        // index As Integer, _
        // langid As Integer, _
        // <MarshalAs(UnmanagedType.LPStr)> _
        // buf As String, _
        // buflen As Integer) As USBRESULT

        // Public Declare Function usb_get_string_simple Lib "libusb0.dll" ( _
        // dev As IntPtr, _
        // index As Integer, _
        // buf As MemPtr, _
        // buflen As Integer) As USBRESULT

        // Public Declare Function usb_get_descriptor_by_endpoint Lib "libusb0.dll" ( _
        // udev As IntPtr, _
        // ep As Integer, _
        // type As Byte, _
        // index As Byte, _
        // buf As IntPtr, _
        // size As Integer) As USBRESULT

        // Public Declare Function usb_get_descriptor Lib "libusb0.dll" ( _
        // udev As IntPtr, _
        // type As Byte, _
        // index As Byte, _
        // buf As IntPtr, _
        // size As Integer) As USBRESULT

        // Public Declare Function usb_bulk_write Lib "libusb0.dll" ( _
        // udev As IntPtr, _
        // ep As Integer, _
        // bytes As IntPtr, _
        // size As Integer, _
        // timeout As Integer) As USBRESULT

        // Public Declare Function usb_bulk_read Lib "libusb0.dll" ( _
        // udev As IntPtr, _
        // ep As Integer, _
        // bytes As IntPtr, _
        // size As Integer, _
        // timeout As Integer) As USBRESULT

        // Public Declare Function usb_interrupt_write Lib "libusb0.dll" ( _
        // udev As IntPtr, _
        // ep As Integer, _
        // bytes As IntPtr, _
        // size As Integer, _
        // timeout As Integer) As USBRESULT

        // Public Declare Function usb_interrupt_read Lib "libusb0.dll" ( _
        // udev As IntPtr, _
        // ep As Integer, _
        // bytes As IntPtr, _
        // size As Integer, _
        // timeout As Integer) As USBRESULT

        // Public Declare Function usb_control_msg Lib "libusb0.dll" ( _
        // dev As IntPtr, _
        // requesttype As Integer, _
        // request As Integer, _
        // value As Integer, _
        // index As Integer, _
        // bytes As IntPtr, _
        // size As Integer, _
        // timeout As Integer) As USBRESULT

        // Public Declare Function usb_set_configuration Lib "libusb0.dll" ( _
        // dev As IntPtr, _
        // configuration As Integer) As USBRESULT

        // Public Declare Function usb_claim_interface Lib "libusb0.dll" ( _
        // dev As IntPtr, _
        // [interface] As Integer) As USBRESULT

        // Public Declare Function usb_release_interface Lib "libusb0.dll" ( _
        // dev As IntPtr, _
        // [interface] As Integer) As USBRESULT

        // Public Declare Function usb_set_altinterface Lib "libusb0.dll" ( _
        // dev As IntPtr, _
        // alternate As Integer) As USBRESULT

        // Public Declare Function usb_resetep Lib "libusb0.dll" ( _
        // dev As IntPtr, _
        // ep As Integer) As USBRESULT

        // Public Declare Function usb_clear_halt Lib "libusb0.dll" ( _
        // dev As IntPtr, _
        // ep As Integer) As USBRESULT

        // Public Declare Function usb_reset Lib "libusb0.dll" (dev As IntPtr) As USBRESULT

        // Public Declare Function usb_reset_ex Lib "libusb0.dll" ( _
        // dev As IntPtr, _
        // reset_type As UInteger) As USBRESULT

        // Public Declare Function usb_strerror Lib "libusb0.dll" () As <MarshalAs(UnmanagedType.LPStr)> String

        // Public Declare Sub usb_init Lib "libusb0.dll" ()

        // Public Declare Sub usb_set_debug Lib "libusb0.dll" (level As Integer)

        // Public Declare Function usb_find_busses Lib "libusb0.dll" () As Integer

        // Public Declare Function usb_find_devices Lib "libusb0.dll" () As Integer

        //' usb_devices
        // Public Declare Function get_usb_device Lib "libusb0.dll" Alias "usb_device" (dev As IntPtr) As IntPtr

        //' usb_busses
        // Public Declare Function usb_get_busses Lib "libusb0.dll" () As IntPtr

        //' Windows specific functions
        // Public Declare Function usb_install_service_np Lib "libusb0.dll" () As USBRESULT

        // <DllImport("libusb0.dll", CallingConvention:=CallingConvention.Winapi, CharSet:=CharSet.Ansi)>
        // Public Sub usb_install_service_np_rundll(hwnd As IntPtr, instance As IntPtr, <MarshalAs(UnmanagedType.LPStr)> cmd_line As String, cmd_show As Integer)
        // End Sub

        // Public Declare Function usb_uninstall_service_np Lib "libusb0.dll" () As USBRESULT

        // <DllImport("libusb0.dll", CallingConvention:=CallingConvention.Winapi, CharSet:=CharSet.Ansi)>
        // Public Sub usb_uninstall_service_np_rundll(hwnd As IntPtr, instance As IntPtr, <MarshalAs(UnmanagedType.LPStr)> cmd_line As String, cmd_show As Integer)
        // End Sub

        // Public Declare Function usb_install_driver_np Lib "libusb0.dll" (<MarshalAs(UnmanagedType.LPStr)> inf_file As String) As USBRESULT

        // <DllImport("libusb0.dll", CallingConvention:=CallingConvention.Winapi, CharSet:=CharSet.Ansi)>
        // Public Sub usb_install_driver_np_rundll(hwnd As IntPtr, instance As IntPtr, <MarshalAs(UnmanagedType.LPStr)> cmd_line As String, cmd_show As Integer)
        // End Sub

        // Public Declare Function usb_touch_inf_file_np Lib "libusb0.dll" (<MarshalAs(UnmanagedType.LPStr)> inf_file As String) As USBRESULT

        // <DllImport("libusb0.dll", CallingConvention:=CallingConvention.Winapi, CharSet:=CharSet.Ansi)>
        // Public Sub usb_touch_inf_file_np_rundll(hwnd As IntPtr, instance As IntPtr, <MarshalAs(UnmanagedType.LPStr)> cmd_line As String, cmd_show As Integer)
        // End Sub

        // Public Declare Function usb_install_needs_restart_np Lib "libusb0.dll" () As USBRESULT

        // Public Declare Unicode Function usb_install_npW Lib "libusb0.dll" ( _
        // hwnd As IntPtr, _
        // instance As IntPtr, _
        // <MarshalAs(UnmanagedType.LPWStr)> _
        // cmd_line As String, _
        // starg_arg As Integer) As USBRESULT

        // Public Declare Ansi Function usb_install_npA Lib "libusb0.dll" ( _
        // hwnd As IntPtr, _
        // instance As IntPtr, _
        // <MarshalAs(UnmanagedType.LPStr)> _
        // cmd_line As String, _
        // starg_arg As Integer) As USBRESULT

        // Public Declare Unicode Function usb_install_np Lib "libusb0.dll" _
        // Alias "usb_install_npW" ( _
        // hwnd As IntPtr, _
        // instance As IntPtr, _
        // <MarshalAs(UnmanagedType.LPWStr)> _
        // cmd_line As String, _
        // starg_arg As Integer) As USBRESULT

        //' usb_version
        // Public Declare Function usb_get_version Lib "libusb0.dll" () As IntPtr

        // Public Declare Function usb_isochronous_setup_async Lib "libusb0.dll" ( _
        // dev As IntPtr, _
        // ByRef context As IntPtr, _
        // ep As Byte, _
        // pktsize As Integer) As USBRESULT

        // Public Declare Function usb_bulk_setup_async Lib "libusb0.dll" ( _
        // dev As IntPtr, _
        // ByRef context As IntPtr, _
        // ep As Byte) As USBRESULT

        // Public Declare Function usb_interrupt_setup_async Lib "libusb0.dll" ( _
        // dev As IntPtr, _
        // ByRef context As IntPtr, _
        // ep As Byte) As USBRESULT

        // Public Declare Function usb_submit_async Lib "libusb0.dll" ( _
        // context As IntPtr, _
        // bytes As IntPtr, _
        // size As Integer) As USBRESULT

        // Public Declare Function usb_reap_async Lib "libusb0.dll" ( _
        // context As IntPtr, _
        // timeout As Integer) As USBRESULT
        // Public Declare Function usb_reap_async_nocancel Lib "libusb0.dll" ( _
        // context As IntPtr, _
        // timeout As Integer) As USBRESULT

        // Public Declare Function usb_cancel_async Lib "libusb0.dll" (context As IntPtr) As USBRESULT

        // Public Declare Function usb_free_async Lib "libusb0.dll" (context As IntPtr) As USBRESULT

    }
}
