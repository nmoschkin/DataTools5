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
}
