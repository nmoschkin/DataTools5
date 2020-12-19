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

namespace DataTools.Win32
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
}
