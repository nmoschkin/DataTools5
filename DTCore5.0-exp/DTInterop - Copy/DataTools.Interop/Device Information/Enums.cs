// ' ************************************************* ''
// ' DataTools Visual Basic Utility Library - Interop
// '
// ' Module: Miscellaneous enums to support devices.
// ' 
// ' Copyright (C) 2011-2020 Nathan Moschkin
// ' All Rights Reserved
// '
// ' Licensed Under the Microsoft Public License   
// ' ************************************************* ''


using System;
using System.ComponentModel;
using CoreCT.Text;
using DataTools.Interop.Native;
using Microsoft.VisualBasic.CompilerServices;

namespace DataTools.Interop
{
    public static class DevClassPresenting
    {

        /// <summary>
    /// System device removal policy.
    /// </summary>
        public enum DeviceRemovalPolicy
        {

            /// <summary>
        /// The device is not expected to be removed.
        /// </summary>
        /// <remarks></remarks>
            ExpectNoRemoval = 1,

            /// <summary>
        /// The device can be expected to be removed in an orderly fashion.
        /// </summary>
        /// <remarks></remarks>
            ExpectOrderlyRemoval = 2,

            /// <summary>
        /// The device can be expected to be removed without any preparation for removal.
        /// </summary>
        /// <remarks></remarks>
            ExpectSurpriseRemoval = 3
        }

        /// <summary>
    /// System device characteristics.
    /// </summary>
        public enum DeviceCharacteristcs
        {
            /// <summary>
        /// Beep
        /// </summary>
            [Description("Beep")]
            Beep = 0x1,

            /// <summary>
        /// Cdrom
        /// </summary>
            [Description("Cdrom")]
            Cdrom = 0x2,

            /// <summary>
        /// Cdfs
        /// </summary>
            [Description("Cdfs")]
            Cdfs = 0x3,

            /// <summary>
        /// Controller
        /// </summary>
            [Description("Controller")]
            Controller = 0x4,

            /// <summary>
        /// Datalink
        /// </summary>
            [Description("Datalink")]
            Datalink = 0x5,

            /// <summary>
        /// Dfs
        /// </summary>
            [Description("Dfs")]
            Dfs = 0x6,

            /// <summary>
        /// Disk
        /// </summary>
            [Description("Disk")]
            Disk = 0x7,

            /// <summary>
        /// Disk File System
        /// </summary>
            [Description("Disk File System")]
            DiskFileSystem = 0x8,

            /// <summary>
        /// File System
        /// </summary>
            [Description("File System")]
            FileSystem = 0x9,

            /// <summary>
        /// Inport Port
        /// </summary>
            [Description("Inport Port")]
            InportPort = 0xA,

            /// <summary>
        /// Keyboard
        /// </summary>
            [Description("Keyboard")]
            Keyboard = 0xB,

            /// <summary>
        /// Mailslot
        /// </summary>
            [Description("Mailslot")]
            Mailslot = 0xC,

            /// <summary>
        /// Midi In
        /// </summary>
            [Description("Midi In")]
            MidiIn = 0xD,

            /// <summary>
        /// Midi Out
        /// </summary>
            [Description("Midi Out")]
            MidiOut = 0xE,

            /// <summary>
        /// Mouse
        /// </summary>
            [Description("Mouse")]
            Mouse = 0xF,

            /// <summary>
        /// Multi Unc Provider
        /// </summary>
            [Description("Multi Unc Provider")]
            MultiUncProvider = 0x10,

            /// <summary>
        /// Named Pipe
        /// </summary>
            [Description("Named Pipe")]
            NamedPipe = 0x11,

            /// <summary>
        /// Network
        /// </summary>
            [Description("Network")]
            Network = 0x12,

            /// <summary>
        /// Network Browser
        /// </summary>
            [Description("Network Browser")]
            NetworkBrowser = 0x13,

            /// <summary>
        /// Network File System
        /// </summary>
            [Description("Network File System")]
            NetworkFileSystem = 0x14,

            /// <summary>
        /// Null
        /// </summary>
            [Description("Null")]
            Null = 0x15,

            /// <summary>
        /// Parallel Port
        /// </summary>
            [Description("Parallel Port")]
            ParallelPort = 0x16,

