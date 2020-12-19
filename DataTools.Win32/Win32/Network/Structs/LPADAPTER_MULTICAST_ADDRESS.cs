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

using DataTools.Win32;
using DataTools.Win32.Memory;

namespace DataTools.Win32.Network
{
    [StructLayout(LayoutKind.Sequential)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public struct LPADAPTER_MULTICAST_ADDRESS
    {
        public MemPtr Handle;

        public LPADAPTER_MULTICAST_ADDRESS[] AddressChain
        {
            get
            {
                if (Handle == IntPtr.Zero)
                    return null;
                int c = 0;
                var mx = this;
                var ac = default(LPADAPTER_MULTICAST_ADDRESS[]);
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

        public LPADAPTER_MULTICAST_ADDRESS Next
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

        public ADAPTER_MULTICAST_ADDRESS Struct
        {
            get
            {
                ADAPTER_MULTICAST_ADDRESS StructRet = default;
                if (Handle == IntPtr.Zero)
                    return default;
                StructRet = ToAddress();
                return StructRet;
            }
        }

        public ADAPTER_MULTICAST_ADDRESS ToAddress()
        {
            ADAPTER_MULTICAST_ADDRESS ToAddressRet = default;
            if (Handle == IntPtr.Zero)
                return default;
            ToAddressRet = Handle.ToStruct<ADAPTER_MULTICAST_ADDRESS>();
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
                    return AddressFamily.AfUnspecified;
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

        public static implicit operator LPADAPTER_MULTICAST_ADDRESS(IntPtr operand)
        {
            var a = new LPADAPTER_MULTICAST_ADDRESS();
            a.Handle = operand;
            return a;
        }

        public static implicit operator IntPtr(LPADAPTER_MULTICAST_ADDRESS operand)
        {
            return operand.Handle;
        }

        public static implicit operator LPADAPTER_MULTICAST_ADDRESS(MemPtr operand)
        {
            var a = new LPADAPTER_MULTICAST_ADDRESS();
            a.Handle = operand;
            return a;
        }

        public static implicit operator MemPtr(LPADAPTER_MULTICAST_ADDRESS operand)
        {
            return operand.Handle;
        }

        public static implicit operator LPADAPTER_MULTICAST_ADDRESS(ADAPTER_MULTICAST_ADDRESS operand)
        {
            var a = new LPADAPTER_MULTICAST_ADDRESS();
            a.Handle.Alloc(Marshal.SizeOf(operand));
            Marshal.StructureToPtr(operand, a.Handle.Handle, true);
            return a;
        }

        public static implicit operator ADAPTER_MULTICAST_ADDRESS(LPADAPTER_MULTICAST_ADDRESS operand)
        {
            return operand.ToAddress();
        }
    }
}
