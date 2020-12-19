// ************************************************* ''
// DataTools C# Native Utility Library For Windows - Interop
//
// Module: DevProp
//         Native Device Properites.
// 
// Copyright (C) 2011-2020 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the Microsoft Public License   
// ************************************************* ''

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using DataTools.Text;

namespace DataTools.Win32
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct DEVPROPKEY
    {
        [MarshalAs(UnmanagedType.Struct)]
        public Guid fmtid;
        public uint pid;

        public DEVPROPKEY(uint ui1, ushort s1, short s2, byte b1, byte b2, byte b3, byte b4, byte b5, byte b6, byte b7, byte b8, uint pid)
        {
            fmtid = new Guid((int)ui1, (short)s1, s2, b1, b2, b3, b4, b5, b6, b7, b8);
            this.pid = pid;
        }

        public override string ToString()
        {
            return DevProp.GetKeyName(this);
        }

        public bool Equals(DEVPROPKEY obj)
        {
            return obj.fmtid.ToString() == fmtid.ToString() && obj.pid == pid;
        }

        public static bool operator ==(DEVPROPKEY operand1, DEVPROPKEY operand2)
        {
            return operand1.Equals(operand2);
        }

        public static bool operator !=(DEVPROPKEY operand1, DEVPROPKEY operand2)
        {
            return !operand1.Equals(operand2);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
}
