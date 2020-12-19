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
using DataTools.Win32;

namespace DataTools.Win32
{
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
}