            /// <summary>
        /// Physical Netcard
        /// </summary>
            [Description("Physical Netcard")]
            PhysicalNetcard = 0x17,

            /// <summary>
        /// Printer
        /// </summary>
            [Description("Printer")]
            Printer = 0x18,

            /// <summary>
        /// Scanner
        /// </summary>
            [Description("Scanner")]
            Scanner = 0x19,

            /// <summary>
        /// Serial Mouse Port
        /// </summary>
            [Description("Serial Mouse Port")]
            SerialMousePort = 0x1A,

            /// <summary>
        /// Serial Port
        /// </summary>
            [Description("Serial Port")]
            SerialPort = 0x1B,

            /// <summary>
        /// Screen
        /// </summary>
            [Description("Screen")]
            Screen = 0x1C,

            /// <summary>
        /// Sound
        /// </summary>
            [Description("Sound")]
            Sound = 0x1D,

            /// <summary>
        /// Streams
        /// </summary>
            [Description("Streams")]
            Streams = 0x1E,

            /// <summary>
        /// Tape
        /// </summary>
            [Description("Tape")]
            Tape = 0x1F,

            /// <summary>
        /// Tape File System
        /// </summary>
            [Description("Tape File System")]
            TapeFileSystem = 0x20,

            /// <summary>
        /// Transport
        /// </summary>
            [Description("Transport")]
            Transport = 0x21,

            /// <summary>
        /// Unknown
        /// </summary>
            [Description("Unknown")]
            Unknown = 0x22,

            /// <summary>
        /// Video
        /// </summary>
            [Description("Video")]
            Video = 0x23,

            /// <summary>
        /// Virtual Disk
        /// </summary>
            [Description("Virtual Disk")]
            VirtualDisk = 0x24,

            /// <summary>
        /// Wave In
        /// </summary>
            [Description("Wave In")]
            WaveIn = 0x25,

            /// <summary>
        /// Wave Out
        /// </summary>
            [Description("Wave Out")]
            WaveOut = 0x26,

            /// <summary>
        /// P8042 Port
        /// </summary>
            [Description("P8042 Port")]
            P8042Port = 0x27,

            /// <summary>
        /// Network Redirector
        /// </summary>
            [Description("Network Redirector")]
            NetworkRedirector = 0x28,

            /// <summary>
        /// Battery
        /// </summary>
            [Description("Battery")]
            Battery = 0x29,

            /// <summary>
        /// Bus Extender
        /// </summary>
            [Description("Bus Extender")]
            BusExtender = 0x2A,

            /// <summary>
        /// Modem
        /// </summary>
            [Description("Modem")]
            Modem = 0x2B,

            /// <summary>
        /// Vdm
        /// </summary>
            [Description("Vdm")]
            Vdm = 0x2C,

            /// <summary>
        /// Mass Storage
        /// </summary>
            [Description("Mass Storage")]
            MassStorage = 0x2D,

            /// <summary>
        /// Smb
        /// </summary>
            [Description("Smb")]
            Smb = 0x2E,

            /// <summary>
        /// Ks
        /// </summary>
            [Description("Ks")]
            Ks = 0x2F,

            /// <summary>
        /// Changer
        /// </summary>
            [Description("Changer")]
            Changer = 0x30,

            /// <summary>
        /// Smartcard
        /// </summary>
            [Description("Smartcard")]
            Smartcard = 0x31,

            /// <summary>
        /// Acpi
        /// </summary>
            [Description("Acpi")]
            Acpi = 0x32,

            /// <summary>
        /// Dvd
        /// </summary>
            [Description("Dvd")]
            Dvd = 0x33,

            /// <summary>
        /// Fullscreen Video
        /// </summary>
            [Description("Fullscreen Video")]
            FullscreenVideo = 0x34,

            /// <summary>
        /// Dfs File System
        /// </summary>
            [Description("Dfs File System")]
            DfsFileSystem = 0x35,

            /// <summary>
        /// Dfs Volume
        /// </summary>
            [Description("Dfs Volume")]
            DfsVolume = 0x36,

            /// <summary>
        /// Serenum
        /// </summary>
            [Description("Serenum")]
            Serenum = 0x37,

            /// <summary>
        /// Termsrv
        /// </summary>
            [Description("Termsrv")]
            Termsrv = 0x38,

