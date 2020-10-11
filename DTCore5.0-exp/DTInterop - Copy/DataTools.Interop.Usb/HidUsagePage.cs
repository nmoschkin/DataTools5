// ' ************************************************* ''
// ' DataTools Visual Basic Utility Library - Interop
// '
// ' Module: HidUsagePage
// '         Hid Usage Page Enum.
// '
// '         Enums are documented in part from the API documentation at MSDN.
// '         Other knowledge and references obtained through various sources
// '         and all is considered public domain/common knowledge.
// '
// ' Copyright (C) 2011-2020 Nathan Moschkin
// ' All Rights Reserved
// '
// ' Licensed Under the Microsoft Public License   
// ' ************************************************* ''

using System.ComponentModel;

namespace DataTools.Interop.Usb
{

    /// <summary>
    /// Human Interface Device Usage Pages.
    /// </summary>
    /// <remarks>
    /// Source: USB Consortium, HID 1.1 spec.
    /// </remarks>
    public enum HidUsagePage : ushort
    {
        /// <summary>
        /// Undefined
        /// </summary>
        [Description("Undefined")]
        Undefined = 0x0,

        /// <summary>
        /// Generic Desktop Controls
        /// </summary>
        [Description("Generic Desktop Controls")]
        GenericDesktopControls = 0x1,

        /// <summary>
        /// Simulation Controls
        /// </summary>
        [Description("Simulation Controls")]
        SimulationControls = 0x2,

        /// <summary>
        /// VR Controls
        /// </summary>
        [Description("VR Controls")]
        VRControls = 0x3,

        /// <summary>
        /// Sport Controls
        /// </summary>
        [Description("Sport Controls")]
        SportControls = 0x4,

        /// <summary>
        /// Game Controls
        /// </summary>
        [Description("Game Controls")]
        GameControls = 0x5,

        /// <summary>
        /// Generic Device Controls
        /// </summary>
        [Description("Generic Device Controls")]
        GenericDeviceControls = 0x6,

        /// <summary>
        /// Keyboard/Keypad
        /// </summary>
        [Description("Keyboard/Keypad")]
        KeyboardKeypad = 0x7,

        /// <summary>
        /// LEDs
        /// </summary>
        [Description("LEDs")]
        LEDs = 0x8,

        /// <summary>
        /// Button
        /// </summary>
        [Description("Button")]
        Button = 0x9,

        /// <summary>
        /// Ordinal
        /// </summary>
        [Description("Ordinal")]
        Ordinal = 0xA,

        /// <summary>
        /// Telephony
        /// </summary>
        [Description("Telephony")]
        Telephony = 0xB,

        /// <summary>
        /// Consumer
        /// </summary>
        [Description("Consumer")]
        Consumer = 0xC,

        /// <summary>
        /// Digitizer
        /// </summary>
        [Description("Digitizer")]
        Digitizer = 0xD,

        /// <summary>
        /// Reserved
        /// </summary>
        [Description("Reserved")]
        Reserved = 0xE,

        /// <summary>
        /// PID Page
        /// </summary>
        [Description("PID Page")]
        PIDPage = 0xF,

        /// <summary>
        /// Unicode
        /// </summary>
        [Description("Unicode")]
        Unicode = 0x10,

        /// <summary>
        /// Beginning of First Reserved Block
        /// </summary>
        [Description("Beginning of First Reserved Block")]
        Reserved1Begin = 0x11,

        /// <summary>
        /// End of First Reserved Block
        /// </summary>
        [Description("End of First Reserved Block")]
        Reserved1End = 0x13,

        /// <summary>
        /// Alphanumeric Display
        /// </summary>
        [Description("Alphanumeric Display")]
        AlphanumericDisplay = 0x14,

        /// <summary>
        /// Beginning of Second Reserved Block
        /// </summary>
        [Description("Beginning of Second Reserved Block")]
        Reserved2Begin = 0x15,

        /// <summary>
        /// End of Second Reserved Block
        /// </summary>
        [Description("End of Second Reserved Block")]
        Reserved2End = 0x3F,

        /// <summary>
        /// Medical Instruments
        /// </summary>
        [Description("Medical Instruments")]
        MedicalInstruments = 0x40,

        /// <summary>
        /// Beginning of Third Reserved Block
        /// </summary>
        [Description("Beginning of Third Reserved Block")]
        Reserved3Begin = 0x41,

        /// <summary>
        /// End of Third Reserved Block
        /// </summary>
        [Description("End of Third Reserved Block")]
        Reserved3End = 0x7F,

        /// <summary>
        /// Monitor Device
        /// </summary>
        [Description("Monitor Device")]
        MonitorDevice1 = 0x80,

        /// <summary>
        /// Monitor Device
        /// </summary>
        [Description("Monitor Device")]
        MonitorDevice2 = 0x81,

        /// <summary>
        /// Monitor Device
        /// </summary>
        [Description("Monitor Device")]
        MonitorDevice3 = 0x82,

        /// <summary>
        /// Monitor Device
        /// </summary>
        [Description("Monitor Device")]
        MonitorDevice4 = 0x83,

        /// <summary>
        /// Power Device
        /// </summary>
        [Description("Power Device")]
        PowerDevice1 = 0x84,

        /// <summary>
        /// Power Device
        /// </summary>
        [Description("Power Device")]
        PowerDevice2 = 0x85,

        /// <summary>
        /// Power Device
        /// </summary>
        [Description("Power Device")]
        PowerDevice3 = 0x86,

        /// <summary>
        /// Power Device
        /// </summary>
        [Description("Power Device")]
        PowerDevice4 = 0x87,

        /// <summary>
        /// Beginning of Fourth Reserved Block
        /// </summary>
        [Description("Beginning of Fourth Reserved Block")]
        Reserved4Begin = 0x88,

        /// <summary>
        /// End of Fourth Reserved Block
        /// </summary>
        [Description("End of Fourth Reserved Block")]
        Reserved4End = 0x8B,

        /// <summary>
        /// Bar Code Scanner
        /// </summary>
        [Description("Bar Code Scanner")]
        BarCodeScanner = 0x8C,

        /// <summary>
        /// Scale
        /// </summary>
        [Description("Scale")]
        Scale = 0x8D,

        /// <summary>
        /// Magnetic Stripe Reading (MSR) Device
        /// </summary>
        [Description("Magnetic Stripe Reading (MSR) Device")]
        MagneticStripeReadingDevice = 0x8E,

        /// <summary>
        /// Point of Sale pages
        /// </summary>
        [Description("Point of Sale")]
        PointOfSale = 0x8F,

        /// <summary>
        /// Camera Control
        /// </summary>
        [Description("Camera Control")]
        CameraControl = 0x90
    }
}