'' ************************************************* ''
'' DataTools Visual Basic Utility Library - Interop
''
'' Module: Miscellaneous enums to support devices.
'' 
'' Copyright (C) 2011-2020 Nathan Moschkin
'' All Rights Reserved
''
'' Licensed Under the Microsoft Public License   
'' ************************************************* ''


Imports System.ComponentModel
Imports DataTools.Interop.Native
Imports CoreCT.Text

Public Module DevClassPresenting

    ''' <summary>
    ''' System device removal policy.
    ''' </summary>
    Public Enum DeviceRemovalPolicy

        ''' <summary>
        ''' The device is not expected to be removed.
        ''' </summary>
        ''' <remarks></remarks>
        ExpectNoRemoval = 1

        ''' <summary>
        ''' The device can be expected to be removed in an orderly fashion.
        ''' </summary>
        ''' <remarks></remarks>
        ExpectOrderlyRemoval = 2

        ''' <summary>
        ''' The device can be expected to be removed without any preparation for removal.
        ''' </summary>
        ''' <remarks></remarks>
        ExpectSurpriseRemoval = 3

    End Enum

    ''' <summary>
    ''' System device characteristics.
    ''' </summary>
    Public Enum DeviceCharacteristcs
        ''' <summary>
        ''' Beep
        ''' </summary>
        <Description("Beep")>
        Beep = &H1

        ''' <summary>
        ''' Cdrom
        ''' </summary>
        <Description("Cdrom")>
        Cdrom = &H2

        ''' <summary>
        ''' Cdfs
        ''' </summary>
        <Description("Cdfs")>
        Cdfs = &H3

        ''' <summary>
        ''' Controller
        ''' </summary>
        <Description("Controller")>
        Controller = &H4

        ''' <summary>
        ''' Datalink
        ''' </summary>
        <Description("Datalink")>
        Datalink = &H5

        ''' <summary>
        ''' Dfs
        ''' </summary>
        <Description("Dfs")>
        Dfs = &H6

        ''' <summary>
        ''' Disk
        ''' </summary>
        <Description("Disk")>
        Disk = &H7

        ''' <summary>
        ''' Disk File System
        ''' </summary>
        <Description("Disk File System")>
        DiskFileSystem = &H8

        ''' <summary>
        ''' File System
        ''' </summary>
        <Description("File System")>
        FileSystem = &H9

        ''' <summary>
        ''' Inport Port
        ''' </summary>
        <Description("Inport Port")>
        InportPort = &HA

        ''' <summary>
        ''' Keyboard
        ''' </summary>
        <Description("Keyboard")>
        Keyboard = &HB

        ''' <summary>
        ''' Mailslot
        ''' </summary>
        <Description("Mailslot")>
        Mailslot = &HC

        ''' <summary>
        ''' Midi In
        ''' </summary>
        <Description("Midi In")>
        MidiIn = &HD

        ''' <summary>
        ''' Midi Out
        ''' </summary>
        <Description("Midi Out")>
        MidiOut = &HE

        ''' <summary>
        ''' Mouse
        ''' </summary>
        <Description("Mouse")>
        Mouse = &HF

        ''' <summary>
        ''' Multi Unc Provider
        ''' </summary>
        <Description("Multi Unc Provider")>
        MultiUncProvider = &H10

        ''' <summary>
        ''' Named Pipe
        ''' </summary>
        <Description("Named Pipe")>
        NamedPipe = &H11

        ''' <summary>
        ''' Network
        ''' </summary>
        <Description("Network")>
        Network = &H12

        ''' <summary>
        ''' Network Browser
        ''' </summary>
        <Description("Network Browser")>
        NetworkBrowser = &H13

        ''' <summary>
        ''' Network File System
        ''' </summary>
        <Description("Network File System")>
        NetworkFileSystem = &H14

        ''' <summary>
        ''' Null
        ''' </summary>
        <Description("Null")>
        Null = &H15

        ''' <summary>
        ''' Parallel Port
        ''' </summary>
        <Description("Parallel Port")>
        ParallelPort = &H16

        ''' <summary>
        ''' Physical Netcard
        ''' </summary>
        <Description("Physical Netcard")>
        PhysicalNetcard = &H17

        ''' <summary>
        ''' Printer
        ''' </summary>
        <Description("Printer")>
        Printer = &H18

        ''' <summary>
        ''' Scanner
        ''' </summary>
        <Description("Scanner")>
        Scanner = &H19

        ''' <summary>
        ''' Serial Mouse Port
        ''' </summary>
        <Description("Serial Mouse Port")>
        SerialMousePort = &H1A

        ''' <summary>
        ''' Serial Port
        ''' </summary>
        <Description("Serial Port")>
        SerialPort = &H1B

        ''' <summary>
        ''' Screen
        ''' </summary>
        <Description("Screen")>
        Screen = &H1C

        ''' <summary>
        ''' Sound
        ''' </summary>
        <Description("Sound")>
        Sound = &H1D

        ''' <summary>
        ''' Streams
        ''' </summary>
        <Description("Streams")>
        Streams = &H1E

        ''' <summary>
        ''' Tape
        ''' </summary>
        <Description("Tape")>
        Tape = &H1F

        ''' <summary>
        ''' Tape File System
        ''' </summary>
        <Description("Tape File System")>
        TapeFileSystem = &H20

        ''' <summary>
        ''' Transport
        ''' </summary>
        <Description("Transport")>
        Transport = &H21

        ''' <summary>
        ''' Unknown
        ''' </summary>
        <Description("Unknown")>
        Unknown = &H22

        ''' <summary>
        ''' Video
        ''' </summary>
        <Description("Video")>
        Video = &H23

        ''' <summary>
        ''' Virtual Disk
        ''' </summary>
        <Description("Virtual Disk")>
        VirtualDisk = &H24

        ''' <summary>
        ''' Wave In
        ''' </summary>
        <Description("Wave In")>
        WaveIn = &H25

        ''' <summary>
        ''' Wave Out
        ''' </summary>
        <Description("Wave Out")>
        WaveOut = &H26

        ''' <summary>
        ''' P8042 Port
        ''' </summary>
        <Description("P8042 Port")>
        P8042Port = &H27

        ''' <summary>
        ''' Network Redirector
        ''' </summary>
        <Description("Network Redirector")>
        NetworkRedirector = &H28

        ''' <summary>
        ''' Battery
        ''' </summary>
        <Description("Battery")>
        Battery = &H29

        ''' <summary>
        ''' Bus Extender
        ''' </summary>
        <Description("Bus Extender")>
        BusExtender = &H2A

        ''' <summary>
        ''' Modem
        ''' </summary>
        <Description("Modem")>
        Modem = &H2B

        ''' <summary>
        ''' Vdm
        ''' </summary>
        <Description("Vdm")>
        Vdm = &H2C

        ''' <summary>
        ''' Mass Storage
        ''' </summary>
        <Description("Mass Storage")>
        MassStorage = &H2D

        ''' <summary>
        ''' Smb
        ''' </summary>
        <Description("Smb")>
        Smb = &H2E

        ''' <summary>
        ''' Ks
        ''' </summary>
        <Description("Ks")>
        Ks = &H2F

        ''' <summary>
        ''' Changer
        ''' </summary>
        <Description("Changer")>
        Changer = &H30

        ''' <summary>
        ''' Smartcard
        ''' </summary>
        <Description("Smartcard")>
        Smartcard = &H31

        ''' <summary>
        ''' Acpi
        ''' </summary>
        <Description("Acpi")>
        Acpi = &H32

        ''' <summary>
        ''' Dvd
        ''' </summary>
        <Description("Dvd")>
        Dvd = &H33

        ''' <summary>
        ''' Fullscreen Video
        ''' </summary>
        <Description("Fullscreen Video")>
        FullscreenVideo = &H34

        ''' <summary>
        ''' Dfs File System
        ''' </summary>
        <Description("Dfs File System")>
        DfsFileSystem = &H35

        ''' <summary>
        ''' Dfs Volume
        ''' </summary>
        <Description("Dfs Volume")>
        DfsVolume = &H36

        ''' <summary>
        ''' Serenum
        ''' </summary>
        <Description("Serenum")>
        Serenum = &H37

        ''' <summary>
        ''' Termsrv
        ''' </summary>
        <Description("Termsrv")>
        Termsrv = &H38

        ''' <summary>
        ''' Ksec
        ''' </summary>
        <Description("Ksec")>
        Ksec = &H39

        ''' <summary>
        ''' Fips
        ''' </summary>
        <Description("Fips")>
        Fips = &H3A

        ''' <summary>
        ''' Infiniband
        ''' </summary>
        <Description("Infiniband")>
        Infiniband = &H3B

        ''' <summary>
        ''' Vmbus
        ''' </summary>
        <Description("Vmbus")>
        Vmbus = &H3E

        ''' <summary>
        ''' Crypt Provider
        ''' </summary>
        <Description("Crypt Provider")>
        CryptProvider = &H3F

        ''' <summary>
        ''' Wpd
        ''' </summary>
        <Description("Wpd")>
        Wpd = &H40

        ''' <summary>
        ''' Bluetooth
        ''' </summary>
        <Description("Bluetooth")>
        Bluetooth = &H41

        ''' <summary>
        ''' Mt Composite
        ''' </summary>
        <Description("Mt Composite")>
        MtComposite = &H42

        ''' <summary>
        ''' Mt Transport
        ''' </summary>
        <Description("Mt Transport")>
        MtTransport = &H43

        ''' <summary>
        ''' Biometric
        ''' </summary>
        <Description("Biometric Device")>
        Biometric = &H44

        ''' <summary>
        ''' Pmi
        ''' </summary>
        <Description("Pmi")>
        Pmi = &H45

        ''' <summary>
        ''' Ehstor
        ''' </summary>
        <Description("Storage Silo Enhanced Storage")>
        Ehstor = &H46

        ''' <summary>
        ''' Devapi
        ''' </summary>
        <Description("Devapi")>
        Devapi = &H47

        ''' <summary>
        ''' Gpio
        ''' </summary>
        <Description("Gpio")>
        Gpio = &H48

        ''' <summary>
        ''' Usbex
        ''' </summary>
        <Description("Usbex")>
        Usbex = &H49

        ''' <summary>
        ''' Console
        ''' </summary>
        <Description("Console")>
        Console = &H50

        ''' <summary>
        ''' Nfp
        ''' </summary>
        <Description("Nfp")>
        Nfp = &H51

        ''' <summary>
        ''' Sysenv
        ''' </summary>
        <Description("Sysenv")>
        Sysenv = &H52

        ''' <summary>
        ''' Virtual Block
        ''' </summary>
        <Description("Virtual Block")>
        VirtualBlock = &H53

        ''' <summary>
        ''' Point Of Service
        ''' </summary>
        <Description("Point Of Service")>
        PointOfService = &H54

    End Enum


    ''' <summary>
    ''' Device interface classes.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum DeviceInterfaceClassEnum

        ''' <summary>
        ''' Monitor brightness control.
        ''' </summary>
        <Description("Monitor Brightness Control")>
        Brightness

        ''' <summary>
        ''' Display adapter.
        ''' </summary>
        <Description("Display Adapter")>
        DisplayAdapter

        ''' <summary>
        ''' Display adapter driver that communicates with child devices over the I2C bus.
        ''' </summary>
        ''' <remarks></remarks>
        <Description("Display adapter driver that communicates with child devices over the I2C bus.")>
        I2C

        ''' <summary>
        ''' Digital camera and scanner devices.
        ''' </summary>
        <Description("Digital Camera or Scanner Device")>
        ImagingDevice

        ''' <summary>
        ''' Computer display monitors.
        ''' </summary>
        <Description("Monitor")>
        Monitor

        ''' <summary>
        ''' Output Protection Manager (OPM) device driver interface for video signals copy protection.
        ''' </summary>
        <Description("Output Protection Manager (OPM) device driver interface for video signals copy protection.")>
        OPM

        ''' <summary>
        ''' Human Interface Devices.
        ''' </summary>
        <Description("Human Interface Device")>
        HID

        ''' <summary>
        ''' Keyboards.
        ''' </summary>
        <Description("Keyboard")>
        Keyboard

        ''' <summary>
        ''' Mice.
        ''' </summary>
        <Description("Mouse")>
        Mouse

        ''' <summary>
        ''' Modems.
        ''' </summary>
        <Description("Modem")>
        Modem

        ''' <summary>
        ''' Network adapters.
        ''' </summary>
        <Description("Network Adapter")>
        Network

        ''' <summary>
        ''' Sensors.
        ''' </summary>
        <Description("Sensor")>
        Sensor

        ''' <summary>
        ''' COM port.
        ''' </summary>
        <Description("COM Port")>
        comPort

        ''' <summary>
        ''' LPT port.
        ''' </summary>
        <Description("Parallel Port")>
        ParallelPort

        ''' <summary>
        ''' LPT device.
        ''' </summary>
        <Description("Parallel Device")>
        ParallelDevice

        ''' <summary>
        ''' Bus Enumerator for Plug'n'Play serial ports.
        ''' </summary>
        <Description("Bus Enumerator for Plug'n'Play Serial Ports")>
        SerialBusEnum

        ''' <summary>
        ''' optical media changing device.
        ''' </summary>
        <Description("optical Media Changing Device")>
        CDChanger

        ''' <summary>
        ''' Optical device.
        ''' </summary>
        <Description("Optical Device")>
        CDROM

        ''' <summary>
        ''' Disk device.
        ''' </summary>
        <Description("Disk Device")>
        Disk

        ''' <summary>
        ''' Floppy disk device.
        ''' </summary>
        <Description("Floppy Disk Device")>
        Floppy

        ''' <summary>
        ''' Medium changing device.
        ''' </summary>
        <Description("Medium changing device")>
        MediumChanger

        ''' <summary>
        ''' Disk partition.
        ''' </summary>
        <Description("Disk partition")>
        Partition

        ''' <summary>
        ''' SCSI/ATA/StorPort Device.
        ''' </summary>
        ''' <remarks></remarks>
        <Description("SCSI/ATA/StorPort Device")>
        StoragePort

        ''' <summary>
        ''' Tape backup device.
        ''' </summary>
        <Description("Tape backup device")>
        Tape

        ''' <summary>
        ''' Logical volume.
        ''' </summary>
        <Description("Logical volume")>
        Volume

        ''' <summary>
        ''' Write once disk.
        ''' </summary>
        <Description("Write once disk")>
        WriteOnceDisk

        ''' <summary>
        ''' USB host controller.
        ''' </summary>
        <Description("USB host controller")>
        UsbHostController

        ''' <summary>
        ''' USB Hub
        ''' </summary>
        <Description("USB Hub")>
        UsbHub

        ''' <summary>
        ''' Windows Portable Device
        ''' </summary>
        <Description("Windows Portable Device")>
        Wpd

        ''' <summary>
        ''' Specialized Windows Portable Device
        ''' </summary>
        <Description("Specialized Windows Portable Device")>
        WpdSpecialized

        ''' <summary>
        ''' Windows SideShow Device
        ''' </summary>
        <Description("Windows SideShow Device")>
        SideShow

        Unknown = -1
    End Enum

    ''' <summary>
    ''' Device classes.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum DeviceClassEnum

        ''' <summary>
        ''' Bus 1394
        ''' </summary>
        <Description("IEEE 1394 Isosynchronous Data Transfer Protocol (FireWire)")>
        Bus1394 = &H200

        ''' <summary>
        ''' Bus 1394 Debug
        ''' </summary>
        <Description("IEEE 1394 (FireWire) Debug Mode")>
        Bus1394Debug

        ''' <summary>
        ''' Iec 61883
        ''' </summary>
        <Description("IEC 61883 Consumer Audio/Video Equipment - Digital Interface")>
        Iec61883

        ''' <summary>
        ''' Adapter
        ''' </summary>
        <Description("Adapter")>
        Adapter

        ''' <summary>
        ''' Apmsupport
        ''' </summary>
        <Description("Advanced Power Management")>
        Apmsupport

        ''' <summary>
        ''' Avc
        ''' </summary>
        <Description("H.264/MPEG-4 Part 10 Advanced Video Coding")>
        Avc

        ''' <summary>
        ''' Battery
        ''' </summary>
        <Description("UPS Battery")>
        Battery

        ''' <summary>
        ''' Biometric
        ''' </summary>
        <Description("Biometric Feedback")>
        Biometric

        ''' <summary>
        ''' Bluetooth
        ''' </summary>
        <Description("Bluetooth")>
        Bluetooth

        ''' <summary>
        ''' Cd Rom
        ''' </summary>
        <Description("Cd Rom")>
        CdRom

        ''' <summary>
        ''' Computer
        ''' </summary>
        <Description("Computer")>
        Computer

        ''' <summary>
        ''' Decoder
        ''' </summary>
        <Description("Decoder")>
        Decoder

        ''' <summary>
        ''' Disk Drive
        ''' </summary>
        <Description("Disk Drive")>
        DiskDrive

        ''' <summary>
        ''' Display
        ''' </summary>
        <Description("Display")>
        Display

        ''' <summary>
        ''' Dot4
        ''' </summary>
        <Description("Dot4")>
        Dot4

        ''' <summary>
        ''' Dot4 Print
        ''' </summary>
        <Description("Dot4 Print")>
        Dot4Print

        ''' <summary>
        ''' Enum 1394
        ''' </summary>
        <Description("IEEE 1394 FireWire Enumerator")>
        Enum1394

        ''' <summary>
        ''' Fdc
        ''' </summary>
        <Description("Floppy Disk Controller")>
        Fdc

        ''' <summary>
        ''' Floppy Disk
        ''' </summary>
        <Description("Floppy Disk")>
        FloppyDisk

        ''' <summary>
        ''' Gps
        ''' </summary>
        <Description("Global Positioning Device")>
        Gps

        ''' <summary>
        ''' Hdc
        ''' </summary>
        <Description("Hard Disk Controller")>
        Hdc

        ''' <summary>
        ''' Hid Class
        ''' </summary>
        <Description("Human Interface Device")>
        HidClass

        ''' <summary>
        ''' Image
        ''' </summary>
        <Description("Imaging Device")>
        Image

        ''' <summary>
        ''' Infini Band
        ''' </summary>
        <Description("InfiniBand Adapter")>
        InfiniBand

        ''' <summary>
        ''' Infrared
        ''' </summary>
        <Description("Infrared Sensor")>
        Infrared

        ''' <summary>
        ''' Keyboard
        ''' </summary>
        <Description("Keyboard")>
        Keyboard

        ''' <summary>
        ''' Legacy Driver
        ''' </summary>
        <Description("Legacy Driver")>
        LegacyDriver

        ''' <summary>
        ''' Media
        ''' </summary>
        <Description("Media Device")>
        Media

        ''' <summary>
        ''' Medium Changer
        ''' </summary>
        <Description("Medium Changer")>
        MediumChanger

        ''' <summary>
        ''' Memory
        ''' </summary>
        <Description("Memory")>
        Memory

        ''' <summary>
        ''' Modem
        ''' </summary>
        <Description("Modem")>
        Modem

        ''' <summary>
        ''' Monitor
        ''' </summary>
        <Description("Monitor")>
        Monitor

        ''' <summary>
        ''' Mouse
        ''' </summary>
        <Description("Mouse")>
        Mouse

        ''' <summary>
        ''' Mtd
        ''' </summary>
        <Description("Memory Technology Device (Flash Memory)")>
        Mtd

        ''' <summary>
        ''' Multifunction
        ''' </summary>
        <Description("Multifunction Device")>
        Multifunction

        ''' <summary>
        ''' Multi Port Serial
        ''' </summary>
        <Description("Multiport Serial Device")>
        MultiPortSerial

        ''' <summary>
        ''' Net
        ''' </summary>
        <Description("Network Adapter")>
        Net

        ''' <summary>
        ''' Net Client
        ''' </summary>
        <Description("Network Client")>
        NetClient

        ''' <summary>
        ''' Net Service
        ''' </summary>
        <Description("Network Service")>
        NetService

        ''' <summary>
        ''' Net Trans
        ''' </summary>
        <Description("Network Translation Device")>
        NetTrans

        ''' <summary>
        ''' No Driver
        ''' </summary>
        <Description("No Driver")>
        NoDriver

        ''' <summary>
        ''' Pcmcia
        ''' </summary>
        <Description("PCMCIA Device")>
        Pcmcia

        ''' <summary>
        ''' Pnp Printers
        ''' </summary>
        <Description("PnP Printer")>
        PnpPrinters

        ''' <summary>
        ''' Ports
        ''' </summary>
        <Description("Ports")>
        Ports

        ''' <summary>
        ''' Printer
        ''' </summary>
        <Description("Printer Queue")>
        PrinterQueue

        ''' <summary>
        ''' Printer
        ''' </summary>
        <Description("Printer")>
        Printer

        ''' <summary>
        ''' Printer Upgrade
        ''' </summary>
        <Description("Printer Upgrade")>
        PrinterUpgrade

        ''' <summary>
        ''' Processor
        ''' </summary>
        <Description("Microprocessor")>
        Processor

        ''' <summary>
        ''' Sbp2
        ''' </summary>
        <Description("Serial Bus Protocol 2")>
        Sbp2

        ''' <summary>
        ''' Scsi Adapter
        ''' </summary>
        <Description("Scsi Adapter")>
        ScsiAdapter

        ''' <summary>
        ''' Security Accelerator
        ''' </summary>
        <Description("Security Accelerator")>
        SecurityAccelerator

        ''' <summary>
        ''' Sensor
        ''' </summary>
        <Description("Sensor")>
        Sensor

        ''' <summary>
        ''' Sideshow
        ''' </summary>
        <Description("Windows Sideshow")>
        Sideshow

        ''' <summary>
        ''' Smart Card Reader
        ''' </summary>
        <Description("Smart Card Reader")>
        SmartCardReader

        ''' <summary>
        ''' Sound
        ''' </summary>
        <Description("Audio Device")>
        Sound

        ''' <summary>
        ''' System
        ''' </summary>
        <Description("System Device")>
        System

        ''' <summary>
        ''' Tape Drive
        ''' </summary>
        <Description("Tape Drive")>
        TapeDrive

        ''' <summary>
        ''' Unknown
        ''' </summary>
        <Description("Unknown")>
        Unknown

        ''' <summary>
        ''' Usb
        ''' </summary>
        <Description("USB Device")>
        Usb

        ''' <summary>
        ''' Volume
        ''' </summary>
        <Description("Storage Volume")>
        Volume

        ''' <summary>
        ''' Volume Snapshot
        ''' </summary>
        <Description("Storage Volume Snapshot")>
        VolumeSnapshot

        ''' <summary>
        ''' Wce Usbs
        ''' </summary>
        <Description("Windows Credential Editor")>
        WceUsbs

        ''' <summary>
        ''' Wpd
        ''' </summary>
        <Description("Windows Portable Device")>
        Wpd

        ''' <summary>
        ''' Eh Storage Silo
        ''' </summary>
        <Description("Storage Silo")>
        EhStorageSilo

        ''' <summary>
        ''' Firmware
        ''' </summary>
        <Description("Firmware Controller")>
        Firmware

        ''' <summary>
        ''' Extension
        ''' </summary>
        <Description("Extension")>
        Extension

        Undefined = 0
    End Enum

    ''' <summary>
    ''' Return a device interface enum value based on a DEVINTERFACE GUID.
    ''' </summary>
    ''' <param name="devInterface">The device interface to translate.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Function GetDevInterfaceClassEnumFromGuid(devInterface As Guid) As DeviceInterfaceClassEnum
        Dim i As Integer

        If devInterface = GUID_DEVINTERFACE_BRIGHTNESS Then Return CType(i, DeviceInterfaceClassEnum)
        i += 1

        If devInterface = GUID_DEVINTERFACE_DISPLAY_ADAPTER Then Return CType(i, DeviceInterfaceClassEnum)
        i += 1

        If devInterface = GUID_DEVINTERFACE_I2C Then Return CType(i, DeviceInterfaceClassEnum)
        i += 1

        If devInterface = GUID_DEVINTERFACE_IMAGE Then Return CType(i, DeviceInterfaceClassEnum)
        i += 1

        If devInterface = GUID_DEVINTERFACE_MONITOR Then Return CType(i, DeviceInterfaceClassEnum)
        i += 1

        If devInterface = GUID_DEVINTERFACE_OPM Then Return CType(i, DeviceInterfaceClassEnum)
        i += 1

        If devInterface = GUID_DEVINTERFACE_HID Then Return CType(i, DeviceInterfaceClassEnum)
        i += 1

        If devInterface = GUID_DEVINTERFACE_KEYBOARD Then Return CType(i, DeviceInterfaceClassEnum)
        i += 1

        If devInterface = GUID_DEVINTERFACE_MOUSE Then Return CType(i, DeviceInterfaceClassEnum)
        i += 1

        If devInterface = GUID_DEVINTERFACE_MODEM Then Return CType(i, DeviceInterfaceClassEnum)
        i += 1

        If devInterface = GUID_DEVINTERFACE_NET Then Return CType(i, DeviceInterfaceClassEnum)
        i += 1

        If devInterface = GUID_DEVINTERFACE_SENSOR Then Return CType(i, DeviceInterfaceClassEnum)
        i += 1

        If devInterface = GUID_DEVINTERFACE_COMPORT Then Return CType(i, DeviceInterfaceClassEnum)
        i += 1

        If devInterface = GUID_DEVINTERFACE_PARALLEL Then Return CType(i, DeviceInterfaceClassEnum)
        i += 1

        If devInterface = GUID_DEVINTERFACE_PARCLASS Then Return CType(i, DeviceInterfaceClassEnum)
        i += 1

        If devInterface = GUID_DEVINTERFACE_SERENUM_BUS_ENUMERATOR Then Return CType(i, DeviceInterfaceClassEnum)
        i += 1

        If devInterface = GUID_DEVINTERFACE_CDCHANGER Then Return CType(i, DeviceInterfaceClassEnum)
        i += 1

        If devInterface = GUID_DEVINTERFACE_CDROM Then Return CType(i, DeviceInterfaceClassEnum)
        i += 1

        If devInterface = GUID_DEVINTERFACE_DISK Then Return CType(i, DeviceInterfaceClassEnum)
        i += 1

        If devInterface = GUID_DEVINTERFACE_FLOPPY Then Return CType(i, DeviceInterfaceClassEnum)
        i += 1

        If devInterface = GUID_DEVINTERFACE_MEDIUMCHANGER Then Return CType(i, DeviceInterfaceClassEnum)
        i += 1

        If devInterface = GUID_DEVINTERFACE_PARTITION Then Return CType(i, DeviceInterfaceClassEnum)
        i += 1

        If devInterface = GUID_DEVINTERFACE_STORAGEPORT Then Return CType(i, DeviceInterfaceClassEnum)
        i += 1

        If devInterface = GUID_DEVINTERFACE_TAPE Then Return CType(i, DeviceInterfaceClassEnum)
        i += 1

        If devInterface = GUID_DEVINTERFACE_VOLUME Then Return CType(i, DeviceInterfaceClassEnum)
        i += 1

        If devInterface = GUID_DEVINTERFACE_WRITEONCEDISK Then Return CType(i, DeviceInterfaceClassEnum)
        i += 1

        If devInterface = GUID_DEVINTERFACE_USB_DEVICE Then Return CType(i, DeviceInterfaceClassEnum)
        i += 1

        If devInterface = GUID_DEVINTERFACE_USB_HOST_CONTROLLER Then Return CType(i, DeviceInterfaceClassEnum)
        i += 1

        If devInterface = GUID_DEVINTERFACE_USB_HUB Then Return CType(i, DeviceInterfaceClassEnum)
        i += 1

        If devInterface = GUID_DEVINTERFACE_WPD Then Return CType(i, DeviceInterfaceClassEnum)
        i += 1

        If devInterface = GUID_DEVINTERFACE_WPD_PRIVATE Then Return CType(i, DeviceInterfaceClassEnum)
        i += 1

        If devInterface = GUID_DEVINTERFACE_SIDESHOW Then Return CType(i, DeviceInterfaceClassEnum)

        Return DeviceInterfaceClassEnum.Unknown

    End Function

    ''' <summary>
    ''' Return a device class enum value based on a DEVCLASS GUID.
    ''' </summary>
    ''' <param name="devClass">The device class to translate.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Function GetDevClassEnumFromGuid(devClass As Guid) As DeviceClassEnum
        Dim i As Integer = &H200

        '' classes
        If devClass = GUID_DEVCLASS_1394 Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_1394DEBUG Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_61883 Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_ADAPTER Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_APMSUPPORT Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_AVC Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_BATTERY Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_BIOMETRIC Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_BLUETOOTH Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_CDROM Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_COMPUTER Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_DECODER Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_DISKDRIVE Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_DISPLAY Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_DOT4 Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_DOT4PRINT Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_ENUM1394 Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_FDC Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_FLOPPYDISK Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_GPS Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_HDC Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_HIDCLASS Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_IMAGE Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_INFINIBAND Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_INFRARED Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_KEYBOARD Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_LEGACYDRIVER Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_MEDIA Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_MEDIUM_CHANGER Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_MEMORY Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_MODEM Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_MONITOR Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_MOUSE Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_MTD Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_MULTIFUNCTION Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_MULTIPORTSERIAL Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_NET Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_NETCLIENT Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_NETSERVICE Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_NETTRANS Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_NODRIVER Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_PCMCIA Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_PNPPRINTERS Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_PORTS Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_PRINTER_QUEUE Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_PRINTER Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_PRINTERUPGRADE Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_PROCESSOR Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_SBP2 Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_SCSIADAPTER Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_SECURITYACCELERATOR Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_SENSOR Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_SIDESHOW Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_SMARTCARDREADER Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_SOUND Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_SYSTEM Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_TAPEDRIVE Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_UNKNOWN Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_USB Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_VOLUME Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_VOLUMESNAPSHOT Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_WCEUSBS Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_WPD Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_EHSTORAGESILO Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_FIRMWARE Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = GUID_DEVCLASS_EXTENSION Then Return CType(i, DeviceClassEnum)
        i += 1

        If devClass = New Guid("1ed2bbf9-11f0-4084-b21f-ad83a8e6dcdc") Then Return DeviceClassEnum.Printer

        Return DeviceClassEnum.Unknown

    End Function

    ''' <summary>
    ''' Specifies the device capabilities.
    ''' </summary>
    ''' <remarks></remarks>
    <Flags>
    Public Enum DeviceCapabilities As UInteger
        DeviceD1 = &H1
        DeviceD2 = &H2
        LockSupported = &H4
        EjectSupported = &H8
        Removable = &H10
        DockDevice = &H20
        UniqueID = &H40
        SilentInstall = &H80
        RawDeviceOK = &H100
        SurpriseRemovalOK = &H200
        WakeFromD0 = &H400
        WakeFromD1 = &H800
        WakeFromD2 = &H1000
        WakeFromD3 = &H2000
        HardwareDisabled = &H4000
        NonDynamic = &H8000
        WarmEjectSupported = &H10000
        NoDisplayInUI = &H20000
        Reserved1 = &H40000
        Reserved = &HFFF80000UI
    End Enum

    ''' <summary>
    ''' Specifies the storage type of the device.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum StorageType
        HardDisk
        RemovableHardDisk
        Removable
        Virtual
        NetworkServer
        NetworkShare
        Optical
        Volume
        Folder
        File
    End Enum

    ''' <summary>
    ''' Specifies the type of the device.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum DeviceType
        Disk
        Network
        Usb
        Volume
    End Enum

    ''' <summary>
    ''' Flags that specify the capabilities of a volume file system.
    ''' </summary>
    ''' <remarks></remarks>
    <Flags>
    Public Enum FileSystemFlags

        ''' <summary>
        ''' The specified volume supports preserved case of file names when it places a name on disk.
        ''' </summary>
        <Description("The specified volume supports preserved case of file names when it places a name on disk.")>
        CasePreservedNames = &H2

        ''' <summary>
        ''' The specified volume supports case-sensitive file names.
        ''' </summary>
        <Description("The specified volume supports case-sensitive file names.")>
        CaseSensitiveSearch = &H1

        ''' <summary>
        ''' The specified volume supports file-based compression.
        ''' </summary>
        <Description("The specified volume supports file-based compression.")>
        Compression = &H10

        ''' <summary>
        ''' The specified volume supports named streams.
        ''' </summary>
        <Description("The specified volume supports named streams.")>
        NamedStreams = &H40000

        ''' <summary>
        ''' The specified volume preserves and enforces access control lists (ACL). For example, the NTFS file system preserves and enforces ACLs, and the FAT file system does not.
        ''' </summary>
        <Description("The specified volume preserves and enforces access control lists (ACL). For example, the NTFS file system preserves and enforces ACLs, and the FAT file system does not.")>
        PersistentACLs = &H8

        ''' <summary>
        ''' The specified volume is read-only.
        ''' </summary>
        <Description("The specified volume is read-only.")>
        ReadOnlyVolume = &H80000

        ''' <summary>
        ''' The specified volume supports a single sequential write.
        ''' </summary>
        <Description("The specified volume supports a single sequential write.")>
        SequentialWriteOnce = &H100000

        ''' <summary>
        ''' The specified volume supports the Encrypted File System (EFS). For more information, see File Encryption.
        ''' </summary>
        <Description("The specified volume supports the Encrypted File System (EFS). For more information, see File Encryption.")>
        SupportsEncryption = &H20000

        ''' <summary>
        ''' The specified volume supports extended attributes. An extended attribute is a piece of application-specific metadata that an application can associate with a file and is not part of the file's data.
        ''' </summary>
        <Description("The specified volume supports extended attributes. An extended attribute is a piece of application-specific metadata that an application can associate with a file and is not part of the file's data.")>
        SupportsExtendedAttributes = &H800000

        ''' <summary>
        ''' The specified volume supports hard links. For more information, see Hard Links and Junctions.
        ''' </summary>
        <Description("The specified volume supports hard links. For more information, see Hard Links and Junctions.")>
        SupportsHardLinks = &H400000

        ''' <summary>
        ''' The specified volume supports object identifiers.
        ''' </summary>
        <Description("The specified volume supports object identifiers.")>
        SupportsObjectIds = &H10000


        ''' <summary>
        ''' The file system supports open by FileID. For more information, see IDBOTHDIRINFO.
        ''' </summary>
        <Description("The file system supports open by FileID. For more information, see IDBOTHDIRINFO.")>
        SupportsOpenById = &H1000000


        ''' <summary>
        ''' The specified volume supports reparse points.
        ''' </summary>
        <Description("The specified volume supports reparse points.")>
        SupportsReparsePoints = &H80


        ''' <summary>
        ''' The specified volume supports sparse files.
        ''' </summary>
        <Description("The specified volume supports sparse files.")>
        SupportsSparseFiles = &H40


        ''' <summary>
        ''' The specified volume supports transactions. For more information, see About KTM.
        ''' </summary>
        <Description("The specified volume supports transactions. For more information, see About KTM.")>
        SupportsTransactions = &H200000

        ''' <summary>
        ''' The specified volume supports update sequence number (USN) journals. For more information, see Change Journal Records.
        ''' </summary>
        <Description("The specified volume supports update sequence number (USN) journals. For more information, see Change Journal Records.")>
        SupportsUSNJournal = &H2000000

        ''' <summary>
        ''' The specified volume supports Unicode in file names as they appear on disk.
        ''' </summary>
        <Description("The specified volume supports Unicode in file names as they appear on disk.")>
        UnicodeOnDisk = &H4

        ''' <summary>
        ''' The specified volume is a compressed volume, for example, a DoubleSpace volume.
        ''' </summary>
        <Description("The specified volume is a compressed volume, for example, a DoubleSpace volume.")>
        VolumeIsCompressed = &H8000

        ''' <summary>
        ''' The specified volume supports disk quotas.
        ''' </summary>
        <Description("The specified volume supports disk quotas.")>
        VolumeQuotas = &H20

    End Enum

    ''' <summary>
    ''' Represents partition location information on a physical disk.
    ''' </summary>
    ''' <remarks></remarks>
    Public Structure DiskExtent

        ''' <summary>
        ''' The physical device number
        ''' </summary>
        ''' <remarks></remarks>
        Public PhysicalDevice As Integer

        ''' <summary>
        ''' Reserved
        ''' </summary>
        ''' <remarks></remarks>
        Public Space As Integer

        ''' <summary>
        ''' Physical byte offset on disk
        ''' </summary>
        ''' <remarks></remarks>
        Public Offset As Long

        ''' <summary>
        ''' Physical size in bytes on disk
        ''' </summary>
        ''' <remarks></remarks>
        Public Size As Long

        ''' <summary>
        ''' Presents this object in a readable string.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function ToString() As String
            Return "Physical Device " & PhysicalDevice & ", " & TextTools.PrintFriendlySize(Size)
        End Function

    End Structure


End Module