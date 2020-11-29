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
}