            /// <summary>
        /// Ksec
        /// </summary>
            [Description("Ksec")]
            Ksec = 0x39,

            /// <summary>
        /// Fips
        /// </summary>
            [Description("Fips")]
            Fips = 0x3A,

            /// <summary>
        /// Infiniband
        /// </summary>
            [Description("Infiniband")]
            Infiniband = 0x3B,

            /// <summary>
        /// Vmbus
        /// </summary>
            [Description("Vmbus")]
            Vmbus = 0x3E,

            /// <summary>
        /// Crypt Provider
        /// </summary>
            [Description("Crypt Provider")]
            CryptProvider = 0x3F,

            /// <summary>
        /// Wpd
        /// </summary>
            [Description("Wpd")]
            Wpd = 0x40,

            /// <summary>
        /// Bluetooth
        /// </summary>
            [Description("Bluetooth")]
            Bluetooth = 0x41,

            /// <summary>
        /// Mt Composite
        /// </summary>
            [Description("Mt Composite")]
            MtComposite = 0x42,

            /// <summary>
        /// Mt Transport
        /// </summary>
            [Description("Mt Transport")]
            MtTransport = 0x43,

            /// <summary>
        /// Biometric
        /// </summary>
            [Description("Biometric Device")]
            Biometric = 0x44,

            /// <summary>
        /// Pmi
        /// </summary>
            [Description("Pmi")]
            Pmi = 0x45,

            /// <summary>
        /// Ehstor
        /// </summary>
            [Description("Storage Silo Enhanced Storage")]
            Ehstor = 0x46,

            /// <summary>
        /// Devapi
        /// </summary>
            [Description("Devapi")]
            Devapi = 0x47,

            /// <summary>
        /// Gpio
        /// </summary>
            [Description("Gpio")]
            Gpio = 0x48,

            /// <summary>
        /// Usbex
        /// </summary>
            [Description("Usbex")]
            Usbex = 0x49,

            /// <summary>
        /// Console
        /// </summary>
            [Description("Console")]
            Console = 0x50,

            /// <summary>
        /// Nfp
        /// </summary>
            [Description("Nfp")]
            Nfp = 0x51,

            /// <summary>
        /// Sysenv
        /// </summary>
            [Description("Sysenv")]
            Sysenv = 0x52,

            /// <summary>
        /// Virtual Block
        /// </summary>
            [Description("Virtual Block")]
            VirtualBlock = 0x53,

            /// <summary>
        /// Point Of Service
        /// </summary>
            [Description("Point Of Service")]
            PointOfService = 0x54
        }


        /// <summary>
    /// Device interface classes.
    /// </summary>
    /// <remarks></remarks>
        public enum DeviceInterfaceClassEnum
        {

            /// <summary>
        /// Monitor brightness control.
        /// </summary>
            [Description("Monitor Brightness Control")]
            Brightness,

            /// <summary>
        /// Display adapter.
        /// </summary>
            [Description("Display Adapter")]
            DisplayAdapter,

            /// <summary>
        /// Display adapter driver that communicates with child devices over the I2C bus.
        /// </summary>
        /// <remarks></remarks>
            [Description("Display adapter driver that communicates with child devices over the I2C bus.")]
            I2C,

            /// <summary>
        /// Digital camera and scanner devices.
        /// </summary>
            [Description("Digital Camera or Scanner Device")]
            ImagingDevice,

            /// <summary>
        /// Computer display monitors.
        /// </summary>
            [Description("Monitor")]
            Monitor,

            /// <summary>
        /// Output Protection Manager (OPM) device driver interface for video signals copy protection.
        /// </summary>
            [Description("Output Protection Manager (OPM) device driver interface for video signals copy protection.")]
            OPM,

            /// <summary>
        /// Human Interface Devices.
        /// </summary>
            [Description("Human Interface Device")]
            HID,

            /// <summary>
        /// Keyboards.
        /// </summary>
            [Description("Keyboard")]
            Keyboard,

            /// <summary>
        /// Mice.
        /// </summary>
            [Description("Mouse")]
            Mouse,

            /// <summary>
        /// Modems.
        /// </summary>
            [Description("Modem")]
            Modem,

            /// <summary>
        /// Network adapters.
        /// </summary>
            [Description("Network Adapter")]
            Network,

