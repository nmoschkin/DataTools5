// ************************************************* ''
// DataTools C# Native Utility Library For Windows - Interop
//
// Module: Miscellaneous enums to support devices.
// 
// Copyright (C) 2011-2020 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the Microsoft Public License   
// ************************************************* ''


using System;
using System.ComponentModel;
using DataTools.Text;
using DataTools.Win32Api;

namespace DataTools.Win32Api
{
    public static class DevEnumHelpers
    {


        /// <summary>
        /// Return a device interface enum value based on a DEVINTERFACE GUID.
        /// </summary>
        /// <param name="devInterface">The device interface to translate.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        internal static DeviceInterfaceClassEnum GetDevInterfaceClassEnumFromGuid(Guid devInterface)
        {
            var i = default(int);
            if (devInterface == DevProp.GUID_DEVINTERFACE_BRIGHTNESS)
                return (DeviceInterfaceClassEnum)(int)(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_DISPLAY_ADAPTER)
                return (DeviceInterfaceClassEnum)(int)(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_I2C)
                return (DeviceInterfaceClassEnum)(int)(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_IMAGE)
                return (DeviceInterfaceClassEnum)(int)(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_MONITOR)
                return (DeviceInterfaceClassEnum)(int)(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_OPM)
                return (DeviceInterfaceClassEnum)(int)(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_HID)
                return (DeviceInterfaceClassEnum)(int)(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_KEYBOARD)
                return (DeviceInterfaceClassEnum)(int)(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_MOUSE)
                return (DeviceInterfaceClassEnum)(int)(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_MODEM)
                return (DeviceInterfaceClassEnum)(int)(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_NET)
                return (DeviceInterfaceClassEnum)(int)(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_SENSOR)
                return (DeviceInterfaceClassEnum)(int)(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_COMPORT)
                return (DeviceInterfaceClassEnum)(int)(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_PARALLEL)
                return (DeviceInterfaceClassEnum)(int)(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_PARCLASS)
                return (DeviceInterfaceClassEnum)(int)(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_SERENUM_BUS_ENUMERATOR)
                return (DeviceInterfaceClassEnum)(int)(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_CDCHANGER)
                return (DeviceInterfaceClassEnum)(int)(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_CDROM)
                return (DeviceInterfaceClassEnum)(int)(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_DISK)
                return (DeviceInterfaceClassEnum)(int)(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_FLOPPY)
                return (DeviceInterfaceClassEnum)(int)(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_MEDIUMCHANGER)
                return (DeviceInterfaceClassEnum)(int)(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_PARTITION)
                return (DeviceInterfaceClassEnum)(int)(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_STORAGEPORT)
                return (DeviceInterfaceClassEnum)(int)(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_TAPE)
                return (DeviceInterfaceClassEnum)(int)(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_VOLUME)
                return (DeviceInterfaceClassEnum)(int)(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_WRITEONCEDISK)
                return (DeviceInterfaceClassEnum)(int)(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_USB_DEVICE)
                return (DeviceInterfaceClassEnum)(int)(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_USB_HOST_CONTROLLER)
                return (DeviceInterfaceClassEnum)(int)(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_USB_HUB)
                return (DeviceInterfaceClassEnum)(int)(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_WPD)
                return (DeviceInterfaceClassEnum)(int)(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_WPD_PRIVATE)
                return (DeviceInterfaceClassEnum)(int)(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_SIDESHOW)
                return (DeviceInterfaceClassEnum)(int)(i);
            return DeviceInterfaceClassEnum.Unknown;
        }

        /// <summary>
        /// Return a device class enum value based on a DEVCLASS GUID.
        /// </summary>
        /// <param name="devClass">The device class to translate.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        internal static DeviceClassEnum GetDevClassEnumFromGuid(Guid devClass)
        {
            int i = 0x200;

            // classes
            if (devClass == DevProp.GUID_DEVCLASS_1394)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_1394DEBUG)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_61883)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_ADAPTER)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_APMSUPPORT)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_AVC)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_BATTERY)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_BIOMETRIC)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_BLUETOOTH)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_CDROM)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_COMPUTER)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_DECODER)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_DISKDRIVE)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_DISPLAY)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_DOT4)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_DOT4PRINT)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_ENUM1394)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_FDC)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_FLOPPYDISK)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_GPS)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_HDC)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_HIDCLASS)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_IMAGE)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_INFINIBAND)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_INFRARED)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_KEYBOARD)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_LEGACYDRIVER)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_MEDIA)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_MEDIUM_CHANGER)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_MEMORY)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_MODEM)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_MONITOR)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_MOUSE)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_MTD)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_MULTIFUNCTION)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_MULTIPORTSERIAL)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_NET)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_NETCLIENT)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_NETSERVICE)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_NETTRANS)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_NODRIVER)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_PCMCIA)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_PNPPRINTERS)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_PORTS)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_PRINTER_QUEUE)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_PRINTER)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_PRINTERUPGRADE)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_PROCESSOR)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_SBP2)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_SCSIADAPTER)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_SECURITYACCELERATOR)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_SENSOR)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_SIDESHOW)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_SMARTCARDREADER)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_SOUND)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_SYSTEM)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_TAPEDRIVE)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_UNKNOWN)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_USB)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_VOLUME)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_VOLUMESNAPSHOT)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_WCEUSBS)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_WPD)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_EHSTORAGESILO)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_FIRMWARE)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_EXTENSION)
                return (DeviceClassEnum)(int)(i);
            i += 1;
            if (devClass == new Guid("1ed2bbf9-11f0-4084-b21f-ad83a8e6dcdc"))
                return DeviceClassEnum.Printer;
            return DeviceClassEnum.Unknown;
        }

    }
}
