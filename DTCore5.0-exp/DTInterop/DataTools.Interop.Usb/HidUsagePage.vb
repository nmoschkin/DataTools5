'' ************************************************* ''
'' DataTools Visual Basic Utility Library - Interop
''
'' Module: HidUsagePage
''         Hid Usage Page Enum.
''
''         Enums are documented in part from the API documentation at MSDN.
''         Other knowledge and references obtained through various sources
''         and all is considered public domain/common knowledge.
''
'' Copyright (C) 2011-2020 Nathan Moschkin
'' All Rights Reserved
''
'' Licensed Under the Microsoft Public License   
'' ************************************************* ''

Imports System.ComponentModel

Namespace Usb

    ''' <summary>
    ''' Human Interface Device Usage Pages.
    ''' </summary>
    ''' <remarks>
    ''' Source: USB Consortium, HID 1.1 spec.
    ''' </remarks>
    Public Enum HidUsagePage As UShort
        ''' <summary>
        ''' Undefined
        ''' </summary>
        <Description("Undefined")>
        Undefined = &H0

        ''' <summary>
        ''' Generic Desktop Controls
        ''' </summary>
        <Description("Generic Desktop Controls")>
        GenericDesktopControls = &H1

        ''' <summary>
        ''' Simulation Controls
        ''' </summary>
        <Description("Simulation Controls")>
        SimulationControls = &H2

        ''' <summary>
        ''' VR Controls
        ''' </summary>
        <Description("VR Controls")>
        VRControls = &H3

        ''' <summary>
        ''' Sport Controls
        ''' </summary>
        <Description("Sport Controls")>
        SportControls = &H4

        ''' <summary>
        ''' Game Controls
        ''' </summary>
        <Description("Game Controls")>
        GameControls = &H5

        ''' <summary>
        ''' Generic Device Controls
        ''' </summary>
        <Description("Generic Device Controls")>
        GenericDeviceControls = &H6

        ''' <summary>
        ''' Keyboard/Keypad
        ''' </summary>
        <Description("Keyboard/Keypad")>
        KeyboardKeypad = &H7

        ''' <summary>
        ''' LEDs
        ''' </summary>
        <Description("LEDs")>
        LEDs = &H8

        ''' <summary>
        ''' Button
        ''' </summary>
        <Description("Button")>
        Button = &H9

        ''' <summary>
        ''' Ordinal
        ''' </summary>
        <Description("Ordinal")>
        Ordinal = &HA

        ''' <summary>
        ''' Telephony
        ''' </summary>
        <Description("Telephony")>
        Telephony = &HB

        ''' <summary>
        ''' Consumer
        ''' </summary>
        <Description("Consumer")>
        Consumer = &HC

        ''' <summary>
        ''' Digitizer
        ''' </summary>
        <Description("Digitizer")>
        Digitizer = &HD

        ''' <summary>
        ''' Reserved
        ''' </summary>
        <Description("Reserved")>
        Reserved = &HE

        ''' <summary>
        ''' PID Page
        ''' </summary>
        <Description("PID Page")>
        PIDPage = &HF

        ''' <summary>
        ''' Unicode
        ''' </summary>
        <Description("Unicode")>
        Unicode = &H10

        ''' <summary>
        ''' Beginning of First Reserved Block
        ''' </summary>
        <Description("Beginning of First Reserved Block")>
        Reserved1Begin = &H11

        ''' <summary>
        ''' End of First Reserved Block
        ''' </summary>
        <Description("End of First Reserved Block")>
        Reserved1End = &H13

        ''' <summary>
        ''' Alphanumeric Display
        ''' </summary>
        <Description("Alphanumeric Display")>
        AlphanumericDisplay = &H14

        ''' <summary>
        ''' Beginning of Second Reserved Block
        ''' </summary>
        <Description("Beginning of Second Reserved Block")>
        Reserved2Begin = &H15

        ''' <summary>
        ''' End of Second Reserved Block
        ''' </summary>
        <Description("End of Second Reserved Block")>
        Reserved2End = &H3F

        ''' <summary>
        ''' Medical Instruments
        ''' </summary>
        <Description("Medical Instruments")>
        MedicalInstruments = &H40

        ''' <summary>
        ''' Beginning of Third Reserved Block
        ''' </summary>
        <Description("Beginning of Third Reserved Block")>
        Reserved3Begin = &H41

        ''' <summary>
        ''' End of Third Reserved Block
        ''' </summary>
        <Description("End of Third Reserved Block")>
        Reserved3End = &H7F

        ''' <summary>
        ''' Monitor Device
        ''' </summary>
        <Description("Monitor Device")>
        MonitorDevice1 = &H80

        ''' <summary>
        ''' Monitor Device
        ''' </summary>
        <Description("Monitor Device")>
        MonitorDevice2 = &H81

        ''' <summary>
        ''' Monitor Device
        ''' </summary>
        <Description("Monitor Device")>
        MonitorDevice3 = &H82

        ''' <summary>
        ''' Monitor Device
        ''' </summary>
        <Description("Monitor Device")>
        MonitorDevice4 = &H83

        ''' <summary>
        ''' Power Device
        ''' </summary>
        <Description("Power Device")>
        PowerDevice1 = &H84

        ''' <summary>
        ''' Power Device
        ''' </summary>
        <Description("Power Device")>
        PowerDevice2 = &H85

        ''' <summary>
        ''' Power Device
        ''' </summary>
        <Description("Power Device")>
        PowerDevice3 = &H86

        ''' <summary>
        ''' Power Device
        ''' </summary>
        <Description("Power Device")>
        PowerDevice4 = &H87

        ''' <summary>
        ''' Beginning of Fourth Reserved Block
        ''' </summary>
        <Description("Beginning of Fourth Reserved Block")>
        Reserved4Begin = &H88

        ''' <summary>
        ''' End of Fourth Reserved Block
        ''' </summary>
        <Description("End of Fourth Reserved Block")>
        Reserved4End = &H8B

        ''' <summary>
        ''' Bar Code Scanner 
        ''' </summary>
        <Description("Bar Code Scanner")>
        BarCodeScanner = &H8C

        ''' <summary>
        ''' Scale
        ''' </summary>
        <Description("Scale")>
        Scale = &H8D

        ''' <summary>
        ''' Magnetic Stripe Reading (MSR) Device
        ''' </summary>
        <Description("Magnetic Stripe Reading (MSR) Device")>
        MagneticStripeReadingDevice = &H8E

        ''' <summary>
        ''' Point of Sale pages
        ''' </summary>
        <Description("Point of Sale")>
        PointOfSale = &H8F

        ''' <summary>
        ''' Camera Control
        ''' </summary>
        <Description("Camera Control")>
        CameraControl = &H90

    End Enum

End Namespace