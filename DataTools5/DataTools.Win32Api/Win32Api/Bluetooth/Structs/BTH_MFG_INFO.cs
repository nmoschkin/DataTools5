// ************************************************* ''
// DataTools C# Native Utility Library For Windows 
//
// Module: Bluetooth API
//         Complete Translation of
//         BluetoothAPI.h
// 
// Copyright (C) 2011-2020 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the Microsoft Public License   
// ************************************************* ''

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using DataTools.Memory;
using DataTools.Text;

namespace DataTools.Win32Api
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct BTH_MFG_INFO
    {
        private static Dictionary<ushort, string> Manufacturers = new Dictionary<ushort, string>();
        private ushort _val;

        public ushort Value
        {
            get
            {
                return _val;
            }
        }

        public string Name
        {
            get
            {
                return ToString();
            }
        }

        internal BTH_MFG_INFO(string name, ushort value)
        {
            _val = value;
            if (!Manufacturers.ContainsKey(value))
                Manufacturers.Add(value, name);
        }

        public override string ToString()
        {
            if (Manufacturers.ContainsKey(_val))
                return string.Format("{0}: {1}", _val, Manufacturers[_val]);
            else
                return _val.ToString();
        }

        public static implicit operator ushort(BTH_MFG_INFO val1)
        {
            return val1._val;
        }

        public static explicit operator BTH_MFG_INFO(ushort val1)
        {
            BTH_MFG_INFO b;
            b._val = val1;
            return b;
        }
    }
}
