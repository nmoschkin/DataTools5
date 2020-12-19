// ************************************************* ''
// DataTools C# Native Utility Library For Windows - Interop
//
// Module: DevProp
//         Native Device Properites.
// 
// Copyright (C) 2011-2020 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the Microsoft Public License   
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
    /// Bus types.
    /// </summary>
    /// <remarks></remarks>
    public enum BusType
    {

        /// <summary>
        /// Internal
        /// </summary>
        Internal,

        /// <summary>
        /// PCMCIA
        /// </summary>
        PCMCIA,

        /// <summary>
        /// PCI
        /// </summary>
        PCI,

        /// <summary>
        /// ISAPnP
        /// </summary>
        ISAPnP,

        /// <summary>
        /// EISA
        /// </summary>
        EISA,

        /// <summary>
        /// MCA
        /// </summary>
        MCA,

        /// <summary>
        /// Serenum
        /// </summary>
        Serenum,

        /// <summary>
        /// USB
        /// </summary>
        USB,

        /// <summary>
        /// Parallel Port
        /// </summary>
        ParallelPort,

        /// <summary>
        /// UsB Printer
        /// </summary>
        USBPrinter,

        /// <summary>
        /// DOT4 Dotmatrix Printer
        /// </summary>
        DOT4Printer,

        /// <summary>
        /// IEEE 1394 / Firewire
        /// </summary>
        Bus1394,

        /// <summary>
        /// Human Interface Device
        /// </summary>
        HID,

        /// <summary>
        /// AVC
        /// </summary>
        AVC,

        /// <summary>
        /// Infrared (IRDA) device
        /// </summary>
        IRDA,

        /// <summary>
        /// MicroSD card
        /// </summary>
        SD,

        /// <summary>
        /// ACPI
        /// </summary>
        ACPI,

        /// <summary>
        /// Software Device
        /// </summary>
        SoftwareDevice
    }
}