            /// <summary>
        /// Sensors.
        /// </summary>
            [Description("Sensor")]
            Sensor,

            /// <summary>
        /// COM port.
        /// </summary>
            [Description("COM Port")]
            comPort,

            /// <summary>
        /// LPT port.
        /// </summary>
            [Description("Parallel Port")]
            ParallelPort,

            /// <summary>
        /// LPT device.
        /// </summary>
            [Description("Parallel Device")]
            ParallelDevice,

            /// <summary>
        /// Bus Enumerator for Plug'n'Play serial ports.
        /// </summary>
            [Description("Bus Enumerator for Plug'n'Play Serial Ports")]
            SerialBusEnum,

            /// <summary>
        /// optical media changing device.
        /// </summary>
            [Description("optical Media Changing Device")]
            CDChanger,

            /// <summary>
        /// Optical device.
        /// </summary>
            [Description("Optical Device")]
            CDROM,

            /// <summary>
        /// Disk device.
        /// </summary>
            [Description("Disk Device")]
            Disk,

            /// <summary>
        /// Floppy disk device.
        /// </summary>
            [Description("Floppy Disk Device")]
            Floppy,

            /// <summary>
        /// Medium changing device.
        /// </summary>
            [Description("Medium changing device")]
            MediumChanger,

            /// <summary>
        /// Disk partition.
        /// </summary>
            [Description("Disk partition")]
            Partition,

            /// <summary>
        /// SCSI/ATA/StorPort Device.
        /// </summary>
        /// <remarks></remarks>
            [Description("SCSI/ATA/StorPort Device")]
            StoragePort,

            /// <summary>
        /// Tape backup device.
        /// </summary>
            [Description("Tape backup device")]
            Tape,

            /// <summary>
        /// Logical volume.
        /// </summary>
            [Description("Logical volume")]
            Volume,

            /// <summary>
        /// Write once disk.
        /// </summary>
            [Description("Write once disk")]
            WriteOnceDisk,

            /// <summary>
        /// USB host controller.
        /// </summary>
            [Description("USB host controller")]
            UsbHostController,

            /// <summary>
        /// USB Hub
        /// </summary>
            [Description("USB Hub")]
            UsbHub,

            /// <summary>
        /// Windows Portable Device
        /// </summary>
            [Description("Windows Portable Device")]
            Wpd,

            /// <summary>
        /// Specialized Windows Portable Device
        /// </summary>
            [Description("Specialized Windows Portable Device")]
            WpdSpecialized,

            /// <summary>
        /// Windows SideShow Device
        /// </summary>
            [Description("Windows SideShow Device")]
            SideShow,
            Unknown = -1
        }

        /// <summary>
    /// Device classes.
    /// </summary>
    /// <remarks></remarks>
        public enum DeviceClassEnum
        {

            /// <summary>
        /// Bus 1394
        /// </summary>
            [Description("IEEE 1394 Isosynchronous Data Transfer Protocol (FireWire)")]
            Bus1394 = 0x200,

            /// <summary>
        /// Bus 1394 Debug
        /// </summary>
            [Description("IEEE 1394 (FireWire) Debug Mode")]
            Bus1394Debug,

            /// <summary>
        /// Iec 61883
        /// </summary>
            [Description("IEC 61883 Consumer Audio/Video Equipment - Digital Interface")]
            Iec61883,

            /// <summary>
        /// Adapter
        /// </summary>
            [Description("Adapter")]
            Adapter,

            /// <summary>
        /// Apmsupport
        /// </summary>
            [Description("Advanced Power Management")]
            Apmsupport,

            /// <summary>
        /// Avc
        /// </summary>
            [Description("H.264/MPEG-4 Part 10 Advanced Video Coding")]
            Avc,

            /// <summary>
        /// Battery
        /// </summary>
            [Description("UPS Battery")]
            Battery,

            /// <summary>
        /// Biometric
        /// </summary>
            [Description("Biometric Feedback")]
            Biometric,

            /// <summary>
        /// Bluetooth
        /// </summary>
            [Description("Bluetooth")]
            Bluetooth,

            /// <summary>
        /// Cd Rom
        /// </summary>
            [Description("Cd Rom")]
            CdRom,

            /// <summary>
        /// Computer
        /// </summary>
            [Description("Computer")]
            Computer,

