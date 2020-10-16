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
    [StructLayout(LayoutKind.Sequential)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public struct LPADAPTER_UNICAST_ADDRESS
    {
        public MemPtr Handle;

        public LPADAPTER_UNICAST_ADDRESS[] AddressChain
        {
            get
            {
                int c = 0;
                var mx = this;
                var ac = default(LPADAPTER_UNICAST_ADDRESS[]);
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

        public LPADAPTER_UNICAST_ADDRESS Next
        {
            get
            {
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
                return Struct.Address.lpSockaddr.IPAddress;
            }
        }

        public ADAPTER_UNICAST_ADDRESS Struct
        {
            get
            {
                ADAPTER_UNICAST_ADDRESS StructRet = default;
                StructRet = ToAddress();
                return StructRet;
            }
        }

        public ADAPTER_UNICAST_ADDRESS ToAddress()
        {
            ADAPTER_UNICAST_ADDRESS ToAddressRet = default;
            if (Handle == IntPtr.Zero)
                return default;
            ToAddressRet = Handle.ToStruct<ADAPTER_UNICAST_ADDRESS>();
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
                return Struct.Address.lpSockaddr.AddressFamily;
            }
        }

        public byte[] Data
        {
            get
            {
                return Struct.Address.lpSockaddr.Data;
            }
        }

        public static implicit operator LPADAPTER_UNICAST_ADDRESS(IntPtr operand)
        {
            var a = new LPADAPTER_UNICAST_ADDRESS();
            a.Handle = operand;
            return a;
        }

        public static implicit operator IntPtr(LPADAPTER_UNICAST_ADDRESS operand)
        {
            return operand.Handle;
        }

        public static implicit operator LPADAPTER_UNICAST_ADDRESS(MemPtr operand)
        {
            var a = new LPADAPTER_UNICAST_ADDRESS();
            a.Handle = operand;
            return a;
        }

        public static implicit operator MemPtr(LPADAPTER_UNICAST_ADDRESS operand)
        {
            return operand.Handle;
        }

        public static implicit operator LPADAPTER_UNICAST_ADDRESS(ADAPTER_UNICAST_ADDRESS operand)
        {
            var a = new LPADAPTER_UNICAST_ADDRESS();
            a.Handle.Alloc(Marshal.SizeOf(operand));
            Marshal.StructureToPtr(operand, a.Handle.Handle, true);
            return a;
        }

        public static implicit operator ADAPTER_UNICAST_ADDRESS(LPADAPTER_UNICAST_ADDRESS operand)
        {
            return operand.ToAddress();
        }
    }
}
