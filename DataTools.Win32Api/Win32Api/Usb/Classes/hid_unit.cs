// ************************************************* ''
// DataTools C# Native Utility Library For Windows - Interop
//
// Module: UsbHid
//         HID-related structures, enums and functions.
//
// Copyright (C) 2011-2020 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the Microsoft Public License   
// ************************************************* ''

using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace DataTools.Win32Api
{
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
}