            /// <summary>
        /// Decoder
        /// </summary>
            [Description("Decoder")]
            Decoder,

            /// <summary>
        /// Disk Drive
        /// </summary>
            [Description("Disk Drive")]
            DiskDrive,

            /// <summary>
        /// Display
        /// </summary>
            [Description("Display")]
            Display,

            /// <summary>
        /// Dot4
        /// </summary>
            [Description("Dot4")]
            Dot4,

            /// <summary>
        /// Dot4 Print
        /// </summary>
            [Description("Dot4 Print")]
            Dot4Print,

            /// <summary>
        /// Enum 1394
        /// </summary>
            [Description("IEEE 1394 FireWire Enumerator")]
            Enum1394,

            /// <summary>
        /// Fdc
        /// </summary>
            [Description("Floppy Disk Controller")]
            Fdc,

            /// <summary>
        /// Floppy Disk
        /// </summary>
            [Description("Floppy Disk")]
            FloppyDisk,

            /// <summary>
        /// Gps
        /// </summary>
            [Description("Global Positioning Device")]
            Gps,

            /// <summary>
        /// Hdc
        /// </summary>
            [Description("Hard Disk Controller")]
            Hdc,

            /// <summary>
        /// Hid Class
        /// </summary>
            [Description("Human Interface Device")]
            HidClass,

            /// <summary>
        /// Image
        /// </summary>
            [Description("Imaging Device")]
            Image,

            /// <summary>
        /// Infini Band
        /// </summary>
            [Description("InfiniBand Adapter")]
            InfiniBand,

            /// <summary>
        /// Infrared
        /// </summary>
            [Description("Infrared Sensor")]
            Infrared,

            /// <summary>
        /// Keyboard
        /// </summary>
            [Description("Keyboard")]
            Keyboard,

            /// <summary>
        /// Legacy Driver
        /// </summary>
            [Description("Legacy Driver")]
            LegacyDriver,

            /// <summary>
        /// Media
        /// </summary>
            [Description("Media Device")]
            Media,

            /// <summary>
        /// Medium Changer
        /// </summary>
            [Description("Medium Changer")]
            MediumChanger,

            /// <summary>
        /// Memory
        /// </summary>
            [Description("Memory")]
            Memory,

            /// <summary>
        /// Modem
        /// </summary>
            [Description("Modem")]
            Modem,

            /// <summary>
        /// Monitor
        /// </summary>
            [Description("Monitor")]
            Monitor,

            /// <summary>
        /// Mouse
        /// </summary>
            [Description("Mouse")]
            Mouse,

            /// <summary>
        /// Mtd
        /// </summary>
            [Description("Memory Technology Device (Flash Memory)")]
            Mtd,

            /// <summary>
        /// Multifunction
        /// </summary>
            [Description("Multifunction Device")]
            Multifunction,

            /// <summary>
        /// Multi Port Serial
        /// </summary>
            [Description("Multiport Serial Device")]
            MultiPortSerial,

            /// <summary>
        /// Net
        /// </summary>
            [Description("Network Adapter")]
            Net,

            /// <summary>
        /// Net Client
        /// </summary>
            [Description("Network Client")]
            NetClient,

            /// <summary>
        /// Net Service
        /// </summary>
            [Description("Network Service")]
            NetService,

            /// <summary>
        /// Net Trans
        /// </summary>
            [Description("Network Translation Device")]
            NetTrans,

            /// <summary>
        /// No Driver
        /// </summary>
            [Description("No Driver")]
            NoDriver,

            /// <summary>
        /// Pcmcia
        /// </summary>
            [Description("PCMCIA Device")]
            Pcmcia,

            /// <summary>
        /// Pnp Printers
        /// </summary>
            [Description("PnP Printer")]
            PnpPrinters,

            /// <summary>
        /// Ports
        /// </summary>
            [Description("Ports")]
            Ports,

            /// <summary>
        /// Printer
        /// </summary>
            [Description("Printer Queue")]
            PrinterQueue,

            /// <summary>
        /// Printer
        /// </summary>
            [Description("Printer")]
            Printer,

            /// <summary>
        /// Printer Upgrade
        /// </summary>
            [Description("Printer Upgrade")]
            PrinterUpgrade,

            /// <summary>
        /// Processor
        /// </summary>
            [Description("Microprocessor")]
            Processor,

