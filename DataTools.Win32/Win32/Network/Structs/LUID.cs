// ************************************************* ''
// DataTools C# Native Utility Library For Windows - Interop
//
// Module: IfDefApi
//         The almighty network interface native API.
//         Some enum documentation comes from the MSDN.
//
// (and an exercise in creative problem solving and data-structure marshaling.)
//
// Copyright (C) 2011-2020 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the MIT License   
// ************************************************* ''


using System;
using System.ComponentModel;
using System.Net;
using System.Runtime.InteropServices;

using DataTools.Win32;

namespace DataTools.Win32.Network
{
    [StructLayout(LayoutKind.Sequential)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public struct LUID
    {
        public ulong Value;

        public int Reserved
        {
            get
            {
                return (int)((long)Value & 0xFFFFFFL);
            }
        }

        public int NetLuidIndex
        {
            get
            {
                return (int)((long)(Value >> 24) & 0xFFFFFFL);
            }
        }

        public IFTYPE IfType
        {
            get
            {
                return (IFTYPE)(int)((long)(Value >> 48) & 0xFFFFL);
            }
        }

        public override string ToString()
        {
            return IfType.ToString();
        }
    }
}
