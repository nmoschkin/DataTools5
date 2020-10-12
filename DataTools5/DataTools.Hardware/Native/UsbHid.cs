// ' ************************************************* ''
// ' DataTools Visual Basic Utility Library - Interop
// '
// ' Module: UsbHid
// '         HID-related structures, enums and functions.
// '
// ' Copyright (C) 2011-2020 Nathan Moschkin
// ' All Rights Reserved
// '
// ' Licensed Under the Microsoft Public License   
// ' ************************************************* ''

using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace DataTools.Hardware.Native
{


    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    internal static class UsbHid
    {
        public class hid_usage_info
        {
            public int UsageId { get; set; }
            public string UsageName { get; set; }
            public hid_usage_type UsageType { get; set; }
            public bool Input { get; set; }
            public bool Output { get; set; }
            public bool Feature { get; set; }
            public string Standard { get; set; }
        }

        public enum hid_power_usage_code : byte
        {
            Undefined = 0x0,
            iName = 0x1,
            PresentStatus = 0x2,
            ChangedStatus = 0x3,
            UPS = 0x4,
            PowerSupply = 0x5,
            Reserved1 = 0x6,
            Reserved2 = 0x7,
            Reserved3 = 0x8,
            Reserved4 = 0x9,
            Reserved5 = 0xA,
            Reserved6 = 0xB,
            Reserved7 = 0xC,
            Reserved8 = 0xD,
            Reserved9 = 0xE,
            Reserved10 = 0xF,
            BatterySystem = 0x10,
            BatterySystemID = 0x11,
            Battery = 0x12,
            BatteryID = 0x13,
            Charger = 0x14,
            ChargerID = 0x15,
            PowerConverter = 0x16,
            PowerConverterID = 0x17,
            OutletSystem = 0x18,
            OutletSystemID = 0x19,
            Input = 0x1A,
            InputID = 0x1B,
            Output = 0x1C,
            OutputID = 0x1D,
            Flow = 0x1E,
            FlowID = 0x1F,
            Outlet = 0x20,
            OutletID = 0x21,
            Gang = 0x22,
            GangID = 0x23,
            PowerSummary = 0x24,
            PowerSummaryID = 0x25,
            Reserved25 = 0x2F,
            Voltage = 0x30,
            Current = 0x31,
            Frequency = 0x32,
            ApparentPower = 0x33,
            ActivePower = 0x34,
            PercentLoad = 0x35,
            Temperature = 0x36,
            Humidity = 0x37,
            BadCount = 0x38,
            Reserved39 = 0x3F,
            ConfigVoltage = 0x40,
            ConfigCurrent = 0x41,
            ConfigFrequency = 0x42,
            ConfigApparentPower = 0x43,
            ConfigActivePower = 0x44,
            ConfigPercentLoad = 0x45,
            ConfigTemperature = 0x46,
            ConfigHumidity = 0x47,
            Reserved38 = 0x4F,
            SwitchOnControl = 0x50,
            SwitchOffControl = 0x51,
            ToggleControl = 0x52,
            LowVoltageTransfer = 0x53,
            HighVoltageTransfer = 0x54,
            DelayBeforeReboot = 0x55,
            DelayBeforeStartup = 0x56,
            DelayBeforeShutdown = 0x57,
            Test = 0x58,
            ModuleReset = 0x59,
            AudibleAlarmControl = 0x5A,
            Reserved11 = 0x5B,
            Reserved12 = 0x5C,
            Reserved13 = 0x5D,
            Reserved14 = 0x5E,
            Reserved15 = 0x5F,
            Present = 0x60,
            Good = 0x61,
            InternalFailure = 0x62,
            VoltageOutOfRange = 0x63,
            FrequencyOutOfRange = 0x64,
            Overload = 0x65,
            OverCharged = 0x66,
            OverTemperature = 0x67,
            ShutdownRequested = 0x68,
            ShutdownImminent = 0x69,
            Reserved16 = 0x6A,
            SwitchOn = 0x6B,
            SwitchOff = 0x6B,
            Switchable = 0x6C,
            Used = 0x6D,
            Boost = 0x6E,
            Buck = 0x6F,
            Initialized = 0x70,
            Tested = 0x71,
            AwaitingPower = 0x72,
            CommunicationLost = 0x73,
            Reserved7Min = 0x74,
            Reserved7Max = 0xFC,
            iManufacturer = 0xFD,
            iProduct = 0xFE,
            iserialNumber = 0xFF
        }

        [StructLayout(LayoutKind.Sequential, Pack = libusb.gPack)]
        public struct usb_hid_descriptor
        {

            /// <summary>
            /// Size of this descriptor in bytes.
            /// </summary>
            /// <remarks>0x09</remarks>
            public byte bLength;

            /// <summary>
            /// HID descriptor type (assigned by USB).
            /// </summary>
            /// <remarks>0x21</remarks>
            public byte bDescriptorType;

            /// <summary>
            /// HID Class Specification release number in binarycoded decimal. For example, 2.10 is 0x210.
            /// </summary>
            /// <remarks>0x100</remarks>
            public short bcdHID;

            /// <summary>
            /// Hardware target country.
            /// </summary>
            /// <remarks>0x00</remarks>
            public byte bCountryCode;

            /// <summary>
            /// Number of HID class descriptors to follow.
            /// </summary>
            /// <remarks>0x01</remarks>
            public byte bNumDescriptors;

            /// <summary>
            /// Report descriptor type.
            /// </summary>
            /// <remarks>0x22</remarks>
            public byte bReportDescriptorType;

            /// <summary>
            /// Total length of Report descriptor.
            /// </summary>
            /// <remarks>0x????</remarks>
            public short wDescriptorLength;
        }

        public enum hid_usage_type
        {

            /// <summary>
            /// Application collection
            /// </summary>
            /// <remarks></remarks>
            CA,

            /// <summary>
            /// Logical collection
            /// </summary>
            /// <remarks></remarks>
            CL,

            /// <summary>
            /// Physical collection
            /// </summary>
            /// <remarks></remarks>
            CP,

            /// <summary>
            /// Dynamic Flag
            /// </summary>
            /// <remarks></remarks>
            DF,

            /// <summary>
            /// Dynamic Value
            /// </summary>
            /// <remarks></remarks>
            DV,

            /// <summary>
            /// Static Flag
            /// </summary>
            /// <remarks></remarks>
            SF,

            /// <summary>
            /// Static Value
            /// </summary>
            /// <remarks></remarks>
            SV
        }

        public class hid_unit
        {
            protected string _physical;
            protected string _hidunit;
            protected int _unitcode;
            protected int _exponent;
            protected int _size;
            protected string _desc;

            public override string ToString()
            {
                return description;
            }

            public string description
            {
                get
                {
                    return _desc;
                }
            }

            public string PhysicalUnit
            {
                get
                {
                    return _physical;
                }
            }

            public string HIDUnit
            {
                get
                {
                    return _hidunit;
                }
            }

            public int HIDUnitCode
            {
                get
                {
                    return _unitcode;
                }
            }

            public int HIDUnitExponent
            {
                get
                {
                    return _exponent;
                }
            }

            public int HIDSize
            {
                get
                {
                    return _size;
                }
            }

            internal hid_unit(string d, string p, string h, int u, int e, int s)
            {
                _desc = d;
                _physical = p;
                _hidunit = h;
                _unitcode = u;
                _exponent = e;
                _size = s;
            }
        }

        public class hid_units
        {
            private static List<hid_unit> _units = new List<hid_unit>();

            public static List<hid_unit> units
            {
                get
                {
                    return _units;
                }
            }

            static hid_units()
            {
                _units.Add(new hid_unit("AC Voltage", "Volt", "Volt", 0xF0D121, 7, 8));
                _units.Add(new hid_unit("AC Current", "centiAmp", "Amp", 0x100001, -2, 16));
                _units.Add(new hid_unit("Frequency", "Hertz", "Hertz", 0xF001, 0, 8));
                _units.Add(new hid_unit("DC Voltage", "centiVolt", "Volt", 0xF0D121, 5, 16));
                _units.Add(new hid_unit("Time", "second", "s", 0x1001, 0, 16));
                _units.Add(new hid_unit("DC Current", "centiAmp", "Amp", 0x100001, -2, 16));
                _units.Add(new hid_unit("Apparent or Active Power", "VA or W", "VA or W", 0xD121, 7, 16));
                _units.Add(new hid_unit("Temperature", "°K", "°K", 0x10001, 0, 16));
                _units.Add(new hid_unit("Battery Capacity", "AmpSec", "AmpSec", 0x101001, 0, 24));
                _units.Add(new hid_unit("None", "None", "None", 0x0, 0, 8));
            }

            public static hid_unit ByUnitCode(int code)
            {
                foreach (var hid in _units)
                {
                    if (hid.HIDUnitCode == code)
                        return hid;
                }

                return null;
            }

            private hid_units()
            {
            }
        }
    }

    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
}