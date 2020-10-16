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
// Licensed Under the Microsoft Public License   
// ************************************************* ''


using System;
using System.ComponentModel;
using System.Net;
using System.Runtime.InteropServices;
using DataTools.Memory;
using DataTools.Win32Api;

namespace DataTools.Win32Api.Network
{
    /// <summary>
    /// Represents a network adapter MAC address.
    /// </summary>
    /// <remarks></remarks>
    [StructLayout(LayoutKind.Sequential)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public struct MACADDRESS
    {
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = IfDefApi.MAX_ADAPTER_ADDRESS_LENGTH)]
        public byte[] Data;

        public override string ToString()
        {
            string ToStringRet = default;
            string s = "";
            byte b;
            if (Data is null)
                return "NULL";

            // let's get a clean string without extraneous zeros at the end:

            int i;
            int c = Data.Length - 1;
            for (i = c; i >= 0; i -= 1)
            {
                if (Data[i] != 0)
                    break;
            }

            c = i;
            i = 0;
            var loopTo = c;
            for (i = 0; i <= loopTo; i++)
            {
                b = Data[i];
                if (!string.IsNullOrEmpty(s))
                    s += ":";
                s += b.ToString("X2");
            }

            if (string.IsNullOrEmpty(s))
                s = "NULL";
            ToStringRet = s;
            return ToStringRet;
        }
    }
}
