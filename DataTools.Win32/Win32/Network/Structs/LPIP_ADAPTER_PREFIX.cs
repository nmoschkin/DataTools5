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
using DataTools.Win32.Memory;

namespace DataTools.Win32.Network
{
    [StructLayout(LayoutKind.Sequential)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public struct LPIP_ADAPTER_PREFIX
    {
        public MemPtr Handle;

        public LPIP_ADAPTER_PREFIX[] AddressChain
        {
            get
            {
                if (Handle == IntPtr.Zero)
                    return null;
                int c = 0;
                var mx = this;
                var ac = default(LPIP_ADAPTER_PREFIX[]);
                do
                {
                    Array.Resize(ref ac, c + 1);
                    ac[c] = mx;
                    mx = mx.Next;
                    c += 1;
                }
                while (mx.Handle.Handle != IntPtr.Zero);
                return ac;
            }
        }

        public LPIP_ADAPTER_PREFIX Next
        {
            get
            {
                if (Handle == IntPtr.Zero)
                    return default;
                return Struct.Next;
            }
        }

        public override string ToString()
        {
            if (Handle.Handle == IntPtr.Zero)
                return "NULL";
            return "" + IPAddress.ToString() + " (" + AddressFamily.ToString() + ")";
        }

        public IPAddress IPAddress
        {
            get
            {
                if (Handle == IntPtr.Zero)
                    return null;
                return Struct.Address.lpSockaddr.IPAddress;
            }
        }

        public IP_ADAPTER_PREFIX Struct
        {
            get
            {
                IP_ADAPTER_PREFIX StructRet = default;
                if (Handle == IntPtr.Zero)
                    return default;
                StructRet = ToAddress();
                return StructRet;
            }
        }

        public IP_ADAPTER_PREFIX ToAddress()
        {
            IP_ADAPTER_PREFIX ToAddressRet = default;
            if (Handle == IntPtr.Zero)
                return default;
            ToAddressRet = Handle.ToStruct<IP_ADAPTER_PREFIX>();
            return ToAddressRet;
        }

        public void Dispose()
        {
            Handle.Free();
        }

        public AddressFamily AddressFamily
        {
            get
            {
                if (Handle == IntPtr.Zero)
                    return default;
                return Struct.Address.lpSockaddr.AddressFamily;
            }
        }

        public byte[] Data
        {
            get
            {
                if (Handle == IntPtr.Zero)
                    return null;
                return Struct.Address.lpSockaddr.Data;
            }
        }

        public static implicit operator LPIP_ADAPTER_PREFIX(IntPtr operand)
        {
            var a = new LPIP_ADAPTER_PREFIX();
            a.Handle = operand;
            return a;
        }

        public static implicit operator IntPtr(LPIP_ADAPTER_PREFIX operand)
        {
            return operand.Handle;
        }

        public static implicit operator LPIP_ADAPTER_PREFIX(MemPtr operand)
        {
            var a = new LPIP_ADAPTER_PREFIX();
            a.Handle = operand;
            return a;
        }

        public static implicit operator MemPtr(LPIP_ADAPTER_PREFIX operand)
        {
            return operand.Handle;
        }

        public static implicit operator LPIP_ADAPTER_PREFIX(IP_ADAPTER_PREFIX operand)
        {
            var a = new LPIP_ADAPTER_PREFIX();
            a.Handle.Alloc(Marshal.SizeOf(operand));
            Marshal.StructureToPtr(operand, a.Handle.Handle, true);
            return a;
        }

        public static implicit operator IP_ADAPTER_PREFIX(LPIP_ADAPTER_PREFIX operand)
        {
            return operand.ToAddress();
        }
    }
}