            /// <summary>
        /// Sbp2
        /// </summary>
            [Description("Serial Bus Protocol 2")]
            Sbp2,

            /// <summary>
        /// Scsi Adapter
        /// </summary>
            [Description("Scsi Adapter")]
            ScsiAdapter,

            /// <summary>
        /// Security Accelerator
        /// </summary>
            [Description("Security Accelerator")]
            SecurityAccelerator,

            /// <summary>
        /// Sensor
        /// </summary>
            [Description("Sensor")]
            Sensor,

            /// <summary>
        /// Sideshow
        /// </summary>
            [Description("Windows Sideshow")]
            Sideshow,

            /// <summary>
        /// Smart Card Reader
        /// </summary>
            [Description("Smart Card Reader")]
            SmartCardReader,

            /// <summary>
        /// Sound
        /// </summary>
            [Description("Audio Device")]
            Sound,

            /// <summary>
        /// System
        /// </summary>
            [Description("System Device")]
            System,

            /// <summary>
        /// Tape Drive
        /// </summary>
            [Description("Tape Drive")]
            TapeDrive,

            /// <summary>
        /// Unknown
        /// </summary>
            [Description("Unknown")]
            Unknown,

            /// <summary>
        /// Usb
        /// </summary>
            [Description("USB Device")]
            Usb,

            /// <summary>
        /// Volume
        /// </summary>
            [Description("Storage Volume")]
            Volume,

            /// <summary>
        /// Volume Snapshot
        /// </summary>
            [Description("Storage Volume Snapshot")]
            VolumeSnapshot,

            /// <summary>
        /// Wce Usbs
        /// </summary>
            [Description("Windows Credential Editor")]
            WceUsbs,

            /// <summary>
        /// Wpd
        /// </summary>
            [Description("Windows Portable Device")]
            Wpd,

            /// <summary>
        /// Eh Storage Silo
        /// </summary>
            [Description("Storage Silo")]
            EhStorageSilo,

            /// <summary>
        /// Firmware
        /// </summary>
            [Description("Firmware Controller")]
            Firmware,

            /// <summary>
        /// Extension
        /// </summary>
            [Description("Extension")]
            Extension,
            Undefined = 0
        }

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
                return (DeviceInterfaceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_DISPLAY_ADAPTER)
                return (DeviceInterfaceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_I2C)
                return (DeviceInterfaceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_IMAGE)
                return (DeviceInterfaceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_MONITOR)
                return (DeviceInterfaceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_OPM)
                return (DeviceInterfaceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_HID)
                return (DeviceInterfaceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_KEYBOARD)
                return (DeviceInterfaceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_MOUSE)
                return (DeviceInterfaceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_MODEM)
                return (DeviceInterfaceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_NET)
                return (DeviceInterfaceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_SENSOR)
                return (DeviceInterfaceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_COMPORT)
                return (DeviceInterfaceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_PARALLEL)
                return (DeviceInterfaceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_PARCLASS)
                return (DeviceInterfaceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_SERENUM_BUS_ENUMERATOR)
                return (DeviceInterfaceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_CDCHANGER)
                return (DeviceInterfaceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_CDROM)
                return (DeviceInterfaceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_DISK)
                return (DeviceInterfaceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_FLOPPY)
                return (DeviceInterfaceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_MEDIUMCHANGER)
                return (DeviceInterfaceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_PARTITION)
                return (DeviceInterfaceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_STORAGEPORT)
                return (DeviceInterfaceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_TAPE)
                return (DeviceInterfaceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_VOLUME)
                return (DeviceInterfaceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_WRITEONCEDISK)
                return (DeviceInterfaceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_USB_DEVICE)
                return (DeviceInterfaceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_USB_HOST_CONTROLLER)
                return (DeviceInterfaceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_USB_HUB)
                return (DeviceInterfaceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_WPD)
                return (DeviceInterfaceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_WPD_PRIVATE)
                return (DeviceInterfaceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devInterface == DevProp.GUID_DEVINTERFACE_SIDESHOW)
                return (DeviceInterfaceClassEnum)Conversions.ToInteger(i);
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

