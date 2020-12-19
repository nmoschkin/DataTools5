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
    public struct SOCKET_ADDRESS
    {
        public LPSOCKADDR lpSockaddr;
        public int iSockaddrLength;

        public override string ToString()
        {
            string ToStringRet = default;
            if (lpSockaddr.Handle.Handle == IntPtr.Zero)
                return "NULL";
            ToStringRet = lpSockaddr.ToString();
            return ToStringRet;
        }
    }
}
