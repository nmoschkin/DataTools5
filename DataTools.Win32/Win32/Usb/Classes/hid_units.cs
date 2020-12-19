// ************************************************* ''
// DataTools C# Native Utility Library For Windows - Interop
//
// Module: UsbHid
//         HID-related structures, enums and functions.
//
// Copyright (C) 2011-2020 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the MIT License   
// ************************************************* ''

using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace DataTools.Win32
{
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