            // ' classes
            if (devClass == DevProp.GUID_DEVCLASS_1394)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_1394DEBUG)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_61883)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_ADAPTER)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_APMSUPPORT)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_AVC)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_BATTERY)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_BIOMETRIC)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_BLUETOOTH)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_CDROM)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_COMPUTER)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_DECODER)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_DISKDRIVE)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_DISPLAY)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_DOT4)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_DOT4PRINT)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_ENUM1394)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_FDC)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_FLOPPYDISK)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_GPS)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_HDC)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_HIDCLASS)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_IMAGE)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_INFINIBAND)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_INFRARED)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_KEYBOARD)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_LEGACYDRIVER)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_MEDIA)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_MEDIUM_CHANGER)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_MEMORY)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_MODEM)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_MONITOR)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_MOUSE)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_MTD)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_MULTIFUNCTION)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_MULTIPORTSERIAL)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_NET)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_NETCLIENT)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_NETSERVICE)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_NETTRANS)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_NODRIVER)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_PCMCIA)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_PNPPRINTERS)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_PORTS)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_PRINTER_QUEUE)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_PRINTER)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_PRINTERUPGRADE)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_PROCESSOR)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_SBP2)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_SCSIADAPTER)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_SECURITYACCELERATOR)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_SENSOR)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_SIDESHOW)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_SMARTCARDREADER)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_SOUND)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_SYSTEM)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_TAPEDRIVE)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_UNKNOWN)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_USB)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_VOLUME)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_VOLUMESNAPSHOT)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_WCEUSBS)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_WPD)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_EHSTORAGESILO)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_FIRMWARE)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == DevProp.GUID_DEVCLASS_EXTENSION)
                return (DeviceClassEnum)Conversions.ToInteger(i);
            i += 1;
            if (devClass == new Guid("1ed2bbf9-11f0-4084-b21f-ad83a8e6dcdc"))
                return DeviceClassEnum.Printer;
            return DeviceClassEnum.Unknown;
        }

        /// <summary>
    /// Specifies the device capabilities.
    /// </summary>
    /// <remarks></remarks>
        [Flags]
        public enum DeviceCapabilities : uint
        {
            DeviceD1 = 0x1U,
            DeviceD2 = 0x2U,
            LockSupported = 0x4U,
            EjectSupported = 0x8U,
            Removable = 0x10U,
            DockDevice = 0x20U,
            UniqueID = 0x40U,
            SilentInstall = 0x80U,
            RawDeviceOK = 0x100U,
            SurpriseRemovalOK = 0x200U,
            WakeFromD0 = 0x400U,
            WakeFromD1 = 0x800U,
            WakeFromD2 = 0x1000U,
            WakeFromD3 = 0x2000U,
            HardwareDisabled = 0x4000U,
            NonDynamic = 0x8000U,
            WarmEjectSupported = 0x10000U,
            NoDisplayInUI = 0x20000U,
            Reserved1 = 0x40000U,
            Reserved = 0xFFF80000U
        }

        /// <summary>
    /// Specifies the storage type of the device.
    /// </summary>
    /// <remarks></remarks>
        public enum StorageType
        {
            HardDisk,
            RemovableHardDisk,
            Removable,
            Virtual,
            NetworkServer,
            NetworkShare,
            Optical,
            Volume,
            Folder,
            File
        }

        /// <summary>
    /// Specifies the type of the device.
    /// </summary>
    /// <remarks></remarks>
        public enum DeviceType
        {
            Disk,
            Network,
            Usb,
            Volume
        }

        /// <summary>
    /// Flags that specify the capabilities of a volume file system.
    /// </summary>
    /// <remarks></remarks>
        [Flags]
        public enum FileSystemFlags
        {

            /// <summary>
        /// The specified volume supports preserved case of file names when it places a name on disk.
        /// </summary>
            [Description("The specified volume supports preserved case of file names when it places a name on disk.")]
            CasePreservedNames = 0x2,

            /// <summary>
        /// The specified volume supports case-sensitive file names.
        /// </summary>
            [Description("The specified volume supports case-sensitive file names.")]
            CaseSensitiveSearch = 0x1,

            /// <summary>
        /// The specified volume supports file-based compression.
        /// </summary>
            [Description("The specified volume supports file-based compression.")]
            Compression = 0x10,

            /// <summary>
        /// The specified volume supports named streams.
        /// </summary>
            [Description("The specified volume supports named streams.")]
            NamedStreams = 0x40000,

            /// <summary>
        /// The specified volume preserves and enforces access control lists (ACL). For example, the NTFS file system preserves and enforces ACLs, and the FAT file system does not.
        /// </summary>
            [Description("The specified volume preserves and enforces access control lists (ACL). For example, the NTFS file system preserves and enforces ACLs, and the FAT file system does not.")]
            PersistentACLs = 0x8,

            /// <summary>
        /// The specified volume is read-only.
        /// </summary>
            [Description("The specified volume is read-only.")]
            ReadOnlyVolume = 0x80000,

            /// <summary>
        /// The specified volume supports a single sequential write.
        /// </summary>
            [Description("The specified volume supports a single sequential write.")]
            SequentialWriteOnce = 0x100000,

            /// <summary>
        /// The specified volume supports the Encrypted File System (EFS). For more information, see File Encryption.
        /// </summary>
            [Description("The specified volume supports the Encrypted File System (EFS). For more information, see File Encryption.")]
            SupportsEncryption = 0x20000,

            /// <summary>
        /// The specified volume supports extended attributes. An extended attribute is a piece of application-specific metadata that an application can associate with a file and is not part of the file's data.
        /// </summary>
            [Description("The specified volume supports extended attributes. An extended attribute is a piece of application-specific metadata that an application can associate with a file and is not part of the file's data.")]
            SupportsExtendedAttributes = 0x800000,

            /// <summary>
        /// The specified volume supports hard links. For more information, see Hard Links and Junctions.
        /// </summary>
            [Description("The specified volume supports hard links. For more information, see Hard Links and Junctions.")]
            SupportsHardLinks = 0x400000,

            /// <summary>
        /// The specified volume supports object identifiers.
        /// </summary>
            [Description("The specified volume supports object identifiers.")]
            SupportsObjectIds = 0x10000,


            /// <summary>
        /// The file system supports open by FileID. For more information, see IDBOTHDIRINFO.
        /// </summary>
            [Description("The file system supports open by FileID. For more information, see IDBOTHDIRINFO.")]
            SupportsOpenById = 0x1000000,


            /// <summary>
        /// The specified volume supports reparse points.
        /// </summary>
            [Description("The specified volume supports reparse points.")]
            SupportsReparsePoints = 0x80,


            /// <summary>
        /// The specified volume supports sparse files.
        /// </summary>
            [Description("The specified volume supports sparse files.")]
            SupportsSparseFiles = 0x40,


            /// <summary>
        /// The specified volume supports transactions. For more information, see About KTM.
        /// </summary>
            [Description("The specified volume supports transactions. For more information, see About KTM.")]
            SupportsTransactions = 0x200000,

            /// <summary>
        /// The specified volume supports update sequence number (USN) journals. For more information, see Change Journal Records.
        /// </summary>
            [Description("The specified volume supports update sequence number (USN) journals. For more information, see Change Journal Records.")]
            SupportsUSNJournal = 0x2000000,

            /// <summary>
        /// The specified volume supports Unicode in file names as they appear on disk.
        /// </summary>
            [Description("The specified volume supports Unicode in file names as they appear on disk.")]
            UnicodeOnDisk = 0x4,

            /// <summary>
        /// The specified volume is a compressed volume, for example, a DoubleSpace volume.
        /// </summary>
            [Description("The specified volume is a compressed volume, for example, a DoubleSpace volume.")]
            VolumeIsCompressed = 0x8000,

            /// <summary>
        /// The specified volume supports disk quotas.
        /// </summary>
            [Description("The specified volume supports disk quotas.")]
            VolumeQuotas = 0x20
        }

        /// <summary>
    /// Represents partition location information on a physical disk.
    /// </summary>
    /// <remarks></remarks>
        public struct DiskExtent
        {

            /// <summary>
        /// The physical device number
        /// </summary>
        /// <remarks></remarks>
            public int PhysicalDevice;

            /// <summary>
        /// Reserved
        /// </summary>
        /// <remarks></remarks>
            public int Space;

            /// <summary>
        /// Physical byte offset on disk
        /// </summary>
        /// <remarks></remarks>
            public long Offset;

            /// <summary>
        /// Physical size in bytes on disk
        /// </summary>
        /// <remarks></remarks>
            public long Size;

            /// <summary>
        /// Presents this object in a readable string.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
            public override string ToString()
            {
                return "Physical Device " + PhysicalDevice + ", " + TextTools.PrintFriendlySize(Size);
            }
        }
    }
}