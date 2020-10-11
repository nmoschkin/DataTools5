'' ************************************************* ''
'' DataTools Visual Basic Utility Library - Interop
''
'' Module: UsbHid
''         HID-related structures, enums and functions.
''
'' Copyright (C) 2011-2020 Nathan Moschkin
'' All Rights Reserved
''
'' Licensed Under the Microsoft Public License   
'' ************************************************* ''

Imports System.Runtime.InteropServices
Imports CoreCT.Memory
Imports System.Reflection
Imports System.ComponentModel
Imports CoreCT.Text
Imports DataTools.Interop

Namespace Native


#Region "HID Subsystem"

    Friend Module UsbHid

        Public Class hid_usage_info

            Public Property UsageId As Integer

            Public Property UsageName As String

            Public Property UsageType As hid_usage_type

            Public Property Input As Boolean

            Public Property Output As Boolean

            Public Property Feature As Boolean

            Public Property Standard As String

        End Class


        Public Enum hid_power_usage_code As Byte

            Undefined = &H0
            iName = &H1
            PresentStatus = &H2
            ChangedStatus = &H3
            UPS = &H4
            PowerSupply = &H5
            Reserved1 = &H6
            Reserved2 = &H7
            Reserved3 = &H8
            Reserved4 = &H9
            Reserved5 = &HA
            Reserved6 = &HB
            Reserved7 = &HC
            Reserved8 = &HD
            Reserved9 = &HE
            Reserved10 = &HF
            BatterySystem = &H10
            BatterySystemID = &H11
            Battery = &H12
            BatteryID = &H13
            Charger = &H14
            ChargerID = &H15
            PowerConverter = &H16
            PowerConverterID = &H17
            OutletSystem = &H18
            OutletSystemID = &H19
            Input = &H1A
            InputID = &H1B
            Output = &H1C
            OutputID = &H1D
            Flow = &H1E
            FlowID = &H1F
            Outlet = &H20
            OutletID = &H21
            Gang = &H22
            GangID = &H23
            PowerSummary = &H24
            PowerSummaryID = &H25
            Reserved25 = &H2F
            Voltage = &H30
            Current = &H31
            Frequency = &H32
            ApparentPower = &H33
            ActivePower = &H34
            PercentLoad = &H35
            Temperature = &H36
            Humidity = &H37
            BadCount = &H38
            Reserved39 = &H3F
            ConfigVoltage = &H40
            ConfigCurrent = &H41
            ConfigFrequency = &H42
            ConfigApparentPower = &H43
            ConfigActivePower = &H44
            ConfigPercentLoad = &H45
            ConfigTemperature = &H46
            ConfigHumidity = &H47
            Reserved38 = &H4F
            SwitchOnControl = &H50
            SwitchOffControl = &H51
            ToggleControl = &H52
            LowVoltageTransfer = &H53
            HighVoltageTransfer = &H54
            DelayBeforeReboot = &H55
            DelayBeforeStartup = &H56
            DelayBeforeShutdown = &H57
            Test = &H58
            ModuleReset = &H59
            AudibleAlarmControl = &H5A
            Reserved11 = &H5B
            Reserved12 = &H5C
            Reserved13 = &H5D
            Reserved14 = &H5E
            Reserved15 = &H5F
            Present = &H60
            Good = &H61
            InternalFailure = &H62
            VoltageOutOfRange = &H63
            FrequencyOutOfRange = &H64
            Overload = &H65
            OverCharged = &H66
            OverTemperature = &H67
            ShutdownRequested = &H68
            ShutdownImminent = &H69
            Reserved16 = &H6A
            SwitchOn = &H6B
            SwitchOff = &H6B
            Switchable = &H6C
            Used = &H6D
            Boost = &H6E
            Buck = &H6F
            Initialized = &H70
            Tested = &H71
            AwaitingPower = &H72
            CommunicationLost = &H73
            Reserved7Min = &H74
            Reserved7Max = &HFC
            iManufacturer = &HFD
            iProduct = &HFE
            iserialNumber = &HFF
        End Enum

        <StructLayout(LayoutKind.Sequential, Pack:=gPack)>
        Public Structure usb_hid_descriptor

            ''' <summary>
            ''' Size of this descriptor in bytes.
            ''' </summary>
            ''' <remarks>0x09</remarks>
            Public bLength As Byte

            ''' <summary>
            ''' HID descriptor type (assigned by USB).
            ''' </summary>
            ''' <remarks>0x21</remarks>
            Public bDescriptorType As Byte

            ''' <summary>
            ''' HID Class Specification release number in binarycoded decimal. For example, 2.10 is 0x210.
            ''' </summary>
            ''' <remarks>0x100</remarks>
            Public bcdHID As Short

            ''' <summary>
            ''' Hardware target country.
            ''' </summary>
            ''' <remarks>0x00</remarks>
            Public bCountryCode As Byte

            ''' <summary>
            ''' Number of HID class descriptors to follow.
            ''' </summary>
            ''' <remarks>0x01</remarks>
            Public bNumDescriptors As Byte

            ''' <summary>
            ''' Report descriptor type.
            ''' </summary>
            ''' <remarks>0x22</remarks>
            Public bReportDescriptorType As Byte

            ''' <summary>
            ''' Total length of Report descriptor.
            ''' </summary>
            ''' <remarks>0x????</remarks>
            Public wDescriptorLength As Short

        End Structure

        Public Enum hid_usage_type

            '''<summary>
            ''' Application collection
            ''' </summary>
            ''' <remarks></remarks>
            CA

            '''<summary>
            ''' Logical collection
            ''' </summary>
            ''' <remarks></remarks>
            CL

            '''<summary>
            ''' Physical collection
            ''' </summary>
            ''' <remarks></remarks>
            CP

            '''<summary>
            ''' Dynamic Flag
            ''' </summary>
            ''' <remarks></remarks>
            DF

            '''<summary>
            ''' Dynamic Value
            ''' </summary>
            ''' <remarks></remarks>
            DV

            '''<summary>
            ''' Static Flag
            ''' </summary>
            ''' <remarks></remarks>
            SF

            '''<summary>
            ''' Static Value
            ''' </summary>
            ''' <remarks></remarks>
            SV

        End Enum

        Public Class hid_unit

            Protected _physical As String
            Protected _hidunit As String
            Protected _unitcode As Integer
            Protected _exponent As Integer
            Protected _size As Integer
            Protected _desc As String

            Public Overrides Function ToString() As String
                Return description
            End Function

            Public ReadOnly Property description As String
                Get
                    Return _desc
                End Get
            End Property

            Public ReadOnly Property PhysicalUnit As String
                Get
                    Return _physical
                End Get
            End Property

            Public ReadOnly Property HIDUnit As String
                Get
                    Return _hidunit
                End Get
            End Property

            Public ReadOnly Property HIDUnitCode As Integer
                Get
                    Return _unitcode
                End Get
            End Property

            Public ReadOnly Property HIDUnitExponent As Integer
                Get
                    Return _exponent
                End Get
            End Property

            Public ReadOnly Property HIDSize As Integer
                Get
                    Return _size
                End Get
            End Property

            Friend Sub New(d As String, p As String, h As String, u As Integer, e As Integer, s As Integer)
                _desc = d
                _physical = p
                _hidunit = h
                _unitcode = u
                _exponent = e
                _size = s
            End Sub

        End Class

        Public Class hid_units

            Private Shared _units As New List(Of hid_unit)

            Public Shared ReadOnly Property units As List(Of hid_unit)
                Get
                    Return _units
                End Get
            End Property

            Shared Sub New()
                _units.Add(New hid_unit("AC Voltage", "Volt", "Volt", &HF0D121, 7, 8))
                _units.Add(New hid_unit("AC Current", "centiAmp", "Amp", &H100001, -2, 16))
                _units.Add(New hid_unit("Frequency", "Hertz", "Hertz", &HF001, 0, 8))
                _units.Add(New hid_unit("DC Voltage", "centiVolt", "Volt", &HF0D121, 5, 16))
                _units.Add(New hid_unit("Time", "second", "s", &H1001, 0, 16))
                _units.Add(New hid_unit("DC Current", "centiAmp", "Amp", &H100001, -2, 16))
                _units.Add(New hid_unit("Apparent or Active Power", "VA or W", "VA or W", &HD121, 7, 16))
                _units.Add(New hid_unit("Temperature", "°K", "°K", &H10001, 0, 16))
                _units.Add(New hid_unit("Battery Capacity", "AmpSec", "AmpSec", &H101001, 0, 24))
                _units.Add(New hid_unit("None", "None", "None", &H0, 0, 8))
            End Sub

            Public Shared Function ByUnitCode(code As Integer) As hid_unit
                For Each hid In _units
                    If hid.HIDUnitCode = code Then Return hid
                Next
                Return Nothing
            End Function

            Private Sub New()

            End Sub

        End Class

    End Module

#End Region

End Namespace